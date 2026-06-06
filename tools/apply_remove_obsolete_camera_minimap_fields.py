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

MARKER_MINIMAP = "// BD REMOVE OBSOLETE MINIMAP ROTATION STATE"
MARKER_CAMERA = "// BD REMOVE OBSOLETE MOVEMENT CAMERA SETTINGS"


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
        "Could not find BDMazeMinimap.cs and BDCameraFollow.cs. "
        "Run this command from the Boredom-and-Dungeons project root."
    )


def remove_declaration(
    text: str,
    field_name: str,
    label: str,
) -> str:
    pattern = re.compile(
        rf"^[ \t]*\[SerializeField\][ \t]+private[ \t]+"
        rf"(?:bool|float)[ \t]+{re.escape(field_name)}"
        rf"[ \t]*=[^;]+;[ \t]*\r?\n",
        re.MULTILINE,
    )

    updated, count = pattern.subn("", text, count=1)

    if count == 0:
        if re.search(rf"\b{re.escape(field_name)}\b", text):
            fail(
                f"{label}: found {field_name}, but its declaration "
                "did not match the expected serialized field format."
            )

        print(f"SKIP: {field_name} is already absent.")
        return text

    print(f"REMOVED: {field_name}")
    return updated


def remove_assignment(
    text: str,
    field_name: str,
) -> str:
    pattern = re.compile(
        rf"^[ \t]*{re.escape(field_name)}[ \t]*=[^;]+;"
        rf"[ \t]*\r?\n",
        re.MULTILINE,
    )

    updated, count = pattern.subn("", text)

    if count == 0:
        print(f"SKIP: no assignment remains for {field_name}.")
        return text

    print(f"REMOVED: {count} assignment(s) to {field_name}")
    return updated


def insert_marker_after(
    text: str,
    anchor: str,
    marker: str,
    label: str,
) -> str:
    if marker in text:
        return text

    index = text.find(anchor)

    if index < 0:
        fail(f"{label}: marker anchor was not found.")

    line_end = text.find("\n", index)

    if line_end < 0:
        fail(f"{label}: marker anchor line ending was not found.")

    return (
        text[: line_end + 1]
        + f"        {marker}\n"
        + text[line_end + 1 :]
    )


def patch_minimap(path: Path) -> None:
    text = path.read_text(encoding="utf-8")

    text = remove_declaration(
        text,
        "rotationSpeedDegreesPerSecond",
        "minimap rotation speed",
    )

    text = remove_declaration(
        text,
        "snapToMovementCardinals",
        "minimap snap toggle",
    )

    text = remove_declaration(
        text,
        "mapRotationInitialized",
        "minimap initialization state",
    )

    text = remove_assignment(
        text,
        "mapRotationInitialized",
    )

    text = insert_marker_after(
        text,
        '[Header("Dynamic Player-Up Rotation")]',
        MARKER_MINIMAP,
        "minimap",
    )

    for identifier in (
        "rotationSpeedDegreesPerSecond",
        "snapToMovementCardinals",
        "mapRotationInitialized",
    ):
        if re.search(rf"\b{re.escape(identifier)}\b", text):
            fail(
                f"Minimap verification failed: {identifier} still remains."
            )

    path.write_text(text, encoding="utf-8")
    print("PATCHED: BDMazeMinimap.cs")


def patch_camera(path: Path) -> None:
    text = path.read_text(encoding="utf-8")

    text = remove_declaration(
        text,
        "minimumMovementDirectionMagnitude",
        "camera movement threshold",
    )

    text = remove_declaration(
        text,
        "rotateOnlyWhenActuallyMoving",
        "camera movement rotation toggle",
    )

    if '[Header("Movement Based Rotation")]' in text:
        text = text.replace(
            '[Header("Movement Based Rotation")]',
            '[Header("Mouse / Player Intent Rotation")]',
            1,
        )

    text = insert_marker_after(
        text,
        '[Header("Mouse / Player Intent Rotation")]',
        MARKER_CAMERA,
        "camera",
    )

    for identifier in (
        "minimumMovementDirectionMagnitude",
        "rotateOnlyWhenActuallyMoving",
    ):
        if re.search(rf"\b{re.escape(identifier)}\b", text):
            fail(
                f"Camera verification failed: {identifier} still remains."
            )

    path.write_text(text, encoding="utf-8")
    print("PATCHED: BDCameraFollow.cs")


def main() -> None:
    root = find_project_root(Path.cwd())
    minimap = root / MINIMAP_RELATIVE
    camera = root / CAMERA_RELATIVE

    backup = (
        Path(tempfile.gettempdir())
        / (
            "BoredomAndDungeons_unused_camera_minimap_fields_backup_"
            f"{datetime.now():%Y%m%d_%H%M%S}"
        )
    )
    backup.mkdir(parents=True, exist_ok=True)

    shutil.copy2(minimap, backup / minimap.name)
    shutil.copy2(camera, backup / camera.name)

    patch_minimap(minimap)
    patch_camera(camera)

    print()
    print("Removed the five obsolete serialized fields.")
    print(f"Backup: {backup}")
    print("Return to Unity and wait for compilation.")
    print("Expected result: no CS0414 warnings from these two files.")


if __name__ == "__main__":
    main()
