using UnityEngine;

namespace BoredomAndDungeons
{
    [DisallowMultipleComponent]
    public sealed class BDPlayerDodgeIFrameFeedback : MonoBehaviour
    {
        [SerializeField] private float pulseFrequency = 22f;
        [SerializeField] private Color iframeColor = new Color(0.55f, 0.90f, 1f, 1f);

        private BDPlayerController playerController;
        private Renderer[] renderers;
        private MaterialPropertyBlock propertyBlock;
        private static readonly int ColorProperty = Shader.PropertyToID("_Color");
        private static readonly int BaseColorProperty = Shader.PropertyToID("_BaseColor");

        private void Awake()
        {
            playerController = GetComponent<BDPlayerController>();
            renderers = GetComponentsInChildren<Renderer>(includeInactive: true);
            propertyBlock = new MaterialPropertyBlock();
        }

        private void LateUpdate()
        {
            if (playerController == null || renderers == null)
                return;

            bool active = playerController.IsDodgeInvulnerable;
            float pulse = active ? 0.55f + Mathf.Sin(Time.time * pulseFrequency) * 0.25f : 0f;

            for (int i = 0; i < renderers.Length; i++)
            {
                Renderer renderer = renderers[i];
                if (renderer == null)
                    continue;

                renderer.GetPropertyBlock(propertyBlock);

                if (active)
                {
                    Color color = Color.Lerp(Color.white, iframeColor, Mathf.Clamp01(pulse));
                    propertyBlock.SetColor(ColorProperty, color);
                    propertyBlock.SetColor(BaseColorProperty, color);
                }
                else
                {
                    propertyBlock.Clear();
                }

                renderer.SetPropertyBlock(propertyBlock);
            }
        }
    }
}
