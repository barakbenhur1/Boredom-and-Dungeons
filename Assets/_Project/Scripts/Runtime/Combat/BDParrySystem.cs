using System.Collections.Generic;
using UnityEngine;

namespace BoredomAndDungeons
{
    public static class BDParrySystem
    {
        private enum ParryPhase
        {
            None,
            Anticipation,
            Frozen,
            Recovery
        }

        private sealed class Runner : MonoBehaviour
        {
            private GUIStyle titleStyle;
            private GUIStyle subtitleStyle;
            private Texture2D whiteTexture;

            private void Update() => Tick();
            private void OnGUI() => DrawOverlay(ref titleStyle, ref subtitleStyle, ref whiteTexture);
            private void OnDestroy()
            {
                if (whiteTexture != null)
                    Object.Destroy(whiteTexture);
            }
        }

        private struct RigidbodyState
        {
            public Rigidbody Body;
            public Vector3 Velocity;
            public Vector3 AngularVelocity;
            public bool WasKinematic;
        }

        private struct AnimatorState
        {
            public Animator Animator;
            public float Speed;
        }

        private static readonly List<MonoBehaviour> DisabledBehaviours = new List<MonoBehaviour>(256);
        private static readonly List<RigidbodyState> FrozenRigidbodies = new List<RigidbodyState>(64);
        private static readonly List<AnimatorState> FrozenAnimators = new List<AnimatorState>(64);
        private static readonly List<ParticleSystem> PausedParticles = new List<ParticleSystem>(64);

        private const float AnticipationDuration = 0.085f;
        private const float RecoveryDuration = 0.24f;

        private static Runner runner;
        private static Transform activePlayer;
        private static Transform activeAttacker;
        private static ParryPhase phase;
        private static float effectStartedAtUnscaled;
        private static float anticipationEndsAtUnscaled;
        private static float freezeStartedAtUnscaled;
        private static float freezeEndsAtUnscaled;
        private static float recoveryStartedAtUnscaled;
        private static float recoveryEndsAtUnscaled;
        private static float flashEndsAtUnscaled;
        private static float freezeDuration;
        private static bool heavyParry;
        private static bool active;

        public static bool IsActive => active;
        public static float Remaining => active ? Mathf.Max(0f, recoveryEndsAtUnscaled - Time.unscaledTime) : 0f;
        public static float Progress01 => !active || recoveryEndsAtUnscaled <= effectStartedAtUnscaled
            ? 1f
            : Mathf.Clamp01((Time.unscaledTime - effectStartedAtUnscaled) / (recoveryEndsAtUnscaled - effectStartedAtUnscaled));

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void Bootstrap()
        {
            EnsureRunner();
            Reset();
        }

        public static void Trigger(Transform player, Transform attacker, float duration, bool heavy)
        {
            if (!Application.isPlaying || player == null)
                return;

            EnsureRunner();
            RestoreFrozenWorld();
            BDMeleeSlashArcVisual.ClearAllActive();
            BDPlayerAirborneAttackAnimation.CancelAllActive();

            activePlayer = player;
            activeAttacker = attacker;
            heavyParry = heavy;
            freezeDuration = Mathf.Max(0.10f, duration);
            effectStartedAtUnscaled = Time.unscaledTime;
            anticipationEndsAtUnscaled = effectStartedAtUnscaled + AnticipationDuration;
            freezeStartedAtUnscaled = anticipationEndsAtUnscaled;
            freezeEndsAtUnscaled = freezeStartedAtUnscaled + freezeDuration;
            recoveryStartedAtUnscaled = freezeEndsAtUnscaled;
            recoveryEndsAtUnscaled = recoveryStartedAtUnscaled + RecoveryDuration;
            flashEndsAtUnscaled = effectStartedAtUnscaled + 0.18f;
            phase = ParryPhase.Anticipation;
            active = true;

            // BD PROFESSIONAL PARRY ANTICIPATION FREEZE RECOVERY V23R10
            SpawnParryBurst(player, heavy, release: false);
            BDGameFeelAudio.PlayParryCue();

            BDPlayerParryState state = player.GetComponent<BDPlayerParryState>();
            BDParryTimeRingVisual.Spawn(
                player,
                AnticipationDuration + freezeDuration + RecoveryDuration,
                heavy,
                state != null && state.HasExtendedFreeze
            );

            BDGameFeelEvents.RequestCameraShake(heavy ? 0.30f : 0.22f, 0.11f);
        }

        public static void Reset()
        {
            RestoreFrozenWorld();
            BDMeleeSlashArcVisual.ClearAllActive();
            BDPlayerAirborneAttackAnimation.CancelAllActive();
            active = false;
            phase = ParryPhase.None;
            activePlayer = null;
            activeAttacker = null;
            effectStartedAtUnscaled = 0f;
            anticipationEndsAtUnscaled = 0f;
            freezeStartedAtUnscaled = 0f;
            freezeEndsAtUnscaled = 0f;
            recoveryStartedAtUnscaled = 0f;
            recoveryEndsAtUnscaled = 0f;
            flashEndsAtUnscaled = 0f;
            freezeDuration = 0f;
            heavyParry = false;
        }

        private static void Tick()
        {
            if (!active)
                return;

            float now = Time.unscaledTime;

            if (phase == ParryPhase.Anticipation && now >= anticipationEndsAtUnscaled)
            {
                FreezeWorldExceptPlayer(activePlayer);
                phase = ParryPhase.Frozen;
                BDGameFeelAudio.PlayParryLock();
                BDGameFeelEvents.RequestCameraShake(heavyParry ? 0.40f : 0.28f, 0.16f);
                return;
            }

            if (phase == ParryPhase.Frozen && now >= freezeEndsAtUnscaled)
            {
                phase = ParryPhase.Recovery;
                recoveryStartedAtUnscaled = now;
                recoveryEndsAtUnscaled = now + RecoveryDuration;
                SpawnParryBurst(activePlayer, heavyParry, release: true);
                BDGameFeelAudio.PlayParryRelease();
                return;
            }

            if (phase == ParryPhase.Recovery)
            {
                float t = Mathf.InverseLerp(recoveryStartedAtUnscaled, recoveryEndsAtUnscaled, now);
                UpdateAnimatorRecovery(Mathf.SmoothStep(0f, 1f, t));

                if (now < recoveryEndsAtUnscaled)
                    return;

                RestoreFrozenWorld();
                active = false;
                phase = ParryPhase.None;
                activePlayer = null;
                activeAttacker = null;
            }
        }

        private static void FreezeWorldExceptPlayer(Transform player)
        {
            DisabledBehaviours.Clear();
            FrozenRigidbodies.Clear();
            FrozenAnimators.Clear();
            PausedParticles.Clear();

            MonoBehaviour[] behaviours = Object.FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None);
            for (int i = 0; i < behaviours.Length; i++)
            {
                MonoBehaviour behaviour = behaviours[i];
                if (behaviour == null || !behaviour.enabled)
                    continue;
                if (ShouldRemainActive(behaviour.transform, player, behaviour))
                    continue;
                behaviour.enabled = false;
                DisabledBehaviours.Add(behaviour);
            }

            Rigidbody[] bodies = Object.FindObjectsByType<Rigidbody>(FindObjectsSortMode.None);
            for (int i = 0; i < bodies.Length; i++)
            {
                Rigidbody body = bodies[i];
                if (body == null || IsInHierarchy(body.transform, player))
                    continue;
                FrozenRigidbodies.Add(new RigidbodyState
                {
                    Body = body,
                    Velocity = body.linearVelocity,
                    AngularVelocity = body.angularVelocity,
                    WasKinematic = body.isKinematic
                });
                body.linearVelocity = Vector3.zero;
                body.angularVelocity = Vector3.zero;
                body.isKinematic = true;
            }

            Animator[] animators = Object.FindObjectsByType<Animator>(FindObjectsSortMode.None);
            for (int i = 0; i < animators.Length; i++)
            {
                Animator animator = animators[i];
                if (animator == null || IsInHierarchy(animator.transform, player))
                    continue;
                FrozenAnimators.Add(new AnimatorState { Animator = animator, Speed = animator.speed });
                animator.speed = 0f;
            }

            ParticleSystem[] particles = Object.FindObjectsByType<ParticleSystem>(FindObjectsSortMode.None);
            for (int i = 0; i < particles.Length; i++)
            {
                ParticleSystem particle = particles[i];
                if (particle == null || IsInHierarchy(particle.transform, player) || !particle.isPlaying)
                    continue;
                particle.Pause(withChildren: true);
                PausedParticles.Add(particle);
            }
        }

        private static void UpdateAnimatorRecovery(float blend)
        {
            for (int i = 0; i < FrozenAnimators.Count; i++)
            {
                AnimatorState state = FrozenAnimators[i];
                if (state.Animator != null)
                    state.Animator.speed = state.Speed * blend;
            }
        }

        private static void RestoreFrozenWorld()
        {
            for (int i = 0; i < DisabledBehaviours.Count; i++)
            {
                MonoBehaviour behaviour = DisabledBehaviours[i];
                if (behaviour != null)
                    behaviour.enabled = true;
            }
            DisabledBehaviours.Clear();

            for (int i = 0; i < FrozenRigidbodies.Count; i++)
            {
                RigidbodyState state = FrozenRigidbodies[i];
                if (state.Body == null)
                    continue;
                state.Body.isKinematic = state.WasKinematic;
                if (!state.WasKinematic)
                {
                    state.Body.linearVelocity = state.Velocity;
                    state.Body.angularVelocity = state.AngularVelocity;
                }
            }
            FrozenRigidbodies.Clear();

            for (int i = 0; i < FrozenAnimators.Count; i++)
            {
                AnimatorState state = FrozenAnimators[i];
                if (state.Animator != null)
                    state.Animator.speed = state.Speed;
            }
            FrozenAnimators.Clear();

            for (int i = 0; i < PausedParticles.Count; i++)
            {
                ParticleSystem particle = PausedParticles[i];
                if (particle != null)
                    particle.Play(withChildren: true);
            }
            PausedParticles.Clear();
        }

        private static bool ShouldRemainActive(Transform candidate, Transform player, MonoBehaviour behaviour)
        {
            if (behaviour == runner)
                return true;
            if (IsInHierarchy(candidate, player))
                return true;

            Camera[] cameras = Camera.allCameras;
            for (int i = 0; i < cameras.Length; i++)
            {
                Camera camera = cameras[i];
                if (camera != null && IsInHierarchy(candidate, camera.transform))
                    return true;
            }
            return false;
        }

        private static bool IsInHierarchy(Transform candidate, Transform root)
        {
            if (candidate == null || root == null)
                return false;
            return candidate == root || candidate.IsChildOf(root) || root.IsChildOf(candidate);
        }

        private static void SpawnParryBurst(Transform player, bool heavy, bool release)
        {
            if (player == null)
                return;

            GameObject root = new GameObject(release ? "BD_Parry_Release_Burst" : "BD_Parry_Anticipation_Burst");
            root.transform.SetParent(player, false);
            root.transform.localPosition = Vector3.up * 0.95f;

            int count = heavy ? 20 : 16;
            float radius = release ? (heavy ? 1.55f : 1.25f) : (heavy ? 1.10f : 0.88f);
            Color color = heavy ? new Color(1f, 0.68f, 0.12f, 1f) : new Color(0.35f, 0.92f, 1f, 1f);

            for (int i = 0; i < count; i++)
            {
                float angle = i * (360f / count);
                Vector3 direction = Quaternion.Euler(0f, angle, 0f) * Vector3.forward;
                GameObject mote = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                mote.name = "BD_Parry_Mote";
                mote.transform.SetParent(root.transform, false);
                mote.transform.localPosition = direction * radius;
                mote.transform.localScale = Vector3.one * (heavy ? 0.15f : 0.11f);
                Collider collider = mote.GetComponent<Collider>();
                if (collider != null) Object.Destroy(collider);
                Renderer renderer = mote.GetComponent<Renderer>();
                if (renderer != null)
                {
                    Material material = renderer.material;
                    material.color = color;
                    if (material.HasProperty("_BaseColor")) material.SetColor("_BaseColor", color);
                    if (material.HasProperty("_EmissionColor"))
                    {
                        material.EnableKeyword("_EMISSION");
                        material.SetColor("_EmissionColor", color * (release ? 4.2f : 3.2f));
                    }
                }
            }

            BDParryBurstLifetime life = root.AddComponent<BDParryBurstLifetime>();
            life.Configure(release ? 0.36f : 0.24f, release ? 2.25f : 1.75f);
        }

        private static void DrawOverlay(ref GUIStyle titleStyle, ref GUIStyle subtitleStyle, ref Texture2D whiteTexture)
        {
            if (!active || !BDGameplayUiVisibility.IsGameplayHudVisible)
                return;

            if (whiteTexture == null)
            {
                whiteTexture = new Texture2D(1, 1, TextureFormat.RGBA32, false);
                whiteTexture.SetPixel(0, 0, Color.white);
                whiteTexture.Apply();
            }

            if (titleStyle == null)
            {
                titleStyle = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter, fontSize = 38, fontStyle = FontStyle.Bold };
                titleStyle.normal.textColor = Color.white;
            }
            if (subtitleStyle == null)
            {
                subtitleStyle = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter, fontSize = 15, fontStyle = FontStyle.Bold };
                subtitleStyle.normal.textColor = new Color(0.78f, 0.96f, 1f, 1f);
            }

            float recoveryFade = phase == ParryPhase.Recovery
                ? 1f - Mathf.SmoothStep(0f, 1f, Mathf.InverseLerp(recoveryStartedAtUnscaled, recoveryEndsAtUnscaled, Time.unscaledTime))
                : 1f;
            float anticipationPulse = phase == ParryPhase.Anticipation
                ? Mathf.SmoothStep(0f, 1f, Mathf.InverseLerp(effectStartedAtUnscaled, anticipationEndsAtUnscaled, Time.unscaledTime))
                : 1f;

            Color previous = GUI.color;
            GUI.color = new Color(0.035f, 0.11f, 0.19f, 0.18f * recoveryFade);
            GUI.DrawTexture(new Rect(0f, 0f, Screen.width, Screen.height), whiteTexture);

            float border = Mathf.Lerp(14f, 7f, anticipationPulse);
            Color accent = heavyParry ? new Color(1f, 0.67f, 0.12f, 0.95f) : new Color(0.28f, 0.90f, 1f, 0.95f);
            accent.a *= recoveryFade;
            GUI.color = accent;
            GUI.DrawTexture(new Rect(0f, 0f, Screen.width, border), whiteTexture);
            GUI.DrawTexture(new Rect(0f, Screen.height - border, Screen.width, border), whiteTexture);
            GUI.DrawTexture(new Rect(0f, 0f, border, Screen.height), whiteTexture);
            GUI.DrawTexture(new Rect(Screen.width - border, 0f, border, Screen.height), whiteTexture);

            GUI.color = new Color(1f, 1f, 1f, recoveryFade);
            string title = phase == ParryPhase.Anticipation ? "PERFECT PARRY" : phase == ParryPhase.Recovery ? "TIME RETURNS" : "PARRY";
            string subtitle = phase == ParryPhase.Anticipation ? "TIME BREAK" : phase == ParryPhase.Recovery ? "MOMENTUM RESTORED" : $"FROZEN MOMENT  {Mathf.Max(0f, freezeEndsAtUnscaled - Time.unscaledTime):0.0}s";
            GUI.Label(new Rect(0f, Screen.height * 0.12f, Screen.width, 48f), title, titleStyle);
            GUI.Label(new Rect(0f, Screen.height * 0.12f + 44f, Screen.width, 28f), subtitle, subtitleStyle);

            if (Time.unscaledTime < flashEndsAtUnscaled)
            {
                float alpha = Mathf.Clamp01((flashEndsAtUnscaled - Time.unscaledTime) / 0.18f);
                GUI.color = new Color(accent.r, accent.g, accent.b, alpha * 0.35f);
                GUI.DrawTexture(new Rect(0f, 0f, Screen.width, Screen.height), whiteTexture);
            }

            GUI.color = previous;
        }

        private static void EnsureRunner()
        {
            if (!Application.isPlaying || runner != null)
                return;
            GameObject go = new GameObject("BD_Parry_System");
            Object.DontDestroyOnLoad(go);
            runner = go.AddComponent<Runner>();
        }
    }

    [DisallowMultipleComponent]
    public sealed class BDParryBurstLifetime : MonoBehaviour
    {
        [SerializeField] private float lifetime = 0.28f;
        [SerializeField] private float expansion = 1.8f;
        private float elapsed;

        public void Configure(float newLifetime, float newExpansion)
        {
            lifetime = Mathf.Max(0.05f, newLifetime);
            expansion = Mathf.Max(1f, newExpansion);
        }

        private void Update()
        {
            elapsed += Time.unscaledDeltaTime;
            float t = Mathf.Clamp01(elapsed / Mathf.Max(0.01f, lifetime));
            transform.localScale = Vector3.one * Mathf.Lerp(0.7f, expansion, Mathf.SmoothStep(0f, 1f, t));
            if (elapsed >= lifetime)
                Destroy(gameObject);
        }
    }
}
