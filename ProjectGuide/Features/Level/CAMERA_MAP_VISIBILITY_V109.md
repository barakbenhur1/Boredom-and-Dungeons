# Camera Map Visibility and 40/60 Composition

## Goal

Normal gameplay must show more useful space in front of the player or horse than behind them without allowing room-boundary compression to create camera jumps.

## Stable framing

```text
distanceBehind: 15.25
height:         17.75
camera FOV:     58
target viewport height: 0.40
```

The target is composed at approximately 40% of the viewport height from the bottom/back side, leaving approximately 60% of the visible space ahead.

## Dynamic pitch

- `BDCameraFollow` derives pitch from the final target/camera geometry, current FOV, and the explicit `targetViewportHeight01 = 0.40f` contract.
- Pitch is clamped to a calm gameplay range, but legacy scene instances that still serialize the old 50-degree minimum are migrated at runtime to a maximum minimum of 35 degrees.
- Room transitions, wall pressure, mounted target changes, and camera diagnostics must not move the target back to screen center.
- The 40/60 composition is applied to normal smoothing and snap-to-target paths.

## Preserved behavior

- Mouse aim and mounted steering remain separate input concerns.
- `BDCameraFollow` remains the only normal-gameplay camera transform owner.
- V23R6 actual-pose handoff release, closed-wall visibility, stable FOV, cinematic ownership, and transition diagnostics remain intact.
- The authored room, corridor, doorway, hazard, portal, enemy, and minimap geometry is not regenerated for composition.

## Verification

1. Stand and walk in the center of a room: the target remains near viewport Y 0.40.
2. Rotate through cardinal and diagonal aim directions: composition remains stable.
3. Cross room nodes in both directions on foot and mounted: no return to center, zoom pulse, or direction snap occurs.
4. Confirm closed walls still hide adjacent rooms.
5. Confirm fresh New Game and cinematic ownership remain unchanged.
