using UnityEngine;
using UnityEngine.UI;

namespace BoredomAndDungeons
{
    public sealed partial class BDModernHandheld3DPresenter
    {
        private const float TutorialLessonScreenExitOffset = 342f;
        private const float TutorialLessonScreenTransitionSeconds = 0.62f;
        private const float TutorialLessonScreenFadeInEnd = 0.32f;
        private const float TutorialLessonScreenFadeOutStart = 0.68f;

        private bool firstLaunchTutorialLessonScreenInitialized;
        private bool firstLaunchTutorialLessonCompleteAwaitingTravel;
        private bool firstLaunchTutorialLessonScreenTransitionActive;
        private bool firstLaunchTutorialLessonScreenTransitionApplied;
        private bool firstLaunchTutorialApplyingScreenStep;
        private FirstLaunchTutorialStep firstLaunchTutorialPendingScreenStep;
        private float firstLaunchTutorialLessonScreenCenterX;
        private float firstLaunchTutorialLessonScreenExitX;
        private float firstLaunchTutorialLessonScreenTransitionStartedAt;
        private float firstLaunchTutorialDodgeLessonStartSide;

        private bool ShouldBlockFirstLaunchTutorialActionForTravel()
        {
            return firstLaunchTutorialLessonCompleteAwaitingTravel ||
                firstLaunchTutorialLessonScreenTransitionActive;
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

            // The only lesson exception requested by design: after clearing the
            // jump, mounting and learning the horse may continue on that screen.
            if (current == FirstLaunchTutorialStep.Jump &&
                next == FirstLaunchTutorialStep.MountHorse)
            {
                return true;
            }
            if (current == FirstLaunchTutorialStep.MountHorse &&
                next == FirstLaunchTutorialStep.RideHorse)
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
            if (current == FirstLaunchTutorialStep.HorseShot &&
                next == FirstLaunchTutorialStep.AttackEnemy)
            {
                return true;
            }
            if (current == FirstLaunchTutorialStep.HorseReturn &&
                next == FirstLaunchTutorialStep.HealHorse)
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

            const float nextScreenTravelDistance = 112f;
            firstLaunchTutorialLessonScreenExitX = Mathf.Clamp(
                firstLaunchTutorialPlayerWorldPosition.x +
                    nextScreenTravelDistance,
                TutorialWorldMinX + 24f,
                TutorialWorldMaxX - 28f
            );

            SuspendFirstLaunchTutorialCompletedLessonWorld();
            SetFirstLaunchTutorialLessonInstructionVisible(false);
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
                SetFirstLaunchTutorialInstructionRequested(true);
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

        private bool UpdateFirstLaunchTutorialLessonScreenFlow()
        {
            if (firstLaunchTutorialLessonScreenTransitionActive)
            {
                UpdateFirstLaunchTutorialLessonScreenTransition();
                return true;
            }

            if (!firstLaunchTutorialLessonCompleteAwaitingTravel)
                return false;

            if (firstLaunchTutorialPlayerWorldPosition.x <
                    firstLaunchTutorialLessonScreenExitX)
            {
                return false;
            }

            firstLaunchTutorialLessonScreenTransitionActive = true;
            firstLaunchTutorialLessonScreenTransitionApplied = false;
            firstLaunchTutorialLessonScreenTransitionStartedAt =
                Time.unscaledTime;
            firstLaunchTutorialMovementActive = false;
            return true;
        }

        private void UpdateFirstLaunchTutorialLessonScreenTransition()
        {
            float elapsed = Time.unscaledTime -
                firstLaunchTutorialLessonScreenTransitionStartedAt;
            float progress = Mathf.Clamp01(
                elapsed / TutorialLessonScreenTransitionSeconds
            );
            // The screen change occurs during a real fully-opaque hold. This
            // prevents the new lesson layout from reading as a respawn or
            // exposing one frame of empty/stale instruction UI.
            float alpha;
            if (progress < TutorialLessonScreenFadeInEnd)
            {
                alpha = Mathf.SmoothStep(
                    0f,
                    1f,
                    progress / TutorialLessonScreenFadeInEnd
                );
            }
            else if (progress <= TutorialLessonScreenFadeOutStart)
            {
                alpha = 1f;
            }
            else
            {
                alpha = Mathf.SmoothStep(
                    1f,
                    0f,
                    (progress - TutorialLessonScreenFadeOutStart) /
                        (1f - TutorialLessonScreenFadeOutStart)
                );
            }

            if (firstLaunchTutorialWhiteOverlay != null)
            {
                firstLaunchTutorialWhiteOverlay.gameObject.SetActive(true);
                firstLaunchTutorialWhiteOverlay.transform.SetAsLastSibling();
                Color color = firstLaunchTutorialWhiteOverlay.color;
                color.r = 0.003f;
                color.g = 0.006f;
                color.b = 0.014f;
                color.a = alpha;
                firstLaunchTutorialWhiteOverlay.color = color;
            }

            if (!firstLaunchTutorialLessonScreenTransitionApplied &&
                progress >= 0.5f)
            {
                firstLaunchTutorialLessonScreenTransitionApplied = true;
                firstLaunchTutorialLessonCompleteAwaitingTravel = false;
                firstLaunchTutorialApplyingScreenStep = true;
                ApplyFirstLaunchTutorialStepImmediately(
                    firstLaunchTutorialPendingScreenStep,
                    preserveCurrentScreen: false
                );
                firstLaunchTutorialApplyingScreenStep = false;
            }

            if (progress < 1f)
                return;

            firstLaunchTutorialLessonScreenTransitionActive = false;
            firstLaunchTutorialLessonScreenTransitionApplied = false;
            if (firstLaunchTutorialWhiteOverlay != null)
            {
                Color color = firstLaunchTutorialWhiteOverlay.color;
                color.a = 0f;
                firstLaunchTutorialWhiteOverlay.color = color;
                firstLaunchTutorialWhiteOverlay.gameObject.SetActive(false);
            }
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

        private void ApplyFirstLaunchTutorialStepImmediately(
            FirstLaunchTutorialStep step,
            bool preserveCurrentScreen)
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

            if (firstLaunchTutorialFeedback != null &&
                float.IsPositiveInfinity(firstLaunchTutorialFeedbackClearAt))
            {
                firstLaunchTutorialFeedback.text = string.Empty;
                firstLaunchTutorialFeedbackClearAt = 0f;
            }

            if (!preserveCurrentScreen)
                ResetFirstLaunchTutorialTransientWorldForNewScreen();

            ConfigureFirstLaunchTutorialScene(step);
            if (!preserveCurrentScreen)
                ApplyFirstLaunchTutorialLessonScreenLayout(step);
            else
                ResetFirstLaunchTutorialEnemyVisualPoses();

            firstLaunchTutorialLessonScreenInitialized = true;
            SetFirstLaunchTutorialLessonInstructionVisible(true);
        }

        private void ApplyFirstLaunchTutorialLessonScreenLayout(
            FirstLaunchTutorialStep step)
        {
            float center = ResolveFirstLaunchTutorialLessonScreenCenter(step);
            firstLaunchTutorialLessonScreenCenterX = center;
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
                    firstLaunchTutorialHorseWorldPosition =
                        new Vector2(TutorialHorseStartX, TutorialGroundY - 8f);
                    break;

                case FirstLaunchTutorialStep.EnemyArrival:
                case FirstLaunchTutorialStep.HorseShot:
                case FirstLaunchTutorialStep.AttackEnemy:
                    PositionFirstLaunchTutorialActorForScreen(
                        firstLaunchTutorialEnemy,
                        TutorialEnemyRole.Sword,
                        center,
                        1f,
                        true
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
                    PositionFirstLaunchTutorialActorForScreen(
                        firstLaunchTutorialEnemy,
                        TutorialEnemyRole.Ranged,
                        center,
                        2f,
                        true
                    );
                    firstLaunchTutorialPlayerWorldPosition =
                        new Vector2(center - 250f, TutorialGroundY);
                    firstLaunchTutorialCheckpointX = center - 250f;
                    firstLaunchTutorialParryProjectileProgress = 0f;
                    firstLaunchTutorialParryProjectileActive = true;
                    firstLaunchTutorialProjectileWorldPosition =
                        new Vector2(center - 20f, TutorialGroundY + 34f);
                    break;

                case FirstLaunchTutorialStep.JumpAttack:
                    PositionFirstLaunchTutorialActorForScreen(
                        firstLaunchTutorialEnemy,
                        TutorialEnemyRole.Sword,
                        center,
                        1f,
                        true
                    );
                    break;

                case FirstLaunchTutorialStep.HorseReturn:
                case FirstLaunchTutorialStep.HealHorse:
                case FirstLaunchTutorialStep.RemountHorse:
                    firstLaunchTutorialHorseWorldPosition =
                        new Vector2(center, TutorialGroundY - 8f);
                    firstLaunchTutorialHorseInjured =
                        step != FirstLaunchTutorialStep.RemountHorse;
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
                        center - 118f,
                        1f,
                        true
                    );
                    PositionFirstLaunchTutorialActorForScreen(
                        firstLaunchTutorialEnemySecondary,
                        TutorialEnemyRole.Small,
                        center + 118f,
                        1f,
                        true
                    );
                    break;

                case FirstLaunchTutorialStep.Grapple:
                    PositionFirstLaunchTutorialActorForScreen(
                        firstLaunchTutorialEnemy,
                        TutorialEnemyRole.Sword,
                        center,
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
                                center,
                                TutorialGroundY + 82f
                            );
                        firstLaunchTutorialSecret.gameObject.SetActive(true);
                    }
                    break;

                case FirstLaunchTutorialStep.WallJump:
                    firstLaunchTutorialPlayerWorldPosition =
                        new Vector2(center - 280f, TutorialGroundY);
                    firstLaunchTutorialCheckpointX = center - 280f;
                    break;

                case FirstLaunchTutorialStep.MiniBossIntro:
                case FirstLaunchTutorialStep.MiniBossPhaseOne:
                case FirstLaunchTutorialStep.MiniBossPhaseTwo:
                    PositionFirstLaunchTutorialActorForScreen(
                        firstLaunchTutorialMiniBoss,
                        TutorialEnemyRole.MiniBoss,
                        center,
                        12f,
                        true
                    );
                    break;

                case FirstLaunchTutorialStep.Collectible:
                    firstLaunchTutorialCollectibleWorldPosition =
                        new Vector2(center, TutorialGroundY + 18f);
                    break;
            }

            ResetFirstLaunchTutorialEnemyVisualPoses();
            RenderFirstLaunchTutorialFreePlayCourse(force: true);
        }

        private float ResolveFirstLaunchTutorialLessonScreenCenter(
            FirstLaunchTutorialStep step)
        {
            switch (step)
            {
                case FirstLaunchTutorialStep.Move:
                    return -650f;
                case FirstLaunchTutorialStep.Jump:
                case FirstLaunchTutorialStep.MountHorse:
                case FirstLaunchTutorialStep.RideHorse:
                    return TutorialJumpObstacleX;
                case FirstLaunchTutorialStep.EnemyArrival:
                case FirstLaunchTutorialStep.HorseShot:
                case FirstLaunchTutorialStep.AttackEnemy:
                    return TutorialAmbushEnemyX;
                case FirstLaunchTutorialStep.HeavyAttack:
                    return TutorialHeavyTargetX;
                case FirstLaunchTutorialStep.Dodge:
                    return TutorialHazardX;
                case FirstLaunchTutorialStep.Parry:
                    return TutorialParryTargetX;
                case FirstLaunchTutorialStep.JumpAttack:
                    return 1320f;
                case FirstLaunchTutorialStep.HorseReturn:
                case FirstLaunchTutorialStep.HealHorse:
                case FirstLaunchTutorialStep.RemountHorse:
                    return 1480f;
                case FirstLaunchTutorialStep.RangedAttack:
                case FirstLaunchTutorialStep.Reload:
                case FirstLaunchTutorialStep.ChargedShot:
                case FirstLaunchTutorialStep.MountedImpact:
                    return TutorialMountedStationX;
                case FirstLaunchTutorialStep.DismountHorse:
                    return TutorialDismountMarkerX;
                case FirstLaunchTutorialStep.SpinAttack:
                    return TutorialSpinTargetX;
                case FirstLaunchTutorialStep.Grapple:
                    return TutorialGapX;
                case FirstLaunchTutorialStep.HazardKnockback:
                    return TutorialHazardStationX;
                case FirstLaunchTutorialStep.SidePath:
                    return TutorialSecretBranchX;
                case FirstLaunchTutorialStep.CombinedEncounter:
                    return TutorialCombinedStationX;
                case FirstLaunchTutorialStep.WallJump:
                    return TutorialWallJumpWallX;
                case FirstLaunchTutorialStep.MiniBossIntro:
                case FirstLaunchTutorialStep.MiniBossPhaseOne:
                case FirstLaunchTutorialStep.MiniBossPhaseTwo:
                case FirstLaunchTutorialStep.MiniBossDefeated:
                    return TutorialMiniBossStationX;
                case FirstLaunchTutorialStep.Collectible:
                case FirstLaunchTutorialStep.Completed:
                    return TutorialCollectibleX;
                default:
                    return Mathf.Clamp(
                        firstLaunchTutorialPlayerWorldPosition.x + 270f,
                        TutorialWorldMinX + TutorialWorldViewportHalfWidth,
                        TutorialWorldMaxX - TutorialWorldViewportHalfWidth
                    );
            }
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
            if (firstLaunchTutorialLessonScreenTransitionActive)
                return firstLaunchTutorialLessonScreenCenterX;
            if (firstLaunchTutorialLessonCompleteAwaitingTravel)
                return normalTarget;
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
                case FirstLaunchTutorialStep.MountHorse:
                case FirstLaunchTutorialStep.RideHorse: return 3;
                case FirstLaunchTutorialStep.EnemyArrival:
                case FirstLaunchTutorialStep.HorseShot:
                case FirstLaunchTutorialStep.AttackEnemy: return 4;
                case FirstLaunchTutorialStep.HeavyAttack: return 5;
                case FirstLaunchTutorialStep.Dodge: return 6;
                case FirstLaunchTutorialStep.Parry: return 7;
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
                firstLaunchTutorialMounted)
            {
                return false;
            }

            float tutorialProjectileDistance = Vector2.Distance(
                firstLaunchTutorialPlayerWorldPosition,
                firstLaunchTutorialProjectileWorldPosition
            );
            if (firstLaunchTutorialParryProjectileActive &&
                tutorialProjectileDistance <= 224f)
            {
                PlayFirstLaunchTutorialParryAnimation(
                    firstLaunchTutorialStep ==
                        FirstLaunchTutorialStep.Parry
                );
                return true;
            }

            if (firstLaunchTutorialEnemyProjectileActive &&
                tutorialProjectileDistance <= 224f)
            {
                CancelFirstLaunchTutorialEnemyProjectile();
                PlayFirstLaunchTutorialParryAnimation(false);
                SetFirstLaunchTutorialLearningState(
                    "Parry",
                    TutorialLearningState.Demonstrated
                );
                return true;
            }

            return TryResolveFirstLaunchTutorialContextParry();
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
