using System;
using System.Collections.Generic;
using UnityEngine;

namespace BoredomAndDungeons
{
    [DefaultExecutionOrder(-800)]
    [DisallowMultipleComponent]
    public sealed class BDSettingsAudioRouter :
        MonoBehaviour
    {
        [SerializeField] private float discoveryInterval =
            1.25f;

        private readonly Dictionary<AudioSource, float>
            baseVolumes =
                new Dictionary<AudioSource, float>();

        private readonly List<AudioSource>
            removalBuffer =
                new List<AudioSource>();

        private float nextDiscoveryAt;

        private void OnEnable()
        {
            BDGameSettings.EnsureLoaded();

            BDGameSettings.SettingsChanged +=
                HandleSettingsChanged;

            DiscoverSources();
            ApplyVolumes();
        }

        private void OnDisable()
        {
            BDGameSettings.SettingsChanged -=
                HandleSettingsChanged;
        }

        private void Update()
        {
            if (Time.unscaledTime <
                nextDiscoveryAt)
            {
                return;
            }

            nextDiscoveryAt =
                Time.unscaledTime +
                Mathf.Max(
                    0.25f,
                    discoveryInterval
                );

            DiscoverSources();
            ApplyVolumes();
        }

        private void DiscoverSources()
        {
            AudioSource[] sources =
                FindObjectsByType<AudioSource>(
                    FindObjectsInactive.Include,
                    FindObjectsSortMode.None
                );

            for (int index = 0;
                 index < sources.Length;
                 index++)
            {
                AudioSource source = sources[index];

                if (source == null ||
                    IsGeneratedGameFeelSource(source))
                {
                    continue;
                }

                if (!baseVolumes.ContainsKey(source))
                {
                    baseVolumes[source] =
                        Mathf.Clamp01(source.volume);
                }
            }

            removalBuffer.Clear();

            foreach (
                KeyValuePair<AudioSource, float>
                    pair in baseVolumes)
            {
                if (pair.Key == null)
                    removalBuffer.Add(pair.Key);
            }

            for (int index = 0;
                 index < removalBuffer.Count;
                 index++)
            {
                baseVolumes.Remove(
                    removalBuffer[index]
                );
            }
        }

        private void ApplyVolumes()
        {
            foreach (
                KeyValuePair<AudioSource, float>
                    pair in baseVolumes)
            {
                AudioSource source = pair.Key;

                if (source == null)
                    continue;

                float categoryVolume =
                    IsMusicSource(source)
                        ? BDGameSettings.MusicVolume
                        : BDGameSettings.SfxVolume;

                source.volume =
                    Mathf.Clamp01(
                        pair.Value *
                        categoryVolume
                    );
            }
        }

        private void HandleSettingsChanged()
        {
            ApplyVolumes();
        }

        private static bool IsGeneratedGameFeelSource(
            AudioSource source)
        {
            if (source == null)
                return false;

            return string.Equals(
                source.gameObject.name,
                "BD_GameFeelAudio",
                StringComparison.Ordinal
            );
        }

        private static bool IsMusicSource(
            AudioSource source)
        {
            if (source == null)
                return false;

            if (source.loop)
                return true;

            string combined =
                (
                    source.gameObject.name +
                    " " +
                    source.name +
                    " " +
                    (
                        source.clip != null
                            ? source.clip.name
                            : string.Empty
                    )
                ).ToLowerInvariant();

            return
                combined.Contains("music") ||
                combined.Contains("bgm") ||
                combined.Contains("theme") ||
                combined.Contains("soundtrack");
        }
    }
}
