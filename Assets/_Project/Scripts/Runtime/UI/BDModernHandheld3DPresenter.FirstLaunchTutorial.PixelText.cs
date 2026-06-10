using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BoredomAndDungeons
{
    public sealed partial class BDModernHandheld3DPresenter
    {
        private static readonly Dictionary<char, string[]>
            FirstLaunchTutorialPixelGlyphs =
                new Dictionary<char, string[]>
                {
                    ['A'] = new[] { "01110", "10001", "10001", "11111", "10001", "10001", "10001" },
                    ['B'] = new[] { "11110", "10001", "10001", "11110", "10001", "10001", "11110" },
                    ['C'] = new[] { "01111", "10000", "10000", "10000", "10000", "10000", "01111" },
                    ['D'] = new[] { "11110", "10001", "10001", "10001", "10001", "10001", "11110" },
                    ['E'] = new[] { "11111", "10000", "10000", "11110", "10000", "10000", "11111" },
                    ['F'] = new[] { "11111", "10000", "10000", "11110", "10000", "10000", "10000" },
                    ['G'] = new[] { "01111", "10000", "10000", "10111", "10001", "10001", "01110" },
                    ['H'] = new[] { "10001", "10001", "10001", "11111", "10001", "10001", "10001" },
                    ['I'] = new[] { "11111", "00100", "00100", "00100", "00100", "00100", "11111" },
                    ['J'] = new[] { "00111", "00010", "00010", "00010", "10010", "10010", "01100" },
                    ['K'] = new[] { "10001", "10010", "10100", "11000", "10100", "10010", "10001" },
                    ['L'] = new[] { "10000", "10000", "10000", "10000", "10000", "10000", "11111" },
                    ['M'] = new[] { "10001", "11011", "10101", "10101", "10001", "10001", "10001" },
                    ['N'] = new[] { "10001", "11001", "10101", "10011", "10001", "10001", "10001" },
                    ['O'] = new[] { "01110", "10001", "10001", "10001", "10001", "10001", "01110" },
                    ['P'] = new[] { "11110", "10001", "10001", "11110", "10000", "10000", "10000" },
                    ['Q'] = new[] { "01110", "10001", "10001", "10001", "10101", "10010", "01101" },
                    ['R'] = new[] { "11110", "10001", "10001", "11110", "10100", "10010", "10001" },
                    ['S'] = new[] { "01111", "10000", "10000", "01110", "00001", "00001", "11110" },
                    ['T'] = new[] { "11111", "00100", "00100", "00100", "00100", "00100", "00100" },
                    ['U'] = new[] { "10001", "10001", "10001", "10001", "10001", "10001", "01110" },
                    ['V'] = new[] { "10001", "10001", "10001", "10001", "10001", "01010", "00100" },
                    ['W'] = new[] { "10001", "10001", "10001", "10101", "10101", "11011", "10001" },
                    ['X'] = new[] { "10001", "10001", "01010", "00100", "01010", "10001", "10001" },
                    ['Y'] = new[] { "10001", "10001", "01010", "00100", "00100", "00100", "00100" },
                    ['Z'] = new[] { "11111", "00001", "00010", "00100", "01000", "10000", "11111" },
                    ['0'] = new[] { "01110", "10001", "10011", "10101", "11001", "10001", "01110" },
                    ['1'] = new[] { "00100", "01100", "00100", "00100", "00100", "00100", "01110" },
                    ['2'] = new[] { "01110", "10001", "00001", "00010", "00100", "01000", "11111" },
                    ['3'] = new[] { "11110", "00001", "00001", "01110", "00001", "00001", "11110" },
                    ['4'] = new[] { "00010", "00110", "01010", "10010", "11111", "00010", "00010" },
                    ['5'] = new[] { "11111", "10000", "10000", "11110", "00001", "00001", "11110" },
                    ['6'] = new[] { "01110", "10000", "10000", "11110", "10001", "10001", "01110" },
                    ['7'] = new[] { "11111", "00001", "00010", "00100", "01000", "01000", "01000" },
                    ['8'] = new[] { "01110", "10001", "10001", "01110", "10001", "10001", "01110" },
                    ['9'] = new[] { "01110", "10001", "10001", "01111", "00001", "00001", "01110" },
                    ['&'] = new[] { "01100", "10010", "10100", "01000", "10101", "10010", "01101" },
                    ['/'] = new[] { "00001", "00010", "00010", "00100", "01000", "01000", "10000" },
                    ['-'] = new[] { "00000", "00000", "00000", "11111", "00000", "00000", "00000" },
                    ['.'] = new[] { "00000", "00000", "00000", "00000", "00000", "00110", "00110" },
                    [':'] = new[] { "00000", "00110", "00110", "00000", "00110", "00110", "00000" },
                    ['?'] = new[] { "01110", "10001", "00001", "00010", "00100", "00000", "00100" },
                    [' '] = new[] { "000", "000", "000", "000", "000", "000", "000" }
                };

        private Image CreateFirstLaunchTutorialPixelText(
            Transform parent,
            string name,
            string text,
            float x,
            float y,
            float width,
            float height,
            Color color,
            int pixelScale,
            int letterSpacing)
        {
            GameObject value = new GameObject(
                name,
                typeof(RectTransform),
                typeof(CanvasRenderer),
                typeof(Image)
            );
            value.layer = parent.gameObject.layer;
            value.transform.SetParent(parent, false);

            RectTransform rect = value.GetComponent<RectTransform>();
            rect.anchorMin = new Vector2(0.5f, 0.5f);
            rect.anchorMax = new Vector2(0.5f, 0.5f);
            rect.pivot = new Vector2(0.5f, 0.5f);
            rect.anchoredPosition = new Vector2(x, y);
            rect.sizeDelta = new Vector2(width, height);

            Image image = value.GetComponent<Image>();
            image.type = Image.Type.Simple;
            image.preserveAspect = true;
            image.raycastTarget = false;
            image.color = color;
            SetFirstLaunchTutorialPixelText(
                image,
                text,
                pixelScale,
                letterSpacing
            );
            return image;
        }

        private void SetFirstLaunchTutorialPixelText(
            Image image,
            string text,
            int pixelScale = 2,
            int letterSpacing = 1)
        {
            if (image == null)
                return;

            Texture2D texture = CreateFirstLaunchTutorialPixelTextTexture(
                image.gameObject.name,
                text,
                pixelScale,
                letterSpacing
            );
            Sprite sprite = Sprite.Create(
                texture,
                new Rect(0f, 0f, texture.width, texture.height),
                new Vector2(0.5f, 0.5f),
                1f,
                0u,
                SpriteMeshType.FullRect
            );
            sprite.name = image.gameObject.name + " Pixel Text Sprite";
            sprite.hideFlags = HideFlags.DontSave;

            firstLaunchTutorialPixelTextures.Add(texture);
            firstLaunchTutorialPixelSprites.Add(sprite);
            image.sprite = sprite;
        }

        private Texture2D CreateFirstLaunchTutorialPixelTextTexture(
            string name,
            string text,
            int requestedPixelScale,
            int requestedLetterSpacing)
        {
            string normalized = string.IsNullOrEmpty(text)
                ? " "
                : text.ToUpperInvariant();
            int pixelScale = Mathf.Clamp(requestedPixelScale, 1, 12);
            int spacing = Mathf.Clamp(requestedLetterSpacing, 1, 4);
            int logicalWidth = 0;

            for (int index = 0; index < normalized.Length; index++)
            {
                string[] glyph = ResolveFirstLaunchTutorialPixelGlyph(
                    normalized[index]
                );
                logicalWidth += glyph[0].Length;
                if (index < normalized.Length - 1)
                    logicalWidth += spacing;
            }

            int logicalHeight = 7;
            Texture2D texture = new Texture2D(
                Mathf.Max(1, logicalWidth * pixelScale),
                logicalHeight * pixelScale,
                TextureFormat.RGBA32,
                mipChain: false
            )
            {
                name = name + " Pixel Text Texture",
                filterMode = FilterMode.Point,
                wrapMode = TextureWrapMode.Clamp,
                anisoLevel = 0,
                hideFlags = HideFlags.DontSave
            };

            Color32[] pixels = new Color32[
                texture.width * texture.height
            ];
            Color32 clear = new Color32(0, 0, 0, 0);
            Color32 white = new Color32(255, 255, 255, 255);
            for (int index = 0; index < pixels.Length; index++)
                pixels[index] = clear;

            int cursor = 0;
            for (int characterIndex = 0;
                 characterIndex < normalized.Length;
                 characterIndex++)
            {
                string[] glyph = ResolveFirstLaunchTutorialPixelGlyph(
                    normalized[characterIndex]
                );
                int glyphWidth = glyph[0].Length;

                for (int row = 0; row < glyph.Length; row++)
                {
                    int targetRow = logicalHeight - row - 1;
                    for (int column = 0;
                         column < glyph[row].Length;
                         column++)
                    {
                        if (glyph[row][column] != '1')
                            continue;

                        int logicalX = cursor + column;
                        for (int scaleY = 0;
                             scaleY < pixelScale;
                             scaleY++)
                        {
                            int targetY =
                                targetRow * pixelScale + scaleY;
                            for (int scaleX = 0;
                                 scaleX < pixelScale;
                                 scaleX++)
                            {
                                int targetX =
                                    logicalX * pixelScale + scaleX;
                                pixels[targetY * texture.width + targetX] =
                                    white;
                            }
                        }
                    }
                }

                cursor += glyphWidth;
                if (characterIndex < normalized.Length - 1)
                    cursor += spacing;
            }

            texture.SetPixels32(pixels);
            texture.Apply(updateMipmaps: false, makeNoLongerReadable: true);
            return texture;
        }

        private static string[] ResolveFirstLaunchTutorialPixelGlyph(
            char character)
        {
            string[] glyph;
            if (FirstLaunchTutorialPixelGlyphs.TryGetValue(
                    character,
                    out glyph))
            {
                return glyph;
            }

            return FirstLaunchTutorialPixelGlyphs['?'];
        }
    }
}
