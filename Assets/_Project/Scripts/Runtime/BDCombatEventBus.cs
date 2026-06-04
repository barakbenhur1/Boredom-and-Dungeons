using UnityEngine;

namespace BoredomAndDungeons
{
    public static class BDCombatEventBus
    {
        public static event System.Action RoomCleared;
        private static string activeTitle = "";
        private static string activeSubtitle = "";
        private static float messageEndsAt;
        private static float messageStartedAt;
        private static float messageDuration = 1f;

        public static int TotalKills { get; private set; }
        public static string ActiveTitle => activeTitle;
        public static string ActiveSubtitle => activeSubtitle;

        public static bool HasActiveMessage
        {
            get
            {
                return Application.isPlaying && Time.time < messageEndsAt && !string.IsNullOrEmpty(activeTitle);
            }
        }

        public static float MessageProgress01
        {
            get
            {
                if (!HasActiveMessage)
                    return 0f;

                float elapsed = Time.time - messageStartedAt;
                return Mathf.Clamp01(elapsed / Mathf.Max(0.01f, messageDuration));
            }
        }

        public static void NotifyEnemyKilled(BDHealth enemy)
        {
            TotalKills++;
        }

        public static void NotifyRoomClear(string subtitle = "Exit is open")
        {
            // No HUD text. This event is visual/system-only.
            RoomCleared?.Invoke();
        }

        public static void ShowMessage(string title, string subtitle, float duration)
        {
            activeTitle = title ?? "";
            activeSubtitle = subtitle ?? "";
            messageDuration = Mathf.Max(0.1f, duration);
            messageStartedAt = Time.time;
            messageEndsAt = Time.time + messageDuration;
        }

        public static void Reset()
        {
            TotalKills = 0;
            activeTitle = "";
            activeSubtitle = "";
            messageEndsAt = 0f;
            messageStartedAt = 0f;
            messageDuration = 1f;
        }
    }
}
