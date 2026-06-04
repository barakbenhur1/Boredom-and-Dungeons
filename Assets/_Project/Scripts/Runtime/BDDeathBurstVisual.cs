using UnityEngine;

namespace BoredomAndDungeons
{
    [DisallowMultipleComponent]
    public sealed class BDDeathBurstVisual : MonoBehaviour
    {
        [SerializeField] private float lifetime = 0.34f;
        [SerializeField] private float startScale = 0.35f;
        [SerializeField] private float endScale = 2.2f;

        private float timer;
        private Renderer cachedRenderer;
        private MaterialPropertyBlock block;
        private static Material cachedMaterial;

        private static readonly int BaseColorId = Shader.PropertyToID("_BaseColor");
        private static readonly int ColorId = Shader.PropertyToID("_Color");
        private static readonly int EmissionColorId = Shader.PropertyToID("_EmissionColor");

        public static void Spawn(Vector3 position)
        {
            GameObject burst = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            burst.name = "BD_Enemy_Death_Burst";
            burst.transform.position = position + Vector3.up * 1.0f;
            burst.transform.localScale = Vector3.one * 0.35f;

            Collider collider = burst.GetComponent<Collider>();
            if (collider != null)
                Destroy(collider);

            Renderer renderer = burst.GetComponent<Renderer>();
            if (renderer != null)
            {
                Material material = GetBurstMaterial();
                if (material != null)
                    renderer.sharedMaterial = material;
            }

            burst.AddComponent<BDDeathBurstVisual>();
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
                Color color = new Color(1f, 0.72f, 0.18f, 1f - t);
                cachedRenderer.GetPropertyBlock(block);
                block.SetColor(BaseColorId, color);
                block.SetColor(ColorId, color);
                block.SetColor(EmissionColorId, color * 1.9f);
                cachedRenderer.SetPropertyBlock(block);
            }

            if (timer >= lifetime)
                Destroy(gameObject);
        }

        private static Material GetBurstMaterial()
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
