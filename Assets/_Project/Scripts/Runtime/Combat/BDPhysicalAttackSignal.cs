using UnityEngine;

namespace BoredomAndDungeons
{
    public static class BDPhysicalAttackSignal
    {
        private struct Signal
        {
            public Transform Attacker;
            public float ReportedAt;
            public bool Consumed;
        }

        private const int Capacity = 8;
        private static readonly Signal[] Signals = new Signal[Capacity];
        private static int writeIndex;

        public static void Report(Transform attacker)
        {
            if (attacker == null)
                return;

            Signals[writeIndex] = new Signal
            {
                Attacker = attacker,
                ReportedAt = Time.unscaledTime,
                Consumed = false
            };

            writeIndex = (writeIndex + 1) % Capacity;
        }

        public static bool TryConsumeRecent(
            Transform target,
            float maxAge,
            float maxDistance,
            out Transform attacker)
        {
            attacker = null;

            if (target == null)
                return false;

            float now = Time.unscaledTime;
            float bestTime = float.MinValue;
            int bestIndex = -1;

            for (int i = 0; i < Capacity; i++)
            {
                Signal signal = Signals[i];

                if (signal.Consumed || signal.Attacker == null)
                    continue;

                float age = now - signal.ReportedAt;
                if (age < 0f || age > Mathf.Max(0.01f, maxAge))
                    continue;

                Vector3 delta = signal.Attacker.position - target.position;
                delta.y = 0f;

                if (delta.sqrMagnitude > maxDistance * maxDistance)
                    continue;

                if (signal.ReportedAt <= bestTime)
                    continue;

                bestTime = signal.ReportedAt;
                bestIndex = i;
            }

            if (bestIndex < 0)
                return false;

            Signal selected = Signals[bestIndex];
            selected.Consumed = true;
            Signals[bestIndex] = selected;
            attacker = selected.Attacker;
            return true;
        }
    }
}
