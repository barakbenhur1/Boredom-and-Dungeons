#if UNITY_EDITOR
using System.IO;

namespace BoredomAndDungeons.EditorTools.Validation
{
    public static class BDRunPresentationPauseQA
    {
        // BD RUN PRESENTATION ROOM BOUNDARY + ACTIVE VISUAL CONTRACT QA V15
        public static void Scan(BDOneClickQAResult result)
        {
            string root = Directory.GetParent(
                UnityEngine.Application.dataPath
            ).FullName;

            Check(result, root,
                "Assets/_Project/Scripts/Runtime/RunPresentation/BDRunPresentationCoordinator.cs",
                new[] {
                    "BD AUTHORED ENTRY SINGLE-OWNER + PORTAL SELF-HEAL V14",
                    "ResetStaticState",
                    "DestroyObsoleteGeneratedPortals",
                    "BD_Maze_Entrance_Marker",
                    "BD_Maze_Exit_Marker",
                    "PlayMountedEntrance",
                    "HoldGameplayControlOnRunStart"
                },
                "RUN_PRESENTATION_CONTRACT_MISSING");

            Check(result, root,
                "Assets/_Project/Scripts/Runtime/UI/BDMainMenuFlow.cs",
                new[] {
                    "BD ACTION-AWARE MENU BUTTONS V7",
                    "OverlayMode.Pause",
                    "DrawActionButton",
                    "MAIN MENU / ABANDON RUN",
                    "AudioListener.pause = true",
                    "ReleaseControlAfterRunPresentation",
                    "MarkDeathRestartWithoutIntro"
                },
                "MAIN_MENU_PAUSE_POLISH_CONTRACT_MISSING");

            Check(result, root,
                "Assets/_Project/Scripts/Runtime/BDHorseController.cs",
                new[] {
                    "BD CINEMATIC MOUNT API V7",
                    "ForceMountForCinematic",
                    "SnapCinematicRiderToMountPoint"
                },
                "CINEMATIC_MOUNT_API_MISSING");

            Check(result, root,
                "Assets/_Project/Scripts/Runtime/BDMazeMinimap.cs",
                new[] {
                    "BD MINIMAP OFFSCREEN RASTER CLIP V13",
                    "RebuildRotatedMinimapRaster",
                    "RotateRasterAsSingleUnit",
                    "GUI.BeginGroup(localMapRect)",
                    "minimapRasterTexture"
                },
                "MINIMAP_RIGID_CLIP_CONTRACT_MISSING");

            Check(result, root,
                "Assets/_Project/Scripts/Runtime/BDPlayerController.cs",
                new[] {
                    "BD RUN PRESENTATION INPUT LOCK V7",
                    "BD INACTIVE CHARACTER CONTROLLER MOVE GUARD V7",
                    "characterController.gameObject.activeInHierarchy"
                },
                "PLAYER_PRESENTATION_GUARD_MISSING");

            Check(result, root,
                "Assets/_Project/Scripts/Runtime/BDCameraFollow.cs",
                new[] {
                    "BD ROOM WALL CAMERA STOP V7",
                    "ResolveRoomBoundaryConstrainedPosition",
                    "ResolveRoomBoundaryConstrainedLookPoint",
                    "BDWallSurfaceProfile"
                },
                "ROOM_BOUNDARY_CAMERA_CONTRACT_MISSING");

            Check(result, root,
                "Assets/_Project/Scripts/Runtime/UI/BDBBHBootIntro.cs",
                new[] {
                    "BD BBH SINGLE-LETTER SEQUENCE + VISIBLE GROWING FILLED CIRCLE V14",
                    "CircleGrowthDuration",
                    "CircleFullHoldDuration = 0.50f",
                    "DrawGrowingFilledCircleBehindText",
                    "CreateCircleTexture"
                },
                "BBH_FILLED_CIRCLE_CONTRACT_MISSING");

            Check(result, root,
                "Assets/_Project/Scripts/Runtime/World/BDWallSurfaceProfile.cs",
                new[] {
                    "BD ASYMMETRIC WALL TEXTURE READINESS V7",
                    "allowTextureMirroring = false",
                    "textureQuarterTurns",
                    "ConfigureFromWorldBounds"
                },
                "ASYMMETRIC_WALL_TEXTURE_PROFILE_MISSING");

            Check(result, root,
                "Assets/_Project/Scripts/Editor/Validation/BDRoomBoundarySceneInstaller.cs",
                new[] {
                    "BD TALL ROOM BOUNDARY INSTALLER V7",
                    "MinimumWallWorldHeight = 64f",
                    "RaiseWallWithoutMovingItsBase",
                    "ValidateActiveScene"
                },
                "TALL_ROOM_BOUNDARY_INSTALLER_MISSING");

            Check(result, root,
                "ProjectGuide/Features/UI/RUN_PRESENTATION_PAUSE_AND_MENU_POLISH.md",
                new[] {
                    "authored entrance",
                    "ordinary death restart",
                    "existing `BDMainMenuFlow`",
                    "rigid unit"
                },
                "RUN_PRESENTATION_DESIGN_MISSING");

            Check(result, root,
                "ProjectGuide/Features/Map/ROOM_BOUNDARY_CAMERA_AND_TEXTURE_READINESS.md",
                new[] {
                    "64 world units",
                    "visibility boundary",
                    "asymmetric texture",
                    "negative scale"
                },
                "ROOM_BOUNDARY_DESIGN_MISSING");

            if (!BDRoomBoundarySceneInstaller.ValidateActiveScene(
                    out string boundaryError))
            {
                result.findings.Add(
                    new BDOneClickQAFinding(
                        BDOneClickQASeverity.Blocker,
                        "ROOM_BOUNDARY_SCENE_INVALID",
                        "Assets/_Project/Scenes/02_CleanCore_MazePrototype.unity",
                        string.Empty,
                        boundaryError
                    )
                );
            }
        }

        private static void Check(
            BDOneClickQAResult result,
            string root,
            string relative,
            string[] tokens,
            string code)
        {
            string path = Path.Combine(root, relative);
            if (!File.Exists(path))
            {
                result.findings.Add(
                    new BDOneClickQAFinding(
                        BDOneClickQASeverity.Blocker,
                        code,
                        relative,
                        string.Empty,
                        "Required source/document is missing."
                    )
                );
                return;
            }

            string text = File.ReadAllText(path);
            foreach (string token in tokens)
            {
                if (text.Contains(token))
                    continue;

                result.findings.Add(
                    new BDOneClickQAFinding(
                        BDOneClickQASeverity.Blocker,
                        code,
                        relative,
                        string.Empty,
                        "Missing required token: " + token
                    )
                );
            }
        }
    }
}
#endif
