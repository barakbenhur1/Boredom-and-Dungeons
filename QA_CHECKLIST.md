# QA Checklist — Required Verification Gates

This document defines the stable verification layers. Actual pass/fail truth and current blockers belong in `PROJECT_STATUS.md`.

## Single required Unity QA command

```text
Boredom And Dungeons -> TEST EVERYTHING
```

New automated checks must integrate into this command. Do not create another mandatory QA menu action.

## Gate order

1. **Repository/preflight**
   - required files exist;
   - no unresolved `<<<<<<<`, `=======`, or `>>>>>>>` conflict markers in maintained source/documentation;
   - Unity `.meta` files are present and stable;
   - no package tools, caches, build output, or duplicate status files are staged.
2. **Package validation**
   - installer syntax check;
   - first installation run;
   - second installation run for idempotency;
   - validator run;
   - `git diff --check`.
3. **Unity compilation**
   - documented Unity version;
   - no compiler errors;
   - no release-blocking Console errors.
4. **Automated project QA**
   - run `TEST EVERYTHING`;
   - blockers must be resolved;
   - warnings must be explained and accepted or fixed.
5. **Focused Play Mode**
   - test the changed feature;
   - repeat reset/death/reload/re-entry paths;
   - test nearby systems and edge cases;
   - verify no duplicate UI, state owner, installer, or runtime controller was introduced.
6. **Performance verification when relevant**
   - capture measurements rather than assumptions;
   - compare before/after on the documented target or representative device;
   - inspect CPU, GPU, memory, GC, draw calls, loading, and spikes relevant to the change.
7. **Documentation truth**
   - update `PROJECT_STATUS.md` with the real result;
   - update relevant design, architecture, decisions, QA, and performance documents.

## Conflict-marker detection precision

The automated conflict scan blocks only real standalone Git marker lines after leading whitespace is removed. Inline examples inside prose, Markdown code spans, comments, or quoted source strings must not produce a false blocker.

## Truthful status language

- `Static validator passed` does not mean Unity compiled.
- `Unity compiled` does not mean Play Mode passed.
- `Play Mode passed once` does not prove restart/re-entry behavior.
- `No profiler data collected` means performance remains unverified.
- A task is not `DONE` until all applicable gates and documentation requirements pass.

## Active V20 focused regression gate

Automated PASS does not close this work while Play Mode or Console still shows a project error. Verify:

- first BBH frame contains no pre-visible first `B`;
- mounted-entry camera is inside the room, farther and higher, looking at the entrance;
- mouse aim and every gameplay action remain locked through the horse right turn, full stop, and short hold;
- normal camera ownership returns only after the stop;
- rotating the camera beside every closed wall, including diagonal and mounted angles, never reveals the adjacent room;
- repeated charged-shot x3 use does not add duplicate `TrailRenderer` components and does not throw `BDChargedProjectileVisual` exceptions;
- exactly one `AudioListener` remains active before, during, and after the cinematic;
- the Console contains no project-generated red error before acceptance.

## Repository-hygiene gate

Before every handoff and commit:

- root Markdown matches the canonical allowlist in `DOCUMENTATION_INDEX.md`;
- no obsolete `NEXT_STEPS.md`, duplicate roadmap/status, package README/manifest, repair narrative, chat export, or copied QA report remains tracked;
- every removed document had its still-valid contract merged first;
- the current snapshot, QA truth, and exact resume point in `PROJECT_STATUS.md` match reality.
