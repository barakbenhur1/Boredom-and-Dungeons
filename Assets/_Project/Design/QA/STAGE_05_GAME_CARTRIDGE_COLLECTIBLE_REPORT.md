# Stage 05 / 36 — Game Cartridge Collectible Report

## Goal

Add the actual Game Cartridge collectible type and pickup behavior.

## Done

- Added `GameCartridge` to `BDGameBoyCollectible.CollectibleKind`.
- Added `SpawnGameCartridge(Vector3 position)`.
- Added procedural cartridge placeholder visual.
- Added cartridge pickup logic.
- Cartridge pickup calls `BDGameBoyInventory.CollectGameCartridge()`.
- Added cartridge collect burst color/name.
- Added design documentation.

## Not done yet

```text
Cartridge is not spawned in the map.
Cartridge is not dropped by mini-boss 3 yet.
Badge HUD is not added yet.
Ending variants are not implemented yet.
```

## Not changed

```text
Gameplay
Movement
Combat aim
Mounted shooting
Camera
Map generation
Minimap
Dodge i-frames
Ammo HUD
Enemy placement
Cinematics
Objective text
```

## Required QA

- Compile.
- Run Create Clean Maze Prototype Scene.
- Enter Play Mode.
- Verify existing Game Boy pickup still works.
- Verify existing battery pickup still works.
- Verify no cartridge objective/checklist/marker appears.
- Verify no cartridge appears in the starting room or map yet.

## Progress

```text
Current stage: 5 / 36
Completed if QA passes: 5 / 36
Remaining: 31 / 36
Progress: 13.9%
```

## Next stage

Stage 6 / 36 — Secret Collectible Badge HUD.
