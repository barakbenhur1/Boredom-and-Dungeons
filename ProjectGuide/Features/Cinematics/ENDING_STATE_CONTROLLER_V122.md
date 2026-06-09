# Ending State Controller V122

## Goal

Add a clean decision layer for the final ending.

## Ending inputs

```text
hasGameBoy
batteryCount >= requiredBatteries
hasGameCartridge
```

## Ending variants

```text
NoGameBoy
GameBoyNoBatteries
PoweredNoCartridge
PoweredWithCartridge
```

## Meaning

### NoGameBoy

The player reaches the ending without the Game Boy.

### GameBoyNoBatteries

The player has the Game Boy but not enough batteries.

### PoweredNoCartridge

The player has Game Boy + enough batteries, but no Game Cartridge.

Later cinematic behavior:
```text
Game Boy turns on with gray/white/no-game light.
```

### PoweredWithCartridge

The player has Game Boy + enough batteries + Game Cartridge.

Later cinematic behavior:
```text
Game Boy turns on with colorful game-loaded lights.
```

## Important

V122 only adds the decision infrastructure.

It does not yet implement the four cinematic branches. That is the next stage.
