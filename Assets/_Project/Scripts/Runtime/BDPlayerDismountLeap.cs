using UnityEngine;

namespace BoredomAndDungeons
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(CharacterController))]
    public sealed class BDPlayerDismountLeap : MonoBehaviour
    {
        [SerializeField] private bool rotateTowardLeapDirection = true;

        private CharacterController characterController;
        private BDPlayerController playerController;

        private bool leaping;
        private Vector3 originalScale = Vector3.one;
        private Vector3 start;
        private Vector3 end;
        private Vector3 faceDirection;
        private float elapsed;
        private float duration = 0.24f;
        private float arcHeight = 0.5f;

        public bool IsLeaping => leaping;

        private void Awake()
        {
            characterController = GetComponent<CharacterController>();
            playerController = GetComponent<BDPlayerController>();
        }

        public void BeginLeap(Vector3 destination, float leapDuration, float leapArcHeight, Vector3 desiredFaceDirection)
        {
            if (characterController == null)
                characterController = GetComponent<CharacterController>();

            if (playerController == null)
                playerController = GetComponent<BDPlayerController>();

            originalScale = transform.localScale;
            start = transform.position;
            end = destination;

            duration = Mathf.Max(0.05f, leapDuration);
            arcHeight = Mathf.Max(0f, leapArcHeight);
            elapsed = 0f;

            faceDirection = desiredFaceDirection;
            faceDirection.y = 0f;

            if (faceDirection.sqrMagnitude < 0.001f)
            {
                faceDirection = end - start;
                faceDirection.y = 0f;
            }

            leaping = true;

            if (playerController != null)
                playerController.enabled = false;
        }

        private void Update()
        {
            if (!leaping)
                return;

            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);

            Vector3 flat = Vector3.Lerp(start, end, SmoothStep(t));
            float arc = Mathf.Sin(t * Mathf.PI) * arcHeight;
            Vector3 next = flat + Vector3.up * arc;

            Vector3 delta = next - transform.position;

            if (characterController != null && characterController.enabled)
                characterController.Move(delta);
            else
                transform.position = next;

            if (rotateTowardLeapDirection && faceDirection.sqrMagnitude > 0.001f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(faceDirection.normalized, Vector3.up);
                transform.rotation = Quaternion.Slerp(
                    transform.rotation,
                    targetRotation,
                    1f - Mathf.Exp(-18f * Time.deltaTime)
                );
            }

            if (t >= 1f)
                EndLeap();
        }

        private void EndLeap()
        {
            leaping = false;
            transform.position = end;
            transform.localScale = originalScale;

            if (playerController != null)
                playerController.enabled = true;
        }

        private static float SmoothStep(float t)
        {
            return t * t * (3f - 2f * t);
        }
    }
}
