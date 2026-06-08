using UnityEngine;

namespace BoredomAndDungeons
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(CharacterController))]
    public sealed class BDPatrolExitBlockerBrain : MonoBehaviour
    {
        [Header("Exit Blocking")]
        [SerializeField] private float moveSpeed = 18.0f;
        [SerializeField] private float holdRadius = 0.18f;
        [SerializeField] private float insideOffset = 2.85f;
        [SerializeField] private float commandDuration = 11.5f;
        [SerializeField] private float rotationSpeed = 20f;

        [Header("Body Block")]
        [SerializeField] private float blockRadius = 1.75f;
        [SerializeField] private float playerAttackRange = 2.55f;
        [SerializeField] private float horseAttackRange = 3.1f;
        [SerializeField] private float attackDamage = 14f;
        [SerializeField] private float attackCooldown = 0.45f;

        [Header("Debug")]
        [SerializeField] private bool showDebugOverlay = false;

        private CharacterController controller;
        private BDHealth health;
        private BDKnockbackReceiver knockback;
        private Transform player;
        private Transform exitAnchor;
        private float timer;
        private float attackTimer;
        private float originalRadius;
        private bool radiusApplied;
        private string state = "idle";

        public bool HasActiveBlockCommand => timer > 0f && exitAnchor != null;

        private void Awake()
        {
            controller = GetComponent<CharacterController>();
            health = GetComponent<BDHealth>();
            knockback = GetComponent<BDKnockbackReceiver>();
            originalRadius = controller != null ? controller.radius : 0.35f;
        }

        private void Start()
        {
            player = BDTargetFinder.FindPlayer();
        }

        private void Update()
        {
            if (health != null && health.IsDead)
            {
                ClearRadius();
                return;
            }

            if (attackTimer > 0f)
                attackTimer -= Time.deltaTime;

            if (timer > 0f)
                timer -= Time.deltaTime;
            else
                exitAnchor = null;

            if (!HasActiveBlockCommand)
            {
                state = "idle";
                ClearRadius();
                return;
            }

            if (knockback != null && knockback.IsBeingKnocked)
            {
                state = "knockback";
                return;
            }

            if (player == null)
                player = BDTargetFinder.FindPlayer();

            ApplyRadius();
            TickBlock();
        }

        public void CommandBlockExit(Transform anchor)
        {
            if (anchor == null)
                return;

            if (health != null && health.IsDead)
                return;

            exitAnchor = anchor;
            timer = commandDuration;
            ApplyRadius();
            state = "ordered";
        }

        private void TickBlock()
        {
            Vector3 target = exitAnchor.position;

            if (player != null)
            {
                Vector3 insideDirection = player.position - exitAnchor.position;
                insideDirection.y = 0f;

                if (insideDirection.sqrMagnitude > 0.001f)
                    target += insideDirection.normalized * insideOffset;
            }

            Vector3 toTarget = target - transform.position;
            toTarget.y = 0f;

            if (toTarget.magnitude > holdRadius)
            {
                Vector3 direction = toTarget.normalized;
                controller.Move(BDEnemyHazardNavigation.FilterBrainMotion(this, direction * moveSpeed * Time.deltaTime));
                RotateToward(direction);
                state = "sprinting to exit";
            }
            else
            {
                state = "body-blocking exit";

                if (player != null)
                    RotateToward(player.position - transform.position);
            }

            TryAttackPlayerOrHorse();
        }

        private void TryAttackPlayerOrHorse()
        {
            if (attackTimer > 0f)
                return;

            bool attacked = false;

            if (player != null && Vector3.Distance(transform.position, player.position) <= playerAttackRange)
            {
                BDHealth playerHealth = player.GetComponent<BDHealth>();
                if (playerHealth != null)
                {
                    playerHealth.ApplyDamage(attackDamage);
                    attacked = true;
                }
            }

            if (!attacked)
                attacked = BDHorseDamageUtility.TryDamageHorseNear(transform.position, horseAttackRange, attackDamage, transform);

            if (attacked)
                attackTimer = attackCooldown;
        }

        private void ApplyRadius()
        {
            if (controller == null || radiusApplied)
                return;

            originalRadius = controller.radius;
            controller.radius = Mathf.Max(controller.radius, blockRadius);
            radiusApplied = true;
        }

        private void ClearRadius()
        {
            if (controller == null || !radiusApplied)
                return;

            controller.radius = originalRadius;
            radiusApplied = false;
        }

        private void RotateToward(Vector3 direction)
        {
            direction.y = 0f;

            if (direction.sqrMagnitude < 0.001f)
                return;

            Quaternion targetRotation = Quaternion.LookRotation(direction.normalized, Vector3.up);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                1f - Mathf.Exp(-rotationSpeed * Time.deltaTime)
            );
        }

        private void OnDisable()
        {
            ClearRadius();
        }

        private void OnDestroy()
        {
            ClearRadius();
        }

        private void OnGUI()
        {
            if (!showDebugOverlay)
                return;

            GUI.Box(new Rect(Screen.width - 330, 470, 315, 95), "Patrol Exit Blocker");
            GUI.Label(new Rect(Screen.width - 318, 500, 290, 22), $"Name: {name}");
            GUI.Label(new Rect(Screen.width - 318, 522, 290, 22), $"State: {state}");
            GUI.Label(new Rect(Screen.width - 318, 544, 290, 22), $"Timer: {timer:0.0}");
        }
    }
}
