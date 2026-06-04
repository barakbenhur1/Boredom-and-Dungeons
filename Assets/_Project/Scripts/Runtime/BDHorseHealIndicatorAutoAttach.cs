using UnityEngine;

namespace BoredomAndDungeons
{
    public static class BDHorseHealIndicatorAutoAttach
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void Attach()
        {
            if (!Application.isPlaying)
                return;

            BDHorseHealth[] horses = Object.FindObjectsByType<BDHorseHealth>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);

            for (int i = 0; i < horses.Length; i++)
            {
                BDHorseHealth horse = horses[i];
                if (horse == null)
                    continue;

                if (horse.GetComponent<BDHorseHealAvailabilityIndicator>() == null)
                    horse.gameObject.AddComponent<BDHorseHealAvailabilityIndicator>();
            }
        }
    }
}
