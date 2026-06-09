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

        private static readonly int EmissionColorId =
            Shader.PropertyToID("_EmissionColor");

        private Vector3 restPosition;
        private Vector3 restScale;
        private Vector3 pressDirection = Vector3.forward;
        private Renderer stateRenderer;
        private MaterialPropertyBlock propertyBlock;

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

            ResolveStateRenderer();
        }

        private void Awake()
        {
            CaptureRestPose();
            ResolveStateRenderer();
        }

        private void OnEnable()
        {
            CaptureRestPose();
            ResolveStateRenderer();
        }

        private void OnDisable()
        {
            transform.localPosition = restPosition;
            transform.localScale = restScale;
        }

        private void LateUpdate()
        {
            bool pressed = ReadPressedState();
            Vector3 delta = transform.localPosition - restPosition;

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

            // The existing control target remains the sole owner of release
            // travel. V6 only normalizes the visible pressed pose and scale,
            // so release can always continue back to the original rest pose.
            Vector3 desiredScale = pressed
                ? restScale * pressedScale
                : restScale;
            transform.localScale = Vector3.MoveTowards(
                transform.localScale,
                desiredScale,
                responseSpeed * Time.unscaledDeltaTime
            );
        }

        private bool ReadPressedState()
        {
            if (stateRenderer == null)
                ResolveStateRenderer();
            if (stateRenderer == null)
                return false;

            if (propertyBlock == null)
                propertyBlock = new MaterialPropertyBlock();

            stateRenderer.GetPropertyBlock(propertyBlock);
            Color emission = propertyBlock.GetColor(EmissionColorId);
            float strength = Mathf.Max(
                emission.r,
                Mathf.Max(emission.g, emission.b)
            );

            // Existing hover feedback peaks near 0.34 while an active press
            // reaches 0.85. This threshold keeps hover and press distinct.
            return strength >= 0.55f;
        }

        private void ResolveStateRenderer()
        {
            Renderer[] renderers =
                GetComponentsInChildren<Renderer>(true);
            for (int index = 0; index < renderers.Length; index++)
            {
                Renderer candidate = renderers[index];
                if (candidate == null)
                    continue;

                if (candidate.gameObject.name.Contains("Textured") ||
                    candidate.gameObject.name.Contains("Texture"))
                {
                    continue;
                }

                stateRenderer = candidate;
                return;
            }

            stateRenderer = GetComponent<Renderer>();
        }

        private void CaptureRestPose()
        {
            restPosition = transform.localPosition;
            restScale = transform.localScale;
        }
    }
}
