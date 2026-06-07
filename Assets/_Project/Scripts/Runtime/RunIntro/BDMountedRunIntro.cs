using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace BoredomAndDungeons
{
    [DefaultExecutionOrder(-15000)]
    [DisallowMultipleComponent]
    public sealed class BDMountedRunIntro : MonoBehaviour
    {
        private const float ResolveTimeoutSeconds = 4.00f;
        private const float RideDurationSeconds = 2.25f;
        private const float ZoomInDurationSeconds = 0.48f;
        private const float ZoomOutDurationSeconds = 0.72f;
        private const float CinematicZoomFactor = 0.78f;
        private const float EntranceOutsideDistance = 2.35f;
        private const float EntranceInsideDistance = 4.75f;
        private const float PortalWidth = 4.20f;
        private const float PortalHeight = 4.60f;

        private static readonly Vector2Int[] CellOffsets =
        {
            Vector2Int.up,
            Vector2Int.down,
            Vector2Int.right,
            Vector2Int.left
        };

        private static readonly Vector3[] WorldDirections =
        {
            Vector3.forward,
            Vector3.back,
            Vector3.right,
            Vector3.left
        };

        private static BDMountedRunIntro instance;
        private static bool deathRestartPending;

        private Coroutine activeRoutine;
        private BDHorseController activeHorse;
        private CameraState activeCameraState;
        private int activeSceneHandle = -1;

        public static bool IsGameplayInputLocked { get; private set; }

        private sealed class CameraState
        {
            public Camera Camera;
            public Vector3 OffsetFromFocus;
            public Quaternion Rotation;
            public bool Orthographic;
            public float OriginalProjection;
            public readonly List<MonoBehaviour> DisabledDrivers =
                new List<MonoBehaviour>();
        }

        private readonly struct RidePath
        {
            public readonly Vector3 Start;
            public readonly Vector3 End;
            public readonly Vector3 Direction;
            public readonly BDMinimapRoom StartRoom;

            public RidePath(
                Vector3 start,
                Vector3 end,
                Vector3 direction,
                BDMinimapRoom startRoom)
            {
                Start = start;
                End = end;
                Direction = direction;
                StartRoom = startRoom;
            }
        }

        [RuntimeInitializeOnLoadMethod(
            RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ResetStaticState()
        {
            instance = null;
            deathRestartPending = false;
            IsGameplayInputLocked = false;
        }

        [RuntimeInitializeOnLoadMethod(
            RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void BootstrapBeforeSceneLoad()
        {
            if (!Application.isPlaying || instance != null)
                return;

            GameObject root =
                new GameObject("BD_Mounted_Run_Intro");

            DontDestroyOnLoad(root);
            instance = root.AddComponent<BDMountedRunIntro>();
        }

        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
                return;
            }

            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void OnEnable()
        {
            SceneManager.sceneLoaded += HandleSceneLoaded;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= HandleSceneLoaded;
        }

        private void Update()
        {
            if (deathRestartPending ||
                !Application.isPlaying)
            {
                return;
            }

            Transform player =
                BDTargetFinder.FindPlayer();

            if (player == null)
                return;

            BDHealth playerHealth =
                player.GetComponent<BDHealth>();

            if (playerHealth != null &&
                playerHealth.IsDead)
            {
                deathRestartPending = true;
            }
        }

        private void HandleSceneLoaded(
            Scene scene,
            LoadSceneMode mode)
        {
            AbortActiveIntro();

            activeSceneHandle = scene.handle;
            IsGameplayInputLocked = true;
            CleanupTransientFloorArtifacts();

            activeRoutine =
                StartCoroutine(RunIntro(scene));
        }

        private void AbortActiveIntro()
        {
            if (activeRoutine != null)
            {
                StopCoroutine(activeRoutine);
                activeRoutine = null;
            }

            if (activeHorse != null)
                activeHorse.CompleteMountedRunIntro();

            RestoreCamera(activeCameraState);

            activeHorse = null;
            activeCameraState = null;
            activeSceneHandle = -1;
            IsGameplayInputLocked = false;
        }

        private IEnumerator RunIntro(Scene scene)
        {
            Transform player = null;
            BDHorseController horse = null;
            float resolveEndsAt =
                Time.realtimeSinceStartup +
                ResolveTimeoutSeconds;

            while (Time.realtimeSinceStartup < resolveEndsAt)
            {
                if (!IsSceneStillActive(scene))
                    yield break;

                player = BDTargetFinder.FindPlayer();

                if (horse == null)
                {
                    horse =
                        Object.FindFirstObjectByType<
                            BDHorseController>(
                            FindObjectsInactive.Exclude
                        );
                }

                CleanupTransientFloorArtifacts();

                if (player != null && horse != null)
                    break;

                yield return null;
            }

            if (player == null || horse == null)
            {
                IsGameplayInputLocked = false;
                activeRoutine = null;
                yield break;
            }

            BDMinimapRoom[] rooms =
                Object.FindObjectsByType<BDMinimapRoom>(
                    FindObjectsInactive.Exclude,
                    FindObjectsSortMode.None
                );

            if (rooms == null ||
                rooms.Length == 0)
            {
                IsGameplayInputLocked = false;
                activeRoutine = null;
                activeSceneHandle = -1;
                yield break;
            }

            RidePath path =
                ResolveRidePath(
                    player,
                    horse,
                    rooms
                );

            EnsureDoorwayPortals(
                path,
                rooms
            );

            if (deathRestartPending)
            {
                deathRestartPending = false;

                yield return RunDeathRestartAtSpawn(
                    scene,
                    player
                );

                yield break;
            }

            activeHorse = horse;

            if (!horse.BeginMountedRunIntro(
                    player,
                    path.Start,
                    path.Direction))
            {
                IsGameplayInputLocked = false;
                activeHorse = null;
                activeRoutine = null;
                yield break;
            }

            activeCameraState =
                CaptureCamera(
                    horse.transform.position,
                    player.position
                );

            float rideStartedAt =
                Time.realtimeSinceStartup;

            while (true)
            {
                if (!IsSceneStillActive(scene) ||
                    horse == null ||
                    player == null)
                {
                    AbortActiveIntro();
                    yield break;
                }

                float elapsed =
                    Time.realtimeSinceStartup -
                    rideStartedAt;

                float rideT =
                    Mathf.Clamp01(
                        elapsed /
                        Mathf.Max(
                            0.01f,
                            RideDurationSeconds
                        )
                    );

                float easedRideT =
                    Mathf.SmoothStep(
                        0f,
                        1f,
                        rideT
                    );

                Vector3 targetPosition =
                    Vector3.Lerp(
                        path.Start,
                        path.End,
                        easedRideT
                    );

                Vector3 motion =
                    targetPosition -
                    horse.transform.position;

                horse.MoveByExternalControl(
                    motion,
                    path.Direction,
                    1.35f
                );

                float zoomT =
                    Mathf.Clamp01(
                        elapsed /
                        Mathf.Max(
                            0.01f,
                            ZoomInDurationSeconds
                        )
                    );

                TrackCamera(
                    activeCameraState,
                    horse.transform.position,
                    player.position,
                    Mathf.SmoothStep(
                        0f,
                        1f,
                        zoomT
                    )
                );

                ClearTransientCombatInput(player);
                CleanupTransientFloorArtifacts();

                if (rideT >= 1f)
                    break;

                yield return null;
            }

            float zoomOutStartedAt =
                Time.realtimeSinceStartup;

            while (true)
            {
                if (!IsSceneStillActive(scene) ||
                    horse == null ||
                    player == null)
                {
                    AbortActiveIntro();
                    yield break;
                }

                float zoomOutT =
                    Mathf.Clamp01(
                        (
                            Time.realtimeSinceStartup -
                            zoomOutStartedAt
                        ) /
                        Mathf.Max(
                            0.01f,
                            ZoomOutDurationSeconds
                        )
                    );

                TrackCamera(
                    activeCameraState,
                    horse.transform.position,
                    player.position,
                    1f -
                    Mathf.SmoothStep(
                        0f,
                        1f,
                        zoomOutT
                    )
                );

                ClearTransientCombatInput(player);
                CleanupTransientFloorArtifacts();

                if (zoomOutT >= 1f)
                    break;

                yield return null;
            }

            while (IsAnyGameplayInputHeld())
            {
                if (!IsSceneStillActive(scene) ||
                    horse == null ||
                    player == null)
                {
                    AbortActiveIntro();
                    yield break;
                }

                TrackCamera(
                    activeCameraState,
                    horse.transform.position,
                    player.position,
                    0f
                );

                ClearTransientCombatInput(player);
                CleanupTransientFloorArtifacts();
                yield return null;
            }

            ClearTransientCombatInput(player);
            CleanupTransientFloorArtifacts();

            horse.CompleteMountedRunIntro();
            RestoreCamera(activeCameraState);

            activeHorse = null;
            activeCameraState = null;
            activeRoutine = null;
            activeSceneHandle = -1;
            IsGameplayInputLocked = false;
        }

        private IEnumerator RunDeathRestartAtSpawn(
            Scene scene,
            Transform player)
        {
            activeHorse = null;
            activeCameraState = null;

            ClearTransientCombatInput(player);
            CleanupTransientFloorArtifacts();

            while (IsAnyGameplayInputHeld())
            {
                if (!IsSceneStillActive(scene) ||
                    player == null)
                {
                    activeRoutine = null;
                    activeSceneHandle = -1;
                    IsGameplayInputLocked = false;
                    yield break;
                }

                ClearTransientCombatInput(player);
                CleanupTransientFloorArtifacts();
                yield return null;
            }

            ClearTransientCombatInput(player);
            CleanupTransientFloorArtifacts();

            activeRoutine = null;
            activeSceneHandle = -1;
            IsGameplayInputLocked = false;
        }

        private bool IsSceneStillActive(Scene scene)
        {
            return scene.IsValid() &&
                   scene.isLoaded &&
                   activeSceneHandle == scene.handle;
        }

        private static RidePath ResolveRidePath(
            Transform player,
            BDHorseController horse,
            BDMinimapRoom[] rooms)
        {
            BDMinimapRoom startRoom =
                ResolveStartRoom(
                    player.position,
                    rooms
                );

            if (startRoom == null)
            {
                Vector3 fallbackDirection =
                    player.forward;

                fallbackDirection.y = 0f;

                if (fallbackDirection.sqrMagnitude < 0.001f)
                    fallbackDirection = Vector3.forward;

                fallbackDirection.Normalize();

                Vector3 fallbackEnd =
                    player.position;

                Vector3 fallbackStart =
                    fallbackEnd -
                    fallbackDirection *
                    5.75f;

                fallbackStart.y =
                    ResolveGroundY(
                        fallbackStart,
                        horse.transform.position.y
                    );
                fallbackEnd.y =
                    ResolveGroundY(
                        fallbackEnd,
                        horse.transform.position.y
                    );

                return new RidePath(
                    fallbackStart,
                    fallbackEnd,
                    fallbackDirection,
                    null
                );
            }

            Vector3 entranceDirection =
                ResolveEntranceDirection(
                    startRoom,
                    rooms,
                    player.position
                );

            float halfRoom =
                Mathf.Max(
                    4f,
                    startRoom.RoomSize * 0.5f
                );

            Vector3 doorwayCenter =
                startRoom.WorldCenter +
                entranceDirection *
                halfRoom;

            Vector3 start =
                doorwayCenter +
                entranceDirection *
                EntranceOutsideDistance;

            Vector3 end =
                doorwayCenter -
                entranceDirection *
                EntranceInsideDistance;

            float fallbackY =
                horse.transform.position.y;

            start.y =
                ResolveGroundY(
                    start,
                    fallbackY
                );
            end.y =
                ResolveGroundY(
                    end,
                    fallbackY
                );

            if (BDHazardVolume.IsRecoveryPointUnsafe(
                    end,
                    1.15f))
            {
                end =
                    startRoom.WorldCenter -
                    entranceDirection *
                    1.25f;

                end.y =
                    ResolveGroundY(
                        end,
                        fallbackY
                    );
            }

            return new RidePath(
                start,
                end,
                -entranceDirection,
                startRoom
            );
        }

        private static BDMinimapRoom ResolveStartRoom(
            Vector3 playerPosition,
            BDMinimapRoom[] rooms)
        {
            if (rooms == null || rooms.Length == 0)
                return null;

            BDMinimapRoom nearest = null;
            float nearestDistance = float.PositiveInfinity;

            for (int index = 0;
                 index < rooms.Length;
                 index++)
            {
                BDMinimapRoom room = rooms[index];

                if (room == null)
                    continue;

                if (room.ContainsWorldPosition(
                        playerPosition,
                        0.75f))
                {
                    return room;
                }

                float distance =
                    room.SqrDistanceToCenter(
                        playerPosition
                    );

                if (distance >= nearestDistance)
                    continue;

                nearestDistance = distance;
                nearest = room;
            }

            return nearest;
        }

        private static Vector3 ResolveEntranceDirection(
            BDMinimapRoom room,
            BDMinimapRoom[] rooms,
            Vector3 playerPosition)
        {
            Dictionary<Vector2Int, BDMinimapRoom> byCell =
                BuildRoomLookup(rooms);

            Vector3 fromCenter =
                playerPosition -
                room.WorldCenter;

            fromCenter.y = 0f;

            float bestScore = float.NegativeInfinity;
            Vector3 bestDirection = Vector3.zero;

            for (int index = 0;
                 index < WorldDirections.Length;
                 index++)
            {
                if (!IsRoomSideOpen(room, index))
                    continue;

                bool external =
                    !byCell.ContainsKey(
                        room.Cell +
                        CellOffsets[index]
                    );

                float score =
                    Vector3.Dot(
                        fromCenter,
                        WorldDirections[index]
                    );

                if (external)
                    score += 1000f;

                if (score <= bestScore)
                    continue;

                bestScore = score;
                bestDirection =
                    WorldDirections[index];
            }

            if (bestDirection.sqrMagnitude > 0.001f)
                return bestDirection.normalized;

            if (Mathf.Abs(fromCenter.x) >=
                Mathf.Abs(fromCenter.z))
            {
                return fromCenter.x >= 0f
                    ? Vector3.right
                    : Vector3.left;
            }

            return fromCenter.z >= 0f
                ? Vector3.forward
                : Vector3.back;
        }

        private static void EnsureDoorwayPortals(
            RidePath path,
            BDMinimapRoom[] rooms)
        {
            if (path.StartRoom != null)
            {
                float halfRoom =
                    Mathf.Max(
                        4f,
                        path.StartRoom.RoomSize * 0.5f
                    );

                Vector3 entranceOutward =
                    -path.Direction;

                Vector3 entrancePosition =
                    path.StartRoom.WorldCenter +
                    entranceOutward *
                    halfRoom;

                entrancePosition.y =
                    ResolveGroundY(
                        entrancePosition,
                        path.End.y
                    );

                BDDoorwayPortalVisual.EnsurePortal(
                    "BD_Entrance_Portal",
                    entrancePosition,
                    entranceOutward,
                    PortalWidth,
                    PortalHeight,
                    false
                );
            }

            if (!TryResolveExitDoor(
                    path.StartRoom,
                    rooms,
                    out Vector3 exitPosition,
                    out Vector3 exitOutward))
            {
                return;
            }

            exitPosition.y =
                ResolveGroundY(
                    exitPosition,
                    path.End.y
                );

            BDDoorwayPortalVisual.EnsurePortal(
                "BD_Exit_Portal",
                exitPosition,
                exitOutward,
                PortalWidth,
                PortalHeight,
                true
            );
        }

        private static bool TryResolveExitDoor(
            BDMinimapRoom startRoom,
            BDMinimapRoom[] rooms,
            out Vector3 position,
            out Vector3 outward)
        {
            position = Vector3.zero;
            outward = Vector3.forward;

            if (rooms == null || rooms.Length == 0)
                return false;

            Dictionary<Vector2Int, BDMinimapRoom> byCell =
                BuildRoomLookup(rooms);

            float bestScore = float.NegativeInfinity;
            bool found = false;

            for (int roomIndex = 0;
                 roomIndex < rooms.Length;
                 roomIndex++)
            {
                BDMinimapRoom room =
                    rooms[roomIndex];

                if (room == null)
                    continue;

                float roomDistance =
                    startRoom != null
                        ? room.SqrDistanceToCenter(
                            startRoom.WorldCenter
                        )
                        : room.WorldCenter.sqrMagnitude;

                for (int sideIndex = 0;
                     sideIndex < WorldDirections.Length;
                     sideIndex++)
                {
                    if (!IsRoomSideOpen(
                            room,
                            sideIndex))
                    {
                        continue;
                    }

                    bool external =
                        !byCell.ContainsKey(
                            room.Cell +
                            CellOffsets[sideIndex]
                        );

                    if (!external)
                        continue;

                    float score =
                        roomDistance;

                    if (startRoom == room)
                        score -= 100000f;

                    if (score <= bestScore)
                        continue;

                    bestScore = score;
                    outward =
                        WorldDirections[sideIndex];

                    position =
                        room.WorldCenter +
                        outward *
                        Mathf.Max(
                            4f,
                            room.RoomSize * 0.5f
                        );

                    found = true;
                }
            }

            return found;
        }

        private static Dictionary<Vector2Int, BDMinimapRoom>
            BuildRoomLookup(BDMinimapRoom[] rooms)
        {
            Dictionary<Vector2Int, BDMinimapRoom> result =
                new Dictionary<Vector2Int, BDMinimapRoom>();

            if (rooms == null)
                return result;

            for (int index = 0;
                 index < rooms.Length;
                 index++)
            {
                BDMinimapRoom room =
                    rooms[index];

                if (room == null)
                    continue;

                result[room.Cell] = room;
            }

            return result;
        }

        private static bool IsRoomSideOpen(
            BDMinimapRoom room,
            int sideIndex)
        {
            switch (sideIndex)
            {
                case 0:
                    return room.NorthOpen;
                case 1:
                    return room.SouthOpen;
                case 2:
                    return room.EastOpen;
                case 3:
                    return room.WestOpen;
                default:
                    return false;
            }
        }

        private static float ResolveGroundY(
            Vector3 position,
            float fallbackY)
        {
            Ray ray =
                new Ray(
                    position + Vector3.up * 8f,
                    Vector3.down
                );

            if (Physics.Raycast(
                    ray,
                    out RaycastHit hit,
                    20f,
                    ~0,
                    QueryTriggerInteraction.Ignore))
            {
                return hit.point.y;
            }

            return fallbackY;
        }

        private static CameraState CaptureCamera(
            Vector3 horsePosition,
            Vector3 playerPosition)
        {
            Camera camera =
                Camera.main;

            if (camera == null)
            {
                camera =
                    Object.FindFirstObjectByType<Camera>(
                        FindObjectsInactive.Exclude
                    );
            }

            if (camera == null)
                return null;

            Vector3 focus =
                ResolveCameraFocus(
                    horsePosition,
                    playerPosition
                );

            CameraState state =
                new CameraState
                {
                    Camera = camera,
                    OffsetFromFocus =
                        camera.transform.position -
                        focus,
                    Rotation =
                        camera.transform.rotation,
                    Orthographic =
                        camera.orthographic,
                    OriginalProjection =
                        camera.orthographic
                            ? camera.orthographicSize
                            : camera.fieldOfView
                };

            DisableCameraDrivers(
                camera.transform,
                state.DisabledDrivers
            );

            return state;
        }

        private static void DisableCameraDrivers(
            Transform cameraTransform,
            List<MonoBehaviour> disabled)
        {
            Transform cursor =
                cameraTransform;

            for (int depth = 0;
                 depth < 4 && cursor != null;
                 depth++)
            {
                MonoBehaviour[] behaviours =
                    cursor.GetComponents<
                        MonoBehaviour>();

                for (int index = 0;
                     index < behaviours.Length;
                     index++)
                {
                    MonoBehaviour behaviour =
                        behaviours[index];

                    if (behaviour == null ||
                        !behaviour.enabled)
                    {
                        continue;
                    }

                    string typeName =
                        behaviour.GetType().Name;

                    string normalized =
                        typeName.ToLowerInvariant();

                    bool looksLikeDriver =
                        normalized.Contains("camera") ||
                        normalized.Contains("follow") ||
                        normalized.Contains("orbit") ||
                        normalized.Contains("rig");

                    if (!looksLikeDriver)
                        continue;

                    behaviour.enabled = false;
                    disabled.Add(behaviour);
                }

                cursor = cursor.parent;
            }
        }

        private static void TrackCamera(
            CameraState state,
            Vector3 horsePosition,
            Vector3 playerPosition,
            float zoomAmount)
        {
            if (state == null ||
                state.Camera == null)
            {
                return;
            }

            Camera camera =
                state.Camera;

            Vector3 focus =
                ResolveCameraFocus(
                    horsePosition,
                    playerPosition
                );

            camera.transform.position =
                focus +
                state.OffsetFromFocus;

            camera.transform.rotation =
                state.Rotation;

            float clampedZoom =
                Mathf.Clamp01(
                    zoomAmount
                );

            if (state.Orthographic)
            {
                camera.orthographicSize =
                    Mathf.Lerp(
                        state.OriginalProjection,
                        state.OriginalProjection *
                        CinematicZoomFactor,
                        clampedZoom
                    );
            }
            else
            {
                camera.fieldOfView =
                    Mathf.Lerp(
                        state.OriginalProjection,
                        Mathf.Max(
                            24f,
                            state.OriginalProjection *
                            CinematicZoomFactor
                        ),
                        clampedZoom
                    );
            }
        }

        private static Vector3 ResolveCameraFocus(
            Vector3 horsePosition,
            Vector3 playerPosition)
        {
            Vector3 focus =
                Vector3.Lerp(
                    horsePosition,
                    playerPosition,
                    0.35f
                );

            focus.y += 1.15f;
            return focus;
        }

        private static void RestoreCamera(
            CameraState state)
        {
            if (state == null)
                return;

            if (state.Camera != null)
            {
                if (state.Orthographic)
                {
                    state.Camera.orthographicSize =
                        state.OriginalProjection;
                }
                else
                {
                    state.Camera.fieldOfView =
                        state.OriginalProjection;
                }
            }

            for (int index = 0;
                 index < state.DisabledDrivers.Count;
                 index++)
            {
                MonoBehaviour behaviour =
                    state.DisabledDrivers[index];

                if (behaviour != null)
                    behaviour.enabled = true;
            }

            state.DisabledDrivers.Clear();
        }

        private static void ClearTransientCombatInput(
            Transform player)
        {
            if (player == null)
                return;

            BDPlayerCombat combat =
                player.GetComponent<
                    BDPlayerCombat>();

            if (combat != null)
                combat.ResetTransientCombatInputState();
        }

        private static void CleanupTransientFloorArtifacts()
        {
            BDEnemyAttackTelegraphVisual[] telegraphs =
                Object.FindObjectsByType<
                    BDEnemyAttackTelegraphVisual>(
                    FindObjectsInactive.Include,
                    FindObjectsSortMode.None
                );

            for (int index = 0;
                 index < telegraphs.Length;
                 index++)
            {
                BDEnemyAttackTelegraphVisual visual =
                    telegraphs[index];

                if (visual != null)
                    Destroy(visual.gameObject);
            }

            string[] exactTransientNames =
            {
                "BD_Enemy_Ranged_Telegraph",
                "BD_Enemy_Melee_Telegraph",
                "BD_Landing_Attack_Visual",
                "BD_Landing_Attack_Impact",
                "BD_Player_Landing_Telegraph"
            };

            for (int index = 0;
                 index < exactTransientNames.Length;
                 index++)
            {
                GameObject transient =
                    GameObject.Find(
                        exactTransientNames[index]
                    );

                if (transient != null)
                    Destroy(transient);
            }
        }

        private static bool IsAnyGameplayInputHeld()
        {
#if ENABLE_INPUT_SYSTEM
            Mouse mouse = Mouse.current;

            if (mouse != null &&
                (
                    mouse.leftButton.isPressed ||
                    mouse.rightButton.isPressed ||
                    mouse.middleButton.isPressed
                ))
            {
                return true;
            }

            Keyboard keyboard =
                Keyboard.current;

            if (keyboard != null &&
                (
                    keyboard.wKey.isPressed ||
                    keyboard.aKey.isPressed ||
                    keyboard.sKey.isPressed ||
                    keyboard.dKey.isPressed ||
                    keyboard.upArrowKey.isPressed ||
                    keyboard.downArrowKey.isPressed ||
                    keyboard.leftArrowKey.isPressed ||
                    keyboard.rightArrowKey.isPressed ||
                    keyboard.spaceKey.isPressed ||
                    keyboard.leftShiftKey.isPressed ||
                    keyboard.rightShiftKey.isPressed ||
                    keyboard.eKey.isPressed ||
                    keyboard.fKey.isPressed ||
                    keyboard.jKey.isPressed ||
                    keyboard.kKey.isPressed ||
                    keyboard.rKey.isPressed ||
                    keyboard.tabKey.isPressed
                ))
            {
                return true;
            }

            Gamepad gamepad =
                Gamepad.current;

            if (gamepad != null &&
                (
                    gamepad.leftStick.ReadValue()
                        .sqrMagnitude > 0.02f ||
                    gamepad.rightStick.ReadValue()
                        .sqrMagnitude > 0.02f ||
                    gamepad.buttonSouth.isPressed ||
                    gamepad.buttonEast.isPressed ||
                    gamepad.buttonWest.isPressed ||
                    gamepad.buttonNorth.isPressed ||
                    gamepad.leftShoulder.isPressed ||
                    gamepad.rightShoulder.isPressed ||
                    gamepad.leftTrigger.ReadValue() > 0.15f ||
                    gamepad.rightTrigger.ReadValue() > 0.15f
                ))
            {
                return true;
            }

            Touchscreen touchscreen =
                Touchscreen.current;

            if (touchscreen != null &&
                touchscreen.primaryTouch.press.isPressed)
            {
                return true;
            }
#endif

#if ENABLE_LEGACY_INPUT_MANAGER
            if (Input.GetMouseButton(0) ||
                Input.GetMouseButton(1) ||
                Input.GetMouseButton(2) ||
                Input.GetKey(KeyCode.W) ||
                Input.GetKey(KeyCode.A) ||
                Input.GetKey(KeyCode.S) ||
                Input.GetKey(KeyCode.D) ||
                Input.GetKey(KeyCode.UpArrow) ||
                Input.GetKey(KeyCode.DownArrow) ||
                Input.GetKey(KeyCode.LeftArrow) ||
                Input.GetKey(KeyCode.RightArrow) ||
                Input.GetKey(KeyCode.Space) ||
                Input.GetKey(KeyCode.LeftShift) ||
                Input.GetKey(KeyCode.RightShift) ||
                Input.GetKey(KeyCode.E) ||
                Input.GetKey(KeyCode.F) ||
                Input.GetKey(KeyCode.J) ||
                Input.GetKey(KeyCode.K) ||
                Input.GetKey(KeyCode.R) ||
                Input.GetKey(KeyCode.Tab))
            {
                return true;
            }
#endif

            return false;
        }
    }
}
