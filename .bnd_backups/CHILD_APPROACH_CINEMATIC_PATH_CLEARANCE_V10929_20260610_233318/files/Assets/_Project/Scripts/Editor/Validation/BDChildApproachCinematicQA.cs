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
                "IntroMainMenuTotalSeconds = 8.25f",
                "ChildApproachWalkEndsAtSeconds = 3.30f",
                "ChildApproachClimbStartsAtSeconds = 3.70f",
                "ChildApproachSeatReachSeconds = 5.55f",
                "ChildApproachCameraSettleSeconds = 6.55f",
                "ChildApproachPowerOnStartsAtSeconds = 6.85f",
                "ChildApproachPowerOnEndsAtSeconds = 7.85f",
                "ChildApproachStartCameraPosition",
                "CinematicChairCenterZ = -11.15f",
                "BuildCinematicDiningChair",
                "Cinematic Dining Chair Seat",
                "new Vector3(-0.62f, -9.62f, -18.10f)",
                "roomFloorY + 4.65f",
                "EvaluateQuadraticBezier",
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
                "Mathf.InverseLerp(0.24f, 0.92f, t)"
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
