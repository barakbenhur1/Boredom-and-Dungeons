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
            if (Time.time < nextMeleeAllowedAt)
                return;

            nextMeleeAllowedAt = Time.time + meleeCooldown;

            Vector3 position = transform.position + ResolveFlatDirection(direction) * 0.88f;
            BDEnemyAttackTelegraphVisual.Spawn(position, direction, meleeRadius, duration, ranged: false);
        }

        public void ShowRanged(Vector3 direction, float duration = 0.20f)
        {
            if (Time.time < nextRangedAllowedAt)
                return;

            nextRangedAllowedAt = Time.time + rangedCooldown;

            Vector3 position = transform.position + ResolveFlatDirection(direction) * 0.70f;
            BDEnemyAttackTelegraphVisual.Spawn(position, direction, rangedRadius, duration, ranged: true);
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
