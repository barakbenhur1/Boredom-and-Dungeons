# Boredom & Dungeons — Authoritative Project Status and Ordered Work

## Current development snapshot

```text
Status date: 2026-06-07
Classification: EARLIER / BLOCKING REGRESSION
Active work: C03/C11/C12.RUNTIME.V20
Latest automated QA: TEST EVERYTHING PASS at 2026-06-07T03:09:04.4634630Z — blockers=0, warnings=0, info=0.
Runtime truth: automated PASS does not close the work. Play Mode/Console still showed duplicate TrailRenderer creation, BDChargedProjectileVisual NullReferenceException, repeated missing AudioListener warnings, mounted-cinematic/input/turn defects, first-B timing failure, and closed-wall camera visibility leakage.
Saved feature resume point after V20 passes: C03.23A -> C07.16A -> C07.16 -> C07.17.
Later infrastructure retained without interrupting V20: C12.42 explicit AudioMixer routing for Master, Music, SFX, and Ambience.
```

This file is the only live source for current status, ordering, blockers, verification truth, and the resume point. Exact durable feature behavior belongs in the maintained files under `Assets/_Project/Design/`, as mapped by `DOCUMENTATION_INDEX.md`. Git history stores previous status versions; obsolete package narratives and duplicate roadmaps are not maintained as live documentation.

## Status labels

- **DONE** — implemented, integrated, Play Mode verified, documented, and committed.
- **VERIFY** — implementation exists but the latest applicable Unity/Play Mode gate is incomplete.
- **IN PROGRESS** — implementation is actively incomplete.
- **DESIGN COMPLETE** — behavior is approved but implementation remains.
- **PLANNED** — ordered future work.
- **RECOVERY REQUIRED** — requirement is known but evidence/details must be recovered.
- **BLOCKED** — later work cannot continue until the blocker is resolved.

## Permanent working rules

1. Every material user request is recorded here before implementation or in the same change.
2. Classify every request as earlier/blocking, current, later, or recovery-required.
3. Earlier regressions stop later feature work; preserve the resume point, repair, verify, then return.
4. Update affected design, architecture, QA, technical-decision, and performance documents with the same truth.
5. Maintained Git documentation always reflects the current state. Merge valid content before deleting a superseded document.
6. Git history stores old versions. Do not retain stale roadmaps, package reports, copied status files, temporary patch notes, or chat exports as live documentation.
7. Do not claim Unity compilation, Play Mode, performance, or QA success unless it actually ran.
8. Do not replace a complete current system file with an older package version. Repair from the actual local state.
9. Repository hygiene runs on every handoff and before every commit.

# Active blocking work — C03/C11/C12.RUNTIME.V20

## V20.1 Charged-shot Runtime stability — BLOCKING

- `BDChargedProjectileVisual.Build` must reuse `GetComponent<TrailRenderer>()` when one already exists.
- Repeated `Attach`, `Configure`, and `Build` calls must be idempotent.
- Never continue with a null trail reference.
- Repeated charged-shot x3 use must produce no duplicate-component error and no `NullReferenceException`.

## V20.2 AudioListener continuity — BLOCKING

- Exactly one active `AudioListener` exists before, during, and after the mounted cinematic.
- Keep the Main Camera GameObject active; disable only the follow driver while cinematic camera ownership is active.
- Do not create a second active listener.
- No missing-listener or multiple-listener warning is accepted.

## V20.3 Mounted entrance cinematic — BLOCKING

Authoritative behavior: `Assets/_Project/Design/Runtime/MOUNTED_RUN_INTRO_AND_DOORWAY_PORTALS.md`.

- Camera is inside the entrance room, not rider POV.
- Resolve the real entrance plane, room bounds, opposite rear wall, and doorway center.
- Position the camera farther and higher than the failed shot, about 30% of room depth measured from the opposite rear wall toward the entrance, with safe wall inset.
- Aim at the doorway center so horse and rider are seen entering from outside.
- Lock every gameplay input through the whole sequence, including mouse aim/facing, steering, movement, attacks, charged shot, dodge, jump, mount/dismount, Pet, interactions, and buffered actions.
- Horse enters straight, turns about 90 degrees right, completes the turn, fully stops, and holds 0.15–0.30 seconds.
- Restore `BDCameraFollow`, projection/FOV, and gameplay input only after the stop hold.
- Discard held/buffered input before release.

## V20.4 BBH first-letter timing — BLOCKING

Authoritative behavior: `Assets/_Project/Design/UI/BBH_BOOT_INTRO_V1.md`.

- First rendered frame is fully black.
- No first `B`, shadow, depth copy, highlight, or rim is visible during pre-roll.
- First `B` starts after a positive delay and animates from zero scale/opacity.
- Second `B` and `H` remain sequential.
- Filled graphite/steel circle grows from zero behind completed `BBH`, holds exactly 0.50 seconds, then fades.

## V20.5 Closed-wall camera visibility — BLOCKING

Authoritative behavior: `Assets/_Project/Design/Map/ROOM_BOUNDARY_CAMERA_AND_TEXTURE_READINESS.md`.

- Closed walls are visibility boundaries, not only movement colliders.
- Clamp camera position and look point to the current room every frame.
- Prevent leakage beside walls, at corners, diagonally, during fast rotation, and while mounted.
- Push the camera inward rather than allowing a view into an adjacent room.
- Legal authored openings remain the only visibility/traversal path.

## V20 acceptance gate

1. Unity compiles with no project errors.
2. `Boredom And Dungeons -> TEST EVERYTHING` passes.
3. Focused Play Mode verifies every V20 item repeatedly.
4. Console contains no duplicate TrailRenderer error, charged-projectile null exception, or AudioListener warning.
5. Real results are recorded here.
6. Only then resume at C03.23A.

# Ordered project categories

## C00 — Governance and requirement recovery — CURRENT FOUNDATION

- Keep this file as the only live status/order source.
- Keep `START_HERE.md`, `DEVELOPMENT_WORKFLOW.md`, and `DOCUMENTATION_INDEX.md` synchronized.
- Recover unknown requirements rather than inventing them.
- Remove obsolete documentation after valid content is merged.

## C01 — Stability, QA, validation, and repository health — IN PROGRESS

- Maintain one QA entry point: `Boredom And Dungeons -> TEST EVERYTHING`.
- Integrate new automated checks into the existing QA window.
- Keep conflict detection line-aware and implementation-contract checks current.
- Add Runtime regression coverage for charged-shot idempotency and AudioListener count.
- Enforce repository-hygiene checks before handoff/commit.

## C02 — Platform, input, architecture, scene assembly, and data — IN PROGRESS

- Unity 6000.0.76f1, C#, desktop development controls, final mobile-landscape target.
- Preserve runtime/editor separation and stable Unity GUID/meta files.
- Continue migration toward reusable prefabs/ScriptableObjects and deterministic scene assembly.
- Preserve scalable mobile input architecture while current development uses keyboard/mouse.

## C03 — Player movement, combat, damage, and weapons — IN PROGRESS

- Complete V20 blockers first.
- Saved resume: C03.23A safe-position and environmental recovery work.
- Continue movement/aiming polish, melee/ranged distinction, ammo/reload, light/heavy/parry/dodge, spinning AOE, charged shot, damage feedback, and lifecycle rules.
- All attacks must remain readable, avoidable, and mobile-compatible.

## C04 — Horse traversal, mounted combat, damage, healing, and flee behavior — IN PROGRESS

- Mounted combat and traversal remain core systems.
- Preserve true hit-target routing: player or horse receives damage based on the collider actually struck.
- Mounted rider/horse hits contribute to the approved buck counter without automatic double damage.
- Finish hazard avoidance/recovery, deterministic healthy start, exhausted follow, contextual Pet, local threat retreat, and death-restart grounding.

## C05 — Normal enemies and encounter behavior — IN PROGRESS

- Maintain sword, patrol/guard, charger/rammer, bomb/trap, ranged, and tactical/exit-interference roles.
- Enemies react to escape intent without unfair unavoidable blocking.
- Spawn, awareness, telegraph, attack, and navigation behavior must remain reproducible and safe.

## C06 — Inventory, hidden collectibles, guardians, rewards, and run boosts — PLANNED/FOUNDATIONS

- Hidden Game Boy, Batteries, and Cartridge remain secret and are never advertised by UI/objectives.
- Guardians, physical rewards, chests, ammo, and temporary run boosts require deterministic legal placement and complete reset behavior.

## C07 — Boss framework and deterministic encounter planning — SAVED RESUME

Resume after V20 and C03.23A:

1. C07.16A shared-framework prerequisite completion.
2. C07.16 playable encounter integration.
3. C07.17 deterministic selection/role/legal-placement continuation.

Framework must support shared health/resource/state contracts, non-combat phases, custom defeat routing, secret activation, and reproducible test harnesses.

## C08 — Individual mini-bosses — PLANNED

- Square Jumper.
- Roller.
- Serpent.
- Quad Gunners.
- Select three of four per run through deterministic legal placement; one remains absent.
- Each encounter requires readable attacks, arena legality, rewards, cleanup, and immediate near-spawn testing.

## C09 — Final and narrative bosses — DESIGN/PLANNED

- Preserve approved black/white final boss behavior.
- Implement Mother Boss exactly from the maintained boss specification, including four phases, separate durability/resource rules, Father interaction, attraction/spiral, clothing occlusion fairness, phase-specific Dodge budgets, non-combat Danger phase, defeat restart, and victory ending.
- Do not simplify or silently rewrite approved boss mechanics.

## C10 — Map generation, level design, hazards, and pacing — IN PROGRESS/PLANNED

- Multi-route maps target at least three meaningful route families and four when space permits.
- Inaccessible macro-regions may occupy roughly one to four rooms and use mountains, rocks/boulder fields, lakes, chasms, pillars, and natural geometry without becoming accessible rooms.
- Keep biome-authentic boundaries, legal doorways, destructible blockers, room hazards, safe spawn/recovery, encounter spacing, and seed validation.

## C11 — Camera, minimap, HUD, UI, readability, and accessibility — BLOCKED BY V20

- Fix closed-wall visibility before continuing.
- Preserve rigid clipped minimap rotation, fog of war, player/horse markers, and no overflow.
- Continue HUD, boss health, ammo/reload, settings, pause/result flow, accessibility, mobile readability, and safe-area work.

## C12 — Art, animation, VFX, lighting, atmosphere, and audio — IN PROGRESS/PLANNED

- Continue model/animation replacement, readable telegraphs, lighting, atmosphere, VFX, shadows, and audio production.
- C12.42 later: replace heuristic loop/name-based routing with explicit AudioMixer groups for Master, Music, SFX, and Ambience while preserving dynamic fades and per-category controls.
- Do not jump to C12.42 until V20 closes.

## C13 — Story, endings, cinematics, and result logic — PLANNED/FOUNDATIONS

- Preserve standard incomplete-set endings and secret complete-set continuation.
- Complete Game Boy/Battery/Cartridge branches, partial Mother reveal, Mother loss restart, Mother victory ending, final camera/lighting/VFX/audio, and state isolation between runs.

## C14 — Balance, performance, persistence, cleanup, and release — PLANNED

- Balance enemies, guardians, mini-bosses, final bosses, Mother phases, hazards, ammo, boosts, dodge/parry, and horse survivability.
- Add pooling, profiling, memory/GC, frame-time, loading, stress, and mobile-input performance gates.
- Define save/version/reset behavior.
- Remove obsolete generated versions and repair artifacts continuously.
- Final gate: clean compiler/Console, complete run, all ending branches, target mobile build, external clean-clone verification, release notes, version, and tag.

# Exact current sequence

1. V20 charged-projectile Runtime repair.
2. V20 exactly-one AudioListener repair.
3. V20 mounted camera/input/right-turn/full-stop repair.
4. V20 BBH first-frame repair.
5. V20 closed-wall camera visibility repair.
6. Unity compile, TEST EVERYTHING, focused Play Mode, and Console verification.
7. Record results here.
8. Resume C03.23A.
9. Continue C07.16A, C07.16, and C07.17.
10. Keep C12.42 ordered later.

# Current blockers and risks

## Blocking now

- Duplicate `TrailRenderer` creation and `BDChargedProjectileVisual` null failure.
- Missing active `AudioListener` warnings.
- Mounted cinematic camera/input/turn/stop sequence incomplete.
- First `B` appears before its entrance animation.
- Camera can reveal adjacent rooms across closed walls.

## Active risks

- Old package files can erase current contracts when copied wholesale.
- Static token QA can pass while visible Runtime behavior is wrong.
- Camera ownership can interrupt audio if the camera GameObject is disabled.
- Buffered input can leak through the cinematic.
- Stale documents can mislead future contributors.

# Current changelog

## 2026-06-07 — Current-state documentation consolidation

- Recorded V20 as the active earlier/blocking work.
- Recorded the real automated PASS and the remaining Runtime/Play Mode blockers.
- Updated mounted-intro, BBH-intro, room-boundary, QA, workflow, technical-decision, and documentation-index contracts directly in Git.
- Removed historical package-by-package status prose from the live status model; Git history remains the archive.
- Made current-only documentation and repository hygiene permanent.
- Preserved the exact resume point: C03.23A -> C07.16A -> C07.16 -> C07.17.
