using System;
using UnityEngine;

namespace BoredomAndDungeons
{
    [Serializable]
    public sealed class BDBossPhaseThreshold
    {
        [Min(0)] public int phaseIndex = 1;
        [Range(0f, 1f)] public float enterAtOrBelowNormalizedHealth = 0.6f;
    }

    [DisallowMultipleComponent]
    public sealed class BDBossPhaseController : MonoBehaviour
    {
        [SerializeField] private BDBossEncounterController encounter;
        [SerializeField] private BDBossHealthChannel sourceHealth;
        [SerializeField] private BDBossPhaseThreshold[] thresholds = Array.Empty<BDBossPhaseThreshold>();
        [SerializeField] private bool evaluateOnEnable = true;

        private void Awake()
        {
            if (encounter == null)
                encounter = GetComponent<BDBossEncounterController>();

            if (sourceHealth == null)
                sourceHealth = GetComponentInChildren<BDBossHealthChannel>();
        }

        private void OnEnable()
        {
            if (sourceHealth != null)
                sourceHealth.HealthChanged += HandleHealthChanged;

            if (evaluateOnEnable)
                EvaluateCurrentHealth();
        }

        private void OnDisable()
        {
            if (sourceHealth != null)
                sourceHealth.HealthChanged -= HandleHealthChanged;
        }

        public void EvaluateCurrentHealth()
        {
            if (sourceHealth == null)
                return;

            Evaluate(sourceHealth.NormalizedHealth);
        }

        private void HandleHealthChanged(BDBossHealthChannel channel, float current, float maximum)
        {
            float normalized = maximum <= 0f ? 0f : Mathf.Clamp01(current / maximum);
            Evaluate(normalized);
        }

        private void Evaluate(float normalizedHealth)
        {
            if (encounter == null || encounter.IsFinished)
                return;

            int requestedPhase = encounter.PhaseIndex;

            for (int i = 0; i < thresholds.Length; i++)
            {
                BDBossPhaseThreshold threshold = thresholds[i];
                if (threshold == null)
                    continue;

                if (normalizedHealth <= threshold.enterAtOrBelowNormalizedHealth)
                    requestedPhase = Mathf.Max(requestedPhase, threshold.phaseIndex);
            }

            if (requestedPhase > encounter.PhaseIndex && encounter.State != BDBossEncounterState.Transition)
                encounter.BeginTransition(requestedPhase);
        }
    }
}
