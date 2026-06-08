#if UNITY_EDITOR
using System;
using System.IO;

namespace BoredomAndDungeons.EditorTools.Validation
{
    internal static class BDV23R18AHorseHazardAnimationQA
    {
        public static void Scan(BDOneClickQAResult result)
        {
            if (result == null)
                return;

            string root =
                Directory.GetParent(
                    UnityEngine.Application.dataPath
                ).FullName;

            Require(
                result,
                root,
                "Assets/_Project/Scripts/Runtime/Hazards/BDHorseHazardSafety.cs",
                "V23R18A_HORSE_HAZARD_OWNER_MISSING",
                "BD HORSE/RIDER HOLE AND LAVA OWNERSHIP V23R18A",
                "horseHealth.ApplyDamage",
                "BeginMountedLavaRecoveryWithoutDamage",
                "BDHazardType.HoleOrChasm",
                "BDHazardType.Lava"
            );

            Require(
                result,
                root,
                "Assets/_Project/Scripts/Runtime/Hazards/BDPlayerHazardRecovery.cs",
                "V23R18A_MOUNTED_LAVA_RECOVERY_MISSING",
                "BD MOUNTED LAVA ZERO-DAMAGE RECOVERY ARC V23R18A",
                "BeginMountedLavaRecoveryWithoutDamage",
                "lavaBounceAppliedDamage = 0f",
                "ResolveReducedLavaBounceTarget"
            );

            Require(
                result,
                root,
                "ART_DIRECTION.md",
                "V23R18A_PRODUCTION_ANIMATION_ROOT_MISSING",
                "Production animation completeness",
                "Every gameplay action that benefits from visible motion",
                "temporary procedural animation is not final release animation"
            );

            Require(
                result,
                root,
                "Assets/_Project/Design/Animation/PRODUCTION_ANIMATION_REQUIREMENTS_V1.md",
                "V23R18A_PRODUCTION_ANIMATION_DESIGN_MISSING",
                "Player animation coverage",
                "Horse animation coverage",
                "Enemy and boss animation coverage",
                "Production acceptance gate"
            );

            Require(
                result,
                root,
                "PROJECT_STATUS.md",
                "V23R18A_STATUS_MISSING",
                "C01/C04/C10/C12.RUNTIME.V23R18A",
                "Saved feature resume point"
            );
        }

        private static void Require(
            BDOneClickQAResult result,
            string root,
            string relative,
            string code,
            params string[] tokens)
        {
            string path =
                Path.Combine(
                    root,
                    relative
                );

            string source =
                File.Exists(path)
                    ? File.ReadAllText(path)
                    : string.Empty;

            if (string.IsNullOrEmpty(source))
            {
                Add(
                    result,
                    code,
                    "Missing required file: " +
                    relative
                );
                return;
            }

            foreach (string token in tokens)
            {
                if (source.IndexOf(
                        token,
                        StringComparison.Ordinal) < 0)
                {
                    Add(
                        result,
                        code,
                        "Missing V23R18A contract: " +
                        token
                    );
                }
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
