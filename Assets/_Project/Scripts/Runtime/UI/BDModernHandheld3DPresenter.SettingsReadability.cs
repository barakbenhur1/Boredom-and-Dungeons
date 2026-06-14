using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace BoredomAndDungeons
{
    public sealed partial class BDModernHandheld3DPresenter
    {
        private const float SettingsReadableRowHeight = 78f;
        private const float SettingsReadableRowSpacing = 76f;
        private const float SettingsReadableFirstRowY = 292f;
        private const float SettingsReadableViewportTop = 326f;
        private const float SettingsReadableViewportBottom = -306f;
        private static readonly float SettingsCanvasToDeviceY =
            ScreenHeight / CanvasSize.y;

        [RuntimeInitializeOnLoadMethod(
            RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void InstallSettingsReadabilityDriver()
        {
            if (UnityEngine.Object.FindFirstObjectByType<
                    SettingsReadabilityDriver>() != null)
            {
                return;
            }

            GameObject root = new GameObject(
                "B&D Handheld Settings Readability Driver"
            );
            DontDestroyOnLoad(root);
            root.AddComponent<SettingsReadabilityDriver>();
        }

        [DefaultExecutionOrder(-805)]
        private sealed class SettingsReadabilityDriver : MonoBehaviour
        {
            private readonly List<float> baseRowY =
                new List<float>(12);
            private readonly List<Vector3> baseTargetPositions =
                new List<Vector3>(12);
            private readonly List<CanvasGroup> rowCanvasGroups =
                new List<CanvasGroup>(12);
            private readonly List<Collider> targetColliders =
                new List<Collider>(12);

            private BDModernHandheld3DPresenter owner;
            private int configuredPageInstanceId;
            private float currentScrollOffset;
            private float targetScrollOffset;
            private float manualScrollUntil;

            private void LateUpdate()
            {
                ResolveOwner();
                if (owner == null ||
                    !owner.visible ||
                    owner.displayedPage != EffectivePage.Settings ||
                    owner.pageRoot == null ||
                    owner.rows.Count == 0)
                {
                    configuredPageInstanceId = 0;
                    return;
                }

                int pageInstanceId = owner.pageRoot.GetInstanceID();
                if (configuredPageInstanceId != pageInstanceId)
                    ConfigureCurrentSettingsPage(pageInstanceId);

                if (configuredPageInstanceId == 0)
                    return;

                float wheel = ReadScrollDelta();
                if (Mathf.Abs(wheel) > 0.01f)
                {
                    targetScrollOffset = Mathf.Clamp(
                        targetScrollOffset - wheel * 34f,
                        0f,
                        ResolveMaximumScrollOffset()
                    );
                    manualScrollUntil = Time.unscaledTime + 0.65f;
                }

                KeepSelectedRowReadable();

                currentScrollOffset = Mathf.MoveTowards(
                    currentScrollOffset,
                    targetScrollOffset,
                    620f * Time.unscaledDeltaTime
                );

                ApplyReadableLayout();
            }

            private void ResolveOwner()
            {
                if (owner != null)
                    return;

                owner = UnityEngine.Object.FindFirstObjectByType<
                    BDModernHandheld3DPresenter>();
            }

            private void ConfigureCurrentSettingsPage(int pageInstanceId)
            {
                configuredPageInstanceId = pageInstanceId;
                currentScrollOffset = 0f;
                targetScrollOffset = 0f;
                manualScrollUntil = 0f;

                baseRowY.Clear();
                baseTargetPositions.Clear();
                rowCanvasGroups.Clear();
                targetColliders.Clear();

                for (int index = 0; index < owner.rows.Count; index++)
                {
                    ScreenRowVisual row = owner.rows[index];
                    if (row == null || row.rect == null)
                    {
                        configuredPageInstanceId = 0;
                        return;
                    }

                    float newY = SettingsReadableFirstRowY -
                        index * SettingsReadableRowSpacing;
                    float previousY = row.rect.anchoredPosition.y;

                    row.rect.sizeDelta = new Vector2(
                        row.rect.sizeDelta.x,
                        SettingsReadableRowHeight
                    );
                    row.rect.anchoredPosition = new Vector2(
                        row.rect.anchoredPosition.x,
                        newY
                    );

                    ConfigureReadableText(row);
                    baseRowY.Add(newY);

                    CanvasGroup group =
                        row.rect.GetComponent<CanvasGroup>();
                    if (group == null)
                        group = row.rect.gameObject.AddComponent<CanvasGroup>();
                    rowCanvasGroups.Add(group);

                    Transform target = ResolveTarget(index);
                    if (target != null)
                    {
                        Vector3 targetPosition = target.localPosition;
                        targetPosition.y +=
                            (newY - previousY) * SettingsCanvasToDeviceY;
                        target.localPosition = targetPosition;
                        baseTargetPositions.Add(targetPosition);
                        targetColliders.Add(target.GetComponent<Collider>());
                    }
                    else
                    {
                        baseTargetPositions.Add(Vector3.zero);
                        targetColliders.Add(null);
                    }
                }

                KeepSelectedRowReadable(force: true);
                currentScrollOffset = targetScrollOffset;
                ApplyReadableLayout();
            }

            private static void ConfigureReadableText(
                ScreenRowVisual row)
            {
                if (row.label != null)
                {
                    row.label.fontSize = 24;
                    row.label.fontStyle = FontStyle.Bold;
                    row.label.resizeTextForBestFit = false;
                    row.label.color = Color.white;
                    RectTransform rect = row.label.rectTransform;
                    rect.anchoredPosition = new Vector2(
                        rect.anchoredPosition.x,
                        15f
                    );
                    rect.sizeDelta = new Vector2(
                        rect.sizeDelta.x,
                        34f
                    );
                }

                if (row.subtitle != null)
                {
                    row.subtitle.fontSize = 20;
                    row.subtitle.fontStyle = FontStyle.Bold;
                    row.subtitle.resizeTextForBestFit = false;
                    row.subtitle.color =
                        new Color(0.72f, 0.88f, 1f, 1f);
                    RectTransform rect = row.subtitle.rectTransform;
                    rect.anchoredPosition = new Vector2(
                        rect.anchoredPosition.x,
                        -17f
                    );
                    rect.sizeDelta = new Vector2(
                        rect.sizeDelta.x,
                        30f
                    );
                }

                if (row.badge != null)
                {
                    row.badge.fontSize = 18;
                    row.badge.fontStyle = FontStyle.Bold;
                    row.badge.resizeTextForBestFit = false;
                }
            }

            private void KeepSelectedRowReadable(bool force = false)
            {
                if (baseRowY.Count == 0)
                    return;

                int selected = Mathf.Clamp(
                    owner.selectedIndex,
                    0,
                    baseRowY.Count - 1
                );
                float selectedY =
                    baseRowY[selected] + targetScrollOffset;
                float readableTop =
                    SettingsReadableViewportTop - 42f;
                float readableBottom =
                    SettingsReadableViewportBottom + 42f;

                bool selectedOutside =
                    selectedY > readableTop ||
                    selectedY < readableBottom;

                if (!force &&
                    !selectedOutside &&
                    Time.unscaledTime < manualScrollUntil)
                {
                    return;
                }

                if (selectedY > readableTop)
                {
                    targetScrollOffset -=
                        selectedY - readableTop;
                }
                else if (selectedY < readableBottom)
                {
                    targetScrollOffset +=
                        readableBottom - selectedY;
                }

                targetScrollOffset = Mathf.Clamp(
                    targetScrollOffset,
                    0f,
                    ResolveMaximumScrollOffset()
                );
            }

            private float ResolveMaximumScrollOffset()
            {
                if (baseRowY.Count == 0)
                    return 0f;

                float lastBottom =
                    baseRowY[baseRowY.Count - 1] -
                    SettingsReadableRowHeight * 0.5f;
                return Mathf.Max(
                    0f,
                    SettingsReadableViewportBottom - lastBottom
                );
            }

            private void ApplyReadableLayout()
            {
                for (int index = 0;
                     index < owner.rows.Count &&
                     index < baseRowY.Count;
                     index++)
                {
                    ScreenRowVisual row = owner.rows[index];
                    if (row == null || row.rect == null)
                        continue;

                    float y = baseRowY[index] + currentScrollOffset;
                    row.rect.anchoredPosition = new Vector2(
                        row.rect.anchoredPosition.x,
                        y
                    );

                    float edgeDistance = Mathf.Min(
                        SettingsReadableViewportTop - y,
                        y - SettingsReadableViewportBottom
                    );
                    float alpha = Mathf.SmoothStep(
                        0f,
                        1f,
                        Mathf.InverseLerp(-6f, 34f, edgeDistance)
                    );

                    CanvasGroup group = rowCanvasGroups[index];
                    if (group != null)
                    {
                        group.alpha = alpha;
                        group.interactable = alpha > 0.55f;
                        group.blocksRaycasts = alpha > 0.55f;
                    }

                    Transform target = ResolveTarget(index);
                    if (target != null &&
                        index < baseTargetPositions.Count)
                    {
                        Vector3 position = baseTargetPositions[index];
                        position.y +=
                            currentScrollOffset * SettingsCanvasToDeviceY;
                        target.localPosition = position;
                    }

                    Collider collider = targetColliders[index];
                    if (collider != null)
                        collider.enabled = alpha > 0.55f;
                }
            }

            private Transform ResolveTarget(int index)
            {
                if (owner.screenHitTargetRoot == null ||
                    index < 0 ||
                    index >= owner.screenHitTargetRoot.childCount)
                {
                    return null;
                }

                return owner.screenHitTargetRoot.GetChild(index);
            }

            private static float ReadScrollDelta()
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
        }
    }
}
