using UnityEngine;
using UnityEngine.UI;

namespace BoredomAndDungeons
{
    public sealed partial class BDModernHandheld3DPresenter
    {
        private const float TutorialLessonScreenExitOffset = 342f;
        private const float TutorialLessonScreenSpacing = 780f;
        private const float TutorialLessonScreenHandoffSeconds = 0.72f;
        private const float TutorialHorseReturnPostHandoffDelaySeconds = 0.14f;
        // BD SPIN PROXIMITY + MID-SCREEN DISMOUNT PROMPT V10.11.30.35
        private const float TutorialSpinEnemyOffsetV1011335 = 82f;
        private const float TutorialDismountInstructionRevealOffsetV1011335 =
            -150f;
        private const float TutorialOpeningScreenCenterX = -650f;
        private static readonly Vector2 TutorialContinueEffectRestPosition =
            new Vector2(0f, -92f);

        private bool firstLaunchTutorialLessonScreenInitialized;
        private bool firstLaunchTutorialLessonCompleteAwaitingTravel;
        private bool firstLaunchTutorialContinuousHandoffActive;
        private bool firstLaunchTutorialApplyingScreenStep;
        private bool firstLaunchTutorialHandoffTargetMounted;
        private bool firstLaunchTutorialHandoffHorseStagedInTargetRoom;
        private bool firstLaunchTutorialHandoffDefersHorseUntilSettled;
        private RectTransform firstLaunchTutorialContinueEffectRoot;
        private CanvasGroup firstLaunchTutorialContinueEffectCanvasGroup;
        private Text firstLaunchTutorialContinueEffectLabel;
        private Text firstLaunchTutorialContinueEffectArrow;
        private float firstLaunchTutorialContinueEffectStartedAt;
        // BD SINGLE-OWNER PARRY TRANSACTION V10.11.30.26
        private bool firstLaunchTutorialParryResolvedV1011326;
        private FirstLaunchTutorialStep firstLaunchTutorialPendingScreenStep;
        private float firstLaunchTutorialLessonScreenCenterX;
        private float firstLaunchTutorialLessonScreenExitX;
        private float firstLaunchTutorialHandoffStartedAt;
        private float firstLaunchTutorialHandoffStartCameraX;
        private float firstLaunchTutorialHandoffTargetCameraX;
        private Vector2 firstLaunchTutorialHandoffStartPlayerPosition;
        private Vector2 firstLaunchTutorialHandoffTargetPlayerPosition;
        private Vector2 firstLaunchTutorialHandoffStartHorsePosition;
        private Vector2 firstLaunchTutorialHandoffTargetHorsePosition;
        private float firstLaunchTutorialDodgeLessonStartSide;
        private bool
            firstLaunchTutorialDismountInstructionRevealedV1011335;

        private bool ShouldBlockFirstLaunchTutorialActionForTravel()
        {
            return firstLaunchTutorialLessonCompleteAwaitingTravel ||
                firstLaunchTutorialContinuousHandoffActive;
        }

        private bool ShouldQueueFirstLaunchTutorialStepForNextScreen(
            FirstLaunchTutorialStep next)
        {
            if (firstLaunchTutorialApplyingScreenStep ||
                !firstLaunchTutorialLessonScreenInitialized ||
                firstLaunchTutorialEntryPhase !=
                    FirstLaunchTutorialEntryPhase.Playing ||
                next == firstLaunchTutorialStep ||
                next == FirstLaunchTutorialStep.Completed)
            {
                return false;
            }

            if (ShouldKeepFirstLaunchTutorialStepsOnSameScreen(
                    firstLaunchTutorialStep,
                    next))
            {
                return false;
            }

            return true;
        }

        private static bool ShouldKeepFirstLaunchTutorialStepsOnSameScreen(
            FirstLaunchTutorialStep current,
            FirstLaunchTutorialStep next)
        {
            if (current == FirstLaunchTutorialStep.WhiteBoot &&
                next == FirstLaunchTutorialStep.Move)
            {
                return true;
            }

            // The opening room owns only walk + jump. Quick/Heavy/Dodge/
            // Parry follow before the deferred horse room. Mount + Ride share
            // one room immediately before the horse-shot story room.
            if (current == FirstLaunchTutorialStep.Move &&
                next == FirstLaunchTutorialStep.Jump)
            {
                return true;
            }
            if (current == FirstLaunchTutorialStep.MountHorse &&
                next == FirstLaunchTutorialStep.RideHorse)
            {
                return true;
            }

            // BD MOUNTED RELOAD SAME-ROOM BRIDGE V10.11.30.31
            // Reload is the automatic consequence of the ordinary mounted shot,
            // not an empty standalone room. Keep its readable beat in the same
            // room, then require travel to the populated Charged Shot room.
            if (current == FirstLaunchTutorialStep.RangedAttack &&
                next == FirstLaunchTutorialStep.Reload)
            {
                return true;
            }

            // Story beats and boss phases are not new mechanic lessons. They
            // remain inside the screen that introduces the related lesson.
            if (current == FirstLaunchTutorialStep.EnemyArrival &&
                next == FirstLaunchTutorialStep.HorseShot)
            {
                return true;
            }
            // BD HORSE SHOOTER SURVIVES STORY BEAT V10.11.30.29
            // The enemy who wounds the horse remains in the same room as the
            // airborne counterattack lesson. It is removed only by the player's
            // real visible kill, never by a room reset.
            if (current == FirstLaunchTutorialStep.HorseShot &&
                next == FirstLaunchTutorialStep.JumpAttack)
            {
                return true;
            }
            if (current == FirstLaunchTutorialStep.HorseReturn &&
                next == FirstLaunchTutorialStep.HealHorse)
            {
                return true;
            }
            // BD HEAL PET MOUNT SAME ROOM V10.11.30.42
            // Healing reveals Pet immediately beside the same horse. Finishing
            // the pet animation reveals Mount Again without Continue, travel,
            // camera motion, a room reset or a second horse spawn.
            if (current == FirstLaunchTutorialStep.HealHorse &&
                next == FirstLaunchTutorialStep.PetHorse)
            {
                return true;
            }
            if (current == FirstLaunchTutorialStep.PetHorse &&
                next == FirstLaunchTutorialStep.RemountHorse)
            {
                return true;
            }
            if (current == FirstLaunchTutorialStep.MiniBossIntro &&
                next == FirstLaunchTutorialStep.MiniBossPhaseOne)
            {
                return true;
            }
            if (current == FirstLaunchTutorialStep.MiniBossPhaseOne &&
                next == FirstLaunchTutorialStep.MiniBossPhaseTwo)
            {
                return true;
            }
            if (current == FirstLaunchTutorialStep.MiniBossPhaseTwo &&
                next == FirstLaunchTutorialStep.MiniBossDefeated)
            {
                return true;
            }
            return false;
        }

        private void QueueFirstLaunchTutorialStepForNextScreen(
            FirstLaunchTutorialStep next)
        {
            if (firstLaunchTutorialLessonCompleteAwaitingTravel &&
                firstLaunchTutorialPendingScreenStep == next)
            {
                return;
            }

            firstLaunchTutorialPendingScreenStep = next;
            firstLaunchTutorialLessonCompleteAwaitingTravel = true;

            // Every room handoff is player-owned. Completing a lesson hides its
            // instruction, but the next room cannot begin until the player
            // physically reaches the visible right edge of the current room.
            firstLaunchTutorialLessonScreenExitX = Mathf.Clamp(
                firstLaunchTutorialLessonScreenCenterX +
                    TutorialLessonScreenExitOffset,
                TutorialWorldMinX + 24f,
                TutorialWorldMaxX - 28f
            );

            SuspendFirstLaunchTutorialCompletedLessonWorld();
            SetFirstLaunchTutorialLessonInstructionVisible(false);
            ShowFirstLaunchTutorialContinueEffect();
            if (firstLaunchTutorialFeedback != null)
            {
                firstLaunchTutorialFeedback.text =
                    FirstLaunchTutorialLessonCompleteTravelMessage;
                firstLaunchTutorialFeedbackClearAt = float.PositiveInfinity;
            }
            PlayClick();
        }

        private void SetFirstLaunchTutorialLessonInstructionVisible(
            bool visible)
        {
            if (visible)
            {
                HideFirstLaunchTutorialContinueEffect();
                SetFirstLaunchTutorialInstructionRequested(true);
            }
            else
                ReleaseFirstLaunchTutorialInstructionForScreenTransition();

            if (firstLaunchTutorialInstructionCanvasGroup != null)
            {
                firstLaunchTutorialInstructionCanvasGroup.interactable = false;
                firstLaunchTutorialInstructionCanvasGroup.blocksRaycasts = false;
                if (!visible)
                    firstLaunchTutorialInstructionCanvasGroup.alpha = 0f;
            }

            if (firstLaunchTutorialInstructionPanel != null)
            {
                firstLaunchTutorialInstructionPanel.gameObject.SetActive(visible);
            }
            else if (firstLaunchTutorialInstructionRect != null)
            {
                firstLaunchTutorialInstructionRect.gameObject.SetActive(visible);
            }

            if (firstLaunchTutorialProgress != null)
                firstLaunchTutorialProgress.gameObject.SetActive(visible);

            if (firstLaunchTutorialGameplayRoot != null)
            {
                Transform instructionShadow =
                    firstLaunchTutorialGameplayRoot.transform.Find(
                        "Tutorial Instruction Shadow"
                    );
                if (instructionShadow != null)
                    instructionShadow.gameObject.SetActive(visible);
            }

            if (!visible)
            {
                firstLaunchTutorialInstructionVisibility = 0f;
                if (firstLaunchTutorialHoldFill != null)
                    firstLaunchTutorialHoldFill.gameObject.SetActive(false);
                ClearFirstLaunchTutorialPhysicalHighlight();
                return;
            }

            UpdateFirstLaunchTutorialPrompt();
            BeginFirstLaunchTutorialInstructionPresentation(
                firstLaunchTutorialStep
            );
            UpdateFirstLaunchTutorialPhysicalHighlight();
        }

        private float
            ResolveFirstLaunchTutorialDismountInstructionRevealX()
        {
            return firstLaunchTutorialLessonScreenCenterX +
                TutorialDismountInstructionRevealOffsetV1011335;
        }

        private void
            HideFirstLaunchTutorialDismountInstructionUntilMarkerV1011335()
        {
            // This is an introduction gate, not lesson completion. Hide the
            // card without publishing CONTINUE or a completion message.
            firstLaunchTutorialInstructionRequested = false;
            firstLaunchTutorialInstructionVisibility = 0f;

            if (firstLaunchTutorialInstructionCanvasGroup != null)
            {
                firstLaunchTutorialInstructionCanvasGroup.alpha = 0f;
                firstLaunchTutorialInstructionCanvasGroup.interactable = false;
                firstLaunchTutorialInstructionCanvasGroup.blocksRaycasts = false;
            }

            if (firstLaunchTutorialInstructionPanel != null)
            {
                firstLaunchTutorialInstructionPanel.gameObject.SetActive(false);
            }
            else if (firstLaunchTutorialInstructionRect != null)
            {
                firstLaunchTutorialInstructionRect.gameObject.SetActive(false);
            }

            if (firstLaunchTutorialProgress != null)
                firstLaunchTutorialProgress.gameObject.SetActive(false);
            if (firstLaunchTutorialHoldFill != null)
                firstLaunchTutorialHoldFill.gameObject.SetActive(false);

            if (firstLaunchTutorialGameplayRoot != null)
            {
                Transform instructionShadow =
                    firstLaunchTutorialGameplayRoot.transform.Find(
                        "Tutorial Instruction Shadow"
                    );
                if (instructionShadow != null)
                    instructionShadow.gameObject.SetActive(false);
            }

            ClearFirstLaunchTutorialPhysicalHighlight();
        }

        private bool UpdateFirstLaunchTutorialLessonScreenFlow()
        {
            if (firstLaunchTutorialContinuousHandoffActive)
            {
                UpdateFirstLaunchTutorialContinuousRoomHandoff();
                return true;
            }

            if (!firstLaunchTutorialLessonCompleteAwaitingTravel)
                return false;

            if (firstLaunchTutorialPlayerWorldPosition.x <
                    firstLaunchTutorialLessonScreenExitX)
            {
                return false;
            }

            BeginFirstLaunchTutorialContinuousRoomHandoff();
            return true;
        }

        private void BeginFirstLaunchTutorialContinuousRoomHandoff()
        {
            Vector2 startPlayer = firstLaunchTutorialPlayerWorldPosition;
            Vector2 startHorse = firstLaunchTutorialHorseWorldPosition;
            bool sourceHorseVisible = firstLaunchTutorialHorse != null &&
                firstLaunchTutorialHorse.gameObject.activeSelf;
            float startCamera = firstLaunchTutorialCameraWorldX;

            firstLaunchTutorialLessonCompleteAwaitingTravel = false;
            firstLaunchTutorialContinuousHandoffActive = true;
            firstLaunchTutorialHandoffStartedAt = Time.unscaledTime;
            firstLaunchTutorialHandoffStartCameraX = startCamera;
            firstLaunchTutorialHandoffStartPlayerPosition = startPlayer;
            firstLaunchTutorialHandoffStartHorsePosition = startHorse;
            firstLaunchTutorialHandoffDefersHorseUntilSettled =
                firstLaunchTutorialPendingScreenStep ==
                    FirstLaunchTutorialStep.HorseReturn;
            HideFirstLaunchTutorialContinueEffect();

            firstLaunchTutorialApplyingScreenStep = true;
            ApplyFirstLaunchTutorialStepImmediately(
                firstLaunchTutorialPendingScreenStep,
                preserveCurrentScreen: false
            );
            firstLaunchTutorialApplyingScreenStep = false;

            firstLaunchTutorialHandoffTargetCameraX =
                firstLaunchTutorialLessonScreenCenterX;
            firstLaunchTutorialHandoffTargetPlayerPosition =
                firstLaunchTutorialPlayerWorldPosition;
            firstLaunchTutorialHandoffTargetHorsePosition =
                firstLaunchTutorialHorseWorldPosition;
            firstLaunchTutorialHandoffTargetMounted = firstLaunchTutorialMounted;
            bool targetHorseVisible = firstLaunchTutorialHorse != null &&
                firstLaunchTutorialHorse.gameObject.activeSelf;
            if (firstLaunchTutorialHandoffDefersHorseUntilSettled &&
                firstLaunchTutorialHorse != null)
            {
                firstLaunchTutorialHorse.gameObject.SetActive(false);
                targetHorseVisible = false;
            }
            firstLaunchTutorialHandoffHorseStagedInTargetRoom =
                targetHorseVisible && !sourceHorseVisible;

            // BD PRELOADED ROOM VISUALS V10.11.30.28
            // The complete target room is positioned and activated before the
            // camera starts moving. Static props, enemies, hazards and the horse
            // therefore enter through the scroll itself and never pop in later.
            PrepareFirstLaunchTutorialRoomVisualsForHandoff(
                firstLaunchTutorialPendingScreenStep
            );

            // Restore the exact edge-contact frame. The room change is then
            // presented as one continuous camera/player travel, never as a
            // respawn, teleport, fade, shutter or opaque cover.
            firstLaunchTutorialPlayerWorldPosition = startPlayer;
            firstLaunchTutorialHorseWorldPosition =
                firstLaunchTutorialHandoffHorseStagedInTargetRoom
                    ? firstLaunchTutorialHandoffTargetHorsePosition
                    : startHorse;
            firstLaunchTutorialCameraWorldX = startCamera;
            if (firstLaunchTutorialHandoffTargetMounted)
                firstLaunchTutorialHorseWorldPosition = startPlayer;

            SetFirstLaunchTutorialLessonInstructionVisible(false);
            DisableFirstLaunchTutorialRoomTransitionOverlay();
        }

        private void UpdateFirstLaunchTutorialContinuousRoomHandoff()
        {
            float elapsed = Time.unscaledTime -
                firstLaunchTutorialHandoffStartedAt;
            float progress = Mathf.Clamp01(
                elapsed / TutorialLessonScreenHandoffSeconds
            );
            float eased = progress * progress * (3f - 2f * progress);

            firstLaunchTutorialCameraWorldX = Mathf.Lerp(
                firstLaunchTutorialHandoffStartCameraX,
                firstLaunchTutorialHandoffTargetCameraX,
                eased
            );
            firstLaunchTutorialPlayerWorldPosition = Vector2.Lerp(
                firstLaunchTutorialHandoffStartPlayerPosition,
                firstLaunchTutorialHandoffTargetPlayerPosition,
                eased
            );
            if (firstLaunchTutorialHandoffTargetMounted)
            {
                firstLaunchTutorialHorseWorldPosition =
                    firstLaunchTutorialPlayerWorldPosition;
            }
            else if (firstLaunchTutorialHandoffHorseStagedInTargetRoom)
            {
                // The horse belongs to the destination room and is already at
                // its authored start position before the camera reaches it.
                firstLaunchTutorialHorseWorldPosition =
                    firstLaunchTutorialHandoffTargetHorsePosition;
            }
            else
            {
                firstLaunchTutorialHorseWorldPosition = Vector2.Lerp(
                    firstLaunchTutorialHandoffStartHorsePosition,
                    firstLaunchTutorialHandoffTargetHorsePosition,
                    eased
                );
            }

            firstLaunchTutorialLastMoveDirection = Vector2.right;
            firstLaunchTutorialMovementActive = true;
            DisableFirstLaunchTutorialRoomTransitionOverlay();

            if (progress < 1f)
                return;

            firstLaunchTutorialContinuousHandoffActive = false;
            firstLaunchTutorialCameraWorldX =
                firstLaunchTutorialHandoffTargetCameraX;
            firstLaunchTutorialPlayerWorldPosition =
                firstLaunchTutorialHandoffTargetPlayerPosition;
            firstLaunchTutorialHorseWorldPosition =
                firstLaunchTutorialHandoffTargetHorsePosition;
            if (firstLaunchTutorialHandoffTargetMounted)
            {
                firstLaunchTutorialHorseWorldPosition =
                    firstLaunchTutorialPlayerWorldPosition;
            }
            firstLaunchTutorialCheckpointX =
                firstLaunchTutorialPlayerWorldPosition.x;
            firstLaunchTutorialMovementActive = false;
            firstLaunchTutorialHandoffHorseStagedInTargetRoom = false;
            if (firstLaunchTutorialHandoffDefersHorseUntilSettled &&
                firstLaunchTutorialHorse != null)
            {
                firstLaunchTutorialHorse.gameObject.SetActive(false);
            }
            firstLaunchTutorialHandoffDefersHorseUntilSettled = false;

            // The next lesson begins only after its room fully owns the frame.
            // Timed events, enemy reactions, reloads and input locks must not
            // consume their opening time while the camera is still crossing.
            firstLaunchTutorialStepStartedAt = Time.unscaledTime;
            firstLaunchTutorialInputUnlockAt = Time.unscaledTime + 0.10f;
            if (firstLaunchTutorialStep == FirstLaunchTutorialStep.Reload)
            {
                firstLaunchTutorialReloadCompletesAt =
                    Time.unscaledTime + 1.15f;
            }
            for (int index = 0; index < firstLaunchTutorialActors.Count; index++)
            {
                TutorialEnemyActor actor = firstLaunchTutorialActors[index];
                if (actor.Active && !actor.Dead &&
                    !float.IsPositiveInfinity(actor.NextActionAt))
                {
                    actor.NextActionAt = Time.unscaledTime + 0.80f;
                }
            }
            bool revealInstructionImmediatelyV1011335 =
                firstLaunchTutorialStep !=
                    FirstLaunchTutorialStep.DismountHorse;
            firstLaunchTutorialDismountInstructionRevealedV1011335 =
                revealInstructionImmediatelyV1011335;
            if (revealInstructionImmediatelyV1011335)
            {
                SetFirstLaunchTutorialLessonInstructionVisible(true);
            }
            else
            {
                HideFirstLaunchTutorialDismountInstructionUntilMarkerV1011335();
            }
        }

        private void PrepareFirstLaunchTutorialRoomVisualsForHandoff(
            FirstLaunchTutorialStep targetStep)
        {
            ApplyFirstLaunchTutorialHorseVisibilityForScreen(targetStep);
            // BD POST-SCROLL HORSE RETURN V10.11.30.29
            // Static room assets still preload and enter with the scroll. The
            // injured horse is the authored exception: it stays inactive until
            // the target room fully settles, then starts its return animation.
            if (targetStep == FirstLaunchTutorialStep.HorseReturn &&
                firstLaunchTutorialHorse != null)
            {
                firstLaunchTutorialHorse.gameObject.SetActive(false);
            }
            EnsureFirstLaunchTutorialPersistentCourseGeometryVisible();
            ResetFirstLaunchTutorialEnemyVisualPoses();
            Canvas.ForceUpdateCanvases();
            RenderFirstLaunchTutorialFreePlayCourse(force: true);
        }

        private void DisableFirstLaunchTutorialRoomTransitionOverlay()
        {
            if (firstLaunchTutorialWhiteOverlay == null)
                return;

            Color color = firstLaunchTutorialWhiteOverlay.color;
            color.a = 0f;
            firstLaunchTutorialWhiteOverlay.color = color;
            firstLaunchTutorialWhiteOverlay.gameObject.SetActive(false);
        }

        private void SuspendFirstLaunchTutorialCompletedLessonWorld()
        {
            firstLaunchTutorialParryProjectileActive = false;
            firstLaunchTutorialHazardKnockbackActive = false;
            firstLaunchTutorialHazardKnockbackImpactApplied = false;
            firstLaunchTutorialHazardKnockbackActor = null;
            CancelFirstLaunchTutorialEnemyProjectile();

            if (firstLaunchTutorialProjectile != null)
                firstLaunchTutorialProjectile.gameObject.SetActive(false);
            if (firstLaunchTutorialHazard != null)
                firstLaunchTutorialHazard.gameObject.SetActive(false);
            if (firstLaunchTutorialGap != null)
                firstLaunchTutorialGap.gameObject.SetActive(false);
            if (firstLaunchTutorialSecret != null)
                firstLaunchTutorialSecret.gameObject.SetActive(false);

            for (int index = 0; index < firstLaunchTutorialActors.Count; index++)
            {
                TutorialEnemyActor actor = firstLaunchTutorialActors[index];
                actor.AttackCommitted = false;
                actor.DamageApplied = false;
                actor.NextActionAt = float.PositiveInfinity;
                actor.Active = false;
                actor.Dead = true;
                if (actor.Image != null)
                    actor.Image.gameObject.SetActive(false);
            }
        }

        private void ResetFirstLaunchTutorialTransientWorldForNewScreen()
        {
            SuspendFirstLaunchTutorialCompletedLessonWorld();
            firstLaunchTutorialPlayerDeathActive = false;
            firstLaunchTutorialRespawnResetApplied = false;
            firstLaunchTutorialInvulnerableUntil = 0f;
            firstLaunchTutorialPrimaryHoldStartedAt = -1f;
            firstLaunchTutorialHealHoldStartedAt = -1f;
            firstLaunchTutorialGrappleHoldStartedAt = -1f;
            firstLaunchTutorialChargedShotPendingStartedAt = -1f;
            firstLaunchTutorialChargedShotStartedAt = -1f;

            if (firstLaunchTutorialSecret != null)
                firstLaunchTutorialSecret.gameObject.SetActive(false);
            if (firstLaunchTutorialRespawnOverlay != null)
                firstLaunchTutorialRespawnOverlay.gameObject.SetActive(false);
            if (firstLaunchTutorialRespawnLabel != null)
                firstLaunchTutorialRespawnLabel.gameObject.SetActive(false);
        }

        private void BuildFirstLaunchTutorialContinueEffect(
            RectTransform parent)
        {
            if (parent == null)
                return;

            Image shadow = CreatePanel(
                parent,
                "Tutorial Continue Effect",
                TutorialContinueEffectRestPosition.x,
                TutorialContinueEffectRestPosition.y - 4f,
                292f,
                68f,
                new Color(0f, 0f, 0f, 0.58f)
            );
            shadow.raycastTarget = false;
            firstLaunchTutorialContinueEffectRoot = shadow.rectTransform;
            firstLaunchTutorialContinueEffectCanvasGroup =
                shadow.gameObject.AddComponent<CanvasGroup>();

            Image surface = CreatePanel(
                firstLaunchTutorialContinueEffectRoot,
                "Tutorial Continue Surface",
                0f,
                4f,
                282f,
                58f,
                new Color(0.025f, 0.055f, 0.10f, 0.985f)
            );
            surface.raycastTarget = false;
            AddOutline(
                surface.gameObject,
                new Color(0.26f, 0.82f, 1f, 0.98f),
                3f
            );
            Image accent = CreatePanel(
                surface.rectTransform,
                "Tutorial Continue Accent",
                0f,
                25f,
                282f,
                6f,
                new Color(1f, 0.72f, 0.22f, 1f)
            );
            accent.raycastTarget = false;
            Image leftPixel = CreatePanel(
                surface.rectTransform,
                "Tutorial Continue Left Pixel",
                -128f,
                0f,
                8f,
                30f,
                new Color(0.22f, 0.92f, 1f, 1f)
            );
            leftPixel.raycastTarget = false;

            firstLaunchTutorialContinueEffectLabel = CreateText(
                surface.rectTransform,
                "Tutorial Continue Label",
                "CONTINUE",
                -12f,
                -1f,
                202f,
                36f,
                23,
                TextAnchor.MiddleCenter,
                Color.white,
                FontStyle.Bold
            );
            firstLaunchTutorialContinueEffectLabel.raycastTarget = false;
            firstLaunchTutorialContinueEffectArrow = CreateText(
                surface.rectTransform,
                "Tutorial Continue Arrow",
                ">>",
                108f,
                -1f,
                54f,
                36f,
                23,
                TextAnchor.MiddleCenter,
                new Color(0.42f, 1f, 0.72f, 1f),
                FontStyle.Bold
            );
            firstLaunchTutorialContinueEffectArrow.raycastTarget = false;
            HideFirstLaunchTutorialContinueEffect();
        }

        private void ShowFirstLaunchTutorialContinueEffect()
        {
            if (firstLaunchTutorialContinueEffectRoot == null)
                return;

            firstLaunchTutorialContinueEffectRoot.gameObject.SetActive(true);
            firstLaunchTutorialContinueEffectRoot.SetAsLastSibling();
            firstLaunchTutorialContinueEffectRoot.anchoredPosition =
                TutorialContinueEffectRestPosition + new Vector2(0f, -8f);
            firstLaunchTutorialContinueEffectRoot.localScale =
                new Vector3(0.92f, 0.92f, 1f);
            if (firstLaunchTutorialContinueEffectCanvasGroup != null)
                firstLaunchTutorialContinueEffectCanvasGroup.alpha = 0f;
            firstLaunchTutorialContinueEffectStartedAt = Time.unscaledTime;
        }

        private void HideFirstLaunchTutorialContinueEffect()
        {
            if (firstLaunchTutorialContinueEffectRoot != null)
                firstLaunchTutorialContinueEffectRoot.gameObject.SetActive(false);
            if (firstLaunchTutorialContinueEffectCanvasGroup != null)
                firstLaunchTutorialContinueEffectCanvasGroup.alpha = 0f;
        }

        private void UpdateFirstLaunchTutorialContinueEffect()
        {
            if (firstLaunchTutorialContinueEffectRoot == null ||
                !firstLaunchTutorialContinueEffectRoot.gameObject.activeSelf)
            {
                return;
            }

            float elapsed = Mathf.Max(
                0f,
                Time.unscaledTime - firstLaunchTutorialContinueEffectStartedAt
            );
            int entranceFrame = Mathf.Clamp(
                Mathf.FloorToInt(elapsed / 0.045f),
                0,
                4
            );
            float entrance = entranceFrame / 4f;
            if (firstLaunchTutorialContinueEffectCanvasGroup != null)
                firstLaunchTutorialContinueEffectCanvasGroup.alpha = entrance;

            float scale = Mathf.Lerp(0.92f, 1f, entrance);
            firstLaunchTutorialContinueEffectRoot.localScale =
                new Vector3(scale, scale, 1f);

            int pulseFrame = elapsed < 0.18f
                ? 0
                : Mathf.FloorToInt((elapsed - 0.18f) / 0.10f) % 6;
            float verticalPixel = pulseFrame == 1 || pulseFrame == 4
                ? 2f
                : 0f;
            firstLaunchTutorialContinueEffectRoot.anchoredPosition =
                TutorialContinueEffectRestPosition +
                new Vector2(0f, Mathf.Lerp(-8f, 0f, entrance) + verticalPixel);

            if (firstLaunchTutorialContinueEffectArrow != null)
            {
                float horizontalPixel = pulseFrame <= 2
                    ? pulseFrame * 3f
                    : (5 - pulseFrame) * 3f;
                firstLaunchTutorialContinueEffectArrow.rectTransform
                    .anchoredPosition = new Vector2(
                        108f + horizontalPixel,
                        -1f
                    );
                firstLaunchTutorialContinueEffectArrow.color =
                    pulseFrame == 2 || pulseFrame == 3
                        ? new Color(1f, 0.78f, 0.28f, 1f)
                        : new Color(0.42f, 1f, 0.72f, 1f);
            }
        }

        private void DisposeFirstLaunchTutorialContinueEffect()
        {
            firstLaunchTutorialContinueEffectRoot = null;
            firstLaunchTutorialContinueEffectCanvasGroup = null;
            firstLaunchTutorialContinueEffectLabel = null;
            firstLaunchTutorialContinueEffectArrow = null;
            firstLaunchTutorialContinueEffectStartedAt = 0f;
        }

        private void ApplyFirstLaunchTutorialStepImmediately(
            FirstLaunchTutorialStep step,
            bool preserveCurrentScreen)
        {
            HideFirstLaunchTutorialContinueEffect();
            firstLaunchTutorialStep = step;
            ResetFirstLaunchTutorialGrappleJumpPetStateV1011338(step);
            ResetFirstLaunchTutorialBossIntroFreezeV1011342(step);
            if (step != FirstLaunchTutorialStep.Parry)
                firstLaunchTutorialParryResolvedV1011326 = false;
            firstLaunchTutorialStepStartedAt = Time.unscaledTime;
            firstLaunchTutorialInputUnlockAt = Time.unscaledTime + 0.10f;
            firstLaunchTutorialPrimaryHoldStartedAt = -1f;
            firstLaunchTutorialHealHoldStartedAt = -1f;
            firstLaunchTutorialGrappleHoldStartedAt = -1f;
            firstLaunchTutorialChargedShotPendingStartedAt = -1f;
            firstLaunchTutorialChargedShotStartedAt = -1f;
            ResetFirstLaunchTutorialHintEscalation();

            if (firstLaunchTutorialFeedback != null &&
                float.IsPositiveInfinity(firstLaunchTutorialFeedbackClearAt))
            {
                firstLaunchTutorialFeedback.text = string.Empty;
                firstLaunchTutorialFeedbackClearAt = 0f;
            }

            if (!preserveCurrentScreen)
            {
                ResetFirstLaunchTutorialTransientWorldForNewScreen();
                firstLaunchTutorialLessonScreenCenterX =
                    ResolveFirstLaunchTutorialLessonScreenCenter(step);
            }

            ConfigureFirstLaunchTutorialScene(step);
            if (!preserveCurrentScreen)
                ApplyFirstLaunchTutorialLessonScreenLayout(step);
            else
                ResetFirstLaunchTutorialEnemyVisualPoses();

            firstLaunchTutorialLessonScreenInitialized = true;
            firstLaunchTutorialDismountInstructionRevealedV1011335 =
                step != FirstLaunchTutorialStep.DismountHorse;
            if (firstLaunchTutorialDismountInstructionRevealedV1011335)
            {
                SetFirstLaunchTutorialLessonInstructionVisible(true);
            }
            else
            {
                HideFirstLaunchTutorialDismountInstructionUntilMarkerV1011335();
            }
        }

        private void ApplyFirstLaunchTutorialLessonScreenLayout(
            FirstLaunchTutorialStep step)
        {
            float center = firstLaunchTutorialLessonScreenCenterX;
            firstLaunchTutorialLessonScreenExitX = Mathf.Min(
                TutorialWorldMaxX - 28f,
                center + TutorialLessonScreenExitOffset
            );
            firstLaunchTutorialCameraWorldX = center;
            firstLaunchTutorialLastMoveDirection = Vector2.right;
            firstLaunchTutorialMovementActive = false;

            float playerX = Mathf.Max(TutorialWorldMinX + 20f, center - 270f);
            firstLaunchTutorialPlayerWorldPosition =
                new Vector2(playerX, TutorialGroundY);
            firstLaunchTutorialCheckpointX = playerX;
            firstLaunchTutorialGrounded = true;
            firstLaunchTutorialGroundedY = TutorialGroundY;
            firstLaunchTutorialVerticalVelocity = 0f;

            switch (step)
            {
                case FirstLaunchTutorialStep.Move:
                    firstLaunchTutorialTravelDistance = 0f;
                    firstLaunchTutorialMoveLessonStartX =
                        firstLaunchTutorialPlayerWorldPosition.x;
                    break;

                case FirstLaunchTutorialStep.Jump:
                    if (firstLaunchTutorialJumpObstacle != null)
                    {
                        firstLaunchTutorialJumpObstacle.rectTransform
                            .anchoredPosition = new Vector2(
                                TutorialJumpObstacleX,
                                TutorialGroundY - 4f
                            );
                    }
                    break;

                case FirstLaunchTutorialStep.MountHorse:
                case FirstLaunchTutorialStep.RideHorse:
                    // BD DEFERRED HORSE ROOM LAYOUT V10.11.30.27
                    if (step == FirstLaunchTutorialStep.MountHorse)
                    {
                        firstLaunchTutorialMounted = false;
                        firstLaunchTutorialMountedCurrentSpeed = 0f;
                        firstLaunchTutorialPlayerWorldPosition =
                            new Vector2(center - 250f, TutorialGroundY);
                        firstLaunchTutorialCheckpointX = center - 250f;
                        firstLaunchTutorialHorseWorldPosition =
                            new Vector2(center, TutorialGroundY - 8f);
                        firstLaunchTutorialHorseInjured = false;
                    }
                    break;

                case FirstLaunchTutorialStep.EnemyArrival:
                case FirstLaunchTutorialStep.HorseShot:
                    // BD DEFERRED HORSE LESSON BEFORE HORSE-SHOT STORY V10.11.30.27
                    firstLaunchTutorialMounted = true;
                    firstLaunchTutorialMountedCurrentSpeed = 0f;
                    firstLaunchTutorialHorseInjured = false;
                    firstLaunchTutorialHorseWorldPosition =
                        new Vector2(center - 250f, TutorialGroundY - 8f);
                    firstLaunchTutorialPlayerWorldPosition =
                        firstLaunchTutorialHorseWorldPosition;
                    firstLaunchTutorialCheckpointX = center - 250f;
                    PositionFirstLaunchTutorialActorForScreen(
                        firstLaunchTutorialEnemy,
                        TutorialEnemyRole.Ranged,
                        center + 96f,
                        1f,
                        true
                    );
                    // The scripted story beat owns the one visible horse shot;
                    // autonomous combat projectiles are forbidden in this room.
                    SetFirstLaunchTutorialActorPassiveForScreen(
                        firstLaunchTutorialEnemy
                    );
                    CancelFirstLaunchTutorialEnemyProjectile();
                    PrepareFirstLaunchTutorialPrimaryEnemyVisualV1011326(
                        new Color(0.84f, 0.20f, 0.30f, 1f)
                    );
                    break;

                case FirstLaunchTutorialStep.AttackEnemy:
                    // BD EXACT-CENTER VISIBLE ONE-HIT TARGET V10.11.30.26
                    firstLaunchTutorialMounted = false;
                    firstLaunchTutorialMountedCurrentSpeed = 0f;
                    firstLaunchTutorialPlayerWorldPosition =
                        new Vector2(center - 270f, TutorialGroundY);
                    firstLaunchTutorialCheckpointX = center - 270f;
                    if (firstLaunchTutorialHorse != null)
                        firstLaunchTutorialHorse.gameObject.SetActive(false);
                    PositionFirstLaunchTutorialActorForScreen(
                        firstLaunchTutorialEnemy,
                        TutorialEnemyRole.Small,
                        center,
                        1f,
                        true
                    );
                    SetFirstLaunchTutorialActorPassiveForScreen(
                        firstLaunchTutorialEnemy
                    );
                    PrepareFirstLaunchTutorialPrimaryEnemyVisualV1011326(
                        new Color(0.92f, 0.16f, 0.24f, 1f)
                    );
                    break;

                case FirstLaunchTutorialStep.HeavyAttack:
                    PositionFirstLaunchTutorialActorForScreen(
                        firstLaunchTutorialEnemy,
                        TutorialEnemyRole.Sword,
                        center,
                        2f,
                        true
                    );
                    break;

                case FirstLaunchTutorialStep.Dodge:
                    firstLaunchTutorialHazardWorldPosition =
                        new Vector2(center, TutorialGroundY - 34f);
                    firstLaunchTutorialPlayerWorldPosition =
                        new Vector2(center - 118f, TutorialGroundY);
                    firstLaunchTutorialCheckpointX = center - 118f;
                    firstLaunchTutorialDodgeLessonStartSide = -1f;
                    break;

                case FirstLaunchTutorialStep.Parry:
                    // BD SINGLE-OWNER PARRY TRANSACTION V10.11.30.26
                    firstLaunchTutorialParryResolvedV1011326 = false;
                    PositionFirstLaunchTutorialActorForScreen(
                        firstLaunchTutorialEnemy,
                        TutorialEnemyRole.Ranged,
                        center,
                        2f,
                        true
                    );
                    SetFirstLaunchTutorialActorPassiveForScreen(
                        firstLaunchTutorialEnemy
                    );
                    PrepareFirstLaunchTutorialPrimaryEnemyVisualV1011326(
                        new Color(0.62f, 0.34f, 0.88f, 1f)
                    );
                    firstLaunchTutorialPlayerWorldPosition =
                        new Vector2(center - 250f, TutorialGroundY);
                    firstLaunchTutorialCheckpointX = center - 250f;
                    CancelFirstLaunchTutorialEnemyProjectile();
                    firstLaunchTutorialParryProjectileProgress = 0f;
                    firstLaunchTutorialParryProjectileActive = true;
                    firstLaunchTutorialProjectileWorldPosition =
                        new Vector2(center - 20f, TutorialGroundY + 34f);
                    break;

                case FirstLaunchTutorialStep.JumpAttack:
                    PositionFirstLaunchTutorialActorForScreen(
                        firstLaunchTutorialEnemy,
                        TutorialEnemyRole.Small,
                        center + 76f,
                        1f,
                        true
                    );
                    SetFirstLaunchTutorialActorPassiveForScreen(
                        firstLaunchTutorialEnemy
                    );
                    break;

                case FirstLaunchTutorialStep.HorseReturn:
                    // BD POST-SCROLL HORSE RETURN V10.11.30.29
                    // Record the authored run-in origin, but do not expose the
                    // horse while this room is scrolling into place.
                    firstLaunchTutorialMounted = false;
                    firstLaunchTutorialHorseInjured = true;
                    firstLaunchTutorialHorseWorldPosition =
                        firstLaunchTutorialPlayerWorldPosition +
                        new Vector2(-320f, -8f);
                    if (firstLaunchTutorialHorse != null)
                        firstLaunchTutorialHorse.gameObject.SetActive(false);
                    break;

                case FirstLaunchTutorialStep.HealHorse:
                    firstLaunchTutorialMounted = false;
                    firstLaunchTutorialHorseWorldPosition =
                        new Vector2(center, TutorialGroundY - 8f);
                    firstLaunchTutorialPlayerWorldPosition =
                        new Vector2(center - 104f, TutorialGroundY);
                    firstLaunchTutorialCheckpointX = center - 104f;
                    firstLaunchTutorialHorseInjured = true;
                    break;

                case FirstLaunchTutorialStep.PetHorse:
                case FirstLaunchTutorialStep.RemountHorse:
                    firstLaunchTutorialMounted = false;
                    firstLaunchTutorialHorseWorldPosition =
                        new Vector2(center, TutorialGroundY - 8f);
                    firstLaunchTutorialPlayerWorldPosition =
                        new Vector2(center - 104f, TutorialGroundY);
                    firstLaunchTutorialCheckpointX = center - 104f;
                    firstLaunchTutorialHorseInjured = false;
                    break;

                case FirstLaunchTutorialStep.RangedAttack:
                case FirstLaunchTutorialStep.Reload:
                case FirstLaunchTutorialStep.ChargedShot:
                case FirstLaunchTutorialStep.MountedImpact:
                    firstLaunchTutorialMounted = true;
                    firstLaunchTutorialHorseWorldPosition =
                        new Vector2(center - 260f, TutorialGroundY - 8f);
                    firstLaunchTutorialPlayerWorldPosition =
                        firstLaunchTutorialHorseWorldPosition;
                    firstLaunchTutorialCheckpointX = center - 260f;
                    PositionFirstLaunchTutorialActorForScreen(
                        firstLaunchTutorialEnemy,
                        TutorialEnemyRole.Small,
                        center,
                        step == FirstLaunchTutorialStep.ChargedShot ? 2f : 1f,
                        step != FirstLaunchTutorialStep.Reload
                    );
                    if (step == FirstLaunchTutorialStep.Reload)
                    {
                        firstLaunchTutorialAmmo = 0;
                        firstLaunchTutorialReloadCompletesAt =
                            Time.unscaledTime + 1.15f;
                    }
                    break;

                case FirstLaunchTutorialStep.DismountHorse:
                    firstLaunchTutorialMounted = true;
                    firstLaunchTutorialHorseWorldPosition =
                        new Vector2(center - 230f, TutorialGroundY - 8f);
                    firstLaunchTutorialPlayerWorldPosition =
                        firstLaunchTutorialHorseWorldPosition;
                    firstLaunchTutorialCheckpointX = center - 230f;
                    break;

                case FirstLaunchTutorialStep.SpinAttack:
                    firstLaunchTutorialPlayerWorldPosition =
                        new Vector2(center, TutorialGroundY);
                    firstLaunchTutorialCheckpointX = center;
                    PositionFirstLaunchTutorialActorForScreen(
                        firstLaunchTutorialEnemy,
                        TutorialEnemyRole.Small,
                        center - TutorialSpinEnemyOffsetV1011335,
                        1f,
                        true
                    );
                    PositionFirstLaunchTutorialActorForScreen(
                        firstLaunchTutorialEnemySecondary,
                        TutorialEnemyRole.Small,
                        center + TutorialSpinEnemyOffsetV1011335,
                        1f,
                        true
                    );
                    break;

                case FirstLaunchTutorialStep.Grapple:
                    firstLaunchTutorialGapWorldPosition =
                        new Vector2(center + 92f, TutorialGroundY - 52f);
                    if (firstLaunchTutorialGap != null)
                        firstLaunchTutorialGap.gameObject.SetActive(true);
                    PositionFirstLaunchTutorialActorForScreen(
                        firstLaunchTutorialEnemy,
                        TutorialEnemyRole.Sword,
                        center + 168f,
                        1f,
                        true
                    );
                    break;

                case FirstLaunchTutorialStep.HazardKnockback:
                    firstLaunchTutorialHazardWorldPosition =
                        new Vector2(center + 82f, TutorialGroundY - 34f);
                    PositionFirstLaunchTutorialActorForScreen(
                        firstLaunchTutorialEnemy,
                        TutorialEnemyRole.Sword,
                        center - 48f,
                        1f,
                        true
                    );
                    break;

                case FirstLaunchTutorialStep.SidePath:
                    if (firstLaunchTutorialSecret != null)
                    {
                        firstLaunchTutorialSecret.rectTransform
                            .anchoredPosition = new Vector2(
                                center + 150f,
                                TutorialGroundY + 82f
                            );
                        firstLaunchTutorialSecret.gameObject.SetActive(true);
                    }
                    break;

                case FirstLaunchTutorialStep.WallJump:
                    firstLaunchTutorialPlayerWorldPosition =
                        new Vector2(center - 280f, TutorialGroundY);
                    firstLaunchTutorialCheckpointX = center - 280f;
                    PositionFirstLaunchTutorialWallJumpGeometry(center);
                    break;

                case FirstLaunchTutorialStep.CombinedEncounter:
                    PositionFirstLaunchTutorialActorForScreen(
                        firstLaunchTutorialEnemy,
                        TutorialEnemyRole.Sword,
                        center + 20f,
                        2f,
                        true
                    );
                    PositionFirstLaunchTutorialActorForScreen(
                        firstLaunchTutorialEnemySecondary,
                        TutorialEnemyRole.Small,
                        center + 140f,
                        1f,
                        true
                    );
                    PositionFirstLaunchTutorialActorForScreen(
                        firstLaunchTutorialRangedEnemy,
                        TutorialEnemyRole.Ranged,
                        center + 260f,
                        2f,
                        true
                    );
                    break;

                case FirstLaunchTutorialStep.MiniBossIntro:
                case FirstLaunchTutorialStep.MiniBossPhaseOne:
                case FirstLaunchTutorialStep.MiniBossPhaseTwo:
                    PositionFirstLaunchTutorialActorForScreen(
                        firstLaunchTutorialMiniBoss,
                        TutorialEnemyRole.MiniBoss,
                        center + 80f,
                        12f,
                        true
                    );
                    if (firstLaunchTutorialFinishGate != null)
                    {
                        firstLaunchTutorialFinishGate.rectTransform
                            .anchoredPosition = new Vector2(
                                center + 320f,
                                TutorialGroundY + 24f
                            );
                        firstLaunchTutorialFinishGate.gameObject.SetActive(false);
                    }
                    break;

                case FirstLaunchTutorialStep.Collectible:
                    firstLaunchTutorialCollectibleWorldPosition =
                        new Vector2(center, TutorialGroundY + 18f);
                    if (firstLaunchTutorialFinishGate != null)
                    {
                        firstLaunchTutorialFinishGate.rectTransform
                            .anchoredPosition = new Vector2(
                                center - 320f,
                                TutorialGroundY + 24f
                            );
                        firstLaunchTutorialFinishGate.gameObject.SetActive(false);
                    }
                    break;
            }

            ApplyFirstLaunchTutorialHorseVisibilityForScreen(step);
            ResetFirstLaunchTutorialEnemyVisualPoses();
            RenderFirstLaunchTutorialFreePlayCourse(force: true);
        }

        private void ApplyFirstLaunchTutorialHorseVisibilityForScreen(
            FirstLaunchTutorialStep step)
        {
            // BD HORSE-FREE OPENING VISIBILITY V10.11.30.27
            bool visible =
                step == FirstLaunchTutorialStep.MountHorse ||
                step == FirstLaunchTutorialStep.RideHorse ||
                step == FirstLaunchTutorialStep.EnemyArrival ||
                step == FirstLaunchTutorialStep.HorseShot ||
                step == FirstLaunchTutorialStep.HorseReturn ||
                step == FirstLaunchTutorialStep.HealHorse ||
                step == FirstLaunchTutorialStep.PetHorse ||
                step == FirstLaunchTutorialStep.RemountHorse ||
                step == FirstLaunchTutorialStep.RangedAttack ||
                step == FirstLaunchTutorialStep.Reload ||
                step == FirstLaunchTutorialStep.ChargedShot ||
                step == FirstLaunchTutorialStep.MountedImpact ||
                step == FirstLaunchTutorialStep.DismountHorse;

            if (firstLaunchTutorialHorse != null)
                firstLaunchTutorialHorse.gameObject.SetActive(visible);
        }

        private float ResolveFirstLaunchTutorialLessonScreenCenter(
            FirstLaunchTutorialStep step)
        {
            int roomIndex;
            switch (step)
            {
                case FirstLaunchTutorialStep.WhiteBoot:
                case FirstLaunchTutorialStep.Move:
                case FirstLaunchTutorialStep.Jump:
                    roomIndex = 0;
                    break;
                case FirstLaunchTutorialStep.AttackEnemy:
                    roomIndex = 1;
                    break;
                case FirstLaunchTutorialStep.HeavyAttack:
                    roomIndex = 2;
                    break;
                case FirstLaunchTutorialStep.Dodge:
                    roomIndex = 3;
                    break;
                case FirstLaunchTutorialStep.Parry:
                    roomIndex = 4;
                    break;
                case FirstLaunchTutorialStep.MountHorse:
                case FirstLaunchTutorialStep.RideHorse:
                    roomIndex = 5;
                    break;
                case FirstLaunchTutorialStep.EnemyArrival:
                case FirstLaunchTutorialStep.HorseShot:
                    roomIndex = 6;
                    break;
                case FirstLaunchTutorialStep.JumpAttack:
                    roomIndex = 7;
                    break;
                case FirstLaunchTutorialStep.HorseReturn:
                case FirstLaunchTutorialStep.HealHorse:
                case FirstLaunchTutorialStep.PetHorse:
                case FirstLaunchTutorialStep.RemountHorse:
                    roomIndex = 8;
                    break;
                case FirstLaunchTutorialStep.RangedAttack:
                case FirstLaunchTutorialStep.Reload:
                    roomIndex = 9;
                    break;
                case FirstLaunchTutorialStep.ChargedShot:
                    roomIndex = 10;
                    break;
                case FirstLaunchTutorialStep.MountedImpact:
                    roomIndex = 11;
                    break;
                case FirstLaunchTutorialStep.DismountHorse:
                    roomIndex = 12;
                    break;
                case FirstLaunchTutorialStep.SpinAttack:
                    roomIndex = 13;
                    break;
                case FirstLaunchTutorialStep.Grapple:
                    roomIndex = 14;
                    break;
                case FirstLaunchTutorialStep.HazardKnockback:
                    roomIndex = 15;
                    break;
                case FirstLaunchTutorialStep.SidePath:
                    roomIndex = 16;
                    break;
                case FirstLaunchTutorialStep.CombinedEncounter:
                    roomIndex = 17;
                    break;
                case FirstLaunchTutorialStep.WallJump:
                    roomIndex = 18;
                    break;
                case FirstLaunchTutorialStep.MiniBossIntro:
                case FirstLaunchTutorialStep.MiniBossPhaseOne:
                case FirstLaunchTutorialStep.MiniBossPhaseTwo:
                case FirstLaunchTutorialStep.MiniBossDefeated:
                    roomIndex = 19;
                    break;
                case FirstLaunchTutorialStep.Collectible:
                case FirstLaunchTutorialStep.Completed:
                    roomIndex = 20;
                    break;
                default:
                    roomIndex = 0;
                    break;
            }

            return TutorialOpeningScreenCenterX +
                roomIndex * TutorialLessonScreenSpacing;
        }

        private void PositionFirstLaunchTutorialWallJumpGeometry(float center)
        {
            float wallX = center + 42f;
            float platformMinX = center - 378f;
            float platformMaxX = center - 68f;
            float upperGroundMinX = center + 69f;
            float upperGroundMaxX = center + 282f;

            if (firstLaunchTutorialWallJumpWall != null)
            {
                firstLaunchTutorialWallJumpWall.rectTransform.anchoredPosition =
                    new Vector2(wallX, TutorialGroundY + 42f);
            }
            if (firstLaunchTutorialWallJumpPlatform != null)
            {
                firstLaunchTutorialWallJumpPlatform.rectTransform.anchoredPosition =
                    new Vector2(
                        (platformMinX + platformMaxX) * 0.5f,
                        TutorialWallJumpPlatformY - 22f
                    );
                firstLaunchTutorialWallJumpPlatform.rectTransform.sizeDelta =
                    new Vector2(
                        platformMaxX - platformMinX,
                        44f
                    );
            }
            if (firstLaunchTutorialWallJumpUpperGround != null)
            {
                firstLaunchTutorialWallJumpUpperGround.rectTransform.anchoredPosition =
                    new Vector2(
                        (upperGroundMinX + upperGroundMaxX) * 0.5f,
                        TutorialWallJumpUpperGroundY - 22f
                    );
                firstLaunchTutorialWallJumpUpperGround.rectTransform.sizeDelta =
                    new Vector2(
                        upperGroundMaxX - upperGroundMinX,
                        44f
                    );
            }
        }

        private float ResolveFirstLaunchTutorialWallJumpWallX()
        {
            return firstLaunchTutorialLessonScreenCenterX + 42f;
        }

        private float ResolveFirstLaunchTutorialWallJumpPlatformMinX()
        {
            return firstLaunchTutorialLessonScreenCenterX - 378f;
        }

        private float ResolveFirstLaunchTutorialWallJumpPlatformMaxX()
        {
            return firstLaunchTutorialLessonScreenCenterX - 68f;
        }

        private float ResolveFirstLaunchTutorialWallJumpUpperGroundMinX()
        {
            return firstLaunchTutorialLessonScreenCenterX + 69f;
        }

        private float ResolveFirstLaunchTutorialWallJumpUpperGroundMaxX()
        {
            return firstLaunchTutorialLessonScreenCenterX + 282f;
        }

        private void PositionFirstLaunchTutorialActorForScreen(
            Image image,
            TutorialEnemyRole role,
            float x,
            float health,
            bool active)
        {
            if (image == null)
                return;

            for (int index = 0;
                 index < firstLaunchTutorialActors.Count;
                 index++)
            {
                TutorialEnemyActor actor = firstLaunchTutorialActors[index];
                if (actor.Image != image)
                    continue;

                actor.Role = role;
                actor.Position = new Vector2(x, TutorialGroundY);
                actor.SpawnPosition = actor.Position;
                actor.MaximumHealth = Mathf.Max(1f, health);
                actor.Health = actor.MaximumHealth;
                actor.Active = active;
                actor.Dead = !active;
                actor.AttackCommitted = false;
                actor.DamageApplied = false;
                actor.UsesProjectileAttack =
                    role == TutorialEnemyRole.Ranged;
                actor.AttackSequence = 0;
                actor.NextActionAt = active
                    ? Time.unscaledTime + 0.80f
                    : float.PositiveInfinity;
                image.gameObject.SetActive(active);
                image.rectTransform.localScale = Vector3.one;
                image.rectTransform.localRotation = Quaternion.identity;
                if (image == firstLaunchTutorialEnemy)
                    firstLaunchTutorialEnemyWorldPosition = actor.Position;
                return;
            }
        }

        private void SetFirstLaunchTutorialActorPassiveForScreen(
            Image image)
        {
            for (int index = 0;
                 index < firstLaunchTutorialActors.Count;
                 index++)
            {
                TutorialEnemyActor actor = firstLaunchTutorialActors[index];
                if (actor.Image != image)
                    continue;

                actor.AttackCommitted = false;
                actor.DamageApplied = false;
                actor.NextActionAt = float.PositiveInfinity;
                return;
            }
        }

        private bool IsFirstLaunchTutorialPrimaryEnemyDead()
        {
            for (int index = 0;
                 index < firstLaunchTutorialActors.Count;
                 index++)
            {
                TutorialEnemyActor actor = firstLaunchTutorialActors[index];
                if (actor.Image == firstLaunchTutorialEnemy)
                    return actor.Dead || !actor.Active;
            }
            return firstLaunchTutorialEnemy == null ||
                !firstLaunchTutorialEnemy.gameObject.activeInHierarchy;
        }

        private bool DidFirstLaunchTutorialDodgeCrossObjective()
        {
            float currentSide = Mathf.Sign(
                firstLaunchTutorialPlayerWorldPosition.x -
                firstLaunchTutorialHazardWorldPosition.x
            );
            return firstLaunchTutorialDodgeLessonStartSide != 0f &&
                currentSide != 0f &&
                currentSide != firstLaunchTutorialDodgeLessonStartSide;
        }

        private float ResolveFirstLaunchTutorialScreenCameraTarget(
            float normalTarget)
        {
            if (firstLaunchTutorialContinuousHandoffActive)
                return firstLaunchTutorialCameraWorldX;
            if (firstLaunchTutorialLessonCompleteAwaitingTravel)
            {
                // Hold the completed room in frame. The player must visibly
                // reach the right screen edge before the next room starts.
                return firstLaunchTutorialLessonScreenCenterX;
            }
            if (firstLaunchTutorialLessonScreenInitialized)
                return firstLaunchTutorialLessonScreenCenterX;
            return normalTarget;
        }

        private int ResolveFirstLaunchTutorialUnlockRank(
            FirstLaunchTutorialStep step)
        {
            switch (step)
            {
                case FirstLaunchTutorialStep.WhiteBoot: return 0;
                case FirstLaunchTutorialStep.Move: return 1;
                case FirstLaunchTutorialStep.Jump: return 2;
                case FirstLaunchTutorialStep.AttackEnemy: return 3;
                case FirstLaunchTutorialStep.HeavyAttack: return 4;
                case FirstLaunchTutorialStep.Dodge: return 5;
                case FirstLaunchTutorialStep.Parry: return 6;
                case FirstLaunchTutorialStep.MountHorse:
                case FirstLaunchTutorialStep.RideHorse:
                case FirstLaunchTutorialStep.EnemyArrival:
                case FirstLaunchTutorialStep.HorseShot: return 7;
                case FirstLaunchTutorialStep.JumpAttack: return 8;
                case FirstLaunchTutorialStep.HorseReturn:
                case FirstLaunchTutorialStep.HealHorse: return 9;
                case FirstLaunchTutorialStep.RemountHorse: return 10;
                case FirstLaunchTutorialStep.RangedAttack:
                case FirstLaunchTutorialStep.Reload: return 11;
                case FirstLaunchTutorialStep.ChargedShot: return 12;
                case FirstLaunchTutorialStep.MountedImpact: return 13;
                case FirstLaunchTutorialStep.DismountHorse: return 14;
                case FirstLaunchTutorialStep.SpinAttack: return 15;
                case FirstLaunchTutorialStep.Grapple: return 16;
                default: return 17;
            }
        }

        private bool IsFirstLaunchTutorialMechanicUnlocked(int requiredRank)
        {
            int currentRank = ResolveFirstLaunchTutorialUnlockRank(
                firstLaunchTutorialStep
            );

            // Established unlock flags remain compatibility evidence while the
            // lesson-screen rank is the monotonic source of truth.
            switch (requiredRank)
            {
                case 2:
                    return firstLaunchTutorialJumpUnlocked ||
                        currentRank >= requiredRank;
                case 3:
                    return firstLaunchTutorialInteractUnlocked ||
                        currentRank >= requiredRank;
                case 4:
                    return firstLaunchTutorialLightAttackUnlocked ||
                        currentRank >= requiredRank;
                case 5:
                    return firstLaunchTutorialHeavyAttackUnlocked ||
                        currentRank >= requiredRank;
                case 6:
                    return firstLaunchTutorialDodgeUnlocked ||
                        currentRank >= requiredRank;
                case 7:
                    return firstLaunchTutorialParryUnlocked ||
                        currentRank >= requiredRank;
                case 11:
                    return firstLaunchTutorialRangedUnlocked ||
                        currentRank >= requiredRank;
                default:
                    return currentRank >= requiredRank;
            }
        }

        private bool TryResolveFirstLaunchTutorialUnifiedParry()
        {
            if (!IsFirstLaunchTutorialMechanicUnlocked(7) ||
                firstLaunchTutorialMounted ||
                firstLaunchTutorialParryResolvedV1011326)
            {
                return false;
            }

            float tutorialProjectileDistance = Vector2.Distance(
                firstLaunchTutorialPlayerWorldPosition,
                firstLaunchTutorialProjectileWorldPosition
            );
            bool focusedParry = firstLaunchTutorialStep ==
                FirstLaunchTutorialStep.Parry;
            if (firstLaunchTutorialParryProjectileActive &&
                tutorialProjectileDistance <= 224f)
            {
                firstLaunchTutorialParryResolvedV1011326 = focusedParry;
                CancelFirstLaunchTutorialParryProjectilesV1011326();
                PlayFirstLaunchTutorialParryAnimation(focusedParry);
                return true;
            }

            if (firstLaunchTutorialEnemyProjectileActive &&
                tutorialProjectileDistance <= 224f)
            {
                firstLaunchTutorialParryResolvedV1011326 = focusedParry;
                CancelFirstLaunchTutorialParryProjectilesV1011326();
                PlayFirstLaunchTutorialParryAnimation(focusedParry);
                SetFirstLaunchTutorialLearningState(
                    "Parry",
                    TutorialLearningState.Demonstrated
                );
                return true;
            }

            return TryResolveFirstLaunchTutorialContextParry();
        }

        private void CancelFirstLaunchTutorialParryProjectilesV1011326()
        {
            firstLaunchTutorialParryProjectileActive = false;
            CancelFirstLaunchTutorialEnemyProjectile();
            if (firstLaunchTutorialProjectile != null)
                firstLaunchTutorialProjectile.gameObject.SetActive(false);
        }

        private void PrepareFirstLaunchTutorialPrimaryEnemyVisualV1011326(
            Color tint)
        {
            if (firstLaunchTutorialEnemy == null)
                return;

            firstLaunchTutorialEnemy.gameObject.SetActive(true);
            firstLaunchTutorialEnemy.enabled = false;
            firstLaunchTutorialEnemy.color = tint;
            firstLaunchTutorialEnemy.rectTransform.sizeDelta =
                new Vector2(64f, 92f);
            firstLaunchTutorialEnemy.rectTransform.localScale = Vector3.one;
            firstLaunchTutorialEnemy.rectTransform.localRotation =
                Quaternion.identity;
            firstLaunchTutorialEnemy.rectTransform.SetAsLastSibling();

            if (firstLaunchTutorialEnemyPixelVisual == null)
            {
                Transform child = firstLaunchTutorialEnemy.rectTransform.Find(
                    "Tutorial Enemy Pixel Visual"
                );
                if (child != null)
                {
                    firstLaunchTutorialEnemyPixelVisual =
                        child as RectTransform;
                    firstLaunchTutorialEnemyPixelImage =
                        child.GetComponent<Image>();
                }
            }

            if (firstLaunchTutorialEnemyPixelVisual == null ||
                firstLaunchTutorialEnemyPixelImage == null)
            {
                // A missing generated child must never turn a required lesson
                // target into a tiny source rectangle or an invisible actor.
                ApplyFirstLaunchTutorialPixelSprite(
                    firstLaunchTutorialEnemy,
                    "Tutorial Enemy",
                    tint
                );
            }

            if (firstLaunchTutorialEnemyPixelVisual == null ||
                firstLaunchTutorialEnemyPixelImage == null)
            {
                firstLaunchTutorialEnemy.enabled = true;
                firstLaunchTutorialEnemy.color = tint;
                return;
            }

            firstLaunchTutorialEnemy.enabled = false;
            firstLaunchTutorialEnemyPixelVisual.gameObject.SetActive(true);
            firstLaunchTutorialEnemyPixelVisual.anchoredPosition = Vector2.zero;
            firstLaunchTutorialEnemyPixelVisual.sizeDelta =
                new Vector2(64f, 92f);
            firstLaunchTutorialEnemyPixelVisual.localScale = Vector3.one;
            firstLaunchTutorialEnemyPixelVisual.localRotation =
                Quaternion.identity;
            firstLaunchTutorialEnemyPixelVisual.SetAsLastSibling();
            firstLaunchTutorialEnemyPixelImage.enabled = true;
            firstLaunchTutorialEnemyPixelImage.preserveAspect = true;
            firstLaunchTutorialEnemyPixelImage.color = tint;
        }


        private void HandleFirstLaunchTutorialMeleeLessonDeathAtImpact(
            TutorialEnemyActor target)
        {
            if (target == null || target.Image != firstLaunchTutorialEnemy)
                return;

            // Preserve compatibility with established impact-proof state while
            // the authoritative actor-death result remains the current path.
            bool impactProof =
                target.Dead ||
                firstLaunchTutorialLessonHitConfirmedV101125 ||
                firstLaunchTutorialProductionLightResolved ||
                firstLaunchTutorialProductionHeavyResolved;
            if (!impactProof)
                return;

            switch (firstLaunchTutorialStep)
            {
                case FirstLaunchTutorialStep.AttackEnemy:
                    QueueFirstLaunchTutorialStepForNextScreen(
                        FirstLaunchTutorialStep.HeavyAttack
                    );
                    break;
                case FirstLaunchTutorialStep.HeavyAttack:
                    QueueFirstLaunchTutorialStepForNextScreen(
                        FirstLaunchTutorialStep.Dodge
                    );
                    break;
                case FirstLaunchTutorialStep.JumpAttack:
                    QueueFirstLaunchTutorialStepForNextScreen(
                        FirstLaunchTutorialStep.HorseReturn
                    );
                    break;
            }
        }

        private void ResetFirstLaunchTutorialEnemyVisualPoses()
        {
            if (firstLaunchTutorialStep ==
                    FirstLaunchTutorialStep.MiniBossDefeated)
            {
                return;
            }

            for (int index = 0;
                 index < firstLaunchTutorialActors.Count;
                 index++)
            {
                TutorialEnemyActor actor = firstLaunchTutorialActors[index];
                if (actor.Image == null)
                    continue;
                actor.Image.rectTransform.localScale = Vector3.one;
                actor.Image.rectTransform.localRotation = Quaternion.identity;
            }
        }
    }
}
