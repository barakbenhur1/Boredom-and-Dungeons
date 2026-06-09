#if UNITY_EDITOR
using System;
using System.IO;

namespace BoredomAndDungeons.EditorTools.Validation
{
    internal static class BDV23R9HorseArtDirectionQA
    {
        public static void Scan(BDOneClickQAResult result)
        {
            if (result == null) return;
            string root = Directory.GetParent(UnityEngine.Application.dataPath).FullName;
            Require(result, root, "Assets/_Project/Scripts/Runtime/BDHorseController.cs", "V23R9_MOUNTED_HEALING_POLICY_MISSING", "BD HEALING ON FOOT ONLY V23R9", "EndHealingSessionIfNeeded", "ClearHealingReleaseHold");
            Forbid(result, root, "Assets/_Project/Scripts/Runtime/BDHorseController.cs", "V23R9_OBSOLETE_MOUNTED_HEALING_REMAINS", "BD MOUNTED STATIONARY HEAL V23R8");
            Require(result, root, "Assets/_Project/Scripts/Runtime/Horse/BDHorseContextActionPrompts.cs", "V23R9_HORSE_PROMPT_STATE_MATRIX_MISSING", "BD HORSE PROMPT STATE MATRIX V23R9", "DISMOUNT", "Healing is strictly an on-foot interaction");
            Require(result, root, "Assets/_Project/Scripts/Runtime/Horse/BDHorseExhaustedFollowAndPetInteraction.cs", "V23R9_MOUNTED_PET_CONTRACT_MISSING", "BD MOUNTED STATIONARY PET WITHOUT PROMPT V23R9", "mountedPetInteraction", "mounted stationary rider pets horse", "horseController.IsMountedStationary");
            Require(result, root, "ProjectGuide/Features/Horse/HORSE_CONTEXT_ACTION_PROMPTS_V1.md", "V23R9_HORSE_PROMPT_DESIGN_MISSING", "State matrix", "Mounted and moving", "Healing is available only on foot");
            Require(result, root, "ProjectGuide/Product/ART_DIRECTION.md", "V23R9_ROOT_ART_DIRECTION_DOCUMENT_MISSING", "Canonical root source", "65% colorful wonder", "35% mystery and danger", "Game Boy menu shell", "True victory transformation", "fantasy display font", "Game Boy-inspired game icons", "smooth elegance with physical weight", "Mounted and stationary", "Mounted and moving");
            Require(result, root, "ProjectGuide/Product/ART_DIRECTION.md", "V23R9_ART_DIRECTION_MIRROR_MISSING", "Canonical root source", "ProjectGuide/Product/ART_DIRECTION.md");
            RequireFile(result, root, "ProjectGuide/References/Visual/BOREDOM_AND_DUNGEONS_ART_DIRECTION_REFERENCE_BOARD_V1.jpg", "V23R9_ART_REFERENCE_BOARD_MISSING");
            Require(result, root, "ProjectGuide/INDEX.md", "V23R9_DOCUMENTATION_INDEX_MISSING", "ART_DIRECTION_AND_INTERFACE_CONVENTIONS_V1.md", "BOREDOM_AND_DUNGEONS_ART_DIRECTION_REFERENCE_BOARD_V1.jpg");
            Require(result, root, "ProjectGuide/Status/CURRENT.md", "V23R9_PROJECT_STATUS_MISSING", "C04/C11/C12.RUNTIME.V23R9", "V23R8 automated QA passed", "Saved feature resume point");
        }
        private static string Read(string root,string relative){string p=Path.Combine(root,relative);return File.Exists(p)?File.ReadAllText(p):string.Empty;}
        private static void RequireFile(BDOneClickQAResult result,string root,string relative,string code){if(!File.Exists(Path.Combine(root,relative)))Add(result,code,"Missing required file: "+relative);}
        private static void Require(BDOneClickQAResult result,string root,string relative,string code,params string[] tokens){string source=Read(root,relative);if(string.IsNullOrEmpty(source)){Add(result,code,"Missing required file: "+relative);return;}foreach(string token in tokens)if(source.IndexOf(token,StringComparison.Ordinal)<0)Add(result,code,"Missing V23R9 contract: "+token);}
        private static void Forbid(BDOneClickQAResult result,string root,string relative,string code,params string[] tokens){string source=Read(root,relative);foreach(string token in tokens)if(source.IndexOf(token,StringComparison.Ordinal)>=0)Add(result,code,"Obsolete V23R9 contract remains: "+token);}
        private static void Add(BDOneClickQAResult result,string code,string message){result.findings.Add(new BDOneClickQAFinding(BDOneClickQASeverity.Blocker,code,string.Empty,string.Empty,message));}
    }
}
#endif
