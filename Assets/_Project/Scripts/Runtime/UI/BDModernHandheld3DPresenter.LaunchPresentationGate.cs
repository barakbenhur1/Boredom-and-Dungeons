using UnityEngine;

namespace BoredomAndDungeons
{
    public sealed partial class BDModernHandheld3DPresenter
    {
        private bool launchPresentationReservation;

        private void InitializeLaunchPresentationGate()
        {
            launchPresentationReservation =
                !BDBBHBootIntro.HasPlayedThisSession;
        }

        private bool ShouldReserveLaunchPresentation()
        {
            return launchPresentationReservation ||
                   ShouldReserveFirstLaunchTutorialPresentation();
        }

        private void TickLaunchPresentationGate()
        {
            if (!launchPresentationReservation)
                return;

            if (flow == null ||
                BDBBHBootIntro.IsPlaying ||
                !BDBBHBootIntro.HasPlayedThisSession)
            {
                return;
            }

            // The modern menu owner is now resolved and the boot overlay has
            // finished. Releasing here means the first exposed frame already
            // belongs to the correct handheld page.
            launchPresentationReservation = false;
        }
    }
}
