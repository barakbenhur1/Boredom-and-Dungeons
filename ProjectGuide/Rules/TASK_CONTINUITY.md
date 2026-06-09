# Task Continuity and Handoff Contract

## Purpose

This contract prevents material project work from being lost, misrepresented, or left dependent on a single chat session, local memory, package README, or one contributor.

It is mandatory for every material task and especially for work that is:

- large or multi-step;
- cross-system;
- likely to continue across several sessions;
- likely to move between ChatGPT, Codex, another developer, or another machine;
- blocked on Unity, Play Mode, device, performance, or user verification;
- capable of changing product behavior, architecture, data, scenes, assets, QA, documentation, or release truth.

The reason this contract exists is practical: conversations can end, context can be truncated, implementation may be handed to Codex, and a task may stop after only part of its work. The repository must therefore contain enough accurate context for another contributor to continue safely without guessing.

## Mandatory two-layer documentation

Every material task is recorded in both layers:

1. `ProjectGuide/Status/CURRENT.md`
   - owns the global current truth, ordering, blockers, verification state, and exact resume point;
   - contains a concise summary of why the task exists, its current stage, what changed, what was verified, what remains open, and the next action.

2. Relevant task and domain documents
   - a large or multi-session task receives an active task record under:
     `ProjectGuide/Tasks/`;
   - durable behavior is also synchronized into every relevant canonical design, architecture, QA, technical-decision, performance, save, input, camera, UI, audio, visual, economy, or other domain document.

A task record never replaces `ProjectGuide/Status/CURRENT.md`, and `ProjectGuide/Status/CURRENT.md` never replaces the detailed task/domain record.

## Required contents of a large-task record

Every active large-task record must contain:

1. task ID and title;
2. originating request and approval source;
3. the reason the task exists;
4. the problem or risk it is intended to solve;
5. user intent and expected outcome;
6. scope;
7. explicit non-goals and protected behavior;
8. relevant systems, scenes, prefabs, assets, data, documents, and QA owners;
9. dependencies and ordering;
10. work breakdown into concrete sub-tasks;
11. status for every sub-task;
12. decisions already approved;
13. open decisions that still require evidence or user approval;
14. implementation changes made;
15. files created, changed, or intentionally left unchanged;
16. migrations, compatibility constraints, and data-loss risks;
17. verification performed, with exact evidence;
18. verification not yet performed;
19. bugs found, their tracker IDs, and current status;
20. contradictions found between code, scenes, documentation, QA, or requirements;
21. blockers and remaining risks;
22. deferred work and why it was deferred;
23. exact resume point;
24. exact next action;
25. commit, branch, package, QA-output, screenshot, recording, or profiling references when available.

## Status vocabulary

Use explicit truthful states:

- `PLANNED`
- `AUDIT IN PROGRESS`
- `IMPLEMENTATION IN PROGRESS`
- `IMPLEMENTED / STATIC VERIFICATION ONLY`
- `IMPLEMENTED / UNITY VERIFICATION REQUIRED`
- `AUTOMATED PASS / PLAY MODE OPEN`
- `DEVICE OR PERFORMANCE VERIFICATION OPEN`
- `VERIFIED`
- `BLOCKED`
- `DEFERRED`
- `SUPERSEDED`

Do not use `DONE`, `FIXED`, or `VERIFIED` when required Unity, Play Mode, device, visual, camera-feel, input-feel, performance, or user acceptance evidence has not actually run.

## Mandatory update events

Update the task record, `ProjectGuide/Status/CURRENT.md`, and every relevant canonical document in the same change whenever:

- a task is accepted, re-scoped, split, reordered, or paused;
- a new requirement or correction is added;
- a design or technical decision is made;
- implementation begins;
- a file or system owner changes;
- a sub-task is completed;
- a bug is discovered, repaired, reopened, reclassified, or verified;
- compilation, automated QA, Play Mode, device, performance, visual, audio, or user verification passes or fails;
- a blocker appears or is removed;
- work is deferred or superseded;
- the exact resume point changes;
- a handoff to Codex, another chat, or another developer occurs.

Do not wait until the end of the task.

## Mandatory documentation relevance sweep before every commit

Before every commit, perform a documentation relevance sweep.

For every maintained document, decide one of:

- `KEEP CURRENT` — it still has a distinct active or durable responsibility;
- `MERGE THEN REMOVE` — it contains valid information, but another canonical document owns that responsibility;
- `REMOVE AS OBSOLETE` — its work is completed, superseded, duplicated, temporary, package-specific, or no longer relevant;
- `BLOCK COMMIT` — its relevance or ownership is unclear and requires resolution.

Rules:

1. Never keep a document merely because it existed in an earlier package.
2. Never keep duplicate roadmaps, status snapshots, repair narratives, chat exports, package READMEs, copied prompts, or completed task notes as live maintained truth.
3. Before removing a document, merge every still-valid requirement, decision, verification result, limitation, and resume-relevant fact into the correct canonical owner.
4. Update `ProjectGuide/INDEX.md` in the same commit whenever a maintained document is added, removed, renamed, superseded, or changes responsibility.
5. Update references from other documents before deletion.
6. Preserve Git history as the archive; the working tree contains only current truth.
7. Completed task records are removed when they no longer provide distinct ongoing operational value, after their durable content is merged.
8. Temporary package artifacts, local QA exports, generated reports, caches, copied status files and stale helper scripts must not be committed.
9. The commit is blocked if obsolete maintained documentation remains or if a deletion would lose still-valid project truth.

## Handoff rule

Before handing work to Codex, another chat, or another contributor, the repository must answer without chat context:

- Why are we doing this?
- What exactly was approved?
- What must not change?
- What has already been done?
- Which files and systems are involved?
- What evidence exists?
- What remains unverified?
- What failed?
- What is blocked?
- What is the exact next step?
- Where should the contributor resume?

If any answer exists only in chat, the handoff is incomplete.

## Completion and document lifecycle

When a task is fully verified:

1. synchronize durable behavior into the correct canonical domain documents;
2. update `ProjectGuide/Status/CURRENT.md` with the verified result and next resume point;
3. update or clear related entries in `OPEN_BUG_TRACKER.md`;
4. update `ProjectGuide/INDEX.md` when maintained-document ownership changes;
5. keep the task record only when it retains distinct long-term operational value;
6. otherwise merge its still-valid content into canonical owners and remove it in the same change;
7. rely on Git history for the completed task's historical snapshot.

## Current required task record

The active architecture/gameplay/camera audit is maintained at:

`ProjectGuide/Tasks/PAUSED/ARCHITECTURE_AUDIT.md`

## Mandatory coverage of bugs, unconfirmed work, and verification levels

Every material update must record not only the requested task but also:

- every currently known open bug;
- every reopened bug;
- every implementation that has not yet been accepted by the user;
- every automated check still awaiting Play Mode;
- every Play Mode result still awaiting user confirmation;
- every device, responsive, performance, visual, audio, camera-feel or input-feel gate;
- every contradiction or risk discovered during implementation;
- every future requirement affected by the task;
- the exact ordered next step.

The repository must distinguish:

1. installer/static validation;
2. compilation;
3. TEST EVERYTHING;
4. focused Play Mode;
5. target-device/performance verification;
6. user acceptance.

An earlier stage never implies a later stage.

A task may not disappear from current documentation merely because code exists. It remains visible as `implemented but not yet user-confirmed` until its required acceptance level is reached.
