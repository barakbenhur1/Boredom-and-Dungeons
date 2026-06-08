using UnityEngine;

namespace BoredomAndDungeons
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(BDHealth))]
    public sealed class BDChargerEnemy : MonoBehaviour
    {
        private enum State { Idle, Approach, Windup, Charging, Recover }

        [SerializeField] private Transform target;
        [SerializeField] private float aggroRange = 18.5f;
        [SerializeField] private float windupRange = 7.2f;
        [SerializeField] private float approachSpeed = 2.55f;
        [SerializeField] private float chargeSpeed = 9.4f;
        [SerializeField] private float rotationSpeed = 10f;
        [SerializeField] private float windupDuration = 0.85f;
        [SerializeField] private float chargeDuration = 0.48f;
        [SerializeField] private float recoverDuration = 1.05f;
        [SerializeField] private float hitRadius = 0.95f;
        [SerializeField] private float chargeDamage = 22f;

        private CharacterController controller;
        private BDHitStaggerReceiver hitStaggerReceiver;
        private BDHealth health;
        private BDKnockbackReceiver knockback;
        private BDEnemyTacticalCommand tacticalCommand;
        private State state = State.Idle;
        private float timer;
        private Vector3 chargeDirection;
        private bool didHit;

        private void Awake()
        {
            hitStaggerReceiver = GetComponent<BDHitStaggerReceiver>();
            controller = GetComponent<CharacterController>();
            health = GetComponent<BDHealth>();
            knockback = GetComponent<BDKnockbackReceiver>();
            tacticalCommand = GetComponent<BDEnemyTacticalCommand>();
            health.Died += OnDied;
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

            if (knockback != null && knockback.IsBeingKnocked)
                return;



            if (hitStaggerReceiver != null && hitStaggerReceiver.ShouldPauseEnemyBrain())
                return;Vector3 toTarget = target.position - transform.position;
            toTarget.y = 0f;
            float distance = toTarget.magnitude;
            Vector3 direction = distance > 0.001f ? toTarget.normalized : transform.forward;

            switch (state)
            {
                case State.Idle:
                    if (distance <= aggroRange) state = State.Approach;
                    break;

                case State.Approach:
                    RotateToward(direction);
                    if (distance <= windupRange)
                    {
                        chargeDirection = direction;
                        timer = windupDuration;
                        state = State.Windup;
                    }
                    else
                    {
                        controller.Move(FilterMoveByHitStagger(direction * approachSpeed * Time.deltaTime));
                    }
                    break;

                case State.Windup:
                    RotateToward(chargeDirection);
                    timer -= Time.deltaTime;
                    if (timer <= 0f)
                    {
                        didHit = false;
                        timer = chargeDuration;
                        state = State.Charging;
                    }
                    break;

                case State.Charging:
                    controller.Move(FilterMoveByHitStagger(chargeDirection * chargeSpeed * Time.deltaTime));
                    RotateToward(chargeDirection);
                    TryHit();
                    timer -= Time.deltaTime;
                    if (timer <= 0f)
                    {
                        timer = recoverDuration;
                        state = State.Recover;
                    }
                    break;

                case State.Recover:
                    timer -= Time.deltaTime;
                    if (timer <= 0f)
                        state = State.Approach;
                    break;
            }
        }

        private void TryHit()
        {
            if (didHit ||
                target == null ||
                BDGrapplingHookPullState.IsContactAttackSuppressed(transform))
            {
                return;
            }

            bool hitAnything = false;

            if (Vector3.Distance(transform.position, target.position) <= hitRadius)
            {
                BDHealth targetHealth = target.GetComponent<BDHealth>();
                if (targetHealth != null)
                {
                    ShowAttackTelegraphBeforeDamage(false);
                    targetHealth.ApplyDamage(chargeDamage);
                    hitAnything = true;
                }
            }

            if (BDHorseDamageUtility.TryDamageHorseNear(transform.position, hitRadius, chargeDamage, transform))
                hitAnything = true;

            if (hitAnything)
                didHit = true;
        }

        private void RotateToward(Vector3 direction)
        {
            if (direction.sqrMagnitude < 0.001f) return;
            Quaternion rot = Quaternion.LookRotation(direction.normalized, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, rot, 1f - Mathf.Exp(-rotationSpeed * Time.deltaTime));
        }

        private void OnDied(BDHealth dead)
        {
            float delay =
                BDCharacterDeathAnimation.PlayEnemyDeath(dead) + 0.10f;
            Destroy(gameObject, Mathf.Max(0.10f, delay));
        }

        private void OnDestroy()
        {
            if (health != null) health.Died -= OnDied;
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

            Vector3 staggerFiltered =
                hitStaggerReceiver != null
                    ? hitStaggerReceiver.FilterMove(move)
                    : move;

            return BDEnemyMovementPolish.Filter(
                this,
                staggerFiltered
            );
        }


    }
}
