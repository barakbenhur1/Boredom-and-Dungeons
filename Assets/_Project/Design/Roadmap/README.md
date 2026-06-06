# Roadmap Documentation

Status date: **2026-06-06**

## Active source of truth

The single authoritative complete development plan, current status, requirement tracker, blockers, QA truth, and exact resume point is:

```text
/PROJECT_STATUS.md
```

The current public summary and documentation map are:

```text
/README.md
/DOCUMENTATION_INDEX.md
```

## Historical roadmap

`MASTER_REMAINING_WORK_ROADMAP_V128.md` is retained as historical planning context only.

It is **not** the current implementation order, current completion state, current QA result, or current next task. The categorized C00–C14 plan in `/PROJECT_STATUS.md` replaced it.

## Rules

- Update `/PROJECT_STATUS.md` in every material commit that changes code, requirements, QA truth, priority, blockers, or implementation order.
- Keep Previous / Current / Next in the snapshot at the top of `/PROJECT_STATUS.md`.
- Keep `/README.md` synchronized with that snapshot without duplicating the complete checklist.
- Use `/DOCUMENTATION_INDEX.md` to identify canonical, superseded, working, and historical documents.
- Dedicated design documents may contain deeper approved specifications, but they never replace the authoritative checklist.
- Stage-numbered QA documents are historical reports, not proof that the present branch still passes.
- Do not create `PROJECT_STATUS_CURRENT*.md`, `WORKING_NOW.md`, copied live roadmaps, or package-specific status snapshots.
- When an old document contradicts a newer approved rule, remove it or mark it clearly as superseded; never leave two documents appearing equally current.
