using UnityEngine;

namespace BoredomAndDungeons
{
    [DisallowMultipleComponent]
    public sealed class BDCombatRoomExitPressureZone : MonoBehaviour
    {
        [SerializeField] private BDCombatRoom combatRoom;
        [SerializeField] private Transform exitAnchor;
        [SerializeField] private float notifyEverySeconds = 0.25f;

        private float cooldown;

        public void Configure(BDCombatRoom room, Transform anchor)
        {
            combatRoom = room;
            exitAnchor = anchor;
        }

        private void Update()
        {
            if (cooldown > 0f)
                cooldown -= Time.deltaTime;
        }

        private void OnTriggerEnter(Collider other)
        {
            TryNotify(other);
        }

        private void OnTriggerStay(Collider other)
        {
            TryNotify(other);
        }

        private void TryNotify(Collider other)
        {
            if (cooldown > 0f)
                return;

            if (combatRoom == null)
                return;

            if (other.GetComponentInParent<BDPlayerMarker>() == null)
                return;

            cooldown = notifyEverySeconds;
            combatRoom.NotifyPlayerNearExit(this, exitAnchor);
        }
    }
}
