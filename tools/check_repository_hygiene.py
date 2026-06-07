#!/usr/bin/env python3
from pathlib import Path
import sys

ROOT = Path(__file__).resolve().parents[1]
REQUIRED = {
    "AGENTS.md", "README.md", "START_HERE.md", "DEVELOPMENT_WORKFLOW.md",
    "PROJECT_STATUS.md", "DOCUMENTATION_INDEX.md", "ARCHITECTURE.md",
    "QA_CHECKLIST.md", "TECHNICAL_DECISIONS.md", "PERFORMANCE_GUIDELINES.md"
}
FORBIDDEN = {
    "NEXT_STEPS.md", "WORKING_NOW.md", "LATEST_STATUS.md",
    "PROJECT_STATUS_V2.md", "CURRENT_STATUS.md"
}
errors = []

for required_path in (".codex/config.toml", ".codex/agents"):
    if not (ROOT / required_path).exists():
        errors.append(f"missing maintained Codex project configuration: {required_path}")

gitignore = ROOT / ".gitignore"
if not gitignore.is_file() or "/AGENTS.rtf" not in gitignore.read_text():
    errors.append("AGENTS.rtf local duplicate is not protected by .gitignore")

for name in sorted(REQUIRED):
    if not (ROOT / name).is_file():
        errors.append(f"missing required root document: {name}")
for name in sorted(FORBIDDEN):
    if (ROOT / name).exists():
        errors.append(f"obsolete/duplicate root document present: {name}")
for path in sorted(ROOT.glob("*.md")):
    if path.name not in REQUIRED:
        errors.append(f"non-canonical root Markdown present: {path.name}")
for path in sorted(ROOT.glob("README_*REPAIR*.txt")):
    errors.append(f"package README left in repository root: {path.name}")
for path in sorted(ROOT.glob("PACKAGE_MANIFEST_*.txt")):
    errors.append(f"package manifest left in repository root: {path.name}")
for rel in (
    "Assets/_Project/Design/Runtime/VISUAL_ENTRY_MINIMAP_MOUNTED_COMBAT_REPAIR_V11.md",
    "Assets/_Project/Design/Runtime/VISUAL_ENTRY_MINIMAP_MOUNTED_COMBAT_REPAIR_V11.md.meta",
):
    if (ROOT / rel).exists():
        errors.append(f"superseded repair document present: {rel}")
if errors:
    for error in errors:
        print(f"ERROR: {error}")
    sys.exit(1)
print("PASS: repository documentation and package-artifact hygiene checks passed.")
