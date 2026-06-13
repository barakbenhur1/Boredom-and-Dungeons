#if UNITY_EDITOR
using System.IO;

namespace BoredomAndDungeons.EditorTools.Validation
{
    internal static class BDMetalMemorylessWarningV1011343QA
    {
        private const string PresenterPath =
            "Assets/_Project/Scripts/Runtime/UI/" +
            "BDModernHandheld3DPresenter.cs";

        private const string RuntimePath =
            "Assets/_Project/Scripts/Runtime/UI/" +
            "BDModernHandheld3DPresenter." +
            "MetalDepthOwnershipV1011343.cs";

        public static void Scan(BDOneClickQAResult result)
        {
            string root = Directory.GetCurrentDirectory();

            Require(
                result,
                root,
                PresenterPath,
                "HANDHELD_V1011343_DEPTH_OWNER_HOOKS_MISSING",
                "SetHandheldRenderOwnershipV1011343(value);",
                "MaintainHandheldRenderOwnershipV1011343();",
                "RestoreCompetingGameCamerasV1011343(); // destroy",
                "ConfigureMetalDepthSurfaceOwnersV1011343();"
            );

            Require(
                result,
                root,
                RuntimePath,
                "HANDHELD_V1011343_METAL_DEPTH_OWNER_MISSING",
                "BD METAL DEPTH OWNER REPAIR V10.11.30.43",
                "candidate.cameraType != CameraType.Game",
                "candidate.targetTexture != null",
                "candidate.enabled = false",
                "camera.enabled = entry.Value",
                "GraphicsDeviceType.Metal",
                "light.name != \"Cinematic Key Light\"",
                "light.shadows = LightShadows.None",
                "light.shadowStrength = 0f"
            );
        }

        private static void Require(
            BDOneClickQAResult result,
            string root,
            string relative,
            string code,
            params string[] tokens)
        {
            string full = Path.Combine(root, relative);
            if (!File.Exists(full))
            {
                Add(result, code, relative, "Required file missing.");
                return;
            }

            string source = File.ReadAllText(full);
            foreach (string token in tokens)
            {
                if (!source.Contains(token))
                {
                    Add(
                        result,
                        code,
                        relative,
                        "Missing required contract token: " + token
                    );
                }
            }
        }

        private static void Add(
            BDOneClickQAResult result,
            string code,
            string path,
            string message)
        {
            result.findings.Add(new BDOneClickQAFinding(
                BDOneClickQASeverity.Blocker,
                code,
                path,
                string.Empty,
                message
            ));
        }
    }
}
#endif
