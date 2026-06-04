using UnityEngine;

namespace BoredomAndDungeons
{
    public static class BDHitStop
    {
        private sealed class Runner : MonoBehaviour
        {
            private void Update()
            {
                Tick();
            }
        }

        private static Runner runner;
        private static float stopEndsAtUnscaled;
        private static float requestedScale = 1f;
        private static float previousTimeScale = 1f;
        private static bool active;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void Bootstrap()
        {
            EnsureRunner();
            Reset();
        }

        public static void Request(float duration, float timeScale)
        {
            if (!Application.isPlaying)
                return;

            duration = Mathf.Clamp(duration, 0f, 0.12f);
            timeScale = Mathf.Clamp(timeScale, 0.04f, 1f);

            if (duration <= 0f || timeScale >= 0.999f)
                return;

            EnsureRunner();

            if (!active)
            {
                previousTimeScale = Time.timeScale <= 0f ? 1f : Time.timeScale;
                active = true;
            }

            // Stronger slow-down wins; longer window extends.
            requestedScale = Mathf.Min(requestedScale, timeScale);
            stopEndsAtUnscaled = Mathf.Max(stopEndsAtUnscaled, Time.unscaledTime + duration);
            Time.timeScale = requestedScale;
        }

        private static void Tick()
        {
            if (!active)
                return;

            if (Time.unscaledTime < stopEndsAtUnscaled)
                return;

            Time.timeScale = Mathf.Approximately(previousTimeScale, 0f) ? 1f : previousTimeScale;
            active = false;
            requestedScale = 1f;
            stopEndsAtUnscaled = 0f;
            previousTimeScale = 1f;
        }

        public static void Reset()
        {
            if (!Application.isPlaying)
                return;

            if (active)
                Time.timeScale = Mathf.Approximately(previousTimeScale, 0f) ? 1f : previousTimeScale;

            active = false;
            requestedScale = 1f;
            stopEndsAtUnscaled = 0f;
            previousTimeScale = 1f;

            if (Application.isPlaying && Time.timeScale <= 0f)
                Time.timeScale = 1f;
        }

        private static void EnsureRunner()
        {
            if (!Application.isPlaying)
                return;

            if (runner != null)
                return;

            GameObject go = new GameObject("BD_HitStop_Runner");
            if (!Application.isPlaying)
            {
                Object.DestroyImmediate(go);
                return;
            }

            Object.DontDestroyOnLoad(go);
            runner = go.AddComponent<Runner>();
        }
    }
}
