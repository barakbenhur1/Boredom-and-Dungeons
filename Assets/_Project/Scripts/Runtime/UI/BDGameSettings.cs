using System;
using UnityEngine;

namespace BoredomAndDungeons
{
    public static class BDGameSettings
    {
        private const string MasterVolumeKey =
            "BD.Settings.MasterVolume";
        private const string MusicVolumeKey =
            "BD.Settings.MusicVolume";
        private const string SfxVolumeKey =
            "BD.Settings.SfxVolume";
        private const string MouseSensitivityKey =
            "BD.Settings.MouseSensitivity";
        private const string CameraShakeKey =
            "BD.Settings.CameraShake";
        private const string QualityLevelKey =
            "BD.Settings.QualityLevel";
        private const string FullscreenKey =
            "BD.Settings.Fullscreen";
        private const string VSyncKey =
            "BD.Settings.VSync";
        private const string TargetFpsKey =
            "BD.Settings.TargetFps";

        private static readonly int[] TargetFpsOptions =
        {
            30,
            60,
            120,
            -1
        };

        private static bool loaded;

        public static event Action SettingsChanged;

        public static float MasterVolume { get; private set; } = 1f;
        public static float MusicVolume { get; private set; } = 0.8f;
        public static float SfxVolume { get; private set; } = 0.9f;
        public static float MouseSensitivityMultiplier
        {
            get;
            private set;
        } = 1f;

        public static float CameraShakeIntensity
        {
            get;
            private set;
        } = 1f;

        public static int QualityLevel { get; private set; }
        public static bool Fullscreen { get; private set; } = true;
        public static bool VSync { get; private set; } = true;
        public static int TargetFps { get; private set; } = 60;

        public static string QualityName
        {
            get
            {
                EnsureLoaded();

                string[] names = QualitySettings.names;

                if (names == null ||
                    names.Length == 0)
                {
                    return "Default";
                }

                int index = Mathf.Clamp(
                    QualityLevel,
                    0,
                    names.Length - 1
                );

                return names[index];
            }
        }

        public static string TargetFpsLabel =>
            TargetFps <= 0
                ? "Unlimited"
                : TargetFps.ToString();

        [RuntimeInitializeOnLoadMethod(
            RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Bootstrap()
        {
            EnsureLoaded();
            ApplyAll();
        }

        public static void EnsureLoaded()
        {
            if (loaded)
                return;

            int defaultQuality =
                Mathf.Max(
                    0,
                    QualitySettings.GetQualityLevel()
                );

            MasterVolume = Mathf.Clamp01(
                PlayerPrefs.GetFloat(
                    MasterVolumeKey,
                    1f
                )
            );

            MusicVolume = Mathf.Clamp01(
                PlayerPrefs.GetFloat(
                    MusicVolumeKey,
                    0.8f
                )
            );

            SfxVolume = Mathf.Clamp01(
                PlayerPrefs.GetFloat(
                    SfxVolumeKey,
                    0.9f
                )
            );

            MouseSensitivityMultiplier = Mathf.Clamp(
                PlayerPrefs.GetFloat(
                    MouseSensitivityKey,
                    1f
                ),
                0.45f,
                1.65f
            );

            CameraShakeIntensity = Mathf.Clamp01(
                PlayerPrefs.GetFloat(
                    CameraShakeKey,
                    1f
                )
            );

            QualityLevel = Mathf.Max(
                0,
                PlayerPrefs.GetInt(
                    QualityLevelKey,
                    defaultQuality
                )
            );

            Fullscreen =
                PlayerPrefs.GetInt(
                    FullscreenKey,
                    Screen.fullScreen ? 1 : 0
                ) != 0;

            VSync =
                PlayerPrefs.GetInt(
                    VSyncKey,
                    QualitySettings.vSyncCount > 0
                        ? 1
                        : 0
                ) != 0;

            TargetFps = NormalizeTargetFps(
                PlayerPrefs.GetInt(
                    TargetFpsKey,
                    60
                )
            );

            loaded = true;
        }

        public static void ApplyAll()
        {
            EnsureLoaded();

            AudioListener.volume = MasterVolume;

            string[] qualityNames =
                QualitySettings.names;

            if (qualityNames != null &&
                qualityNames.Length > 0)
            {
                QualityLevel = Mathf.Clamp(
                    QualityLevel,
                    0,
                    qualityNames.Length - 1
                );

                QualitySettings.SetQualityLevel(
                    QualityLevel,
                    applyExpensiveChanges: true
                );
            }

#if !UNITY_IOS && !UNITY_ANDROID
            Screen.fullScreenMode =
                Fullscreen
                    ? FullScreenMode.FullScreenWindow
                    : FullScreenMode.Windowed;
#endif

            QualitySettings.vSyncCount =
                VSync ? 1 : 0;

            Application.targetFrameRate =
                VSync
                    ? -1
                    : TargetFps;

            SettingsChanged?.Invoke();
        }

        public static void SetMasterVolume(float value)
        {
            EnsureLoaded();
            MasterVolume = Mathf.Clamp01(value);
            AudioListener.volume = MasterVolume;

            PlayerPrefs.SetFloat(
                MasterVolumeKey,
                MasterVolume
            );

            CommitChange();
        }

        public static void SetMusicVolume(float value)
        {
            EnsureLoaded();
            MusicVolume = Mathf.Clamp01(value);

            PlayerPrefs.SetFloat(
                MusicVolumeKey,
                MusicVolume
            );

            CommitChange();
        }

        public static void SetSfxVolume(float value)
        {
            EnsureLoaded();
            SfxVolume = Mathf.Clamp01(value);

            PlayerPrefs.SetFloat(
                SfxVolumeKey,
                SfxVolume
            );

            CommitChange();
        }

        public static void SetMouseSensitivity(float value)
        {
            EnsureLoaded();

            MouseSensitivityMultiplier = Mathf.Clamp(
                value,
                0.45f,
                1.65f
            );

            PlayerPrefs.SetFloat(
                MouseSensitivityKey,
                MouseSensitivityMultiplier
            );

            CommitChange();
        }

        public static void SetCameraShake(float value)
        {
            EnsureLoaded();

            CameraShakeIntensity =
                Mathf.Clamp01(value);

            PlayerPrefs.SetFloat(
                CameraShakeKey,
                CameraShakeIntensity
            );

            CommitChange();
        }

        public static void CycleQuality(int direction)
        {
            EnsureLoaded();

            string[] names = QualitySettings.names;

            if (names == null ||
                names.Length == 0)
            {
                return;
            }

            int count = names.Length;
            int next =
                (QualityLevel + direction) %
                count;

            if (next < 0)
                next += count;

            QualityLevel = next;

            PlayerPrefs.SetInt(
                QualityLevelKey,
                QualityLevel
            );

            QualitySettings.SetQualityLevel(
                QualityLevel,
                applyExpensiveChanges: true
            );

            CommitChange();
        }

        public static void SetFullscreen(bool value)
        {
            EnsureLoaded();
            Fullscreen = value;

            PlayerPrefs.SetInt(
                FullscreenKey,
                Fullscreen ? 1 : 0
            );

#if !UNITY_IOS && !UNITY_ANDROID
            Screen.fullScreenMode =
                Fullscreen
                    ? FullScreenMode.FullScreenWindow
                    : FullScreenMode.Windowed;
#endif

            CommitChange();
        }

        public static void SetVSync(bool value)
        {
            EnsureLoaded();
            VSync = value;

            PlayerPrefs.SetInt(
                VSyncKey,
                VSync ? 1 : 0
            );

            QualitySettings.vSyncCount =
                VSync ? 1 : 0;

            Application.targetFrameRate =
                VSync
                    ? -1
                    : TargetFps;

            CommitChange();
        }

        public static void CycleTargetFps(int direction)
        {
            EnsureLoaded();

            int currentIndex = 0;

            for (int index = 0;
                 index < TargetFpsOptions.Length;
                 index++)
            {
                if (TargetFpsOptions[index] ==
                    TargetFps)
                {
                    currentIndex = index;
                    break;
                }
            }

            int next =
                (currentIndex + direction) %
                TargetFpsOptions.Length;

            if (next < 0)
                next += TargetFpsOptions.Length;

            TargetFps =
                TargetFpsOptions[next];

            PlayerPrefs.SetInt(
                TargetFpsKey,
                TargetFps
            );

            if (!VSync)
            {
                Application.targetFrameRate =
                    TargetFps;
            }

            CommitChange();
        }

        public static void ResetDefaults()
        {
            loaded = true;

            MasterVolume = 1f;
            MusicVolume = 0.8f;
            SfxVolume = 0.9f;
            MouseSensitivityMultiplier = 1f;
            CameraShakeIntensity = 1f;
            QualityLevel = Mathf.Max(
                0,
                QualitySettings.names.Length - 1
            );
            Fullscreen = true;
            VSync = true;
            TargetFps = 60;

            PlayerPrefs.SetFloat(
                MasterVolumeKey,
                MasterVolume
            );
            PlayerPrefs.SetFloat(
                MusicVolumeKey,
                MusicVolume
            );
            PlayerPrefs.SetFloat(
                SfxVolumeKey,
                SfxVolume
            );
            PlayerPrefs.SetFloat(
                MouseSensitivityKey,
                MouseSensitivityMultiplier
            );
            PlayerPrefs.SetFloat(
                CameraShakeKey,
                CameraShakeIntensity
            );
            PlayerPrefs.SetInt(
                QualityLevelKey,
                QualityLevel
            );
            PlayerPrefs.SetInt(
                FullscreenKey,
                1
            );
            PlayerPrefs.SetInt(
                VSyncKey,
                1
            );
            PlayerPrefs.SetInt(
                TargetFpsKey,
                TargetFps
            );

            PlayerPrefs.Save();
            ApplyAll();
        }

        public static void Save()
        {
            PlayerPrefs.Save();
        }

        private static int NormalizeTargetFps(
            int requested)
        {
            for (int index = 0;
                 index < TargetFpsOptions.Length;
                 index++)
            {
                if (TargetFpsOptions[index] ==
                    requested)
                {
                    return requested;
                }
            }

            return 60;
        }

        private static void CommitChange()
        {
            PlayerPrefs.Save();
            SettingsChanged?.Invoke();
        }
    }
}
