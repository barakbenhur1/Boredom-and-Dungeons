#if UNITY_EDITOR
using System;
using System.IO;

namespace BoredomAndDungeons.EditorTools.Validation
{
    internal static class BDV23R19ICombatantForcedMovementCompatibilityQA
    {
        public static void Scan(BDOneClickQAResult result)
        {
            if (result == null)
                return;

            string root = Directory.GetParent(
                UnityEngine.Application.dataPath
            ).FullName;

            Require(result, root,
                "Assets/_Project/Scripts/Runtime/BDCombatantProfile.cs",
                "V23R19I_FORCED_MOVEMENT_API_MISSING",
                "BD FORCED MOVEMENT COMPATIBILITY API V23R19I",
                "public bool ReceivesForcedMovement",
                "public static bool CanReceiveForcedMovement(BDHealth health)",
                "return profile == null || profile.ReceivesForcedMovement;",
                "return CanReceiveForcedMovement(health);"
            );

            Require(result, root,
                "ProjectGuide/Status/CURRENT.md",
                "V23R19I_COMPILE_HISTORY_MISSING",
                "V23R19I forced-movement API compile compatibility",
                "Unity compiled after V23R19I"
            );

            Require(result, root,
                "ProjectGuide/Features/Movement/ROPE_CLIMBING_AND_QUICKSAND_SWAMP_HE_V1.md",
                "V23R19I_TRAVERSAL_REQUIREMENTS_MISSING",
                "REQUIRED / LATER / NOT IMPLEMENTED",
                "חבלים מתנדנדים",
                "משטחי טיפוס",
                "ביצה טובענית"
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
                Add(result, code, "Missing V23R19I contract: " + token);
            }
        }

        private static void Add(
            BDOneClickQAResult result,
            string code,
            string message)
        {
            result.findings.Add(new BDOneClickQAFinding(
                BDOneClickQASeverity.Blocker,
                code,
                string.Empty,
                string.Empty,
                message
            ));
        }
    }
}
#endif
