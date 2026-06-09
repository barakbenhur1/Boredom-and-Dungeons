using UnityEngine;
using UnityEngine.UI;

namespace BoredomAndDungeons
{
    public sealed partial class BDModernHandheldV6Polish
    {
        private void PolishPage(RectTransform page)
        {
            ConfigureText(page);
            EnsureSettingsGear(page);
            FixBottomCard(page);

            switch (page.name)
            {
                case "Page MainMenu":
                    PolishMain(page);
                    break;
                case "Page Pause":
                    PolishPause(page);
                    break;
                case "Page Progression":
                    FitPanelText(page, "Progression Panel", 18f);
                    break;
                case "Page Credits":
                    FitPanelText(page, "Credits Panel", 18f);
                    break;
                case "Page Settings":
                    FitPanelText(page, "Settings Artwork Frame", 12f);
                    break;
                case "Page QuitConfirm":
                    FitPanelText(
                        page,
                        "Quit Confirmation Panel",
                        18f
                    );
                    break;
                case "Page AbandonConfirm":
                    FitPanelText(page, "Confirmation Panel", 18f);
                    break;
            }
        }

        private static void ConfigureText(RectTransform page)
        {
            Text[] texts = page.GetComponentsInChildren<Text>(true);
            for (int index = 0; index < texts.Length; index++)
            {
                Text text = texts[index];
                if (text == null)
                    continue;

                text.horizontalOverflow = HorizontalWrapMode.Wrap;
                text.verticalOverflow = VerticalWrapMode.Truncate;

                if (text.name == "Page Title")
                    BestFit(text, 32, text.fontSize);
                else if (text.name == "Page Subtitle")
                    BestFit(text, 13, text.fontSize);
                else if (text.name == "Control Legend")
                    BestFit(text, 9, 12);
                else if (text.name == "Label" ||
                         text.name == "Badge")
                    BestFit(text, 12, text.fontSize);
            }
        }

        private void PolishMain(RectTransform page)
        {
            RectTransform hero = Rect(page, "Hero Art Frame");
            if (hero != null)
                hero.anchoredPosition = new Vector2(275f, 68f);

            RectTransform card = Rect(
                page,
                "New Game Memory Card"
            );
            if (card != null)
            {
                card.anchoredPosition = new Vector2(275f, -210f);
                card.sizeDelta = new Vector2(320f, 124f);
                card.gameObject.SetActive(true);
            }
        }

        private void FixBottomCard(RectTransform page)
        {
            RectTransform card = Rect(page, "Next Card");
            if (card == null)
                return;

            card.anchoredPosition = new Vector2(0f, -350f);
            card.sizeDelta = new Vector2(860f, 132f);

            SetTextRect(
                card,
                "Next Heading",
                new Vector2(-310f, 31f),
                new Vector2(200f, 36f),
                13,
                18
            );
            SetTextRect(
                card,
                "Next Body",
                new Vector2(70f, -18f),
                new Vector2(620f, 70f),
                14,
                20
            );
        }

        private void UpdateContextCard(RectTransform page)
        {
            RectTransform selected = SelectedRow(page);
            RectTransform card = Rect(
                page,
                "New Game Memory Card"
            );
            if (selected == null || card == null)
                return;

            card.gameObject.SetActive(true);
            if (lastSelectedRow == selected.name)
                return;

            lastSelectedRow = selected.name;
            string heading;
            string body;
            ResolveCardCopy(selected.name, out heading, out body);

            Text headingText = TextAt(
                card,
                "New Game Card Heading"
            );
            Text bodyText = TextAt(
                card,
                "New Game Card Status"
            );
            if (headingText != null)
            {
                headingText.text = heading;
                BestFit(headingText, 12, 18);
            }
            if (bodyText != null)
            {
                bodyText.text = body;
                bodyText.fontSize = 21;
                BestFit(bodyText, 14, 21);
            }
        }

        private static void ResolveCardCopy(
            string rowName,
            out string heading,
            out string body)
        {
            string value = rowName.ToUpperInvariant();
            if (value.Contains("PROGRESSION"))
            {
                heading = "PERSISTENT MEMORY";
                body = "Milestones and future upgrades";
            }
            else if (value.Contains("SETTINGS"))
            {
                heading = "SYSTEM CONFIGURATION";
                body = "Audio, controls and display";
            }
            else if (value.Contains("CREDITS"))
            {
                heading = "BEHIND THE ADVENTURE";
                body = "People, ideas and production";
            }
            else if (value.Contains("QUIT") ||
                     value.Contains("EXIT"))
            {
                heading = "LEAVE THE HANDHELD";
                body = "Exit only after confirmation";
            }
            else
            {
                heading = "ADVENTURE MEMORY";
                body = "A new path is ready";
            }
        }
    }
}
