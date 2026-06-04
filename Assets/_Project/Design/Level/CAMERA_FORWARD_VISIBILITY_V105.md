# Camera Forward Visibility V105

Immediate tuning fix.

## Problem

The player cannot see far enough forward.

This is not a mouse/front-cone/aim problem. It is a camera composition problem.

## Fix

The camera now shows more space ahead of the player:

```text
distanceBehind = 11.25
height = 13.25
lookAhead = 4.75
minPitch = 46
maxPitch = 64
```

## Not changed

```text
mouseModelFrontConeDegrees unchanged
mountedMouseModelFrontConeDegrees unchanged
W/S/A/D behavior unchanged
aim behavior unchanged
combat behavior unchanged
map/AI/HUD unchanged
```
