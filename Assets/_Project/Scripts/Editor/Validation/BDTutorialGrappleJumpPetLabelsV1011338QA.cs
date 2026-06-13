#if UNITY_EDITOR
using System.IO;

namespace BoredomAndDungeons.EditorTools.Validation
{
    internal static class BDTutorialGrappleJumpPetLabelsV1011338QA
    {
        // BD V39 QA SAME-ROOM / ENGRAVED REALIGNMENT V10.11.30.42
        private const string CorePath =
            "Assets/_Project/Scripts/Runtime/UI/" +
            "BDModernHandheld3DPresenter.FirstLaunchTutorial.cs";
        private const string GameplayPath =
            "Assets/_Project/Scripts/Runtime/UI/" +
            "BDModernHandheld3DPresenter.FirstLaunchTutorial.Gameplay.cs";
        private const string LessonPath =
            "Assets/_Project/Scripts/Runtime/UI/" +
            "BDModernHandheld3DPresenter.FirstLaunchTutorial.LessonScreens.cs";
        private const string ActionPath =
            "Assets/_Project/Scripts/Runtime/UI/" +
            "BDModernHandheld3DPresenter.FirstLaunchTutorial.ActionPresentation.cs";
        private const string RuntimePath =
            "Assets/_Project/Scripts/Runtime/UI/" +
            "BDModernHandheld3DPresenter.FirstLaunchTutorial." +
            "GrappleJumpPetV1011338.cs";
        private const string PresenterPath =
            "Assets/_Project/Scripts/Runtime/UI/BDModernHandheld3DPresenter.cs";

        public static void Scan(BDOneClickQAResult result)
        {
            string root = Directory.GetCurrentDirectory();

            Require(result, root, CorePath,
                "TUTORIAL_V1011338_PET_BINDING_MISSING",
                "PetHorse, // BD PET LESSON V10.11.30.38",
                "case FirstLaunchTutorialStep.PetHorse: return \"PET\";",
                "case \"PET\": return \"TAB\";",
                "return \"VIEW / SELECT\";",
                "return \"SELECT\";",
                "BD PHYSICAL SELECT PET ROUTING V10.11.30.38");

            Require(result, root, GameplayPath,
                "TUTORIAL_V1011338_JUMP_OR_GRAPPLE_ROUTING_MISSING",
                "UpdateFirstLaunchTutorialPetInputV1011338();",
                "RejectFirstLaunchTutorialGroundedJumpAttackV1011338()",
                "RejectFirstLaunchTutorialNonJumpAttackV1011338",
                "PrepareFirstLaunchTutorialProfessionalGrappleTargetV1011338",
                "BD PET DOES NOT REUSE E INTERACT V10.11.30.38");

            Require(result, root, LessonPath,
                "TUTORIAL_V1011338_PET_ROOM_MISSING",
                "ResetFirstLaunchTutorialGrappleJumpPetStateV1011338(step);",
                "case FirstLaunchTutorialStep.PetHorse:",
                "BD HEAL PET MOUNT SAME ROOM V10.11.30.42",
                "roomIndex = 20;");

            Require(result, root, ActionPath,
                "TUTORIAL_V1011338_PRESENTATION_HOOKS_MISSING",
                "PetHorse, // BD PET PRESENTATION V10.11.30.38",
                "BD PROFESSIONAL HOOK CONTACT + PULL V10.11.30.38",
                "CompleteFirstLaunchTutorialProfessionalGrapplePullV1011338",
                "CompleteFirstLaunchTutorialHealThenPetV1011338",
                "CompleteFirstLaunchTutorialPetHorseAnimationV1011338");

            Require(result, root, RuntimePath,
                "TUTORIAL_V1011338_RUNTIME_OWNER_MISSING",
                "Keyboard.current.tabKey.wasPressedThisFrame",
                "Gamepad.current.selectButton.wasPressedThisFrame",
                "ENEMY PULLED — ATTACK NOW",
                "ForceFirstLaunchTutorialLessonActorDeathV101128",
                "GROUND ATTACKS CANNOT HURT THIS TARGET",
                "FirstLaunchTutorialStep.PetHorse");

            Require(result, root, PresenterPath,
                "HANDHELD_V1011338_SHORTCUT_LABEL_FINISH_MISSING",
                "BD ENGRAVED SHORTCUT LABEL V10.11.30.42",
                "characterSize",
                "new Color(0.055f, 0.075f, 0.095f, 1f)");
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
