using UnityEngine;

#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace BoredomAndDungeons
{
    public sealed partial class BDModernHandheld3DPresenter
    {
        private enum ChildApproachScreenPowerState
        {
            On,
            Off,
            PoweringOn
        }

        private const float ChildApproachWalkEndsAtSeconds = 3.65f;
        private const float ChildApproachClimbStartsAtSeconds = 4.10f;
        private const float ChildApproachSeatReachSeconds = 5.85f;
        private const float ChildApproachCameraSettleSeconds = 6.75f;
        private const float ChildApproachPowerOnStartsAtSeconds = 7.00f;
        private const float ChildApproachPowerOnEndsAtSeconds = 8.05f;

        // Authored around the V10.9.26 chair: well behind it and clearly
        // offset to the left, while preserving the established child POV
        // height above the physical room floor.
        private static readonly Vector3 ChildApproachStartCameraPosition =
            new Vector3(-3.05f, -9.58f, -21.60f);
        private static readonly Vector3 ChildApproachWalkEndCameraPosition =
            new Vector3(-2.88f, -9.54f, -15.55f);
        private static readonly Vector3 ChildApproachClimbPrepCameraPosition =
            new Vector3(-2.65f, -9.40f, -14.05f);
        private static readonly Vector3 ChildApproachFirstLiftCameraPosition =
            new Vector3(-2.60f, -8.70f, -12.95f);
        private static readonly Vector3 ChildApproachSeatEdgeCameraPosition =
            new Vector3(-2.55f, -7.75f, -11.60f);
        private static readonly Vector3 ChildApproachSeatCameraPosition =
            new Vector3(-1.10f, -7.55f, -11.15f);
        private static readonly Vector3 ChildApproachLeanCameraPosition =
            new Vector3(-0.05f, -4.90f, -7.10f);

        private static readonly Vector3 ChildApproachStartLookTarget =
            new Vector3(-1.25f, -7.35f, -13.00f);
        private static readonly Vector3 ChildApproachWalkLookTarget =
            new Vector3(-0.65f, -7.45f, -11.65f);
        private static readonly Vector3 ChildApproachClimbLookTarget =
            new Vector3(-0.55f, -7.25f, -10.35f);
        private static readonly Vector3 ChildApproachFirstLiftLookTarget =
            new Vector3(-0.30f, -6.90f, -8.20f);
        private static readonly Vector3 ChildApproachSeatEdgeLookTarget =
            new Vector3(0f, -7.05f, -5.60f);
        private static readonly Vector3 ChildApproachSeatLookTarget =
            new Vector3(0f, -7.18f, -4.45f);

        private ChildApproachScreenPowerState childApproachScreenPowerState;
        private bool childApproachDisplayStateCaptured;
        private Color childApproachDisplayOnColor = Color.white;
        private Color childApproachDisplayOnEmission = Color.black;
        private float childApproachDisplayOnBrightness = 1f;
        private bool childApproachContentTransformCaptured;
        private Vector3 childApproachContentRestLocalPosition;
        private Vector3 childApproachContentRestLocalScale = Vector3.one;

        private void InitializeChildApproachCinematicState()
        {
            childApproachScreenPowerState =
                ChildApproachScreenPowerState.On;
            childApproachDisplayStateCaptured = false;
            childApproachContentTransformCaptured = false;
        }

        private void ResetChildApproachCinematicState()
        {
            childApproachScreenPowerState =
                ChildApproachScreenPowerState.On;
        }

        private void ApplyChildApproachCameraPose(float progress)
        {
            if (deviceCamera == null)
                return;

            float normalized = Mathf.Clamp01(progress);
            float seconds = normalized * IntroMainMenuTotalSeconds;
            Vector3 cameraPosition;
            Vector3 lookTarget;
            float rollDegrees = 0f;
            float finalRotationBlend = 0f;
            float lensBlend = 0f;

            if (seconds <= ChildApproachWalkEndsAtSeconds)
            {
                float raw = Mathf.InverseLerp(
                    0f,
                    ChildApproachWalkEndsAtSeconds,
                    seconds
                );
                float t = SmoothestStep01(raw);
                cameraPosition = Vector3.Lerp(
                    ChildApproachStartCameraPosition,
                    ChildApproachWalkEndCameraPosition,
                    t
                );
                lookTarget = Vector3.Lerp(
                    ChildApproachStartLookTarget,
                    ChildApproachWalkLookTarget,
                    t
                );

                // Six restrained child steps over the longer approach. The
                // camera starts well behind and left of the chair, preserves the
                // established POV height, and advances with grounded weight
                // transfer instead of a low floating bob.
                float stepEnvelope = Mathf.Sin(Mathf.PI * raw);
                float stepPhase = raw * Mathf.PI * 12f;
                float heelToToe = Mathf.Sin(stepPhase);
                cameraPosition.y +=
                    heelToToe * 0.034f * stepEnvelope;
                cameraPosition.z +=
                    Mathf.Sin(stepPhase * 0.5f + 0.35f) *
                    0.018f * stepEnvelope;
                cameraPosition.x +=
                    Mathf.Sin(stepPhase * 0.5f + 0.65f) *
                    0.016f * stepEnvelope;
                rollDegrees =
                    Mathf.Sin(stepPhase * 0.5f) *
                    0.22f * stepEnvelope;
            }
            else if (seconds <= ChildApproachClimbStartsAtSeconds)
            {
                float t = SmoothestStep01(Mathf.InverseLerp(
                    ChildApproachWalkEndsAtSeconds,
                    ChildApproachClimbStartsAtSeconds,
                    seconds
                ));
                cameraPosition = Vector3.Lerp(
                    ChildApproachWalkEndCameraPosition,
                    ChildApproachClimbPrepCameraPosition,
                    t
                );
                lookTarget = Vector3.Lerp(
                    ChildApproachWalkLookTarget,
                    ChildApproachClimbLookTarget,
                    t
                );
                rollDegrees = Mathf.Lerp(0f, -0.65f, t);
            }
            else if (seconds <= ChildApproachSeatReachSeconds)
            {
                float climbRaw = Mathf.InverseLerp(
                    ChildApproachClimbStartsAtSeconds,
                    ChildApproachSeatReachSeconds,
                    seconds
                );

                if (climbRaw < 0.34f)
                {
                    float t = SmoothestStep01(climbRaw / 0.34f);
                    cameraPosition = EvaluateQuadraticBezier(
                        ChildApproachClimbPrepCameraPosition,
                        new Vector3(-2.82f, -9.10f, -13.58f),
                        ChildApproachFirstLiftCameraPosition,
                        t
                    );
                    lookTarget = EvaluateQuadraticBezier(
                        ChildApproachClimbLookTarget,
                        new Vector3(-0.46f, -7.08f, -9.18f),
                        ChildApproachFirstLiftLookTarget,
                        t
                    );
                }
                else if (climbRaw < 0.72f)
                {
                    float t = SmoothestStep01(
                        (climbRaw - 0.34f) / 0.38f
                    );
                    cameraPosition = EvaluateQuadraticBezier(
                        ChildApproachFirstLiftCameraPosition,
                        new Vector3(-2.72f, -8.16f, -12.30f),
                        ChildApproachSeatEdgeCameraPosition,
                        t
                    );
                    lookTarget = EvaluateQuadraticBezier(
                        ChildApproachFirstLiftLookTarget,
                        new Vector3(-0.18f, -6.88f, -6.82f),
                        ChildApproachSeatEdgeLookTarget,
                        t
                    );
                }
                else
                {
                    float t = SmoothestStep01(
                        (climbRaw - 0.72f) / 0.28f
                    );
                    cameraPosition = EvaluateQuadraticBezier(
                        ChildApproachSeatEdgeCameraPosition,
                        new Vector3(-2.02f, -7.45f, -11.30f),
                        ChildApproachSeatCameraPosition,
                        t
                    );
                    lookTarget = EvaluateQuadraticBezier(
                        ChildApproachSeatEdgeLookTarget,
                        new Vector3(0f, -7.02f, -5.02f),
                        ChildApproachSeatLookTarget,
                        t
                    );
                }

                // Until the camera has cleared the backrest and reached the
                // seat-side position, keep it physically outside the chair's
                // left edge. Only then may it move inward over the seat.
                if (climbRaw < 0.72f)
                {
                    float chairLeftClearanceX =
                        CinematicChairCenterX -
                        CinematicChairSeatWidth * 0.5f -
                        0.30f;
                    cameraPosition.x = Mathf.Min(
                        cameraPosition.x,
                        chairLeftClearanceX
                    );
                }

                float effort = Mathf.Sin(Mathf.PI * climbRaw);
                float braceDip = Mathf.Sin(
                    Mathf.PI * Mathf.Clamp01(climbRaw / 0.34f)
                );
                cameraPosition.y += effort * 0.038f;
                cameraPosition.y -= braceDip * 0.060f;
                cameraPosition.x +=
                    Mathf.Sin(climbRaw * Mathf.PI * 2f) * 0.012f;
                rollDegrees =
                    Mathf.Sin(climbRaw * Mathf.PI * 2f) *
                    0.26f * (1f - climbRaw * 0.55f);
            }
            else if (seconds <= ChildApproachCameraSettleSeconds)
            {
                float settleRaw = Mathf.InverseLerp(
                    ChildApproachSeatReachSeconds,
                    ChildApproachCameraSettleSeconds,
                    seconds
                );

                if (settleRaw < 0.62f)
                {
                    float t = SmoothestStep01(settleRaw / 0.62f);
                    cameraPosition = Vector3.Lerp(
                        ChildApproachSeatCameraPosition,
                        ChildApproachLeanCameraPosition,
                        t
                    );
                    lookTarget = Vector3.Lerp(
                        ChildApproachSeatLookTarget,
                        RegularMainMenuLookTarget,
                        t * 0.72f
                    );
                }
                else
                {
                    float t = SmoothestStep01(
                        (settleRaw - 0.62f) / 0.38f
                    );
                    cameraPosition = Vector3.Lerp(
                        ChildApproachLeanCameraPosition,
                        RegularMainMenuCameraPosition,
                        t
                    );
                    lookTarget = Vector3.Lerp(
                        Vector3.Lerp(
                            ChildApproachSeatLookTarget,
                            RegularMainMenuLookTarget,
                            0.72f
                        ),
                        RegularMainMenuLookTarget,
                        t
                    );
                }

                float balanceEnvelope = 1f - settleRaw;
                cameraPosition.x +=
                    Mathf.Sin(settleRaw * Mathf.PI * 4f) *
                    0.018f * balanceEnvelope;
                rollDegrees =
                    Mathf.Sin(settleRaw * Mathf.PI * 3f) *
                    0.24f * balanceEnvelope;
                finalRotationBlend = SmoothestStep01(
                    Mathf.InverseLerp(0.68f, 1f, settleRaw)
                );
                lensBlend = SmoothestStep01(
                    Mathf.InverseLerp(0.40f, 1f, settleRaw)
                );
            }
            else
            {
                cameraPosition = RegularMainMenuCameraPosition;
                lookTarget = RegularMainMenuLookTarget;
                finalRotationBlend = 1f;
                lensBlend = 1f;
            }

            // The authored path clears the chair front, rises above the seat,
            // then leans over the table. Keep a hard floor safety margin too.
            float roomFloorY = TableRestPosition.y - 14.84f;
            cameraPosition.y = Mathf.Max(
                roomFloorY + 4.65f,
                cameraPosition.y
            );

            Vector3 cameraDirection = lookTarget - cameraPosition;
            Quaternion trackedRotation = Quaternion.LookRotation(
                cameraDirection.normalized,
                Vector3.up
            );
            trackedRotation *= Quaternion.Euler(0f, 0f, rollDegrees);
            Quaternion cameraRotation = Quaternion.Slerp(
                trackedRotation,
                RegularMainMenuCameraRotation,
                finalRotationBlend
            );

            deviceCamera.transform.localPosition = cameraPosition;
            deviceCamera.transform.localRotation = cameraRotation;
            deviceCamera.fieldOfView = Mathf.Lerp(
                introToMainMenuStartFieldOfView,
                introToMainMenuFinalFieldOfView,
                lensBlend
            );
            deviceCamera.nearClipPlane = IntroMainMenuCameraNearClip;
            deviceCamera.farClipPlane = IntroMainMenuCameraFarClip;
        }

        private static Vector3 EvaluateQuadraticBezier(
            Vector3 start,
            Vector3 control,
            Vector3 end,
            float progress)
        {
            float t = Mathf.Clamp01(progress);
            float inverse = 1f - t;
            return inverse * inverse * start +
                   2f * inverse * t * control +
                   t * t * end;
        }

        private void CaptureChildApproachDisplayState()
        {
            if (childApproachDisplayStateCaptured || displayMaterial == null)
                return;

            if (displayMaterial.HasProperty("_Color"))
                childApproachDisplayOnColor =
                    displayMaterial.GetColor("_Color");
            if (displayMaterial.HasProperty("_EmissionColor"))
                childApproachDisplayOnEmission =
                    displayMaterial.GetColor("_EmissionColor");
            if (displayMaterial.HasProperty("_Brightness"))
                childApproachDisplayOnBrightness =
                    displayMaterial.GetFloat("_Brightness");

            if (!childApproachContentTransformCaptured &&
                pageCanvasGroup != null)
            {
                childApproachContentRestLocalPosition =
                    pageCanvasGroup.transform.localPosition;
                childApproachContentRestLocalScale =
                    pageCanvasGroup.transform.localScale;
                childApproachContentTransformCaptured = true;
            }

            childApproachDisplayStateCaptured = true;
        }

        private void SetChildApproachScreenOff()
        {
            CaptureChildApproachDisplayState();
            childApproachScreenPowerState =
                ChildApproachScreenPowerState.Off;

            if (screenCanvasRoot != null)
                screenCanvasRoot.SetActive(false);
            if (screenCamera != null)
                screenCamera.enabled = false;
            if (screenScanlineRoot != null)
                screenScanlineRoot.gameObject.SetActive(false);
            if (screenTransitionRoot != null)
                screenTransitionRoot.gameObject.SetActive(false);
            if (screenHitTargetRoot != null)
                screenHitTargetRoot.gameObject.SetActive(false);
            ApplyChildApproachContentReveal(0f);
            if (pageCanvasGroup != null)
            {
                pageCanvasGroup.blocksRaycasts = false;
                pageCanvasGroup.interactable = false;
            }

            ApplyChildApproachDisplayPower(0f);
        }

        private void UpdateChildApproachScreenPower(float progress)
        {
            float seconds = Mathf.Clamp01(progress) *
                            IntroMainMenuTotalSeconds;
            if (seconds < ChildApproachPowerOnStartsAtSeconds)
            {
                SetChildApproachScreenOff();
                return;
            }

            float powerProgress = Mathf.InverseLerp(
                ChildApproachPowerOnStartsAtSeconds,
                ChildApproachPowerOnEndsAtSeconds,
                seconds
            );
            ApplyChildApproachScreenPowerOnProgress(powerProgress);
        }

        private void ApplyChildApproachScreenPowerOnProgress(float progress)
        {
            float t = Mathf.Clamp01(progress);
            if (childApproachScreenPowerState ==
                ChildApproachScreenPowerState.Off)
            {
                childApproachScreenPowerState =
                    ChildApproachScreenPowerState.PoweringOn;

                if (screenCanvasRoot != null)
                    screenCanvasRoot.SetActive(true);
                if (screenCamera != null)
                    screenCamera.enabled = visible;
                if (screenScanlineRoot != null)
                    screenScanlineRoot.gameObject.SetActive(false);
                if (screenHitTargetRoot != null)
                    screenHitTargetRoot.gameObject.SetActive(false);
                ApplyChildApproachContentReveal(0f);
                if (pageCanvasGroup != null)
                {
                    pageCanvasGroup.blocksRaycasts = false;
                    pageCanvasGroup.interactable = false;
                }
                if (screenTransitionRoot != null)
                    screenTransitionRoot.gameObject.SetActive(true);

                // Render while fully hidden so the first visible content frame
                // is already complete and never pops into existence.
                ForceScreenRender();
            }

            float glassWake = SmoothestStep01(
                Mathf.InverseLerp(0.02f, 0.58f, t)
            );
            ApplyChildApproachDisplayPower(glassWake);

            // Content is deliberately delayed until the glass and backlight are
            // established. It feeds in with a restrained fade, vertical settle
            // and micro-scale correction instead of appearing mid-power-on.
            float contentReveal = SmoothestStep01(
                Mathf.InverseLerp(0.56f, 0.97f, t)
            );
            ApplyChildApproachContentReveal(contentReveal);

            if (screenScanlineRoot != null)
            {
                screenScanlineRoot.gameObject.SetActive(t >= 0.54f);
            }

            float shutterOpen = SmoothestStep01(
                Mathf.InverseLerp(0.10f, 0.62f, t)
            );
            if (transitionTopShutter != null)
            {
                transitionTopShutter.color =
                    new Color(0.002f, 0.006f, 0.018f, 1f);
                transitionTopShutter.rectTransform.anchoredPosition =
                    new Vector2(
                        0f,
                        CanvasSize.y * (0.25f + 0.52f * shutterOpen)
                    );
            }
            if (transitionBottomShutter != null)
            {
                transitionBottomShutter.color =
                    new Color(0.002f, 0.006f, 0.018f, 1f);
                transitionBottomShutter.rectTransform.anchoredPosition =
                    new Vector2(
                        0f,
                        -CanvasSize.y * (0.25f + 0.52f * shutterOpen)
                    );
            }

            float scanProgress = Mathf.Clamp01(
                Mathf.InverseLerp(0.42f, 0.92f, t)
            );
            if (transitionLine != null)
            {
                float linePulse = Mathf.Sin(Mathf.PI * scanProgress);
                transitionLine.color = new Color(
                    0.16f,
                    0.68f,
                    1f,
                    linePulse * 0.68f
                );
                transitionLine.rectTransform.anchoredPosition =
                    new Vector2(
                        0f,
                        Mathf.Lerp(
                            -CanvasSize.y * 0.42f,
                            CanvasSize.y * 0.42f,
                            scanProgress
                        )
                    );
                transitionLine.rectTransform.sizeDelta =
                    new Vector2(
                        CanvasSize.x * 0.94f,
                        Mathf.Lerp(3f, 7f, linePulse)
                    );
            }
            if (transitionFlash != null)
            {
                float blueWake = Mathf.Sin(Mathf.PI * t) * 0.028f;
                transitionFlash.color = new Color(
                    0.04f,
                    0.18f,
                    0.42f,
                    blueWake
                );
            }

            if (t < 1f)
                return;

            childApproachScreenPowerState =
                ChildApproachScreenPowerState.On;
            ApplyChildApproachDisplayPower(1f);
            ApplyChildApproachContentReveal(1f);
            if (pageCanvasGroup != null)
            {
                pageCanvasGroup.blocksRaycasts = true;
                pageCanvasGroup.interactable = true;
            }
            if (screenHitTargetRoot != null)
                screenHitTargetRoot.gameObject.SetActive(true);
            if (screenTransitionRoot != null)
                screenTransitionRoot.gameObject.SetActive(false);
        }

        private void ApplyChildApproachContentReveal(float reveal)
        {
            if (pageCanvasGroup == null)
                return;

            CaptureChildApproachDisplayState();
            float t = Mathf.Clamp01(reveal);
            pageCanvasGroup.alpha = t;

            if (!childApproachContentTransformCaptured)
                return;

            Vector3 hiddenPosition =
                childApproachContentRestLocalPosition +
                new Vector3(0f, -22f, 0f);
            pageCanvasGroup.transform.localPosition = Vector3.Lerp(
                hiddenPosition,
                childApproachContentRestLocalPosition,
                t
            );
            pageCanvasGroup.transform.localScale = Vector3.Lerp(
                childApproachContentRestLocalScale * 0.988f,
                childApproachContentRestLocalScale,
                t
            );
        }

        private void ApplyChildApproachDisplayPower(float power)
        {
            if (displayMaterial == null)
                return;

            CaptureChildApproachDisplayState();
            float t = Mathf.Clamp01(power);
            Color poweredOffGlass =
                new Color(0.002f, 0.004f, 0.009f, 1f);

            if (displayMaterial.HasProperty("_Color"))
            {
                displayMaterial.SetColor(
                    "_Color",
                    Color.Lerp(
                        poweredOffGlass,
                        childApproachDisplayOnColor,
                        t
                    )
                );
            }
            if (displayMaterial.HasProperty("_EmissionColor"))
            {
                displayMaterial.SetColor(
                    "_EmissionColor",
                    Color.Lerp(
                        Color.black,
                        childApproachDisplayOnEmission,
                        t
                    )
                );
            }
            if (displayMaterial.HasProperty("_Brightness"))
            {
                displayMaterial.SetFloat(
                    "_Brightness",
                    Mathf.Lerp(0f, childApproachDisplayOnBrightness, t)
                );
            }
        }

        private void ForceChildApproachScreenOnImmediate()
        {
            CaptureChildApproachDisplayState();
            childApproachScreenPowerState =
                ChildApproachScreenPowerState.On;

            if (screenCanvasRoot != null)
                screenCanvasRoot.SetActive(true);
            if (screenCamera != null)
                screenCamera.enabled = visible;
            if (screenScanlineRoot != null)
                screenScanlineRoot.gameObject.SetActive(true);
            if (screenHitTargetRoot != null)
                screenHitTargetRoot.gameObject.SetActive(true);
            ApplyChildApproachContentReveal(1f);
            if (pageCanvasGroup != null)
            {
                pageCanvasGroup.blocksRaycasts = true;
                pageCanvasGroup.interactable = true;
            }
            if (screenTransitionRoot != null)
                screenTransitionRoot.gameObject.SetActive(false);

            ApplyChildApproachDisplayPower(1f);
            ForceScreenRender();
        }

        private static bool ShouldSkipChildApproachCinematic()
        {
#if ENABLE_INPUT_SYSTEM
            if (Keyboard.current != null &&
                (Keyboard.current.escapeKey.wasPressedThisFrame ||
                 Keyboard.current.enterKey.wasPressedThisFrame ||
                 Keyboard.current.spaceKey.wasPressedThisFrame))
            {
                return true;
            }

            if (Gamepad.current != null &&
                (Gamepad.current.buttonSouth.wasPressedThisFrame ||
                 Gamepad.current.startButton.wasPressedThisFrame))
            {
                return true;
            }

            return false;
#else
            return Input.GetKeyDown(KeyCode.Escape) ||
                   Input.GetKeyDown(KeyCode.Return) ||
                   Input.GetKeyDown(KeyCode.Space) ||
                   Input.GetKeyDown(KeyCode.JoystickButton0) ||
                   Input.GetKeyDown(KeyCode.JoystickButton7);
#endif
        }
    }
}
