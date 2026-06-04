using UnityEngine;

namespace BoredomAndDungeons
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(BDHealth))]
    public sealed class BDEnemyLootDropper : MonoBehaviour
    {
        [Header("Player Heal Drop")]
        [Range(0f, 1f)]
        [SerializeField] private float healDropChance = 0.16f;
        [SerializeField] private float playerHealFractionOfMax = 0.40f;
        [SerializeField] private float spawnHeight = 0.45f;

        private BDHealth health;
        private bool dropped;

        private void Awake()
        {
            health = GetComponent<BDHealth>();

            if (health != null)
                health.Died += OnDied;
        }

        private void OnDied(BDHealth deadHealth)
        {
            TryDropHeal();
        }

        private void TryDropHeal()
        {
            if (dropped)
                return;

            dropped = true;

            if (Random.value > healDropChance)
                return;

            Vector3 spawnPosition = transform.position;
            spawnPosition.y = spawnHeight;

            BDPlayerHealingPickup.Spawn(spawnPosition, playerHealFractionOfMax);
        }


        private void EnsureHealingPickupPolish(GameObject pickup)
        {
            if (pickup == null)
                return;

            if (pickup.GetComponent<BDPlayerHealingPickup>() == null)
                pickup.AddComponent<BDPlayerHealingPickup>();

            Collider collider = pickup.GetComponent<Collider>();
            if (collider != null)
                collider.isTrigger = true;
        }

        private void OnDestroy()
        {
            if (health != null)
                health.Died -= OnDied;
        }
    }
}
