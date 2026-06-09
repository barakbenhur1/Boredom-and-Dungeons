# Ending Branches V123

## Goal

Connect `BDGameEndingStateController` to the actual procedural ending cinematic.

## Implemented branches

```text
NoGameBoy
GameBoyNoBatteries
PoweredNoCartridge
PoweredWithCartridge
```

## Branch behavior

### NoGameBoy

The player walks to the chair, sits, and nothing is pulled out.

### GameBoyNoBatteries

The player sits, pulls out the Game Boy, it has no power, and it is placed aside.

### PoweredNoCartridge

The player sits, pulls out the Game Boy, inserts batteries, and the screen lights up gray/white to show there is power but no game to play.

### PoweredWithCartridge

The player sits, pulls out the Game Boy, inserts batteries, inserts/loads the Game Cartridge, and the screen cycles colorful lights to show the game loaded.

## Important

This is still procedural prototype cinematic work.

Later passes should improve:
- camera composition
- hand animation
- sitting animation
- fade in/out
- sound
- final visual assets
