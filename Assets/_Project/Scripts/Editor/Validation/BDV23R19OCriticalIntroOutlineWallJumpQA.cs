#if UNITY_EDITOR
using System;
using System.IO;

namespace BoredomAndDungeons.EditorTools.Validation
{
    internal static class BDV23R19OCriticalIntroOutlineWallJumpQA
    {
        public static void Scan(BDOneClickQAResult result)
        {
            if (result == null)
                return;

            string root = Directory.GetParent(
                UnityEngine.Application.dataPath
            ).FullName;

            Require(result, root,
                "Assets/_Project/Scripts/Runtime/RunPresentation/BDRunPresentationCoordinator.cs",
                "V23R19O_INTRO_FIRST_VISIBLE_FRAME_MISSING",
                "BD MOUNTED INTRO RIDER FIRST-VISIBLE-FRAME V23R19O",
                "CaptureMountedIntroRiderPresentationBaseline",
                "ForceMountedIntroRiderPresentationVisible",
                "updateWhenOffscreen = true",
                "EnsureMountedIntroRiderVisible"
            );

            Require(result, root,
                "Assets/_Project/Scripts/Runtime/BDHorseController.cs",
                "V23R19O_HORSE_VISIBILITY_REASSERT_MISSING",
                "BDRunPresentationCoordinator.EnsureMountedIntroRiderVisible"
            );

            Require(result, root,
                "Assets/_Project/Scripts/Runtime/Combat/BDTargetOutlineVisual.cs",
                "V23R19O_DAMAGEABLE_MODEL_OUTLINE_MISSING",
                "BD DAMAGEABLE MODEL ONLY TARGET OUTLINE V23R19O",
                "ResolveDamageableColliders",
                "RendererIntersectsDamageableEnvelope",
                "IsAuxiliaryRingRenderer"
            );

            Require(result, root,
                "Assets/_Project/Scripts/Runtime/Combat/BDAuxiliaryEnemyRingTransparency.cs",
                "V23R19O_RING_TRANSPARENCY_OWNER_MISSING",
                "BD AUXILIARY ENEMY RING TRANSPARENCY V23R19O",
                "alphaMultiplier = 0.62f",
                "ApplyTransparency"
            );

            Require(result, root,
                "Assets/_Project/Scripts/Runtime/BDEnemyBootstrap.cs",
                "V23R19O_RING_INSTALLER_MISSING",
                "BDAuxiliaryEnemyRingTransparency"
            );

            Require(result, root,
                "Assets/_Project/Scripts/Runtime/BDPlayerController.cs",
                "V23R19O_STEERABLE_WALL_JUMP_MISSING",
                "BD STEERABLE LONGER WALL JUMP V23R19O",
                "wallJumpSteeringDegreesPerSecond",
                "wallJumpMinimumRetainedSpeed",
                "TickWallJumpMotion(worldMove, wantsMove)",
                "RotateToward(lastLookDirection)"
            );

            Require(result, root,
                "Assets/_Project/Scripts/Runtime/BDCameraFollow.cs",
                "V23R19O_WALL_JUMP_CAMERA_YAW_MISSING",
                "wallJumpYawSpeedMultiplier",
                "playerController.IsWallJumping"
            );

            Require(result, root,
                "Assets/_Project/Design/Runtime/OPEN_BUG_TRACKER.md",
                "V23R19O_BUG_LEDGER_MISSING",
                "BUG-V23R19G-005",
                "BUG-V23R19O-001"
            );

            Require(result, root,
                "PROJECT_STATUS.md",
                "V23R19O_STATUS_MISSING",
                "C03/C04/C11.RUNTIME.V23R19O",
                "V23R19O acceptance gate"
            );
        }

        private static string Read(
            string root,
            string relative)
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
                Add(
                    result,
                    code,
                    "Missing required file: " + relative
                );
                return;
            }

            foreach (string token in tokens)
            {
                if (source.IndexOf(
                        token,
                        StringComparison.Ordinal) >= 0)
                {
                    continue;
                }

                Add(
                    result,
                    code,
                    "Missing V23R19O contract: " + token
                );
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
