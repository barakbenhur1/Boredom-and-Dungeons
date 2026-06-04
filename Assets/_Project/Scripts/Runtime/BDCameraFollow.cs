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
        [SerializeField] private float lookAhead = 6.75f;
        [SerializeField] private float followSmooth = 9.5f;
        [SerializeField] private float rotationSmooth = 8.5f;

        [Header("Movement Based Rotation")]
        [SerializeField] private float cameraYawDegreesPerSecond = 86f;
        [SerializeField] private float movementDirectionBlend = 7.5f;
        [SerializeField] private float minimumMovementDirectionMagnitude = 0.05f;
        [SerializeField] private bool rotateOnlyWhenActuallyMoving = true;

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

            UpdateCameraForwardFromMovementOnly();

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

        private void UpdateCameraForwardFromMovementOnly()
        {
            Vector3 movementDirection = ResolveRealMovementDirection();
            movementDirection.y = 0f;

            if (movementDirection.sqrMagnitude < minimumMovementDirectionMagnitude * minimumMovementDirectionMagnitude)
            {
                if (rotateOnlyWhenActuallyMoving)
                {
                    cameraState = playerController != null && playerController.ShouldLockCameraYawForDodge ? "stable during dodge" : "stable while aiming/combat idle";
                    return;
                }

                movementDirection = lastForward;
            }

            movementDirection.Normalize();

            float blendT = 1f - Mathf.Exp(-movementDirectionBlend * Time.deltaTime);
            smoothedMoveForward = Vector3.Slerp(smoothedMoveForward, movementDirection, blendT);
            smoothedMoveForward.y = 0f;

            if (smoothedMoveForward.sqrMagnitude < 0.001f)
                smoothedMoveForward = movementDirection;

            smoothedMoveForward.Normalize();

            float maxRadians = Mathf.Deg2Rad * Mathf.Max(1f, cameraYawDegreesPerSecond) * Time.deltaTime;
            lastForward = Vector3.RotateTowards(lastForward, smoothedMoveForward, maxRadians, 0f);
            lastForward.y = 0f;

            if (lastForward.sqrMagnitude < 0.001f)
                lastForward = movementDirection;

            lastForward.Normalize();
            cameraState = "locked behind real movement";
        }

        private Vector3 ResolveRealMovementDirection()
        {
            if (horseController != null && horseController.IsMounted)
            {
                if (horseController.HasRideMoveInput)
                {
                    Vector3 mountedMove = horseController.LastMountedMovementDirection;
                    mountedMove.y = 0f;

                    if (mountedMove.sqrMagnitude > 0.001f)
                        return mountedMove.normalized;
                }

                return Vector3.zero;
            }

            if (playerController != null)
            {
                // Dodge is an evasive burst, not a request to rotate the whole camera rig.
                // Backward dodge especially should not make the camera orbit behind the reversed dash direction.
                if (playerController.ShouldLockCameraYawForDodge)
                    return Vector3.zero;

                if (playerController.HasMoveInput)
                {
                    Vector3 playerMove = playerController.LastMoveWorldDirection;
                    playerMove.y = 0f;

                    if (playerMove.sqrMagnitude > 0.001f)
                        return playerMove.normalized;
                }
            }

            return Vector3.zero;
        }

        private void FollowLockedBehind(Vector3 forward)
        {
            if (forward.sqrMagnitude < 0.001f)
                forward = Vector3.forward;

            Vector3 targetPosition = target.position;
            Vector3 desiredPosition = targetPosition - forward.normalized * distanceBehind + Vector3.up * height;

            Vector3 rawShake = BDGameFeelEvents.SampleCameraShakeOffset() * cameraShakeMultiplier;
            smoothedShakeOffset = Vector3.Lerp(smoothedShakeOffset, rawShake, 1f - Mathf.Exp(-cameraShakeSmoothing * Time.deltaTime));

            transform.position = Vector3.Lerp(
                transform.position,
                desiredPosition + smoothedShakeOffset,
                1f - Mathf.Exp(-followSmooth * Time.deltaTime)
            );

            Vector3 lookPoint = targetPosition + forward.normalized * lookAhead;
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

            transform.position = target.position - lastForward * distanceBehind + Vector3.up * height;
            Vector3 lookPoint = target.position + lastForward * lookAhead;
            transform.rotation = Quaternion.LookRotation(lookPoint - transform.position, Vector3.up);
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
