using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace BoredomAndDungeons.EditorTools
{
    public static class BDBossSummonBudgetSetupTools
    {
        [MenuItem(
            "Boredom & Dungeons/Bosses/Stage 16/" +
            "Add Shared Summon Budget To Selected Encounter")]
        private static void AddBudgetToSelectedEncounter()
        {
            GameObject selected = Selection.activeGameObject;

            if (selected == null)
            {
                EditorUtility.DisplayDialog(
                    "B&D Summon Budget",
                    "Select the encounter root in the Hierarchy.",
                    "OK"
                );
                return;
            }

            BDBossEncounterController encounter =
                selected.GetComponent<BDBossEncounterController>();

            if (encounter == null)
            {
                EditorUtility.DisplayDialog(
                    "B&D Summon Budget",
                    "The selected root must contain " +
                    "BDBossEncounterController.",
                    "OK"
                );
                return;
            }

            BDBossSharedSummonBudget budget =
                selected.GetComponent<BDBossSharedSummonBudget>();

            if (budget == null)
            {
                budget =
                    Undo.AddComponent<BDBossSharedSummonBudget>(
                        selected
                    );
            }

            budget.Configure(encounter, maximumAlive: 8);
            EditorUtility.SetDirty(budget);
            EditorSceneManager.MarkSceneDirty(selected.scene);

            Debug.Log(
                $"Shared summon budget added to {selected.name}. " +
                "Default maximum alive: 8.",
                selected
            );
        }

        [MenuItem(
            "Boredom & Dungeons/Bosses/Stage 16/" +
            "Add Summon Emitter To Selected Boss Part")]
        private static void AddEmitterToSelectedBossPart()
        {
            GameObject selected = Selection.activeGameObject;

            if (selected == null)
            {
                EditorUtility.DisplayDialog(
                    "B&D Summon Emitter",
                    "Select the boss or boss part in the Hierarchy.",
                    "OK"
                );
                return;
            }

            BDBossSharedSummonBudget budget =
                selected.GetComponentInParent<
                    BDBossSharedSummonBudget
                >();

            if (budget == null)
            {
                EditorUtility.DisplayDialog(
                    "B&D Summon Emitter",
                    "No BDBossSharedSummonBudget was found " +
                    "on the encounter root.",
                    "OK"
                );
                return;
            }

            BDBossSummonEmitter emitter =
                selected.GetComponent<BDBossSummonEmitter>();

            if (emitter == null)
            {
                emitter =
                    Undo.AddComponent<BDBossSummonEmitter>(
                        selected
                    );
            }

            EditorUtility.SetDirty(emitter);
            EditorSceneManager.MarkSceneDirty(selected.scene);

            Debug.Log(
                $"Summon emitter added to {selected.name}. " +
                "Assign prefab(s), spawn points and timing in Inspector.",
                selected
            );
        }

        [MenuItem(
            "Boredom & Dungeons/Bosses/Stage 16/" +
            "Validate Selected Summon Setup")]
        private static void ValidateSelectedSummonSetup()
        {
            GameObject selected = Selection.activeGameObject;

            if (selected == null)
            {
                EditorUtility.DisplayDialog(
                    "B&D Summon Validation",
                    "Select the encounter root.",
                    "OK"
                );
                return;
            }

            BDBossSharedSummonBudget budget =
                selected.GetComponentInChildren<
                    BDBossSharedSummonBudget
                >(includeInactive: true);

            BDBossSummonEmitter[] emitters =
                selected.GetComponentsInChildren<
                    BDBossSummonEmitter
                >(includeInactive: true);

            int errors = 0;

            if (budget == null)
            {
                Debug.LogError(
                    "Missing BDBossSharedSummonBudget.",
                    selected
                );
                errors++;
            }

            if (emitters.Length == 0)
            {
                Debug.LogWarning(
                    "No BDBossSummonEmitter found.",
                    selected
                );
            }

            for (int i = 0; i < emitters.Length; i++)
            {
                SerializedObject serialized =
                    new SerializedObject(emitters[i]);

                SerializedProperty prefabs =
                    serialized.FindProperty("summonPrefabs");

                if (prefabs == null || prefabs.arraySize == 0)
                {
                    Debug.LogError(
                        $"{emitters[i].name}: no summon prefabs assigned.",
                        emitters[i]
                    );
                    errors++;
                }
            }

            if (errors == 0)
            {
                Debug.Log(
                    $"Summon setup valid. Emitters={emitters.Length}.",
                    selected
                );
            }
        }
    }
}
