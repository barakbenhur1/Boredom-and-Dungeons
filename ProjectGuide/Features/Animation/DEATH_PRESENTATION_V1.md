# Player and Enemy Death Presentation V1

Status: V23R19E working Runtime implementation; final production-authored animation remains required.

## Player

- Lethal damage immediately disables player movement, combat, interaction, and solid collision.
- The gameplay view remains visible while the player performs a readable fall/death pose.
- The Game Boy main/death menu may appear only after the pose and short final hold complete.
- Death presentation uses unscaled time so hit-stop or time effects cannot skip or strand it.

## Regular enemies

- On death, AI, attacks, movement, and collision stop immediately.
- A readable death fall plays before loot release and object destruction.
- Sword, Patrol, Charger, Trap, Ranged, Jumper, Exit-blocker, and Battery-guardian actors use the shared regular-enemy contract.
- Bosses and mini-bosses with authored sequences keep their specialized death owners.

## Production requirement

The procedural V23R19E fall is a functional presentation and regression-safe timing owner. It is not a substitute for final authored character-specific death animations, impact reactions, audio, VFX, and animation blending.

## V23R19M small regular-enemy death readability

### Small regular enemies

- Small regular enemies use an intact-body recoil and loss-of-balance fall.
- The body first reacts briefly, then tips and settles toward the floor.
- The final pose preserves almost all body proportions so the enemy still reads as the same character.
- Do not compress a small enemy into a flat pancake, rubber blob, or sudden scale collapse.
- Loot and despawn still wait for the death motion to complete.
- Large enemies, Elite guardians, mini-bosses, bosses, and the player keep their existing specialized/current death paths unless separately changed.
