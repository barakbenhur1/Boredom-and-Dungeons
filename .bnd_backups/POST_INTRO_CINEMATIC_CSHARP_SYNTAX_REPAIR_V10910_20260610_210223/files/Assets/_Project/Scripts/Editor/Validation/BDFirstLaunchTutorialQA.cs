#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace BoredomAndDungeons.EditorTools.Validation
{
    internal static class BDFirstLaunchTutorialQA
    {
        private static readonly string[] RequiredFiles =
        {
            "Assets/_Project/Scripts/Runtime/UI/BDFirstLaunchTutorialStateStore.cs",
            "Assets/_Project/Scripts/Runtime/BDPlayerCombat.cs",
            "Assets/_Project/Scripts/Runtime/UI/BDModernHandheld3DPresenter.FirstLaunchTutorial.cs",
            "Assets/_Project/Scripts/Runtime/UI/" +
            "BDModernHandheld3DPresenter.FirstLaunchTutorial." +
            "PixelPresentation.cs",
            "Assets/_Project/Scripts/Runtime/UI/" +
            "BDModernHandheld3DPresenter.FirstLaunchTutorial." +
            "PixelText.cs",
            "Assets/_Project/Scripts/Runtime/UI/" +
            "BDModernHandheld3DPresenter.FirstLaunchTutorial." +
            "Gameplay.cs",
            "Assets/_Project/Scripts/Runtime/UI/" +
            "BDModernHandheld3DPresenter.FirstLaunchTutorial." +
            "ActionPresentation.cs",
            "Assets/_Project/Scripts/Runtime/UI/" +
            "BDModernHandheld3DPresenter.FirstLaunchTutorial." +
            "ProductionCourse.cs",
            "Assets/_Project/Scripts/Runtime/UI/" +
            "BDModernHandheld3DPresenter.FirstLaunchTutorial." +
            "V108Repair.cs",
            "Assets/_Project/Scripts/Runtime/UI/" +
            "BDModernHandheld3DPresenter.LaunchPresentationGate.cs",
            "Assets/_Project/Scripts/Runtime/UI/" +
            "BDModernHandheld3DPresenter.IntroToMainMenuTransition.cs",
            "Assets/_Project/Scripts/Runtime/UI/" +
            "BDModernHandheld3DPresenter.CinematicEnvironment.cs",
            "Assets/_Project/Scripts/Editor/Validation/BDFirstLaunchTutorialEditorTools.cs",
            "ProjectGuide/Features/UI/FIRST_LAUNCH_TUTORIAL_V1.md",
            "ProjectGuide/Features/UI/" +
            "FIRST_LAUNCH_TUTORIAL_ENTRY_AND_ANIMATION_V11.md",
            "ProjectGuide/Tasks/ACTIVE/FIRST_LAUNCH_TUTORIAL_AND_HANDHELD_PRODUCTION_REPAIR.md"
        };

        private static readonly string[] RequiredRuntimeTokens =
        {
            "BDFirstLaunchTutorialStatus",
            "MarkInProgress",
            "MarkCompleted",
            "MarkSkipped",
            "PlayerPrefs.Save()",
            "EffectivePage.FirstLaunchTutorial",
            "BuildFirstLaunchTutorialPage",
            "UpdateFirstLaunchTutorialNavigationInput",
            "HandleFirstLaunchTutorialControl",
            "TutorialExitInputGuardSeconds",
            "CONTINUE TUTORIAL",
            "LEAVE TUTORIAL",
            "The tutorial will not appear automatically again.",
            "SetTutorialHighlighted",
            "TutorialSpinHoldSeconds",
            "TutorialHealHoldSeconds",
            "TutorialGrappleHoldSeconds",
            "Tutorial Instruction Panel",
            "ResolveFirstLaunchTutorialKeyboardBinding",
            "ResolveFirstLaunchTutorialHandheldBinding",
            "ResolveFirstLaunchTutorialGamepadBinding",
            "Tutorial Keyboard Binding Card",
            "Tutorial Handheld Binding Card",
            "UpdateFirstLaunchTutorialBindingPresentation",
            "SnapFirstLaunchTutorialPixelValue",
            "Remembered-console palette",
            "KEYBOARD / MOUSE",
            "HANDHELD",
            "CONTROLLER",
            "FilterMode.Point",
            "ApplyFirstLaunchTutorialPixelSprite",
            "BuildFirstLaunchTutorialPixelBackdrop",
            "BeginFirstLaunchTutorialInstructionPresentation",
            "TutorialWorldMinX = -920f",
            "TutorialWorldMaxX = 4250f",
            "UpdateFirstLaunchTutorialFreePlay",
            "UpdateFirstLaunchTutorialContinuousMovement",
            "ResolveFirstLaunchTutorialMaximumPlayerX",
            "ReadFirstLaunchTutorialMovementVector",
            "firstLaunchTutorialMounted",
            "Tutorial Wide Course",
            "PlayFirstLaunchTutorialMountAnimation",
            "PlayFirstLaunchTutorialLightAttackAnimation",
            "PlayFirstLaunchTutorialRangedAttackAnimation",
            "PlayFirstLaunchTutorialDodgeAnimation",
            "PlayFirstLaunchTutorialHeavyAttackAnimation",
            "PlayFirstLaunchTutorialSpinAttackAnimation",
            "PlayFirstLaunchTutorialParryAnimation",
            "PlayFirstLaunchTutorialGrappleAnimation",
            "PlayFirstLaunchTutorialHealAnimation",
            "Tutorial Player Projectile Effect",
            "Tutorial Light Slash Effect",
            "Tutorial Muzzle Flash Effect",
            "Tutorial Spin Effect",
            "Tutorial Parry Effect",
            "Tutorial Grapple Line Effect",
            "ApplyFirstLaunchTutorialPlayerActionPose",
            "ApplyFirstLaunchTutorialHorseActionPose",
            "ApplyFirstLaunchTutorialEnemyActionPose",
            "UpdateFirstLaunchTutorialLightAttackPresentation",
            "UpdateFirstLaunchTutorialRangedAttackPresentation",
            "UpdateFirstLaunchTutorialDodgePresentation",
            "UpdateFirstLaunchTutorialHeavyAttackPresentation",
            "UpdateFirstLaunchTutorialSpinPresentation",
            "UpdateFirstLaunchTutorialParryPresentation",
            "UpdateFirstLaunchTutorialGrapplePresentation",
            "UpdateFirstLaunchTutorialHealPresentation",
            "UpdateFirstLaunchTutorialHorseHitPresentation",
            "FirstLaunchTutorialStep.Jump",
            "RequestFirstLaunchTutorialJump",
            "TutorialJumpObstacleX",
            "TutorialLearningState",
            "TutorialCheckpoint",
            "ResetFirstLaunchTutorialToCheckpoint",
            "TutorialMagazineSize",
            "BeginFirstLaunchTutorialReload",
            "MountedImpact",
            "Tutorial Optional Secret",
            "CombinedEncounter",
            "MiniBossPhaseOne",
            "MiniBossPhaseTwo",
            "Tutorial Mini Boss Health",
            "UpdateFirstLaunchTutorialMiniBossDeath",
            "Tutorial Hold Progress Fill",
            "TutorialDirectionalDodgeDoubleTapSeconds",
            "TryReadFirstLaunchTutorialDirectionalDodge",
            "TryRegisterFirstLaunchTutorialDirectionalDodge",
            "Tutorial Respawn Fade",
            "RETURNING TO CHECKPOINT...",
            "firstLaunchTutorialRespawnResetApplied",
            "ShouldReserveFirstLaunchTutorialPresentation",
            "FirstLaunchTutorialEntryPhase",
            "PLAY TUTORIAL",
            "SKIP TUTORIAL",
            "BuildFirstLaunchTutorialEntryChoice",
            "ConfirmFirstLaunchTutorialEntrySelection",
            "SetTutorialEntryTargetsActive",
            "InitializeLaunchPresentationGate",
            "ShouldReserveLaunchPresentation",
            "TickLaunchPresentationGate",
            "MainMenuEntryMode",
            "IntroToMainMenuTransition",
            "TryConsumeIntroToMainMenuTransition",
            "IsEligiblePostIntroLandingPage",
            "SmoothestStep01",
            "EvaluateNaturalCubicSplineComponent",
            "IntroMainMenuTotalSeconds = 4.40f",
            "IntroMainMenuEstablishSeconds = 0.55f",
            "IntroMainMenuDescentEndsAtSeconds = 2.10f",
            "IntroMainMenuAlignmentEndsAtSeconds = 3.35f",
            "BuildCinematicProductEnvironment",
            "Full 3D Tabletop",
            "Cinematic Floor",
            "Cinematic Cyclorama",
            "RuntimeInitializeLoadType.BeforeSceneLoad",
            "CreateFirstLaunchTutorialPixelText",
            "FirstLaunchTutorialPixelGlyphs",
            "COMPLETE THE CURRENT LESSON TO CONTINUE",
            "RejectFirstLaunchTutorialMountedMelee",
            "ON HORSE: RANGED ATTACKS ONLY",
            "TutorialSpinTargetX - 54f",
            "TutorialSpinTargetX + 54f",
            "TutorialGapX + 150f",
            "firstLaunchTutorialAmmo = 1",
            "FirstLaunchTutorialStep.ChargedShot",
            "TutorialChargedShotHoldThresholdSeconds = 0.22f",
            "FireFirstLaunchTutorialChargedShotAutomatically",
            "ResolveFirstLaunchTutorialRangedProjectileImpact",
            "BeginFirstLaunchTutorialShotTransaction",
            "CompleteFirstLaunchTutorialProductionHookTransaction",
            "ResolveFirstLaunchTutorialCollisionX",
            "HEAL THE HORSE BEFORE MOUNTING",
            "RegisterFirstLaunchTutorialWalkVisual",
            "ResolveFirstLaunchTutorialWalkPattern",
            "RECOVERY — ATTACK NOW",
            "PRESS INTERACT WHEN READY",
            "ApplyIntroToMainMenuCameraPose",
            "RestoreStaticIntroScenePose",
            "PrimeIntroToMainMenuFirstFrame",
            "introToMainMenuStartPosePrimed",
            "DeviceRealWorldScale = 0.16f",
            "DeviceRestScale",
            "deviceCamera.fieldOfView =",
            "ResolveRegularMainMenuFieldOfView();",
            "new Vector3(0f, -7.27f, -3.60f)",
            "Quaternion.Euler(90f, 0f, 0f)",
            "new Vector2(1.62f, 2.55f)",
            "TableEnvironmentWidth",
            "TableEnvironmentHeight",
            "advancesMountedShotLesson",
            "CompleteFirstLaunchTutorialMountedShotLessonAtImpact",
            "UpdateFirstLaunchTutorialCompletionContact"
        };

        public static void Scan(BDOneClickQAResult result)
        {
            if (result == null)
                return;

            string root = Directory.GetParent(Application.dataPath).FullName;
            List<string> errors = new List<string>();

            for (int index = 0; index < RequiredFiles.Length; index++)
            {
                string path = Path.Combine(root, RequiredFiles[index]);
                if (!File.Exists(path))
                    errors.Add("Missing first-launch tutorial file: " + RequiredFiles[index]);
            }

            string statePath = Path.Combine(
                root,
                "Assets/_Project/Scripts/Runtime/UI/BDFirstLaunchTutorialStateStore.cs"
            );
            string combatPath = Path.Combine(
                root,
                "Assets/_Project/Scripts/Runtime/BDPlayerCombat.cs"
            );
            string presenterPath = Path.Combine(
                root,
                "Assets/_Project/Scripts/Runtime/UI/BDModernHandheld3DPresenter.cs"
            );
            string tutorialPath = Path.Combine(
                root,
                "Assets/_Project/Scripts/Runtime/UI/" +
                "BDModernHandheld3DPresenter.FirstLaunchTutorial.cs"
            );
            string pixelPresentationPath = Path.Combine(
                root,
                "Assets/_Project/Scripts/Runtime/UI/" +
                "BDModernHandheld3DPresenter.FirstLaunchTutorial.PixelPresentation.cs"
            );
            string pixelTextPath = Path.Combine(
                root,
                "Assets/_Project/Scripts/Runtime/UI/" +
                "BDModernHandheld3DPresenter.FirstLaunchTutorial.PixelText.cs"
            );
            string gameplayPath = Path.Combine(
                root,
                "Assets/_Project/Scripts/Runtime/UI/" +
                "BDModernHandheld3DPresenter.FirstLaunchTutorial.Gameplay.cs"
            );
            string actionPresentationPath = Path.Combine(
                root,
                "Assets/_Project/Scripts/Runtime/UI/" +
                "BDModernHandheld3DPresenter.FirstLaunchTutorial.ActionPresentation.cs"
            );
            string productionCoursePath = Path.Combine(
                root,
                "Assets/_Project/Scripts/Runtime/UI/" +
                "BDModernHandheld3DPresenter.FirstLaunchTutorial.ProductionCourse.cs"
            );
            string v108RepairPath = Path.Combine(
                root,
                "Assets/_Project/Scripts/Runtime/UI/" +
                "BDModernHandheld3DPresenter.FirstLaunchTutorial.V108Repair.cs"
            );
            string targetPath = Path.Combine(
                root,
                "Assets/_Project/Scripts/Runtime/UI/BDModernHandheldControlTarget.cs"
            );
            string launchGatePath = Path.Combine(
                root,
                "Assets/_Project/Scripts/Runtime/UI/" +
                "BDModernHandheld3DPresenter.LaunchPresentationGate.cs"
            );
            string introMainMenuPath = Path.Combine(
                root,
                "Assets/_Project/Scripts/Runtime/UI/" +
                "BDModernHandheld3DPresenter.IntroToMainMenuTransition.cs"
            );
            string cinematicEnvironmentPath = Path.Combine(
                root,
                "Assets/_Project/Scripts/Runtime/UI/" +
                "BDModernHandheld3DPresenter.CinematicEnvironment.cs"
            );
            string bootIntroPath = Path.Combine(
                root,
                "Assets/_Project/Scripts/Runtime/UI/BDBBHBootIntro.cs"
            );

            string introMainMenu = ReadIfPresent(introMainMenuPath);
            string runtime = ReadIfPresent(statePath) +
                             ReadIfPresent(combatPath) +
                             ReadIfPresent(presenterPath) +
                             ReadIfPresent(tutorialPath) +
                             ReadIfPresent(pixelPresentationPath) +
                             ReadIfPresent(pixelTextPath) +
                             ReadIfPresent(gameplayPath) +
                             ReadIfPresent(actionPresentationPath) +
                             ReadIfPresent(productionCoursePath) +
                             ReadIfPresent(v108RepairPath) +
                             ReadIfPresent(targetPath) +
                             ReadIfPresent(launchGatePath) +
                             introMainMenu +
                             ReadIfPresent(cinematicEnvironmentPath) +
                             ReadIfPresent(bootIntroPath);

            for (int index = 0; index < RequiredRuntimeTokens.Length; index++)
            {
                string token = RequiredRuntimeTokens[index];
                if (runtime.IndexOf(token, StringComparison.Ordinal) < 0)
                    errors.Add("Missing first-launch tutorial contract token: " + token);
            }

            RequireRuntimeContract(
                runtime,
                errors,
                "Grapple handheld binding",
                "ResolveFirstLaunchTutorialHandheldBinding",
                "case \"GRAPPLE\":",
                "return \"HOLD Y\";"
            );
            RequireRuntimeContract(
                runtime,
                errors,
                "Ranged keyboard binding",
                "case \"RANGED\":",
                "return \"Q\";"
            );
            RequireRuntimeContract(
                runtime,
                errors,
                "Ranged handheld binding",
                "case \"RANGED\":",
                "return \"A\";"
            );
            RequireRuntimeContract(
                runtime,
                errors,
                "Jump keyboard binding",
                "case \"JUMP\":",
                "return \"SPACE\";"
            );
            RequireRuntimeContract(
                runtime,
                errors,
                "Jump handheld binding",
                "case \"JUMP\":",
                "return \"B\";"
            );
            RequireRuntimeContract(
                runtime,
                errors,
                "Directional dodge keyboard binding",
                "case \"DODGE\":",
                "return \"DOUBLE-TAP A/D OR LEFT/RIGHT\";"
            );
            RequireRuntimeContract(
                runtime,
                errors,
                "Directional dodge handheld binding",
                "case \"DODGE\":",
                "return \"DOUBLE-TAP D-PAD LEFT/RIGHT\";"
            );
            RequireRuntimeContract(
                runtime,
                errors,
                "Parry keyboard binding",
                "case \"PARRY\":",
                "return \"J / LEFT CLICK OR K / RIGHT CLICK\";"
            );
            RequireRuntimeContract(
                runtime,
                errors,
                "Parry handheld binding",
                "case \"PARRY\":",
                "return \"X OR Y\";"
            );
            RequireRuntimeContract(
                runtime,
                errors,
                "Controller binding family",
                "ResolveFirstLaunchTutorialGamepadBinding",
                "return \"A / SOUTH\";",
                "return \"RB\";",
                "return \"HOLD LB\";",
                "return \"X / WEST OR Y / NORTH\";"
            );
            RequireRuntimeContract(
                runtime,
                errors,
                "first-launch tutorial entry choice",
                "PLAY TUTORIAL",
                "SKIP TUTORIAL",
                "BDFirstLaunchTutorialStateStore.MarkInProgress();",
                "BDFirstLaunchTutorialStateStore.MarkSkipped();",
                "SetTutorialEntryTargetsActive"
            );
            RequireRuntimeContract(
                runtime,
                errors,
                "pixel entry typography",
                "CreateFirstLaunchTutorialPixelText",
                "FirstLaunchTutorialPixelGlyphs",
                "FilterMode.Point",
                "PLAY TUTORIAL",
                "SKIP TUTORIAL"
            );
            RequireRuntimeContract(
                runtime,
                errors,
                "invisible lesson clamp and forward progression",
                "COMPLETE THE CURRENT LESSON TO CONTINUE",
                "ResolveFirstLaunchTutorialMaximumPlayerX",
                "TutorialSpinTargetX - 54f",
                "TutorialSpinTargetX + 54f",
                "TutorialGapX + 150f",
                "firstLaunchTutorialAmmo = 1"
            );
            RequireRuntimeContract(
                runtime,
                errors,
                "mounted melee prohibition",
                "RejectFirstLaunchTutorialMountedMelee",
                "ON HORSE: RANGED ATTACKS ONLY",
                "firstLaunchTutorialPrimaryHoldStartedAt = -1f",
                "firstLaunchTutorialGrappleHoldStartedAt = -1f"
            );
            RequireRuntimeContract(
                runtime,
                errors,
                "first-frame modern presentation reservation",
                "RuntimeInitializeLoadType.BeforeSceneLoad",
                "InitializeLaunchPresentationGate",
                "ShouldReserveLaunchPresentation",
                "TickLaunchPresentationGate"
            );
            RequireRuntimeContract(
                runtime,
                errors,
                "camera-only full-set intro-to-main-menu cinematic",
                "MainMenuEntryMode.IntroToMainMenuTransition",
                "TryConsumeIntroToMainMenuTransition",
                "IsEligiblePostIntroLandingPage",
                "page == EffectivePage.MainMenu",
                "page == EffectivePage.FirstLaunchTutorial",
                "IntroMainMenuTotalSeconds = 4.40f",
                "IntroMainMenuEstablishSeconds = 0.55f",
                "IntroMainMenuDescentEndsAtSeconds = 2.10f",
                "IntroMainMenuAlignmentEndsAtSeconds = 3.35f",
                "PrepareNaturalCubicSpline",
                "EvaluateNaturalCubicSplineComponent",
                "SmoothestStep01",
                "BuildCinematicProductEnvironment",
                "Full 3D Tabletop",
                "Table Front Apron",
                "Table Front Left Leg",
                "Cinematic Floor",
                "Cinematic Cyclorama",
                "deviceCamera.transform.localPosition = cameraPosition",
                "tableRoot.localRotation = Quaternion.identity",
                "menuInputUnlockAt = float.PositiveInfinity",
                "PrimeIntroToMainMenuFirstFrame",
                "RegularMainMenuLookTarget",
                "new Vector3(0f, 1.50f, -3.19f)",
                "Mathf.Lerp(49f, 36.4f, fit)",
                "deviceVisualRoot.localScale = DeviceRestScale"
            );
            RequireRuntimeContract(
                runtime,
                errors,
                "charged-shot mechanic parity",
                "chargedShotHoldThreshold = 0.22f",
                "chargedShotBaseDuration = 0.90f",
                "chargedShotSecondsPerAdditionalAmmo = 0.45f",
                "chargedShotMaximumDuration = 3.20f",
                "FireChargedShot(direction);",
                "StartReloadImmediatelyAfterChargedShot();",
                "FirstLaunchTutorialStep.ChargedShot",
                "FireFirstLaunchTutorialChargedShotAutomatically"
            );
            RequireRuntimeContract(
                runtime,
                errors,
                "impact-synchronized tutorial transactions",
                "BeginFirstLaunchTutorialShotTransaction",
                "ResolveFirstLaunchTutorialRangedProjectileImpact",
                "advancesMountedShotLesson",
                "advancesLesson: advancesMountedShotLesson",
                "hitLivingTarget",
                "CompleteFirstLaunchTutorialMountedShotLessonAtImpact",
                "SetFirstLaunchTutorialStep(FirstLaunchTutorialStep.Reload)",
                "BeginFirstLaunchTutorialProductionHookTransaction",
                "CompleteFirstLaunchTutorialProductionHookTransaction",
                "progress >= 0.82f"
            );
            RequireRuntimeContract(
                runtime,
                errors,
                "injured horse and actor body collision",
                "HEAL THE HORSE BEFORE MOUNTING",
                "ResolveFirstLaunchTutorialCollisionX",
                "TutorialEnemyCollisionRadius",
                "TutorialMountedCollisionRadius"
            );
            RequireRuntimeContract(
                runtime,
                errors,
                "articulated pixel walk frames",
                "TutorialPixelWalkVisual",
                "ResolveFirstLaunchTutorialWalkPattern",
                "entry.StepA",
                "entry.StepB"
            );
            RequireRuntimeContract(
                runtime,
                errors,
                "readable final-test combat",
                "PRESS INTERACT WHEN READY",
                "WINDUP — MOVE / DODGE",
                "RECOVERY — ATTACK NOW",
                "firstLaunchTutorialEnemyProjectileTarget"
            );

            RequireRuntimeContract(
                runtime,
                errors,
                "tutorial entry title hierarchy spacing",
                "Tutorial Choice Brand",
                "292f",
                "Tutorial Choice Subtitle",
                "154f",
                "Play Tutorial Option",
                "20f"
            );
            RequireRuntimeContract(
                runtime,
                errors,
                "contextual instruction pacing",
                "SetFirstLaunchTutorialInstructionRequested",
                "TutorialInstructionTriggerRange"
            );
            RequireRuntimeContract(
                runtime,
                errors,
                "wide playable course",
                "TutorialHorseStartX = -650f",
                "TutorialAmbushTriggerX = -350f",
                "TutorialSecretBranchX = 3000f",
                "TutorialMiniBossStationX = 3600f",
                "TutorialCollectibleX = 3980f",
                "firstLaunchTutorialTravelDistance"
            );
            RequireRuntimeContract(
                runtime,
                errors,
                "mounted free-play",
                "firstLaunchTutorialMounted",
                "TutorialMountedMoveSpeed",
                "PlayFirstLaunchTutorialMountAnimation",
                "PlayFirstLaunchTutorialDismountAnimation"
            );
            RequireRuntimeContract(
                runtime,
                errors,
                "action animation family",
                "Tutorial Light Slash Effect",
                "Tutorial Muzzle Flash Effect",
                "Tutorial Player Projectile Effect",
                "Tutorial Dodge Trail Effect",
                "Tutorial Heavy Impact Effect",
                "Tutorial Spin Effect",
                "Tutorial Parry Effect",
                "Tutorial Grapple Line Effect",
                "Tutorial Healing Effect",
                "Tutorial Horse Hit Effect"
            );

            RequireRuntimeContract(
                runtime,
                errors,
                "production course progression",
                "FirstLaunchTutorialStep.Jump",
                "FirstLaunchTutorialStep.SidePath",
                "FirstLaunchTutorialStep.CombinedEncounter",
                "FirstLaunchTutorialStep.MiniBossPhaseOne",
                "FirstLaunchTutorialStep.MiniBossPhaseTwo",
                "FirstLaunchTutorialStep.MiniBossDefeated"
            );
            RequireRuntimeContract(
                runtime,
                errors,
                "checkpoint and reset cleanup",
                "ResetFirstLaunchTutorialToCheckpoint",
                "firstLaunchTutorialPrimaryHoldStartedAt = -1f",
                "firstLaunchTutorialGrappleHoldStartedAt = -1f",
                "firstLaunchTutorialReloadCompletesAt = 0f"
            );
            RequireRuntimeContract(
                runtime,
                errors,
                "active input presentation",
                "bool keyboardActive",
                "bool controllerActive",
                "bool handheldActive",
                "firstLaunchTutorialBindingDivider.gameObject.SetActive(false)"
            );

            string[] forbiddenIntroTransitionTokens =
            {
                "introToMainMenuDeviceStartPosition",
                "introToMainMenuDeviceStartRotation",
                "introToMainMenuShadowStartPosition",
                "introToMainMenuShadowStartRotation",
                "deviceVisualRoot.localPosition = devicePosition",
                "deviceVisualRoot.localScale = Vector3.one * deviceScale",
                "shadowRoot.localPosition = Vector3.Lerp",
                "ApplyIntroToMainMenuThreeDimensionalPose",
                "EvaluateCubicBezier",
                "SmootherStep01",
                "IntroMainMenuCinematicSeconds",
                "new Vector3(0f, 0.44f, -25.2f)",
                "new Vector3(0f, 1.50f, -14.0f)",
                "Mathf.Lerp(17f, 15f, fit)",
                "deviceVisualRoot.localScale = Vector3.Lerp",
            };

            for (int index = 0;
                 index < forbiddenIntroTransitionTokens.Length;
                 index++)
            {
                string token = forbiddenIntroTransitionTokens[index];
                if (introMainMenu.IndexOf(token, StringComparison.Ordinal) >= 0)
                {
                    errors.Add(
                        "Forbidden non-camera post-intro animation token: " +
                        token
                    );
                }
            }

            string[] forbiddenRuntimeTokens =
            {
                "entryProgress",
                "new Vector3(0f, -7.27f, 0f)",
                "Professional Blurred Wood Table",
                "Table Cinematic Vignette",
                "Tutorial Visible Lesson Gate",
                "UpdateFirstLaunchTutorialLessonGateVisual",
                "COMPLETE THE CURRENT LESSON TO OPEN THE GATE",
                "IntroMainMenuWideShotHoldSeconds",
                "IntroMainMenuTravelSeconds",
                "BDModernHandheldV6Polish",
                "BDModernHandheldTactileCompatibility",
                "BDModernHandheldPressScaleFeedback",
                "Time.timeScale = 0",
                "CancelPendingIntroToMainMenuTransitionForTutorialPlay",
                "BDBBHBootIntro.CancelPendingIntroToMainMenuTransition();",
                "HandleModernPrimaryAction(); // tutorial",
                "AddComponent<BDMainMenuFlow>",
                "case \"GRAPPLE\":\n" +
                "                    return \"Press the physical " +
                "SELECT button.\"",
                "V6Polish",
                "ResolveFirstLaunchTutorialDualBinding",
                "firstLaunchTutorialPrompt.resizeTextMaxSize = 38",
                "firstLaunchTutorialDetail.resizeTextMaxSize = 22",
                "Use four movement inputs in any direction.",
                "firstLaunchTutorialMovementInputs >= 4",
                "return \"Q\";\n                case \"SPIN\"",
                "firstLaunchTutorialJumpDemonstrated",
                "firstLaunchTutorialDodgeDemonstrated",
                "firstLaunchTutorialParryDemonstrated",
                "firstLaunchTutorialHazardDemonstrated",
                "firstLaunchTutorialMountedShotDemonstrated",
                "firstLaunchTutorialMountedImpactDemonstrated",
                "return \"W / UP\";",
                "return \"D-PAD UP\";",
                "return \"SPACE\";\n                case \"HEAVY\"",
                "private static bool ReadFirstLaunchTutorialDodgePressed()",
                "Keyboard.current.wKey.wasPressedThisFrame ||\n                 Keyboard.current.upArrowKey.wasPressedThisFrame",
                "action == BDModernHandheldControlTarget.ControlAction.DPadUp &&\n                !firstLaunchTutorialMounted",
                "TutorialEnemyRole.Small, 1050f, 1f",
                "TutorialEnemyRole.Small, 1120f, 1f",
                "TutorialEnemyRole.Small, 1370f, 2f",
                "new Vector3(0f, 0.28f, 0f)",
                "Quaternion.Euler(9.4f, 0f, 0f)",
                "new Vector2(10.9f, 3.85f)",
                "new Vector2(8.55f, 1.42f)"
            };

            for (int index = 0; index < forbiddenRuntimeTokens.Length; index++)
            {
                string token = forbiddenRuntimeTokens[index];
                if (runtime.IndexOf(token, StringComparison.Ordinal) >= 0)
                    errors.Add("Forbidden tutorial/handheld implementation token: " + token);
            }

            string[] retiredFiles =
            {
                "Assets/_Project/Scripts/Runtime/UI/BDModernHandheldV6Polish.cs",
                "Assets/_Project/Scripts/Runtime/UI/BDModernHandheldTactileCompatibility.cs",
                "Assets/_Project/Scripts/Runtime/UI/BDModernHandheldPressScaleFeedback.cs",
                "Assets/_Project/Scripts/Editor/Validation/BDModernHandheldV6QA.cs"
            };

            for (int index = 0; index < retiredFiles.Length; index++)
            {
                if (File.Exists(Path.Combine(root, retiredFiles[index])))
                    errors.Add("Retired compatibility layer still exists: " + retiredFiles[index]);
            }

            if (errors.Count == 0)
                return;

            result.findings.Add(
                new BDOneClickQAFinding(
                    BDOneClickQASeverity.Blocker,
                    "FIRST_LAUNCH_TUTORIAL_CONTRACT_INVALID",
                    string.Empty,
                    string.Empty,
                    string.Join("\n", errors)
                )
            );
        }

        private static void RequireRuntimeContract(
            string runtime,
            List<string> errors,
            string contractName,
            params string[] requiredTokens)
        {
            for (int index = 0; index < requiredTokens.Length; index++)
            {
                string token = requiredTokens[index];
                if (runtime.IndexOf(token, StringComparison.Ordinal) >= 0)
                    continue;

                errors.Add(
                    "Missing first-launch tutorial " +
                    contractName +
                    " token: " +
                    token
                );
            }
        }

        private static string ReadIfPresent(string path)
        {
            return File.Exists(path)
                ? File.ReadAllText(path)
                : string.Empty;
        }
    }
}
#endif
