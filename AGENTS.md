# Boredom & Dungeons — AI/Codex Operating Contract

Read this file first, then follow [`ProjectGuide/README.md`](ProjectGuide/README.md).

## Non-negotiable behavior

1. Read current status, workflow, bugs, work queue and relevant specifications before editing.
2. Inspect the real repository/local state; do not assume chat or remote `main` is newer than the supplied working tree.
3. Record every material request, correction, priority change, bug, deferral and resume point in the maintained guide in the same package.
4. Preserve code, scenes, prefabs, assets, colors, behavior and requirements by default.
5. Extend the existing owner; do not create parallel gameplay, input, UI, audio, camera or state systems without documented need.
6. Ask before inventing missing product behavior or resolving a real contradiction by assumption.
7. Update relevant status, bug, task, feature, architecture, QA, technical-decision and performance files together.
8. Run a documentation relevance sweep: merge valid content before deleting obsolete/duplicate/temporary files.
9. Never claim Unity compile, TEST EVERYTHING, Play Mode, device performance or user acceptance unless actually run.
10. Never commit, push, create/update branches, or open/merge pull requests. Deliver a validated local patch ZIP with backup, rollback, and exact local commands; the user verifies and creates the commit.

## Required workflow

1. Read `ProjectGuide/README.md`.
2. Inspect repository, current Git state, affected code/scenes/prefabs/tests and nearby systems.
3. Classify the request: earlier/blocking, current, later, or clarification required.
4. Save the current resume point before an interruption.
5. Implement minimally and additively.
6. Update guide and QA in the same working tree.
7. Validate installer twice, validator, `git diff --check`, repository hygiene and source scans.
8. User runs Unity compile, TEST EVERYTHING and focused Play Mode.
9. Record real results and continue from the exact saved point.

## Current special requirements

- Current user priority after guide reorganization: real 3D upright handheld Main/Pause UI.
- No flat in-game device image: modeled shell, screen, glass, buttons and tactile interaction.
- Main mapping: `X` New Game, `A` Progression, `B` Settings, `Y` Credits; `B` is Back away from Main; black `SELECT` activates focus and black `EXIT` opens the legal in-screen confirmation.
- Every Boy image requires a composition-matched Girl version.
- `Progression` is the user-facing single-line label.

## Safety

- No destructive reset, clean, broad checkout or silent deletion.
- Preserve Unity GUIDs and `.meta` files for actual Unity assets.
- Do not add runtime dependencies on `UnityEditor`.
- No expensive scene searches or allocations in per-frame paths.
- Gameplay remains authoritative; animation/UI present state and do not duplicate damage, cooldown or progression truth.


<!-- B&D LOCAL PATCH + PRODUCTION CODE CONTRACT V1 START -->
## Permanent local-delivery and production-code contract

- The assistant performs no GitHub write and no local Git commit operation. No push, branch, PR, merge, reset, stash, clean, checkout or pull is part of delivery.
- Material work is delivered as a backup-aware local patch ZIP with preflight, idempotent apply, validator and rollback. The downloaded ZIP is removed only after every apply/validation step succeeds.
- Preserve local changes: if an authoritative source anchor is not the expected current shape, stop before overwriting and report the conflict.
- Every touched or materially encountered production area follows `ProjectGuide/Rules/PRODUCTION_CODE_STANDARD.md`.
- Implement durable behavior in its authoritative owner. Do not create or rebrand an avoidable workaround/compatibility layer as architecture.
<!-- B&D LOCAL PATCH + PRODUCTION CODE CONTRACT V1 END -->
