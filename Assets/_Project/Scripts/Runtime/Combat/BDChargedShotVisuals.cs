using UnityEngine;

namespace BoredomAndDungeons
{
    [DisallowMultipleComponent]
    public sealed class BDChargedShotChargeVisual : MonoBehaviour
    {
        private Transform owner;
        private Transform visualRoot;
        private Transform core;
        private Transform innerOrbit;
        private Transform outerOrbit;
        private Light chargeLight;

        private Vector3 aimDirection = Vector3.forward;
        private int ammoCount = 2;
        private float chargeProgress;
        private float requiredDuration;
        private bool released;

        public static BDChargedShotChargeVisual Spawn(
            Transform owner,
            Vector3 aimDirection,
            int ammoCount,
            float requiredDuration)
        {
            GameObject root =
                new GameObject("BD_Charged_Shot_Charge_Visual");

            BDChargedShotChargeVisual visual =
                root.AddComponent<BDChargedShotChargeVisual>();

            visual.owner = owner;
            visual.ammoCount = Mathf.Max(2, ammoCount);
            visual.requiredDuration =
                Mathf.Max(0.05f, requiredDuration);

            visual.SetAim(aimDirection);
            visual.Build();
            visual.UpdateWorldPose();

            return visual;
        }

        public void SetCharge(
            float normalizedProgress,
            Vector3 newAimDirection)
        {
            chargeProgress = Mathf.Clamp01(normalizedProgress);
            SetAim(newAimDirection);
            ApplyVisualState();
        }

        public void ReleaseToProjectile()
        {
            if (released)
                return;

            released = true;
            Destroy(gameObject);
        }

        public void CancelCharge()
        {
            if (released)
                return;

            released = true;
            BDChargedShotVisualUtility.SpawnCancelImplosion(
                transform.position,
                ammoCount,
                chargeProgress
            );
            Destroy(gameObject);
        }

        private void LateUpdate()
        {
            if (released || owner == null)
            {
                if (!released)
                    Destroy(gameObject);

                return;
            }

            UpdateWorldPose();

            float spinMultiplier =
                Mathf.Lerp(0.45f, 2.8f, chargeProgress);

            if (innerOrbit != null)
            {
                innerOrbit.Rotate(
                    Vector3.up,
                    170f * spinMultiplier * Time.deltaTime,
                    Space.Self
                );
            }

            if (outerOrbit != null)
            {
                outerOrbit.Rotate(
                    Vector3.up,
                    -115f * spinMultiplier * Time.deltaTime,
                    Space.Self
                );
            }

            ApplyVisualState();
        }

        private void Build()
        {
            GameObject visualObject = new GameObject("Visual");
            visualRoot = visualObject.transform;
            visualRoot.SetParent(transform, false);

            Color color =
                BDChargedShotVisualUtility.ResolvePowerColor(ammoCount);

            GameObject coreObject =
                GameObject.CreatePrimitive(PrimitiveType.Sphere);

            coreObject.name = "Charge_Core";
            core = coreObject.transform;
            core.SetParent(visualRoot, false);
            core.localScale = Vector3.one * 0.12f;

            BDChargedShotVisualUtility.ConfigureRenderer(
                coreObject,
                color,
                emissionMultiplier: 3.6f
            );

            innerOrbit = BuildOrbit(
                "Inner_Orbit",
                8,
                0.42f,
                0.075f,
                color
            );

            outerOrbit = BuildOrbit(
                "Outer_Orbit",
                12,
                0.66f,
                0.055f,
                Color.Lerp(color, Color.white, 0.42f)
            );

            GameObject lightObject =
                new GameObject("Charge_Light");

            lightObject.transform.SetParent(visualRoot, false);

            chargeLight = lightObject.AddComponent<Light>();
            chargeLight.type = LightType.Point;
            chargeLight.color = color;
            chargeLight.range = 2.4f;
            chargeLight.intensity = 0f;

            ApplyVisualState();
        }

        private Transform BuildOrbit(
            string orbitName,
            int count,
            float radius,
            float moteScale,
            Color color)
        {
            GameObject orbitObject = new GameObject(orbitName);
            Transform orbit = orbitObject.transform;
            orbit.SetParent(visualRoot, false);

            for (int i = 0; i < count; i++)
            {
                float angle = i * (360f / count);

                GameObject mote =
                    GameObject.CreatePrimitive(PrimitiveType.Sphere);

                mote.name = $"{orbitName}_Mote_{i}";
                mote.transform.SetParent(orbit, false);
                mote.transform.localPosition =
                    Quaternion.Euler(0f, angle, 0f) *
                    Vector3.forward *
                    radius;
                mote.transform.localScale =
                    Vector3.one * moteScale;

                BDChargedShotVisualUtility.ConfigureRenderer(
                    mote,
                    color,
                    emissionMultiplier: 2.8f
                );
            }

            return orbit;
        }

        private void SetAim(Vector3 newAimDirection)
        {
            newAimDirection.y = 0f;

            if (newAimDirection.sqrMagnitude > 0.001f)
                aimDirection = newAimDirection.normalized;
        }

        private void UpdateWorldPose()
        {
            if (owner == null)
                return;

            transform.position =
                owner.position +
                aimDirection * 0.92f +
                Vector3.up * 1.18f;

            transform.rotation =
                Quaternion.LookRotation(aimDirection, Vector3.up);
        }

        private void ApplyVisualState()
        {
            float ammoPower = Mathf.Max(0f, ammoCount - 1f);
            float eased =
                chargeProgress * chargeProgress *
                (3f - 2f * chargeProgress);

            float pulse =
                1f +
                Mathf.Sin(
                    Time.unscaledTime *
                    Mathf.Lerp(7f, 18f, chargeProgress)
                ) *
                Mathf.Lerp(0.02f, 0.11f, chargeProgress);

            float coreScale =
                Mathf.Lerp(
                    0.12f,
                    0.46f + ammoPower * 0.075f,
                    eased
                ) *
                pulse;

            if (core != null)
                core.localScale = Vector3.one * coreScale;

            if (innerOrbit != null)
            {
                float scale =
                    Mathf.Lerp(0.35f, 1f + ammoPower * 0.05f, eased);

                innerOrbit.localScale = Vector3.one * scale;
            }

            if (outerOrbit != null)
            {
                float scale =
                    Mathf.Lerp(0.18f, 1f + ammoPower * 0.09f, eased);

                outerOrbit.localScale = Vector3.one * scale;
            }

            if (chargeLight != null)
            {
                chargeLight.intensity =
                    Mathf.Lerp(
                        0f,
                        1.8f + ammoPower * 0.55f,
                        eased
                    );

                chargeLight.range =
                    Mathf.Lerp(
                        1.2f,
                        2.3f + ammoPower * 0.32f,
                        eased
                    );
            }
        }
    }


    [DisallowMultipleComponent]
    public sealed class BDChargedProjectileVisual : MonoBehaviour
    {
        // BD CHARGED PROJECTILE IDEMPOTENT BUILD V20
        private Transform orbitRoot;
        private Transform secondOrbitRoot;
        private TrailRenderer chargedTrail;
        private Vector3 baseScale;
        private int ammoCount;
        private bool configured;

        public static void Attach(GameObject projectile, int ammoCount)
        {
            if (projectile == null)
                return;

            BDChargedProjectileVisual visual =
                projectile.GetComponent<BDChargedProjectileVisual>();

            if (visual == null)
                visual = projectile.AddComponent<BDChargedProjectileVisual>();

            visual.Configure(ammoCount);
        }

        public void Configure(int consumedAmmo)
        {
            ammoCount = Mathf.Max(2, consumedAmmo);

            if (!configured)
                baseScale = transform.localScale;

            BuildOrRefresh();
            configured = true;
        }

        private void Update()
        {
            if (!configured)
                return;

            float power = Mathf.Max(0f, ammoCount - 1f);
            float pulse =
                1f +
                Mathf.Sin(Time.time * (11f + power)) *
                Mathf.Min(0.16f, 0.045f + power * 0.018f);

            transform.localScale = baseScale * pulse;

            if (orbitRoot != null)
            {
                orbitRoot.Rotate(
                    Vector3.forward,
                    (250f + power * 28f) * Time.deltaTime,
                    Space.Self
                );
            }

            if (secondOrbitRoot != null)
            {
                secondOrbitRoot.Rotate(
                    Vector3.forward,
                    -(165f + power * 21f) * Time.deltaTime,
                    Space.Self
                );
            }
        }

        private void OnDestroy()
        {
            if (!configured || !Application.isPlaying)
                return;

            BDChargedShotVisualUtility.SpawnChargedImpactBurst(
                transform.position,
                ammoCount
            );
        }

        private void BuildOrRefresh()
        {
            Color color =
                BDChargedShotVisualUtility.ResolvePowerColor(ammoCount);

            RefreshRenderer(GetComponent<Renderer>(), color, 3.2f + ammoCount * 0.42f);

            orbitRoot = BuildOrRefreshProjectileOrbit(
                "Charged_Projectile_Orbit_A",
                8,
                0.72f,
                0.075f,
                color
            );

            secondOrbitRoot = BuildOrRefreshProjectileOrbit(
                "Charged_Projectile_Orbit_B",
                6,
                0.49f,
                0.055f,
                Color.Lerp(color, Color.white, 0.55f)
            );

            if (secondOrbitRoot != null)
                secondOrbitRoot.localRotation = Quaternion.Euler(90f, 0f, 0f);

            chargedTrail = GetComponent<TrailRenderer>();
            if (chargedTrail == null)
                chargedTrail = gameObject.AddComponent<TrailRenderer>();

            ConfigureTrail(chargedTrail, color);
        }

        private Transform BuildOrRefreshProjectileOrbit(
            string orbitName,
            int count,
            float radius,
            float scale,
            Color color)
        {
            Transform orbit = transform.Find(orbitName);
            if (orbit == null)
            {
                GameObject root = new GameObject(orbitName);
                orbit = root.transform;
                orbit.SetParent(transform, false);
            }

            for (int i = 0; i < count; i++)
            {
                string moteName = $"{orbitName}_Mote_{i}";
                Transform mote = orbit.Find(moteName);
                if (mote == null)
                {
                    GameObject moteObject =
                        GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    moteObject.name = moteName;
                    mote = moteObject.transform;
                    mote.SetParent(orbit, false);
                }

                float angle = i * (360f / count);
                mote.localPosition =
                    Quaternion.Euler(0f, 0f, angle) *
                    Vector3.up *
                    radius;
                mote.localScale = Vector3.one * scale;

                Collider collider = mote.GetComponent<Collider>();
                if (collider != null)
                    Destroy(collider);

                RefreshRenderer(mote.GetComponent<Renderer>(), color, 3.1f);
            }

            return orbit;
        }

        private void ConfigureTrail(TrailRenderer trail, Color color)
        {
            if (trail == null)
                return;

            trail.time =
                Mathf.Clamp(0.18f + ammoCount * 0.045f, 0.22f, 0.55f);
            trail.startWidth = 0.20f + ammoCount * 0.035f;
            trail.endWidth = 0f;
            trail.minVertexDistance = 0.035f;

            if (trail.sharedMaterial == null)
            {
                trail.sharedMaterial =
                    BDChargedShotVisualUtility.CreateVisualMaterial(
                        color,
                        3.4f
                    );
            }
            else
            {
                RefreshMaterial(trail.material, color, 3.4f);
            }

            Gradient gradient = new Gradient();
            gradient.SetKeys(
                new[]
                {
                    new GradientColorKey(Color.white, 0f),
                    new GradientColorKey(color, 0.28f),
                    new GradientColorKey(
                        Color.Lerp(color, Color.black, 0.35f),
                        1f
                    )
                },
                new[]
                {
                    new GradientAlphaKey(1f, 0f),
                    new GradientAlphaKey(0.86f, 0.34f),
                    new GradientAlphaKey(0f, 1f)
                }
            );
            trail.colorGradient = gradient;
        }

        private static void RefreshRenderer(
            Renderer renderer,
            Color color,
            float emissionMultiplier)
        {
            if (renderer == null)
                return;

            Material material = renderer.material;
            if (material == null)
            {
                material =
                    BDChargedShotVisualUtility.CreateVisualMaterial(
                        color,
                        emissionMultiplier
                    );
                if (material != null)
                    renderer.material = material;
                return;
            }

            RefreshMaterial(material, color, emissionMultiplier);
        }

        private static void RefreshMaterial(
            Material material,
            Color color,
            float emissionMultiplier)
        {
            if (material == null)
                return;

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
                    color * Mathf.Max(1f, emissionMultiplier)
                );
            }
        }
    }
    public static class BDChargedShotVisualUtility
    {
        public static Color ResolvePowerColor(int ammoCount)
        {
            float strength =
                Mathf.InverseLerp(2f, 6f, Mathf.Max(2, ammoCount));

            Color lower =
                new Color(0.18f, 0.78f, 1f, 1f);

            Color upper =
                new Color(1f, 0.42f, 0.08f, 1f);

            return Color.Lerp(lower, upper, strength);
        }

        public static void SpawnChargedMuzzleBurst(
            Vector3 position,
            Vector3 direction,
            int ammoCount)
        {
            direction.y = 0f;

            if (direction.sqrMagnitude < 0.001f)
                direction = Vector3.forward;

            Color color = ResolvePowerColor(ammoCount);

            GameObject root =
                new GameObject("BD_Charged_Muzzle_Burst");

            root.transform.position = position;
            root.transform.rotation =
                Quaternion.LookRotation(direction.normalized, Vector3.up);

            int count = Mathf.Clamp(8 + ammoCount * 3, 12, 28);
            float radius = 0.34f + ammoCount * 0.075f;

            for (int i = 0; i < count; i++)
            {
                float angle = i * (360f / count);

                GameObject mote =
                    GameObject.CreatePrimitive(PrimitiveType.Sphere);

                mote.name = "Charged_Muzzle_Mote";
                mote.transform.SetParent(root.transform, false);

                Vector3 radial =
                    Quaternion.Euler(0f, 0f, angle) *
                    Vector3.up *
                    radius;

                mote.transform.localPosition =
                    Vector3.forward * 0.18f + radial;
                mote.transform.localScale =
                    Vector3.one *
                    Mathf.Lerp(0.055f, 0.105f, ammoCount / 6f);

                ConfigureRenderer(
                    mote,
                    color,
                    emissionMultiplier: 3.4f
                );
            }

            BDChargedBurstLifetime lifetime =
                root.AddComponent<BDChargedBurstLifetime>();

            lifetime.Configure(
                0.28f,
                0.72f + ammoCount * 0.10f
            );
        }

        public static void SpawnChargedImpactBurst(
            Vector3 position,
            int ammoCount)
        {
            Color color = ResolvePowerColor(ammoCount);

            GameObject root =
                new GameObject("BD_Charged_Impact_Burst");

            root.transform.position = position;

            int count = Mathf.Clamp(12 + ammoCount * 4, 18, 38);
            float radius = 0.75f + ammoCount * 0.13f;

            for (int i = 0; i < count; i++)
            {
                float angle = i * (360f / count);
                Vector3 direction =
                    Quaternion.Euler(0f, angle, 0f) *
                    Vector3.forward;

                GameObject mote =
                    GameObject.CreatePrimitive(PrimitiveType.Sphere);

                mote.name = "Charged_Impact_Mote";
                mote.transform.SetParent(root.transform, false);
                mote.transform.localPosition =
                    direction * radius;
                mote.transform.localScale =
                    Vector3.one * (0.07f + ammoCount * 0.012f);

                ConfigureRenderer(
                    mote,
                    color,
                    emissionMultiplier: 3.6f
                );
            }

            BDChargedBurstLifetime lifetime =
                root.AddComponent<BDChargedBurstLifetime>();

            lifetime.Configure(
                0.42f,
                1.45f + ammoCount * 0.12f
            );
        }

        public static void SpawnCancelImplosion(
            Vector3 position,
            int ammoCount,
            float progress)
        {
            if (progress <= 0.05f)
                return;

            Color color = ResolvePowerColor(ammoCount);

            GameObject root =
                new GameObject("BD_Charged_Shot_Cancel");

            root.transform.position = position;

            int count = Mathf.Clamp(
                Mathf.RoundToInt(6f + progress * 10f),
                6,
                16
            );

            for (int i = 0; i < count; i++)
            {
                float angle = i * (360f / count);

                GameObject mote =
                    GameObject.CreatePrimitive(PrimitiveType.Sphere);

                mote.name = "Charge_Cancel_Mote";
                mote.transform.SetParent(root.transform, false);
                mote.transform.localPosition =
                    Quaternion.Euler(0f, angle, 0f) *
                    Vector3.forward *
                    Mathf.Lerp(0.18f, 0.58f, progress);
                mote.transform.localScale =
                    Vector3.one * 0.055f;

                ConfigureRenderer(
                    mote,
                    color,
                    emissionMultiplier: 2.1f
                );
            }

            BDChargedBurstLifetime lifetime =
                root.AddComponent<BDChargedBurstLifetime>();

            lifetime.Configure(0.18f, 0.12f);
        }

        public static void ConfigureRenderer(
            GameObject visual,
            Color color,
            float emissionMultiplier)
        {
            if (visual == null)
                return;

            Collider collider = visual.GetComponent<Collider>();

            if (collider != null)
                Object.Destroy(collider);

            Renderer renderer = visual.GetComponent<Renderer>();

            if (renderer == null)
                return;

            Material material =
                CreateVisualMaterial(color, emissionMultiplier);

            if (material != null)
                renderer.material = material;
        }

        public static Material CreateVisualMaterial(
            Color color,
            float emissionMultiplier)
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
                    color * Mathf.Max(1f, emissionMultiplier)
                );
            }

            return material;
        }
    }

    [DisallowMultipleComponent]
    public sealed class BDChargedBurstLifetime : MonoBehaviour
    {
        private float lifetime = 0.35f;
        private float targetScale = 1.5f;
        private float elapsed;
        private Vector3 startScale;

        public void Configure(
            float newLifetime,
            float newTargetScale)
        {
            lifetime = Mathf.Max(0.05f, newLifetime);
            targetScale = Mathf.Max(0.05f, newTargetScale);
            startScale = transform.localScale;
        }

        private void Awake()
        {
            startScale = transform.localScale;
        }

        private void Update()
        {
            elapsed += Time.unscaledDeltaTime;

            float t =
                Mathf.Clamp01(elapsed / Mathf.Max(0.01f, lifetime));

            transform.localScale =
                Vector3.Lerp(
                    startScale,
                    Vector3.one * targetScale,
                    t
                );

            if (elapsed >= lifetime)
                Destroy(gameObject);
        }
    }
}
