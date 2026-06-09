# Dodge I-Frame Projectile Rule

## Purpose

During dodge invulnerability frames, the player should be able to pass through bullets/projectiles that were fired at them.

This is a planning rule only. It is not implemented in code yet.

## Required behavior

```text
If the player is dodging and i-frames are active:
- enemy bullets should not damage the player
- enemy bullets should not block the dodge
- the player should feel like they pass through bullet-hell patterns cleanly
```

```text
If i-frames are not active:
- bullets should damage normally
- bullets should collide/resolve normally according to projectile rules
```

## Design reason

Mini-boss 1 is planned to use bullet-hell patterns around the player. The dodge must be a reliable defensive answer during the active i-frame window.

## Important distinction

This rule is about player damage / projectile interaction during dodge i-frames.

It does not mean bullets should pass through walls.

Existing rule still stands:

```text
Bullets should hit walls and should not pass through walls.
```

## QA checklist for implementation

```text
Dodge through enemy bullets while i-frames are active: no damage.
Stand still inside enemy bullet path: damage happens.
Dodge after i-frame window ended: damage happens.
Bullets still collide with walls.
No permanent invulnerability after dodge.
No console errors.
```
