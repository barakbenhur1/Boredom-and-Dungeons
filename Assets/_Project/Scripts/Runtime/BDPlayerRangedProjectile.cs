using UnityEngine;

namespace BoredomAndDungeons
{
    [DisallowMultipleComponent]
    public sealed class BDPlayerRangedProjectile : MonoBehaviour
    {
        [SerializeField] private float collisionRadius = 0.28f;
        [SerializeField] private float floorIgnoreNormalY = 0.68f;
        [SerializeField] private float hitCameraShakeStrength = 0.14f;
        [SerializeField] private float hitCameraShakeDuration = 0.09f;
        [SerializeField] private float hitStopDuration = 0.025f;
        [SerializeField] private float hitStopTimeScale = 0.45f;
        [SerializeField] private float rangedHitStaggerDuration = 0.055f;
        [SerializeField] private float rangedKnockbackLockDuration = 0.08f;

        private static readonly RaycastHit[] CastHits = new RaycastHit[32];
        private static readonly Collider[] AreaHits = new Collider[32];

        private Vector3 direction = Vector3.forward;
        private Vector3 previousPosition;
        private Transform owner;
        private float speed = 16f;
        private float damage = 75f;
        private float lifetime = 4.5f;
        private float hitRadius = 1.05f;
        private float knockback = 9f;
        private float age;
        private bool configured;
        private bool destroyed;

        public void Configure(Vector3 projectileDirection, float projectileSpeed, float projectileDamage, float projectileLifetime, float projectileHitRadius, float projectileKnockback, Transform projectileOwner)
        {
            projectileDirection.y = 0f;

            if (projectileDirection.sqrMagnitude < 0.001f)
                projectileDirection = Vector3.forward;

            direction = projectileDirection.normalized;
            speed = Mathf.Max(0.1f, projectileSpeed);
            damage = Mathf.Max(0f, projectileDamage);
            lifetime = Mathf.Max(0.1f, projectileLifetime);
            hitRadius = Mathf.Max(0.05f, projectileHitRadius);
            knockback = Mathf.Max(0f, projectileKnockback);
            owner = projectileOwner;
            previousPosition = transform.position;
            configured = true;
        }

        private void Awake()
        {
            previousPosition = transform.position;
        }

        private void Update()
        {
            if (destroyed)
                return;

            if (!configured)
                previousPosition = transform.position;

            age += Time.deltaTime;
            if (age >= lifetime)
            {
                Destroy(gameObject);
                return;
            }

            Vector3 from = transform.position;
            Vector3 to = from + direction * speed * Time.deltaTime;

            if (TryResolveHit(from, to))
                return;

            transform.position = to;
            previousPosition = to;
        }

        private bool TryResolveHit(Vector3 from, Vector3 to)
        {
            Vector3 travel = to - from;
            float travelDistance = travel.magnitude;

            if (travelDistance <= 0.0001f)
                return false;

            Vector3 castDirection = travel / travelDistance;
            float castRadius = Mathf.Clamp(collisionRadius, 0.08f, Mathf.Max(0.08f, hitRadius * 0.55f));

            int hitCount = Physics.SphereCastNonAlloc(
                from,
                castRadius,
                castDirection,
                CastHits,
                travelDistance,
                ~0,
                QueryTriggerInteraction.Ignore
            );

            SortHitsByDistance(CastHits, hitCount);

            for (int i = 0; i < hitCount; i++)
            {
                RaycastHit hit = CastHits[i];

                if (hit.collider == null)
                    continue;

                if (ShouldIgnoreCollider(hit.collider))
                    continue;

                BDHealth health = hit.collider.GetComponentInParent<BDHealth>();
                if (health != null && !health.IsDead && IsValidDamageTarget(health, hit.collider))
                {
                    ResolveDamageHit(health, hit.collider, hit.point);
                    return true;
                }

                if (IsBlockingWorldCollider(hit))
                {
                    ResolveWorldHit(hit.point);
                    return true;
                }
            }

            // Area check keeps the forgiving hit radius against enemies without using that full radius for wall collision.
            int areaCount = Physics.OverlapCapsuleNonAlloc(
                from,
                to,
                hitRadius,
                AreaHits,
                ~0,
                QueryTriggerInteraction.Ignore
            );

            for (int i = 0; i < areaCount; i++)
            {
                Collider hit = AreaHits[i];

                if (hit == null)
                    continue;

                if (ShouldIgnoreCollider(hit))
                    continue;

                BDHealth health = hit.GetComponentInParent<BDHealth>();
                if (health == null || health.IsDead || !IsValidDamageTarget(health, hit))
                    continue;

                Vector3 point = hit.ClosestPoint(to);
                ResolveDamageHit(health, hit, point);
                return true;
            }

            return false;
        }

        private void ResolveDamageHit(BDHealth health, Collider hitCollider, Vector3 hitPoint)
        {
            if (destroyed)
                return;

            destroyed = true;

            health.ApplyDamage(damage);
            RequestEnemyHitStagger(health, rangedHitStaggerDuration);
            RequestEnemyRangedHitFlash(health);

            // BD PROJECTILE KNOCKBACK POLICY:
            // Damage always applies. Knockback applies only when the target profile allows it.
            if (BDCombatantProfile.CanReceivePlayerProjectileKnockback(health))
            {
                Vector3 knockDirection = direction;
                knockDirection.y = 0f;

                if (knockDirection.sqrMagnitude < 0.001f)
                {
                    knockDirection = health.transform.position - transform.position;
                    knockDirection.y = 0f;
                }

                BDKnockbackReceiver receiver =
                    health.GetComponent<BDKnockbackReceiver>();

                if (receiver == null &&
                    health.GetComponent<CharacterController>() != null)
                {
                    receiver =
                        health.gameObject.AddComponent<BDKnockbackReceiver>();
                }

                if (receiver != null)
                {
                    receiver.AddKnockback(
                        knockDirection,
                        knockback,
                        rangedKnockbackLockDuration
                    );
                }
                else
                {
                    Rigidbody body = health.GetComponent<Rigidbody>();

                    if (body != null && !body.isKinematic)
                    {
                        body.AddForce(
                            knockDirection.normalized * knockback,
                            ForceMode.VelocityChange
                        );
                    }
                }
            }

            BDGameFeelEvents.RequestCameraShake(hitCameraShakeStrength, hitCameraShakeDuration);
            BDHitStop.Request(hitStopDuration, hitStopTimeScale);
            BDGameFeelAudio.PlayRangedHit();
            BDRangedAttackVisuals.SpawnImpactBurst(hitPoint, playerProjectile: true);

            Destroy(gameObject);
        }



        private void RequestEnemyRangedHitFlash(BDHealth health)
        {
            if (health == null || health.IsDead)
                return;

            BDEnemyHitFlashReceiver flash = health.GetComponent<BDEnemyHitFlashReceiver>();
            if (flash == null && health.GetComponent<CharacterController>() != null)
                flash = health.gameObject.AddComponent<BDEnemyHitFlashReceiver>();

            if (flash != null)
                flash.FlashRanged();
        }

        private void RequestEnemyHitStagger(BDHealth health, float duration)
        {
            if (health == null || health.IsDead)
                return;

            BDHitStaggerReceiver stagger = health.GetComponent<BDHitStaggerReceiver>();
            if (stagger == null && health.GetComponent<CharacterController>() != null)
                stagger = health.gameObject.AddComponent<BDHitStaggerReceiver>();

            if (stagger != null)
                stagger.RequestStagger(duration);
        }

        private void ResolveWorldHit(Vector3 hitPoint)
        {
            if (destroyed)
                return;

            destroyed = true;

            BDRangedAttackVisuals.SpawnImpactBurst(hitPoint, playerProjectile: true);
            BDGameFeelAudio.PlayRangedHit();

            Destroy(gameObject);
        }

        private bool ShouldIgnoreCollider(Collider collider)
        {
            if (collider == null)
                return true;

            if (owner != null && (collider.transform == owner || collider.transform.IsChildOf(owner) || owner.IsChildOf(collider.transform)))
                return true;

            // Player projectiles should not collide with player or horse.
            if (collider.GetComponentInParent<BDPlayerMarker>() != null)
                return true;

            if (collider.GetComponentInParent<BDHorseHealth>() != null)
                return true;

            // Ignore pure VFX / UI helpers.
            if (collider.GetComponentInParent<BDMeleeImpactBurst>() != null)
                return true;

            if (collider.GetComponentInParent<BDRangedMuzzleFlashVisual>() != null)
                return true;

            if (collider.GetComponentInParent<BDRangedImpactBurstVisual>() != null)
                return true;

            if (collider.GetComponentInParent<BDDeathBurstVisual>() != null)
                return true;

            if (collider.GetComponentInParent<BDHealingPickupCollectBurst>() != null)
                return true;

            if (collider.GetComponentInParent<BDPlayerHealingPickup>() != null)
                return true;

            return false;
        }

        private bool IsValidDamageTarget(BDHealth health, Collider collider)
        {
            if (health == null || health.IsDead)
                return false;

            if (health.GetComponent<BDPlayerMarker>() != null)
                return false;

            if (health.GetComponent<BDHorseHealth>() != null)
                return false;

            return true;
        }

        private bool IsBlockingWorldCollider(RaycastHit hit)
        {
            Collider collider = hit.collider;

            if (collider == null)
                return false;

            if (collider.isTrigger)
                return false;

            // Do not explode on floors/ground while flying horizontally.
            if (hit.normal.y >= floorIgnoreNormalY)
                return false;

            if (collider.GetComponentInParent<BDHealth>() != null)
                return false;

            return true;
        }

        private static void SortHitsByDistance(RaycastHit[] hits, int count)
        {
            for (int i = 1; i < count; i++)
            {
                RaycastHit key = hits[i];
                float distance = key.distance;
                int j = i - 1;

                while (j >= 0 && hits[j].distance > distance)
                {
                    hits[j + 1] = hits[j];
                    j--;
                }

                hits[j + 1] = key;
            }
        }
    }
}
