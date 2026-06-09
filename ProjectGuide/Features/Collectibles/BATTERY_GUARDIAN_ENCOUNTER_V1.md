# Boredom & Dungeons — Battery Guardian Encounter V1

Status: recovered and approved requirement, 2026-06-06.

## Core collectible rule

- Both Batteries are placed physically in the map.
- Batteries do not come from reward chests.
- The player can see and approach the Battery itself before the guardian encounter begins.
- The Battery remains an optional hidden collectible and must not be advertised through objectives, empty slots, missing-item UI, or map markers.

## Trigger flow

1. The player enters the Battery encounter trigger radius.
2. Spawn anticipation VFX begins around the encounter area.
3. Guardian enemies are created hidden/inactive or otherwise prevented from attacking immediately.
4. A short anticipation delay gives the player readable warning.
5. Guardians resolve legal positions around the player and Battery.
6. Guardians appear and become active.
7. A short recovery window occurs before attacks begin.
8. The player defeats the guardians and can collect the Battery.

Existing timing direction from the recovered roadmap:

- approximately `0.5s` anticipation before reveal;
- approximately `0.3s` recovery after reveal before AI attacks;
- final values remain subject to Play Mode balance.

## Spawn shape and fairness

Guardians spawn around the player, but never directly adjacent to the player.

Every spawn point must:

- be outside a minimum safety radius from the player;
- be outside a minimum safety radius from the Battery;
- avoid overlap with another guardian;
- avoid walls, props, lava, holes, chasms, inaccessible terrain, and active barriers;
- remain inside a legal combat area;
- preserve at least one reachable movement/escape direction;
- avoid placing the player inside an unavoidable immediate attack.

The encounter should feel difficult through enemy count, roles, angles, and pressure—not through unfair contact spawns.

## Placement resolution

- Try multiple angles and distances around the player.
- Score candidates for safety, spacing, line of sight, route pressure, and arena usefulness.
- Prefer positions that pressure more than one direction without forming a complete unavoidable ring.
- If the first candidate is invalid, try alternate positions.
- If no perfect position exists, use the best legal scored fallback.
- If there is no legal fallback, delay or cancel that guardian spawn rather than spawning inside invalid geometry or on top of the player.

## Encounter difficulty

Battery A:

- first serious protected-collectible group;
- difficult but fair;
- introduces the guardian-spawn language clearly.

Battery B:

- harder than Battery A;
- more pressure, stronger role combination, or more demanding angles;
- still obeys the same safety and anticipation rules.

Difficulty may be increased through:

- more guardians within a validated cap;
- complementary enemy roles;
- wider attack angles;
- stronger pursuit/exit pressure;
- shorter but still readable recovery windows.

Difficulty must not be increased through:

- contact-distance spawns;
- invisible attacks;
- spawn overlap;
- unavoidable hazard placement;
- complete escape-route closure;
- repeated infinite spawning.

## Encounter lifecycle

- Each Battery encounter activates once per run.
- Guardians do not appear before the player enters the trigger.
- Guardians do not spawn endlessly.
- Leaving and re-entering the radius must not duplicate the encounter.
- The Battery does not move into a chest after victory.
- The Battery remains at its map position and becomes safely collectible.
- Encounter state resets correctly on full run restart.

## VFX/audio direction

Possible spawn presentation:

- smoke;
- teleport circle/ring;
- thickening shadow;
- red or purple light;
- particles;
- spawn sound;
- final reveal flash.

VFX must not hide hazard edges, the player, or the only safe direction.

## QA requirements

- Verify both Batteries exist physically on the map.
- Verify neither Battery is delivered through a chest.
- Verify trigger activates only on player approach.
- Verify enemies spawn around the player at fair distances.
- Verify no guardian spawns too close to the player or Battery.
- Verify no overlap with guardians, walls, props, lava, holes, chasms, or barriers.
- Verify at least one reachable movement route remains.
- Verify Battery A is hard but fair.
- Verify Battery B is harder without violating safety rules.
- Verify no duplicate activation or infinite spawn.
- Verify the Battery can be collected after the encounter.
- Verify hidden-collectible UI rules remain intact.


## V23R19B runtime activation and combat classification

- A guardian is constructed as one inactive GameObject with its CharacterController, health, AI archetype, grounding, collision discipline, tactical stack, and combat profile already present.
- The root becomes active atomically only after the anticipation delay and at the final legal spawn position. Partial component disabling or below-floor live initialization is prohibited.
- On activation, the guardian must immediately acquire the player, move, attack, expose a valid damage collider, lose health from player attacks, show hit feedback, and die through the normal `BDHealth` path.
- Battery guardians are `Elite`, not small regular enemies. They are immune to grappling pull, player/horse knockback, and other small-enemy-only forced movement while remaining fully damageable, stagger-readable, and killable.
- Their elite classification does not turn them into bosses or grant damage immunity.

## V23R19E collectible-independent reveal repair

- The delayed guardian reveal must not run on the collectible GameObject because collecting the Battery destroys that owner.
- `BDCollectibleGuardianSpawner` constructs every guardian completely while inactive, then `BDGuardianSpawnSequence` finishes the delay and activates it from a separate scene object.
- Exact collectible-room ownership remains preferred. When a hideout pickup lies just outside exact minimap-room bounds, fallback is allowed only to the player's containing room, within bounded trigger distance, with a clear physical path.
- Collecting the Battery during the reveal never cancels guardians. Adjacent-room players behind a wall can never trigger the encounter.
- The normal radius check remains the early trigger, and player `OnTriggerEnter` is the final same-frame trigger before pickup destruction; both paths converge on the same one-shot spawn method.
