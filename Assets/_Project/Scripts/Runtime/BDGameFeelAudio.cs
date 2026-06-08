using UnityEngine;

namespace BoredomAndDungeons
{
    public static class BDGameFeelAudio
    {
        private sealed class Runner : MonoBehaviour
        {
            private AudioSource source;

            private void Awake()
            {
                source = gameObject.AddComponent<AudioSource>();
                source.playOnAwake = false;
                source.spatialBlend = 0f;
                source.volume = 0.55f;
                source.pitch = 1f;
            }

            public void Play(AudioClip clip, float volume, float pitch)
            {
                if (clip == null || source == null)
                    return;

                source.pitch = Mathf.Clamp(pitch, 0.55f, 1.65f);
                source.PlayOneShot(clip, Mathf.Clamp01(volume));
            }

            public void StopTransientAudio()
            {
                if (source != null)
                    source.Stop();
            }
        }

        private static Runner runner;

        private static AudioClip lightHitClip;
        private static AudioClip heavyHitClip;
        private static AudioClip rangedShotClip;
        private static AudioClip rangedHitClip;
        private static AudioClip pickupClip;
        private static AudioClip deathClip;
        private static AudioClip dodgeClip;
        private static AudioClip damageClip;
        private static AudioClip parryCueClip;
        private static AudioClip parryLockClip;
        private static AudioClip parryReleaseClip;
        private static AudioClip bombExplosionClip;
        private static AudioClip quicksandEnterClip;
        private static AudioClip quicksandSinkClip;
        private static AudioClip quicksandEscapeClip;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void Bootstrap()
        {
            EnsureRunner();
            EnsureClips();
        }

        public static void PlayLightHit() => Play(lightHitClip, 0.32f, 1.05f);
        public static void PlayHeavyHit() => Play(heavyHitClip, 0.48f, 0.92f);
        public static void PlayRangedShot() => Play(rangedShotClip, 0.25f, 1.05f);
        public static void PlayRangedHit() => Play(rangedHitClip, 0.30f, 1.0f);
        public static void PlayPickup() => Play(pickupClip, 0.38f, 1.12f);
        public static void PlayEnemyDeath() => Play(deathClip, 0.34f, 0.82f);
        public static void PlayDodge() => Play(dodgeClip, 0.25f, 1.15f);
        public static void PlayDamage() => Play(damageClip, 0.30f, 0.95f);
        public static void PlayParryCue() => Play(parryCueClip, 0.42f, 1.22f);
        public static void PlayParryLock() => Play(parryLockClip, 0.50f, 0.88f);
        public static void PlayParryRelease() => Play(parryReleaseClip, 0.38f, 1.08f);
        public static void PlayBombExplosion() => Play(bombExplosionClip, 0.62f, 0.90f);
        public static void PlayQuicksandEnter() => Play(quicksandEnterClip, 0.30f, 0.82f);
        public static void PlayQuicksandSink() => Play(quicksandSinkClip, 0.48f, 0.68f);
        public static void PlayQuicksandEscape() => Play(quicksandEscapeClip, 0.28f, 1.08f);

        private static void Play(AudioClip clip, float volume, float pitch)
        {
            if (!Application.isPlaying ||
                BDNewRunFeedbackReset.IsFeedbackSuppressed)
            {
                return;
            }

            EnsureRunner();
            EnsureClips();

            if (runner != null)
                runner.Play(
                    clip,
                    volume *
                    BDGameSettings.SfxVolume,
                    pitch
                );
        }

        public static void StopTransientAudio()
        {
            if (runner != null)
                runner.StopTransientAudio();
        }

        private static void EnsureRunner()
        {
            if (!Application.isPlaying)
                return;

            if (runner != null)
                return;

            GameObject go = new GameObject("BD_GameFeelAudio");
            Object.DontDestroyOnLoad(go);
            runner = go.AddComponent<Runner>();
        }

        private static void EnsureClips()
        {
            if (!Application.isPlaying)
                return;

            if (lightHitClip != null)
                return;

            lightHitClip = MakeImpactClip("BD_LightHit", 0.085f, 360f, 90f, 0.25f);
            heavyHitClip = MakeImpactClip("BD_HeavyHit", 0.145f, 190f, 46f, 0.55f);
            rangedShotClip = MakeLaserClip("BD_RangedShot", 0.105f, 670f, 330f);
            rangedHitClip = MakeImpactClip("BD_RangedHit", 0.095f, 420f, 110f, 0.32f);
            pickupClip = MakePickupClip("BD_Pickup", 0.20f);
            deathClip = MakeImpactClip("BD_EnemyDeath", 0.22f, 145f, 36f, 0.72f);
            dodgeClip = MakeWhooshClip("BD_Dodge", 0.13f);
            damageClip = MakeImpactClip("BD_Damage", 0.12f, 240f, 65f, 0.42f);
            parryCueClip = MakePickupClip("BD_ParryCue", 0.11f);
            parryLockClip = MakeImpactClip("BD_ParryLock", 0.18f, 520f, 72f, 0.18f);
            parryReleaseClip = MakeWhooshClip("BD_ParryRelease", 0.20f);
            bombExplosionClip = MakeImpactClip("BD_BombExplosion", 0.28f, 135f, 38f, 0.82f);
            quicksandEnterClip = MakeWhooshClip("BD_QuicksandEnter", 0.18f);
            quicksandSinkClip = MakeImpactClip("BD_QuicksandSink", 0.32f, 118f, 34f, 0.62f);
            quicksandEscapeClip = MakePickupClip("BD_QuicksandEscape", 0.16f);
        }

        private static AudioClip MakeImpactClip(string name, float duration, float startHz, float endHz, float noiseAmount)
        {
            const int sampleRate = 44100;
            int sampleCount = Mathf.Max(1, Mathf.RoundToInt(sampleRate * duration));
            float[] data = new float[sampleCount];

            float phase = 0f;

            for (int i = 0; i < sampleCount; i++)
            {
                float t = i / (float)sampleCount;
                float hz = Mathf.Lerp(startHz, endHz, t);
                phase += hz / sampleRate;
                float envelope = Mathf.Exp(-9f * t);
                float tone = Mathf.Sin(phase * Mathf.PI * 2f);
                float noise = PseudoNoise(i, name.Length) * noiseAmount;
                data[i] = Mathf.Clamp((tone * 0.65f + noise) * envelope, -1f, 1f);
            }

            return CreateClip(name, data, sampleRate);
        }

        private static AudioClip MakeLaserClip(string name, float duration, float startHz, float endHz)
        {
            const int sampleRate = 44100;
            int sampleCount = Mathf.Max(1, Mathf.RoundToInt(sampleRate * duration));
            float[] data = new float[sampleCount];

            float phase = 0f;

            for (int i = 0; i < sampleCount; i++)
            {
                float t = i / (float)sampleCount;
                float hz = Mathf.Lerp(startHz, endHz, t);
                phase += hz / sampleRate;
                float envelope = Mathf.Sin(t * Mathf.PI) * Mathf.Exp(-2.2f * t);
                float tone = Mathf.Sin(phase * Mathf.PI * 2f);
                data[i] = Mathf.Clamp(tone * envelope * 0.55f, -1f, 1f);
            }

            return CreateClip(name, data, sampleRate);
        }

        private static AudioClip MakePickupClip(string name, float duration)
        {
            const int sampleRate = 44100;
            int sampleCount = Mathf.Max(1, Mathf.RoundToInt(sampleRate * duration));
            float[] data = new float[sampleCount];

            float phaseA = 0f;
            float phaseB = 0f;

            for (int i = 0; i < sampleCount; i++)
            {
                float t = i / (float)sampleCount;
                float hzA = Mathf.Lerp(520f, 980f, t);
                float hzB = Mathf.Lerp(780f, 1360f, t);

                phaseA += hzA / sampleRate;
                phaseB += hzB / sampleRate;

                float envelope = Mathf.Sin(t * Mathf.PI);
                float tone = Mathf.Sin(phaseA * Mathf.PI * 2f) * 0.42f + Mathf.Sin(phaseB * Mathf.PI * 2f) * 0.26f;
                data[i] = Mathf.Clamp(tone * envelope, -1f, 1f);
            }

            return CreateClip(name, data, sampleRate);
        }

        private static AudioClip MakeWhooshClip(string name, float duration)
        {
            const int sampleRate = 44100;
            int sampleCount = Mathf.Max(1, Mathf.RoundToInt(sampleRate * duration));
            float[] data = new float[sampleCount];

            for (int i = 0; i < sampleCount; i++)
            {
                float t = i / (float)sampleCount;
                float envelope = Mathf.Sin(t * Mathf.PI);
                float sweep = Mathf.Lerp(0.15f, 0.72f, t);
                float noise = PseudoNoise(i, 37) * envelope * sweep;
                data[i] = Mathf.Clamp(noise * 0.55f, -1f, 1f);
            }

            return CreateClip(name, data, sampleRate);
        }

        private static AudioClip CreateClip(string name, float[] data, int sampleRate)
        {
            AudioClip clip = AudioClip.Create(name, data.Length, 1, sampleRate, false);
            clip.SetData(data, 0);
            return clip;
        }

        private static float PseudoNoise(int sample, int seed)
        {
            float x = Mathf.Sin((sample + seed * 131) * 12.9898f) * 43758.5453f;
            return (x - Mathf.Floor(x)) * 2f - 1f;
        }
    }
}
