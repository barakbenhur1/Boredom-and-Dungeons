using UnityEngine;

namespace BoredomAndDungeons
{
    [DisallowMultipleComponent]
    public sealed class BDEnemyAttackTelegraphVisual : MonoBehaviour
    {
        [SerializeField] private float lifetime = 0.18f;
        [SerializeField] private float startScale = 0.25f;
        [SerializeField] private float endScale = 1.25f;
        [SerializeField] private Color color = new Color(1f, 0.26f, 0.08f, 0.72f);

        private float timer;
        private Renderer cachedRenderer;
        private MaterialPropertyBlock block;
        private Vector3 baseScale;

        private static Material sharedMaterial;
        private static readonly int BaseColorId = Shader.PropertyToID("_BaseColor");
        private static readonly int ColorId = Shader.PropertyToID("_Color");
        private static readonly int EmissionColorId = Shader.PropertyToID("_EmissionColor");

        public static void Spawn(Vector3 position, Vector3 direction, float radius, float duration, bool ranged)
        {
            if (BDMountedRunIntro.IsGameplayInputLocked)
                return;

            if (!BDPerformanceGuard.AllowCosmeticSpawn(ranged ? 0.45f : 0.60f))
                return;
            direction.y = 0f;

            if (direction.sqrMagnitude < 0.001f)
                direction = Vector3.forward;

            direction.Normalize();

            GameObject go = GameObject.CreatePrimitive(ranged ? PrimitiveType.Cylinder : PrimitiveType.Cube);
            go.name = ranged ? "BD_Enemy_Ranged_Telegraph" : "BD_Enemy_Melee_Telegraph";
            go.transform.position = position + Vector3.up * 0.055f;
            go.transform.rotation = Quaternion.LookRotation(direction, Vector3.up);

            float safeRadius = Mathf.Max(0.35f, radius);
            go.transform.localScale = ranged
                ? new Vector3(safeRadius * 0.72f, 0.018f, safeRadius * 0.72f)
                : new Vector3(safeRadius * 0.95f, 0.035f, safeRadius * 1.35f);

            Collider collider = go.GetComponent<Collider>();
            if (collider != null)
                Destroy(collider);

            Renderer renderer = go.GetComponent<Renderer>();
            if (renderer != null)
            {
                Material material = GetSharedMaterial();
                if (material != null)
                    renderer.sharedMaterial = material;
            }

            BDEnemyAttackTelegraphVisual visual = go.AddComponent<BDEnemyAttackTelegraphVisual>();
            visual.Configure(duration, safeRadius, ranged);
        }

        private void Configure(float duration, float radius, bool ranged)
        {
            lifetime = BDPerformanceGuard.ResolveEffectLifetime(Mathf.Max(0.08f, duration));
            startScale = 0.65f;
            endScale = ranged ? 1.45f : 1.18f;
            color = ranged
                ? new Color(1f, 0.55f, 0.12f, 0.62f)
                : new Color(1f, 0.18f, 0.08f, 0.70f);
        }

        private void Awake()
        {
            cachedRenderer = GetComponent<Renderer>();
            block = new MaterialPropertyBlock();
            baseScale = transform.localScale;
            transform.localScale = baseScale * startScale;
        }

        private void Update()
        {
            timer += Time.deltaTime;
            float t = Mathf.Clamp01(timer / Mathf.Max(0.01f, lifetime));
            float pulse = Mathf.Sin(t * Mathf.PI);
            float scale = Mathf.Lerp(startScale, endScale, t) + pulse * 0.08f;

            transform.localScale = baseScale * scale;

            if (cachedRenderer != null)
            {
                Color c = color;
                c.a *= 1f - t * 0.92f;

                cachedRenderer.GetPropertyBlock(block);
                block.SetColor(BaseColorId, c);
                block.SetColor(ColorId, c);
                block.SetColor(EmissionColorId, c * 1.6f);
                cachedRenderer.SetPropertyBlock(block);
            }

            if (timer >= lifetime)
                Destroy(gameObject);
        }

        private static Material GetSharedMaterial()
        {
            if (sharedMaterial != null)
                return sharedMaterial;

            Shader shader = Shader.Find("Universal Render Pipeline/Lit");
            if (shader == null) shader = Shader.Find("Standard");
            if (shader == null) shader = Shader.Find("Unlit/Color");
            if (shader == null) shader = Shader.Find("Sprites/Default");
            if (shader == null) shader = Shader.Find("Hidden/InternalErrorShader");

            if (shader != null)
            {
                sharedMaterial = new Material(shader);
                sharedMaterial.color = Color.white;

                if (sharedMaterial.HasProperty("_EmissionColor"))
                {
                    sharedMaterial.EnableKeyword("_EMISSION");
                    sharedMaterial.SetColor("_EmissionColor", Color.white);
                }

                return sharedMaterial;
            }

            Material builtIn = Resources.GetBuiltinResource<Material>("Default-Material.mat");
            if (builtIn != null)
                sharedMaterial = new Material(builtIn);

            return sharedMaterial;
        }
    }
}
