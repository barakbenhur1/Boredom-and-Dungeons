#if UNITY_EDITOR
using System.IO;

namespace BoredomAndDungeons.EditorTools.Validation
{
    internal static class BDTutorialCenteredParryHorseMetalV1011326QA
    {
        private const string GameplayPath =
            "Assets/_Project/Scripts/Runtime/UI/" +
            "BDModernHandheld3DPresenter.FirstLaunchTutorial.Gameplay.cs";
        private const string LessonPath =
            "Assets/_Project/Scripts/Runtime/UI/" +
            "BDModernHandheld3DPresenter.FirstLaunchTutorial.LessonScreens.cs";
        private const string ContractPath =
            "Assets/_Project/Scripts/Runtime/UI/" +
            "BDModernHandheld3DPresenter.FirstLaunchTutorial.LessonContractsV101128.cs";
        private const string RepairPath =
            "Assets/_Project/Scripts/Runtime/UI/" +
            "BDModernHandheld3DPresenter.FirstLaunchTutorial.V108Repair.cs";
        private const string PresenterPath =
            "Assets/_Project/Scripts/Runtime/UI/BDModernHandheld3DPresenter.cs";
        private const string SettingsPath =
            "ProjectSettings/ProjectSettings.asset";

        public static void Scan(BDOneClickQAResult result)
        {
            string root = Directory.GetCurrentDirectory();

            Require(result, root, LessonPath,
                "TUTORIAL_V1011326_CENTERED_VISIBLE_TARGET_MISSING",
                "BD EXACT-CENTER VISIBLE ONE-HIT TARGET V10.11.30.26",
                "TutorialEnemyRole.Small,\n                        center,",
                "PrepareFirstLaunchTutorialPrimaryEnemyVisualV1011326",
                "new Vector2(64f, 92f)",
                "firstLaunchTutorialEnemy.rectTransform.SetAsLastSibling()",
                "firstLaunchTutorialEnemyPixelVisual.SetAsLastSibling()",
                "ApplyFirstLaunchTutorialPixelSprite(",
                "firstLaunchTutorialEnemy.enabled = true;");
            Require(result, root, ContractPath,
                "TUTORIAL_V1011326_CENTER_TARGET_PERSISTENCE_MISSING",
                "EnsureFirstLaunchTutorialFocusedLessonActorV101130(\n                    firstLaunchTutorialLessonScreenCenterX",
                "PrepareFirstLaunchTutorialPrimaryEnemyVisualV1011326");
            Forbid(result, root, ContractPath,
                "TUTORIAL_V1011326_PLAYER_RELATIVE_TARGET_REGRESSION",
                "targetX = firstLaunchTutorialPlayerWorldPosition.x + 160f;");

            Require(result, root, GameplayPath,
                "TUTORIAL_V1011326_HORSE_ORDER_MISSING",
                "TutorialWorldMaxX = 17200f",
                "BD DEFERRED HORSE LESSON V10.11.30.27",
                "firstLaunchTutorialLessonScreenCenterX + 118f",
                "FirstLaunchTutorialStep.EnemyArrival",
                "BD COMPLETED STORY ROOM TRAVEL V10.11.30.26",
                "!firstLaunchTutorialLessonCompleteAwaitingTravel &&");
            Require(result, root, LessonPath,
                "TUTORIAL_V1011326_HORSE_ROOM_MAP_MISSING",
                "case FirstLaunchTutorialStep.Jump:\n                    roomIndex = 0;",
                "case FirstLaunchTutorialStep.AttackEnemy:\n                    roomIndex = 1;",
                "case FirstLaunchTutorialStep.Parry:\n                    roomIndex = 4;",
                "case FirstLaunchTutorialStep.MountHorse:\n                case FirstLaunchTutorialStep.RideHorse:\n                    roomIndex = 5;",
                "case FirstLaunchTutorialStep.EnemyArrival:\n                case FirstLaunchTutorialStep.HorseShot:\n                    roomIndex = 6;",
                "step == FirstLaunchTutorialStep.HorseShot ||");
            Forbid(result, root, LessonPath,
                "TUTORIAL_V1011326_STALE_SAME_ROOM_ORDER_REMAINS",
                "current == FirstLaunchTutorialStep.Jump &&\n                next == FirstLaunchTutorialStep.MountHorse",
                "current == FirstLaunchTutorialStep.HorseShot &&\n                next == FirstLaunchTutorialStep.AttackEnemy");

            Require(result, root, LessonPath,
                "TUTORIAL_V1011326_SINGLE_OWNER_PARRY_MISSING",
                "BD SINGLE-OWNER PARRY TRANSACTION V10.11.30.26",
                "firstLaunchTutorialParryResolvedV1011326",
                "SetFirstLaunchTutorialActorPassiveForScreen(\n                        firstLaunchTutorialEnemy",
                "CancelFirstLaunchTutorialParryProjectilesV1011326",
                "CancelFirstLaunchTutorialEnemyProjectile();");
            Require(result, root, GameplayPath,
                "TUTORIAL_V1011326_PARRY_SOFTLOCK_GUARD_MISSING",
                "firstLaunchTutorialStep != FirstLaunchTutorialStep.Parry ||",
                "firstLaunchTutorialParryResolvedV1011326",
                "shooter == null || shooter.Dead || !shooter.Active",
                "CancelFirstLaunchTutorialEnemyProjectile();",
                "if (firstLaunchTutorialLessonCompleteAwaitingTravel ||");
            Require(result, root, RepairPath,
                "TUTORIAL_V1011326_PARRY_TARGET_INVULNERABLE_MISSING",
                "case FirstLaunchTutorialStep.Parry:",
                "PARRY THE PROJECTILE — DO NOT ATTACK THE SHOOTER",
                "return false;");

            Require(result, root, PresenterPath,
                "SCREEN_V1011326_DEVICE_CAMERA_DEPTH_CONTRACT_MISSING",
                "BD NON-MEMORYLESS DEVICE CAMERA DEPTH V10.11.30.26",
                "deviceCamera.allowMSAA = false;",
                "deviceCamera.depthTextureMode = DepthTextureMode.None;",
                "BD PERSISTENT NON-MEMORYLESS SCREEN DEPTH V10.11.30.30",
                "screenDescriptor.depthStencilFormat = screenDepthStencilFormat;",
                "screenDescriptor.memoryless = RenderTextureMemoryless.None;");
            Forbid(result, root, PresenterPath,
                "SCREEN_V1011326_MSAA_DEPTH_REGRESSION",
                "deviceCamera.allowMSAA = true;",
                "screenDescriptor.depthStencilFormat = GraphicsFormat.None;");
            Require(result, root, SettingsPath,
                "SCREEN_V1011326_PROJECT_MEMORYLESS_UNUSED_MISSING",
                "framebufferDepthMemorylessMode: 0");
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
