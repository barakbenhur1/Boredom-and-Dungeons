# Early-Run and Combat Recovery Regression Repair

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
- Intentional jump, dodge, and gap entry remain valid and do not trigger forced recovery.
- Unexpected loss of valid ground support during the guard recovers immediately without damage.
- Ground probes reject player, horse, enemy, `CharacterController`, hazard, structural-wall, and moving-body colliders.
- Recovery root height is calculated from `CharacterController.center`, `height`, and `skinWidth` so the capsule bottom is above the ground surface.
- Recovery clears player motion and cannot create a repeated recovery loop or embedded/stuck state.

## Verification

1. Die, reach the menu, click `New Game`, and confirm no leaked attack.
2. Release the mouse and perform one deliberate attack.
3. Repeat at least three times.
4. Fight several enemies near walls and room transitions while taking repeated hits.
5. Confirm the player never falls below the floor.
6. Force or observe a recovery and confirm the player returns fully above ground with movement restored.
7. Repeat while jumping and dodging; intentional airborne movement must remain valid.
8. Run `Boredom And Dungeons -> TEST EVERYTHING` and inspect the Console.
