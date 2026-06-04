# Stage 04A — Minimap Hotfix Report

## Goal

Fix minimap not revealing anything after exploration.

## Done

- Added `BDMinimapRoom.ContainsWorldPosition`.
- Added `BDMinimapRoom.SqrDistanceToCenter`.
- Added `BDMazeMinimap.TickPlayerDiscovery`.
- Added fallback discovery based on player world position.
- Added nearest-room fallback if no room is discovered.
- Improved discovered room color.
- Added current-room outline.

## Required QA

- Compile.
- Create Clean Maze Prototype Scene.
- Enter Play Mode.
- Walk from starting room into nearby rooms.
- Verify minimap reveals current room and previous explored rooms.
- Verify player marker appears.
- Verify minimap does not reveal the full map at once.

## Progress

This is a hotfix during Stage 4 / 36.

```text
Current planned stage: 4 / 36
Completed if QA passes: 4 / 36
Remaining: 32 / 36
Progress: 11.1%
```

## Next planned stage

Stage 5 / 36 — Game Cartridge collectible.
