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
10. Do not commit or push. Deliver a validated ZIP and exact local commands; the user verifies and pushes.

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
- Mouse + D-pad; `A` confirm, `B` back, `X` Settings, `Y` Progression.
- Every Boy image requires a composition-matched Girl version.
- `Progression` is the user-facing single-line label.

## Safety

- No destructive reset, clean, broad checkout or silent deletion.
- Preserve Unity GUIDs and `.meta` files for actual Unity assets.
- Do not add runtime dependencies on `UnityEditor`.
- No expensive scene searches or allocations in per-frame paths.
- Gameplay remains authoritative; animation/UI present state and do not duplicate damage, cooldown or progression truth.
