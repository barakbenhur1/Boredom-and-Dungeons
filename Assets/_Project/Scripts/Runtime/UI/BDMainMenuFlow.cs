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
        public enum HandheldPage
        {
            MainMenu,
            Settings,
            Progression,
            Pause,
            AbandonConfirm,
            Loading
        }

        private enum OverlayMode
        {
            MainMenu,
            Settings,
            Progression,
            Pause,
            AbandonConfirm,
            Loading
        }

        // BD ACTION-AWARE MENU BUTTONS V7
        private enum MenuActionVisual
        {
            Progress,
            Settings,
            Leave,
            Neutral
        }

        private static bool autoStartAfterReload;
        private static bool showMainMenuAfterReload;

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
        private Coroutine playerDeathRoutine;

        private OverlayMode lastPresentedMode;
        private bool modeTransitionInitialized;
        private float modeTransitionStartedAt;
        private const float ModeTransitionDuration = 0.18f;

        private GUIStyle titleStyle;
        private GUIStyle subtitleStyle;
        private GUIStyle sectionStyle;
        private GUIStyle valueStyle;
        private GUIStyle buttonStyle;
        private GUIStyle smallButtonStyle;
        private GUIStyle panelStyle;
        private GUIStyle overlayStyle;
        private GUIStyle sliderStyle;
        private GUIStyle sliderThumbStyle;

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
        public bool IsGameplayHudVisible =>
            runActive &&
            !pausedFromGameplay &&
            mode != OverlayMode.Settings &&
            mode != OverlayMode.Progression &&
            mode != OverlayMode.Loading &&
            !resultSequenceActive;
        public bool IsMenuOverlayVisible =>
            !IsGameplayHudVisible;

        public bool ShouldPresentModernHandheld =>
            IsMenuOverlayVisible &&
            !BDBBHBootIntro.IsPlaying;

        public HandheldPage CurrentHandheldPage
        {
            get
            {
                switch (mode)
                {
                    case OverlayMode.Settings:
                        return HandheldPage.Settings;
                    case OverlayMode.Progression:
                        return HandheldPage.Progression;
                    case OverlayMode.Pause:
                        return HandheldPage.Pause;
                    case OverlayMode.AbandonConfirm:
                        return HandheldPage.AbandonConfirm;
                    case OverlayMode.Loading:
                        return HandheldPage.Loading;
                    default:
                        return HandheldPage.MainMenu;
                }
            }
        }

        public bool IsPausedFromGameplay => pausedFromGameplay;
        public string PrimaryRunActionLabel =>
            runNeedsReload ? "NEW RUN" : "START GAME";
        public float ReloadProgress =>
            reloadOperation != null
                ? Mathf.Clamp01(reloadOperation.progress / 0.9f)
                : 0f;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ResetStaticState()
        {
            // BD MAIN MENU STATIC FLAGS RESET V23R19G
            // Reset only at the beginning of a Play session. Scene reloads
            // intentionally preserve the one-shot destination flags.
            autoStartAfterReload = false;
            showMainMenuAfterReload = false;
            Instance = null;
        }

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

            if (showMainMenuAfterReload)
            {
                // BD REAL ABANDON-TO-MAIN-MENU RELOAD V23R19G
                // A confirmed abandon owns a clean scene reload. The main menu
                // is never drawn as a popup over the abandoned live run.
                showMainMenuAfterReload = false;
                autoStartAfterReload = false;
                ShowPlainMainMenu(needsReload: false);
                return;
            }

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

            if (BDModernHandheld3DPresenter.OwnsMenuInput)
                return;

            if (!enableEscapePause ||
                !runActive ||
                resultSequenceActive ||
                BDRunPresentationCoordinator.InputLocked)
            {
                return;
            }

            if (!ReadEscapePressed())
                return;

            if (mode == OverlayMode.Settings ||
                mode == OverlayMode.Progression)
            {
                mode = pausedFromGameplay
                    ? OverlayMode.Pause
                    : OverlayMode.MainMenu;
                return;
            }

            if (mode == OverlayMode.AbandonConfirm)
            {
                mode = OverlayMode.Pause;
                return;
            }

            if (mode == OverlayMode.Pause)
                ResumeGame();
            else
                PauseGame();
        }

        public bool HandlePlayerDeath()
        {
            return HandlePlayerDeath(null);
        }

        public bool HandlePlayerDeath(BDHealth playerHealth)
        {
            if (!runActive)
                return false;

            if (resultSequenceActive || playerDeathRoutine != null)
                return true;

            playerDeathRoutine = StartCoroutine(
                PresentPlayerDeathThenMenu(playerHealth)
            );
            return true;
        }

        private IEnumerator PresentPlayerDeathThenMenu(
            BDHealth playerHealth)
        {
            // BD PLAYER DEATH BEFORE MENU V23R19E
            // Keep the world visible and unpaused long enough to show the
            // player's death animation. Only then may the Game Boy menu cover it.
            resultSequenceActive = true;
            pausedFromGameplay = false;
            RestoreTimeScale();
            SetPlayerControlEnabled(false);

            BDParrySystem.Reset();
            BDMeleeSlashArcVisual.ClearAllActive();
            BDPlayerAirborneAttackAnimation.CancelAllActive();

            float duration =
                BDCharacterDeathAnimation.PlayPlayerDeath(playerHealth);
            float remaining = Mathf.Max(1.65f, duration);

            // BD READABLE PLAYER DEATH BEFORE MENU V23R19G
            // Keep the gameplay view unobscured for the complete pose and a
            // short readable hold; only then may the menu take ownership.
            while (remaining > 0f ||
                   BDCharacterDeathAnimation.IsDeathPresentationActive(
                       playerHealth))
            {
                remaining -= Time.unscaledDeltaTime;
                yield return null;
            }

            float finalReadHold = 0.18f;
            while (finalReadHold > 0f)
            {
                finalReadHold -= Time.unscaledDeltaTime;
                yield return null;
            }

            BDRunPresentationCoordinator.MarkDeathRestartWithoutIntro();
            resultSequenceActive = false;
            playerDeathRoutine = null;

            ShowPlainMainMenu(
                needsReload: true
            );
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
            BDRunPresentationCoordinator.MarkCinematicSeen();
            resultSequenceActive = false;

            ShowPlainMainMenu(
                needsReload: true
            );
        }

        public void CompleteMotherVictorySequence()
        {
            BDRunPresentationCoordinator.MarkCinematicSeen();
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
            if (runActive)
                BDRunPresentationCoordinator.MarkNextRunAsFreshOrVictoryIntro();

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
            if (!BDRunPresentationCoordinator.HoldGameplayControlOnRunStart)
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
            // Clear transient combat presentation before the menu takes
            // ownership. A death or interrupted combat frame must not leave
            // Parry/slash visuals or frozen behaviours behind.
            BDParrySystem.Reset();
            BDMeleeSlashArcVisual.ClearAllActive();
            BDPlayerAirborneAttackAnimation.CancelAllActive();

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
            AudioListener.pause = true;
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

        private void OpenProgression(bool fromPause)
        {
            pausedFromGameplay = fromPause;
            mode = OverlayMode.Progression;
        }

        private void CloseProgression()
        {
            mode = pausedFromGameplay
                ? OverlayMode.Pause
                : OverlayMode.MainMenu;
        }

        public void HandleModernPrimaryAction()
        {
            if (mode == OverlayMode.Pause)
                ResumeGame();
            else if (mode == OverlayMode.AbandonConfirm)
                BeginConfirmedAbandonToMainMenu();
            else if (mode == OverlayMode.MainMenu)
                StartGamePressed();
        }

        public void HandleModernOpenSettings()
        {
            if (mode == OverlayMode.Loading ||
                mode == OverlayMode.AbandonConfirm)
            {
                return;
            }

            OpenSettings(runActive || pausedFromGameplay);
        }

        public void HandleModernOpenProgression()
        {
            if (mode == OverlayMode.Loading ||
                mode == OverlayMode.AbandonConfirm)
            {
                return;
            }

            OpenProgression(runActive || pausedFromGameplay);
        }

        public void HandleModernBack()
        {
            switch (mode)
            {
                case OverlayMode.Settings:
                    CloseSettings();
                    break;
                case OverlayMode.Progression:
                    CloseProgression();
                    break;
                case OverlayMode.AbandonConfirm:
                    mode = OverlayMode.Pause;
                    break;
                case OverlayMode.Pause:
                    ResumeGame();
                    break;
            }
        }

        public void HandleModernRequestMainMenu()
        {
            if (!runActive)
                return;

            mode = OverlayMode.AbandonConfirm;
        }

        public void HandleModernCancelAbandon()
        {
            if (mode == OverlayMode.AbandonConfirm)
                mode = OverlayMode.Pause;
        }

        public void HandleModernQuit()
        {
#if !UNITY_IOS && !UNITY_ANDROID && !UNITY_WEBGL
            if (!showQuitButtonOnDesktop || runActive)
                return;

            BDGameSettings.Save();
            Application.Quit();
#endif
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
            if (BDModernHandheld3DPresenter.SuppressLegacyMenu)
                return;

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

            // BD PROFESSIONAL HANDHELD SCREEN MENU V23R19Q
            // The physical shell and all page content remain one ordered IMGUI
            // composition. Only the screen content receives the short page move.
            float transition = ResolveModeTransition();

            BDGameBoyMenuShell.DrawIntegrated(
                panelRect,
                virtualWidth,
                virtualHeight,
                ResolveScreenLabel()
            );

            Rect animatedPanelRect = panelRect;
            animatedPanelRect.y +=
                (1f - transition) * 11f;

            Color previousContentColor = GUI.color;
            GUI.color = new Color(
                previousContentColor.r,
                previousContentColor.g,
                previousContentColor.b,
                previousContentColor.a * transition
            );

            GUILayout.BeginArea(
                animatedPanelRect,
                panelStyle
            );

            if (mode == OverlayMode.Settings)
                DrawSettings();
            else if (mode == OverlayMode.Progression)
                DrawProgression();
            else if (mode == OverlayMode.Pause)
                DrawPause();
            else if (mode == OverlayMode.AbandonConfirm)
                DrawAbandonConfirmation();
            else if (mode == OverlayMode.Loading)
                DrawLoading();
            else
                DrawMainMenu();

            GUILayout.EndArea();
            GUI.color = previousContentColor;

            BDGameBoyMenuShell.DrawScreenOverlay(
                panelRect,
                virtualWidth,
                virtualHeight
            );

            GUI.matrix = previousMatrix;
        }
        private float ResolveModeTransition()
        {
            if (!modeTransitionInitialized)
            {
                modeTransitionInitialized = true;
                lastPresentedMode = mode;
                modeTransitionStartedAt = Time.unscaledTime;
            }
            else if (lastPresentedMode != mode)
            {
                lastPresentedMode = mode;
                modeTransitionStartedAt = Time.unscaledTime;
            }

            float raw =
                (Time.unscaledTime - modeTransitionStartedAt) /
                Mathf.Max(0.01f, ModeTransitionDuration);
            float t = Mathf.Clamp01(raw);
            return t * t * (3f - 2f * t);
        }

        private string ResolveScreenLabel()
        {
            switch (mode)
            {
                case OverlayMode.Settings:
                    return "SYSTEM // SETTINGS";
                case OverlayMode.Progression:
                    return "MEMORY // PROGRESSION";
                case OverlayMode.Pause:
                    return "RUN // PAUSED";
                case OverlayMode.AbandonConfirm:
                    return "RUN // CONFIRM";
                case OverlayMode.Loading:
                    return "MEMORY // LOADING";
                default:
                    return BDGameProgress.MotherDefeated
                        ? "AWAKENED // MAIN"
                        : "MEMORY // MAIN";
            }
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
            if (DrawActionButton(
                    "START GAME",
                    MenuActionVisual.Progress,
                    60f))
            {
                StartGamePressed();
            }

            GUILayout.Space(14f);

            if (DrawActionButton(
                    "PROGRESSION",
                    MenuActionVisual.Neutral,
                    52f))
            {
                OpenProgression(fromPause: false);
            }

            GUILayout.Space(14f);

            if (DrawActionButton(
                    "SETTINGS",
                    MenuActionVisual.Settings,
                    52f))
            {
                OpenSettings(
                    fromPause: false
                );
            }

#if !UNITY_IOS && !UNITY_ANDROID && !UNITY_WEBGL
            if (showQuitButtonOnDesktop)
            {
                GUILayout.Space(14f);

                if (DrawActionButton(
                        "QUIT",
                        MenuActionVisual.Leave,
                        46f))
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
        public void ReleaseControlAfterRunPresentation(
            bool playerIsMounted)
        {
            if (!runActive || pausedFromGameplay || resultSequenceActive)
                return;

            if (!playerIsMounted)
                SetPlayerControlEnabled(true);
        }

        private bool DrawActionButton(
            string label,
            MenuActionVisual visual,
            float height)
        {
            Rect rect = GUILayoutUtility.GetRect(
                GUIContent.none,
                buttonStyle,
                GUILayout.Height(height),
                GUILayout.ExpandWidth(true)
            );

            bool hovered =
                Event.current != null &&
                rect.Contains(Event.current.mousePosition);

            Color tint = ResolveActionTint(visual);
            Texture2D background = hovered
                ? dreamyButtonHoverTexture
                : dreamyButtonTexture;

            Color previousColor = GUI.color;
            GUI.color = Color.white;
            GUI.DrawTexture(rect, background);

            if (hovered)
            {
                float pulse =
                    0.70f +
                    0.30f *
                    Mathf.Sin(Time.unscaledTime * 4.6f);

                DrawMenuRect(
                    new Rect(
                        rect.x,
                        rect.y + 5f,
                        4f,
                        rect.height - 10f
                    ),
                    new Color(
                        tint.r,
                        tint.g,
                        tint.b,
                        0.78f + 0.18f * pulse
                    )
                );

                DrawMenuBorder(
                    rect,
                    new Color(
                        tint.r,
                        tint.g,
                        tint.b,
                        0.44f
                    ),
                    1f
                );
            }
            else
            {
                DrawMenuBorder(
                    rect,
                    new Color(
                        tint.r,
                        tint.g,
                        tint.b,
                        0.20f
                    ),
                    1f
                );
            }

            bool pressed = GUI.Button(
                rect,
                ResolveActionPrefix(visual) + "  " + label,
                buttonStyle
            );

            GUI.color = previousColor;
            return pressed;
        }

        private void DrawMenuBorder(
            Rect rect,
            Color color,
            float thickness)
        {
            DrawMenuRect(
                new Rect(rect.x, rect.y, rect.width, thickness),
                color
            );
            DrawMenuRect(
                new Rect(
                    rect.x,
                    rect.yMax - thickness,
                    rect.width,
                    thickness
                ),
                color
            );
            DrawMenuRect(
                new Rect(rect.x, rect.y, thickness, rect.height),
                color
            );
            DrawMenuRect(
                new Rect(
                    rect.xMax - thickness,
                    rect.y,
                    thickness,
                    rect.height
                ),
                color
            );
        }

        private void DrawMenuRect(Rect rect, Color color)
        {
            if (rect.width <= 0f || rect.height <= 0f)
                return;

            Color previous = GUI.color;
            GUI.color = color;
            GUI.DrawTexture(rect, Texture2D.whiteTexture);
            GUI.color = previous;
        }

        private static Color ResolveActionTint(
            MenuActionVisual visual)
        {
            switch (visual)
            {
                case MenuActionVisual.Progress:
                    return StartGameHighlightTint;
                case MenuActionVisual.Settings:
                    return new Color(0.66f, 0.72f, 1.00f, 1f);
                case MenuActionVisual.Leave:
                    return new Color(1.00f, 0.48f, 0.38f, 1f);
                default:
                    return new Color(0.72f, 0.80f, 0.92f, 1f);
            }
        }

        private static string ResolveActionPrefix(
            MenuActionVisual visual)
        {
            switch (visual)
            {
                case MenuActionVisual.Progress:
                    return "[>]";
                case MenuActionVisual.Settings:
                    return "[*]";
                case MenuActionVisual.Leave:
                    return "[!]";
                default:
                    return "[-]";
            }
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
            GUILayout.Label("GAME PAUSED", titleStyle);
            GUILayout.FlexibleSpace();

            if (DrawActionButton(
                    "RESUME",
                    MenuActionVisual.Progress,
                    58f))
            {
                ResumeGame();
            }

            GUILayout.Space(14f);

            if (DrawActionButton(
                    "PROGRESSION",
                    MenuActionVisual.Neutral,
                    52f))
            {
                OpenProgression(fromPause: true);
            }

            GUILayout.Space(14f);

            if (DrawActionButton(
                    "SETTINGS",
                    MenuActionVisual.Settings,
                    52f))
            {
                OpenSettings(fromPause: true);
            }

            GUILayout.Space(14f);

            if (DrawActionButton(
                    "MAIN MENU / ABANDON RUN",
                    MenuActionVisual.Leave,
                    52f))
            {
                // BD ABANDON RUN CONFIRMATION V23R19D
                // Never destroy a live run from the first button press.
                mode = OverlayMode.AbandonConfirm;
            }

            GUILayout.FlexibleSpace();
            GUILayout.Space(24f);
        }

        private void DrawAbandonConfirmation()
        {
            GUILayout.Space(34f);
            GUILayout.Label("ABANDON THIS RUN?", titleStyle);
            GUILayout.Space(18f);
            GUILayout.Label(
                "Current run progress will be lost. This cannot be undone.",
                subtitleStyle
            );
            GUILayout.FlexibleSpace();

            if (DrawActionButton(
                    "YES, ABANDON RUN",
                    MenuActionVisual.Leave,
                    58f))
            {
                BeginConfirmedAbandonToMainMenu();
            }

            GUILayout.Space(14f);

            if (DrawActionButton(
                    "CANCEL",
                    MenuActionVisual.Progress,
                    54f))
            {
                mode = OverlayMode.Pause;
            }

            GUILayout.FlexibleSpace();
            GUILayout.Space(24f);
        }


        private void BeginConfirmedAbandonToMainMenu()
        {
            if (reloadOperation != null)
                return;

            // Preserve the approved fresh mounted introduction for the next
            // run, but first rebuild the scene into a genuine clean main menu.
            BDRunPresentationCoordinator.MarkNextRunAsFreshOrVictoryIntro();
            resultSequenceActive = false;
            pausedFromGameplay = false;
            runActive = false;
            mode = OverlayMode.Loading;
            showMainMenuAfterReload = true;
            autoStartAfterReload = false;
            RestoreTimeScale();

            StartCoroutine(ReloadToMainMenuRoutine());
        }

        private IEnumerator ReloadToMainMenuRoutine()
        {
            Scene scene = SceneManager.GetActiveScene();

            reloadOperation = scene.buildIndex >= 0
                ? SceneManager.LoadSceneAsync(scene.buildIndex)
                : SceneManager.LoadSceneAsync(scene.name);

            if (reloadOperation == null)
            {
                showMainMenuAfterReload = false;
                reloadOperation = null;
                ShowPlainMainMenu(needsReload: true);
                yield break;
            }

            while (!reloadOperation.isDone)
                yield return null;
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

            DrawMemoryProgressBar(progress);

            GUILayout.FlexibleSpace();
        }

        private void DrawMemoryProgressBar(float progress)
        {
            Rect rect = GUILayoutUtility.GetRect(
                360f,
                18f,
                GUILayout.ExpandWidth(true)
            );

            DrawMenuRect(
                rect,
                new Color(0.015f, 0.055f, 0.058f, 0.96f)
            );

            Rect fill = new Rect(
                rect.x + 3f,
                rect.y + 3f,
                Mathf.Max(0f, (rect.width - 6f) * Mathf.Clamp01(progress)),
                rect.height - 6f
            );

            DrawMenuRect(
                fill,
                new Color(0.42f, 0.86f, 0.76f, 0.92f)
            );

            DrawMenuBorder(
                rect,
                new Color(0.66f, 0.92f, 0.82f, 0.34f),
                1f
            );
        }

        private void DrawProgression()
        {
            GUILayout.Space(24f);
            GUILayout.Label("PROGRESSION", titleStyle);
            GUILayout.Space(18f);

            GUILayout.Label(
                BDGameProgress.MotherDefeated
                    ? "Mother restored: YES"
                    : "Mother restored: NO",
                sectionStyle
            );

            GUILayout.Space(12f);
            GUILayout.Label(
                "Persistent upgrades are not available yet. This page is the stable entry point for future cross-run progression.",
                subtitleStyle
            );

            GUILayout.FlexibleSpace();

            if (DrawActionButton(
                    "BACK",
                    MenuActionVisual.Progress,
                    54f))
            {
                CloseProgression();
            }

            GUILayout.Space(24f);
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

            Rect sliderRect = GUILayoutUtility.GetRect(
                220f,
                22f,
                GUILayout.Width(220f)
            );

            float nextValue = GUI.HorizontalSlider(
                sliderRect,
                value,
                minimum,
                maximum,
                sliderStyle,
                sliderThumbStyle
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
                    new Color(0f, 0f, 0f, 0.035f)
                );

            dreamyPanelTexture =
                CreateMenuTexture(
                    new Color(
                        0.015f,
                        0.060f,
                        0.060f,
                        0.22f
                    )
                );

            dreamyButtonTexture =
                CreateMenuTexture(
                    new Color(
                        0.045f,
                        0.135f,
                        0.135f,
                        0.96f
                    )
                );

            dreamyButtonHoverTexture =
                CreateMenuTexture(
                    new Color(
                        0.075f,
                        0.215f,
                        0.205f,
                        0.99f
                    )
                );

            dreamyButtonActiveTexture =
                CreateMenuTexture(
                    new Color(
                        0.025f,
                        0.095f,
                        0.095f,
                        1f
                    )
                );

            titleStyle =
                new GUIStyle(GUI.skin.label)
                {
                    alignment = TextAnchor.MiddleCenter,
                    font = GUI.skin.label.font,
                    fontSize = 37,
                    fontStyle = FontStyle.Bold,
                    clipping = TextClipping.Overflow,
                    normal =
                    {
                        textColor =
                            new Color(
                                0.94f,
                                0.96f,
                                0.79f,
                                1f
                            )
                    }
                };

            subtitleStyle =
                new GUIStyle(GUI.skin.label)
                {
                    alignment = TextAnchor.MiddleCenter,
                    font = GUI.skin.label.font,
                    fontSize = 16,
                    fontStyle = FontStyle.Bold,
                    wordWrap = true,
                    normal =
                    {
                        textColor =
                            new Color(
                                0.63f,
                                0.88f,
                                0.82f,
                                0.92f
                            )
                    }
                };

            sectionStyle =
                new GUIStyle(GUI.skin.label)
                {
                    alignment = TextAnchor.MiddleLeft,
                    font = GUI.skin.label.font,
                    fontSize = 17,
                    normal =
                    {
                        textColor =
                            new Color(
                                0.90f,
                                0.94f,
                                0.82f,
                                1f
                            )
                    }
                };

            valueStyle =
                new GUIStyle(GUI.skin.label)
                {
                    alignment = TextAnchor.MiddleCenter,
                    font = GUI.skin.label.font,
                    fontSize = 14,
                    normal =
                    {
                        textColor =
                            new Color(
                                0.58f,
                                0.84f,
                                0.78f,
                                0.84f
                            )
                    }
                };

            buttonStyle =
                new GUIStyle(GUI.skin.button)
                {
                    alignment = TextAnchor.MiddleLeft,
                    font = GUI.skin.button.font,
                    fontSize = 19,
                    fontStyle = FontStyle.Bold,
                    padding = new RectOffset(24, 18, 8, 8),
                    margin = new RectOffset(0, 0, 0, 0),
                    normal =
                    {
                        background = dreamyTransparentTexture,
                        textColor =
                            new Color(
                                0.94f,
                                0.96f,
                                0.80f,
                                1f
                            )
                    },
                    hover =
                    {
                        background = dreamyTransparentTexture,
                        textColor = Color.white
                    },
                    active =
                    {
                        background = dreamyTransparentTexture,
                        textColor =
                            new Color(
                                1f,
                                0.86f,
                                0.52f,
                                1f
                            )
                    }
                };

            smallButtonStyle =
                new GUIStyle(buttonStyle)
                {
                    alignment = TextAnchor.MiddleCenter,
                    fontSize = 14,
                    padding = new RectOffset(10, 10, 6, 6),
                    normal =
                    {
                        background = dreamyButtonTexture,
                        textColor = new Color(0.92f, 0.96f, 0.82f, 1f)
                    },
                    hover =
                    {
                        background = dreamyButtonHoverTexture,
                        textColor = Color.white
                    },
                    active =
                    {
                        background = dreamyButtonActiveTexture,
                        textColor = new Color(1f, 0.86f, 0.52f, 1f)
                    }
                };

            sliderStyle =
                new GUIStyle(GUI.skin.horizontalSlider)
                {
                    fixedHeight = 8f,
                    margin = new RectOffset(0, 0, 7, 7),
                    normal =
                    {
                        background = dreamyButtonTexture
                    }
                };

            sliderThumbStyle =
                new GUIStyle(GUI.skin.horizontalSliderThumb)
                {
                    fixedWidth = 18f,
                    fixedHeight = 18f,
                    normal =
                    {
                        background = dreamyButtonHoverTexture
                    },
                    hover =
                    {
                        background = dreamyButtonHoverTexture
                    },
                    active =
                    {
                        background = dreamyButtonActiveTexture
                    }
                };

            panelStyle =
                new GUIStyle(GUI.skin.box)
                {
                    padding =
                        new RectOffset(
                            32,
                            32,
                            36,
                            22
                        ),
                    normal =
                    {
                        background = dreamyPanelTexture
                    }
                };

            overlayStyle =
                new GUIStyle(GUI.skin.box)
                {
                    normal =
                    {
                        background = dreamyTransparentTexture
                    }
                };
        }

    }
}
