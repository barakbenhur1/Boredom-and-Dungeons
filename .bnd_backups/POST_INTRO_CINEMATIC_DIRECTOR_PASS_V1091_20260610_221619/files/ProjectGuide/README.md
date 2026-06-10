<!-- BND_FIRST_LAUNCH_TUTORIAL_V1081_HOTFIX:BEGIN -->
## Current V10.8.1 hotfix truth

The active blocker is the mounted shooting lesson failing to advance after a real hit. The repair makes progression occur at projectile impact against the living lesson target. The post-BBH landing is camera-only inside the persistent full-screen table scene, and package tools must use the maintained semantic terminal-color standard. The retro tutorial redesign and articulated new-enemy/difficulty work are captured as queued tasks after this gate.
<!-- BND_FIRST_LAUNCH_TUTORIAL_V1081_HOTFIX:END -->

<!-- BND_FIRST_LAUNCH_TUTORIAL_MECHANICS_REPAIR_V108:BEGIN -->
## Current active verification slice — 2026-06-10

The immediate active item is the V10.8 first-launch tutorial mechanics/readability repair. It synchronizes projectile and Hook effects to visible completion, adds production-faithful Charged Shot teaching, local checkpoints, actor blocking, moving leg frames, a readable/fair final boss and a full-screen real-3D post-BBH handoff. It is implemented locally but remains unverified until Unity compilation, TEST EVERYTHING and focused Play Mode pass. The broader tutorial/main-game animation production work remains queued after this gate.
<!-- BND_FIRST_LAUNCH_TUTORIAL_MECHANICS_REPAIR_V108:END -->

# Boredom & Dungeons — Project Guide

This folder is the complete maintained knowledge base for the project. It is deliberately outside `Assets/` so Unity does not import planning, QA and reference material as game assets.

## Mandatory read order before any change

1. [`Status/CURRENT.md`](Status/CURRENT.md) — current truth, active priority, blockers and exact resume point.
2. [`Rules/WORKFLOW.md`](Rules/WORKFLOW.md) — how changes are inspected, implemented, packaged and verified.
3. [`Status/BUGS.md`](Status/BUGS.md) — every open, reopened or implemented-but-unverified defect.
4. [`Status/WORK_QUEUE.md`](Status/WORK_QUEUE.md) — ordered work after the active item.
5. The relevant product, engineering, QA and feature specifications linked from [`INDEX.md`](INDEX.md).

For AI/Codex work, root [`../AGENTS.md`](../AGENTS.md) must be read first.

## Current project truth

- Current user-prioritized work: organize and validate this Project Guide, then implement the real 3D upright handheld Main/Pause UI.
- The handheld is a real 3D object, not a flat image: modeled shell, recessed display, clear glass/plastic, tactile buttons and physical press feedback.
- Input contract: mouse, D-pad, arrows and WASD navigate; Main Menu `X` New Game, `A` Progression, `B` Settings, `Y` Credits; `B` Back elsewhere; center `SELECT` activates focus and center `EXIT` opens confirmation.
- User-facing label: `Progression`, always on one line where supported.
- Only the New Game / New Run preview uses protagonist art: active Boy shows the Boy image and active Girl shows the Girl image. Progression, Settings, Credits, Quit, Resume/Pause and confirmation artwork must remain character-neutral and use one shared asset each.
- Existing runtime/QA/target-outline bugs remain open until their own Unity and user-verification gates pass.

## Source-of-truth map

| Need | Read |
|---|---|
| Current state and exact next action | [`Status/CURRENT.md`](Status/CURRENT.md) |
| Ordered execution queue | [`Status/WORK_QUEUE.md`](Status/WORK_QUEUE.md) |
| Bugs and disputed behavior | [`Status/BUGS.md`](Status/BUGS.md) |
| Delivery, ZIP and Git rules | [`Rules/WORKFLOW.md`](Rules/WORKFLOW.md) |
| Documentation lifecycle | [`Rules/DOCUMENT_MAINTENANCE.md`](Rules/DOCUMENT_MAINTENANCE.md) |
| Architecture and ownership | [`Engineering/ARCHITECTURE.md`](Engineering/ARCHITECTURE.md) |
| Technical decisions | [`Engineering/TECHNICAL_DECISIONS.md`](Engineering/TECHNICAL_DECISIONS.md) |
| Performance policy | [`Engineering/PERFORMANCE.md`](Engineering/PERFORMANCE.md) |
| QA gates | [`QA/QA_CHECKLIST.md`](QA/QA_CHECKLIST.md) |
| Product and visual language | [`Product/`](Product/) |
| Detailed feature contracts | [`Features/`](Features/) |
| Future plans and deferred work | [`Plans/`](Plans/) |
| Current/paused/queued task records | [`Tasks/`](Tasks/) |
| Asset production rules | [`Production/`](Production/) |

## Non-negotiable rules

- Preserve existing work; changes are additive unless removal is explicitly approved or a document is proven obsolete after its valid content is merged.
- Never claim Unity compile, TEST EVERYTHING, Play Mode, device performance or user acceptance unless it actually happened.
- The assistant does not commit or push. It returns a tested ZIP and exact commands; the user verifies in Unity and pushes.
- Every material request, correction, bug, status change, deferral and resume-point change is written into the maintained guide in the same package.
- Git history is the archive. Maintained files contain current truth, not repeated package histories.

## Current handheld control summary — 2026-06-09

The presenter owns one semantic action per physical control: center SELECT activates focus; center EXIT opens the legal quit/abandon confirmation; Main Menu X starts New Game, A opens Progression, B opens Settings and Y opens Credits; B returns on every non-main page. WASD/arrows remain navigation-only, so keyboard A is never overloaded as the face-button A shortcut. Page UI, button pulse and state transition must derive from the same semantic action to prevent double execution or mismatched labels.
