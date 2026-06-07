# QA Checklist — Required Verification Gates

Actual pass/fail truth belongs in `PROJECT_STATUS.md`.

## Single required Unity QA command

```text
Boredom And Dungeons -> TEST EVERYTHING
```

New automated checks integrate into this command; do not create another mandatory QA action.

## Gate order

1. **Repository/preflight:** required files, stable `.meta`, no real conflict markers, no staged package/cache/duplicate-status artifacts.
2. **Package validation:** syntax, first install, second install for idempotency, validator, `git diff --check`.
3. **Unity compilation:** documented Unity version, no compiler errors, no unexplained compiler warnings, no project-generated red Console error.
4. **Automated QA:** TEST EVERYTHING, zero blockers; warnings fixed or explicitly accepted.
5. **Console cleanliness:** no repeated project-generated warning spam, edit-mode material leak, charged-shot warning, or AudioListener warning.
6. **Focused Play Mode:** changed behavior, repeated use, death/restart/re-entry, nearby-system regression checks.
7. **Performance when relevant:** measure CPU/GPU/memory/GC/draw calls/loading on a representative target.
8. **Documentation truth:** record real results and exact resume point in `PROJECT_STATUS.md`.

## Active V23R2 regression gate

1. **Single camera owner:** `BDCameraFollow` is the only normal-gameplay Main Camera transform owner; the old viewport-bias source is absent.
2. **Stable yaw/pitch:** wall proximity, enemies, damage, and room/tile handoff do not alter mouse sensitivity or gameplay pitch.
3. **Planar shake:** combat shake has no vertical component.
4. **Closed walls:** on foot and mounted, side/corner/diagonal rotation never reveals an adjacent room.
5. **Room/tile handoff:** cross several legal transitions in both directions. Distance/FOV/pitch remain stable with no snap or zoom.
6. **Combat grounding:** repeated enemy hits near walls and transitions do not move the player below the floor.
7. **Root-safe recovery:** any forced recovery places the CharacterController capsule fully above ground and movable.
8. **Walking-proof hole boundary:** approach every side and corner of the hole using ordinary walking at slow/full speed and diagonal angles; walking never enters.
9. **Swept footprint:** hazard filtering checks intermediate path samples and capsule footprint, not only final center position.
10. **Active intent only:** intentional gap entry is limited to active dodge, actively ascending jump, or explicit forced movement. Recent historical dodge/jump timestamps do not authorize later walking entry.
11. **Post-recovery suppression:** after a hole recovery, immediately walk toward the same hole from multiple angles; ordinary movement remains blocked.
12. **Near-hole respawn:** intentional fall recovery uses a valid local point just outside the same hole before older safe points or spawn.
13. **Respawn safety:** local recovery is outside all hazard volumes, above ground, free of overlap, and does not loop.
14. **Existing regressions:** charged shot, AudioListener, mounted intro, BBH first frame, current-status QA, and V22R2 Console cleanup remain passing.
15. **Console:** no project-generated red errors or repeated warnings.

## Repository-hygiene gate

- Run `python3 tools/check_repository_hygiene.py` before handoff and commit.
- Root Markdown matches `DOCUMENTATION_INDEX.md`.
- No obsolete roadmap/status copy, package README/manifest, repair narrative, chat export, or copied QA report remains tracked.
- Every removed document had valid requirements merged first.
- `PROJECT_STATUS.md` matches the real current state and resume point.

## Truthful status language

- Static validator passed does not mean Unity compiled.
- Unity compiled does not mean Play Mode passed.
- Automated PASS does not mean the Console is clean.
- One Play Mode pass does not prove restart/re-entry behavior.
- No profiler data means performance remains unverified.
- A task is not DONE until all applicable gates pass.
