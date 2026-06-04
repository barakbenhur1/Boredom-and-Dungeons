using UnityEngine;

namespace BoredomAndDungeons
{
    public static class BDPerformanceGuard
    {
        private sealed class Runner : MonoBehaviour
        {
            private void Update()
            {
                Tick(Time.unscaledDeltaTime);
            }
        }

        private static Runner runner;
        private static float smoothedFps = 60f;
        private static float lowFpsTimer;
        private static float recoveryTimer;
        private static bool reducedEffects;

        private const float LowFpsThreshold = 43f;
        private const float RecoverFpsThreshold = 54f;
        private const float LowFpsHoldSeconds = 1.15f;
        private const float RecoverHoldSeconds = 2.35f;

        public static bool ReducedEffects
        {
            get
            {
                if (!Application.isPlaying)
                    return false;

                EnsureRunner();
                return reducedEffects;
            }
        }

        public static float SmoothedFps
        {
            get
            {
                if (!Application.isPlaying)
                    return smoothedFps;

                EnsureRunner();
                return smoothedFps;
            }
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void Bootstrap()
        {
            EnsureRunner();
            Reset();
        }

        private static void Tick(float unscaledDeltaTime)
        {
            if (unscaledDeltaTime <= 0f)
                return;

            float instantFps = 1f / Mathf.Max(0.0001f, unscaledDeltaTime);
            smoothedFps = Mathf.Lerp(smoothedFps, instantFps, 1f - Mathf.Exp(-4.0f * unscaledDeltaTime));

            if (smoothedFps < LowFpsThreshold)
            {
                lowFpsTimer += unscaledDeltaTime;
                recoveryTimer = 0f;

                if (lowFpsTimer >= LowFpsHoldSeconds)
                    reducedEffects = true;
            }
            else if (smoothedFps > RecoverFpsThreshold)
            {
                recoveryTimer += unscaledDeltaTime;
                lowFpsTimer = 0f;

                if (recoveryTimer >= RecoverHoldSeconds)
                    reducedEffects = false;
            }
            else
            {
                lowFpsTimer = Mathf.Max(0f, lowFpsTimer - unscaledDeltaTime * 0.5f);
                recoveryTimer = Mathf.Max(0f, recoveryTimer - unscaledDeltaTime * 0.5f);
            }
        }

        public static bool AllowCosmeticSpawn(float reducedModeChance = 0.35f)
        {
            if (!Application.isPlaying)
                return true;

            EnsureRunner();

            if (!reducedEffects)
                return true;

            reducedModeChance = Mathf.Clamp01(reducedModeChance);
            return Random.value <= reducedModeChance;
        }

        public static float ResolveEffectLifetime(float normalLifetime)
        {
            return reducedEffects ? normalLifetime * 0.65f : normalLifetime;
        }

        public static float ResolveEffectScale(float normalScale)
        {
            return reducedEffects ? normalScale * 0.82f : normalScale;
        }

        public static void Reset()
        {
            if (!Application.isPlaying)
            {
                smoothedFps = 60f;
                lowFpsTimer = 0f;
                recoveryTimer = 0f;
                reducedEffects = false;
                return;
            }

            smoothedFps = 60f;
            lowFpsTimer = 0f;
            recoveryTimer = 0f;
            reducedEffects = false;
        }

        private static void EnsureRunner()
        {
            if (!Application.isPlaying)
                return;

            if (runner != null)
                return;

            GameObject go = new GameObject("BD_PerformanceGuard");
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
