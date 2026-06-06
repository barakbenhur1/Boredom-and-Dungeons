# Boredom & Dungeons — Working Now

This file is updated whenever the active development item changes.

Every progress report to the user must include:

1. **Previous** — the last completed or paused item and its category.
2. **Current** — the exact item currently being worked on and its category.
3. **Next** — the next planned item and its category.

If work is temporarily interrupted by an earlier requirement, this file must preserve the saved resume point.

## Previous

```text
Category: C00 — Governance, documentation, and requirement recovery
Item: Record the permanent previous/current/next reporting rule
Status: DONE
Result: The communication and tracking rule is now persistent in Git.
```

## Current

```text
Category: C01 — Project stability, QA, validation, and repository health
Item: C01.10–C01.16 — Verify the latest main baseline
Status: IN PROGRESS / UNITY VERIFICATION REQUIRED
```

Current findings:

- The latest gameplay-code commit remains `11abb7f1b1996a240dc3d407af5acb59b28cf229` (`Remove obsolete camera and minimap fields`).
- All later commits currently inspected are design/status documentation changes.
- The latest code change removed these obsolete fields:
  - `rotationSpeedDegreesPerSecond`
  - `snapToMovementCardinals`
  - `mapRotationInitialized`
  - `minimumMovementDirectionMagnitude`
  - `rotateOnlyWhenActuallyMoving`
- The commit explicitly requires returning to Unity, waiting for compilation, and running `Boredom And Dungeons -> TEST EVERYTHING`.
- The project already contains a full stability gate and a manual Play Mode smoke checklist.
- GitHub currently reports no automated CI status checks for the repository, so Unity compilation and Play Mode cannot be truthfully marked PASS from GitHub alone.

Remaining C01 gate:

- open latest `main` in Unity `6000.0.76f1`;
- wait for compilation;
- confirm zero compiler errors and that the removed fields leave no dead serialized/reference errors;
- run `Boredom And Dungeons -> TEST EVERYTHING`;
- rebuild the authoritative prototype scene if requested by the tool;
- complete the Play Mode smoke checklist;
- save the PASS reports;
- update the status tracker with the exact result.

## Next

```text
Category: C03 — Player movement, combat, damage, weapons, and hazards
Item: C03.46 — Implement the validated last-safe-position tracker
Status: QUEUED AFTER C01
```

C03 will then continue in order with:

- hole/chasm damage: 15 health;
- safe respawn after falling;
- lava contact damage: 10 health;
- recovery to a valid non-lava point;
- repeated-damage protection;
- mounted player/horse paired recovery;
- edge-case and Play Mode QA.

## Saved later resume point

```text
Category: C07 — Boss framework, deterministic role planning, and encounter contracts
Item: C07.16 — Wire the framework into one real playable test encounter
```

Before returning to C07, the queued C03 and C04 earlier requirements must be implemented and verified.
