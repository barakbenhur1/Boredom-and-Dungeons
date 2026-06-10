using UnityEngine;

namespace BoredomAndDungeons
{
    public enum BDFirstLaunchTutorialStatus
    {
        NotStarted = 0,
        InProgress = 1,
        Completed = 2,
        Skipped = 3
    }

    /// <summary>
    /// Owns the durable, versioned first-launch tutorial completion decision.
    /// Completed and Skipped are terminal display states. InProgress is purposely
    /// non-terminal so an interrupted tutorial restarts safely on the next launch.
    /// </summary>
    public static class BDFirstLaunchTutorialStateStore
    {
        private const string StatusKey =
            "BoredomAndDungeons.FirstLaunchTutorial.Status.v1";

        public static BDFirstLaunchTutorialStatus Status
        {
            get
            {
                int value = PlayerPrefs.GetInt(
                    StatusKey,
                    (int)BDFirstLaunchTutorialStatus.NotStarted
                );

                if (value < (int)BDFirstLaunchTutorialStatus.NotStarted ||
                    value > (int)BDFirstLaunchTutorialStatus.Skipped)
                {
                    return BDFirstLaunchTutorialStatus.NotStarted;
                }

                return (BDFirstLaunchTutorialStatus)value;
            }
        }

        public static bool ShouldPresent
        {
            get
            {
                BDFirstLaunchTutorialStatus status = Status;
                return status == BDFirstLaunchTutorialStatus.NotStarted ||
                       status == BDFirstLaunchTutorialStatus.InProgress;
            }
        }

        public static void MarkInProgress()
        {
            Write(BDFirstLaunchTutorialStatus.InProgress);
        }

        public static void MarkCompleted()
        {
            Write(BDFirstLaunchTutorialStatus.Completed);
        }

        public static void MarkSkipped()
        {
            // Persist before any visual transition. A crash after confirmation
            // must never cause the tutorial to appear automatically again.
            Write(BDFirstLaunchTutorialStatus.Skipped);
        }

#if UNITY_EDITOR
        public static void ResetForDevelopment()
        {
            PlayerPrefs.DeleteKey(StatusKey);
            PlayerPrefs.Save();
        }
#endif

        private static void Write(BDFirstLaunchTutorialStatus status)
        {
            PlayerPrefs.SetInt(StatusKey, (int)status);
            PlayerPrefs.Save();
        }
    }
}
