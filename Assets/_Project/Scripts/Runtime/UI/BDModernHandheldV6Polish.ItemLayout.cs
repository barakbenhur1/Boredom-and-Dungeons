using UnityEngine;

namespace BoredomAndDungeons
{
    public sealed partial class BDModernHandheldV6Polish
    {
        private static void PlaceMenuItem(
            RectTransform item,
            float x,
            float y,
            float width,
            float height)
        {
            item.anchoredPosition = new Vector2(x, y);
            item.sizeDelta = new Vector2(width, height);

            SetRect(
                item,
                "Icon",
                new Vector2(-width * 0.5f + 39f, 0f),
                new Vector2(54f, height - 12f)
            );
            SetRect(
                item,
                "Label",
                new Vector2(10f, 12f),
                new Vector2(width - 150f, 34f)
            );
            SetRect(
                item,
                "Subtitle",
                new Vector2(10f, -17f),
                new Vector2(width - 150f, 28f)
            );
            SetRect(
                item,
                "Badge",
                new Vector2(width * 0.5f - 45f, 0f),
                new Vector2(70f, 38f)
            );
        }
    }
}
