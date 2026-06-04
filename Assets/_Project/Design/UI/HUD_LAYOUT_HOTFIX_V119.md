# HUD Layout Hotfix V119

## Request

Move the ammo/reload menu upward and return the minimap to its original place.

## Changed

```text
Ammo / Reload HUD: top-right
Minimap: bottom-right
```

## Preserved

The V118 minimap discovery hard-fix is preserved:

```text
- discovers current room by player world position
- nearest-room fallback
- explored X/Y counter
- stronger discovered-room visibility
```

The V118 dodge i-frames are preserved:

```text
dodgeInvulnerabilityExtraTime = 0.14
BDPlayerDodgeIFrameFeedback
```

## Not changed

```text
Gameplay
Movement
Combat aim
Inventory
Collectibles
Enemies
Camera
Map generation
Cinematics
```
