using UnityEngine;

namespace BoredomAndDungeons
{
    public sealed partial class BDModernHandheld3DPresenter
    {
        // BD GRAPPLE FOLLOW-UP KILL V10.11.25.1
        private bool firstLaunchTutorialGrappleFinishPendingV101125;
        private TutorialEnemyActor
            firstLaunchTutorialGrappleFinishTargetV101125;

        // BD LESSON DAMAGE OWNERSHIP V10.11.25
        // Every unlocked mechanic may still execute. During a focused lesson,
        // only the authored mechanic may damage or kill its lesson targets.
        private enum FirstLaunchTutorialDamageSourceV101125
        {
            None,
            Light,
            Heavy,
            Spin,
            Grapple,
            Hazard,
            Ranged,
            Charged,
            MountedImpact
        }

        private FirstLaunchTutorialDamageSourceV101125
            firstLaunchTutorialDamageSourceV101125;
        private float firstLaunchTutorialWrongDamageFeedbackAtV101125;
        private bool firstLaunchTutorialLessonHitConfirmedV101125;

        private bool IsFirstLaunchTutorialLessonDamageAllowedV101125(
            TutorialEnemyActor actor,
            FirstLaunchTutorialDamageSourceV101125 source)
        {
            if (actor == null || actor.Role == TutorialEnemyRole.MiniBoss)
                return true;

            switch (firstLaunchTutorialStep)
            {
                case FirstLaunchTutorialStep.AttackEnemy:
                case FirstLaunchTutorialStep.JumpAttack:
                    return source ==
                        FirstLaunchTutorialDamageSourceV101125.Light;
                case FirstLaunchTutorialStep.HeavyAttack:
                    return source ==
                        FirstLaunchTutorialDamageSourceV101125.Heavy;
                case FirstLaunchTutorialStep.SpinAttack:
                    return source ==
                        FirstLaunchTutorialDamageSourceV101125.Spin;
                case FirstLaunchTutorialStep.Grapple:
                    if (!firstLaunchTutorialGrappleFinishPendingV101125)
                    {
                        return source ==
                            FirstLaunchTutorialDamageSourceV101125.Grapple;
                    }
                    return actor ==
                               firstLaunchTutorialGrappleFinishTargetV101125 &&
                           (source ==
                                FirstLaunchTutorialDamageSourceV101125.Light ||
                            source ==
                                FirstLaunchTutorialDamageSourceV101125.Heavy ||
                            source ==
                                FirstLaunchTutorialDamageSourceV101125.Spin ||
                            source ==
                                FirstLaunchTutorialDamageSourceV101125.Ranged ||
                            source ==
                                FirstLaunchTutorialDamageSourceV101125.Charged);
                case FirstLaunchTutorialStep.HazardKnockback:
                    return source ==
                        FirstLaunchTutorialDamageSourceV101125.Hazard;
                case FirstLaunchTutorialStep.RangedAttack:
                    return source ==
                        FirstLaunchTutorialDamageSourceV101125.Ranged;
                case FirstLaunchTutorialStep.ChargedShot:
                    return source ==
                        FirstLaunchTutorialDamageSourceV101125.Charged;
                case FirstLaunchTutorialStep.MountedImpact:
                    return source ==
                        FirstLaunchTutorialDamageSourceV101125.MountedImpact;
                default:
                    return true;
            }
        }

        private bool TryApplyFirstLaunchTutorialLessonDamageV101125(
            TutorialEnemyActor actor,
            float damage,
            FirstLaunchTutorialDamageSourceV101125 source)
        {
            if (actor == null || actor.Dead || damage <= 0f)
                return false;

            if (!IsFirstLaunchTutorialLessonDamageAllowedV101125(actor, source))
            {
                NotifyFirstLaunchTutorialWrongDamageSourceV101125();
                return false;
            }

            bool grapplePull =
                firstLaunchTutorialStep == FirstLaunchTutorialStep.Grapple &&
                !firstLaunchTutorialGrappleFinishPendingV101125 &&
                source == FirstLaunchTutorialDamageSourceV101125.Grapple;

            // Spin lesson death is owned only by the atomic pair resolver.
            if (firstLaunchTutorialStep == FirstLaunchTutorialStep.SpinAttack &&
                source == FirstLaunchTutorialDamageSourceV101125.Spin)
            {
                return false;
            }

            bool focusedKill = false;
            if (actor.Role != TutorialEnemyRole.MiniBoss)
            {
                switch (firstLaunchTutorialStep)
                {
                    case FirstLaunchTutorialStep.AttackEnemy:
                    case FirstLaunchTutorialStep.JumpAttack:
                        focusedKill = source ==
                            FirstLaunchTutorialDamageSourceV101125.Light;
                        break;
                    case FirstLaunchTutorialStep.HeavyAttack:
                        focusedKill = source ==
                            FirstLaunchTutorialDamageSourceV101125.Heavy;
                        break;
                    case FirstLaunchTutorialStep.HazardKnockback:
                        focusedKill = source ==
                            FirstLaunchTutorialDamageSourceV101125.Hazard;
                        break;
                    case FirstLaunchTutorialStep.RangedAttack:
                        focusedKill = source ==
                            FirstLaunchTutorialDamageSourceV101125.Ranged;
                        break;
                    case FirstLaunchTutorialStep.ChargedShot:
                        focusedKill = source ==
                            FirstLaunchTutorialDamageSourceV101125.Charged;
                        break;
                    case FirstLaunchTutorialStep.MountedImpact:
                        focusedKill = source ==
                            FirstLaunchTutorialDamageSourceV101125.MountedImpact;
                        break;
                    case FirstLaunchTutorialStep.Grapple:
                        focusedKill =
                            firstLaunchTutorialGrappleFinishPendingV101125 &&
                            actor == firstLaunchTutorialGrappleFinishTargetV101125 &&
                            source != FirstLaunchTutorialDamageSourceV101125.Grapple;
                        break;
                }
            }

            if (grapplePull)
            {
                float healthBefore = actor.Health;
                FirstLaunchTutorialDamageSourceV101125 previous =
                    firstLaunchTutorialDamageSourceV101125;
                firstLaunchTutorialDamageSourceV101125 = source;
                try
                {
                    float nonLethal = actor.Health > 1f
                        ? Mathf.Min(Mathf.Max(0.01f, damage), actor.Health - 1f)
                        : 0f;
                    if (nonLethal > 0f)
                        ApplyFirstLaunchTutorialActorDamage(actor, nonLethal);
                }
                finally
                {
                    firstLaunchTutorialDamageSourceV101125 = previous;
                }
                if (actor.Dead || actor.Health <= 0f)
                {
                    actor.Health = 1f;
                    actor.Dead = false;
                    actor.Active = true;
                    if (actor.Image != null)
                        actor.Image.gameObject.SetActive(true);
                }
                firstLaunchTutorialLessonHitConfirmedV101125 = true;
                return actor.Active && !actor.Dead && actor.Health > 0f;
            }

            // BD CORRECT LESSON HIT IS ATOMICALLY LETHAL V10.11.28
            // Kill state and both actor/image representations change at impact,
            // not later at animation completion.
            if (focusedKill)
            {
                ForceFirstLaunchTutorialLessonActorDeathV101128(actor);
                firstLaunchTutorialLessonHitConfirmedV101125 = true;

                if (firstLaunchTutorialStep == FirstLaunchTutorialStep.Grapple &&
                    firstLaunchTutorialGrappleFinishPendingV101125 &&
                    actor == firstLaunchTutorialGrappleFinishTargetV101125)
                {
                    firstLaunchTutorialGrappleFinishPendingV101125 = false;
                    firstLaunchTutorialGrappleFinishTargetV101125 = null;
                    SetFirstLaunchTutorialLearningState(
                        "Grapple",
                        TutorialLearningState.Demonstrated
                    );
                    ShowFirstLaunchTutorialSuccess("PULLED ENEMY DEFEATED");
                    SetFirstLaunchTutorialStep(
                        FirstLaunchTutorialStep.HazardKnockback
                    );
                }
                return actor.Dead && !actor.Active;
            }

            float healthBeforeGeneric = actor.Health;
            FirstLaunchTutorialDamageSourceV101125 previousGeneric =
                firstLaunchTutorialDamageSourceV101125;
            firstLaunchTutorialDamageSourceV101125 = source;
            try
            {
                ApplyFirstLaunchTutorialActorDamage(actor, damage);
            }
            finally
            {
                firstLaunchTutorialDamageSourceV101125 = previousGeneric;
            }
            bool damaged = actor.Dead || actor.Health < healthBeforeGeneric;
            if (damaged)
                firstLaunchTutorialLessonHitConfirmedV101125 = true;
            return damaged;
        }

        private void RetireFirstLaunchTutorialPrimaryLessonTargetV101125()
        {
            for (int index = 0;
                 index < firstLaunchTutorialActors.Count;
                 index++)
            {
                TutorialEnemyActor actor = firstLaunchTutorialActors[index];
                if (actor == null ||
                    actor.Image != firstLaunchTutorialEnemy ||
                    actor.Role == TutorialEnemyRole.MiniBoss)
                {
                    continue;
                }
                actor.Health = 0f;
                actor.Dead = true;
                actor.Active = false;
                if (actor.Image != null)
                    actor.Image.gameObject.SetActive(false);
            }
            if (firstLaunchTutorialEnemy != null)
                firstLaunchTutorialEnemy.gameObject.SetActive(false);
        }

        private void NotifyFirstLaunchTutorialWrongDamageSourceV101125()
        {
            if (Time.unscaledTime <
                    firstLaunchTutorialWrongDamageFeedbackAtV101125)
            {
                return;
            }
            firstLaunchTutorialWrongDamageFeedbackAtV101125 =
                Time.unscaledTime + 0.75f;

            string message;
            switch (firstLaunchTutorialStep)
            {
                case FirstLaunchTutorialStep.AttackEnemy:
                case FirstLaunchTutorialStep.JumpAttack:
                    message = "HIT THE TARGET WITH THE QUICK ATTACK";
                    break;
                case FirstLaunchTutorialStep.HeavyAttack:
                    message = "HIT THE TARGET WITH THE HEAVY ATTACK";
                    break;
                case FirstLaunchTutorialStep.SpinAttack:
                    message = "HIT THE TARGET WITH THE SPIN ATTACK";
                    break;
                case FirstLaunchTutorialStep.Grapple:
                    message = "COMPLETE THE GRAPPLE ON THE TARGET";
                    break;
                case FirstLaunchTutorialStep.HazardKnockback:
                    message = "KNOCK THE TARGET INTO THE HAZARD";
                    break;
                case FirstLaunchTutorialStep.RangedAttack:
                    message = "HIT THE TARGET WITH THE MOUNTED SHOT";
                    break;
                case FirstLaunchTutorialStep.ChargedShot:
                    message = "HIT THE TARGET WITH THE CHARGED SHOT";
                    break;
                case FirstLaunchTutorialStep.MountedImpact:
                    message = "RAM THE TARGET WITH THE HORSE";
                    break;
                default:
                    message = "COMPLETE THE CURRENT LESSON";
                    break;
            }
            ShowFirstLaunchTutorialSuccess(message);
        }

        // BD MOUNTED RANGED SEQUENCE BARRIER V10.11.24
        // Projectile impact and reload completion are independent events. The
        // lesson advances only after both confirmations, in either order.
        private bool firstLaunchTutorialMountedShotImpactConfirmedV101124;
        private bool firstLaunchTutorialMountedShotReloadConfirmedV101124;
        private bool firstLaunchTutorialChargedShotImpactConfirmedV101124;
        private bool firstLaunchTutorialChargedShotReloadConfirmedV101124;

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
            firstLaunchTutorialMountedShotImpactConfirmedV101124 = false;
            firstLaunchTutorialMountedShotReloadConfirmedV101124 = false;
            firstLaunchTutorialChargedShotImpactConfirmedV101124 = false;
            firstLaunchTutorialChargedShotReloadConfirmedV101124 = false;
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

        private void ResetFirstLaunchTutorialMountedRangedSequenceForStepV101124(
            FirstLaunchTutorialStep step)
        {
            if (step == FirstLaunchTutorialStep.RangedAttack)
            {
                firstLaunchTutorialMountedShotImpactConfirmedV101124 = false;
                firstLaunchTutorialMountedShotReloadConfirmedV101124 = false;
                firstLaunchTutorialChargedShotImpactConfirmedV101124 = false;
                firstLaunchTutorialChargedShotReloadConfirmedV101124 = false;
                firstLaunchTutorialChargedShotAutoFired = false;
                ResetFirstLaunchTutorialPendingShotForRetryV101124();
                return;
            }

            // Reload is a continuation of the ordinary mounted-shot lesson.
            // Preserve both confirmations across RangedAttack -> Reload.
            if (step == FirstLaunchTutorialStep.Reload)
                return;

            if (step == FirstLaunchTutorialStep.ChargedShot)
            {
                firstLaunchTutorialChargedShotImpactConfirmedV101124 = false;
                firstLaunchTutorialChargedShotReloadConfirmedV101124 = false;
                firstLaunchTutorialChargedShotAutoFired = false;
                ResetFirstLaunchTutorialPendingShotForRetryV101124();
                return;
            }

            if (step != FirstLaunchTutorialStep.MountedImpact)
            {
                firstLaunchTutorialMountedShotImpactConfirmedV101124 = false;
                firstLaunchTutorialMountedShotReloadConfirmedV101124 = false;
                firstLaunchTutorialChargedShotImpactConfirmedV101124 = false;
                firstLaunchTutorialChargedShotReloadConfirmedV101124 = false;
            }
        }

        private void ResetFirstLaunchTutorialPendingShotForRetryV101124()
        {
            firstLaunchTutorialPendingShotTarget = null;
            firstLaunchTutorialPendingShotDamage = 0f;
            firstLaunchTutorialPendingShotStep = firstLaunchTutorialStep;
            firstLaunchTutorialPendingShotCharged = false;
            firstLaunchTutorialPendingShotAdvancesLesson = false;
            firstLaunchTutorialPendingShotImpactResolved = false;
            firstLaunchTutorialPendingShotHitResolved = false;
            firstLaunchTutorialChargedShotPendingStartedAt = -1f;
            firstLaunchTutorialChargedShotStartedAt = -1f;
        }

        private void UpdateFirstLaunchTutorialMountedRangedSequenceV101124()
        {
            if (firstLaunchTutorialStep ==
                    FirstLaunchTutorialStep.RangedAttack)
            {
                bool ordinaryImpactResolved =
                    firstLaunchTutorialPendingShotStep ==
                        FirstLaunchTutorialStep.RangedAttack &&
                    firstLaunchTutorialPendingShotImpactResolved;
                if (!ordinaryImpactResolved)
                    return;

                if (firstLaunchTutorialPendingShotHitResolved &&
                    firstLaunchTutorialMountedShotImpactConfirmedV101124)
                {
                    SetFirstLaunchTutorialStep(
                        FirstLaunchTutorialStep.Reload
                    );
                    return;
                }

                if (firstLaunchTutorialMountedShotReloadConfirmedV101124)
                {
                    // A miss must be retryable even if reload completed first.
                    firstLaunchTutorialMountedShotReloadConfirmedV101124 = false;
                    firstLaunchTutorialAmmo = 1;
                    firstLaunchTutorialReloadCompletesAt = 0f;
                    ResetFirstLaunchTutorialPendingShotForRetryV101124();
                    SpawnTutorialActor(
                        firstLaunchTutorialEnemy,
                        TutorialEnemyRole.Small,
                        TutorialMountedStationX + 130f,
                        1f
                    );
                    ShowFirstLaunchTutorialSuccess(
                        "SHOT MISSED — AIM AND FIRE AGAIN"
                    );
                }
                return;
            }

            if (firstLaunchTutorialStep == FirstLaunchTutorialStep.Reload)
            {
                // Keep the reload lesson readable for a short beat even when
                // the physical reload finished before projectile impact.
                if (firstLaunchTutorialMountedShotImpactConfirmedV101124 &&
                    firstLaunchTutorialMountedShotReloadConfirmedV101124 &&
                    Time.unscaledTime - firstLaunchTutorialStepStartedAt >=
                        0.30f)
                {
                    SetFirstLaunchTutorialStep(
                        FirstLaunchTutorialStep.ChargedShot
                    );
                }
                return;
            }

            if (firstLaunchTutorialStep !=
                    FirstLaunchTutorialStep.ChargedShot ||
                !firstLaunchTutorialChargedShotAutoFired)
            {
                return;
            }

            bool chargedImpactResolved =
                firstLaunchTutorialPendingShotStep ==
                    FirstLaunchTutorialStep.ChargedShot &&
                firstLaunchTutorialPendingShotImpactResolved;
            if (!chargedImpactResolved ||
                !firstLaunchTutorialChargedShotReloadConfirmedV101124)
            {
                return;
            }

            if (firstLaunchTutorialPendingShotHitResolved &&
                firstLaunchTutorialChargedShotImpactConfirmedV101124)
            {
                SetFirstLaunchTutorialLearningState(
                    "ChargedShot",
                    TutorialLearningState.Demonstrated
                );
                ShowFirstLaunchTutorialSuccess(
                    "CHARGED IMPACT — RIDE INTO THE TARGET"
                );
                SetFirstLaunchTutorialStep(
                    FirstLaunchTutorialStep.MountedImpact
                );
                return;
            }

            // Auto-fire completed but the projectile missed. Restore a complete
            // deterministic lesson state rather than leaving auto-fire latched.
            firstLaunchTutorialChargedShotAutoFired = false;
            firstLaunchTutorialChargedShotImpactConfirmedV101124 = false;
            firstLaunchTutorialChargedShotReloadConfirmedV101124 = false;
            firstLaunchTutorialAmmo = TutorialMagazineSize;
            firstLaunchTutorialReloadCompletesAt = 0f;
            ResetFirstLaunchTutorialPendingShotForRetryV101124();
            SpawnTutorialActor(
                firstLaunchTutorialEnemy,
                TutorialEnemyRole.Small,
                TutorialMountedStationX + 260f,
                4f
            );
            ShowFirstLaunchTutorialSuccess(
                "CHARGED SHOT MISSED — HOLD AND AIM AGAIN"
            );
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
                    // BD FOCUSED ATTACK DIRECTION V10.11.30
            firstLaunchTutorialPendingMeleeDirection =
                ResolveFirstLaunchTutorialFocusedAttackDirectionV101130(
                    target,
                    firstLaunchTutorialPendingMeleeDirection
                );
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

            Vector2 delta =
                target.Position - firstLaunchTutorialPlayerWorldPosition;
            bool hit = target.Active &&
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
                    !target.Dead &&
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
                if (target.Dead)
                    HandleFirstLaunchTutorialMeleeLessonDeathAtImpact(target);
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
            bool hitLivingTarget = target != null &&
                target.Active &&
                !target.Dead &&
                Vector2.Distance(target.Position, impactWorld) <=
                    TutorialProjectileHitRadius;
            if (hitLivingTarget)
            {
                ApplyFirstLaunchTutorialActorDamage(
                    target,
                    firstLaunchTutorialPendingShotDamage
                );
            }

            bool objectiveKill = hitLivingTarget && target.Dead;
            firstLaunchTutorialPendingShotHitResolved = objectiveKill;

            if (objectiveKill &&
                firstLaunchTutorialPendingShotAdvancesLesson &&
                firstLaunchTutorialPendingShotStep ==
                    FirstLaunchTutorialStep.RangedAttack)
            {
                CompleteFirstLaunchTutorialMountedShotLessonAtImpact();
            }
            else if (objectiveKill &&
                     firstLaunchTutorialPendingShotCharged &&
                     firstLaunchTutorialPendingShotStep ==
                         FirstLaunchTutorialStep.ChargedShot)
            {
                ShowFirstLaunchTutorialSuccess("CHARGED TARGET DESTROYED");
                SetFirstLaunchTutorialStep(
                    FirstLaunchTutorialStep.MountedImpact
                );
            }
            else if (hitLivingTarget && !objectiveKill)
            {
                ShowFirstLaunchTutorialSuccess(
                    "THE TARGET SURVIVED — HIT IT AGAIN"
                );
            }

            firstLaunchTutorialPendingShotTarget = null;
            firstLaunchTutorialPendingShotDamage = 0f;
        }


        private void CompleteFirstLaunchTutorialMountedShotLessonAtImpact()
        {
            if (firstLaunchTutorialPendingShotStep !=
                    FirstLaunchTutorialStep.RangedAttack)
            {
                return;
            }

            firstLaunchTutorialMountedShotImpactConfirmedV101124 = true;
            SetFirstLaunchTutorialLearningState(
                "MountedShot",
                TutorialLearningState.Demonstrated
            );
            ShowFirstLaunchTutorialSuccess("PROJECTILE IMPACT");
            UpdateFirstLaunchTutorialMountedRangedSequenceV101124();
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
            TutorialEnemyActor target =
                firstLaunchTutorialPendingHookTarget;
            if (target != null && target.Active && !target.Dead)
            {
                bool connected =
                    TryApplyFirstLaunchTutorialLessonDamageV101125(
                        target,
                        firstLaunchTutorialPendingHookDamage,
                        FirstLaunchTutorialDamageSourceV101125.Grapple
                    );
                if (connected &&
                    firstLaunchTutorialStep ==
                        FirstLaunchTutorialStep.Grapple &&
                    !target.Dead)
                {
                    // BD GRAPPLE FOLLOW-UP KILL V10.11.25.1
                    firstLaunchTutorialGrappleFinishPendingV101125 = true;
                    firstLaunchTutorialGrappleFinishTargetV101125 = target;
                    target.Health = Mathf.Max(target.Health, 1f);
                    target.Active = true;
                    target.Dead = false;
                    if (target.Image != null)
                        target.Image.gameObject.SetActive(true);
                }
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

            if (firstLaunchTutorialLessonCompleteAwaitingTravel ||
                firstLaunchTutorialLessonScreenTransitionActive)
            {
                return requestedX;
            }

            if (firstLaunchTutorialMounted &&
                firstLaunchTutorialStep ==
                    FirstLaunchTutorialStep.MountedImpact)
            {
                return requestedX;
            }

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

                // The parry target must never be an invisible wall. The lesson
                // is completed by timing the parry, not by body blocking.
                if (firstLaunchTutorialStep ==
                        FirstLaunchTutorialStep.Parry &&
                    actor.Image == firstLaunchTutorialEnemy)
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
                    resolvedX = Mathf.Min(resolvedX, actorX - separation);
                }
                else if (crossingFromRight ||
                         (alreadyOverlapping && currentX > actorX))
                {
                    resolvedX = Mathf.Max(resolvedX, actorX + separation);
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
