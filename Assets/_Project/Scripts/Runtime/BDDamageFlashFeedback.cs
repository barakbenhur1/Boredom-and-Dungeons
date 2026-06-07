using UnityEngine;

namespace BoredomAndDungeons
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(BDHealth))]
    public sealed class BDDamageFlashFeedback : MonoBehaviour
    {
        [SerializeField] private float flashDuration = 0.12f;
        [SerializeField] private Color damageFlashColor = new Color(1f, 0.22f, 0.12f, 1f);
        [SerializeField] private Color heavyFlashColor = new Color(1f, 0.55f, 0.05f, 1f);
        [SerializeField] private Color deathFlashColor = new Color(0.18f, 0.18f, 0.18f, 1f);

        private BDHealth health;
        private Renderer[] renderers;
        private MaterialPropertyBlock block;
        private float previousHealth;
        private float flashTimer;
        private bool deathFlash;
        private Color activeFlashColor;

        private static readonly int BaseColorId = Shader.PropertyToID("_BaseColor");
        private static readonly int ColorId = Shader.PropertyToID("_Color");
        private static readonly int EmissionColorId = Shader.PropertyToID("_EmissionColor");

        private void Awake()
        {
            health = GetComponent<BDHealth>();
            renderers = GetComponentsInChildren<Renderer>(includeInactive: true);
            block = new MaterialPropertyBlock();

            previousHealth = health != null ? health.CurrentHealth : 0f;
            activeFlashColor = damageFlashColor;

            if (health != null)
            {
                health.HealthChanged += OnHealthChanged;
                health.Died += OnDied;
            }
        }

        private void Update()
        {
            if (BDNewRunFeedbackReset.IsFeedbackSuppressed)
            {
                ResetTransientFeedback();
                return;
            }

            if (flashTimer <= 0f)
                return;

            flashTimer -= Time.deltaTime;
            float t = Mathf.Clamp01(flashTimer / Mathf.Max(0.01f, flashDuration));

            Color color = deathFlash ? deathFlashColor : activeFlashColor;
            color.a = t;

            ApplyFlash(color, t);

            if (flashTimer <= 0f)
                ClearFlash();
        }


        public void ResetTransientFeedback()
        {
            flashTimer = 0f;
            deathFlash = false;
            previousHealth = health != null ? health.CurrentHealth : previousHealth;
            ClearFlash();
        }

        public void TriggerImpactFlash(bool heavy)
        {
            deathFlash = false;
            activeFlashColor = heavy ? heavyFlashColor : damageFlashColor;
            flashTimer = flashDuration * (heavy ? 1.85f : 1.0f);
        }

        private void OnHealthChanged(BDHealth changedHealth, float current, float max)
        {
            if (BDNewRunFeedbackReset.IsFeedbackSuppressed)
            {
                previousHealth = current;
                ResetTransientFeedback();
                return;
            }

            if (current < previousHealth)
            {
                deathFlash = false;
                activeFlashColor = damageFlashColor;
                flashTimer = Mathf.Max(flashTimer, flashDuration);
            }

            previousHealth = current;
        }

        private void OnDied(BDHealth deadHealth)
        {
            deathFlash = true;
            flashTimer = flashDuration * 1.65f;
        }

        private void ApplyFlash(Color color, float intensity)
        {
            if (renderers == null)
                return;

            Color emission = color * Mathf.Lerp(0.6f, 1.6f, intensity);

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

        private void ClearFlash()
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

        private void OnDestroy()
        {
            if (health != null)
            {
                health.HealthChanged -= OnHealthChanged;
                health.Died -= OnDied;
            }
        }
    }
}
