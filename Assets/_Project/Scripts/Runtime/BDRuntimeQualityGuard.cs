using UnityEngine;

namespace BoredomAndDungeons
{
    public sealed class BDRuntimeQualityGuard : MonoBehaviour
    {
        [SerializeField] private int targetFrameRate = 60;
        [SerializeField] private bool setTargetFrameRate = true;
        [SerializeField] private bool disableVSyncInEditorPlay = false;

        private static BDRuntimeQualityGuard instance;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void Bootstrap()
        {
            if (instance != null)
                return;

            GameObject go = new GameObject("BD_RuntimeQualityGuard");
            if (!Application.isPlaying)
            {
                Object.DestroyImmediate(go);
                return;
            }

            DontDestroyOnLoad(go);
            instance = go.AddComponent<BDRuntimeQualityGuard>();
        }

        private void Awake()
        {
            Apply();
        }

        private void Apply()
        {
            if (setTargetFrameRate)
                Application.targetFrameRate = Mathf.Clamp(targetFrameRate, 30, 120);

#if UNITY_EDITOR
            if (disableVSyncInEditorPlay)
                QualitySettings.vSyncCount = 0;
#endif
        }
    }
}
