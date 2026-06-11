#if UNITY_EDITOR
using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace BoredomAndDungeons.EditorTools.Validation
{
    internal static class BDModernHandheld3DQA
    {
        public static void Scan(BDOneClickQAResult result)
        {
            if (result == null)
                return;

            string root = Directory.GetParent(
                Application.dataPath
            ).FullName;

            Require(
                result,
                root,
                "Assets/_Project/Scripts/Runtime/UI/BDModernHandheld3DPresenter.cs",
                "HANDHELD_3D_PRESENTER_MISSING",
                "BD Modern Upright Handheld",
                "BD Modern Handheld Screen RT",
                "Screen Glass",
                "Button Select",
                "Button Exit",
                "HANDHELD_HERO_BOY_V1",
                "HANDHELD_HERO_GIRL_V1",
                "HANDHELD_ART_PROGRESSION_V1",
                "HANDHELD_ART_SETTINGS_V1",
                "HANDHELD_ART_CREDITS_V1",
                "HANDHELD_ART_QUIT_V1",
                "HANDHELD_ART_RESUME_V1",
                "ResolveContextArtwork",
                "Only Start Game / New Run is protagonist-aware",
                "DPadUp",
                "ControlAction.Confirm",
                "ControlAction.Exit",
                "ControlAction.Primary",
                "ControlAction.Progression",
                "ControlAction.ContextBackSettings",
                "ControlAction.Credits",
                "PROGRESSION",
                "HANDHELD_TABLE_DARK_WOOD_SHARP_V1",
                "HANDHELD_TABLE_DARK_WOOD_BLUR_V1",
                "DeviceLayer = 29",
                "DeviceRestRotation",
                "DeviceRealWorldScale = 0.16f",
                "DeviceRestScale",
                "deviceCamera.fieldOfView =",
                "ResolveRegularMainMenuFieldOfView();",
                "new Vector3(0f, -7.27f, -4.28f)",
                "Quaternion.Euler(90f, 0f, 0f)",
                "TableRestPosition",
                "FrontSurfaceZ",
                "Molded Outer Edge Bevel",
                "BuildCinematicProductEnvironment",
                "New Game Memory Card",
                "UpdateNewGameMemoryCardVisibility",
                "ADVENTURE SYSTEM",
                "WASD",
                "CreateDPadArm",
                "CreateDPadTarget",
                "BeginScreenTransition",
                "UpdateScreenTransition",
                "RenderMode.ScreenSpaceCamera",
                "ForceScreenRender",
                "IsMenuInputReady",
                "ReadAnyMenuControlHeld",
                "X NEW GAME",
                "A PROGRESSION",
                "B SETTINGS",
                "Y CREDITS",
                "BuildQuitConfirmPage",
                "RequestExitShortcut",
                "ActivatePrimaryShortcut",
                "HandleContextBShortcut",
                "OpenProgressionShortcut",
                "OpenCreditsShortcut",
                "ResolveControlLegend",
                "CreateRecessedHardwareLabel",
                "CreateCapQuad",
                "HANDHELD_BUTTON_X_V1",
                "HANDHELD_BUTTON_A_V1",
                "HANDHELD_BUTTON_B_V1",
                "HANDHELD_BUTTON_Y_V1",
                "HANDHELD_SHORTCUT_BUTTON_V1",
                "ReadPrimaryPressed",
                "ReadProgressionPressed",
                "ReadContextBPressed",
                "Gamepad.current.startButton",
                "Gamepad.current.selectButton",
                " Hit Target"
            );

            Require(
                result,
                root,
                "Assets/_Project/Scripts/Runtime/UI/" +
                "BDModernHandheld3DPresenter.CinematicEnvironment.cs",
                "HANDHELD_3D_CINEMATIC_ENVIRONMENT_MISSING",
                "BuildCinematicProductEnvironment",
                "Full 3D Tabletop",
                "Table Front Edge Lip",
                "Table Front Apron",
                "Table Front Left Leg",
                "Table Front Right Leg",
                "Table Back Left Leg",
                "Table Back Right Leg",
                "Cinematic Floor",
                "Cinematic Cyclorama",
                "Device Soft Contact Penumbra",
                "Device Core Contact Shadow",
                "Device Base Contact Shadow",
                "Table Leg Contact Shadow",
                "new Vector2(1.62f, 2.55f)",
                "DeviceRestPosition.z + 0.10f",
                "DeviceRestPosition.z + 0.04f",
                "new Vector3(0f, -7.15f, DeviceRestPosition.z + 0.40f)",
                "new Vector3(0f, -7.12f, DeviceRestPosition.z + 0.45f)",
                "new Vector3(0f, -7.05f, DeviceRestPosition.z + 0.60f)",
                "Cinematic Key Light",
                "Cinematic Camera Fill",
                "Cinematic Separation Light",
                "BuildCinematicRoomDressing",
                "Cinematic Woven Rug",
                "Cinematic Walnut Credenza",
                "Cinematic Practical Lamp",
                "Cinematic Warm Practical",
                "Cinematic Plant",
                "Cinematic Wall Slat",
                "CreateCinematicWallpaperMaterial",
                "BuildCinematicWallpaperPanels",
                "Cinematic Kitchen Wallpaper Back Wall",
                "BD Cinematic Kitchen Wallpaper"
            );

            Require(
                result,
                root,
                "Assets/_Project/Scripts/Runtime/UI/" +
                "BDModernHandheld3DPresenter.CinematicWallpaperBackWall.cs",
                "HANDHELD_CINEMATIC_BACKWALL_WALLPAPER_MISSING",
                "BuildExactCinematicBackWallWallpaper",
                "CinematicWallpaperResourcePath",
                "BDCinematicKitchenWallpaper",
                "Cinematic Exact Fruit Wallpaper Back Wall"
            );

            Forbid(
                result,
                root,
                "Assets/_Project/Scripts/Runtime/UI/" +
                "BDModernHandheld3DPresenter.CinematicEnvironment.cs",
                "HANDHELD_FLAT_CINEMATIC_ENVIRONMENT_REMAINS",
                "Professional Blurred Wood Table",
                "Table Cinematic Vignette",
                "new Vector2(10.9f, 3.85f)",
                "new Vector2(8.55f, 1.42f)"
            );
            Require(
                result,
                root,
                "Assets/_Project/Scripts/Runtime/UI/" +
                "BDModernHandheld3DPresenter.IntroToMainMenuTransition.cs",
                "HANDHELD_3D_PHYSICAL_STAGING_MISSING",
                "PrimeIntroToMainMenuFirstFrame",
                "introToMainMenuStartPosePrimed",
                "RegularMainMenuLookTarget",
                "new Vector3(0f, -1.68f, -4.18f)",
                "new Vector3(0f, -7.18f, -4.18f)",
                "new Vector3(-0.18f, 0.08f, -6.72f)",
                "Mathf.Lerp(38.8f, 31.8f, fit)",
                "deviceVisualRoot.localScale = DeviceRestScale"
            );

            Require(
                result,
                root,
                "Assets/_Project/Scripts/Runtime/UI/" +
                "BDModernHandheld3DPresenter.CinematicDepthOfField.cs",
                "HANDHELD_CINEMATIC_DOF_MISSING",
                "BDCinematicDepthOfField",
                "ConfigureCinematicDepthOfField",
                "DepthTextureMode.Depth",
                "ModernHandheld/Shaders/BDCinematicDepthOfField",
                "_FocusDistance",
                "OnRenderImage",
                "2.15f",
                "8.20f",
                "0.28f",
                "2.10f",
                "descriptor.width = source.width",
                "descriptor.height = source.height"
            );
            Require(
                result,
                root,
                "Assets/_Project/Resources/ModernHandheld/Shaders/BDCinematicDepthOfField.shader",
                "HANDHELD_CINEMATIC_DOF_SHADER_MISSING",
                "Hidden/BoredomAndDungeons/CinematicDepthOfField",
                "_CameraDepthTexture",
                "LinearEyeDepth",
                "fragComposite"
            );
            Forbid(
                result,
                root,
                "Assets/_Project/Scripts/Runtime/UI/" +
                "BDModernHandheld3DPresenter.IntroToMainMenuTransition.cs",
                "HANDHELD_3D_OLD_CAMERA_REMAINS",
                "new Vector3(0f, 0.44f, -25.2f)",
                "new Vector3(0f, 1.50f, -14.0f)",
                "Mathf.Lerp(17f, 15f, fit)",
                "new Vector3(0f, 1.50f, -3.19f)",
                "Mathf.Lerp(49f, 36.4f, fit)",
                "new Vector3(0f, -1.92f, -3.82f)",
                "new Vector3(0f, -7.20f, -3.82f)",
                "new Vector3(-0.42f, -0.35f, -7.10f)",
                "Mathf.Lerp(39f, 33.5f, fit)",
                "new Vector3(0f, -1.58f, -3.94f)",
                "new Vector3(0f, -7.22f, -3.94f)",
                "new Vector3(-0.26f, -0.02f, -6.96f)",
                "Mathf.Lerp(40.5f, 34.6f, fit)",
                "deviceVisualRoot.localScale = Vector3.Lerp"
            );

            Forbid(
                result,
                root,
                "Assets/_Project/Scripts/Runtime/UI/BDModernHandheld3DPresenter.cs",
                "HANDHELD_RETIRED_ENTRY_STATE_REMAINS",
                "entryProgress",
                "new Vector3(0f, -7.27f, 0f)"
            );

            Require(
                result,
                root,
                "Assets/_Project/Scripts/Runtime/UI/BDPlayableCharacterIdentity.cs",
                "HANDHELD_CHARACTER_IDENTITY_MISSING",
                "BDPlayableCharacterKind",
                "PreferredCharacterKey",
                "ResolveCurrent",
                "RefreshFromScene",
                "Girl",
                "Boy"
            );

            Require(
                result,
                root,
                "Assets/_Project/Scripts/Runtime/UI/BDMainMenuFlow.cs",
                "HANDHELD_FLOW_BRIDGE_MISSING",
                "ShouldPresentModernHandheld",
                "CurrentHandheldPage",
                "HandleModernPrimaryAction",
                "HandleModernOpenSettings",
                "HandleModernOpenProgression",
                "HandleModernRequestMainMenu"
            );

            Require(
                result,
                root,
                "Assets/_Project/Scripts/Runtime/UI/BDDreamyMainMenuBackdrop.cs",
                "HANDHELD_LEGACY_BACKDROP_SUPPRESSION_MISSING",
                "BDModernHandheld3DPresenter.SuppressLegacyMenu"
            );

            Require(
                result,
                root,
                "Assets/_Project/Shaders/BDModernHandheldSurface.shader",
                "HANDHELD_SURFACE_SHADER_MISSING",
                "ModernHandheldSurface",
                "_UseObjectGradient",
                "_GradientLeft",
                "_SpecularStrength",
                "Cull Off"
            );


            Require(
                result,
                root,
                "Assets/_Project/Shaders/BDModernHandheldTable.shader",
                "HANDHELD_TABLE_SHADER_MISSING",
                "ModernHandheldTable",
                "_BlurTex",
                "_FocusCenter",
                "_FocusFalloff",
                "smoothstep"
            );
            Require(
                result,
                root,
                "Assets/_Project/Shaders/BDModernHandheldDisplay.shader",
                "HANDHELD_DISPLAY_SHADER_MISSING",
                "ModernHandheldDisplay",
                "Cull Off",
                "Live Screen"
            );

            Require(
                result,
                root,
                "Assets/_Project/Shaders/BDModernHandheldGlass.shader",
                "HANDHELD_GLASS_SHADER_MISSING",
                "ModernHandheldGlass",
                "_FresnelPower",
                "_GlintColor",
                "upper-right side",
                "ZWrite Off"
            );

            Require(
                result,
                root,
                "Assets/_Project/Shaders/BDModernHandheldShadow.shader",
                "HANDHELD_SHADOW_SHADER_MISSING",
                "ModernHandheldShadow",
                "Transparent-40",
                "mask.a * _Color.a"
            );

            Require(
                result,
                root,
                "Assets/_Project/Shaders/BDModernHandheldButtonCap.shader",
                "HANDHELD_BUTTON_CAP_SHADER_MISSING",
                "ModernHandheldButtonCap",
                "Button Texture",
                "clip(tex.a - 0.025)",
                "Edge Glow"
            );

            Forbid(
                result,
                root,
                "Assets/_Project/Scripts/Runtime/UI/BDModernHandheld3DPresenter.cs",
                "HANDHELD_OBSOLETE_LABEL_REMAINS",
                "Meta Progression"
            );
            Forbid(
                result,
                root,
                "Assets/_Project/Scripts/Runtime/UI/BDModernHandheld3DPresenter.cs",
                "HANDHELD_FLAT_DECAL_OR_CHARACTER_DETAIL_REMAINS",
                "Approved Handheld Texture Decal",
                "BOY ROUTE",
                "GIRL ROUTE",
                "CHARACTER //"
            );


            Forbid(
                result,
                root,
                "Assets/_Project/Scripts/Runtime/UI/BDModernHandheld3DPresenter.cs",
                "HANDHELD_OBSOLETE_CONTROL_MAPPING_REMAINS",
                "X SELECT",
                "Y EXIT",
                "A CREDITS",
                "Button Settings",
                "Button Progression",
                "ControlAction.Back",
                "ControlAction.Settings"
            );

            ValidatePackageDependencies(result, root);
            ValidateCharacterPair(result);
            ValidateOptionArtwork(result);
            ValidateControlTextures(result);
            ValidateSurfaceAssets(result);
            ValidateSourceReferences(result, root);
            ValidateDocumentation(result, root);
        }

        private static void ValidatePackageDependencies(
            BDOneClickQAResult result,
            string root)
        {
            string manifestPath = Path.Combine(
                root,
                "Packages/manifest.json"
            );

            if (!File.Exists(manifestPath))
            {
                Add(
                    result,
                    "HANDHELD_UGUI_MANIFEST_MISSING",
                    "Packages/manifest.json is required for the 3D handheld screen UI."
                );
                return;
            }

            string manifest = File.ReadAllText(manifestPath);
            if (!manifest.Contains(
                    "\"com.unity.ugui\": \"2.0.0\"",
                    StringComparison.Ordinal))
            {
                Add(
                    result,
                    "HANDHELD_UGUI_PACKAGE_MISSING",
                    "The 3D handheld uses UnityEngine.UI Image/Text/RawImage/Outline and requires com.unity.ugui 2.0.0."
                );
            }
        }

        private static void ValidateCharacterPair(
            BDOneClickQAResult result)
        {
            const string boyPath =
                "Assets/_Project/Resources/ModernHandheld/UI/HANDHELD_HERO_BOY_V1.png";
            const string girlPath =
                "Assets/_Project/Resources/ModernHandheld/UI/HANDHELD_HERO_GIRL_V1.png";

            Texture2D boy = AssetDatabase.LoadAssetAtPath<Texture2D>(
                boyPath
            );
            Texture2D girl = AssetDatabase.LoadAssetAtPath<Texture2D>(
                girlPath
            );

            if (boy == null || girl == null)
            {
                Add(
                    result,
                    "HANDHELD_CHARACTER_ART_PAIR_MISSING",
                    "Both Boy and Girl hero images are required in the same package."
                );
                return;
            }

            if (boy.width != girl.width ||
                boy.height != girl.height)
            {
                Add(
                    result,
                    "HANDHELD_CHARACTER_ART_DIMENSIONS_MISMATCH",
                    "Boy/Girl hero images must have identical dimensions."
                );
            }

            TextureImporter boyImporter =
                AssetImporter.GetAtPath(boyPath) as TextureImporter;
            TextureImporter girlImporter =
                AssetImporter.GetAtPath(girlPath) as TextureImporter;

            if (boyImporter == null || girlImporter == null)
                return;

            bool importsMatch =
                boyImporter.textureType == girlImporter.textureType &&
                boyImporter.mipmapEnabled == girlImporter.mipmapEnabled &&
                boyImporter.sRGBTexture == girlImporter.sRGBTexture &&
                boyImporter.maxTextureSize == girlImporter.maxTextureSize &&
                boyImporter.textureCompression == girlImporter.textureCompression;

            if (!importsMatch)
            {
                Add(
                    result,
                    "HANDHELD_CHARACTER_ART_IMPORT_MISMATCH",
                    "Boy/Girl paired art must use matching import settings."
                );
            }
        }

        private static void ValidateOptionArtwork(
            BDOneClickQAResult result)
        {
            string[] paths =
            {
                "Assets/_Project/Resources/ModernHandheld/UI/HANDHELD_ART_PROGRESSION_V1.png",
                "Assets/_Project/Resources/ModernHandheld/UI/HANDHELD_ART_SETTINGS_V1.png",
                "Assets/_Project/Resources/ModernHandheld/UI/HANDHELD_ART_CREDITS_V1.png",
                "Assets/_Project/Resources/ModernHandheld/UI/HANDHELD_ART_QUIT_V1.png",
                "Assets/_Project/Resources/ModernHandheld/UI/HANDHELD_ART_RESUME_V1.png"
            };

            int width = -1;
            int height = -1;
            foreach (string path in paths)
            {
                Texture2D texture =
                    AssetDatabase.LoadAssetAtPath<Texture2D>(path);
                if (texture == null)
                {
                    Add(
                        result,
                        "HANDHELD_OPTION_ART_MISSING",
                        "Missing character-neutral option artwork: " + path
                    );
                    continue;
                }

                if (width < 0)
                {
                    width = texture.width;
                    height = texture.height;
                }
                else if (texture.width != width ||
                         texture.height != height)
                {
                    Add(
                        result,
                        "HANDHELD_OPTION_ART_DIMENSIONS_MISMATCH",
                        "All character-neutral option images must use identical dimensions."
                    );
                }

                TextureImporter importer =
                    AssetImporter.GetAtPath(path) as TextureImporter;
                if (importer != null && importer.mipmapEnabled)
                {
                    Add(
                        result,
                        "HANDHELD_OPTION_ART_MIPMAPS_ENABLED",
                        "Screen UI option artwork must not use mipmaps: " + path
                    );
                }
            }
        }

        private static void ValidateControlTextures(
            BDOneClickQAResult result)
        {
            string[] paths =
            {
                "Assets/_Project/Resources/ModernHandheld/Controls/HANDHELD_BUTTON_X_V1.png",
                "Assets/_Project/Resources/ModernHandheld/Controls/HANDHELD_BUTTON_Y_V1.png",
                "Assets/_Project/Resources/ModernHandheld/Controls/HANDHELD_BUTTON_A_V1.png",
                "Assets/_Project/Resources/ModernHandheld/Controls/HANDHELD_BUTTON_B_V1.png",
                "Assets/_Project/Resources/ModernHandheld/Controls/HANDHELD_DPAD_UP_V1.png",
                "Assets/_Project/Resources/ModernHandheld/Controls/HANDHELD_DPAD_DOWN_V1.png",
                "Assets/_Project/Resources/ModernHandheld/Controls/HANDHELD_DPAD_LEFT_V1.png",
                "Assets/_Project/Resources/ModernHandheld/Controls/HANDHELD_DPAD_RIGHT_V1.png",
                "Assets/_Project/Resources/ModernHandheld/Controls/HANDHELD_DPAD_CENTER_V1.png",
                "Assets/_Project/Resources/ModernHandheld/Controls/HANDHELD_SHORTCUT_BUTTON_V1.png"
            };

            foreach (string path in paths)
            {
                Texture2D texture =
                    AssetDatabase.LoadAssetAtPath<Texture2D>(path);
                if (texture == null)
                {
                    Add(
                        result,
                        "HANDHELD_CONTROL_TEXTURE_MISSING",
                        "Missing modeled-control texture: " + path
                    );
                    continue;
                }

                if (texture.width < 256 || texture.height < 128)
                {
                    Add(
                        result,
                        "HANDHELD_CONTROL_TEXTURE_TOO_SMALL",
                        "Control texture is below production minimum: " + path
                    );
                }

                TextureImporter importer =
                    AssetImporter.GetAtPath(path) as TextureImporter;
                if (importer == null || !importer.alphaIsTransparency)
                {
                    Add(
                        result,
                        "HANDHELD_CONTROL_TEXTURE_ALPHA_INVALID",
                        "Modeled-control caps require transparent crop edges: " + path
                    );
                }
            }
        }

        private static void ValidateSurfaceAssets(
            BDOneClickQAResult result)
        {
            const string shellPath =
                "Assets/_Project/Resources/ModernHandheld/Textures/HANDHELD_SHELL_GRADIENT_V1.png";
            const string glintPath =
                "Assets/_Project/Resources/ModernHandheld/Textures/HANDHELD_GLASS_GLINT_V2.png";
            const string sharpPath =
                "Assets/_Project/Resources/ModernHandheld/Textures/HANDHELD_TABLE_DARK_WOOD_SHARP_V1.png";
            const string blurPath =
                "Assets/_Project/Resources/ModernHandheld/Textures/HANDHELD_TABLE_DARK_WOOD_BLUR_V1.png";
            const string softShadowPath =
                "Assets/_Project/Resources/ModernHandheld/Textures/HANDHELD_DEVICE_SHADOW_V2.png";
            const string contactShadowPath =
                "Assets/_Project/Resources/ModernHandheld/Textures/HANDHELD_CONTACT_SHADOW_V2.png";

            Texture2D shell =
                AssetDatabase.LoadAssetAtPath<Texture2D>(shellPath);
            Texture2D glint =
                AssetDatabase.LoadAssetAtPath<Texture2D>(glintPath);
            Texture2D sharp =
                AssetDatabase.LoadAssetAtPath<Texture2D>(sharpPath);
            Texture2D blur =
                AssetDatabase.LoadAssetAtPath<Texture2D>(blurPath);
            Texture2D softShadow =
                AssetDatabase.LoadAssetAtPath<Texture2D>(softShadowPath);
            Texture2D contactShadow =
                AssetDatabase.LoadAssetAtPath<Texture2D>(contactShadowPath);

            if (shell == null || shell.width < 2048 || shell.height < 2048)
            {
                Add(
                    result,
                    "HANDHELD_SHELL_SURFACE_TEXTURE_INVALID",
                    "The molded shell micro-surface texture must remain at least 2048x2048."
                );
            }

            if (glint == null)
            {
                Add(
                    result,
                    "HANDHELD_GLASS_GLINT_TEXTURE_MISSING",
                    "The upper-right directional glass glint texture is required."
                );
            }

            if (softShadow == null || contactShadow == null)
            {
                Add(
                    result,
                    "HANDHELD_PRODUCT_SHADOW_MASKS_MISSING",
                    "Both the short left penumbra and contact-shadow masks are required."
                );
            }

            if (sharp == null || blur == null)
            {
                Add(
                    result,
                    "HANDHELD_TABLE_FOCUS_TEXTURES_MISSING",
                    "Both sharp and defocused versions of the supplied wood texture are required."
                );
                return;
            }

            if (sharp.width != blur.width ||
                sharp.height != blur.height)
            {
                Add(
                    result,
                    "HANDHELD_TABLE_FOCUS_DIMENSIONS_MISMATCH",
                    "Sharp and defocused table textures must have identical dimensions."
                );
            }
        }


        private static void ValidateSourceReferences(
            BDOneClickQAResult result,
            string root)
        {
            RequireExists(
                result,
                root,
                "ProjectGuide/References/Visual/ModernHandheld3D/HANDHELD_ORTHOGRAPHIC_TEXTURE_SHEET_V1.png",
                "HANDHELD_APPROVED_SOURCE_SHEET_MISSING"
            );
            RequireExists(
                result,
                root,
                "ProjectGuide/References/Visual/ModernHandheld3D/HANDHELD_TABLE_DARK_WOOD_SOURCE_V1.png",
                "HANDHELD_APPROVED_WOOD_SOURCE_MISSING"
            );
        }

        private static void ValidateDocumentation(
            BDOneClickQAResult result,
            string root)
        {
            Require(
                result,
                root,
                "ProjectGuide/Tasks/ACTIVE/MODERN_HANDHELD_MAIN_PAUSE_UI.md",
                "HANDHELD_ACTIVE_TASK_NOT_SYNCHRONIZED",
                "IMPLEMENTED / UNITY VERIFICATION REQUIRED",
                "active character identity",
                "Only Start Game / New Run",
                "character-neutral",
                "full-face decal",
                "short left shadow",
                "WASD",
                "upper-right glass glint",
                "Main Menu X starts New Game",
                "center SELECT",
                "center EXIT",
                "B returns on every non-main page",
                "Boy",
                "Girl",
                "Exact resume point"
            );

            Require(
                result,
                root,
                "ProjectGuide/Production/CHARACTER_ART_PARITY.md",
                "HANDHELD_CHARACTER_PARITY_DOC_MISSING",
                "active playable character",
                "New Game",
                "character-neutral",
                "text-only",
                "Mother state",
                "never random"
            );

            Require(
                result,
                root,
                "ProjectGuide/Status/CURRENT.md",
                "HANDHELD_CURRENT_STATUS_NOT_SYNCHRONIZED",
                "V5 CONTROL, LAYOUT AND PRODUCT-SHOT REPAIR",
                "Main Menu X=New Game",
                "center-left=SELECT",
                "center-right=EXIT",
                "AUTOMATED PASS",
                "UNITY VERIFICATION REQUIRED",
                "raises the device slightly higher"
            );
        }


        private static void RequireExists(
            BDOneClickQAResult result,
            string root,
            string relative,
            string code)
        {
            if (File.Exists(Path.Combine(root, relative)))
                return;

            Add(result, code, "Missing required file: " + relative);
        }

        private static string Read(
            string root,
            string relative)
        {
            string path = Path.Combine(root, relative);
            return File.Exists(path)
                ? File.ReadAllText(path)
                : string.Empty;
        }

        private static void Require(
            BDOneClickQAResult result,
            string root,
            string relative,
            string code,
            params string[] tokens)
        {
            string source = Read(root, relative);
            if (string.IsNullOrEmpty(source))
            {
                Add(result, code, "Missing required file: " + relative);
                return;
            }

            foreach (string token in tokens)
            {
                if (source.IndexOf(token, StringComparison.Ordinal) >= 0)
                    continue;

                Add(result, code, "Missing required contract: " + token);
            }
        }

        private static void Forbid(
            BDOneClickQAResult result,
            string root,
            string relative,
            string code,
            params string[] tokens)
        {
            string source = Read(root, relative);
            foreach (string token in tokens)
            {
                if (source.IndexOf(token, StringComparison.Ordinal) < 0)
                    continue;

                Add(result, code, "Obsolete runtime label remains: " + token);
            }
        }

        private static void Add(
            BDOneClickQAResult result,
            string code,
            string message)
        {
            result.findings.Add(
                new BDOneClickQAFinding(
                    BDOneClickQASeverity.Blocker,
                    code,
                    string.Empty,
                    string.Empty,
                    message
                )
            );
        }
    }
}
#endif
