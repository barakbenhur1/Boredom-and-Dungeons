# Boredom & Dungeons — Current Authoritative Status V3

Status date: 2026-06-06

This is the latest status index.

Authoritative package:

- `/PROJECT_STATUS_CURRENT_V3.md`
- `/PROJECT_STATUS_CURRENT_V2.md`
- `/PROJECT_STATUS_CURRENT.md`
- `/PROJECT_STATUS.md`
- `/Assets/_Project/Design/Collectibles/BATTERY_GUARDIAN_ENCOUNTER_V1.md`
- `/Assets/_Project/Design/Map/MAP_ROUTES_AND_HAZARDS_DESIGN_V1.md`
- `/Assets/_Project/Design/Bosses/MOTHER_BOSS_DESIGN_V3_DECISIONS.md`

Newest status/design document wins for its feature area.

## Current execution order

1. Complete C01.10–C01.16: compile, `TEST EVERYTHING`, and Play Mode QA.
2. Implement C03.46–C03.55: player hazard damage and safe recovery.
3. Implement C04.24–C04.31: horse hazard avoidance and recovery.
4. Return to C07.16: real shared boss-framework encounter integration.
5. Complete Stage 16 and Stage 17 foundations.
6. Stop at the C07 category gate and request user review before C08.

## C06 — Battery clarification

- Batteries are physical map collectibles.
- They never come from chests.
- Approaching a Battery triggers one guardian encounter per run.
- Guardians spawn around the player and Battery with warning, fair minimum distance, safe placement, and a reachable escape route.
- Battery B is harder than Battery A without weakening fairness.

Full design:

`/Assets/_Project/Design/Collectibles/BATTERY_GUARDIAN_ENCOUNTER_V1.md`

## C09 — Mother Boss V3 decisions

Confirmed:

- visible resource is Patience, not HP;
- separate full Patience bar for Calm, Irritated, and Angry;
- capacities remain `2.5x`, `2.5x`, and `3x` black/white boss reference;
- normal player damage reduces Patience;
- Patience does not regenerate;
- phase transitions use animation only, preserve player movement, clear projectiles, heal `10%` max health, and do not change room geometry/lighting;
- broom is parryable;
- cleaner projectiles respect dodge i-frames;
- footwear projectiles cannot be destroyed;
- jump reduces attraction buildup by `35%`;
- dodge reduces attraction buildup by `60%`;
- Father cannot be parried;
- pinning stun is `1.75s`;
- scream stun is `0.75s` and interrupts player attacks;
- post-stun resistance is `1.25s`;
- Mother dodge follows the player’s core dodge rules;
- clothing cannot be attacked and scales in frequency by phase;
- clothing is disabled in Danger;
- Danger uses exactly four randomized task groups, no required order;
- task-group completion pauses Mother for `0.8s`;
- attacks cannot affect Mother during Danger;
- no player health damage, lava, holes, or chasms in Danger;
- light enemies delay through knockback only;
- all four task groups must be complete before the bed destination counts;
- failure occurs after Mother completes the Game Boy pickup animation;
- success: Mother stops and leaves, player takes Game Boy, colorful-light ending plays;
- defeat: short defeat animation, normal bed cutscene, `I'm bored`, full new-seed restart, all run items/boosts reset, no checkpoint;
- Mother arena is a stable transformed bedroom with no structural/lighting phase changes;
- Game Boy is hidden in phases 1–3 and revealed on a bedside table at Danger start.

Single unresolved design decision:

- whether the horse is present in the Mother Boss arena.

Full design:

`/Assets/_Project/Design/Bosses/MOTHER_BOSS_DESIGN_V3_DECISIONS.md`

## Changelog

### 2026-06-06

- resolved Patience-bar presentation and damage behavior;
- resolved transition behavior and recovery;
- resolved attack interactions, stun durations, and attraction reductions;
- resolved phase-aware clothing schedule;
- resolved four-group Danger structure and success/failure rules;
- resolved bedroom layout and no-hazard rule;
- left horse presence as the only open Mother Boss design question.
