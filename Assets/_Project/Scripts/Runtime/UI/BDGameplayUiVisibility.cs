using UnityEngine;

namespace BoredomAndDungeons
{
    public static class BDGameplayUiVisibility
    {
        // BD SINGLE GAMEPLAY UI VISIBILITY OWNER V23R10
        public static bool IsGameplayHudVisible
        {
            get
            {
                if (!Application.isPlaying)
                    return false;

                BDMainMenuFlow flow = BDMainMenuFlow.Instance;
                if (flow == null || !flow.IsGameplayHudVisible)
                    return false;

                if (BDBBHBootIntro.IsPlaying)
                    return false;

                return true;
            }
        }

        public static bool IsTargetingVisible =>
            IsGameplayHudVisible &&
            !BDMountedRunIntro.IsGameplayInputLocked &&
            !BDNewRunFeedbackReset.IsCombatInputSuppressed &&
            !BDParrySystem.IsActive;
    }
}
