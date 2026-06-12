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
        // BD TUTORIAL LESSON PERSISTENCE GATE V10.11.23
        // Once a lesson is shown it remains visible until that lesson changes.
        // The forward world gate is owned by the active lesson, not by proximity.
        private bool firstLaunchTutorialInstructionLatchedV101123;
        private float firstLaunchTutorialLessonGateFeedbackAtV101123 = -999f;

        private const float TutorialWorldMinX = -1180f; // BD OPENING RUNWAY V10.11.25
        private const float TutorialWorldMaxX = 6480f;
        private const float TutorialWorldMinY = -126f;
        private const float TutorialWorldMaxY = 56f;
        private const float TutorialWorldViewportHalfWidth = 390f;
        private const float TutorialFootMoveSpeed = 176f;
        private const float TutorialMountedMoveSpeed = 270f;
        private const float TutorialMountRange = 104f;
        private const float TutorialAttackRange = 172f;
        private const float TutorialInstructionTriggerRange = 238f;
        private const string FirstLaunchTutorialLessonCompleteTravelMessage =
            "LESSON COMPLETE — MOVE RIGHT TO THE NEXT SCREEN";

        private const float TutorialHorseStartX = -650f;
        private const float TutorialAmbushTriggerX = -350f;
        private const float TutorialAmbushEnemyX = -165f;
        private const float TutorialRangedTargetX = 2480f;
        private const float TutorialDismountMarkerX = 3320f;
        private const float TutorialHazardX = 1260f;
        private const float TutorialHeavyTargetX = 920f;
        private const float TutorialSpinTargetX = 3560f;
        private const float TutorialParryTargetX = 1640f;
        private const float TutorialGapX = 3920f;
        private const float TutorialCollectibleX = 6200f;

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
        private float firstLaunchTutorialProgressFloorX;
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
        private bool firstLaunchTutorialJumpUnlocked;
        private bool firstLaunchTutorialInteractUnlocked;
        private bool firstLaunchTutorialLightAttackUnlocked;
        private bool firstLaunchTutorialHeavyAttackUnlocked;
        private bool firstLaunchTutorialDodgeUnlocked;
        private bool firstLaunchTutorialParryUnlocked;
        private bool firstLaunchTutorialRangedUnlocked;
        private float firstLaunchTutorialLockedAbilityFeedbackAt;

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
                new Color(0.035f, 0.13f, 0.14f, 1f)
            );
            ResizeFirstLaunchTutorialCourseSurface(
                "Tutorial Path",
                -104f,
                58f,
                new Color(0.48f, 0.29f, 0.13f, 1f)
            );

            BuildFirstLaunchTutorialCourseDecorations();
            BuildFirstLaunchTutorialActionPresentation(
                firstLaunchTutorialCourseRoot
            );

            firstLaunchTutorialPlayerWorldPosition =
                new Vector2(-1120f, -108f); /* BD OPENING RUNWAY V10.11.25 */
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

            // BD FORWARD COURSE ENTRY ANCHOR V10.11.25.2
            // This is the original forward-course entry contract, not the
            // player's spawn. The child remains farther back at the opening.
            Vector2 tutorialForwardCourseEntryAnchor =
                new Vector2(-820f, -108f);
            firstLaunchTutorialCameraWorldX = Mathf.Min(
                firstLaunchTutorialPlayerWorldPosition.x,
                tutorialForwardCourseEntryAnchor.x
            );
            firstLaunchTutorialProgressFloorX = TutorialWorldMinX;
            firstLaunchTutorialTravelDistance = 0f;
            firstLaunchTutorialPhysicalMoveLatch = Vector2.zero;
            firstLaunchTutorialPhysicalMoveLatchUntil = 0f;
            firstLaunchTutorialMounted = false;
            firstLaunchTutorialHorseInjured = false;
            firstLaunchTutorialParryProjectileActive = false;
            firstLaunchTutorialJumpUnlocked = false;
            firstLaunchTutorialInteractUnlocked = false;
            firstLaunchTutorialLightAttackUnlocked = false;
            firstLaunchTutorialHeavyAttackUnlocked = false;
            firstLaunchTutorialDodgeUnlocked = false;
            firstLaunchTutorialParryUnlocked = false;
            firstLaunchTutorialRangedUnlocked = false;
            firstLaunchTutorialLockedAbilityFeedbackAt = 0f;
            firstLaunchTutorialInstructionLatchedV101123 = false;
            firstLaunchTutorialLessonScreenInitialized = false;
            firstLaunchTutorialLessonCompleteAwaitingTravel = false;
            firstLaunchTutorialLessonScreenTransitionActive = false;
            firstLaunchTutorialLessonScreenTransitionApplied = false;
            firstLaunchTutorialInstructionRequested = true;
            firstLaunchTutorialInstructionVisibility = 1f;
            firstLaunchTutorialNextGateFeedbackAt = 0f;

            InitializeFirstLaunchTutorialProductionCourse();
            InitializeFirstLaunchTutorialFinalProductionPass();
            RenderFirstLaunchTutorialFreePlayCourse(force: true);
        }

        private void DisposeFirstLaunchTutorialFreePlayCourse()
        {
            DisposeFirstLaunchTutorialFinalProductionPass();
            DisposeFirstLaunchTutorialProductionCourse();
            firstLaunchTutorialCourseDecorations.Clear();
            firstLaunchTutorialCourseRoot = null;
            firstLaunchTutorialPhysicalMoveLatch = Vector2.zero;
            firstLaunchTutorialPhysicalMoveLatchUntil = 0f;
            firstLaunchTutorialMounted = false;
            firstLaunchTutorialHorseInjured = false;
            firstLaunchTutorialParryProjectileActive = false;
            firstLaunchTutorialMovementActive = false;
            firstLaunchTutorialJumpUnlocked = false;
            firstLaunchTutorialInteractUnlocked = false;
            firstLaunchTutorialLightAttackUnlocked = false;
            firstLaunchTutorialHeavyAttackUnlocked = false;
            firstLaunchTutorialDodgeUnlocked = false;
            firstLaunchTutorialParryUnlocked = false;
            firstLaunchTutorialRangedUnlocked = false;
            firstLaunchTutorialLockedAbilityFeedbackAt = 0f;
            firstLaunchTutorialInstructionLatchedV101123 = false;
            firstLaunchTutorialLessonScreenInitialized = false;
            firstLaunchTutorialLessonCompleteAwaitingTravel = false;
            firstLaunchTutorialLessonScreenTransitionActive = false;
            firstLaunchTutorialLessonScreenTransitionApplied = false;
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
                new Color(0.025f, 0.13f, 0.14f, 1f);
            Color treeLight =
                new Color(0.06f, 0.28f, 0.21f, 1f);
            Color trunk =
                new Color(0.25f, 0.13f, 0.075f, 1f);
            Color stone =
                new Color(0.25f, 0.23f, 0.38f, 1f);
            Color grass =
                new Color(0.18f, 0.48f, 0.25f, 1f);
            Color marker =
                new Color(1f, 0.58f, 0.18f, 1f);

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

                float treeX = x + 54f * side;
                AddFirstLaunchTutorialCourseDecoration(
                    "Course Tree Trunk " + index,
                    treeX,
                    88f,
                    16f,
                    72f,
                    trunk
                );
                AddFirstLaunchTutorialCourseDecoration(
                    "Course Tree Canopy Lower " + index,
                    treeX,
                    126f,
                    72f,
                    36f,
                    treeDark
                );
                AddFirstLaunchTutorialCourseDecoration(
                    "Course Tree Canopy Upper " + index,
                    treeX + 8f * side,
                    154f,
                    48f,
                    28f,
                    index % 3 == 0 ? treeLight : treeDark
                );
                AddFirstLaunchTutorialCourseDecoration(
                    "Course Stone Base " + index,
                    x - 68f * side,
                    -105f,
                    32f,
                    12f,
                    stone
                );
                AddFirstLaunchTutorialCourseDecoration(
                    "Course Stone Cap " + index,
                    x - 64f * side,
                    -95f,
                    20f,
                    8f,
                    stone
                );
                AddFirstLaunchTutorialCourseDecoration(
                    "Course Grass Left " + index,
                    x - 26f,
                    -112f,
                    8f,
                    16f,
                    grass
                );
                AddFirstLaunchTutorialCourseDecoration(
                    "Course Grass Right " + index,
                    x - 18f,
                    -116f,
                    8f,
                    12f,
                    grass
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
                float markerX = lessonMarkers[index];
                AddFirstLaunchTutorialCourseDecoration(
                    "Course Path Rune Center " + index,
                    markerX,
                    -101f,
                    8f,
                    8f,
                    marker
                );
                AddFirstLaunchTutorialCourseDecoration(
                    "Course Path Rune Left " + index,
                    markerX - 8f,
                    -101f,
                    4f,
                    4f,
                    marker
                );
                AddFirstLaunchTutorialCourseDecoration(
                    "Course Path Rune Right " + index,
                    markerX + 8f,
                    -101f,
                    4f,
                    4f,
                    marker
                );
            }
        }

        private void AddFirstLaunchTutorialCourseDecoration(
            string name,
            float x,
            float y,
            float width,
            float height,
            Color color)
        {
            firstLaunchTutorialCourseDecorations.Add(
                CreateFirstLaunchTutorialCourseDecoration(
                    name,
                    SnapFirstLaunchTutorialPixelValue(x),
                    SnapFirstLaunchTutorialPixelValue(y),
                    SnapFirstLaunchTutorialPixelValue(width),
                    SnapFirstLaunchTutorialPixelValue(height),
                    color
                )
            );
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
            AddOutline(
                image.gameObject,
                new Color(0.025f, 0.025f, 0.075f, 0.92f),
                2f
            );
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

            if (UpdateFirstLaunchTutorialLessonScreenFlow())
            {
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
            if (firstLaunchTutorialWhiteOverlay != null)
                firstLaunchTutorialWhiteOverlay.gameObject.SetActive(false);

            SetFirstLaunchTutorialStep(FirstLaunchTutorialStep.Move);
        }

        private void UpdateFirstLaunchTutorialContinuousMovement()
        {
            if (firstLaunchTutorialRelicCompletionActive)
                return;

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
                if (source != FirstLaunchTutorialInputSource.Handheld &&
                    Mathf.Abs(movement.x) > 0.05f)
                {
                    PulsePersistentControl(
                        movement.x < 0f
                            ? BDModernHandheldControlTarget.ControlAction.DPadLeft
                            : BDModernHandheldControlTarget.ControlAction.DPadRight
                    );
                }
            }

            if (movement.sqrMagnitude > 1f)
                movement.Normalize();
            Vector2 delta =
                new Vector2(movement.x, 0f) *
                speed *
                Time.unscaledDeltaTime;

            // BD STRICT CURRENT-LESSON FORWARD GATE V10.11.23
            float currentPlayerX =
                firstLaunchTutorialPlayerWorldPosition.x;
            float lessonGuidanceX = Mathf.Min(
                TutorialWorldMaxX,
                ResolveFirstLaunchTutorialMaximumPlayerX()
            );
            // Backtracking remains free. Forward movement stops at the active
            // lesson boundary until the step's real success path installs the
            // next step. If an older save is already beyond the boundary, the
            // player is not teleported; only further forward movement is held.
            float maximumPlayerX = Mathf.Clamp(
                Mathf.Max(currentPlayerX, lessonGuidanceX),
                TutorialWorldMinX,
                TutorialWorldMaxX
            );
            float requestedPlayerX = Mathf.Max(
                firstLaunchTutorialProgressFloorX,
                currentPlayerX + delta.x
            );
            float collisionResolvedX =
                ResolveFirstLaunchTutorialCollisionX(
                    currentPlayerX,
                    requestedPlayerX
                );
            collisionResolvedX =
                ResolveFirstLaunchTutorialLivingActorCollisionX(
                    currentPlayerX,
                    collisionResolvedX
                );
            if (firstLaunchTutorialStep == FirstLaunchTutorialStep.WallJump &&
                currentPlayerX < TutorialWallJumpWallX &&
                requestedPlayerX > currentPlayerX &&
                firstLaunchTutorialPlayerWorldPosition.y <
                    TutorialWallJumpUpperGroundStandingY + 12f &&
                collisionResolvedX > TutorialWallJumpWallX - 52f)
            {
                collisionResolvedX = TutorialWallJumpWallX - 52f;
            }
            firstLaunchTutorialPlayerWorldPosition.x = Mathf.Clamp(
                collisionResolvedX,
                TutorialWorldMinX,
                maximumPlayerX
            );

            if (movement.x > 0.05f &&
                requestedPlayerX > lessonGuidanceX + 0.5f)
            {
                // Reaching the boundary always surfaces the current lesson and
                // keeps it visible. The feedback is rate-limited independently
                // from the physical clamp so holding right cannot spam audio/UI.
                SetFirstLaunchTutorialInstructionRequested(true);
                if (Time.unscaledTime >=
                        firstLaunchTutorialLessonGateFeedbackAtV101123)
                {
                    firstLaunchTutorialLessonGateFeedbackAtV101123 =
                        Time.unscaledTime + 0.85f;
                    NotifyFirstLaunchTutorialLessonGate();
                }
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

            if (firstLaunchTutorialStep == FirstLaunchTutorialStep.WallJump &&
                firstLaunchTutorialGroundedY ==
                    TutorialWallJumpUpperGroundStandingY &&
                firstLaunchTutorialPlayerWorldPosition.x >= 3600f)
            {
                firstLaunchTutorialGrounded = false;
                firstLaunchTutorialGroundedY = TutorialGroundY;
                firstLaunchTutorialLastMoveDirection = Vector2.right;
                SetFirstLaunchTutorialLearningState(
                    "WallJump",
                    TutorialLearningState.Demonstrated
                );
                ShowFirstLaunchTutorialSuccess("WALL JUMP CLEARED");
                SetFirstLaunchTutorialStep(FirstLaunchTutorialStep.MiniBossIntro);
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
                controller = currentGamepad.leftStick.ReadValue();
                if (controller.sqrMagnitude < 0.04f)
                    controller = currentGamepad.dpad.ReadValue();
            }

            bool pointerHeld = Mouse.current != null &&
                Mouse.current.leftButton.isPressed;
            bool touchHeld = Touchscreen.current != null &&
                Touchscreen.current.primaryTouch.press.isPressed;
            if ((pointerHeld || touchHeld) && hoveredTarget != null)
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

            if (Input.GetMouseButton(0) && hoveredTarget != null)
            {
                physical = ResolveFirstLaunchTutorialMovementAction(
                    hoveredTarget.Action
                );
            }
#endif

            if (Time.unscaledTime < firstLaunchTutorialPhysicalMoveLatchUntil)
                physical += firstLaunchTutorialPhysicalMoveLatch;

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
                case FirstLaunchTutorialStep.JumpAttack:
                    return TutorialParryTargetX + 220f;
                case FirstLaunchTutorialStep.WallJump:
                    return TutorialWallJumpUpperGroundMaxX + 40f;
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
                    return TutorialMountedStationX + 420f;
                case FirstLaunchTutorialStep.MountedImpact:
                    // BD MOUNTED IMPACT RUNWAY V10.11.25
                    return TutorialMountedStationX + 680f;
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
        // BD WORLD MOUSE LIGHT ATTACK V10.11.20.3
        // A click inside the playable tutorial viewport is resolved before the
        // handheld screen/UI hit path; the screen hit cannot consume the attack.
        // BD PHYSICAL SCREEN MOUSE ATTACK V10.11.21
        // The tutorial is rendered into a texture on a real 3D screen. Testing
        // the off-screen Canvas against desktop coordinates cannot work reliably.
        // Ray-plane projection resolves the actual physical display instead.
        // BD COMPLETE TUTORIAL INPUT PARITY V10.11.22
        // Direct desktop mouse input is captured only when the pointer presses
        // the actual 3D display. Hardware controls keep their existing owner.
        private Vector2 firstLaunchTutorialWorldMouseMoveV101122;

        private bool TryConsumeFirstLaunchTutorialWorldMouseAttackV1011203()
        {
            bool primaryHeld =
                IsFirstLaunchTutorialWorldMouseButtonHeldV101122(0);
            bool secondaryHeld =
                IsFirstLaunchTutorialWorldMouseButtonHeldV101122(1);
            bool rangedHeld =
                IsFirstLaunchTutorialWorldMouseButtonHeldV101122(2);

            if (!primaryHeld)
            if (!secondaryHeld)
            if (!rangedHeld)

            if (firstLaunchTutorialExitOpen ||
                IsFirstLaunchTutorialActionInputLocked())
            {
                return false;
            }

            bool primaryPressed = false;
            bool secondaryPressed = false;
            bool rangedPressed = false;
            Vector2 screenPosition = Vector2.zero;

#if ENABLE_INPUT_SYSTEM
            if (Mouse.current != null)
            {
                primaryPressed =
                    Mouse.current.leftButton.wasPressedThisFrame;
                secondaryPressed =
                    Mouse.current.rightButton.wasPressedThisFrame;
                rangedPressed =
                    Mouse.current.middleButton.wasPressedThisFrame;
                if (primaryPressed || secondaryPressed || rangedPressed)
                    screenPosition = Mouse.current.position.ReadValue();
            }
#endif

#if ENABLE_LEGACY_INPUT_MANAGER
            if (!primaryPressed && Input.GetMouseButtonDown(0))
            {
                primaryPressed = true;
                screenPosition = Input.mousePosition;
            }
            if (!secondaryPressed && Input.GetMouseButtonDown(1))
            {
                secondaryPressed = true;
                screenPosition = Input.mousePosition;
            }
            if (!rangedPressed && Input.GetMouseButtonDown(2))
            {
                rangedPressed = true;
                screenPosition = Input.mousePosition;
            }
#endif

            if ((!primaryPressed && !secondaryPressed && !rangedPressed) ||
                !TryResolveFirstLaunchTutorialPhysicalScreenPointV101122(
                    screenPosition,
                    out Vector3 localHit
                ))
            {
                return false;
            }

            SetFirstLaunchTutorialInputSource(
                FirstLaunchTutorialInputSource.Keyboard
            );

            if (primaryPressed)
            {
                firstLaunchTutorialWorldMouseMoveV101122 =
                    localHit.x < 0f ? Vector2.left : Vector2.right;

                switch (firstLaunchTutorialStep)
                {
                    case FirstLaunchTutorialStep.Move:
                    case FirstLaunchTutorialStep.RideHorse:
                    case FirstLaunchTutorialStep.MountedImpact:
                    case FirstLaunchTutorialStep.SidePath:
                        firstLaunchTutorialPhysicalMoveLatch =
                            firstLaunchTutorialWorldMouseMoveV101122;
                        firstLaunchTutorialPhysicalMoveLatchUntil =
                            Time.unscaledTime + 0.18f;
                        return true;

                    case FirstLaunchTutorialStep.Jump:
                    case FirstLaunchTutorialStep.WallJump:
                        RequestFirstLaunchTutorialJump();
                        return true;

                    case FirstLaunchTutorialStep.MountHorse:
                    case FirstLaunchTutorialStep.RemountHorse:
                    case FirstLaunchTutorialStep.DismountHorse:
                    case FirstLaunchTutorialStep.MiniBossIntro:
                        HandleFirstLaunchTutorialAction(
                            BDModernHandheldControlTarget.ControlAction.Confirm,
                            FirstLaunchTutorialInputSource.Keyboard
                        );
                        return true;

                    case FirstLaunchTutorialStep.AttackEnemy:
                    case FirstLaunchTutorialStep.JumpAttack:
                    case FirstLaunchTutorialStep.SpinAttack:
                    case FirstLaunchTutorialStep.Parry:
                    case FirstLaunchTutorialStep.CombinedEncounter:
                        HandleFirstLaunchTutorialAction(
                            BDModernHandheldControlTarget.ControlAction.Primary,
                            FirstLaunchTutorialInputSource.Keyboard
                        );
                        return true;

                    case FirstLaunchTutorialStep.HeavyAttack:
                    case FirstLaunchTutorialStep.Grapple:
                    case FirstLaunchTutorialStep.HazardKnockback:
                        HandleFirstLaunchTutorialAction(
                            BDModernHandheldControlTarget.ControlAction.Credits,
                            FirstLaunchTutorialInputSource.Keyboard
                        );
                        return true;

                    case FirstLaunchTutorialStep.HealHorse:
                    case FirstLaunchTutorialStep.RangedAttack:
                    case FirstLaunchTutorialStep.ChargedShot:
                    case FirstLaunchTutorialStep.MiniBossPhaseOne:
                    case FirstLaunchTutorialStep.MiniBossPhaseTwo:
                        HandleFirstLaunchTutorialAction(
                            BDModernHandheldControlTarget.ControlAction.Progression,
                            FirstLaunchTutorialInputSource.Keyboard
                        );
                        return true;

                    case FirstLaunchTutorialStep.Dodge:
                        bool dodged =
                            TryRegisterFirstLaunchTutorialDirectionalDodge(
                                localHit.x < 0f,
                                localHit.x >= 0f
                            );
                        if (dodged)
                            HandleFirstLaunchTutorialDodge();
                        else
                            ShowFirstLaunchTutorialSuccess(
                                "DOUBLE-CLICK A SCREEN SIDE TO DODGE"
                            );
                        return true;
                }
            }

            if (secondaryPressed)
            {
                switch (firstLaunchTutorialStep)
                {
                    case FirstLaunchTutorialStep.HeavyAttack:
                    case FirstLaunchTutorialStep.Grapple:
                    case FirstLaunchTutorialStep.HazardKnockback:
                    case FirstLaunchTutorialStep.Parry:
                    case FirstLaunchTutorialStep.CombinedEncounter:
                    case FirstLaunchTutorialStep.MiniBossPhaseOne:
                    case FirstLaunchTutorialStep.MiniBossPhaseTwo:
                        HandleFirstLaunchTutorialAction(
                            BDModernHandheldControlTarget.ControlAction.Credits,
                            FirstLaunchTutorialInputSource.Keyboard
                        );
                        return true;
                }
            }

            if (rangedPressed)
            {
                switch (firstLaunchTutorialStep)
                {
                    case FirstLaunchTutorialStep.RangedAttack:
                    case FirstLaunchTutorialStep.ChargedShot:
                    case FirstLaunchTutorialStep.CombinedEncounter:
                    case FirstLaunchTutorialStep.MiniBossPhaseOne:
                    case FirstLaunchTutorialStep.MiniBossPhaseTwo:
                        HandleFirstLaunchTutorialAction(
                            BDModernHandheldControlTarget.ControlAction.Progression,
                            FirstLaunchTutorialInputSource.Keyboard
                        );
                        return true;
                }
            }

            return false;
        }

        private bool TryResolveFirstLaunchTutorialPhysicalScreenPointV101122(
            Vector2 screenPosition,
            out Vector3 localHit)
        {
            localHit = Vector3.zero;
            if (deviceCamera == null || deviceVisualRoot == null)
                return false;

            Ray ray = deviceCamera.ScreenPointToRay(screenPosition);
            Vector3 localScreenCenter = new Vector3(
                0f,
                ScreenCenterY,
                -0.555f
            );
            Vector3 worldScreenCenter =
                deviceVisualRoot.TransformPoint(localScreenCenter);
            Vector3 worldNormal =
                deviceVisualRoot.TransformDirection(Vector3.forward);
            Plane screenPlane = new Plane(worldNormal, worldScreenCenter);
            if (!screenPlane.Raycast(ray, out float distance) || distance < 0f)
                return false;

            localHit = deviceVisualRoot.InverseTransformPoint(
                ray.GetPoint(distance)
            );
            const float tolerance = 0.12f;
            return Mathf.Abs(localHit.x) <= ScreenWidth * 0.5f + tolerance &&
                   Mathf.Abs(localHit.y - ScreenCenterY) <=
                       ScreenHeight * 0.5f + tolerance;
        }

        private static bool IsFirstLaunchTutorialWorldMouseButtonHeldV101122(
            int button)
        {
            bool held = false;
#if ENABLE_INPUT_SYSTEM
            if (Mouse.current != null)
            {
                if (button == 0)
                    held |= Mouse.current.leftButton.isPressed;
                else if (button == 1)
                    held |= Mouse.current.rightButton.isPressed;
                else if (button == 2)
                    held |= Mouse.current.middleButton.isPressed;
            }
#endif
#if ENABLE_LEGACY_INPUT_MANAGER
            held |= Input.GetMouseButton(button);
#endif
            return held;
        }

        private bool IsFirstLaunchTutorialWorldMouseMovementStepV101122()
        {
            return firstLaunchTutorialStep == FirstLaunchTutorialStep.Move ||
                   firstLaunchTutorialStep == FirstLaunchTutorialStep.RideHorse ||
                   firstLaunchTutorialStep == FirstLaunchTutorialStep.MountedImpact ||
                   firstLaunchTutorialStep == FirstLaunchTutorialStep.SidePath;
        }


        private bool IsFirstLaunchTutorialPointerOverPhysicalScreenV101121(
            Vector2 screenPosition)
        {
            if (deviceCamera == null || deviceVisualRoot == null)
                return false;

            Ray ray = deviceCamera.ScreenPointToRay(screenPosition);
            Vector3 localScreenCenter = new Vector3(
                0f,
                ScreenCenterY,
                -0.555f
            );
            Vector3 worldScreenCenter =
                deviceVisualRoot.TransformPoint(localScreenCenter);
            Vector3 worldNormal =
                deviceVisualRoot.TransformDirection(Vector3.forward);
            Plane screenPlane = new Plane(worldNormal, worldScreenCenter);
            if (!screenPlane.Raycast(ray, out float distance) || distance < 0f)
                return false;

            Vector3 localHit = deviceVisualRoot.InverseTransformPoint(
                ray.GetPoint(distance)
            );
            const float tolerance = 0.16f;
            return Mathf.Abs(localHit.x) <= ScreenWidth * 0.5f + tolerance &&
                   Mathf.Abs(localHit.y - ScreenCenterY) <=
                       ScreenHeight * 0.5f + tolerance;
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

            // Jump has its own unlock and screen-state contract. It must not be
            // rejected by a generic melee/action lock after the Jump screen opens.
            if (ReadFirstLaunchTutorialJumpPressed() &&
                IsFirstLaunchTutorialMechanicUnlocked(2) &&
                !firstLaunchTutorialMounted &&
                !firstLaunchTutorialLessonCompleteAwaitingTravel &&
                !firstLaunchTutorialLessonScreenTransitionActive)
            {
                RequestFirstLaunchTutorialJump();
                return;
            }

            if (IsFirstLaunchTutorialActionInputLocked() ||
                ShouldBlockFirstLaunchTutorialActionForTravel())
            {
                return;
            }

            if (IsFirstLaunchTutorialMechanicUnlocked(3) &&
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

            if (IsFirstLaunchTutorialMechanicUnlocked(11) &&
                ReadFirstLaunchTutorialRangedPressed())
            {
                if (IsFirstLaunchTutorialBossCombatStep())
                    BeginFirstLaunchTutorialBossChargeInput();
                else if (firstLaunchTutorialStep ==
                         FirstLaunchTutorialStep.ChargedShot)
                    BeginFirstLaunchTutorialChargedShotInput();
                else
                    HandleFirstLaunchTutorialAction(
                        BDModernHandheldControlTarget.ControlAction.Progression,
                        ResolveFirstLaunchTutorialSource(
                            FirstLaunchTutorialInputSource.Keyboard
                        )
                    );
                return;
            }

            if (IsFirstLaunchTutorialMechanicUnlocked(4) &&
                ReadFirstLaunchTutorialLightPressed())
            {
                HandleFirstLaunchTutorialAction(
                    BDModernHandheldControlTarget.ControlAction.Primary,
                    ResolveFirstLaunchTutorialSource(
                        FirstLaunchTutorialInputSource.Keyboard
                    )
                );
                return;
            }

            if (IsFirstLaunchTutorialMechanicUnlocked(6) &&
                TryReadFirstLaunchTutorialDirectionalDodge())
            {
                HandleFirstLaunchTutorialDodge();
                return;
            }

            if (IsFirstLaunchTutorialMechanicUnlocked(5) &&
                ReadFirstLaunchTutorialHeavyPressed())
            {
                HandleFirstLaunchTutorialAction(
                    BDModernHandheldControlTarget.ControlAction.Credits,
                    ResolveFirstLaunchTutorialSource(
                        FirstLaunchTutorialInputSource.Keyboard
                    )
                );
            }
        }

        private bool IsFirstLaunchTutorialWorldLightPress()
        {
#if ENABLE_INPUT_SYSTEM
            if (Mouse.current != null &&
                Mouse.current.leftButton.wasPressedThisFrame)
            {
                return hoveredTarget == null ||
                       hoveredTarget.Action ==
                           BDModernHandheldControlTarget.ControlAction.ScreenItem;
            }
#endif
#if ENABLE_LEGACY_INPUT_MANAGER
            if (Input.GetMouseButtonDown(0))
            {
                return hoveredTarget == null ||
                       hoveredTarget.Action ==
                           BDModernHandheldControlTarget.ControlAction.ScreenItem;
            }
#endif
            return false;
        }

        private bool IsFirstLaunchTutorialWorldHeavyPress()
        {
#if ENABLE_INPUT_SYSTEM
            if (Mouse.current != null &&
                Mouse.current.rightButton.wasPressedThisFrame)
            {
                return hoveredTarget == null ||
                       hoveredTarget.Action ==
                           BDModernHandheldControlTarget.ControlAction.ScreenItem;
            }
#endif
#if ENABLE_LEGACY_INPUT_MANAGER
            if (Input.GetMouseButtonDown(1))
            {
                return hoveredTarget == null ||
                       hoveredTarget.Action ==
                           BDModernHandheldControlTarget.ControlAction.ScreenItem;
            }
#endif
            return false;
        }

        private bool ReadFirstLaunchTutorialRangedPressed()
        {
#if ENABLE_INPUT_SYSTEM
            if (Keyboard.current != null &&
                Keyboard.current.qKey.wasPressedThisFrame)
                return true;
            if (Gamepad.current != null &&
                Gamepad.current.rightShoulder.wasPressedThisFrame)
                return true;
#endif
#if ENABLE_LEGACY_INPUT_MANAGER
            if (Input.GetKeyDown(KeyCode.Q) ||
                Input.GetKeyDown(KeyCode.JoystickButton5))
                return true;
#endif
            return false;
        }

        private void HandleFirstLaunchTutorialFreePlayAction(
            BDModernHandheldControlTarget.ControlAction action,
            FirstLaunchTutorialInputSource source)
        {
            Vector2 movement = ResolveFirstLaunchTutorialMovementAction(action);
            if (movement.sqrMagnitude > 0.0001f)
            {
                bool directionalDodge =
                    IsFirstLaunchTutorialMechanicUnlocked(6) &&
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

            if (ShouldBlockFirstLaunchTutorialActionForTravel())
                return;

            if (firstLaunchTutorialMounted &&
                (action == BDModernHandheldControlTarget.ControlAction.Primary ||
                 action == BDModernHandheldControlTarget.ControlAction.Credits))
            {
                RejectFirstLaunchTutorialMountedMelee();
                return;
            }

            switch (action)
            {
                case BDModernHandheldControlTarget.ControlAction.Confirm:
                    if (!IsFirstLaunchTutorialMechanicUnlocked(3))
                        return;
                    if (firstLaunchTutorialStep ==
                            FirstLaunchTutorialStep.SidePath &&
                        TryHandleFirstLaunchTutorialProductionAction(action))
                    {
                        return;
                    }
                    HandleFirstLaunchTutorialInteractAction();
                    break;

                case BDModernHandheldControlTarget.ControlAction.Primary:
                    if (!IsFirstLaunchTutorialMechanicUnlocked(4))
                        return;
                    if (TryResolveFirstLaunchTutorialUnifiedParry())
                        return;
                    if (firstLaunchTutorialStep ==
                            FirstLaunchTutorialStep.HazardKnockback &&
                        TryHandleFirstLaunchTutorialProductionAction(action))
                    {
                        return;
                    }
                    firstLaunchTutorialPrimaryHoldStartedAt = Time.unscaledTime;
                    firstLaunchTutorialProductionLightResolved = false;
                    break;

                case BDModernHandheldControlTarget.ControlAction.Progression:
                    if (ShouldRouteFirstLaunchTutorialProgressionToHealing())
                    {
                        firstLaunchTutorialHealHoldStartedAt = Time.unscaledTime;
                        return;
                    }
                    if (!IsFirstLaunchTutorialMechanicUnlocked(11))
                        return;
                    if (IsFirstLaunchTutorialBossCombatStep())
                        BeginFirstLaunchTutorialBossChargeInput();
                    else if (firstLaunchTutorialStep ==
                             FirstLaunchTutorialStep.ChargedShot)
                        BeginFirstLaunchTutorialChargedShotInput();
                    else
                        HandleFirstLaunchTutorialRangedAttack();
                    break;

                case BDModernHandheldControlTarget.ControlAction.ContextBackSettings:
                    if (IsFirstLaunchTutorialMechanicUnlocked(2))
                        RequestFirstLaunchTutorialJump();
                    break;

                case BDModernHandheldControlTarget.ControlAction.Credits:
                    if (!IsFirstLaunchTutorialMechanicUnlocked(5))
                        return;
                    if (TryResolveFirstLaunchTutorialUnifiedParry())
                        return;
                    if (firstLaunchTutorialStep ==
                            FirstLaunchTutorialStep.HazardKnockback &&
                        TryHandleFirstLaunchTutorialProductionAction(action))
                    {
                        return;
                    }
                    firstLaunchTutorialGrappleHoldStartedAt = Time.unscaledTime;
                    firstLaunchTutorialProductionHeavyResolved = false;
                    break;
            }
        }

        private bool ShouldRouteFirstLaunchTutorialProgressionToHealing()
        {
            return firstLaunchTutorialStep == FirstLaunchTutorialStep.HealHorse &&
                   firstLaunchTutorialHorseInjured &&
                   !firstLaunchTutorialMounted &&
                   Vector2.Distance(
                       firstLaunchTutorialPlayerWorldPosition,
                       firstLaunchTutorialHorseWorldPosition
                   ) <= TutorialMountRange + 22f;
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

            // Collectibles are contact-owned; interact never completes them.
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
                BeginFirstLaunchTutorialRelicCompletion();
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

            bool attackLesson =
                firstLaunchTutorialStep == FirstLaunchTutorialStep.AttackEnemy &&
                firstLaunchTutorialGrounded;
            bool jumpAttackLesson =
                firstLaunchTutorialStep == FirstLaunchTutorialStep.JumpAttack &&
                !firstLaunchTutorialGrounded;
            bool lessonAttempt = attackLesson || jumpAttackLesson;

            TutorialEnemyActor targetActor = lessonAttempt
                ? PrepareFirstLaunchTutorialPrimaryMeleeTarget(1f)
                : FindClosestLivingTutorialActor(TutorialAttackRange);
            bool validHit = targetActor != null &&
                IsFirstLaunchTutorialTargetInFront(
                    targetActor.Position,
                    TutorialAttackRange
                );
            bool advances = lessonAttempt && validHit &&
                targetActor.Image == firstLaunchTutorialEnemy;

            if (validHit)
            {
                if (advances)
                {
                    BeginFirstLaunchTutorialMeleeTransaction(
                        targetActor,
                        targetActor.Health,
                        TutorialAttackRange,
                        heavy: false
                    );
                }
                else
                {
                    BeginFirstLaunchTutorialMeleeTransaction(
                        targetActor,
                        1f,
                        TutorialAttackRange,
                        heavy: false
                    );
                }
            }
            else if (lessonAttempt)
            {
                ShowFirstLaunchTutorialSuccess(
                    jumpAttackLesson
                        ? "JUMP, FACE THE TARGET, THEN ATTACK"
                        : "MOVE INTO RANGE"
                );
            }

            PlayFirstLaunchTutorialLightAttackAnimation(advances);
        }

        private void HandleFirstLaunchTutorialRangedAttack()
        {
            if (Time.unscaledTime < firstLaunchTutorialReloadCompletesAt)
                return;
            if (firstLaunchTutorialAmmo <= 0)
            {
                BeginFirstLaunchTutorialReload();
                return;
            }

            bool rangedLesson =
                firstLaunchTutorialStep == FirstLaunchTutorialStep.RangedAttack;
            TutorialEnemyActor targetActor =
                FindClosestLivingTutorialActor(520f);
            bool advances = rangedLesson &&
                firstLaunchTutorialMounted &&
                targetActor != null &&
                targetActor.Image == firstLaunchTutorialEnemy &&
                IsFirstLaunchTutorialTargetInFront(targetActor.Position, 460f);

            if (rangedLesson && !firstLaunchTutorialMounted)
                ShowFirstLaunchTutorialSuccess("MOUNT TO ADVANCE — SHOT STILL FIRES");
            else if (rangedLesson && !advances)
                ShowFirstLaunchTutorialSuccess("RIDE CLOSER");

            Vector2 targetWorld = targetActor != null
                ? targetActor.Position
                : firstLaunchTutorialPlayerWorldPosition +
                  ResolveFirstLaunchTutorialActionDirection() * 360f;
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
                targetWorld,
                advances,
                chargedShot: false
            );
            if (firstLaunchTutorialAmmo <= 0)
                BeginFirstLaunchTutorialReload();
        }

        private void HandleFirstLaunchTutorialDodge()
        {
            bool dodgeLesson =
                firstLaunchTutorialStep == FirstLaunchTutorialStep.Dodge;
            bool nearHazard = Vector2.Distance(
                firstLaunchTutorialPlayerWorldPosition,
                firstLaunchTutorialHazardWorldPosition
            ) <= TutorialInstructionTriggerRange;

            Vector2 dodgeDirection = firstLaunchTutorialLastMoveDirection;
            bool advances = false;
            if (dodgeLesson && nearHazard)
            {
                firstLaunchTutorialDodgeLessonStartSide = Mathf.Sign(
                    firstLaunchTutorialPlayerWorldPosition.x -
                    firstLaunchTutorialHazardWorldPosition.x
                );
                if (firstLaunchTutorialDodgeLessonStartSide == 0f)
                    firstLaunchTutorialDodgeLessonStartSide = -1f;
                dodgeDirection = new Vector2(
                    -firstLaunchTutorialDodgeLessonStartSide,
                    0f
                );
                advances = true;
            }
            else if (dodgeLesson)
            {
                ShowFirstLaunchTutorialSuccess("APPROACH THE HAZARD");
            }

            if (dodgeDirection.sqrMagnitude < 0.001f)
                dodgeDirection = Vector2.right;
            firstLaunchTutorialInvulnerableUntil = Mathf.Max(
                firstLaunchTutorialInvulnerableUntil,
                Time.unscaledTime + 0.34f
            );
            PlayFirstLaunchTutorialDodgeAnimation(
                dodgeDirection,
                advances
            );
        }

        private void HandleFirstLaunchTutorialHeavyAttack()
        {
            if (RejectFirstLaunchTutorialMountedMelee())
                return;

            bool heavyLesson =
                firstLaunchTutorialStep == FirstLaunchTutorialStep.HeavyAttack;
            float range = TutorialAttackRange + 24f;
            TutorialEnemyActor targetActor = heavyLesson
                ? PrepareFirstLaunchTutorialPrimaryMeleeTarget(2f)
                : FindClosestLivingTutorialActor(range);
            bool validHit = targetActor != null &&
                IsFirstLaunchTutorialTargetInFront(targetActor.Position, range);
            bool advances = heavyLesson && validHit &&
                targetActor.Image == firstLaunchTutorialEnemy;

            if (validHit)
            {
                if (advances)
                {
                    BeginFirstLaunchTutorialMeleeTransaction(
                        targetActor,
                        targetActor.Health,
                        range,
                        heavy: true
                    );
                }
                else
                {
                    BeginFirstLaunchTutorialMeleeTransaction(
                        targetActor,
                        2f,
                        range,
                        heavy: true
                    );
                }
            }
            else if (heavyLesson)
            {
                ShowFirstLaunchTutorialSuccess("MOVE INTO RANGE");
            }

            PlayFirstLaunchTutorialHeavyAttackAnimation(advances);
        }

        private void HandleFirstLaunchTutorialParry()
        {
            if (!TryResolveFirstLaunchTutorialUnifiedParry())
                ShowFirstLaunchTutorialSuccess("PARRY WHEN THE HIT REACHES YOU");
        }

        private void UpdateFirstLaunchTutorialHeldActions()
        {
            if (IsFirstLaunchTutorialActionInputLocked() ||
                ShouldBlockFirstLaunchTutorialActionForTravel())
            {
                return;
            }

            if (IsFirstLaunchTutorialBossCombatStep())
                UpdateFirstLaunchTutorialBossChargeHold();
            else if (firstLaunchTutorialStep ==
                     FirstLaunchTutorialStep.ChargedShot)
                UpdateFirstLaunchTutorialChargedShotHold();

            if (firstLaunchTutorialStep ==
                    FirstLaunchTutorialStep.HealHorse)
                UpdateFirstLaunchTutorialHealingHold();

            if (firstLaunchTutorialMounted)
            {
                firstLaunchTutorialPrimaryHoldStartedAt = -1f;
                firstLaunchTutorialGrappleHoldStartedAt = -1f;
                return;
            }

            if (IsFirstLaunchTutorialMechanicUnlocked(4))
                UpdateFirstLaunchTutorialSpinningHold();
            if (IsFirstLaunchTutorialMechanicUnlocked(5))
                UpdateFirstLaunchTutorialGrappleHoldFreePlay();
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
            if (firstLaunchTutorialPrimaryHoldStartedAt < 0f)
                return;

            float held = Time.unscaledTime -
                firstLaunchTutorialPrimaryHoldStartedAt;
            bool isHeld = IsFirstLaunchTutorialPrimaryHeld();
            bool spinUnlocked = IsFirstLaunchTutorialMechanicUnlocked(15);
            if (spinUnlocked && isHeld &&
                held >= TutorialSpinHoldSeconds)
            {
                firstLaunchTutorialPrimaryHoldStartedAt = -1f;
                bool lesson = firstLaunchTutorialStep ==
                    FirstLaunchTutorialStep.SpinAttack;
                bool advances = lesson &&
                    HasFirstLaunchTutorialAtomicSpinPairInRange();
                PlayFirstLaunchTutorialSpinAttackAnimation(advances);
                if (lesson && !advances)
                {
                    ShowFirstLaunchTutorialSuccess(
                        "CENTER BETWEEN BOTH ENEMIES — HIT BOTH"
                    );
                }
                ShowFirstLaunchTutorialHoldProgress(
                    string.Empty,
                    0f,
                    visible: false
                );
                return;
            }

            if (isHeld)
            {
                if (spinUnlocked)
                {
                    ShowFirstLaunchTutorialHoldProgress(
                        "SPIN CHARGE",
                        Mathf.Clamp01(held / TutorialSpinHoldSeconds),
                        visible: held >= 0.12f
                    );
                }
                return;
            }

            firstLaunchTutorialPrimaryHoldStartedAt = -1f;
            ShowFirstLaunchTutorialHoldProgress(
                string.Empty,
                0f,
                visible: false
            );
            HandleFirstLaunchTutorialLightAttack();
        }

        private void UpdateFirstLaunchTutorialGrappleHoldFreePlay()
        {
            if (firstLaunchTutorialGrappleHoldStartedAt < 0f)
                return;

            float held = Time.unscaledTime -
                firstLaunchTutorialGrappleHoldStartedAt;
            bool isHeld = IsFirstLaunchTutorialHeavyHeld();
            bool grappleUnlocked = IsFirstLaunchTutorialMechanicUnlocked(16);
            if (grappleUnlocked && isHeld &&
                held >= TutorialGrappleHoldSeconds)
            {
                firstLaunchTutorialGrappleHoldStartedAt = -1f;
                TutorialEnemyActor target =
                    FindClosestLivingTutorialActor(560f);
                bool advances = firstLaunchTutorialStep ==
                        FirstLaunchTutorialStep.Grapple &&
                    target != null;
                if (target != null)
                {
                    BeginFirstLaunchTutorialProductionHookTransaction(
                        target,
                        0.5f
                    );
                }
                PlayFirstLaunchTutorialGrappleAnimation(advances);
                if (target == null)
                    ShowFirstLaunchTutorialSuccess("THE HOOK FOUND NOTHING");
                ShowFirstLaunchTutorialHoldProgress(
                    string.Empty,
                    0f,
                    visible: false
                );
                return;
            }

            if (isHeld)
            {
                if (grappleUnlocked)
                {
                    ShowFirstLaunchTutorialHoldProgress(
                        "GRAPPLE LOCK",
                        Mathf.Clamp01(held / TutorialGrappleHoldSeconds),
                        visible: held >= 0.12f
                    );
                }
                return;
            }

            firstLaunchTutorialGrappleHoldStartedAt = -1f;
            ShowFirstLaunchTutorialHoldProgress(
                string.Empty,
                0f,
                visible: false
            );
            HandleFirstLaunchTutorialHeavyAttack();
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
            // V10.11.30.6 lesson-complete guard
            if (firstLaunchTutorialLessonCompleteAwaitingTravel ||
                firstLaunchTutorialLessonScreenTransitionActive)
            {
                SetFirstLaunchTutorialLessonInstructionVisible(false);
                return;
            }

            // BD IMMEDIATE PERSISTENT LESSON UI V10.11.25
            // Step assignment owns room entry. Once the step is active, its
            // instruction is visible immediately and remains until completion.
            SetFirstLaunchTutorialInstructionRequested(true);
        }

        private void ConfigureFirstLaunchTutorialFreePlayScene(
            FirstLaunchTutorialStep step)
        {
            UnlockFirstLaunchTutorialAbilitiesForStep(step);
            SetFirstLaunchTutorialInstructionRequested(false);
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
                case FirstLaunchTutorialStep.JumpAttack:
                    firstLaunchTutorialInstructionRequested = true;
                    SetFirstLaunchTutorialCheckpoint(
                        1190f
                    );
                    firstLaunchTutorialLastMoveDirection = Vector2.right;
                    break;
                case FirstLaunchTutorialStep.WallJump:
                    firstLaunchTutorialInstructionRequested = false;
                    SetFirstLaunchTutorialCheckpoint(
                        firstLaunchTutorialPlayerWorldPosition.x
                    );
                    firstLaunchTutorialGroundedY = TutorialGroundY;
                    firstLaunchTutorialWallJumpPlatformReached = false;
                    firstLaunchTutorialWallJumpConsumed = false;
                    firstLaunchTutorialLastMoveDirection = Vector2.right;
                    break;
                case FirstLaunchTutorialStep.MountHorse:
                case FirstLaunchTutorialStep.RemountHorse:
                    // BD IMMEDIATE POST-JUMP MOUNT TEACHING V10.11.17
                    firstLaunchTutorialInstructionRequested = true;
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

            // BD IMMEDIATE PERSISTENT LESSON UI V10.11.25
            firstLaunchTutorialInstructionRequested = true;

            ConfigureFirstLaunchTutorialProductionStep(step);
            bool wallJumpActive =
                step == FirstLaunchTutorialStep.WallJump;
            SetTutorialEntityActive(
                firstLaunchTutorialWallJumpWall,
                wallJumpActive
            );
            SetTutorialEntityActive(
                firstLaunchTutorialWallJumpPlatform,
                wallJumpActive
            );
            SetTutorialEntityActive(
                firstLaunchTutorialWallJumpUpperGround,
                wallJumpActive
            );

            bool initialInstructionRequest =
                firstLaunchTutorialInstructionRequested;
            firstLaunchTutorialInstructionRequested = false;
            SetFirstLaunchTutorialInstructionRequested(
                initialInstructionRequest
            );
        }

        private void UnlockFirstLaunchTutorialAbilitiesForStep(
            FirstLaunchTutorialStep step)
        {
            switch (step)
            {
                case FirstLaunchTutorialStep.Jump:
                    firstLaunchTutorialJumpUnlocked = true;
                    break;
                case FirstLaunchTutorialStep.MountHorse:
                    firstLaunchTutorialInteractUnlocked = true;
                    break;
                case FirstLaunchTutorialStep.AttackEnemy:
                    firstLaunchTutorialLightAttackUnlocked = true;
                    break;
                case FirstLaunchTutorialStep.HeavyAttack:
                    firstLaunchTutorialHeavyAttackUnlocked = true;
                    break;
                case FirstLaunchTutorialStep.Dodge:
                    firstLaunchTutorialDodgeUnlocked = true;
                    break;
                case FirstLaunchTutorialStep.Parry:
                    firstLaunchTutorialParryUnlocked = true;
                    break;
                case FirstLaunchTutorialStep.RangedAttack:
                    firstLaunchTutorialRangedUnlocked = true;
                    break;
            }
        }

        private bool RequireFirstLaunchTutorialAbility(
            bool unlocked,
            string abilityName)
        {
            if (unlocked)
                return true;

            if (Time.unscaledTime >=
                firstLaunchTutorialLockedAbilityFeedbackAt)
            {
                firstLaunchTutorialLockedAbilityFeedbackAt =
                    Time.unscaledTime + 0.85f;
                ShowFirstLaunchTutorialSuccess(
                    abilityName + " UNLOCKS IN ITS LESSON"
                );
            }
            return false;
        }

        private void ReleaseFirstLaunchTutorialInstructionForScreenTransition()
        {
            // Proximity is not allowed to hide an active lesson, but verified
            // lesson completion is an explicit owner transition and must release
            // the latch before the composed card is disabled.
            firstLaunchTutorialInstructionLatchedV101123 = false;
            bool changed = firstLaunchTutorialInstructionRequested;
            firstLaunchTutorialInstructionRequested = false;
            firstLaunchTutorialInstructionVisibility = 0f;
            ClearFirstLaunchTutorialPhysicalHighlight();
            if (changed && firstLaunchTutorialProgress != null)
            {
                firstLaunchTutorialProgress.text =
                    FirstLaunchTutorialLessonCompleteTravelMessage;
            }
        }

        private void SetFirstLaunchTutorialInstructionRequested(
            bool requested)
        {
            // A proximity check may introduce a lesson, but it may never hide
            // an already introduced lesson. Only SetFirstLaunchTutorialStep
            // releases the latch after verified completion of the current step.
            if (!requested && firstLaunchTutorialInstructionLatchedV101123)
                return;

            if (requested)
                firstLaunchTutorialInstructionLatchedV101123 = true;

            bool changed =
                firstLaunchTutorialInstructionRequested != requested;
            firstLaunchTutorialInstructionRequested = requested;

            if (requested)
            {
                if (changed)
                {
                    firstLaunchTutorialInstructionStartedAt =
                        Time.unscaledTime;
                }
                UpdateFirstLaunchTutorialPrompt();
                UpdateFirstLaunchTutorialPhysicalHighlight();
            }
            else if (changed)
            {
                ClearFirstLaunchTutorialPhysicalHighlight();
                if (firstLaunchTutorialProgress != null)
                    firstLaunchTutorialProgress.text =
                        FirstLaunchTutorialLessonCompleteTravelMessage;
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
                case FirstLaunchTutorialStep.JumpAttack:
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
                case FirstLaunchTutorialStep.WallJump:
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
            float normalCameraTarget =
                firstLaunchTutorialPlayerWorldPosition.x +
                facingLead * leadDistance;
            float targetCamera = Mathf.Clamp(
                ResolveFirstLaunchTutorialScreenCameraTarget(
                    normalCameraTarget
                ),
                minimumCamera,
                maximumCamera
            );
            if (!force)
                targetCamera = Mathf.Max(firstLaunchTutorialCameraWorldX, targetCamera);

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

            firstLaunchTutorialProgressFloorX = Mathf.Max(
                firstLaunchTutorialProgressFloorX,
                firstLaunchTutorialCameraWorldX -
                    TutorialWorldViewportHalfWidth + 54f
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
                float horseFacing =
                    Mathf.Abs(firstLaunchTutorialLastMoveDirection.x) > 0.01f
                        ? Mathf.Sign(firstLaunchTutorialLastMoveDirection.x)
                        : 1f;
                firstLaunchTutorialHorse.rectTransform.localScale =
                    new Vector3(horseFacing, 1f, 1f);
                firstLaunchTutorialHorse.rectTransform.localRotation =
                    Quaternion.identity;
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
                // BD CONTEXTUAL PLAYER FACING V10.11.30
                facing = ResolveFirstLaunchTutorialPresentationFacingV101130(facing);
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

            // BD PLAYER ART FINAL OWNER V10.11.17
            // Re-assert the authored player sprite after every legacy/course
            // renderer so no generic tutorial tint can replace the blond hair,
            // red shirt and blue pants presentation.
            ApplyFirstLaunchTutorialPlayerVisualPolish();
        }

        private static Vector2 SnapFirstLaunchTutorialWorldPosition(
            Vector2 value)
        {
            return new Vector2(
                SnapFirstLaunchTutorialPixelValue(value.x),
                SnapFirstLaunchTutorialPixelValue(value.y)
            );
        }

        // BD MOUNT LESSON PERSISTENCE V10.11.17
        // Mount guidance persists until the mount animation really completes;
        // only that completion advances MountHorse to RideHorse.
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

            if (!firstLaunchTutorialPendingMeleeHitResolved ||
                !IsFirstLaunchTutorialPrimaryEnemyDead())
            {
                ShowFirstLaunchTutorialSuccess("THE TARGET SURVIVED — TRY AGAIN");
                return;
            }

            if (firstLaunchTutorialStep ==
                    FirstLaunchTutorialStep.JumpAttack)
            {
                ShowFirstLaunchTutorialSuccess("AIR ATTACK");
                SetFirstLaunchTutorialStep(
                    FirstLaunchTutorialStep.HorseReturn
                );
                return;
            }

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
            if (!advancesLesson)
                return;

            if (!DidFirstLaunchTutorialDodgeCrossObjective())
            {
                ShowFirstLaunchTutorialSuccess(
                    "DODGE THROUGH AND CROSS THE HAZARD"
                );
                return;
            }

            SetFirstLaunchTutorialLearningState(
                "Dodge",
                TutorialLearningState.Demonstrated
            );
            ShowFirstLaunchTutorialSuccess("EVADED");
            SetFirstLaunchTutorialStep(
                FirstLaunchTutorialStep.Parry
            );
        }

        private void CompleteFirstLaunchTutorialHeavyAttackAnimation(
            bool advancesLesson)
        {
            if (!advancesLesson)
                return;

            if (!firstLaunchTutorialPendingMeleeHitResolved ||
                !IsFirstLaunchTutorialPrimaryEnemyDead())
            {
                ShowFirstLaunchTutorialSuccess("THE TARGET SURVIVED — TRY AGAIN");
                return;
            }

            ShowFirstLaunchTutorialSuccess("HEAVY IMPACT");
            SetFirstLaunchTutorialStep(
                FirstLaunchTutorialStep.Dodge
            );
        }

        private void CompleteFirstLaunchTutorialSpinAttackAnimation(
            bool advancesLesson)
        {
            if (advancesLesson)
            {
                if (!ResolveFirstLaunchTutorialAtomicSpinPairAtImpact())
                {
                    ShowFirstLaunchTutorialSuccess(
                        "BOTH ENEMIES MUST BE HIT BY THE SAME SPIN"
                    );
                    return;
                }
                ShowFirstLaunchTutorialSuccess("GROUP CLEARED");
                SetFirstLaunchTutorialStep(
                    FirstLaunchTutorialStep.Grapple
                );
                return;
            }

            bool hit = ResolveFirstLaunchTutorialFreeSpinAtImpact();
            SetFirstLaunchTutorialLearningState(
                "Spin",
                hit
                    ? TutorialLearningState.Demonstrated
                    : TutorialLearningState.Attempted
            );
            if (!hit)
                ShowFirstLaunchTutorialSuccess("NO TARGET IN RANGE");
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
                    FirstLaunchTutorialStep.JumpAttack
                );
            }
        }

        private void CompleteFirstLaunchTutorialGrappleAnimation(
            bool advancesLesson)
        {
            CompleteFirstLaunchTutorialProductionHookTransaction();
            if (!advancesLesson)
                return;

            TutorialEnemyActor pulled =
                FindClosestLivingTutorialActor(180f, requireForward: false);
            if (pulled == null)
            {
                ShowFirstLaunchTutorialSuccess(
                    "THE TARGET WAS NOT PULLED INTO RANGE"
                );
                return;
            }

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
