#if UNITY_EDITOR
using System;
using System.IO;

namespace BoredomAndDungeons.EditorTools.Validation
{
    internal static class BDV23R19DFocusedRegressionQA
    {
        public static void Scan(BDOneClickQAResult result)
        {
            if (result == null)
                return;

            string root = Directory.GetParent(
                UnityEngine.Application.dataPath
            ).FullName;

            Require(result, root,
                "Assets/_Project/Scripts/Runtime/BDPlayerController.cs",
                "V23R19D_QUICKSAND_SPEED_OWNER_MISSING",
                "BD EXPLICIT QUICKSAND SPEED OWNER V23R19D",
                "quicksandMoveSpeedMultiplier",
                "SetQuicksandMovementMultiplier",
                "QuicksandMoveSpeedMultiplier",
                "moveSpeed *",
                "boostMoveSpeedMultiplier *",
                "quicksandMoveSpeedMultiplier"
            );

            Forbid(result, root,
                "Assets/_Project/Scripts/Runtime/BDPlayerController.cs",
                "V23R19D_PLAYER_DOUBLE_QUICKSAND_FILTER_REMAINS",
                "requestedMotion = BDQuicksandStatus.FilterMotion"
            );

            Require(result, root,
                "Assets/_Project/Scripts/Runtime/Hazards/BDQuicksandStatus.cs",
                "V23R19D_QUICKSAND_PUSH_MISSING",
                "BD EXPLICIT QUICKSAND SPEED OWNER V23R19D",
                "ApplyPlayerMovementMultiplier(MovementMultiplier)",
                "player.SetQuicksandMovementMultiplier(multiplier)",
                "ApplyPlayerMovementMultiplier(1f)"
            );

            Require(result, root,
                "Assets/_Project/Scripts/Runtime/Hazards/BDPlayerHazardRecovery.cs",
                "V23R19D_CONTROLLED_JUMP_TELEPORT_GUARD_MISSING",
                "BD CONTROLLED JUMP COMBAT CONTACT IS NOT FLOOR LOSS V23R19D",
                "playerController.IsAirborneFromControlledJump",
                "combatImpactGuardUntil = -999f"
            );

            Require(result, root,
                "Assets/_Project/Scripts/Runtime/Combat/BDPlayerAirborneAttackAnimation.cs",
                "V23R19D_AIR_BODY_DIRECTION_MISSING",
                "BD AIRBORNE BODY SUPPORTS ROTATED ATTACK V23R19E",
                "Quaternion.Euler(pitch, 0f, 0f)",
                "Vector3.forward *",
                "strikePosition"
            );

            Require(result, root,
                "Assets/_Project/Scripts/Runtime/BDMeleeSlashArcVisual.cs",
                "V23R19D_AIR_SLASH_DIRECTION_MISSING",
                "BD LOCAL-Z AIRBORNE LINE ROTATION V23R19M",
                "Quaternion.AngleAxis(90f, Vector3.forward)",
                "verticalPlane: false",
                "float arcDegrees = isHeavy ? 92f : 68f"
            );

            Require(result, root,
                "Assets/_Project/Scripts/Runtime/UI/BDMainMenuFlow.cs",
                "V23R19D_ABANDON_CONFIRMATION_MISSING",
                "AbandonConfirm",
                "BD ABANDON RUN CONFIRMATION V23R19D",
                "ABANDON THIS RUN?",
                "YES, ABANDON RUN",
                "CANCEL"
            );

            Require(result, root,
                "Assets/_Project/Scripts/Runtime/RunPresentation/BDRunPresentationCoordinator.cs",
                "V23R19D_CURRENT_RIDER_BINDING_MISSING",
                "BD AUTHORITATIVE MOUNTED INTRO BINDING V23R19E",
                "BeginMountedRunIntro",
                "ResolveCurrentPlayerTransform(horseController)",
                "horseController.Rider",
                "CompleteMountedRunIntro"
            );

            Require(result, root,
                "ProjectGuide/Status/CURRENT.md",
                "V23R19D_STATUS_MISSING",
                "C01/C03/C10/C11.RUNTIME.V23R19D",
                "V23R19D acceptance gate"
            );

            Require(result, root,
                "ProjectGuide/QA/QA_CHECKLIST.md",
                "V23R19D_QA_CONTRACT_MISSING",
                "Active V23R19D quicksand, airborne, combat-contact, and abandon gate",
                "Abandon replay"
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

                Add(result, code, "Missing V23R19D contract: " + token);
            }
        }

        private static void Forbid(
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

                Add(result, code, "Obsolete V23R19D behavior remains: " + token);
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
