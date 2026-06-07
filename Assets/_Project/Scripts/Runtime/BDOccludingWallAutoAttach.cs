using UnityEngine;

namespace BoredomAndDungeons
{
    public static class BDOccludingWallAutoAttach
    {
        // BD DO NOT ATTACH FADER TO STRUCTURAL WALLS V20
        // BD SANITIZE LEGACY STRUCTURAL WALL FADERS V22
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void Attach()
        {
            BDOccludingWall[] existingWalls =
                Object.FindObjectsByType<BDOccludingWall>(
                    FindObjectsInactive.Include,
                    FindObjectsSortMode.None
                );
            for (int i = 0; i < existingWalls.Length; i++)
            {
                BDOccludingWall existing = existingWalls[i];
                if (existing == null || !IsStructuralRoomBoundary(existing.transform))
                    continue;

                existing.ForceOpaqueImmediateAndDisableFading();
            }

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

                if (go.GetComponentInParent<BDWallSurfaceProfile>() != null)
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

        private static bool IsStructuralRoomBoundary(Transform value)
        {
            if (value == null)
                return false;

            if (value.GetComponentInParent<BDWallSurfaceProfile>() != null)
                return true;

            string name = value.name.ToLowerInvariant();
            return name.Contains("roomwall") ||
                   name.Contains("boundary") ||
                   name.Contains("cavewall") ||
                   name.Contains("rockwall") ||
                   name.StartsWith("wall_");
        }
    }
}
