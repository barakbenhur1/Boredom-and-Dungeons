# Boredom & Dungeons — Current Authoritative Development Status

Status date: 2026-06-06

This file is the current authoritative status index. It supersedes the execution snapshot in `/PROJECT_STATUS.md` while preserving that file as the detailed baseline for all previously recorded tasks.

The authoritative status package is:

- `/PROJECT_STATUS_CURRENT.md` — current category state, exact work order, and all newly recovered requirements.
- `/PROJECT_STATUS.md` — detailed categorized baseline and unchanged task inventory.
- `/Assets/_Project/Design/Map/MAP_ROUTES_AND_HAZARDS_DESIGN_V1.md` — complete multi-route map and hazard rules.
- `/Assets/_Project/Design/Bosses/MOTHER_BOSS_DESIGN_V1.md` — complete recovered Mother Boss design.

No requirement in the earlier baseline is removed.

## Current position

```text
Unity: 6000.0.76f1
Latest verified QA saved before final camera/minimap cleanup: PASS, 0 blockers, 0 warnings
Immediate gate: C01.10–C01.16
Saved feature resume point: C07.16
Earlier work inserted before returning to C07:
1. C03.46–C03.55 — player hazard damage and safe recovery
2. C04.24–C04.31 — horse hazard avoidance and recovery
Then resume: C07.16
```

## Change-order rule

- A new requirement before the current point is handled first, verified, documented, and then work returns to the saved resume point.
- A requirement inside the current category is inserted into the correct dependency order.
- A later requirement is recorded in its future category without abandoning the current point.
- Before entering a new category, update Git, announce the transition, and ask the user for corrections or additions.

# Complete category status

## C00 — Governance, documentation, and requirement recovery

Status: recurring.

Completed:

- persistent Git status tracking;
- dependency-ordered categories;
- earlier/current/later change rule;
- mandatory category transition review;
- historical roadmap marker;
- Mother Boss design recovery.

Remaining:

- keep every code/requirement/QA change synchronized;
- keep README/status pointers synchronized;
- record any further recovered requirements without guessing.

## C01 — Stability, QA, validation, and repository health

Status: VERIFY.

Immediate work:

- open latest `main` in Unity 6000.0.76f1;
- confirm clean compilation;
- run `Boredom And Dungeons -> TEST EVERYTHING`;
- rebuild the authoritative prototype scene if needed;
- run movement, combat, parry, shooting, reload, horse, death/reset, minimap, and Console smoke tests;
- add hazard smoke tests:
  - hole/chasm damage and safe respawn;
  - lava damage and non-lava recovery;
  - mounted player-and-horse recovery;
  - horse avoidance of both hazard types;
- save PASS reports;
- fix regressions before feature work resumes.

## C02 — Platform, input, architecture, scene assembly, and data

Status: IN PROGRESS.

Includes:

- Unity/C# foundation;
- desktop debug input;
- mobile landscape target;
- finger-trace/drag movement;
- tap-enemy attack targeting;
- shared input abstraction;
- scene-builder idempotency;
- explicit prefabs/ScriptableObjects;
- deterministic run seed ownership;
- removal of duplicate installers and runtime repair dependencies;
- stable Unity `.meta`/GUID handling.

## C03 — Player movement, combat, damage, weapons, and hazards

Status: PROTOTYPE DONE plus inserted earlier work.

Existing foundations remain as recorded in `/PROJECT_STATUS.md`:

- movement and aiming;
- dodge/i-frames;
- light/heavy attacks;
- attack buffer;
- landing attack;
- physical parry and time freeze;
- tap/charged shooting;
- reload and projectile rules;
- damage/death/reset foundations.

New ordered hazard work:

- **C03.46** validated last-safe-position tracker;
- safe point updates only on stable walkable ground outside hazards and away from invalid geometry;
- hole/chasm fall removes exactly 15 health;
- hole/chasm recovery returns the player to the latest valid safe point;
- lava contact removes exactly 10 health;
- lava recovery returns/knocks the player to the latest valid non-lava point;
- short recovery protection prevents immediate repeated hazard damage;
- mounted player-and-horse falls recover both together when safe;
- recovery cannot place actors inside enemies, walls, props, lava, holes, chasms, or boss barriers;
- test repeated falls, low-health falls, mounted falls, lava edges, invalid checkpoints, and death during hazard damage.

## C04 — Horse traversal, mounted combat, healing, flee, and hazards

Status: IN PROGRESS plus inserted earlier work.

Existing horse foundations remain unchanged.

New ordered hazard work:

- add hole/chasm avoidance to follow, flee, wander, approach, and relevant mounted pathing;
- add lava avoidance to the same states;
- horse loses no health if it falls into a hole/chasm;
- horse loses no health if it enters lava;
- horse returns to a legal horse-safe/non-lava point;
- mounted pair recovers together when possible, otherwise safely beside one another;
- prevent recovery loops and follow/flee overrides;
- run mounted/unmounted hazard tests and fallback tests.

## C05 — Normal enemies, AI roles, encounter behavior, and spawn safety

Status: IN PROGRESS.

Existing roster and requirements remain:

- Sword, Shooter, Patrol/Guard, Jumper, Bomb Placer;
- Rammer still pending;
- pursuit, exit blocking, flanking, bomb safety;
- safe spawn/landing/summon placement;
- no clustering or impossible exit blockage.

Additional hazard interaction rule:

- enemies blocking escape cannot force the player into an unavoidable hole, chasm, or lava area.

## C06 — Inventory, hidden collectibles, guardians, rewards, and run boosts

Status: IN PROGRESS.

Unchanged core rules:

- Game Boy, two Batteries, Game Cartridge;
- no secret checklist, empty slots, objective markers, or missing-item hints;
- badges only after pickup;
- reward chests after encounter completion;
- Parry freeze upgrade;
- regular run boosts and drop rules remain pending.

The complete hidden set now also unlocks the secret Mother Boss continuation.

## C07 — Boss framework, deterministic role planning, and encounter contracts

Status: CURRENT FEATURE CATEGORY after earlier hazard work.

Saved resume point:

- **C07.16** connect the shared framework to a real playable test encounter.

Existing C07 work remains unchanged, plus the framework must support:

- named boss-state presentation instead of a normal HP bar (`Calm`, `Irritated`, `Angry`, `Danger`);
- internal durability thresholds separate from visible state;
- three combat phases followed by a fourth non-combat objective phase;
- custom loss cinematic from any phase;
- clean full-run restart;
- post-ending secret-boss activation without breaking standard ending branches.

After C07.16:

- real damage/health channel wiring;
- boss HUD/presentation wiring;
- arena lock/intro/victory lifecycle;
- knockout/critical/linked-death tests;
- shared summon budget;
- Stage 16 report;
- deterministic 3-of-4 selection;
- legal room validation;
- bounded reroll/fallback;
- multi-seed testing;
- Stage 17 report;
- mandatory pause before C08.

## C08 — Individual mini-bosses

Status: DESIGN COMPLETE, implementation not started.

Unchanged roster:

- Square Jumper;
- Roller;
- Serpent;
- Quad Gunners.

All detailed attack, summon, health, reward, and QA requirements remain in `/PROJECT_STATUS.md`.

## C09 — Final and narrative bosses

Status: black/white boss design complete; Mother Boss design recovered and complete.

### Black/white boss

Unchanged:

- existing normal final boss;
- joined, split, and final split phases;
- magical exit barrier;
- linked final death and separate collapses.

### Mother Boss

Role:

- secret post-ending boss;
- unlocked only by Game Boy + two Batteries + Game Cartridge;
- does not replace the black/white boss.

Entry:

- colorful light first;
- bedroom door opens;
- only part, silhouette, or shadow of Mother appears;
- encounter begins.

Loss from any phase:

- player is shown in bed;
- says `I'm bored`;
- run restarts from the beginning.

Victory:

- colorful-light ending;
- Mother does not enter at the end;
- game ends on the colorful light.

Appearance:

- large model;
- ponytail;
- arms spread outward;
- during scream, arms extend farther to the sides;
- directional dodge like the player.

Visible states, not normal HP:

1. Calm;
2. Irritated;
3. Angry;
4. Danger.

Internal durability:

- Phase 1: `2.5x` black/white boss health reference;
- Phase 2: `2.5x`;
- Phase 3: `3x`;
- Phase 4: no combat durability.

Attacks:

- broom side sweep and overhead strike, available in all combat phases;
- close-range window-cleaner spray: three normal-damage fan projectiles;
- complex shoes/slippers bullet hell with slightly larger hit areas than normal bullets;
- full-screen attraction field with progressive walking slowdown/pull;
- jumping or dodging resets the accumulated pull;
- early attraction fires eight equal radial sectors;
- from the midpoint of Phase 2, Mother rotates and the rows become a spiral;
- Phase 3 uses spiral attraction from the beginning;
- Father runs across the screen for high collision damage;
- fast direct pinning beam: low damage, long stun;
- radial scream: medium-plus damage, short stun, arms farther outward;
- Mother dodges in all directions.

Foreground clothing:

- shirts, trousers, and underwear float between camera and arena;
- when active, minimum 2 and maximum 4 visible simultaneously;
- partial occlusion only, never the whole screen;
- cannot hide the player, critical telegraph, hazard edge, and only safe route at the same time.

Phase escalation:

- Calm: core broom/spray/basic footwear, stationary attraction, generous recovery;
- Irritated: faster combinations, Father/pinning/scream added, spiral begins at phase midpoint;
- Angry: full kit, spiral from start, stronger pull, denser patterns, controlled combinations;
- Danger: non-combat task race.

Fairness:

- no unavoidable pinning-stun chain into Father, scream, attraction, or lethal bullet hell;
- short stun resistance after recovery;
- distinct telegraphs;
- at least one legal escape option;
- Mother dodge cannot erase every punish window.

Phase 4 — Danger:

- Mother walks toward the Game Boy; her movement is the visible timer;
- player completes a validated subset of room tasks using existing movement/melee/shooting/pickup/combat mechanics;
- player must finish tasks and reach the marked spot beside the bed before Mother takes the Game Boy.

Approved task pool:

- hit/shoot dirt or dust targets;
- break light clutter blockers;
- shoot high stains/cobweb targets;
- collect toys/clothes by touch and auto-deliver through a basket/box zone;
- defeat 2–3 light enemies;
- clear a small ordered set of tidy spots by movement;
- navigate furniture/hazards and reach the bed endpoint.

Use a readable subset, not every task simultaneously. Task generation must never be impossible.

Full specification: `/Assets/_Project/Design/Bosses/MOTHER_BOSS_DESIGN_V1.md`.

## C10 — Map generation, routes, obstacles, hazards, placement, and pacing

Status: DESIGN EXPANDED, implementation planned.

Major route rules:

- every accepted map has at least 3 meaningful solution routes;
- target 4 when space/seed permits;
- routes may split, merge, and split again;
- each route contains a substantial unique segment and different rooms/areas;
- routes should ideally be spatially far apart;
- convergence into one route occurs only after enough progression;
- tiny nearby detours do not count;
- graph validation rejects fake/insufficient route families.

Inaccessible regions:

- approximate footprint: 1–4 room units;
- mountains, lakes, chasms, holes, columns, obstacles, or mixed formations;
- used as negative space, landmarks, route separators, and meaningful detours;
- cannot disconnect all routes or trap required content.

Room hazards:

- holes/chasms: player loses 15 health and respawns at latest safe point;
- lava: player loses 10 health and returns to latest safe non-lava point;
- horse avoids both and loses no health if recovery is required;
- player and horse recover together when they fall together;
- hazard placement cannot break required routes, bosses, chests, collectibles, or endings;
- hazard edges must remain readable under lighting, VFX, enemies, bullet hell, and foreground clothing.

Full specification: `/Assets/_Project/Design/Map/MAP_ROUTES_AND_HAZARDS_DESIGN_V1.md`.

## C11 — Camera, minimap, HUD, UI, readability, and accessibility

Status: IN PROGRESS.

Existing requirements remain, plus:

- Mother anger-state UI without a normal HP bar;
- concise Phase-4 task feedback that does not resemble the forbidden hidden-collectible checklist;
- hazard readability under all visual conditions;
- validate clothing events maintain 2–4 items only while active and never erase all critical information.

## C12 — Art, animation, VFX, lighting, atmosphere, and audio

Status: PLANNED.

Existing production list remains, plus:

- Mother model, ponytail, arm poses, broom, spray, footwear, attraction, spiral, Father, beam, scream, dodge, state transitions;
- foreground clothing system;
- hazard fall/lava/recovery VFX;
- all Mother and hazard audio.

## C13 — Story, endings, cinematics, and final result logic

Status: DESIGN EXPANDED.

Incomplete-set endings remain unchanged.

Complete-set path:

1. colorful light on the player’s face;
2. bedroom door opens;
3. partial Mother/shadow reveal;
4. Mother Boss encounter.

Mother loss from any phase:

- bed cutscene;
- `I'm bored`;
- reset to beginning.

Mother victory:

- colorful-light ending;
- no Mother entrance;
- end on colored light.

All transitions must reset run/boss/room state correctly and must never advertise hidden collectible requirements before the ending.

## C14 — Balance, performance, persistence, production cleanup, and release

Status: PLANNED.

Existing requirements remain, plus:

- balance approved Mother durability multipliers without changing them;
- balance attraction, spiral, Father, stuns, clothing, dodge, and Phase-4 timing;
- balance 15-damage chasm and 10-damage lava penalties;
- stress-test Mother’s three combat pools and fourth phase;
- stress-test route-family validation, inaccessible regions, hazards, and safe points across many seeds;
- final acceptance requires Mother unlock/four phases/loss restart/victory ending;
- every accepted map requires at least 3 meaningful routes, targeting 4;
- all player/horse hazard rules must pass.

# Exact current work sequence

1. Complete C01.10–C01.16.
2. Implement and verify C03.46–C03.55.
3. Implement and verify C04.24–C04.31.
4. Update Git and return to C07.16.
5. Complete Stage 16 framework integration, including Mother-compatible state/non-combat phase/loss routing.
6. Complete Stage 17 deterministic selection and legal placement.
7. Update Git and stop at the C07 category gate.
8. Announce completion of C07 and propose C08.
9. Ask the user for corrections before entering C08.
10. Keep C09/C10/C13/C14 work in the recorded future positions unless a new earlier blocking request is added.

# Current blockers and risks

Blockers:

- latest code still requires the C01 verification gate;
- no Mother design blocker remains.

Risks:

- fake route diversity from tiny nearby detours;
- safe-point loops or invalid respawn positions;
- horse recovery overridden by flee/follow logic;
- stun-lock combinations in Mother combat;
- foreground clothing hiding critical information;
- impossible Phase-4 task combinations;
- post-ending boss transitions leaking state into restart or victory endings.

# Changelog

## 2026-06-06

- recovered and documented multi-route map topology;
- added 1–4-room-scale inaccessible regions;
- added exact chasm/lava player and horse rules;
- added safe checkpoint and paired player/horse recovery;
- recovered the complete Mother Boss design;
- defined phase adaptations and fairness constraints;
- added Phase-4 task suggestions using existing mechanics;
- added secret complete-set ending continuation, loss restart, and victory result;
- inserted earlier hazard implementation before the saved C07 resume point.
