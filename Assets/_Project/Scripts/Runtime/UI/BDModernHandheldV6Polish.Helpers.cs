using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BoredomAndDungeons
{
    public sealed partial class BDModernHandheldV6Polish
    {
        private static RectTransform SelectedRow(
            RectTransform page)
        {
            RectTransform[] rows = DirectRows(page);
            RectTransform selected = null;
            float best = float.MinValue;
            for (int index = 0; index < rows.Length; index++)
            {
                if (rows[index].localScale.x > best)
                {
                    best = rows[index].localScale.x;
                    selected = rows[index];
                }
            }
            return selected;
        }

        private static RectTransform[] DirectRows(
            RectTransform page)
        {
            List<RectTransform> rows = new List<RectTransform>();
            if (page == null)
                return rows.ToArray();

            for (int index = 0; index < page.childCount; index++)
            {
                RectTransform child =
                    page.GetChild(index) as RectTransform;
                if (child != null &&
                    child.name.StartsWith(
                        "Row ",
                        StringComparison.Ordinal))
                {
                    rows.Add(child);
                }
            }
            return rows.ToArray();
        }

        private static Image FirstRowImage(RectTransform page)
        {
            RectTransform[] rows = DirectRows(page);
            return rows.Length > 0
                ? rows[0].GetComponent<Image>()
                : null;
        }

        private static void SetTextRect(
            Transform root,
            string name,
            Vector2 position,
            Vector2 size,
            int min,
            int max)
        {
            Text text = TextAt(root, name);
            if (text == null)
                return;

            text.rectTransform.anchoredPosition = position;
            text.rectTransform.sizeDelta = size;
            BestFit(text, min, max);
        }

        private static void SetRect(
            Transform root,
            string name,
            Vector2 position,
            Vector2 size)
        {
            RectTransform rect = Rect(root, name);
            if (rect == null)
                return;

            rect.anchoredPosition = position;
            rect.sizeDelta = size;
        }

        private static void SetActive(
            Transform root,
            string name,
            bool active)
        {
            Transform child = Find(root, name);
            if (child != null)
                child.gameObject.SetActive(active);
        }

        private static void BestFit(
            Text text,
            int min,
            int max)
        {
            text.resizeTextForBestFit = true;
            text.resizeTextMinSize = Mathf.Max(1, min);
            text.resizeTextMaxSize = Mathf.Max(
                text.resizeTextMinSize,
                max
            );
        }

        private static Text TextAt(Transform root, string name)
        {
            Transform child = Find(root, name);
            return child != null
                ? child.GetComponent<Text>()
                : null;
        }

        private static RectTransform Rect(
            Transform root,
            string name)
        {
            return Find(root, name) as RectTransform;
        }

        private static Transform Find(
            Transform root,
            string name)
        {
            if (root == null)
                return null;
            if (root.name == name)
                return root;

            for (int index = 0; index < root.childCount; index++)
            {
                Transform result = Find(root.GetChild(index), name);
                if (result != null)
                    return result;
            }
            return null;
        }

        private static void SetLayer(GameObject root, int layer)
        {
            root.layer = layer;
            for (int index = 0;
                 index < root.transform.childCount;
                 index++)
            {
                SetLayer(
                    root.transform.GetChild(index).gameObject,
                    layer
                );
            }
        }
    }
}
