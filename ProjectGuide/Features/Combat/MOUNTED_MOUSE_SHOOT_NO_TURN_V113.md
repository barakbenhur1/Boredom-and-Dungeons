# Mounted Mouse Shoot No Turn V113

## Goal

Mounted shooting should shoot toward the mouse point, but the horse must not rotate because of the shot.

## Rules

On foot:
```text
attack direction = mouse world point
player rotates to attack direction
```

Mounted:
```text
shot direction = mouse world point
horse direction unchanged
mounted movement unchanged
no mounted body/horse turn caused by shooting
```

## Changed

`BDPlayerCombat.ApplyCombatFacing` now returns early while mounted.

## Not changed

```text
W/S/A/D movement unchanged
camera unchanged
dodge unchanged
collectibles unchanged
HUD unchanged
map unchanged
```
