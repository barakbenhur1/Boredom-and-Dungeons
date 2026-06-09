#if UNITY_EDITOR
using System;
using System.IO;

namespace BoredomAndDungeons.EditorTools.Validation
{
    internal static class BDV23R11BombAirborneAudioQA
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
                "V23R11_QA_INTEGRATION_MISSING",
                "BDV23R11BombAirborneAudioQA.Scan(result)"
            );

            Require(result, root,
                "ProjectGuide/Product/AUDIO_DIRECTION.md",
                "V23R11_AUDIO_DIRECTION_MISSING",
                "Exploration / no active combat",
                "Mini-boss combat",
                "Boss combat",
                "Mother boss — child-song transformation",
                "MotherTick",
                "-1 dBTP",
                "C12.42"
            );

            Require(result, root,
                "ProjectGuide/Product/AUDIO_DIRECTION.md",
                "V23R11_AUDIO_MIRROR_MISSING",
                "Complete sound-event coverage",
                "MotherPhase4",
                "AudioMixer"
            );

            Require(result, root,
                "Assets/_Project/Scripts/Runtime/BDBombHazard.cs",
                "V23R11_BOMB_RUNTIME_MISSING",
                "BD BOMB EXPLOSION + ENEMY FRIENDLY FIRE V23R11",
                "BDBombExplosionVisual.Spawn",
                "PlayBombExplosion",
                "HashSet<BDHealth>",
                "ApplyEnemyExplosionReaction",
                "IsOwnerHealth"
            );

            Require(result, root,
                "Assets/_Project/Scripts/Runtime/BDBombExplosionVisual.cs",
                "V23R11_BOMB_VISUAL_MISSING",
                "BD_Bomb_Explosion_V23R11",
                "Bomb_Explosion_Ground_Ring",
                "Bomb_Explosion_Spark_"
            );

            Require(result, root,
                "Assets/_Project/Scripts/Runtime/BDTrapLayerEnemy.cs",
                "V23R11_BOMB_OWNER_MISSING",
                "hazard.ConfigureOwner(transform)"
            );

            Require(result, root,
                "Assets/_Project/Scripts/Runtime/BDGameFeelAudio.cs",
                "V23R11_BOMB_AUDIO_MISSING",
                "PlayBombExplosion",
                "BD_BombExplosion"
            );

            Require(result, root,
                "Assets/_Project/Scripts/Runtime/BDMeleeSlashArcVisual.cs",
                "V23R11_VERTICAL_MESH_VISUAL_MISSING",
                "BD TRUE VERTICAL AIRBORNE MELEE ARC V23R11",
                "SpawnVertical",
                "BD_Vertical_Melee_Slash_Arc_Mesh",
                "verticalPlane"
            );

            Require(result, root,
                "Assets/_Project/Scripts/Runtime/Combat/BDPlayerMeleeEnhancer.cs",
                "V23R11_COMMITTED_AIRBORNE_MISSING",
                "BD COMMITTED AIRBORNE ATTACK PRESENTATION V23R11",
                "PrepareCommittedAttackDamage",
                "out bool airbornePresentation",
                "ShouldSpawnAirborneSlashVisual"
            );

            Require(result, root,
                "Assets/_Project/Scripts/Runtime/BDPlayerCombat.cs",
                "V23R11_COMBAT_COMMIT_HOOK_MISSING",
                "BD COMMITTED AIRBORNE ATTACK PRESENTATION V23R11",
                "UsesLightHoldInput",
                "MeleeAttackRange",
                "PrepareCommittedAttackDamage"
            );

            // BD EXPLICIT AIRBORNE PRESENTATION OWNER QA V23R19K
            // The enhancer resolves identity/body animation. The committed combat
            // owner consumes that identity and spawns exactly one selected arc.
            Require(result, root,
                "Assets/_Project/Scripts/Runtime/BDPlayerCombat.cs",
                "V23R11_COMBAT_COMMIT_HOOK_MISSING",
                "BD EXPLICIT COMMITTED AIRBORNE VISUAL OWNER V23R19K",
                "out airbornePresentation",
                "SpawnCommittedMeleeSlashArc",
                "BDMeleeSlashArcVisual.SpawnVertical"
            );

            Forbid(result, root,
                "Assets/_Project/Scripts/Runtime/Combat/BDPlayerMeleeEnhancer.cs",
                "V23R11_LEGACY_LINE_AIRBORNE_CALL_REMAINS",
                "BDLandingAttackVisual.Spawn("
            );

            Require(result, root,
                "ProjectGuide/Features/Combat/BOMB_EXPLOSION_AND_FRIENDLY_FIRE_V1.md",
                "V23R11_BOMB_DESIGN_MISSING",
                "visibly explode",
                "friendly-fire",
                "multiple colliders",
                "bomb owner"
            );

            Require(result, root,
                "ProjectGuide/Features/Combat/AIRBORNE_VERTICAL_ATTACK_PRESENTATION_V1.md",
                "V23R11_AIRBORNE_DESIGN_MISSING",
                "V23R11 committed-animation repair",
                "IMPLEMENTED in V23R11",
                "vertical high-to-low"
            );

            Require(result, root,
                "ProjectGuide/Status/CURRENT.md",
                "V23R11_STATUS_MISSING",
                "C01/C03/C05/C12.RUNTIME.V23R11",
                "Music and audio direction — COMPLETED AS DOCUMENTATION",
                "Airborne light/heavy animation repair — IMPLEMENTED",
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
                Add(result, code, "Missing V23R11 contract: " + token);
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
                Add(result, code, "Obsolete V23R11 token remains: " + token);
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
