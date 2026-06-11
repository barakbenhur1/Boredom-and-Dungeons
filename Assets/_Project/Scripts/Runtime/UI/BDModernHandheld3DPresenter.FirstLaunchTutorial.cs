using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace BoredomAndDungeons
{
    public sealed partial class BDModernHandheld3DPresenter
    {
        private enum FirstLaunchTutorialStep
        {
            WhiteBoot,
            Move,
            Jump,
            JumpAttack,
            WallJump,
            MountHorse,
            RideHorse,
            EnemyArrival,
            HorseShot,
            AttackEnemy,
            HeavyAttack,
            Dodge,
            Parry,
            HorseReturn,
            HealHorse,
            RemountHorse,
            SpinAttack,
            Grapple,
            HazardKnockback,
            RangedAttack,
            Reload,
            ChargedShot,
            MountedImpact,
            DismountHorse,
            SidePath,
            CombinedEncounter,
            MiniBossIntro,
            MiniBossPhaseOne,
            MiniBossPhaseTwo,
            MiniBossDefeated,
            Collectible,
            Completed
        }

        private enum FirstLaunchTutorialInputSource
        {
            Keyboard,
            Gamepad,
            Handheld,
            Touch
        }

        private enum FirstLaunchTutorialEntryPhase
        {
            Choice,
            StartingTutorial,
            Playing,
            Skipping
        }

        private const int TutorialContinueTargetIndex = 100;
        private const int TutorialLeaveTargetIndex = 101;
        private const int TutorialPlayTargetIndex = 102;
        private const int TutorialSkipTargetIndex = 103;
        private const float TutorialExitInputGuardSeconds = 0.20f;
        private const float TutorialEntryConfirmSeconds = 0.28f;
        private const float TutorialTransitionSeconds = 0.52f;
        private const float TutorialSpinHoldSeconds = 0.78f;
        private const float TutorialHealHoldSeconds = 1.10f;
        private const float TutorialGrappleHoldSeconds = 0.72f;
        private const float TutorialChargedShotHoldThresholdSeconds = 0.22f;
        private const float TutorialChargedShotBaseSeconds = 0.90f;
        private const float TutorialChargedShotSecondsPerAdditionalRound = 0.45f;
        private const float TutorialChargedShotMaximumSeconds = 3.20f;
        private const float TutorialDirectionalDodgeDoubleTapSeconds = 0.30f;

        private bool firstLaunchTutorialInitialized;
        private bool firstLaunchTutorialActive;
        private bool firstLaunchTutorialFinishedThisSession;
        private bool firstLaunchTutorialExitOpen;
        private bool firstLaunchTutorialTransitionOut;
        private int firstLaunchTutorialExitSelection;
        private float firstLaunchTutorialStepStartedAt;
        private float firstLaunchTutorialInputUnlockAt;
        private float firstLaunchTutorialTransitionStartedAt;
        private float firstLaunchTutorialPrimaryHoldStartedAt = -1f;
        private float firstLaunchTutorialHealHoldStartedAt = -1f;
        private float firstLaunchTutorialGrappleHoldStartedAt = -1f;
        private float firstLaunchTutorialChargedShotPendingStartedAt = -1f;
        private float firstLaunchTutorialChargedShotStartedAt = -1f;
        private float firstLaunchTutorialFeedbackClearAt;
        private float firstLaunchTutorialPauseStartedAt = -1f;
        private float firstLaunchTutorialExitOpenedAt;
        private float firstLaunchTutorialLastLeftTapAt = -999f;
        private float firstLaunchTutorialLastRightTapAt = -999f;
        private int firstLaunchTutorialLastActionFrame = -1;
        private FirstLaunchTutorialStep firstLaunchTutorialStep;
        private FirstLaunchTutorialInputSource firstLaunchTutorialInputSource;
        private FirstLaunchTutorialEntryPhase firstLaunchTutorialEntryPhase;
        private int firstLaunchTutorialEntrySelection;
        private float firstLaunchTutorialEntryTransitionStartedAt;
        private int firstLaunchTutorialEntryLastNavigationFrame = -1;

        private GameObject firstLaunchTutorialGameplayRoot;
        private GameObject firstLaunchTutorialEntryRoot;
        private CanvasGroup firstLaunchTutorialEntryCanvasGroup;
        private Image firstLaunchTutorialPlayOption;
        private Image firstLaunchTutorialSkipOption;
        private Image firstLaunchTutorialPlayLabel;
        private Image firstLaunchTutorialSkipLabel;
        private Image firstLaunchTutorialEntryStatus;
        private Image firstLaunchTutorialWhiteOverlay;
        private Image firstLaunchTutorialRespawnOverlay;
        private Text firstLaunchTutorialRespawnLabel;
        private Image firstLaunchTutorialWorldPanel;
        private Image firstLaunchTutorialPlayer;
        private Image firstLaunchTutorialHorse;
        private Image firstLaunchTutorialEnemy;
        private Image firstLaunchTutorialHazard;
        private Image firstLaunchTutorialProjectile;
        private Image firstLaunchTutorialCollectible;
        private Image firstLaunchTutorialGap;
        private Image firstLaunchTutorialExitDimmer;
        private GameObject firstLaunchTutorialExitPanel;
        private Image firstLaunchTutorialContinueOption;
        private Image firstLaunchTutorialLeaveOption;
        private Image firstLaunchTutorialInstructionPanel;
        private Image firstLaunchTutorialInstructionAccent;
        private CanvasGroup firstLaunchTutorialInstructionCanvasGroup;
        private RectTransform firstLaunchTutorialInstructionRect;
        private Text firstLaunchTutorialPrompt;
        private Text firstLaunchTutorialDetail;
        private Text firstLaunchTutorialProgress;
        private Text firstLaunchTutorialFeedback;
        private Image firstLaunchTutorialKeyboardBindingCard;
        private Image firstLaunchTutorialHandheldBindingCard;
        private Text firstLaunchTutorialKeyboardBindingTitle;
        private Text firstLaunchTutorialHandheldBindingTitle;
        private Text firstLaunchTutorialKeyboardBinding;
        private Text firstLaunchTutorialHandheldBinding;
        private Text firstLaunchTutorialBindingDivider;
        private Image firstLaunchTutorialHoldFill;

        private float firstLaunchTutorialInstructionStartedAt;
        private readonly List<Texture2D> firstLaunchTutorialPixelTextures =
            new List<Texture2D>();
        private readonly List<Sprite> firstLaunchTutorialPixelSprites =
            new List<Sprite>();

        private void InitializeFirstLaunchTutorial()
        {
            firstLaunchTutorialInitialized = true;
            firstLaunchTutorialFinishedThisSession = false;
            firstLaunchTutorialActive = false;
            firstLaunchTutorialExitOpen = false;
            firstLaunchTutorialTransitionOut = false;
            firstLaunchTutorialEntryPhase = FirstLaunchTutorialEntryPhase.Choice;
            firstLaunchTutorialEntrySelection = 0;
            firstLaunchTutorialEntryTransitionStartedAt = 0f;
            firstLaunchTutorialEntryLastNavigationFrame = -1;
            firstLaunchTutorialLastLeftTapAt = -999f;
            firstLaunchTutorialLastRightTapAt = -999f;
        }

        private void DisposeFirstLaunchTutorial()
        {
            DisposeFirstLaunchTutorialActionPresentation();
            DisposeFirstLaunchTutorialFreePlayCourse();
            DisposeFirstLaunchTutorialPixelAssets();
            ClearFirstLaunchTutorialPhysicalHighlight();
            SetTutorialEntryTargetsActive(false);
            SetTutorialExitTargetsActive(false);
            firstLaunchTutorialActive = false;
            firstLaunchTutorialExitOpen = false;
            firstLaunchTutorialTransitionOut = false;
            firstLaunchTutorialGameplayRoot = null;
            firstLaunchTutorialEntryRoot = null;
            firstLaunchTutorialEntryCanvasGroup = null;
            firstLaunchTutorialPlayOption = null;
            firstLaunchTutorialSkipOption = null;
            firstLaunchTutorialPlayLabel = null;
            firstLaunchTutorialSkipLabel = null;
            firstLaunchTutorialEntryStatus = null;
            firstLaunchTutorialWhiteOverlay = null;
            firstLaunchTutorialRespawnOverlay = null;
            firstLaunchTutorialRespawnLabel = null;
        }

        private void ResetFirstLaunchTutorialForScene()
        {
            DisposeFirstLaunchTutorialActionPresentation();
            DisposeFirstLaunchTutorialFreePlayCourse();
            DisposeFirstLaunchTutorialPixelAssets();
            ClearFirstLaunchTutorialPhysicalHighlight();
            SetTutorialEntryTargetsActive(false);
            SetTutorialExitTargetsActive(false);
            firstLaunchTutorialActive = false;
            firstLaunchTutorialExitOpen = false;
            firstLaunchTutorialTransitionOut = false;
            firstLaunchTutorialPrimaryHoldStartedAt = -1f;
            firstLaunchTutorialHealHoldStartedAt = -1f;
            firstLaunchTutorialGrappleHoldStartedAt = -1f;
            firstLaunchTutorialChargedShotPendingStartedAt = -1f;
            firstLaunchTutorialChargedShotStartedAt = -1f;
            firstLaunchTutorialPauseStartedAt = -1f;
            firstLaunchTutorialLastLeftTapAt = -999f;
            firstLaunchTutorialLastRightTapAt = -999f;
            firstLaunchTutorialLastActionFrame = -1;
            firstLaunchTutorialEntryPhase = FirstLaunchTutorialEntryPhase.Choice;
            firstLaunchTutorialEntrySelection = 0;
            firstLaunchTutorialEntryTransitionStartedAt = 0f;
            firstLaunchTutorialEntryLastNavigationFrame = -1;
            firstLaunchTutorialGameplayRoot = null;
            firstLaunchTutorialEntryRoot = null;
            firstLaunchTutorialEntryCanvasGroup = null;
            firstLaunchTutorialPlayOption = null;
            firstLaunchTutorialSkipOption = null;
            firstLaunchTutorialPlayLabel = null;
            firstLaunchTutorialSkipLabel = null;
            firstLaunchTutorialEntryStatus = null;
            firstLaunchTutorialWhiteOverlay = null;
            firstLaunchTutorialRespawnOverlay = null;
            firstLaunchTutorialRespawnLabel = null;
        }

        private bool ShouldReserveFirstLaunchTutorialPresentation()
        {
            return firstLaunchTutorialInitialized &&
                   !firstLaunchTutorialFinishedThisSession &&
                   BDFirstLaunchTutorialStateStore.ShouldPresent;
        }

        private bool ShouldPresentFirstLaunchTutorial()
        {
            if (!ShouldReserveFirstLaunchTutorialPresentation())
                return false;

            // Reserve the tutorial surface while BDMainMenuFlow is still being
            // resolved. This prevents the legacy menu from receiving one visible
            // frame between the BBH intro and the modern handheld presentation.
            if (flow == null)
                return true;

            return !flow.IsRunActive &&
                   !flow.IsPausedFromGameplay &&
                   flow.CurrentHandheldPage ==
                       BDMainMenuFlow.HandheldPage.MainMenu;
        }

        private void BuildFirstLaunchTutorialPage()
        {
            DisposeFirstLaunchTutorialPixelAssets();
            firstLaunchTutorialActive = true;
            firstLaunchTutorialExitOpen = false;
            firstLaunchTutorialTransitionOut = false;
            firstLaunchTutorialExitSelection = 0;
            firstLaunchTutorialPrimaryHoldStartedAt = -1f;
            firstLaunchTutorialHealHoldStartedAt = -1f;
            firstLaunchTutorialGrappleHoldStartedAt = -1f;
            firstLaunchTutorialChargedShotPendingStartedAt = -1f;
            firstLaunchTutorialChargedShotStartedAt = -1f;
            firstLaunchTutorialPauseStartedAt = -1f;
            firstLaunchTutorialLastLeftTapAt = -999f;
            firstLaunchTutorialLastRightTapAt = -999f;
            firstLaunchTutorialLastActionFrame = -1;
            firstLaunchTutorialInputSource =
                FirstLaunchTutorialInputSource.Keyboard;

            Image rootPanel = CreatePanel(
                pageRoot,
                "First Launch Tutorial Root",
                0f,
                -10f,
                900f,
                850f,
                new Color(0.018f, 0.020f, 0.055f, 1f)
            );
            firstLaunchTutorialGameplayRoot = rootPanel.gameObject;
            AddOutline(
                rootPanel.gameObject,
                new Color(0.18f, 0.30f, 0.46f, 0.52f),
                2f
            );

            CreateText(
                rootPanel.rectTransform,
                "Tutorial Title",
                "FIRST ADVENTURE",
                0f,
                390f,
                760f,
                44f,
                28,
                TextAnchor.MiddleCenter,
                new Color(1f, 0.78f, 0.30f, 1f),
                FontStyle.Bold
            );

            firstLaunchTutorialProgress = CreateText(
                rootPanel.rectTransform,
                "Tutorial Progress",
                string.Empty,
                0f,
                352f,
                760f,
                22f,
                12,
                TextAnchor.MiddleCenter,
                new Color(0.38f, 0.76f, 1f, 1f),
                FontStyle.Bold
            );

            firstLaunchTutorialWorldPanel = CreatePanel(
                rootPanel.rectTransform,
                "Tutorial World",
                0f,
                78f,
                820f,
                390f,
                new Color(0.055f, 0.055f, 0.16f, 1f)
            );
            AddOutline(
                firstLaunchTutorialWorldPanel.gameObject,
                new Color(0.30f, 0.56f, 0.68f, 0.54f),
                2f
            );
            BuildFirstLaunchTutorialPixelBackdrop(
                firstLaunchTutorialWorldPanel.rectTransform
            );

            CreatePanel(
                firstLaunchTutorialWorldPanel.rectTransform,
                "Tutorial Ground",
                0f,
                -154f,
                820f,
                82f,
                new Color(0.035f, 0.13f, 0.14f, 1f)
            );
            CreatePanel(
                firstLaunchTutorialWorldPanel.rectTransform,
                "Tutorial Path",
                0f,
                -128f,
                640f,
                48f,
                new Color(0.48f, 0.29f, 0.13f, 1f)
            );

            firstLaunchTutorialPlayer = CreateTutorialEntity(
                firstLaunchTutorialWorldPanel.rectTransform,
                "Tutorial Player",
                -210f,
                -108f,
                49f,
                77f,
                new Color(0.20f, 0.62f, 1f, 1f)
            );
            firstLaunchTutorialHorse = CreateTutorialEntity(
                firstLaunchTutorialWorldPanel.rectTransform,
                "Tutorial Horse",
                -100f,
                -116f,
                112f,
                64f,
                new Color(0.58f, 0.35f, 0.18f, 1f)
            );
            firstLaunchTutorialEnemy = CreateTutorialEntity(
                firstLaunchTutorialWorldPanel.rectTransform,
                "Tutorial Enemy",
                300f,
                -108f,
                49f,
                77f,
                new Color(0.72f, 0.12f, 0.20f, 1f)
            );
            firstLaunchTutorialHazard = CreateTutorialEntity(
                firstLaunchTutorialWorldPanel.rectTransform,
                "Tutorial Hazard",
                100f,
                -142f,
                96f,
                32f,
                new Color(0.92f, 0.38f, 0.06f, 1f)
            );
            firstLaunchTutorialProjectile = CreateTutorialEntity(
                firstLaunchTutorialWorldPanel.rectTransform,
                "Tutorial Projectile",
                280f,
                -66f,
                30f,
                18f,
                new Color(1f, 0.82f, 0.20f, 1f)
            );
            firstLaunchTutorialCollectible = CreateTutorialEntity(
                firstLaunchTutorialWorldPanel.rectTransform,
                "Tutorial Collectible",
                250f,
                -88f,
                40f,
                56f,
                new Color(0.22f, 0.94f, 0.78f, 1f)
            );
            firstLaunchTutorialGap = CreateTutorialEntity(
                firstLaunchTutorialWorldPanel.rectTransform,
                "Tutorial Gap",
                105f,
                -160f,
                180f,
                64f,
                new Color(0.004f, 0.006f, 0.012f, 1f)
            );

            Image instructionShadow = CreatePanel(
                rootPanel.rectTransform,
                "Tutorial Instruction Shadow",
                0f,
                -274f,
                838f,
                244f,
                new Color(0f, 0f, 0f, 0.52f)
            );
            instructionShadow.raycastTarget = false;

            firstLaunchTutorialInstructionPanel = CreatePanel(
                rootPanel.rectTransform,
                "Tutorial Instruction Panel",
                0f,
                -266f,
                820f,
                230f,
                new Color(0.025f, 0.038f, 0.070f, 0.992f)
            );
            firstLaunchTutorialInstructionPanel.raycastTarget = false;
            AddOutline(
                firstLaunchTutorialInstructionPanel.gameObject,
                new Color(0.30f, 0.74f, 1f, 0.96f),
                3f
            );
            firstLaunchTutorialInstructionRect =
                firstLaunchTutorialInstructionPanel.rectTransform;
            firstLaunchTutorialInstructionCanvasGroup =
                firstLaunchTutorialInstructionPanel.gameObject
                    .AddComponent<CanvasGroup>();

            firstLaunchTutorialInstructionAccent = CreatePanel(
                firstLaunchTutorialInstructionRect,
                "Tutorial Instruction Accent",
                0f,
                111f,
                820f,
                8f,
                new Color(0.18f, 0.76f, 1f, 1f)
            );
            firstLaunchTutorialInstructionAccent.raycastTarget = false;

            firstLaunchTutorialPrompt = CreateText(
                firstLaunchTutorialInstructionRect,
                "Tutorial Prompt",
                string.Empty,
                0f,
                68f,
                760f,
                58f,
                44,
                TextAnchor.MiddleCenter,
                Color.white,
                FontStyle.Bold
            );
            firstLaunchTutorialPrompt.resizeTextForBestFit = true;
            firstLaunchTutorialPrompt.resizeTextMinSize = 34;
            firstLaunchTutorialPrompt.resizeTextMaxSize = 44;

            firstLaunchTutorialDetail = CreateText(
                firstLaunchTutorialInstructionRect,
                "Tutorial Detail",
                string.Empty,
                0f,
                25f,
                748f,
                32f,
                20,
                TextAnchor.MiddleCenter,
                new Color(0.76f, 0.86f, 0.95f, 1f),
                FontStyle.Normal
            );
            firstLaunchTutorialDetail.resizeTextForBestFit = true;
            firstLaunchTutorialDetail.resizeTextMinSize = 16;
            firstLaunchTutorialDetail.resizeTextMaxSize = 20;

            firstLaunchTutorialKeyboardBindingCard =
                CreateFirstLaunchTutorialBindingCard(
                    firstLaunchTutorialInstructionRect,
                    "Tutorial Keyboard Binding Card",
                    "KEYBOARD / MOUSE",
                    -205f,
                    new Color(0.055f, 0.105f, 0.145f, 1f),
                    out firstLaunchTutorialKeyboardBindingTitle,
                    out firstLaunchTutorialKeyboardBinding
                );
            firstLaunchTutorialHandheldBindingCard =
                CreateFirstLaunchTutorialBindingCard(
                    firstLaunchTutorialInstructionRect,
                    "Tutorial Handheld Binding Card",
                    "HANDHELD",
                    205f,
                    new Color(0.105f, 0.070f, 0.155f, 1f),
                    out firstLaunchTutorialHandheldBindingTitle,
                    out firstLaunchTutorialHandheldBinding
                );
            firstLaunchTutorialBindingDivider = CreateText(
                firstLaunchTutorialInstructionRect,
                "Tutorial Binding Divider",
                "OR",
                0f,
                -59f,
                42f,
                42f,
                15,
                TextAnchor.MiddleCenter,
                new Color(0.62f, 0.72f, 0.82f, 1f),
                FontStyle.Bold
            );

            firstLaunchTutorialHoldFill = CreatePanel(
                firstLaunchTutorialInstructionRect,
                "Tutorial Hold Progress Fill",
                -360f,
                -108f,
                0f,
                8f,
                new Color(0.30f, 0.86f, 1f, 1f)
            );
            firstLaunchTutorialHoldFill.rectTransform.pivot =
                new Vector2(0f, 0.5f);
            firstLaunchTutorialHoldFill.gameObject.SetActive(false);
            firstLaunchTutorialHoldFill.raycastTarget = false;

            firstLaunchTutorialFeedback = CreateText(
                rootPanel.rectTransform,
                "Tutorial Feedback",
                string.Empty,
                0f,
                310f,
                760f,
                24f,
                16,
                TextAnchor.MiddleCenter,
                new Color(0.48f, 1f, 0.70f, 1f),
                FontStyle.Bold
            );

            BuildFirstLaunchTutorialExitConfirmation(rootPanel.rectTransform);

            firstLaunchTutorialRespawnOverlay = CreatePanel(
                pageRoot,
                "Tutorial Respawn Fade",
                0f,
                0f,
                CanvasSize.x,
                CanvasSize.y,
                new Color(0.003f, 0.006f, 0.014f, 0f)
            );
            firstLaunchTutorialRespawnOverlay.raycastTarget = false;
            firstLaunchTutorialRespawnOverlay.gameObject.SetActive(false);
            firstLaunchTutorialRespawnLabel = CreateText(
                firstLaunchTutorialRespawnOverlay.rectTransform,
                "Tutorial Respawn Label",
                "RETURNING TO CHECKPOINT...",
                0f,
                -8f,
                720f,
                46f,
                22,
                TextAnchor.MiddleCenter,
                new Color(0.78f, 0.90f, 1f, 0f),
                FontStyle.Bold
            );
            firstLaunchTutorialRespawnLabel.raycastTarget = false;

            firstLaunchTutorialWhiteOverlay = CreatePanel(
                pageRoot,
                "Tutorial Dark Transition Overlay",
                0f,
                0f,
                CanvasSize.x,
                CanvasSize.y,
                new Color(0.003f, 0.006f, 0.014f, 1f)
            );
            firstLaunchTutorialWhiteOverlay.transform.SetAsLastSibling();
            firstLaunchTutorialWhiteOverlay.gameObject.SetActive(false);

            InitializeFirstLaunchTutorialFreePlayCourse();
            SetFirstLaunchTutorialStep(FirstLaunchTutorialStep.Move);
            SetTutorialExitTargetsActive(false);
            BuildFirstLaunchTutorialEntryChoice();
            SetFirstLaunchTutorialEntryChoiceActive(true);
        }

        private void BuildFirstLaunchTutorialEntryChoice()
        {
            Image background = CreatePanel(
                pageRoot,
                "First Launch Tutorial Choice",
                0f,
                0f,
                CanvasSize.x,
                CanvasSize.y,
                Color.black
            );
            firstLaunchTutorialEntryRoot = background.gameObject;
            firstLaunchTutorialEntryCanvasGroup =
                background.gameObject.AddComponent<CanvasGroup>();

            CreateFirstLaunchTutorialPixelText(
                background.rectTransform,
                "Tutorial Choice Brand",
                "B&D",
                0f,
                292f,
                760f,
                136f,
                Color.white,
                8,
                3
            );
            CreateFirstLaunchTutorialPixelText(
                background.rectTransform,
                "Tutorial Choice Subtitle",
                "Boredom & Dungeons",
                0f,
                154f,
                760f,
                56f,
                Color.white,
                4,
                2
            );

            firstLaunchTutorialPlayOption = CreatePanel(
                background.rectTransform,
                "Play Tutorial Option",
                0f,
                20f,
                600f,
                88f,
                Color.black
            );
            firstLaunchTutorialSkipOption = CreatePanel(
                background.rectTransform,
                "Skip Tutorial Option",
                0f,
                -96f,
                600f,
                88f,
                Color.black
            );

            firstLaunchTutorialPlayLabel =
                CreateFirstLaunchTutorialPixelText(
                    firstLaunchTutorialPlayOption.rectTransform,
                    "Play Tutorial Label",
                    "PLAY TUTORIAL",
                    0f,
                    0f,
                    540f,
                    64f,
                    Color.white,
                    4,
                    2
                );
            firstLaunchTutorialSkipLabel =
                CreateFirstLaunchTutorialPixelText(
                    firstLaunchTutorialSkipOption.rectTransform,
                    "Skip Tutorial Label",
                    "SKIP TUTORIAL",
                    0f,
                    0f,
                    540f,
                    64f,
                    Color.white,
                    4,
                    2
                );

            firstLaunchTutorialEntryStatus =
                CreateFirstLaunchTutorialPixelText(
                    background.rectTransform,
                    "Tutorial Choice Status",
                    "UP / DOWN TO CHOOSE   SELECT TO CONFIRM",
                    0f,
                    -260f,
                    760f,
                    52f,
                    new Color(0.86f, 0.90f, 0.94f, 1f),
                    2,
                    1
                );

            AddOutline(
                firstLaunchTutorialPlayOption.gameObject,
                Color.white,
                3f
            );
            AddOutline(
                firstLaunchTutorialSkipOption.gameObject,
                Color.white,
                3f
            );

            background.transform.SetAsLastSibling();
            RefreshFirstLaunchTutorialEntryChoiceVisuals();
        }

        private void SetFirstLaunchTutorialEntryChoiceActive(bool active)
        {
            firstLaunchTutorialEntryPhase = active
                ? FirstLaunchTutorialEntryPhase.Choice
                : FirstLaunchTutorialEntryPhase.Playing;
            firstLaunchTutorialEntrySelection = 0;
            firstLaunchTutorialEntryTransitionStartedAt = 0f;
            firstLaunchTutorialEntryLastNavigationFrame = -1;

            if (firstLaunchTutorialGameplayRoot != null)
                firstLaunchTutorialGameplayRoot.SetActive(!active);
            if (firstLaunchTutorialEntryRoot != null)
            {
                firstLaunchTutorialEntryRoot.SetActive(active);
                firstLaunchTutorialEntryRoot.transform.SetAsLastSibling();
            }
            if (firstLaunchTutorialEntryCanvasGroup != null)
                firstLaunchTutorialEntryCanvasGroup.alpha = 1f;

            SetTutorialEntryTargetsActive(active);
            RefreshFirstLaunchTutorialEntryChoiceVisuals();
            if (active)
            {
                SetFirstLaunchTutorialPhysicalHighlight(
                    BDModernHandheldControlTarget.ControlAction.Confirm
                );
            }
        }

        private void SetFirstLaunchTutorialEntrySelection(int selection)
        {
            int clamped = Mathf.Clamp(selection, 0, 1);
            if (clamped == firstLaunchTutorialEntrySelection)
                return;

            firstLaunchTutorialEntrySelection = clamped;
            PlayClick();
            RefreshFirstLaunchTutorialEntryChoiceVisuals();
        }

        private void RefreshFirstLaunchTutorialEntryChoiceVisuals()
        {
            bool playSelected = firstLaunchTutorialEntrySelection == 0;
            ApplyFirstLaunchTutorialEntryOptionVisual(
                firstLaunchTutorialPlayOption,
                firstLaunchTutorialPlayLabel,
                playSelected
            );
            ApplyFirstLaunchTutorialEntryOptionVisual(
                firstLaunchTutorialSkipOption,
                firstLaunchTutorialSkipLabel,
                !playSelected
            );
        }

        private static void ApplyFirstLaunchTutorialEntryOptionVisual(
            Image option,
            Image label,
            bool selected)
        {
            if (option != null)
            {
                option.color = selected ? Color.white : Color.black;
                option.rectTransform.localScale = selected
                    ? new Vector3(1.025f, 1.025f, 1f)
                    : Vector3.one;
            }
            if (label != null)
                label.color = selected ? Color.black : Color.white;
        }

        private void ConfirmFirstLaunchTutorialEntrySelection()
        {
            if (firstLaunchTutorialEntryPhase !=
                    FirstLaunchTutorialEntryPhase.Choice ||
                Time.frameCount == firstLaunchTutorialEntryLastNavigationFrame)
            {
                return;
            }

            firstLaunchTutorialEntryLastNavigationFrame = Time.frameCount;
            firstLaunchTutorialEntryTransitionStartedAt = Time.unscaledTime;
            PlayClick();

            if (firstLaunchTutorialEntrySelection == 0)
            {
                BDFirstLaunchTutorialStateStore.MarkInProgress();
                firstLaunchTutorialEntryPhase =
                    FirstLaunchTutorialEntryPhase.StartingTutorial;
                SetFirstLaunchTutorialPixelText(
                    firstLaunchTutorialEntryStatus,
                    "LOADING FIRST ADVENTURE..."
                );
            }
            else
            {
                BDFirstLaunchTutorialStateStore.MarkSkipped();
                firstLaunchTutorialEntryPhase =
                    FirstLaunchTutorialEntryPhase.Skipping;
                SetFirstLaunchTutorialPixelText(
                    firstLaunchTutorialEntryStatus,
                    "TUTORIAL SKIPPED"
                );
            }

            SetTutorialEntryTargetsActive(false);
            ClearFirstLaunchTutorialPhysicalHighlight();
        }

        private bool UpdateFirstLaunchTutorialEntryChoice()
        {
            if (firstLaunchTutorialEntryPhase ==
                    FirstLaunchTutorialEntryPhase.Playing ||
                (firstLaunchTutorialEntryPhase ==
                     FirstLaunchTutorialEntryPhase.Skipping &&
                 firstLaunchTutorialTransitionOut))
            {
                return false;
            }

            if (firstLaunchTutorialEntryPhase ==
                    FirstLaunchTutorialEntryPhase.Choice)
            {
                if (firstLaunchTutorialEntryRoot != null)
                    firstLaunchTutorialEntryRoot.transform.SetAsLastSibling();

                float pulse =
                    1f + Mathf.Sin(Time.unscaledTime * 5f) * 0.012f;
                Image selected = firstLaunchTutorialEntrySelection == 0
                    ? firstLaunchTutorialPlayOption
                    : firstLaunchTutorialSkipOption;
                if (selected != null)
                {
                    selected.rectTransform.localScale =
                        new Vector3(pulse, pulse, 1f);
                }
                return true;
            }

            float elapsed =
                Time.unscaledTime -
                firstLaunchTutorialEntryTransitionStartedAt;
            float progress = Mathf.Clamp01(
                elapsed / TutorialEntryConfirmSeconds
            );
            if (firstLaunchTutorialEntryCanvasGroup != null)
            {
                firstLaunchTutorialEntryCanvasGroup.alpha =
                    1f - Mathf.SmoothStep(0f, 1f, progress);
            }

            if (progress < 1f)
                return true;

            if (firstLaunchTutorialEntryPhase ==
                    FirstLaunchTutorialEntryPhase.StartingTutorial)
            {
                if (firstLaunchTutorialEntryRoot != null)
                    firstLaunchTutorialEntryRoot.SetActive(false);
                if (firstLaunchTutorialGameplayRoot != null)
                    firstLaunchTutorialGameplayRoot.SetActive(true);
                if (firstLaunchTutorialWhiteOverlay != null)
                    firstLaunchTutorialWhiteOverlay.gameObject.SetActive(false);

                firstLaunchTutorialEntryPhase =
                    FirstLaunchTutorialEntryPhase.Playing;
                firstLaunchTutorialInputUnlockAt =
                    Time.unscaledTime + 0.18f;
                firstLaunchTutorialStepStartedAt = Time.unscaledTime;
                UpdateFirstLaunchTutorialPrompt();
                return false;
            }

            if (!firstLaunchTutorialTransitionOut)
                BeginFirstLaunchTutorialTransition(skip: true);
            return false;
        }

        private void UpdateFirstLaunchTutorialEntryNavigationInput()
        {
            if (firstLaunchTutorialEntryPhase !=
                    FirstLaunchTutorialEntryPhase.Choice)
            {
                return;
            }

            bool up = ReadFirstLaunchTutorialEntryUpPressed();
            bool down = ReadFirstLaunchTutorialEntryDownPressed();
            if (up)
                SetFirstLaunchTutorialEntrySelection(0);
            else if (down)
                SetFirstLaunchTutorialEntrySelection(1);

            if (ReadFirstLaunchTutorialConfirmPressed())
                ConfirmFirstLaunchTutorialEntrySelection();
        }

        private static bool ReadFirstLaunchTutorialEntryUpPressed()
        {
#if ENABLE_INPUT_SYSTEM
            Keyboard keyboard = Keyboard.current;
            if (keyboard != null &&
                (keyboard.wKey.wasPressedThisFrame ||
                 keyboard.upArrowKey.wasPressedThisFrame))
            {
                return true;
            }
            Gamepad gamepad = Gamepad.current;
            if (gamepad != null &&
                (gamepad.dpad.up.wasPressedThisFrame ||
                 gamepad.leftStick.up.wasPressedThisFrame))
            {
                return true;
            }
#endif
#if ENABLE_LEGACY_INPUT_MANAGER
            if (Input.GetKeyDown(KeyCode.W) ||
                Input.GetKeyDown(KeyCode.UpArrow))
            {
                return true;
            }
#endif
            return false;
        }

        private static bool ReadFirstLaunchTutorialEntryDownPressed()
        {
#if ENABLE_INPUT_SYSTEM
            Keyboard keyboard = Keyboard.current;
            if (keyboard != null &&
                (keyboard.sKey.wasPressedThisFrame ||
                 keyboard.downArrowKey.wasPressedThisFrame))
            {
                return true;
            }
            Gamepad gamepad = Gamepad.current;
            if (gamepad != null &&
                (gamepad.dpad.down.wasPressedThisFrame ||
                 gamepad.leftStick.down.wasPressedThisFrame))
            {
                return true;
            }
#endif
#if ENABLE_LEGACY_INPUT_MANAGER
            if (Input.GetKeyDown(KeyCode.S) ||
                Input.GetKeyDown(KeyCode.DownArrow))
            {
                return true;
            }
#endif
            return false;
        }

        private void SetTutorialEntryTargetsActive(bool active)
        {
            SetTutorialScreenTargetActive(
                TutorialPlayTargetIndex,
                active,
                0f,
                2f,
                600f,
                92f
            );
            SetTutorialScreenTargetActive(
                TutorialSkipTargetIndex,
                active,
                0f,
                -114f,
                600f,
                92f
            );
        }

        private Image CreateTutorialEntity(
            Transform parent,
            string name,
            float x,
            float y,
            float width,
            float height,
            Color color)
        {
            Image entity = CreatePanel(
                parent,
                name,
                x,
                y,
                width,
                height,
                color
            );
            ApplyFirstLaunchTutorialPixelSprite(
                entity,
                name,
                color
            );
            return entity;
        }

        private Image CreateFirstLaunchTutorialBindingCard(
            Transform parent,
            string name,
            string category,
            float x,
            Color background,
            out Text categoryText,
            out Text bindingText)
        {
            Image card = CreatePanel(
                parent,
                name,
                x,
                -62f,
                360f,
                76f,
                background
            );
            card.raycastTarget = false;
            AddOutline(
                card.gameObject,
                new Color(0.30f, 0.50f, 0.68f, 0.72f),
                2f
            );

            categoryText = CreateText(
                card.rectTransform,
                name + " Category",
                category,
                0f,
                21f,
                320f,
                18f,
                12,
                TextAnchor.MiddleCenter,
                new Color(0.56f, 0.74f, 0.88f, 1f),
                FontStyle.Bold
            );

            bindingText = CreateText(
                card.rectTransform,
                name + " Value",
                string.Empty,
                0f,
                -10f,
                320f,
                38f,
                25,
                TextAnchor.MiddleCenter,
                Color.white,
                FontStyle.Bold
            );
            bindingText.resizeTextForBestFit = true;
            bindingText.resizeTextMinSize = 18;
            bindingText.resizeTextMaxSize = 25;
            return card;
        }

        private void BuildFirstLaunchTutorialExitConfirmation(
            Transform parent)
        {
            firstLaunchTutorialExitDimmer = CreatePanel(
                parent,
                "Tutorial Exit Dimmer",
                0f,
                0f,
                900f,
                850f,
                new Color(0f, 0f, 0f, 0.68f)
            );

            Image panel = CreatePanel(
                parent,
                "Tutorial Exit Confirmation",
                0f,
                0f,
                650f,
                430f,
                new Color(0.035f, 0.055f, 0.085f, 0.99f)
            );
            firstLaunchTutorialExitPanel = panel.gameObject;
            AddOutline(
                panel.gameObject,
                new Color(0.48f, 0.68f, 0.92f, 0.80f),
                3f
            );

            CreateText(
                panel.rectTransform,
                "Tutorial Exit Title",
                "LEAVE THE TUTORIAL?",
                0f,
                145f,
                560f,
                64f,
                32,
                TextAnchor.MiddleCenter,
                Color.white,
                FontStyle.Bold
            );
            CreateText(
                panel.rectTransform,
                "Tutorial Exit Warning",
                "The tutorial will not appear automatically again.",
                0f,
                82f,
                540f,
                70f,
                18,
                TextAnchor.MiddleCenter,
                new Color(0.78f, 0.84f, 0.92f, 1f),
                FontStyle.Normal
            );

            firstLaunchTutorialContinueOption = CreatePanel(
                panel.rectTransform,
                "Continue Tutorial Option",
                0f,
                -20f,
                500f,
                72f,
                new Color(0.04f, 0.20f, 0.34f, 1f)
            );
            firstLaunchTutorialLeaveOption = CreatePanel(
                panel.rectTransform,
                "Leave Tutorial Option",
                0f,
                -112f,
                500f,
                72f,
                new Color(0.30f, 0.06f, 0.08f, 1f)
            );

            CreateText(
                firstLaunchTutorialContinueOption.rectTransform,
                "Continue Tutorial Label",
                "CONTINUE TUTORIAL",
                0f,
                0f,
                450f,
                52f,
                22,
                TextAnchor.MiddleCenter,
                Color.white,
                FontStyle.Bold
            );
            CreateText(
                firstLaunchTutorialLeaveOption.rectTransform,
                "Leave Tutorial Label",
                "LEAVE TUTORIAL",
                0f,
                0f,
                450f,
                52f,
                22,
                TextAnchor.MiddleCenter,
                Color.white,
                FontStyle.Bold
            );

            firstLaunchTutorialExitDimmer.gameObject.SetActive(false);
            firstLaunchTutorialExitPanel.SetActive(false);
        }

        private bool UpdateFirstLaunchTutorial()
        {
            if (!firstLaunchTutorialActive ||
                displayedPage != EffectivePage.FirstLaunchTutorial)
            {
                return false;
            }

            if (UpdateFirstLaunchTutorialEntryChoice())
                return true;

            if (firstLaunchTutorialFeedback != null &&
                firstLaunchTutorialFeedbackClearAt > 0f &&
                Time.unscaledTime >= firstLaunchTutorialFeedbackClearAt)
            {
                firstLaunchTutorialFeedback.text = string.Empty;
                firstLaunchTutorialFeedbackClearAt = 0f;
            }

            if (firstLaunchTutorialTransitionOut)
            {
                UpdateFirstLaunchTutorialTransitionOut();
                UpdateFirstLaunchTutorialVisualPresentation();
                return true;
            }

            if (firstLaunchTutorialExitOpen)
            {
                UpdateFirstLaunchTutorialExitAnimation();
                UpdateFirstLaunchTutorialVisualPresentation();
                return true;
            }

            float elapsed =
                Time.unscaledTime -
                firstLaunchTutorialStepStartedAt;
            UpdateFirstLaunchTutorialFreePlay(elapsed);
            UpdateFirstLaunchTutorialVisualPresentation();
            return true;
        }

        private void ShowFirstLaunchTutorialHoldProgress(
            string label,
            float progress,
            bool visible)
        {
            if (firstLaunchTutorialHoldFill != null)
            {
                firstLaunchTutorialHoldFill.gameObject.SetActive(visible);
                firstLaunchTutorialHoldFill.rectTransform.sizeDelta =
                    new Vector2(720f * Mathf.Clamp01(progress), 8f);
            }

            if (firstLaunchTutorialFeedback == null)
                return;

            if (!visible)
            {
                if (firstLaunchTutorialFeedbackClearAt <= 0f)
                    firstLaunchTutorialFeedback.text = string.Empty;
                return;
            }

            int percent = Mathf.RoundToInt(Mathf.Clamp01(progress) * 100f);
            firstLaunchTutorialFeedback.text = label + "  " + percent + "%";
            firstLaunchTutorialFeedbackClearAt = 0f;
        }

        private void UpdateFirstLaunchTutorialTransitionOut()
        {
            float elapsed = Time.unscaledTime -
                            firstLaunchTutorialTransitionStartedAt;
            float progress = Mathf.Clamp01(
                elapsed / TutorialTransitionSeconds
            );

            if (firstLaunchTutorialWhiteOverlay != null)
            {
                firstLaunchTutorialWhiteOverlay.gameObject.SetActive(true);
                firstLaunchTutorialWhiteOverlay.transform.SetAsLastSibling();
                Color color = firstLaunchTutorialWhiteOverlay.color;
                color.r = 0.003f;
                color.g = 0.006f;
                color.b = 0.014f;
                color.a = progress;
                firstLaunchTutorialWhiteOverlay.color = color;
            }

            if (progress < 1f)
                return;

            firstLaunchTutorialFinishedThisSession = true;
            firstLaunchTutorialActive = false;
            firstLaunchTutorialExitOpen = false;
            firstLaunchTutorialTransitionOut = false;
            ClearFirstLaunchTutorialPhysicalHighlight();
            displayedPageInitialized = false;
            menuInputUnlockAt = Time.unscaledTime + 0.18f;
            menuInputNeedsRelease = true;
        }

        private void UpdateFirstLaunchTutorialNavigationInput()
        {
            if (firstLaunchTutorialEntryPhase !=
                    FirstLaunchTutorialEntryPhase.Playing)
            {
                UpdateFirstLaunchTutorialEntryNavigationInput();
                return;
            }

            UpdateFirstLaunchTutorialFreePlayInput();
        }

        private bool HandleFirstLaunchTutorialControl(
            BDModernHandheldControlTarget target)
        {
            if (!firstLaunchTutorialActive ||
                displayedPage != EffectivePage.FirstLaunchTutorial ||
                target == null)
            {
                return false;
            }

            if (firstLaunchTutorialEntryPhase !=
                    FirstLaunchTutorialEntryPhase.Playing)
            {
                target.Pulse();
                if (target.Action ==
                        BDModernHandheldControlTarget.ControlAction.ScreenItem)
                {
                    if (target.Index == TutorialPlayTargetIndex)
                    {
                        SetFirstLaunchTutorialEntrySelection(0);
                        ConfirmFirstLaunchTutorialEntrySelection();
                    }
                    else if (target.Index == TutorialSkipTargetIndex)
                    {
                        SetFirstLaunchTutorialEntrySelection(1);
                        ConfirmFirstLaunchTutorialEntrySelection();
                    }
                    return true;
                }

                if (target.Action ==
                        BDModernHandheldControlTarget.ControlAction.DPadUp)
                {
                    SetFirstLaunchTutorialEntrySelection(0);
                }
                else if (target.Action ==
                         BDModernHandheldControlTarget.ControlAction.DPadDown)
                {
                    SetFirstLaunchTutorialEntrySelection(1);
                }
                else if (target.Action ==
                         BDModernHandheldControlTarget.ControlAction.Confirm)
                {
                    ConfirmFirstLaunchTutorialEntrySelection();
                }
                return true;
            }

            if (target.Action ==
                    BDModernHandheldControlTarget.ControlAction.ScreenItem)
            {
                if (!firstLaunchTutorialExitOpen)
                    return true;

                if (target.Index == TutorialContinueTargetIndex)
                {
                    SetFirstLaunchTutorialExitSelection(0);
                    target.Pulse();
                    ConfirmFirstLaunchTutorialExitSelection();
                }
                else if (target.Index == TutorialLeaveTargetIndex)
                {
                    SetFirstLaunchTutorialExitSelection(1);
                    target.Pulse();
                    ConfirmFirstLaunchTutorialExitSelection();
                }
                return true;
            }

            target.Pulse();
            FirstLaunchTutorialInputSource source =
                FirstLaunchTutorialInputSource.Handheld;
#if ENABLE_INPUT_SYSTEM
            if (Touchscreen.current != null &&
                Touchscreen.current.primaryTouch.press.isPressed)
            {
                source = FirstLaunchTutorialInputSource.Touch;
            }
#endif
            HandleFirstLaunchTutorialAction(target.Action, source);
            return true;
        }

        private bool HandleFirstLaunchTutorialHover(
            BDModernHandheldControlTarget target)
        {
            if (!firstLaunchTutorialActive ||
                displayedPage != EffectivePage.FirstLaunchTutorial)
            {
                return false;
            }

            if (firstLaunchTutorialEntryPhase !=
                    FirstLaunchTutorialEntryPhase.Playing &&
                target != null &&
                target.Action ==
                    BDModernHandheldControlTarget.ControlAction.ScreenItem)
            {
                if (target.Index == TutorialPlayTargetIndex)
                    SetFirstLaunchTutorialEntrySelection(0);
                else if (target.Index == TutorialSkipTargetIndex)
                    SetFirstLaunchTutorialEntrySelection(1);
                return true;
            }

            if (firstLaunchTutorialExitOpen && target != null &&
                target.Action ==
                    BDModernHandheldControlTarget.ControlAction.ScreenItem)
            {
                if (target.Index == TutorialContinueTargetIndex)
                    SetFirstLaunchTutorialExitSelection(0);
                else if (target.Index == TutorialLeaveTargetIndex)
                    SetFirstLaunchTutorialExitSelection(1);
            }

            return true;
        }

        private void HandleFirstLaunchTutorialAction(
            BDModernHandheldControlTarget.ControlAction action,
            FirstLaunchTutorialInputSource source)
        {
            if (Time.frameCount == firstLaunchTutorialLastActionFrame)
                return;

            firstLaunchTutorialLastActionFrame = Time.frameCount;
            firstLaunchTutorialInputSource =
                ResolveFirstLaunchTutorialSource(source);
            UpdateFirstLaunchTutorialPrompt();

            if (action == BDModernHandheldControlTarget.ControlAction.Exit)
            {
                if (firstLaunchTutorialExitOpen)
                    CloseFirstLaunchTutorialExitConfirmation();
                else
                    OpenFirstLaunchTutorialExitConfirmation();
                return;
            }

            if (firstLaunchTutorialExitOpen)
            {
                if (action ==
                        BDModernHandheldControlTarget.ControlAction.DPadUp ||
                    action ==
                        BDModernHandheldControlTarget.ControlAction.DPadLeft)
                {
                    SetFirstLaunchTutorialExitSelection(0);
                }
                else if (action ==
                             BDModernHandheldControlTarget.ControlAction.DPadDown ||
                         action ==
                             BDModernHandheldControlTarget.ControlAction.DPadRight)
                {
                    SetFirstLaunchTutorialExitSelection(1);
                }
                else if (action ==
                         BDModernHandheldControlTarget.ControlAction.Confirm)
                {
                    ConfirmFirstLaunchTutorialExitSelection();
                }
                return;
            }

            HandleFirstLaunchTutorialFreePlayAction(
                action,
                firstLaunchTutorialInputSource
            );
        }

        private void SetFirstLaunchTutorialStep(
            FirstLaunchTutorialStep step)
        {
            firstLaunchTutorialStep = step;
            firstLaunchTutorialStepStartedAt = Time.unscaledTime;
            firstLaunchTutorialInputUnlockAt = Time.unscaledTime + 0.10f;
            firstLaunchTutorialPrimaryHoldStartedAt = -1f;
            firstLaunchTutorialHealHoldStartedAt = -1f;
            firstLaunchTutorialGrappleHoldStartedAt = -1f;
            firstLaunchTutorialChargedShotPendingStartedAt = -1f;
            firstLaunchTutorialChargedShotStartedAt = -1f;
            ResetFirstLaunchTutorialHintEscalation();

            ConfigureFirstLaunchTutorialScene(step);
            UpdateFirstLaunchTutorialPrompt();
            BeginFirstLaunchTutorialInstructionPresentation(step);
            UpdateFirstLaunchTutorialPhysicalHighlight();
        }

        private void ConfigureFirstLaunchTutorialScene(
            FirstLaunchTutorialStep step)
        {
            ConfigureFirstLaunchTutorialFreePlayScene(step);
        }

        private void UpdateFirstLaunchTutorialPrompt()
        {
            if (firstLaunchTutorialPrompt == null ||
                firstLaunchTutorialDetail == null ||
                firstLaunchTutorialProgress == null)
            {
                return;
            }

            firstLaunchTutorialProgress.text =
                ResolveFirstLaunchTutorialProgressLabel();
            firstLaunchTutorialPrompt.text =
                ResolveFirstLaunchTutorialPrompt();
            firstLaunchTutorialDetail.text =
                ResolveFirstLaunchTutorialDetail();
            UpdateFirstLaunchTutorialBindings();
        }

        private void UpdateFirstLaunchTutorialBindings()
        {
            string action = ResolveFirstLaunchTutorialBindingAction();
            bool hasBindings = !string.IsNullOrEmpty(action);
            bool keyboardActive =
                firstLaunchTutorialInputSource ==
                    FirstLaunchTutorialInputSource.Keyboard;
            bool controllerActive =
                firstLaunchTutorialInputSource ==
                    FirstLaunchTutorialInputSource.Gamepad;
            bool handheldActive = !keyboardActive;

            if (firstLaunchTutorialKeyboardBindingCard != null)
            {
                firstLaunchTutorialKeyboardBindingCard.gameObject.SetActive(
                    hasBindings && keyboardActive
                );
                firstLaunchTutorialKeyboardBindingCard.rectTransform.anchoredPosition =
                    new Vector2(0f, -62f);
            }
            if (firstLaunchTutorialHandheldBindingCard != null)
            {
                firstLaunchTutorialHandheldBindingCard.gameObject.SetActive(
                    hasBindings && handheldActive
                );
                firstLaunchTutorialHandheldBindingCard.rectTransform.anchoredPosition =
                    new Vector2(0f, -62f);
            }
            if (firstLaunchTutorialBindingDivider != null)
                firstLaunchTutorialBindingDivider.gameObject.SetActive(false);

            if (firstLaunchTutorialDetail != null)
            {
                firstLaunchTutorialDetail.rectTransform.anchoredPosition =
                    hasBindings ? new Vector2(0f, 25f) : new Vector2(0f, -12f);
                firstLaunchTutorialDetail.rectTransform.sizeDelta =
                    hasBindings ? new Vector2(748f, 32f) : new Vector2(748f, 70f);
            }

            if (!hasBindings)
                return;
            if (firstLaunchTutorialKeyboardBindingTitle != null)
                firstLaunchTutorialKeyboardBindingTitle.text =
                    "KEYBOARD / MOUSE";
            if (firstLaunchTutorialKeyboardBinding != null)
                firstLaunchTutorialKeyboardBinding.text =
                    ResolveFirstLaunchTutorialKeyboardBinding(action);
            if (firstLaunchTutorialHandheldBindingTitle != null)
            {
                firstLaunchTutorialHandheldBindingTitle.text =
                    controllerActive ? "CONTROLLER" : "HANDHELD";
            }
            if (firstLaunchTutorialHandheldBinding != null)
            {
                firstLaunchTutorialHandheldBinding.text =
                    controllerActive
                        ? ResolveFirstLaunchTutorialGamepadBinding(action)
                        : ResolveFirstLaunchTutorialHandheldBinding(action);
            }
        }

        private string ResolveFirstLaunchTutorialProgressLabel()
        {
            return ResolveFirstLaunchTutorialFreePlayProgressLabel();
        }

        private string ResolveFirstLaunchTutorialPrompt()
        {
            switch (firstLaunchTutorialStep)
            {
                case FirstLaunchTutorialStep.WhiteBoot: return "WAKING THE HANDHELD...";
                case FirstLaunchTutorialStep.Move: return "MOVE";
                case FirstLaunchTutorialStep.Jump: return "JUMP THE ROOT";
                case FirstLaunchTutorialStep.JumpAttack: return "ATTACK IN THE AIR";
                case FirstLaunchTutorialStep.WallJump: return "WALL JUMP TO HIGH GROUND";
                case FirstLaunchTutorialStep.MountHorse: return "RIDE";
                case FirstLaunchTutorialStep.RideHorse: return "FEEL THE HORSE'S WEIGHT";
                case FirstLaunchTutorialStep.EnemyArrival: return "AN AMBUSH";
                case FirstLaunchTutorialStep.HorseShot: return "THE HORSE IS HIT";
                case FirstLaunchTutorialStep.AttackEnemy: return "QUICK ATTACK";
                case FirstLaunchTutorialStep.HeavyAttack: return "HEAVY ATTACK";
                case FirstLaunchTutorialStep.Dodge: return "EVADE THE STRIKE";
                case FirstLaunchTutorialStep.Parry: return "PARRY BEFORE IMPACT";
                case FirstLaunchTutorialStep.HorseReturn: return "THE HORSE RETURNS";
                case FirstLaunchTutorialStep.HealHorse: return "HEAL THE HORSE";
                case FirstLaunchTutorialStep.RemountHorse: return "MOUNT AGAIN";
                case FirstLaunchTutorialStep.SpinAttack: return "SPIN ATTACK";
                case FirstLaunchTutorialStep.Grapple: return "GRAPPLING HOOK";
                case FirstLaunchTutorialStep.HazardKnockback: return "TURN THE WORLD INTO A WEAPON";
                case FirstLaunchTutorialStep.RangedAttack: return "FIRE WHILE RIDING";
                case FirstLaunchTutorialStep.Reload: return "RELOADING";
                case FirstLaunchTutorialStep.ChargedShot: return "HOLD UNTIL IT FIRES";
                case FirstLaunchTutorialStep.MountedImpact: return "RAM SMALL ENEMIES";
                case FirstLaunchTutorialStep.DismountHorse: return "DISMOUNT";
                case FirstLaunchTutorialStep.SidePath: return "THE ROAD IS NOT THE WHOLE WORLD";
                case FirstLaunchTutorialStep.CombinedEncounter: return "USE WHAT YOU LEARNED";
                case FirstLaunchTutorialStep.MiniBossIntro: return "FINAL TEST — READ THIS FIRST";
                case FirstLaunchTutorialStep.MiniBossPhaseOne: return "DODGE THE WINDUP — ATTACK RECOVERY";
                case FirstLaunchTutorialStep.MiniBossPhaseTwo: return "DODGE THE RELEASE — ATTACK RECOVERY";
                case FirstLaunchTutorialStep.MiniBossDefeated: return "THE WAY IS OPEN";
                case FirstLaunchTutorialStep.Collectible: return "TAKE THE LAST MEMORY";
                default: return "YOU ARE READY";
            }
        }

        private string ResolveFirstLaunchTutorialDetail()
        {
            switch (firstLaunchTutorialStep)
            {
                case FirstLaunchTutorialStep.WhiteBoot: return "A short playable adventure is loading.";
                case FirstLaunchTutorialStep.Move: return "Explore the clearing. The guide appears only when it matters.";
                case FirstLaunchTutorialStep.Jump: return "Clear the obstacle and land safely.";
                case FirstLaunchTutorialStep.JumpAttack: return "Jump, face the target and attack before landing.";
                case FirstLaunchTutorialStep.WallJump: return "Jump into the wall, jump left to the platform, then jump right onto the ground above the wall.";
                case FirstLaunchTutorialStep.MountHorse: return "Walk close, then use the active interaction binding.";
                case FirstLaunchTutorialStep.RideHorse: return "The horse accelerates, turns wider and takes longer to stop.";
                case FirstLaunchTutorialStep.EnemyArrival: return "Read the enemy before committing.";
                case FirstLaunchTutorialStep.HorseShot: return "The hit separates rider and horse.";
                case FirstLaunchTutorialStep.AttackEnemy: return "Tap for a fast strike with short recovery.";
                case FirstLaunchTutorialStep.HeavyAttack: return "Tap heavy for more damage, knockback and longer recovery.";
                case FirstLaunchTutorialStep.Dodge: return "Pass through the active hit during the invulnerable window.";
                case FirstLaunchTutorialStep.Parry: return "Parry is optional, but success leaves the enemy exposed.";
                case FirstLaunchTutorialStep.HorseReturn: return "The injured horse returns when the danger clears.";
                case FirstLaunchTutorialStep.HealHorse: return "Stay close and hold until the healing action completes.";
                case FirstLaunchTutorialStep.RemountHorse: return "Return to the saddle and continue east.";
                case FirstLaunchTutorialStep.SpinAttack: return "Hold light when several enemies crowd you.";
                case FirstLaunchTutorialStep.Grapple: return "Hold heavy to pull a small enemy into sword range.";
                case FirstLaunchTutorialStep.HazardKnockback: return "Heavy, spin, hook placement or horse impact can push enemies into danger.";
                case FirstLaunchTutorialStep.RangedAttack: return "Fire the final round while riding; the empty magazine reloads automatically.";
                case FirstLaunchTutorialStep.Reload: return "The empty magazine reloads automatically; firing is locked until ready.";
                case FirstLaunchTutorialStep.ChargedShot: return "Hold the ranged input. At full charge it fires automatically and consumes every loaded round; releasing after charge begins cancels it.";
                case FirstLaunchTutorialStep.MountedImpact: return "Build speed and hit a small enemy directly.";
                case FirstLaunchTutorialStep.DismountHorse: return "Slow down, then leave the saddle at the safe marker.";
                case FirstLaunchTutorialStep.SidePath: return "No marker points to secrets. Look beyond the main path.";
                case FirstLaunchTutorialStep.CombinedEncounter: return "Ride, shoot, dismount, control space and survive in your own order.";
                case FirstLaunchTutorialStep.MiniBossIntro: return "Press interact when ready. Do not attack the windup: dodge or parry, then strike only while RECOVERY — ATTACK NOW is visible.";
                case FirstLaunchTutorialStep.MiniBossPhaseOne: return "The boss cannot damage you from a distance. Read WINDUP, avoid IMPACT, then attack during the clearly labeled recovery window.";
                case FirstLaunchTutorialStep.MiniBossPhaseTwo: return "Projectiles lock their target when released. Move or dodge away, then punish the recovery window.";
                case FirstLaunchTutorialStep.MiniBossDefeated: return "Damage is disabled; the gate opens only after the death animation.";
                case FirstLaunchTutorialStep.Collectible: return "Collect the memory to finish the first adventure.";
                default: return "The main menu will appear next.";
            }
        }

        private string ResolveFirstLaunchTutorialBindingAction()
        {
            switch (firstLaunchTutorialStep)
            {
                case FirstLaunchTutorialStep.Move:
                case FirstLaunchTutorialStep.RideHorse: return "MOVE";
                case FirstLaunchTutorialStep.Jump:
                case FirstLaunchTutorialStep.WallJump: return "JUMP";
                case FirstLaunchTutorialStep.JumpAttack: return "ATTACK";
                case FirstLaunchTutorialStep.MountHorse:
                case FirstLaunchTutorialStep.RemountHorse:
                case FirstLaunchTutorialStep.DismountHorse:
                case FirstLaunchTutorialStep.Collectible: return "INTERACT";
                case FirstLaunchTutorialStep.AttackEnemy: return "ATTACK";
                case FirstLaunchTutorialStep.HeavyAttack: return "HEAVY";
                case FirstLaunchTutorialStep.Dodge: return "DODGE";
                case FirstLaunchTutorialStep.Parry: return "PARRY";
                case FirstLaunchTutorialStep.HealHorse: return "HEAL";
                case FirstLaunchTutorialStep.SpinAttack: return "SPIN";
                case FirstLaunchTutorialStep.Grapple: return "GRAPPLE";
                case FirstLaunchTutorialStep.RangedAttack:
                case FirstLaunchTutorialStep.ChargedShot: return "RANGED";
                case FirstLaunchTutorialStep.MiniBossIntro: return "INTERACT";
                case FirstLaunchTutorialStep.MountedImpact: return "MOVE";
                default: return string.Empty;
            }
        }

        private static string ResolveFirstLaunchTutorialKeyboardBinding(
            string action)
        {
            switch (action)
            {
                case "MOVE":
                    return "WASD / ARROWS";
                case "INTERACT":
                    return "E";
                case "JUMP":
                    return "SPACE";
                case "ATTACK":
                    return "J / LEFT CLICK";
                case "HEAL":
                    return "HOLD F";
                case "RANGED":
                    return "Q";
                case "DODGE":
                    return "DOUBLE-TAP A/D OR LEFT/RIGHT";
                case "HEAVY":
                    return "K / RIGHT CLICK";
                case "SPIN":
                    return "HOLD J / LEFT CLICK";
                case "PARRY":
                    return "J / LEFT CLICK OR K / RIGHT CLICK";
                case "GRAPPLE":
                    return "HOLD K / RIGHT CLICK";
                default:
                    return string.Empty;
            }
        }


        private static string ResolveFirstLaunchTutorialGamepadBinding(
            string action)
        {
            switch (action)
            {
                case "MOVE":
                    return "LEFT STICK / D-PAD";
                case "INTERACT":
                    return "B / EAST";
                case "JUMP":
                    return "A / SOUTH";
                case "ATTACK":
                    return "X / WEST";
                case "HEAL":
                    return "HOLD LB";
                case "RANGED":
                    return "RB";
                case "DODGE":
                    return "DOUBLE-TAP D-PAD LEFT/RIGHT";
                case "HEAVY":
                    return "Y / NORTH";
                case "SPIN":
                    return "HOLD X / WEST";
                case "PARRY":
                    return "X / WEST OR Y / NORTH";
                case "GRAPPLE":
                    return "HOLD Y / NORTH";
                default:
                    return string.Empty;
            }
        }

        private static string ResolveFirstLaunchTutorialHandheldBinding(
            string action)
        {
            switch (action)
            {
                case "MOVE":
                    return "D-PAD";
                case "INTERACT":
                    return "SELECT";
                case "JUMP":
                    return "B";
                case "ATTACK":
                    return "X";
                case "HEAL":
                    return "HOLD A";
                case "RANGED":
                    return "A";
                case "DODGE":
                    return "DOUBLE-TAP D-PAD LEFT/RIGHT";
                case "HEAVY":
                    return "Y";
                case "PARRY":
                    return "X OR Y";
                case "SPIN":
                    return "HOLD X";
                case "GRAPPLE":
                    return "HOLD Y";
                default:
                    return string.Empty;
            }
        }

        private void ShowFirstLaunchTutorialSuccess(string value)
        {
            if (firstLaunchTutorialFeedback == null)
                return;

            firstLaunchTutorialFeedback.text = value;
            firstLaunchTutorialFeedbackClearAt = Time.unscaledTime + 0.85f;
            PlayClick();
        }

        private void OpenFirstLaunchTutorialExitConfirmation()
        {
            if (firstLaunchTutorialExitOpen ||
                firstLaunchTutorialTransitionOut)
            {
                return;
            }

            firstLaunchTutorialExitOpen = true;
            firstLaunchTutorialExitSelection = 0;
            firstLaunchTutorialPauseStartedAt = Time.unscaledTime;
            firstLaunchTutorialExitOpenedAt = Time.unscaledTime;
            firstLaunchTutorialInputUnlockAt =
                Time.unscaledTime + TutorialExitInputGuardSeconds;

            if (firstLaunchTutorialExitPanel != null)
                firstLaunchTutorialExitPanel.transform.localScale =
                    Vector3.one * 0.90f;
            if (firstLaunchTutorialExitDimmer != null)
                firstLaunchTutorialExitDimmer.gameObject.SetActive(true);
            if (firstLaunchTutorialExitPanel != null)
                firstLaunchTutorialExitPanel.SetActive(true);

            SetTutorialExitTargetsActive(true);
            UpdateFirstLaunchTutorialExitVisuals();
            PlayClick();
        }

        private void UpdateFirstLaunchTutorialExitAnimation()
        {
            if (firstLaunchTutorialExitPanel == null)
                return;

            float progress = Mathf.Clamp01(
                (Time.unscaledTime - firstLaunchTutorialExitOpenedAt) / 0.16f
            );
            float eased = 1f - Mathf.Pow(1f - progress, 3f);
            firstLaunchTutorialExitPanel.transform.localScale =
                Vector3.one * Mathf.Lerp(0.90f, 1f, eased);
        }

        private void CloseFirstLaunchTutorialExitConfirmation()
        {
            if (!firstLaunchTutorialExitOpen)
                return;

            firstLaunchTutorialExitOpen = false;
            if (firstLaunchTutorialPauseStartedAt >= 0f)
            {
                firstLaunchTutorialStepStartedAt +=
                    Time.unscaledTime - firstLaunchTutorialPauseStartedAt;
                if (firstLaunchTutorialPrimaryHoldStartedAt >= 0f)
                {
                    firstLaunchTutorialPrimaryHoldStartedAt +=
                        Time.unscaledTime - firstLaunchTutorialPauseStartedAt;
                }
            }
            firstLaunchTutorialPauseStartedAt = -1f;
            firstLaunchTutorialInputUnlockAt =
                Time.unscaledTime + TutorialExitInputGuardSeconds;

            if (firstLaunchTutorialExitDimmer != null)
                firstLaunchTutorialExitDimmer.gameObject.SetActive(false);
            if (firstLaunchTutorialExitPanel != null)
                firstLaunchTutorialExitPanel.SetActive(false);

            SetTutorialExitTargetsActive(false);
            UpdateFirstLaunchTutorialPhysicalHighlight();
            PlayClick();
        }

        private void SetFirstLaunchTutorialExitSelection(int selection)
        {
            int clamped = Mathf.Clamp(selection, 0, 1);
            if (firstLaunchTutorialExitSelection == clamped)
                return;

            firstLaunchTutorialExitSelection = clamped;
            UpdateFirstLaunchTutorialExitVisuals();
            PlayClick();
        }

        private void UpdateFirstLaunchTutorialExitVisuals()
        {
            if (firstLaunchTutorialContinueOption != null)
            {
                firstLaunchTutorialContinueOption.color =
                    firstLaunchTutorialExitSelection == 0
                        ? new Color(0.08f, 0.42f, 0.62f, 1f)
                        : new Color(0.04f, 0.20f, 0.34f, 1f);
                firstLaunchTutorialContinueOption.rectTransform.localScale =
                    firstLaunchTutorialExitSelection == 0
                        ? new Vector3(1.025f, 1.025f, 1f)
                        : Vector3.one;
            }

            if (firstLaunchTutorialLeaveOption != null)
            {
                firstLaunchTutorialLeaveOption.color =
                    firstLaunchTutorialExitSelection == 1
                        ? new Color(0.62f, 0.10f, 0.14f, 1f)
                        : new Color(0.30f, 0.06f, 0.08f, 1f);
                firstLaunchTutorialLeaveOption.rectTransform.localScale =
                    firstLaunchTutorialExitSelection == 1
                        ? new Vector3(1.025f, 1.025f, 1f)
                        : Vector3.one;
            }

            SetFirstLaunchTutorialPhysicalHighlight(
                BDModernHandheldControlTarget.ControlAction.Confirm
            );
        }

        private void ConfirmFirstLaunchTutorialExitSelection()
        {
            if (!firstLaunchTutorialExitOpen ||
                Time.unscaledTime < firstLaunchTutorialInputUnlockAt)
            {
                return;
            }

            if (firstLaunchTutorialExitSelection == 0)
            {
                CloseFirstLaunchTutorialExitConfirmation();
                return;
            }

            BDFirstLaunchTutorialStateStore.MarkSkipped();
            firstLaunchTutorialExitOpen = false;
            firstLaunchTutorialPauseStartedAt = -1f;
            if (firstLaunchTutorialExitDimmer != null)
                firstLaunchTutorialExitDimmer.gameObject.SetActive(false);
            if (firstLaunchTutorialExitPanel != null)
                firstLaunchTutorialExitPanel.SetActive(false);
            SetTutorialExitTargetsActive(false);
            BeginFirstLaunchTutorialTransition(skip: true);
        }

        private void BeginFirstLaunchTutorialTransition(bool skip)
        {
            if (firstLaunchTutorialTransitionOut)
                return;

            firstLaunchTutorialTransitionOut = true;
            firstLaunchTutorialTransitionStartedAt = Time.unscaledTime;
            ClearFirstLaunchTutorialPhysicalHighlight();

            if (firstLaunchTutorialPrompt != null)
            {
                firstLaunchTutorialPrompt.text = skip
                    ? "RETURNING TO THE MAIN MENU"
                    : "LESSON COMPLETE";
            }
            if (firstLaunchTutorialDetail != null)
            {
                firstLaunchTutorialDetail.text = skip
                    ? "The tutorial will not appear automatically again."
                    : "Your adventure can begin.";
            }
        }

        private void SetTutorialExitTargetsActive(bool active)
        {
            SetTutorialScreenTargetActive(
                TutorialContinueTargetIndex,
                active,
                0f,
                -30f,
                500f,
                72f
            );
            SetTutorialScreenTargetActive(
                TutorialLeaveTargetIndex,
                active,
                0f,
                -122f,
                500f,
                72f
            );
        }

        private void SetTutorialScreenTargetActive(
            int index,
            bool active,
            float x,
            float y,
            float width,
            float height)
        {
            if (screenHitTargetRoot == null)
                return;

            string targetName = "Screen Item Target " + index;
            Transform target = screenHitTargetRoot.Find(targetName);
            if (target == null && active)
            {
                CreateScreenItemTarget(index, x, y, width, height);
                target = screenHitTargetRoot.Find(targetName);
            }

            if (target != null)
                target.gameObject.SetActive(active);
        }

        private void UpdateFirstLaunchTutorialPhysicalHighlight()
        {
            if (firstLaunchTutorialEntryPhase !=
                    FirstLaunchTutorialEntryPhase.Playing)
            {
                SetFirstLaunchTutorialPhysicalHighlight(
                    BDModernHandheldControlTarget.ControlAction.Confirm
                );
                return;
            }

            if (firstLaunchTutorialExitOpen)
            {
                SetFirstLaunchTutorialPhysicalHighlight(
                    BDModernHandheldControlTarget.ControlAction.Confirm
                );
                return;
            }

            SetFirstLaunchTutorialPhysicalHighlight(
                ResolveFirstLaunchTutorialRequiredControl()
            );
        }

        private BDModernHandheldControlTarget.ControlAction
            ResolveFirstLaunchTutorialRequiredControl()
        {
            if (!IsFirstLaunchTutorialInstructionRequested())
            {
                return BDModernHandheldControlTarget.ControlAction.None;
            }

            switch (firstLaunchTutorialStep)
            {
                case FirstLaunchTutorialStep.MountHorse:
                case FirstLaunchTutorialStep.RemountHorse:
                case FirstLaunchTutorialStep.DismountHorse:
                case FirstLaunchTutorialStep.Collectible:
                    return BDModernHandheldControlTarget.ControlAction.Confirm;
                case FirstLaunchTutorialStep.AttackEnemy:
                case FirstLaunchTutorialStep.SpinAttack:
                case FirstLaunchTutorialStep.Parry:
                    return BDModernHandheldControlTarget.ControlAction.Primary;
                case FirstLaunchTutorialStep.HealHorse:
                case FirstLaunchTutorialStep.RangedAttack:
                case FirstLaunchTutorialStep.ChargedShot:
                    return BDModernHandheldControlTarget.ControlAction.Progression;
                case FirstLaunchTutorialStep.Move:
                case FirstLaunchTutorialStep.RideHorse:
                case FirstLaunchTutorialStep.Dodge:
                    return BDModernHandheldControlTarget.ControlAction.DPadRight;
                case FirstLaunchTutorialStep.Jump:
                    return BDModernHandheldControlTarget.ControlAction.ContextBackSettings;
                case FirstLaunchTutorialStep.HeavyAttack:
                case FirstLaunchTutorialStep.Grapple:
                    return BDModernHandheldControlTarget.ControlAction.Credits;
                default:
                    return BDModernHandheldControlTarget.ControlAction.None;
            }
        }

        private void SetFirstLaunchTutorialPhysicalHighlight(
            BDModernHandheldControlTarget.ControlAction action)
        {
            for (int index = 0; index < persistentControls.Count; index++)
            {
                BDModernHandheldControlTarget control = persistentControls[index];
                if (control == null)
                    continue;

                bool highlight = control.Action == action;
                if (firstLaunchTutorialStep ==
                        FirstLaunchTutorialStep.Parry &&
                    action ==
                        BDModernHandheldControlTarget.ControlAction.Primary &&
                    control.Action ==
                        BDModernHandheldControlTarget.ControlAction.Credits)
                {
                    highlight = true;
                }
                if (action ==
                        BDModernHandheldControlTarget.ControlAction.DPadRight &&
                    (control.Action ==
                         BDModernHandheldControlTarget.ControlAction.DPadLeft ||
                     control.Action ==
                         BDModernHandheldControlTarget.ControlAction.DPadUp ||
                     control.Action ==
                         BDModernHandheldControlTarget.ControlAction.DPadDown))
                {
                    highlight = true;
                }

                control.SetTutorialHighlighted(highlight);
            }
        }

        private void ClearFirstLaunchTutorialPhysicalHighlight()
        {
            SetFirstLaunchTutorialPhysicalHighlight(
                BDModernHandheldControlTarget.ControlAction.None
            );
        }

        private FirstLaunchTutorialInputSource ResolveFirstLaunchTutorialSource(
            FirstLaunchTutorialInputSource requested)
        {
            if (requested == FirstLaunchTutorialInputSource.Handheld ||
                requested == FirstLaunchTutorialInputSource.Touch)
            {
                return requested;
            }

#if ENABLE_INPUT_SYSTEM
            if (Gamepad.current != null &&
                Gamepad.current.wasUpdatedThisFrame)
            {
                return FirstLaunchTutorialInputSource.Gamepad;
            }
            if (Touchscreen.current != null &&
                Touchscreen.current.primaryTouch.press.isPressed)
            {
                return FirstLaunchTutorialInputSource.Touch;
            }
#endif
            return FirstLaunchTutorialInputSource.Keyboard;
        }

        private static bool ReadFirstLaunchTutorialConfirmPressed()
        {
            return ReadConfirmPressed() ||
                   ReadFirstLaunchTutorialInteractPressed();
        }

        private static bool ReadFirstLaunchTutorialInteractPressed()
        {
#if ENABLE_INPUT_SYSTEM
            if (Keyboard.current != null &&
                Keyboard.current.eKey.wasPressedThisFrame)
            {
                return true;
            }
            if (Gamepad.current != null &&
                Gamepad.current.buttonEast.wasPressedThisFrame)
            {
                return true;
            }
#endif
#if ENABLE_LEGACY_INPUT_MANAGER
            if (Input.GetKeyDown(KeyCode.E))
                return true;
#endif
            return false;
        }

        private static bool ReadFirstLaunchTutorialLightPressed()
        {
#if ENABLE_INPUT_SYSTEM
            if (Keyboard.current != null &&
                Keyboard.current.jKey.wasPressedThisFrame)
            {
                return true;
            }
            if (Mouse.current != null &&
                Mouse.current.leftButton.wasPressedThisFrame)
            {
                return true;
            }
            if (Gamepad.current != null &&
                Gamepad.current.buttonWest.wasPressedThisFrame)
            {
                return true;
            }
#endif
#if ENABLE_LEGACY_INPUT_MANAGER
            if (Input.GetKeyDown(KeyCode.J) ||
                Input.GetMouseButtonDown(0))
            {
                return true;
            }
#endif
            return false;
        }

        private static bool ReadFirstLaunchTutorialHealPressed()
        {
#if ENABLE_INPUT_SYSTEM
            if (Keyboard.current != null &&
                Keyboard.current.fKey.wasPressedThisFrame)
            {
                return true;
            }
            if (Gamepad.current != null &&
                Gamepad.current.leftShoulder.wasPressedThisFrame)
            {
                return true;
            }
#endif
#if ENABLE_LEGACY_INPUT_MANAGER
            if (Input.GetKeyDown(KeyCode.F))
                return true;
#endif
            return false;
        }

        private bool TryReadFirstLaunchTutorialDirectionalDodge()
        {
            bool leftPressed = false;
            bool rightPressed = false;

#if ENABLE_INPUT_SYSTEM
            Keyboard keyboard = Keyboard.current;
            if (keyboard != null)
            {
                leftPressed =
                    keyboard.aKey.wasPressedThisFrame ||
                    keyboard.leftArrowKey.wasPressedThisFrame;
                rightPressed =
                    keyboard.dKey.wasPressedThisFrame ||
                    keyboard.rightArrowKey.wasPressedThisFrame;
            }

            Gamepad gamepad = Gamepad.current;
            if (gamepad != null)
            {
                leftPressed |= gamepad.dpad.left.wasPressedThisFrame;
                rightPressed |= gamepad.dpad.right.wasPressedThisFrame;
            }
#endif
#if ENABLE_LEGACY_INPUT_MANAGER
            leftPressed |=
                Input.GetKeyDown(KeyCode.A) ||
                Input.GetKeyDown(KeyCode.LeftArrow);
            rightPressed |=
                Input.GetKeyDown(KeyCode.D) ||
                Input.GetKeyDown(KeyCode.RightArrow);
#endif

            return TryRegisterFirstLaunchTutorialDirectionalDodge(
                leftPressed,
                rightPressed
            );
        }

        private bool TryRegisterFirstLaunchTutorialDirectionalDodge(
            bool leftPressed,
            bool rightPressed)
        {
            float now = Time.unscaledTime;

            if (leftPressed)
            {
                bool committed =
                    now - firstLaunchTutorialLastLeftTapAt <=
                    TutorialDirectionalDodgeDoubleTapSeconds;
                firstLaunchTutorialLastLeftTapAt =
                    committed ? -999f : now;
                if (committed)
                {
                    firstLaunchTutorialLastMoveDirection = Vector2.left;
                    return true;
                }
            }

            if (rightPressed)
            {
                bool committed =
                    now - firstLaunchTutorialLastRightTapAt <=
                    TutorialDirectionalDodgeDoubleTapSeconds;
                firstLaunchTutorialLastRightTapAt =
                    committed ? -999f : now;
                if (committed)
                {
                    firstLaunchTutorialLastMoveDirection = Vector2.right;
                    return true;
                }
            }

            return false;
        }

        private static bool ReadFirstLaunchTutorialHeavyPressed()
        {
#if ENABLE_INPUT_SYSTEM
            if (Keyboard.current != null &&
                Keyboard.current.kKey.wasPressedThisFrame)
            {
                return true;
            }
            if (Mouse.current != null &&
                Mouse.current.rightButton.wasPressedThisFrame)
            {
                return true;
            }
            if (Gamepad.current != null &&
                Gamepad.current.buttonNorth.wasPressedThisFrame)
            {
                return true;
            }
#endif
#if ENABLE_LEGACY_INPUT_MANAGER
            if (Input.GetKeyDown(KeyCode.K) ||
                Input.GetMouseButtonDown(1))
            {
                return true;
            }
#endif
            return false;
        }

        private static bool ReadFirstLaunchTutorialParryPressed()
        {
            // Parry is the timing result of a committed light or heavy melee
            // attack immediately before impact; it is not a separate key.
            return ReadFirstLaunchTutorialLightPressed() ||
                   ReadFirstLaunchTutorialHeavyPressed();
        }

        private bool IsFirstLaunchTutorialHealHeld()
        {
#if ENABLE_INPUT_SYSTEM
            if (Keyboard.current != null &&
                Keyboard.current.fKey.isPressed)
            {
                return true;
            }
            if (Gamepad.current != null &&
                Gamepad.current.leftShoulder.isPressed)
            {
                return true;
            }
            if (Mouse.current != null &&
                Mouse.current.leftButton.isPressed &&
                hoveredTarget != null &&
                hoveredTarget.Action ==
                    BDModernHandheldControlTarget.ControlAction.Progression)
            {
                firstLaunchTutorialInputSource =
                    FirstLaunchTutorialInputSource.Handheld;
                return true;
            }
            if (Touchscreen.current != null &&
                Touchscreen.current.primaryTouch.press.isPressed &&
                hoveredTarget != null &&
                hoveredTarget.Action ==
                    BDModernHandheldControlTarget.ControlAction.Progression)
            {
                firstLaunchTutorialInputSource =
                    FirstLaunchTutorialInputSource.Touch;
                return true;
            }
#endif
#if ENABLE_LEGACY_INPUT_MANAGER
            if (Input.GetKey(KeyCode.F))
                return true;
            if (Input.GetMouseButton(0) &&
                hoveredTarget != null &&
                hoveredTarget.Action ==
                    BDModernHandheldControlTarget.ControlAction.Progression)
            {
                firstLaunchTutorialInputSource =
                    FirstLaunchTutorialInputSource.Handheld;
                return true;
            }
#endif
            return false;
        }

        private bool IsFirstLaunchTutorialRangedHeld()
        {
#if ENABLE_INPUT_SYSTEM
            if (Keyboard.current != null &&
                Keyboard.current.qKey.isPressed)
            {
                return true;
            }
            if (Gamepad.current != null &&
                Gamepad.current.rightShoulder.isPressed)
            {
                return true;
            }
            if (Mouse.current != null &&
                Mouse.current.leftButton.isPressed &&
                hoveredTarget != null &&
                hoveredTarget.Action ==
                    BDModernHandheldControlTarget.ControlAction.Progression)
            {
                firstLaunchTutorialInputSource =
                    FirstLaunchTutorialInputSource.Handheld;
                return true;
            }
            if (Touchscreen.current != null &&
                Touchscreen.current.primaryTouch.press.isPressed &&
                hoveredTarget != null &&
                hoveredTarget.Action ==
                    BDModernHandheldControlTarget.ControlAction.Progression)
            {
                firstLaunchTutorialInputSource =
                    FirstLaunchTutorialInputSource.Touch;
                return true;
            }
#endif
#if ENABLE_LEGACY_INPUT_MANAGER
            if (Input.GetKey(KeyCode.Q))
                return true;
            if (Input.GetMouseButton(0) &&
                hoveredTarget != null &&
                hoveredTarget.Action ==
                    BDModernHandheldControlTarget.ControlAction.Progression)
            {
                firstLaunchTutorialInputSource =
                    FirstLaunchTutorialInputSource.Handheld;
                return true;
            }
#endif
            return false;
        }

        private bool IsFirstLaunchTutorialHeavyHeld()
        {
#if ENABLE_INPUT_SYSTEM
            if (Keyboard.current != null &&
                Keyboard.current.kKey.isPressed)
            {
                return true;
            }
            if (Gamepad.current != null &&
                Gamepad.current.buttonNorth.isPressed)
            {
                return true;
            }
            if (Mouse.current != null)
            {
                if (Mouse.current.rightButton.isPressed &&
                    hoveredTarget == null)
                {
                    return true;
                }
                if (Mouse.current.leftButton.isPressed &&
                    hoveredTarget != null &&
                    hoveredTarget.Action ==
                        BDModernHandheldControlTarget.ControlAction.Credits)
                {
                    firstLaunchTutorialInputSource =
                        FirstLaunchTutorialInputSource.Handheld;
                    return true;
                }
            }
            if (Touchscreen.current != null &&
                Touchscreen.current.primaryTouch.press.isPressed &&
                hoveredTarget != null &&
                hoveredTarget.Action ==
                    BDModernHandheldControlTarget.ControlAction.Credits)
            {
                firstLaunchTutorialInputSource =
                    FirstLaunchTutorialInputSource.Touch;
                return true;
            }
#endif
#if ENABLE_LEGACY_INPUT_MANAGER
            if (Input.GetKey(KeyCode.K))
                return true;
            if (Input.GetMouseButton(1) &&
                hoveredTarget == null)
            {
                return true;
            }
            if (Input.GetMouseButton(0) &&
                hoveredTarget != null &&
                hoveredTarget.Action ==
                    BDModernHandheldControlTarget.ControlAction.Credits)
            {
                firstLaunchTutorialInputSource =
                    FirstLaunchTutorialInputSource.Handheld;
                return true;
            }
#endif
            return false;
        }

        private void RegisterFirstLaunchTutorialInputSource()
        {
            firstLaunchTutorialInputSource =
                ResolveFirstLaunchTutorialSource(
                    FirstLaunchTutorialInputSource.Keyboard
                );
            UpdateFirstLaunchTutorialPrompt();
        }

        private bool IsFirstLaunchTutorialPrimaryHeld()
        {
#if ENABLE_INPUT_SYSTEM
            if (Keyboard.current != null &&
                Keyboard.current.jKey.isPressed)
            {
                return true;
            }
            if (Gamepad.current != null &&
                Gamepad.current.buttonWest.isPressed)
            {
                return true;
            }
            if (Mouse.current != null &&
                Mouse.current.leftButton.isPressed &&
                hoveredTarget != null &&
                hoveredTarget.Action ==
                    BDModernHandheldControlTarget.ControlAction.Primary)
            {
                firstLaunchTutorialInputSource =
                    FirstLaunchTutorialInputSource.Handheld;
                return true;
            }
            if (Touchscreen.current != null &&
                Touchscreen.current.primaryTouch.press.isPressed &&
                hoveredTarget != null &&
                hoveredTarget.Action ==
                    BDModernHandheldControlTarget.ControlAction.Primary)
            {
                firstLaunchTutorialInputSource =
                    FirstLaunchTutorialInputSource.Touch;
                return true;
            }
#endif
#if ENABLE_LEGACY_INPUT_MANAGER
            if (Input.GetKey(KeyCode.J))
                return true;
            if (Input.GetMouseButton(0) &&
                hoveredTarget == null)
            {
                return true;
            }
            if (Input.GetMouseButton(0) &&
                hoveredTarget != null &&
                hoveredTarget.Action ==
                    BDModernHandheldControlTarget.ControlAction.Primary)
            {
                firstLaunchTutorialInputSource =
                    FirstLaunchTutorialInputSource.Handheld;
                return true;
            }
#endif
            return false;
        }

        private static void SetTutorialEntityActive(Image image, bool active)
        {
            if (image != null)
                image.gameObject.SetActive(active);
        }
    }
}
