using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace BoredomAndDungeons.EditorTools
{
    public static class BDStage16BossSetupTools
    {
        [MenuItem(
            "Boredom & Dungeons/Bosses/Stage 16/Prepare Selected Encounter Root")]
        private static void PrepareSelectedEncounterRoot()
        {
            GameObject root = Selection.activeGameObject;

            if (root == null)
            {
                EditorUtility.DisplayDialog(
                    "B&D Stage 16",
                    "Select the boss encounter root in the Hierarchy.",
                    "OK"
                );
                return;
            }

            Undo.RegisterFullObjectHierarchyUndo(
                root,
                "Prepare B&D Stage 16 Encounter"
            );

            BDBossEncounterController encounter =
                GetOrAdd<BDBossEncounterController>(root);

            BDBossHealthGroup healthGroup =
                GetOrAdd<BDBossHealthGroup>(root);

            BDBossEncounterRuntimeBindings bindings =
                GetOrAdd<BDBossEncounterRuntimeBindings>(root);

            GetOrAdd<BDBossHealthHud>(root);

            BDBossRewardChest rewardChest =
                root.GetComponentInChildren<BDBossRewardChest>(
                    includeInactive: true
                );

            BDBossArenaBarrier[] barriers =
                root.GetComponentsInChildren<BDBossArenaBarrier>(
                    includeInactive: true
                );

            BDBossArenaTrigger[] triggers =
                root.GetComponentsInChildren<BDBossArenaTrigger>(
                    includeInactive: true
                );

            SerializedObject serializedBindings =
                new SerializedObject(bindings);

            serializedBindings.FindProperty("encounter").objectReferenceValue =
                encounter;
            serializedBindings.FindProperty("healthGroup").objectReferenceValue =
                healthGroup;
            serializedBindings.FindProperty("rewardChest").objectReferenceValue =
                rewardChest;

            WriteObjectArray(
                serializedBindings.FindProperty("barriers"),
                barriers
            );

            WriteObjectArray(
                serializedBindings.FindProperty("arenaTriggers"),
                triggers
            );

            serializedBindings.ApplyModifiedPropertiesWithoutUndo();

            for (int i = 0; i < triggers.Length; i++)
            {
                if (triggers[i] == null)
                    continue;

                triggers[i].Configure(encounter);
                EditorUtility.SetDirty(triggers[i]);
            }

            healthGroup.RefreshChannels();
            bindings.RefreshReferences();

            EditorUtility.SetDirty(encounter);
            EditorUtility.SetDirty(healthGroup);
            EditorUtility.SetDirty(bindings);
            EditorSceneManager.MarkSceneDirty(root.scene);

            Debug.Log(
                $"B&D Stage 16 prepared: {root.name}. " +
                $"Barriers={barriers.Length}, " +
                $"Triggers={triggers.Length}, " +
                $"Chest={(rewardChest != null ? "yes" : "no")}.",
                root
            );
        }

        [MenuItem(
            "Boredom & Dungeons/Bosses/Stage 16/Add Health Bridge To Selected Boss Body")]
        private static void AddHealthBridgeToSelectedBossBody()
        {
            GameObject selected = Selection.activeGameObject;

            if (selected == null)
            {
                EditorUtility.DisplayDialog(
                    "B&D Stage 16",
                    "Select the boss body that contains BDHealth.",
                    "OK"
                );
                return;
            }

            BDHealth health =
                selected.GetComponent<BDHealth>();

            if (health == null)
            {
                EditorUtility.DisplayDialog(
                    "B&D Stage 16",
                    "The selected object must contain BDHealth.",
                    "OK"
                );
                return;
            }

            GetOrAdd<BDBossHealthChannel>(selected);
            GetOrAdd<BDBossHealthDamageBridge>(selected);

            EditorUtility.SetDirty(selected);
            EditorSceneManager.MarkSceneDirty(selected.scene);

            Debug.Log(
                $"B&D Stage 16 health bridge added to {selected.name}.",
                selected
            );
        }

        private static T GetOrAdd<T>(GameObject target)
            where T : Component
        {
            T component = target.GetComponent<T>();

            if (component == null)
                component = Undo.AddComponent<T>(target);

            return component;
        }

        private static void WriteObjectArray<T>(
            SerializedProperty property,
            T[] values)
            where T : Object
        {
            property.arraySize = values != null ? values.Length : 0;

            for (int i = 0; i < property.arraySize; i++)
            {
                property.GetArrayElementAtIndex(i).objectReferenceValue =
                    values[i];
            }
        }
    }
}
