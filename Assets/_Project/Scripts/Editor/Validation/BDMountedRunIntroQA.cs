#if UNITY_EDITOR
using System;
using System.IO;
using UnityEngine;

namespace BoredomAndDungeons.EditorTools.Validation
{
    internal static class BDMountedRunIntroQA
    {
        public static void Scan(BDOneClickQAResult result)
        {
            if (result == null)
                return;

            string root = ResolveProjectRoot();
            Require(
                result,
                root,
                "Assets/_Project/Scripts/Runtime/RunIntro/BDMountedRunIntro.cs",
                "MOUNTED_INTRO_SINGLE_OWNER_MISSING",
                "BD SINGLE RUN-PRESENTATION OWNER V13",
                "BDRunPresentationCoordinator.InputLocked"
            );

            string compatibility = Read(
                root,
                "Assets/_Project/Scripts/Runtime/RunIntro/BDMountedRunIntro.cs"
            );
            Forbid(
                result,
                compatibility,
                "MOUNTED_INTRO_DUPLICATE_BOOTSTRAP",
                "RuntimeInitializeLoadType.BeforeSceneLoad"
            );
            Forbid(
                result,
                compatibility,
                "MOUNTED_INTRO_DUPLICATE_PORTAL_OWNER",
                "EnsureDoorwayPortals"
            );

            Require(
                result,
                root,
                "Assets/_Project/Scripts/Runtime/RunPresentation/BDRunPresentationCoordinator.cs",
                "RUN_PRESENTATION_COORDINATOR_V13_MISSING",
                "BD AUTHORED ENTRY SINGLE-OWNER + PORTAL SELF-HEAL V14",
                "BD_Maze_Entrance_Marker",
                "BD_Maze_Exit_Marker",
                "EnsureEntranceApproachGround",
                "SetEntranceReturnBlocking(true)",
                "EnsureAuthoredPortalEffects",
                "AttachEffectOnly(",
                "EntranceEffectName",
                "ExitEffectName"
            );

            string coordinator = Read(
                root,
                "Assets/_Project/Scripts/Runtime/RunPresentation/BDRunPresentationCoordinator.cs"
            );
            if (coordinator.IndexOf(
                    "AttachExitApproachTrigger(exit)",
                    StringComparison.Ordinal) >= 0)
            {
                Add(
                    result,
                    "GENERATED_EXIT_TRIGGER_STILL_ACTIVE",
                    "The authored exit must keep its original trigger/flow; " +
                    "the generated cinematic trigger call must remain absent."
                );
            }
        }

        private static string Read(string root, string relative)
        {
            string absolute = Path.Combine(root, relative);
            return File.Exists(absolute)
                ? File.ReadAllText(absolute)
                : string.Empty;
        }

        private static void Require(
            BDOneClickQAResult result,
            string root,
            string relative,
            string code,
            params string[] tokens)
        {
            string absolute = Path.Combine(root, relative);
            if (!File.Exists(absolute))
            {
                Add(result, code, "Required run-presentation file is missing: " + relative);
                return;
            }

            string source = File.ReadAllText(absolute);
            foreach (string token in tokens)
            {
                if (source.IndexOf(token, StringComparison.Ordinal) >= 0)
                    continue;
                Add(result, code, "Missing run-presentation contract: " + token);
            }
        }

        private static void Forbid(
            BDOneClickQAResult result,
            string source,
            string code,
            string token)
        {
            if (source.IndexOf(token, StringComparison.Ordinal) < 0)
                return;
            Add(result, code, "Obsolete duplicate intro contract returned: " + token);
        }

        private static string ResolveProjectRoot()
        {
            DirectoryInfo assets = new DirectoryInfo(Application.dataPath);
            return assets.Parent != null
                ? assets.Parent.FullName
                : Application.dataPath;
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
