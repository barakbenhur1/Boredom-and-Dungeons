using System;
using System.Collections.Generic;
using UnityEngine;

namespace BoredomAndDungeons
{
    [DisallowMultipleComponent]
    public sealed class BDBossSummonBudget : MonoBehaviour
    {
        [SerializeField] private int maxActiveSummons = 8;
        [SerializeField] private bool pruneDestroyedEntriesEachFrame = true;

        private readonly HashSet<GameObject> activeSummons = new HashSet<GameObject>();

        public event Action<int, int> ActiveCountChanged;

        public int MaxActiveSummons => Mathf.Max(0, maxActiveSummons);
        public int ActiveCount => activeSummons.Count;
        public int RemainingCapacity => Mathf.Max(0, MaxActiveSummons - activeSummons.Count);
        public bool HasCapacity => activeSummons.Count < MaxActiveSummons;

        private void Update()
        {
            if (pruneDestroyedEntriesEachFrame)
                PruneDestroyedEntries();
        }

        public bool TryReserve(int requestedCount)
        {
            if (requestedCount <= 0)
                return true;

            return RemainingCapacity >= requestedCount;
        }

        public bool TryRegister(GameObject summon)
        {
            if (summon == null || !HasCapacity)
                return false;

            bool added = activeSummons.Add(summon);
            if (added)
                ActiveCountChanged?.Invoke(activeSummons.Count, MaxActiveSummons);

            return added;
        }

        public void Release(GameObject summon)
        {
            if (summon == null)
                return;

            if (activeSummons.Remove(summon))
                ActiveCountChanged?.Invoke(activeSummons.Count, MaxActiveSummons);
        }

        public void Clear()
        {
            if (activeSummons.Count == 0)
                return;

            activeSummons.Clear();
            ActiveCountChanged?.Invoke(0, MaxActiveSummons);
        }

        public void SetMaximum(int value)
        {
            maxActiveSummons = Mathf.Max(0, value);
            PruneDestroyedEntries();
            ActiveCountChanged?.Invoke(activeSummons.Count, MaxActiveSummons);
        }

        private void PruneDestroyedEntries()
        {
            if (activeSummons.Count == 0)
                return;

            List<GameObject> removed = null;

            foreach (GameObject summon in activeSummons)
            {
                if (summon != null)
                    continue;

                if (removed == null)
                    removed = new List<GameObject>();

                removed.Add(summon);
            }

            if (removed == null)
                return;

            for (int i = 0; i < removed.Count; i++)
                activeSummons.Remove(removed[i]);

            ActiveCountChanged?.Invoke(activeSummons.Count, MaxActiveSummons);
        }
    }
}
