using UnityEngine;

namespace BoredomAndDungeons
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(BDHealth))]
    public sealed class BDTrapLayerEnemy : MonoBehaviour
    {
        private enum State { Idle, ApproachToPlant, PlantBomb, RetreatAfterPlant, Reposition }

        [SerializeField] private Transform target;

        [Header("Role: approach, plant, retreat")]
        [SerializeField] private float aggroRange = 19f;
        [SerializeField] private float plantRange = 5.0f;
        [SerializeField] private float tooCloseRange = 2.75f;
        [SerializeField] private float retreatUntilRange = 8.6f;
        [SerializeField] private float approachSpeed = 2.85f;
        [SerializeField] private float retreatSpeed = 4.15f;
        [SerializeField] private float strafeSpeed = 2.0f;
        [SerializeField] private float rotationSpeed = 11f;

        [Header("Bomb")]
        [SerializeField] private float placeBombCooldown = 4.0f;
        [SerializeField] private float placeBombDuration = 0.42f;
        [SerializeField] private float recoverDuration = 0.35f;
        [SerializeField] private float bombSpawnForwardOffset = 0.9f;

        private CharacterController controller;
        private BDHitStaggerReceiver hitStaggerReceiver;
        private BDHealth health;
        private BDKnockbackReceiver knockback;

        private State state = State.Idle;
        private float stateTimer;
        private float bombCooldown;
        private float strafeSign = 1f;
        private float strafeSwitchTimer;

        private void Awake()
        {
            hitStaggerReceiver = GetComponent<BDHitStaggerReceiver>();
            controller = GetComponent<CharacterController>();
            health = GetComponent<BDHealth>();
            knockback = GetComponent<BDKnockbackReceiver>();
            health.Died += OnDied;
            strafeSign = Random.value < 0.5f ? -1f : 1f;
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

            if (bombCooldown > 0f)
                bombCooldown -= Time.deltaTime;

            if (strafeSwitchTimer > 0f)
                strafeSwitchTimer -= Time.deltaTime;
            else
            {
                strafeSwitchTimer = Random.Range(1.2f, 2.4f);
                if (Random.value < 0.45f)
                    strafeSign *= -1f;
            }

            if (knockback != null && knockback.IsBeingKnocked)
                return;



            if (hitStaggerReceiver != null && hitStaggerReceiver.ShouldPauseEnemyBrain())
                return;TickTrapLayer();
        }

        private void TickTrapLayer()
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

            switch (state)
            {
                case State.Idle:
                    state = State.ApproachToPlant;
                    break;

                case State.ApproachToPlant:
                    if (bombCooldown > 0f)
                    {
                        MaintainSafeReposition(direction, distance);
                        break;
                    }

                    if (distance <= plantRange && distance > tooCloseRange)
                    {
                        stateTimer = placeBombDuration;
                        state = State.PlantBomb;
                        break;
                    }

                    if (distance > plantRange)
                        controller.Move(FilterMoveByHitStagger(direction * approachSpeed * Time.deltaTime));
                    else
                        controller.Move(FilterMoveByHitStagger(-direction * retreatSpeed * Time.deltaTime));
                    break;

                case State.PlantBomb:
                    RotateToward(direction);
                    stateTimer -= Time.deltaTime;

                    if (stateTimer <= 0f)
                    {
                        SpawnBomb();
                        bombCooldown = placeBombCooldown;
                        stateTimer = recoverDuration;
                        state = State.RetreatAfterPlant;
                    }
                    break;

                case State.RetreatAfterPlant:
                    controller.Move(FilterMoveByHitStagger(-direction * retreatSpeed * Time.deltaTime));
                    stateTimer -= Time.deltaTime;

                    if (distance >= retreatUntilRange && stateTimer <= 0f)
                        state = State.Reposition;
                    break;

                case State.Reposition:
                    MaintainSafeReposition(direction, distance);

                    if (bombCooldown <= 0f)
                        state = State.ApproachToPlant;
                    break;
            }
        }

        private void MaintainSafeReposition(Vector3 direction, float distance)
        {
            if (distance < retreatUntilRange)
            {
                controller.Move(FilterMoveByHitStagger(-direction * retreatSpeed * 0.85f * Time.deltaTime));
                return;
            }

            Vector3 side = Vector3.Cross(Vector3.up, direction).normalized * strafeSign;
            controller.Move(FilterMoveByHitStagger(side * strafeSpeed * Time.deltaTime));
        }

        private void SpawnBomb()
        {
            Vector3 position = transform.position + transform.forward * bombSpawnForwardOffset;
            position.y = 0.35f;

            GameObject bomb = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            bomb.name = "BD_BombHazard";
            bomb.transform.position = position;
            bomb.transform.localScale = Vector3.one * 0.65f;

            SphereCollider col = bomb.GetComponent<SphereCollider>();
            col.isTrigger = true;

            BDBombHazard hazard = bomb.AddComponent<BDBombHazard>();
            hazard.ConfigureOwner(transform);
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
            float delay =
                BDCharacterDeathAnimation.PlayEnemyDeath(dead) + 0.10f;
            Destroy(gameObject, Mathf.Max(0.10f, delay));
        }

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
