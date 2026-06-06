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
                "hazards",
                "Player and horse environmental hazards",
                "Walking into a hole/chasm is rejected without damage. Jumping or dodging into it causes 15 damage. Walking, jumping, dodging, knockback, forced movement, or mounted entry into lava causes exactly 10 damage. Mounted recovery returns the player on foot, and the horse avoids hazards proactively."),
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

            if (EditorUtility.scriptCompilationFailed)
            {
                latestResult = new BDOneClickQAResult
                {
                    generatedUtc = DateTime.UtcNow.ToString("O"),
                    unityVersion = Application.unityVersion,
                    blockerCount = 1,
                    warningCount = 0,
                    infoCount = 0
                };

                latestResult.findings.Add(
                    new BDOneClickQAFinding(
                        BDOneClickQASeverity.Blocker,
                        "UNITY_SCRIPT_COMPILATION_FAILED",
                        string.Empty,
                        string.Empty,
                        "Unity reports C# compilation errors. Clear every Console compiler error before TEST EVERYTHING can pass."
                    )
                );

                WriteAutomatedReport(latestResult);
                Repaint();

                EditorUtility.DisplayDialog(
                    "TEST EVERYTHING — BLOCKED",
                    "Unity still has C# compilation errors. Fix the Console errors, wait for compilation to finish, and run TEST EVERYTHING again.",
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

            if (!BDPrototypeHazardSceneInstaller.TryEnsureInstalled(
                    PrototypeScenePath,
                    out string hazardInstallError))
            {
                Debug.LogError(
                    "B&D hazard scene integration failed: " +
                    hazardInstallError
                );
            }

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
            // BD UNIFIED CAMERA/MINIMAP + REPOSITORY HYGIENE QA V1
            ScanCameraMinimapRegression(result);
            ScanHazardRecoveryContracts(result);
            ScanJumpTimestampDeclaration(result);
            ScanPrototypeHazardScene(result);
            ScanArchitectureContracts(result);
            ScanRepositoryHygiene(result);
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


        // BD UNIFIED QA METHODS V1
        private static void ScanCameraMinimapRegression(
            BDOneClickQAResult result)
        {
            string root = ResolveProjectRoot();

            string cameraRelative =
                "Assets/_Project/Scripts/Runtime/BDCameraFollow.cs";
            string minimapRelative =
                "Assets/_Project/Scripts/Runtime/BDMazeMinimap.cs";

            string cameraPath = Path.Combine(root, cameraRelative);
            string minimapPath = Path.Combine(root, minimapRelative);

            if (!File.Exists(cameraPath) || !File.Exists(minimapPath))
            {
                Add(
                    result,
                    BDOneClickQASeverity.Blocker,
                    "CAMERA_MINIMAP_SOURCE_MISSING",
                    !File.Exists(cameraPath)
                        ? cameraRelative
                        : minimapRelative,
                    string.Empty,
                    "Camera/minimap regression source is missing."
                );
                return;
            }

            string camera = File.ReadAllText(cameraPath);
            string minimap = File.ReadAllText(minimapPath);

            string[] obsoleteTokens =
            {
                "rotationSpeedDegreesPerSecond",
                "snapToMovementCardinals",
                "mapRotationInitialized",
                "minimumMovementDirectionMagnitude",
                "rotateOnlyWhenActuallyMoving"
            };

            foreach (string token in obsoleteTokens)
            {
                if (!camera.Contains(token) && !minimap.Contains(token))
                    continue;

                Add(
                    result,
                    BDOneClickQASeverity.Blocker,
                    "OBSOLETE_CAMERA_MINIMAP_FIELD_RETURNED",
                    camera.Contains(token)
                        ? cameraRelative
                        : minimapRelative,
                    string.Empty,
                    $"Obsolete field/reference returned: {token}."
                );
            }

            ValidateRequiredSourceTokens(
                result,
                cameraRelative,
                camera,
                new[]
                {
                    "cameraYawDegreesPerSecond",
                    "movementDirectionBlend",
                    "ResolveCameraIntentDirection",
                    "Vector3.RotateTowards",
                    "LastMountedAimDirection",
                    "LastLookDirection"
                },
                "CAMERA_REGRESSION_ANCHOR_MISSING"
            );

            ValidateRequiredSourceTokens(
                result,
                minimapRelative,
                minimap,
                new[]
                {
                    "rotateWithPlayerDirection",
                    "movementSnapThreshold",
                    "diagonalBoundaryHoldEpsilon",
                    "TryResolveMovementCardinalRotation",
                    "currentMapRotationDegrees",
                    "GUIUtility.RotateAroundPivot",
                    "LastMountedMovementDirection",
                    "LastMoveWorldDirection"
                },
                "MINIMAP_REGRESSION_ANCHOR_MISSING"
            );
        }
        private static void ScanPrototypeHazardScene(
            BDOneClickQAResult result)
        {
            Scene active = SceneManager.GetActiveScene();

            if (!active.IsValid() ||
                active.path != PrototypeScenePath)
            {
                Add(
                    result,
                    BDOneClickQASeverity.Blocker,
                    "HAZARD_PROTOTYPE_SCENE_NOT_ACTIVE",
                    PrototypeScenePath,
                    string.Empty,
                    "TEST EVERYTHING could not open the prototype scene."
                );
                return;
            }

            GameObject[] roots = active.GetRootGameObjects();
            List<GameObject> hazardRoots = roots
                .Where(item =>
                    item != null &&
                    item.name ==
                        BDPrototypeHazardSceneInstaller.RootName)
                .ToList();

            if (hazardRoots.Count != 1)
            {
                Add(
                    result,
                    BDOneClickQASeverity.Blocker,
                    "HAZARD_TEST_ROOT_COUNT_INVALID",
                    PrototypeScenePath,
                    string.Empty,
                    $"Expected exactly one hazard test root, found {hazardRoots.Count}."
                );
                return;
            }

            BDHazardVolume[] volumes =
                hazardRoots[0].GetComponentsInChildren<BDHazardVolume>(
                    true
                );

            int holes = volumes.Count(
                item =>
                    item != null &&
                    item.HazardType ==
                        BDHazardType.HoleOrChasm
            );
            int lava = volumes.Count(
                item =>
                    item != null &&
                    item.HazardType ==
                        BDHazardType.Lava
            );

            if (holes != 1 || lava != 1)
            {
                Add(
                    result,
                    BDOneClickQASeverity.Blocker,
                    "HAZARD_TEST_VOLUME_SET_INVALID",
                    PrototypeScenePath,
                    BDPrototypeHazardSceneInstaller.RootName,
                    $"Expected one hole/chasm and one lava volume; found hole={holes}, lava={lava}."
                );
            }

            foreach (BDHazardVolume volume in volumes)
            {
                Collider collider =
                    volume != null
                        ? volume.GetComponent<Collider>()
                        : null;

                if (collider != null && collider.isTrigger)
                    continue;

                Add(
                    result,
                    BDOneClickQASeverity.Blocker,
                    "HAZARD_VOLUME_NOT_TRIGGER",
                    PrototypeScenePath,
                    volume != null ? volume.name : string.Empty,
                    "Every BDHazardVolume must use a trigger collider."
                );
            }

            BDPlayerMarker player =
                UnityEngine.Object.FindFirstObjectByType<BDPlayerMarker>();
            if (player == null ||
                player.GetComponent<BDPlayerHazardRecovery>() == null)
            {
                Add(
                    result,
                    BDOneClickQASeverity.Blocker,
                    "PLAYER_HAZARD_RECOVERY_NOT_SERIALIZED",
                    PrototypeScenePath,
                    player != null ? player.name : string.Empty,
                    "The prototype player must serialize BDPlayerHazardRecovery."
                );
            }

            BDHorseController horse =
                UnityEngine.Object.FindFirstObjectByType<BDHorseController>();
            if (horse == null ||
                horse.GetComponent<BDHorseHazardSafety>() == null)
            {
                Add(
                    result,
                    BDOneClickQASeverity.Blocker,
                    "HORSE_HAZARD_SAFETY_NOT_SERIALIZED",
                    PrototypeScenePath,
                    horse != null ? horse.name : string.Empty,
                    "The prototype horse must serialize BDHorseHazardSafety."
                );
            }
        }
        private static void ScanJumpTimestampDeclaration(
            BDOneClickQAResult result)
        {
            const string relativePath =
                "Assets/_Project/Scripts/Runtime/BDPlayerController.cs";
            string absolutePath = Path.Combine(
                ResolveProjectRoot(),
                relativePath
            );

            if (!File.Exists(absolutePath))
            {
                Add(
                    result,
                    BDOneClickQASeverity.Blocker,
                    "PLAYER_CONTROLLER_SOURCE_MISSING",
                    relativePath,
                    string.Empty,
                    "BDPlayerController.cs is required for hazard intent validation."
                );
                return;
            }

            string source = File.ReadAllText(absolutePath);
            const string declaration =
                "private float lastJumpStartedAt = -999f;";

            int first = source.IndexOf(
                declaration,
                StringComparison.Ordinal
            );
            int last = source.LastIndexOf(
                declaration,
                StringComparison.Ordinal
            );

            if (first >= 0 && first == last)
                return;

            Add(
                result,
                BDOneClickQASeverity.Blocker,
                "JUMP_TIMESTAMP_DECLARATION_INVALID",
                relativePath,
                string.Empty,
                first < 0
                    ? "The lastJumpStartedAt field declaration is missing."
                    : "The lastJumpStartedAt field is declared more than once."
            );
        }


        private static void ScanHazardRecoveryContracts(
            BDOneClickQAResult result)
        {
            string root = ResolveProjectRoot();

            string[] relativePaths =
            {
                "Assets/_Project/Scripts/Runtime/Hazards/BDHazardType.cs",
                "Assets/_Project/Scripts/Runtime/Hazards/BDHazardVolume.cs",
                "Assets/_Project/Scripts/Runtime/Hazards/BDPlayerHazardRecovery.cs",
                "Assets/_Project/Scripts/Runtime/Hazards/BDHorseHazardSafety.cs",
                "Assets/_Project/Scripts/Editor/Validation/BDPrototypeHazardSceneInstaller.cs",
                "Assets/_Project/Scripts/Runtime/BDHealth.cs",
                "Assets/_Project/Scripts/Runtime/BDPlayerController.cs",
                "Assets/_Project/Scripts/Runtime/BDHorseController.cs"
            };

            foreach (string relative in relativePaths)
            {
                if (File.Exists(Path.Combine(root, relative)))
                    continue;

                Add(
                    result,
                    BDOneClickQASeverity.Blocker,
                    "HAZARD_RECOVERY_SOURCE_MISSING",
                    relative,
                    string.Empty,
                    "Required hazard recovery source is missing."
                );
            }

            string recoveryPath = Path.Combine(
                root,
                "Assets/_Project/Scripts/Runtime/Hazards/" +
                "BDPlayerHazardRecovery.cs"
            );
            string horseSafetyPath = Path.Combine(
                root,
                "Assets/_Project/Scripts/Runtime/Hazards/" +
                "BDHorseHazardSafety.cs"
            );
            string horsePath = Path.Combine(
                root,
                "Assets/_Project/Scripts/Runtime/BDHorseController.cs"
            );

            if (File.Exists(recoveryPath))
            {
                ValidateRequiredSourceTokens(
                    result,
                    MakeRelative(recoveryPath),
                    File.ReadAllText(recoveryPath),
                    new[]
                    {
                        "HasIntentionalGapEntry",
                        "forceActivation",
                        "ApplyUnavoidableDamage",
                        "BDHazardType.HoleOrChasm"
                    },
                    "PLAYER_HAZARD_RULE_MISSING"
                );
            }

            if (File.Exists(horseSafetyPath))
            {
                ValidateRequiredSourceTokens(
                    result,
                    MakeRelative(horseSafetyPath),
                    File.ReadAllText(horseSafetyPath),
                    new[]
                    {
                        "FilterMovement",
                        "TryHandleHazard",
                        "RecoverHorseWithoutDamage",
                        "ForceDismountAfterHazardRecovery",
                        "IsHorsePositionSafe"
                    },
                    "HORSE_HAZARD_RULE_MISSING"
                );
            }

            if (File.Exists(horsePath))
            {
                ValidateRequiredSourceTokens(
                    result,
                    MakeRelative(horsePath),
                    File.ReadAllText(horsePath),
                    new[]
                    {
                        "RequireComponent(typeof(BDHorseHazardSafety))",
                        "ForceDismountAfterHazardRecovery",
                        "MoveHorse",
                        "hazardSafety.FilterMovement"
                    },
                    "HORSE_HAZARD_CONTROLLER_RULE_MISSING"
                );
            }
        }


        private static void ValidateRequiredSourceTokens(
            BDOneClickQAResult result,
            string relativePath,
            string source,
            IEnumerable<string> requiredTokens,
            string findingCode)
        {
            foreach (string token in requiredTokens)
            {
                if (source.Contains(token))
                    continue;

                Add(
                    result,
                    BDOneClickQASeverity.Blocker,
                    findingCode,
                    relativePath,
                    string.Empty,
                    $"Required regression anchor is missing: {token}."
                );
            }
        }


        private static void ScanArchitectureContracts(
            BDOneClickQAResult result)
        {
            string[] sceneGuids = AssetDatabase.FindAssets(
                "t:Scene",
                new[] { "Assets/_Project/Scenes" }
            );

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

                    List<MonoBehaviour> behaviours =
                        new List<MonoBehaviour>();

                    foreach (GameObject root in scene.GetRootGameObjects())
                    {
                        behaviours.AddRange(
                            root.GetComponentsInChildren<MonoBehaviour>(true)
                                .Where(item => item != null)
                        );
                    }

                    ValidateRequiredSceneComponents(
                        behaviours,
                        path,
                        result
                    );

                    ValidateConflictingSceneFamilies(
                        behaviours,
                        path,
                        result
                    );

                    ValidateInstallerMultiplicity(
                        behaviours,
                        path,
                        result
                    );
                }
            }
            catch (Exception exception)
            {
                Add(
                    result,
                    BDOneClickQASeverity.Blocker,
                    "ARCHITECTURE_CONTRACT_SCAN_FAILED",
                    string.Empty,
                    string.Empty,
                    exception.Message
                );
            }
            finally
            {
                EditorSceneManager.RestoreSceneManagerSetup(
                    originalSetup
                );
            }
        }

        private static void ValidateRequiredSceneComponents(
            List<MonoBehaviour> behaviours,
            string assetPath,
            BDOneClickQAResult result)
        {
            MonoBehaviour playerMarker =
                behaviours.FirstOrDefault(item =>
                    item.GetType().Name == "BDPlayerMarker"
                );

            if (playerMarker == null)
            {
                Add(
                    result,
                    BDOneClickQASeverity.Blocker,
                    "PLAYER_MISSING",
                    assetPath,
                    string.Empty,
                    "No BDPlayerMarker was found."
                );
            }
            else
            {
                ValidateRequiredTypesOnGameObject(
                    playerMarker.gameObject,
                    assetPath,
                    result,
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
                    item.GetType().Name == "BDHorseController"
                );

            if (horseController == null)
            {
                Add(
                    result,
                    BDOneClickQASeverity.Warning,
                    "HORSE_MISSING",
                    assetPath,
                    string.Empty,
                    "No BDHorseController was found."
                );
            }
            else
            {
                ValidateRequiredTypesOnGameObject(
                    horseController.gameObject,
                    assetPath,
                    result,
                    "HORSE_REQUIRED_COMPONENT_MISSING",
                    new[] { "BDHorseHealth" }
                );
            }
        }

        private static void ValidateRequiredTypesOnGameObject(
            GameObject target,
            string assetPath,
            BDOneClickQAResult result,
            string findingCode,
            IEnumerable<string> requiredTypes)
        {
            HashSet<string> attached =
                target.GetComponents<MonoBehaviour>()
                    .Where(item => item != null)
                    .Select(item => item.GetType().Name)
                    .ToHashSet();

            foreach (string required in requiredTypes)
            {
                if (attached.Contains(required))
                    continue;

                Add(
                    result,
                    BDOneClickQASeverity.Blocker,
                    findingCode,
                    assetPath,
                    BuildObjectPath(target.transform),
                    $"Required component is missing: {required}."
                );
            }
        }

        private static void ValidateConflictingSceneFamilies(
            List<MonoBehaviour> behaviours,
            string assetPath,
            BDOneClickQAResult result)
        {
            string[][] families =
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

            foreach (string[] family in families)
            {
                List<MonoBehaviour> matches =
                    behaviours
                        .Where(item =>
                            item.isActiveAndEnabled &&
                            family.Contains(item.GetType().Name))
                        .ToList();

                int distinctTypes =
                    matches.Select(item => item.GetType().Name)
                        .Distinct()
                        .Count();

                if (distinctTypes <= 1)
                    continue;

                Add(
                    result,
                    BDOneClickQASeverity.Blocker,
                    "CONFLICTING_SYSTEM_FAMILY",
                    assetPath,
                    string.Join(
                        " | ",
                        matches.Select(item =>
                            $"{item.GetType().Name}@" +
                            BuildObjectPath(item.transform))
                    ),
                    "More than one implementation of the same " +
                    "gameplay responsibility is active."
                );
            }
        }

        // BD PER-ENTITY BOOTSTRAP CLASSIFICATION FIX V1
        private static void ValidateInstallerMultiplicity(
            List<MonoBehaviour> behaviours,
            string assetPath,
            BDOneClickQAResult result)
        {
            // BDEnemyBootstrap is an intentional per-enemy setup component.
            // It may legitimately appear once on every enemy instance. The
            // generic Bootstrap keyword must not classify those independent
            // enemy components as duplicate scene-level installers.
            HashSet<string> allowedPerEntityBootstrapTypes =
                new HashSet<string>(StringComparer.Ordinal)
                {
                    "BDEnemyBootstrap"
                };

            string[] globalLifecycleKeywords =
            {
                "Installer",
                "RuntimeRepair",
                "AutoSetup"
            };

            IEnumerable<IGrouping<string, MonoBehaviour>> groups =
                behaviours
                    .Where(item =>
                    {
                        if (!item.isActiveAndEnabled)
                            return false;

                        string typeName = item.GetType().Name;

                        if (allowedPerEntityBootstrapTypes.Contains(typeName))
                            return false;

                        bool explicitGlobalLifecycleType =
                            globalLifecycleKeywords.Any(keyword =>
                                typeName.IndexOf(
                                    keyword,
                                    StringComparison.OrdinalIgnoreCase
                                ) >= 0);

                        bool otherBootstrapType =
                            typeName.IndexOf(
                                "Bootstrap",
                                StringComparison.OrdinalIgnoreCase
                            ) >= 0;

                        return explicitGlobalLifecycleType ||
                               otherBootstrapType;
                    })
                    .GroupBy(item => item.GetType().Name)
                    .Where(group => group.Count() > 1);

            foreach (IGrouping<string, MonoBehaviour> group in groups)
            {
                Add(
                    result,
                    BDOneClickQASeverity.Warning,
                    "MULTIPLE_RUNTIME_INSTALLERS",
                    assetPath,
                    string.Join(
                        " | ",
                        group.Select(item =>
                            BuildObjectPath(item.transform))
                    ),
                    $"{group.Count()} active instances of " +
                    $"{group.Key} were found. Verify that every " +
                    "scene-level installer is necessary and idempotent."
                );
            }
        }

        private static void ScanRepositoryHygiene(
            BDOneClickQAResult result)
        {
            string root = ResolveProjectRoot();
            string gitIgnorePath = Path.Combine(root, ".gitignore");

            if (!File.Exists(gitIgnorePath))
            {
                Add(
                    result,
                    BDOneClickQASeverity.Blocker,
                    "GITIGNORE_MISSING",
                    ".gitignore",
                    string.Empty,
                    "The repository must contain a project .gitignore."
                );
            }
            else
            {
                string ignore = File.ReadAllText(gitIgnorePath);
                string[] requiredIgnoreRules =
                {
                    "/[Ll]ibrary/",
                    "/[Tt]emp/",
                    "/[Oo]bj/",
                    "/[Ll]ogs/",
                    "*.zip",
                    "/APPLY_*.command",
                    "/PROJECT_STATUS_CURRENT_V*.md",
                    "/ONE_CLICK_QA*.txt",
                    "/tools/apply_*.py",
                    "/.package_tools/"
                };

                foreach (string rule in requiredIgnoreRules)
                {
                    if (ignore.Contains(rule))
                        continue;

                    Add(
                        result,
                        BDOneClickQASeverity.Blocker,
                        "GITIGNORE_RULE_MISSING",
                        ".gitignore",
                        string.Empty,
                        $"Required ignore rule is missing: {rule}"
                    );
                }
            }

            if (!TryReadTrackedFiles(
                    root,
                    out List<string> tracked,
                    out string gitError))
            {
                Add(
                    result,
                    BDOneClickQASeverity.Warning,
                    "GIT_TRACKED_SCAN_UNAVAILABLE",
                    ".git",
                    string.Empty,
                    gitError
                );
                return;
            }

            foreach (string rawPath in tracked)
            {
                string path = rawPath.Replace('\\', '/');
                string fileName = Path.GetFileName(path);
                bool rootFile = path.IndexOf('/') < 0;

                bool versionedStatus =
                    rootFile &&
                    fileName.StartsWith(
                        "PROJECT_STATUS_CURRENT_V",
                        StringComparison.OrdinalIgnoreCase
                    ) &&
                    fileName.EndsWith(
                        ".md",
                        StringComparison.OrdinalIgnoreCase
                    );

                bool rootPatchCommand =
                    rootFile &&
                    fileName.StartsWith(
                        "APPLY_",
                        StringComparison.OrdinalIgnoreCase
                    ) &&
                    fileName.EndsWith(
                        ".command",
                        StringComparison.OrdinalIgnoreCase
                    );

                bool oldRootHelper =
                    rootFile &&
                    (
                        fileName.Equals(
                            "RUN_STABILITY_SOURCE_SCAN.command",
                            StringComparison.OrdinalIgnoreCase
                        ) ||
                        fileName.Equals(
                            "README_HE.txt",
                            StringComparison.OrdinalIgnoreCase
                        )
                    );

                bool oneShotTool =
                    path.StartsWith(
                        "tools/apply_",
                        StringComparison.OrdinalIgnoreCase
                    ) &&
                    path.EndsWith(
                        ".py",
                        StringComparison.OrdinalIgnoreCase
                    );

                bool generatedArtifact =
                    IsGeneratedTrackedPath(path);

                if (!versionedStatus &&
                    !rootPatchCommand &&
                    !oldRootHelper &&
                    !oneShotTool &&
                    !generatedArtifact)
                {
                    continue;
                }

                Add(
                    result,
                    BDOneClickQASeverity.Blocker,
                    "TRACKED_REPOSITORY_CLUTTER",
                    path,
                    string.Empty,
                    "Generated, one-shot, duplicated-status, or local-only " +
                    "artifact is tracked. Remove it from Git."
                );
            }
        }

        private static bool IsGeneratedTrackedPath(string path)
        {
            string normalized = path.Replace('\\', '/');
            string lower = normalized.ToLowerInvariant();

            string[] generatedSegments =
            {
                "library/",
                "temp/",
                "obj/",
                "logs/",
                "usersettings/",
                "memorycaptures/",
                "recordings/",
                "__pycache__/",
                ".pytest_cache/",
                ".mypy_cache/",
                ".ruff_cache/",
                "node_modules/"
            };

            if (generatedSegments.Any(segment =>
                    lower.StartsWith(segment) ||
                    lower.Contains("/" + segment)))
            {
                return true;
            }

            string[] generatedSuffixes =
            {
                ".zip",
                ".rar",
                ".7z",
                ".tar",
                ".tar.gz",
                ".unitypackage",
                ".log",
                ".bak",
                ".backup",
                ".orig",
                ".tmp",
                ".temp",
                ".pyc",
                ".pyo",
                ".ds_store"
            };

            if (generatedSuffixes.Any(suffix =>
                    lower.EndsWith(suffix)))
            {
                return true;
            }

            string fileName = Path.GetFileName(normalized);

            return fileName.StartsWith(
                       "ONE_CLICK_QA",
                       StringComparison.OrdinalIgnoreCase
                   ) ||
                   fileName.StartsWith(
                       "PACKAGE_MANIFEST",
                       StringComparison.OrdinalIgnoreCase
                   ) ||
                   fileName.StartsWith(
                       "README_C01_",
                       StringComparison.OrdinalIgnoreCase
                   );
        }

        private static bool TryReadTrackedFiles(
            string root,
            out List<string> tracked,
            out string error)
        {
            tracked = new List<string>();
            error = string.Empty;

            try
            {
                System.Diagnostics.ProcessStartInfo startInfo =
                    new System.Diagnostics.ProcessStartInfo
                    {
                        FileName = "git",
                        Arguments = "ls-files -z",
                        WorkingDirectory = root,
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        CreateNoWindow = true
                    };

                using (System.Diagnostics.Process process =
                       new System.Diagnostics.Process())
                {
                    process.StartInfo = startInfo;

                    if (!process.Start())
                    {
                        error = "Could not start git ls-files.";
                        return false;
                    }

                    string output = process.StandardOutput.ReadToEnd();
                    string standardError =
                        process.StandardError.ReadToEnd();

                    process.WaitForExit();

                    if (process.ExitCode != 0)
                    {
                        error =
                            "git ls-files failed: " +
                            standardError.Trim();
                        return false;
                    }

                    tracked.AddRange(
                        output.Split(
                            new[] { '\0' },
                            StringSplitOptions.RemoveEmptyEntries
                        )
                    );
                }

                return true;
            }
            catch (Exception exception)
            {
                error =
                    "Could not inspect tracked repository files: " +
                    exception.Message;
                return false;
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
