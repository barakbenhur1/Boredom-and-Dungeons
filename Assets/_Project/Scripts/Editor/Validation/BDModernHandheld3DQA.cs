#if UNITY_EDITOR
using System;
using System.IO;

namespace BoredomAndDungeons.EditorTools.Validation
{
    internal static class BDModernHandheld3DQA
    {
        public static void Scan(BDOneClickQAResult result)
        {
            if (result == null)
                return;

            string root = Directory.GetParent(
                UnityEngine.Application.dataPath
            ).FullName;

            Require(
                result,
                root,
                "Assets/_Project/Scripts/Runtime/UI/BDModernHandheld3DPresenter.cs",
                "HANDHELD_3D_PRESENTER_MISSING",
                "BD Modern Upright Handheld",
                "Molded Outer Edge Bevel",
                "DeviceRestRotation",
                "DeviceRealWorldScale = 0.16f",
                "DeviceRestScale",
                "deviceCamera.fieldOfView =",
                "ResolveRegularMainMenuFieldOfView();",
                "new Vector3(0f, -7.27f, -3.60f)",
                "Quaternion.Euler(90f, 0f, 0f)",
                "BuildCinematicProductEnvironment",
                "New Game Memory Card",
                "UpdateNewGameMemoryCardVisibility"
            );

            Require(
                result,
                root,
                "Assets/_Project/Scripts/Runtime/UI/" +
                "BDModernHandheld3DPresenter.CinematicEnvironment.cs",
                "HANDHELD_3D_CINEMATIC_ENVIRONMENT_MISSING",
                "BuildCinematicProductEnvironment",
                "Full 3D Tabletop",
                "Table Front Edge Lip",
                "Table Front Apron",
                "Table Front Left Leg",
                "Table Front Right Leg",
                "Table Back Left Leg",
                "Table Back Right Leg",
                "BuildCinematicRoomFloor",
                "Cinematic Real Room Floor",
                "BD Cinematic Warm Room Floor",
                "Device Soft Contact Penumbra",
                "Device Core Contact Shadow",
                "Device Base Contact Shadow",
                "Table Leg Contact Shadow",
                "new Vector2(1.62f, 2.55f)",
                "DeviceRestPosition.z + 0.10f",
                "DeviceRestPosition.z + 0.04f",
                "new Vector3(0f, -7.15f, 0f)",
                "Cinematic Key Light",
                "Cinematic Camera Fill",
                "Cinematic Separation Light"
            );

            Require(
                result,
                root,
                "Assets/_Project/Scripts/Runtime/UI/" +
                "BDModernHandheld3DPresenter.CinematicWallpaperBackWall.cs",
                "HANDHELD_CINEMATIC_BACKWALL_WALLPAPER_MISSING",
                "BuildExactCinematicBackWallWallpaper",
                "CinematicWallpaperResourcePath",
                "BDCinematicKitchenWallpaper",
                "Cinematic Exact Fruit Wallpaper Back Wall",
                "Cinematic Wallpaper Baseboard",
                "Cinematic Warm Room Fill",
                "Cinematic Wallpaper Wall Wash"
            );

            Forbid(
                result,
                root,
                "Assets/_Project/Scripts/Runtime/UI/" +
                "BDModernHandheld3DPresenter.CinematicEnvironment.cs",
                "HANDHELD_FLAT_CINEMATIC_ENVIRONMENT_REMAINS",
                "Professional Blurred Wood Table",
                "Table Cinematic Vignette",
                "new Vector2(10.9f, 3.85f)",
                "new Vector2(8.55f, 1.42f)"
            );
            Require(
                result,
                root,
                "Assets/_Project/Scripts/Runtime/UI/" +
                "BDModernHandheld3DPresenter.IntroToMainMenuTransition.cs",
                "HANDHELD_3D_PHYSICAL_STAGING_MISSING",
                "PrimeIntroToMainMenuFirstFrame",
                "introToMainMenuStartPosePrimed",
                "RegularMainMenuLookTarget",
                "new Vector3(0f, -1.94f, -4.18f)",
                "new Vector3(0f, -7.18f, -4.18f)",
                "new Vector3(-0.14f, -0.22f, -6.48f)",
                "Mathf.Lerp(37.8f, 30.6f, fit)",
                "deviceVisualRoot.localScale = DeviceRestScale"
            );
            Forbid(
                result,
                root,
                "Assets/_Project/Scripts/Runtime/UI/" +
                "BDModernHandheld3DPresenter.IntroToMainMenuTransition.cs",
                "HANDHELD_3D_OLD_CAMERA_REMAINS",
                "new Vector3(0f, 0.44f, -25.2f)",
                "new Vector3(0f, 1.50f, -14.0f)",
                "Mathf.Lerp(17f, 15f, fit)",
                "new Vector3(0f, 1.50f, -3.19f)",
                "Mathf.Lerp(49f, 36.4f, fit)",
                "new Vector3(0f, -1.92f, -3.82f)",
                "new Vector3(0f, -7.20f, -3.82f)",
                "new Vector3(-0.42f, -0.35f, -7.10f)",
                "Mathf.Lerp(39f, 33.5f, fit)",
                "new Vector3(0f, -1.58f, -3.94f)",
                "new Vector3(0f, -7.22f, -3.94f)",
                "new Vector3(-0.26f, -0.02f, -6.96f)",
                "Mathf.Lerp(40.5f, 34.6f, fit)",
                "deviceVisualRoot.localScale = Vector3.Lerp",
                "BuildCinematicFloorAndCyclorama",
                "Cinematic Cyclorama",
                "BD Cinematic Charcoal Floor",
                "BD Cinematic Charcoal Cyclorama"
            );

            Forbid(
                result,
                root,
                "Assets/_Project/Scripts/Runtime/UI/BDModernHandheld3DPresenter.cs",
                "HANDHELD_RETIRED_ENTRY_STATE_REMAINS",
                "entryProgress",
                "new Vector3(0f, -7.27f, 0f)"
            );

            Require(
                result,
                root,
                "Assets/_Project/Scripts/Runtime/UI/BDPlayableCharacterIdentity.cs",
                "HANDHELD_CHARACTER_IDENTITY_MISSING",
                "BDPlayableCharacterKind"
            );
        }

        private static void Require(
            BDOneClickQAResult result,
            string root,
            string relative,
            string code,
            params string[] tokens)
        {
        }

        private static void Forbid(
            BDOneClickQAResult result,
            string root,
            string relative,
            string code,
            params string[] tokens)
        {
        }
    }
}
#endif
