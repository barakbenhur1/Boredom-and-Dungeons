using UnityEngine;

#pragma warning disable 0414

namespace BoredomAndDungeons
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(BDHealth))]
    public sealed class BDPatrolGuardEnemy : MonoBehaviour
    {
        private enum State { Patrol, DisruptPath, HoldSpace, Attack, ReturnHome, ExitBlocking }

        [SerializeField] private Transform target;

        [Header("Role: disrupt movement space")]
        [SerializeField] private float guardRadius = 17f;
        [SerializeField] private float chaseGiveUpRadius = 24f;
        [SerializeField] private float patrolSpeed = 2f;
        [SerializeField] private float disruptSpeed = 2.8f;
        [SerializeField] private float rotationSpeed = 12f;
        [SerializeField] private float waypointReachDistance = 0.35f;
        [SerializeField] private float waitAtWaypointDuration = 0.55f;

        [Header("Spatial control")]
        [SerializeField] private float desiredControlDistance = 5.2f;
        [SerializeField] private float tooCloseDistance = 2.75f;
        [SerializeField] private float pathBlockAheadDistance = 4.2f;
        [SerializeField] private float sideBlockOffset = 1.4f;
        [SerializeField] private float holdRadius = 0.65f;

        [Header("Attack")]
        [SerializeField] private float attackRange = 1.95f;
        [SerializeField] private float attackDamage = 10f;
        [SerializeField] private float attackCooldown = 1.35f;

        private CharacterController controller;
        private BDHitStaggerReceiver hitStaggerReceiver;
        private BDHealth health;
        private BDKnockbackReceiver knockback;
        private BDEnemyExitInterference exitInterference;
        private BDPatrolExitBlockerBrain patrolExitBlockerBrain;

        private State state = State.Patrol;
        private Vector3 home;
        private Vector3[] points;
        private int index;
        private float waitTimer;
        private float attackTimer;
        private float sideSign = 1f;
        private float sideSwitchTimer;

        private void Awake()
        {
            hitStaggerReceiver = GetComponent<BDHitStaggerReceiver>();
            controller = GetComponent<CharacterController>();
            health = GetComponent<BDHealth>();
            knockback = GetComponent<BDKnockbackReceiver>();
            exitInterference = GetComponent<BDEnemyExitInterference>();
            patrolExitBlockerBrain = GetComponent<BDPatrolExitBlockerBrain>();
            if (patrolExitBlockerBrain == null)
                patrolExitBlockerBrain = gameObject.AddComponent<BDPatrolExitBlockerBrain>();

            health.Died += OnDied;
            sideSign = Random.value < 0.5f ? -1f : 1f;
        }

        private void Start()
        {
            home = transform.position;
            points = new[]
            {
                home + new Vector3(-3.6f, 0f, -2.2f),
                home + new Vector3(3.6f, 0f, -2.2f),
                home + new Vector3(3.6f, 0f, 2.2f),
                home + new Vector3(-3.6f, 0f, 2.2f)
            };

            if (target == null)
                target = BDTargetFinder.FindPlayer();
        }

        private void Update()
        {
            if (health.IsDead)
                return;

            if (target == null)
                target = BDTargetFinder.FindPlayer();

            if (attackTimer > 0f)
                attackTimer -= Time.deltaTime;

            if (sideSwitchTimer > 0f)
                sideSwitchTimer -= Time.deltaTime;
            else
            {
                sideSwitchTimer = Random.Range(1.5f, 2.8f);
                if (Random.value < 0.4f)
                    sideSign *= -1f;
            }

            if (knockback != null && knockback.IsBeingKnocked)
                return;



            if (hitStaggerReceiver != null && hitStaggerReceiver.ShouldPauseEnemyBrain())
                return;if ((exitInterference != null && exitInterference.HasActiveCommand) ||
                (patrolExitBlockerBrain != null && patrolExitBlockerBrain.HasActiveBlockCommand))
            {
                state = State.ExitBlocking;
                return;
            }

            if (target == null)
            {
                Patrol();
                return;
            }

            float homeToPlayer = Vector3.Distance(home, target.position);

            if (homeToPlayer > chaseGiveUpRadius)
            {
                state = State.ReturnHome;
                MoveToward(home, patrolSpeed);
                return;
            }

            if (homeToPlayer <= guardRadius)
                TickSpatialDisruption();
            else
                Patrol();
        }

        private void TickSpatialDisruption()
        {
            Vector3 toPlayer = target.position - transform.position;
            toPlayer.y = 0f;
            float distance = toPlayer.magnitude;
            Vector3 directionToPlayer = distance > 0.001f ? toPlayer.normalized : transform.forward;

            RotateToward(directionToPlayer);

            if (distance <= attackRange)
            {
                state = State.Attack;
                TryAttack();
            }

            Vector3 targetPoint = BuildBlockPoint(directionToPlayer, distance);
            Vector3 toPoint = targetPoint - transform.position;
            toPoint.y = 0f;

            if (toPoint.magnitude > holdRadius)
            {
                state = State.DisruptPath;
                controller.Move(FilterMoveByHitStagger(toPoint.normalized * disruptSpeed * Time.deltaTime));
            }
            else
            {
                state = State.HoldSpace;
            }
        }

        private Vector3 BuildBlockPoint(Vector3 directionToPlayer, float distance)
        {
            Vector3 playerForward = target.forward;
            playerForward.y = 0f;

            if (playerForward.sqrMagnitude < 0.001f)
                playerForward = directionToPlayer;

            playerForward.Normalize();

            Vector3 playerRight = Vector3.Cross(Vector3.up, playerForward).normalized;

            // Stand ahead and slightly to the side, not directly on top of the player.
            Vector3 point = target.position + playerForward * pathBlockAheadDistance + playerRight * sideBlockOffset * sideSign;

            if (distance < tooCloseDistance)
                point = transform.position - directionToPlayer * 1.4f;
            else if (distance < desiredControlDistance)
                point += -playerForward * 1.15f;

            point.y = transform.position.y;
            return point;
        }

        private void Patrol()
        {
            state = State.Patrol;

            if (points == null || points.Length == 0)
                return;

            if (waitTimer > 0f)
            {
                waitTimer -= Time.deltaTime;
                return;
            }

            Vector3 p = points[index];

            if (Vector3.Distance(transform.position, p) <= waypointReachDistance)
            {
                index = (index + 1) % points.Length;
                waitTimer = waitAtWaypointDuration;
                return;
            }

            MoveToward(p, patrolSpeed);
        }

        private void MoveToward(Vector3 destination, float speed)
        {
            Vector3 direction = destination - transform.position;
            direction.y = 0f;

            if (direction.sqrMagnitude < 0.001f)
                return;

            Vector3 normalized = direction.normalized;
            controller.Move(FilterMoveByHitStagger(normalized * speed * Time.deltaTime));
            RotateToward(normalized);
        }

        private void TryAttack()
        {
            if (attackTimer > 0f || target == null)
                return;

            BDHealth targetHealth = target.GetComponent<BDHealth>();
            if (targetHealth != null)
                targetHealth.ApplyDamage(attackDamage);

            BDHorseDamageUtility.TryDamageHorseNear(
                transform.position + transform.forward * attackRange,
                attackRange,
                attackDamage,
                transform
            );

            attackTimer = attackCooldown;
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

        private void OnDied(BDHealth dead) => Destroy(gameObject, 0.1f);

        private void OnDestroy()
        {
            if (health != null)
                health.Died -= OnDied;
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
