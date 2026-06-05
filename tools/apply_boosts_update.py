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
    candidates = [start.resolve(), *start.resolve().parents]
    for candidate in candidates:
        if (candidate / "Assets/_Project/Scripts/Runtime").is_dir():
            return candidate
    fail(
        "Run this script from the Boredom-and-Dungeons project root "
        "or from inside the extracted update package."
    )


def replace_once(text: str, old: str, new: str, label: str) -> str:
    count = text.count(old)
    if count != 1:
        fail(f"{label}: expected exactly one match, found {count}.")
    return text.replace(old, new, 1)


def patch_player_combat(path: Path) -> None:
    text = path.read_text(encoding="utf-8")

    if "// BD BOOST API: runtime modifiers" in text:
        print("SKIP: BDPlayerCombat.cs already contains the Boost API.")
        return

    text = replace_once(
        text,
        "        private float nextMountedHorseResolveAt;\n",
        '''        private float nextMountedHorseResolveAt;

        // BD BOOST API: runtime modifiers
        private int boostAdditionalMagazineCapacity;
        private float boostReloadDurationReduction;
        private float boostWeaponDamageMultiplier = 1f;
        private float boostMinimumReloadDuration = 1f;
''',
        "BDPlayerCombat boost fields",
    )

    text = replace_once(
        text,
        '''        public int RangedAmmo => rangedAmmo;
        public int RangedMagazineSize => Mathf.Max(1, rangedMagazineSize);
        public bool IsReloading => reloading;
''',
        '''        public int RangedAmmo => rangedAmmo;
        public int RangedMagazineSize =>
            Mathf.Max(1, rangedMagazineSize + boostAdditionalMagazineCapacity);
        public float EffectiveRangedReloadDuration =>
            Mathf.Max(
                boostMinimumReloadDuration,
                rangedReloadDuration - boostReloadDurationReduction
            );
        public float WeaponDamageMultiplier =>
            Mathf.Max(0.01f, boostWeaponDamageMultiplier);
        public bool IsReloading => reloading;
''',
        "BDPlayerCombat public boost properties",
    )

    text = replace_once(
        text,
        "                float duration = Mathf.Max(0.1f, rangedReloadDuration);\n",
        "                float duration = EffectiveRangedReloadDuration;\n",
        "BDPlayerCombat reload progress",
    )

    text = replace_once(
        text,
        '''        private void Awake()
        {
            rangedAmmo = Mathf.Max(1, rangedMagazineSize);
        }
''',
        '''        private void Awake()
        {
            rangedAmmo = RangedMagazineSize;
        }
''',
        "BDPlayerCombat initial ammo",
    )

    text = replace_once(
        text,
        '''            if (Time.time < reloadEndsAt)
                return;

            rangedAmmo = Mathf.Max(1, rangedMagazineSize);
            reloading = false;
''',
        '''            if (Time.time < reloadEndsAt)
                return;

            rangedAmmo = RangedMagazineSize;
            reloading = false;
''',
        "BDPlayerCombat reload ammo",
    )

    text = replace_once(
        text,
        "            reloadEndsAt = Time.time + Mathf.Max(0.1f, rangedReloadDuration);\n",
        "            reloadEndsAt = Time.time + EffectiveRangedReloadDuration;\n",
        "BDPlayerCombat reload duration",
    )

    text = replace_once(
        text,
        '''            int hitCount = 0;
            int uniqueHealthCount = 0;
''',
        '''            int hitCount = 0;
            int uniqueHealthCount = 0;
            float effectiveDamage = damage * WeaponDamageMultiplier;
''',
        "BDPlayerCombat effective melee damage",
    )

    text = replace_once(
        text,
        "                health.ApplyDamage(damage);\n",
        "                health.ApplyDamage(effectiveDamage);\n",
        "BDPlayerCombat apply melee damage",
    )

    text = replace_once(
        text,
        '''                rangedProjectileSpeed,
                rangedDamage,
                rangedProjectileLifetime,
''',
        '''                rangedProjectileSpeed,
                rangedDamage * WeaponDamageMultiplier,
                rangedProjectileLifetime,
''',
        "BDPlayerCombat ranged damage multiplier",
    )

    marker = "        private bool ReadLightAttackPressed()\n"
    api_method = r'''        public void ApplyBoostModifiers(
            int additionalMagazineCapacity,
            float reloadDurationReduction,
            float weaponDamageMultiplier,
            float minimumReloadDuration,
            bool grantAddedAmmo)
        {
            int previousMagazineSize = RangedMagazineSize;

            boostAdditionalMagazineCapacity =
                Mathf.Max(0, additionalMagazineCapacity);
            boostReloadDurationReduction =
                Mathf.Max(0f, reloadDurationReduction);
            boostWeaponDamageMultiplier =
                Mathf.Max(0.01f, weaponDamageMultiplier);
            boostMinimumReloadDuration =
                Mathf.Max(0.1f, minimumReloadDuration);

            int updatedMagazineSize = RangedMagazineSize;

            if (grantAddedAmmo &&
                updatedMagazineSize > previousMagazineSize)
            {
                int gainedCapacity =
                    updatedMagazineSize - previousMagazineSize;

                rangedAmmo = Mathf.Min(
                    updatedMagazineSize,
                    rangedAmmo + gainedCapacity
                );
            }
            else
            {
                rangedAmmo =
                    Mathf.Min(rangedAmmo, updatedMagazineSize);
            }

            if (reloading)
            {
                reloadEndsAt = Mathf.Min(
                    reloadEndsAt,
                    Time.time + EffectiveRangedReloadDuration
                );
            }
        }

'''
    text = replace_once(
        text,
        marker,
        api_method + marker,
        "BDPlayerCombat ApplyBoostModifiers",
    )

    text = replace_once(
        text,
        '$"Ranged ammo: {rangedAmmo} / {rangedMagazineSize}"',
        '$"Ranged ammo: {rangedAmmo} / {RangedMagazineSize}"',
        "BDPlayerCombat debug magazine",
    )

    path.write_text(text, encoding="utf-8")
    print("PATCHED: BDPlayerCombat.cs")


def patch_player_controller(path: Path) -> None:
    text = path.read_text(encoding="utf-8")

    if "// BD BOOST API: movement multiplier" in text:
        print("SKIP: BDPlayerController.cs already contains the Boost API.")
        return

    text = replace_once(
        text,
        "        private CharacterController characterController;\n",
        '''        private CharacterController characterController;

        // BD BOOST API: movement multiplier
        private float boostMoveSpeedMultiplier = 1f;
''',
        "BDPlayerController boost field",
    )

    text = replace_once(
        text,
        "        public bool IsDashing => dashTimer > 0f;\n",
        '''        public bool IsDashing => dashTimer > 0f;
        public float EffectiveMoveSpeed =>
            Mathf.Max(0.1f, moveSpeed * boostMoveSpeedMultiplier);

        public void SetBoostMoveSpeedMultiplier(float multiplier)
        {
            boostMoveSpeedMultiplier = Mathf.Max(0.1f, multiplier);
        }
''',
        "BDPlayerController public Boost API",
    )

    text = replace_once(
        text,
        "                desiredVelocity = desiredMoveDirection.normalized * moveSpeed;\n",
        "                desiredVelocity = desiredMoveDirection.normalized * EffectiveMoveSpeed;\n",
        "BDPlayerController effective move speed",
    )

    path.write_text(text, encoding="utf-8")
    print("PATCHED: BDPlayerController.cs")


def patch_loot_dropper(path: Path) -> None:
    text = path.read_text(encoding="utf-8")

    if '[Header("BD BOOST DROPS")]' in text:
        print("SKIP: BDEnemyLootDropper.cs already contains Boost drops.")
        return

    text = replace_once(
        text,
        '''        [SerializeField] private float playerHealFractionOfMax = 0.40f;
        [SerializeField] private float spawnHeight = 0.45f;
''',
        '''        [SerializeField] private float playerHealFractionOfMax = 0.40f;
        [SerializeField] private float spawnHeight = 0.45f;

        [Header("BD BOOST DROPS")]
        [Range(0f, 1f)]
        [SerializeField] private float regularEnemyBoostDropChance = 0.02f;
        [Range(0f, 1f)]
        [SerializeField] private float miniBossBoostDropChance = 0.12f;
        [SerializeField] private float boostSpawnHeight = 0.70f;
''',
        "BDEnemyLootDropper fields",
    )

    text = replace_once(
        text,
        '''        private BDHealth health;
        private bool dropped;
''',
        '''        private BDHealth health;
        private bool dropped;
        private bool boostDropResolved;
''',
        "BDEnemyLootDropper state",
    )

    text = replace_once(
        text,
        '''        private void OnDied(BDHealth deadHealth)
        {
            TryDropHeal();
        }
''',
        '''        private void OnDied(BDHealth deadHealth)
        {
            TryDropHeal();
            TryDropBoost(deadHealth);
        }
''',
        "BDEnemyLootDropper death hook",
    )

    marker = "\n\n        private void EnsureHealingPickupPolish(GameObject pickup)\n"
    method = r'''

        private void TryDropBoost(BDHealth deadHealth)
        {
            if (boostDropResolved)
                return;

            boostDropResolved = true;

            BDCombatantRank rank =
                BDCombatantProfile.ResolveRank(deadHealth);

            if (rank == BDCombatantRank.Boss)
                return;

            float dropChance =
                rank == BDCombatantRank.MiniBoss
                    ? miniBossBoostDropChance
                    : regularEnemyBoostDropChance;

            if (Random.value > Mathf.Clamp01(dropChance))
                return;

            Transform player = BDTargetFinder.FindPlayer();

            if (player == null)
            {
                BDPlayerMarker marker =
                    FindFirstObjectByType<BDPlayerMarker>();

                if (marker != null)
                    player = marker.transform;
            }

            if (player == null)
                return;

            BDPlayerBoostState boostState =
                player.GetComponent<BDPlayerBoostState>();

            if (boostState == null)
            {
                boostState =
                    player.gameObject.AddComponent<BDPlayerBoostState>();
            }

            if (!boostState.TryChooseRandomAvailable(
                    out BDPlayerBoostType boostType))
                return;

            Vector3 spawnPosition = transform.position;
            spawnPosition.y = boostSpawnHeight;

            BDPlayerBoostPickup.Spawn(spawnPosition, boostType);
        }
'''
    text = replace_once(
        text,
        marker,
        method + marker,
        "BDEnemyLootDropper boost method",
    )

    path.write_text(text, encoding="utf-8")
    print("PATCHED: BDEnemyLootDropper.cs")


def main() -> None:
    project_root = find_project_root(Path.cwd())

    runtime = project_root / "Assets/_Project/Scripts/Runtime"
    files = [
        runtime / "BDPlayerCombat.cs",
        runtime / "BDPlayerController.cs",
        runtime / "BDEnemyLootDropper.cs",
    ]

    for file_path in files:
        if not file_path.is_file():
            fail(f"Required file not found: {file_path}")

    backup_root = (
        Path(tempfile.gettempdir()) /
        f"BoredomAndDungeons_boosts_backup_{datetime.now():%Y%m%d_%H%M%S}"
    )
    backup_root.mkdir(parents=True, exist_ok=True)

    for file_path in files:
        shutil.copy2(file_path, backup_root / file_path.name)

    patch_player_combat(files[0])
    patch_player_controller(files[1])
    patch_loot_dropper(files[2])

    boost_file = (
        project_root /
        "Assets/_Project/Scripts/Runtime/Boosts/BDPlayerBoostSystem.cs"
    )
    if not boost_file.is_file():
        fail(
            "BDPlayerBoostSystem.cs is missing. "
            "Extract the entire ZIP into the project root first."
        )

    print()
    print("Boost update applied successfully.")
    print(f"Backup: {backup_root}")
    print("Next: open Unity and wait for compilation.")


if __name__ == "__main__":
    main()
