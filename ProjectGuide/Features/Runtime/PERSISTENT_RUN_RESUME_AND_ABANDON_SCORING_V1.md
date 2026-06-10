# Persistent Run Resume, Safe Return and Abandon Scoring V1

```text
Status: APPROVED / QUEUED AFTER FIRST-LAUNCH TUTORIAL VERIFICATION
Implementation claim: NONE
Owners: BDMainMenuFlow + future single run-snapshot owner + existing meta-progression owner
```

## Purpose

A live run may be left normally without being abandoned. The player can return to the Main Menu, later choose `CONTINUE`, or deliberately start a new run. `ABANDON RUN` remains a separate destructive end-of-run transaction.

## Main Menu contract

- No valid saved run: show `START NEW GAME` and do not show an empty or disabled Continue row.
- Valid saved run: show `CONTINUE` and `START NEW GAME`.
- `CONTINUE` restores the saved run without executing the New Game opening again.
- Selecting `START NEW GAME` while a valid run exists opens an in-handheld confirmation.
- Safe default: `CANCEL`.
- Destructive choice: `START NEW RUN`.
- The existing saved run is not erased before explicit confirmation.

## Pause actions

### Exit to Main Menu / Save & Return

- Produces one stable, atomic run snapshot.
- Returns to Main Menu without ending the run.
- Preserves Continue.
- Grants no end-of-run meta reward.
- Is never described or recorded as Abandon.

### Abandon Run

- Requires explicit confirmation.
- Ends the run and removes Continue for that run.
- Uses the same authoritative evaluator that calculates the meta points the player would receive if the player died at that exact moment.
- Computes the Abandon award as `death-equivalent points × 0.84` before the evaluator's existing rounding/clamp policy.
- Awards the result exactly once through an idempotent end-of-run transaction.
- Shows the shared meta-score result screen before any exit animation.
- Starts the agreed gameplay-to-handheld exit animation only after the player closes the result screen.
- Cleans the run snapshot only as part of the committed end transaction.

## Snapshot contract

The future snapshot stores approved stable truth only: schema version, Run ID, Seed, current room/region, legal restore point, player/horse stable state, health/ammo, cleared encounters and run progress required by real systems.

It does not persist active attack frames, projectiles, Hook/rope, Hit Stop, camera shake, popup state, transient VFX, coroutines or event subscriptions.

The precise restore-position policy remains an audit decision. It must be chosen from the actual room, checkpoint, encounter and safe-position owners rather than guessed.

## Transaction ordering for Abandon

1. Accept one confirmation and lock duplicate input.
2. Close or stabilize transient combat state.
3. Capture immutable performance inputs.
4. calculate death-equivalent meta points;
5. multiply by 0.84;
6. apply existing rounding/clamp policy;
7. award once using Run ID plus end-transaction identity;
8. mark the run Abandoned and retire Continue;
9. show the shared result screen;
10. wait for an explicit legal close;
11. play the agreed exit animation;
12. complete cleanup and return to Main Menu once.

A crash or reload between reward persistence and visual exit may never grant the award again.

## Verification

- Exit normally, Continue, close/relaunch the application, and Continue again.
- Start New Game with an existing run, cancel, and prove the save remains unchanged.
- Confirm Start New Game and prove a new Run ID/Seed replaces the old run once.
- Compare death-equivalent and Abandon awards at the same state; Abandon must resolve to exactly 84% under the shared rounding policy.
- Prove the result screen precedes the exit animation.
- Prove closing the result screen begins the animation once.
- Prove Abandon removes Continue while ordinary Exit preserves it.
- Test double input, save failure, corrupt snapshot, version migration and crash recovery.
