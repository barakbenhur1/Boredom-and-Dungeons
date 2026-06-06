# Boredom & Dungeons — Documentation Index

Status date: **2026-06-06**

## Authority order

1. `/PROJECT_STATUS.md` — complete authoritative plan, requirements, QA truth, blockers, and resume point.
2. `/README.md` — current project overview.
3. `/DOCUMENTATION_INDEX.md` — document ownership and status map.
4. `Assets/_Project/Design/**` — focused specifications.
5. `Assets/_Project/Design/QA/**` — historical reports.

When documents conflict, the earlier item wins.

## Current snapshot

- Engine: Unity `6000.0.76f1`.
- Current committed baseline: natural movement, wider/faster horse movement, enemy-awareness improvements, temporary facing markers, START GAME emphasis, and long-press spinning AOE light attack.
- Current verification gate: clean Unity compilation, `Boredom And Dungeons -> TEST EVERYTHING`, and Play Mode verification.
- Approved next horse change: two-step retreat from a detected hazard instead of a one-second stationary refusal.
- Approved next minimap change: slower polished interpolation between the existing four cardinal targets.
- No second required QA command may be added.

## Current focused specifications

- `Design/Combat/SPINNING_AOE_LONG_PRESS_LIGHT_ATTACK_V1.md`
- `Design/Movement/NATURAL_MOVEMENT_AWARENESS_FACING_V1.md`
- `Design/Horse/HORSE_HAZARD_TWO_STEP_RETREAT_V1.md`
- `Design/Horse/HORSE_EXHAUSTED_FOLLOW_AND_PET_INTERACTION_V1.md`
- `Design/UI/MINIMAP_POLISHED_CARDINAL_ROTATION_V1.md`
- `Design/UI/MAIN_MENU_SETTINGS_RESULT_FLOW_V2.md`
- `Design/UI/MAIN_MENU_DREAMY_STORYBOOK_V1.md`
- `Design/UI/BBH_BOOT_INTRO_V1.md`
- `Design/Rendering/GAMEPLAY_SHADOW_POLICY_V1.md`
- `Design/Map/MAP_ROUTES_AND_HAZARDS_DESIGN_V1.md`

Mother Boss final decisions are governed by `/PROJECT_STATUS.md` and supported by the later documents in `Design/Bosses/`.

## Enemy-layout clarification

The prototype uses randomness when the Unity scene is generated, but a normal run reloads the existing generated scene. Enemy order can therefore feel identical between runs. Enemy type progression is also partly based on map depth and cell coordinates.

Final run variation is already planned through run-seed ownership, full map generation, random legal encounter placement, and multi-seed validation. Do not add a temporary competing randomization system now.

## Historical and superseded material

- `Design/Roadmap/MASTER_REMAINING_WORK_ROADMAP_V128.md` is historical only.
- Stage-numbered files in `Design/QA/` are historical reports.
- `Design/Bosses/MOTHER_BOSS_DESIGN_V2_WORKING.md` is working-era material, not final authority.
- `Design/UI/MAIN_MENU_SETTINGS_RESULT_FLOW_V1.md` is only a short redirect to V2.

## Rules

- Do not create `WORKING_NOW.md`, `PROJECT_STATUS_CURRENT*.md`, duplicate live roadmaps, or copied status snapshots.
- Do not commit ZIP files, package payloads, chat exports, one-shot patch tools, local QA output, or accidental terminal files.
- Every material code or requirement change must update `/PROJECT_STATUS.md`; update `/README.md` when the overview changes.
- Keep Unity `.meta` files for committed assets.
