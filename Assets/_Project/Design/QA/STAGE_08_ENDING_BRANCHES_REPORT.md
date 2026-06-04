# Stage 08 / 36 — Four Procedural Ending Variants Report

## Goal

Implement the four procedural ending variants.

## Done

- Ending trigger no longer requires Game Boy + batteries to start.
- Ending cinematic evaluates `BDGameEndingState`.
- Added NoGameBoy branch.
- Added GameBoyNoBatteries branch.
- Added PoweredNoCartridge branch.
- Added PoweredWithCartridge branch.
- Added procedural cartridge load visual for the full ending.
- Added gray/white powered screen for no-cartridge ending.
- Added colorful screen cycle for cartridge ending.

## Not done yet

```text
No final animation assets.
No final camera cuts.
No final audio.
No fade system.
No boss/exit gating yet.
```

## Required QA

- Compile.
- Run Create Clean Maze Prototype Scene.
- Enter Play Mode.
- Enter ending room with no secrets: verify sit-only branch.
- Enter with Game Boy but not enough batteries: verify Game Boy aside branch.
- Enter with Game Boy + 2 batteries and no cartridge: verify gray/white powered branch.
- Enter with Game Boy + 2 batteries + cartridge later when cartridge can be acquired/spawned: verify colorful branch.
- Verify controls return after cinematic.

## Progress

```text
Current stage: 8 / 36
Completed if QA passes: 8 / 36
Remaining: 28 / 36
Progress: 22.2%
```

## Next stage

Stage 9 / 36 — Remove/avoid visible collectible advertising.
