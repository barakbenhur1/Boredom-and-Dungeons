using UnityEngine;

namespace BoredomAndDungeons
{
    [DefaultExecutionOrder(90)]
    [DisallowMultipleComponent]
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(BDHorseHealth))]
    public sealed class BDHorseHazardSafety : MonoBehaviour
    {
        [Header("Proactive Avoidance")]
        [SerializeField] private float proactiveClearance = 1.05f;
        [SerializeField] private float jumpPathClearance = 1.25f;
        [SerializeField, Min(4)] private int jumpTrajectorySamples = 16;
        [SerializeField] private float groundProbeHeight = 1.4f;
        [SerializeField] private float groundProbeDistance = 4.0f;
        [SerializeField] private float maximumGroundAngle = 55f;

        [Header("Safe Recovery")]
        [SerializeField] private float sampleInterval = 0.18f;
        [SerializeField] private float protectionSeconds = 1.0f;
        [SerializeField] private float recoveryGraceSeconds = 1.45f;
        [SerializeField] private float safePointUpdateLockSeconds = 1.65f;
        [SerializeField] private float rapidRecoveryLoopWindow = 3.25f;
        [SerializeField] private float continuousHazardPollInterval = 0.05f;
        [SerializeField] private float mountedPairSeparation = 1.65f;
        [SerializeField] private float verticalOffset = 0.08f;

        private static readonly float[] SteeringAngles =
        {
            -35f, 35f, -70f, 70f, -110f, 110f, 180f
        };

        private readonly RaycastHit[] groundHits =
            new RaycastHit[24];

        private CharacterController controller;
        private BDHorseController horse;
        private Transform rider;
        private Vector3 initialPosition;
        private Quaternion initialRotation;
        private Vector3 lastSafePosition;
        private Quaternion lastSafeRotation;
        private bool hasSafePosition;
        private float nextSampleAt;
        private float protectedUntil;
        private bool recovering;
        private Vector3 previousSafePosition;
        private Quaternion previousSafeRotation;
        private bool hasPreviousSafePosition;
        private float recoveryGraceUntil = -999f;
        private float safePointUpdatesBlockedUntil = -999f;
        private float lastRecoveryCompletedAt = -999f;
        private float nextHazardPollAt;


        public bool IsRecovering =>
            recovering ||
            Time.unscaledTime <
                recoveryGraceUntil;
        private void Awake()
        {
            controller = GetComponent<CharacterController>();
            horse = GetComponent<BDHorseController>();

            initialPosition = transform.position;
            initialRotation = transform.rotation;

            lastSafePosition = initialPosition;
            lastSafeRotation = initialRotation;
            hasSafePosition = true;

            previousSafePosition = initialPosition;
            previousSafeRotation = initialRotation;
            hasPreviousSafePosition = true;
        }

        private void Start()
        {
            rider = horse != null ? horse.Rider : null;
            TryRecordSafePoint(force: true);
        }
        private void LateUpdate()
        {
            if (!Application.isPlaying ||
                recovering)
            {
                return;
            }

            float now = Time.unscaledTime;

            if (now >= nextHazardPollAt)
            {
                nextHazardPollAt =
                    now +
                    Mathf.Max(
                        0.02f,
                        continuousHazardPollInterval
                    );

                PollCurrentHazard();

                if (recovering)
                    return;
            }

            if (now < protectedUntil ||
                now < recoveryGraceUntil ||
                now < safePointUpdatesBlockedUntil ||
                now < nextSampleAt)
            {
                return;
            }

            nextSampleAt =
                now +
                Mathf.Max(
                    0.05f,
                    sampleInterval
                );

            if (controller != null &&
                controller.enabled &&
                controller.isGrounded)
            {
                TryRecordSafePoint(force: false);
            }
        }
        private void PollCurrentHazard()
        {
            Vector3 center = transform.position;

            if (controller != null &&
                controller.enabled)
            {
                center = controller.bounds.center;
            }

            if (!BDHazardVolume.TryFindUnsafeVolume(
                    center,
                    0.05f,
                    out BDHazardVolume hazard))
            {
                return;
            }

            TryHandleHazard(
                hazard,
                forceActivation: true
            );
        }


        public Vector3 FilterMovement(Vector3 requestedMotion)
        {
            if (recovering || requestedMotion.sqrMagnitude < 0.000001f)
                return requestedMotion;

            Vector3 horizontal =
                new Vector3(requestedMotion.x, 0f, requestedMotion.z);

            if (horizontal.sqrMagnitude < 0.000001f)
                return requestedMotion;

            Vector3 predicted = transform.position + horizontal;

            if (IsHorsePositionSafe(predicted))
                return requestedMotion;

            foreach (float angle in SteeringAngles)
            {
                Vector3 alternative =
                    Quaternion.AngleAxis(angle, Vector3.up) * horizontal;

                if (!IsHorsePositionSafe(
                        transform.position + alternative))
                {
                    continue;
                }

                return alternative +
                       Vector3.up * requestedMotion.y;
            }

            return Vector3.up * requestedMotion.y;
        }

        public bool CanStartJump(
            Vector3 horizontalVelocity,
            float jumpHeight,
            float gravity)
        {
            if (recovering)
                return false;

            float gravityMagnitude = Mathf.Max(
                0.01f,
                Mathf.Abs(gravity)
            );

            float initialVerticalVelocity =
                Mathf.Sqrt(
                    Mathf.Max(0f, jumpHeight) *
                    2f *
                    gravityMagnitude
                );

            float flightDuration = Mathf.Max(
                0.15f,
                (2f * initialVerticalVelocity) /
                gravityMagnitude
            );

            Vector3 horizontal = horizontalVelocity;
            horizontal.y = 0f;

            int samples = Mathf.Max(
                4,
                jumpTrajectorySamples
            );

            float clearance = Mathf.Max(
                proactiveClearance,
                jumpPathClearance
            );

            for (int sampleIndex = 1;
                 sampleIndex <= samples;
                 sampleIndex++)
            {
                float progress =
                    sampleIndex / (float)samples;

                float sampleTime =
                    flightDuration * progress;

                Vector3 samplePosition =
                    transform.position +
                    horizontal *
                    sampleTime;

                if (BDHazardVolume.IsRecoveryPointUnsafe(
                        samplePosition,
                        clearance))
                {
                    return false;
                }

                if (!TryResolveGround(
                        samplePosition,
                        out _))
                {
                    return false;
                }
            }

            return true;
        }
        public bool TryHandleHazard(
            BDHazardVolume volume,
            bool forceActivation = false)
        {
            float now = Time.unscaledTime;

            if (volume == null ||
                recovering ||
                now < protectedUntil ||
                now < recoveryGraceUntil)
            {
                return false;
            }

            if (!forceActivation &&
                volume.HazardType == BDHazardType.Lava &&
                !volume.IsActorTouchingSurface(
                    controller,
                    0.08f))
            {
                return false;
            }

            bool wasMounted =
                horse != null &&
                horse.IsMounted;

            Transform mountedRider =
                wasMounted &&
                horse != null
                    ? horse.Rider
                    : null;

            BDPlayerHazardRecovery riderRecovery =
                mountedRider != null
                    ? mountedRider.GetComponent<
                        BDPlayerHazardRecovery>()
                    : null;

            recovering = true;

            safePointUpdatesBlockedUntil =
                now +
                Mathf.Max(
                    0.50f,
                    safePointUpdateLockSeconds
                );

            if (!TryResolveRecoveryPosition(
                    out Vector3 recoveryPosition,
                    out Quaternion recoveryRotation))
            {
                recoveryPosition = initialPosition;
                recoveryRotation = initialRotation;
            }

            if (wasMounted && horse != null)
            {
                horse.ForceDismountAfterHazardRecovery();
            }

            bool controllerWasEnabled =
                controller != null &&
                controller.enabled;

            if (controllerWasEnabled)
                controller.enabled = false;

            transform.SetPositionAndRotation(
                recoveryPosition,
                recoveryRotation
            );

            Physics.SyncTransforms();

            if (controllerWasEnabled)
                controller.enabled = true;

            lastSafePosition = recoveryPosition;
            lastSafeRotation = recoveryRotation;
            hasSafePosition = true;

            protectedUntil =
                now +
                Mathf.Max(
                    0.10f,
                    protectionSeconds
                );

            recoveryGraceUntil =
                now +
                Mathf.Max(
                    0.50f,
                    recoveryGraceSeconds
                );

            lastRecoveryCompletedAt = now;
            recovering = false;

            if (wasMounted &&
                riderRecovery != null)
            {
                riderRecovery.PrepareMountedHazardRecovery(
                    recoveryPosition,
                    mountedPairSeparation
                );

                riderRecovery.TryHandleHazard(
                    volume,
                    forceActivation: true
                );
            }

            return true;
        }
        private bool TryResolveRecoveryPosition(
            out Vector3 position,
            out Quaternion rotation)
        {
            bool rapidLoop =
                Time.unscaledTime -
                lastRecoveryCompletedAt <=
                Mathf.Max(
                    0.50f,
                    rapidRecoveryLoopWindow
                );

            if (rapidLoop &&
                hasPreviousSafePosition &&
                IsHorsePositionSafe(
                    previousSafePosition))
            {
                position = previousSafePosition;
                rotation = previousSafeRotation;
                return true;
            }

            if (hasSafePosition &&
                IsHorsePositionSafe(
                    lastSafePosition))
            {
                position = lastSafePosition;
                rotation = lastSafeRotation;
                return true;
            }

            if (hasPreviousSafePosition &&
                IsHorsePositionSafe(
                    previousSafePosition))
            {
                position = previousSafePosition;
                rotation = previousSafeRotation;
                return true;
            }

            if (IsHorsePositionSafe(
                    initialPosition))
            {
                position = initialPosition;
                rotation = initialRotation;
                return true;
            }

            position = initialPosition;
            rotation = initialRotation;
            return false;
        }


        private void RecoverHorseWithoutDamage()
        {
            Vector3 target = hasSafePosition
                ? lastSafePosition
                : initialPosition;
            Quaternion rotation = hasSafePosition
                ? lastSafeRotation
                : initialRotation;

            bool wasEnabled =
                controller != null &&
                controller.enabled;

            if (wasEnabled)
                controller.enabled = false;

            transform.SetPositionAndRotation(target, rotation);
            Physics.SyncTransforms();

            if (wasEnabled)
                controller.enabled = true;
        }
        private bool TryRecordSafePoint(
            bool force)
        {
            float now = Time.unscaledTime;

            if (!force &&
                (recovering ||
                 now < protectedUntil ||
                 now < recoveryGraceUntil ||
                 now < safePointUpdatesBlockedUntil))
            {
                return false;
            }

            if (!TryResolveGround(
                    transform.position,
                    out Vector3 grounded))
            {
                return false;
            }

            Vector3 candidate =
                grounded +
                Vector3.up *
                Mathf.Max(
                    0f,
                    verticalOffset
                );

            if (!IsHorsePositionSafe(candidate))
                return false;

            if (!force &&
                hasSafePosition &&
                (candidate - lastSafePosition)
                    .sqrMagnitude < 0.04f)
            {
                return false;
            }

            if (hasSafePosition)
            {
                previousSafePosition =
                    lastSafePosition;

                previousSafeRotation =
                    lastSafeRotation;

                hasPreviousSafePosition = true;
            }

            lastSafePosition = candidate;

            lastSafeRotation = Quaternion.Euler(
                0f,
                transform.eulerAngles.y,
                0f
            );

            hasSafePosition = true;
            return true;
        }

        private bool IsHorsePositionSafe(Vector3 position)
        {
            if (BDHazardVolume.IsRecoveryPointUnsafe(
                    position,
                    Mathf.Max(0f, proactiveClearance)))
            {
                return false;
            }

            return TryResolveGround(position, out _);
        }

        private bool TryResolveGround(
            Vector3 position,
            out Vector3 grounded)
        {
            Vector3 origin =
                position +
                Vector3.up * Mathf.Max(0.5f, groundProbeHeight);

            int count = Physics.RaycastNonAlloc(
                origin,
                Vector3.down,
                groundHits,
                Mathf.Max(1f, groundProbeDistance),
                ~0,
                QueryTriggerInteraction.Ignore
            );

            float nearestDistance = float.PositiveInfinity;
            grounded = position;
            bool found = false;

            for (int i = 0; i < count; i++)
            {
                RaycastHit hit = groundHits[i];

                if (hit.collider == null ||
                    hit.transform == transform ||
                    hit.transform.IsChildOf(transform))
                {
                    continue;
                }

                float angle =
                    Vector3.Angle(hit.normal, Vector3.up);

                if (angle >
                    Mathf.Clamp(maximumGroundAngle, 0f, 89f))
                {
                    continue;
                }

                if (hit.distance >= nearestDistance)
                    continue;

                nearestDistance = hit.distance;
                grounded = hit.point;
                found = true;
            }

            return found;
        }
    }
}
