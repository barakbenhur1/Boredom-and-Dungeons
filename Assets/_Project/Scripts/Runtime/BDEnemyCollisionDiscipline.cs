using UnityEngine;

namespace BoredomAndDungeons
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(CharacterController))]
    public sealed class BDEnemyCollisionDiscipline : MonoBehaviour
    {
        [Header("Personal Space")]
        [SerializeField] private float personalRadius = 1.35f;
        [SerializeField] private float separationStrength = 7.5f;
        [SerializeField] private float maxSeparationSpeed = 2.35f;
        [SerializeField] private float checkInterval = 0.035f;

        [Header("Role Multipliers")]
        [SerializeField] private float meleeRadiusMultiplier = 0.90f;
        [SerializeField] private float chargerRadiusMultiplier = 1.00f;
        [SerializeField] private float jumperRadiusMultiplier = 1.18f;
        [SerializeField] private float rangedRadiusMultiplier = 1.28f;
        [SerializeField] private float patrolRadiusMultiplier = 1.22f;
        [SerializeField] private float trapLayerRadiusMultiplier = 1.12f;

        private static readonly Collider[] NeighborBuffer = new Collider[32];

        private CharacterController controller;
        private BDHealth health;
        private float nextCheckAt;
        private float resolvedPersonalRadius;
        private Vector3 smoothedSeparation;

        private void Awake()
        {
            controller = GetComponent<CharacterController>();
            health = GetComponent<BDHealth>();
            resolvedPersonalRadius = ResolvePersonalRadius();
        }

        private void Update()
        {
            if (health != null && health.IsDead)
                return;

            if (Time.time < nextCheckAt)
            {
                ApplySmoothedSeparation();
                return;
            }

            nextCheckAt = Time.time + checkInterval;
            ComputeSeparation();
            ApplySmoothedSeparation();
        }

        private float ResolvePersonalRadius()
        {
            float multiplier = 1f;

            if (GetComponent<BDSwordEnemy>() != null)
                multiplier = meleeRadiusMultiplier;
            else if (GetComponent<BDChargerEnemy>() != null)
                multiplier = chargerRadiusMultiplier;
            else if (GetComponent<BDJumperEnemy>() != null)
                multiplier = jumperRadiusMultiplier;
            else if (GetComponent<BDRangedShooterEnemy>() != null)
                multiplier = rangedRadiusMultiplier;
            else if (GetComponent<BDPatrolGuardEnemy>() != null)
                multiplier = patrolRadiusMultiplier;
            else if (GetComponent<BDTrapLayerEnemy>() != null)
                multiplier = trapLayerRadiusMultiplier;

            return Mathf.Max(0.55f, personalRadius * multiplier);
        }

        private void ComputeSeparation()
        {
            Vector3 separation = Vector3.zero;
            int contributors = 0;

            int count = Physics.OverlapSphereNonAlloc(
                transform.position,
                resolvedPersonalRadius,
                NeighborBuffer,
                ~0,
                QueryTriggerInteraction.Ignore
            );

            for (int i = 0; i < count; i++)
            {
                Collider other = NeighborBuffer[i];

                if (other == null)
                    continue;

                if (other.transform == transform || other.transform.IsChildOf(transform))
                    continue;

                BDHealth otherHealth = other.GetComponentInParent<BDHealth>();
                if (otherHealth == null || otherHealth == health || otherHealth.IsDead)
                    continue;

                // Only separate from enemies. Do not push away from player/horse through this system.
                if (other.GetComponentInParent<BDPlayerMarker>() != null)
                    continue;

                if (other.GetComponentInParent<BDHorseHealth>() != null)
                    continue;

                Vector3 away = transform.position - otherHealth.transform.position;
                away.y = 0f;

                float distance = away.magnitude;
                if (distance <= 0.001f)
                {
                    float seed = Mathf.Abs(transform.GetInstanceID() * 0.173f);
                    away = new Vector3(Mathf.Sin(seed), 0f, Mathf.Cos(seed));
                    distance = 0.01f;
                }

                float penetration = Mathf.Clamp01((resolvedPersonalRadius - distance) / resolvedPersonalRadius);
                separation += away.normalized * penetration;
                contributors++;
            }

            if (contributors > 0)
            {
                separation /= contributors;
                separation.y = 0f;

                if (separation.sqrMagnitude > 1f)
                    separation.Normalize();

                separation *= separationStrength;
            }

            smoothedSeparation = Vector3.Lerp(smoothedSeparation, separation, 1f - Mathf.Exp(-18f * Time.deltaTime));
        }

        private void ApplySmoothedSeparation()
        {
            if (controller == null)
                return;

            Vector3 push = smoothedSeparation;
            push.y = 0f;

            float magnitude = push.magnitude;
            if (magnitude < 0.001f)
                return;

            if (magnitude > maxSeparationSpeed)
                push = push.normalized * maxSeparationSpeed;

            controller.Move(BDEnemyHazardNavigation.FilterBrainMotion(this, push * Time.deltaTime));
        }
    }
}
