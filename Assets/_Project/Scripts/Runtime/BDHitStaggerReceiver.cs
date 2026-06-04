using UnityEngine;

namespace BoredomAndDungeons
{
    [DisallowMultipleComponent]
    public sealed class BDHitStaggerReceiver : MonoBehaviour
    {
        [SerializeField] private float maxStaggerDuration = 0.22f;
        [SerializeField] private float staggerVelocityDamping = 0.18f;

        private float staggerUntil;
        private CharacterController controller;
        private BDHealth health;

        public bool IsStaggered => Application.isPlaying && Time.time < staggerUntil;
        public float RemainingStaggerSeconds => !Application.isPlaying ? 0f : Mathf.Max(0f, staggerUntil - Time.time);

        private void Awake()
        {
            controller = GetComponent<CharacterController>();
            health = GetComponent<BDHealth>();
        }

        public void RequestStagger(float duration)
        {
            if (!Application.isPlaying)
                return;

            if (health != null && health.IsDead)
                return;

            duration = Mathf.Clamp(duration, 0f, maxStaggerDuration);
            if (duration <= 0f)
                return;

            staggerUntil = Mathf.Max(staggerUntil, Time.time + duration);
        }

        public Vector3 FilterMove(Vector3 requestedMove)
        {
            if (!IsStaggered)
                return requestedMove;

            return requestedMove * Mathf.Clamp01(staggerVelocityDamping);
        }

        public bool ShouldSkipAttack()
        {
            return IsStaggered;
        }

        public bool ShouldPauseEnemyBrain()
        {
            return IsStaggered;
        }

        private void Update()
        {
            if (!IsStaggered)
                return;

            if (controller != null && controller.enabled)
                controller.Move(Vector3.zero);
        }
    }
}
