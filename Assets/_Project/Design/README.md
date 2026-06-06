# Boredom & Dungeons — Design Documentation

Status date: **2026-06-06**

This folder contains detailed design specifications and historical QA records. It is not a second project-status system.

## Authority

```text
/PROJECT_STATUS.md
```

is the only complete authoritative source for current progress, approved requirements, QA truth, blockers, implementation order, and the exact resume point.

Use:

```text
/DOCUMENTATION_INDEX.md
```

to determine which design documents are canonical, working, superseded, or historical.

## Folder meanings

- `ArtDirection/` — asset organization and production-art direction.
- `Audio/` — audio design and future implementation notes.
- `Bosses/` — boss and mini-boss specifications.
- `Cinematics/` — ending and cinematic behavior.
- `Collectibles/` — inventory, secret collectible, and guardian rules.
- `Combat/` — player attack, dodge, parry, aiming, and combat rules.
- `Encounters/` — encounter-specific behavior and spawn presentation.
- `Enemies/` — regular-enemy design.
- `Horse/` — horse traversal, health, recovery, follow, and interaction behavior.
- `Level/` — camera, scene-builder, and earlier level-shape work.
- `Map/` — final map topology, routes, hazards, and navigation contracts.
- `Movement/` — current player/horse/enemy movement and facing behavior.
- `QA/` — historical stage reports and validation evidence.
- `Rendering/` — rendering and shadow policy.
- `Roadmap/` — roadmap ownership notice and the historical V128 roadmap.
- `UI/` — HUD, minimap, boot intro, menu, settings, and result flow.
- `VFX/` — visual-effect specifications.

## Versioned-document rule

A filename containing `V1`, `V2`, `V126`, or another version does not automatically make that file current.

- Later correction documents override earlier documents for the same exact behavior.
- `/PROJECT_STATUS.md` overrides every versioned design document when decisions conflict.
- Stage-numbered QA reports are historical snapshots.
- Working documents must never be treated as final approval merely because they are present in Git.

## Current clarification

- `UI/MAIN_MENU_SETTINGS_RESULT_FLOW_V2.md` is the current menu/result-flow specification.
- The contradictory V1 document was removed.
- `Roadmap/MASTER_REMAINING_WORK_ROADMAP_V128.md` is historical only.
- Mother Boss final decisions are governed by `/PROJECT_STATUS.md`, supported by the later decision documents in `Bosses/`.
- Natural movement and spinning AOE design documents describe committed code that still requires the current Unity verification gate.

## Maintenance

Whenever a material design decision changes:

1. update `/PROJECT_STATUS.md`;
2. update the focused design document when deeper detail is needed;
3. update `/README.md` only when the public/current overview changes;
4. update `/DOCUMENTATION_INDEX.md` if document ownership or canonical status changes;
5. remove a contradictory duplicate only after preserving every still-valid requirement.
