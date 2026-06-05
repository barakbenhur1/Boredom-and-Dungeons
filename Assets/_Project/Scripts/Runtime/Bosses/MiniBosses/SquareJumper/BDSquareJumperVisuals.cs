using System.Collections.Generic;
using UnityEngine;

namespace BoredomAndDungeons
{
    public static class BDSquareJumperVisuals
    {
        public static void SpawnGroundTelegraph(
            Vector3 center,
            float radius,
            Color color,
            float duration)
        {
            GameObject root =
                new GameObject(
                    "BD_SquareJumper_GroundTelegraph"
                );

            root.transform.position =
                center + Vector3.up * 0.035f;

            LineRenderer line =
                root.AddComponent<LineRenderer>();

            int points = 52;
            line.loop = true;
            line.positionCount = points;
            line.useWorldSpace = false;
            line.widthMultiplier = 0.075f;
            line.material = CreateMaterial(
                color,
                emission: 2.4f
            );

            for (int i = 0; i < points; i++)
            {
                float angle =
                    i * Mathf.PI * 2f / points;

                line.SetPosition(
                    i,
                    new Vector3(
                        Mathf.Cos(angle) * radius,
                        0f,
                        Mathf.Sin(angle) * radius
                    )
                );
            }

            BDSquareJumperTimedVisual timed =
                root.AddComponent<
                    BDSquareJumperTimedVisual
                >();

            timed.Configure(
                Mathf.Max(0.05f, duration),
                Vector3.one * 0.18f,
                Vector3.one,
                newDestroyAtEnd: true
            );
        }

        public static void SpawnChargePulse(
            Vector3 center,
            Color color,
            float duration)
        {
            SpawnRingMotes(
                "BD_SquareJumper_Charge",
                center + Vector3.up * 0.3f,
                14,
                1.4f,
                0.10f,
                color,
                duration,
                0.55f,
                1.15f
            );
        }

        public static void SpawnLandingImpact(
            Vector3 center,
            float radius,
            Color color)
        {
            SpawnRingMotes(
                "BD_SquareJumper_LandingImpact",
                center + Vector3.up * 0.08f,
                28,
                radius,
                0.16f,
                color,
                0.42f,
                0.65f,
                1.45f
            );
        }

        public static void SpawnBulletBurst(
            Vector3 center,
            Color color)
        {
            SpawnRingMotes(
                "BD_SquareJumper_BulletBurst",
                center,
                12,
                0.75f,
                0.08f,
                color,
                0.24f,
                0.5f,
                1.2f
            );
        }

        public static void SpawnSummonPulse(
            Vector3 center,
            Color color,
            float duration)
        {
            SpawnRingMotes(
                "BD_SquareJumper_SummonPulse",
                center + Vector3.up * 0.1f,
                20,
                2.3f,
                0.12f,
                Color.Lerp(
                    color,
                    new Color(0.55f, 0.25f, 1f, 1f),
                    0.55f
                ),
                duration,
                0.35f,
                1.2f
            );
        }

        public static void SpawnEnrageBurst(
            Vector3 center,
            Color color)
        {
            SpawnRingMotes(
                "BD_SquareJumper_Enrage",
                center + Vector3.up * 0.6f,
                36,
                2.4f,
                0.18f,
                color,
                0.55f,
                0.45f,
                1.8f
            );
        }

        public static void SpawnSwordImpact(
            Vector3 leftCenter,
            Vector3 rightCenter)
        {
            Color color =
                new Color(0.92f, 0.88f, 0.70f, 1f);

            SpawnRingMotes(
                "BD_SquareJumper_LeftSwordImpact",
                leftCenter,
                10,
                0.85f,
                0.09f,
                color,
                0.22f,
                0.5f,
                1.3f
            );

            SpawnRingMotes(
                "BD_SquareJumper_RightSwordImpact",
                rightCenter,
                10,
                0.85f,
                0.09f,
                color,
                0.22f,
                0.5f,
                1.3f
            );
        }

        public static void SpawnProjectileImpact(
            Vector3 center,
            float scale)
        {
            SpawnRingMotes(
                "BD_SquareJumper_ProjectileImpact",
                center,
                8,
                Mathf.Max(0.35f, scale * 1.2f),
                Mathf.Max(0.05f, scale * 0.14f),
                new Color(1f, 0.52f, 0.08f, 1f),
                0.18f,
                0.45f,
                1.25f
            );
        }

        public static void SpawnDeathBurst(
            Vector3 center,
            Color color)
        {
            GameObject root =
                new GameObject(
                    "BD_SquareJumper_DeathBurst"
                );

            root.transform.position = center;

            for (int i = 0; i < 30; i++)
            {
                float angle =
                    i * (360f / 30f);

                Vector3 direction =
                    Quaternion.Euler(0f, angle, 0f) *
                    Vector3.forward;

                GameObject fragment =
                    GameObject.CreatePrimitive(
                        PrimitiveType.Cube
                    );

                fragment.name =
                    "BD_SquareJumper_DeathFragment";

                fragment.transform.SetParent(
                    root.transform,
                    false
                );

                fragment.transform.localPosition =
                    direction *
                    Random.Range(0.8f, 2.4f) +
                    Vector3.up *
                    Random.Range(0.1f, 1.6f);

                fragment.transform.localRotation =
                    Random.rotation;

                fragment.transform.localScale =
                    Vector3.one *
                    Random.Range(0.08f, 0.24f);

                SetVisualOnly(
                    fragment,
                    color,
                    emission: 2.2f
                );
            }

            BDSquareJumperTimedVisual timed =
                root.AddComponent<
                    BDSquareJumperTimedVisual
                >();

            timed.Configure(
                0.75f,
                Vector3.one * 0.65f,
                Vector3.one * 1.45f,
                newDestroyAtEnd: true
            );
        }

        private static void SpawnRingMotes(
            string objectName,
            Vector3 center,
            int count,
            float radius,
            float moteScale,
            Color color,
            float duration,
            float startScale,
            float endScale)
        {
            GameObject root =
                new GameObject(objectName);

            root.transform.position = center;

            int safeCount = Mathf.Max(3, count);

            for (int i = 0; i < safeCount; i++)
            {
                float angle =
                    i * (360f / safeCount);

                GameObject mote =
                    GameObject.CreatePrimitive(
                        PrimitiveType.Sphere
                    );

                mote.name = objectName + "_Mote";
                mote.transform.SetParent(
                    root.transform,
                    false
                );

                mote.transform.localPosition =
                    Quaternion.Euler(0f, angle, 0f) *
                    Vector3.forward *
                    radius;

                mote.transform.localScale =
                    Vector3.one *
                    Mathf.Max(0.02f, moteScale);

                SetVisualOnly(
                    mote,
                    color,
                    emission: 2.6f
                );
            }

            BDSquareJumperTimedVisual timed =
                root.AddComponent<
                    BDSquareJumperTimedVisual
                >();

            timed.Configure(
                Mathf.Max(0.05f, duration),
                Vector3.one * startScale,
                Vector3.one * endScale,
                newDestroyAtEnd: true
            );
        }

        private static void SetVisualOnly(
            GameObject visual,
            Color color,
            float emission)
        {
            Collider collider =
                visual.GetComponent<Collider>();

            if (collider != null)
                Object.Destroy(collider);

            Renderer renderer =
                visual.GetComponent<Renderer>();

            if (renderer == null)
                return;

            renderer.material =
                CreateMaterial(color, emission);
        }

        private static Material CreateMaterial(
            Color color,
            float emission)
        {
            Shader shader =
                Shader.Find(
                    "Universal Render Pipeline/Lit"
                );

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
    public sealed class BDSquareJumperTimedVisual :
        MonoBehaviour
    {
        private float duration = 0.3f;
        private Vector3 startScale = Vector3.one;
        private Vector3 endScale = Vector3.one;
        private bool destroyAtEnd = true;
        private float elapsed;

        public void Configure(
            float newDuration,
            Vector3 newStartScale,
            Vector3 newEndScale,
            bool newDestroyAtEnd)
        {
            duration = Mathf.Max(0.02f, newDuration);
            startScale = newStartScale;
            endScale = newEndScale;
            destroyAtEnd = newDestroyAtEnd;
            elapsed = 0f;
            transform.localScale = startScale;
        }

        private void Update()
        {
            elapsed += Time.unscaledDeltaTime;

            float t = Mathf.Clamp01(
                elapsed / Mathf.Max(0.01f, duration)
            );

            float eased =
                1f - Mathf.Pow(1f - t, 3f);

            transform.localScale =
                Vector3.Lerp(
                    startScale,
                    endScale,
                    eased
                );

            if (elapsed >= duration &&
                destroyAtEnd)
            {
                Destroy(gameObject);
            }
        }
    }
}
