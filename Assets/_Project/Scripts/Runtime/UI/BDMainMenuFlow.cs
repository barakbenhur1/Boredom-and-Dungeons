using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace BoredomAndDungeons
{
    [DefaultExecutionOrder(-900)]
    [DisallowMultipleComponent]
    public sealed class BDMainMenuFlow : MonoBehaviour
    {
        private enum OverlayMode
        {
            MainMenu,
            Settings,
            Pause,
            Loading
        }

        private static bool autoStartAfterReload;

        private static readonly Color StartGameHighlightTint =
            new Color(0.74f, 0.88f, 1.00f, 1f);

        [Header("Identity")]
        [SerializeField] private string gameTitle =
            "BOREDOM & DUNGEONS";

        [SerializeField] private string gameSubtitle =
            "ENTER THE MAZE";

        [Header("Behaviour")]
        [SerializeField] private bool showMenuOnFirstLoad =
            true;

        [SerializeField] private bool enableEscapePause =
            true;

        [SerializeField] private bool showQuitButtonOnDesktop =
            true;

        private OverlayMode mode =
            OverlayMode.MainMenu;

        private BDPlayerController playerController;
        private bool playerControllerWasEnabled;
        private bool runActive;
        private bool runNeedsReload;
        private bool resultSequenceActive;
        private bool pausedFromGameplay;
        private float nextBindingRefreshAt;
        private AsyncOperation reloadOperation;

        private GUIStyle titleStyle;
        private GUIStyle subtitleStyle;
        private GUIStyle sectionStyle;
        private GUIStyle valueStyle;
        private GUIStyle buttonStyle;
        private GUIStyle smallButtonStyle;
        private GUIStyle panelStyle;
        private GUIStyle overlayStyle;

        private Texture2D dreamyTransparentTexture;
        private Texture2D dreamyPanelTexture;
        private Texture2D dreamyButtonTexture;
        private Texture2D dreamyButtonHoverTexture;
        private Texture2D dreamyButtonActiveTexture;

        public static BDMainMenuFlow Instance
        {
            get;
            private set;
        }

        public bool IsRunActive => runActive;
        public bool IsResultSequenceActive =>
            resultSequenceActive;

        private void Awake()
        {
            if (Instance != null &&
                Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            BDGameSettings.EnsureLoaded();
            BDGameSettings.ApplyAll();

            if (autoStartAfterReload)
            {
                autoStartAfterReload = false;
                StartCoroutine(
                    AutoStartAfterSceneReload()
                );
                return;
            }

            if (showMenuOnFirstLoad)
            {
                ShowPlainMainMenu(
                    needsReload: false
                );
            }
            else
            {
                BeginCurrentRun();
            }
        }
        private void OnDestroy()
        {
            if (Instance == this)
                Instance = null;

            DestroyMenuTextures();
            RestoreTimeScale();
        }

        private void OnApplicationQuit()
        {
            BDGameSettings.Save();
        }

        private void Update()
        {
            if (reloadOperation != null)
                return;

            if (Time.unscaledTime >=
                nextBindingRefreshAt)
            {
                nextBindingRefreshAt =
                    Time.unscaledTime + 1f;

                RefreshRuntimeBindings();
            }

            if (!enableEscapePause ||
                !runActive ||
                resultSequenceActive)
            {
                return;
            }

            if (!ReadEscapePressed())
                return;

            if (mode == OverlayMode.Settings)
            {
                mode = pausedFromGameplay
                    ? OverlayMode.Pause
                    : OverlayMode.MainMenu;
                return;
            }

            if (mode == OverlayMode.Pause)
                ResumeGame();
            else
                PauseGame();
        }

        public bool HandlePlayerDeath()
        {
            if (!runActive)
                return false;

            // A narrative sequence owns the transition. Suppress old
            // Died listeners and wait for the cutscene's finish signal.
            if (resultSequenceActive)
                return true;

            ShowPlainMainMenu(
                needsReload: true
            );

            return true;
        }

        public void BeginResultSequence()
        {
            if (!runActive)
                return;

            resultSequenceActive = true;
            pausedFromGameplay = false;
            RestoreTimeScale();
        }

        public void ReturnToMainMenuAfterSequence()
        {
            resultSequenceActive = false;

            ShowPlainMainMenu(
                needsReload: true
            );
        }

        public void CompleteMotherVictorySequence()
        {
            BDGameProgress.MarkMotherDefeated();
            resultSequenceActive = false;

            ShowPlainMainMenu(
                needsReload: true
            );
        }

        // Compatibility APIs keep the main menu visually unchanged.
        public void ShowVictory()
        {
            CompleteMotherVictorySequence();
        }

        public void ShowDefeat()
        {
            ReturnToMainMenuAfterSequence();
        }

        public void ReturnToMainMenu()
        {
            resultSequenceActive = false;

            ShowPlainMainMenu(
                needsReload: runActive
            );
        }

        private IEnumerator AutoStartAfterSceneReload()
        {
            yield return null;
            BeginCurrentRun();
        }

        private void BeginCurrentRun()
        {
            runNeedsReload = false;
            resultSequenceActive = false;
            pausedFromGameplay = false;
            mode = OverlayMode.MainMenu;
            runActive = true;

            RestoreTimeScale();
            RefreshRuntimeBindings();
            SetPlayerControlEnabled(true);
        }

        private void StartGamePressed()
        {
            if (reloadOperation != null)
                return;

            if (!runNeedsReload)
            {
                BeginCurrentRun();
                return;
            }

            StartCoroutine(
                ReloadAndStartRoutine()
            );
        }

        private IEnumerator ReloadAndStartRoutine()
        {
            mode = OverlayMode.Loading;
            autoStartAfterReload = true;
            RestoreTimeScale();

            Scene scene =
                SceneManager.GetActiveScene();

            if (scene.buildIndex >= 0)
            {
                reloadOperation =
                    SceneManager.LoadSceneAsync(
                        scene.buildIndex
                    );
            }
            else
            {
                reloadOperation =
                    SceneManager.LoadSceneAsync(
                        scene.name
                    );
            }

            if (reloadOperation == null)
            {
                autoStartAfterReload = false;
                reloadOperation = null;

                ShowPlainMainMenu(
                    needsReload: true
                );

                yield break;
            }

            while (!reloadOperation.isDone)
                yield return null;
        }

        private void ShowPlainMainMenu(
            bool needsReload)
        {
            runNeedsReload = needsReload;
            runActive = false;
            pausedFromGameplay = false;
            mode = OverlayMode.MainMenu;

            Time.timeScale = 0f;
            AudioListener.pause = false;
            SetPlayerControlEnabled(false);

            Cursor.visible = true;
            Cursor.lockState =
                CursorLockMode.None;
        }

        private void PauseGame()
        {
            if (!runActive)
                return;

            pausedFromGameplay = true;
            mode = OverlayMode.Pause;
            Time.timeScale = 0f;
            SetPlayerControlEnabled(false);

            Cursor.visible = true;
            Cursor.lockState =
                CursorLockMode.None;
        }

        private void ResumeGame()
        {
            if (!runActive)
                return;

            pausedFromGameplay = false;
            mode = OverlayMode.MainMenu;
            RestoreTimeScale();
            SetPlayerControlEnabled(true);
        }

        private void OpenSettings(
            bool fromPause)
        {
            pausedFromGameplay = fromPause;
            mode = OverlayMode.Settings;
        }

        private void CloseSettings()
        {
            BDGameSettings.Save();

            mode = pausedFromGameplay
                ? OverlayMode.Pause
                : OverlayMode.MainMenu;
        }

        private void RefreshRuntimeBindings()
        {
            BDPlayerMarker marker =
                Object.FindFirstObjectByType<
                    BDPlayerMarker>();

            BDPlayerController nextController =
                marker != null
                    ? marker.GetComponent<
                        BDPlayerController>()
                    : null;

            if (nextController ==
                playerController)
            {
                return;
            }

            playerController =
                nextController;

            playerControllerWasEnabled =
                playerController != null &&
                playerController.enabled;
        }

        private void SetPlayerControlEnabled(
            bool enabled)
        {
            if (playerController == null)
                RefreshRuntimeBindings();

            if (playerController == null)
                return;

            if (enabled)
            {
                playerController.enabled =
                    playerControllerWasEnabled;
            }
            else
            {
                playerControllerWasEnabled =
                    playerController.enabled;

                playerController.enabled = false;
            }
        }

        private static void RestoreTimeScale()
        {
            Time.timeScale = 1f;
            AudioListener.pause = false;
        }

        private static bool ReadEscapePressed()
        {
#if ENABLE_INPUT_SYSTEM
            return Keyboard.current != null &&
                   Keyboard.current.escapeKey
                       .wasPressedThisFrame;
#else
            return Input.GetKeyDown(
                KeyCode.Escape
            );
#endif
        }
        private Texture2D CreateMenuTexture(
            Color color)
        {
            Texture2D texture =
                new Texture2D(
                    1,
                    1,
                    TextureFormat.RGBA32,
                    mipChain: false
                );

            texture.SetPixel(
                0,
                0,
                color
            );

            texture.Apply(
                updateMipmaps: false,
                makeNoLongerReadable: true
            );

            return texture;
        }

        private void DestroyMenuTextures()
        {
            Texture2D[] textures =
            {
                dreamyTransparentTexture,
                dreamyPanelTexture,
                dreamyButtonTexture,
                dreamyButtonHoverTexture,
                dreamyButtonActiveTexture
            };

            for (int index = 0;
                 index < textures.Length;
                 index++)
            {
                if (textures[index] != null)
                    Destroy(textures[index]);
            }

            dreamyTransparentTexture = null;
            dreamyPanelTexture = null;
            dreamyButtonTexture = null;
            dreamyButtonHoverTexture = null;
            dreamyButtonActiveTexture = null;
        }


        private void OnGUI()
        {
            if (runActive &&
                !pausedFromGameplay &&
                mode != OverlayMode.Settings)
            {
                return;
            }

            EnsureStyles();

            Matrix4x4 previousMatrix =
                GUI.matrix;

            float scale = Mathf.Max(
                0.55f,
                Mathf.Min(
                    Screen.width / 1280f,
                    Screen.height / 720f
                )
            );

            float virtualWidth =
                Screen.width / scale;

            float virtualHeight =
                Screen.height / scale;

            GUI.matrix =
                Matrix4x4.Scale(
                    new Vector3(
                        scale,
                        scale,
                        1f
                    )
                );

            GUI.Box(
                new Rect(
                    0f,
                    0f,
                    virtualWidth,
                    virtualHeight
                ),
                GUIContent.none,
                overlayStyle
            );

            Rect panelRect = new Rect(
                (virtualWidth - 560f) * 0.5f,
                (virtualHeight - 600f) * 0.5f,
                560f,
                600f
            );

            GUILayout.BeginArea(
                panelRect,
                panelStyle
            );

            if (mode == OverlayMode.Settings)
                DrawSettings();
            else if (mode == OverlayMode.Pause)
                DrawPause();
            else if (mode == OverlayMode.Loading)
                DrawLoading();
            else
                DrawMainMenu();

            GUILayout.EndArea();

            GUI.matrix = previousMatrix;
        }
        private void DrawMainMenu()
        {
            GUILayout.Space(20f);

            GUILayout.Label(
                gameTitle,
                titleStyle
            );

            GUILayout.Space(8f);

            GUILayout.Label(
                gameSubtitle,
                subtitleStyle
            );

            GUILayout.Space(12f);
            DrawDreamyMenuOrnament();

            if (BDGameProgress.MotherDefeated)
            {
                GUILayout.Space(6f);
                DrawCompletedGameBoyRelic();
                GUILayout.Space(6f);
            }
            else
            {
                GUILayout.Space(34f);
            }



            Color previousButtonTint =
                GUI.backgroundColor;

            GUI.backgroundColor =
                StartGameHighlightTint;

            bool startGamePressed =
                GUILayout.Button(
                    "START GAME",
                    buttonStyle,
                    GUILayout.Height(60f));

            GUI.backgroundColor =
                previousButtonTint;

            if (startGamePressed)
                StartGamePressed();

            GUILayout.Space(14f);

            if (GUILayout.Button(
                    "SETTINGS",
                    buttonStyle,
                    GUILayout.Height(52f)))
            {
                OpenSettings(
                    fromPause: false
                );
            }

#if !UNITY_IOS && !UNITY_ANDROID && !UNITY_WEBGL
            if (showQuitButtonOnDesktop)
            {
                GUILayout.Space(14f);

                if (GUILayout.Button(
                        "QUIT",
                        buttonStyle,
                        GUILayout.Height(46f)))
                {
                    BDGameSettings.Save();
                    Application.Quit();
                }
            }
#endif

            GUILayout.FlexibleSpace();

            DrawDreamyMenuOrnament();

            GUILayout.Space(8f);

            GUILayout.Label(
                "Settings are saved automatically",
                valueStyle
            );

            GUILayout.Space(12f);
        }

        private void DrawCompletedGameBoyRelic()
        {
            Rect area =
                GUILayoutUtility.GetRect(
                    180f,
                    106f,
                    GUILayout.ExpandWidth(true)
                );

            float centerX = area.center.x;
            float top = area.y + 2f;
            float pulse =
                0.5f +
                0.5f *
                Mathf.Sin(
                    Time.unscaledTime * 2.2f
                );

            Color previousColor = GUI.color;

            Color glow = Color.HSVToRGB(
                Mathf.Repeat(
                    Time.unscaledTime * 0.06f,
                    1f
                ),
                0.72f,
                1f
            );

            GUI.color =
                new Color(
                    glow.r,
                    glow.g,
                    glow.b,
                    0.10f + 0.08f * pulse
                );

            GUI.DrawTexture(
                new Rect(
                    centerX - 58f,
                    top - 5f,
                    116f,
                    112f
                ),
                Texture2D.whiteTexture
            );

            GUI.color =
                new Color(
                    0.65f,
                    0.68f,
                    0.72f,
                    1f
                );

            GUI.DrawTexture(
                new Rect(
                    centerX - 37f,
                    top,
                    74f,
                    100f
                ),
                Texture2D.whiteTexture
            );

            GUI.color =
                new Color(
                    0.11f,
                    0.13f,
                    0.16f,
                    1f
                );

            GUI.DrawTexture(
                new Rect(
                    centerX - 28f,
                    top + 10f,
                    56f,
                    40f
                ),
                Texture2D.whiteTexture
            );

            GUI.color =
                new Color(
                    glow.r,
                    glow.g,
                    glow.b,
                    0.72f + 0.20f * pulse
                );

            GUI.DrawTexture(
                new Rect(
                    centerX - 23f,
                    top + 15f,
                    46f,
                    30f
                ),
                Texture2D.whiteTexture
            );

            // A seated cartridge: the game is present, not missing.
            GUI.color =
                new Color(
                    0.30f,
                    0.31f,
                    0.36f,
                    1f
                );

            GUI.DrawTexture(
                new Rect(
                    centerX - 18f,
                    top - 8f,
                    36f,
                    14f
                ),
                Texture2D.whiteTexture
            );

            // D-pad.
            GUI.color =
                new Color(
                    0.12f,
                    0.12f,
                    0.14f,
                    1f
                );

            GUI.DrawTexture(
                new Rect(
                    centerX - 23f,
                    top + 62f,
                    22f,
                    8f
                ),
                Texture2D.whiteTexture
            );

            GUI.DrawTexture(
                new Rect(
                    centerX - 16f,
                    top + 55f,
                    8f,
                    22f
                ),
                Texture2D.whiteTexture
            );

            // Two game buttons.
            GUI.color =
                new Color(
                    0.42f,
                    0.08f,
                    0.18f,
                    1f
                );

            GUI.DrawTexture(
                new Rect(
                    centerX + 8f,
                    top + 60f,
                    10f,
                    10f
                ),
                Texture2D.whiteTexture
            );

            GUI.DrawTexture(
                new Rect(
                    centerX + 22f,
                    top + 54f,
                    10f,
                    10f
                ),
                Texture2D.whiteTexture
            );

            // A few silent colored pixels connect the relic to
            // the colored-light ending without adding any words.
            for (int index = 0;
                 index < 4;
                 index++)
            {
                float phase =
                    Time.unscaledTime *
                    (0.75f + index * 0.12f) +
                    index * 1.7f;

                float x =
                    centerX - 62f +
                    index * 41f +
                    Mathf.Sin(phase) * 5f;

                float y =
                    top + 8f +
                    Mathf.Repeat(
                        phase * 12f,
                        70f
                    );

                GUI.color =
                    new Color(
                        glow.r,
                        glow.g,
                        glow.b,
                        0.35f
                    );

                GUI.DrawTexture(
                    new Rect(
                        x,
                        y,
                        4f,
                        4f
                    ),
                    Texture2D.whiteTexture
                );
            }

            GUI.color = previousColor;
        }
        private void DrawDreamyMenuOrnament()
        {
            Rect rect =
                GUILayoutUtility.GetRect(
                    330f,
                    12f,
                    GUILayout.ExpandWidth(true)
                );

            float center = rect.center.x;
            Color previous = GUI.color;

            GUI.color = new Color(
                0.91f,
                0.72f,
                0.31f,
                0.62f
            );

            GUI.DrawTexture(
                new Rect(
                    center - 132f,
                    rect.center.y,
                    108f,
                    1f
                ),
                dreamyButtonHoverTexture
            );

            GUI.DrawTexture(
                new Rect(
                    center + 24f,
                    rect.center.y,
                    108f,
                    1f
                ),
                dreamyButtonHoverTexture
            );

            GUI.DrawTexture(
                new Rect(
                    center - 3f,
                    rect.center.y - 3f,
                    6f,
                    6f
                ),
                dreamyButtonHoverTexture
            );

            GUI.color = previous;
        }


        private void DrawPause()
        {
            GUILayout.Space(30f);

            GUILayout.Label(
                "PAUSED",
                titleStyle
            );

            GUILayout.FlexibleSpace();

            if (GUILayout.Button(
                    "RESUME",
                    buttonStyle,
                    GUILayout.Height(58f)))
            {
                ResumeGame();
            }

            GUILayout.Space(14f);

            if (GUILayout.Button(
                    "SETTINGS",
                    buttonStyle,
                    GUILayout.Height(52f)))
            {
                OpenSettings(
                    fromPause: true
                );
            }

            GUILayout.Space(14f);

            if (GUILayout.Button(
                    "MAIN MENU",
                    buttonStyle,
                    GUILayout.Height(52f)))
            {
                ReturnToMainMenu();
            }

            GUILayout.FlexibleSpace();
            GUILayout.Space(24f);
        }

        private void DrawLoading()
        {
            GUILayout.FlexibleSpace();

            GUILayout.Label(
                "STARTING...",
                titleStyle
            );

            GUILayout.Space(18f);

            float progress =
                reloadOperation != null
                    ? Mathf.Clamp01(
                        reloadOperation.progress /
                        0.9f
                    )
                    : 0f;

            GUILayout.HorizontalSlider(
                progress,
                0f,
                1f
            );

            GUILayout.FlexibleSpace();
        }

        private void DrawSettings()
        {
            GUILayout.Space(18f);

            GUILayout.Label(
                "SETTINGS",
                titleStyle
            );

            GUILayout.Space(12f);

            DrawCycleRow(
                "Graphics Quality",
                BDGameSettings.QualityName,
                () =>
                    BDGameSettings.CycleQuality(-1),
                () =>
                    BDGameSettings.CycleQuality(1)
            );

#if !UNITY_IOS && !UNITY_ANDROID
            DrawToggleRow(
                "Fullscreen",
                BDGameSettings.Fullscreen,
                BDGameSettings.SetFullscreen
            );
#endif

            DrawToggleRow(
                "VSync",
                BDGameSettings.VSync,
                BDGameSettings.SetVSync
            );

            DrawCycleRow(
                "Target FPS",
                BDGameSettings.TargetFpsLabel,
                () =>
                    BDGameSettings.CycleTargetFps(-1),
                () =>
                    BDGameSettings.CycleTargetFps(1)
            );

            DrawSliderRow(
                "Master Volume",
                BDGameSettings.MasterVolume,
                0f,
                1f,
                BDGameSettings.SetMasterVolume,
                showPercent: true
            );

            DrawSliderRow(
                "Music Volume",
                BDGameSettings.MusicVolume,
                0f,
                1f,
                BDGameSettings.SetMusicVolume,
                showPercent: true
            );

            DrawSliderRow(
                "SFX Volume",
                BDGameSettings.SfxVolume,
                0f,
                1f,
                BDGameSettings.SetSfxVolume,
                showPercent: true
            );

            DrawSliderRow(
                "Mouse Sensitivity",
                BDGameSettings
                    .MouseSensitivityMultiplier,
                0.45f,
                1.65f,
                BDGameSettings.SetMouseSensitivity,
                showPercent: false
            );

            DrawSliderRow(
                "Camera Shake",
                BDGameSettings.CameraShakeIntensity,
                0f,
                1f,
                BDGameSettings.SetCameraShake,
                showPercent: true
            );

            GUILayout.FlexibleSpace();

            GUILayout.BeginHorizontal();

            if (GUILayout.Button(
                    "RESET DEFAULTS",
                    smallButtonStyle,
                    GUILayout.Height(42f)))
            {
                BDGameSettings.ResetDefaults();
            }

            GUILayout.Space(12f);

            if (GUILayout.Button(
                    "BACK",
                    smallButtonStyle,
                    GUILayout.Height(42f)))
            {
                CloseSettings();
            }

            GUILayout.EndHorizontal();

            GUILayout.Space(18f);
        }

        private void DrawCycleRow(
            string label,
            string value,
            System.Action previous,
            System.Action next)
        {
            GUILayout.BeginHorizontal();

            GUILayout.Label(
                label,
                sectionStyle,
                GUILayout.Width(240f)
            );

            if (GUILayout.Button(
                    "<",
                    smallButtonStyle,
                    GUILayout.Width(48f),
                    GUILayout.Height(36f)))
            {
                previous?.Invoke();
            }

            GUILayout.Label(
                value,
                valueStyle,
                GUILayout.Width(150f)
            );

            if (GUILayout.Button(
                    ">",
                    smallButtonStyle,
                    GUILayout.Width(48f),
                    GUILayout.Height(36f)))
            {
                next?.Invoke();
            }

            GUILayout.EndHorizontal();
            GUILayout.Space(8f);
        }

        private void DrawToggleRow(
            string label,
            bool value,
            System.Action<bool> setter)
        {
            GUILayout.BeginHorizontal();

            GUILayout.Label(
                label,
                sectionStyle,
                GUILayout.Width(360f)
            );

            string buttonText =
                value ? "ON" : "OFF";

            if (GUILayout.Button(
                    buttonText,
                    smallButtonStyle,
                    GUILayout.Width(126f),
                    GUILayout.Height(36f)))
            {
                setter?.Invoke(!value);
            }

            GUILayout.EndHorizontal();
            GUILayout.Space(8f);
        }

        private void DrawSliderRow(
            string label,
            float value,
            float minimum,
            float maximum,
            System.Action<float> setter,
            bool showPercent)
        {
            GUILayout.BeginHorizontal();

            GUILayout.Label(
                label,
                sectionStyle,
                GUILayout.Width(205f)
            );

            float nextValue =
                GUILayout.HorizontalSlider(
                    value,
                    minimum,
                    maximum,
                    GUILayout.Width(220f)
                );

            string valueText =
                showPercent
                    ? Mathf.RoundToInt(
                        nextValue * 100f
                    ) + "%"
                    : nextValue.ToString("0.00");

            GUILayout.Label(
                valueText,
                valueStyle,
                GUILayout.Width(70f)
            );

            GUILayout.EndHorizontal();
            GUILayout.Space(8f);

            if (!Mathf.Approximately(
                    nextValue,
                    value))
            {
                setter?.Invoke(nextValue);
            }
        }
        private void EnsureStyles()
        {
            if (titleStyle != null)
                return;

            dreamyTransparentTexture =
                CreateMenuTexture(
                    new Color(
                        0f,
                        0f,
                        0f,
                        0.055f
                    )
                );

            dreamyPanelTexture =
                CreateMenuTexture(
                    new Color(
                        0.035f,
                        0.055f,
                        0.13f,
                        0.90f
                    )
                );

            dreamyButtonTexture =
                CreateMenuTexture(
                    new Color(
                        0.070f,
                        0.115f,
                        0.24f,
                        0.98f
                    )
                );

            dreamyButtonHoverTexture =
                CreateMenuTexture(
                    new Color(
                        0.105f,
                        0.175f,
                        0.34f,
                        1f
                    )
                );

            dreamyButtonActiveTexture =
                CreateMenuTexture(
                    new Color(
                        0.050f,
                        0.085f,
                        0.18f,
                        1f
                    )
                );

            titleStyle =
                new GUIStyle(
                    GUI.skin.label)
                {
                    alignment =
                        TextAnchor.MiddleCenter,
                    font =
                        GUI.skin.label.font,
                    fontSize = 39,
                    fontStyle =
                        FontStyle.Bold,
                    clipping =
                        TextClipping.Overflow,
                    normal =
                    {
                        textColor =
                            new Color(
                                1f,
                                0.94f,
                                0.79f,
                                1f
                            )
                    }
                };

            subtitleStyle =
                new GUIStyle(
                    GUI.skin.label)
                {
                    alignment =
                        TextAnchor.MiddleCenter,
                    font =
                        GUI.skin.label.font,
                    fontSize = 18,
                    fontStyle =
                        FontStyle.Bold,
                    normal =
                    {
                        textColor =
                            new Color(
                                0.72f,
                                0.82f,
                                1f,
                                0.94f
                            )
                    }
                };

            sectionStyle =
                new GUIStyle(
                    GUI.skin.label)
                {
                    alignment =
                        TextAnchor.MiddleLeft,
                    font =
                        GUI.skin.label.font,
                    fontSize = 18,
                    normal =
                    {
                        textColor =
                            new Color(
                                0.94f,
                                0.94f,
                                0.94f,
                                1f
                            )
                    }
                };

            valueStyle =
                new GUIStyle(
                    GUI.skin.label)
                {
                    alignment =
                        TextAnchor.MiddleCenter,
                    font =
                        GUI.skin.label.font,
                    fontSize = 15,
                    normal =
                    {
                        textColor =
                            new Color(
                                0.66f,
                                0.74f,
                                0.90f,
                                0.82f
                            )
                    }
                };

            buttonStyle =
                new GUIStyle(
                    GUI.skin.button)
                {
                    alignment =
                        TextAnchor.MiddleCenter,
                    font =
                        GUI.skin.button.font,
                    fontSize = 21,
                    fontStyle =
                        FontStyle.Bold,
                    normal =
                    {
                        background =
                            dreamyButtonTexture,
                        textColor =
                            new Color(
                                1f,
                                0.94f,
                                0.78f,
                                1f
                            )
                    },
                    hover =
                    {
                        background =
                            dreamyButtonHoverTexture,
                        textColor =
                            Color.white
                    },
                    active =
                    {
                        background =
                            dreamyButtonActiveTexture,
                        textColor =
                            new Color(
                                1f,
                                0.86f,
                                0.56f,
                                1f
                            )
                    }
                };

            smallButtonStyle =
                new GUIStyle(
                    buttonStyle)
                {
                    fontSize = 15
                };

            panelStyle =
                new GUIStyle(
                    GUI.skin.box)
                {
                    padding =
                        new RectOffset(
                            30,
                            30,
                            20,
                            20
                        ),
                    normal =
                    {
                        background =
                            dreamyPanelTexture
                    }
                };

            overlayStyle =
                new GUIStyle(
                    GUI.skin.box)
                {
                    normal =
                    {
                        background =
                            dreamyTransparentTexture
                    }
                };
        }

    }
}
