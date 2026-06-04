# Stage 06 / 36 — Secret Collectible Badge HUD Report

## Goal

Add badges for collected secret collectibles without advertising uncollected ones.

## Done

- Added `BDSecretCollectibleHud`.
- Added it to `BD_GameHud`.
- Shows nothing before first secret pickup.
- Shows `GB` only after Game Boy pickup.
- Shows `BAT` / `BAT xN` only after battery pickup.
- Shows `CART` only after cartridge pickup.
- Added short pulse animation on newly collected badge.
- Added documentation.

## Not done yet

```text
No empty slots.
No checklist.
No objective text.
No marker.
No cartridge map placement.
No mini-boss 3 drop yet.
No ending branch changes yet.
```

## Required QA

- Compile.
- Run Create Clean Maze Prototype Scene.
- Enter Play Mode.
- Before pickup: verify no secret badge row appears.
- Pick up Game Boy: verify `GB` appears.
- Pick up battery: verify `BAT` / `BAT x2` appears.
- Verify no “missing” or “0/4” text appears.
- Verify ammo HUD and minimap still appear in correct positions.

## Progress

```text
Current stage: 6 / 36
Completed if QA passes: 6 / 36
Remaining: 30 / 36
Progress: 16.7%
```

## Next stage

Stage 7 / 36 — Ending State Controller.
