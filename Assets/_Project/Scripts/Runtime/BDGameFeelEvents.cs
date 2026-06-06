using UnityEngine;

namespace BoredomAndDungeons
{
    public static class BDGameFeelEvents
    {
        private static float shakeEndTime;
        private static float shakeStartedAt;
        private static float shakeDuration;
        private static float shakeStrength;
        private static float shakeFrequency = 33f;
        private static Vector2 shakeSeed = new Vector2(19.73f, 41.17f);

        public static bool HasCameraShake => Application.isPlaying && Time.time < shakeEndTime && shakeStrength > 0.001f;

        public static void RequestCameraShake(float strength, float duration)
        {
            if (!Application.isPlaying)
                return;

            strength *=
                BDGameSettings.CameraShakeIntensity;

            strength = Mathf.Max(0f, strength);
            duration = Mathf.Max(0f, duration);

            if (strength <= 0f || duration <= 0f)
                return;

            // Stronger or longer shake wins, but repeated impacts can extend feedback.
            shakeStrength = Mathf.Max(shakeStrength, strength);
            shakeDuration = Mathf.Max(shakeDuration, duration);
            shakeStartedAt = Time.time;
            shakeEndTime = Mathf.Max(shakeEndTime, Time.time + duration);

            float seed = Time.time * 97.31f + strength * 13.7f;
            shakeSeed = new Vector2(Mathf.Sin(seed) * 100f, Mathf.Cos(seed * 1.37f) * 100f);
        }

        public static Vector3 SampleCameraShakeOffset()
        {
            if (!Application.isPlaying)
                return Vector3.zero;

            if (!HasCameraShake)
                return Vector3.zero;

            float duration = Mathf.Max(0.01f, shakeDuration);
            float progress = Mathf.Clamp01((Time.time - shakeStartedAt) / duration);
            float fade = 1f - progress;
            fade *= fade;

            float t = Time.time * shakeFrequency;
            float x = (Mathf.PerlinNoise(shakeSeed.x, t) - 0.5f) * 2f;
            float y = (Mathf.PerlinNoise(shakeSeed.y, t + 7.3f) - 0.5f) * 2f;

            return new Vector3(x, y * 0.55f, 0f) * shakeStrength * fade;
        }

        public static void Reset()
        {
            if (!Application.isPlaying)
                return;
            shakeEndTime = 0f;
            shakeStartedAt = 0f;
            shakeDuration = 0f;
            shakeStrength = 0f;
        }
    }
}
