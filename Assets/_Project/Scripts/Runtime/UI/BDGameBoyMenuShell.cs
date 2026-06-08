using UnityEngine;
using UnityEngine.SceneManagement;

namespace BoredomAndDungeons
{
    [DefaultExecutionOrder(-820)]
    [DisallowMultipleComponent]
    public sealed class BDGameBoyMenuShell : MonoBehaviour
    {
        // BD GAME BOY MENU SHELL AND UI OWNERSHIP V23R10
        // BD INTEGRATED GAME BOY MENU SHELL V23R12
        // The shell is drawn by BDMainMenuFlow in the same IMGUI pass as the
        // menu content. This removes independent OnGUI depth/order races that
        // previously produced either an empty black device or a plain menu.
        private static BDGameBoyMenuShell instance;

        private GUIStyle smallLabel;
        private GUIStyle deviceLabel;
        private GUIStyle screenLabel;
        private GUIStyle roundedStyle;
        private Texture2D pixel;
        private Texture2D roundedRectTexture;
        private Texture2D softGlowTexture;
        private Texture2D scanlineTexture;
        private Texture2D screenVignetteTexture;
        private string currentScreenMode = "MEMORY // READY";

        [RuntimeInitializeOnLoadMethod(
            RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void Install()
        {
            if (Object.FindFirstObjectByType<
                    BDGameBoyMenuShell>() != null)
            {
                return;
            }

            GameObject root = new GameObject(
                "B&D Game Boy Menu Shell"
            );
            Object.DontDestroyOnLoad(root);
            root.AddComponent<BDGameBoyMenuShell>();

            SceneManager.sceneLoaded -= HandleSceneLoaded;
            SceneManager.sceneLoaded += HandleSceneLoaded;
        }

        private static void HandleSceneLoaded(
            Scene scene,
            LoadSceneMode mode)
        {
            if (Object.FindFirstObjectByType<
                    BDGameBoyMenuShell>() == null)
            {
                Install();
            }
        }

        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
                return;
            }

            instance = this;
            pixel = Texture2D.whiteTexture;
            EnsureResources();
        }

        private void OnDestroy()
        {
            if (instance == this)
                instance = null;

            DestroyGeneratedTexture(ref roundedRectTexture);
            DestroyGeneratedTexture(ref softGlowTexture);
            DestroyGeneratedTexture(ref scanlineTexture);
            DestroyGeneratedTexture(ref screenVignetteTexture);
        }

        public static void DrawIntegrated(
            Rect content,
            float virtualWidth,
            float virtualHeight)
        {
            DrawIntegrated(
                content,
                virtualWidth,
                virtualHeight,
                "MEMORY // READY"
            );
        }

        public static void DrawIntegrated(
            Rect content,
            float virtualWidth,
            float virtualHeight,
            string screenMode)
        {
            if (instance == null)
            {
                instance = Object.FindFirstObjectByType<
                    BDGameBoyMenuShell>();
            }

            if (instance == null)
                return;

            instance.DrawIntegratedInternal(
                content,
                virtualWidth,
                virtualHeight,
                screenMode
            );
        }

        public static void DrawScreenOverlay(
            Rect content,
            float virtualWidth,
            float virtualHeight)
        {
            if (instance == null)
            {
                instance = Object.FindFirstObjectByType<
                    BDGameBoyMenuShell>();
            }

            if (instance == null)
                return;

            instance.DrawScreenOverlayInternal(content);
        }

        // BD PROFESSIONAL MEMORY-HANDHELD SHELL V23R19Q
        private void DrawIntegratedInternal(
            Rect content,
            float virtualWidth,
            float virtualHeight,
            string screenMode)
        {
            EnsureResources();
            EnsureStyles();
            currentScreenMode = string.IsNullOrEmpty(screenMode)
                ? "MEMORY // READY"
                : screenMode;

            Color previousColor = GUI.color;

            Rect body = new Rect(
                content.x - 52f,
                content.y - 42f,
                content.width + 104f,
                content.height + 112f
            );

            Rect bezel = new Rect(
                content.x - 15f,
                content.y - 15f,
                content.width + 30f,
                content.height + 30f
            );

            bool awakened = BDGameProgress.MotherDefeated;

            Color bodyColor = awakened
                ? new Color(0.055f, 0.17f, 0.18f, 0.99f)
                : new Color(0.105f, 0.125f, 0.15f, 0.99f);

            Color bodyHighlight = awakened
                ? new Color(0.12f, 0.30f, 0.30f, 0.92f)
                : new Color(0.19f, 0.21f, 0.24f, 0.92f);

            Color trim = awakened
                ? new Color(0.28f, 1f, 0.92f, 0.96f)
                : new Color(0.92f, 0.68f, 0.26f, 0.93f);

            DrawRoundedRect(
                new Rect(
                    body.x + 10f,
                    body.y + 15f,
                    body.width,
                    body.height
                ),
                new Color(0f, 0f, 0f, 0.52f)
            );

            GUI.color = new Color(
                trim.r,
                trim.g,
                trim.b,
                awakened ? 0.10f : 0.07f
            );
            GUI.DrawTexture(
                new Rect(
                    body.x - 22f,
                    body.y - 18f,
                    body.width + 44f,
                    body.height + 52f
                ),
                softGlowTexture,
                ScaleMode.StretchToFill,
                alphaBlend: true
            );
            GUI.color = previousColor;

            DrawRoundedRect(body, bodyColor);
            DrawRoundedBorder(
                body,
                new Color(
                    bodyHighlight.r,
                    bodyHighlight.g,
                    bodyHighlight.b,
                    0.88f
                ),
                2f
            );

            Rect cartridgeRidge = new Rect(
                body.center.x - 112f,
                body.y - 3f,
                224f,
                24f
            );
            DrawRoundedRect(
                cartridgeRidge,
                new Color(
                    bodyHighlight.r,
                    bodyHighlight.g,
                    bodyHighlight.b,
                    0.78f
                )
            );
            DrawRoundedBorder(
                cartridgeRidge,
                new Color(trim.r, trim.g, trim.b, 0.34f),
                1f
            );

            DrawScreenHousing(
                content,
                bezel,
                trim,
                awakened
            );

            DrawScrew(
                new Vector2(body.x + 18f, body.y + 18f),
                bodyHighlight
            );
            DrawScrew(
                new Vector2(body.xMax - 18f, body.y + 18f),
                bodyHighlight
            );
            DrawScrew(
                new Vector2(body.x + 18f, body.yMax - 18f),
                bodyHighlight
            );
            DrawScrew(
                new Vector2(body.xMax - 18f, body.yMax - 18f),
                bodyHighlight
            );

            DrawRoundedRect(
                new Rect(
                    body.x + 22f,
                    body.y + 18f,
                    8f,
                    8f
                ),
                awakened
                    ? trim
                    : new Color(0.50f, 0.95f, 0.44f, 1f)
            );

            GUI.Label(
                new Rect(
                    body.x + 38f,
                    body.y + 8f,
                    300f,
                    28f
                ),
                awakened
                    ? "B&D // AWAKENED MEMORY"
                    : "B&D // POCKET MEMORY",
                deviceLabel
            );

            float controlsY = body.yMax - 62f;

            DrawDPad(
                new Vector2(
                    body.x + 84f,
                    controlsY + 21f
                ),
                trim
            );

            DrawActionButtons(
                new Vector2(
                    body.xMax - 104f,
                    controlsY + 21f
                ),
                trim
            );

            DrawStartSelectButtons(
                new Vector2(
                    body.center.x,
                    controlsY + 17f
                ),
                trim
            );

            DrawSpeaker(
                new Rect(
                    body.center.x - 45f,
                    controlsY + 32f,
                    90f,
                    18f
                ),
                trim
            );

            GUI.Label(
                new Rect(
                    body.center.x - 140f,
                    body.yMax - 23f,
                    280f,
                    16f
                ),
                awakened
                    ? "TRUE VICTORY LINK ACTIVE"
                    : "MEMORY CARTRIDGE READY",
                smallLabel
            );

            GUI.color = previousColor;
        }

        private void DrawScreenOverlayInternal(Rect content)
        {
            EnsureResources();
            EnsureStyles();

            Color previous = GUI.color;

            GUI.color = new Color(1f, 1f, 1f, 0.13f);
            GUI.DrawTextureWithTexCoords(
                content,
                scanlineTexture,
                new Rect(
                    0f,
                    0f,
                    Mathf.Max(1f, content.width / 8f),
                    Mathf.Max(1f, content.height / 8f)
                ),
                alphaBlend: true
            );

            GUI.color = new Color(1f, 1f, 1f, 0.50f);
            GUI.DrawTexture(
                content,
                screenVignetteTexture,
                ScaleMode.StretchToFill,
                alphaBlend: true
            );

            DrawRoundedRect(
                new Rect(
                    content.x + 10f,
                    content.y + 8f,
                    content.width * 0.44f,
                    content.height * 0.12f
                ),
                new Color(0.72f, 0.92f, 0.94f, 0.055f)
            );

            DrawRoundedBorder(
                content,
                new Color(0.68f, 0.90f, 0.84f, 0.22f),
                1f
            );

            GUI.color = Color.white;
            GUI.Label(
                new Rect(
                    content.x + 18f,
                    content.y + 7f,
                    content.width - 36f,
                    22f
                ),
                currentScreenMode,
                screenLabel
            );

            GUI.color = previous;
        }

        private void DrawScreenHousing(
            Rect content,
            Rect bezel,
            Color trim,
            bool awakened)
        {
            Color previousColor = GUI.color;

            DrawRoundedRect(
                bezel,
                new Color(0.018f, 0.026f, 0.032f, 0.99f)
            );

            DrawRoundedBorder(
                bezel,
                new Color(trim.r, trim.g, trim.b, 0.46f),
                2f
            );

            Color screenColor = awakened
                ? new Color(0.020f, 0.115f, 0.105f, 1f)
                : new Color(0.025f, 0.075f, 0.082f, 1f);

            DrawRoundedRect(content, screenColor);

            GUI.color = new Color(
                awakened ? 0.18f : 0.08f,
                awakened ? 0.86f : 0.48f,
                awakened ? 0.74f : 0.56f,
                0.10f
            );
            GUI.DrawTexture(
                new Rect(
                    content.x - 8f,
                    content.y - 8f,
                    content.width + 16f,
                    content.height + 16f
                ),
                softGlowTexture,
                ScaleMode.StretchToFill,
                alphaBlend: true
            );

            GUI.color = previousColor;
        }

        private void DrawOuterBands(
            Rect body,
            Rect content,
            Color bodyColor)
        {
            DrawRect(
                new Rect(
                    body.x,
                    body.y,
                    body.width,
                    Mathf.Max(0f, content.y - body.y)
                ),
                bodyColor
            );

            DrawRect(
                new Rect(
                    body.x,
                    content.yMax,
                    body.width,
                    Mathf.Max(
                        0f,
                        body.yMax - content.yMax
                    )
                ),
                bodyColor
            );

            DrawRect(
                new Rect(
                    body.x,
                    content.y,
                    Mathf.Max(0f, content.x - body.x),
                    content.height
                ),
                bodyColor
            );

            DrawRect(
                new Rect(
                    content.xMax,
                    content.y,
                    Mathf.Max(
                        0f,
                        body.xMax - content.xMax
                    ),
                    content.height
                ),
                bodyColor
            );
        }

        private void DrawDPad(
            Vector2 center,
            Color color)
        {
            Color shadow = new Color(0f, 0f, 0f, 0.44f);
            Color face = new Color(0.055f, 0.065f, 0.075f, 0.99f);
            Color edge = new Color(
                color.r,
                color.g,
                color.b,
                0.28f
            );

            DrawRoundedRect(
                new Rect(center.x - 11f + 3f, center.y - 29f + 4f, 22f, 58f),
                shadow
            );
            DrawRoundedRect(
                new Rect(center.x - 29f + 3f, center.y - 11f + 4f, 58f, 22f),
                shadow
            );

            DrawRoundedRect(
                new Rect(center.x - 11f, center.y - 29f, 22f, 58f),
                face
            );
            DrawRoundedRect(
                new Rect(center.x - 29f, center.y - 11f, 58f, 22f),
                face
            );
            DrawRoundedBorder(
                new Rect(center.x - 29f, center.y - 11f, 58f, 22f),
                edge,
                1f
            );

            DrawRoundedRect(
                new Rect(center.x - 7f, center.y - 7f, 14f, 14f),
                new Color(0.015f, 0.02f, 0.025f, 1f)
            );
        }

        private void DrawActionButtons(
            Vector2 center,
            Color color)
        {
            DrawRoundActionButton(
                new Rect(
                    center.x - 35f,
                    center.y - 3f,
                    28f,
                    28f
                ),
                "B",
                color
            );

            DrawRoundActionButton(
                new Rect(
                    center.x + 7f,
                    center.y - 25f,
                    28f,
                    28f
                ),
                "A",
                color
            );
        }

        private void DrawRoundActionButton(
            Rect rect,
            string label,
            Color color)
        {
            DrawRoundedRect(
                new Rect(
                    rect.x + 3f,
                    rect.y + 4f,
                    rect.width,
                    rect.height
                ),
                new Color(0f, 0f, 0f, 0.44f)
            );

            DrawRoundedRect(
                rect,
                new Color(
                    color.r,
                    color.g * 0.62f,
                    color.b * 0.72f,
                    0.96f
                )
            );

            DrawRoundedBorder(
                rect,
                new Color(1f, 1f, 1f, 0.14f),
                1f
            );

            GUI.Label(rect, label, smallLabel);
        }

        private void DrawSpeaker(
            Rect rect,
            Color color)
        {
            for (int index = 0; index < 6; index++)
            {
                DrawRoundedRect(
                    new Rect(
                        rect.x + index * 14f,
                        rect.y + index * 0.6f,
                        4f,
                        rect.height - index * 0.4f
                    ),
                    new Color(
                        color.r,
                        color.g,
                        color.b,
                        0.24f
                    )
                );
            }
        }

        private void DrawStartSelectButtons(
            Vector2 center,
            Color color)
        {
            DrawRoundedRect(
                new Rect(center.x - 42f, center.y - 3f, 32f, 8f),
                new Color(0.03f, 0.04f, 0.05f, 0.94f)
            );
            DrawRoundedRect(
                new Rect(center.x + 10f, center.y - 3f, 32f, 8f),
                new Color(0.03f, 0.04f, 0.05f, 0.94f)
            );
            DrawRoundedBorder(
                new Rect(center.x - 42f, center.y - 3f, 32f, 8f),
                new Color(color.r, color.g, color.b, 0.20f),
                1f
            );
            DrawRoundedBorder(
                new Rect(center.x + 10f, center.y - 3f, 32f, 8f),
                new Color(color.r, color.g, color.b, 0.20f),
                1f
            );
        }

        private void DrawScrew(Vector2 center, Color color)
        {
            DrawRoundedRect(
                new Rect(center.x - 3f, center.y - 3f, 6f, 6f),
                new Color(
                    color.r,
                    color.g,
                    color.b,
                    0.46f
                )
            );
            DrawRect(
                new Rect(center.x - 2f, center.y - 0.5f, 4f, 1f),
                new Color(0f, 0f, 0f, 0.42f)
            );
        }

        private void EnsureStyles()
        {
            if (smallLabel != null)
                return;

            smallLabel = new GUIStyle(GUI.skin.label)
            {
                alignment = TextAnchor.MiddleCenter,
                fontSize = 10,
                fontStyle = FontStyle.Bold
            };
            smallLabel.normal.textColor =
                new Color(0.93f, 0.93f, 0.82f, 0.94f);

            deviceLabel = new GUIStyle(smallLabel)
            {
                alignment = TextAnchor.MiddleLeft,
                fontSize = 12
            };

            screenLabel = new GUIStyle(smallLabel)
            {
                alignment = TextAnchor.MiddleRight,
                fontSize = 10,
                fontStyle = FontStyle.Bold
            };
            screenLabel.normal.textColor =
                new Color(0.72f, 0.90f, 0.82f, 0.74f);

            roundedStyle = new GUIStyle(GUI.skin.box)
            {
                border = new RectOffset(14, 14, 14, 14),
                normal =
                {
                    background = roundedRectTexture
                }
            };
        }

        private void EnsureResources()
        {
            if (roundedRectTexture == null)
                roundedRectTexture = CreateRoundedRectTexture(64, 12f);

            if (softGlowTexture == null)
                softGlowTexture = CreateRadialGlowTexture(64);

            if (scanlineTexture == null)
                scanlineTexture = CreateScanlineTexture(8);

            if (screenVignetteTexture == null)
                screenVignetteTexture = CreateVignetteTexture(64);
        }

        private void DrawRoundedRect(Rect rect, Color color)
        {
            if (rect.width <= 0f || rect.height <= 0f)
                return;

            Color previousColor = GUI.color;
            Color previousBackground = GUI.backgroundColor;

            if (rect.width < 28f || rect.height < 28f)
            {
                GUI.color = color;
                GUI.DrawTexture(
                    rect,
                    roundedRectTexture,
                    ScaleMode.StretchToFill,
                    alphaBlend: true
                );
            }
            else
            {
                GUI.color = Color.white;
                GUI.backgroundColor = color;
                GUI.Box(rect, GUIContent.none, roundedStyle);
            }

            GUI.backgroundColor = previousBackground;
            GUI.color = previousColor;
        }

        private void DrawRoundedBorder(
            Rect rect,
            Color color,
            float thickness)
        {
            float safeThickness = Mathf.Max(1f, thickness);
            DrawRect(
                new Rect(rect.x, rect.y, rect.width, safeThickness),
                color
            );
            DrawRect(
                new Rect(
                    rect.x,
                    rect.yMax - safeThickness,
                    rect.width,
                    safeThickness
                ),
                color
            );
            DrawRect(
                new Rect(rect.x, rect.y, safeThickness, rect.height),
                color
            );
            DrawRect(
                new Rect(
                    rect.xMax - safeThickness,
                    rect.y,
                    safeThickness,
                    rect.height
                ),
                color
            );
        }

        private static Texture2D CreateRoundedRectTexture(
            int size,
            float radius)
        {
            int safeSize = Mathf.Max(16, size);
            float safeRadius = Mathf.Clamp(
                radius,
                2f,
                safeSize * 0.48f
            );

            Texture2D texture = new Texture2D(
                safeSize,
                safeSize,
                TextureFormat.RGBA32,
                mipChain: false
            );
            texture.name = "B&D Memory Handheld Rounded Rect";
            texture.wrapMode = TextureWrapMode.Clamp;
            texture.filterMode = FilterMode.Bilinear;

            for (int y = 0; y < safeSize; y++)
            {
                for (int x = 0; x < safeSize; x++)
                {
                    float dx = Mathf.Max(
                        Mathf.Abs(x - (safeSize - 1) * 0.5f) -
                        ((safeSize - 1) * 0.5f - safeRadius),
                        0f
                    );
                    float dy = Mathf.Max(
                        Mathf.Abs(y - (safeSize - 1) * 0.5f) -
                        ((safeSize - 1) * 0.5f - safeRadius),
                        0f
                    );
                    float distance = Mathf.Sqrt(dx * dx + dy * dy);
                    float alpha = 1f - Mathf.SmoothStep(
                        safeRadius - 1.5f,
                        safeRadius + 0.5f,
                        distance
                    );
                    texture.SetPixel(
                        x,
                        y,
                        new Color(1f, 1f, 1f, alpha)
                    );
                }
            }

            texture.Apply(false, true);
            return texture;
        }

        private static Texture2D CreateRadialGlowTexture(int size)
        {
            int safeSize = Mathf.Max(16, size);
            Texture2D texture = new Texture2D(
                safeSize,
                safeSize,
                TextureFormat.RGBA32,
                mipChain: false
            );
            texture.name = "B&D Memory Handheld Glow";
            texture.wrapMode = TextureWrapMode.Clamp;
            texture.filterMode = FilterMode.Bilinear;

            Vector2 center = new Vector2(
                (safeSize - 1) * 0.5f,
                (safeSize - 1) * 0.5f
            );
            float radius = Mathf.Max(1f, safeSize * 0.5f);

            for (int y = 0; y < safeSize; y++)
            {
                for (int x = 0; x < safeSize; x++)
                {
                    float d =
                        Vector2.Distance(new Vector2(x, y), center) /
                        radius;
                    float alpha = Mathf.Pow(
                        Mathf.Clamp01(1f - d),
                        2.4f
                    );
                    texture.SetPixel(
                        x,
                        y,
                        new Color(1f, 1f, 1f, alpha)
                    );
                }
            }

            texture.Apply(false, true);
            return texture;
        }

        private static Texture2D CreateScanlineTexture(int size)
        {
            int safeSize = Mathf.Max(4, size);
            Texture2D texture = new Texture2D(
                safeSize,
                safeSize,
                TextureFormat.RGBA32,
                mipChain: false
            );
            texture.name = "B&D Memory Handheld Scanlines";
            texture.wrapMode = TextureWrapMode.Repeat;
            texture.filterMode = FilterMode.Point;

            for (int y = 0; y < safeSize; y++)
            {
                float alpha = y == safeSize - 1 ? 0.12f : 0f;
                for (int x = 0; x < safeSize; x++)
                {
                    texture.SetPixel(
                        x,
                        y,
                        new Color(0f, 0f, 0f, alpha)
                    );
                }
            }

            texture.Apply(false, true);
            return texture;
        }

        private static Texture2D CreateVignetteTexture(int size)
        {
            int safeSize = Mathf.Max(16, size);
            Texture2D texture = new Texture2D(
                safeSize,
                safeSize,
                TextureFormat.RGBA32,
                mipChain: false
            );
            texture.name = "B&D Memory Handheld Screen Vignette";
            texture.wrapMode = TextureWrapMode.Clamp;
            texture.filterMode = FilterMode.Bilinear;

            Vector2 center = new Vector2(
                (safeSize - 1) * 0.5f,
                (safeSize - 1) * 0.5f
            );
            float radius = Mathf.Max(1f, safeSize * 0.72f);

            for (int y = 0; y < safeSize; y++)
            {
                for (int x = 0; x < safeSize; x++)
                {
                    float d =
                        Vector2.Distance(new Vector2(x, y), center) /
                        radius;
                    float alpha =
                        Mathf.SmoothStep(0.55f, 1f, d) * 0.52f;
                    texture.SetPixel(
                        x,
                        y,
                        new Color(0f, 0f, 0f, alpha)
                    );
                }
            }

            texture.Apply(false, true);
            return texture;
        }

        private static void DestroyGeneratedTexture(
            ref Texture2D texture)
        {
            if (texture == null)
                return;

            Object.Destroy(texture);
            texture = null;
        }

        private void DrawBorder(
            Rect rect,
            Color color,
            float thickness)
        {
            DrawRect(
                new Rect(
                    rect.x,
                    rect.y,
                    rect.width,
                    thickness
                ),
                color
            );

            DrawRect(
                new Rect(
                    rect.x,
                    rect.yMax - thickness,
                    rect.width,
                    thickness
                ),
                color
            );

            DrawRect(
                new Rect(
                    rect.x,
                    rect.y,
                    thickness,
                    rect.height
                ),
                color
            );

            DrawRect(
                new Rect(
                    rect.xMax - thickness,
                    rect.y,
                    thickness,
                    rect.height
                ),
                color
            );
        }

        private void DrawRect(
            Rect rect,
            Color color)
        {
            if (rect.width <= 0f ||
                rect.height <= 0f)
            {
                return;
            }

            Color previous = GUI.color;
            GUI.color = color;
            GUI.DrawTexture(rect, pixel);
            GUI.color = previous;
        }
    }
}
