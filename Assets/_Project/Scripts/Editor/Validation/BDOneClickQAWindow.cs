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
                "W/S/A/D move in straight lines. Tiny accidental mouse movement does not rotate the player. Jumping onto an attacking enemy never teleports the player to an older safe point."),
            new ManualCheck(
                "minimap",
                "Minimap",
                "The minimap keeps four 90-degree targets, transitions smoothly through the shortest angle, settles cleanly, and never draws outside its frame."),
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
                "The horse starts at full health, stays calm through the startup window, and ignores remote encounters while preserving healing, buck, mount, real local-threat flee, and two-step hazard retreat behavior. After death -> New Game, the child starts on foot and the horse stands fully above the floor beside the player, neither sunken nor floating. At zero health it remains fainted while nearby; after the player stays beyond 14m for 1.25 seconds it follows very slowly through hazard-safe movement, stops by 8m, never heals or mounts, and exits after healing. Within 2.25m on foot, tap Pet for player-pets-horse and hold 0.65 seconds for horse-nuzzles-player; the actions are exclusive and cancel safely."),
            new ManualCheck(
                "square_jumper",
                "Square Jumper",
                "Jump, hard landing, bullet hell, visible double swords, summons, enraged phase, death cleanup, and reward chest all work."),
            new ManualCheck(
                "hazards",
                "Ground, holes, lava, horse, and minimap",
                "The minimap remains clipped while its cardinal transition animates. Normal walking never leaves supported ground. Quicksand visibly slows movement more as the player sinks, jumping does not prematurely respawn, and leaving restores normal speed. A hole applies exactly 15 damage and respawns; real lava contact applies exactly 10 damage and returns the player safely. The horse performs a two-step retreat before hazards, and mounted emergency recovery returns the player on foot without horse damage."),
            new ManualCheck(
                "run_presentation",
                "Run entrance, authored exit, pause, and menu",
                "The mounted entrance uses a farther/higher room camera, locks every input including mouse aim, then the horse turns, fully stops, and only then returns camera/control. Pause -> Abandon requires confirmation. After confirmed abandon, the next mounted entrance keeps the current player visibly attached to the horse for the full sequence. Death restarts stay on foot and the authored exit remains authoritative."),
                        new ManualCheck(
                "modern_handheld",
                "Modern 3D handheld Main and Pause",
                "The real upright device sits slightly lower without clipping; the large context image aligns with the title composition; every Main selection has relevant text-only context; no physical button receives a blue hover frame; D-Pad, SELECT, EXIT and X/Y/A/B share one press family; all text stays inside its card; Settings has a visible icon; and Escape opens a dedicated internal handheld Pause menu."),
            new ManualCheck(
                "first_launch_tutorial",
                "First-launch tutorial",
                "After the room reveal, the top-left mother bubble appears with professional fade/scale, plays the female nonverbal cue, reverses out completely, and only then may the child walk. Verify the wall-jump route can be failed by falling naturally, Jump + Attack is explicit, mounted shots damage one target only at projectile impact, the existing local Mini-Boss telegraph/slam presentation remains intact with an occasional phase-two shot, backdrop blocks stay behind gameplay, and the larger animated tutorial type remains readable. Complete all lessons with keyboard, controller and physical controls; EXIT, persistence and no-auto-replay behavior remain unchanged."),
            new ManualCheck(
                "room_boundaries",
                "Tall room walls and camera stop",
                "Closed walls remain complete visibility boundaries. Standing beside any wall and rotating through diagonal angles, on foot or mounted, never reveals the adjacent room."),
            new ManualCheck(
                "bbh_circle",
                "BBH completed-circle intro",
                "The intro begins black and reveals B, B, H strictly one at a time. The first B is careful, the second B arrives confidently and nudges it, and the H lands with a restrained shared weight reaction. Only after all three settle, a filled responsive circle gathers from a point, grows behind the letters to a clearly larger final badge without cropping, holds for exactly 0.50 seconds, then fades without replaying on a same-session New Game."),
            new ManualCheck(
                "console",
                "Console",
                "No red project errors. Charged shots never create duplicate TrailRenderers or throw null exceptions, and exactly one AudioListener stays active.")
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
                    "Press Play once, verify every listed item in order, and check each box. Nothing else needs to be run.",
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
            BDMinimapRigidRotationQA.Scan(result);
            ScanSingleCameraOwnerContracts(result);
            ScanHazardRecoveryContracts(result);
            ScanGroundHazardAndMinimapContracts(result);
            ScanCameraMinimapSceneRepairContracts(result);
            ScanRespawnLoopSafetyContracts(result);
            ScanLongFallAndHorseJumpSafetyContracts(result);
            ScanMountedHorseHazardRecoveryContracts(result);
            ScanHorseCleanStartContracts(result);
            ScanSceneYamlIntegrityContracts(result);
            ScanBBHBootIntroContracts(result);
            BDBBHCinematicIntroQA.Scan(result);
            BDFirstLaunchTutorialQA.Scan(result);
            BDTutorialOpeningPolishV1011QA.Scan(result);
            BDTutorialFinalInputCombatPlayerV1011301QA.Scan(result);
            BDTutorialLegacyPlayerContractRealignmentV1011302QA.Scan(result);
            BDTutorialPlayerVisibilityRuntimeV1011303QA.Scan(result);
            BDTutorialPlayerCanonicalAssetNameV1011304QA.Scan(result);
            BDTutorialInputMechanicsMountedImpactV1011305QA.Scan(result);
            BDTutorialLessonScreensInputParryV1011306QA.Scan(result);
            BDTutorialRuntimeIntegrityV1011319QA.Scan(result);
            BDScreenRenderSchedulingV1011321QA.Scan(result);
            BDTutorialOpeningScreenSequenceV1011322QA.Scan(result);
            BDTutorialSecondScreenLightAttackV1011323QA.Scan(result);
            BDTutorialContinuousRoomSequenceV1011325QA.Scan(result);
            BDTutorialCenteredParryHorseMetalV1011326QA.Scan(result);
            BDTutorialHorseFreeOpeningPetSuppressionV1011327QA.Scan(result);
            BDTutorialFlowCoherenceV1011328QA.Scan(result);
            BDTutorialHorseCombatContinueV1011329QA.Scan(result);
            BDTutorialBubbleDepthV1011330QA.Scan(result);
            BDChildApproachCinematicQA.Scan(result);
            ScanDreamyMainMenuContracts(result);
            ScanNaturalMovementAwarenessFacingContracts(result);
            BDHorseExhaustedFollowPetQA.Scan(result);
            BDHorseHudMinimapQA.Scan(result);
            BDNewRunFeedbackResetQA.Scan(result);
            BDDocumentationGovernanceQA.Scan(result);
            BDVisualEntryMinimapMountedCombatQA.Scan(result);
            BDRunPresentationPauseQA.Scan(result);
            BDEarlyRunRegressionRepairQA.Scan(result);
            BDMountedRunIntroQA.Scan(result);
            BDV20ActiveRegressionQA.Scan(result);
            BDV23CameraGroundingQA.Scan(result);
            BDV23R8CombatUxQA.Scan(result);
            BDV23R9HorseArtDirectionQA.Scan(result);
            BDV23R10RuntimePolishQA.Scan(result);
            BDV23R11BombAirborneAudioQA.Scan(result);
            BDV23R12RegressionRepairQA.Scan(result);
            BDV23R13AudioQuicksandOutlineQA.Scan(result);
            BDV23R14DamageNumbersLabelVisibilityQA.Scan(result);
            BDV23R15MeleeDamageCriticalQA.Scan(result);
            BDV23R17HazardHorseWallJumpQA.Scan(result);
            BDV23R18AHorseHazardAnimationQA.Scan(result);
            BDV23R18BMountedHoleAnimationTokenQA.Scan(result);
            BDV23R19TraversalQuicksandAirborneQA.Scan(result);
            BDV23R19BHookGuardianQA.Scan(result);
            BDV23R19DFocusedRegressionQA.Scan(result);
            BDV23R19EDeathGuardianIntroQA.Scan(result);
            BDV23R19GFocusedRegressionQA.Scan(result);
            BDV23R19HCharacterMountedHookQA.Scan(result);
            BDV23R19ICombatantForcedMovementCompatibilityQA.Scan(result);
            BDV23R19JQaSemanticRealignmentQA.Scan(result);
            BDV23R19KAirborneDialogueQA.Scan(result);
            BDV23R19MAirborneAxisSmallEnemyDeathQA.Scan(result);
            BDV23R19OCriticalIntroOutlineWallJumpQA.Scan(result);
            BDV23R19PQaSemanticCaterpillarSpecQA.Scan(result);
            BDV23R19QProfessionalMemoryHandheldUiQA.Scan(result);
            BDModernHandheld3DQA.Scan(result);
            BDV23R19RMenuContractAndWorkLedgerQA.Scan(result);
            BDV23R19SContinuitySemanticQA.Scan(result);
            BDV23R19TPhaseAgnosticStatusQA.Scan(result);
            BDHorseCleanRunStartQA.Scan(result);
            ScanSpinningAoeAttackContracts(result);
            ScanGameplayShadowPolicyContracts(result);
            ScanMainMenuSettingsContracts(result);
            ScanGuardianSameRoomSpawnContracts(result);
            ScanC07PlayableFrameworkEncounter(result);
            ScanObsoleteHazardFieldContracts(result);
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
        private static void ScanSingleCameraOwnerContracts(
            BDOneClickQAResult result)
        {
            string root = ResolveProjectRoot();
            string legacyRelative =
                "Assets/_Project/Scripts/Runtime/Camera/" +
                "BDCameraForwardViewBias.cs";

            if (File.Exists(Path.Combine(root, legacyRelative)))
            {
                Add(
                    result,
                    BDOneClickQASeverity.Blocker,
                    "SECONDARY_CAMERA_TRANSFORM_OWNER_PRESENT",
                    legacyRelative,
                    string.Empty,
                    "BDCameraFollow must remain the sole normal-gameplay camera transform owner."
                );
            }

            const string cameraRelative =
                "Assets/_Project/Scripts/Runtime/BDCameraFollow.cs";
            string cameraPath = Path.Combine(root, cameraRelative);
            if (!File.Exists(cameraPath))
            {
                Add(
                    result,
                    BDOneClickQASeverity.Blocker,
                    "SINGLE_CAMERA_OWNER_SOURCE_MISSING",
                    cameraRelative,
                    string.Empty,
                    "The sole gameplay camera owner source is missing."
                );
                return;
            }

            ValidateRequiredSourceTokens(
                result,
                cameraRelative,
                File.ReadAllText(cameraPath),
                new[]
                {
                    "BD SINGLE CAMERA TRANSFORM OWNER V23",
                    "BD STABLE SINGLE-STAGE CAMERA YAW V23",
                    "BD PLANAR COMBAT SHAKE V23",
                    "ResolvePlanarCameraShake",
                    "ResolveRoomBoundaryConstrainedPosition"
                },
                "SINGLE_CAMERA_OWNER_CONTRACT_MISSING"
            );
        }


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
                    "BD SINGLE CAMERA TRANSFORM OWNER V23",
                    "BD STABLE SINGLE-STAGE CAMERA YAW V23",
                    "cameraYawDegreesPerSecond",
                    "ResolveCameraIntentDirection",
                    "Vector3.RotateTowards",
                    "ResolvePlanarCameraShake",
                    "transform.SetPositionAndRotation",
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
                    "targetMapRotationDegrees",
                    "cardinalRotationSmoothTime",
                    "Mathf.SmoothDampAngle",
                    "Mathf.DeltaAngle",
                    "hasMapRotationTarget",
                    "RotateMapPoint",
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
            int quicksand = volumes.Count(
                item =>
                    item != null &&
                    item.HazardType ==
                        BDHazardType.Quicksand
            );

            if (holes != 1 || lava != 1 || quicksand != 1)
            {
                Add(
                    result,
                    BDOneClickQASeverity.Blocker,
                    "HAZARD_TEST_VOLUME_SET_INVALID",
                    PrototypeScenePath,
                    BDPrototypeHazardSceneInstaller.RootName,
                    $"Expected one hole/chasm, one lava, and one quicksand volume; found hole={holes}, lava={lava}, quicksand={quicksand}."
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


        private static void ScanGroundHazardAndMinimapContracts(
            BDOneClickQAResult result)
        {
            string root = ResolveProjectRoot();

            string playerRelative =
                "Assets/_Project/Scripts/Runtime/BDPlayerController.cs";
            string volumeRelative =
                "Assets/_Project/Scripts/Runtime/Hazards/" +
                "BDHazardVolume.cs";
            string recoveryRelative =
                "Assets/_Project/Scripts/Runtime/Hazards/" +
                "BDPlayerHazardRecovery.cs";
            string minimapRelative =
                "Assets/_Project/Scripts/Runtime/BDMazeMinimap.cs";

            string[] requiredPaths =
            {
                playerRelative,
                volumeRelative,
                recoveryRelative,
                minimapRelative
            };

            foreach (string relative in requiredPaths)
            {
                if (File.Exists(Path.Combine(root, relative)))
                    continue;

                Add(
                    result,
                    BDOneClickQASeverity.Blocker,
                    "GROUND_HAZARD_MINIMAP_SOURCE_MISSING",
                    relative,
                    string.Empty,
                    "Required ground, hazard, or minimap source is missing."
                );
            }

            ValidateRequiredSourceTokens(
                result,
                playerRelative,
                File.ReadAllText(
                    Path.Combine(root, playerRelative)),
                new[]
                {
                    "forcedGapEntryUntil",
                    "NotifyForcedGapEntry",
                    "BDHazardVolume.FilterPlayerMotion(",
                    "forcedGapEntryUntil = -999f"
                },
                "PLAYER_GROUND_EXIT_CONTRACT_MISSING"
            );

            ValidateRequiredSourceTokens(
                result,
                volumeRelative,
                File.ReadAllText(
                    Path.Combine(root, volumeRelative)),
                new[]
                {
                    "HasGroundSupport",
                    "ResolveGroundedAxes",
                    "IsActorTouchingSurface",
                    "IsInsideHoleHorizontal"
                },
                "GROUND_SUPPORT_CONTRACT_MISSING"
            );

            ValidateRequiredSourceTokens(
                result,
                recoveryRelative,
                File.ReadAllText(
                    Path.Combine(root, recoveryRelative)),
                new[]
                {
                    "TickHoleFall",
                    "holeFallDuration",
                    "TickLavaBounce",
                    "lavaBounceHeight",
                    "CheckGroundExit",
                    "DetectForcedDisplacement"
                },
                "HAZARD_TRANSITION_CONTRACT_MISSING"
            );

            ValidateRequiredSourceTokens(
                result,
                minimapRelative,
                File.ReadAllText(
                    Path.Combine(root, minimapRelative)),
                new[]
                {
                    "ResolveScreenPanelRect",
                    "GUI.BeginGroup(localMapRect)",
                    "GUI.EndGroup()",
                    "localMapRect"
                },
                "MINIMAP_CLIPPING_CONTRACT_MISSING"
            );
        }
        private static void ScanRespawnLoopSafetyContracts(
            BDOneClickQAResult result)
        {
            string root = ResolveProjectRoot();

            string volumeRelative =
                "Assets/_Project/Scripts/Runtime/Hazards/" +
                "BDHazardVolume.cs";
            string recoveryRelative =
                "Assets/_Project/Scripts/Runtime/Hazards/" +
                "BDPlayerHazardRecovery.cs";

            ValidateRequiredSourceTokens(
                result,
                volumeRelative,
                File.ReadAllText(
                    Path.Combine(root, volumeRelative)),
                new[]
                {
                    "IsRecoveryPointUnsafe",
                    "ContainsHorizontalPoint"
                },
                "RESPAWN_HORIZONTAL_SAFETY_MISSING"
            );

            ValidateRequiredSourceTokens(
                result,
                recoveryRelative,
                File.ReadAllText(
                    Path.Combine(root, recoveryRelative)),
                new[]
                {
                    "previousSafePosition",
                    "safePointUpdatesBlockedUntil",
                    "TryResolveLoopBreakerPoint",
                    "rapidRecoveryLoopWindow",
                    "postRecoverySafePointLock",
                    "IsRecoveryPointUnsafe"
                },
                "RESPAWN_LOOP_BREAKER_MISSING"
            );
        }

        private static void ScanCameraMinimapSceneRepairContracts(
            BDOneClickQAResult result)
        {
            string root = ResolveProjectRoot();

            string minimapRelative =
                "Assets/_Project/Scripts/Runtime/" +
                "BDMazeMinimap.cs";

            string sceneInstallerRelative =
                "Assets/_Project/Scripts/Editor/Validation/" +
                "BDPrototypeHazardSceneInstaller.cs";

            ValidateRequiredSourceTokens(
                result,
                minimapRelative,
                File.ReadAllText(
                    Path.Combine(root, minimapRelative)),
                new[]
                {
                    "float mapSize = Mathf.Min",
                    "GUI.BeginGroup(localMapRect)",
                    "DrawRotatedRoomsClipped",
                    "DrawRotatedMarkerClipped",
                    "localPanelRect.height - 76f"
                },
                "MINIMAP_INNER_SQUARE_CONTRACT_MISSING"
            );

            ValidateRequiredSourceTokens(
                result,
                sceneInstallerRelative,
                File.ReadAllText(
                    Path.Combine(root, sceneInstallerRelative)),
                new[]
                {
                    "ConfigureMouseSensitivity(player.gameObject)",
                    "RemoveStartRoomEnemies(player)",
                    "new Vector3(0f, 0.55f, 0f)",
                    "new Vector3(0f, 0.04f, 0f)"
                },
                "SCENE_REPAIR_CONTRACT_MISSING"
            );
        }

        private static void ScanLongFallAndHorseJumpSafetyContracts(
            BDOneClickQAResult result)
        {
            string root = ResolveProjectRoot();

            string recoveryRelative =
                "Assets/_Project/Scripts/Runtime/Hazards/" +
                "BDPlayerHazardRecovery.cs";

            string horseSafetyRelative =
                "Assets/_Project/Scripts/Runtime/Hazards/" +
                "BDHorseHazardSafety.cs";

            string horseControllerRelative =
                "Assets/_Project/Scripts/Runtime/" +
                "BDHorseController.cs";

            ValidateRequiredSourceTokens(
                result,
                recoveryRelative,
                File.ReadAllText(
                    Path.Combine(root, recoveryRelative)),
                new[]
                {
                    "holeFallDuration = 2.05f",
                    "holeFallSpeed = 4.60f",
                    "TickHoleFall"
                },
                "LONG_HOLE_FALL_CONTRACT_MISSING"
            );

            ValidateRequiredSourceTokens(
                result,
                horseSafetyRelative,
                File.ReadAllText(
                    Path.Combine(root, horseSafetyRelative)),
                new[]
                {
                    "jumpPathClearance",
                    "jumpTrajectorySamples",
                    "CanStartJump",
                    "IsRecoveryPointUnsafe",
                    "TryResolveGround"
                },
                "HORSE_JUMP_HAZARD_SAFETY_MISSING"
            );

            ValidateRequiredSourceTokens(
                result,
                horseControllerRelative,
                File.ReadAllText(
                    Path.Combine(root, horseControllerRelative)),
                new[]
                {
                    "CanStartHorseJump",
                    "hazardSafety.CanStartJump",
                    "horse jump blocked by hole/lava"
                },
                "HORSE_JUMP_GUARD_WIRING_MISSING"
            );
        }

        private static void ScanObsoleteHazardFieldContracts(
            BDOneClickQAResult result)
        {
            const string relativePath =
                "Assets/_Project/Scripts/Runtime/Hazards/" +
                "BDPlayerHazardRecovery.cs";

            string absolutePath = Path.Combine(
                ResolveProjectRoot(),
                relativePath
            );

            if (!File.Exists(absolutePath))
            {
                Add(
                    result,
                    BDOneClickQASeverity.Blocker,
                    "PLAYER_HAZARD_RECOVERY_SOURCE_MISSING",
                    relativePath,
                    string.Empty,
                    "BDPlayerHazardRecovery.cs is required."
                );
                return;
            }

            string source = File.ReadAllText(absolutePath);

            if (source.IndexOf(
                    "emergencyHoleFallDepth",
                    StringComparison.Ordinal) < 0)
            {
                return;
            }

            Add(
                result,
                BDOneClickQASeverity.Warning,
                "OBSOLETE_EMERGENCY_HOLE_FIELD_PRESENT",
                relativePath,
                "emergencyHoleFallDepth",
                "The obsolete emergencyHoleFallDepth field causes CS0414 and must remain removed."
            );
        }

        private static void ScanMountedHorseHazardRecoveryContracts(
            BDOneClickQAResult result)
        {
            string root = ResolveProjectRoot();

            string volumeRelative =
                "Assets/_Project/Scripts/Runtime/Hazards/" +
                "BDHazardVolume.cs";

            string playerRecoveryRelative =
                "Assets/_Project/Scripts/Runtime/Hazards/" +
                "BDPlayerHazardRecovery.cs";

            string horseSafetyRelative =
                "Assets/_Project/Scripts/Runtime/Hazards/" +
                "BDHorseHazardSafety.cs";

            string horseControllerRelative =
                "Assets/_Project/Scripts/Runtime/" +
                "BDHorseController.cs";

            ValidateRequiredSourceTokens(
                result,
                volumeRelative,
                File.ReadAllText(
                    Path.Combine(root, volumeRelative)),
                new[]
                {
                    "TryFindUnsafeVolume",
                    "ContainsHorizontalPoint"
                },
                "HORSE_CONTINUOUS_HAZARD_QUERY_MISSING"
            );

            ValidateRequiredSourceTokens(
                result,
                playerRecoveryRelative,
                File.ReadAllText(
                    Path.Combine(root, playerRecoveryRelative)),
                new[]
                {
                    "PrepareMountedHazardRecovery",
                    "TryResolveMountedRecoveryAnchor",
                    "mountedRecoveryAnchor",
                    "mountedRecoverySeparation",
                    "TryHandleHazard"
                },
                "MOUNTED_PLAYER_RECOVERY_CONTRACT_MISSING"
            );

            ValidateRequiredSourceTokens(
                result,
                horseSafetyRelative,
                File.ReadAllText(
                    Path.Combine(root, horseSafetyRelative)),
                new[]
                {
                    "public bool IsRecovering",
                    "PollCurrentHazard",
                    "recoveryGraceUntil",
                    "safePointUpdatesBlockedUntil",
                    "previousSafePosition",
                    "TryResolveRecoveryPosition",
                    "PrepareMountedHazardRecovery",
                    "ForceDismountAfterHazardRecovery"
                },
                "HORSE_RECOVERY_LOOP_CONTRACT_MISSING"
            );

            ValidateRequiredSourceTokens(
                result,
                horseControllerRelative,
                File.ReadAllText(
                    Path.Combine(root, horseControllerRelative)),
                new[]
                {
                    "hazardSafety.IsRecovering",
                    "hazard recovery lock",
                    "ForceDismountAfterHazardRecovery"
                },
                "HORSE_AI_RECOVERY_LOCK_MISSING"
            );
        }
        private static void ScanBBHBootIntroContracts(
            BDOneClickQAResult result)
        {
            string root = ResolveProjectRoot();

            string introRelative =
                "Assets/_Project/Scripts/Runtime/UI/" +
                "BDBBHBootIntro.cs";

            string installerRelative =
                "Assets/_Project/Scripts/Editor/Validation/" +
                "BDMainMenuSettingsSceneInstaller.cs";

            string c07InstallerRelative =
                "Assets/_Project/Scripts/Editor/Validation/" +
                "BDC07PlayableBossEncounterInstaller.cs";

            string designRelative =
                "ProjectGuide/Features/UI/" +
                "BBH_BOOT_INTRO_V1.md";

            string[] requiredFiles =
            {
                introRelative,
                installerRelative,
                c07InstallerRelative,
                designRelative
            };

            for (int index = 0;
                 index < requiredFiles.Length;
                 index++)
            {
                string relative = requiredFiles[index];

                if (File.Exists(
                        Path.Combine(root, relative)))
                {
                    continue;
                }

                Add(
                    result,
                    BDOneClickQASeverity.Blocker,
                    "BBH_BOOT_INTRO_FILE_MISSING",
                    relative,
                    string.Empty,
                    "Required BBH boot-intro file is missing."
                );
            }

            if (File.Exists(
                    Path.Combine(root, introRelative)))
            {
                ValidateRequiredSourceTokens(
                    result,
                    introRelative,
                    File.ReadAllText(
                        Path.Combine(root, introRelative)),
                    new[]
                    {
                        "IntroText = \"BBH\"",
                        "VerticalScreenPositionFromTop = 0.45f",
                        "playedThisApplicationSession",
                        "PerLetterWindow",
                        "LetterStartTime(int index)",
                        "DrawCompactDepth",
                        "DrawGrowingFilledCircleBehindText",
                        "CircleFullHoldDuration = 0.50f",
                        "CircleHoldEndTime",
                        "Time.realtimeSinceStartup"
                    },
                    "BBH_BOOT_INTRO_RUNTIME_MISSING"
                );
            }

            if (File.Exists(
                    Path.Combine(root, installerRelative)))
            {
                ValidateRequiredSourceTokens(
                    result,
                    installerRelative,
                    File.ReadAllText(
                        Path.Combine(root, installerRelative)),
                    new[]
                    {
                        "RemoveMissingScriptsRecursively",
                        "GetMonoBehavioursWithMissingScriptCount",
                        "RemoveMonoBehavioursWithMissingScript",
                        "BDBBHBootIntro",
                        "EditorSceneManager.MarkSceneDirty"
                    },
                    "MAIN_MENU_MISSING_SCRIPT_REPAIR_MISSING"
                );
            }

            if (File.Exists(
                    Path.Combine(root, c07InstallerRelative)))
            {
                string c07Source =
                    File.ReadAllText(
                        Path.Combine(
                            root,
                            c07InstallerRelative));

                if (c07Source.Contains(
                        "EditorSceneManager.SaveScene("))
                {
                    Add(
                        result,
                        BDOneClickQASeverity.Blocker,
                        "C07_NESTED_SCENE_SAVE_FORBIDDEN",
                        c07InstallerRelative,
                        "TryInstallActiveScene",
                        "Nested C07 installation must mark the scene dirty instead of saving it directly."
                    );
                }
            }

            if (!BDMainMenuSettingsSceneInstaller
                    .ValidateActiveScene(out string error))
            {
                Add(
                    result,
                    BDOneClickQASeverity.Blocker,
                    "BBH_BOOT_INTRO_SCENE_INVALID",
                    PrototypeScenePath,
                    BDMainMenuSettingsSceneInstaller
                        .RootName,
                    error
                );
            }
        }
        private static void ScanSceneYamlIntegrityContracts(
            BDOneClickQAResult result)
        {
            string root = ResolveProjectRoot();
            string sceneRelative = PrototypeScenePath;
            string absolute =
                Path.Combine(root, sceneRelative);

            if (!File.Exists(absolute))
            {
                Add(
                    result,
                    BDOneClickQASeverity.Blocker,
                    "SCENE_YAML_FILE_MISSING",
                    sceneRelative,
                    string.Empty,
                    "The prototype scene file is missing."
                );

                return;
            }

            string[] lines =
                File.ReadAllLines(absolute);

            for (int lineIndex = 0;
                 lineIndex < lines.Length;
                 lineIndex++)
            {
                string line = lines[lineIndex];

                if (line.Contains("<<<<<<<") ||
                    line.Contains("=======") ||
                    line.Contains(">>>>>>>"))
                {
                    Add(
                        result,
                        BDOneClickQASeverity.Blocker,
                        "SCENE_YAML_CONFLICT_MARKER",
                        sceneRelative,
                        "line " + (lineIndex + 1),
                        "The Unity scene contains an unresolved merge marker."
                    );

                    continue;
                }

                int braceBalance = 0;
                bool inSingleQuote = false;
                bool inDoubleQuote = false;
                bool escaped = false;

                for (int characterIndex = 0;
                     characterIndex < line.Length;
                     characterIndex++)
                {
                    char character =
                        line[characterIndex];

                    if (inDoubleQuote)
                    {
                        if (escaped)
                        {
                            escaped = false;
                            continue;
                        }

                        if (character == '\\')
                        {
                            escaped = true;
                            continue;
                        }

                        if (character == '"')
                            inDoubleQuote = false;

                        continue;
                    }

                    if (inSingleQuote)
                    {
                        if (character == '\'')
                        {
                            if (characterIndex + 1 <
                                    line.Length &&
                                line[characterIndex + 1] ==
                                    '\'')
                            {
                                characterIndex++;
                                continue;
                            }

                            inSingleQuote = false;
                        }

                        continue;
                    }

                    if (character == '#')
                        break;

                    if (character == '"')
                    {
                        inDoubleQuote = true;
                        continue;
                    }

                    if (character == '\'')
                    {
                        inSingleQuote = true;
                        continue;
                    }

                    if (character == '{')
                        braceBalance++;
                    else if (character == '}')
                        braceBalance--;
                }

                if (braceBalance == 0)
                    continue;

                Add(
                    result,
                    BDOneClickQASeverity.Blocker,
                    "SCENE_YAML_FLOW_MAPPING_UNBALANCED",
                    sceneRelative,
                    "line " + (lineIndex + 1),
                    "Unity YAML inline mapping braces are unbalanced: " +
                    line.Trim()
                );
            }
        }



        private static void ScanHazardRecoveryContracts(
            BDOneClickQAResult result)
        {
            string root = ResolveProjectRoot();

            string volumeRelative =
                "Assets/_Project/Scripts/Runtime/Hazards/" +
                "BDHazardVolume.cs";
            string recoveryRelative =
                "Assets/_Project/Scripts/Runtime/Hazards/" +
                "BDPlayerHazardRecovery.cs";
            string horseSafetyRelative =
                "Assets/_Project/Scripts/Runtime/Hazards/" +
                "BDHorseHazardSafety.cs";
            string horseControllerRelative =
                "Assets/_Project/Scripts/Runtime/" +
                "BDHorseController.cs";
            string playerControllerRelative =
                "Assets/_Project/Scripts/Runtime/" +
                "BDPlayerController.cs";
            string sceneInstallerRelative =
                "Assets/_Project/Scripts/Editor/Validation/" +
                "BDPrototypeHazardSceneInstaller.cs";

            string[] relativePaths =
            {
                volumeRelative,
                recoveryRelative,
                horseSafetyRelative,
                horseControllerRelative,
                playerControllerRelative,
                sceneInstallerRelative
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

            string volumePath =
                Path.Combine(root, volumeRelative);
            string recoveryPath =
                Path.Combine(root, recoveryRelative);
            string horseSafetyPath =
                Path.Combine(root, horseSafetyRelative);
            string horseControllerPath =
                Path.Combine(root, horseControllerRelative);
            string playerControllerPath =
                Path.Combine(root, playerControllerRelative);
            string sceneInstallerPath =
                Path.Combine(root, sceneInstallerRelative);

            if (File.Exists(volumePath))
            {
                ValidateRequiredSourceTokens(
                    result,
                    volumeRelative,
                    File.ReadAllText(volumePath),
                    new[]
                    {
                        "BDHazardType.HoleOrChasm",
                        "BDHazardType.Lava",
                        "BDHazardType.Quicksand",
                        "BDQuicksandStatus.TouchActor",
                        "IsActorTouchingSurface",
                        "IsInsideHoleHorizontal"
                    },
                    "HAZARD_VOLUME_RULE_MISSING"
                );
            }

            if (File.Exists(recoveryPath))
            {
                ValidateRequiredSourceTokens(
                    result,
                    recoveryRelative,
                    File.ReadAllText(recoveryPath),
                    new[]
                    {
                        "HasRecentIntentionalGapEntry",
                        "forceActivation",
                        "ApplyUnavoidableDamage",
                        "TickHoleFall",
                        "TickLavaBounce",
                        "BDHazardType.Quicksand"
                    },
                    "PLAYER_HAZARD_RULE_MISSING"
                );
            }

            if (File.Exists(horseSafetyPath))
            {
                ValidateRequiredSourceTokens(
                    result,
                    horseSafetyRelative,
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

            if (File.Exists(horseControllerPath))
            {
                ValidateRequiredSourceTokens(
                    result,
                    horseControllerRelative,
                    File.ReadAllText(horseControllerPath),
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

            if (File.Exists(playerControllerPath))
            {
                ValidateRequiredSourceTokens(
                    result,
                    playerControllerRelative,
                    File.ReadAllText(playerControllerPath),
                    new[]
                    {
                        "HasRecentIntentionalGapEntry",
                        "NotifyForcedGapEntry",
                        "BDHazardVolume.FilterPlayerMotion"
                    },
                    "PLAYER_GROUND_EXIT_RULE_MISSING"
                );
            }

            if (File.Exists(sceneInstallerPath))
            {
                ValidateRequiredSourceTokens(
                    result,
                    sceneInstallerRelative,
                    File.ReadAllText(sceneInstallerPath),
                    new[]
                    {
                        "new Vector3(0f, 0.04f, 0f)",
                        "new Vector3(0f, 0.55f, 0f)",
                        "CleanSerializedYaml"
                    },
                    "HAZARD_SCENE_INSTALLER_RULE_MISSING"
                );
            }
        }

        private static void ScanC07PlayableFrameworkEncounter(
            BDOneClickQAResult result)
        {
            string root = ResolveProjectRoot();

            const string runtimeRelative =
                "Assets/_Project/Scripts/Runtime/Bosses/" +
                "BDC07FrameworkTestBoss.cs";

            const string installerRelative =
                "Assets/_Project/Scripts/Editor/Validation/" +
                "BDC07PlayableBossEncounterInstaller.cs";

            const string sceneInstallerRelative =
                "Assets/_Project/Scripts/Editor/Validation/" +
                "BDPrototypeHazardSceneInstaller.cs";

            string runtimePath =
                Path.Combine(root, runtimeRelative);

            string installerPath =
                Path.Combine(root, installerRelative);

            string sceneInstallerPath =
                Path.Combine(root, sceneInstallerRelative);

            if (!File.Exists(runtimePath) ||
                !File.Exists(installerPath) ||
                !File.Exists(sceneInstallerPath))
            {
                Add(
                    result,
                    BDOneClickQASeverity.Blocker,
                    "C07_PLAYABLE_ENCOUNTER_SOURCE_MISSING",
                    PrototypeScenePath,
                    BDC07PlayableBossEncounterInstaller
                        .EncounterRootName,
                    "One or more C07.16 source files are missing."
                );

                return;
            }

            ValidateRequiredSourceTokens(
                result,
                runtimeRelative,
                File.ReadAllText(runtimePath),
                new[]
                {
                    "BDBossEncounterController",
                    "encounter.IsCombatActive",
                    "ApplyDamage",
                    "AttackRoutine",
                    "arenaRadius"
                },
                "C07_PLAYABLE_BOSS_RUNTIME_MISSING"
            );

            ValidateRequiredSourceTokens(
                result,
                installerRelative,
                File.ReadAllText(installerPath),
                new[]
                {
                    "C07_FrameworkTestEncounter",
                    "BDBossHealthDamageBridge",
                    "BDBossEncounterRuntimeBindings",
                    "BDBossHealthHud",
                    "CreateEntryTrigger",
                    "ValidateActiveScene"
                },
                "C07_PLAYABLE_ENCOUNTER_INSTALLER_MISSING"
            );

            ValidateRequiredSourceTokens(
                result,
                sceneInstallerRelative,
                File.ReadAllText(sceneInstallerPath),
                new[]
                {
                    "BDC07PlayableBossEncounterInstaller",
                    "TryInstallActiveScene"
                },
                "C07_ENCOUNTER_ONE_BUTTON_WIRING_MISSING"
            );

            if (!BDC07PlayableBossEncounterInstaller
                    .ValidateActiveScene(out string error))
            {
                Add(
                    result,
                    BDOneClickQASeverity.Blocker,
                    "C07_PLAYABLE_ENCOUNTER_INVALID",
                    PrototypeScenePath,
                    BDC07PlayableBossEncounterInstaller
                        .EncounterRootName,
                    error
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


        private static void ScanGuardianSameRoomSpawnContracts(
            BDOneClickQAResult result)
        {
            string root = ResolveProjectRoot();
            const string relative =
                "Assets/_Project/Scripts/Runtime/Collectibles/" +
                "BDCollectibleGuardianSpawner.cs";
            string absolute = Path.Combine(root, relative);

            if (!File.Exists(absolute))
            {
                Add(
                    result,
                    BDOneClickQASeverity.Blocker,
                    "GUARDIAN_SPAWNER_MISSING",
                    relative,
                    string.Empty,
                    "The collectible guardian spawner source is missing."
                );
                return;
            }

            ValidateRequiredSourceTokens(
                result,
                relative,
                File.ReadAllText(absolute),
                new[]
                {
                    "BD SAME-ROOM GUARDIAN SPAWN SAFETY V1",
                    "TryResolveSpawnRoom",
                    "TryResolvePlayerRoomFallback",
                    "spawnRoom.ContainsWorldPosition(",
                    "playerTransform.position",
                    "ClampToRoomInterior",
                    "IsInsideRoomInterior",
                    "HasClearPathFromCollectible"
                },
                "GUARDIAN_SAME_ROOM_SAFETY_MISSING"
            );
        }

        private static void ScanHorseCleanStartContracts(
            BDOneClickQAResult result)
        {
            string root = ResolveProjectRoot();

            const string healthRelative =
                "Assets/_Project/Scripts/Runtime/BDHorseHealth.cs";

            const string controllerRelative =
                "Assets/_Project/Scripts/Runtime/BDHorseController.cs";

            ValidateRequiredSourceTokens(
                result,
                healthRelative,
                File.ReadAllText(Path.Combine(root, healthRelative)),
                new[]
                {
                    "BD HORSE CLEAN START V1",
                    "startupDamageProtectionSeconds",
                    "ResetForCleanGameStart",
                    "startup damage ignored"
                },
                "HORSE_CLEAN_START_HEALTH_MISSING"
            );

            ValidateRequiredSourceTokens(
                result,
                controllerRelative,
                File.ReadAllText(Path.Combine(root, controllerRelative)),
                new[]
                {
                    "BD HORSE CLEAN START V1",
                    "startupCalmUntil",
                    "TryResolveSafeStartPosition",
                    "BDHazardVolume.IsRecoveryPointUnsafe",
                    "HasLivingEnemyNearHorseOrPlayer",
                    "ignored remote combat"
                },
                "HORSE_CLEAN_START_CONTROLLER_MISSING"
            );
        }

        private static void ScanGameplayShadowPolicyContracts(
            BDOneClickQAResult result)
        {
            string root = ResolveProjectRoot();

            string runtimeRelative =
                "Assets/_Project/Scripts/Runtime/Rendering/" +
                "BDGameplayShadowPolicy.cs";

            string installerRelative =
                "Assets/_Project/Scripts/Editor/Validation/" +
                "BDGameplayShadowSceneInstaller.cs";

            string sceneInstallerRelative =
                "Assets/_Project/Scripts/Editor/Validation/" +
                "BDPrototypeHazardSceneInstaller.cs";

            string designRelative =
                "ProjectGuide/Features/Rendering/" +
                "GAMEPLAY_SHADOW_POLICY_V1.md";

            string[] requiredFiles =
            {
                runtimeRelative,
                installerRelative,
                designRelative
            };

            for (int index = 0;
                 index < requiredFiles.Length;
                 index++)
            {
                string relative = requiredFiles[index];

                if (File.Exists(
                        Path.Combine(root, relative)))
                {
                    continue;
                }

                Add(
                    result,
                    BDOneClickQASeverity.Blocker,
                    "GAMEPLAY_SHADOW_POLICY_FILE_MISSING",
                    relative,
                    string.Empty,
                    "Required gameplay shadow policy file is missing."
                );
            }

            if (File.Exists(
                    Path.Combine(root, runtimeRelative)))
            {
                ValidateRequiredSourceTokens(
                    result,
                    runtimeRelative,
                    File.ReadAllText(
                        Path.Combine(root, runtimeRelative)),
                    new[]
                    {
                        "keepRequiredAlwaysOn",
                        "optionalShadowDistance",
                        "maxOptionalShadowRenderers",
                        "dynamicDiscoveryInterval",
                        "ApplyRequiredShadows",
                        "ApplyOptionalBudget",
                        "BDPlayerMarker",
                        "BDHorseController",
                        "BDEnemyBootstrap",
                        "BDBossHealthChannel",
                        "BDCollectibleGuardianSpawner",
                        "Battery",
                        "GameBoy",
                        "Cartridge",
                        "Cassette",
                        "DecorationTokens"
                    },
                    "GAMEPLAY_SHADOW_POLICY_CONTRACT_MISSING"
                );
            }

            if (File.Exists(
                    Path.Combine(root, installerRelative)))
            {
                ValidateRequiredSourceTokens(
                    result,
                    installerRelative,
                    File.ReadAllText(
                        Path.Combine(root, installerRelative)),
                    new[]
                    {
                        "B&D Gameplay Shadow Policy",
                        "TryInstallActiveScene",
                        "ValidateActiveScene",
                        "ShadowCastingMode.Off",
                        "receiveShadows"
                    },
                    "GAMEPLAY_SHADOW_INSTALLER_CONTRACT_MISSING"
                );
            }

            if (File.Exists(
                    Path.Combine(root, sceneInstallerRelative)))
            {
                ValidateRequiredSourceTokens(
                    result,
                    sceneInstallerRelative,
                    File.ReadAllText(
                        Path.Combine(root, sceneInstallerRelative)),
                    new[]
                    {
                        "BDGameplayShadowSceneInstaller",
                        "TryInstallActiveScene"
                    },
                    "GAMEPLAY_SHADOW_ONE_BUTTON_WIRING_MISSING"
                );
            }

            if (!BDGameplayShadowSceneInstaller
                    .ValidateActiveScene(out string error))
            {
                Add(
                    result,
                    BDOneClickQASeverity.Blocker,
                    "GAMEPLAY_SHADOW_POLICY_INVALID",
                    PrototypeScenePath,
                    BDGameplayShadowSceneInstaller
                        .PolicyRootName,
                    error
                );
            }
        }
        private static void ScanMainMenuSettingsContracts(
            BDOneClickQAResult result)
        {
            string root = ResolveProjectRoot();

            string menuRelative =
                "Assets/_Project/Scripts/Runtime/UI/" +
                "BDMainMenuFlow.cs";

            string signalsRelative =
                "Assets/_Project/Scripts/Runtime/UI/" +
                "BDGameFlowSignals.cs";

            string progressRelative =
                "Assets/_Project/Scripts/Runtime/UI/" +
                "BDGameProgress.cs";

            string markerRelative =
                "Assets/_Project/Scripts/Runtime/UI/" +
                "BDGameCompletionMarker.cs";

            string settingsRelative =
                "Assets/_Project/Scripts/Runtime/UI/" +
                "BDGameSettings.cs";

            string installerRelative =
                "Assets/_Project/Scripts/Editor/Validation/" +
                "BDMainMenuSettingsSceneInstaller.cs";

            string healthRelative =
                "Assets/_Project/Scripts/Runtime/" +
                "BDHealth.cs";

            string sceneInstallerRelative =
                "Assets/_Project/Scripts/Editor/Validation/" +
                "BDPrototypeHazardSceneInstaller.cs";

            string designRelative =
                "ProjectGuide/Features/UI/" +
                "MAIN_MENU_SETTINGS_RESULT_FLOW_V2.md";

            string[] requiredFiles =
            {
                menuRelative,
                signalsRelative,
                progressRelative,
                markerRelative,
                settingsRelative,
                installerRelative,
                healthRelative,
                designRelative
            };

            for (int index = 0;
                 index < requiredFiles.Length;
                 index++)
            {
                string relative =
                    requiredFiles[index];

                if (File.Exists(
                        Path.Combine(root, relative)))
                {
                    continue;
                }

                Add(
                    result,
                    BDOneClickQASeverity.Blocker,
                    "MAIN_MENU_SETTINGS_FILE_MISSING",
                    relative,
                    string.Empty,
                    "Required main-menu/settings V2 file is missing."
                );
            }

            if (File.Exists(
                    Path.Combine(root, menuRelative)))
            {
                string menuSource =
                    File.ReadAllText(
                        Path.Combine(root, menuRelative));

                ValidateRequiredSourceTokens(
                    result,
                    menuRelative,
                    menuSource,
                    new[]
                    {
                        "\"START GAME\"",
                        "HandlePlayerDeath",
                        "BeginResultSequence",
                        "ReturnToMainMenuAfterSequence",
                        "CompleteMotherVictorySequence",
                        "BDGameProgress.MotherDefeated",
                        "DrawCompletedGameBoyRelic",
                        "LoadSceneAsync"
                    },
                    "MAIN_MENU_FLOW_V2_CONTRACT_MISSING"
                );

                if (menuSource.Contains("\"DEFEAT\"") ||
                    menuSource.Contains("\"VICTORY\"") ||
                    menuSource.Contains("\"START NEW GAME\"") ||
                    menuSource.Contains("\"I'M BORED\""))
                {
                    Add(
                        result,
                        BDOneClickQASeverity.Blocker,
                        "MAIN_MENU_RESULT_TEXT_FORBIDDEN",
                        menuRelative,
                        "DrawMainMenu",
                        "The main menu must not display result wording or change its Start-button text."
                    );
                }
            }

            if (File.Exists(
                    Path.Combine(root, signalsRelative)))
            {
                ValidateRequiredSourceTokens(
                    result,
                    signalsRelative,
                    File.ReadAllText(
                        Path.Combine(root, signalsRelative)),
                    new[]
                    {
                        "TryHandleDeath",
                        "BeginResultSequence",
                        "ReturnToMainMenuAfterSequence",
                        "CompleteMotherVictorySequence"
                    },
                    "RESULT_SEQUENCE_ROUTER_MISSING"
                );
            }

            if (File.Exists(
                    Path.Combine(root, progressRelative)))
            {
                ValidateRequiredSourceTokens(
                    result,
                    progressRelative,
                    File.ReadAllText(
                        Path.Combine(root, progressRelative)),
                    new[]
                    {
                        "MotherDefeated",
                        "MarkMotherDefeated",
                        "PlayerPrefs.Save"
                    },
                    "MOTHER_COMPLETION_PROGRESS_MISSING"
                );
            }

            if (File.Exists(
                    Path.Combine(root, markerRelative)))
            {
                ValidateRequiredSourceTokens(
                    result,
                    markerRelative,
                    File.ReadAllText(
                        Path.Combine(root, markerRelative)),
                    new[]
                    {
                        "BeginResultSequence",
                        "FinishSequenceToUnchangedMainMenu",
                        "FinishMotherVictorySequence"
                    },
                    "CUTSCENE_RESULT_MARKER_MISSING"
                );
            }

            if (File.Exists(
                    Path.Combine(root, healthRelative)))
            {
                ValidateRequiredSourceTokens(
                    result,
                    healthRelative,
                    File.ReadAllText(
                        Path.Combine(root, healthRelative)),
                    new[]
                    {
                        "BDGameFlowSignals.TryHandleDeath",
                        "Died?.Invoke(this)"
                    },
                    "PLAYER_DEATH_MENU_ROUTING_MISSING"
                );
            }

            if (File.Exists(
                    Path.Combine(root, sceneInstallerRelative)))
            {
                ValidateRequiredSourceTokens(
                    result,
                    sceneInstallerRelative,
                    File.ReadAllText(
                        Path.Combine(root, sceneInstallerRelative)),
                    new[]
                    {
                        "BDMainMenuSettingsSceneInstaller",
                        "TryInstallActiveScene"
                    },
                    "MAIN_MENU_ONE_BUTTON_WIRING_MISSING"
                );
            }

            if (!BDMainMenuSettingsSceneInstaller
                    .ValidateActiveScene(
                        out string error))
            {
                Add(
                    result,
                    BDOneClickQASeverity.Blocker,
                    "MAIN_MENU_SETTINGS_SCENE_INVALID",
                    PrototypeScenePath,
                    BDMainMenuSettingsSceneInstaller
                        .RootName,
                    error
                );
            }
        }

        private static void ScanDreamyMainMenuContracts(
            BDOneClickQAResult result)
        {
            string root = ResolveProjectRoot();

            string backdropRelative =
                "Assets/_Project/Scripts/Runtime/UI/" +
                "BDDreamyMainMenuBackdrop.cs";

            string menuRelative =
                "Assets/_Project/Scripts/Runtime/UI/" +
                "BDMainMenuFlow.cs";

            string introRelative =
                "Assets/_Project/Scripts/Runtime/UI/" +
                "BDBBHBootIntro.cs";

            string designRelative =
                "ProjectGuide/Features/UI/" +
                "MAIN_MENU_DREAMY_STORYBOOK_V1.md";

            string[] requiredFiles =
            {
                backdropRelative,
                menuRelative,
                introRelative,
                designRelative
            };

            for (int index = 0;
                 index < requiredFiles.Length;
                 index++)
            {
                string relative = requiredFiles[index];

                if (File.Exists(
                        Path.Combine(root, relative)))
                {
                    continue;
                }

                Add(
                    result,
                    BDOneClickQASeverity.Blocker,
                    "DREAMY_MAIN_MENU_FILE_MISSING",
                    relative,
                    string.Empty,
                    "Required dreamy main-menu file is missing."
                );
            }

            if (File.Exists(
                    Path.Combine(root, backdropRelative)))
            {
                ValidateRequiredSourceTokens(
                    result,
                    backdropRelative,
                    File.ReadAllText(
                        Path.Combine(root, backdropRelative)),
                    new[]
                    {
                        "DrawMoon",
                        "DrawStars",
                        "DrawCloudHaze",
                        "DrawStorybookHorizon",
                        "DrawGoldenPath",
                        "CreateVerticalGradientTexture",
                        "CreateRadialGlowTexture"
                    },
                    "DREAMY_MAIN_MENU_BACKDROP_MISSING"
                );
            }

            if (File.Exists(
                    Path.Combine(root, menuRelative)))
            {
                ValidateRequiredSourceTokens(
                    result,
                    menuRelative,
                    File.ReadAllText(
                        Path.Combine(root, menuRelative)),
                    new[]
                    {
                        "dreamyPanelTexture",
                        "dreamyButtonHoverTexture",
                        "DrawDreamyMenuOrnament",
                        "CreateMenuTexture",
                        "DestroyMenuTextures",
                        "START GAME",
                        "SETTINGS"
                    },
                    "DREAMY_MAIN_MENU_SKIN_MISSING"
                );
            }

            if (File.Exists(
                    Path.Combine(root, introRelative)))
            {
                string introSource =
                    File.ReadAllText(
                        Path.Combine(root, introRelative));

                if (introSource.Contains(
                        "CreateDynamicFontFromOSFont") ||
                    introSource.Contains(
                        "ResolveIntroFont") ||
                    introSource.Contains(
                        "introFont"))
                {
                    Add(
                        result,
                        BDOneClickQASeverity.Blocker,
                        "BBH_BOOT_INTRO_INVALID_FONT_REFERENCE",
                        introRelative,
                        "EnsureResources",
                        "BBH intro must use Unity's GUI skin font and must not retain a temporary OS font."
                    );
                }

                if (!introSource.Contains(
                        "font = GUI.skin.label.font"))
                {
                    Add(
                        result,
                        BDOneClickQASeverity.Blocker,
                        "BBH_BOOT_INTRO_SAFE_FONT_MISSING",
                        introRelative,
                        "EnsureResources",
                        "BBH intro is missing the GUI skin font assignment."
                    );
                }
            }
        }

        private static void ScanNaturalMovementAwarenessFacingContracts(
            BDOneClickQAResult result)
        {
            string root = ResolveProjectRoot();

            string[] requiredFiles =
            {
                "Assets/_Project/Scripts/Runtime/BDEnemyMovementPolish.cs",
                "Assets/_Project/Scripts/Runtime/BDEnemyAwarenessPulse.cs",
                "Assets/_Project/Scripts/Runtime/BDTemporaryFacingIndicator.cs",
                "Assets/_Project/Scripts/Runtime/Hazards/BDHorseHazardSafety.cs",
                "Assets/_Project/Scripts/Runtime/BDHorseController.cs",
                "Assets/_Project/Scripts/Runtime/BDPlayerController.cs",
                "ProjectGuide/Features/Movement/NATURAL_MOVEMENT_AWARENESS_FACING_V1.md"
            };

            for (int index = 0;
                 index < requiredFiles.Length;
                 index++)
            {
                string relative = requiredFiles[index];

                if (File.Exists(Path.Combine(root, relative)))
                    continue;

                Add(
                    result,
                    BDOneClickQASeverity.Blocker,
                    "NATURAL_MOVEMENT_FILE_MISSING",
                    relative,
                    string.Empty,
                    "Required movement, awareness, hazard-refusal, or temporary-facing file is missing."
                );
            }

            ValidateRequiredSourceTokens(
                result,
                "Assets/_Project/Scripts/Runtime/BDPlayerController.cs",
                File.ReadAllText(Path.Combine(root,
                    "Assets/_Project/Scripts/Runtime/BDPlayerController.cs")),
                new[]
                {
                    "ApplyNaturalMovementProfile",
                    "moveAcceleration = 22f",
                    "moveDeceleration = 28f"
                },
                "PLAYER_NATURAL_MOVEMENT_CONTRACT_MISSING"
            );

            ValidateRequiredSourceTokens(
                result,
                "Assets/_Project/Scripts/Runtime/BDHorseController.cs",
                File.ReadAllText(Path.Combine(root,
                    "Assets/_Project/Scripts/Runtime/BDHorseController.cs")),
                new[]
                {
                    "ApplyNaturalHorseMovementProfile",
                    "mountedMoveSpeed = 9.6f",
                    "mountedTravelTurnDegreesPerSecond",
                    "SmoothMountedTravelDirection",
                    "natural-wide-horse-turn"
                },
                "HORSE_NATURAL_MOVEMENT_CONTRACT_MISSING"
            );

            string horseHazardSource =
                File.ReadAllText(Path.Combine(root,
                    "Assets/_Project/Scripts/Runtime/Hazards/BDHorseHazardSafety.cs"));

            ValidateRequiredSourceTokens(
                result,
                "Assets/_Project/Scripts/Runtime/Hazards/BDHorseHazardSafety.cs",
                horseHazardSource,
                new[]
                {
                    "hazardLookAheadDistance",
                    "hazardRetreatDistance = 2.6f",
                    "hazardRetreatSpeed = 4.8f",
                    "hazardRetreatRemainingDistance",
                    "TryBeginHazardRetreat",
                    "BuildHazardRetreatMotion",
                    "FinishHazardRetreat",
                    "IsRefusingHazard",
                    "IsHorsePathSafe"
                },
                "HORSE_HAZARD_RETREAT_CONTRACT_MISSING"
            );

            if (horseHazardSource.Contains(
                    "hazardRefusalStopSeconds") ||
                horseHazardSource.Contains(
                    "hazardSwerveScale") ||
                horseHazardSource.Contains(
                    "hazardRefusalUntil"))
            {
                Add(
                    result,
                    BDOneClickQASeverity.Blocker,
                    "OBSOLETE_HORSE_HAZARD_STOP_RETURNED",
                    "Assets/_Project/Scripts/Runtime/Hazards/BDHorseHazardSafety.cs",
                    "FilterMovement",
                    "The old one-second refusal/stop implementation must not return."
                );
            }

            ValidateRequiredSourceTokens(
                result,
                "Assets/_Project/Scripts/Runtime/BDEnemyAwarenessPulse.cs",
                File.ReadAllText(Path.Combine(root,
                    "Assets/_Project/Scripts/Runtime/BDEnemyAwarenessPulse.cs")),
                new[]
                {
                    "refreshInterval = 0.08f",
                    "ApplyMinimumRanges",
                    "BDTargetFinder.FindPlayer",
                    "maxShootRange"
                },
                "ENEMY_AWARENESS_CONTRACT_MISSING"
            );

            ValidateRequiredSourceTokens(
                result,
                "Assets/_Project/Scripts/Runtime/BDTemporaryFacingIndicator.cs",
                File.ReadAllText(Path.Combine(root,
                    "Assets/_Project/Scripts/Runtime/BDTemporaryFacingIndicator.cs")),
                new[]
                {
                    "BD_TEMP_FRONT_UNTIL_REAL_MODELS",
                    "BD_TEMP_REAR_UNTIL_REAL_MODELS",
                    "ActorKind.Player",
                    "ActorKind.Horse",
                    "ActorKind.Enemy"
                },
                "TEMPORARY_FACING_MARKER_CONTRACT_MISSING"
            );

            ValidateRequiredSourceTokens(
                result,
                "Assets/_Project/Scripts/Runtime/UI/BDMainMenuFlow.cs",
                File.ReadAllText(Path.Combine(root,
                    "Assets/_Project/Scripts/Runtime/UI/BDMainMenuFlow.cs")),
                new[]
                {
                    "StartGameHighlightTint",
                    "MenuActionVisual.Progress"
                },
                "START_GAME_HIGHLIGHT_CONTRACT_MISSING"
            );
        }

        private static void ScanSpinningAoeAttackContracts(
            BDOneClickQAResult result)
        {
            string root = ResolveProjectRoot();

            const string combatRelative =
                "Assets/_Project/Scripts/Runtime/BDPlayerCombat.cs";

            const string visualRelative =
                "Assets/_Project/Scripts/Runtime/BDSpinAttackVisual.cs";

            const string designRelative =
                "ProjectGuide/Features/Combat/" +
                "SPINNING_AOE_LONG_PRESS_LIGHT_ATTACK_V1.md";

            string[] requiredFiles =
            {
                combatRelative,
                visualRelative,
                designRelative
            };

            for (int index = 0;
                 index < requiredFiles.Length;
                 index++)
            {
                string relative = requiredFiles[index];

                if (File.Exists(Path.Combine(root, relative)))
                    continue;

                Add(
                    result,
                    BDOneClickQASeverity.Blocker,
                    "SPINNING_AOE_FILE_MISSING",
                    relative,
                    string.Empty,
                    "Required spinning AOE attack file is missing."
                );
            }

            string combatPath = Path.Combine(root, combatRelative);
            if (File.Exists(combatPath))
            {
                ValidateRequiredSourceTokens(
                    result,
                    combatRelative,
                    File.ReadAllText(combatPath),
                    new[]
                    {
                        "Spinning AOE Attack",
                        "spinningAttackHoldThreshold = 0.24f",
                        "spinningAttackCooldown = 0.85f",
                        "spinningAttackDamageMultiplier = 0.82f",
                        "BeginPendingLightPress",
                        "TickPendingLightPress",
                        "TrySpinningAoeAttack",
                        "Time.time < nextSpinningAttackAllowedAt",
                        "TryMeleeAttack(",
                        "Physics.OverlapSphereNonAlloc",
                        "WeaponDamageMultiplier",
                        "BDSpinAttackVisual.Spawn",
                        "spinningAttackKnockbackStrength",
                        "TriggerSpinningAttackGlobalFeedback"
                    },
                    "SPINNING_AOE_COMBAT_CONTRACT_MISSING"
                );
            }

            string visualPath = Path.Combine(root, visualRelative);
            if (File.Exists(visualPath))
            {
                ValidateRequiredSourceTokens(
                    result,
                    visualRelative,
                    File.ReadAllText(visualPath),
                    new[]
                    {
                        "BD_Spin_Attack_AOE_Visual",
                        "rotations = 1.55f",
                        "BuildArcMesh",
                        "Time.unscaledDeltaTime",
                        "AllowCosmeticSpawn"
                    },
                    "SPINNING_AOE_VISUAL_CONTRACT_MISSING"
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
