using System.Collections;
using UnityEngine;

namespace BoredomAndDungeons
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(BDBossEncounterController))]
    public sealed class BDBossEncounterRuntimeBindings : MonoBehaviour
    {
        [Header("Core References")]
        [SerializeField] private BDBossEncounterController encounter;
        [SerializeField] private BDBossHealthGroup healthGroup;
        [SerializeField] private BDBossRewardChest rewardChest;
        [SerializeField] private BDBossArenaBarrier[] barriers;
        [SerializeField] private BDBossArenaTrigger[] arenaTriggers;

        [Header("Boss Gameplay")]
        [Tooltip("AI/attack behaviours disabled during Intro, Transition and Victory.")]
        [SerializeField] private MonoBehaviour[] bossCombatBehaviours;

        [Header("Victory")]
        [SerializeField] private float victoryReleaseDelay = 0.35f;
        [SerializeField] private bool unlockRewardChestOnVictory = true;
        [SerializeField] private bool openRewardChestOnVictory = true;
        [SerializeField] private bool completeEncounterAfterRelease = true;

        [Header("Reset")]
        [SerializeField] private bool resetChestWhenDormant = true;
        [SerializeField] private bool openArenaWhenDormant = true;

        private Coroutine victoryRoutine;

        private void Awake()
        {
            ResolveReferences();
        }

        private void OnEnable()
        {
            ResolveReferences();

            if (encounter != null)
            {
                encounter.StateChanged -= HandleEncounterStateChanged;
                encounter.StateChanged += HandleEncounterStateChanged;
                ApplyState(encounter.State);
            }
        }

        private void OnDisable()
        {
            if (encounter != null)
                encounter.StateChanged -= HandleEncounterStateChanged;

            if (victoryRoutine != null)
            {
                StopCoroutine(victoryRoutine);
                victoryRoutine = null;
            }
        }

        public void RefreshReferences()
        {
            ResolveReferences(forceRefresh: true);
        }

        private void ResolveReferences(bool forceRefresh = false)
        {
            if (encounter == null || forceRefresh)
                encounter = GetComponent<BDBossEncounterController>();

            if (healthGroup == null || forceRefresh)
                healthGroup = GetComponent<BDBossHealthGroup>();

            if (rewardChest == null || forceRefresh)
            {
                rewardChest =
                    GetComponentInChildren<BDBossRewardChest>(
                        includeInactive: true
                    );
            }

            if (barriers == null || barriers.Length == 0 || forceRefresh)
            {
                barriers =
                    GetComponentsInChildren<BDBossArenaBarrier>(
                        includeInactive: true
                    );
            }

            if (arenaTriggers == null ||
                arenaTriggers.Length == 0 ||
                forceRefresh)
            {
                arenaTriggers =
                    GetComponentsInChildren<BDBossArenaTrigger>(
                        includeInactive: true
                    );
            }
        }

        private void HandleEncounterStateChanged(
            BDBossEncounterState nextState)
        {
            ApplyState(nextState);
        }

        private void ApplyState(BDBossEncounterState currentState)
        {
            switch (currentState)
            {
                case BDBossEncounterState.Dormant:
                    CancelVictoryRoutine();
                    SetBossCombatEnabled(false);

                    if (openArenaWhenDormant)
                        SetBarriersLocked(false, immediate: true);

                    ResetArenaTriggers();

                    if (resetChestWhenDormant && rewardChest != null)
                        rewardChest.ResetChest(lockChest: true);
                    break;

                case BDBossEncounterState.Intro:
                    CancelVictoryRoutine();
                    SetBarriersLocked(true, immediate: false);
                    SetBossCombatEnabled(false);
                    break;

                case BDBossEncounterState.Active:
                    SetBarriersLocked(true, immediate: true);
                    SetBossCombatEnabled(true);
                    break;

                case BDBossEncounterState.Transition:
                    SetBarriersLocked(true, immediate: true);
                    SetBossCombatEnabled(false);
                    break;

                case BDBossEncounterState.Victory:
                    SetBossCombatEnabled(false);
                    StartVictoryRoutine();
                    break;

                case BDBossEncounterState.Completed:
                    SetBossCombatEnabled(false);
                    SetBarriersLocked(false, immediate: true);
                    break;

                case BDBossEncounterState.Failed:
                    CancelVictoryRoutine();
                    SetBossCombatEnabled(false);
                    break;
            }
        }

        private void StartVictoryRoutine()
        {
            CancelVictoryRoutine();
            victoryRoutine = StartCoroutine(VictoryRoutine());
        }

        private IEnumerator VictoryRoutine()
        {
            float delay = Mathf.Max(0f, victoryReleaseDelay);

            if (delay > 0f)
                yield return new WaitForSeconds(delay);

            SetBarriersLocked(false, immediate: false);

            while (AnyBarrierAnimating())
                yield return null;

            if (rewardChest != null && unlockRewardChestOnVictory)
            {
                if (openRewardChestOnVictory)
                    rewardChest.UnlockAndOpen();
                else
                    rewardChest.Unlock();
            }

            if (completeEncounterAfterRelease &&
                encounter != null &&
                encounter.State == BDBossEncounterState.Victory)
            {
                encounter.CompleteEncounter();
            }

            victoryRoutine = null;
        }

        private void CancelVictoryRoutine()
        {
            if (victoryRoutine == null)
                return;

            StopCoroutine(victoryRoutine);
            victoryRoutine = null;
        }

        private void SetBossCombatEnabled(bool enabled)
        {
            if (bossCombatBehaviours == null)
                return;

            for (int i = 0; i < bossCombatBehaviours.Length; i++)
            {
                MonoBehaviour behaviour = bossCombatBehaviours[i];

                if (behaviour != null)
                    behaviour.enabled = enabled;
            }
        }

        private void SetBarriersLocked(bool locked, bool immediate)
        {
            if (barriers == null)
                return;

            for (int i = 0; i < barriers.Length; i++)
            {
                BDBossArenaBarrier barrier = barriers[i];

                if (barrier != null)
                    barrier.SetLocked(locked, immediate);
            }
        }

        private bool AnyBarrierAnimating()
        {
            if (barriers == null)
                return false;

            for (int i = 0; i < barriers.Length; i++)
            {
                BDBossArenaBarrier barrier = barriers[i];

                if (barrier != null && barrier.IsAnimating)
                    return true;
            }

            return false;
        }

        private void ResetArenaTriggers()
        {
            if (arenaTriggers == null)
                return;

            for (int i = 0; i < arenaTriggers.Length; i++)
            {
                BDBossArenaTrigger arenaTrigger = arenaTriggers[i];

                if (arenaTrigger != null)
                    arenaTrigger.ResetTrigger();
            }
        }
    }
}
