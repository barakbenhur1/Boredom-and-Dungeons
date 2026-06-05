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
    public enum BDStabilitySeverity
    {
        Info = 0,
        Warning = 1,
        Blocker = 2
    }

    [Serializable]
    public sealed class BDStabilityFinding
    {
        public BDStabilitySeverity severity;
        public string code;
        public string assetPath;
        public string objectPath;
        public string message;

        public BDStabilityFinding(
            BDStabilitySeverity newSeverity,
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
    public sealed class BDStabilityReport
    {
        public string generatedUtc;
        public string unityVersion;
        public string projectPath;
        public string scope;
        public int blockerCount;
        public int warningCount;
        public int infoCount;
        public List<BDStabilityFinding> findings =
            new List<BDStabilityFinding>();
    }

    public static class BDProjectStabilityGate
    {
        private const string MenuRoot =
            "Boredom And Dungeons/Advanced QA/";

        private const string PrototypeBuilderMenu =
            "Boredom And Dungeons/Create Clean Maze Prototype Scene";

        private const string PrototypeScenePath =
            "Assets/_Project/Scenes/02_CleanCore_MazePrototype.unity";

        private static readonly string[] CriticalSingletonTypeNames =
        {
            "BDPlayerMarker",
            "BDPlayerController",
            "BDPlayerCombat",
            "BDHorseController",
            "BDHorseHealth",
            "BDGameHud",
            "BDMazeMinimap"
        };

        private static readonly string[][] ConflictingTypeFamilies =
        {
            new[]
            {
                "BDMazeMinimap",
                "BDMinimapPerspectiveAlignment"
            },
            new[]
            {
                "BDHorseReliableFleeMotor",
                "BDHorseFleeMotor",
                "BDHorseCombatFleeMotor"
            },
            new[]
            {
                "BDPlayerAttackBuffer",
                "BDMeleeAttackBuffer"
            },
            new[]
            {
                "BDHitStopController",
                "BDGlobalHitStop",
                "BDParryTimeFreezeController"
            }
        };

        private static readonly string[] RuntimeInstallerKeywords =
        {
            "Installer",
            "Bootstrap",
            "RuntimeRepair",
            "AutoSetup"
        };

        [MenuItem(MenuRoot + "Run Full Stability Gate", priority = 1)]
        public static void RunFullGate()
        {
            if (EditorApplication.isCompiling)
            {
                EditorUtility.DisplayDialog(
                    "B&D Stability Gate",
                    "Unity is compiling. Wait for compilation to finish and run the gate again.",
                    "OK"
                );
                return;
            }

            if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                return;

            BDStabilityReport report =
                CreateReport("All project scenes, prefabs and source files");

            SceneSetup[] originalSetup =
                EditorSceneManager.GetSceneManagerSetup();

            try
            {
                ScanRuntimeSource(report);
                ScanMetaGuids(report);
                ScanAllProjectScenes(report);
                ScanAllProjectPrefabs(report);
            }
            finally
            {
                EditorSceneManager.RestoreSceneManagerSetup(originalSetup);
            }

            FinalizeAndWriteReport(report);
        }

        [MenuItem(MenuRoot + "Run Current Scene Gate", priority = 2)]
        public static void RunCurrentSceneGate()
        {
            BDStabilityReport report =
                CreateReport("Loaded scenes only");

            ScanRuntimeSource(report);
            ScanMetaGuids(report);

            for (int index = 0;
                 index < SceneManager.sceneCount;
                 index++)
            {
                Scene scene = SceneManager.GetSceneAt(index);

                if (scene.IsValid() && scene.isLoaded)
                    ScanScene(scene, report);
            }

            FinalizeAndWriteReport(report);
        }

        [MenuItem(
            MenuRoot + "Rebuild Prototype And Run Gate",
            priority = 3)]
        public static void RebuildPrototypeAndRunGate()
        {
            if (EditorApplication.isPlayingOrWillChangePlaymode)
            {
                EditorUtility.DisplayDialog(
                    "B&D Stability Gate",
                    "Stop Play Mode before rebuilding the prototype scene.",
                    "OK"
                );
                return;
            }

            bool executed =
                EditorApplication.ExecuteMenuItem(
                    PrototypeBuilderMenu
                );

            if (!executed)
            {
                Debug.LogError(
                    $"B&D Stability Gate could not execute menu item: " +
                    $"{PrototypeBuilderMenu}"
                );
                return;
            }

            EditorApplication.delayCall += () =>
            {
                AssetDatabase.Refresh();

                if (File.Exists(PrototypeScenePath))
                {
                    Selection.activeObject =
                        AssetDatabase.LoadAssetAtPath<SceneAsset>(
                            PrototypeScenePath
                        );
                }

                RunFullGate();
            };
        }

        [MenuItem(
            MenuRoot + "Open Play Mode Smoke Checklist",
            priority = 20)]
        public static void OpenSmokeChecklist()
        {
            BDPlayModeSmokeChecklistWindow.OpenWindow();
        }

        [MenuItem(
            MenuRoot + "Open Latest Report Folder",
            priority = 30)]
        public static void OpenLatestReportFolder()
        {
            string directory = ResolveReportDirectory();
            Directory.CreateDirectory(directory);
            EditorUtility.RevealInFinder(directory);
        }

        private static BDStabilityReport CreateReport(string scope)
        {
            return new BDStabilityReport
            {
                generatedUtc =
                    DateTime.UtcNow.ToString("O"),
                unityVersion =
                    Application.unityVersion,
                projectPath =
                    ResolveProjectRoot(),
                scope =
                    scope ?? string.Empty
            };
        }

        private static void ScanAllProjectScenes(
            BDStabilityReport report)
        {
            string[] sceneGuids =
                AssetDatabase.FindAssets(
                    "t:Scene",
                    new[] { "Assets/_Project/Scenes" }
                );

            if (sceneGuids.Length == 0)
            {
                AddFinding(
                    report,
                    BDStabilitySeverity.Blocker,
                    "NO_PROJECT_SCENES",
                    "Assets/_Project/Scenes",
                    string.Empty,
                    "No project scenes were found."
                );
                return;
            }

            foreach (string guid in sceneGuids)
            {
                string path =
                    AssetDatabase.GUIDToAssetPath(guid);

                if (string.IsNullOrWhiteSpace(path))
                    continue;

                Scene scene =
                    EditorSceneManager.OpenScene(
                        path,
                        OpenSceneMode.Single
                    );

                ScanScene(scene, report);
            }
        }

        private static void ScanScene(
            Scene scene,
            BDStabilityReport report)
        {
            string assetPath =
                string.IsNullOrWhiteSpace(scene.path)
                    ? scene.name
                    : scene.path;

            GameObject[] roots = scene.GetRootGameObjects();
            List<MonoBehaviour> allBehaviours =
                new List<MonoBehaviour>();

            foreach (GameObject root in roots)
            {
                if (root == null)
                    continue;

                ScanGameObjectHierarchy(
                    root,
                    assetPath,
                    report,
                    allBehaviours
                );
            }

            ValidateCriticalSingletons(
                allBehaviours,
                assetPath,
                report
            );

            ValidateConflictingFamilies(
                allBehaviours,
                assetPath,
                report
            );

            ValidateRuntimeInstallerMultiplicity(
                allBehaviours,
                assetPath,
                report
            );

            ValidatePlayerHorseEnemyRoots(
                allBehaviours,
                assetPath,
                report
            );
        }

        private static void ScanGameObjectHierarchy(
            GameObject root,
            string assetPath,
            BDStabilityReport report,
            List<MonoBehaviour> allBehaviours)
        {
            Transform[] transforms =
                root.GetComponentsInChildren<Transform>(
                    includeInactive: true
                );

            foreach (Transform transform in transforms)
            {
                if (transform == null)
                    continue;

                GameObject gameObject = transform.gameObject;
                string objectPath = BuildObjectPath(transform);

                int missingCount =
                    GameObjectUtility
                        .GetMonoBehavioursWithMissingScriptCount(
                            gameObject
                        );

                if (missingCount > 0)
                {
                    AddFinding(
                        report,
                        BDStabilitySeverity.Blocker,
                        "MISSING_SCRIPT",
                        assetPath,
                        objectPath,
                        $"{missingCount} missing MonoBehaviour script(s)."
                    );
                }

                MonoBehaviour[] behaviours =
                    gameObject.GetComponents<MonoBehaviour>();

                foreach (MonoBehaviour behaviour in behaviours)
                {
                    if (behaviour != null)
                        allBehaviours.Add(behaviour);
                }

                IEnumerable<IGrouping<Type, MonoBehaviour>>
                    duplicateGroups =
                        behaviours
                            .Where(item => item != null)
                            .GroupBy(item => item.GetType())
                            .Where(group => group.Count() > 1);

                foreach (IGrouping<Type, MonoBehaviour> group
                         in duplicateGroups)
                {
                    AddFinding(
                        report,
                        BDStabilitySeverity.Blocker,
                        "DUPLICATE_COMPONENT",
                        assetPath,
                        objectPath,
                        $"{group.Count()} copies of " +
                        $"{group.Key.FullName} are attached " +
                        "to the same GameObject."
                    );
                }
            }
        }

        private static void ValidateCriticalSingletons(
            List<MonoBehaviour> behaviours,
            string assetPath,
            BDStabilityReport report)
        {
            foreach (string typeName
                     in CriticalSingletonTypeNames)
            {
                List<MonoBehaviour> matches =
                    behaviours
                        .Where(item =>
                            item != null &&
                            item.isActiveAndEnabled &&
                            item.GetType().Name == typeName)
                        .ToList();

                if (matches.Count <= 1)
                    continue;

                AddFinding(
                    report,
                    BDStabilitySeverity.Blocker,
                    "DUPLICATE_CRITICAL_SYSTEM",
                    assetPath,
                    string.Join(
                        " | ",
                        matches.Select(item =>
                            BuildObjectPath(item.transform))
                    ),
                    $"{matches.Count} active instances of " +
                    $"{typeName} were found."
                );
            }
        }

        private static void ValidateConflictingFamilies(
            List<MonoBehaviour> behaviours,
            string assetPath,
            BDStabilityReport report)
        {
            foreach (string[] family
                     in ConflictingTypeFamilies)
            {
                List<MonoBehaviour> matches =
                    behaviours
                        .Where(item =>
                            item != null &&
                            item.isActiveAndEnabled &&
                            family.Contains(item.GetType().Name))
                        .ToList();

                int distinctTypes =
                    matches
                        .Select(item => item.GetType().Name)
                        .Distinct()
                        .Count();

                if (distinctTypes <= 1)
                    continue;

                AddFinding(
                    report,
                    BDStabilitySeverity.Blocker,
                    "CONFLICTING_SYSTEM_FAMILY",
                    assetPath,
                    string.Join(
                        " | ",
                        matches.Select(item =>
                            $"{item.GetType().Name}@" +
                            $"{BuildObjectPath(item.transform)}")
                    ),
                    "More than one implementation of the same " +
                    "gameplay responsibility is active."
                );
            }
        }

        private static void ValidateRuntimeInstallerMultiplicity(
            List<MonoBehaviour> behaviours,
            string assetPath,
            BDStabilityReport report)
        {
            List<MonoBehaviour> installers =
                behaviours
                    .Where(item =>
                    {
                        if (item == null ||
                            !item.isActiveAndEnabled)
                        {
                            return false;
                        }

                        string typeName = item.GetType().Name;

                        return RuntimeInstallerKeywords.Any(
                            keyword =>
                                typeName.IndexOf(
                                    keyword,
                                    StringComparison.OrdinalIgnoreCase
                                ) >= 0
                        );
                    })
                    .ToList();

            IEnumerable<IGrouping<string, MonoBehaviour>> groups =
                installers
                    .GroupBy(item => item.GetType().Name)
                    .Where(group => group.Count() > 1);

            foreach (IGrouping<string, MonoBehaviour> group
                     in groups)
            {
                AddFinding(
                    report,
                    BDStabilitySeverity.Warning,
                    "MULTIPLE_RUNTIME_INSTALLERS",
                    assetPath,
                    string.Join(
                        " | ",
                        group.Select(item =>
                            BuildObjectPath(item.transform))
                    ),
                    $"{group.Count()} active instances of " +
                    $"{group.Key} were found. Verify that every " +
                    "instance is required and idempotent."
                );
            }
        }

        private static void ValidatePlayerHorseEnemyRoots(
            List<MonoBehaviour> behaviours,
            string assetPath,
            BDStabilityReport report)
        {
            MonoBehaviour playerMarker =
                behaviours.FirstOrDefault(item =>
                    item != null &&
                    item.GetType().Name == "BDPlayerMarker"
                );

            if (playerMarker == null)
            {
                AddFinding(
                    report,
                    BDStabilitySeverity.Blocker,
                    "PLAYER_MISSING",
                    assetPath,
                    string.Empty,
                    "No BDPlayerMarker was found."
                );
            }
            else
            {
                ValidateRequiredTypesOnObject(
                    playerMarker.gameObject,
                    assetPath,
                    report,
                    "PLAYER_REQUIRED_COMPONENT_MISSING",
                    new[]
                    {
                        "BDPlayerController",
                        "BDPlayerCombat",
                        "BDHealth"
                    }
                );
            }

            MonoBehaviour horseController =
                behaviours.FirstOrDefault(item =>
                    item != null &&
                    item.GetType().Name == "BDHorseController"
                );

            if (horseController == null)
            {
                AddFinding(
                    report,
                    BDStabilitySeverity.Warning,
                    "HORSE_MISSING",
                    assetPath,
                    string.Empty,
                    "No BDHorseController was found."
                );
            }
            else
            {
                ValidateRequiredTypesOnObject(
                    horseController.gameObject,
                    assetPath,
                    report,
                    "HORSE_REQUIRED_COMPONENT_MISSING",
                    new[]
                    {
                        "BDHorseHealth"
                    }
                );
            }

            List<MonoBehaviour> healthComponents =
                behaviours
                    .Where(item =>
                        item != null &&
                        item.GetType().Name == "BDHealth")
                    .ToList();

            foreach (MonoBehaviour health in healthComponents)
            {
                if (health == null)
                    continue;

                GameObject owner = health.gameObject;

                bool isPlayer =
                    owner.GetComponents<MonoBehaviour>()
                        .Any(item =>
                            item != null &&
                            item.GetType().Name ==
                                "BDPlayerMarker"
                        );

                bool isHorse =
                    owner.GetComponents<MonoBehaviour>()
                        .Any(item =>
                            item != null &&
                            item.GetType().Name ==
                                "BDHorseHealth"
                        );

                if (isPlayer || isHorse)
                    continue;

                int healthCount =
                    owner
                        .GetComponents<MonoBehaviour>()
                        .Count(item =>
                            item != null &&
                            item.GetType().Name == "BDHealth"
                        );

                if (healthCount > 1)
                {
                    AddFinding(
                        report,
                        BDStabilitySeverity.Blocker,
                        "ENEMY_DUPLICATE_HEALTH",
                        assetPath,
                        BuildObjectPath(owner.transform),
                        "Enemy root has more than one BDHealth."
                    );
                }
            }
        }

        private static void ValidateRequiredTypesOnObject(
            GameObject target,
            string assetPath,
            BDStabilityReport report,
            string code,
            IEnumerable<string> requiredTypeNames)
        {
            HashSet<string> attached =
                target
                    .GetComponents<MonoBehaviour>()
                    .Where(item => item != null)
                    .Select(item => item.GetType().Name)
                    .ToHashSet();

            foreach (string required in requiredTypeNames)
            {
                if (attached.Contains(required))
                    continue;

                AddFinding(
                    report,
                    BDStabilitySeverity.Blocker,
                    code,
                    assetPath,
                    BuildObjectPath(target.transform),
                    $"Required component {required} is missing."
                );
            }
        }

        private static void ScanAllProjectPrefabs(
            BDStabilityReport report)
        {
            string[] prefabGuids =
                AssetDatabase.FindAssets(
                    "t:Prefab",
                    new[] { "Assets/_Project" }
                );

            foreach (string guid in prefabGuids)
            {
                string path =
                    AssetDatabase.GUIDToAssetPath(guid);

                if (string.IsNullOrWhiteSpace(path))
                    continue;

                GameObject root = null;

                try
                {
                    root =
                        PrefabUtility.LoadPrefabContents(path);

                    if (root == null)
                        continue;

                    List<MonoBehaviour> behaviours =
                        new List<MonoBehaviour>();

                    ScanGameObjectHierarchy(
                        root,
                        path,
                        report,
                        behaviours
                    );
                }
                catch (Exception exception)
                {
                    AddFinding(
                        report,
                        BDStabilitySeverity.Blocker,
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

        private static void ScanRuntimeSource(
            BDStabilityReport report)
        {
            string projectRoot = ResolveProjectRoot();
            string runtimeRoot =
                Path.Combine(
                    projectRoot,
                    "Assets/_Project/Scripts/Runtime"
                );

            if (!Directory.Exists(runtimeRoot))
            {
                AddFinding(
                    report,
                    BDStabilitySeverity.Blocker,
                    "RUNTIME_FOLDER_MISSING",
                    "Assets/_Project/Scripts/Runtime",
                    string.Empty,
                    "Runtime scripts folder does not exist."
                );
                return;
            }

            foreach (string file in Directory.EnumerateFiles(
                         runtimeRoot,
                         "*.cs",
                         SearchOption.AllDirectories))
            {
                string content = File.ReadAllText(file);
                string relative =
                    MakeProjectRelative(file);

                if (content.Contains("using UnityEditor;"))
                {
                    AddFinding(
                        report,
                        BDStabilitySeverity.Blocker,
                        "UNITYEDITOR_IN_RUNTIME",
                        relative,
                        string.Empty,
                        "Runtime source imports UnityEditor."
                    );
                }

                if (content.Contains(
                        "[RuntimeInitializeOnLoadMethod",
                        StringComparison.Ordinal))
                {
                    AddFinding(
                        report,
                        BDStabilitySeverity.Info,
                        "RUNTIME_INSTALLER_PRESENT",
                        relative,
                        string.Empty,
                        "RuntimeInitializeOnLoadMethod is present. " +
                        "Verify that the installer is necessary, " +
                        "idempotent and does not duplicate scene wiring."
                    );
                }
            }
        }

        private static void ScanMetaGuids(
            BDStabilityReport report)
        {
            string projectRoot = ResolveProjectRoot();
            string assetsRoot =
                Path.Combine(projectRoot, "Assets/_Project");

            if (!Directory.Exists(assetsRoot))
                return;

            Dictionary<string, string> guidOwners =
                new Dictionary<string, string>(
                    StringComparer.OrdinalIgnoreCase
                );

            foreach (string metaFile in Directory.EnumerateFiles(
                         assetsRoot,
                         "*.meta",
                         SearchOption.AllDirectories))
            {
                string guid = ReadGuid(metaFile);

                if (string.IsNullOrWhiteSpace(guid))
                    continue;

                string relative =
                    MakeProjectRelative(metaFile);

                if (guidOwners.TryGetValue(
                        guid,
                        out string previousOwner))
                {
                    AddFinding(
                        report,
                        BDStabilitySeverity.Blocker,
                        "DUPLICATE_META_GUID",
                        relative,
                        string.Empty,
                        $"GUID {guid} is also used by " +
                        $"{previousOwner}."
                    );
                }
                else
                {
                    guidOwners.Add(guid, relative);
                }
            }
        }

        private static string ReadGuid(string metaFile)
        {
            foreach (string line in File.ReadLines(metaFile))
            {
                if (!line.StartsWith(
                        "guid:",
                        StringComparison.Ordinal))
                {
                    continue;
                }

                return line.Substring("guid:".Length).Trim();
            }

            return string.Empty;
        }

        private static void AddFinding(
            BDStabilityReport report,
            BDStabilitySeverity severity,
            string code,
            string assetPath,
            string objectPath,
            string message)
        {
            report.findings.Add(
                new BDStabilityFinding(
                    severity,
                    code,
                    assetPath,
                    objectPath,
                    message
                )
            );
        }

        private static void FinalizeAndWriteReport(
            BDStabilityReport report)
        {
            report.findings =
                report.findings
                    .OrderByDescending(item => item.severity)
                    .ThenBy(item => item.code)
                    .ThenBy(item => item.assetPath)
                    .ThenBy(item => item.objectPath)
                    .ToList();

            report.blockerCount =
                report.findings.Count(item =>
                    item.severity == BDStabilitySeverity.Blocker
                );

            report.warningCount =
                report.findings.Count(item =>
                    item.severity == BDStabilitySeverity.Warning
                );

            report.infoCount =
                report.findings.Count(item =>
                    item.severity == BDStabilitySeverity.Info
                );

            string directory = ResolveReportDirectory();
            Directory.CreateDirectory(directory);

            string stamp =
                DateTime.UtcNow.ToString("yyyyMMdd_HHmmss");

            string jsonPath =
                Path.Combine(
                    directory,
                    $"stability_gate_{stamp}.json"
                );

            string textPath =
                Path.Combine(
                    directory,
                    $"stability_gate_{stamp}.txt"
                );

            File.WriteAllText(
                jsonPath,
                JsonUtility.ToJson(report, prettyPrint: true)
            );

            File.WriteAllText(
                textPath,
                BuildTextReport(report)
            );

            string latestJson =
                Path.Combine(
                    directory,
                    "stability_gate_latest.json"
                );

            string latestText =
                Path.Combine(
                    directory,
                    "stability_gate_latest.txt"
                );

            File.Copy(jsonPath, latestJson, overwrite: true);
            File.Copy(textPath, latestText, overwrite: true);

            LogReportToConsole(report, latestText);

            EditorUtility.DisplayDialog(
                "B&D Stability Gate",
                report.blockerCount == 0
                    ? $"PASS\n\nWarnings: " +
                      $"{report.warningCount}\n" +
                      $"Info: {report.infoCount}\n\n" +
                      $"Report:\n{latestText}"
                    : $"BLOCKED\n\nBlockers: " +
                      $"{report.blockerCount}\n" +
                      $"Warnings: {report.warningCount}\n\n" +
                      $"Report:\n{latestText}",
                "OK"
            );
        }

        private static string BuildTextReport(
            BDStabilityReport report)
        {
            StringBuilder builder = new StringBuilder();

            builder.AppendLine("Boredom & Dungeons Stability Gate");
            builder.AppendLine(
                $"Generated UTC: {report.generatedUtc}"
            );
            builder.AppendLine(
                $"Unity: {report.unityVersion}"
            );
            builder.AppendLine(
                $"Scope: {report.scope}"
            );
            builder.AppendLine(
                $"Status: " +
                $"{(report.blockerCount == 0 ? "PASS" : "BLOCKED")}"
            );
            builder.AppendLine(
                $"Blockers: {report.blockerCount}"
            );
            builder.AppendLine(
                $"Warnings: {report.warningCount}"
            );
            builder.AppendLine(
                $"Info: {report.infoCount}"
            );
            builder.AppendLine();

            foreach (BDStabilityFinding finding
                     in report.findings)
            {
                builder.AppendLine(
                    $"[{finding.severity}] {finding.code}"
                );

                if (!string.IsNullOrWhiteSpace(
                        finding.assetPath))
                {
                    builder.AppendLine(
                        $"  Asset: {finding.assetPath}"
                    );
                }

                if (!string.IsNullOrWhiteSpace(
                        finding.objectPath))
                {
                    builder.AppendLine(
                        $"  Object: {finding.objectPath}"
                    );
                }

                builder.AppendLine(
                    $"  {finding.message}"
                );
                builder.AppendLine();
            }

            return builder.ToString();
        }

        private static void LogReportToConsole(
            BDStabilityReport report,
            string reportPath)
        {
            string summary =
                $"B&D Stability Gate: " +
                $"{(report.blockerCount == 0 ? "PASS" : "BLOCKED")} | " +
                $"blockers={report.blockerCount}, " +
                $"warnings={report.warningCount}, " +
                $"info={report.infoCount}\n" +
                $"Report: {reportPath}";

            if (report.blockerCount > 0)
                Debug.LogError(summary);
            else if (report.warningCount > 0)
                Debug.LogWarning(summary);
            else
                Debug.Log(summary);

            foreach (BDStabilityFinding finding
                     in report.findings)
            {
                string line =
                    $"[{finding.code}] " +
                    $"{finding.assetPath} " +
                    $"{finding.objectPath} — " +
                    $"{finding.message}";

                switch (finding.severity)
                {
                    case BDStabilitySeverity.Blocker:
                        Debug.LogError(line);
                        break;

                    case BDStabilitySeverity.Warning:
                        Debug.LogWarning(line);
                        break;

                    default:
                        Debug.Log(line);
                        break;
                }
            }
        }

        private static string BuildObjectPath(
            Transform transform)
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

            return string.Join("/", names);
        }

        private static string ResolveProjectRoot()
        {
            return Directory.GetParent(
                Application.dataPath
            )?.FullName ?? string.Empty;
        }

        private static string ResolveReportDirectory()
        {
            return Path.Combine(
                ResolveProjectRoot(),
                "Library",
                "BoredomAndDungeons",
                "StabilityReports"
            );
        }

        private static string MakeProjectRelative(
            string absolutePath)
        {
            string root =
                ResolveProjectRoot()
                    .Replace('\\', '/')
                    .TrimEnd('/');

            string normalized =
                absolutePath.Replace('\\', '/');

            if (normalized.StartsWith(
                    root + "/",
                    StringComparison.OrdinalIgnoreCase))
            {
                return normalized.Substring(root.Length + 1);
            }

            return normalized;
        }
    }
}
#endif
