using System.Collections;
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
        [SerializeField] private float aggroRange = 18.5f;
        [SerializeField] private float moveSpeed = 3.35f;
        [SerializeField] private float circleSpeed = 1.75f;
        [SerializeField] private float rotationSpeed = 14f;
        [SerializeField] private float desiredDistance = 1.65f;
        [SerializeField] private float tooCloseDistance = 1.15f;
        [SerializeField] private float attackRange = 2.10f;
        [SerializeField] private float attackDamage = 12f;
        [SerializeField] private float attackCooldown = 1.05f;

        [Header("Sword visual")]
        [SerializeField] private float swordWindupDuration = 0.12f;
        [SerializeField] private float swordSlashDuration = 0.16f;
        [SerializeField] private float swordRecoveryDuration = 0.22f;
        [SerializeField] private float swordWindupAngle = 72f;
        [SerializeField] private float swordSlashAngle = 28f;
        [SerializeField] private float swordForwardReach = 0.22f;

        private CharacterController controller;
        private BDHitStaggerReceiver hitStaggerReceiver;
        private BDHealth health;
        private BDKnockbackReceiver knockback;
        private BDEnemyTacticalCommand tacticalCommand;

        private Transform leftSwordPivot;
        private Transform rightSwordPivot;
        private Quaternion leftSwordRestRotation;
        private Quaternion rightSwordRestRotation;
        private Vector3 leftSwordRestPosition;
        private Vector3 rightSwordRestPosition;
        private Coroutine swordAttackRoutine;

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

            EnsureSwordVisuals();
            CacheSwordRestPose();

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
            if (cooldown > 0f ||
                distance > attackRange ||
                BDGrapplingHookPullState.IsContactAttackSuppressed(transform))
            {
                return;
            }

            ShowAttackTelegraphBeforeDamage(false);
            PlayDoubleSlashVisual();

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

        private void EnsureSwordVisuals()
        {
            leftSwordPivot = FindOrCreateSword(
                "BD_Sword_Left_Pivot",
                new Vector3(-0.48f, 0.30f, 0.05f),
                -42f,
                new Color(0.72f, 0.82f, 0.90f, 1f)
            );

            rightSwordPivot = FindOrCreateSword(
                "BD_Sword_Right_Pivot",
                new Vector3(0.48f, 0.30f, 0.05f),
                42f,
                new Color(0.72f, 0.82f, 0.90f, 1f)
            );
        }

        private Transform FindOrCreateSword(
            string pivotName,
            Vector3 localPosition,
            float yaw,
            Color bladeColor)
        {
            Transform existing = transform.Find(pivotName);
            if (existing != null)
                return existing;

            GameObject pivotObject = new GameObject(pivotName);
            Transform pivot = pivotObject.transform;
            pivot.SetParent(transform, false);
            pivot.localPosition = localPosition;
            pivot.localRotation = Quaternion.Euler(0f, yaw, 0f);

            GameObject blade = GameObject.CreatePrimitive(PrimitiveType.Cube);
            blade.name = pivotName.Replace("Pivot", "Blade");
            blade.transform.SetParent(pivot, false);
            blade.transform.localPosition = new Vector3(0f, 0f, 0.59f);
            blade.transform.localScale = new Vector3(0.10f, 0.065f, 1.18f);
            SetVisualOnly(blade, bladeColor);

            GameObject guard = GameObject.CreatePrimitive(PrimitiveType.Cube);
            guard.name = pivotName.Replace("Pivot", "Guard");
            guard.transform.SetParent(pivot, false);
            guard.transform.localPosition = new Vector3(0f, 0f, 0.08f);
            guard.transform.localScale = new Vector3(0.38f, 0.10f, 0.10f);
            SetVisualOnly(guard, new Color(0.38f, 0.20f, 0.05f, 1f));

            GameObject handle = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            handle.name = pivotName.Replace("Pivot", "Handle");
            handle.transform.SetParent(pivot, false);
            handle.transform.localPosition = new Vector3(0f, 0f, -0.12f);
            handle.transform.localRotation = Quaternion.Euler(90f, 0f, 0f);
            handle.transform.localScale = new Vector3(0.07f, 0.20f, 0.07f);
            SetVisualOnly(handle, new Color(0.08f, 0.06f, 0.05f, 1f));

            return pivot;
        }

        private static void SetVisualOnly(GameObject visual, Color color)
        {
            Collider visualCollider = visual.GetComponent<Collider>();
            if (visualCollider != null)
                Destroy(visualCollider);

            Renderer visualRenderer = visual.GetComponent<Renderer>();
            if (visualRenderer == null)
                return;

            Material material = visualRenderer.material;
            material.color = color;

            if (material.HasProperty("_BaseColor"))
                material.SetColor("_BaseColor", color);

            if (material.HasProperty("_Color"))
                material.SetColor("_Color", color);
        }

        private void CacheSwordRestPose()
        {
            if (leftSwordPivot != null)
            {
                leftSwordRestRotation = leftSwordPivot.localRotation;
                leftSwordRestPosition = leftSwordPivot.localPosition;
            }

            if (rightSwordPivot != null)
            {
                rightSwordRestRotation = rightSwordPivot.localRotation;
                rightSwordRestPosition = rightSwordPivot.localPosition;
            }
        }

        private void RestoreSwordRestPose()
        {
            if (leftSwordPivot != null)
            {
                leftSwordPivot.localRotation = leftSwordRestRotation;
                leftSwordPivot.localPosition = leftSwordRestPosition;
            }

            if (rightSwordPivot != null)
            {
                rightSwordPivot.localRotation = rightSwordRestRotation;
                rightSwordPivot.localPosition = rightSwordRestPosition;
            }
        }

        private void PlayDoubleSlashVisual()
        {
            if (leftSwordPivot == null || rightSwordPivot == null)
                return;

            if (swordAttackRoutine != null)
                StopCoroutine(swordAttackRoutine);

            RestoreSwordRestPose();
            swordAttackRoutine = StartCoroutine(DoubleSlashRoutine());
        }

        private IEnumerator DoubleSlashRoutine()
        {
            Quaternion leftWindup = leftSwordRestRotation * Quaternion.Euler(0f, -swordWindupAngle, 0f);
            Quaternion rightWindup = rightSwordRestRotation * Quaternion.Euler(0f, swordWindupAngle, 0f);

            yield return AnimateSwordPose(
                leftSwordRestRotation,
                rightSwordRestRotation,
                leftWindup,
                rightWindup,
                leftSwordRestPosition,
                rightSwordRestPosition,
                swordWindupDuration
            );

            Quaternion leftSlash = leftSwordRestRotation * Quaternion.Euler(0f, swordSlashAngle, 0f);
            Quaternion rightSlash = rightSwordRestRotation * Quaternion.Euler(0f, -swordSlashAngle, 0f);

            yield return AnimateSwordPose(
                leftWindup,
                rightWindup,
                leftSlash,
                rightSlash,
                leftSwordRestPosition + Vector3.forward * swordForwardReach,
                rightSwordRestPosition + Vector3.forward * swordForwardReach,
                swordSlashDuration
            );

            yield return AnimateSwordPose(
                leftSlash,
                rightSlash,
                leftSwordRestRotation,
                rightSwordRestRotation,
                leftSwordRestPosition,
                rightSwordRestPosition,
                swordRecoveryDuration
            );

            RestoreSwordRestPose();
            swordAttackRoutine = null;
        }

        private IEnumerator AnimateSwordPose(
            Quaternion leftFrom,
            Quaternion rightFrom,
            Quaternion leftTo,
            Quaternion rightTo,
            Vector3 leftTargetPosition,
            Vector3 rightTargetPosition,
            float duration)
        {
            Vector3 leftStartPosition = leftSwordPivot.localPosition;
            Vector3 rightStartPosition = rightSwordPivot.localPosition;
            float safeDuration = Mathf.Max(0.01f, duration);
            float elapsed = 0f;

            while (elapsed < safeDuration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / safeDuration);
                float eased = t * t * (3f - 2f * t);

                leftSwordPivot.localRotation = Quaternion.Slerp(leftFrom, leftTo, eased);
                rightSwordPivot.localRotation = Quaternion.Slerp(rightFrom, rightTo, eased);
                leftSwordPivot.localPosition = Vector3.Lerp(leftStartPosition, leftTargetPosition, eased);
                rightSwordPivot.localPosition = Vector3.Lerp(rightStartPosition, rightTargetPosition, eased);

                yield return null;
            }

            leftSwordPivot.localRotation = leftTo;
            rightSwordPivot.localRotation = rightTo;
            leftSwordPivot.localPosition = leftTargetPosition;
            rightSwordPivot.localPosition = rightTargetPosition;
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

        private void OnDisable()
        {
            if (swordAttackRoutine != null)
            {
                StopCoroutine(swordAttackRoutine);
                swordAttackRoutine = null;
            }

            RestoreSwordRestPose();
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
