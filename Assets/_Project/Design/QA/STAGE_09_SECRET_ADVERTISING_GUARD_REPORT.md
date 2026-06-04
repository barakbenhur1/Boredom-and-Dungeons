# Stage 09 / 36 — Secret Collectible Advertising Guard Report

## Goal

Prevent accidental visible advertising of secret collectibles.

## Done

- Added `BDSecretCollectibleAdvertisingValidator`.
- Added Unity menu validation:
  `Boredom & Dungeons/Validation/Check Secret Collectible Advertising`
- Added forbidden phrase list for gameplay scripts.
- Added rules documentation.
- Kept badge HUD behavior unchanged.

## What this stage protects

```text
No objective text.
No checklist.
No missing item text.
No 0/4 progress list.
No arrows or markers.
No text telling the player to find/collect secrets.
```

## What remains allowed

```text
GB / BAT / CART badges after pickup.
Pickup VFX.
Cinematic consequences.
Environmental hints.
```

## Required QA

- Compile.
- Run validation menu:
  `Boredom & Dungeons/Validation/Check Secret Collectible Advertising`
- Run Create Clean Maze Prototype Scene.
- Enter Play Mode.
- Verify no objective/checklist/missing text appears.
- Verify badge still appears only after pickup.

## Progress

```text
Current stage: 9 / 36
Completed if QA passes: 9 / 36
Remaining: 27 / 36
Progress: 25.0%
```

## Next stage

Stage 10 / 36 — Guardian Spawn VFX.
