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
        private const float TutorialWorldMinX = -920f;
        private const float TutorialWorldMaxX = 4250f;
        private const float TutorialWorldMinY = -126f;
        private const float TutorialWorldMaxY = 56f;
        private const float TutorialWorldViewportHalfWidth = 390f;
        private const float TutorialFootMoveSpeed = 176f;
        private const float TutorialMountedMoveSpeed = 270f;
        private const float TutorialMountRange = 104f;
        private const float TutorialAttackRange = 172f;
        private const float TutorialInstructionTriggerRange = 238f;

        private const float TutorialHorseStartX = -650f;
        private const float TutorialAmbushTriggerX = -350f;
        private const float TutorialAmbushEnemyX = -165f;
        private const float TutorialRangedTargetX = 1660f;
        private const float TutorialDismountMarkerX = 2240f;
        private const float TutorialHazardX = 860f;
        private const float TutorialHeavyTargetX = 680f;
        private const float TutorialSpinTargetX = 2400f;
        private const float TutorialParryTargetX = 1100f;
        private const float TutorialGapX = 2580f;
        private const float TutorialCollectibleX = 3980f;

        private RectTransform firstLaunchTutorialCourseRoot;
        private readonly List<Image> firstLaunchTutorialCourseDecorations =
            new List<Image>();

        private Vector2 firstLaunchTutorialPlayerWorldPosition;
        private Vector2 firstLaunchTutorialHorseWorldPosition;
        private Vector2 firstLaunchTutorialEnemyWorldPosition;
        private Vector2 firstLaunchTutorialHazardWorldPosition;
        private Vector2 firstLaunchTutorialProjectileWorldPosition;
        private Vector2 firstLaunchTutorialCollectibleWorldPosition;
        private Vector2 firstLaunchTutorialGapWorldPosition;
        private Vector2 firstLaunchTutorialLastMoveDirection =
            Vector2.right;
        private Vector2 firstLaunchTutorialPhysicalMoveLatch;

        private float firstLaunchTutorialCameraWorldX;
        private float firstLaunchTutorialTravelDistance;
        private float firstLaunchTutorialPhysicalMoveLatchUntil;
        private float firstLaunchTutorialScriptStartedAt;
        private float firstLaunchTutorialParryProjectileProgress;
        private float firstLaunchTutorialInstructionVisibility;
        private float firstLaunchTutorialNextGateFeedbackAt;
        private bool firstLaunchTutorialInstructionRequested;
        private bool firstLaunchTutorialMounted;
        private bool firstLaunchTutorialHorseInjured;
        private bool firstLaunchTutorialParryProjectileActive;
        private bool firstLaunchTutorialMovementActive;

        private void InitializeFirstLaunchTutorialFreePlayCourse()
        {
            if (firstLaunchTutorialWorldPanel == null)
                return;

            Mask mask =
                firstLaunchTutorialWorldPanel.GetComponent<Mask>();
            if (mask == null)
                mask = firstLaunchTutorialWorldPanel.gameObject
                    .AddComponent<Mask>();
            mask.showMaskGraphic = true;

            List<RectTransform> existingChildren =
                new List<RectTransform>();
            RectTransform worldRect =
                firstLaunchTutorialWorldPanel.rectTransform;
            for (int index = 0; index < worldRect.childCount; index++)
            {
                RectTransform child =
                    worldRect.GetChild(index) as RectTransform;
                if (child != null)
                    existingChildren.Add(child);
            }

            GameObject courseObject = new GameObject(
                "Tutorial Wide Course",
                typeof(RectTransform)
            );
            courseObject.layer =
                firstLaunchTutorialWorldPanel.gameObject.layer;
            courseObject.transform.SetParent(worldRect, false);

            firstLaunchTutorialCourseRoot =
                courseObject.GetComponent<RectTransform>();
            firstLaunchTutorialCourseRoot.anchorMin =
                new Vector2(0.5f, 0.5f);
            firstLaunchTutorialCourseRoot.anchorMax =
                new Vector2(0.5f, 0.5f);
            firstLaunchTutorialCourseRoot.pivot =
                new Vector2(0.5f, 0.5f);
            firstLaunchTutorialCourseRoot.sizeDelta =
                new Vector2(
                    TutorialWorldMaxX - TutorialWorldMinX + 240f,
                    390f
                );
            firstLaunchTutorialCourseRoot.anchoredPosition =
                Vector2.zero;

            for (int index = 0;
                 index < existingChildren.Count;
                 index++)
            {
                RectTransform child = existingChildren[index];
                Vector2 anchored = child.anchoredPosition;
                Vector2 size = child.sizeDelta;
                Vector3 scale = child.localScale;
                Quaternion rotation = child.localRotation;

                child.SetParent(firstLaunchTutorialCourseRoot, false);
                child.anchoredPosition = anchored;
                child.sizeDelta = size;
                child.localScale = scale;
                child.localRotation = rotation;
            }

            ResizeFirstLaunchTutorialCourseSurface(
                "Tutorial Ground",
                -36f,
                112f,
                new Color(0.055f, 0.18f, 0.15f, 1f)
            );
            ResizeFirstLaunchTutorialCourseSurface(
                "Tutorial Path",
                -104f,
                58f,
                new Color(0.43f, 0.28f, 0.14f, 1f)
            );

            BuildFirstLaunchTutorialCourseDecorations();
            BuildFirstLaunchTutorialActionPresentation(
                firstLaunchTutorialCourseRoot
            );

            firstLaunchTutorialPlayerWorldPosition =
                new Vector2(-820f, -108f);
            firstLaunchTutorialHorseWorldPosition =
                new Vector2(TutorialHorseStartX, -116f);
            firstLaunchTutorialEnemyWorldPosition =
                new Vector2(TutorialAmbushEnemyX, -108f);
            firstLaunchTutorialHazardWorldPosition =
                new Vector2(TutorialHazardX, -142f);
            firstLaunchTutorialProjectileWorldPosition =
                new Vector2(TutorialAmbushEnemyX, -68f);
            firstLaunchTutorialCollectibleWorldPosition =
                new Vector2(TutorialCollectibleX, -90f);
            firstLaunchTutorialGapWorldPosition =
                new Vector2(TutorialGapX, -160f);
            firstLaunchTutorialCameraWorldX =
                firstLaunchTutorialPlayerWorldPosition.x;
            firstLaunchTutorialTravelDistance = 0f;
            firstLaunchTutorialPhysicalMoveLatch = Vector2.zero;
            firstLaunchTutorialPhysicalMoveLatchUntil = 0f;
            firstLaunchTutorialMounted = false;
            firstLaunchTutorialHorseInjured = false;
            firstLaunchTutorialParryProjectileActive = false;
            firstLaunchTutorialInstructionRequested = true;
            firstLaunchTutorialInstructionVisibility = 1f;
            firstLaunchTutorialNextGateFeedbackAt = 0f;

            InitializeFirstLaunchTutorialProductionCourse();
            RenderFirstLaunchTutorialFreePlayCourse(force: true);
        }

        private void DisposeFirstLaunchTutorialFreePlayCourse()
        {
            DisposeFirstLaunchTutorialProductionCourse();
            firstLaunchTutorialCourseDecorations.Clear();
            firstLaunchTutorialCourseRoot = null;
            firstLaunchTutorialPhysicalMoveLatch = Vector2.zero;
            firstLaunchTutorialPhysicalMoveLatchUntil = 0f;
            firstLaunchTutorialMounted = false;
            firstLaunchTutorialHorseInjured = false;
            firstLaunchTutorialParryProjectileActive = false;
            firstLaunchTutorialMovementActive = false;
            firstLaunchTutorialInstructionRequested = false;
            firstLaunchTutorialInstructionVisibility = 0f;
            firstLaunchTutorialNextGateFeedbackAt = 0f;
        }

        private void ResizeFirstLaunchTutorialCourseSurface(
            string name,
            float y,
            float height,
            Color color)
        {
            if (firstLaunchTutorialCourseRoot == null)
                return;

            Transform value =
                firstLaunchTutorialCourseRoot.Find(name);
            RectTransform rect = value as RectTransform;
            if (rect == null)
                return;

            rect.anchoredPosition = new Vector2(
                (TutorialWorldMinX + TutorialWorldMaxX) * 0.5f,
                y
            );
            rect.sizeDelta = new Vector2(
                TutorialWorldMaxX - TutorialWorldMinX + 160f,
                height
            );

            Image image = rect.GetComponent<Image>();
            if (image != null)
                image.color = color;
        }

        private void BuildFirstLaunchTutorialCourseDecorations()
        {
            if (firstLaunchTutorialCourseRoot == null)
                return;

            Color treeDark =
                new Color(0.025f, 0.15f, 0.15f, 0.98f);
            Color treeLight =
                new Color(0.055f, 0.27f, 0.23f, 0.98f);
            Color stone =
                new Color(0.22f, 0.20f, 0.34f, 0.94f);
            Color marker =
                new Color(0.96f, 0.58f, 0.20f, 0.88f);

            float[] stations =
            {
                -820f,
                TutorialHorseStartX,
                TutorialAmbushTriggerX,
                TutorialRangedTargetX,
                TutorialDismountMarkerX,
                TutorialHazardX,
                TutorialHeavyTargetX,
                TutorialSpinTargetX,
                TutorialParryTargetX,
                TutorialGapX,
                TutorialHazardStationX,
                TutorialMountedStationX,
                TutorialSecretBranchX,
                TutorialCombinedStationX,
                TutorialMiniBossStationX,
                TutorialCollectibleX
            };

            for (int index = 0; index < stations.Length; index++)
            {
                float x = stations[index];
                float side = index % 2 == 0 ? 1f : -1f;

                firstLaunchTutorialCourseDecorations.Add(
                    CreateFirstLaunchTutorialCourseDecoration(
                        "Course Tree " + index,
                        x + 54f * side,
                        112f,
                        54f,
                        76f,
                        index % 3 == 0 ? treeLight : treeDark
                    )
                );
                firstLaunchTutorialCourseDecorations.Add(
                    CreateFirstLaunchTutorialCourseDecoration(
                        "Course Stone " + index,
                        x - 68f * side,
                        -105f,
                        30f,
                        18f,
                        stone
                    )
                );
            }

            float[] lessonMarkers =
            {
                TutorialAmbushTriggerX,
                TutorialRangedTargetX,
                TutorialDismountMarkerX,
                TutorialHazardX,
                TutorialHeavyTargetX,
                TutorialSpinTargetX,
                TutorialParryTargetX,
                TutorialGapX,
                TutorialHazardStationX,
                TutorialMountedStationX,
                TutorialCombinedStationX,
                TutorialMiniBossStationX,
                TutorialCollectibleX
            };

            for (int index = 0;
                 index < lessonMarkers.Length;
                 index++)
            {
                firstLaunchTutorialCourseDecorations.Add(
                    CreateFirstLaunchTutorialCourseDecoration(
                        "Course Lesson Marker " + index,
                        lessonMarkers[index],
                        -101f,
                        18f,
                        8f,
                        marker
                    )
                );
            }
        }

        private Image CreateFirstLaunchTutorialCourseDecoration(
            string name,
            float x,
            float y,
            float width,
            float height,
            Color color)
        {
            Image image = CreatePanel(
                firstLaunchTutorialCourseRoot,
                name,
                x,
                y,
                width,
                height,
                color
            );
            image.raycastTarget = false;
            image.sprite = null;
            image.type = Image.Type.Simple;
            return image;
        }

        private void UpdateFirstLaunchTutorialFreePlay(float elapsed)
        {
            if (firstLaunchTutorialCourseRoot == null)
                return;

            if (firstLaunchTutorialStep ==
                    FirstLaunchTutorialStep.WhiteBoot)
            {
                UpdateFirstLaunchTutorialFreePlayWhiteBoot(elapsed);
                RenderFirstLaunchTutorialFreePlayCourse(force: false);
                return;
            }

            if (firstLaunchTutorialStep ==
                    FirstLaunchTutorialStep.Completed)
            {
                SetFirstLaunchTutorialInstructionRequested(true);
                if (elapsed >= 1.20f)
                    BeginFirstLaunchTutorialTransition(skip: false);
                RenderFirstLaunchTutorialFreePlayCourse(force: false);
                return;
            }

            firstLaunchTutorialMovementActive = false;

            bool scripted =
                firstLaunchTutorialStep ==
                    FirstLaunchTutorialStep.EnemyArrival ||
                firstLaunchTutorialStep ==
                    FirstLaunchTutorialStep.HorseShot ||
                firstLaunchTutorialStep ==
                    FirstLaunchTutorialStep.HorseReturn;

            if (!scripted &&
                !IsFirstLaunchTutorialActionInputLocked())
            {
                UpdateFirstLaunchTutorialContinuousMovement();
            }

            UpdateFirstLaunchTutorialHeldActions();
            UpdateFirstLaunchTutorialProductionCourse(elapsed);
            UpdateFirstLaunchTutorialCompletionContact(elapsed);

            switch (firstLaunchTutorialStep)
            {
                case FirstLaunchTutorialStep.EnemyArrival:
                    UpdateFirstLaunchTutorialAmbush(elapsed);
                    break;
                case FirstLaunchTutorialStep.HorseShot:
                    UpdateFirstLaunchTutorialHorseShotEvent(elapsed);
                    break;
                case FirstLaunchTutorialStep.HorseReturn:
                    UpdateFirstLaunchTutorialHorseReturnEvent(elapsed);
                    break;
                case FirstLaunchTutorialStep.Parry:
                    UpdateFirstLaunchTutorialParryProjectile();
                    break;
            }

            UpdateFirstLaunchTutorialLessonAvailability(elapsed);
            RenderFirstLaunchTutorialFreePlayCourse(force: false);
        }

        private void UpdateFirstLaunchTutorialFreePlayWhiteBoot(
            float elapsed)
        {
            float reveal = Mathf.Clamp01(
                (elapsed - 0.30f) / 0.85f
            );
            if (firstLaunchTutorialWhiteOverlay != null)
            {
                Color color = firstLaunchTutorialWhiteOverlay.color;
                color.a = 1f - reveal;
                firstLaunchTutorialWhiteOverlay.color = color;
            }

            if (elapsed >= 1.25f)
                SetFirstLaunchTutorialStep(FirstLaunchTutorialStep.Move);
        }

        private void UpdateFirstLaunchTutorialContinuousMovement()
        {
            FirstLaunchTutorialInputSource source;
            Vector2 movement =
                ReadFirstLaunchTutorialMovementVector(out source);
            bool hasMovementInput = movement.sqrMagnitude > 0.0001f;
            float speed = ResolveFirstLaunchTutorialTravelSpeed(
                hasMovementInput ? movement : Vector2.zero
            );

            if (!hasMovementInput)
            {
                if (!firstLaunchTutorialMounted || speed <= 0.01f)
                    return;
                float inertiaDirection = Mathf.Abs(firstLaunchTutorialLastMoveDirection.x) > 0.01f
                    ? Mathf.Sign(firstLaunchTutorialLastMoveDirection.x)
                    : 1f;
                movement = new Vector2(inertiaDirection, 0f);
            }
            else
            {
                SetFirstLaunchTutorialInputSource(source);
            }

            if (movement.sqrMagnitude > 1f)
                movement.Normalize();
            Vector2 delta =
                new Vector2(movement.x, 0f) *
                speed *
                Time.unscaledDeltaTime;

            float maximumPlayerX = Mathf.Min(
                TutorialWorldMaxX,
                ResolveFirstLaunchTutorialMaximumPlayerX()
            );
            float currentPlayerX =
                firstLaunchTutorialPlayerWorldPosition.x;
            float requestedPlayerX = currentPlayerX + delta.x;
            float collisionResolvedX =
                ResolveFirstLaunchTutorialCollisionX(
                    currentPlayerX,
                    requestedPlayerX
                );
            firstLaunchTutorialPlayerWorldPosition.x = Mathf.Clamp(
                collisionResolvedX,
                TutorialWorldMinX,
                maximumPlayerX
            );

            if (movement.x > 0.05f &&
                requestedPlayerX > maximumPlayerX + 0.5f)
            {
                NotifyFirstLaunchTutorialLessonGate();
            }

            if (firstLaunchTutorialStep == FirstLaunchTutorialStep.Jump &&
                firstLaunchTutorialGrounded &&
                firstLaunchTutorialPlayerWorldPosition.x > TutorialJumpObstacleX - 44f &&
                firstLaunchTutorialPlayerWorldPosition.x < TutorialJumpObstacleX + 44f)
            {
                firstLaunchTutorialPlayerWorldPosition.x =
                    movement.x >= 0f
                        ? TutorialJumpObstacleX - 44f
                        : TutorialJumpObstacleX + 44f;
            }

            if (firstLaunchTutorialMounted)
            {
                firstLaunchTutorialHorseWorldPosition =
                    firstLaunchTutorialPlayerWorldPosition;
            }

            firstLaunchTutorialMovementActive = true;
            firstLaunchTutorialLastMoveDirection = movement.normalized;
            firstLaunchTutorialTravelDistance += delta.magnitude;

            if (firstLaunchTutorialStep ==
                    FirstLaunchTutorialStep.Move &&
                firstLaunchTutorialTravelDistance >= 118f)
            {
                ShowFirstLaunchTutorialSuccess("KEEP EXPLORING");
                SetFirstLaunchTutorialStep(
                    FirstLaunchTutorialStep.Jump
                );
            }

            if (firstLaunchTutorialStep ==
                    FirstLaunchTutorialStep.RideHorse &&
                firstLaunchTutorialMounted &&
                firstLaunchTutorialPlayerWorldPosition.x >=
                    TutorialAmbushTriggerX)
            {
                SetFirstLaunchTutorialStep(
                    FirstLaunchTutorialStep.EnemyArrival
                );
            }
        }

        private Vector2 ReadFirstLaunchTutorialMovementVector(
            out FirstLaunchTutorialInputSource source)
        {
            source = FirstLaunchTutorialInputSource.Keyboard;
            Vector2 keyboard = Vector2.zero;
            Vector2 controller = Vector2.zero;
            Vector2 physical = Vector2.zero;

#if ENABLE_INPUT_SYSTEM
            Keyboard currentKeyboard = Keyboard.current;
            if (currentKeyboard != null)
            {
                if (currentKeyboard.aKey.isPressed ||
                    currentKeyboard.leftArrowKey.isPressed)
                {
                    keyboard.x -= 1f;
                }
                if (currentKeyboard.dKey.isPressed ||
                    currentKeyboard.rightArrowKey.isPressed)
                {
                    keyboard.x += 1f;
                }
                if (currentKeyboard.sKey.isPressed ||
                    currentKeyboard.downArrowKey.isPressed)
                {
                    keyboard.y -= 1f;
                }
                if (currentKeyboard.wKey.isPressed ||
                    currentKeyboard.upArrowKey.isPressed)
                {
                    keyboard.y += 1f;
                }
            }

            Gamepad currentGamepad = Gamepad.current;
            if (currentGamepad != null)
            {
                controller =
                    currentGamepad.leftStick.ReadValue();
                if (controller.sqrMagnitude < 0.04f)
                    controller = currentGamepad.dpad.ReadValue();
            }

            bool pointerHeld =
                Mouse.current != null &&
                Mouse.current.leftButton.isPressed;
            bool touchHeld =
                Touchscreen.current != null &&
                Touchscreen.current.primaryTouch.press.isPressed;
            if ((pointerHeld || touchHeld) &&
                hoveredTarget != null)
            {
                physical = ResolveFirstLaunchTutorialMovementAction(
                    hoveredTarget.Action
                );
            }
#endif

#if ENABLE_LEGACY_INPUT_MANAGER
            if (Input.GetKey(KeyCode.A) ||
                Input.GetKey(KeyCode.LeftArrow))
            {
                keyboard.x -= 1f;
            }
            if (Input.GetKey(KeyCode.D) ||
                Input.GetKey(KeyCode.RightArrow))
            {
                keyboard.x += 1f;
            }
            if (Input.GetKey(KeyCode.S) ||
                Input.GetKey(KeyCode.DownArrow))
            {
                keyboard.y -= 1f;
            }
            if (Input.GetKey(KeyCode.W) ||
                Input.GetKey(KeyCode.UpArrow))
            {
                keyboard.y += 1f;
            }

            if (Input.GetMouseButton(0) &&
                hoveredTarget != null)
            {
                physical = ResolveFirstLaunchTutorialMovementAction(
                    hoveredTarget.Action
                );
            }
#endif

            if (Time.unscaledTime <
                    firstLaunchTutorialPhysicalMoveLatchUntil)
            {
                physical += firstLaunchTutorialPhysicalMoveLatch;
            }

            if (physical.sqrMagnitude > 0.0001f)
            {
                source = FirstLaunchTutorialInputSource.Handheld;
                return Vector2.ClampMagnitude(physical, 1f);
            }

            if (keyboard.sqrMagnitude > 0.0001f)
            {
                source = FirstLaunchTutorialInputSource.Keyboard;
                return Vector2.ClampMagnitude(keyboard, 1f);
            }

            if (controller.sqrMagnitude > 0.0001f)
            {
                source = FirstLaunchTutorialInputSource.Gamepad;
                return Vector2.ClampMagnitude(controller, 1f);
            }

            return Vector2.zero;
        }

        private float ResolveFirstLaunchTutorialMaximumPlayerX()
        {
            switch (firstLaunchTutorialStep)
            {
                case FirstLaunchTutorialStep.WhiteBoot:
                case FirstLaunchTutorialStep.Move:
                    return TutorialJumpObstacleX + 20f;
                case FirstLaunchTutorialStep.Jump:
                case FirstLaunchTutorialStep.MountHorse:
                    return TutorialHorseStartX + 170f;
                case FirstLaunchTutorialStep.RideHorse:
                    return TutorialAmbushTriggerX + 70f;
                case FirstLaunchTutorialStep.EnemyArrival:
                case FirstLaunchTutorialStep.HorseShot:
                    return firstLaunchTutorialPlayerWorldPosition.x;
                case FirstLaunchTutorialStep.AttackEnemy:
                case FirstLaunchTutorialStep.HeavyAttack:
                case FirstLaunchTutorialStep.Dodge:
                case FirstLaunchTutorialStep.Parry:
                case FirstLaunchTutorialStep.HorseReturn:
                case FirstLaunchTutorialStep.HealHorse:
                case FirstLaunchTutorialStep.RemountHorse:
                    return TutorialParryTargetX + 220f;
                case FirstLaunchTutorialStep.RangedAttack:
                case FirstLaunchTutorialStep.Reload:
                case FirstLaunchTutorialStep.ChargedShot:
                case FirstLaunchTutorialStep.MountedImpact:
                    return TutorialMountedStationX + 420f;
                case FirstLaunchTutorialStep.DismountHorse:
                    return TutorialSpinTargetX + 160f;
                case FirstLaunchTutorialStep.SpinAttack:
                    return TutorialSpinTargetX + 260f;
                case FirstLaunchTutorialStep.Grapple:
                    return TutorialGapX + 240f;
                case FirstLaunchTutorialStep.HazardKnockback:
                    return TutorialHazardStationX + 340f;
                case FirstLaunchTutorialStep.SidePath:
                    return TutorialSecretBranchX + 360f;
                case FirstLaunchTutorialStep.CombinedEncounter:
                    return TutorialMiniBossStationX - 160f;
                case FirstLaunchTutorialStep.MiniBossIntro:
                case FirstLaunchTutorialStep.MiniBossPhaseOne:
                case FirstLaunchTutorialStep.MiniBossPhaseTwo:
                    return TutorialFinishX - 80f;
                case FirstLaunchTutorialStep.MiniBossDefeated:
                case FirstLaunchTutorialStep.Collectible:
                case FirstLaunchTutorialStep.Completed:
                    return TutorialWorldMaxX;
                default:
                    return TutorialWorldMaxX;
            }
        }

        private void NotifyFirstLaunchTutorialLessonGate()
        {
            if (Time.unscaledTime < firstLaunchTutorialNextGateFeedbackAt)
                return;

            firstLaunchTutorialNextGateFeedbackAt =
                Time.unscaledTime + 1.25f;
            SetFirstLaunchTutorialInstructionRequested(true);
            ShowFirstLaunchTutorialSuccess(
                "COMPLETE THE CURRENT LESSON TO CONTINUE"
            );
        }

        private static Vector2 ResolveFirstLaunchTutorialMovementAction(
            BDModernHandheldControlTarget.ControlAction action)
        {
            switch (action)
            {
                case BDModernHandheldControlTarget.ControlAction.DPadLeft:
                    return Vector2.left;
                case BDModernHandheldControlTarget.ControlAction.DPadRight:
                    return Vector2.right;
                case BDModernHandheldControlTarget.ControlAction.DPadUp:
                case BDModernHandheldControlTarget.ControlAction.DPadDown:
                    return Vector2.zero;
                default:
                    return Vector2.zero;
            }
        }
        private void UpdateFirstLaunchTutorialFreePlayInput()
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

            if (ReadFirstLaunchTutorialJumpPressed())
            {
                RequestFirstLaunchTutorialJump();
                return;
            }

            if (IsFirstLaunchTutorialActionInputLocked())
                return;

            // Ranged input is consumed before generic interaction. The charged
            // lesson starts a hold transaction and fires automatically only when
            // the real charge duration reaches 100 percent.
            if (firstLaunchTutorialStep ==
                    FirstLaunchTutorialStep.ChargedShot &&
                ReadFirstLaunchTutorialRangedPressed())
            {
                BeginFirstLaunchTutorialChargedShotInput();
                return;
            }

            if (firstLaunchTutorialStep ==
                    FirstLaunchTutorialStep.RangedAttack &&
                ReadFirstLaunchTutorialRangedPressed())
            {
                HandleFirstLaunchTutorialAction(
                    BDModernHandheldControlTarget.ControlAction.Progression,
                    ResolveFirstLaunchTutorialSource(
                        FirstLaunchTutorialInputSource.Keyboard
                    )
                );
                return;
            }

            bool healingLesson =
                firstLaunchTutorialStep ==
                    FirstLaunchTutorialStep.HealHorse;

            if (!healingLesson &&
                ReadFirstLaunchTutorialInteractPressed())
            {
                HandleFirstLaunchTutorialAction(
                    BDModernHandheldControlTarget.ControlAction.Confirm,
                    ResolveFirstLaunchTutorialSource(
                        FirstLaunchTutorialInputSource.Keyboard
                    )
                );
                return;
            }

            if (firstLaunchTutorialStep ==
                    FirstLaunchTutorialStep.Parry &&
                ReadFirstLaunchTutorialParryPressed())
            {
                HandleFirstLaunchTutorialParry();
                return;
            }

            if (ReadFirstLaunchTutorialLightPressed())
            {
                HandleFirstLaunchTutorialAction(
                    BDModernHandheldControlTarget.ControlAction.Primary,
                    ResolveFirstLaunchTutorialSource(
                        FirstLaunchTutorialInputSource.Keyboard
                    )
                );
                return;
            }

            if (!healingLesson &&
                firstLaunchTutorialStep !=
                    FirstLaunchTutorialStep.RangedAttack &&
                firstLaunchTutorialStep !=
                    FirstLaunchTutorialStep.ChargedShot &&
                ReadFirstLaunchTutorialRangedPressed())
            {
                HandleFirstLaunchTutorialAction(
                    BDModernHandheldControlTarget.ControlAction.Progression,
                    ResolveFirstLaunchTutorialSource(
                        FirstLaunchTutorialInputSource.Keyboard
                    )
                );
                return;
            }

            if (TryReadFirstLaunchTutorialDirectionalDodge())
            {
                HandleFirstLaunchTutorialDodge();
                return;
            }

            if (ReadFirstLaunchTutorialHeavyPressed())
            {
                HandleFirstLaunchTutorialAction(
                    BDModernHandheldControlTarget.ControlAction.Credits,
                    ResolveFirstLaunchTutorialSource(
                        FirstLaunchTutorialInputSource.Keyboard
                    )
                );
            }
        }

        private static bool ReadFirstLaunchTutorialRangedPressed()
        {
#if ENABLE_INPUT_SYSTEM
            if (Keyboard.current != null &&
                Keyboard.current.qKey.wasPressedThisFrame)
            {
                return true;
            }
            if (Gamepad.current != null &&
                Gamepad.current.rightShoulder.wasPressedThisFrame)
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

        private void HandleFirstLaunchTutorialFreePlayAction(
            BDModernHandheldControlTarget.ControlAction action,
            FirstLaunchTutorialInputSource source)
        {
            if (firstLaunchTutorialMounted &&
                (action ==
                     BDModernHandheldControlTarget.ControlAction.Primary ||
                 action ==
                     BDModernHandheldControlTarget.ControlAction.Credits))
            {
                RejectFirstLaunchTutorialMountedMelee();
                return;
            }

            if (TryHandleFirstLaunchTutorialProductionAction(action))
                return;

            Vector2 movement =
                ResolveFirstLaunchTutorialMovementAction(action);
            if (movement.sqrMagnitude > 0.0001f)
            {
                bool directionalDodge =
                    TryRegisterFirstLaunchTutorialDirectionalDodge(
                        movement.x < -0.5f,
                        movement.x > 0.5f
                    );
                SetFirstLaunchTutorialInputSource(source);
                if (directionalDodge)
                {
                    HandleFirstLaunchTutorialDodge();
                    return;
                }

                firstLaunchTutorialPhysicalMoveLatch = movement;
                firstLaunchTutorialPhysicalMoveLatchUntil =
                    Time.unscaledTime + 0.16f;
                return;
            }

            switch (action)
            {
                case BDModernHandheldControlTarget.ControlAction.Confirm:
                    HandleFirstLaunchTutorialInteractAction();
                    break;

                case BDModernHandheldControlTarget.ControlAction.Primary:
                    if (firstLaunchTutorialStep ==
                            FirstLaunchTutorialStep.Parry)
                    {
                        HandleFirstLaunchTutorialParry();
                    }
                    else if (firstLaunchTutorialStep ==
                            FirstLaunchTutorialStep.SpinAttack)
                    {
                        firstLaunchTutorialPrimaryHoldStartedAt =
                            Time.unscaledTime;
                    }
                    else
                    {
                        HandleFirstLaunchTutorialLightAttack();
                    }
                    break;

                case BDModernHandheldControlTarget.ControlAction.Progression:
                    if (firstLaunchTutorialStep ==
                            FirstLaunchTutorialStep.HealHorse)
                    {
                        firstLaunchTutorialHealHoldStartedAt =
                            Time.unscaledTime;
                    }
                    else if (firstLaunchTutorialStep ==
                                 FirstLaunchTutorialStep.ChargedShot)
                    {
                        BeginFirstLaunchTutorialChargedShotInput();
                    }
                    else
                    {
                        HandleFirstLaunchTutorialRangedAttack();
                    }
                    break;

                case BDModernHandheldControlTarget.ControlAction.ContextBackSettings:
                    RequestFirstLaunchTutorialJump();
                    break;

                case BDModernHandheldControlTarget.ControlAction.Credits:
                    if (firstLaunchTutorialStep ==
                            FirstLaunchTutorialStep.Grapple)
                    {
                        firstLaunchTutorialGrappleHoldStartedAt =
                            Time.unscaledTime;
                    }
                    else if (firstLaunchTutorialStep ==
                                 FirstLaunchTutorialStep.Parry)
                    {
                        HandleFirstLaunchTutorialParry();
                    }
                    else
                    {
                        HandleFirstLaunchTutorialHeavyAttack();
                    }
                    break;
            }
        }

        private void HandleFirstLaunchTutorialInteractAction()
        {
            if (firstLaunchTutorialStep ==
                    FirstLaunchTutorialStep.MiniBossIntro)
            {
                if (Time.unscaledTime - firstLaunchTutorialStepStartedAt < 1.20f)
                    return;
                ShowFirstLaunchTutorialSuccess("FINAL TEST STARTED");
                SetFirstLaunchTutorialStep(
                    FirstLaunchTutorialStep.MiniBossPhaseOne
                );
                return;
            }

            if (firstLaunchTutorialStep ==
                    FirstLaunchTutorialStep.Collectible)
            {
                if (Vector2.Distance(
                        firstLaunchTutorialPlayerWorldPosition,
                        firstLaunchTutorialCollectibleWorldPosition) >
                    TutorialMountRange)
                {
                    ShowFirstLaunchTutorialSuccess("MOVE CLOSER");
                    return;
                }

                CompleteFirstLaunchTutorialCourse();
                return;
            }

            float horseDistance = Vector2.Distance(
                firstLaunchTutorialPlayerWorldPosition,
                firstLaunchTutorialHorseWorldPosition
            );
            if (horseDistance > TutorialMountRange)
            {
                ShowFirstLaunchTutorialSuccess("MOVE CLOSER");
                return;
            }

            if (!firstLaunchTutorialMounted &&
                firstLaunchTutorialHorseInjured)
            {
                ShowFirstLaunchTutorialSuccess(
                    "HEAL THE HORSE BEFORE MOUNTING"
                );
                return;
            }

            if (firstLaunchTutorialMounted)
            {
                bool dismountLesson =
                    firstLaunchTutorialStep ==
                        FirstLaunchTutorialStep.DismountHorse;
                if (dismountLesson &&
                    firstLaunchTutorialPlayerWorldPosition.x <
                        TutorialDismountMarkerX - 150f)
                {
                    ShowFirstLaunchTutorialSuccess(
                        "RIDE TO THE MARKER"
                    );
                    return;
                }

                PlayFirstLaunchTutorialDismountAnimation(
                    dismountLesson
                );
                return;
            }

            bool advances =
                firstLaunchTutorialStep ==
                    FirstLaunchTutorialStep.MountHorse ||
                firstLaunchTutorialStep ==
                    FirstLaunchTutorialStep.RemountHorse;
            PlayFirstLaunchTutorialMountAnimation(advances);
        }

        private bool RejectFirstLaunchTutorialMountedMelee()
        {
            if (!firstLaunchTutorialMounted)
                return false;

            firstLaunchTutorialPrimaryHoldStartedAt = -1f;
            firstLaunchTutorialGrappleHoldStartedAt = -1f;
            ShowFirstLaunchTutorialHoldProgress(
                string.Empty,
                0f,
                visible: false
            );
            ShowFirstLaunchTutorialSuccess(
                "ON HORSE: RANGED ATTACKS ONLY"
            );
            return true;
        }

        private void UpdateFirstLaunchTutorialCompletionContact(
            float elapsed)
        {
            if (firstLaunchTutorialStep ==
                    FirstLaunchTutorialStep.MiniBossDefeated &&
                elapsed >= 1.55f)
            {
                firstLaunchTutorialFinishGateOpen = true;
                if (firstLaunchTutorialFinishGate != null)
                    firstLaunchTutorialFinishGate.gameObject.SetActive(false);
                SetFirstLaunchTutorialStep(
                    FirstLaunchTutorialStep.Collectible
                );
                return;
            }

            if (firstLaunchTutorialStep !=
                    FirstLaunchTutorialStep.Collectible)
            {
                return;
            }

            float distance = Vector2.Distance(
                firstLaunchTutorialPlayerWorldPosition,
                firstLaunchTutorialCollectibleWorldPosition
            );
            bool crossedFinish =
                firstLaunchTutorialPlayerWorldPosition.x >=
                TutorialCollectibleX + 36f;
            if (distance <= 82f || crossedFinish)
                CompleteFirstLaunchTutorialCourse();
        }

        private void CompleteFirstLaunchTutorialCourse()
        {
            if (firstLaunchTutorialStep ==
                    FirstLaunchTutorialStep.Completed)
            {
                return;
            }

            if (firstLaunchTutorialCollectible != null)
                firstLaunchTutorialCollectible.gameObject.SetActive(false);
            if (firstLaunchTutorialFinishGate != null)
                firstLaunchTutorialFinishGate.gameObject.SetActive(false);

            firstLaunchTutorialFinishGateOpen = true;
            firstLaunchTutorialPrimaryHoldStartedAt = -1f;
            firstLaunchTutorialGrappleHoldStartedAt = -1f;
            ShowFirstLaunchTutorialSuccess("TUTORIAL COMPLETE");
            BDFirstLaunchTutorialStateStore.MarkCompleted();
            SetFirstLaunchTutorialStep(
                FirstLaunchTutorialStep.Completed
            );
        }

        private void HandleFirstLaunchTutorialLightAttack()
        {
            if (RejectFirstLaunchTutorialMountedMelee())
                return;

            bool advances =
                firstLaunchTutorialStep ==
                    FirstLaunchTutorialStep.AttackEnemy &&
                firstLaunchTutorialEnemy != null &&
                firstLaunchTutorialEnemy.gameObject.activeSelf &&
                Vector2.Distance(
                    firstLaunchTutorialPlayerWorldPosition,
                    firstLaunchTutorialEnemyWorldPosition) <=
                    TutorialAttackRange;

            if (firstLaunchTutorialStep ==
                    FirstLaunchTutorialStep.AttackEnemy &&
                !advances)
            {
                ShowFirstLaunchTutorialSuccess("MOVE INTO RANGE");
            }

            PlayFirstLaunchTutorialLightAttackAnimation(advances);
        }

        private void HandleFirstLaunchTutorialRangedAttack()
        {
            bool correctStep =
                firstLaunchTutorialStep ==
                    FirstLaunchTutorialStep.RangedAttack;
            if (correctStep && !firstLaunchTutorialMounted)
            {
                ShowFirstLaunchTutorialSuccess("MOUNT FIRST");
                return;
            }

            float targetDistance = Vector2.Distance(
                firstLaunchTutorialPlayerWorldPosition,
                firstLaunchTutorialEnemyWorldPosition
            );
            bool advances =
                correctStep &&
                firstLaunchTutorialEnemy != null &&
                firstLaunchTutorialEnemy.gameObject.activeSelf &&
                targetDistance <= 460f;

            if (correctStep && !advances)
            {
                ShowFirstLaunchTutorialSuccess("RIDE CLOSER");
                return;
            }

            TutorialEnemyActor targetActor =
                FindClosestLivingTutorialActor(520f);
            BeginFirstLaunchTutorialShotTransaction(
                targetActor,
                1f,
                charged: false,
                advancesLesson: advances
            );
            firstLaunchTutorialAmmo = Mathf.Max(
                0,
                firstLaunchTutorialAmmo - 1
            );
            PlayFirstLaunchTutorialRangedAttackAnimation(
                firstLaunchTutorialEnemyWorldPosition,
                advances,
                chargedShot: false
            );
            if (firstLaunchTutorialAmmo <= 0)
                BeginFirstLaunchTutorialReload();
        }

        private void HandleFirstLaunchTutorialDodge()
        {
            bool nearHazard =
                Vector2.Distance(
                    firstLaunchTutorialPlayerWorldPosition,
                    firstLaunchTutorialHazardWorldPosition) <=
                TutorialInstructionTriggerRange;

            bool advances =
                firstLaunchTutorialStep ==
                    FirstLaunchTutorialStep.Dodge &&
                nearHazard;

            if (firstLaunchTutorialStep ==
                    FirstLaunchTutorialStep.Dodge &&
                !nearHazard)
            {
                ShowFirstLaunchTutorialSuccess("APPROACH THE HAZARD");
            }

            Vector2 dodgeDirection =
                advances
                    ? (firstLaunchTutorialHazardWorldPosition -
                       firstLaunchTutorialPlayerWorldPosition).normalized
                    : firstLaunchTutorialLastMoveDirection;
            firstLaunchTutorialInvulnerableUntil =
                Mathf.Max(firstLaunchTutorialInvulnerableUntil, Time.unscaledTime + 0.34f);
            PlayFirstLaunchTutorialDodgeAnimation(
                dodgeDirection,
                advances
            );
        }

        private void HandleFirstLaunchTutorialHeavyAttack()
        {
            if (RejectFirstLaunchTutorialMountedMelee())
                return;

            bool advances =
                firstLaunchTutorialStep ==
                    FirstLaunchTutorialStep.HeavyAttack &&
                firstLaunchTutorialEnemy != null &&
                firstLaunchTutorialEnemy.gameObject.activeSelf &&
                Vector2.Distance(
                    firstLaunchTutorialPlayerWorldPosition,
                    firstLaunchTutorialEnemyWorldPosition) <=
                    TutorialAttackRange + 24f;

            if (firstLaunchTutorialStep ==
                    FirstLaunchTutorialStep.HeavyAttack &&
                !advances)
            {
                ShowFirstLaunchTutorialSuccess("MOVE INTO RANGE");
            }

            PlayFirstLaunchTutorialHeavyAttackAnimation(advances);
        }

        private void HandleFirstLaunchTutorialParry()
        {
            if (RejectFirstLaunchTutorialMountedMelee())
                return;

            float projectileDistance = Vector2.Distance(
                firstLaunchTutorialPlayerWorldPosition,
                firstLaunchTutorialProjectileWorldPosition
            );
            if (!firstLaunchTutorialParryProjectileActive ||
                projectileDistance > 148f)
            {
                ShowFirstLaunchTutorialSuccess("WAIT FOR THE SHOT");
                return;
            }

            PlayFirstLaunchTutorialParryAnimation(advancesLesson: true);
        }

        private void UpdateFirstLaunchTutorialHeldActions()
        {
            if (firstLaunchTutorialStep ==
                    FirstLaunchTutorialStep.ChargedShot)
            {
                if (!IsFirstLaunchTutorialActionInputLocked())
                    UpdateFirstLaunchTutorialChargedShotHold();
                return;
            }

            if (firstLaunchTutorialMounted)
            {
                firstLaunchTutorialPrimaryHoldStartedAt = -1f;
                firstLaunchTutorialGrappleHoldStartedAt = -1f;
                ShowFirstLaunchTutorialHoldProgress(
                    string.Empty,
                    0f,
                    visible: false
                );
                return;
            }

            if (IsFirstLaunchTutorialActionInputLocked())
                return;

            switch (firstLaunchTutorialStep)
            {
                case FirstLaunchTutorialStep.HealHorse:
                    UpdateFirstLaunchTutorialHealingHold();
                    break;
                case FirstLaunchTutorialStep.SpinAttack:
                    UpdateFirstLaunchTutorialSpinningHold();
                    break;
                case FirstLaunchTutorialStep.Grapple:
                    UpdateFirstLaunchTutorialGrappleHoldFreePlay();
                    break;
            }
        }

        private void UpdateFirstLaunchTutorialHealingHold()
        {
            bool closeEnough =
                Vector2.Distance(
                    firstLaunchTutorialPlayerWorldPosition,
                    firstLaunchTutorialHorseWorldPosition) <=
                TutorialMountRange + 22f;
            if (!closeEnough || !IsFirstLaunchTutorialHealHeld())
            {
                firstLaunchTutorialHealHoldStartedAt = -1f;
                SetFirstLaunchTutorialHealingPreview(0f, visible: false);
                ShowFirstLaunchTutorialHoldProgress(
                    "HOLD TO HEAL",
                    0f,
                    visible: false
                );
                return;
            }

            if (firstLaunchTutorialHealHoldStartedAt < 0f)
                firstLaunchTutorialHealHoldStartedAt = Time.unscaledTime;

            float progress = Mathf.Clamp01(
                (Time.unscaledTime -
                 firstLaunchTutorialHealHoldStartedAt) /
                TutorialHealHoldSeconds
            );
            ShowFirstLaunchTutorialHoldProgress(
                "HEALING",
                progress,
                visible: true
            );
            SetFirstLaunchTutorialHealingPreview(
                progress,
                visible: true
            );

            if (progress < 1f)
                return;

            firstLaunchTutorialHealHoldStartedAt = -1f;
            SetFirstLaunchTutorialHealingPreview(1f, visible: true);
            PlayFirstLaunchTutorialHealAnimation(
                advancesLesson: true
            );
        }

        private void UpdateFirstLaunchTutorialSpinningHold()
        {
            bool nearTargets =
                Vector2.Distance(
                    firstLaunchTutorialPlayerWorldPosition,
                    firstLaunchTutorialEnemyWorldPosition) <=
                TutorialInstructionTriggerRange;
            if (!nearTargets || !IsFirstLaunchTutorialPrimaryHeld())
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

            float progress = Mathf.Clamp01(
                (Time.unscaledTime -
                 firstLaunchTutorialPrimaryHoldStartedAt) /
                TutorialSpinHoldSeconds
            );
            ShowFirstLaunchTutorialHoldProgress(
                "SPIN CHARGE",
                progress,
                visible: true
            );

            if (progress < 1f)
                return;

            firstLaunchTutorialPrimaryHoldStartedAt = -1f;
            PlayFirstLaunchTutorialSpinAttackAnimation(
                advancesLesson: true
            );
        }

        private void UpdateFirstLaunchTutorialGrappleHoldFreePlay()
        {
            bool nearGap =
                Mathf.Abs(
                    firstLaunchTutorialPlayerWorldPosition.x -
                    TutorialGapX) <= 220f;
            if (!nearGap || !IsFirstLaunchTutorialHeavyHeld())
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

            float progress = Mathf.Clamp01(
                (Time.unscaledTime -
                 firstLaunchTutorialGrappleHoldStartedAt) /
                TutorialGrappleHoldSeconds
            );
            ShowFirstLaunchTutorialHoldProgress(
                "GRAPPLE LOCK",
                progress,
                visible: true
            );

            if (progress < 1f)
                return;

            firstLaunchTutorialGrappleHoldStartedAt = -1f;
            PlayFirstLaunchTutorialGrappleAnimation(
                advancesLesson: true
            );
        }

        private void UpdateFirstLaunchTutorialAmbush(float elapsed)
        {
            SetFirstLaunchTutorialInstructionRequested(true);

            float progress = Mathf.Clamp01(elapsed / 0.90f);
            firstLaunchTutorialEnemyWorldPosition =
                new Vector2(
                    Mathf.Lerp(
                        firstLaunchTutorialPlayerWorldPosition.x + 430f,
                        firstLaunchTutorialPlayerWorldPosition.x + 190f,
                        progress
                    ),
                    -108f
                );

            if (elapsed >= 1f)
            {
                SetFirstLaunchTutorialStep(
                    FirstLaunchTutorialStep.HorseShot
                );
            }
        }

        private void UpdateFirstLaunchTutorialHorseShotEvent(
            float elapsed)
        {
            SetFirstLaunchTutorialInstructionRequested(true);
            float progress = Mathf.Clamp01(elapsed / 0.60f);
            firstLaunchTutorialProjectileWorldPosition =
                Vector2.Lerp(
                    firstLaunchTutorialEnemyWorldPosition +
                        new Vector2(-26f, 34f),
                    firstLaunchTutorialHorseWorldPosition +
                        new Vector2(0f, 26f),
                    progress
                );

            if (elapsed < 0.62f)
                return;

            if (firstLaunchTutorialMounted)
            {
                firstLaunchTutorialMounted = false;
                firstLaunchTutorialPlayerWorldPosition =
                    firstLaunchTutorialHorseWorldPosition +
                    new Vector2(78f, 2f);
            }

            firstLaunchTutorialHorseInjured = true;
            firstLaunchTutorialHorseWorldPosition =
                firstLaunchTutorialPlayerWorldPosition +
                new Vector2(-230f, -8f);
            PlayFirstLaunchTutorialHorseHitAnimation();

            SetFirstLaunchTutorialStep(
                FirstLaunchTutorialStep.AttackEnemy
            );
        }

        private void UpdateFirstLaunchTutorialHorseReturnEvent(
            float elapsed)
        {
            SetFirstLaunchTutorialInstructionRequested(true);
            float progress = Mathf.Clamp01(elapsed / 1f);
            firstLaunchTutorialHorseWorldPosition =
                Vector2.Lerp(
                    new Vector2(
                        firstLaunchTutorialPlayerWorldPosition.x - 320f,
                        -116f
                    ),
                    firstLaunchTutorialPlayerWorldPosition +
                        new Vector2(-94f, -8f),
                    progress
                );

            if (elapsed >= 1.04f)
            {
                SetFirstLaunchTutorialStep(
                    FirstLaunchTutorialStep.HealHorse
                );
            }
        }

        private void UpdateFirstLaunchTutorialParryProjectile()
        {
            bool nearStation =
                Mathf.Abs(
                    firstLaunchTutorialPlayerWorldPosition.x -
                    TutorialParryTargetX) <= 280f;
            if (!nearStation)
            {
                firstLaunchTutorialParryProjectileActive = false;
                if (firstLaunchTutorialProjectile != null)
                {
                    firstLaunchTutorialProjectile.gameObject
                        .SetActive(false);
                }
                return;
            }

            firstLaunchTutorialParryProjectileActive = true;
            firstLaunchTutorialParryProjectileProgress =
                Mathf.Repeat(
                    firstLaunchTutorialParryProjectileProgress +
                    Time.unscaledDeltaTime / 1.55f,
                    1f
                );
            firstLaunchTutorialProjectileWorldPosition =
                Vector2.Lerp(
                    firstLaunchTutorialEnemyWorldPosition +
                        new Vector2(-22f, 34f),
                    firstLaunchTutorialPlayerWorldPosition +
                        new Vector2(0f, 24f),
                    firstLaunchTutorialParryProjectileProgress
                );
            if (firstLaunchTutorialProjectile != null)
            {
                firstLaunchTutorialProjectile.gameObject
                    .SetActive(true);
            }
        }

        private void UpdateFirstLaunchTutorialLessonAvailability(
            float elapsed)
        {
            bool requested;
            switch (firstLaunchTutorialStep)
            {
                case FirstLaunchTutorialStep.Move:
                case FirstLaunchTutorialStep.Jump:
                    requested = true;
                    break;
                case FirstLaunchTutorialStep.MountHorse:
                case FirstLaunchTutorialStep.RemountHorse:
                    requested = Vector2.Distance(
                        firstLaunchTutorialPlayerWorldPosition,
                        firstLaunchTutorialHorseWorldPosition) <= 164f;
                    break;
                case FirstLaunchTutorialStep.RideHorse:
                    requested = elapsed <= 1.65f;
                    break;
                case FirstLaunchTutorialStep.EnemyArrival:
                case FirstLaunchTutorialStep.HorseShot:
                case FirstLaunchTutorialStep.HorseReturn:
                case FirstLaunchTutorialStep.MiniBossIntro:
                case FirstLaunchTutorialStep.MiniBossDefeated:
                    requested = true;
                    break;
                case FirstLaunchTutorialStep.AttackEnemy:
                case FirstLaunchTutorialStep.HeavyAttack:
                case FirstLaunchTutorialStep.Parry:
                    requested = Vector2.Distance(
                        firstLaunchTutorialPlayerWorldPosition,
                        firstLaunchTutorialEnemyWorldPosition) <=
                        TutorialInstructionTriggerRange + 40f;
                    break;
                case FirstLaunchTutorialStep.Dodge:
                    requested = Vector2.Distance(
                        firstLaunchTutorialPlayerWorldPosition,
                        firstLaunchTutorialHazardWorldPosition) <=
                        TutorialInstructionTriggerRange;
                    break;
                case FirstLaunchTutorialStep.HealHorse:
                    requested = Vector2.Distance(
                        firstLaunchTutorialPlayerWorldPosition,
                        firstLaunchTutorialHorseWorldPosition) <= 178f;
                    break;
                case FirstLaunchTutorialStep.RangedAttack:
                case FirstLaunchTutorialStep.Reload:
                case FirstLaunchTutorialStep.ChargedShot:
                case FirstLaunchTutorialStep.MountedImpact:
                    requested = firstLaunchTutorialMounted &&
                        Mathf.Abs(firstLaunchTutorialPlayerWorldPosition.x -
                                  TutorialMountedStationX) <= 520f;
                    break;
                case FirstLaunchTutorialStep.DismountHorse:
                    requested = firstLaunchTutorialPlayerWorldPosition.x >=
                        TutorialDismountMarkerX - 150f;
                    break;
                case FirstLaunchTutorialStep.SpinAttack:
                    requested = Vector2.Distance(
                        firstLaunchTutorialPlayerWorldPosition,
                        firstLaunchTutorialEnemyWorldPosition) <=
                        TutorialInstructionTriggerRange + 40f;
                    break;
                case FirstLaunchTutorialStep.Grapple:
                    requested = Mathf.Abs(
                        firstLaunchTutorialPlayerWorldPosition.x -
                        TutorialGapX) <= 270f;
                    break;
                case FirstLaunchTutorialStep.HazardKnockback:
                    requested = Mathf.Abs(
                        firstLaunchTutorialPlayerWorldPosition.x -
                        TutorialHazardStationX) <= 300f;
                    break;
                case FirstLaunchTutorialStep.SidePath:
                    requested = false;
                    break;
                case FirstLaunchTutorialStep.CombinedEncounter:
                    requested = elapsed <= 1.40f;
                    break;
                case FirstLaunchTutorialStep.MiniBossPhaseOne:
                case FirstLaunchTutorialStep.MiniBossPhaseTwo:
                    requested = true;
                    break;
                case FirstLaunchTutorialStep.Collectible:
                    requested = Vector2.Distance(
                        firstLaunchTutorialPlayerWorldPosition,
                        firstLaunchTutorialCollectibleWorldPosition) <= 190f;
                    break;
                default:
                    requested = true;
                    break;
            }
            SetFirstLaunchTutorialInstructionRequested(requested);
        }

        private void ConfigureFirstLaunchTutorialFreePlayScene(
            FirstLaunchTutorialStep step)
        {
            SetTutorialEntityActive(firstLaunchTutorialEnemy, false);
            SetTutorialEntityActive(firstLaunchTutorialHazard, false);
            SetTutorialEntityActive(firstLaunchTutorialProjectile, false);
            SetTutorialEntityActive(firstLaunchTutorialCollectible, false);
            SetTutorialEntityActive(firstLaunchTutorialGap, false);

            if (firstLaunchTutorialHorse != null)
                firstLaunchTutorialHorse.gameObject.SetActive(true);
            if (firstLaunchTutorialPlayer != null)
                firstLaunchTutorialPlayer.gameObject.SetActive(true);

            firstLaunchTutorialScriptStartedAt = Time.unscaledTime;
            firstLaunchTutorialParryProjectileActive = false;

            switch (step)
            {
                case FirstLaunchTutorialStep.WhiteBoot:
                    firstLaunchTutorialInstructionRequested = true;
                    break;
                case FirstLaunchTutorialStep.Move:
                    firstLaunchTutorialInstructionRequested = true;
                    firstLaunchTutorialTravelDistance = 0f;
                    break;
                case FirstLaunchTutorialStep.Jump:
                    firstLaunchTutorialInstructionRequested = true;
                    firstLaunchTutorialCheckpointX = -840f;
                    break;
                case FirstLaunchTutorialStep.MountHorse:
                case FirstLaunchTutorialStep.RemountHorse:
                    firstLaunchTutorialInstructionRequested = false;
                    break;
                case FirstLaunchTutorialStep.RideHorse:
                    firstLaunchTutorialInstructionRequested = true;
                    break;
                case FirstLaunchTutorialStep.EnemyArrival:
                    firstLaunchTutorialInstructionRequested = true;
                    SetTutorialEntityActive(firstLaunchTutorialEnemy, true);
                    break;
                case FirstLaunchTutorialStep.HorseShot:
                    firstLaunchTutorialInstructionRequested = true;
                    SetTutorialEntityActive(firstLaunchTutorialEnemy, true);
                    SetTutorialEntityActive(firstLaunchTutorialProjectile, true);
                    break;
                case FirstLaunchTutorialStep.AttackEnemy:
                    firstLaunchTutorialInstructionRequested = true;
                    SetFirstLaunchTutorialCheckpoint(
                        firstLaunchTutorialPlayerWorldPosition.x - 90f
                    );
                    SetTutorialEntityActive(firstLaunchTutorialEnemy, true);
                    firstLaunchTutorialEnemyWorldPosition =
                        firstLaunchTutorialPlayerWorldPosition + new Vector2(160f, 0f);
                    break;
                case FirstLaunchTutorialStep.HeavyAttack:
                    firstLaunchTutorialInstructionRequested = false;
                    SetFirstLaunchTutorialCheckpoint(
                        TutorialHeavyTargetX - 220f
                    );
                    SetTutorialEntityActive(firstLaunchTutorialEnemy, true);
                    firstLaunchTutorialEnemyWorldPosition =
                        new Vector2(TutorialHeavyTargetX, TutorialGroundY);
                    if (firstLaunchTutorialEnemy != null)
                        firstLaunchTutorialEnemy.color = new Color(0.52f, 0.32f, 0.68f, 1f);
                    break;
                case FirstLaunchTutorialStep.Dodge:
                    firstLaunchTutorialInstructionRequested = false;
                    SetFirstLaunchTutorialCheckpoint(
                        TutorialHazardX - 230f
                    );
                    SetTutorialEntityActive(firstLaunchTutorialHazard, true);
                    firstLaunchTutorialHazardWorldPosition =
                        new Vector2(TutorialHazardX, TutorialGroundY - 34f);
                    break;
                case FirstLaunchTutorialStep.Parry:
                    firstLaunchTutorialInstructionRequested = false;
                    SetFirstLaunchTutorialCheckpoint(
                        TutorialParryTargetX - 230f
                    );
                    SetTutorialEntityActive(firstLaunchTutorialEnemy, true);
                    firstLaunchTutorialEnemyWorldPosition =
                        new Vector2(TutorialParryTargetX + 126f, TutorialGroundY);
                    firstLaunchTutorialProjectileWorldPosition =
                        firstLaunchTutorialEnemyWorldPosition + new Vector2(-22f, 34f);
                    firstLaunchTutorialParryProjectileProgress = 0f;
                    break;
                case FirstLaunchTutorialStep.HorseReturn:
                    firstLaunchTutorialInstructionRequested = true;
                    break;
                case FirstLaunchTutorialStep.HealHorse:
                    firstLaunchTutorialInstructionRequested = false;
                    SetFirstLaunchTutorialCheckpoint(
                        firstLaunchTutorialHorseWorldPosition.x - 110f
                    );
                    firstLaunchTutorialHorseInjured = true;
                    break;
                case FirstLaunchTutorialStep.RangedAttack:
                    firstLaunchTutorialInstructionRequested = false;
                    SetFirstLaunchTutorialCheckpoint(
                        TutorialMountedStationX - 250f
                    );
                    firstLaunchTutorialHorseWorldPosition =
                        new Vector2(TutorialMountedStationX - 170f, TutorialGroundY - 8f);
                    firstLaunchTutorialPlayerWorldPosition = firstLaunchTutorialHorseWorldPosition;
                    firstLaunchTutorialMounted = true;
                    break;
                case FirstLaunchTutorialStep.Reload:
                    firstLaunchTutorialInstructionRequested = true;
                    break;
                case FirstLaunchTutorialStep.ChargedShot:
                    firstLaunchTutorialInstructionRequested = true;
                    firstLaunchTutorialMounted = true;
                    firstLaunchTutorialHorseWorldPosition =
                        new Vector2(
                            TutorialMountedStationX - 120f,
                            TutorialGroundY - 8f
                        );
                    firstLaunchTutorialPlayerWorldPosition =
                        firstLaunchTutorialHorseWorldPosition;
                    break;
                case FirstLaunchTutorialStep.MountedImpact:
                    firstLaunchTutorialInstructionRequested = true;
                    SetFirstLaunchTutorialCheckpoint(
                        TutorialMountedStationX - 120f
                    );
                    break;
                case FirstLaunchTutorialStep.DismountHorse:
                    firstLaunchTutorialInstructionRequested = false;
                    SetFirstLaunchTutorialCheckpoint(
                        TutorialDismountMarkerX - 190f
                    );
                    break;
                case FirstLaunchTutorialStep.SpinAttack:
                    firstLaunchTutorialInstructionRequested = false;
                    SetFirstLaunchTutorialCheckpoint(
                        TutorialSpinTargetX - 240f
                    );
                    SetTutorialEntityActive(firstLaunchTutorialEnemy, true);
                    firstLaunchTutorialEnemyWorldPosition =
                        new Vector2(TutorialSpinTargetX + 52f, TutorialGroundY);
                    break;
                case FirstLaunchTutorialStep.Grapple:
                    firstLaunchTutorialInstructionRequested = false;
                    SetFirstLaunchTutorialCheckpoint(
                        TutorialGapX - 270f
                    );
                    SetTutorialEntityActive(firstLaunchTutorialEnemy, true);
                    firstLaunchTutorialEnemyWorldPosition =
                        new Vector2(TutorialGapX + 130f, TutorialGroundY);
                    break;
                case FirstLaunchTutorialStep.HazardKnockback:
                    firstLaunchTutorialInstructionRequested = false;
                    SetFirstLaunchTutorialCheckpoint(
                        TutorialHazardStationX - 250f
                    );
                    break;
                case FirstLaunchTutorialStep.SidePath:
                    firstLaunchTutorialInstructionRequested = false;
                    firstLaunchTutorialCheckpointX = TutorialSecretBranchX - 220f;
                    break;
                case FirstLaunchTutorialStep.CombinedEncounter:
                    firstLaunchTutorialInstructionRequested = true;
                    firstLaunchTutorialCheckpointX = TutorialCombinedStationX - 190f;
                    break;
                case FirstLaunchTutorialStep.MiniBossIntro:
                    firstLaunchTutorialInstructionRequested = true;
                    firstLaunchTutorialCheckpointX = TutorialMiniBossStationX - 220f;
                    break;
                case FirstLaunchTutorialStep.MiniBossPhaseOne:
                case FirstLaunchTutorialStep.MiniBossPhaseTwo:
                case FirstLaunchTutorialStep.MiniBossDefeated:
                    firstLaunchTutorialInstructionRequested = true;
                    break;
                case FirstLaunchTutorialStep.Collectible:
                    firstLaunchTutorialInstructionRequested = false;
                    SetTutorialEntityActive(firstLaunchTutorialCollectible, true);
                    firstLaunchTutorialCollectibleWorldPosition =
                        new Vector2(TutorialCollectibleX, -90f);
                    break;
                case FirstLaunchTutorialStep.Completed:
                    firstLaunchTutorialInstructionRequested = true;
                    break;
            }

            ConfigureFirstLaunchTutorialProductionStep(step);
            UpdateFirstLaunchTutorialPhysicalHighlight();
        }

        private void SetFirstLaunchTutorialInstructionRequested(
            bool requested)
        {
            if (firstLaunchTutorialInstructionRequested == requested)
                return;

            firstLaunchTutorialInstructionRequested = requested;
            if (requested)
            {
                firstLaunchTutorialInstructionStartedAt =
                    Time.unscaledTime;
                UpdateFirstLaunchTutorialPrompt();
                UpdateFirstLaunchTutorialPhysicalHighlight();
            }
            else
            {
                ClearFirstLaunchTutorialPhysicalHighlight();
            }
        }

        private bool IsFirstLaunchTutorialInstructionRequested()
        {
            return firstLaunchTutorialInstructionRequested;
        }

        private void SetFirstLaunchTutorialInputSource(
            FirstLaunchTutorialInputSource source)
        {
            FirstLaunchTutorialInputSource resolved =
                ResolveFirstLaunchTutorialSource(source);
            if (firstLaunchTutorialInputSource == resolved)
                return;

            firstLaunchTutorialInputSource = resolved;
            UpdateFirstLaunchTutorialPrompt();
        }

        private string ResolveFirstLaunchTutorialFreePlayProgressLabel()
        {
            int section;
            const int total = 10;
            switch (firstLaunchTutorialStep)
            {
                case FirstLaunchTutorialStep.WhiteBoot:
                case FirstLaunchTutorialStep.Move:
                case FirstLaunchTutorialStep.Jump:
                    section = 1;
                    break;
                case FirstLaunchTutorialStep.MountHorse:
                case FirstLaunchTutorialStep.RideHorse:
                    section = 2;
                    break;
                case FirstLaunchTutorialStep.EnemyArrival:
                case FirstLaunchTutorialStep.HorseShot:
                case FirstLaunchTutorialStep.AttackEnemy:
                case FirstLaunchTutorialStep.HeavyAttack:
                case FirstLaunchTutorialStep.Dodge:
                case FirstLaunchTutorialStep.Parry:
                    section = 3;
                    break;
                case FirstLaunchTutorialStep.HorseReturn:
                case FirstLaunchTutorialStep.HealHorse:
                case FirstLaunchTutorialStep.RemountHorse:
                    section = 4;
                    break;
                case FirstLaunchTutorialStep.RangedAttack:
                case FirstLaunchTutorialStep.Reload:
                case FirstLaunchTutorialStep.ChargedShot:
                case FirstLaunchTutorialStep.MountedImpact:
                case FirstLaunchTutorialStep.DismountHorse:
                    section = 5;
                    break;
                case FirstLaunchTutorialStep.SpinAttack:
                case FirstLaunchTutorialStep.Grapple:
                    section = 6;
                    break;
                case FirstLaunchTutorialStep.HazardKnockback:
                    section = 7;
                    break;
                case FirstLaunchTutorialStep.SidePath:
                    section = 8;
                    break;
                case FirstLaunchTutorialStep.CombinedEncounter:
                    section = 9;
                    break;
                default:
                    section = 10;
                    break;
            }
            return "GUIDE " + section + " / " + total;
        }

        private void RenderFirstLaunchTutorialFreePlayCourse(bool force)
        {
            if (firstLaunchTutorialCourseRoot == null)
                return;

            float minimumCamera =
                TutorialWorldMinX + TutorialWorldViewportHalfWidth;
            float maximumCamera =
                TutorialWorldMaxX - TutorialWorldViewportHalfWidth;
            float facingLead = Mathf.Abs(firstLaunchTutorialLastMoveDirection.x) > 0.01f
                ? Mathf.Sign(firstLaunchTutorialLastMoveDirection.x)
                : 1f;
            float leadDistance = firstLaunchTutorialMounted ? 96f : 54f;
            float targetCamera = Mathf.Clamp(
                firstLaunchTutorialPlayerWorldPosition.x + facingLead * leadDistance,
                minimumCamera,
                maximumCamera
            );

            if (force)
            {
                firstLaunchTutorialCameraWorldX = targetCamera;
            }
            else
            {
                firstLaunchTutorialCameraWorldX = Mathf.MoveTowards(
                    firstLaunchTutorialCameraWorldX,
                    targetCamera,
                    560f * Time.unscaledDeltaTime
                );
            }

            firstLaunchTutorialCourseRoot.anchoredPosition =
                new Vector2(
                    -SnapFirstLaunchTutorialPixelValue(
                        firstLaunchTutorialCameraWorldX
                    ),
                    0f
                );

            if (firstLaunchTutorialHorse != null)
            {
                firstLaunchTutorialHorse.rectTransform.anchoredPosition =
                    SnapFirstLaunchTutorialWorldPosition(
                        firstLaunchTutorialHorseWorldPosition
                    );
                firstLaunchTutorialHorse.color =
                    firstLaunchTutorialHorseInjured
                        ? new Color(0.62f, 0.14f, 0.16f, 1f)
                        : new Color(0.58f, 0.35f, 0.18f, 1f);
            }

            if (firstLaunchTutorialPlayer != null)
            {
                Vector2 playerPosition =
                    firstLaunchTutorialMounted
                        ? firstLaunchTutorialHorseWorldPosition +
                          new Vector2(0f, 34f)
                        : firstLaunchTutorialPlayerWorldPosition;
                firstLaunchTutorialPlayer.rectTransform.anchoredPosition =
                    SnapFirstLaunchTutorialWorldPosition(playerPosition);

                float facing =
                    Mathf.Abs(firstLaunchTutorialLastMoveDirection.x) >
                    0.01f
                        ? Mathf.Sign(
                            firstLaunchTutorialLastMoveDirection.x
                        )
                        : 1f;
                if (!firstLaunchTutorialPlayerDeathActive)
                {
                    float scale = firstLaunchTutorialMounted ? 0.78f : 1f;
                    firstLaunchTutorialPlayer.rectTransform.localScale =
                        new Vector3(facing * scale, scale, 1f);
                    firstLaunchTutorialPlayer.rectTransform.localRotation =
                        Quaternion.identity;
                }
            }

            if (firstLaunchTutorialEnemy != null)
            {
                firstLaunchTutorialEnemy.rectTransform.anchoredPosition =
                    SnapFirstLaunchTutorialWorldPosition(
                        firstLaunchTutorialEnemyWorldPosition
                    );
            }

            if (firstLaunchTutorialHazard != null)
            {
                firstLaunchTutorialHazard.rectTransform.anchoredPosition =
                    SnapFirstLaunchTutorialWorldPosition(
                        firstLaunchTutorialHazardWorldPosition
                    );
            }

            if (firstLaunchTutorialProjectile != null)
            {
                firstLaunchTutorialProjectile.rectTransform
                    .anchoredPosition =
                    SnapFirstLaunchTutorialWorldPosition(
                        firstLaunchTutorialProjectileWorldPosition
                    );
            }

            if (firstLaunchTutorialCollectible != null)
            {
                firstLaunchTutorialCollectible.rectTransform
                    .anchoredPosition =
                    SnapFirstLaunchTutorialWorldPosition(
                        firstLaunchTutorialCollectibleWorldPosition
                    );
            }

            if (firstLaunchTutorialGap != null)
            {
                firstLaunchTutorialGap.rectTransform.anchoredPosition =
                    SnapFirstLaunchTutorialWorldPosition(
                        firstLaunchTutorialGapWorldPosition
                    );
            }

            RenderFirstLaunchTutorialProductionCourse();
        }

        private static Vector2 SnapFirstLaunchTutorialWorldPosition(
            Vector2 value)
        {
            return new Vector2(
                SnapFirstLaunchTutorialPixelValue(value.x),
                SnapFirstLaunchTutorialPixelValue(value.y)
            );
        }

        private void CompleteFirstLaunchTutorialMountAnimation(
            bool advancesLesson)
        {
            firstLaunchTutorialMounted = true;
            firstLaunchTutorialPlayerWorldPosition =
                firstLaunchTutorialHorseWorldPosition;
            ShowFirstLaunchTutorialSuccess("MOUNTED");

            if (!advancesLesson)
                return;

            if (firstLaunchTutorialStep ==
                    FirstLaunchTutorialStep.MountHorse)
            {
                SetFirstLaunchTutorialStep(
                    FirstLaunchTutorialStep.RideHorse
                );
            }
            else if (firstLaunchTutorialStep ==
                         FirstLaunchTutorialStep.RemountHorse)
            {
                SetFirstLaunchTutorialStep(
                    FirstLaunchTutorialStep.RangedAttack
                );
            }
        }

        private void CompleteFirstLaunchTutorialDismountAnimation(
            bool advancesLesson)
        {
            firstLaunchTutorialMounted = false;
            firstLaunchTutorialPlayerWorldPosition =
                firstLaunchTutorialHorseWorldPosition +
                new Vector2(76f, 0f);
            ShowFirstLaunchTutorialSuccess("DISMOUNTED");

            if (advancesLesson)
            {
                SetFirstLaunchTutorialStep(
                    FirstLaunchTutorialStep.SpinAttack
                );
            }
        }

        private void CompleteFirstLaunchTutorialLightAttackAnimation(
            bool advancesLesson)
        {
            if (!advancesLesson)
                return;

            if (firstLaunchTutorialEnemy != null)
                firstLaunchTutorialEnemy.gameObject.SetActive(false);
            ShowFirstLaunchTutorialSuccess("OPENING FOUND");
            SetFirstLaunchTutorialStep(
                FirstLaunchTutorialStep.HeavyAttack
            );
        }

        private void CompleteFirstLaunchTutorialRangedAttackAnimation(
            bool advancesLesson)
        {
            if (!advancesLesson)
                return;

            SetFirstLaunchTutorialLearningState(
                "MountedShot",
                TutorialLearningState.Demonstrated
            );
            ShowFirstLaunchTutorialSuccess("CLEAN SHOT");
        }

        private void CompleteFirstLaunchTutorialDodgeAnimation(
            bool advancesLesson)
        {
            if (advancesLesson)
            {
                SetFirstLaunchTutorialLearningState(
                    "Dodge",
                    TutorialLearningState.Demonstrated
                );
                ShowFirstLaunchTutorialSuccess("EVADED");
                SetFirstLaunchTutorialStep(
                    FirstLaunchTutorialStep.Parry
                );
            }
        }

        private void CompleteFirstLaunchTutorialHeavyAttackAnimation(
            bool advancesLesson)
        {
            if (!advancesLesson)
                return;

            if (firstLaunchTutorialEnemy != null)
                firstLaunchTutorialEnemy.gameObject.SetActive(false);
            ShowFirstLaunchTutorialSuccess("HEAVY IMPACT");
            SetFirstLaunchTutorialStep(
                FirstLaunchTutorialStep.Dodge
            );
        }

        private void CompleteFirstLaunchTutorialSpinAttackAnimation(
            bool advancesLesson)
        {
            if (!advancesLesson)
                return;

            if (firstLaunchTutorialEnemy != null)
                firstLaunchTutorialEnemy.gameObject.SetActive(false);
            if (firstLaunchTutorialHazard != null)
                firstLaunchTutorialHazard.gameObject.SetActive(false);
            for (int actorIndex = 0;
                 actorIndex < firstLaunchTutorialActors.Count;
                 actorIndex++)
            {
                TutorialEnemyActor actor = firstLaunchTutorialActors[actorIndex];
                if (actor.Role == TutorialEnemyRole.MiniBoss)
                    continue;
                actor.Active = false;
                actor.Dead = true;
                if (actor.Image != null)
                    actor.Image.gameObject.SetActive(false);
            }
            ShowFirstLaunchTutorialSuccess("GROUP CLEARED");
            SetFirstLaunchTutorialStep(
                FirstLaunchTutorialStep.Grapple
            );
        }

        private void CompleteFirstLaunchTutorialParryAnimation(
            bool advancesLesson)
        {
            firstLaunchTutorialParryProjectileActive = false;
            if (firstLaunchTutorialProjectile != null)
            {
                firstLaunchTutorialProjectile.gameObject.SetActive(false);
            }

            if (advancesLesson)
            {
                SetFirstLaunchTutorialLearningState(
                    "Parry",
                    TutorialLearningState.Demonstrated
                );
                ShowFirstLaunchTutorialSuccess("PARRIED");
                SetFirstLaunchTutorialStep(
                    FirstLaunchTutorialStep.HorseReturn
                );
            }
        }

        private void CompleteFirstLaunchTutorialGrappleAnimation(
            bool advancesLesson)
        {
            CompleteFirstLaunchTutorialProductionHookTransaction();
            if (!advancesLesson)
                return;

            if (firstLaunchTutorialEnemy != null)
                firstLaunchTutorialEnemy.gameObject.SetActive(false);
            ShowFirstLaunchTutorialSuccess("PULLED INTO RANGE");
            SetFirstLaunchTutorialStep(
                FirstLaunchTutorialStep.HazardKnockback
            );
        }

        private void CompleteFirstLaunchTutorialHealAnimation(
            bool advancesLesson)
        {
            firstLaunchTutorialHorseInjured = false;
            if (!advancesLesson)
                return;

            ShowFirstLaunchTutorialSuccess("HORSE HEALED");
            SetFirstLaunchTutorialStep(
                FirstLaunchTutorialStep.RemountHorse
            );
        }
    }
}
