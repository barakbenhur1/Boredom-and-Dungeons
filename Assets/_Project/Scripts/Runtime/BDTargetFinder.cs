using UnityEngine;

namespace BoredomAndDungeons
{
    public static class BDTargetFinder
    {
        private static Transform cachedPlayer;
        private static float nextSearchAt;
        public static Transform FindPlayer()
        {
            if (IsValidPlayer(cachedPlayer))
                return cachedPlayer;

            cachedPlayer = null;

            if (Application.isPlaying &&
                Time.unscaledTime < nextSearchAt)
            {
                return null;
            }

            nextSearchAt =
                Application.isPlaying
                    ? Time.unscaledTime + 0.08f
                    : 0f;

            BDPlayerMarker marker =
                Object.FindFirstObjectByType<BDPlayerMarker>();

            if (marker != null &&
                marker.gameObject.activeInHierarchy)
            {
                cachedPlayer = marker.transform;
                return cachedPlayer;
            }

            GameObject byName =
                GameObject.Find("BD_Player") ??
                GameObject.Find("Player");

            cachedPlayer =
                byName != null &&
                byName.activeInHierarchy
                    ? byName.transform
                    : null;

            return cachedPlayer;
        }
        private static bool IsValidPlayer(
            Transform candidate)
        {
            return candidate != null &&
                   candidate.gameObject.activeInHierarchy &&
                   candidate.GetComponent<BDPlayerMarker>() != null;
        }


        public static void ClearCache()
        {
            cachedPlayer = null;
            nextSearchAt = 0f;
        }
    }
}
