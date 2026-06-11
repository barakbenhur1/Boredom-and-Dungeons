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
            string environmentPath = Path.Combine(
                root,
                "Assets/_Project/Scripts/Runtime/UI/" +
                "BDModernHandheld3DPresenter.CinematicEnvironment.cs"
            );

            var errors = new List<string>();
            RequireFile(errors, transitionPath);
            RequireFile(errors, childPath);
            RequireFile(errors, environmentPath);

            string runtime =
                ReadIfPresent(transitionPath) +
                ReadIfPresent(childPath) +
                ReadIfPresent(environmentPath);

            RequireTokens(
                errors,
                runtime,
                "child point-of-view cinematic",
                "IntroMainMenuTotalSeconds = 7.80f",
                "ChildApproachBlackHoldEndsAtSeconds = 0.42f",
                "ChildApproachSceneFadeEndsAtSeconds = 1.20f",
                "ChildApproachWalkEndsAtSeconds = 4.10f",
                "ChildApproachClimbStartsAtSeconds = 4.32f",
                "ChildApproachSeatReachSeconds = 5.62f",
                "ChildApproachCameraSettleSeconds = 6.40f",
                "ChildApproachPowerOnStartsAtSeconds = 6.55f",
                "ChildApproachPowerOnEndsAtSeconds = 7.55f",
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
                "new Vector3(-6.85f, -9.40f, -34.20f)",
                "SetChildApproachSceneFadeImmediate(1f)",
                "UpdateChildApproachSceneFade(elapsed)",
                "RenderMode.ScreenSpaceOverlay",
                "canvas.sortingOrder = 32760",
                "roomFloorY + 4.65f",
                "EvaluateCatmullRom",
                "EvaluateCubicBezier",
                "ChildApproachSeatEdgeCameraPosition",
                "chairLeftClearanceX",
                "Mathf.InverseLerp(0.64f, 0.86f, climbRaw)",
                "new Vector3(-0.82f, -6.78f, -9.72f)",
                "CinematicChairCenterZ -"
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
                "Mathf.InverseLerp(0.56f, 0.97f, t)",
                "pageCanvasGroup.transform.localPosition",
                "screenScanlineRoot.gameObject.SetActive(t >= 0.54f)"
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
                ReadIfPresent(childPath),
                "white-screen startup",
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
                "new Vector3(-3.82f, -9.58f, -23.85f)"
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
