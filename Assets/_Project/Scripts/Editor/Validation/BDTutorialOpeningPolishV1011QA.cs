#if UNITY_EDITOR
using System.IO;

namespace BoredomAndDungeons.EditorTools.Validation
{
    internal static class BDTutorialOpeningPolishV1011QA
    {
        private const string DialoguePath =
            "Assets/_Project/Scripts/Runtime/UI/" +
            "BDModernHandheld3DPresenter.ChildApproachDialogue.cs";
        private const string ChildPath =
            "Assets/_Project/Scripts/Runtime/UI/" +
            "BDModernHandheld3DPresenter.ChildApproachCinematic.cs";
        private const string IntroPath =
            "Assets/_Project/Scripts/Runtime/UI/" +
            "BDModernHandheld3DPresenter.IntroToMainMenuTransition.cs";
        private const string CoursePath =
            "Assets/_Project/Scripts/Runtime/UI/" +
            "BDModernHandheld3DPresenter.FirstLaunchTutorial.ProductionCourse.cs";
        private const string GameplayPath =
            "Assets/_Project/Scripts/Runtime/UI/" +
            "BDModernHandheld3DPresenter.FirstLaunchTutorial.Gameplay.cs";
        private const string ActionPath =
            "Assets/_Project/Scripts/Runtime/UI/" +
            "BDModernHandheld3DPresenter.FirstLaunchTutorial.ActionPresentation.cs";
        private const string RepairPath =
            "Assets/_Project/Scripts/Runtime/UI/" +
            "BDModernHandheld3DPresenter.FirstLaunchTutorial.V108Repair.cs";
        private const string PolishPath =
            "Assets/_Project/Scripts/Runtime/UI/" +
            "BDModernHandheld3DPresenter.FirstLaunchTutorial.V1011Polish.cs";
        private const string BossChargePath =
            "Assets/_Project/Scripts/Runtime/UI/" +
            "BDModernHandheld3DPresenter.FirstLaunchTutorial.BossCharge.cs";
        private const string PixelPath =
            "Assets/_Project/Scripts/Runtime/UI/" +
            "BDModernHandheld3DPresenter.FirstLaunchTutorial.PixelPresentation.cs";
        private const string LetterPath =
            "Assets/_Project/Scripts/Runtime/UI/" +
            "BDTutorialLetterPulseEffect.cs";

        public static void Scan(BDOneClickQAResult result)
        {
            string root = Directory.GetCurrentDirectory();
            Require(result, root, DialoguePath,
                "TUTORIAL_V10118_DIALOGUE_UNIFIED_VISUAL_MISSING",
                "Mother Speech Bubble Visual",
                "childApproachDialogueVisualRect",
                "Mother Speech Bubble Tail Shadow",
                "Mother Speech Bubble Tail Seam Cover",
                "panelObject.transform.SetAsLastSibling()",
                "childApproachDialogueVisualRect.localScale",
                "childApproachDialogueVisualRect.anchoredPosition");
            Forbid(result, root, DialoguePath,
                "TUTORIAL_V10118_DIALOGUE_SPLIT_ANIMATION_REGRESSION",
                "childApproachDialogueTailRect.localScale =",
                "childApproachDialogueTailRect.anchoredPosition =\n                    childApproachDialogueRestPosition");

            Require(result, root, ChildPath,
                "TUTORIAL_V10113_TIMING_CONTRACT_MISSING",
                "ChildApproachDialogueEnterStartsAtSeconds = 1.55f",
                "ChildApproachDialogueExitEndsAtSeconds = 3.70f",
                "ChildApproachWalkStartsAtSeconds = 3.82f",
                "ChildApproachPowerOnStartsAtSeconds = 9.20f",
                "ChildApproachPowerOnEndsAtSeconds = 10.20f");
            Require(result, root, IntroPath,
                "TUTORIAL_V10113_SEQUENCE_CONTRACT_MISSING",
                "IntroMainMenuTotalSeconds = 10.67f",
                "UpdateChildApproachDialogue(elapsed)",
                "StopChildApproachDialogueVoice()");

            Require(result, root, CoursePath,
                "TUTORIAL_V10118_WALL_JUMP_STATE_CONTRACT_MISSING",
                "TutorialWallJumpPlatformStandingY = 8f",
                "TutorialWallJumpUpperGroundStandingY = 64f",
                "firstLaunchTutorialWallJumpConsumed",
                "TutorialJumpVelocity * 1.28f",
                "TutorialWallJumpPlatformMaxX = 3370f");
            Require(result, root, GameplayPath,
                "TUTORIAL_V10118_WALL_JUMP_ROUTE_CONTRACT_MISSING",
                "TutorialWallJumpUpperGroundStandingY + 12f",
                "TutorialWallJumpWallX - 52f",
                "firstLaunchTutorialWallJumpConsumed = false");
            Require(result, root, PolishPath,
                "TUTORIAL_V10118_PLATFORM_SUPPORT_CONTRACT_MISSING",
                "TutorialWallJumpPlatformStandingY",
                "TutorialWallJumpUpperGroundStandingY");

            Require(result, root, PolishPath,
                "TUTORIAL_V101110_MODERN_TYPOGRAPHY_MISSING",
                "ConfigureFirstLaunchTutorialText(",
                "firstLaunchTutorialKeyboardBindingTitle",
                "firstLaunchTutorialHandheldBindingTitle",
                "firstLaunchTutorialHealthText",
                "firstLaunchTutorialAmmoText",
                "firstLaunchTutorialBossHealthText",
                "VerticalWrapMode.Truncate",
                "firstLaunchTutorialTextEffects");
            Require(result, root, PolishPath,
                "TUTORIAL_V101110_PLAYER_AND_COLLECTIBLE_VISUALS_MISSING",
                "B&D Tutorial Player Modern Pixel Sprite",
                "Color hair =",
                "Color shirt =",
                "Color pants =",
                "B&D Tutorial Collectible Relic Sprite",
                "COLLECT THE GREEN RELIC",
                "Contact collects it automatically.");
            Require(result, root, LetterPath,
                "TUTORIAL_V10118_LETTER_ANIMATION_MISSING",
                "textChangedAt",
                "characterIndex * 0.012f",
                "verticalAmplitude",
                "horizontalAmplitude",
                "Color.Lerp(",
                "secondary,",
                "protected override void OnEnable()",
                "base.OnEnable();");

            Require(result, root, CoursePath,
                "TUTORIAL_V10118_BOSS_PRESENTATION_MISSING",
                "actor.AttackSequence % 2 == 1",
                "actor.AttackSequence % 3 == 2",
                "A compact charge orb replaces the old full-length beam",
                "The slam now warns on the floor where it will land",
                "LaunchFirstLaunchTutorialEnemyProjectile(actor)");
            Require(result, root, BossChargePath,
                "TUTORIAL_V10118_BOSS_CHARGED_SHOT_MISSING",
                "TutorialBossChargeDurationSeconds = 1.18f",
                "BeginFirstLaunchTutorialBossChargeInput",
                "UpdateFirstLaunchTutorialBossChargeHold",
                "FireFirstLaunchTutorialBossChargedShotAutomatically",
                "CHARGED — WAIT FOR RECOVERY",
                "Releasing is not required");
            Require(result, root, GameplayPath,
                "TUTORIAL_V10118_BOSS_INPUT_ROUTING_MISSING",
                "IsFirstLaunchTutorialBossCombatStep()",
                "BeginFirstLaunchTutorialBossChargeInput()",
                "UpdateFirstLaunchTutorialBossChargeHold()");

            Require(result, root, RepairPath,
                "TUTORIAL_V10113_IMPACT_CONTRACT_MISSING",
                "ResolveFirstLaunchTutorialRangedProjectileImpact",
                "firstLaunchTutorialPendingShotTarget",
                "hitLivingTarget",
                "boss.UsesProjectileAttack");
            Require(result, root, GameplayPath,
                "TUTORIAL_V10117_MOVEMENT_COMPLETION_CONTRACT_MISSING",
                "float lessonGuidanceX = Mathf.Min",
                "float maximumPlayerX = TutorialWorldMaxX",
                "float horseFacing =",
                "new Vector3(horseFacing, 1f, 1f)");
            Require(result, root, ActionPath,
                "TUTORIAL_V10117_ACTION_PRESENTATION_CONTRACT_MISSING",
                "UpdateFirstLaunchTutorialRangedAttackPresentation(\n                        progress",
                "facing * horizontalScale");
            Require(result, root, RepairPath,
                "TUTORIAL_V10117_VISIBLE_COLLISION_CONTRACT_MISSING",
                "!actor.Image.gameObject.activeInHierarchy");
            Forbid(result, root, CoursePath,
                "TUTORIAL_V10117_DOUBLE_MOUNTED_TARGET_REGRESSION",
                "TutorialMountedStationX + 300f");
            Require(result, root, GameplayPath,
                "TUTORIAL_V101110_START_AND_CONTACT_COLLECTION_MISSING",
                "new Vector2(-900f, -108f)",
                "distance <= 82f || crossedFinish");
            Require(result, root, GameplayPath,
                "TUTORIAL_V101110_CONTACT_COLLECTION_OWNERSHIP_MISSING",
                "Collectibles are contact-owned; interact never completes them.");
            Require(result, root, CoursePath,
                "TUTORIAL_V101110_ENVIRONMENT_IMPACT_SEQUENCE_MISSING",
                "firstLaunchTutorialHazardKnockbackActive",
                "UpdateFirstLaunchTutorialHazardKnockbackSequence()",
                "KNOCK IT INTO THE HAZARD",
                "ENVIRONMENT IMPACT",
                "elapsed < 0.92f");
            Require(result, root, CoursePath,
                "TUTORIAL_V101110_BOSS_ORDINARY_SHOT_MISSING",
                "TutorialEnemyActor actor = bossStep",
                "ordinaryProjectileImpact",
                "actor.AttackCommitted && !ordinaryProjectileImpact");
            Require(result, root, PixelPath,
                "TUTORIAL_V10113_TEXT_PRESENTATION_MISSING",
                "UpdateFirstLaunchTutorialV1011Polish()");
        }

        private static void Require(
            BDOneClickQAResult result,
            string root,
            string relativePath,
            string code,
            params string[] tokens)
        {
            string absolute = Path.Combine(root, relativePath);
            if (!File.Exists(absolute))
            {
                Add(result, code, relativePath, "Required file is missing.");
                return;
            }

            string text = File.ReadAllText(absolute);
            for (int index = 0; index < tokens.Length; index++)
            {
                if (text.Contains(tokens[index]))
                    continue;
                Add(result, code, relativePath,
                    "Missing required contract token: " + tokens[index]);
            }
        }

        private static void Forbid(
            BDOneClickQAResult result,
            string root,
            string relativePath,
            string code,
            params string[] tokens)
        {
            string absolute = Path.Combine(root, relativePath);
            if (!File.Exists(absolute))
            {
                Add(result, code, relativePath, "Required file is missing.");
                return;
            }

            string text = File.ReadAllText(absolute);
            for (int index = 0; index < tokens.Length; index++)
            {
                if (!text.Contains(tokens[index]))
                    continue;
                Add(result, code, relativePath,
                    "Forbidden regression token remains: " + tokens[index]);
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
