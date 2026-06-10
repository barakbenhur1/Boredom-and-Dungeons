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
            MountHorse,
            EnemyArrival,
            HorseShot,
            AttackEnemy,
            HorseReturn,
            HealHorse,
            Move,
            Dodge,
            HeavyAttack,
            SpinAttack,
            Parry,
            Grapple,
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

        private const int TutorialContinueTargetIndex = 100;
        private const int TutorialLeaveTargetIndex = 101;
        private const float TutorialExitInputGuardSeconds = 0.20f;
        private const float TutorialTransitionSeconds = 0.52f;
        private const float TutorialSpinHoldSeconds = 0.78f;
        private const float TutorialHealHoldSeconds = 1.10f;
        private const float TutorialGrappleHoldSeconds = 0.72f;

        private bool firstLaunchTutorialInitialized;
        private bool firstLaunchTutorialActive;
        private bool firstLaunchTutorialFinishedThisSession;
        private bool firstLaunchTutorialExitOpen;
        private bool firstLaunchTutorialTransitionOut;
        private int firstLaunchTutorialExitSelection;
        private int firstLaunchTutorialMovementInputs;
        private float firstLaunchTutorialStepStartedAt;
        private float firstLaunchTutorialInputUnlockAt;
        private float firstLaunchTutorialTransitionStartedAt;
        private float firstLaunchTutorialPrimaryHoldStartedAt = -1f;
        private float firstLaunchTutorialHealHoldStartedAt = -1f;
        private float firstLaunchTutorialGrappleHoldStartedAt = -1f;
        private float firstLaunchTutorialFeedbackClearAt;
        private float firstLaunchTutorialPauseStartedAt = -1f;
        private float firstLaunchTutorialExitOpenedAt;
        private int firstLaunchTutorialLastActionFrame = -1;
        private FirstLaunchTutorialStep firstLaunchTutorialStep;
        private FirstLaunchTutorialInputSource firstLaunchTutorialInputSource;

        private Image firstLaunchTutorialWhiteOverlay;
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
        private Text firstLaunchTutorialKeyboardBinding;
        private Text firstLaunchTutorialHandheldBinding;
        private Text firstLaunchTutorialBindingDivider;

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
        }

        private void DisposeFirstLaunchTutorial()
        {
            DisposeFirstLaunchTutorialPixelAssets();
            ClearFirstLaunchTutorialPhysicalHighlight();
            firstLaunchTutorialActive = false;
            firstLaunchTutorialExitOpen = false;
            firstLaunchTutorialTransitionOut = false;
        }

        private void ResetFirstLaunchTutorialForScene()
        {
            DisposeFirstLaunchTutorialPixelAssets();
            ClearFirstLaunchTutorialPhysicalHighlight();
            firstLaunchTutorialActive = false;
            firstLaunchTutorialExitOpen = false;
            firstLaunchTutorialTransitionOut = false;
            firstLaunchTutorialPrimaryHoldStartedAt = -1f;
            firstLaunchTutorialHealHoldStartedAt = -1f;
            firstLaunchTutorialGrappleHoldStartedAt = -1f;
            firstLaunchTutorialPauseStartedAt = -1f;
            firstLaunchTutorialLastActionFrame = -1;
        }

        private bool ShouldPresentFirstLaunchTutorial()
        {
            if (!firstLaunchTutorialInitialized ||
                firstLaunchTutorialFinishedThisSession ||
                flow == null ||
                flow.IsRunActive ||
                flow.IsPausedFromGameplay ||
                flow.CurrentHandheldPage !=
                    BDMainMenuFlow.HandheldPage.MainMenu)
            {
                return false;
            }

            return BDFirstLaunchTutorialStateStore.ShouldPresent;
        }

        private void BuildFirstLaunchTutorialPage()
        {
            DisposeFirstLaunchTutorialPixelAssets();
            firstLaunchTutorialActive = true;
            firstLaunchTutorialExitOpen = false;
            firstLaunchTutorialTransitionOut = false;
            firstLaunchTutorialExitSelection = 0;
            firstLaunchTutorialMovementInputs = 0;
            firstLaunchTutorialPrimaryHoldStartedAt = -1f;
            firstLaunchTutorialHealHoldStartedAt = -1f;
            firstLaunchTutorialGrappleHoldStartedAt = -1f;
            firstLaunchTutorialPauseStartedAt = -1f;
            firstLaunchTutorialLastActionFrame = -1;
            firstLaunchTutorialInputSource =
                FirstLaunchTutorialInputSource.Keyboard;

            BDFirstLaunchTutorialStateStore.MarkInProgress();

            Image rootPanel = CreatePanel(
                pageRoot,
                "First Launch Tutorial Root",
                0f,
                -10f,
                900f,
                850f,
                new Color(0.012f, 0.016f, 0.028f, 1f)
            );
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
                new Color(0.90f, 0.96f, 1f, 1f),
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
                new Color(0.055f, 0.045f, 0.105f, 1f)
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
                new Color(0.075f, 0.20f, 0.16f, 1f)
            );
            CreatePanel(
                firstLaunchTutorialWorldPanel.rectTransform,
                "Tutorial Path",
                0f,
                -128f,
                640f,
                48f,
                new Color(0.42f, 0.27f, 0.13f, 1f)
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
                    out firstLaunchTutorialKeyboardBinding
                );
            firstLaunchTutorialHandheldBindingCard =
                CreateFirstLaunchTutorialBindingCard(
                    firstLaunchTutorialInstructionRect,
                    "Tutorial Handheld Binding Card",
                    "HANDHELD",
                    205f,
                    new Color(0.105f, 0.070f, 0.155f, 1f),
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

            firstLaunchTutorialWhiteOverlay = CreatePanel(
                pageRoot,
                "Tutorial White Boot Light",
                0f,
                0f,
                CanvasSize.x,
                CanvasSize.y,
                Color.white
            );
            firstLaunchTutorialWhiteOverlay.transform.SetAsLastSibling();

            SetFirstLaunchTutorialStep(FirstLaunchTutorialStep.WhiteBoot);
            SetTutorialExitTargetsActive(false);
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

            CreateText(
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

            if (firstLaunchTutorialFeedback != null &&
                firstLaunchTutorialFeedbackClearAt > 0f &&
                Time.unscaledTime >= firstLaunchTutorialFeedbackClearAt)
            {
                firstLaunchTutorialFeedback.text = string.Empty;
                firstLaunchTutorialFeedbackClearAt = 0f;
            }

            UpdateFirstLaunchTutorialVisualPresentation();

            if (firstLaunchTutorialTransitionOut)
            {
                UpdateFirstLaunchTutorialTransitionOut();
                return true;
            }

            if (firstLaunchTutorialExitOpen)
            {
                UpdateFirstLaunchTutorialExitAnimation();
                return true;
            }

            float elapsed = Time.unscaledTime - firstLaunchTutorialStepStartedAt;

            switch (firstLaunchTutorialStep)
            {
                case FirstLaunchTutorialStep.WhiteBoot:
                    UpdateFirstLaunchTutorialWhiteBoot(elapsed);
                    break;
                case FirstLaunchTutorialStep.EnemyArrival:
                    UpdateFirstLaunchTutorialEnemyArrival(elapsed);
                    break;
                case FirstLaunchTutorialStep.HorseShot:
                    UpdateFirstLaunchTutorialHorseShot(elapsed);
                    break;
                case FirstLaunchTutorialStep.HorseReturn:
                    UpdateFirstLaunchTutorialHorseReturn(elapsed);
                    break;
                case FirstLaunchTutorialStep.HealHorse:
                    UpdateFirstLaunchTutorialHealHold();
                    break;
                case FirstLaunchTutorialStep.SpinAttack:
                    UpdateFirstLaunchTutorialSpinHold();
                    break;
                case FirstLaunchTutorialStep.Parry:
                    UpdateFirstLaunchTutorialProjectile(elapsed);
                    break;
                case FirstLaunchTutorialStep.Grapple:
                    UpdateFirstLaunchTutorialGrappleHold();
                    break;
                case FirstLaunchTutorialStep.Completed:
                    if (elapsed >= 1.20f)
                        BeginFirstLaunchTutorialTransition(skip: false);
                    break;
            }

            return true;
        }

        private void UpdateFirstLaunchTutorialWhiteBoot(float elapsed)
        {
            float reveal = Mathf.Clamp01((elapsed - 0.30f) / 0.85f);
            if (firstLaunchTutorialWhiteOverlay != null)
            {
                Color color = firstLaunchTutorialWhiteOverlay.color;
                color.a = 1f - reveal;
                firstLaunchTutorialWhiteOverlay.color = color;
            }

            if (elapsed >= 1.25f)
                SetFirstLaunchTutorialStep(FirstLaunchTutorialStep.MountHorse);
        }

        private void UpdateFirstLaunchTutorialEnemyArrival(float elapsed)
        {
            if (firstLaunchTutorialEnemy != null)
            {
                Vector2 position = firstLaunchTutorialEnemy.rectTransform
                    .anchoredPosition;
                position.x = SnapFirstLaunchTutorialPixelValue(
                    Mathf.Lerp(370f, 265f, Mathf.Clamp01(elapsed))
                );
                firstLaunchTutorialEnemy.rectTransform.anchoredPosition = position;
            }

            if (elapsed >= 1.10f)
                SetFirstLaunchTutorialStep(FirstLaunchTutorialStep.HorseShot);
        }

        private void UpdateFirstLaunchTutorialHorseShot(float elapsed)
        {
            if (firstLaunchTutorialProjectile != null)
            {
                firstLaunchTutorialProjectile.gameObject.SetActive(true);
                Vector2 position = firstLaunchTutorialProjectile.rectTransform
                    .anchoredPosition;
                position.x = SnapFirstLaunchTutorialPixelValue(
                    Mathf.Lerp(
                        245f,
                        -72f,
                        Mathf.Clamp01(elapsed / 0.65f)
                    )
                );
                firstLaunchTutorialProjectile.rectTransform.anchoredPosition = position;
            }

            if (elapsed >= 0.68f)
            {
                if (firstLaunchTutorialProjectile != null)
                    firstLaunchTutorialProjectile.gameObject.SetActive(false);
                if (firstLaunchTutorialHorse != null)
                {
                    firstLaunchTutorialHorse.color =
                        new Color(0.62f, 0.14f, 0.16f, 1f);
                    firstLaunchTutorialHorse.rectTransform.anchoredPosition =
                        new Vector2(-280f, -126f);
                }
                if (firstLaunchTutorialPlayer != null)
                {
                    firstLaunchTutorialPlayer.rectTransform.anchoredPosition =
                        new Vector2(-130f, -118f);
                    firstLaunchTutorialPlayer.rectTransform.localRotation =
                        Quaternion.Euler(0f, 0f, 16f);
                }
                SetFirstLaunchTutorialStep(
                    FirstLaunchTutorialStep.AttackEnemy
                );
            }
        }

        private void UpdateFirstLaunchTutorialHorseReturn(float elapsed)
        {
            if (firstLaunchTutorialHorse != null)
            {
                Vector2 position = firstLaunchTutorialHorse.rectTransform
                    .anchoredPosition;
                position.x = SnapFirstLaunchTutorialPixelValue(
                    Mathf.Lerp(-310f, -72f, Mathf.Clamp01(elapsed))
                );
                firstLaunchTutorialHorse.rectTransform.anchoredPosition = position;
            }

            if (elapsed >= 1.05f)
                SetFirstLaunchTutorialStep(FirstLaunchTutorialStep.HealHorse);
        }

        private void UpdateFirstLaunchTutorialProjectile(float elapsed)
        {
            if (firstLaunchTutorialProjectile == null)
                return;

            firstLaunchTutorialProjectile.gameObject.SetActive(true);
            float cycle = Mathf.Repeat(elapsed, 1.35f) / 1.35f;
            firstLaunchTutorialProjectile.rectTransform.anchoredPosition =
                new Vector2(
                    SnapFirstLaunchTutorialPixelValue(
                        Mathf.Lerp(285f, -120f, cycle)
                    ),
                    -70f
                );
        }

        private void UpdateFirstLaunchTutorialSpinHold()
        {
            if (!IsFirstLaunchTutorialPrimaryHeld())
            {
                firstLaunchTutorialPrimaryHoldStartedAt = -1f;
                ShowFirstLaunchTutorialHoldProgress(
                    "HOLD TO CHARGE",
                    0f,
                    visible: false
                );
                return;
            }

            if (firstLaunchTutorialPrimaryHoldStartedAt < 0f)
                firstLaunchTutorialPrimaryHoldStartedAt = Time.unscaledTime;

            float held = Time.unscaledTime -
                         firstLaunchTutorialPrimaryHoldStartedAt;
            float progress = Mathf.Clamp01(held / TutorialSpinHoldSeconds);
            ShowFirstLaunchTutorialHoldProgress(
                "SPIN CHARGE",
                progress,
                visible: true
            );

            if (progress >= 1f)
            {
                ShowFirstLaunchTutorialSuccess("SPIN ATTACK READY");
                SetFirstLaunchTutorialStep(FirstLaunchTutorialStep.Parry);
            }
        }

        private void UpdateFirstLaunchTutorialHealHold()
        {
            if (!IsFirstLaunchTutorialHealHeld())
            {
                firstLaunchTutorialHealHoldStartedAt = -1f;
                ShowFirstLaunchTutorialHoldProgress(
                    "HOLD TO HEAL",
                    0f,
                    visible: false
                );
                return;
            }

            if (firstLaunchTutorialHealHoldStartedAt < 0f)
                firstLaunchTutorialHealHoldStartedAt = Time.unscaledTime;

            float held = Time.unscaledTime -
                         firstLaunchTutorialHealHoldStartedAt;
            float progress = Mathf.Clamp01(
                held / TutorialHealHoldSeconds
            );
            ShowFirstLaunchTutorialHoldProgress(
                "HEALING",
                progress,
                visible: true
            );

            if (progress < 1f)
                return;

            if (firstLaunchTutorialHorse != null)
            {
                firstLaunchTutorialHorse.color =
                    new Color(0.58f, 0.35f, 0.18f, 1f);
            }

            ShowFirstLaunchTutorialSuccess("HORSE HEALED");
            SetFirstLaunchTutorialStep(FirstLaunchTutorialStep.Move);
        }

        private void UpdateFirstLaunchTutorialGrappleHold()
        {
            if (!IsFirstLaunchTutorialHeavyHeld())
            {
                firstLaunchTutorialGrappleHoldStartedAt = -1f;
                ShowFirstLaunchTutorialHoldProgress(
                    "HOLD TO GRAPPLE",
                    0f,
                    visible: false
                );
                return;
            }

            if (firstLaunchTutorialGrappleHoldStartedAt < 0f)
                firstLaunchTutorialGrappleHoldStartedAt =
                    Time.unscaledTime;

            float held = Time.unscaledTime -
                         firstLaunchTutorialGrappleHoldStartedAt;
            float progress = Mathf.Clamp01(
                held / TutorialGrappleHoldSeconds
            );
            ShowFirstLaunchTutorialHoldProgress(
                "GRAPPLE",
                progress,
                visible: true
            );

            if (progress < 1f)
                return;

            if (firstLaunchTutorialPlayer != null)
            {
                firstLaunchTutorialPlayer.rectTransform
                    .anchoredPosition = new Vector2(230f, -118f);
            }

            ShowFirstLaunchTutorialSuccess("GRAPPLED ACROSS");
            SetFirstLaunchTutorialStep(
                FirstLaunchTutorialStep.Collectible
            );
        }

        private void ShowFirstLaunchTutorialHoldProgress(
            string label,
            float progress,
            bool visible)
        {
            if (firstLaunchTutorialFeedback == null)
                return;

            if (!visible)
            {
                if (firstLaunchTutorialFeedbackClearAt <= 0f)
                    firstLaunchTutorialFeedback.text = string.Empty;
                return;
            }

            int percent = Mathf.RoundToInt(
                Mathf.Clamp01(progress) * 100f
            );
            firstLaunchTutorialFeedback.text =
                label + "  " + percent + "%";
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
            if (!firstLaunchTutorialActive ||
                displayedPage != EffectivePage.FirstLaunchTutorial ||
                Time.unscaledTime < firstLaunchTutorialInputUnlockAt)
            {
                return;
            }

            if (ReadExitPressed())
            {
                RegisterFirstLaunchTutorialInputSource();
                if (firstLaunchTutorialExitOpen)
                    CloseFirstLaunchTutorialExitConfirmation();
                else
                    OpenFirstLaunchTutorialExitConfirmation();
                return;
            }

            if (firstLaunchTutorialExitOpen)
            {
                if (ReadUpPressed() || ReadLeftPressed())
                    SetFirstLaunchTutorialExitSelection(0);
                else if (ReadDownPressed() || ReadRightPressed())
                    SetFirstLaunchTutorialExitSelection(1);

                if (ReadFirstLaunchTutorialConfirmPressed())
                    ConfirmFirstLaunchTutorialExitSelection();
                return;
            }

            if (ReadUpPressed())
                HandleFirstLaunchTutorialAction(
                    BDModernHandheldControlTarget.ControlAction.DPadUp,
                    FirstLaunchTutorialInputSource.Keyboard
                );
            else if (ReadDownPressed())
                HandleFirstLaunchTutorialAction(
                    BDModernHandheldControlTarget.ControlAction.DPadDown,
                    FirstLaunchTutorialInputSource.Keyboard
                );
            else if (ReadLeftPressed())
                HandleFirstLaunchTutorialAction(
                    BDModernHandheldControlTarget.ControlAction.DPadLeft,
                    FirstLaunchTutorialInputSource.Keyboard
                );
            else if (ReadRightPressed())
                HandleFirstLaunchTutorialAction(
                    BDModernHandheldControlTarget.ControlAction.DPadRight,
                    FirstLaunchTutorialInputSource.Keyboard
                );

            switch (firstLaunchTutorialStep)
            {
                case FirstLaunchTutorialStep.MountHorse:
                case FirstLaunchTutorialStep.Collectible:
                    if (ReadFirstLaunchTutorialInteractPressed())
                    {
                        HandleFirstLaunchTutorialAction(
                            BDModernHandheldControlTarget.ControlAction.Confirm,
                            FirstLaunchTutorialInputSource.Keyboard
                        );
                    }
                    break;

                case FirstLaunchTutorialStep.Grapple:
                    if (ReadFirstLaunchTutorialHeavyPressed())
                    {
                        HandleFirstLaunchTutorialAction(
                            BDModernHandheldControlTarget.ControlAction.Credits,
                            FirstLaunchTutorialInputSource.Keyboard
                        );
                    }
                    break;

                case FirstLaunchTutorialStep.AttackEnemy:
                    if (ReadFirstLaunchTutorialLightPressed())
                    {
                        HandleFirstLaunchTutorialAction(
                            BDModernHandheldControlTarget.ControlAction.Primary,
                            FirstLaunchTutorialInputSource.Keyboard
                        );
                    }
                    break;

                case FirstLaunchTutorialStep.HealHorse:
                    if (ReadFirstLaunchTutorialHealPressed())
                    {
                        HandleFirstLaunchTutorialAction(
                            BDModernHandheldControlTarget.ControlAction.Progression,
                            FirstLaunchTutorialInputSource.Keyboard
                        );
                    }
                    break;

                case FirstLaunchTutorialStep.Dodge:
                    if (ReadFirstLaunchTutorialDodgePressed())
                    {
                        HandleFirstLaunchTutorialAction(
                            BDModernHandheldControlTarget.ControlAction.ContextBackSettings,
                            FirstLaunchTutorialInputSource.Keyboard
                        );
                    }
                    break;

                case FirstLaunchTutorialStep.HeavyAttack:
                    if (ReadFirstLaunchTutorialHeavyPressed())
                    {
                        HandleFirstLaunchTutorialAction(
                            BDModernHandheldControlTarget.ControlAction.Credits,
                            FirstLaunchTutorialInputSource.Keyboard
                        );
                    }
                    break;

                case FirstLaunchTutorialStep.Parry:
                    if (ReadFirstLaunchTutorialParryPressed())
                    {
                        HandleFirstLaunchTutorialAction(
                            BDModernHandheldControlTarget.ControlAction.Credits,
                            FirstLaunchTutorialInputSource.Keyboard
                        );
                    }
                    break;
            }
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
            firstLaunchTutorialInputSource = ResolveFirstLaunchTutorialSource(
                source
            );
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
                if (action == BDModernHandheldControlTarget.ControlAction.DPadUp ||
                    action == BDModernHandheldControlTarget.ControlAction.DPadLeft)
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

            switch (firstLaunchTutorialStep)
            {
                case FirstLaunchTutorialStep.MountHorse:
                    if (action ==
                        BDModernHandheldControlTarget.ControlAction.Confirm)
                    {
                        if (firstLaunchTutorialPlayer != null)
                        {
                            firstLaunchTutorialPlayer.rectTransform
                                .anchoredPosition = new Vector2(-110f, -92f);
                        }
                        ShowFirstLaunchTutorialSuccess("MOUNTED");
                        SetFirstLaunchTutorialStep(
                            FirstLaunchTutorialStep.EnemyArrival
                        );
                    }
                    break;

                case FirstLaunchTutorialStep.AttackEnemy:
                    if (action ==
                        BDModernHandheldControlTarget.ControlAction.Primary)
                    {
                        if (firstLaunchTutorialEnemy != null)
                            firstLaunchTutorialEnemy.gameObject.SetActive(false);
                        ShowFirstLaunchTutorialSuccess("ONE-HIT ATTACK");
                        SetFirstLaunchTutorialStep(
                            FirstLaunchTutorialStep.HorseReturn
                        );
                    }
                    break;

                case FirstLaunchTutorialStep.HealHorse:
                    if (action ==
                        BDModernHandheldControlTarget.ControlAction.Progression)
                    {
                        firstLaunchTutorialHealHoldStartedAt =
                            Time.unscaledTime;
                    }
                    break;

                case FirstLaunchTutorialStep.Move:
                    if (IsTutorialMovementAction(action))
                    {
                        firstLaunchTutorialMovementInputs++;
                        MoveFirstLaunchTutorialPlayer(action);
                        if (firstLaunchTutorialMovementInputs >= 4)
                        {
                            ShowFirstLaunchTutorialSuccess("MOVEMENT READY");
                            SetFirstLaunchTutorialStep(
                                FirstLaunchTutorialStep.Dodge
                            );
                        }
                    }
                    break;

                case FirstLaunchTutorialStep.Dodge:
                    if (action ==
                        BDModernHandheldControlTarget.ControlAction.ContextBackSettings)
                    {
                        if (firstLaunchTutorialPlayer != null)
                        {
                            firstLaunchTutorialPlayer.rectTransform.localScale =
                                new Vector3(1.28f, 0.72f, 1f);
                        }
                        ShowFirstLaunchTutorialSuccess("DODGED");
                        SetFirstLaunchTutorialStep(
                            FirstLaunchTutorialStep.HeavyAttack
                        );
                    }
                    break;

                case FirstLaunchTutorialStep.HeavyAttack:
                    if (action ==
                        BDModernHandheldControlTarget.ControlAction.Credits)
                    {
                        if (firstLaunchTutorialEnemy != null)
                            firstLaunchTutorialEnemy.gameObject.SetActive(false);
                        ShowFirstLaunchTutorialSuccess("HEAVY HIT");
                        SetFirstLaunchTutorialStep(
                            FirstLaunchTutorialStep.SpinAttack
                        );
                    }
                    break;

                case FirstLaunchTutorialStep.SpinAttack:
                    if (action ==
                        BDModernHandheldControlTarget.ControlAction.Primary)
                    {
                        firstLaunchTutorialPrimaryHoldStartedAt =
                            Time.unscaledTime;
                    }
                    break;

                case FirstLaunchTutorialStep.Parry:
                    if (action ==
                        BDModernHandheldControlTarget.ControlAction.Credits)
                    {
                        if (firstLaunchTutorialProjectile != null)
                            firstLaunchTutorialProjectile.gameObject.SetActive(false);
                        ShowFirstLaunchTutorialSuccess("PARRIED");
                        SetFirstLaunchTutorialStep(
                            FirstLaunchTutorialStep.Grapple
                        );
                    }
                    break;

                case FirstLaunchTutorialStep.Grapple:
                    if (action ==
                        BDModernHandheldControlTarget.ControlAction.Credits)
                    {
                        firstLaunchTutorialGrappleHoldStartedAt =
                            Time.unscaledTime;
                    }
                    break;

                case FirstLaunchTutorialStep.Collectible:
                    if (action ==
                        BDModernHandheldControlTarget.ControlAction.Confirm)
                    {
                        if (firstLaunchTutorialCollectible != null)
                        {
                            firstLaunchTutorialCollectible.gameObject
                                .SetActive(false);
                        }
                        ShowFirstLaunchTutorialSuccess("TUTORIAL COMPLETE");
                        BDFirstLaunchTutorialStateStore.MarkCompleted();
                        SetFirstLaunchTutorialStep(
                            FirstLaunchTutorialStep.Completed
                        );
                    }
                    break;
            }
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

            ConfigureFirstLaunchTutorialScene(step);
            UpdateFirstLaunchTutorialPrompt();
            BeginFirstLaunchTutorialInstructionPresentation(step);
            UpdateFirstLaunchTutorialPhysicalHighlight();
        }

        private void ConfigureFirstLaunchTutorialScene(
            FirstLaunchTutorialStep step)
        {
            SetTutorialEntityActive(firstLaunchTutorialEnemy, false);
            SetTutorialEntityActive(firstLaunchTutorialHazard, false);
            SetTutorialEntityActive(firstLaunchTutorialProjectile, false);
            SetTutorialEntityActive(firstLaunchTutorialCollectible, false);
            SetTutorialEntityActive(firstLaunchTutorialGap, false);

            if (firstLaunchTutorialPlayer != null)
            {
                firstLaunchTutorialPlayer.rectTransform.localScale = Vector3.one;
                firstLaunchTutorialPlayer.rectTransform.localRotation =
                    Quaternion.identity;
            }

            switch (step)
            {
                case FirstLaunchTutorialStep.EnemyArrival:
                case FirstLaunchTutorialStep.HorseShot:
                case FirstLaunchTutorialStep.AttackEnemy:
                    SetTutorialEntityActive(firstLaunchTutorialEnemy, true);
                    if (firstLaunchTutorialEnemy != null)
                    {
                        firstLaunchTutorialEnemy.rectTransform.anchoredPosition =
                            new Vector2(300f, -120f);
                    }
                    break;

                case FirstLaunchTutorialStep.Dodge:
                    SetTutorialEntityActive(firstLaunchTutorialHazard, true);
                    break;

                case FirstLaunchTutorialStep.HeavyAttack:
                    SetTutorialEntityActive(firstLaunchTutorialEnemy, true);
                    if (firstLaunchTutorialEnemy != null)
                    {
                        firstLaunchTutorialEnemy.rectTransform.anchoredPosition =
                            new Vector2(205f, -120f);
                        firstLaunchTutorialEnemy.color =
                            new Color(0.44f, 0.30f, 0.58f, 1f);
                    }
                    break;

                case FirstLaunchTutorialStep.SpinAttack:
                    SetTutorialEntityActive(firstLaunchTutorialEnemy, true);
                    SetTutorialEntityActive(firstLaunchTutorialHazard, true);
                    if (firstLaunchTutorialEnemy != null)
                    {
                        firstLaunchTutorialEnemy.rectTransform.anchoredPosition =
                            new Vector2(160f, -120f);
                        firstLaunchTutorialEnemy.color =
                            new Color(0.72f, 0.12f, 0.20f, 1f);
                    }
                    if (firstLaunchTutorialHazard != null)
                    {
                        firstLaunchTutorialHazard.rectTransform.anchoredPosition =
                            new Vector2(-20f, -120f);
                        firstLaunchTutorialHazard.color =
                            new Color(0.72f, 0.12f, 0.20f, 1f);
                    }
                    break;

                case FirstLaunchTutorialStep.Parry:
                    SetTutorialEntityActive(firstLaunchTutorialProjectile, true);
                    break;

                case FirstLaunchTutorialStep.Grapple:
                    SetTutorialEntityActive(firstLaunchTutorialGap, true);
                    break;

                case FirstLaunchTutorialStep.Collectible:
                    SetTutorialEntityActive(firstLaunchTutorialCollectible, true);
                    if (firstLaunchTutorialCollectible != null)
                    {
                        firstLaunchTutorialCollectible.rectTransform
                            .anchoredPosition = new Vector2(250f, -96f);
                    }
                    break;
            }
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

            if (firstLaunchTutorialKeyboardBindingCard != null)
            {
                firstLaunchTutorialKeyboardBindingCard.gameObject.SetActive(
                    hasBindings
                );
            }
            if (firstLaunchTutorialHandheldBindingCard != null)
            {
                firstLaunchTutorialHandheldBindingCard.gameObject.SetActive(
                    hasBindings
                );
            }
            if (firstLaunchTutorialBindingDivider != null)
            {
                firstLaunchTutorialBindingDivider.gameObject.SetActive(
                    hasBindings
                );
            }

            if (firstLaunchTutorialDetail != null)
            {
                firstLaunchTutorialDetail.rectTransform.anchoredPosition =
                    hasBindings
                        ? new Vector2(0f, 25f)
                        : new Vector2(0f, -12f);
                firstLaunchTutorialDetail.rectTransform.sizeDelta =
                    hasBindings
                        ? new Vector2(748f, 32f)
                        : new Vector2(748f, 70f);
            }

            if (!hasBindings)
                return;

            if (firstLaunchTutorialKeyboardBinding != null)
            {
                firstLaunchTutorialKeyboardBinding.text =
                    ResolveFirstLaunchTutorialKeyboardBinding(action);
            }
            if (firstLaunchTutorialHandheldBinding != null)
            {
                firstLaunchTutorialHandheldBinding.text =
                    ResolveFirstLaunchTutorialHandheldBinding(action);
            }
        }

        private string ResolveFirstLaunchTutorialProgressLabel()
        {
            int index = Mathf.Clamp(
                (int)firstLaunchTutorialStep,
                0,
                (int)FirstLaunchTutorialStep.Completed
            );
            int total = (int)FirstLaunchTutorialStep.Completed;
            return "LESSON " + Mathf.Min(index + 1, total) +
                   " / " + total;
        }

        private string ResolveFirstLaunchTutorialPrompt()
        {
            switch (firstLaunchTutorialStep)
            {
                case FirstLaunchTutorialStep.WhiteBoot:
                    return "WAKING THE HANDHELD...";
                case FirstLaunchTutorialStep.MountHorse:
                    return "MOUNT THE HORSE";
                case FirstLaunchTutorialStep.EnemyArrival:
                    return "STAY READY";
                case FirstLaunchTutorialStep.HorseShot:
                    return "THE HORSE IS HIT";
                case FirstLaunchTutorialStep.AttackEnemy:
                    return "ATTACK THE ENEMY";
                case FirstLaunchTutorialStep.HorseReturn:
                    return "THE HORSE IS RETURNING";
                case FirstLaunchTutorialStep.HealHorse:
                    return "HEAL THE HORSE";
                case FirstLaunchTutorialStep.Move:
                    return "MOVE THROUGH THE ROOM";
                case FirstLaunchTutorialStep.Dodge:
                    return "DODGE THE HAZARD";
                case FirstLaunchTutorialStep.HeavyAttack:
                    return "BREAK THE ARMORED TARGET";
                case FirstLaunchTutorialStep.SpinAttack:
                    return "CHARGE A SPIN ATTACK";
                case FirstLaunchTutorialStep.Parry:
                    return "PARRY THE PROJECTILE";
                case FirstLaunchTutorialStep.Grapple:
                    return "CROSS THE GAP";
                case FirstLaunchTutorialStep.Collectible:
                    return "COLLECT THE MEMORY";
                default:
                    return "YOU ARE READY";
            }
        }

        private string ResolveFirstLaunchTutorialDetail()
        {
            switch (firstLaunchTutorialStep)
            {
                case FirstLaunchTutorialStep.WhiteBoot:
                    return "A short interactive lesson is loading.";
                case FirstLaunchTutorialStep.MountHorse:
                    return "Approach the horse and use either control route.";
                case FirstLaunchTutorialStep.EnemyArrival:
                    return "Watch the clean scripted encounter.";
                case FirstLaunchTutorialStep.HorseShot:
                    return "The next action unlocks when the event completes.";
                case FirstLaunchTutorialStep.AttackEnemy:
                    return "Use a quick light attack.";
                case FirstLaunchTutorialStep.HorseReturn:
                    return "The injured horse returns automatically.";
                case FirstLaunchTutorialStep.HealHorse:
                    return "Keep holding until the healing meter completes.";
                case FirstLaunchTutorialStep.Move:
                    return "Use four movement inputs in any direction.";
                case FirstLaunchTutorialStep.Dodge:
                    return "Evade before touching the hazard.";
                case FirstLaunchTutorialStep.HeavyAttack:
                    return "Use the heavy-action input once.";
                case FirstLaunchTutorialStep.SpinAttack:
                    return "Keep holding until the spin is fully charged.";
                case FirstLaunchTutorialStep.Parry:
                    return "Time the parry as the projectile approaches.";
                case FirstLaunchTutorialStep.Grapple:
                    return "Keep holding the heavy-action input to cross.";
                case FirstLaunchTutorialStep.Collectible:
                    return "Interact with the memory to finish the lesson.";
                default:
                    return "The main menu will appear next.";
            }
        }

        private string ResolveFirstLaunchTutorialBindingAction()
        {
            switch (firstLaunchTutorialStep)
            {
                case FirstLaunchTutorialStep.MountHorse:
                case FirstLaunchTutorialStep.Collectible:
                    return "INTERACT";
                case FirstLaunchTutorialStep.AttackEnemy:
                    return "ATTACK";
                case FirstLaunchTutorialStep.HealHorse:
                    return "HEAL";
                case FirstLaunchTutorialStep.Move:
                    return "MOVE";
                case FirstLaunchTutorialStep.Dodge:
                    return "DODGE";
                case FirstLaunchTutorialStep.HeavyAttack:
                    return "HEAVY";
                case FirstLaunchTutorialStep.SpinAttack:
                    return "SPIN";
                case FirstLaunchTutorialStep.Parry:
                    return "PARRY";
                case FirstLaunchTutorialStep.Grapple:
                    return "GRAPPLE";
                default:
                    return string.Empty;
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
                case "GRAPPLE":
                    return "HOLD K / RIGHT CLICK";
                case "ATTACK":
                    return "J / LEFT CLICK";
                case "HEAL":
                    return "HOLD F";
                case "DODGE":
                    return "SPACE";
                case "HEAVY":
                    return "K / RIGHT CLICK";
                case "PARRY":
                    return "Q";
                case "SPIN":
                    return "HOLD J / LEFT CLICK";
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
                case "GRAPPLE":
                    return "HOLD Y";
                case "ATTACK":
                    return "X";
                case "HEAL":
                    return "HOLD A";
                case "DODGE":
                    return "B";
                case "HEAVY":
                    return "Y";
                case "PARRY":
                    return "Y";
                case "SPIN":
                    return "HOLD X";
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
            switch (firstLaunchTutorialStep)
            {
                case FirstLaunchTutorialStep.MountHorse:
                case FirstLaunchTutorialStep.Collectible:
                    return BDModernHandheldControlTarget.ControlAction.Confirm;
                case FirstLaunchTutorialStep.Grapple:
                    return BDModernHandheldControlTarget.ControlAction.Credits;
                case FirstLaunchTutorialStep.AttackEnemy:
                case FirstLaunchTutorialStep.SpinAttack:
                    return BDModernHandheldControlTarget.ControlAction.Primary;
                case FirstLaunchTutorialStep.HealHorse:
                    return BDModernHandheldControlTarget.ControlAction.Progression;
                case FirstLaunchTutorialStep.Move:
                    return BDModernHandheldControlTarget.ControlAction.DPadRight;
                case FirstLaunchTutorialStep.Dodge:
                    return BDModernHandheldControlTarget.ControlAction.ContextBackSettings;
                case FirstLaunchTutorialStep.HeavyAttack:
                case FirstLaunchTutorialStep.Parry:
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

        private static bool IsTutorialMovementAction(
            BDModernHandheldControlTarget.ControlAction action)
        {
            return action == BDModernHandheldControlTarget.ControlAction.DPadUp ||
                   action == BDModernHandheldControlTarget.ControlAction.DPadDown ||
                   action == BDModernHandheldControlTarget.ControlAction.DPadLeft ||
                   action == BDModernHandheldControlTarget.ControlAction.DPadRight;
        }

        private void MoveFirstLaunchTutorialPlayer(
            BDModernHandheldControlTarget.ControlAction action)
        {
            if (firstLaunchTutorialPlayer == null)
                return;

            Vector2 delta = Vector2.zero;
            switch (action)
            {
                case BDModernHandheldControlTarget.ControlAction.DPadUp:
                    delta.y = 18f;
                    break;
                case BDModernHandheldControlTarget.ControlAction.DPadDown:
                    delta.y = -18f;
                    break;
                case BDModernHandheldControlTarget.ControlAction.DPadLeft:
                    delta.x = -28f;
                    break;
                case BDModernHandheldControlTarget.ControlAction.DPadRight:
                    delta.x = 28f;
                    break;
            }

            Vector2 position = firstLaunchTutorialPlayer.rectTransform
                .anchoredPosition + delta;
            position.x = Mathf.Clamp(position.x, -310f, 300f);
            position.y = Mathf.Clamp(position.y, -145f, 80f);
            firstLaunchTutorialPlayer.rectTransform.anchoredPosition = position;
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
                Gamepad.current.buttonSouth.wasPressedThisFrame)
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
                Gamepad.current.buttonSouth.wasPressedThisFrame)
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

        private static bool ReadFirstLaunchTutorialDodgePressed()
        {
#if ENABLE_INPUT_SYSTEM
            if (Keyboard.current != null &&
                Keyboard.current.spaceKey.wasPressedThisFrame)
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
            if (Input.GetKeyDown(KeyCode.Space))
                return true;
#endif
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
#if ENABLE_INPUT_SYSTEM
            if (Keyboard.current != null &&
                Keyboard.current.qKey.wasPressedThisFrame)
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
            if (Input.GetKeyDown(KeyCode.Q))
                return true;
#endif
            return false;
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
                Gamepad.current.buttonSouth.isPressed)
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
