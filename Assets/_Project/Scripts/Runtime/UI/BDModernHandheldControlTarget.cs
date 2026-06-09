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
        private Renderer feedbackRenderer;

        [SerializeField]
        private float pressDistance = 0.10f;

        [SerializeField]
        private float pressSpeed = 2.4f;

        private Vector3 restLocalPosition;
        private float pressedUntil;
        private bool hovered;
        private MaterialPropertyBlock propertyBlock;

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
            feedbackRenderer = newFeedbackRenderer;
            pressDistance = Mathf.Max(0f, newPressDistance);

            CaptureRestPose();
            EnsurePropertyBlock();
            ApplyFeedback();
        }

        private void Awake()
        {
            CaptureRestPose();
            EnsurePropertyBlock();
        }

        private void OnDisable()
        {
            hovered = false;
            pressedUntil = 0f;

            if (movingPart != null)
                movingPart.localPosition = restLocalPosition;

            ApplyFeedback();
        }

        private void Update()
        {
            if (movingPart == null || pressDistance <= 0f)
            {
                ApplyFeedback();
                return;
            }

            bool pressed = Time.unscaledTime < pressedUntil;
            float depth = pressed
                ? pressDistance
                : hovered
                    ? pressDistance * 0.18f
                    : 0f;

            Vector3 target =
                restLocalPosition + Vector3.forward * depth;

            movingPart.localPosition =
                Vector3.MoveTowards(
                    movingPart.localPosition,
                    target,
                    pressSpeed * Time.unscaledDeltaTime
                );

            ApplyFeedback();
        }

        public void Pulse(float duration = 0.12f)
        {
            pressedUntil = Mathf.Max(
                pressedUntil,
                Time.unscaledTime + Mathf.Max(0.04f, duration)
            );
            ApplyFeedback();
        }

        public void SetHovered(bool value)
        {
            if (hovered == value)
                return;

            hovered = value;
            ApplyFeedback();
        }

        private void CaptureRestPose()
        {
            if (movingPart == null)
                movingPart = transform;

            restLocalPosition = movingPart.localPosition;
        }

        private void EnsurePropertyBlock()
        {
            if (propertyBlock == null)
                propertyBlock = new MaterialPropertyBlock();
        }

        private void ApplyFeedback()
        {
            if (feedbackRenderer == null)
                return;

            EnsurePropertyBlock();

            bool pressed = Time.unscaledTime < pressedUntil;
            float strength = pressed
                ? 0.85f
                : hovered
                    ? 0.34f
                    : 0f;

            Color emission = new Color(
                0.12f,
                0.46f,
                1f,
                1f
            ) * strength;

            feedbackRenderer.GetPropertyBlock(propertyBlock);
            propertyBlock.SetColor("_EmissionColor", emission);
            feedbackRenderer.SetPropertyBlock(propertyBlock);
        }
    }
}
