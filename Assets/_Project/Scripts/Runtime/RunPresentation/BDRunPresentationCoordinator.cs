using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BoredomAndDungeons
{
    [DefaultExecutionOrder(-32000)]
    [DisallowMultipleComponent]
    public sealed class BDRunPresentationCoordinator : MonoBehaviour
    {
        // BD AUTHORED ENTRY SINGLE-OWNER + PORTAL SELF-HEAL V14
        // BD EXACT ROOM CAMERA + DOORWAY GEOMETRY V18
        // BD V14 COORDINATOR RESTORE + INSIDE CAMERA V17B
        // BD V20 CINEMATIC INPUT TURN STOP + AUDIO LISTENER
        private const string RootName = "BD_RunPresentationCoordinator";
        private const string EntranceEffectName = "BD_AuthoredEntrance_PortalEffectOnly";
        private const string ExitEffectName = "BD_AuthoredExit_PortalEffectOnly";
        private const string ExitApproachName = "BD_Exit_Cinematic_ApproachTrigger";
        private const string EntranceBlockerName = "BD_AuthoredEntrance_ReturnBlocker_Invisible";
        // BD AUTHORED ENTRANCE MOUTH ONLY V10

        private static BDRunPresentationCoordinator instance;
        private static bool introPlayedThisSession;
        private static bool forceNextIntro;
        private static bool cinematicSeenSinceLastMenu;

        private float coverAlpha = 1f;
        private float coverTarget = 1f;
        private float coverSpeed = 8f;
        private bool inputLocked = true;
        private bool awaitingRunStart = true;
        private bool mainMenuWasVisible;
        private Coroutine activeSequence;
        private float nextPortalRepairAt;
        private float nextAudioListenerRepairAt;

        // BD MOUNTED INTRO RIDER FIRST-VISIBLE-FRAME V23R19O
        private readonly List<Renderer> mountedIntroRiderRenderers =
            new List<Renderer>(16);
        private readonly List<SkinnedMeshRenderer>
            mountedIntroRiderSkinnedRenderers =
                new List<SkinnedMeshRenderer>(8);
        private readonly List<bool>
            mountedIntroRiderOriginalOffscreenPolicy =
                new List<bool>(8);
        private Transform mountedIntroRiderRoot;

        // BD FIRST-FRAME CINEMATIC CAMERA PRIME V23R5
        private bool mountedEntranceCameraPrimed;
        private Camera primedCinematicCamera;
        private BDCameraFollow primedCameraFollow;
        private bool primedCameraFollowWasEnabled;
        private float primedOriginalFov;
        private float primedOriginalOrtho;

        public static bool InputLocked => instance != null && instance.inputLocked;
        public static bool HoldGameplayControlOnRunStart =>
            instance != null && (instance.awaitingRunStart || instance.inputLocked);
        public static bool IsTransitionActive => instance != null && instance.activeSequence != null;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ResetStaticState()
        {
            instance = null;
            introPlayedThisSession = false;
            forceNextIntro = false;
            cinematicSeenSinceLastMenu = false;
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Bootstrap()
        {
            if (instance != null)
                return;

            GameObject root = new GameObject(RootName);
            DontDestroyOnLoad(root);
            instance = root.AddComponent<BDRunPresentationCoordinator>();
        }

        public static void MarkNextRunAsFreshOrVictoryIntro()
        {
            forceNextIntro = true;
            if (instance != null)
                instance.awaitingRunStart = true;
        }

        public static void MarkCinematicSeen()
        {
            cinematicSeenSinceLastMenu = true;
            MarkNextRunAsFreshOrVictoryIntro();
        }

        public static void MarkDeathRestartWithoutIntro()
        {
            forceNextIntro = false;
            introPlayedThisSession = true;
            if (instance != null)
                instance.awaitingRunStart = true;
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
            SceneManager.sceneLoaded += HandleSceneLoaded;
        }

        private void OnDestroy()
        {
            if (instance != this)
                return;

            SceneManager.sceneLoaded -= HandleSceneLoaded;
            instance = null;
        }

        private void HandleSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            // BD FRESH-SCENE PLAYER CACHE FOR MOUNTED INTRO V23R19E
            BDTargetFinder.ClearCache();
            ClearMountedIntroRiderPresentationBaseline();
            CaptureMountedIntroRiderPresentationBaseline(
                ResolveCurrentPlayerTransform()
            );
            EnsureExactlyOneActiveAudioListener(Camera.main);
            coverAlpha = 1f;
            coverTarget = 1f;
            inputLocked = true;
            awaitingRunStart = true;
            mainMenuWasVisible = false;

            if (activeSequence != null)
                StopCoroutine(activeSequence);

            ClearPrimedMountedEntranceCamera(false);
            if (!introPlayedThisSession || forceNextIntro)
                PrimeMountedEntranceCameraBeforeRender();

            activeSequence = StartCoroutine(PrepareLoadedScene());
        }

        // BD FIRST-FRAME CINEMATIC CAMERA PRIME V23R5
        // sceneLoaded runs before Start. Take cinematic camera ownership here so
        // BDCameraFollow cannot render one entrance-close frame before the
        // mounted introduction establishes its approved inside-room shot.
        private bool PrimeMountedEntranceCameraBeforeRender()
        {
            Transform entrance = FindAuthoredDoor(true);
            Transform horse = ResolveCurrentHorseTransform();
            BDHorseController horseController =
                horse != null
                    ? horse.GetComponent<BDHorseController>()
                    : null;
            Transform player = ResolveCurrentPlayerTransform(horseController);
            Camera camera = Camera.main;

            if (entrance == null ||
                player == null ||
                horse == null ||
                camera == null)
            {
                return false;
            }

            Vector3 inwardDirection = player.position - entrance.position;
            inwardDirection.y = 0f;
            if (inwardDirection.sqrMagnitude < 0.01f)
                inwardDirection = ResolveEntranceInwardDirection(entrance);
            inwardDirection.Normalize();

            ResolveRoomEntranceCameraPose(
                entrance,
                inwardDirection,
                out Vector3 cameraPosition,
                out Vector3 lookPoint,
                out _
            );
            Quaternion cameraRotation = Quaternion.LookRotation(
                lookPoint - cameraPosition,
                Vector3.up
            );

            primedCinematicCamera = camera;
            primedCameraFollow = camera.GetComponent<BDCameraFollow>();
            primedCameraFollowWasEnabled =
                primedCameraFollow != null && primedCameraFollow.enabled;
            primedOriginalFov = camera.fieldOfView;
            primedOriginalOrtho = camera.orthographicSize;

            if (primedCameraFollow != null)
                primedCameraFollow.enabled = false;

            HoldCinematicCamera(
                camera,
                cameraPosition,
                cameraRotation,
                primedOriginalFov,
                primedOriginalOrtho
            );
            EnsureExactlyOneActiveAudioListener(camera);
            mountedEntranceCameraPrimed = true;
            return true;
        }

        private void ClearPrimedMountedEntranceCamera(bool restoreFollow)
        {
            if (restoreFollow &&
                primedCameraFollow != null &&
                primedCameraFollowWasEnabled)
            {
                primedCameraFollow.enabled = true;
            }

            mountedEntranceCameraPrimed = false;
            primedCinematicCamera = null;
            primedCameraFollow = null;
            primedCameraFollowWasEnabled = false;
            primedOriginalFov = 0f;
            primedOriginalOrtho = 0f;
        }

        private IEnumerator PrepareLoadedScene()
        {
            // Deliberately no initial frame yield: the scene is covered and the
            // mounted camera is primed before any gameplay camera can render.
            DestroyObsoleteGeneratedPortals();
            Transform entrance = FindAuthoredDoor(true);
            Transform exit = FindAuthoredDoor(false);
            EnsureAuthoredPortalEffects();
            BDEntranceReturnBlocker entranceBlocker =
                AttachEntranceReturnBlocker(entrance);
            if (entranceBlocker != null)
                entranceBlocker.SetBlocking(false);

            float deadline = Time.realtimeSinceStartup + 3f;
            while (Time.realtimeSinceStartup < deadline &&
                   !IsMainMenuVisible() &&
                   !IsGameplayRunActive())
            {
                yield return null;
            }

            if (IsMainMenuVisible())
            {
                mainMenuWasVisible = true;
                inputLocked = false;
                yield return FadeCoverTo(0f, 7f);

                while (IsMainMenuVisible())
                    yield return null;

                inputLocked = true;
                yield return FadeCoverTo(1f, 12f);
            }

            bool shouldPlayIntro = !introPlayedThisSession || forceNextIntro;
            if (shouldPlayIntro)
            {
                forceNextIntro = false;
                yield return PlayMountedEntrance(entrance);
                introPlayedThisSession = true;
                SetEntranceReturnBlocking(true);
            }
            else
            {
                // Ordinary death -> New Game: no mounted doorway replay.
                ClearPrimedMountedEntranceCamera(true);
                yield return FadeCoverTo(0f, 9f);
                awaitingRunStart = false;
                inputLocked = false;
                ReleaseOnFootGameplayControl();
                SetEntranceReturnBlocking(true);
            }

            activeSequence = null;
        }

        private void Update()
        {
            // BD AUTHORED PORTAL EFFECT SELF-HEAL V14
            // Enter Play Mode/domain reload timing can otherwise create the
            // coordinator after the sceneLoaded callback. Re-assert only the
            // two effect-only surfaces; no doorway geometry or trigger is made.
            if (Time.unscaledTime >= nextPortalRepairAt)
            {
                nextPortalRepairAt = Time.unscaledTime + 1f;
                EnsureAuthoredPortalEffects();
            }

            if (Time.unscaledTime >= nextAudioListenerRepairAt)
            {
                nextAudioListenerRepairAt = Time.unscaledTime + 0.50f;
                EnsureExactlyOneActiveAudioListener(Camera.main);
            }

            coverAlpha = Mathf.MoveTowards(
                coverAlpha,
                coverTarget,
                Mathf.Max(0.01f, coverSpeed) * Time.unscaledDeltaTime);

            bool menuVisible = IsMainMenuVisible();
            if (menuVisible && !mainMenuWasVisible)
            {
                mainMenuWasVisible = true;
                awaitingRunStart = true;
                inputLocked = false;

                if (cinematicSeenSinceLastMenu)
                {
                    forceNextIntro = true;
                    cinematicSeenSinceLastMenu = false;
                }
            }
            else if (!menuVisible)
            {
                mainMenuWasVisible = false;
            }
        }

        private void OnGUI()
        {
            if (coverAlpha <= 0.001f)
                return;

            int oldDepth = GUI.depth;
            Color oldColor = GUI.color;
            // The BBH boot intro uses -10000 and remains visible above this cover.
            // The menu and gameplay remain below it until the cover is released.
            GUI.depth = -9000;
            GUI.color = new Color(0.012f, 0.014f, 0.020f, Mathf.Clamp01(coverAlpha));
            GUI.DrawTexture(new Rect(0f, 0f, Screen.width, Screen.height), Texture2D.whiteTexture);
            GUI.color = oldColor;
            GUI.depth = oldDepth;
        }


        private IEnumerator PlayMountedEntrance(Transform entrance)
        {
            Transform horse = ResolveCurrentHorseTransform();
            BDHorseController horseController =
                horse != null
                    ? horse.GetComponent<BDHorseController>()
                    : null;
            Transform player = ResolveCurrentPlayerTransform(horseController);

            if (player == null ||
                horse == null ||
                entrance == null ||
                horseController == null)
            {
                ClearPrimedMountedEntranceCamera(true);
                yield return FadeCoverTo(0f, 8f);
                awaitingRunStart = false;
                inputLocked = false;
                ReleaseOnFootGameplayControl();
                yield break;
            }

            CaptureMountedIntroRiderPresentationBaseline(player);
            ForceMountedIntroRiderPresentationVisible(player);

            Vector3 straightEnd = player.position;
            float horseGroundOffset = ResolveHorseGroundOffset(horse);
            Vector3 inwardDirection = straightEnd - entrance.position;
            inwardDirection.y = 0f;
            if (inwardDirection.sqrMagnitude < 0.01f)
                inwardDirection = ResolveEntranceInwardDirection(entrance);
            inwardDirection.Normalize();

            Vector3 start = ResolveAuthoredEntranceMouthStart(
                entrance,
                inwardDirection,
                horseGroundOffset
            );
            EnsureEntranceApproachGround(
                entrance,
                start,
                inwardDirection
            );

            // BD AUTHORITATIVE MOUNTED INTRO BINDING V23R19E
            // BeginMountedRunIntro resets stale horse state, places the exact
            // active-scene rider immediately, and owns the mount state before
            // any cinematic frame moves the horse.
            if (!horseController.BeginMountedRunIntro(
                    player,
                    start,
                    inwardDirection) ||
                horseController.Rider == null)
            {
                ClearPrimedMountedEntranceCamera(true);
                yield return FadeCoverTo(0f, 8f);
                awaitingRunStart = false;
                inputLocked = false;
                ReleaseOnFootGameplayControl();
                yield break;
            }

            player = horseController.Rider;
            horseController.SnapCinematicRiderToMountPoint();
            Physics.SyncTransforms();

            List<ComponentState> states =
                CaptureAndDisableControls(player, horse);
            Camera camera = Camera.main;
            EnsureExactlyOneActiveAudioListener(camera);
            bool usePrimedCamera =
                mountedEntranceCameraPrimed &&
                primedCinematicCamera == camera;
            BDCameraFollow cameraFollow = usePrimedCamera
                ? primedCameraFollow
                : camera != null
                    ? camera.GetComponent<BDCameraFollow>()
                    : null;
            bool restoreCameraFollow = usePrimedCamera
                ? primedCameraFollowWasEnabled
                : cameraFollow != null && cameraFollow.enabled;
            if (!usePrimedCamera && restoreCameraFollow)
                cameraFollow.enabled = false;

            float originalFov = usePrimedCamera
                ? primedOriginalFov
                : camera != null ? camera.fieldOfView : 60f;
            float originalOrtho = usePrimedCamera
                ? primedOriginalOrtho
                : camera != null ? camera.orthographicSize : 5f;

            horse.position = start;
            horse.rotation =
                Quaternion.LookRotation(inwardDirection, Vector3.up);
            horseController.SnapCinematicRiderToMountPoint();
            Physics.SyncTransforms();

            ResolveRoomEntranceCameraPose(
                entrance,
                inwardDirection,
                out Vector3 cinematicCameraPosition,
                out Vector3 cinematicLookPoint,
                out BDMinimapRoom entranceRoom
            );
            Quaternion cinematicCameraRotation =
                Quaternion.LookRotation(
                    cinematicLookPoint - cinematicCameraPosition,
                    Vector3.up
                );

            HoldCinematicCamera(
                camera,
                cinematicCameraPosition,
                cinematicCameraRotation,
                originalFov,
                originalOrtho
            );

            // The rider must already be rendered before the first uncovered
            // cinematic frame. Transform binding alone does not update stale
            // skinned bounds after a scene reload/teleport.
            ForceMountedIntroRiderPresentationVisible(player);
            Physics.SyncTransforms();

            yield return FadeCoverTo(0f, 7f);

            const float straightDuration = 2.35f;
            float elapsed = 0f;
            while (elapsed < straightDuration)
            {
                elapsed += Time.unscaledDeltaTime;
                float t = Mathf.Clamp01(elapsed / straightDuration);
                float eased = t * t * (3f - 2f * t);
                horse.position = Vector3.Lerp(start, straightEnd, eased);
                horse.rotation =
                    Quaternion.LookRotation(inwardDirection, Vector3.up);
                horseController.SnapCinematicRiderToMountPoint();
                HoldCinematicCamera(
                    camera,
                    cinematicCameraPosition,
                    cinematicCameraRotation,
                    originalFov,
                    originalOrtho
                );
                Physics.SyncTransforms();
                yield return null;
            }

            Vector3 clearDirection = ResolveClearMountedIntroDirection(
                horse,
                player,
                straightEnd,
                inwardDirection,
                entranceRoom
            );
            Quaternion turnStartRotation =
                Quaternion.LookRotation(inwardDirection, Vector3.up);
            Quaternion turnEndRotation =
                Quaternion.LookRotation(clearDirection, Vector3.up);

            const float turnDuration = 0.78f;
            elapsed = 0f;
            while (elapsed < turnDuration)
            {
                elapsed += Time.unscaledDeltaTime;
                float t = Mathf.Clamp01(elapsed / turnDuration);
                float eased = t * t * (3f - 2f * t);
                horse.position = straightEnd;
                horse.rotation = Quaternion.Slerp(
                    turnStartRotation,
                    turnEndRotation,
                    eased
                );
                horseController.SnapCinematicRiderToMountPoint();
                HoldCinematicCamera(
                    camera,
                    cinematicCameraPosition,
                    cinematicCameraRotation,
                    originalFov,
                    originalOrtho
                );
                Physics.SyncTransforms();
                yield return null;
            }

            horse.position = straightEnd;
            horse.rotation = turnEndRotation;
            horseController.SnapCinematicRiderToMountPoint();
            Physics.SyncTransforms();

            const float fullStopHoldDuration = 0.24f;
            elapsed = 0f;
            while (elapsed < fullStopHoldDuration)
            {
                elapsed += Time.unscaledDeltaTime;
                HoldCinematicCamera(
                    camera,
                    cinematicCameraPosition,
                    cinematicCameraRotation,
                    originalFov,
                    originalOrtho
                );
                horseController.SnapCinematicRiderToMountPoint();
                yield return null;
            }

            const float gameplayDistanceBehind = 15.25f;
            const float gameplayHeight = 17.75f;
            const float gameplayLookAhead = 10.60f;
            Vector3 gameplayCameraPosition =
                horse.position -
                clearDirection * gameplayDistanceBehind +
                Vector3.up * gameplayHeight;
            gameplayCameraPosition = ClampPointInsideRoom(
                entranceRoom,
                gameplayCameraPosition,
                1.65f
            );
            Vector3 gameplayLookPoint =
                horse.position + clearDirection * gameplayLookAhead;
            gameplayLookPoint = ClampPointInsideRoom(
                entranceRoom,
                gameplayLookPoint,
                1.65f
            );
            Quaternion gameplayCameraRotation =
                Quaternion.LookRotation(
                    gameplayLookPoint - gameplayCameraPosition,
                    Vector3.up
                );

            const float cameraReturnDuration = 0.60f;
            elapsed = 0f;
            while (elapsed < cameraReturnDuration)
            {
                elapsed += Time.unscaledDeltaTime;
                float t = Mathf.Clamp01(elapsed / cameraReturnDuration);
                float eased = t * t * (3f - 2f * t);
                if (camera != null)
                {
                    camera.transform.position = Vector3.Lerp(
                        cinematicCameraPosition,
                        gameplayCameraPosition,
                        eased
                    );
                    camera.transform.rotation = Quaternion.Slerp(
                        cinematicCameraRotation,
                        gameplayCameraRotation,
                        eased
                    );
                    if (camera.orthographic)
                    {
                        camera.orthographicSize = Mathf.Lerp(
                            originalOrtho * 0.92f,
                            originalOrtho,
                            eased
                        );
                    }
                    else
                    {
                        camera.fieldOfView = Mathf.Lerp(
                            originalFov * 0.94f,
                            originalFov,
                            eased
                        );
                    }
                }
                horseController.SnapCinematicRiderToMountPoint();
                yield return null;
            }

            if (camera != null)
            {
                camera.fieldOfView = originalFov;
                camera.orthographicSize = originalOrtho;
            }

            horseController.MaintainMountedRunIntroBinding(player);
            horseController.CompleteMountedRunIntro();
            horseController.PrepareMountedGameplayAfterCinematic(
                clearDirection
            );
            if (restoreCameraFollow && cameraFollow != null)
            {
                cameraFollow.SetTarget(horse);
                cameraFollow.enabled = true;
            }
            RestoreMountedIntroControls(
                states,
                horseController,
                player
            );
            horseController.MaintainMountedRunIntroBinding(player);
            EnsureExactlyOneActiveAudioListener(camera);
            ClearPrimedMountedEntranceCamera(false);

            awaitingRunStart = false;
            inputLocked = false;
            RestoreMountedIntroRiderOffscreenPolicy(player);
        }






        // BD CLEAR-DIRECTION MOUNTED INTRO TURN V23R17
        private static Vector3 ResolveClearMountedIntroDirection(
            Transform horse,
            Transform player,
            Vector3 origin,
            Vector3 inwardDirection,
            BDMinimapRoom entranceRoom)
        {
            Vector3 inward = inwardDirection;
            inward.y = 0f;
            if (inward.sqrMagnitude < 0.001f)
                inward = Vector3.forward;
            inward.Normalize();

            Vector3[] candidates =
            {
                inward,
                Quaternion.AngleAxis(90f, Vector3.up) * inward,
                Quaternion.AngleAxis(-90f, Vector3.up) * inward,
                -inward
            };

            for (int index = 0; index < candidates.Length; index++)
            {
                Vector3 direction = candidates[index].normalized;
                if (IsMountedIntroDirectionClear(
                        horse,
                        player,
                        origin,
                        direction,
                        entranceRoom))
                {
                    return direction;
                }
            }

            return inward;
        }

        private static bool IsMountedIntroDirectionClear(
            Transform horse,
            Transform player,
            Vector3 origin,
            Vector3 direction,
            BDMinimapRoom entranceRoom)
        {
            const float probeDistance = 4.8f;
            CharacterController body = horse != null
                ? horse.GetComponent<CharacterController>()
                : null;
            float radius = body != null
                ? Mathf.Max(0.32f, body.radius * 0.92f)
                : 0.70f;
            float height = body != null
                ? Mathf.Max(radius * 2f, body.height)
                : 2.20f;
            Vector3 center = origin +
                (body != null ? body.center : Vector3.up * 1.1f);
            float halfSegment = Mathf.Max(0f, height * 0.5f - radius);
            Vector3 bottom = center - Vector3.up * halfSegment;
            Vector3 top = center + Vector3.up * halfSegment;

            Vector3 endPoint = origin + direction * probeDistance;
            if (entranceRoom != null &&
                !entranceRoom.ContainsWorldPosition(endPoint, -1.10f))
            {
                return false;
            }

            if (BDHazardVolume.TryFindUnsafeVolume(
                    endPoint,
                    radius + 0.20f,
                    out _))
            {
                return false;
            }

            RaycastHit[] hits = Physics.CapsuleCastAll(
                bottom,
                top,
                radius,
                direction,
                probeDistance,
                ~0,
                QueryTriggerInteraction.Ignore
            );

            foreach (RaycastHit hit in hits)
            {
                if (hit.collider == null)
                    continue;
                Transform hitTransform = hit.collider.transform;
                if ((horse != null &&
                     (hitTransform == horse || hitTransform.IsChildOf(horse))) ||
                    (player != null &&
                     (hitTransform == player || hitTransform.IsChildOf(player))))
                {
                    continue;
                }
                return false;
            }

            return true;
        }

        private static void ResolveRoomEntranceCameraPose(
            Transform entrance,
            Vector3 inwardDirection,
            out Vector3 cameraPosition,
            out Vector3 lookPoint,
            out BDMinimapRoom entranceRoom)
        {
            Vector3 inward = inwardDirection;
            inward.y = 0f;
            if (inward.sqrMagnitude < 0.001f)
                inward = ResolveEntranceInwardDirection(entrance);
            inward.Normalize();

            if (TryResolveAdjacentRoom(
                    entrance,
                    inward,
                    out entranceRoom))
            {
                Vector3 center = entranceRoom.WorldCenter;
                float roomDepth = Mathf.Max(12f, entranceRoom.RoomSize);
                float halfDepth = roomDepth * 0.5f;
                float rearWallCoordinate =
                    Vector3.Dot(center, inward) + halfDepth;

                // BD HIGHER ENTRANCE ESTABLISHING CAMERA V21
                // Exactly 30% of room depth from the rear wall toward the
                // entrance: this is farther from the entrance than the failed
                // marker/renderer-bounds calculation and remains room-authored.
                float cameraCoordinate =
                    rearWallCoordinate - roomDepth * 0.30f;
                Vector3 horizontal = center;
                horizontal += inward * (
                    cameraCoordinate - Vector3.Dot(horizontal, inward)
                );
                horizontal = ClampPointInsideRoom(
                    entranceRoom,
                    horizontal,
                    2.0f
                );

                float cameraHeight = Mathf.Clamp(
                    roomDepth * 0.72f,
                    17.0f,
                    21.5f
                );
                cameraPosition = new Vector3(
                    horizontal.x,
                    center.y + cameraHeight,
                    horizontal.z
                );
                lookPoint = new Vector3(
                    entrance.position.x,
                    center.y + 3.8f,
                    entrance.position.z
                );
                return;
            }

            entranceRoom = null;
            cameraPosition =
                entrance.position +
                inward * 14.0f +
                Vector3.up * 17.0f;
            lookPoint = entrance.position + Vector3.up * 3.8f;
        }

        private static Vector3 ResolveEntranceInwardDirection(
            Transform entrance)
        {
            if (entrance == null)
                return Vector3.forward;

            Renderer[] renderers =
                Resources.FindObjectsOfTypeAll<Renderer>();
            Renderer nearestRoom = null;
            float nearestDistance = float.PositiveInfinity;

            foreach (Renderer renderer in renderers)
            {
                if (renderer == null ||
                    !renderer.gameObject.scene.IsValid() ||
                    renderer.name.IndexOf(
                        "BD_MinimapRoom_",
                        StringComparison.OrdinalIgnoreCase) < 0)
                {
                    continue;
                }

                Vector3 delta =
                    renderer.bounds.center - entrance.position;
                delta.y = 0f;
                float distance = delta.sqrMagnitude;
                if (distance < nearestDistance)
                {
                    nearestDistance = distance;
                    nearestRoom = renderer;
                }
            }

            if (nearestRoom != null)
            {
                Vector3 direction =
                    nearestRoom.bounds.center - entrance.position;
                direction.y = 0f;
                if (direction.sqrMagnitude > 0.001f)
                    return direction.normalized;
            }

            Vector3 fallback = entrance.forward;
            fallback.y = 0f;
            return fallback.sqrMagnitude > 0.001f
                ? fallback.normalized
                : Vector3.forward;
        }


        private static bool TryResolveAdjacentRoom(
            Transform entrance,
            Vector3 inwardDirection,
            out BDMinimapRoom room)
        {
            room = null;
            if (entrance == null)
                return false;

            BDMinimapRoom[] rooms =
                UnityEngine.Object.FindObjectsByType<BDMinimapRoom>(
                    FindObjectsSortMode.None
                );
            Vector3 probe =
                entrance.position + inwardDirection.normalized * 2.0f;
            float bestScore = float.PositiveInfinity;

            foreach (BDMinimapRoom candidate in rooms)
            {
                if (candidate == null)
                    continue;

                float score = candidate.SqrDistanceToCenter(probe);
                if (!candidate.ContainsWorldPosition(probe, 0.35f))
                    score += 10000f;

                Vector3 towardRoom =
                    candidate.WorldCenter - entrance.position;
                towardRoom.y = 0f;
                if (towardRoom.sqrMagnitude > 0.001f &&
                    Vector3.Dot(
                        towardRoom.normalized,
                        inwardDirection.normalized
                    ) < 0.25f)
                {
                    score += 10000f;
                }

                if (score < bestScore)
                {
                    bestScore = score;
                    room = candidate;
                }
            }

            return room != null;
        }

        private static Vector3 ClampPointInsideRoom(
            BDMinimapRoom room,
            Vector3 point,
            float inset)
        {
            if (room == null)
                return point;

            float halfSize = Mathf.Max(
                0.5f,
                room.RoomSize * 0.5f - Mathf.Max(0f, inset)
            );
            Vector3 center = room.WorldCenter;
            point.x = Mathf.Clamp(
                point.x,
                center.x - halfSize,
                center.x + halfSize
            );
            point.z = Mathf.Clamp(
                point.z,
                center.z - halfSize,
                center.z + halfSize
            );
            return point;
        }

        private static void HoldCinematicCamera(
            Camera camera,
            Vector3 position,
            Quaternion rotation,
            float originalFov,
            float originalOrtho)
        {
            if (camera == null)
                return;

            camera.transform.position = position;
            camera.transform.rotation = rotation;
            if (camera.orthographic)
                camera.orthographicSize = originalOrtho * 0.92f;
            else
                camera.fieldOfView = originalFov * 0.94f;
        }

        // BD EXACTLY ONE ACTIVE AUDIO LISTENER V20
        private static void EnsureExactlyOneActiveAudioListener(
            Camera preferredCamera)
        {
            Camera ownerCamera = preferredCamera != null
                ? preferredCamera
                : Camera.main;

            if (ownerCamera == null)
            {
                Camera[] cameras =
                    Resources.FindObjectsOfTypeAll<Camera>();
                foreach (Camera candidate in cameras)
                {
                    if (candidate == null ||
                        !candidate.gameObject.scene.IsValid() ||
                        !candidate.gameObject.activeInHierarchy)
                    {
                        continue;
                    }
                    ownerCamera = candidate;
                    break;
                }
            }

            if (ownerCamera == null)
                return;

            AudioListener primary =
                ownerCamera.GetComponent<AudioListener>();
            if (primary == null)
                primary = ownerCamera.gameObject.AddComponent<AudioListener>();

            AudioListener[] listeners =
                Resources.FindObjectsOfTypeAll<AudioListener>();
            foreach (AudioListener listener in listeners)
            {
                if (listener == null ||
                    !listener.gameObject.scene.IsValid())
                {
                    continue;
                }

                listener.enabled = listener == primary;
            }
            primary.enabled = true;
        }
        public static void BeginExitTransition(Transform actor, Transform authoredExit)
        {
            if (instance == null || instance.activeSequence != null)
                return;

            instance.activeSequence = instance.StartCoroutine(
                instance.PlayExitTransition(actor, authoredExit));
        }

        private IEnumerator PlayExitTransition(Transform actor, Transform authoredExit)
        {
            if (actor == null || authoredExit == null)
            {
                activeSequence = null;
                yield break;
            }

            inputLocked = true;
            awaitingRunStart = false;
            Transform player = ResolveCurrentPlayerTransform();
            Transform horse = ResolveCurrentHorseTransform();
            BDHorseController horseController =
                horse != null ? horse.GetComponent<BDHorseController>() : null;
            List<ComponentState> states = CaptureAndDisableControls(player, horse);

            Vector3 direction = authoredExit.position - actor.position;
            direction.y = 0f;
            if (direction.sqrMagnitude < 0.01f)
                direction = authoredExit.forward;
            direction.Normalize();

            Vector3 start = actor.position;
            Vector3 end = authoredExit.position + direction * 2.5f;
            end.y = start.y;

            float elapsed = 0f;
            const float moveDuration = 1.15f;
            while (elapsed < moveDuration)
            {
                elapsed += Time.unscaledDeltaTime;
                float t = Mathf.Clamp01(elapsed / moveDuration);
                actor.position = Vector3.Lerp(start, end, t * t * (3f - 2f * t));
                if (horseController != null && horseController.IsMounted)
                    horseController.SnapCinematicRiderToMountPoint();
                Physics.SyncTransforms();
                yield return null;
            }

            MarkCinematicSeen();
            yield return FadeCoverTo(1f, 9f);
            InvokeExistingExitFlow(authoredExit);

            float sequenceDeadline = Time.realtimeSinceStartup + 1f;
            while (Time.realtimeSinceStartup < sequenceDeadline)
                yield return null;

            bool sequenceOwnsControl =
                BDMainMenuFlow.Instance != null &&
                BDMainMenuFlow.Instance.IsResultSequenceActive;

            if (!sequenceOwnsControl)
            {
                RestoreControls(states);
                inputLocked = false;
            }

            yield return FadeCoverTo(0f, 7f);
            activeSequence = null;
        }

        private IEnumerator FadeCoverTo(float target, float speed)
        {
            coverTarget = Mathf.Clamp01(target);
            coverSpeed = Mathf.Max(0.1f, speed);
            while (Mathf.Abs(coverAlpha - coverTarget) > 0.01f)
                yield return null;
            coverAlpha = coverTarget;
        }

        private static bool IsMainMenuVisible()
        {
            if (BDMainMenuFlow.Instance != null &&
                !BDMainMenuFlow.Instance.IsRunActive)
            {
                return true;
            }

            GameObject[] roots = SceneManager.GetActiveScene().GetRootGameObjects();
            foreach (GameObject root in roots)
            {
                string name = root.name.ToLowerInvariant();
                if (root.activeInHierarchy &&
                    (name.Contains("mainmenu") || name.Contains("main_menu") ||
                     name.Contains("mainmenusettings")))
                {
                    return true;
                }
            }

            return false;
        }

        private static bool IsGameplayRunActive()
        {
            return BDMainMenuFlow.Instance != null &&
                   BDMainMenuFlow.Instance.IsRunActive;
        }

        private static void ReleaseOnFootGameplayControl()
        {
            if (BDMainMenuFlow.Instance != null)
                BDMainMenuFlow.Instance.ReleaseControlAfterRunPresentation(false);
        }
        private static Vector3 ResolveAuthoredEntranceMouthStart(
            Transform entrance,
            Vector3 inwardDirection,
            float horseGroundOffset)
        {
            Vector3 direction = inwardDirection;
            direction.y = 0f;
            if (direction.sqrMagnitude < 0.001f)
            {
                direction = entrance != null
                    ? entrance.forward
                    : Vector3.forward;
            }
            direction.Normalize();

            Vector3 start = entrance != null
                ? entrance.position - direction * 2.20f
                : Vector3.zero;

            if (TryResolveGroundPoint(start, out Vector3 ground))
                return ground + Vector3.up * horseGroundOffset;

            return new Vector3(
                start.x,
                entrance != null
                    ? entrance.position.y + horseGroundOffset
                    : horseGroundOffset,
                start.z
            );
        }

        private static float ResolveHorseGroundOffset(Transform horse)
        {
            if (horse != null &&
                TryResolveGroundPoint(horse.position, out Vector3 ground))
            {
                return Mathf.Clamp(
                    horse.position.y - ground.y,
                    0.04f,
                    2.5f
                );
            }

            CharacterController controller =
                horse != null
                    ? horse.GetComponent<CharacterController>()
                    : null;

            if (controller == null)
                return 1.05f;

            return Mathf.Max(
                0.05f,
                controller.height * 0.5f - controller.center.y
            );
        }

        private static bool TryResolveGroundPoint(
            Vector3 requested,
            out Vector3 ground)
        {
            Vector3 origin = requested + Vector3.up * 5f;
            RaycastHit[] hits = Physics.RaycastAll(
                origin,
                Vector3.down,
                12f,
                ~0,
                QueryTriggerInteraction.Ignore
            );
            Array.Sort(
                hits,
                (left, right) =>
                    left.distance.CompareTo(right.distance)
            );

            foreach (RaycastHit hit in hits)
            {
                if (hit.collider == null || hit.collider.isTrigger)
                    continue;
                if (hit.collider.GetComponentInParent<BDPlayerMarker>() != null)
                    continue;
                if (hit.collider.GetComponentInParent<BDHorseController>() != null)
                    continue;

                ground = hit.point;
                return true;
            }

            ground = requested;
            return false;
        }

        private static void EnsureEntranceApproachGround(
            Transform entrance,
            Vector3 start,
            Vector3 inwardDirection)
        {
            if (entrance == null ||
                TryResolveGroundPoint(start, out _))
            {
                return;
            }

            const string supportName =
                "BD_Entrance_ApproachGround_SupportOnly";
            Transform existing = entrance.parent != null
                ? entrance.parent.Find(supportName)
                : null;
            if (existing != null)
                return;

            GameObject support =
                GameObject.CreatePrimitive(PrimitiveType.Cube);
            support.name = supportName;
            support.transform.SetParent(
                entrance.parent,
                worldPositionStays: true
            );

            Vector3 direction = inwardDirection;
            direction.y = 0f;
            if (direction.sqrMagnitude < 0.001f)
                direction = entrance.forward;
            direction.Normalize();

            Vector3 midpoint =
                (start + entrance.position) * 0.5f;
            support.transform.position = new Vector3(
                midpoint.x,
                entrance.position.y - 0.10f,
                midpoint.z
            );
            support.transform.rotation =
                Quaternion.LookRotation(direction, Vector3.up);

            Renderer entranceRenderer =
                entrance.GetComponentInChildren<Renderer>();
            float width = entranceRenderer != null
                ? Mathf.Clamp(
                    Mathf.Max(
                        entranceRenderer.bounds.size.x,
                        entranceRenderer.bounds.size.z
                    ) * 0.55f,
                    2.4f,
                    5.5f
                )
                : 3.2f;

            float length = Mathf.Max(
                2.8f,
                Vector3.Distance(start, entrance.position) + 1.2f
            );
            support.transform.localScale =
                new Vector3(width, 0.20f, length);

            Renderer supportRenderer =
                support.GetComponent<Renderer>();
            if (supportRenderer != null && entranceRenderer != null)
            {
                supportRenderer.sharedMaterial =
                    entranceRenderer.sharedMaterial;
            }
        }

        private static Transform ResolveCurrentPlayerTransform(
            BDHorseController preferredHorse = null)
        {
            Scene activeScene = SceneManager.GetActiveScene();

            Transform serializedRider =
                preferredHorse != null
                    ? preferredHorse.Rider
                    : null;
            if (IsValidCurrentScenePlayer(
                    serializedRider,
                    activeScene))
            {
                return serializedRider;
            }

            Transform canonicalPlayer =
                BDTargetFinder.FindPlayer();
            if (IsValidCurrentScenePlayer(
                    canonicalPlayer,
                    activeScene))
            {
                return canonicalPlayer;
            }

            BDPlayerController player =
                FindBestCurrentSceneComponent<BDPlayerController>();
            return player != null ? player.transform : null;
        }

        private static bool IsValidCurrentScenePlayer(
            Transform candidate,
            Scene activeScene)
        {
            return candidate != null &&
                   candidate.gameObject.activeInHierarchy &&
                   candidate.gameObject.scene.IsValid() &&
                   candidate.gameObject.scene.isLoaded &&
                   candidate.gameObject.scene == activeScene &&
                   candidate.GetComponent<BDPlayerMarker>() != null;
        }

        private static Transform ResolveCurrentHorseTransform()
        {
            BDHorseController horse =
                FindBestCurrentSceneComponent<BDHorseController>();
            return horse != null ? horse.transform : null;
        }

        private static T FindBestCurrentSceneComponent<T>()
            where T : MonoBehaviour
        {
            T[] candidates = Resources.FindObjectsOfTypeAll<T>();
            Scene activeScene = SceneManager.GetActiveScene();
            T best = null;
            int bestScore = int.MinValue;

            foreach (T candidate in candidates)
            {
                if (candidate == null ||
                    !candidate.gameObject.scene.IsValid() ||
                    !candidate.gameObject.scene.isLoaded)
                {
                    continue;
                }

                int score = 0;
                if (candidate.gameObject.scene == activeScene)
                    score += 100;
                if (candidate.gameObject.activeInHierarchy)
                    score += 40;
                if (candidate.enabled)
                    score += 10;
                if (candidate.hideFlags == HideFlags.None)
                    score += 5;

                if (score <= bestScore)
                    continue;

                best = candidate;
                bestScore = score;
            }

            return best;
        }

        public static bool EnsureMountedIntroRiderVisible(
            Transform rider)
        {
            return instance != null &&
                   instance.ForceMountedIntroRiderPresentationVisible(
                       rider
                   );
        }

        private void ClearMountedIntroRiderPresentationBaseline()
        {
            RestoreMountedIntroRiderOffscreenPolicy(
                mountedIntroRiderRoot
            );

            mountedIntroRiderRenderers.Clear();
            mountedIntroRiderSkinnedRenderers.Clear();
            mountedIntroRiderOriginalOffscreenPolicy.Clear();
            mountedIntroRiderRoot = null;
        }

        private void CaptureMountedIntroRiderPresentationBaseline(
            Transform rider)
        {
            if (rider == null)
                return;

            if (mountedIntroRiderRoot == rider &&
                mountedIntroRiderRenderers.Count > 0)
            {
                return;
            }

            ClearMountedIntroRiderPresentationBaseline();
            mountedIntroRiderRoot = rider;

            Renderer[] candidates =
                rider.GetComponentsInChildren<Renderer>(
                    includeInactive: true
                );

            for (int pass = 0; pass < 2; pass++)
            {
                for (int index = 0;
                     index < candidates.Length;
                     index++)
                {
                    Renderer candidate = candidates[index];

                    if (!IsMountedIntroRiderBodyRenderer(candidate))
                        continue;

                    bool baselineVisible =
                        candidate.gameObject.activeInHierarchy &&
                        candidate.enabled &&
                        !candidate.forceRenderingOff;

                    bool fallbackEligible =
                        candidate.gameObject.activeSelf &&
                        candidate.enabled;

                    if ((pass == 0 && !baselineVisible) ||
                        (pass == 1 &&
                         mountedIntroRiderRenderers.Count > 0) ||
                        (pass == 1 && !fallbackEligible))
                    {
                        continue;
                    }

                    if (mountedIntroRiderRenderers.Contains(candidate))
                        continue;

                    mountedIntroRiderRenderers.Add(candidate);

                    if (candidate is SkinnedMeshRenderer skinned)
                    {
                        mountedIntroRiderSkinnedRenderers.Add(skinned);
                        mountedIntroRiderOriginalOffscreenPolicy.Add(
                            skinned.updateWhenOffscreen
                        );
                    }
                }

                if (mountedIntroRiderRenderers.Count > 0)
                    break;
            }
        }

        private bool ForceMountedIntroRiderPresentationVisible(
            Transform rider)
        {
            if (rider == null)
                return false;

            if (mountedIntroRiderRoot != rider ||
                mountedIntroRiderRenderers.Count == 0)
            {
                CaptureMountedIntroRiderPresentationBaseline(rider);
            }

            if (!rider.gameObject.activeSelf)
                rider.gameObject.SetActive(true);

            bool visibleRendererFound = false;

            for (int index = 0;
                 index < mountedIntroRiderRenderers.Count;
                 index++)
            {
                Renderer renderer =
                    mountedIntroRiderRenderers[index];

                if (renderer == null)
                    continue;

                ActivateMountedIntroRendererPath(
                    renderer.transform,
                    rider
                );

                renderer.enabled = true;
                renderer.forceRenderingOff = false;

                if (renderer is SkinnedMeshRenderer skinned)
                    skinned.updateWhenOffscreen = true;

                visibleRendererFound = true;
            }

            return visibleRendererFound;
        }

        private void RestoreMountedIntroRiderOffscreenPolicy(
            Transform rider)
        {
            if (rider != null &&
                mountedIntroRiderRoot != null &&
                rider != mountedIntroRiderRoot)
            {
                return;
            }

            int count = Mathf.Min(
                mountedIntroRiderSkinnedRenderers.Count,
                mountedIntroRiderOriginalOffscreenPolicy.Count
            );

            for (int index = 0; index < count; index++)
            {
                SkinnedMeshRenderer renderer =
                    mountedIntroRiderSkinnedRenderers[index];

                if (renderer != null)
                {
                    renderer.updateWhenOffscreen =
                        mountedIntroRiderOriginalOffscreenPolicy[index];
                }
            }
        }

        private static void ActivateMountedIntroRendererPath(
            Transform rendererTransform,
            Transform riderRoot)
        {
            Transform current = rendererTransform;

            while (current != null)
            {
                if (!current.gameObject.activeSelf)
                    current.gameObject.SetActive(true);

                if (current == riderRoot)
                    break;

                current = current.parent;
            }
        }

        private static bool IsMountedIntroRiderBodyRenderer(
            Renderer renderer)
        {
            if (renderer == null ||
                renderer is LineRenderer ||
                renderer is TrailRenderer ||
                renderer is ParticleSystemRenderer)
            {
                return false;
            }

            if (!(renderer is MeshRenderer) &&
                !(renderer is SkinnedMeshRenderer))
            {
                return false;
            }

            string lower =
                renderer.gameObject.name.ToLowerInvariant();

            string[] excluded =
            {
                "outline",
                "afterimage",
                "damage",
                "hitflash",
                "telegraph",
                "indicator",
                "prompt",
                "health",
                "worldbar",
                "trail",
                "particle",
                "vfx",
                "effect",
                "slash",
                "projectile",
                "hookline"
            };

            for (int index = 0;
                 index < excluded.Length;
                 index++)
            {
                if (lower.Contains(excluded[index]))
                    return false;
            }

            return true;
        }

        private static Transform FindGameplayTransform(string componentTypeName, string nameHint)
        {
            MonoBehaviour[] behaviours = Resources.FindObjectsOfTypeAll<MonoBehaviour>();
            foreach (MonoBehaviour behaviour in behaviours)
            {
                if (behaviour == null || !behaviour.gameObject.scene.IsValid())
                    continue;
                if (behaviour.GetType().Name == componentTypeName)
                    return behaviour.transform;
            }

            GameObject tagged = null;
            try { tagged = GameObject.FindGameObjectWithTag(nameHint); }
            catch (UnityException) { }
            if (tagged != null)
                return tagged.transform;

            Transform[] transforms = Resources.FindObjectsOfTypeAll<Transform>();
            foreach (Transform candidate in transforms)
            {
                if (candidate != null && candidate.gameObject.scene.IsValid() &&
                    candidate.name.IndexOf(nameHint, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    return candidate;
                }
            }

            return null;
        }

        private static Transform FindBestSpawnPoint()
        {
            string[] hints =
            {
                "PlayerSpawn", "SpawnPoint", "StartSpawn", "Player_Start", "StartPoint"
            };
            Transform[] all = Resources.FindObjectsOfTypeAll<Transform>();
            foreach (string hint in hints)
            {
                foreach (Transform item in all)
                {
                    if (item != null && item.gameObject.scene.IsValid() &&
                        item.name.IndexOf(hint, StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        return item;
                    }
                }
            }
            return null;
        }

        private static Transform FindAuthoredDoor(bool entrance)
        {
            string exactName = entrance
                ? "BD_Maze_Entrance_Marker"
                : "BD_Maze_Exit_Marker";

            GameObject exact = GameObject.Find(exactName);
            if (exact != null)
                return exact.transform;

            // Conservative fallback for older scenes. Exit-pressure props are
            // explicitly excluded so the original exit marker remains owner.
            string requiredWord = entrance ? "entrance" : "exit";
            Transform[] all =
                Resources.FindObjectsOfTypeAll<Transform>();
            Transform best = null;
            int bestScore = int.MinValue;

            foreach (Transform item in all)
            {
                if (item == null || !item.gameObject.scene.IsValid())
                    continue;

                string lower = item.name.ToLowerInvariant();
                if (!lower.Contains(requiredWord) ||
                    lower.Contains("pressure") ||
                    lower.Contains("portal") ||
                    lower.Contains("effect") ||
                    lower.Contains("approachtrigger") ||
                    lower.Contains("returnblocker"))
                {
                    continue;
                }

                int score = 10;
                if (lower.Contains("marker"))
                    score += 10;
                if (lower.Contains("maze"))
                    score += 5;
                if (item.GetComponent<Renderer>() != null)
                    score += 3;

                if (score > bestScore)
                {
                    bestScore = score;
                    best = item;
                }
            }

            return best;
        }

        private static void DestroyObsoleteGeneratedPortals()
        {
            string[] names =
            {
                "BD_Entrance_Portal",
                "BD_Exit_Portal",
                "BD_GeneratedEntranceBox",
                "BD_GeneratedExitBox",
                "BD_RunIntroEntranceBox",
                "BD_RunIntroExitBox",
                ExitApproachName
            };

            Transform[] all =
                Resources.FindObjectsOfTypeAll<Transform>();
            foreach (Transform item in all)
            {
                if (item == null || !item.gameObject.scene.IsValid())
                    continue;

                bool remove = false;
                foreach (string name in names)
                {
                    if (item.name == name)
                    {
                        remove = true;
                        break;
                    }
                }

                if (!remove &&
                    item.GetComponent<BDDoorwayPortalVisual>() != null)
                {
                    remove = true;
                }

                if (remove)
                    Destroy(item.gameObject);
            }
        }

        private static BDEntranceReturnBlocker AttachEntranceReturnBlocker(
            Transform authoredEntrance)
        {
            if (authoredEntrance == null)
                return null;

            Transform parent = authoredEntrance.parent;
            Transform existing = authoredEntrance.Find(EntranceBlockerName);
            if (existing == null && parent != null)
                existing = parent.Find(EntranceBlockerName);
            if (existing == null)
            {
                GameObject found = GameObject.Find(EntranceBlockerName);
                existing = found != null ? found.transform : null;
            }

            GameObject blockerObject = existing != null
                ? existing.gameObject
                : new GameObject(EntranceBlockerName);

            blockerObject.transform.SetParent(
                parent,
                worldPositionStays: true
            );

            BDEntranceReturnBlocker blocker =
                blockerObject.GetComponent<BDEntranceReturnBlocker>();
            if (blocker == null)
            {
                blocker = blockerObject.AddComponent<
                    BDEntranceReturnBlocker>();
            }

            blocker.ConfigureFromAuthoredEntrance(authoredEntrance);
            return blocker;
        }

        private static void SetEntranceReturnBlocking(bool blocking)
        {
            Transform entrance = FindAuthoredDoor(true);
            if (entrance == null)
                return;

            BDEntranceReturnBlocker blocker =
                AttachEntranceReturnBlocker(entrance);
            if (blocker != null)
                blocker.SetBlocking(blocking);
        }

        private static void EnsureAuthoredPortalEffects()
        {
            AttachEffectOnly(
                FindAuthoredDoor(true),
                EntranceEffectName
            );
            AttachEffectOnly(
                FindAuthoredDoor(false),
                ExitEffectName
            );
        }

        private static void AttachEffectOnly(
            Transform authoredDoor,
            string effectName)
        {
            if (authoredDoor == null)
                return;

            Transform parent = authoredDoor.parent;
            Transform existing = authoredDoor.Find(effectName);
            if (existing == null && parent != null)
                existing = parent.Find(effectName);
            if (existing == null)
            {
                GameObject found = GameObject.Find(effectName);
                existing = found != null ? found.transform : null;
            }

            GameObject effect = existing != null
                ? existing.gameObject
                : new GameObject(effectName);

            effect.SetActive(true);
            effect.transform.SetParent(
                parent,
                worldPositionStays: true
            );
            effect.transform.localScale = Vector3.one;

            BDPortalSurfaceEffect portal =
                effect.GetComponent<BDPortalSurfaceEffect>();
            if (portal == null)
                portal = effect.AddComponent<BDPortalSurfaceEffect>();

            portal.enabled = true;
            portal.Configure(authoredDoor);
        }

        private static List<ComponentState> CaptureAndDisableControls(
            Transform player,
            Transform horse)
        {
            List<ComponentState> states = new List<ComponentState>();
            CaptureTransformControls(player, states);
            CaptureTransformControls(horse, states);
            return states;
        }

        private static void CaptureTransformControls(
            Transform root,
            List<ComponentState> states)
        {
            if (root == null)
                return;

            MonoBehaviour[] behaviours = root.GetComponentsInChildren<MonoBehaviour>(true);
            foreach (MonoBehaviour behaviour in behaviours)
            {
                if (behaviour == null)
                    continue;

                string name = behaviour.GetType().Name.ToLowerInvariant();
                if (!(name.Contains("controller") || name.Contains("combat") ||
                      name.Contains("melee") || name.Contains("ranged") ||
                      name.Contains("interaction") || name.Contains("pet")))
                {
                    continue;
                }

                states.Add(new ComponentState(behaviour, behaviour.enabled));
                behaviour.enabled = false;
            }

            CharacterController[] controllers =
                root.GetComponentsInChildren<CharacterController>(true);
            foreach (CharacterController controller in controllers)
            {
                states.Add(new ComponentState(controller, controller.enabled));
                controller.enabled = false;
            }
        }


        private static void RestoreMountedIntroControls(
            List<ComponentState> states,
            BDHorseController horseController,
            Transform rider)
        {
            foreach (ComponentState state in states)
            {
                if (state.component == null)
                    continue;

                if (state.component is BDPlayerController playerController &&
                    rider != null &&
                    (playerController.transform == rider ||
                     playerController.transform.IsChildOf(rider) ||
                     rider.IsChildOf(playerController.transform)))
                {
                    playerController.enabled = false;
                    continue;
                }

                if (state.component is Behaviour behaviour)
                    behaviour.enabled = state.enabled;
                else if (state.component is Collider collider)
                    collider.enabled = state.enabled;
            }

            if (horseController != null)
                horseController.MaintainMountedRunIntroBinding(rider);
        }

        private static void RestoreControls(List<ComponentState> states)
        {
            foreach (ComponentState state in states)
            {
                if (state.component == null)
                    continue;

                if (state.component is Behaviour behaviour)
                    behaviour.enabled = state.enabled;
                else if (state.component is Collider collider)
                    collider.enabled = state.enabled;
            }
        }

        private static void InvokeExistingExitFlow(Transform authoredExit)
        {
            string[] names =
            {
                "BeginExit", "EnterExit", "TriggerExit", "StartEnding",
                "PlayEnding", "CompleteRun"
            };

            foreach (MonoBehaviour component in
                     authoredExit.GetComponentsInChildren<MonoBehaviour>(true))
            {
                if (component == null || component is BDExitCinematicApproachTrigger)
                    continue;

                foreach (string name in names)
                {
                    MethodInfo method = component.GetType().GetMethod(
                        name,
                        BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
                        null,
                        Type.EmptyTypes,
                        null);
                    if (method == null)
                        continue;

                    try
                    {
                        method.Invoke(component, null);
                        return;
                    }
                    catch (Exception) { }
                }
            }
        }

        private readonly struct ComponentState
        {
            public readonly Component component;
            public readonly bool enabled;

            public ComponentState(Component component, bool enabled)
            {
                this.component = component;
                this.enabled = enabled;
            }
        }
    }
}
