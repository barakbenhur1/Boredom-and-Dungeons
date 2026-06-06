# Boredom & Dungeons — Authoritative Categorized Development Plan

> This is the single authoritative living development plan and status tracker.
> It must be updated whenever code, requirements, QA results, priorities, blockers, category status, or implementation order changes.

## 1. Snapshot

```text
Status date: 2026-06-06
Engine: Unity 6000.0.76f1
Code baseline inspected: 11abb7f1b1996a240dc3d407af5acb59b28cf229
Current active feature category: C07 — Boss framework, role planning, and encounter contracts
Current active item: C07.16 — Wire the framework into one real playable test encounter
Immediate prerequisite gate: C01.10 — Recompile latest main, rerun TEST EVERYTHING, and complete Play Mode smoke QA
Resume point after the prerequisite gate: C07.16, then continue through C07.29
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

## Known recovery item

- [ ] **RECOVERY REQUIRED — “Mother Boss” design**
  - Existence was confirmed by the user.
  - The detailed design was lost with the damaged chat.
  - Unknown: appearance, role, narrative meaning, arena, phases, attacks, rewards, and relationship to the existing black/white final boss.
  - Do not silently merge it with, replace, or position it relative to the black/white boss until the design is recovered or re-approved.

## Category acceptance

- One source of truth exists.
- README points to it.
- Historical roadmaps cannot be mistaken for current status.
- No known requirement is left only in chat.

---

# C01 — Project stability, QA, validation, and repository health

**Category status: VERIFY**
**Purpose:** keep the project compilable and prevent feature work from building on a broken baseline.

## Completed foundations

- [x] C01.01 QA baseline workflow.
- [x] C01.02 Full project stability gate foundation.
- [x] C01.03 Current-scene validation foundation.
- [x] C01.04 Play Mode smoke checklist tool.
- [x] C01.05 Generated prototype scene validation.
- [x] C01.06 Repository/meta/runtime-source validation foundations.
- [x] C01.07 Last saved automated QA result:
  - Unity `6000.0.76f1`;
  - status `PASS`;
  - blockers `0`;
  - warnings `0`;
  - info `0`;
  - generated UTC `2026-06-06T00:16:23.7665330Z`.
- [x] C01.08 Remove obsolete camera/minimap serialized fields from code.
- [x] C01.09 Add instructions to rerun `TEST EVERYTHING` after the cleanup.

## Immediate prerequisite gate

- [ ] **C01.10 Pull/open latest `main` in Unity 6000.0.76f1.**
- [ ] C01.11 Wait for compilation and confirm zero compiler errors.
- [ ] C01.12 Run `Boredom And Dungeons -> TEST EVERYTHING`.
- [ ] C01.13 Rebuild `Assets/_Project/Scenes/02_CleanCore_MazePrototype.unity` if required.
- [ ] C01.14 Run the complete Play Mode smoke checklist:
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
  - dynamic minimap;
  - Console cleanliness.
- [ ] C01.15 Save the latest automated and manual PASS reports.
- [ ] C01.16 Fix any regression, update this plan, then resume at C07.16.

## Later production QA

- [ ] C01.17 Add deterministic multi-seed test coverage.
- [ ] C01.18 Add boost drop-rate statistical tests.
- [ ] C01.19 Add boss state-transition tests.
- [ ] C01.20 Add build/version metadata validation.
- [ ] C01.21 Add external clean-clone/open/build verification.
- [ ] C01.22 Preserve zero release-blocking errors through final build.

## Current blocker truth

No saved automated blocker is known. The latest code baseline is still **VERIFY** because the saved PASS predates the final camera/minimap field cleanup.

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

## C09B — Mother Boss

- [ ] **C09.24 RECOVERY REQUIRED — recover or re-approve the complete Mother Boss design.**
- [ ] C09.25 Decide whether it:
  - replaces the black/white final boss;
  - precedes/follows it;
  - is an optional/story boss;
  - belongs to a separate mode/ending.
- [ ] C09.26 Recover/define visual identity, arena, phases, attacks, story function, rewards, dialogue, and acceptance tests.
- [ ] C09.27 Update all later categories after the design is recovered.
- [ ] C09.28 Do not implement speculative details before approval.

## Category acceptance

- The relationship between every narrative/final boss is explicit.
- No lost boss concept is overwritten.
- Final room, ending, reward, and barrier contracts are stable.
- Pause and request user review before C10.

---

# C10 — Map generation, level design, encounter placement, and pacing

**Category status: PLANNED**

## Map structure

- [x] C10.01 Prototype procedural maze exists.
- [ ] C10.02 Replace prototype maze feel with a designed final level.
- [ ] C10.03 Complex but understandable overall route.
- [ ] C10.04 Readable primary route.
- [ ] C10.05 Optional side paths.
- [ ] C10.06 Hidden secret spaces.
- [ ] C10.07 Large rooms rather than repeated maze cells.
- [ ] C10.08 Room sizes support horse traversal, bullet hell, and boss movement.
- [ ] C10.09 Dedicated final boss room and exit.
- [ ] C10.10 Dedicated ending/cinematic room.

## Natural geometry

- [ ] C10.11 Rounded turns.
- [ ] C10.12 Curved corridors.
- [ ] C10.13 Occasional winding/curling paths.
- [ ] C10.14 Reduce constant 90-degree geometry.
- [ ] C10.15 Preserve readability with a controlled mix of straight and curved forms.
- [ ] C10.16 Natural transitions between spaces.

## Final placement and pacing

- [ ] C10.17 Place and validate player start.
- [ ] C10.18 Place horse.
- [ ] C10.19 Place normal enemy encounters.
- [ ] C10.20 Place protected batteries.
- [ ] C10.21 Integrate Game Boy mini-boss role.
- [ ] C10.22 Integrate Game Cartridge mini-boss role.
- [ ] C10.23 Integrate pre-boss role with map remaining afterward.
- [ ] C10.24 Integrate final/narrative boss room(s).
- [ ] C10.25 Place ammo/health support.
- [ ] C10.26 Place hidden routes without advertising them.
- [ ] C10.27 Integrate reward chests and ending room.
- [ ] C10.28 Validate sequential and parallel encounter layouts.
- [ ] C10.29 Validate all legal-room contracts across many seeds.
- [ ] C10.30 Ensure escape-blocking enemies do not create impossible rooms.

## Biomes and environment layout

- [ ] C10.31 Grass areas.
- [ ] C10.32 Sand areas.
- [ ] C10.33 Mud areas.
- [ ] C10.34 Water areas.
- [ ] C10.35 Dry-ground areas.
- [ ] C10.36 Mixed transition zones.
- [ ] C10.37 Region-appropriate walls.
- [ ] C10.38 Vegetation density varies by area.
- [ ] C10.39 Large rooms have distinct silhouettes.
- [ ] C10.40 Environmental storytelling props.

## Category acceptance

- The map feels authored, not grid-generated.
- Every encounter has legal space and pacing.
- Secrets remain hidden.
- Navigation, horse movement, mobile controls, and boss arenas all work.
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

## Readability/accessibility

- [ ] C11.28 Projectile color contrast.
- [ ] C11.29 Black/white boss contrast.
- [ ] C11.30 Telegraph readability.
- [ ] C11.31 Screen-clutter limits.
- [ ] C11.32 Camera motion comfort.
- [ ] C11.33 Status-effect clarity.
- [ ] C11.34 Input feedback for taps, drags, target loss, and cooldowns.
- [ ] C11.35 Accessibility options appropriate to the final scope.

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
- [ ] C12.06 Mother Boss art after design recovery.
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

## Category acceptance

- Prototype visuals are replaced consistently.
- VFX never obscure telegraphs.
- Audio communicates state and impact.
- Pause and request user review before C13.

---

# C13 — Story, endings, cinematics, and final result logic

**Category status: IN PROGRESS**

## Existing ending-state logic

- [x] C13.01 Ending-state controller.
- [x] C13.02 `NoGameBoy`.
- [x] C13.03 `GameBoyNoBatteries`.
- [x] C13.04 `PoweredNoCartridge`.
- [x] C13.05 `PoweredWithCartridge`.
- [x] C13.06 Required complete set:
  - Game Boy;
  - 2 Batteries;
  - Game Cartridge.
- [x] C13.07 Missing any required collectible: `I'm bored`.
- [x] C13.08 Complete set: `I'm having fun :)`.
- [x] C13.09 Speech bubble is final-scene result only, never a hint/checklist.

## Final cinematic implementation

- [ ] C13.10 Final sit-down animation.
- [ ] C13.11 No Game Boy: player sits without taking anything out.
- [ ] C13.12 Game Boy/no power: take it out, fail to power it, place it aside.
- [ ] C13.13 Powered/no cartridge: gray/white no-game lighting.
- [ ] C13.14 Powered/with cartridge: colorful loaded-game lighting.
- [ ] C13.15 Camera cuts and timing.
- [ ] C13.16 Final lighting and VFX.
- [ ] C13.17 Final audio.
- [ ] C13.18 Speech-bubble presentation.
- [ ] C13.19 Integrate final/narrative boss consequences.
- [ ] C13.20 Revisit endings after Mother Boss recovery.
- [ ] C13.21 Verify all ending branches from clean runs.

## Category acceptance

- Every inventory state produces the correct ending.
- No ending leaks secret requirements before the result.
- Boss outcomes, barrier, exit, and ending state are synchronized.
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

## Performance and robustness

- [ ] C14.14 Object pooling for projectiles and VFX.
- [ ] C14.15 Profiler pass.
- [ ] C14.16 Memory and GC allocation pass.
- [ ] C14.17 Stable frame-time target for supported mobile devices.
- [ ] C14.18 Stress test bullet hell, summons, VFX, and multi-boss health UI.
- [ ] C14.19 Prevent enemy clustering.
- [ ] C14.20 Validate touch/input latency and target-selection performance.

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
- [ ] C14.43 All final/narrative boss requirements function.
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

1. Complete C01.10–C01.16: compile latest `main`, run `TEST EVERYTHING`, perform Play Mode smoke QA, and fix regressions.
2. Return to saved resume point C07.16.
3. Finish the Stage 16 shared boss framework integration through C07.23.
4. Finish deterministic selection/role/legal-placement foundation through C07.29.
5. Update Git and pause at C07.30.
6. Tell the user C07 is complete and propose C08.
7. Ask for changes before implementing the first mini-boss category.
8. Apply future requests using the earlier/current/later rule.

# 7. Current blockers and risks

## Blockers

- Latest code baseline still requires the C01 verification gate.
- Mother Boss details are unavailable and must not be invented.

## High risks

1. Runtime repair/install scripts may create duplicate or overlapping systems.
2. Reusable boss classes may be mistaken for integrated encounters.
3. Deterministic planning may not match final map progression.
4. Desktop prototype input may diverge from the mobile landscape target.
5. README and historical roadmaps may mislead contributors if not synchronized.
6. Lost chat-only requirements may remain undiscovered.
7. Final art, audio, map, balance, performance, persistence, and external build work remain substantial.

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
