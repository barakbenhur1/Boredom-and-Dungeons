using UnityEngine;
using UnityEngine.SceneManagement;

namespace BoredomAndDungeons
{
    public static class BDHorseRuntimeRepair
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void InstallAfterSceneLoad()
        {
            RepairAllHorses();
            SceneManager.sceneLoaded -= HandleSceneLoaded;
            SceneManager.sceneLoaded += HandleSceneLoaded;
        }

        private static void HandleSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            RepairAllHorses();
        }

        public static void RepairAllHorses()
        {
            BDHorseHealth[] horses = Object.FindObjectsByType<BDHorseHealth>(FindObjectsSortMode.None);

            for (int i = 0; i < horses.Length; i++)
            {
                BDHorseHealth horseHealth = horses[i];
                if (horseHealth == null)
                    continue;

                GameObject horse = horseHealth.gameObject;

                EnsureComponent<BDHorseWorldStatusIndicator>(horse);
                EnsureComponent<BDHorseHealAvailabilityIndicator>(horse);
                EnsureComponent<BDHorseNeedsHealingEffect>(horse);
                EnsureComponent<BDHorseBuckRepair>(horse);

                if (horse.GetComponent<BDHorseController>() != null)
                {
                    EnsureComponent<BDHorseCombatFleeController>(horse);
                    EnsureComponent<BDHorseReliableFleeMotor>(horse);
                    EnsureComponent<BDHorseExhaustedFollowAndPetInteraction>(
                        horse
                    );
                }
            }
        }

        private static T EnsureComponent<T>(GameObject target) where T : Component
        {
            T component = target.GetComponent<T>();
            return component != null ? component : target.AddComponent<T>();
        }
    }
}
