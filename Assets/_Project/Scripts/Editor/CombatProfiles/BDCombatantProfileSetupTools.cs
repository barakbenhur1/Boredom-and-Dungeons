using UnityEditor;
using UnityEngine;

namespace BoredomAndDungeons.EditorTools
{
    public static class BDCombatantProfileSetupTools
    {
        [MenuItem(
            "Boredom & Dungeons/Combat Profiles/Set Selected/Regular Enemy")]
        private static void SetRegularEnemy()
        {
            ConfigureSelected(
                BDCombatantRank.Regular,
                receivesKnockback: true,
                "Regular Enemy"
            );
        }

        [MenuItem(
            "Boredom & Dungeons/Combat Profiles/Set Selected/Small Mini-Boss")]
        private static void SetSmallMiniBoss()
        {
            ConfigureSelected(
                BDCombatantRank.MiniBoss,
                receivesKnockback: true,
                "Small Mini-Boss"
            );
        }

        [MenuItem(
            "Boredom & Dungeons/Combat Profiles/Set Selected/Large Mini-Boss")]
        private static void SetLargeMiniBoss()
        {
            ConfigureSelected(
                BDCombatantRank.MiniBoss,
                receivesKnockback: false,
                "Large Mini-Boss"
            );
        }

        [MenuItem(
            "Boredom & Dungeons/Combat Profiles/Set Selected/Final Boss")]
        private static void SetFinalBoss()
        {
            ConfigureSelected(
                BDCombatantRank.Boss,
                receivesKnockback: false,
                "Final Boss"
            );
        }

        private static void ConfigureSelected(
            BDCombatantRank rank,
            bool receivesKnockback,
            string label)
        {
            GameObject[] selected = Selection.gameObjects;

            if (selected == null || selected.Length == 0)
            {
                EditorUtility.DisplayDialog(
                    "B&D Combat Profile",
                    "Select at least one enemy root in the Hierarchy.",
                    "OK"
                );
                return;
            }

            int configured = 0;

            foreach (GameObject selectedObject in selected)
            {
                if (selectedObject == null)
                    continue;

                BDHealth health =
                    selectedObject.GetComponent<BDHealth>();

                if (health == null)
                    health = selectedObject.GetComponentInParent<BDHealth>();

                if (health == null)
                {
                    health = selectedObject.GetComponentInChildren<BDHealth>(
                        includeInactive: true
                    );
                }

                if (health == null)
                {
                    Debug.LogWarning(
                        $"Skipped {selectedObject.name}: no BDHealth found.",
                        selectedObject
                    );
                    continue;
                }

                BDCombatantProfile profile =
                    health.GetComponent<BDCombatantProfile>();

                if (profile == null)
                {
                    profile = Undo.AddComponent<BDCombatantProfile>(
                        health.gameObject
                    );
                }

                Undo.RecordObject(
                    profile,
                    $"Set B&D profile: {label}"
                );

                profile.Configure(rank, receivesKnockback);
                EditorUtility.SetDirty(profile);
                configured++;
            }

            Debug.Log(
                $"B&D Combat Profiles: configured {configured} object(s) " +
                $"as {label}."
            );
        }
    }
}
