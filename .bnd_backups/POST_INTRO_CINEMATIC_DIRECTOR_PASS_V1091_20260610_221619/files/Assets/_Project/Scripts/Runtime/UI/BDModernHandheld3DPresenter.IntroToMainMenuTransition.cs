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

        private const float IntroMainMenuEstablishSeconds = 0.46f;
        private const float IntroMainMenuCinematicSeconds = 3.05f;

        private MainMenuEntryMode currentMainMenuEntryMode;
        private bool introToMainMenuTransitionActive;
        private float introToMainMenuTransitionStartedAt;
        private Vector3 introToMainMenuStartPosition;
        private float introToMainMenuStartFieldOfView;
        private Vector3 introToMainMenuFinalPosition;
        private Quaternion introToMainMenuFinalRotation;
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

            introToMainMenuFinalPosition =
                new Vector3(0f, 0.44f, -25.2f);
            introToMainMenuFinalRotation =
                Quaternion.Euler(1.35f, 0f, 0f);
            introToMainMenuFinalFieldOfView =
                ResolveRegularMainMenuFieldOfView();

            // The table environment and handheld already fill the entire
            // viewport as one real 3D scene. The transition is a camera dolly
            // inside that scene: the table, device, shadow and screen are never
            // translated, rotated or scaled to fake a screen-space zoom.
            RestoreStaticIntroScenePose();
            introToMainMenuStartPosition =
                new Vector3(-3.85f, 2.75f, -29.15f);
            introToMainMenuStartFieldOfView = Mathf.Clamp(
                introToMainMenuFinalFieldOfView - 3.2f,
                32.8f,
                43.5f
            );

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
            if (elapsed <= IntroMainMenuEstablishSeconds)
            {
                ApplyIntroToMainMenuCameraPose(0f);
                return;
            }

            float progress = Mathf.Clamp01(
                (elapsed - IntroMainMenuEstablishSeconds) /
                IntroMainMenuCinematicSeconds
            );
            ApplyIntroToMainMenuCameraPose(progress);

            if (progress >= 1f)
                CompleteIntroToMainMenuTransition();
        }

        private void ApplyIntroToMainMenuCameraPose(float progress)
        {
            float eased = SmootherStep01(progress);

            Vector3 firstControl =
                introToMainMenuStartPosition +
                new Vector3(0.92f, -0.08f, 1.62f);
            Vector3 secondControl =
                introToMainMenuFinalPosition +
                new Vector3(-1.18f, 0.62f, -1.46f);
            Vector3 cameraPosition = EvaluateCubicBezier(
                introToMainMenuStartPosition,
                firstControl,
                secondControl,
                introToMainMenuFinalPosition,
                eased
            );

            Vector3 lookTarget = Vector3.Lerp(
                DeviceRestPosition + new Vector3(-0.72f, 0.58f, 0.16f),
                DeviceRestPosition + new Vector3(0f, 0.70f, 0.18f),
                eased
            );
            Quaternion trackedRotation = Quaternion.LookRotation(
                lookTarget - cameraPosition,
                Vector3.up
            );
            Quaternion cinematicRoll = Quaternion.Euler(
                0f,
                0f,
                Mathf.Lerp(-1.65f, 0f, eased)
            );
            float finalFrameBlend = SmootherStep01(
                Mathf.InverseLerp(0.86f, 1f, progress)
            );

            deviceCamera.transform.localPosition = cameraPosition;
            deviceCamera.transform.localRotation = Quaternion.Slerp(
                trackedRotation * cinematicRoll,
                introToMainMenuFinalRotation,
                finalFrameBlend
            );
            deviceCamera.fieldOfView = Mathf.Lerp(
                introToMainMenuStartFieldOfView,
                introToMainMenuFinalFieldOfView,
                eased
            );
        }

        private void RestoreStaticIntroScenePose()
        {
            if (deviceVisualRoot != null)
            {
                deviceVisualRoot.localPosition = DeviceRestPosition;
                deviceVisualRoot.localRotation = DeviceRestRotation;
                deviceVisualRoot.localScale = Vector3.one;
            }
            if (shadowRoot != null)
            {
                shadowRoot.localPosition = DeviceRestPosition;
                shadowRoot.localRotation = DeviceRestRotation;
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
                new Vector3(0f, 0.44f, -25.2f);
            deviceCamera.transform.localRotation =
                Quaternion.Euler(1.35f, 0f, 0f);
            deviceCamera.fieldOfView = ResolveRegularMainMenuFieldOfView();
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

        private static float SmootherStep01(float value)
        {
            float t = Mathf.Clamp01(value);
            return t * t * t * (t * (t * 6f - 15f) + 10f);
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
            return inverse * inverse * inverse * start +
                   3f * inverse * inverse * t * firstControl +
                   3f * inverse * t * t * secondControl +
                   t * t * t * end;
        }
    }
}
