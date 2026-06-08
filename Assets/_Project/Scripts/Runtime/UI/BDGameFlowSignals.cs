using UnityEngine;

namespace BoredomAndDungeons
{
    public static class BDGameFlowSignals
    {
        public static bool TryHandleDeath(
            BDHealth health)
        {
            if (health == null ||
                health.GetComponent<BDPlayerMarker>() ==
                    null)
            {
                return false;
            }

            BDMainMenuFlow flow =
                ResolveFlow();

            if (flow == null ||
                !flow.IsRunActive)
            {
                return false;
            }

            return flow.HandlePlayerDeath(health);
        }

        public static void BeginResultSequence()
        {
            BDMainMenuFlow flow =
                ResolveFlow();

            if (flow != null)
                flow.BeginResultSequence();
        }

        public static void ReturnToMainMenuAfterSequence()
        {
            BDMainMenuFlow flow =
                ResolveFlow();

            if (flow != null)
            {
                flow.ReturnToMainMenuAfterSequence();
            }
        }

        public static void CompleteMotherVictorySequence()
        {
            BDMainMenuFlow flow =
                ResolveFlow();

            if (flow != null)
            {
                flow.CompleteMotherVictorySequence();
            }
        }

        // Compatibility wrappers. They intentionally do not add result text.
        public static void ShowDefeat()
        {
            ReturnToMainMenuAfterSequence();
        }

        public static void ShowVictory()
        {
            CompleteMotherVictorySequence();
        }

        public static void ReturnToMainMenu()
        {
            BDMainMenuFlow flow =
                ResolveFlow();

            if (flow != null)
                flow.ReturnToMainMenu();
        }

        private static BDMainMenuFlow ResolveFlow()
        {
            if (BDMainMenuFlow.Instance != null)
                return BDMainMenuFlow.Instance;

            return Object.FindFirstObjectByType<
                BDMainMenuFlow>();
        }
    }
}
