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

            Require(result, root, PresenterPath,
                "SCREEN_V1011330_PERSISTENT_DEPTH_ATTACHMENT_MISSING",
                "BD PERSISTENT NON-MEMORYLESS SCREEN DEPTH V10.11.30.30",
                "SystemInfo.GetGraphicsFormat(DefaultFormat.DepthStencil)",
                "GraphicsFormat.D32_SFloat_S8_UInt",
                "screenDescriptor.depthStencilFormat = screenDepthStencilFormat",
                "screenDescriptor.memoryless = RenderTextureMemoryless.None",
                "screenDescriptor.msaaSamples = 1",
                "screenCamera.allowMSAA = false",
                "screenCamera.depthTextureMode = DepthTextureMode.None");
            Forbid(result, root, PresenterPath,
                "SCREEN_V1011330_DEPTHLESS_TRANSIENT_REGRESSION",
                "screenDescriptor.depthStencilFormat = GraphicsFormat.None;");
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
