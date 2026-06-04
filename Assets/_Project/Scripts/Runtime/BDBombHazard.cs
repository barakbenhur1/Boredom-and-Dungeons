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

        private float timer;
        private bool armed;
        private Renderer cachedRenderer;

        private void Awake()
        {
            cachedRenderer = GetComponent<Renderer>();
            ApplyColor(new Color(0.15f, 0.02f, 0.02f, 1f));
        }

        private void Update()
        {
            timer += Time.deltaTime;

            if (!armed && timer >= armTime)
            {
                armed = true;
                ApplyColor(new Color(1f, 0.35f, 0.05f, 1f));
            }

            float pulse = 1f + Mathf.Sin(Time.time * 12f) * 0.08f;
            transform.localScale = Vector3.one * 0.65f * pulse;

            if (timer >= explodeAfter)
                Explode();
        }

        private void Explode()
        {
            Collider[] hits = Physics.OverlapSphere(transform.position, explosionRadius, ~0, QueryTriggerInteraction.Ignore);

            foreach (Collider hit in hits)
            {
                BDPlayerMarker player = hit.GetComponentInParent<BDPlayerMarker>();
                if (player == null)
                    continue;

                BDHealth health = player.GetComponent<BDHealth>();
                if (health != null)
                    health.ApplyDamage(damage);
            }

            BDHorseDamageUtility.TryDamageHorseNear(transform.position, explosionRadius, damage, transform);

            Destroy(gameObject);
        }

        private void ApplyColor(Color color)
        {
            if (cachedRenderer == null)
                return;

            Material mat = new Material(Shader.Find("Standard"));
            mat.color = color;
            cachedRenderer.material = mat;
        }
    }
}
