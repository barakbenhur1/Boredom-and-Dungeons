#if UNITY_EDITOR
using System.IO;

namespace BoredomAndDungeons.EditorTools.Validation
{
    internal static class BDTutorialBubbleDepthV1011330QA
    {
        private const string DialoguePath =
            "Assets/_Project/Scripts/Runtime/UI/" +
            "BDModernHandheld3DPresenter.ChildApproachDialogue.cs";
        private const string PresenterPath =
            "Assets/_Project/Scripts/Runtime/UI/" +
            "BDModernHandheld3DPresenter.cs";
        private const string SettingsPath =
            "ProjectSettings/ProjectSettings.asset";

        public static void Scan(BDOneClickQAResult result)
        {
            string root = Directory.GetCurrentDirectory();

            Require(result, root, DialoguePath,
                "TUTORIAL_V1011330_FAR_DIAMOND_FRAME_MISSING",
                "BD COMPLETE FAR-LEFT DIAMOND FRAME V10.11.30.30",
                "Mother Tail Curve Far Frame",
                "farTailShadowRect.anchoredPosition = new Vector2(-42f, -66f)",
                "farTailShadowRect.sizeDelta = new Vector2(26f, 26f)",
                "farTailShadow.color = outline.effectColor",
                "farTailShadowObject.transform.SetSiblingIndex(1)",
                "farTailObject.transform.SetSiblingIndex(2)");

            // V10.11.30.31 supersedes the combined color+depth target.
            // The bubble contract remains here; rendering now requires explicit
            // persistent color and depth buffers bound to the screen camera.
            Require(result, root, PresenterPath,
                "SCREEN_V1011330_DEPTH_OWNER_SUPERSEDED_BY_V1011331",
                "BD EXPLICIT PERSISTENT SCREEN COLOR DEPTH V10.11.30.31",
                "private RenderTexture screenDepthRenderTexture;",
                "screenDescriptor.depthStencilFormat = GraphicsFormat.None",
                "screenDescriptor.memoryless = RenderTextureMemoryless.None",
                "screenDepthDescriptor.memoryless = RenderTextureMemoryless.None",
                "screenDepthDescriptor.depthStencilFormat =",
                "screenCamera.SetTargetBuffers(",
                "screenRenderTexture.colorBuffer",
                "screenDepthRenderTexture.depthBuffer");
            Forbid(result, root, PresenterPath,
                "SCREEN_V1011330_COMBINED_DEPTH_OWNER_RETURNED",
                "screenDescriptor.depthStencilFormat = screenDepthStencilFormat;",
                "screenDepthDescriptor.memoryless = RenderTextureMemoryless.Depth;");
            Require(result, root, SettingsPath,
                "SCREEN_V1011330_FRAMEBUFFER_MEMORYLESS_UNUSED_MISSING",
                "framebufferDepthMemorylessMode: 0");
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
                    Add(result, code, relative,
                        "Missing required contract token: " + token);
            }
        }

        private static void Forbid(
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
                if (source.Contains(token))
                    Add(result, code, relative,
                        "Forbidden regression token remains: " + token);
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
