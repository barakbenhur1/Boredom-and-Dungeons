using UnityEngine;
using UnityEngine.UI;

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

        private const float ChildApproachBlackHoldEndsAtSeconds = 0.42f;
        private const float ChildApproachSceneFadeEndsAtSeconds = 1.20f;
        private const float ChildApproachDialogueEnterStartsAtSeconds = 1.55f;
        private const float ChildApproachDialogueEnterEndsAtSeconds = 1.90f;
        private const float ChildApproachDialogueHoldEndsAtSeconds = 4.18f;
        private const float ChildApproachDialogueExitEndsAtSeconds = 4.78f;
        // BD MOTHER BUBBLE OVERLAPS FIRST CHILD STEPS V10.11.17
        private const float ChildApproachWalkStartsAtSeconds = 3.84f;
        private const float ChildApproachWalkEndsAtSeconds = 6.72f;
        private const float ChildApproachClimbStartsAtSeconds = 6.94f;
        private const float ChildApproachSeatReachSeconds = 8.24f;
        private const float ChildApproachCameraSettleSeconds = 9.02f;
        private const float ChildApproachPowerOnStartsAtSeconds = 9.20f;
        private const float ChildApproachPowerOnEndsAtSeconds = 10.20f;
        private const float ChildApproachCameraHeightOffset = 0.24f;

        // Authored around the V10.9.26 chair: well behind it and clearly
        // offset to the left, while preserving the established child POV
        // height above the physical room floor.
        private static readonly Vector3 ChildApproachStartCameraPosition =
            new Vector3(-6.85f, -9.40f, -34.20f);
        private static readonly Vector3 ChildApproachEntranceFirstControl =
            new Vector3(-6.55f, -9.32f, -29.40f);
        private static readonly Vector3 ChildApproachEntranceSecondControl =
            new Vector3(-4.72f, -9.49f, -20.65f);
        private static readonly Vector3 ChildApproachWalkEndCameraPosition =
            new Vector3(-2.96f, -9.54f, -15.78f);
        private static readonly Vector3 ChildApproachClimbPrepCameraPosition =
            new Vector3(-2.72f, -9.39f, -14.18f);
        private static readonly Vector3 ChildApproachFirstLiftCameraPosition =
            new Vector3(-2.66f, -8.71f, -13.02f);
        private static readonly Vector3 ChildApproachSeatEdgeCameraPosition =
            new Vector3(-2.58f, -7.76f, -11.64f);
        private static readonly Vector3 ChildApproachSeatCameraPosition =
            new Vector3(-1.08f, -7.54f, -11.14f);
        private static readonly Vector3 ChildApproachLeanCameraPosition =
            new Vector3(-0.05f, -4.90f, -7.10f);

        private static readonly Vector3 ChildApproachStartLookTarget =
            new Vector3(-2.10f, -7.48f, -16.20f);
        private static readonly Vector3 ChildApproachEntranceFirstLookControl =
            new Vector3(-1.90f, -7.46f, -15.15f);
        private static readonly Vector3 ChildApproachEntranceSecondLookControl =
            new Vector3(-1.12f, -7.44f, -12.55f);
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
        private GameObject childApproachSceneFadeRoot;
        private CanvasGroup childApproachSceneFadeCanvasGroup;

        private void InitializeChildApproachCinematicState()
        {
            childApproachScreenPowerState =
                ChildApproachScreenPowerState.On;
            childApproachDisplayStateCaptured = false;
            childApproachContentTransformCaptured = false;
            childApproachSceneFadeRoot = null;
            childApproachSceneFadeCanvasGroup = null;
            InitializeChildApproachDialogueState();
        }

        private void ResetChildApproachCinematicState()
        {
            childApproachScreenPowerState =
                ChildApproachScreenPowerState.On;
            SetChildApproachSceneFadeImmediate(0f);
            ResetChildApproachDialogueForPlayback();
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
                    ChildApproachWalkStartsAtSeconds,
                    ChildApproachWalkEndsAtSeconds,
                    seconds
                );
                float t = SmoothestStep01(raw);
                cameraPosition = EvaluateCubicBezier(
                    ChildApproachStartCameraPosition,
                    ChildApproachEntranceFirstControl,
                    ChildApproachEntranceSecondControl,
                    ChildApproachWalkEndCameraPosition,
                    t
                );
                lookTarget = EvaluateCubicBezier(
                    ChildApproachStartLookTarget,
                    ChildApproachEntranceFirstLookControl,
                    ChildApproachEntranceSecondLookControl,
                    ChildApproachWalkLookTarget,
                    t
                );

                // Nine compact child steps begin only after the longer black hold
                // and filmic room reveal. The route starts at the kitchen
                // entrance, curves naturally toward the chair, and preserves
                // grounded child-scale movement without a floating camera.
                float stepEnvelope = Mathf.Sin(Mathf.PI * raw);
                float stepPhase = raw * Mathf.PI * 18f;
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
                float route = Mathf.Clamp01(climbRaw) * 3f;
                int segment = Mathf.Min(2, Mathf.FloorToInt(route));
                float segmentT = SmoothestStep01(route - segment);

                Vector3 cameraBeforePrep = Vector3.Lerp(
                    ChildApproachWalkEndCameraPosition,
                    ChildApproachClimbPrepCameraPosition,
                    0.35f
                );
                Vector3 cameraAfterSeat = Vector3.Lerp(
                    ChildApproachSeatCameraPosition,
                    ChildApproachLeanCameraPosition,
                    0.28f
                );
                Vector3 lookBeforePrep = Vector3.Lerp(
                    ChildApproachWalkLookTarget,
                    ChildApproachClimbLookTarget,
                    0.35f
                );
                Vector3 lookAfterSeat = Vector3.Lerp(
                    ChildApproachSeatLookTarget,
                    RegularMainMenuLookTarget,
                    0.22f
                );

                if (segment == 0)
                {
                    cameraPosition = EvaluateCatmullRom(
                        cameraBeforePrep,
                        ChildApproachClimbPrepCameraPosition,
                        ChildApproachFirstLiftCameraPosition,
                        ChildApproachSeatEdgeCameraPosition,
                        segmentT
                    );
                    lookTarget = EvaluateCatmullRom(
                        lookBeforePrep,
                        ChildApproachClimbLookTarget,
                        ChildApproachFirstLiftLookTarget,
                        ChildApproachSeatEdgeLookTarget,
                        segmentT
                    );
                }
                else if (segment == 1)
                {
                    cameraPosition = EvaluateCatmullRom(
                        ChildApproachClimbPrepCameraPosition,
                        ChildApproachFirstLiftCameraPosition,
                        ChildApproachSeatEdgeCameraPosition,
                        ChildApproachSeatCameraPosition,
                        segmentT
                    );
                    lookTarget = EvaluateCatmullRom(
                        ChildApproachClimbLookTarget,
                        ChildApproachFirstLiftLookTarget,
                        ChildApproachSeatEdgeLookTarget,
                        ChildApproachSeatLookTarget,
                        segmentT
                    );
                }
                else
                {
                    cameraPosition = EvaluateCatmullRom(
                        ChildApproachFirstLiftCameraPosition,
                        ChildApproachSeatEdgeCameraPosition,
                        ChildApproachSeatCameraPosition,
                        cameraAfterSeat,
                        segmentT
                    );
                    lookTarget = EvaluateCatmullRom(
                        ChildApproachFirstLiftLookTarget,
                        ChildApproachSeatEdgeLookTarget,
                        ChildApproachSeatLookTarget,
                        lookAfterSeat,
                        segmentT
                    );
                }

                // The camera remains outside the chair's left edge until the
                // backrest and seat edge are safely cleared. The release is
                // blended over time instead of switching at a hard threshold.
                float chairLeftClearanceX =
                    CinematicChairCenterX -
                    CinematicChairSeatWidth * 0.5f -
                    0.30f;
                float clearanceRelease = SmoothestStep01(
                    Mathf.InverseLerp(0.64f, 0.86f, climbRaw)
                );
                float clearedX = Mathf.Min(
                    cameraPosition.x,
                    chairLeftClearanceX
                );
                cameraPosition.x = Mathf.Lerp(
                    clearedX,
                    cameraPosition.x,
                    clearanceRelease
                );

                float effort = Mathf.Sin(Mathf.PI * climbRaw);
                float braceDip = Mathf.Sin(
                    Mathf.PI * Mathf.Clamp01(climbRaw / 0.30f)
                );
                cameraPosition.y += effort * 0.030f;
                cameraPosition.y -= braceDip * 0.048f;
                cameraPosition.x +=
                    Mathf.Sin(climbRaw * Mathf.PI * 2f) * 0.009f;
                rollDegrees =
                    Mathf.Sin(climbRaw * Mathf.PI * 2f) *
                    0.20f * (1f - climbRaw * 0.62f);
            }
            else if (seconds <= ChildApproachCameraSettleSeconds)
            {
                float settleRaw = Mathf.InverseLerp(
                    ChildApproachSeatReachSeconds,
                    ChildApproachCameraSettleSeconds,
                    seconds
                );
                float t = SmoothestStep01(settleRaw);

                cameraPosition = EvaluateCubicBezier(
                    ChildApproachSeatCameraPosition,
                    new Vector3(-0.82f, -6.78f, -9.72f),
                    new Vector3(-0.16f, -3.72f, -5.42f),
                    RegularMainMenuCameraPosition,
                    t
                );
                lookTarget = EvaluateCubicBezier(
                    ChildApproachSeatLookTarget,
                    new Vector3(0f, -7.08f, -4.34f),
                    new Vector3(0f, -7.15f, -4.20f),
                    RegularMainMenuLookTarget,
                    t
                );

                float balanceEnvelope =
                    1f - SmoothestStep01(
                        Mathf.InverseLerp(0.05f, 0.72f, settleRaw)
                    );
                cameraPosition.x +=
                    Mathf.Sin(settleRaw * Mathf.PI * 3f) *
                    0.012f * balanceEnvelope;
                rollDegrees =
                    Mathf.Sin(settleRaw * Mathf.PI * 2.5f) *
                    0.16f * balanceEnvelope;
                finalRotationBlend = SmoothestStep01(
                    Mathf.InverseLerp(0.48f, 1f, settleRaw)
                );
                lensBlend = SmoothestStep01(
                    Mathf.InverseLerp(0.18f, 1f, settleRaw)
                );
            }
            else
            {
                cameraPosition = RegularMainMenuCameraPosition;
                lookTarget = RegularMainMenuLookTarget;
                finalRotationBlend = 1f;
                lensBlend = 1f;
            }

            // Preserve the higher child POV at full strength throughout the
            // complete walk and chair climb. Only the final settle dissolves
            // the offset into the unchanged regular-menu camera.
            float childCameraHeightBlend = 1f - SmoothestStep01(
                Mathf.InverseLerp(
                    ChildApproachSeatReachSeconds,
                    ChildApproachCameraSettleSeconds,
                    seconds
                )
            );
            cameraPosition.y +=
                ChildApproachCameraHeightOffset * childCameraHeightBlend;

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

        private static Vector3 EvaluateCatmullRom(
            Vector3 previous,
            Vector3 start,
            Vector3 end,
            Vector3 next,
            float progress)
        {
            float t = Mathf.Clamp01(progress);
            float t2 = t * t;
            float t3 = t2 * t;
            return 0.5f * (
                2f * start +
                (-previous + end) * t +
                (2f * previous - 5f * start + 4f * end - next) * t2 +
                (-previous + 3f * start - 3f * end + next) * t3
            );
        }

        private static Vector3 EvaluateCubicBezier(
            Vector3 start,
            Vector3 firstControl,
            Vector3 secondControl,
            Vector3 end,
            float progress)
        {
            float t = Mathf.Clamp01(progress);
            float inverse = 1f - t;
            float inverse2 = inverse * inverse;
            float t2 = t * t;
            return inverse2 * inverse * start +
                   3f * inverse2 * t * firstControl +
                   3f * inverse * t2 * secondControl +
                   t2 * t * end;
        }

        private void EnsureChildApproachSceneFadeOverlay()
        {
            if (childApproachSceneFadeRoot != null &&
                childApproachSceneFadeCanvasGroup != null)
            {
                return;
            }

            childApproachSceneFadeRoot = new GameObject(
                "B&D Child Approach Scene Fade",
                typeof(RectTransform),
                typeof(Canvas),
                typeof(CanvasGroup),
                typeof(Image)
            );
            childApproachSceneFadeRoot.transform.SetParent(
                transform,
                false
            );

            Canvas canvas = childApproachSceneFadeRoot.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 32760;

            childApproachSceneFadeCanvasGroup =
                childApproachSceneFadeRoot.GetComponent<CanvasGroup>();
            childApproachSceneFadeCanvasGroup.interactable = false;
            childApproachSceneFadeCanvasGroup.blocksRaycasts = false;

            Image image = childApproachSceneFadeRoot.GetComponent<Image>();
            image.color = Color.black;
            image.raycastTarget = false;
            RectTransform rect = image.rectTransform;
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;
        }

        private void SetChildApproachSceneFadeImmediate(float opacity)
        {
            EnsureChildApproachSceneFadeOverlay();
            float alpha = Mathf.Clamp01(opacity);
            childApproachSceneFadeRoot.SetActive(alpha > 0.001f);
            childApproachSceneFadeCanvasGroup.alpha = alpha;
        }

        private void UpdateChildApproachSceneFade(float elapsedSeconds)
        {
            EnsureChildApproachSceneFadeOverlay();
            if (elapsedSeconds <= ChildApproachBlackHoldEndsAtSeconds)
            {
                SetChildApproachSceneFadeImmediate(1f);
                return;
            }

            float fadeProgress = Mathf.InverseLerp(
                ChildApproachBlackHoldEndsAtSeconds,
                ChildApproachSceneFadeEndsAtSeconds,
                elapsedSeconds
            );
            float reveal = EvaluateFilmicFadeIn(fadeProgress);
            SetChildApproachSceneFadeImmediate(1f - reveal);
        }

        private static float EvaluateFilmicFadeIn(float progress)
        {
            float t = Mathf.Clamp01(progress);
            float smooth = t * t * (3f - 2f * t);
            return Mathf.Pow(smooth, 1.12f);
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
        // BD TRUE SCREEN POWER REVEAL V10.11.22
        private Image childApproachPowerRevealOverlayV101122;
        private Material childApproachPowerRevealMaterialV101122;

        private void EnsureChildApproachPowerRevealV101122()
        {
            if (childApproachPowerRevealOverlayV101122 != null ||
                screenCanvasRoot == null)
            {
                return;
            }

            GameObject overlayObject = new GameObject(
                "Handheld True Power Reveal V10.11.22",
                typeof(RectTransform),
                typeof(CanvasRenderer),
                typeof(Image)
            );
            overlayObject.transform.SetParent(
                screenCanvasRoot.transform,
                worldPositionStays: false
            );

            RectTransform rect =
                overlayObject.GetComponent<RectTransform>();
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;
            rect.localScale = Vector3.one;

            childApproachPowerRevealOverlayV101122 =
                overlayObject.GetComponent<Image>();
            childApproachPowerRevealOverlayV101122.raycastTarget = false;
            childApproachPowerRevealOverlayV101122.color = Color.white;

            Shader shader = Shader.Find(
                "Hidden/BoredomAndDungeons/HandheldScreenPowerReveal"
            );
            if (shader == null)
            {
                childApproachPowerRevealOverlayV101122.color = Color.black;
                return;
            }

            childApproachPowerRevealMaterialV101122 = new Material(shader)
            {
                name = "BD Handheld True Screen Reveal V10.11.22"
            };
            generatedMaterials.Add(
                childApproachPowerRevealMaterialV101122
            );
            childApproachPowerRevealMaterialV101122.SetFloat(
                "_EdgeWidth",
                0.016f
            );
            childApproachPowerRevealMaterialV101122.SetFloat(
                "_GlowStrength",
                0.92f
            );
            childApproachPowerRevealOverlayV101122.material =
                childApproachPowerRevealMaterialV101122;
        }

        private void ApplyChildApproachContentReveal(float t)
        {
            float progress = SmoothestStep01(Mathf.Clamp01(t));

            if (pageCanvasGroup != null)
            {
                if (!childApproachContentTransformCaptured)
                {
                    childApproachContentTransformCaptured = true;
                    childApproachContentRestLocalPosition =
                        pageCanvasGroup.transform.localPosition;
                    childApproachContentRestLocalScale =
                        pageCanvasGroup.transform.localScale;
                }

                // Content is fully rendered behind a real moving mask. It is not
                // exposed early and then decorated with a scanline afterward.
                pageCanvasGroup.alpha = 1f;
                pageCanvasGroup.transform.localPosition =
                    childApproachContentRestLocalPosition;
                pageCanvasGroup.transform.localScale =
                    childApproachContentRestLocalScale;
            }

            EnsureChildApproachPowerRevealV101122();
            if (childApproachPowerRevealOverlayV101122 != null)
            {
                bool revealing = progress < 0.999f;
                childApproachPowerRevealOverlayV101122.gameObject.SetActive(
                    revealing
                );
                if (revealing)
                {
                    childApproachPowerRevealOverlayV101122.transform
                        .SetAsLastSibling();
                }
            }

            if (childApproachPowerRevealMaterialV101122 != null)
            {
                childApproachPowerRevealMaterialV101122.SetFloat(
                    "_Progress",
                    progress
                );
            }

            // The old scanline was decorative and appeared over already-visible
            // content. The reveal shader owns the moving frontier instead.
            if (screenScanlineRoot != null)
            {
                screenScanlineRoot.gameObject.SetActive(progress >= 0.999f);
            }
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
            return Keyboard.current != null &&
                   Keyboard.current.escapeKey.wasPressedThisFrame;
#else
            return Input.GetKeyDown(KeyCode.Escape);
#endif
        }
    }
}
