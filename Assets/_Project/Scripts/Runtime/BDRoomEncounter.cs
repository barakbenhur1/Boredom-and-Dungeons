using System.Collections.Generic;
using UnityEngine;

namespace BoredomAndDungeons
{
    [DisallowMultipleComponent]
    public sealed class BDRoomEncounter : MonoBehaviour
    {
        [SerializeField] private bool startActive = true;
        [SerializeField] private List<BDHealth> enemyHealth = new List<BDHealth>();
        [SerializeField] private List<BDExitBlockerEnemy> exitBlockers = new List<BDExitBlockerEnemy>();
        [SerializeField] private float exitAttemptCooldown = 1f;
        [SerializeField] private float blockerCommandDuration = 4f;
        [SerializeField] private GameObject exitWarningMarker;
        [SerializeField] private bool showDebugOverlay = false;

        private bool active;
        private bool complete;
        private float exitCooldown;
        private int liveEnemies;
        private int exitAttempts;
        private string lastExit = "none";
        private BDHorseController horse;
        private bool clearAnnounced;

        public bool IsComplete => complete;
        public int LiveEnemies => liveEnemies;

        private void Start()
        {
            active = startActive;
            horse = FindFirstObjectByType<BDHorseController>();
            RefreshLiveEnemies();

            if (active && horse != null)
                horse.ForceDismountForCombat();

            if (exitWarningMarker != null)
                exitWarningMarker.SetActive(false);
        }

        private void Update()
        {
            if (exitCooldown > 0f)
                exitCooldown -= Time.deltaTime;

            RefreshLiveEnemies();

            if (active && !complete && liveEnemies <= 0)
            {
                complete = true;
                active = false;

                if (exitWarningMarker != null)
                    exitWarningMarker.SetActive(false);

                AnnounceRoomClearOnce();
                Debug.Log("Room encounter complete.");
            }
        }


        private void AnnounceRoomClearOnce()
        {
            if (clearAnnounced)
                return;

            clearAnnounced = true;
            BDCombatEventBus.NotifyRoomClear("Exit is open"); // no HUD text in V70
        }

        public void RegisterEnemy(BDHealth health)
        {
            if (health == null || enemyHealth.Contains(health))
                return;

            enemyHealth.Add(health);
        }

        public void RegisterExitBlocker(BDExitBlockerEnemy blocker)
        {
            if (blocker == null || exitBlockers.Contains(blocker))
                return;

            exitBlockers.Add(blocker);

            BDHealth health = blocker.GetComponent<BDHealth>();
            RegisterEnemy(health);
        }

        public void NotifyPlayerTriedExit(BDRoomExitZone zone, Transform exitAnchor)
        {
            if (!active || complete)
                return;

            if (horse == null)
                horse = FindFirstObjectByType<BDHorseController>();

            if (horse != null)
                horse.SendToSafeSpot();

            if (exitCooldown > 0f)
                return;

            exitCooldown = exitAttemptCooldown;
            exitAttempts++;
            lastExit = zone != null ? zone.name : "unknown";

            if (exitWarningMarker != null && exitAnchor != null)
            {
                exitWarningMarker.transform.position = exitAnchor.position + Vector3.up * 0.25f;
                exitWarningMarker.SetActive(true);
                CancelInvoke(nameof(HideExitWarning));
                Invoke(nameof(HideExitWarning), 0.9f);
            }

            foreach (BDExitBlockerEnemy blocker in exitBlockers)
            {
                if (blocker != null)
                    blocker.CommandBlockExit(exitAnchor, blockerCommandDuration);
            }

            Debug.Log($"Exit attempt: {lastExit}. Blockers commanded.");
        }

        private void HideExitWarning()
        {
            if (exitWarningMarker != null)
                exitWarningMarker.SetActive(false);
        }

        private void RefreshLiveEnemies()
        {
            int count = 0;

            for (int i = enemyHealth.Count - 1; i >= 0; i--)
            {
                BDHealth health = enemyHealth[i];

                if (health == null)
                {
                    enemyHealth.RemoveAt(i);
                    continue;
                }

                if (!health.IsDead)
                    count++;
            }

            liveEnemies = count;
        }

        private void OnGUI()
        {
            if (!showDebugOverlay)
                return;

            GUI.Box(new Rect(12, 290, 420, 130), "B&D Clean Core — Room");
            GUI.Label(new Rect(24, 320, 390, 22), $"Active: {active}");
            GUI.Label(new Rect(24, 342, 390, 22), $"Complete: {complete}");
            GUI.Label(new Rect(24, 364, 390, 22), $"Live Enemies: {liveEnemies}");
            GUI.Label(new Rect(24, 386, 390, 22), $"Exit Attempts: {exitAttempts}");
            GUI.Label(new Rect(24, 408, 390, 22), $"Last Exit: {lastExit}");
        }
    }
}
