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
- Broad reset, clean, or checkout is not an accepted repair strategy without explicit user approval.

### TD-004 — Existing system owner before new system

- Extend the established owner when possible.
- Do not introduce parallel menu, pause, run-flow, camera, minimap, mount, health, or QA controllers for the same responsibility.

### TD-005 — Runtime/editor separation

- Runtime assemblies do not depend on `UnityEditor`.
- Scene generation, validation, and repair tools remain editor-only.

### TD-006 — Structural patching over fragile text matching

- Installers prefer method, block, or token-aware edits.
- Whitespace-sensitive replacement is acceptable only when preflight uniquely proves the expected state.
- Installers must be idempotent and support partial previous application.

### TD-007 — Documentation changes are part of implementation

- Every user request is captured in the correct `PROJECT_STATUS.md` location before or with implementation.
- Architecture, design, QA, technical decisions, and performance documents are updated when their truth changes.
- Chat-only decisions are not accepted project state.

### TD-008 — Conflict-marker scans are line-aware

- Only standalone Git conflict-marker lines are blockers.
- Inline examples in maintained documentation and quoted strings in validator source are allowed.
- External package validation and Unity QA use the same interpretation.

### TD-009 — QA contracts follow the active implementation

- Regression checks validate current behavior and stable ownership contracts.
- QA must not require obsolete local-variable names or superseded package labels after a structural implementation changes.

### TD-010 — Current-only maintained documentation

- Maintained Git documents describe current truth, not a chronological accumulation of package repairs.
- Git history stores historical versions.
- When a document is superseded, merge valid requirements into its authoritative owner, update `DOCUMENTATION_INDEX.md`, and remove the obsolete file in the same change.
- Root Markdown is restricted to the canonical allowlist. Feature contracts live under `Assets/_Project/Design/`.

### TD-011 — Cinematic camera and input ownership

- `BDRunPresentationCoordinator` may temporarily own camera transform and input lock during the mounted entrance.
- `BDCameraFollow` is restored only after the horse completes the approved right turn and full stop.
- The Main Camera GameObject and its sole `AudioListener` remain active.
- All gameplay input readers, including mouse-facing state, respect the central presentation lock.

### TD-012 — Semantic documentation QA

- Stable IDs and implementation anchors remain strict.
- Human documentation wording is validated semantically when equivalent phrases describe the same approved behavior.
- Semantic wording tolerance must not weaken Runtime, scene, ownership, or Play Mode verification.

### TD-013 — One normal-gameplay camera transform owner

- `BDCameraFollow` owns every normal-gameplay Main Camera position and rotation write.
- Composition, room containment, smoothing, collision, and shake are resolved inside that owner before one final transform assignment.
- A second post-follow offset component is not allowed because it can bypass wall containment and produce transition or combat camera drift.
- Mouse yaw uses one rate-limited stage. Wall proximity does not change sensitivity, and combat shake does not change vertical position or pitch.

### TD-014 — CharacterController-root-safe recovery

- Ground recovery computes root height from `CharacterController.center`, `height`, and `skinWidth`; a fixed world-space offset is insufficient.
- Safe-ground probes reject dynamic actors, hazards, structural walls, and moving bodies.
- Successful player damage starts a short grounding guard that freezes safe-point updates and recovers only unexpected floor loss, while intentional jump/dodge/gap motion remains valid.

## Decision lifecycle

A decision may be `ACTIVE`, `SUPERSEDED`, `REJECTED`, or `RECOVERY REQUIRED`. Do not silently delete superseded decisions.

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
