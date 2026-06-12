using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BoredomAndDungeons
{
    public sealed partial class BDModernHandheld3DPresenter
    {
        private static readonly Vector2 TutorialInstructionRestPosition =
            new Vector2(0f, -266f);

        private RectTransform firstLaunchTutorialPlayerPixelVisual;
        private RectTransform firstLaunchTutorialHorsePixelVisual;
        private RectTransform firstLaunchTutorialEnemyPixelVisual;
        private RectTransform firstLaunchTutorialHazardPixelVisual;
        private RectTransform firstLaunchTutorialProjectilePixelVisual;
        private RectTransform firstLaunchTutorialCollectiblePixelVisual;
        private RectTransform firstLaunchTutorialGapPixelVisual;

        private Image firstLaunchTutorialPlayerPixelImage;
        private Image firstLaunchTutorialHorsePixelImage;
        private Image firstLaunchTutorialEnemyPixelImage;
        private Image firstLaunchTutorialHazardPixelImage;
        private Image firstLaunchTutorialProjectilePixelImage;
        private Image firstLaunchTutorialCollectiblePixelImage;
        private Image firstLaunchTutorialGapPixelImage;

        private sealed class TutorialPixelWalkVisual
        {
            public Image Source;
            public Image Visual;
            public Sprite Idle;
            public Sprite StepA;
            public Sprite StepB;
            public Sprite ActionA;
            public Sprite ActionB;
            public Vector2 LastSourcePosition;
            public bool IsPlayer;
            public bool IsHorse;
            public bool IsEnemy;
        }

        private readonly List<TutorialPixelWalkVisual>
            firstLaunchTutorialWalkVisuals =
                new List<TutorialPixelWalkVisual>(12);

        private void BuildFirstLaunchTutorialPixelBackdrop(
            RectTransform parent)
        {
            if (parent == null)
                return;

            // Remembered-console palette: deep indigo space, dark teal
            // vegetation, warm path and a few readable accent pixels.
            // Decorations stay at the edges so the lesson action remains clear.
            Color skyHigh = new Color(0.055f, 0.055f, 0.16f, 1f);
            Color skyLow = new Color(0.12f, 0.10f, 0.25f, 1f);
            Color horizon = new Color(0.28f, 0.18f, 0.36f, 1f);
            Color canopyDark = new Color(0.035f, 0.16f, 0.16f, 0.96f);
            Color canopyLight = new Color(0.06f, 0.26f, 0.22f, 0.96f);
            Color trunk = new Color(0.22f, 0.12f, 0.08f, 1f);
            Color stone = new Color(0.22f, 0.20f, 0.34f, 0.92f);
            Color grass = new Color(0.20f, 0.48f, 0.28f, 0.92f);
            Color ember = new Color(1f, 0.54f, 0.20f, 0.96f);
            Color moon = new Color(1f, 0.78f, 0.30f, 1f);

            CreateTutorialDecoration(
                parent,
                "Pixel Sky High",
                0f,
                126f,
                820f,
                138f,
                skyHigh
            );
            CreateTutorialDecoration(
                parent,
                "Pixel Sky Low",
                0f,
                30f,
                820f,
                58f,
                skyLow
            );

            CreateTutorialDecoration(
                parent,
                "Pixel Horizon",
                0f,
                -108f,
                820f,
                4f,
                horizon
            );

            CreateTutorialDecoration(
                parent,
                "Pixel Moon Core",
                252f,
                118f,
                40f,
                40f,
                moon
            );
            CreateTutorialDecoration(
                parent,
                "Pixel Moon Cut",
                264f,
                126f,
                28f,
                28f,
                skyHigh
            );

            Vector2[] starPositions =
            {
                new Vector2(-284f, 138f),
                new Vector2(-218f, 92f),
                new Vector2(-82f, 146f),
                new Vector2(68f, 112f),
                new Vector2(164f, 150f),
                new Vector2(334f, 78f)
            };
            for (int index = 0; index < starPositions.Length; index++)
            {
                Vector2 position = starPositions[index];
                CreateTutorialDecoration(
                    parent,
                    "Pixel Star " + index,
                    position.x,
                    position.y,
                    index % 2 == 0 ? 8f : 4f,
                    index % 2 == 0 ? 8f : 4f,
                    moon
                );
            }

            CreateTutorialDecoration(
                parent,
                "Pixel Distant Ruin Left",
                -172f,
                -30f,
                86f,
                64f,
                stone
            );
            CreateTutorialDecoration(
                parent,
                "Pixel Distant Ruin Left Cut",
                -172f,
                -14f,
                34f,
                32f,
                skyLow
            );
            CreateTutorialDecoration(
                parent,
                "Pixel Distant Tower",
                82f,
                -18f,
                42f,
                88f,
                stone
            );
            CreateTutorialDecoration(
                parent,
                "Pixel Distant Tower Crown",
                82f,
                30f,
                62f,
                12f,
                stone
            );

            CreateTutorialDecoration(
                parent,
                "Pixel Forest Left Trunk",
                -362f,
                24f,
                34f,
                196f,
                trunk
            );
            CreateTutorialDecoration(
                parent,
                "Pixel Forest Left Canopy A",
                -342f,
                92f,
                118f,
                92f,
                canopyDark
            );
            CreateTutorialDecoration(
                parent,
                "Pixel Forest Left Canopy B",
                -374f,
                126f,
                68f,
                58f,
                canopyLight
            );

            CreateTutorialDecoration(
                parent,
                "Pixel Forest Right Trunk",
                362f,
                20f,
                34f,
                188f,
                trunk
            );
            CreateTutorialDecoration(
                parent,
                "Pixel Forest Right Canopy A",
                340f,
                88f,
                122f,
                88f,
                canopyDark
            );
            CreateTutorialDecoration(
                parent,
                "Pixel Forest Right Canopy B",
                374f,
                124f,
                68f,
                58f,
                canopyLight
            );

            CreateTutorialDecoration(
                parent,
                "Pixel Ruin Left",
                -244f,
                114f,
                70f,
                46f,
                stone
            );
            CreateTutorialDecoration(
                parent,
                "Pixel Ruin Right",
                246f,
                122f,
                82f,
                38f,
                stone
            );

            Vector2[] grassPositions =
            {
                new Vector2(-300f, -118f),
                new Vector2(-246f, -146f),
                new Vector2(264f, -142f),
                new Vector2(316f, -116f)
            };
            for (int index = 0; index < grassPositions.Length; index++)
            {
                Vector2 position = grassPositions[index];
                CreateTutorialDecoration(
                    parent,
                    "Pixel Grass " + index,
                    position.x,
                    position.y,
                    28f,
                    index % 2 == 0 ? 10f : 14f,
                    grass
                );
            }

            Vector2[] emberPositions =
            {
                new Vector2(-190f, 134f),
                new Vector2(0f, 154f),
                new Vector2(192f, 136f)
            };
            for (int index = 0; index < emberPositions.Length; index++)
            {
                Vector2 position = emberPositions[index];
                CreateTutorialDecoration(
                    parent,
                    "Pixel Ember " + index,
                    position.x,
                    position.y,
                    8f,
                    8f,
                    ember
                );
            }
        }

        private Image CreateTutorialDecoration(
            Transform parent,
            string name,
            float x,
            float y,
            float width,
            float height,
            Color color)
        {
            Image image = CreatePanel(
                parent,
                name,
                x,
                y,
                width,
                height,
                color
            );
            image.raycastTarget = false;
            return image;
        }

        private void ApplyFirstLaunchTutorialPixelSprite(
            Image entity,
            string entityName,
            Color tint)
        {
            if (entity == null)
                return;

            entity.raycastTarget = false;
            entity.enabled = false;

            string[] pattern = ResolveFirstLaunchTutorialPixelPattern(
                entityName
            );
            Texture2D texture = CreateFirstLaunchTutorialPixelTexture(
                entityName,
                pattern
            );
            Sprite sprite = Sprite.Create(
                texture,
                new Rect(0f, 0f, texture.width, texture.height),
                new Vector2(0.5f, 0.5f),
                1f,
                0u,
                SpriteMeshType.FullRect
            );
            sprite.name = entityName + " Pixel Sprite";
            sprite.hideFlags = HideFlags.DontSave;

            firstLaunchTutorialPixelTextures.Add(texture);
            firstLaunchTutorialPixelSprites.Add(sprite);

            GameObject visualObject = new GameObject(
                entityName + " Pixel Visual",
                typeof(RectTransform),
                typeof(CanvasRenderer),
                typeof(Image)
            );
            visualObject.layer = entity.gameObject.layer;
            visualObject.transform.SetParent(entity.rectTransform, false);

            RectTransform visualRect =
                visualObject.GetComponent<RectTransform>();
            visualRect.anchorMin = new Vector2(0.5f, 0.5f);
            visualRect.anchorMax = new Vector2(0.5f, 0.5f);
            visualRect.pivot = new Vector2(0.5f, 0.5f);
            visualRect.anchoredPosition = Vector2.zero;
            visualRect.sizeDelta = entity.rectTransform.sizeDelta;

            Image visualImage = visualObject.GetComponent<Image>();
            visualImage.sprite = sprite;
            visualImage.type = Image.Type.Simple;
            visualImage.preserveAspect = false;
            visualImage.color = tint;
            visualImage.raycastTarget = false;

            RegisterFirstLaunchTutorialWalkVisual(
                entity,
                entityName,
                visualImage,
                sprite
            );
            RegisterFirstLaunchTutorialPixelVisual(
                entityName,
                visualRect,
                visualImage
            );
        }

        private void RegisterFirstLaunchTutorialWalkVisual(
            Image source,
            string entityName,
            Image visual,
            Sprite idle)
        {
            bool isPlayer = entityName.IndexOf(
                "Player",
                StringComparison.OrdinalIgnoreCase) >= 0;
            bool isHorse = entityName.IndexOf(
                "Horse",
                StringComparison.OrdinalIgnoreCase) >= 0;
            bool isEnemy = entityName.IndexOf(
                "Enemy",
                StringComparison.OrdinalIgnoreCase) >= 0 ||
                entityName.IndexOf(
                    "Mini Boss",
                    StringComparison.OrdinalIgnoreCase) >= 0;
            if (!isPlayer && !isHorse && !isEnemy)
                return;

            Sprite stepA = CreateFirstLaunchTutorialPixelSpriteAsset(
                entityName + " Walk A",
                ResolveFirstLaunchTutorialWalkPattern(entityName, 0)
            );
            Sprite stepB = CreateFirstLaunchTutorialPixelSpriteAsset(
                entityName + " Walk B",
                ResolveFirstLaunchTutorialWalkPattern(entityName, 1)
            );
            Sprite actionA = CreateFirstLaunchTutorialPixelSpriteAsset(
                entityName + " Action A",
                ResolveFirstLaunchTutorialActionPattern(entityName, 0)
            );
            Sprite actionB = CreateFirstLaunchTutorialPixelSpriteAsset(
                entityName + " Action B",
                ResolveFirstLaunchTutorialActionPattern(entityName, 1)
            );
            firstLaunchTutorialWalkVisuals.Add(
                new TutorialPixelWalkVisual
                {
                    Source = source,
                    Visual = visual,
                    Idle = idle,
                    StepA = stepA,
                    StepB = stepB,
                    ActionA = actionA,
                    ActionB = actionB,
                    LastSourcePosition =
                        source.rectTransform.anchoredPosition,
                    IsPlayer = isPlayer,
                    IsHorse = isHorse,
                    IsEnemy = isEnemy
                }
            );
        }

        private Sprite CreateFirstLaunchTutorialPixelSpriteAsset(
            string assetName,
            string[] pattern)
        {
            Texture2D texture = CreateFirstLaunchTutorialPixelTexture(
                assetName,
                pattern
            );
            Sprite sprite = Sprite.Create(
                texture,
                new Rect(0f, 0f, texture.width, texture.height),
                new Vector2(0.5f, 0.5f),
                1f,
                0u,
                SpriteMeshType.FullRect
            );
            sprite.name = assetName + " Pixel Sprite";
            sprite.hideFlags = HideFlags.DontSave;
            firstLaunchTutorialPixelTextures.Add(texture);
            firstLaunchTutorialPixelSprites.Add(sprite);
            return sprite;
        }

        private static string[] ResolveFirstLaunchTutorialWalkPattern(
            string entityName,
            int phase)
        {
            bool alternate = phase % 2 != 0;
            if (entityName.IndexOf(
                    "Horse",
                    StringComparison.OrdinalIgnoreCase) >= 0)
            {
                return alternate
                    ? new[]
                    {
                        ".........22...",
                        "..2222222112..",
                        ".21111111112..",
                        "2111111111112.",
                        "21111311111112",
                        ".211111111112.",
                        "...11....11...",
                        ".11........11."
                    }
                    : new[]
                    {
                        ".........22...",
                        "..2222222112..",
                        ".21111111112..",
                        "2111111111112.",
                        "21111311111112",
                        ".211111111112.",
                        ".11........11.",
                        "...11....11..."
                    };
            }

            bool miniBoss = entityName.IndexOf(
                "Mini Boss",
                StringComparison.OrdinalIgnoreCase) >= 0;
            if (miniBoss)
            {
                return alternate
                    ? new[]
                    {
                        "...444444...",
                        "..41111114..",
                        ".4114444114.",
                        "411111111114",
                        "411333333114",
                        "411111111114",
                        ".3111111113.",
                        "..31111113..",
                        ".31......13.",
                        "31........13"
                    }
                    : new[]
                    {
                        "...444444...",
                        "..41111114..",
                        ".4114444114.",
                        "411111111114",
                        "411333333114",
                        "411111111114",
                        ".3111111113.",
                        "..31111113..",
                        "..31....13..",
                        ".31......13."
                    };
            }

            bool enemy = entityName.IndexOf(
                "Enemy",
                StringComparison.OrdinalIgnoreCase) >= 0;
            string head = enemy ? "..444.." : "..222..";
            string face = enemy ? ".41114." : ".21112.";
            string eyes = enemy ? ".41414." : ".21412.";
            return alternate
                ? new[]
                {
                    head,
                    face,
                    eyes,
                    "..111..",
                    ".31113.",
                    "3111113",
                    "3111113",
                    "..111..",
                    "...11..",
                    "..11.1.",
                    ".11..11"
                }
                : new[]
                {
                    head,
                    face,
                    eyes,
                    "..111..",
                    ".31113.",
                    "3111113",
                    "3111113",
                    "..111..",
                    "..11...",
                    ".1.11..",
                    "11..11."
                };
        }

        private Texture2D CreateFirstLaunchTutorialPixelTexture(
            string entityName,
            string[] pattern)
        {
            int height = pattern.Length;
            int width = 0;
            for (int row = 0; row < height; row++)
                width = Mathf.Max(width, pattern[row].Length);

            Texture2D texture = new Texture2D(
                Mathf.Max(1, width),
                Mathf.Max(1, height),
                TextureFormat.RGBA32,
                mipChain: false
            )
            {
                name = entityName + " Pixel Texture",
                filterMode = FilterMode.Point,
                wrapMode = TextureWrapMode.Clamp,
                anisoLevel = 0,
                hideFlags = HideFlags.DontSave
            };

            Color32 clear = new Color32(0, 0, 0, 0);
            Color32 bright = new Color32(255, 255, 255, 255);
            Color32 light = new Color32(208, 208, 208, 255);
            Color32 middle = new Color32(146, 146, 146, 255);
            Color32 dark = new Color32(72, 72, 72, 255);

            Color32[] pixels = new Color32[texture.width * texture.height];
            for (int index = 0; index < pixels.Length; index++)
                pixels[index] = clear;

            for (int sourceRow = 0; sourceRow < height; sourceRow++)
            {
                string row = pattern[sourceRow];
                int targetY = height - sourceRow - 1;
                for (int x = 0; x < row.Length; x++)
                {
                    Color32 color;
                    switch (row[x])
                    {
                        case '1':
                            color = bright;
                            break;
                        case '2':
                            color = light;
                            break;
                        case '3':
                            color = middle;
                            break;
                        case '4':
                            color = dark;
                            break;
                        default:
                            continue;
                    }

                    pixels[targetY * texture.width + x] = color;
                }
            }

            texture.SetPixels32(pixels);
            texture.Apply(updateMipmaps: false, makeNoLongerReadable: true);
            return texture;
        }

        private static string[] ResolveFirstLaunchTutorialPixelPattern(
            string entityName)
        {
            if (entityName.IndexOf(
                    "Player",
                    StringComparison.OrdinalIgnoreCase) >= 0)
            {
                return new[]
                {
                    "..222..",
                    ".21112.",
                    ".21412.",
                    "..111..",
                    ".31113.",
                    "3111113",
                    "3111113",
                    "..111..",
                    "..1.1..",
                    ".11.11.",
                    "11...11"
                };
            }

            if (entityName.IndexOf(
                    "Horse",
                    StringComparison.OrdinalIgnoreCase) >= 0)
            {
                return new[]
                {
                    ".........22...",
                    "..2222222112..",
                    ".21111111112..",
                    "2111111111112.",
                    "21111311111112",
                    ".211111111112.",
                    "..11......11..",
                    ".11........11."
                };
            }

            if (entityName.IndexOf(
                    "Mini Boss",
                    StringComparison.OrdinalIgnoreCase) >= 0)
            {
                return new[]
                {
                    "...444444...",
                    "..41111114..",
                    ".4114444114.",
                    "411111111114",
                    "411333333114",
                    "411111111114",
                    ".3111111113.",
                    "..31111113..",
                    "..31....13..",
                    ".31......13."
                };
            }

            if (entityName.IndexOf(
                    "Secret",
                    StringComparison.OrdinalIgnoreCase) >= 0)
            {
                return new[]
                {
                    "..2..",
                    ".212.",
                    "21112",
                    ".121.",
                    "..2.."
                };
            }

            if (entityName.IndexOf(
                    "Finish Gate",
                    StringComparison.OrdinalIgnoreCase) >= 0)
            {
                return new[]
                {
                    "2222222",
                    "2111112",
                    "21...12",
                    "21...12",
                    "21...12",
                    "21...12",
                    "21...12",
                    "21...12"
                };
            }

            if (entityName.IndexOf(
                    "Jump Root",
                    StringComparison.OrdinalIgnoreCase) >= 0)
            {
                return new[]
                {
                    "..111111..",
                    ".11111111.",
                    "1113331111",
                    ".13333331.",
                    "..33..33.."
                };
            }

            if (entityName.IndexOf(
                    "Enemy",
                    StringComparison.OrdinalIgnoreCase) >= 0)
            {
                return new[]
                {
                    "..444..",
                    ".41114.",
                    ".41414.",
                    "..111..",
                    ".31113.",
                    "3111113",
                    "3111113",
                    ".31113.",
                    "..1.1..",
                    ".11.11.",
                    "11...11"
                };
            }

            if (entityName.IndexOf(
                    "Hazard",
                    StringComparison.OrdinalIgnoreCase) >= 0)
            {
                return new[]
                {
                    ".1..1..1..1.",
                    "111111111111",
                    ".3333333333.",
                    "..44444444.."
                };
            }

            if (entityName.IndexOf(
                    "Projectile",
                    StringComparison.OrdinalIgnoreCase) >= 0)
            {
                return new[]
                {
                    ".222.",
                    "21112",
                    ".222."
                };
            }

            if (entityName.IndexOf(
                    "Collectible",
                    StringComparison.OrdinalIgnoreCase) >= 0)
            {
                return new[]
                {
                    "..2..",
                    ".212.",
                    "21112",
                    ".111.",
                    "21112",
                    ".212.",
                    "..2.."
                };
            }

            return new[]
            {
                "44......44",
                "444....444",
                "4444444444",
                ".44444444."
            };
        }

        private void RegisterFirstLaunchTutorialPixelVisual(
            string entityName,
            RectTransform visual,
            Image image)
        {
            switch (entityName)
            {
                case "Tutorial Player":
                    firstLaunchTutorialPlayerPixelVisual = visual;
                    firstLaunchTutorialPlayerPixelImage = image;
                    break;
                case "Tutorial Horse":
                    firstLaunchTutorialHorsePixelVisual = visual;
                    firstLaunchTutorialHorsePixelImage = image;
                    break;
                case "Tutorial Enemy":
                    firstLaunchTutorialEnemyPixelVisual = visual;
                    firstLaunchTutorialEnemyPixelImage = image;
                    break;
                case "Tutorial Hazard":
                    firstLaunchTutorialHazardPixelVisual = visual;
                    firstLaunchTutorialHazardPixelImage = image;
                    break;
                case "Tutorial Projectile":
                    firstLaunchTutorialProjectilePixelVisual = visual;
                    firstLaunchTutorialProjectilePixelImage = image;
                    break;
                case "Tutorial Collectible":
                    firstLaunchTutorialCollectiblePixelVisual = visual;
                    firstLaunchTutorialCollectiblePixelImage = image;
                    break;
                case "Tutorial Gap":
                    firstLaunchTutorialGapPixelVisual = visual;
                    firstLaunchTutorialGapPixelImage = image;
                    break;
            }
        }

        private static string[] ResolveFirstLaunchTutorialActionPattern(
            string entityName,
            int phase)
        {
            bool committed = phase % 2 != 0;
            if (entityName.IndexOf(
                    "Horse",
                    StringComparison.OrdinalIgnoreCase) >= 0)
            {
                return committed
                    ? new[]
                    {
                        "..........22..",
                        "..22222222112.",
                        ".211111111112.",
                        "21111111111112",
                        "21111311111112",
                        ".211111111112.",
                        "...11...11....",
                        "..11.....11..."
                    }
                    : new[]
                    {
                        ".........22...",
                        "..2222222112..",
                        ".21111111112..",
                        "2111111111112.",
                        "21111311111112",
                        ".211111111112.",
                        "..11......11..",
                        ".11........11."
                    };
            }

            bool miniBoss = entityName.IndexOf(
                "Mini Boss",
                StringComparison.OrdinalIgnoreCase) >= 0;
            if (miniBoss)
            {
                return committed
                    ? new[]
                    {
                        "...444444...",
                        "..41111114..",
                        ".4114444114.",
                        "411111111114",
                        "333111111333",
                        ".3311111133.",
                        "..3111113...",
                        ".31.1111.13.",
                        "31........13",
                        "3..........3"
                    }
                    : new[]
                    {
                        "...444444...",
                        "..41111114..",
                        ".4114444114.",
                        "411111111114",
                        ".3311111133.",
                        "..31111113..",
                        "..31111113..",
                        ".31......13.",
                        "31........13",
                        ".3........3."
                    };
            }

            bool enemy = entityName.IndexOf(
                "Enemy",
                StringComparison.OrdinalIgnoreCase) >= 0;
            if (enemy)
            {
                return committed
                    ? new[]
                    {
                        "..444....",
                        ".41114...",
                        ".41414...",
                        "..111.33.",
                        ".311133..",
                        "311111...",
                        ".31113...",
                        "..111....",
                        ".11.11...",
                        "11...11.."
                    }
                    : new[]
                    {
                        "..444..",
                        ".41114.",
                        ".41414.",
                        "..111..",
                        "3311133",
                        ".31113.",
                        "3111113",
                        "..111..",
                        ".11.11.",
                        "11...11"
                    };
            }

            return committed
                ? new[]
                {
                    "..222.....",
                    ".21112....",
                    ".21412....",
                    "..111.33..",
                    ".311133...",
                    "311111....",
                    ".31113....",
                    "..111.....",
                    ".11..11...",
                    "11....11.."
                }
                : new[]
                {
                    "..222..",
                    ".21112.",
                    ".21412.",
                    "..111..",
                    "3311133",
                    ".31113.",
                    "3111113",
                    "..111..",
                    ".11.11.",
                    "11...11"
                };
        }

        private void BeginFirstLaunchTutorialInstructionPresentation(
            FirstLaunchTutorialStep step)
        {
            firstLaunchTutorialInstructionStartedAt = Time.unscaledTime;

            if (firstLaunchTutorialInstructionAccent != null)
            {
                firstLaunchTutorialInstructionAccent.color =
                    ResolveFirstLaunchTutorialInstructionAccent(step);
            }

            firstLaunchTutorialInstructionVisibility =
                firstLaunchTutorialInstructionRequested ? 0f : 0f;

            if (firstLaunchTutorialInstructionCanvasGroup != null)
                firstLaunchTutorialInstructionCanvasGroup.alpha = 0f;

            if (firstLaunchTutorialInstructionRect != null)
            {
                firstLaunchTutorialInstructionRect.anchoredPosition =
                    TutorialInstructionRestPosition +
                    new Vector2(0f, -12f);
                firstLaunchTutorialInstructionRect.localScale =
                    new Vector3(0.955f, 0.955f, 1f);
            }
        }

        private static Color ResolveFirstLaunchTutorialInstructionAccent(
            FirstLaunchTutorialStep step)
        {
            switch (step)
            {
                case FirstLaunchTutorialStep.AttackEnemy:
                case FirstLaunchTutorialStep.HeavyAttack:
                case FirstLaunchTutorialStep.SpinAttack:
                case FirstLaunchTutorialStep.Parry:
                    return new Color(1f, 0.38f, 0.20f, 1f);
                case FirstLaunchTutorialStep.HealHorse:
                case FirstLaunchTutorialStep.Completed:
                    return new Color(0.28f, 1f, 0.62f, 1f);
                case FirstLaunchTutorialStep.Move:
                case FirstLaunchTutorialStep.Dodge:
                case FirstLaunchTutorialStep.Grapple:
                    return new Color(0.22f, 0.82f, 1f, 1f);
                default:
                    return new Color(0.52f, 0.72f, 1f, 1f);
            }
        }

        private void UpdateFirstLaunchTutorialVisualPresentation()
        {
            UpdateFirstLaunchTutorialInstructionPresentation();
            UpdateFirstLaunchTutorialBindingPresentation();
            UpdateFirstLaunchTutorialPixelAnimation();
            UpdateFirstLaunchTutorialActionPresentation();
            UpdateFirstLaunchTutorialFeedbackPresentation();
            UpdateFirstLaunchTutorialV1011Polish();
        }

        private void UpdateFirstLaunchTutorialInstructionPresentation()
        {
            if (firstLaunchTutorialInstructionRect == null ||
                firstLaunchTutorialInstructionCanvasGroup == null ||
                !firstLaunchTutorialInstructionRect.gameObject.activeInHierarchy)
            {
                return;
            }

            float target =
                firstLaunchTutorialInstructionRequested ? 1f : 0f;
            float speed =
                firstLaunchTutorialInstructionRequested ? 8.5f : 6.5f;
            firstLaunchTutorialInstructionVisibility =
                Mathf.MoveTowards(
                    firstLaunchTutorialInstructionVisibility,
                    target,
                    speed * Time.unscaledDeltaTime
                );

            float entrance = Mathf.Clamp01(
                (Time.unscaledTime -
                 firstLaunchTutorialInstructionStartedAt) /
                0.22f
            );
            float easedEntrance =
                1f - Mathf.Pow(1f - entrance, 3f);
            float visible =
                firstLaunchTutorialInstructionRequested
                    ? Mathf.Min(
                        firstLaunchTutorialInstructionVisibility,
                        easedEntrance
                    )
                    : firstLaunchTutorialInstructionVisibility;

            firstLaunchTutorialInstructionCanvasGroup.alpha = visible;
            float steppedVisible =
                Mathf.Floor(visible * 4f) / 4f;
            firstLaunchTutorialInstructionRect.anchoredPosition =
                TutorialInstructionRestPosition +
                new Vector2(
                    0f,
                    SnapFirstLaunchTutorialPixelValue(
                        -12f * (1f - steppedVisible)
                    )
                );
            float scale = steppedVisible >= 0.75f ? 1f : 0.96f;
            firstLaunchTutorialInstructionRect.localScale =
                new Vector3(scale, scale, 1f);

            if (firstLaunchTutorialInstructionAccent != null)
            {
                Color color = firstLaunchTutorialInstructionAccent.color;
                float pulse =
                    Mathf.FloorToInt(Time.unscaledTime * 4f) % 4 == 0
                        ? 1f
                        : 0.86f;
                color.a = Mathf.Clamp01(pulse * visible);
                firstLaunchTutorialInstructionAccent.color = color;
            }
        }

        private void UpdateFirstLaunchTutorialBindingPresentation()
        {
            if (firstLaunchTutorialKeyboardBindingCard == null ||
                firstLaunchTutorialHandheldBindingCard == null)
            {
                return;
            }

            bool keyboardActive =
                firstLaunchTutorialInputSource ==
                    FirstLaunchTutorialInputSource.Keyboard;
            bool handheldActive =
                firstLaunchTutorialInputSource ==
                    FirstLaunchTutorialInputSource.Handheld ||
                firstLaunchTutorialInputSource ==
                    FirstLaunchTutorialInputSource.Gamepad ||
                firstLaunchTutorialInputSource ==
                    FirstLaunchTutorialInputSource.Touch;

            firstLaunchTutorialKeyboardBindingCard.color =
                keyboardActive
                    ? new Color(0.07f, 0.24f, 0.31f, 1f)
                    : new Color(0.045f, 0.075f, 0.105f, 1f);
            firstLaunchTutorialHandheldBindingCard.color =
                handheldActive
                    ? new Color(0.20f, 0.11f, 0.30f, 1f)
                    : new Color(0.075f, 0.055f, 0.11f, 1f);
        }

        private static float SnapFirstLaunchTutorialPixelValue(
            float value,
            float increment = 4f)
        {
            float safeIncrement = Mathf.Max(1f, increment);
            return Mathf.Round(value / safeIncrement) * safeIncrement;
        }

        private void UpdateFirstLaunchTutorialPixelAnimation()
        {
            int frame = Mathf.FloorToInt(Time.unscaledTime * 4f);
            bool walking =
                firstLaunchTutorialMovementActive &&
                !firstLaunchTutorialMounted;
            bool riding =
                firstLaunchTutorialMovementActive &&
                firstLaunchTutorialMounted;

            bool airborne = !firstLaunchTutorialGrounded &&
                !firstLaunchTutorialMounted;
            float playerOffset = airborne
                ? (firstLaunchTutorialVerticalVelocity >= 0f ? 3f : -1f)
                : walking && frame % 2 == 1 ? 2f : 0f;
            Vector3 playerScale = airborne
                ? firstLaunchTutorialVerticalVelocity >= 0f
                    ? new Vector3(0.92f, 1.08f, 1f)
                    : new Vector3(1.04f, 0.96f, 1f)
                : Vector3.one;
            UpdateFirstLaunchTutorialPixelVisual(
                firstLaunchTutorialPlayer,
                firstLaunchTutorialPlayerPixelVisual,
                firstLaunchTutorialPlayerPixelImage,
                playerOffset,
                playerScale
            );
            UpdateFirstLaunchTutorialPixelVisual(
                firstLaunchTutorialHorse,
                firstLaunchTutorialHorsePixelVisual,
                firstLaunchTutorialHorsePixelImage,
                (riding || firstLaunchTutorialMovementActive) &&
                    frame % 2 == 0
                    ? 2f
                    : 0f,
                riding && frame % 4 == 1
                    ? new Vector3(1.02f, 0.98f, 1f)
                    : Vector3.one
            );
            UpdateFirstLaunchTutorialPixelVisual(
                firstLaunchTutorialEnemy,
                firstLaunchTutorialEnemyPixelVisual,
                firstLaunchTutorialEnemyPixelImage,
                frame % 4 == 3 ? 2f : 0f,
                Vector3.one
            );
            UpdateFirstLaunchTutorialPixelVisual(
                firstLaunchTutorialHazard,
                firstLaunchTutorialHazardPixelVisual,
                firstLaunchTutorialHazardPixelImage,
                0f,
                Vector3.one
            );
            UpdateFirstLaunchTutorialPixelVisual(
                firstLaunchTutorialProjectile,
                firstLaunchTutorialProjectilePixelVisual,
                firstLaunchTutorialProjectilePixelImage,
                frame % 2 == 0 ? 0f : 1f,
                frame % 2 == 0
                    ? Vector3.one
                    : new Vector3(1.08f, 1f, 1f)
            );
            UpdateFirstLaunchTutorialPixelVisual(
                firstLaunchTutorialCollectible,
                firstLaunchTutorialCollectiblePixelVisual,
                firstLaunchTutorialCollectiblePixelImage,
                frame % 4 < 2 ? 0f : 2f,
                frame % 4 < 2
                    ? Vector3.one
                    : new Vector3(1.06f, 1.06f, 1f)
            );
            UpdateFirstLaunchTutorialPixelVisual(
                firstLaunchTutorialGap,
                firstLaunchTutorialGapPixelVisual,
                firstLaunchTutorialGapPixelImage,
                0f,
                Vector3.one
            );
            UpdateFirstLaunchTutorialWalkFrames(frame, walking, riding);
        }

        private void UpdateFirstLaunchTutorialWalkFrames(
            int frame,
            bool playerWalking,
            bool horseRiding)
        {
            for (int index = 0;
                 index < firstLaunchTutorialWalkVisuals.Count;
                 index++)
            {
                TutorialPixelWalkVisual entry =
                    firstLaunchTutorialWalkVisuals[index];
                if (entry.Source == null || entry.Visual == null)
                    continue;

                Vector2 currentPosition =
                    entry.Source.rectTransform.anchoredPosition;
                bool translated = Vector2.SqrMagnitude(
                    currentPosition - entry.LastSourcePosition
                ) > 0.25f;
                entry.LastSourcePosition = currentPosition;

                bool acting =
                    entry.IsPlayer
                        ? IsFirstLaunchTutorialPlayerVisualActing()
                        : entry.IsHorse
                            ? firstLaunchTutorialActionPresentationType ==
                                  FirstLaunchTutorialActionPresentationType
                                      .HorseHit
                            : entry.IsEnemy &&
                              IsFirstLaunchTutorialEnemyVisualAttacking(
                                  entry.Source
                              );
                if (acting)
                {
                    entry.Visual.sprite = frame % 2 == 0
                        ? entry.ActionA
                        : entry.ActionB;
                    continue;
                }

                bool stepping =
                    entry.IsPlayer
                        ? playerWalking
                        : entry.IsHorse
                            ? horseRiding || translated ||
                              firstLaunchTutorialStep ==
                                  FirstLaunchTutorialStep.HorseReturn
                            : entry.IsEnemy &&
                              (translated ||
                               IsFirstLaunchTutorialEnemyVisualAttacking(
                                   entry.Source
                               ));

                if (!stepping)
                {
                    entry.Visual.sprite = entry.Idle;
                    continue;
                }

                entry.Visual.sprite = frame % 2 == 0
                    ? entry.StepA
                    : entry.StepB;
            }
        }

        private bool IsFirstLaunchTutorialPlayerVisualActing()
        {
            switch (firstLaunchTutorialActionPresentationType)
            {
                case FirstLaunchTutorialActionPresentationType.LightAttack:
                case FirstLaunchTutorialActionPresentationType.RangedAttack:
                case FirstLaunchTutorialActionPresentationType.Dodge:
                case FirstLaunchTutorialActionPresentationType.HeavyAttack:
                case FirstLaunchTutorialActionPresentationType.SpinAttack:
                case FirstLaunchTutorialActionPresentationType.Parry:
                case FirstLaunchTutorialActionPresentationType.Grapple:
                case FirstLaunchTutorialActionPresentationType.Heal:
                    return true;
                default:
                    return false;
            }
        }

        private bool IsFirstLaunchTutorialEnemyVisualAttacking(
            Image source)
        {
            for (int index = 0;
                 index < firstLaunchTutorialActors.Count;
                 index++)
            {
                TutorialEnemyActor actor =
                    firstLaunchTutorialActors[index];
                if (actor.Image == source)
                    return actor.Active && !actor.Dead &&
                           actor.AttackCommitted;
            }
            return false;
        }

        private static void UpdateFirstLaunchTutorialPixelVisual(
            Image source,
            RectTransform visual,
            Image visualImage,
            float yOffset,
            Vector3 scale)
        {
            if (source == null ||
                visual == null ||
                visualImage == null)
            {
                return;
            }

            visual.anchoredPosition = new Vector2(0f, yOffset);
            visual.localScale = scale;
            visualImage.color = source.color;
        }

        private void UpdateFirstLaunchTutorialFeedbackPresentation()
        {
            if (firstLaunchTutorialFeedback == null)
                return;

            Color color = firstLaunchTutorialFeedback.color;
            if (firstLaunchTutorialFeedbackClearAt <= 0f)
            {
                color.a = string.IsNullOrEmpty(
                    firstLaunchTutorialFeedback.text
                )
                    ? 0f
                    : 1f;
                firstLaunchTutorialFeedback.color = color;
                return;
            }

            float remaining =
                firstLaunchTutorialFeedbackClearAt -
                Time.unscaledTime;
            color.a = Mathf.Clamp01(remaining / 0.18f);
            firstLaunchTutorialFeedback.color = color;
        }

        private void DisposeFirstLaunchTutorialPixelAssets()
        {
            for (int index = 0;
                 index < firstLaunchTutorialPixelSprites.Count;
                 index++)
            {
                DestroyFirstLaunchTutorialRuntimeObject(
                    firstLaunchTutorialPixelSprites[index]
                );
            }

            for (int index = 0;
                 index < firstLaunchTutorialPixelTextures.Count;
                 index++)
            {
                DestroyFirstLaunchTutorialRuntimeObject(
                    firstLaunchTutorialPixelTextures[index]
                );
            }

            firstLaunchTutorialPixelSprites.Clear();
            firstLaunchTutorialPixelTextures.Clear();
            firstLaunchTutorialWalkVisuals.Clear();

            firstLaunchTutorialPlayerPixelVisual = null;
            firstLaunchTutorialHorsePixelVisual = null;
            firstLaunchTutorialEnemyPixelVisual = null;
            firstLaunchTutorialHazardPixelVisual = null;
            firstLaunchTutorialProjectilePixelVisual = null;
            firstLaunchTutorialCollectiblePixelVisual = null;
            firstLaunchTutorialGapPixelVisual = null;

            firstLaunchTutorialPlayerPixelImage = null;
            firstLaunchTutorialHorsePixelImage = null;
            firstLaunchTutorialEnemyPixelImage = null;
            firstLaunchTutorialHazardPixelImage = null;
            firstLaunchTutorialProjectilePixelImage = null;
            firstLaunchTutorialCollectiblePixelImage = null;
            firstLaunchTutorialGapPixelImage = null;
        }

        private static void DestroyFirstLaunchTutorialRuntimeObject(
            UnityEngine.Object value)
        {
            if (value == null)
                return;

            if (Application.isPlaying)
                UnityEngine.Object.Destroy(value);
            else
                UnityEngine.Object.DestroyImmediate(value);
        }
    }
}
