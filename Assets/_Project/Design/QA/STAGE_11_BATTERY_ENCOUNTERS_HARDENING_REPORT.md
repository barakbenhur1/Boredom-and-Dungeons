# Stage 11 / 36 — Battery Encounters Hardening Report

## Goal

Make battery guardian encounters harder, clearer, and fairer.

## Done

- Increased default battery guardian encounter pressure.
- Added fair spawn position resolver.
- Added minimum distance from player.
- Added minimum distance from collectible.
- Added minimum spacing between guardians.
- Added alternate angle/distance candidate search.
- Added best fallback scoring if all candidates are imperfect.
- Preserved V125 spawn VFX and activation delay.

## Not changed

```text
Collectible placement
Map generation
Secret advertising rules
Minimap
Dodge i-frames
Combat aim
Mounted shooting
Ending branches
Badge HUD
```

## Required QA

- Compile.
- Run Create Clean Maze Prototype Scene.
- Enter Play Mode.
- Approach each protected battery.
- Verify guardians spawn once.
- Verify enemies do not spawn directly on top of the player.
- Verify enemies do not instantly damage before spawn VFX delay ends.
- Verify encounter is harder but still readable.
- Verify no Console errors.

## Progress

```text
Current stage: 11 / 36
Completed if QA passes: 11 / 36
Remaining: 25 / 36
Progress: 30.6%
```

## Next stage

Stage 12 / 36 — Mini-boss 1 design: Game Boy guardian.
