#if UNITY_EDITOR
using System;
using System.IO;
using UnityEngine;

namespace BoredomAndDungeons.EditorTools.Validation
{
    internal static class BDVisualEntryMinimapMountedCombatQA
    {
        // BD VISIBLE CIRCLE + AUTHORED PORTAL LIGHT QA V14
        // BD VERSION-AGNOSTIC CURRENT PROJECT STATUS QA V21R1
        public static void Scan(BDOneClickQAResult result)
        {
            if (result == null)
                return;

            string root = ResolveProjectRoot();

            Require(
                result,
                root,
                "Assets/_Project/Scripts/Runtime/UI/BDBBHBootIntro.cs",
                "BBH_INTRO_V14_CONTRACT_MISSING",
                "BD BBH SINGLE-LETTER SEQUENCE + VISIBLE GROWING FILLED CIRCLE V14",
                "BD BBH CIRCLE ALPHA SDF FIX V14",
                "Mathf.InverseLerp(",
                "ScaleMode.StretchToFill",
                "DrawGrowingFilledCircleBehindText",
                "CircleFullHoldDuration = 0.50f"
            );

            Require(
                result,
                root,
                "Assets/_Project/Scripts/Runtime/RunPresentation/BDPortalSurfaceEffect.cs",
                "AUTHORED_PORTAL_LIGHT_V14_MISSING",
                "BD AUTHORED OPENING VISIBLE LIGHT COVER V14",
                "BD PORTAL MASK NORMALIZED ALPHA FIX V14",
                "PortalLightCore_EffectOnly_NotADoor",
                "PortalLightHalo_EffectOnly_NotADoor",
                "Shader.Find(\"Sprites/Default\")",
                "LightType.Point"
            );

            Require(
                result,
                root,
                "Assets/_Project/Scripts/Runtime/RunPresentation/BDRunPresentationCoordinator.cs",
                "PORTAL_EFFECT_SELF_HEAL_V14_MISSING",
                "BD AUTHORED ENTRY SINGLE-OWNER + PORTAL SELF-HEAL V14",
                "EnsureAuthoredPortalEffects();",
                "nextPortalRepairAt",
                "portal.enabled = true"
            );

            Require(
                result,
                root,
                "Assets/_Project/Scripts/Runtime/RunPresentation/BDEntranceReturnBlocker.cs",
                "ENTRANCE_ONE_WAY_BARRIER_V13_MISSING",
                "BD AUTHORED ENTRANCE WORLD-SPACE ONE-WAY BARRIER V13",
                "ConfigureFromAuthoredEntrance",
                "barrier.enabled = blocking"
            );

            Require(
                result,
                root,
                "Assets/_Project/Scripts/Runtime/BDHealth.cs",
                "MOUNTED_DAMAGE_ROUTING_V13_MISSING",
                "BD ACTUAL COLLIDER DAMAGE ROUTING V13",
                "bridgedHorseHealth.ApplyDamage(amount)",
                "NotifyMountedRiderHitForBuck",
                "RegisterMountedRiderHitForBuck"
            );

            Require(
                result,
                root,
                "Assets/_Project/Scripts/Runtime/BDHorseHealth.cs",
                "MOUNTED_BUCK_SHARED_HIT_COUNT_MISSING",
                "EnsureMountedDamageBridge",
                "RegisterMountedRiderHitForBuck",
                "RegisterRecentHit()"
            );

            Require(
                result,
                root,
                "Assets/_Project/Scripts/Runtime/BDHorseController.cs",
                "MOUNTED_RIDER_HURTBOX_V13_MISSING",
                "playerCharacterController.enabled = true"
            );

            ValidateCurrentProjectStatus(result, root);
        }


        private static void ValidateCurrentProjectStatus(
            BDOneClickQAResult result,
            string root)
        {
            const string relative = "PROJECT_STATUS.md";
            string absolute = Path.Combine(root, relative);
            if (!File.Exists(absolute))
            {
                Add(
                    result,
                    "PROJECT_STATUS_CURRENT_ACTIVE_WORK_MISSING",
                    relative,
                    "Required current project-status document is missing."
                );
                return;
            }

            string source = File.ReadAllText(absolute);
            string[] stableTokens =
            {
                "Current development snapshot",
                "Active work:",
                "# Active blocking work",
                ".RUNTIME.V",
                "Saved feature resume point",
                "C12.42"
            };

            foreach (string token in stableTokens)
            {
                if (source.IndexOf(token, StringComparison.Ordinal) >= 0)
                    continue;

                Add(
                    result,
                    "PROJECT_STATUS_CURRENT_ACTIVE_WORK_MISSING",
                    relative,
                    "Missing current project-status contract: " + token
                );
            }

            if (source.IndexOf(
                    "Active work: C03/C11/C12.RUNTIME.V",
                    StringComparison.Ordinal) >= 0 ||
                source.IndexOf(
                    "Active blocking work — C03/C11/C12.RUNTIME.V",
                    StringComparison.Ordinal) >= 0)
            {
                Add(
                    result,
                    "PROJECT_STATUS_OBSOLETE_FROZEN_CATEGORY_CONTRACT",
                    relative,
                    "Current-status QA/documentation still contains the obsolete frozen C03/C11/C12 category contract."
                );
            }
        }

        private static void Require(
            BDOneClickQAResult result,
            string root,
            string relative,
            string code,
            params string[] tokens)
        {
            string absolute = Path.Combine(root, relative);
            if (!File.Exists(absolute))
            {
                Add(result, code, relative, "Required V13 file is missing.");
                return;
            }

            string source = File.ReadAllText(absolute);
            foreach (string token in tokens)
            {
                if (source.IndexOf(token, StringComparison.Ordinal) >= 0)
                    continue;
                Add(result, code, relative, "Missing V13 contract: " + token);
            }
        }

        private static string ResolveProjectRoot()
        {
            DirectoryInfo assets = new DirectoryInfo(Application.dataPath);
            return assets.Parent != null ? assets.Parent.FullName : Application.dataPath;
        }

        private static void Add(
            BDOneClickQAResult result,
            string code,
            string assetPath,
            string message)
        {
            result.findings.Add(
                new BDOneClickQAFinding(
                    BDOneClickQASeverity.Blocker,
                    code,
                    assetPath,
                    string.Empty,
                    message
                )
            );
        }
    }
}
#endif
