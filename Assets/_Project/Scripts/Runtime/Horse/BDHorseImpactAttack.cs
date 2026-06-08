using System.Collections.Generic;
using UnityEngine;

namespace BoredomAndDungeons
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(BDHorseController))]
    [RequireComponent(typeof(CharacterController))]
    public sealed class BDHorseImpactAttack : MonoBehaviour
    {
        // BD MOUNTED SMALL-ENEMY IMPACT DAMAGE V23R17
        [SerializeField] private float minimumImpactSpeed = 2.20f;
        [SerializeField] private float minimumDamage = 4f;
        [SerializeField] private float maximumDamage = 10f;
        [SerializeField] private float minimumKnockback = 4.5f;
        [SerializeField] private float maximumKnockback = 11.5f;
        [SerializeField] private float sameEnemyCooldown = 0.62f;
        [SerializeField] private float staggerDuration = 0.20f;

        private readonly Dictionary<int, float> nextImpactAt =
            new Dictionary<int, float>();

        private BDHorseController horse;

        private void Awake()
        {
            horse = GetComponent<BDHorseController>();
        }

        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            if (!Application.isPlaying ||
                horse == null ||
                !horse.IsMounted ||
                hit == null ||
                hit.collider == null)
            {
                return;
            }

            float speed = horse.CurrentMountedPlanarSpeed;
            if (speed < Mathf.Max(0.1f, minimumImpactSpeed))
                return;

            BDHealth enemyHealth =
                hit.collider.GetComponentInParent<BDHealth>();
            if (!BDEnemyHazardNavigation.IsSmallRegularEnemy(enemyHealth))
                return;

            int key = enemyHealth.GetInstanceID();
            if (nextImpactAt.TryGetValue(key, out float allowedAt) &&
                Time.time < allowedAt)
            {
                return;
            }

            Vector3 travel = horse.CurrentMountedTravelDirection;
            travel.y = 0f;
            if (travel.sqrMagnitude < 0.001f)
                travel = transform.forward;
            travel.Normalize();

            Vector3 contactNormal = hit.normal;
            contactNormal.y = 0f;
            if (contactNormal.sqrMagnitude < 0.001f)
                contactNormal = -travel;
            contactNormal.Normalize();

            float directness = Mathf.Clamp01(
                Vector3.Dot(travel, -contactNormal)
            );
            float speed01 = Mathf.InverseLerp(
                Mathf.Max(0.1f, minimumImpactSpeed),
                Mathf.Max(minimumImpactSpeed + 0.1f, horse.MountedMaximumMoveSpeed),
                speed
            );

            // A glancing rub still lands near the minimum. A fast, square hit
            // reaches the full ten damage.
            float impact01 = Mathf.Clamp01(
                speed01 * Mathf.Lerp(0.34f, 1f, directness)
            );
            float damage = Mathf.Lerp(
                Mathf.Max(0f, minimumDamage),
                Mathf.Max(minimumDamage, maximumDamage),
                impact01
            );

            Vector3 away = enemyHealth.transform.position - transform.position;
            away.y = 0f;
            if (away.sqrMagnitude < 0.001f)
                away = -contactNormal;
            away.Normalize();

            Vector3 knockDirection = Vector3.Slerp(
                away,
                travel,
                Mathf.Lerp(0.28f, 0.72f, directness)
            );
            knockDirection.y = 0f;
            if (knockDirection.sqrMagnitude < 0.001f)
                knockDirection = travel;
            knockDirection.Normalize();

            enemyHealth.ApplyDamage(damage);

            BDKnockbackReceiver knockback =
                enemyHealth.GetComponent<BDKnockbackReceiver>();
            if (knockback == null)
                knockback = enemyHealth.gameObject.AddComponent<BDKnockbackReceiver>();

            BDEnemyHazardNavigation navigation =
                enemyHealth.GetComponent<BDEnemyHazardNavigation>();
            navigation?.NotifyForcedHazardEntry(1.10f);

            float knockStrength = Mathf.Lerp(
                Mathf.Max(0f, minimumKnockback),
                Mathf.Max(minimumKnockback, maximumKnockback),
                impact01
            );
            knockback.AddKnockback(
                knockDirection,
                knockStrength,
                0.24f
            );

            BDHitStaggerReceiver stagger =
                enemyHealth.GetComponent<BDHitStaggerReceiver>();
            stagger?.RequestStagger(staggerDuration);

            BDEnemyMotionStabilizer stabilizer =
                enemyHealth.GetComponent<BDEnemyMotionStabilizer>();
            stabilizer?.AcceptCurrentPositionAsBaseline();

            nextImpactAt[key] =
                Time.time + Mathf.Max(0.15f, sameEnemyCooldown);
        }
    }
}
