using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace BoredomAndDungeons.EditorTools
{
    public static class BDStage17MiniBossMapSetupTools
    {
        [MenuItem(
            "Boredom & Dungeons/Bosses/Stage 17/" +
            "Prepare Selected Map Root")]
        private static void PrepareSelectedMapRoot()
        {
            GameObject root = Selection.activeGameObject;

            if (root == null)
            {
                EditorUtility.DisplayDialog(
                    "B&D Stage 17",
                    "Select the generated map root in the Hierarchy.",
                    "OK"
                );
                return;
            }

            BDStage17MiniBossPlanGenerator generator =
                root.GetComponent<BDStage17MiniBossPlanGenerator>();

            if (generator == null)
            {
                generator =
                    Undo.AddComponent<
                        BDStage17MiniBossPlanGenerator
                    >(root);
            }

            BDStage17MiniBossPrefabRegistry registry =
                root.GetComponent<
                    BDStage17MiniBossPrefabRegistry
                >();

            if (registry == null)
            {
                registry =
                    Undo.AddComponent<
                        BDStage17MiniBossPrefabRegistry
                    >(root);
            }

            generator.RefreshCandidates();

            EditorUtility.SetDirty(generator);
            EditorUtility.SetDirty(registry);
            EditorSceneManager.MarkSceneDirty(root.scene);

            Debug.Log(
                $"B&D Stage 17 map root prepared: {root.name}.",
                root
            );
        }

        [MenuItem(
            "Boredom & Dungeons/Bosses/Stage 17/" +
            "Mark Selected Rooms As Candidates")]
        private static void MarkSelectedRoomsAsCandidates()
        {
            GameObject[] selected = Selection.gameObjects;

            if (selected == null || selected.Length == 0)
            {
                EditorUtility.DisplayDialog(
                    "B&D Stage 17",
                    "Select one or more legal mini-boss room roots.",
                    "OK"
                );
                return;
            }

            int added = 0;

            for (int i = 0; i < selected.Length; i++)
            {
                GameObject room = selected[i];

                if (room == null)
                    continue;

                if (room.GetComponent<
                        BDStage17MiniBossEncounterSlot
                    >() == null)
                {
                    Undo.AddComponent<
                        BDStage17MiniBossEncounterSlot
                    >(room);
                }

                if (room.GetComponent<
                        BDStage17MiniBossRoomCandidate
                    >() == null)
                {
                    Undo.AddComponent<
                        BDStage17MiniBossRoomCandidate
                    >(room);

                    added++;
                }

                EditorUtility.SetDirty(room);
                EditorSceneManager.MarkSceneDirty(room.scene);
            }

            Debug.Log(
                $"B&D Stage 17: {added} candidate room(s) added."
            );
        }

        [MenuItem(
            "Boredom & Dungeons/Bosses/Stage 17/" +
            "Auto Assign Candidate Map Data")]
        private static void AutoAssignCandidateMapData()
        {
            GameObject root = Selection.activeGameObject;

            if (root == null)
            {
                EditorUtility.DisplayDialog(
                    "B&D Stage 17",
                    "Select the map root that contains the candidates.",
                    "OK"
                );
                return;
            }

            BDStage17MiniBossRoomCandidate[] candidates =
                root.GetComponentsInChildren<
                    BDStage17MiniBossRoomCandidate
                >(includeInactive: true);

            if (candidates.Length == 0)
            {
                EditorUtility.DisplayDialog(
                    "B&D Stage 17",
                    "No candidate rooms were found.",
                    "OK"
                );
                return;
            }

            Bounds bounds =
                new Bounds(
                    candidates[0].transform.position,
                    Vector3.zero
                );

            for (int i = 1; i < candidates.Length; i++)
                bounds.Encapsulate(candidates[i].transform.position);

            float width = Mathf.Max(0.01f, bounds.size.x);
            float depth = Mathf.Max(0.01f, bounds.size.z);

            List<BDStage17MiniBossRoomCandidate> ordered =
                new List<BDStage17MiniBossRoomCandidate>(
                    candidates
                );

            ordered.Sort(
                (a, b) =>
                    a.transform.position.z.CompareTo(
                        b.transform.position.z
                    )
            );

            for (int i = 0; i < ordered.Count; i++)
            {
                BDStage17MiniBossRoomCandidate candidate =
                    ordered[i];

                Vector3 position = candidate.transform.position;

                Vector2 normalizedCoordinates =
                    new Vector2(
                        Mathf.InverseLerp(
                            bounds.min.x,
                            bounds.max.x,
                            position.x
                        ),
                        Mathf.InverseLerp(
                            bounds.min.z,
                            bounds.max.z,
                            position.z
                        )
                    );

                float progression =
                    ordered.Count <= 1
                        ? 0.5f
                        : i / (float)(ordered.Count - 1);

                Undo.RecordObject(
                    candidate,
                    "Auto Assign Stage 17 Candidate Data"
                );

                candidate.ConfigureMapData(
                    candidate.gameObject.name,
                    normalizedCoordinates,
                    progression
                );

                EditorUtility.SetDirty(candidate);
            }

            EditorSceneManager.MarkSceneDirty(root.scene);

            Debug.Log(
                $"B&D Stage 17: map data assigned to " +
                $"{ordered.Count} candidate rooms. " +
                "Review progression values before final use.",
                root
            );
        }

        [MenuItem(
            "Boredom & Dungeons/Bosses/Stage 17/" +
            "Validate Selected Map Setup")]
        private static void ValidateSelectedMapSetup()
        {
            GameObject root = Selection.activeGameObject;

            if (root == null)
            {
                EditorUtility.DisplayDialog(
                    "B&D Stage 17",
                    "Select the map root.",
                    "OK"
                );
                return;
            }

            BDStage17MiniBossPlanGenerator generator =
                root.GetComponentInChildren<
                    BDStage17MiniBossPlanGenerator
                >(includeInactive: true);

            BDStage17MiniBossRoomCandidate[] candidates =
                root.GetComponentsInChildren<
                    BDStage17MiniBossRoomCandidate
                >(includeInactive: true);

            int errors = 0;

            if (generator == null)
            {
                Debug.LogError(
                    "Missing BDStage17MiniBossPlanGenerator.",
                    root
                );
                errors++;
            }

            if (candidates.Length < 3)
            {
                Debug.LogError(
                    "Stage 17 requires at least three candidate rooms.",
                    root
                );
                errors++;
            }

            HashSet<string> ids = new HashSet<string>();

            for (int i = 0; i < candidates.Length; i++)
            {
                BDStage17MiniBossRoomCandidate candidate =
                    candidates[i];

                if (!ids.Add(candidate.CandidateId))
                {
                    Debug.LogError(
                        $"Duplicate Stage 17 candidate id: " +
                        $"{candidate.CandidateId}",
                        candidate
                    );
                    errors++;
                }

                if (candidate.Slot == null)
                {
                    Debug.LogError(
                        $"{candidate.name}: missing encounter slot.",
                        candidate
                    );
                    errors++;
                }
            }

            if (errors == 0)
            {
                Debug.Log(
                    $"B&D Stage 17 validation passed. " +
                    $"Candidates={candidates.Length}.",
                    root
                );
            }
        }
    }
}
