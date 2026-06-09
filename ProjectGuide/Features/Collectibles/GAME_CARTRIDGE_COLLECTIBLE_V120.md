# Game Cartridge Collectible V120

## Goal

Add the Game Cartridge as a real secret collectible type.

## Added

```text
BDGameBoyCollectible.CollectibleKind.GameCartridge
BDGameBoyCollectible.SpawnGameCartridge(Vector3 position)
Game cartridge procedural placeholder visual
Game cartridge pickup calls BDGameBoyInventory.CollectGameCartridge()
Game cartridge collect burst
```

## Important design rule

The Game Cartridge is secret.

Do not:
```text
show objective text
show checklist
show marker
tell the player to find it
spawn it in the starting room
```

## Placement rule

The cartridge is not placed in the map yet.

Future behavior:
```text
Mini-boss 3 drops the Game Cartridge.
The player only sees the badge after pickup.
```

## Ending use later

The Game Cartridge will affect the powered Game Boy ending:

```text
Game Boy + 2 batteries, no cartridge:
gray/white/no-game light

Game Boy + 2 batteries + cartridge:
colorful game-loaded light
```
