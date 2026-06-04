# Minimap Visibility Hotfix V117

## Problem

The minimap could appear empty even after exploration.

The old behavior relied mostly on `BDMinimapRoom` trigger events. If the trigger did not fire reliably, no room was marked as discovered.

## Fix

V117 keeps trigger discovery but adds a robust world-position fallback:

```text
Every Update:
- find the player
- find the minimap room that contains the player world position
- force-discover that room
- if nothing is discovered yet, force-discover nearest room
```

## Visual readability

- Discovered room color is stronger.
- Current room gets a clear outline.
- Discovered rooms get an outline.

## Not changed

```text
Gameplay
Movement
Combat
Inventory
Collectibles
Enemies
Camera
Map generation
Ammo HUD
Cinematics
```
