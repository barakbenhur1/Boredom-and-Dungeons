#if UNITY_EDITOR
using System.IO;

namespace BoredomAndDungeons.EditorTools.Validation
{
    internal static class
        BDTutorialSpinDismountHorseThrowV1011335QA
    {
        // BD V10.11.30.36 FOCUSED QA COMPILE REPAIR
        private const string LessonPath =
            "Assets/_Project/Scripts/Runtime/UI/" +
            "BDModernHandheld3DPresenter.FirstLaunchTutorial.LessonScreens.cs";
        private const string GameplayPath =
            "Assets/_Project/Scripts/Runtime/UI/" +
            "BDModernHandheld3DPresenter.FirstLaunchTutorial.Gameplay.cs";
        private const string ActionPath =
            "Assets/_Project/Scripts/Runtime/UI/" +
            "BDModernHandheld3DPresenter.FirstLaunchTutorial.ActionPresentation.cs";
        private const string ContractsPathV1011337 =
            "Assets/_Project/Scripts/Runtime/UI/" +
            "BDModernHandheld3DPresenter.FirstLaunchTutorial.LessonContractsV101128.cs";

        public static void Scan(BDOneClickQAResult result)
        {
            string root = Directory.GetCurrentDirectory();

            Require(result, root, LessonPath,
                "TUTORIAL_V1011335_SPIN_AND_DISMOUNT_MISSING",
                "TutorialSpinEnemyOffsetV1011335 = 82f",
                "center - TutorialSpinEnemyOffsetV1011335",
                "center + TutorialSpinEnemyOffsetV1011335",
                "ResolveFirstLaunchTutorialDismountInstructionRevealX()",
                "HideFirstLaunchTutorialDismountInstructionUntilMarkerV1011335()",
                "firstLaunchTutorialDismountInstructionRevealedV1011335");

            Require(result, root, GameplayPath,
                "TUTORIAL_V1011335_DISMOUNT_AND_HORSE_SHOT_OWNER_MISSING",
                "BD MID-SCREEN DISMOUNT INSTRUCTION GATE V10.11.30.35",
                "ResolveFirstLaunchTutorialDismountInstructionRevealX())",
                "BD HORSE THROWS RIDER THEN FLEES V10.11.30.35",
                "FirstLaunchTutorialActionPresentationType.HorseHit");

            Forbid(result, root, GameplayPath,
                "TUTORIAL_V1011335_HORSE_SHOT_TELEPORT_REGRESSION",
                "firstLaunchTutorialPlayerWorldPosition =\n" +
                    "                    firstLaunchTutorialHorseWorldPosition +\n" +
                    "                    new Vector2(78f, 2f);",
                "firstLaunchTutorialHorseWorldPosition =\n" +
                    "                firstLaunchTutorialPlayerWorldPosition +\n" +
                    "                new Vector2(-230f, -8f);");

            Require(result, root, ActionPath,
                "TUTORIAL_V1011335_PROFESSIONAL_HORSE_THROW_MISSING",
                "BD PROFESSIONAL RIDER THROW + HORSE ESCAPE V10.11.30.35",
                "firstLaunchTutorialHorseHitPlayerLandingV1011335",
                "firstLaunchTutorialHorseHitEscapeTargetV1011335",
                "riderArc",
                "horseEscape",
                "CompleteFirstLaunchTutorialHorseHitAnimationV1011335()",
                "FirstLaunchTutorialStep.JumpAttack");

            Require(result, root, GameplayPath,
                "TUTORIAL_V1011337_SPIN_INPUT_ROUTING_MISSING",
                "BD SPIN LESSON ALWAYS UNLOCKED V10.11.30.37",
                "UpdateFirstLaunchTutorialSpinPairV101128();",
                "advancesV1011337");
            Require(result, root, ActionPath,
                "TUTORIAL_V1011337_ATOMIC_SPIN_IMPACT_MISSING",
                "BD SPIN IMPACT FRAME RESOLUTION V10.11.30.37",
                "BD ATOMIC SPIN IMPACT FRAME V10.11.30.37",
                "TryCompleteFirstLaunchTutorialAtomicSpinV101128()",
                "firstLaunchTutorialSpinImpactResolvedV1011337");
            Require(result, root, ContractsPathV1011337,
                "TUTORIAL_V1011337_SPIN_PAIR_ALIGNMENT_MISSING",
                "BD ATOMIC SPIN IMPACT ALIGNMENT V10.11.30.37",
                "TutorialSpinPairOffsetV101128 = 82f");
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
