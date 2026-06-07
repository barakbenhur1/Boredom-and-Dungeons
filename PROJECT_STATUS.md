# Boredom & Dungeons — Authoritative Project Status and Ordered Work

## Current development snapshot

```text
Status date: 2026-06-07
Classification: EARLIER / BLOCKING REGRESSION
Active work: C03/C11/C12.RUNTIME.V22
Current truth: V20 and V21R1 are installed locally. V21 improved the entrance camera but did not close wall visibility and introduced a small camera zoom during room/minimap-node handoff.
Prepared repair: V22 permanent structural-wall opacity, 64-unit wall height, closed-wall camera-intent guard, near-wall top-down pitch, and distance-preserving two-room union handoff.
Verification state: package validation passed; Unity compilation, TEST EVERYTHING, focused Play Mode, and Console verification are still required.
Saved feature resume point after V22 passes: C03.23A -> C07.16A -> C07.16 -> C07.17.
Later work retained without interrupting V22: C12.42 explicit AudioMixer routing for Master, Music, SFX, and Ambience.
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

# Active blocking work — C03/C11/C12.RUNTIME.V22

## V20 Runtime fixes — IMPLEMENTED LOCALLY / VERIFY

- Charged-shot visual construction reuses its `TrailRenderer` and is idempotent.
- Exactly one active `AudioListener` is preserved through the mounted cinematic.
- Mounted entrance uses the approved room-authored camera, complete input lock, right turn, full stop, hold, and delayed release.
- BBH timing starts from the first real repaint so the first B does not appear pre-rendered.

## V21R1 documentation QA — IMPLEMENTED LOCALLY / VERIFY

- Current-status QA no longer pins the obsolete exact V20 heading.
- Required verification: `PROJECT_STATUS_CURRENT_ACTIVE_WORK_MISSING` does not return.

## V22.1 Permanent closed-wall visibility — BLOCKING / PREPARED

Authoritative contract: `Assets/_Project/Design/Map/ROOM_BOUNDARY_CAMERA_AND_TEXTURE_READINESS.md`.

- Raise structural room walls to at least 64 world units while preserving their base.
- Remove legacy `BDOccludingWall` components from structural walls in the authoritative scene.
- Force structural walls immediately opaque at Runtime and never route them through fading.
- Near a closed wall, attenuate outward camera intent while preserving inward and tangential rotation.
- Increase pitch near a closed wall to prevent side, corner, diagonal, mounted, and screen-edge leakage.
- Preserve legal authored openings.

## V22.2 Distance-preserving room/node handoff — BLOCKING / PREPARED

- Remove room-center/half-size interpolation that introduced the visible zoom-in.
- During a legal crossing, use the union of previous and next room bounds.
- End handoff only when the target and desired camera position naturally fit inside the next room.
- Do not change FOV, camera distance, player transform, or horse transform when the minimap node changes.
- Accept neither backward snap nor zoom-in.

## V22 acceptance gate

1. Unity compiles with no project errors.
2. `Boredom And Dungeons -> TEST EVERYTHING` passes and saves the scene with 64-unit structural walls and no legacy structural wall faders.
3. Focused Play Mode confirms no view beyond closed walls from any tested angle.
4. Repeated room/node crossings show neither backward snap nor zoom-in.
5. Recheck charged shot, AudioListener, mounted input/turn/stop, and BBH first frame.
6. Console contains no project-generated red error.
7. Record real results here, then resume at C03.23A.

# Ordered project categories

- **C00 Governance:** one authoritative status, current-only documentation, request capture, repository hygiene.
- **C01 Stability/QA:** one TEST EVERYTHING entry point and truthful Runtime regression coverage.
- **C02 Platform/architecture:** Unity 6000.0.76f1, runtime/editor separation, mobile-landscape target.
- **C03 Player/combat:** finish V22 verification, then resume C03.23A.
- **C04 Horse:** mounted hit routing, buck logic, healing, flee, hazard safety, and restart grounding.
- **C05 Enemies:** sword, patrol, charger, trap, ranged, and exit-interference roles.
- **C06 Collectibles/rewards:** secret Game Boy, Batteries, Cartridge, guardians, chests, ammo, and run boosts.
- **C07 Boss framework:** after C03.23A continue C07.16A -> C07.16 -> C07.17.
- **C08 Mini-bosses:** Square Jumper, Roller, Serpent, Quad Gunners; choose three per run.
- **C09 Narrative bosses:** preserve final-boss and complete Mother-boss contracts, including phase-specific Dodge budgets.
- **C10 Map/hazards:** multi-route generation, inaccessible natural macro-regions, legal doorways, hazards, and recovery.
- **C11 Camera/UI:** close V22 first, then minimap/HUD/settings/accessibility/mobile readability.
- **C12 Art/audio:** visual/audio production; C12.42 AudioMixer routing remains later.
- **C13 Story/endings:** incomplete-set endings, secret continuation, Mother loss/victory, state isolation.
- **C14 Balance/release:** profiling, pooling, persistence, cleanup, target build, clean-clone verification, release tag.

# Exact current sequence

1. Install V22 on top of the current post-V21R1 local state.
2. Wait for Unity compilation.
3. Run `Boredom And Dungeons -> TEST EVERYTHING` so structural walls are upgraded and saved.
4. Test every closed wall on foot and mounted while rotating cardinally, diagonally, and at corners.
5. Cross multiple legal room/minimap nodes in both directions and confirm no backward snap or zoom-in.
6. Recheck V20 Runtime fixes and inspect the Console.
7. Record real results here.
8. Resume C03.23A, then C07.16A -> C07.16 -> C07.17.
9. Keep C12.42 ordered later.

# Current risks

- Static QA can pass while visual Runtime behavior remains wrong.
- Camera ownership must never disable the only AudioListener.
- Old full-file package copies can erase current fixes.
- Stale documents can mislead future contributors.

# Current changelog

## 2026-06-07 — V22 wall visibility and no-zoom handoff

- User confirmed V21 still allowed visibility beyond closed walls.
- User confirmed V21 introduced a small camera zoom during room/minimap-node handoff.
- Replaced boundary-size smoothing with a distance-preserving previous/current-room union.
- Added permanent structural-wall opacity, legacy fader removal, 64-unit wall height, closed-wall intent guard, and near-wall top-down pitch.
- Preserved the saved feature resume point.
