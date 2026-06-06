# Boredom & Dungeons — Current Authoritative Status V4

Status date: 2026-06-06

This is the latest authoritative status index.

Authoritative package:

- `/PROJECT_STATUS_CURRENT_V4.md`
- `/PROJECT_STATUS_CURRENT_V3.md`
- `/PROJECT_STATUS_CURRENT_V2.md`
- `/PROJECT_STATUS_CURRENT.md`
- `/PROJECT_STATUS.md`
- `/Assets/_Project/Design/Collectibles/BATTERY_GUARDIAN_ENCOUNTER_V1.md`
- `/Assets/_Project/Design/Map/MAP_ROUTES_AND_HAZARDS_DESIGN_V1.md`
- `/Assets/_Project/Design/Bosses/MOTHER_BOSS_DESIGN_V3_DECISIONS.md`
- `/Assets/_Project/Design/Bosses/MOTHER_BOSS_HORSE_ENTRY_GATE_V1.md`

Newest status/design document wins for its feature area.

## Current execution order

1. Complete C01.10–C01.16: compile, `TEST EVERYTHING`, and Play Mode QA.
2. Implement C03.46–C03.55: player hazard damage and safe recovery.
3. Implement C04.24–C04.31: horse hazard avoidance and recovery.
4. Return to C07.16: real shared boss-framework encounter integration.
5. Complete Stage 16 and Stage 17 foundations.
6. Stop at the C07 category gate and request user review before C08.

## C09 — Mother Boss horse rule resolved

The final open Mother Boss design question is resolved:

- the horse does not participate in the Mother Boss encounter;
- after defeating the black/white boss, the player reaches the door leading to the ending/Mother sequence;
- the player must manually dismount before entering that door;
- mounted entry is blocked;
- a short prompt instructs the player to dismount;
- the game does not automatically carry the horse through the transition;
- after dismounting, the horse remains at a validated safe waiting point outside the doorway;
- follow, flee, wander, and recall systems cannot move it through the door;
- the horse does not appear in the Mother arena, Danger phase, defeat cutscene, or victory cutscene;
- if the player is already on foot, entry works normally even when the horse is distant, fainted, or absent;
- Mother defeat resets the horse with the full run;
- Mother victory ends through the colorful-light ending without a horse reunion scene.

Recommended English door prompt:

`Dismount before entering.`

Full specification:

`/Assets/_Project/Design/Bosses/MOTHER_BOSS_HORSE_ENTRY_GATE_V1.md`

## C04/C13 integration requirements

Future implementation must include:

- explicit mounted-state gate on the post-black/white-boss door;
- safe horse waiting-point validation;
- prevention of horse follow/teleport across the transition;
- cleanup of mounted camera, mounted weapon, rider, horse HUD, and horse AI state before the ending/Mother scene loads;
- QA for mounted blocked entry, manual dismount, on-foot entry, distant/fainted horse, defeat restart, and victory ending.

## Open Mother Boss decisions

No Mother Boss design question from the V2 list remains unresolved.

## Changelog

### 2026-06-06

- confirmed the horse is absent from the Mother Boss encounter;
- added mandatory manual dismount before the door after the black/white boss;
- added mounted-entry blocking and dismount prompt;
- added safe horse waiting state outside the door;
- added transition-state and QA requirements;
- closed the final unresolved Mother Boss design question.
