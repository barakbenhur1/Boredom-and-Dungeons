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

            string absolute = Path.Combine(
                ResolveProjectRoot(),
                MinimapRelative
            );
            if (!File.Exists(absolute))
            {
                Add(result, "MINIMAP_RASTER_FILE_MISSING", "BDMazeMinimap.cs is missing.");
                return;
            }

            string source = File.ReadAllText(absolute);
            string[] required =
            {
                "BD MINIMAP OFFSCREEN RASTER CLIP V13",
                "EnsureMinimapRaster",
                "RebuildRotatedMinimapRaster",
                "RotateRasterAsSingleUnit",
                "GUI.BeginGroup(localMapRect)",
                "GUI.DrawTexture(",
                "minimapRasterTexture",
                "Mathf.SmoothDampAngle(",
                "cardinalRotationSnapEpsilon"
            };

            foreach (string token in required)
            {
                if (source.IndexOf(token, StringComparison.Ordinal) >= 0)
                    continue;
                Add(result, "MINIMAP_RASTER_CONTRACT_MISSING", "Missing minimap V13 contract: " + token);
            }

            if (source.IndexOf(
                    "DrawRigidRotatedMapContent(clipRect, mapScreenOrigin)",
                    StringComparison.Ordinal) >= 0)
            {
                Add(
                    result,
                    "MINIMAP_BROKEN_GUI_MATRIX_PATH_ACTIVE",
                    "The broken screen-space GUI-matrix path must remain removed."
                );
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
