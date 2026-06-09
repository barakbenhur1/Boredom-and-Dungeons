using System;
using UnityEngine;
using UnityEngine.UI;

namespace BoredomAndDungeons
{
    public sealed partial class BDModernHandheldV6Polish
    {
        private static void EnsureSettingsGear(RectTransform page)
        {
            RectTransform[] rows = DirectRows(page);
            for (int index = 0; index < rows.Length; index++)
            {
                RectTransform row = rows[index];
                if (row.name.IndexOf(
                        "SETTINGS",
                        StringComparison.OrdinalIgnoreCase) < 0)
                {
                    continue;
                }

                Text glyph = TextAt(row, "Icon Glyph");
                if (glyph == null)
                    continue;

                bool supportsGear =
                    glyph.font != null &&
                    glyph.font.HasCharacter('\u2699');
                bool supportsMenuMark =
                    glyph.font != null &&
                    glyph.font.HasCharacter('\u2261');

                glyph.text = supportsGear
                    ? "\u2699"
                    : supportsMenuMark
                        ? "\u2261"
                        : "S";
                glyph.fontSize = supportsGear
                    ? 24
                    : supportsMenuMark
                        ? 27
                        : 22;
                glyph.alignment = TextAnchor.MiddleCenter;
                BestFit(glyph, 18, glyph.fontSize);
            }
        }

        private static void FitPanelText(
            RectTransform page,
            string panelName,
            float margin)
        {
            RectTransform panel = Rect(page, panelName);
            if (panel == null)
                return;

            Vector2 panelSize = panel.rect.size;
            Text[] texts = panel.GetComponentsInChildren<Text>(true);
            for (int index = 0; index < texts.Length; index++)
            {
                Text text = texts[index];
                RectTransform rect = text.rectTransform;
                Vector2 size = rect.sizeDelta;
                size.x = Mathf.Min(
                    size.x,
                    Mathf.Max(20f, panelSize.x - margin * 2f)
                );
                size.y = Mathf.Min(
                    size.y,
                    Mathf.Max(20f, panelSize.y - margin * 2f)
                );
                rect.sizeDelta = size;

                float xLimit = Mathf.Max(
                    0f,
                    (panelSize.x - size.x) * 0.5f - margin
                );
                float yLimit = Mathf.Max(
                    0f,
                    (panelSize.y - size.y) * 0.5f - margin
                );
                Vector2 position = rect.anchoredPosition;
                position.x = Mathf.Clamp(
                    position.x,
                    -xLimit,
                    xLimit
                );
                position.y = Mathf.Clamp(
                    position.y,
                    -yLimit,
                    yLimit
                );
                rect.anchoredPosition = position;
                BestFit(
                    text,
                    Mathf.Max(10, text.fontSize - 8),
                    text.fontSize
                );
            }
        }
    }
}
