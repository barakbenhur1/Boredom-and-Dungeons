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

        private const float IntroMainMenuTotalSeconds = 8.05f;
        private const float IntroMainMenuCameraNearClip = 0.05f;
        private const float IntroMainMenuCameraFarClip = 120f;

        private static readonly Vector3 RegularMainMenuCameraPosition =
            new Vector3(0f, -1.94f, -4.18f);
        private static readonly Vector3 RegularMainMenuLookTarget =
            new Vector3(0f, -7.18f, -4.18f);
        private static readonly Quaternion RegularMainMenuCameraRotation =
            Quaternion.Euler(90f, 0f, 0f);

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
            InitializeChildApproachCinematicState();
        }

        private void DisposeIntroToMainMenuTransition()
        {
            ForceChildApproachScreenOnImmediate();
            SetChildApproachSceneFadeImmediate(0f);
            RestoreRegularMainMenuCameraPose();
            introToMainMenuTransitionActive = false;
            currentMainMenuEntryMode = MainMenuEntryMode.RegularMainMenuEntry;
        }

        private void ResetIntroToMainMenuTransitionForScene()
        {
            ForceChildApproachScreenOnImmediate();
            SetChildApproachSceneFadeImmediate(0f);
            RestoreRegularMainMenuCameraPose();
            introToMainMenuTransitionActive = false;
            currentMainMenuEntryMode = MainMenuEntryMode.RegularMainMenuEntry;
            introToMainMenuTransitionStartedAt = 0f;
            ResetChildApproachCinematicState();
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
                // Keep the physical screen dark before the BBH overlay releases
                // the first visible room frame. This prevents a one-frame white
                // or already-rendered tutorial leak.
                if (BDBBHBootIntro.IsPlaying)
                {
                    PrimeIntroToMainMenuFirstFrame();
                    return;
                }

                if (!IsEligiblePostIntroLandingPage(displayedPage) ||
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

            PrimeIntroToMainMenuFirstFrame();
            menuInputUnlockAt = float.PositiveInfinity;
            menuInputNeedsRelease = true;

            // The display stays physically off. Do not force a live screen render
            // until the authored power-on phase begins after camera settlement.
            introToMainMenuTransitionStartedAt = Time.unscaledTime;
        }

        private void PrimeIntroToMainMenuFirstFrame()
        {
            introToMainMenuFinalFieldOfView =
                ResolveRegularMainMenuFieldOfView();
            introToMainMenuStartFieldOfView = Mathf.Clamp(
                introToMainMenuFinalFieldOfView + 2.35f,
                34.8f,
                42.4f
            );

            RestoreStaticIntroScenePose();
            SetChildApproachScreenOff();
            SetChildApproachSceneFadeImmediate(1f);
            ApplyChildApproachCameraPose(0f);
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

            if (ShouldSkipChildApproachCinematic())
            {
                CompleteIntroToMainMenuTransition();
                return;
            }

            float elapsed =
                Time.unscaledTime - introToMainMenuTransitionStartedAt;
            float progress = Mathf.Clamp01(
                elapsed / IntroMainMenuTotalSeconds
            );

            UpdateChildApproachSceneFade(elapsed);
            ApplyChildApproachCameraPose(progress);
            UpdateChildApproachScreenPower(progress);

            if (progress >= 1f)
                CompleteIntroToMainMenuTransition();
        }

        private void RestoreStaticIntroScenePose()
        {
            if (deviceVisualRoot != null)
            {
                deviceVisualRoot.localPosition = DeviceRestPosition;
                deviceVisualRoot.localRotation = DeviceRestRotation;
                deviceVisualRoot.localScale = DeviceRestScale;
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
            ApplyChildApproachCameraPose(1f);
            ForceChildApproachScreenOnImmediate();
            SetChildApproachSceneFadeImmediate(0f);
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
            return Mathf.Lerp(37.8f, 30.6f, fit);
        }

        private static float SmoothestStep01(float value)
        {
            float t = Mathf.Clamp01(value);
            return t * t * t * t *
                   (35f - 84f * t + 70f * t * t - 20f * t * t * t);
        }
    }
}
