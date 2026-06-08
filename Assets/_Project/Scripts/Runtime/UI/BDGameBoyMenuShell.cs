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
        private Texture2D pixel;

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
        }

        private void OnDestroy()
        {
            if (instance == this)
                instance = null;
        }

        public static void DrawIntegrated(
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

            instance.DrawIntegratedInternal(
                content,
                virtualWidth,
                virtualHeight
            );
        }

        private void DrawIntegratedInternal(
            Rect content,
            float virtualWidth,
            float virtualHeight)
        {
            EnsureStyles();

            Color previousColor = GUI.color;

            Rect body = new Rect(
                content.x - 52f,
                content.y - 42f,
                content.width + 104f,
                content.height + 112f
            );

            Rect bezel = new Rect(
                content.x - 13f,
                content.y - 13f,
                content.width + 26f,
                content.height + 26f
            );

            bool awakened =
                BDGameProgress.MotherDefeated;

            Color bodyColor = awakened
                ? new Color(0.055f, 0.16f, 0.18f, 0.985f)
                : new Color(0.12f, 0.14f, 0.16f, 0.985f);

            Color trim = awakened
                ? new Color(0.22f, 0.95f, 0.92f, 0.95f)
                : new Color(0.88f, 0.66f, 0.24f, 0.90f);

            DrawOuterBands(
                body,
                content,
                bodyColor
            );

            DrawBorder(
                body,
                trim,
                3f
            );

            DrawBorder(
                bezel,
                new Color(
                    trim.r,
                    trim.g,
                    trim.b,
                    0.62f
                ),
                2f
            );

            DrawRect(
                new Rect(
                    body.x + 24f,
                    body.y + 20f,
                    8f,
                    8f
                ),
                awakened
                    ? trim
                    : new Color(
                        0.55f,
                        0.95f,
                        0.42f,
                        1f
                    )
            );

            GUI.Label(
                new Rect(
                    body.x + 38f,
                    body.y + 10f,
                    260f,
                    26f
                ),
                awakened
                    ? "B&D AWAKENED POCKET"
                    : "B&D POCKET ADVENTURE",
                deviceLabel
            );

            float controlsY =
                body.yMax - 62f;

            DrawDPad(
                new Vector2(
                    body.x + 84f,
                    controlsY + 22f
                ),
                trim
            );

            DrawActionButtons(
                new Vector2(
                    body.xMax - 104f,
                    controlsY + 22f
                ),
                trim
            );

            DrawSpeaker(
                new Rect(
                    body.center.x - 42f,
                    controlsY + 8f,
                    84f,
                    34f
                ),
                trim
            );

            GUI.Label(
                new Rect(
                    body.center.x - 125f,
                    body.yMax - 26f,
                    250f,
                    18f
                ),
                awakened
                    ? "TRUE VICTORY LINK ACTIVE"
                    : "MEMORY CARTRIDGE READY",
                smallLabel
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
            DrawRect(
                new Rect(
                    center.x - 10f,
                    center.y - 28f,
                    20f,
                    56f
                ),
                new Color(
                    color.r,
                    color.g,
                    color.b,
                    0.62f
                )
            );

            DrawRect(
                new Rect(
                    center.x - 28f,
                    center.y - 10f,
                    56f,
                    20f
                ),
                new Color(
                    color.r,
                    color.g,
                    color.b,
                    0.62f
                )
            );

            DrawRect(
                new Rect(
                    center.x - 8f,
                    center.y - 8f,
                    16f,
                    16f
                ),
                new Color(
                    0.02f,
                    0.03f,
                    0.04f,
                    0.95f
                )
            );
        }

        private void DrawActionButtons(
            Vector2 center,
            Color color)
        {
            DrawRect(
                new Rect(
                    center.x - 32f,
                    center.y - 6f,
                    24f,
                    24f
                ),
                new Color(
                    color.r,
                    color.g * 0.72f,
                    color.b * 0.72f,
                    0.85f
                )
            );

            DrawRect(
                new Rect(
                    center.x + 8f,
                    center.y - 24f,
                    24f,
                    24f
                ),
                new Color(
                    color.r,
                    color.g * 0.72f,
                    color.b * 0.72f,
                    0.85f
                )
            );

            GUI.Label(
                new Rect(
                    center.x - 32f,
                    center.y - 6f,
                    24f,
                    24f
                ),
                "B",
                smallLabel
            );

            GUI.Label(
                new Rect(
                    center.x + 8f,
                    center.y - 24f,
                    24f,
                    24f
                ),
                "A",
                smallLabel
            );
        }

        private void DrawSpeaker(
            Rect rect,
            Color color)
        {
            for (int index = 0; index < 5; index++)
            {
                DrawRect(
                    new Rect(
                        rect.x + index * 17f,
                        rect.y + index * 2f,
                        4f,
                        rect.height - index * 2f
                    ),
                    new Color(
                        color.r,
                        color.g,
                        color.b,
                        0.34f
                    )
                );
            }
        }

        private void EnsureStyles()
        {
            if (smallLabel != null)
                return;

            smallLabel = new GUIStyle(
                GUI.skin.label)
            {
                alignment =
                    TextAnchor.MiddleCenter,
                fontSize = 10,
                fontStyle = FontStyle.Bold
            };
            smallLabel.normal.textColor =
                new Color(
                    0.92f,
                    0.90f,
                    0.78f,
                    0.90f
                );

            deviceLabel =
                new GUIStyle(smallLabel)
                {
                    alignment =
                        TextAnchor.MiddleLeft,
                    fontSize = 12
                };
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
