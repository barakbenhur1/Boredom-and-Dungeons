#if UNITY_EDITOR
using System.IO;

namespace BoredomAndDungeons.EditorTools.Validation
{
    internal static class BDScreenRenderSchedulingV1011321QA
    {
        private const string PresenterPath =
            "Assets/_Project/Scripts/Runtime/UI/BDModernHandheld3DPresenter.cs";
        private const string ProjectSettingsPath =
            "ProjectSettings/ProjectSettings.asset";

        public static void Scan(BDOneClickQAResult result)
        {
            string root = Directory.GetCurrentDirectory();
            string presenter = Read(root, PresenterPath, result);
            string settings = Read(root, ProjectSettingsPath, result);
            if (presenter.Length == 0 || settings.Length == 0)
                return;

            Require(result, presenter, PresenterPath,
                "SCREEN_RENDER_V1011321_AUTOMATIC_SCHEDULING_MISSING",
                "BD AUTOMATIC SCREEN CAMERA RENDER SCHEDULING V10.11.30.21",
                "Canvas.ForceUpdateCanvases();",
                "BD EXPLICIT PERSISTENT SCREEN COLOR DEPTH V10.11.30.31",
                "private RenderTexture screenDepthRenderTexture;",
                "screenDescriptor.depthStencilFormat = GraphicsFormat.None",
                "screenDescriptor.memoryless = RenderTextureMemoryless.None;",
                "screenDepthDescriptor.memoryless =",
                "screenDepthDescriptor.depthStencilFormat =",
                "screenCamera.SetTargetBuffers(",
                "screenRenderTexture.colorBuffer",
                "screenDepthRenderTexture.depthBuffer",
                "screenCamera.depthTextureMode = DepthTextureMode.None;",
                "BD NON-MEMORYLESS DEVICE CAMERA DEPTH V10.11.30.26",
                "deviceCamera.allowMSAA = false;",
                "deviceCamera.depthTextureMode = DepthTextureMode.None;");

            Forbid(result, presenter, PresenterPath,
                "SCREEN_RENDER_V1011321_DUPLICATE_CAMERA_RENDER_REGRESSION",
                "screenCamera.Render();",
                "deviceCamera.allowMSAA = true;",
                "screenDescriptor.depthStencilFormat = screenDepthStencilFormat;",
                "screenDepthDescriptor.memoryless = RenderTextureMemoryless.Depth;");

            Require(result, settings, ProjectSettingsPath,
                "SCREEN_RENDER_V1011321_FRAMEBUFFER_MEMORYLESS_SETTING_REGRESSION",
                "framebufferDepthMemorylessMode: 0");
        }

        private static string Read(
            string root,
            string relativePath,
            BDOneClickQAResult result)
        {
            string absolute = Path.Combine(root, relativePath);
            if (File.Exists(absolute))
                return File.ReadAllText(absolute);

            Add(result,
                "SCREEN_RENDER_V1011321_REQUIRED_FILE_MISSING",
                relativePath,
                "Required file is missing.");
            return string.Empty;
        }

        private static void Require(
            BDOneClickQAResult result,
            string text,
            string path,
            string code,
            params string[] tokens)
        {
            for (int index = 0; index < tokens.Length; index++)
            {
                if (text.Contains(tokens[index]))
                    continue;
                Add(result, code, path,
                    "Missing required contract token: " + tokens[index]);
            }
        }

        private static void Forbid(
            BDOneClickQAResult result,
            string text,
            string path,
            string code,
            params string[] tokens)
        {
            for (int index = 0; index < tokens.Length; index++)
            {
                if (!text.Contains(tokens[index]))
                    continue;
                Add(result, code, path,
                    "Forbidden duplicate-render token remains: " + tokens[index]);
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
