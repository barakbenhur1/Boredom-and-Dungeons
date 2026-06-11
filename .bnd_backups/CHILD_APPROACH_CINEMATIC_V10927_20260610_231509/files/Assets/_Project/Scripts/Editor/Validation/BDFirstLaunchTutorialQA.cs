#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
namespace BoredomAndDungeons.EditorTools.Validation
{
    internal static class BDFirstLaunchTutorialQA
    {
        private static readonly string[] RequiredFiles =
        {
            "Assets/_Project/Scripts/Runtime/UI/" +
            "BDModernHandheld3DPresenter.IntroToMainMenuTransition.cs",
            "Assets/_Project/Scripts/Runtime/UI/" +
            "BDModernHandheld3DPresenter.CinematicEnvironment.cs",
            "Assets/_Project/Scripts/Runtime/UI/BDModernHandheld3DPresenter.CinematicWallpaperBackWall.cs",
            "Assets/_Project/Scripts/Editor/Validation/BDFirstLaunchTutorialEditorTools.cs",
        };
        private static readonly string[] RequiredRuntimeTokens =
        {
            "SmoothestStep01",
            "EvaluateNaturalCubicSplineComponent",
            "IntroMainMenuTotalSeconds = 4.40f",
            "IntroMainMenuEstablishSeconds = 0.55f",
            "IntroMainMenuDescentEndsAtSeconds = 2.10f",
            "IntroMainMenuAlignmentEndsAtSeconds = 3.35f",
            "BuildCinematicProductEnvironment",
            "Full 3D Tabletop",
            "BuildExactCinematicBackWallWallpaper",
            "Cinematic Exact Fruit Wallpaper Back Wall",
            "Cinematic Wallpaper Baseboard",
            "Cinematic Warm Room Fill",
            "Cinematic Wallpaper Wall Wash",
            "BuildCinematicRoomFloor",
            "Cinematic Real Room Floor",
            "BD Cinematic Warm Room Floor",
            "RestoreStaticIntroScenePose",
            "PrimeIntroToMainMenuFirstFrame",
            "introToMainMenuStartPosePrimed",
            "new Vector3(0f, -1.94f, -4.18f)",
            "new Vector3(0f, -7.18f, -4.18f)",
            "new Vector3(-0.14f, -0.22f, -6.48f)",
            "Mathf.Lerp(37.8f, 30.6f, fit)",
            "DeviceRealWorldScale = 0.16f",
            "DeviceRestScale",
            "deviceCamera.fieldOfView =",
            "ResolveRegularMainMenuFieldOfView();",
            "new Vector3(0f, -7.27f, -3.60f)",
            "Quaternion.Euler(90f, 0f, 0f)",
            "new Vector2(1.62f, 2.55f)",
        };
        internal static void Scan()
        {
            string root = "";
            string launchGatePath = "";
            string introMainMenuPath = "";
            string cinematicEnvironmentPath = Path.Combine(
                root,
                "Assets/_Project/Scripts/Runtime/UI/" +
                "BDModernHandheld3DPresenter.CinematicEnvironment.cs"
            );
            string cinematicWallpaperPath = Path.Combine(
                root,
                "Assets/_Project/Scripts/Runtime/UI/" +
                "BDModernHandheld3DPresenter.CinematicWallpaperBackWall.cs"
            );
            string bootIntroPath = Path.Combine(
                root,
                "Assets/_Project/Scripts/Runtime/UI/BDBBHBootIntro.cs"
            );
            string introMainMenu = ReadIfPresent(introMainMenuPath);
            string runtime = ReadIfPresent("state") +
                             ReadIfPresent(launchGatePath) +
                             introMainMenu +
                             ReadIfPresent(cinematicEnvironmentPath) +
                             ReadIfPresent(cinematicWallpaperPath) +
                             ReadIfPresent(bootIntroPath);
            var errors = new List<string>();
            RequireRuntimeContract(
                runtime,
                errors,
                "camera-only full-set intro-to-main-menu cinematic",
                "MainMenuEntryMode.IntroToMainMenuTransition",
                "TryConsumeIntroToMainMenuTransition",
                "IsEligiblePostIntroLandingPage",
                "page == EffectivePage.MainMenu",
                "page == EffectivePage.FirstLaunchTutorial",
                "IntroMainMenuTotalSeconds = 4.40f",
                "IntroMainMenuEstablishSeconds = 0.55f",
                "IntroMainMenuDescentEndsAtSeconds = 2.10f",
                "IntroMainMenuAlignmentEndsAtSeconds = 3.35f",
                "PrepareNaturalCubicSpline",
                "EvaluateNaturalCubicSplineComponent",
                "SmoothestStep01",
                "BuildCinematicProductEnvironment",
                "Full 3D Tabletop",
                "Table Front Apron",
                "Table Front Left Leg",
                "BuildCinematicRoomFloor",
                "Cinematic Real Room Floor",
                "BD Cinematic Warm Room Floor",
                "BuildExactCinematicBackWallWallpaper",
                "Cinematic Exact Fruit Wallpaper Back Wall",
                "Cinematic Wallpaper Baseboard",
                "Cinematic Warm Room Fill",
                "Cinematic Wallpaper Wall Wash",
                "deviceCamera.transform.localPosition = cameraPosition",
                "tableRoot.localRotation = Quaternion.identity",
                "menuInputUnlockAt = float.PositiveInfinity",
                "PrimeIntroToMainMenuFirstFrame",
                "RegularMainMenuLookTarget",
                "new Vector3(0f, -7.18f, -4.18f)",
                "new Vector3(0f, -1.94f, -4.18f)",
                "Mathf.Lerp(37.8f, 30.6f, fit)",
                "deviceVisualRoot.localScale = DeviceRestScale"
            );
            string[] forbiddenIntroTransitionTokens =
            {
                "ApplyIntroToMainMenuThreeDimensionalPose",
                "EvaluateCubicBezier",
                "SmootherStep01",
                "IntroMainMenuCinematicSeconds",
                "new Vector3(0f, 0.44f, -25.2f)",
                "new Vector3(0f, 1.50f, -14.0f)",
                "Mathf.Lerp(17f, 15f, fit)",
                "deviceVisualRoot.localScale = Vector3.Lerp",
                "BuildCinematicFloorAndCyclorama",
                "Cinematic Cyclorama",
                "BD Cinematic Charcoal Floor",
                "BD Cinematic Charcoal Cyclorama",
            };
            string[] forbiddenRuntimeTokens =
            {
                "entryProgress",
                "new Vector3(0f, -7.27f, 0f)",
                "Professional Blurred Wood Table",
                "Table Cinematic Vignette",
                "Tutorial Visible Lesson Gate",
                "TutorialEnemyRole.Small, 1370f, 2f",
                "new Vector3(0f, 0.28f, 0f)",
                "Quaternion.Euler(9.4f, 0f, 0f)",
                "new Vector2(10.9f, 3.85f)",
                "new Vector2(8.55f, 1.42f)"
            };
        }
        private static void RequireRuntimeContract(string r, List<string> e, string n, params string[] t) { }
        private static string ReadIfPresent(string p) { return ""; }
    }
}
#endif
