#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace BoredomAndDungeons.EditorTools.Validation
{
    internal static class BDFirstLaunchTutorialQA
    {
        private static readonly string[] RequiredFiles =
        {
            "Assets/_Project/Scripts/Runtime/UI/BDFirstLaunchTutorialStateStore.cs",
            "Assets/_Project/Scripts/Runtime/UI/BDModernHandheld3DPresenter.FirstLaunchTutorial.cs",
            "Assets/_Project/Scripts/Runtime/UI/" +
            "BDModernHandheld3DPresenter.FirstLaunchTutorial." +
            "PixelPresentation.cs",
            "Assets/_Project/Scripts/Editor/Validation/BDFirstLaunchTutorialEditorTools.cs",
            "ProjectGuide/Features/UI/FIRST_LAUNCH_TUTORIAL_V1.md",
            "ProjectGuide/Tasks/ACTIVE/FIRST_LAUNCH_TUTORIAL_AND_HANDHELD_PRODUCTION_REPAIR.md"
        };

        private static readonly string[] RequiredRuntimeTokens =
        {
            "BDFirstLaunchTutorialStatus",
            "MarkInProgress",
            "MarkCompleted",
            "MarkSkipped",
            "PlayerPrefs.Save()",
            "EffectivePage.FirstLaunchTutorial",
            "BuildFirstLaunchTutorialPage",
            "UpdateFirstLaunchTutorialNavigationInput",
            "HandleFirstLaunchTutorialControl",
            "TutorialExitInputGuardSeconds",
            "CONTINUE TUTORIAL",
            "LEAVE TUTORIAL",
            "The tutorial will not appear automatically again.",
            "SetTutorialHighlighted",
            "TutorialSpinHoldSeconds",
            "TutorialHealHoldSeconds",
            "TutorialGrappleHoldSeconds",
            "Tutorial Instruction Panel",
            "ResolveFirstLaunchTutorialKeyboardBinding",
            "ResolveFirstLaunchTutorialHandheldBinding",
            "Tutorial Keyboard Binding Card",
            "Tutorial Handheld Binding Card",
            "UpdateFirstLaunchTutorialBindingPresentation",
            "SnapFirstLaunchTutorialPixelValue",
            "Remembered-console palette",
            "KEYBOARD / MOUSE",
            "HANDHELD",
            "FilterMode.Point",
            "ApplyFirstLaunchTutorialPixelSprite",
            "BuildFirstLaunchTutorialPixelBackdrop",
            "BeginFirstLaunchTutorialInstructionPresentation"
        };

        public static void Scan(BDOneClickQAResult result)
        {
            if (result == null)
                return;

            string root = Directory.GetParent(Application.dataPath).FullName;
            List<string> errors = new List<string>();

            for (int index = 0; index < RequiredFiles.Length; index++)
            {
                string path = Path.Combine(root, RequiredFiles[index]);
                if (!File.Exists(path))
                    errors.Add("Missing first-launch tutorial file: " + RequiredFiles[index]);
            }

            string statePath = Path.Combine(
                root,
                "Assets/_Project/Scripts/Runtime/UI/BDFirstLaunchTutorialStateStore.cs"
            );
            string presenterPath = Path.Combine(
                root,
                "Assets/_Project/Scripts/Runtime/UI/BDModernHandheld3DPresenter.cs"
            );
            string tutorialPath = Path.Combine(
                root,
                "Assets/_Project/Scripts/Runtime/UI/" +
                "BDModernHandheld3DPresenter.FirstLaunchTutorial.cs"
            );
            string pixelPresentationPath = Path.Combine(
                root,
                "Assets/_Project/Scripts/Runtime/UI/" +
                "BDModernHandheld3DPresenter.FirstLaunchTutorial.PixelPresentation.cs"
            );
            string targetPath = Path.Combine(
                root,
                "Assets/_Project/Scripts/Runtime/UI/BDModernHandheldControlTarget.cs"
            );

            string runtime = ReadIfPresent(statePath) +
                             ReadIfPresent(presenterPath) +
                             ReadIfPresent(tutorialPath) +
                             ReadIfPresent(pixelPresentationPath) +
                             ReadIfPresent(targetPath);

            for (int index = 0; index < RequiredRuntimeTokens.Length; index++)
            {
                string token = RequiredRuntimeTokens[index];
                if (runtime.IndexOf(token, StringComparison.Ordinal) < 0)
                    errors.Add("Missing first-launch tutorial contract token: " + token);
            }

            RequireRuntimeContract(
                runtime,
                errors,
                "Grapple handheld binding",
                "ResolveFirstLaunchTutorialHandheldBinding",
                "case \"GRAPPLE\":",
                "return \"HOLD Y\";"
            );

            string[] forbiddenRuntimeTokens =
            {
                "BDModernHandheldV6Polish",
                "BDModernHandheldTactileCompatibility",
                "BDModernHandheldPressScaleFeedback",
                "Time.timeScale = 0",
                "HandleModernPrimaryAction(); // tutorial",
                "AddComponent<BDMainMenuFlow>",
                "case \"GRAPPLE\":\n" +
                "                    return \"Press the physical " +
                "SELECT button.\"",
                "V6Polish",
                "ResolveFirstLaunchTutorialDualBinding",
                "firstLaunchTutorialPrompt.resizeTextMaxSize = 38",
                "firstLaunchTutorialDetail.resizeTextMaxSize = 22"
            };

            for (int index = 0; index < forbiddenRuntimeTokens.Length; index++)
            {
                string token = forbiddenRuntimeTokens[index];
                if (runtime.IndexOf(token, StringComparison.Ordinal) >= 0)
                    errors.Add("Forbidden tutorial/handheld implementation token: " + token);
            }

            string[] retiredFiles =
            {
                "Assets/_Project/Scripts/Runtime/UI/BDModernHandheldV6Polish.cs",
                "Assets/_Project/Scripts/Runtime/UI/BDModernHandheldTactileCompatibility.cs",
                "Assets/_Project/Scripts/Runtime/UI/BDModernHandheldPressScaleFeedback.cs",
                "Assets/_Project/Scripts/Editor/Validation/BDModernHandheldV6QA.cs"
            };

            for (int index = 0; index < retiredFiles.Length; index++)
            {
                if (File.Exists(Path.Combine(root, retiredFiles[index])))
                    errors.Add("Retired compatibility layer still exists: " + retiredFiles[index]);
            }

            if (errors.Count == 0)
                return;

            result.findings.Add(
                new BDOneClickQAFinding(
                    BDOneClickQASeverity.Blocker,
                    "FIRST_LAUNCH_TUTORIAL_CONTRACT_INVALID",
                    string.Empty,
                    string.Empty,
                    string.Join("\n", errors)
                )
            );
        }

        private static void RequireRuntimeContract(
            string runtime,
            List<string> errors,
            string contractName,
            params string[] requiredTokens)
        {
            for (int index = 0; index < requiredTokens.Length; index++)
            {
                string token = requiredTokens[index];
                if (runtime.IndexOf(token, StringComparison.Ordinal) >= 0)
                    continue;

                errors.Add(
                    "Missing first-launch tutorial " +
                    contractName +
                    " token: " +
                    token
                );
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
