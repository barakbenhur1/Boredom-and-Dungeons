# Boredom & Dungeons — Authoritative Project Status and Ordered Work

## Current development snapshot

```text
Status date: 2026-06-07
Classification: EARLIER / BLOCKING REGRESSION
Active work: C03/C11/C12.RUNTIME.V22R2
Current truth: V22 TEST EVERYTHING passed with 0 blockers, 0 warnings, and 0 info, but the Unity Console is still not clean.
Console blockers: CS0414 for the unused BDCameraFollow.minimumCameraDistance field and repeated edit-mode material-instantiation leak warnings from BDOccludingWall.Initialize during TEST EVERYTHING.
Prepared repair: V22R2 removes the unused field, uses sharedMaterial in Edit Mode and a per-renderer material instance only during Play Mode, and adds regression QA for both warning classes.
Verification state: package validation passed; Unity compilation, TEST EVERYTHING, focused Play Mode, and a clean Console are still required.
Saved feature resume point after V22R2 passes: C03.23A -> C07.16A -> C07.16 -> C07.17.
Later work retained without interrupting V22R2: C12.42 explicit AudioMixer routing for Master, Music, SFX, and Ambience.
```

This file is the only live source for current status, ordering, blockers, verification truth, and the resume point. Durable behavior belongs in the maintained files under `Assets/_Project/Design/`. Git history stores previous states; stale package narratives and duplicate roadmaps are not live documentation.

## Permanent working rules

1. Record every material user request here before implementation or in the same change.
2. Classify it as earlier/blocking, current, later, or recovery-required.
3. Earlier regressions stop later feature work; preserve the resume point, repair, verify, then return.
4. Synchronize every affected design, architecture, QA, technical-decision, and performance document.
5. Maintained Git documentation reflects current truth only. Merge valid content before deleting a superseded document.
6. Do not claim Unity compilation, Play Mode, performance, or QA success unless it actually ran.
7. Repair from the actual local state; never replace current systems with older package copies.
8. Run repository hygiene on every handoff and before every commit.

# Active blocking work — C03/C11/C12.RUNTIME.V22R2

## V20 Runtime fixes — IMPLEMENTED LOCALLY / VERIFY

- Charged-shot visual construction reuses its `TrailRenderer` and is idempotent.
- Exactly one active `AudioListener` is preserved through the mounted cinematic.
- Mounted entrance uses the approved room-authored camera, complete input lock, right turn, full stop, hold, and delayed release.
- BBH timing starts from the first real repaint so the first B does not appear pre-rendered.

## V21R1 documentation QA — IMPLEMENTED LOCALLY / VERIFY

- Current-status QA no longer pins an obsolete exact version heading.
- Latest TEST EVERYTHING no longer reports `PROJECT_STATUS_CURRENT_ACTIVE_WORK_MISSING`.

## V22 wall visibility and room handoff — IMPLEMENTED LOCALLY / PLAY MODE VERIFY

- Structural walls are raised to at least 64 world units and remain permanently opaque.
- Legacy structural-wall fading is removed.
- Near-wall camera intent and pitch prevent side/corner/diagonal leakage.
- Room transitions use a distance-preserving previous/current-room union with no intended FOV or camera-distance change.
- Automated QA passed; focused Play Mode still determines whether wall visibility and room-node handoff are truly accepted.

## V22R2 Console-warning cleanup — BLOCKING / PREPARED

- Remove the unused serialized `minimumCameraDistance` field from `BDCameraFollow`.
- `BDOccludingWall.Initialize` uses `renderer.sharedMaterial` outside Play Mode and `renderer.material` only during Play Mode.
- TEST EVERYTHING must not instantiate materials during edit-mode scene installation.
- Add automated QA that blocks the unsafe direct material assignment and the removed unused field from returning.

## V22R2 acceptance gate

1. Install V22R2 on the exact post-V22 local state.
2. Unity compiles with no CS0414 warning from `BDCameraFollow`.
3. Run `Boredom And Dungeons -> TEST EVERYTHING` twice.
4. Both runs pass with zero blockers and no edit-mode material-instantiation warning.
5. Focused Play Mode confirms wall visibility, room/node handoff, mounted intro, BBH, charged shot, and AudioListener behavior.
6. Console contains no project-generated red errors or warning spam from the repaired systems.
7. Record real results here, then resume at C03.23A.

# Ordered project categories

- **C00 Governance:** one authoritative status, current-only documentation, request capture, repository hygiene.
- **C01 Stability/QA:** one TEST EVERYTHING entry point and truthful Runtime/Console regression coverage.
- **C02 Platform/architecture:** Unity 6000.0.76f1, runtime/editor separation, mobile-landscape target.
- **C03 Player/combat:** finish V22R2 verification, then resume C03.23A.
- **C04 Horse:** mounted hit routing, buck logic, healing, flee, hazard safety, and restart grounding.
- **C05 Enemies:** sword, patrol, charger, trap, ranged, and exit-interference roles.
- **C06 Collectibles/rewards:** secret Game Boy, Batteries, Cartridge, guardians, chests, ammo, and run boosts.
- **C07 Boss framework:** after C03.23A continue C07.16A -> C07.16 -> C07.17.
- **C08 Mini-bosses:** Square Jumper, Roller, Serpent, Quad Gunners; choose three per run.
- **C09 Narrative bosses:** preserve final-boss and complete Mother-boss contracts, including phase-specific Dodge budgets.
- **C10 Map/hazards:** multi-route generation, inaccessible natural macro-regions, legal doorways, hazards, and recovery.
- **C11 Camera/UI:** close V22R2 first, then minimap/HUD/settings/accessibility/mobile readability.
- **C12 Art/audio:** visual/audio production; C12.42 AudioMixer routing remains later.
- **C13 Story/endings:** incomplete-set endings, secret continuation, Mother loss/victory, state isolation.
- **C14 Balance/release:** profiling, pooling, persistence, cleanup, target build, clean-clone verification, release tag.

# Exact current sequence

1. Install V22R2 on top of the current post-V22 local state.
2. Wait for Unity compilation and confirm the CS0414 warning is gone.
3. Clear the Console.
4. Run TEST EVERYTHING twice and confirm no material-instantiation leak warning appears.
5. Perform focused Play Mode verification for closed walls and room/node handoff.
6. Recheck V20 Runtime fixes and inspect the Console.
7. Record real results here.
8. Resume C03.23A, then C07.16A -> C07.16 -> C07.17.
9. Keep C12.42 ordered later.

# Current risks

- Automated QA can pass while the Console still contains compiler/editor warnings.
- Static QA can pass while visual Runtime behavior remains wrong.
- Camera ownership must never disable the only AudioListener.
- Old full-file package copies can erase current fixes.
- Stale documents can mislead future contributors.

# Current changelog

## 2026-06-07 — V22 wall visibility and no-zoom handoff

- Added permanent structural-wall opacity, 64-unit wall height, closed-wall intent guard, near-wall top-down pitch, and distance-preserving union handoff.
- TEST EVERYTHING later passed with zero automated findings.

## 2026-06-07 — V22R2 Console-warning cleanup prepared

- Latest TEST EVERYTHING report passed with zero blockers, warnings, and info.
- Unity Console still reported one unused-field compiler warning and repeated edit-mode material-instantiation leak warnings.
- Prepared a minimal code-only cleanup that does not change V22 camera behavior, wall geometry, room handoff, FOV, input, or scene layout.
