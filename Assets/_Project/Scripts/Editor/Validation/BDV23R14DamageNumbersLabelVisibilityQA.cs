#if UNITY_EDITOR
using System;
using System.IO;

namespace BoredomAndDungeons.EditorTools.Validation
{
    internal static class BDV23R14DamageNumbersLabelVisibilityQA
    {
        public static void Scan(BDOneClickQAResult result)
        {
            if (result == null)
                return;

            string root = Directory.GetParent(
                UnityEngine.Application.dataPath
            ).FullName;

            Require(result, root,
                "Assets/_Project/Scripts/Editor/Validation/BDOneClickQAWindow.cs",
                "V23R14_QA_INTEGRATION_MISSING",
                "BDV23R14DamageNumbersLabelVisibilityQA.Scan(result)"
            );

            Require(result, root,
                "Assets/_Project/Scripts/Runtime/UI/BDDamageNumberFeedback.cs",
                "V23R14_DAMAGE_NUMBERS_MISSING",
                "BD ANIMATED PLAYER/ENEMY DAMAGE NUMBERS V23R14",
                "PlayerDamageColor",
                "EnemyDamageColor",
                "Time.unscaledDeltaTime",
                "EaseOutBack",
                "ResolveAnchorPosition"
            );

            Require(result, root,
                "Assets/_Project/Scripts/Runtime/BDHealth.cs",
                "V23R14_DAMAGE_DISPATCH_MISSING",
                "BDDamageNumberFeedback.Spawn(",
                "appliedDamage",
                "critical"
            );

            Require(result, root,
                "Assets/_Project/Scripts/Runtime/UI/BDPrototypeHazardLabelVisibility.cs",
                "V23R14_LABEL_OCCLUSION_MISSING",
                "BD OCCLUSION-SAFE PROTOTYPE HAZARD LABELS V23R14",
                "UpgradeExistingPrototypeLabels",
                "Physics.RaycastNonAlloc",
                "maxVisibleDistance",
                "BDGameplayUiVisibility.IsGameplayHudVisible"
            );

            Require(result, root,
                "Assets/_Project/Scripts/Editor/Validation/BDPrototypeHazardSceneInstaller.cs",
                "V23R14_LABEL_INSTALLER_MISSING",
                "BDPrototypeHazardLabelVisibility",
                "visibility.Configure(9.0f)"
            );

            Require(result, root,
                "ProjectGuide/Features/UI/DAMAGE_NUMBERS_AND_TEST_LABEL_VISIBILITY_V1.md",
                "V23R14_DESIGN_MISSING",
                "Damage-number implementation — DONE IN CODE",
                "Prototype-label visibility — DONE IN CODE",
                "Player damage",
                "Enemy damage"
            );

            Require(result, root,
                "ProjectGuide/Status/CURRENT.md",
                "V23R14_STATUS_MISSING",
                "C01/C03/C11.RUNTIME.V23R14",
                "Animated damage numbers — IMPLEMENTED",
                "Saved feature resume point"
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

                Add(result, code, "Missing V23R14 contract: " + token);
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
