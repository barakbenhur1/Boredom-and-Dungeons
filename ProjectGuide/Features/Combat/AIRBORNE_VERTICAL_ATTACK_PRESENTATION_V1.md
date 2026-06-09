# Airborne Vertical Attack Presentation V1

## Core contract

- An airborne sword attack keeps the identity of the input that committed it.
- A light attack remains the fast cyan/light presentation.
- A heavy attack remains the wider orange/heavy presentation.
- While airborne, the slash plane becomes **vertical** instead of spawning the normal horizontal ground slash.
- The visual is an attack arc, not a generic landing cube or ring.

## Damage and ownership

- Airborne presentation does not create a second damage owner.
- The existing melee attack remains responsible for hit detection, damage, cooldown, and attack buffering.
- The existing descending/landing damage multiplier is preserved only when its prior gameplay conditions are true.
- A normal upward or level airborne attack changes presentation only; it does not receive unapproved bonus damage.
- Exactly one visual is emitted for the committed light or heavy attack.

## Verification

1. Jump and press light: a vertical light slash appears with light timing and identity.
2. Jump and press heavy: a vertical heavy slash appears with heavy timing and identity.
3. No horizontal slash is duplicated while airborne.
4. Buffered airborne attacks preserve the same behavior.
5. Ground attacks retain their existing horizontal presentation.
6. Damage and cooldown values remain owned by the existing combat system.

## V23R11 committed-animation repair

- The actual committed melee transaction now chooses the presentation; an initial hold-capable button press does not emit an early slash.
- Light-hold/spin and heavy-hold/hook input can wait for release/threshold without allowing a later horizontal arc to leak through.
- The airborne slash uses the same mesh, color identity, width language, and fade behavior as the corresponding grounded light/heavy slash.
- The mesh is built in a **vertical high-to-low** plane and travels from high to low. It is not a camera-facing LineRenderer substitute.
- Standard horizontal visual suppression lasts through the real committed attack and is consumed exactly once.
- Status: **IMPLEMENTED in V23R11; Unity visual verification required before PASS.**

## V23R17 committed body-animation clarification

- Air Light and Air Heavy now own distinct `BD_Player_Visual` body-animation timelines, not only rotated slash VFX.
- Light uses a short wind-up and fast vertical chop.
- Heavy uses a deeper overhead wind-up, heavier vertical slam, stronger compression, and longer recovery.
- Parry, menu, death, disable, and reset restore the visual rest pose.

## V23R19 explicit presentation identity repair

- The committed melee transaction returns an explicit airborne/grounded presentation identity.
- Airborne Light and Heavy invoke the dedicated body timeline and vertical slash branch directly.
- The normal horizontal slash function is not invoked for the same airborne attack; correctness no longer depends on a timing-based suppression flag.
- Grounded attacks continue to use only the normal horizontal presentation.


## V23R19D front-facing downward-strike refinement

- The airborne slash appears fully in front of the player and faces the same horizontal direction as the player.
- Its plane remains vertical and square to the facing direction; it does not roll diagonally around the body.
- The strike travels from overhead to near floor level.
- The body presentation uses an overhead wind-up and a forward/downward chop without sideways roll.
- Light and Heavy keep their approved color, timing, width, damage, and cooldown identities.

## V23R19E exact visual identity refinement

- The airborne attack must use the exact Light or Heavy slash selected by the player.
- Its mesh, arc angle, thickness, range identity, color, and heavy/light distinction match the grounded attack.
- The airborne variant is produced by rotating that same attack exactly 90 degrees so it is perpendicular to the player model, placing it in front, and moving it toward the floor.
- Do not substitute a narrow custom vertical arc and do not spawn the grounded horizontal slash at the same time.

## V23R19M exact long-axis orientation

- The grounded attack reads as a line whose long axis runs left-to-right, parallel to the floor.
- The airborne attack uses the same Light/Heavy geometry, but rotates the **long axis itself** from left-to-right into top-to-bottom.
- In the current mesh coordinate system, the long axis is local X and forward/depth is local Z. Therefore the correct conversion is a 90-degree rotation around the **local Z axis**, not local X.
- Local Z remains pointed in the player's attack direction while local X becomes world up/down.
- The result must read as one vertical top-to-bottom attack line, perpendicular to the floor, directly in front of the player.
- Rotating only the plane while leaving the long axis horizontal does not satisfy this contract.
