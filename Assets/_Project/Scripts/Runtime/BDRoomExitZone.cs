using UnityEngine;

namespace BoredomAndDungeons
{
    [DisallowMultipleComponent]
    public sealed class BDRoomExitZone : MonoBehaviour
    {
        [SerializeField] private BDRoomEncounter encounter;
        [SerializeField] private Transform exitAnchor;
        [SerializeField] private float notifyEverySeconds = 0.75f;

        private float cooldown;

        public void Configure(BDRoomEncounter newEncounter, Transform newExitAnchor)
        {
            encounter = newEncounter;
            exitAnchor = newExitAnchor;
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
            if (cooldown > 0f || encounter == null)
                return;

            BDPlayerMarker player = other.GetComponentInParent<BDPlayerMarker>();
            if (player == null)
                return;

            cooldown = notifyEverySeconds;
            encounter.NotifyPlayerTriedExit(this, exitAnchor);
        }
    }
}
