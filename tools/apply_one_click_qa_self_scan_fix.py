#!/usr/bin/env python3
from __future__ import annotations

import shutil
import sys
import tempfile
from datetime import datetime
from pathlib import Path


def fail(message: str) -> None:
    print(f"ERROR: {message}", file=sys.stderr)
    raise SystemExit(1)


def find_project_root(start: Path) -> Path:
    for candidate in [start.resolve(), *start.resolve().parents]:
        target = (
            candidate
            / "Assets/_Project/Scripts/Editor/Validation"
            / "BDOneClickQAWindow.cs"
        )

        if target.is_file():
            return candidate

    fail(
        "BDOneClickQAWindow.cs was not found. "
        "Run this script from the Boredom-and-Dungeons project root."
    )


def main() -> None:
    root = find_project_root(Path.cwd())

    path = (
        root
        / "Assets/_Project/Scripts/Editor/Validation"
        / "BDOneClickQAWindow.cs"
    )

    text = path.read_text(encoding="utf-8")

    marker = "// BD ONE-CLICK QA SELF-SCAN FALSE-POSITIVE FIX"

    if marker in text:
        print("SKIP: the one-click QA self-scan fix is already installed.")
        return

    old = '''                    string text = File.ReadAllText(file);
                    string relative = MakeRelative(file);

                    if (text.Contains(
                            "renderer.material",
                            StringComparison.Ordinal) ||
'''

    new = '''                    string text = File.ReadAllText(file);
                    string relative = MakeRelative(file);

                    // BD ONE-CLICK QA SELF-SCAN FALSE-POSITIVE FIX
                    // This validator contains the forbidden tokens inside
                    // its own diagnostic strings. Do not let the validator
                    // report those strings as real material API calls.
                    if (relative.EndsWith(
                            "Assets/_Project/Scripts/Editor/Validation/" +
                            "BDOneClickQAWindow.cs",
                            StringComparison.OrdinalIgnoreCase))
                    {
                        continue;
                    }

                    if (text.Contains(
                            "renderer.material",
                            StringComparison.Ordinal) ||
'''

    count = text.count(old)

    if count != 1:
        fail(
            "Expected editor-source scan block was not found exactly once. "
            f"Found {count} matches."
        )

    backup_root = (
        Path(tempfile.gettempdir())
        / (
            "BoredomAndDungeons_one_click_qa_self_scan_backup_"
            f"{datetime.now():%Y%m%d_%H%M%S}"
        )
    )
    backup_root.mkdir(parents=True, exist_ok=True)
    shutil.copy2(path, backup_root / path.name)

    text = text.replace(old, new, 1)
    path.write_text(text, encoding="utf-8")

    print("PATCHED: BDOneClickQAWindow.cs")
    print(f"Backup: {backup_root}")
    print(
        "Next: return to Unity, wait for compilation, then run "
        "Boredom And Dungeons -> TEST EVERYTHING."
    )


if __name__ == "__main__":
    main()
