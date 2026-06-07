namespace BoredomAndDungeons
{
    /// <summary>
    /// Compatibility bridge kept for older runtime callers.
    /// BDRunPresentationCoordinator is the single owner of run-entry presentation.
    /// This type must never bootstrap a second intro, portal, door, or camera flow.
    /// </summary>
    public static class BDMountedRunIntro
    {
        // BD SINGLE RUN-PRESENTATION OWNER V13
        public static bool IsGameplayInputLocked =>
            BDRunPresentationCoordinator.InputLocked;
    }
}
