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
        [SerializeField] private float holeFallDuration = 2.05f;
        [SerializeField] private float holeFallSpeed = 4.60f;
        [SerializeField] private float holeFallAccelerationMultiplier = 1.35f;
[Header("Lava Bounce")]
        [SerializeField] private float lavaBounceDuration = 0.42f;
        [SerializeField] private float lavaBounceHeight = 1.35f;
        [SerializeField, Range(0.55f, 1f)] private float lavaBounceDistanceMultiplier = 0.80f;
        [SerializeField] private float lavaSurfaceContactTolerance = 0.05f;

        [Header("Ground Safety")]
        [SerializeField] private float accidentalGroundExitGrace = 0.16f;
        [SerializeField] private float accidentalGroundExitDepth = 0.34f;

        [Header("Forced Movement")]
        [SerializeField] private float forcedDisplacementWindow = 0.85f;
        [SerializeField] private float forcedDisplacementMinDistance = 0.10f;
        [SerializeField] private float forcedDisplacementSpeedMultiplier = 1.35f;

        [Header("Combat Grounding Guard")]
        // BD COMBAT FLOOR LOSS GUARD V23
        [SerializeField] private float combatImpactGuardDuration = 0.75f;
        [SerializeField] private float combatImpactGroundGrace = 0.10f;
        [SerializeField] private float combatImpactRecoveryDepth = 0.22f;
        [SerializeField] private float combatGroundSupportDistance = 0.85f;
        [SerializeField] private float combatSafePointFreeze = 1.10f;

        [Header("Recovery")]
        [SerializeField] private float recoveryProtectionSeconds = 1.0f;
        [SerializeField] private float postRecoverySafePointLock = 1.65f;
        [SerializeField] private float rapidRecoveryLoopWindow = 3.25f;
        [SerializeField] private float recoveryVerticalOffset = 0.38f;

        [Header("Hole Local Recovery")]
        // BD NEAR-HOLE LOCAL RECOVERY V23R2
        [SerializeField] private float holeRespawnEdgeClearance = 0.82f;
        [SerializeField] private float holeRespawnSearchStep = 0.32f;
        [SerializeField] private int holeRespawnSearchRings = 3;

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
        private Vector3 localHoleRecoveryPosition;
        private Quaternion localHoleRecoveryRotation;
        private bool hasLocalHoleRecovery;
        private float combatImpactStartedAt = -999f;
        private float combatImpactGuardUntil = -999f;
        private float combatImpactStartY;

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
            ApplyHazardFeelProfile();

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

        private void ApplyHazardFeelProfile()
        {
            holeFallDuration = 2.05f;
            holeFallSpeed = 4.60f;
            holeFallAccelerationMultiplier = 1.35f;
            lavaBounceDistanceMultiplier = 0.80f;
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

            CheckCombatGroundingGuard();
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

        public void NotifyCombatImpact()
        {
            if (!Application.isPlaying ||
                characterController == null ||
                health == null ||
                health.IsDead)
            {
                return;
            }

            combatImpactStartedAt = Time.unscaledTime;
            combatImpactGuardUntil =
                combatImpactStartedAt +
                Mathf.Max(0.15f, combatImpactGuardDuration);
            combatImpactStartY = transform.position.y;
            groundedSafeSince = -1f;
            safePointUpdatesBlockedUntil = Mathf.Max(
                safePointUpdatesBlockedUntil,
                combatImpactStartedAt +
                Mathf.Max(0.25f, combatSafePointFreeze)
            );
        }

        private void CheckCombatGroundingGuard()
        {
            if (Time.unscaledTime > combatImpactGuardUntil ||
                characterController == null ||
                !characterController.enabled ||
                characterController.isGrounded ||
                IsRecoveryProtected ||
                (playerController != null &&
                 playerController.HasRecentIntentionalGapEntry))
            {
                return;
            }

            if (Time.unscaledTime - combatImpactStartedAt <
                Mathf.Max(0.02f, combatImpactGroundGrace))
            {
                return;
            }

            bool droppedBelowImpact =
                transform.position.y <
                combatImpactStartY -
                Mathf.Max(0.05f, combatImpactRecoveryDepth);
            bool hasSupport = HasWalkableGroundSupport(
                Mathf.Max(0.25f, combatGroundSupportDistance)
            );

            if (!droppedBelowImpact && hasSupport)
                return;

            combatImpactGuardUntil = -999f;
            RecoverImmediatelyWithoutDamage();
        }

        private bool HasWalkableGroundSupport(float extraDistance)
        {
            Vector3 origin = characterController.bounds.center;
            float radius = Mathf.Max(
                0.08f,
                characterController.radius * 0.72f
            );
            float distance =
                characterController.height * 0.5f +
                Mathf.Max(0.05f, extraDistance);

            int hitCount = Physics.SphereCastNonAlloc(
                origin,
                radius,
                Vector3.down,
                groundHits,
                distance,
                ~0,
                QueryTriggerInteraction.Ignore
            );

            for (int i = 0; i < hitCount; i++)
            {
                RaycastHit hit = groundHits[i];
                if (!IsValidRecoveryGroundCollider(hit.collider))
                    continue;

                if (Vector3.Angle(hit.normal, Vector3.up) <=
                    Mathf.Clamp(maximumGroundAngle, 0f, 89f))
                {
                    return true;
                }
            }

            return false;
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

        private void CaptureLocalHoleRecoveryAnchor(
            BDHazardVolume volume)
        {
            hasLocalHoleRecovery = false;

            if (volume == null)
                return;

            float controllerClearance =
                characterController != null
                    ? characterController.radius +
                      characterController.skinWidth + 0.12f
                    : 0.62f;
            float edgeClearance = Mathf.Max(
                0.55f,
                holeRespawnEdgeClearance,
                controllerClearance
            );

            if (!volume.TryResolveNearestHorizontalExitPoint(
                    transform.position,
                    edgeClearance,
                    out Vector3 nearestExit))
            {
                return;
            }

            float bestDistance = float.PositiveInfinity;
            Vector3 bestPosition = default;
            bool found = false;
            int rings = Mathf.Max(1, holeRespawnSearchRings);
            float step = Mathf.Max(0.15f, holeRespawnSearchStep);

            for (int ring = 0; ring <= rings; ring++)
            {
                int samples = ring == 0 ? 1 : Mathf.Max(8, ring * 8);
                float radius = ring * step;

                for (int sample = 0; sample < samples; sample++)
                {
                    float angle = samples == 1
                        ? 0f
                        : sample * (360f / samples) * Mathf.Deg2Rad;
                    Vector3 requested = nearestExit + new Vector3(
                        Mathf.Cos(angle),
                        0f,
                        Mathf.Sin(angle)
                    ) * radius;

                    if (!TryResolveGroundedCandidate(
                            requested,
                            out Vector3 candidate))
                    {
                        continue;
                    }

                    float localHazardClearance = Mathf.Max(
                        0.20f,
                        edgeClearance * 0.34f
                    );
                    if (!IsCandidateSafe(
                            candidate,
                            localHazardClearance) ||
                        volume.ContainsHorizontalPoint(
                            candidate,
                            Mathf.Max(0.05f, controllerClearance * 0.20f)))
                    {
                        continue;
                    }

                    Vector3 delta = candidate - transform.position;
                    delta.y = 0f;
                    float distance = delta.sqrMagnitude;
                    if (distance >= bestDistance)
                        continue;

                    bestDistance = distance;
                    bestPosition = candidate;
                    found = true;
                }
            }

            if (!found)
                return;

            localHoleRecoveryPosition = bestPosition;
            localHoleRecoveryRotation = Quaternion.Euler(
                0f,
                transform.rotation.eulerAngles.y,
                0f
            );
            hasLocalHoleRecovery = true;
        }

        private bool TryResolveLocalHoleRecoveryAnchor(
            out Vector3 position,
            out Quaternion rotation)
        {
            position = default;
            rotation = Quaternion.identity;

            if (!hasLocalHoleRecovery ||
                !TryResolveGroundedCandidate(
                    localHoleRecoveryPosition,
                    out Vector3 candidate) ||
                !IsCandidateSafe(
                    candidate,
                    Mathf.Max(0.20f, holeRespawnEdgeClearance * 0.34f)))
            {
                return false;
            }

            position = candidate;
            rotation = localHoleRecoveryRotation;
            return true;
        }

        private void BeginHoleFall(
            BDHazardVolume volume)
        {
            CaptureLocalHoleRecoveryAnchor(volume);
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
            float duration = Mathf.Max(0.12f, holeFallDuration);
            float elapsed = Time.unscaledTime - holeFallStartedAt;
            float normalizedFall = Mathf.Clamp01(elapsed / duration);
            float acceleration = Mathf.Lerp(
                1f,
                Mathf.Max(1f, holeFallAccelerationMultiplier),
                normalizedFall
            );
            float speed = Mathf.Max(0.25f, holeFallSpeed) * acceleration;

            transform.position +=
                Vector3.down * speed * Time.unscaledDeltaTime;

            if (elapsed < duration)
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

            lavaBounceTarget =
                ResolveReducedLavaBounceTarget(
                    lavaBounceStart,
                    lavaBounceTarget
                );
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

        private Vector3 ResolveReducedLavaBounceTarget(
            Vector3 start,
            Vector3 fullSafeTarget)
        {
            Vector3 horizontal = fullSafeTarget - start;
            horizontal.y = 0f;

            if (horizontal.sqrMagnitude < 0.25f)
                return fullSafeTarget;

            float firstFactor = Mathf.Clamp(
                lavaBounceDistanceMultiplier,
                0.55f,
                1f
            );

            for (float factor = firstFactor;
                 factor < 1f;
                 factor += 0.06f)
            {
                Vector3 requested = Vector3.Lerp(
                    start,
                    fullSafeTarget,
                    factor
                );

                if (!TryResolveGroundedCandidate(
                        requested,
                        out Vector3 candidate))
                {
                    continue;
                }

                if (!IsCandidateSafe(candidate))
                    continue;

                return candidate;
            }

            return fullSafeTarget;
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
            hasLocalHoleRecovery = false;
            hasMountedRecoveryAnchor = false;
            mountedRecoveryAnchorUntil = -999f;
        }

        private bool TryResolveLoopBreakerPoint(
            out Vector3 position,
            out Quaternion rotation)
        {
            if (TryResolveLocalHoleRecoveryAnchor(
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
            if (TryResolveLocalHoleRecoveryAnchor(
                    out position,
                    out rotation))
            {
                return true;
            }

            if (TryResolveMountedRecoveryAnchor(
                    out position,
                    out rotation))
            {
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
                Mathf.Max(0.5f, groundProbeHeight);

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

                if (!IsValidRecoveryGroundCollider(hit.collider))
                    continue;

                float groundAngle = Vector3.Angle(hit.normal, Vector3.up);
                if (groundAngle > Mathf.Clamp(maximumGroundAngle, 0f, 89f))
                    continue;

                if (hit.distance >= bestDistance)
                    continue;

                bestDistance = hit.distance;
                float rootHeight = ResolveCharacterControllerRootHeightAboveGround();
                groundedPosition =
                    hit.point +
                    Vector3.up * Mathf.Max(rootHeight, recoveryVerticalOffset);
                found = true;
            }

            return found;
        }

        // BD CHARACTER CONTROLLER ROOT-SAFE RECOVERY V23
        private float ResolveCharacterControllerRootHeightAboveGround()
        {
            if (characterController == null)
                return Mathf.Max(0.22f, recoveryVerticalOffset);

            float capsuleBottomFromRoot =
                characterController.center.y -
                characterController.height * 0.5f;
            return
                -capsuleBottomFromRoot +
                Mathf.Max(0.02f, characterController.skinWidth) +
                0.03f;
        }

        private bool IsValidRecoveryGroundCollider(Collider candidate)
        {
            if (candidate == null ||
                !candidate.enabled ||
                candidate.isTrigger ||
                IsOwnCollider(candidate))
            {
                return false;
            }

            if (candidate.GetComponentInParent<BDHazardVolume>() != null ||
                candidate.GetComponentInParent<BDHealth>() != null ||
                candidate.GetComponentInParent<BDHorseHealth>() != null ||
                candidate.GetComponentInParent<CharacterController>() != null ||
                candidate.GetComponentInParent<BDWallSurfaceProfile>() != null)
            {
                return false;
            }

            Rigidbody body = candidate.attachedRigidbody;
            if (body != null && !body.isKinematic)
                return false;

            return true;
        }


        private bool IsCandidateSafe(
            Vector3 candidate)
        {
            return IsCandidateSafe(
                candidate,
                Mathf.Max(0f, hazardEdgeClearance)
            );
        }

        private bool IsCandidateSafe(
            Vector3 candidate,
            float hazardClearance)
        {
            if (BDHazardVolume.IsRecoveryPointUnsafe(
                    candidate,
                    Mathf.Max(0f, hazardClearance)))
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
