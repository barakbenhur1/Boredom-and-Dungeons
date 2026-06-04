using System;
using System.Collections;
using UnityEngine;

namespace BoredomAndDungeons
{
    [DisallowMultipleComponent]
    public sealed class BDBossEncounterController : MonoBehaviour
    {
        [Header("Timing")]
        [SerializeField] private float introLockDuration = 1.0f;
        [SerializeField] private float transitionLockDuration = 1.0f;

        [Header("State")]
        [SerializeField] private BDBossEncounterState state = BDBossEncounterState.Dormant;
        [SerializeField] private int phaseIndex;

        public event Action<BDBossEncounterState> StateChanged;
        public event Action<int> PhaseChanged;

        public BDBossEncounterState State => state;
        public int PhaseIndex => phaseIndex;
        public bool IsCombatActive => state == BDBossEncounterState.Active;
        public bool IsFinished => state == BDBossEncounterState.Victory || state == BDBossEncounterState.Completed;

        public void BeginEncounter()
        {
            if (state != BDBossEncounterState.Dormant)
                return;

            StopAllCoroutines();
            StartCoroutine(BeginEncounterRoutine());
        }

        public void BeginTransition(int nextPhase)
        {
            if (IsFinished)
                return;

            StopAllCoroutines();
            StartCoroutine(TransitionRoutine(Mathf.Max(0, nextPhase)));
        }

        public void SetPhaseImmediately(int nextPhase)
        {
            phaseIndex = Mathf.Max(0, nextPhase);
            PhaseChanged?.Invoke(phaseIndex);
        }

        public void SetActive()
        {
            if (IsFinished)
                return;

            SetState(BDBossEncounterState.Active);
        }

        public void MarkVictory()
        {
            if (IsFinished)
                return;

            StopAllCoroutines();
            SetState(BDBossEncounterState.Victory);
        }

        public void CompleteEncounter()
        {
            StopAllCoroutines();
            SetState(BDBossEncounterState.Completed);
        }

        public void FailEncounter()
        {
            StopAllCoroutines();
            SetState(BDBossEncounterState.Failed);
        }

        public void ResetEncounter()
        {
            StopAllCoroutines();
            phaseIndex = 0;
            PhaseChanged?.Invoke(phaseIndex);
            SetState(BDBossEncounterState.Dormant);
        }

        private IEnumerator BeginEncounterRoutine()
        {
            SetState(BDBossEncounterState.Intro);

            float delay = Mathf.Max(0f, introLockDuration);
            if (delay > 0f)
                yield return new WaitForSeconds(delay);

            SetState(BDBossEncounterState.Active);
        }

        private IEnumerator TransitionRoutine(int nextPhase)
        {
            SetState(BDBossEncounterState.Transition);

            float delay = Mathf.Max(0f, transitionLockDuration);
            if (delay > 0f)
                yield return new WaitForSeconds(delay);

            phaseIndex = nextPhase;
            PhaseChanged?.Invoke(phaseIndex);
            SetState(BDBossEncounterState.Active);
        }

        private void SetState(BDBossEncounterState next)
        {
            if (state == next)
                return;

            state = next;
            StateChanged?.Invoke(state);
        }
    }
}
