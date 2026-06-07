using UnityEngine;

namespace BoredomAndDungeons
{
    [DisallowMultipleComponent]
    public sealed class BDEnemyProximityTelegraph : MonoBehaviour
    {
        [SerializeField] private float closeThreatDistance = 2.25f;
        [SerializeField] private float rangedThreatDistance = 8.0f;
        [SerializeField] private float closeTelegraphCooldown = 1.10f;
        [SerializeField] private float rangedTelegraphCooldown = 1.45f;
        [SerializeField] private bool useRangedStyleForShooters = true;

        private BDHealth health;
        private BDEnemyAttackTelegraph telegraph;
        private float nextAllowedAt;

        private void Awake()
        {
            health = GetComponent<BDHealth>();
            telegraph = GetComponent<BDEnemyAttackTelegraph>();

            if (telegraph == null)
                telegraph = gameObject.AddComponent<BDEnemyAttackTelegraph>();
        }

        private void Update()
        {
            if (BDMountedRunIntro.IsGameplayInputLocked)
                return;

            if (health != null && health.IsDead)
                return;

            if (Time.time < nextAllowedAt)
                return;

            Transform player = BDTargetFinder.FindPlayer();
            if (player == null)
                return;

            Vector3 toPlayer = player.position - transform.position;
            toPlayer.y = 0f;

            float distance = toPlayer.magnitude;
            if (distance <= 0.001f)
                return;

            bool rangedStyle = useRangedStyleForShooters && IsRangedLike();

            if (rangedStyle)
            {
                if (distance > rangedThreatDistance)
                    return;

                nextAllowedAt = Time.time + rangedTelegraphCooldown;
                telegraph.ShowRanged(toPlayer, 0.18f);
            }
            else
            {
                if (distance > closeThreatDistance)
                    return;

                nextAllowedAt = Time.time + closeTelegraphCooldown;
                telegraph.ShowMelee(toPlayer, 0.16f);
            }
        }

        private bool IsRangedLike()
        {
            MonoBehaviour[] behaviours = GetComponents<MonoBehaviour>();
            for (int i = 0; i < behaviours.Length; i++)
            {
                MonoBehaviour behaviour = behaviours[i];
                if (behaviour == null)
                    continue;

                string typeName = behaviour.GetType().Name;
                if (typeName.Contains("Ranged") || typeName.Contains("Shooter"))
                    return true;
            }

            return false;
        }
    }
}
