#if UNITY_EDITOR
using System;
using System.IO;

namespace BoredomAndDungeons.EditorTools.Validation
{
    public static class BDDocumentationGovernanceQA
    {
        private static readonly string[] RequiredDocuments =
        {
            "README.md",
            "START_HERE.md",
            "DEVELOPMENT_WORKFLOW.md",
            "PROJECT_STATUS.md",
            "DOCUMENTATION_INDEX.md",
            "ARCHITECTURE.md",
            "QA_CHECKLIST.md",
            "TECHNICAL_DECISIONS.md",
            "PERFORMANCE_GUIDELINES.md"
        };

        public static void Scan(BDOneClickQAResult result)
        {
            string root = ResolveProjectRoot();

            for (int index = 0; index < RequiredDocuments.Length; index++)
            {
                string relative = RequiredDocuments[index];
                string absolute = Path.Combine(root, relative);

                if (!File.Exists(absolute))
                {
                    Add(
                        result,
                        "DOCUMENTATION_REQUIRED_FILE_MISSING",
                        relative,
                        "Required maintained documentation file is missing."
                    );
                    continue;
                }

                string source = File.ReadAllText(absolute);
                ScanConflictMarkers(result, relative, source);
            }

            ValidateTokens(
                result,
                root,
                "START_HERE.md",
                new[]
                {
                    "Mandatory First Read",
                    "Permanent user-request capture rule",
                    "The user does not need to explain or repeat it"
                }
            );

            ValidateTokens(
                result,
                root,
                "DEVELOPMENT_WORKFLOW.md",
                new[]
                {
                    "PERMANENT USER-REQUEST CAPTURE CONTRACT",
                    "The user never needs to repeat this instruction",
                    "DOCUMENTATION_INDEX.md"
                }
            );

            ValidateTokens(
                result,
                root,
                "DOCUMENTATION_INDEX.md",
                new[]
                {
                    "ARCHITECTURE.md",
                    "QA_CHECKLIST.md",
                    "TECHNICAL_DECISIONS.md",
                    "PERFORMANCE_GUIDELINES.md"
                }
            );

            ValidateTokens(
                result,
                root,
                "ARCHITECTURE.md",
                new[]
                {
                    "flowchart",
                    "BDMainMenuFlow",
                    "BDOneClickQAWindow"
                }
            );

            ValidateTokens(
                result,
                root,
                "PROJECT_STATUS.md",
                new[]
                {
                    "C00.DOC-GOVERNANCE.V8",
                    "request is recorded before or with implementation",
                    "saved resume point is preserved"
                }
            );

            string oneClickRelative =
                "Assets/_Project/Scripts/Editor/Validation/" +
                "BDOneClickQAWindow.cs";
            string oneClickPath = Path.Combine(root, oneClickRelative);

            if (File.Exists(oneClickPath))
            {
                string source = File.ReadAllText(oneClickPath);
                ScanConflictMarkers(result, oneClickRelative, source);

                if (!source.Contains(
                        "BDDocumentationGovernanceQA.Scan(result);"))
                {
                    Add(
                        result,
                        "DOCUMENTATION_QA_NOT_INTEGRATED",
                        oneClickRelative,
                        "Documentation governance QA is not integrated into TEST EVERYTHING."
                    );
                }
            }
        }

        private static void ValidateTokens(
            BDOneClickQAResult result,
            string root,
            string relative,
            string[] tokens)
        {
            string absolute = Path.Combine(root, relative);
            if (!File.Exists(absolute))
                return;

            string source = File.ReadAllText(absolute);
            for (int index = 0; index < tokens.Length; index++)
            {
                if (source.Contains(tokens[index]))
                    continue;

                Add(
                    result,
                    "DOCUMENTATION_CONTRACT_TOKEN_MISSING",
                    relative,
                    "Missing documentation contract token: " +
                    tokens[index]
                );
            }
        }

        private static void ScanConflictMarkers(
            BDOneClickQAResult result,
            string relative,
            string source)
        {
            if (!ContainsConflictMarkerLine(source))
                return;

            Add(
                result,
                "DOCUMENTATION_OR_QA_CONFLICT_MARKER",
                relative,
                "Unresolved standalone Git conflict marker line blocks reliable development and QA."
            );
        }

        private static bool ContainsConflictMarkerLine(
            string source)
        {
            if (string.IsNullOrEmpty(source))
                return false;

            using (StringReader reader = new StringReader(source))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string trimmed = line.TrimStart();

                    // Only real standalone conflict-marker lines count.
                    // Inline Markdown examples and quoted C# strings must not
                    // block TEST EVERYTHING.
                    if (trimmed.StartsWith(
                            "<<<<<<<",
                            StringComparison.Ordinal) ||
                        trimmed.Equals(
                            "=======",
                            StringComparison.Ordinal) ||
                        trimmed.StartsWith(
                            ">>>>>>>",
                            StringComparison.Ordinal))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private static string ResolveProjectRoot()
        {
            return Directory.GetParent(
                UnityEngine.Application.dataPath
            )?.FullName ?? string.Empty;
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
