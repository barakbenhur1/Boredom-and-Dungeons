# Project Guide Maintenance Rules

## Purpose

Keep the repository understandable to a new developer or AI session without relying on chat history.

## Update in the same change

Update the guide whenever work changes requirements, priority, implementation state, QA truth, architecture, performance policy, bugs, blockers, deferrals or the exact resume point.

## File lifecycle

Every maintained file is classified before delivery:

- **Keep current** — still owns distinct active or durable truth.
- **Merge then remove** — still-valid content is moved to the correct owner, then the obsolete file is deleted.
- **Remove** — duplicate, superseded, temporary or historical-only material already preserved in Git history.
- **Block delivery** — ownership is unclear or information would be lost.

Do not create parallel status files, package diaries or copied roadmaps. Use:

- `Status/CURRENT.md` for global truth;
- `Status/BUGS.md` for defects;
- `Status/WORK_QUEUE.md` for execution order;
- `Tasks/` for large active/paused/queued work;
- `Plans/` for future and deferred systems;
- `Features/` for durable behavior contracts.

## Required review before code

1. Read `ProjectGuide/README.md` and the mandatory read order.
2. Inspect current code, scenes, prefabs, tests and local Git state.
3. Record the request and classification.
4. Resolve contradictions or ask before inventing missing behavior.
5. Only then edit code.

## Character-image parity

Every image that depicts the Boy must have a matching Girl version with the same dimensions, crop, camera, background, lighting, horse pose when present, grading, safe areas and import settings. The pair is one deliverable; either missing version blocks acceptance.
