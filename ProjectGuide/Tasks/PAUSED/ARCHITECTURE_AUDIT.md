# Architecture, Gameplay, Camera, Run-System and Documentation Audit V1

## Task identity

- **Task ID:** `C01.ARCH.AUDIT.V1`
- **Status:** `AUDIT IN PROGRESS`
- **Baseline commit:** `50ca143`
- **Audit mode:** inspection and evidence gathering only; no broad refactor before the audit report
- **Automated baseline:** Unity `TEST EVERYTHING` passed on `2026-06-08T18:17:20.5169230Z` with 0 blockers, 0 warnings and 0 info items

## Why this task exists

The project has grown through many additive gameplay, camera, horse, hazard, combat, UI, QA and documentation iterations. Several regressions were repaired successfully, but the accumulated implementation now has a meaningful risk of:

- multiple systems reading physical input independently;
- more than one component writing actor or camera transforms;
- camera room-handoff movement that still requires measured diagnosis;
- state represented by many independent booleans and timers;
- magic timing standing in for explicit events;
- mixed gameplay, real-time, physics and animation clocks;
- reflection, scene searches, global locks or hidden dependencies;
- Inspector values being overwritten by code;
- fixed buffers silently dropping gameplay interactions;
- duplicated tuning and unclear sources of truth;
- documentation and QA checking text rather than behavior;
- important decisions being recoverable only from chat history.

The audit exists to produce an evidence-based map before any wide architectural refactor. It must prevent a large cleanup from breaking already-approved gameplay.

## Originating approved task

The authoritative user task is the full gameplay/camera/architecture audit request uploaded on 2026-06-08. It requires:

- a full mapping audit first;
- canonical system contracts and documentation second;
- measured camera diagnosis before camera repair;
- one input owner and command architecture;
- explicit player action state and movement authority;
- cleanup of timing, clocks, reflection, locks, searches, tuning, buffers and runtime instantiation;
- resume/run/pacing/meta/performance planning;
- behavioral QA, Play Mode evidence and profiling;
- no redesign of characters, attacks, story content, enemies, bosses, items or visual identity.

## Protected requirements and non-goals

This task must not:

- redesign characters;
- invent characters, enemies, bosses, areas, items, attacks or abilities;
- change approved attack specifications;
- rewrite story content except where a shared system contract requires it;
- enlarge rooms or regenerate the map to hide a camera defect;
- convert Resume into a normal player-controlled Save system;
- choose the unresolved run-duration target between 3–4 hours and 6–8 hours;
- choose unresolved mobile-control layout;
- choose unresolved resume restore position;
- claim camera, input feel, Play Mode, device or performance success from static checks;
- start a broad refactor before Phase 1 findings are documented.

All changes remain additive by default.

## Approved game/run truths protected by the audit

- one continuous run until victory, death or explicit abandon;
- random new Seed for each new run;
- 80–120 rooms per run;
- 3–5 regions and 3–5 meaningful routes;
- dynamic macro pacing with local encounter-duration variety;
- all mandatory routes possible on foot;
- no visible objective/checklist that reveals the mystery;
- one death ends the run;
- Resume exists only for accidental interruption recovery;
- meta progression persists across runs;
- regular victory, full Mother route and later Father route remain as already approved;
- current character, boss and attack content is outside redesign scope.

## Mandatory phase breakdown

### Phase 1 — Audit only — `IN PROGRESS`

Map and document:

1. every physical input reader;
2. every player, horse, enemy and traversal transform writer;
3. every camera transform/FOV writer;
4. all clocks and mixed-clock operations;
5. all magic timings that may replace state/events;
6. gameplay reflection;
7. scene-wide searches and repeated hierarchy/component lookup;
8. global/static locks and ownership;
9. duplicated or code-overwritten tuning;
10. OnGUI runtime UI;
11. fixed physics/gameplay buffers and overflow behavior;
12. runtime primitive/material/VFX/projectile instantiation;
13. current and missing canonical documents;
14. completed, superseded, duplicate, temporary or irrelevant documents that must be merged and removed before the next commit;
15. contradictions among requirements, code, scenes, prefabs, QA and documentation.

**Deliverable:** evidence-based audit findings, contradictions, open decisions, severity, affected files/symbols, and recommended ordering.

### Phase 2 — Documents and contracts — `PLANNED`

- update `ProjectGuide/Status/CURRENT.md`;
- create or update canonical system contracts;
- update `ProjectGuide/INDEX.md`;
- correct technical-decision numbering;
- record open decisions without guessing.

### Phase 3 — Camera diagnosis and minimal repair — `PLANNED`

- add gated diagnostics;
- reproduce on foot and mounted;
- identify the actual writer/source;
- measure distance, yaw, pitch, FOV, screen-space and one-frame displacement;
- repair minimally;
- verify both room-transition directions, corners, diagonals, hazards, intro and boss framing.

### Phase 4 — Input/state/movement ownership — `PLANNED`

- one input adapter;
- intent/command layer;
- player action state machine;
- movement authority/arbitration;
- token-based locks;
- event-driven mount and camera handoff;
- remove gameplay reflection and duplicate reads.

### Phase 5 — Data and timing cleanup — `PLANNED`

- explicit clocks;
- events/callbacks instead of hidden timing;
- data-driven profiles;
- eliminate Inspector/code source conflicts;
- overflow handling;
- cached/event-driven references.

### Phase 6 — Resume and run systems — `PLANNED`

- snapshot contract without choosing the unresolved restore position;
- cleanup rules;
- run pacing profile;
- 80–120 room-count model;
- dynamic duration model;
- meta scoring and anti-farm.

### Phase 7 — QA and profiling — `PLANNED`

- static checks;
- runtime behavior tests;
- focused Play Mode;
- mobile/device checks;
- performance profiling;
- documentation-truth gate.

## Preliminary findings already evidenced

These are audit findings, not repaired items.

### AUDIT-F001 — Physical input is read by more than one gameplay component

Evidence reviewed at baseline:

- `BDPlayerController` reads keyboard, gamepad, touch and mouse state directly.
- `BDPlayerCombat` independently reads combat button state and owns tap/hold timing.

Impact:

- the approved one-input-owner command architecture is not implemented;
- automated replay, remapping, accessibility, buffering and deterministic tests are harder;
- input locks must be repeated across components.

Status: `CONFIRMED / NOT YET REFACTORED`.

### AUDIT-F002 — Player movement tuning has two sources of truth

Evidence:

- movement values are serialized fields;
- `BDPlayerController.Awake()` calls `ApplyNaturalMovementProfile()`;
- that method overwrites several serialized movement and aim values.

Impact:

- Inspector values may appear editable while code silently replaces them;
- tuning provenance and scene serialization truth are unclear.

Status: `CONFIRMED / NOT YET REFACTORED`.

### AUDIT-F003 — Camera target handoff still uses polling and scene search

Evidence:

- `BDCameraFollow.ResolveTargets()` polls target state;
- it resolves on a timed interval;
- it uses `FindFirstObjectByType<BDHorseHealth>()` when the cached horse is absent.

Impact:

- target ownership is not fully event-driven;
- polling can make mount/dismount and scene reload continuity harder to prove;
- missing/duplicate actor references are not represented as an explicit handoff contract.

Status: `CONFIRMED / REQUIRES FULL CALL-SITE MAP`.

### AUDIT-F004 — Camera body is constrained before and after smoothing

Evidence:

- `FollowLockedBehind()` computes a constrained desired position;
- smooths toward it;
- applies a final constraint again.

Impact:

- this may be correct for safety, but it is a candidate source of boundary pressure or oscillation;
- the audit must measure it rather than assume it is the camera bug.

Status: `MEASUREMENT REQUIRED / NOT A CONFIRMED DEFECT`.

### AUDIT-F005 — Combat uses fixed non-alloc buffers

Evidence:

- melee overlap and health buffers have fixed capacities;
- the current reviewed path does not yet prove complete overflow reporting or fallback behavior.

Impact:

- dense encounters may silently omit a valid target depending on collider order;
- requires full search of buffer guards and QA before classification.

Status: `PARTIAL EVIDENCE / FULL AUDIT REQUIRED`.

### AUDIT-F006 — Combat action state is spread across booleans and timers

Evidence:

- pending light, pending heavy, ranged pending, charged state, reloading and cooldown timestamps coexist in `BDPlayerCombat`.

Impact:

- legal/illegal simultaneous states and cancellation ownership are difficult to prove;
- supports the approved state-machine audit, but does not by itself justify a broad rewrite.

Status: `CONFIRMED STRUCTURAL RISK / BEHAVIOR PRESERVATION REQUIRED`.

### AUDIT-F007 — Repository status was stale after the latest successful QA run

Evidence:

- `ProjectGuide/Status/CURRENT.md` still said the V23R19K Unity rerun was required;
- the uploaded QA result subsequently proved 0 blockers, 0 warnings and 0 info.

Impact:

- a new chat or Codex session could repeat completed QA or treat repaired blockers as active;
- this task-continuity package updates the current truth.

Status: `DOCUMENTATION REPAIR INCLUDED IN V23R19L`.

## V23R19K focused Play Mode gates retained

Automated QA passed, but the following remain open until focused Play Mode/user verification:

1. airborne Light and Heavy use only the correct vertical slash orientation;
2. player death animation is visible before the menu;
3. large enemies and Battery guardians show death before loot/despawn;
4. confirmed abandon loads a clean main menu rather than overlaying gameplay;
5. abandon → Start Game keeps the player mounted from the first visible intro frame;
6. the Boy cannot use sword melee or the hook while mounted and does not consume hook cooldown.

## Documentation continuity requirements for this task

Every audit finding, contradiction, decision, code change and verification result must be updated in the same change in:

- this task record;
- `ProjectGuide/Status/CURRENT.md`;
- the relevant canonical domain document;
- `OPEN_BUG_TRACKER.md` when a defect is involved;
- `ProjectGuide/Engineering/ARCHITECTURE.md` when ownership changes;
- `ProjectGuide/Engineering/TECHNICAL_DECISIONS.md` for durable choices;
- `ProjectGuide/QA/QA_CHECKLIST.md` and automated QA when verification rules change;
- `ProjectGuide/Engineering/PERFORMANCE.md` when budgets or profiling policy change;
- `ProjectGuide/INDEX.md` when maintained-document responsibility changes.

## Current verification truth

Verified:

- commit `50ca143` is on `main`;
- working tree was reported clean immediately after push;
- Unity `TEST EVERYTHING` passed at `2026-06-08T18:17:20.5169230Z`;
- blockers: 0;
- warnings: 0;
- info: 0.

Not yet verified:

- focused V23R19G/V23R19H/V23R19K Play Mode behavior;
- camera transition feel and numeric acceptance metrics;
- input architecture behavior after any future refactor;
- mobile device behavior;
- performance budgets and target-device profiling;
- complete repository-wide audit coverage.

## Exact resume point

Continue **Phase 1 — Audit only**.

The next contributor must:

1. finish repository-wide search/mapping for all input readers;
2. map every transform and camera writer;
3. map clocks, timing-based transitions, reflection, scene searches and static/global locks;
4. map duplicated tuning, OnGUI, fixed buffers and runtime instantiation;
5. inspect scenes, prefabs, ScriptableObjects, Editor installers and QA—not only Runtime filenames;
6. perform a documentation relevance sweep and classify every non-canonical/task document as keep, merge-then-remove, obsolete-remove or commit-blocking;
7. add every finding here with exact file/symbol evidence and severity;
8. update `ProjectGuide/Status/CURRENT.md` in the same change;
9. do not begin broad implementation until the Phase 1 audit report is complete.

## Required final audit output

- `AUDIT FINDINGS`
- `CONTRADICTIONS`
- `OPEN DECISIONS`
- `DOCUMENT CHANGES`
- `CODE CHANGES`
- `CAMERA DIAGNOSIS`
- `QA RESULTS`
- `DEFERRED WORK`
- `EXACT RESUME POINT`

<!-- B&D V23R19M EARLIER BLOCKER INTERRUPTION START -->
## Earlier blocker interruption — V23R19M

The Phase 1 audit is temporarily paused because focused Play Mode found two earlier visual regressions and the post-V23R19L automated run exposed two stale historical QA requirements.

### Confirmed reports

1. Airborne attack: the long attack line is still horizontal because the prior local-X rotation changed the plane but not the mesh's long left-to-right axis.
2. Small regular-enemy death: the current shared scale compression reads as an unnatural flat/pancake collapse.
3. Automated QA at `2026-06-08T18:53:41.7256860Z` reported 2 blockers because V23R19J/V23R19K scanners required resolved/historical bug-ledger text to remain in the current open table.

### Repair scope

- rotate the airborne mesh around local Z so its long axis becomes top-to-bottom;
- add an intact-body recoil/fall branch only for small regular enemies;
- preserve player, large, Elite, mini-boss and boss death paths;
- remove historical scanner dependencies on resolved bug rows/status history;
- rerun TEST EVERYTHING and focused Play Mode;
- then resume this audit at the exact Phase 1 mapping point recorded below.

### Documentation relevance sweep

- No maintained feature/task document is safe to delete in this focused repair.
- Obsolete historical bug-ledger/status dependencies are removed from QA source rather than preserving stale documentation.
- The active audit record remains required because the audit resumes immediately after the blockers pass.
<!-- B&D V23R19M EARLIER BLOCKER INTERRUPTION END -->

<!-- B&D V23R19N LEGACY QA INTERRUPTION START -->
## Earlier blocker interruption — V23R19N

The Phase 1 audit remains paused while a documentation/QA semantic blocker is cleared.

- Unity run: `2026-06-08T19:10:30.7997940Z`.
- Result: 6 blockers, 0 warnings, 0 info.
- Root cause: three legacy scanners each required the two superseded local-X tokens removed by V23R19M.
- Runtime impact: none.
- Repair: update only V23R19D/V23R19E/V23R19G QA expectations to the active local-Z contract.
- Remaining verification after automated PASS: V23R19M grounded/airborne Light/Heavy visuals, two small-enemy deaths and one large/Elite death.
- Exact resume point afterward: continue Phase 1 repository-wide mapping.

### Documentation relevance sweep

No maintained document is obsolete because of this focused QA repair. No document is removed. Historical implementation tokens are removed only from active QA expectations; Git history remains the archive.
<!-- B&D V23R19N LEGACY QA INTERRUPTION END -->

<!-- B&D V23R19O EARLIER BLOCKER INTERRUPTION START -->
## Earlier blocker interruption — V23R19O

The architecture audit remains paused for one serious run-flow regression and two focused gameplay-presentation refinements.

### Verified before this interruption

- TEST EVERYTHING passed at `2026-06-08T19:25:10.9933680Z` with 0 blockers, 0 warnings and 0 info.
- The user accepted the corrected airborne Light/Heavy orientation, absence of a horizontal duplicate, the small regular-enemy intact fall, and the retained large/Elite death path.

### Active interruption

1. Boy renderer missing during mounted intro after abandon until the cinematic ends.
2. Target outline includes a non-damageable enemy ring instead of only the vulnerable model.
3. Enemy ring should be subtler.
4. Wall Jump should be higher/farther and allow steering with matching model/camera direction.

### Exact resume point

After V23R19O automated and focused Play Mode gates pass, return to Phase 1 repository-wide mapping. Do not begin the broad architecture refactor inside this focused repair.

### Documentation relevance sweep

- Resolved V23R19M/V23R19N bug rows are removed from the current open-bug table after their verified truth is summarized here and in `ProjectGuide/Status/CURRENT.md`.
- No current canonical design or active task document is obsolete.
- Historical repair packages remain in Git history; current maintained documents contain only active/durable truth.
<!-- B&D V23R19O EARLIER BLOCKER INTERRUPTION END -->

<!-- B&D V23R19P QA + CATERPILLAR CAPTURE INTERRUPTION START -->
## Earlier interruption — V23R19P

- Automated run: `2026-06-08T20:02:41.9132920Z`.
- Result: 3 blockers, 0 warnings, 0 info.
- Root cause: three stale/brittle QA text expectations; no compiler failure.
- Runtime change in V23R19P: none.
- Current focused Runtime verification remains V23R19O.

The approved Caterpillar gambling NPC was also captured as future work, including the latest clarifications:

- only specifically selected rooms can host it;
- not every room receives one;
- selected-room Caterpillars appear only after room clear through animation;
- enemy presence causes animated disappearance;
- active gambling prevents enemies from approaching or attacking the player;
- all unspecified game/economy/presentation values remain open.

### Exact resume point

After V23R19P automated PASS, finish V23R19O focused Play Mode. Then return to Phase 1 repository-wide audit mapping.

### Documentation relevance sweep

The uploaded prompt is not kept as a duplicate live document. Its valid content is merged into the canonical Caterpillar specification, global status and relevant architecture/economy/QA records. No current canonical document is obsolete.
<!-- B&D V23R19P QA + CATERPILLAR CAPTURE INTERRUPTION END -->

<!-- B&D V23R19Q UI INTERRUPTION START -->
## Earlier interruption — V23R19Q

The architecture audit remains paused for a user-requested professional UI pass and two QA-token repairs.

- Automated source run: `2026-06-08T20:14:21.3030580Z`.
- Result: 2 blockers, 0 warnings, 0 info.
- Runtime gameplay impact of blockers: none.
- UI scope: BBH intro, handheld shell, Main, Settings, Pause, Abandon, Loading and backdrop focus.
- Preservation rule: no existing menu content, behavior or boot effect may be removed.
- Performance rule: cached textures only; no per-frame resource creation.
- Exact resume point: after V23R19Q visual acceptance and retained V23R19O focused checks, continue Phase 1 repository-wide mapping.
<!-- B&D V23R19Q UI INTERRUPTION END -->

<!-- B&D V23R19R ORDERED INTERRUPTION START -->
## Current ordered interruption — V23R19R

The architecture audit remains paused.

Mandatory order:

1. close the single V23R19R automated blocker;
2. implement and verify Enemy Attack Animations;
3. implement and verify the Professional Opening Cinematic;
4. complete retained implemented-but-unconfirmed checks;
5. resume Phase 1 audit mapping.

All details, bugs, verification levels and future tasks are maintained in:

`ProjectGuide/Status/WORK_QUEUE.md`
<!-- B&D V23R19R ORDERED INTERRUPTION END -->

<!-- B&D V23R19S QA INTERRUPTION START -->
## Current interruption — V23R19S

The architecture audit remains paused.

The only active automated blocker is a brittle continuity-QA sentence check. V23R19S changes QA/documentation only.

After 0 blockers, 0 warnings and 0 info:

1. implement Enemy Attack Animations;
2. implement the Professional Opening Cinematic;
3. complete retained verification;
4. resume the architecture audit.
<!-- B&D V23R19S QA INTERRUPTION END -->

<!-- B&D V23R19T QA INTERRUPTION START -->
## Current interruption — V23R19T

The architecture audit remains paused.

The active blocker is historical-phase coupling inside legacy QA, not a Runtime or gameplay failure.

After TEST EVERYTHING reaches 0 blockers, 0 warnings and 0 info:

1. Enemy Attack Animations;
2. Professional Opening Cinematic;
3. retained implementation verification;
4. architecture audit.
<!-- B&D V23R19T QA INTERRUPTION END -->
