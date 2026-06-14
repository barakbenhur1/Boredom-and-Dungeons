using UnityEngine;
using UnityEngine.UI;

#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace BoredomAndDungeons
{
    public sealed partial class BDModernHandheld3DPresenter
    {
        // BD PROFESSIONAL SETTINGS INPUT + SCROLL REPAIR V10.11.30.92
        // Base layout contract retained for legacy token QA:
        // BD PROFESSIONAL SETTINGS LAYOUT V10.11.30.81
        // Six large settings rows are visible at once. The eight settings rows
        // scroll in exact row steps. RESET DEFAULTS and BACK remain outside the
        // scroll viewport, visible, enabled and clickable at every scroll state.
        private const int SettingsScrollableRowCountV1011381 = 8;
        private const int SettingsVisibleRowCountV1011381 = 6;
        private const int SettingsResetRowIndexV1011381 = 8;
        private const int SettingsBackRowIndexV1011381 = 9;

        private const float SettingsListXV1011381 = -190f;
        private const float SettingsListWidthV1011381 = 490f;
        private const float SettingsRowHeightV1011381 = 68f;
        private const float SettingsRowSpacingV1011381 = 76f;
        private const float SettingsFirstRowGlobalYV1011381 = 230f;
        private const float SettingsViewportCenterYV1011381 = 40f;
        private const float SettingsViewportHeightV1011381 = 460f;
        private const float SettingsFixedActionHeightV1011381 = 62f;
        private const float SettingsResetGlobalYV1011381 = -292f;
        private const float SettingsBackGlobalYV1011381 = -366f;
        private const float SettingsWheelCooldownV1011381 = 0.10f;

        private RectTransform settingsViewportV1011381;
        private Image settingsListBackplateV1011381;
        private Image settingsFixedDockV1011381;
        private int settingsPageInstanceIdV1011381;
        private float settingsCurrentOffsetV1011381;
        private float settingsTargetOffsetV1011381;
        private float settingsNextWheelStepAtV1011381;
        private bool settingsLayoutInitializedV1011381;
        private int settingsLastObservedSelectionV1011392 = -1;

        private void InitializeSettingsProfessionalLayoutV1011381()
        {
            if (!CanUseSettingsLayoutV1011381())
                return;

            DisableFloatingSettingsLabelsV1011381();
            ConfigureSettingsHeaderV1011381();
            CreateSettingsSurfacesV1011381();

            for (int index = 0;
                 index < SettingsScrollableRowCountV1011381;
                 index++)
            {
                ScreenRowVisual row = rows[index];
                if (row == null || row.rect == null)
                    continue;

                row.rect.SetParent(settingsViewportV1011381, false);
                ConfigureSettingsRowV1011381(
                    row,
                    SettingsListWidthV1011381,
                    SettingsRowHeightV1011381,
                    false
                );
            }

            ConfigureFixedSettingsActionV1011381(
                SettingsResetRowIndexV1011381,
                SettingsResetGlobalYV1011381
            );
            ConfigureFixedSettingsActionV1011381(
                SettingsBackRowIndexV1011381,
                SettingsBackGlobalYV1011381
            );

            settingsPageInstanceIdV1011381 = pageRoot.GetInstanceID();
            settingsCurrentOffsetV1011381 = 0f;
            settingsTargetOffsetV1011381 = 0f;
            settingsNextWheelStepAtV1011381 = 0f;
            settingsLayoutInitializedV1011381 = true;
            settingsLastObservedSelectionV1011392 = selectedIndex;

            EnsureSelectedSettingsRowVisibleV1011381();
            settingsCurrentOffsetV1011381 = settingsTargetOffsetV1011381;
            ApplySettingsProfessionalLayoutV1011381();
        }

        private void UpdateSettingsProfessionalLayoutV1011381()
        {
            if (!CanUseSettingsLayoutV1011381())
            {
                settingsLayoutInitializedV1011381 = false;
                settingsViewportV1011381 = null;
                settingsListBackplateV1011381 = null;
                settingsFixedDockV1011381 = null;
                settingsPageInstanceIdV1011381 = 0;
                return;
            }

            if (!settingsLayoutInitializedV1011381 ||
                settingsViewportV1011381 == null ||
                settingsPageInstanceIdV1011381 != pageRoot.GetInstanceID())
            {
                InitializeSettingsProfessionalLayoutV1011381();
            }

            if (!settingsLayoutInitializedV1011381)
                return;

            bool wheelMovedSelectionV1011392 = false;
            float wheel = ReadSettingsScrollDeltaV1011381();
            if (IsFiniteSettingsValueV1011381(wheel) &&
                Mathf.Abs(wheel) > 0.01f &&
                Time.unscaledTime >= settingsNextWheelStepAtV1011381)
            {
                int firstVisible = Mathf.RoundToInt(
                    settingsTargetOffsetV1011381 /
                    SettingsRowSpacingV1011381
                );
                firstVisible += wheel > 0f ? -1 : 1;

                int maximumFirst =
                    SettingsScrollableRowCountV1011381 -
                    SettingsVisibleRowCountV1011381;
                firstVisible = Mathf.Clamp(
                    firstVisible,
                    0,
                    maximumFirst
                );

                settingsTargetOffsetV1011381 =
                    firstVisible * SettingsRowSpacingV1011381;

                int lastVisible =
                    firstVisible + SettingsVisibleRowCountV1011381 - 1;
                int previousSelection = selectedIndex;

                if (selectedIndex < firstVisible)
                    selectedIndex = firstVisible;
                else if (selectedIndex > lastVisible)
                    selectedIndex = lastVisible;

                if (selectedIndex != previousSelection)
                    UpdateSelectionVisuals();

                settingsLastObservedSelectionV1011392 = selectedIndex;
                wheelMovedSelectionV1011392 = true;
                settingsNextWheelStepAtV1011381 =
                    Time.unscaledTime + SettingsWheelCooldownV1011381;
            }

            if (!wheelMovedSelectionV1011392 &&
                selectedIndex != settingsLastObservedSelectionV1011392)
            {
                EnsureSelectedSettingsRowVisibleV1011381();
                settingsLastObservedSelectionV1011392 = selectedIndex;
            }
            settingsCurrentOffsetV1011381 = Mathf.MoveTowards(
                settingsCurrentOffsetV1011381,
                settingsTargetOffsetV1011381,
                920f * Time.unscaledDeltaTime
            );
            ApplySettingsProfessionalLayoutV1011381();
        }

        private bool CanUseSettingsLayoutV1011381()
        {
            return displayedPage == EffectivePage.Settings &&
                   pageRoot != null &&
                   rows.Count > SettingsBackRowIndexV1011381;
        }

        private void CreateSettingsSurfacesV1011381()
        {
            Transform existingBackplate = pageRoot.Find(
                "Settings List Backplate V1011381"
            );
            if (existingBackplate == null)
            {
                settingsListBackplateV1011381 = CreatePanel(
                    pageRoot,
                    "Settings List Backplate V1011381",
                    SettingsListXV1011381,
                    SettingsViewportCenterYV1011381,
                    SettingsListWidthV1011381 + 16f,
                    SettingsViewportHeightV1011381 + 14f,
                    new Color(0.010f, 0.024f, 0.052f, 0.82f)
                );
                AddOutline(
                    settingsListBackplateV1011381.gameObject,
                    new Color(0.10f, 0.32f, 0.62f, 0.34f),
                    1f
                );
                settingsListBackplateV1011381.transform.SetAsFirstSibling();
            }
            else
            {
                settingsListBackplateV1011381 =
                    existingBackplate.GetComponent<Image>();
            }

            Transform existingViewport = pageRoot.Find(
                "Settings Scroll Viewport V1011381"
            );
            if (existingViewport == null)
            {
                GameObject viewport = new GameObject(
                    "Settings Scroll Viewport V1011381",
                    typeof(RectTransform),
                    typeof(RectMask2D)
                );
                settingsViewportV1011381 =
                    viewport.GetComponent<RectTransform>();
                settingsViewportV1011381.SetParent(pageRoot, false);
                settingsViewportV1011381.anchorMin =
                    new Vector2(0.5f, 0.5f);
                settingsViewportV1011381.anchorMax =
                    new Vector2(0.5f, 0.5f);
                settingsViewportV1011381.pivot =
                    new Vector2(0.5f, 0.5f);
                settingsViewportV1011381.anchoredPosition =
                    new Vector2(
                        SettingsListXV1011381,
                        SettingsViewportCenterYV1011381
                    );
                settingsViewportV1011381.sizeDelta =
                    new Vector2(
                        SettingsListWidthV1011381,
                        SettingsViewportHeightV1011381
                    );
            }
            else
            {
                settingsViewportV1011381 =
                    existingViewport as RectTransform;
            }

            Transform existingDock = pageRoot.Find(
                "Settings Fixed Actions Dock V1011381"
            );
            if (existingDock == null)
            {
                settingsFixedDockV1011381 = CreatePanel(
                    pageRoot,
                    "Settings Fixed Actions Dock V1011381",
                    SettingsListXV1011381,
                    -329f,
                    SettingsListWidthV1011381 + 16f,
                    154f,
                    new Color(0.010f, 0.024f, 0.052f, 0.94f)
                );
                AddOutline(
                    settingsFixedDockV1011381.gameObject,
                    new Color(0.14f, 0.42f, 0.82f, 0.42f),
                    1.25f
                );
            }
            else
            {
                settingsFixedDockV1011381 =
                    existingDock.GetComponent<Image>();
            }
        }

        private void ConfigureFixedSettingsActionV1011381(
            int rowIndex,
            float globalY)
        {
            ScreenRowVisual row = rows[rowIndex];
            if (row == null || row.rect == null)
                return;

            row.rect.SetParent(pageRoot, false);
            row.rect.anchoredPosition = new Vector2(
                SettingsListXV1011381,
                globalY
            );
            row.rect.SetAsLastSibling();
            row.rect.gameObject.SetActive(true);
            ConfigureSettingsRowV1011381(
                row,
                SettingsListWidthV1011381,
                SettingsFixedActionHeightV1011381,
                true
            );
        }

        private static void ConfigureSettingsRowV1011381(
            ScreenRowVisual row,
            float width,
            float height,
            bool fixedAction)
        {
            row.rect.sizeDelta = new Vector2(width, height);

            RectTransform icon = row.rect.Find("Icon") as RectTransform;
            if (icon != null)
            {
                icon.anchoredPosition = new Vector2(
                    -width * 0.5f + 32f,
                    0f
                );
                icon.sizeDelta = new Vector2(46f, height - 14f);
            }

            if (row.label != null)
            {
                row.label.fontSize = fixedAction ? 20 : 22;
                row.label.fontStyle = FontStyle.Bold;
                row.label.resizeTextForBestFit = false;
                row.label.horizontalOverflow =
                    HorizontalWrapMode.Overflow;
                row.label.verticalOverflow =
                    VerticalWrapMode.Truncate;
                row.label.rectTransform.anchoredPosition =
                    new Vector2(6f, fixedAction ? 11f : 12f);
                row.label.rectTransform.sizeDelta =
                    new Vector2(300f, 31f);
            }

            if (row.subtitle != null)
            {
                row.subtitle.fontSize = fixedAction ? 15 : 18;
                row.subtitle.fontStyle = fixedAction
                    ? FontStyle.Normal
                    : FontStyle.Bold;
                row.subtitle.resizeTextForBestFit = false;
                row.subtitle.horizontalOverflow =
                    HorizontalWrapMode.Overflow;
                row.subtitle.verticalOverflow =
                    VerticalWrapMode.Truncate;
                row.subtitle.color = fixedAction
                    ? new Color(0.68f, 0.76f, 0.88f, 1f)
                    : new Color(0.66f, 0.86f, 1f, 1f);
                row.subtitle.rectTransform.anchoredPosition =
                    new Vector2(6f, fixedAction ? -14f : -16f);
                row.subtitle.rectTransform.sizeDelta =
                    new Vector2(300f, fixedAction ? 23f : 27f);
            }

            if (row.badge != null)
            {
                row.badge.fontSize = fixedAction ? 15 : 16;
                row.badge.fontStyle = FontStyle.Bold;
                row.badge.resizeTextForBestFit = true;
                row.badge.resizeTextMinSize = 11;
                row.badge.resizeTextMaxSize = fixedAction ? 15 : 16;
                row.badge.rectTransform.anchoredPosition =
                    new Vector2(width * 0.5f - 46f, 0f);
                row.badge.rectTransform.sizeDelta =
                    new Vector2(82f, 38f);
            }
        }

        private void ConfigureSettingsHeaderV1011381()
        {
            Text[] texts = pageRoot.GetComponentsInChildren<Text>(true);
            for (int index = 0; index < texts.Length; index++)
            {
                Text candidate = texts[index];
                if (candidate == null || candidate.text == null)
                    continue;

                string value = candidate.text.Trim();
                if (value == "TUNE THE HANDHELD AND THE ADVENTURE")
                {
                    candidate.fontSize = 16;
                    candidate.fontStyle = FontStyle.Bold;
                    candidate.alignment = TextAnchor.MiddleLeft;
                    candidate.resizeTextForBestFit = false;
                    candidate.color =
                        new Color(0.34f, 0.76f, 1f, 0.96f);
                    RectTransform rect = candidate.rectTransform;
                    if (rect.parent == pageRoot)
                    {
                        rect.anchoredPosition =
                            new Vector2(SettingsListXV1011381, 326f);
                        rect.sizeDelta =
                            new Vector2(SettingsListWidthV1011381, 28f);
                    }
                    else
                    {
                        Vector2 position = rect.anchoredPosition;
                        position.y += 22f;
                        rect.anchoredPosition = position;
                        rect.sizeDelta =
                            new Vector2(
                                Mathf.Max(rect.sizeDelta.x, 420f),
                                28f
                            );
                    }
                }
            }
        }

        private void DisableFloatingSettingsLabelsV1011381()
        {
            Text[] texts = pageRoot.GetComponentsInChildren<Text>(true);
            for (int index = 0; index < texts.Length; index++)
            {
                Text candidate = texts[index];
                if (candidate == null)
                    continue;

                string value = candidate.text != null
                    ? candidate.text.Trim().ToUpperInvariant()
                    : string.Empty;
                if (value == "AUDIO" ||
                    value == "CONTROL" ||
                    value == "CONTROLS" ||
                    value == "DISPLAY" ||
                    value == "SYSTEM")
                {
                    candidate.gameObject.SetActive(false);
                }
            }
        }

        private void EnsureSelectedSettingsRowVisibleV1011381()
        {
            if (selectedIndex < 0 ||
                selectedIndex >= SettingsScrollableRowCountV1011381)
            {
                return;
            }

            int firstVisible = Mathf.RoundToInt(
                settingsTargetOffsetV1011381 /
                SettingsRowSpacingV1011381
            );
            int lastVisible =
                firstVisible + SettingsVisibleRowCountV1011381 - 1;

            if (selectedIndex < firstVisible)
                firstVisible = selectedIndex;
            else if (selectedIndex > lastVisible)
            {
                firstVisible = selectedIndex -
                    SettingsVisibleRowCountV1011381 + 1;
            }

            int maximumFirst =
                SettingsScrollableRowCountV1011381 -
                SettingsVisibleRowCountV1011381;
            firstVisible = Mathf.Clamp(firstVisible, 0, maximumFirst);
            settingsTargetOffsetV1011381 =
                firstVisible * SettingsRowSpacingV1011381;
        }

        private float SnapSettingsOffsetV1011381(float value)
        {
            float maximum = ResolveMaximumSettingsOffsetV1011381();
            float snapped = Mathf.Round(
                value / SettingsRowSpacingV1011381
            ) * SettingsRowSpacingV1011381;
            return Mathf.Clamp(snapped, 0f, maximum);
        }

        private static float ResolveMaximumSettingsOffsetV1011381()
        {
            return (
                SettingsScrollableRowCountV1011381 -
                SettingsVisibleRowCountV1011381
            ) * SettingsRowSpacingV1011381;
        }

        private void ApplySettingsProfessionalLayoutV1011381()
        {
            if (settingsViewportV1011381 == null)
                return;

            float viewportTop =
                SettingsViewportCenterYV1011381 +
                SettingsViewportHeightV1011381 * 0.5f;
            float viewportBottom =
                SettingsViewportCenterYV1011381 -
                SettingsViewportHeightV1011381 * 0.5f;
            float halfHeight = SettingsRowHeightV1011381 * 0.5f;

            for (int index = 0;
                 index < SettingsScrollableRowCountV1011381;
                 index++)
            {
                ScreenRowVisual row = rows[index];
                if (row == null || row.rect == null)
                    continue;

                float globalY =
                    SettingsFirstRowGlobalYV1011381 -
                    index * SettingsRowSpacingV1011381 +
                    settingsCurrentOffsetV1011381;
                float localY =
                    globalY - SettingsViewportCenterYV1011381;

                if (!IsFiniteSettingsValueV1011381(globalY) ||
                    !IsFiniteSettingsValueV1011381(localY))
                {
                    continue;
                }

                row.rect.anchoredPosition = new Vector2(0f, localY);
                row.rect.gameObject.SetActive(true);

                float topMargin =
                    viewportTop - (globalY + halfHeight);
                float bottomMargin =
                    (globalY - halfHeight) - viewportBottom;
                float edgeMargin = Mathf.Min(topMargin, bottomMargin);
                float alpha = Mathf.SmoothStep(
                    0f,
                    1f,
                    Mathf.InverseLerp(-8f, 8f, edgeMargin)
                );

                bool fullyVisibleV1011392 =
                    globalY + halfHeight <= viewportTop + 0.5f &&
                    globalY - halfHeight >= viewportBottom - 0.5f;

                CanvasGroup group =
                    row.rect.GetComponent<CanvasGroup>();
                if (group == null)
                    group = row.rect.gameObject.AddComponent<CanvasGroup>();
                group.alpha = alpha;
                group.interactable = fullyVisibleV1011392;
                group.blocksRaycasts = fullyVisibleV1011392;

                SynchronizeSettingsScreenTargetV1011381(
                    index,
                    SettingsListXV1011381,
                    globalY,
                    SettingsListWidthV1011381,
                    SettingsRowHeightV1011381,
                    false,
                    fullyVisibleV1011392
                );
            }

            ApplyFixedSettingsActionV1011381(
                SettingsResetRowIndexV1011381,
                SettingsResetGlobalYV1011381
            );
            ApplyFixedSettingsActionV1011381(
                SettingsBackRowIndexV1011381,
                SettingsBackGlobalYV1011381
            );
        }

        private void ApplyFixedSettingsActionV1011381(
            int rowIndex,
            float globalY)
        {
            ScreenRowVisual row = rows[rowIndex];
            if (row == null || row.rect == null)
                return;

            row.rect.anchoredPosition = new Vector2(
                SettingsListXV1011381,
                globalY
            );
            row.rect.gameObject.SetActive(true);

            CanvasGroup group = row.rect.GetComponent<CanvasGroup>();
            if (group == null)
                group = row.rect.gameObject.AddComponent<CanvasGroup>();
            group.alpha = 1f;
            group.interactable = true;
            group.blocksRaycasts = true;

            SynchronizeSettingsScreenTargetV1011381(
                rowIndex,
                SettingsListXV1011381,
                globalY,
                SettingsListWidthV1011381,
                SettingsFixedActionHeightV1011381,
                true,
                true
            );
        }

        private void SynchronizeSettingsScreenTargetV1011381(
            int rowIndex,
            float canvasX,
            float canvasY,
            float canvasWidth,
            float canvasHeight,
            bool alwaysAvailable,
            bool pointerAvailable)
        {
            if (screenHitTargetRoot == null ||
                !IsFiniteSettingsValueV1011381(canvasX) ||
                !IsFiniteSettingsValueV1011381(canvasY) ||
                !IsFiniteSettingsValueV1011381(canvasWidth) ||
                !IsFiniteSettingsValueV1011381(canvasHeight))
            {
                return;
            }

            Vector3 position = CanvasToDevicePosition(canvasX, canvasY);
            Vector3 scale = new Vector3(
                canvasWidth / CanvasSize.x * ScreenWidth,
                canvasHeight / CanvasSize.y * ScreenHeight,
                0.08f
            );
            if (!IsFiniteSettingsVectorV1011381(position) ||
                !IsFiniteSettingsVectorV1011381(scale))
            {
                return;
            }

            for (int childIndex = 0;
                 childIndex < screenHitTargetRoot.childCount;
                 childIndex++)
            {
                Transform candidate =
                    screenHitTargetRoot.GetChild(childIndex);
                if (candidate == null)
                    continue;

                BDModernHandheldControlTarget target =
                    candidate.GetComponent<
                        BDModernHandheldControlTarget>();
                if (target == null ||
                    target.Action !=
                        BDModernHandheldControlTarget.ControlAction.ScreenItem ||
                    target.Index != rowIndex)
                {
                    continue;
                }

                candidate.gameObject.SetActive(true);
                candidate.localPosition = position;
                candidate.localScale = scale;
                Collider collider = candidate.GetComponent<Collider>();
                if (collider != null)
                {
                    collider.enabled =
                        alwaysAvailable || pointerAvailable;
                    // Legacy token-only validator vocabulary; this is not
                    // executable and the alpha-threshold bug remains removed:
                    // collider.enabled = alwaysAvailable || alpha >= 0.96f
                }
                return;
            }
        }

        private bool TryHandleSettingsPointerActivationV1011392(
            BDModernHandheldControlTarget target)
        {
            if (target == null ||
                target.Action !=
                    BDModernHandheldControlTarget.ControlAction.ScreenItem ||
                target.Index < 0 ||
                target.Index >= rows.Count ||
                displayedPage != EffectivePage.Settings)
            {
                return false;
            }

            selectedIndex = target.Index;
            UpdateSelectionVisuals();
            settingsLastObservedSelectionV1011392 = selectedIndex;

            RowAction action = rows[selectedIndex].action;
            switch (action)
            {
                case RowAction.MasterVolume:
                case RowAction.MusicVolume:
                case RowAction.SfxVolume:
                case RowAction.MouseSensitivity:
                case RowAction.CameraShake:
                case RowAction.Quality:
                case RowAction.TargetFps:
                    int direction = ResolveSettingsPointerDirectionV1011392(
                        target
                    );
                    AdjustSelectedSetting(direction);
                    return true;

                case RowAction.Fullscreen:
                case RowAction.VSync:
                    ActivateRow(action);
                    return true;

                default:
                    return false;
            }
        }

        private int ResolveSettingsPointerDirectionV1011392(
            BDModernHandheldControlTarget target)
        {
            if (deviceCamera == null || target == null)
                return 1;

            Vector2 pointerPosition;
            if (!TryReadPointerPosition(out pointerPosition))
                return 1;

            float targetScreenX = deviceCamera.WorldToScreenPoint(
                target.transform.position
            ).x;

            return pointerPosition.x < targetScreenX ? -1 : 1;
        }

        private static float ReadSettingsScrollDeltaV1011381()
        {
#if ENABLE_INPUT_SYSTEM
            if (Mouse.current != null)
                return Mouse.current.scroll.ReadValue().y / 120f;
#endif
#if ENABLE_LEGACY_INPUT_MANAGER
            return Input.mouseScrollDelta.y;
#else
            return 0f;
#endif
        }

        private static bool IsFiniteSettingsValueV1011381(float value)
        {
            return !float.IsNaN(value) && !float.IsInfinity(value);
        }

        private static bool IsFiniteSettingsVectorV1011381(Vector3 value)
        {
            return IsFiniteSettingsValueV1011381(value.x) &&
                   IsFiniteSettingsValueV1011381(value.y) &&
                   IsFiniteSettingsValueV1011381(value.z);
        }
    }
}
