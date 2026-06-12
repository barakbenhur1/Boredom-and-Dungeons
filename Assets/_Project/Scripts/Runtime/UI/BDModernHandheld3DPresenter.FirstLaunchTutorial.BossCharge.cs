using UnityEngine;

namespace BoredomAndDungeons
{
    public sealed partial class BDModernHandheld3DPresenter
    {
        private const float TutorialBossChargeHoldThresholdSeconds = 0.22f;
        private const float TutorialBossChargeDurationSeconds = 1.18f;
        private const float TutorialBossChargedShotDamage = 3f;
        private const int TutorialBossChargedShotAmmoCost = 2;

        private float firstLaunchTutorialBossChargePendingStartedAt = -1f;
        private float firstLaunchTutorialBossChargeStartedAt = -1f;
        private bool firstLaunchTutorialBossChargeAutoFired;
        private bool firstLaunchTutorialBossChargeNeedsRelease;

        private bool IsFirstLaunchTutorialBossCombatStep()
        {
            return firstLaunchTutorialStep ==
                       FirstLaunchTutorialStep.MiniBossPhaseOne ||
                   firstLaunchTutorialStep ==
                       FirstLaunchTutorialStep.MiniBossPhaseTwo;
        }

        private void ResetFirstLaunchTutorialBossChargeState()
        {
            firstLaunchTutorialBossChargePendingStartedAt = -1f;
            firstLaunchTutorialBossChargeStartedAt = -1f;
            firstLaunchTutorialBossChargeAutoFired = false;
            firstLaunchTutorialBossChargeNeedsRelease = false;
            ShowFirstLaunchTutorialHoldProgress(
                string.Empty,
                0f,
                visible: false
            );
        }

        private void BeginFirstLaunchTutorialBossChargeInput()
        {
            if (!IsFirstLaunchTutorialBossCombatStep() ||
                firstLaunchTutorialBossChargeAutoFired ||
                firstLaunchTutorialBossChargeNeedsRelease ||
                Time.unscaledTime < firstLaunchTutorialReloadCompletesAt)
            {
                return;
            }

            if (firstLaunchTutorialAmmo < TutorialBossChargedShotAmmoCost)
            {
                BeginFirstLaunchTutorialReload();
                return;
            }

            if (firstLaunchTutorialBossChargePendingStartedAt < 0f &&
                firstLaunchTutorialBossChargeStartedAt < 0f)
            {
                firstLaunchTutorialBossChargePendingStartedAt =
                    Time.unscaledTime;
                ShowFirstLaunchTutorialSuccess("HOLD TO CHARGE");
            }
        }

        private void UpdateFirstLaunchTutorialBossChargeHold()
        {
            if (!IsFirstLaunchTutorialBossCombatStep())
            {
                ResetFirstLaunchTutorialBossChargeState();
                return;
            }

            if (firstLaunchTutorialBossChargeAutoFired)
            {
                bool presentationFinished =
                    firstLaunchTutorialActionPresentationType ==
                        FirstLaunchTutorialActionPresentationType.None;
                bool reloadFinished =
                    Time.unscaledTime >= firstLaunchTutorialReloadCompletesAt;
                if (presentationFinished && reloadFinished)
                    firstLaunchTutorialBossChargeAutoFired = false;
                return;
            }

            bool held = IsFirstLaunchTutorialRangedHeld();
            float now = Time.unscaledTime;

            if (firstLaunchTutorialBossChargeNeedsRelease)
            {
                if (!held)
                    firstLaunchTutorialBossChargeNeedsRelease = false;
                return;
            }

            if (firstLaunchTutorialBossChargePendingStartedAt < 0f &&
                firstLaunchTutorialBossChargeStartedAt < 0f)
            {
                if (held)
                    BeginFirstLaunchTutorialBossChargeInput();
                return;
            }

            if (firstLaunchTutorialBossChargeStartedAt < 0f)
            {
                float pendingElapsed =
                    now - firstLaunchTutorialBossChargePendingStartedAt;
                if (!held)
                {
                    FireFirstLaunchTutorialBossOrdinaryShot();
                    return;
                }

                ShowFirstLaunchTutorialHoldProgress(
                    "BOSS CHARGE",
                    pendingElapsed /
                        TutorialBossChargeHoldThresholdSeconds,
                    visible: true
                );
                if (pendingElapsed <
                        TutorialBossChargeHoldThresholdSeconds)
                {
                    return;
                }

                firstLaunchTutorialBossChargeStartedAt = now;
                firstLaunchTutorialBossChargePendingStartedAt = -1f;
            }

            float progress = Mathf.Clamp01(
                (now - firstLaunchTutorialBossChargeStartedAt) /
                TutorialBossChargeDurationSeconds
            );
            ShowFirstLaunchTutorialHoldProgress(
                progress >= 1f
                    ? "CHARGED — WAIT FOR RECOVERY"
                    : "BOSS CHARGED SHOT",
                progress,
                visible: true
            );

            if (progress < 1f)
            {
                if (!held)
                {
                    firstLaunchTutorialBossChargeStartedAt = -1f;
                    ShowFirstLaunchTutorialHoldProgress(
                        string.Empty,
                        0f,
                        visible: false
                    );
                    ShowFirstLaunchTutorialSuccess(
                        "CHARGE CANCELLED — KEEP HOLDING"
                    );
                }
                return;
            }

            TutorialEnemyActor boss = FindTutorialBoss();
            if (boss == null || boss.Dead || !boss.Active)
            {
                ResetFirstLaunchTutorialBossChargeState();
                return;
            }

            // Once full, the charge remains armed and auto-fires on the first
            // valid recovery frame. Releasing is not required.
            if (boss.AttackCommitted)
                return;

            FireFirstLaunchTutorialBossChargedShotAutomatically(boss);
        }


        private void FireFirstLaunchTutorialBossOrdinaryShot()
        {
            firstLaunchTutorialBossChargePendingStartedAt = -1f;
            firstLaunchTutorialBossChargeStartedAt = -1f;
            ShowFirstLaunchTutorialHoldProgress(
                string.Empty,
                0f,
                visible: false
            );

            TutorialEnemyActor boss = FindTutorialBoss();
            if (boss == null || boss.Dead || !boss.Active)
                return;
            if (firstLaunchTutorialAmmo <= 0)
            {
                BeginFirstLaunchTutorialReload();
                return;
            }

            firstLaunchTutorialAmmo = Mathf.Max(
                0,
                firstLaunchTutorialAmmo - 1
            );
            BeginFirstLaunchTutorialShotTransaction(
                boss,
                1f,
                charged: false,
                advancesLesson: false
            );
            PlayFirstLaunchTutorialRangedAttackAnimation(
                boss.Position,
                advancesLesson: false,
                chargedShot: false
            );
            ShowFirstLaunchTutorialSuccess("ORDINARY SHOT");
            if (firstLaunchTutorialAmmo <= 0)
                BeginFirstLaunchTutorialReload();
        }

        private void FireFirstLaunchTutorialBossChargedShotAutomatically(
            TutorialEnemyActor boss)
        {
            firstLaunchTutorialBossChargeAutoFired = true;
            firstLaunchTutorialBossChargeNeedsRelease = true;
            firstLaunchTutorialBossChargeStartedAt = -1f;
            firstLaunchTutorialBossChargePendingStartedAt = -1f;
            ShowFirstLaunchTutorialHoldProgress(
                string.Empty,
                0f,
                visible: false
            );

            firstLaunchTutorialAmmo = Mathf.Max(
                0,
                firstLaunchTutorialAmmo - TutorialBossChargedShotAmmoCost
            );
            BeginFirstLaunchTutorialShotTransaction(
                boss,
                TutorialBossChargedShotDamage,
                charged: true,
                advancesLesson: false
            );
            PlayFirstLaunchTutorialRangedAttackAnimation(
                boss.Position,
                advancesLesson: false,
                chargedShot: true
            );
            ShowFirstLaunchTutorialSuccess("CHARGED SHOT RELEASED");

            if (firstLaunchTutorialAmmo < TutorialBossChargedShotAmmoCost)
                BeginFirstLaunchTutorialReload();
        }
    }
}
