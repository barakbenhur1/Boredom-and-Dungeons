using UnityEngine;
using UnityEngine.SceneManagement;

namespace BoredomAndDungeons
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(BDPlayerMarker))]
    public sealed class BDPlayerParryState : MonoBehaviour
    {
        [Header("Timing")]
        [SerializeField] private float attackTimingEpsilon = 0.16f;
        [SerializeField] private float physicalSignalMaxAge = 0.20f;
        // BD PARRY DURATION TUNING FIX
        [SerializeField] private float normalParryFreezeDuration = 1.5f;
        [SerializeField] private float upgradedParryFreezeDuration = 2.5f;

        [Header("Range")]
        [SerializeField] private float maximumParryDistance = 3.2f;

        [Header("Run Upgrade")]
        [SerializeField] private bool extendedFreezeCollected;

        private float lastMeleeAttackAtUnscaled = -999f;
        private bool lastAttackWasHeavy;
        private bool attackWindowConsumed;

        public bool HasExtendedFreeze => extendedFreezeCollected;
        public float CurrentFreezeDuration => extendedFreezeCollected
            ? Mathf.Max(0.10f, upgradedParryFreezeDuration)
            : Mathf.Max(0.10f, normalParryFreezeDuration);

        public float ParryWindowRemaining => Mathf.Max(
            0f,
            attackTimingEpsilon - (Time.unscaledTime - lastMeleeAttackAtUnscaled)
        );

        public bool IsParryWindowActive =>
            !attackWindowConsumed &&
            Time.unscaledTime - lastMeleeAttackAtUnscaled <= Mathf.Max(0.01f, attackTimingEpsilon);

        public void RecordMeleeAttack(bool heavy)
        {
            lastMeleeAttackAtUnscaled = Time.unscaledTime;
            lastAttackWasHeavy = heavy;
            attackWindowConsumed = false;
        }

        public bool CollectExtendedFreezeUpgrade()
        {
            if (extendedFreezeCollected)
                return false;

            extendedFreezeCollected = true;
            return true;
        }

        public bool TryParryIncomingPhysicalDamage()
        {
            if (!IsParryWindowActive)
                return false;

            if (!BDPhysicalAttackSignal.TryConsumeRecent(
                    transform,
                    Mathf.Max(0.01f, physicalSignalMaxAge),
                    Mathf.Max(0.25f, maximumParryDistance),
                    out Transform attacker))
                return false;

            attackWindowConsumed = true;
            BDParrySystem.Trigger(
                transform,
                attacker,
                CurrentFreezeDuration,
                lastAttackWasHeavy
            );
            return true;
        }

        public static void NotifyPlayerMeleeAttack(bool heavy)
        {
            Transform player = BDTargetFinder.FindPlayer();
            if (player == null)
            {
                BDPlayerMarker marker = Object.FindFirstObjectByType<BDPlayerMarker>();
                if (marker != null)
                    player = marker.transform;
            }

            if (player == null)
                return;

            BDPlayerParryState state = player.GetComponent<BDPlayerParryState>();
            if (state == null)
                state = player.gameObject.AddComponent<BDPlayerParryState>();

            state.RecordMeleeAttack(heavy);
        }
    }

    public static class BDPlayerParryInstaller
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void InstallAfterSceneLoad()
        {
            Install();
            SceneManager.sceneLoaded -= HandleSceneLoaded;
            SceneManager.sceneLoaded += HandleSceneLoaded;
        }

        private static void HandleSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            Install();
        }

        private static void Install()
        {
            BDPlayerMarker[] players = Object.FindObjectsByType<BDPlayerMarker>(FindObjectsSortMode.None);
            for (int i = 0; i < players.Length; i++)
            {
                BDPlayerMarker player = players[i];
                if (player != null && player.GetComponent<BDPlayerParryState>() == null)
                    player.gameObject.AddComponent<BDPlayerParryState>();
            }
        }
    }
}
