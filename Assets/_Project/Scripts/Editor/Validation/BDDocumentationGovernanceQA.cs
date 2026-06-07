#if UNITY_EDITOR
using System;
using System.IO;

namespace BoredomAndDungeons.EditorTools.Validation
{
    public static class BDDocumentationGovernanceQA
    {
        private static readonly string[] RequiredDocuments =
        {
            "AGENTS.md",
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

        // BD PERMANENT REPOSITORY HYGIENE QA V19
        private static readonly string[] ForbiddenRootDocuments =
        {
            "NEXT_STEPS.md",
            "WORKING_NOW.md",
            "LATEST_STATUS.md",
            "PROJECT_STATUS_V2.md",
            "CURRENT_STATUS.md"
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

            for (int index = 0; index < ForbiddenRootDocuments.Length; index++)
            {
                string relative = ForbiddenRootDocuments[index];
                if (!File.Exists(Path.Combine(root, relative)))
                    continue;

                Add(
                    result,
                    "OBSOLETE_ROOT_DOCUMENT_PRESENT",
                    relative,
                    "Obsolete or duplicate root documentation must be removed; Git history preserves old plans."
                );
            }

            if (Directory.Exists(Path.Combine(root, ".ai")))
            {
                Add(
                    result,
                    "LOCAL_AI_CACHE_PRESENT",
                    ".ai/",
                    "Local AI-assistant cache must not remain as repository content."
                );
            }

            ValidateTokens(
                result,
                root,
                "START_HERE.md",
                new[]
                {
                    "Mandatory First Read",
                    "AGENTS.md",
                    ".codex/",
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
                    "PERMANENT REPOSITORY HYGIENE CONTRACT",
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
                    "AGENTS.md",
                    ".codex/config.toml",
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
                    "Current development snapshot",
                    "Record every material user request here",
                    "Saved feature resume point",
                    "Run repository hygiene on every handoff"
                }
            );

            ValidateCodexConfiguration(result, root);

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

        private static void ValidateCodexConfiguration(
            BDOneClickQAResult result,
            string root)
        {
            string config = Path.Combine(root, ".codex", "config.toml");
            string agents = Path.Combine(root, ".codex", "agents");

            if (!File.Exists(config))
            {
                Add(
                    result,
                    "CODEX_PROJECT_CONFIG_MISSING",
                    ".codex/config.toml",
                    "Maintained Codex project configuration is missing."
                );
            }

            if (!Directory.Exists(agents))
            {
                Add(
                    result,
                    "CODEX_AGENT_DIRECTORY_MISSING",
                    ".codex/agents/",
                    "Maintained Codex specialist-agent directory is missing."
                );
            }

            string gitignore = Path.Combine(root, ".gitignore");
            if (!File.Exists(gitignore) ||
                !File.ReadAllText(gitignore).Contains("/AGENTS.rtf"))
            {
                Add(
                    result,
                    "CODEX_RTF_DUPLICATE_NOT_IGNORED",
                    ".gitignore",
                    "AGENTS.rtf must remain an ignored local duplicate of canonical AGENTS.md."
                );
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
