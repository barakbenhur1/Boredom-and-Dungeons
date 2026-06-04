using UnityEngine;

#pragma warning disable 0414

namespace BoredomAndDungeons
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(BDHealth))]
    public sealed class BDSwordEnemy : MonoBehaviour
    {
        private enum State { Idle, ClosePressure, Circle, Attack }

        [SerializeField] private Transform target;

        [Header("Role: close attacker")]
        [SerializeField] private float aggroRange = 15f;
        [SerializeField] private float moveSpeed = 3.35f;
        [SerializeField] private float circleSpeed = 1.75f;
        [SerializeField] private float rotationSpeed = 14f;
        [SerializeField] private float desiredDistance = 1.65f;
        [SerializeField] private float tooCloseDistance = 1.15f;
        [SerializeField] private float attackRange = 1.72f;
        [SerializeField] private float attackDamage = 12f;
        [SerializeField] private float attackCooldown = 1.05f;

        private CharacterController controller;
        private BDHitStaggerReceiver hitStaggerReceiver;
        private BDHealth health;
        private BDKnockbackReceiver knockback;
        private BDEnemyTacticalCommand tacticalCommand;
        private BDEnemySwordWeaponVisual weaponVisual;

        private State state = State.Idle;
        private float cooldown;
        private float circleSign = 1f;
        private float circleSwitchTimer;

        private void Awake()
        {
            hitStaggerReceiver = GetComponent<BDHitStaggerReceiver>();
            controller = GetComponent<CharacterController>();
            health = GetComponent<BDHealth>();
            knockback = GetComponent<BDKnockbackReceiver>();
            tacticalCommand = GetComponent<BDEnemyTacticalCommand>();
            weaponVisual = GetComponent<BDEnemySwordWeaponVisual>();

            if (weaponVisual == null)
                weaponVisual = gameObject.AddComponent<BDEnemySwordWeaponVisual>();

            health.Died += OnDied;
            circleSign = Random.value < 0.5f ? -1f : 1f;
        }

        private void Start()
        {
            if (target == null)
                target = BDTargetFinder.FindPlayer();
        }

        private void Update()
        {
            if (health.IsDead)
                return;

            if (target == null)
                target = BDTargetFinder.FindPlayer();

            if (target == null)
                return;

            if (tacticalCommand != null && tacticalCommand.IsTakingControl)
                return;

            if (cooldown > 0f)
                cooldown -= Time.deltaTime;

            if (circleSwitchTimer > 0f)
                circleSwitchTimer -= Time.deltaTime;
            else
            {
                circleSwitchTimer = Random.Range(1.0f, 2.2f);
                if (Random.value < 0.35f)
                    circleSign *= -1f;
            }

            if (knockback != null && knockback.IsBeingKnocked)
                return;

            if (hitStaggerReceiver != null && hitStaggerReceiver.ShouldPauseEnemyBrain())
                return;

            TickCloseAttacker();
        }

        private void TickCloseAttacker()
        {
            Vector3 toTarget = target.position - transform.position;
            toTarget.y = 0f;
            float distance = toTarget.magnitude;

            if (distance > aggroRange)
            {
                state = State.Idle;
                return;
            }

            Vector3 direction = distance > 0.001f ? toTarget.normalized : transform.forward;
            RotateToward(direction);

            if (distance <= attackRange)
            {
                state = State.Attack;
                TryAttack(distance);

                // Do not stand exactly inside the player. Slide around them.
                if (distance < tooCloseDistance)
                    Circle(direction, pushOut: true);
                return;
            }

            if (distance > desiredDistance)
            {
                state = State.ClosePressure;
                controller.Move(FilterMoveByHitStagger(direction * moveSpeed * Time.deltaTime));
            }
            else
            {
                state = State.Circle;
                Circle(direction, pushOut: false);
            }
        }

        private void Circle(Vector3 directionToTarget, bool pushOut)
        {
            Vector3 side = Vector3.Cross(Vector3.up, directionToTarget).normalized * circleSign;
            Vector3 move = side;

            if (pushOut)
                move -= directionToTarget * 0.75f;

            if (move.sqrMagnitude > 0.001f)
                controller.Move(FilterMoveByHitStagger(move.normalized * circleSpeed * Time.deltaTime));
        }

        private void TryAttack(float distance)
        {
            if (cooldown > 0f || distance > attackRange)
                return;

            ShowAttackTelegraphBeforeDamage(false);

            if (weaponVisual != null)
                weaponVisual.PlayDoubleSlash();

            BDHealth targetHealth = target.GetComponent<BDHealth>();
            if (targetHealth != null)
                targetHealth.ApplyDamage(attackDamage);

            BDHorseDamageUtility.TryDamageHorseNear(
                transform.position + transform.forward * attackRange,
                attackRange,
                attackDamage,
                transform
            );

            cooldown = attackCooldown;
        }

        private void RotateToward(Vector3 direction)
        {
            direction.y = 0f;

            if (direction.sqrMagnitude < 0.001f)
                return;

            Quaternion rot = Quaternion.LookRotation(direction.normalized, Vector3.up);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                rot,
                1f - Mathf.Exp(-rotationSpeed * Time.deltaTime)
            );
        }

        private void OnDied(BDHealth dead)
        {
            Destroy(gameObject, 0.1f);
        }

        private void OnDestroy()
        {
            if (health != null)
                health.Died -= OnDied;
        }

        private void ShowAttackTelegraphBeforeDamage(bool ranged)
        {
            BDEnemyAttackTelegraph telegraph = GetComponent<BDEnemyAttackTelegraph>();
            if (telegraph == null)
                telegraph = gameObject.AddComponent<BDEnemyAttackTelegraph>();

            Vector3 direction = transform.forward;
            Transform player = BDTargetFinder.FindPlayer();

            if (player != null)
                direction = player.position - transform.position;

            direction.y = 0f;

            if (direction.sqrMagnitude < 0.001f)
                direction = transform.forward;

            if (ranged)
                telegraph.ShowRanged(direction);
            else
                telegraph.ShowMelee(direction);
        }

        private Vector3 FilterMoveByHitStagger(Vector3 move)
        {
            if (hitStaggerReceiver == null)
                hitStaggerReceiver = GetComponent<BDHitStaggerReceiver>();

            return hitStaggerReceiver != null ? hitStaggerReceiver.FilterMove(move) : move;
        }
    }
}
