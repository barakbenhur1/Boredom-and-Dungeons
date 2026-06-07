# Technical Decisions — Durable Project Choices

This file records long-lived choices and rationale. Current implementation status and open work remain in `PROJECT_STATUS.md`.

## Active decisions

### TD-001 — One authoritative product-status source

- `PROJECT_STATUS.md` is the only live source for requirements, priorities, current/next work, blockers, QA truth, and resume point.
- Git history stores previous states.
- No competing `WORKING_NOW`, `LATEST_STATUS`, or versioned status documents.

### TD-002 — One mandatory QA entry point

- All automated project checks integrate into `Boredom And Dungeons -> TEST EVERYTHING`.
- Focused QA classes remain modular but are orchestrated by the existing window.

### TD-003 — Additive, non-destructive changes

- Preserve existing functionality and assets by default.
- Partial-package failures are repaired on top of the real local state.
- Broad reset/clean/checkout is not an accepted repair strategy without explicit user approval.

### TD-004 — Existing system owner before new system

- Extend the established owner when possible.
- Do not introduce parallel menu, pause, run-flow, camera, minimap, mount, health, or QA controllers for the same responsibility.

### TD-005 — Runtime/editor separation

- Runtime assemblies do not depend on `UnityEditor`.
- Scene generation, validation, and repair tools remain editor-only.

### TD-006 — Structural patching over fragile text matching

- Installers prefer method/block/token-aware edits.
- Whitespace-sensitive replacement is acceptable only when the preflight uniquely proves the expected state.
- Installers must be idempotent and support partial previous application.

### TD-007 — Documentation changes are part of implementation

- Every user request is captured in the correct `PROJECT_STATUS.md` location before or with implementation.
- Architecture, design, QA, technical decisions, and performance documents are updated when their truth changes.
- Chat-only decisions are not accepted project state.

<!-- B&D QA CONTRACT DRIFT DECISIONS V9 START -->
### TD-008 — Conflict-marker scans are line-aware

- Only standalone Git conflict-marker lines are blockers.
- Inline examples in maintained documentation and quoted strings in validator
  source are allowed and must not be treated as unresolved conflicts.
- External package validation and Unity QA use the same interpretation.

### TD-009 — QA contracts follow the active implementation

- Regression checks validate current behavior and stable ownership contracts.
- QA must not require obsolete local-variable names or superseded version labels
  after a structural implementation changes.
- The V7 minimap contract is `BD MINIMAP RIGID CLIP MASK V7` plus the rigid
  `GUI.matrix` rotation method.
- The current START GAME highlight contract is the action-aware
  `DrawActionButton` path with `MenuActionVisual.Progress` and
  `StartGameHighlightTint`; the removed `startGamePressed` local variable is not
  a product requirement.
<!-- B&D QA CONTRACT DRIFT DECISIONS V9 END -->

## Decision lifecycle

A decision may be:

- `ACTIVE`
- `SUPERSEDED` with a link/name of the replacing decision
- `REJECTED` with rationale
- `RECOVERY REQUIRED` when evidence is incomplete

Do not silently delete superseded decisions.

## Template for future decisions

```text
### TD-XXX — Title
Status: ACTIVE | SUPERSEDED | REJECTED | RECOVERY REQUIRED
Date: YYYY-MM-DD
Context:
Decision:
Consequences:
Alternatives considered:
Affected code/docs/QA:
```
