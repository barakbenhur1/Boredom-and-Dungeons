# Boredom & Dungeons — Authoritative Categorized Development Plan

<!-- B&D CURRENT SNAPSHOT START -->
## Current Development Snapshot

```text
Status date: 2026-06-07
Engine: Unity 6000.0.76f1
Previous package result: Repair V6 copied its payload, then stopped before core-file patching because the minimap preflight did not recognize the actual local partial state.
Current repair: cumulative V7 continues from that partial state without reset; completes run presentation, authored entrance/exit flow, existing Pause/menu polish, rigid minimap clipping, tall room walls, closed-wall camera stop, asymmetric-texture readiness, and the BBH filled-circle completion.
Current status: IMPLEMENTED LOCALLY / VERIFY
Required next action: install V7, compile, run TEST EVERYTHING, then complete the focused run-flow, minimap, wall/camera, and BBH intro Play Mode matrix.
Acceptance: no pre-menu map flash; fresh/abandoned/post-cinematic mounted intro; ordinary death restart on foot; authored exit continuation; one Pause owner; rigid clipped minimap; adjacent rooms invisible across closed walls; camera stops at closed walls; BBH filled circle grows behind the letters and holds 0.50s.
Later requirements recorded without interrupting current work: C10 inaccessible 1–4-room macro-regions already exist and now explicitly include rocks/boulder fields; Mother Dodge receives exact phase budgets and scheduler/fairness rules for later C09 implementation.
Saved resume point after PASS: C03.23A, then C07.16A, C07.16, and C07.17.
Do not commit until automated QA and the focused Play Mode matrix pass.
```
<!-- B&D CURRENT SNAPSHOT END -->

<!-- B&D QA CONTRACT DRIFT V9 SNAPSHOT START -->
### Earlier/blocking QA reliability repair — 2026-06-07

- ID: `C01.QA-CONTRACT-DRIFT.V9`.
- Real Unity result before repair: `TEST EVERYTHING` BLOCKED with 4 blockers and 0 warnings.
- Two blockers were false conflict reports caused by substring scanning of inline documentation/source examples.
- The other two blockers were stale QA anchors: the current V7 rigid minimap and action-aware START GAME implementations were present, but older checks required superseded identifiers.
- Repair rule: fix QA truth rather than reintroduce obsolete runtime code solely to satisfy stale string checks.
- Status: IMPLEMENTED LOCALLY / EXTERNAL VALIDATOR REQUIRED / UNITY TEST EVERYTHING RERUN REQUIRED.
- Resume point: after automated QA passes and focused Play Mode is rechecked, return to the previously recorded current gameplay item; no feature ordering changes.
<!-- B&D QA CONTRACT DRIFT V9 SNAPSHOT END -->

<!-- B&D DOC GOVERNANCE V8 SNAPSHOT START -->
### Cross-cutting documentation/QA repair — 2026-06-07

- ID: `C00.DOC-GOVERNANCE.V8`.
- Every user request is recorded before or with implementation in its correct `PROJECT_STATUS.md` location.
- Every request is classified as earlier/blocking, current, later, or recovery-required; the saved resume point is preserved whenever work is interrupted.
- The standing rule is permanent and does not need to be repeated by the user.
- Added mandatory first-read, documentation index, architecture/flow diagrams, QA, technical-decision, and performance guidance documents.
- `BDOneClickQAWindow.cs` conflict markers are an earlier QA blocker. The repair restores only that file from `HEAD` when conflicted, preserves every other local change, and reconnects all discovered modular `BD*QA.Scan` checks.
- Status: IMPLEMENTED LOCALLY / EXTERNAL VALIDATOR REQUIRED / UNITY COMPILE AND TEST EVERYTHING STILL REQUIRED.
- Resume rule: after this blocker passes, return to the exact pre-existing resume point already recorded above; this repair does not reorder gameplay work.
<!-- B&D DOC GOVERNANCE V8 SNAPSHOT END -->

<!-- B&D PET-INTRO-QA-CONTRACT-REPAIR START -->
## Current QA repair — mounted intro Pet contract

```text
Latest TEST EVERYTHING: BLOCKED at 2026-06-06T21:47:46.9476910Z
Blocker: MOUNTED_RUN_INTRO_CONTRACT_MISSING
Asset: Assets/_Project/Scripts/Runtime/Horse/BDHorseExhaustedFollowAndPetInteraction.cs
Reported missing token: PET_RUN_INTRO_CANCEL_CONTRACT
Root cause: BDMountedRunIntroQA required an internal package marker as if it were a real runtime contract
Repair: remove the synthetic marker requirement and keep validation of the real behavior contracts only
Current status: C03 PET INTRO QA CONTRACT REPAIR V4 IMPLEMENTED LOCALLY / VERIFY
Next action: return to Unity and run TEST EVERYTHING again
```
<!-- B&D PET-INTRO-QA-CONTRACT-REPAIR END -->

<!-- B&D PET-INTRO-COMPILE-REPAIR START -->
## Current compile repair — mounted intro Pet cancellation

```text
Latest TEST EVERYTHING: BLOCKED at 2026-06-06T21:38:18.5757470Z
Compiler error: Assets/_Project/Scripts/Runtime/Horse/BDHorseExhaustedFollowAndPetInteraction.cs(169,17) CS0103
Root cause: the mounted-intro input gate called an undefined CancelInteraction method
Repair: use CancelPetInteractionForRunIntro, clear pending Pet hold state, and request the existing Pet coroutine's safe emergency-cancel cleanup only when an interaction is active
Current status: C03 PET INTRO COMPILE REPAIR V3 IMPLEMENTED LOCALLY / VERIFY
Next action: allow Unity to compile, copy any remaining Console compiler errors exactly, then run TEST EVERYTHING
```
<!-- B&D PET-INTRO-COMPILE-REPAIR END -->


> This is the single authoritative living development plan and status tracker.
> It must be updated whenever code, requirements, QA results, priorities, blockers, category status, or implementation order changes.

## 1. Current-stage rule

The marked `Current Development Snapshot` block at the top of this file is the only current Previous / Current / Next pointer. The ordered checklist below remains the complete requirement list. No second snapshot or parallel status document is allowed.

## 2. Status labels

- **DONE** — implemented, integrated into the authoritative scene/prefab flow, verified in Play Mode, edge cases checked, committed, and documented.
- **PROTOTYPE DONE** — functional prototype exists, but production integration, art, balance, portability, or polish remains.
- **DESIGN COMPLETE** — behavior and acceptance rules are approved, but implementation is incomplete.
- **IN PROGRESS** — meaningful implementation exists, but integration or verification remains.
- **VERIFY** — code exists, but the latest relevant build/Play Mode gate has not been rerun.
- **PLANNED** — requirement is recorded and ordered, but implementation has not started.
- **RECOVERY REQUIRED** — the requirement is known to exist, but details were lost and must be recovered before implementation.
- **BLOCKED** — work cannot proceed until the documented blocker is resolved.

A task is never marked **DONE** merely because a script, class, design document, or generated scene exists.

## 3. Mandatory working protocol

### 3.1 One authoritative list

This file is the source of truth for:

- what was requested;
- what was implemented;
- what was verified;
- what remains;
- the exact current category and item;
- blockers and risks;
- the next action;
- the change history.

The old numeric roadmap remains historical reference only. When it conflicts with this file, this file wins.

### 3.2 Every change updates Git

Every material code or requirement change must update this file in the same commit, or in the immediately following documentation commit.

At minimum, update:

1. status date;
2. current category and current item;
3. the affected checklist entries;
4. QA truth;
5. blockers and risks;
6. exact resume point;
7. changelog.

Requirements must not be silently deleted. Rejected or replaced requirements stay documented with a reason.

### 3.3 Earlier/current/later change rule

When a new request arrives, first place it in the correct category and dependency position.

- **Earlier than the current item:** pause current work, implement/fix the earlier item immediately, verify it, update this file, then return to the saved resume point.
- **Inside the current category/item:** update the current checklist and implement it in dependency order.
- **Later than the current item:** add it to the correct future category without abandoning the current item.
- **Cross-cutting regression:** treat it as an earlier blocking task and resolve it before feature work continues.
- **Unknown/lost requirement:** record it as **RECOVERY REQUIRED** rather than inventing details.

### 3.4 Category transition gate

Before starting a new category:

1. finish the current category acceptance gate;
2. update this file and Git;
3. announce which category was completed;
4. announce the proposed next category;
5. ask the user whether there are corrections, additions, removals, or priority changes;
6. record the user’s response in this file;
7. only then start the new category.

This pause is mandatory even when the next category appears obvious.

### 3.5 Verification gate

A category cannot be completed until applicable checks pass:

- clean Unity compilation;
- zero release-blocking Console errors;
- authoritative scene generation;
- Play Mode verification;
- edge-case verification;
- automated project validation;
- Git diff check and repository hygiene;
- status document update.

<!-- B&D PACKAGE AND TEST-HARNESS PROTOCOL START -->
### 3.6 Mandatory ZIP and Git handoff

Every material change is delivered as a downloadable ZIP. Prefer the complete
updated project; when it is too large, include every changed directory and all
required source, asset, scene, `.meta`, QA, design, and status files.

The package must include an idempotent installer, validator, README, manifest,
preflight checks, and no automatic commit/push. After Unity verification, provide
the exact `git add`, staged diff checks, commit, pull-rebase, and push commands.

The assistant never commits directly. Failed packages are repaired from the real
partial local state without discarding unrelated local changes.

The full process contract is `DEVELOPMENT_WORKFLOW.md`.

### 3.7 Near-spawn test harness and cleanup

Every newly added testable boss, mini-boss, enemy, obstacle, hazard, reward,
transition, attack, interaction, or system must be immediately reachable through
a clearly identified test room, lane, arena, or portal close to player spawn.

The user must not search a generated map to locate new test content.

Temporary lava, holes, enemies, barriers, or props must never threaten the normal
player/horse spawn. Mini-boss test encounters remain escapable and may not hard-lock
the room.

After user acceptance, every temporary object is removed, deliberately retained
as an isolated documented development harness, or converted to production.
Obsolete placeholders and test clutter must be cleaned continuously rather than
deferred to the end of the project.
<!-- B&D PACKAGE AND TEST-HARNESS PROTOCOL END -->

## 4. Permanent product rules

- Working title: **Boredom & Dungeons**.
- Game language: **English**.
- Game style: top-down / angled 2.5D action-adventure.
- Final product target: **mobile landscape**.
- Current development harness: desktop / Unity Editor with keyboard and mouse.
- Planned mobile control direction:
  - finger trace / drag for movement;
  - tap an enemy to attack that target;
  - mobile behavior must preserve the intended melee/ranged distinction and readable aiming.
- The game must feel like a designed level, not an endless square maze.
- Horse traversal and mounted combat are core systems.
- Hidden collectibles must remain genuinely hidden.
- Never add objective markers, empty collectible slots, missing-item text, `0/4`, or instructions that advertise secrets.
- Difficult attacks must be readable and avoidable.
- Runtime code must not depend on `UnityEditor`.
- Unity `.meta` files must remain committed and stable.
- Do not commit `Library`, `Temp`, `Obj`, `Logs`, local builds, ZIP packages, or generated IDE files.

---

# C00 — Governance, documentation, and requirement recovery

**Category status: IN PROGRESS — recurring**
**Purpose:** prevent loss of decisions and keep one dependable development history.

## Ordered work

- [x] C00.01 Create a root-level persistent project status file.
- [x] C00.02 Define objective status labels.
- [x] C00.03 Record the current category and exact resume point.
- [x] C00.04 Add mandatory earlier/current/later request handling.
- [x] C00.05 Add the mandatory category-transition pause and user review.
- [x] C00.06 Preserve completed, pending, blocked, and superseded requirements.
- [x] C00.07 Add a permanent changelog.
- [x] C00.08 Reorganize all known work into dependency-ordered categories.
- [ ] C00.09 Update this file with every future code or requirement change.
- [ ] C00.10 Keep README status synchronized with this file.
- [x] C00.11 Mark the old V128 numeric roadmap as historical/reference-only.
- [ ] C00.12 Recover lost requirements from damaged chat/export evidence when possible.
- [ ] C00.13 Record unrecoverable requirements explicitly instead of guessing.
- [x] C00.14 Establish `/PROJECT_STATUS.md` as the only authoritative complete progress list.
- [x] C00.15 Put Previous / Current / Next directly in `/PROJECT_STATUS.md`; do not maintain a competing `WORKING_NOW.md` source of truth.
- [ ] C00.16 Update `/PROJECT_STATUS.md` in every material commit that changes code, requirements, QA truth, priorities, blockers, or implementation order.
- [ ] C00.17 `TEST EVERYTHING` must block a material pending change when `/PROJECT_STATUS.md` is not updated in the same working tree/staged change set.
- [ ] C00.18 Remove and prevent versioned status snapshots such as `PROJECT_STATUS_CURRENT_V*.md`; historical detail stays in Git history, not duplicate live files.
- [x] C00.19 Add `/DEVELOPMENT_WORKFLOW.md` as the authoritative working-method contract while keeping `/PROJECT_STATUS.md` as the only product-status and requirement source of truth.
- [x] C00.20 Require every material assistant-delivered change to arrive as a validated idempotent ZIP package, followed only after user verification by exact local Git commit/pull/push commands; the assistant does not commit directly.
- [x] C00.21 Require a clearly identified near-spawn test room, lane, arena, or portal for every new boss, mini-boss, enemy, obstacle, hazard, reward, transition, attack, interaction, or system that otherwise requires map searching.
- [ ] C00.22 After each accepted feature, remove temporary test objects or explicitly classify them as retained development harnesses or converted production content; do not accumulate placeholder/test cleanup debt.
- [ ] C00.23 Keep every material package synchronized across code, scenes/prefabs, `.meta` files, QA contracts, relevant design documents, `PROJECT_STATUS.md`, and stable README/workflow discovery.

<!-- B&D DOC GOVERNANCE V8 ITEMS START -->
- [ ] **C00.24 `C00.DOC-GOVERNANCE.V8` — Permanent request capture:** every user request is recorded in the correct logical/dependency position before or with implementation; the user never needs to repeat this rule. — IMPLEMENTED LOCALLY / VERIFY.
- [ ] **C00.25 Request timing classification:** classify every request as earlier/blocking, current, later, or unknown/recovery-required; preserve and return to the saved resume point. — IMPLEMENTED LOCALLY / VERIFY.
- [ ] **C00.26 Mandatory first-read discovery:** maintain `START_HERE.md` and a prominent README link so a new human/AI contributor cannot miss the required reading order. — IMPLEMENTED LOCALLY / VERIFY.
- [ ] **C00.27 Maintained documentation map:** maintain `DOCUMENTATION_INDEX.md` with purpose, ownership, and update triggers for every durable document. — IMPLEMENTED LOCALLY / VERIFY.
- [ ] **C00.28 Architecture and flow truth:** maintain `ARCHITECTURE.md` and its diagrams whenever system ownership, dependencies, scene/run flow, or integration boundaries change. — IMPLEMENTED LOCALLY / VERIFY.
- [ ] **C00.29 QA, decisions, and performance truth:** maintain `QA_CHECKLIST.md`, `TECHNICAL_DECISIONS.md`, and `PERFORMANCE_GUIDELINES.md` whenever their contracts change. — IMPLEMENTED LOCALLY / VERIFY.
- [ ] **C00.30 Critical conflict-marker guard:** unresolved source/scene/documentation conflict markers are immediate blockers and must be detected before later work continues. — IMPLEMENTED LOCALLY / VERIFY.
<!-- B&D DOC GOVERNANCE V8 ITEMS END -->


## Recovered requirement

- [x] **Mother Boss design recovered and re-approved from the user’s reconstruction.**
  - It is a secret post-ending boss unlocked only when the player reaches the ending with the Game Boy, two Batteries, and the Game Cartridge.
  - It does not replace the existing black/white final boss.
  - The complete recovered design is recorded in C09B and in `Assets/_Project/Design/Bosses/MOTHER_BOSS_DESIGN_V1.md`.
  - Remaining unknown details must be resolved during implementation without changing the approved structure.

## Category acceptance

- `/PROJECT_STATUS.md` is the one complete source of truth.
- README and roadmap documentation point directly to it.
- Historical roadmaps cannot be mistaken for current status.
- No known requirement is left only in chat.

---

# C01 — Project stability, QA, validation, and repository health

**Category status: DONE**
**Purpose:** keep the project compilable and prevent feature work from building on a broken baseline.

## Completed foundations

- [x] C01.01 QA baseline workflow.
- [x] C01.02 Full project stability gate foundation.
- [x] C01.03 Current-scene validation foundation.
- [x] C01.04 Play Mode smoke checklist tool.
- [x] C01.05 Generated prototype scene validation.
- [x] C01.06 Repository/meta/runtime-source validation foundations.
- [x] C01.07 Latest automated QA result before the current classifier fix:
  - Unity `6000.0.76f1`;
  - automated status `PASS`;
  - blockers `0`;
  - warnings `1`;
  - info `0`;
  - generated UTC `2026-06-06T03:42:01.6743540Z`;
  - warning code: `MULTIPLE_RUNTIME_INSTALLERS`;
  - finding: `79` active `BDEnemyBootstrap` components distributed across enemy objects;
  - resolution: classify `BDEnemyBootstrap` as an intentional per-entity setup component while continuing to detect duplicate scene-level installers and other Bootstrap types.
- [x] C01.08 Remove obsolete camera/minimap serialized fields from code.
- [x] C01.09 Add instructions to rerun `TEST EVERYTHING` after the cleanup.
- [x] C01.23 Integrate Camera/Minimap regression checks into the existing `TEST EVERYTHING` command; no second QA menu action is required.
- [x] C01.24 Integrate repository-hygiene checks into `TEST EVERYTHING` and extend `.gitignore` for generated packages, reports, one-shot patch tools, and duplicate status snapshots.
- [x] C01.25 Repair the invalid backslash character literals introduced by the unified-QA patch and restore clean Unity compilation.
- [x] C01.26 Correct the generic runtime-installer classifier so `BDEnemyBootstrap` may appear once per enemy without producing a false duplicate-installer warning.
- [ ] C01.27 Rerun `TEST EVERYTHING` after C01.26 and confirm `0` blockers and `0` warnings.

<!-- B&D QA CONTRACT DRIFT V9 ITEMS START -->
## Cross-cutting QA contract reliability

- [ ] **C01.QA-CONFLICT-SCAN.V9:** detect only real standalone Git conflict-marker lines; inline examples and quoted strings must not block QA. — IMPLEMENTED LOCALLY / VERIFY.
- [ ] **C01.MINIMAP-QA-DRIFT.V9:** validate the active V7 rigid clipped minimap contract instead of the superseded V1 marker. — IMPLEMENTED LOCALLY / VERIFY.
- [ ] **C01.MENU-QA-DRIFT.V9:** validate the action-aware START GAME highlight path instead of the removed `startGamePressed` local variable. — IMPLEMENTED LOCALLY / VERIFY.
- [ ] **C01.QA-CONTRACT-DRIFT.V9:** rerun Unity compilation and `TEST EVERYTHING`; record the real result before resuming feature work. — VERIFY.
<!-- B&D QA CONTRACT DRIFT V9 ITEMS END -->

## Immediate prerequisite gate

- [x] **C01.10 Pull/open latest `main` in Unity 6000.0.76f1.**
- [x] C01.11 Wait for compilation and confirm zero compiler errors.
- [x] C01.12 Run `Boredom And Dungeons -> TEST EVERYTHING`.
- [x] C01.13 Rebuild `Assets/_Project/Scenes/02_CleanCore_MazePrototype.unity` if required.
- [x] C01.14 Run the complete Play Mode smoke checklist:
  - movement and collision;
  - jump and landing;
  - dodge and i-frames;
  - light/heavy/buffered melee;
  - landing attack;
  - physical parry;
  - tap shot and charged shot;
  - automatic reload;
  - horse mount/dismount/damage/heal/buck/flee;
  - damage/death/reset;
  - chasm/hole damage and safe respawn;
  - lava damage and return to the latest safe non-lava point;
  - mounted player-and-horse hazard recovery;
  - horse avoidance of chasms and lava;
  - dynamic minimap;
  - Console cleanliness.
- [x] C01.15 Save the latest automated and manual PASS reports.
- [x] C01.16 Fix any regression, update this plan, then continue with C03.46–C03.55, C04.24–C04.31, and then resume at C07.16.

## Current one-button QA and repository-hygiene work

- [x] C01.16A Existing `TEST EVERYTHING` automated run passed on Unity `6000.0.76f1` at `2026-06-06T03:22:56.8937980Z` with 0 blockers, 0 warnings, and 0 info.
- [x] **C01.16B Integrate Camera/Minimap regression checks into the existing `Boredom And Dungeons -> TEST EVERYTHING` action.**
- [x] C01.16C Integrate tracked-file repository hygiene into the same `TEST EVERYTHING` action; no second QA menu item may be required.
- [x] C01.16D Verify the five obsolete Camera/Minimap fields cannot return:
  - `rotationSpeedDegreesPerSecond`;
  - `snapToMovementCardinals`;
  - `mapRotationInitialized`;
  - `minimumMovementDirectionMagnitude`;
  - `rotateOnlyWhenActuallyMoving`.
- [x] C01.16E Validate required Camera/Minimap behavior anchors remain present.
- [x] C01.16F Reject tracked Unity-generated folders, archives, QA exports, backups, IDE outputs, one-shot patch scripts, duplicate status files, and other root clutter.
- [x] C01.16G Expand `.gitignore` for all generated/local/package/QA/status-snapshot artifacts while preserving real Unity `.meta` files.
- [x] C01.16H Enforce `/PROJECT_STATUS.md` update alongside every material pending change.
- [ ] C01.16I Run only `TEST EVERYTHING`, complete its one Play Mode checklist, save FINAL QA PASS, and record the result here. — AUTOMATED PASS 0/0/0 AT 2026-06-06T17:00:51.3087910Z; FULL MANUAL FINAL PASS REMAINS.


## Latest completed unified QA result

- Generated UTC: `2026-06-06T03:51:35.3691270Z`.
- Unity: `6000.0.76f1`.
- Automated status: `PASS`.
- Blockers: `0`.
- Warnings: `0`.
- Info: `0`.
- Camera/Minimap regression checks are included in `TEST EVERYTHING`.
- Repository hygiene checks are included in `TEST EVERYTHING`.
- No separate QA menu command is required.

## Later production QA

- [ ] C01.17 Add deterministic multi-seed test coverage.
- [ ] C01.18 Add boost drop-rate statistical tests.
- [ ] C01.19 Add boss state-transition tests.
- [ ] C01.20 Add build/version metadata validation.
- [ ] C01.21 Add external clean-clone/open/build verification.
- [ ] C01.22 Preserve zero release-blocking errors through final build.

## Current blocker truth

The latest supplied `TEST EVERYTHING` report was generated on `2026-06-06T21:38:18.5757470Z` in Unity `6000.0.76f1` and is **BLOCKED** with `1` blocker: `UNITY_SCRIPT_COMPILATION_FAILED`. The report contains no compiler file, line, or C# error text, so the Unity Console compiler lines remain required before the current mounted-run-intro work can be accepted. The projectile/AoE and destructible-obstacle changes in this package are documentation-only future requirements and do not bypass the active compilation gate.

---

# C02 — Platform, input, architecture, scene assembly, and data

**Category status: IN PROGRESS**

## Product/platform foundation

- [x] C02.01 Unity/C# project foundation.
- [x] C02.02 Desktop Unity Editor prototype controls.
- [ ] C02.03 Define the final supported mobile device/orientation matrix.
- [ ] C02.04 Implement landscape-safe layout and safe-area handling.
- [ ] C02.05 Implement finger-trace/drag movement.
- [ ] C02.06 Implement tap-enemy target selection and attack.
- [ ] C02.07 Define touch rules for empty-space taps, target loss, overlapping enemies, and UI interception.
- [ ] C02.08 Preserve keyboard/mouse as a development/debug input path.
- [ ] C02.09 Add input abstraction so gameplay logic is not coupled to one platform.
- [ ] C02.09A Build a central rebindable desktop action registry as a later input/settings task: persistent bindings, conflict detection/confirmation, reserved-key rejection, reset defaults, and one display-string API used by all gameplay prompts. This request does not interrupt the current C03/C04 verification work.

## Architecture and assembly

- [x] C02.10 Runtime/editor folder separation.
- [x] C02.11 Clean maze scene-builder foundation.
- [x] C02.12 Scene-builder decomposition preparation.
- [x] C02.13 Authoritative generated prototype scene path.
- [ ] C02.14 Replace fragile runtime repair/install bootstraps with explicit serialized setup where appropriate.
- [ ] C02.15 Remove duplicate/overlapping installers and components.
- [ ] C02.16 Convert reusable configuration to prefabs and ScriptableObjects where appropriate.
- [ ] C02.17 Define stable gameplay service/event boundaries.
- [ ] C02.18 Define deterministic run seed ownership and storage.
- [ ] C02.19 Separate run state, persistent state, presentation state, and editor generation state.
- [ ] C02.20 Ensure scene generation is idempotent and does not create duplicate systems.
- [ ] C02.21 Preserve stable Unity GUID and `.meta` references.
- [ ] C02.22 Remove obsolete generated versions and local repair artifacts.
- [ ] C02.23 Ensure no runtime source imports or calls `UnityEditor`.

## Category acceptance

- Desktop and touch input use shared gameplay commands.
- The authoritative scene is generated without duplicate systems.
- Core configuration is explicit and serializable.
- No repair script is required for normal gameplay startup.

---

<!-- B&D BIOME-WALLS-AND-PORTALS START -->
### C02.BIOME — Biome-authentic world boundaries and doorway presentation

- [ ] **C02.BIOME.1 Replace generic, flat, artificial-looking room/corridor walls with natural or context-appropriate boundary forms for the active biome.**
- [ ] Forest/overgrown biomes may use trees, roots, thick vegetation, earthen banks, mossy stones, wooden fences, and fallen trunks.
- [ ] Mountain/highland biomes may use boulders, rock shelves, cliff faces, ridges, steep slopes, and distant mountain massing.
- [ ] Cave/underground biomes may use cave walls, irregular stone, stalagmites, collapsed rock, roots, mineral formations, and carved passages.
- [ ] Ruin/settlement/farm biomes may use broken masonry, natural stone walls, hedges, timber fences, ruins, gates, and terrain embankments where appropriate.
- [ ] Desert/dry biomes may use sandstone, dunes, eroded rock, canyon walls, dead vegetation, and rough fences where appropriate.
- [ ] Boundaries must never look like one repeated placeholder cube wall across every biome.
- [ ] Preserve clear gameplay readability: the player must understand what is solid, traversable, dangerous, decorative, climbable, or an entrance.
- [ ] Preserve valid collision, nav/pathing, horse clearance, combat spacing, line of sight, camera framing, minimap room boundaries, and hazard recovery.
- [ ] Use controlled procedural variation in scale, rotation, silhouette, material, clustering, and decoration without creating collision gaps or visual noise.
- [ ] Blend transitions between neighboring biomes so boundary language changes gradually instead of snapping abruptly.
- [ ] Reuse modular/poolable assets and LOD/culling-friendly compositions so the richer environment remains production-safe on target hardware.
- [ ] Add focused near-spawn biome test rooms/lanes for new boundary sets, then remove, retain as isolated harnesses, or convert them to production after approval.

- [ ] **C02.PORTAL.1 Open external entrance and exit doorways receive two-sided opaque animated light portals with a luminous frame and moving light bands; they hide what is behind the doorway, have no collider, and are generated only for legal open doorway metadata. — RUNTIME PROTOTYPE IMPLEMENTED LOCALLY / VERIFY.**
- [ ] The portal surface must clearly communicate an active passage without looking like a debug plane or opaque flat color.
- [ ] The effect should use restrained depth, motion, particles/noise, rim light, and color variation appropriate to the biome.
- [ ] The surface must not obscure interaction prompts, collision readability, mounted clearance, or transition state.
- [ ] Closed/locked doors must not display the same open-passage treatment.
<!-- B&D BIOME-WALLS-AND-PORTALS END -->

# C03 — Player movement, aiming, combat, damage, and weapons

<!-- B&D MOUNTED-RUN-INTRO START -->
### C03.RUN-INTRO — Mounted cinematic entrance before gameplay control

- [ ] **C03.RUN-INTRO.1 Play the mounted doorway entrance only for a fresh New Game or a non-death victory/cinematic restart. For the current rule, any new-run return after a cinematic shown beyond the doorway following the first boss counts as a victory/cinematic restart. The child begins mounted, rides through the legal entrance, receives the cinematic zoom-in/zoom-out, and control opens after the animation and input release. — REPAIR V2 IMPLEMENTED LOCALLY / VERIFY.**
- [ ] **C03.RUN-INTRO.1A Death -> New Game must not play the mounted entrance.**
  - Death overrides a death cutscene or other death presentation; it remains a death restart.
  - Keep the newly loaded player unmounted at the authored spawn point in the starting room.
  - Do not reposition the player/horse to the doorway, do not mount the player, and do not run the cinematic camera zoom.
  - Use the normal gameplay camera immediately.
  - Clear transient combat/telegraph state.
  - Inputs held during loading are discarded until physical release; when nothing is held, gameplay opens without an additional timer.
  - Menu/cinematic scenes without gameplay room metadata may not consume the pending death-restart reason.
  - — REPAIR V2 IMPLEMENTED LOCALLY / VERIFY.**
- [ ] On every fresh run and death -> New Game restart, the child begins already mounted on the horse outside/inside the entrance threshold as appropriate.
- [ ] The horse carries the child through the visible map entrance into the start area using a deliberate riding animation/path.
- [ ] During the entrance, player movement, attacks, ranged input, horse commands, Pet, menus that affect gameplay, and buffered actions remain locked.
- [ ] Inputs pressed while the scene is loading or while the cinematic is active are discarded, not replayed when control opens.
- [ ] The camera starts slightly closer to the child and horse for a cinematic zoom-in, tracks the entrance ride cleanly, then performs a smooth zoom-out to the normal gameplay framing.
- [ ] Gameplay control opens only after the horse reaches the validated end marker, the camera finishes returning to normal, and all transient input buffers are empty.
- [ ] The transition must not produce a slash, landing attack, red ground marker, damage effect, camera shake, hit-stop, or audio impact.
- [ ] The entrance route and end marker must be safe from enemies, lava, holes, telegraphs, test fixtures, collision seams, and horse flee triggers.
- [ ] The implementation must preserve normal mounted controls immediately after the cinematic ends.
- [ ] Provide a near-spawn deterministic test path and automated QA contracts; after approval, keep only the production entrance presentation and remove temporary markers/debug visuals.

- [ ] **C03.RUN-INTRO.2 Remove the red startup floor artifact by blocking enemy proximity/attack telegraphs and landing/combat visuals during the locked intro, destroying transient telegraph objects on load, and preventing player damage until gameplay opens. — IMPLEMENTED LOCALLY / VERIFY.**
<!-- B&D MOUNTED-RUN-INTRO END -->

**Category status: PROTOTYPE DONE / verification and production work remain**

## Movement and orientation

- [x] C03.01 Player movement.
- [x] C03.02 Top-down/angled world navigation.
- [x] C03.03 Player rotates toward attack direction on foot.
- [x] C03.04 Mouse-world aiming for desktop prototype.
- [x] C03.05 Dodge/dash foundation.
- [x] C03.06 Dodge i-frame foundation.
- [x] C03.07 Dodge invulnerability visual pulse.
- [x] C03.08 Improved forward-dodge visual.
- [ ] C03.09 Verify forward, backward, and side dodge behavior.
- [ ] C03.10 Ensure backward dodge never causes slow camera spin.
- [ ] C03.11 Tune movement, acceleration, turn response, and collision for mobile.

## Melee combat

- [x] C03.12 Light attack.
- [x] C03.13 Heavy attack.
- [x] C03.14 Physical attack signal tracking.
- [x] C03.15 Player parry timing state.
- [x] C03.16 Successful parry cancels physical damage.
- [x] C03.17 Physical melee attacks can be parried.
- [x] C03.18 Selective parry time freeze.
- [x] C03.19 Parry visual feedback.
- [x] C03.20 Melee attack buffer foundation.
- [x] C03.21 Airborne descent tracking.
- [x] C03.22 Landing attack detection and `x1.2` damage foundation.
- [x] C03.23 Landing attack visual improvement.
- [ ] **C03.23A Replace the current landing-attack presentation with the normal melee slash visual language rotated into a vertical/top-to-bottom strike; preserve landing damage, hit feedback, exclusivity, and heavy/light distinction. — INSERTED EARLIER BLOCKING WORK.**
- [ ] C03.24 Verify buffered attack ordering and prevent duplicate attacks.
- [ ] C03.25 Verify landing attack minimum-height threshold.
- [ ] C03.26 Verify landing attack multi-target damage exactly once per target.
- [ ] C03.27 Build complete physical-parry matrix:
  - sword;
  - charge/ram;
  - jump landing;
  - bite;
  - stomp;
  - hand strike;
  - tail attack;
  - body roll;
  - future physical attacks.
- [ ] C03.28 Confirm bullets, lasers, bombs, environmental damage, and bullet hell are not incorrectly parryable.
- [ ] C03.29 Verify freeze recovery for rigidbodies, animators, particles, enemies, and projectiles.

## Ranged combat and ammunition

- [x] C03.30 Ranged shooting foundation.
- [x] C03.31 Tap shooting.
- [x] C03.32 Charged magazine shot foundation.
- [x] C03.33 Automatic reload fix.
- [x] C03.34 Ammo/reload HUD foundation.
- [x] C03.35 Projectiles collide with walls.
- [x] C03.36 Projectile knockback policy by combatant profile.
- [ ] C03.37 Verify tap/charge/cancel/full-charge behavior.
- [ ] **C03.38 Define charged-shot ammunition cost and authoritative AoE damage: a normal shot uses multiplier `1`; a charged shot uses multiplier `N`, where `N` is the exact number of bullets actually consumed by that shot. Every unique eligible enemy reached by the shot's AoE receives base ranged damage × the multiplier, and every unique destructible reached receives the same multiplier as structural-hit damage. — REQUIREMENT DEFINED / IMPLEMENTATION PENDING.**
<!-- B&D RANGED-AOE-PROJECTILES START -->
- [ ] **C03.38A Make every player projectile an authoritative area-of-effect attack against enemies and destructible objects.**
  - A normal shot resolves one logical AoE event with multiplier `1`.
  - A charged shot that actually consumes `N` bullets resolves one logical AoE event with multiplier `N`.
  - Each unique enemy inside the valid AoE receives the shot's authoritative base ranged damage multiplied by `N`.
  - Each unique destructible object inside the valid AoE receives `N` structural hits; therefore a normal shot deals `1` structural hit.
  - `N` comes from ammunition actually removed at fire time, not requested charge duration, UI prediction, magazine capacity, or a stale pre-reload value.
  - If only part of the requested charged ammunition is available and the shot is legally fired, use the exact amount actually consumed.
  - Resolve direct projectile contact and the AoE through one shared logical-shot identifier so a target cannot receive both direct damage and duplicate overlap damage from the same shot.
  - Multiple colliders belonging to one enemy or one destructible count as one target for that logical shot.
  - Every different eligible enemy or destructible touched by the AoE may receive damage once from that shot; damage is not limited to the first target.
  - If a projectile is allowed to travel through several targets, retain a per-shot hit set and damage each unique target at most once.
  - Closed walls, closed doors, and other solid room boundaries block AoE propagation; do not damage unrelated targets through sealed geometry.
  - The gameplay collision radius and the visible impact/VFX radius must agree. Exact radius tuning remains a Play Mode task and may not silently change the approved multipliers.
  - Damage numbers show one final applied value per damaged enemy. A charged shot displays the total applied damage once per enemy, not one number per consumed bullet.
  - Destructible impact feedback must show the correct structural stage after the complete shot multiplier is applied.
  - Verify normal shots, partially charged shots, full-magazine shots, multiple enemies, multiple destructibles, mixed enemy/destructible groups, overlapping colliders, walls, doors, low ammunition, reload boundaries, and mobile aiming.
<!-- B&D RANGED-AOE-PROJECTILES END -->
- [ ] C03.39 Verify automatic reload without requiring another fire input.
- [ ] C03.40 Define and test laser behavior against dodge i-frames.
- [ ] C03.41 Tune ammo economy and recovery windows.

## Player health and lifecycle

- [x] C03.42 Damage/death/reset foundations exist.
- [ ] C03.43 Verify one source cannot apply duplicate damage in one hit.
- [ ] C03.44 Verify clean death and restart/reset state.
- [ ] **C03.44A Run-start input safety is reason-aware: fresh/cinematic starts discard input throughout the mounted entrance; death restarts skip the entrance but still discard held loading input until release while leaving the player unmounted at the authored spawn. No buffered action may replay. — SUPERSEDED BY C03.RUN-INTRO.1 / C03.RUN-INTRO.1A IMPLEMENTATION / VERIFY.**
- [ ] C03.45 Add final player health feedback and accessibility cues.
<!-- B&D FLOATING-DAMAGE-NUMBERS START -->
- [ ] **C03.45A Add professional world-space RPG damage numbers for every real damage event that actually reduces health.**
  - Show exactly one number per logical damage application, even when several colliders, trigger callbacks, or feedback systems observe the same hit.
  - Do not show a damage number for a parried, blocked, dodged, invulnerable, cancelled, zero-damage, or otherwise rejected hit.
  - Display the number close to the damaged visible target, facing the camera and remaining readable in the top-down/angled view.
  - Use a polished animation: quick controlled pop, slight upward travel, easing, short hold, and clean fade rather than a debug-style instant label.
  - Use strong contrast, outline/shadow, and restrained role-aware color treatment that fits the game; do not use an arbitrary flat debug red.
  - Scale emphasis by damage amount within safe visual limits, without allowing large values to cover the target or UI.
  - Offset/stack simultaneous numbers so multi-hit attacks remain readable and unrelated hits are not merged accidentally.
  - Preserve exact damage truth: displayed numbers must come from the authoritative applied-health delta, not the requested pre-mitigation amount.
  - Support player, enemy, horse, mini-boss, and boss damage when the target is visible and the encounter's presentation policy allows it.
  - Damage numbers are damage-only for this requirement; healing numbers require a separate approved rule.
  - Pool/reuse number objects and avoid per-hit material/font allocations so rapid attacks remain production-safe.
  - Verify desktop and mobile readability, camera zoom changes, mounted combat, multi-target attacks, repeated ticks, and low/high damage values.
<!-- B&D FLOATING-DAMAGE-NUMBERS END -->

## Environmental hazard recovery — inserted earlier-category work

- [ ] **C03.46 Add a validated last-safe-position tracker for the player.**
- [ ] C03.47 A safe point updates only when the player is grounded on stable walkable terrain, outside lava/chasm triggers, with enough clearance from edges and blockers.
- [ ] C03.48 Falling into a hole or chasm removes exactly 15 health.
- [ ] C03.49 After a hole/chasm fall, respawn the player at the latest valid safe point.
- [ ] C03.50 Touching lava removes exactly 10 health.
- [ ] C03.51 After lava contact, immediately return/knock the player to the latest valid non-lava safe point.
- [ ] C03.52 Add short recovery protection so a respawn cannot immediately retrigger the same hazard or duplicate damage.
- [ ] C03.53 If the player and horse enter a hazard while mounted, recover both to legal safe locations and always return the player on foot; never restore the mount relationship automatically.
- [ ] C03.54 Ensure hazard recovery cannot place the player inside enemies, walls, props, lava, holes, chasms, or active boss barriers.
- [ ] C03.55 Add Play Mode tests for repeated falls, low-health falls, mounted falls, lava-edge contact, moving hazards, death during hazard damage, and missing/invalid safe-point fallback.
- [ ] **C03.56 After every external hazard recovery, normal walking must never inherit jump/dodge/forced-gap permission. Reset dodge, jump, dash, and forced-entry timers; suppress new forced-gap classification for `0.55s`; then verify repeated walking around the same hole cannot start a damaging fall. — IMPLEMENTED LOCALLY / VERIFY.**
- [ ] **C03.57 Keep the hole/chasm fall duration at `2.05s`, with `4.60` base downward speed and acceleration up to `1.35x`, so the fall remains decisive without feeling too long. — IMPLEMENTED LOCALLY / VERIFY.**
- [ ] **C03.58 Reduce lava horizontal knockback toward `80%` of the previous safe displacement, expanding outward only as much as needed to find a validated safe non-lava landing point. Damage and bounce duration remain unchanged. — IMPLEMENTED LOCALLY / VERIFY.**
- [ ] **C03.59 Keep lava/hole/chasm test fixtures in a deliberate test lane at least `14m` from player spawn, using `16–28m` candidates and `18m/24m` fallbacks; TEST EVERYTHING regenerates the authoritative scene. — REPAIR V2 IMPLEMENTED LOCALLY / VERIFY.**
<!-- B&D QUICKSAND-HAZARD START -->
- [ ] **C03.60 Add quicksand as a room hazard/obstacle with the same legal physical entry possibilities as lava.**
  - The player may enter it by walking, landing, being knocked, dodging, or otherwise crossing into the quicksand surface/volume wherever the existing lava-entry rules permit equivalent contact.
  - Quicksand damage is exactly `4` health every `0.50s`.
  - A damage tick is legal only while the player is physically touching the quicksand, standing on its surface, or submerged in its volume.
  - If the player jumps and is airborne without touching the quicksand surface/volume, quicksand damage stops immediately; no damage tick may occur merely because the player is horizontally above it.
  - Sinking progress also pauses while the player is airborne and not touching quicksand.
  - Re-entering after a clean airborne separation resumes through the authoritative contact state; stale contact callbacks may not continue ticking damage.
  - Multiple quicksand colliders or overlapping trigger callbacks may not produce duplicate `4`-damage ticks in the same `0.50s` interval.
- [ ] **C03.61 While the player remains in contact, sink the character gradually and visibly into the quicksand.**
  - Sinking must be smooth, readable, and tied to authoritative continuous contact rather than frame rate.
  - The player escapes by jumping out.
  - The deeper the player has sunk, the more jump effort/progress is required to escape and the harder it becomes to leave.
  - Exact sink speed, jump-progress curve, movement reduction, and escape thresholds must be tuned in Play Mode without changing the approved damage/timing contract.
  - Leaving the quicksand legally clears sinking and escape-progress state so it cannot affect later movement.
- [ ] **C03.62 If the player becomes submerged beyond half of the body, trigger a quicksand failure/fall state.**
  - Stop normal movement/attack input for the failure transition.
  - Apply the same authoritative damage amount and death interaction used by falling into a hole/chasm.
  - Respawn the player at the latest validated safe position using the existing hazard-recovery rules.
  - The safe position tracker must never record a point inside, touching, or too close to active quicksand.
  - Recovery protection must prevent immediate quicksand retrigger or duplicate damage.
- [ ] **C03.63 Add professional quicksand presentation and QA.**
  - Use biome-appropriate sand/mud motion, sinking ripples, contact feedback, and audio without obscuring the player.
  - Make the contact boundary readable without looking like a debug trigger.
  - Show clear visual escalation as the player approaches the half-body failure threshold.
  - Preserve camera framing, minimap readability, doorway clearance, enemy navigation, and room escape routes.
  - Provide a clearly labeled deterministic quicksand test lane/room near spawn but isolated from normal player/horse startup safety.
  - Test walking entry, landing entry, knockback entry, dodge entry, jumping while still touching, fully airborne no-contact state, re-entry, overlapping colliders, repeated ticks, low health, death, safe respawn, and missing-safe-point fallback.
- [ ] **C03.64 The horse and mounted player must treat quicksand through the same established horse obstacle/hazard pipeline used for the other environmental obstacles.**
  - Horse AI, pathfinding, safe-spot selection, retreat, and mounted movement must classify quicksand as unsafe/non-traversable exactly as they classify the existing lava/hole obstacle set.
  - The horse must not choose a route, flee destination, follow path, or recovery point that crosses, touches, or ends inside quicksand.
  - When approaching quicksand, the horse uses the same existing obstacle-avoidance response as for the other hazards: stop, back away/retreat, and choose a safe alternate route where available.
  - Accidental horse contact or entry uses the existing horse hazard-safety and safe-recovery pipeline, returning the horse and rider to the latest valid horse-safe point without leaving stale flee, retreat, or mounted-control state.
  - While mounted, the horse remains authoritative for obstacle interaction; the rider follows the horse recovery and does not independently enter the player quicksand sinking/jump-escape loop.
  - After dismounting, the player follows the normal player quicksand rules whenever the player personally touches or enters quicksand.
  - Quicksand must be included in the same horse hazard masks, path rejection, safe-point validation, startup safety, near-spawn QA, and regression coverage as the other obstacles.
  - Do not create a special exception that allows the horse to cross quicksand, and do not leave horse behavior undefined.**
<!-- B&D QUICKSAND-HAZARD END -->

## Category acceptance

- Desktop and touch input produce identical combat intent.
- Every physical and non-physical damage type follows an explicit rule.
- No duplicate attacks, hits, reloads, or state transitions occur.
- Full Play Mode combat matrix passes.

---

# C04 — Horse traversal, mounted combat, damage, healing, and flee behavior

**Category status: IN PROGRESS**

## Implemented foundations

- [x] C04.01 Horse exists in gameplay.
- [x] C04.02 Mount/dismount foundation.
- [x] C04.03 Mounted shooting.
- [x] C04.04 Mounted shooting does not rotate the horse toward the shot.
- [x] C04.05 Horse health/healing support.
- [x] C04.06 Needs-healing visual restored.
- [x] C04.07 Buck/throw safety fallback restored.
- [x] C04.08 Combat flee controller foundation.
- [x] C04.09 Reliable flee motor foundation.
- [x] C04.10 Runtime horse repair/bootstrap support.
- [x] C04.11 Horse starting-position fix.
- [x] C04.12 Combatant rank/knockback profile foundation.

## Remaining ordered work

- [ ] C04.13 Verify combat flee from every combat activation path.
- [ ] C04.14 Define legal safe-spot selection and obstacle checks.
- [ ] C04.15 Keep flee destination away from active enemies and arenas.
- [ ] C04.16 Prevent follow/idle systems from overriding flee.
- [ ] C04.17 Define mounted combat-start behavior.
- [ ] C04.18 Verify two-hit buck/throw timing and animation.
- [ ] C04.19 Verify healthy, damaged, fainted, healing, and recovered states.
- [ ] C04.20 Verify horse cannot receive duplicate damage from one event. — Includes the observed startup damage/flee-with-no-local-enemy issue; user instructed that it remain deferred until this verification work.
- [ ] **C04.20A Maintain full horse health and cleared AI/hazard state through a `1.50s` startup stabilization window with `3.50s` calm/protection; flee requires a living combatant in the same room and within local range. — REPAIR V2 IMPLEMENTED LOCALLY / VERIFY.**
- [ ] C04.21 Tune horse movement for final room scale and mobile controls.
- [ ] C04.22 Add final riding, damage, healing, buck, and flee feedback.
- [ ] C04.23 Complete full horse Play Mode QA.

## Environmental hazard behavior — inserted earlier-category work

- [ ] **C04.24 Add chasm/hole avoidance to horse navigation and flee pathing. — IMPLEMENTED / VERIFY.**
- [ ] C04.25 Add lava avoidance to horse navigation and flee pathing. — IMPLEMENTED / VERIFY.
- [ ] C04.26 Hazard avoidance must apply while following, fleeing, wandering, approaching the player, and being ridden where AI pathing is active. — IMPLEMENTED / VERIFY.
- [ ] C04.27 If the horse nevertheless falls into a hole/chasm, the horse loses no health and returns to the latest legal horse-safe position. — IMPLEMENTED / VERIFY.
- [ ] C04.28 If the horse nevertheless enters lava, the horse loses no health and returns to the latest legal non-lava horse-safe position. — IMPLEMENTED / VERIFY.
- [ ] C04.29 SUPERSEDED BY LATER USER DECISION: mounted hazard recovery always returns the player on foot; recover the horse without health loss; place both in one legal area without overlap when possible. — IMPLEMENTED / VERIFY.
- [ ] C04.30 Prevent repeated hazard loops, separation across inaccessible geometry, and flee/follow systems overriding recovery. — IMPLEMENTED / VERIFY.
- [ ] **C04.31 Add Play Mode tests for mounted/unmounted chasm and lava recovery, flee pathing near hazards, and safe fallback when no recent horse-safe point is valid. — CURRENT ACCEPTANCE GATE.**
- [ ] **C04.31A Replace the one-second stationary hazard refusal with a safe retreat of roughly two short horse steps, then resume the prior mounted/AI behavior. — IMPLEMENTED IN LOCAL PACKAGE / VERIFY.**

## Zero-health exhausted follow and contextual Pet interaction

- [ ] **C04.32 Preserve the horse's current zero-health behavior while the player remains nearby. — IMPLEMENTED LOCALLY / VERIFY.**
- [ ] C04.33 When the player remains farther than `14m` for at least `1.25s`, enter an exhausted-follow state and move toward the player at approximately `20%` of normal follow speed. — IMPLEMENTED LOCALLY / VERIFY.
- [ ] C04.34 Stop exhausted follow at `8m` or less; use threshold hysteresis to prevent rapid start/stop switching. — IMPLEMENTED LOCALLY / VERIFY.
- [ ] C04.35 Exhausted follow does not restore health and cannot enable mounting, mounted shooting, sprinting, combat flee, attack utility, or normal healthy follow behavior. — IMPLEMENTED LOCALLY / VERIFY.
- [ ] C04.36 Exhausted movement must retain obstacle, hole, chasm, lava, boss-barrier, and doorway avoidance; invalid cross-room paths use a safe reposition fallback. — IMPLEMENTED LOCALLY / VERIFY.
- [ ] C04.37 Show the separated contextual Pet button and amber world cue without covering the horse HUD. The desktop default Pet binding is `Tab`; legacy serialized `P` migrates to `Tab`; short/long press behavior is unchanged; world and screen labels display the active binding dynamically. — IMPLEMENTED LOCALLY / VERIFY.
- [ ] C04.38 Short press: player pets the horse. Long press of at least `0.65s`: the horse affectionately nuzzles/rubs the player. Short and long press are mutually exclusive and long-press progress is visible. — IMPLEMENTED LOCALLY / VERIFY.
- [ ] C04.39 Pet interactions are expressive only: no healing, stat, score, collectible, or progression effect; cancel safely for distance, combat, dodge, mount, damage, cutscene, or door transition. — IMPLEMENTED LOCALLY / VERIFY.

## Category acceptance

- Horse state transitions are deterministic.
- Combat cannot leave the horse trapped, overlapping, or incorrectly following.
- Mounted aiming and touch targeting are stable.
- Damage/healing/buck/flee tests pass.

---

# C05 — Normal enemies, AI roles, encounter behavior, and spawn safety

**Category status: IN PROGRESS**

## Required enemy roster

- [x] C05.01 Sword enemy foundation.
- [x] C05.02 Shooter enemy foundation.
- [x] C05.03 Patrol/guard enemy foundation.
- [x] C05.04 Jumper enemy foundation.
- [x] C05.05 Bomb-placer enemy foundation.
- [ ] C05.06 Rammer enemy:
  - runs directly at the player;
  - body collision deals damage;
  - readable wind-up and recovery;
  - cannot overlap or repeatedly damage without cooldown.
- [ ] C05.07 Give every enemy a readable health bar/state where appropriate.

## Combat and escape behavior

- [ ] C05.08 Define enemy detection, pursuit, attack, disengage, and return states.
- [ ] C05.09 When the player tries to escape an active room, enemies may:
  - move to block the room exit; or
  - pursue/attack the fleeing player.
- [ ] C05.10 Prevent exit blockers from creating impossible overlaps or unwinnable geometry.
- [ ] C05.11 Prevent enemy clustering and unfair simultaneous attacks.
- [ ] C05.12 Define coordinated sword flanking.
- [ ] C05.13 Define patrol/guard routes and alert behavior.
- [ ] C05.14 Define bomb placement safety and readable explosion timing.

## Spawn and placement safety

- [x] C05.15 Guardian spawn anticipation/VFX foundation.
- [x] C05.16 Guardians are created hidden/inactive before reveal.
- [x] C05.17 Fair position resolution and alternate candidate search.
- [x] C05.18 Avoid spawning too close to player, collectible, or another guardian.
- [x] C05.19 Best-scored fallback when no perfect point exists.
- [x] C05.20 General enemy placement safety foundation.
- [x] C05.21 Future Square Jumper landing/summon safety support.
- [ ] C05.22 Verify all enemies avoid walls, obstacles, player overlap, and illegal terrain. — COLLECTIBLE GUARDIAN SAME-ROOM SAFETY IMPLEMENTED / VERIFY.
- [ ] C05.23 Skip a summon safely when no valid position exists. — SAME-ROOM HARD SAFETY IMPLEMENTED / VERIFY FOR COLLECTIBLE GUARDIANS.
- [ ] C05.24 Add spawn/summon limits per encounter.
- [ ] C05.25 Verify sword weapon/double-slash visuals in Play Mode.
- [ ] C05.26 Final normal-enemy composition, placement, and encounter order must vary legally per run under the deterministic run seed; editor-time scene randomness alone is not sufficient.

## Category acceptance

- Every normal enemy has one clear role and readable counterplay.
- Escape behavior is threatening but never geometrically unfair.
- All spawn, landing, and summon paths pass safety validation.

---

# C06 — Inventory, hidden collectibles, guardians, rewards, and run boosts

**Category status: IN PROGRESS**

## Inventory and hidden collectible rules

- [x] C06.01 Expanded inventory state.
- [x] C06.02 Game Boy collectible type.
- [x] C06.03 Battery collectible type.
- [x] C06.04 Game Cartridge collectible type.
- [x] C06.05 Secret badge HUD foundation.
- [x] C06.06 Badge labels: `GB`, `BAT`/`BAT x2`, `CART`.
- [x] C06.07 Badge appears only after pickup.
- [x] C06.08 No objective marker/checklist/empty slot/missing-item instruction.
- [x] C06.09 Environmental hints and cinematic consequences are allowed.
- [x] C06.10 Protected battery guardian encounters.
- [x] C06.11 Battery encounter hardening.

## Physical Battery guardian encounters

- [x] C06.11A Both Batteries are placed physically in the map and never come from reward chests.
- [ ] C06.11B Approaching a Battery activates its guardian encounter once per run.
- [ ] C06.11C Show a short spawn anticipation effect before guardians become active.
- [ ] C06.11D Spawn guardians around the player and Battery, but outside fair minimum safety radii and never directly adjacent to the player.
- [ ] C06.11E Spawn points must avoid other guardians, walls, props, lava, holes, chasms, inaccessible terrain, and active barriers while preserving at least one reachable movement/escape direction. — SAME-ROOM/WALL CONTRACT IMPLEMENTED / VERIFY.
- [ ] C06.11F Use alternate candidate positions and a scored legal fallback; delay/cancel an individual guardian spawn when no legal point exists instead of forcing an invalid spawn. — SAME-ROOM FALLBACK IMPLEMENTED / VERIFY; FULL CANCEL CONTRACT REMAINS.
- [ ] C06.11G Battery A is the first serious protected-collectible group encounter; Battery B is harder through roles, pressure, angles, or validated count—not unfair spawn proximity.
- [ ] C06.11H Re-entry cannot duplicate or infinitely restart the encounter; after victory the Battery remains at its original map position and becomes safely collectible.

## Rewards and chests

- [x] C06.12 Shared boss reward-chest controller foundation.
- [x] C06.13 Secret reward appears only after the assigned boss is defeated and chest flow completes.
- [x] C06.14 Third defeated mini-boss chest can expose a separate Parry freeze pickup.
- [x] C06.15 Parry freeze upgrade foundation from 1 second to 2 seconds.
- [ ] C06.16 Verify chest cannot open before encounter completion.
- [ ] C06.17 Verify rewards cannot duplicate or disappear.
- [ ] C06.18 Add final chest open/pickup feedback.

## Regular run boosts

- [ ] C06.19 Replace fragile reflection-based boost state with explicit data/state.
- [ ] C06.20 Extra Ammo boost.
- [ ] C06.21 Faster Reload boost.
- [ ] C06.22 Movement Speed boost.
- [ ] C06.23 Weapon Damage boost.
- [ ] C06.24 Maximum 3 ranks per boost.
- [ ] C06.25 Remove maxed boost types from available drop pool.
- [ ] C06.26 Normal-enemy boost drop chance: 2%.
- [ ] C06.27 Mini-boss boost drop chance: 12%.
- [ ] C06.28 Create physical pickup objects and stable visual identity.
- [ ] C06.29 Add pickup feedback and HUD level display.
- [ ] C06.30 Reset all run-only boosts between runs.
- [ ] C06.31 Add deterministic statistical QA for drop rates and rank limits.

## Category acceptance

- Secrets remain hidden.
- Inventory, chest, reward, and boost state are explicit and reset correctly.
- No reward can be granted early, duplicated, or advertised before discovery.

---

# C07 — Boss framework, deterministic role planning, and encounter contracts

**Category status: IN PROGRESS — CURRENT FEATURE CATEGORY**
**Resume item after C01 gate: C07.16**

## Approved encounter structure

- [x] C07.01 Four mini-boss archetypes approved:
  - Square Jumper;
  - Roller;
  - Serpent;
  - Quad Gunners.
- [x] C07.02 Each run selects 3 of 4 and leaves 1 absent.
- [x] C07.03 Selected archetypes receive unique roles:
  - Game Boy guardian;
  - Game Cartridge guardian;
  - pre-final-boss encounter.
- [x] C07.04 Secret rewards are delivered through post-victory chests.
- [x] C07.05 Pre-boss placement must leave additional playable map afterward.

## Implemented framework foundations

- [x] C07.06 Shared encounter/life-state types.
- [x] C07.07 Shared boss encounter controller.
- [x] C07.08 Configurable boss health channels.
- [x] C07.09 Multi-channel health group.
- [x] C07.10 Reusable multi-channel boss HUD.
- [x] C07.11 Shared attack-telegraph interface.
- [x] C07.12 Reward chest, phase threshold, magical barrier, and shared summon-budget foundations.
- [x] C07.13 Deterministic mini-boss planning data types and plan generator.
- [x] C07.14 Stage 17 seed fixes.
- [x] C07.15 Duplicate Stage 16 boss-health HUD removed.

## Current ordered work

- [ ] **C07.16 Wire the framework into one real playable test encounter. — IMPLEMENTED / VERIFY.**
- [ ] **C07.16A Add a clearly labeled, deterministic boss-framework test room/arena or direct portal immediately near player spawn so C07.16 can be tested without searching the generated map. Keep it isolated from normal spawn safety and classify/remove/convert it immediately after acceptance. — CURRENT PREREQUISITE.**
- [ ] **C07.17 Connect real damage sources to health channels and life states. — NEXT AFTER C07.16 ACCEPTANCE.**
- [ ] C07.18 Connect boss HUD to authoritative scene/prefab flow.
- [ ] C07.19 Implement arena activation, entrance lock, intro lockout, victory unlock, and cleanup.
- [ ] **C07.19A Mini-boss encounters do not hard-lock or seal the room: the player may escape. Reserve mandatory arena locking for approved full/final-boss encounters, and define safe mini-boss disengage/re-entry/reset behavior. — USER-APPROVED CONTRACT CLARIFICATION.**
- [ ] C07.20 Prove one-bar, multi-bar, knockout, zero-health-active, critical, and linked-death states.
- [ ] C07.21 Connect every real summoner to the shared summon budget.
- [ ] C07.22 Replace unnecessary reflection/runtime repair wiring with serialized references.
- [ ] C07.23 Complete Stage 16 Play Mode QA and completion report.
- [ ] C07.23A Ensure the shared framework can present a named boss state/resource such as `Calm`, `Irritated`, `Angry`, and `Danger` instead of a normal HP presentation while still using internal damage thresholds.
- [ ] C07.23B Support a boss encounter that transitions from three combat phases into a fourth non-combat objective/race phase.
- [ ] C07.23C Support routing defeat from any boss phase into a custom loss cinematic and clean full-run restart.
- [ ] C07.23D Support post-ending secret-boss activation without breaking the normal ending branches.
- [ ] C07.24 Complete seeded 3-of-4 selection and unique role assignment.
- [ ] C07.25 Define legal-room contract:
  - not the start room;
  - not the final-boss room;
  - enough space for the archetype;
  - supports sequential or parallel layouts;
  - similar progression allowed only with large spatial separation;
  - playable map remains after pre-boss.
- [ ] C07.26 Add bounded reroll and deterministic fallback behavior.
- [ ] C07.27 Add encounter-root creation and role-specific reward binding.
- [ ] C07.28 Validate many seeds and save reproducible failure seeds.
- [ ] C07.29 Complete Stage 17 integration report.
- [ ] C07.30 Pause at category gate, update Git, and ask the user before C08 begins.

## Category acceptance

- One shared framework supports all intended boss forms.
- 3-of-4 selection and role assignment are deterministic per seed.
- Invalid placements reroll/fallback without infinite loops.
- Stage 16 and Stage 17 completion reports pass.

---

# C08 — Individual mini-boss implementations

**Category status: DESIGN COMPLETE / implementation not started**
**Do not start until C07 category gate is approved.**

## C08A — Square Jumper

- [x] C08.01 Design approved.
- [ ] C08.02 Large square body and combatant setup.
- [ ] C08.03 Slow tracking movement.
- [ ] C08.04 Fast heavy jump.
- [ ] C08.05 Safe preselected landing position.
- [ ] C08.06 Readable landing telegraph and impact.
- [ ] C08.07 Landing damage and parry/i-frame rules.
- [ ] C08.08 Bullet-hell patterns.
- [ ] C08.09 Two simultaneous side swords.
- [ ] C08.10 Summon only sword, shooter, and patrol enemies.
- [ ] C08.11 Summons use VFX, delay, safe placement, and shared cap.
- [ ] C08.12 Late-phase increased jump and bullet-hell pressure.
- [ ] C08.13 Reward chest after victory.
- [ ] C08.14 Full Play Mode QA.

## C08B — Roller

- [x] C08.15 Design approved.
- [ ] C08.16 Large round body.
- [ ] C08.17 Fast roll charge with readable startup.
- [ ] C08.18 Body-contact damage cooldown.
- [ ] C08.19 Continuous rotating spiral bullet hell.
- [ ] C08.20 Summon jumper, bomb-placer, and sword enemies.
- [ ] C08.21 Safe summon placement and shared cap.
- [ ] C08.22 Readable recovery window.
- [ ] C08.23 Late-phase faster rolls and denser spiral.
- [ ] C08.24 Reward chest after victory.
- [ ] C08.25 Full Play Mode QA.

## C08C — Serpent

- [x] C08.26 Design approved.
- [ ] C08.27 Segmented snake movement.
- [ ] C08.28 Head-only damage weak point.
- [ ] C08.29 Body contact damage with cooldown.
- [ ] C08.30 Lunge bite.
- [ ] C08.31 Tail grab with guaranteed automatic release.
- [ ] C08.32 Tail whip.
- [ ] C08.33 Three-shot fan repeatable up to three times.
- [ ] C08.34 Enraged faster final phase.
- [ ] C08.35 Reward chest after victory.
- [ ] C08.36 Full Play Mode QA.

## C08D — Quad Gunners

- [x] C08.37 Design approved.
- [ ] C08.38 Implement fixed identities:
  - `RapidBlue`: fast low-damage bullets; summons jumpers;
  - `HeavyRed`: slow high-damage bullets; summons bomb placers;
  - `SlowPurple`: normal damage plus 1-second slow; summons shooters;
  - `KnockbackYellow`: normal damage plus knockback; summons sword enemies.
- [ ] C08.39 Hard-code identity-to-color/attack/summon mapping.
- [ ] C08.40 Per-member 10-second summon timer.
- [ ] C08.41 Two enemies per summon.
- [ ] C08.42 Shared active-summon cap.
- [ ] C08.43 Coordinated sword flanking without clustering.
- [ ] C08.44 Speed escalation:
  - 4 alive: `1.00x`;
  - 3 alive: `1.08x`;
  - 2 alive: `1.16x`;
  - 1 alive: `1.25x`.
- [ ] C08.45 Four-color segmented health UI.
- [ ] C08.46 Reward chest only after all four die.
- [ ] C08.47 Full Play Mode QA.

## Category acceptance

- All four archetypes work in isolated arenas.
- Any three can be selected and assigned without archetype-specific framework hacks.
- Every encounter has readable telegraphs, safe summons, reward flow, and QA report.
- Pause and request user review before C09.

---

# C09 — Final and narrative bosses

**Category status: PARTLY DESIGN COMPLETE / RECOVERY REQUIRED**

## C09A — Existing approved black/white final boss

- [x] C09.01 Black/white colossus concept approved.
- [x] C09.02 At least 2.5 times player height.
- [x] C09.03 Final room only.
- [x] C09.04 Magical exit barrier remains until full victory.
- [x] C09.05 Three-stage structure approved:
  - Stage 1: joined form, one health bar, no summons;
  - Stage 2: split form, one health bar per half, no summons, zero-health half remains knocked out;
  - Stage 3: both halves return with final bars, summoning enabled, final collapse waits for both.
- [ ] C09.06 Joined clap crush.
- [ ] C09.07 Dual-eye three-pass sweeping lasers.
- [ ] C09.08 Fast direct laser.
- [ ] C09.09 Stomp.
- [ ] C09.10 Two-arm moving spin.
- [ ] C09.11 Hard but readable bullet hell.
- [ ] C09.12 Split transition at 60%.
- [ ] C09.13 Independent Stage-2 health and knockout.
- [ ] C09.14 Stage-3 health reset/final bars.
- [ ] C09.15 Per-half laser, leg jump/stomp, hand strike, and half-range one-arm spin.
- [ ] C09.16 Split flanking behavior.
- [ ] C09.17 Stage-3-only summoning every 2 seconds per half.
- [ ] C09.18 Shared summon cap.
- [ ] C09.19 Linked final-death condition.
- [ ] C09.20 Separate black and white collapses.
- [ ] C09.21 Animated barrier disappearance.
- [ ] C09.22 Exit becomes traversable only after barrier animation completes.
- [ ] C09.23 Full Play Mode QA.

## C09B — Mother Boss — recovered secret post-ending boss

**Design status: DESIGN COMPLETE — implementation not started**

### Unlock and story position

- [x] C09.24 The Mother Boss is a secret post-ending encounter.
- [x] C09.25 It activates only when the player reaches the ending with the complete hidden set:
  - Game Boy;
  - two Batteries;
  - Game Cartridge.
- [x] C09.26 The existing black/white boss remains the normal final combat boss; Mother does not replace it.
- [x] C09.27 In the complete-set ending, the normal colorful light plays across the player’s face first.
- [x] C09.28 Then the bedroom door opens and Mother enters only partially: a body fragment, silhouette, shadow, or similarly limited reveal rather than her full body.
- [x] C09.29 The partial entrance transitions into the Mother Boss encounter.
- [x] C09.30 Losing during any Mother Boss phase plays a cutscene of the player in bed saying `I'm bored`, then restarts the game from the beginning.
- [x] C09.31 Winning plays the colorful-light ending without Mother entering at the end; the game ends on the colorful light.

### Visual identity and resource model

- [x] C09.32 Mother uses a large model with a ponytail and arms normally extended outward.
- [x] C09.33 During the scream, her arms move even farther out to the sides.
- [x] C09.34 Mother has no normal visible HP bar.
- [x] C09.35 Her visible state is an anger progression:
  1. `Calm`;
  2. `Irritated`;
  3. `Angry`;
  4. `Danger`.
- [x] C09.36 Internally, Phase 1 and Phase 2 each have effective durability equal to `2.5x` the black/white boss health reference.
- [x] C09.37 Internally, Phase 3 has effective durability equal to `3x` the black/white boss health reference.
- [x] C09.38 Phase 4 has no combat HP/durability; it is an objective race.
- [ ] C09.39 Design the anger-state UI so it communicates progress without looking like a normal HP bar.

### Core attack kit

- [ ] C09.40 **Broom attack:** Mother can produce the broom in every combat phase and attack both side-to-side and top-to-bottom.
- [ ] C09.41 Broom attacks need readable startup, separate sweep/overhead hit volumes, and a recovery window.
- [ ] C09.42 **Window-cleaner spray:** at relatively close range, fire three normal-damage projectiles in a fan.
- [ ] C09.43 Window-cleaner spray remains available repeatedly in every combat phase, controlled by the attack scheduler rather than a one-time limit.
- [ ] C09.44 **Shoes/slippers bullet hell:** create complex patterns using shoes and slippers; every shoe/slipper projectile has a slightly larger hit area than a normal bullet.
- [ ] C09.45 **Full-screen attraction field:** Mother initially stands in place and pulls the player toward her across the full arena.
- [ ] C09.46 While the player continuously walks without jumping or dodging, movement slows progressively until the player begins to slide/pull backward toward Mother.
- [ ] C09.47 Jumping or dodging interrupts/resets the accumulated pull slowdown.
- [ ] C09.48 During the early attraction version, Mother fires radial rows that divide the circle into eight equal sectors.
- [ ] C09.49 From the midpoint of Phase 2 onward, Mother rotates during attraction and the radial rows become a spiral.
- [ ] C09.50 **Call Father:** a large Father figure runs across the screen; collision deals high damage.
- [ ] C09.51 Father’s lane and entry side require a clear warning and a reachable avoidance gap.
- [ ] C09.52 **Pinning beam:** a fast direct laser aimed at the player deals low damage and applies a long stun on hit.
- [ ] C09.53 **Scream:** Mother stands in place, moves her arms farther outward, and screams in a radius; a hit deals medium-plus damage and applies a short stun.
- [ ] C09.54 **Mother dodge:** Mother can dodge in every direction like the player, with readable recovery and controlled frequency.

### Foreground clothing occlusion

- [ ] C09.55 During clothing-obstruction events, shirts, trousers, and underwear float between the camera and the arena.
- [ ] C09.56 When the event is active, maintain a minimum of 2 and a maximum of 4 clothing items visible simultaneously.
- [ ] C09.57 Outside an active clothing-obstruction event, the clothing does not need to remain on screen.
- [ ] C09.58 Clothing may obscure visibility briefly or sometimes somewhat longer, but only part of the screen and never the entire screen.
- [ ] C09.59 Clothing must never fully hide the player, Mother, a critical telegraph, a hazard edge, or the only safe route at the same time.

### Phase adaptation

- [ ] C09.60 **Phase 1 — Calm (`2.5x` reference durability):**
  - broom sweep/overhead;
  - close-range three-shot spray;
  - introductory shoes/slippers patterns;
  - stationary attraction with eight radial sectors;
  - occasional directional dodge;
  - generous recovery windows.
- [ ] C09.61 **Phase 2 — Irritated (`2.5x` reference durability):**
  - faster broom combinations and spray reuse;
  - denser shoe/slipper patterns;
  - Father charge, pinning beam, and scream enter the active pool;
  - attraction remains stationary during the first half;
  - from the phase midpoint, Mother rotates and radial rows become a spiral;
  - dodges become more frequent but remain telegraphed.
- [ ] C09.62 **Phase 3 — Angry (`3x` reference durability):**
  - full attack kit available;
  - spiral attraction available from the beginning;
  - stronger pull accumulation and faster spiral rotation while preserving dodge/jump escape windows;
  - more complex shoes/slippers bullet hell with readable gaps;
  - faster broom chains, Father charges, screams, pinning beams, and directional dodges;
  - limited attack combinations rather than simultaneous unavoidable overlaps.
- [ ] C09.63 Phase transitions include a clear animation/anger-state change and temporary transition protection.

### Fairness and anti-lock rules

- [ ] C09.64 A long pinning stun cannot be immediately chained into an unavoidable Father collision, scream, attraction capture, or lethal bullet pattern.
- [ ] C09.65 After either stun ends, apply short stun resistance before another stun can take effect.
- [ ] C09.66 Father charge, scream, broom overhead, and attraction/spiral each require distinct readable telegraphs.
- [ ] C09.67 The attack scheduler must preserve at least one legal escape option and prevent the foreground clothing from hiding that option.
- [ ] C09.68 Mother’s dodge cannot cancel every player punish window or create permanent invulnerability.

### Phase 4 — Danger — non-combat room-task race

- [x] C09.69 Phase 4 is not a combat phase.
- [x] C09.70 Mother walks toward the Game Boy to take it; her physical progress is the visible timer.
- [x] C09.71 The player must complete room-cleaning/tasks using existing controls and mechanics, then reach the required spot beside the bed before Mother reaches the Game Boy.
- [ ] C09.72 Use a deterministic or validated task set that remains completable within the available route/time.
- [ ] C09.73 Approved task pool, without adding a new interaction button:
  - hit or shoot dirt/dust targets;
  - break light clutter piles or blockers with sword/shots;
  - shoot hanging stains/cobweb-style targets that melee cannot reach;
  - collect scattered toys/clothes by touching them and auto-deliver them by entering a basket/box zone;
  - defeat 2–3 light enemies or mess-themed variants;
  - touch/clear a small ordered set of marked tidy spots through movement;
  - navigate around existing furniture/hazards and reach the bed endpoint.
- [ ] C09.74 Select a readable subset of tasks per encounter rather than requiring every task type at once.
- [ ] C09.75 Task completion must be visible through world changes and concise feedback, not a large checklist that advertises unrelated hidden collectibles.
- [ ] C09.76 Success requires all selected tasks complete and the player entering the bed safe spot before Mother enters the Game Boy pickup zone.
- [ ] C09.77 Failure occurs if Mother reaches/takes the Game Boy first; route to the same `I'm bored` loss cutscene and full restart.
- [ ] C09.78 Phase 4 cannot become impossible because of enemy placement, blocked paths, hazards, missing task objects, or an invalid bed destination.

### Final approved Patience, transition, interaction, and arena decisions

- [x] C09.79A Mother uses a fresh full-width **Patience** bar in Calm, Irritated, and Angry; labels are `Patience — Calm`, `Patience — Irritated`, and `Patience — Angry`.
- [x] C09.79B Normal player damage and applicable damage boosts reduce Patience; Patience never regenerates.
- [x] C09.79C Patience capacities remain `2.5x`, `2.5x`, and `3x` the black/white boss reference. Danger has no draining Patience bar.
- [ ] C09.79D On each transition, play a dedicated animation, make Mother temporarily invulnerable, keep player movement enabled, clear active player/boss projectiles safely, preserve room geometry/lighting, restore `10%` maximum player health, and grant no automatic ammunition.
- [x] C09.79E Broom attacks are physical and parryable; cleaner projectiles respect player Dodge i-frames; footwear projectiles cannot be destroyed.
- [x] C09.79F Jump reduces attraction buildup by `35%`; Dodge reduces it by `60%`; neither completely resets it.
- [x] C09.79G Father cannot be parried. Pinning stun is `1.75s`; scream stun is `0.75s` and interrupts the current player attack; post-stun resistance is `1.25s`.
- [x] C09.79H Mother's Dodge uses the player's core distance, duration, i-frame, and collision rules, while AI frequency preserves real punish windows.
- [x] C09.79H1 Mother may dodge only from neutral movement or approved dodge-cancellable recovery; she cannot cancel committed broom/spray/Father/beam/scream/attraction/transition/stun/defeat states.
- [x] C09.79H2 Calm: 1 charge, `10.0s` recharge, maximum 1 dodge per rolling `8.0s` and 3 total in the phase; `0.65s` attack lock after recovery.
- [x] C09.79H3 Irritated: 2 charges, `8.0s` recharge each, maximum 2 dodges per rolling `14.0s`, minimum `1.10s` between starts, and a `1.00s` punish window after the second.
- [x] C09.79H4 Angry: 2 charges, `6.0s` recharge each, maximum 3 dodges per rolling `14.0s`, no more than two consecutive, minimum `0.75s` between starts, `1.20s` lockout after two consecutive dodges, and no dodge during the attraction/spiral channel.
- [x] C09.79H5 Danger: zero combat dodges; Mother follows only the fixed visible Game Boy route. All dodge directions must pass wall, hazard, doorway, arena, player-overlap, and legal-follow-up validation.
- [ ] C09.79I Foreground clothing is non-interactive and phase-aware:
  - Calm: every `14–20s`, 2 items, about `0.8–1.2s`;
  - Irritated: every `10–16s`, 2–3 items, about `1.0–1.5s`;
  - Angry: every `7–12s`, 3–4 items, about `1.2–1.8s`;
  - Danger: disabled.
- [x] C09.79J Danger selects exactly 4 randomized task groups with no mandatory order: at least one cleanup/pickup, one destruction/combat, one movement/navigation, and one compatible extra group.
- [x] C09.79K Completing one task group pauses Mother for `0.8s`; attacks cannot damage, parry, stun, or slow her during Danger.
- [x] C09.79L Player takes no health damage in Danger; there are no lava pools, holes, or chasms; light enemies delay only through knockback.
- [x] C09.79M All four task groups must be complete before the bed-side destination counts. Failure occurs only after Mother completes the Game Boy pickup animation.
- [x] C09.79N Success sequence: Mother stops, turns and leaves through the door; player takes the Game Boy; colorful-light ending plays; Mother does not re-enter.
- [x] C09.79O Defeat sequence: short defeat animation, normal bed cutscene presentation, `I'm bored`, new procedural seed, all run items/boosts reset, no Mother checkpoint.
- [x] C09.79P Arena is a large transformed bedroom with stable geometry and lighting, no lava/holes/chasms, and furniture/task objects as navigation obstacles.
- [x] C09.79Q Game Boy is hidden during phases 1–3 and revealed on a low bedside table at Danger start; Mother follows one fixed visible route from the door to it.
- [x] C09.79R The horse never enters the Mother sequence. After the black/white boss, mounted entry through the next door is blocked until the player manually dismounts; recommended prompt: `Dismount before entering.`
- [ ] C09.79S After dismounting, validate a safe waiting point outside the door and prevent follow/flee/wander/recall, mounted camera, mounted weapon, rider state, or horse HUD from crossing into the ending/Mother scene.

### Mother Boss QA

- [ ] C09.79 Verify unlock occurs only with the full collectible set.
- [ ] C09.80 Verify losing from every phase uses the correct cutscene and restarts cleanly.
- [ ] C09.81 Verify winning suppresses Mother’s final entrance and ends on the colorful light.
- [ ] C09.82 Verify all anger-state durability multipliers.
- [ ] C09.83 Verify every attack, stun immunity rule, clothing-occlusion rule, and Phase-4 task combination.
- [ ] C09.84 Complete full Play Mode, balance, readability, and performance QA.

## Category acceptance

- The relationship between every narrative/final boss is explicit.
- No lost boss concept is overwritten.
- Final room, ending, reward, and barrier contracts are stable.
- Pause and request user review before C10.

---

# C10 — Map generation, level design, encounter placement, hazards, and pacing

**Category status: DESIGN EXPANDED / implementation planned**

## Multi-route map topology

- [x] C10.01 Prototype procedural maze exists.
- [ ] C10.02 Replace the prototype maze feel with a designed final level.
- [ ] C10.03 Generate a minimum of 3 major viable solution routes from the early map toward completion; target 4 when the seed and available space permit.
- [ ] C10.04 Routes do not need to remain completely separate: they should split, merge, and split again at intentional points.
- [ ] C10.05 Each major route must contain a substantial unique segment and lead part of the way through different areas and rooms.
- [ ] C10.06 Major routes should ideally be spatially far from one another rather than running as nearby parallel corridors.
- [ ] C10.07 Routes may converge into one route only after enough distance/progression has passed to make the earlier choices meaningful.
- [ ] C10.08 Later branches may separate again after a merge when it improves exploration and replayability.
- [ ] C10.09 Do not count tiny detours around the same obstacle as separate solution routes.
- [ ] C10.10 Add graph validation proving the required number of topologically meaningful route families for each accepted seed.
- [ ] C10.11 Preserve a complex but understandable overall route and readable primary progression.
- [ ] C10.12 Include optional side paths and hidden secret spaces.
- [ ] C10.13 Use large rooms rather than repeated maze cells.
- [ ] C10.14 Room sizes must support horse traversal, bullet hell, mini-bosses, and final/narrative boss movement.
- [ ] C10.15 Include dedicated final-boss, post-ending Mother Boss, exit, and ending/cinematic spaces as required by the final sequence.

## Inaccessible macro-regions and route-shaping obstacles

- [ ] C10.16 Reserve inaccessible regions with an approximate footprint of 1–4 room units.
- [ ] C10.17 Inaccessible regions may use mountains, rock formations, boulder fields, lakes, chasms, holes, columns, large obstacles, or mixed landmark formations.
- [ ] C10.18 These regions are route-shaping negative space, not fake accessible rooms.
- [ ] C10.19 Use them to keep major routes apart, create landmarks, hide connections, and force meaningful detours.
- [ ] C10.20 Never create an inaccessible formation that disconnects every valid route, traps required content, or makes the horse path impossible.
- [ ] C10.21 Validate navigable clearance around columns/obstacles for player, horse, enemies, and camera.

## Natural geometry

- [ ] C10.22 Rounded turns.
- [ ] C10.23 Curved corridors.
- [ ] C10.24 Occasional winding/curling paths.
- [ ] C10.25 Reduce constant 90-degree geometry.
- [ ] C10.26 Preserve readability with a controlled mix of straight and curved forms.
- [ ] C10.27 Create natural transitions between rooms, routes, and inaccessible landmarks.

## Room hazards

- [ ] C10.28 Add holes and chasms inside selected rooms with readable edges and legal routes around them.
- [ ] C10.29 Connect hole/chasm triggers to the C03 safe-position system: player loses 15 health and respawns at the latest safe point.
- [ ] C10.30 Connect horse navigation/recovery to C04: horse avoids holes/chasms and loses no health if recovery is needed.
- [ ] C10.31 Add lava zones inside selected rooms with readable boundaries and non-lava escape space.
- [ ] C10.32 Connect lava triggers to C03: player loses 10 health and returns to the latest valid non-lava safe point.
- [ ] C10.33 Connect horse navigation/recovery to C04: horse avoids lava and loses no health if it enters.
- [ ] C10.34 If player and horse fall together, ensure both return to a legal shared recovery area.
- [ ] C10.35 Hazards must not be hidden completely by foreground clothing, camera framing, VFX, darkness, or enemy clutter.
- [ ] C10.36 Required routes, boss arenas, spawn points, reward chests, and ending interactions must remain reachable after hazard placement.

<!-- B&D DESTRUCTIBLE-PATH-BLOCKERS START -->
## Destructible biome-authentic path and doorway blockers

- [ ] **C10.36A Add a shared destructible-obstacle system for blockers placed inside rooms, across internal passages, or across open room exits.**
  - Every blocker is a real solid navigation/collision obstacle until fully destroyed.
  - A blocker receives structural damage at most once from one logical sword attack or one logical projectile, even if several hit colliders overlap it.
  - Standard light sword hit = `1` structural hit; spinning sword = `1` per blocker per attack; heavy or valid landing sword strike = `2`.
  - Normal projectile AoE = `1` structural hit to every unique destructible reached by that shot.
  - Charged projectile AoE = `N` structural hits to every unique destructible reached, where `N` is the exact number of bullets actually consumed by that charged shot.
  - Direct projectile collision and the projectile's AoE are one logical damage event; the same blocker may not receive both and may not be multiplied by child colliders.
  - One projectile may damage every different eligible blocker and enemy reached by its AoE, each exactly once.
  - Each hit must have readable impact, staged visual damage, debris/audio appropriate to the material, and no health-style combat feedback that confuses it with an enemy.
  - When destroyed, remove every blocking collider/nav obstruction atomically, update affected pathfinding, and leave no invisible collision.
  - Horse and enemy navigation treat the blocker as solid before destruction and re-evaluate routes after destruction.
  - Required progression can ask the player to destroy a blocker, but placement must never create a softlock, trap the horse, hide an unavoidable attack, or block the only safe combat area.

- [ ] **C10.36B Use the following initial eight blocker families, selected only where they fit the active biome and room language.**
  1. **Clay pots / urn row — `1` hit per pot.** Use rows or clustered pots in ruins, settlements, caves, and dry areas. Each pot shatters independently; collision disappears with that pot immediately. A single projectile AoE may shatter every separate pot it reaches.
  2. **Tall grass, reeds, or thin bamboo — `2` hits.** First hit bends/thins the mass; second clears it. Best for grassland, swamp, lakeside, and overgrown rooms.
  3. **Thorn bush / bramble — `3` hits.** Each hit removes one visible density layer. It blocks movement but deals no contact damage unless a separate rule is approved.
  4. **Dense vines / root tangle — `4` hits.** Cut sections progressively; keep the passage solid until the final required section breaks. Best for forest, cave, ruin, and overgrown transitions.
  5. **Rotten wooden barricade / rough fence — `4` hits.** Splinters in stages and fits settlements, farms, forest paths, and abandoned structures.
  6. **Sand, dirt, or loose-rubble pile — `5` hits.** Visually collapses after every hit, but keeps one clean authoritative collider until fully cleared so the player cannot squeeze through a half-broken state.
  7. **Cracked boulder / fractured stone slab — `6` hits.** Three crack stages, material-specific chips, and a strong final break. Fits mountain, cave, desert, lava, and ruin areas.
  8. **Crystal, ice, or mineral growth — `5` hits.** Biome-specific brittle cluster with progressive fractures and a clean final shatter; cosmetic shards may not become damaging physics clutter.

- [ ] **C10.36C Apply blocker families by biome rather than selecting from one universal visual pool.**
  - Grass/forest/overgrown: tall grass, bramble, vines/roots, wooden barricades.
  - Mud/swamp/lakeside: reeds, roots, bramble, rotten wood.
  - Sand/desert/dry ground: pots, sand piles, cracked sandstone, rough fences.
  - Cave/mountain: roots, rubble, boulders, mineral growth.
  - Ruin/settlement/farm: pots, barricades, rubble piles, cracked masonry.
  - Ice/magical/mineral regions: cracked stone plus ice/crystal growth.
  - Transition rooms may blend only compatible neighboring families.

- [ ] **C10.36D Increase blocker frequency and per-room density gradually by normalized route depth, using these initial tuning targets.**
  - Start/tutorial/entrance room and immediate mounted-intro route: `0` blockers.
  - `0–20%` depth: `0–1` blocker group in at most about `25%` of eligible rooms; never block the sole mandatory exit.
  - `20–45%` depth: `0–2` groups in about `30–45%` of eligible rooms.
  - `45–70%` depth: `1–3` groups in about `50–65%` of eligible rooms; one required breakable passage may appear when safe.
  - `70–100%` depth: `2–4` groups in about `65–80%` of eligible rooms; up to two blocked lanes/doorways only when enough combat and horse clearance remains.
  - These percentages are tuning targets, not permission to violate route, encounter, reward, boss, or accessibility contracts.

- [ ] **C10.36E Prevent unfair repetition and procedural softlocks.**
  - Use the deterministic run seed for family, placement, durability variant, and grouping.
  - Cap consecutive blocker-heavy rooms and avoid repeating the same family in every neighboring room.
  - Never place blockers inside boss-intro paths, reward/chest interaction space, spawn protection, hazard recovery points, hidden-item pickup radius, ending interactions, or the mounted run entrance.
  - A doorway blocker must be reachable by sword or projectile from a legal standing position and may not overlap lava, holes, quicksand, enemies, portals, doors, or another blocker.
  - Keep at least one readable movement pocket and one legal retreat direction during active encounters.

- [ ] **C10.36F Add a clearly labeled deterministic blocker test room close to spawn containing all eight families and depth presets.**
  - The test room must be isolated from normal startup, horse calm, mounted intro, and production progression.
  - Validate light/heavy/spin/landing hit accounting, normal-shot `1` structural damage, charged-shot `N` damage from actual bullets consumed, AoE multi-enemy/multi-blocker damage, duplicate collider protection, direct-plus-AoE deduplication, staged visuals, collider removal, nav refresh, horse refusal before destruction, traversal after destruction, and mobile readability.
  - After approval, remove temporary labels/debug objects and either convert the room into an isolated retained harness or remove it from production generation.
<!-- B&D DESTRUCTIBLE-PATH-BLOCKERS END -->

## Final placement and pacing

- [ ] C10.37 Place and validate player start.
- [ ] C10.38 Place horse.
- [ ] C10.39 Place normal enemy encounters.
- [ ] C10.40 Place protected batteries.
- [ ] C10.41 Integrate Game Boy mini-boss role.
- [ ] C10.42 Integrate Game Cartridge mini-boss role.
- [ ] C10.43 Integrate pre-boss role with map remaining afterward.
- [ ] C10.44 Integrate black/white boss, secret Mother Boss transition, and ending spaces.
- [ ] C10.45 Place ammo/health support.
- [ ] C10.46 Place hidden routes without advertising them.
- [ ] C10.47 Integrate reward chests and ending room.
- [ ] C10.48 Validate sequential and parallel encounter layouts.
- [ ] C10.49 Validate all legal-room contracts and route counts across many seeds.
- [ ] C10.50 Ensure escape-blocking enemies do not create impossible rooms or force the player into unavoidable hazards.

## Biomes and environment layout

- [ ] C10.51 Grass areas.
- [ ] C10.52 Sand areas.
- [ ] C10.53 Mud areas.
- [ ] C10.54 Water/lake areas.
- [ ] C10.55 Dry-ground areas.
- [ ] C10.56 Lava regions where appropriate.
- [ ] C10.57 Mixed transition zones.
- [ ] C10.58 Region-appropriate walls, mountains, columns, and obstacle materials.
- [ ] **C10.58A Keep every closed room boundary visually and physically high enough that the angled camera cannot see over it into an adjacent room; the prototype minimum is `22` world units. — IMPLEMENTED LOCALLY / VERIFY.**
- [ ] **C10.58B Preserve each wall base while extending height upward, avoid negative transform scale, and attach explicit texture-facing/quarter-turn/UV metadata so future natural asymmetric textures are never mirrored accidentally. — IMPLEMENTED LOCALLY / VERIFY.**
- [ ] C10.58C Future natural boundaries may replace the prototype wall silhouette, but must preserve the same collision, camera occlusion, doorway, minimap, horse-clearance, and asymmetric-texture orientation contracts.
- [ ] C10.59 Vegetation density varies by area.
- [ ] C10.60 Large rooms and inaccessible regions have distinct silhouettes.
- [ ] C10.61 Environmental storytelling props.

## Category acceptance

- Every accepted seed has at least 3 meaningful solution routes and targets 4 when space allows.
- Routes meaningfully separate, merge, and reconnect without becoming near-identical corridors.
- Inaccessible 1–4-room-scale regions shape the map without breaking progression.
- Every hazard applies the exact player/horse damage and recovery rules.
- The map feels authored, secrets remain hidden, and navigation works for player, horse, enemies, bosses, and mobile controls.
- Pause and request user review before C11.

---

# C11 — Camera, minimap, HUD, UI, readability, and accessibility

**Category status: IN PROGRESS**

## Camera and minimap foundations

- [x] C11.01 Camera follow foundation.
- [x] C11.02 Dynamic player-up minimap.
- [x] C11.03 Minimap room discovery.
- [x] C11.04 Player-position discovery fallback.
- [x] C11.05 Explored-room count.
- [x] C11.06 Minimap sector/alignment fixes.
- [x] C11.07 Camera/control polish pass.
- [x] C11.08 Obsolete camera/minimap fields removed.
- [ ] C11.09 Verify all four sectors and diagonal boundaries.
- [ ] C11.10 Verify keyboard movement does not rotate the camera incorrectly.
- [ ] C11.11 Preserve mouse/touch aiming rules while changing camera visibility.
- [ ] C11.12 Tune camera tilt, forward visibility, darkness, and horizon readability.
- [ ] **C11.12A When the player or mounted horse approaches a closed room wall, clamp both camera position and look point to the current room boundary and stop at the real wall collider; never reveal the next room through or above a closed wall. Authored open doorway sides remain traversable. — IMPLEMENTED LOCALLY / VERIFY.**
- [ ] **C11.12B Add a completed BBH circular badge: after all letters settle, a fully filled circle with a visible rim grows from zero behind the text, reaches full size, holds for exactly `0.50s`, and the intro ends at the end of that hold. — IMPLEMENTED LOCALLY / VERIFY.**
- [ ] C11.13 Make camera and minimap resolution/aspect-ratio safe for mobile landscape.
- [ ] **C11.13A The previous per-element cardinal rotation retained the approved four targets and easing but FAILED visual acceptance because room nodes, walls, and markers appeared to deform or flicker. — SUPERSEDED BY C11.13B RIGID-UNIT CORRECTION / VERIFY.**
<!-- B&D MINIMAP-RIGID-ROTATION START -->
- [ ] **C11.13B Rotate the complete minimap content as one rigid visual unit under one GUI matrix around one authoritative pivot. The fixed frame and clipping group remain outside the rotation; rooms, walls, openings, player marker, and horse marker preserve exact relative positions. Existing shortest-angle SmoothDampAngle targeting and exact epsilon snap remain active. — REPAIR V2 IMPLEMENTED LOCALLY / VERIFY.**
  - Put every room node, corridor/link, discovered/undiscovered layer, route line, hazard marker, player/horse/enemy marker, and map decoration under one stable minimap-content rotation root.
  - The clipping mask/frame may remain fixed, but all content inside it must rotate through the same parent transform and preserve exact relative positions.
  - Animate only the single content-root angle. Individual nodes, lines, icons, and labels may not run separate rotation, position, scale, layout, or easing animations during a direction change.
  - Preserve a fixed central pivot, uniform scale, stable anchors, and one authoritative local transform; do not rebuild layout or recalculate node coordinates every animation frame.
  - Continue using the four cardinal target angles and interpolate through the shortest signed angle with the approved slower professional ease-in/ease-out.
  - A new direction request during an active turn must retarget from the currently displayed root angle without snapping back, overshooting, stretching, or restarting child animations.
  - Snap to the exact target angle within a small completion epsilon to prevent perpetual sub-pixel shimmer.
  - Rotation must not disconnect corridors from rooms, move icons relative to their rooms, change clipping shape, alter room discovery, or distort the map at any aspect ratio.
  - Verify repeated rapid `90°`, `180°`, clockwise/counter-clockwise changes, low/high frame rates, paused/unpaused state, newly discovered rooms during rotation, mounted movement, and desktop/mobile landscape resolutions.
  - Acceptance requires zero node jitter, flicker, stretching, drifting, pulsing, corridor separation, or per-element wobble throughout the complete animation.
<!-- B&D MINIMAP-RIGID-ROTATION END -->

## HUD/UI

- [x] C11.14 Ammo count/reload HUD foundation.
- [x] C11.15 Secret badges after pickup only.
- [x] C11.16 Boss multi-bar HUD foundation.
- [x] C11.17 Parry feedback foundation.
- [ ] C11.18 Show time until next ammo/reload completion.
- [ ] C11.19 Animated reload state.
- [ ] C11.20 Final boss health-bar presentation.
- [ ] C11.21 Four-color Quad Gunners health UI.
- [ ] C11.22 Slow/knockback/status feedback.
- [ ] C11.23 Boost level HUD.
- [ ] C11.24 Enemy health-bar presentation.
- [ ] C11.25 Touch target-selection feedback.
- [ ] C11.26 Mobile safe-area and scaling pass.
- [ ] C11.27 Ensure HUD never advertises hidden collectibles.
- [ ] C11.27A Add a Mother Boss anger-state presentation (`Calm`, `Irritated`, `Angry`, `Danger`) without a normal HP bar.
- [ ] C11.27B Add concise Phase-4 task feedback that does not resemble the hidden-collectible checklist forbidden elsewhere.

## Readability/accessibility

- [ ] C11.28 Projectile color contrast.
- [ ] C11.29 Black/white boss contrast.
- [ ] C11.30 Telegraph readability.
- [ ] C11.31 Screen-clutter limits.
- [ ] C11.32 Camera motion comfort.
- [ ] C11.33 Status-effect clarity.
- [ ] C11.34 Input feedback for taps, drags, target loss, and cooldowns.
- [ ] C11.35 Accessibility options appropriate to the final scope.
- [ ] **C11.35A Add Key Rebinding to Settings for every rebindable desktop action. Capturing a new key/button must persist, handle duplicates safely, support reset defaults, and immediately update every gameplay HUD prompt, world prompt, tutorial label, cooldown hint, healing label, Pet label, and settings row to the live binding. Touch controls remain a separate presentation of the same gameplay actions.**
- [ ] C11.36 Validate holes, chasms, lava, and safe routes remain readable under lighting, VFX, bullet hell, and foreground clothing.
- [ ] C11.37 Validate Mother Boss clothing events keep 2–4 items visible only while active, never cover the full screen, and never erase all critical information.

## Category acceptance

- UI is readable across target mobile landscape resolutions.
- Minimap/camera behavior is stable.
- Hidden items remain unadvertised.
- All attacks and statuses are visually understandable.
- Pause and request user review before C12.

---

# C12 — Art, animation, VFX, lighting, atmosphere, and audio

**Category status: PLANNED**

## Character and gameplay art

- [ ] C12.01 Final player model/textures.
- [ ] C12.02 Final horse model/textures.
- [ ] C12.03 Final normal enemy models/textures.
- [ ] C12.04 Final mini-boss models/textures.
- [ ] C12.05 Final black/white boss model/textures.
- [ ] C12.06 Mother Boss model: large body, ponytail, arms extended outward, with a farther-out arm pose for the scream.
- [ ] C12.07 Weapons and sword visuals.
- [ ] C12.08 Bullets, lasers, bombs, and explosions.
- [ ] C12.09 Collectibles and reward chests.
- [ ] C12.10 Final animations for movement, attacks, damage, death, healing, buck, flee, phases, and collapses.

## Lighting, atmosphere, and VFX

- [ ] C12.11 Room lighting.
- [ ] C12.12 Outdoor atmosphere.
- [ ] C12.13 Wind.
- [ ] C12.14 Biome transition visuals.
- [ ] C12.15 Spawn smoke/teleport effects.
- [ ] C12.16 Impact VFX.
- [ ] C12.17 Parry/freeze VFX.
- [ ] C12.18 Laser charge/fire VFX.
- [ ] C12.19 Bullet-hell readability VFX.
- [ ] C12.20 Magical barrier VFX.
- [ ] C12.21 Chest and collectible pickup VFX.
- [ ] C12.21A Mother Boss broom, cleaning-spray, shoes/slippers, attraction field, eight-sector rows, spiral, Father charge, pinning beam, scream, dodge, and anger-transition VFX.
- [ ] C12.21B Foreground shirts, trousers, and underwear system with partial camera occlusion and 2–4 simultaneous visible items while active.
- [ ] C12.21C Hole/chasm, lava, damage, safe-respawn, and horse-recovery VFX.

## Audio

- [ ] C12.22 General gameplay music/ambience.
- [ ] C12.23 Wind ambience.
- [ ] C12.24 Walking/running.
- [ ] C12.25 Horse riding and horse states.
- [ ] C12.26 Weak/heavy hits.
- [ ] C12.27 Sword attacks.
- [ ] C12.28 Gunfire and charged shot.
- [ ] C12.29 Projectile impacts.
- [ ] C12.30 Bomb placement/explosion.
- [ ] C12.31 Enemy spawn.
- [ ] C12.32 Mini-boss intro.
- [ ] C12.33 Boss phase changes.
- [ ] C12.34 Laser charge/fire.
- [ ] C12.35 Clap crush and stomp.
- [ ] C12.36 Serpent attacks.
- [ ] C12.37 Chest opening and collectible pickup.
- [ ] C12.38 Magical barrier disappearance.
- [ ] C12.39 Ending/cinematic audio.
- [ ] C12.40 Mother Boss broom, spray, footwear bullet hell, attraction/spiral, Father charge, pinning beam, scream, dodge, anger transitions, Phase-4 task, loss, and victory audio.
- [ ] C12.41 Hole/chasm fall, lava contact, player recovery, and horse recovery audio.

## Category acceptance

- Prototype visuals are replaced consistently.
- VFX never obscure telegraphs.
- Audio communicates state and impact.
- Pause and request user review before C13.

---

# C13 — Story, endings, cinematics, Mother Boss branching, and final result logic

**Category status: DESIGN EXPANDED / implementation in progress**

## Existing ending-state logic

- [x] C13.01 Ending-state controller.
- [x] C13.02 `NoGameBoy`.
- [x] C13.03 `GameBoyNoBatteries`.
- [x] C13.04 `PoweredNoCartridge`.
- [x] C13.05 `PoweredWithCartridge`.
- [x] C13.06 Required complete hidden set:
  - Game Boy;
  - 2 Batteries;
  - Game Cartridge.
- [x] C13.07 Missing any required collectible: `I'm bored`.
- [x] C13.08 Complete set initially reaches the colorful-light result.
- [x] C13.09 Speech bubble/result rules are final-scene outcomes only, never hints or checklists.

## Standard ending branches

- [ ] C13.10 Final sit-down animation.
- [ ] C13.11 No Game Boy: player sits without taking anything out.
- [ ] C13.12 Game Boy/no power: take it out, fail to power it, place it aside.
- [ ] C13.13 Powered/no cartridge: gray/white no-game lighting.
- [ ] C13.14 Implement camera, lighting, VFX, audio, and speech-bubble presentation for incomplete-set endings.

## Complete-set secret continuation

- [x] C13.15 With Game Boy + two Batteries + Game Cartridge, colorful light appears on the player’s face.
- [ ] C13.16 After the colorful light, open the bedroom door.
- [ ] C13.17 Reveal only part of Mother or her shadow/silhouette rather than her complete body.
- [ ] C13.18 Transition from that partial reveal into the Mother Boss encounter.
- [ ] C13.19 Preserve the complete-set secret continuation without advertising its requirements before the ending.

## Mother Boss loss result

- [x] C13.20 Defeat in any Mother Boss phase uses the same failure result.
- [ ] C13.21 Play a cutscene of the player in bed saying `I'm bored`.
- [ ] C13.22 After the loss cutscene, reset all run state and return to the beginning of the game.
- [ ] C13.23 Verify defeat during Phase 1, 2, 3, and 4 all use the correct sequence without duplicate transitions.

## Mother Boss victory result

- [x] C13.24 Winning the Mother Boss removes Mother’s entrance from the final victory cutscene.
- [ ] C13.25 Play the colorful-light ending again/finally without Mother appearing.
- [ ] C13.26 End the game on the colorful light.
- [ ] C13.27 Verify black/white boss state, collectibles, Mother Boss victory, room state, and restart state cannot leak into another run.

## Final cinematic production

- [ ] C13.28 Camera cuts and timing.
- [ ] C13.29 Final lighting and VFX.
- [ ] C13.30 Final audio.
- [ ] C13.31 Final speech-bubble presentation.
- [ ] C13.32 Mother partial-reveal animation/shadow composition.
- [ ] C13.33 Mother Boss transition and return-to-ending transition.
- [ ] C13.34 Verify every standard and secret ending branch from clean runs.

## Category acceptance

- Incomplete hidden sets preserve the existing endings and `I'm bored` result.
- The complete set secretly continues from colorful light to the partial Mother reveal and boss.
- Losing any Mother phase shows the bed/`I'm bored` cutscene and restarts the game.
- Winning ends on colorful light without Mother entering.
- No ending leaks hidden collectible requirements before the result.
- Pause and request user review before C14.

---

# C14 — Balance, performance, persistence, production cleanup, and release

**Category status: PLANNED / some cleanup foundations exist**

## Combat and difficulty balance

- [ ] C14.01 Normal enemies.
- [ ] C14.02 Battery guardians.
- [ ] C14.03 Mini-boss health/damage.
- [ ] C14.04 Final/narrative boss health/damage.
- [ ] C14.05 Projectile density.
- [ ] C14.06 Summon caps and timing.
- [ ] C14.07 Dodge i-frame duration.
- [ ] C14.08 Parry timing/freeze duration.
- [ ] C14.09 Ammo economy.
- [ ] C14.10 Boost drop/effect balance.
- [ ] C14.11 Horse survivability.
- [ ] C14.12 Recovery windows.
- [ ] C14.13 Ensure every difficult attack remains readable and avoidable.
- [ ] C14.13A Balance Mother Boss Phase 1/2 `2.5x` and Phase 3 `3x` durability against fight length and mobile play without changing the approved multipliers.
- [ ] C14.13B Balance attraction buildup, spiral speed, Father damage, pinning duration, scream stun, clothing occlusion, dodge frequency, and Phase-4 task timing.
- [ ] C14.13C Balance 15-damage chasm recovery and 10-damage lava recovery so hazards punish mistakes without creating unavoidable death loops.

## Performance and robustness

- [ ] C14.14 Object pooling for projectiles and VFX.
- [ ] C14.15 Profiler pass.
- [ ] C14.16 Memory and GC allocation pass.
- [ ] C14.17 Stable frame-time target for supported mobile devices.
- [ ] C14.18 Stress test bullet hell, summons, VFX, and multi-boss health UI.
- [ ] C14.19 Prevent enemy clustering.
- [ ] C14.20 Validate touch/input latency and target-selection performance.
- [ ] C14.20A Stress-test Mother Boss bullet hell, attraction/spiral, Father runner, clothing occlusion, task objects, and three combat durability pools.
- [ ] C14.20B Stress-test multi-route map generation, route-family validation, inaccessible regions, hazard triggers, and safe-respawn tracking across many seeds.

## Persistence and run lifecycle

- [ ] C14.21 Define save/progression model.
- [ ] C14.22 Define what persists between runs.
- [ ] C14.23 Reset run-only inventory and boosts correctly.
- [ ] C14.24 Store deterministic map/run seed where required.
- [ ] C14.25 Version save data.
- [ ] C14.26 Handle corrupted/incompatible save data safely.

## Production cleanup

- [x] C14.27 Automated validation foundations.
- [x] C14.28 Initial obsolete-field cleanup.
- [ ] C14.29 Modular architecture cleanup.
- [ ] C14.30 Remove obsolete generated versions and repair packages.
- [ ] C14.31 Keep README and this plan synchronized.
- [ ] C14.32 Verify `.meta` integrity.
- [ ] C14.33 Ensure no ignored/generated folders are committed.
- [ ] C14.34 Migrate final configuration to prefabs/ScriptableObjects.
- [ ] C14.35 Remove runtime `UnityEditor` dependencies.
- [ ] C14.36 Add build/version metadata.

## Final vertical-slice acceptance

- [ ] C14.37 Zero compiler errors.
- [ ] C14.38 Zero release-blocking Console errors.
- [ ] C14.39 Complete start-to-finish run.
- [ ] C14.40 All 3 selected mini-bosses function.
- [ ] C14.41 Random roles and placement function.
- [ ] C14.42 One of the 4 mini-bosses is absent per run.
- [ ] C14.43 All final/narrative boss requirements function, including secret Mother Boss unlock, four phases, loss restart, and victory ending.
- [ ] C14.43A Every accepted map seed has at least 3 meaningful solution routes and targets 4 when space permits.
- [ ] C14.43B Chasm/hole and lava player/horse damage, avoidance, and safe recovery rules function.
- [ ] C14.44 Exit opens only after full victory and barrier completion.
- [ ] C14.45 All ending variants work.
- [ ] C14.46 Speech-bubble rule works.
- [ ] C14.47 Minimap works.
- [ ] C14.48 Ammo/reload UI works.
- [ ] C14.49 Mobile controls and landscape UI work.
- [ ] C14.50 Audio works.
- [ ] C14.51 No hidden collectible advertising.
- [ ] C14.52 Performance target passes.
- [ ] C14.53 Playable target build generated.
- [ ] C14.54 Clean-clone external verification.
- [ ] C14.55 Release notes, version, and Git tag.

## Category acceptance

The vertical slice is playable from start to finish on the target platform, externally verified, versioned, documented, and reproducible.

---

# 5. Legacy 36-stage cross-reference

The old roadmap numbers are retained for historical continuity:

| Legacy stage | New category |
|---:|---|
| 1–3 | C01, C02 |
| 4–6 | C06 |
| 7–9 | C13, C06 |
| 10–11 | C05, C06 |
| 12–15 | C07, C08, C09 |
| 16 | C07 |
| 17 | C07 and C10 |
| 18–21 | C08 |
| 22 | C09 |
| 23–27 | C10 |
| 28–29 | C11, C12 |
| 30 | C11 |
| 31 | C12 |
| 32 | C13 |
| 33 | C14 |
| 34 | C11, C14 |
| 35 | C00, C01, C02, C14 |
| 36 | C14 |

No legacy requirement is removed by this reorganization.

# 6. Exact current work sequence

1. Complete C00.14–C00.18: restore `/PROJECT_STATUS.md` as the single complete authoritative progress list and remove duplicate live status files.
2. Complete C01.16B–C01.16I: integrate Camera/Minimap regression, repository hygiene, and status-update enforcement into the existing `TEST EVERYTHING` button; run the one button and its one Play Mode checklist.
3. Implement and verify C03.46–C03.55: player last-safe-position, chasm/hole damage, lava damage, and recovery.
4. Implement and verify C04.24–C04.31: horse hazard avoidance and no-damage recovery.
5. Implement and verify C04.32–C04.39: zero-health exhausted follow and contextual Pet interaction.
6. Update `/PROJECT_STATUS.md` in the same commit, then return to saved resume point C07.16.
7. Finish Stage 16 shared boss-framework integration, including Mother-compatible named resource/state, non-combat phase, custom defeat routing, and secret post-ending activation.
8. Finish deterministic selection/role/legal-placement foundation through C07.29.
9. Update Git and pause at C07.30.
10. Tell the user C07 is complete, propose C08, and ask for changes before entering the next category.



<!-- B&D C03-C11 REPAIR V7 START -->
## Cross-cutting V7 repair order

1. Repair the failed V6 partial installation without resetting unrelated local work.
2. Finish the earlier blocking run-start/death-restart/entrance/exit/Pause/minimap contracts.
3. Implement the closed-wall room-visibility regression because it affects current camera and prototype verification.
4. Implement the BBH completed-circle change because it is an existing startup presentation, not a future art replacement.
5. Record the inaccessible macro-region clarification and the detailed Mother Dodge budgets as later C10/C09 work; do not begin those later systems before the current verification gate passes.
<!-- B&D C03-C11 REPAIR V7 END -->

# 7. Current blockers and risks

## Blockers

- Duplicate live status snapshots and one-shot patch artifacts must be removed in the same commit; `/PROJECT_STATUS.md` remains the only authoritative tracker.
- The latest TEST EVERYTHING run passed with one validator warning. C01 cannot close until the corrected classifier is installed and the one-button QA rerun reports `0` blockers and `0` warnings.
- No Mother Boss design blocker remains; the approved decisions must be implemented exactly and balanced without changing their structure.

## High risks

1. Runtime repair/install scripts may create duplicate or overlapping systems.
2. Reusable boss classes may be mistaken for integrated encounters.
3. Deterministic planning may not match final map progression.
4. Desktop prototype input may diverge from the mobile landscape target.
5. README and historical roadmaps may mislead contributors if not synchronized.
6. Lost chat-only requirements may remain undiscovered.
7. Final art, audio, map, balance, performance, persistence, and external build work remain substantial.
8. Multi-route validation must reject seeds whose apparent routes are only tiny nearby detours.
9. Hazard recovery and Mother Boss stuns can create soft-locks or repeated-damage loops unless recovery grace and legal-position validation are strict.
10. Foreground clothing can become unfair if it hides critical telegraphs, hazards, or the only safe route.
11. Duplicate live status files can silently diverge; only `/PROJECT_STATUS.md` may remain authoritative.
12. A commit that changes code or requirements without updating `/PROJECT_STATUS.md` breaks external continuity and must be blocked by QA/process.

# 8. Changelog

## 2026-06-07 — Repair V7: partial-package recovery, tall walls, camera stop, BBH circle, and Mother Dodge specification

- Recorded the failed V6 installer truth: payload files were copied, then the minimap preflight stopped before core-file edits.
- Built an idempotent continuation package for the actual partial state; no reset, stash, checkout, pull, clean, commit, or push is performed.
- Completed the authored entrance/exit, mounted-versus-death restart, existing Pause/menu, movement guard, and rigid minimap contracts.
- Raised prototype room-boundary walls to a 22-unit minimum while preserving their bases and added closed-room camera position/look-point clamping plus real wall collision stopping.
- Added explicit positive-scale, non-mirrored, per-segment facing/quarter-turn/UV metadata for future asymmetric natural wall textures.
- Confirmed that C10.16–C10.21 already define inaccessible 1–4-room macro-regions and explicitly added rocks/boulder fields to the allowed formations.
- Added the filled BBH circle behind the completed letters: zero-to-full growth, interior fill plus rim, exact 0.50-second final hold, and end-of-hold completion.
- Expanded the already-approved Mother Dodge into exact phase charge, recharge, rolling-window, follow-up, cancellation, and fairness rules; implementation remains ordered later with the Mother Boss.


## 2026-06-07 — Repair rigid-unit minimap installer and implement the single rotation root

- Corrected the V1 installer failure caused by an indentation-sensitive `DrawRotatedRoomsClipped` method anchor.
- The V2 installer uses whitespace-tolerant matching and a safe fallback insertion point.
- Replaced active per-room and per-marker drawing calls with one `DrawRigidRotatedMapContent` call.
- The full map drawing is transformed once through `GUIUtility.RotateAroundPivot`.
- Rooms, outlines, walls/openings, player marker, and horse marker preserve exact relative coordinates.
- The minimap frame, title, footer, and clipping group remain fixed.
- The original `GUI.matrix` is restored in a `finally` block.
- Preserved four cardinal targets, diagonal boundary hold, shortest-angle `SmoothDampAngle`, mid-turn retargeting, maximum speed, and exact completion snap.
- Added `BDMinimapRigidRotationQA` to the existing `TEST EVERYTHING` flow.
- Added the focused canonical design contract `MINIMAP_RIGID_UNIT_ROTATION.md`.
- No scene, camera setting, movement, combat, damage, portal, hazard, or procedural-generation value was changed.
- The next ordered item after real acceptance remains `C03.23A`.


## 2026-06-07 — Remove false Pet marker requirement from mounted-intro QA

- Recorded the exact `TEST EVERYTHING` blocker from `2026-06-06T21:47:46.9476910Z`.
- The blocker was not a runtime failure: `BDMountedRunIntroQA` required the internal marker `PET_RUN_INTRO_CANCEL_CONTRACT` inside the Pet runtime file.
- Removed the synthetic marker from the QA contract.
- Kept validation of the real mounted-intro Pet behavior: input lock gate, `CancelPetInteractionForRunIntro`, hold cleanup, and emergency coroutine cancellation.
- Confirmed the undefined `CancelInteraction()` call remains removed.
- No runtime behavior, Pet timing, key binding, horse movement, scene, portal, combat value, minimap, hazard, or requirement was changed.
- `TEST EVERYTHING` must be rerun before Play Mode acceptance or commit.


## 2026-06-07 — Make player projectiles AoE against enemies and destructible blockers

- Replaced the previous rule that bullets could not damage destructible blockers.
- Every normal player shot is now specified as one logical AoE event with multiplier `1`.
- Every charged shot uses multiplier `N`, where `N` is the exact number of bullets actually consumed at fire time.
- Each unique enemy in the valid AoE receives base ranged damage × `N`.
- Each unique destructible in the valid AoE receives `N` structural hits; a normal shot therefore deals exactly one structural hit.
- Direct collision and AoE overlap use one logical-shot identifier, preventing duplicate damage to targets with several colliders.
- One shot may damage every different enemy and destructible reached by its AoE, each once.
- Closed walls and closed doors block AoE propagation.
- Charged-shot damage numbers show one final total per enemy rather than one number per consumed bullet.
- Updated blocker-family examples, legal doorway reachability, the deterministic blocker test matrix, ranged-combat requirements, snapshot note, and latest compilation-blocked QA truth.
- This is a future combat/destructible requirement and does not interrupt the current compilation and mounted-run-intro verification gate.


## 2026-06-07 — Repair undefined Pet cancellation call in mounted intro

- Recorded the exact Unity compiler error: `BDHorseExhaustedFollowAndPetInteraction.cs(169,17): CS0103`.
- Confirmed the mounted-intro Pet gate called `CancelInteraction()`, but that method does not exist.
- Replaced it with `CancelPetInteractionForRunIntro()`.
- Pending Pet hold state is cleared immediately.
- An active Pet coroutine uses the existing emergency-cancel path, preserving its established restoration of visual rotations, player controls, horse external control, and flee components.
- Added regression coverage to `BDMountedRunIntroQA`.
- No Pet timing, key binding, horse movement, combat value, portal behavior, scene content, or unrelated local change was modified.
- Compilation and `TEST EVERYTHING` remain the acceptance gate.


## 2026-06-07 — Split mounted run intro by fresh/victory versus death restart

- Corrected the mounted entrance rule: it plays only for a fresh New Game or a non-death victory/cinematic restart.
- For the current progression definition, any new-run return after a cinematic beyond the doorway following the first boss counts as victory/cinematic and receives the mounted entrance.
- Death always takes precedence over a death cutscene or presentation.
- Death -> New Game now skips mounting, doorway repositioning, ride animation, and cinematic camera zoom.
- On death restart, the freshly loaded player remains unmounted at the authored start-room spawn point with the normal gameplay camera.
- Held input during death-restart loading is still discarded until release, but there is no arbitrary timer.
- The pending death reason is consumed only by a real gameplay scene containing player, horse, and minimap-room metadata; menus and cinematics cannot consume it.
- Updated mounted-intro QA contracts, the focused design document, the current snapshot, and blocker truth.
- The supplied automated report remains compilation-blocked and still does not include the underlying Console compiler lines.


## 2026-06-07 — Add destructible path blockers and rigid-unit minimap correction

- Added a future C10 destructible path/doorway blocker system with exact structural-hit rules, staged destruction, collision/nav cleanup, horse/enemy path behavior, procedural safety, and deterministic QA.
- Approved eight initial biome-authentic blocker families: pots/urns, tall grass/reeds, thorn bramble, vines/roots, wooden barricades, sand/rubble piles, cracked boulders, and crystal/ice/mineral growth.
- Added a comfortable normalized-depth ramp that increases both the chance of blocker rooms and the number of blocker groups per room while protecting the start, mounted entrance, bosses, rewards, hazards, and required routes.
- Added the required near-spawn isolated test room/harness and post-approval cleanup rule.
- Recorded that the existing minimap direction animation failed visual acceptance because its nodes/elements deform and flicker.
- Added C11.13B: all minimap content rotates under one stable parent/root as a rigid unit; no independent node/corridor/icon transforms are animated.
- C11.13B is queued as an immediate cross-cutting regression after the current compilation and mounted-run-intro acceptance gate, before returning to C03.23A.
- Updated the single QA truth: the latest supplied TEST EVERYTHING report is blocked by `UNITY_SCRIPT_COMPILATION_FAILED`; the supplied report does not include the actual Console compiler error lines.


## 2026-06-07 — Implement mounted run intro and doorway portals

- Added the persistent `BDMountedRunIntro` scene-load coordinator.
- The child begins already mounted and the horse is externally driven through the start room's legal entrance over `2.25s`.
- Added a modest `0.48s` camera zoom-in and `0.72s` return to the original gameplay projection.
- Existing camera follow/orbit drivers are temporarily disabled and restored without replacing their authored configuration.
- Gameplay input is discarded throughout loading and the cinematic; after the animation, control opens immediately when all held gameplay inputs are physically released.
- Added explicit mounted-intro APIs to `BDHorseController` so the rider remains mounted when external cinematic control ends.
- Player movement, melee, ranged/charged state, landing-attack classification, horse input, and Pet interaction are gated during the intro.
- Player damage and enemy proximity/attack telegraph spawning are blocked during the intro.
- Existing red/orange transient floor telegraphs are removed on scene load and throughout the cinematic.
- Added animated opaque two-sided entrance/exit light portals with luminous frames and moving light bands; portal objects have no colliders.
- Portal placement uses `BDMinimapRoom` open-side metadata and prefers external openings rather than creating passages through closed walls.
- Added `BDMountedRunIntroQA` to the existing single `TEST EVERYTHING` flow and added the focused design contract.
- No attack damage, cooldown, horse health, hazard damage, quicksand requirement, or unrelated gameplay value was changed.
- The next ordered item remains `C03.23A` only after real Play Mode acceptance.


## 2026-06-06 — Clarify horse and mounted quicksand behavior

- Replaced the unresolved horse/mounted quicksand placeholder with an approved rule.
- The horse treats quicksand exactly through the same established obstacle/hazard pipeline used for lava, holes, and the other environmental obstacles.
- Horse pathing, safe-spot selection, follow, flee, retreat, mounted movement, and recovery may not cross or end inside quicksand.
- Approaching quicksand uses the existing stop/back-away/alternate-route behavior.
- Accidental contact uses the existing horse hazard-safe recovery and restores both horse and rider without stale control/flee state.
- While mounted, the horse owns the obstacle interaction; the rider does not independently enter the player sinking mini-game.
- After dismounting, the player uses the normal player quicksand contact, damage, sinking, jump-escape, failure, and respawn rules.
- The current implementation order remains unchanged.


## 2026-06-06 — Add quicksand hazard and floating RPG damage-number requirements

- Added quicksand as a future room hazard/obstacle with the same physical entry possibilities as lava.
- Quicksand deals exactly `4` damage every `0.50s` only while the player is physically touching, standing on, or submerged in the quicksand.
- A player who is airborne above the quicksand and no longer touching its surface/volume receives no quicksand damage and no new sinking progress.
- Added progressive sinking, jump-based escape, increased escape difficulty at greater depth, half-body failure threshold, hole-equivalent fall damage, and safe respawn requirements.
- Added duplicate-tick prevention, safe-point restrictions, entry/exit reset rules, telegraphing, mobile/desktop parity, and a deterministic near-spawn test lane.
- Approved horse/mounted quicksand behavior: quicksand uses the same horse obstacle/hazard classification, avoidance, retreat, path rejection, safe-point validation, and recovery pipeline as the existing environmental obstacles.
- Added professional world-space RPG damage numbers for real health loss: one readable animated number per logical damage event, no number for blocked/parried/invulnerable/no-damage events, overlap control, pooling, and mobile readability.
- These are later requirements and do not interrupt the current earlier blocking restart/run-intro regression work.


## 2026-06-06 — Add biome-authentic boundaries, portal doors, and mounted run intro requirements

- Added a future environment requirement to replace generic artificial walls with natural, biome-authentic boundaries such as rocks, boulders, cliff faces, cave walls, vegetation, trees, mountains, roots, ruins, and fences according to the current biome.
- Added requirements for readable collision, navigation, minimap boundaries, camera safety, procedural variation, transition blending, and performance-safe reuse.
- Added a door presentation requirement: open entrances and exits use an attractive light/portal surface that hides what is behind the doorway while matching the game’s visual language.
- Added the revised run-start direction: the child starts mounted, rides through the entrance in a short cinematic with a modest camera zoom-in, the camera returns to normal at the end, and gameplay inputs open only after the entrance animation completes.
- The mounted intro replaces an arbitrary invisible input delay; inputs pressed during loading must be discarded and may not trigger an attack when control opens.
- The red floor artifact visible immediately after death -> New Game remains a blocking early-run regression and must be removed before the run intro is accepted.
- The current blocking work remains earlier than these later environment-art tasks.


## 2026-06-06 — Block New Game UI click from leaking into combat

- Recorded that the hit-like effect still occurred after the feedback-only repairs.
- Identified the remaining root cause: the left mouse click used on `New Game` reached both combat readers on the first gameplay frame.
- Added a release-aware combat-input quarantine with a `0.20s` minimum and `1.50s` maximum.
- `BDPlayerCombat` clears pending light, ranged, and charged state while quarantined.
- `BDPlayerMeleeEnhancer` clears its buffer and cannot spawn a leaked slash or landing strike.
- `BDPlayerAirStateTracker` cannot classify startup settling as a landing attack during quarantine.
- Migrated the versioned early-run design file to `EARLY_RUN_REGRESSION_REPAIR.md` and removed the obsolete V2 design copy.
- Extended the existing QA helper; no new QA menu command was created.
- No horse, hazard, damage, cooldown, or scene values were changed.


## 2026-06-06 — Synchronize stale horse clean-start QA contracts

- Recorded the supplied `TEST EVERYTHING` result from `2026-06-06T19:54:38.9494250Z`: `BLOCKED`, `5` blockers, `0` warnings, `0` info.
- Confirmed all five blockers came from superseded expectations in `BDHorseCleanRunStartQA`, not from new compiler failures.
- Updated the helper from `2.50s` / `0.55s` to the implemented `3.50s` / `1.50s` startup contract.
- Replaced obsolete `CanReactToCombatThreat` and exact formatting expectations with the current `BDHorseLocalThreatUtility.HasLivingThreatNear` contract.
- Added regression checks for direct static `ResetHorse` use and qualified `UnityEngine.Object.FindObjectsByType` calls.
- No runtime code, scene content, horse health, startup timing, threat radius, hazard placement, or unrelated local changes were modified.
- Early-run behavior remains `VERIFY` until automated QA and all three Play Mode checks pass.


## 2026-06-06 — Repair V2 Unity compilation errors

- Recorded the supplied `TEST EVERYTHING` result from `2026-06-06T19:44:25.7374870Z`: `BLOCKED`, `1` blocker, `0` warnings, `0` info.
- Fixed `CS0176` in `BDHorseCleanRunStartGuard.RegisterHorse`: static `ResetHorse` is now called directly.
- Fixed both `CS0103` errors in `BDHorseLocalThreatUtility`: static scene queries now call `UnityEngine.Object.FindObjectsByType<T>`.
- Extended `BDEarlyRunRegressionRepairQA` so these exact source regressions cannot silently return.
- No gameplay values, startup durations, radii, hazard positions, scene objects, or unrelated local changes were modified.
- The early-run behavior repair remains `VERIFY` until compilation, TEST EVERYTHING, and focused Play Mode checks pass.


## 2026-06-06 — Early-run regression repair V2 after failed Play Mode verification

- Recorded that the first fixes did not resolve the three observed behaviors.
- Removed damage shake/audio from `BDHealth.SetMaxHealth`; configuration/refill is no longer treated as a hit.
- Added explicit suppression/reset entry points to camera shake, one-shot game-feel audio, and damage flash for the `1.50s` new-run window.
- Strengthened horse startup to repeat full-health and transient-state reset through `1.50s`, with `3.50s` calm/protection.
- Replaced scene-global unfinished-encounter flee detection with same-room, in-range living-combatant detection.
- Moved the generated hazard test lane from 5.5–12m candidates to 16–28m candidates, with at least 14m spawn clearance and 18m/24m fallbacks.
- Added automated regression contracts to the existing single `TEST EVERYTHING` flow.
- Work remains before C07.16A; after this repair passes, continue with C03.23A.


## 2026-06-06 — Horse full-health clean start and local-threat flee

- Added `BDHorseCleanRunStartGuard` before scene loading with a `2.50s` calm/protection contract and `0.55s` startup maintenance window.
- The guard resets full health, recent-hit/buck/healing locks, controller motion/state, flee state, and hazard retreat/polling before startup AI can restore stale behavior.
- `BDHorseController` now exposes explicit startup-calm and real-local-threat contracts and rejects safe-spot commands during startup.
- `BDHorseCombatFleeController` and `BDHorseReliableFleeMotor` no longer treat every unfinished `BDRoomEncounter` in the scene as danger; they react only to a living combatant near the horse or player after startup calm.
- `BDHorseHazardSafety` now clears retreat/recovery state and delays startup polling through the calm window.
- Runtime horse repair registers newly repaired horses with the active clean-start guard.
- Added focused design documentation and `BDHorseCleanRunStartQA` to the existing single `TEST EVERYTHING` command.
- C03.44A remains pending user Play Mode acceptance; the next earlier blocker after this item is C03.59, followed by C03.23A, then the saved C07.16A resume point.


## 2026-06-06 — Clear stale hit feedback on New Game reload

- Added `BDNewRunFeedbackReset.cs`, installed automatically before scene loading and retained across scene reloads without requiring scene edits.
- Added a `0.45s` real-time startup suppression window that restores normal time/audio state, resets known transient parry/game-feel systems, and repeatedly clears strictly identified old hit/damage/impact particles, audio, flashes, and camera-shake state.
- Added explicit `IsFeedbackSuppressed` and `RunStartFeedbackResetRequested` hooks for future feedback systems.
- Added `BDNewRunFeedbackResetQA.cs` to the existing single `TEST EVERYTHING` flow.
- Added the focused design/verification contract at `Assets/_Project/Design/Runtime/NEW_RUN_FEEDBACK_RESET.md`.
- No player health, scene layout, enemy behavior, horse behavior, hazard behavior, or New Game routing was changed.
- Next ordered blocking item after acceptance is `C04.20A`; the saved feature resume point remains `C07.16A`.


## 2026-06-06 — Canonical workflow and near-spawn test-harness policy

- Added root-level `DEVELOPMENT_WORKFLOW.md` as the exact process contract without creating a second progress source of truth.
- Recorded mandatory ZIP delivery, idempotent installer/validator testing, Unity verification, and post-PASS local Git handoff.
- Added the permanent rule that every new testable gameplay element must be immediately reachable in a clearly identified near-spawn test room/lane/arena/portal.
- Added continuous cleanup: accepted test objects must be removed, deliberately retained as isolated development harnesses, or converted to production rather than left for final cleanup.
- Recorded the pending earlier blocking regressions: death -> New Game stale hit effect, horse startup injury/phantom flee, unsafe spawn-adjacent lava/hole tests, and the vertical normal-slash landing-attack presentation.
- Clarified that mini-boss rooms remain escapable and do not use mandatory boss-room locking.
- Saved the return point at C07.16, with C07.16A as its immediate testability prerequisite.


## 2026-06-06 — Shorten accepted hole/chasm fall timing to 2.05s

- Recorded the supplied `TEST EVERYTHING` PASS from `2026-06-06T18:31:23.9561590Z`: `0` blockers, `0` warnings, `0` info.
- Recorded the user's Play Mode acceptance that the current hazard, lava, Pet, and horse-cue behavior is excellent.
- Shortened only `holeFallDuration` from `2.25s` to `2.05s` — a `0.20s` reduction.
- Preserved `holeFallSpeed = 4.60f`, progressive acceleration up to `1.35x`, hole damage, recovery target, repeated-fall protection, lava behavior, and all Pet behavior.
- Updated the runtime source plus both the main and dedicated QA regression contracts to the same `2.05s` source of truth.
- The next saved item is `C07.16`; this is a category transition, so work pauses for additions or changes before entering it.


## 2026-06-06 — Repair stale long-hole QA speed contract

- Recorded the supplied `TEST EVERYTHING` result from `2026-06-06T18:27:33.8935110Z`: `BLOCKED`, `1` blocker, `0` warnings, `0` info.
- The only blocker was `LONG_HOLE_FALL_CONTRACT_MISSING`; the main QA window still required obsolete source token `holeFallSpeed = 2.35f`.
- Updated that QA regression anchor to the implemented C03.57 value `holeFallSpeed = 4.60f`.
- No runtime hazard behavior, timing, damage, lava behavior, Pet behavior, scene data, or additional QA command was changed.


## 2026-06-06 — Hazard feel tuning, Tab Pet default, and QA repair

- Kept hole/chasm recovery duration at `2.25s` while raising actual downward speed to `4.60` and adding progressive acceleration up to `1.35x`.
- Reduced lava horizontal throw toward `80%` of the previous displacement, accepting the shorter destination only when the existing grounded and hazard validators approve it.
- Changed the Pet desktop default from `P` to `Tab`, migrates legacy serialized `P`, and made both Pet labels display the active binding dynamically.
- Repaired the dedicated Pet QA contract so it validates dynamic binding text instead of requiring obsolete hard-coded `P  PET`.
- Verification remains in the existing `Boredom And Dungeons -> TEST EVERYTHING` command.


## 2026-06-06 — Cue stacking, repeated-hole walking guard, and rebinding requirement

- Repositioned the healing cue to a lower-left screen-relative slot and the Pet cue to a higher-right screen-relative slot, reduced both visual scales, added toward-camera depth offsets, and assigned distinct renderer sorting orders so they do not hide behind the horse health display or one another.
- Added C03.56 and code support that clears stale dodge/jump/forced-gap permission after external hazard recovery and suppresses forced-gap reclassification for `0.55s`.
- Added the future C02.09A/C11.35A key-rebinding requirement: Settings persistence, conflict handling, default reset, and immediate refresh of all UI/world prompt labels.
- Extended the existing dedicated `BDHorseExhaustedFollowPetQA.cs` path; no additional QA menu command was created.


## 2026-06-06 — C04 exhausted follow and Pet implementation

- Added `BDHorsePetAvailabilityIndicator.cs`: a pulsing amber world-space heart and `P PET` label appears when Pet is usable, shows hold percentage during the `0.65s` long press, and displays `PETTING` during the animation; the existing bottom-right button remains available for pointer/touch input.
- C04 exhausted-follow/Pet automated validation is isolated in `BDHorseExhaustedFollowPetQA.cs`; the existing `TEST EVERYTHING` window contains one call only, avoiding duplicate QA commands and large injected method blocks.
- Recorded the supplied `TEST EVERYTHING` automated PASS from `2026-06-06T17:00:51.3087910Z`: Unity `6000.0.76f1`, `0` blockers, `0` warnings, `0` info.
- Preserved C04.31A/C11.13A as VERIFY because focused Play Mode confirmation was not explicitly supplied.
- Implemented C04.32–C04.36: the zero-health horse preserves nearby fainted behavior, begins a delayed `14m` exhausted follow, stops by `8m`, moves at `20%` reference speed, uses existing hazard filtering, and attempts a validated near-player fallback only after being stuck while far away.
- Implemented C04.37–C04.39: contextual `Pet` at `2.25m`, short press for player-pets-horse, `0.65s` hold for horse-nuzzles-player, visible progress, exclusive outcomes, input locking, emergency cancellation, and no health/stat/progression effect.
- Added explicit external-control ownership to `BDHorseController` so exhausted follow and Pet cannot fight normal movement, mounting, fleeing, or healing state.
- Integrated scene serialization, runtime fallback repair, automated source contracts, scene validation, and expanded Play Mode wording into the existing `TEST EVERYTHING` command.
- Renamed the canonical design document to remove the version suffix; no duplicate V1 copy remains.
- Kept the observed startup horse damage and flee-with-no-local-enemy issue deferred under existing C04.13/C04.19/C04.20, as requested.


## 2026-06-06 — Horse retreat, minimap transition, and single-source synchronization

- Implemented C04.31A in the local install package: the horse no longer waits in place for one second near a detected hazard; it chooses a validated away direction, backs away roughly `2.6m`, and then returns to normal mounted or AI behavior.
- Implemented C11.13A in the local install package: the minimap still selects only `0/90/180/270` degree targets, but uses `Mathf.SmoothDampAngle` and shortest-angle settling instead of an immediate snap.
- Integrated both contracts and their Play Mode wording into the existing `Boredom And Dungeons -> TEST EVERYTHING` command.
- Recorded C05.26 so final normal-enemy composition, placement, and encounter order vary by deterministic run seed; current editor-generated scene randomness is not treated as final per-run variation.
- Removed the duplicate section-1 current snapshot and made the marked snapshot at the top of this file the only live Previous / Current / Next pointer.
- Removed known superseded or duplicate progress/reference documents. Their active requirements are preserved in this authoritative file.
- Verification remains open until the package is installed in the real Unity project and passes compilation, TEST EVERYTHING, and Play Mode.


## 2026-06-06 — Categorized authoritative plan

- Replaced the flat numeric work order with dependency-ordered categories.
- Preserved the complete legacy 36-stage roadmap through a cross-reference.
- Added permanent earlier/current/later request handling.
- Added mandatory category-transition pauses and user review.
- Recorded mobile landscape, finger-trace movement, and tap-enemy attack requirements.
- Added normal enemy roster and escape-blocking/pursuit behavior.
- Recorded the Mother Boss as `RECOVERY REQUIRED` without inventing details.
- Preserved the current resume point at the Stage 16–17 boss framework work.
- Kept the latest code baseline in `VERIFY` until Unity QA is rerun.
- Added `Assets/_Project/Design/Roadmap/README.md` to mark V128 as historical and point to this file.

## 2026-06-06 — Multi-route map, hazards, Mother Boss, and secret ending recovered

- Recovered and recorded the requirement for at least 3 meaningful map solution routes, targeting 4 when space allows.
- Added route separation, intentional split/merge/re-split topology, and delayed convergence requirements.
- Added inaccessible 1–4-room-scale regions using mountains, lakes, chasms, holes, columns, and obstacles.
- Added exact player hazard rules: 15 damage for hole/chasm falls and 10 damage for lava contact.
- Added last-safe-position recovery and player-plus-horse recovery requirements.
- Added horse avoidance of chasms/lava and no-damage horse recovery.
- Recovered the complete Mother Boss position as a secret post-ending encounter unlocked by the full collectible set.
- Recorded all four anger phases, durability multipliers, attacks, clothing occlusion, anti-stun/fairness rules, and Phase-4 task race.
- Added Phase-4 task suggestions that use existing movement, melee, shooting, pickup-trigger, and enemy-combat mechanics.
- Added Mother loss and victory cinematic branches and full-run restart behavior.
- Added the scream pose requirement: Mother’s arms extend farther to the sides while screaming.
- Inserted earlier C03/C04 hazard work before returning to the saved C07 resume point.



## 2026-06-06 — Single authoritative tracker, Battery clarification, Mother decisions, and horse interaction

- Re-established `/PROJECT_STATUS.md` as the only complete live progress list.
- Added Previous / Current / Next directly to the authoritative snapshot.
- Added the requirement that every material commit updates this file and that one-click QA enforces the rule.
- Recorded that Batteries are physical map collectibles and never chest rewards; approach triggers one fair but difficult guardian encounter.
- Finalized Mother’s Patience bars, phase transitions, attack interactions, stun timings, clothing schedule, Danger task structure, arena layout, success/failure flow, and horse exclusion gate.
- Added zero-health exhausted horse follow and short/long `Pet` interactions.
- Recorded the latest automated TEST EVERYTHING PASS: 0 blockers, 0 warnings, 0 info at 2026-06-06T03:22:56.8937980Z.

## 2026-06-06 — Unified QA classifier correction

- Recorded TEST EVERYTHING PASS at `2026-06-06T03:42:01.6743540Z` with `0` blockers and one `MULTIPLE_RUNTIME_INSTALLERS` warning.
- Determined that the warning was caused by the generic `Bootstrap` keyword matching `BDEnemyBootstrap`, which is intentionally attached once to each enemy instance.
- Updated the classifier to exempt only `BDEnemyBootstrap` from scene-level multiplicity warnings while preserving checks for other Installer, Bootstrap, RuntimeRepair, and AutoSetup types.
- Kept same-object duplicate component detection active through the existing scene scan.
- Preserved `TEST EVERYTHING` as the only QA command.
- Consolidated progress tracking into `/PROJECT_STATUS.md` and queued a clean rerun targeting `0` warnings.

## 2026-06-06 — C01 Git hygiene rule fix

- Previous: C01 runtime-installer classifier correction completed.
- Current: added the exact `.gitignore` rule required by `TEST EVERYTHING`:
  `/PROJECT_STATUS_CURRENT_V*.md`
- QA truth before this fix: BLOCKED with 1 blocker and 0 warnings because the rule was missing.
- Next: rerun `Boredom And Dungeons -> TEST EVERYTHING` and require 0 blockers / 0 warnings.
- Delivery rule: every future extraction command must delete the downloaded ZIP only after successful extraction.

## 2026-06-06 — C01 unified QA completed

- Previous category: `C01 — Project stability, QA, validation, and repository health`.
- Completed result: `TEST EVERYTHING` passed in Unity `6000.0.76f1`.
- Generated UTC: `2026-06-06T03:51:35.3691270Z`.
- Automated result: `0` blockers, `0` warnings, `0` info.
- `TEST EVERYTHING` is the only QA entry point required.
- Camera/Minimap regression and repository-hygiene checks are part of the same test.
- Current category: `C03 — Player movement, combat, damage, weapons, and hazards`.
- Current item: `C03.46 — validated last-safe-position tracker`.
- Next after C03: `C04 — horse hazard avoidance and recovery`.
- Saved later resume point: `C07.16 — wire the shared boss framework into one playable encounter`.
- Delivery rule: every ZIP command deletes the downloaded ZIP only after successful extraction.

## 2026-06-06 — Tracker transition repair after C01 PASS

- Previous: C01 unified `TEST EVERYTHING` completed successfully.
- QA result: `0` blockers, `0` warnings, `0` info.
- Current: repaired the single authoritative progress tracker after a format-sensitive script failure.
- Next: final QA confirmation and one clean commit, then begin `C03.46`.
- Delivery rule: downloaded ZIP files are deleted only after successful extraction.

## 2026-06-06 — Corrected player/horse hazard rules

- Hole/chasm entry by ordinary grounded walking is rejected without damage.
- A player receives the `15` hole/chasm damage only after entering through a jump, dodge, forced displacement, or mounted exceptional fall.
- Lava applies exactly `10` unavoidable environmental damage through every entry method: walking, jumping, dodging, knockback, forced movement, or mounted entry.
- When player and horse enter any hazard while mounted, both recover separately and the player returns on foot.
- Hazard recovery never restores the mounted relationship automatically.
- The horse proactively filters movement toward lava, holes, chasms, and unsupported ground.
- Horse AI attempts safe steering alternatives before stopping.
- A normal horse walk, follow, flee, return, or mounted movement must not cause a fall.
- Exceptional or scripted horse hazard entry recovers the horse without health loss.
- C03/C04 remain awaiting Unity compilation, `TEST EVERYTHING`, and Play Mode verification.

## 2026-06-06 — Prototype hazard scene integration

- Previous automated result: `TEST EVERYTHING` passed with `0` blockers, `0` warnings, and `0` info.
- `TEST EVERYTHING` now opens the authoritative prototype scene and idempotently creates one test hole/chasm and one test lava volume.
- No additional menu command or QA button is required.
- The installer finds nearby clear grounded positions around the player and avoids placing the two hazards on top of each other.
- The prototype player receives a serialized `BDPlayerHazardRecovery`.
- The prototype horse receives a serialized `BDHorseHazardSafety`.
- Automated QA verifies exactly one hazard root, one hole/chasm volume, one lava volume, trigger colliders, and serialized player/horse safety components.
- Current verification target: the single Play Mode hazard checklist in `TEST EVERYTHING`.

## 2026-06-06 — Robust compiler-state and jump-field QA repair

- `lastJumpStartedAt` is now required exactly once in `BDPlayerController.cs`.
- The check is implemented as an independent scanner and no longer depends on the structure of another hazard-token list.
- `TEST EVERYTHING` blocks and writes a blocker report when `EditorUtility.scriptCompilationFailed` is true.
- The previous stale `PASS` reports remain invalid.
- Cleaned Unity-scene trailing whitespace produced by empty serialized string fields.
- A fresh report is accepted only after Unity completes compilation with no Console compiler errors.

## 2026-06-06 — Lava damages through every entry method

- The hole/chasm walking restriction applies only to holes and chasms.
- Lava has no safe entry method.
- Walking, jumping, dodging, knockback, forced movement, or mounted entry into lava applies exactly `10` unavoidable environmental damage.
- Lava recovery still returns the player to a validated non-lava safe point.
- Mounted lava recovery returns the player on foot and restores the horse separately without horse health loss.
- `TEST EVERYTHING` wording now explicitly covers every lava entry method.

## 2026-06-06 — Precise hole and lava contact repair

- Ordinary player movement is filtered before `CharacterController.Move`; a normal walk cannot cross into a hole/chasm footprint.
- The movement filter preserves safe-axis sliding around the hole instead of freezing all movement.
- A recent jump or dodge bypasses the walk filter and allows intentional hole entry.
- Hole/chasm triggers now sit below the surface instead of extending high above it.
- An emergency horizontal-footprint/depth check applies `15` damage and safe recovery if a falling player misses the trigger callback.
- Lava triggers are thin and aligned to the visible surface.
- Lava damage requires actual collider overlap at the lava surface; passing above or beside lava does not apply damage.
- Every real lava contact still applies exactly `10` damage regardless of walking, jumping, dodging, knockback, forced movement, or mounted entry.
- The previous automated PASS remains insufficient; this repair requires a fresh Play Mode check.

## 2026-06-06 — Minimap, ground safety, and hazard-transition repair

- Minimap rooms, walls, horse marker, and player marker are clipped to the internal minimap rectangle even while the map rotates.
- The outer minimap panel is clamped to the current screen dimensions.
- Normal voluntary movement cannot leave supported ground or cross a hole/chasm boundary.
- Ground exit is allowed only during a recent jump, dodge, detected knockback, or explicitly notified forced movement.
- Accidental non-intentional falling is returned to the latest validated safe point without damage.
- A hole/chasm fall now has a visible downward fall interval before applying exactly `15` damage and respawning.
- A real lava-surface contact applies exactly `10` damage immediately and animates a short upward arc back to the safe map point.
- Passing above or beside lava without collider contact does not apply damage.
- The failed Knockback V2 installer is superseded by this repair.
- `TEST EVERYTHING` validates minimap clipping, ground support, forced movement, hole fall, and lava bounce.

## 2026-06-06 — Correct hazard QA contract ownership

- The five blockers in the latest report were false QA ownership errors, not runtime failures.
- `FilterPlayerMotion` belongs to `BDHazardVolume.cs`, not `BDHorseHazardSafety.cs`.
- `CheckEmergencyHoleFall` / hole-transition behavior belongs to `BDPlayerHazardRecovery.cs`, not the horse file.
- Prototype trigger geometry tokens belong to `BDPrototypeHazardSceneInstaller.cs`.
- The current player recovery property is `HasRecentIntentionalGapEntry`; the obsolete `HasIntentionalGapEntry` token is no longer required.
- `ScanHazardRecoveryContracts` was normalized so every source file is checked only for its own responsibilities.
- The newer combined minimap/ground/hazard scanner remains active and is not duplicated.

## 2026-06-06 — Respawn safe-point timing and death-loop repair

- Safe points require a longer stable grounded interval before they are committed.
- Safe points are rejected using the full horizontal footprint of holes, chasms, and lava, even when the hazard trigger is physically below the floor surface.
- Safe-point updates are frozen during jump/dodge/knockback intent, hazard transitions, and for a protected period after recovery.
- The recovery component preserves both the latest safe point and the previous safe point.
- A second hazard recovery within the rapid-loop window skips the newest point and uses the older safe point or initial spawn.
- Respawn cannot immediately overwrite the stored point with the recovery location.
- `TEST EVERYTHING` now validates the horizontal recovery clearance, saved-point history, post-recovery lock, and rapid-loop fallback.

## 2026-06-06 — Forward-biased gameplay camera framing

- The gameplay camera now frames slightly more world in front of the player and slightly less behind.
- Default forward bias is `1.65` world units and is smoothed rather than snapped.
- The direction follows current movement first, then current look/aim direction.
- During short idle periods, the last useful forward direction is retained to prevent framing jitter.
- Mounted movement and mounted aim direction are supported.
- The component removes its previous offset before the existing camera-follow logic runs, then reapplies the final bias after camera movement, preventing cumulative drift.
- The bias returns smoothly to zero when player control is disabled.
- The component installs automatically on the active gameplay camera; no additional button or menu is required.
- `TEST EVERYTHING` validates the camera-framing source contract.

## 2026-06-06 — Viewport camera composition and true minimap square

- The previous directional camera bias was removed because it moved the camera in world space and did not guarantee a useful screen composition.
- The player is now targeted at viewport `x = 0.50`, `y = 0.40`: horizontally centered and forty percent up from the bottom of the screen.
- The correction is calculated in the camera's own right/up plane, so it remains stable when the player changes direction.
- The correction is resolution-, aspect-ratio-, field-of-view-, and orthographic-safe.
- The minimap now has a dedicated square whose height excludes both the title and the `Explored` footer.
- `GUI.BeginGroup(localMapRect)` hard-clips all rooms, walls, and markers to that inner square.
- Player and horse markers remain clamped inside the square and can no longer overlap the footer.
- The previously failed scene-installer edit is completed by replacing `CreateHazard` directly rather than searching for one historical geometry layout.
- Start-room cleanup and mouse-sensitivity scene serialization are completed.
- `TEST EVERYTHING` validates the camera viewport target, the true minimap square, and the repaired scene installer.

## 2026-06-06 — Remove duplicate minimap panel resolver

- The active Unity compiler failure was `CS0111`: `BDMazeMinimap` defined `ResolveScreenPanelRect()` more than once.
- Historical errors earlier in `Editor.log` were not part of the latest compiler output.
- Every existing `ResolveScreenPanelRect()` method body is removed using brace-aware parsing.
- Exactly one canonical screen-clamped implementation is inserted.
- Existing viewport-camera composition and inner-square minimap rendering are preserved.
- The package validator requires exactly one method declaration and checks that `OnGUI` still calls it.

## 2026-06-06 — Remove obsolete QA regression anchors

- The latest `TEST EVERYTHING` report compiled successfully but was blocked by five stale source-token checks.
- Scene hazard geometry now uses lava trigger `new Vector3(0f, 0.04f, 0f)` and hole trigger `new Vector3(0f, 0.55f, 0f)`.
- Player recovery now uses `CheckGroundExit`, replacing the obsolete `CheckUnintentionalGroundExit`.
- The minimap now clips inside `GUI.BeginGroup(localMapRect)`, not the obsolete `GUI.BeginGroup(mapRect)`.
- The minimap intentionally no longer uses `GUIUtility.RotateAroundPivot`; mathematical rotation is validated through `RotateMapPoint` and `DrawRotatedRoomsClipped`.
- All legacy scanners were updated globally so an older scanner cannot contradict the current camera/minimap/hazard contract.
- No runtime gameplay behavior was changed by this repair.

## 2026-06-06 — Longer falling and horse jump hazard prevention

- Hole/chasm and intentional off-map falling now lasts `2.25` seconds, more than 2.5 times the previous `0.85` seconds.
- Fall speed is reduced to `2.35` world units per second so the longer sequence remains readable instead of dropping the player far below the camera immediately.
- The horse evaluates the full projected jump path before accepting a jump.
- Every sampled jump-path position is checked against the horizontal footprint of lava, holes, and chasms.
- Every sampled position must also have supported ground; the horse cannot jump off the map.
- The same horizontal hazard-footprint check is used by normal horse movement while airborne, so momentum cannot carry the horse over the guard after takeoff.
- A blocked jump does not consume the jump and reports `horse jump blocked by hole/lava` in the horse debug state.
- The rule applies to mounted horse jumps. Independent horse navigation continues to use proactive movement filtering and cannot enter the same hazards.
- Exceptional external entry still invokes the existing no-damage horse recovery and forces the rider to return on foot.
- `TEST EVERYTHING` validates the longer fall timing, jump-path guard, controller wiring, and horizontal hazard-footprint protection.

## 2026-06-06 — Remove obsolete emergency hole-depth field

- Unity emitted `CS0414` because `emergencyHoleFallDepth` was assigned but never read.
- The field belonged to the older `CheckEmergencyHoleFall` implementation.
- Current hazard behavior uses continuous `CheckActiveHazardContact` and `CheckGroundExit`, so the field has no runtime purpose.
- Removed only the obsolete serialized field; hole timing, damage, respawn, lava behavior, and horse safety are unchanged.
- `TEST EVERYTHING` now reports a warning if `emergencyHoleFallDepth` is reintroduced.

## 2026-06-06 — C04 mounted-hazard partial-install repair V2

- The first C04.29–C04.31 package stopped after updating only `BDHazardVolume.cs` and `BDPlayerHazardRecovery.cs`.
- Root cause: it expected a nonexistent `LastSafePosition` public-property anchor.
- Additional latent incompatibilities were corrected:
  - the actual horse-controller field is `horse`, not `horseController`;
  - the actual safe-point method is `TryRecordSafePoint`, not `RecordSafePosition`;
  - the original horse-safety class had no `TryResolveRecoveryPosition` method, so V2 adds it explicitly.
- V2 continues from the partial state without rolling back or duplicating the already-applied volume/player changes.
- Mounted lava/hole/chasm recovery returns the rider on foot.
- Horse health remains unchanged.
- Player and horse recover into one legal area when possible without overlap.
- Continuous horse hazard polling catches missed triggers and external displacement.
- Latest and previous horse-safe points, rapid-loop fallback, post-recovery grace, and safe-point write locks prevent death/recovery loops.
- Horse follow/flee/return/interaction updates pause while recovery or recovery grace is active.
- `C04.31` remains the active Play Mode acceptance gate before returning to `C07.16`.

## 2026-06-06 — C07.16 playable shared-framework encounter

- The user approved the mandatory transition from C04 to C07 by replying `הבא` after the clean C04 QA result.
- Added one real playable framework encounter to the authoritative prototype-scene generation flow.
- The encounter is deliberately a neutral `Framework Test Boss`, not one of the four C08 archetypes; individual archetype implementation remains blocked until the C07 category gate.
- The installer selects a large non-start room, strongly preferring a distant room without resident combatants, then clears remaining non-player/non-horse combatants from that test room.
- The generated encounter includes:
  - `BDBossEncounterController`;
  - one `BDBossHealthChannel` inside a `BDBossHealthGroup`;
  - `BDBossHealthDamageBridge` connected to a real `BDHealth` target that existing player attacks can hit;
  - `BDBossHealthHud`;
  - `BDBossEncounterRuntimeBindings`;
  - `BDBossArenaTrigger`;
  - a playable boss that moves only while the encounter is `Active`;
  - a readable expanding attack telegraph, avoidable range check, player damage, player-death failure routing, and victory through the shared health group.
- `BDPrototypeHazardSceneInstaller` invokes the C07 installer automatically, preserving the single `TEST EVERYTHING` workflow.
- The C07 installer saves the authoritative scene itself, preventing the generated encounter from existing only in memory.
- `TEST EVERYTHING` validates the source contracts and generated scene hierarchy.
- C07.16 does not claim C07.17 complete: the existing bridge is used for this one encounter; generalized damage-source and life-state routing remains next.

## 2026-06-06 — Guardian same-room spawn safety and corrected C07.16 installer

- The first C07.16 package stopped in preflight before changing project files because the Stage 16 bridge path was checked without the `Stage16/` folder.
- Corrected all Stage 16 prerequisite paths and the `BDPrototypeHazardSceneInstaller.TryEnsureInstalled` integration point.
- Collectible guardian encounters now require the player to be in the collectible's own `BDMinimapRoom`; trigger radius can no longer activate through a wall from an adjacent room.
- Every generated candidate is clamped to an inset inside that room and rejected as a preferred candidate when a solid wall blocks the path from the collectible.
- The fallback is also constrained to the same room, so a guardian can never be forced into the room on the other side of the wall.
- Added source-contract checks to `TEST EVERYTHING`.
- C07.16 remains `IMPLEMENTED / VERIFY` until Unity compilation and Play Mode pass.

## 2026-06-06 — Deterministic healthy horse start and C07 QA repair

- The horse health component already filled health during `Awake`, so a visibly reduced bar at the start meant damage was being applied immediately afterward rather than being serialized as a partial starting value.
- Added a short startup damage-protection window and an explicit clean-start reset that clears recent-hit/buck state and starts at maximum health.
- The horse now resolves its beside-player start position against walkable ground, lava, holes, chasms, walls, and solid overlap.
- Added a startup calm window. During it, combat notifications cannot send the horse to its safe spot.
- Combat awareness is now local to a living enemy near the horse or player.
- Dormant boss encounters do not count as danger.
- `ForceDismountForCombat` ignores remote combat when no nearby living enemy exists.
- Corrected the stale C07.16 QA token from `ApplyUnavoidableDamage` to the actual `ApplyDamage` call.
- Added clean-start regression contracts to `TEST EVERYTHING`.

## 2026-06-06 — Global gameplay-model shadow policy

- Added an explicit global shadow policy before continuing C07.
- Player, horse, enemies, mini-bosses, bosses, batteries, Game Boy, cartridge/tape, collectibles, pickups, quest items, and central interactables always cast and receive basic shadows.
- Required gameplay shadows are never disabled by the optional performance budget.
- Secondary non-decoration gameplay models use distance- and count-budgeted shadows.
- Floors, walls, ceilings, terrain, backgrounds, UI, minimap, VFX, particles, labels, telegraphs, lava, holes, chasms, hazards, and general decoration are not automatically promoted to shadow casters.
- Default optional budget: 22 world units, 28 renderers, 0.35-second refresh, 2.5-second dynamic discovery.
- Dynamically spawned gameplay models are periodically discovered.
- The policy changes only Renderer shadow settings and preserves all models, materials, effects, lights, animation, colliders, and gameplay content.
- Added `GAMEPLAY_SHADOW_POLICY_V1.md` as the permanent design contract.
- Integrated installation and validation into the existing one-button `TEST EVERYTHING` workflow.

## 2026-06-06 — Main menu, settings, pause, and result routing

- Added a main screen before gameplay with:
  - `START GAME` / `START NEW GAME`;
  - `SETTINGS`;
  - desktop `QUIT`.
- Added a settings overlay with persistent:
  - graphics quality;
  - fullscreen/windowed mode;
  - VSync;
  - 30/60/120/unlimited target FPS;
  - master, music, and SFX volume;
  - mouse sensitivity;
  - camera-shake intensity;
  - reset defaults.
- Added Escape pause flow with Resume, Settings, and Main Menu.
- Player death is intercepted by `BDGameFlowSignals` before legacy `Died` listeners can automatically reload the scene.
- Defeat and victory show the main screen immediately and pause the current run.
- The scene reload occurs only after the player explicitly selects `START NEW GAME`; the new scene auto-starts after loading.
- Added explicit victory APIs and `BDGameCompletionMarker` for the future final Mother-boss ending.
- Final Mother-boss integration rule:
  - loss cutscene finishes with the player saying `I'm bored`, then calls `ShowDefeat`;
  - victory colored-light ending finishes, then calls `ShowVictory`.
- Connected mouse sensitivity to `BDPlayerController`.
- Connected camera-shake intensity to `BDGameFeelEvents`.
- Connected SFX volume to `BDGameFeelAudio`.
- Added runtime routing for ordinary music/SFX `AudioSource` components.
- Corrected the stale hazard QA rule: player lava/hole damage intentionally uses `ApplyUnavoidableDamage`, not `ApplyDamage`.
- Installer scans legacy result methods and replaces automatic result-scene loads with menu routing when safely identifiable.
- Added `MAIN_MENU_SETTINGS_RESULT_FLOW_V1.md` as the permanent product contract.
- Integrated installation and validation into the existing one-button `TEST EVERYTHING` workflow.

## 2026-06-06 — C09 correction: cinematic-first endings and non-verbal completion relic

- Removed all result wording from the main menu:
  - no `DEFEAT`;
  - no `VICTORY`;
  - no `I'M BORED`;
  - no `START NEW GAME`.
- The main-menu title, subtitle, and `START GAME` button remain unchanged after every run.
- Ordinary player death returns directly to the unchanged main menu without an automatic scene reload.
- Endings with cinematics now use an explicit sequence ownership contract:
  - call `BeginResultSequence` before the ending starts;
  - player death during that sequence is consumed without opening the menu or invoking legacy reload listeners;
  - call `ReturnToMainMenuAfterSequence` only after the ending-door-without-items sequence or Mother-loss cutscene finishes;
  - call `CompleteMotherVictorySequence` only after all Mother-victory cinematics and the final colored-light ending finish.
- Added Timeline/Animation-event compatible methods to `BDGameCompletionMarker`.
- Mother victory stores a permanent progression flag.
- The only permanent main-menu change is non-verbal:
  - a small Game Boy appears;
  - its cartridge is inserted;
  - its screen emits a softly animated version of the colored ending light;
  - a few colored pixels drift around it;
  - no completion words, badge text, trophy text, percentage, or explicit statement is shown.
- The relic remains visible across future launches.
- Loading still occurs only after the player explicitly presses the unchanged `START GAME` button.
- Added `MAIN_MENU_SETTINGS_RESULT_FLOW_V2.md` as the corrected permanent contract.

## 2026-06-06 — BBH operating-system boot intro and installer stability repair V2

- Added a black boot-intro screen shown once per application launch before the main menu.
- `BBH` is centered on X and positioned at 60% of screen height from the top.
- Letters animate strictly one after another. A later letter cannot begin until the previous letter finishes.
- Each letter uses a polished depth entrance:
  - very small distant start;
  - opacity and brightness ramp;
  - seven-layer perspective trail;
  - slight rotation from depth;
  - controlled overshoot and settle;
  - edge highlight, restrained glow, and final light sweep.
- The completed mark holds briefly and fades into the unchanged main menu.
- Scene reloads caused by `START GAME` do not replay the intro during the same application session.
- Added `BBH_BOOT_INTRO_V1.md` as the permanent visual/behaviour contract.
- Repaired the `B&D Main Menu And Settings` root:
  - removes stale missing MonoBehaviour references recursively;
  - restores `BDMainMenuFlow`;
  - restores `BDSettingsAudioRouter`;
  - installs `BDBBHBootIntro`.
- Corrected nested scene-save ownership:
  - `BDC07PlayableBossEncounterInstaller` no longer calls `EditorSceneManager.SaveScene`;
  - it marks the scene dirty and lets the top-level `TEST EVERYTHING` flow save once.
- Added regression checks for the intro, missing-script repair, exact placement, sequential animation contract, and forbidden nested scene save.

## 2026-06-06 — BBH intro validation wording fix V4

- The V3 installer completed successfully and installed the BBH intro.
- Its external validator then failed only because the design sentence
  `60% of the screen height` was wrapped across two Markdown lines.
- Normalized that sentence to one physical line.
- No gameplay, animation, scene, prefab, material, or renderer behaviour
  changed in this repair.
- Future package validation normalizes whitespace before checking the
  placement contract, so ordinary Markdown line wrapping cannot cause the
  same false failure again.

## 2026-06-06 — Scene YAML parser repair and truthful QA

- Unity reported `Expected closing '}'` near line 54904 in
  `02_CleanCore_MazePrototype.unity`.
- The existing automated suite still reported PASS because it validated source
  contracts but did not lexically validate the serialized scene file.
- Added a safe local repair that closes one uniquely identifiable malformed
  inline Unity YAML mapping near the reported parser line.
- The repair refuses to write when the candidate is ambiguous.
- Added `ScanSceneYamlIntegrityContracts` to `TEST EVERYTHING`.
- `TEST EVERYTHING` now blocks on:
  - unbalanced inline `{...}` mappings in the scene;
  - unresolved Git conflict markers in the scene;
  - a missing prototype scene file.
- The QA report showing `0 blockers / 0 warnings / 0 info` describes the old
  automated checks only and does not override Unity's parser failure.

## 2026-06-06 — BBH intro visual vertical placement fix V1

- User feedback after Play Mode visual QA: the BBH intro appears too high,
  reading closer to ~40% of screen height than the requested ~60%.
- Kept the public placement contract at `0.60f`.
- Added a visual compensation offset so the visible BBH glyph block sits lower
  on screen while preserving the existing sequential animation and fade logic.
- This is a presentation-only adjustment. No scene-flow, gameplay, or
  main-menu logic changed.

## 2026-06-06 — BBH intro upper-screen position fix V2

- Visual QA with screenshot clarified that the requested placement is in the
  upper part of the screen, not the lower part.
- Reinterpreted the requirement as: the visible BBH glyph block should read
  around 40% from the top, which is equivalently about 60% from the bottom.
- Updated the runtime default vertical position from `0.60f` to `0.40f`.
- Removed the downward visual compensation introduced in V1 by resetting the
  compensation multiplier to `0.00f` and allowing symmetric offset range.
- Updated the BBH design note and recorded the correction in PROJECT_STATUS.md.
- No sequencing, fade, or main-menu flow changed.

## 2026-06-06 — BBH upper-position QA and font-reference repair V3

- The upper-position runtime fix correctly changed
  `verticalScreenPosition` from `0.60f` to `0.40f`.
- `TEST EVERYTHING` still expected the obsolete `0.60f` source token and
  therefore produced a false blocker.
- Updated the BBH runtime regression contract to require `0.40f`.
- Removed explicit destruction of the dynamically created OS font from
  `BDBBHBootIntro.OnDestroy`, preventing Unity's
  `Deleting invalid font reference` script-reload warning.
- The intro animation, sequential timing, upper-screen placement, fade, and
  main-menu flow remain unchanged.

## 2026-06-06 — BBH pseudo-3D cube text and growing shadow polish V1

- Updated the BBH intro lettering so the text itself reads as pseudo-3D,
  with layered cube-like extrusion rather than only a depth trail.
- Added a dedicated growing shadow behind each letter. The shadow scales and
  pulls away with the letter during the entrance animation.
- Kept the sequential one-letter-at-a-time timing, upper-screen placement,
  fade-out, and main-menu handoff unchanged.
- This is a visual polish pass only.

## 2026-06-06 — BBH exact 45%-from-top placement and warning cleanup V1

- Fixed the BBH intro position to an exact visual anchor:
  the center of the text is now `45%` from the top and `55%` from the bottom.
- Removed the serialized vertical-position and compensation fields so stale
  Inspector values saved in the scene can no longer keep the logo too low.
- The runtime now uses the non-serialized constant
  `VerticalScreenPositionFromTop = 0.45f`.
- Removed the obsolete `depthTrailCopies` field, eliminating CS0414.
- Updated TEST EVERYTHING to require the exact `0.45f` contract.
- Pseudo-3D extrusion, growing shadow, sequential timing, fade, and menu flow
  remain unchanged.

## 2026-06-06 — Dreamy childhood-adventure main menu V2

- Added a procedural midnight-indigo storybook background with moon glow,
  twinkling stars, lavender cloud haze, layered silhouettes, and a winding
  golden path.
- Replaced the default grey Unity menu appearance with a translucent
  midnight-blue panel.
- Restyled the title in warm ivory and the buttons in deep blue with gold-toned
  text and brighter hover/active states.
- Added quiet decorative dividers to strengthen the storybook composition.
- Preserved START GAME, SETTINGS, desktop QUIT, pause/settings flow, and the
  permanent Game Boy relic after Mother victory.
- Removed the BBH temporary OS-font lifecycle and switched the intro to
  `GUI.skin.label.font`, addressing `Deleting invalid font reference`.
- Added permanent design documentation and TEST EVERYTHING contracts.

## 2026-06-06 — Main menu button vertical-position polish V1

- Raised the main menu action buttons slightly upward.
- Reduced the empty vertical gap between the title section and the buttons.
- Preserved the dreamy background, panel, title, button styling, and menu behaviour.
- Kept START GAME, SETTINGS, and QUIT unchanged functionally.

## 2026-06-06 — Main menu button vertical-position polish V2

- Raised the main menu action buttons further upward.
- Moved the button group closer to the visual middle of the panel.
- Further reduced the empty vertical gap between the title section and the buttons.
- Preserved the dreamy background, panel, title, button styling, and menu behaviour.
- Kept START GAME, SETTINGS, and QUIT unchanged functionally.

## 2026-06-06 — Natural movement, awareness, hazard refusal, and temporary facing V1

- Recorded the failed START GAME highlight preflight as no-op; no rollback required.
- Added a non-serialized tasteful highlight for START GAME using a scoped GUI tint.
- Player movement now applies softer acceleration/deceleration and body rotation at runtime.
- Horse movement is faster than the player, accelerates/brakes more softly, and follows wider travel turns.
- Horse hazard safety now looks ahead, swerves before lava/holes/chasms/missing ground, then refuses movement for about one second.
- Enemy movement changes direction more smoothly while preserving committed high-speed attacks.
- Enemy target awareness refreshes frequently and rebinds after mount/dismount or scene state changes.
- Enemy awareness and attack/start ranges were increased moderately.
- Added temporary front/rear visual markers for player, horse, and enemies until real models are integrated.
- Added permanent removal instructions for the temporary markers.
- Added TEST EVERYTHING contracts for every part of this package.
- Status remains VERIFY until Unity compilation, TEST EVERYTHING, and Play Mode checks pass.

## 2026-06-06 — Long-press light spinning AOE attack V1

- Added a dedicated long-press path for the normal light-attack input.
- Quick click remains the regular light attack.
- Holding for 0.24 seconds triggers a fast spinning AOE when its own cooldown is ready.
- If the spinning AOE is cooling down, the press immediately falls back to a normal light attack without waiting.
- The AOE uses an independent 0.85 second cooldown and does not apply that cooldown to the regular light attack.
- The AOE damages every unique living enemy inside its radius for 82% of regular light damage per target.
- Existing `WeaponDamageMultiplier` is applied, so weapon-damage pickups improve the AOE.
- Every hit receives outward knockback, stagger, flash, and impact feedback.
- Added `BDSpinAttackVisual` for a short rotating three-arc animation; the standard melee slash visual is not spawned.
- Added design documentation and TEST EVERYTHING contracts.

<!-- B&D DOC GOVERNANCE V8 CHANGELOG START -->
## 2026-06-07 — Permanent documentation governance and one-click QA conflict repair V8

- Made request capture and implementation timing a permanent repository rule that the user does not need to repeat.
- Added `START_HERE.md` as the unmistakable mandatory entry point.
- Added `DOCUMENTATION_INDEX.md` with maintained-document ownership and update triggers.
- Added `ARCHITECTURE.md` with runtime/editor/QA ownership and Mermaid flow diagrams.
- Added `QA_CHECKLIST.md`, `TECHNICAL_DECISIONS.md`, and `PERFORMANCE_GUIDELINES.md` without creating a second status source.
- Added `BDDocumentationGovernanceQA` and integrated it into the existing `TEST EVERYTHING` entry point.
- Added a non-destructive repair for a conflicted `BDOneClickQAWindow.cs`: restore only that tracked file from `HEAD`, preserve all other local work, then reconnect every discovered modular `BD*QA.Scan` check and required manual checks.
- Static/package validation is required before Unity. Unity compilation, `TEST EVERYTHING`, and focused Play Mode remain unverified until run locally.
<!-- B&D DOC GOVERNANCE V8 CHANGELOG END -->

<!-- B&D QA CONTRACT DRIFT V9 CHANGELOG START -->
## 2026-06-07 — Line-aware conflict scan and stale QA contract repair V9

- Recorded the real four-blocker `TEST EVERYTHING` result supplied by the user.
- Corrected `BDDocumentationGovernanceQA` so only standalone conflict-marker lines block QA; inline examples in `QA_CHECKLIST.md` and quoted strings in QA source no longer create false blockers.
- Updated the stale minimap QA requirement from the failed V1 marker to the active V7 rigid clipped-map contract.
- Updated the stale START GAME highlight QA requirement from the removed `startGamePressed` local variable to the current action-aware `MenuActionVisual.Progress` path.
- Preserved runtime minimap and menu implementations; no obsolete compatibility code was reintroduced.
- Updated `QA_CHECKLIST.md` and `TECHNICAL_DECISIONS.md` so future checks follow active behavior rather than fragile implementation trivia.
- Static/package validation is required before Unity. Unity compilation, automated QA, and Play Mode remain unverified until rerun locally.
<!-- B&D QA CONTRACT DRIFT V9 CHANGELOG END -->
