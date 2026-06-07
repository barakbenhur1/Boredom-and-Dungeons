# Early-Run, Combat Recovery, and Hole Boundary Regression Repair

## New Game click leakage

The `New Game` UI uses the same left mouse button as player melee. When the gameplay scene loads in the same input frame, combat readers can observe that UI click as a fresh attack request. Startup settling may also mark the player as descending, producing an unintended landing strike or slash.

### Required behavior

- combat-input quarantine minimum: `0.20s`;
- maximum quarantine: `1.50s`;
- after the minimum, quarantine ends when attack buttons are released;
- pending melee, ranged, charged, and buffered attack state is cleared;
- startup settling cannot become a landing attack;
- the first deliberate attack after release works normally.

The existing transient-feedback suppression remains separate.

## Combat floor-loss protection

Rare enemy-contact sequences must not push the player below the floor or recover the player inside floor geometry.

- A successful player hit starts a short combat-grounding guard and freezes safe-point updates.
- Intentional jump, dodge, and explicit forced gap entry remain valid and do not trigger accidental-ground recovery.
- Unexpected loss of valid ground support during the guard recovers immediately without damage.
- Ground probes reject player, horse, enemy, `CharacterController`, hazard, structural-wall, and moving-body colliders.
- Recovery root height is calculated from `CharacterController.center`, `height`, and `skinWidth` so the capsule bottom is above the ground surface.
- Recovery clears player motion and cannot create a repeated recovery loop or embedded/stuck state.

## Walking-proof hole boundary

Ordinary walking may never enter a hole/chasm, including diagonal or corner approaches.

- Motion filtering checks the swept CharacterController footprint across the entire requested horizontal move.
- Intermediate samples include center and leading/side footprint points so a diagonal frame cannot tunnel through the trigger edge.
- When a movement vector would enter a hole, preserve only safe axis movement so the player can slide along the edge without entering it.
- A hole fall is intentional only while a dodge is actively moving, a jump is actively ascending over the boundary, or an explicit forced-gap window is active.
- Historical dodge/jump grace tails do not authorize later walking entry.
- External recovery clears all gap-entry state and suppresses immediate re-entry.

## Near-hole recovery

An intentional hole fall returns the player to a close safe point beside the same hole.

- Capture a local recovery anchor when the hole fall begins.
- Resolve the nearest grounded point outside the hole bounds using the entry side first, then nearby perimeter samples.
- Use a dedicated small hole-edge clearance that prevents immediate overlap without sending the player far away.
- Prefer the local anchor before the latest/previous safe point or initial spawn.
- Apply CharacterController-root-safe height and overlap checks.
- The player must regain movement immediately and ordinary walking toward the hole remains blocked.

## Verification

1. Die, reach the menu, click `New Game`, and confirm no leaked attack.
2. Release the mouse and perform one deliberate attack.
3. Fight several enemies near walls and room transitions while taking repeated hits; confirm no floor fall-through.
4. Walk toward every side and corner of the hole at slow and full speed; ordinary walking never enters.
5. Dodge and jump into the hole; intentional falls still occur.
6. Confirm the respawn is near the same hole, fully above ground, and immediately movable.
7. Immediately walk toward the same hole after recovery from several angles; walking remains blocked.
8. Repeat falls and recoveries to confirm there is no loop or distant spawn fallback.
9. Run `Boredom And Dungeons -> TEST EVERYTHING` and inspect the Console.
