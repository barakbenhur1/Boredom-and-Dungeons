using UnityEngine;

namespace BoredomAndDungeons
{
    [DisallowMultipleComponent]
    public sealed class BDMeleeImpactBurst : MonoBehaviour
    {
        [SerializeField] private float lifetime = 0.18f;
        [SerializeField] private float startScale = 0.22f;
        [SerializeField] private float endScale = 1.25f;
        [SerializeField] private bool heavy;

        private float timer;
        private Renderer cachedRenderer;
        private MaterialPropertyBlock block;
        private static Material cachedMaterial;

        private static readonly int BaseColorId = Shader.PropertyToID("_BaseColor");
        private static readonly int ColorId = Shader.PropertyToID("_Color");
        private static readonly int EmissionColorId = Shader.PropertyToID("_EmissionColor");

        public static void Spawn(Vector3 position, Vector3 direction, bool isHeavy)
        {
            GameObject burst = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            burst.name = isHeavy ? "BD_Heavy_Melee_Impact" : "BD_Light_Melee_Impact";
            burst.transform.position = position;

            direction.y = 0f;
            if (direction.sqrMagnitude > 0.001f)
                burst.transform.rotation = Quaternion.LookRotation(direction.normalized, Vector3.up);

            Collider collider = burst.GetComponent<Collider>();
            if (collider != null)
                Destroy(collider);

            Renderer renderer = burst.GetComponent<Renderer>();
            if (renderer != null)
            {
                Material material = GetImpactMaterial();
                if (material != null)
                    renderer.sharedMaterial = material;
            }

            BDMeleeImpactBurst impact = burst.AddComponent<BDMeleeImpactBurst>();
            impact.Configure(isHeavy);
        }

        private void Configure(bool isHeavy)
        {
            heavy = isHeavy;
            lifetime = heavy ? 0.24f : 0.14f;
            startScale = heavy ? 0.34f : 0.18f;
            endScale = heavy ? 1.75f : 0.85f;
            transform.localScale = Vector3.one * startScale;
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

            transform.localScale = Vector3.one * Mathf.Lerp(startScale, endScale, eased);

            if (cachedRenderer != null)
            {
                Color color = heavy
                    ? new Color(1f, 0.46f, 0.08f, 1f - t)
                    : new Color(1f, 0.95f, 0.35f, 1f - t);

                cachedRenderer.GetPropertyBlock(block);
                block.SetColor(BaseColorId, color);
                block.SetColor(ColorId, color);
                block.SetColor(EmissionColorId, color * (heavy ? 2.2f : 1.4f));
                cachedRenderer.SetPropertyBlock(block);
            }

            if (timer >= lifetime)
                Destroy(gameObject);
        }

        private static Material GetImpactMaterial()
        {
            if (cachedMaterial != null)
                return cachedMaterial;

            Shader shader = Shader.Find("Universal Render Pipeline/Lit");
            if (shader == null) shader = Shader.Find("Standard");
            if (shader == null) shader = Shader.Find("Unlit/Color");
            if (shader == null) shader = Shader.Find("Sprites/Default");
            if (shader == null) shader = Shader.Find("Hidden/InternalErrorShader");

            if (shader != null)
            {
                cachedMaterial = new Material(shader);
                cachedMaterial.color = Color.white;

                if (cachedMaterial.HasProperty("_EmissionColor"))
                {
                    cachedMaterial.EnableKeyword("_EMISSION");
                    cachedMaterial.SetColor("_EmissionColor", Color.white);
                }

                return cachedMaterial;
            }

            Material builtIn = Resources.GetBuiltinResource<Material>("Default-Material.mat");
            if (builtIn != null)
                cachedMaterial = new Material(builtIn);

            return cachedMaterial;
        }
    }
}
