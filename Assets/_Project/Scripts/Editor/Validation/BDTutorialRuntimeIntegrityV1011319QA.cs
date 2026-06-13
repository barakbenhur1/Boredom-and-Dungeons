#if UNITY_EDITOR
using System.IO;

namespace BoredomAndDungeons.EditorTools.Validation
{
    internal static class BDTutorialRuntimeIntegrityV1011319QA
    {
        private const string TutorialPath =
            "Assets/_Project/Scripts/Runtime/UI/" +
            "BDModernHandheld3DPresenter.FirstLaunchTutorial.cs";
        private const string GameplayPath =
            "Assets/_Project/Scripts/Runtime/UI/" +
            "BDModernHandheld3DPresenter.FirstLaunchTutorial.Gameplay.cs";
        private const string LessonPath =
            "Assets/_Project/Scripts/Runtime/UI/" +
            "BDModernHandheld3DPresenter.FirstLaunchTutorial.LessonScreens.cs";
        private const string DialoguePath =
            "Assets/_Project/Scripts/Runtime/UI/" +
            "BDModernHandheld3DPresenter.ChildApproachDialogue.cs";
        private const string FinalPassPath =
            "Assets/_Project/Scripts/Runtime/UI/" +
            "BDModernHandheld3DPresenter.FirstLaunchTutorial.FinalProductionPass.cs";
        private const string PresenterPath =
            "Assets/_Project/Scripts/Runtime/UI/BDModernHandheld3DPresenter.cs";

        public static void Scan(BDOneClickQAResult result)
        {
            string root = Directory.GetCurrentDirectory();

            Require(result, root, LessonPath,
                "TUTORIAL_V1011319_SCREEN_FLOW_INTEGRITY_MISSING",
                "TutorialLessonScreenHandoffSeconds = 0.72f",
                "BeginFirstLaunchTutorialContinuousRoomHandoff",
                "UpdateFirstLaunchTutorialContinuousRoomHandoff",
                "SuspendFirstLaunchTutorialCompletedLessonWorld",
                "ResetFirstLaunchTutorialTransientWorldForNewScreen",
                "firstLaunchTutorialRespawnOverlay.gameObject.SetActive(false)");
            Forbid(result, root, LessonPath,
                "TUTORIAL_V1011319_OLD_SCREEN_TRANSITION_REMAINS",
                "TutorialLessonScreenTransitionSeconds = 0.34f",
                "TutorialLessonScreenFadeInEnd",
                "TutorialLessonScreenFadeOutStart");

            Require(result, root, GameplayPath,
                "TUTORIAL_V1011319_INSTRUCTION_RELEASE_MISSING",
                "ReleaseFirstLaunchTutorialInstructionForScreenTransition",
                "firstLaunchTutorialTravelDistance >= 64f",
                "firstLaunchTutorialInstructionLatchedV101123 = false",
                "firstLaunchTutorialContinuousHandoffActive = false",
                "IsFirstLaunchTutorialWorldHeavyPress");

            Require(result, root, FinalPassPath,
                "TUTORIAL_V1011319_PARALLEL_TRAVEL_OWNER_REMAINS",
                "BD LESSON-SCREEN OWNER SUPERSEDES LEGACY TRAVEL GATE V10.11.30.19",
                "if (firstLaunchTutorialLessonScreenInitialized)",
                "firstLaunchTutorialTravelGateActive = false;");

            Require(result, root, TutorialPath,
                "TUTORIAL_V1011319_INPUT_PARITY_MISSING",
                "case \"MOVE\": return \"WASD / ARROW KEYS\";",
                "case \"INTERACT\": return \"E\";",
                "case \"ATTACK\": return \"J / LEFT CLICK\";",
                "case \"HEAL\": return \"HOLD F\";",
                "case \"RANGED\": return \"Q / HOLD Q\";",
                "case \"HEAVY\": return \"K / RIGHT CLICK\";",
                "case \"JUMP_ATTACK\": return \"SPACE + J / SPACE + LEFT CLICK\";",
                "Keyboard.current.qKey.isPressed",
                "Gamepad.current.buttonEast.wasPressedThisFrame",
                "keyboard.aKey.wasPressedThisFrame",
                "keyboard.dKey.wasPressedThisFrame",
                "FirstLaunchTutorialStep.JumpAttack",
                "FirstLaunchTutorialStep.HazardKnockback",
                "IsFirstLaunchTutorialPointerHeldForAction");
            Forbid(result, root, TutorialPath,
                "TUTORIAL_V1011319_STALE_INPUT_COPY_REMAINS",
                "case \"MOVE\": return \"ARROW KEYS\";",
                "case \"INTERACT\": return \"ENTER\";",
                "case \"HEAL\": return \"HOLD LEFT CLICK\";",
                "case \"RANGED\": return \"LEFT CLICK / HOLD\";",
                "Gamepad.current.selectButton.wasPressedThisFrame");

            Require(result, root, DialoguePath,
                "TUTORIAL_V1011319_LEFT_DIALOGUE_POINTER_MISSING",
                "BD LEFT-POINTING MOTHER BUBBLE TAIL V10.11.30.19",
                "new Vector2(-16f, -68f)",
                "new Vector2(-42f, -66f)",
                "Mother Tail Curve Far Frame",
                "new Vector2(26f, 26f)",
                "panelObject.transform.SetSiblingIndex(4)",
                "seamObject.transform.SetSiblingIndex(5)");
            Forbid(result, root, DialoguePath,
                "TUTORIAL_V1011319_DOWN_DIALOGUE_POINTER_REMAINS",
                "new Vector2(62f, -136f)",
                "new Vector2(38f, -154f)");

            Require(result, root, PresenterPath,
                "TUTORIAL_V1011330_PERSISTENT_SCREEN_DEPTH_MISSING",
                "BD EXPLICIT PERSISTENT SCREEN COLOR DEPTH V10.11.30.31",
                "private RenderTexture screenDepthRenderTexture;",
                "screenDescriptor.depthStencilFormat = GraphicsFormat.None",
                "screenDescriptor.memoryless = RenderTextureMemoryless.None",
                "screenDepthDescriptor.memoryless = RenderTextureMemoryless.None",
                "screenDepthDescriptor.depthStencilFormat =",
                "screenCamera.SetTargetBuffers(",
                "screenRenderTexture.colorBuffer",
                "screenDepthRenderTexture.depthBuffer");
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
                        "Missing required contract token: " + token
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
            {
                Add(result, code, relative, "Required file missing.");
                return;
            }

            string source = File.ReadAllText(full);
            foreach (string token in tokens)
            {
                if (source.Contains(token))
                {
                    Add(
                        result,
                        code,
                        relative,
                        "Forbidden regression token remains: " + token
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
