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
        [SerializeField] private float groundProbeHeight = 1.4f;
        [SerializeField] private float groundProbeDistance = 4.0f;
        [SerializeField] private float maximumGroundAngle = 55f;

        [Header("Safe Recovery")]
        [SerializeField] private float sampleInterval = 0.18f;
        [SerializeField] private float protectionSeconds = 1.0f;
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

        private void Awake()
        {
            controller = GetComponent<CharacterController>();
            horse = GetComponent<BDHorseController>();
            initialPosition = transform.position;
            initialRotation = transform.rotation;
            lastSafePosition = initialPosition;
            lastSafeRotation = initialRotation;
            hasSafePosition = true;
        }

        private void Start()
        {
            rider = horse != null ? horse.Rider : null;
            TryRecordSafePoint(force: true);
        }

        private void LateUpdate()
        {
            if (recovering || Time.unscaledTime < nextSampleAt)
                return;

            nextSampleAt =
                Time.unscaledTime + Mathf.Max(0.05f, sampleInterval);

            if (controller != null &&
                controller.enabled &&
                controller.isGrounded)
            {
                TryRecordSafePoint(force: false);
            }
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

        public bool TryHandleHazard(BDHazardVolume volume)
        {
            if (volume == null ||
                recovering ||
                Time.unscaledTime < protectedUntil)
            {
                return false;
            }

            recovering = true;
            protectedUntil =
                Time.unscaledTime +
                Mathf.Max(0.1f, protectionSeconds);

            rider = horse != null ? horse.Rider : rider;
            BDPlayerHazardRecovery playerRecovery =
                rider != null
                    ? rider.GetComponent<BDPlayerHazardRecovery>()
                    : null;

            bool wasMounted =
                horse != null &&
                horse.IsMounted;

            if (wasMounted)
                horse.ForceDismountAfterHazardRecovery();

            RecoverHorseWithoutDamage();

            if (wasMounted && playerRecovery != null)
            {
                playerRecovery.TryHandleHazard(
                    volume,
                    forceActivation: true
                );
            }

            recovering = false;
            return true;
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

        private bool TryRecordSafePoint(bool force)
        {
            if (!TryResolveGround(
                    transform.position,
                    out Vector3 grounded))
            {
                return false;
            }

            if (!IsHorsePositionSafe(grounded))
                return false;

            if (!force &&
                hasSafePosition &&
                (grounded - lastSafePosition).sqrMagnitude < 0.04f)
            {
                return false;
            }

            lastSafePosition =
                grounded + Vector3.up * Mathf.Max(0f, verticalOffset);
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
            if (BDHazardVolume.IsPointUnsafe(
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
