using UnityEngine;

namespace BoredomAndDungeons
{
    [DisallowMultipleComponent]
    public sealed class BDSquareJumperProjectile : MonoBehaviour
    {
        private Vector3 direction;
        private float speed;
        private float damage;
        private float hitRadius;
        private float expiresAt;
        private Transform owner;
        private BDBossEncounterController encounter;
        private bool resolved;

        public static BDSquareJumperProjectile Spawn(
            Vector3 position,
            Vector3 direction,
            float speed,
            float damage,
            float hitRadius,
            float lifetime,
            Transform owner,
            BDBossEncounterController encounter,
            bool enraged)
        {
            GameObject projectile =
                GameObject.CreatePrimitive(
                    PrimitiveType.Sphere
                );

            projectile.name =
                enraged
                    ? "BD_SquareJumper_EnragedBullet"
                    : "BD_SquareJumper_Bullet";

            projectile.transform.position = position;
            projectile.transform.localScale =
                Vector3.one *
                Mathf.Max(
                    0.16f,
                    hitRadius * 1.55f
                );

            Collider collider =
                projectile.GetComponent<Collider>();

            if (collider != null)
                Destroy(collider);

            BDSquareJumperProjectile logic =
                projectile.AddComponent<
                    BDSquareJumperProjectile
                >();

            logic.Configure(
                direction,
                speed,
                damage,
                hitRadius,
                lifetime,
                owner,
                encounter,
                enraged
            );

            return logic;
        }

        private void Configure(
            Vector3 newDirection,
            float newSpeed,
            float newDamage,
            float newHitRadius,
            float lifetime,
            Transform newOwner,
            BDBossEncounterController newEncounter,
            bool enraged)
        {
            newDirection.y = 0f;

            direction =
                newDirection.sqrMagnitude > 0.001f
                    ? newDirection.normalized
                    : Vector3.forward;

            speed = Mathf.Max(0.1f, newSpeed);
            damage = Mathf.Max(0f, newDamage);
            hitRadius = Mathf.Max(0.08f, newHitRadius);
            expiresAt =
                Time.time + Mathf.Max(0.1f, lifetime);
            owner = newOwner;
            encounter = newEncounter;

            Color color =
                enraged
                    ? new Color(
                        1f,
                        0.14f,
                        0.04f,
                        1f
                    )
                    : new Color(
                        1f,
                        0.58f,
                        0.08f,
                        1f
                    );

            Renderer renderer =
                GetComponent<Renderer>();

            if (renderer != null)
            {
                Material material = renderer.material;
                material.color = color;

                if (material.HasProperty("_BaseColor"))
                    material.SetColor("_BaseColor", color);

                if (material.HasProperty("_Color"))
                    material.SetColor("_Color", color);

                if (material.HasProperty("_EmissionColor"))
                {
                    material.EnableKeyword("_EMISSION");
                    material.SetColor(
                        "_EmissionColor",
                        color * (enraged ? 4f : 2.6f)
                    );
                }
            }

            TrailRenderer trail =
                gameObject.AddComponent<TrailRenderer>();

            trail.time = enraged ? 0.28f : 0.20f;
            trail.startWidth =
                Mathf.Max(0.08f, hitRadius * 1.2f);
            trail.endWidth = 0f;
            trail.minVertexDistance = 0.035f;

            if (renderer != null)
                trail.material = renderer.material;

            Gradient gradient = new Gradient();

            gradient.SetKeys(
                new[]
                {
                    new GradientColorKey(
                        Color.white,
                        0f
                    ),
                    new GradientColorKey(
                        color,
                        0.25f
                    ),
                    new GradientColorKey(
                        color * 0.45f,
                        1f
                    )
                },
                new[]
                {
                    new GradientAlphaKey(1f, 0f),
                    new GradientAlphaKey(0.78f, 0.4f),
                    new GradientAlphaKey(0f, 1f)
                }
            );

            trail.colorGradient = gradient;
        }

        private void Update()
        {
            if (resolved)
                return;

            if (Time.time >= expiresAt ||
                (encounter != null &&
                 !encounter.IsCombatActive))
            {
                ResolveWithoutDamage();
                return;
            }

            Vector3 start = transform.position;
            Vector3 delta =
                direction * speed * Time.deltaTime;
            Vector3 end = start + delta;

            if (TryHitPlayerOrHorse(end))
                return;

            if (TryHitObstacle(start, delta))
                return;

            transform.position = end;
        }

        private bool TryHitPlayerOrHorse(Vector3 position)
        {
            Collider[] hits = Physics.OverlapSphere(
                position,
                hitRadius,
                ~0,
                QueryTriggerInteraction.Ignore
            );

            for (int i = 0; i < hits.Length; i++)
            {
                Collider hit = hits[i];

                if (hit == null ||
                    IsOwnedCollider(hit.transform))
                {
                    continue;
                }

                BDPlayerMarker player =
                    hit.GetComponentInParent<BDPlayerMarker>();

                if (player != null)
                {
                    BDHealth health =
                        player.GetComponent<BDHealth>();

                    if (health != null)
                        health.ApplyDamage(damage);

                    ResolveImpact();
                    return true;
                }

                BDHorseHealth horse =
                    hit.GetComponentInParent<BDHorseHealth>();

                if (horse != null && !horse.IsFainted)
                {
                    horse.ApplyDamage(damage);
                    ResolveImpact();
                    return true;
                }
            }

            return false;
        }

        private bool TryHitObstacle(
            Vector3 start,
            Vector3 delta)
        {
            float distance = delta.magnitude;

            if (distance <= 0.001f)
                return false;

            RaycastHit[] hits = Physics.SphereCastAll(
                start,
                hitRadius * 0.65f,
                delta.normalized,
                distance,
                ~0,
                QueryTriggerInteraction.Ignore
            );

            for (int i = 0; i < hits.Length; i++)
            {
                Transform hitTransform =
                    hits[i].collider != null
                        ? hits[i].collider.transform
                        : null;

                if (hitTransform == null ||
                    IsOwnedCollider(hitTransform))
                {
                    continue;
                }

                if (hitTransform.GetComponentInParent<
                        BDPlayerMarker>() != null ||
                    hitTransform.GetComponentInParent<
                        BDHorseHealth>() != null)
                {
                    continue;
                }

                BDHealth otherHealth =
                    hitTransform.GetComponentInParent<
                        BDHealth>();

                if (otherHealth != null)
                {
                    // Enemy bullets pass through other enemies.
                    continue;
                }

                transform.position = hits[i].point;
                ResolveImpact();
                return true;
            }

            return false;
        }

        private bool IsOwnedCollider(Transform candidate)
        {
            if (owner == null || candidate == null)
                return false;

            return candidate == owner ||
                   candidate.IsChildOf(owner);
        }

        private void ResolveImpact()
        {
            if (resolved)
                return;

            resolved = true;

            BDSquareJumperVisuals.SpawnProjectileImpact(
                transform.position,
                transform.localScale.x
            );

            Destroy(gameObject);
        }

        private void ResolveWithoutDamage()
        {
            if (resolved)
                return;

            resolved = true;
            Destroy(gameObject);
        }
    }
}
