using System.Collections;
using UnityEngine;

namespace BoredomAndDungeons
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(BDHorseHealth))]
    [RequireComponent(typeof(BDHorseController))]
    public sealed class BDHorseBuckRepair : MonoBehaviour
    {
        [SerializeField] private float fallbackCheckDelay = 0.05f;

        private BDHorseHealth horseHealth;
        private BDHorseController horseController;
        private Coroutine fallbackRoutine;

        private void Awake()
        {
            horseHealth = GetComponent<BDHorseHealth>();
            horseController = GetComponent<BDHorseController>();
        }

        private void OnEnable()
        {
            if (horseHealth != null)
            {
                horseHealth.DamageBurstTriggered -= HandleDamageBurst;
                horseHealth.DamageBurstTriggered += HandleDamageBurst;
            }
        }

        private void OnDisable()
        {
            if (horseHealth != null)
                horseHealth.DamageBurstTriggered -= HandleDamageBurst;

            if (fallbackRoutine != null)
            {
                StopCoroutine(fallbackRoutine);
                fallbackRoutine = null;
            }
        }

        private void HandleDamageBurst(BDHorseHealth health)
        {
            if (horseController == null || !horseController.IsMounted)
                return;

            if (fallbackRoutine != null)
                StopCoroutine(fallbackRoutine);

            fallbackRoutine = StartCoroutine(EnsureDismountedAfterBurst());
        }

        private IEnumerator EnsureDismountedAfterBurst()
        {
            float delay = Mathf.Max(0f, fallbackCheckDelay);
            if (delay > 0f)
                yield return new WaitForSeconds(delay);

            // BDHorseController normally performs the full buck/throw animation itself.
            // This is only a safety fallback in case another component interrupted that handler.
            if (horseController != null && horseController.IsMounted)
                horseController.ForceDismountForCombat();

            fallbackRoutine = null;
        }
    }
}
