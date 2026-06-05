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

    fail(
        "Run this script from the Boredom-and-Dungeons project root "
        "after extracting the complete ZIP there."
    )


def replace_once(text: str, old: str, new: str, label: str) -> str:
    count = text.count(old)
    if count != 1:
        fail(f"{label}: expected exactly one match, found {count}.")
    return text.replace(old, new, 1)


def patch_projectile(path: Path) -> None:
    text = path.read_text(encoding="utf-8")

    if "// BD PROJECTILE KNOCKBACK POLICY" in text:
        print("SKIP: BDPlayerRangedProjectile.cs is already updated.")
        return

    text = replace_once(
        text,
        '        [SerializeField] private float rangedHitStaggerDuration = 0.055f;\n',
        '        [SerializeField] private float rangedHitStaggerDuration = 0.055f;\n'
        '        [SerializeField] private float rangedKnockbackLockDuration = 0.08f;\n',
        "projectile knockback lock field",
    )

    old_block = '''            BDKnockbackReceiver receiver = health.GetComponent<BDKnockbackReceiver>();
            if (receiver == null && health.GetComponent<CharacterController>() != null)
                receiver = health.gameObject.AddComponent<BDKnockbackReceiver>();

            if (receiver != null)
                receiver.AddKnockback(health.transform.position - transform.position, knockback);
'''

    new_block = '''            // BD PROJECTILE KNOCKBACK POLICY:
            // Damage always applies. Knockback applies only when the target profile allows it.
            if (BDCombatantProfile.CanReceivePlayerProjectileKnockback(health))
            {
                Vector3 knockDirection = direction;
                knockDirection.y = 0f;

                if (knockDirection.sqrMagnitude < 0.001f)
                {
                    knockDirection = health.transform.position - transform.position;
                    knockDirection.y = 0f;
                }

                BDKnockbackReceiver receiver =
                    health.GetComponent<BDKnockbackReceiver>();

                if (receiver == null &&
                    health.GetComponent<CharacterController>() != null)
                {
                    receiver =
                        health.gameObject.AddComponent<BDKnockbackReceiver>();
                }

                if (receiver != null)
                {
                    receiver.AddKnockback(
                        knockDirection,
                        knockback,
                        rangedKnockbackLockDuration
                    );
                }
                else
                {
                    Rigidbody body = health.GetComponent<Rigidbody>();

                    if (body != null && !body.isKinematic)
                    {
                        body.AddForce(
                            knockDirection.normalized * knockback,
                            ForceMode.VelocityChange
                        );
                    }
                }
            }
'''

    text = replace_once(
        text,
        old_block,
        new_block,
        "projectile knockback application",
    )

    path.write_text(text, encoding="utf-8")
    print("PATCHED: BDPlayerRangedProjectile.cs")


def patch_profile(path: Path) -> None:
    text = path.read_text(encoding="utf-8")

    if "// BD COMBATANT PROFILE POLICY" in text:
        print("SKIP: BDCombatantProfile.cs is already updated.")
        return

    text = replace_once(
        text,
        '''        public BDCombatantRank Rank => rank;
        public bool ReceivesPlayerProjectileKnockback => receivesPlayerProjectileKnockback;
''',
        '''        public BDCombatantRank Rank => rank;

        // BD COMBATANT PROFILE POLICY:
        // Final bosses are always immune. Mini-bosses remain explicit:
        // Quad Gunners use true; large mini-bosses use false.
        public bool ReceivesPlayerProjectileKnockback =>
            rank != BDCombatantRank.Boss &&
            receivesPlayerProjectileKnockback;
''',
        "combatant profile public properties",
    )

    text = replace_once(
        text,
        '''        public void Configure(BDCombatantRank newRank, bool receivesKnockback)
        {
            rank = newRank;
            receivesPlayerProjectileKnockback = receivesKnockback;
        }
''',
        '''        public void Configure(
            BDCombatantRank newRank,
            bool receivesKnockback)
        {
            rank = newRank;
            receivesPlayerProjectileKnockback =
                newRank != BDCombatantRank.Boss &&
                receivesKnockback;
        }

        public void ConfigureRegularEnemy()
        {
            Configure(BDCombatantRank.Regular, true);
        }

        public void ConfigureSmallMiniBoss()
        {
            Configure(BDCombatantRank.MiniBoss, true);
        }

        public void ConfigureLargeMiniBoss()
        {
            Configure(BDCombatantRank.MiniBoss, false);
        }

        public void ConfigureFinalBoss()
        {
            Configure(BDCombatantRank.Boss, false);
        }
''',
        "combatant profile configuration",
    )

    text = replace_once(
        text,
        '''            BDCombatantProfile profile = health.GetComponent<BDCombatantProfile>();
            if (profile == null)
                return true;

            return profile.receivesPlayerProjectileKnockback;
''',
        '''            BDCombatantProfile profile = ResolveProfile(health);

            // Existing regular enemies without a profile keep receiving knockback.
            if (profile == null)
                return true;

            return profile.ReceivesPlayerProjectileKnockback;
''',
        "combatant knockback policy",
    )

    text = replace_once(
        text,
        '''            BDCombatantProfile profile = health.GetComponent<BDCombatantProfile>();
            return profile != null ? profile.rank : BDCombatantRank.Regular;
        }
''',
        '''            BDCombatantProfile profile = ResolveProfile(health);
            return profile != null
                ? profile.rank
                : BDCombatantRank.Regular;
        }

        public static BDCombatantProfile ResolveProfile(BDHealth health)
        {
            if (health == null)
                return null;

            BDCombatantProfile profile =
                health.GetComponent<BDCombatantProfile>();

            if (profile == null)
                profile = health.GetComponentInParent<BDCombatantProfile>();

            return profile;
        }

        private void OnValidate()
        {
            if (rank == BDCombatantRank.Boss)
                receivesPlayerProjectileKnockback = false;
        }
''',
        "combatant profile resolver",
    )

    path.write_text(text, encoding="utf-8")
    print("PATCHED: BDCombatantProfile.cs")


def main() -> None:
    project_root = find_project_root(Path.cwd())
    runtime = project_root / "Assets/_Project/Scripts/Runtime"

    projectile = runtime / "BDPlayerRangedProjectile.cs"
    profile = runtime / "BDCombatantProfile.cs"

    for file_path in (projectile, profile):
        if not file_path.is_file():
            fail(f"Required file not found: {file_path}")

    backup_root = (
        Path(tempfile.gettempdir()) /
        f"BoredomAndDungeons_projectile_knockback_backup_"
        f"{datetime.now():%Y%m%d_%H%M%S}"
    )
    backup_root.mkdir(parents=True, exist_ok=True)

    shutil.copy2(projectile, backup_root / projectile.name)
    shutil.copy2(profile, backup_root / profile.name)

    patch_projectile(projectile)
    patch_profile(profile)

    editor_tool = (
        project_root /
        "Assets/_Project/Scripts/Editor/CombatProfiles/"
        "BDCombatantProfileSetupTools.cs"
    )

    if not editor_tool.is_file():
        fail(
            "The profile setup tool is missing. "
            "Extract the complete ZIP into the project root first."
        )

    print()
    print("Projectile knockback update applied successfully.")
    print(f"Backup: {backup_root}")
    print("Next: open Unity and wait for compilation.")


if __name__ == "__main__":
    main()
