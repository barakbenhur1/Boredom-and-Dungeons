using UnityEngine;

namespace BoredomAndDungeons
{
    [DefaultExecutionOrder(5000)]
    [DisallowMultipleComponent]
    public sealed class BDCameraFollow : MonoBehaviour
    {
        // BD SINGLE CAMERA TRANSFORM OWNER V23
        // BDCameraFollow is the only normal-gameplay writer of the Main Camera transform.
        [Header("Target")]
        [SerializeField] private Transform target;
        [SerializeField] private bool preferMountedHorseTarget = true;

        [Header("Locked Behind Camera")]
        [SerializeField] private bool lockedBehindTarget = true;
        [SerializeField] private float distanceBehind = 15.25f;
        [SerializeField] private float height = 17.75f;
        [SerializeField] private float lookAhead = 8.25f;
        // BD FORWARD SCREEN SPACE LOOKAHEAD FIX
        [SerializeField] private float extraForwardCompositionLookAhead = 2.35f;
        // BD EXPLICIT 40-BEHIND 60-AHEAD COMPOSITION V23R8
        [SerializeField, Range(0.30f, 0.48f)]
        private float targetViewportHeight01 = 0.40f;
        [SerializeField] private float followSmooth = 9.5f;
        [SerializeField] private float rotationSmooth = 8.5f;

        [Header("Mouse / Player Intent Rotation")]
        // BD OBSOLETE CAMERA FIELDS REMOVED V2
        [SerializeField] private float cameraYawDegreesPerSecond = 86f;

        [Header("Room Boundary Camera")]
        // BD ROOM WALL CAMERA STOP V7
        // BD CLOSED-WALL FRUSTUM CONTAINMENT V20
        // BD CLOSED-WALL HEIGHT + SMOOTH ROOM HANDOFF V21
        // BD DISTANCE-PRESERVING UNION ROOM HANDOFF V22
        // BD STABLE WALL PRESSURE CAMERA V23R4
        // BD ACTUAL-POSE ROOM HANDOFF RELEASE V23R6
        [SerializeField] private bool stopAtClosedRoomWalls = true;
        [SerializeField] private float roomBoundaryInset = 0.80f;
        [SerializeField] private float cameraFrustumSafetyInset = 1.20f;
        [SerializeField] private float constrainedLookPointSmooth = 12f;
        [SerializeField] private float wallCollisionRadius = 0.55f;
        [SerializeField] private float wallCollisionPadding = 0.35f;
        // BD UNUSED CAMERA DISTANCE FIELD REMOVED V22R2
        [SerializeField] private float roomResolveInterval = 0.12f;
        [SerializeField] private float roomCacheRefreshInterval = 1.0f;
        [SerializeField] private float roomSwitchInteriorMargin = 1.25f;
        [SerializeField] private float roomHandoffReleaseMargin = 2.25f;
        [SerializeField] private LayerMask roomBoundaryCollisionMask = ~0;

        [Header("Shake")]
        [SerializeField] private float cameraShakeMultiplier = 0.42f;
        [SerializeField] private float cameraShakeSmoothing = 18f;

        [Header("Angle")]
        [SerializeField] private float minPitch = 35f;
        [SerializeField] private float maxPitch = 68f;

        [Header("Debug")]
        [SerializeField] private bool showDebugOverlay = false;

        private Transform player;
        private BDPlayerController playerController;
        private BDHorseController horseController;
        private BDHorseHealth horseHealth;

        private Vector3 lastForward = Vector3.forward;
        private Vector3 smoothedShakeOffset;
        private Vector3 smoothedLookPoint;
        private bool smoothedLookPointReady;
        private string cameraState = "locked behind movement";
        private float nextTargetResolveAt;
        private BDMinimapRoom currentCameraRoom;
        private BDMinimapRoom previousCameraRoom;
        private bool roomHandoffActive;
        private float nextRoomResolveAt;
        private float nextRoomCacheRefreshAt;
        private int lastRoomResolveFrame = -1;
        private BDMinimapRoom[] cachedRooms = new BDMinimapRoom[0];
        private BDCameraTransitionDiagnostics transitionDiagnostics;
        private Camera attachedCamera;
        private bool handoffWallCastAppliedThisFrame;

        public void SetTarget(Transform newTarget)
        {
            target = newTarget;

            if (target == null)
                return;

            BDPlayerMarker marker = target.GetComponent<BDPlayerMarker>();
            if (marker != null)
            {
                player = target;
                playerController = target.GetComponent<BDPlayerController>();
            }

            BDHorseController horse = target.GetComponent<BDHorseController>();
            if (horse != null)
            {
                horseController = horse;
                horseHealth = target.GetComponent<BDHorseHealth>();
            }

            InitializeForwardFromTarget();
            SnapToTarget();
        }

        private void OnEnable()
        {
            // BD LEGACY SERIALIZED COMPOSITION MIGRATION V23R8
            // Existing scene instances may still serialize the old 50-degree
            // minimum pitch. Bound it here so the explicit 40/60 composition
            // works without regenerating or rewriting the authoritative scene.
            minPitch = Mathf.Min(minPitch, 35f);
            targetViewportHeight01 = Mathf.Clamp(
                targetViewportHeight01,
                0.30f,
                0.48f
            );

            smoothedLookPointReady = false;
            lastRoomResolveFrame = -1;
            nextRoomResolveAt = 0f;
            nextRoomCacheRefreshAt = 0f;

            if (Application.isPlaying)
                EnsureTransitionDiagnostics();
        }

        private void Start()
        {
            ResolveTargets();
            InitializeForwardFromTarget();

            if (target != null)
                SnapToTarget();
        }


        private void LateUpdate()
        {
            Vector3 cameraPositionBeforeFollow = transform.position;
            Quaternion cameraRotationBeforeFollow = transform.rotation;
            ResolveTargets();

            if (target == null)
                return;

            if (BDRunPresentationCoordinator.InputLocked)
            {
                cameraState = "run presentation camera lock";
                return;
            }

            UpdateCameraForwardFromPlayerIntent();

            if (!lockedBehindTarget)
            {
                FollowFixedOffset(
                    cameraPositionBeforeFollow,
                    cameraRotationBeforeFollow
                );
                return;
            }

            FollowLockedBehind(
                lastForward,
                cameraPositionBeforeFollow,
                cameraRotationBeforeFollow
            );
        }


        private void ResolveTargets()
        {
            if (Application.isPlaying && Time.time < nextTargetResolveAt)
            {
                if (preferMountedHorseTarget && horseController != null && horseController.IsMounted)
                    target = horseController.transform;
                else if (player != null)
                    target = player;

                return;
            }

            nextTargetResolveAt = Application.isPlaying ? Time.time + 0.20f : 0f;

            if (player == null)
            {
                player = BDTargetFinder.FindPlayer();
                if (player != null)
                    playerController = player.GetComponent<BDPlayerController>();
            }

            if (horseHealth == null)
                horseHealth = FindFirstObjectByType<BDHorseHealth>();

            if (horseHealth != null && horseController == null)
                horseController = horseHealth.GetComponent<BDHorseController>();

            if (preferMountedHorseTarget && horseController != null && horseController.IsMounted)
            {
                target = horseController.transform;
                return;
            }

            if (player != null)
                target = player;
        }

        private void InitializeForwardFromTarget()
        {
            Vector3 forward = Vector3.zero;

            if (target != null)
                forward = target.forward;

            forward.y = 0f;

            if (forward.sqrMagnitude < 0.001f)
                forward = lastForward.sqrMagnitude > 0.001f ? lastForward : Vector3.forward;

            lastForward = forward.normalized;
        }

        // BD CAMERA FOLLOWS MOUSE INTENT, NOT MOVEMENT
        // BD STABLE SINGLE-STAGE CAMERA YAW V23
        private void UpdateCameraForwardFromPlayerIntent()
        {
            Vector3 intent = ResolveCameraIntentDirection();
            intent.y = 0f;

            if (intent.sqrMagnitude < 0.001f)
            {
                cameraState = "stable: no new mouse intent";
                return;
            }

            intent.Normalize();

            float sensitivity = Mathf.Clamp(
                BDGameSettings.MouseSensitivityMultiplier,
                0.55f,
                1.45f
            );
            float maximumRadians =
                Mathf.Deg2Rad *
                Mathf.Max(1f, cameraYawDegreesPerSecond) *
                sensitivity *
                Time.deltaTime;

            lastForward = Vector3.RotateTowards(
                lastForward.sqrMagnitude > 0.001f
                    ? lastForward.normalized
                    : intent,
                intent,
                maximumRadians,
                0f
            );
            lastForward.y = 0f;

            if (lastForward.sqrMagnitude < 0.001f)
                lastForward = intent;

            lastForward.Normalize();
            cameraState = "single-owner stable mouse/player aim";
        }


        private Vector3 ResolveCameraIntentDirection()
        {
            if (horseController != null && horseController.IsMounted)
            {
                Vector3 mountedAim =
                    horseController.LastMountedAimDirection;

                mountedAim.y = 0f;

                return mountedAim.sqrMagnitude > 0.001f
                    ? mountedAim.normalized
                    : Vector3.zero;
            }

            if (playerController == null)
                return Vector3.zero;

            Vector3 lookDirection =
                playerController.LastLookDirection;

            lookDirection.y = 0f;

            return lookDirection.sqrMagnitude > 0.001f
                ? lookDirection.normalized
                : Vector3.zero;
        }


        private void FollowLockedBehind(
            Vector3 forward,
            Vector3 cameraPositionBeforeFollow,
            Quaternion cameraRotationBeforeFollow)
        {
            if (forward.sqrMagnitude < 0.001f)
                forward = Vector3.forward;

            forward.y = 0f;
            forward.Normalize();

            Vector3 targetPosition = target.position;
            ResolveCurrentCameraRoom(targetPosition);
            Vector3 shakeOffset = ResolvePlanarCameraShake();
            handoffWallCastAppliedThisFrame = false;

            Vector3 rawDesiredPosition =
                targetPosition -
                forward * distanceBehind +
                Vector3.up * height +
                shakeOffset;
            Vector3 desiredPosition = ResolveRoomBoundaryConstrainedPosition(
                targetPosition,
                rawDesiredPosition,
                true
            );

            Vector3 blendedPosition = Vector3.Lerp(
                transform.position,
                desiredPosition,
                1f - Mathf.Exp(-followSmooth * Time.deltaTime)
            );
            Vector3 finalPosition = ResolveRoomBoundaryConstrainedPosition(
                targetPosition,
                blendedPosition,
                false
            );

            Vector3 rawDesiredLookPoint =
                targetPosition +
                forward *
                (lookAhead + Mathf.Max(0f, extraForwardCompositionLookAhead));
            Vector3 desiredLookPoint = ResolveRoomBoundaryConstrainedLookPoint(
                targetPosition,
                rawDesiredLookPoint
            );
            Vector3 lookPoint = ResolveSmoothedLookPoint(
                targetPosition,
                desiredLookPoint
            );

            Quaternion desiredRotation = Quaternion.LookRotation(
                lookPoint - finalPosition,
                Vector3.up
            );
            Vector3 euler = desiredRotation.eulerAngles;
            euler.x = ResolveScreenCompositionPitch(
                targetPosition,
                finalPosition
            );
            euler.z = 0f;
            desiredRotation = Quaternion.Euler(euler);

            Quaternion finalRotation = Quaternion.Slerp(
                transform.rotation,
                desiredRotation,
                1f - Mathf.Exp(-rotationSmooth * Time.deltaTime)
            );

            // Apply one final room-safe pose after all smoothing and shake.
            transform.SetPositionAndRotation(finalPosition, finalRotation);
            TryCompleteRoomHandoffAfterFinalPose(
                targetPosition,
                finalPosition,
                lookPoint
            );

            CaptureTransitionDiagnostics(
                "locked-behind",
                cameraPositionBeforeFollow,
                cameraRotationBeforeFollow,
                targetPosition,
                rawDesiredPosition,
                desiredPosition,
                blendedPosition,
                finalPosition,
                rawDesiredLookPoint,
                desiredLookPoint,
                lookPoint,
                desiredRotation,
                finalRotation
            );
        }



        private void FollowFixedOffset(
            Vector3 cameraPositionBeforeFollow,
            Quaternion cameraRotationBeforeFollow)
        {
            Vector3 targetPosition = target.position;
            ResolveCurrentCameraRoom(targetPosition);
            Vector3 offset = new Vector3(0f, height, -distanceBehind);
            handoffWallCastAppliedThisFrame = false;
            Vector3 rawDesiredPosition =
                targetPosition + offset + ResolvePlanarCameraShake();
            Vector3 desiredPosition = ResolveRoomBoundaryConstrainedPosition(
                targetPosition,
                rawDesiredPosition,
                true
            );
            Vector3 blended = Vector3.Lerp(
                transform.position,
                desiredPosition,
                1f - Mathf.Exp(-followSmooth * Time.deltaTime)
            );
            Vector3 finalPosition = ResolveRoomBoundaryConstrainedPosition(
                targetPosition,
                blended,
                false
            );

            Vector3 rawDesiredLookPoint =
                targetPosition + Vector3.up * 0.5f;
            Vector3 desiredLookPoint = ResolveRoomBoundaryConstrainedLookPoint(
                targetPosition,
                rawDesiredLookPoint
            );
            Vector3 lookPoint = ResolveSmoothedLookPoint(
                targetPosition,
                desiredLookPoint
            );
            Quaternion desiredRotation = Quaternion.LookRotation(
                lookPoint - finalPosition,
                Vector3.up
            );
            transform.SetPositionAndRotation(
                finalPosition,
                Quaternion.Slerp(
                    transform.rotation,
                    desiredRotation,
                    1f - Mathf.Exp(-rotationSmooth * Time.deltaTime)
                )
            );
            cameraState = "fixed room-contained fallback";
            TryCompleteRoomHandoffAfterFinalPose(
                targetPosition,
                finalPosition,
                lookPoint
            );

            CaptureTransitionDiagnostics(
                "fixed-offset",
                cameraPositionBeforeFollow,
                cameraRotationBeforeFollow,
                targetPosition,
                rawDesiredPosition,
                desiredPosition,
                blended,
                finalPosition,
                rawDesiredLookPoint,
                desiredLookPoint,
                lookPoint,
                desiredRotation,
                transform.rotation
            );
        }

        // BD PLANAR COMBAT SHAKE V23
        private Vector3 ResolvePlanarCameraShake()
        {
            Vector3 sample =
                BDGameFeelEvents.SampleCameraShakeOffset() *
                cameraShakeMultiplier;

            Vector3 cameraRight = transform.right;
            cameraRight.y = 0f;
            if (cameraRight.sqrMagnitude < 0.001f)
                cameraRight = Vector3.right;
            cameraRight.Normalize();

            Vector3 cameraForward = transform.forward;
            cameraForward.y = 0f;
            if (cameraForward.sqrMagnitude < 0.001f)
                cameraForward = lastForward;
            if (cameraForward.sqrMagnitude < 0.001f)
                cameraForward = Vector3.forward;
            cameraForward.Normalize();

            // Camera shake stays on the gameplay plane. Enemy hits must not
            // make the camera bob vertically or change its pitch/sensitivity.
            Vector3 planar =
                cameraRight * sample.x +
                cameraForward * sample.y * 0.35f;
            planar.y = 0f;

            smoothedShakeOffset = Vector3.Lerp(
                smoothedShakeOffset,
                planar,
                1f - Mathf.Exp(-cameraShakeSmoothing * Time.deltaTime)
            );
            smoothedShakeOffset.y = 0f;
            return smoothedShakeOffset;
        }


        private void SnapToTarget()
        {
            if (target == null)
                return;

            Vector3 forward = lastForward;

            if (forward.sqrMagnitude < 0.001f)
                forward = target.forward;

            forward.y = 0f;

            if (forward.sqrMagnitude < 0.001f)
                forward = Vector3.forward;

            lastForward = forward.normalized;

            Vector3 snappedPosition =
                target.position -
                lastForward * distanceBehind +
                Vector3.up * height;
            ResolveCurrentCameraRoom(target.position);
            transform.position = ResolveRoomBoundaryConstrainedPosition(
                target.position,
                snappedPosition,
                true
            );
            Vector3 lookPoint = ResolveRoomBoundaryConstrainedLookPoint(
                target.position,
                target.position + lastForward * lookAhead
            );
            smoothedLookPoint = lookPoint;
            smoothedLookPointReady = true;
            Quaternion snappedRotation = Quaternion.LookRotation(
                lookPoint - transform.position,
                Vector3.up
            );
            Vector3 snappedEuler = snappedRotation.eulerAngles;
            snappedEuler.x = ResolveScreenCompositionPitch(
                target.position,
                transform.position
            );
            snappedEuler.z = 0f;
            transform.rotation = Quaternion.Euler(snappedEuler);
        }

        private float ResolveScreenCompositionPitch(
            Vector3 targetPosition,
            Vector3 cameraPosition)
        {
            Vector3 delta = targetPosition - cameraPosition;
            float horizontalDistance = new Vector2(
                delta.x,
                delta.z
            ).magnitude;

            if (horizontalDistance < 0.01f)
                return Mathf.Clamp(minPitch, 1f, maxPitch);

            float targetDownPitch = Mathf.Atan2(
                cameraPosition.y - targetPosition.y,
                horizontalDistance
            ) * Mathf.Rad2Deg;

            Camera cameraComponent = attachedCamera;
            if (cameraComponent == null)
                cameraComponent = GetComponent<Camera>();

            float verticalFov = cameraComponent != null &&
                                !cameraComponent.orthographic
                ? cameraComponent.fieldOfView
                : 58f;

            float viewportY = Mathf.Clamp(
                targetViewportHeight01,
                0.30f,
                0.48f
            );
            float normalizedScreenY = viewportY * 2f - 1f;
            float targetAngleFromCenter = Mathf.Atan(
                normalizedScreenY *
                Mathf.Tan(verticalFov * 0.5f * Mathf.Deg2Rad)
            ) * Mathf.Rad2Deg;

            return Mathf.Clamp(
                targetDownPitch + targetAngleFromCenter,
                minPitch,
                maxPitch
            );
        }

        private Vector3 ResolveRoomBoundaryConstrainedPosition(
            Vector3 targetPosition,
            Vector3 desiredPosition,
            bool allowHandoffWallCast)
        {
            if (!stopAtClosedRoomWalls)
                return desiredPosition;

            ResolveCurrentCameraRoom(targetPosition);
            float safeInset = ResolveStableCameraSafetyInset();
            desiredPosition = ClampPointToClosedRoomBounds(
                desiredPosition,
                safeInset
            );

            // BD ROOM WALL CAST ONLY DURING HANDOFF V23R4
            // Room bounds already keep the camera inside a stable room. Recasting
            // against tall wall colliders during every ordinary follow frame made
            // tiny hit-distance changes look like zoom pulses. The physical cast is
            // retained only while two room bounds are temporarily joined.
            if (allowHandoffWallCast &&
                roomHandoffActive &&
                previousCameraRoom != null)
            {
                Vector3 castOrigin = targetPosition + Vector3.up * 1.65f;
                Vector3 castDelta = desiredPosition - castOrigin;
                float distance = castDelta.magnitude;
                if (distance > 0.001f &&
                    Physics.SphereCast(
                        castOrigin,
                        Mathf.Max(0.05f, wallCollisionRadius),
                        castDelta / distance,
                        out RaycastHit hit,
                        distance,
                        roomBoundaryCollisionMask,
                        QueryTriggerInteraction.Ignore) &&
                    IsRoomBoundaryCollider(hit.collider))
                {
                    float allowedDistance = Mathf.Clamp(
                        hit.distance - Mathf.Max(0f, wallCollisionPadding),
                        0.35f,
                        distance
                    );
                    desiredPosition =
                        castOrigin + castDelta.normalized * allowedDistance;
                    cameraState = "handoff wall cast containment";
                    handoffWallCastAppliedThisFrame = true;
                }
            }

            return ClampPointToClosedRoomBounds(
                desiredPosition,
                safeInset
            );
        }



        // BD INDEPENDENT LOOK POINT SOFT CONSTRAINT V23R4
        private Vector3 ResolveRoomBoundaryConstrainedLookPoint(
            Vector3 targetPosition,
            Vector3 lookPoint)
        {
            if (!stopAtClosedRoomWalls)
                return lookPoint;

            ResolveCurrentCameraRoom(targetPosition);
            return ClampPointToClosedRoomBounds(
                lookPoint,
                Mathf.Max(0.05f, roomBoundaryInset)
            );
        }

        private Vector3 ResolveSmoothedLookPoint(
            Vector3 targetPosition,
            Vector3 desiredLookPoint)
        {
            if (!smoothedLookPointReady || !Application.isPlaying)
            {
                smoothedLookPoint = desiredLookPoint;
                smoothedLookPointReady = true;
            }
            else
            {
                float blend = 1f - Mathf.Exp(
                    -Mathf.Max(0.1f, constrainedLookPointSmooth) *
                    Time.deltaTime
                );
                smoothedLookPoint = Vector3.Lerp(
                    smoothedLookPoint,
                    desiredLookPoint,
                    blend
                );
            }

            return ResolveRoomBoundaryConstrainedLookPoint(
                targetPosition,
                smoothedLookPoint
            );
        }

        private float ResolveStableCameraSafetyInset()
        {
            // The previous serialized 2.25-3.40 inset made the usable half-room
            // smaller than the 15.25 camera boom even at room center. Keep enough
            // room for the cast radius and near plane without continuously shortening
            // the normal camera distance.
            return Mathf.Clamp(
                cameraFrustumSafetyInset,
                0.90f,
                1.35f
            );
        }



        // BD SINGLE ROOM SCAN PER FRAME V23R4
        private void ResolveCurrentCameraRoom(Vector3 targetPosition)
        {
            if (Application.isPlaying && lastRoomResolveFrame == Time.frameCount)
                return;

            lastRoomResolveFrame = Time.frameCount;

            if (Application.isPlaying &&
                currentCameraRoom != null &&
                Time.unscaledTime < nextRoomResolveAt &&
                currentCameraRoom.ContainsWorldPosition(targetPosition, 0.02f))
            {
                return;
            }

            BDMinimapRoom[] rooms = ResolveCachedRooms();

            BDMinimapRoom containing = null;
            float containingDistance = float.PositiveInfinity;
            BDMinimapRoom nearest = null;
            float nearestDistance = float.PositiveInfinity;

            foreach (BDMinimapRoom room in rooms)
            {
                if (room == null)
                    continue;

                float distance = room.SqrDistanceToCenter(targetPosition);
                if (room.ContainsWorldPosition(targetPosition, 0.02f) &&
                    distance < containingDistance)
                {
                    containing = room;
                    containingDistance = distance;
                }

                if (distance < nearestDistance)
                {
                    nearest = room;
                    nearestDistance = distance;
                }
            }

            if (currentCameraRoom == null)
            {
                currentCameraRoom = containing != null ? containing : nearest;
                previousCameraRoom = null;
                roomHandoffActive = false;
            }
            else if (containing != null && containing != currentCameraRoom &&
                     IsInsideRoomInterior(
                         containing,
                         targetPosition,
                         Mathf.Max(0.05f, roomSwitchInteriorMargin)))
            {
                if (roomHandoffActive && containing == previousCameraRoom)
                {
                    BDMinimapRoom oldCurrent = currentCameraRoom;
                    currentCameraRoom = previousCameraRoom;
                    previousCameraRoom = oldCurrent;
                    cameraState = "reversed union room handoff";
                }
                else
                {
                    previousCameraRoom = currentCameraRoom;
                    currentCameraRoom = containing;
                    roomHandoffActive = previousCameraRoom != null;
                    cameraState = "union room boundary handoff";
                }
            }
            else if (containing == null && nearest != null &&
                     !currentCameraRoom.ContainsWorldPosition(
                         targetPosition,
                         Mathf.Max(0.10f, roomSwitchInteriorMargin)))
            {
                previousCameraRoom = currentCameraRoom;
                currentCameraRoom = nearest;
                roomHandoffActive = previousCameraRoom != null;
                cameraState = "recovered union room boundary";
            }

            nextRoomResolveAt = Time.unscaledTime +
                Mathf.Max(0.01f, roomResolveInterval);
        }

        private BDMinimapRoom[] ResolveCachedRooms()
        {
            bool refresh =
                cachedRooms == null ||
                cachedRooms.Length == 0 ||
                !Application.isPlaying ||
                Time.unscaledTime >= nextRoomCacheRefreshAt;

            if (refresh)
            {
                cachedRooms = FindObjectsByType<BDMinimapRoom>(
                    FindObjectsSortMode.None
                );
                nextRoomCacheRefreshAt = Application.isPlaying
                    ? Time.unscaledTime +
                      Mathf.Max(0.10f, roomCacheRefreshInterval)
                    : 0f;
            }

            return cachedRooms;
        }


        // The previous-room union is released from the actual final pose,
        // never from the unsmoothed desired position. This prevents the next
        // frame from clamping the camera body or look point into the new room.
        private void TryCompleteRoomHandoffAfterFinalPose(
            Vector3 targetPosition,
            Vector3 actualCameraPosition,
            Vector3 actualLookPoint)
        {
            if (!roomHandoffActive || currentCameraRoom == null)
                return;

            float targetMargin = Mathf.Max(
                roomSwitchInteriorMargin,
                roomHandoffReleaseMargin
            );
            if (!IsInsideRoomInterior(
                    currentCameraRoom,
                    targetPosition,
                    targetMargin))
            {
                return;
            }

            if (!IsPointInsideRoomBounds(
                    currentCameraRoom,
                    actualCameraPosition,
                    ResolveStableCameraSafetyInset()))
            {
                return;
            }

            if (!IsPointInsideRoomBounds(
                    currentCameraRoom,
                    actualLookPoint,
                    Mathf.Max(0.05f, roomBoundaryInset)))
            {
                return;
            }

            roomHandoffActive = false;
            previousCameraRoom = null;
            cameraState = "completed actual-pose room handoff";
        }

        private static bool IsInsideRoomInterior(
            BDMinimapRoom room,
            Vector3 worldPosition,
            float interiorMargin)
        {
            if (room == null)
                return false;

            float halfSize = Mathf.Max(
                0.25f,
                room.RoomSize * 0.5f - Mathf.Max(0f, interiorMargin)
            );
            Vector3 delta = worldPosition - room.WorldCenter;
            return Mathf.Abs(delta.x) <= halfSize &&
                   Mathf.Abs(delta.z) <= halfSize;
        }

        private static bool IsPointInsideRoomBounds(
            BDMinimapRoom room,
            Vector3 point,
            float inset)
        {
            if (room == null)
                return false;

            float halfSize = Mathf.Max(
                0.5f,
                room.RoomSize * 0.5f - Mathf.Max(0f, inset)
            );
            Vector3 delta = point - room.WorldCenter;
            return Mathf.Abs(delta.x) <= halfSize &&
                   Mathf.Abs(delta.z) <= halfSize;
        }

        private Vector3 ClampPointToClosedRoomBounds(
            Vector3 point,
            float inset)
        {
            if (currentCameraRoom == null)
                return point;

            ResolveCameraClampBounds(
                inset,
                out float minX,
                out float maxX,
                out float minZ,
                out float maxZ
            );

            point.x = Mathf.Clamp(point.x, minX, maxX);
            point.z = Mathf.Clamp(point.z, minZ, maxZ);
            return point;
        }

        private void ResolveCameraClampBounds(
            float inset,
            out float minX,
            out float maxX,
            out float minZ,
            out float maxZ)
        {
            float currentHalf = Mathf.Max(
                0.5f,
                currentCameraRoom.RoomSize * 0.5f
            );
            minX = currentCameraRoom.WorldCenter.x - currentHalf;
            maxX = currentCameraRoom.WorldCenter.x + currentHalf;
            minZ = currentCameraRoom.WorldCenter.z - currentHalf;
            maxZ = currentCameraRoom.WorldCenter.z + currentHalf;

            if (roomHandoffActive && previousCameraRoom != null)
            {
                float previousHalf = Mathf.Max(
                    0.5f,
                    previousCameraRoom.RoomSize * 0.5f
                );
                minX = Mathf.Min(
                    minX,
                    previousCameraRoom.WorldCenter.x - previousHalf
                );
                maxX = Mathf.Max(
                    maxX,
                    previousCameraRoom.WorldCenter.x + previousHalf
                );
                minZ = Mathf.Min(
                    minZ,
                    previousCameraRoom.WorldCenter.z - previousHalf
                );
                maxZ = Mathf.Max(
                    maxZ,
                    previousCameraRoom.WorldCenter.z + previousHalf
                );
            }

            float safeInset = Mathf.Max(0f, inset);
            minX += safeInset;
            maxX -= safeInset;
            minZ += safeInset;
            maxZ -= safeInset;

            if (minX > maxX)
            {
                float center = (minX + maxX) * 0.5f;
                minX = center;
                maxX = center;
            }
            if (minZ > maxZ)
            {
                float center = (minZ + maxZ) * 0.5f;
                minZ = center;
                maxZ = center;
            }
        }






        private static bool IsRoomBoundaryCollider(
            Collider candidate)
        {
            if (candidate == null || candidate.isTrigger)
                return false;

            if (candidate.GetComponentInParent<BDWallSurfaceProfile>() != null)
                return true;

            string value = candidate.name.ToLowerInvariant();
            return value.Contains("wall") ||
                   value.Contains("boundary") ||
                   value.Contains("cliff") ||
                   value.Contains("cavewall") ||
                   value.Contains("rockwall");
        }


        private float NormalizePitch(float pitch)
        {
            if (pitch > 180f)
                pitch -= 360f;

            return pitch;
        }

        private void EnsureTransitionDiagnostics()
        {
            if (transitionDiagnostics == null)
                transitionDiagnostics =
                    GetComponent<BDCameraTransitionDiagnostics>();

            if (transitionDiagnostics == null)
                transitionDiagnostics =
                    gameObject.AddComponent<BDCameraTransitionDiagnostics>();

            if (attachedCamera == null)
                attachedCamera = GetComponent<Camera>();
        }

        private void CaptureTransitionDiagnostics(
            string mode,
            Vector3 cameraPositionBeforeFollow,
            Quaternion cameraRotationBeforeFollow,
            Vector3 targetPosition,
            Vector3 rawDesiredPosition,
            Vector3 containedDesiredPosition,
            Vector3 blendedPosition,
            Vector3 finalPosition,
            Vector3 rawDesiredLookPoint,
            Vector3 containedDesiredLookPoint,
            Vector3 finalLookPoint,
            Quaternion desiredRotation,
            Quaternion finalRotation)
        {
            if (!Application.isPlaying)
                return;

            EnsureTransitionDiagnostics();

            if (transitionDiagnostics == null ||
                !transitionDiagnostics.IsRecording)
            {
                return;
            }

            transitionDiagnostics.Capture(
                new BDCameraTransitionSample
                {
                    Mode = mode,
                    CameraState = cameraState,
                    Target = target,
                    CurrentRoom = currentCameraRoom,
                    PreviousRoom = previousCameraRoom,
                    RoomHandoffActive = roomHandoffActive,
                    HandoffWallCastApplied =
                        handoffWallCastAppliedThisFrame,
                    CameraPositionBeforeFollow =
                        cameraPositionBeforeFollow,
                    CameraRotationBeforeFollow =
                        cameraRotationBeforeFollow,
                    TargetPosition = targetPosition,
                    RawDesiredCameraPosition = rawDesiredPosition,
                    ContainedDesiredCameraPosition =
                        containedDesiredPosition,
                    BlendedCameraPosition = blendedPosition,
                    FinalCameraPosition = finalPosition,
                    RawDesiredLookPoint = rawDesiredLookPoint,
                    ContainedDesiredLookPoint =
                        containedDesiredLookPoint,
                    FinalLookPoint = finalLookPoint,
                    DesiredRotation = desiredRotation,
                    FinalRotation = finalRotation,
                    FieldOfView =
                        attachedCamera != null
                            ? attachedCamera.fieldOfView
                            : 0f
                }
            );
        }

        private void OnGUI()
        {
            if (!showDebugOverlay)
                return;

            GUI.Box(new Rect(Screen.width - 330, 12, 315, 118), "Camera");
            GUI.Label(new Rect(Screen.width - 318, 42, 290, 22), $"State: {cameraState}");
            GUI.Label(new Rect(Screen.width - 318, 64, 290, 22), $"Target: {(target != null ? target.name : "none")}");
            GUI.Label(new Rect(Screen.width - 318, 86, 290, 22), $"Forward: {lastForward.x:0.00}, {lastForward.z:0.00}");
            GUI.Label(new Rect(Screen.width - 318, 108, 290, 22), $"Mounted: {(horseController != null && horseController.IsMounted)}");
        }
    }
}
