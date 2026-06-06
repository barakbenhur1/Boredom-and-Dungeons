# Boredom & Dungeons — Current Authoritative Status V5

Status date: 2026-06-06

This is the latest authoritative status index.

Authoritative package:

- `/PROJECT_STATUS_CURRENT_V5.md`
- `/PROJECT_STATUS_CURRENT_V4.md`
- `/PROJECT_STATUS_CURRENT_V3.md`
- `/PROJECT_STATUS_CURRENT_V2.md`
- `/PROJECT_STATUS_CURRENT.md`
- `/PROJECT_STATUS.md`
- `/Assets/_Project/Design/Horse/HORSE_EXHAUSTED_FOLLOW_AND_PET_INTERACTION_V1.md`
- `/Assets/_Project/Design/Bosses/MOTHER_BOSS_HORSE_ENTRY_GATE_V1.md`
- `/Assets/_Project/Design/Bosses/MOTHER_BOSS_DESIGN_V3_DECISIONS.md`
- `/Assets/_Project/Design/Collectibles/BATTERY_GUARDIAN_ENCOUNTER_V1.md`
- `/Assets/_Project/Design/Map/MAP_ROUTES_AND_HAZARDS_DESIGN_V1.md`

Newest status/design document wins for its feature area.

## Current execution order

1. Complete C01.10–C01.16: compile, `TEST EVERYTHING`, and Play Mode QA.
2. Implement C03.46–C03.55: player hazard damage and safe recovery.
3. Implement C04.24–C04.31: horse hazard avoidance and recovery.
4. Implement C04.32–C04.39: zero-health exhausted follow and contextual Pet interaction.
5. Return to C07.16: real shared boss-framework encounter integration.
6. Complete Stage 16 and Stage 17 foundations.
7. Stop at the C07 category gate and request user review before C08.

## C04 — New horse requirements

### Zero-health exhausted follow

- Horse reaching zero health keeps its current nearby behavior.
- If the player becomes genuinely far away, the zero-health horse moves toward the player very slowly.
- This is separation/soft-lock prevention only; it does not restore health or return normal behavior.
- Working start threshold: greater than `14m` for `1.25s`.
- Working stop threshold: `8m` or less.
- Working speed: approximately `20%` of normal follow speed.
- Obstacle, hole, chasm, lava, barrier, and doorway avoidance remain active.
- Mounting, mounted shooting, flee, sprint, attack, and normal follow remain disabled at zero health.
- Healing exits the exhausted-follow state through the normal recovery flow.

### Contextual Pet button

- A `Pet` button appears when the player is on foot and within `2.25m` of the horse, with a valid safe interaction position.
- It hides when the player leaves range or enters combat/transition states.
- It may be used with a healthy, damaged, or zero-health horse.
- Petting has no healing, stat, score, collectible, or progression effect.

Short press:

- player pets the horse.

Long press:

- threshold: `0.65s`;
- horse pets the player through a head nuzzle/muzzle rub/affection animation;
- it must not imitate a human hand-petting animation.

Input rules:

- short and long press are mutually exclusive;
- releasing after long-press activation does not also fire short press;
- visible hold progress is shown;
- interaction cancels safely for movement out of range, attack, dodge, mount, damage interrupt, cutscene, or door transition;
- no duplicate interaction or stuck player/horse state.

Full specification:

`/Assets/_Project/Design/Horse/HORSE_EXHAUSTED_FOLLOW_AND_PET_INTERACTION_V1.md`

## Relationship to Mother Boss door

- Zero-health exhausted follow and Pet interactions do not bypass the mandatory dismount gate after the black/white boss.
- The horse remains outside the ending/Mother sequence.
- Pet UI and horse interaction are disabled during the post-boss doorway transition and inside the Mother sequence.

## Changelog

### 2026-06-06

- added very slow zero-health follow when the player becomes far away;
- preserved current zero-health behavior while player remains nearby;
- added contextual `Pet` button;
- added short press player-pets-horse interaction;
- added long press horse-nuzzles-player interaction;
- added proximity, input, cancellation, UI, safety, and QA rules;
- inserted this C04 work before the saved C07 resume point.
