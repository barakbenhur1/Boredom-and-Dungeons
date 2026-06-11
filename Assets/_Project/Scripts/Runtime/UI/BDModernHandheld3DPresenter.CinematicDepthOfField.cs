using UnityEngine;

namespace BoredomAndDungeons
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Camera))]
    internal sealed class BDCinematicDepthOfField : MonoBehaviour
    {
        private const string ResourcePath =
            "ModernHandheld/Shaders/BDCinematicDepthOfField";
        private const string ShaderName =
            "Hidden/BoredomAndDungeons/CinematicDepthOfField";

        private Camera ownerCamera;
        private Transform focusTarget;
        private Material material;
        private float nearFocusRange = 2.15f;
        private float farFocusRange = 8.20f;
        private float blurStrength = 0.28f;
        private float maximumBlurPixels = 2.10f;

        public void Configure(
            Transform target,
            float nearRange,
            float farRange,
            float strength,
            float blurPixels)
        {
            focusTarget = target;
            nearFocusRange = Mathf.Max(0.05f, nearRange);
            farFocusRange = Mathf.Max(0.05f, farRange);
            blurStrength = Mathf.Clamp01(strength);
            maximumBlurPixels = Mathf.Clamp(blurPixels, 0f, 12f);
            EnsureResources();
        }

        private void OnEnable()
        {
            EnsureResources();
        }

        private bool EnsureResources()
        {
            if (ownerCamera == null)
                ownerCamera = GetComponent<Camera>();

            if (ownerCamera != null)
            {
                ownerCamera.depthTextureMode |= DepthTextureMode.Depth;
            }

            if (material != null)
                return true;

            Shader shader = Resources.Load<Shader>(ResourcePath);
            if (shader == null)
                shader = Shader.Find(ShaderName);

            if (shader == null || !shader.isSupported)
                return false;

            material = new Material(shader)
            {
                name = "BD Cinematic Depth Of Field",
                hideFlags = HideFlags.HideAndDontSave
            };
            return true;
        }

        private void OnRenderImage(
            RenderTexture source,
            RenderTexture destination)
        {
            if (!EnsureResources() ||
                focusTarget == null ||
                blurStrength <= 0.001f)
            {
                Graphics.Blit(source, destination);
                return;
            }

            float focusDistance = Mathf.Max(
                0.05f,
                Vector3.Dot(
                    focusTarget.position - ownerCamera.transform.position,
                    ownerCamera.transform.forward
                )
            );

            RenderTextureDescriptor descriptor = source.descriptor;
            descriptor.width = source.width;
            descriptor.height = source.height;
            descriptor.depthBufferBits = 0;
            descriptor.msaaSamples = 1;

            RenderTexture horizontal =
                RenderTexture.GetTemporary(descriptor);
            RenderTexture vertical =
                RenderTexture.GetTemporary(descriptor);
            horizontal.filterMode = FilterMode.Bilinear;
            vertical.filterMode = FilterMode.Bilinear;

            float horizontalStep =
                maximumBlurPixels / Mathf.Max(1f, descriptor.width);
            float verticalStep =
                maximumBlurPixels / Mathf.Max(1f, descriptor.height);

            material.SetVector(
                "_BlurDirection",
                new Vector4(horizontalStep, 0f, 0f, 0f)
            );
            Graphics.Blit(source, horizontal, material, 0);

            material.SetVector(
                "_BlurDirection",
                new Vector4(0f, verticalStep, 0f, 0f)
            );
            Graphics.Blit(horizontal, vertical, material, 0);

            material.SetTexture("_BlurTex", vertical);
            material.SetFloat("_FocusDistance", focusDistance);
            material.SetFloat("_NearFocusRange", nearFocusRange);
            material.SetFloat("_FarFocusRange", farFocusRange);
            material.SetFloat("_BlurStrength", blurStrength);
            Graphics.Blit(source, destination, material, 1);

            RenderTexture.ReleaseTemporary(horizontal);
            RenderTexture.ReleaseTemporary(vertical);
        }

        private void OnDestroy()
        {
            if (material == null)
                return;

            if (Application.isPlaying)
                Destroy(material);
            else
                DestroyImmediate(material);

            material = null;
        }
    }

    public sealed partial class BDModernHandheld3DPresenter
    {
        private BDCinematicDepthOfField cinematicDepthOfField;

        private void ConfigureCinematicDepthOfField()
        {
            if (deviceCamera == null || deviceVisualRoot == null)
                return;

            cinematicDepthOfField =
                deviceCamera.GetComponent<BDCinematicDepthOfField>();

            if (cinematicDepthOfField == null)
            {
                cinematicDepthOfField =
                    deviceCamera.gameObject.AddComponent<
                        BDCinematicDepthOfField>();
            }

            cinematicDepthOfField.Configure(
                deviceVisualRoot,
                2.15f,
                8.20f,
                0.28f,
                2.10f
            );
        }
    }
}
