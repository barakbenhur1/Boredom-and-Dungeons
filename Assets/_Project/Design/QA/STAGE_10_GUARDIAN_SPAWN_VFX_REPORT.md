# Stage 10 / 36 — Guardian Spawn VFX Report

## Goal

Add a visible spawn effect and activation delay for collectible guardians.

## Done

- Added `BDGuardianSpawnVfx`.
- Patched `BDCollectibleGuardianSpawner`.
- Guardians now start hidden/inactive during spawn VFX.
- Guardians activate after the anticipation delay.
- Added final flash after activation.
- Added design and QA documentation.

## Not changed

```text
Enemy counts
Enemy health
Enemy damage
Collectible placement
Map generation
Combat aim
Movement
Minimap
Dodge i-frames
Ending branches
Secret badge HUD
```

## Required QA

- Compile.
- Run Create Clean Maze Prototype Scene.
- Enter Play Mode.
- Approach a protected battery collectible.
- Verify smoke/ring effect appears before enemies are active.
- Verify enemies do not damage immediately before the delay ends.
- Verify enemies activate after the delay.
- Verify no Console errors.

## Progress

```text
Current stage: 10 / 36
Completed if QA passes: 10 / 36
Remaining: 26 / 36
Progress: 27.8%
```

## Next stage

Stage 11 / 36 — Battery encounters hardening.
