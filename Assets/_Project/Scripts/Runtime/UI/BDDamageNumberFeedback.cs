using System.Collections.Generic;
using UnityEngine;

namespace BoredomAndDungeons
{
    // BD ANIMATED PLAYER/ENEMY DAMAGE NUMBERS V23R14
    public sealed class BDDamageNumberFeedback : MonoBehaviour
    {
        private const float Lifetime = 0.96f;
        private const float RiseDistance = 1.05f;
        private const float HorizontalDrift = 0.16f;
        private const float StackHeight = 0.18f;

        private static readonly Color PlayerDamageColor =
            new Color(1.00f, 0.27f, 0.30f, 1f);
        private static readonly Color EnemyDamageColor =
            new Color(1.00f, 0.73f, 0.22f, 1f);
        private static readonly Color CriticalDamageColor =
            new Color(1.00f, 0.20f, 0.72f, 1f);
        private static readonly Color ShadowColor =
            new Color(0.02f, 0.025f, 0.04f, 0.88f);

        private static readonly Dictionary<int, int> ActiveStacks =
            new Dictionary<int, int>();

        private TextMesh mainText;
        private TextMesh shadowText;
        private Color mainColor;
        private Vector3 startPosition;
        private Vector3 driftDirection;
        private float age;
        private int ownerId;
        private Camera cachedCamera;
        private bool initialized;

        public static void Spawn(
            BDHealth owner,
            float appliedDamage,
            bool critical = false)
        {
            if (!Application.isPlaying ||
                owner == null ||
                appliedDamage <= 0.001f)
            {
                return;
            }

            if (owner.GetComponent<BDHorseHealth>() != null)
                return;

            bool isPlayer = owner.GetComponent<BDPlayerMarker>() != null;
            bool isEnemy = !isPlayer;
            if (!isPlayer && !isEnemy)
                return;

            GameObject root = new GameObject(
                isPlayer
                    ? "BD_Player_Damage_Number"
                    : "BD_Enemy_Damage_Number"
            );

            BDDamageNumberFeedback feedback =
                root.AddComponent<BDDamageNumberFeedback>();
            feedback.Initialize(
                owner,
                appliedDamage,
                isPlayer,
                critical
            );
        }

        private void Initialize(
            BDHealth owner,
            float appliedDamage,
            bool isPlayer,
            bool critical)
        {
            ownerId = owner.GetInstanceID();
            int stackIndex = ResolveStackIndex(ownerId);

            cachedCamera = Camera.main;
            startPosition =
                ResolveAnchorPosition(owner) +
                Vector3.up * (stackIndex * StackHeight);

            Vector2 random = Random.insideUnitCircle;
            driftDirection = new Vector3(
                random.x,
                0f,
                random.y
            ).normalized * HorizontalDrift;

            transform.position = startPosition;
            transform.localScale = Vector3.one *
                (critical && !isPlayer ? 0.50f : 0.42f);

            mainColor = isPlayer
                ? PlayerDamageColor
                : critical
                    ? CriticalDamageColor
                    : EnemyDamageColor;

            string amount = FormatDamage(appliedDamage);
            mainText = CreateText(
                gameObject,
                amount,
                mainColor,
                0
            );

            GameObject shadow = new GameObject("Shadow");
            shadow.transform.SetParent(transform, false);
            shadow.transform.localPosition =
                new Vector3(0.025f, -0.025f, 0.018f);
            shadowText = CreateText(
                shadow,
                amount,
                ShadowColor,
                -1
            );

            initialized = true;
            UpdateBillboard();
        }

        private void Update()
        {
            if (!initialized)
                return;

            if (!BDGameplayUiVisibility.IsGameplayHudVisible)
            {
                Destroy(gameObject);
                return;
            }

            age += Time.unscaledDeltaTime;
            float t = Mathf.Clamp01(age / Lifetime);
            float rise01 = 1f - Mathf.Pow(1f - t, 3f);

            transform.position =
                startPosition +
                Vector3.up * (RiseDistance * rise01) +
                driftDirection * t;

            float scale;
            if (t < 0.18f)
            {
                float popT = Mathf.Clamp01(t / 0.18f);
                scale = Mathf.LerpUnclamped(
                    0.42f,
                    1.18f,
                    EaseOutBack(popT)
                );
            }
            else
            {
                float settleT = Mathf.InverseLerp(0.18f, 1f, t);
                scale = Mathf.Lerp(
                    1.18f,
                    0.92f,
                    Smooth01(settleT)
                );
            }

            transform.localScale = Vector3.one * scale;

            float alpha = 1f - Smooth01(
                Mathf.InverseLerp(0.54f, 1f, t)
            );
            ApplyAlpha(mainText, mainColor, alpha);
            ApplyAlpha(shadowText, ShadowColor, alpha * 0.90f);
            UpdateBillboard();

            if (age >= Lifetime)
                Destroy(gameObject);
        }

        private void UpdateBillboard()
        {
            if (cachedCamera == null)
                cachedCamera = Camera.main;

            if (cachedCamera == null)
                return;

            Vector3 awayFromCamera =
                transform.position - cachedCamera.transform.position;
            if (awayFromCamera.sqrMagnitude <= 0.0001f)
                return;

            transform.rotation = Quaternion.LookRotation(
                awayFromCamera.normalized,
                cachedCamera.transform.up
            );
        }

        private void OnDestroy()
        {
            if (ownerId == 0)
                return;

            if (!ActiveStacks.TryGetValue(ownerId, out int count))
                return;

            count--;
            if (count <= 0)
                ActiveStacks.Remove(ownerId);
            else
                ActiveStacks[ownerId] = count;
        }

        private static int ResolveStackIndex(int id)
        {
            ActiveStacks.TryGetValue(id, out int count);
            ActiveStacks[id] = count + 1;
            return Mathf.Min(count, 4);
        }

        private static TextMesh CreateText(
            GameObject owner,
            string value,
            Color color,
            int sortingOrder)
        {
            TextMesh text = owner.AddComponent<TextMesh>();
            text.text = value;
            text.anchor = TextAnchor.MiddleCenter;
            text.alignment = TextAlignment.Center;
            text.characterSize = 0.095f;
            text.fontSize = 72;
            text.color = color;

            MeshRenderer renderer =
                owner.GetComponent<MeshRenderer>();
            if (renderer != null)
                renderer.sortingOrder = sortingOrder;

            return text;
        }

        private static Vector3 ResolveAnchorPosition(BDHealth health)
        {
            CharacterController controller =
                health.GetComponent<CharacterController>();
            if (controller == null)
            {
                controller =
                    health.GetComponentInParent<CharacterController>();
            }

            if (controller != null)
            {
                float top =
                    controller.center.y +
                    controller.height * 0.55f +
                    0.22f;
                return controller.transform.TransformPoint(
                    new Vector3(
                        controller.center.x,
                        top,
                        controller.center.z
                    )
                );
            }

            Renderer[] renderers =
                health.GetComponentsInChildren<Renderer>(true);
            bool hasBounds = false;
            Bounds bounds = new Bounds(
                health.transform.position,
                Vector3.one
            );

            for (int i = 0; i < renderers.Length; i++)
            {
                Renderer renderer = renderers[i];
                if (renderer == null ||
                    renderer.GetComponentInParent<BDDamageNumberFeedback>() != null)
                {
                    continue;
                }

                if (!hasBounds)
                {
                    bounds = renderer.bounds;
                    hasBounds = true;
                }
                else
                {
                    bounds.Encapsulate(renderer.bounds);
                }
            }

            return hasBounds
                ? new Vector3(
                    bounds.center.x,
                    bounds.max.y + 0.22f,
                    bounds.center.z
                )
                : health.transform.position + Vector3.up * 1.4f;
        }

        private static string FormatDamage(float damage)
        {
            float rounded = Mathf.Round(damage);
            if (Mathf.Abs(damage - rounded) <= 0.05f)
                return "-" + rounded.ToString("0");

            return "-" + damage.ToString("0.0");
        }

        private static void ApplyAlpha(
            TextMesh text,
            Color baseColor,
            float alpha)
        {
            if (text == null)
                return;

            Color color = baseColor;
            color.a = Mathf.Clamp01(alpha);
            text.color = color;
        }

        private static float Smooth01(float value)
        {
            value = Mathf.Clamp01(value);
            return value * value * (3f - 2f * value);
        }

        private static float EaseOutBack(float value)
        {
            value = Mathf.Clamp01(value) - 1f;
            const float overshoot = 1.70158f;
            return 1f +
                value * value *
                ((overshoot + 1f) * value + overshoot);
        }
    }
}
