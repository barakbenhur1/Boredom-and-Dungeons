#!/usr/bin/env python3
from pathlib import Path
import sys


def find_project_root(start: Path) -> Path:
    for candidate in [start.resolve(), *start.resolve().parents]:
        runtime = candidate / "Assets/_Project/Scripts/Runtime"
        if runtime.is_dir():
            return candidate

    print(
        "ERROR: Run this script from the Boredom-and-Dungeons project root.",
        file=sys.stderr,
    )
    raise SystemExit(1)


def main() -> None:
    root = find_project_root(Path.cwd())

    duplicate_files = [
        root / "Assets/_Project/Scripts/Runtime/Bosses/Stage16/BDBossHealthHud.cs",
        root / "Assets/_Project/Scripts/Runtime/Bosses/Stage16/BDBossHealthHud.cs.meta",
    ]

    removed = 0

    for path in duplicate_files:
        if path.exists():
            path.unlink()
            print(f"REMOVED: {path.relative_to(root)}")
            removed += 1
        else:
            print(f"NOT FOUND (already clean): {path.relative_to(root)}")

    print()
    if removed > 0:
        print("Duplicate Stage 16 Boss HUD removed successfully.")
    else:
        print("No duplicate Stage 16 Boss HUD files remained.")

    print("Unity will continue using the existing BDBossHealthHud already in the project.")
    print("Next: return to Unity and wait for recompilation.")


if __name__ == "__main__":
    main()
