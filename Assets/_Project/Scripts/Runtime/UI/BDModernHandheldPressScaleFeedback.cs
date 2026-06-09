using UnityEngine;

namespace BoredomAndDungeons
{
    [DisallowMultipleComponent]
    public sealed class BDModernHandheldPressScaleFeedback : MonoBehaviour
    {
        [SerializeField]
        private float pressedDepth = 0.12f;

        [SerializeField]
        private float pressedScale = 0.925f;

        [SerializeField]
        private float responseSpeed = 8.5f;

        private Vector3 restPosition;
        private Vector3 restScale;
        private Vector3 pressDirection = Vector3.forward;

        public void Configure(
            float depth,
            float speed,
            float scaleCompression)
        {
            pressedDepth = Mathf.Max(0f, depth);
            responseSpeed = Mathf.Max(0.01f, speed);
            pressedScale = 1f - Mathf.Clamp(
                scaleCompression,
                0f,
                0.20f
            );
        }

        private void Awake()
        {
            CaptureRestPose();
        }

        private void OnEnable()
        {
            CaptureRestPose();
        }

        private void OnDisable()
        {
            transform.localPosition = restPosition;
            transform.localScale = restScale;
        }

        private void LateUpdate()
        {
            Vector3 delta = transform.localPosition - restPosition;
            bool pressed = delta.sqrMagnitude > 0.0009f;

            if (pressed)
            {
                if (delta.sqrMagnitude > 0.000001f)
                    pressDirection = delta.normalized;

                transform.localPosition = Vector3.MoveTowards(
                    transform.localPosition,
                    restPosition + pressDirection * pressedDepth,
                    responseSpeed * Time.unscaledDeltaTime
                );
            }

            Vector3 desiredScale = pressed
                ? restScale * pressedScale
                : restScale;
            transform.localScale = Vector3.MoveTowards(
                transform.localScale,
                desiredScale,
                responseSpeed * Time.unscaledDeltaTime
            );
        }

        private void CaptureRestPose()
        {
            restPosition = transform.localPosition;
            restScale = transform.localScale;
        }
    }
}
