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

<!-- B&D LINE-AWARE CONFLICT SCAN V9 START -->
## Conflict-marker detection precision

The automated conflict scan blocks only real standalone Git marker lines after
leading whitespace is removed:

- a line beginning with the seven opening angle brackets;
- the seven-equals separator line;
- a line beginning with the seven closing angle brackets.

Inline examples inside prose, Markdown code spans, comments, or quoted source
strings are documentation and must not produce a false blocker. The external
package validator and `BDDocumentationGovernanceQA` must use the same line-aware
rule.
<!-- B&D LINE-AWARE CONFLICT SCAN V9 END -->

## Truthful status language

Use precise statements:

- `Static validator passed` does not mean Unity compiled.
- `Unity compiled` does not mean Play Mode passed.
- `Play Mode passed once` does not prove restart/re-entry behavior.
- `No profiler data collected` means performance remains unverified.
- A task is not `DONE` until all applicable gates and documentation requirements pass.

## Conflict-marker blocker

Any unresolved Git conflict marker in C#, scenes, prefabs, configuration, or maintained Markdown is an immediate blocker. Repair only the affected file or block while preserving unrelated local changes; do not discard the working tree.
