using System.Collections;
using UnityEngine;

#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace BoredomAndDungeons
{
    [DefaultExecutionOrder(120)]
    [DisallowMultipleComponent]
    [RequireComponent(typeof(BDHorseController))]
    [RequireComponent(typeof(BDHorseHealth))]
    [RequireComponent(typeof(CharacterController))]
    public sealed class BDHorseExhaustedFollowAndPetInteraction : MonoBehaviour
    {
        [Header("Zero-Health Exhausted Follow")]
        [SerializeField] private float exhaustedFollowBeginDistance = 14f;
        [SerializeField] private float exhaustedFollowStopDistance = 8f;
        [SerializeField] private float exhaustedFollowStartDelay = 1.25f;
        [SerializeField, Range(0.05f, 0.50f)]
        private float exhaustedFollowSpeedFraction = 0.20f;
        [SerializeField] private float referenceNormalFollowSpeed = 5.6f;
        [SerializeField] private float exhaustedTurnSpeedMultiplier = 0.45f;
        [SerializeField] private float stuckFallbackSeconds = 2.75f;
        [SerializeField] private float fallbackMinimumPlayerDistance = 24f;
        [SerializeField] private float fallbackSearchRadius = 4.5f;
        [SerializeField] private float fallbackBehindPlayerDistance = 3.2f;

        [Header("Pet Interaction")]
        [SerializeField] private float petInteractionRange = 2.25f;
        [SerializeField] private float petEmergencyCancelRange = 3.25f;
        [SerializeField] private float petLongPressThreshold = 0.65f;
        [SerializeField] private float playerPetsHorseDuration = 0.95f;
        [SerializeField] private float horseNuzzlesPlayerDuration = 1.20f;
        [SerializeField] private float nearbyEnemyBlockRadius = 9f;
        [SerializeField] private KeyCode desktopPetKey = KeyCode.Tab;

        [Header("Pet UI")]
        [SerializeField] private Vector2 petButtonSize = new Vector2(132f, 54f);
        [SerializeField] private Vector2 petButtonBottomRightMargin =
            new Vector2(24f, 118f);

        private BDHorseController horseController;
        private BDHorseHealth horseHealth;
        private CharacterController horseCharacterController;
        private BDHorseHazardSafety hazardSafety;
        private BDHorseCombatFleeController combatFlee;
        private BDHorseReliableFleeMotor reliableFlee;

        private Transform player;
        private BDPlayerController playerController;
        private BDPlayerCombat playerCombat;
        private BDHealth playerHealth;

        private bool exhaustedFollowActive;
        private float farDistanceStartedAt = -999f;
        private Vector3 lastProgressPosition;
        private float lastMeaningfulProgressAt = -999f;

        private bool petAvailable;
        private bool petHoldActive;
        private bool petHoldFromPointer;
        private bool pointerHeld;
        private float petHoldStartedAt = -999f;
        private Coroutine petRoutine;
        private bool petInteractionActive;
        private bool emergencyCancelRequested;

        private bool fleeStateCaptured;
        private bool combatFleeWasEnabled;
        private bool reliableFleeWasEnabled;
        private bool playerControllerWasEnabled;
        private bool playerCombatWasEnabled;
        private bool playerControlStateCaptured;
        private bool ownsHorseExternalControl;

        private float previousHorseHealth;
        private float previousPlayerHealth;
        private float nextPlayerResolveAt;

        private Transform horseVisual;
        private Transform playerVisual;
        private Quaternion horseVisualOriginalRotation;
        private Quaternion playerVisualOriginalRotation;

        public bool IsExhaustedFollowing => exhaustedFollowActive;
        public bool IsPetInteractionActive => petInteractionActive;
        public bool IsPetAvailable => petAvailable;
        public KeyCode DesktopPetKey => desktopPetKey;
        public float PetHoldProgress01 =>
            !petHoldActive
                ? 0f
                : Mathf.Clamp01(
                    (Time.unscaledTime - petHoldStartedAt) /
                    Mathf.Max(0.05f, petLongPressThreshold)
                );

        private void Awake()
        {
            if (desktopPetKey == KeyCode.P)
                desktopPetKey = KeyCode.Tab;

            horseController = GetComponent<BDHorseController>();
            horseHealth = GetComponent<BDHorseHealth>();
            horseCharacterController = GetComponent<CharacterController>();
            hazardSafety = GetComponent<BDHorseHazardSafety>();
            combatFlee = GetComponent<BDHorseCombatFleeController>();
            reliableFlee = GetComponent<BDHorseReliableFleeMotor>();
            previousHorseHealth =
                horseHealth != null ? horseHealth.CurrentHealth : 0f;

            EnsurePetAvailabilityIndicator();
        }

        private void EnsurePetAvailabilityIndicator()
        {
            if (!Application.isPlaying)
                return;

            if (GetComponent<BDHorsePetAvailabilityIndicator>() != null)
                return;

            gameObject.AddComponent<BDHorsePetAvailabilityIndicator>();
        }

        private void OnEnable()
        {
            if (horseHealth != null)
            {
                horseHealth.HealthChanged -= HandleHorseHealthChanged;
                horseHealth.HealthChanged += HandleHorseHealthChanged;
                horseHealth.Recovered -= HandleHorseRecovered;
                horseHealth.Recovered += HandleHorseRecovered;
            }

            ResolvePlayerReferences(force: true);
        }

        private void OnDisable()
        {
            if (horseHealth != null)
            {
                horseHealth.HealthChanged -= HandleHorseHealthChanged;
                horseHealth.Recovered -= HandleHorseRecovered;
            }

            UnsubscribePlayerHealth();

            if (petRoutine != null)
            {
                StopCoroutine(petRoutine);
                petRoutine = null;
            }

            RestoreVisualRotations();
            RestorePlayerControls();
            StopExhaustedFollow();
            ReleaseHorseExternalControl();
            RestoreFleeComponents();
            ClearPetHold();
            petInteractionActive = false;
        }

        private void Update()
        {
            ResolvePlayerReferences(force: false);

            if (petInteractionActive)
            {
                petAvailable = false;
                return;
            }

            petAvailable = CanOfferPetInteraction();
            TickDesktopPetInput();
            TickPendingPetHold();

            if (!petInteractionActive)
                TickExhaustedFollow();
        }

        private void TickExhaustedFollow()
        {
            if (horseHealth == null ||
                horseController == null ||
                player == null ||
                !horseHealth.IsFainted)
            {
                StopExhaustedFollow();
                farDistanceStartedAt = -999f;
                return;
            }

            if (horseController.IsMounted ||
                (hazardSafety != null && hazardSafety.IsRecovering))
            {
                StopExhaustedFollow();
                return;
            }

            Vector3 toPlayer = player.position - transform.position;
            toPlayer.y = 0f;
            float distance = toPlayer.magnitude;

            if (exhaustedFollowActive)
            {
                if (distance <= Mathf.Max(0.5f, exhaustedFollowStopDistance))
                {
                    StopExhaustedFollow();
                    farDistanceStartedAt = -999f;
                    return;
                }

                TickExhaustedMovement(toPlayer, distance);
                return;
            }

            if (distance <= Mathf.Max(
                    exhaustedFollowStopDistance + 0.25f,
                    exhaustedFollowBeginDistance))
            {
                farDistanceStartedAt = -999f;
                return;
            }

            if (farDistanceStartedAt < 0f)
            {
                farDistanceStartedAt = Time.unscaledTime;
                return;
            }

            if (Time.unscaledTime - farDistanceStartedAt <
                Mathf.Max(0f, exhaustedFollowStartDelay))
            {
                return;
            }

            BeginExhaustedFollow();
        }

        private void BeginExhaustedFollow()
        {
            if (exhaustedFollowActive ||
                horseController == null ||
                horseHealth == null ||
                !horseHealth.IsFainted)
            {
                return;
            }

            exhaustedFollowActive = true;
            SuppressFleeComponents();
            AcquireHorseExternalControl(
                "zero-health exhausted follow"
            );

            lastProgressPosition = transform.position;
            lastMeaningfulProgressAt = Time.unscaledTime;
        }

        private void StopExhaustedFollow()
        {
            if (!exhaustedFollowActive)
                return;

            exhaustedFollowActive = false;

            if (!petInteractionActive)
            {
                ReleaseHorseExternalControl();
                RestoreFleeComponents();
            }
        }

        private void TickExhaustedMovement(Vector3 toPlayer, float distance)
        {
            if (toPlayer.sqrMagnitude < 0.001f)
                return;

            Vector3 direction = toPlayer.normalized;
            float speed =
                Mathf.Max(0.25f, referenceNormalFollowSpeed) *
                Mathf.Clamp(exhaustedFollowSpeedFraction, 0.05f, 0.50f);

            Vector3 motion =
                direction * speed * Mathf.Max(0f, Time.deltaTime) +
                Vector3.down * 2f * Mathf.Max(0f, Time.deltaTime);

            horseController.MoveByExternalControl(
                motion,
                direction,
                exhaustedTurnSpeedMultiplier
            );

            Vector3 progressDelta = transform.position - lastProgressPosition;
            progressDelta.y = 0f;

            if (progressDelta.sqrMagnitude >= 0.16f)
            {
                lastProgressPosition = transform.position;
                lastMeaningfulProgressAt = Time.unscaledTime;
                return;
            }

            bool stuckLongEnough =
                Time.unscaledTime - lastMeaningfulProgressAt >=
                Mathf.Max(0.75f, stuckFallbackSeconds);

            bool farEnoughForFallback =
                distance >= Mathf.Max(
                    exhaustedFollowBeginDistance + 2f,
                    fallbackMinimumPlayerDistance
                );

            if (!stuckLongEnough || !farEnoughForFallback)
                return;

            TryUseSafeFallbackNearPlayer();
            lastProgressPosition = transform.position;
            lastMeaningfulProgressAt = Time.unscaledTime;
        }

        private void TryUseSafeFallbackNearPlayer()
        {
            if (player == null ||
                hazardSafety == null ||
                horseCharacterController == null)
            {
                return;
            }

            Vector3 playerForward = player.forward;
            playerForward.y = 0f;

            if (playerForward.sqrMagnitude < 0.001f)
                playerForward = Vector3.forward;

            playerForward.Normalize();

            Vector3 preferred =
                player.position -
                playerForward * Mathf.Max(1.5f, fallbackBehindPlayerDistance) +
                player.right * 1.15f;

            if (!hazardSafety.TryResolveSafePositionNear(
                    preferred,
                    Mathf.Max(1f, fallbackSearchRadius),
                    out Vector3 safePosition))
            {
                return;
            }

            bool wasEnabled = horseCharacterController.enabled;

            if (wasEnabled)
                horseCharacterController.enabled = false;

            transform.position = safePosition;
            Physics.SyncTransforms();

            if (wasEnabled)
                horseCharacterController.enabled = true;

            Vector3 facePlayer = player.position - transform.position;
            facePlayer.y = 0f;

            horseController.MoveByExternalControl(
                Vector3.zero,
                facePlayer,
                exhaustedTurnSpeedMultiplier
            );
        }

        private void TickDesktopPetInput()
        {
            if (!petHoldActive)
            {
                if (petAvailable && ReadPetPressed())
                    BeginPetHold(fromPointer: false);

                return;
            }

            if (petHoldFromPointer)
                return;

            if (ReadPetReleased() || !ReadPetHeld())
                ReleasePetHold();
        }

        private void TickPendingPetHold()
        {
            if (!petHoldActive || petInteractionActive)
                return;

            if (!CanOfferPetInteraction())
            {
                ClearPetHold();
                return;
            }

            bool stillHeld =
                petHoldFromPointer ? pointerHeld : ReadPetHeld();

            if (!stillHeld)
                return;

            if (Time.unscaledTime - petHoldStartedAt <
                Mathf.Max(0.05f, petLongPressThreshold))
            {
                return;
            }

            StartPetInteraction(horseNuzzlesPlayer: true);
        }

        private void BeginPetHold(bool fromPointer)
        {
            if (petHoldActive ||
                petInteractionActive ||
                !CanOfferPetInteraction())
            {
                return;
            }

            petHoldActive = true;
            petHoldFromPointer = fromPointer;
            pointerHeld = fromPointer;
            petHoldStartedAt = Time.unscaledTime;
        }

        private void ReleasePetHold()
        {
            if (!petHoldActive)
                return;

            float heldSeconds = Time.unscaledTime - petHoldStartedAt;

            bool triggerShort =
                !petInteractionActive &&
                heldSeconds < Mathf.Max(0.05f, petLongPressThreshold) &&
                CanOfferPetInteraction();

            ClearPetHold();

            if (triggerShort)
                StartPetInteraction(horseNuzzlesPlayer: false);
        }

        private void ClearPetHold()
        {
            petHoldActive = false;
            petHoldFromPointer = false;
            pointerHeld = false;
            petHoldStartedAt = -999f;
        }

        private void StartPetInteraction(bool horseNuzzlesPlayer)
        {
            if (petInteractionActive || !CanOfferPetInteraction())
            {
                ClearPetHold();
                return;
            }

            ClearPetHold();

            if (petRoutine != null)
                StopCoroutine(petRoutine);

            petRoutine = StartCoroutine(
                PetInteractionRoutine(horseNuzzlesPlayer)
            );
        }

        private IEnumerator PetInteractionRoutine(bool horseNuzzlesPlayer)
        {
            petInteractionActive = true;
            emergencyCancelRequested = false;
            StopExhaustedFollow();
            SuppressFleeComponents();

            AcquireHorseExternalControl(
                horseNuzzlesPlayer
                    ? "horse nuzzles player"
                    : "player pets horse"
            );

            CaptureAndDisablePlayerControls();
            CaptureVisualTransforms();

            float duration =
                horseNuzzlesPlayer
                    ? Mathf.Max(0.35f, horseNuzzlesPlayerDuration)
                    : Mathf.Max(0.35f, playerPetsHorseDuration);

            float elapsed = 0f;

            while (elapsed < duration)
            {
                if (ShouldEmergencyCancelPet())
                    break;

                elapsed += Mathf.Max(0f, Time.unscaledDeltaTime);
                float progress = Mathf.Clamp01(elapsed / duration);

                FaceInteractionPair();
                AnimatePetVisuals(horseNuzzlesPlayer, progress);
                yield return null;
            }

            RestoreVisualRotations();
            RestorePlayerControls();
            ReleaseHorseExternalControl();
            RestoreFleeComponents();

            petInteractionActive = false;
            emergencyCancelRequested = false;
            petRoutine = null;
        }

        private bool ShouldEmergencyCancelPet()
        {
            if (emergencyCancelRequested ||
                player == null ||
                horseHealth == null)
            {
                return true;
            }

            if (Vector3.Distance(player.position, transform.position) >
                Mathf.Max(petInteractionRange, petEmergencyCancelRange))
            {
                return true;
            }

            if (hazardSafety != null && hazardSafety.IsRecovering)
                return true;

            if (Time.timeScale <= 0f)
                return true;

            return playerHealth != null && playerHealth.IsDead;
        }

        private void CaptureAndDisablePlayerControls()
        {
            playerControlStateCaptured = true;

            if (playerController != null)
            {
                playerControllerWasEnabled = playerController.enabled;
                playerController.enabled = false;
            }

            if (playerCombat != null)
            {
                playerCombatWasEnabled = playerCombat.enabled;
                playerCombat.enabled = false;
            }
        }

        private void RestorePlayerControls()
        {
            if (!playerControlStateCaptured)
                return;

            if (playerController != null)
                playerController.enabled = playerControllerWasEnabled;

            if (playerCombat != null)
                playerCombat.enabled = playerCombatWasEnabled;

            playerControlStateCaptured = false;
        }

        private void CaptureVisualTransforms()
        {
            horseVisual = FindPrimaryVisual(transform);
            playerVisual = FindPrimaryVisual(player);

            if (horseVisual != null)
                horseVisualOriginalRotation = horseVisual.localRotation;

            if (playerVisual != null)
                playerVisualOriginalRotation = playerVisual.localRotation;
        }

        private void RestoreVisualRotations()
        {
            if (horseVisual != null)
                horseVisual.localRotation = horseVisualOriginalRotation;

            if (playerVisual != null)
                playerVisual.localRotation = playerVisualOriginalRotation;

            horseVisual = null;
            playerVisual = null;
        }

        private static Transform FindPrimaryVisual(Transform actor)
        {
            if (actor == null)
                return null;

            Renderer[] renderers =
                actor.GetComponentsInChildren<Renderer>(includeInactive: true);

            for (int index = 0; index < renderers.Length; index++)
            {
                Renderer renderer = renderers[index];

                if (renderer == null || renderer.transform == actor)
                    continue;

                Transform candidate = renderer.transform;

                while (candidate.parent != null &&
                       candidate.parent != actor)
                {
                    candidate = candidate.parent;
                }

                if (candidate != actor)
                    return candidate;
            }

            return null;
        }

        private void FaceInteractionPair()
        {
            if (player == null)
                return;

            Vector3 horseToPlayer = player.position - transform.position;
            horseToPlayer.y = 0f;

            horseController.MoveByExternalControl(
                Vector3.zero,
                horseToPlayer,
                1.35f
            );

            Vector3 playerToHorse = -horseToPlayer;

            if (playerToHorse.sqrMagnitude < 0.001f)
                return;

            Quaternion target =
                Quaternion.LookRotation(playerToHorse.normalized, Vector3.up);

            player.rotation = Quaternion.Slerp(
                player.rotation,
                target,
                1f - Mathf.Exp(
                    -12f * Mathf.Max(0f, Time.unscaledDeltaTime))
            );
        }

        private void AnimatePetVisuals(
            bool horseNuzzlesPlayer,
            float progress)
        {
            float wave = Mathf.Sin(progress * Mathf.PI);

            if (horseVisual != null)
            {
                Quaternion motion =
                    horseNuzzlesPlayer
                        ? Quaternion.Euler(
                            8f * wave,
                            -10f * wave,
                            5f * wave)
                        : Quaternion.Euler(
                            7f * wave,
                            0f,
                            -3f * wave);

                horseVisual.localRotation =
                    horseVisualOriginalRotation * motion;
            }

            if (playerVisual != null)
            {
                Quaternion motion =
                    horseNuzzlesPlayer
                        ? Quaternion.Euler(
                            -5f * wave,
                            0f,
                            4f * wave)
                        : Quaternion.Euler(
                            11f * wave,
                            0f,
                            -4f * wave);

                playerVisual.localRotation =
                    playerVisualOriginalRotation * motion;
            }
        }

        private bool CanOfferPetInteraction()
        {
            if (petInteractionActive ||
                player == null ||
                horseController == null ||
                horseHealth == null)
            {
                return false;
            }

            if (horseController.IsMounted ||
                (horseController.IsExternallyControlled &&
                 !exhaustedFollowActive))
            {
                return false;
            }

            if (Vector3.Distance(player.position, transform.position) >
                Mathf.Max(0.25f, petInteractionRange))
            {
                return false;
            }

            if (playerController != null &&
                (!playerController.enabled ||
                 playerController.IsDodging))
            {
                return false;
            }

            if (playerCombat != null &&
                (!playerCombat.enabled ||
                 playerCombat.IsChargingRangedShot ||
                 playerCombat.IsLightAttackHoldPending))
            {
                return false;
            }

            if (hazardSafety != null &&
                (hazardSafety.IsRecovering ||
                 hazardSafety.IsRefusingHazard))
            {
                return false;
            }

            if (HasNearbyLivingEnemy())
                return false;

            return HasClearInteractionLine();
        }

        private bool HasNearbyLivingEnemy()
        {
            float radius = Mathf.Max(1f, nearbyEnemyBlockRadius);
            float radiusSquared = radius * radius;

            BDHealth[] candidates =
                FindObjectsByType<BDHealth>(
                    FindObjectsInactive.Exclude,
                    FindObjectsSortMode.None
                );

            for (int index = 0; index < candidates.Length; index++)
            {
                BDHealth candidate = candidates[index];

                if (candidate == null ||
                    candidate.IsDead ||
                    candidate.GetComponentInParent<BDPlayerMarker>() != null ||
                    candidate.GetComponentInParent<BDHorseHealth>() != null)
                {
                    continue;
                }

                Vector3 horseDelta =
                    candidate.transform.position - transform.position;
                horseDelta.y = 0f;

                Vector3 playerDelta =
                    candidate.transform.position - player.position;
                playerDelta.y = 0f;

                if (horseDelta.sqrMagnitude <= radiusSquared ||
                    playerDelta.sqrMagnitude <= radiusSquared)
                {
                    return true;
                }
            }

            return false;
        }

        private bool HasClearInteractionLine()
        {
            Vector3 origin = player.position + Vector3.up * 0.9f;
            Vector3 target = transform.position + Vector3.up * 0.9f;
            Vector3 delta = target - origin;
            float distance = delta.magnitude;

            if (distance <= 0.05f)
                return true;

            RaycastHit[] hits = Physics.RaycastAll(
                origin,
                delta.normalized,
                distance,
                ~0,
                QueryTriggerInteraction.Ignore
            );

            System.Array.Sort(
                hits,
                (left, right) => left.distance.CompareTo(right.distance)
            );

            for (int index = 0; index < hits.Length; index++)
            {
                Collider collider = hits[index].collider;

                if (collider == null)
                    continue;

                Transform hit = collider.transform;

                if (hit == transform ||
                    hit.IsChildOf(transform) ||
                    hit == player ||
                    hit.IsChildOf(player))
                {
                    continue;
                }

                return false;
            }

            return true;
        }

        private void ResolvePlayerReferences(bool force)
        {
            if (!force &&
                player != null &&
                Time.unscaledTime < nextPlayerResolveAt)
            {
                return;
            }

            nextPlayerResolveAt = Time.unscaledTime + 0.50f;

            Transform resolved =
                player != null ? player : BDTargetFinder.FindPlayer();

            if (resolved == player && !force)
                return;

            UnsubscribePlayerHealth();
            player = resolved;

            if (player == null)
            {
                playerController = null;
                playerCombat = null;
                playerHealth = null;
                return;
            }

            playerController = player.GetComponent<BDPlayerController>();
            playerCombat = player.GetComponent<BDPlayerCombat>();
            playerHealth = player.GetComponent<BDHealth>();

            if (playerHealth != null)
            {
                previousPlayerHealth = playerHealth.CurrentHealth;
                playerHealth.HealthChanged -= HandlePlayerHealthChanged;
                playerHealth.HealthChanged += HandlePlayerHealthChanged;
            }
        }

        private void UnsubscribePlayerHealth()
        {
            if (playerHealth != null)
                playerHealth.HealthChanged -= HandlePlayerHealthChanged;
        }

        private void HandleHorseHealthChanged(
            BDHorseHealth source,
            float current,
            float maximum)
        {
            if (petInteractionActive &&
                current < previousHorseHealth - 0.001f)
            {
                emergencyCancelRequested = true;
            }

            previousHorseHealth = current;

            if (!source.IsFainted)
                StopExhaustedFollow();
        }

        private void HandleHorseRecovered(BDHorseHealth source)
        {
            StopExhaustedFollow();
        }

        private void HandlePlayerHealthChanged(
            BDHealth source,
            float current,
            float maximum)
        {
            if (petInteractionActive &&
                current < previousPlayerHealth - 0.001f)
            {
                emergencyCancelRequested = true;
            }

            previousPlayerHealth = current;
        }

        private void SuppressFleeComponents()
        {
            if (fleeStateCaptured)
                return;

            fleeStateCaptured = true;

            if (combatFlee != null)
            {
                combatFleeWasEnabled = combatFlee.enabled;
                combatFlee.enabled = false;
            }

            if (reliableFlee != null)
            {
                reliableFleeWasEnabled = reliableFlee.enabled;
                reliableFlee.enabled = false;
            }
        }

        private void RestoreFleeComponents()
        {
            if (!fleeStateCaptured)
                return;

            if (combatFlee != null)
                combatFlee.enabled = combatFleeWasEnabled;

            if (reliableFlee != null)
                reliableFlee.enabled = reliableFleeWasEnabled;

            fleeStateCaptured = false;
        }

        private void AcquireHorseExternalControl(
            string reason)
        {
            if (horseController == null)
                return;

            horseController.SetExternalControlLock(true, reason);
            ownsHorseExternalControl = true;
        }

        private void ReleaseHorseExternalControl()
        {
            if (!ownsHorseExternalControl)
                return;

            if (horseController != null &&
                horseController.IsExternallyControlled)
            {
                horseController.SetExternalControlLock(false, "released");
            }

            ownsHorseExternalControl = false;
        }

        private bool ReadPetPressed()
        {
#if ENABLE_INPUT_SYSTEM
            if (Keyboard.current != null &&
                Keyboard.current.pKey.wasPressedThisFrame)
            {
                return true;
            }
#endif

#if ENABLE_LEGACY_INPUT_MANAGER
            if (Input.GetKeyDown(desktopPetKey))
                return true;
#endif

            return false;
        }

        private bool ReadPetHeld()
        {
#if ENABLE_INPUT_SYSTEM
            if (Keyboard.current != null &&
                Keyboard.current.pKey.isPressed)
            {
                return true;
            }
#endif

#if ENABLE_LEGACY_INPUT_MANAGER
            if (Input.GetKey(desktopPetKey))
                return true;
#endif

            return false;
        }

        private bool ReadPetReleased()
        {
#if ENABLE_INPUT_SYSTEM
            if (Keyboard.current != null &&
                Keyboard.current.pKey.wasReleasedThisFrame)
            {
                return true;
            }
#endif

#if ENABLE_LEGACY_INPUT_MANAGER
            if (Input.GetKeyUp(desktopPetKey))
                return true;
#endif

            return false;
        }

        private void OnGUI()
        {
            if (!petAvailable &&
                !petHoldActive &&
                !petInteractionActive)
            {
                return;
            }

            Rect button = ResolvePetButtonRect();
            Color previousColor = GUI.color;
            Color previousContent = GUI.contentColor;

            GUI.color = petInteractionActive
                ? new Color(0.78f, 0.78f, 0.78f, 0.92f)
                : new Color(0.92f, 0.78f, 0.42f, 0.96f);

            GUI.Box(button, GUIContent.none);

            if (petHoldActive)
            {
                Rect progressRect = new Rect(
                    button.x + 4f,
                    button.yMax - 8f,
                    (button.width - 8f) * PetHoldProgress01,
                    4f
                );

                GUI.color = new Color(1f, 0.95f, 0.65f, 1f);
                GUI.DrawTexture(progressRect, Texture2D.whiteTexture);
            }

            GUI.color = previousColor;
            GUI.contentColor = Color.black;

            string label = BuildPetButtonLabel();

            GUI.Label(
                button,
                label,
                new GUIStyle(GUI.skin.label)
                {
                    alignment = TextAnchor.MiddleCenter,
                    fontStyle = FontStyle.Bold,
                    fontSize = 16
                }
            );

            GUI.contentColor = previousContent;

            Event current = Event.current;

            if (!petInteractionActive &&
                current.type == EventType.MouseDown &&
                current.button == 0 &&
                button.Contains(current.mousePosition))
            {
                BeginPetHold(fromPointer: true);
                pointerHeld = true;
                current.Use();
            }

            if (petHoldFromPointer &&
                current.type == EventType.MouseUp &&
                current.button == 0)
            {
                pointerHeld = false;
                ReleasePetHold();
                current.Use();
            }
        }

        private string BuildPetButtonLabel()
        {
            if (petInteractionActive)
                return "Petting...";

            if (petHoldActive)
            {
                return
                    $"Pet  {Mathf.RoundToInt(PetHoldProgress01 * 100f)}%";
            }

            string keyLabel =
                desktopPetKey == KeyCode.None
                    ? "Unbound"
                    : desktopPetKey.ToString();

            return $"Pet [{keyLabel}]";
        }

        private Rect ResolvePetButtonRect()
        {
            float width = Mathf.Max(96f, petButtonSize.x);
            float height = Mathf.Max(42f, petButtonSize.y);

            float x = Mathf.Max(
                8f,
                Screen.width -
                width -
                Mathf.Max(8f, petButtonBottomRightMargin.x)
            );

            float y = Mathf.Max(
                8f,
                Screen.height -
                height -
                Mathf.Max(8f, petButtonBottomRightMargin.y)
            );

            return new Rect(x, y, width, height);
        }
    }
}
