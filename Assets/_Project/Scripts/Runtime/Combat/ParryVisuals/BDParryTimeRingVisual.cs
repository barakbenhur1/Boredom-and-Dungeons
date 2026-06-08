using UnityEngine;

namespace BoredomAndDungeons
{
    [DisallowMultipleComponent]
    public sealed class BDParryTimeRingVisual : MonoBehaviour
    {
        private Transform followTarget;
        private Transform ringRoot;
        private LineRenderer outerRing;
        private LineRenderer remainingRing;
        private Light ringLight;

        private float startedAt;
        private float duration = 1.5f;
        private bool upgraded;
        private Color baseColor;
        private Color upgradeColor;

        public static void Spawn(
            Transform target,
            float freezeDuration,
            bool heavy,
            bool hasUpgrade)
        {
            if (target == null)
                return;

            GameObject root = new GameObject(
                hasUpgrade
                    ? "BD_Parry_Upgraded_Time_Ring"
                    : "BD_Parry_Time_Ring"
            );

            root.transform.SetParent(target, false);
            root.transform.localPosition = Vector3.zero;
            root.transform.localRotation = Quaternion.identity;

            BDParryTimeRingVisual visual =
                root.AddComponent<BDParryTimeRingVisual>();

            visual.Configure(
                target,
                freezeDuration,
                heavy,
                hasUpgrade
            );
        }

        private void Configure(
            Transform target,
            float freezeDuration,
            bool heavy,
            bool hasUpgrade)
        {
            followTarget = target;
            duration = Mathf.Max(0.10f, freezeDuration);
            startedAt = Time.unscaledTime;
            upgraded = hasUpgrade;

            baseColor = heavy
                ? new Color(1f, 0.62f, 0.12f, 1f)
                : new Color(0.36f, 0.92f, 1f, 1f);

            upgradeColor = new Color(1f, 0.86f, 0.22f, 1f);

            BuildVisual();
            UpdatePose();
            UpdateVisual();
        }

        private void LateUpdate()
        {
            if (followTarget == null)
            {
                Destroy(gameObject);
                return;
            }

            UpdatePose();
            UpdateVisual();

            if (Time.unscaledTime - startedAt >= duration)
                Destroy(gameObject);
        }

        private void UpdatePose()
        {
            transform.localPosition = Vector3.up * 0.045f;
            transform.localRotation = Quaternion.identity;
        }

        private void BuildVisual()
        {
            ringRoot = new GameObject("RingRoot").transform;
            ringRoot.SetParent(transform, false);

            outerRing = CreateRing(
                "Frozen_Time_Outer_Ring",
                radius: upgraded ? 1.75f : 1.48f,
                width: upgraded ? 0.055f : 0.045f,
                color: upgraded ? upgradeColor : baseColor,
                loop: true
            );

            remainingRing = CreateRing(
                "Frozen_Time_Remaining_Ring",
                radius: upgraded ? 1.45f : 1.24f,
                width: upgraded ? 0.080f : 0.065f,
                color: baseColor,
                loop: false
            );

            GameObject lightObject = new GameObject("TimeRingLight");
            lightObject.transform.SetParent(transform, false);
            lightObject.transform.localPosition = Vector3.up * 0.45f;

            ringLight = lightObject.AddComponent<Light>();
            ringLight.type = LightType.Point;
            ringLight.color = upgraded ? upgradeColor : baseColor;
            ringLight.range = upgraded ? 4.2f : 3.2f;
            ringLight.intensity = upgraded ? 2.4f : 1.7f;

            SpawnFlashRing();
        }

        private LineRenderer CreateRing(
            string objectName,
            float radius,
            float width,
            Color color,
            bool loop)
        {
            GameObject ringObject = new GameObject(objectName);
            ringObject.transform.SetParent(ringRoot, false);

            LineRenderer line =
                ringObject.AddComponent<LineRenderer>();

            line.useWorldSpace = false;
            line.loop = loop;
            line.positionCount = loop ? 72 : 73;
            line.widthMultiplier = width;
            line.material = CreateMaterial(color, 3.4f);

            int count = line.positionCount;
            for (int i = 0; i < count; i++)
            {
                float t = i / (float)(count - 1);
                float angle = t * Mathf.PI * 2f;

                line.SetPosition(
                    i,
                    new Vector3(
                        Mathf.Cos(angle) * radius,
                        0f,
                        Mathf.Sin(angle) * radius
                    )
                );
            }

            return line;
        }

        private void UpdateVisual()
        {
            float elapsed = Time.unscaledTime - startedAt;
            float progress01 = Mathf.Clamp01(elapsed / duration);
            float remaining01 = 1f - progress01;
            float releaseFade = 1f - Mathf.SmoothStep(0f, 1f, Mathf.InverseLerp(0.78f, 1f, progress01));

            if (ringRoot != null)
            {
                ringRoot.Rotate(
                    Vector3.up,
                    (upgraded ? 80f : 55f) * Time.unscaledDeltaTime,
                    Space.World
                );
            }

            if (remainingRing != null)
            {
                int segments = Mathf.Max(
                    3,
                    Mathf.RoundToInt(72 * remaining01)
                );

                remainingRing.positionCount = segments + 1;

                float radius = upgraded ? 1.45f : 1.24f;
                for (int i = 0; i <= segments; i++)
                {
                    float t = i / Mathf.Max(1f, segments);
                    float angle = t * Mathf.PI * 2f * remaining01;

                    remainingRing.SetPosition(
                        i,
                        new Vector3(
                            Mathf.Cos(angle) * radius,
                            0f,
                            Mathf.Sin(angle) * radius
                        )
                    );
                }
            }

            if (outerRing != null)
            {
                float pulse =
                    0.85f +
                    Mathf.Sin(Time.unscaledTime * 13f) * 0.15f;

                outerRing.widthMultiplier =
                    (upgraded ? 0.055f : 0.045f) * pulse * Mathf.Lerp(0.35f, 1f, releaseFade);
                Color faded = upgraded ? upgradeColor : baseColor;
                faded.a *= releaseFade;
                outerRing.startColor = faded;
                outerRing.endColor = faded;
            }

            if (remainingRing != null)
            {
                Color faded = baseColor;
                faded.a *= releaseFade;
                remainingRing.startColor = faded;
                remainingRing.endColor = faded;
            }

            if (ringLight != null)
            {
                ringLight.intensity =
                    (upgraded ? 2.4f : 1.7f) *
                    Mathf.Lerp(0.20f, 1f, remaining01) *
                    releaseFade;
            }
        }

        private void SpawnFlashRing()
        {
            GameObject burst = new GameObject("Parry_Time_Ring_Burst");
            burst.transform.SetParent(transform, false);

            LineRenderer line = burst.AddComponent<LineRenderer>();
            line.useWorldSpace = false;
            line.loop = true;
            line.positionCount = 64;
            line.widthMultiplier = upgraded ? 0.075f : 0.065f;
            line.material = CreateMaterial(
                upgraded ? upgradeColor : baseColor,
                4.5f
            );

            float radius = upgraded ? 2.15f : 1.85f;

            for (int i = 0; i < line.positionCount; i++)
            {
                float angle = i * Mathf.PI * 2f / line.positionCount;

                line.SetPosition(
                    i,
                    new Vector3(
                        Mathf.Cos(angle) * radius,
                        0f,
                        Mathf.Sin(angle) * radius
                    )
                );
            }

            BDParryTimeRingBurst burstLogic =
                burst.AddComponent<BDParryTimeRingBurst>();

            burstLogic.Configure(
                line,
                radius,
                upgraded ? upgradeColor : baseColor
            );
        }

        private static Material CreateMaterial(
            Color color,
            float emission)
        {
            Shader shader =
                Shader.Find("Universal Render Pipeline/Lit");

            if (shader == null)
                shader = Shader.Find("Standard");

            if (shader == null)
                shader = Shader.Find("Unlit/Color");

            if (shader == null)
                shader = Shader.Find("Sprites/Default");

            if (shader == null)
                return null;

            Material material = new Material(shader);
            material.color = color;

            if (material.HasProperty("_BaseColor"))
                material.SetColor("_BaseColor", color);

            if (material.HasProperty("_Color"))
                material.SetColor("_Color", color);

            if (material.HasProperty("_EmissionColor"))
            {
                material.EnableKeyword("_EMISSION");
                material.SetColor(
                    "_EmissionColor",
                    color * Mathf.Max(1f, emission)
                );
            }

            return material;
        }
    }

    [DisallowMultipleComponent]
    public sealed class BDParryTimeRingBurst : MonoBehaviour
    {
        private LineRenderer line;
        private Color color = Color.white;
        private float baseRadius = 1.8f;
        private float elapsed;
        private float lifetime = 0.24f;

        public void Configure(
            LineRenderer newLine,
            float newBaseRadius,
            Color newColor)
        {
            line = newLine;
            baseRadius = Mathf.Max(0.1f, newBaseRadius);
            color = newColor;
        }

        private void Update()
        {
            elapsed += Time.unscaledDeltaTime;

            float t = Mathf.Clamp01(elapsed / lifetime);
            float radius = Mathf.Lerp(baseRadius * 0.35f, baseRadius, t);

            if (line != null)
            {
                for (int i = 0; i < line.positionCount; i++)
                {
                    float angle = i * Mathf.PI * 2f / line.positionCount;

                    line.SetPosition(
                        i,
                        new Vector3(
                            Mathf.Cos(angle) * radius,
                            0f,
                            Mathf.Sin(angle) * radius
                        )
                    );
                }

                Color faded = color;
                faded.a = 1f - t;
                line.startColor = faded;
                line.endColor = faded;
                line.widthMultiplier = Mathf.Lerp(0.12f, 0.01f, t);
            }

            if (elapsed >= lifetime)
                Destroy(gameObject);
        }
    }
}
