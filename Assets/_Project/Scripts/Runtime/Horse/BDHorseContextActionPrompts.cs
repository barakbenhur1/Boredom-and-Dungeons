using UnityEngine;
using UnityEngine.SceneManagement;

namespace BoredomAndDungeons
{
    [DefaultExecutionOrder(170)]
    [DisallowMultipleComponent]
    [RequireComponent(typeof(BDHorseController))]
    [RequireComponent(typeof(BDHorseHealth))]
    public sealed class BDHorseContextActionPrompts : MonoBehaviour
    {
        // BD UNIFIED HORSE CONTEXT PROMPTS V23R8
        [Header("Visibility")]
        [SerializeField] private float onFootPromptRange = 3.45f;
        [SerializeField] private float worldHeight = 4.20f;
        [SerializeField] private Vector2 screenOffset =
            new Vector2(0f, 4f);

        [Header("Layout")]
        [SerializeField] private float cardWidth = 104f;
        [SerializeField] private float cardHeight = 30f;
        [SerializeField] private float cardGap = 6f;
        [SerializeField] private float keycapWidth = 27f;
        [SerializeField] private float accentHeight = 2f;

        [Header("Style")]
        [SerializeField] private Color panelColor =
            new Color(0.035f, 0.050f, 0.060f, 0.84f);
        [SerializeField] private Color borderColor =
            new Color(0.75f, 0.84f, 0.86f, 0.22f);
        [SerializeField] private Color keycapColor =
            new Color(0.12f, 0.16f, 0.18f, 0.96f);
        [SerializeField] private Color textColor =
            new Color(0.95f, 0.98f, 1f, 1f);
        [SerializeField] private Color mountColor =
            new Color(0.30f, 0.92f, 1f, 1f);
        [SerializeField] private Color dismountColor =
            new Color(1f, 0.72f, 0.24f, 1f);
        [SerializeField] private Color healColor =
            new Color(0.34f, 1f, 0.52f, 1f);
        [SerializeField] private Color petColor =
            new Color(1f, 0.55f, 0.70f, 1f);

        private readonly PromptAction[] actions =
            new PromptAction[4];

        private BDHorseController horseController;
        private BDHorseHealth horseHealth;
        private BDHorseExhaustedFollowAndPetInteraction petInteraction;
        private Transform player;
        private Camera mainCamera;
        private GUIStyle keyStyle;
        private GUIStyle labelStyle;
        private GUIStyle symbolStyle;
        private int actionCount;
        private float nextResolveAt;

        private readonly struct PromptAction
        {
            public readonly string Key;
            public readonly string Symbol;
            public readonly string Label;
            public readonly Color Accent;

            public PromptAction(
                string key,
                string symbol,
                string label,
                Color accent)
            {
                Key = key;
                Symbol = symbol;
                Label = label;
                Accent = accent;
            }
        }

        private void Awake()
        {
            horseController = GetComponent<BDHorseController>();
            horseHealth = GetComponent<BDHorseHealth>();
            petInteraction =
                GetComponent<
                    BDHorseExhaustedFollowAndPetInteraction
                >();
        }

        private void LateUpdate()
        {
            if (Time.unscaledTime < nextResolveAt)
                return;

            nextResolveAt = Time.unscaledTime + 0.10f;

            if (player == null)
                player = horseController != null
                    ? horseController.Rider
                    : null;

            if (player == null)
                player = BDTargetFinder.FindPlayer();

            if (petInteraction == null)
            {
                petInteraction =
                    GetComponent<
                        BDHorseExhaustedFollowAndPetInteraction
                    >();
            }
        }

        private void OnGUI()
        {
            if (!BDGameplayUiVisibility.IsGameplayHudVisible ||
                !Application.isPlaying ||
                Event.current.type != EventType.Repaint)
            {
                return;
            }

            BuildActions();

            if (actionCount <= 0)
                return;

            Camera camera = ResolveCamera();
            if (camera == null)
                return;

            Vector3 screen = camera.WorldToScreenPoint(
                transform.position + Vector3.up * worldHeight
            );

            if (screen.z <= 0.05f)
                return;

            EnsureStyles();

            float totalWidth =
                actionCount * cardWidth +
                Mathf.Max(0, actionCount - 1) * cardGap;

            float startX =
                screen.x -
                totalWidth * 0.5f +
                screenOffset.x;

            float y =
                Screen.height -
                screen.y +
                screenOffset.y;

            startX = Mathf.Clamp(
                startX,
                8f,
                Mathf.Max(8f, Screen.width - totalWidth - 8f)
            );
            y = Mathf.Clamp(
                y,
                8f,
                Mathf.Max(8f, Screen.height - cardHeight - 8f)
            );

            GUI.depth = -900;

            for (int index = 0; index < actionCount; index++)
            {
                Rect rect = new Rect(
                    startX + index * (cardWidth + cardGap),
                    y,
                    cardWidth,
                    cardHeight
                );

                DrawAction(rect, actions[index]);
            }
        }

        private void BuildActions()
        {
            actionCount = 0;

            if (horseController == null ||
                horseHealth == null ||
                player == null ||
                BDRunPresentationCoordinator.InputLocked ||
                BDMountedRunIntro.IsGameplayInputLocked)
            {
                return;
            }

            // BD HORSE PROMPT STATE MATRIX V23R9
            // On foot: Mount, conditional Heal, and Pet each show a prompt.
            // Mounted + stationary: show Dismount only; Pet remains usable
            // through its bound key but intentionally has no prompt.
            // Mounted + moving: Dismount remains usable but no prompt is shown.
            // Healing is strictly an on-foot interaction.
            if (horseController.IsMounted)
            {
                if (horseController.IsMountedStationary)
                {
                    AddAction(
                        "E",
                        "↓",
                        "DISMOUNT",
                        dismountColor
                    );
                }

                return;
            }

            Vector3 toPlayer =
                player.position - transform.position;
            toPlayer.y = 0f;

            if (toPlayer.sqrMagnitude >
                onFootPromptRange * onFootPromptRange)
            {
                return;
            }

            if (horseController.CanOfferMountAction)
            {
                AddAction(
                    "E",
                    "↑",
                    "MOUNT",
                    mountColor
                );
            }

            if (horseController.NeedsHealing)
            {
                AddAction(
                    "F",
                    "+",
                    "HOLD HEAL",
                    healColor
                );
            }

            if (petInteraction != null &&
                (petInteraction.IsPetAvailable ||
                 petInteraction.IsPetInteractionActive ||
                 petInteraction.PetHoldProgress01 > 0f))
            {
                string label =
                    petInteraction.IsPetInteractionActive
                        ? "PETTING"
                        : petInteraction.PetHoldProgress01 > 0f
                            ? "HOLD PET"
                            : "PET";

                AddAction(
                    ResolvePetKey(),
                    "♥",
                    label,
                    petColor
                );
            }
        }

        private string ResolvePetKey()
        {
            if (petInteraction == null)
                return "TAB";

            string value =
                petInteraction.DesktopPetKey.ToString();

            return string.IsNullOrWhiteSpace(value)
                ? "TAB"
                : value.ToUpperInvariant();
        }

        private void AddAction(
            string key,
            string symbol,
            string label,
            Color accent)
        {
            if (actionCount >= actions.Length)
                return;

            actions[actionCount++] = new PromptAction(
                key,
                symbol,
                label,
                accent
            );
        }

        private Camera ResolveCamera()
        {
            if (mainCamera == null ||
                !mainCamera.isActiveAndEnabled)
            {
                mainCamera = Camera.main;
            }

            return mainCamera;
        }

        private void EnsureStyles()
        {
            if (keyStyle != null)
                return;

            keyStyle = new GUIStyle(GUI.skin.label)
            {
                alignment = TextAnchor.MiddleCenter,
                fontSize = 11,
                fontStyle = FontStyle.Bold,
                clipping = TextClipping.Clip
            };
            keyStyle.normal.textColor = textColor;

            symbolStyle = new GUIStyle(GUI.skin.label)
            {
                alignment = TextAnchor.MiddleCenter,
                fontSize = 15,
                fontStyle = FontStyle.Bold,
                clipping = TextClipping.Clip
            };
            symbolStyle.normal.textColor = textColor;

            labelStyle = new GUIStyle(GUI.skin.label)
            {
                alignment = TextAnchor.MiddleLeft,
                fontSize = 11,
                fontStyle = FontStyle.Bold,
                clipping = TextClipping.Clip
            };
            labelStyle.normal.textColor = textColor;
        }

        private void DrawAction(
            Rect rect,
            PromptAction action)
        {
            Color previous = GUI.color;
            Texture2D texture = Texture2D.whiteTexture;

            GUI.color = panelColor;
            GUI.DrawTexture(rect, texture);

            GUI.color = borderColor;
            DrawBorder(rect, 1f, texture);

            GUI.color = action.Accent;
            GUI.DrawTexture(
                new Rect(
                    rect.x,
                    rect.y,
                    rect.width,
                    Mathf.Max(1f, accentHeight)
                ),
                texture
            );

            Rect keyRect = new Rect(
                rect.x + 5f,
                rect.y + 5f,
                keycapWidth,
                rect.height - 10f
            );

            GUI.color = keycapColor;
            GUI.DrawTexture(keyRect, texture);

            GUI.color = previous;
            GUI.Label(keyRect, action.Key, keyStyle);

            Rect symbolRect = new Rect(
                keyRect.xMax + 3f,
                rect.y + 4f,
                18f,
                rect.height - 8f
            );

            symbolStyle.normal.textColor = action.Accent;
            GUI.Label(
                symbolRect,
                action.Symbol,
                symbolStyle
            );

            Rect labelRect = new Rect(
                symbolRect.xMax + 1f,
                rect.y + 4f,
                rect.xMax - symbolRect.xMax - 5f,
                rect.height - 8f
            );

            labelStyle.normal.textColor = textColor;
            GUI.Label(
                labelRect,
                action.Label,
                labelStyle
            );

            GUI.color = previous;
        }

        private static void DrawBorder(
            Rect rect,
            float thickness,
            Texture texture)
        {
            GUI.DrawTexture(
                new Rect(
                    rect.x,
                    rect.y,
                    rect.width,
                    thickness
                ),
                texture
            );
            GUI.DrawTexture(
                new Rect(
                    rect.x,
                    rect.yMax - thickness,
                    rect.width,
                    thickness
                ),
                texture
            );
            GUI.DrawTexture(
                new Rect(
                    rect.x,
                    rect.y,
                    thickness,
                    rect.height
                ),
                texture
            );
            GUI.DrawTexture(
                new Rect(
                    rect.xMax - thickness,
                    rect.y,
                    thickness,
                    rect.height
                ),
                texture
            );
        }
    }

    public static class BDHorseContextActionPromptsInstaller
    {
        [RuntimeInitializeOnLoadMethod(
            RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void InstallAfterSceneLoad()
        {
            Install();

            SceneManager.sceneLoaded -= HandleSceneLoaded;
            SceneManager.sceneLoaded += HandleSceneLoaded;
        }

        private static void HandleSceneLoaded(
            Scene scene,
            LoadSceneMode mode)
        {
            Install();
        }

        private static void Install()
        {
            BDHorseController[] horses =
                Object.FindObjectsByType<BDHorseController>(
                    FindObjectsSortMode.None
                );

            for (int index = 0; index < horses.Length; index++)
            {
                BDHorseController horse = horses[index];

                if (horse != null &&
                    horse.GetComponent<
                        BDHorseContextActionPrompts
                    >() == null)
                {
                    horse.gameObject.AddComponent<
                        BDHorseContextActionPrompts
                    >();
                }
            }
        }
    }
}
