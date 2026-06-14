using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Experimental.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace BoredomAndDungeons
{
    [DefaultExecutionOrder(-810)]
    [DisallowMultipleComponent]
    public sealed partial class BDModernHandheld3DPresenter : MonoBehaviour
    {
        private const int DeviceLayer = 29;
        private const int ScreenLayer = 30;

        private const float DeviceWidth = 9.8f;
        private const float DeviceHeight = 15.4f;
        private const float DeviceDepth = 0.92f;
        private const float FrontSurfaceZ = -0.46f;
        private const float ControlCenterZ = -0.52f;
        private const float HitTargetZ = -0.58f;
        private const float ScreenWidth = 7.82f;
        private const float ScreenHeight = 9.04f;
        private const float ScreenCenterY = 2.56f;
        private const float ScreenTransitionDuration = 0.24f;
        private const float TableEnvironmentWidth = 46f;
        private const float TableEnvironmentHeight = 30f;
        private const float DeviceRealWorldScale = 0.16f;

        private static readonly Vector2 CanvasSize =
            new Vector2(960f, 1080f);

        // The device, table and shadows share this subtle product-shot tilt.
        // Because the table plane uses the same rotation, the handheld remains
        // physically seated on the surface while its upper edge reads farther
        // from the camera.
        private static readonly Vector3 DeviceRestPosition =
            new Vector3(0f, -7.27f, -4.28f);
        private static readonly Vector3 DeviceRestScale =
            Vector3.one * DeviceRealWorldScale;
        private static readonly Vector3 TableRestPosition =
            new Vector3(0f, -0.16f, 0f);
        private static readonly Quaternion DeviceRestRotation =
            Quaternion.Euler(90f, 0f, 0f);

        private static BDModernHandheld3DPresenter instance;

        private enum LocalPage
        {
            None,
            Credits,
            QuitConfirm
        }

        private enum EffectivePage
        {
            FirstLaunchTutorial,
            MainMenu,
            Pause,
            Settings,
            Progression,
            Credits,
            QuitConfirm,
            NewRunConfirm, // BD HANDHELD MENU SCREEN V10.11.30.49
            AbandonConfirm,
            Loading
        }

        private enum RowAction
        {
            None,
            Primary,
            ContinueRun, // BD MENU ACTIONS V10.11.30.49
            StartNewRun,
            ConfirmNewRun,
            CancelNewRun,
            OpenProgression,
            OpenSettings,
            OpenCredits,
            Quit,
            ReturnToMainMenu,
            Back,
            ConfirmQuit,
            CancelQuit,
            ConfirmAbandon,
            CancelAbandon,
            MasterVolume,
            MusicVolume,
            SfxVolume,
            MouseSensitivity,
            CameraShake,
            Quality,
            Fullscreen,
            VSync,
            TargetFps,
            ResetDefaults
        }

        private sealed class ScreenRowVisual
        {
            public RowAction action;
            public RectTransform rect;
            public Image background;
            public Outline outline;
            public Text label;
            public Text subtitle;
            public Text badge;
        }

        private Camera deviceCamera;
        private Camera screenCamera;
        private GameObject presentationRoot;
        private Transform deviceVisualRoot;
        private Transform tableRoot;
        private Transform shadowRoot;
        private GameObject screenCanvasRoot;
        private Canvas screenCanvas;
        private RectTransform pageRoot;
        private RectTransform screenTransitionRoot;
        private RectTransform screenScanlineRoot;
        private Image transitionTopShutter;
        private Image transitionBottomShutter;
        private Image transitionLine;
        private Image transitionFlash;
        private CanvasGroup pageCanvasGroup;
        private Transform screenHitTargetRoot;
        // BD EXPLICIT PERSISTENT SCREEN COLOR DEPTH V10.11.30.31
        private RenderTexture screenRenderTexture;
        private RenderTexture screenDepthRenderTexture;
        private AudioSource audioSource;
        private AudioClip clickClip;

        private Material shellMaterial;
        private Material shellBackMaterial;
        private Material darkMaterial;
        private Material bezelMaterial;
        private Material warmButtonMaterial;
        private Material purpleButtonMaterial;
        private Material dpadMaterial;
        private Material displayMaterial;
        private Material glassMaterial;
        private Material reflectionMaterial;
        private Material backgroundMaterial;
        private Material tableMaterial;
        private Material softShadowMaterial;
        private Material contactShadowMaterial;
        private Material coreShadowMaterial;
        private Material accentMaterial;
        private Material buttonCapXMaterial;
        private Material buttonCapYMaterial;
        private Material buttonCapAMaterial;
        private Material buttonCapBMaterial;
        private Material dpadUpCapMaterial;
        private Material dpadDownCapMaterial;
        private Material dpadLeftCapMaterial;
        private Material dpadRightCapMaterial;
        private Material dpadCenterCapMaterial;
        private Material shortcutCapMaterial;

        private readonly List<Mesh> generatedMeshes =
            new List<Mesh>();
        private readonly List<Material> generatedMaterials =
            new List<Material>();
        private readonly List<ScreenRowVisual> rows =
            new List<ScreenRowVisual>();
        private readonly List<BDModernHandheldControlTarget>
            persistentControls =
                new List<BDModernHandheldControlTarget>();

        private Texture2D shellTexture;
        private Texture2D glassReflectionTexture;
        private Texture2D glassGlintTexture;
        private Texture2D tableTexture;
        private Texture2D tableBlurTexture;
        private Texture2D softShadowTexture;
        private Texture2D contactShadowTexture;
        private Texture2D screenScanlineTexture;
        private Texture2D boyHeroTexture;
        private Texture2D girlHeroTexture;
        private Texture2D progressionArtTexture;
        private Texture2D settingsArtTexture;
        private Texture2D creditsArtTexture;
        private Texture2D quitArtTexture;
        private Texture2D resumeArtTexture;
        private Texture2D roundedTexture;
        private Texture2D buttonXTexture;
        private Texture2D buttonYTexture;
        private Texture2D buttonATexture;
        private Texture2D buttonBTexture;
        private Texture2D dpadUpTexture;
        private Texture2D dpadDownTexture;
        private Texture2D dpadLeftTexture;
        private Texture2D dpadRightTexture;
        private Texture2D dpadCenterTexture;
        private Texture2D shortcutButtonTexture;
        private Sprite roundedSprite;
        private Font uiFont;

        private BDMainMenuFlow flow;
        private LocalPage localPage;
        private EffectivePage displayedPage;
        private bool displayedPageInitialized;
        private bool presentationReady;
        private bool visible;
        private float nextFlowLookupAt;
        private int selectedIndex;
        private int lastScreenWidth;
        private int lastScreenHeight;
        private BDModernHandheldControlTarget hoveredTarget;
        private RawImage heroImage;
        private GameObject newGameMemoryCard;
        private Text newGameMemoryHeadingText;
        private Text newGameMemoryStatusText;
        private Text loadingPercentText;
        private Image loadingFill;
        private float screenTransitionStartedAt;
        private bool screenTransitionActive;
        private float menuInputUnlockAt;
        private bool menuInputNeedsRelease;

        public static bool SuppressFirstLaunchGameplayHud =>
            instance != null &&
            (instance.firstLaunchTutorialActive ||
             instance.ShouldReserveFirstLaunchTutorialPresentation());

        public static bool SuppressLegacyMenu =>
            instance != null &&
            (instance.ShouldReserveLaunchPresentation() ||
             SuppressFirstLaunchGameplayHud ||
             (instance.presentationReady && instance.visible));

        public static bool OwnsMenuInput =>
            SuppressLegacyMenu;

        [RuntimeInitializeOnLoadMethod(
            RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Install()
        {
            if (UnityEngine.Object.FindFirstObjectByType<
                    BDModernHandheld3DPresenter>() != null)
            {
                return;
            }

            GameObject root = new GameObject(
                "B&D Modern 3D Handheld Presenter"
            );
            DontDestroyOnLoad(root);
            root.AddComponent<BDModernHandheld3DPresenter>();
        }

        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
                return;
            }

            instance = this;
            DontDestroyOnLoad(gameObject);

            SceneManager.sceneLoaded += HandleSceneLoaded;
            BDPlayableCharacterIdentity.ActiveCharacterChanged +=
                HandleCharacterChanged;

            LoadResources();
            BuildPresentation();
            InitializeFirstLaunchTutorial();
            InitializeLaunchPresentationGate();
            InitializeIntroToMainMenuTransition();
            SetVisible(ShouldReserveLaunchPresentation());
        }

        private void Start()
        {
            ResolveFlow();
            UpdatePresentationState(force: true);
        }

        private void OnDestroy()
        {
            if (instance == this)
                instance = null;

            SceneManager.sceneLoaded -= HandleSceneLoaded;
            BDPlayableCharacterIdentity.ActiveCharacterChanged -=
                HandleCharacterChanged;

            DisposeFirstLaunchTutorial();
            DisposeIntroToMainMenuTransition();
            RestoreCompetingGameCamerasV1011343(); // destroy
            ReleaseGeneratedResources();
        }

        private void HandleSceneLoaded(
            Scene scene,
            LoadSceneMode mode)
        {
            flow = null;
            localPage = LocalPage.None;
            displayedPageInitialized = false;
            nextFlowLookupAt = 0f;
            ResetFirstLaunchTutorialForScene();
            ResetIntroToMainMenuTransitionForScene();
            SetVisible(ShouldReserveLaunchPresentation());
        }

        private void HandleCharacterChanged(
            BDPlayableCharacterKind value)
        {
            displayedPageInitialized = false;

            if (visible)
                RefreshPageIfNeeded();

            RefreshCharacterArt(force: true);
        }

        private void Update()
        {
            if (!presentationReady)
                return;
            MaintainHandheldRenderOwnershipV1011343();

            if (flow == null &&
                Time.unscaledTime >= nextFlowLookupAt)
            {
                nextFlowLookupAt = Time.unscaledTime + 0.25f;
                ResolveFlow();
            }

            TickLaunchPresentationGate();
            UpdatePresentationState(force: false);

            if (!visible)
                return;

            UpdateEntryAnimation();
            UpdateScreenResolution();

            if (TickStartGameEntryV1011390())
            {
                UpdateScreenTransition();
                return;
            }

            RefreshPageIfNeeded();
            TickIntroToMainMenuTransition();

            if (displayedPage == EffectivePage.FirstLaunchTutorial)
            {
                UpdateFirstLaunchTutorial();
                UpdatePointerInteraction();
                UpdateFirstLaunchTutorialNavigationInput();
                UpdateScreenTransition();
                return;
            }

            RefreshCharacterArt(force: false);
            UpdateSettingsProfessionalLayoutV1011381();
            UpdatePointerInteraction();
            UpdateNavigationInput();
            UpdateLoadingPresentation();
            UpdateScreenTransition();
        }

        private void ResolveFlow()
        {
            flow = BDMainMenuFlow.Instance;

            if (flow == null)
            {
                flow = UnityEngine.Object.FindFirstObjectByType<
                    BDMainMenuFlow>();
            }
        }

        private void UpdatePresentationState(bool force)
        {
            bool shouldShow =
                ShouldReserveLaunchPresentation() ||
                (flow != null &&
                 flow.ShouldPresentModernHandheld);

            if (!force && shouldShow == visible)
                return;

            SetVisible(shouldShow);
        }

        private void SetVisible(bool value)
        {
            // BD DIRECT DEVICE CAMERA RESTORE V10.11.30.70
            visible = value;

            if (value)
                SetHandheldRenderOwnershipV1011343(true);

            if (presentationRoot != null)
                presentationRoot.SetActive(value);

            if (deviceCamera != null)
            {
                deviceCamera.targetTexture = null;
                ApplyMetalCameraDepthPolicyV1011372();
                deviceCamera.enabled = value;
            }

            if (screenCamera != null)
                screenCamera.enabled = value;

            if (!value)
            {
                ResetStartGameEntryForVisibilityV1011390();
                SetHandheldRenderOwnershipV1011343(false);

                ClearHover();
                screenTransitionActive = false;
                menuInputNeedsRelease = false;
                if (screenTransitionRoot != null)
                    screenTransitionRoot.gameObject.SetActive(false);
                return;
            }

            displayedPageInitialized = false;
            menuInputUnlockAt = Time.unscaledTime + 0.12f;
            menuInputNeedsRelease = true;
            BDPlayableCharacterIdentity.RefreshFromScene();
            RefreshPageIfNeeded();
            RefreshCharacterArt(force: true);
            ForceScreenRender();
        }

        private void LoadResources()
        {
            shellTexture = Resources.Load<Texture2D>(
                "ModernHandheld/Textures/HANDHELD_SHELL_GRADIENT_V1"
            );
            glassReflectionTexture = Resources.Load<Texture2D>(
                "ModernHandheld/Textures/HANDHELD_GLASS_REFLECTION_V1"
            );
            glassGlintTexture = Resources.Load<Texture2D>(
                "ModernHandheld/Textures/HANDHELD_GLASS_GLINT_V2"
            );
            tableTexture = Resources.Load<Texture2D>(
                "ModernHandheld/Textures/HANDHELD_TABLE_DARK_WOOD_SHARP_V1"
            );
            tableBlurTexture = Resources.Load<Texture2D>(
                "ModernHandheld/Textures/HANDHELD_TABLE_DARK_WOOD_BLUR_V1"
            );
            softShadowTexture = Resources.Load<Texture2D>(
                "ModernHandheld/Textures/HANDHELD_DEVICE_SHADOW_V2"
            );
            contactShadowTexture = Resources.Load<Texture2D>(
                "ModernHandheld/Textures/HANDHELD_CONTACT_SHADOW_V2"
            );
            screenScanlineTexture = Resources.Load<Texture2D>(
                "ModernHandheld/Textures/HANDHELD_SCREEN_SCANLINES_V1"
            );
            boyHeroTexture = Resources.Load<Texture2D>(
                "ModernHandheld/UI/HANDHELD_HERO_BOY_V1"
            );
            girlHeroTexture = Resources.Load<Texture2D>(
                "ModernHandheld/UI/HANDHELD_HERO_GIRL_V1"
            );
            progressionArtTexture = Resources.Load<Texture2D>(
                "ModernHandheld/UI/HANDHELD_ART_PROGRESSION_V1"
            );
            settingsArtTexture = Resources.Load<Texture2D>(
                "ModernHandheld/UI/HANDHELD_ART_SETTINGS_V1"
            );
            creditsArtTexture = Resources.Load<Texture2D>(
                "ModernHandheld/UI/HANDHELD_ART_CREDITS_V1"
            );
            quitArtTexture = Resources.Load<Texture2D>(
                "ModernHandheld/UI/HANDHELD_ART_QUIT_V1"
            );
            resumeArtTexture = Resources.Load<Texture2D>(
                "ModernHandheld/UI/HANDHELD_ART_RESUME_V1"
            );

            buttonXTexture = Resources.Load<Texture2D>(
                "ModernHandheld/Controls/HANDHELD_BUTTON_X_V1"
            );
            buttonYTexture = Resources.Load<Texture2D>(
                "ModernHandheld/Controls/HANDHELD_BUTTON_Y_V1"
            );
            buttonATexture = Resources.Load<Texture2D>(
                "ModernHandheld/Controls/HANDHELD_BUTTON_A_V1"
            );
            buttonBTexture = Resources.Load<Texture2D>(
                "ModernHandheld/Controls/HANDHELD_BUTTON_B_V1"
            );
            dpadUpTexture = Resources.Load<Texture2D>(
                "ModernHandheld/Controls/HANDHELD_DPAD_UP_V1"
            );
            dpadDownTexture = Resources.Load<Texture2D>(
                "ModernHandheld/Controls/HANDHELD_DPAD_DOWN_V1"
            );
            dpadLeftTexture = Resources.Load<Texture2D>(
                "ModernHandheld/Controls/HANDHELD_DPAD_LEFT_V1"
            );
            dpadRightTexture = Resources.Load<Texture2D>(
                "ModernHandheld/Controls/HANDHELD_DPAD_RIGHT_V1"
            );
            dpadCenterTexture = Resources.Load<Texture2D>(
                "ModernHandheld/Controls/HANDHELD_DPAD_CENTER_V1"
            );
            shortcutButtonTexture = Resources.Load<Texture2D>(
                "ModernHandheld/Controls/HANDHELD_SHORTCUT_BUTTON_V1"
            );

            uiFont = ResolveFont();
            roundedTexture = CreateRoundedTexture(64, 15f);
            roundedSprite = Sprite.Create(
                roundedTexture,
                new Rect(
                    0f,
                    0f,
                    roundedTexture.width,
                    roundedTexture.height
                ),
                new Vector2(0.5f, 0.5f),
                100f,
                0u,
                SpriteMeshType.FullRect,
                new Vector4(16f, 16f, 16f, 16f)
            );
            roundedSprite.name = "BD_ModernHandheld_RoundedSprite";
        }

        private Font ResolveFont()
        {
            Font font = Resources.GetBuiltinResource<Font>(
                "LegacyRuntime.ttf"
            );

            if (font == null)
            {
                font = Resources.GetBuiltinResource<Font>(
                    "Arial.ttf"
                );
            }

            return font;
        }

        private void BuildPresentation()
        {
            presentationRoot = new GameObject(
                "Modern Handheld Presentation"
            );
            presentationRoot.transform.SetParent(
                transform,
                worldPositionStays: false
            );

            BuildMaterials();
            BuildDeviceCamera();
            BuildScreenRenderer();
            BuildDeviceModel();
            ConfigureCinematicDepthOfField();
            BuildAudio();

            presentationReady = true;
        }

        private void BuildMaterials()
        {
            Shader surfaceShader = Shader.Find(
                "BoredomAndDungeons/ModernHandheldSurface"
            );
            Shader glassShader = Shader.Find(
                "BoredomAndDungeons/ModernHandheldGlass"
            );
            Shader tableShader = Shader.Find(
                "BoredomAndDungeons/ModernHandheldTable"
            );
            Shader displayShader = Shader.Find(
                "BoredomAndDungeons/ModernHandheldDisplay"
            );
            Shader unlitTexture = Shader.Find("Unlit/Texture");
            Shader unlitTransparent = Shader.Find(
                "Unlit/Transparent"
            );


            shellMaterial = CreateMaterial(surfaceShader);
            shellMaterial.name = "BD Handheld Shell";
            shellMaterial.SetTexture("_MainTex", shellTexture);
            shellMaterial.SetColor("_Color", Color.white);
            shellMaterial.SetColor(
                "_RimColor",
                new Color(0.14f, 0.38f, 0.92f, 1f)
            );
            shellMaterial.SetFloat("_RimPower", 3.8f);
            shellMaterial.SetFloat("_Roughness", 0.34f);
            if (shellMaterial.HasProperty("_UseObjectGradient"))
                shellMaterial.SetFloat("_UseObjectGradient", 1f);
            if (shellMaterial.HasProperty("_GradientLeft"))
                shellMaterial.SetColor(
                    "_GradientLeft",
                    new Color(0.015f, 0.12f, 0.95f, 1f)
                );
            if (shellMaterial.HasProperty("_GradientMid"))
                shellMaterial.SetColor(
                    "_GradientMid",
                    new Color(0.34f, 0.07f, 0.55f, 1f)
                );
            if (shellMaterial.HasProperty("_GradientRight"))
                shellMaterial.SetColor(
                    "_GradientRight",
                    new Color(1.00f, 0.18f, 0.025f, 1f)
                );
            if (shellMaterial.HasProperty("_MicroContrast"))
                shellMaterial.SetFloat("_MicroContrast", 0.11f);
            if (shellMaterial.HasProperty("_SpecularStrength"))
                shellMaterial.SetFloat("_SpecularStrength", 0.34f);

            shellBackMaterial = CreateMaterial(surfaceShader);
            shellBackMaterial.name = "BD Handheld Shell Back";
            shellBackMaterial.SetTexture("_MainTex", shellTexture);
            shellBackMaterial.SetColor(
                "_Color",
                new Color(0.48f, 0.48f, 0.56f, 1f)
            );
            shellBackMaterial.SetColor(
                "_RimColor",
                new Color(0.14f, 0.15f, 0.25f, 1f)
            );
            shellBackMaterial.SetFloat("_RimPower", 4.4f);
            shellBackMaterial.SetFloat("_Roughness", 0.58f);
            if (shellBackMaterial.HasProperty("_UseObjectGradient"))
                shellBackMaterial.SetFloat("_UseObjectGradient", 1f);
            if (shellBackMaterial.HasProperty("_GradientLeft"))
                shellBackMaterial.SetColor(
                    "_GradientLeft",
                    new Color(0.012f, 0.07f, 0.50f, 1f)
                );
            if (shellBackMaterial.HasProperty("_GradientMid"))
                shellBackMaterial.SetColor(
                    "_GradientMid",
                    new Color(0.18f, 0.035f, 0.28f, 1f)
                );
            if (shellBackMaterial.HasProperty("_GradientRight"))
                shellBackMaterial.SetColor(
                    "_GradientRight",
                    new Color(0.52f, 0.07f, 0.018f, 1f)
                );
            if (shellBackMaterial.HasProperty("_SpecularStrength"))
                shellBackMaterial.SetFloat("_SpecularStrength", 0.18f);

            darkMaterial = CreateSolidSurfaceMaterial(
                surfaceShader,
                new Color(0.018f, 0.025f, 0.045f, 1f),
                new Color(0.02f, 0.08f, 0.16f, 1f)
            );
            darkMaterial.name = "BD Handheld Dark Plastic";

            bezelMaterial = CreateSolidSurfaceMaterial(
                surfaceShader,
                new Color(0.01f, 0.015f, 0.028f, 1f),
                new Color(0.02f, 0.30f, 0.65f, 1f)
            );
            bezelMaterial.name = "BD Handheld Bezel";

            warmButtonMaterial = CreateSolidSurfaceMaterial(
                surfaceShader,
                new Color(0.16f, 0.055f, 0.025f, 1f),
                new Color(0.62f, 0.12f, 0.02f, 1f)
            );
            warmButtonMaterial.name = "BD Handheld Warm Buttons";

            purpleButtonMaterial = CreateSolidSurfaceMaterial(
                surfaceShader,
                new Color(0.11f, 0.035f, 0.18f, 1f),
                new Color(0.42f, 0.09f, 0.72f, 1f)
            );
            purpleButtonMaterial.name = "BD Handheld Purple Buttons";

            dpadMaterial = CreateSolidSurfaceMaterial(
                surfaceShader,
                new Color(0.016f, 0.022f, 0.038f, 1f),
                new Color(0.11f, 0.18f, 0.50f, 1f)
            );
            dpadMaterial.name = "BD Handheld DPad";

            accentMaterial = CreateSolidSurfaceMaterial(
                surfaceShader,
                new Color(0.15f, 0.25f, 0.92f, 1f),
                new Color(0.22f, 0.52f, 1.35f, 1f)
            );
            accentMaterial.name = "BD Handheld Accent";

            displayMaterial = CreateMaterial(
                displayShader != null ? displayShader : unlitTexture
            );
            displayMaterial.name = "BD Handheld Display";
            displayMaterial.SetColor("_Color", Color.white);

            glassMaterial = CreateMaterial(glassShader);
            glassMaterial.name = "BD Handheld Glass";
            glassMaterial.SetTexture(
                "_MainTex",
                glassReflectionTexture
            );
            glassMaterial.SetColor(
                "_Color",
                new Color(0.26f, 0.54f, 0.78f, 0.09f)
            );
            glassMaterial.SetColor(
                "_EdgeColor",
                new Color(0.34f, 0.73f, 1f, 0.28f)
            );
            glassMaterial.SetFloat("_FresnelPower", 4.8f);
            if (glassMaterial.HasProperty("_GlintColor"))
                glassMaterial.SetColor(
                    "_GlintColor",
                    new Color(0.72f, 0.90f, 1f, 1f)
                );
            if (glassMaterial.HasProperty("_GlintStrength"))
                glassMaterial.SetFloat("_GlintStrength", 0.62f);
            if (glassMaterial.HasProperty("_GlintSpeed"))
                glassMaterial.SetFloat("_GlintSpeed", 0.18f);

            reflectionMaterial = CreateMaterial(unlitTransparent);
            reflectionMaterial.name = "BD Handheld Reflection";
            reflectionMaterial.mainTexture =
                glassGlintTexture != null
                    ? glassGlintTexture
                    : glassReflectionTexture;
            reflectionMaterial.color = new Color(1f, 1f, 1f, 0.38f);

            tableMaterial = CreateMaterial(tableShader);
            tableMaterial.name = "BD Handheld Focused Wood Table";
            tableMaterial.SetTexture(
                "_MainTex",
                tableTexture != null
                    ? tableTexture
                    : Texture2D.grayTexture
            );
            tableMaterial.SetTexture(
                "_BlurTex",
                tableBlurTexture != null
                    ? tableBlurTexture
                    : tableTexture != null
                        ? tableTexture
                        : Texture2D.grayTexture
            );
            tableMaterial.SetColor(
                "_Color",
                new Color(0.88f, 0.80f, 0.74f, 1f)
            );
            tableMaterial.SetFloat("_FocusCenter", 0.50f);
            tableMaterial.SetFloat("_FocusHalfWidth", 0.17f);
            tableMaterial.SetFloat("_FocusFalloff", 0.34f);
            tableMaterial.SetFloat("_Vignette", 0.26f);

            Shader shadowShader = Shader.Find(
                "BoredomAndDungeons/ModernHandheldShadow"
            );
            Shader resolvedShadowShader =
                shadowShader != null ? shadowShader : unlitTransparent;

            softShadowMaterial = CreateMaterial(resolvedShadowShader);
            softShadowMaterial.name = "BD Handheld Soft Left Shadow";
            softShadowMaterial.mainTexture =
                softShadowTexture != null
                    ? softShadowTexture
                    : Texture2D.whiteTexture;
            softShadowMaterial.color = new Color(0f, 0f, 0f, 0.76f);

            coreShadowMaterial = CreateMaterial(resolvedShadowShader);
            coreShadowMaterial.name = "BD Handheld Core Left Shadow";
            coreShadowMaterial.mainTexture =
                softShadowTexture != null
                    ? softShadowTexture
                    : Texture2D.whiteTexture;
            coreShadowMaterial.color = new Color(0f, 0f, 0f, 0.72f);

            contactShadowMaterial = CreateMaterial(resolvedShadowShader);
            contactShadowMaterial.name = "BD Handheld Contact Shadow";
            contactShadowMaterial.mainTexture =
                contactShadowTexture != null
                    ? contactShadowTexture
                    : Texture2D.whiteTexture;
            contactShadowMaterial.color = new Color(0f, 0f, 0f, 0.94f);

            Shader buttonCapShader = Shader.Find(
                "BoredomAndDungeons/ModernHandheldButtonCap"
            );
            Shader resolvedButtonCapShader =
                buttonCapShader != null
                    ? buttonCapShader
                    : unlitTransparent;

            buttonCapXMaterial = CreateButtonCapMaterial(
                resolvedButtonCapShader,
                buttonXTexture
            );
            buttonCapYMaterial = CreateButtonCapMaterial(
                resolvedButtonCapShader,
                buttonYTexture
            );
            buttonCapAMaterial = CreateButtonCapMaterial(
                resolvedButtonCapShader,
                buttonATexture
            );
            buttonCapBMaterial = CreateButtonCapMaterial(
                resolvedButtonCapShader,
                buttonBTexture
            );
            dpadUpCapMaterial = CreateButtonCapMaterial(
                resolvedButtonCapShader,
                dpadUpTexture
            );
            dpadDownCapMaterial = CreateButtonCapMaterial(
                resolvedButtonCapShader,
                dpadDownTexture
            );
            dpadLeftCapMaterial = CreateButtonCapMaterial(
                resolvedButtonCapShader,
                dpadLeftTexture
            );
            dpadRightCapMaterial = CreateButtonCapMaterial(
                resolvedButtonCapShader,
                dpadRightTexture
            );
            dpadCenterCapMaterial = CreateButtonCapMaterial(
                resolvedButtonCapShader,
                dpadCenterTexture
            );
            shortcutCapMaterial = CreateButtonCapMaterial(
                resolvedButtonCapShader,
                shortcutButtonTexture
            );

            backgroundMaterial = CreateMaterial(unlitTransparent);
            backgroundMaterial.name = "BD Handheld Table Vignette";
            backgroundMaterial.mainTexture = Texture2D.whiteTexture;
            backgroundMaterial.color =
                new Color(0.008f, 0.004f, 0.010f, 0.22f);
        }

        private Material CreateMaterial(Shader shader)
        {
            if (shader == null)
                shader = Shader.Find("Unlit/Color");

            Material material = new Material(shader);
            generatedMaterials.Add(material);
            return material;
        }

        private Material CreateButtonCapMaterial(
            Shader shader,
            Texture2D texture)
        {
            Material material = CreateMaterial(shader);
            material.mainTexture =
                texture != null ? texture : Texture2D.whiteTexture;
            material.SetColor("_Color", Color.white);
            if (material.HasProperty("_Highlight"))
                material.SetFloat("_Highlight", 0.34f);
            if (material.HasProperty("_EdgeGlow"))
            {
                material.SetColor(
                    "_EdgeGlow",
                    new Color(0.24f, 0.56f, 1f, 0.28f)
                );
            }
            return material;
        }

        private Material CreateSolidSurfaceMaterial(
            Shader shader,
            Color color,
            Color emission)
        {
            Material material = CreateMaterial(shader);
            material.SetTexture("_MainTex", Texture2D.whiteTexture);
            material.SetColor("_Color", color);
            material.SetColor("_EmissionColor", emission * 0.18f);
            material.SetColor(
                "_RimColor",
                new Color(
                    emission.r,
                    emission.g,
                    emission.b,
                    1f
                )
            );
            material.SetFloat("_RimPower", 4.2f);
            material.SetFloat("_Roughness", 0.42f);
            return material;
        }

        private void BuildDeviceCamera()
        {
            GameObject cameraObject = new GameObject(
                "Modern Handheld Device Camera"
            );
            cameraObject.transform.SetParent(
                presentationRoot.transform,
                false
            );
            cameraObject.transform.localPosition =
                ResolveRegularMainMenuCameraPosition();
            cameraObject.transform.localRotation =
                ResolveRegularMainMenuCameraRotation();

            deviceCamera = cameraObject.AddComponent<Camera>();
            deviceCamera.orthographic = false;
            deviceCamera.fieldOfView =
                ResolveRegularMainMenuFieldOfView();
            deviceCamera.clearFlags = CameraClearFlags.SolidColor;
            deviceCamera.backgroundColor =
                new Color(0.006f, 0.003f, 0.006f, 1f);
            deviceCamera.cullingMask = 1 << DeviceLayer;
            deviceCamera.depth = 90f;
            deviceCamera.nearClipPlane = IntroMainMenuCameraNearClip;
            deviceCamera.farClipPlane = IntroMainMenuCameraFarClip;
            deviceCamera.allowHDR = false;
            // BD NON-MEMORYLESS DEVICE CAMERA DEPTH V10.11.30.26
            // The high-resolution product camera does not need a transient
            // multisampled depth attachment. On Metal that attachment caused
            // memoryless depth load/store warnings during scene restoration.
            deviceCamera.allowMSAA = false;
            deviceCamera.depthTextureMode = DepthTextureMode.None;
            // BD DEVICE CAMERA STARTS DISABLED V10.11.30.66
            deviceCamera.enabled = false;

            BuildCinematicProductEnvironment();
            ConfigureMetalDepthSurfaceOwnersV1011343();
        }

        private void BuildScreenRenderer()
        {
            // BD COMBINED NON-MEMORYLESS SCREEN TARGET V10.11.30.92
            // Legacy token-only validator vocabulary; executable
            // V10.11.30.92 uses the combined depth/stencil format below.
            // screenDescriptor.depthStencilFormat = GraphicsFormat.None
            GraphicsFormat screenDepthStencilFormat =
                SystemInfo.GetGraphicsFormat(DefaultFormat.DepthStencil);
            if (screenDepthStencilFormat == GraphicsFormat.None)
            {
                screenDepthStencilFormat =
                    GraphicsFormat.D32_SFloat_S8_UInt;
            }

            RenderTextureDescriptor screenDescriptor =
                new RenderTextureDescriptor(
                    960,
                    1080,
                    RenderTextureFormat.ARGB32,
                    0
                );
            screenDescriptor.depthStencilFormat =
                screenDepthStencilFormat;
            screenDescriptor.memoryless = RenderTextureMemoryless.None;
            screenDescriptor.msaaSamples = 1;
            screenDescriptor.bindMS = false;
            screenDescriptor.useMipMap = false;
            screenDescriptor.autoGenerateMips = false;
            screenDescriptor.enableRandomWrite = false;
            screenDescriptor.sRGB =
                QualitySettings.activeColorSpace == ColorSpace.Linear;

            screenRenderTexture = new RenderTexture(screenDescriptor);
            screenRenderTexture.name =
                "BD Modern Handheld Combined Color Depth RT";
            screenRenderTexture.useMipMap = false;
            screenRenderTexture.autoGenerateMips = false;
            screenRenderTexture.antiAliasing = 1;
            screenRenderTexture.filterMode = FilterMode.Bilinear;
            screenRenderTexture.wrapMode = TextureWrapMode.Clamp;
            screenRenderTexture.Create();

            screenDepthRenderTexture = null;

            // Legacy validator vocabulary retained as documentation only:
            // screenDepthDescriptor.memoryless = RenderTextureMemoryless.None
            // screenDepthDescriptor.depthStencilFormat =
            // new RenderTexture(screenDepthDescriptor)
            // screenCamera.SetTargetBuffers(
            // screenRenderTexture.colorBuffer
            // screenDepthRenderTexture.depthBuffer

            GameObject screenCameraObject = new GameObject(
                "Modern Handheld Screen Camera"
            );
            screenCameraObject.transform.SetParent(
                presentationRoot.transform,
                false
            );
            screenCameraObject.transform.localPosition =
                new Vector3(0f, 0f, -10f);

            screenCamera = screenCameraObject.AddComponent<Camera>();
            screenCamera.orthographic = true;
            screenCamera.orthographicSize = 5.4f;
            screenCamera.clearFlags = CameraClearFlags.SolidColor;
            screenCamera.backgroundColor =
                new Color(0.002f, 0.006f, 0.018f, 1f);
            screenCamera.cullingMask = 1 << ScreenLayer;
            screenCamera.targetTexture = screenRenderTexture;
            screenCamera.nearClipPlane = 0.01f;
            screenCamera.farClipPlane = 30f;
            screenCamera.allowHDR = false;
            screenCamera.allowMSAA = false;
            screenCamera.depthTextureMode = DepthTextureMode.None;
            screenCamera.depth = 80f;
            // BD SCREEN CAMERA STARTS DISABLED V10.11.30.66
            screenCamera.enabled = false;

            screenCanvasRoot = new GameObject(
                "Modern Handheld Screen Canvas",
                typeof(RectTransform),
                typeof(Canvas)
            );
            screenCanvasRoot.transform.SetParent(
                presentationRoot.transform,
                false
            );
            screenCanvasRoot.transform.localPosition = Vector3.zero;
            SetLayerRecursively(screenCanvasRoot, ScreenLayer);

            screenCanvas = screenCanvasRoot.GetComponent<Canvas>();
            screenCanvas.renderMode = RenderMode.ScreenSpaceCamera;
            screenCanvas.worldCamera = screenCamera;
            screenCanvas.planeDistance = 1f;
            screenCanvas.pixelPerfect = false;
            screenCanvas.overrideSorting = true;
            screenCanvas.sortingOrder = 10;

            CanvasScaler scaler =
                screenCanvasRoot.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ConstantPixelSize;
            scaler.scaleFactor = 1f;
            scaler.referencePixelsPerUnit = 100f;

            RectTransform canvasRect =
                screenCanvasRoot.GetComponent<RectTransform>();
            canvasRect.anchorMin = Vector2.zero;
            canvasRect.anchorMax = Vector2.one;
            canvasRect.offsetMin = Vector2.zero;
            canvasRect.offsetMax = Vector2.zero;
            canvasRect.localScale = Vector3.one;

            CreatePanel(
                canvasRect,
                "Screen Background",
                0f,
                0f,
                CanvasSize.x,
                CanvasSize.y,
                new Color(0.004f, 0.009f, 0.027f, 1f)
            );

            BuildPersistentScreenOverlays(canvasRect);
        }

        private void BuildPersistentScreenOverlays(
            RectTransform canvasRect)
        {
            GameObject scanlineObject = new GameObject(
                "Modern LCD Scanline Layer",
                typeof(RectTransform),
                typeof(RawImage)
            );
            scanlineObject.transform.SetParent(canvasRect, false);
            SetLayerRecursively(scanlineObject, ScreenLayer);

            screenScanlineRoot =
                scanlineObject.GetComponent<RectTransform>();
            screenScanlineRoot.anchorMin = Vector2.zero;
            screenScanlineRoot.anchorMax = Vector2.one;
            screenScanlineRoot.offsetMin = Vector2.zero;
            screenScanlineRoot.offsetMax = Vector2.zero;

            RawImage scanlines =
                scanlineObject.GetComponent<RawImage>();
            scanlines.texture = screenScanlineTexture;
            scanlines.color = new Color(1f, 1f, 1f, 0.055f);
            scanlines.uvRect = new Rect(0f, 0f, 15f, 17f);
            scanlines.raycastTarget = false;

            GameObject transitionObject = new GameObject(
                "Modern Game Boy Screen Transition",
                typeof(RectTransform)
            );
            transitionObject.transform.SetParent(canvasRect, false);
            SetLayerRecursively(transitionObject, ScreenLayer);

            screenTransitionRoot =
                transitionObject.GetComponent<RectTransform>();
            screenTransitionRoot.anchorMin = Vector2.zero;
            screenTransitionRoot.anchorMax = Vector2.one;
            screenTransitionRoot.offsetMin = Vector2.zero;
            screenTransitionRoot.offsetMax = Vector2.zero;

            transitionFlash = CreatePanel(
                screenTransitionRoot,
                "Transition Blue Flash",
                0f,
                0f,
                CanvasSize.x,
                CanvasSize.y,
                new Color(0.08f, 0.32f, 0.68f, 0f)
            );
            transitionTopShutter = CreatePanel(
                screenTransitionRoot,
                "Transition Top Shutter",
                0f,
                CanvasSize.y * 0.25f,
                CanvasSize.x,
                CanvasSize.y * 0.5f,
                new Color(0.002f, 0.006f, 0.018f, 1f)
            );
            transitionBottomShutter = CreatePanel(
                screenTransitionRoot,
                "Transition Bottom Shutter",
                0f,
                -CanvasSize.y * 0.25f,
                CanvasSize.x,
                CanvasSize.y * 0.5f,
                new Color(0.002f, 0.006f, 0.018f, 1f)
            );
            transitionLine = CreatePanel(
                screenTransitionRoot,
                "Transition Luminous Line",
                0f,
                0f,
                CanvasSize.x,
                10f,
                new Color(0.20f, 0.72f, 1f, 1f)
            );
            AddOutline(
                transitionLine.gameObject,
                new Color(0.55f, 0.20f, 1f, 0.72f),
                2f
            );

            screenTransitionRoot.gameObject.SetActive(false);
        }

        private void ForceScreenRender()
        {
            if (screenCamera == null ||
                screenRenderTexture == null ||
                !screenRenderTexture.IsCreated())
            {
                return;
            }

            // BD AUTOMATIC SCREEN CAMERA RENDER SCHEDULING V10.11.30.21
            // The screen camera is already enabled by the presentation/power
            // owner before this method is called. Unity renders enabled cameras
            // later in the same frame, after Update and canvas batching. Calling
            // Camera.Render here created a second Metal render pass with a
            // transient depth attachment during scene restore. Keep the canvas
            // preparation, but let the existing camera own its single scheduled
            // render so screen power-on remains fully prepared without duplicate
            // load/store actions.
            Canvas.ForceUpdateCanvases();
        }

        private void BuildDeviceModel()
        {
            deviceVisualRoot = new GameObject(
                "BD Modern Upright Handheld"
            ).transform;
            deviceVisualRoot.SetParent(
                presentationRoot.transform,
                false
            );
            deviceVisualRoot.localPosition = DeviceRestPosition;
            deviceVisualRoot.localRotation = DeviceRestRotation;
            deviceVisualRoot.localScale = DeviceRestScale;
            SetLayerRecursively(
                deviceVisualRoot.gameObject,
                DeviceLayer
            );

            Mesh shellMesh = CreateRoundedRingPrismMesh(
                DeviceWidth,
                DeviceHeight,
                8.48f,
                9.70f,
                ScreenCenterY,
                DeviceDepth,
                0.70f,
                0.44f,
                8
            );
            CreateMeshObject(
                "Shell Front",
                deviceVisualRoot,
                shellMesh,
                shellMaterial,
                Vector3.zero,
                Quaternion.identity
            );

            Mesh outerEdgeMesh = CreateRoundedRingPrismMesh(
                DeviceWidth + 0.16f,
                DeviceHeight + 0.16f,
                DeviceWidth - 0.14f,
                DeviceHeight - 0.14f,
                0f,
                0.18f,
                0.78f,
                0.62f,
                10
            );
            CreateMeshObject(
                "Molded Outer Edge Bevel",
                deviceVisualRoot,
                outerEdgeMesh,
                bezelMaterial,
                new Vector3(0f, 0f, FrontSurfaceZ + 0.02f),
                Quaternion.identity
            );

            Mesh backMesh = CreateRoundedPrismMesh(
                DeviceWidth - 0.12f,
                DeviceHeight - 0.12f,
                0.36f,
                0.64f,
                10
            );
            CreateMeshObject(
                "Shell Back",
                deviceVisualRoot,
                backMesh,
                shellBackMaterial,
                new Vector3(0f, 0f, 0.52f),
                Quaternion.identity
            );

            Mesh sideRailMesh = CreateRoundedPrismMesh(
                0.18f,
                DeviceHeight - 1.10f,
                0.58f,
                0.09f,
                6
            );
            CreateMeshObject(
                "Left Mold Seam",
                deviceVisualRoot,
                sideRailMesh,
                darkMaterial,
                new Vector3(-4.83f, -0.02f, 0.18f),
                Quaternion.identity
            );
            CreateMeshObject(
                "Right Mold Seam",
                deviceVisualRoot,
                sideRailMesh,
                darkMaterial,
                new Vector3(4.83f, -0.02f, 0.18f),
                Quaternion.identity
            );

            Mesh bezelMesh = CreateRoundedRingPrismMesh(
                8.54f,
                9.76f,
                8.02f,
                9.20f,
                0f,
                0.16f,
                0.46f,
                0.34f,
                8
            );
            CreateMeshObject(
                "Screen Bezel",
                deviceVisualRoot,
                bezelMesh,
                bezelMaterial,
                new Vector3(0f, ScreenCenterY, -0.49f),
                Quaternion.identity
            );

            Mesh screenBackingMesh = CreateRoundedPrismMesh(
                8.02f,
                9.20f,
                0.10f,
                0.34f,
                8
            );
            CreateMeshObject(
                "Screen Backing",
                deviceVisualRoot,
                screenBackingMesh,
                darkMaterial,
                new Vector3(0f, ScreenCenterY, -0.36f),
                Quaternion.identity
            );

            GameObject display = GameObject.CreatePrimitive(
                PrimitiveType.Quad
            );
            display.name = "Screen Display";
            Destroy(display.GetComponent<Collider>());
            display.transform.SetParent(deviceVisualRoot, false);
            display.transform.localPosition =
                new Vector3(0f, ScreenCenterY, -0.555f);
            display.transform.localRotation = Quaternion.identity;
            display.transform.localScale =
                new Vector3(ScreenWidth, ScreenHeight, 1f);
            displayMaterial.mainTexture = screenRenderTexture;
            display.GetComponent<Renderer>().sharedMaterial =
                displayMaterial;
            SetLayerRecursively(display, DeviceLayer);

            Mesh glassMesh = CreateRoundedPrismMesh(
                8.20f,
                9.38f,
                0.075f,
                0.40f,
                10
            );
            CreateMeshObject(
                "Screen Glass",
                deviceVisualRoot,
                glassMesh,
                glassMaterial,
                new Vector3(0f, ScreenCenterY, -0.635f),
                Quaternion.identity
            );

            GameObject reflection = GameObject.CreatePrimitive(
                PrimitiveType.Quad
            );
            reflection.name = "Screen Glass Reflection";
            Destroy(reflection.GetComponent<Collider>());
            reflection.transform.SetParent(deviceVisualRoot, false);
            reflection.transform.localPosition =
                new Vector3(0.10f, ScreenCenterY + 0.06f, -0.700f);
            reflection.transform.localRotation =
                Quaternion.Euler(0f, 180f, 0f);
            reflection.transform.localScale =
                new Vector3(8.04f, 9.20f, 1f);
            reflection.GetComponent<Renderer>().sharedMaterial =
                reflectionMaterial;
            SetLayerRecursively(reflection, DeviceLayer);

            screenHitTargetRoot = new GameObject(
                "Screen Interaction Targets"
            ).transform;
            screenHitTargetRoot.SetParent(deviceVisualRoot, false);
            SetLayerRecursively(
                screenHitTargetRoot.gameObject,
                DeviceLayer
            );

            BuildDPad();
            BuildFaceButtons();
            BuildShortcutButtons();
            BuildSpeakerGrilles();
            BuildStatusLight();
        }

        private GameObject CreateCapQuad(
            string name,
            Transform parent,
            Material material,
            Vector2 size,
            float localZ)
        {
            GameObject cap = GameObject.CreatePrimitive(PrimitiveType.Quad);
            cap.name = name;
            Destroy(cap.GetComponent<Collider>());
            cap.transform.SetParent(parent, false);
            cap.transform.localPosition = new Vector3(0f, 0f, localZ);
            cap.transform.localRotation = Quaternion.identity;
            cap.transform.localScale = new Vector3(size.x, size.y, 1f);
            cap.GetComponent<Renderer>().sharedMaterial = material;
            SetLayerRecursively(cap, DeviceLayer);
            return cap;
        }

        private void BuildDPad()
        {
            Transform dpadRoot = new GameObject("DPadRoot").transform;
            dpadRoot.SetParent(deviceVisualRoot, false);
            dpadRoot.localPosition =
                new Vector3(-2.58f, -4.52f, ControlCenterZ);
            SetLayerRecursively(dpadRoot.gameObject, DeviceLayer);

            Mesh centerMesh = CreateRoundedPrismMesh(
                0.88f,
                0.88f,
                0.28f,
                0.18f,
                8
            );
            GameObject center = CreateMeshObject(
                "DPad Center",
                dpadRoot,
                centerMesh,
                dpadMaterial,
                Vector3.zero,
                Quaternion.identity
            );
            CreateCapQuad(
                "DPad Center Textured Cap",
                center.transform,
                dpadCenterCapMaterial,
                new Vector2(0.80f, 0.80f),
                -0.155f
            );

            GameObject up = CreateDPadArm(
                dpadRoot,
                "DPad Up Cap",
                new Vector3(0f, 0.82f, 0f),
                0.86f,
                0.96f,
                dpadUpCapMaterial
            );
            GameObject down = CreateDPadArm(
                dpadRoot,
                "DPad Down Cap",
                new Vector3(0f, -0.82f, 0f),
                0.86f,
                0.96f,
                dpadDownCapMaterial
            );
            GameObject left = CreateDPadArm(
                dpadRoot,
                "DPad Left Cap",
                new Vector3(-0.82f, 0f, 0f),
                0.96f,
                0.86f,
                dpadLeftCapMaterial
            );
            GameObject right = CreateDPadArm(
                dpadRoot,
                "DPad Right Cap",
                new Vector3(0.82f, 0f, 0f),
                0.96f,
                0.86f,
                dpadRightCapMaterial
            );

            GameObject accent = GameObject.CreatePrimitive(
                PrimitiveType.Sphere
            );
            accent.name = "DPad Center Accent";
            Destroy(accent.GetComponent<Collider>());
            accent.transform.SetParent(center.transform, false);
            accent.transform.localPosition =
                new Vector3(0f, 0f, -0.19f);
            accent.transform.localScale =
                new Vector3(0.18f, 0.18f, 0.07f);
            accent.GetComponent<Renderer>().sharedMaterial =
                accentMaterial;
            SetLayerRecursively(accent, DeviceLayer);

            CreateDPadTarget(
                dpadRoot,
                "DPad Up",
                new Vector3(0f, 0.82f, -0.18f),
                new Vector3(0.82f, 0.98f, 0.34f),
                BDModernHandheldControlTarget.ControlAction.DPadUp,
                up
            );
            CreateDPadTarget(
                dpadRoot,
                "DPad Down",
                new Vector3(0f, -0.82f, -0.18f),
                new Vector3(0.82f, 0.98f, 0.34f),
                BDModernHandheldControlTarget.ControlAction.DPadDown,
                down
            );
            CreateDPadTarget(
                dpadRoot,
                "DPad Left",
                new Vector3(-0.82f, 0f, -0.18f),
                new Vector3(0.98f, 0.82f, 0.34f),
                BDModernHandheldControlTarget.ControlAction.DPadLeft,
                left
            );
            CreateDPadTarget(
                dpadRoot,
                "DPad Right",
                new Vector3(0.82f, 0f, -0.18f),
                new Vector3(0.98f, 0.82f, 0.34f),
                BDModernHandheldControlTarget.ControlAction.DPadRight,
                right
            );
        }

        private GameObject CreateDPadArm(
            Transform parent,
            string name,
            Vector3 position,
            float width,
            float height,
            Material capMaterial)
        {
            Mesh mesh = CreateRoundedPrismMesh(
                width,
                height,
                0.28f,
                0.18f,
                8
            );
            GameObject arm = CreateMeshObject(
                name,
                parent,
                mesh,
                dpadMaterial,
                position,
                Quaternion.identity
            );
            CreateCapQuad(
                name + " Texture",
                arm.transform,
                capMaterial,
                new Vector2(width * 0.92f, height * 0.92f),
                -0.155f
            );
            return arm;
        }

        private void CreateDPadTarget(
            Transform parent,
            string name,
            Vector3 position,
            Vector3 size,
            BDModernHandheldControlTarget.ControlAction action,
            GameObject movingArm)
        {
            CreateControlTarget(
                name,
                parent,
                position,
                size,
                action,
                -1,
                movingArm != null ? movingArm.transform : null,
                movingArm != null
                    ? movingArm.GetComponent<Renderer>()
                    : null,
                0.12f
            );
        }

        private void BuildFaceButtons()
        {
            CreateFaceButton(
                "Button X",
                new Vector3(2.70f, -3.85f, ControlCenterZ),
                purpleButtonMaterial,
                buttonCapXMaterial,
                BDModernHandheldControlTarget.ControlAction.Primary
            );
            CreateFaceButton(
                "Button Y",
                new Vector3(1.88f, -4.68f, ControlCenterZ),
                purpleButtonMaterial,
                buttonCapYMaterial,
                BDModernHandheldControlTarget.ControlAction.Credits
            );
            CreateFaceButton(
                "Button A",
                new Vector3(3.55f, -4.68f, ControlCenterZ),
                warmButtonMaterial,
                buttonCapAMaterial,
                BDModernHandheldControlTarget.ControlAction.Progression
            );
            CreateFaceButton(
                "Button B",
                new Vector3(2.72f, -5.52f, ControlCenterZ),
                warmButtonMaterial,
                buttonCapBMaterial,
                BDModernHandheldControlTarget.ControlAction.ContextBackSettings
            );
        }

        private void CreateFaceButton(
            string name,
            Vector3 position,
            Material sideMaterial,
            Material capMaterial,
            BDModernHandheldControlTarget.ControlAction action)
        {
            GameObject root = new GameObject(name + " Root");
            root.transform.SetParent(deviceVisualRoot, false);
            root.transform.localPosition = position;
            SetLayerRecursively(root, DeviceLayer);

            GameObject button = GameObject.CreatePrimitive(
                PrimitiveType.Cylinder
            );
            button.name = name;
            button.transform.SetParent(root.transform, false);
            button.transform.localPosition = Vector3.zero;
            button.transform.localRotation = Quaternion.Euler(90f, 0f, 0f);
            button.transform.localScale = new Vector3(0.54f, 0.24f, 0.54f);

            Collider primitiveCollider = button.GetComponent<Collider>();
            if (primitiveCollider != null)
                Destroy(primitiveCollider);

            Renderer renderer = button.GetComponent<Renderer>();
            renderer.sharedMaterial = sideMaterial;
            SetLayerRecursively(button, DeviceLayer);

            CreateCapQuad(
                name + " Textured Face",
                root.transform,
                capMaterial,
                new Vector2(0.96f, 0.96f),
                -0.255f
            );

            CreateControlTarget(
                name + " Hit Target",
                deviceVisualRoot,
                new Vector3(position.x, position.y, HitTargetZ),
                new Vector3(1.02f, 1.02f, 0.40f),
                action,
                -1,
                root.transform,
                renderer,
                0.12f
            );
        }

        private void BuildShortcutButtons()
        {
            CreateShortcutButton(
                "Button Select",
                "SELECT",
                new Vector3(-0.66f, -3.82f, ControlCenterZ),
                BDModernHandheldControlTarget.ControlAction.Confirm
            );
            CreateShortcutButton(
                "Button Exit",
                "EXIT",
                new Vector3(0.66f, -3.82f, ControlCenterZ),
                BDModernHandheldControlTarget.ControlAction.Exit
            );
        }

        private void CreateShortcutButton(
            string name,
            string label,
            Vector3 position,
            BDModernHandheldControlTarget.ControlAction action)
        {
            Mesh mesh = CreateRoundedPrismMesh(
                1.18f,
                0.38f,
                0.22f,
                0.16f,
                6
            );
            GameObject button = CreateMeshObject(
                name,
                deviceVisualRoot,
                mesh,
                darkMaterial,
                position,
                Quaternion.identity
            );
            Renderer renderer = button.GetComponent<Renderer>();
            CreateCapQuad(
                name + " Textured Face",
                button.transform,
                shortcutCapMaterial,
                new Vector2(1.08f, 0.32f),
                -0.145f
            );

            CreateRecessedHardwareLabel(
                label,
                deviceVisualRoot,
                new Vector3(
                    position.x,
                    position.y - 0.50f,
                    FrontSurfaceZ - 0.025f
                ),
                0.024f
            );

            CreateControlTarget(
                name + " Hit Target",
                deviceVisualRoot,
                new Vector3(position.x, position.y, HitTargetZ),
                new Vector3(1.24f, 0.62f, 0.32f),
                action,
                -1,
                button.transform,
                renderer,
                0.12f
            );
        }

        private void BuildSpeakerGrilles()
        {
            Mesh left = CreateSpeakerGrilleMesh(
                5,
                4,
                0.23f,
                0.055f,
                0.055f
            );
            CreateMeshObject(
                "Speaker Grille Left",
                deviceVisualRoot,
                left,
                darkMaterial,
                new Vector3(-2.62f, -6.45f, FrontSurfaceZ - 0.04f),
                Quaternion.identity
            );

            Mesh right = CreateSpeakerGrilleMesh(
                5,
                4,
                0.23f,
                0.055f,
                0.055f
            );
            CreateMeshObject(
                "Speaker Grille Right",
                deviceVisualRoot,
                right,
                darkMaterial,
                new Vector3(2.62f, -6.45f, FrontSurfaceZ - 0.04f),
                Quaternion.identity
            );
        }

        private void BuildStatusLight()
        {
            GameObject light = GameObject.CreatePrimitive(
                PrimitiveType.Sphere
            );
            light.name = "Handheld Status Light";
            Destroy(light.GetComponent<Collider>());
            light.transform.SetParent(deviceVisualRoot, false);
            light.transform.localPosition =
                new Vector3(0f, 7.10f, FrontSurfaceZ - 0.08f);
            light.transform.localScale =
                new Vector3(0.16f, 0.16f, 0.08f);
            light.GetComponent<Renderer>().sharedMaterial =
                accentMaterial;
            SetLayerRecursively(light, DeviceLayer);
        }

        private void CreateRecessedHardwareLabel(
            string text,
            Transform parent,
            Vector3 localPosition,
            float characterSize)
        {
            // BD ENGRAVED SHORTCUT LABEL V10.11.30.42
            // The authored center and characterSize are unchanged. The dark face
            // sits deeper in the shell, while opposing near-surface rims create
            // an engraved/inset read instead of a bright raised print.
            CreateHardwareLabel(
                text,
                parent,
                localPosition + new Vector3(0.006f, -0.010f, -0.003f),
                characterSize,
                new Color(0.70f, 0.78f, 0.86f, 0.72f)
            );
            CreateHardwareLabel(
                text,
                parent,
                localPosition + new Vector3(-0.006f, 0.010f, -0.002f),
                characterSize,
                new Color(0.002f, 0.004f, 0.010f, 0.94f)
            );
            CreateHardwareLabel(
                text,
                parent,
                localPosition + new Vector3(0f, 0f, 0.010f),
                characterSize,
                new Color(0.055f, 0.075f, 0.095f, 1f)
            );
        }

        private TextMesh CreateHardwareLabel(
            string text,
            Transform parent,
            Vector3 localPosition,
            float characterSize,
            Color color)
        {
            GameObject labelObject = new GameObject(
                "Label " + text
            );
            labelObject.transform.SetParent(parent, false);
            labelObject.transform.localPosition = localPosition;
            labelObject.transform.localRotation = Quaternion.identity;
            SetLayerRecursively(labelObject, DeviceLayer);

            TextMesh label = labelObject.AddComponent<TextMesh>();
            label.text = text;
            label.font = uiFont;
            label.fontSize = 64;
            label.characterSize = characterSize;
            label.anchor = TextAnchor.MiddleCenter;
            label.alignment = TextAlignment.Center;
            label.color = color;
            label.fontStyle = FontStyle.Bold;
            return label;
        }

        private BDModernHandheldControlTarget CreateControlTarget(
            string name,
            Transform parent,
            Vector3 localPosition,
            Vector3 size,
            BDModernHandheldControlTarget.ControlAction action,
            int index,
            Transform movingPart,
            Renderer feedbackRenderer,
            float pressDistance)
        {
            GameObject targetObject = new GameObject(name);
            targetObject.transform.SetParent(parent, false);
            targetObject.transform.localPosition = localPosition;
            targetObject.transform.localScale = size;
            SetLayerRecursively(targetObject, DeviceLayer);

            BoxCollider collider =
                targetObject.AddComponent<BoxCollider>();
            collider.size = Vector3.one;

            BDModernHandheldControlTarget target =
                targetObject.AddComponent<
                    BDModernHandheldControlTarget>();
            target.Configure(
                action,
                index,
                movingPart,
                feedbackRenderer,
                pressDistance
            );

            persistentControls.Add(target);
            return target;
        }

        private void BuildAudio()
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
            audioSource.loop = false;
            audioSource.spatialBlend = 0f;
            audioSource.ignoreListenerPause = true;
            clickClip = CreateClickClip();
        }

        private AudioClip CreateClickClip()
        {
            const int sampleRate = 22050;
            const float duration = 0.055f;
            int count = Mathf.CeilToInt(sampleRate * duration);
            float[] data = new float[count];

            for (int index = 0; index < count; index++)
            {
                float t = index / (float)sampleRate;
                float envelope = Mathf.Exp(-t * 55f);
                float tone = Mathf.Sin(t * Mathf.PI * 2f * 620f);
                float tick = index < 36
                    ? (1f - index / 36f)
                    : 0f;
                data[index] =
                    (tone * 0.24f + tick * 0.22f) * envelope;
            }

            AudioClip clip = AudioClip.Create(
                "BD Handheld Tactile Click",
                count,
                1,
                sampleRate,
                stream: false
            );
            clip.SetData(data, 0);
            return clip;
        }

        private void PlayClick()
        {
            if (audioSource == null || clickClip == null)
                return;

            audioSource.PlayOneShot(
                clickClip,
                Mathf.Clamp01(BDGameSettings.SfxVolume) * 0.48f
            );
        }

        private void RefreshPageIfNeeded()
        {
            if (flow == null)
                return;

            EffectivePage effective = ResolveEffectivePage();
            if (displayedPageInitialized && effective == displayedPage)
                return;

            displayedPage = effective;
            displayedPageInitialized = true;
            selectedIndex = 0;
            BuildScreenPage(effective);
            UpdateBackdropForPage(effective);
        }

        private EffectivePage ResolveEffectivePage()
        {
            if (ShouldPresentFirstLaunchTutorial())
                return EffectivePage.FirstLaunchTutorial;

            if (flow == null)
                return EffectivePage.MainMenu;

            BDMainMenuFlow.HandheldPage flowPage =
                flow.CurrentHandheldPage;

            if (localPage == LocalPage.Credits &&
                flowPage != BDMainMenuFlow.HandheldPage.Loading &&
                flowPage != BDMainMenuFlow.HandheldPage.AbandonConfirm)
            {
                return EffectivePage.Credits;
            }

            if (localPage == LocalPage.QuitConfirm &&
                flowPage == BDMainMenuFlow.HandheldPage.MainMenu)
            {
                return EffectivePage.QuitConfirm;
            }

            switch (flowPage)
            {
                case BDMainMenuFlow.HandheldPage.Pause:
                    return EffectivePage.Pause;
                case BDMainMenuFlow.HandheldPage.Settings:
                    return EffectivePage.Settings;
                case BDMainMenuFlow.HandheldPage.Progression:
                    return EffectivePage.Progression;
                case BDMainMenuFlow.HandheldPage.AbandonConfirm:
                    return EffectivePage.AbandonConfirm;
                case BDMainMenuFlow.HandheldPage.NewRunConfirm:
                    return EffectivePage.NewRunConfirm;
                case BDMainMenuFlow.HandheldPage.Loading:
                    return EffectivePage.Loading;
                default:
                    return EffectivePage.MainMenu;
            }
        }

        private void BuildScreenPage(EffectivePage page)
        {
            ClearScreenPage();

            GameObject pageObject = new GameObject(
                "Page " + page,
                typeof(RectTransform)
            );
            pageObject.transform.SetParent(
                screenCanvasRoot.transform,
                false
            );
            SetLayerRecursively(pageObject, ScreenLayer);

            pageRoot = pageObject.GetComponent<RectTransform>();
            pageCanvasGroup = pageObject.AddComponent<CanvasGroup>();
            pageCanvasGroup.alpha = 1f;
            pageRoot.sizeDelta = CanvasSize;
            pageRoot.localPosition = Vector3.zero;
            pageRoot.localScale = Vector3.one;

            BuildTopBar(page);

            switch (page)
            {
                case EffectivePage.FirstLaunchTutorial:
                    BuildFirstLaunchTutorialPage();
                    break;
                case EffectivePage.Pause:
                    BuildPausePage();
                    break;
                case EffectivePage.Settings:
                    BuildSettingsPage();
                    break;
                case EffectivePage.Progression:
                    BuildProgressionPage();
                    break;
                case EffectivePage.Credits:
                    BuildCreditsPage();
                    break;
                case EffectivePage.QuitConfirm:
                    BuildQuitConfirmPage();
                    break;
                case EffectivePage.NewRunConfirm:
                    BuildProductionNewRunConfirmV1011349();
                    break;
                case EffectivePage.AbandonConfirm:
                    BuildAbandonConfirmPage();
                    break;
                case EffectivePage.Loading:
                    BuildLoadingPage();
                    break;
                default:
                    BuildMainMenuPage();
                    break;
            }

            if (page != EffectivePage.FirstLaunchTutorial)
                BuildFooter();
            selectedIndex = Mathf.Clamp(
                selectedIndex,
                0,
                Mathf.Max(0, rows.Count - 1)
            );
            UpdateSelectionVisuals();
            RefreshCharacterArt(force: true);

            if (screenScanlineRoot != null)
                screenScanlineRoot.SetAsLastSibling();
            if (screenTransitionRoot != null)
                screenTransitionRoot.SetAsLastSibling();

            BeginScreenTransition();
            ForceScreenRender();
        }

        private void ClearScreenPage()
        {
            rows.Clear();
            heroImage = null;
            newGameMemoryCard = null;
            newGameMemoryHeadingText = null;
            newGameMemoryStatusText = null;
            loadingFill = null;
            loadingPercentText = null;
            pageCanvasGroup = null;

            if (pageRoot != null)
            {
                pageRoot.gameObject.SetActive(false);
                Destroy(pageRoot.gameObject);
                pageRoot = null;
            }

            if (screenHitTargetRoot != null)
            {
                for (int index =
                         screenHitTargetRoot.childCount - 1;
                     index >= 0;
                     index--)
                {
                    GameObject child =
                        screenHitTargetRoot.GetChild(index).gameObject;
                    child.SetActive(false);
                    Destroy(child);
                }
            }
        }

        private void BuildTopBar(EffectivePage page)
        {
            Image bar = CreatePanel(
                pageRoot,
                "Top Bar",
                0f,
                500f,
                900f,
                58f,
                new Color(0.018f, 0.035f, 0.075f, 0.96f)
            );
            AddOutline(
                bar.gameObject,
                new Color(0.15f, 0.42f, 0.82f, 0.28f),
                1f
            );

            CreateText(
                bar.rectTransform,
                "Top Identity",
                "B&D // MEMORY HANDHELD",
                -285f,
                0f,
                300f,
                40f,
                20,
                TextAnchor.MiddleLeft,
                new Color(0.75f, 0.86f, 1f, 1f),
                FontStyle.Bold
            );

            CreateText(
                bar.rectTransform,
                "System Identity",
                "ADVENTURE SYSTEM",
                65f,
                0f,
                230f,
                40f,
                18,
                TextAnchor.MiddleCenter,
                new Color(0.68f, 0.48f, 1f, 1f),
                FontStyle.Bold
            );

            CreateText(
                bar.rectTransform,
                "Page Status",
                ResolvePageStatus(page),
                318f,
                0f,
                250f,
                40f,
                18,
                TextAnchor.MiddleRight,
                new Color(0.28f, 0.84f, 1f, 1f),
                FontStyle.Bold
            );
        }

        private string ResolvePageStatus(EffectivePage page)
        {
            switch (page)
            {
                case EffectivePage.FirstLaunchTutorial:
                    return "FIRST LAUNCH";
                case EffectivePage.Pause:
                    return "RUN PAUSED";
                case EffectivePage.Settings:
                    return "SETTINGS";
                case EffectivePage.Progression:
                    return "PROGRESSION";
                case EffectivePage.Credits:
                    return "CREDITS";
                case EffectivePage.QuitConfirm:
                    return "EXIT CONFIRM";
                case EffectivePage.NewRunConfirm:
                    return "NEW RUN CONFIRM";
                case EffectivePage.AbandonConfirm:
                    return "CONFIRM";
                case EffectivePage.Loading:
                    return "LOADING";
                default:
                    return "READY";
            }
        }

        private void BuildMainMenuPage()
        {
            if (TryBuildProductionMainMenuV1011349())
                return;
            CreateTitleBlock(
                "BOREDOM\n& DUNGEONS",
                "AN ADVENTURE REMEMBERED"
            );
            BuildHeroCard();

            float y = 236f;
            AddScreenRow(
                flow != null
                    ? flow.PrimaryRunActionLabel
                    : "START GAME",
                "Enter the maze and begin the adventure",
                RowAction.Primary,
                y,
                "X"
            );
            y -= 82f;
            AddScreenRow(
                "PROGRESSION",
                "View persistent milestones and future upgrades",
                RowAction.OpenProgression,
                y,
                "A"
            );
            y -= 82f;
            AddScreenRow(
                "SETTINGS",
                "Audio, controls, display and accessibility",
                RowAction.OpenSettings,
                y,
                "B"
            );
            y -= 82f;
            AddScreenRow(
                "CREDITS",
                "The people and ideas behind the adventure",
                RowAction.OpenCredits,
                y,
                "Y"
            );
            y -= 82f;
            AddScreenRow(
                "QUIT",
                "Exit the game",
                RowAction.Quit,
                y,
                "EXIT"
            );

            BuildRunCard(
                "ADVENTURE MEMORY",
                flow != null && flow.IsRunActive
                    ? "A run is active"
                    : "A new path is ready"
            );
            BuildNextCard(
                "THE MAZE AWAITS",
                "Ride into the unknown. Clear rooms, discover secrets and find the way home."
            );
        }

        private void BuildPausePage()
        {
            if (TryBuildProductionPauseV1011349())
                return;
            CreateTitleBlock(
                "PAUSED",
                "RUN CONTROL"
            );

            Image panel = CreatePanel(
                pageRoot,
                "Pause Internal Menu Panel",
                0f,
                18f,
                720f,
                600f,
                new Color(0.010f, 0.024f, 0.052f, 0.97f)
            );
            AddOutline(
                panel.gameObject,
                new Color(0.16f, 0.46f, 0.90f, 0.64f),
                2f
            );

            float y = 170f;
            AddScreenRow(
                "RESUME",
                "Return to the current run",
                RowAction.Primary,
                y,
                "SELECT"
            );
            y -= 98f;
            AddScreenRow(
                "PROGRESSION",
                "Review persistent progress",
                RowAction.OpenProgression,
                y,
                "SELECT"
            );
            y -= 98f;
            AddScreenRow(
                "SETTINGS",
                "Adjust the experience",
                RowAction.OpenSettings,
                y,
                "SELECT"
            );
            y -= 98f;
            AddScreenRow(
                "RETURN TO MAIN MENU",
                "End this run after confirmation",
                RowAction.ReturnToMainMenu,
                y,
                "EXIT"
            );

            CreateText(
                panel.rectTransform,
                "Pause Internal Status",
                "RUN PAUSED  //  SELECT AN OPTION",
                0f,
                -245f,
                620f,
                54f,
                18,
                TextAnchor.MiddleCenter,
                new Color(0.72f, 0.88f, 1f, 1f),
                FontStyle.Bold
            );
        }

        private void BuildSettingsPage()
        {
            if (TryBuildProductionSettingsV1011349())
                return;
            CreateTitleBlock(
                "SETTINGS",
                "TUNE THE HANDHELD AND THE ADVENTURE"
            );
            BuildCompactArtCard(
                settingsArtTexture,
                "Settings Artwork"
            );

            float y = 305f;
            AddScreenRow(
                "MASTER VOLUME",
                string.Empty,
                RowAction.MasterVolume,
                y,
                "◀ ▶"
            );
            y -= 67f;
            AddScreenRow(
                "MUSIC VOLUME",
                string.Empty,
                RowAction.MusicVolume,
                y,
                "◀ ▶"
            );
            y -= 67f;
            AddScreenRow(
                "SFX VOLUME",
                string.Empty,
                RowAction.SfxVolume,
                y,
                "◀ ▶"
            );
            y -= 67f;
            AddScreenRow(
                "MOUSE SENSITIVITY",
                string.Empty,
                RowAction.MouseSensitivity,
                y,
                "◀ ▶"
            );
            y -= 67f;
            AddScreenRow(
                "CAMERA SHAKE",
                string.Empty,
                RowAction.CameraShake,
                y,
                "◀ ▶"
            );
            y -= 67f;
            AddScreenRow(
                "QUALITY",
                string.Empty,
                RowAction.Quality,
                y,
                "◀ ▶"
            );
            y -= 67f;
            AddScreenRow(
                "FULLSCREEN",
                string.Empty,
                RowAction.Fullscreen,
                y,
                "SELECT"
            );
            y -= 67f;
            AddScreenRow(
                "V-SYNC / TARGET FPS",
                string.Empty,
                RowAction.VSync,
                y,
                "SELECT / ◀ ▶"
            );
            y -= 67f;
            AddScreenRow(
                "RESET DEFAULTS",
                "Restore the recommended settings",
                RowAction.ResetDefaults,
                y,
                "SELECT"
            );
            y -= 67f;
            AddScreenRow(
                "BACK",
                "Return to the previous page",
                RowAction.Back,
                y,
                "B"
            );

            RefreshSettingsRowValues();
            InitializeSettingsProfessionalLayoutV1011381();
        }

        private void BuildProgressionPage()
        {
            if (TryBuildProductionProgressionV1011349())
                return;
            CreateTitleBlock(
                "PROGRESSION",
                "THE MEMORY THAT SURVIVES BETWEEN RUNS"
            );
            BuildHeroCard(progressionArtTexture);

            Image panel = CreatePanel(
                pageRoot,
                "Progression Panel",
                -215f,
                65f,
                430f,
                420f,
                new Color(0.018f, 0.035f, 0.075f, 0.94f)
            );
            AddOutline(
                panel.gameObject,
                new Color(0.36f, 0.20f, 0.78f, 0.70f),
                2f
            );

            CreateText(
                panel.rectTransform,
                "Progression Heading",
                "PERSISTENT MEMORY",
                0f,
                178f,
                370f,
                48f,
                26,
                TextAnchor.MiddleLeft,
                Color.white,
                FontStyle.Bold
            );
            CreateText(
                panel.rectTransform,
                "Mother State",
                BDGameProgress.MotherDefeated
                    ? "MOTHER RESTORED // YES"
                    : "MOTHER RESTORED // NOT YET",
                0f,
                105f,
                370f,
                48f,
                20,
                TextAnchor.MiddleLeft,
                new Color(0.54f, 0.86f, 1f, 1f),
                FontStyle.Bold
            );
            CreateText(
                panel.rectTransform,
                "Progression Body",
                "This is the permanent entry point for cross-run progress. Future upgrades, blessings and discoveries will appear here without changing the menu architecture.",
                0f,
                -20f,
                370f,
                175f,
                19,
                TextAnchor.UpperLeft,
                new Color(0.76f, 0.80f, 0.90f, 1f),
                FontStyle.Normal
            );

            AddScreenRow(
                "BACK",
                "Return to the previous page",
                RowAction.Back,
                -305f,
                "B"
            );
        }

        private void BuildCreditsPage()
        {
            if (TryBuildProductionCreditsV1011349())
                return;
            CreateTitleBlock(
                "CREDITS",
                "THE PEOPLE BEHIND THE ADVENTURE"
            );
            BuildHeroCard(creditsArtTexture);

            Image panel = CreatePanel(
                pageRoot,
                "Credits Panel",
                -215f,
                55f,
                430f,
                440f,
                new Color(0.018f, 0.035f, 0.075f, 0.94f)
            );
            AddOutline(
                panel.gameObject,
                new Color(0.15f, 0.46f, 0.90f, 0.62f),
                2f
            );

            CreateText(
                panel.rectTransform,
                "Credits Body",
                "CREATED BY BARAK\n\nA living production project built with Unity, original systems, iterative design and a commitment to preserving every approved idea.\n\nThank you for entering the maze.",
                0f,
                0f,
                370f,
                350f,
                22,
                TextAnchor.MiddleCenter,
                new Color(0.88f, 0.92f, 1f, 1f),
                FontStyle.Normal
            );

            AddScreenRow(
                "BACK",
                "Return to the main menu",
                RowAction.Back,
                -305f,
                "B"
            );
        }

        private void BuildQuitConfirmPage()
        {
            if (TryBuildProductionQuitConfirmV1011349())
                return;
            CreateTitleBlock(
                "EXIT THE GAME?",
                "ARE YOU SURE YOU WANT TO LEAVE?"
            );
            BuildHeroCard(quitArtTexture);

            Image panel = CreatePanel(
                pageRoot,
                "Quit Confirmation Panel",
                -215f,
                55f,
                430f,
                300f,
                new Color(0.08f, 0.025f, 0.035f, 0.94f)
            );
            AddOutline(
                panel.gameObject,
                new Color(0.86f, 0.18f, 0.28f, 0.78f),
                2f
            );
            CreateText(
                panel.rectTransform,
                "Quit Confirmation Text",
                "Quit Boredom & Dungeons?\n\nUnsaved runtime state will be closed.",
                0f,
                0f,
                370f,
                220f,
                24,
                TextAnchor.MiddleCenter,
                new Color(0.96f, 0.90f, 0.92f, 1f),
                FontStyle.Normal
            );

            AddScreenRow(
                "YES — EXIT GAME",
                "Close the application",
                RowAction.ConfirmQuit,
                -235f,
                "SELECT"
            );
            AddScreenRow(
                "NO — STAY HERE",
                "Return to the previous page",
                RowAction.CancelQuit,
                -320f,
                "B"
            );
        }

        private void BuildAbandonConfirmPage()
        {
            if (TryBuildProductionAbandonConfirmV1011349())
                return;
            CreateTitleBlock(
                "LEAVE THIS RUN?",
                "CURRENT PROGRESS IN THIS RUN WILL END"
            );

            BuildHeroCard(quitArtTexture);

            Image panel = CreatePanel(
                pageRoot,
                "Confirmation Panel",
                -215f,
                55f,
                430f,
                300f,
                new Color(0.08f, 0.025f, 0.035f, 0.94f)
            );
            AddOutline(
                panel.gameObject,
                new Color(0.86f, 0.18f, 0.28f, 0.78f),
                2f
            );
            CreateText(
                panel.rectTransform,
                "Confirmation Text",
                "Return to the main menu?\n\nThe scene will reload into a clean menu state.",
                0f,
                0f,
                370f,
                220f,
                24,
                TextAnchor.MiddleCenter,
                new Color(0.96f, 0.90f, 0.92f, 1f),
                FontStyle.Normal
            );

            AddScreenRow(
                "YES — RETURN TO MAIN MENU",
                "Confirm abandon and reload",
                RowAction.ConfirmAbandon,
                -225f,
                "SELECT"
            );
            AddScreenRow(
                "NO — KEEP PLAYING",
                "Return to Pause",
                RowAction.CancelAbandon,
                -310f,
                "B"
            );
        }

        private void BuildLoadingPage()
        {
            CreateTitleBlock(
                "LOADING",
                "BUILDING A CLEAN ADVENTURE STATE"
            );

            Image panel = CreatePanel(
                pageRoot,
                "Loading Panel",
                0f,
                20f,
                760f,
                320f,
                new Color(0.018f, 0.035f, 0.075f, 0.94f)
            );
            AddOutline(
                panel.gameObject,
                new Color(0.15f, 0.52f, 1f, 0.72f),
                2f
            );

            Image track = CreatePanel(
                panel.rectTransform,
                "Loading Track",
                0f,
                -30f,
                620f,
                34f,
                new Color(0.02f, 0.04f, 0.08f, 1f)
            );

            loadingFill = CreatePanel(
                track.rectTransform,
                "Loading Fill",
                -300f,
                0f,
                20f,
                24f,
                new Color(0.12f, 0.52f, 1f, 1f)
            );
            loadingFill.rectTransform.pivot =
                new Vector2(0f, 0.5f);

            loadingPercentText = CreateText(
                panel.rectTransform,
                "Loading Percent",
                "0%",
                0f,
                60f,
                300f,
                80f,
                40,
                TextAnchor.MiddleCenter,
                Color.white,
                FontStyle.Bold
            );
        }

        private void CreateTitleBlock(
            string title,
            string subtitle)
        {
            bool multilineTitle = title.Contains("\n");
            int titleFontSize = multilineTitle
                ? 48
                : title.Length >= 11
                    ? 46
                    : title.Length >= 9
                        ? 52
                        : 62;

            Text titleText = CreateText(
                pageRoot,
                "Page Title",
                title,
                -225f,
                394f,
                480f,
                150f,
                titleFontSize,
                TextAnchor.MiddleCenter,
                new Color(0.90f, 0.96f, 1f, 1f),
                FontStyle.Bold
            );
            AddOutline(
                titleText.gameObject,
                new Color(0.10f, 0.42f, 0.90f, 0.80f),
                2f
            );

            CreateText(
                pageRoot,
                "Page Subtitle",
                subtitle,
                -215f,
                316f,
                510f,
                44f,
                17,
                TextAnchor.MiddleCenter,
                new Color(0.28f, 0.72f, 1f, 1f),
                FontStyle.Bold
            );
        }

        private void BuildHeroCard(Texture2D initialTexture = null)
        {
            Image frame = CreatePanel(
                pageRoot,
                "Hero Art Frame",
                275f,
                218f,
                320f,
                408f,
                new Color(0.012f, 0.025f, 0.055f, 0.96f)
            );
            AddOutline(
                frame.gameObject,
                new Color(0.16f, 0.52f, 1f, 0.72f),
                2f
            );

            heroImage = CreateRawImage(
                frame.rectTransform,
                "Context Artwork",
                0f,
                0f,
                304f,
                392f,
                initialTexture != null
                    ? initialTexture
                    : Texture2D.blackTexture,
                Color.white
            );
        }

        private void BuildCompactArtCard(
            Texture2D texture,
            string objectName)
        {
            Image frame = CreatePanel(
                pageRoot,
                objectName + " Frame",
                305f,
                26f,
                260f,
                720f,
                new Color(0.012f, 0.025f, 0.055f, 0.96f)
            );
            AddOutline(
                frame.gameObject,
                new Color(0.16f, 0.52f, 1f, 0.62f),
                2f
            );
            CreateRawImage(
                frame.rectTransform,
                objectName,
                0f,
                0f,
                244f,
                704f,
                texture != null
                    ? texture
                    : Texture2D.blackTexture,
                Color.white
            );
        }

        private void BuildRunCard(
            string heading,
            string status)
        {
            Image panel = CreatePanel(
                pageRoot,
                "New Game Memory Card",
                275f,
                -78f,
                320f,
                128f,
                new Color(0.014f, 0.030f, 0.065f, 0.97f)
            );
            newGameMemoryCard = panel.gameObject;
            AddOutline(
                panel.gameObject,
                new Color(0.10f, 0.58f, 0.88f, 0.70f),
                2f
            );

            newGameMemoryHeadingText = CreateText(
                panel.rectTransform,
                "New Game Card Heading",
                heading,
                0f,
                34f,
                272f,
                34f,
                18,
                TextAnchor.MiddleCenter,
                new Color(0.22f, 0.80f, 1f, 1f),
                FontStyle.Bold
            );
            newGameMemoryStatusText = CreateText(
                panel.rectTransform,
                "New Game Card Status",
                status,
                0f,
                -20f,
                272f,
                58f,
                25,
                TextAnchor.MiddleCenter,
                Color.white,
                FontStyle.Bold
            );

            // The small card is intentionally text-only. Character art lives
            // only in the large hero panel for Start Game / New Run.
            UpdateNewGameMemoryCardVisibility();
        }

        private void UpdateNewGameMemoryCardVisibility()
        {
            if (TryUpdateProductionNewGameMemoryCardV1011349())
                return;
            if (newGameMemoryCard == null)
                return;

            bool shouldShow = displayedPage == EffectivePage.MainMenu;
            newGameMemoryCard.SetActive(shouldShow);
            if (!shouldShow ||
                selectedIndex < 0 ||
                selectedIndex >= rows.Count)
            {
                return;
            }

            string heading;
            string status;
            switch (rows[selectedIndex].action)
            {
                case RowAction.OpenProgression:
                    heading = "PERSISTENT MEMORY";
                    status = "Milestones and future upgrades";
                    break;
                case RowAction.OpenSettings:
                    heading = "SYSTEM CONFIGURATION";
                    status = "Audio, controls and display";
                    break;
                case RowAction.OpenCredits:
                    heading = "BEHIND THE ADVENTURE";
                    status = "People, ideas and production";
                    break;
                case RowAction.Quit:
                    heading = "LEAVE THE HANDHELD";
                    status = "Exit only after confirmation";
                    break;
                default:
                    heading = "ADVENTURE MEMORY";
                    status = flow != null && flow.IsRunActive
                        ? "A run is active"
                        : "A new path is ready";
                    break;
            }

            if (newGameMemoryHeadingText != null)
                newGameMemoryHeadingText.text = heading;
            if (newGameMemoryStatusText != null)
                newGameMemoryStatusText.text = status;
        }

        private void BuildNextCard(
            string heading,
            string body)
        {
            Image panel = CreatePanel(
                pageRoot,
                "Next Card",
                0f,
                -350f,
                860f,
                132f,
                new Color(0.014f, 0.026f, 0.052f, 0.97f)
            );
            AddOutline(
                panel.gameObject,
                new Color(0.26f, 0.16f, 0.54f, 0.58f),
                1.5f
            );

            CreateText(
                panel.rectTransform,
                "Next Heading",
                heading,
                -305f,
                30f,
                210f,
                42f,
                18,
                TextAnchor.MiddleLeft,
                new Color(0.68f, 0.42f, 1f, 1f),
                FontStyle.Bold
            );
            CreateText(
                panel.rectTransform,
                "Next Body",
                body,
                82f,
                -18f,
                590f,
                76f,
                20,
                TextAnchor.MiddleLeft,
                new Color(0.78f, 0.82f, 0.91f, 1f),
                FontStyle.Normal
            );
        }

        private string ResolveHealthSummary()
        {
            BDHealth health = ResolvePlayerHealth();
            if (health == null)
                return "RUN ACTIVE";

            return "HEALTH " +
                Mathf.CeilToInt(health.CurrentHealth) +
                " / " +
                Mathf.CeilToInt(health.MaxHealth);
        }

        private BDHealth ResolvePlayerHealth()
        {
            BDPlayerMarker[] markers =
                UnityEngine.Object.FindObjectsByType<BDPlayerMarker>(
                    FindObjectsInactive.Include,
                    FindObjectsSortMode.None
                );

            for (int pass = 0; pass < 2; pass++)
            {
                bool requireActive = pass == 0;
                for (int index = 0; index < markers.Length; index++)
                {
                    BDPlayerMarker marker = markers[index];
                    if (marker == null ||
                        marker.gameObject.activeInHierarchy != requireActive)
                    {
                        continue;
                    }

                    BDHealth health =
                        marker.GetComponentInParent<BDHealth>();
                    if (health != null)
                        return health;
                }
            }

            return null;
        }

        private void AddScreenRow(
            string label,
            string subtitle,
            RowAction action,
            float y,
            string badge)
        {
            int index = rows.Count;
            bool isPauseInternalMenu =
                displayedPage == EffectivePage.Pause;
            float width = displayedPage == EffectivePage.Settings
                ? 560f
                : isPauseInternalMenu
                    ? 620f
                    : 470f;
            float x = displayedPage == EffectivePage.Settings
                ? -150f
                : isPauseInternalMenu
                    ? 0f
                    : -215f;
            float height = displayedPage == EffectivePage.Settings
                ? 58f
                : isPauseInternalMenu
                    ? 78f
                    : 72f;
            ResolveProductionMenuRowGeometryV1011349(
                ref x,
                ref width,
                ref height
            );

            Image background = CreatePanel(
                pageRoot,
                "Row " + label,
                x,
                y,
                width,
                height,
                new Color(0.018f, 0.035f, 0.070f, 0.96f)
            );
            Outline outline = AddOutline(
                background.gameObject,
                new Color(0f, 0f, 0f, 0f),
                1.5f
            );

            Image icon = CreatePanel(
                background.rectTransform,
                "Icon",
                -width * 0.5f + 39f,
                0f,
                54f,
                height - 12f,
                ResolveActionColor(action)
            );
            AddOutline(
                icon.gameObject,
                new Color(1f, 1f, 1f, 0.16f),
                1f
            );

            CreateText(
                icon.rectTransform,
                "Icon Glyph",
                ResolveActionGlyph(action),
                0f,
                0f,
                48f,
                48f,
                24,
                TextAnchor.MiddleCenter,
                Color.white,
                FontStyle.Bold
            );
            BuildProductionActionIconV1011349(
                icon.rectTransform,
                action
            );

            Text labelText = CreateText(
                background.rectTransform,
                "Label",
                label,
                16f,
                subtitle.Length > 0 ? 12f : 0f,
                width - 150f,
                34f,
                displayedPage == EffectivePage.Settings ? 20 : 23,
                TextAnchor.MiddleLeft,
                Color.white,
                FontStyle.Bold
            );

            Text subtitleText = CreateText(
                background.rectTransform,
                "Subtitle",
                subtitle,
                16f,
                -17f,
                width - 150f,
                28f,
                15,
                TextAnchor.MiddleLeft,
                new Color(0.68f, 0.73f, 0.84f, 1f),
                FontStyle.Normal
            );

            Text badgeText = CreateText(
                background.rectTransform,
                "Badge",
                badge,
                width * 0.5f - 45f,
                0f,
                70f,
                38f,
                17,
                TextAnchor.MiddleCenter,
                new Color(0.82f, 0.88f, 1f, 1f),
                FontStyle.Bold
            );

            rows.Add(
                new ScreenRowVisual
                {
                    action = action,
                    rect = background.rectTransform,
                    background = background,
                    outline = outline,
                    label = labelText,
                    subtitle = subtitleText,
                    badge = badgeText
                }
            );

            CreateScreenItemTarget(
                index,
                x,
                y,
                width,
                height
            );
        }

        private Color ResolveActionColor(RowAction action)
        {
            Color productionColorV1011349;
            if (TryResolveProductionActionColorV1011349(
                    action,
                    out productionColorV1011349))
            {
                return productionColorV1011349;
            }
            switch (action)
            {
                case RowAction.Primary:
                    return new Color(0.02f, 0.32f, 0.70f, 1f);
                case RowAction.OpenProgression:
                    return new Color(0.28f, 0.08f, 0.52f, 1f);
                case RowAction.OpenSettings:
                    return new Color(0.48f, 0.16f, 0.045f, 1f);
                case RowAction.Quit:
                case RowAction.ReturnToMainMenu:
                case RowAction.ConfirmQuit:
                case RowAction.ConfirmAbandon:
                    return new Color(0.52f, 0.05f, 0.12f, 1f);
                case RowAction.CancelQuit:
                case RowAction.CancelAbandon:
                case RowAction.Back:
                    return new Color(0.05f, 0.26f, 0.42f, 1f);
                default:
                    return new Color(0.04f, 0.24f, 0.30f, 1f);
            }
        }

        private string ResolveActionGlyph(RowAction action)
        {
            string productionGlyphV1011349;
            if (TryResolveProductionActionGlyphV1011349(
                    action,
                    out productionGlyphV1011349))
            {
                return productionGlyphV1011349;
            }
            switch (action)
            {
                case RowAction.Primary:
                    return "▶";
                case RowAction.OpenProgression:
                    return "✦";
                case RowAction.OpenSettings:
                    return ResolveSupportedGlyph("⚙", "≡", "S");
                case RowAction.OpenCredits:
                    return "▣";
                case RowAction.Quit:
                case RowAction.ReturnToMainMenu:
                    return "↪";
                case RowAction.Back:
                case RowAction.CancelQuit:
                case RowAction.CancelAbandon:
                    return "←";
                case RowAction.ConfirmQuit:
                case RowAction.ConfirmAbandon:
                    return "✓";
                default:
                    return "•";
            }
        }

        private string ResolveSupportedGlyph(
            string preferred,
            string secondary,
            string fallback)
        {
            if (uiFont != null && uiFont.HasCharacter(preferred[0]))
                return preferred;
            if (uiFont != null && uiFont.HasCharacter(secondary[0]))
                return secondary;
            return fallback;
        }

        private void CreateScreenItemTarget(
            int index,
            float canvasX,
            float canvasY,
            float canvasWidth,
            float canvasHeight)
        {
            if (screenHitTargetRoot == null)
                return;

            GameObject targetObject = new GameObject(
                "Screen Item Target " + index
            );
            targetObject.transform.SetParent(
                screenHitTargetRoot,
                false
            );
            targetObject.transform.localPosition =
                CanvasToDevicePosition(canvasX, canvasY);
            targetObject.transform.localScale =
                new Vector3(
                    canvasWidth / CanvasSize.x * ScreenWidth,
                    canvasHeight / CanvasSize.y * ScreenHeight,
                    0.08f
                );
            SetLayerRecursively(targetObject, DeviceLayer);

            BoxCollider collider =
                targetObject.AddComponent<BoxCollider>();
            collider.size = Vector3.one;

            BDModernHandheldControlTarget target =
                targetObject.AddComponent<
                    BDModernHandheldControlTarget>();
            target.Configure(
                BDModernHandheldControlTarget.ControlAction.ScreenItem,
                index,
                null,
                null,
                0f
            );
        }

        private Vector3 CanvasToDevicePosition(
            float canvasX,
            float canvasY)
        {
            return new Vector3(
                canvasX / CanvasSize.x * ScreenWidth,
                ScreenCenterY +
                canvasY / CanvasSize.y * ScreenHeight,
                -0.27f
            );
        }

        private void BuildFooter()
        {
            Image footer = CreatePanel(
                pageRoot,
                "Control Footer",
                0f,
                -505f,
                900f,
                56f,
                new Color(0.012f, 0.024f, 0.050f, 0.98f)
            );
            AddOutline(
                footer.gameObject,
                new Color(0.14f, 0.30f, 0.58f, 0.45f),
                1f
            );

            string controls = ResolveControlLegend();

            CreateText(
                footer.rectTransform,
                "Control Legend",
                controls,
                0f,
                0f,
                880f,
                44f,
                12,
                TextAnchor.MiddleCenter,
                new Color(0.78f, 0.84f, 0.94f, 1f),
                FontStyle.Bold
            );
        }


        private string ResolveControlLegend()
        {
            if (displayedPage == EffectivePage.FirstLaunchTutorial)
            {
                return "FOLLOW THE ON-SCREEN LESSON  •  PHYSICAL CONTROLS / KEYBOARD / GAMEPAD / TOUCH  •  EXIT OPENS CONFIRMATION";
            }

            if (displayedPage == EffectivePage.MainMenu)
            {
                return "D-PAD / ARROWS / WASD NAVIGATE  •  SELECT ENTERS  •  EXIT CONFIRMS  •  X NEW GAME  •  A PROGRESSION  •  B SETTINGS  •  Y CREDITS";
            }

            return "D-PAD / ARROWS / WASD NAVIGATE  •  SELECT ENTERS  •  EXIT CONFIRMS  •  B BACK";
        }

        private void RefreshSettingsRowValues()
        {
            for (int index = 0; index < rows.Count; index++)
            {
                ScreenRowVisual row = rows[index];
                if (row == null || row.subtitle == null)
                    continue;

                switch (row.action)
                {
                    case RowAction.MasterVolume:
                        row.subtitle.text =
                            Mathf.RoundToInt(
                                BDGameSettings.MasterVolume * 100f
                            ) + "%";
                        break;
                    case RowAction.MusicVolume:
                        row.subtitle.text =
                            Mathf.RoundToInt(
                                BDGameSettings.MusicVolume * 100f
                            ) + "%";
                        break;
                    case RowAction.SfxVolume:
                        row.subtitle.text =
                            Mathf.RoundToInt(
                                BDGameSettings.SfxVolume * 100f
                            ) + "%";
                        break;
                    case RowAction.MouseSensitivity:
                        row.subtitle.text =
                            BDGameSettings.MouseSensitivityMultiplier
                                .ToString("0.00") + "×";
                        break;
                    case RowAction.CameraShake:
                        row.subtitle.text =
                            Mathf.RoundToInt(
                                BDGameSettings.CameraShakeIntensity * 100f
                            ) + "%";
                        break;
                    case RowAction.Quality:
                        row.subtitle.text =
                            BDGameSettings.QualityName;
                        break;
                    case RowAction.Fullscreen:
                        row.subtitle.text =
                            BDGameSettings.Fullscreen ? "ON" : "OFF";
                        break;
                    case RowAction.VSync:
                        row.subtitle.text =
                            (BDGameSettings.VSync ? "V-SYNC ON" : "V-SYNC OFF") +
                            "  •  " +
                            BDGameSettings.TargetFpsLabel +
                            " FPS";
                        break;
                }
            }
        }

        private void RefreshCharacterArt(bool force)
        {
            BDPlayableCharacterKind character =
                BDPlayableCharacterIdentity.ActiveCharacter;
            Texture2D texture = ResolveContextArtwork(character);

            if (texture == null)
                texture = Texture2D.blackTexture;

            bool alreadyApplied =
                heroImage != null && heroImage.texture == texture;

            if (!force && alreadyApplied)
                return;

            if (heroImage != null)
                heroImage.texture = texture;

        }

        private Texture2D ResolveContextArtwork(
            BDPlayableCharacterKind character)
        {
            Texture2D productionArtworkV1011349;
            if (TryResolveProductionContextArtworkV1011349(
                    character,
                    out productionArtworkV1011349))
            {
                return productionArtworkV1011349;
            }
            RowAction selectedAction =
                selectedIndex >= 0 && selectedIndex < rows.Count
                    ? rows[selectedIndex].action
                    : RowAction.None;

            switch (displayedPage)
            {
                case EffectivePage.MainMenu:
                    switch (selectedAction)
                    {
                        case RowAction.OpenProgression:
                            return progressionArtTexture;
                        case RowAction.OpenSettings:
                            return settingsArtTexture;
                        case RowAction.OpenCredits:
                            return creditsArtTexture;
                        case RowAction.Quit:
                            return quitArtTexture;
                        default:
                            // Only Start Game / New Run is protagonist-aware.
                            return character == BDPlayableCharacterKind.Girl
                                ? girlHeroTexture
                                : boyHeroTexture;
                    }

                case EffectivePage.Pause:
                    switch (selectedAction)
                    {
                        case RowAction.OpenProgression:
                            return progressionArtTexture;
                        case RowAction.OpenSettings:
                            return settingsArtTexture;
                        case RowAction.ReturnToMainMenu:
                            return quitArtTexture;
                        default:
                            return resumeArtTexture;
                    }

                case EffectivePage.Progression:
                    return progressionArtTexture;
                case EffectivePage.Settings:
                    return settingsArtTexture;
                case EffectivePage.Credits:
                    return creditsArtTexture;
                case EffectivePage.QuitConfirm:
                    return quitArtTexture;
                case EffectivePage.AbandonConfirm:
                    return quitArtTexture;
                default:
                    return resumeArtTexture;
            }
        }

        private void UpdateSelectionVisuals()
        {
            for (int index = 0; index < rows.Count; index++)
            {
                ScreenRowVisual row = rows[index];
                bool selected = index == selectedIndex;

                row.background.color = selected
                    ? new Color(0.025f, 0.16f, 0.38f, 0.98f)
                    : new Color(0.018f, 0.035f, 0.070f, 0.96f);
                row.outline.effectColor = selected
                    ? new Color(0.16f, 0.68f, 1f, 0.92f)
                    : new Color(0f, 0f, 0f, 0f);
                row.rect.localScale = selected
                    ? new Vector3(1.018f, 1.018f, 1f)
                    : Vector3.one;
                row.label.color = selected
                    ? Color.white
                    : new Color(0.82f, 0.86f, 0.92f, 1f);
            }

            RefreshCharacterArt(force: true);
            UpdateNewGameMemoryCardVisibility();
        }

        private void NavigateVertical(int direction)
        {
            if (rows.Count == 0 || direction == 0)
                return;

            selectedIndex =
                (selectedIndex + direction) % rows.Count;
            if (selectedIndex < 0)
                selectedIndex += rows.Count;

            UpdateSelectionVisuals();
            PulsePersistentControl(
                direction < 0
                    ? BDModernHandheldControlTarget.ControlAction.DPadUp
                    : BDModernHandheldControlTarget.ControlAction.DPadDown
            );
            PlayClick();
        }

        private void NavigateHorizontal(int direction)
        {
            if (direction == 0 || rows.Count == 0)
                return;

            PulsePersistentControl(
                direction < 0
                    ? BDModernHandheldControlTarget.ControlAction.DPadLeft
                    : BDModernHandheldControlTarget.ControlAction.DPadRight
            );

            if (displayedPage == EffectivePage.Settings)
                AdjustSelectedSetting(direction);

            PlayClick();
        }

        private void ActivateSelected()
        {
            if (rows.Count == 0 ||
                selectedIndex < 0 ||
                selectedIndex >= rows.Count)
            {
                return;
            }

            PulsePersistentControl(
                BDModernHandheldControlTarget.ControlAction.Confirm
            );
            ActivateRow(rows[selectedIndex].action);
            PlayClick();
        }

        private void ActivateRow(RowAction action)
        {
            if (flow == null &&
                action != RowAction.Back)
            {
                return;
            }

            switch (action)
            {
                case RowAction.Primary:
                    if (!TryBeginStartGameEntryV1011390())
                        flow.HandleModernPrimaryAction();
                    break;
                case RowAction.ContinueRun:
                    flow.HandleModernContinueRun();
                    break;
                case RowAction.StartNewRun:
                    // BD SCREEN START NEW RUN ROUTES THROUGH ENTRY V10.11.30.91
                    // Production menu rows use StartNewRun rather than Primary.
                    // Never let Select/Confirm or pointer activation bypass the
                    // visible screen-plane cinematic.
                    if (!TryBeginStartGameEntryV1011390())
                        flow.HandleModernStartNewRun();
                    break;
                case RowAction.ConfirmNewRun:
                    flow.HandleModernConfirmNewRun();
                    break;
                case RowAction.CancelNewRun:
                    flow.HandleModernCancelNewRun();
                    break;
                case RowAction.OpenProgression:
                    flow.HandleModernOpenProgression();
                    break;
                case RowAction.OpenSettings:
                    flow.HandleModernOpenSettings();
                    break;
                case RowAction.OpenCredits:
                    localPage = LocalPage.Credits;
                    displayedPageInitialized = false;
                    break;
                case RowAction.Quit:
                    RequestExitShortcut();
                    break;
                case RowAction.ReturnToMainMenu:
                    flow.HandleModernRequestMainMenu();
                    break;
                case RowAction.Back:
                    HandleBack();
                    break;
                case RowAction.ConfirmQuit:
                    flow.HandleModernQuit();
                    break;
                case RowAction.CancelQuit:
                    localPage = LocalPage.None;
                    displayedPageInitialized = false;
                    break;
                case RowAction.ConfirmAbandon:
                    flow.HandleModernPrimaryAction();
                    break;
                case RowAction.CancelAbandon:
                    flow.HandleModernCancelAbandon();
                    break;
                case RowAction.Fullscreen:
                    BDGameSettings.SetFullscreen(
                        !BDGameSettings.Fullscreen
                    );
                    RefreshSettingsRowValues();
                    break;
                case RowAction.VSync:
                    BDGameSettings.SetVSync(
                        !BDGameSettings.VSync
                    );
                    RefreshSettingsRowValues();
                    break;
                case RowAction.ResetDefaults:
                    BDGameSettings.ResetDefaults();
                    RefreshSettingsRowValues();
                    break;
                case RowAction.MasterVolume:
                case RowAction.MusicVolume:
                case RowAction.SfxVolume:
                case RowAction.MouseSensitivity:
                case RowAction.CameraShake:
                case RowAction.Quality:
                case RowAction.TargetFps:
                    AdjustSelectedSetting(1);
                    break;
            }
        }

        private void AdjustSelectedSetting(int direction)
        {
            if (rows.Count == 0 ||
                selectedIndex < 0 ||
                selectedIndex >= rows.Count)
            {
                return;
            }

            RowAction action = rows[selectedIndex].action;
            float signed = direction < 0 ? -1f : 1f;

            switch (action)
            {
                case RowAction.MasterVolume:
                    BDGameSettings.SetMasterVolume(
                        BDGameSettings.MasterVolume + signed * 0.05f
                    );
                    break;
                case RowAction.MusicVolume:
                    BDGameSettings.SetMusicVolume(
                        BDGameSettings.MusicVolume + signed * 0.05f
                    );
                    break;
                case RowAction.SfxVolume:
                    BDGameSettings.SetSfxVolume(
                        BDGameSettings.SfxVolume + signed * 0.05f
                    );
                    break;
                case RowAction.MouseSensitivity:
                    BDGameSettings.SetMouseSensitivity(
                        BDGameSettings.MouseSensitivityMultiplier +
                        signed * 0.05f
                    );
                    break;
                case RowAction.CameraShake:
                    BDGameSettings.SetCameraShake(
                        BDGameSettings.CameraShakeIntensity +
                        signed * 0.10f
                    );
                    break;
                case RowAction.Quality:
                    BDGameSettings.CycleQuality(direction);
                    break;
                case RowAction.VSync:
                case RowAction.TargetFps:
                    BDGameSettings.CycleTargetFps(direction);
                    break;
                default:
                    return;
            }

            RefreshSettingsRowValues();
        }

        private void HandleBack()
        {
            PulsePersistentControl(
                BDModernHandheldControlTarget.ControlAction.ContextBackSettings
            );

            if (localPage == LocalPage.Credits ||
                localPage == LocalPage.QuitConfirm)
            {
                localPage = LocalPage.None;
                displayedPageInitialized = false;
                PlayClick();
                return;
            }

            if (flow != null)
                flow.HandleModernBack();

            PlayClick();
        }

        private void ActivatePrimaryShortcut()
        {
            if (flow == null ||
                displayedPage != EffectivePage.MainMenu)
            {
                return;
            }

            PulsePersistentControl(
                BDModernHandheldControlTarget.ControlAction.Primary
            );

            localPage = LocalPage.None;

            if (!TryBeginStartGameEntryV1011390())
            {
                flow.HandleModernStartNewRun(); // BD X IS NEW GAME V10.11.30.49
            }

            PlayClick();
        }

        private void HandleContextBShortcut()
        {
            if (displayedPage == EffectivePage.MainMenu)
                OpenSettingsShortcut();
            else
                HandleBack();
        }

        private void OpenSettingsShortcut()
        {
            if (flow == null)
                return;

            PulsePersistentControl(
                BDModernHandheldControlTarget.ControlAction.ContextBackSettings
            );
            localPage = LocalPage.None;
            flow.HandleModernOpenSettings();
            PlayClick();
        }

        private void OpenProgressionShortcut()
        {
            if (flow == null || displayedPage != EffectivePage.MainMenu)
                return;

            PulsePersistentControl(
                BDModernHandheldControlTarget.ControlAction.Progression
            );
            localPage = LocalPage.None;
            flow.HandleModernOpenProgression();
            PlayClick();
        }

        private void OpenCreditsShortcut()
        {
            if (flow == null || displayedPage != EffectivePage.MainMenu)
                return;

            PulsePersistentControl(
                BDModernHandheldControlTarget.ControlAction.Credits
            );
            localPage = LocalPage.Credits;
            displayedPageInitialized = false;
            PlayClick();
        }

        private void RequestExitShortcut()
        {
            if (flow == null)
                return;

            PulsePersistentControl(
                BDModernHandheldControlTarget.ControlAction.Exit
            );

            if (flow.IsRunActive || flow.IsPausedFromGameplay)
            {
                localPage = LocalPage.None;
                flow.HandleModernRequestMainMenu();
            }
            else
            {
                if (flow.CurrentHandheldPage ==
                        BDMainMenuFlow.HandheldPage.Settings ||
                    flow.CurrentHandheldPage ==
                        BDMainMenuFlow.HandheldPage.Progression)
                {
                    flow.HandleModernBack();
                }

                localPage = LocalPage.QuitConfirm;
                displayedPageInitialized = false;
            }

            PlayClick();
        }

        private bool IsMenuInputReady()
        {
            if (IsStartGameEntryActiveV1011390())
                return false;

            if (IsIntroToMainMenuTransitionBlockingInput())
                return false;

            if (Time.unscaledTime < menuInputUnlockAt)
                return false;

            if (!menuInputNeedsRelease)
                return true;

            if (ReadAnyMenuControlHeld())
                return false;

            menuInputNeedsRelease = false;
            return true;
        }

        private void UpdateNavigationInput()
        {
            if (!IsMenuInputReady() ||
                displayedPage == EffectivePage.Loading)
            {
                return;
            }

            if (ReadUpPressed())
                NavigateVertical(-1);
            else if (ReadDownPressed())
                NavigateVertical(1);

            if (ReadLeftPressed())
                NavigateHorizontal(-1);
            else if (ReadRightPressed())
                NavigateHorizontal(1);

            if (ReadConfirmPressed())
                ActivateSelected();

            if (ReadPrimaryPressed())
                ActivatePrimaryShortcut();

            if (ReadProgressionPressed())
                OpenProgressionShortcut();

            if (ReadContextBPressed())
                HandleContextBShortcut();

            if (ReadCreditsPressed())
                OpenCreditsShortcut();

            if (ReadExitPressed())
                RequestExitShortcut();
        }

        private void UpdatePointerInteraction()
        {
            if (deviceCamera == null || !IsMenuInputReady())
                return;

            Vector2 pointerPosition;
            if (!TryReadPointerPosition(out pointerPosition))
            {
                ClearHover();
                return;
            }

            Ray ray = deviceCamera.ScreenPointToRay(pointerPosition);
            RaycastHit hit;
            BDModernHandheldControlTarget target = null;

            if (Physics.Raycast(
                    ray,
                    out hit,
                    60f,
                    1 << DeviceLayer,
                    QueryTriggerInteraction.Ignore))
            {
                target = hit.collider.GetComponent<
                    BDModernHandheldControlTarget>();
            }

            SetHoveredTarget(target);

            if (target == null || !ReadPointerPressed())
                return;

            ActivateControlTarget(target);
        }

        private void ActivateControlTarget(
            BDModernHandheldControlTarget target)
        {
            if (target == null)
                return;

            // BD SCREEN ITEM MIRRORS PHYSICAL CONFIRM V10.11.11
            // Clicking an on-screen option is the virtual equivalent of
            // pressing the handheld's physical select/confirm control.
            if (target.Action ==
                BDModernHandheldControlTarget.ControlAction.ScreenItem)
            {
                PulsePersistentControl(
                    BDModernHandheldControlTarget.ControlAction.Confirm
                );
            }

            if (HandleFirstLaunchTutorialControl(target))
                return;

            switch (target.Action)
            {
                case BDModernHandheldControlTarget.ControlAction.ScreenItem:
                    if (target.Index >= 0 &&
                        target.Index < rows.Count)
                    {
                        selectedIndex = target.Index;
                        UpdateSelectionVisuals();
                        target.Pulse();
                        if (!TryHandleSettingsPointerActivationV1011392(target))
                        {
                            ActivateRow(rows[selectedIndex].action);
                        }
                        PlayClick();
                    }
                    break;
                case BDModernHandheldControlTarget.ControlAction.DPadUp:
                    NavigateVertical(-1);
                    break;
                case BDModernHandheldControlTarget.ControlAction.DPadDown:
                    NavigateVertical(1);
                    break;
                case BDModernHandheldControlTarget.ControlAction.DPadLeft:
                    NavigateHorizontal(-1);
                    break;
                case BDModernHandheldControlTarget.ControlAction.DPadRight:
                    NavigateHorizontal(1);
                    break;
                case BDModernHandheldControlTarget.ControlAction.Confirm:
                    ActivateSelected();
                    break;
                case BDModernHandheldControlTarget.ControlAction.Exit:
                    RequestExitShortcut();
                    break;
                case BDModernHandheldControlTarget.ControlAction.Primary:
                    ActivatePrimaryShortcut();
                    break;
                case BDModernHandheldControlTarget.ControlAction.Progression:
                    OpenProgressionShortcut();
                    break;
                case BDModernHandheldControlTarget.ControlAction.ContextBackSettings:
                    HandleContextBShortcut();
                    break;
                case BDModernHandheldControlTarget.ControlAction.Credits:
                    OpenCreditsShortcut();
                    break;
            }
        }

        private void SetHoveredTarget(
            BDModernHandheldControlTarget target)
        {
            if (hoveredTarget == target)
                return;

            if (hoveredTarget != null)
                hoveredTarget.SetHovered(false);

            hoveredTarget = target;

            if (HandleFirstLaunchTutorialHover(target))
            {
                if (hoveredTarget != null)
                    hoveredTarget.SetHovered(true);
                return;
            }

            if (hoveredTarget != null)
            {
                hoveredTarget.SetHovered(true);

                if (hoveredTarget.Action ==
                        BDModernHandheldControlTarget.ControlAction.ScreenItem &&
                    hoveredTarget.Index >= 0 &&
                    hoveredTarget.Index < rows.Count)
                {
                    selectedIndex = hoveredTarget.Index;
                    UpdateSelectionVisuals();
                }
            }
        }

        private void ClearHover()
        {
            SetHoveredTarget(null);
        }

        private void PulsePersistentControl(
            BDModernHandheldControlTarget.ControlAction action)
        {
            for (int index = 0;
                 index < persistentControls.Count;
                 index++)
            {
                BDModernHandheldControlTarget target =
                    persistentControls[index];
                if (target != null && target.Action == action)
                {
                    target.Pulse();
                    return;
                }
            }
        }

        private void UpdateEntryAnimation()
        {
            if (deviceVisualRoot == null)
                return;

            if (IsStartGameEntryActiveV1011390())
                return;

            // The idle product shot owns the authoritative tabletop pose.
            // The Start cinematic temporarily overrides it after this method.
            deviceVisualRoot.localPosition = DeviceRestPosition;
            deviceVisualRoot.localRotation = DeviceRestRotation;
            deviceVisualRoot.localScale = DeviceRestScale;
        }

        private void UpdateScreenResolution()
        {
            if (lastScreenWidth == Screen.width &&
                lastScreenHeight == Screen.height)
            {
                return;
            }

            lastScreenWidth = Screen.width;
            lastScreenHeight = Screen.height;

            if (deviceCamera != null &&
                !introToMainMenuTransitionActive)
            {
                deviceCamera.fieldOfView =
                    ResolveRegularMainMenuFieldOfView();
            }

        }

        private void UpdateBackdropForPage(EffectivePage page)
        {
            if (backgroundMaterial == null)
                return;

            float alpha = page == EffectivePage.Pause ||
                          page == EffectivePage.Settings ||
                          page == EffectivePage.Progression ||
                          page == EffectivePage.QuitConfirm ||
                          page == EffectivePage.NewRunConfirm ||
                          page == EffectivePage.AbandonConfirm
                ? 0.30f
                : 0.18f;

            backgroundMaterial.color =
                new Color(0.004f, 0.007f, 0.018f, alpha);
        }

        private void BeginScreenTransition()
        {
            if (screenTransitionRoot == null)
                return;

            screenTransitionStartedAt = Time.unscaledTime;
            screenTransitionActive = true;
            screenTransitionRoot.gameObject.SetActive(true);
            screenTransitionRoot.SetAsLastSibling();

            if (pageCanvasGroup != null)
                pageCanvasGroup.alpha = 0.20f;

            UpdateScreenTransition();
        }

        private void UpdateScreenTransition()
        {
            if (!screenTransitionActive ||
                screenTransitionRoot == null)
            {
                return;
            }

            float t = Mathf.Clamp01(
                (Time.unscaledTime - screenTransitionStartedAt) /
                ScreenTransitionDuration
            );
            float reveal = t * t * (3f - 2f * t);
            float shutterHeight =
                CanvasSize.y * 0.5f * (1f - reveal);

            if (transitionTopShutter != null)
            {
                RectTransform rect =
                    transitionTopShutter.rectTransform;
                rect.sizeDelta = new Vector2(
                    CanvasSize.x,
                    shutterHeight
                );
                rect.anchoredPosition = new Vector2(
                    0f,
                    CanvasSize.y * 0.5f -
                    shutterHeight * 0.5f
                );
            }

            if (transitionBottomShutter != null)
            {
                RectTransform rect =
                    transitionBottomShutter.rectTransform;
                rect.sizeDelta = new Vector2(
                    CanvasSize.x,
                    shutterHeight
                );
                rect.anchoredPosition = new Vector2(
                    0f,
                    -CanvasSize.y * 0.5f +
                    shutterHeight * 0.5f
                );
            }

            if (transitionLine != null)
            {
                Color color = transitionLine.color;
                color.a = 1f - reveal;
                transitionLine.color = color;
                transitionLine.rectTransform.sizeDelta = new Vector2(
                    CanvasSize.x,
                    Mathf.Lerp(12f, 2f, reveal)
                );
            }

            if (transitionFlash != null)
            {
                Color color = transitionFlash.color;
                color.a = (1f - reveal) * 0.22f;
                transitionFlash.color = color;
            }

            if (pageCanvasGroup != null)
                pageCanvasGroup.alpha = Mathf.Lerp(0.20f, 1f, reveal);

            if (t < 1f)
                return;

            screenTransitionActive = false;
            screenTransitionRoot.gameObject.SetActive(false);

            if (pageCanvasGroup != null)
                pageCanvasGroup.alpha = 1f;
        }

        private void UpdateLoadingPresentation()
        {
            if (displayedPage != EffectivePage.Loading ||
                flow == null ||
                loadingFill == null ||
                loadingPercentText == null)
            {
                return;
            }

            float progress = flow.ReloadProgress;
            RectTransform fillRect = loadingFill.rectTransform;
            fillRect.sizeDelta = new Vector2(
                Mathf.Max(8f, 600f * progress),
                24f
            );
            loadingPercentText.text =
                Mathf.RoundToInt(progress * 100f) + "%";
        }

        private Image CreatePanel(
            Transform parent,
            string name,
            float x,
            float y,
            float width,
            float height,
            Color color)
        {
            GameObject panelObject = new GameObject(
                name,
                typeof(RectTransform)
            );
            panelObject.transform.SetParent(parent, false);
            SetLayerRecursively(panelObject, ScreenLayer);

            RectTransform rect =
                panelObject.GetComponent<RectTransform>();
            rect.anchorMin = new Vector2(0.5f, 0.5f);
            rect.anchorMax = new Vector2(0.5f, 0.5f);
            rect.pivot = new Vector2(0.5f, 0.5f);
            rect.anchoredPosition = new Vector2(x, y);
            rect.sizeDelta = new Vector2(width, height);

            Image image = panelObject.AddComponent<Image>();
            image.sprite = roundedSprite;
            image.type = Image.Type.Sliced;
            image.color = color;
            image.raycastTarget = false;
            return image;
        }

        private Text CreateText(
            Transform parent,
            string name,
            string value,
            float x,
            float y,
            float width,
            float height,
            int fontSize,
            TextAnchor alignment,
            Color color,
            FontStyle fontStyle)
        {
            GameObject textObject = new GameObject(
                name,
                typeof(RectTransform)
            );
            textObject.transform.SetParent(parent, false);
            SetLayerRecursively(textObject, ScreenLayer);

            RectTransform rect =
                textObject.GetComponent<RectTransform>();
            rect.anchorMin = new Vector2(0.5f, 0.5f);
            rect.anchorMax = new Vector2(0.5f, 0.5f);
            rect.pivot = new Vector2(0.5f, 0.5f);
            rect.anchoredPosition = new Vector2(x, y);
            rect.sizeDelta = new Vector2(width, height);

            Text text = textObject.AddComponent<Text>();
            text.font = uiFont;
            text.text = value;
            text.fontSize = fontSize;
            text.fontStyle = fontStyle;
            text.alignment = alignment;
            text.color = color;
            text.horizontalOverflow = HorizontalWrapMode.Wrap;
            text.verticalOverflow = VerticalWrapMode.Truncate;
            text.resizeTextForBestFit = true;
            text.resizeTextMinSize = Mathf.Max(9, fontSize - 8);
            text.resizeTextMaxSize = fontSize;
            text.supportRichText = false;
            text.raycastTarget = false;
            return text;
        }

        private RawImage CreateRawImage(
            Transform parent,
            string name,
            float x,
            float y,
            float width,
            float height,
            Texture texture,
            Color color)
        {
            GameObject imageObject = new GameObject(
                name,
                typeof(RectTransform)
            );
            imageObject.transform.SetParent(parent, false);
            SetLayerRecursively(imageObject, ScreenLayer);

            RectTransform rect =
                imageObject.GetComponent<RectTransform>();
            rect.anchorMin = new Vector2(0.5f, 0.5f);
            rect.anchorMax = new Vector2(0.5f, 0.5f);
            rect.pivot = new Vector2(0.5f, 0.5f);
            rect.anchoredPosition = new Vector2(x, y);
            rect.sizeDelta = new Vector2(width, height);

            RawImage image = imageObject.AddComponent<RawImage>();
            image.texture = texture;
            image.color = color;
            image.raycastTarget = false;
            image.uvRect = new Rect(0f, 0f, 1f, 1f);
            return image;
        }

        private Outline AddOutline(
            GameObject target,
            Color color,
            float distance)
        {
            Outline outline = target.AddComponent<Outline>();
            outline.effectColor = color;
            outline.effectDistance = new Vector2(distance, -distance);
            outline.useGraphicAlpha = true;
            return outline;
        }

        private GameObject CreateMeshObject(
            string name,
            Transform parent,
            Mesh mesh,
            Material material,
            Vector3 localPosition,
            Quaternion localRotation)
        {
            GameObject gameObject = new GameObject(name);
            gameObject.transform.SetParent(parent, false);
            gameObject.transform.localPosition = localPosition;
            gameObject.transform.localRotation = localRotation;
            SetLayerRecursively(gameObject, DeviceLayer);

            MeshFilter filter = gameObject.AddComponent<MeshFilter>();
            filter.sharedMesh = mesh;
            MeshRenderer renderer =
                gameObject.AddComponent<MeshRenderer>();
            renderer.sharedMaterial = material;
            return gameObject;
        }

        private Mesh CreateRoundedPrismMesh(
            float width,
            float height,
            float depth,
            float radius,
            int segmentsPerCorner)
        {
            List<Vector2> loop = BuildRoundedRectLoop(
                width,
                height,
                radius,
                segmentsPerCorner,
                0f
            );
            int count = loop.Count;
            List<Vector3> vertices = new List<Vector3>();
            List<Vector2> uvs = new List<Vector2>();
            List<int> triangles = new List<int>();

            float frontZ = -depth * 0.5f;
            float backZ = depth * 0.5f;

            int frontCenter = vertices.Count;
            vertices.Add(new Vector3(0f, 0f, frontZ));
            uvs.Add(new Vector2(0.5f, 0.5f));
            int frontStart = vertices.Count;
            for (int index = 0; index < count; index++)
            {
                Vector2 p = loop[index];
                vertices.Add(new Vector3(p.x, p.y, frontZ));
                uvs.Add(new Vector2(
                    p.x / width + 0.5f,
                    p.y / height + 0.5f
                ));
            }

            int backCenter = vertices.Count;
            vertices.Add(new Vector3(0f, 0f, backZ));
            uvs.Add(new Vector2(0.5f, 0.5f));
            int backStart = vertices.Count;
            for (int index = 0; index < count; index++)
            {
                Vector2 p = loop[index];
                vertices.Add(new Vector3(p.x, p.y, backZ));
                uvs.Add(new Vector2(
                    p.x / width + 0.5f,
                    p.y / height + 0.5f
                ));
            }

            for (int index = 0; index < count; index++)
            {
                int next = (index + 1) % count;
                triangles.Add(frontCenter);
                triangles.Add(frontStart + next);
                triangles.Add(frontStart + index);

                triangles.Add(backCenter);
                triangles.Add(backStart + index);
                triangles.Add(backStart + next);

                AddQuad(
                    triangles,
                    frontStart + index,
                    frontStart + next,
                    backStart + next,
                    backStart + index
                );
            }

            return FinalizeMesh(
                "Rounded Prism",
                vertices,
                uvs,
                triangles
            );
        }

        private Mesh CreateRoundedRingPrismMesh(
            float outerWidth,
            float outerHeight,
            float innerWidth,
            float innerHeight,
            float innerCenterY,
            float depth,
            float outerRadius,
            float innerRadius,
            int segmentsPerCorner)
        {
            List<Vector2> outer = BuildRoundedRectLoop(
                outerWidth,
                outerHeight,
                outerRadius,
                segmentsPerCorner,
                0f
            );
            List<Vector2> inner = BuildRoundedRectLoop(
                innerWidth,
                innerHeight,
                innerRadius,
                segmentsPerCorner,
                innerCenterY
            );

            int count = Mathf.Min(outer.Count, inner.Count);
            List<Vector3> vertices = new List<Vector3>();
            List<Vector2> uvs = new List<Vector2>();
            List<int> triangles = new List<int>();
            float frontZ = -depth * 0.5f;
            float backZ = depth * 0.5f;

            int outerFront = AddLoop(
                vertices,
                uvs,
                outer,
                frontZ,
                outerWidth,
                outerHeight
            );
            int innerFront = AddLoop(
                vertices,
                uvs,
                inner,
                frontZ,
                outerWidth,
                outerHeight
            );
            int outerBack = AddLoop(
                vertices,
                uvs,
                outer,
                backZ,
                outerWidth,
                outerHeight
            );
            int innerBack = AddLoop(
                vertices,
                uvs,
                inner,
                backZ,
                outerWidth,
                outerHeight
            );

            for (int index = 0; index < count; index++)
            {
                int next = (index + 1) % count;

                AddQuad(
                    triangles,
                    outerFront + index,
                    outerFront + next,
                    innerFront + next,
                    innerFront + index
                );
                AddQuad(
                    triangles,
                    outerBack + next,
                    outerBack + index,
                    innerBack + index,
                    innerBack + next
                );
                AddQuad(
                    triangles,
                    outerFront + index,
                    outerBack + index,
                    outerBack + next,
                    outerFront + next
                );
                AddQuad(
                    triangles,
                    innerFront + next,
                    innerBack + next,
                    innerBack + index,
                    innerFront + index
                );
            }

            return FinalizeMesh(
                "Rounded Ring Prism",
                vertices,
                uvs,
                triangles
            );
        }

        private int AddLoop(
            List<Vector3> vertices,
            List<Vector2> uvs,
            List<Vector2> points,
            float z,
            float uvWidth,
            float uvHeight)
        {
            int start = vertices.Count;
            for (int index = 0; index < points.Count; index++)
            {
                Vector2 p = points[index];
                vertices.Add(new Vector3(p.x, p.y, z));
                uvs.Add(new Vector2(
                    p.x / uvWidth + 0.5f,
                    p.y / uvHeight + 0.5f
                ));
            }
            return start;
        }

        private List<Vector2> BuildRoundedRectLoop(
            float width,
            float height,
            float radius,
            int segmentsPerCorner,
            float centerY)
        {
            float halfWidth = width * 0.5f;
            float halfHeight = height * 0.5f;
            radius = Mathf.Clamp(
                radius,
                0.001f,
                Mathf.Min(halfWidth, halfHeight)
            );
            segmentsPerCorner = Mathf.Max(2, segmentsPerCorner);

            List<Vector2> points =
                new List<Vector2>(segmentsPerCorner * 4);

            AddCorner(
                points,
                new Vector2(
                    halfWidth - radius,
                    centerY + halfHeight - radius
                ),
                radius,
                90f,
                0f,
                segmentsPerCorner
            );
            AddCorner(
                points,
                new Vector2(
                    halfWidth - radius,
                    centerY - halfHeight + radius
                ),
                radius,
                0f,
                -90f,
                segmentsPerCorner
            );
            AddCorner(
                points,
                new Vector2(
                    -halfWidth + radius,
                    centerY - halfHeight + radius
                ),
                radius,
                -90f,
                -180f,
                segmentsPerCorner
            );
            AddCorner(
                points,
                new Vector2(
                    -halfWidth + radius,
                    centerY + halfHeight - radius
                ),
                radius,
                180f,
                90f,
                segmentsPerCorner
            );

            return points;
        }

        private void AddCorner(
            List<Vector2> points,
            Vector2 center,
            float radius,
            float startDegrees,
            float endDegrees,
            int segments)
        {
            for (int index = 0; index < segments; index++)
            {
                float t = index / (float)(segments - 1);
                float angle = Mathf.Lerp(
                    startDegrees,
                    endDegrees,
                    t
                ) * Mathf.Deg2Rad;
                points.Add(
                    center + new Vector2(
                        Mathf.Cos(angle),
                        Mathf.Sin(angle)
                    ) * radius
                );
            }
        }

        private Mesh CreateSpeakerGrilleMesh(
            int columns,
            int rowsCount,
            float spacing,
            float radius,
            float depth)
        {
            List<Vector3> vertices = new List<Vector3>();
            List<Vector2> uvs = new List<Vector2>();
            List<int> triangles = new List<int>();
            const int segments = 10;

            float startX = -(columns - 1) * spacing * 0.5f;
            float startY = -(rowsCount - 1) * spacing * 0.5f;

            for (int row = 0; row < rowsCount; row++)
            {
                for (int column = 0; column < columns; column++)
                {
                    Vector2 center = new Vector2(
                        startX + column * spacing,
                        startY + row * spacing
                    );
                    int frontCenter = vertices.Count;
                    vertices.Add(new Vector3(
                        center.x,
                        center.y,
                        -depth
                    ));
                    uvs.Add(Vector2.zero);
                    int ringStart = vertices.Count;
                    for (int segment = 0;
                         segment < segments;
                         segment++)
                    {
                        float angle =
                            segment / (float)segments *
                            Mathf.PI * 2f;
                        vertices.Add(new Vector3(
                            center.x + Mathf.Cos(angle) * radius,
                            center.y + Mathf.Sin(angle) * radius,
                            -depth
                        ));
                        uvs.Add(Vector2.zero);
                    }
                    for (int segment = 0;
                         segment < segments;
                         segment++)
                    {
                        int next = (segment + 1) % segments;
                        triangles.Add(frontCenter);
                        triangles.Add(ringStart + next);
                        triangles.Add(ringStart + segment);
                    }
                }
            }

            return FinalizeMesh(
                "Speaker Grille",
                vertices,
                uvs,
                triangles
            );
        }

        private Mesh FinalizeMesh(
            string name,
            List<Vector3> vertices,
            List<Vector2> uvs,
            List<int> triangles)
        {
            Mesh mesh = new Mesh();
            mesh.name = name;
            mesh.SetVertices(vertices);
            mesh.SetUVs(0, uvs);
            mesh.SetTriangles(triangles, 0, true);
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
            generatedMeshes.Add(mesh);
            return mesh;
        }

        private static void AddQuad(
            List<int> triangles,
            int a,
            int b,
            int c,
            int d)
        {
            triangles.Add(a);
            triangles.Add(b);
            triangles.Add(c);
            triangles.Add(a);
            triangles.Add(c);
            triangles.Add(d);
        }

        private Texture2D CreateRoundedTexture(
            int size,
            float radius)
        {
            Texture2D texture = new Texture2D(
                size,
                size,
                TextureFormat.RGBA32,
                mipChain: false,
                linear: false
            );
            texture.name = "BD Rounded UI Texture";
            texture.wrapMode = TextureWrapMode.Clamp;
            texture.filterMode = FilterMode.Bilinear;

            Color[] pixels = new Color[size * size];
            float half = size * 0.5f;
            Vector2 center = new Vector2(half, half);

            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    Vector2 p = new Vector2(x + 0.5f, y + 0.5f);
                    Vector2 q = new Vector2(
                        Mathf.Abs(p.x - center.x),
                        Mathf.Abs(p.y - center.y)
                    ) - new Vector2(half - radius, half - radius);
                    float outside = new Vector2(
                        Mathf.Max(q.x, 0f),
                        Mathf.Max(q.y, 0f)
                    ).magnitude;
                    float inside = Mathf.Min(
                        Mathf.Max(q.x, q.y),
                        0f
                    );
                    float distance = outside + inside - radius;
                    float alpha = Mathf.Clamp01(0.5f - distance);
                    pixels[y * size + x] =
                        new Color(1f, 1f, 1f, alpha);
                }
            }

            texture.SetPixels(pixels);
            texture.Apply(updateMipmaps: false, makeNoLongerReadable: true);
            return texture;
        }

        private void ReleaseGeneratedResources()
        {
            if (screenCamera != null)
                screenCamera.targetTexture = null;

            if (screenDepthRenderTexture != null)
            {
                screenDepthRenderTexture.Release();
                Destroy(screenDepthRenderTexture);
                screenDepthRenderTexture = null;
            }

            if (screenRenderTexture != null)
            {
                screenRenderTexture.Release();
                Destroy(screenRenderTexture);
                screenRenderTexture = null;
            }

            if (clickClip != null)
                Destroy(clickClip);

            if (roundedSprite != null)
                Destroy(roundedSprite);

            if (roundedTexture != null)
                Destroy(roundedTexture);

            for (int index = 0;
                 index < generatedMaterials.Count;
                 index++)
            {
                if (generatedMaterials[index] != null)
                    Destroy(generatedMaterials[index]);
            }
            generatedMaterials.Clear();

            for (int index = 0;
                 index < generatedMeshes.Count;
                 index++)
            {
                if (generatedMeshes[index] != null)
                    Destroy(generatedMeshes[index]);
            }
            generatedMeshes.Clear();
        }

        private static void SetLayerRecursively(
            GameObject root,
            int layer)
        {
            if (root == null)
                return;

            root.layer = layer;
            Transform transform = root.transform;
            for (int index = 0;
                 index < transform.childCount;
                 index++)
            {
                SetLayerRecursively(
                    transform.GetChild(index).gameObject,
                    layer
                );
            }
        }

        private static bool TryReadPointerPosition(
            out Vector2 position)
        {
#if ENABLE_INPUT_SYSTEM
            if (Mouse.current != null)
            {
                position = Mouse.current.position.ReadValue();
                return true;
            }
            if (Touchscreen.current != null &&
                Touchscreen.current.primaryTouch.press.isPressed)
            {
                position = Touchscreen.current.primaryTouch.position.ReadValue();
                return true;
            }
#endif
#if ENABLE_LEGACY_INPUT_MANAGER
            position = Input.mousePosition;
            return true;
#else
            position = Vector2.zero;
            return false;
#endif
        }

        private static bool ReadPointerPressed()
        {
#if ENABLE_INPUT_SYSTEM
            if (Mouse.current != null &&
                Mouse.current.leftButton.wasPressedThisFrame)
            {
                return true;
            }
            if (Touchscreen.current != null &&
                Touchscreen.current.primaryTouch.press.wasPressedThisFrame)
            {
                return true;
            }
#endif
#if ENABLE_LEGACY_INPUT_MANAGER
            return Input.GetMouseButtonDown(0);
#else
            return false;
#endif
        }

        private static bool ReadAnyMenuControlHeld()
        {
#if ENABLE_INPUT_SYSTEM
            if (Keyboard.current != null &&
                (Keyboard.current.escapeKey.isPressed ||
                 Keyboard.current.backspaceKey.isPressed ||
                 Keyboard.current.enterKey.isPressed ||
                 Keyboard.current.numpadEnterKey.isPressed ||
                 Keyboard.current.spaceKey.isPressed ||
                 Keyboard.current.xKey.isPressed ||
                 Keyboard.current.yKey.isPressed ||
                 Keyboard.current.bKey.isPressed ||
                 Keyboard.current.pKey.isPressed ||
                 Keyboard.current.upArrowKey.isPressed ||
                 Keyboard.current.downArrowKey.isPressed ||
                 Keyboard.current.leftArrowKey.isPressed ||
                 Keyboard.current.rightArrowKey.isPressed ||
                 Keyboard.current.wKey.isPressed ||
                 Keyboard.current.sKey.isPressed ||
                 Keyboard.current.aKey.isPressed ||
                 Keyboard.current.dKey.isPressed))
            {
                return true;
            }

            if (Gamepad.current != null &&
                (Gamepad.current.buttonSouth.isPressed ||
                 Gamepad.current.buttonEast.isPressed ||
                 Gamepad.current.buttonWest.isPressed ||
                 Gamepad.current.buttonNorth.isPressed ||
                 Gamepad.current.startButton.isPressed ||
                 Gamepad.current.selectButton.isPressed ||
                 Gamepad.current.dpad.up.isPressed ||
                 Gamepad.current.dpad.down.isPressed ||
                 Gamepad.current.dpad.left.isPressed ||
                 Gamepad.current.dpad.right.isPressed))
            {
                return true;
            }

            if (Mouse.current != null &&
                Mouse.current.leftButton.isPressed)
            {
                return true;
            }
#endif
#if ENABLE_LEGACY_INPUT_MANAGER
            return Input.GetKey(KeyCode.Escape) ||
                   Input.GetKey(KeyCode.Backspace) ||
                   Input.GetKey(KeyCode.Return) ||
                   Input.GetKey(KeyCode.KeypadEnter) ||
                   Input.GetKey(KeyCode.Space) ||
                   Input.GetKey(KeyCode.X) ||
                   Input.GetKey(KeyCode.Y) ||
                   Input.GetKey(KeyCode.B) ||
                   Input.GetKey(KeyCode.P) ||
                   Input.GetKey(KeyCode.UpArrow) ||
                   Input.GetKey(KeyCode.DownArrow) ||
                   Input.GetKey(KeyCode.LeftArrow) ||
                   Input.GetKey(KeyCode.RightArrow) ||
                   Input.GetKey(KeyCode.W) ||
                   Input.GetKey(KeyCode.S) ||
                   Input.GetKey(KeyCode.A) ||
                   Input.GetKey(KeyCode.D) ||
                   Input.GetMouseButton(0);
#else
            return false;
#endif
        }

        private static bool ReadUpPressed()
        {
#if ENABLE_INPUT_SYSTEM
            if (Keyboard.current != null &&
                (Keyboard.current.upArrowKey.wasPressedThisFrame ||
                 Keyboard.current.wKey.wasPressedThisFrame))
            {
                return true;
            }
            if (Gamepad.current != null &&
                Gamepad.current.dpad.up.wasPressedThisFrame)
            {
                return true;
            }
#endif
#if ENABLE_LEGACY_INPUT_MANAGER
            return Input.GetKeyDown(KeyCode.UpArrow) ||
                   Input.GetKeyDown(KeyCode.W);
#else
            return false;
#endif
        }

        private static bool ReadDownPressed()
        {
#if ENABLE_INPUT_SYSTEM
            if (Keyboard.current != null &&
                (Keyboard.current.downArrowKey.wasPressedThisFrame ||
                 Keyboard.current.sKey.wasPressedThisFrame))
            {
                return true;
            }
            if (Gamepad.current != null &&
                Gamepad.current.dpad.down.wasPressedThisFrame)
            {
                return true;
            }
#endif
#if ENABLE_LEGACY_INPUT_MANAGER
            return Input.GetKeyDown(KeyCode.DownArrow) ||
                   Input.GetKeyDown(KeyCode.S);
#else
            return false;
#endif
        }

        private static bool ReadLeftPressed()
        {
#if ENABLE_INPUT_SYSTEM
            if (Keyboard.current != null &&
                (Keyboard.current.leftArrowKey.wasPressedThisFrame ||
                 Keyboard.current.aKey.wasPressedThisFrame))
            {
                return true;
            }
            if (Gamepad.current != null &&
                Gamepad.current.dpad.left.wasPressedThisFrame)
            {
                return true;
            }
#endif
#if ENABLE_LEGACY_INPUT_MANAGER
            return Input.GetKeyDown(KeyCode.LeftArrow) ||
                   Input.GetKeyDown(KeyCode.A);
#else
            return false;
#endif
        }

        private static bool ReadRightPressed()
        {
#if ENABLE_INPUT_SYSTEM
            if (Keyboard.current != null &&
                (Keyboard.current.rightArrowKey.wasPressedThisFrame ||
                 Keyboard.current.dKey.wasPressedThisFrame))
            {
                return true;
            }
            if (Gamepad.current != null &&
                Gamepad.current.dpad.right.wasPressedThisFrame)
            {
                return true;
            }
#endif
#if ENABLE_LEGACY_INPUT_MANAGER
            return Input.GetKeyDown(KeyCode.RightArrow) ||
                   Input.GetKeyDown(KeyCode.D);
#else
            return false;
#endif
        }

        private static bool ReadConfirmPressed()
        {
#if ENABLE_INPUT_SYSTEM
            if (Keyboard.current != null &&
                (Keyboard.current.enterKey.wasPressedThisFrame ||
                 Keyboard.current.numpadEnterKey.wasPressedThisFrame ||
                 Keyboard.current.spaceKey.wasPressedThisFrame))
            {
                return true;
            }
            if (Gamepad.current != null &&
                Gamepad.current.startButton.wasPressedThisFrame)
            {
                return true;
            }
#endif
#if ENABLE_LEGACY_INPUT_MANAGER
            return Input.GetKeyDown(KeyCode.Return) ||
                   Input.GetKeyDown(KeyCode.KeypadEnter) ||
                   Input.GetKeyDown(KeyCode.Space);
#else
            return false;
#endif
        }

        private static bool ReadPrimaryPressed()
        {
#if ENABLE_INPUT_SYSTEM
            if (Keyboard.current != null &&
                Keyboard.current.xKey.wasPressedThisFrame)
            {
                return true;
            }
            if (Gamepad.current != null &&
                Gamepad.current.buttonWest.wasPressedThisFrame)
            {
                return true;
            }
#endif
#if ENABLE_LEGACY_INPUT_MANAGER
            return Input.GetKeyDown(KeyCode.X);
#else
            return false;
#endif
        }

        private static bool ReadProgressionPressed()
        {
#if ENABLE_INPUT_SYSTEM
            if (Keyboard.current != null &&
                Keyboard.current.pKey.wasPressedThisFrame)
            {
                return true;
            }
            if (Gamepad.current != null &&
                Gamepad.current.buttonSouth.wasPressedThisFrame)
            {
                return true;
            }
#endif
#if ENABLE_LEGACY_INPUT_MANAGER
            return Input.GetKeyDown(KeyCode.P);
#else
            return false;
#endif
        }

        private static bool ReadContextBPressed()
        {
#if ENABLE_INPUT_SYSTEM
            if (Keyboard.current != null &&
                Keyboard.current.bKey.wasPressedThisFrame)
            {
                return true;
            }
            if (Gamepad.current != null &&
                Gamepad.current.buttonEast.wasPressedThisFrame)
            {
                return true;
            }
#endif
#if ENABLE_LEGACY_INPUT_MANAGER
            return Input.GetKeyDown(KeyCode.B);
#else
            return false;
#endif
        }

        private static bool ReadCreditsPressed()
        {
#if ENABLE_INPUT_SYSTEM
            if (Keyboard.current != null &&
                Keyboard.current.yKey.wasPressedThisFrame)
            {
                return true;
            }
            if (Gamepad.current != null &&
                Gamepad.current.buttonNorth.wasPressedThisFrame)
            {
                return true;
            }
#endif
#if ENABLE_LEGACY_INPUT_MANAGER
            return Input.GetKeyDown(KeyCode.Y);
#else
            return false;
#endif
        }

        private static bool ReadExitPressed()
        {
#if ENABLE_INPUT_SYSTEM
            if (Keyboard.current != null &&
                (Keyboard.current.escapeKey.wasPressedThisFrame ||
                 Keyboard.current.backspaceKey.wasPressedThisFrame))
            {
                return true;
            }
            if (Gamepad.current != null &&
                Gamepad.current.selectButton.wasPressedThisFrame)
            {
                return true;
            }
#endif
#if ENABLE_LEGACY_INPUT_MANAGER
            return Input.GetKeyDown(KeyCode.Escape) ||
                   Input.GetKeyDown(KeyCode.Backspace);
#else
            return false;
#endif
        }
    }
}
