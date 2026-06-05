using UnityEngine;
using UnityEngine.SceneManagement;

namespace BoredomAndDungeons
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(BDPlayerMarker))]
    public sealed class BDPlayerVisualProportion : MonoBehaviour
    {
        private const string VisualChildName = "BD_Player_Visual";

        [Header("Visual proportions only")]
        [SerializeField, Range(0.70f, 1f)] private float widthScale = 0.90f;
        [SerializeField, Range(0.75f, 1f)] private float depthScale = 0.94f;
        [SerializeField, Range(0.90f, 1.10f)] private float heightScale = 1.00f;

        private Transform visualRoot;

        private void Awake()
        {
            EnsureSeparateVisual();
            ApplyScale();
        }

        private void OnValidate()
        {
            widthScale = Mathf.Clamp(widthScale, 0.70f, 1f);
            depthScale = Mathf.Clamp(depthScale, 0.75f, 1f);
            heightScale = Mathf.Clamp(heightScale, 0.90f, 1.10f);

            if (Application.isPlaying)
            {
                EnsureSeparateVisual();
                ApplyScale();
            }
        }

        private void EnsureSeparateVisual()
        {
            Transform existing = transform.Find(VisualChildName);
            if (existing != null)
            {
                visualRoot = existing;
                return;
            }

            MeshFilter sourceMeshFilter = GetComponent<MeshFilter>();
            Renderer sourceRenderer = GetComponent<Renderer>();

            if (sourceMeshFilter == null || sourceMeshFilter.sharedMesh == null || sourceRenderer == null)
                return;

            GameObject visualObject = new GameObject(VisualChildName);
            visualRoot = visualObject.transform;
            visualRoot.SetParent(transform, false);
            visualRoot.localPosition = Vector3.zero;
            visualRoot.localRotation = Quaternion.identity;

            MeshFilter visualMeshFilter = visualObject.AddComponent<MeshFilter>();
            visualMeshFilter.sharedMesh = sourceMeshFilter.sharedMesh;

            MeshRenderer visualRenderer = visualObject.AddComponent<MeshRenderer>();
            visualRenderer.sharedMaterials = sourceRenderer.sharedMaterials;
            visualRenderer.shadowCastingMode = sourceRenderer.shadowCastingMode;
            visualRenderer.receiveShadows = sourceRenderer.receiveShadows;
            visualRenderer.lightProbeUsage = sourceRenderer.lightProbeUsage;
            visualRenderer.reflectionProbeUsage = sourceRenderer.reflectionProbeUsage;

            sourceRenderer.enabled = false;
        }

        private void ApplyScale()
        {
            if (visualRoot == null)
                return;

            visualRoot.localScale = new Vector3(widthScale, heightScale, depthScale);
        }
    }

    public static class BDPlayerVisualProportionInstaller
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void InstallAfterSceneLoad()
        {
            InstallForAllPlayers();
            SceneManager.sceneLoaded -= HandleSceneLoaded;
            SceneManager.sceneLoaded += HandleSceneLoaded;
        }

        private static void HandleSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            InstallForAllPlayers();
        }

        private static void InstallForAllPlayers()
        {
            BDPlayerMarker[] players = Object.FindObjectsByType<BDPlayerMarker>(FindObjectsSortMode.None);

            for (int i = 0; i < players.Length; i++)
            {
                BDPlayerMarker player = players[i];
                if (player == null)
                    continue;

                if (player.GetComponent<BDPlayerVisualProportion>() == null)
                    player.gameObject.AddComponent<BDPlayerVisualProportion>();
            }
        }
    }
}
