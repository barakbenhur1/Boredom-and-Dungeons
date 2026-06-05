using System.Collections.Generic;
using UnityEngine;

namespace BoredomAndDungeons
{
    public static class BDParrySystem
    {
        private sealed class Runner : MonoBehaviour
        {
            private GUIStyle titleStyle;
            private GUIStyle subtitleStyle;
            private Texture2D whiteTexture;

            private void Update()
            {
                Tick();
            }

            private void OnGUI()
            {
                DrawOverlay(ref titleStyle, ref subtitleStyle, ref whiteTexture);
            }

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

        private static Runner runner;
        private static Transform activePlayer;
        private static Transform activeAttacker;
        private static float freezeEndsAtUnscaled;
        private static float flashEndsAtUnscaled;
        private static float freezeStartedAtUnscaled;
        private static float freezeDuration;
        private static bool heavyParry;
        private static bool active;

        public static bool IsActive => active;
        public static float Remaining => active ? Mathf.Max(0f, freezeEndsAtUnscaled - Time.unscaledTime) : 0f;
        public static float Progress01 => !active || freezeDuration <= 0f
            ? 1f
            : Mathf.Clamp01((Time.unscaledTime - freezeStartedAtUnscaled) / freezeDuration);

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

            if (active)
                RestoreFrozenWorld();

            activePlayer = player;
            activeAttacker = attacker;
            heavyParry = heavy;
            freezeDuration = Mathf.Max(0.10f, duration);
            freezeStartedAtUnscaled = Time.unscaledTime;
            freezeEndsAtUnscaled = freezeStartedAtUnscaled + freezeDuration;
            flashEndsAtUnscaled = freezeStartedAtUnscaled + 0.20f;
            active = true;

            SpawnParryBurst(player.position + Vector3.up * 0.95f, heavy);
            FreezeWorldExceptPlayer(player);
            BDGameFeelEvents.RequestCameraShake(heavy ? 0.38f : 0.26f, 0.18f);
        }

        public static void Reset()
        {
            RestoreFrozenWorld();
            active = false;
            activePlayer = null;
            activeAttacker = null;
            freezeEndsAtUnscaled = 0f;
            freezeStartedAtUnscaled = 0f;
            freezeDuration = 0f;
            flashEndsAtUnscaled = 0f;
            heavyParry = false;
        }

        private static void Tick()
        {
            if (!active)
                return;

            if (Time.unscaledTime < freezeEndsAtUnscaled)
                return;

            RestoreFrozenWorld();
            active = false;
            activePlayer = null;
            activeAttacker = null;
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

        private static void SpawnParryBurst(Vector3 position, bool heavy)
        {
            GameObject root = new GameObject("BD_Parry_Burst");
            root.transform.position = position;

            int count = heavy ? 18 : 14;
            float radius = heavy ? 1.2f : 0.95f;
            Color color = heavy
                ? new Color(1f, 0.68f, 0.12f, 1f)
                : new Color(0.35f, 0.92f, 1f, 1f);

            for (int i = 0; i < count; i++)
            {
                float angle = i * (360f / count);
                Vector3 direction = Quaternion.Euler(0f, angle, 0f) * Vector3.forward;
                GameObject mote = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                mote.name = "BD_Parry_Mote";
                mote.transform.SetParent(root.transform, false);
                mote.transform.localPosition = direction * radius;
                mote.transform.localScale = Vector3.one * (heavy ? 0.16f : 0.12f);

                Collider collider = mote.GetComponent<Collider>();
                if (collider != null)
                    Object.Destroy(collider);

                Renderer renderer = mote.GetComponent<Renderer>();
                if (renderer != null)
                {
                    Material material = renderer.material;
                    material.color = color;
                    if (material.HasProperty("_BaseColor")) material.SetColor("_BaseColor", color);
                    if (material.HasProperty("_EmissionColor"))
                    {
                        material.EnableKeyword("_EMISSION");
                        material.SetColor("_EmissionColor", color * 3f);
                    }
                }
            }

            root.AddComponent<BDParryBurstLifetime>();
        }

        private static void DrawOverlay(ref GUIStyle titleStyle, ref GUIStyle subtitleStyle, ref Texture2D whiteTexture)
        {
            bool flashActive = Time.unscaledTime < flashEndsAtUnscaled;
            if (!active && !flashActive)
                return;

            if (whiteTexture == null)
            {
                whiteTexture = new Texture2D(1, 1, TextureFormat.RGBA32, false);
                whiteTexture.SetPixel(0, 0, Color.white);
                whiteTexture.Apply();
            }

            if (titleStyle == null)
            {
                titleStyle = new GUIStyle(GUI.skin.label)
                {
                    alignment = TextAnchor.MiddleCenter,
                    fontSize = 38,
                    fontStyle = FontStyle.Bold
                };
                titleStyle.normal.textColor = Color.white;
            }

            if (subtitleStyle == null)
            {
                subtitleStyle = new GUIStyle(GUI.skin.label)
                {
                    alignment = TextAnchor.MiddleCenter,
                    fontSize = 15,
                    fontStyle = FontStyle.Bold
                };
                subtitleStyle.normal.textColor = new Color(0.78f, 0.96f, 1f, 1f);
            }

            Color previous = GUI.color;

            if (active)
            {
                GUI.color = new Color(0.05f, 0.14f, 0.24f, 0.20f);
                GUI.DrawTexture(new Rect(0f, 0f, Screen.width, Screen.height), whiteTexture);

                float border = 9f;
                GUI.color = new Color(0.28f, 0.90f, 1f, 0.92f);
                GUI.DrawTexture(new Rect(0f, 0f, Screen.width, border), whiteTexture);
                GUI.DrawTexture(new Rect(0f, Screen.height - border, Screen.width, border), whiteTexture);
                GUI.DrawTexture(new Rect(0f, 0f, border, Screen.height), whiteTexture);
                GUI.DrawTexture(new Rect(Screen.width - border, 0f, border, Screen.height), whiteTexture);

                float progressWidth = Mathf.Min(460f, Screen.width * 0.42f);
                Rect progressBack = new Rect((Screen.width - progressWidth) * 0.5f, Screen.height * 0.20f, progressWidth, 10f);
                GUI.color = new Color(0f, 0f, 0f, 0.72f);
                GUI.DrawTexture(progressBack, whiteTexture);
                Rect progressFill = progressBack;
                progressFill.width *= 1f - Progress01;
                GUI.color = new Color(0.28f, 0.90f, 1f, 1f);
                GUI.DrawTexture(progressFill, whiteTexture);

                GUI.color = Color.white;
                GUI.Label(new Rect(0f, Screen.height * 0.12f, Screen.width, 48f), "PARRY", titleStyle);
                GUI.Label(
                    new Rect(0f, Screen.height * 0.12f + 44f, Screen.width, 28f),
                    $"TIME BREAK  {Remaining:0.0}s",
                    subtitleStyle
                );
            }

            if (flashActive)
            {
                float alpha = Mathf.Clamp01((flashEndsAtUnscaled - Time.unscaledTime) / 0.20f);
                GUI.color = new Color(0.70f, 0.96f, 1f, alpha * 0.42f);
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

        private void Update()
        {
            elapsed += Time.unscaledDeltaTime;
            float t = Mathf.Clamp01(elapsed / Mathf.Max(0.01f, lifetime));
            transform.localScale = Vector3.one * Mathf.Lerp(0.7f, expansion, t);

            if (elapsed >= lifetime)
                Destroy(gameObject);
        }
    }
}
