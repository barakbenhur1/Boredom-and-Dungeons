using UnityEngine;

#pragma warning disable 0414

#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace BoredomAndDungeons
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(BDHorseHealth))]
    [RequireComponent(typeof(BDHorseHazardSafety))]
    public sealed class BDHorseController : MonoBehaviour
    {
        private enum HorseState
        {
            Idle,
            Mounted,
            MovingToSafeSpot,
            WaitingSafe,
            FleeingToSafeSpot,
            ReturningToPlayer,
            Fainted
        }

        private readonly struct DismountPlan
        {
            public readonly Vector3 Destination;
            public readonly Vector3 Direction;
            public readonly float Duration;
            public readonly float ArcHeight;
            public readonly bool EnemyNear;
            public readonly bool HadInput;

            public DismountPlan(Vector3 destination, Vector3 direction, float duration, float arcHeight, bool enemyNear, bool hadInput)
            {
                Destination = destination;
                Direction = direction;
                Duration = duration;
                ArcHeight = arcHeight;
                EnemyNear = enemyNear;
                HadInput = hadInput;
            }
        }

        [Header("References")]
        [SerializeField] private Transform rider;
        [SerializeField] private Transform safeSpot;

        [Header("Movement")]
        [SerializeField] private float baseMoveSpeed = 5.2f;
        [SerializeField] private float mountedMoveSpeed = 9.0f;
        [SerializeField] private float mountedMoveAcceleration = 24f;
        [SerializeField] private float mountedMoveDeceleration = 30f;
        [SerializeField] private float rotationSpeed = 32f;
        [SerializeField] private float mountedTravelTurnDegreesPerSecond = 112f;
        [SerializeField] private float safeSpotReachDistance = 0.5f;

        [Header("Horse Jump")]
        [SerializeField] private float horseJumpHeight = 1.45f;
        [SerializeField] private float horseGravity = -26f;
        [SerializeField] private float groundedStickVelocity = -2f;
        [SerializeField] private bool allowHorseJumpOnlyMounted = true;

        [Header("Interaction")]
        [SerializeField] private float interactionRange = 3.25f;
        [SerializeField] private float healPerSecond = 28f;

        [Header("Start Beside Player")]
        // BD HORSE START-BESIDE-PLAYER FIX
        [SerializeField] private bool placeBesidePlayerOnStart = true;
        [SerializeField] private Vector3 startLocalOffsetFromPlayer =
            new Vector3(2.35f, 0f, 0.45f);
        [SerializeField] private bool faceSameDirectionAsPlayerOnStart = true;
        [SerializeField] private float nearbyIdleNoTrackingRadius = 4.25f;
        [Header("Clean Game Start")]
        [SerializeField] private float startupCalmSeconds = 2.50f;
        [SerializeField] private float startupHazardClearance = 1.65f;
        [SerializeField] private float startupGroundProbeHeight = 4.0f;
        [SerializeField] private float startupGroundProbeDistance = 8.0f;
        [SerializeField] private float combatAwarenessRadius = 12.0f;
        private float startupCalmUntil = -999f;
        private bool startPositionApplied;

        [Header("Horse Healing Curve")]
        [SerializeField] private float minHealSpeedMultiplier = 0.22f;
        [SerializeField] private float maxHealSpeedMultiplier = 1.85f;
        [SerializeField] private float healRampSeconds = 2.25f;
        [SerializeField] private float finalEighthSlowdownStart = 0.875f;
        [SerializeField] private float finalHealSpeedMultiplier = 0.32f;
        [SerializeField] private float healingProtectionRefresh = 0.35f;
        [SerializeField] private float releasedHealingProtectionRefresh = 0.35f;

        private bool healingSessionActive;
        private bool healingReleaseHoldActive;
        private float healingSessionStartedAt;
        private float healingStartHealth;
        private float healingHeldFloor;
        [SerializeField] private Vector3 mountedLocalOffset = new Vector3(0f, 1.2f, -0.15f);
        [SerializeField] private Vector3 fallbackDismountOffset = new Vector3(1.5f, 0f, 0f);
        [SerializeField] private bool runToSafeSpotAfterDismount = false;
        [SerializeField] private bool runToSafeSpotAfterCombatDismount = true;

        [Header("Return To Player After Combat")]
        [SerializeField] private bool returnToPlayerAfterDangerEnds = true;
        [SerializeField] private float returnToPlayerDistance = 9.0f;
        [SerializeField] private float returnComfortRadius = 4.2f;
        [SerializeField] private float returnTooCloseRadius = 2.8f;
        [SerializeField] private float returnStopHysteresis = 0.45f;
        [SerializeField] private bool rotateTowardPlayerWhenReturning = true;
        [SerializeField] private float returnMoveSpeed = 5.8f;
        [SerializeField] private float returnBackAwaySpeed = 2.8f;
        [SerializeField] private float dangerEndedDelay = 0.9f;
        private float lastCombatActiveAt = -999f;

        [Header("Dismount Leap")]
        [SerializeField] private float normalDismountDistance = 1.35f;
        [SerializeField] private float normalDismountDuration = 0.22f;
        [SerializeField] private float normalDismountArcHeight = 0.45f;
        [SerializeField] private float enemyAwareDismountRadius = 5.2f;
        [SerializeField] private float enemyDismountDistance = 2.55f;
        [SerializeField] private float enemyDismountDuration = 0.34f;
        [SerializeField] private float enemyDismountArcHeight = 0.90f;
        [SerializeField] private float inputDeadZone = 0.12f;

        [Header("Buck / Throw Rider")]
        [SerializeField] private float buckThrowDistance = 2.8f;
        [SerializeField] private float buckThrowDuration = 0.38f;
        [SerializeField] private float buckThrowArcHeight = 1.05f;

        [Header("Mouse Look While Mounted")]
        [SerializeField] private bool enableMountedMouseLook = true;
        [SerializeField] private bool mountedMovementRelativeToMouseLook = true;
        [SerializeField] private float mountedMouseAimMinDistance = 2.20f; // expanded mounted center dead zone radius
        [SerializeField] private float mountedMouseScreenCenterDeadZonePixels = 88f;
        [SerializeField] private float mountedMouseFrontDeadZoneDegrees = 3f;
        [SerializeField] private float mountedMouseModelFrontConeDegrees = 60f;
        [SerializeField] private float mountedMouseAimIdleTurnDegreesPerSecond = 150f;
        [SerializeField] private float mountedMouseAimMovingTurnDegreesPerSecond = 220f;
        [SerializeField] private float mountedMouseAimTargetSmoothing = 22f;
        [SerializeField] private float mountedMousePointAimSmoothing = 60f;
        [SerializeField] private float mountedMouseYawDegreesPerPixel = 0.52f;
        [SerializeField] private float mountedMouseDeltaDeadZonePixels = 0.0f;
        [SerializeField] private float mountedMouseMaxDeltaPixelsPerFrame = 220f;
        [SerializeField] private float mountedMouseYawSmoothing = 90f;
        [SerializeField] private bool mountedMouseImmediateYaw = true;
        [SerializeField] private bool lockCursorForMountedMouseLook = true;
        private float mountedTargetYaw;
        private float mountedCurrentYaw;
        private bool mountedYawInitialized;
        private Vector3 lastMountedAimDirection = Vector3.forward;
        private Vector3 smoothedMountedHorizontalVelocity;
        private Vector3 targetMountedAimDirection = Vector3.forward;
        private Vector3 smoothedMountedTargetAimDirection = Vector3.forward;
        private Vector3 smoothedMountedTravelDirection = Vector3.forward;
        private Vector3 mountedMovementReferenceForward = Vector3.forward;

        [Header("Touch / Mouse Drag")]
        [SerializeField] private bool enableMouseDragAsFingerTrace = false;
        [SerializeField] private float dragDeadZonePixels = 8f;
        [SerializeField] private float dragMaxPixels = 150f;

        [Header("Debug")]
        [SerializeField] private bool showDebugOverlay = false;

        private CharacterController controller;
        private BDHorseHealth health;
        private BDPlayerController playerController;
        private CharacterController playerCharacterController;
        private BDPlayerDismountLeap playerDismountLeap;
        private BDHorseHazardSafety hazardSafety;

        private HorseState state = HorseState.Idle;
        private bool playerInRange;
        private string lastAction = "none";

        // BD C04 EXTERNAL HORSE CONTROL V1
        private bool externalControlLock;
        private string externalControlReason = "none";
        private HorseState stateBeforeExternalControl =
            HorseState.Idle;

        private bool pointerDragging;
        private Vector2 pointerDragStart;
        private Vector2 lastRideInput;
        private string lastRideInputSource = "none";
        private string lastDismountMode = "none";

        private float verticalVelocity;
        private bool jumpedThisFrame;
        private Vector3 riderOriginalScale = Vector3.one;
        private Quaternion riderOriginalLocalRotation = Quaternion.identity;

        public bool IsMounted => state == HorseState.Mounted;
        public float CurrentMountedPlanarSpeed =>
            smoothedMountedHorizontalVelocity.magnitude;
        public float MountedMaximumMoveSpeed =>
            Mathf.Max(0.1f, mountedMoveSpeed);
        public Vector3 CurrentMountedTravelDirection
        {
            get
            {
                Vector3 direction = smoothedMountedHorizontalVelocity;
                direction.y = 0f;
                if (direction.sqrMagnitude < 0.001f)
                    direction = transform.forward;
                direction.y = 0f;
                return direction.sqrMagnitude > 0.001f
                    ? direction.normalized
                    : Vector3.forward;
            }
        }
        public bool IsStartupCalm =>
            Time.unscaledTime < startupCalmUntil;
        public bool HasImmediateCombatThreat =>
            HasLivingEnemyNearHorseOrPlayer(
                combatAwarenessRadius
            );
        public bool CanReactToCombatThreat =>
            !IsStartupCalm &&
            HasImmediateCombatThreat;
        public Transform Rider => rider;
        public Vector3 LastMountedAimDirection => lastMountedAimDirection.sqrMagnitude > 0.001f ? lastMountedAimDirection.normalized : transform.forward;
        public Vector2 LastRideInput => lastRideInput;
        public bool HasRideMoveInput =>
            lastRideInput.sqrMagnitude > 0.0001f;
        public bool IsMountedStationary =>
            IsMounted &&
            !HasRideMoveInput &&
            smoothedMountedHorizontalVelocity.sqrMagnitude <= 0.04f;
        public bool IsPlayerInInteractionRange => playerInRange;
        public bool CanOfferMountAction =>
            IsAvailable && playerInRange;
        public bool NeedsHealing =>
            health != null &&
            (health.IsFainted ||
             health.CurrentHealth < health.MaxHealth - 0.5f);
        public Vector3 LastMountedMovementDirection
        {
            get
            {
                Vector3 direction = smoothedMountedHorizontalVelocity;
                direction.y = 0f;

                if (direction.sqrMagnitude > 0.0025f)
                    return direction.normalized;

                if (lastRideInput.sqrMagnitude > 0.0001f)
                {
                    Vector3 fallback = ToMountedPlayerRelativeMove(lastRideInput);
                    fallback.y = 0f;

                    if (fallback.sqrMagnitude > 0.0025f)
                        return fallback.normalized;
                }

                return Vector3.zero;
            }
        }
        public bool IsAvailable =>
            !externalControlLock &&
            !health.IsFainted &&
            state != HorseState.Mounted;
        public bool HasSafeSpot => safeSpot != null;
        public bool IsGrounded =>
            controller != null && controller.isGrounded;
        public bool IsExternallyControlled =>
            externalControlLock;
        public string ExternalControlReason =>
            externalControlReason;
        public bool IsFainted =>
            health != null && health.IsFainted;

        public void ResetForCleanGameStart(
            float calmSeconds)
        {
            float safeCalmSeconds =
                Mathf.Max(0f, calmSeconds);

            startupCalmUntil =
                Mathf.Max(
                    startupCalmUntil,
                    Time.unscaledTime +
                    safeCalmSeconds
                );

            state = HorseState.Idle;
            stateBeforeExternalControl =
                HorseState.Idle;
            externalControlLock = false;
            externalControlReason = "none";

            smoothedMountedHorizontalVelocity =
                Vector3.zero;
            smoothedMountedTravelDirection =
                transform.forward;
            smoothedMountedTravelDirection.y = 0f;
            lastRideInput = Vector2.zero;
            verticalVelocity =
                groundedStickVelocity;
            mountedYawInitialized = false;
            pointerDragging = false;

            healingSessionActive = false;
            healingReleaseHoldActive = false;
            healingSessionStartedAt = 0f;
            healingStartHealth = 0f;
            healingHeldFloor = 0f;

            playerInRange = false;
            lastCombatActiveAt = -999f;
            lastDismountMode = "none";
            lastAction =
                "clean run start - full health calm idle";
        }

        public void SetExternalControlLock(
            bool locked,
            string reason)
        {
            if (externalControlLock == locked)
            {
                if (locked &&
                    !string.IsNullOrWhiteSpace(reason))
                {
                    externalControlReason = reason;
                }

                return;
            }

            if (locked)
            {
                stateBeforeExternalControl = state;
                externalControlLock = true;
                externalControlReason =
                    string.IsNullOrWhiteSpace(reason)
                        ? "external"
                        : reason;

                smoothedMountedHorizontalVelocity =
                    Vector3.zero;
                lastRideInput = Vector2.zero;
                verticalVelocity = groundedStickVelocity;
                return;
            }

            externalControlLock = false;
            externalControlReason = "none";
            smoothedMountedHorizontalVelocity =
                Vector3.zero;
            lastRideInput = Vector2.zero;
            verticalVelocity = groundedStickVelocity;

            if (health != null && health.IsFainted)
            {
                state = HorseState.Fainted;
                return;
            }

            HorseState restored =
                stateBeforeExternalControl;

            if (restored == HorseState.Mounted ||
                restored == HorseState.Fainted ||
                restored == HorseState.FleeingToSafeSpot)
            {
                restored = HorseState.Idle;
            }

            state = restored;
        }

        public bool BeginMountedRunIntro(
            Transform introRider,
            Vector3 startPosition,
            Vector3 facingDirection)
        {
            if (health == null)
                health = GetComponent<BDHorseHealth>();

            if (health == null || health.IsFainted)
                return false;

            if (introRider != null)
                rider = introRider;

            if (rider == null)
                rider = BDTargetFinder.FindPlayer();

            if (rider == null)
                return false;

            CachePlayerComponents();

            if (externalControlLock &&
                externalControlReason != "mounted run intro")
            {
                return false;
            }

            externalControlLock = false;
            externalControlReason = "none";
            state = HorseState.Idle;

            bool controllerWasEnabled =
                controller != null &&
                controller.enabled;

            if (controllerWasEnabled)
                controller.enabled = false;

            transform.position = startPosition;

            facingDirection.y = 0f;

            if (facingDirection.sqrMagnitude > 0.001f)
            {
                transform.rotation =
                    Quaternion.LookRotation(
                        facingDirection.normalized,
                        Vector3.up
                    );
            }

            if (controllerWasEnabled)
                controller.enabled = true;

            Physics.SyncTransforms();

            Mount();

            if (state != HorseState.Mounted)
                return false;

            stateBeforeExternalControl =
                HorseState.Mounted;
            externalControlLock = true;
            externalControlReason =
                "mounted run intro";

            smoothedMountedHorizontalVelocity =
                Vector3.zero;
            lastRideInput = Vector2.zero;
            verticalVelocity =
                groundedStickVelocity;
            mountedYawInitialized = false;

            bool bound = MaintainMountedRunIntroBinding(rider);
            lastAction = bound
                ? "mounted run intro active"
                : "mounted run intro rider binding failed";
            return bound;
        }

        public void CompleteMountedRunIntro()
        {
            if (!externalControlLock ||
                externalControlReason != "mounted run intro")
            {
                return;
            }

            externalControlLock = false;
            externalControlReason = "none";
            stateBeforeExternalControl =
                HorseState.Mounted;
            state = HorseState.Mounted;

            smoothedMountedHorizontalVelocity =
                Vector3.zero;
            lastRideInput = Vector2.zero;
            verticalVelocity =
                groundedStickVelocity;
            mountedYawInitialized = false;
            InitializeMountedYawFromTransform();

            if (playerController != null)
                playerController.enabled = false;

            if (playerCharacterController != null)
                playerCharacterController.enabled = true; // BD MOUNTED RIDER HURTBOX ENABLED V10

            PlaceRiderOnMountPoint();
            lastAction =
                "mounted run intro complete";
        }

        public void MoveByExternalControl(
            Vector3 motion,
            Vector3 facingDirection,
            float turnSpeedMultiplier = 1f)
        {
            if (!externalControlLock)
                return;

            MoveHorse(motion);

            facingDirection.y = 0f;

            if (facingDirection.sqrMagnitude >= 0.001f)
            {
                float previousRotationSpeed = rotationSpeed;
                rotationSpeed = Mathf.Max(
                    0.1f,
                    previousRotationSpeed *
                    Mathf.Max(0.1f, turnSpeedMultiplier)
                );

                RotateToward(facingDirection);
                rotationSpeed = previousRotationSpeed;
            }

            // BD EXTERNAL-CONTROL RIDER FOLLOW V23R19E
            // The horse controller is deliberately disabled during the mounted
            // entrance, so its normal Update cannot keep the unparented rider
            // on the mount point. External movement must move the rider too.
            if (state == HorseState.Mounted && rider != null)
                PlaceRiderOnMountPoint();
        }
        private void ApplyNaturalHorseMovementProfile()
        {
            baseMoveSpeed = 5.6f;
            mountedMoveSpeed = 9.6f;
            // BD COHERENT MOUNTED MOUSE STEERING V23R8
            mountedMoveAcceleration = 14f;
            mountedMoveDeceleration = 20f;
            rotationSpeed = 8.0f;
            mountedTravelTurnDegreesPerSecond = 96f;
            mountedMouseScreenCenterDeadZonePixels = 104f;
            mountedMouseFrontDeadZoneDegrees = 5f;
            mountedMouseAimIdleTurnDegreesPerSecond = 92f;
            mountedMouseAimMovingTurnDegreesPerSecond = 132f;
            mountedMouseAimTargetSmoothing = 9f;
            mountedMousePointAimSmoothing = 18f;
            mountedMouseImmediateYaw = false;
        }


        private void Awake()
        {
            ApplyNaturalHorseMovementProfile();
            smoothedMountedTravelDirection = transform.forward;
            smoothedMountedTravelDirection.y = 0f;

            controller = GetComponent<CharacterController>();
            hazardSafety = GetComponent<BDHorseHazardSafety>();

            if (hazardSafety == null)
                hazardSafety = gameObject.AddComponent<BDHorseHazardSafety>();
            health = GetComponent<BDHorseHealth>();
            EnsureContextActionPrompts();
            if (GetComponent<BDHorseImpactAttack>() == null)
                gameObject.AddComponent<BDHorseImpactAttack>();
            health.Fainted += OnFainted;
            health.Recovered += OnRecovered;
            health.DamageBurstTriggered += OnHorseDamageBurstTriggered;

            ResetForCleanGameStart(
                startupCalmSeconds
            );
        }

        private void EnsureContextActionPrompts()
        {
            if (!Application.isPlaying)
                return;

            if (GetComponent<BDHorseContextActionPrompts>() == null)
                gameObject.AddComponent<BDHorseContextActionPrompts>();
        }

        private void Start()
        {
            if (rider == null)
                rider = BDTargetFinder.FindPlayer();

            CachePlayerComponents();
            ResetForCleanGameStart(
                startupCalmSeconds
            );

            if (health != null)
            {
                health.ResetForCleanGameStart(
                    startupCalmSeconds
                );
            }

            PlaceHorseBesidePlayerAtStart();
            ResolveSafeSpotIfNeeded();
        }
        private void PlaceHorseBesidePlayerAtStart()
        {
            if (startPositionApplied ||
                !placeBesidePlayerOnStart ||
                rider == null ||
                state == HorseState.Mounted)
            {
                return;
            }

            Vector3 horizontalOffset =
                rider.right * startLocalOffsetFromPlayer.x +
                rider.forward * startLocalOffsetFromPlayer.z;

            Vector3 requestedPosition =
                rider.position +
                horizontalOffset +
                Vector3.up * startLocalOffsetFromPlayer.y;

            Vector3 targetPosition = requestedPosition;

            bool foundSafeStart =
                TryResolveSafeStartPosition(
                    requestedPosition,
                    out targetPosition
                );

            bool controllerWasEnabled =
                controller != null &&
                controller.enabled;

            if (controllerWasEnabled)
                controller.enabled = false;

            transform.position = targetPosition;

            if (faceSameDirectionAsPlayerOnStart)
            {
                Vector3 playerForward = rider.forward;
                playerForward.y = 0f;

                if (playerForward.sqrMagnitude > 0.001f)
                {
                    transform.rotation =
                        Quaternion.LookRotation(
                            playerForward.normalized,
                            Vector3.up
                        );
                }
            }

            if (controllerWasEnabled)
                controller.enabled = true;

            Physics.SyncTransforms();

            if (health != null)
            {
                health.ResetForCleanGameStart(
                    startupCalmSeconds
                );
            }

            state = HorseState.Idle;
            lastCombatActiveAt = -999f;
            lastAction = foundSafeStart
                ? "clean start beside player"
                : "clean start fallback - no safe candidate";
            startPositionApplied = true;
        }

        // BD HORSE CLEAN START V1
        private bool TryResolveSafeStartPosition(
            Vector3 requested,
            out Vector3 resolved)
        {
            resolved = requested;

            Vector3 fromPlayer =
                requested - rider.position;

            fromPlayer.y = 0f;

            if (fromPlayer.sqrMagnitude < 0.001f)
                fromPlayer = rider.right;

            fromPlayer.Normalize();

            float baseRadius = Mathf.Max(
                1.75f,
                new Vector2(
                    requested.x - rider.position.x,
                    requested.z - rider.position.z
                ).magnitude
            );

            float[] angleOffsets =
            {
                0f, 45f, -45f, 90f,
                -90f, 135f, -135f, 180f
            };

            float[] radiusOffsets =
            {
                0f, 0.85f, 1.70f
            };

            for (int radiusIndex = 0;
                 radiusIndex < radiusOffsets.Length;
                 radiusIndex++)
            {
                float radius =
                    baseRadius +
                    radiusOffsets[radiusIndex];

                for (int angleIndex = 0;
                     angleIndex < angleOffsets.Length;
                     angleIndex++)
                {
                    Vector3 direction =
                        Quaternion.AngleAxis(
                            angleOffsets[angleIndex],
                            Vector3.up
                        ) *
                        fromPlayer;

                    Vector3 candidate =
                        rider.position +
                        direction.normalized *
                        radius;

                    if (!TryResolveHorseStartGround(
                            candidate,
                            out Vector3 grounded))
                    {
                        continue;
                    }

                    if (BDHazardVolume.IsRecoveryPointUnsafe(
                            grounded,
                            Mathf.Max(
                                0.25f,
                                startupHazardClearance
                            )))
                    {
                        continue;
                    }

                    if (!HasHorseStartClearance(grounded))
                        continue;

                    // BD DEATH RESTART HORSE ROOT GROUNDING V15
                    // Clearance is validated against the real surface point.
                    // The CharacterController root is then lifted so its
                    // capsule bottom, not its transform pivot, rests on ground.
                    resolved =
                        ResolveHorseRootPositionFromGroundAnchor(
                            grounded
                        );
                    return true;
                }
            }

            return false;
        }

        private bool TryResolveHorseStartGround(
            Vector3 requested,
            out Vector3 grounded)
        {
            Vector3 origin =
                requested +
                Vector3.up *
                Mathf.Max(
                    1f,
                    startupGroundProbeHeight
                );

            RaycastHit[] hits = Physics.RaycastAll(
                origin,
                Vector3.down,
                Mathf.Max(
                    2f,
                    startupGroundProbeHeight +
                    startupGroundProbeDistance
                ),
                ~0,
                QueryTriggerInteraction.Ignore
            );

            System.Array.Sort(
                hits,
                (left, right) =>
                    left.distance.CompareTo(
                        right.distance)
            );

            for (int index = 0;
                 index < hits.Length;
                 index++)
            {
                RaycastHit hit = hits[index];

                if (hit.collider == null ||
                    hit.collider.isTrigger)
                {
                    continue;
                }

                Transform hitTransform =
                    hit.collider.transform;

                if (hitTransform == transform ||
                    hitTransform.IsChildOf(transform))
                {
                    continue;
                }

                if (hit.collider.GetComponentInParent<
                        BDPlayerMarker>() != null)
                {
                    continue;
                }

                if (Vector3.Angle(
                        hit.normal,
                        Vector3.up) > 55f)
                {
                    continue;
                }

                // Return the actual ground surface. Root-height conversion
                // happens only after hazard and clearance validation.
                grounded = hit.point;
                return true;
            }

            grounded = requested;
            return false;
        }

        private Vector3 ResolveHorseRootPositionFromGroundAnchor(
            Vector3 groundPoint)
        {
            // BD DEATH RESTART HORSE ROOT GROUNDING V15
            // CharacterController.position is the object root. With a centered
            // two-unit capsule, placing that root at the raycast hit sinks half
            // the horse below the floor. Convert surface -> root generically.
            float rootLift = 0.08f;

            if (controller != null)
            {
                float capsuleBottomFromRoot =
                    controller.center.y -
                    controller.height * 0.5f;

                float groundClearance = Mathf.Clamp(
                    controller.skinWidth * 0.625f,
                    0.03f,
                    0.08f
                );

                rootLift =
                    -capsuleBottomFromRoot +
                    groundClearance;
            }

            Vector3 resolved = groundPoint;
            resolved.y += Mathf.Max(0.02f, rootLift);
            return resolved;
        }

        private bool HasHorseStartClearance(
            Vector3 groundPosition)
        {
            float radius =
                controller != null
                    ? Mathf.Max(
                        0.30f,
                        controller.radius * 0.92f
                    )
                    : 0.55f;

            float height =
                controller != null
                    ? Mathf.Max(
                        radius * 2f,
                        controller.height
                    )
                    : 2.0f;

            Vector3 bottom =
                groundPosition +
                Vector3.up *
                (radius + 0.12f);

            Vector3 top =
                groundPosition +
                Vector3.up *
                Mathf.Max(
                    radius + 0.12f,
                    height - radius
                );

            Collider[] overlaps = Physics.OverlapCapsule(
                bottom,
                top,
                radius,
                ~0,
                QueryTriggerInteraction.Ignore
            );

            for (int index = 0;
                 index < overlaps.Length;
                 index++)
            {
                Collider overlap = overlaps[index];

                if (overlap == null ||
                    overlap.isTrigger)
                {
                    continue;
                }

                Transform overlapTransform =
                    overlap.transform;

                if (overlapTransform == transform ||
                    overlapTransform.IsChildOf(transform))
                {
                    continue;
                }

                if (overlap.bounds.max.y <=
                    groundPosition.y + 0.20f)
                {
                    continue;
                }

                return false;
            }

            return true;
        }

        private bool HasLivingEnemyNearHorseOrPlayer(
            float radius)
        {
            return BDHorseLocalThreatUtility.HasLivingThreatNear(
                transform,
                rider,
                radius
            );
        }

        private void Update()
        {
            jumpedThisFrame = false;

            if (rider == null)
                rider = BDTargetFinder.FindPlayer();

            CachePlayerComponents();
            ResolveSafeSpotIfNeeded();

            // BD PRESENTATION HORSE INPUT HARD LOCK V20
            if (BDRunPresentationCoordinator.InputLocked)
            {
                lastRideInput = Vector2.zero;
                pointerDragging = false;
                smoothedMountedHorizontalVelocity = Vector3.zero;
                verticalVelocity = groundedStickVelocity;
                lastAction = "run presentation input lock";
                return;
            }

            if (hazardSafety != null &&
                hazardSafety.IsRecovering)
            {
                lastAction = "hazard recovery lock";
                return;
            }

            if (externalControlLock)
            {
                lastAction =
                    "external control - " +
                    externalControlReason;
                return;
            }

            playerInRange = rider != null && Vector3.Distance(transform.position, rider.position) <= interactionRange;

            if (ReadInteractPressed())
                HandleInteract();

            if (Time.unscaledTime <
                    startupCalmUntil &&
                state != HorseState.Mounted &&
                state != HorseState.Fainted)
            {
                state = HorseState.Idle;
                lastAction =
                    "startup calm - healthy idle";
                return;
            }

            if (state == HorseState.Mounted)
            {
                TickMounted();

                // BD HEALING ON FOOT ONLY V23R9
                // Mounted play never heals the horse, even while stationary.
                // Any on-foot healing session is closed immediately when
                // mounting so F cannot continue healing from the saddle.
                EndHealingSessionIfNeeded();

                if (healingReleaseHoldActive)
                    ClearHealingReleaseHold();
            }
            else if (state == HorseState.MovingToSafeSpot)
                TickMoveToSafeSpot();
            else if (state == HorseState.Fainted)
                TickFainted();
            else
                TickHealingIfNear();
            TickReturnToPlayerAfterDanger();
        }

        private void LateUpdate()
        {
            if (state == HorseState.Mounted)
                PlaceRiderOnMountPoint();
        }

        public void SendToSafeSpot()
        {
            if (IsStartupCalm)
            {
                if (state != HorseState.Mounted &&
                    state != HorseState.Fainted)
                {
                    state = HorseState.Idle;
                }

                lastAction =
                    "startup calm - ignored safe spot request";
                return;
            }

            if (externalControlLock ||
                health.IsFainted ||
                state == HorseState.Mounted)
            {
                return;
            }

            ResolveSafeSpotIfNeeded();

            if (safeSpot == null)
            {
                state = HorseState.Idle;
                lastAction = "no safe spot";
                return;
            }

            state = HorseState.MovingToSafeSpot;
            lastAction = "sent to safe spot";
        }
        public void ForceDismountForCombat()
        {
            if (externalControlLock)
                return;

            if (Time.unscaledTime < startupCalmUntil)
            {
                if (state != HorseState.Mounted &&
                    state != HorseState.Fainted)
                {
                    state = HorseState.Idle;
                }

                lastAction =
                    "startup calm - ignored combat";
                return;
            }

            if (!HasLivingEnemyNearHorseOrPlayer(
                    combatAwarenessRadius))
            {
                if (state != HorseState.Mounted &&
                    state != HorseState.Fainted)
                {
                    state = HorseState.Idle;
                }

                lastAction =
                    "ignored remote combat";
                return;
            }

            if (state == HorseState.Mounted)
                Dismount(sendToSafeSpotAfterDismount: true);
            else
                SendToSafeSpot();
        }

        public void ForceDismountAfterHazardRecovery()
        {
            if (state != HorseState.Mounted)
                return;

            CachePlayerComponents();

            if (rider != null)
            {
                rider.localScale = riderOriginalScale;
                rider.localRotation = riderOriginalLocalRotation;
            }

            if (playerCharacterController != null)
                playerCharacterController.enabled = true;

            if (playerController != null)
            {
                playerController.enabled = true;
                playerController.ResetMotionAfterExternalTeleport();
            }

            smoothedMountedHorizontalVelocity = Vector3.zero;
            lastRideInput = Vector2.zero;
            verticalVelocity = groundedStickVelocity;
            mountedYawInitialized = false;

            state =
                health != null && health.IsFainted
                    ? HorseState.Fainted
                    : HorseState.Idle;

            lastAction = "hazard recovery forced dismount";
        }


        public void SetSafeSpot(Transform newSafeSpot)
        {
            safeSpot = newSafeSpot;
        }

        private void HandleInteract()
        {
            if (!playerInRange || rider == null)
                return;

            if (state == HorseState.Mounted)
            {
                if (!controller.isGrounded)
                {
                    lastAction = "cannot dismount while horse jumping";
                    return;
                }

                Dismount(sendToSafeSpotAfterDismount: ShouldRunToSafeAfterManualDismount());
                return;
            }

            if (health.IsFainted)
            {
                lastAction = "horse fainted - hold F to revive";
                return;
            }

            // Pressing E mounts even if the horse is injured.
            // This makes mounting reliable under enemy pressure.
            Mount();
        }



        private void TickReturnToPlayerAfterDanger()
        {
            if (!returnToPlayerAfterDangerEnds)
                return;

            if (state == HorseState.Mounted || state == HorseState.Fainted)
                return;

            if (health != null && health.IsFainted)
                return;

            if (IsCombatActiveForHorse())
            {
                lastCombatActiveAt = Time.time;
                return;
            }

            if (Time.time - lastCombatActiveAt < dangerEndedDelay)
                return;

            Transform player = BDTargetFinder.FindPlayer();
            if (player == null)
                return;

            Vector3 toPlayer = player.position - transform.position;
            toPlayer.y = 0f;

            float distance = toPlayer.magnitude;
            if (distance <= 0.001f)
                return;

            float comfortRadius =
                Mathf.Max(
                    returnTooCloseRadius + 0.25f,
                    returnComfortRadius
                );

            float noTrackingRadius =
                Mathf.Max(
                    interactionRange,
                    nearbyIdleNoTrackingRadius,
                    comfortRadius
                );

            // When the player is already near the horse, the horse must remain
            // completely idle. It does not rotate to follow the player and it
            // does not back away merely because the player walked around it.
            if (distance <= noTrackingRadius)
            {
                if (state == HorseState.ReturningToPlayer)
                    state = HorseState.Idle;

                lastAction =
                    $"player nearby - idle without tracking {distance:0.0}";
                return;
            }

            Vector3 directionToPlayer = toPlayer.normalized;
            float resumeRadius = comfortRadius + Mathf.Max(0f, returnStopHysteresis);

            // Too close: do not stick to the player. Back away gently.
            if (distance < returnTooCloseRadius)
            {
                Vector3 awayFromPlayer = -directionToPlayer;

                if (controller != null)
                    MoveHorse(awayFromPlayer * returnBackAwaySpeed * Time.deltaTime);

                // Too-close correction faces away from the rider, never toward the rider.
                RotateToward(awayFromPlayer);
                state = HorseState.ReturningToPlayer;
                lastAction = "backing away from rider to comfortable distance";
                return;
            }

            // Close enough: stop nearby and wait.
            if (distance <= comfortRadius)
            {
                if (state == HorseState.ReturningToPlayer)
                {
                    state = HorseState.Idle;
                    lastAction = $"returned near player radius {distance:0.0}";
                }

                // Close enough: do not keep turning to stare at the player.
                // This prevents idle horse jitter/constant player-facing rotation.
                return;
            }

            // Start returning only when clearly far enough.
            if (state != HorseState.ReturningToPlayer && distance < returnToPlayerDistance)
                return;

            // When already returning, avoid jitter around the comfort boundary.
            if (state == HorseState.ReturningToPlayer && distance <= resumeRadius)
            {
                state = HorseState.Idle;
                lastAction = $"returned near player resume radius {distance:0.0}";
                return;
            }

            state = HorseState.ReturningToPlayer;

            if (controller != null)
                MoveHorse(directionToPlayer * returnMoveSpeed * Time.deltaTime);
            // When the horse is actively coming to the player from far away, it should face the player.
            // When the player is the one approaching the horse, the earlier comfort/idle branches return before this point,
            // so the horse keeps its current pose.
            if (rotateTowardPlayerWhenReturning)
                RotateToward(directionToPlayer);

            lastAction = rotateTowardPlayerWhenReturning
                ? $"horse coming to rider facing rider target radius {comfortRadius:0.0}"
                : $"horse coming to rider without yaw target radius {comfortRadius:0.0}";
        }

        private bool ShouldRunToSafeAfterManualDismount()
        {
            if (runToSafeSpotAfterDismount)
                return true;

            if (!runToSafeSpotAfterCombatDismount)
                return false;

            return IsCombatActiveForHorse();
        }
        private bool IsCombatActiveForHorse()
        {
            if (Time.unscaledTime < startupCalmUntil)
                return false;

            return HasLivingEnemyNearHorseOrPlayer(
                combatAwarenessRadius
            );
        }
        // BD CINEMATIC MOUNT API V7
        public bool ForceMountForCinematic(Transform expectedRider)
        {
            if (expectedRider != null)
                rider = expectedRider;

            CachePlayerComponents();
            Mount();
            return IsMounted;
        }

        public bool MaintainMountedRunIntroBinding(
            Transform expectedRider)
        {
            // BD AUTHORITATIVE PER-FRAME INTRO RIDER BINDING V23R19G
            // The cinematic can disable normal Update. Reassert the exact
            // rider, mounted state, hurtbox and mount-point pose explicitly.
            if (expectedRider != null)
                rider = expectedRider;

            if (rider == null)
                return false;

            CachePlayerComponents();
            state = HorseState.Mounted;
            stateBeforeExternalControl = HorseState.Mounted;

            if (playerController != null)
                playerController.enabled = false;

            if (playerCharacterController != null)
                playerCharacterController.enabled = true;

            PlaceRiderOnMountPoint();
            BDRunPresentationCoordinator.EnsureMountedIntroRiderVisible(
                rider
            );
            return true;
        }

        public void SnapCinematicRiderToMountPoint()
        {
            if (externalControlReason == "mounted run intro")
            {
                MaintainMountedRunIntroBinding(rider);
                return;
            }

            if (rider != null && IsMounted)
                PlaceRiderOnMountPoint();
        }

        // BD POST-CINEMATIC MOUNTED INPUT RESET V20
        public void PrepareMountedGameplayAfterCinematic(
            Vector3 finalFacingDirection)
        {
            finalFacingDirection.y = 0f;
            if (finalFacingDirection.sqrMagnitude < 0.001f)
                finalFacingDirection = transform.forward;
            finalFacingDirection.Normalize();

            transform.rotation = Quaternion.LookRotation(
                finalFacingDirection,
                Vector3.up
            );
            lastRideInput = Vector2.zero;
            pointerDragging = false;
            smoothedMountedHorizontalVelocity = Vector3.zero;
            smoothedMountedTravelDirection = finalFacingDirection;
            lastMountedAimDirection = finalFacingDirection;
            targetMountedAimDirection = finalFacingDirection;
            mountedYawInitialized = false;
            InitializeMountedYawFromTransform();
            verticalVelocity = groundedStickVelocity;
            SnapCinematicRiderToMountPoint();
        }


        private void Mount()
        {
            if (externalControlLock ||
                health.IsFainted ||
                rider == null)
            {
                return;
            }

            ClearHealingReleaseHold();

            state = HorseState.Mounted;
            smoothedMountedHorizontalVelocity = Vector3.zero;
            smoothedMountedTravelDirection = transform.forward;
            smoothedMountedTravelDirection.y = 0f;
            mountedYawInitialized = false;
            InitializeMountedYawFromTransform();
            lastAction = "mounted";

            verticalVelocity = groundedStickVelocity;

            riderOriginalScale = rider.localScale;
            riderOriginalLocalRotation = rider.localRotation;

            if (playerController != null)
                playerController.enabled = false;

            if (playerCharacterController != null)
                playerCharacterController.enabled = true; // BD MOUNTED RIDER HURTBOX ENABLED V10

            // Important:
            // Do NOT parent the player to the horse.
            // The horse visual may be non-uniformly scaled, and parenting would stretch the player.
            PlaceRiderOnMountPoint();
        }

        private void Dismount(bool sendToSafeSpotAfterDismount)
        {
            if (rider == null)
                return;

            CachePlayerComponents();

            DismountPlan plan = BuildDismountPlan();
            Vector3 leapStart = GetMountPointWorldPosition();

            rider.position = leapStart;
            rider.localScale = riderOriginalScale;

            if (playerCharacterController != null)
                playerCharacterController.enabled = true;

            if (playerDismountLeap != null)
            {
                playerDismountLeap.BeginLeap(
                    plan.Destination,
                    plan.Duration,
                    plan.ArcHeight,
                    plan.Direction
                );
            }
            else
            {
                rider.position = plan.Destination;

                if (playerController != null)
                    playerController.enabled = true;
            }

            if (health.IsFainted)
            {
                state = HorseState.Fainted;
                lastAction = "dismounted -> fainted";
                return;
            }

            if (sendToSafeSpotAfterDismount)
            {
                ResolveSafeSpotIfNeeded();

                if (safeSpot != null)
                {
                    state = HorseState.MovingToSafeSpot;
                    lastAction = plan.EnemyNear
                        ? "dismounted -> enemy-size input leap -> safe spot"
                        : "dismounted -> small input leap -> safe spot";
                    return;
                }

                lastAction = "dismounted -> no safe spot";
            }
            else
            {
                lastAction = plan.EnemyNear
                    ? "dismounted -> enemy-size input leap"
                    : "dismounted -> small input leap";
            }

            state = HorseState.Idle;
        }

        private DismountPlan BuildDismountPlan()
        {
            Vector2 input = ReadRideMoveInput();
            bool hadInput = input.magnitude >= inputDeadZone;

            Vector3 direction;

            if (hadInput)
            {
                Vector3 aimDirection = ResolveMountedAimDirection();
                direction = ToMountedWorldMove(input, aimDirection);

                if (direction.sqrMagnitude > 1f)
                    direction.Normalize();
            }
            else
            {
                direction = transform.TransformDirection(fallbackDismountOffset);
                direction.y = 0f;

                if (direction.sqrMagnitude < 0.001f)
                    direction = transform.right;

                direction.Normalize();
            }

            bool enemyNear = HasLivingEnemyNear(enemyAwareDismountRadius);
            float distance = enemyNear ? enemyDismountDistance : normalDismountDistance;
            float duration = enemyNear ? enemyDismountDuration : normalDismountDuration;
            float arc = enemyNear ? enemyDismountArcHeight : normalDismountArcHeight;

            Vector3 destination = transform.position + direction.normalized * distance;
            destination.y = transform.position.y;

            lastDismountMode = enemyNear
                ? (hadInput ? "enemy-near, input direction" : "enemy-near, fallback direction")
                : (hadInput ? "normal, input direction" : "normal, fallback direction");

            return new DismountPlan(destination, direction, duration, arc, enemyNear, hadInput);
        }

        private bool HasLivingEnemyNear(float radius)
        {
            BDHealth[] healths = FindObjectsByType<BDHealth>(FindObjectsSortMode.None);

            foreach (BDHealth candidate in healths)
            {
                if (candidate == null || candidate.IsDead)
                    continue;

                if (candidate.GetComponent<BDPlayerMarker>() != null)
                    continue;

                if (candidate.GetComponent<BDHorseHealth>() != null)
                    continue;

                float distance = Vector3.Distance(transform.position, candidate.transform.position);
                if (distance <= radius)
                    return true;
            }

            return false;
        }



        private void InitializeMountedYawFromTransform()
        {
            Vector3 forward = transform.forward;
            forward.y = 0f;
            float yaw = BDMouseAimUtility.YawFromDirection(forward.sqrMagnitude > 0.001f ? forward : Vector3.forward, transform.eulerAngles.y);
            mountedTargetYaw = yaw;
            mountedCurrentYaw = yaw;
            lastMountedAimDirection = BDMouseAimUtility.DirectionFromYaw(mountedCurrentYaw);
            mountedYawInitialized = true;
        }

        private Vector3 ResolveMountedAimDirection()
        {
            if (enableMountedMouseLook && BDMouseAimUtility.IsMouseInsideScreenCenterDeadZone(transform, mountedMouseScreenCenterDeadZonePixels))
                return targetMountedAimDirection.sqrMagnitude > 0.001f ? targetMountedAimDirection.normalized : lastMountedAimDirection.normalized;

            if (enableMountedMouseLook && BDMouseAimUtility.TryGetMouseAimDirection(transform, mountedMouseAimMinDistance, out Vector3 mouseDirection))
                return ApplyMountedFrontMouseDeadZone(mouseDirection);

            Vector3 fallback = targetMountedAimDirection.sqrMagnitude > 0.001f ? targetMountedAimDirection : lastMountedAimDirection.sqrMagnitude > 0.001f ? lastMountedAimDirection : transform.forward;
            fallback.y = 0f;

            if (fallback.sqrMagnitude < 0.001f)
                fallback = Vector3.forward;

            return fallback.normalized;
        }

        private Vector3 ApplyMountedFrontMouseDeadZone(Vector3 rawAimDirection)
        {
            rawAimDirection.y = 0f;

            if (rawAimDirection.sqrMagnitude < 0.001f)
                return lastMountedAimDirection.sqrMagnitude > 0.001f ? lastMountedAimDirection.normalized : transform.forward;

            rawAimDirection.Normalize();

            Vector3 frontBasis = ResolveMountedModelFrontConeBasis();
            Vector3 clampedAim = ClampMountedDirectionToFrontCone(rawAimDirection, frontBasis, mountedMouseModelFrontConeDegrees);

            Vector3 currentForward = targetMountedAimDirection.sqrMagnitude > 0.001f ? targetMountedAimDirection : lastMountedAimDirection.sqrMagnitude > 0.001f ? lastMountedAimDirection : frontBasis;
            currentForward.y = 0f;

            if (currentForward.sqrMagnitude < 0.001f)
                return clampedAim;

            currentForward.Normalize();

            float angle = Vector3.Angle(currentForward, clampedAim);

            if (angle <= mountedMouseFrontDeadZoneDegrees)
                return currentForward;

            return clampedAim;
        }

        private Vector3 ResolveMountedModelFrontConeBasis()
        {
            Vector3 basis = ResolveMountedMovementReferenceForward();
            basis.y = 0f;

            if (basis.sqrMagnitude < 0.001f)
            {
                basis = transform.forward;
                basis.y = 0f;
            }

            if (basis.sqrMagnitude < 0.001f)
                basis = Vector3.forward;

            return basis.normalized;
        }

        private Vector3 ClampMountedDirectionToFrontCone(Vector3 direction, Vector3 frontBasis, float coneDegrees)
        {
            direction.y = 0f;
            frontBasis.y = 0f;

            if (direction.sqrMagnitude < 0.001f)
                return frontBasis.sqrMagnitude > 0.001f ? frontBasis.normalized : Vector3.forward;

            if (frontBasis.sqrMagnitude < 0.001f)
                frontBasis = Vector3.forward;

            direction.Normalize();
            frontBasis.Normalize();

            float halfCone = Mathf.Clamp(coneDegrees * 0.5f, 1f, 179f);
            float signedAngle = Vector3.SignedAngle(frontBasis, direction, Vector3.up);
            float clampedAngle = Mathf.Clamp(signedAngle, -halfCone, halfCone);

            Vector3 clamped = Quaternion.AngleAxis(clampedAngle, Vector3.up) * frontBasis;
            clamped.y = 0f;

            if (clamped.sqrMagnitude < 0.001f)
                return frontBasis;

            return clamped.normalized;
        }


        private Vector3 SmoothMountedAimDirection(Vector3 rawDirection)
        {
            rawDirection.y = 0f;

            if (rawDirection.sqrMagnitude < 0.001f)
                return lastMountedAimDirection.sqrMagnitude > 0.001f ? lastMountedAimDirection.normalized : transform.forward;

            rawDirection.Normalize();

            Vector3 current = lastMountedAimDirection;
            current.y = 0f;

            if (current.sqrMagnitude < 0.001f)
                return rawDirection;

            current.Normalize();

            float t = 1f - Mathf.Exp(-mountedMousePointAimSmoothing * Time.deltaTime);
            Vector3 smoothed = Vector3.Slerp(current, rawDirection, t);
            smoothed.y = 0f;

            if (smoothed.sqrMagnitude < 0.001f)
                return rawDirection;

            return smoothed.normalized;
        }



        private Vector3 SmoothMountedAimTargetDirection(Vector3 rawTargetDirection)
        {
            rawTargetDirection.y = 0f;

            if (rawTargetDirection.sqrMagnitude < 0.001f)
                return targetMountedAimDirection.sqrMagnitude > 0.001f ? targetMountedAimDirection.normalized : lastMountedAimDirection;

            rawTargetDirection.Normalize();

            if (smoothedMountedTargetAimDirection.sqrMagnitude < 0.001f)
                smoothedMountedTargetAimDirection = rawTargetDirection;

            smoothedMountedTargetAimDirection.y = 0f;
            smoothedMountedTargetAimDirection.Normalize();

            float t = 1f - Mathf.Exp(-mountedMouseAimTargetSmoothing * Time.deltaTime);
            smoothedMountedTargetAimDirection = Vector3.Slerp(smoothedMountedTargetAimDirection, rawTargetDirection, t);
            smoothedMountedTargetAimDirection.y = 0f;

            if (smoothedMountedTargetAimDirection.sqrMagnitude < 0.001f)
                smoothedMountedTargetAimDirection = rawTargetDirection;

            return smoothedMountedTargetAimDirection.normalized;
        }

        private Vector3 TurnMountedAimGradually(Vector3 currentDirection, Vector3 targetDirection, float degreesPerSecond)
        {
            currentDirection.y = 0f;
            targetDirection.y = 0f;

            if (targetDirection.sqrMagnitude < 0.001f)
                return currentDirection.sqrMagnitude > 0.001f ? currentDirection.normalized : transform.forward;

            targetDirection.Normalize();

            if (currentDirection.sqrMagnitude < 0.001f)
                return targetDirection;

            currentDirection.Normalize();

            float maxRadiansDelta = Mathf.Deg2Rad * Mathf.Max(1f, degreesPerSecond) * Time.deltaTime;
            Vector3 turned = Vector3.RotateTowards(currentDirection, targetDirection, maxRadiansDelta, 0f);
            turned.y = 0f;

            if (turned.sqrMagnitude < 0.001f)
                return targetDirection;

            return turned.normalized;
        }


        private Vector3 ToMountedPlayerRelativeMove(Vector2 input)
        {
            Vector3 cameraForward = ResolveMountedMovementReferenceForward();
            Vector3 mouseForward = ResolveMountedMovementInputForward();
            Vector3 cameraRight = new Vector3(cameraForward.z, 0f, -cameraForward.x);

            Vector3 move = Vector3.zero;

            // Only forward mounted movement follows the mouse/model direction.
            // Backward and side movement stay camera/screen relative.
            if (input.y > 0.001f)
                move += mouseForward * input.y;
            else if (input.y < -0.001f)
                move += cameraForward * input.y;

            move += cameraRight * input.x;
            move.y = 0f;

            if (move.sqrMagnitude > 1f)
                move.Normalize();

            return move;
        }

        private Vector3 ResolveMountedMovementInputForward()
        {
            Vector3 basis = ResolveMountedMovementReferenceForward();
            Vector3 forward = Vector3.zero;

            // Movement follows the same rate-limited direction that the horse
            // and camera already show. Using the raw target made the horse slide
            // toward the cursor before its body had turned there.
            if (lastMountedAimDirection.sqrMagnitude > 0.001f)
                forward = lastMountedAimDirection;
            else if (targetMountedAimDirection.sqrMagnitude > 0.001f)
                forward = targetMountedAimDirection;

            forward.y = 0f;

            if (forward.sqrMagnitude < 0.001f)
                forward = basis;

            // Mounted movement follows mounted mouse/model direction, but only inside the 60-degree front cone.
            forward = ClampMountedDirectionToFrontCone(forward, basis, mountedMouseModelFrontConeDegrees);

            if (forward.sqrMagnitude < 0.001f)
                return basis.sqrMagnitude > 0.001f ? basis.normalized : Vector3.forward;

            return forward.normalized;
        }

        private Vector3 ResolveMountedMovementReferenceForward()
        {
            Camera camera = Camera.main;
            Vector3 forward = Vector3.zero;

            if (camera != null)
            {
                forward = camera.transform.forward;
                forward.y = 0f;
            }

            if (forward.sqrMagnitude < 0.001f)
            {
                forward = transform.forward;
                forward.y = 0f;
            }

            if (forward.sqrMagnitude < 0.001f)
                forward = mountedMovementReferenceForward.sqrMagnitude > 0.001f ? mountedMovementReferenceForward : Vector3.forward;

            forward.Normalize();

            mountedMovementReferenceForward = Vector3.Slerp(
                mountedMovementReferenceForward.sqrMagnitude > 0.001f ? mountedMovementReferenceForward.normalized : forward,
                forward,
                1f - Mathf.Exp(-16f * Time.deltaTime)
            );

            mountedMovementReferenceForward.y = 0f;

            if (mountedMovementReferenceForward.sqrMagnitude < 0.001f)
                mountedMovementReferenceForward = forward;

            return mountedMovementReferenceForward.normalized;
        }

        private Vector3 ToMountedWorldMove(Vector2 input, Vector3 aimDirection)
        {
            if (input.sqrMagnitude < 0.0001f)
                return Vector3.zero;

            if (mountedMovementRelativeToMouseLook && aimDirection.sqrMagnitude > 0.001f)
            {
                Vector3 forward = aimDirection;
                forward.y = 0f;
                forward = forward.sqrMagnitude < 0.001f ? Vector3.forward : forward.normalized;

                Vector3 right = Vector3.Cross(Vector3.up, forward).normalized;
                Vector3 world = forward * input.y + right * input.x;

                return world.sqrMagnitude > 1f ? world.normalized : world;
            }

            Vector3 fallback = new Vector3(input.x, 0f, input.y);
            return fallback.sqrMagnitude > 1f ? fallback.normalized : fallback;
        }
        private Vector3 SmoothMountedTravelDirection(
            Vector3 desiredDirection,
            bool wantsMove)
        {
            desiredDirection.y = 0f;

            if (!wantsMove ||
                desiredDirection.sqrMagnitude < 0.001f)
            {
                return Vector3.zero;
            }

            desiredDirection.Normalize();

            if (smoothedMountedTravelDirection.sqrMagnitude < 0.001f)
            {
                smoothedMountedTravelDirection =
                    transform.forward;

                smoothedMountedTravelDirection.y = 0f;
            }

            if (smoothedMountedTravelDirection.sqrMagnitude < 0.001f)
            {
                smoothedMountedTravelDirection =
                    desiredDirection;
            }

            float maxRadians =
                Mathf.Deg2Rad *
                Mathf.Max(
                    35f,
                    mountedTravelTurnDegreesPerSecond
                ) *
                Time.deltaTime;

            smoothedMountedTravelDirection =
                Vector3.RotateTowards(
                    smoothedMountedTravelDirection.normalized,
                    desiredDirection,
                    maxRadians,
                    0f
                );

            smoothedMountedTravelDirection.y = 0f;

            return smoothedMountedTravelDirection.sqrMagnitude > 0.001f
                ? smoothedMountedTravelDirection.normalized
                : desiredDirection;
        }


        private void TickMounted()
        {
            if (health.IsFainted)
            {
                Dismount(sendToSafeSpotAfterDismount: false);
                state = HorseState.Fainted;
                return;
            }

            Vector2 input = ReadRideMoveInput();
            lastRideInput = input;

            bool wantsRideMove = input.sqrMagnitude > 0.0001f;

            Vector3 mountedAimTarget = ResolveMountedAimDirection();
            if (mountedAimTarget.sqrMagnitude > 0.001f)
                targetMountedAimDirection = SmoothMountedAimTargetDirection(mountedAimTarget.normalized);

            float turnSpeed = wantsRideMove ? mountedMouseAimMovingTurnDegreesPerSecond : mountedMouseAimIdleTurnDegreesPerSecond;
            lastMountedAimDirection = TurnMountedAimGradually(lastMountedAimDirection, targetMountedAimDirection, turnSpeed);

            Vector3 desiredMove =
                wantsRideMove
                    ? ToMountedPlayerRelativeMove(input)
                    : Vector3.zero;

            Vector3 move =
                SmoothMountedTravelDirection(
                    desiredMove,
                    wantsRideMove
                );

            if (controller.isGrounded && verticalVelocity < 0f)
                verticalVelocity = groundedStickVelocity;
            if ((!allowHorseJumpOnlyMounted ||
                 state == HorseState.Mounted) &&
                controller.isGrounded &&
                ReadHorseJumpPressed())
            {
                if (CanStartHorseJump(move))
                {
                    verticalVelocity = Mathf.Sqrt(
                        horseJumpHeight *
                        -2f *
                        horseGravity
                    );

                    jumpedThisFrame = true;
                    lastAction = "horse jump";
                }
                else
                {
                    verticalVelocity =
                        groundedStickVelocity;

                    jumpedThisFrame = false;
                    lastAction =
                        "horse jump blocked by hole/lava";
                }
            }

            verticalVelocity += horseGravity * Time.deltaTime;

            float injuryMultiplier = Mathf.Lerp(1f, 0.45f, health.Injury01);
            Vector3 horizontal = SmoothMountedHorizontalVelocity(move, wantsRideMove, injuryMultiplier);
            Vector3 velocity = horizontal + Vector3.up * verticalVelocity;

            MoveHorse(velocity * Time.deltaTime);

            Vector3 mountedFacingDirection =
                wantsRideMove &&
                move.sqrMagnitude > 0.001f
                    ? move
                    : lastMountedAimDirection;

            if (mountedFacingDirection.sqrMagnitude > 0.001f)
            {
                RotateToward(mountedFacingDirection);

                lastRideInputSource = wantsRideMove
                    ? "natural-wide-horse-turn + " + lastRideInputSource
                    : "idle-turning-to-mounted-target-aim";
            }

            PlaceRiderOnMountPoint();
        }


        private Vector3 SmoothMountedHorizontalVelocity(Vector3 desiredMoveDirection, bool wantsMove, float speedMultiplier)
        {
            desiredMoveDirection.y = 0f;

            Vector3 desiredVelocity = Vector3.zero;

            if (wantsMove && desiredMoveDirection.sqrMagnitude > 0.001f)
                desiredVelocity = desiredMoveDirection.normalized * mountedMoveSpeed * speedMultiplier;

            float rate = desiredVelocity.sqrMagnitude > smoothedMountedHorizontalVelocity.sqrMagnitude
                ? mountedMoveAcceleration
                : mountedMoveDeceleration;

            smoothedMountedHorizontalVelocity = Vector3.MoveTowards(
                smoothedMountedHorizontalVelocity,
                desiredVelocity,
                Mathf.Max(0.01f, rate) * Time.deltaTime
            );

            if (!wantsMove && smoothedMountedHorizontalVelocity.sqrMagnitude < 0.0004f)
                smoothedMountedHorizontalVelocity = Vector3.zero;

            return smoothedMountedHorizontalVelocity;
        }

        private void TickMoveToSafeSpot()
        {
            ResolveSafeSpotIfNeeded();

            if (safeSpot == null)
            {
                state = HorseState.Idle;
                lastAction = "safe spot missing";
                return;
            }

            if (controller.isGrounded && verticalVelocity < 0f)
                verticalVelocity = groundedStickVelocity;

            verticalVelocity += horseGravity * Time.deltaTime;

            Vector3 toSpot = safeSpot.position - transform.position;
            toSpot.y = 0f;
            float distance = toSpot.magnitude;

            if (distance <= safeSpotReachDistance && controller.isGrounded)
            {
                state = HorseState.WaitingSafe;
                lastAction = "waiting safe";
                return;
            }

            Vector3 direction = toSpot.sqrMagnitude > 0.001f ? toSpot.normalized : Vector3.zero;
            float injuryMultiplier = Mathf.Lerp(1f, 0.45f, health.Injury01);

            Vector3 velocity = direction * baseMoveSpeed * injuryMultiplier + Vector3.up * verticalVelocity;
            MoveHorse(velocity * Time.deltaTime);

            if (direction.sqrMagnitude > 0.001f)
                RotateToward(direction);
        }

        private void TickFainted()
        {
            if (playerInRange)
                TickHealingIfNear();
            TickReturnToPlayerAfterDanger();
        }

        private void TickHealingIfNear()
        {
            if (healingReleaseHoldActive)
            {
                if (playerInRange && !ReadHealHeld())
                {
                    // After releasing F, keep the HP exactly where the heal reached
                    // while the player is still next to the horse.
                    health.BeginHealingProtection(releasedHealingProtectionRefresh);
                    health.LockHealingFloor(healingHeldFloor);
                    lastAction = $"healing released - HP locked at {healingHeldFloor:0.0}";
                    return;
                }

                if (!playerInRange)
                    ClearHealingReleaseHold();
            }

            bool canHeal =
                playerInRange &&
                ReadHealHeld() &&
                health.CurrentHealth < health.MaxHealth;

            if (!canHeal)
            {
                EndHealingSessionIfNeeded();
                return;
            }

            StartHealingSessionIfNeeded();

            float healAmount = CalculateCurvedHealPerSecond() * Time.deltaTime;

            // The healing system only heals. It never applies damage or drain.
            health.ClearHealingFloorLock();
            health.BeginHealingProtection(healingProtectionRefresh);
            health.Heal(healAmount);

            healingHeldFloor = health.CurrentHealth;
            lastAction = $"healing curved +{healAmount:0.00}";
        }

        private void StartHealingSessionIfNeeded()
        {
            if (healingSessionActive)
                return;

            healingSessionActive = true;
            healingReleaseHoldActive = false;
            healingSessionStartedAt = Time.time;
            healingStartHealth = health.CurrentHealth;
            healingHeldFloor = health.CurrentHealth;
            health.ClearHealingFloorLock();
            lastAction = "healing started";
        }

        private void EndHealingSessionIfNeeded()
        {
            if (!healingSessionActive)
                return;

            healingSessionActive = false;
            healingReleaseHoldActive = true;
            healingHeldFloor = health.CurrentHealth;

            // Releasing F must not reduce HP. Lock the floor at the reached value.
            health.LockHealingFloor(healingHeldFloor);
            health.BeginHealingProtection(releasedHealingProtectionRefresh);
            lastAction = $"healing ended - HP stays at {healingHeldFloor:0.0}";
        }

        private void ClearHealingReleaseHold()
        {
            healingReleaseHoldActive = false;
            health.ClearHealingFloorLock();
            lastAction = "healing release lock cleared";
        }

        private float CalculateCurvedHealPerSecond()
        {
            float maxHealth = Mathf.Max(1f, health.MaxHealth);
            float current = Mathf.Clamp(health.CurrentHealth, 0f, maxHealth);

            float hpRatio = Mathf.Clamp01(current / maxHealth);
            float sessionTime = Mathf.Max(0f, Time.time - healingSessionStartedAt);
            float ramp01 = Mathf.Clamp01(sessionTime / Mathf.Max(0.05f, healRampSeconds));

            float acceleration = Smooth01(ramp01);
            float speedMultiplier = Mathf.Lerp(minHealSpeedMultiplier, maxHealSpeedMultiplier, acceleration);

            if (hpRatio >= finalEighthSlowdownStart)
            {
                float final01 = Mathf.InverseLerp(finalEighthSlowdownStart, 1f, hpRatio);
                float slow01 = Smooth01(final01);
                speedMultiplier = Mathf.Lerp(speedMultiplier, finalHealSpeedMultiplier, slow01);
            }

            return Mathf.Max(0f, healPerSecond * speedMultiplier);
        }

        private static float Smooth01(float t)
        {
            t = Mathf.Clamp01(t);
            return t * t * (3f - 2f * t);
        }

        private void HealOnce()
        {
            health.ClearHealingFloorLock();
            health.BeginHealingProtection(releasedHealingProtectionRefresh);
            health.Heal(Mathf.Max(0f, healPerSecond * 0.35f));
            healingHeldFloor = health.CurrentHealth;
            health.LockHealingFloor(healingHeldFloor);
            lastAction = "heal tap protected - HP locked";
        }

        private void CachePlayerComponents()
        {
            if (rider == null)
                return;

            if (playerController == null)
                playerController = rider.GetComponent<BDPlayerController>();

            if (playerCharacterController == null)
                playerCharacterController = rider.GetComponent<CharacterController>();

            if (playerDismountLeap == null)
                playerDismountLeap = rider.GetComponent<BDPlayerDismountLeap>();
        }

        private void ResolveSafeSpotIfNeeded()
        {
            if (safeSpot != null)
                return;

            BDHorseSafeSpot spot = FindFirstObjectByType<BDHorseSafeSpot>();
            if (spot != null)
                safeSpot = spot.transform;
        }

        private Vector3 GetMountPointWorldPosition()
        {
            return transform.TransformPoint(mountedLocalOffset);
        }

        private void PlaceRiderOnMountPoint()
        {
            if (rider == null)
                return;

            rider.position = GetMountPointWorldPosition();
            rider.rotation = transform.rotation;
            rider.localScale = riderOriginalScale;
        }

        private Vector2 ReadRideMoveInput()
        {
            Vector2 keyboard = ReadKeyboardMove();
            Vector2 pointer = ReadPointerMove();

            if (pointer.sqrMagnitude > keyboard.sqrMagnitude)
            {
                lastRideInputSource = "pointer/touch";
                return Clamp(pointer);
            }

            lastRideInputSource = keyboard.sqrMagnitude > 0.0001f ? "keyboard/gamepad" : "none";
            return Clamp(keyboard);
        }

        private static Vector2 Clamp(Vector2 value)
        {
            return value.sqrMagnitude > 1f ? value.normalized : value;
        }

        private Vector2 ReadKeyboardMove()
        {
            Vector2 move = Vector2.zero;

#if ENABLE_INPUT_SYSTEM
            Keyboard keyboard = Keyboard.current;
            if (keyboard != null)
            {
                if (keyboard.aKey.isPressed || keyboard.leftArrowKey.isPressed) move.x -= 1f;
                if (keyboard.dKey.isPressed || keyboard.rightArrowKey.isPressed) move.x += 1f;
                if (keyboard.sKey.isPressed || keyboard.downArrowKey.isPressed) move.y -= 1f;
                if (keyboard.wKey.isPressed || keyboard.upArrowKey.isPressed) move.y += 1f;
            }

            Gamepad gamepad = Gamepad.current;
            if (gamepad != null)
            {
                Vector2 stick = gamepad.leftStick.ReadValue();
                if (stick.sqrMagnitude > move.sqrMagnitude)
                    move = stick;
            }
#endif

#if ENABLE_LEGACY_INPUT_MANAGER
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) move.x -= 1f;
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) move.x += 1f;
            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) move.y -= 1f;
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) move.y += 1f;

            Vector2 axes = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            if (axes.sqrMagnitude > move.sqrMagnitude)
                move = axes;
#endif

            return move;
        }

        private Vector2 ReadPointerMove()
        {
            Vector2 move = Vector2.zero;

#if ENABLE_INPUT_SYSTEM
            Touchscreen touchscreen = Touchscreen.current;
            if (touchscreen != null && touchscreen.primaryTouch.press.isPressed)
            {
                Vector2 start = touchscreen.primaryTouch.startPosition.ReadValue();
                Vector2 current = touchscreen.primaryTouch.position.ReadValue();
                Vector2 delta = current - start;

                if (delta.magnitude > dragDeadZonePixels)
                    move = delta / Mathf.Max(1f, dragMaxPixels);
            }

            Mouse mouse = Mouse.current;
            if (enableMouseDragAsFingerTrace && mouse != null)
            {
                if (mouse.leftButton.wasPressedThisFrame)
                {
                    pointerDragging = true;
                    pointerDragStart = mouse.position.ReadValue();
                }

                if (mouse.leftButton.isPressed && pointerDragging)
                {
                    Vector2 current = mouse.position.ReadValue();
                    Vector2 delta = current - pointerDragStart;

                    if (delta.magnitude > dragDeadZonePixels)
                    {
                        Vector2 mouseMove = delta / Mathf.Max(1f, dragMaxPixels);
                        if (mouseMove.sqrMagnitude > move.sqrMagnitude)
                            move = mouseMove;
                    }
                }

                if (mouse.leftButton.wasReleasedThisFrame)
                    pointerDragging = false;
            }
#endif

#if ENABLE_LEGACY_INPUT_MANAGER
            if (enableMouseDragAsFingerTrace)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    pointerDragging = true;
                    pointerDragStart = Input.mousePosition;
                }

                if (Input.GetMouseButton(0) && pointerDragging)
                {
                    Vector2 current = Input.mousePosition;
                    Vector2 delta = current - pointerDragStart;

                    if (delta.magnitude > dragDeadZonePixels)
                    {
                        Vector2 mouseMove = delta / Mathf.Max(1f, dragMaxPixels);
                        if (mouseMove.sqrMagnitude > move.sqrMagnitude)
                            move = mouseMove;
                    }
                }

                if (Input.GetMouseButtonUp(0))
                    pointerDragging = false;
            }
#endif

            return move;
        }

        private bool CanStartHorseJump(
            Vector3 desiredMoveDirection)
        {
            if (hazardSafety == null)
                return true;

            Vector3 horizontalVelocity = Vector3.zero;

            if (desiredMoveDirection.sqrMagnitude > 0.001f)
            {
                float injuryMultiplier = Mathf.Lerp(
                    1f,
                    0.45f,
                    health != null
                        ? health.Injury01
                        : 0f
                );

                horizontalVelocity =
                    desiredMoveDirection.normalized *
                    mountedMoveSpeed *
                    injuryMultiplier;
            }

            return hazardSafety.CanStartJump(
                horizontalVelocity,
                horseJumpHeight,
                horseGravity
            );
        }

        private bool ReadHorseJumpPressed()
        {
#if ENABLE_INPUT_SYSTEM
            if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
                return true;

            if (Gamepad.current != null && Gamepad.current.buttonSouth.wasPressedThisFrame)
                return true;
#endif

#if ENABLE_LEGACY_INPUT_MANAGER
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.JoystickButton0))
                return true;
#endif

            return false;
        }
        private void MoveHorse(Vector3 motion)
        {
            if (controller == null)
                return;

            Vector3 filtered =
                hazardSafety != null
                    ? hazardSafety.FilterMovement(motion)
                    : motion;

            filtered = BDQuicksandStatus.FilterMotion(
                gameObject,
                filtered
            );

            controller.Move(filtered);
        }


        private void RotateToward(Vector3 direction)
        {
            direction.y = 0f;

            if (direction.sqrMagnitude < 0.001f)
                return;

            Quaternion target = Quaternion.LookRotation(direction.normalized, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, target, 1f - Mathf.Exp(-rotationSpeed * Time.deltaTime));
        }

        private void OnHorseDamageBurstTriggered(BDHorseHealth horseHealth)
        {
            if (state != HorseState.Mounted || rider == null)
                return;

            ThrowRiderFromHorse();
        }

        private void ThrowRiderFromHorse()
        {
            CachePlayerComponents();

            Vector3 direction = Vector3.zero;

            if (lastRideInput.magnitude >= inputDeadZone)
                direction = new Vector3(lastRideInput.x, 0f, lastRideInput.y);

            if (direction.sqrMagnitude < 0.001f)
                direction = -transform.forward;

            direction.y = 0f;
            direction = direction.sqrMagnitude > 0.001f ? direction.normalized : Vector3.back;

            Vector3 leapStart = GetMountPointWorldPosition();
            Vector3 destination = transform.position + direction * buckThrowDistance;
            destination.y = transform.position.y;

            rider.position = leapStart;
            rider.localScale = riderOriginalScale;

            if (playerCharacterController != null)
                playerCharacterController.enabled = true;

            if (playerDismountLeap != null)
            {
                playerDismountLeap.BeginLeap(
                    destination,
                    buckThrowDuration,
                    buckThrowArcHeight,
                    direction
                );
            }
            else
            {
                rider.position = destination;

                if (playerController != null)
                    playerController.enabled = true;
            }

            lastDismountMode = "horse buck / damage burst";
            lastAction = "horse bucked rider";

            ResolveSafeSpotIfNeeded();

            if (safeSpot != null && !health.IsFainted)
                state = HorseState.MovingToSafeSpot;
            else
                state = health.IsFainted ? HorseState.Fainted : HorseState.Idle;
        }

        private void OnFainted(BDHorseHealth horseHealth)
        {
            if (state == HorseState.Mounted)
                Dismount(sendToSafeSpotAfterDismount: false);

            state = HorseState.Fainted;
            lastAction = "fainted";
        }

        private void OnRecovered(BDHorseHealth horseHealth)
        {
            if (state == HorseState.Fainted)
                state = HorseState.Idle;

            lastAction = "recovered";
        }


        private bool ReadHealHeld()
        {
#if ENABLE_INPUT_SYSTEM
            if (Keyboard.current != null && Keyboard.current.fKey.isPressed)
                return true;
#endif

#if ENABLE_LEGACY_INPUT_MANAGER
            if (Input.GetKey(KeyCode.F))
                return true;
#endif

            return false;
        }

        private bool ReadInteractPressed()
        {
#if ENABLE_INPUT_SYSTEM
            if (Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
                return true;
#endif

#if ENABLE_LEGACY_INPUT_MANAGER
            if (Input.GetKeyDown(KeyCode.E))
                return true;
#endif

            return false;
        }

        private bool ReadInteractHeld()
        {
#if ENABLE_INPUT_SYSTEM
            if (Keyboard.current != null && Keyboard.current.eKey.isPressed)
                return true;
#endif

#if ENABLE_LEGACY_INPUT_MANAGER
            if (Input.GetKey(KeyCode.E))
                return true;
#endif

            return false;
        }

        private void OnDestroy()
        {
            if (health != null)
            {
                health.Fainted -= OnFainted;
                health.Recovered -= OnRecovered;
                health.DamageBurstTriggered -= OnHorseDamageBurstTriggered;
            }
        }

        private void OnGUI()
        {
            if (!showDebugOverlay)
                return;

            GUI.Box(new Rect(12, 430, 450, 243), "B&D Clean Core — Horse");
            GUI.Label(new Rect(24, 460, 420, 22), $"State: {state}");
            GUI.Label(new Rect(24, 482, 420, 22), $"HP: {health.CurrentHealth:0}/{health.MaxHealth:0}");
            GUI.Label(new Rect(24, 504, 420, 22), $"Grounded: {(controller != null && controller.isGrounded)} | Y Vel: {verticalVelocity:0.00}");
            GUI.Label(new Rect(24, 526, 420, 22), $"Jumped This Frame: {jumpedThisFrame}");
            GUI.Label(new Rect(24, 548, 420, 22), $"Near Player: {playerInRange}");
            GUI.Label(new Rect(24, 570, 420, 22), $"Safe Spot: {(safeSpot != null ? safeSpot.name : "none")}");
            GUI.Label(new Rect(24, 592, 420, 22), $"Ride Move X/Y: {lastRideInput.x:0.00}, {lastRideInput.y:0.00}");
            GUI.Label(new Rect(24, 614, 420, 22), $"Dismount: {lastDismountMode}");
            GUI.Label(new Rect(24, 636, 420, 22), $"Player Scale: {(rider != null ? rider.localScale.ToString("F2") : "none")}");
            GUI.Label(new Rect(24, 658, 420, 22), $"Healing: {healingSessionActive} | Release Lock: {healingReleaseHoldActive}");
            GUI.Label(new Rect(24, 680, 420, 22), $"Last: {lastAction}");
        }
    }
}
