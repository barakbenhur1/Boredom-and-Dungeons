using UnityEngine;

namespace BoredomAndDungeons
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Collider))]
    public sealed class BDBossArenaTrigger : MonoBehaviour
    {
        [SerializeField] private BDBossEncounterController encounter;
        [SerializeField] private bool triggerOnlyOnce = true;
        [SerializeField] private bool disableColliderAfterTrigger = true;

        private Collider triggerCollider;
        private bool triggered;

        public bool HasTriggered => triggered;

        private void Awake()
        {
            triggerCollider = GetComponent<Collider>();

            if (triggerCollider != null)
                triggerCollider.isTrigger = true;

            ResolveEncounter();
        }

        private void Reset()
        {
            Collider collider = GetComponent<Collider>();

            if (collider != null)
                collider.isTrigger = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other == null)
                return;

            if (triggerOnlyOnce && triggered)
                return;

            if (other.GetComponentInParent<BDPlayerMarker>() == null)
                return;

            ResolveEncounter();

            if (encounter == null ||
                encounter.State != BDBossEncounterState.Dormant)
                return;

            triggered = true;
            encounter.BeginEncounter();

            if (disableColliderAfterTrigger && triggerCollider != null)
                triggerCollider.enabled = false;
        }

        public void ResetTrigger()
        {
            triggered = false;

            if (triggerCollider == null)
                triggerCollider = GetComponent<Collider>();

            if (triggerCollider != null)
            {
                triggerCollider.isTrigger = true;
                triggerCollider.enabled = true;
            }
        }

        public void Configure(BDBossEncounterController newEncounter)
        {
            encounter = newEncounter;
        }

        private void ResolveEncounter()
        {
            if (encounter != null)
                return;

            encounter =
                GetComponentInParent<BDBossEncounterController>();

            if (encounter == null)
            {
                encounter =
                    FindFirstObjectByType<BDBossEncounterController>();
            }
        }
    }
}
