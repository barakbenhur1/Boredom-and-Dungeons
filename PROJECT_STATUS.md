# Boredom & Dungeons — Persistent Project Status

> This is the authoritative living status file for the project.
> Update it in the same commit, or immediately after every material code, design, QA, roadmap, or scope change.

## Snapshot

```text
Status date: 2026-06-06
Unity version: 6000.0.76f1
Roadmap size: 36 stages
Baseline inspected before this document: 11abb7f1b1996a240dc3d407af5acb59b28cf229
Current development position: stabilization and integration of Stage 16–17 foundations
Current immediate action: compile in Unity, run TEST EVERYTHING, then complete the Play Mode smoke checklist
```

## Status meanings

- **DONE** — implemented, connected to the generated scene, verified in Play Mode, edge cases checked, committed, and documented.
- **PROTOTYPE DONE** — working at system/prototype level, but still needs final production art, balance, or polish.
- **DESIGN COMPLETE** — rules and behavior are approved, but implementation is not complete.
- **IN PROGRESS** — meaningful code exists, but integration or validation is incomplete.
- **NOT STARTED** — no complete implementation exists yet.
- **BLOCKED** — cannot proceed until the listed blocker is removed.

A stage must not be marked **DONE** until all of these are true:

1. Unity compiles cleanly.
2. The feature is connected to the real generated scene or prefab flow.
3. Play Mode testing passes.
4. Important edge cases pass.
5. The implementation is committed to Git.
6. This file and the relevant completion report are updated.

## Current QA truth

### Last recorded automated QA

```text
Generated UTC: 2026-06-06T00:16:23.7665330Z
Unity: 6000.0.76f1
Automated status: PASS
Blockers: 0
Warnings: 0
Info: 0
```

### Important qualification

The last recorded automated PASS was produced immediately before the latest camera/minimap cleanup commit. Therefore the current `main` still requires:

- [ ] Open the latest `main` in Unity 6000.0.76f1.
- [ ] Wait for compilation to finish.
- [ ] Confirm zero compiler errors.
- [ ] Run `Boredom And Dungeons -> TEST EVERYTHING` again.
- [ ] Run the full Play Mode smoke checklist.
- [ ] Save the latest PASS reports.

There is currently no known code blocker recorded in Git, but the latest HEAD is not considered fully verified until those checks are rerun.

## Executive status

```text
Stages 1–11: PROTOTYPE DONE
Stages 12–15: DESIGN COMPLETE
Stage 16: IN PROGRESS — advanced shared framework exists; full integration and Play Mode QA remain
Stage 17: IN PROGRESS — deterministic planning/seed work exists; real map-room integration and multi-seed validation remain
Stages 18–36: NOT COMPLETE
```

The project is not at Stage 18 completion work yet. The correct current position is finishing stabilization and integration for the Stage 16–17 foundation.

## Roadmap status — all 36 stages

| Stage | Name | Status | Current truth / remaining acceptance |
|---:|---|---|---|
| 1 | QA baseline workflow | PROTOTYPE DONE | Baseline QA flow exists. New stability gate and smoke tools were added later. |
| 2 | Project structure pass | PROTOTYPE DONE | Runtime/editor separation exists; production cleanup remains in Stage 35. |
| 3 | Scene-builder decomposition preparation | PROTOTYPE DONE | Clean prototype scene builder exists; more production prefab/serialization work remains. |
| 4 | Inventory state expansion | PROTOTYPE DONE | Secret inventory states exist. |
| 5 | Game Cartridge collectible type | PROTOTYPE DONE | Cartridge collectible/state exists. |
| 6 | Secret collectible badge HUD | PROTOTYPE DONE | Badges appear only after pickup; no missing-item checklist is allowed. |
| 7 | Ending-state controller | PROTOTYPE DONE | Ending state logic exists. |
| 8 | Four procedural ending variants | PROTOTYPE DONE | Logic exists; final cinematic art/audio/camera polish remains in Stage 32. |
| 9 | Secret collectible advertising guard | PROTOTYPE DONE | No objective marker, empty slot, missing-item text, or collection checklist. |
| 10 | Guardian spawn VFX | PROTOTYPE DONE | Spawn anticipation/VFX flow exists. |
| 11 | Battery encounters hardening | PROTOTYPE DONE | Safer/fairer guardian placement exists. |
| 12 | Four-mini-boss roster and random-role rules | DESIGN COMPLETE | Pool and 3-of-4 role rules are approved. |
| 13 | Square Jumper and Roller designs | DESIGN COMPLETE | Designs exist; complete production encounters do not. |
| 14 | Serpent and Quad Gunners designs | DESIGN COMPLETE | Designs exist; complete production encounters do not. |
| 15 | Final black/white split-boss design | DESIGN COMPLETE | Three-phase design and exit-barrier rules are approved. |
| 16 | Boss / mini-boss production framework | IN PROGRESS | Shared encounter/life states, phase logic, health channels/group/HUD, telegraph contract, reward chest, barrier, and summon-budget foundations exist. Real encounter wiring, arena lifecycle, complete damage/life integration, final prefab setup, and Play Mode QA remain. |
| 17 | Random selection, role assignment, placement | IN PROGRESS | Planning data types, deterministic plan generator, and seed fixes exist. Real map candidate extraction, legal-room placement, progression scoring, bounded rerolls/fallback validation, encounter-root creation, role rewards, and many-seed testing remain. |
| 18 | Square Jumper implementation | NOT STARTED | Some future-facing placement safety support exists, but the full mini-boss encounter is not complete. |
| 19 | Roller implementation | NOT STARTED | Design only. |
| 20 | Serpent implementation | NOT STARTED | Design only. |
| 21 | Quad Gunners implementation | NOT STARTED | Design only. |
| 22 | Final black/white boss implementation | NOT STARTED | Shared framework pieces exist, but the complete boss encounter is not implemented. |
| 23 | Full map redesign | NOT STARTED | Current clean maze is still a prototype. |
| 24 | Natural geometry / curved-turn pass | NOT STARTED | Final anti-grid level geometry remains. |
| 25 | Gameplay placement and pacing | NOT STARTED | Final player, horse, enemy, collectible, mini-boss, boss, support-item, secret-route, and ending-room placement remains. |
| 26 | Biomes / terrain / ground textures | NOT STARTED | Grass, sand, mud, water, dry ground, and transitions remain. |
| 27 | Walls / environment materials / props | NOT STARTED | Final environment identity and storytelling props remain. |
| 28 | Lighting / atmosphere / camera readability / VFX | NOT STARTED | Prototype camera/VFX fixes exist; final production pass remains. |
| 29 | Character / enemy / horse / weapon / projectile art | NOT STARTED | Prototype visuals remain; player visual was narrowed slightly. |
| 30 | HUD / minimap / ammo / reload / boss UI polish | IN PROGRESS | Ammo/reload, dynamic minimap, collectible badges, parry feedback, and boss HUD foundations exist. Resolution scaling and final presentation remain. |
| 31 | Audio foundation and full sound pass | NOT STARTED | Final music, ambience, movement, combat, boss, chest, pickup, and barrier audio remain. |
| 32 | Ending cinematics and speech bubbles | IN PROGRESS | Four ending-state variants exist procedurally. Final animation, camera, lighting, audio, and speech-bubble presentation remain. |
| 33 | Combat and difficulty balance | NOT STARTED | Systems are changing; final balancing must wait for complete encounters. |
| 34 | Readability / accessibility / performance | NOT STARTED | Some readability and safety fixes exist; full profiler, pooling, GC, clutter, contrast, and stable-frame-time work remains. |
| 35 | Production cleanup / assets / progression | IN PROGRESS | Stability validators and cleanup work started. Reflection/runtime repair bootstraps, prefab/data migration, obsolete assets, save/progression, metadata, and repository cleanup remain. |
| 36 | Full vertical-slice QA and playable build | NOT STARTED | Requires a complete start-to-finish game, external build verification, release notes, version, and tag. |

## Cross-cutting implementation status

### Player combat

**Implemented foundations**

- [x] Light and heavy melee attacks.
- [x] Dodge movement and i-frames foundation.
- [x] Physical-attack parry signal tracking.
- [x] Player parry timing state.
- [x] Successful parry cancels physical damage.
- [x] Physical melee attacks are eligible for parry.
- [x] Selective parry time freeze and visual feedback.
- [x] Melee attack buffer foundation.
- [x] Landing attack detection/damage multiplier foundation (`x1.2`).
- [x] Improved landing attack visuals.
- [x] Tap shooting.
- [x] Charged magazine shot foundation.
- [x] Automatic reload fix.
- [x] Projectile knockback policy by combatant profile.

**Still required**

- [ ] Full Play Mode verification for attack buffer ordering and duplicate-input prevention.
- [ ] Full landing-attack edge-case testing, including tiny drops and multi-target hits.
- [ ] Full parry matrix: sword, charge, jump/landing, bite, stomp, hand, tail, body roll, and future physical attacks.
- [ ] Confirm projectiles, lasers, bombs, environmental damage, and bullet hell are not incorrectly parryable.
- [ ] Confirm world freeze restores rigidbodies, animators, particles, enemies, and projectiles correctly.
- [ ] Define and test laser behavior against dodge i-frames.
- [ ] Final combat feel, timing, VFX, audio, and balance.

### Horse

**Implemented foundations / repairs**

- [x] Combat flee controller and reliable flee motor foundations.
- [x] Runtime horse feature repair/bootstrap support.
- [x] Needs-healing visual restored.
- [x] Buck/throw safety fallback restored.
- [x] Horse starting-position fix.
- [x] Mounted shooting foundation.

**Still required**

- [ ] Verify flee behavior from every combat activation path.
- [ ] Verify safe spots are legal and not near enemies.
- [ ] Prevent other systems from forcing Follow during combat flee.
- [ ] Verify mounted combat start behavior.
- [ ] Verify two-hit buck/throw timing and animation.
- [ ] Verify damaged, fainted, healing, and healthy visual states.
- [ ] Verify no double damage from one hit.
- [ ] Full horse Play Mode QA.

### Minimap, camera, controls, and placement safety

**Implemented / recently changed**

- [x] Dynamic player-up minimap behavior.
- [x] Minimap sector and alignment fixes.
- [x] Camera/minimap obsolete serialized fields removed.
- [x] Camera/minimap/control polish pass.
- [x] Enemy placement safety foundations.
- [x] Square Jumper landing/summon safety support for future implementation.

**Still required**

- [ ] Recompile the latest HEAD after obsolete-field removal.
- [ ] Rerun TEST EVERYTHING.
- [ ] Verify minimap behavior across all four sectors and diagonal boundaries.
- [ ] Verify camera intent behavior with keyboard movement and mouse aiming.
- [ ] Verify enemies never spawn or land inside walls, obstacles, or the player.

### Boosts and rewards

**Implemented**

- [x] Third defeated mini-boss chest can expose a separate Parry freeze boost pickup.
- [x] Parry freeze upgrade foundation from 1 second to 2 seconds.
- [x] Shared boss summon-budget foundation.
- [x] Boss reward-chest foundation.

**Still required**

- [ ] Complete regular boost state without fragile reflection.
- [ ] Extra Ammo, Faster Reload, Movement Speed, and Weapon Damage.
- [ ] Maximum 3 ranks per boost.
- [ ] Remove maxed boost types from the available pool.
- [ ] Normal enemy drop chance: 2%.
- [ ] Mini-boss drop chance: 12%.
- [ ] Physical pickups, stable visual identity, pickup feedback, and HUD level display.
- [ ] Reset all run-only boosts between runs.
- [ ] Deterministic statistical drop-rate QA.

### Boss / mini-boss shared framework

**Implemented foundations**

- [x] Encounter and life-state types.
- [x] Shared boss encounter controller.
- [x] Configurable health channels.
- [x] Multi-channel health group.
- [x] Reusable multi-channel boss health HUD.
- [x] Shared summon-budget controller/foundation.
- [x] Shared attack telegraph interface.
- [x] Reward chest controller.
- [x] Boss phase threshold controller.
- [x] Animated magical barrier controller.
- [x] Deterministic mini-boss plan generator foundation.
- [x] Duplicate Stage 16 boss-health HUD cleanup.

**Still required before Stage 16 is DONE**

- [ ] Wire the framework into real playable encounters.
- [ ] Connect real damage sources to health channels and life states.
- [ ] Connect health HUD to the final scene/prefab flow.
- [ ] Implement arena activation, lock, unlock, intro lockout, and exit rules.
- [ ] Prove knockout, zero-health active state, critical state, and linked final death.
- [ ] Prove one-bar and multi-bar encounters.
- [ ] Connect summon budget to every real summoner.
- [ ] Eliminate unnecessary reflection/runtime repair wiring where serialized prefab references are appropriate.
- [ ] Complete Play Mode QA and a Stage 16 completion report.

## Current blockers and risks

### Blockers

- No known automated blocker is recorded in the latest saved QA report.
- The current HEAD is **verification-pending**, because the most recent saved PASS predates the latest cleanup commit.

### High risks

1. Multiple runtime installers/repair scripts may create duplicate components or overlapping behavior.
2. Stage 16 has many reusable classes, but reusable code is not the same as a fully connected boss encounter.
3. Stage 17 planning logic may not yet reflect real map-room progression and legal placement.
4. README and the old roadmap status are stale and may mislead contributors.
5. Individual mini-bosses and the final boss must not be marked implemented just because their design documents or shared framework exist.
6. Final art, audio, environment, balance, performance, save/progression, and external build QA remain large bodies of work.

## Exact current work order

1. Pull latest `main`.
2. Open in Unity 6000.0.76f1.
3. Wait for a clean compile.
4. Run `Boredom And Dungeons -> TEST EVERYTHING`.
5. Generate/rebuild `Assets/_Project/Scenes/02_CleanCore_MazePrototype.unity` if required.
6. Run the Play Mode smoke checklist:
   - movement
   - jump / landing
   - dodge + i-frames
   - light / heavy / buffer
   - landing attack
   - physical parry
   - tap / charged shot
   - automatic reload
   - horse
   - damage / death / reset
   - dynamic minimap
   - Console cleanliness
7. Fix any regressions introduced by the latest camera/minimap cleanup.
8. Finish Stage 16 real scene integration and its completion report.
9. Finish Stage 17 real map placement integration and multi-seed validation.
10. Implement Stage 18: Square Jumper.
11. Implement Stage 19: Roller.
12. Implement Stage 20: Serpent.
13. Implement Stage 21: Quad Gunners.
14. Implement Stage 22: final black/white boss.
15. Continue Stages 23–36 in roadmap order.

## Persistent update protocol

Every material change must update this file. At minimum:

1. Change the status date.
2. Update the baseline/current commit reference.
3. Update the relevant roadmap row.
4. Move completed checklist items from `[ ]` to `[x]` only after verification.
5. Add newly discovered work; do not silently delete requirements.
6. Update QA truth with the exact timestamp and result.
7. Record new blockers/risks.
8. Update the exact next action.
9. Add a changelog entry below.

## Status changelog

### 2026-06-06 — Persistent tracker created

- Added a root-level authoritative status file.
- Reconciled the stale README/Roadmap summary with the actual Stage 16–17 foundations and recent gameplay commits.
- Recorded the latest saved automated QA PASS.
- Marked the latest HEAD as verification-pending because cleanup changes followed the saved PASS.
- Defined the mandatory update protocol for every future task or scope change.
