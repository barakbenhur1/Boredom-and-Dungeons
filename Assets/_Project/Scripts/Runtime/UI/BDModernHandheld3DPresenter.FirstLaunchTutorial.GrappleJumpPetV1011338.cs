using UnityEngine;

#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace BoredomAndDungeons
{
    public sealed partial class BDModernHandheld3DPresenter
    {
        // BD GRAPPLE / JUMP ATTACK / PET OWNER V10.11.30.38
        private TutorialEnemyActor
            firstLaunchTutorialProfessionalGrappleTargetV1011338;
        private bool
            firstLaunchTutorialGrappleFinisherInFlightV1011338;

        private void ResetFirstLaunchTutorialGrappleJumpPetStateV1011338(
            FirstLaunchTutorialStep step)
        {
            if (step == FirstLaunchTutorialStep.Grapple)
            {
                firstLaunchTutorialProfessionalGrappleTargetV1011338 = null;
                firstLaunchTutorialGrappleFinisherInFlightV1011338 = false;
                firstLaunchTutorialGrappleFinishPendingV101125 = false;
                firstLaunchTutorialPendingHookTarget = null;
            }
            else if (step != FirstLaunchTutorialStep.HazardKnockback)
            {
                firstLaunchTutorialGrappleFinisherInFlightV1011338 = false;
            }

            if (step == FirstLaunchTutorialStep.PetHorse)
            {
                firstLaunchTutorialMounted = false;
                firstLaunchTutorialHorseInjured = false;
            }
        }

        private void UpdateFirstLaunchTutorialPetInputV1011338()
        {
            if (firstLaunchTutorialStep !=
                    FirstLaunchTutorialStep.PetHorse ||
                IsFirstLaunchTutorialActionInputLocked() ||
                ShouldBlockFirstLaunchTutorialActionForTravel())
            {
                return;
            }

            bool pressed = false;
            FirstLaunchTutorialInputSource source =
                FirstLaunchTutorialInputSource.Keyboard;

#if ENABLE_INPUT_SYSTEM
            if (Keyboard.current != null &&
                Keyboard.current.tabKey.wasPressedThisFrame)
            {
                pressed = true;
                source = FirstLaunchTutorialInputSource.Keyboard;
            }
            else if (Gamepad.current != null &&
                     Gamepad.current.selectButton.wasPressedThisFrame)
            {
                pressed = true;
                source = FirstLaunchTutorialInputSource.Gamepad;
            }
#endif

#if ENABLE_LEGACY_INPUT_MANAGER
            if (!pressed && Input.GetKeyDown(KeyCode.Tab))
            {
                pressed = true;
                source = FirstLaunchTutorialInputSource.Keyboard;
            }
            else if (!pressed &&
                     Input.GetKeyDown(KeyCode.JoystickButton6))
            {
                pressed = true;
                source = FirstLaunchTutorialInputSource.Gamepad;
            }
#endif

            if (!pressed)
                return;

            SetFirstLaunchTutorialInputSource(source);
            TryStartFirstLaunchTutorialPetHorseV1011338();
        }

        private bool TryStartFirstLaunchTutorialPetHorseV1011338()
        {
            if (firstLaunchTutorialStep !=
                    FirstLaunchTutorialStep.PetHorse ||
                IsFirstLaunchTutorialActionInputLocked() ||
                ShouldBlockFirstLaunchTutorialActionForTravel())
            {
                return false;
            }

            if (firstLaunchTutorialMounted)
            {
                ShowFirstLaunchTutorialSuccess(
                    "DISMOUNT BEFORE PETTING"
                );
                return false;
            }

            if (firstLaunchTutorialHorseInjured)
            {
                ShowFirstLaunchTutorialSuccess(
                    "HEAL THE HORSE FIRST"
                );
                return false;
            }

            float distance = Vector2.Distance(
                firstLaunchTutorialPlayerWorldPosition,
                firstLaunchTutorialHorseWorldPosition
            );
            if (distance > TutorialMountRange + 22f)
            {
                ShowFirstLaunchTutorialSuccess("MOVE CLOSER");
                return false;
            }

            BeginFirstLaunchTutorialActionPresentation(
                FirstLaunchTutorialActionPresentationType.PetHorse,
                0.78f,
                advancesLesson: true,
                firstLaunchTutorialPlayerWorldPosition,
                firstLaunchTutorialHorseWorldPosition
            );
            PlayClick();
            return true;
        }

        private void UpdateFirstLaunchTutorialPetHorsePresentationV1011338(
            float progress)
        {
            float ease = progress * progress * (3f - 2f * progress);
            float stroke = Mathf.Sin(progress * Mathf.PI * 4f);
            float affection = Mathf.Sin(progress * Mathf.PI);

            float side = Mathf.Sign(
                firstLaunchTutorialPlayerWorldPosition.x -
                firstLaunchTutorialHorseWorldPosition.x
            );
            if (Mathf.Abs(side) < 0.5f)
                side = -1f;

            Vector2 petPosition =
                firstLaunchTutorialHorseWorldPosition +
                new Vector2(side * 76f, 0f);
            firstLaunchTutorialPlayerWorldPosition = Vector2.Lerp(
                firstLaunchTutorialActionStartWorld,
                petPosition,
                Mathf.Clamp01(ease / 0.45f)
            );

            ApplyFirstLaunchTutorialPlayerActionPose(
                firstLaunchTutorialPlayerWorldPosition,
                1f + affection * 0.06f,
                1f - affection * 0.12f,
                4f + Mathf.Max(0f, stroke) * 5f
            );
            ApplyFirstLaunchTutorialHorseActionPose(
                firstLaunchTutorialHorseWorldPosition,
                1f + affection * 0.05f,
                1f - affection * 0.04f,
                stroke * 2f,
                Mathf.Abs(stroke) * 3f
            );

            SetEffectVisible(firstLaunchTutorialHealEffect, true);
            if (firstLaunchTutorialHealEffect != null)
            {
                firstLaunchTutorialHealEffect.anchoredPosition =
                    SnapFirstLaunchTutorialWorldPosition(
                        firstLaunchTutorialHorseWorldPosition +
                        new Vector2(
                            side * -14f,
                            40f + affection * 18f
                        )
                    );
                firstLaunchTutorialHealEffect.localRotation =
                    Quaternion.Euler(0f, 0f, stroke * 10f);
                firstLaunchTutorialHealEffect.localScale =
                    Vector3.one * (0.48f + affection * 0.44f);
            }
        }

        private void CompleteFirstLaunchTutorialPetHorseAnimationV1011338()
        {
            SetEffectVisible(firstLaunchTutorialHealEffect, false);
            firstLaunchTutorialHorseInjured = false;
            firstLaunchTutorialMounted = false;
            SetFirstLaunchTutorialLearningState(
                "PetHorse",
                TutorialLearningState.Demonstrated
            );
            ShowFirstLaunchTutorialSuccess("HORSE CALMED");
            SetFirstLaunchTutorialStep(
                FirstLaunchTutorialStep.RemountHorse
            );
        }

        private void CompleteFirstLaunchTutorialHealThenPetV1011338(
            bool advancesLesson)
        {
            if (!advancesLesson)
            {
                CompleteFirstLaunchTutorialHealAnimation(false);
                return;
            }

            SetFirstLaunchTutorialHealingPreview(0f, visible: false);
            firstLaunchTutorialHorseInjured = false;
            firstLaunchTutorialMounted = false;
            SetFirstLaunchTutorialLearningState(
                "Heal",
                TutorialLearningState.Demonstrated
            );
            ShowFirstLaunchTutorialSuccess("HORSE HEALED");
            SetFirstLaunchTutorialStep(
                FirstLaunchTutorialStep.PetHorse
            );
        }

        private void PrepareFirstLaunchTutorialProfessionalGrappleTargetV1011338(
            TutorialEnemyActor target)
        {
            if (target == null)
                return;

            firstLaunchTutorialProfessionalGrappleTargetV1011338 = target;
            firstLaunchTutorialPendingHookTarget = target;
            firstLaunchTutorialGrappleFinishPendingV101125 = false;
            target.MaximumHealth = Mathf.Max(1f, target.MaximumHealth);
            target.Health = Mathf.Max(1f, target.Health);
            target.Active = true;
            target.Dead = false;
            target.AttackCommitted = false;
            target.DamageApplied = false;
            target.NextActionAt = float.PositiveInfinity;
            if (target.Image != null)
                target.Image.gameObject.SetActive(true);
        }

        private void CompleteFirstLaunchTutorialProfessionalGrapplePullV1011338(
            bool advancesLesson)
        {
            if (firstLaunchTutorialStep !=
                    FirstLaunchTutorialStep.Grapple ||
                !advancesLesson)
            {
                CompleteFirstLaunchTutorialGrappleAnimation(advancesLesson);
                return;
            }

            TutorialEnemyActor target =
                firstLaunchTutorialProfessionalGrappleTargetV1011338 ??
                firstLaunchTutorialPendingHookTarget;
            if (target == null)
            {
                firstLaunchTutorialGrappleFinishPendingV101125 = false;
                ShowFirstLaunchTutorialSuccess("THE HOOK FOUND NOTHING");
                return;
            }

            float side = Mathf.Sign(
                target.Position.x -
                firstLaunchTutorialPlayerWorldPosition.x
            );
            if (Mathf.Abs(side) < 0.5f)
                side = 1f;

            target.Position =
                firstLaunchTutorialPlayerWorldPosition +
                new Vector2(side * 104f, 0f);
            target.SpawnPosition = target.Position;
            target.MaximumHealth = Mathf.Max(1f, target.MaximumHealth);
            target.Health = Mathf.Max(1f, target.Health);
            target.Active = true;
            target.Dead = false;
            target.AttackCommitted = false;
            target.DamageApplied = false;
            target.NextActionAt = float.PositiveInfinity;
            if (target.Image != null)
            {
                target.Image.gameObject.SetActive(true);
                target.Image.rectTransform.localRotation =
                    Quaternion.identity;
                target.Image.rectTransform.localScale = Vector3.one;
            }
            if (target.Image == firstLaunchTutorialEnemy)
                firstLaunchTutorialEnemyWorldPosition = target.Position;

            firstLaunchTutorialPendingHookTarget = target;
            firstLaunchTutorialGrappleFinishPendingV101125 = true;
            firstLaunchTutorialLessonHitConfirmedV101125 = false;
            SetFirstLaunchTutorialLearningState(
                "Grapple",
                TutorialLearningState.Performed
            );
            SetFirstLaunchTutorialInstructionRequested(true);
            UpdateFirstLaunchTutorialPrompt();
            ShowFirstLaunchTutorialSuccess(
                "ENEMY PULLED — ATTACK NOW"
            );
        }

        private bool
            TryHandleFirstLaunchTutorialProfessionalGrappleFinisherV1011338(
                bool heavy)
        {
            if (firstLaunchTutorialStep !=
                    FirstLaunchTutorialStep.Grapple ||
                !firstLaunchTutorialGrappleFinishPendingV101125)
            {
                return false;
            }

            TutorialEnemyActor target =
                firstLaunchTutorialProfessionalGrappleTargetV1011338 ??
                firstLaunchTutorialPendingHookTarget;
            float range = TutorialAttackRange + (heavy ? 24f : 0f);
            bool valid = target != null &&
                target.Active &&
                !target.Dead &&
                IsFirstLaunchTutorialTargetInFront(
                    target.Position,
                    range
                );
            if (!valid)
            {
                ShowFirstLaunchTutorialSuccess(
                    "FACE THE PULLED ENEMY AND ATTACK"
                );
                return true;
            }

            firstLaunchTutorialGrappleFinisherInFlightV1011338 = true;
            firstLaunchTutorialProfessionalGrappleTargetV1011338 = target;
            BeginFirstLaunchTutorialMeleeTransaction(
                target,
                Mathf.Max(1f, target.Health),
                range,
                heavy
            );

            if (heavy)
                PlayFirstLaunchTutorialHeavyAttackAnimation(false);
            else
                PlayFirstLaunchTutorialLightAttackAnimation(false);
            return true;
        }

        private void
            CompleteFirstLaunchTutorialProfessionalGrappleFinisherV1011338()
        {
            TutorialEnemyActor target =
                firstLaunchTutorialProfessionalGrappleTargetV1011338 ??
                firstLaunchTutorialPendingHookTarget;

            firstLaunchTutorialGrappleFinisherInFlightV1011338 = false;
            if (target == null)
            {
                firstLaunchTutorialGrappleFinishPendingV101125 = false;
                ShowFirstLaunchTutorialSuccess("PULL THE ENEMY AGAIN");
                return;
            }

            ForceFirstLaunchTutorialLessonActorDeathV101128(target);
            firstLaunchTutorialGrappleFinishPendingV101125 = false;
            firstLaunchTutorialPendingHookTarget = null;
            firstLaunchTutorialProfessionalGrappleTargetV1011338 = null;
            firstLaunchTutorialLessonHitConfirmedV101125 = true;
            SetFirstLaunchTutorialLearningState(
                "Grapple",
                TutorialLearningState.Demonstrated
            );
            ShowFirstLaunchTutorialSuccess(
                "PULL AND FINISH COMPLETE"
            );
            SetFirstLaunchTutorialStep(
                FirstLaunchTutorialStep.HazardKnockback
            );
        }

        private bool RejectFirstLaunchTutorialGroundedJumpAttackV1011338()
        {
            if (firstLaunchTutorialStep !=
                    FirstLaunchTutorialStep.JumpAttack ||
                !firstLaunchTutorialGrounded)
            {
                return false;
            }

            ProtectFirstLaunchTutorialJumpAttackTargetV1011338();
            ShowFirstLaunchTutorialSuccess(
                "JUMP FIRST — GROUND ATTACKS CANNOT HURT THIS TARGET"
            );
            PlayFirstLaunchTutorialLightAttackAnimation(false);
            return true;
        }

        private bool RejectFirstLaunchTutorialNonJumpAttackV1011338(
            string feedback)
        {
            if (firstLaunchTutorialStep !=
                    FirstLaunchTutorialStep.JumpAttack)
            {
                return false;
            }

            ProtectFirstLaunchTutorialJumpAttackTargetV1011338();
            ShowFirstLaunchTutorialSuccess(feedback);
            return true;
        }

        private void ProtectFirstLaunchTutorialJumpAttackTargetV1011338()
        {
            TutorialEnemyActor target =
                FindFirstLaunchTutorialActorByImageV101128(
                    firstLaunchTutorialEnemy
                );
            if (target == null)
                return;

            target.MaximumHealth = Mathf.Max(1f, target.MaximumHealth);
            target.Health = Mathf.Max(1f, target.Health);
            target.Active = true;
            target.Dead = false;
            target.AttackCommitted = false;
            target.DamageApplied = false;
            target.NextActionAt = float.PositiveInfinity;
            if (target.Image != null)
                target.Image.gameObject.SetActive(true);
        }
    }
}
