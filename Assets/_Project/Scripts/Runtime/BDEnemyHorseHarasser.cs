using UnityEngine;

namespace BoredomAndDungeons
{
    [DisallowMultipleComponent]
    public sealed class BDEnemyHorseHarasser : MonoBehaviour
    {
        [SerializeField] private float horseAttackRange = 2.2f;
        [SerializeField] private float horseAttackDamage = 12f;
        [SerializeField] private float horseAttackCooldown = 0.9f;
        [SerializeField] private float onlyAttackHorseIfPlayerFartherThan = 1.2f;

        private Transform player;
        private BDHorseController horse;
        private BDHorseHealth horseHealth;
        private float cooldown;

        private void Start()
        {
            player = BDTargetFinder.FindPlayer();
            horse = FindFirstObjectByType<BDHorseController>();
            if (horse != null)
                horseHealth = horse.GetComponent<BDHorseHealth>();
        }

        private void Update()
        {
            if (cooldown > 0f)
                cooldown -= Time.deltaTime;

            if (player == null)
                player = BDTargetFinder.FindPlayer();

            if (horse == null)
            {
                horse = FindFirstObjectByType<BDHorseController>();
                if (horse != null)
                    horseHealth = horse.GetComponent<BDHorseHealth>();
            }

            if (horse == null || horseHealth == null || player == null)
                return;

            if (horse.IsMounted)
                return;

            if (cooldown > 0f)
                return;

            float distanceToHorse = Vector3.Distance(transform.position, horse.transform.position);
            if (distanceToHorse > horseAttackRange)
                return;

            float distanceToPlayer = Vector3.Distance(transform.position, player.position);
            if (distanceToPlayer <= onlyAttackHorseIfPlayerFartherThan)
                return;

            horseHealth.ApplyDamage(horseAttackDamage);
            cooldown = horseAttackCooldown;
        }
    }
}
