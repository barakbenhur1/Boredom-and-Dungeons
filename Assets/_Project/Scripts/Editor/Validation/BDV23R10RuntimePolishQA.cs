#if UNITY_EDITOR
using System;
using System.IO;

namespace BoredomAndDungeons.EditorTools.Validation
{
    internal static class BDV23R10RuntimePolishQA
    {
        public static void Scan(BDOneClickQAResult result)
        {
            if (result == null) return;
            string root = Directory.GetParent(UnityEngine.Application.dataPath).FullName;

            Require(result, root, "Assets/_Project/Scripts/Editor/Validation/BDOneClickQAWindow.cs", "V23R10_QA_NOT_INTEGRATED", "BDV23R10RuntimePolishQA.Scan(result)");
            Require(result, root, "ART_DIRECTION.md", "V23R10_ROOT_ART_DIRECTION_MISSING", "Canonical root source", "Game Boy menu shell", "65% colorful wonder");
            Require(result, root, "Assets/_Project/Scripts/Runtime/UI/BDGameplayUiVisibility.cs", "V23R10_UI_VISIBILITY_OWNER_MISSING", "BD SINGLE GAMEPLAY UI VISIBILITY OWNER V23R10", "IsGameplayHudVisible", "IsTargetingVisible");
            Require(result, root, "Assets/_Project/Scripts/Runtime/UI/BDGameBoyMenuShell.cs", "V23R10_GAME_BOY_MENU_SHELL_MISSING", "BD GAME BOY MENU SHELL AND UI OWNERSHIP V23R10", "TRUE VICTORY LINK ACTIVE", "B&D POCKET ADVENTURE");
            Require(result, root, "Assets/_Project/Scripts/Runtime/Combat/BDPlayerGrapplingHook.cs", "V23R10_HOOK_SAFE_RELEASE_MISSING", "BD SAFE HOOK RELEASE BEFORE CONTACT V23R10", "ResolveSafePullStopDistance", "BDGrapplingHookPullState", "0.55f");
            Require(result, root, "Assets/_Project/Scripts/Runtime/BDPlayerCombat.cs", "V23R10_HOOK_OR_HIGHLIGHT_TUNING_MISSING", "grapplingHookRange = 13.5f", "grapplingHookHitRadius = 0.52f", "grapplingHookPullStopDistance = 2.35f", "BD PRIMARY LOOKED-AT RANGED TARGET V23R10");
            Require(result, root, "Assets/_Project/Scripts/Runtime/Combat/BDParrySystem.cs", "V23R10_PARRY_PRESENTATION_MISSING", "BD PROFESSIONAL PARRY ANTICIPATION FREEZE RECOVERY V23R10", "ParryPhase.Anticipation", "ParryPhase.Recovery", "PlayParryCue", "PlayParryRelease");
            Require(result, root, "Assets/_Project/Scripts/Runtime/Combat/ParryVisuals/BDParryTimeRingVisual.cs", "V23R10_PARRY_RING_FOLLOW_MISSING", "root.transform.SetParent(target, false)", "transform.localPosition = Vector3.up * 0.045f", "releaseFade");
            Require(result, root, "Assets/_Project/Scripts/Runtime/BDEnemyMotionStabilizer.cs", "V23R10_ENEMY_STABILIZER_MISSING", "BD SINGLE ENEMY MOTION STABILITY OWNER V23R10", "RecoverIfFloating", "minimumFrameAllowance");
            Require(result, root, "Assets/_Project/Scripts/Runtime/BDEnemyBootstrap.cs", "V23R10_ENEMY_STABILIZER_NOT_INSTALLED", "BDEnemyMotionStabilizer");
            Require(result, root, "Assets/_Project/Scripts/Runtime/BDJumperEnemy.cs", "V23R10_JUMPER_STABILITY_MISSING", "IsPerformingAuthoredJump", "landingPlayerClearance", "finalCorrection");
            Require(result, root, "DOCUMENTATION_INDEX.md", "V23R10_DOCUMENTATION_MAP_MISSING", "ART_DIRECTION.md", "GAME_BOY_MENU_AND_UI_OWNERSHIP_V1.md", "PARRY_SUCCESS_PRESENTATION_V1.md", "ENEMY_MOTION_STABILITY_V1.md");
            Require(result, root, "PROJECT_STATUS.md", "V23R10_STATUS_MISSING", "C01/C03/C05/C11/C12.RUNTIME.V23R10", "V23R10 automated verification pending", "Saved feature resume point");
        }

        private static string Read(string root, string relative){string p=Path.Combine(root,relative);return File.Exists(p)?File.ReadAllText(p):string.Empty;}
        private static void Require(BDOneClickQAResult result,string root,string relative,string code,params string[] tokens){string source=Read(root,relative);if(string.IsNullOrEmpty(source)){Add(result,code,"Missing required file: "+relative);return;}foreach(string token in tokens)if(source.IndexOf(token,StringComparison.Ordinal)<0)Add(result,code,"Missing V23R10 contract: "+token);}
        private static void Add(BDOneClickQAResult result,string code,string message){result.findings.Add(new BDOneClickQAFinding(BDOneClickQASeverity.Blocker,code,string.Empty,string.Empty,message));}
    }
}
#endif
