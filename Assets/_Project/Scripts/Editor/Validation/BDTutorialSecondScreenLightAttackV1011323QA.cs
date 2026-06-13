#if UNITY_EDITOR
using System.IO;

namespace BoredomAndDungeons.EditorTools.Validation
{
    internal static class BDTutorialSecondScreenLightAttackV1011323QA
    {
        private const string GameplayPath =
            "Assets/_Project/Scripts/Runtime/UI/" +
            "BDModernHandheld3DPresenter.FirstLaunchTutorial.Gameplay.cs";
        private const string LessonScreensPath =
            "Assets/_Project/Scripts/Runtime/UI/" +
            "BDModernHandheld3DPresenter.FirstLaunchTutorial.LessonScreens.cs";
        private const string RepairPath =
            "Assets/_Project/Scripts/Runtime/UI/" +
            "BDModernHandheld3DPresenter.FirstLaunchTutorial.V108Repair.cs";
        private const string ProductionCoursePath =
            "Assets/_Project/Scripts/Runtime/UI/" +
            "BDModernHandheld3DPresenter.FirstLaunchTutorial.ProductionCourse.cs";

        public static void Scan(BDOneClickQAResult result)
        {
            string root = Directory.GetCurrentDirectory();

            Require(result, root, GameplayPath,
                "TUTORIAL_V1011323_CURRENT_ATTACK_ENTRY_MISSING",
                "FirstLaunchTutorialStep.AttackEnemy",
                "HandleFirstLaunchTutorialLightAttack",
                "PrepareFirstLaunchTutorialPrimaryMeleeTarget(1f)");
            Require(result, root, ProductionCoursePath,
                "TUTORIAL_V1011327_JUMP_TO_ATTACK_ROOM_MISSING",
                "BD HORSE-FREE OPENING HANDOFF V10.11.30.27",
                "SetFirstLaunchTutorialStep(FirstLaunchTutorialStep.AttackEnemy)");

            Require(result, root, LessonScreensPath,
                "TUTORIAL_V1011323_SCREEN_TWO_LAYOUT_MISSING",
                "case FirstLaunchTutorialStep.AttackEnemy:",
                "BD EXACT-CENTER VISIBLE ONE-HIT TARGET V10.11.30.26",
                "firstLaunchTutorialMounted = false;",
                "firstLaunchTutorialHorse.gameObject.SetActive(false)",
                "TutorialEnemyRole.Small",
                "PrepareFirstLaunchTutorialPrimaryEnemyVisualV1011326",
                "SetFirstLaunchTutorialActorPassiveForScreen",
                "actor.NextActionAt = float.PositiveInfinity");

            Require(result, root, RepairPath,
                "TUTORIAL_V1011324_LIGHT_IMPACT_SOURCE_MISSING",
                "BD SCREEN TWO IMPACT DAMAGE SOURCE V10.11.30.24",
                "FirstLaunchTutorialDamageSourceV101125.Light",
                "TryApplyFirstLaunchTutorialLessonDamageV101125(",
                "firstLaunchTutorialPendingMeleeDamage",
                "if (target.Dead)\n                    HandleFirstLaunchTutorialMeleeLessonDeathAtImpact(target)");

            Require(result, root, LessonScreensPath,
                "TUTORIAL_V1011325_SHARED_EDGE_HANDOFF_MISSING",
                "firstLaunchTutorialLessonScreenExitX = Mathf.Clamp",
                "BeginFirstLaunchTutorialContinuousRoomHandoff()",
                "UpdateFirstLaunchTutorialContinuousRoomHandoff()",
                "firstLaunchTutorialContinuousHandoffActive",
                "SetFirstLaunchTutorialLessonInstructionVisible(false)",
                "SetFirstLaunchTutorialLessonInstructionVisible(true)");

            Forbid(result, root, GameplayPath,
                "TUTORIAL_V1011327_HORSE_BEFORE_QUICK_ATTACK_REGRESSION",
                "BD SECOND SCREEN DIRECT LIGHT-ATTACK ENTRY V10.11.30.23");
            Forbid(result, root, LessonScreensPath,
                "TUTORIAL_V1011326_HORSESHOT_ATTACK_SAME_ROOM_REGRESSION",
                "current == FirstLaunchTutorialStep.HorseShot &&\n                next == FirstLaunchTutorialStep.AttackEnemy");

            Forbid(result, root, RepairPath,
                "TUTORIAL_V1011324_SOURCELESS_LIGHT_DAMAGE_REMAINS",
                "ApplyFirstLaunchTutorialActorDamage(\n                    target,\n                    firstLaunchTutorialPendingMeleeDamage\n                );");
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
                    Add(result, code, relative,
                        "Missing required contract token: " + token);
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
            {
                Add(result, code, relative, "Required file missing.");
                return;
            }

            string source = File.ReadAllText(full);
            foreach (string token in tokens)
            {
                if (source.Contains(token))
                {
                    Add(result, code, relative,
                        "Forbidden regression token remains: " + token);
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
