#if UNITY_EDITOR
using System;
using System.IO;

namespace BoredomAndDungeons.EditorTools.Validation
{
    internal static class BDV23R19MAirborneAxisSmallEnemyDeathQA
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
                "V23R19M_AIRBORNE_LONG_AXIS_NOT_VERTICAL",
                "BD LOCAL-Z AIRBORNE LINE ROTATION V23R19M",
                "Quaternion.AngleAxis(90f, Vector3.forward)"
            );

            Forbid(result, root,
                "Assets/_Project/Scripts/Runtime/BDMeleeSlashArcVisual.cs",
                "V23R19M_OBSOLETE_LOCAL_X_ROTATION_REMAINS",
                "Quaternion.AngleAxis(-90f, Vector3.right)"
            );

            Require(result, root,
                "Assets/_Project/Scripts/Runtime/BDCharacterDeathAnimation.cs",
                "V23R19M_SMALL_ENEMY_DEATH_OWNER_MISSING",
                "BD SMALL REGULAR ENEMY INTACT FALL V23R19M",
                "IsSmallRegularEnemy",
                "activeSmallRegularEnemy",
                "AnimateSmallRegularEnemyPose"
            );

            Require(result, root,
                "ProjectGuide/Features/Combat/AIRBORNE_VERTICAL_ATTACK_PRESENTATION_V1.md",
                "V23R19M_AIRBORNE_DESIGN_AXIS_MISSING",
                "local Z axis",
                "left-to-right",
                "top-to-bottom"
            );

            Require(result, root,
                "ProjectGuide/Features/Animation/DEATH_PRESENTATION_V1.md",
                "V23R19M_SMALL_ENEMY_DEATH_DESIGN_MISSING",
                "Small regular enemies",
                "intact-body recoil",
                "pancake"
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

                Add(result, code, "Missing V23R19M contract: " + token);
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

                Add(result, code, "Obsolete V23R19M token remains: " + token);
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
