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
        private bool firstLaunchTutorialPendingShotHitResolved;
        private bool firstLaunchTutorialChargedShotAutoFired;

        private TutorialEnemyActor firstLaunchTutorialPendingMeleeTarget;
        private float firstLaunchTutorialPendingMeleeDamage;
        private float firstLaunchTutorialPendingMeleeRange;
        private Vector2 firstLaunchTutorialPendingMeleeDirection;
        private bool firstLaunchTutorialPendingMeleeHeavy;
        private bool firstLaunchTutorialPendingMeleeImpactResolved;
        private bool firstLaunchTutorialPendingMeleeHitResolved;

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
        private const float TutorialProjectileHitRadius = 58f;

        private void ResetFirstLaunchTutorialV108Transactions()
        {
            firstLaunchTutorialPendingShotTarget = null;
            firstLaunchTutorialPendingShotDamage = 0f;
            firstLaunchTutorialPendingShotStep = firstLaunchTutorialStep;
            firstLaunchTutorialPendingShotCharged = false;
            firstLaunchTutorialPendingShotAdvancesLesson = false;
            firstLaunchTutorialPendingShotImpactResolved = false;
            firstLaunchTutorialPendingShotHitResolved = false;
            firstLaunchTutorialChargedShotAutoFired = false;
            firstLaunchTutorialPendingMeleeTarget = null;
            firstLaunchTutorialPendingMeleeDamage = 0f;
            firstLaunchTutorialPendingMeleeRange = 0f;
            firstLaunchTutorialPendingMeleeDirection = Vector2.right;
            firstLaunchTutorialPendingMeleeHeavy = false;
            firstLaunchTutorialPendingMeleeImpactResolved = false;
            firstLaunchTutorialPendingMeleeHitResolved = false;
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
            firstLaunchTutorialPendingShotHitResolved = false;
        }

        private void BeginFirstLaunchTutorialMeleeTransaction(
            TutorialEnemyActor target,
            float damage,
            float range,
            bool heavy)
        {
            firstLaunchTutorialPendingMeleeTarget = target;
            firstLaunchTutorialPendingMeleeDamage = Mathf.Max(0f, damage);
            firstLaunchTutorialPendingMeleeRange = Mathf.Max(0f, range);
            firstLaunchTutorialPendingMeleeDirection =
                ResolveFirstLaunchTutorialActionDirection();
            firstLaunchTutorialPendingMeleeHeavy = heavy;
            firstLaunchTutorialPendingMeleeImpactResolved = false;
            firstLaunchTutorialPendingMeleeHitResolved = false;
        }

        private void ResolveFirstLaunchTutorialMeleeImpact()
        {
            if (firstLaunchTutorialPendingMeleeImpactResolved)
                return;

            firstLaunchTutorialPendingMeleeImpactResolved = true;
            TutorialEnemyActor target =
                firstLaunchTutorialPendingMeleeTarget;
            if (target == null)
                return;

            Vector2 delta = target != null
                ? target.Position - firstLaunchTutorialPlayerWorldPosition
                : Vector2.zero;
            bool hit =
                target != null &&
                target.Active &&
                !target.Dead &&
                delta.magnitude <= firstLaunchTutorialPendingMeleeRange &&
                Vector2.Dot(
                    delta.normalized,
                    firstLaunchTutorialPendingMeleeDirection
                   ) >= 0.35f;
            firstLaunchTutorialPendingMeleeHitResolved = hit;
            if (hit)
            {
                ApplyFirstLaunchTutorialActorDamage(
                    target,
                    firstLaunchTutorialPendingMeleeDamage
                );
                if (firstLaunchTutorialPendingMeleeHeavy &&
                    target.Role != TutorialEnemyRole.MiniBoss)
                {
                    target.Position.x +=
                        firstLaunchTutorialPendingMeleeDirection.x * 72f;
                }
                SetFirstLaunchTutorialLearningState(
                    firstLaunchTutorialPendingMeleeHeavy
                        ? "Heavy"
                        : "Light",
                    TutorialLearningState.Demonstrated
                );
            }
            else
            {
                ShowFirstLaunchTutorialSuccess("ATTACK MISSED");
            }

            firstLaunchTutorialPendingMeleeTarget = null;
            firstLaunchTutorialPendingMeleeDamage = 0f;
        }

        private void ResolveFirstLaunchTutorialRangedProjectileImpact()
        {
            if (firstLaunchTutorialPendingShotImpactResolved)
                return;

            firstLaunchTutorialPendingShotImpactResolved = true;
            TutorialEnemyActor target = firstLaunchTutorialPendingShotTarget;
            Vector2 impactWorld =
                firstLaunchTutorialActionTargetWorld -
                new Vector2(0f, 18f);
            bool hitLivingTarget =
                target != null &&
                target.Active &&
                !target.Dead &&
                Vector2.Distance(target.Position, impactWorld) <=
                    TutorialProjectileHitRadius;
            firstLaunchTutorialPendingShotHitResolved = hitLivingTarget;
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
            else if (hitLivingTarget &&
                     firstLaunchTutorialPendingShotCharged &&
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
                    ResolveFirstLaunchTutorialActionDirection() * 360f;

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
                    ResolveFirstLaunchTutorialActionDirection() * 420f;
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
            float requestedX,
            bool includeStaticObstacles = true)
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
                if (!actor.Active || actor.Dead || actor.Image == null ||
                    !actor.Image.gameObject.activeInHierarchy)
                {
                    continue;
                }

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

            if (!includeStaticObstacles)
                return resolvedX;

            if (firstLaunchTutorialGrounded &&
                firstLaunchTutorialJumpObstacle != null &&
                firstLaunchTutorialJumpObstacle.gameObject.activeSelf)
            {
                resolvedX = ResolveFirstLaunchTutorialSolidCollisionX(
                    currentX,
                    resolvedX,
                    TutorialJumpObstacleX,
                    34f,
                    playerRadius
                );
            }
            if (firstLaunchTutorialHazard != null &&
                firstLaunchTutorialHazard.gameObject.activeSelf)
            {
                resolvedX = ResolveFirstLaunchTutorialSolidCollisionX(
                    currentX,
                    resolvedX,
                    firstLaunchTutorialHazardWorldPosition.x,
                    48f,
                    playerRadius
                );
            }
            if (firstLaunchTutorialGap != null &&
                firstLaunchTutorialGap.gameObject.activeSelf)
            {
                resolvedX = ResolveFirstLaunchTutorialSolidCollisionX(
                    currentX,
                    resolvedX,
                    firstLaunchTutorialGapWorldPosition.x,
                    90f,
                    playerRadius
                );
            }
            if (!firstLaunchTutorialFinishGateOpen &&
                firstLaunchTutorialFinishGate != null &&
                firstLaunchTutorialFinishGate.gameObject.activeSelf)
            {
                resolvedX = ResolveFirstLaunchTutorialSolidCollisionX(
                    currentX,
                    resolvedX,
                    TutorialFinishX,
                    34f,
                    playerRadius
                );
            }
            return resolvedX;
        }

        private static float ResolveFirstLaunchTutorialSolidCollisionX(
            float currentX,
            float requestedX,
            float solidX,
            float solidRadius,
            float playerRadius)
        {
            float separation = solidRadius + playerRadius;
            if (currentX <= solidX &&
                requestedX > solidX - separation)
            {
                return Mathf.Min(requestedX, solidX - separation);
            }
            if (currentX >= solidX &&
                requestedX < solidX + separation)
            {
                return Mathf.Max(requestedX, solidX + separation);
            }
            return requestedX;
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
                {
                    return boss.UsesProjectileAttack
                        ? "RANGED WINDUP — MOVE"
                        : "SLAM WINDUP — DODGE";
                }
                return boss.UsesProjectileAttack
                    ? "SHOT RELEASE — PARRY / MOVE"
                    : "SLAM IMPACT — DO NOT TRADE";
            }
            if (Time.unscaledTime < boss.NextActionAt)
                return "RECOVERY — ATTACK NOW";
            return "READY — WATCH THE TELEGRAPH";
        }
    }
}
