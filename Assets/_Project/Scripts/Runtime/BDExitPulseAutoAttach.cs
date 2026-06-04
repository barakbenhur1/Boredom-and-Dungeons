using UnityEngine;

namespace BoredomAndDungeons
{
    public static class BDExitPulseAutoAttach
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void AttachToExistingExitObjects()
        {
            GameObject[] all = Object.FindObjectsByType<GameObject>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);

            for (int i = 0; i < all.Length; i++)
            {
                GameObject go = all[i];
                if (go == null)
                    continue;

                string name = go.name;
                if (string.IsNullOrEmpty(name))
                    continue;

                if (!name.Contains("Exit") && !name.Contains("Goal") && !name.Contains("Door"))
                    continue;

                if (go.GetComponentInChildren<Renderer>() == null)
                    continue;

                if (go.GetComponent<BDExitOpenPulse>() == null)
                    go.AddComponent<BDExitOpenPulse>();
            }
        }
    }
}
