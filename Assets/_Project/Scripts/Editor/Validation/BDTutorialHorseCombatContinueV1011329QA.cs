#if UNITY_EDITOR
using System.IO;

namespace BoredomAndDungeons.EditorTools.Validation
{
    internal static class BDTutorialHorseCombatContinueV1011329QA
    {
        private const string PagePath =
            "Assets/_Project/Scripts/Runtime/UI/" +
            "BDModernHandheld3DPresenter.FirstLaunchTutorial.cs";
        private const string GameplayPath =
            "Assets/_Project/Scripts/Runtime/UI/" +
            "BDModernHandheld3DPresenter.FirstLaunchTutorial.Gameplay.cs";
        private const string ScreensPath =
            "Assets/_Project/Scripts/Runtime/UI/" +
            "BDModernHandheld3DPresenter.FirstLaunchTutorial.LessonScreens.cs";
        private const string RepairPath =
            "Assets/_Project/Scripts/Runtime/UI/" +
            "BDModernHandheld3DPresenter.FirstLaunchTutorial.V108Repair.cs";

        public static void Scan(BDOneClickQAResult result)
        {
            string root = Directory.GetCurrentDirectory();

            Require(result, root, ScreensPath,
                "TUTORIAL_V1011329_HORSE_SHOOTER_PERSISTENCE_MISSING",
                "BD HORSE SHOOTER SURVIVES STORY BEAT V10.11.30.29",
                "current == FirstLaunchTutorialStep.HorseShot &&\n                next == FirstLaunchTutorialStep.JumpAttack");
            Require(result, root, GameplayPath,
                "TUTORIAL_V1011329_HORSE_SHOOTER_KILL_OWNER_MISSING",
                "BD HORSE SHOOTER KILL OWNER V10.11.30.29",
                "TutorialEnemyRole.Ranged,\n                        firstLaunchTutorialEnemyWorldPosition.x,\n                        1f,\n                        true");
            Require(result, root, GameplayPath,
                "TUTORIAL_V1011329_HORSE_SHOOTER_EVENT_REBIND_MISSING",
                "SetTutorialEntityActive(firstLaunchTutorialProjectile, false);",
                "SetFirstLaunchTutorialActorPassiveForScreen(\n                firstLaunchTutorialEnemy\n            );",
                "FirstLaunchTutorialStep.JumpAttack");

            Require(result, root, ScreensPath,
                "TUTORIAL_V1011329_POST_SCROLL_HORSE_RETURN_MISSING",
                "TutorialHorseReturnPostHandoffDelaySeconds = 0.14f",
                "firstLaunchTutorialHandoffDefersHorseUntilSettled",
                "targetStep == FirstLaunchTutorialStep.HorseReturn",
                "firstLaunchTutorialHorse.gameObject.SetActive(false)");
            Require(result, root, GameplayPath,
                "TUTORIAL_V1011329_HORSE_RETURN_DELAY_MISSING",
                "BD POST-SCROLL HORSE RETURN V10.11.30.29",
                "elapsed < TutorialHorseReturnPostHandoffDelaySeconds",
                "elapsed - TutorialHorseReturnPostHandoffDelaySeconds",
                "firstLaunchTutorialHorse.gameObject.SetActive(true)");

            Require(result, root, RepairPath,
                "TUTORIAL_V1011329_MOUNTED_SHOT_DAMAGE_SOURCE_MISSING",
                "BD MOUNTED SHOT DAMAGE SOURCE V10.11.30.29",
                "firstLaunchTutorialPendingShotCharged\n                        ? FirstLaunchTutorialDamageSourceV101125.Charged\n                        : FirstLaunchTutorialDamageSourceV101125.Ranged",
                "TryApplyFirstLaunchTutorialLessonDamageV101125(",
                "hitLivingTarget && damageApplied && target.Dead");
            Forbid(result, root, RepairPath,
                "TUTORIAL_V1011329_DIRECT_RANGE_DAMAGE_REGRESSION",
                "ApplyFirstLaunchTutorialActorDamage(\n                    target,\n                    firstLaunchTutorialPendingShotDamage\n                );");

            Require(result, root, PagePath,
                "TUTORIAL_V1011329_CONTINUE_EFFECT_BUILD_MISSING",
                "BD PIXEL CONTINUE EFFECT V10.11.30.29",
                "BuildFirstLaunchTutorialContinueEffect(");
            Require(result, root, ScreensPath,
                "TUTORIAL_V1011329_CONTINUE_EFFECT_FLOW_MISSING",
                "Tutorial Continue Effect",
                "Tutorial Continue Accent",
                "Tutorial Continue Label",
                "Tutorial Continue Arrow",
                "ShowFirstLaunchTutorialContinueEffect();",
                "UpdateFirstLaunchTutorialContinueEffect()",
                "HideFirstLaunchTutorialContinueEffect();",
                "firstLaunchTutorialContinueEffectRoot.SetAsLastSibling()",
                "firstLaunchTutorialLessonScreenExitX");
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
                    Add(result, code, relative,
                        "Missing required contract token: " + token);
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
                    Add(result, code, relative,
                        "Forbidden regression token remains: " + token);
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
