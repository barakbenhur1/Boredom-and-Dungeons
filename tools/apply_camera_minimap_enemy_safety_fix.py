#!/usr/bin/env python3
from __future__ import annotations

import shutil
import sys
import tempfile
from datetime import datetime
from pathlib import Path

CAMERA_MARKER = "// BD CAMERA FOLLOWS MOUSE INTENT, NOT MOVEMENT"
MINIMAP_MARKER = "// BD MINIMAP NEAREST-CARDINAL SECTOR FIX"
SQUARE_MARKER = "// BD SQUARE JUMPER SAFE LANDING AND SPAWN FIX"


def fail(message: str) -> None:
    print(f"ERROR: {message}", file=sys.stderr)
    raise SystemExit(1)


def find_root(start: Path) -> Path:
    for candidate in [start.resolve(), *start.resolve().parents]:
        if (candidate / "Assets/_Project/Scripts/Runtime").is_dir():
            return candidate
    fail("Run this script from the Boredom-and-Dungeons project root.")


def method_span(text: str, signature: str) -> tuple[int, int]:
    start = text.find(signature)
    if start < 0:
        fail(f"Method not found: {signature}")

    opening = text.find("{", start)
    if opening < 0:
        fail(f"Opening brace not found: {signature}")

    depth = 0
    in_string = False
    in_char = False
    escaped = False
    line_comment = False
    block_comment = False
    index = opening

    while index < len(text):
        current = text[index]
        following = text[index + 1] if index + 1 < len(text) else ""

        if line_comment:
            if current == "\n":
                line_comment = False
            index += 1
            continue

        if block_comment:
            if current == "*" and following == "/":
                block_comment = False
                index += 2
                continue
            index += 1
            continue

        if in_string:
            if escaped:
                escaped = False
            elif current == "\\":
                escaped = True
            elif current == '"':
                in_string = False
            index += 1
            continue

        if in_char:
            if escaped:
                escaped = False
            elif current == "\\":
                escaped = True
            elif current == "'":
                in_char = False
            index += 1
            continue

        if current == "/" and following == "/":
            line_comment = True
            index += 2
            continue

        if current == "/" and following == "*":
            block_comment = True
            index += 2
            continue

        if current == '"':
            in_string = True
            index += 1
            continue

        if current == "'":
            in_char = True
            index += 1
            continue

        if current == "{":
            depth += 1
        elif current == "}":
            depth -= 1
            if depth == 0:
                return start, index + 1

        index += 1

    fail(f"Closing brace not found: {signature}")


def replace_method(text: str, signature: str, replacement: str) -> str:
    start, end = method_span(text, signature)
    return text[:start] + replacement + text[end:]


def patch_camera(path: Path) -> None:
    text = path.read_text(encoding="utf-8")

    if CAMERA_MARKER in text:
        print("SKIP: camera fix already installed.")
        return

    if "UpdateCameraForwardFromMovementOnly();" not in text:
        fail("Camera movement update call was not found.")

    text = text.replace(
        "UpdateCameraForwardFromMovementOnly();",
        "UpdateCameraForwardFromPlayerIntent();",
        1,
    )

    text = replace_method(
        text,
        "        private void UpdateCameraForwardFromMovementOnly()",
        r'''        // BD CAMERA FOLLOWS MOUSE INTENT, NOT MOVEMENT
        private void UpdateCameraForwardFromPlayerIntent()
        {
            Vector3 intent = ResolveCameraIntentDirection();
            intent.y = 0f;

            if (intent.sqrMagnitude < 0.001f)
            {
                cameraState = "stable: no new mouse intent";
                return;
            }

            intent.Normalize();

            float blend =
                1f - Mathf.Exp(-movementDirectionBlend * Time.deltaTime);

            smoothedMoveForward = Vector3.Slerp(
                smoothedMoveForward,
                intent,
                blend
            );

            smoothedMoveForward.y = 0f;

            if (smoothedMoveForward.sqrMagnitude < 0.001f)
                smoothedMoveForward = intent;

            smoothedMoveForward.Normalize();

            float maximumRadians =
                Mathf.Deg2Rad *
                Mathf.Max(1f, cameraYawDegreesPerSecond) *
                Time.deltaTime;

            lastForward = Vector3.RotateTowards(
                lastForward,
                smoothedMoveForward,
                maximumRadians,
                0f
            );

            lastForward.y = 0f;

            if (lastForward.sqrMagnitude < 0.001f)
                lastForward = intent;

            lastForward.Normalize();
            cameraState = "locked behind mouse/player aim intent";
        }''',
    )

    text = replace_method(
        text,
        "        private Vector3 ResolveRealMovementDirection()",
        r'''        private Vector3 ResolveCameraIntentDirection()
        {
            if (horseController != null && horseController.IsMounted)
            {
                Vector3 mountedAim =
                    horseController.LastMountedAimDirection;

                mountedAim.y = 0f;

                return mountedAim.sqrMagnitude > 0.001f
                    ? mountedAim.normalized
                    : Vector3.zero;
            }

            if (playerController == null)
                return Vector3.zero;

            Vector3 lookDirection =
                playerController.LastLookDirection;

            lookDirection.y = 0f;

            return lookDirection.sqrMagnitude > 0.001f
                ? lookDirection.normalized
                : Vector3.zero;
        }''',
    )

    path.write_text(text, encoding="utf-8")
    print("PATCHED: BDCameraFollow.cs")


def patch_minimap(path: Path) -> None:
    text = path.read_text(encoding="utf-8")

    if MINIMAP_MARKER in text:
        print("SKIP: minimap fix already installed.")
        return

    anchor = (
        "        [SerializeField] private float "
        "movementSnapThreshold = 0.35f;\n"
    )

    if anchor not in text:
        fail("Minimap movement threshold field was not found.")

    text = text.replace(
        anchor,
        anchor +
        "        // BD MINIMAP NEAREST-CARDINAL SECTOR FIX\n"
        "        [SerializeField] private float "
        "diagonalBoundaryHoldEpsilon = 0.015f;\n",
        1,
    )

    text = replace_method(
        text,
        "        private void TickDynamicPlayerUpRotation()",
        r'''        private void TickDynamicPlayerUpRotation()
        {
            if (!rotateWithPlayerDirection || player == null)
                return;

            if (!TryResolveMovementCardinalRotation(
                    out float desiredRotation))
            {
                return;
            }

            // Immediate 90-degree snap keeps the map parallel to its frame.
            currentMapRotationDegrees =
                desiredRotation + rotationOffsetDegrees;

            mapRotationInitialized = true;
        }''',
    )

    text = replace_method(
        text,
        "        private bool TryResolveMovementCardinalRotation(",
        r'''        private bool TryResolveMovementCardinalRotation(
            out float rotationDegrees)
        {
            rotationDegrees = currentMapRotationDegrees;

            Vector3 movement = Vector3.zero;

            if (horseController == null)
                horseController = FindFirstObjectByType<BDHorseController>();

            if (horseController != null &&
                horseController.IsMounted &&
                horseController.HasRideMoveInput)
            {
                movement =
                    horseController.LastMountedMovementDirection;
            }
            else if (playerController != null)
            {
                movement =
                    playerController.LastMoveWorldDirection;
            }

            movement.y = 0f;

            if (movement.sqrMagnitude <
                movementSnapThreshold * movementSnapThreshold)
            {
                return false;
            }

            movement.Normalize();

            float absoluteX = Mathf.Abs(movement.x);
            float absoluteZ = Mathf.Abs(movement.z);

            // The diagonals are the borders between the four sectors.
            // Exactly on a border, hold the previous orientation.
            if (Mathf.Abs(absoluteX - absoluteZ) <=
                Mathf.Max(0.0001f, diagonalBoundaryHoldEpsilon))
            {
                return false;
            }

            if (absoluteX > absoluteZ)
            {
                rotationDegrees =
                    movement.x > 0f ? -90f : 90f;

                return true;
            }

            rotationDegrees =
                movement.z > 0f ? 0f : 180f;

            return true;
        }''',
    )

    path.write_text(text, encoding="utf-8")
    print("PATCHED: BDMazeMinimap.cs")


def patch_square_jumper(path: Path) -> None:
    if not path.is_file():
        print("SKIP: Square Jumper is not installed.")
        return

    text = path.read_text(encoding="utf-8")

    if SQUARE_MARKER in text:
        print("SKIP: Square Jumper safety already installed.")
        return

    old_jump = '''            Vector3 targetPoint = target.position;
            targetPoint.y = transform.position.y;
'''

    new_jump = '''            // BD SQUARE JUMPER SAFE LANDING AND SPAWN FIX
            Vector3 targetPoint = target.position;
            targetPoint.y = transform.position.y;

            float bodyRadius =
                controller != null
                    ? Mathf.Max(0.35f, controller.radius)
                    : 1.25f;

            float bodyHeight =
                controller != null
                    ? Mathf.Max(bodyRadius * 2f, controller.height)
                    : 2.4f;

            float playerClearance = bodyRadius + 1.15f;

            if (BDSafeEnemyPlacement.TryFindSafeGroundPosition(
                    targetPoint,
                    transform,
                    target,
                    bodyRadius,
                    bodyHeight,
                    playerClearance,
                    4.8f,
                    out Vector3 safeLandingPoint))
            {
                targetPoint = safeLandingPoint;
            }
            else
            {
                Vector3 away =
                    transform.position - target.position;

                away.y = 0f;

                if (away.sqrMagnitude < 0.001f)
                    away = -target.forward;

                if (away.sqrMagnitude < 0.001f)
                    away = Vector3.forward;

                targetPoint =
                    target.position +
                    away.normalized * playerClearance;

                targetPoint.y = transform.position.y;
            }
'''

    if old_jump not in text:
        fail("Square Jumper jump target block was not found.")

    text = text.replace(old_jump, new_jump, 1)

    old_spawn = '''                ResolveSummonPose(
                    i,
                    out Vector3 position,
                    out Quaternion rotation
                );

                if (summonBudget != null)
'''

    new_spawn = '''                ResolveSummonPose(
                    i,
                    out Vector3 position,
                    out Quaternion rotation
                );

                if (!BDSafeEnemyPlacement.TryFindSafeGroundPosition(
                        position,
                        owner: null,
                        player: target,
                        bodyRadius: 0.55f,
                        bodyHeight: 1.8f,
                        minimumPlayerDistance: 1.55f,
                        maximumSearchRadius: 5.5f,
                        out position))
                {
                    continue;
                }

                if (summonBudget != null)
'''

    if old_spawn not in text:
        fail("Square Jumper summon block was not found.")

    text = text.replace(old_spawn, new_spawn, 1)
    path.write_text(text, encoding="utf-8")
    print("PATCHED: BDSquareJumperMiniBoss.cs")


def patch_qa(path: Path) -> None:
    if not path.is_file():
        return

    text = path.read_text(encoding="utf-8")
    old = (
        '"Jump, hard landing, bullet hell, visible double-sword attack, " +\n'
        '                "2-3 regular summons, enraged phase, death cleanup, and reward " +\n'
        '                "chest all work."'
    )
    new = (
        '"Jump, hard landing, bullet hell, visible double-sword attack, " +\n'
        '                "2-3 regular summons, no landing on the player, no spawning " +\n'
        '                "inside walls, enraged phase, cleanup, and reward chest work."'
    )

    if old in text:
        path.write_text(text.replace(old, new, 1), encoding="utf-8")
        print("PATCHED: TEST EVERYTHING enemy-safety description.")


def main() -> None:
    root = find_root(Path.cwd())
    runtime = root / "Assets/_Project/Scripts/Runtime"

    camera = runtime / "BDCameraFollow.cs"
    minimap = runtime / "BDMazeMinimap.cs"
    safety = runtime / "EnemyPlacementSafety/BDEnemyPlacementSafety.cs"
    square = (
        runtime /
        "Bosses/MiniBosses/SquareJumper/"
        "BDSquareJumperMiniBoss.cs"
    )
    qa = (
        root /
        "Assets/_Project/Scripts/Editor/Validation/"
        "BDOneClickQAWindow.cs"
    )

    for required in [camera, minimap, safety]:
        if not required.is_file():
            fail(f"Required file not found: {required}")

    backup = (
        Path(tempfile.gettempdir()) /
        f"BoredomAndDungeons_camera_minimap_enemy_safety_backup_"
        f"{datetime.now():%Y%m%d_%H%M%S}"
    )
    backup.mkdir(parents=True, exist_ok=True)

    for path in [camera, minimap, square, qa]:
        if path.is_file():
            shutil.copy2(path, backup / path.name)

    patch_camera(camera)
    patch_minimap(minimap)
    patch_square_jumper(square)
    patch_qa(qa)

    print()
    print("Camera, minimap sector and enemy placement fixes applied.")
    print(f"Backup: {backup}")
    print("Return to Unity, wait for compilation, then run TEST EVERYTHING.")


if __name__ == "__main__":
    main()
