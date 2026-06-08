using UnityEngine;

namespace BoredomAndDungeons
{
    [DisallowMultipleComponent]
    public sealed class BDGameHud : MonoBehaviour
    {
        [SerializeField] private BDHealth playerHealth;
        [SerializeField] private BDHorseHealth horseHealth;
        [SerializeField] private BDPlayerCombat playerCombat;
        [SerializeField] private bool showHud = true;

        private GUIStyle labelStyle;
        private GUIStyle smallStyle;
        private GUIStyle titleStyle;
        private GUIStyle centerStyle;
        private GUIStyle ammoNumberStyle;
        private GUIStyle ammoSmallStyle;
        private Texture2D whiteTexture;

        private void Awake()
        {
            whiteTexture = Texture2D.whiteTexture;
        }

        private void Update()
        {
            Transform player = null;

            if (playerHealth == null || playerCombat == null)
            {
                player = BDTargetFinder.FindPlayer();

                if (playerHealth == null && player != null)
                    playerHealth = player.GetComponent<BDHealth>();

                if (playerCombat == null && player != null)
                    playerCombat = player.GetComponent<BDPlayerCombat>();
            }

            if (horseHealth == null)
                horseHealth = FindFirstObjectByType<BDHorseHealth>();
        }

        private void OnGUI()
        {
            if (!showHud ||
                !BDGameplayUiVisibility.IsGameplayHudVisible)
            {
                return;
            }

            EnsureStyles();

            DrawStatusPanel();
            DrawPlayerBar();
            DrawHorseBar();
            DrawAmmoWidget();
            DrawCombatEventMessage();
            DrawDeathMessages();
        }

        private void DrawStatusPanel()
        {
            GUI.Box(new Rect(12, Screen.height - 120, 390, 104), "Status");
        }

        private void DrawPlayerBar()
        {
            float hp = playerHealth != null ? playerHealth.CurrentHealth : 0f;
            float max = playerHealth != null ? playerHealth.MaxHealth : 1f;
            float ratio = Mathf.Clamp01(hp / Mathf.Max(1f, max));

            GUI.Label(new Rect(24, Screen.height - 84, 140, 22), "Player", labelStyle);
            DrawBar(new Rect(92, Screen.height - 82, 180, 18), ratio, new Color(0.85f, 0.12f, 0.10f, 1f), new Color(0.16f, 0.04f, 0.04f, 1f));
            GUI.Label(new Rect(282, Screen.height - 86, 78, 22), $"{hp:0}/{max:0}", labelStyle);
        }

        private void DrawHorseBar()
        {
            float hp = horseHealth != null ? horseHealth.CurrentHealth : 0f;
            float max = horseHealth != null ? horseHealth.MaxHealth : 1f;
            float ratio = Mathf.Clamp01(hp / Mathf.Max(1f, max));
            bool fainted = horseHealth != null && horseHealth.IsFainted;

            string horseState = "No horse";
            if (horseHealth != null)
                horseState = fainted ? "Fainted - Hold F to revive" : ratio < 0.98f ? "Injured - Hold F to heal" : "Ready";

            GUI.Label(new Rect(24, Screen.height - 50, 140, 22), "Horse", labelStyle);
            DrawBar(new Rect(92, Screen.height - 48, 180, 18), ratio, fainted ? new Color(0.45f, 0.45f, 0.45f, 1f) : new Color(0.10f, 0.65f, 0.95f, 1f), new Color(0.03f, 0.08f, 0.11f, 1f));
            GUI.Label(new Rect(282, Screen.height - 52, 78, 22), $"{hp:0}/{max:0}", labelStyle);
            GUI.Label(new Rect(92, Screen.height - 26, 240, 22), horseState, smallStyle);
        }

        private void DrawAmmoWidget()
        {
            if (playerCombat == null)
                return;

            int ammo = playerCombat.RangedAmmo;
            int maxAmmo = playerCombat.RangedMagazineSize;
            bool reloading = playerCombat.IsReloading;
            float reloadProgress = playerCombat.RangedReloadProgress01;
            float remaining = playerCombat.RangedReloadRemaining;

            // BD EXPANDED AMMO UI FIX
            int visibleAmmoCapacity = Mathf.Clamp(maxAmmo, 1, 6);
            float panelWidth = Mathf.Max(258f, 212f + visibleAmmoCapacity * 28f);
            float panelHeight = 112f;
            float panelMarginRight = 18f;
            float panelMarginTop = 18f;
            Rect panel = new Rect(Screen.width - panelWidth - panelMarginRight, panelMarginTop, panelWidth, panelHeight);

            Color old = GUI.color;
            GUI.color = new Color(0.025f, 0.035f, 0.045f, 0.82f);
            GUI.DrawTexture(panel, whiteTexture);
            GUI.color = reloading ? new Color(1.0f, 0.70f, 0.18f, 0.95f) : new Color(0.36f, 0.95f, 1f, 0.95f);
            GUI.Box(panel, GUIContent.none);
            GUI.color = old;

            GUI.Label(new Rect(panel.x + 16f, panel.y + 10f, 118f, 20f), "RANGED", ammoSmallStyle);
            GUI.Label(new Rect(panel.x + 16f, panel.y + 30f, 118f, 44f), $"{ammo}/{maxAmmo}", ammoNumberStyle);

            Rect bar = new Rect(panel.x + 16f, panel.y + 78f, panel.width - 32f, 10f);
            float readyRatio = reloading ? reloadProgress : 1f;
            Color fill = reloading ? new Color(1.0f, 0.72f, 0.16f, 1f) : new Color(0.35f, 0.95f, 1f, 1f);
            DrawBar(bar, readyRatio, fill, new Color(0.035f, 0.07f, 0.085f, 1f));

            DrawAmmoPips(
                new Rect(
                    panel.x + 96f,
                    panel.y + 25f,
                    panel.width - 158f,
                    34f
                ),
                ammo,
                maxAmmo,
                reloading
            );
            DrawReloadRing(new Vector2(panel.x + panel.width - 48f, panel.y + 42f), 24f, reloadProgress, reloading);

            string status = reloading ? $"RELOAD {remaining:0.0}s" : "READY";
            GUI.Label(new Rect(panel.x + 126f, panel.y + 55f, 104f, 18f), status, ammoSmallStyle);
        }

        private void DrawAmmoPips(
            Rect rect,
            int ammo,
            int maxAmmo,
            bool reloading)
        {
            maxAmmo = Mathf.Clamp(maxAmmo, 1, 6);
            ammo = Mathf.Clamp(ammo, 0, maxAmmo);

            float gap = 6f;
            float size =
                Mathf.Min(
                    20f,
                    (rect.width - gap * (maxAmmo - 1)) / maxAmmo
                );

            float totalWidth =
                maxAmmo * size + (maxAmmo - 1) * gap;

            float startX = rect.x + Mathf.Max(0f, (rect.width - totalWidth) * 0.5f);
            float y = rect.y + (rect.height - size) * 0.5f;

            for (int i = 0; i < maxAmmo; i++)
            {
                Rect pip =
                    new Rect(
                        startX + i * (size + gap),
                        y,
                        size,
                        size
                    );

                bool filled = i < ammo;

                Color background =
                    new Color(0.02f, 0.035f, 0.04f, 1f);

                Color fill = filled
                    ? new Color(0.35f, 0.95f, 1f, 1f)
                    : new Color(0.045f, 0.09f, 0.105f, 1f);

                if (!filled && reloading)
                {
                    float pulse =
                        0.55f +
                        Mathf.Sin(
                            Time.time * 9f + i * 0.75f
                        ) * 0.22f;

                    fill =
                        new Color(
                            1f,
                            0.68f,
                            0.16f,
                            Mathf.Clamp01(pulse)
                        );
                }

                DrawBar(pip, 1f, background, background);
                DrawAmmoBulletIcon(pip, fill, filled);
            }
        }

        private void DrawAmmoBulletIcon(
            Rect rect,
            Color color,
            bool filled)
        {
            Color old = GUI.color;

            float capHeight = rect.height * 0.32f;
            Rect body =
                new Rect(
                    rect.x + rect.width * 0.22f,
                    rect.y + capHeight * 0.65f,
                    rect.width * 0.56f,
                    rect.height - capHeight * 0.65f
                );

            Rect cap =
                new Rect(
                    rect.x + rect.width * 0.32f,
                    rect.y,
                    rect.width * 0.36f,
                    capHeight
                );

            GUI.color = color;
            GUI.DrawTexture(body, whiteTexture);
            GUI.DrawTexture(cap, whiteTexture);

            GUI.color = filled
                ? new Color(1f, 1f, 1f, 0.55f)
                : new Color(1f, 1f, 1f, 0.18f);

            GUI.Box(body, GUIContent.none);
            GUI.Box(cap, GUIContent.none);
            GUI.color = old;
        }

        private void DrawReloadRing(Vector2 center, float radius, float progress, bool reloading)
        {
            int segments = 28;
            float segmentWidth = 3.2f;
            float segmentHeight = 8.8f;

            for (int i = 0; i < segments; i++)
            {
                float t = i / (float)segments;
                bool active = !reloading || t <= Mathf.Clamp01(progress);
                float angle = t * 360f - 90f;
                float pulse = reloading ? 0.72f + Mathf.Sin(Time.time * 9f + i * 0.33f) * 0.20f : 1f;

                Color old = GUI.color;
                GUI.color = active
                    ? (reloading ? new Color(1f, 0.72f, 0.16f, pulse) : new Color(0.35f, 0.95f, 1f, 0.95f))
                    : new Color(0.05f, 0.09f, 0.10f, 0.85f);

                Matrix4x4 oldMatrix = GUI.matrix;
                GUIUtility.RotateAroundPivot(angle, center);

                Rect segment = new Rect(center.x - segmentWidth * 0.5f, center.y - radius, segmentWidth, segmentHeight);
                GUI.DrawTexture(segment, whiteTexture);

                GUI.matrix = oldMatrix;
                GUI.color = old;
            }
        }

        private void DrawCombatEventMessage()
        {
            if (!BDCombatEventBus.HasActiveMessage)
                return;

            float progress = BDCombatEventBus.MessageProgress01;
            float fadeIn = Mathf.Clamp01(progress / 0.18f);
            float fadeOut = Mathf.Clamp01((1f - progress) / 0.22f);
            float alpha = Mathf.Min(fadeIn, fadeOut);

            Color old = GUI.color;
            GUI.color = new Color(1f, 1f, 1f, alpha);

            DrawCenteredMessage(BDCombatEventBus.ActiveTitle, BDCombatEventBus.ActiveSubtitle);

            GUI.color = old;
        }

        private void DrawDeathMessages()
        {
            if (playerHealth != null && playerHealth.IsDead)
            {
                DrawCenteredMessage("YOU DIED", "Movement and combat disabled");
            }
            else if (horseHealth != null && horseHealth.IsFainted)
            {
                DrawCenteredMessage("HORSE FAINTED", "Stand near it and hold F to revive");
            }
        }

        private void DrawCenteredMessage(string title, string subtitle)
        {
            Rect titleRect = new Rect(0, Screen.height * 0.34f, Screen.width, 42);
            Rect subtitleRect = new Rect(0, Screen.height * 0.34f + 44, Screen.width, 30);

            GUI.Label(titleRect, title, centerStyle);
            GUI.Label(subtitleRect, subtitle, labelStyle);
        }

        private void DrawBar(Rect rect, float ratio, Color fillColor, Color backgroundColor)
        {
            Color old = GUI.color;

            GUI.color = backgroundColor;
            GUI.DrawTexture(rect, whiteTexture);

            GUI.color = fillColor;
            GUI.DrawTexture(new Rect(rect.x, rect.y, rect.width * Mathf.Clamp01(ratio), rect.height), whiteTexture);

            GUI.color = Color.white;
            GUI.Box(rect, GUIContent.none);

            GUI.color = old;
        }

        private void EnsureStyles()
        {
            if (labelStyle != null)
                return;

            labelStyle = new GUIStyle(GUI.skin.label)
            {
                fontSize = 14,
                alignment = TextAnchor.MiddleLeft
            };
            labelStyle.normal.textColor = Color.white;

            smallStyle = new GUIStyle(GUI.skin.label)
            {
                fontSize = 12,
                alignment = TextAnchor.MiddleLeft
            };
            smallStyle.normal.textColor = new Color(0.85f, 0.92f, 0.96f, 1f);

            titleStyle = new GUIStyle(GUI.skin.label)
            {
                fontSize = 18,
                fontStyle = FontStyle.Bold,
                alignment = TextAnchor.MiddleLeft
            };
            titleStyle.normal.textColor = Color.white;

            centerStyle = new GUIStyle(GUI.skin.label)
            {
                fontSize = 38,
                fontStyle = FontStyle.Bold,
                alignment = TextAnchor.MiddleCenter
            };
            centerStyle.normal.textColor = Color.white;

            ammoNumberStyle = new GUIStyle(GUI.skin.label)
            {
                fontSize = 32,
                fontStyle = FontStyle.Bold,
                alignment = TextAnchor.MiddleLeft
            };
            ammoNumberStyle.normal.textColor = new Color(0.84f, 0.98f, 1f, 1f);

            ammoSmallStyle = new GUIStyle(GUI.skin.label)
            {
                fontSize = 11,
                fontStyle = FontStyle.Bold,
                alignment = TextAnchor.MiddleLeft
            };
            ammoSmallStyle.normal.textColor = new Color(0.70f, 0.88f, 0.95f, 1f);
        }
    }
}
