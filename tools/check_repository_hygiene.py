#!/usr/bin/env python3
from pathlib import Path
import os
import sys

ROOT = Path(__file__).resolve().parents[1]
REQUIRED = {
    "AGENTS.md",
    "README.md",
    "ProjectGuide/README.md",
    "ProjectGuide/INDEX.md",
    "ProjectGuide/Rules/WORKFLOW.md",
    "ProjectGuide/Rules/DOCUMENT_MAINTENANCE.md",
    "ProjectGuide/Status/CURRENT.md",
    "ProjectGuide/Status/BUGS.md",
    "ProjectGuide/Status/WORK_QUEUE.md",
    "ProjectGuide/Product/ART_DIRECTION.md",
    "ProjectGuide/Product/AUDIO_DIRECTION.md",
    "ProjectGuide/Engineering/ARCHITECTURE.md",
    "ProjectGuide/Engineering/TECHNICAL_DECISIONS.md",
    "ProjectGuide/Engineering/PERFORMANCE.md",
    "ProjectGuide/QA/QA_CHECKLIST.md",
}
FORBIDDEN_ROOT_MD = {
    "START_HERE.md", "DEVELOPMENT_WORKFLOW.md", "PROJECT_STATUS.md",
    "DOCUMENTATION_INDEX.md", "ARCHITECTURE.md", "QA_CHECKLIST.md",
    "TECHNICAL_DECISIONS.md", "PERFORMANCE_GUIDELINES.md",
    "ART_DIRECTION.md", "AUDIO_DIRECTION.md", "NEXT_STEPS.md",
    "WORKING_NOW.md", "LATEST_STATUS.md", "PROJECT_STATUS_V2.md",
    "CURRENT_STATUS.md"
}
errors=[]

for required in sorted(REQUIRED):
    if not (ROOT/required).is_file():
        errors.append(f"missing maintained guide file: {required}")
for name in sorted(FORBIDDEN_ROOT_MD):
    if (ROOT/name).exists():
        errors.append(f"obsolete or relocated root document present: {name}")
for path in sorted(ROOT.glob("*.md")):
    if path.name not in {"AGENTS.md","README.md"}:
        errors.append(f"unexpected root Markdown present: {path.name}")
if (ROOT/'Assets/_Project/Design').exists() or (ROOT/'Assets/_Project/Design.meta').exists():
    errors.append("legacy Assets/_Project/Design tree remains; project knowledge belongs in ProjectGuide")
for required in ('.codex/config.toml','.codex/agents'):
    if not (ROOT/required).exists(): errors.append(f"missing Codex configuration: {required}")
gitignore=ROOT/'.gitignore'
if not gitignore.is_file() or '/AGENTS.rtf' not in gitignore.read_text(encoding='utf-8'):
    errors.append('AGENTS.rtf local duplicate is not protected by .gitignore')
for path in ROOT.rglob('*'):
    if not path.is_file() or '.git' in path.parts: continue
    if path.name in {'.DS_Store'} or path.name.startswith('._'):
        errors.append(f"OS metadata present: {path.relative_to(ROOT)}")
allow_package_artifacts = os.environ.get('BD_ALLOW_PACKAGE_ARTIFACTS') == '1'
if not allow_package_artifacts:
    for pattern in ('README_*REPAIR*.txt', 'README_PROJECT_GUIDE_*.txt', 'PACKAGE_MANIFEST_*.txt'):
        for path in ROOT.glob(pattern):
            errors.append(f"package artifact left in root: {path.name}")
if errors:
    for error in errors: print(f"ERROR: {error}")
    sys.exit(1)
print('PASS: ProjectGuide structure and repository hygiene checks passed.')
