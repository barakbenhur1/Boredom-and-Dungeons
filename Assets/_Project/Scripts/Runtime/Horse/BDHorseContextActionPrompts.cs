using UnityEngine;
using UnityEngine.SceneManagement;

namespace BoredomAndDungeons
{
    /// <summary>
    /// Presents context-sensitive horse actions in a fixed, non-world-space HUD
    /// strip. Gameplay input and state remain owned by the horse systems.
    /// </summary>
    [DefaultExecutionOrder(170)]
    [DisallowMultipleComponent]
    [RequireComponent(typeof(BDHorseController))]
    [RequireComponent(typeof(BDHorseHealth))]
    public sealed class BDHorseContextActionPrompts : MonoBehaviour
    {
        // BD HORSE BOTTOM CONTEXT STRIP V2
        // BD UNIFIED HORSE CONTEXT PROMPTS V23R8
        [Header("Visibility")]
        [SerializeField, Min(0.5f)]
        private float onFootPromptRange = 3.45f;

        [Header("Bottom Context Strip")]
        [SerializeField, Min(96f)]
        private float cardWidth = 132f;

        [SerializeField, Min(26f)]
        private float cardHeight = 34f;

        [SerializeField, Min(0f)]
        private float cardGap = 8f;

        [SerializeField, Min(20f)]
        private float keycapWidth = 31f;

        [SerializeField, Min(0f)]
        private float bottomMargin = 38f;

        [SerializeField, Min(0.05f)]
        private float fadeInSeconds = 0.14f;

        [SerializeField, Min(0.05f)]
        private float fadeOutSeconds = 0.20f;

        [Header("Style")]
        [SerializeField] private Color panelColor =
            new Color(0.035f, 0.050f, 0.060f, 0.90f);

        [SerializeField] private Color borderColor =
            new Color(0.75f, 0.84f, 0.86f, 0.24f);

        [SerializeField] private Color keycapColor =
            new Color(0.12f, 0.16f, 0.18f, 0.98f);

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
        private GUIStyle keyStyle;
        private GUIStyle labelStyle;
        private int actionCount;
        private int lastVisibleActionCount;
        private float promptAlpha;
        private float nextResolveAt;

        private readonly struct PromptAction
        {
            public readonly string Key;
            public readonly string Label;
            public readonly Color Accent;

            public PromptAction(
                string key,
                string label,
                Color accent)
            {
                Key = key;
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
                    BDHorseExhaustedFollowAndPetInteraction>();
        }

        private void LateUpdate()
        {
            if (Time.unscaledTime >= nextResolveAt)
            {
                nextResolveAt = Time.unscaledTime + 0.10f;

                if (player == null)
                {
                    player = horseController != null
                        ? horseController.Rider
                        : null;
                }

                if (player == null)
                    player = BDTargetFinder.FindPlayer();

                if (petInteraction == null)
                {
                    petInteraction =
                        GetComponent<
                            BDHorseExhaustedFollowAndPetInteraction>();
                }
            }

            // Fade state is updated once per frame in LateUpdate, never from
            // the event-driven OnGUI pass.
            BuildActions();

            bool globallyVisible =
                BDGameplayUiVisibility.IsGameplayHudVisible;
            bool hasActions = globallyVisible && actionCount > 0;
            float duration = hasActions
                ? Mathf.Max(0.05f, fadeInSeconds)
                : Mathf.Max(0.05f, fadeOutSeconds);

            promptAlpha = Mathf.MoveTowards(
                promptAlpha,
                hasActions ? 1f : 0f,
                Time.unscaledDeltaTime / duration
            );

            if (hasActions)
                lastVisibleActionCount = actionCount;
            else if (promptAlpha <= 0.001f)
                lastVisibleActionCount = 0;
        }

        private void OnGUI()
        {
            if (!Application.isPlaying ||
                Event.current.type != EventType.Repaint)
            {
                return;
            }

            bool hasActions =
                BDGameplayUiVisibility.IsGameplayHudVisible &&
                actionCount > 0;
            int drawCount = hasActions
                ? actionCount
                : lastVisibleActionCount;

            if (drawCount <= 0 || promptAlpha <= 0.001f)
                return;

            EnsureStyles();

            float totalWidth =
                drawCount * cardWidth +
                Mathf.Max(0, drawCount - 1) * cardGap;

            float startX =
                Mathf.Clamp(
                    (Screen.width - totalWidth) * 0.5f,
                    8f,
                    Mathf.Max(8f, Screen.width - totalWidth - 8f)
                );

            float y = Mathf.Clamp(
                Screen.height -
                Mathf.Max(0f, bottomMargin) -
                cardHeight,
                8f,
                Mathf.Max(8f, Screen.height - cardHeight - 8f)
            );

            float easedAlpha = EaseOutCubic(promptAlpha);
            float verticalOffset = (1f - easedAlpha) * 10f;
            Color previousColor = GUI.color;
            GUI.color = new Color(
                previousColor.r,
                previousColor.g,
                previousColor.b,
                previousColor.a * easedAlpha
            );
            GUI.depth = -900;

            for (int index = 0; index < drawCount; index++)
            {
                Rect rect = new Rect(
                    startX + index * (cardWidth + cardGap),
                    y + verticalOffset,
                    cardWidth,
                    cardHeight
                );
                DrawAction(rect, actions[index]);
            }

            GUI.color = previousColor;

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
            // Healing is strictly an on-foot interaction.
            // Horse actions are intentionally presented in a fixed bottom HUD
            // strip. No icon, label, health bar, or interaction card is drawn
            // above the horse model.
            if (horseController.IsMounted)
            {
                if (horseController.IsMountedStationary)
                {
                    AddAction(
                        "E",
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
                    "MOUNT",
                    mountColor
                );
            }

            if (horseController.NeedsHealing)
            {
                AddAction(
                    "F",
                    horseController.IsHealing
                        ? "HEALING"
                        : "HOLD HEAL",
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
            string label,
            Color accent)
        {
            if (actionCount >= actions.Length)
                return;

            actions[actionCount++] = new PromptAction(
                key,
                label,
                accent
            );
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
            Texture2D texture = Texture2D.whiteTexture;
            Color inherited = GUI.color;

            GUI.color = Multiply(inherited, panelColor);
            GUI.DrawTexture(rect, texture);

            GUI.color = Multiply(inherited, borderColor);
            DrawBorder(rect, 1f, texture);

            GUI.color = Multiply(inherited, action.Accent);
            GUI.DrawTexture(
                new Rect(rect.x, rect.y, rect.width, 2f),
                texture
            );

            Rect keyRect = new Rect(
                rect.x + 6f,
                rect.y + 5f,
                keycapWidth,
                rect.height - 10f
            );

            GUI.color = Multiply(inherited, keycapColor);
            GUI.DrawTexture(keyRect, texture);

            GUI.color = inherited;
            GUI.Label(keyRect, action.Key, keyStyle);

            Rect labelRect = new Rect(
                keyRect.xMax + 8f,
                rect.y + 4f,
                rect.xMax - keyRect.xMax - 13f,
                rect.height - 8f
            );
            labelStyle.normal.textColor = textColor;
            GUI.Label(labelRect, action.Label, labelStyle);

            GUI.color = inherited;
        }

        private static Color Multiply(Color left, Color right)
        {
            return new Color(
                left.r * right.r,
                left.g * right.g,
                left.b * right.b,
                left.a * right.a
            );
        }

        private static float EaseOutCubic(float value)
        {
            float t = Mathf.Clamp01(value);
            float inverse = 1f - t;
            return 1f - inverse * inverse * inverse;
        }

        private static void DrawBorder(
            Rect rect,
            float thickness,
            Texture texture)
        {
            GUI.DrawTexture(
                new Rect(rect.x, rect.y, rect.width, thickness),
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
                new Rect(rect.x, rect.y, thickness, rect.height),
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
                if (horse == null)
                    continue;

                if (horse.GetComponent<
                        BDHorseContextActionPrompts>() == null)
                {
                    horse.gameObject.AddComponent<
                        BDHorseContextActionPrompts>();
                }
            }
        }
    }
}
