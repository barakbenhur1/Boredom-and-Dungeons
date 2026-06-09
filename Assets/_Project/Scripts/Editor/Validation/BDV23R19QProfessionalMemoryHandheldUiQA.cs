#if UNITY_EDITOR
using System;
using System.IO;

namespace BoredomAndDungeons.EditorTools.Validation
{
    internal static class BDV23R19QProfessionalMemoryHandheldUiQA
    {
        public static void Scan(BDOneClickQAResult result)
        {
            if (result == null)
                return;

            string root = Directory.GetParent(
                UnityEngine.Application.dataPath
            ).FullName;

            Require(result, root,
                "Assets/_Project/Scripts/Runtime/UI/BDBBHBootIntro.cs",
                "V23R19Q_BOOT_POLISH_MISSING",
                "BD PROFESSIONAL BBH BOOT SURFACE V23R19Q",
                "CreateBootScanlineTexture",
                "CreateBootVignetteTexture",
                "DrawProfessionalBootSurface",
                "DrawComposition(globalAlpha)"
            );

            Require(result, root,
                "Assets/_Project/Scripts/Runtime/UI/BDGameBoyMenuShell.cs",
                "V23R19Q_HANDHELD_SHELL_MISSING",
                "BD PROFESSIONAL MEMORY-HANDHELD SHELL V23R19Q",
                "CreateRoundedRectTexture",
                "DrawScreenOverlay",
                "DrawStartSelectButtons",
                "DrawScreenHousing",
                "currentScreenMode"
            );

            Require(result, root,
                "Assets/_Project/Scripts/Runtime/UI/BDMainMenuFlow.cs",
                "V23R19Q_MENU_SCREEN_MISSING",
                "BD PROFESSIONAL HANDHELD SCREEN MENU V23R19Q",
                "ResolveScreenLabel",
                "ResolveModeTransition",
                "BDGameBoyMenuShell.DrawScreenOverlay",
                "DrawMemoryProgressBar",
                "sliderThumbStyle",
                "\"START GAME\"",
                "\"SETTINGS\"",
                "\"GAME PAUSED\"",
                "\"ABANDON THIS RUN?\"",
                "\"STARTING...\""
            );

            Require(result, root,
                "Assets/_Project/Scripts/Runtime/UI/BDDreamyMainMenuBackdrop.cs",
                "V23R19Q_BACKDROP_FOCUS_MISSING",
                "BD PROFESSIONAL DEVICE FOCUS HALO V23R19Q",
                "DrawDeviceFocusHalo"
            );

            Require(result, root,
                "ProjectGuide/Features/UI/GAME_BOY_MENU_AND_UI_OWNERSHIP_V1.md",
                "V23R19Q_UI_CONTRACT_MISSING",
                "memory rather than replica",
                "Professional finish",
                "cached",
                "all menu pages"
            );

            Require(result, root,
                "ProjectGuide/Tasks/ACTIVE/MODERN_HANDHELD_MAIN_PAUSE_UI.md",
                "V23R19Q_TASK_RECORD_MISSING",
                "Why this task exists",
                "Protected content and behavior",
                "Performance contract",
                "Exact resume point"
            );

            Require(result, root,
                "ProjectGuide/Status/CURRENT.md",
                "V23R19Q_STATUS_MISSING",
                "C11.UI.V23R19Q",
                "V23R19Q acceptance gate"
            );

            Forbid(result, root,
                "Assets/_Project/Scripts/Editor/Validation/BDV23R19TraversalQuicksandAirborneQA.cs",
                "V23R19Q_WALL_JUMP_QA_STILL_BRITTLE",
                "\"wallJumpHorizontalSpeed = 10.4f\""
            );

            Forbid(result, root,
                "Assets/_Project/Scripts/Editor/Validation/BDV23R19PQaSemanticCaterpillarSpecQA.cs",
                "V23R19Q_CATERPILLAR_QA_STILL_BRITTLE",
                "\"passive-refill threshold is **not** an absolute wallet maximum\""
            );
        }

        private static string Read(string root, string relative)
        {
            string path = Path.Combine(root, relative);
            return File.Exists(path)
                ? File.ReadAllText(path)
                : string.Empty;
        }

        private static void Require(
            BDOneClickQAResult result,
            string root,
            string relative,
            string code,
            params string[] tokens)
        {
            string source = Read(root, relative);
            if (string.IsNullOrEmpty(source))
            {
                Add(result, code, "Missing required file: " + relative);
                return;
            }

            foreach (string token in tokens)
            {
                if (source.IndexOf(token, StringComparison.Ordinal) >= 0)
                    continue;

                Add(result, code, "Missing V23R19Q contract: " + token);
            }
        }

        private static void Forbid(
            BDOneClickQAResult result,
            string root,
            string relative,
            string code,
            params string[] tokens)
        {
            string source = Read(root, relative);
            foreach (string token in tokens)
            {
                if (source.IndexOf(token, StringComparison.Ordinal) < 0)
                    continue;

                Add(result, code, "Obsolete V23R19Q token remains: " + token);
            }
        }

        private static void Add(
            BDOneClickQAResult result,
            string code,
            string message)
        {
            result.findings.Add(
                new BDOneClickQAFinding(
                    BDOneClickQASeverity.Blocker,
                    code,
                    string.Empty,
                    string.Empty,
                    message
                )
            );
        }
    }
}
#endif
