#if UNITY_EDITOR
using System;
using System.IO;

namespace BoredomAndDungeons.EditorTools.Validation
{
    internal static class BDV23R19TraversalQuicksandAirborneQA
    {
        public static void Scan(BDOneClickQAResult result)
        {
            if (result == null) return;
            string root = Directory.GetParent(UnityEngine.Application.dataPath).FullName;
            Require(result, root, "Assets/_Project/Scripts/Runtime/Hazards/BDQuicksandStatus.cs", "V23R19_QUICKSAND_OWNER_MISSING",
                "BD QUICKSAND OWNS NONLETHAL JUMP/GROUND RECOVERY V23R19", "IsActorUnderQuicksandControl", "BlocksGenericGroundRecovery");
            Require(result, root, "Assets/_Project/Scripts/Runtime/Hazards/BDPlayerHazardRecovery.cs", "V23R19_QUICKSAND_TELEPORT_GUARD_MISSING",
                "BD QUICKSAND NONLETHAL JUMP MUST NOT TELEPORT V23R19", "CheckGroundExit()", "CheckCombatGroundingGuard()");
            Require(result, root, "Assets/_Project/Scripts/Runtime/BDPlayerController.cs", "V23R19_TRAVERSAL_TUNING_MISSING",
                "BD SLIGHTLY LONGER CONTROLLED JUMP TRAVEL V23R19", "jumpTravelMultiplier = 1.10f", "dashDistance = 3.35f",
                "wallJumpHorizontalSpeed = 8.2f", "wallJumpHorizontalDuration = 0.48f",
                "BD ANY SOLID VERTICAL WALL-JUMP SURFACE V23R19", "Physics.SphereCastNonAlloc", "QueryTriggerInteraction.Ignore");
            Require(result, root, "Assets/_Project/Scripts/Runtime/Combat/BDPlayerMeleeEnhancer.cs", "V23R19_AIRBORNE_IDENTITY_MISSING",
                "BD EXPLICIT AIRBORNE ATTACK PRESENTATION BRANCH V23R19", "out bool airbornePresentation", "IsAirborneFromControlledJump");
            Require(result, root, "Assets/_Project/Scripts/Runtime/BDPlayerCombat.cs", "V23R19_AIRBORNE_VISUAL_BRANCH_MISSING",
                "BD EXPLICIT COMMITTED AIRBORNE VISUAL OWNER V23R19K", "out airbornePresentation", "SpawnCommittedMeleeSlashArc", "BDMeleeSlashArcVisual.SpawnVertical");
            Require(result, root, "Assets/_Project/Scenes/02_CleanCore_MazePrototype.unity", "V23R19_SCENE_TUNING_MISSING",
                "jumpTravelMultiplier: 1.1", "wallJumpHorizontalSpeed: 8.2", "wallJumpHorizontalDuration: 0.48", "dashDistance: 3.35");
            Require(result, root, "PROJECT_STATUS.md", "V23R19_STATUS_MISSING", "C01/C03/C10.RUNTIME.V23R19", "UNITY VERIFICATION REQUIRED");
        }
        private static void Require(BDOneClickQAResult result,string root,string relative,string code,params string[] tokens)
        {
            string path=Path.Combine(root,relative); string source=File.Exists(path)?File.ReadAllText(path):string.Empty;
            if(string.IsNullOrEmpty(source)){Add(result,code,"Missing required file: "+relative);return;}
            foreach(string token in tokens) if(source.IndexOf(token,StringComparison.Ordinal)<0) Add(result,code,"Missing V23R19 contract: "+token);
        }
        private static void Add(BDOneClickQAResult result,string code,string message)
        {
            result.findings.Add(new BDOneClickQAFinding(BDOneClickQASeverity.Blocker,code,string.Empty,string.Empty,message));
        }
    }
}
#endif
