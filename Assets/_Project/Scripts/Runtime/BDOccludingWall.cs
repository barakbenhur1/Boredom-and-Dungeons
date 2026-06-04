using UnityEngine;

namespace BoredomAndDungeons
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Renderer))]
    public sealed class BDOccludingWall : MonoBehaviour
    {
        [SerializeField] private float fadedAlpha = 0.28f;
        [SerializeField] private float fadeSmoothing = 18f;

        private Renderer cachedRenderer;
        private Material runtimeMaterial;
        private Color originalColor;
        private bool initialized;
        private bool faded;
        private float currentAlpha;
        private float targetAlpha;

        private void Awake()
        {
            Initialize();
        }

        private void Update()
        {
            if (!initialized)
                Initialize();

            if (runtimeMaterial == null)
                return;

            float nextAlpha = Mathf.Lerp(currentAlpha, targetAlpha, 1f - Mathf.Exp(-fadeSmoothing * Time.deltaTime));

            if (Mathf.Abs(nextAlpha - currentAlpha) <= 0.001f)
                return;

            currentAlpha = nextAlpha;
            ApplyAlpha(currentAlpha);
        }

        public void SetFaded(bool value)
        {
            Initialize();

            if (runtimeMaterial == null)
                return;

            if (faded == value)
                return;

            faded = value;
            targetAlpha = faded ? fadedAlpha : originalColor.a;

            if (faded)
                ConfigureTransparent(runtimeMaterial);
            else if (targetAlpha >= originalColor.a - 0.01f)
                ConfigureOpaque(runtimeMaterial);
        }

        private void Initialize()
        {
            if (initialized)
                return;

            initialized = true;
            cachedRenderer = GetComponent<Renderer>();

            if (cachedRenderer == null)
                return;

            runtimeMaterial = cachedRenderer.material;
            originalColor = runtimeMaterial.color;
            currentAlpha = originalColor.a;
            targetAlpha = originalColor.a;
        }

        private void ApplyAlpha(float alpha)
        {
            if (runtimeMaterial == null)
                return;

            Color color = originalColor;
            color.a = alpha;
            runtimeMaterial.color = color;

            if (!faded && alpha >= originalColor.a - 0.012f)
                ConfigureOpaque(runtimeMaterial);
        }

        private static void ConfigureTransparent(Material material)
        {
            if (material == null)
                return;

            material.SetFloat("_Mode", 3f);
            material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            material.SetInt("_ZWrite", 0);
            material.DisableKeyword("_ALPHATEST_ON");
            material.EnableKeyword("_ALPHABLEND_ON");
            material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            material.renderQueue = 3000;
        }

        private static void ConfigureOpaque(Material material)
        {
            if (material == null)
                return;

            material.SetFloat("_Mode", 0f);
            material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
            material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
            material.SetInt("_ZWrite", 1);
            material.DisableKeyword("_ALPHATEST_ON");
            material.DisableKeyword("_ALPHABLEND_ON");
            material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            material.renderQueue = -1;
        }

        private void OnDisable()
        {
            if (faded)
                SetFaded(false);
        }
    }
}
