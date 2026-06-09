#if UNITY_EDITOR
using System;
using System.IO;

namespace BoredomAndDungeons.EditorTools.Validation
{
    internal static class BDV23R13AudioQuicksandOutlineQA
    {
        public static void Scan(BDOneClickQAResult result)
        {
            if (result == null)
                return;

            string root = Directory.GetParent(
                UnityEngine.Application.dataPath
            ).FullName;

            Require(result, root,
                "Assets/_Project/Scripts/Editor/Validation/BDOneClickQAWindow.cs",
                "V23R13_QA_INTEGRATION_MISSING",
                "BDV23R13AudioQuicksandOutlineQA.Scan(result)",
                "BDHazardType.Quicksand",
                "quicksand != 1"
            );

            Require(result, root, "ProjectGuide/Product/AUDIO_DIRECTION.md",
                "V23R13_AUDIO_COVERAGE_MISSING",
                "Complete sound-event coverage",
                "Player combat and weapons",
                "Movement and physical actions",
                "Horse and mounted play",
                "UI, Game Boy shell, menus, intro, and narrative",
                "minimum coverage model, not an exclusive list"
            );
            Require(result, root,
                "ProjectGuide/Product/AUDIO_DIRECTION.md",
                "V23R13_AUDIO_MIRROR_MISSING",
                "Complete sound-event coverage",
                "quicksand entry",
                "button hover/focus"
            );

            Require(result, root,
                "Assets/_Project/Scripts/Runtime/Hazards/BDHazardType.cs",
                "V23R13_QUICKSAND_TYPE_MISSING",
                "Quicksand = 2"
            );
            Require(result, root,
                "Assets/_Project/Scripts/Runtime/Hazards/BDQuicksandStatus.cs",
                "V23R13_QUICKSAND_RUNTIME_MISSING",
                "public sealed class BDQuicksandStatus",
                "BD EXACT PLAYER QUICKSAND ESCAPE/DAMAGE V23R17",
                "MovementMultiplier",
                "TickPlayerQuicksand",
                "TriggerPlayerHalfBodyFailure",
                "TriggerHorseFullSink",
                "TriggerEnemyFullSink"
            );
            Require(result, root,
                "Assets/_Project/Scripts/Runtime/Hazards/BDHazardVolume.cs",
                "V23R13_QUICKSAND_VOLUME_MISSING",
                "BD PLAYABLE QUICKSAND PROGRESSIVE SINK V23R17",
                "BDQuicksandStatus.TouchActor",
                "BDHazardType.Quicksand"
            );
            Require(result, root,
                "Assets/_Project/Scripts/Runtime/BDPlayerController.cs",
                "V23R13_PLAYER_QUICKSAND_FILTER_MISSING",
                "BD EXPLICIT QUICKSAND SPEED OWNER V23R19D",
                "SetQuicksandMovementMultiplier",
                "quicksandMoveSpeedMultiplier"
            );
            Require(result, root,
                "Assets/_Project/Scripts/Runtime/BDHorseController.cs",
                "V23R13_HORSE_QUICKSAND_FILTER_MISSING",
                "BDQuicksandStatus.FilterMotion"
            );
            Require(result, root,
                "Assets/_Project/Scripts/Runtime/Hazards/BDPlayerHazardRecovery.cs",
                "V23R13_QUICKSAND_RECOVERY_MISSING",
                "BDHazardType.Quicksand",
                "HazardRecovered?.Invoke"
            );
            Require(result, root,
                "Assets/_Project/Scripts/Editor/Validation/BDPrototypeHazardSceneInstaller.cs",
                "V23R13_QUICKSAND_SCENE_MISSING",
                "QuicksandName",
                "Hazard_Quicksand",
                "KEEP MOVING TO ESCAPE"
            );

            Require(result, root,
                "Assets/_Project/Scripts/Runtime/Combat/BDCombatTargetHighlighter.cs",
                "V23R13_SILHOUETTE_SELECTION_MISSING",
                "BD CONSTANT-SIZE SILHOUETTE TARGET OUTLINE V23R13",
                "BDTargetOutlineVisual",
                "outlineWidthPixels"
            );
            Forbid(result, root,
                "Assets/_Project/Scripts/Runtime/Combat/BDCombatTargetHighlighter.cs",
                "V23R13_RECTANGULAR_FRAME_REMAINS",
                "private void OnGUI(",
                "DrawCornerFrame",
                "minimumFrameSizePixels",
                "pulseAmount",
                "framePaddingPixels",
                "originHeight"
            );
            Require(result, root,
                "Assets/_Project/Scripts/Runtime/Combat/BDTargetOutlineVisual.cs",
                "V23R13_OUTLINE_VISUAL_MISSING",
                "BDTargetOutlineVisual",
                "CreateSkinnedShell",
                "CreateMeshShell",
                "_OutlinePixels"
            );
            Require(result, root,
                "Assets/_Project/Shaders/BDTargetSilhouetteOutline.shader",
                "V23R13_OUTLINE_SHADER_MISSING",
                "TargetSilhouetteOutline",
                "Cull Front",
                "_ScreenParams",
                "_OutlinePixels"
            );

            Forbid(result, root,
                "Assets/_Project/Scripts/Runtime/Combat/BDParrySystem.cs",
                "V23R13_PARRY_WARNING_FIELD_REMAINS",
                "worldFrozen"
            );
            Forbid(result, root,
                "Assets/_Project/Scripts/Runtime/EnemyPlacementSafety/BDEnemyPlacementSafety.cs",
                "V23R13_PLACEMENT_WARNING_FIELDS_REMAIN",
                "landingSearchRadius",
                "descendingThresholdPerFrame"
            );

            Require(result, root,
                "ProjectGuide/Features/Runtime/V23R13_AUDIO_QUICKSAND_TARGET_OUTLINE_REPAIR.md",
                "V23R13_DESIGN_MISSING",
                "Quicksand implementation — DONE IN CODE",
                "Target outline implementation — DONE IN CODE",
                "Warning cleanup — DONE IN CODE"
            );
            Require(result, root, "ProjectGuide/Status/CURRENT.md",
                "V23R13_STATUS_MISSING",
                "C01/C03/C10/C11/C12.RUNTIME.V23R13",
                "Saved feature resume point"
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
                Add(result, code, "Missing V23R13 contract: " + token);
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
                Add(result, code, "Obsolete V23R13 token remains: " + token);
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
