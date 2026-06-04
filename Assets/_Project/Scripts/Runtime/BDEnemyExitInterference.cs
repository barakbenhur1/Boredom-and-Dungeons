using UnityEngine;

namespace BoredomAndDungeons
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(CharacterController))]
    public sealed class BDEnemyExitInterference : MonoBehaviour
    {
        [Header("Generic Interference")]
        [SerializeField] private float moveSpeed = 5.8f;
        [SerializeField] private float rotationSpeed = 16f;
        [SerializeField] private float holdRadius = 1.8f;
        [SerializeField] private float playerAttackRange = 2.15f;
        [SerializeField] private float horseAttackRange = 2.75f;
        [SerializeField] private float attackDamage = 10f;
        [SerializeField] private float attackCooldown = 0.62f;
        [SerializeField] private float interceptOffsetInsideRoom = 3.8f;

        [Header("Patrol Guard Exit Blocker")]
        [SerializeField] private float patrolBlockMoveSpeed = 11.5f;
        [SerializeField] private float patrolBlockHoldRadius = 0.22f;
        [SerializeField] private float patrolBlockInsideOffset = 1.25f;
        [SerializeField] private float patrolBlockExtraDuration = 3.25f;
        [SerializeField] private float patrolBlockControllerRadius = 1.05f;
        [SerializeField] private float patrolBlockAttackRange = 2.55f;
        [SerializeField] private float patrolBlockHorseAttackRange = 3.05f;
        [SerializeField] private float patrolBlockAttackDamage = 14f;
        [SerializeField] private float patrolBlockAttackCooldown = 0.48f;

        [Header("Debug")]
        [SerializeField] private bool showDebugOverlay = false;

        private CharacterController controller;
        private BDHitStaggerReceiver hitStaggerReceiver;
        private BDHealth health;
        private BDKnockbackReceiver knockback;
        private BDPatrolGuardEnemy patrolGuard;
        private Transform player;
        private Transform currentExitAnchor;

        private float commandTimer;
        private float attackTimer;
        private float originalControllerRadius;
        private bool patrolRadiusApplied;
        private string state = "idle";

        public bool HasActiveCommand => commandTimer > 0f && currentExitAnchor != null;
        public bool IsPatrolExitBlocker => false;

        private void Awake()
        {
            hitStaggerReceiver = GetComponent<BDHitStaggerReceiver>();
            controller = GetComponent<CharacterController>();
            health = GetComponent<BDHealth>();
            knockback = GetComponent<BDKnockbackReceiver>();
            patrolGuard = GetComponent<BDPatrolGuardEnemy>();

            if (controller != null)
                originalControllerRadius = controller.radius;
        }

        private void Start()
        {
            player = BDTargetFinder.FindPlayer();
        }

        private void Update()
        {

            if (hitStaggerReceiver != null && hitStaggerReceiver.ShouldPauseEnemyBrain())
                return;
            if (health != null && health.IsDead)
            {
                ClearPatrolRadius();
                return;
            }

            if (attackTimer > 0f)
                attackTimer -= Time.deltaTime;

            if (commandTimer > 0f)
            {
                commandTimer -= Time.deltaTime;
            }
            else
            {
                currentExitAnchor = null;
                ClearPatrolRadius();
            }

            if (currentExitAnchor == null)
            {
                state = "idle";
                return;
            }

            if (knockback != null && knockback.IsBeingKnocked)
            {
                state = "knockback";
                return;
            }

            if (player == null)
                player = BDTargetFinder.FindPlayer();

            if (IsPatrolExitBlocker)
                TickPatrolExitBlocker();
            else
                TickGenericInterference();
        }

        public void CommandInterfereWithExit(Transform exitAnchor, float duration)
        {
            if (exitAnchor == null)
                return;

            if (health != null && health.IsDead)
                return;

            currentExitAnchor = exitAnchor;

            float actualDuration = IsPatrolExitBlocker
                ? duration + patrolBlockExtraDuration
                : duration;

            commandTimer = Mathf.Max(commandTimer, actualDuration);

            if (IsPatrolExitBlocker)
                ApplyPatrolRadius();

            state = IsPatrolExitBlocker ? "patrol ordered to block exit" : "commanded";
        }

        private void TickPatrolExitBlocker()
        {
            if (currentExitAnchor == null)
                return;

            Vector3 targetPoint = currentExitAnchor.position;

            if (player != null)
            {
                Vector3 insideDirection = player.position - currentExitAnchor.position;
                insideDirection.y = 0f;

                if (insideDirection.sqrMagnitude > 0.001f)
                    targetPoint += insideDirection.normalized * patrolBlockInsideOffset;
            }

            Vector3 toPoint = targetPoint - transform.position;
            toPoint.y = 0f;

            if (toPoint.magnitude > patrolBlockHoldRadius)
            {
                state = "patrol moving to block exit";
                Vector3 direction = toPoint.normalized;
                controller.Move(FilterMoveByHitStagger(direction * patrolBlockMoveSpeed * Time.deltaTime));
                RotateToward(direction);
            }
            else
            {
                state = "patrol holding exit";
                if (player != null)
                    RotateToward(player.position - transform.position);
            }

            TryAttackPlayerOrHorse(
                patrolBlockAttackRange,
                patrolBlockHorseAttackRange,
                patrolBlockAttackDamage,
                patrolBlockAttackCooldown
            );
        }

        private void TickGenericInterference()
        {
            if (currentExitAnchor == null)
                return;

            Vector3 anchor = currentExitAnchor.position;
            Vector3 targetPoint = anchor;

            if (player != null)
            {
                Vector3 fromExitToPlayer = player.position - anchor;
                fromExitToPlayer.y = 0f;

                if (fromExitToPlayer.sqrMagnitude > 0.001f)
                    targetPoint = anchor + fromExitToPlayer.normalized * interceptOffsetInsideRoom;
            }

            Vector3 toPoint = targetPoint - transform.position;
            toPoint.y = 0f;

            if (toPoint.magnitude > holdRadius)
            {
                state = "generic moving to interfere";
                Vector3 direction = toPoint.normalized;
                controller.Move(FilterMoveByHitStagger(direction * moveSpeed * Time.deltaTime));
                RotateToward(direction);
            }
            else
            {
                state = "generic interfering near exit";
                if (player != null)
                    RotateToward(player.position - transform.position);
            }

            TryAttackPlayerOrHorse(
                playerAttackRange,
                horseAttackRange,
                attackDamage,
                attackCooldown
            );
        }

        private void TryAttackPlayerOrHorse(float playerRange, float horseRange, float damage, float cooldown)
        {
            if (attackTimer > 0f)
                return;

            bool attacked = false;

            if (player != null)
            {
                float playerDistance = Vector3.Distance(transform.position, player.position);

                if (playerDistance <= playerRange)
                {
                    BDHealth playerHealth = player.GetComponent<BDHealth>();
                    if (playerHealth != null)
                    {
                        playerHealth.ApplyDamage(damage);
                        attacked = true;
                    }
                }
            }

            if (!attacked)
                attacked = BDHorseDamageUtility.TryDamageHorseNear(transform.position, horseRange, damage, transform);

            if (attacked)
                attackTimer = cooldown;
        }

        private void ApplyPatrolRadius()
        {
            if (controller == null || patrolRadiusApplied)
                return;

            originalControllerRadius = controller.radius;
            controller.radius = Mathf.Max(controller.radius, patrolBlockControllerRadius);
            patrolRadiusApplied = true;
        }

        private void ClearPatrolRadius()
        {
            if (controller == null || !patrolRadiusApplied)
                return;

            controller.radius = originalControllerRadius;
            patrolRadiusApplied = false;
        }

        private void RotateToward(Vector3 direction)
        {
            direction.y = 0f;

            if (direction.sqrMagnitude < 0.001f)
                return;

            Quaternion target = Quaternion.LookRotation(direction.normalized, Vector3.up);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                target,
                1f - Mathf.Exp(-rotationSpeed * Time.deltaTime)
            );
        }

        private void OnDisable()
        {
            ClearPatrolRadius();
        }

        private void OnDestroy()
        {
            ClearPatrolRadius();
        }

        private void OnGUI()
        {
            if (!showDebugOverlay)
                return;

            GUI.Box(new Rect(Screen.width - 330, 350, 315, 118), "Exit Interference Enemy");
            GUI.Label(new Rect(Screen.width - 318, 380, 290, 22), $"Name: {name}");
            GUI.Label(new Rect(Screen.width - 318, 402, 290, 22), $"Role: {(IsPatrolExitBlocker ? "PATROL EXIT BLOCKER" : "generic")}");
            GUI.Label(new Rect(Screen.width - 318, 424, 290, 22), $"State: {state}");
            GUI.Label(new Rect(Screen.width - 318, 446, 290, 22), $"Timer: {commandTimer:0.0}");
        }
        private Vector3 FilterMoveByHitStagger(Vector3 move)
        {
            if (hitStaggerReceiver == null)
                hitStaggerReceiver = GetComponent<BDHitStaggerReceiver>();

            return hitStaggerReceiver != null ? hitStaggerReceiver.FilterMove(move) : move;
        }


    }
}
