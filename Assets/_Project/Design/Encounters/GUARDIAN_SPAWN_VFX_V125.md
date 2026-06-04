# Guardian Spawn VFX V125

## Goal

Guardians should not pop into existence instantly when the player approaches a protected collectible.

## New behavior

When the player enters a collectible guardian radius:

```text
1. A teleport/smoke ring appears on the ground.
2. The enemy is created hidden below the floor and inactive.
3. A short anticipation delay runs.
4. The enemy moves to the spawn position.
5. Enemy AI and collision become active.
6. A short final flash appears.
```

## Why

This makes optional collectibles feel protected by a designed encounter rather than random enemy pop-in.

## Current values

```text
spawnVfxDelay = 0.95
inactiveSinkDepth = 1.65
spawnVerticalOffset = 1.05
```

## Not changed

```text
No enemy counts changed.
No enemy damage changed.
No collectible placement changed.
No map generation changed.
No UI changed.
```
