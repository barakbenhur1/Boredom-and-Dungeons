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

                // The modern handheld owns the whole presentation during the
                // first-launch tutorial and menus. Gameplay HUD prompts, including
                // the horse Pet card, must never leak outside the device.
                if (BDModernHandheld3DPresenter.SuppressLegacyMenu)
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
