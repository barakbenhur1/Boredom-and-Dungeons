#if UNITY_EDITOR
using System;
using System.IO;

namespace BoredomAndDungeons.EditorTools.Validation
{
    internal static class BDHorseHudMinimapQA
    {
        public static void Scan(BDOneClickQAResult result)
        {
            if (result == null)
                return;

            string root = Directory.GetParent(
                UnityEngine.Application.dataPath
            ).FullName;

            Require(
                result,
                root,
                "Assets/_Project/Scripts/Runtime/BDHorseController.cs",
                "HORSE_INJURY_HEALING_CONTRACT_MISSING",
                "BD DISCRETE HORSE INJURY SPEED BANDS V2",
                "speedPenaltyPerThirtyPercentMissing = 0.08f",
                "ResolveHealthMovementSpeedMultiplier",
                "Injury affects only horizontal travel",
                "healPerSecond = 14f",
                "BDHorseHealingPresentation",
                "public bool IsHealing"
            );
            Forbid(
                result,
                root,
                "Assets/_Project/Scripts/Runtime/BDHorseController.cs",
                "HORSE_CONTINUOUS_INJURY_CURVE_REMAINS",
                "Mathf.Lerp(1f, 0.45f, health.Injury01)"
            );
            Require(
                result,
                root,
                "Assets/_Project/Scripts/Runtime/Horse/BDHorseHealingPresentation.cs",
                "HORSE_HEALING_PRESENTATION_MISSING",
                "BD Horse Healing Ground Ring",
                "BD Horse Healing Particles",
                "BeginHealing",
                "NotifyHealApplied",
                "EndHealing"
            );
            Require(
                result,
                root,
                "Assets/_Project/Scripts/Runtime/Horse/BDHorseContextActionPrompts.cs",
                "HORSE_BOTTOM_CONTEXT_PROMPT_MISSING",
                "BD HORSE BOTTOM CONTEXT STRIP V2",
                "bottomMargin",
                "HOLD HEAL",
                "No icon, label, health bar, or interaction card is drawn",
                "EaseOutCubic",
                "Fade state is updated once per frame in LateUpdate"
            );
            Forbid(
                result,
                root,
                "Assets/_Project/Scripts/Runtime/Horse/BDHorseContextActionPrompts.cs",
                "HORSE_WORLD_PROMPT_REMAINS",
                "WorldToScreenPoint",
                "worldHeight",
                "screenOffset"
            );
            Require(
                result,
                root,
                "Assets/_Project/Scripts/Runtime/UI/BDGameplayHudPresentationDirector.cs",
                "CONTEXTUAL_HUD_DIRECTOR_MISSING",
                "PlayerHealthAlpha",
                "HorseHealthAlpha",
                "AmmoAlpha",
                "IsRangedInputHeld",
                "IsPlayerStationary",
                "IsPlayerNearHorse",
                "Riding the horse does not count as standing beside it"
            );
            Require(
                result,
                root,
                "Assets/_Project/Scripts/Runtime/BDGameHud.cs",
                "CONTEXTUAL_HUD_RENDERER_MISSING",
                "BD CONTEXTUAL GAMEPLAY HUD PRESENTATION V2",
                "DrawPlayerBar(playerAlpha)",
                "DrawHorseBar(horseAlpha)",
                "DrawAmmoWidget(ammoAlpha)",
                "WithWidgetAlpha"
            );
            Require(
                result,
                root,
                "Assets/_Project/Scripts/Runtime/BDPlayerCombat.cs",
                "RANGED_HUD_INPUT_STATE_MISSING",
                "BD CONTEXTUAL AMMO HUD INPUT STATE V2",
                "public bool IsRangedInputHeld",
                "rangedInputHeld = ReadRangedAttackHeld()"
            );
            Require(
                result,
                root,
                "Assets/_Project/Scripts/Runtime/BDMazeMinimap.cs",
                "MINIMAP_MARKER_LANGUAGE_MISSING",
                "BD MINIMAP COMBATANT MARKERS V2",
                "DrawRasterCircleMarker",
                "DrawRasterHexagonMarker",
                "IsWorldPositionInDiscoveredRoom",
                "BDCombatantRank.MiniBoss",
                "BDCombatantRank.Boss",
                "stationaryMinimapAlpha = 0.38f",
                "WakeMinimap"
            );
            Require(
                result,
                root,
                "ProjectGuide/Features/Horse/" +
                "HORSE_INJURY_HEALING_AND_MINIMAP_MARKERS_V1.md",
                "HORSE_FEATURE_DOCUMENT_MISSING",
                "100% / 92% / 84% / 76%",
                "large red hexagon",
                "future shop and NPC markers"
            );
            Require(
                result,
                root,
                "ProjectGuide/Features/UI/" +
                "CONTEXTUAL_GAMEPLAY_HUD_VISIBILITY_V1.md",
                "CONTEXTUAL_HUD_DOCUMENT_MISSING",
                "Player health",
                "Ranged ammunition",
                "38%"
            );
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
                if (source.IndexOf(token, StringComparison.Ordinal) < 0)
                {
                    Add(
                        result,
                        code,
                        "Missing required contract token: " + token
                    );
                }
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
                if (source.IndexOf(token, StringComparison.Ordinal) >= 0)
                {
                    Add(
                        result,
                        code,
                        "Obsolete contract remains: " + token
                    );
                }
            }
        }

        private static string Read(string root, string relative)
        {
            string path = Path.Combine(root, relative);
            return File.Exists(path)
                ? File.ReadAllText(path)
                : string.Empty;
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
