using UnityEngine;
using UnityEngine.UI;

namespace BoredomAndDungeons
{
        // BD INDIE INPUT KEYCAPS V10.11.17
        // PHYSICAL HANDHELD
        // Keyboard/mouse keycaps and the illustrated handheld control remain
        // separate, bounded visual systems instead of plain duplicated text.
    public sealed partial class BDModernHandheld3DPresenter
    {
        // BD INDIE INPUT KEYCAPS V10.11.19
        private RectTransform firstLaunchTutorialDesktopKeycapRoot;
        private Image firstLaunchTutorialDesktopKeycapFace;
        private Text firstLaunchTutorialDesktopKeycapLabel;

        private RectTransform firstLaunchTutorialPhysicalBindingRoot;
        private GameObject firstLaunchTutorialPhysicalDpadRoot;
        private Text firstLaunchTutorialPhysicalPrefix;
        private Text firstLaunchTutorialPhysicalJoiner;
        private GameObject firstLaunchTutorialPhysicalButtonOneRoot;
        private Image firstLaunchTutorialPhysicalButtonOneFace;
        private Text firstLaunchTutorialPhysicalButtonOneGlyph;
        private GameObject firstLaunchTutorialPhysicalButtonTwoRoot;
        private Image firstLaunchTutorialPhysicalButtonTwoFace;
        private Text firstLaunchTutorialPhysicalButtonTwoGlyph;

        private void UpdateFirstLaunchTutorialBindingVisuals(
            string action,
            bool controllerActive,
            bool visible)
        {
            if (!visible)
            {
                SetFirstLaunchTutorialBindingVisualsActive(false);
                return;
            }

            EnsureFirstLaunchTutorialBindingVisuals();
            SetFirstLaunchTutorialBindingVisualsActive(true);

            string desktopBinding = controllerActive
                ? ResolveFirstLaunchTutorialGamepadBinding(action)
                : ResolveFirstLaunchTutorialKeyboardBinding(action);
            firstLaunchTutorialDesktopKeycapLabel.text = desktopBinding;
            firstLaunchTutorialDesktopKeycapFace.color = controllerActive
                ? new Color(0.25f, 0.18f, 0.62f, 1f)
                : new Color(0.025f, 0.42f, 0.62f, 1f);

            if (firstLaunchTutorialKeyboardBinding != null)
                firstLaunchTutorialKeyboardBinding.gameObject.SetActive(false);
            if (firstLaunchTutorialHandheldBinding != null)
                firstLaunchTutorialHandheldBinding.gameObject.SetActive(false);

            BuildFirstLaunchTutorialPhysicalBindingVisual(action);
        }

        private void SetFirstLaunchTutorialBindingVisualsActive(bool active)
        {
            if (firstLaunchTutorialDesktopKeycapRoot != null)
                firstLaunchTutorialDesktopKeycapRoot.gameObject.SetActive(active);
            if (firstLaunchTutorialPhysicalBindingRoot != null)
                firstLaunchTutorialPhysicalBindingRoot.gameObject.SetActive(active);
        }

        private void EnsureFirstLaunchTutorialBindingVisuals()
        {
            if (firstLaunchTutorialDesktopKeycapRoot == null &&
                firstLaunchTutorialKeyboardBindingCard != null)
            {
                GameObject root = new GameObject(
                    "Keyboard / Mouse Keycap",
                    typeof(RectTransform)
                );
                root.transform.SetParent(
                    firstLaunchTutorialKeyboardBindingCard.rectTransform,
                    false
                );
                firstLaunchTutorialDesktopKeycapRoot =
                    root.GetComponent<RectTransform>();
                ConfigureFirstLaunchTutorialBindingRectV101117(
                    firstLaunchTutorialDesktopKeycapRoot,
                    new Vector2(0f, -6f),
                    new Vector2(248f, 38f)
                );

                firstLaunchTutorialDesktopKeycapFace =
                    CreateFirstLaunchTutorialBindingPanel(
                        firstLaunchTutorialDesktopKeycapRoot,
                        "Desktop Keycap Face",
                        Vector2.zero,
                        new Vector2(224f, 34f),
                        new Color(0.025f, 0.42f, 0.62f, 1f),
                        new Color(0.42f, 0.92f, 1f, 0.95f)
                    );
                firstLaunchTutorialDesktopKeycapLabel =
                    CreateFirstLaunchTutorialBindingLabel(
                        firstLaunchTutorialDesktopKeycapFace.rectTransform,
                        "Desktop Keycap Label",
                        string.Empty,
                        Vector2.zero,
                        new Vector2(208f, 30f),
                        17
                    );
            }

            if (firstLaunchTutorialPhysicalBindingRoot == null &&
                firstLaunchTutorialHandheldBindingCard != null)
            {
                GameObject root = new GameObject(
                    "Physical Handheld Binding Picture",
                    typeof(RectTransform)
                );
                root.transform.SetParent(
                    firstLaunchTutorialHandheldBindingCard.rectTransform,
                    false
                );
                firstLaunchTutorialPhysicalBindingRoot =
                    root.GetComponent<RectTransform>();
                ConfigureFirstLaunchTutorialBindingRectV101117(
                    firstLaunchTutorialPhysicalBindingRoot,
                    new Vector2(0f, -6f),
                    new Vector2(248f, 48f)
                );

                firstLaunchTutorialPhysicalPrefix =
                    CreateFirstLaunchTutorialBindingLabel(
                        firstLaunchTutorialPhysicalBindingRoot,
                        "Physical Hold Prefix",
                        string.Empty,
                        new Vector2(-80f, 0f),
                        new Vector2(56f, 28f),
                        13
                    );
                firstLaunchTutorialPhysicalJoiner =
                    CreateFirstLaunchTutorialBindingLabel(
                        firstLaunchTutorialPhysicalBindingRoot,
                        "Physical Button Joiner",
                        "+",
                        Vector2.zero,
                        new Vector2(24f, 28f),
                        17
                    );

                CreateFirstLaunchTutorialPhysicalButton(
                    firstLaunchTutorialPhysicalBindingRoot,
                    "Tutorial Physical Button Picture One",
                    new Vector2(-28f, 0f),
                    out firstLaunchTutorialPhysicalButtonOneRoot,
                    out firstLaunchTutorialPhysicalButtonOneFace,
                    out firstLaunchTutorialPhysicalButtonOneGlyph
                );
                CreateFirstLaunchTutorialPhysicalButton(
                    firstLaunchTutorialPhysicalBindingRoot,
                    "Tutorial Physical Button Picture Two",
                    new Vector2(28f, 0f),
                    out firstLaunchTutorialPhysicalButtonTwoRoot,
                    out firstLaunchTutorialPhysicalButtonTwoFace,
                    out firstLaunchTutorialPhysicalButtonTwoGlyph
                );

                firstLaunchTutorialPhysicalDpadRoot =
                    CreateFirstLaunchTutorialPhysicalDpad(
                        firstLaunchTutorialPhysicalBindingRoot
                    );
            }
        }

        private void BuildFirstLaunchTutorialPhysicalBindingVisual(
            string action)
        {
            EnsureFirstLaunchTutorialBindingVisuals();
            if (firstLaunchTutorialPhysicalBindingRoot == null)
                return;

            bool showDpad = action == "MOVE" || action == "DODGE";
            bool jumpAttack =
                firstLaunchTutorialStep == FirstLaunchTutorialStep.JumpAttack;
            bool twoButtons = jumpAttack || action == "PARRY";
            bool hold =
                action == "HEAL" ||
                action == "SPIN" ||
                action == "GRAPPLE" ||
                (action == "RANGED" &&
                 firstLaunchTutorialStep == FirstLaunchTutorialStep.ChargedShot);

            firstLaunchTutorialPhysicalDpadRoot.SetActive(showDpad);
            firstLaunchTutorialPhysicalButtonOneRoot.SetActive(!showDpad);
            firstLaunchTutorialPhysicalButtonTwoRoot.SetActive(
                !showDpad && twoButtons
            );
            firstLaunchTutorialPhysicalJoiner.gameObject.SetActive(
                !showDpad && twoButtons
            );
            firstLaunchTutorialPhysicalPrefix.gameObject.SetActive(
                hold || action == "DODGE"
            );
            firstLaunchTutorialPhysicalPrefix.text = action == "DODGE"
                ? "×2"
                : hold ? "HOLD" : string.Empty;
            firstLaunchTutorialPhysicalPrefix.rectTransform.anchoredPosition =
                showDpad
                    ? new Vector2(60f, 0f)
                    : new Vector2(-80f, 0f);

            if (showDpad)
                return;

            string firstGlyph;
            Color firstColor;
            string secondGlyph = string.Empty;
            Color secondColor = Color.white;

            if (jumpAttack)
            {
                firstGlyph = "B";
                firstColor = ResolveFirstLaunchTutorialPhysicalButtonColor("B");
                secondGlyph = "X";
                secondColor = ResolveFirstLaunchTutorialPhysicalButtonColor("X");
            }
            else if (action == "PARRY")
            {
                firstGlyph = "X";
                firstColor = ResolveFirstLaunchTutorialPhysicalButtonColor("X");
                secondGlyph = "Y";
                secondColor = ResolveFirstLaunchTutorialPhysicalButtonColor("Y");
            }
            else
            {
                firstGlyph = ResolveFirstLaunchTutorialPhysicalButtonGlyph(action);
                firstColor = ResolveFirstLaunchTutorialPhysicalButtonColor(firstGlyph);
            }

            ConfigureFirstLaunchTutorialPhysicalButton(
                firstLaunchTutorialPhysicalButtonOneFace,
                firstLaunchTutorialPhysicalButtonOneGlyph,
                firstGlyph,
                firstColor
            );
            if (twoButtons)
            {
                ConfigureFirstLaunchTutorialPhysicalButton(
                    firstLaunchTutorialPhysicalButtonTwoFace,
                    firstLaunchTutorialPhysicalButtonTwoGlyph,
                    secondGlyph,
                    secondColor
                );
            }

            float firstX = twoButtons ? -28f : 0f;
            firstLaunchTutorialPhysicalButtonOneRoot
                .GetComponent<RectTransform>().anchoredPosition =
                    new Vector2(firstX, 0f);
        }

        private static string ResolveFirstLaunchTutorialPhysicalButtonGlyph(
            string action)
        {
            switch (action)
            {
                case "INTERACT": return "SELECT";
                case "JUMP": return "B";
                case "HEAL": return "A";
                case "RANGED": return "A";
                case "HEAVY": return "Y";
                case "GRAPPLE": return "Y";
                case "ATTACK": return "X";
                case "SPIN": return "X";
                default: return "X";
            }
        }

        private static Color ResolveFirstLaunchTutorialPhysicalButtonColor(
            string glyph)
        {
            switch (glyph)
            {
                case "A": return new Color(0.96f, 0.48f, 0.06f, 1f);
                case "B": return new Color(0.90f, 0.18f, 0.08f, 1f);
                case "Y": return new Color(0.30f, 0.20f, 0.84f, 1f);
                case "SELECT": return new Color(0.07f, 0.08f, 0.11f, 1f);
                default: return new Color(0.80f, 0.14f, 0.76f, 1f);
            }
        }

        private void CreateFirstLaunchTutorialPhysicalButton(
            RectTransform parent,
            string name,
            Vector2 position,
            out GameObject root,
            out Image face,
            out Text glyph)
        {
            root = new GameObject(name, typeof(RectTransform));
            root.transform.SetParent(parent, false);
            RectTransform rootRect = root.GetComponent<RectTransform>();
            ConfigureFirstLaunchTutorialBindingRectV101117(rootRect, position, new Vector2(46f, 42f));

            face = CreateFirstLaunchTutorialBindingPanel(
                rootRect,
                name + " Face",
                Vector2.zero,
                new Vector2(38f, 38f),
                new Color(0.80f, 0.14f, 0.76f, 1f),
                new Color(0.02f, 0.025f, 0.055f, 1f)
            );
            glyph = CreateFirstLaunchTutorialBindingLabel(
                face.rectTransform,
                name + " Glyph",
                "X",
                Vector2.zero,
                new Vector2(96f, 34f),
                20
            );
        }

        private void ConfigureFirstLaunchTutorialPhysicalButton(
            Image face,
            Text glyph,
            string value,
            Color color)
        {
            bool select = value == "SELECT";
            face.color = color;
            face.rectTransform.sizeDelta = select
                ? new Vector2(92f, 28f)
                : new Vector2(38f, 38f);
            glyph.text = value;
            glyph.fontSize = select ? 12 : 20;
        }

        private GameObject CreateFirstLaunchTutorialPhysicalDpad(
            RectTransform parent)
        {
            GameObject root = new GameObject(
                "Tutorial Physical DPad Picture",
                typeof(RectTransform)
            );
            root.transform.SetParent(parent, false);
            RectTransform rect = root.GetComponent<RectTransform>();
            ConfigureFirstLaunchTutorialBindingRectV101117(
                rect,
                new Vector2(0f, 0f),
                new Vector2(64f, 38f)
            );
            CreateFirstLaunchTutorialBindingPanel(
                rect,
                "Physical DPad Horizontal",
                Vector2.zero,
                new Vector2(62f, 16f),
                new Color(0.055f, 0.065f, 0.095f, 1f),
                new Color(0.34f, 0.48f, 0.88f, 0.82f)
            );
            CreateFirstLaunchTutorialBindingPanel(
                rect,
                "Physical DPad Vertical",
                Vector2.zero,
                new Vector2(16f, 38f),
                new Color(0.055f, 0.065f, 0.095f, 1f),
                new Color(0.34f, 0.48f, 0.88f, 0.82f)
            );
            CreateFirstLaunchTutorialBindingPanel(
                rect,
                "Physical DPad Center",
                Vector2.zero,
                new Vector2(12f, 12f),
                new Color(0.22f, 0.38f, 0.92f, 1f),
                Color.clear
            );
            return root;
        }

        private Image CreateFirstLaunchTutorialBindingPanel(
            RectTransform parent,
            string name,
            Vector2 position,
            Vector2 size,
            Color color,
            Color outlineColor)
        {
            GameObject value = new GameObject(
                name,
                typeof(RectTransform),
                typeof(CanvasRenderer),
                typeof(Image),
                typeof(Outline)
            );
            value.transform.SetParent(parent, false);
            RectTransform rect = value.GetComponent<RectTransform>();
            ConfigureFirstLaunchTutorialBindingRectV101117(rect, position, size);
            Image image = value.GetComponent<Image>();
            image.sprite = roundedSprite;
            image.type = roundedSprite != null
                ? Image.Type.Sliced
                : Image.Type.Simple;
            image.color = color;
            image.raycastTarget = false;
            Outline outline = value.GetComponent<Outline>();
            outline.effectColor = outlineColor;
            outline.effectDistance = new Vector2(2f, -2f);
            outline.useGraphicAlpha = true;
            return image;
        }

        private Text CreateFirstLaunchTutorialBindingLabel(
            RectTransform parent,
            string name,
            string value,
            Vector2 position,
            Vector2 size,
            int fontSize)
        {
            GameObject labelObject = new GameObject(
                name,
                typeof(RectTransform),
                typeof(CanvasRenderer),
                typeof(Text)
            );
            labelObject.transform.SetParent(parent, false);
            RectTransform rect = labelObject.GetComponent<RectTransform>();
            ConfigureFirstLaunchTutorialBindingRectV101117(rect, position, size);
            Text label = labelObject.GetComponent<Text>();
            label.font = uiFont != null
                ? uiFont
                : Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            label.fontSize = fontSize;
            label.fontStyle = FontStyle.Bold;
            label.alignment = TextAnchor.MiddleCenter;
            label.color = Color.white;
            label.raycastTarget = false;
            label.resizeTextForBestFit = true;
            label.resizeTextMinSize = 10;
            label.resizeTextMaxSize = fontSize;
            label.horizontalOverflow = HorizontalWrapMode.Wrap;
            label.verticalOverflow = VerticalWrapMode.Truncate;
            return label;
        }

        private static void ConfigureFirstLaunchTutorialBindingRectV101117(
            RectTransform rect,
            Vector2 position,
            Vector2 size)
        {
            rect.anchorMin = new Vector2(0.5f, 0.5f);
            rect.anchorMax = new Vector2(0.5f, 0.5f);
            rect.pivot = new Vector2(0.5f, 0.5f);
            rect.anchoredPosition = position;
            rect.sizeDelta = size;
        }
    }
}
