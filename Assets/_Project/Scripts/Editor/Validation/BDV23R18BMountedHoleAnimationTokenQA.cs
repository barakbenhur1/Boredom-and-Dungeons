#if UNITY_EDITOR
using System;
using System.IO;

namespace BoredomAndDungeons.EditorTools.Validation
{
    internal static class BDV23R18BMountedHoleAnimationTokenQA
    {
        private const string ExactAnimationToken =
            "temporary procedural animation is not final release animation";

        public static void Scan(BDOneClickQAResult result)
        {
            if (result == null)
                return;

            string root = Directory.GetParent(
                UnityEngine.Application.dataPath
            ).FullName;

            Require(
                result,
                root,
                "ProjectGuide/Product/ART_DIRECTION.md",
                "V23R18B_ANIMATION_TOKEN_ROOT_MISSING",
                ExactAnimationToken
            );

            Require(
                result,
                root,
                "ProjectGuide/Product/ART_DIRECTION.md",
                "V23R18B_ANIMATION_TOKEN_MIRROR_MISSING",
                ExactAnimationToken
            );

            Require(
                result,
                root,
                "ProjectGuide/Features/Animation/PRODUCTION_ANIMATION_REQUIREMENTS_V1.md",
                "V23R18B_ANIMATION_TOKEN_REQUIREMENTS_MISSING",
                ExactAnimationToken
            );

            Require(
                result,
                root,
                "Assets/_Project/Scripts/Runtime/BDHorseController.cs",
                "V23R18B_HAZARD_DISMOUNT_API_MISSING",
                "public void ForceDismountAfterHazardRecovery()"
            );

            ValidateMountedHazardOrder(result, root);

            Require(
                result,
                root,
                "ProjectGuide/Features/Hazards/QUICKSAND_AND_ENEMY_HAZARD_BEHAVIOR_V1.md",
                "V23R18B_HAZARD_ORDER_DESIGN_MISSING",
                "Mounted recovery ordering refinement — V23R18B",
                "before horse damage callbacks",
                "already unmounted"
            );

            Require(
                result,
                root,
                "ProjectGuide/Status/CURRENT.md",
                "V23R18B_STATUS_MISSING",
                "C01/C04/C10/C12.RUNTIME.V23R18B",
                "dismount-before-damage",
                "UNITY VERIFICATION REQUIRED"
            );

            Require(
                result,
                root,
                "ProjectGuide/QA/QA_CHECKLIST.md",
                "V23R18B_QA_GATE_MISSING",
                "Active V23R18B mounted-hole ordering and animation-token gate",
                "before horse damage callbacks"
            );
        }

        private static void ValidateMountedHazardOrder(
            BDOneClickQAResult result,
            string root)
        {
            const string relative =
                "Assets/_Project/Scripts/Runtime/Hazards/BDHorseHazardSafety.cs";

            string source = Read(root, relative);
            if (string.IsNullOrEmpty(source))
            {
                Add(
                    result,
                    "V23R18B_MOUNTED_HAZARD_ORDER_MISSING",
                    "Missing required file: " + relative
                );
                return;
            }

            const string marker =
                "BD MOUNTED HAZARD DISMOUNT BEFORE DAMAGE AND RESPAWN V23R18B";

            int markerIndex = source.IndexOf(
                marker,
                StringComparison.Ordinal
            );

            int dismountIndex = source.IndexOf(
                "horse.ForceDismountAfterHazardRecovery();",
                Math.Max(0, markerIndex),
                StringComparison.Ordinal
            );

            int damageIndex = source.IndexOf(
                "horseHealth.ApplyDamage(",
                Math.Max(0, markerIndex),
                StringComparison.Ordinal
            );

            int horseRelocationIndex = source.IndexOf(
                "transform.SetPositionAndRotation(",
                Math.Max(0, markerIndex),
                StringComparison.Ordinal
            );

            int riderRecoveryIndex = source.IndexOf(
                "riderRecovery.TryHandleHazard(",
                Math.Max(0, markerIndex),
                StringComparison.Ordinal
            );

            bool ordered =
                markerIndex >= 0 &&
                dismountIndex > markerIndex &&
                damageIndex > dismountIndex &&
                horseRelocationIndex > damageIndex &&
                riderRecoveryIndex > horseRelocationIndex;

            if (!ordered)
            {
                Add(
                    result,
                    "V23R18B_MOUNTED_HAZARD_ORDER_MISSING",
                    "Mounted hazard recovery must force-dismount before horse damage callbacks, horse relocation, and rider hole recovery."
                );
            }
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

                Add(result, code, "Missing V23R18B contract: " + token);
            }
        }

        private static string Read(string root, string relative)
        {
            string path = Path.Combine(root, relative);
            return File.Exists(path)
                ? File.ReadAllText(path)
                : string.Empty;
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
