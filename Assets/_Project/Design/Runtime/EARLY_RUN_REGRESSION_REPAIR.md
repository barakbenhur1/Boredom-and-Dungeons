# Early-Run Regression Repair

## New Game click leakage

The `New Game` UI uses the same left mouse button as player melee.
When the gameplay scene loads in the same input frame, both combat readers
can observe that UI click as a fresh attack request.

Startup settling may also mark the player as descending. The leaked click can
therefore create a landing strike or normal slash as the map appears.

## Required behavior

- combat-input quarantine minimum: `0.20s`;
- maximum quarantine: `1.50s`;
- after the minimum, quarantine ends when attack buttons are released;
- pending melee, ranged, charged, and buffered attack state is cleared;
- startup settling cannot become a landing attack;
- the first deliberate attack after release works normally.

The existing transient-feedback suppression remains separate.

## Verification

1. Die and reach the game-over/menu flow.
2. Click `New Game` with the left mouse button.
3. Do not press another control during loading.
4. Confirm there is no slash, landing strike, impact, hit-stop, shake,
   damage flash, or hit audio.
5. Release the mouse and perform one deliberate attack.
6. Repeat at least three times.
7. Run `Boredom And Dungeons -> TEST EVERYTHING`.
