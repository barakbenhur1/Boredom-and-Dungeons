#if UNITY_EDITOR
using System;
using System.IO;

namespace BoredomAndDungeons.EditorTools.Validation
{
    internal static class BDStartGameEntryCinematicV1011385QA
    {
        private const string PresenterPath =
            "Assets/_Project/Scripts/Runtime/UI/" +
            "BDModernHandheld3DPresenter.cs";

        private const string TransitionPath =
            "Assets/_Project/Scripts/Runtime/UI/" +
            "BDModernHandheld3DPresenter.StartGameTransition.cs";

        private const string FlowPath =
            "Assets/_Project/Scripts/Runtime/UI/" +
            "BDMainMenuFlow.cs";

        public static void Scan(BDOneClickQAResult result)
        {
            if (result == null)
                return;

            string root = Directory.GetCurrentDirectory();

            Require(
                result,
                Path.Combine(root, TransitionPath),
                "START_ENTRY_V1011390_VISUAL_RUNTIME_MISSING",
                "BD VISIBLE SCREEN-PLANE START ENTRY V10.11.30.90",
                "StartEntryDurationV1011390 = 2.08f",
                "ApplyStartEntryDeviceResponseV1011390",
                "ApplyStartEntryCameraV1011390",
                "StartEntryQuadraticBezierV1011390",
                "flow.HandleModernStartNewRun(); " +
                "// BD X IS NEW GAME V10.11.30.49"
            );

            Require(
                result,
                Path.Combine(root, PresenterPath),
                "START_ENTRY_V1011390_ROUTING_MISSING",
                "if (TickStartGameEntryV1011390())",
                "if (!TryBeginStartGameEntryV1011390())",
                "if (IsStartGameEntryActiveV1011390())",
                "ResetStartGameEntryForVisibilityV1011390();",
                "flow.HandleModernStartNewRun(); " +
                "// BD X IS NEW GAME V10.11.30.49",
                "BD SCREEN START NEW RUN ROUTES THROUGH ENTRY V10.11.30.91",
                "case RowAction.StartNewRun:",
                "if (!TryBeginStartGameEntryV1011390())",
                "screenDepthDescriptor.memoryless = " +
                "RenderTextureMemoryless.None",
                "screenDepthDescriptor.depthStencilFormat =",
                "new RenderTexture(screenDepthDescriptor)",
                "screenCamera.SetTargetBuffers(",
                "screenRenderTexture.colorBuffer",
                "screenDepthRenderTexture.depthBuffer"
            );

            Require(
                result,
                Path.Combine(root, FlowPath),
                "START_ENTRY_V1011390_NEW_RUN_API_MISSING",
                "public void HandleModernStartNewRun()"
            );

            Forbid(
                result,
                Path.Combine(root, TransitionPath),
                "START_ENTRY_V1011390_OLD_OWNER_REMAINS",
                "StartGameEntryDriverV1011385",
                "StartEntryPhaseV1011386",
                "StartEntryPhaseV1011387",
                "StartEntryPhaseV1011388",
                "StartEntryPhaseV1011389"
            );
        }

        private static void Require(
            BDOneClickQAResult result,
            string path,
            string code,
            params string[] tokens)
        {
            if (!File.Exists(path))
            {
                Add(result, code, path, "Required file missing.");
                return;
            }

            string source = File.ReadAllText(path);

            foreach (string token in tokens)
            {
                if (source.IndexOf(
                        token,
                        StringComparison.Ordinal) < 0)
                {
                    Add(
                        result,
                        code,
                        path,
                        "Missing required contract: " + token
                    );
                }
            }
        }

        private static void Forbid(
            BDOneClickQAResult result,
            string path,
            string code,
            params string[] tokens)
        {
            if (!File.Exists(path))
            {
                Add(result, code, path, "Required file missing.");
                return;
            }

            string source = File.ReadAllText(path);

            foreach (string token in tokens)
            {
                if (source.IndexOf(
                        token,
                        StringComparison.Ordinal) >= 0)
                {
                    Add(
                        result,
                        code,
                        path,
                        "Superseded contract remains: " + token
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
            result.findings.Add(
                new BDOneClickQAFinding(
                    BDOneClickQASeverity.Blocker,
                    code,
                    path,
                    string.Empty,
                    message
                )
            );
        }
    }
}
#endif
