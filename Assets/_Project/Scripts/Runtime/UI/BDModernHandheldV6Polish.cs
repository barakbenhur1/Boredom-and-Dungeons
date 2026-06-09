using System;
using UnityEngine;
using UnityEngine.UI;

namespace BoredomAndDungeons
{
    /// <summary>
    /// Additive visual-only correction layer for the generated 3D handheld.
    /// BDMainMenuFlow and BDModernHandheld3DPresenter keep semantic ownership.
    /// </summary>
    [DefaultExecutionOrder(-800)]
    [DisallowMultipleComponent]
    public sealed partial class BDModernHandheldV6Polish : MonoBehaviour
    {
        private const float DeviceYOffset = -0.34f;
        private const float ScreenWidth = 7.82f;
        private const float ScreenHeight = 9.04f;
        private const float ScreenCenterY = 2.56f;
        private static readonly Vector2 CanvasSize =
            new Vector2(960f, 1080f);

        private static BDModernHandheldV6Polish instance;
        private BDModernHandheld3DPresenter presenter;
        private Transform presentationRoot;
        private Transform deviceRoot;
        private Transform offsetRoot;
        private RectTransform currentPage;
        private int currentPageId;
        private string lastSelectedRow = string.Empty;
        private bool controlsReady;
        private bool tactileReady;

        [RuntimeInitializeOnLoadMethod(
            RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void Install()
        {
            if (FindFirstObjectByType<BDModernHandheldV6Polish>() != null)
                return;

            GameObject root = new GameObject(
                "B&D Modern Handheld V6 Polish"
            );
            DontDestroyOnLoad(root);
            root.AddComponent<BDModernHandheldV6Polish>();
        }

        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
                return;
            }

            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void OnDestroy()
        {
            if (instance == this)
                instance = null;
        }

        private void LateUpdate()
        {
            if (!ResolvePresenter())
                return;

            LowerDeviceAndShadows();
            PolishPhysicalControls();
            ApplySharedTactileProfile();
            RefreshPage();
        }

        private bool ResolvePresenter()
        {
            if (presenter == null)
            {
                presenter = FindFirstObjectByType<
                    BDModernHandheld3DPresenter>();
                presentationRoot = null;
                deviceRoot = null;
                offsetRoot = null;
                currentPage = null;
                currentPageId = 0;
                controlsReady = false;
                tactileReady = false;
            }

            if (presenter == null)
                return false;

            if (presentationRoot == null)
            {
                presentationRoot = Find(
                    presenter.transform,
                    "Modern Handheld Presentation"
                );
            }

            if (presentationRoot == null)
                return false;

            if (deviceRoot == null)
            {
                deviceRoot = Find(
                    presentationRoot,
                    "BD Modern Upright Handheld"
                );
            }

            return deviceRoot != null;
        }

        private void RefreshPage()
        {
            RectTransform page = FindActivePage();
            if (page == null)
            {
                currentPage = null;
                currentPageId = 0;
                lastSelectedRow = string.Empty;
                return;
            }

            int id = page.GetInstanceID();
            if (currentPage == null || id != currentPageId)
            {
                currentPage = page;
                currentPageId = id;
                lastSelectedRow = string.Empty;
                PolishPage(page);
            }

            if (page.name == "Page MainMenu")
                UpdateContextCard(page);
        }

        private RectTransform FindActivePage()
        {
            RectTransform[] rects =
                presenter.GetComponentsInChildren<RectTransform>(true);
            for (int index = 0; index < rects.Length; index++)
            {
                RectTransform rect = rects[index];
                if (rect != null &&
                    rect.gameObject.activeInHierarchy &&
                    rect.name.StartsWith(
                        "Page ",
                        StringComparison.Ordinal))
                {
                    return rect;
                }
            }

            return null;
        }
    }
}
