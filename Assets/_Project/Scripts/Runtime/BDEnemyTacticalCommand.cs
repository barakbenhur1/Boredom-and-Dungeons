using UnityEngine;

namespace BoredomAndDungeons
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(CharacterController))]
    public sealed class BDEnemyTacticalCommand : MonoBehaviour
    {
        private enum CommandMode
        {
            None,
            FlankLeft,
            FlankRight,
            Pressure
        }

        [Header("Movement")]
        [SerializeField] private float flankMoveSpeed = 3.55f;
        [SerializeField] private float pressureMoveSpeed = 3.35f;
        [SerializeField] private float rotationSpeed = 16f;
        [SerializeField] private float flankDistance = 4.6f;
        [SerializeField] private float flankBackOffset = 2.35f;
        [SerializeField] private float pressureStopDistance = 1.65f;
        [SerializeField] private float arriveDistance = 0.55f;

        [Header("Attack")]
        [SerializeField] private float attackRange = 1.65f;
        [SerializeField] private float horseAttackRange = 2.35f;
        [SerializeField] private float attackDamage = 10f;
        [SerializeField] private float attackCooldown = 0.95f;

        [Header("Debug")]
        [SerializeField] private bool showDebugOverlay = false;

        private CharacterController controller;
        private BDHealth health;
        private BDKnockbackReceiver knockback;
        private Transform player;
        private CommandMode mode = CommandMode.None;
        private float commandUntil;
        private float attackTimer;
        private string state = "idle";

        public bool IsTakingControl => mode != CommandMode.None && Time.time < commandUntil;

        private void Awake()
        {
            controller = GetComponent<CharacterController>();
            health = GetComponent<BDHealth>();
            knockback = GetComponent<BDKnockbackReceiver>();
        }

        private void Start()
        {
            player = BDTargetFinder.FindPlayer();
        }

        private void Update()
        {
            if (attackTimer > 0f)
                attackTimer -= Time.deltaTime;

            if (health != null && health.IsDead)
            {
                mode = CommandMode.None;
                return;
            }

            if (!IsTakingControl)
            {
                mode = CommandMode.None;
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

            if (player == null)
                return;

            TickCommand();
        }

        public void CommandFlankLeft(float duration)
        {
            mode = CommandMode.FlankLeft;
            commandUntil = Mathf.Max(commandUntil, Time.time + duration);
        }

        public void CommandFlankRight(float duration)
        {
            mode = CommandMode.FlankRight;
            commandUntil = Mathf.Max(commandUntil, Time.time + duration);
        }

        public void CommandPressure(float duration)
        {
            mode = CommandMode.Pressure;
            commandUntil = Mathf.Max(commandUntil, Time.time + duration);
        }

        private void TickCommand()
        {
            Vector3 targetPoint = BuildTargetPoint();
            Vector3 toTarget = targetPoint - transform.position;
            toTarget.y = 0f;

            float speed = mode == CommandMode.Pressure ? pressureMoveSpeed : flankMoveSpeed;

            if (toTarget.magnitude > arriveDistance)
            {
                Vector3 direction = toTarget.normalized;
                controller.Move(direction * speed * Time.deltaTime);
                RotateToward(direction);
                state = mode.ToString();
            }
            else
            {
                Vector3 toPlayer = player.position - transform.position;
                toPlayer.y = 0f;
                RotateToward(toPlayer);
                state = mode + " hold";
            }

            TryAttackPlayerOrHorse();
        }

        private Vector3 BuildTargetPoint()
        {
            Vector3 playerForward = player.forward;
            playerForward.y = 0f;

            if (playerForward.sqrMagnitude < 0.001f)
                playerForward = Vector3.forward;

            playerForward.Normalize();

            Vector3 playerRight = Vector3.Cross(Vector3.up, playerForward).normalized;
            Vector3 targetPoint = player.position;

            switch (mode)
            {
                case CommandMode.FlankLeft:
                    targetPoint += -playerRight * flankDistance - playerForward * flankBackOffset;
                    break;

                case CommandMode.FlankRight:
                    targetPoint += playerRight * flankDistance - playerForward * flankBackOffset;
                    break;

                case CommandMode.Pressure:
                    Vector3 fromEnemyToPlayer = player.position - transform.position;
                    fromEnemyToPlayer.y = 0f;
                    Vector3 approach = fromEnemyToPlayer.sqrMagnitude > 0.001f
                        ? fromEnemyToPlayer.normalized
                        : transform.forward;
                    targetPoint = player.position - approach * pressureStopDistance;
                    break;
            }

            targetPoint.y = transform.position.y;
            return targetPoint;
        }

        private void TryAttackPlayerOrHorse()
        {
            if (attackTimer > 0f || player == null)
                return;

            bool attacked = false;

            if (Vector3.Distance(transform.position, player.position) <= attackRange)
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

        private void OnGUI()
        {
            if (!showDebugOverlay)
                return;

            GUI.Box(new Rect(Screen.width - 330, 570, 315, 95), "Tactical Command");
            GUI.Label(new Rect(Screen.width - 318, 600, 290, 22), $"Name: {name}");
            GUI.Label(new Rect(Screen.width - 318, 622, 290, 22), $"Mode: {mode}");
            GUI.Label(new Rect(Screen.width - 318, 644, 290, 22), $"State: {state}");
        }
    }
}
