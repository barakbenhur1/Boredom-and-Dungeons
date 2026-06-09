# Damage Numbers and Prototype Test-Label Visibility V1

## Purpose

Provide immediate, readable combat feedback without clutter, and prevent prototype hazard instructions from leaking through walls or confusing adjacent rooms.

## Damage-number implementation — DONE IN CODE

- Successful damage to the player and enemies creates one animated world-space number at the damaged actor.
- Zero damage, blocked damage, dodged damage, and successful Parry do not create a false number.
- Player damage uses a clear coral-red (`#FF454D`) identity.
- Enemy damage uses a warm gold/amber (`#FFBA38`) identity.
- Both use a restrained dark shadow for readability without a large box or background plate.
- Values use whole numbers when possible and one decimal only when the real applied damage requires it.
- Repeated hits stack vertically with slight lateral variation so combo damage remains legible.
- The animation performs a short pop, upward drift, settle, and fade over roughly one second.
- Animation uses unscaled time so Parry slow motion does not leave numbers frozen on screen.
- Numbers disappear immediately when gameplay HUD ownership is not active.
- Damage numbers are presentation only; `BDHealth` remains the sole health/damage authority.

## Prototype-label visibility — DONE IN CODE

- Hole, lava, quicksand, and future first-room test labels remain prototype guidance, not permanent HUD.
- Labels are visible only while the player is near them.
- A camera-to-label visibility ray hides the label behind walls and solid blockers.
- Labels also hide in menu, death, pause, intro, and other non-gameplay UI states.
- Existing generated labels are upgraded at runtime; newly generated labels receive the visibility component from the scene installer.
- The obstacle itself remains visible according to normal room/camera rules; only the explanatory test text is gated.

## Visual quality rules

1. Damage numbers never cover the crosshair, horse prompts, boss bars, or menu content.
2. Player and enemy colors remain distinct in bright and dark biomes.
3. No giant critical-style scaling is introduced unless a later critical-hit system explicitly requires it.
4. Prototype labels never reveal hazards in a room the player cannot currently see.
5. Production levels may disable/remove prototype labels entirely without changing hazard behavior.

## Verification

- Damage the player with multiple enemy archetypes and confirm red animated values.
- Damage regular enemies, mini-bosses, and bosses with sword, projectile, hook, bomb, and hazard-friendly fire; confirm amber animated values.
- Trigger rapid multi-hit damage and confirm readable stacking without permanent text.
- Perform Parry/slow motion and confirm numbers continue and clean up normally.
- Stand near each prototype hazard, then move behind a wall and into an adjacent room; the label must hide.
- Open menu/pause/death while near a label; no test text may remain visible.
