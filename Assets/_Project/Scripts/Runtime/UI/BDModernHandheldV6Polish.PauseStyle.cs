using UnityEngine;
using UnityEngine.UI;

namespace BoredomAndDungeons
{
    public sealed partial class BDModernHandheldV6Polish
    {
        private void EnsurePausePanel(RectTransform page)
        {
            RectTransform panel = Rect(page, "Next Card");
            if (panel == null)
                return;

            panel.gameObject.SetActive(true);
            panel.SetAsFirstSibling();
            panel.anchoredPosition = new Vector2(0f, 16f);
            panel.sizeDelta = new Vector2(720f, 600f);

            Text heading = TextAt(panel, "Next Heading");
            if (heading != null)
            {
                heading.text = "PAUSE MENU";
                heading.alignment = TextAnchor.MiddleCenter;
                heading.rectTransform.anchoredPosition =
                    new Vector2(0f, -240f);
                heading.rectTransform.sizeDelta =
                    new Vector2(620f, 38f);
                BestFit(heading, 14, 20);
            }

            Text body = TextAt(panel, "Next Body");
            if (body != null)
            {
                body.text = "RUN PAUSED  //  SELECT AN OPTION";
                body.alignment = TextAnchor.MiddleCenter;
                body.rectTransform.anchoredPosition =
                    new Vector2(0f, -278f);
                body.rectTransform.sizeDelta =
                    new Vector2(620f, 42f);
                BestFit(body, 12, 18);
            }
        }
    }
}
