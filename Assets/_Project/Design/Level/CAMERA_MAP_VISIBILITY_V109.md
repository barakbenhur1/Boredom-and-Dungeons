# Camera Map Visibility V109

## Problem

The previous camera tuning changed pitch, but the actual problem was map coverage:
the player still did not see enough of the map ahead.

## Fix

V109 changes framing, not mouse control:

```text
distanceBehind: 11.25 → 15.25
height:         13.25 → 17.75
lookAhead:       4.75 →  6.75
camera FOV:        48 →    58
```

Pitch is not pushed further. It is kept slightly calmer:

```text
minPitch = 50
maxPitch = 68
```

## Not changed

```text
mouse controls unchanged
mouseModelFrontConeDegrees unchanged
mountedMouseModelFrontConeDegrees unchanged
W/S/A/D unchanged
aim unchanged
combat unchanged
AI unchanged
HUD unchanged
Game Boy cinematic unchanged
```
