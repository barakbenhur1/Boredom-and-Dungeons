#if UNITY_EDITOR
using System.IO;

namespace BoredomAndDungeons.EditorTools.Validation
{
    internal static class BDFullGameAirborneAttackAnimationV1011373QA
    {
        private const string AirbornePath =
            "Assets/_Project/Scripts/Runtime/Combat/" +
            "BDPlayerAirborneAttackAnimation.cs";

        private const string MeleePath =
            "Assets/_Project/Scripts/Runtime/Combat/" +
            "BDPlayerMeleeEnhancer.cs";

        private const string CombatPath =
            "Assets/_Project/Scripts/Runtime/BDPlayerCombat.cs";

        public static void Scan(BDOneClickQAResult result)
        {
            string root = Directory.GetCurrentDirectory();

            Require(
                result,
                root,
                AirbornePath,
                "FULL_GAME_AIRBORNE_BODY_OVERRIDE_V1011373_MISSING",
                "BD FULL GAME AIRBORNE BODY OVERRIDE V10.11.30.73",
                "[DefaultExecutionOrder(12000)]",
                "private void LateUpdate()",
                "Application.onBeforeRender += ReapplyBeforeRender;",
                "Application.onBeforeRender -= ReapplyBeforeRender;",
                "ApplyPose(activeHeavy, progress);",
                "Quaternion.Euler(-Mathf.Abs(windupPitch), 0f, 0f)",
                "float pitch = Mathf.Abs(strikePitch);",
                "Quaternion strikeRotation =",
                "Quaternion.Euler(pitch, 0f, 0f);",
                "visualRoot.localScale = Vector3.Lerp(",
                "public bool IsPlaying => active;"
            );

            Forbid(
                result,
                root,
                AirbornePath,
                "FULL_GAME_AIRBORNE_COROUTINE_OVERRIDE_REGRESSION_V1011373",
                "StartCoroutine(Animate(heavy))",
                "private IEnumerator Animate(bool heavy)"
            );

            Require(
                result,
                root,
                MeleePath,
                "FULL_GAME_AIRBORNE_COMMIT_OWNER_V1011373_MISSING",
                "BD EXPLICIT AIRBORNE ATTACK PRESENTATION BRANCH V23R19",
                "airborneAnimation?.Play(heavy);",
                "lastCommittedPresentationWasAirborne"
            );

            Require(
                result,
                root,
                CombatPath,
                "FULL_GAME_AIRBORNE_VERTICAL_SLASH_OWNER_V1011373_MISSING",
                "SpawnCommittedMeleeSlashArc(",
                "if (airbornePresentation)",
                "BDMeleeSlashArcVisual.SpawnVertical(",
                "SpawnMeleeSlashArc(aim, heavySwing);"
            );

            Forbid(
                result,
                root,
                AirbornePath,
                "FULL_GAME_AIRBORNE_TUTORIAL_COUPLING_V1011373",
                "FirstLaunchTutorial",
                "FirstLaunchTutorialStep"
            );
        }

        private static void Require(
            BDOneClickQAResult result,
            string root,
            string relative,
            string code,
            params string[] tokens)
        {
            string full = Path.Combine(root, relative);
            if (!File.Exists(full))
            {
                Add(result, code, relative, "Required file missing.");
                return;
            }

            string source = File.ReadAllText(full);
            foreach (string token in tokens)
            {
                if (!source.Contains(token))
                {
                    Add(
                        result,
                        code,
                        relative,
                        "Missing required token: " + token
                    );
                }
            }
        }

        private static void Forbid(
            BDOneClickQAResult result,
            string root,
            string relative,
            string code,
            params string[] tokens)
        {
            string full = Path.Combine(root, relative);
            if (!File.Exists(full))
                return;

            string source = File.ReadAllText(full);
            foreach (string token in tokens)
            {
                if (source.Contains(token))
                {
                    Add(
                        result,
                        code,
                        relative,
                        "Forbidden token remains: " + token
                    );
                }
            }
        }

        private static void Add(
            BDOneClickQAResult result,
            string code,
            string path,
            string message)
        {
            result.findings.Add(new BDOneClickQAFinding(
                BDOneClickQASeverity.Blocker,
                code,
                path,
                string.Empty,
                message
            ));
        }
    }
}
#endif
