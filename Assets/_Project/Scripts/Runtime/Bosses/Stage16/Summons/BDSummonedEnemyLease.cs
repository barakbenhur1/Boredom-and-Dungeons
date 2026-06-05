using UnityEngine;

namespace BoredomAndDungeons
{
    [DisallowMultipleComponent]
    public sealed class BDSummonedEnemyLease : MonoBehaviour
    {
        [SerializeField] private BDHealth health;

        private BDBossSharedSummonBudget budget;
        private bool released;

        public BDBossSharedSummonBudget Budget => budget;
        public BDHealth Health => health;
        public bool IsReleased => released;

        public void Bind(
            BDBossSharedSummonBudget newBudget,
            BDHealth newHealth)
        {
            UnsubscribeHealth();

            budget = newBudget;
            health = newHealth;
            released = false;

            SubscribeHealth();
        }

        public bool IsBoundTo(
            BDBossSharedSummonBudget expectedBudget)
        {
            return !released &&
                   budget != null &&
                   budget == expectedBudget;
        }

        public void ReleaseFromBudget(bool destroyInstance)
        {
            ReleaseOnce();

            if (destroyInstance && gameObject != null)
                Destroy(gameObject);
        }

        private void OnDestroy()
        {
            ReleaseOnce();
        }

        private void HandleDied(BDHealth deadHealth)
        {
            ReleaseOnce();
        }

        private void SubscribeHealth()
        {
            if (health == null)
                return;

            health.Died -= HandleDied;
            health.Died += HandleDied;
        }

        private void UnsubscribeHealth()
        {
            if (health != null)
                health.Died -= HandleDied;
        }

        private void ReleaseOnce()
        {
            if (released)
                return;

            released = true;
            UnsubscribeHealth();

            BDBossSharedSummonBudget previousBudget = budget;
            budget = null;

            if (previousBudget != null)
                previousBudget.ReleaseLease(this);
        }
    }
}
