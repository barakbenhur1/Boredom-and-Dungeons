using UnityEngine;

namespace BoredomAndDungeons
{
    public static class BDCameraOcclusionAutoAttach
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void Attach()
        {
            Camera camera = Camera.main;
            if (camera == null)
                return;

            if (camera.GetComponent<BDCameraOccluderFader>() == null)
                camera.gameObject.AddComponent<BDCameraOccluderFader>();
        }
    }
}
