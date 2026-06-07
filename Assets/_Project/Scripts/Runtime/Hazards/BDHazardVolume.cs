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

        public float SurfaceY
        {
            get
            {
                EnsureVolumeCollider();

                return volumeCollider != null
                    ? volumeCollider.bounds.max.y
                    : transform.position.y;
            }
        }

        private void Reset()
        {
            EnsureVolumeCollider();

            if (volumeCollider != null)
                volumeCollider.isTrigger = true;
        }

        private void Awake()
        {
            EnsureVolumeCollider();

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
                if (hazardType != BDHazardType.Lava ||
                    IsActorTouchingSurface(other, 0.08f))
                {
                    horseSafety.TryHandleHazard(this);
                }

                return;
            }

            BDPlayerHazardRecovery playerRecovery =
                other.GetComponentInParent<BDPlayerHazardRecovery>();

            if (playerRecovery == null)
                return;

            if (hazardType == BDHazardType.Lava &&
                !IsActorTouchingSurface(other, 0.05f))
            {
                return;
            }

            playerRecovery.TryHandleHazard(this);
        }

        public void Configure(BDHazardType type)
        {
            hazardType = type;
            EnsureVolumeCollider();

            if (volumeCollider != null)
                volumeCollider.isTrigger = true;
        }

        public bool IsActorTouchingSurface(
            Collider actorCollider,
            float verticalTolerance = 0.05f)
        {
            if (actorCollider == null)
                return false;

            EnsureVolumeCollider();

            if (volumeCollider == null ||
                !volumeCollider.enabled)
            {
                return false;
            }

            Bounds actorBounds = actorCollider.bounds;
            Bounds hazardBounds = volumeCollider.bounds;

            bool horizontalOverlap =
                actorBounds.max.x >= hazardBounds.min.x &&
                actorBounds.min.x <= hazardBounds.max.x &&
                actorBounds.max.z >= hazardBounds.min.z &&
                actorBounds.min.z <= hazardBounds.max.z;

            if (!horizontalOverlap)
                return false;

            float tolerance = Mathf.Max(0f, verticalTolerance);

            return actorBounds.min.y <= hazardBounds.max.y + tolerance &&
                   actorBounds.max.y >= hazardBounds.min.y - tolerance;
        }

        public bool ContainsHorizontalPoint(
            Vector3 worldPoint,
            float clearance)
        {
            EnsureVolumeCollider();

            if (volumeCollider == null ||
                !volumeCollider.enabled)
            {
                return false;
            }

            Bounds bounds = volumeCollider.bounds;
            float safeClearance = Mathf.Max(0f, clearance);

            return worldPoint.x >= bounds.min.x - safeClearance &&
                   worldPoint.x <= bounds.max.x + safeClearance &&
                   worldPoint.z >= bounds.min.z - safeClearance &&
                   worldPoint.z <= bounds.max.z + safeClearance;
        }

        // BD NEAREST HOLE EXIT POINT V23R2
        public bool TryResolveNearestHorizontalExitPoint(
            Vector3 worldPoint,
            float clearance,
            out Vector3 exitPoint)
        {
            EnsureVolumeCollider();
            exitPoint = worldPoint;

            if (volumeCollider == null ||
                !volumeCollider.enabled)
            {
                return false;
            }

            Bounds bounds = volumeCollider.bounds;
            float safeClearance = Mathf.Max(0.05f, clearance);

            float toMinX = Mathf.Abs(worldPoint.x - bounds.min.x);
            float toMaxX = Mathf.Abs(bounds.max.x - worldPoint.x);
            float toMinZ = Mathf.Abs(worldPoint.z - bounds.min.z);
            float toMaxZ = Mathf.Abs(bounds.max.z - worldPoint.z);
            float nearest = Mathf.Min(toMinX, toMaxX, toMinZ, toMaxZ);

            exitPoint.y = worldPoint.y;

            if (nearest == toMinX)
            {
                exitPoint.x = bounds.min.x - safeClearance;
                exitPoint.z = Mathf.Clamp(worldPoint.z, bounds.min.z, bounds.max.z);
            }
            else if (nearest == toMaxX)
            {
                exitPoint.x = bounds.max.x + safeClearance;
                exitPoint.z = Mathf.Clamp(worldPoint.z, bounds.min.z, bounds.max.z);
            }
            else if (nearest == toMinZ)
            {
                exitPoint.z = bounds.min.z - safeClearance;
                exitPoint.x = Mathf.Clamp(worldPoint.x, bounds.min.x, bounds.max.x);
            }
            else
            {
                exitPoint.z = bounds.max.z + safeClearance;
                exitPoint.x = Mathf.Clamp(worldPoint.x, bounds.min.x, bounds.max.x);
            }

            return true;
        }

        public bool IsPointWithin(
            Vector3 worldPoint,
            float extraClearance)
        {
            EnsureVolumeCollider();

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

        public static Vector3 FilterPlayerMotion(
            CharacterController actor,
            Vector3 requestedMotion,
            bool allowIntentionalGroundExit)
        {
            if (actor == null)
                return requestedMotion;

            Vector3 horizontal = new Vector3(
                requestedMotion.x,
                0f,
                requestedMotion.z
            );

            if (horizontal.sqrMagnitude < 0.000001f)
                return requestedMotion;

            Vector3 currentCenter = actor.bounds.center;
            float clearance = Mathf.Max(
                0.08f,
                actor.radius * 0.98f + actor.skinWidth
            );

            // BD SWEPT HOLE FOOTPRINT GUARD V23R2
            if (!allowIntentionalGroundExit &&
                WouldSweepIntoHole(
                    actor,
                    currentCenter,
                    horizontal,
                    clearance))
            {
                horizontal = ResolveSafeAxes(
                    actor,
                    currentCenter,
                    horizontal,
                    clearance
                );

                return horizontal +
                       Vector3.up * requestedMotion.y;
            }

            if (allowIntentionalGroundExit ||
                !actor.isGrounded)
            {
                return requestedMotion;
            }

            Vector3 targetCenter = currentCenter + horizontal;
            Vector3 leadingDirection = horizontal.normalized;
            Vector3 leadingPoint =
                targetCenter +
                leadingDirection *
                Mathf.Max(0.08f, actor.radius * 0.72f);

            if (HasGroundSupport(actor, targetCenter) &&
                HasGroundSupport(actor, leadingPoint))
            {
                return requestedMotion;
            }

            Vector3 safeGroundMotion = ResolveGroundedAxes(
                actor,
                currentCenter,
                horizontal
            );

            return safeGroundMotion +
                   Vector3.up * requestedMotion.y;
        }


        private static bool WouldSweepIntoHole(
            CharacterController actor,
            Vector3 currentCenter,
            Vector3 horizontal,
            float clearance)
        {
            float distance = horizontal.magnitude;
            if (distance < 0.0001f)
                return false;

            float stepLength = Mathf.Max(
                0.035f,
                actor.radius * 0.28f
            );
            int samples = Mathf.Clamp(
                Mathf.CeilToInt(distance / stepLength),
                1,
                16
            );

            Vector3 direction = horizontal / distance;
            Vector3 side = Vector3.Cross(Vector3.up, direction);
            if (side.sqrMagnitude > 0.0001f)
                side.Normalize();

            float leadingOffset = Mathf.Max(
                0.05f,
                actor.radius * 0.68f
            );
            float sideOffset = Mathf.Max(
                0.04f,
                actor.radius * 0.58f
            );
            float edgeClearance = Mathf.Max(
                0.02f,
                clearance * 0.30f
            );

            for (int sample = 1; sample <= samples; sample++)
            {
                float t = sample / (float)samples;
                Vector3 center = currentCenter + horizontal * t;

                if (IsInsideHoleHorizontal(center, clearance, out _) ||
                    IsInsideHoleHorizontal(
                        center + direction * leadingOffset,
                        edgeClearance,
                        out _) ||
                    IsInsideHoleHorizontal(
                        center + side * sideOffset,
                        edgeClearance,
                        out _) ||
                    IsInsideHoleHorizontal(
                        center - side * sideOffset,
                        edgeClearance,
                        out _))
                {
                    return true;
                }
            }

            return false;
        }

        private static Vector3 ResolveSafeAxes(
            CharacterController actor,
            Vector3 currentCenter,
            Vector3 horizontal,
            float clearance)
        {
            Vector3 safe = Vector3.zero;

            Vector3 xOnly = new Vector3(
                horizontal.x,
                0f,
                0f
            );

            if (xOnly.sqrMagnitude > 0.000001f &&
                !WouldSweepIntoHole(
                    actor,
                    currentCenter,
                    xOnly,
                    clearance) &&
                HasGroundSupport(actor, currentCenter + xOnly))
            {
                safe += xOnly;
            }

            Vector3 zOnly = new Vector3(
                0f,
                0f,
                horizontal.z
            );

            if (zOnly.sqrMagnitude > 0.000001f &&
                !WouldSweepIntoHole(
                    actor,
                    currentCenter + safe,
                    zOnly,
                    clearance) &&
                HasGroundSupport(
                    actor,
                    currentCenter + safe + zOnly))
            {
                safe += zOnly;
            }

            return safe;
        }


        private static Vector3 ResolveGroundedAxes(
            CharacterController actor,
            Vector3 currentCenter,
            Vector3 horizontal)
        {
            Vector3 safe = Vector3.zero;

            Vector3 xOnly = new Vector3(
                horizontal.x,
                0f,
                0f
            );

            if (xOnly.sqrMagnitude > 0.000001f &&
                HasGroundSupport(actor, currentCenter + xOnly))
            {
                safe += xOnly;
            }

            Vector3 zOnly = new Vector3(
                0f,
                0f,
                horizontal.z
            );

            if (zOnly.sqrMagnitude > 0.000001f &&
                HasGroundSupport(
                    actor,
                    currentCenter + safe + zOnly))
            {
                safe += zOnly;
            }

            return safe;
        }

        private static bool HasGroundSupport(
            CharacterController actor,
            Vector3 center)
        {
            float feetY = actor.bounds.min.y;
            Vector3 origin = new Vector3(
                center.x,
                feetY + 0.48f,
                center.z
            );

            RaycastHit[] hits = Physics.RaycastAll(
                origin,
                Vector3.down,
                1.05f,
                ~0,
                QueryTriggerInteraction.Ignore
            );

            foreach (RaycastHit hit in hits)
            {
                if (hit.collider == null)
                    continue;

                Transform hitTransform = hit.collider.transform;

                if (hitTransform == actor.transform ||
                    hitTransform.IsChildOf(actor.transform))
                {
                    continue;
                }

                if (Vector3.Angle(hit.normal, Vector3.up) > 58f)
                    continue;

                return true;
            }

            return false;
        }

        public static bool TryFindTouchingLava(
            Collider actorCollider,
            float verticalTolerance,
            out BDHazardVolume lava)
        {
            ActiveVolumes.RemoveWhere(item => item == null);

            foreach (BDHazardVolume volume in ActiveVolumes)
            {
                if (volume == null ||
                    !volume.isActiveAndEnabled ||
                    volume.HazardType != BDHazardType.Lava)
                {
                    continue;
                }

                if (!volume.IsActorTouchingSurface(
                        actorCollider,
                        verticalTolerance))
                {
                    continue;
                }

                lava = volume;
                return true;
            }

            lava = null;
            return false;
        }

        public static bool IsInsideHoleHorizontal(
            Vector3 worldPoint,
            float clearance,
            out BDHazardVolume hole)
        {
            ActiveVolumes.RemoveWhere(item => item == null);

            foreach (BDHazardVolume volume in ActiveVolumes)
            {
                if (volume == null ||
                    volume.HazardType !=
                        BDHazardType.HoleOrChasm)
                {
                    continue;
                }

                if (!volume.ContainsHorizontalPoint(
                        worldPoint,
                        clearance))
                {
                    continue;
                }

                hole = volume;
                return true;
            }

            hole = null;
            return false;
        }

        public static bool TryFindUnsafeVolume(
            Vector3 worldPoint,
            float horizontalClearance,
            out BDHazardVolume hazard)
        {
            ActiveVolumes.RemoveWhere(item => item == null);

            float clearance = Mathf.Max(
                0f,
                horizontalClearance
            );

            foreach (BDHazardVolume volume in ActiveVolumes)
            {
                if (volume == null ||
                    !volume.isActiveAndEnabled)
                {
                    continue;
                }

                if (!volume.ContainsHorizontalPoint(
                        worldPoint,
                        clearance))
                {
                    continue;
                }

                hazard = volume;
                return true;
            }

            hazard = null;
            return false;
        }

        public static bool IsRecoveryPointUnsafe(
            Vector3 worldPoint,
            float horizontalClearance)
        {
            ActiveVolumes.RemoveWhere(item => item == null);

            float clearance = Mathf.Max(
                0f,
                horizontalClearance
            );

            foreach (BDHazardVolume volume in ActiveVolumes)
            {
                if (volume == null ||
                    !volume.isActiveAndEnabled)
                {
                    continue;
                }

                if (volume.ContainsHorizontalPoint(
                        worldPoint,
                        clearance))
                {
                    return true;
                }
            }

            return false;
        }

        public static bool IsPointUnsafe(
            Vector3 worldPoint,
            float extraClearance)
        {
            ActiveVolumes.RemoveWhere(item => item == null);

            foreach (BDHazardVolume volume in ActiveVolumes)
            {
                if (volume.IsPointWithin(
                        worldPoint,
                        extraClearance))
                {
                    return true;
                }
            }

            return false;
        }

        private void EnsureVolumeCollider()
        {
            if (volumeCollider == null)
                volumeCollider = GetComponent<Collider>();
        }
    }
}
