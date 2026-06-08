using System.Collections.Generic;
using UnityEngine;

namespace BoredomAndDungeons
{
    public static class BDEnemyMovementPolish
    {
        private sealed class MotionState
        {
            public Vector3 Velocity;
            public float LastSampleTime;
        }

        private static readonly Dictionary<int, MotionState>
            States = new Dictionary<int, MotionState>();

        private static float nextCleanupAt;

        public static Vector3 Filter(
            MonoBehaviour owner,
            Vector3 requestedMotion)
        {
            if (owner == null ||
                Time.deltaTime <= 0.00001f)
            {
                return requestedMotion;
            }

            Vector3 vertical =
                Vector3.up * requestedMotion.y;

            Vector3 horizontal =
                new Vector3(
                    requestedMotion.x,
                    0f,
                    requestedMotion.z
                );

            int key = owner.GetInstanceID();

            if (!States.TryGetValue(
                    key,
                    out MotionState state))
            {
                state = new MotionState();
                States.Add(key, state);
            }

            float now = Time.unscaledTime;

            if (now - state.LastSampleTime > 0.20f)
                state.Velocity = Vector3.zero;

            state.LastSampleTime = now;

            if (horizontal.sqrMagnitude < 0.0000001f)
            {
                state.Velocity = Vector3.zero;
                CleanupIfNeeded(now);
                return vertical;
            }

            float deltaTime =
                Mathf.Max(
                    0.0001f,
                    Time.deltaTime
                );

            Vector3 desiredVelocity =
                horizontal / deltaTime;

            float desiredSpeed =
                desiredVelocity.magnitude;

            float maximumBrainSpeed =
                owner is BDChargerEnemy
                    ? 14f
                    : 8.75f;
            desiredSpeed = Mathf.Min(
                desiredSpeed,
                maximumBrainSpeed
            );

            Vector3 desiredDirection =
                desiredVelocity / desiredSpeed;

            float currentSpeed =
                state.Velocity.magnitude;

            Vector3 currentDirection =
                currentSpeed > 0.01f
                    ? state.Velocity / currentSpeed
                    : owner.transform.forward;

            currentDirection.y = 0f;

            if (currentDirection.sqrMagnitude < 0.001f)
                currentDirection = desiredDirection;

            bool fastCommittedMove =
                desiredSpeed >= 7f;

            float turnDegreesPerSecond =
                fastCommittedMove
                    ? 620f
                    : 320f;

            float acceleration =
                fastCommittedMove
                    ? 75f
                    : 20f;

            Vector3 turnedDirection =
                Vector3.RotateTowards(
                    currentDirection.normalized,
                    desiredDirection,
                    Mathf.Deg2Rad *
                    turnDegreesPerSecond *
                    deltaTime,
                    0f
                );

            float smoothedSpeed =
                Mathf.MoveTowards(
                    currentSpeed,
                    desiredSpeed,
                    acceleration * deltaTime
                );

            state.Velocity =
                turnedDirection.normalized *
                smoothedSpeed;

            CleanupIfNeeded(now);

            Vector3 resolved =
                state.Velocity * deltaTime + vertical;
            resolved = BDEnemyHazardNavigation.FilterBrainMotion(
                owner,
                resolved
            );
            resolved = BDQuicksandStatus.FilterMotion(
                owner.gameObject,
                resolved
            );
            return resolved;
        }

        private static void CleanupIfNeeded(
            float now)
        {
            if (now < nextCleanupAt)
                return;

            nextCleanupAt = now + 8f;

            List<int> stale = null;

            foreach (KeyValuePair<int, MotionState> pair
                     in States)
            {
                if (now - pair.Value.LastSampleTime <= 15f)
                    continue;

                if (stale == null)
                    stale = new List<int>();

                stale.Add(pair.Key);
            }

            if (stale == null)
                return;

            for (int index = 0;
                 index < stale.Count;
                 index++)
            {
                States.Remove(stale[index]);
            }
        }
    }
}
