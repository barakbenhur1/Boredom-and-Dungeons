using UnityEngine;

namespace BoredomAndDungeons
{
    [DisallowMultipleComponent]
    public sealed class BDEnemyAttackTelegraph : MonoBehaviour
    {
        [SerializeField] private float meleeCooldown = 0.45f;
        [SerializeField] private float rangedCooldown = 0.55f;
        [SerializeField] private float meleeRadius = 1.35f;
        [SerializeField] private float rangedRadius = 1.65f;

        private float nextMeleeAllowedAt;
        private float nextRangedAllowedAt;

        public void ShowMelee(Vector3 direction, float duration = 0.18f)
        {
            if (BDMountedRunIntro.IsGameplayInputLocked)
                return;

            // Every direct physical attack reports through the same shared path.
            // This includes swords, charges, jumps, bites, stomps, hands, tails,
            // rolling body attacks and future physical boss attacks.
            // The signal is intentionally reported even when the visual telegraph
            // is still on cooldown, because the damage/parry event must never be lost.
            ReportPhysicalImpact();

            if (Time.time < nextMeleeAllowedAt)
                return;

            nextMeleeAllowedAt = Time.time + meleeCooldown;

            Vector3 position = transform.position + ResolveFlatDirection(direction) * 0.88f;
            BDEnemyAttackTelegraphVisual.Spawn(position, direction, meleeRadius, duration, ranged: false);
        }

        public void ShowRanged(Vector3 direction, float duration = 0.20f)
        {
            // Ranged attacks are not parryable by the physical-parry system.
            if (Time.time < nextRangedAllowedAt)
                return;

            nextRangedAllowedAt = Time.time + rangedCooldown;

            Vector3 position = transform.position + ResolveFlatDirection(direction) * 0.70f;
            BDEnemyAttackTelegraphVisual.Spawn(position, direction, rangedRadius, duration, ranged: true);
        }

        public void ReportPhysicalImpact()
        {
            BDPhysicalAttackSignal.Report(transform);
        }

        private Vector3 ResolveFlatDirection(Vector3 direction)
        {
            direction.y = 0f;

            if (direction.sqrMagnitude < 0.001f)
                direction = transform.forward;

            direction.y = 0f;

            if (direction.sqrMagnitude < 0.001f)
                return Vector3.forward;

            return direction.normalized;
        }
    }
}
