using UnityEngine;

namespace BoredomAndDungeons
{
    public static class BDRangedAttackVisuals
    {
        private static Material trailMaterial;
        private static Material flashMaterial;

        public static void AddProjectileTrail(GameObject projectile, bool playerProjectile)
        {
            if (!BDPerformanceGuard.AllowCosmeticSpawn(playerProjectile ? 0.70f : 0.45f))
                return;
            if (projectile == null)
                return;

            TrailRenderer trail = projectile.GetComponent<TrailRenderer>();
            if (trail == null)
                trail = projectile.AddComponent<TrailRenderer>();

            trail.time = playerProjectile ? 0.18f : 0.12f;
            trail.minVertexDistance = 0.035f;
            trail.widthMultiplier = playerProjectile ? 0.18f : 0.12f;
            trail.autodestruct = false;
            trail.emitting = true;
            trail.numCornerVertices = 3;
            trail.numCapVertices = 3;
            trail.material = GetTrailMaterial();

            Color start = playerProjectile
                ? new Color(0.35f, 0.92f, 1f, 0.90f)
                : new Color(1f, 0.35f, 0.16f, 0.85f);

            Color end = new Color(start.r, start.g, start.b, 0f);
            trail.startColor = start;
            trail.endColor = end;
        }

        public static void SpawnMuzzleFlash(Vector3 position, Vector3 direction, bool playerProjectile)
        {
            if (!BDPerformanceGuard.AllowCosmeticSpawn(playerProjectile ? 0.80f : 0.55f))
                return;
            direction.y = 0f;

            if (direction.sqrMagnitude < 0.001f)
                direction = Vector3.forward;

            direction.Normalize();

            GameObject flash = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            flash.name = playerProjectile ? "BD_Player_Ranged_MuzzleFlash" : "BD_Enemy_Ranged_MuzzleFlash";
            flash.transform.position = position;
            flash.transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
            flash.transform.localScale = Vector3.one * (playerProjectile ? 0.34f : 0.24f);

            Collider collider = flash.GetComponent<Collider>();
            if (collider != null)
                Object.Destroy(collider);

            Renderer renderer = flash.GetComponent<Renderer>();
            if (renderer != null)
            {
                Material material = GetFlashMaterial();
                if (material != null)
                    renderer.sharedMaterial = material;
            }

            BDRangedMuzzleFlashVisual visual = flash.AddComponent<BDRangedMuzzleFlashVisual>();
            visual.Configure(playerProjectile);
        }

        public static void SpawnImpactBurst(Vector3 position, bool playerProjectile)
        {
            if (!BDPerformanceGuard.AllowCosmeticSpawn(playerProjectile ? 0.85f : 0.55f))
                return;
            GameObject burst = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            burst.name = playerProjectile ? "BD_Player_Ranged_Impact" : "BD_Enemy_Ranged_Impact";
            burst.transform.position = position;
            burst.transform.localScale = Vector3.one * (playerProjectile ? 0.28f : 0.22f);

            Collider collider = burst.GetComponent<Collider>();
            if (collider != null)
                Object.Destroy(collider);

            Renderer renderer = burst.GetComponent<Renderer>();
            if (renderer != null)
            {
                Material material = GetFlashMaterial();
                if (material != null)
                    renderer.sharedMaterial = material;
            }

            BDRangedImpactBurstVisual visual = burst.AddComponent<BDRangedImpactBurstVisual>();
            visual.Configure(playerProjectile);
        }

        private static Material GetTrailMaterial()
        {
            if (trailMaterial != null)
                return trailMaterial;

            Shader shader = Shader.Find("Sprites/Default");
            if (shader == null) shader = Shader.Find("Unlit/Color");
            if (shader == null) shader = Shader.Find("Universal Render Pipeline/Unlit");
            if (shader == null) shader = Shader.Find("Universal Render Pipeline/Lit");
            if (shader == null) shader = Shader.Find("Standard");
            if (shader == null) shader = Shader.Find("Hidden/InternalErrorShader");

            if (shader != null)
            {
                trailMaterial = new Material(shader);
                trailMaterial.color = Color.white;
                return trailMaterial;
            }

            Material builtIn = Resources.GetBuiltinResource<Material>("Default-Material.mat");
            if (builtIn != null)
                trailMaterial = new Material(builtIn);

            return trailMaterial;
        }

        private static Material GetFlashMaterial()
        {
            if (flashMaterial != null)
                return flashMaterial;

            Shader shader = Shader.Find("Universal Render Pipeline/Lit");
            if (shader == null) shader = Shader.Find("Standard");
            if (shader == null) shader = Shader.Find("Unlit/Color");
            if (shader == null) shader = Shader.Find("Sprites/Default");
            if (shader == null) shader = Shader.Find("Hidden/InternalErrorShader");

            if (shader != null)
            {
                flashMaterial = new Material(shader);
                flashMaterial.color = Color.white;

                if (flashMaterial.HasProperty("_EmissionColor"))
                {
                    flashMaterial.EnableKeyword("_EMISSION");
                    flashMaterial.SetColor("_EmissionColor", Color.white);
                }

                return flashMaterial;
            }

            Material builtIn = Resources.GetBuiltinResource<Material>("Default-Material.mat");
            if (builtIn != null)
                flashMaterial = new Material(builtIn);

            return flashMaterial;
        }
    }

    [DisallowMultipleComponent]
    public sealed class BDRangedMuzzleFlashVisual : MonoBehaviour
    {
        [SerializeField] private float lifetime = 0.105f;
        [SerializeField] private bool playerProjectile;

        private float timer;
        private Renderer cachedRenderer;
        private MaterialPropertyBlock block;

        private static readonly int BaseColorId = Shader.PropertyToID("_BaseColor");
        private static readonly int ColorId = Shader.PropertyToID("_Color");
        private static readonly int EmissionColorId = Shader.PropertyToID("_EmissionColor");

        public void Configure(bool isPlayerProjectile)
        {
            playerProjectile = isPlayerProjectile;
            lifetime = BDPerformanceGuard.ResolveEffectLifetime(playerProjectile ? 0.115f : 0.085f);
        }

        private void Awake()
        {
            cachedRenderer = GetComponent<Renderer>();
            block = new MaterialPropertyBlock();
        }

        private void Update()
        {
            timer += Time.deltaTime;
            float t = Mathf.Clamp01(timer / Mathf.Max(0.01f, lifetime));
            float alpha = 1f - t;
            float scale = Mathf.Lerp(1f, playerProjectile ? 2.1f : 1.6f, t);
            transform.localScale = transform.localScale.normalized * Mathf.Max(0.01f, transform.localScale.magnitude) * (1f + Time.deltaTime * scale);

            if (cachedRenderer != null)
            {
                Color color = playerProjectile
                    ? new Color(0.50f, 0.95f, 1f, alpha)
                    : new Color(1f, 0.38f, 0.12f, alpha);

                cachedRenderer.GetPropertyBlock(block);
                block.SetColor(BaseColorId, color);
                block.SetColor(ColorId, color);
                block.SetColor(EmissionColorId, color * 2.1f);
                cachedRenderer.SetPropertyBlock(block);
            }

            if (timer >= lifetime)
                Destroy(gameObject);
        }
    }

    [DisallowMultipleComponent]
    public sealed class BDRangedImpactBurstVisual : MonoBehaviour
    {
        [SerializeField] private float lifetime = 0.18f;
        [SerializeField] private bool playerProjectile;

        private float timer;
        private Renderer cachedRenderer;
        private MaterialPropertyBlock block;

        private static readonly int BaseColorId = Shader.PropertyToID("_BaseColor");
        private static readonly int ColorId = Shader.PropertyToID("_Color");
        private static readonly int EmissionColorId = Shader.PropertyToID("_EmissionColor");

        public void Configure(bool isPlayerProjectile)
        {
            playerProjectile = isPlayerProjectile;
            lifetime = BDPerformanceGuard.ResolveEffectLifetime(playerProjectile ? 0.18f : 0.13f);
        }

        private void Awake()
        {
            cachedRenderer = GetComponent<Renderer>();
            block = new MaterialPropertyBlock();
        }

        private void Update()
        {
            timer += Time.deltaTime;
            float t = Mathf.Clamp01(timer / Mathf.Max(0.01f, lifetime));
            float eased = 1f - Mathf.Pow(1f - t, 3f);
            float scale = Mathf.Lerp(0.35f, playerProjectile ? 1.35f : 0.95f, eased);
            transform.localScale = Vector3.one * scale;

            if (cachedRenderer != null)
            {
                Color color = playerProjectile
                    ? new Color(0.38f, 0.92f, 1f, 1f - t)
                    : new Color(1f, 0.35f, 0.12f, 1f - t);

                cachedRenderer.GetPropertyBlock(block);
                block.SetColor(BaseColorId, color);
                block.SetColor(ColorId, color);
                block.SetColor(EmissionColorId, color * 1.9f);
                cachedRenderer.SetPropertyBlock(block);
            }

            if (timer >= lifetime)
                Destroy(gameObject);
        }
    }
}
