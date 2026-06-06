# Boredom & Dungeons — Design Documentation

Status date: **2026-06-06**

This folder contains detailed specifications and historical QA records. It is not a second project-status system.

## Authority

- `/PROJECT_STATUS.md` is the only complete authoritative plan and status.
- `/README.md` is the current overview.
- `/DOCUMENTATION_INDEX.md` identifies canonical, working, superseded, and historical documents.

## Folder roles

- `Bosses/`, `Combat/`, `Horse/`, `Map/`, `Movement/`, `Rendering/`, `UI/`, and the other system folders contain focused specifications.
- `QA/` contains historical stage reports.
- `Roadmap/MASTER_REMAINING_WORK_ROADMAP_V128.md` is historical only.

## Current clarifications

- `UI/MAIN_MENU_SETTINGS_RESULT_FLOW_V2.md` is current.
- `UI/MAIN_MENU_SETTINGS_RESULT_FLOW_V1.md` is only a short superseded redirect.
- `Horse/HORSE_HAZARD_TWO_STEP_RETREAT_V1.md` records the approved next horse-hazard behavior.
- `UI/MINIMAP_POLISHED_CARDINAL_ROTATION_V1.md` records the approved next minimap behavior.
- Mother Boss final decisions are governed by `/PROJECT_STATUS.md` and supported by the later boss decision documents.
- Natural movement and spinning AOE code still require the current Unity verification gate.

## Maintenance

For every material decision, update `/PROJECT_STATUS.md` first. Update the focused specification and public README when relevant. Do not create duplicate live status files.
