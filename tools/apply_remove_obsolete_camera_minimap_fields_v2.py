#!/usr/bin/env python3
from __future__ import annotations

import re
import shutil
import sys
import tempfile
from datetime import datetime
from pathlib import Path


MINIMAP_RELATIVE = Path(
    "Assets/_Project/Scripts/Runtime/BDMazeMinimap.cs"
)

CAMERA_RELATIVE = Path(
    "Assets/_Project/Scripts/Runtime/BDCameraFollow.cs"
)

MINIMAP_FIELDS = (
    "rotationSpeedDegreesPerSecond",
    "snapToMovementCardinals",
    "mapRotationInitialized",
)

CAMERA_FIELDS = (
    "minimumMovementDirectionMagnitude",
    "rotateOnlyWhenActuallyMoving",
)

MINIMAP_MARKER = "// BD OBSOLETE MINIMAP FIELDS REMOVED V2"
CAMERA_MARKER = "// BD OBSOLETE CAMERA FIELDS REMOVED V2"


def fail(message: str) -> None:
    print(f"ERROR: {message}", file=sys.stderr)
    raise SystemExit(1)


def find_project_root(start: Path) -> Path:
    for candidate in [start.resolve(), *start.resolve().parents]:
        if (
            (candidate / MINIMAP_RELATIVE).is_file()
            and (candidate / CAMERA_RELATIVE).is_file()
        ):
            return candidate

    fail(
        "Could not find the Unity project files. "
        "Run this command from the Boredom-and-Dungeons project root."
    )


def remove_field_declaration(
    text: str,
    field_name: str,
) -> tuple[str, int]:
    # Supports:
    # [SerializeField] private float field = 1f;
    # [SerializeField] private bool field = true;
    # private bool field;
    # private float field = 0f;
    pattern = re.compile(
        rf"^[ \t]*"
        rf"(?:\[SerializeField\][ \t]*)?"
        rf"private[ \t]+(?:bool|float)[ \t]+"
        rf"{re.escape(field_name)}"
        rf"(?:[ \t]*=[^;]+)?;"
        rf"[ \t]*\r?\n",
        re.MULTILINE,
    )

    return pattern.subn("", text)


def remove_simple_assignments(
    text: str,
    field_name: str,
) -> tuple[str, int]:
    pattern = re.compile(
        rf"^[ \t]*{re.escape(field_name)}[ \t]*=[^;]+;"
        rf"[ \t]*(?://[^\r\n]*)?\r?\n",
        re.MULTILINE,
    )

    return pattern.subn("", text)


def remove_or_report(
    text: str,
    field_name: str,
) -> str:
    updated, declaration_count = remove_field_declaration(
        text,
        field_name,
    )

    updated, assignment_count = remove_simple_assignments(
        updated,
        field_name,
    )

    if declaration_count > 0:
        print(
            f"REMOVED: {field_name} declaration "
            f"({declaration_count})"
        )
    else:
        print(
            f"SKIP: {field_name} declaration is already absent."
        )

    if assignment_count > 0:
        print(
            f"REMOVED: {field_name} assignment(s) "
            f"({assignment_count})"
        )

    remaining = list(
        re.finditer(
            rf"\b{re.escape(field_name)}\b",
            updated,
        )
    )

    if remaining:
        lines = []
        for match in remaining[:5]:
            line_number = updated.count("\n", 0, match.start()) + 1
            line_start = updated.rfind("\n", 0, match.start()) + 1
            line_end = updated.find("\n", match.end())
            if line_end < 0:
                line_end = len(updated)

            lines.append(
                f"line {line_number}: "
                f"{updated[line_start:line_end].strip()}"
            )

        fail(
            f"{field_name} still has non-removable references:\n"
            + "\n".join(lines)
        )

    return updated


def ensure_marker(
    text: str,
    anchor: str,
    marker: str,
    label: str,
) -> str:
    if marker in text:
        return text

    index = text.find(anchor)
    if index < 0:
        fail(f"{label}: header anchor was not found.")

    line_end = text.find("\n", index)
    if line_end < 0:
        fail(f"{label}: header line ending was not found.")

    return (
        text[: line_end + 1]
        + f"        {marker}\n"
        + text[line_end + 1 :]
    )


def patch_minimap(path: Path) -> None:
    text = path.read_text(encoding="utf-8")

    for field_name in MINIMAP_FIELDS:
        text = remove_or_report(text, field_name)

    text = ensure_marker(
        text,
        '[Header("Dynamic Player-Up Rotation")]',
        MINIMAP_MARKER,
        "BDMazeMinimap",
    )

    path.write_text(text, encoding="utf-8")
    print("PATCHED: BDMazeMinimap.cs")


def patch_camera(path: Path) -> None:
    text = path.read_text(encoding="utf-8")

    for field_name in CAMERA_FIELDS:
        text = remove_or_report(text, field_name)

    old_header = '[Header("Movement Based Rotation")]'
    new_header = '[Header("Mouse / Player Intent Rotation")]'

    if old_header in text:
        text = text.replace(old_header, new_header, 1)
        print("RENAMED: camera rotation Inspector header")

    if new_header not in text:
        fail(
            "BDCameraFollow: neither the old nor new "
            "camera-rotation header was found."
        )

    text = ensure_marker(
        text,
        new_header,
        CAMERA_MARKER,
        "BDCameraFollow",
    )

    path.write_text(text, encoding="utf-8")
    print("PATCHED: BDCameraFollow.cs")


def verify_no_obsolete_identifiers(
    minimap_text: str,
    camera_text: str,
) -> None:
    failures = []

    for field_name in MINIMAP_FIELDS:
        if re.search(
            rf"\b{re.escape(field_name)}\b",
            minimap_text,
        ):
            failures.append(
                f"BDMazeMinimap still contains {field_name}"
            )

    for field_name in CAMERA_FIELDS:
        if re.search(
            rf"\b{re.escape(field_name)}\b",
            camera_text,
        ):
            failures.append(
                f"BDCameraFollow still contains {field_name}"
            )

    if failures:
        fail(
            "Final verification failed:\n"
            + "\n".join(failures)
        )


def main() -> None:
    root = find_project_root(Path.cwd())
    minimap = root / MINIMAP_RELATIVE
    camera = root / CAMERA_RELATIVE

    backup = (
        Path(tempfile.gettempdir())
        / (
            "BoredomAndDungeons_obsolete_fields_v2_backup_"
            f"{datetime.now():%Y%m%d_%H%M%S}"
        )
    )
    backup.mkdir(parents=True, exist_ok=True)

    shutil.copy2(minimap, backup / minimap.name)
    shutil.copy2(camera, backup / camera.name)

    patch_minimap(minimap)
    patch_camera(camera)

    verify_no_obsolete_identifiers(
        minimap.read_text(encoding="utf-8"),
        camera.read_text(encoding="utf-8"),
    )

    print()
    print("SUCCESS: all five obsolete fields are gone.")
    print(f"Backup: {backup}")
    print("Return to Unity and wait for compilation.")
    print("Then run Boredom And Dungeons -> TEST EVERYTHING.")


if __name__ == "__main__":
    main()
