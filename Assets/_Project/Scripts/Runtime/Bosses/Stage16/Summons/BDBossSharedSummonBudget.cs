using System;
using System.Collections.Generic;
using UnityEngine;

namespace BoredomAndDungeons
{
    [DisallowMultipleComponent]
    public sealed class BDBossSharedSummonBudget : MonoBehaviour
    {
        [Header("Encounter")]
        [SerializeField] private BDBossEncounterController encounter;

        [Header("Shared limit")]
        [SerializeField, Min(0)] private int maximumAliveSummons = 8;

        [Header("Cleanup")]
        [SerializeField] private bool clearOnDormant = true;
        [SerializeField] private bool clearOnVictory = true;
        [SerializeField] private bool clearOnFailure = true;
        [SerializeField] private bool destroySummonsWhenCleared = true;

        [Header("Debug")]
        [SerializeField] private bool logBudgetChanges;

        private readonly HashSet<BDSummonedEnemyLease> activeLeases =
            new HashSet<BDSummonedEnemyLease>();

        public event Action<int, int> BudgetChanged;

        public int MaximumAliveSummons => Mathf.Max(0, maximumAliveSummons);
        public int AliveSummons
        {
            get
            {
                RemoveInvalidLeases();
                return activeLeases.Count;
            }
        }

        public int AvailableSlots =>
            Mathf.Max(0, MaximumAliveSummons - AliveSummons);

        public bool IsFull => AvailableSlots <= 0;

        private void Awake()
        {
            ResolveEncounter();
        }

        private void OnEnable()
        {
            ResolveEncounter();

            if (encounter != null)
            {
                encounter.StateChanged -= HandleEncounterStateChanged;
                encounter.StateChanged += HandleEncounterStateChanged;
            }

            NotifyBudgetChanged();
        }

        private void OnDisable()
        {
            if (encounter != null)
                encounter.StateChanged -= HandleEncounterStateChanged;
        }

        private void OnValidate()
        {
            maximumAliveSummons = Mathf.Max(0, maximumAliveSummons);
        }

        public void Configure(
            BDBossEncounterController newEncounter,
            int maximumAlive)
        {
            if (encounter != null)
                encounter.StateChanged -= HandleEncounterStateChanged;

            encounter = newEncounter;
            maximumAliveSummons = Mathf.Max(0, maximumAlive);

            if (isActiveAndEnabled && encounter != null)
            {
                encounter.StateChanged -= HandleEncounterStateChanged;
                encounter.StateChanged += HandleEncounterStateChanged;
            }

            NotifyBudgetChanged();
        }

        public bool CanSpawn(int requestedCount = 1)
        {
            return requestedCount > 0 &&
                   AvailableSlots >= requestedCount;
        }

        public int ClampToAvailable(int requestedCount)
        {
            return Mathf.Clamp(
                requestedCount,
                0,
                AvailableSlots
            );
        }

        public bool TrySpawn(
            GameObject prefab,
            Vector3 position,
            Quaternion rotation,
            Transform parent,
            out GameObject instance)
        {
            instance = null;

            if (prefab == null || AvailableSlots <= 0)
                return false;

            instance = Instantiate(
                prefab,
                position,
                rotation,
                parent
            );

            if (instance == null)
                return false;

            BDSummonedEnemyLease lease =
                instance.GetComponent<BDSummonedEnemyLease>();

            if (lease == null)
                lease = instance.AddComponent<BDSummonedEnemyLease>();

            BDHealth health =
                instance.GetComponent<BDHealth>();

            if (health == null)
            {
                health = instance.GetComponentInChildren<BDHealth>(
                    includeInactive: true
                );
            }

            lease.Bind(this, health);
            RegisterLease(lease);
            return true;
        }

        public bool TryRegisterExistingSummon(GameObject summon)
        {
            if (summon == null || AvailableSlots <= 0)
                return false;

            BDSummonedEnemyLease lease =
                summon.GetComponent<BDSummonedEnemyLease>();

            if (lease == null)
                lease = summon.AddComponent<BDSummonedEnemyLease>();

            if (activeLeases.Contains(lease))
                return true;

            BDHealth health = summon.GetComponent<BDHealth>();

            if (health == null)
            {
                health = summon.GetComponentInChildren<BDHealth>(
                    includeInactive: true
                );
            }

            lease.Bind(this, health);
            RegisterLease(lease);
            return true;
        }

        public void SetMaximumAliveSummons(int maximumAlive)
        {
            maximumAliveSummons = Mathf.Max(0, maximumAlive);
            NotifyBudgetChanged();
        }

        public void ClearAllSummons()
        {
            ClearAllSummons(destroySummonsWhenCleared);
        }

        public void ClearAllSummons(bool destroyInstances)
        {
            RemoveInvalidLeases();

            BDSummonedEnemyLease[] snapshot =
                new BDSummonedEnemyLease[activeLeases.Count];

            activeLeases.CopyTo(snapshot);

            for (int i = 0; i < snapshot.Length; i++)
            {
                BDSummonedEnemyLease lease = snapshot[i];

                if (lease != null)
                    lease.ReleaseFromBudget(destroyInstances);
            }

            activeLeases.Clear();
            NotifyBudgetChanged();
        }

        internal void ReleaseLease(BDSummonedEnemyLease lease)
        {
            if (lease == null)
                return;

            if (!activeLeases.Remove(lease))
                return;

            if (logBudgetChanges)
            {
                Debug.Log(
                    $"Summon budget released: " +
                    $"{activeLeases.Count}/{MaximumAliveSummons}",
                    this
                );
            }

            NotifyBudgetChanged();
        }

        private void RegisterLease(BDSummonedEnemyLease lease)
        {
            if (lease == null)
                return;

            if (!activeLeases.Add(lease))
                return;

            if (logBudgetChanges)
            {
                Debug.Log(
                    $"Summon budget used: " +
                    $"{activeLeases.Count}/{MaximumAliveSummons}",
                    this
                );
            }

            NotifyBudgetChanged();
        }

        private void ResolveEncounter()
        {
            if (encounter != null)
                return;

            encounter =
                GetComponent<BDBossEncounterController>();

            if (encounter == null)
            {
                encounter =
                    GetComponentInParent<BDBossEncounterController>();
            }
        }

        private void HandleEncounterStateChanged(
            BDBossEncounterState state)
        {
            if (state == BDBossEncounterState.Dormant &&
                clearOnDormant)
            {
                ClearAllSummons();
                return;
            }

            if ((state == BDBossEncounterState.Victory ||
                 state == BDBossEncounterState.Completed) &&
                clearOnVictory)
            {
                ClearAllSummons();
                return;
            }

            if (state == BDBossEncounterState.Failed &&
                clearOnFailure)
            {
                ClearAllSummons();
            }
        }

        private void RemoveInvalidLeases()
        {
            activeLeases.RemoveWhere(
                lease => lease == null || !lease.IsBoundTo(this)
            );
        }

        private void NotifyBudgetChanged()
        {
            BudgetChanged?.Invoke(
                AliveSummons,
                MaximumAliveSummons
            );
        }
    }
}
