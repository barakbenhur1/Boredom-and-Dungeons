using System.Collections;
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

        [Header("BD BOOST DROPS")]
        [Range(0f, 1f)]
        [SerializeField] private float regularEnemyBoostDropChance = 0.02f;
        [Range(0f, 1f)]
        [SerializeField] private float miniBossBoostDropChance = 0.12f;
        [SerializeField] private float boostSpawnHeight = 0.70f;

        private BDHealth health;
        private bool dropped;
        private bool boostDropResolved;

        private void Awake()
        {
            health = GetComponent<BDHealth>();

            if (health != null)
                health.Died += OnDied;
        }

        private void OnDied(BDHealth deadHealth)
        {
            StartCoroutine(
                DropAfterDeathAnimation(deadHealth)
            );
        }

        private IEnumerator DropAfterDeathAnimation(
            BDHealth deadHealth)
        {
            float remaining =
                BDCharacterDeathAnimation.GetEnemyDeathDuration(deadHealth);

            while (remaining > 0f)
            {
                remaining -= Time.unscaledDeltaTime;
                yield return null;
            }

            TryDropHeal();
            TryDropBoost(deadHealth);
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


        private void TryDropBoost(BDHealth deadHealth)
        {
            if (boostDropResolved)
                return;

            boostDropResolved = true;

            BDCombatantRank rank =
                BDCombatantProfile.ResolveRank(deadHealth);

            if (rank == BDCombatantRank.Boss)
                return;

            float dropChance =
                rank == BDCombatantRank.MiniBoss
                    ? miniBossBoostDropChance
                    : regularEnemyBoostDropChance;

            if (Random.value > Mathf.Clamp01(dropChance))
                return;

            Transform player = BDTargetFinder.FindPlayer();

            if (player == null)
            {
                BDPlayerMarker marker =
                    FindFirstObjectByType<BDPlayerMarker>();

                if (marker != null)
                    player = marker.transform;
            }

            if (player == null)
                return;

            BDPlayerBoostState boostState =
                player.GetComponent<BDPlayerBoostState>();

            if (boostState == null)
            {
                boostState =
                    player.gameObject.AddComponent<BDPlayerBoostState>();
            }

            if (!boostState.TryChooseRandomAvailable(
                    out BDPlayerBoostType boostType))
                return;

            Vector3 spawnPosition = transform.position;
            spawnPosition.y = boostSpawnHeight;

            BDPlayerBoostPickup.Spawn(spawnPosition, boostType);
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
