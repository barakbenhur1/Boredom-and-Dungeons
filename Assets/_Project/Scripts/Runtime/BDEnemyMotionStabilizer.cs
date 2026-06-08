using UnityEngine;

namespace BoredomAndDungeons
{
    [DefaultExecutionOrder(450)]
    [DisallowMultipleComponent]
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(BDHealth))]
    public sealed class BDEnemyMotionStabilizer : MonoBehaviour
    {
        // BD SINGLE ENEMY MOTION STABILITY OWNER V23R10
        [SerializeField] private float ordinaryMaxSpeed = 8.75f;
        [SerializeField] private float chargerMaxSpeed = 14f;
        [SerializeField] private float exceptionalMaxSpeed = 20f;
        [SerializeField] private float frameSlack = 0.10f;
        [SerializeField] private float minimumFrameAllowance = 0.32f;
        [SerializeField] private float floatingRecoveryDelay = 0.18f;
        [SerializeField] private float floatingHeightTolerance = 0.62f;

        private CharacterController controller;
        private BDHealth health;
        private BDKnockbackReceiver knockback;
        private BDEnemyGroundStick groundStick;
        private BDJumperEnemy jumper;
        private BDChargerEnemy charger;
        private BDGrapplingHookPullState hookPull;
        private Vector3 previousPosition;
        private bool previousReady;
        private float floatingSince = -1f;
        private float nextGroundCheckAt;

        private void Awake()
        {
            controller = GetComponent<CharacterController>();
            health = GetComponent<BDHealth>();
            knockback = GetComponent<BDKnockbackReceiver>();
            groundStick = GetComponent<BDEnemyGroundStick>();
            jumper = GetComponent<BDJumperEnemy>();
            charger = GetComponent<BDChargerEnemy>();
            hookPull = GetComponent<BDGrapplingHookPullState>();
            previousPosition = transform.position;
            previousReady = true;
        }

        public void AcceptCurrentPositionAsBaseline()
        {
            previousPosition = transform.position;
            previousReady = true;
            floatingSince = -1f;
        }

        private void LateUpdate()
        {
            if (health != null && health.IsDead)
                return;

            if (!previousReady)
            {
                previousPosition = transform.position;
                previousReady = true;
                return;
            }

            if (hookPull == null)
                hookPull = GetComponent<BDGrapplingHookPullState>();

            bool authoredJump = jumper != null && jumper.IsPerformingAuthoredJump;
            bool exceptional = authoredJump ||
                               (knockback != null && knockback.IsBeingKnocked) ||
                               (hookPull != null && hookPull.IsActive);

            Vector3 current = transform.position;
            Vector3 delta = current - previousPosition;
            Vector3 horizontal = new Vector3(delta.x, 0f, delta.z);
            float maxSpeed = exceptional
                ? exceptionalMaxSpeed
                : charger != null
                    ? chargerMaxSpeed
                    : ordinaryMaxSpeed;
            float allowed = Mathf.Max(
                minimumFrameAllowance,
                maxSpeed * Mathf.Max(0.0001f, Time.deltaTime) + frameSlack
            );

            if (horizontal.magnitude > allowed)
            {
                Vector3 corrected = previousPosition + horizontal.normalized * allowed;
                corrected.y = current.y;
                MoveToCorrectedPosition(corrected);
                current = transform.position;
            }

            if (!authoredJump &&
                (hookPull == null || !hookPull.IsActive) &&
                Time.time >= nextGroundCheckAt)
            {
                nextGroundCheckAt = Time.time + 0.08f;
                RecoverIfFloating();
            }

            previousPosition = transform.position;
        }

        private void RecoverIfFloating()
        {
            if (groundStick == null || controller == null)
                return;

            if (!groundStick.TryFindGround(out RaycastHit groundHit))
            {
                floatingSince = -1f;
                return;
            }

            Vector3 desiredRoot =
                BDSafeEnemyPlacement.ResolveRootPositionForGround(
                    groundHit.point,
                    transform,
                    controller
                );
            float verticalError =
                Mathf.Abs(transform.position.y - desiredRoot.y);

            if (verticalError <= floatingHeightTolerance)
            {
                floatingSince = -1f;
                return;
            }

            if (floatingSince < 0f)
            {
                floatingSince = Time.time;
                return;
            }

            if (Time.time - floatingSince < floatingRecoveryDelay)
                return;

            if (knockback != null)
                knockback.ClearKnockback();
            groundStick.ForceSnapNow();
            floatingSince = -1f;
        }

        private void MoveToCorrectedPosition(Vector3 corrected)
        {
            if (controller != null && controller.enabled)
            {
                Vector3 correction = corrected - transform.position;
                controller.Move(correction);
                return;
            }

            transform.position = corrected;
        }
    }
}
