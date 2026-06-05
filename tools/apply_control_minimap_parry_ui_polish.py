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
        if (candidate / "Assets/_Project/Scripts/Runtime").is_dir():
            return candidate

    fail("Run this script from the Boredom-and-Dungeons project root.")


def replace_once(text: str, old: str, new: str, label: str) -> str:
    count = text.count(old)

    if count != 1:
        fail(f"{label}: expected exactly one match, found {count}.")

    return text.replace(old, new, 1)


def find_method_span(text: str, signature: str) -> tuple[int, int]:
    start = text.find(signature)
    if start < 0:
        fail(f"Method not found: {signature}")

    open_brace = text.find("{", start)
    if open_brace < 0:
        fail(f"Opening brace not found: {signature}")

    depth = 0
    for index in range(open_brace, len(text)):
        if text[index] == "{":
            depth += 1
        elif text[index] == "}":
            depth -= 1
            if depth == 0:
                return start, index + 1

    fail(f"Closing brace not found: {signature}")


def replace_method(text: str, signature: str, replacement: str) -> str:
    start, end = find_method_span(text, signature)
    return text[:start] + replacement + text[end:]


def patch_square_jumper_editor(path: Path) -> None:
    if not path.is_file():
        print("SKIP: Square Jumper editor tool not installed.")
        return

    text = path.read_text(encoding="utf-8")

    if "// BD SHARED-MATERIAL EDITOR FIX" in text:
        print("SKIP: BDSquareJumperSetupTools.cs already fixed.")
        return

    text = text.replace(
        "Material material = renderer.material;",
        "// BD SHARED-MATERIAL EDITOR FIX\n"
        "            Material material = renderer.sharedMaterial;\n"
        "\n"
        "            if (material == null)\n"
        "            {\n"
        "                material = new Material(Shader.Find(\"Universal Render Pipeline/Lit\") ?? Shader.Find(\"Standard\") ?? Shader.Find(\"Unlit/Color\"));\n"
        "                renderer.sharedMaterial = material;\n"
        "            }",
        1,
    )

    # Replace any remaining editor material access in this editor tool.
    text = text.replace("renderer.material", "renderer.sharedMaterial")

    path.write_text(text, encoding="utf-8")
    print("PATCHED: BDSquareJumperSetupTools.cs")


def patch_player_controller(path: Path) -> None:
    text = path.read_text(encoding="utf-8")

    if "// BD MOUSE-INTENT DEADZONE FIX" in text:
        print("SKIP: BDPlayerController.cs already fixed.")
        return

    text = replace_once(
        text,
        '''        [SerializeField] private float mousePointAimSmoothing = 60f;
''',
        '''        [SerializeField] private float mousePointAimSmoothing = 60f;
        // BD MOUSE-INTENT DEADZONE FIX
        [SerializeField] private float mouseIntentDeadZonePixels = 9f;
''',
        "mouse intent field",
    )

    text = replace_once(
        text,
        '''        private string lastLookSource = "mouse-point";
''',
        '''        private string lastLookSource = "mouse-point";
        private Vector2 lastMouseIntentPosition;
        private bool hasMouseIntentPosition;
''',
        "mouse intent state",
    )

    text = replace_once(
        text,
        '''            Vector3 aimTarget = ResolveMouseAimDirection();
            if (aimTarget.sqrMagnitude > 0.001f)
                targetLookDirection = SmoothAimTargetDirection(aimTarget.normalized);
''',
        '''            if (ShouldAcceptMouseAimUpdate())
            {
                Vector3 aimTarget = ResolveMouseAimDirection();

                if (aimTarget.sqrMagnitude > 0.001f)
                    targetLookDirection = SmoothAimTargetDirection(aimTarget.normalized);
            }
''',
        "mouse aim update gate",
    )

    insert_at = text.find("        private Vector3 ResolveMouseAimDirection()")
    if insert_at < 0:
        fail("ResolveMouseAimDirection not found.")

    helper = r'''        private bool ShouldAcceptMouseAimUpdate()
        {
            Vector2 mousePosition = ReadMouseScreenPosition();

            if (!hasMouseIntentPosition)
            {
                lastMouseIntentPosition = mousePosition;
                hasMouseIntentPosition = true;
                return true;
            }

            float minPixels = Mathf.Max(0.5f, mouseIntentDeadZonePixels);
            float sqrDelta =
                (mousePosition - lastMouseIntentPosition).sqrMagnitude;

            if (sqrDelta < minPixels * minPixels)
                return false;

            lastMouseIntentPosition = mousePosition;
            return true;
        }

        private static Vector2 ReadMouseScreenPosition()
        {
#if ENABLE_INPUT_SYSTEM
            Mouse mouse = Mouse.current;

            if (mouse != null)
                return mouse.position.ReadValue();
#endif

#if ENABLE_LEGACY_INPUT_MANAGER
            return Input.mousePosition;
#else
            return new Vector2(
                Screen.width * 0.5f,
                Screen.height * 0.5f
            );
#endif
        }

'''
    text = text[:insert_at] + helper + text[insert_at:]
    path.write_text(text, encoding="utf-8")
    print("PATCHED: BDPlayerController.cs")


def patch_minimap(path: Path) -> None:
    text = path.read_text(encoding="utf-8")

    if "// BD MINIMAP 90-DEGREE MOVEMENT SNAP FIX" in text:
        print("SKIP: BDMazeMinimap.cs already fixed.")
        return

    text = replace_once(
        text,
        '''        [SerializeField] private float rotationOffsetDegrees = 0f;
''',
        '''        [SerializeField] private float rotationOffsetDegrees = 0f;
        // BD MINIMAP 90-DEGREE MOVEMENT SNAP FIX
        [SerializeField] private bool snapToMovementCardinals = true;
        [SerializeField] private float movementSnapThreshold = 0.35f;
''',
        "minimap snap fields",
    )

    text = replace_method(
        text,
        "        private void TickDynamicPlayerUpRotation()",
        r'''        private void TickDynamicPlayerUpRotation()
        {
            if (!rotateWithPlayerDirection || player == null)
                return;

            float desiredRotation;

            if (snapToMovementCardinals &&
                TryResolveMovementCardinalRotation(out desiredRotation))
            {
                desiredRotation += rotationOffsetDegrees;
            }
            else if (snapToMovementCardinals)
            {
                return;
            }
            else
            {
                Vector3 direction = ResolvePlayerViewDirection();
                direction.y = 0f;

                if (direction.sqrMagnitude < 0.001f)
                    return;

                direction.Normalize();

                float playerYaw =
                    Mathf.Atan2(
                        direction.x,
                        direction.z
                    ) * Mathf.Rad2Deg;

                desiredRotation =
                    -playerYaw + rotationOffsetDegrees;
            }

            if (!mapRotationInitialized)
            {
                currentMapRotationDegrees = desiredRotation;
                mapRotationInitialized = true;
                return;
            }

            currentMapRotationDegrees =
                Mathf.MoveTowardsAngle(
                    currentMapRotationDegrees,
                    desiredRotation,
                    Mathf.Max(
                        90f,
                        rotationSpeedDegreesPerSecond
                    ) * Time.unscaledDeltaTime
                );
        }''',
    )

    insert_at = text.find("        private Vector3 ResolvePlayerViewDirection()")
    if insert_at < 0:
        fail("ResolvePlayerViewDirection not found.")

    helper = r'''        private bool TryResolveMovementCardinalRotation(
            out float rotationDegrees)
        {
            rotationDegrees = currentMapRotationDegrees;

            Vector2 moveInput =
                playerController != null
                    ? playerController.LastMoveInput
                    : Vector2.zero;

            if (horseController == null)
                horseController = FindFirstObjectByType<BDHorseController>();

            if (horseController != null &&
                horseController.IsMounted &&
                horseController.HasRideMoveInput)
            {
                Vector3 mountedDirection =
                    horseController.LastMountedMovementDirection;

                mountedDirection.y = 0f;

                if (mountedDirection.sqrMagnitude >
                    movementSnapThreshold * movementSnapThreshold)
                {
                    float yaw =
                        Mathf.Atan2(
                            mountedDirection.x,
                            mountedDirection.z
                        ) * Mathf.Rad2Deg;

                    rotationDegrees = -Mathf.Round(yaw / 90f) * 90f;
                    return true;
                }
            }

            if (moveInput.sqrMagnitude <
                movementSnapThreshold * movementSnapThreshold)
            {
                return false;
            }

            if (Mathf.Abs(moveInput.x) > Mathf.Abs(moveInput.y))
            {
                rotationDegrees = moveInput.x > 0f ? -90f : 90f;
                return true;
            }

            rotationDegrees = moveInput.y > 0f ? 0f : 180f;
            return true;
        }

'''
    text = text[:insert_at] + helper + text[insert_at:]

    path.write_text(text, encoding="utf-8")
    print("PATCHED: BDMazeMinimap.cs")


def patch_camera(path: Path) -> None:
    text = path.read_text(encoding="utf-8")

    if "// BD FORWARD SCREEN SPACE LOOKAHEAD FIX" in text:
        print("SKIP: BDCameraFollow.cs already fixed.")
        return

    text = replace_once(
        text,
        '''        [SerializeField] private float lookAhead = 6.75f;
''',
        '''        [SerializeField] private float lookAhead = 8.25f;
        // BD FORWARD SCREEN SPACE LOOKAHEAD FIX
        [SerializeField] private float extraForwardCompositionLookAhead = 2.35f;
''',
        "camera lookahead field",
    )

    text = replace_once(
        text,
        '''            Vector3 lookPoint = targetPosition + forward.normalized * lookAhead;
''',
        '''            Vector3 lookPoint =
                targetPosition +
                forward.normalized *
                (lookAhead + Mathf.Max(0f, extraForwardCompositionLookAhead));
''',
        "camera look point",
    )

    path.write_text(text, encoding="utf-8")
    print("PATCHED: BDCameraFollow.cs")


def patch_game_hud(path: Path) -> None:
    text = path.read_text(encoding="utf-8")

    if "// BD EXPANDED AMMO UI FIX" in text:
        print("SKIP: BDGameHud.cs already fixed.")
        return

    text = replace_once(
        text,
        '''            float panelWidth = 258f;
            float panelHeight = 104f;
''',
        '''            // BD EXPANDED AMMO UI FIX
            int visibleAmmoCapacity = Mathf.Clamp(maxAmmo, 1, 6);
            float panelWidth = Mathf.Max(258f, 212f + visibleAmmoCapacity * 28f);
            float panelHeight = 112f;
''',
        "ammo panel sizing",
    )

    text = replace_once(
        text,
        '''            DrawAmmoPips(new Rect(panel.x + 96f, panel.y + 27f, 92f, 26f), ammo, maxAmmo, reloading);
''',
        '''            DrawAmmoPips(
                new Rect(
                    panel.x + 96f,
                    panel.y + 25f,
                    panel.width - 158f,
                    34f
                ),
                ammo,
                maxAmmo,
                reloading
            );
''',
        "ammo pips rect",
    )

    text = replace_method(
        text,
        "        private void DrawAmmoPips(Rect rect, int ammo, int maxAmmo, bool reloading)",
        r'''        private void DrawAmmoPips(
            Rect rect,
            int ammo,
            int maxAmmo,
            bool reloading)
        {
            maxAmmo = Mathf.Clamp(maxAmmo, 1, 6);
            ammo = Mathf.Clamp(ammo, 0, maxAmmo);

            float gap = 6f;
            float size =
                Mathf.Min(
                    20f,
                    (rect.width - gap * (maxAmmo - 1)) / maxAmmo
                );

            float totalWidth =
                maxAmmo * size + (maxAmmo - 1) * gap;

            float startX = rect.x + Mathf.Max(0f, (rect.width - totalWidth) * 0.5f);
            float y = rect.y + (rect.height - size) * 0.5f;

            for (int i = 0; i < maxAmmo; i++)
            {
                Rect pip =
                    new Rect(
                        startX + i * (size + gap),
                        y,
                        size,
                        size
                    );

                bool filled = i < ammo;

                Color background =
                    new Color(0.02f, 0.035f, 0.04f, 1f);

                Color fill = filled
                    ? new Color(0.35f, 0.95f, 1f, 1f)
                    : new Color(0.045f, 0.09f, 0.105f, 1f);

                if (!filled && reloading)
                {
                    float pulse =
                        0.55f +
                        Mathf.Sin(
                            Time.time * 9f + i * 0.75f
                        ) * 0.22f;

                    fill =
                        new Color(
                            1f,
                            0.68f,
                            0.16f,
                            Mathf.Clamp01(pulse)
                        );
                }

                DrawBar(pip, 1f, background, background);
                DrawAmmoBulletIcon(pip, fill, filled);
            }
        }

        private void DrawAmmoBulletIcon(
            Rect rect,
            Color color,
            bool filled)
        {
            Color old = GUI.color;

            float capHeight = rect.height * 0.32f;
            Rect body =
                new Rect(
                    rect.x + rect.width * 0.22f,
                    rect.y + capHeight * 0.65f,
                    rect.width * 0.56f,
                    rect.height - capHeight * 0.65f
                );

            Rect cap =
                new Rect(
                    rect.x + rect.width * 0.32f,
                    rect.y,
                    rect.width * 0.36f,
                    capHeight
                );

            GUI.color = color;
            GUI.DrawTexture(body, whiteTexture);
            GUI.DrawTexture(cap, whiteTexture);

            GUI.color = filled
                ? new Color(1f, 1f, 1f, 0.55f)
                : new Color(1f, 1f, 1f, 0.18f);

            GUI.Box(body, GUIContent.none);
            GUI.Box(cap, GUIContent.none);
            GUI.color = old;
        }''',
    )

    path.write_text(text, encoding="utf-8")
    print("PATCHED: BDGameHud.cs")


def patch_parry_state(path: Path) -> None:
    text = path.read_text(encoding="utf-8")

    if "// BD PARRY DURATION TUNING FIX" in text:
        print("SKIP: BDPlayerParryState.cs already fixed.")
        return

    text = replace_once(
        text,
        '''        [SerializeField] private float normalParryFreezeDuration = 1.0f;
        [SerializeField] private float upgradedParryFreezeDuration = 2.0f;
''',
        '''        // BD PARRY DURATION TUNING FIX
        [SerializeField] private float normalParryFreezeDuration = 1.5f;
        [SerializeField] private float upgradedParryFreezeDuration = 2.5f;
''',
        "parry duration fields",
    )

    path.write_text(text, encoding="utf-8")
    print("PATCHED: BDPlayerParryState.cs")


def patch_parry_system(path: Path) -> None:
    text = path.read_text(encoding="utf-8")

    if "// BD PARRY RING VISUAL REDESIGN FIX" in text:
        print("SKIP: BDParrySystem.cs already fixed.")
        return

    text = replace_once(
        text,
        '''            SpawnParryBurst(player.position + Vector3.up * 0.95f, heavy);
            FreezeWorldExceptPlayer(player);
''',
        '''            // BD PARRY RING VISUAL REDESIGN FIX
            SpawnParryBurst(player.position + Vector3.up * 0.95f, heavy);

            BDPlayerParryState state =
                player.GetComponent<BDPlayerParryState>();

            BDParryTimeRingVisual.Spawn(
                player,
                freezeDuration,
                heavy,
                state != null && state.HasExtendedFreeze
            );

            FreezeWorldExceptPlayer(player);
''',
        "parry ring spawn",
    )

    loading_bar = '''                float progressWidth = Mathf.Min(460f, Screen.width * 0.42f);
                Rect progressBack = new Rect((Screen.width - progressWidth) * 0.5f, Screen.height * 0.20f, progressWidth, 10f);
                GUI.color = new Color(0f, 0f, 0f, 0.72f);
                GUI.DrawTexture(progressBack, whiteTexture);
                Rect progressFill = progressBack;
                progressFill.width *= 1f - Progress01;
                GUI.color = new Color(0.28f, 0.90f, 1f, 1f);
                GUI.DrawTexture(progressFill, whiteTexture);

'''

    if loading_bar in text:
        text = text.replace(
            loading_bar,
            '''                // The old loading bar was intentionally removed.
                // Remaining time is now shown by the in-world ring around the player.

''',
            1,
        )
    else:
        print("WARNING: parry loading bar block not found; ring still added.")

    path.write_text(text, encoding="utf-8")
    print("PATCHED: BDParrySystem.cs")


def main() -> None:
    root = find_project_root(Path.cwd())
    runtime = root / "Assets/_Project/Scripts/Runtime"
    editor = root / "Assets/_Project/Scripts/Editor"

    required = [
        runtime / "BDPlayerController.cs",
        runtime / "BDMazeMinimap.cs",
        runtime / "BDCameraFollow.cs",
        runtime / "BDGameHud.cs",
        runtime / "Combat/BDPlayerParryState.cs",
        runtime / "Combat/BDParrySystem.cs",
    ]

    optional_square_editor = (
        editor /
        "BossesMiniBosses/BDSquareJumperSetupTools.cs"
    )

    for path in required:
        if not path.is_file():
            fail(f"Required file not found: {path}")

    new_ring = (
        runtime /
        "Combat/ParryVisuals/BDParryTimeRingVisual.cs"
    )

    if not new_ring.is_file():
        fail(
            "BDParryTimeRingVisual.cs is missing. "
            "Extract the full ZIP first."
        )

    backup = (
        Path(tempfile.gettempdir()) /
        f"BoredomAndDungeons_control_minimap_parry_ui_backup_"
        f"{datetime.now():%Y%m%d_%H%M%S}"
    )
    backup.mkdir(parents=True, exist_ok=True)

    for path in [*required, optional_square_editor]:
        if path.is_file():
            shutil.copy2(path, backup / path.name)

    patch_player_controller(required[0])
    patch_minimap(required[1])
    patch_camera(required[2])
    patch_game_hud(required[3])
    patch_parry_state(required[4])
    patch_parry_system(required[5])
    patch_square_jumper_editor(optional_square_editor)

    print()
    print("Control, minimap, camera, ammo UI, parry visual and")
    print("Square Jumper editor material fixes were applied.")
    print(f"Backup: {backup}")
    print("Next: return to Unity and wait for compilation.")


if __name__ == "__main__":
    main()
