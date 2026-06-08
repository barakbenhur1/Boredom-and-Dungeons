using UnityEngine;

namespace BoredomAndDungeons
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(BDHealth))]
    public sealed class BDRangedShooterEnemy : MonoBehaviour
    {
        private enum State { Idle, Reposition, Aim, Shoot, Recover }

        [SerializeField] private Transform target;

        [Header("Role: keep distance and shoot")]
        [SerializeField] private float aggroRange = 21f;
        [SerializeField] private float idealRange = 11.0f;
        [SerializeField] private float minSafeRange = 7.4f;
        [SerializeField] private float maxShootRange = 16.5f;
        [SerializeField] private float approachSpeed = 1.85f;
        [SerializeField] private float retreatSpeed = 4.25f;
        [SerializeField] private float strafeSpeed = 1.85f;
        [SerializeField] private float rotationSpeed = 13f;

        [Header("Shooting")]
        [SerializeField] private float aimDuration = 0.62f;
        [SerializeField] private float recoverDuration = 0.7f;
        [SerializeField] private float shootCooldown = 1.85f;
        [SerializeField] private float projectileSpeed = 9.5f;
        [SerializeField] private float projectileDamage = 14f;
        [SerializeField] private float projectileSpawnForwardOffset = 0.9f;

        private CharacterController controller;
        private BDHitStaggerReceiver hitStaggerReceiver;
        private BDHealth health;
        private BDKnockbackReceiver knockback;
        private State state = State.Idle;
        private float stateTimer;
        private float cooldown;
        private float strafeSign = 1f;
        private float strafeSwitchTimer;
        private Vector3 aimDirection;

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

            if (cooldown > 0f)
                cooldown -= Time.deltaTime;

            if (strafeSwitchTimer > 0f)
                strafeSwitchTimer -= Time.deltaTime;
            else
            {
                strafeSwitchTimer = Random.Range(1.3f, 2.4f);
                if (Random.value < 0.45f)
                    strafeSign *= -1f;
            }

            if (knockback != null && knockback.IsBeingKnocked)
                return;



            if (hitStaggerReceiver != null && hitStaggerReceiver.ShouldPauseEnemyBrain())
                return;TickRanged();
        }

        private void TickRanged()
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
                    state = State.Reposition;
                    break;

                case State.Reposition:
                    MaintainRange(direction, distance);

                    if (distance >= minSafeRange && distance <= maxShootRange && cooldown <= 0f)
                    {
                        aimDirection = PredictAimDirection();
                        stateTimer = aimDuration;
                        state = State.Aim;
                    }
                    break;

                case State.Aim:
                    // Still kite while aiming if the player closes distance.
                    if (distance < minSafeRange)
                        controller.Move(FilterMoveByHitStagger(-direction * retreatSpeed * Time.deltaTime));

                    RotateToward(aimDirection);
                    stateTimer -= Time.deltaTime;

                    if (stateTimer <= 0f)
                        state = State.Shoot;
                    break;

                case State.Shoot:
                    Fire();
                    cooldown = shootCooldown;
                    stateTimer = recoverDuration;
                    state = State.Recover;
                    break;

                case State.Recover:
                    MaintainRange(direction, distance);
                    stateTimer -= Time.deltaTime;

                    if (stateTimer <= 0f)
                        state = State.Reposition;
                    break;
            }
        }

        private void MaintainRange(Vector3 direction, float distance)
        {
            if (distance < minSafeRange)
            {
                controller.Move(FilterMoveByHitStagger(-direction * retreatSpeed * Time.deltaTime));
                return;
            }

            if (distance > idealRange + 1.4f)
            {
                controller.Move(FilterMoveByHitStagger(direction * approachSpeed * Time.deltaTime));
                return;
            }

            Vector3 side = Vector3.Cross(Vector3.up, direction).normalized * strafeSign;
            controller.Move(FilterMoveByHitStagger(side * strafeSpeed * Time.deltaTime));
        }

        private Vector3 PredictAimDirection()
        {
            Vector3 toTarget = target.position - transform.position;
            toTarget.y = 0f;

            if (toTarget.sqrMagnitude < 0.001f)
                return transform.forward;

            return toTarget.normalized;
        }

        private void Fire()
        {
            Vector3 position = transform.position + transform.forward * projectileSpawnForwardOffset + Vector3.up * 0.8f;

            ShowAttackTelegraphBeforeDamage(true);
            GameObject projectile = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            projectile.name = "BD_RangedProjectile";
            projectile.transform.position = position;
            projectile.transform.localScale = Vector3.one * 0.42f;

            SphereCollider col = projectile.GetComponent<SphereCollider>();
            col.isTrigger = true;

            Material mat = new Material(Shader.Find("Standard"));
            mat.color = new Color(0.6f, 0.1f, 1f, 1f);
            projectile.GetComponent<Renderer>().material = mat;

            BDRangedProjectile logic = projectile.AddComponent<BDRangedProjectile>();
            logic.Configure(aimDirection, projectileSpeed, projectileDamage, transform);
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
