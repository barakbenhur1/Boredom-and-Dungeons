# Bomb Explosion and Enemy Friendly Fire V1

## Core contract

- Trap-layer bombs visibly explode; destroying the sphere without an explosion effect is invalid.
- The explosion has a bright core, expanding ground shock ring, outward sparks, impact audio, and restrained camera shake when the player is near.
- The visual radius communicates the real gameplay radius.

## Damage ownership

- `BDBombHazard` owns one explosion transaction and cannot explode twice.
- The player and horse retain their established damage routing.
- The same explosion also damages enemy `BDHealth` targets once each, even when they have multiple colliders.
- The trap-layer enemy that created the bomb is excluded from its own bomb; other enemies are valid friendly-fire targets.
- Enemy hits receive a short stagger, flash, radial knockback, and ground-safe motion handling.
- Walls and future cover rules may refine exposure later, but visual and gameplay radii must remain synchronized.

## Verification

1. A placed bomb visibly arms, pulses, and produces an unmistakable explosion.
2. Player damage occurs once per explosion.
3. Horse damage follows the existing horse damage utility.
4. Two nearby ordinary enemies each take one damage event.
5. Multiple colliders do not cause repeated damage to one enemy.
6. The bomb owner does not damage itself.
7. Enemy knockback does not create floating, teleport, or impossible-speed states.
8. VFX, audio, and temporary objects clean themselves up.
