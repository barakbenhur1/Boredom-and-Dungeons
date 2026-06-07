using UnityEngine;

namespace BoredomAndDungeons
{
    [DisallowMultipleComponent]
    public sealed class BDCameraFollow : MonoBehaviour
    {
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
        [SerializeField] private float followSmooth = 9.5f;
        [SerializeField] private float rotationSmooth = 8.5f;

        [Header("Mouse / Player Intent Rotation")]
        // BD OBSOLETE CAMERA FIELDS REMOVED V2
        [SerializeField] private float cameraYawDegreesPerSecond = 86f;
        [SerializeField] private float movementDirectionBlend = 7.5f;

        [Header("Room Boundary Camera")]
        // BD ROOM WALL CAMERA STOP V7
        [SerializeField] private bool stopAtClosedRoomWalls = true;
        [SerializeField] private float roomBoundaryInset = 0.80f;
        [SerializeField] private float wallCollisionRadius = 0.55f;
        [SerializeField] private float wallCollisionPadding = 0.35f;
        [SerializeField] private float minimumCameraDistance = 2.25f;
        [SerializeField] private float roomResolveInterval = 0.12f;
        [SerializeField] private LayerMask roomBoundaryCollisionMask = ~0;

        [Header("Shake")]
        [SerializeField] private float cameraShakeMultiplier = 0.42f;
        [SerializeField] private float cameraShakeSmoothing = 18f;

        [Header("Angle")]
        [SerializeField] private float minPitch = 50f;
        [SerializeField] private float maxPitch = 68f;

        [Header("Debug")]
        [SerializeField] private bool showDebugOverlay = false;

        private Transform player;
        private BDPlayerController playerController;
        private BDHorseController horseController;
        private BDHorseHealth horseHealth;

        private Vector3 lastForward = Vector3.forward;
        private Vector3 smoothedMoveForward = Vector3.forward;
        private Vector3 smoothedShakeOffset;
        private string cameraState = "locked behind movement";
        private float nextTargetResolveAt;
        private BDMinimapRoom currentCameraRoom;
        private float nextRoomResolveAt;

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

        private void Start()
        {
            ResolveTargets();
            InitializeForwardFromTarget();

            if (target != null)
                SnapToTarget();
        }

        private void LateUpdate()
        {
            ResolveTargets();

            if (target == null)
                return;

            UpdateCameraForwardFromPlayerIntent();

            if (!lockedBehindTarget)
            {
                FollowFixedOffset();
                return;
            }

            FollowLockedBehind(lastForward);
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
            smoothedMoveForward = lastForward;
        }

        // BD CAMERA FOLLOWS MOUSE INTENT, NOT MOVEMENT
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

            float blend =
                1f - Mathf.Exp(-movementDirectionBlend * Time.deltaTime);

            smoothedMoveForward = Vector3.Slerp(
                smoothedMoveForward,
                intent,
                blend
            );

            smoothedMoveForward.y = 0f;

            if (smoothedMoveForward.sqrMagnitude < 0.001f)
                smoothedMoveForward = intent;

            smoothedMoveForward.Normalize();

            float maximumRadians =
                Mathf.Deg2Rad *
                Mathf.Max(1f, cameraYawDegreesPerSecond) *
                Time.deltaTime;

            lastForward = Vector3.RotateTowards(
                lastForward,
                smoothedMoveForward,
                maximumRadians,
                0f
            );

            lastForward.y = 0f;

            if (lastForward.sqrMagnitude < 0.001f)
                lastForward = intent;

            lastForward.Normalize();
            cameraState = "locked behind mouse/player aim intent";
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

        private void FollowLockedBehind(Vector3 forward)
        {
            if (forward.sqrMagnitude < 0.001f)
                forward = Vector3.forward;

            Vector3 targetPosition = target.position;
            Vector3 desiredPosition =
                targetPosition -
                forward.normalized * distanceBehind +
                Vector3.up * height;
            desiredPosition = ResolveRoomBoundaryConstrainedPosition(
                targetPosition,
                desiredPosition
            );

            Vector3 rawShake = BDGameFeelEvents.SampleCameraShakeOffset() * cameraShakeMultiplier;
            smoothedShakeOffset = Vector3.Lerp(smoothedShakeOffset, rawShake, 1f - Mathf.Exp(-cameraShakeSmoothing * Time.deltaTime));

            transform.position = Vector3.Lerp(
                transform.position,
                desiredPosition + smoothedShakeOffset,
                1f - Mathf.Exp(-followSmooth * Time.deltaTime)
            );

            Vector3 lookPoint =
                targetPosition +
                forward.normalized *
                (lookAhead + Mathf.Max(0f, extraForwardCompositionLookAhead));
            lookPoint = ResolveRoomBoundaryConstrainedLookPoint(
                targetPosition,
                lookPoint
            );
            Quaternion desiredRotation = Quaternion.LookRotation(lookPoint - transform.position, Vector3.up);

            Vector3 euler = desiredRotation.eulerAngles;
            float pitch = NormalizePitch(euler.x);
            pitch = Mathf.Clamp(pitch, minPitch, maxPitch);
            euler.x = pitch;
            euler.z = 0f;
            desiredRotation = Quaternion.Euler(euler);

            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                desiredRotation,
                1f - Mathf.Exp(-rotationSmooth * Time.deltaTime)
            );
        }

        private void FollowFixedOffset()
        {
            Vector3 offset = new Vector3(0f, height, -distanceBehind);
            Vector3 rawShake = BDGameFeelEvents.SampleCameraShakeOffset() * cameraShakeMultiplier;
            smoothedShakeOffset = Vector3.Lerp(smoothedShakeOffset, rawShake, 1f - Mathf.Exp(-cameraShakeSmoothing * Time.deltaTime));

            Vector3 desiredPosition = target.position + offset + smoothedShakeOffset;
            transform.position = Vector3.Lerp(transform.position, desiredPosition, 1f - Mathf.Exp(-followSmooth * Time.deltaTime));

            Quaternion desiredRotation = Quaternion.LookRotation(target.position + Vector3.up * 0.5f - transform.position, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, 1f - Mathf.Exp(-rotationSmooth * Time.deltaTime));
            cameraState = "fixed fallback";
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
            smoothedMoveForward = lastForward;

            Vector3 snappedPosition =
                target.position -
                lastForward * distanceBehind +
                Vector3.up * height;
            transform.position = ResolveRoomBoundaryConstrainedPosition(
                target.position,
                snappedPosition
            );
            Vector3 lookPoint = ResolveRoomBoundaryConstrainedLookPoint(
                target.position,
                target.position + lastForward * lookAhead
            );
            transform.rotation = Quaternion.LookRotation(lookPoint - transform.position, Vector3.up);
        }
        private Vector3 ResolveRoomBoundaryConstrainedPosition(
            Vector3 targetPosition,
            Vector3 desiredPosition)
        {
            if (!stopAtClosedRoomWalls)
                return desiredPosition;

            ResolveCurrentCameraRoom(targetPosition);
            desiredPosition = ClampPointToClosedRoomBounds(
                desiredPosition,
                roomBoundaryInset
            );

            Vector3 castOrigin =
                targetPosition + Vector3.up * 1.65f;
            Vector3 castDelta = desiredPosition - castOrigin;
            float distance = castDelta.magnitude;
            if (distance <= 0.001f)
                return desiredPosition;

            if (Physics.SphereCast(
                    castOrigin,
                    Mathf.Max(0.05f, wallCollisionRadius),
                    castDelta / distance,
                    out RaycastHit hit,
                    distance,
                    roomBoundaryCollisionMask,
                    QueryTriggerInteraction.Ignore) &&
                IsRoomBoundaryCollider(hit.collider))
            {
                float allowedDistance = Mathf.Max(
                    minimumCameraDistance,
                    hit.distance - Mathf.Max(0f, wallCollisionPadding)
                );
                desiredPosition =
                    castOrigin +
                    castDelta.normalized * allowedDistance;
                cameraState = "stopped at closed room wall";
            }

            return desiredPosition;
        }

        private Vector3 ResolveRoomBoundaryConstrainedLookPoint(
            Vector3 targetPosition,
            Vector3 lookPoint)
        {
            if (!stopAtClosedRoomWalls)
                return lookPoint;

            ResolveCurrentCameraRoom(targetPosition);
            return ClampPointToClosedRoomBounds(
                lookPoint,
                roomBoundaryInset
            );
        }

        private void ResolveCurrentCameraRoom(
            Vector3 targetPosition)
        {
            if (currentCameraRoom != null &&
                currentCameraRoom.ContainsWorldPosition(targetPosition, 0.30f) &&
                Time.unscaledTime < nextRoomResolveAt)
            {
                return;
            }

            nextRoomResolveAt =
                Time.unscaledTime +
                Mathf.Max(0.02f, roomResolveInterval);

            BDMinimapRoom[] rooms =
                FindObjectsByType<BDMinimapRoom>(
                    FindObjectsSortMode.None
                );

            BDMinimapRoom nearest = null;
            float nearestDistance = float.PositiveInfinity;
            foreach (BDMinimapRoom room in rooms)
            {
                if (room == null)
                    continue;

                if (room.ContainsWorldPosition(targetPosition, 0.05f))
                {
                    currentCameraRoom = room;
                    return;
                }

                float distance = room.SqrDistanceToCenter(targetPosition);
                if (distance < nearestDistance)
                {
                    nearestDistance = distance;
                    nearest = room;
                }
            }

            currentCameraRoom = nearest;
        }

        private Vector3 ClampPointToClosedRoomBounds(
            Vector3 point,
            float inset)
        {
            if (currentCameraRoom == null)
                return point;

            float halfSize = Mathf.Max(
                0.5f,
                currentCameraRoom.RoomSize * 0.5f - Mathf.Max(0f, inset)
            );
            Vector3 center = currentCameraRoom.WorldCenter;

            if (!currentCameraRoom.WestOpen)
                point.x = Mathf.Max(point.x, center.x - halfSize);
            if (!currentCameraRoom.EastOpen)
                point.x = Mathf.Min(point.x, center.x + halfSize);
            if (!currentCameraRoom.SouthOpen)
                point.z = Mathf.Max(point.z, center.z - halfSize);
            if (!currentCameraRoom.NorthOpen)
                point.z = Mathf.Min(point.z, center.z + halfSize);

            return point;
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
