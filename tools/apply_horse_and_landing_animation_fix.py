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


def patch_horse_controller(path: Path) -> None:
    text = path.read_text(encoding="utf-8")

    if "// BD HORSE START-BESIDE-PLAYER FIX" in text:
        print("SKIP: BDHorseController.cs already contains this fix.")
        return

    text = replace_once(
        text,
        '''        [Header("Interaction")]
        [SerializeField] private float interactionRange = 3.25f;
        [SerializeField] private float healPerSecond = 28f;
''',
        '''        [Header("Interaction")]
        [SerializeField] private float interactionRange = 3.25f;
        [SerializeField] private float healPerSecond = 28f;

        [Header("Start Beside Player")]
        // BD HORSE START-BESIDE-PLAYER FIX
        [SerializeField] private bool placeBesidePlayerOnStart = true;
        [SerializeField] private Vector3 startLocalOffsetFromPlayer =
            new Vector3(2.35f, 0f, 0.45f);
        [SerializeField] private bool faceSameDirectionAsPlayerOnStart = true;
        [SerializeField] private float nearbyIdleNoTrackingRadius = 4.25f;
        private bool startPositionApplied;
''',
        "horse start-position fields",
    )

    text = replace_once(
        text,
        '''        private void Start()
        {
            if (rider == null)
                rider = BDTargetFinder.FindPlayer();

            CachePlayerComponents();
            ResolveSafeSpotIfNeeded();
        }
''',
        '''        private void Start()
        {
            if (rider == null)
                rider = BDTargetFinder.FindPlayer();

            CachePlayerComponents();
            PlaceHorseBesidePlayerAtStart();
            ResolveSafeSpotIfNeeded();
        }

        private void PlaceHorseBesidePlayerAtStart()
        {
            if (startPositionApplied ||
                !placeBesidePlayerOnStart ||
                rider == null ||
                state == HorseState.Mounted)
            {
                return;
            }

            Vector3 horizontalOffset =
                rider.right * startLocalOffsetFromPlayer.x +
                rider.forward * startLocalOffsetFromPlayer.z;

            Vector3 targetPosition =
                rider.position +
                horizontalOffset +
                Vector3.up * startLocalOffsetFromPlayer.y;

            bool controllerWasEnabled =
                controller != null && controller.enabled;

            if (controllerWasEnabled)
                controller.enabled = false;

            transform.position = targetPosition;

            if (faceSameDirectionAsPlayerOnStart)
            {
                Vector3 playerForward = rider.forward;
                playerForward.y = 0f;

                if (playerForward.sqrMagnitude > 0.001f)
                {
                    transform.rotation =
                        Quaternion.LookRotation(
                            playerForward.normalized,
                            Vector3.up
                        );
                }
            }

            if (controllerWasEnabled)
                controller.enabled = true;

            Physics.SyncTransforms();

            state = HorseState.Idle;
            lastCombatActiveAt = Time.time;
            lastAction = "started beside player";
            startPositionApplied = true;
        }
''',
        "horse Start method",
    )

    text = replace_once(
        text,
        '''            float distance = toPlayer.magnitude;
            if (distance <= 0.001f)
                return;

            Vector3 directionToPlayer = toPlayer.normalized;

            float comfortRadius = Mathf.Max(returnTooCloseRadius + 0.25f, returnComfortRadius);
''',
        '''            float distance = toPlayer.magnitude;
            if (distance <= 0.001f)
                return;

            float comfortRadius =
                Mathf.Max(
                    returnTooCloseRadius + 0.25f,
                    returnComfortRadius
                );

            float noTrackingRadius =
                Mathf.Max(
                    interactionRange,
                    nearbyIdleNoTrackingRadius,
                    comfortRadius
                );

            // When the player is already near the horse, the horse must remain
            // completely idle. It does not rotate to follow the player and it
            // does not back away merely because the player walked around it.
            if (distance <= noTrackingRadius)
            {
                if (state == HorseState.ReturningToPlayer)
                    state = HorseState.Idle;

                lastAction =
                    $"player nearby - idle without tracking {distance:0.0}";
                return;
            }

            Vector3 directionToPlayer = toPlayer.normalized;
''',
        "horse nearby no-tracking guard",
    )

    path.write_text(text, encoding="utf-8")
    print("PATCHED: BDHorseController.cs")


def patch_player_combat(path: Path) -> None:
    text = path.read_text(encoding="utf-8")

    if "// BD LANDING ATTACK VISUAL EXCLUSIVITY" in text:
        print("SKIP: BDPlayerCombat.cs already contains this fix.")
        return

    text = replace_once(
        text,
        '''        private BDHorseController cachedMountedHorseCheck;
        private float nextMountedHorseResolveAt;
''',
        '''        private BDHorseController cachedMountedHorseCheck;
        private float nextMountedHorseResolveAt;

        // BD LANDING ATTACK VISUAL EXCLUSIVITY
        private float suppressStandardMeleeVisualUntilUnscaled = -999f;
''',
        "combat landing visual suppression state",
    )

    text = replace_once(
        text,
        '''        private void Awake()
        {
            rangedAmmo = RangedMagazineSize;
        }
''',
        '''        public void SuppressNextStandardMeleeVisual(
            float maximumWaitSeconds = 0.18f)
        {
            suppressStandardMeleeVisualUntilUnscaled =
                Time.unscaledTime +
                Mathf.Max(0.02f, maximumWaitSeconds);
        }

        private void Awake()
        {
            rangedAmmo = RangedMagazineSize;
        }
''',
        "combat suppression API",
    )

    text = replace_once(
        text,
        '''        private void SpawnMeleeSlashArc(Vector3 aim, bool heavySwing)
        {
            if (!spawnMeleeSlashArc)
                return;

            Vector3 origin = transform.position + aim.normalized * 0.25f;
            BDMeleeSlashArcVisual.Spawn(origin, aim, attackRange, attackRadius, heavySwing);
        }
''',
        '''        private void SpawnMeleeSlashArc(
            Vector3 aim,
            bool heavySwing)
        {
            if (Time.unscaledTime <=
                suppressStandardMeleeVisualUntilUnscaled)
            {
                suppressStandardMeleeVisualUntilUnscaled = -999f;
                return;
            }

            suppressStandardMeleeVisualUntilUnscaled = -999f;

            if (!spawnMeleeSlashArc)
                return;

            Vector3 origin =
                transform.position +
                aim.normalized * 0.25f;

            BDMeleeSlashArcVisual.Spawn(
                origin,
                aim,
                attackRange,
                attackRadius,
                heavySwing
            );
        }
''',
        "standard melee visual method",
    )

    path.write_text(text, encoding="utf-8")
    print("PATCHED: BDPlayerCombat.cs")


def patch_melee_enhancer(path: Path) -> None:
    text = path.read_text(encoding="utf-8")

    if "// BD LANDING-ONLY ANIMATION FIX" in text:
        print("SKIP: BDPlayerMeleeEnhancer.cs already contains this fix.")
        return

    text = replace_once(
        text,
        '''            if (!landing)
                return;

            ApplyTemporaryLandingDamage(attack);

            if (spawnLandingAttackVisual)
                BDLandingAttackVisual.Spawn(transform.position, ResolveAimDirection(), heavy);
''',
        '''            if (!landing)
                return;

            // BD LANDING-ONLY ANIMATION FIX:
            // The landing strike replaces the normal slash visual for this hit.
            combat.SuppressNextStandardMeleeVisual();
            ApplyTemporaryLandingDamage(attack);

            if (spawnLandingAttackVisual)
            {
                BDLandingAttackVisual.Spawn(
                    transform.position,
                    ResolveAimDirection(),
                    heavy
                );
            }
''',
        "immediate landing animation",
    )

    text = replace_once(
        text,
        '''            parryState.RecordMeleeAttack(heavy);

            if (landing && spawnLandingAttackVisual)
                BDLandingAttackVisual.Spawn(transform.position, ResolveAimDirection(), heavy);

            object[] arguments =
''',
        '''            parryState.RecordMeleeAttack(heavy);

            if (landing)
            {
                combat.SuppressNextStandardMeleeVisual();

                if (spawnLandingAttackVisual)
                {
                    BDLandingAttackVisual.Spawn(
                        transform.position,
                        ResolveAimDirection(),
                        heavy
                    );
                }
            }

            object[] arguments =
''',
        "buffered landing animation",
    )

    path.write_text(text, encoding="utf-8")
    print("PATCHED: BDPlayerMeleeEnhancer.cs")


def main() -> None:
    root = find_project_root(Path.cwd())
    runtime = root / "Assets/_Project/Scripts/Runtime"

    horse = runtime / "BDHorseController.cs"
    combat = runtime / "BDPlayerCombat.cs"
    enhancer = runtime / "Combat/BDPlayerMeleeEnhancer.cs"

    required = [horse, combat, enhancer]

    for path in required:
        if not path.is_file():
            fail(f"Required file not found: {path}")

    backup_root = (
        Path(tempfile.gettempdir()) /
        f"BoredomAndDungeons_horse_landing_fix_backup_"
        f"{datetime.now():%Y%m%d_%H%M%S}"
    )
    backup_root.mkdir(parents=True, exist_ok=True)

    for path in required:
        shutil.copy2(path, backup_root / path.name)

    patch_horse_controller(horse)
    patch_player_combat(combat)
    patch_melee_enhancer(enhancer)

    print()
    print("Horse proximity and landing animation fix applied.")
    print(f"Backup: {backup_root}")
    print("Next: return to Unity and wait for compilation.")


if __name__ == "__main__":
    main()
