#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BoredomAndDungeons.EditorTools.Validation
{
    public enum BDOneClickQASeverity
    {
        Info,
        Warning,
        Blocker
    }

    [Serializable]
    public sealed class BDOneClickQAFinding
    {
        public BDOneClickQASeverity severity;
        public string code;
        public string assetPath;
        public string objectPath;
        public string message;

        public BDOneClickQAFinding(
            BDOneClickQASeverity newSeverity,
            string newCode,
            string newAssetPath,
            string newObjectPath,
            string newMessage)
        {
            severity = newSeverity;
            code = newCode ?? string.Empty;
            assetPath = newAssetPath ?? string.Empty;
            objectPath = newObjectPath ?? string.Empty;
            message = newMessage ?? string.Empty;
        }
    }

    [Serializable]
    public sealed class BDOneClickQAResult
    {
        public string generatedUtc;
        public string unityVersion;
        public int blockerCount;
        public int warningCount;
        public int infoCount;
        public List<BDOneClickQAFinding> findings =
            new List<BDOneClickQAFinding>();

        public bool AutomatedPassed => blockerCount == 0;
    }

    public sealed class BDOneClickQAWindow : EditorWindow
    {
        private const string MenuPath =
            "Boredom And Dungeons/TEST EVERYTHING";

        private const string PrototypeScenePath =
            "Assets/_Project/Scenes/02_CleanCore_MazePrototype.unity";

        private const string SessionPrefix =
            "BD.OneClickQA.Manual.";

        private static readonly ManualCheck[] ManualChecks =
        {
            new ManualCheck(
                "movement",
                "Movement and mouse direction",
                "W/S/A/D move in straight lines. Tiny accidental mouse movement does not rotate the player."),
            new ManualCheck(
                "minimap",
                "Minimap",
                "The minimap changes only in 90-degree steps when moving forward/back/left/right, and map edges stay parallel to the frame."),
            new ManualCheck(
                "camera",
                "Camera and forward visibility",
                "The player is lower on screen and there is clearly more visible space ahead."),
            new ManualCheck(
                "ammo",
                "Ammo UI and shooting",
                "The HUD shows the real 3-6 bullet capacity. Tap fires, hold charges, cancel consumes nothing, and an empty magazine reloads automatically."),
            new ManualCheck(
                "parry",
                "Parry",
                "Physical parry cancels the hit, freezes for 1.5 seconds, uses the world ring with no loading bar, and the upgrade changes it to 2.5 seconds."),
            new ManualCheck(
                "horse",
                "Horse",
                "The horse starts beside the player, stays still nearby, flees combat, heals, bucks correctly, and keeps all previous behaviour."),
            new ManualCheck(
                "square_jumper",
                "Square Jumper",
                "Jump, hard landing, bullet hell, visible double swords, summons, enraged phase, death cleanup, and reward chest all work."),
            new ManualCheck(
                "console",
                "Console",
                "No red errors and no edit-mode material or Destroy warnings appear while creating or testing Square Jumper.")
        };

        private static BDOneClickQAResult latestResult;
        private Vector2 findingsScroll;
        private Vector2 manualScroll;

        [MenuItem(MenuPath, priority = -1000)]
        public static void OpenAndRun()
        {
            BDOneClickQAWindow window =
                GetWindow<BDOneClickQAWindow>(
                    "B&D — Test Everything"
                );

            window.minSize = new Vector2(760f, 620f);
            window.Show();
            window.RunEverything();
        }

        private void OnGUI()
        {
            EditorGUILayout.Space(10);

            GUIStyle titleStyle = new GUIStyle(
                EditorStyles.boldLabel
            )
            {
                fontSize = 18
            };

            EditorGUILayout.LabelField(
                "Boredom & Dungeons — TEST EVERYTHING",
                titleStyle
            );

            EditorGUILayout.LabelField(
                "This is the only QA screen you need to use.",
                EditorStyles.wordWrappedLabel
            );

            EditorGUILayout.Space(8);

            if (GUILayout.Button(
                    "RUN EVERYTHING AGAIN",
                    GUILayout.Height(38)))
            {
                RunEverything();
            }

            EditorGUILayout.Space(10);
            DrawAutomatedSection();
            EditorGUILayout.Space(10);
            DrawManualSection();
        }

        private void DrawAutomatedSection()
        {
            EditorGUILayout.BeginVertical("box");
            EditorGUILayout.LabelField(
                "1. AUTOMATED CHECKS",
                EditorStyles.boldLabel
            );

            if (latestResult == null)
            {
                EditorGUILayout.HelpBox(
                    "The automated check has not run yet.",
                    MessageType.Info
                );

                EditorGUILayout.EndVertical();
                return;
            }

            MessageType messageType =
                latestResult.AutomatedPassed
                    ? latestResult.warningCount > 0
                        ? MessageType.Warning
                        : MessageType.Info
                    : MessageType.Error;

            EditorGUILayout.HelpBox(
                $"{(latestResult.AutomatedPassed ? "AUTOMATED PASS" : "AUTOMATED BLOCKED")}\n" +
                $"Blockers: {latestResult.blockerCount} | " +
                $"Warnings: {latestResult.warningCount} | " +
                $"Info: {latestResult.infoCount}",
                messageType
            );

            findingsScroll = EditorGUILayout.BeginScrollView(
                findingsScroll,
                GUILayout.MinHeight(150),
                GUILayout.MaxHeight(250)
            );

            if (latestResult.findings.Count == 0)
            {
                EditorGUILayout.LabelField(
                    "No automated findings.",
                    EditorStyles.wordWrappedLabel
                );
            }

            foreach (BDOneClickQAFinding finding
                     in latestResult.findings)
            {
                GUIStyle style = new GUIStyle(
                    EditorStyles.wordWrappedLabel
                );

                if (finding.severity == BDOneClickQASeverity.Blocker)
                    style.normal.textColor = new Color(1f, 0.32f, 0.28f);
                else if (finding.severity == BDOneClickQASeverity.Warning)
                    style.normal.textColor = new Color(1f, 0.72f, 0.20f);
                else
                    style.normal.textColor = new Color(0.48f, 0.82f, 1f);

                string location =
                    string.IsNullOrWhiteSpace(finding.objectPath)
                        ? finding.assetPath
                        : $"{finding.assetPath} → {finding.objectPath}";

                EditorGUILayout.LabelField(
                    $"[{finding.severity}] {finding.code}\n" +
                    $"{location}\n{finding.message}",
                    style
                );

                EditorGUILayout.Space(5);
            }

            EditorGUILayout.EndScrollView();

            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("Open Report Folder"))
                RevealReportFolder();

            if (GUILayout.Button("Select Prototype Scene"))
            {
                UnityEngine.Object sceneAsset =
                    AssetDatabase.LoadAssetAtPath<SceneAsset>(
                        PrototypeScenePath
                    );

                if (sceneAsset != null)
                {
                    Selection.activeObject = sceneAsset;
                    EditorGUIUtility.PingObject(sceneAsset);
                }
            }

            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();
        }

        private void DrawManualSection()
        {
            EditorGUILayout.BeginVertical("box");
            EditorGUILayout.LabelField(
                "2. ONE PLAY MODE CHECKLIST",
                EditorStyles.boldLabel
            );

            bool automatedReady =
                latestResult != null &&
                latestResult.AutomatedPassed;

            if (!automatedReady)
            {
                EditorGUILayout.HelpBox(
                    "Fix automated blockers first. The Play Mode checklist is the final gate after automated PASS.",
                    MessageType.Warning
                );
            }
            else
            {
                EditorGUILayout.HelpBox(
                    "Press Play once, verify these eight items in order, and check each box. Nothing else needs to be run.",
                    MessageType.Info
                );
            }

            GUI.enabled = automatedReady;

            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button(
                    EditorApplication.isPlaying
                        ? "PLAY MODE IS RUNNING"
                        : "ENTER PLAY MODE",
                    GUILayout.Height(28)))
            {
                if (!EditorApplication.isPlaying)
                    EditorApplication.isPlaying = true;
            }

            if (GUILayout.Button(
                    "RESET MANUAL CHECKLIST",
                    GUILayout.Height(28)))
            {
                ResetManualChecklist();
            }

            EditorGUILayout.EndHorizontal();

            manualScroll = EditorGUILayout.BeginScrollView(
                manualScroll,
                GUILayout.MinHeight(210)
            );

            foreach (ManualCheck check in ManualChecks)
            {
                string key = SessionPrefix + check.key;
                bool current = SessionState.GetBool(key, false);

                EditorGUILayout.BeginVertical("box");

                bool next = EditorGUILayout.ToggleLeft(
                    check.title,
                    current,
                    EditorStyles.boldLabel
                );

                if (next != current)
                    SessionState.SetBool(key, next);

                EditorGUILayout.LabelField(
                    check.description,
                    EditorStyles.wordWrappedMiniLabel
                );

                EditorGUILayout.EndVertical();
            }

            EditorGUILayout.EndScrollView();

            int passed = CountManualPassed();

            EditorGUILayout.LabelField(
                $"Manual Play Mode: {passed}/{ManualChecks.Length}",
                passed == ManualChecks.Length
                    ? EditorStyles.boldLabel
                    : EditorStyles.label
            );

            GUI.enabled =
                automatedReady &&
                passed == ManualChecks.Length;

            if (GUILayout.Button(
                    "SAVE FINAL QA PASS",
                    GUILayout.Height(36)))
            {
                SaveFinalPassReport();
            }

            GUI.enabled = true;

            if (automatedReady && passed == ManualChecks.Length)
            {
                EditorGUILayout.HelpBox(
                    "FINAL QA PASS — automated and manual checks are complete.",
                    MessageType.Info
                );
            }

            EditorGUILayout.EndVertical();
        }

        private void RunEverything()
        {
            if (EditorApplication.isCompiling)
            {
                EditorUtility.DisplayDialog(
                    "TEST EVERYTHING",
                    "Unity is compiling. Wait for compilation to finish, then run TEST EVERYTHING again.",
                    "OK"
                );
                return;
            }

            if (EditorApplication.isPlayingOrWillChangePlaymode)
            {
                EditorUtility.DisplayDialog(
                    "TEST EVERYTHING",
                    "Exit Play Mode before running automated checks.",
                    "OK"
                );
                return;
            }

            if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                return;

            latestResult = ExecuteAutomatedChecks();
            WriteAutomatedReport(latestResult);
            Repaint();
        }

        private static BDOneClickQAResult ExecuteAutomatedChecks()
        {
            BDOneClickQAResult result = new BDOneClickQAResult
            {
                generatedUtc = DateTime.UtcNow.ToString("O"),
                unityVersion = Application.unityVersion
            };

            ScanRequiredFiles(result);
            ScanSource(result);
            ScanMetaGuids(result);
            ScanAllScenes(result);
            ScanAllPrefabs(result);
            ScanSquareJumperAssets(result);

            result.findings = result.findings
                .OrderByDescending(item => item.severity)
                .ThenBy(item => item.code)
                .ThenBy(item => item.assetPath)
                .ThenBy(item => item.objectPath)
                .ToList();

            result.blockerCount = result.findings.Count(
                item => item.severity == BDOneClickQASeverity.Blocker
            );
            result.warningCount = result.findings.Count(
                item => item.severity == BDOneClickQASeverity.Warning
            );
            result.infoCount = result.findings.Count(
                item => item.severity == BDOneClickQASeverity.Info
            );

            if (result.blockerCount > 0)
            {
                Debug.LogError(
                    $"B&D TEST EVERYTHING: BLOCKED | blockers={result.blockerCount}, warnings={result.warningCount}"
                );
            }
            else if (result.warningCount > 0)
            {
                Debug.LogWarning(
                    $"B&D TEST EVERYTHING: AUTOMATED PASS WITH WARNINGS | warnings={result.warningCount}"
                );
            }
            else
            {
                Debug.Log("B&D TEST EVERYTHING: AUTOMATED PASS");
            }

            return result;
        }

        private static void ScanRequiredFiles(BDOneClickQAResult result)
        {
            string[] requiredPaths =
            {
                "Assets/_Project/Scripts/Runtime/BDPlayerController.cs",
                "Assets/_Project/Scripts/Runtime/BDPlayerCombat.cs",
                "Assets/_Project/Scripts/Runtime/BDCameraFollow.cs",
                "Assets/_Project/Scripts/Runtime/BDMazeMinimap.cs",
                "Assets/_Project/Scripts/Runtime/BDGameHud.cs",
                "Assets/_Project/Scripts/Runtime/Combat/BDPlayerParryState.cs",
                "Assets/_Project/Scripts/Runtime/Combat/BDParrySystem.cs",
                PrototypeScenePath
            };

            string projectRoot = ResolveProjectRoot();

            foreach (string relativePath in requiredPaths)
            {
                string absolutePath = Path.Combine(
                    projectRoot,
                    relativePath
                );

                if (!File.Exists(absolutePath))
                {
                    Add(
                        result,
                        BDOneClickQASeverity.Blocker,
                        "REQUIRED_FILE_MISSING",
                        relativePath,
                        string.Empty,
                        "Required project file is missing."
                    );
                }
            }
        }

        private static void ScanSource(BDOneClickQAResult result)
        {
            string root = ResolveProjectRoot();
            string runtimeRoot = Path.Combine(
                root,
                "Assets/_Project/Scripts/Runtime"
            );
            string editorRoot = Path.Combine(
                root,
                "Assets/_Project/Scripts/Editor"
            );

            string rendererMaterialToken =
                "renderer" + ".material";
            string bladeMaterialToken =
                "bladeRenderer" + ".material";

            if (Directory.Exists(runtimeRoot))
            {
                foreach (string file in Directory.EnumerateFiles(
                             runtimeRoot,
                             "*.cs",
                             SearchOption.AllDirectories))
                {
                    string text = File.ReadAllText(file);
                    string relative = MakeRelative(file);

                    if (text.Contains("using UnityEditor;"))
                    {
                        Add(
                            result,
                            BDOneClickQASeverity.Blocker,
                            "UNITYEDITOR_IN_RUNTIME",
                            relative,
                            string.Empty,
                            "Runtime code imports UnityEditor."
                        );
                    }

                    if (relative.EndsWith(
                            "BDSquareJumperMiniBoss.cs",
                            StringComparison.OrdinalIgnoreCase))
                    {
                        if (text.Contains(bladeMaterialToken))
                        {
                            Add(
                                result,
                                BDOneClickQASeverity.Blocker,
                                "SQUARE_JUMPER_EDIT_MATERIAL_LEAK",
                                relative,
                                string.Empty,
                                "Sword creation still calls bladeRenderer.material."
                            );
                        }

                        if (text.Contains("Destroy(bladeCollider)"))
                        {
                            Add(
                                result,
                                BDOneClickQASeverity.Blocker,
                                "SQUARE_JUMPER_EDIT_DESTROY",
                                relative,
                                string.Empty,
                                "Sword creation still calls Destroy while it can run in Edit Mode."
                            );
                        }
                    }
                }
            }

            if (Directory.Exists(editorRoot))
            {
                foreach (string file in Directory.EnumerateFiles(
                             editorRoot,
                             "*.cs",
                             SearchOption.AllDirectories))
                {
                    string text = File.ReadAllText(file);
                    string relative = MakeRelative(file);

                    // BD ONE-CLICK QA SELF-SCAN FALSE-POSITIVE FIX V2
                    // The validator contains the forbidden text inside its own
                    // diagnostics and search expressions. Skip only this validator
                    // file so it cannot report itself as a material-access violation.
                    if (relative.EndsWith(
                            "Assets/_Project/Scripts/Editor/Validation/" +
                            "BDOneClickQAWindow.cs",
                            StringComparison.OrdinalIgnoreCase))
                    {
                        continue;
                    }


                    if (text.Contains(rendererMaterialToken) ||
                        text.Contains(bladeMaterialToken))
                    {
                        Add(
                            result,
                            BDOneClickQASeverity.Blocker,
                            "EDITOR_RENDERER_MATERIAL_ACCESS",
                            relative,
                            string.Empty,
                            "Editor code calls renderer.material. Use a reusable material asset through sharedMaterial."
                        );
                    }
                }
            }
        }

        private static void ScanMetaGuids(BDOneClickQAResult result)
        {
            string assetsRoot = Path.Combine(
                ResolveProjectRoot(),
                "Assets/_Project"
            );

            if (!Directory.Exists(assetsRoot))
                return;

            Dictionary<string, string> owners =
                new Dictionary<string, string>(
                    StringComparer.OrdinalIgnoreCase
                );

            foreach (string file in Directory.EnumerateFiles(
                         assetsRoot,
                         "*.meta",
                         SearchOption.AllDirectories))
            {
                string guid = string.Empty;

                foreach (string line in File.ReadLines(file))
                {
                    if (!line.StartsWith("guid:", StringComparison.Ordinal))
                        continue;

                    guid = line.Substring("guid:".Length).Trim();
                    break;
                }

                if (string.IsNullOrWhiteSpace(guid))
                    continue;

                string relative = MakeRelative(file);

                if (owners.TryGetValue(guid, out string previous))
                {
                    Add(
                        result,
                        BDOneClickQASeverity.Blocker,
                        "DUPLICATE_META_GUID",
                        relative,
                        string.Empty,
                        $"GUID {guid} is also used by {previous}."
                    );
                }
                else
                {
                    owners.Add(guid, relative);
                }
            }
        }

        private static void ScanAllScenes(BDOneClickQAResult result)
        {
            string[] sceneGuids = AssetDatabase.FindAssets(
                "t:Scene",
                new[] { "Assets/_Project/Scenes" }
            );

            if (sceneGuids.Length == 0)
            {
                Add(
                    result,
                    BDOneClickQASeverity.Blocker,
                    "NO_SCENES",
                    "Assets/_Project/Scenes",
                    string.Empty,
                    "No project scenes were found."
                );
                return;
            }

            SceneSetup[] originalSetup =
                EditorSceneManager.GetSceneManagerSetup();

            try
            {
                foreach (string guid in sceneGuids)
                {
                    string path = AssetDatabase.GUIDToAssetPath(guid);
                    Scene scene = EditorSceneManager.OpenScene(
                        path,
                        OpenSceneMode.Single
                    );

                    ScanSceneObjects(scene, path, result);
                }
            }
            catch (Exception exception)
            {
                Add(
                    result,
                    BDOneClickQASeverity.Blocker,
                    "SCENE_SCAN_FAILED",
                    string.Empty,
                    string.Empty,
                    exception.Message
                );
            }
            finally
            {
                EditorSceneManager.RestoreSceneManagerSetup(originalSetup);
            }
        }

        private static void ScanSceneObjects(
            Scene scene,
            string assetPath,
            BDOneClickQAResult result)
        {
            Dictionary<string, List<string>> activeCritical =
                new Dictionary<string, List<string>>(
                    StringComparer.Ordinal
                );

            HashSet<string> critical = new HashSet<string>
            {
                "BDPlayerMarker",
                "BDPlayerController",
                "BDPlayerCombat",
                "BDHorseController",
                "BDHorseHealth",
                "BDGameHud",
                "BDMazeMinimap"
            };

            foreach (GameObject root in scene.GetRootGameObjects())
            {
                foreach (Transform transform
                         in root.GetComponentsInChildren<Transform>(true))
                {
                    GameObject gameObject = transform.gameObject;
                    string objectPath = BuildObjectPath(transform);

                    int missing =
                        GameObjectUtility.GetMonoBehavioursWithMissingScriptCount(
                            gameObject
                        );

                    if (missing > 0)
                    {
                        Add(
                            result,
                            BDOneClickQASeverity.Blocker,
                            "MISSING_SCRIPT",
                            assetPath,
                            objectPath,
                            $"{missing} missing script(s)."
                        );
                    }

                    MonoBehaviour[] behaviours =
                        gameObject.GetComponents<MonoBehaviour>();

                    foreach (IGrouping<Type, MonoBehaviour> group
                             in behaviours
                                 .Where(item => item != null)
                                 .GroupBy(item => item.GetType())
                                 .Where(group => group.Count() > 1))
                    {
                        Add(
                            result,
                            BDOneClickQASeverity.Blocker,
                            "DUPLICATE_COMPONENT",
                            assetPath,
                            objectPath,
                            $"{group.Count()} copies of {group.Key.Name} are attached."
                        );
                    }

                    foreach (MonoBehaviour behaviour in behaviours)
                    {
                        if (behaviour == null ||
                            !behaviour.isActiveAndEnabled)
                        {
                            continue;
                        }

                        string typeName = behaviour.GetType().Name;

                        if (!critical.Contains(typeName))
                            continue;

                        if (!activeCritical.TryGetValue(
                                typeName,
                                out List<string> paths))
                        {
                            paths = new List<string>();
                            activeCritical.Add(typeName, paths);
                        }

                        paths.Add(objectPath);
                    }
                }
            }

            foreach (KeyValuePair<string, List<string>> pair
                     in activeCritical)
            {
                if (pair.Value.Count <= 1)
                    continue;

                Add(
                    result,
                    BDOneClickQASeverity.Blocker,
                    "DUPLICATE_CRITICAL_SYSTEM",
                    assetPath,
                    string.Join(" | ", pair.Value),
                    $"{pair.Value.Count} active {pair.Key} components exist."
                );
            }
        }

        private static void ScanAllPrefabs(BDOneClickQAResult result)
        {
            string[] prefabGuids = AssetDatabase.FindAssets(
                "t:Prefab",
                new[] { "Assets/_Project" }
            );

            foreach (string guid in prefabGuids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                GameObject root = null;

                try
                {
                    root = PrefabUtility.LoadPrefabContents(path);

                    foreach (Transform transform
                             in root.GetComponentsInChildren<Transform>(true))
                    {
                        int missing =
                            GameObjectUtility.GetMonoBehavioursWithMissingScriptCount(
                                transform.gameObject
                            );

                        if (missing > 0)
                        {
                            Add(
                                result,
                                BDOneClickQASeverity.Blocker,
                                "PREFAB_MISSING_SCRIPT",
                                path,
                                BuildObjectPath(transform),
                                $"{missing} missing script(s)."
                            );
                        }
                    }
                }
                catch (Exception exception)
                {
                    Add(
                        result,
                        BDOneClickQASeverity.Blocker,
                        "PREFAB_SCAN_FAILED",
                        path,
                        string.Empty,
                        exception.Message
                    );
                }
                finally
                {
                    if (root != null)
                        PrefabUtility.UnloadPrefabContents(root);
                }
            }
        }

        private static void ScanSquareJumperAssets(
            BDOneClickQAResult result)
        {
            string[] scriptGuids = AssetDatabase.FindAssets(
                "BDSquareJumperMiniBoss t:MonoScript",
                new[] { "Assets/_Project/Scripts" }
            );

            if (scriptGuids.Length == 0)
            {
                Add(
                    result,
                    BDOneClickQASeverity.Warning,
                    "SQUARE_JUMPER_NOT_INSTALLED",
                    string.Empty,
                    string.Empty,
                    "Square Jumper scripts are not installed yet."
                );
                return;
            }

            string[] summonPrefabNames =
            {
                "BD_Summon_SwordEnemy",
                "BD_Summon_ShooterEnemy",
                "BD_Summon_PatrolEnemy"
            };

            foreach (string prefabName in summonPrefabNames)
            {
                string[] matches = AssetDatabase.FindAssets(
                    $"{prefabName} t:Prefab",
                    new[] { "Assets/_Project" }
                );

                if (matches.Length == 0)
                {
                    Add(
                        result,
                        BDOneClickQASeverity.Warning,
                        "SQUARE_JUMPER_SUMMON_PREFAB_MISSING",
                        prefabName,
                        string.Empty,
                        "Create or assign the equivalent real summon prefab."
                    );
                }
            }
        }

        private static void Add(
            BDOneClickQAResult result,
            BDOneClickQASeverity severity,
            string code,
            string assetPath,
            string objectPath,
            string message)
        {
            result.findings.Add(
                new BDOneClickQAFinding(
                    severity,
                    code,
                    assetPath,
                    objectPath,
                    message
                )
            );
        }

        private static int CountManualPassed()
        {
            int count = 0;

            foreach (ManualCheck check in ManualChecks)
            {
                if (SessionState.GetBool(
                        SessionPrefix + check.key,
                        false))
                {
                    count++;
                }
            }

            return count;
        }

        private static void ResetManualChecklist()
        {
            foreach (ManualCheck check in ManualChecks)
            {
                SessionState.SetBool(
                    SessionPrefix + check.key,
                    false
                );
            }
        }

        private static void WriteAutomatedReport(
            BDOneClickQAResult result)
        {
            string directory = ResolveReportDirectory();
            Directory.CreateDirectory(directory);

            string path = Path.Combine(
                directory,
                "ONE_CLICK_QA_latest.txt"
            );

            StringBuilder builder = new StringBuilder();
            builder.AppendLine("Boredom & Dungeons — TEST EVERYTHING");
            builder.AppendLine($"Generated UTC: {result.generatedUtc}");
            builder.AppendLine($"Unity: {result.unityVersion}");
            builder.AppendLine(
                $"Automated status: {(result.AutomatedPassed ? "PASS" : "BLOCKED")}"
            );
            builder.AppendLine($"Blockers: {result.blockerCount}");
            builder.AppendLine($"Warnings: {result.warningCount}");
            builder.AppendLine($"Info: {result.infoCount}");
            builder.AppendLine();

            foreach (BDOneClickQAFinding finding in result.findings)
            {
                builder.AppendLine($"[{finding.severity}] {finding.code}");

                if (!string.IsNullOrWhiteSpace(finding.assetPath))
                    builder.AppendLine($"  Asset: {finding.assetPath}");

                if (!string.IsNullOrWhiteSpace(finding.objectPath))
                    builder.AppendLine($"  Object: {finding.objectPath}");

                builder.AppendLine($"  {finding.message}");
                builder.AppendLine();
            }

            File.WriteAllText(path, builder.ToString());
        }

        private static void SaveFinalPassReport()
        {
            if (latestResult == null ||
                !latestResult.AutomatedPassed ||
                CountManualPassed() != ManualChecks.Length)
            {
                return;
            }

            string directory = ResolveReportDirectory();
            Directory.CreateDirectory(directory);

            string path = Path.Combine(
                directory,
                "ONE_CLICK_QA_FINAL_PASS.txt"
            );

            StringBuilder builder = new StringBuilder();
            builder.AppendLine("Boredom & Dungeons — FINAL QA PASS");
            builder.AppendLine($"Generated UTC: {DateTime.UtcNow:O}");
            builder.AppendLine($"Unity: {Application.unityVersion}");
            builder.AppendLine("Automated: PASS");
            builder.AppendLine(
                $"Manual: {ManualChecks.Length}/{ManualChecks.Length} PASS"
            );
            builder.AppendLine();

            foreach (ManualCheck check in ManualChecks)
                builder.AppendLine($"[PASS] {check.title}");

            File.WriteAllText(path, builder.ToString());

            EditorUtility.DisplayDialog(
                "TEST EVERYTHING",
                $"FINAL QA PASS saved:\n{path}",
                "OK"
            );

            EditorUtility.RevealInFinder(path);
        }

        private static void RevealReportFolder()
        {
            string directory = ResolveReportDirectory();
            Directory.CreateDirectory(directory);
            EditorUtility.RevealInFinder(directory);
        }

        private static string ResolveReportDirectory()
        {
            return Path.Combine(
                ResolveProjectRoot(),
                "Library",
                "BoredomAndDungeons",
                "OneClickQA"
            );
        }

        private static string ResolveProjectRoot()
        {
            DirectoryInfo parent = Directory.GetParent(
                Application.dataPath
            );

            return parent != null ? parent.FullName : string.Empty;
        }

        private static string MakeRelative(string absolutePath)
        {
            string root = ResolveProjectRoot()
                .Replace('\\', '/')
                .TrimEnd('/');

            string normalized = absolutePath.Replace('\\', '/');

            if (normalized.StartsWith(
                    root + "/",
                    StringComparison.OrdinalIgnoreCase))
            {
                return normalized.Substring(root.Length + 1);
            }

            return normalized;
        }

        private static string BuildObjectPath(Transform transform)
        {
            if (transform == null)
                return string.Empty;

            Stack<string> names = new Stack<string>();
            Transform current = transform;

            while (current != null)
            {
                names.Push(current.name);
                current = current.parent;
            }

            return string.Join("/", names.ToArray());
        }

        private struct ManualCheck
        {
            public readonly string key;
            public readonly string title;
            public readonly string description;

            public ManualCheck(
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
