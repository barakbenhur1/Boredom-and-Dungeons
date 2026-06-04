using UnityEngine;

namespace BoredomAndDungeons
{
    public static class BDOccludingWallAutoAttach
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void Attach()
        {
            Renderer[] renderers = Object.FindObjectsByType<Renderer>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);

            for (int i = 0; i < renderers.Length; i++)
            {
                Renderer renderer = renderers[i];
                if (renderer == null)
                    continue;

                GameObject go = renderer.gameObject;
                if (go == null)
                    continue;

                if (go.GetComponent<BDOccludingWall>() != null)
                    continue;

                string name = go.name;
                if (string.IsNullOrEmpty(name))
                    continue;

                bool likelyWall =
                    name.Contains("Wall") ||
                    name.Contains("Pillar") ||
                    name.Contains("Column") ||
                    name.Contains("Rock") ||
                    name.Contains("Block");

                if (!likelyWall)
                    continue;

                if (go.GetComponent<BDHealth>() != null)
                    continue;

                if (go.GetComponent<BDPlayerMarker>() != null)
                    continue;

                if (go.GetComponent<BDHorseHealth>() != null)
                    continue;

                go.AddComponent<BDOccludingWall>();
            }
        }
    }
}
