# Boredom & Dungeons — Authoritative Categorized Development Plan

<!-- B&D CURRENT SNAPSHOT START -->
## Current Development Snapshot

```text
Status date: 2026-06-06
Engine: Unity 6000.0.76f1
Previous category: C01 — Project stability, QA, validation, and repository health
Previous result: DONE — TEST EVERYTHING passed with 0 blockers / 0 warnings / 0 info
Current category: C03 — Player movement, aiming, combat, damage, and weapons
Current item: C03.46–C03.55 — player hazard rules, mounted dismount recovery, and Play Mode verification
Current status: IMPLEMENTED — awaiting Unity compilation and TEST EVERYTHING
Next category: C04 — Horse traversal, mounted combat, damage, healing, and flee behavior
Next item: Verify proactive horse avoidance and exceptional no-damage recovery for holes, chasms, and lava
Saved later resume point: C07.16 — Wire the shared boss framework into one playable encounter
```
<!-- B&D CURRENT SNAPSHOT END -->


> This is the single authoritative living development plan and status tracker.
> It must be updated whenever code, requirements, QA results, priorities, blockers, category status, or implementation order changes.

## 1. Snapshot

```text
Status date: 2026-06-06
Engine: Unity 6000.0.76f1
Single authoritative progress file: /PROJECT_STATUS.md
Previous category: C01 — Project stability, QA, validation, and repository health
Previous item: Repair invalid C# backslash character literals in unified TEST EVERYTHING
Previous result: Unity compilation recovered; TEST EVERYTHING PASS with 0 blockers and 1 warning at 2026-06-06T03:42:01.6743540Z
Current category: C01 — Project stability, QA, validation, and repository health
Current item: Correct MULTIPLE_RUNTIME_INSTALLERS false positive for per-enemy BDEnemyBootstrap instances
Current finding: 79 BDEnemyBootstrap components are distributed across enemy objects; they are per-entity setup components, not duplicate scene-level installers
Next category: C01 — Project stability, QA, validation, and repository health
Next item: Rerun the single TEST EVERYTHING command and complete the Play Mode checklist; target 0 blockers and 0 warnings
Queued after C01: C03.46–C03.55 player safe-point/chasm/lava recovery
Queued after C03: C04.24–C04.39 horse hazard recovery, zero-health exhausted follow, and Pet interaction
Saved later resume point: C07.16 — Wire the shared boss framework into one real playable test encounter
```

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
- [ ] **C01.16B Integrate Camera/Minimap regression checks into the existing `Boredom And Dungeons -> TEST EVERYTHING` action.**
- [ ] C01.16C Integrate tracked-file repository hygiene into the same `TEST EVERYTHING` action; no second QA menu item may be required.
- [ ] C01.16D Verify the five obsolete Camera/Minimap fields cannot return:
  - `rotationSpeedDegreesPerSecond`;
  - `snapToMovementCardinals`;
  - `mapRotationInitialized`;
  - `minimumMovementDirectionMagnitude`;
  - `rotateOnlyWhenActuallyMoving`.
- [ ] C01.16E Validate required Camera/Minimap behavior anchors remain present.
- [ ] C01.16F Reject tracked Unity-generated folders, archives, QA exports, backups, IDE outputs, one-shot patch scripts, duplicate status files, and other root clutter.
- [ ] C01.16G Expand `.gitignore` for all generated/local/package/QA/status-snapshot artifacts while preserving real Unity `.meta` files.
- [ ] C01.16H Enforce `/PROJECT_STATUS.md` update alongside every material pending change.
- [ ] C01.16I Run only `TEST EVERYTHING`, complete its one Play Mode checklist, save FINAL QA PASS, and record the result here.


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

The existing one-click automated gate passed on 2026-06-06 at 03:22:56Z with 0 blockers, 0 warnings, and 0 info. C01 remains **IN PROGRESS** because Camera/Minimap regression, repository hygiene, and the single-authoritative-status enforcement still need to be integrated into that same button and rerun with the final Play Mode checklist.

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

# C03 — Player movement, aiming, combat, damage, and weapons

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
- [ ] C03.38 Define charged-shot ammunition cost and damage.
- [ ] C03.39 Verify automatic reload without requiring another fire input.
- [ ] C03.40 Define and test laser behavior against dodge i-frames.
- [ ] C03.41 Tune ammo economy and recovery windows.

## Player health and lifecycle

- [x] C03.42 Damage/death/reset foundations exist.
- [ ] C03.43 Verify one source cannot apply duplicate damage in one hit.
- [ ] C03.44 Verify clean death and restart/reset state.
- [ ] C03.45 Add final player health feedback and accessibility cues.

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
- [ ] C04.20 Verify horse cannot receive duplicate damage from one event.
- [ ] C04.21 Tune horse movement for final room scale and mobile controls.
- [ ] C04.22 Add final riding, damage, healing, buck, and flee feedback.
- [ ] C04.23 Complete full horse Play Mode QA.

## Environmental hazard behavior — inserted earlier-category work

- [ ] **C04.24 Add chasm/hole avoidance to horse navigation and flee pathing.**
- [ ] C04.25 Add lava avoidance to horse navigation and flee pathing.
- [ ] C04.26 Hazard avoidance must apply while following, fleeing, wandering, approaching the player, and being ridden where AI pathing is active.
- [ ] C04.27 If the horse nevertheless falls into a hole/chasm, the horse loses no health and returns to the latest legal horse-safe position.
- [ ] C04.28 If the horse nevertheless enters lava, the horse loses no health and returns to the latest legal non-lava horse-safe position.
- [ ] C04.29 If player and horse fall together, recover them as one mounted pair when possible; otherwise place them safely beside each other without overlap.
- [ ] C04.30 Prevent repeated hazard loops, separation across inaccessible geometry, and flee/follow systems overriding recovery.
- [ ] C04.31 Add Play Mode tests for mounted/unmounted chasm and lava recovery, flee pathing near hazards, and safe fallback when no recent horse-safe point is valid.

## Zero-health exhausted follow and contextual Pet interaction

- [ ] **C04.32 Preserve the horse's current zero-health behavior while the player remains nearby.**
- [ ] C04.33 When the player remains farther than `14m` for at least `1.25s`, enter an exhausted-follow state and move toward the player at approximately `20%` of normal follow speed.
- [ ] C04.34 Stop exhausted follow at `8m` or less; use threshold hysteresis to prevent rapid start/stop switching.
- [ ] C04.35 Exhausted follow does not restore health and cannot enable mounting, mounted shooting, sprinting, combat flee, attack utility, or normal healthy follow behavior.
- [ ] C04.36 Exhausted movement must retain obstacle, hole, chasm, lava, boss-barrier, and doorway avoidance; invalid cross-room paths use a safe reposition fallback.
- [ ] C04.37 Show a contextual `Pet` button when the on-foot player is within `2.25m` of the horse and both actors are in a safe interruptible state.
- [ ] C04.38 Short press: player pets the horse. Long press of at least `0.65s`: the horse affectionately nuzzles/rubs the player. Short and long press are mutually exclusive and long-press progress is visible.
- [ ] C04.39 Pet interactions are expressive only: no healing, stat, score, collectible, or progression effect; cancel safely for distance, combat, dodge, mount, damage, cutscene, or door transition.

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
- [ ] C05.22 Verify all enemies avoid walls, obstacles, player overlap, and illegal terrain.
- [ ] C05.23 Skip a summon safely when no valid position exists.
- [ ] C05.24 Add spawn/summon limits per encounter.
- [ ] C05.25 Verify sword weapon/double-slash visuals in Play Mode.

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
- [ ] C06.11E Spawn points must avoid other guardians, walls, props, lava, holes, chasms, inaccessible terrain, and active barriers while preserving at least one reachable movement/escape direction.
- [ ] C06.11F Use alternate candidate positions and a scored legal fallback; delay/cancel an individual guardian spawn when no legal point exists instead of forcing an invalid spawn.
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

- [ ] **C07.16 Wire the framework into one real playable test encounter.**
- [ ] C07.17 Connect real damage sources to health channels and life states.
- [ ] C07.18 Connect boss HUD to authoritative scene/prefab flow.
- [ ] C07.19 Implement arena activation, entrance lock, intro lockout, victory unlock, and cleanup.
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
- [ ] C10.17 Inaccessible regions may use mountains, lakes, chasms, holes, columns, large obstacles, or mixed landmark formations.
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
- [ ] C11.13 Make camera and minimap resolution/aspect-ratio safe for mobile landscape.

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
- Lava may be entered by ordinary walking and applies exactly `10` unavoidable environmental damage.
- When player and horse enter any hazard while mounted, both recover separately and the player returns on foot.
- Hazard recovery never restores the mounted relationship automatically.
- The horse proactively filters movement toward lava, holes, chasms, and unsupported ground.
- Horse AI attempts safe steering alternatives before stopping.
- A normal horse walk, follow, flee, return, or mounted movement must not cause a fall.
- Exceptional or scripted horse hazard entry recovers the horse without health loss.
- C03/C04 remain awaiting Unity compilation, `TEST EVERYTHING`, and Play Mode verification.
