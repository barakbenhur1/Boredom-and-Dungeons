#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BoredomAndDungeons.EditorTools.Validation
{
    public static class BDPrototypeHazardSceneInstaller
    {
        public const string RootName = "__BD_HAZARD_TEST_AREA";
        public const string HoleName = "Hazard_HoleOrChasm";
        public const string LavaName = "Hazard_Lava";
        public const string QuicksandName = "Hazard_Quicksand";

        private const float HazardHalfSize = 1.35f;
        private const float MinimumPlayerDistance = 14.0f;
        private const float MinimumHazardSpacing = 7.0f;

        public static bool TryEnsureInstalled(
            string scenePath,
            out string error)
        {
            error = string.Empty;

            if (string.IsNullOrWhiteSpace(scenePath) ||
                !File.Exists(scenePath))
            {
                error = $"Prototype scene is missing: {scenePath}";
                return false;
            }

            try
            {
                Scene scene = SceneManager.GetActiveScene();

                if (!scene.IsValid() || scene.path != scenePath)
                {
                    scene = EditorSceneManager.OpenScene(
                        scenePath,
                        OpenSceneMode.Single
                    );
                }

                Physics.SyncTransforms();

                BDPlayerMarker playerMarker =
                    UnityEngine.Object.FindFirstObjectByType<BDPlayerMarker>();

                if (playerMarker == null)
                {
                    error = "Prototype scene has no BDPlayerMarker.";
                    return false;
                }

                Transform player = playerMarker.transform;

                EnsureActorComponents(player.gameObject);
                ConfigureMouseSensitivity(player.gameObject);
                RemoveStartRoomEnemies(player);
                EnsureHorseComponents();

                GameObject previous = GameObject.Find(RootName);
                if (previous != null)
                    UnityEngine.Object.DestroyImmediate(previous);

                GameObject root = new GameObject(RootName);
                root.transform.SetPositionAndRotation(
                    Vector3.zero,
                    Quaternion.identity
                );

                Vector3 holePosition = FindPlacement(
                    player.position,
                    null,
                    null,
                    player.right
                );

                Vector3 lavaPosition = FindPlacement(
                    player.position,
                    holePosition,
                    null,
                    player.right + player.forward * 0.35f
                );

                Vector3 quicksandPosition = FindPlacement(
                    player.position,
                    holePosition,
                    lavaPosition,
                    -player.right + player.forward * 0.25f
                );

                CreateHazard(
                    root.transform,
                    HoleName,
                    BDHazardType.HoleOrChasm,
                    holePosition,
                    "HOLE / CHASM\nJUMP OR DODGE"
                );

                CreateHazard(
                    root.transform,
                    LavaName,
                    BDHazardType.Lava,
                    lavaPosition,
                    "LAVA\nWALK INTO IT"
                );

                CreateHazard(
                    root.transform,
                    QuicksandName,
                    BDHazardType.Quicksand,
                    quicksandPosition,
                    "QUICKSAND\nKEEP MOVING TO ESCAPE"
                );

                if (!BDC07PlayableBossEncounterInstaller
                        .TryInstallActiveScene(out string c07Error))
                {
                    error = c07Error;
                    return false;
                }

                if (!BDGameplayShadowSceneInstaller
                        .TryInstallActiveScene(
                            out string shadowError))
                {
                    error = shadowError;
                    return false;
                }

                if (!BDRoomBoundarySceneInstaller
                        .TryInstallActiveScene(
                            out string boundaryError))
                {
                    error = boundaryError;
                    return false;
                }

                if (!BDMainMenuSettingsSceneInstaller
                        .TryInstallActiveScene(
                            out string menuError))
                {
                    error = menuError;
                    return false;
                }

                EditorSceneManager.MarkSceneDirty(scene);

                if (!EditorSceneManager.SaveScene(scene))
                {
                    error = "Unity could not save the prototype scene.";
                    return false;
                }

                CleanSerializedYaml(scenePath);
                AssetDatabase.ImportAsset(
                    scenePath,
                    ImportAssetOptions.ForceUpdate
                );
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                return true;
            }
            catch (Exception exception)
            {
                error = exception.ToString();
                return false;
            }
        }

        private static void EnsureActorComponents(GameObject player)
        {
            if (player.GetComponent<BDPlayerHazardRecovery>() == null)
            {
                Undo.AddComponent<BDPlayerHazardRecovery>(player);
            }
        }

        private static void ConfigureMouseSensitivity(
            GameObject player)
        {
            BDPlayerController controller =
                player.GetComponent<
                    BDPlayerController>();

            if (controller == null)
                return;

            SerializedObject serialized =
                new SerializedObject(controller);

            SerializedProperty sensitivity =
                serialized.FindProperty(
                    "mouseSensitivityMultiplier"
                );

            if (sensitivity != null)
                sensitivity.floatValue = 0.90f;

            serialized.ApplyModifiedPropertiesWithoutUndo();
            EditorUtility.SetDirty(controller);
        }

        private static void RemoveStartRoomEnemies(
            Transform player)
        {
            BDMinimapRoom[] rooms =
                UnityEngine.Object.FindObjectsByType<
                    BDMinimapRoom>(
                    FindObjectsInactive.Include,
                    FindObjectsSortMode.None
                );

            BDMinimapRoom startRoom = null;
            float nearestDistance =
                float.PositiveInfinity;

            foreach (BDMinimapRoom room in rooms)
            {
                if (room == null)
                    continue;

                if (room.ContainsWorldPosition(
                        player.position,
                        0.05f))
                {
                    startRoom = room;
                    break;
                }

                float distance =
                    room.SqrDistanceToCenter(
                        player.position);

                if (distance >= nearestDistance)
                    continue;

                nearestDistance = distance;
                startRoom = room;
            }

            if (startRoom == null)
                return;

            BDHealth[] healthComponents =
                UnityEngine.Object.FindObjectsByType<
                    BDHealth>(
                    FindObjectsInactive.Include,
                    FindObjectsSortMode.None
                );

            foreach (BDHealth health in healthComponents)
            {
                if (health == null ||
                    health.GetComponentInParent<
                        BDPlayerMarker>() != null ||
                    health.GetComponentInParent<
                        BDHorseHealth>() != null ||
                    health.GetComponent<
                        CharacterController>() == null ||
                    !startRoom.ContainsWorldPosition(
                        health.transform.position,
                        0f))
                {
                    continue;
                }

                UnityEngine.Object.DestroyImmediate(
                    health.gameObject
                );
            }

            MonoBehaviour[] behaviours =
                UnityEngine.Object.FindObjectsByType<
                    MonoBehaviour>(
                    FindObjectsInactive.Include,
                    FindObjectsSortMode.None
                );

            foreach (MonoBehaviour behaviour in behaviours)
            {
                if (behaviour == null)
                    continue;

                string typeName =
                    behaviour.GetType().Name;

                if (typeName.IndexOf(
                        "Enemy",
                        StringComparison.OrdinalIgnoreCase) < 0 ||
                    typeName.IndexOf(
                        "Spawner",
                        StringComparison.OrdinalIgnoreCase) < 0 ||
                    !startRoom.ContainsWorldPosition(
                        behaviour.transform.position,
                        0f))
                {
                    continue;
                }

                UnityEngine.Object.DestroyImmediate(
                    behaviour.gameObject
                );
            }
        }

        private static void EnsureHorseComponents()
        {
            BDHorseController horse =
                UnityEngine.Object.FindFirstObjectByType<BDHorseController>();

            if (horse == null)
                return;

            if (horse.GetComponent<BDHorseHazardSafety>() == null)
            {
                Undo.AddComponent<BDHorseHazardSafety>(horse.gameObject);
            }

            if (horse.GetComponent<
                    BDHorseExhaustedFollowAndPetInteraction>() == null)
            {
                Undo.AddComponent<
                    BDHorseExhaustedFollowAndPetInteraction>(
                    horse.gameObject
                );
            }
        }

        private static Vector3 FindPlacement(
            Vector3 playerPosition,
            Vector3? avoidPosition,
            Vector3? secondAvoidPosition,
            Vector3 preferredDirection)
        {
            Vector3 preferred = preferredDirection;
            preferred.y = 0f;

            if (preferred.sqrMagnitude < 0.001f)
                preferred = Vector3.forward;

            preferred.Normalize();

            List<Vector3> directions = new List<Vector3>
            {
                preferred,
                Quaternion.Euler(0f, 45f, 0f) * preferred,
                Quaternion.Euler(0f, -45f, 0f) * preferred,
                Quaternion.Euler(0f, 90f, 0f) * preferred,
                Quaternion.Euler(0f, -90f, 0f) * preferred,
                Quaternion.Euler(0f, 135f, 0f) * preferred,
                Quaternion.Euler(0f, -135f, 0f) * preferred,
                -preferred
            };

            float[] radii = { 16.0f, 19.0f, 22.0f, 25.0f, 28.0f };

            foreach (float radius in radii)
            {
                foreach (Vector3 direction in directions)
                {
                    Vector3 requested =
                        playerPosition +
                        direction.normalized * radius;

                    if (!TryFindGround(requested, out Vector3 grounded))
                        continue;

                    if (HorizontalDistance(
                            grounded,
                            playerPosition) < MinimumPlayerDistance)
                    {
                        continue;
                    }

                    if (avoidPosition.HasValue &&
                        HorizontalDistance(
                            grounded,
                            avoidPosition.Value) < MinimumHazardSpacing)
                    {
                        continue;
                    }

                    if (secondAvoidPosition.HasValue &&
                        HorizontalDistance(
                            grounded,
                            secondAvoidPosition.Value) < MinimumHazardSpacing)
                    {
                        continue;
                    }

                    if (!HasPlacementClearance(grounded))
                        continue;

                    return grounded;
                }
            }

            Vector3 fallback =
                playerPosition +
                preferred * (avoidPosition.HasValue ? 24f : 18f);

            if (TryFindGround(fallback, out Vector3 fallbackGround))
                return fallbackGround;

            fallback.y = playerPosition.y;
            return fallback;
        }

        private static bool TryFindGround(
            Vector3 requested,
            out Vector3 grounded)
        {
            Vector3 origin = requested + Vector3.up * 4f;
            RaycastHit[] hits = Physics.RaycastAll(
                origin,
                Vector3.down,
                10f,
                ~0,
                QueryTriggerInteraction.Ignore
            );

            Array.Sort(
                hits,
                (left, right) =>
                    left.distance.CompareTo(right.distance)
            );

            foreach (RaycastHit hit in hits)
            {
                if (hit.collider == null)
                    continue;

                if (hit.collider.GetComponentInParent<BDPlayerMarker>() != null)
                    continue;

                if (hit.collider.GetComponentInParent<BDHorseController>() != null)
                    continue;

                float slope = Vector3.Angle(hit.normal, Vector3.up);
                if (slope > 35f)
                    continue;

                grounded = hit.point + Vector3.up * 0.03f;
                return true;
            }

            grounded = requested;
            return false;
        }

        private static bool HasPlacementClearance(Vector3 grounded)
        {
            Vector3 center = grounded + Vector3.up * 0.8f;
            Vector3 halfExtents = new Vector3(
                HazardHalfSize + 0.45f,
                0.65f,
                HazardHalfSize + 0.45f
            );

            Collider[] overlaps = Physics.OverlapBox(
                center,
                halfExtents,
                Quaternion.identity,
                ~0,
                QueryTriggerInteraction.Ignore
            );

            foreach (Collider overlap in overlaps)
            {
                if (overlap == null)
                    continue;

                if (overlap.GetComponentInParent<BDPlayerMarker>() != null)
                    continue;

                if (overlap.GetComponentInParent<BDHorseController>() != null)
                    continue;

                if (overlap.bounds.max.y <= grounded.y + 0.20f)
                    continue;

                return false;
            }

            return true;
        }
        private static void CreateHazard(
            Transform parent,
            string objectName,
            BDHazardType type,
            Vector3 position,
            string labelText)
        {
            GameObject hazard =
                new GameObject(objectName);

            hazard.transform.SetParent(parent, false);
            hazard.transform.position = position;

            GameObject visual =
                GameObject.CreatePrimitive(
                    type == BDHazardType.Lava
                        ? PrimitiveType.Cube
                        : PrimitiveType.Cylinder
                );

            visual.name = "Visual";
            visual.transform.SetParent(
                hazard.transform,
                false
            );

            if (type == BDHazardType.Lava)
            {
                visual.transform.localPosition =
                    new Vector3(0f, 0.02f, 0f);

                visual.transform.localScale =
                    new Vector3(
                        2.70f,
                        0.06f,
                        2.70f
                    );
            }
            else if (type == BDHazardType.Quicksand)
            {
                visual.transform.localPosition =
                    new Vector3(0f, 0.015f, 0f);

                visual.transform.localScale =
                    new Vector3(
                        2.85f,
                        0.035f,
                        2.85f
                    );
            }
            else
            {
                visual.transform.localPosition =
                    new Vector3(0f, -0.015f, 0f);

                visual.transform.localScale =
                    new Vector3(
                        2.70f,
                        0.025f,
                        2.70f
                    );
            }

            Collider visualCollider =
                visual.GetComponent<Collider>();

            if (visualCollider != null)
            {
                UnityEngine.Object.DestroyImmediate(
                    visualCollider
                );
            }

            Renderer renderer =
                visual.GetComponent<Renderer>();

            if (renderer != null)
            {
                renderer.shadowCastingMode =
                    UnityEngine.Rendering
                        .ShadowCastingMode.Off;

                renderer.receiveShadows = false;
            }

            GameObject trigger =
                new GameObject("Trigger");

            trigger.transform.SetParent(
                hazard.transform,
                false
            );

            BoxCollider box =
                trigger.AddComponent<BoxCollider>();

            box.isTrigger = true;

            if (type == BDHazardType.Lava)
            {
                trigger.transform.localPosition =
                    new Vector3(0f, 0.04f, 0f);

                box.size = new Vector3(
                    2.50f,
                    0.16f,
                    2.50f
                );
            }
            else if (type == BDHazardType.Quicksand)
            {
                trigger.transform.localPosition =
                    new Vector3(0f, 0.20f, 0f);

                box.size = new Vector3(
                    2.65f,
                    0.42f,
                    2.65f
                );
            }
            else
            {
                trigger.transform.localPosition =
                    new Vector3(0f, 0.55f, 0f);

                box.size = new Vector3(
                    2.45f,
                    1.20f,
                    2.45f
                );
            }

            BDHazardVolume volume =
                trigger.AddComponent<
                    BDHazardVolume>();

            volume.Configure(type);

            GameObject label =
                new GameObject("Label");

            label.transform.SetParent(
                hazard.transform,
                false
            );

            label.transform.localPosition =
                new Vector3(0f, 0.08f, -1.55f);

            label.transform.localRotation =
                Quaternion.Euler(
                    90f,
                    0f,
                    0f
                );

            TextMesh text =
                label.AddComponent<TextMesh>();

            text.text = labelText;
            text.anchor = TextAnchor.MiddleCenter;
            text.alignment = TextAlignment.Center;
            text.characterSize = 0.20f;
            text.fontSize = 48;

            BDPrototypeHazardLabelVisibility visibility =
                label.AddComponent<
                    BDPrototypeHazardLabelVisibility>();
            visibility.Configure(9.0f);
        }

        private static void CleanSerializedYaml(
            string scenePath)
        {
            string source = File.ReadAllText(scenePath);
            string[] lines = source.Replace(
                "\r\n",
                "\n"
            ).Split('\n');

            for (int index = 0;
                 index < lines.Length;
                 index++)
            {
                string trimmed =
                    lines[index].TrimEnd(' ', '\t');

                if (trimmed.EndsWith(
                        "m_Name:",
                        StringComparison.Ordinal))
                {
                    trimmed += " \"\"";
                }
                else if (trimmed.EndsWith(
                             "m_EditorClassIdentifier:",
                             StringComparison.Ordinal))
                {
                    trimmed += " \"\"";
                }

                lines[index] = trimmed;
            }

            File.WriteAllText(
                scenePath,
                string.Join("\n", lines)
                    .TrimEnd('\n') +
                "\n"
            );
        }

        private static float HorizontalDistance(
            Vector3 first,
            Vector3 second)
        {
            first.y = 0f;
            second.y = 0f;
            return Vector3.Distance(first, second);
        }
    }
}
#endif
