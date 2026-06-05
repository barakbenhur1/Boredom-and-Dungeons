using UnityEngine;
using UnityEngine.SceneManagement;

namespace BoredomAndDungeons
{
    [DefaultExecutionOrder(30000)]
    [DisallowMultipleComponent]
    public sealed class BDMinimapPerspectiveAlignment : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Camera gameplayCamera;
        [SerializeField] private Camera minimapCamera;
        [SerializeField] private Transform player;

        [Header("Alignment")]
        [SerializeField] private bool alignMapTopToGameplayView = true;
        [SerializeField] private bool followPlayerPosition = true;
        [SerializeField] private float yawOffsetDegrees;
        [SerializeField] private float cameraResolveInterval = 0.75f;

        [Header("Detection")]
        [SerializeField] private bool logResolvedCameras;

        private float nextResolveAtUnscaled;
        private bool loggedMissingMinimap;
        private bool loggedResolution;

        public Camera GameplayCamera => gameplayCamera;
        public Camera MinimapCamera => minimapCamera;

        private void Awake()
        {
            ResolveReferences(force: true);
        }

        private void LateUpdate()
        {
            ResolveReferences(force: false);

            if (minimapCamera == null)
                return;

            if (followPlayerPosition && player != null)
            {
                Vector3 position = minimapCamera.transform.position;
                position.x = player.position.x;
                position.z = player.position.z;
                minimapCamera.transform.position = position;
            }

            if (!alignMapTopToGameplayView || gameplayCamera == null)
                return;

            Vector3 screenUpOnGround =
                Vector3.ProjectOnPlane(
                    gameplayCamera.transform.up,
                    Vector3.up
                );

            if (screenUpOnGround.sqrMagnitude < 0.001f)
            {
                screenUpOnGround =
                    Vector3.ProjectOnPlane(
                        gameplayCamera.transform.forward,
                        Vector3.up
                    );
            }

            if (screenUpOnGround.sqrMagnitude < 0.001f &&
                player != null)
            {
                screenUpOnGround =
                    Vector3.ProjectOnPlane(
                        player.forward,
                        Vector3.up
                    );
            }

            if (screenUpOnGround.sqrMagnitude < 0.001f)
                return;

            screenUpOnGround.Normalize();

            if (Mathf.Abs(yawOffsetDegrees) > 0.001f)
            {
                screenUpOnGround =
                    Quaternion.AngleAxis(
                        yawOffsetDegrees,
                        Vector3.up
                    ) * screenUpOnGround;
            }

            minimapCamera.transform.rotation =
                Quaternion.LookRotation(
                    Vector3.down,
                    screenUpOnGround
                );
        }

        public void ResolveReferences(bool force)
        {
            if (!force &&
                Time.unscaledTime < nextResolveAtUnscaled &&
                gameplayCamera != null &&
                minimapCamera != null &&
                player != null)
            {
                return;
            }

            nextResolveAtUnscaled =
                Time.unscaledTime +
                Mathf.Max(0.15f, cameraResolveInterval);

            Camera[] cameras =
                Object.FindObjectsByType<Camera>(
                    FindObjectsSortMode.None
                );

            if (gameplayCamera == null || force)
                gameplayCamera = ResolveGameplayCamera(cameras);

            if (minimapCamera == null ||
                minimapCamera == gameplayCamera ||
                force)
            {
                minimapCamera =
                    ResolveMinimapCamera(
                        cameras,
                        gameplayCamera
                    );
            }

            if (player == null || force)
            {
                BDPlayerMarker marker =
                    Object.FindFirstObjectByType<BDPlayerMarker>();

                player = marker != null
                    ? marker.transform
                    : null;
            }

            if (minimapCamera == null)
            {
                if (!loggedMissingMinimap)
                {
                    Debug.LogWarning(
                        "B&D minimap alignment could not find a " +
                        "minimap camera. Name it MinimapCamera, or use " +
                        "an orthographic camera with a RenderTexture.",
                        this
                    );

                    loggedMissingMinimap = true;
                }

                return;
            }

            loggedMissingMinimap = false;

            if (logResolvedCameras && !loggedResolution)
            {
                Debug.Log(
                    $"B&D minimap aligned. Gameplay camera: " +
                    $"{(gameplayCamera != null ? gameplayCamera.name : "none")}; " +
                    $"minimap camera: {minimapCamera.name}.",
                    this
                );

                loggedResolution = true;
            }
        }

        public void Configure(
            Camera newGameplayCamera,
            Camera newMinimapCamera,
            Transform newPlayer)
        {
            gameplayCamera = newGameplayCamera;
            minimapCamera = newMinimapCamera;
            player = newPlayer;
            loggedResolution = false;
        }

        private static Camera ResolveGameplayCamera(Camera[] cameras)
        {
            Camera main = Camera.main;

            if (main != null && !LooksLikeMinimapCamera(main))
                return main;

            Camera best = null;
            float bestScore = float.NegativeInfinity;

            if (cameras == null)
                return null;

            for (int i = 0; i < cameras.Length; i++)
            {
                Camera candidate = cameras[i];

                if (candidate == null ||
                    !candidate.isActiveAndEnabled ||
                    LooksLikeMinimapCamera(candidate))
                {
                    continue;
                }

                float score = 0f;

                if (candidate.targetTexture == null)
                    score += 80f;

                score +=
                    candidate.rect.width *
                    candidate.rect.height *
                    25f;

                if (!candidate.orthographic)
                    score += 15f;

                if (score > bestScore)
                {
                    bestScore = score;
                    best = candidate;
                }
            }

            return best;
        }

        private static Camera ResolveMinimapCamera(
            Camera[] cameras,
            Camera gameplay)
        {
            Camera best = null;
            float bestScore = float.NegativeInfinity;

            if (cameras == null)
                return null;

            for (int i = 0; i < cameras.Length; i++)
            {
                Camera candidate = cameras[i];

                if (candidate == null ||
                    candidate == gameplay ||
                    !candidate.isActiveAndEnabled)
                {
                    continue;
                }

                string lowerName =
                    candidate.name.ToLowerInvariant();

                bool explicitName =
                    lowerName.Contains("minimap") ||
                    lowerName.Contains("mini_map") ||
                    lowerName.Contains("mini map") ||
                    (lowerName.Contains("mini") &&
                     lowerName.Contains("map"));

                bool renderTextureMap =
                    candidate.orthographic &&
                    candidate.targetTexture != null;

                bool viewportMap =
                    candidate.orthographic &&
                    candidate.rect.width <= 0.55f &&
                    candidate.rect.height <= 0.55f;

                if (!explicitName &&
                    !renderTextureMap &&
                    !viewportMap)
                {
                    continue;
                }

                float score = 0f;

                if (explicitName)
                    score += 220f;

                if (renderTextureMap)
                    score += 100f;

                if (viewportMap)
                    score += 45f;

                if (candidate.orthographic)
                    score += 30f;

                if (score > bestScore)
                {
                    bestScore = score;
                    best = candidate;
                }
            }

            return best;
        }

        private static bool LooksLikeMinimapCamera(Camera camera)
        {
            if (camera == null)
                return false;

            string lowerName = camera.name.ToLowerInvariant();

            if (lowerName.Contains("minimap") ||
                lowerName.Contains("mini_map") ||
                lowerName.Contains("mini map"))
            {
                return true;
            }

            return camera.orthographic &&
                   camera.targetTexture != null;
        }
    }

    public static class BDMinimapPerspectiveAlignmentInstaller
    {
        [RuntimeInitializeOnLoadMethod(
            RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void InstallAfterSceneLoad()
        {
            Install();

            SceneManager.sceneLoaded -= HandleSceneLoaded;
            SceneManager.sceneLoaded += HandleSceneLoaded;
        }

        private static void HandleSceneLoaded(
            Scene scene,
            LoadSceneMode mode)
        {
            Install();
        }

        private static void Install()
        {
            BDMinimapPerspectiveAlignment existing =
                Object.FindFirstObjectByType<
                    BDMinimapPerspectiveAlignment
                >();

            if (existing != null)
            {
                existing.ResolveReferences(force: true);
                return;
            }

            GameObject root =
                new GameObject(
                    "BD_Minimap_Perspective_Alignment"
                );

            root.AddComponent<
                BDMinimapPerspectiveAlignment
            >();
        }
    }
}
