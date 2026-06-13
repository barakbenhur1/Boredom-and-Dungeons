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

                // BD FIRST-LAUNCH PET/HUD HARD GATE V10.11.30.27
                // The first-launch reservation is authoritative even during
                // presenter setup/handoffs. Full-game horse prompts, including
                // the PET card in the upper-right corner, must never render.
                if (BDModernHandheld3DPresenter.SuppressFirstLaunchGameplayHud ||
                    BDModernHandheld3DPresenter.SuppressLegacyMenu)
                {
                    return false;
                }

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
