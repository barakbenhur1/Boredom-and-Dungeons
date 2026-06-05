using System.Collections.Generic;
using UnityEngine;

#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace BoredomAndDungeons
{
    [DisallowMultipleComponent]
    public sealed class BDMazeMinimap : MonoBehaviour
    {
        [Header("Display")]
        [SerializeField] private bool visible = true;
        [SerializeField] private Rect panel = new Rect(0f, 0f, 300f, 300f);
        [SerializeField] private float marginRight = 18f;
        [SerializeField] private float marginBottom = 18f;
        [SerializeField] private float roomGap = 2f;
        [SerializeField] private float markerSize = 8f;
        [SerializeField] private float nearestDiscoveryMaxDistance = 34f;

        [Header("Dynamic Player-Up Rotation")]
        // BD PLAYER-UP DYNAMIC MINIMAP FIX
        [SerializeField] private bool rotateWithPlayerDirection = true;
        [SerializeField] private float rotationSpeedDegreesPerSecond = 900f;
        [SerializeField] private float rotationOffsetDegrees = 0f;
        // BD MINIMAP 90-DEGREE MOVEMENT SNAP FIX
        [SerializeField] private bool snapToMovementCardinals = true;
        [SerializeField] private float movementSnapThreshold = 0.35f;
        // BD MINIMAP NEAREST-CARDINAL SECTOR FIX
        [SerializeField] private float diagonalBoundaryHoldEpsilon = 0.015f;

        [Header("Colors")]
        [SerializeField] private Color backgroundColor = new Color(0f, 0f, 0f, 0.78f);
        [SerializeField] private Color unknownColor = new Color(0f, 0f, 0f, 0.0f);
        [SerializeField] private Color discoveredRoomColor = new Color(0.36f, 0.48f, 0.62f, 0.96f);
        [SerializeField] private Color wallColor = new Color(0.08f, 0.10f, 0.13f, 0.95f);
        [SerializeField] private Color playerColor = new Color(1f, 1f, 1f, 1f);
        [SerializeField] private Color horseColor = new Color(0.15f, 0.95f, 1f, 1f);
        [SerializeField] private Color currentRoomColor = new Color(0.55f, 0.80f, 1f, 1f);
        [SerializeField] private Color roomOutlineColor = new Color(0.12f, 0.18f, 0.24f, 0.95f);

        private readonly List<BDMinimapRoom> rooms = new List<BDMinimapRoom>();
        private Texture2D whiteTexture;
        private Transform player;
        private Transform horse;
        private GUIStyle labelStyle;
        private BDPlayerController playerController;
        private BDHorseController horseController;
        private float currentMapRotationDegrees;
        private bool mapRotationInitialized;

        private int minX;
        private int maxX;
        private int minY;
        private int maxY;
        private bool boundsReady;
        private float refreshRetryTimer;

        private void Awake()
        {
            whiteTexture = Texture2D.whiteTexture;
        }

        private void Start()
        {
            RefreshRooms();
        }

        private void ResolvePlayerReference()
        {
            if (player == null)
            {
                BDPlayerMarker marker =
                    FindFirstObjectByType<BDPlayerMarker>();

                if (marker != null)
                    player = marker.transform;
                else
                    player = BDTargetFinder.FindPlayer();
            }

            if (player != null && playerController == null)
            {
                playerController =
                    player.GetComponent<BDPlayerController>();
            }
        }

        private void Update()
        {
            if (ReadTogglePressed())
                visible = !visible;

            ResolvePlayerReference();
            TickDynamicPlayerUpRotation();

            if (horse == null)
            {
                BDHorseHealth horseHealth = FindFirstObjectByType<BDHorseHealth>();
                if (horseHealth != null)
                    horse = horseHealth.transform;
            }

            TickPlayerDiscovery();

            if (rooms.Count == 0)
            {
                refreshRetryTimer -= Time.deltaTime;
                if (refreshRetryTimer <= 0f)
                {
                    refreshRetryTimer = 0.5f;
                    RefreshRooms();
                }
            }
        }

        public void RefreshRooms()
        {
            rooms.Clear();
            rooms.AddRange(FindObjectsByType<BDMinimapRoom>(FindObjectsSortMode.None));

            boundsReady = false;

            if (rooms.Count == 0)
                return;

            minX = maxX = rooms[0].Cell.x;
            minY = maxY = rooms[0].Cell.y;

            foreach (BDMinimapRoom room in rooms)
            {
                if (room == null)
                    continue;

                Vector2Int cell = room.Cell;
                minX = Mathf.Min(minX, cell.x);
                maxX = Mathf.Max(maxX, cell.x);
                minY = Mathf.Min(minY, cell.y);
                maxY = Mathf.Max(maxY, cell.y);
            }

            boundsReady = true;
        }

        private void TickPlayerDiscovery()
        {
            ResolvePlayerReference();

            if (player == null)
                return;

            if (rooms.Count == 0 || !boundsReady)
                RefreshRooms();

            if (rooms.Count == 0)
                return;

            BDMinimapRoom currentRoom = FindRoomContainingWorldPosition(player.position);
            if (currentRoom != null)
            {
                currentRoom.ForceDiscover();
                return;
            }

            // Hard fallback:
            // If the trigger volume or world containment is imperfect, still reveal the nearest legal room
            // as the player moves through the maze. This prevents the minimap from appearing frozen.
            BDMinimapRoom nearest = FindNearestRoom(player.position);
            if (nearest != null && nearest.SqrDistanceToCenter(player.position) <= nearestDiscoveryMaxDistance * nearestDiscoveryMaxDistance)
                nearest.ForceDiscover();
        }

        private bool HasAnyDiscoveredRoom()
        {
            foreach (BDMinimapRoom room in rooms)
            {
                if (room != null && room.Discovered)
                    return true;
            }

            return false;
        }

        private BDMinimapRoom FindRoomContainingWorldPosition(Vector3 worldPosition)
        {
            foreach (BDMinimapRoom room in rooms)
            {
                if (room != null && room.ContainsWorldPosition(worldPosition, 1.25f))
                    return room;
            }

            return null;
        }

        private BDMinimapRoom FindNearestRoom(Vector3 worldPosition)
        {
            BDMinimapRoom best = null;
            float bestDistance = float.MaxValue;

            foreach (BDMinimapRoom room in rooms)
            {
                if (room == null)
                    continue;

                float distance = room.SqrDistanceToCenter(worldPosition);
                if (distance < bestDistance)
                {
                    bestDistance = distance;
                    best = room;
                }
            }

            return best;
        }


        private void TickDynamicPlayerUpRotation()
        {
            if (!rotateWithPlayerDirection || player == null)
                return;

            if (!TryResolveMovementCardinalRotation(
                    out float desiredRotation))
            {
                return;
            }

            // Immediate 90-degree snap keeps the map parallel to its frame.
            currentMapRotationDegrees =
                desiredRotation + rotationOffsetDegrees;

            mapRotationInitialized = true;
        }

        private bool TryResolveMovementCardinalRotation(
            out float rotationDegrees)
        {
            rotationDegrees = currentMapRotationDegrees;

            Vector3 movement = Vector3.zero;

            if (horseController == null)
                horseController = FindFirstObjectByType<BDHorseController>();

            if (horseController != null &&
                horseController.IsMounted &&
                horseController.HasRideMoveInput)
            {
                movement =
                    horseController.LastMountedMovementDirection;
            }
            else if (playerController != null)
            {
                movement =
                    playerController.LastMoveWorldDirection;
            }

            movement.y = 0f;

            if (movement.sqrMagnitude <
                movementSnapThreshold * movementSnapThreshold)
            {
                return false;
            }

            movement.Normalize();

            float absoluteX = Mathf.Abs(movement.x);
            float absoluteZ = Mathf.Abs(movement.z);

            // The diagonals are the borders between the four sectors.
            // Exactly on a border, hold the previous orientation.
            if (Mathf.Abs(absoluteX - absoluteZ) <=
                Mathf.Max(0.0001f, diagonalBoundaryHoldEpsilon))
            {
                return false;
            }

            if (absoluteX > absoluteZ)
            {
                rotationDegrees =
                    movement.x > 0f ? -90f : 90f;

                return true;
            }

            rotationDegrees =
                movement.z > 0f ? 0f : 180f;

            return true;
        }

        private Vector3 ResolvePlayerViewDirection()
        {
            if (player == null)
                return Vector3.forward;

            if (playerController == null)
            {
                playerController =
                    player.GetComponent<BDPlayerController>();
            }

            if (playerController != null)
            {
                Vector3 lookDirection =
                    playerController.LastLookDirection;

                lookDirection.y = 0f;

                if (lookDirection.sqrMagnitude > 0.001f)
                    return lookDirection.normalized;
            }

            if (horseController == null)
            {
                horseController =
                    FindFirstObjectByType<BDHorseController>();
            }

            if (horseController != null &&
                horseController.IsMounted &&
                horseController.Rider != null)
            {
                Transform rider = horseController.Rider;

                bool sameRider =
                    rider == player ||
                    player.IsChildOf(rider) ||
                    rider.IsChildOf(player);

                if (sameRider)
                {
                    Vector3 mountedDirection =
                        horseController.LastMountedAimDirection;

                    mountedDirection.y = 0f;

                    if (mountedDirection.sqrMagnitude > 0.001f)
                        return mountedDirection.normalized;

                    Vector3 horseForward =
                        horseController.transform.forward;

                    horseForward.y = 0f;

                    if (horseForward.sqrMagnitude > 0.001f)
                        return horseForward.normalized;
                }
            }

            Vector3 playerForward = player.forward;
            playerForward.y = 0f;

            if (playerForward.sqrMagnitude < 0.001f)
                return Vector3.forward;

            return playerForward.normalized;
        }

        private void OnGUI()
        {
            if (!visible)
                return;

            EnsureStyle();

            if (rooms.Count == 0)
            {
                refreshRetryTimer -= Time.deltaTime;
                if (refreshRetryTimer <= 0f)
                {
                    refreshRetryTimer = 0.5f;
                    RefreshRooms();
                }
            }

            TickPlayerDiscovery();

            Rect rect = new Rect(
                Screen.width - panel.width - marginRight,
                Screen.height - panel.height - marginBottom,
                panel.width,
                panel.height
            );

            DrawRect(rect, backgroundColor);
            GUI.Box(rect, GUIContent.none);
            GUI.Label(new Rect(rect.x + 8f, rect.y + 6f, rect.width - 16f, 22f), "Minimap / Fog of War", labelStyle);

            if (!boundsReady || rooms.Count == 0)
            {
                GUI.Label(new Rect(rect.x + 8f, rect.y + 32f, rect.width - 16f, 22f), "No minimap rooms", labelStyle);
                return;
            }

            Rect mapRect = new Rect(
                rect.x + 12f,
                rect.y + 34f,
                rect.width - 24f,
                rect.height - 46f
            );

            Matrix4x4 originalGuiMatrix = GUI.matrix;
            Vector2 rotationPivot = mapRect.center;

            if (TryResolveMapPoint(
                    mapRect,
                    player,
                    out Vector2 playerMapPoint))
            {
                rotationPivot = playerMapPoint;
            }

            if (rotateWithPlayerDirection)
            {
                GUIUtility.RotateAroundPivot(
                    currentMapRotationDegrees,
                    rotationPivot
                );
            }

            DrawRooms(mapRect);
            DrawMarker(
                mapRect,
                horse,
                horseColor,
                markerSize * 0.85f
            );

            GUI.matrix = originalGuiMatrix;

            DrawMarker(
                mapRect,
                player,
                playerColor,
                markerSize
            );

            GUI.Label(new Rect(rect.x + 8f, rect.y + rect.height - 24f, rect.width - 16f, 20f), $"Explored {CountDiscoveredRooms()}/{rooms.Count}  |  M: toggle", labelStyle);
        }

        private int CountDiscoveredRooms()
        {
            int count = 0;

            foreach (BDMinimapRoom room in rooms)
            {
                if (room != null && room.Discovered)
                    count++;
            }

            return count;
        }

        private void DrawRooms(Rect mapRect)
        {
            int widthCells = Mathf.Max(1, maxX - minX + 1);
            int heightCells = Mathf.Max(1, maxY - minY + 1);

            float cellSize = Mathf.Min(
                mapRect.width / widthCells,
                mapRect.height / heightCells
            );

            foreach (BDMinimapRoom room in rooms)
            {
                if (room == null || !room.Discovered)
                    continue;

                Rect roomRect = CellToRect(mapRect, room.Cell, cellSize);
                roomRect.x += roomGap * 0.5f;
                roomRect.y += roomGap * 0.5f;
                roomRect.width -= roomGap;
                roomRect.height -= roomGap;

                DrawRect(roomRect, discoveredRoomColor);

                if (player != null && room.ContainsWorldPosition(player.position, 1.25f))
                    DrawRoomOutline(roomRect, currentRoomColor, 2.5f);
                else
                    DrawRoomOutline(roomRect, roomOutlineColor, 1.0f);

                DrawRoomWalls(room, roomRect);
            }
        }

        private void DrawRoomOutline(Rect rect, Color color, float thickness)
        {
            thickness = Mathf.Max(1f, thickness);
            DrawRect(new Rect(rect.x, rect.y, rect.width, thickness), color);
            DrawRect(new Rect(rect.x, rect.yMax - thickness, rect.width, thickness), color);
            DrawRect(new Rect(rect.x, rect.y, thickness, rect.height), color);
            DrawRect(new Rect(rect.xMax - thickness, rect.y, thickness, rect.height), color);
        }

        private void DrawRoomWalls(BDMinimapRoom room, Rect roomRect)
        {
            float thickness = 3f;

            if (!room.NorthOpen)
                DrawRect(new Rect(roomRect.x, roomRect.y, roomRect.width, thickness), wallColor);

            if (!room.SouthOpen)
                DrawRect(new Rect(roomRect.x, roomRect.yMax - thickness, roomRect.width, thickness), wallColor);

            if (!room.WestOpen)
                DrawRect(new Rect(roomRect.x, roomRect.y, thickness, roomRect.height), wallColor);

            if (!room.EastOpen)
                DrawRect(new Rect(roomRect.xMax - thickness, roomRect.y, thickness, roomRect.height), wallColor);
        }

        private void DrawMarker(
            Rect mapRect,
            Transform target,
            Color color,
            float size)
        {
            if (!TryResolveMapPoint(
                    mapRect,
                    target,
                    out Vector2 position))
            {
                return;
            }

            DrawRect(
                new Rect(
                    position.x - size * 0.5f,
                    position.y - size * 0.5f,
                    size,
                    size
                ),
                color
            );
        }

        private bool TryResolveMapPoint(
            Rect mapRect,
            Transform target,
            out Vector2 position)
        {
            position = mapRect.center;

            if (target == null)
                return false;

            BDMinimapRoom nearest =
                FindNearestDiscoveredRoom(target.position);

            if (nearest == null)
                return false;

            Vector2 normalized =
                WorldToCellLocal(nearest, target.position);

            int widthCells =
                Mathf.Max(1, maxX - minX + 1);

            int heightCells =
                Mathf.Max(1, maxY - minY + 1);

            float cellSize = Mathf.Min(
                mapRect.width / widthCells,
                mapRect.height / heightCells
            );

            Rect roomRect =
                CellToRect(mapRect, nearest.Cell, cellSize);

            position = new Vector2(
                roomRect.x + normalized.x * roomRect.width,
                roomRect.y +
                    (1f - normalized.y) *
                    roomRect.height
            );

            return true;
        }

        private BDMinimapRoom FindNearestDiscoveredRoom(Vector3 worldPosition)
        {
            BDMinimapRoom best = null;
            float bestDistance = float.MaxValue;

            foreach (BDMinimapRoom room in rooms)
            {
                if (room == null || !room.Discovered)
                    continue;

                float distance = Vector3.SqrMagnitude(worldPosition - room.WorldCenter);
                if (distance < bestDistance)
                {
                    bestDistance = distance;
                    best = room;
                }
            }

            if (best != null)
                return best;

            return FindNearestRoom(worldPosition);
        }

        private Vector2 WorldToCellLocal(BDMinimapRoom room, Vector3 worldPosition)
        {
            Vector3 delta = worldPosition - room.WorldCenter;
            float size = Mathf.Max(1f, room.RoomSize);

            return new Vector2(
                Mathf.Clamp01((delta.x / size) + 0.5f),
                Mathf.Clamp01((delta.z / size) + 0.5f)
            );
        }

        private Rect CellToRect(Rect mapRect, Vector2Int cell, float cellSize)
        {
            int x = cell.x - minX;
            int y = maxY - cell.y;

            return new Rect(
                mapRect.x + x * cellSize,
                mapRect.y + y * cellSize,
                cellSize,
                cellSize
            );
        }

        private void DrawRect(Rect rect, Color color)
        {
            Color old = GUI.color;
            GUI.color = color;
            GUI.DrawTexture(rect, whiteTexture);
            GUI.color = old;
        }

        private void EnsureStyle()
        {
            if (labelStyle != null)
                return;

            labelStyle = new GUIStyle(GUI.skin.label)
            {
                fontSize = 12,
                alignment = TextAnchor.MiddleLeft
            };
            labelStyle.normal.textColor = Color.white;
        }

        private bool ReadTogglePressed()
        {
#if ENABLE_INPUT_SYSTEM
            if (Keyboard.current != null && Keyboard.current.mKey.wasPressedThisFrame)
                return true;
#endif

#if ENABLE_LEGACY_INPUT_MANAGER
            if (Input.GetKeyDown(KeyCode.M))
                return true;
#endif

            return false;
        }
    }
}
