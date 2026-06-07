# Boredom & Dungeons — Authoritative Project Status and Ordered Work

## Current development snapshot

```text
Status date: 2026-06-07
Classification: EARLIER / BLOCKING REGRESSION
Active work: C03/C11.RUNTIME.V23
Current truth: V22R2 addresses Console warnings, but focused Play Mode still shows three blocking regressions: the camera can reveal beyond closed walls, camera motion becomes unstable around enemies and room/tile transitions, and rare enemy-hit sequences can push the player below the floor before hazard recovery returns the player embedded/stuck.
Prepared repair: V23 consolidates normal gameplay camera transform ownership into BDCameraFollow, removes the secondary viewport-bias writer, uses one stable yaw stage and planar-only shake, preserves room-contained final camera pose, and makes player recovery CharacterController-root-safe with a combat floor-loss guard.
Verification state: package construction and static validation are required; Unity compilation, TEST EVERYTHING, focused Play Mode, and Console verification remain mandatory.
Saved feature resume point after V23 passes: C03.23A -> C07.16A -> C07.16 -> C07.17.
Later work retained without interrupting V23: C12.42 explicit AudioMixer routing for Master, Music, SFX, and Ambience.
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

# Active blocking work — C03/C11.RUNTIME.V23

## V23.1 Single-owner stable gameplay camera — BLOCKING / PREPARED

- `BDCameraFollow` becomes the only normal-gameplay writer of the Main Camera transform.
- Remove the auto-installed `BDCameraForwardViewBias` secondary transform writer and its obsolete QA requirement.
- Preserve temporary cinematic ownership only through the existing run-presentation coordinator.
- Apply room containment after smoothing and shake, then write one final camera pose.
- Keep mouse/player-intent yaw in one angular-speed-limited stage; remove wall-proximity direction scaling and dynamic near-wall pitch that made turning alternately too sensitive or too dull.
- Convert impact shake to planar translation so enemy hits do not bob the camera vertically or change pitch.
- Preserve stable camera distance/FOV through room/tile handoff and prevent any adjacent-room visibility through closed walls.

## V23.2 Combat floor-loss and recovery safety — BLOCKING / PREPARED

- A successful hit on the player notifies `BDPlayerHazardRecovery` and temporarily freezes safe-point updates.
- During the short combat-impact window, unexpected loss of walkable floor support triggers immediate no-damage recovery unless the player deliberately jumped, dodged, or entered a gap.
- Recovery ground probes ignore players, horses, enemies, CharacterControllers, hazards, structural walls, and moving non-kinematic bodies.
- Recovery placement computes the correct root height from `CharacterController.center`, `height`, and `skinWidth`; it must never place the controller capsule inside the floor.
- Repeated enemy attacks near walls, corners, room transitions, and hazards must not cause a fall-through or stuck recovery.

## V23 acceptance gate

1. Install V23 on the exact post-V22R2 local state.
2. Unity compiles without project errors or new warnings.
3. `Boredom And Dungeons -> TEST EVERYTHING` passes, including single-camera-owner and combat-grounding contracts.
4. On foot and mounted, rotate beside every closed wall and corner; no adjacent room is visible.
5. Cross multiple room/tile nodes in both directions; no zoom, snap, vertical bob, or sensitivity change occurs.
6. Fight several enemies while moving, turning, taking hits, and crossing rooms; camera feel remains stable.
7. Repeated enemy attacks never place the player below the floor or return the player embedded/stuck.
8. Recheck charged shot, AudioListener, mounted intro, BBH first frame, and Console cleanliness.
9. Record real results here, then resume at C03.23A.

# Ordered project categories

- **C00 Governance:** one authoritative status, current-only documentation, request capture, repository hygiene.
- **C01 Stability/QA:** one TEST EVERYTHING entry point and truthful Runtime/Console regression coverage.
- **C02 Platform/architecture:** Unity 6000.0.76f1, runtime/editor separation, mobile-landscape target.
- **C03 Player/combat:** finish V23 verification, then resume C03.23A.
- **C04 Horse:** mounted hit routing, buck logic, healing, flee, hazard safety, and restart grounding.
- **C05 Enemies:** sword, patrol, charger, trap, ranged, and exit-interference roles.
- **C06 Collectibles/rewards:** secret Game Boy, Batteries, Cartridge, guardians, chests, ammo, and run boosts.
- **C07 Boss framework:** after C03.23A continue C07.16A -> C07.16 -> C07.17.
- **C08 Mini-bosses:** Square Jumper, Roller, Serpent, Quad Gunners; choose three per run.
- **C09 Narrative bosses:** preserve final-boss and complete Mother-boss contracts, including phase-specific Dodge budgets.
- **C10 Map/hazards:** multi-route generation, inaccessible natural macro-regions, legal doorways, hazards, and recovery.
- **C11 Camera/UI:** close V23 first, then minimap/HUD/settings/accessibility/mobile readability.
- **C12 Art/audio:** visual/audio production; C12.42 AudioMixer routing remains later.
- **C13 Story/endings:** incomplete-set endings, secret continuation, Mother loss/victory, state isolation.
- **C14 Balance/release:** profiling, pooling, persistence, cleanup, target build, clean-clone verification, release tag.

# Exact current sequence

1. Build and install V23 on top of the current post-V22R2 local state.
2. Wait for Unity compilation.
3. Run TEST EVERYTHING.
4. Verify closed walls and corners on foot and mounted.
5. Verify room/tile transitions and enemy-combat camera stability.
6. Stress enemy attacks and confirm no floor fall-through or stuck recovery.
7. Inspect the Console and record real results here.
8. Resume C03.23A, then C07.16A -> C07.16 -> C07.17.
9. Keep C12.42 ordered later.

# Current risks

- Multiple camera transform writers can bypass room containment even when each individual system passes static QA.
- Dynamic pitch, wall-direction scaling, and vertical shake can make camera sensitivity feel inconsistent.
- A recovery point based on hit point plus a fixed offset can embed a CharacterController whose root is at capsule center.
- Static QA can pass while visual Runtime behavior remains wrong.
- Old full-file package copies can erase current fixes.

# Current changelog

## 2026-06-07 — V23 camera ownership and combat grounding regression

- User confirmed closed-wall visibility still fails.
- User reported unstable up/down camera motion and inconsistent turning around enemies and room/tile transitions.
- User reported rare enemy-hit floor fall-through followed by an embedded/stuck recovery.
- Root-cause direction recorded: secondary post-follow camera transform writer, variable near-wall camera shaping, vertical shake, and fixed-offset recovery placement that ignores CharacterController root geometry.
- Preserved the saved feature resume point.
