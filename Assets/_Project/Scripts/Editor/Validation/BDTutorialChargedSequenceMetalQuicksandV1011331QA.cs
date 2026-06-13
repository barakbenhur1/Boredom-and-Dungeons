#if UNITY_EDITOR
using System.IO;

namespace BoredomAndDungeons.EditorTools.Validation
{
    internal static class BDTutorialChargedSequenceMetalQuicksandV1011331QA
    {
        private const string PresenterPath =
            "Assets/_Project/Scripts/Runtime/UI/" +
            "BDModernHandheld3DPresenter.cs";
        private const string LessonScreensPath =
            "Assets/_Project/Scripts/Runtime/UI/" +
            "BDModernHandheld3DPresenter.FirstLaunchTutorial.LessonScreens.cs";
        private const string ProductionCoursePath =
            "Assets/_Project/Scripts/Runtime/UI/" +
            "BDModernHandheld3DPresenter.FirstLaunchTutorial.ProductionCourse.cs";
        private const string HealthPath =
            "Assets/_Project/Scripts/Runtime/BDHealth.cs";
        private const string QuicksandPath =
            "Assets/_Project/Scripts/Runtime/Hazards/BDQuicksandStatus.cs";

        public static void Scan(BDOneClickQAResult result)
        {
            string root = Directory.GetCurrentDirectory();

            Require(result, root, PresenterPath,
                "SCREEN_V1011331_EXPLICIT_COLOR_DEPTH_BUFFERS_MISSING",
                "BD EXPLICIT PERSISTENT SCREEN COLOR DEPTH V10.11.30.31",
                "private RenderTexture screenDepthRenderTexture;",
                "screenDescriptor.depthStencilFormat = GraphicsFormat.None",
                "screenDepthDescriptor.memoryless =",
                "screenDepthDescriptor.depthStencilFormat =",
                "screenDepthRenderTexture =",
                "new RenderTexture(screenDepthDescriptor)",
                "screenCamera.SetTargetBuffers(",
                "screenRenderTexture.colorBuffer",
                "screenDepthRenderTexture.depthBuffer",
                "screenDepthRenderTexture.Release()");
            Forbid(result, root, PresenterPath,
                "SCREEN_V1011331_COMBINED_DEPTH_ATTACHMENT_REGRESSION",
                "screenDescriptor.depthStencilFormat = screenDepthStencilFormat;");

            Require(result, root, LessonScreensPath,
                "TUTORIAL_V1011331_RELOAD_ROOM_SEQUENCE_MISSING",
                "BD MOUNTED RELOAD SAME-ROOM BRIDGE V10.11.30.31",
                "current == FirstLaunchTutorialStep.RangedAttack",
                "next == FirstLaunchTutorialStep.Reload",
                "case FirstLaunchTutorialStep.RangedAttack:",
                "case FirstLaunchTutorialStep.Reload:",
                "roomIndex = 9;",
                "case FirstLaunchTutorialStep.ChargedShot:",
                "roomIndex = 10;",
                "case FirstLaunchTutorialStep.MountedImpact:",
                "roomIndex = 11;");

            Require(result, root, ProductionCoursePath,
                "TUTORIAL_V1011331_MOUNTED_IMPACT_WORLD_TARGET_MISSING",
                "BD STABLE WORLD-OWNED MOUNTED IMPACT TARGET V10.11.30.31",
                "BD MOUNTED IMPACT PLAYER-RELATIVE TARGET RETIRED V10.11.30.31",
                "actor.Image != firstLaunchTutorialEnemy",
                "Vector2.Distance(",
                "TutorialMountedCollisionRadius + TutorialEnemyCollisionRadius",
                "FirstLaunchTutorialDamageSourceV101125.MountedImpact",
                "if (!impactApplied || !actor.Dead)");
            Forbid(result, root, ProductionCoursePath,
                "TUTORIAL_V1011331_MOUNTED_IMPACT_TETHER_REMAINS",
                "EnsureFirstLaunchTutorialMountedImpactTarget(");

            Require(result, root, HealthPath,
                "QUICKSAND_V1011331_QUIET_DAMAGE_ROUTE_MISSING",
                "ApplyUnavoidableDamage(float amount, bool logResolvedDamage)",
                "bool logResolvedDamage)",
                "if (logDamage && logResolvedDamage)");
            Require(result, root, QuicksandPath,
                "QUICKSAND_V1011331_CONSOLE_SPAM_SUPPRESSION_MISSING",
                "BD QUIET PERIODIC QUICKSAND DAMAGE V10.11.30.31",
                "logResolvedDamage: false");
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
