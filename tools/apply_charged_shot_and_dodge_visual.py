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


def patch_player_combat(path: Path) -> None:
    text = path.read_text(encoding="utf-8")

    if "// BD CHARGED SHOT SYSTEM" in text:
        print("SKIP: BDPlayerCombat.cs already contains Charged Shot.")
        return

    text = replace_once(
        text,
        '''        [SerializeField] private int rangedMagazineSize = 3;
        [SerializeField] private float rangedReloadDuration = 6.0f;
''',
        '''        [SerializeField] private int rangedMagazineSize = 3;
        [SerializeField] private float rangedReloadDuration = 6.0f;

        [Header("Charged Ranged Shot")]
        // BD CHARGED SHOT SYSTEM
        [SerializeField] private bool enableChargedShot = true;
        [SerializeField] private float chargedShotBaseDuration = 0.90f;
        [SerializeField] private float chargedShotSecondsPerAdditionalAmmo = 0.45f;
        [SerializeField] private float chargedShotMaximumDuration = 3.20f;
        [SerializeField] private float chargedProjectileScalePerExtraAmmo = 0.22f;
        [SerializeField] private float chargedHitRadiusPerExtraAmmo = 0.10f;
        [SerializeField] private float chargedKnockbackPerExtraAmmo = 0.28f;
        [SerializeField] private float chargedSpeedPerExtraAmmo = 0.035f;
''',
        "charged shot settings",
    )

    text = replace_once(
        text,
        '''        private float boostReloadDurationReduction;
        private float boostWeaponDamageMultiplier = 1f;
        private float boostMinimumReloadDuration = 1f;
''',
        '''        private float boostReloadDurationReduction;
        private float boostWeaponDamageMultiplier = 1f;
        private float boostMinimumReloadDuration = 1f;

        private bool chargedShotCharging;
        private float chargedShotStartedAtUnscaled;
        private float chargedShotRequiredDuration;
        private int chargedShotReservedAmmo;
        private BDChargedShotChargeVisual chargedShotChargeVisual;
''',
        "charged shot runtime state",
    )

    text = replace_once(
        text,
        '''        public float WeaponDamageMultiplier =>
            Mathf.Max(0.01f, boostWeaponDamageMultiplier);
        public bool IsReloading => reloading;
''',
        '''        public float WeaponDamageMultiplier =>
            Mathf.Max(0.01f, boostWeaponDamageMultiplier);
        public bool IsChargingRangedShot => chargedShotCharging;
        public int ChargedShotReservedAmmo => chargedShotReservedAmmo;
        public float ChargedShotRequiredDuration =>
            chargedShotRequiredDuration;
        public float ChargedShotProgress01 =>
            !chargedShotCharging
                ? 0f
                : Mathf.Clamp01(
                    (Time.unscaledTime -
                     chargedShotStartedAtUnscaled) /
                    Mathf.Max(0.01f, chargedShotRequiredDuration)
                );
        public bool IsReloading => reloading;
''',
        "charged shot public state",
    )

    text = replace_once(
        text,
        '''        private void Awake()
        {
            rangedAmmo = RangedMagazineSize;
        }
''',
        '''        private void Awake()
        {
            rangedAmmo = RangedMagazineSize;
        }

        private void OnDisable()
        {
            CancelChargedShot();
        }
''',
        "charged shot disable cleanup",
    )

    text = replace_once(
        text,
        '''            if (ReadRangedAttackPressed())
                TryRangedAttack();
''',
        '''            TickChargedRangedAttack();
''',
        "ranged input routing",
    )

    marker = '''        private void TryRangedAttack()
        {
'''

    charged_methods = r'''        private void TickChargedRangedAttack()
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

            // With one projectile left, charging has no purpose.
            // The last projectile fires immediately.
            if (rangedAmmo == 1)
            {
                TryRangedAttack();
                return;
            }

            BeginChargedShot();
        }

        private void BeginChargedShot()
        {
            chargedShotCharging = true;
            chargedShotStartedAtUnscaled = Time.unscaledTime;
            chargedShotReservedAmmo = Mathf.Max(2, rangedAmmo);

            int additionalAmmo =
                Mathf.Max(0, chargedShotReservedAmmo - 2);

            chargedShotRequiredDuration =
                Mathf.Min(
                    Mathf.Max(
                        0.10f,
                        chargedShotMaximumDuration
                    ),
                    Mathf.Max(
                        0.10f,
                        chargedShotBaseDuration
                    ) +
                    additionalAmmo *
                    Mathf.Max(
                        0f,
                        chargedShotSecondsPerAdditionalAmmo
                    )
                );

            Vector3 direction = GetCombatAimDirection();
            ApplyCombatFacing(direction);

            chargedShotChargeVisual =
                BDChargedShotChargeVisual.Spawn(
                    transform,
                    direction,
                    chargedShotReservedAmmo,
                    chargedShotRequiredDuration
                );

            lastCombatAction =
                $"charging {chargedShotReservedAmmo} ammo " +
                $"{chargedShotRequiredDuration:0.00}s";
        }

        private void TickActiveChargedShot()
        {
            if (reloading || rangedAmmo <= 0)
            {
                CancelChargedShot();
                return;
            }

            Vector3 direction = GetCombatAimDirection();
            ApplyCombatFacing(direction);

            float progress = ChargedShotProgress01;

            if (chargedShotChargeVisual != null)
            {
                chargedShotChargeVisual.SetCharge(
                    progress,
                    direction
                );
            }

            // Completion is checked before release, so releasing on the
            // exact completion frame still fires the charged projectile.
            if (progress >= 1f)
            {
                FireChargedShot(direction);
                return;
            }

            if (ReadRangedAttackReleased() ||
                !ReadRangedAttackHeld())
            {
                CancelChargedShot();
            }
        }

        private void FireChargedShot(Vector3 direction)
        {
            int ammoToConsume =
                Mathf.Clamp(
                    chargedShotReservedAmmo,
                    1,
                    rangedAmmo
                );

            if (ammoToConsume <= 0)
            {
                CancelChargedShot();
                BeginReloadIfNeeded();
                return;
            }

            nextRangedAllowedAt =
                Time.time + Mathf.Max(0.01f, rangedCooldown);

            rangedAmmo -= ammoToConsume;

            direction.y = 0f;

            if (direction.sqrMagnitude < 0.001f)
                direction = GetCombatAimDirection();

            if (direction.sqrMagnitude < 0.001f)
                direction = transform.forward;

            direction.Normalize();
            ApplyCombatFacing(direction);

            Vector3 spawnPosition =
                transform.position +
                direction * rangedSpawnForwardOffset;

            spawnPosition.y = ResolveProjectileSpawnY();

            float extraAmmo = Mathf.Max(0f, ammoToConsume - 1f);
            float projectileScaleMultiplier =
                1f +
                extraAmmo *
                Mathf.Max(
                    0f,
                    chargedProjectileScalePerExtraAmmo
                );

            float projectileSpeedMultiplier =
                1f +
                extraAmmo *
                Mathf.Max(
                    0f,
                    chargedSpeedPerExtraAmmo
                );

            float projectileHitRadiusMultiplier =
                1f +
                extraAmmo *
                Mathf.Max(
                    0f,
                    chargedHitRadiusPerExtraAmmo
                );

            float projectileKnockbackMultiplier =
                1f +
                extraAmmo *
                Mathf.Max(
                    0f,
                    chargedKnockbackPerExtraAmmo
                );

            GameObject projectile =
                GameObject.CreatePrimitive(PrimitiveType.Sphere);

            projectile.name =
                $"BD_Player_Charged_Projectile_x{ammoToConsume}";

            projectile.transform.position = spawnPosition;
            projectile.transform.rotation =
                Quaternion.LookRotation(direction, Vector3.up);
            projectile.transform.localScale =
                Vector3.one *
                0.55f *
                projectileScaleMultiplier;

            Renderer renderer = projectile.GetComponent<Renderer>();

            if (renderer != null)
            {
                Material projectileMaterial =
                    CreateProjectileMaterial();

                if (projectileMaterial != null)
                    renderer.sharedMaterial = projectileMaterial;
            }

            Collider collider = projectile.GetComponent<Collider>();

            if (collider != null)
                Destroy(collider);

            BDPlayerRangedProjectile projectileLogic =
                projectile.AddComponent<BDPlayerRangedProjectile>();

            projectileLogic.Configure(
                direction,
                rangedProjectileSpeed *
                    projectileSpeedMultiplier,
                rangedDamage *
                    WeaponDamageMultiplier *
                    ammoToConsume,
                rangedProjectileLifetime,
                rangedProjectileHitRadius *
                    projectileHitRadiusMultiplier,
                rangedProjectileKnockback *
                    projectileKnockbackMultiplier,
                transform
            );

            BDRangedAttackVisuals.AddProjectileTrail(
                projectile,
                playerProjectile: true
            );

            BDChargedProjectileVisual.Attach(
                projectile,
                ammoToConsume
            );

            BDRangedAttackVisuals.SpawnMuzzleFlash(
                spawnPosition,
                direction,
                playerProjectile: true
            );

            BDChargedShotVisualUtility.SpawnChargedMuzzleBurst(
                spawnPosition,
                direction,
                ammoToConsume
            );

            if (chargedShotChargeVisual != null)
                chargedShotChargeVisual.ReleaseToProjectile();

            chargedShotChargeVisual = null;
            chargedShotCharging = false;
            chargedShotRequiredDuration = 0f;
            chargedShotReservedAmmo = 0;

            BDGameFeelAudio.PlayRangedShot();
            BDGameFeelEvents.RequestCameraShake(
                Mathf.Clamp(
                    0.10f + ammoToConsume * 0.045f,
                    0.16f,
                    0.42f
                ),
                0.14f
            );

            lastCombatAction =
                $"charged shot x{ammoToConsume} " +
                $"ammo={rangedAmmo}";

            if (rangedAmmo <= 0)
                BeginReloadIfNeeded();
        }

        private void CancelChargedShot()
        {
            if (!chargedShotCharging &&
                chargedShotChargeVisual == null)
            {
                return;
            }

            if (chargedShotChargeVisual != null)
                chargedShotChargeVisual.CancelCharge();

            chargedShotChargeVisual = null;
            chargedShotCharging = false;
            chargedShotStartedAtUnscaled = 0f;
            chargedShotRequiredDuration = 0f;
            chargedShotReservedAmmo = 0;
            lastCombatAction = "charged shot cancelled";
        }

'''

    text = replace_once(
        text,
        marker,
        charged_methods + marker,
        "charged shot methods",
    )

    input_marker = '''        private void OnGUI()
        {
'''

    input_methods = r'''        private bool ReadRangedAttackHeld()
        {
#if ENABLE_INPUT_SYSTEM
            Keyboard keyboard = Keyboard.current;

            if (keyboard != null && keyboard.qKey.isPressed)
                return true;
#endif

#if ENABLE_LEGACY_INPUT_MANAGER
            if (Input.GetKey(KeyCode.Q))
                return true;
#endif

            return false;
        }

        private bool ReadRangedAttackReleased()
        {
#if ENABLE_INPUT_SYSTEM
            Keyboard keyboard = Keyboard.current;

            if (keyboard != null &&
                keyboard.qKey.wasReleasedThisFrame)
            {
                return true;
            }
#endif

#if ENABLE_LEGACY_INPUT_MANAGER
            if (Input.GetKeyUp(KeyCode.Q))
                return true;
#endif

            return false;
        }

'''

    text = replace_once(
        text,
        input_marker,
        input_methods + input_marker,
        "charged shot input helpers",
    )

    path.write_text(text, encoding="utf-8")
    print("PATCHED: BDPlayerCombat.cs")


def patch_player_controller(path: Path) -> None:
    text = path.read_text(encoding="utf-8")

    if "// BD FORWARD DODGE REAR VISUAL OFFSET" in text:
        print("SKIP: BDPlayerController.cs already contains dodge offset.")
        return

    text = replace_once(
        text,
        '''        [SerializeField] private float dodgeAfterimageInterval = 0.035f;
        [SerializeField] private bool spawnDodgeAfterimages = true;
''',
        '''        [SerializeField] private float dodgeAfterimageInterval = 0.035f;
        [SerializeField] private bool spawnDodgeAfterimages = true;
        // BD FORWARD DODGE REAR VISUAL OFFSET
        [SerializeField] private float forwardDodgeVisualRearOffset = 0.82f;
''',
        "forward dodge visual offset setting",
    )

    text = replace_once(
        text,
        '''            nextDodgeAfterimageAt = 0f;
            SpawnDodgeAfterimageIfNeeded();
            BDGameFeelAudio.PlayDodge();
            lastDodgeDirection = dodgeDirection;
''',
        '''            nextDodgeAfterimageAt = 0f;
            lastDodgeDirection = dodgeDirection;
            SpawnDodgeAfterimageIfNeeded();
            BDGameFeelAudio.PlayDodge();
''',
        "forward dodge direction timing",
    )

    text = replace_once(
        text,
        '''            if (dashDirection.sqrMagnitude > 0.001f)
                visualRotation = Quaternion.LookRotation(dashDirection.normalized, Vector3.up);

            BDPlayerDodgeAfterimage.Spawn(transform.position, visualRotation, radius, height);
''',
        '''            Vector3 visualPosition = transform.position;

            if (dashDirection.sqrMagnitude > 0.001f)
            {
                Vector3 normalizedDashDirection =
                    dashDirection.normalized;

                visualRotation =
                    Quaternion.LookRotation(
                        normalizedDashDirection,
                        Vector3.up
                    );

                if (lastDodgeDirection ==
                    DodgeDirection.Forward)
                {
                    visualPosition -=
                        normalizedDashDirection *
                        Mathf.Max(
                            0f,
                            forwardDodgeVisualRearOffset
                        );
                }
            }

            BDPlayerDodgeAfterimage.Spawn(
                visualPosition,
                visualRotation,
                radius,
                height
            );
''',
        "forward dodge afterimage position",
    )

    path.write_text(text, encoding="utf-8")
    print("PATCHED: BDPlayerController.cs")


def main() -> None:
    root = find_project_root(Path.cwd())
    runtime = root / "Assets/_Project/Scripts/Runtime"

    combat = runtime / "BDPlayerCombat.cs"
    controller = runtime / "BDPlayerController.cs"
    visuals = runtime / "Combat/BDChargedShotVisuals.cs"

    for path in (combat, controller, visuals):
        if not path.is_file():
            fail(f"Required file not found: {path}")

    backup_root = (
        Path(tempfile.gettempdir()) /
        f"BoredomAndDungeons_charged_shot_backup_"
        f"{datetime.now():%Y%m%d_%H%M%S}"
    )
    backup_root.mkdir(parents=True, exist_ok=True)

    shutil.copy2(combat, backup_root / combat.name)
    shutil.copy2(controller, backup_root / controller.name)

    patch_player_combat(combat)
    patch_player_controller(controller)

    print()
    print("Charged Shot and forward Dodge visual offset applied.")
    print(f"Backup: {backup_root}")
    print("Next: return to Unity and wait for compilation.")


if __name__ == "__main__":
    main()
