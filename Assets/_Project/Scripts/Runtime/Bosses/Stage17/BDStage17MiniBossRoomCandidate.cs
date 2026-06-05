using UnityEngine;

namespace BoredomAndDungeons
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(BDStage17MiniBossEncounterSlot))]
    public sealed class BDStage17MiniBossRoomCandidate : MonoBehaviour
    {
        [Header("Identity")]
        [SerializeField] private string candidateId;

        [Header("Normalized map data")]
        [Tooltip("X and Y are 2D map coordinates, independent of Unity's X/Z ground plane.")]
        [SerializeField] private Vector2 mapCoordinates = new Vector2(0.5f, 0.5f);
        [Range(0f, 1f)]
        [SerializeField] private float progressionFromEntrance = 0.5f;

        [Header("Allowed roles")]
        [SerializeField] private bool allowGameBoyGuardian = true;
        [SerializeField] private bool allowCartridgeGuardian = true;
        [SerializeField] private bool allowPreBoss = true;

        [Header("Runtime")]
        [SerializeField] private bool permanentlyBlocked;
        [SerializeField] private bool occupied;

        private BDStage17MiniBossEncounterSlot slot;

        public string CandidateId =>
            string.IsNullOrWhiteSpace(candidateId)
                ? gameObject.name
                : candidateId;

        public Vector2 MapCoordinates => mapCoordinates;
        public float ProgressionFromEntrance => progressionFromEntrance;
        public bool PermanentlyBlocked => permanentlyBlocked;
        public bool Occupied => occupied;
        public BDStage17MiniBossEncounterSlot Slot
        {
            get
            {
                if (slot == null)
                    slot = GetComponent<BDStage17MiniBossEncounterSlot>();

                return slot;
            }
        }

        private void Awake()
        {
            slot = GetComponent<BDStage17MiniBossEncounterSlot>();
        }

        private void Reset()
        {
            if (string.IsNullOrWhiteSpace(candidateId))
                candidateId = gameObject.name;

            slot = GetComponent<BDStage17MiniBossEncounterSlot>();
        }

        private void OnValidate()
        {
            progressionFromEntrance =
                Mathf.Clamp01(progressionFromEntrance);

            mapCoordinates.x = Mathf.Clamp01(mapCoordinates.x);
            mapCoordinates.y = Mathf.Clamp01(mapCoordinates.y);

            if (string.IsNullOrWhiteSpace(candidateId))
                candidateId = gameObject.name;
        }

        public bool SupportsRole(BDStage17MiniBossRole role)
        {
            if (permanentlyBlocked || occupied)
                return false;

            switch (role)
            {
                case BDStage17MiniBossRole.GameBoyGuardian:
                    return allowGameBoyGuardian;

                case BDStage17MiniBossRole.CartridgeGuardian:
                    return allowCartridgeGuardian;

                case BDStage17MiniBossRole.PreBoss:
                    return allowPreBoss;

                default:
                    return false;
            }
        }

        public bool TryReserve(BDStage17MiniBossRole role)
        {
            if (!SupportsRole(role))
                return false;

            occupied = true;
            return true;
        }

        public void ReleaseReservation()
        {
            occupied = false;
        }

        public void ConfigureMapData(
            string id,
            Vector2 normalizedCoordinates,
            float normalizedProgression)
        {
            candidateId = string.IsNullOrWhiteSpace(id)
                ? gameObject.name
                : id;

            mapCoordinates = new Vector2(
                Mathf.Clamp01(normalizedCoordinates.x),
                Mathf.Clamp01(normalizedCoordinates.y)
            );

            progressionFromEntrance =
                Mathf.Clamp01(normalizedProgression);
        }

        private void OnDrawGizmosSelected()
        {
            Color color = occupied
                ? new Color(1f, 0.28f, 0.18f, 0.85f)
                : new Color(0.20f, 0.85f, 1f, 0.85f);

            Gizmos.color = color;
            Gizmos.DrawWireSphere(transform.position, 1.15f);
            Gizmos.DrawLine(
                transform.position,
                transform.position + Vector3.up * 2.2f
            );
        }
    }
}
