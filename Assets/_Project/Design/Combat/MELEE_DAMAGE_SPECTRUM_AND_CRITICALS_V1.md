# Boredom & Dungeons — Melee Damage Spectrum and Criticals V1

## Status

Implemented in code under V23R15/V23R15B. Unity compilation, TEST EVERYTHING, and focused Play Mode verification remain required.

## Sword-only damage spectrum

Player sword attacks do not use one fixed value. Every committed sword attack rolls once inside a configurable spectrum around its authored base damage. The default spectrum is ±10%. The same attack-level spectrum roll is shared by every enemy hit by that swing. For spinning AOE only, the critical roll is resolved independently per valid enemy target.

Eligible attacks:

- normal/light sword attack;
- heavy sword attack;
- airborne light attack;
- airborne heavy attack;
- spinning sword AOE.

Projectiles and the grappling hook remain fixed-damage systems. They never use the sword spectrum and never roll a sword critical. Weapon progression multipliers may still deliberately modify their authored fixed base damage; random variance does not.

## Critical attack contract

- Every committed eligible sword attack has exactly a 6% critical chance.
- A critical multiplies the resolved spectrum damage by exactly 1.5.
- Light, heavy, airborne light, and airborne heavy roll critical once per committed attack.
- A spinning AOE performs an independent critical roll for every valid enemy hit. One enemy may crit while another enemy hit by the same spin does not.
- Duplicate colliders belonging to the same enemy never create extra critical rolls or extra damage.
- Missed attacks may roll internally but apply no damage and display no number.
- Hook, bullets, charged shots, bombs, hazards, enemy attacks, and horse damage cannot use this player-sword critical path.

## Damage-number identity

- player damage received: coral red;
- normal damage dealt to enemies: amber gold;
- critical sword damage: vivid fuchsia/magenta.

Critical numbers keep the same short pop/rise/fade language and are slightly more prominent without becoming oversized, adding an opaque box, or changing with distance.

## Acceptance

1. Light, heavy, airborne light/heavy, and spin produce varying values around their configured base.
2. Ranged shots and the grappling hook remain numerically fixed for identical conditions.
3. Statistical sampling over many sword attacks approaches 6% criticals.
4. A critical is exactly 1.5 times that attack's pre-critical resolved damage.
5. One multi-target spin uses one shared spectrum roll, while every unique enemy hit receives its own independent 6% critical roll.
6. Critical numbers use the dedicated fuchsia color.
