# Quicksand and Enemy Hazard Behavior V1

## Player quicksand contract

- A grounded player touching quicksand sinks continuously whether standing or walking.
- Horizontal movement slows progressively with sink depth: approximately 78% at entry and 24% near the half-body failure point.
- Every complete second of grounded surface contact deals exactly 2 unavoidable damage.
- Being airborne over the volume pauses sinking and periodic damage.
- Every committed jump extracts a fixed amount; greater depth therefore requires more jumps.
- Dodge pauses additional sinking but does not extract the player.
- At half-body depth the player loses control, receives the configured fall damage, and respawns at the latest legal safe point.
- Quicksand is never a legal safe point.

## Enemy intent and forced entry

- Enemy brain movement treats holes, lava, quicksand, and future registered hazard volumes as forbidden space.
- Enemy movement is swept before execution and may slide along a safe axis rather than enter a hazard.
- Jumping enemies reject hazard landing candidates.
- External force is different from intent: small regular enemies may be knocked or struck by a mounted horse into hazards.
- A small regular enemy landing in a hole or on lava dies immediately.
- A small regular enemy entering quicksand sinks progressively and dies at full submerge.
- Mini-bosses and bosses do not receive the small-enemy environmental disposal contract.

## Horse hole and lava damage ownership — V23R18A

- A horse that reaches a hole/chasm without a rider receives the configured fall damage and returns to its latest legal horse safe point.
- A mounted horse that reaches a hole/chasm receives fall damage; the rider separately receives the configured player fall damage and recovery.
- A horse that reaches lava without a rider receives lava damage and returns to its latest legal horse safe point.
- Mounted lava applies damage to the horse instead of the rider.
- Mounted recovery ends the mounted relationship before teleporting either actor.
- The rider follows a zero-damage lava recovery arc to a separated safe point beside the recovered horse.

### Mounted recovery ordering refinement — V23R18B

- If a mounted horse reaches a hole/chasm or lava, the hazard-specific forced dismount occurs before horse damage callbacks, before horse relocation, and before rider fall/recovery begins.
- This ordering prevents ordinary buck or faint dismount presentation from competing with hazard recovery.
- Mounted hole/chasm still damages both horse and rider; both complete recovery already unmounted at separate legal safe positions.
- Mounted lava still damages only the horse; the rider retains the zero-damage recovery arc.

## Nonlethal jumping inside quicksand — V23R19

- Jumping and steering while still alive in quicksand never invokes generic ground-loss or combat-grounding recovery.
- Quicksand remains the movement/sink owner while active or while residual sink depth is recovering.
- Only the quicksand half-body failure path may request damage and safe respawn.
- Jump extraction and progressive horizontal slowdown remain active; ordinary jump movement cannot teleport the player outside the hazard.


## V23R19D authoritative player slowdown application

- `BDQuicksandStatus` pushes the current depth-derived slowdown directly into `BDPlayerController` every active/recovery frame.
- `BDPlayerController.EffectiveMoveSpeed` applies that value exactly once.
- The player movement path does not multiply the same quicksand value again after velocity is resolved.
- Leaving, disabling, or completing failure resets the multiplier to `1.0`.
- Visual sinking without measurable horizontal slowdown is a failed implementation.
