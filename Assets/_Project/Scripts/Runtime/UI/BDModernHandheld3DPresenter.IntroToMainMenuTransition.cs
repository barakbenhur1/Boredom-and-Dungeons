using UnityEngine;

namespace BoredomAndDungeons
{
    public sealed partial class BDModernHandheld3DPresenter
    {
        private enum MainMenuEntryMode
        {
            RegularMainMenuEntry,
            IntroToMainMenuTransition
        }

        private const float IntroMainMenuTotalSeconds = 4.40f;
        private const float IntroMainMenuEstablishSeconds = 0.55f;
        private const float IntroMainMenuDescentEndsAtSeconds = 2.10f;
        private const float IntroMainMenuAlignmentEndsAtSeconds = 3.35f;
        private const float IntroMainMenuCameraNearClip = 0.05f;
        private const float IntroMainMenuCameraFarClip = 120f;

        private static readonly Vector3 RegularMainMenuCameraPosition =
            new Vector3(0f, 0.44f, -25.2f);
        private static readonly Quaternion RegularMainMenuCameraRotation =
            Quaternion.Euler(1.35f, 0f, 0f);
        private static readonly float[] IntroMainMenuSplineTimes =
        {
            0f,
            IntroMainMenuEstablishSeconds / IntroMainMenuTotalSeconds,
            IntroMainMenuDescentEndsAtSeconds / IntroMainMenuTotalSeconds,
            IntroMainMenuAlignmentEndsAtSeconds / IntroMainMenuTotalSeconds,
            1f
        };

        private readonly Vector3[] introToMainMenuCameraSpline =
            new Vector3[5];
        private readonly Vector3[] introToMainMenuCameraSecondDerivatives =
            new Vector3[5];
        private readonly Vector3[] introToMainMenuLookSpline =
            new Vector3[5];
        private readonly Vector3[] introToMainMenuLookSecondDerivatives =
            new Vector3[5];
        private readonly float[] introToMainMenuSplineLower =
            new float[5];
        private readonly float[] introToMainMenuSplineDiagonal =
            new float[5];
        private readonly float[] introToMainMenuSplineUpper =
            new float[5];
        private readonly Vector3[] introToMainMenuSplineRightHandSide =
            new Vector3[5];

        private MainMenuEntryMode currentMainMenuEntryMode;
        private bool introToMainMenuTransitionActive;
        private float introToMainMenuTransitionStartedAt;
        private float introToMainMenuStartFieldOfView;
        private float introToMainMenuFinalFieldOfView;

        private void InitializeIntroToMainMenuTransition()
        {
            currentMainMenuEntryMode = MainMenuEntryMode.RegularMainMenuEntry;
            introToMainMenuTransitionActive = false;
            introToMainMenuTransitionStartedAt = 0f;
        }

        private void DisposeIntroToMainMenuTransition()
        {
            RestoreRegularMainMenuCameraPose();
            introToMainMenuTransitionActive = false;
            currentMainMenuEntryMode = MainMenuEntryMode.RegularMainMenuEntry;
        }

        private void ResetIntroToMainMenuTransitionForScene()
        {
            RestoreRegularMainMenuCameraPose();
            introToMainMenuTransitionActive = false;
            currentMainMenuEntryMode = MainMenuEntryMode.RegularMainMenuEntry;
            introToMainMenuTransitionStartedAt = 0f;
        }

        private bool IsIntroToMainMenuTransitionBlockingInput()
        {
            return introToMainMenuTransitionActive &&
                   currentMainMenuEntryMode ==
                       MainMenuEntryMode.IntroToMainMenuTransition;
        }

        private void TickIntroToMainMenuTransition()
        {
            if (deviceCamera == null || !visible)
                return;

            if (!introToMainMenuTransitionActive)
            {
                if (BDBBHBootIntro.IsPlaying ||
                    !IsEligiblePostIntroLandingPage(displayedPage) ||
                    !BDBBHBootIntro.TryConsumeIntroToMainMenuTransition())
                {
                    return;
                }

                BeginIntroToMainMenuTransition();
            }

            UpdateIntroToMainMenuTransition();
        }

        private static bool IsEligiblePostIntroLandingPage(
            EffectivePage page)
        {
            return page == EffectivePage.MainMenu ||
                   page == EffectivePage.FirstLaunchTutorial;
        }

        private void BeginIntroToMainMenuTransition()
        {
            currentMainMenuEntryMode =
                MainMenuEntryMode.IntroToMainMenuTransition;
            introToMainMenuTransitionActive = true;
            introToMainMenuTransitionStartedAt = Time.unscaledTime;
            introToMainMenuFinalFieldOfView =
                ResolveRegularMainMenuFieldOfView();
            introToMainMenuStartFieldOfView = Mathf.Clamp(
                introToMainMenuFinalFieldOfView + 3.4f,
                39.8f,
                52.4f
            );

            RestoreStaticIntroScenePose();
            PrepareIntroToMainMenuSplines();

            entryProgress = 1f;
            ApplyIntroToMainMenuCameraPose(0f);
            menuInputUnlockAt = float.PositiveInfinity;
            menuInputNeedsRelease = true;
            ForceScreenRender();
        }

        private void UpdateIntroToMainMenuTransition()
        {
            if (!introToMainMenuTransitionActive || deviceCamera == null)
                return;

            if (!IsEligiblePostIntroLandingPage(displayedPage))
            {
                CompleteIntroToMainMenuTransition();
                return;
            }

            float elapsed =
                Time.unscaledTime - introToMainMenuTransitionStartedAt;
            float progress = Mathf.Clamp01(
                elapsed / IntroMainMenuTotalSeconds
            );
            ApplyIntroToMainMenuCameraPose(progress);

            if (progress >= 1f)
                CompleteIntroToMainMenuTransition();
        }

        private void PrepareIntroToMainMenuSplines()
        {
            introToMainMenuCameraSpline[0] =
                new Vector3(-14.8f, 10.8f, -47.5f);
            introToMainMenuCameraSpline[1] =
                new Vector3(-13.6f, 10.1f, -45.6f);
            introToMainMenuCameraSpline[2] =
                new Vector3(-5.2f, 5.4f, -34.8f);
            introToMainMenuCameraSpline[3] =
                new Vector3(-0.75f, 1.7f, -28.3f);
            introToMainMenuCameraSpline[4] =
                RegularMainMenuCameraPosition;

            Vector3 finalLookTarget =
                RegularMainMenuCameraPosition +
                RegularMainMenuCameraRotation * Vector3.forward * 25f;
            introToMainMenuLookSpline[0] =
                new Vector3(0f, -4.30f, 2.40f);
            introToMainMenuLookSpline[1] =
                new Vector3(-0.20f, -3.70f, 2.20f);
            introToMainMenuLookSpline[2] =
                new Vector3(-0.15f, -1.70f, 1.20f);
            introToMainMenuLookSpline[3] =
                new Vector3(0f, -0.40f, 0.10f);
            introToMainMenuLookSpline[4] = finalLookTarget;

            PrepareNaturalCubicSpline(
                introToMainMenuCameraSpline,
                introToMainMenuCameraSecondDerivatives
            );
            PrepareNaturalCubicSpline(
                introToMainMenuLookSpline,
                introToMainMenuLookSecondDerivatives
            );
        }

        private void ApplyIntroToMainMenuCameraPose(float progress)
        {
            float normalized = Mathf.Clamp01(progress);

            // Each axis has its own jerk-limited clock. Horizontal alignment
            // completes first, while descent and forward travel continue into
            // the long final settle. This avoids a UI-like single SmoothStep.
            float horizontalClock = SmoothestStep01(
                Mathf.InverseLerp(0f, 0.76f, normalized)
            );
            float verticalClock = SmoothestStep01(normalized);
            float forwardClock = SmoothestStep01(normalized);
            float lookClock = SmoothestStep01(
                Mathf.InverseLerp(0.02f, 1f, normalized)
            );
            float lensClock = SmoothestStep01(
                Mathf.InverseLerp(0.28f, 1f, normalized)
            );

            Vector3 cameraPosition = new Vector3(
                EvaluateNaturalCubicSplineComponent(
                    introToMainMenuCameraSpline,
                    introToMainMenuCameraSecondDerivatives,
                    horizontalClock,
                    0
                ),
                EvaluateNaturalCubicSplineComponent(
                    introToMainMenuCameraSpline,
                    introToMainMenuCameraSecondDerivatives,
                    verticalClock,
                    1
                ),
                EvaluateNaturalCubicSplineComponent(
                    introToMainMenuCameraSpline,
                    introToMainMenuCameraSecondDerivatives,
                    forwardClock,
                    2
                )
            );
            Vector3 lookTarget = EvaluateNaturalCubicSpline(
                introToMainMenuLookSpline,
                introToMainMenuLookSecondDerivatives,
                lookClock
            );
            Quaternion cameraRotation = Quaternion.LookRotation(
                lookTarget - cameraPosition,
                Vector3.up
            );

            deviceCamera.transform.localPosition = cameraPosition;
            deviceCamera.transform.localRotation = cameraRotation;
            deviceCamera.fieldOfView = Mathf.Lerp(
                introToMainMenuStartFieldOfView,
                introToMainMenuFinalFieldOfView,
                lensClock
            );
            deviceCamera.nearClipPlane = IntroMainMenuCameraNearClip;
            deviceCamera.farClipPlane = IntroMainMenuCameraFarClip;
        }

        private void RestoreStaticIntroScenePose()
        {
            if (deviceVisualRoot != null)
            {
                deviceVisualRoot.localPosition = DeviceRestPosition;
                deviceVisualRoot.localRotation = DeviceRestRotation;
                deviceVisualRoot.localScale = Vector3.one;
            }

            if (tableRoot != null)
            {
                tableRoot.localPosition = TableRestPosition;
                tableRoot.localRotation = Quaternion.identity;
                tableRoot.localScale = Vector3.one;
            }

            if (shadowRoot != null)
            {
                shadowRoot.localPosition = Vector3.zero;
                shadowRoot.localRotation = Quaternion.identity;
                shadowRoot.localScale = Vector3.one;
            }
        }

        private void CompleteIntroToMainMenuTransition()
        {
            RestoreRegularMainMenuCameraPose();
            introToMainMenuTransitionActive = false;
            currentMainMenuEntryMode =
                MainMenuEntryMode.RegularMainMenuEntry;
            menuInputUnlockAt = Time.unscaledTime + 0.14f;
            menuInputNeedsRelease = true;
        }

        private void RestoreRegularMainMenuCameraPose()
        {
            RestoreStaticIntroScenePose();
            if (deviceCamera == null)
                return;

            deviceCamera.transform.localPosition =
                RegularMainMenuCameraPosition;
            deviceCamera.transform.localRotation =
                RegularMainMenuCameraRotation;
            deviceCamera.fieldOfView = ResolveRegularMainMenuFieldOfView();
            deviceCamera.nearClipPlane = IntroMainMenuCameraNearClip;
            deviceCamera.farClipPlane = IntroMainMenuCameraFarClip;
        }

        private static Vector3 ResolveRegularMainMenuCameraPosition()
        {
            return RegularMainMenuCameraPosition;
        }

        private static Quaternion ResolveRegularMainMenuCameraRotation()
        {
            return RegularMainMenuCameraRotation;
        }

        private static float ResolveRegularMainMenuFieldOfView()
        {
            float aspect = Mathf.Max(
                0.75f,
                Screen.width / Mathf.Max(1f, Screen.height)
            );
            float fit = Mathf.InverseLerp(0.78f, 1.55f, aspect);
            return Mathf.Lerp(49f, 36.4f, fit);
        }

        private static float SmoothestStep01(float value)
        {
            float t = Mathf.Clamp01(value);
            return t * t * t * t *
                   (35f - 84f * t + 70f * t * t - 20f * t * t * t);
        }

        private void PrepareNaturalCubicSpline(
            Vector3[] points,
            Vector3[] secondDerivatives)
        {
            int count = points.Length;
            for (int index = 0; index < count; index++)
            {
                introToMainMenuSplineLower[index] = 0f;
                introToMainMenuSplineDiagonal[index] = 1f;
                introToMainMenuSplineUpper[index] = 0f;
                introToMainMenuSplineRightHandSide[index] = Vector3.zero;
                secondDerivatives[index] = Vector3.zero;
            }

            for (int index = 1; index < count - 1; index++)
            {
                float previousInterval =
                    IntroMainMenuSplineTimes[index] -
                    IntroMainMenuSplineTimes[index - 1];
                float nextInterval =
                    IntroMainMenuSplineTimes[index + 1] -
                    IntroMainMenuSplineTimes[index];

                introToMainMenuSplineLower[index] = previousInterval;
                introToMainMenuSplineDiagonal[index] =
                    2f * (previousInterval + nextInterval);
                introToMainMenuSplineUpper[index] = nextInterval;
                introToMainMenuSplineRightHandSide[index] = 6f *
                    ((points[index + 1] - points[index]) / nextInterval -
                     (points[index] - points[index - 1]) /
                     previousInterval);
            }

            for (int index = 2; index < count - 1; index++)
            {
                float factor =
                    introToMainMenuSplineLower[index] /
                    introToMainMenuSplineDiagonal[index - 1];
                introToMainMenuSplineDiagonal[index] -=
                    factor * introToMainMenuSplineUpper[index - 1];
                introToMainMenuSplineRightHandSide[index] -=
                    factor * introToMainMenuSplineRightHandSide[index - 1];
            }

            secondDerivatives[count - 2] =
                introToMainMenuSplineRightHandSide[count - 2] /
                introToMainMenuSplineDiagonal[count - 2];
            for (int index = count - 3; index >= 1; index--)
            {
                secondDerivatives[index] =
                    (introToMainMenuSplineRightHandSide[index] -
                     introToMainMenuSplineUpper[index] *
                     secondDerivatives[index + 1]) /
                    introToMainMenuSplineDiagonal[index];
            }
        }

        private static Vector3 EvaluateNaturalCubicSpline(
            Vector3[] points,
            Vector3[] secondDerivatives,
            float progress)
        {
            return new Vector3(
                EvaluateNaturalCubicSplineComponent(
                    points,
                    secondDerivatives,
                    progress,
                    0
                ),
                EvaluateNaturalCubicSplineComponent(
                    points,
                    secondDerivatives,
                    progress,
                    1
                ),
                EvaluateNaturalCubicSplineComponent(
                    points,
                    secondDerivatives,
                    progress,
                    2
                )
            );
        }

        private static float EvaluateNaturalCubicSplineComponent(
            Vector3[] points,
            Vector3[] secondDerivatives,
            float progress,
            int component)
        {
            float t = Mathf.Clamp01(progress);
            int segment = 0;
            while (segment < IntroMainMenuSplineTimes.Length - 2 &&
                   t > IntroMainMenuSplineTimes[segment + 1])
            {
                segment++;
            }

            float startTime = IntroMainMenuSplineTimes[segment];
            float endTime = IntroMainMenuSplineTimes[segment + 1];
            float interval = endTime - startTime;
            float startWeight = (endTime - t) / interval;
            float endWeight = (t - startTime) / interval;

            float startValue = ResolveVectorComponent(
                points[segment],
                component
            );
            float endValue = ResolveVectorComponent(
                points[segment + 1],
                component
            );
            float startSecond = ResolveVectorComponent(
                secondDerivatives[segment],
                component
            );
            float endSecond = ResolveVectorComponent(
                secondDerivatives[segment + 1],
                component
            );

            return startWeight * startValue +
                   endWeight * endValue +
                   ((startWeight * startWeight * startWeight - startWeight) *
                        startSecond +
                    (endWeight * endWeight * endWeight - endWeight) *
                        endSecond) *
                   interval * interval / 6f;
        }

        private static float ResolveVectorComponent(
            Vector3 value,
            int component)
        {
            if (component == 0)
                return value.x;
            if (component == 1)
                return value.y;
            return value.z;
        }

#if UNITY_EDITOR || DEVELOPMENT_BUILD
        [ContextMenu("Replay Post-Intro Camera Cinematic")]
        private void ReplayPostIntroCinematicForDevelopment()
        {
            if (!presentationReady || deviceCamera == null || !visible)
                return;

            BeginIntroToMainMenuTransition();
        }
#endif
    }
}
