#!/usr/bin/env python3
from __future__ import annotations

import shutil
import sys
import tempfile
from datetime import datetime
from pathlib import Path


TARGET_RELATIVE = Path(
    "Assets/_Project/Scripts/Editor/Validation/BDOneClickQAWindow.cs"
)

MARKER = "// BD ONE-CLICK QA SELF-SCAN FALSE-POSITIVE FIX V2"


def fail(message: str) -> None:
    print(f"ERROR: {message}", file=sys.stderr)
    raise SystemExit(1)


def find_project_root(start: Path) -> Path:
    for candidate in [start.resolve(), *start.resolve().parents]:
        target = candidate / TARGET_RELATIVE

        if target.is_file():
            return candidate

    fail(
        "Could not find BDOneClickQAWindow.cs. "
        "Run this command from the Boredom-and-Dungeons project root."
    )


def line_indent(text: str, index: int) -> str:
    line_start = text.rfind("\n", 0, index) + 1
    line = text[line_start:index]

    return line[: len(line) - len(line.lstrip())]


def main() -> None:
    root = find_project_root(Path.cwd())
    path = root / TARGET_RELATIVE
    text = path.read_text(encoding="utf-8")

    if MARKER in text:
        print("SKIP: V2 self-scan fix is already installed.")
        return

    editor_block_anchor = "if (Directory.Exists(editorRoot))"
    editor_block_index = text.find(editor_block_anchor)

    if editor_block_index < 0:
        fail(
            "Could not locate the Editor source scan block "
            "(if (Directory.Exists(editorRoot)))."
        )

    relative_anchor = "string relative = MakeRelative(file);"
    relative_index = text.find(
        relative_anchor,
        editor_block_index,
    )

    if relative_index < 0:
        fail(
            "Could not locate 'string relative = MakeRelative(file);' "
            "inside the Editor source scan block."
        )

    next_major_method = text.find(
        "private static void ScanDuplicateTypeDeclarations",
        editor_block_index,
    )

    if (
        next_major_method >= 0
        and relative_index > next_major_method
    ):
        fail(
            "The relative-path line was found outside the expected "
            "Editor scan method."
        )

    insertion_point = relative_index + len(relative_anchor)
    indent = line_indent(text, relative_index)

    insertion = f'''

{indent}{MARKER}
{indent}// The validator contains the forbidden text inside its own
{indent}// diagnostics and search expressions. Skip only this validator
{indent}// file so it cannot report itself as a material-access violation.
{indent}if (relative.EndsWith(
{indent}        "Assets/_Project/Scripts/Editor/Validation/" +
{indent}        "BDOneClickQAWindow.cs",
{indent}        StringComparison.OrdinalIgnoreCase))
{indent}{{
{indent}    continue;
{indent}}}
'''

    backup_root = (
        Path(tempfile.gettempdir())
        / (
            "BoredomAndDungeons_one_click_qa_self_scan_v2_backup_"
            f"{datetime.now():%Y%m%d_%H%M%S}"
        )
    )
    backup_root.mkdir(parents=True, exist_ok=True)
    shutil.copy2(path, backup_root / path.name)

    updated = (
        text[:insertion_point]
        + insertion
        + text[insertion_point:]
    )

    if MARKER not in updated:
        fail("Internal patch verification failed: marker missing.")

    if updated.count(MARKER) != 1:
        fail(
            "Internal patch verification failed: marker count is not 1."
        )

    if "BDOneClickQAWindow.cs" not in updated:
        fail(
            "Internal patch verification failed: target filename missing."
        )

    path.write_text(updated, encoding="utf-8")

    print("PATCHED: BDOneClickQAWindow.cs")
    print(f"Backup: {backup_root}")
    print()
    print("Next:")
    print("1. Return to Unity.")
    print("2. Wait for compilation.")
    print("3. Run Boredom And Dungeons -> TEST EVERYTHING.")


if __name__ == "__main__":
    main()
