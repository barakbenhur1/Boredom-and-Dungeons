using UnityEngine;

namespace BoredomAndDungeons
{
    [DisallowMultipleComponent]
    public sealed class BDModernHandheldControlTarget : MonoBehaviour
    {
        public enum ControlAction
        {
            None,
            ScreenItem,
            DPadUp,
            DPadDown,
            DPadLeft,
            DPadRight,
            Confirm,
            Exit,
            Primary,
            Progression,
            ContextBackSettings,
            Credits
        }

        [SerializeField]
        private ControlAction action;

        [SerializeField]
        private int index = -1;

        [SerializeField]
        private Transform movingPart;

        [SerializeField]
        private float pressDistance = 0.12f;

        [SerializeField]
        private float pressSpeed = 8.5f;

        [SerializeField]
        [Range(0f, 0.20f)]
        private float pressedScaleCompression = 0.075f;

        [SerializeField]
        [Range(0f, 0.08f)]
        private float tutorialHighlightScale = 0.025f;

        private Vector3 restLocalPosition;
        private Vector3 restLocalScale;
        private float pressedUntil;
        private bool tutorialHighlighted;

        public ControlAction Action => action;
        public int Index => index;

        public void Configure(
            ControlAction newAction,
            int newIndex,
            Transform newMovingPart,
            Renderer newFeedbackRenderer,
            float newPressDistance)
        {
            action = newAction;
            index = newIndex;
            movingPart = newMovingPart;
            pressDistance = Mathf.Max(0f, newPressDistance);
            _ = newFeedbackRenderer;
            CaptureRestPose();
        }

        public void SetTactileProfile(
            float newPressDistance,
            float newPressSpeed,
            float newPressedScaleCompression)
        {
            pressDistance = Mathf.Max(0f, newPressDistance);
            pressSpeed = Mathf.Max(0.01f, newPressSpeed);
            pressedScaleCompression = Mathf.Clamp(
                newPressedScaleCompression,
                0f,
                0.20f
            );
            CaptureRestPose();
        }

        public void SetTutorialHighlighted(bool value)
        {
            tutorialHighlighted = value;
        }

        private void Awake()
        {
            CaptureRestPose();
        }

        private void OnDisable()
        {
            pressedUntil = 0f;
            tutorialHighlighted = false;

            if (movingPart == null)
                return;

            movingPart.localPosition = restLocalPosition;
            movingPart.localScale = restLocalScale;
        }

        private void Update()
        {
            if (movingPart == null || pressDistance <= 0f)
                return;

            bool pressed = Time.unscaledTime < pressedUntil;
            Vector3 targetPosition = restLocalPosition +
                Vector3.forward * (pressed ? pressDistance : 0f);

            float guidePulse = tutorialHighlighted && !pressed
                ? (Mathf.Sin(Time.unscaledTime * 4.2f) * 0.5f + 0.5f) *
                  tutorialHighlightScale
                : 0f;
            float scaleFactor = pressed
                ? 1f - pressedScaleCompression
                : 1f + guidePulse;
            Vector3 targetScale = restLocalScale * scaleFactor;
            float step = pressSpeed * Time.unscaledDeltaTime;

            movingPart.localPosition = Vector3.MoveTowards(
                movingPart.localPosition,
                targetPosition,
                step
            );
            movingPart.localScale = Vector3.MoveTowards(
                movingPart.localScale,
                targetScale,
                step
            );
        }

        public void Pulse(float duration = 0.12f)
        {
            pressedUntil = Mathf.Max(
                pressedUntil,
                Time.unscaledTime + Mathf.Max(0.04f, duration)
            );
        }

        public void SetHovered(bool value)
        {
            // Pointer hover intentionally has no blue frame, emission or depth.
            // Selection remains visible on the handheld screen; the hardware only
            // moves when it is actually pressed or tutorial-guided.
            _ = value;
        }

        private void CaptureRestPose()
        {
            if (movingPart == null)
                movingPart = transform;

            restLocalPosition = movingPart.localPosition;
            restLocalScale = movingPart.localScale;
        }
    }
}
