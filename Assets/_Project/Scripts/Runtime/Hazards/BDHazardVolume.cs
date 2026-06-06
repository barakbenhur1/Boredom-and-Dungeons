using System.Collections.Generic;
using UnityEngine;

namespace BoredomAndDungeons
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Collider))]
    public sealed class BDHazardVolume : MonoBehaviour
    {
        private static readonly HashSet<BDHazardVolume> ActiveVolumes =
            new HashSet<BDHazardVolume>();

        [SerializeField] private BDHazardType hazardType =
            BDHazardType.HoleOrChasm;

        private Collider volumeCollider;

        public BDHazardType HazardType => hazardType;
        public float Damage =>
            hazardType == BDHazardType.Lava ? 10f : 15f;

        private void Reset()
        {
            Collider candidate = GetComponent<Collider>();

            if (candidate != null)
                candidate.isTrigger = true;
        }

        private void Awake()
        {
            volumeCollider = GetComponent<Collider>();

            if (volumeCollider != null)
                volumeCollider.isTrigger = true;
        }

        private void OnEnable()
        {
            ActiveVolumes.Add(this);
        }

        private void OnDisable()
        {
            ActiveVolumes.Remove(this);
        }

        private void OnDestroy()
        {
            ActiveVolumes.Remove(this);
        }

        private void OnTriggerEnter(Collider other)
        {
            TryAffectActor(other);
        }

        private void OnTriggerStay(Collider other)
        {
            TryAffectActor(other);
        }

        private void TryAffectActor(Collider other)
        {
            if (other == null)
                return;

            BDHorseHazardSafety horseSafety =
                other.GetComponentInParent<BDHorseHazardSafety>();

            if (horseSafety != null)
            {
                horseSafety.TryHandleHazard(this);
                return;
            }

            BDPlayerHazardRecovery playerRecovery =
                other.GetComponentInParent<BDPlayerHazardRecovery>();

            if (playerRecovery != null)
                playerRecovery.TryHandleHazard(this);
        }
        public void Configure(BDHazardType type)
        {
            hazardType = type;

            if (volumeCollider == null)
                volumeCollider = GetComponent<Collider>();

            if (volumeCollider != null)
                volumeCollider.isTrigger = true;
        }


        public bool IsPointWithin(
            Vector3 worldPoint,
            float extraClearance)
        {
            if (!isActiveAndEnabled ||
                volumeCollider == null ||
                !volumeCollider.enabled)
            {
                return false;
            }

            Vector3 closest = volumeCollider.ClosestPoint(worldPoint);
            float clearance = Mathf.Max(0f, extraClearance);

            return (closest - worldPoint).sqrMagnitude <=
                   clearance * clearance;
        }

        public static bool IsPointUnsafe(
            Vector3 worldPoint,
            float extraClearance)
        {
            ActiveVolumes.RemoveWhere(item => item == null);

            foreach (BDHazardVolume volume in ActiveVolumes)
            {
                if (volume.IsPointWithin(worldPoint, extraClearance))
                    return true;
            }

            return false;
        }
    }
}
