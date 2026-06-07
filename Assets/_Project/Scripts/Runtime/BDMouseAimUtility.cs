using UnityEngine;

#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace BoredomAndDungeons
{
    public static class BDMouseAimUtility
    {
        // BD CINEMATIC MOUSE AIM HARD LOCK V20
        private static readonly RaycastHit[] AimHits = new RaycastHit[64];

        public static bool IsMouseInsideScreenCenterDeadZone(Transform source, float screenRadiusPixels)
        {
            if (BDRunPresentationCoordinator.InputLocked)
                return true;

            if (source == null || screenRadiusPixels <= 0f)
                return false;

            Camera camera = Camera.main;
            if (camera == null)
                return false;

            if (!TryReadMousePosition(out Vector2 mousePosition))
                return false;

            Vector3 screenPoint = camera.WorldToScreenPoint(source.position);

            if (screenPoint.z <= 0f)
                return false;

            Vector2 actorScreen = new Vector2(screenPoint.x, screenPoint.y);
            float sqrRadius = screenRadiusPixels * screenRadiusPixels;
            return (mousePosition - actorScreen).sqrMagnitude <= sqrRadius;
        }

        public static bool TryGetMouseAimDirection(Transform source, out Vector3 direction)
        {
            return TryGetMouseAimDirection(source, 0.0f, out direction);
        }

        public static bool TryGetMouseAimDirection(Transform source, float minWorldDistance, out Vector3 direction)
        {
            direction = Vector3.zero;

            if (BDRunPresentationCoordinator.InputLocked)
                return false;

            if (source == null)
                return false;

            if (Cursor.lockState != CursorLockMode.None)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }

            if (!TryGetMouseWorldPoint(source, out Vector3 worldPoint))
                return false;

            direction = worldPoint - source.position;
            direction.y = 0f;

            // Center dead zone behavior:
            // If the mouse is inside the caller-provided radius around the actor,
            // return false so the caller keeps its current facing direction.
            if (direction.magnitude < Mathf.Max(0f, minWorldDistance))
                return false;

            if (direction.sqrMagnitude < 0.001f)
                return false;

            direction.Normalize();
            return true;
        }

        public static bool TryGetMouseWorldPoint(Transform source, out Vector3 worldPoint)
        {
            worldPoint = Vector3.zero;

            if (BDRunPresentationCoordinator.InputLocked)
                return false;

            Camera camera = Camera.main;
            if (camera == null)
                return false;

            if (!TryReadMousePosition(out Vector2 mousePosition))
                return false;

            Ray ray = camera.ScreenPointToRay(mousePosition);

            int hitCount = Physics.RaycastNonAlloc(ray, AimHits, 1000f, ~0, QueryTriggerInteraction.Ignore);
            SortHitsByDistance(AimHits, hitCount);

            for (int i = 0; i < hitCount; i++)
            {
                RaycastHit hit = AimHits[i];

                if (hit.collider == null)
                    continue;

                if (ShouldIgnoreAimHit(source, hit.collider))
                    continue;

                if (hit.normal.y < 0.35f)
                    continue;

                worldPoint = hit.point;
                return true;
            }

            // Stable top-down fallback: aim on the gameplay ground plane, not at actor height.
            // Actor-height fallback can shift the target sideways with an angled camera.
            Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
            if (groundPlane.Raycast(ray, out float enter))
            {
                worldPoint = ray.GetPoint(enter);
                return true;
            }

            return false;
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

        private static bool ShouldIgnoreAimHit(Transform source, Collider collider)
        {
            if (collider == null)
                return true;

            if (collider.isTrigger)
                return true;

            Transform hitTransform = collider.transform;

            if (source != null && (hitTransform == source || hitTransform.IsChildOf(source) || source.IsChildOf(hitTransform)))
                return true;

            if (collider.GetComponentInParent<BDPlayerMarker>() != null)
                return true;

            if (collider.GetComponentInParent<BDHorseHealth>() != null)
                return true;

            if (collider.GetComponentInParent<BDHealth>() != null)
                return true;

            if (collider.GetComponentInParent<BDPlayerHealingPickup>() != null)
                return true;

            if (collider.GetComponentInParent<BDMinimapRoom>() != null)
                return true;

            string name = collider.name;
            if (name.Contains("Trigger") || name.Contains("Pressure") || name.Contains("Prediction") || name.Contains("Minimap"))
                return true;

            return false;
        }

        public static Vector3 DirectionFromYaw(float yawDegrees)
        {
            return Quaternion.Euler(0f, yawDegrees, 0f) * Vector3.forward;
        }

        public static float YawFromDirection(Vector3 direction, float fallbackYaw = 0f)
        {
            direction.y = 0f;

            if (direction.sqrMagnitude < 0.001f)
                return fallbackYaw;

            direction.Normalize();
            return Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        }

        private static bool TryReadMousePosition(out Vector2 position)
        {
            position = Vector2.zero;

#if ENABLE_INPUT_SYSTEM
            if (Mouse.current != null)
            {
                position = Mouse.current.position.ReadValue();
                return true;
            }
#endif

#if ENABLE_LEGACY_INPUT_MANAGER
            position = Input.mousePosition;
            return true;
#else
            return false;
#endif
        }
    }
}
