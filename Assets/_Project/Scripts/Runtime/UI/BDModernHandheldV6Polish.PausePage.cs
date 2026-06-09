using UnityEngine;

namespace BoredomAndDungeons
{
    public sealed partial class BDModernHandheldV6Polish
    {
        private void PolishPause(RectTransform page)
        {
            SetActive(page, "Hero Art Frame", false);
            SetActive(page, "New Game Memory Card", false);
            SetActive(page, "Next Card", false);

            RectTransform title = Rect(page, "Page Title");
            if (title != null)
            {
                title.anchoredPosition = new Vector2(0f, 400f);
                title.sizeDelta = new Vector2(720f, 92f);
            }

            RectTransform subtitle = Rect(page, "Page Subtitle");
            if (subtitle != null)
            {
                subtitle.anchoredPosition = new Vector2(0f, 335f);
                subtitle.sizeDelta = new Vector2(660f, 44f);
            }

            EnsurePausePanel(page);

            float[] y = { 170f, 72f, -26f, -124f };
            RectTransform[] rows = DirectRows(page);
            for (int index = 0;
                 index < rows.Length && index < y.Length;
                 index++)
            {
                ResizeRow(rows[index], 0f, y[index], 620f, 78f);
                MoveScreenTarget(index, 0f, y[index], 620f, 78f);
            }
        }
    }
}
