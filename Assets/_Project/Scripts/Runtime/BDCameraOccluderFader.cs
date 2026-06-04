using System.Collections.Generic;
using UnityEngine;

namespace BoredomAndDungeons
{
    [DisallowMultipleComponent]
    public sealed class BDCameraOccluderFader : MonoBehaviour
    {
        [SerializeField] private Transform target;
        [SerializeField] private float targetHeight = 0.9f;
        [SerializeField] private float sphereCastRadius = 0.42f;
        [SerializeField] private LayerMask occlusionMask = ~0;
        [SerializeField] private float normalCheckInterval = 0.025f;
        [SerializeField] private float reducedEffectsCheckInterval = 0.065f;
        [SerializeField] private bool showDebugRay = false;

        private readonly HashSet<BDOccludingWall> currentlyFaded = new HashSet<BDOccludingWall>();
        private readonly List<BDOccludingWall> toRestore = new List<BDOccludingWall>();
        private static readonly RaycastHit[] HitBuffer = new RaycastHit[48];

        private float nextCheckAt;

        public void SetTarget(Transform newTarget)
        {
            target = newTarget;
        }

        private void LateUpdate()
        {
            if (Application.isPlaying && Time.time < nextCheckAt)
                return;

            float interval = BDPerformanceGuard.ReducedEffects ? reducedEffectsCheckInterval : normalCheckInterval;
            nextCheckAt = Application.isPlaying ? Time.time + Mathf.Max(0.005f, interval) : 0f;

            ResolveTarget();

            if (target == null)
            {
                RestoreAll();
                return;
            }

            Vector3 origin = transform.position;
            Vector3 destination = target.position + Vector3.up * targetHeight;
            Vector3 direction = destination - origin;
            float distance = direction.magnitude;

            if (distance <= 0.01f)
            {
                RestoreAll();
                return;
            }

            toRestore.Clear();
            foreach (BDOccludingWall wall in currentlyFaded)
                toRestore.Add(wall);

            int hitCount = Physics.SphereCastNonAlloc(
                origin,
                sphereCastRadius,
                direction.normalized,
                HitBuffer,
                distance,
                occlusionMask,
                QueryTriggerInteraction.Ignore
            );

            SortHitsByDistance(HitBuffer, hitCount);

            for (int i = 0; i < hitCount; i++)
            {
                RaycastHit hit = HitBuffer[i];

                if (hit.collider == null)
                    continue;

                BDOccludingWall wall = hit.collider.GetComponentInParent<BDOccludingWall>();
                if (wall == null)
                    continue;

                wall.SetFaded(true);
                currentlyFaded.Add(wall);
                toRestore.Remove(wall);
            }

            for (int i = 0; i < toRestore.Count; i++)
            {
                BDOccludingWall wall = toRestore[i];
                if (wall == null)
                    continue;

                wall.SetFaded(false);
                currentlyFaded.Remove(wall);
            }

            toRestore.Clear();

            if (showDebugRay)
                Debug.DrawLine(origin, destination, Color.cyan);
        }

        private void ResolveTarget()
        {
            if (target != null)
                return;

            Transform found = BDTargetFinder.FindPlayer();
            if (found != null)
                target = found;
        }

        private static void SortHitsByDistance(RaycastHit[] hits, int count)
        {
            for (int i = 1; i < count; i++)
            {
                RaycastHit key = hits[i];
                float distance = key.distance;
                int j = i - 1;

                while (j >= 0 && hits[j].distance > distance)
                {
                    hits[j + 1] = hits[j];
                    j--;
                }

                hits[j + 1] = key;
            }
        }

        private void RestoreAll()
        {
            foreach (BDOccludingWall wall in currentlyFaded)
            {
                if (wall != null)
                    wall.SetFaded(false);
            }

            currentlyFaded.Clear();
            toRestore.Clear();
        }

        private void OnDisable()
        {
            RestoreAll();
        }

        private void OnDestroy()
        {
            RestoreAll();
        }
    }
}
