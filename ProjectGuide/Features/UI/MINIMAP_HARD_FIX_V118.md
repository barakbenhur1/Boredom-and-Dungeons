# Minimap Hard Fix V118

## Problem

The minimap still did not appear to update after exploration.

## Fix

V118 makes minimap discovery independent from trigger reliability:

```text
Every Update and OnGUI:
- resolve player using BDPlayerMarker directly
- refresh minimap rooms if needed
- discover room containing player
- if containment fails, discover nearest room within range
```

## Visibility

The minimap is moved to the top-right so the ammo/reload HUD cannot cover it.

It now shows:

```text
Explored X/Y
current-room outline
stronger discovered room color
```

## Not changed

```text
Map generation
Gameplay
Movement
Combat
Inventory
Collectibles
Enemies
Camera
```
