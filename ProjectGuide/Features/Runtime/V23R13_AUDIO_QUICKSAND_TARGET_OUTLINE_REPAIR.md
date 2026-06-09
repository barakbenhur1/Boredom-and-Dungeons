# V23R13 — Audio Coverage, Quicksand, and Silhouette Target Outline

## Scope

This repair expands the canonical audio contract from a small set of examples to complete event coverage, implements playable quicksand, replaces the rectangular target frame with a red mesh-silhouette outline, and removes the reported CS0414 warnings.

## Audio coverage

`ProjectGuide/Product/AUDIO_DIRECTION.md` and its Unity mirror now define minimum coverage for weapons, impacts, shots, breakables, holes, lava, quicksand, jump, dodge, footsteps, mounted movement, horse voices, menus, Game Boy UI, buttons, intro/cinematics, ambience, enemies, bosses, and future actions. The list is deliberately non-exclusive: every audible game event requires an intentional sound decision.

The expanded document does not falsely claim that the final mixer, authored music, or complete asset library already exists. Those remain under C12.42.

## Quicksand implementation — DONE IN CODE

- Adds `BDHazardType.Quicksand`.
- Adds a third playable hazard to the prototype hazard area.
- Applies progressive movement reduction and a following surface ring.
- Leaving before full sink recovers gradually.
- Full player sink deals 12 unavoidable damage and uses safe-point recovery.
- Horse movement is slowed; full sink uses horse-safe recovery without horse damage.
- Mounted recovery applies the rider consequence once and avoids repeated loops.
- Adds generated prototype entry/sink/escape cues pending authored SFX.

## Target outline implementation — DONE IN CODE

- Removes the GUI rectangle/corner brackets and pulse.
- Adds a red inverted-hull silhouette shader around the target model.
- Uses fixed pixel thickness so the outline does not grow with distance.
- Preserves one-target selection, truthful projectile path, range, mounted targeting, and wall blocking.

## Warning cleanup — DONE IN CODE

Removes obsolete assigned-but-unused fields:

- `BDEnemyPlacementGuard.landingSearchRadius`
- `BDEnemyPlacementGuard.descendingThresholdPerFrame`
- `BDParrySystem.worldFrozen`
- `BDCombatTargetHighlighter.originHeight`

## Verification required

1. TEST EVERYTHING passes with zero blockers.
2. Unity compilation no longer reports the four CS0414 warnings.
3. Quicksand slows player and horse progressively, shows its ring, clears after escape, and recovers safely after full sink.
4. Mounted quicksand recovery does not double-damage the rider or leave either actor stuck.
5. One red outline follows the actual enemy silhouette at sword, hook, and projectile ranges.
6. The outline has stable thickness, no rectangular box, no pulse, no material leak, and no visibility behind blockers.
7. Existing hole, lava, camera, menu, bomb, airborne attack, horse, enemy placement, and combat behavior remain passing.
