using UnityEngine;

namespace BoredomAndDungeons
{
    [DisallowMultipleComponent]
    public sealed class BDRoomClearExitPulseListener : MonoBehaviour
    {
        [SerializeField] private float maxExitSearchDistance = 60f;
        [SerializeField] private bool pulseAllKnownExits = false;

        private static BDRoomClearExitPulseListener instance;
        private Transform player;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void Bootstrap()
        {
            if (instance != null)
                return;

            GameObject go = new GameObject("BD_RoomClearExitPulseListener");
            if (!Application.isPlaying)
            {
                Object.DestroyImmediate(go);
                return;
            }

            DontDestroyOnLoad(go);
            instance = go.AddComponent<BDRoomClearExitPulseListener>();
        }

        private void OnEnable()
        {
            BDCombatEventBus.RoomCleared += OnRoomCleared;
        }

        private void OnDisable()
        {
            BDCombatEventBus.RoomCleared -= OnRoomCleared;
        }

        private void OnRoomCleared()
        {
            player = BDTargetFinder.FindPlayer();

            BDExitOpenPulse[] pulses = FindObjectsByType<BDExitOpenPulse>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);

            if (pulses == null || pulses.Length == 0)
                return;

            if (pulseAllKnownExits)
            {
                for (int i = 0; i < pulses.Length; i++)
                    if (pulses[i] != null) pulses[i].TriggerOpenPulse();

                return;
            }

            BDExitOpenPulse closest = null;
            float closestSqr = maxExitSearchDistance * maxExitSearchDistance;

            Vector3 origin = player != null ? player.position : Vector3.zero;

            for (int i = 0; i < pulses.Length; i++)
            {
                BDExitOpenPulse pulse = pulses[i];
                if (pulse == null)
                    continue;

                float sqr = (pulse.transform.position - origin).sqrMagnitude;
                if (sqr < closestSqr)
                {
                    closestSqr = sqr;
                    closest = pulse;
                }
            }

            if (closest != null)
                closest.TriggerOpenPulse();
        }
    }
}
