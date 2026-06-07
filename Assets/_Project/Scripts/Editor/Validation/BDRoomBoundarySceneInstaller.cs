#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BoredomAndDungeons.EditorTools.Validation
{
    public static class BDRoomBoundarySceneInstaller
    {
        // BD TALL ROOM BOUNDARY INSTALLER V7
        public const float MinimumWallWorldHeight = 22f;
        private const float MinimumWallLength = 4f;
        private const float MaximumWallThickness = 3.75f;

        public static bool TryInstallActiveScene(out string error)
        {
            error = string.Empty;

            try
            {
                Scene scene = SceneManager.GetActiveScene();
                if (!scene.IsValid())
                {
                    error = "No valid active scene for room-boundary installation.";
                    return false;
                }

                Renderer[] renderers =
                    UnityEngine.Object.FindObjectsByType<Renderer>(
                        FindObjectsInactive.Include,
                        FindObjectsSortMode.None
                    );

                int wallCount = 0;
                foreach (Renderer renderer in renderers)
                {
                    if (!IsWallCandidate(renderer, scene))
                        continue;

                    wallCount++;
                    RaiseWallWithoutMovingItsBase(renderer);

                    BDWallSurfaceProfile profile =
                        renderer.GetComponent<BDWallSurfaceProfile>();
                    if (profile == null)
                    {
                        profile = Undo.AddComponent<BDWallSurfaceProfile>(
                            renderer.gameObject
                        );
                    }

                    profile.ConfigureFromWorldBounds(
                        renderer.bounds,
                        MinimumWallWorldHeight
                    );
                    EditorUtility.SetDirty(profile);
                    EditorUtility.SetDirty(renderer.transform);
                }

                if (wallCount == 0)
                {
                    error =
                        "No room-boundary wall candidates were found. " +
                        "Expected a named wall/boundary or a long thin wall slab.";
                    return false;
                }

                Physics.SyncTransforms();
                return ValidateActiveScene(out error);
            }
            catch (Exception exception)
            {
                error = exception.ToString();
                return false;
            }
        }

        public static bool ValidateActiveScene(out string error)
        {
            error = string.Empty;
            BDWallSurfaceProfile[] profiles =
                UnityEngine.Object.FindObjectsByType<BDWallSurfaceProfile>(
                    FindObjectsInactive.Include,
                    FindObjectsSortMode.None
                );

            if (profiles.Length == 0)
            {
                error = "No BDWallSurfaceProfile exists in the active scene.";
                return false;
            }

            foreach (BDWallSurfaceProfile profile in profiles)
            {
                if (profile == null)
                    continue;

                Renderer renderer = profile.GetComponent<Renderer>();
                if (renderer == null)
                {
                    error = profile.name + " has no Renderer.";
                    return false;
                }

                if (renderer.bounds.size.y < MinimumWallWorldHeight - 0.10f)
                {
                    error =
                        profile.name + " wall height is only " +
                        renderer.bounds.size.y.ToString("0.00") +
                        "; expected at least " +
                        MinimumWallWorldHeight.ToString("0.00") + ".";
                    return false;
                }

                Vector3 scale = profile.transform.localScale;
                if (scale.x < 0f || scale.y < 0f || scale.z < 0f)
                {
                    error =
                        profile.name +
                        " uses negative scale, which can mirror asymmetric textures.";
                    return false;
                }

                if (!profile.PreserveAsymmetricTextureOrientation ||
                    profile.AllowTextureMirroring)
                {
                    error =
                        profile.name +
                        " is not configured for non-mirrored asymmetric textures.";
                    return false;
                }
            }

            return true;
        }

        private static bool IsWallCandidate(
            Renderer renderer,
            Scene activeScene)
        {
            if (renderer == null ||
                !renderer.gameObject.scene.IsValid() ||
                renderer.gameObject.scene != activeScene ||
                renderer.GetComponentInParent<BDPlayerMarker>() != null ||
                renderer.GetComponentInParent<BDHorseController>() != null ||
                renderer.GetComponentInParent<BDHealth>() != null)
            {
                return false;
            }

            string hierarchyName = BuildHierarchyName(renderer.transform);
            if (ContainsAny(
                    hierarchyName,
                    "floor", "ground", "ceiling", "roof", "door", "gate",
                    "portal", "hazard", "telegraph", "projectile", "weapon"))
            {
                return false;
            }

            Bounds bounds = renderer.bounds;
            float longAxis = Mathf.Max(bounds.size.x, bounds.size.z);
            float shortAxis = Mathf.Min(bounds.size.x, bounds.size.z);
            bool wallLikeGeometry =
                longAxis >= MinimumWallLength &&
                shortAxis <= MaximumWallThickness &&
                bounds.size.y >= 0.45f;

            bool wallLikeName = ContainsAny(
                hierarchyName,
                "wall", "boundary", "roomedge", "room_edge",
                "cavewall", "rockwall", "cliffwall"
            );

            return wallLikeName || wallLikeGeometry;
        }

        private static void RaiseWallWithoutMovingItsBase(
            Renderer renderer)
        {
            float currentHeight = renderer.bounds.size.y;
            if (currentHeight >= MinimumWallWorldHeight ||
                currentHeight <= 0.001f)
            {
                return;
            }

            Transform wall = renderer.transform;
            if (Vector3.Dot(wall.up.normalized, Vector3.up) < 0.90f)
                return;

            Vector3 currentScale = wall.localScale;
            if (currentScale.y <= 0f)
                return;

            float oldBaseY = renderer.bounds.min.y;
            currentScale.y *= MinimumWallWorldHeight / currentHeight;
            wall.localScale = currentScale;
            Physics.SyncTransforms();

            float baseCorrection = oldBaseY - renderer.bounds.min.y;
            wall.position += Vector3.up * baseCorrection;
            Physics.SyncTransforms();
        }

        private static string BuildHierarchyName(Transform transform)
        {
            string value = string.Empty;
            Transform current = transform;
            for (int depth = 0; current != null && depth < 5; depth++)
            {
                value += "/" + current.name.ToLowerInvariant();
                current = current.parent;
            }
            return value;
        }

        private static bool ContainsAny(
            string value,
            params string[] tokens)
        {
            foreach (string token in tokens)
            {
                if (value.IndexOf(token, StringComparison.OrdinalIgnoreCase) >= 0)
                    return true;
            }
            return false;
        }
    }
}
#endif
