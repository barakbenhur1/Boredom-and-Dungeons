# Boredom & Dungeons — Production Animation Requirements V1

## Authority

This document defines mandatory production-animation coverage for the shipped game and complements the canonical visual language in `ART_DIRECTION.md`.

Prototype transforms, temporary procedural posing, debug motion and placeholder clips may prove gameplay, but they are not final release animation.

## Core rule

Every gameplay action that benefits from visible motion requires an authored, production-quality animation or a deliberately approved procedural-animation solution.

The final result must communicate anticipation, action, impact, recovery, weight, direction, interruption rules and gameplay timing from the real gameplay camera. A feature is not visually complete merely because its hitbox, movement, cooldown or state transition works.

## Player animation coverage

The final player set must cover every applicable action, including:

- idle variants and breathing;
- walking, running, acceleration, deceleration, stopping, turning and direction changes;
- jump anticipation, ascent, apex, fall and landing;
- wall contact and wall jump with a readable push away from the wall;
- dodge anticipation, travel and recovery;
- light sword attack, heavy sword attack and combo transitions;
- spinning AOE attack;
- airborne light and airborne heavy attacks with distinct vertical body mechanics;
- Parry preparation, success reaction and recovery;
- ranged aiming, normal firing, charged firing and recoil;
- grappling-hook launch, pull reaction and recovery;
- item use, interaction and collectible pickup;
- damage reaction, knockback, stagger, stun, launch, fall, death and recovery;
- hazard struggle, sinking, burning, falling and respawn;
- mounting, mounted idle, dismounting and emergency hazard separation.

## Horse animation coverage

The final horse set must cover every applicable action, including:

- idle, breathing, head motion and natural weight shifts;
- walk, trot, run, gallop, acceleration, deceleration, stop and turning;
- mounted and unmounted locomotion transitions;
- jump, ascent, landing and unsafe-jump refusal;
- player mounting and dismounting;
- petting from the ground and mounted petting while stationary;
- healing, revival and recovery;
- fear, threat reaction, retreat and flee behavior;
- damage reaction, mounted impact, knockback resistance, stagger, stun, bucking and fainting;
- hole/chasm fall, lava contact, quicksand struggle and safe recovery;
- cinematic entrance, clear-direction turn and final stop.

## Enemy and boss animation coverage

Every enemy archetype, mini-boss and boss must receive the relevant final set:

- idle, patrol, awareness, alert and target acquisition;
- walk, run, strafe, turn and stop;
- anticipation/telegraph for every attack;
- attack execution and recovery;
- projectile preparation, release and recoil;
- trap placement and bomb throw/placement;
- charge preparation, charge, impact, miss and recovery;
- jump, launch, landing and aerial states;
- damage reaction, knockback, stagger, stun, guard break and interruption;
- normal-ground death;
- hazard-specific death for hole/chasm, lava and quicksand;
- phase transitions, enrages and unique boss reactions;
- Mother phase-specific movement and attack language.

## Interaction and world animation coverage

Production animation must also be evaluated for:

- doors, portals, chests, destructible objects and environmental mechanisms;
- breaking wood, stone, crates, pots and other materials;
- pickups, cartridges, batteries and secret objects;
- lava, quicksand, traps, bombs and hazard telegraphs;
- UI transitions, Game Boy boot, menu selection and post-victory device transformation;
- camera accents and VFX synchronization where motion readability depends on them.

## Technical requirements

- Gameplay state owns timing; animation must not invent extra damage or movement.
- Root motion is opt-in per action and must have a documented owner.
- Hit frames, projectile release, foot plants and interaction contact use animation events or an equivalent deterministic contract.
- Blend trees and transitions must avoid foot sliding, pose popping and accidental double playback.
- Interrupt rules must be explicit for damage, Parry, death, pause, menu, hazard recovery and scene changes.
- Mirroring is allowed only when anatomy, weapon side and readability remain correct.
- Animation-speed changes must preserve hit timing and audiovisual synchronization.
- Final clips must read from the approved top-down camera and at gameplay scale.
- Mobile performance budgets, skeleton complexity and active animator counts must be respected.
- Accessibility options for screen shake, flashes and motion intensity must not break animation readability.

## Placeholder and procedural policy

Procedural motion may remain in the final game only when it is visually intentional, stable, testable and equal to authored production quality.

A temporary rotation, scale pulse, transform lerp or primitive body tilt used during prototyping must be replaced or explicitly approved before release.

Temporary procedural animation is not final release animation.

<!-- temporary procedural animation is not final release animation -->

## Production acceptance gate

An animation set is approved only when:

1. the action is visually distinct from other actions;
2. anticipation and recovery match gameplay timing;
3. body, weapon and facing agree with the hit direction;
4. there is no foot sliding, snapping, frozen pose or double animation;
5. interruption and recovery paths are safe;
6. sound, VFX, damage and camera accents synchronize with the motion;
7. it reads clearly from the real gameplay camera;
8. it performs within target desktop and mobile budgets;
9. it has passed repeated Play Mode verification;
10. placeholder animation debt remains recorded until replaced.

## Player and enemy death animation requirement

- The player must have a readable death animation before any death/main-menu overlay appears.
- Regular enemies must stop gameplay, show a readable death animation, and only then release loot/despawn.
- Boss and mini-boss authored death sequences remain specialized encounter responsibilities.
- The V23R19E procedural fall is a working Runtime presentation and does not remove the requirement for final production-authored death animation.
