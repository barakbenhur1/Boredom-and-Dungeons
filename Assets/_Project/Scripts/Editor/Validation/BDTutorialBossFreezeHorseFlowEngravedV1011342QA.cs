#if UNITY_EDITOR
using System.IO;

namespace BoredomAndDungeons.EditorTools.Validation
{
    internal static class
        BDTutorialBossFreezeHorseFlowEngravedV1011342QA
    {
        private const string GameplayPath =
            "Assets/_Project/Scripts/Runtime/UI/" +
            "BDModernHandheld3DPresenter.FirstLaunchTutorial.Gameplay.cs";
        private const string LessonPath =
            "Assets/_Project/Scripts/Runtime/UI/" +
            "BDModernHandheld3DPresenter.FirstLaunchTutorial.LessonScreens.cs";
        private const string RuntimePath =
            "Assets/_Project/Scripts/Runtime/UI/" +
            "BDModernHandheld3DPresenter.FirstLaunchTutorial." +
            "BossIntroFreezeV1011342.cs";
        private const string PresenterPath =
            "Assets/_Project/Scripts/Runtime/UI/BDModernHandheld3DPresenter.cs";

        public static void Scan(BDOneClickQAResult result)
        {
            string root = Directory.GetCurrentDirectory();

            Require(result, root, GameplayPath,
                "TUTORIAL_V1011342_BOSS_INPUT_FREEZE_MISSING",
                "BD BOSS INTRO WORLD FREEZE V10.11.30.42",
                "BD BOSS INTRO KEYBOARD INPUT FREEZE V10.11.30.42",
                "BD BOSS INTRO PHYSICAL INPUT FREEZE V10.11.30.42",
                "MaintainFirstLaunchTutorialBossIntroFreezeV1011342()",
                "ReleaseFirstLaunchTutorialBossIntroFreezeV1011342()",
                "tutorialRoomCount = 21");

            Require(result, root, RuntimePath,
                "TUTORIAL_V1011342_BOSS_STATE_FREEZE_MISSING",
                "BD BOSS INTRO FREEZE OWNER V10.11.30.42",
                "boss.Health = boss.MaximumHealth",
                "boss.Dead = false",
                "boss.NextActionAt = float.PositiveInfinity",
                "firstLaunchTutorialActionPresentationType =",
                "FirstLaunchTutorialActionPresentationType.None",
                "ApplyFirstLaunchTutorialBossIntroFrozenVisualsV1011342");

            Require(result, root, LessonPath,
                "TUTORIAL_V1011342_HEAL_PET_MOUNT_ROOM_MISSING",
                "BD HEAL PET MOUNT SAME ROOM V10.11.30.42",
                "current == FirstLaunchTutorialStep.HealHorse &&",
                "next == FirstLaunchTutorialStep.PetHorse",
                "current == FirstLaunchTutorialStep.PetHorse &&",
                "next == FirstLaunchTutorialStep.RemountHorse",
                "case FirstLaunchTutorialStep.PetHorse:",
                "roomIndex = 20;");

            Require(result, root, PresenterPath,
                "HANDHELD_V1011342_ENGRAVED_SHORTCUT_LABEL_MISSING",
                "BD ENGRAVED SHORTCUT LABEL V10.11.30.42",
                "localPosition + new Vector3(0.006f, -0.010f, -0.003f)",
                "localPosition + new Vector3(-0.006f, 0.010f, -0.002f)",
                "localPosition + new Vector3(0f, 0f, 0.010f)",
                "characterSize");

            Forbid(result, root, GameplayPath,
                "TUTORIAL_V1011342_EXTRA_PET_ROOM_REMAINS",
                "tutorialRoomCount = 22");
            Forbid(result, root, LessonPath,
                "TUTORIAL_V1011342_SHIFTED_ROOM_MAP_REMAINS",
                "roomIndex = 21;",
                "BD PET THEN IMMEDIATE REMOUNT V10.11.30.38");
            Forbid(result, root, PresenterPath,
                "HANDHELD_V1011342_RAISED_SHORTCUT_PRINT_REMAINS",
                "new Color(0.74f, 0.84f, 0.96f, 0.98f)");
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
                        "Forbidden stale contract token remains: " + token);
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
