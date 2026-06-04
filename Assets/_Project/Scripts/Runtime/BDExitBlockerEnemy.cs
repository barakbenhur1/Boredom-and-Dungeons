using UnityEngine;

namespace BoredomAndDungeons
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(BDHealth))]
    public sealed class BDExitBlockerEnemy : MonoBehaviour
    {
        private enum State { Chase, MoveToExit, HoldExit, Attack }

        [SerializeField] private Transform player;
        [SerializeField] private float chaseSpeed = 3.2f;
        [SerializeField] private float blockExitSpeed = 5f;
        [SerializeField] private float rotationSpeed = 14f;
        [SerializeField] private float stopDistance = 1.4f;
        [SerializeField] private float attackRange = 1.65f;
        [SerializeField] private float attackDamage = 11f;
        [SerializeField] private float attackCooldown = 0.95f;
        [SerializeField] private float holdExitRadius = 0.75f;

        private CharacterController controller;
        private BDHitStaggerReceiver hitStaggerReceiver;
        private BDHealth health;
        private BDKnockbackReceiver knockback;
        private Transform currentExit;
        private float blockTimer;
        private float attackTimer;
        private State state = State.Chase;

        private void Awake()
        {
            hitStaggerReceiver = GetComponent<BDHitStaggerReceiver>();
            controller = GetComponent<CharacterController>();
            health = GetComponent<BDHealth>();
            knockback = GetComponent<BDKnockbackReceiver>();
            health.Died += OnDied;
        }

        private void Start()
        {
            if (player == null)
                player = BDTargetFinder.FindPlayer();
        }

        private void Update()
        {
            if (health.IsDead)
                return;

            if (player == null)
                player = BDTargetFinder.FindPlayer();

            if (attackTimer > 0f)
                attackTimer -= Time.deltaTime;

            if (blockTimer > 0f)
                blockTimer -= Time.deltaTime;
            else
                currentExit = null;

            if (knockback != null && knockback.IsBeingKnocked)
                return;

            

            if (hitStaggerReceiver != null && hitStaggerReceiver.ShouldPauseEnemyBrain())
                return;if (currentExit != null)
                TickBlockExit();
            else
                TickChase();
        }

        public void CommandBlockExit(Transform exitAnchor, float duration)
        {
            if (exitAnchor == null)
                return;

            currentExit = exitAnchor;
            blockTimer = duration;
            state = State.MoveToExit;
        }

        private void TickBlockExit()
        {
            Vector3 toExit = currentExit.position - transform.position;
            toExit.y = 0f;

            if (toExit.magnitude > holdExitRadius)
            {
                state = State.MoveToExit;
                Vector3 direction = toExit.normalized;
                controller.Move(FilterMoveByHitStagger(direction * blockExitSpeed * Time.deltaTime));
                RotateToward(direction);
                return;
            }

            state = State.HoldExit;

            if (player == null)
                return;

            Vector3 toPlayer = player.position - transform.position;
            toPlayer.y = 0f;

            if (toPlayer.sqrMagnitude > 0.001f)
                RotateToward(toPlayer.normalized);

            if (toPlayer.magnitude <= attackRange)
                TryAttack();
        }

        private void TickChase()
        {
            if (player == null)
                return;

            Vector3 toPlayer = player.position - transform.position;
            toPlayer.y = 0f;
            float distance = toPlayer.magnitude;

            if (distance > stopDistance)
            {
                state = State.Chase;
                Vector3 direction = toPlayer.normalized;
                controller.Move(FilterMoveByHitStagger(direction * chaseSpeed * Time.deltaTime));
                RotateToward(direction);
            }
            else
            {
                state = State.Attack;
                RotateToward(toPlayer.normalized);
                TryAttack();
            }
        }

        private void TryAttack()
        {
            if (attackTimer > 0f || player == null)
                return;

            BDHealth playerHealth = player.GetComponent<BDHealth>();
            if (playerHealth != null)
                playerHealth.ApplyDamage(attackDamage);

            BDHorseDamageUtility.TryDamageHorseNear(transform.position + transform.forward * attackRange, attackRange, attackDamage, transform);

            attackTimer = attackCooldown;
        }

        private void RotateToward(Vector3 direction)
        {
            direction.y = 0f;
            if (direction.sqrMagnitude < 0.001f) return;

            Quaternion rot = Quaternion.LookRotation(direction.normalized, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, rot, 1f - Mathf.Exp(-rotationSpeed * Time.deltaTime));
        }

        private void OnDied(BDHealth dead) => Destroy(gameObject, 0.1f);

        private void OnDestroy()
        {
            if (health != null) health.Died -= OnDied;
        }

        private void OnGUI()
        {
            GUI.Box(new Rect(Screen.width - 285, 12, 270, 95), "Exit Blocker");
            GUI.Label(new Rect(Screen.width - 273, 42, 245, 22), $"State: {state}");
            GUI.Label(new Rect(Screen.width - 273, 64, 245, 22), $"Block Timer: {blockTimer:0.0}");
            GUI.Label(new Rect(Screen.width - 273, 86, 245, 22), "Try exiting during combat.");
        }
        private Vector3 FilterMoveByHitStagger(Vector3 move)
        {
            if (hitStaggerReceiver == null)
                hitStaggerReceiver = GetComponent<BDHitStaggerReceiver>();

            return hitStaggerReceiver != null ? hitStaggerReceiver.FilterMove(move) : move;
        }


    }
}
