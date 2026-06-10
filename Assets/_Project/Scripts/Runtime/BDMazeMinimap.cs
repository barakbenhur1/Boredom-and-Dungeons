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
        // BD OBSOLETE MINIMAP FIELDS REMOVED V2
        // BD PLAYER-UP DYNAMIC MINIMAP FIX
        [SerializeField] private bool rotateWithPlayerDirection = true;
        [SerializeField] private float rotationOffsetDegrees = 0f;
        // BD MINIMAP 90-DEGREE MOVEMENT SNAP FIX
        [SerializeField] private float movementSnapThreshold = 0.35f;
        // BD MINIMAP NEAREST-CARDINAL SECTOR FIX
        [SerializeField] private float diagonalBoundaryHoldEpsilon = 0.015f;
        [SerializeField, Range(0.12f, 0.65f)]
        private float cardinalRotationSmoothTime = 0.28f;
        [SerializeField] private float cardinalRotationMaxSpeed = 300f;
        [SerializeField] private float cardinalRotationSnapEpsilon = 0.15f;

        [Header("Colors")]
        [SerializeField] private Color backgroundColor = new Color(0f, 0f, 0f, 0.78f);
        [SerializeField] private Color unknownColor = new Color(0f, 0f, 0f, 0.0f);
        [SerializeField] private Color discoveredRoomColor = new Color(0.36f, 0.48f, 0.62f, 0.96f);
        [SerializeField] private Color wallColor = new Color(0.08f, 0.10f, 0.13f, 0.95f);
        [SerializeField] private Color playerColor = new Color(1f, 1f, 1f, 1f);
        [SerializeField] private Color horseColor = new Color(0.18f, 0.95f, 0.34f, 1f);
        [SerializeField] private Color currentRoomColor = new Color(0.55f, 0.80f, 1f, 1f);
        [SerializeField] private Color roomOutlineColor = new Color(0.12f, 0.18f, 0.24f, 0.95f);

        [Header("Dynamic Marker Language")]
        // BD MINIMAP COMBATANT MARKERS V2
        [SerializeField] private Color enemyMarkerColor =
            new Color(0.96f, 0.12f, 0.10f, 1f);
        [SerializeField] private float regularEnemyMarkerScale = 0.72f;
        [SerializeField] private float miniBossMarkerScale = 1.25f;
        [SerializeField] private float bossMarkerScale = 1.80f;
        [SerializeField] private float dynamicMarkerRefreshSeconds = 0.40f;

        [Header("Contextual Visibility")]
        [SerializeField, Range(0.20f, 0.70f)]
        private float stationaryMinimapAlpha = 0.38f;
        [SerializeField] private float stationaryDimDelay = 1.50f;
        [SerializeField] private float minimapFadeInSeconds = 0.16f;
        [SerializeField] private float minimapFadeOutSeconds = 0.34f;
        [SerializeField] private float threatWakeRadius = 14f;
        [SerializeField] private float eventWakeSeconds = 2.0f;

        private readonly List<BDMinimapRoom> rooms = new List<BDMinimapRoom>();
        private readonly List<BDHealth> dynamicCombatants =
            new List<BDHealth>(32);
        private Texture2D whiteTexture;
        private Transform player;
        private Transform horse;
        private GUIStyle labelStyle;
        private BDPlayerController playerController;
        private BDHorseController horseController;
        private float currentMapRotationDegrees;
        private float targetMapRotationDegrees;
        private float mapRotationVelocity;
        private bool hasMapRotationTarget;

        private int minX;
        private int maxX;
        private int minY;
        private int maxY;
        private bool boundsReady;
        private float refreshRetryTimer;
        private float nextDynamicMarkerRefreshAt;
        private float minimapAlpha = 1f;
        private float fullVisibilityUntil;
        private float stationaryStartedAt = -999f;
        private int lastDiscoveredRoomCount = -1;

        // BD MINIMAP OFFSCREEN RASTER CLIP V13
        private const int RasterSize = 256;
        private Texture2D minimapRasterTexture;
        private Color32[] minimapSourcePixels;
        private Color32[] minimapRotatedPixels;

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
            {
                visible = !visible;
                WakeMinimap(eventWakeSeconds);
            }

            ResolvePlayerReference();
            TickDynamicPlayerUpRotation();
            RefreshDynamicMarkersIfNeeded();
            TickContextualVisibility();

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

            if (TryResolveMovementCardinalRotation(
                    out float desiredRotation))
            {
                targetMapRotationDegrees =
                    desiredRotation +
                    rotationOffsetDegrees;

                hasMapRotationTarget = true;
            }

            if (!hasMapRotationTarget)
                return;

            currentMapRotationDegrees =
                Mathf.SmoothDampAngle(
                    currentMapRotationDegrees,
                    targetMapRotationDegrees,
                    ref mapRotationVelocity,
                    Mathf.Max(
                        0.05f,
                        cardinalRotationSmoothTime
                    ),
                    Mathf.Max(
                        90f,
                        cardinalRotationMaxSpeed
                    ),
                    Mathf.Max(
                        0f,
                        Time.unscaledDeltaTime
                    )
                );

            if (Mathf.Abs(
                    Mathf.DeltaAngle(
                        currentMapRotationDegrees,
                        targetMapRotationDegrees
                    )) <=
                Mathf.Max(
                    0.01f,
                    cardinalRotationSnapEpsilon
                ))
            {
                currentMapRotationDegrees =
                    targetMapRotationDegrees;

                mapRotationVelocity = 0f;
            }
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
            if (!visible ||
                !BDGameplayUiVisibility.IsGameplayHudVisible)
            {
                return;
            }

            EnsureStyle();
            Color previousMinimapGuiColor = GUI.color;
            GUI.color = new Color(
                previousMinimapGuiColor.r,
                previousMinimapGuiColor.g,
                previousMinimapGuiColor.b,
                previousMinimapGuiColor.a * Mathf.Clamp01(minimapAlpha)
            );

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
            Rect panelRect = ResolveScreenPanelRect();
            GUI.BeginGroup(panelRect);

            Rect localPanelRect = new Rect(
                0f,
                0f,
                panelRect.width,
                panelRect.height
            );

            DrawRect(localPanelRect, backgroundColor);
            GUI.Box(localPanelRect, GUIContent.none);

            if (!boundsReady || rooms.Count == 0)
            {
                GUI.Label(
                    new Rect(8f, 32f, localPanelRect.width - 16f, 22f),
                    "No minimap rooms",
                    labelStyle
                );
                GUI.EndGroup();
                GUI.color = previousMinimapGuiColor;
                return;
            }

            float availableMapWidth = Mathf.Max(
                24f,
                localPanelRect.width - 24f
            );
            float availableMapHeight = Mathf.Max(
                24f,
                localPanelRect.height - 76f
            );
            float mapSize = Mathf.Min(
                availableMapWidth,
                availableMapHeight
            );

            Rect localMapRect = new Rect(
                (localPanelRect.width - mapSize) * 0.5f,
                30f,
                mapSize,
                mapSize
            );

            DrawRect(
                localMapRect,
                new Color(
                    backgroundColor.r,
                    backgroundColor.g,
                    backgroundColor.b,
                    Mathf.Clamp01(backgroundColor.a + 0.12f)
                )
            );

            // One pre-clipped texture is drawn inside one fixed square. Rotation
            // happens in the pixel buffer, so no GUI matrix can escape the frame.
            EnsureMinimapRaster();
            if (Event.current == null ||
                Event.current.type == EventType.Repaint)
            {
                RebuildRotatedMinimapRaster();
            }

            GUI.BeginGroup(localMapRect);
            if (minimapRasterTexture != null)
            {
                GUI.DrawTexture(
                    new Rect(0f, 0f, mapSize, mapSize),
                    minimapRasterTexture,
                    ScaleMode.StretchToFill,
                    true
                );
            }
            GUI.EndGroup();

            DrawRoomOutline(localMapRect, roomOutlineColor, 2f);

            GUI.Label(
                new Rect(8f, 5f, localPanelRect.width - 16f, 22f),
                "Minimap / Fog of War",
                labelStyle
            );
            GUI.Label(
                new Rect(
                    8f,
                    localPanelRect.height - 24f,
                    localPanelRect.width - 16f,
                    20f
                ),
                $"Explored {CountDiscoveredRooms()}/{rooms.Count}  |  M: toggle",
                labelStyle
            );

            GUI.EndGroup();
            GUI.color = previousMinimapGuiColor;
        }

        private void EnsureMinimapRaster()
        {
            int pixelCount = RasterSize * RasterSize;
            if (minimapSourcePixels == null ||
                minimapSourcePixels.Length != pixelCount)
            {
                minimapSourcePixels = new Color32[pixelCount];
                minimapRotatedPixels = new Color32[pixelCount];
            }

            if (minimapRasterTexture != null)
                return;

            minimapRasterTexture = new Texture2D(
                RasterSize,
                RasterSize,
                TextureFormat.RGBA32,
                mipChain: false
            );
            minimapRasterTexture.name = "BD Minimap Clipped Raster V13";
            minimapRasterTexture.wrapMode = TextureWrapMode.Clamp;
            minimapRasterTexture.filterMode = FilterMode.Bilinear;
        }

        private void RebuildRotatedMinimapRaster()
        {
            if (minimapRasterTexture == null ||
                minimapSourcePixels == null ||
                minimapRotatedPixels == null)
            {
                return;
            }

            System.Array.Clear(
                minimapSourcePixels,
                0,
                minimapSourcePixels.Length
            );
            System.Array.Clear(
                minimapRotatedPixels,
                0,
                minimapRotatedPixels.Length
            );

            Rect rasterRect = new Rect(
                0f,
                0f,
                RasterSize,
                RasterSize
            );

            int widthCells = Mathf.Max(1, maxX - minX + 1);
            int heightCells = Mathf.Max(1, maxY - minY + 1);
            float cellSize = Mathf.Min(
                rasterRect.width / widthCells,
                rasterRect.height / heightCells
            );
            float rasterGap = Mathf.Max(
                1f,
                roomGap * RasterSize /
                Mathf.Max(1f, panel.width)
            );

            BDMinimapRoom currentRoom = player != null
                ? FindRoomContainingWorldPosition(player.position)
                : null;

            foreach (BDMinimapRoom room in rooms)
            {
                if (room == null || !room.Discovered)
                    continue;

                Rect roomRect = CellToRect(
                    rasterRect,
                    room.Cell,
                    cellSize
                );
                roomRect.x += rasterGap * 0.5f;
                roomRect.y += rasterGap * 0.5f;
                roomRect.width = Mathf.Max(1f, roomRect.width - rasterGap);
                roomRect.height = Mathf.Max(1f, roomRect.height - rasterGap);

                FillRasterRect(
                    minimapSourcePixels,
                    roomRect,
                    discoveredRoomColor
                );
                DrawRasterOutline(
                    minimapSourcePixels,
                    roomRect,
                    room == currentRoom
                        ? currentRoomColor
                        : roomOutlineColor,
                    room == currentRoom ? 3 : 1
                );
                DrawRasterRoomWalls(
                    minimapSourcePixels,
                    room,
                    roomRect
                );
            }

            Vector2 pivot = rasterRect.center;
            if (TryResolveMapPoint(
                    rasterRect,
                    player,
                    out Vector2 playerPoint))
            {
                pivot = playerPoint;
            }

            DrawDynamicCombatantMarkers(rasterRect);
            DrawRasterCircleMarker(
                minimapSourcePixels,
                rasterRect,
                horse,
                horseColor,
                markerSize * RasterSize /
                Mathf.Max(1f, panel.width) * 0.88f
            );
            DrawRasterMarker(
                minimapSourcePixels,
                rasterRect,
                player,
                playerColor,
                markerSize * RasterSize /
                Mathf.Max(1f, panel.width)
            );

            RotateRasterAsSingleUnit(
                minimapSourcePixels,
                minimapRotatedPixels,
                pivot,
                currentMapRotationDegrees
            );

            minimapRasterTexture.SetPixels32(minimapRotatedPixels);
            minimapRasterTexture.Apply(
                updateMipmaps: false,
                makeNoLongerReadable: false
            );
        }


        private void RefreshDynamicMarkersIfNeeded()
        {
            if (Time.unscaledTime < nextDynamicMarkerRefreshAt)
                return;

            nextDynamicMarkerRefreshAt =
                Time.unscaledTime +
                Mathf.Max(0.10f, dynamicMarkerRefreshSeconds);
            dynamicCombatants.Clear();

            BDHealth[] candidates = FindObjectsByType<BDHealth>(
                FindObjectsInactive.Exclude,
                FindObjectsSortMode.None
            );
            for (int index = 0; index < candidates.Length; index++)
            {
                BDHealth candidate = candidates[index];
                if (!IsMinimapCombatant(candidate))
                    continue;
                dynamicCombatants.Add(candidate);
            }
        }

        private static bool IsMinimapCombatant(BDHealth candidate)
        {
            if (candidate == null ||
                candidate.IsDead ||
                !candidate.gameObject.activeInHierarchy)
            {
                return false;
            }

            if (candidate.GetComponentInParent<BDPlayerMarker>() != null ||
                candidate.GetComponentInParent<BDHorseHealth>() != null)
            {
                return false;
            }

            bool combatant =
                candidate.GetComponent<CharacterController>() != null ||
                candidate.GetComponent<BDBossHealthChannel>() != null;
            if (!combatant)
                return false;

            return true;
        }

        private void DrawDynamicCombatantMarkers(Rect rasterRect)
        {
            float baseSize =
                markerSize * RasterSize /
                Mathf.Max(1f, panel.width);

            for (int index = 0; index < dynamicCombatants.Count; index++)
            {
                BDHealth health = dynamicCombatants[index];
                if (!IsMinimapCombatant(health) ||
                    !IsWorldPositionInDiscoveredRoom(
                        health.transform.position))
                {
                    continue;
                }

                BDCombatantRank rank =
                    BDCombatantProfile.ResolveRank(health);
                if (rank == BDCombatantRank.Boss)
                {
                    DrawRasterHexagonMarker(
                        minimapSourcePixels,
                        rasterRect,
                        health.transform,
                        enemyMarkerColor,
                        baseSize * Mathf.Max(1f, bossMarkerScale)
                    );
                }
                else
                {
                    float scale = rank == BDCombatantRank.MiniBoss
                        ? Mathf.Max(1f, miniBossMarkerScale)
                        : Mathf.Clamp(regularEnemyMarkerScale, 0.45f, 1f);
                    DrawRasterCircleMarker(
                        minimapSourcePixels,
                        rasterRect,
                        health.transform,
                        enemyMarkerColor,
                        baseSize * scale
                    );
                }
            }
        }

        private bool IsWorldPositionInDiscoveredRoom(
            Vector3 worldPosition)
        {
            BDMinimapRoom containing =
                FindRoomContainingWorldPosition(worldPosition);
            if (containing != null)
                return containing.Discovered;

            BDMinimapRoom nearest = FindNearestRoom(worldPosition);
            return nearest != null &&
                   nearest.Discovered &&
                   nearest.SqrDistanceToCenter(worldPosition) <=
                       nearestDiscoveryMaxDistance *
                       nearestDiscoveryMaxDistance;
        }

        private void DrawRasterCircleMarker(
            Color32[] pixels,
            Rect mapRect,
            Transform target,
            Color color,
            float size)
        {
            if (!TryResolveMapPoint(
                    mapRect,
                    target,
                    out Vector2 point))
            {
                return;
            }

            float radius = Mathf.Max(1.5f, size * 0.5f);
            int minX = Mathf.Clamp(
                Mathf.FloorToInt(point.x - radius),
                0,
                RasterSize - 1
            );
            int maxX = Mathf.Clamp(
                Mathf.CeilToInt(point.x + radius),
                0,
                RasterSize - 1
            );
            int minY = Mathf.Clamp(
                Mathf.FloorToInt(point.y - radius),
                0,
                RasterSize - 1
            );
            int maxY = Mathf.Clamp(
                Mathf.CeilToInt(point.y + radius),
                0,
                RasterSize - 1
            );
            float radiusSquared = radius * radius;
            Color32 packed = color;

            for (int y = minY; y <= maxY; y++)
            {
                for (int x = minX; x <= maxX; x++)
                {
                    float dx = x + 0.5f - point.x;
                    float dy = y + 0.5f - point.y;
                    if (dx * dx + dy * dy <= radiusSquared)
                        pixels[ToRasterIndex(x, y)] = packed;
                }
            }
        }

        private void DrawRasterHexagonMarker(
            Color32[] pixels,
            Rect mapRect,
            Transform target,
            Color color,
            float size)
        {
            if (!TryResolveMapPoint(
                    mapRect,
                    target,
                    out Vector2 point))
            {
                return;
            }

            float radius = Mathf.Max(3f, size * 0.5f);
            int minX = Mathf.Clamp(
                Mathf.FloorToInt(point.x - radius),
                0,
                RasterSize - 1
            );
            int maxX = Mathf.Clamp(
                Mathf.CeilToInt(point.x + radius),
                0,
                RasterSize - 1
            );
            int minY = Mathf.Clamp(
                Mathf.FloorToInt(point.y - radius),
                0,
                RasterSize - 1
            );
            int maxY = Mathf.Clamp(
                Mathf.CeilToInt(point.y + radius),
                0,
                RasterSize - 1
            );
            Color32 packed = color;
            float halfWidth = radius * 0.8660254f;

            for (int y = minY; y <= maxY; y++)
            {
                float dy = Mathf.Abs(y + 0.5f - point.y);
                if (dy > radius)
                    continue;
                float rowHalfWidth = dy <= radius * 0.5f
                    ? halfWidth
                    : halfWidth * (radius - dy) / (radius * 0.5f);
                for (int x = minX; x <= maxX; x++)
                {
                    if (Mathf.Abs(x + 0.5f - point.x) <= rowHalfWidth)
                        pixels[ToRasterIndex(x, y)] = packed;
                }
            }
        }

        private void TickContextualVisibility()
        {
            if (!visible)
                return;

            int discovered = CountDiscoveredRooms();
            if (lastDiscoveredRoomCount >= 0 &&
                discovered > lastDiscoveredRoomCount)
            {
                WakeMinimap(eventWakeSeconds);
            }
            lastDiscoveredRoomCount = discovered;

            bool moving = IsPlayerOrHorseMoving();
            bool threatNear = HasNearbyMinimapThreat();
            if (moving || threatNear)
            {
                stationaryStartedAt = -999f;
                WakeMinimap(eventWakeSeconds);
            }
            else if (stationaryStartedAt < 0f)
            {
                stationaryStartedAt = Time.unscaledTime;
            }

            bool idleLongEnough =
                stationaryStartedAt >= 0f &&
                Time.unscaledTime - stationaryStartedAt >=
                    Mathf.Max(0f, stationaryDimDelay);
            bool full =
                !idleLongEnough ||
                Time.unscaledTime < fullVisibilityUntil;
            float target = full
                ? 1f
                : Mathf.Clamp(stationaryMinimapAlpha, 0.20f, 0.70f);
            float duration = target > minimapAlpha
                ? Mathf.Max(0.05f, minimapFadeInSeconds)
                : Mathf.Max(0.05f, minimapFadeOutSeconds);
            minimapAlpha = Mathf.MoveTowards(
                minimapAlpha,
                target,
                Time.unscaledDeltaTime / duration
            );
        }

        private bool IsPlayerOrHorseMoving()
        {
            if (horseController != null &&
                horseController.IsMounted &&
                horseController.HasRideMoveInput)
            {
                return true;
            }

            return playerController != null &&
                   playerController.HasMoveInput;
        }

        private bool HasNearbyMinimapThreat()
        {
            if (player == null)
                return false;

            float radius = Mathf.Max(1f, threatWakeRadius);
            float radiusSquared = radius * radius;
            for (int index = 0; index < dynamicCombatants.Count; index++)
            {
                BDHealth health = dynamicCombatants[index];
                if (!IsMinimapCombatant(health))
                    continue;
                Vector3 delta = health.transform.position - player.position;
                delta.y = 0f;
                if (delta.sqrMagnitude <= radiusSquared)
                    return true;
            }
            return false;
        }

        private void WakeMinimap(float seconds)
        {
            fullVisibilityUntil = Mathf.Max(
                fullVisibilityUntil,
                Time.unscaledTime + Mathf.Max(0f, seconds)
            );
        }

        private static void RotateRasterAsSingleUnit(
            Color32[] source,
            Color32[] destination,
            Vector2 pivot,
            float degrees)
        {
            float radians = -degrees * Mathf.Deg2Rad;
            float cosine = Mathf.Cos(radians);
            float sine = Mathf.Sin(radians);

            for (int y = 0; y < RasterSize; y++)
            {
                for (int x = 0; x < RasterSize; x++)
                {
                    float dx = x + 0.5f - pivot.x;
                    float dy = y + 0.5f - pivot.y;
                    float sourceX =
                        pivot.x + dx * cosine - dy * sine;
                    float sourceY =
                        pivot.y + dx * sine + dy * cosine;

                    int sx = Mathf.FloorToInt(sourceX);
                    int sy = Mathf.FloorToInt(sourceY);
                    int destinationIndex =
                        ToRasterIndex(x, y);

                    if (sx < 0 || sx >= RasterSize ||
                        sy < 0 || sy >= RasterSize)
                    {
                        destination[destinationIndex] =
                            new Color32(0, 0, 0, 0);
                        continue;
                    }

                    destination[destinationIndex] =
                        source[ToRasterIndex(sx, sy)];
                }
            }
        }

        private void DrawRasterMarker(
            Color32[] pixels,
            Rect mapRect,
            Transform target,
            Color color,
            float size)
        {
            if (!TryResolveMapPoint(
                    mapRect,
                    target,
                    out Vector2 point))
            {
                return;
            }

            float safeSize = Mathf.Max(3f, size);
            FillRasterRect(
                pixels,
                new Rect(
                    point.x - safeSize * 0.5f,
                    point.y - safeSize * 0.5f,
                    safeSize,
                    safeSize
                ),
                color
            );
        }

        private static void DrawRasterRoomWalls(
            Color32[] pixels,
            BDMinimapRoom room,
            Rect roomRect)
        {
            const int thickness = 3;
            Color color = new Color(0.08f, 0.10f, 0.13f, 0.95f);

            if (!room.NorthOpen)
                FillRasterRect(pixels, new Rect(roomRect.x, roomRect.y, roomRect.width, thickness), color);
            if (!room.SouthOpen)
                FillRasterRect(pixels, new Rect(roomRect.x, roomRect.yMax - thickness, roomRect.width, thickness), color);
            if (!room.WestOpen)
                FillRasterRect(pixels, new Rect(roomRect.x, roomRect.y, thickness, roomRect.height), color);
            if (!room.EastOpen)
                FillRasterRect(pixels, new Rect(roomRect.xMax - thickness, roomRect.y, thickness, roomRect.height), color);
        }

        private static void DrawRasterOutline(
            Color32[] pixels,
            Rect rect,
            Color color,
            int thickness)
        {
            int safeThickness = Mathf.Max(1, thickness);
            FillRasterRect(pixels, new Rect(rect.x, rect.y, rect.width, safeThickness), color);
            FillRasterRect(pixels, new Rect(rect.x, rect.yMax - safeThickness, rect.width, safeThickness), color);
            FillRasterRect(pixels, new Rect(rect.x, rect.y, safeThickness, rect.height), color);
            FillRasterRect(pixels, new Rect(rect.xMax - safeThickness, rect.y, safeThickness, rect.height), color);
        }

        private static void FillRasterRect(
            Color32[] pixels,
            Rect rect,
            Color color)
        {
            int xMin = Mathf.Clamp(Mathf.FloorToInt(rect.xMin), 0, RasterSize);
            int xMax = Mathf.Clamp(Mathf.CeilToInt(rect.xMax), 0, RasterSize);
            int yMin = Mathf.Clamp(Mathf.FloorToInt(rect.yMin), 0, RasterSize);
            int yMax = Mathf.Clamp(Mathf.CeilToInt(rect.yMax), 0, RasterSize);
            Color32 packed = color;

            for (int y = yMin; y < yMax; y++)
            {
                for (int x = xMin; x < xMax; x++)
                    pixels[ToRasterIndex(x, y)] = packed;
            }
        }

        private static int ToRasterIndex(int x, int guiY)
        {
            int textureY = RasterSize - 1 - guiY;
            return textureY * RasterSize + x;
        }

        private void OnDestroy()
        {
            if (minimapRasterTexture != null)
            {
                Destroy(minimapRasterTexture);
                minimapRasterTexture = null;
            }
        }

       private void DrawRotatedRoomsClipped(
            Rect mapRect,
            Vector2 pivot)
        {
            int widthCells =
                Mathf.Max(1, maxX - minX + 1);
            int heightCells =
                Mathf.Max(1, maxY - minY + 1);

            float cellSize = Mathf.Min(
                mapRect.width / widthCells,
                mapRect.height / heightCells
            );

            foreach (BDMinimapRoom room in rooms)
            {
                if (room == null || !room.Discovered)
                    continue;

                Rect sourceRect =
                    CellToRect(
                        mapRect,
                        room.Cell,
                        cellSize
                    );

                sourceRect.x += roomGap * 0.5f;
                sourceRect.y += roomGap * 0.5f;
                sourceRect.width -= roomGap;
                sourceRect.height -= roomGap;

                Vector2 center = RotateMapPoint(
                    sourceRect.center,
                    pivot
                );

                Rect rotatedRect = new Rect(
                    center.x - sourceRect.width * 0.5f,
                    center.y - sourceRect.height * 0.5f,
                    sourceRect.width,
                    sourceRect.height
                );

                DrawRectClipped(
                    rotatedRect,
                    discoveredRoomColor,
                    mapRect
                );

                Color outline =
                    player != null &&
                    room.ContainsWorldPosition(
                        player.position,
                        1.25f)
                        ? currentRoomColor
                        : roomOutlineColor;

                DrawOutlineClipped(
                    rotatedRect,
                    outline,
                    player != null &&
                    room.ContainsWorldPosition(
                        player.position,
                        1.25f)
                        ? 2.5f
                        : 1f,
                    mapRect
                );

                DrawRotatedRoomWallsClipped(
                    room,
                    sourceRect,
                    pivot,
                    mapRect
                );
            }
        }

        private void DrawRotatedRoomWallsClipped(
            BDMinimapRoom room,
            Rect sourceRect,
            Vector2 pivot,
            Rect clipRect)
        {
            if (!room.NorthOpen)
            {
                DrawRotatedEdgeClipped(
                    new Vector2(
                        sourceRect.xMin,
                        sourceRect.yMin),
                    new Vector2(
                        sourceRect.xMax,
                        sourceRect.yMin),
                    pivot,
                    clipRect
                );
            }

            if (!room.SouthOpen)
            {
                DrawRotatedEdgeClipped(
                    new Vector2(
                        sourceRect.xMin,
                        sourceRect.yMax),
                    new Vector2(
                        sourceRect.xMax,
                        sourceRect.yMax),
                    pivot,
                    clipRect
                );
            }

            if (!room.WestOpen)
            {
                DrawRotatedEdgeClipped(
                    new Vector2(
                        sourceRect.xMin,
                        sourceRect.yMin),
                    new Vector2(
                        sourceRect.xMin,
                        sourceRect.yMax),
                    pivot,
                    clipRect
                );
            }

            if (!room.EastOpen)
            {
                DrawRotatedEdgeClipped(
                    new Vector2(
                        sourceRect.xMax,
                        sourceRect.yMin),
                    new Vector2(
                        sourceRect.xMax,
                        sourceRect.yMax),
                    pivot,
                    clipRect
                );
            }
        }

        private void DrawRotatedEdgeClipped(
            Vector2 start,
            Vector2 end,
            Vector2 pivot,
            Rect clipRect)
        {
            start = RotateMapPoint(start, pivot);
            end = RotateMapPoint(end, pivot);

            const float thickness = 3f;

            Rect edge;

            if (Mathf.Abs(end.x - start.x) >=
                Mathf.Abs(end.y - start.y))
            {
                edge = new Rect(
                    Mathf.Min(start.x, end.x),
                    Mathf.Min(start.y, end.y) -
                        thickness * 0.5f,
                    Mathf.Abs(end.x - start.x),
                    thickness
                );
            }
            else
            {
                edge = new Rect(
                    Mathf.Min(start.x, end.x) -
                        thickness * 0.5f,
                    Mathf.Min(start.y, end.y),
                    thickness,
                    Mathf.Abs(end.y - start.y)
                );
            }

            DrawRectClipped(
                edge,
                wallColor,
                clipRect
            );
        }

        private void DrawRotatedMarkerClipped(
            Rect mapRect,
            Vector2 pivot,
            Transform target,
            Color color,
            float size)
        {
            if (!TryResolveMapPoint(
                    mapRect,
                    target,
                    out Vector2 point))
            {
                return;
            }

            point = RotateMapPoint(point, pivot);

            float half = Mathf.Max(1f, size * 0.5f);

            point.x = Mathf.Clamp(
                point.x,
                mapRect.xMin + half,
                mapRect.xMax - half
            );

            point.y = Mathf.Clamp(
                point.y,
                mapRect.yMin + half,
                mapRect.yMax - half
            );

            DrawRectClipped(
                new Rect(
                    point.x - half,
                    point.y - half,
                    half * 2f,
                    half * 2f
                ),
                color,
                mapRect
            );
        }

        private Vector2 RotateMapPoint(
            Vector2 point,
            Vector2 pivot)
        {
            float radians =
                currentMapRotationDegrees *
                Mathf.Deg2Rad;

            float cosine = Mathf.Cos(radians);
            float sine = Mathf.Sin(radians);
            Vector2 delta = point - pivot;

            return pivot + new Vector2(
                delta.x * cosine -
                delta.y * sine,
                delta.x * sine +
                delta.y * cosine
            );
        }

        private void DrawOutlineClipped(
            Rect rect,
            Color color,
            float thickness,
            Rect clipRect)
        {
            thickness = Mathf.Max(1f, thickness);

            DrawRectClipped(
                new Rect(
                    rect.x,
                    rect.y,
                    rect.width,
                    thickness),
                color,
                clipRect
            );

            DrawRectClipped(
                new Rect(
                    rect.x,
                    rect.yMax - thickness,
                    rect.width,
                    thickness),
                color,
                clipRect
            );

            DrawRectClipped(
                new Rect(
                    rect.x,
                    rect.y,
                    thickness,
                    rect.height),
                color,
                clipRect
            );

            DrawRectClipped(
                new Rect(
                    rect.xMax - thickness,
                    rect.y,
                    thickness,
                    rect.height),
                color,
                clipRect
            );
        }

        private void DrawRectClipped(
            Rect rect,
            Color color,
            Rect clipRect)
        {
            Rect clipped = IntersectRects(
                rect,
                clipRect
            );

            if (clipped.width <= 0f ||
                clipped.height <= 0f)
            {
                return;
            }

            DrawRect(clipped, color);
        }

        private static Rect IntersectRects(
            Rect first,
            Rect second)
        {
            float xMin = Mathf.Max(
                first.xMin,
                second.xMin
            );

            float yMin = Mathf.Max(
                first.yMin,
                second.yMin
            );

            float xMax = Mathf.Min(
                first.xMax,
                second.xMax
            );

            float yMax = Mathf.Min(
                first.yMax,
                second.yMax
            );

            if (xMax <= xMin || yMax <= yMin)
                return Rect.zero;

            return Rect.MinMaxRect(
                xMin,
                yMin,
                xMax,
                yMax
            );
        }

        private Rect ResolveScreenPanelRect()
        {
            float availableWidth = Mathf.Max(
                120f,
                Screen.width -
                Mathf.Max(0f, marginRight) -
                4f
            );

            float availableHeight = Mathf.Max(
                120f,
                Screen.height -
                Mathf.Max(0f, marginBottom) -
                4f
            );

            float width = Mathf.Clamp(
                panel.width,
                120f,
                availableWidth
            );

            float height = Mathf.Clamp(
                panel.height,
                120f,
                availableHeight
            );

            float x = Mathf.Clamp(
                Screen.width -
                width -
                Mathf.Max(0f, marginRight),
                0f,
                Mathf.Max(0f, Screen.width - width)
            );

            float y = Mathf.Clamp(
                Screen.height -
                height -
                Mathf.Max(0f, marginBottom),
                0f,
                Mathf.Max(0f, Screen.height - height)
            );

            return new Rect(
                x,
                y,
                width,
                height
            );
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
