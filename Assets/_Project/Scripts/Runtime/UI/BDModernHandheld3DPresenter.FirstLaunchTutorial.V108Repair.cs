using UnityEngine;

namespace BoredomAndDungeons
{
    public sealed partial class BDModernHandheld3DPresenter
    {
        private TutorialEnemyActor firstLaunchTutorialPendingShotTarget;
        private float firstLaunchTutorialPendingShotDamage;
        private FirstLaunchTutorialStep firstLaunchTutorialPendingShotStep;
        private bool firstLaunchTutorialPendingShotCharged;
        private bool firstLaunchTutorialPendingShotAdvancesLesson;
        private bool firstLaunchTutorialPendingShotImpactResolved;
        private bool firstLaunchTutorialChargedShotAutoFired;

        private TutorialEnemyActor firstLaunchTutorialPendingHookTarget;
        private float firstLaunchTutorialPendingHookDamage;

        private Vector2 firstLaunchTutorialEnemyProjectileStart;
        private Vector2 firstLaunchTutorialEnemyProjectileTarget;
        private float firstLaunchTutorialEnemyProjectileStartedAt;
        private float firstLaunchTutorialEnemyProjectileDuration;
        private bool firstLaunchTutorialEnemyProjectileActive;

        private const float TutorialPlayerCollisionRadius = 30f;
        private const float TutorialMountedCollisionRadius = 54f;
        private const float TutorialEnemyCollisionRadius = 32f;

        private void ResetFirstLaunchTutorialV108Transactions()
        {
            firstLaunchTutorialPendingShotTarget = null;
            firstLaunchTutorialPendingShotDamage = 0f;
            firstLaunchTutorialPendingShotStep = firstLaunchTutorialStep;
            firstLaunchTutorialPendingShotCharged = false;
            firstLaunchTutorialPendingShotAdvancesLesson = false;
            firstLaunchTutorialPendingShotImpactResolved = false;
            firstLaunchTutorialChargedShotAutoFired = false;
            firstLaunchTutorialPendingHookTarget = null;
            firstLaunchTutorialPendingHookDamage = 0f;
            firstLaunchTutorialEnemyProjectileStart = Vector2.zero;
            firstLaunchTutorialEnemyProjectileTarget = Vector2.zero;
            firstLaunchTutorialEnemyProjectileStartedAt = 0f;
            firstLaunchTutorialEnemyProjectileDuration = 0f;
            CancelFirstLaunchTutorialEnemyProjectile();
        }

        private void BeginFirstLaunchTutorialShotTransaction(
            TutorialEnemyActor target,
            float damage,
            bool charged,
            bool advancesLesson)
        {
            firstLaunchTutorialPendingShotTarget = target;
            firstLaunchTutorialPendingShotDamage = Mathf.Max(0f, damage);
            firstLaunchTutorialPendingShotStep = firstLaunchTutorialStep;
            firstLaunchTutorialPendingShotCharged = charged;
            firstLaunchTutorialPendingShotAdvancesLesson = advancesLesson;
            firstLaunchTutorialPendingShotImpactResolved = false;
        }

        private void ResolveFirstLaunchTutorialRangedProjectileImpact()
        {
            if (firstLaunchTutorialPendingShotImpactResolved)
                return;

            firstLaunchTutorialPendingShotImpactResolved = true;
            TutorialEnemyActor target = firstLaunchTutorialPendingShotTarget;
            bool hitLivingTarget =
                target != null && target.Active && !target.Dead;
            if (hitLivingTarget)
            {
                ApplyFirstLaunchTutorialActorDamage(
                    target,
                    firstLaunchTutorialPendingShotDamage
                );
            }

            if (hitLivingTarget &&
                firstLaunchTutorialPendingShotAdvancesLesson &&
                firstLaunchTutorialPendingShotStep ==
                    FirstLaunchTutorialStep.RangedAttack)
            {
                CompleteFirstLaunchTutorialMountedShotLessonAtImpact();
            }
            else if (firstLaunchTutorialPendingShotCharged &&
                     firstLaunchTutorialPendingShotStep ==
                         FirstLaunchTutorialStep.ChargedShot)
            {
                ShowFirstLaunchTutorialSuccess("CHARGED IMPACT");
            }

            firstLaunchTutorialPendingShotTarget = null;
            firstLaunchTutorialPendingShotDamage = 0f;
        }


        private void CompleteFirstLaunchTutorialMountedShotLessonAtImpact()
        {
            if (firstLaunchTutorialStep !=
                FirstLaunchTutorialStep.RangedAttack)
            {
                return;
            }

            SetFirstLaunchTutorialLearningState(
                "MountedShot",
                TutorialLearningState.Demonstrated
            );
            ShowFirstLaunchTutorialSuccess("PROJECTILE IMPACT");
            SetFirstLaunchTutorialStep(FirstLaunchTutorialStep.Reload);
        }

        private float ResolveFirstLaunchTutorialChargedShotSeconds()
        {
            int additionalRounds = Mathf.Max(0, firstLaunchTutorialAmmo - 2);
            return Mathf.Min(
                TutorialChargedShotMaximumSeconds,
                TutorialChargedShotBaseSeconds +
                additionalRounds *
                    TutorialChargedShotSecondsPerAdditionalRound
            );
        }

        private void BeginFirstLaunchTutorialChargedShotInput()
        {
            if (firstLaunchTutorialStep !=
                    FirstLaunchTutorialStep.ChargedShot ||
                firstLaunchTutorialChargedShotAutoFired ||
                Time.unscaledTime < firstLaunchTutorialReloadCompletesAt)
            {
                return;
            }

            if (firstLaunchTutorialAmmo < 2)
            {
                BeginFirstLaunchTutorialReload();
                return;
            }

            if (firstLaunchTutorialChargedShotPendingStartedAt < 0f &&
                firstLaunchTutorialChargedShotStartedAt < 0f)
            {
                firstLaunchTutorialChargedShotPendingStartedAt =
                    Time.unscaledTime;
                ShowFirstLaunchTutorialSuccess("KEEP HOLDING");
            }
        }

        private void UpdateFirstLaunchTutorialChargedShotHold()
        {
            if (firstLaunchTutorialChargedShotAutoFired)
                return;

            bool held = IsFirstLaunchTutorialRangedHeld();
            float now = Time.unscaledTime;

            if (firstLaunchTutorialChargedShotPendingStartedAt < 0f &&
                firstLaunchTutorialChargedShotStartedAt < 0f)
            {
                if (held)
                    BeginFirstLaunchTutorialChargedShotInput();
                return;
            }

            if (firstLaunchTutorialChargedShotStartedAt < 0f)
            {
                float pendingElapsed =
                    now - firstLaunchTutorialChargedShotPendingStartedAt;
                if (!held)
                {
                    // The production mechanic treats a release before the hold
                    // threshold as an ordinary shot. It does not complete this
                    // charged-shot lesson, and the magazine is restored for retry.
                    FireFirstLaunchTutorialChargedLessonOrdinaryShot();
                    return;
                }

                ShowFirstLaunchTutorialHoldProgress(
                    "HOLD TO BEGIN CHARGE",
                    pendingElapsed /
                        TutorialChargedShotHoldThresholdSeconds,
                    visible: true
                );
                if (pendingElapsed <
                        TutorialChargedShotHoldThresholdSeconds)
                {
                    return;
                }

                firstLaunchTutorialChargedShotStartedAt = now;
                firstLaunchTutorialChargedShotPendingStartedAt = -1f;
            }

            float duration = ResolveFirstLaunchTutorialChargedShotSeconds();
            float progress = Mathf.Clamp01(
                (now - firstLaunchTutorialChargedShotStartedAt) /
                Mathf.Max(0.01f, duration)
            );

            if (!held && progress < 1f)
            {
                firstLaunchTutorialChargedShotStartedAt = -1f;
                ShowFirstLaunchTutorialHoldProgress(
                    "CHARGE CANCELLED — KEEP HOLDING",
                    0f,
                    visible: false
                );
                ShowFirstLaunchTutorialSuccess(
                    "RELEASE AFTER CHARGE STARTS CANCELS IT"
                );
                return;
            }

            ShowFirstLaunchTutorialHoldProgress(
                "CHARGED SHOT",
                progress,
                visible: true
            );
            if (progress < 1f)
                return;

            FireFirstLaunchTutorialChargedShotAutomatically();
        }

        private void FireFirstLaunchTutorialChargedLessonOrdinaryShot()
        {
            firstLaunchTutorialChargedShotPendingStartedAt = -1f;
            firstLaunchTutorialChargedShotStartedAt = -1f;
            TutorialEnemyActor target =
                FindClosestLivingTutorialActor(620f);
            Vector2 targetWorld = target != null
                ? target.Position
                : firstLaunchTutorialPlayerWorldPosition +
                    Vector2.right * 360f;

            firstLaunchTutorialAmmo = Mathf.Max(
                0,
                firstLaunchTutorialAmmo - 1
            );
            BeginFirstLaunchTutorialShotTransaction(
                target,
                1f,
                charged: false,
                advancesLesson: false
            );
            PlayFirstLaunchTutorialRangedAttackAnimation(
                targetWorld,
                advancesLesson: false,
                chargedShot: false
            );
            ShowFirstLaunchTutorialSuccess(
                "ORDINARY SHOT — HOLD LONGER"
            );

            // Keep the lesson deterministic: a premature tap demonstrates the
            // real ordinary-shot branch, then restores a full magazine.
            firstLaunchTutorialAmmo = TutorialMagazineSize;
        }

        private void FireFirstLaunchTutorialChargedShotAutomatically()
        {
            firstLaunchTutorialChargedShotAutoFired = true;
            firstLaunchTutorialChargedShotStartedAt = -1f;
            firstLaunchTutorialChargedShotPendingStartedAt = -1f;
            ShowFirstLaunchTutorialHoldProgress(
                string.Empty,
                0f,
                visible: false
            );

            TutorialEnemyActor target =
                FindClosestLivingTutorialActor(720f);
            Vector2 targetWorld = target != null
                ? target.Position
                : firstLaunchTutorialPlayerWorldPosition +
                    Vector2.right * 420f;
            float damage = Mathf.Max(2f, firstLaunchTutorialAmmo);
            firstLaunchTutorialAmmo = 0;

            BeginFirstLaunchTutorialShotTransaction(
                target,
                damage,
                charged: true,
                advancesLesson: false
            );
            PlayFirstLaunchTutorialRangedAttackAnimation(
                targetWorld,
                advancesLesson: false,
                chargedShot: true
            );
            BeginFirstLaunchTutorialReload();
            ShowFirstLaunchTutorialSuccess("AUTO FIRE");
        }

        private void BeginFirstLaunchTutorialProductionHookTransaction(
            TutorialEnemyActor target,
            float damage)
        {
            firstLaunchTutorialPendingHookTarget = target;
            firstLaunchTutorialPendingHookDamage = Mathf.Max(0f, damage);
        }

        private void CompleteFirstLaunchTutorialProductionHookTransaction()
        {
            TutorialEnemyActor target = firstLaunchTutorialPendingHookTarget;
            if (target != null && target.Active && !target.Dead)
            {
                ApplyFirstLaunchTutorialActorDamage(
                    target,
                    firstLaunchTutorialPendingHookDamage
                );
            }
            firstLaunchTutorialPendingHookTarget = null;
            firstLaunchTutorialPendingHookDamage = 0f;
        }

        private float ResolveFirstLaunchTutorialCollisionX(
            float currentX,
            float requestedX)
        {
            float direction = Mathf.Sign(requestedX - currentX);
            if (Mathf.Abs(direction) < 0.001f)
                return requestedX;

            float playerRadius = firstLaunchTutorialMounted
                ? TutorialMountedCollisionRadius
                : TutorialPlayerCollisionRadius;
            float resolvedX = requestedX;
            for (int index = 0;
                 index < firstLaunchTutorialActors.Count;
                 index++)
            {
                TutorialEnemyActor actor = firstLaunchTutorialActors[index];
                if (!actor.Active || actor.Dead || actor.Image == null)
                    continue;

                float actorRadius = actor.Role == TutorialEnemyRole.MiniBoss
                    ? 64f
                    : TutorialEnemyCollisionRadius;
                float separation = playerRadius + actorRadius;
                float actorX = actor.Position.x;
                bool crossingFromLeft = direction > 0f &&
                    currentX <= actorX &&
                    requestedX > actorX - separation;
                bool crossingFromRight = direction < 0f &&
                    currentX >= actorX &&
                    requestedX < actorX + separation;
                bool alreadyOverlapping =
                    Mathf.Abs(requestedX - actorX) < separation;

                if (crossingFromLeft ||
                    (alreadyOverlapping && currentX < actorX))
                {
                    resolvedX = Mathf.Min(
                        resolvedX,
                        actorX - separation
                    );
                }
                else if (crossingFromRight ||
                         (alreadyOverlapping && currentX > actorX))
                {
                    resolvedX = Mathf.Max(
                        resolvedX,
                        actorX + separation
                    );
                }
            }
            return resolvedX;
        }

        private void SetFirstLaunchTutorialCheckpoint(float safeX)
        {
            firstLaunchTutorialCheckpointX = Mathf.Clamp(
                safeX,
                TutorialWorldMinX + 40f,
                TutorialWorldMaxX - 40f
            );
        }

        private void LaunchFirstLaunchTutorialEnemyProjectile(
            TutorialEnemyActor source)
        {
            if (source == null || firstLaunchTutorialEnemyProjectileActive)
                return;

            firstLaunchTutorialEnemyProjectileStart =
                source.Position + new Vector2(-18f, 28f);
            firstLaunchTutorialEnemyProjectileTarget =
                firstLaunchTutorialPlayerWorldPosition +
                new Vector2(0f, 20f);
            firstLaunchTutorialEnemyProjectileStartedAt = Time.unscaledTime;
            firstLaunchTutorialEnemyProjectileDuration =
                source.Role == TutorialEnemyRole.MiniBoss ? 0.78f : 0.92f;
            firstLaunchTutorialEnemyProjectileActive = true;
            firstLaunchTutorialProjectileWorldPosition =
                firstLaunchTutorialEnemyProjectileStart;
            if (firstLaunchTutorialProjectile != null)
                firstLaunchTutorialProjectile.gameObject.SetActive(true);
        }

        private void UpdateFirstLaunchTutorialEnemyProjectileTransaction()
        {
            if (!firstLaunchTutorialEnemyProjectileActive)
                return;

            float progress = Mathf.Clamp01(
                (Time.unscaledTime -
                 firstLaunchTutorialEnemyProjectileStartedAt) /
                Mathf.Max(0.01f, firstLaunchTutorialEnemyProjectileDuration)
            );
            firstLaunchTutorialProjectileWorldPosition = Vector2.Lerp(
                firstLaunchTutorialEnemyProjectileStart,
                firstLaunchTutorialEnemyProjectileTarget,
                progress
            );
            if (progress < 1f)
                return;

            bool hit = Vector2.Distance(
                firstLaunchTutorialPlayerWorldPosition +
                    new Vector2(0f, 20f),
                firstLaunchTutorialEnemyProjectileTarget
            ) <= 78f;
            if (hit &&
                Time.unscaledTime >= firstLaunchTutorialInvulnerableUntil)
            {
                DamageFirstLaunchTutorialPlayer(1);
            }
            CancelFirstLaunchTutorialEnemyProjectile();
        }

        private void CancelFirstLaunchTutorialEnemyProjectile()
        {
            firstLaunchTutorialEnemyProjectileActive = false;
            if (firstLaunchTutorialProjectile != null &&
                !firstLaunchTutorialParryProjectileActive)
            {
                firstLaunchTutorialProjectile.gameObject.SetActive(false);
            }
        }

        private string ResolveFirstLaunchTutorialBossState(
            TutorialEnemyActor boss)
        {
            if (boss == null)
                return string.Empty;
            if (boss.AttackCommitted)
            {
                float elapsed =
                    Time.unscaledTime - boss.ActionStartedAt;
                if (elapsed < 0.52f)
                    return "WINDUP — MOVE / DODGE";
                return "IMPACT — DO NOT TRADE";
            }
            if (Time.unscaledTime < boss.NextActionAt)
                return "RECOVERY — ATTACK NOW";
            return "READY — WATCH THE TELEGRAPH";
        }
    }
}
