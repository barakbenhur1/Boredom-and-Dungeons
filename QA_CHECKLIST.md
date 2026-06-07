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
5. **Console cleanliness:** no repeated project-generated warning spam, edit-mode material-instantiation leak, charged-shot warning, or AudioListener warning.
6. **Focused Play Mode:** changed behavior, repeated use, death/restart/re-entry, nearby-system regression checks.
7. **Performance when relevant:** measure CPU/GPU/memory/GC/draw calls/loading on a representative target.
8. **Documentation truth:** record real results and exact resume point in `PROJECT_STATUS.md`.

## Active V20/V21R1/V22/V22R2 regression gate

1. **Charged shot:** repeatedly fire charged x3 shots. No duplicate `TrailRenderer`, no `BDChargedProjectileVisual` null exception, and visuals remain present.
2. **Audio:** exactly one active `AudioListener` before, during, and after mounted entrance.
3. **Mounted cinematic:** approved entrance camera, full input lock, right turn, full stop, short hold, then camera/control return.
4. **BBH:** first rendered frame black; first B visibly animates from zero.
5. **Current-status QA:** no false blocker caused by an obsolete exact version heading.
6. **Structural walls:** after TEST EVERYTHING, every structural wall is at least 64 world units high, fully opaque, and has no active legacy structural `BDOccludingWall`.
7. **Closed-wall camera:** on foot and mounted, at sides/corners/diagonals, outward rotation cannot reveal or see over an adjacent room; inward/tangential rotation remains usable.
8. **Room/node handoff:** cross multiple legal doorways in both directions. Camera distance/FOV remains stable; there is no backward snap and no zoom-in.
9. **Minimap isolation:** minimap discovery/rotation does not move player or horse transforms.
10. **V22R2 compiler cleanup:** no CS0414 warning remains for `BDCameraFollow.minimumCameraDistance`.
11. **V22R2 edit-mode material safety:** `BDOccludingWall` uses `sharedMaterial` outside Play Mode and per-renderer `material` only in Play Mode.
12. **Repeated QA:** clear Console, run TEST EVERYTHING twice, and confirm the edit-mode material-instantiation warning does not return or accumulate.
13. **Console:** no project-generated red errors and no repaired-system warning spam.

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
