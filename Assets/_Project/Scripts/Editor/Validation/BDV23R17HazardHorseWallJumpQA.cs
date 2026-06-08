#if UNITY_EDITOR
using System;
using System.IO;

namespace BoredomAndDungeons.EditorTools.Validation
{
    internal static class BDV23R17HazardHorseWallJumpQA
    {
        public static void Scan(BDOneClickQAResult result)
        {
            if (result == null)
                return;

            string root = Directory.GetParent(UnityEngine.Application.dataPath).FullName;
            Require(result, root,
                "Assets/_Project/Scripts/Runtime/BDPlayerController.cs",
                "V23R17_WALL_JUMP_MISSING",
                "Wall Jump",
                "TryJumpOrWallJump",
                "OnControllerColliderHit",
                "wallJumpHorizontalVelocity",
                "JumpSequence");
            Require(result, root,
                "Assets/_Project/Scripts/Runtime/Hazards/BDQuicksandStatus.cs",
                "V23R17_QUICKSAND_MISSING",
                "BD EXACT PLAYER QUICKSAND ESCAPE/DAMAGE V23R17",
                "playerDamagePerSecond = 2f",
                "jumpEscapeAmount",
                "player.IsDodging",
                "halfBodyFailureThreshold = 0.50f",
                "TickEnemyQuicksand");
            Require(result, root,
                "Assets/_Project/Scripts/Runtime/Hazards/BDEnemyHazardNavigation.cs",
                "V23R17_ENEMY_HAZARD_NAV_MISSING",
                "BD ENEMY HAZARD INTENT/FORCED ENTRY OWNER V23R17",
                "FilterBrainMotion",
                "IsSmallRegularEnemy",
                "NotifyForcedHazardEntry");
            Require(result, root,
                "Assets/_Project/Scripts/Runtime/Horse/BDHorseImpactAttack.cs",
                "V23R17_HORSE_IMPACT_MISSING",
                "BD MOUNTED SMALL-ENEMY IMPACT DAMAGE V23R17",
                "minimumDamage = 4f",
                "maximumDamage = 10f",
                "directness",
                "knockDirection");
            Require(result, root,
                "Assets/_Project/Scripts/Runtime/RunPresentation/BDRunPresentationCoordinator.cs",
                "V23R17_INTRO_DIRECTION_MISSING",
                "BD CLEAR-DIRECTION MOUNTED INTRO TURN V23R17",
                "ResolveClearMountedIntroDirection",
                "Physics.CapsuleCastAll");
            Require(result, root,
                "Assets/_Project/Scripts/Runtime/Combat/BDPlayerAirborneAttackAnimation.cs",
                "V23R17_AIRBORNE_ANIMATION_MISSING",
                "BD DISTINCT AIRBORNE LIGHT/HEAVY BODY ANIMATION V23R17",
                "lightPitchDegrees",
                "heavyPitchDegrees");
        }

        private static void Require(
            BDOneClickQAResult result,
            string root,
            string relative,
            string code,
            params string[] tokens)
        {
            string path = Path.Combine(root, relative);
            string source = File.Exists(path) ? File.ReadAllText(path) : string.Empty;
            if (string.IsNullOrEmpty(source))
            {
                Add(result, code, "Missing required file: " + relative);
                return;
            }
            foreach (string token in tokens)
            {
                if (source.IndexOf(token, StringComparison.Ordinal) < 0)
                    Add(result, code, "Missing V23R17 contract: " + token);
            }
        }

        private static void Add(BDOneClickQAResult result, string code, string message)
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
