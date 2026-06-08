using System.Collections.Generic;
using UnityEngine;

namespace BoredomAndDungeons
{
    [DisallowMultipleComponent]
    public sealed class BDBombHazard : MonoBehaviour
    {
        [SerializeField] private float armTime = 0.55f;
        [SerializeField] private float explodeAfter = 1.65f;
        [SerializeField] private float explosionRadius = 2.25f;
        [SerializeField] private float damage = 24f;
        [SerializeField] private float enemyStaggerDuration = 0.38f;
        [SerializeField] private float enemyKnockbackStrength = 5.4f;

        private static readonly Collider[] HitBuffer = new Collider[96];
        private readonly HashSet<BDHealth> damagedHealth = new HashSet<BDHealth>();

        private float timer;
        private bool armed;
        private bool exploded;
        private Renderer cachedRenderer;
        private MaterialPropertyBlock colorBlock;
        private Transform owner;

        private static readonly int BaseColorId = Shader.PropertyToID("_BaseColor");
        private static readonly int ColorId = Shader.PropertyToID("_Color");
        private static readonly int EmissionColorId = Shader.PropertyToID("_EmissionColor");

        public void ConfigureOwner(Transform bombOwner)
        {
            owner = bombOwner;
        }

        private void Awake()
        {
            cachedRenderer = GetComponent<Renderer>();
            colorBlock = new MaterialPropertyBlock();
            ApplyColor(new Color(0.15f, 0.02f, 0.02f, 1f), 0.25f);
        }

        private void Update()
        {
            timer += Time.deltaTime;

            if (!armed && timer >= armTime)
            {
                armed = true;
                ApplyColor(new Color(1f, 0.35f, 0.05f, 1f), 1.8f);
            }

            float pulse = 1f + Mathf.Sin(Time.time * 12f) * 0.08f;
            transform.localScale = Vector3.one * 0.65f * pulse;

            if (timer >= explodeAfter)
                Explode();
        }

        // BD BOMB EXPLOSION + ENEMY FRIENDLY FIRE V23R11
        private void Explode()
        {
            if (exploded)
                return;
            exploded = true;

            Vector3 center = transform.position;
            float radius = Mathf.Max(0.25f, explosionRadius);

            BDBombExplosionVisual.Spawn(center, radius);
            BDGameFeelAudio.PlayBombExplosion();

            Transform player = BDTargetFinder.FindPlayer();
            if (player != null &&
                Vector3.Distance(player.position, center) <= radius + 4.5f)
            {
                BDGameFeelEvents.RequestCameraShake(0.24f, 0.18f);
            }

            damagedHealth.Clear();
            int count = Physics.OverlapSphereNonAlloc(
                center,
                radius,
                HitBuffer,
                ~0,
                QueryTriggerInteraction.Ignore
            );

            for (int i = 0; i < count; i++)
            {
                Collider hit = HitBuffer[i];
                HitBuffer[i] = null;
                if (hit == null)
                    continue;

                if (hit.GetComponentInParent<BDHorseHealth>() != null)
                    continue;

                BDHealth health = hit.GetComponentInParent<BDHealth>();
                if (health == null || health.IsDead || !damagedHealth.Add(health))
                    continue;

                if (IsOwnerHealth(health))
                    continue;

                health.ApplyDamage(damage);

                if (health.GetComponent<BDPlayerMarker>() != null)
                    continue;

                ApplyEnemyExplosionReaction(health, center);
            }

            BDHorseDamageUtility.TryDamageHorseNear(
                center,
                radius,
                damage,
                transform
            );

            Destroy(gameObject);
        }

        private bool IsOwnerHealth(BDHealth health)
        {
            if (owner == null || health == null)
                return false;

            Transform healthTransform = health.transform;
            return
                healthTransform == owner ||
                healthTransform.IsChildOf(owner) ||
                owner.IsChildOf(healthTransform);
        }

        private void ApplyEnemyExplosionReaction(
            BDHealth health,
            Vector3 center)
        {
            BDEnemyHitFlashReceiver flash =
                health.GetComponent<BDEnemyHitFlashReceiver>();
            if (flash == null && health.GetComponent<CharacterController>() != null)
                flash = health.gameObject.AddComponent<BDEnemyHitFlashReceiver>();
            if (flash != null)
                flash.FlashHeavy();

            BDHitStaggerReceiver stagger =
                health.GetComponent<BDHitStaggerReceiver>();
            if (stagger == null && health.GetComponent<CharacterController>() != null)
                stagger = health.gameObject.AddComponent<BDHitStaggerReceiver>();
            if (stagger != null)
                stagger.RequestStagger(enemyStaggerDuration);

            BDKnockbackReceiver knockback =
                health.GetComponent<BDKnockbackReceiver>();
            if (knockback == null && health.GetComponent<CharacterController>() != null)
                knockback = health.gameObject.AddComponent<BDKnockbackReceiver>();

            if (knockback != null)
            {
                Vector3 direction = health.transform.position - center;
                direction.y = 0f;
                if (direction.sqrMagnitude < 0.001f)
                    direction = health.transform.forward;
                knockback.AddKnockback(
                    direction.normalized,
                    enemyKnockbackStrength,
                    0.18f
                );
            }
        }

        private void ApplyColor(Color color, float emission)
        {
            if (cachedRenderer == null)
                return;

            cachedRenderer.GetPropertyBlock(colorBlock);
            colorBlock.SetColor(BaseColorId, color);
            colorBlock.SetColor(ColorId, color);
            colorBlock.SetColor(EmissionColorId, color * emission);
            cachedRenderer.SetPropertyBlock(colorBlock);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = new Color(1f, 0.30f, 0.05f, 0.65f);
            Gizmos.DrawWireSphere(transform.position, explosionRadius);
        }
    }
}
