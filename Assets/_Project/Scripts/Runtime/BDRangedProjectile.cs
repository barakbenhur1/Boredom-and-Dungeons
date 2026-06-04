using UnityEngine;

namespace BoredomAndDungeons
{
    [DisallowMultipleComponent]
    public sealed class BDRangedProjectile : MonoBehaviour
    {
        [SerializeField] private float speed = 9.5f;
        [SerializeField] private float damage = 15f;
        [SerializeField] private float lifetime = 3.5f;
        [SerializeField] private float hitRadius = 0.62f;
        [SerializeField] private float collisionRadius = 0.24f;
        [SerializeField] private float floorIgnoreNormalY = 0.68f;

        private static readonly RaycastHit[] CastHits = new RaycastHit[24];
        private static readonly Collider[] AreaHits = new Collider[24];

        private Transform owner;
        private Vector3 direction = Vector3.forward;
        private float timer;
        private bool destroyed;

        private void Awake()
        {
            BDRangedAttackVisuals.AddProjectileTrail(gameObject, playerProjectile: false);
        }

        public void Configure(Transform projectileOwner, Vector3 projectileDirection, float projectileSpeed, float projectileDamage)
        {
            owner = projectileOwner;

            projectileDirection.y = 0f;
            if (projectileDirection.sqrMagnitude < 0.001f)
                projectileDirection = Vector3.forward;

            direction = projectileDirection.normalized;
            speed = Mathf.Max(0.1f, projectileSpeed);
            damage = Mathf.Max(0f, projectileDamage);

            BDRangedAttackVisuals.AddProjectileTrail(gameObject, playerProjectile: false);
        }

        public void Configure(Vector3 projectileDirection, float projectileSpeed, float projectileDamage, Transform projectileOwner)
        {
            Configure(projectileOwner, projectileDirection, projectileSpeed, projectileDamage);
        }

        public void Configure(Vector3 projectileDirection, float projectileSpeed, float projectileDamage)
        {
            Configure(null, projectileDirection, projectileSpeed, projectileDamage);
        }

        private void Update()
        {
            if (destroyed)
                return;

            timer += Time.deltaTime;
            if (timer >= lifetime)
            {
                Destroy(gameObject);
                return;
            }

            Vector3 from = transform.position;
            Vector3 to = from + direction * speed * Time.deltaTime;

            if (TryResolveHit(from, to))
                return;

            transform.position = to;
        }

        private bool TryResolveHit(Vector3 from, Vector3 to)
        {
            Vector3 travel = to - from;
            float travelDistance = travel.magnitude;

            if (travelDistance <= 0.0001f)
                return false;

            Vector3 castDirection = travel / travelDistance;
            float castRadius = Mathf.Clamp(collisionRadius, 0.08f, Mathf.Max(0.08f, hitRadius * 0.60f));

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
                if (health != null && !health.IsDead && IsValidDamageTarget(health))
                {
                    ResolveDamageHit(health, hit.point);
                    return true;
                }

                if (IsBlockingWorldCollider(hit))
                {
                    ResolveWorldHit(hit.point);
                    return true;
                }
            }

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
                if (health == null || health.IsDead || !IsValidDamageTarget(health))
                    continue;

                ResolveDamageHit(health, hit.ClosestPoint(to));
                return true;
            }

            return false;
        }

        private void ResolveDamageHit(BDHealth health, Vector3 hitPoint)
        {
            if (destroyed)
                return;

            destroyed = true;

            health.ApplyDamage(damage);
            BDRangedAttackVisuals.SpawnImpactBurst(hitPoint, playerProjectile: false);

            Destroy(gameObject);
        }

        private void ResolveWorldHit(Vector3 hitPoint)
        {
            if (destroyed)
                return;

            destroyed = true;

            BDRangedAttackVisuals.SpawnImpactBurst(hitPoint, playerProjectile: false);

            Destroy(gameObject);
        }

        private bool ShouldIgnoreCollider(Collider collider)
        {
            if (collider == null)
                return true;

            if (owner != null && (collider.transform == owner || collider.transform.IsChildOf(owner) || owner.IsChildOf(collider.transform)))
                return true;

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

        private bool IsValidDamageTarget(BDHealth health)
        {
            if (health == null || health.IsDead)
                return false;

            // Enemy bullets hit player and horse, but do not damage other enemies.
            if (health.GetComponent<BDPlayerMarker>() != null)
                return true;

            if (health.GetComponent<BDHorseHealth>() != null)
                return true;

            return false;
        }

        private bool IsBlockingWorldCollider(RaycastHit hit)
        {
            Collider collider = hit.collider;

            if (collider == null)
                return false;

            if (collider.isTrigger)
                return false;

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
