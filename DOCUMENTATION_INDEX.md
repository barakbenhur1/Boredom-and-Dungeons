# Boredom & Dungeons — Documentation Index

Status date: **2026-06-06**

This file explains which documents are current, which are detailed specifications, and which are historical records. It exists to prevent old versioned documents from being mistaken for the current project status.

## Source-of-truth order

1. **`/PROJECT_STATUS.md`** — the only complete authoritative development plan, requirement tracker, QA truth, blockers, and exact resume point.
2. **`/README.md`** — current public project overview and onboarding summary. It must stay synchronized with the current snapshot at the top of `PROJECT_STATUS.md`.
3. **`/DOCUMENTATION_INDEX.md`** — documentation ownership and status map. It does not replace `PROJECT_STATUS.md`.
4. **`/Assets/_Project/Design/**`** — detailed approved specifications for individual systems, encounters, UI, bosses, movement, rendering, map design, and cinematics.
5. **`/Assets/_Project/Design/QA/**`** — historical implementation/verification reports. They describe what was checked at a specific stage; they are not the current project status.
6. **Git history** — superseded implementation detail and removed duplicate documentation.

When documents conflict, the earlier item in this list wins.

## Current development snapshot

- Engine: Unity `6000.0.76f1`.
- Current code baseline on `main`: natural movement polish, faster/wider-turn horse movement, improved enemy awareness, temporary facing markers, START GAME emphasis, and long-press spinning AOE light attack.
- Current feature category: **C06 — player melee combat expansion**.
- Current feature state: implemented in code and committed, but still requires clean Unity compilation, `Boredom And Dungeons -> TEST EVERYTHING`, and Play Mode verification before it can be marked DONE.
- Current immediate follow-up requirements:
  - replace the horse's one-second hazard stop with a short two-step retreat away from the hazard, then resume normal behavior;
  - make minimap 90-degree orientation transitions slower, smoother, and more polished while keeping the map inside its frame;
  - do not add a separate QA command; all automated contracts remain under `TEST EVERYTHING`.

## Enemy-layout clarification

The current prototype scene generator uses randomness while building the Unity scene, but a normal run reloads the already-generated scene. Therefore enemy locations and room order can feel identical between runs until the scene is regenerated.

Some enemy-type selection is also intentionally depth/cell based, so the progression pattern remains partly predictable even during scene generation.

This is not the final run-randomization architecture. The authoritative plan already schedules:

- deterministic run-seed ownership and storage in C02;
- full map/route generation and multi-seed validation in C10;
- random mini-boss role/placement and run lifecycle verification in C14.

Because that work is already planned as part of the final map/run architecture, the current follow-up does **not** add a temporary competing enemy-randomization system.

## Current canonical design documents

- Combat: `Assets/_Project/Design/Combat/SPINNING_AOE_LONG_PRESS_LIGHT_ATTACK_V1.md`
- Movement and awareness: `Assets/_Project/Design/Movement/NATURAL_MOVEMENT_AWARENESS_FACING_V1.md`
- Horse exhausted follow/pet interaction: `Assets/_Project/Design/Horse/HORSE_EXHAUSTED_FOLLOW_AND_PET_INTERACTION_V1.md`
- Main menu/result flow: `Assets/_Project/Design/UI/MAIN_MENU_SETTINGS_RESULT_FLOW_V2.md`
- Main-menu visual direction: `Assets/_Project/Design/UI/MAIN_MENU_DREAMY_STORYBOOK_V1.md`
- Boot intro: `Assets/_Project/Design/UI/BBH_BOOT_INTRO_V1.md`
- Gameplay shadows: `Assets/_Project/Design/Rendering/GAMEPLAY_SHADOW_POLICY_V1.md`
- Map routes/hazards: `Assets/_Project/Design/Map/MAP_ROUTES_AND_HAZARDS_DESIGN_V1.md`
- Mother Boss: use `PROJECT_STATUS.md` as the final decision authority, supported by the documents under `Assets/_Project/Design/Bosses/`.

## Historical or superseded documents

The following are retained only when they still provide useful implementation history or detailed rationale:

- `Assets/_Project/Design/Roadmap/MASTER_REMAINING_WORK_ROADMAP_V128.md` — historical roadmap only; never use it as the active queue.
- Stage-numbered documents under `Assets/_Project/Design/QA/` — historical reports only.
- Version-numbered design documents — system-specific history/specification; they do not override `PROJECT_STATUS.md`.
- `MOTHER_BOSS_DESIGN_V2_WORKING.md` — working-era material only; final decisions are governed by `PROJECT_STATUS.md` and the later decision documents.

The contradictory `MAIN_MENU_SETTINGS_RESULT_FLOW_V1.md` has been removed because V2 explicitly corrected it.

## Repository-documentation rules

- Do not create `WORKING_NOW.md`, `PROJECT_STATUS_CURRENT*.md`, duplicate current roadmaps, or copied status snapshots.
- Do not commit chat exports, one-shot package READMEs, ZIP files, generated reports, or local patch tools.
- Every material code or requirement change must update `PROJECT_STATUS.md` and, when the public overview changes, `README.md`.
- Dedicated design documents may be added, but they must identify whether they are approved, working, superseded, or historical.
- Never delete an approved requirement merely because implementation changed; move its truth into the authoritative status before removing a duplicate document.
