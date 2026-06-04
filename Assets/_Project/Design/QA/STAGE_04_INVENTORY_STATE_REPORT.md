# Stage 04 / 36 — Inventory State Report

## Goal

Expand the secret collectible inventory state for Game Cartridge and future ending variants.

## Done

- Added `BDSecretCollectibleKind`.
- Added `hasGameCartridge`.
- Added `CollectGameCartridge()`.
- Added `HasEnoughBatteries()`.
- Added `HasPoweredGameBoy()`.
- Added `HasPlayableGameBoy()`.
- Added generic `Collect(BDSecretCollectibleKind kind)`.
- Added `HasCollected(BDSecretCollectibleKind kind)`.
- Added `SecretCollected` event for future badge HUD.

## Not changed

```text
Gameplay
Movement
Mouse attacks
Mounted shooting
Camera
Map layout
Collectible placement
Enemy placement
Ammo HUD
Cinematic behavior
Ending behavior
```

## Required QA

- Compile.
- Run Create Clean Maze Prototype Scene.
- Enter Play Mode.
- Verify Game Boy and battery pickup still work.
- Verify existing cinematic still starts with Game Boy + 2 batteries.
- Verify no objective text or collectible checklist appears.

## Progress

```text
Current stage: 4 / 36
Completed if QA passes: 4 / 36
Remaining: 32 / 36
Progress: 11.1%
```

## Next stage

Stage 5 / 36 — Game Cartridge collectible.

That stage should create the actual cartridge collectible visual and pickup behavior, but still must not advertise it to the player.
