using System;
using UnityEngine;

namespace BoredomAndDungeons
{
    [DefaultExecutionOrder(100)]
    [DisallowMultipleComponent]
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(BDHealth))]
    public sealed class BDPlayerHazardRecovery : MonoBehaviour
    {
        [Header("Safe Point Tracking")]
        [SerializeField] private float sampleInterval = 0.15f;
        [SerializeField] private float groundedStableDuration = 0.25f;
        [SerializeField] private float hazardEdgeClearance = 0.85f;
        [SerializeField] private float minimumSafePointMovement = 0.20f;
        [SerializeField] private float maximumGroundAngle = 55f;
        [SerializeField] private float groundProbeHeight = 1.25f;
        [SerializeField] private float groundProbeDistance = 3.5f;

        [Header("Recovery")]
        [SerializeField] private float recoveryProtectionSeconds = 1.0f;
        [SerializeField] private float rejectedWalkProtectionSeconds = 0.20f;
        [SerializeField] private float recoveryVerticalOffset = 0.08f;
        [SerializeField] private float fallbackSearchRadiusStep = 0.85f;
        [SerializeField] private int fallbackSearchRings = 3;

        private readonly Collider[] overlapBuffer = new Collider[48];
        private readonly RaycastHit[] groundHits = new RaycastHit[24];

        private CharacterController characterController;
        private BDPlayerController playerController;
        private BDHealth health;

        private Vector3 initialSpawnPosition;
        private Quaternion initialSpawnRotation;
        private Vector3 lastSafePosition;
        private Quaternion lastSafeRotation;
        private bool hasSafePosition;
        private float groundedSafeSince = -1f;
        private float nextSampleAt;
        private float recoveryProtectedUntil;
        private bool recoveryInProgress;

        public event Action<BDHazardType, float> HazardRecovered;

        public bool HasSafePosition => hasSafePosition;
        public Vector3 LastSafePosition => lastSafePosition;
        public bool IsRecoveryProtected =>
            Time.unscaledTime < recoveryProtectedUntil;

        private void Awake()
        {
            characterController = GetComponent<CharacterController>();
            playerController = GetComponent<BDPlayerController>();
            health = GetComponent<BDHealth>();

            initialSpawnPosition = transform.position;
            initialSpawnRotation = transform.rotation;
            lastSafePosition = initialSpawnPosition;
            lastSafeRotation = initialSpawnRotation;
            hasSafePosition = true;
        }

        private void Start()
        {
            TryRecordCurrentPosition(force: true);
        }

        private void LateUpdate()
        {
            if (!Application.isPlaying ||
                recoveryInProgress ||
                health == null ||
                health.IsDead)
            {
                return;
            }

            if (Time.unscaledTime < nextSampleAt)
                return;

            nextSampleAt =
                Time.unscaledTime + Mathf.Max(0.05f, sampleInterval);

            TickSafePointTracking();
        }

        private void TickSafePointTracking()
        {
            if (characterController == null ||
                !characterController.enabled ||
                !characterController.isGrounded ||
                (playerController != null && playerController.IsDashing))
            {
                groundedSafeSince = -1f;
                return;
            }

            if (!TryResolveGroundedCandidate(
                    transform.position,
                    out Vector3 candidate) ||
                !IsCandidateSafe(candidate))
            {
                groundedSafeSince = -1f;
                return;
            }

            if (groundedSafeSince < 0f)
            {
                groundedSafeSince = Time.unscaledTime;
                return;
            }

            if (Time.unscaledTime - groundedSafeSince <
                Mathf.Max(0f, groundedStableDuration))
            {
                return;
            }

            RecordSafePoint(candidate, transform.rotation, force: false);
        }

        public bool TryHandleHazard(
            BDHazardVolume volume,
            bool forceActivation = false)
        {
            if (volume == null ||
                recoveryInProgress ||
                IsRecoveryProtected ||
                health == null ||
                health.IsDead)
            {
                return false;
            }

            if (!forceActivation &&
                volume.HazardType == BDHazardType.HoleOrChasm &&
                !HasIntentionalGapEntry())
            {
                recoveryProtectedUntil =
                    Time.unscaledTime +
                    Mathf.Max(0.05f, rejectedWalkProtectionSeconds);

                RecoverToLatestSafePoint();
                return true;
            }

            recoveryInProgress = true;
            recoveryProtectedUntil =
                Time.unscaledTime +
                Mathf.Max(0.1f, recoveryProtectionSeconds);

            float damage = volume.Damage;
            health.ApplyUnavoidableDamage(damage);

            if (!health.IsDead)
                RecoverToLatestSafePoint();

            HazardRecovered?.Invoke(volume.HazardType, damage);
            recoveryInProgress = false;
            return true;
        }

        private bool HasIntentionalGapEntry()
        {
            return playerController != null &&
                   playerController.HasRecentIntentionalGapEntry;
        }

        public bool TryRecordCurrentPosition(bool force = false)
        {
            if (!TryResolveGroundedCandidate(
                    transform.position,
                    out Vector3 candidate))
            {
                return false;
            }

            if (!IsCandidateSafe(candidate))
                return false;

            RecordSafePoint(candidate, transform.rotation, force);
            return true;
        }

        private void RecordSafePoint(
            Vector3 position,
            Quaternion rotation,
            bool force)
        {
            if (!force &&
                hasSafePosition &&
                (position - lastSafePosition).sqrMagnitude <
                minimumSafePointMovement * minimumSafePointMovement)
            {
                return;
            }

            lastSafePosition = position;
            lastSafeRotation = Quaternion.Euler(
                0f,
                rotation.eulerAngles.y,
                0f
            );
            hasSafePosition = true;
        }

        private void RecoverToLatestSafePoint()
        {
            Vector3 targetPosition;
            Quaternion targetRotation;

            if (!TryResolveRecoveryPoint(
                    out targetPosition,
                    out targetRotation))
            {
                targetPosition = initialSpawnPosition;
                targetRotation = initialSpawnRotation;
            }

            bool controllerWasEnabled =
                characterController != null &&
                characterController.enabled;

            if (controllerWasEnabled)
                characterController.enabled = false;

            transform.SetPositionAndRotation(
                targetPosition,
                targetRotation
            );

            Physics.SyncTransforms();

            if (controllerWasEnabled)
                characterController.enabled = true;

            if (playerController != null)
            {
                playerController.enabled = true;
                playerController.ResetMotionAfterExternalTeleport();
            }

            groundedSafeSince = -1f;
            nextSampleAt =
                Time.unscaledTime +
                Mathf.Max(0.05f, sampleInterval);
        }

        private bool TryResolveRecoveryPoint(
            out Vector3 position,
            out Quaternion rotation)
        {
            if (hasSafePosition &&
                TryResolveGroundedCandidate(
                    lastSafePosition,
                    out Vector3 latestCandidate) &&
                IsCandidateSafe(latestCandidate))
            {
                position = latestCandidate;
                rotation = lastSafeRotation;
                return true;
            }

            if (TryResolveGroundedCandidate(
                    initialSpawnPosition,
                    out Vector3 spawnCandidate) &&
                IsCandidateSafe(spawnCandidate))
            {
                position = spawnCandidate;
                rotation = initialSpawnRotation;
                return true;
            }

            Vector3 searchOrigin =
                hasSafePosition
                    ? lastSafePosition
                    : initialSpawnPosition;

            int rings = Mathf.Max(1, fallbackSearchRings);
            float radiusStep = Mathf.Max(0.25f, fallbackSearchRadiusStep);

            for (int ring = 1; ring <= rings; ring++)
            {
                float radius = ring * radiusStep;
                int samples = Mathf.Max(8, ring * 8);

                for (int sample = 0; sample < samples; sample++)
                {
                    float angle =
                        sample * (360f / samples) * Mathf.Deg2Rad;

                    Vector3 offset = new Vector3(
                        Mathf.Cos(angle),
                        0f,
                        Mathf.Sin(angle)
                    ) * radius;

                    if (!TryResolveGroundedCandidate(
                            searchOrigin + offset,
                            out Vector3 candidate))
                    {
                        continue;
                    }

                    if (!IsCandidateSafe(candidate))
                        continue;

                    position = candidate;
                    rotation = hasSafePosition
                        ? lastSafeRotation
                        : initialSpawnRotation;
                    return true;
                }
            }

            position = default;
            rotation = Quaternion.identity;
            return false;
        }

        private bool TryResolveGroundedCandidate(
            Vector3 requestedPosition,
            out Vector3 groundedPosition)
        {
            Vector3 origin =
                requestedPosition +
                Vector3.up * Mathf.Max(0.5f, groundProbeHeight);

            int hitCount = Physics.RaycastNonAlloc(
                origin,
                Vector3.down,
                groundHits,
                Mathf.Max(1f, groundProbeDistance),
                ~0,
                QueryTriggerInteraction.Ignore
            );

            float bestDistance = float.PositiveInfinity;
            bool found = false;
            groundedPosition = requestedPosition;

            for (int i = 0; i < hitCount; i++)
            {
                RaycastHit hit = groundHits[i];

                if (hit.collider == null ||
                    IsOwnCollider(hit.collider))
                {
                    continue;
                }

                float groundAngle =
                    Vector3.Angle(hit.normal, Vector3.up);

                if (groundAngle >
                    Mathf.Clamp(maximumGroundAngle, 0f, 89f))
                {
                    continue;
                }

                if (hit.distance >= bestDistance)
                    continue;

                bestDistance = hit.distance;
                groundedPosition =
                    hit.point +
                    Vector3.up * Mathf.Max(0f, recoveryVerticalOffset);
                found = true;
            }

            return found;
        }

        private bool IsCandidateSafe(Vector3 candidate)
        {
            if (BDHazardVolume.IsPointUnsafe(
                    candidate,
                    Mathf.Max(0f, hazardEdgeClearance)))
            {
                return false;
            }

            if (characterController == null)
                return true;

            float radius =
                Mathf.Max(0.1f, characterController.radius * 0.88f);
            float height =
                Mathf.Max(radius * 2f, characterController.height);
            Vector3 center =
                candidate + characterController.center;
            float vertical =
                Mathf.Max(0f, height * 0.5f - radius);
            Vector3 bottom = center - Vector3.up * vertical;
            Vector3 top = center + Vector3.up * vertical;

            int count = Physics.OverlapCapsuleNonAlloc(
                bottom,
                top,
                radius,
                overlapBuffer,
                ~0,
                QueryTriggerInteraction.Collide
            );

            float groundAllowanceY = candidate.y + 0.18f;

            for (int i = 0; i < count; i++)
            {
                Collider other = overlapBuffer[i];

                if (other == null ||
                    !other.enabled ||
                    IsOwnCollider(other))
                {
                    continue;
                }

                if (other.GetComponentInParent<BDHazardVolume>() != null)
                    return false;

                if (other.isTrigger &&
                    other.name.IndexOf(
                        "barrier",
                        StringComparison.OrdinalIgnoreCase) < 0)
                {
                    continue;
                }

                if (!other.isTrigger &&
                    other.bounds.max.y <= groundAllowanceY)
                {
                    continue;
                }

                return false;
            }

            return true;
        }

        private bool IsOwnCollider(Collider candidate)
        {
            if (candidate == null)
                return false;

            Transform candidateTransform = candidate.transform;

            return candidateTransform == transform ||
                   candidateTransform.IsChildOf(transform) ||
                   transform.IsChildOf(candidateTransform);
        }
    }
}
