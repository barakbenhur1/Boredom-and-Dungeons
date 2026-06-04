using UnityEngine;

namespace BoredomAndDungeons
{
    [DisallowMultipleComponent]
    public sealed class BDEnemyHitFlashReceiver : MonoBehaviour
    {
        [SerializeField] private float defaultFlashDuration = 0.085f;
        [SerializeField] private Color lightFlashColor = new Color(1f, 0.90f, 0.55f, 1f);
        [SerializeField] private Color heavyFlashColor = new Color(1f, 0.42f, 0.10f, 1f);
        [SerializeField] private Color rangedFlashColor = new Color(0.55f, 0.92f, 1f, 1f);
        [SerializeField] private float flashBlendStrength = 0.58f;
        [SerializeField] private float emissionMultiplier = 1.85f;

        private struct RendererFlashState
        {
            public Renderer renderer;
            public Color baseColor;
            public Color color;
            public Color emissionColor;
            public bool hasBaseColor;
            public bool hasColor;
            public bool hasEmissionColor;
            public bool eligible;
        }

        private RendererFlashState[] states;
        private MaterialPropertyBlock block;
        private float flashUntil;
        private float flashDuration;
        private Color activeColor;
        private bool hasActiveFlash;

        private static readonly int BaseColorId = Shader.PropertyToID("_BaseColor");
        private static readonly int ColorId = Shader.PropertyToID("_Color");
        private static readonly int EmissionColorId = Shader.PropertyToID("_EmissionColor");

        private void Awake()
        {
            block = new MaterialPropertyBlock();
            CacheRenderers();
        }

        public void FlashLight()
        {
            RequestFlash(lightFlashColor, defaultFlashDuration);
        }

        public void FlashHeavy()
        {
            RequestFlash(heavyFlashColor, defaultFlashDuration * 1.45f);
        }

        public void FlashRanged()
        {
            RequestFlash(rangedFlashColor, defaultFlashDuration * 0.95f);
        }

        public void RequestFlash(Color color, float duration)
        {
            if (!Application.isPlaying)
                return;

            if (states == null || states.Length == 0)
                CacheRenderers();

            activeColor = color;
            flashDuration = Mathf.Max(0.025f, duration);
            flashUntil = Time.time + flashDuration;
            hasActiveFlash = true;

            Apply(1f);
        }

        private void LateUpdate()
        {
            if (!hasActiveFlash)
                return;

            float remaining = flashUntil - Time.time;
            if (remaining <= 0f)
            {
                RestoreOriginalColors();
                return;
            }

            float t = Mathf.Clamp01(remaining / Mathf.Max(0.01f, flashDuration));
            float strength = t * t;
            Apply(strength);
        }

        private void CacheRenderers()
        {
            Renderer[] renderers = GetComponentsInChildren<Renderer>();
            states = new RendererFlashState[renderers.Length];

            for (int i = 0; i < renderers.Length; i++)
            {
                Renderer renderer = renderers[i];

                RendererFlashState state = new RendererFlashState
                {
                    renderer = renderer,
                    eligible = IsEligibleRenderer(renderer),
                    baseColor = Color.white,
                    color = Color.white,
                    emissionColor = Color.black
                };

                Material material = renderer != null ? renderer.sharedMaterial : null;
                if (material != null)
                {
                    state.hasBaseColor = material.HasProperty(BaseColorId);
                    state.hasColor = material.HasProperty(ColorId);
                    state.hasEmissionColor = material.HasProperty(EmissionColorId);

                    if (state.hasBaseColor)
                        state.baseColor = material.GetColor(BaseColorId);

                    if (state.hasColor)
                        state.color = material.GetColor(ColorId);

                    if (state.hasEmissionColor)
                        state.emissionColor = material.GetColor(EmissionColorId);
                }

                states[i] = state;
            }
        }

        private bool IsEligibleRenderer(Renderer renderer)
        {
            if (renderer == null)
                return false;

            // Do not tint world-space HP bars, telegraphs, pickups, or helper visuals.
            if (renderer.GetComponentInParent<BDEnemyWorldHealthBar>() != null)
                return false;

            if (renderer.GetComponentInParent<BDEnemyAttackTelegraphVisual>() != null)
                return false;

            if (renderer.GetComponentInParent<BDRangedImpactBurstVisual>() != null)
                return false;

            if (renderer.GetComponentInParent<BDMeleeImpactBurst>() != null)
                return false;

            if (renderer.GetComponentInParent<BDDeathBurstVisual>() != null)
                return false;

            if (renderer.GetComponentInParent<BDHealingPickupCollectBurst>() != null)
                return false;

            return true;
        }

        private void Apply(float strength)
        {
            if (states == null)
                return;

            strength = Mathf.Clamp01(strength);
            Color flash = activeColor;
            flash.a = 1f;

            for (int i = 0; i < states.Length; i++)
            {
                RendererFlashState state = states[i];
                Renderer renderer = state.renderer;

                if (!state.eligible || renderer == null)
                    continue;

                renderer.GetPropertyBlock(block);

                if (state.hasBaseColor)
                {
                    Color blended = Color.Lerp(state.baseColor, flash, flashBlendStrength * strength);
                    blended.a = state.baseColor.a;
                    block.SetColor(BaseColorId, blended);
                }

                if (state.hasColor)
                {
                    Color blended = Color.Lerp(state.color, flash, flashBlendStrength * strength);
                    blended.a = state.color.a;
                    block.SetColor(ColorId, blended);
                }

                if (state.hasEmissionColor)
                {
                    Color emission = state.emissionColor + flash * emissionMultiplier * strength;
                    block.SetColor(EmissionColorId, emission);
                }

                renderer.SetPropertyBlock(block);
            }
        }

        private void RestoreOriginalColors()
        {
            hasActiveFlash = false;

            if (states == null)
                return;

            for (int i = 0; i < states.Length; i++)
            {
                RendererFlashState state = states[i];
                Renderer renderer = state.renderer;

                if (!state.eligible || renderer == null)
                    continue;

                renderer.GetPropertyBlock(block);

                if (state.hasBaseColor)
                    block.SetColor(BaseColorId, state.baseColor);

                if (state.hasColor)
                    block.SetColor(ColorId, state.color);

                if (state.hasEmissionColor)
                    block.SetColor(EmissionColorId, state.emissionColor);

                renderer.SetPropertyBlock(block);
            }
        }

        private void OnDisable()
        {
            RestoreOriginalColors();
        }

        private void OnDestroy()
        {
            RestoreOriginalColors();
        }
    }
}
