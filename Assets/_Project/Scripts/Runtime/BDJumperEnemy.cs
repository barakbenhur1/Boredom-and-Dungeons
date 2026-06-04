using UnityEngine;

namespace BoredomAndDungeons
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(BDHealth))]
    public sealed class BDJumperEnemy : MonoBehaviour
    {
        private enum State { Idle, CreateDistance, StrafePrepare, Windup, Jumping, Recover }

        [SerializeField] private Transform target;

        [Header("Role: distance then leap")]
        [SerializeField] private float aggroRange = 17f;
        [SerializeField] private float preferredJumpDistance = 7.0f;
        [SerializeField] private float minJumpDistance = 5.0f;
        [SerializeField] private float maxJumpDistance = 9.6f;
        [SerializeField] private float tooCloseDistance = 3.35f;
        [SerializeField] private float approachSpeed = 2.7f;
        [SerializeField] private float retreatSpeed = 4.0f;
        [SerializeField] private float strafeSpeed = 2.45f;
        [SerializeField] private float rotationSpeed = 14f;

        [Header("Jump")]
        [SerializeField] private float windupDuration = 0.72f;
        [SerializeField] private float jumpDuration = 0.48f;
        [SerializeField] private float recoveryDuration = 1.25f;
        [SerializeField] private float jumpCooldown = 4.1f;
        [SerializeField] private float jumpArcHeight = 1.8f;
        [SerializeField] private float landingBehindOffset = 1.55f;
        [SerializeField] private float landingLeadOffset = 0.85f;
        [SerializeField] private float maxLandingDistance = 8.5f;
        [SerializeField] private float landingDamage = 18f;
        [SerializeField] private float landingHitRadius = 1.28f;

        [Header("Anti-wall / Grounding")]
        [SerializeField] private float wallCheckRadius = 1.15f;
        [SerializeField] private float landingGroundRayHeight = 6f;
        [SerializeField] private float landingGroundRayDistance = 11f;
        [SerializeField] private float fallbackSideLandingOffset = 2.2f;
        [SerializeField] private float fallbackBackLandingOffset = 3.2f;

        private CharacterController controller;
        private BDHitStaggerReceiver hitStaggerReceiver;
        private BDHealth health;
        private BDKnockbackReceiver knockback;
        private BDEnemyGroundStick groundStick;

        private State state = State.Idle;

        private float stateTimer;
        private float jumpCooldownTimer;
        private float strafeSign = 1f;
        private float strafeSwitchTimer;

        private Vector3 jumpStart;
        private Vector3 jumpEnd;
        private float jumpElapsed;
        private bool didDamage;

        private void Awake()
        {
            hitStaggerReceiver = GetComponent<BDHitStaggerReceiver>();
            controller = GetComponent<CharacterController>();
            health = GetComponent<BDHealth>();
            knockback = GetComponent<BDKnockbackReceiver>();
            groundStick = GetComponent<BDEnemyGroundStick>();
            if (groundStick == null)
                groundStick = gameObject.AddComponent<BDEnemyGroundStick>();

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

            if (jumpCooldownTimer > 0f)
                jumpCooldownTimer -= Time.deltaTime;

            if (strafeSwitchTimer > 0f)
                strafeSwitchTimer -= Time.deltaTime;
            else
            {
                strafeSwitchTimer = Random.Range(1.1f, 2.0f);
                if (Random.value < 0.5f)
                    strafeSign *= -1f;
            }

            if (knockback != null && knockback.IsBeingKnocked)
                return;

            

            if (hitStaggerReceiver != null && hitStaggerReceiver.ShouldPauseEnemyBrain())
                return;TickJumper();
        }

        private void TickJumper()
        {
            Vector3 toTarget = target.position - transform.position;
            toTarget.y = 0f;
            float distance = toTarget.magnitude;
            Vector3 direction = distance > 0.001f ? toTarget.normalized : transform.forward;

            if (distance > aggroRange)
            {
                state = State.Idle;
                return;
            }

            switch (state)
            {
                case State.Idle:
                    state = State.CreateDistance;
                    break;

                case State.CreateDistance:
                    RotateToward(direction);

                    if (distance < tooCloseDistance)
                    {
                        controller.Move(FilterMoveByHitStagger(-direction * retreatSpeed * Time.deltaTime));
                        break;
                    }

                    if (distance > preferredJumpDistance + 0.9f)
                    {
                        controller.Move(FilterMoveByHitStagger(direction * approachSpeed * Time.deltaTime));
                        break;
                    }

                    state = State.StrafePrepare;
                    break;

                case State.StrafePrepare:
                    RotateToward(direction);

                    if (distance < minJumpDistance)
                    {
                        state = State.CreateDistance;
                        break;
                    }

                    if (distance > maxJumpDistance)
                    {
                        controller.Move(FilterMoveByHitStagger(direction * approachSpeed * Time.deltaTime));
                        break;
                    }

                    Vector3 side = Vector3.Cross(Vector3.up, direction).normalized * strafeSign;
                    controller.Move(FilterMoveByHitStagger(side * strafeSpeed * Time.deltaTime));

                    if (jumpCooldownTimer <= 0f)
                    {
                        if (TryPrepareLanding(direction, out Vector3 safeLanding))
                            BeginWindup(safeLanding);
                        else
                            jumpCooldownTimer = 0.85f;
                    }
                    break;

                case State.Windup:
                    RotateToward(jumpEnd - transform.position);
                    stateTimer -= Time.deltaTime;

                    if (stateTimer <= 0f)
                        BeginJump();
                    break;

                case State.Jumping:
                    TickJump();
                    break;

                case State.Recover:
                    groundStick?.ForceSnapNow();

                    RotateToward(direction);
                    stateTimer -= Time.deltaTime;

                    if (distance < preferredJumpDistance)
                        controller.Move(FilterMoveByHitStagger(-direction * retreatSpeed * 0.8f * Time.deltaTime));

                    if (stateTimer <= 0f)
                        state = State.CreateDistance;
                    break;
            }
        }

        private void BeginWindup(Vector3 safeLanding)
        {
            jumpEnd = safeLanding;
            stateTimer = windupDuration;
            state = State.Windup;
            RotateToward(jumpEnd - transform.position);
        }

        private bool TryPrepareLanding(Vector3 directionToTarget, out Vector3 landing)
        {
            Vector3 playerForward = target.forward;
            playerForward.y = 0f;
            playerForward = playerForward.sqrMagnitude > 0.001f ? playerForward.normalized : directionToTarget;

            Vector3 playerRight = Vector3.Cross(Vector3.up, playerForward).normalized;

            Vector3[] candidates =
            {
                target.position - playerForward * landingBehindOffset,
                target.position + directionToTarget * landingLeadOffset,
                target.position - directionToTarget * fallbackBackLandingOffset,
                target.position + playerRight * fallbackSideLandingOffset,
                target.position - playerRight * fallbackSideLandingOffset
            };

            foreach (Vector3 candidate in candidates)
            {
                if (TryValidateLanding(candidate, out landing))
                    return true;
            }

            landing = transform.position;
            return false;
        }

        private bool TryValidateLanding(Vector3 candidate, out Vector3 landing)
        {
            Vector3 flatCandidate = candidate;
            flatCandidate.y = transform.position.y;

            Vector3 fromEnemy = flatCandidate - transform.position;
            fromEnemy.y = 0f;

            if (fromEnemy.magnitude > maxLandingDistance)
                flatCandidate = transform.position + fromEnemy.normalized * maxLandingDistance;

            if (!TrySnapCandidateToGround(flatCandidate, out Vector3 grounded))
            {
                landing = transform.position;
                return false;
            }

            if (IsNearWall(grounded))
            {
                landing = transform.position;
                return false;
            }

            if (JumpPathHitsWall(grounded))
            {
                landing = transform.position;
                return false;
            }

            landing = grounded;
            return true;
        }

        private bool TrySnapCandidateToGround(Vector3 candidate, out Vector3 grounded)
        {
            Vector3 origin = candidate + Vector3.up * landingGroundRayHeight;
            RaycastHit[] hits = Physics.RaycastAll(origin, Vector3.down, landingGroundRayDistance, ~0, QueryTriggerInteraction.Ignore);

            System.Array.Sort(hits, (a, b) => a.distance.CompareTo(b.distance));

            foreach (RaycastHit hit in hits)
            {
                if (hit.collider == null)
                    continue;

                if (hit.collider.GetComponentInParent<BDOccludingWall>() != null)
                    continue;

                if (hit.collider.GetComponentInParent<BDHealth>() != null)
                    continue;

                if (hit.normal.y < 0.65f)
                    continue;

                grounded = hit.point;
                grounded.y += Mathf.Max(controller.height * 0.5f, controller.radius) + 0.02f;
                return true;
            }

            grounded = candidate;
            return false;
        }

        private bool IsNearWall(Vector3 point)
        {
            Collider[] hits = Physics.OverlapSphere(point, wallCheckRadius, ~0, QueryTriggerInteraction.Ignore);

            foreach (Collider hit in hits)
            {
                if (hit == null)
                    continue;

                if (hit.GetComponentInParent<BDOccludingWall>() != null || hit.name.Contains("Wall"))
                    return true;
            }

            return false;
        }

        private bool JumpPathHitsWall(Vector3 landing)
        {
            Vector3 start = transform.position + Vector3.up * 0.8f;
            Vector3 end = landing + Vector3.up * 0.8f;
            Vector3 direction = end - start;
            float distance = direction.magnitude;

            if (distance <= 0.01f)
                return false;

            RaycastHit[] hits = Physics.SphereCastAll(
                start,
                controller.radius * 0.75f,
                direction.normalized,
                distance,
                ~0,
                QueryTriggerInteraction.Ignore
            );

            foreach (RaycastHit hit in hits)
            {
                if (hit.collider == null)
                    continue;

                if (hit.collider.GetComponentInParent<BDOccludingWall>() != null || hit.collider.name.Contains("Wall"))
                    return true;
            }

            return false;
        }

        private void BeginJump()
        {
            jumpStart = transform.position;
            jumpElapsed = 0f;
            didDamage = false;
            jumpCooldownTimer = jumpCooldown;
            groundStick?.DisableFor(jumpDuration + 0.1f);
            state = State.Jumping;
        }

        private void TickJump()
        {
            jumpElapsed += Time.deltaTime;
            float t = Mathf.Clamp01(jumpElapsed / Mathf.Max(0.01f, jumpDuration));

            Vector3 flat = Vector3.Lerp(jumpStart, jumpEnd, t);
            float arc = Mathf.Sin(t * Mathf.PI) * jumpArcHeight;
            Vector3 next = flat + Vector3.up * arc;
            Vector3 delta = next - transform.position;

            controller.Move(FilterMoveByHitStagger(delta));
            RotateToward(jumpEnd - transform.position);
            TryLandingDamage(false);

            if (t >= 1f)
            {
                TryLandingDamage(true);
                groundStick?.ForceSnapNow();
                stateTimer = recoveryDuration;
                state = State.Recover;
            }
        }

        private void TryLandingDamage(bool force)
        {
            if (didDamage || target == null)
                return;

            float distance = Vector3.Distance(transform.position, target.position);
            float allowed = force ? landingHitRadius * 1.15f : landingHitRadius;

            if (distance > allowed)
                return;

            BDHealth targetHealth = target.GetComponent<BDHealth>();
            if (targetHealth != null)
                ShowAttackTelegraphBeforeDamage(false);
                targetHealth.ApplyDamage(landingDamage);

            BDHorseDamageUtility.TryDamageHorseNear(transform.position, landingHitRadius, landingDamage, transform);

            didDamage = true;
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
