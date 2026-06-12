using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BoredomAndDungeons
{
    public sealed partial class BDModernHandheld3DPresenter
    {
        private readonly Dictionary<Text, BDTutorialLetterPulseEffect>
            firstLaunchTutorialTextEffects =
                new Dictionary<Text, BDTutorialLetterPulseEffect>(16);
        private RectTransform firstLaunchTutorialBackdropLayerOwner;
        private Image firstLaunchTutorialPlayerVisualOwner;
        private Image firstLaunchTutorialCollectibleVisualOwner;
        private Sprite firstLaunchTutorialPolishedPlayerSprite;

        private Sprite firstLaunchTutorialSimplePlayerWalkASprite;
        private Sprite firstLaunchTutorialSimplePlayerWalkBSprite;
        private Sprite firstLaunchTutorialSimplePlayerActionASprite;
        private Sprite firstLaunchTutorialSimplePlayerActionBSprite;
        private Sprite firstLaunchTutorialPolishedCollectibleSprite;

        private void UpdateFirstLaunchTutorialV1011Polish()
        {
            ApplyFirstLaunchTutorialTypographyPolish();
            ApplyFirstLaunchTutorialJumpAttackPromptClarity();
            ApplyFirstLaunchTutorialCollectiblePromptClarity();
            ApplyFirstLaunchTutorialBackdropLayering();
            ApplyFirstLaunchTutorialPlayerVisualPolish();
            ApplyFirstLaunchTutorialCollectibleVisualPolish();
        }

        private void ApplyFirstLaunchTutorialTypographyPolish()
        {
            if (firstLaunchTutorialPrompt == null)
                return;

            if (firstLaunchTutorialInstructionRect != null)
            {
                Vector2 size = firstLaunchTutorialInstructionRect.sizeDelta;
                firstLaunchTutorialInstructionRect.sizeDelta = new Vector2(
                    Mathf.Clamp(Mathf.Max(872f, size.x), 872f, 916f),
                    Mathf.Clamp(Mathf.Max(320f, size.y), 320f, 348f)
                );
            }

            Color accent = ResolveFirstLaunchTutorialInstructionAccent(
                firstLaunchTutorialStep
            );
            Color secondary = Color.Lerp(
                accent,
                new Color(1f, 0.30f, 0.78f, 1f),
                0.34f
            );

            ConfigureFirstLaunchTutorialText(
                firstLaunchTutorialPrompt, 62, 48, 62, 2.8f, 0.90f,
                Color.white, accent, secondary, new Vector2(3f, -3f)
            );
            ConfigureFirstLaunchTutorialText(
                firstLaunchTutorialDetail, 34, 27, 34, 1.25f, 0.42f,
                new Color(0.90f, 0.96f, 1f, 1f),
                Color.Lerp(Color.white, accent, 0.56f),
                secondary, new Vector2(2f, -2f)
            );
            ConfigureFirstLaunchTutorialText(
                firstLaunchTutorialProgress, 27, 22, 27, 0.72f, 0.24f,
                accent, Color.white, secondary, new Vector2(2f, -2f)
            );
            ConfigureFirstLaunchTutorialText(
                firstLaunchTutorialFeedback, 31, 25, 31, 1.45f, 0.52f,
                new Color(1f, 0.92f, 0.46f, 1f),
                accent, secondary, new Vector2(2f, -2f)
            );
            ConfigureFirstLaunchTutorialText(
                firstLaunchTutorialKeyboardBindingTitle,
                22, 18, 22, 0.64f, 0.20f,
                Color.Lerp(Color.white, accent, 0.58f),
                accent, secondary, new Vector2(1f, -1f)
            );
            ConfigureFirstLaunchTutorialText(
                firstLaunchTutorialHandheldBindingTitle,
                22, 18, 22, 0.64f, 0.20f,
                Color.Lerp(Color.white, secondary, 0.52f),
                secondary, accent, new Vector2(1f, -1f)
            );
            ConfigureFirstLaunchTutorialText(
                firstLaunchTutorialKeyboardBinding,
                40, 31, 40, 1.10f, 0.36f,
                Color.white, accent, secondary, new Vector2(2f, -2f)
            );
            ConfigureFirstLaunchTutorialText(
                firstLaunchTutorialHandheldBinding,
                40, 31, 40, 1.10f, 0.36f,
                Color.white, secondary, accent, new Vector2(2f, -2f)
            );
            ConfigureFirstLaunchTutorialText(
                firstLaunchTutorialBindingDivider,
                25, 20, 25, 0.54f, 0.18f,
                accent, secondary, Color.white, new Vector2(1f, -1f)
            );
            ConfigureFirstLaunchTutorialText(
                firstLaunchTutorialHealthText,
                25, 20, 25, 0.46f, 0.14f,
                new Color(1f, 0.42f, 0.52f, 1f),
                Color.white, accent, new Vector2(1f, -1f)
            );
            ConfigureFirstLaunchTutorialText(
                firstLaunchTutorialAmmoText,
                25, 20, 25, 0.46f, 0.14f,
                new Color(0.42f, 0.88f, 1f, 1f),
                Color.white, secondary, new Vector2(1f, -1f)
            );
            ConfigureFirstLaunchTutorialText(
                firstLaunchTutorialBossHealthText,
                25, 20, 25, 0.52f, 0.16f,
                new Color(1f, 0.34f, 0.42f, 1f),
                Color.white, accent, new Vector2(1f, -1f)
            );

            if (firstLaunchTutorialInstructionAccent != null)
                firstLaunchTutorialInstructionAccent.color = accent;
        }

        private void ConfigureFirstLaunchTutorialText(
            Text text,
            int authoredSize,
            int minimumSize,
            int maximumSize,
            float verticalMotion,
            float horizontalMotion,
            Color primary,
            Color accent,
            Color secondary,
            Vector2 outlineDistance)
        {
            if (text == null)
                return;

            text.fontSize = authoredSize;
            text.fontStyle = FontStyle.Bold;
            text.resizeTextForBestFit = true;
            text.resizeTextMinSize = minimumSize;
            text.resizeTextMaxSize = maximumSize;
            text.horizontalOverflow = HorizontalWrapMode.Wrap;
            text.verticalOverflow = VerticalWrapMode.Truncate;
            text.lineSpacing = 0.94f;

            Outline outline = text.GetComponent<Outline>();
            if (outline == null)
                outline = text.gameObject.AddComponent<Outline>();
            outline.effectColor = new Color(0.01f, 0.02f, 0.05f, 0.94f);
            outline.effectDistance = outlineDistance;
            outline.useGraphicAlpha = true;

            BDTutorialLetterPulseEffect effect;
            if (!firstLaunchTutorialTextEffects.TryGetValue(text, out effect) ||
                effect == null)
            {
                effect = text.GetComponent<BDTutorialLetterPulseEffect>();
                if (effect == null)
                {
                    effect = text.gameObject.AddComponent<
                        BDTutorialLetterPulseEffect>();
                }
                firstLaunchTutorialTextEffects[text] = effect;
            }
            effect.SetMotion(verticalMotion, horizontalMotion);
            effect.SetPalette(primary, accent, secondary);
        }

        private void ApplyFirstLaunchTutorialJumpAttackPromptClarity()
        {
            if (firstLaunchTutorialStep != FirstLaunchTutorialStep.JumpAttack)
                return;

            if (firstLaunchTutorialPrompt != null)
                firstLaunchTutorialPrompt.text = "JUMP + ATTACK";
            if (firstLaunchTutorialDetail != null)
            {
                firstLaunchTutorialDetail.text =
                    "Jump, face the target, then attack while airborne.";
            }
            if (firstLaunchTutorialKeyboardBinding != null)
                firstLaunchTutorialKeyboardBinding.text =
                    "SPACE + J / LEFT CLICK";
            if (firstLaunchTutorialHandheldBinding != null)
                firstLaunchTutorialHandheldBinding.text = "B + X";
        }

        private void ApplyFirstLaunchTutorialCollectiblePromptClarity()
        {
            if (firstLaunchTutorialStep != FirstLaunchTutorialStep.Collectible)
                return;

            if (firstLaunchTutorialPrompt != null)
                firstLaunchTutorialPrompt.text = "COLLECT THE GREEN RELIC";
            if (firstLaunchTutorialDetail != null)
            {
                firstLaunchTutorialDetail.text =
                    "Walk into the glowing relic. Contact collects it automatically.";
            }
            if (firstLaunchTutorialKeyboardBindingTitle != null)
                firstLaunchTutorialKeyboardBindingTitle.text = "KEYBOARD";
            if (firstLaunchTutorialHandheldBindingTitle != null)
                firstLaunchTutorialHandheldBindingTitle.text = "HANDHELD";
            if (firstLaunchTutorialKeyboardBinding != null)
                firstLaunchTutorialKeyboardBinding.text = "MOVE INTO IT";
            if (firstLaunchTutorialHandheldBinding != null)
                firstLaunchTutorialHandheldBinding.text = "MOVE INTO IT";
        }

        private void ApplyFirstLaunchTutorialPlayerVisualPolish()
        {
            if (firstLaunchTutorialPlayer == null)
                return;

            if (firstLaunchTutorialPlayerVisualOwner !=
                    firstLaunchTutorialPlayer)
            {
                firstLaunchTutorialPlayerVisualOwner =
                    firstLaunchTutorialPlayer;
            }

            EnsureFirstLaunchTutorialSimplePlayerSprites();
            BindFirstLaunchTutorialSimplePlayerVisual();
        }

        private Sprite CreateFirstLaunchTutorialPolishedPlayerSprite()
        {
            return CreateFirstLaunchTutorialSimplePlayerSprite(
                "B&D Tutorial Player Simple Right Facing Sprite",
                0
            );
        }

        private void EnsureFirstLaunchTutorialSimplePlayerSprites()
        {
            if (firstLaunchTutorialPolishedPlayerSprite == null)
            {
                firstLaunchTutorialPolishedPlayerSprite =
                    CreateFirstLaunchTutorialPolishedPlayerSprite();
            }
            if (firstLaunchTutorialSimplePlayerWalkASprite == null)
            {
                firstLaunchTutorialSimplePlayerWalkASprite =
                    CreateFirstLaunchTutorialSimplePlayerSprite(
                        "B&D Tutorial Player Simple Right Facing Walk A",
                        1
                    );
            }
            if (firstLaunchTutorialSimplePlayerWalkBSprite == null)
            {
                firstLaunchTutorialSimplePlayerWalkBSprite =
                    CreateFirstLaunchTutorialSimplePlayerSprite(
                        "B&D Tutorial Player Simple Right Facing Walk B",
                        2
                    );
            }
            if (firstLaunchTutorialSimplePlayerActionASprite == null)
            {
                firstLaunchTutorialSimplePlayerActionASprite =
                    CreateFirstLaunchTutorialSimplePlayerSprite(
                        "B&D Tutorial Player Simple Right Facing Action A",
                        3
                    );
            }
            if (firstLaunchTutorialSimplePlayerActionBSprite == null)
            {
                firstLaunchTutorialSimplePlayerActionBSprite =
                    CreateFirstLaunchTutorialSimplePlayerSprite(
                        "B&D Tutorial Player Simple Right Facing Action B",
                        4
                    );
            }
        }

        private void BindFirstLaunchTutorialSimplePlayerVisual()
        {
            if (firstLaunchTutorialPlayer == null)
                return;

            if (firstLaunchTutorialPlayerPixelVisual == null ||
                firstLaunchTutorialPlayerPixelImage == null)
            {
                Transform child = firstLaunchTutorialPlayer.rectTransform.Find(
                    "Tutorial Player Pixel Visual"
                );
                if (child != null)
                {
                    firstLaunchTutorialPlayerPixelVisual =
                        child as RectTransform;
                    firstLaunchTutorialPlayerPixelImage =
                        child.GetComponent<Image>();
                }
            }

            if (firstLaunchTutorialPlayerPixelVisual == null ||
                firstLaunchTutorialPlayerPixelImage == null)
            {
                return;
            }

            // ApplyFirstLaunchTutorialPixelSprite intentionally disables the
            // source Image and gives visual ownership to this child Image.
            firstLaunchTutorialPlayer.enabled = false;
            firstLaunchTutorialPlayer.color = Color.white;
            firstLaunchTutorialPlayerPixelVisual.gameObject.SetActive(true);
            firstLaunchTutorialPlayerPixelVisual.SetAsLastSibling();
            firstLaunchTutorialPlayerPixelVisual.sizeDelta =
                new Vector2(64f, 92f);
            firstLaunchTutorialPlayerPixelImage.enabled = true;
            firstLaunchTutorialPlayerPixelImage.type = Image.Type.Simple;
            firstLaunchTutorialPlayerPixelImage.preserveAspect = true;
            firstLaunchTutorialPlayerPixelImage.color = Color.white;

            for (int index = 0;
                 index < firstLaunchTutorialWalkVisuals.Count;
                 index++)
            {
                TutorialPixelWalkVisual entry =
                    firstLaunchTutorialWalkVisuals[index];
                if (!entry.IsPlayer ||
                    entry.Source != firstLaunchTutorialPlayer)
                {
                    continue;
                }

                entry.Visual = firstLaunchTutorialPlayerPixelImage;
                entry.Idle = firstLaunchTutorialPolishedPlayerSprite;
                entry.StepA = firstLaunchTutorialSimplePlayerWalkASprite;
                entry.StepB = firstLaunchTutorialSimplePlayerWalkBSprite;
                entry.ActionA = firstLaunchTutorialSimplePlayerActionASprite;
                entry.ActionB = firstLaunchTutorialSimplePlayerActionBSprite;
            }

            int frame = Mathf.FloorToInt(Time.unscaledTime * 4f);
            bool acting = IsFirstLaunchTutorialPlayerVisualActing();
            bool walking = firstLaunchTutorialMovementActive &&
                !firstLaunchTutorialMounted;
            firstLaunchTutorialPlayerPixelImage.sprite = acting
                ? (frame % 2 == 0
                    ? firstLaunchTutorialSimplePlayerActionASprite
                    : firstLaunchTutorialSimplePlayerActionBSprite)
                : walking
                    ? (frame % 2 == 0
                        ? firstLaunchTutorialSimplePlayerWalkASprite
                        : firstLaunchTutorialSimplePlayerWalkBSprite)
                    : firstLaunchTutorialPolishedPlayerSprite;
        }

        private Sprite CreateFirstLaunchTutorialSimplePlayerSprite(
            string spriteName,
            int pose)
        {
            const int width = 18;
            const int height = 26;
            Texture2D texture = new Texture2D(
                width,
                height,
                TextureFormat.RGBA32,
                false
            );
            texture.name = spriteName + " Texture";
            texture.filterMode = FilterMode.Point;
            texture.wrapMode = TextureWrapMode.Clamp;
            Color[] pixels = new Color[width * height];
            for (int index = 0; index < pixels.Length; index++)
                pixels[index] = Color.clear;

            Color outline = new Color(0.025f, 0.035f, 0.07f, 1f);
            Color skin = new Color(0.97f, 0.72f, 0.50f, 1f);
            Color hair = new Color(1f, 0.78f, 0.10f, 1f);
            Color hairLight = new Color(1f, 0.92f, 0.34f, 1f);
            Color shirt = new Color(0.92f, 0.08f, 0.12f, 1f);
            Color shirtLight = new Color(1f, 0.24f, 0.20f, 1f);
            Color pants = new Color(0.06f, 0.28f, 0.88f, 1f);
            Color pantsLight = new Color(0.16f, 0.52f, 1f, 1f);
            Color shoe = new Color(0.06f, 0.05f, 0.08f, 1f);

            int leftLegX = pose == 1 ? 3 : pose == 2 ? 6 : 4;
            int rightLegX = pose == 1 ? 11 : pose == 2 ? 9 : 10;
            int leftFootX = pose == 1 ? 1 : pose == 2 ? 5 : 3;
            int rightFootX = pose == 1 ? 11 : pose == 2 ? 9 : 10;

            FillFirstLaunchTutorialPolishRect(
                pixels, width, leftFootX, 1, 5, 3, shoe
            );
            FillFirstLaunchTutorialPolishRect(
                pixels, width, rightFootX, 1, 5, 3, shoe
            );
            FillFirstLaunchTutorialPolishRect(
                pixels, width, leftLegX, 4, 3, 7, pants
            );
            FillFirstLaunchTutorialPolishRect(
                pixels, width, rightLegX, 4, 3, 7, pants
            );
            FillFirstLaunchTutorialPolishRect(
                pixels, width, leftLegX + 1, 7, 2, 3, pantsLight
            );

            FillFirstLaunchTutorialPolishRect(
                pixels, width, 4, 10, 10, 8, outline
            );
            FillFirstLaunchTutorialPolishRect(
                pixels, width, 5, 11, 8, 7, shirt
            );
            FillFirstLaunchTutorialPolishRect(
                pixels, width, 6, 15, 7, 2, shirtLight
            );

            if (pose == 1)
            {
                FillFirstLaunchTutorialPolishRect(
                    pixels, width, 2, 11, 2, 5, skin
                );
                FillFirstLaunchTutorialPolishRect(
                    pixels, width, 14, 14, 2, 5, skin
                );
            }
            else if (pose == 2)
            {
                FillFirstLaunchTutorialPolishRect(
                    pixels, width, 2, 14, 2, 5, skin
                );
                FillFirstLaunchTutorialPolishRect(
                    pixels, width, 14, 11, 2, 5, skin
                );
            }
            else if (pose == 3)
            {
                FillFirstLaunchTutorialPolishRect(
                    pixels, width, 1, 15, 4, 2, skin
                );
                FillFirstLaunchTutorialPolishRect(
                    pixels, width, 13, 13, 3, 2, skin
                );
            }
            else if (pose == 4)
            {
                FillFirstLaunchTutorialPolishRect(
                    pixels, width, 3, 11, 2, 5, skin
                );
                FillFirstLaunchTutorialPolishRect(
                    pixels, width, 13, 14, 5, 2, skin
                );
            }
            else
            {
                FillFirstLaunchTutorialPolishRect(
                    pixels, width, 2, 12, 2, 6, skin
                );
                FillFirstLaunchTutorialPolishRect(
                    pixels, width, 14, 12, 2, 6, skin
                );
            }

            FillFirstLaunchTutorialPolishRect(
                pixels, width, 6, 18, 8, 7, outline
            );
            FillFirstLaunchTutorialPolishRect(
                pixels, width, 7, 19, 7, 5, skin
            );
            FillFirstLaunchTutorialPolishRect(
                pixels, width, 6, 23, 8, 3, hair
            );
            FillFirstLaunchTutorialPolishRect(
                pixels, width, 7, 24, 7, 2, hairLight
            );
            FillFirstLaunchTutorialPolishRect(
                pixels, width, 6, 21, 2, 3, hair
            );
            // Positive X is authored facing right: eye and nose are on the right.
            FillFirstLaunchTutorialPolishRect(
                pixels, width, 12, 21, 1, 1, outline
            );
            FillFirstLaunchTutorialPolishRect(
                pixels, width, 14, 19, 2, 2, skin
            );

            texture.SetPixels(pixels);
            texture.Apply(false, false);
            Sprite sprite = Sprite.Create(
                texture,
                new Rect(0f, 0f, width, height),
                new Vector2(0.5f, 0.5f),
                1f,
                0u,
                SpriteMeshType.FullRect
            );
            sprite.name = spriteName;
            firstLaunchTutorialPixelTextures.Add(texture);
            firstLaunchTutorialPixelSprites.Add(sprite);
            return sprite;
        }

        private void ApplyFirstLaunchTutorialCollectibleVisualPolish()
        {
            if (firstLaunchTutorialCollectible == null)
                return;

            if (firstLaunchTutorialCollectibleVisualOwner !=
                    firstLaunchTutorialCollectible ||
                firstLaunchTutorialPolishedCollectibleSprite == null)
            {
                firstLaunchTutorialCollectibleVisualOwner =
                    firstLaunchTutorialCollectible;
                firstLaunchTutorialPolishedCollectibleSprite =
                    CreateFirstLaunchTutorialPolishedCollectibleSprite();
            }

            firstLaunchTutorialCollectible.sprite =
                firstLaunchTutorialPolishedCollectibleSprite;
            firstLaunchTutorialCollectible.color = Color.white;
            firstLaunchTutorialCollectible.preserveAspect = true;
            firstLaunchTutorialCollectible.rectTransform.sizeDelta =
                new Vector2(58f, 70f);

            if (!firstLaunchTutorialCollectible.gameObject.activeInHierarchy)
                return;

            float pulse = 1f +
                Mathf.Sin(Time.unscaledTime * 5.8f) * 0.07f;
            firstLaunchTutorialCollectible.rectTransform.localScale =
                new Vector3(pulse, pulse, 1f);
            firstLaunchTutorialCollectible.rectTransform.localRotation =
                Quaternion.Euler(
                    0f,
                    0f,
                    Mathf.Sin(Time.unscaledTime * 2.7f) * 4f
                );
        }

        private Sprite CreateFirstLaunchTutorialPolishedCollectibleSprite()
        {
            const int width = 20;
            const int height = 24;
            Texture2D texture = new Texture2D(
                width,
                height,
                TextureFormat.RGBA32,
                false
            );
            texture.name = "B&D Tutorial Collectible Relic Sprite";
            texture.filterMode = FilterMode.Point;
            texture.wrapMode = TextureWrapMode.Clamp;
            Color[] pixels = new Color[width * height];
            for (int index = 0; index < pixels.Length; index++)
                pixels[index] = Color.clear;

            Color outline = new Color(0.02f, 0.09f, 0.07f, 1f);
            Color dark = new Color(0.06f, 0.52f, 0.24f, 1f);
            Color green = new Color(0.18f, 0.96f, 0.42f, 1f);
            Color light = new Color(0.74f, 1f, 0.78f, 1f);
            for (int y = 1; y < height - 1; y++)
            {
                int distance = Mathf.Abs(y - 12);
                int half = Mathf.Max(1, 8 - distance / 2);
                int left = 10 - half;
                int right = 10 + half;
                for (int x = left; x <= right; x++)
                {
                    bool border = x == left || x == right ||
                        y == 1 || y == height - 2;
                    pixels[y * width + x] = border ? outline : green;
                }
            }
            FillFirstLaunchTutorialPolishRect(
                pixels, width, 7, 8, 3, 8, dark
            );
            FillFirstLaunchTutorialPolishRect(
                pixels, width, 11, 15, 3, 4, light
            );
            FillFirstLaunchTutorialPolishRect(
                pixels, width, 14, 19, 2, 2, Color.white
            );

            texture.SetPixels(pixels);
            texture.Apply(false, false);
            Sprite sprite = Sprite.Create(
                texture,
                new Rect(0f, 0f, width, height),
                new Vector2(0.5f, 0.5f),
                1f
            );
            sprite.name = "B&D Tutorial Collectible Relic Sprite";
            firstLaunchTutorialPixelTextures.Add(texture);
            firstLaunchTutorialPixelSprites.Add(sprite);
            return sprite;
        }

        private static void FillFirstLaunchTutorialPolishRect(
            Color[] pixels,
            int width,
            int x,
            int y,
            int rectWidth,
            int rectHeight,
            Color color)
        {
            int height = pixels.Length / width;
            for (int row = y; row < y + rectHeight; row++)
            {
                if (row < 0 || row >= height)
                    continue;
                for (int column = x;
                     column < x + rectWidth;
                     column++)
                {
                    if (column < 0 || column >= width)
                        continue;
                    pixels[row * width + column] = color;
                }
            }
        }

        private void ApplyFirstLaunchTutorialBackdropLayering()
        {
            if (firstLaunchTutorialCourseRoot == null ||
                firstLaunchTutorialBackdropLayerOwner ==
                    firstLaunchTutorialCourseRoot)
            {
                return;
            }

            Transform ground =
                firstLaunchTutorialCourseRoot.Find("Tutorial Ground");
            Transform path =
                firstLaunchTutorialCourseRoot.Find("Tutorial Path");
            if (ground != null)
                ground.SetSiblingIndex(0);
            if (path != null)
                path.SetSiblingIndex(1);

            int sibling = 2;
            for (int index = 0;
                 index < firstLaunchTutorialCourseDecorations.Count;
                 index++)
            {
                Image decoration =
                    firstLaunchTutorialCourseDecorations[index];
                if (decoration == null)
                    continue;
                decoration.rectTransform.SetSiblingIndex(sibling);
                sibling++;
            }

            firstLaunchTutorialBackdropLayerOwner =
                firstLaunchTutorialCourseRoot;
        }

        private void ReleaseFirstLaunchTutorialWallJumpSupportIfNeeded()
        {
            if (firstLaunchTutorialStep != FirstLaunchTutorialStep.WallJump ||
                !firstLaunchTutorialGrounded)
            {
                return;
            }

            float x = firstLaunchTutorialPlayerWorldPosition.x;
            bool onPlatform = Mathf.Approximately(
                firstLaunchTutorialGroundedY,
                TutorialWallJumpPlatformStandingY
            );
            bool onUpperGround = Mathf.Approximately(
                firstLaunchTutorialGroundedY,
                TutorialWallJumpUpperGroundStandingY
            );
            bool platformSupported =
                x >= TutorialWallJumpPlatformMinX &&
                x <= TutorialWallJumpPlatformMaxX;
            bool upperGroundSupported =
                x >= TutorialWallJumpUpperGroundMinX &&
                x <= TutorialWallJumpUpperGroundMaxX;

            if ((!onPlatform || platformSupported) &&
                (!onUpperGround || upperGroundSupported))
            {
                return;
            }

            firstLaunchTutorialGrounded = false;
            firstLaunchTutorialGroundedY = TutorialGroundY;
            firstLaunchTutorialVerticalVelocity = Mathf.Min(
                firstLaunchTutorialVerticalVelocity,
                -48f
            );
        }
    }
}
