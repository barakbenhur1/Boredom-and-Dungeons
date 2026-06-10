# Repository-Specific Rules

This file preserves the project-specific rules that apply to every Codex role. Role profiles define ownership; they do not replace this contract.

## Source of truth

- Start with `ProjectGuide/README.md`.
- For material game or product work, read in order:
  1. `ProjectGuide/Status/CURRENT.md`
  2. `ProjectGuide/Rules/WORKFLOW.md`
  3. `ProjectGuide/Status/BUGS.md`
  4. `ProjectGuide/Status/WORK_QUEUE.md`
  5. only the relevant specifications linked from `ProjectGuide/INDEX.md`
- `ProjectGuide/Status/CURRENT.md` owns current product status, priority, blockers, QA truth, and the exact resume point. Do not create a parallel status source.
- Record material product requests, corrections, bugs, deferrals, verification results, and resume-point changes in the authoritative ProjectGuide documents in the same change.
- Update `ProjectGuide/INDEX.md` when maintained document ownership or paths change. Update architecture, technical decisions, performance, and QA documents only when their owned truth changes.

## Project structure and ownership

- Runtime code is under `Assets/_Project/Scripts/Runtime`; it must not depend on `UnityEditor`.
- Editor installers/builders are under `Assets/_Project/Scripts/Editor`.
- Extend the existing authoritative owner. Do not create parallel gameplay, input, UI, audio, camera, save, damage, or state systems without a documented need.
- Preserve approved behavior, scenes, prefabs, assets, colors, serialized fields, references, Unity GUIDs, and `.meta` files by default.
- Do not manually edit generated/cache/build trees such as `Library`, `Temp`, `Logs`, `obj`, `Build`, or `Builds`.
- Avoid broad scene searches, repeated lookups, LINQ, material creation, and allocations in per-frame paths.

## Build and QA

- The documented Unity version is `6000.0.76f1`.
- The single required Unity QA entry point is:

```text
Boredom And Dungeons -> TEST EVERYTHING
```

- Use focused Edit Mode/Play Mode checks described by the relevant QA contract after the automated gate.
- Never claim Unity compilation, `TEST EVERYTHING`, Play Mode, device performance, rendered review, or user acceptance unless it actually ran.
- For repository/tooling-only changes, run the relevant validator, `python3 tools/check_repository_hygiene.py`, `git diff --check`, and inspect `git status --short`. Do not launch Unity when no Unity/game files changed.

## Git and delivery

- Preserve unrelated local work. No destructive reset, clean, broad checkout, stash, pull, history rewrite, or force-push without explicit user authorization.
- Normal material game delivery follows `ProjectGuide/Rules/WORKFLOW.md`: backup-aware patch packaging, validation, rollback, then user-run Unity verification and Git operations.
- A user's explicit request may authorize Codex to make a narrowly scoped commit and push after all requested checks pass. Stage only the requested system and documentation files, inspect the staged diff, and push only the current branch.
- Before every commit or handoff, remove prohibited temporary/package artifacts, run `python3 tools/check_repository_hygiene.py`, and inspect the final diff for generated files, secrets, unrelated changes, and scope creep.
- Repository tooling follows `ProjectGuide/Rules/TERMINAL_OUTPUT_STANDARD.md`.
