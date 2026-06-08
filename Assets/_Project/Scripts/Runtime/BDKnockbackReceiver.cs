using UnityEngine;

namespace BoredomAndDungeons
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(CharacterController))]
    public sealed class BDKnockbackReceiver : MonoBehaviour
    {
        [SerializeField] private float drag = 5.8f;
        [SerializeField] private float maxKnockbackSpeed = 18f;
        [SerializeField] private float minimumLockTime = 0.05f;

        private CharacterController characterController;
        private BDCombatantProfile combatantProfile;
        private Vector3 velocity;
        private float knockLockTimer;

        public bool IsBeingKnocked => knockLockTimer > 0f || velocity.sqrMagnitude > 0.04f;

        private void Awake()
        {
            characterController = GetComponent<CharacterController>();
            combatantProfile = GetComponent<BDCombatantProfile>();
        }

        private void Update()
        {
            if (knockLockTimer > 0f)
                knockLockTimer -= Time.deltaTime;

            if (velocity.sqrMagnitude <= 0.0001f)
                return;

            if (characterController != null && characterController.enabled)
                characterController.Move(velocity * Time.deltaTime);

            velocity = Vector3.Lerp(velocity, Vector3.zero, 1f - Mathf.Exp(-drag * Time.deltaTime));

            if (velocity.sqrMagnitude < 0.0025f)
                velocity = Vector3.zero;
        }

        public void AddKnockback(Vector3 direction, float strength)
        {
            AddKnockback(direction, strength, minimumLockTime);
        }

        public void AddKnockback(Vector3 direction, float strength, float lockDuration)
        {
            // BD NON-SMALL COMBATANT FORCED-MOVEMENT IMMUNITY V23R19B
            if (combatantProfile == null)
                combatantProfile = GetComponent<BDCombatantProfile>();

            if (combatantProfile != null &&
                !combatantProfile.ReceivesForcedMovement)
            {
                ClearKnockback();
                return;
            }

            direction.y = 0f;

            if (direction.sqrMagnitude < 0.001f)
                direction = transform.forward;

            direction.y = 0f;

            if (direction.sqrMagnitude < 0.001f)
                return;

            direction.Normalize();

            float safeStrength = Mathf.Max(0f, strength);
            velocity += direction * safeStrength;

            if (velocity.magnitude > maxKnockbackSpeed)
                velocity = velocity.normalized * maxKnockbackSpeed;

            knockLockTimer = Mathf.Max(knockLockTimer, Mathf.Max(minimumLockTime, lockDuration));

            BDEnemyHazardNavigation navigation =
                GetComponent<BDEnemyHazardNavigation>();
            navigation?.NotifyForcedHazardEntry(
                Mathf.Max(lockDuration, 0.85f)
            );
        }

        public void ClearKnockback()
        {
            velocity = Vector3.zero;
            knockLockTimer = 0f;
        }
    }
}
