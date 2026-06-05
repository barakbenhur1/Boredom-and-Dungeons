#!/usr/bin/env python3
from __future__ import annotations

import re
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


def find_method_span(
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

    for index in range(open_brace, len(text)):
        character = text[index]

        if character == "{":
            depth += 1
        elif character == "}":
            depth -= 1

            if depth == 0:
                return start, index + 1

    fail(f"Closing brace not found for: {signature}")


def replace_method(
    text: str,
    signature: str,
    replacement: str,
) -> str:
    start, end = find_method_span(text, signature)
    return text[:start] + replacement + text[end:]


def insert_once(
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

    return text.replace(
        marker,
        marker + insertion,
        1,
    )


def patch_player_combat(path: Path) -> None:
    text = path.read_text(encoding="utf-8")

    if "// BD TAP-VS-HOLD CHARGED SHOT FIX" in text:
        print("SKIP: BDPlayerCombat.cs already contains this fix.")
        return

    if "// BD CHARGED SHOT SYSTEM" not in text:
        fail(
            "BDPlayerCombat.cs does not contain the Charged Shot "
            "system. Apply the previous Charged Shot update first."
        )

    field_marker = (
        '        [SerializeField] private bool '
        'enableChargedShot = true;\n'
    )

    text = insert_once(
        text,
        field_marker,
        '''        // BD TAP-VS-HOLD CHARGED SHOT FIX
        [SerializeField] private float chargedShotHoldThreshold = 0.22f;
''',
        "charged hold-threshold field",
    )

    runtime_marker = (
        '        private BDChargedShotChargeVisual '
        'chargedShotChargeVisual;\n'
    )

    text = insert_once(
        text,
        runtime_marker,
        '''        private bool rangedPressPending;
        private float rangedPressStartedAtUnscaled;
''',
        "charged tap/hold runtime state",
    )

    text = replace_method(
        text,
        "        private void OnDisable()",
        '''        private void OnDisable()
        {
            ClearPendingRangedPress();
            CancelChargedShot();
        }''',
    )

    text = replace_method(
        text,
        "        private void TickChargedRangedAttack()",
        r'''        private void TickChargedRangedAttack()
        {
            if (!enableChargedShot)
            {
                if (ReadRangedAttackPressed())
                    TryRangedAttack();

                return;
            }

            if (chargedShotCharging)
            {
                TickActiveChargedShot();
                return;
            }

            if (rangedPressPending)
            {
                TickPendingRangedPress();
                return;
            }

            if (!ReadRangedAttackPressed())
                return;

            if (Time.time < nextRangedAllowedAt ||
                reloading)
            {
                return;
            }

            if (rangedAmmo <= 0)
            {
                BeginReloadIfNeeded();
                return;
            }

            if (rangedAmmo == 1)
            {
                TryRangedAttack();
                return;
            }

            BeginPendingRangedPress();
        }''',
    )

    pending_methods = r'''
        private void BeginPendingRangedPress()
        {
            rangedPressPending = true;
            rangedPressStartedAtUnscaled = Time.unscaledTime;
            lastCombatAction = "ranged press pending";
        }

        private void TickPendingRangedPress()
        {
            if (!rangedPressPending)
                return;

            if (reloading || rangedAmmo <= 0)
            {
                ClearPendingRangedPress();
                BeginReloadIfNeeded();
                return;
            }

            float heldDuration =
                Time.unscaledTime -
                rangedPressStartedAtUnscaled;

            if (ReadRangedAttackReleased() ||
                !ReadRangedAttackHeld())
            {
                ClearPendingRangedPress();
                TryRangedAttack();
                return;
            }

            if (heldDuration <
                Mathf.Max(0.05f, chargedShotHoldThreshold))
            {
                return;
            }

            ClearPendingRangedPress();
            BeginChargedShot();
        }

        private void ClearPendingRangedPress()
        {
            rangedPressPending = false;
            rangedPressStartedAtUnscaled = 0f;
        }

'''

    begin_signature = "        private void BeginChargedShot()"
    begin_index = text.find(begin_signature)

    if begin_index < 0:
        fail("BeginChargedShot method was not found.")

    text = (
        text[:begin_index] +
        pending_methods +
        text[begin_index:]
    )

    fire_start, fire_end = find_method_span(
        text,
        "        private void FireChargedShot(Vector3 direction)",
    )

    fire_method = text[fire_start:fire_end]

    reserved_block = '''            int ammoToConsume =
                Mathf.Clamp(
                    chargedShotReservedAmmo,
                    1,
                    rangedAmmo
                );
'''

    if reserved_block not in fire_method:
        fail(
            "The charged-shot ammo calculation no longer matches "
            "the expected version."
        )

    fire_method = fire_method.replace(
        reserved_block,
        '''            // Consume every projectile currently left.
            int ammoToConsume = Mathf.Max(0, rangedAmmo);
''',
        1,
    )

    reload_block = '''            if (rangedAmmo <= 0)
                BeginReloadIfNeeded();
'''

    if reload_block not in fire_method:
        fail(
            "The charged-shot reload block was not found."
        )

    fire_method = fire_method.replace(
        reload_block,
        '''            StartReloadImmediatelyAfterChargedShot();
''',
        1,
    )

    text = text[:fire_start] + fire_method + text[fire_end:]

    reload_method = r'''
        private void StartReloadImmediatelyAfterChargedShot()
        {
            rangedAmmo = 0;
            reloading = true;
            reloadEndsAt =
                Time.time + EffectiveRangedReloadDuration;

            lastCombatAction =
                "charged shot fired; automatic reload";
        }

'''

    cancel_signature = "        private void CancelChargedShot()"
    cancel_index = text.find(cancel_signature)

    if cancel_index < 0:
        fail("CancelChargedShot method was not found.")

    text = (
        text[:cancel_index] +
        reload_method +
        text[cancel_index:]
    )

    path.write_text(text, encoding="utf-8")
    print("PATCHED: BDPlayerCombat.cs")


def patch_stage17_tick_count(root: Path) -> None:
    matches = list(
        root.rglob("BDStage17MiniBossPlanGenerator.cs")
    )

    if not matches:
        print(
            "WARNING: BDStage17MiniBossPlanGenerator.cs was not found. "
            "Stage 17 may not be installed yet."
        )
        return

    for path in matches:
        text = path.read_text(encoding="utf-8")

        fixed = re.sub(
            r"(?<!System\.)\bEnvironment\.TickCount\b",
            "System.Environment.TickCount",
            text,
        )

        if fixed == text:
            print(
                f"SKIP: {path.relative_to(root)} already uses "
                "System.Environment.TickCount."
            )
            continue

        path.write_text(fixed, encoding="utf-8")
        print(
            f"PATCHED: {path.relative_to(root)} "
            "(System.Environment.TickCount)"
        )


def main() -> None:
    root = find_project_root(Path.cwd())
    runtime = root / "Assets/_Project/Scripts/Runtime"

    combat = runtime / "BDPlayerCombat.cs"
    minimap_alignment = (
        runtime /
        "UI/Minimap/BDMinimapPerspectiveAlignment.cs"
    )

    if not combat.is_file():
        fail(f"Required file not found: {combat}")

    if not minimap_alignment.is_file():
        fail(
            "BDMinimapPerspectiveAlignment.cs is missing. "
            "Extract the complete ZIP into the project root first."
        )

    backup_root = (
        Path(tempfile.gettempdir()) /
        f"BoredomAndDungeons_tap_charge_minimap_fix_backup_"
        f"{datetime.now():%Y%m%d_%H%M%S}"
    )
    backup_root.mkdir(parents=True, exist_ok=True)

    shutil.copy2(combat, backup_root / combat.name)

    stage17_files = list(
        root.rglob("BDStage17MiniBossPlanGenerator.cs")
    )

    for stage17_file in stage17_files:
        shutil.copy2(
            stage17_file,
            backup_root / stage17_file.name,
        )

    patch_player_combat(combat)
    patch_stage17_tick_count(root)

    print()
    print("Tap/hold shooting, automatic reload, minimap alignment,")
    print("and Stage 17 TickCount fixes were applied.")
    print(f"Backup: {backup_root}")
    print("Next: return to Unity and wait for compilation.")


if __name__ == "__main__":
    main()
