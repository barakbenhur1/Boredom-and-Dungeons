#if UNITY_EDITOR
using System;
using System.IO;
using UnityEngine;

namespace BoredomAndDungeons.EditorTools.Validation
{
    internal static class BDMinimapRigidRotationQA
    {
        private const string MinimapRelative =
            "Assets/_Project/Scripts/Runtime/BDMazeMinimap.cs";

        public static void Scan(BDOneClickQAResult result)
        {
            if (result == null)
                return;

            string root = ResolveProjectRoot();
            string absolute = Path.Combine(root, MinimapRelative);

            if (!File.Exists(absolute))
            {
                Add(
                    result,
                    "MINIMAP_RIGID_ROTATION_FILE_MISSING",
                    "BDMazeMinimap.cs is missing."
                );
                return;
            }

            string source = File.ReadAllText(absolute);

            string[] required =
            {
                "BD MINIMAP RIGID CLIP MASK V7",
                "DrawRigidRotatedMapContent(",
                "Matrix4x4 originalMatrix = GUI.matrix;",
                "GUIUtility.RotateAroundPivot(",
                "currentMapRotationDegrees",
                "DrawRooms(mapRect);",
                "DrawMarker(",
                "GUI.matrix = originalMatrix;",
                "Mathf.SmoothDampAngle(",
                "cardinalRotationSnapEpsilon"
            };

            foreach (string token in required)
            {
                if (source.IndexOf(
                        token,
                        StringComparison.Ordinal) >= 0)
                {
                    continue;
                }

                Add(
                    result,
                    "MINIMAP_RIGID_ROTATION_CONTRACT_MISSING",
                    "Missing minimap rigid-rotation contract: " +
                    token
                );
            }

            if (CountOccurrences(
                    source,
                    "DrawRigidRotatedMapContent(") != 2)
            {
                Add(
                    result,
                    "MINIMAP_RIGID_ROTATION_CALL_COUNT_INVALID",
                    "DrawRigidRotatedMapContent must have exactly " +
                    "one definition and one OnGUI call."
                );
            }

            if (CountOccurrences(
                    source,
                    "DrawRotatedRoomsClipped(") != 1)
            {
                Add(
                    result,
                    "MINIMAP_PER_ROOM_ROTATION_STILL_ACTIVE",
                    "Legacy per-room rotation must remain definition-only."
                );
            }

            if (CountOccurrences(
                    source,
                    "DrawRotatedMarkerClipped(") != 1)
            {
                Add(
                    result,
                    "MINIMAP_PER_MARKER_ROTATION_STILL_ACTIVE",
                    "Legacy per-marker rotation must remain definition-only."
                );
            }

            if (CountOccurrences(
                    source,
                    "GUIUtility.RotateAroundPivot(") != 1)
            {
                Add(
                    result,
                    "MINIMAP_MULTIPLE_ROTATION_ROOTS",
                    "The minimap must use exactly one GUI rotation root."
                );
            }

            int rotateIndex = source.IndexOf(
                "GUIUtility.RotateAroundPivot(",
                StringComparison.Ordinal
            );

            int restoreIndex = source.IndexOf(
                "GUI.matrix = originalMatrix;",
                StringComparison.Ordinal
            );

            if (rotateIndex < 0 ||
                restoreIndex <= rotateIndex)
            {
                Add(
                    result,
                    "MINIMAP_MATRIX_RESTORE_ORDER_INVALID",
                    "The original GUI matrix must be restored after " +
                    "the rigid content is drawn."
                );
            }
        }

        private static int CountOccurrences(
            string source,
            string token)
        {
            int count = 0;
            int searchFrom = 0;

            while (true)
            {
                int index = source.IndexOf(
                    token,
                    searchFrom,
                    StringComparison.Ordinal
                );

                if (index < 0)
                    return count;

                count++;
                searchFrom = index + token.Length;
            }
        }

        private static string ResolveProjectRoot()
        {
            DirectoryInfo assets = new DirectoryInfo(
                Application.dataPath
            );

            return assets.Parent != null
                ? assets.Parent.FullName
                : Application.dataPath;
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
                    MinimapRelative,
                    string.Empty,
                    message
                )
            );
        }
    }
}
#endif
