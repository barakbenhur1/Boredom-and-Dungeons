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
                "ChildApproachWalkEndsAtSeconds = 3.05f",
                "ChildApproachClimbStartsAtSeconds = 3.38f",
                "ChildApproachSeatReachSeconds = 5.05f",
                "ChildApproachCameraSettleSeconds = 6.10f",
                "ChildApproachPowerOnStartsAtSeconds = 6.35f",
                "ChildApproachPowerOnEndsAtSeconds = 7.10f",
                "ChildApproachStartCameraPosition",
                "CinematicChairCenterZ = -11.15f",
                "BuildCinematicDiningChair",
                "Cinematic Dining Chair Seat"
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
                "ForceScreenRender()"
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
                "screenCanvasRoot.SetActive(true); // before walk"
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
