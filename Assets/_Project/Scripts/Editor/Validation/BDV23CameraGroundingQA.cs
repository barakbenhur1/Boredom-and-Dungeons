#if UNITY_EDITOR
using System;
using System.IO;

namespace BoredomAndDungeons.EditorTools.Validation
{
    internal static class BDV23CameraGroundingQA
    {
        public static void Scan(BDOneClickQAResult result)
        {
            if (result == null)
                return;

            string root = Directory.GetParent(
                UnityEngine.Application.dataPath
            ).FullName;

            Require(result, root,
                "Assets/_Project/Scripts/Runtime/BDCameraFollow.cs",
                "V23_STABLE_CAMERA_OWNER_MISSING",
                "BD SINGLE CAMERA TRANSFORM OWNER V23",
                "BD STABLE SINGLE-STAGE CAMERA YAW V23",
                "BD PLANAR COMBAT SHAKE V23",
                "BD STABLE WALL PRESSURE CAMERA V23R4",
                "BD ROOM WALL CAST ONLY DURING HANDOFF V23R4",
                "BD INDEPENDENT LOOK POINT SOFT CONSTRAINT V23R4",
                "BD SINGLE ROOM SCAN PER FRAME V23R4",
                "ResolvePlanarCameraShake",
                "ResolveStableCameraSafetyInset",
                "ResolveSmoothedLookPoint",
                "ResolveCachedRooms",
                "transform.SetPositionAndRotation"
            );

            Require(result, root,
                "Assets/_Project/Scenes/02_CleanCore_MazePrototype.unity",
                "V23R4_CAMERA_SCENE_TUNING_MISSING",
                "cameraFrustumSafetyInset: 1.2",
                "constrainedLookPointSmooth: 12",
                "roomCacheRefreshInterval: 1"
            );

            ForbidFile(result, root,
                "Assets/_Project/Scripts/Runtime/Camera/BDCameraForwardViewBias.cs",
                "V23_SECONDARY_CAMERA_OWNER_REMAINS"
            );

            Require(result, root,
                "Assets/_Project/Scripts/Runtime/Hazards/BDPlayerHazardRecovery.cs",
                "V23_COMBAT_GROUNDING_GUARD_MISSING",
                "BD COMBAT FLOOR LOSS GUARD V23",
                "NotifyCombatImpact",
                "CheckCombatGroundingGuard",
                "HasWalkableGroundSupport",
                "BD CHARACTER CONTROLLER ROOT-SAFE RECOVERY V23",
                "ResolveCharacterControllerRootHeightAboveGround",
                "IsValidRecoveryGroundCollider"
            );

            Require(result, root,
                "Assets/_Project/Scripts/Runtime/BDHealth.cs",
                "V23_COMBAT_GROUNDING_NOTIFY_MISSING",
                "BD PLAYER COMBAT IMPACT GROUNDING NOTIFY V23",
                "NotifyPlayerCombatImpactForGrounding",
                "recovery.NotifyCombatImpact()"
            );

            Require(result, root,
                "Assets/_Project/Scripts/Runtime/BDPlayerController.cs",
                "V23R2_ACTIVE_GAP_INTENT_MISSING",
                "BD ACTIVE GAP ENTRY ONLY V23R2",
                "BD POST-RECOVERY WALK REENTRY SUPPRESSION V23R3",
                "HasActiveIntentionalGapEntry",
                "IsPostRecoveryGapEntrySuppressed",
                "activeAscendingJump",
                "PostRecoveryGapEntrySuppressionSeconds = 0.85f"
            );

            Require(result, root,
                "Assets/_Project/Scripts/Runtime/Hazards/BDHazardVolume.cs",
                "V23R2_SWEPT_HOLE_GUARD_MISSING",
                "BD SWEPT HOLE FOOTPRINT GUARD V23R2",
                "WouldSweepIntoHole",
                "BD NEAREST HOLE EXIT POINT V23R2",
                "TryResolveNearestHorizontalExitPoint"
            );

            Require(result, root,
                "Assets/_Project/Scripts/Runtime/Hazards/BDPlayerHazardRecovery.cs",
                "V23R2_LOCAL_HOLE_RECOVERY_MISSING",
                "BD NEAR-HOLE LOCAL RECOVERY V23R2",
                "CaptureLocalHoleRecoveryAnchor",
                "TryResolveLocalHoleRecoveryAnchor",
                "holeRespawnEdgeClearance"
            );

            ForbidToken(result, root,
                "Assets/_Project/Scripts/Runtime/BDPlayerController.cs",
                "V23R2_STALE_GAP_GRACE_REMAINS",
                "Time.time - lastDodgeStartedAt <= 0.40f",
                "Time.time - lastJumpStartedAt <= 0.75f"
            );

            ForbidToken(result, root,
                "Assets/_Project/Scripts/Runtime/BDCameraFollow.cs",
                "V23_VARIABLE_CAMERA_FEEL_REMAINS",
                "ConstrainForwardAgainstClosedWalls",
                "ResolveClosedWallProximity01",
                "nearClosedWallPitch",
                "movementDirectionBlend"
            );
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
                Add(result, code, "Missing V23 contract: " + token);
            }
        }

        private static void ForbidToken(
            BDOneClickQAResult result,
            string root,
            string relative,
            string code,
            params string[] tokens)
        {
            string source = Read(root, relative);
            foreach (string token in tokens)
            {
                if (source.IndexOf(token, StringComparison.Ordinal) < 0)
                    continue;
                Add(result, code, "Obsolete V23 camera token remains: " + token);
            }
        }

        private static void ForbidFile(
            BDOneClickQAResult result,
            string root,
            string relative,
            string code)
        {
            if (File.Exists(Path.Combine(root, relative)))
                Add(result, code, "Obsolete secondary camera owner remains: " + relative);
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
