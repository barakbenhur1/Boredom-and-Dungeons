# Boredom & Dungeons — Current Authoritative Status V2

Status date: 2026-06-06

This file is the latest authoritative status index.

It preserves all earlier requirements and points to the full current status package:

- `/PROJECT_STATUS_CURRENT_V2.md` — latest execution/status index.
- `/PROJECT_STATUS_CURRENT.md` — previous complete category snapshot.
- `/PROJECT_STATUS.md` — detailed categorized baseline.
- `/Assets/_Project/Design/Collectibles/BATTERY_GUARDIAN_ENCOUNTER_V1.md` — authoritative Battery encounter rules.
- `/Assets/_Project/Design/Map/MAP_ROUTES_AND_HAZARDS_DESIGN_V1.md` — route and hazard design.
- `/Assets/_Project/Design/Bosses/MOTHER_BOSS_DESIGN_V1.md` — recovered Mother Boss baseline.
- `/Assets/_Project/Design/Bosses/MOTHER_BOSS_DESIGN_V2_WORKING.md` — confirmed Patience model and open design decisions.

When snapshots differ, this file and the newest dedicated design file win.

## Current execution position

```text
Immediate prerequisite: C01.10–C01.16 — compile, TEST EVERYTHING, Play Mode smoke QA
Earlier inserted implementation:
1. C03.46–C03.55 — player hazards and safe recovery
2. C04.24–C04.31 — horse hazard avoidance and recovery
Saved feature resume point: C07.16
Current design discussion: Mother Boss V2 — Patience, phase escalation, and Danger phase
```

## C06 clarification — Batteries and guardians

The Battery collectible flow is now explicit:

- Both Batteries are physical map collectibles.
- Batteries never come from reward chests.
- A player approaching a Battery activates its guardian encounter.
- Guardians spawn into the map around the player and Battery.
- Guardians must not spawn directly next to the player.
- The encounter must be hard through pressure, enemy roles, angles, and numbers, but fair through warning, spacing, safe placement, and a reachable escape direction.
- Guardians avoid the player safety radius, Battery safety radius, other guardians, walls, props, lava, holes, chasms, inaccessible terrain, and active barriers.
- Invalid candidate positions are rerolled/scored; if no legal position exists, delay/cancel that spawn rather than force an invalid spawn.
- The encounter activates once per run and cannot duplicate or spawn endlessly.
- After victory, the Battery remains physically where it was placed and becomes safely collectible.
- Battery A is the first serious protected-collectible encounter: hard but fair.
- Battery B is harder through stronger pressure/role combinations, without weakening fairness rules.
- Hidden collectible rules remain unchanged: no objective marker, checklist, empty slot, missing-item text, or advance advertising.

Full design:

`/Assets/_Project/Design/Collectibles/BATTERY_GUARDIAN_ENCOUNTER_V1.md`

## Mother Boss clarification — Patience, not HP

Confirmed:

- Mother has a visible **Patience** bar, not a normal HP bar.
- Player attacks reduce Patience.
- Emptying the bar ends the current emotional phase; it does not physically kill or injure Mother.
- The bar refills for the next phase.
- Calm Patience capacity: `2.5x` black/white boss health reference.
- Irritated Patience capacity: `2.5x`.
- Angry Patience capacity: `3x`.
- Danger has no Patience bar because it is a non-combat objective phase.

Working escalation and all unanswered questions are maintained in:

`/Assets/_Project/Design/Bosses/MOTHER_BOSS_DESIGN_V2_WORKING.md`

## Change-order decision

The Battery clarification changes C06 requirements but does not require abandoning the current design discussion.

It is recorded now and will be enforced when C06/encounter-map work is implemented.

The current conversation resumes with the unanswered Mother Boss questions, beginning with the Patience-bar presentation and phase-transition rules.

## Changelog

### 2026-06-06

- clarified that Batteries are physical map collectibles, never chest rewards;
- documented approach-triggered guardian spawning around the player;
- preserved difficult-but-fair spawn spacing and encounter pressure;
- recorded Battery A/B difficulty distinction and one-activation lifecycle;
- confirmed Mother’s visible resource is Patience rather than HP;
- added Mother Boss V2 working design and open-decision list.
