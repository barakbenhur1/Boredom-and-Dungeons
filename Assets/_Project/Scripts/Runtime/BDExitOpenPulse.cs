using UnityEngine;

namespace BoredomAndDungeons
{
    [DisallowMultipleComponent]
    public sealed class BDExitOpenPulse : MonoBehaviour
    {
        [SerializeField] private float pulseDuration = 2.2f;
        [SerializeField] private float pulseScale = 1.18f;
        [SerializeField] private Color pulseColor = new Color(0.25f, 1f, 0.62f, 1f);
        [SerializeField] private bool pulseOnceOnly = true;

        private Renderer[] renderers;
        private MaterialPropertyBlock block;
        private Vector3 originalScale;
        private float pulseStartedAt = -999f;
        private bool hasPulsed;

        private static readonly int BaseColorId = Shader.PropertyToID("_BaseColor");
        private static readonly int ColorId = Shader.PropertyToID("_Color");
        private static readonly int EmissionColorId = Shader.PropertyToID("_EmissionColor");

        private void Awake()
        {
            renderers = GetComponentsInChildren<Renderer>(includeInactive: true);
            block = new MaterialPropertyBlock();
            originalScale = transform.localScale;
        }

        public void TriggerOpenPulse()
        {
            if (pulseOnceOnly && hasPulsed)
                return;

            hasPulsed = true;
            pulseStartedAt = Time.time;
        }

        private void Update()
        {
            if (pulseStartedAt < -100f)
                return;

            float elapsed = Time.time - pulseStartedAt;

            if (elapsed >= pulseDuration)
            {
                transform.localScale = originalScale;
                ClearBlocks();
                pulseStartedAt = -999f;
                return;
            }

            float progress = Mathf.Clamp01(elapsed / Mathf.Max(0.01f, pulseDuration));
            float wave = Mathf.Sin(progress * Mathf.PI * 4f) * (1f - progress);
            float scale = 1f + Mathf.Abs(wave) * (pulseScale - 1f);

            transform.localScale = originalScale * scale;

            Color color = pulseColor;
            color.a = Mathf.Clamp01(1f - progress);

            ApplyColor(color, Mathf.Abs(wave));
        }

        private void ApplyColor(Color color, float intensity)
        {
            if (renderers == null)
                return;

            Color emission = color * Mathf.Lerp(0.7f, 2.0f, intensity);

            for (int i = 0; i < renderers.Length; i++)
            {
                Renderer renderer = renderers[i];
                if (renderer == null)
                    continue;

                renderer.GetPropertyBlock(block);
                block.SetColor(BaseColorId, color);
                block.SetColor(ColorId, color);
                block.SetColor(EmissionColorId, emission);
                renderer.SetPropertyBlock(block);
            }
        }

        private void ClearBlocks()
        {
            if (renderers == null)
                return;

            for (int i = 0; i < renderers.Length; i++)
            {
                Renderer renderer = renderers[i];
                if (renderer == null)
                    continue;

                renderer.GetPropertyBlock(block);
                block.Clear();
                renderer.SetPropertyBlock(block);
            }
        }
    }
}
