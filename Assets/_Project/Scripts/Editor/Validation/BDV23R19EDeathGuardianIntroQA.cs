#if UNITY_EDITOR
using System;
using System.IO;

namespace BoredomAndDungeons.EditorTools.Validation
{
    internal static class BDV23R19EDeathGuardianIntroQA
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
                "V23R19E_MOUNTED_REPLAY_BINDING_MISSING",
                "BD FRESH-SCENE PLAYER CACHE FOR MOUNTED INTRO V23R19E",
                "BD AUTHORITATIVE MOUNTED INTRO BINDING V23R19E",
                "BeginMountedRunIntro",
                "SnapCinematicRiderToMountPoint",
                "CompleteMountedRunIntro",
                "IsValidCurrentScenePlayer"
            );

            Require(result, root,
                "Assets/_Project/Scripts/Runtime/BDHorseController.cs",
                "V23R19E_EXTERNAL_RIDER_FOLLOW_MISSING",
                "BD EXTERNAL-CONTROL RIDER FOLLOW V23R19E",
                "MoveByExternalControl",
                "PlaceRiderOnMountPoint"
            );

            Require(result, root,
                "Assets/_Project/Scripts/Runtime/BDMeleeSlashArcVisual.cs",
                "V23R19E_EXACT_ROTATED_AIR_SLASH_MISSING",
                "BD LOCAL-Z AIRBORNE LINE ROTATION V23R19M",
                "Quaternion.AngleAxis(90f, Vector3.forward)",
                "verticalPlane: false",
                "float arcDegrees = isHeavy ? 92f : 68f"
            );

            Require(result, root,
                "Assets/_Project/Scripts/Runtime/BDCharacterDeathAnimation.cs",
                "V23R19E_DEATH_ANIMATION_OWNER_MISSING",
                "BD RENDERER-BRANCH PLAYER AND ENEMY DEATH V23R19G",
                "PlayPlayerDeath",
                "PlayEnemyDeath",
                "DisableGameplayAfterDeath",
                "Time.unscaledDeltaTime"
            );

            Require(result, root,
                "Assets/_Project/Scripts/Runtime/UI/BDMainMenuFlow.cs",
                "V23R19E_PLAYER_DEATH_MENU_DELAY_MISSING",
                "BD PLAYER DEATH BEFORE MENU V23R19E",
                "PresentPlayerDeathThenMenu",
                "BDCharacterDeathAnimation.PlayPlayerDeath",
                "ShowPlainMainMenu"
            );

            Require(result, root,
                "Assets/_Project/Scripts/Runtime/Collectibles/BDGuardianSpawnSequence.cs",
                "V23R19E_GUARDIAN_SEQUENCE_MISSING",
                "BD COLLECTIBLE-INDEPENDENT GUARDIAN SPAWN V23R19E",
                "BDCollectibleGuardianSpawner.ActivateGuardian",
                "Time.unscaledDeltaTime"
            );

            Require(result, root,
                "Assets/_Project/Scripts/Runtime/Collectibles/BDCollectibleGuardianSpawner.cs",
                "V23R19E_GUARDIAN_FALLBACK_MISSING",
                "BD PLAYER-ROOM FALLBACK FOR HIDEOUT COLLECTIBLES V23R19E",
                "TryResolvePlayerRoomFallback",
                "HasClearPathFromCollectible",
                "BD PICKUP-FRAME GUARDIAN TRIGGER V23R19E",
                "OnTriggerEnter",
                "BDGuardianSpawnSequence.Schedule"
            );

            Require(result, root,
                "ProjectGuide/Features/Economy/SHOP_AND_CURRENCY_SYSTEM_V1.md",
                "V23R19E_MERCHANT_STATE_REQUIREMENTS_MISSING",
                "Automatic partial refresh",
                "Paid full reroll",
                "HostileAlive",
                "Defeated",
                "exclusive reward choice"
            );

            Require(result, root,
                "ProjectGuide/Features/Economy/META_PROGRESSION_SYSTEM_V1.md",
                "V23R19E_META_REQUIREMENT_MISSING",
                "REQUIRED FUTURE SYSTEM",
                "separate from the in-run merchant currency",
                "new playable character",
                "new boss"
            );

            Require(result, root,
                "ProjectGuide/Status/CURRENT.md",
                "V23R19E_STATUS_MISSING",
                "C01/C03/C05/C06/C11/C12.RUNTIME.V23R19E",
                "V23R19E acceptance gate",
                "C06.META"
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

                Add(result, code, "Missing V23R19E contract: " + token);
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
