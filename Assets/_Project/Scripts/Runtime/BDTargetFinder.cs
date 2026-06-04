using UnityEngine;

namespace BoredomAndDungeons
{
    public static class BDTargetFinder
    {
        private static Transform cachedPlayer;
        private static float nextSearchAt;

        public static Transform FindPlayer()
        {
            if (cachedPlayer != null)
                return cachedPlayer;

            if (Application.isPlaying && Time.time < nextSearchAt)
                return null;

            nextSearchAt = Application.isPlaying ? Time.time + 0.25f : 0f;

            BDPlayerMarker marker = Object.FindFirstObjectByType<BDPlayerMarker>();
            if (marker != null)
            {
                cachedPlayer = marker.transform;
                return cachedPlayer;
            }

            GameObject byName = GameObject.Find("BD_Player") ?? GameObject.Find("Player");
            cachedPlayer = byName != null ? byName.transform : null;
            return cachedPlayer;
        }

        public static void ClearCache()
        {
            cachedPlayer = null;
            nextSearchAt = 0f;
        }
    }
}
