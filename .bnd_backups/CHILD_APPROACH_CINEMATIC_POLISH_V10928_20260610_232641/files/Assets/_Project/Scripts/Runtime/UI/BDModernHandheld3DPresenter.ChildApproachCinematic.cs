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

        private const float ChildApproachWalkEndsAtSeconds = 3.05f;
        private const float ChildApproachClimbStartsAtSeconds = 3.38f;
        private const float ChildApproachSeatReachSeconds = 5.05f;
        private const float ChildApproachCameraSettleSeconds = 6.10f;
        private const float ChildApproachPowerOnStartsAtSeconds = 6.35f;
        private const float ChildApproachPowerOnEndsAtSeconds = 7.10f;

        // Authored around the V10.9.26 chair: behind it, slightly left, at
        // believable small-child eye height above the physical room floor.
        private static readonly Vector3 ChildApproachStartCameraPosition =
            new Vector3(-0.58f, -11.35f, -16.60f);
        private static readonly Vector3 ChildApproachWalkEndCameraPosition =
            new Vector3(-0.28f, -11.30f, -13.40f);
        private static readonly Vector3 ChildApproachClimbPrepCameraPosition =
            new Vector3(-0.18f, -11.10f, -12.85f);
        private static readonly Vector3 ChildApproachFirstLiftCameraPosition =
            new Vector3(-0.15f, -10.25f, -12.30f);
        private static readonly Vector3 ChildApproachSeatCameraPosition =
            new Vector3(-0.08f, -8.72f, -10.70f);
        private static readonly Vector3 ChildApproachLeanCameraPosition =
            new Vector3(-0.02f, -5.25f, -7.05f);

        private static readonly Vector3 ChildApproachStartLookTarget =
            new Vector3(-0.10f, -9.45f, -11.25f);
        private static readonly Vector3 ChildApproachWalkLookTarget =
            new Vector3(0f, -9.00f, -10.00f);
        private static readonly Vector3 ChildApproachClimbLookTarget =
            new Vector3(0f, -8.45f, -8.80f);
        private static readonly Vector3 ChildApproachFirstLiftLookTarget =
            new Vector3(0f, -7.65f, -6.35f);
        private static readonly Vector3 ChildApproachSeatLookTarget =
            new Vector3(0f, -7.25f, -4.45f);

        private ChildApproachScreenPowerState childApproachScreenPowerState;
        private bool childApproachDisplayStateCaptured;
        private Color childApproachDisplayOnColor = Color.white;
        private Color childApproachDisplayOnEmission = Color.black;
        private float childApproachDisplayOnBrightness = 1f;

        private void InitializeChildApproachCinematicState()
        {
            childApproachScreenPowerState =
                ChildApproachScreenPowerState.On;
            childApproachDisplayStateCaptured = false;
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

                // Six small, damped child steps. The envelope removes any snap
                // at the first and last step and keeps the motion comfortable.
                float stepEnvelope = Mathf.Sin(Mathf.PI * raw);
                float stepPhase = raw * Mathf.PI * 12f;
                cameraPosition.y +=
                    Mathf.Sin(stepPhase) * 0.065f * stepEnvelope;
                cameraPosition.x +=
                    Mathf.Sin(stepPhase * 0.5f + 0.65f) *
                    0.030f * stepEnvelope;
                rollDegrees =
                    Mathf.Sin(stepPhase * 0.5f) *
                    0.48f * stepEnvelope;
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

                if (climbRaw < 0.48f)
                {
                    float t = SmoothestStep01(climbRaw / 0.48f);
                    cameraPosition = Vector3.Lerp(
                        ChildApproachClimbPrepCameraPosition,
                        ChildApproachFirstLiftCameraPosition,
                        t
                    );
                    lookTarget = Vector3.Lerp(
                        ChildApproachClimbLookTarget,
                        ChildApproachFirstLiftLookTarget,
                        t
                    );
                }
                else
                {
                    float t = SmoothestStep01(
                        (climbRaw - 0.48f) / 0.52f
                    );
                    cameraPosition = Vector3.Lerp(
                        ChildApproachFirstLiftCameraPosition,
                        ChildApproachSeatCameraPosition,
                        t
                    );
                    lookTarget = Vector3.Lerp(
                        ChildApproachFirstLiftLookTarget,
                        ChildApproachSeatLookTarget,
                        t
                    );
                }

                float effort = Mathf.Sin(Mathf.PI * climbRaw);
                cameraPosition.y += effort * 0.085f;
                cameraPosition.z -= effort * 0.055f;
                rollDegrees =
                    Mathf.Sin(climbRaw * Mathf.PI * 2f) *
                    0.72f * (1f - climbRaw * 0.45f);
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
                    0.035f * balanceEnvelope;
                rollDegrees =
                    Mathf.Sin(settleRaw * Mathf.PI * 3f) *
                    0.55f * balanceEnvelope;
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
                roomFloorY + 2.65f,
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
            if (pageCanvasGroup != null)
            {
                pageCanvasGroup.alpha = 0f;
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
                    screenScanlineRoot.gameObject.SetActive(true);
                if (screenHitTargetRoot != null)
                    screenHitTargetRoot.gameObject.SetActive(false);
                if (pageCanvasGroup != null)
                {
                    pageCanvasGroup.alpha = 0f;
                    pageCanvasGroup.blocksRaycasts = false;
                    pageCanvasGroup.interactable = false;
                }
                if (screenTransitionRoot != null)
                    screenTransitionRoot.gameObject.SetActive(true);

                ForceScreenRender();
            }

            float electronicWake = SmoothestStep01(t);
            ApplyChildApproachDisplayPower(electronicWake);

            float contentReveal = SmoothestStep01(
                Mathf.InverseLerp(0.24f, 0.92f, t)
            );
            if (pageCanvasGroup != null)
                pageCanvasGroup.alpha = contentReveal;

            float shutterOpen = SmoothestStep01(
                Mathf.InverseLerp(0.14f, 0.84f, t)
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
            if (transitionLine != null)
            {
                float linePulse = Mathf.Sin(Mathf.PI * t);
                transitionLine.color = new Color(
                    0.16f,
                    0.68f,
                    1f,
                    linePulse * 0.82f
                );
                transitionLine.rectTransform.sizeDelta =
                    new Vector2(CanvasSize.x, Mathf.Lerp(3f, 10f, linePulse));
            }
            if (transitionFlash != null)
            {
                float blueWake = Mathf.Sin(Mathf.PI * t) * 0.055f;
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
            if (pageCanvasGroup != null)
            {
                pageCanvasGroup.alpha = 1f;
                pageCanvasGroup.blocksRaycasts = true;
                pageCanvasGroup.interactable = true;
            }
            if (screenHitTargetRoot != null)
                screenHitTargetRoot.gameObject.SetActive(true);
            if (screenTransitionRoot != null)
                screenTransitionRoot.gameObject.SetActive(false);
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
            if (pageCanvasGroup != null)
            {
                pageCanvasGroup.alpha = 1f;
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
