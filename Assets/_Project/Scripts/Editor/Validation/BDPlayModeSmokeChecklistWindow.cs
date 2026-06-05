#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace BoredomAndDungeons.EditorTools.Validation
{
    public sealed class BDPlayModeSmokeChecklistWindow :
        EditorWindow
    {
        private const string SessionPrefix =
            "BD.StabilitySmoke.";

        private static readonly SmokeItem[] Items =
        {
            new SmokeItem(
                "movement",
                "Movement",
                "Walk in every direction and verify collision."
            ),
            new SmokeItem(
                "jump",
                "Jump / Landing",
                "Jump, fall and land without getting stuck."
            ),
            new SmokeItem(
                "dodge",
                "Dodge + i-frames",
                "Test forward, backward and side dodge."
            ),
            new SmokeItem(
                "melee",
                "Light / Heavy / Buffer",
                "Test normal attacks and one buffered attack."
            ),
            new SmokeItem(
                "landing_attack",
                "Landing Attack",
                "Verify one correct animation and ×1.2 damage."
            ),
            new SmokeItem(
                "parry",
                "Physical Parry",
                "Verify damage cancellation and time freeze."
            ),
            new SmokeItem(
                "ranged",
                "Tap / Charged Shot",
                "Test tap shot, cancel charge and full charge."
            ),
            new SmokeItem(
                "reload",
                "Automatic Reload",
                "Empty the magazine and do not press fire again."
            ),
            new SmokeItem(
                "horse",
                "Horse",
                "Mount, dismount, damage, heal, buck and flee."
            ),
            new SmokeItem(
                "damage_death",
                "Damage / Death / Reset",
                "Verify one hit, death flow and clean reset."
            ),
            new SmokeItem(
                "minimap",
                "Dynamic Minimap",
                "Rotate the player and verify player-up orientation."
            ),
            new SmokeItem(
                "console",
                "Console",
                "No new exceptions, errors or important warnings."
            )
        };

        private Vector2 scroll;
        private string notes = string.Empty;

        public static void OpenWindow()
        {
            BDPlayModeSmokeChecklistWindow window =
                GetWindow<BDPlayModeSmokeChecklistWindow>(
                    "B&D Smoke Test"
                );

            window.minSize = new Vector2(560f, 540f);
            window.Show();
        }

        private void OnGUI()
        {
            EditorGUILayout.Space(8);

            EditorGUILayout.LabelField(
                "Boredom & Dungeons — Play Mode Smoke Test",
                EditorStyles.boldLabel
            );

            EditorGUILayout.HelpBox(
                "A stage is not complete until compilation, scene " +
                "generation and this smoke test pass. Check items only " +
                "after verifying them in the current generated scene.",
                MessageType.Info
            );

            EditorGUILayout.BeginHorizontal();

            GUI.enabled = !EditorApplication.isPlaying;

            if (GUILayout.Button("Enter Play Mode", GUILayout.Height(28)))
                EditorApplication.isPlaying = true;

            GUI.enabled = EditorApplication.isPlaying;

            if (GUILayout.Button("Exit Play Mode", GUILayout.Height(28)))
                EditorApplication.isPlaying = false;

            GUI.enabled = true;

            if (GUILayout.Button("Reset Checklist", GUILayout.Height(28)))
                ResetChecklist();

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space(8);

            scroll = EditorGUILayout.BeginScrollView(scroll);

            foreach (SmokeItem item in Items)
            {
                bool current =
                    SessionState.GetBool(
                        SessionPrefix + item.key,
                        false
                    );

                EditorGUILayout.BeginVertical("box");

                bool next = EditorGUILayout.ToggleLeft(
                    item.title,
                    current,
                    EditorStyles.boldLabel
                );

                if (next != current)
                {
                    SessionState.SetBool(
                        SessionPrefix + item.key,
                        next
                    );
                }

                EditorGUILayout.LabelField(
                    item.description,
                    EditorStyles.wordWrappedMiniLabel
                );

                EditorGUILayout.EndVertical();
            }

            EditorGUILayout.Space(6);
            EditorGUILayout.LabelField(
                "Notes",
                EditorStyles.boldLabel
            );

            notes = EditorGUILayout.TextArea(
                notes,
                GUILayout.MinHeight(90)
            );

            EditorGUILayout.EndScrollView();

            int passed = CountPassed();
            int total = Items.Length;

            EditorGUILayout.Space(6);

            EditorGUILayout.LabelField(
                $"Passed: {passed}/{total}",
                passed == total
                    ? EditorStyles.boldLabel
                    : EditorStyles.label
            );

            GUI.enabled = passed == total;

            if (GUILayout.Button(
                    "Save PASS Report",
                    GUILayout.Height(34)))
            {
                SaveReport(passed, total);
            }

            GUI.enabled = true;

            if (passed != total)
            {
                EditorGUILayout.HelpBox(
                    "The smoke gate remains blocked until every item " +
                    "is verified.",
                    MessageType.Warning
                );
            }
        }

        private static int CountPassed()
        {
            int count = 0;

            foreach (SmokeItem item in Items)
            {
                if (SessionState.GetBool(
                        SessionPrefix + item.key,
                        false))
                {
                    count++;
                }
            }

            return count;
        }

        private void ResetChecklist()
        {
            foreach (SmokeItem item in Items)
            {
                SessionState.SetBool(
                    SessionPrefix + item.key,
                    false
                );
            }

            notes = string.Empty;
            Repaint();
        }

        private void SaveReport(int passed, int total)
        {
            string projectRoot =
                Directory.GetParent(
                    Application.dataPath
                )?.FullName ?? string.Empty;

            string directory =
                Path.Combine(
                    projectRoot,
                    "Library",
                    "BoredomAndDungeons",
                    "StabilityReports"
                );

            Directory.CreateDirectory(directory);

            string stamp =
                DateTime.UtcNow.ToString("yyyyMMdd_HHmmss");

            string path =
                Path.Combine(
                    directory,
                    $"play_mode_smoke_{stamp}.txt"
                );

            StringBuilder builder = new StringBuilder();

            builder.AppendLine(
                "Boredom & Dungeons Play Mode Smoke Test"
            );
            builder.AppendLine(
                $"Generated UTC: {DateTime.UtcNow:O}"
            );
            builder.AppendLine(
                $"Unity: {Application.unityVersion}"
            );
            builder.AppendLine(
                $"Status: {(passed == total ? "PASS" : "BLOCKED")}"
            );
            builder.AppendLine(
                $"Passed: {passed}/{total}"
            );
            builder.AppendLine();

            foreach (SmokeItem item in Items)
            {
                bool value =
                    SessionState.GetBool(
                        SessionPrefix + item.key,
                        false
                    );

                builder.AppendLine(
                    $"[{(value ? "PASS" : "FAIL")}] " +
                    $"{item.title}"
                );
            }

            if (!string.IsNullOrWhiteSpace(notes))
            {
                builder.AppendLine();
                builder.AppendLine("Notes:");
                builder.AppendLine(notes.Trim());
            }

            File.WriteAllText(path, builder.ToString());

            string latest =
                Path.Combine(
                    directory,
                    "play_mode_smoke_latest.txt"
                );

            File.Copy(path, latest, overwrite: true);

            EditorUtility.DisplayDialog(
                "B&D Smoke Test",
                $"PASS report saved:\n{latest}",
                "OK"
            );

            EditorUtility.RevealInFinder(latest);
        }

        private readonly struct SmokeItem
        {
            public readonly string key;
            public readonly string title;
            public readonly string description;

            public SmokeItem(
                string newKey,
                string newTitle,
                string newDescription)
            {
                key = newKey;
                title = newTitle;
                description = newDescription;
            }
        }
    }
}
#endif
