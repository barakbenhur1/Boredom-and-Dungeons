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
        [SerializeField] private float groundedStableDuration = 0.85f;
        [SerializeField] private float hazardEdgeClearance = 2.10f;
        [SerializeField] private float minimumSafePointMovement = 0.20f;
        [SerializeField] private float maximumGroundAngle = 55f;
        [SerializeField] private float groundProbeHeight = 1.25f;
        [SerializeField] private float groundProbeDistance = 3.5f;

        [Header("Hole / Chasm Fall")]
        [SerializeField] private float holeFallDuration = 2.25f;
        [SerializeField] private float holeFallSpeed = 2.35f;
[Header("Lava Bounce")]
        [SerializeField] private float lavaBounceDuration = 0.42f;
        [SerializeField] private float lavaBounceHeight = 1.35f;
        [SerializeField] private float lavaSurfaceContactTolerance = 0.05f;

        [Header("Ground Safety")]
        [SerializeField] private float accidentalGroundExitGrace = 0.16f;
        [SerializeField] private float accidentalGroundExitDepth = 0.34f;

        [Header("Forced Movement")]
        [SerializeField] private float forcedDisplacementWindow = 0.85f;
        [SerializeField] private float forcedDisplacementMinDistance = 0.10f;
        [SerializeField] private float forcedDisplacementSpeedMultiplier = 1.35f;

        [Header("Recovery")]
        [SerializeField] private float recoveryProtectionSeconds = 1.0f;
        [SerializeField] private float postRecoverySafePointLock = 1.65f;
        [SerializeField] private float rapidRecoveryLoopWindow = 3.25f;
        [SerializeField] private float recoveryVerticalOffset = 0.38f;
        [Header("Mounted Hazard Recovery")]
        [SerializeField] private float mountedRecoverySeparation = 1.65f;
        [SerializeField] private float mountedRecoveryAnchorLifetime = 4.0f;

        [SerializeField] private float fallbackSearchRadiusStep = 0.85f;
        [SerializeField] private int fallbackSearchRings = 3;

        private readonly Collider[] overlapBuffer =
            new Collider[48];
        private readonly RaycastHit[] groundHits =
            new RaycastHit[24];

        private CharacterController characterController;
        private BDPlayerController playerController;
        private BDHealth health;

        private Vector3 initialSpawnPosition;
        private Quaternion initialSpawnRotation;
        private Vector3 lastSafePosition;
        private Quaternion lastSafeRotation;
        private bool hasSafePosition;
        private float groundedSafeSince = -1f;
        private float lastGroundedAt;
        private float nextSampleAt;
        private float recoveryProtectedUntil;
        private Vector3 previousSafePosition;
        private Quaternion previousSafeRotation;
        private bool hasPreviousSafePosition;
        private float safePointUpdatesBlockedUntil;
        private float lastRecoveryCompletedAt = -999f;

        private Vector3 mountedRecoveryAnchor;
        private float mountedRecoveryAnchorUntil = -999f;
        private bool hasMountedRecoveryAnchor;

        private Vector3 previousTrackedPosition;
        private bool hasPreviousTrackedPosition;

        private bool holeFallInProgress;
        private BDHazardVolume holeFallVolume;
        private float holeFallStartedAt;
        private bool holeControllerWasEnabled;
        private bool holePlayerWasEnabled;

        private bool lavaBounceInProgress;
        private BDHazardVolume lavaBounceVolume;
        private float lavaBounceStartedAt;
        private Vector3 lavaBounceStart;
        private Vector3 lavaBounceTarget;
        private Quaternion lavaBounceRotation;
        private bool lavaControllerWasEnabled;
        private bool lavaPlayerWasEnabled;

        public event Action<BDHazardType, float>
            HazardRecovered;

        public bool HasSafePosition => hasSafePosition;
        public Vector3 LastSafePosition => lastSafePosition;
        public bool IsRecoveryProtected =>
            Time.unscaledTime < recoveryProtectedUntil;

        private void Awake()
        {
            characterController =
                GetComponent<CharacterController>();
            playerController =
                GetComponent<BDPlayerController>();
            health = GetComponent<BDHealth>();

            initialSpawnPosition = transform.position;
            initialSpawnRotation = transform.rotation;
            lastSafePosition = initialSpawnPosition;
            lastSafeRotation = initialSpawnRotation;
            hasSafePosition = true;
            previousSafePosition = initialSpawnPosition;
            previousSafeRotation = initialSpawnRotation;
            hasPreviousSafePosition = true;
            safePointUpdatesBlockedUntil =
                Time.unscaledTime +
                Mathf.Max(0.25f, groundedStableDuration);
            lastGroundedAt = Time.unscaledTime;
            previousTrackedPosition = transform.position;
            hasPreviousTrackedPosition = true;
        }

        private void Start()
        {
            TryRecordCurrentPosition(force: true);
        }

        private void LateUpdate()
        {
            if (!Application.isPlaying)
                return;

            if (holeFallInProgress)
            {
                TickHoleFall();
                return;
            }

            if (lavaBounceInProgress)
            {
                TickLavaBounce();
                return;
            }

            if (health == null || health.IsDead)
                return;

            DetectForcedDisplacement();
            CheckActiveHazardContact();

            if (holeFallInProgress ||
                lavaBounceInProgress)
            {
                return;
            }

            CheckGroundExit();

            if (characterController != null &&
                characterController.enabled &&
                characterController.isGrounded)
            {
                lastGroundedAt = Time.unscaledTime;
            }

            if (Time.unscaledTime < nextSampleAt)
                return;

            nextSampleAt =
                Time.unscaledTime +
                Mathf.Max(0.05f, sampleInterval);

            TickSafePointTracking();
        }

        public void PrepareMountedHazardRecovery(
            Vector3 recoveredHorsePosition,
            float requestedSeparation)
        {
            mountedRecoveryAnchor =
                recoveredHorsePosition;

            mountedRecoverySeparation =
                Mathf.Max(
                    0.75f,
                    requestedSeparation
                );

            mountedRecoveryAnchorUntil =
                Time.unscaledTime +
                Mathf.Max(
                    1.0f,
                    mountedRecoveryAnchorLifetime
                );

            hasMountedRecoveryAnchor = true;
            FreezeSafePointUpdates();
        }

        public bool TryHandleHazard(
            BDHazardVolume volume,
            bool forceActivation = false)
        {
            if (volume == null ||
                holeFallInProgress ||
                lavaBounceInProgress ||
                IsRecoveryProtected ||
                health == null ||
                health.IsDead)
            {
                return false;
            }

            if (volume.HazardType == BDHazardType.Lava)
            {
                if (!forceActivation &&
                    !volume.IsActorTouchingSurface(
                        characterController,
                        lavaSurfaceContactTolerance))
                {
                    return false;
                }

                BeginLavaBounce(volume);
                return true;
            }

            if (!forceActivation &&
                (playerController == null ||
                 !playerController.HasRecentIntentionalGapEntry))
            {
                RecoverImmediatelyWithoutDamage();
                return true;
            }

            BeginHoleFall(volume);
            return true;
        }

        private void BeginHoleFall(
            BDHazardVolume volume)
        {
            FreezeSafePointUpdates();
            holeFallInProgress = true;
            holeFallVolume = volume;
            holeFallStartedAt = Time.unscaledTime;
            recoveryProtectedUntil =
                Time.unscaledTime +
                Mathf.Max(
                    0.1f,
                    holeFallDuration +
                    recoveryProtectionSeconds
                );

            holeControllerWasEnabled =
                characterController != null &&
                characterController.enabled;
            holePlayerWasEnabled =
                playerController != null &&
                playerController.enabled;

            if (playerController != null)
                playerController.enabled = false;

            if (characterController != null)
                characterController.enabled = false;
        }

        private void TickHoleFall()
        {
            float speed = Mathf.Max(0.25f, holeFallSpeed);

            transform.position +=
                Vector3.down *
                speed *
                Time.unscaledDeltaTime;

            float duration =
                Mathf.Max(0.12f, holeFallDuration);

            if (Time.unscaledTime - holeFallStartedAt < duration)
                return;

            float damage =
                holeFallVolume != null
                    ? holeFallVolume.Damage
                    : 15f;

            health.ApplyUnavoidableDamage(damage);

            if (!health.IsDead)
            {
                RecoverToLatestSafePointImmediate(
                    holeControllerWasEnabled,
                    holePlayerWasEnabled
                );
            }
            else
            {
                RestoreControlState(
                    holeControllerWasEnabled,
                    holePlayerWasEnabled
                );
            }

            HazardRecovered?.Invoke(
                BDHazardType.HoleOrChasm,
                damage
            );

            holeFallInProgress = false;
            holeFallVolume = null;
        }

        private void BeginLavaBounce(
            BDHazardVolume volume)
        {
            FreezeSafePointUpdates();
            float damage = volume.Damage;
            health.ApplyUnavoidableDamage(damage);

            recoveryProtectedUntil =
                Time.unscaledTime +
                Mathf.Max(
                    0.1f,
                    lavaBounceDuration +
                    recoveryProtectionSeconds
                );

            if (health.IsDead)
            {
                HazardRecovered?.Invoke(
                    BDHazardType.Lava,
                    damage
                );
                return;
            }

            if (!TryResolveRecoveryPoint(
                    out lavaBounceTarget,
                    out lavaBounceRotation))
            {
                lavaBounceTarget = initialSpawnPosition;
                lavaBounceRotation = initialSpawnRotation;
            }

            lavaBounceInProgress = true;
            lavaBounceVolume = volume;
            lavaBounceStartedAt = Time.unscaledTime;
            lavaBounceStart = transform.position;

            lavaControllerWasEnabled =
                characterController != null &&
                characterController.enabled;
            lavaPlayerWasEnabled =
                playerController != null &&
                playerController.enabled;

            if (playerController != null)
                playerController.enabled = false;

            if (characterController != null)
                characterController.enabled = false;
        }

        private void TickLavaBounce()
        {
            float duration =
                Mathf.Max(0.08f, lavaBounceDuration);

            float t = Mathf.Clamp01(
                (Time.unscaledTime - lavaBounceStartedAt) /
                duration
            );

            Vector3 position = Vector3.Lerp(
                lavaBounceStart,
                lavaBounceTarget,
                t
            );

            position.y +=
                Mathf.Sin(t * Mathf.PI) *
                Mathf.Max(0f, lavaBounceHeight);

            transform.SetPositionAndRotation(
                position,
                Quaternion.Slerp(
                    transform.rotation,
                    lavaBounceRotation,
                    t
                )
            );

            if (t < 1f)
                return;

            transform.SetPositionAndRotation(
                lavaBounceTarget,
                lavaBounceRotation
            );

            RestoreControlState(
                lavaControllerWasEnabled,
                lavaPlayerWasEnabled
            );

            HazardRecovered?.Invoke(
                BDHazardType.Lava,
                lavaBounceVolume != null
                    ? lavaBounceVolume.Damage
                    : 10f
            );

            lavaBounceInProgress = false;
            lavaBounceVolume = null;
        }

        private void DetectForcedDisplacement()
        {
            Vector3 currentPosition = transform.position;

            if (!hasPreviousTrackedPosition)
            {
                previousTrackedPosition = currentPosition;
                hasPreviousTrackedPosition = true;
                return;
            }

            Vector3 displacement =
                currentPosition - previousTrackedPosition;
            previousTrackedPosition = currentPosition;
            displacement.y = 0f;

            float minimumDistance = Mathf.Max(
                0.01f,
                forcedDisplacementMinDistance
            );

            if (displacement.sqrMagnitude <
                minimumDistance * minimumDistance)
            {
                return;
            }

            if (playerController == null ||
                playerController.IsDashing)
            {
                return;
            }

            float deltaTime = Mathf.Max(
                0.0001f,
                Time.unscaledDeltaTime
            );
            float actualSpeed =
                displacement.magnitude / deltaTime;
            float expectedSpeed =
                playerController.HasMoveInput
                    ? playerController.EffectiveMoveSpeed
                    : 0f;

            Vector3 expectedDirection =
                playerController.LastMoveWorldDirection;
            Vector3 actualDirection =
                displacement.normalized;

            bool directionMismatch =
                !playerController.HasMoveInput ||
                expectedDirection.sqrMagnitude < 0.001f ||
                Vector3.Dot(
                    expectedDirection.normalized,
                    actualDirection
                ) < 0.35f;

            bool unexpectedlyFast =
                actualSpeed >
                expectedSpeed *
                    Mathf.Max(
                        1.05f,
                        forcedDisplacementSpeedMultiplier
                    ) +
                0.75f;

            if (!directionMismatch && !unexpectedlyFast)
                return;

            playerController.NotifyForcedGapEntry(
                Mathf.Max(
                    0.10f,
                    forcedDisplacementWindow
                )
            );
        }

        private void CheckActiveHazardContact()
        {
            if (characterController == null ||
                !characterController.enabled ||
                health == null ||
                health.IsDead ||
                IsRecoveryProtected)
            {
                return;
            }

            if (BDHazardVolume.TryFindTouchingLava(
                    characterController,
                    lavaSurfaceContactTolerance,
                    out BDHazardVolume lava))
            {
                BeginLavaBounce(lava);
                return;
            }

            Vector3 center =
                characterController.bounds.center;
            float footprintClearance = Mathf.Max(
                0.02f,
                characterController.radius * 0.10f
            );

            if (!BDHazardVolume.IsInsideHoleHorizontal(
                    center,
                    footprintClearance,
                    out BDHazardVolume hole))
            {
                return;
            }

            bool intentional =
                playerController != null &&
                playerController.HasRecentIntentionalGapEntry;

            if (intentional)
            {
                BeginHoleFall(hole);
                return;
            }

            RecoverImmediatelyWithoutDamage();
        }

        private void CheckGroundExit()
        {
            if (characterController == null ||
                !characterController.enabled ||
                characterController.isGrounded ||
                IsRecoveryProtected)
            {
                return;
            }

            if (Time.unscaledTime - lastGroundedAt <
                Mathf.Max(
                    0.05f,
                    accidentalGroundExitGrace))
            {
                return;
            }

            if (transform.position.y >
                lastSafePosition.y -
                Mathf.Max(
                    0.10f,
                    accidentalGroundExitDepth))
            {
                return;
            }

            bool intentional =
                playerController != null &&
                playerController.HasRecentIntentionalGapEntry;

            if (intentional)
            {
                BeginHoleFall(null);
                return;
            }

            RecoverImmediatelyWithoutDamage();
        }

        private void TickSafePointTracking()
        {
            if (characterController == null ||
                !characterController.enabled ||
                !characterController.isGrounded ||
                Time.unscaledTime <
                    safePointUpdatesBlockedUntil ||
                (playerController != null &&
                 playerController.HasRecentIntentionalGapEntry))
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
                groundedSafeSince =
                    Time.unscaledTime;
                return;
            }

            if (Time.unscaledTime - groundedSafeSince <
                Mathf.Max(
                    0f,
                    groundedStableDuration
                ))
            {
                return;
            }

            RecordSafePoint(
                candidate,
                transform.rotation,
                force: false
            );
        }

        public bool TryRecordCurrentPosition(
            bool force = false)
        {
            if (!TryResolveGroundedCandidate(
                    transform.position,
                    out Vector3 candidate))
            {
                return false;
            }

            if (!IsCandidateSafe(candidate))
                return false;

            RecordSafePoint(
                candidate,
                transform.rotation,
                force
            );
            return true;
        }

        private void FreezeSafePointUpdates()
        {
            groundedSafeSince = -1f;
            safePointUpdatesBlockedUntil = Mathf.Max(
                safePointUpdatesBlockedUntil,
                Time.unscaledTime +
                Mathf.Max(
                    0.50f,
                    postRecoverySafePointLock
                )
            );
        }

        private void RecordSafePoint(
            Vector3 position,
            Quaternion rotation,
            bool force)
        {
            if (!force &&
                hasSafePosition &&
                (position - lastSafePosition).sqrMagnitude <
                minimumSafePointMovement *
                minimumSafePointMovement)
            {
                return;
            }

            if (hasSafePosition)
            {
                previousSafePosition = lastSafePosition;
                previousSafeRotation = lastSafeRotation;
                hasPreviousSafePosition = true;
            }

            lastSafePosition = position;
            lastSafeRotation = Quaternion.Euler(
                0f,
                rotation.eulerAngles.y,
                0f
            );
            hasSafePosition = true;
        }

        private void RecoverImmediatelyWithoutDamage()
        {
            RecoverToLatestSafePointImmediate(
                characterController != null &&
                characterController.enabled,
                playerController != null &&
                playerController.enabled
            );

            recoveryProtectedUntil =
                Time.unscaledTime +
                Mathf.Max(
                    0.1f,
                    recoveryProtectionSeconds
                );
        }

        private void RecoverToLatestSafePointImmediate(
            bool enableController,
            bool enablePlayer)
        {
            bool rapidRecoveryLoop =
                Time.unscaledTime -
                lastRecoveryCompletedAt <=
                Mathf.Max(
                    0.50f,
                    rapidRecoveryLoopWindow
                );

            bool resolved = rapidRecoveryLoop
                ? TryResolveLoopBreakerPoint(
                    out Vector3 targetPosition,
                    out Quaternion targetRotation)
                : TryResolveRecoveryPoint(
                    out targetPosition,
                    out targetRotation);

            if (!resolved)
            {
                targetPosition = initialSpawnPosition;
                targetRotation = initialSpawnRotation;
            }

            if (characterController != null)
                characterController.enabled = false;

            transform.SetPositionAndRotation(
                targetPosition,
                targetRotation
            );

            Physics.SyncTransforms();

            RestoreControlState(
                enableController,
                enablePlayer
            );
        }

        private void RestoreControlState(
            bool enableController,
            bool enablePlayer)
        {
            if (characterController != null)
            {
                characterController.enabled =
                    enableController;
            }

            if (playerController != null)
            {
                playerController.enabled =
                    enablePlayer;

                if (enablePlayer)
                {
                    playerController
                        .ResetMotionAfterExternalTeleport();
                }
            }

            groundedSafeSince = -1f;
            safePointUpdatesBlockedUntil =
                Time.unscaledTime +
                Mathf.Max(
                    0.50f,
                    postRecoverySafePointLock
                );
            lastRecoveryCompletedAt =
                Time.unscaledTime;
            lastGroundedAt = Time.unscaledTime;
            nextSampleAt =
                Time.unscaledTime +
                Mathf.Max(0.05f, sampleInterval);
            previousTrackedPosition =
                transform.position;
            hasPreviousTrackedPosition = true;
                    hasMountedRecoveryAnchor = false;
            mountedRecoveryAnchorUntil = -999f;
}

        private bool TryResolveLoopBreakerPoint(
            out Vector3 position,
            out Quaternion rotation)
        {
            if (hasPreviousSafePosition &&
                TryResolveGroundedCandidate(
                    previousSafePosition,
                    out Vector3 previousCandidate) &&
                IsCandidateSafe(previousCandidate))
            {
                position = previousCandidate;
                rotation = previousSafeRotation;
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

            return TryResolveRecoveryPoint(
                out position,
                out rotation
            );
        }

        private bool TryResolveMountedRecoveryAnchor(
            out Vector3 position,
            out Quaternion rotation)
        {
            position = default;
            rotation = Quaternion.identity;

            if (!hasMountedRecoveryAnchor ||
                Time.unscaledTime >
                    mountedRecoveryAnchorUntil)
            {
                hasMountedRecoveryAnchor = false;
                return false;
            }

            float separation = Mathf.Max(
                0.75f,
                mountedRecoverySeparation
            );

            const int sampleCount = 12;

            for (int index = 0;
                 index < sampleCount;
                 index++)
            {
                float angle =
                    index *
                    (360f / sampleCount) *
                    Mathf.Deg2Rad;

                Vector3 offset = new Vector3(
                    Mathf.Cos(angle),
                    0f,
                    Mathf.Sin(angle)
                ) * separation;

                if (!TryResolveGroundedCandidate(
                        mountedRecoveryAnchor + offset,
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

            return false;
        }

        private bool TryResolveRecoveryPoint(
            out Vector3 position,
            out Quaternion rotation)
        {
            if (TryResolveMountedRecoveryAnchor(
                    out position,
                    out rotation))
            {
                return true;
            }


            if (hasPreviousSafePosition &&
                TryResolveGroundedCandidate(
                    previousSafePosition,
                    out Vector3 previousCandidate) &&
                IsCandidateSafe(previousCandidate))
            {
                position = previousCandidate;
                rotation = previousSafeRotation;
                return true;
            }

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

            int rings = Mathf.Max(
                1,
                fallbackSearchRings
            );
            float radiusStep = Mathf.Max(
                0.25f,
                fallbackSearchRadiusStep
            );

            for (int ring = 1; ring <= rings; ring++)
            {
                float radius = ring * radiusStep;
                int samples = Mathf.Max(8, ring * 8);

                for (int sample = 0;
                     sample < samples;
                     sample++)
                {
                    float angle =
                        sample *
                        (360f / samples) *
                        Mathf.Deg2Rad;

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
                Vector3.up *
                Mathf.Max(
                    0.5f,
                    groundProbeHeight
                );

            int hitCount = Physics.RaycastNonAlloc(
                origin,
                Vector3.down,
                groundHits,
                Mathf.Max(
                    1f,
                    groundProbeDistance
                ),
                ~0,
                QueryTriggerInteraction.Ignore
            );

            float bestDistance =
                float.PositiveInfinity;
            bool found = false;
            groundedPosition = requestedPosition;

            for (int i = 0;
                 i < hitCount;
                 i++)
            {
                RaycastHit hit = groundHits[i];

                if (hit.collider == null ||
                    IsOwnCollider(hit.collider))
                {
                    continue;
                }

                float groundAngle =
                    Vector3.Angle(
                        hit.normal,
                        Vector3.up
                    );

                if (groundAngle >
                    Mathf.Clamp(
                        maximumGroundAngle,
                        0f,
                        89f
                    ))
                {
                    continue;
                }

                if (hit.distance >= bestDistance)
                    continue;

                bestDistance = hit.distance;
                float controllerLift =
                    characterController != null
                        ? characterController.skinWidth + 0.22f
                        : 0.22f;

                groundedPosition =
                    hit.point +
                    Vector3.up *
                    Mathf.Max(
                        controllerLift,
                        recoveryVerticalOffset
                    );
                found = true;
            }

            return found;
        }

        private bool IsCandidateSafe(
            Vector3 candidate)
        {
            if (BDHazardVolume.IsRecoveryPointUnsafe(
                    candidate,
                    Mathf.Max(
                        0f,
                        hazardEdgeClearance
                    )))
            {
                return false;
            }

            if (characterController == null)
                return true;

            float radius = Mathf.Max(
                0.1f,
                characterController.radius * 0.88f
            );
            float height = Mathf.Max(
                radius * 2f,
                characterController.height
            );
            Vector3 center =
                candidate +
                characterController.center;
            float vertical = Mathf.Max(
                0f,
                height * 0.5f - radius
            );
            Vector3 bottom =
                center -
                Vector3.up * vertical;
            Vector3 top =
                center +
                Vector3.up * vertical;

            int count = Physics.OverlapCapsuleNonAlloc(
                bottom,
                top,
                radius,
                overlapBuffer,
                ~0,
                QueryTriggerInteraction.Collide
            );

            float groundAllowanceY =
                candidate.y + 0.18f;

            for (int i = 0; i < count; i++)
            {
                Collider other = overlapBuffer[i];

                if (other == null ||
                    !other.enabled ||
                    IsOwnCollider(other))
                {
                    continue;
                }

                if (other.GetComponentInParent<
                        BDHazardVolume>() != null)
                {
                    return false;
                }

                if (other.isTrigger &&
                    other.name.IndexOf(
                        "barrier",
                        StringComparison
                            .OrdinalIgnoreCase) < 0)
                {
                    continue;
                }

                if (!other.isTrigger &&
                    other.bounds.max.y <=
                        groundAllowanceY)
                {
                    continue;
                }

                return false;
            }

            return true;
        }

        private bool IsOwnCollider(
            Collider candidate)
        {
            if (candidate == null)
                return false;

            Transform candidateTransform =
                candidate.transform;

            return candidateTransform == transform ||
                   candidateTransform.IsChildOf(transform) ||
                   transform.IsChildOf(candidateTransform);
        }
    }
}
