using UnityEngine;
using UnityEngine.SceneManagement;

namespace BoredomAndDungeons
{
    [DefaultExecutionOrder(10000)]
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Camera))]
    public sealed class BDCameraForwardViewBias : MonoBehaviour
    {
        [Header("Viewport Composition")]
        [SerializeField] private Vector2 desiredPlayerViewport =
            new Vector2(0.50f, 0.40f);

        [SerializeField] private float playerFocusHeight = 0.65f;
        [SerializeField] private float smoothingSpeed = 10f;
        [SerializeField] private float maximumWorldCorrection = 14f;

        private static bool sceneHookInstalled;

        private Camera targetCamera;
        private Transform player;
        private CharacterController playerCharacterController;

        private Vector3 currentOffset;
        private Vector3 appliedOffset;

        [RuntimeInitializeOnLoadMethod(
            RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void InstallSceneHook()
        {
            if (sceneHookInstalled)
                return;

            sceneHookInstalled = true;
            SceneManager.sceneLoaded += HandleSceneLoaded;
        }

        [RuntimeInitializeOnLoadMethod(
            RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void InstallOnActiveCamera()
        {
            TryInstall();
        }

        private static void HandleSceneLoaded(
            Scene scene,
            LoadSceneMode mode)
        {
            TryInstall();
        }

        private static void TryInstall()
        {
            Camera camera = Camera.main;

            if (camera == null)
            {
                camera =
                    Object.FindFirstObjectByType<Camera>();
            }

            if (camera == null ||
                camera.GetComponent<
                    BDCameraForwardViewBias>() != null)
            {
                return;
            }

            camera.gameObject.AddComponent<
                BDCameraForwardViewBias>();
        }

        private void Awake()
        {
            targetCamera = GetComponent<Camera>();
        }

        private void OnEnable()
        {
            ResolvePlayer();
        }

        private void Update()
        {
            RemovePreviouslyAppliedOffset();
        }

        private void LateUpdate()
        {
            ResolvePlayer();

            Vector3 desiredOffset = Vector3.zero;

            if (targetCamera != null &&
                player != null)
            {
                Vector3 focusPoint =
                    ResolvePlayerFocusPoint();

                Vector3 viewport =
                    targetCamera.WorldToViewportPoint(
                        focusPoint
                    );

                if (viewport.z > 0.01f)
                {
                    float worldHeight;
                    float worldWidth;

                    if (targetCamera.orthographic)
                    {
                        worldHeight =
                            targetCamera.orthographicSize *
                            2f;
                    }
                    else
                    {
                        worldHeight =
                            2f *
                            viewport.z *
                            Mathf.Tan(
                                targetCamera.fieldOfView *
                                0.5f *
                                Mathf.Deg2Rad
                            );
                    }

                    worldWidth =
                        worldHeight *
                        Mathf.Max(
                            0.01f,
                            targetCamera.aspect
                        );

                    float horizontalError =
                        viewport.x -
                        Mathf.Clamp01(
                            desiredPlayerViewport.x
                        );

                    float verticalError =
                        viewport.y -
                        Mathf.Clamp01(
                            desiredPlayerViewport.y
                        );

                    desiredOffset =
                        targetCamera.transform.right *
                            horizontalError *
                            worldWidth +
                        targetCamera.transform.up *
                            verticalError *
                            worldHeight;

                    desiredOffset = Vector3.ClampMagnitude(
                        desiredOffset,
                        Mathf.Max(
                            0.25f,
                            maximumWorldCorrection
                        )
                    );
                }
            }

            float interpolation =
                1f -
                Mathf.Exp(
                    -Mathf.Max(
                        0.01f,
                        smoothingSpeed
                    ) *
                    Time.unscaledDeltaTime
                );

            currentOffset = Vector3.Lerp(
                currentOffset,
                desiredOffset,
                interpolation
            );

            appliedOffset = currentOffset;
            transform.position += appliedOffset;
        }

        private void OnDisable()
        {
            RemovePreviouslyAppliedOffset();
            currentOffset = Vector3.zero;
        }

        private void OnDestroy()
        {
            RemovePreviouslyAppliedOffset();
        }

        private void RemovePreviouslyAppliedOffset()
        {
            if (appliedOffset.sqrMagnitude <= 0.000001f)
                return;

            transform.position -= appliedOffset;
            appliedOffset = Vector3.zero;
        }

        private void ResolvePlayer()
        {
            if (player != null)
                return;

            BDPlayerMarker marker =
                Object.FindFirstObjectByType<
                    BDPlayerMarker>();

            if (marker != null)
                player = marker.transform;
            else
                player = BDTargetFinder.FindPlayer();

            if (player != null)
            {
                playerCharacterController =
                    player.GetComponent<
                        CharacterController>();
            }
        }

        private Vector3 ResolvePlayerFocusPoint()
        {
            if (playerCharacterController != null &&
                playerCharacterController.enabled)
            {
                return playerCharacterController
                    .bounds.center;
            }

            return player.position +
                   Vector3.up *
                   Mathf.Max(
                       0f,
                       playerFocusHeight
                   );
        }
    }
}
