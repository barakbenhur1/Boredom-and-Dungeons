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
        private const string BindingVisualPath =
            "Assets/_Project/Scripts/Runtime/UI/" +
            "BDModernHandheld3DPresenter.FirstLaunchTutorial.BindingVisuals.cs";
        private const string IconInstallerPath =
            "Assets/_Project/Scripts/Editor/Branding/" +
            "BDApplicationIconInstaller.cs";
        private const string IconPath =
            "Assets/_Project/Art/Branding/" +
            "BoredomAndDungeons_AppIcon.png";
        private const string FinalPath =
            "Assets/_Project/Scripts/Runtime/UI/" +
            "BDModernHandheld3DPresenter.FirstLaunchTutorial.FinalProductionPass.cs";
        private const string BootPath =
            "Assets/_Project/Scripts/Runtime/UI/BDBBHBootIntro.cs";
        private const string PresenterPath =
            "Assets/_Project/Scripts/Runtime/UI/BDModernHandheld3DPresenter.cs";
        private const string GameplayVisibilityPath =
            "Assets/_Project/Scripts/Runtime/UI/BDGameplayUiVisibility.cs";
        private const string TutorialUiPath =
            "Assets/_Project/Scripts/Runtime/UI/" +
            "BDModernHandheld3DPresenter.FirstLaunchTutorial.cs";
        private const string HorseTaskPath =
            "ProjectGuide/Tasks/BACKLOG/" +
            "FULL_GAME_HORSE_ACCELERATION_BRAKING_AND_WEIGHT.md";

        public static void Scan(BDOneClickQAResult result)
        {
            string root = Directory.GetCurrentDirectory();

            Require(result, root, DialoguePath,
                "TUTORIAL_V101113_MOTHER_DIALOGUE_MISSING",
                "Sweety, where are you?",
                "Mother Tail Curve Far",
                "Mother Speech Bubble Tail Seam",
                "CreateChildApproachMurmurClip");
            Forbid(result, root, DialoguePath,
                "TUTORIAL_V101113_CHILD_REPLY_REMAINS",
                "רק שניה",
                "Child Speech Bubble Visual",
                "childApproachChildVoiceClip",
                "ChildApproachChildReply");
            Require(result, root, ChildPath,
                "TUTORIAL_V101113_DIALOGUE_TIMING_MISSING",
                "ChildApproachWalkStartsAtSeconds = 3.84f",
                "ChildApproachDialogueHoldEndsAtSeconds = 4.18f",
                "ChildApproachDialogueExitEndsAtSeconds = 4.78f",
                "BD MOTHER BUBBLE OVERLAPS FIRST CHILD STEPS V10.11.17",
                "ChildApproachWalkEndsAtSeconds = 6.72f",
                "ChildApproachPowerOnStartsAtSeconds = 9.20f",
                "ChildApproachPowerOnEndsAtSeconds = 10.20f");
            Forbid(result, root, ChildPath,
                "TUTORIAL_V101113_CHILD_REPLY_TIMING_REMAINS",
                "ChildApproachChildReplyEnterStartsAtSeconds",
                "ChildApproachChildReplyExitEndsAtSeconds");
            Require(result, root, IntroPath,
                "TUTORIAL_V101117_POST_INTRO_SEQUENCE_MISSING",
                "IntroMainMenuTotalSeconds = 10.67f",
                "UpdateChildApproachDialogue(elapsed)",
                "BD INTRO DRIP UNDERLAY HANDOFF V10.11.17",
                "SetChildApproachSceneFadeImmediate(0f)");
            Forbid(result, root, IntroPath,
                "TUTORIAL_V101117_STALE_DRIP_VISIBILITY_GATE",
                "BDBBHBootIntro.IsDripping ? 0f : 1f");
            Require(result, root, BootPath,
                "TUTORIAL_V101111_INTRO_DRIP_MISSING",
                "public static bool IsDripping",
                "DrawDrippingBootLayer",
                "DripStripCount",
                "BD INTRO TRUE DRIP V10.11.17",
                "GUI.BeginGroup(new Rect(x, 0f, width, Screen.height))",
                "GUI.BeginGroup(new Rect(-x, offsetY, Screen.width, Screen.height))",
                "FadeOutDuration = 0.62f");

            Require(result, root, GameplayPath,
                "TUTORIAL_V101111_FORWARD_COURSE_MISSING",
                "TutorialWorldMaxX = 6480f",
                "firstLaunchTutorialProgressFloorX",
                "targetCamera = Mathf.Max",
                "BeginFirstLaunchTutorialRelicCompletion",
                "ShouldBlockFirstLaunchTutorialActionForTravel",
                "new Vector2(-820f, -108f)",
                "firstLaunchTutorialTravelDistance >= 118f",
                "UnlockFirstLaunchTutorialAbilitiesForStep",
                "RequireFirstLaunchTutorialAbility",
                "firstLaunchTutorialJumpUnlocked",
                "UpdateFirstLaunchTutorialLessonScreenFlow()");
            Require(result, root, CoursePath,
                "TUTORIAL_V101111_PERSISTENT_COURSE_MISSING",
                "TutorialWallJumpWallX = 4380f",
                "TutorialMiniBossStationX = 5280f",
                "firstLaunchTutorialWallJumpWall.gameObject.SetActive(true)",
                "preserveVisibleActor",
                "spawnX = visibleRightEdge + 96f",
                "firstLaunchTutorialFinishGate.gameObject.SetActive(true)",
                "EnsureFirstLaunchTutorialPersistentCourseGeometryVisible");
            Forbid(result, root, CoursePath,
                "TUTORIAL_V101111_VISIBLE_FINISH_GATE_DESPAWN",
                "firstLaunchTutorialFinishGate.gameObject.SetActive(false)");
            Require(result, root, FinalPath,
                "TUTORIAL_V101111_BOSS_AND_HIT_REACTION_MISSING",
                "LaunchFirstLaunchTutorialBossFan",
                "firstLaunchTutorialBossFanProjectiles",
                "TutorialBossPhysicalAttackKind.Slash",
                "TutorialBossPhysicalAttackKind.JumpSlam",
                "RenderFirstLaunchTutorialBossPhysicalTelegraph",
                "BeginFirstLaunchTutorialActorHitReaction",
                "BeginFirstLaunchTutorialPlayerHitReaction",
                "ResolveFirstLaunchTutorialLivingActorCollisionX",
                "actor.Image.gameObject.activeInHierarchy");
            Require(result, root, FinalPath,
                "TUTORIAL_V101111_COMBAT_PRESENTATION_MISSING",
                "OverrideFirstLaunchTutorialAttackPresentation",
                "firstLaunchTutorialFinalAttackWasAirborne",
                "strikeProgress = SmoothestStep01",
                "Mathf.Clamp01(progress / 0.66f)",
                "direction.x >= 0f ? 0f : 180f",
                "ResolveFirstLaunchTutorialBossPhysicalRange",
                "IsFirstLaunchTutorialBossPhysicalHit");
            Require(result, root, FinalPath,
                "TUTORIAL_V101111_RELIC_CINEMATIC_MISSING",
                "RELIC ACQUIRED",
                "firstLaunchTutorialRelicCompletionPlayerPosition",
                "Relic Magical Wash",
                "Relic Expanding Ring",
                "BDFirstLaunchTutorialStateStore.MarkCompleted",
                "displayedPageInitialized = false");
            Require(result, root, FinalPath,
                "TUTORIAL_V101111_INPUT_MIRROR_MISSING",
                "PulseMirroredTutorialPhysicalControl",
                "UpdateFirstLaunchTutorialMirroredPointerControl",
                "PulsePersistentControl");
            Require(result, root, GameplayPath,
                "TUTORIAL_V101111_CONTINUOUS_INPUT_MIRROR_MISSING",
                "source != FirstLaunchTutorialInputSource.Handheld",
                "ControlAction.DPadLeft",
                "ControlAction.DPadRight");
            Require(result, root, PresenterPath,
                "HANDHELD_SCREEN_ITEM_CONFIRM_MIRROR_MISSING",
                "BD SCREEN ITEM MIRRORS PHYSICAL CONFIRM V10.11.11",
                "ControlAction.ScreenItem",
                "ControlAction.Confirm");
            Require(result, root, GameplayVisibilityPath,
                "TUTORIAL_V101113_GAMEPLAY_HUD_LEAK_GUARD_MISSING",
                "BDModernHandheld3DPresenter.SuppressLegacyMenu",
                "return false;");

            Require(
                result,
                root,
                PolishPath,
                "TUTORIAL_V101111_PLAYER_AND_TEXT_POLISH_MISSING",
                "firstLaunchTutorialPrompt, 62, 48, 62",
                "firstLaunchTutorialDetail, 34, 27, 34",
                "VerticalWrapMode.Truncate",
                "BindFirstLaunchTutorialSimplePlayerVisual()",
                "B&D Tutorial Player Simple Right Facing Sprite",
                "Positive X is authored facing right"
            );
            Require(result, root, LetterPath,
                "TUTORIAL_V101111_TEXT_EFFECT_LIFECYCLE_MISSING",
                "protected override void OnEnable()",
                "base.OnEnable();",
                "characterIndex * 0.012f");
            Require(result, root, BossChargePath,
                "TUTORIAL_V101111_BOSS_CHARGE_REGRESSION",
                "FireFirstLaunchTutorialBossChargedShotAutomatically",
                "FireFirstLaunchTutorialBossOrdinaryShot",
                "CHARGED — WAIT FOR RECOVERY",
                "Releasing is not required");
            Require(result, root, RepairPath,
                "TUTORIAL_V101111_IMPACT_TRANSACTION_REGRESSION",
                "ResolveFirstLaunchTutorialRangedProjectileImpact",
                "hitLivingTarget",
                "firstLaunchTutorialPendingShotTarget");
            Require(result, root, PixelPath,
                "TUTORIAL_V101111_POLISH_UPDATE_REGRESSION",
                "UpdateFirstLaunchTutorialV1011Polish()");
            Forbid(result, root, GameplayPath,
                "TUTORIAL_V101113_START_OR_TRIGGER_REGRESSION",
                "new Vector2(-900f, -108f)",
                "firstLaunchTutorialTravelDistance >= 12f");
            Forbid(result, root, FinalPath,
                "TUTORIAL_V101113_IMMEDIATE_LESSON_TRAVEL_GATE",
                "case FirstLaunchTutorialStep.Jump:\n                case FirstLaunchTutorialStep.JumpAttack",
                "case FirstLaunchTutorialStep.MountHorse:\n                case FirstLaunchTutorialStep.AttackEnemy");

            Require(result, root, GameplayPath,
                "TUTORIAL_V101114_WORLD_MOUSE_ATTACK_MISSING",
                "IsFirstLaunchTutorialWorldLightPress",
                "screen hit cannot consume the attack",
                "HandleFirstLaunchTutorialFreePlayAction(");
            Forbid(result, root, FinalPath,
                "TUTORIAL_V101114_MOUNT_TRAVEL_GATE_REMAINS",
                "case FirstLaunchTutorialStep.WallJump:\n                case FirstLaunchTutorialStep.MountHorse:");
            Require(result, root, IntroPath,
                "TUTORIAL_V101114_DRIP_BACKDROP_NOT_VISIBLE",
                "SetChildApproachSceneFadeImmediate(0f)");
            Require(result, root, TutorialUiPath,
                "TUTORIAL_V101114_DUAL_BINDING_CARDS_MISSING",
                "PHYSICAL HANDHELD",
                "firstLaunchTutorialBindingDivider.gameObject.SetActive(true)");

            Require(result, root, GameplayPath,
                "TUTORIAL_V101117_MOUNT_HANDOFF_MISSING",
                "BD MOUNT LESSON PERSISTENCE V10.11.17",
                "BD IMMEDIATE POST-JUMP MOUNT TEACHING V10.11.17",
                "BD PLAYER ART FINAL OWNER V10.11.17");
            Require(result, root, CoursePath,
                "TUTORIAL_V101117_JUMP_MOUNT_STATE_MISSING",
                "BD JUMP TO MOUNT HANDOFF V10.11.17",
                "TutorialJumpObstacleX + 24f",
                "TutorialHorseStartX - 64f",
                "JUMP CLEARED — MOUNT THE HORSE");
            Require(
                result,
                root,
                PolishPath,
                "TUTORIAL_V101117_PLAYER_VISUAL_MISSING",
                "BindFirstLaunchTutorialSimplePlayerVisual()",
                "B&D Tutorial Player Simple Right Facing Sprite",
                "Positive X is authored facing right"
            );
            Require(result, root, BindingVisualPath,
                "TUTORIAL_V101117_INDIE_BINDING_VISUALS_MISSING",
                "BD INDIE INPUT KEYCAPS V10.11.17",
                "BuildFirstLaunchTutorialPhysicalBindingVisual",
                "PHYSICAL HANDHELD",
                "Keyboard / Mouse Keycap");
            Require(result, root, IconInstallerPath,
                "APPLICATION_ICON_INSTALLER_MISSING",
                "BoredomAndDungeons_AppIcon.png",
                "PlayerSettings.SetIcons",
                "NamedBuildTarget.Standalone");
            Require(result, root, IconPath,
                "APPLICATION_ICON_ASSET_MISSING");

            Require(result, root, HorseTaskPath,
                "FULL_GAME_HORSE_MOMENTUM_TASK_MISSING",
                "Status:** `QUEUED",
                "1.5 seconds",
                "0.9 seconds",
                "Full game only",
                "Do not apply to the standalone first-launch tutorial");
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
