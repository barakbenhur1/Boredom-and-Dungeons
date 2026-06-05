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
        runtime = candidate / "Assets/_Project/Scripts/Runtime"
        if runtime.is_dir():
            return candidate

    fail(
        "Run this script from the Boredom-and-Dungeons project root "
        "after extracting the complete ZIP there."
    )


def find_csharp_method_span(
    text: str,
    signature: str,
) -> tuple[int, int]:
    start = text.find(signature)

    if start < 0:
        fail(f"Method signature not found: {signature}")

    open_brace = text.find("{", start)

    if open_brace < 0:
        fail(f"Opening brace not found for: {signature}")

    depth = 0
    index = open_brace
    in_string = False
    in_char = False
    in_line_comment = False
    in_block_comment = False
    verbatim_string = False
    escaped = False

    while index < len(text):
        current = text[index]
        next_char = text[index + 1] if index + 1 < len(text) else ""

        if in_line_comment:
            if current == "\n":
                in_line_comment = False
            index += 1
            continue

        if in_block_comment:
            if current == "*" and next_char == "/":
                in_block_comment = False
                index += 2
                continue
            index += 1
            continue

        if in_string:
            if verbatim_string:
                if current == '"' and next_char == '"':
                    index += 2
                    continue

                if current == '"':
                    in_string = False
                    verbatim_string = False

                index += 1
                continue

            if escaped:
                escaped = False
                index += 1
                continue

            if current == "\\":
                escaped = True
                index += 1
                continue

            if current == '"':
                in_string = False

            index += 1
            continue

        if in_char:
            if escaped:
                escaped = False
                index += 1
                continue

            if current == "\\":
                escaped = True
                index += 1
                continue

            if current == "'":
                in_char = False

            index += 1
            continue

        if current == "/" and next_char == "/":
            in_line_comment = True
            index += 2
            continue

        if current == "/" and next_char == "*":
            in_block_comment = True
            index += 2
            continue

        if current == "@" and next_char == '"':
            in_string = True
            verbatim_string = True
            index += 2
            continue

        if current == '"':
            in_string = True
            verbatim_string = False
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

    fail(f"Closing brace not found for: {signature}")


def replace_csharp_method(
    text: str,
    signature: str,
    replacement: str,
) -> str:
    start, end = find_csharp_method_span(text, signature)
    return text[:start] + replacement + text[end:]


def insert_after_once(
    text: str,
    marker: str,
    insertion: str,
    label: str,
) -> str:
    count = text.count(marker)

    if count != 1:
        fail(
            f"{label}: expected exactly one marker, found {count}."
        )

    return text.replace(marker, marker + insertion, 1)


def patch_player_combat(path: Path) -> None:
    text = path.read_text(encoding="utf-8")

    if "// BD AUTO-RELOAD WATCHDOG FIX" in text:
        print("SKIP: BDPlayerCombat.cs already contains the reload fix.")
        return

    if "private void TickChargedRangedAttack()" not in text:
        fail(
            "Charged Shot was not found in BDPlayerCombat.cs. "
            "Apply the Charged Shot update first."
        )

    update_start, update_end = find_csharp_method_span(
        text,
        "        private void Update()",
    )

    update_method = text[update_start:update_end]
    update_marker = "            TickChargedRangedAttack();\n"

    if update_marker not in update_method:
        fail(
            "TickChargedRangedAttack call was not found in Update."
        )

    update_method = update_method.replace(
        update_marker,
        update_marker +
        "            EnsureAutomaticReloadForEmptyMagazine();\n",
        1,
    )

    text = text[:update_start] + update_method + text[update_end:]

    reload_helpers = r'''
        // BD AUTO-RELOAD WATCHDOG FIX
        private void EnsureAutomaticReloadForEmptyMagazine()
        {
            if (rangedAmmo > 0 || reloading)
                return;

            if (chargedShotCharging)
                return;

            StartReloadFromEmptyMagazine(
                "empty magazine watchdog"
            );
        }

        private void StartReloadFromEmptyMagazine(string reason)
        {
            if (rangedAmmo > 0)
                return;

            reloading = true;
            reloadEndsAt =
                Time.time + EffectiveRangedReloadDuration;

            lastCombatAction =
                $"ranged reload: {reason}";
        }

'''

    tick_reload_signature = "        private void TickReload()"
    tick_reload_index = text.find(tick_reload_signature)

    if tick_reload_index < 0:
        fail("TickReload method was not found.")

    text = (
        text[:tick_reload_index] +
        reload_helpers +
        text[tick_reload_index:]
    )

    text = replace_csharp_method(
        text,
        "        private void BeginReloadIfNeeded()",
        '''        private void BeginReloadIfNeeded()
        {
            if (rangedAmmo > 0 || reloading)
                return;

            StartReloadFromEmptyMagazine(
                "requested automatically"
            );
        }''',
    )

    charged_reload_method = r'''        private void StartReloadImmediatelyAfterChargedShot()
        {
            rangedAmmo = 0;
            reloading = false;

            StartReloadFromEmptyMagazine(
                "charged shot emptied magazine"
            );
        }'''

    if "private void StartReloadImmediatelyAfterChargedShot()" in text:
        text = replace_csharp_method(
            text,
            "        private void StartReloadImmediatelyAfterChargedShot()",
            charged_reload_method,
        )
    else:
        fire_start, fire_end = find_csharp_method_span(
            text,
            "        private void FireChargedShot(Vector3 direction)",
        )

        fire_method = text[fire_start:fire_end]

        possible_blocks = [
            '''            if (rangedAmmo <= 0)
                BeginReloadIfNeeded();
''',
            '''            if (rangedAmmo <= 0)
            {
                BeginReloadIfNeeded();
            }
''',
        ]

        replaced = False

        for old_block in possible_blocks:
            if old_block in fire_method:
                fire_method = fire_method.replace(
                    old_block,
                    '''            StartReloadImmediatelyAfterChargedShot();
''',
                    1,
                )
                replaced = True
                break

        if not replaced:
            closing_brace = fire_method.rfind("}")

            if closing_brace < 0:
                fail("Could not patch the end of FireChargedShot.")

            fire_method = (
                fire_method[:closing_brace] +
                "            StartReloadImmediatelyAfterChargedShot();\n" +
                fire_method[closing_brace:]
            )

        text = text[:fire_start] + fire_method + text[fire_end:]

        cancel_index = text.find(
            "        private void CancelChargedShot()"
        )

        if cancel_index < 0:
            fail("CancelChargedShot method was not found.")

        text = (
            text[:cancel_index] +
            charged_reload_method +
            "\n\n" +
            text[cancel_index:]
        )

    path.write_text(text, encoding="utf-8")
    print("PATCHED: BDPlayerCombat.cs")


def patch_maze_minimap(path: Path) -> None:
    text = path.read_text(encoding="utf-8")

    if "// BD PLAYER-UP DYNAMIC MINIMAP FIX" in text:
        print("SKIP: BDMazeMinimap.cs already contains the dynamic fix.")
        return

    display_marker = (
        "        [SerializeField] private float "
        "nearestDiscoveryMaxDistance = 34f;\n"
    )

    text = insert_after_once(
        text,
        display_marker,
        '''
        [Header("Dynamic Player-Up Rotation")]
        // BD PLAYER-UP DYNAMIC MINIMAP FIX
        [SerializeField] private bool rotateWithPlayerDirection = true;
        [SerializeField] private float rotationSpeedDegreesPerSecond = 900f;
        [SerializeField] private float rotationOffsetDegrees = 0f;
''',
        "minimap rotation settings",
    )

    state_marker = "        private GUIStyle labelStyle;\n"

    text = insert_after_once(
        text,
        state_marker,
        '''        private BDPlayerController playerController;
        private BDHorseController horseController;
        private float currentMapRotationDegrees;
        private bool mapRotationInitialized;
''',
        "minimap rotation runtime state",
    )

    resolve_method = r'''        private void ResolvePlayerReference()
        {
            if (player == null)
            {
                BDPlayerMarker marker =
                    FindFirstObjectByType<BDPlayerMarker>();

                if (marker != null)
                    player = marker.transform;
                else
                    player = BDTargetFinder.FindPlayer();
            }

            if (player != null && playerController == null)
            {
                playerController =
                    player.GetComponent<BDPlayerController>();
            }
        }'''

    text = replace_csharp_method(
        text,
        "        private void ResolvePlayerReference()",
        resolve_method,
    )

    update_start, update_end = find_csharp_method_span(
        text,
        "        private void Update()",
    )

    update_method = text[update_start:update_end]
    update_marker = "            ResolvePlayerReference();\n"

    if update_marker not in update_method:
        fail(
            "ResolvePlayerReference call was not found in minimap Update."
        )

    update_method = update_method.replace(
        update_marker,
        update_marker +
        "            TickDynamicPlayerUpRotation();\n",
        1,
    )

    text = text[:update_start] + update_method + text[update_end:]

    on_gui_start, on_gui_end = find_csharp_method_span(
        text,
        "        private void OnGUI()",
    )

    on_gui_method = text[on_gui_start:on_gui_end]

    old_draw_block = '''            Rect mapRect = new Rect(rect.x + 12f, rect.y + 34f, rect.width - 24f, rect.height - 46f);
            DrawRooms(mapRect);
            DrawMarker(mapRect, player, playerColor, markerSize);
            DrawMarker(mapRect, horse, horseColor, markerSize * 0.85f);
'''

    if old_draw_block not in on_gui_method:
        fail(
            "The expected minimap draw block was not found."
        )

    new_draw_block = '''            Rect mapRect = new Rect(
                rect.x + 12f,
                rect.y + 34f,
                rect.width - 24f,
                rect.height - 46f
            );

            Matrix4x4 originalGuiMatrix = GUI.matrix;
            Vector2 rotationPivot = mapRect.center;

            if (TryResolveMapPoint(
                    mapRect,
                    player,
                    out Vector2 playerMapPoint))
            {
                rotationPivot = playerMapPoint;
            }

            if (rotateWithPlayerDirection)
            {
                GUIUtility.RotateAroundPivot(
                    currentMapRotationDegrees,
                    rotationPivot
                );
            }

            DrawRooms(mapRect);
            DrawMarker(
                mapRect,
                horse,
                horseColor,
                markerSize * 0.85f
            );

            GUI.matrix = originalGuiMatrix;

            DrawMarker(
                mapRect,
                player,
                playerColor,
                markerSize
            );
'''

    on_gui_method = on_gui_method.replace(
        old_draw_block,
        new_draw_block,
        1,
    )

    text = text[:on_gui_start] + on_gui_method + text[on_gui_end:]

    draw_marker_replacement = r'''        private void DrawMarker(
            Rect mapRect,
            Transform target,
            Color color,
            float size)
        {
            if (!TryResolveMapPoint(
                    mapRect,
                    target,
                    out Vector2 position))
            {
                return;
            }

            DrawRect(
                new Rect(
                    position.x - size * 0.5f,
                    position.y - size * 0.5f,
                    size,
                    size
                ),
                color
            );
        }

        private bool TryResolveMapPoint(
            Rect mapRect,
            Transform target,
            out Vector2 position)
        {
            position = mapRect.center;

            if (target == null)
                return false;

            BDMinimapRoom nearest =
                FindNearestDiscoveredRoom(target.position);

            if (nearest == null)
                return false;

            Vector2 normalized =
                WorldToCellLocal(nearest, target.position);

            int widthCells =
                Mathf.Max(1, maxX - minX + 1);

            int heightCells =
                Mathf.Max(1, maxY - minY + 1);

            float cellSize = Mathf.Min(
                mapRect.width / widthCells,
                mapRect.height / heightCells
            );

            Rect roomRect =
                CellToRect(mapRect, nearest.Cell, cellSize);

            position = new Vector2(
                roomRect.x + normalized.x * roomRect.width,
                roomRect.y +
                    (1f - normalized.y) *
                    roomRect.height
            );

            return true;
        }'''

    text = replace_csharp_method(
        text,
        "        private void DrawMarker(Rect mapRect, Transform target, Color color, float size)",
        draw_marker_replacement,
    )

    rotation_methods = r'''
        private void TickDynamicPlayerUpRotation()
        {
            if (!rotateWithPlayerDirection || player == null)
                return;

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

            float desiredRotation =
                -playerYaw + rotationOffsetDegrees;

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
        }

        private Vector3 ResolvePlayerViewDirection()
        {
            if (player == null)
                return Vector3.forward;

            if (playerController == null)
            {
                playerController =
                    player.GetComponent<BDPlayerController>();
            }

            if (playerController != null)
            {
                Vector3 lookDirection =
                    playerController.LastLookDirection;

                lookDirection.y = 0f;

                if (lookDirection.sqrMagnitude > 0.001f)
                    return lookDirection.normalized;
            }

            if (horseController == null)
            {
                horseController =
                    FindFirstObjectByType<BDHorseController>();
            }

            if (horseController != null &&
                horseController.IsMounted &&
                horseController.Rider != null)
            {
                Transform rider = horseController.Rider;

                bool sameRider =
                    rider == player ||
                    player.IsChildOf(rider) ||
                    rider.IsChildOf(player);

                if (sameRider)
                {
                    Vector3 mountedDirection =
                        horseController.LastMountedAimDirection;

                    mountedDirection.y = 0f;

                    if (mountedDirection.sqrMagnitude > 0.001f)
                        return mountedDirection.normalized;

                    Vector3 horseForward =
                        horseController.transform.forward;

                    horseForward.y = 0f;

                    if (horseForward.sqrMagnitude > 0.001f)
                        return horseForward.normalized;
                }
            }

            Vector3 playerForward = player.forward;
            playerForward.y = 0f;

            if (playerForward.sqrMagnitude < 0.001f)
                return Vector3.forward;

            return playerForward.normalized;
        }

'''

    on_gui_signature = "        private void OnGUI()"
    on_gui_index = text.find(on_gui_signature)

    if on_gui_index < 0:
        fail("Minimap OnGUI method was not found.")

    text = (
        text[:on_gui_index] +
        rotation_methods +
        text[on_gui_index:]
    )

    path.write_text(text, encoding="utf-8")
    print("PATCHED: BDMazeMinimap.cs")


def remove_obsolete_camera_alignment(root: Path) -> None:
    obsolete = [
        root /
        "Assets/_Project/Scripts/Runtime/UI/Minimap/"
        "BDMinimapPerspectiveAlignment.cs",
        root /
        "Assets/_Project/Scripts/Runtime/UI/Minimap/"
        "BDMinimapPerspectiveAlignment.cs.meta",
    ]

    for path in obsolete:
        if path.exists():
            path.unlink()
            print(f"REMOVED obsolete file: {path.relative_to(root)}")


def main() -> None:
    root = find_project_root(Path.cwd())
    runtime = root / "Assets/_Project/Scripts/Runtime"

    combat = runtime / "BDPlayerCombat.cs"
    minimap = runtime / "BDMazeMinimap.cs"

    for path in (combat, minimap):
        if not path.is_file():
            fail(f"Required file not found: {path}")

    backup_root = (
        Path(tempfile.gettempdir()) /
        f"BoredomAndDungeons_reload_player_up_minimap_backup_"
        f"{datetime.now():%Y%m%d_%H%M%S}"
    )
    backup_root.mkdir(parents=True, exist_ok=True)

    shutil.copy2(combat, backup_root / combat.name)
    shutil.copy2(minimap, backup_root / minimap.name)

    patch_player_combat(combat)
    patch_maze_minimap(minimap)
    remove_obsolete_camera_alignment(root)

    print()
    print("Automatic reload watchdog and dynamic player-up minimap")
    print("were applied successfully.")
    print(f"Backup: {backup_root}")
    print("Next: return to Unity and wait for compilation.")


if __name__ == "__main__":
    main()
