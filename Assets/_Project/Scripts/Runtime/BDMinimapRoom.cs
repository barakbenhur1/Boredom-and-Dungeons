using UnityEngine;

namespace BoredomAndDungeons
{
    [DisallowMultipleComponent]
    public sealed class BDMinimapRoom : MonoBehaviour
    {
        [SerializeField] private Vector2Int cell;
        [SerializeField] private Vector3 worldCenter;
        [SerializeField] private float roomSize = 24f;
        [SerializeField] private bool northOpen;
        [SerializeField] private bool southOpen;
        [SerializeField] private bool eastOpen;
        [SerializeField] private bool westOpen;
        [SerializeField] private bool discovered;

        public Vector2Int Cell => cell;
        public Vector3 WorldCenter => worldCenter;
        public float RoomSize => roomSize;
        public bool NorthOpen => northOpen;
        public bool SouthOpen => southOpen;
        public bool EastOpen => eastOpen;
        public bool WestOpen => westOpen;
        public bool Discovered => discovered;

        public void Configure(
            Vector2Int newCell,
            Vector3 newWorldCenter,
            float newRoomSize,
            bool newNorthOpen,
            bool newSouthOpen,
            bool newEastOpen,
            bool newWestOpen)
        {
            cell = newCell;
            worldCenter = newWorldCenter;
            roomSize = newRoomSize;
            northOpen = newNorthOpen;
            southOpen = newSouthOpen;
            eastOpen = newEastOpen;
            westOpen = newWestOpen;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponentInParent<BDPlayerMarker>() == null)
                return;

            discovered = true;
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.GetComponentInParent<BDPlayerMarker>() == null)
                return;

            discovered = true;
        }

        public bool ContainsWorldPosition(Vector3 worldPosition, float padding = 0f)
        {
            float halfSize = Mathf.Max(0.5f, roomSize * 0.5f + Mathf.Max(0f, padding));
            Vector3 delta = worldPosition - worldCenter;
            return Mathf.Abs(delta.x) <= halfSize && Mathf.Abs(delta.z) <= halfSize;
        }

        public float SqrDistanceToCenter(Vector3 worldPosition)
        {
            Vector3 delta = worldPosition - worldCenter;
            delta.y = 0f;
            return delta.sqrMagnitude;
        }

        public void ForceDiscover()
        {
            discovered = true;
        }
    }
}
