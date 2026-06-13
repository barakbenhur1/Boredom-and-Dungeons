using UnityEngine;

namespace BoredomAndDungeons
{
    public sealed partial class BDModernHandheld3DPresenter
    {
        // BD BOSS INTRO FREEZE OWNER V10.11.30.42
        private bool firstLaunchTutorialBossIntroFreezeInitializedV1011342;
        private Vector2 firstLaunchTutorialBossIntroPlayerAnchorV1011342;
        private Vector2 firstLaunchTutorialBossIntroBossAnchorV1011342;

        private void ResetFirstLaunchTutorialBossIntroFreezeV1011342(
            FirstLaunchTutorialStep step)
        {
            if (step == FirstLaunchTutorialStep.MiniBossIntro)
            {
                firstLaunchTutorialBossIntroFreezeInitializedV1011342 = false;
                return;
            }

            if (step != FirstLaunchTutorialStep.MiniBossPhaseOne &&
                step != FirstLaunchTutorialStep.MiniBossPhaseTwo)
            {
                firstLaunchTutorialBossIntroFreezeInitializedV1011342 = false;
            }
        }

        private bool MaintainFirstLaunchTutorialBossIntroFreezeV1011342()
        {
            if (firstLaunchTutorialStep !=
                    FirstLaunchTutorialStep.MiniBossIntro)
            {
                return false;
            }

            TutorialEnemyActor boss = FindTutorialBoss();
            if (!firstLaunchTutorialBossIntroFreezeInitializedV1011342)
            {
                firstLaunchTutorialBossIntroPlayerAnchorV1011342 =
                    firstLaunchTutorialPlayerWorldPosition;
                firstLaunchTutorialBossIntroBossAnchorV1011342 =
                    new Vector2(
                        firstLaunchTutorialLessonScreenCenterX + 80f,
                        TutorialGroundY
                    );
                firstLaunchTutorialBossIntroFreezeInitializedV1011342 = true;

                ResetFirstLaunchTutorialV108Transactions();
                ResetFirstLaunchTutorialBossChargeState();
                CancelFirstLaunchTutorialEnemyProjectile();

                firstLaunchTutorialActionPresentationType =
                    FirstLaunchTutorialActionPresentationType.None;
                firstLaunchTutorialActionAdvancesLesson = false;
                SetFirstLaunchTutorialActionEffectsVisible(false);
            }

            firstLaunchTutorialPlayerWorldPosition =
                firstLaunchTutorialBossIntroPlayerAnchorV1011342;
            firstLaunchTutorialCheckpointX =
                firstLaunchTutorialBossIntroPlayerAnchorV1011342.x;
            firstLaunchTutorialVerticalVelocity = 0f;
            firstLaunchTutorialGrounded = true;
            firstLaunchTutorialGroundedY = TutorialGroundY;
            firstLaunchTutorialMovementActive = false;
            firstLaunchTutorialMounted = false;
            firstLaunchTutorialMountedCurrentSpeed = 0f;
            firstLaunchTutorialPhysicalMoveLatch = Vector2.zero;
            firstLaunchTutorialPhysicalMoveLatchUntil = 0f;
            firstLaunchTutorialPrimaryHoldStartedAt = -1f;
            firstLaunchTutorialGrappleHoldStartedAt = -1f;
            firstLaunchTutorialChargedShotPendingStartedAt = -1f;
            firstLaunchTutorialChargedShotStartedAt = -1f;
            firstLaunchTutorialProductionLightResolved = false;
            firstLaunchTutorialProductionHeavyResolved = false;
            firstLaunchTutorialMiniBossDeathStarted = false;
            firstLaunchTutorialMiniBossPhaseTwo = false;
            firstLaunchTutorialPlayerDeathActive = false;
            firstLaunchTutorialPlayerHealth =
                Mathf.Max(1, firstLaunchTutorialPlayerHealth);

            RestoreFirstLaunchTutorialBossForFrozenIntroV1011342(boss);

            firstLaunchTutorialParryProjectileActive = false;
            if (firstLaunchTutorialProjectile != null)
                firstLaunchTutorialProjectile.gameObject.SetActive(false);
            if (firstLaunchTutorialBossAttackTelegraph != null)
                firstLaunchTutorialBossAttackTelegraph.gameObject.SetActive(false);
            if (firstLaunchTutorialBossImpactFlash != null)
                firstLaunchTutorialBossImpactFlash.gameObject.SetActive(false);

            SetFirstLaunchTutorialInstructionRequested(true);
            UpdateFirstLaunchTutorialProductionHud();
            return true;
        }

        private void RestoreFirstLaunchTutorialBossForFrozenIntroV1011342(
            TutorialEnemyActor boss)
        {
            if (boss == null)
                return;

            boss.Role = TutorialEnemyRole.MiniBoss;
            boss.Position = firstLaunchTutorialBossIntroBossAnchorV1011342;
            boss.SpawnPosition = boss.Position;
            boss.MaximumHealth = Mathf.Max(12f, boss.MaximumHealth);
            boss.Health = boss.MaximumHealth;
            boss.Active = true;
            boss.Dead = false;
            boss.AttackCommitted = false;
            boss.DamageApplied = false;
            boss.UsesProjectileAttack = false;
            boss.AttackSequence = 0;
            boss.NextActionAt = float.PositiveInfinity;

            if (boss.Image != null)
            {
                boss.Image.gameObject.SetActive(true);
                boss.Image.rectTransform.localScale = Vector3.one;
                boss.Image.rectTransform.localRotation = Quaternion.identity;
            }
        }

        private void ApplyFirstLaunchTutorialBossIntroFrozenVisualsV1011342()
        {
            if (firstLaunchTutorialStep !=
                    FirstLaunchTutorialStep.MiniBossIntro)
            {
                return;
            }

            if (firstLaunchTutorialPlayer != null)
            {
                firstLaunchTutorialPlayer.rectTransform.anchoredPosition =
                    SnapFirstLaunchTutorialWorldPosition(
                        firstLaunchTutorialBossIntroPlayerAnchorV1011342
                    );
                firstLaunchTutorialPlayer.rectTransform.localScale =
                    Vector3.one;
                firstLaunchTutorialPlayer.rectTransform.localRotation =
                    Quaternion.identity;
            }

            TutorialEnemyActor boss = FindTutorialBoss();
            if (boss != null && boss.Image != null)
            {
                boss.Image.rectTransform.anchoredPosition =
                    SnapFirstLaunchTutorialWorldPosition(
                        firstLaunchTutorialBossIntroBossAnchorV1011342
                    );
                boss.Image.rectTransform.localScale = Vector3.one;
                boss.Image.rectTransform.localRotation =
                    Quaternion.identity;
            }
        }

        private void ReleaseFirstLaunchTutorialBossIntroFreezeV1011342()
        {
            TutorialEnemyActor boss = FindTutorialBoss();
            RestoreFirstLaunchTutorialBossForFrozenIntroV1011342(boss);

            if (boss != null)
            {
                boss.NextActionAt = Time.unscaledTime + 0.80f;
                boss.UsesProjectileAttack = false;
            }

            firstLaunchTutorialMiniBossPhaseStartedAt = Time.unscaledTime;
            firstLaunchTutorialNextEnemyAttackAt =
                Time.unscaledTime + 0.80f;
            firstLaunchTutorialBossIntroFreezeInitializedV1011342 = false;
        }
    }
}
