using UnityEngine;

namespace BoredomAndDungeons
{
    [DefaultExecutionOrder(80)]
    [DisallowMultipleComponent]
    public sealed class BDQuicksandStatus : MonoBehaviour
    {
        // BD EXACT PLAYER QUICKSAND ESCAPE/DAMAGE V23R17
        [Header("Quicksand Feel")]
        [SerializeField] private float touchGraceSeconds = 0.18f;
        [SerializeField] private float secondsToHalfBodySink = 4.2f;
        [SerializeField] private float jumpEscapeAmount = 0.105f;
        [SerializeField] private float escapeRecoverySeconds = 0.85f;
        [SerializeField] private float entryMoveMultiplier = 0.78f;
        [SerializeField] private float halfBodyMoveMultiplier = 0.24f;
        [SerializeField] private float playerDamagePerSecond = 2f;
        [SerializeField] private float playerDamageInterval = 1f;
        [SerializeField] private float halfBodyFailureThreshold = 0.50f;
        [SerializeField] private float enemyFullSinkSeconds = 3.2f;
        [SerializeField] private float retriggerProtectionSeconds = 1.0f;

        [Header("Visual")]
        [SerializeField] private Color sandRingColor =
            new Color(0.82f, 0.54f, 0.20f, 0.92f);
        [SerializeField] private int ringSegments = 32;
        [SerializeField] private float ringWidth = 0.085f;

        private CharacterController controller;
        private BDPlayerController player;
        private BDHorseController horse;
        private BDPlayerHazardRecovery playerRecovery;
        private BDHorseHazardSafety horseSafety;
        private BDHealth health;
        private BDHorseHealth horseHealth;
        private BDEnemyHazardNavigation enemyNavigation;
        private BDHazardVolume source;
        private LineRenderer ring;
        private Transform visualRoot;
        private Vector3 visualRestLocalPosition;
        private bool visualRestCaptured;
        private float lastTouchAt = -999f;
        private float protectedUntil = -999f;
        private float nextPlayerDamageAt = -999f;
        private float sink01;
        private bool active;
        private bool failureTriggered;
        private int lastObservedJumpSequence;

        public bool IsActive => active;
        public float Sink01 => sink01;
        // BD QUICKSAND OWNS NONLETHAL JUMP/GROUND RECOVERY V23R19
        public bool BlocksGenericGroundRecovery =>
            active || sink01 > 0.001f;
        public float MovementMultiplier => active
            ? Mathf.Lerp(
                Mathf.Clamp(entryMoveMultiplier, 0.1f, 1f),
                Mathf.Clamp(halfBodyMoveMultiplier, 0.05f, 1f),
                Mathf.Clamp01(sink01 / Mathf.Max(0.05f, halfBodyFailureThreshold)))
            : 1f;

        private bool IsEnemy =>
            player == null && horse == null && health != null;

        private void Awake()
        {
            ResolveReferences();
            ResolveVisualRoot();
            EnsureRing();
            SetRingVisible(false);
        }

        private void OnDisable()
        {
            active = false;
            sink01 = 0f;
            source = null;
            ApplyPlayerMovementMultiplier(1f);
            RestoreVisual();
            SetRingVisible(false);
        }

        private void Update()
        {
            if (!Application.isPlaying)
                return;

            ResolveReferences();
            ResolveVisualRoot();

            bool recentTouch =
                Time.unscaledTime - lastTouchAt <=
                Mathf.Max(0.05f, touchGraceSeconds);
            bool protectedNow = Time.unscaledTime < protectedUntil;
            bool touchingSurface =
                recentTouch &&
                source != null &&
                controller != null &&
                source.IsActorTouchingSurface(controller, 0.10f);

            if (touchingSurface && !protectedNow)
            {
                if (!active)
                {
                    active = true;
                    failureTriggered = false;
                    nextPlayerDamageAt =
                        Time.unscaledTime + Mathf.Max(0.1f, playerDamageInterval);
                    lastObservedJumpSequence =
                        player != null ? player.JumpSequence : 0;
                    BDGameFeelAudio.PlayQuicksandEnter();
                }

                if (player != null)
                {
                    TickPlayerQuicksand();
                    // BD EXPLICIT QUICKSAND SPEED OWNER V23R19D
                    // Push the current depth-derived value directly to the
                    // movement owner every active frame.
                    ApplyPlayerMovementMultiplier(MovementMultiplier);
                }
                else if (horse != null)
                    TickHorseQuicksand();
                else if (IsEnemy)
                    TickEnemyQuicksand();

                ApplyVisualSink();
                UpdateRing();
                return;
            }

            RecoverAfterLeavingSurface();
        }

        private void TickPlayerQuicksand()
        {
            int jumpSequence = player.JumpSequence;
            if (jumpSequence != lastObservedJumpSequence)
            {
                int jumps = Mathf.Max(1, jumpSequence - lastObservedJumpSequence);
                sink01 = Mathf.Max(
                    0f,
                    sink01 - Mathf.Max(0.01f, jumpEscapeAmount) * jumps
                );
                lastObservedJumpSequence = jumpSequence;
            }

            bool groundedContact = player.IsGrounded;
            if (!groundedContact)
                return;

            if (!player.IsDodging)
            {
                float sinkPerSecond =
                    Mathf.Max(0.05f, halfBodyFailureThreshold) /
                    Mathf.Max(0.50f, secondsToHalfBodySink);
                sink01 = Mathf.MoveTowards(
                    sink01,
                    halfBodyFailureThreshold,
                    sinkPerSecond * Time.unscaledDeltaTime
                );
            }

            while (Time.unscaledTime >= nextPlayerDamageAt)
            {
                nextPlayerDamageAt += Mathf.Max(0.10f, playerDamageInterval);
                if (health != null && !health.IsDead)
                {
                    // BD QUIET PERIODIC QUICKSAND DAMAGE V10.11.30.31
                    // Keep the authored unavoidable damage, number feedback,
                    // shake and health event, but do not print a full stack trace
                    // every periodic environmental tick.
                    health.ApplyUnavoidableDamage(
                        Mathf.Max(0f, playerDamagePerSecond),
                        logResolvedDamage: false
                    );
                }
            }

            if (sink01 >= halfBodyFailureThreshold && !failureTriggered)
                TriggerPlayerHalfBodyFailure();
        }

        private void TickHorseQuicksand()
        {
            float rate = 1f / Mathf.Max(1.0f, secondsToHalfBodySink * 1.15f);
            sink01 = Mathf.MoveTowards(
                sink01,
                1f,
                rate * Time.unscaledDeltaTime
            );

            if (sink01 >= 0.999f && !failureTriggered)
                TriggerHorseFullSink();
        }

        private void TickEnemyQuicksand()
        {
            float rate = 1f / Mathf.Max(0.75f, enemyFullSinkSeconds);
            sink01 = Mathf.MoveTowards(
                sink01,
                1f,
                rate * Time.unscaledDeltaTime
            );

            if (sink01 >= 0.999f && !failureTriggered)
                TriggerEnemyFullSink();
        }

        private void RecoverAfterLeavingSurface()
        {
            if (sink01 > 0f)
            {
                sink01 = Mathf.MoveTowards(
                    sink01,
                    0f,
                    Time.unscaledDeltaTime /
                    Mathf.Max(0.15f, escapeRecoverySeconds)
                );
                ApplyVisualSink();
                UpdateRing();
                ApplyPlayerMovementMultiplier(MovementMultiplier);
            }

            if (sink01 > 0.001f)
                return;

            if (active)
                BDGameFeelAudio.PlayQuicksandEscape();

            active = false;
            source = null;
            failureTriggered = false;
            ApplyPlayerMovementMultiplier(1f);
            RestoreVisual();
            SetRingVisible(false);
        }

        private void ApplyPlayerMovementMultiplier(float multiplier)
        {
            if (player != null)
                player.SetQuicksandMovementMultiplier(multiplier);
        }

        public void Touch(BDHazardVolume volume)
        {
            if (volume == null ||
                volume.HazardType != BDHazardType.Quicksand ||
                Time.unscaledTime < protectedUntil)
            {
                return;
            }

            source = volume;
            lastTouchAt = Time.unscaledTime;
            EnsureRing();
        }

        public static void TouchActor(
            Collider actorCollider,
            BDHazardVolume volume)
        {
            if (!Application.isPlaying ||
                actorCollider == null ||
                volume == null)
            {
                return;
            }

            GameObject actor = ResolveActorRoot(actorCollider);
            if (actor == null)
                return;

            BDQuicksandStatus status =
                actor.GetComponent<BDQuicksandStatus>();

            if (status == null)
                status = actor.AddComponent<BDQuicksandStatus>();

            status.Touch(volume);
        }

        public static bool IsActorUnderQuicksandControl(GameObject actor)
        {
            if (actor == null)
                return false;

            BDQuicksandStatus status =
                actor.GetComponent<BDQuicksandStatus>();

            return status != null &&
                   status.BlocksGenericGroundRecovery;
        }

        public static float ResolveMovementMultiplier(GameObject actor)
        {
            if (actor == null)
                return 1f;

            BDQuicksandStatus status =
                actor.GetComponent<BDQuicksandStatus>();

            return status != null
                ? status.MovementMultiplier
                : 1f;
        }

        public static Vector3 FilterMotion(
            GameObject actor,
            Vector3 requestedMotion)
        {
            float multiplier = ResolveMovementMultiplier(actor);
            if (multiplier >= 0.999f)
                return requestedMotion;

            requestedMotion.x *= multiplier;
            requestedMotion.z *= multiplier;

            // Jump height remains intact. Jumping is the deliberate escape
            // action and must not be weakened by the sand filter.
            return requestedMotion;
        }

        private static GameObject ResolveActorRoot(Collider actorCollider)
        {
            BDHorseController horseController =
                actorCollider.GetComponentInParent<BDHorseController>();
            if (horseController != null)
                return horseController.gameObject;

            BDPlayerMarker playerMarker =
                actorCollider.GetComponentInParent<BDPlayerMarker>();
            if (playerMarker != null)
                return playerMarker.gameObject;

            BDHealth enemyHealth =
                actorCollider.GetComponentInParent<BDHealth>();
            if (enemyHealth != null &&
                enemyHealth.GetComponent<BDHorseHealth>() == null)
            {
                return enemyHealth.gameObject;
            }

            return null;
        }

        private void ResolveReferences()
        {
            if (controller == null)
                controller = GetComponent<CharacterController>();
            if (player == null)
                player = GetComponent<BDPlayerController>();
            if (horse == null)
                horse = GetComponent<BDHorseController>();
            if (playerRecovery == null)
                playerRecovery = GetComponent<BDPlayerHazardRecovery>();
            if (horseSafety == null)
                horseSafety = GetComponent<BDHorseHazardSafety>();
            if (health == null)
                health = GetComponent<BDHealth>();
            if (horseHealth == null)
                horseHealth = GetComponent<BDHorseHealth>();
            if (enemyNavigation == null)
                enemyNavigation = GetComponent<BDEnemyHazardNavigation>();
        }

        private void TriggerPlayerHalfBodyFailure()
        {
            failureTriggered = true;
            BDGameFeelAudio.PlayQuicksandSink();

            bool handled = playerRecovery != null &&
                playerRecovery.TryHandleHazard(source, true);

            if (!handled && health != null)
                health.ApplyUnavoidableDamage(source != null ? source.Damage : 15f);

            FinishFailure();
        }

        private void TriggerHorseFullSink()
        {
            failureTriggered = true;
            BDGameFeelAudio.PlayQuicksandSink();

            bool handled = horseSafety != null &&
                horseSafety.TryHandleHazard(source, true);

            if (!handled && horseHealth != null)
                horseHealth.ApplyDamage(source != null ? source.Damage : 15f);

            FinishFailure();
        }

        private void TriggerEnemyFullSink()
        {
            failureTriggered = true;
            BDGameFeelAudio.PlayQuicksandSink();

            if (health != null && !health.IsDead)
            {
                health.ApplyUnavoidableDamage(
                    Mathf.Max(1f, health.CurrentHealth + health.MaxHealth)
                );
            }

            FinishFailure();
        }

        private void FinishFailure()
        {
            protectedUntil =
                Time.unscaledTime +
                Mathf.Max(0.25f, retriggerProtectionSeconds);
            sink01 = 0f;
            active = false;
            source = null;
            ApplyPlayerMovementMultiplier(1f);
            RestoreVisual();
            SetRingVisible(false);
        }

        private void ResolveVisualRoot()
        {
            if (visualRoot != null)
                return;

            if (player != null)
                visualRoot = transform.Find("BD_Player_Visual");
            else
                visualRoot = transform.Find("Visual");

            if (visualRoot == null)
                return;

            visualRestLocalPosition = visualRoot.localPosition;
            visualRestCaptured = true;
        }

        private void ApplyVisualSink()
        {
            if (!visualRestCaptured || visualRoot == null)
                return;

            float bodyHeight = controller != null
                ? controller.height * Mathf.Abs(transform.lossyScale.y)
                : 1.8f;
            float normalized = player != null
                ? Mathf.Clamp01(sink01 / Mathf.Max(0.05f, halfBodyFailureThreshold))
                : Mathf.Clamp01(sink01);
            float depth = bodyHeight * (player != null ? 0.46f : 0.55f) * normalized;
            visualRoot.localPosition =
                visualRestLocalPosition + Vector3.down * depth;
        }

        private void RestoreVisual()
        {
            if (visualRestCaptured && visualRoot != null)
                visualRoot.localPosition = visualRestLocalPosition;
        }

        private void EnsureRing()
        {
            if (ring != null)
                return;

            GameObject ringObject = new GameObject("BD_QuicksandSinkRing");
            ringObject.transform.SetParent(transform, false);
            ring = ringObject.AddComponent<LineRenderer>();
            ring.useWorldSpace = true;
            ring.loop = true;
            ring.positionCount = Mathf.Clamp(ringSegments, 16, 64);
            ring.startWidth = ringWidth;
            ring.endWidth = ringWidth;
            ring.startColor = sandRingColor;
            ring.endColor = sandRingColor;
            ring.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            ring.receiveShadows = false;

            Shader shader = Shader.Find("Sprites/Default");
            if (shader != null)
                ring.material = new Material(shader);
        }

        private void UpdateRing()
        {
            EnsureRing();
            if (ring == null)
                return;

            SetRingVisible(active || sink01 > 0.001f);

            Bounds bounds = controller != null
                ? controller.bounds
                : new Bounds(transform.position, Vector3.one);

            float radius = Mathf.Max(
                0.42f,
                Mathf.Max(bounds.extents.x, bounds.extents.z) + 0.16f
            );
            radius *= Mathf.Lerp(1.08f, 0.82f, sink01);

            float surfaceY = source != null
                ? source.SurfaceY + 0.045f
                : bounds.min.y + 0.045f;

            Vector3 center = new Vector3(
                bounds.center.x,
                surfaceY,
                bounds.center.z
            );

            int count = ring.positionCount;
            for (int index = 0; index < count; index++)
            {
                float angle = index * Mathf.PI * 2f / count;
                ring.SetPosition(
                    index,
                    center + new Vector3(
                        Mathf.Cos(angle) * radius,
                        0f,
                        Mathf.Sin(angle) * radius
                    )
                );
            }

            float width = Mathf.Lerp(ringWidth, ringWidth * 1.55f, sink01);
            ring.startWidth = width;
            ring.endWidth = width;

            Color color = sandRingColor;
            color.a = Mathf.Lerp(0.58f, 0.98f, sink01);
            ring.startColor = color;
            ring.endColor = color;
        }

        private void SetRingVisible(bool visible)
        {
            if (ring != null)
                ring.enabled = visible;
        }
    }
}
