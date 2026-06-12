#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace BoredomAndDungeons.EditorTools.Validation
{
    internal static class BDChildApproachCinematicQA
    {
        public static void Scan(BDOneClickQAResult result)
        {
            if (result == null)
                return;

            string root = Directory.GetParent(Application.dataPath).FullName;
            string transitionPath = Path.Combine(
                root,
                "Assets/_Project/Scripts/Runtime/UI/" +
                "BDModernHandheld3DPresenter.IntroToMainMenuTransition.cs"
            );
            string childPath = Path.Combine(
                root,
                "Assets/_Project/Scripts/Runtime/UI/" +
                "BDModernHandheld3DPresenter.ChildApproachCinematic.cs"
            );
            string dialoguePath = Path.Combine(
                root,
                "Assets/_Project/Scripts/Runtime/UI/" +
                "BDModernHandheld3DPresenter.ChildApproachDialogue.cs"
            );
            string environmentPath = Path.Combine(
                root,
                "Assets/_Project/Scripts/Runtime/UI/" +
                "BDModernHandheld3DPresenter.CinematicEnvironment.cs"
            );
            string wallpaperPath = Path.Combine(
                root,
                "Assets/_Project/Scripts/Runtime/UI/" +
                "BDModernHandheld3DPresenter.CinematicWallpaperBackWall.cs"
            );

            var errors = new List<string>();
            RequireFile(errors, transitionPath);
            RequireFile(errors, childPath);
            RequireFile(errors, dialoguePath);
            RequireFile(errors, environmentPath);
            RequireFile(errors, wallpaperPath);

            string runtime =
                ReadIfPresent(transitionPath) +
                ReadIfPresent(childPath) +
                ReadIfPresent(dialoguePath) +
                ReadIfPresent(environmentPath) +
                ReadIfPresent(wallpaperPath);

            RequireTokens(
                errors,
                runtime,
                "child point-of-view cinematic",
                "IntroMainMenuTotalSeconds = 10.67f",
                "ChildApproachBlackHoldEndsAtSeconds = 0.42f",
                "ChildApproachSceneFadeEndsAtSeconds = 1.20f",
                "ChildApproachWalkEndsAtSeconds = 6.72f",
                "ChildApproachDialogueHoldEndsAtSeconds = 4.18f",
                "ChildApproachDialogueExitEndsAtSeconds = 4.78f",
                "ChildApproachClimbStartsAtSeconds = 6.94f",
                "ChildApproachSeatReachSeconds = 8.24f",
                "ChildApproachCameraSettleSeconds = 9.02f",
                "ChildApproachPowerOnStartsAtSeconds = 9.20f",
                "ChildApproachPowerOnEndsAtSeconds = 10.20f",
                "ChildApproachStartCameraPosition",
                "CinematicChairCenterZ = -11.15f",
                "ChildApproachEntranceFirstControl",
                "ChildApproachEntranceSecondControl",
                "EvaluateFilmicFadeIn",
                "Cinematic Exact Fruit Wallpaper Right Wall",
                "Cinematic Right Wall Baseboard",
                "Cinematic Kitchen Ceiling",
                "BuildCinematicDiningChair",
                "Cinematic Dining Chair Seat",
                "backPanelBottomY = CinematicChairSeatSurfaceY + 0.52f",
                "float slatBottomY =",
                "float slatTopY =",
                "float slatHeight = slatTopY - slatBottomY",
                "new Vector3(-6.85f, -9.40f, -34.20f)",
                "BD INTRO DRIP UNDERLAY HANDOFF V10.11.17",
                "SetChildApproachSceneFadeImmediate(0f)",
                "RenderMode.ScreenSpaceOverlay",
                "canvas.sortingOrder = 32760",
                "roomFloorY + 4.65f",
                "EvaluateCatmullRom",
                "EvaluateCubicBezier",
                "ChildApproachSeatEdgeCameraPosition",
                "chairLeftClearanceX",
                "Mathf.InverseLerp(0.64f, 0.86f, climbRaw)",
                "new Vector3(-0.82f, -6.78f, -9.72f)",
                "CinematicChairCenterZ -",
                "ChildApproachWalkStartsAtSeconds = 3.84f",
                "BD MOTHER BUBBLE OVERLAPS FIRST CHILD STEPS V10.11.17",
                "Sweety, where are you?"
            );
            RequireTokens(
                errors,
                runtime,
                "physical screen-off and power-on contract",
                "SetChildApproachScreenOff",
                "screenCanvasRoot.SetActive(false)",
                "screenCamera.enabled = false",
                "ApplyChildApproachDisplayPower(0f)",
                "ChildApproachScreenPowerState.PoweringOn",
                "ForceChildApproachScreenOnImmediate",
                "poweredOffGlass",
                "ForceScreenRender()",
                "ApplyChildApproachContentReveal",
                "BD TRUE SCREEN POWER REVEAL V10.11.22",
                "pageCanvasGroup.transform.localPosition",
                "childApproachPowerRevealMaterialV101122.SetFloat"
            );
            RequireTokens(
                errors,
                runtime,
                "skip-to-valid-final-state contract",
                "ShouldSkipChildApproachCinematic",
                "ApplyChildApproachCameraPose(1f)",
                "ForceChildApproachScreenOnImmediate()"
            );

            ForbidTokens(
                errors,
                ReadIfPresent(childPath) + ReadIfPresent(dialoguePath),
                "white-screen startup and removed child reply",
                "transitionFlash.color = Color.white",
                "new Color(1f, 1f, 1f, 1f)",
                "screenCanvasRoot.SetActive(true); // before walk",
                "introToMainMenuStartPosePrimed",
                "new Vector3(-0.58f, -11.35f, -16.60f)",
                "new Vector3(-0.62f, -9.62f, -18.10f)",
                "new Vector3(-3.05f, -9.58f, -21.60f)",
                "new Vector3(-0.16f, -9.06f, -13.18f)",
                "new Vector3(-0.09f, -7.78f, -12.18f)",
                "Mathf.InverseLerp(0.24f, 0.92f, t)",
                "climbRaw < 0.72f",
                "IntroMainMenuTotalSeconds = 8.25f",
                "IntroMainMenuTotalSeconds = 7.45f",
                "ChildApproachSceneFadeEndsAtSeconds = 0.55f",
                "new Vector3(-3.82f, -9.58f, -23.85f)",
                "backPanelBottomY = CinematicChairSeatSurfaceY + 0.92f",
                "float slatYCenter = backPanelCenterY + 0.18f",
                "float slatHeight = backPanelHeight - 0.58f",
                "ChildApproachPowerOnStartsAtSeconds = 6.55f",
                "ChildApproachPowerOnEndsAtSeconds = 7.55f",
                "IntroMainMenuTotalSeconds = 7.80f",
                "ChildApproachChildReplyEnterStartsAtSeconds",
                "ChildApproachChildReplyExitEndsAtSeconds",
                "BDBBHBootIntro.IsDripping ? 0f : 1f",
                "Child Speech Bubble Visual",
                "רק שניה"
            );

            if (errors.Count == 0)
                return;

            result.findings.Add(
                new BDOneClickQAFinding(
                    BDOneClickQASeverity.Blocker,
                    "CHILD_APPROACH_CINEMATIC_CONTRACT_INVALID",
                    string.Empty,
                    string.Empty,
                    string.Join("\n", errors)
                )
            );
        }

        private static void RequireFile(
            List<string> errors,
            string path)
        {
            if (!File.Exists(path))
                errors.Add("Missing child-approach cinematic file: " + path);
        }

        private static void RequireTokens(
            List<string> errors,
            string source,
            string contract,
            params string[] tokens)
        {
            for (int index = 0; index < tokens.Length; index++)
            {
                if (source.IndexOf(tokens[index], StringComparison.Ordinal) < 0)
                {
                    errors.Add(
                        "Missing " + contract + " token: " + tokens[index]
                    );
                }
            }
        }

        private static void ForbidTokens(
            List<string> errors,
            string source,
            string contract,
            params string[] tokens)
        {
            for (int index = 0; index < tokens.Length; index++)
            {
                if (source.IndexOf(tokens[index], StringComparison.Ordinal) >= 0)
                {
                    errors.Add(
                        "Forbidden " + contract + " token: " + tokens[index]
                    );
                }
            }
        }

        private static string ReadIfPresent(string path)
        {
            return File.Exists(path)
                ? File.ReadAllText(path)
                : string.Empty;
        }
    }
}
#endif
