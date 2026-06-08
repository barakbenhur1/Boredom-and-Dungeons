#if UNITY_EDITOR
using System;
using System.IO;

namespace BoredomAndDungeons.EditorTools.Validation
{
    internal static class BDV23R19GFocusedRegressionQA
    {
        public static void Scan(BDOneClickQAResult result)
        {
            if (result == null)
                return;

            string root = Directory.GetParent(
                UnityEngine.Application.dataPath
            ).FullName;

            Require(result, root,
                "Assets/_Project/Scripts/Runtime/BDMeleeSlashArcVisual.cs",
                "V23R19G_AIRBORNE_ROTATION_MISSING",
                "BD CORRECT LOCAL-X AIRBORNE ROTATION V23R19G",
                "Quaternion.AngleAxis(-90f, Vector3.right)",
                "verticalPlane: false",
                "Vector3.one * expand"
            );

            Require(result, root,
                "Assets/_Project/Scripts/Runtime/BDCharacterDeathAnimation.cs",
                "V23R19G_RENDERER_DEATH_MISSING",
                "BD RENDERER-BRANCH PLAYER AND ENEMY DEATH V23R19G",
                "ResolveAndCaptureVisualBranches",
                "GetComponentsInChildren<Renderer>(true)",
                "IsDeathPresentationActive",
                "Time.unscaledDeltaTime"
            );

            Require(result, root,
                "Assets/_Project/Scripts/Runtime/BDHealth.cs",
                "V23R19G_SYNCHRONOUS_DEATH_MISSING",
                "BD SYNCHRONOUS LETHAL DEATH PRESENTATION V23R19G",
                "BDCharacterDeathAnimation.PlayPlayerDeath(this)",
                "BDCharacterDeathAnimation.PlayEnemyDeath(this)"
            );

            Require(result, root,
                "Assets/_Project/Scripts/Runtime/UI/BDMainMenuFlow.cs",
                "V23R19G_ABANDON_OR_DEATH_FLOW_MISSING",
                "BD REAL ABANDON-TO-MAIN-MENU RELOAD V23R19G",
                "BD MAIN MENU STATIC FLAGS RESET V23R19G",
                "showMainMenuAfterReload",
                "ReloadToMainMenuRoutine",
                "BD READABLE PLAYER DEATH BEFORE MENU V23R19G",
                "IsDeathPresentationActive"
            );

            Require(result, root,
                "Assets/_Project/Scripts/Runtime/BDHorseController.cs",
                "V23R19G_MOUNTED_BINDING_MISSING",
                "BD AUTHORITATIVE PER-FRAME INTRO RIDER BINDING V23R19G",
                "MaintainMountedRunIntroBinding",
                "PlaceRiderOnMountPoint"
            );

            Require(result, root,
                "Assets/_Project/Scripts/Runtime/RunPresentation/BDRunPresentationCoordinator.cs",
                "V23R19G_MOUNTED_RESTORE_MISSING",
                "RestoreMountedIntroControls",
                "MaintainMountedRunIntroBinding(player)",
                "playerController.enabled = false"
            );

            Require(result, root,
                "Assets/_Project/Design/Runtime/OPEN_BUG_TRACKER.md",
                "V23R19G_BUG_TRACKER_MISSING",
                "This document must be updated every time",
                "BUG-V23R19G-001",
                "BUG-V23R19H-001",
                "IMPLEMENTED / UNITY VERIFICATION REQUIRED"
            );

            Require(result, root,
                "PROJECT_STATUS.md",
                "V23R19G_STATUS_MISSING",
                "C01/C03/C04/C05/C11.RUNTIME.V23R19G",
                "V23R19G acceptance gate",
                "OPEN_BUG_TRACKER.md"
            );
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

                Add(result, code, "Missing V23R19G contract: " + token);
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
