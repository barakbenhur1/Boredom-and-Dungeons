using System.Collections.Generic;
using UnityEngine;

namespace BoredomAndDungeons
{
    [DisallowMultipleComponent]
    public sealed class BDBossHealthHud : MonoBehaviour
    {
        [SerializeField] private BDBossEncounterController encounter;
        [SerializeField] private BDBossHealthGroup healthGroup;
        [SerializeField] private string encounterTitle = "BOSS";
        [SerializeField] private bool showOnlyDuringEncounter = true;
        [SerializeField] private Vector2 panelSize = new Vector2(720f, 128f);
        [SerializeField] private float topMargin = 24f;
        [SerializeField] private float barHeight = 18f;
        [SerializeField] private float barSpacing = 8f;

        private GUIStyle titleStyle;
        private GUIStyle labelStyle;
        private GUIStyle stateStyle;
        private Texture2D whiteTexture;

        private void Awake()
        {
            if (encounter == null)
                encounter = GetComponentInParent<BDBossEncounterController>();

            if (healthGroup == null)
                healthGroup = GetComponentInParent<BDBossHealthGroup>();
        }

        private void OnDestroy()
        {
            if (whiteTexture != null)
                Destroy(whiteTexture);
        }

        private void OnGUI()
        {
            if (healthGroup == null)
                return;

            if (showOnlyDuringEncounter && encounter != null)
            {
                if (encounter.State == BDBossEncounterState.Dormant ||
                    encounter.State == BDBossEncounterState.Completed ||
                    encounter.State == BDBossEncounterState.Failed)
                    return;
            }

            EnsureStyles();

            IReadOnlyList<BDBossHealthChannel> channels = healthGroup.Channels;
            if (channels == null || channels.Count == 0)
                return;

            float width = Mathf.Min(panelSize.x, Screen.width - 32f);
            float requiredHeight = 50f + channels.Count * (barHeight + barSpacing + 18f);
            float height = Mathf.Max(panelSize.y, requiredHeight);
            Rect panel = new Rect((Screen.width - width) * 0.5f, topMargin, width, height);

            DrawRect(panel, new Color(0f, 0f, 0f, 0.72f));
            GUI.Label(new Rect(panel.x + 16f, panel.y + 8f, panel.width - 32f, 28f), encounterTitle, titleStyle);

            float y = panel.y + 42f;

            for (int i = 0; i < channels.Count; i++)
            {
                BDBossHealthChannel channel = channels[i];
                if (channel == null)
                    continue;

                string label = $"{channel.DisplayName}  {channel.CurrentHealth:0}/{channel.MaxHealth:0}";
                GUI.Label(new Rect(panel.x + 16f, y, panel.width - 32f, 18f), label, labelStyle);
                y += 20f;

                Rect background = new Rect(panel.x + 16f, y, panel.width - 32f, barHeight);
                DrawRect(background, new Color(0.12f, 0.12f, 0.12f, 0.95f));

                Rect fill = background;
                fill.width *= channel.NormalizedHealth;
                DrawRect(fill, ResolveBarColor(i, channel.LifeState));

                if (channel.LifeState != BDBossLifeState.Alive)
                {
                    GUI.Label(background, ResolveStateText(channel.LifeState), stateStyle);
                }

                y += barHeight + barSpacing;
            }
        }

        private void EnsureStyles()
        {
            if (whiteTexture == null)
            {
                whiteTexture = new Texture2D(1, 1, TextureFormat.RGBA32, false);
                whiteTexture.SetPixel(0, 0, Color.white);
                whiteTexture.Apply();
            }

            if (titleStyle == null)
            {
                titleStyle = new GUIStyle(GUI.skin.label)
                {
                    alignment = TextAnchor.MiddleCenter,
                    fontSize = 20,
                    fontStyle = FontStyle.Bold
                };
                titleStyle.normal.textColor = Color.white;
            }

            if (labelStyle == null)
            {
                labelStyle = new GUIStyle(GUI.skin.label)
                {
                    alignment = TextAnchor.MiddleLeft,
                    fontSize = 13,
                    fontStyle = FontStyle.Bold
                };
                labelStyle.normal.textColor = Color.white;
            }

            if (stateStyle == null)
            {
                stateStyle = new GUIStyle(GUI.skin.label)
                {
                    alignment = TextAnchor.MiddleCenter,
                    fontSize = 12,
                    fontStyle = FontStyle.Bold
                };
                stateStyle.normal.textColor = Color.white;
            }
        }

        private void DrawRect(Rect rect, Color color)
        {
            Color previous = GUI.color;
            GUI.color = color;
            GUI.DrawTexture(rect, whiteTexture);
            GUI.color = previous;
        }

        private static Color ResolveBarColor(int index, BDBossLifeState state)
        {
            if (state == BDBossLifeState.KnockedOut)
                return new Color(0.28f, 0.28f, 0.28f, 1f);

            if (state == BDBossLifeState.CriticalAtZero)
                return new Color(0.55f, 0.12f, 0.12f, 1f);

            if (state == BDBossLifeState.Dead)
                return new Color(0.08f, 0.08f, 0.08f, 1f);

            Color[] palette =
            {
                new Color(0.78f, 0.16f, 0.18f, 1f),
                new Color(0.18f, 0.44f, 0.82f, 1f),
                new Color(0.66f, 0.24f, 0.78f, 1f),
                new Color(0.90f, 0.67f, 0.12f, 1f)
            };

            return palette[Mathf.Abs(index) % palette.Length];
        }

        private static string ResolveStateText(BDBossLifeState state)
        {
            switch (state)
            {
                case BDBossLifeState.KnockedOut:
                    return "KNOCKED OUT";
                case BDBossLifeState.CriticalAtZero:
                    return "0 HP — STILL ACTIVE";
                case BDBossLifeState.Dead:
                    return "DEFEATED";
                default:
                    return string.Empty;
            }
        }
    }
}
