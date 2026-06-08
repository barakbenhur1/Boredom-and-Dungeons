#if UNITY_EDITOR
using System;
using System.IO;

namespace BoredomAndDungeons.EditorTools.Validation
{
    internal static class BDV20ActiveRegressionQA
    {
        public static void Scan(BDOneClickQAResult result)
        {
            if (result == null)
                return;

            string root = Directory.GetParent(
                UnityEngine.Application.dataPath
            ).FullName;

            Require(result, root,
                "Assets/_Project/Scripts/Runtime/Combat/BDChargedShotVisuals.cs",
                "V20_CHARGED_PROJECTILE_CONTRACT_MISSING",
                "BD CHARGED PROJECTILE IDEMPOTENT BUILD V20",
                "GetComponent<TrailRenderer>()",
                "if (chargedTrail == null)",
                "BuildOrRefreshProjectileOrbit"
            );

            Require(result, root,
                "Assets/_Project/Scripts/Runtime/RunPresentation/BDRunPresentationCoordinator.cs",
                "V20_RUN_PRESENTATION_CONTRACT_MISSING",
                "BD V20 CINEMATIC INPUT TURN STOP + AUDIO LISTENER",
                "BD FIRST-FRAME CINEMATIC CAMERA PRIME V23R5",
                "PrimeMountedEntranceCameraBeforeRender",
                "ClearPrimedMountedEntranceCamera",
                "mountedEntranceCameraPrimed",
                "BD HIGHER ENTRANCE ESTABLISHING CAMERA V21",
                "roomDepth * 0.72f",
                "17.0f",
                "21.5f",
                "BD EXACTLY ONE ACTIVE AUDIO LISTENER V20",
                "roomDepth * 0.30f",
                "Quaternion.AngleAxis(90f, Vector3.up)",
                "fullStopHoldDuration = 0.24f",
                "cameraReturnDuration = 0.60f",
                "PrepareMountedGameplayAfterCinematic",
                "EnsureExactlyOneActiveAudioListener"
            );

            Require(result, root,
                "Assets/_Project/Scripts/Runtime/UI/BDBBHBootIntro.cs",
                "V20_BBH_FIRST_FRAME_CONTRACT_MISSING",
                "BD FIRST RENDER FRAME BLACK CLOCK V20",
                "renderClockStarted",
                "EventType.Repaint",
                "InitialBlackHold = 0.52f"
            );

            Require(result, root,
                "Assets/_Project/Scripts/Runtime/BDCameraFollow.cs",
                "V20_CLOSED_WALL_CAMERA_CONTRACT_MISSING",
                "BD CLOSED-WALL FRUSTUM CONTAINMENT V20",
                "BD SINGLE CAMERA TRANSFORM OWNER V23",
                "BD STABLE SINGLE-STAGE CAMERA YAW V23",
                "BD PLANAR COMBAT SHAKE V23",
                "BD STABLE WALL PRESSURE CAMERA V23R4",
                "cameraFrustumSafetyInset = 1.20f",
                "constrainedLookPointSmooth = 12f",
                "roomSwitchInteriorMargin = 1.25f",
                "roomHandoffReleaseMargin = 2.25f",
                "ResolvePlanarCameraShake",
                "ResolveCameraClampBounds",
                "handoff wall cast containment",
                "union room boundary handoff",
                "BD ACTUAL-POSE ROOM HANDOFF RELEASE V23R6",
                "TryCompleteRoomHandoffAfterFinalPose",
                "completed actual-pose room handoff"
            );


            Require(result, root,
                "Assets/_Project/Scripts/Runtime/BDOccludingWall.cs",
                "V22R2_EDIT_MODE_MATERIAL_SAFETY_MISSING",
                "BD EDIT-MODE SHARED MATERIAL SAFETY V22R2",
                "Application.isPlaying",
                "cachedRenderer.sharedMaterial"
            );

            Forbid(result, root,
                "Assets/_Project/Scripts/Runtime/BDOccludingWall.cs",
                "V22R2_EDIT_MODE_MATERIAL_INSTANCE_LEAK",
                "runtimeMaterial = cachedRenderer.material;"
            );

            Forbid(result, root,
                "Assets/_Project/Scripts/Runtime/BDCameraFollow.cs",
                "V22R2_UNUSED_CAMERA_DISTANCE_FIELD",
                "minimumCameraDistance"
            );

            Require(result, root,
                "Assets/_Project/Scripts/Runtime/BDCameraOccluderFader.cs",
                "V20_STRUCTURAL_WALL_FADE_CONTRACT_MISSING",
                "BD STRUCTURAL WALL FORCE-OPAQUE V22",
                "IsStructuralRoomBoundary",
                "ForceOpaqueImmediateAndDisableFading",
                "BDWallSurfaceProfile"
            );

            Require(result, root,
                "Assets/_Project/Scripts/Runtime/BDOccludingWallAutoAttach.cs",
                "V20_STRUCTURAL_WALL_AUTOATTACH_CONTRACT_MISSING",
                "BD SANITIZE LEGACY STRUCTURAL WALL FADERS V22",
                "ForceOpaqueImmediateAndDisableFading",
                "GetComponentInParent<BDWallSurfaceProfile>()"
            );

            Require(result, root,
                "Assets/_Project/Scripts/Editor/Validation/BDRoomBoundarySceneInstaller.cs",
                "V21_WALL_HEIGHT_CONTRACT_MISSING",
                "BD PERMANENT OPAQUE TALL WALLS V22",
                "MinimumWallWorldHeight = 64f",
                "Undo.DestroyObjectImmediate(legacyFader)"
            );

            Require(result, root,
                "Assets/_Project/Scripts/Runtime/BDMouseAimUtility.cs",
                "V20_MOUSE_LOCK_CONTRACT_MISSING",
                "BD CINEMATIC MOUSE AIM HARD LOCK V20",
                "BDRunPresentationCoordinator.InputLocked"
            );

            Require(result, root,
                "Assets/_Project/Scripts/Runtime/BDHorseController.cs",
                "V20_HORSE_INPUT_RESET_CONTRACT_MISSING",
                "BD PRESENTATION HORSE INPUT HARD LOCK V20",
                "BD POST-CINEMATIC MOUNTED INPUT RESET V20",
                "PrepareMountedGameplayAfterCinematic"
            );

            string coordinator = Read(root,
                "Assets/_Project/Scripts/Runtime/RunPresentation/BDRunPresentationCoordinator.cs");
            int stop = coordinator.IndexOf(
                "fullStopHoldDuration = 0.24f",
                StringComparison.Ordinal);
            // BD SEMANTIC MOUNTED INTRO RELEASE ORDER QA V23R19J
            // V23R19G consolidated restoration into RestoreMountedIntroControls.
            // Validate the active owner and retain compatibility with older reviewed states.
            int restoreMounted = coordinator.IndexOf(
                "RestoreMountedIntroControls(",
                stop >= 0 ? stop : 0,
                StringComparison.Ordinal);
            int restoreLegacy = coordinator.IndexOf(
                "RestoreControls(states)",
                stop >= 0 ? stop : 0,
                StringComparison.Ordinal);
            int restore = restoreMounted >= 0
                ? restoreMounted
                : restoreLegacy;
            int unlock = coordinator.IndexOf(
                "inputLocked = false",
                restore >= 0 ? restore : (stop >= 0 ? stop : 0),
                StringComparison.Ordinal);
            if (stop < 0 || restore < stop || unlock < restore)
            {
                Add(result,
                    "V20_INPUT_RELEASE_ORDER_INVALID",
                    "Input/control release must remain after the right turn and full-stop hold.");
            }

            int scenePrime = coordinator.IndexOf(
                "PrimeMountedEntranceCameraBeforeRender();",
                StringComparison.Ordinal);
            int prepareStart = coordinator.IndexOf(
                "StartCoroutine(PrepareLoadedScene())",
                StringComparison.Ordinal);
            int prepareMethod = coordinator.IndexOf(
                "private IEnumerator PrepareLoadedScene()",
                StringComparison.Ordinal);
            int firstSetup = coordinator.IndexOf(
                "DestroyObsoleteGeneratedPortals();",
                prepareMethod >= 0 ? prepareMethod : 0,
                StringComparison.Ordinal);
            int firstPrepareYield = coordinator.IndexOf(
                "yield return null;",
                prepareMethod >= 0 ? prepareMethod : 0,
                StringComparison.Ordinal);
            if (scenePrime < 0 ||
                prepareStart < 0 ||
                scenePrime > prepareStart ||
                prepareMethod < 0 ||
                firstSetup < 0 ||
                (firstPrepareYield >= 0 && firstPrepareYield < firstSetup))
            {
                Add(result,
                    "V23R5_FIRST_FRAME_CAMERA_PRIME_ORDER_INVALID",
                    "The mounted cinematic camera must be primed in sceneLoaded before the coroutine and before any frame yield.");
            }
        }

        private static string Read(string root, string relative)
        {
            string absolute = Path.Combine(root, relative);
            return File.Exists(absolute)
                ? File.ReadAllText(absolute)
                : string.Empty;
        }

        private static void Require(
            BDOneClickQAResult result,
            string root,
            string relative,
            string code,
            params string[] tokens)
        {
            string source = Read(root, relative);
            if (string.IsNullOrEmpty(source))
            {
                Add(result, code, "Missing required file: " + relative);
                return;
            }

            foreach (string token in tokens)
            {
                if (source.IndexOf(token, StringComparison.Ordinal) >= 0)
                    continue;
                Add(result, code, "Missing V20 contract: " + token);
            }
        }

        private static void Forbid(
            BDOneClickQAResult result,
            string root,
            string relative,
            string code,
            params string[] forbiddenTokens)
        {
            string source = Read(root, relative);
            if (string.IsNullOrEmpty(source))
            {
                Add(result, code, "Missing required file: " + relative);
                return;
            }

            foreach (string token in forbiddenTokens)
            {
                if (source.IndexOf(token, StringComparison.Ordinal) < 0)
                    continue;

                Add(result, code, "Forbidden warning-prone contract remains: " + token);
            }
        }

        private static void Add(
            BDOneClickQAResult result,
            string code,
            string message)
        {
            result.findings.Add(
                new BDOneClickQAFinding(
                    BDOneClickQASeverity.Blocker,
                    code,
                    string.Empty,
                    string.Empty,
                    message
                )
            );
        }
    }
}
#endif
