# Boredom & Dungeons — Authoritative Project Status and Ordered Work

## Current development snapshot

```text
Status date: 2026-06-07
Classification: EARLIER / BLOCKING REGRESSION
Active work: C03/C11/C12.RUNTIME.V21
Implementation state: V20 is installed locally and V21 repair is prepared; Unity compilation, TEST EVERYTHING, focused Play Mode, and Console verification are still required.
Last verified automated QA before V20: PASS with 0 blockers and 0 warnings, but Runtime/Play Mode defects remained.
Saved feature resume point after V21 passes: C03.23A -> C07.16A -> C07.16 -> C07.17.
Later work retained without interrupting V21: C12.42 explicit AudioMixer routing for Master, Music, SFX, and Ambience.
```

This file is the only live source for current status, ordering, blockers, verification truth, and the resume point. Durable feature behavior belongs in the maintained files under `Assets/_Project/Design/`. Git history stores previous states; obsolete package narratives and duplicate roadmaps are not live documentation.

## Permanent working rules

1. Record every material user request here before implementation or in the same change.
2. Classify it as earlier/blocking, current, later, or recovery-required.
3. Earlier regressions stop later feature work; preserve the resume point, repair, verify, then return.
4. Synchronize every affected design, architecture, QA, technical-decision, and performance document.
5. Maintained Git documentation always reflects current truth. Merge valid content before deleting a superseded document.
6. Git history stores old versions. Do not retain stale roadmaps, repair reports, package files, copied status files, or chat exports as live documentation.
7. Do not claim Unity compilation, Play Mode, performance, or QA success unless it actually ran.
8. Repair from the actual local state; never overwrite current systems with older package copies.
9. Run repository hygiene on every handoff and before every commit.

# Active blocking work — C03/C11/C12.RUNTIME.V21

## V20.1 Charged-shot Runtime stability — IMPLEMENTED LOCALLY / VERIFY

- `BDChargedProjectileVisual` now reuses its existing `TrailRenderer`.
- Repeated `Attach`, `Configure`, and visual build calls are idempotent.
- Projectile orbit roots and motes are reused rather than duplicated.
- Required verification: repeatedly fire charged x3 shots and confirm no duplicate-component error or null exception.

## V20.2 AudioListener continuity — IMPLEMENTED LOCALLY / VERIFY

- The run-presentation owner keeps exactly one active listener on the active Main Camera.
- The camera GameObject remains active while only `BDCameraFollow` is temporarily disabled.
- Required verification: no missing-listener or multiple-listener warning before, during, or after the cinematic.

## V20.3 Mounted entrance cinematic — IMPLEMENTED LOCALLY / VERIFY

Authoritative contract: `Assets/_Project/Design/Runtime/MOUNTED_RUN_INTRO_AND_DOORWAY_PORTALS.md`.

- Camera is inside the entrance room, farther and higher, at 30% room depth from the opposite rear wall, looking at the doorway center.
- Every gameplay input remains locked, including mouse aim/facing and buffered actions.
- Horse enters straight, turns 90 degrees right, fully stops, holds 0.24 seconds, and only then returns camera and control.
- Normal mounted aim state is reset to the completed right-turn direction before input release.

## V20.4 BBH first-letter timing — IMPLEMENTED LOCALLY / VERIFY

Authoritative contract: `Assets/_Project/Design/UI/BBH_BOOT_INTRO_V1.md`.

- The visible intro clock begins on the first real IMGUI repaint rather than in `Awake`.
- Loading time can no longer consume the first B animation.
- First rendered frame remains black; first B begins after the positive pre-roll.

## V21.1 Closed-wall camera visibility and wall height — BLOCKING

Authoritative contract: `Assets/_Project/Design/Map/ROOM_BOUNDARY_CAMERA_AND_TEXTURE_READINESS.md`.

- Raise structural room walls from 22 to at least 36 world units so the camera frustum cannot see over them.
- Camera position and look point remain clamped inside the active room.
- Structural room walls never fade; only non-structural props may fade.
- Closed-wall sphere collision pushes the camera inward.
- Open-door flags control actor traversal, not early camera/frustum entry into an adjacent room.

## V21.2 Smooth room/node camera handoff — BLOCKING

- A legal doorway crossing may not switch camera bounds by a full room in one frame.
- Keep old room ownership until the target is clearly inside the next room, then blend boundary center/size.
- The minimap never writes player/horse transforms.
- No visual backward jump is accepted when the active minimap node changes.

## V21.3 Entrance camera height refinement — CURRENT

- Keep the approved 30%-from-rear-wall depth.
- Raise the cinematic camera to approximately 72% of room depth, clamped to 17–21.5 units, while still looking at the entrance.

## V21 acceptance gate

1. Unity compiles with no project errors.
2. `Boredom And Dungeons -> TEST EVERYTHING` passes.
3. Focused Play Mode verifies all V20 and V21 items repeatedly.
4. Console contains no duplicate TrailRenderer error, charged-projectile null exception, or AudioListener warning.
5. Real results are recorded here.
6. Only then resume at C03.23A.

# Ordered project categories

- **C00 Governance:** one authoritative status, current-only documentation, request capture, repository hygiene.
- **C01 Stability/QA:** one TEST EVERYTHING entry point, truthful contracts, Runtime regression coverage.
- **C02 Platform/architecture:** Unity 6000.0.76f1, runtime/editor separation, mobile-landscape target.
- **C03 Player/combat:** finish V21 verification, then resume C03.23A safe-position/environmental recovery.
- **C04 Horse:** preserve mounted hit routing, buck logic, healing, flee, hazard safety, and restart grounding.
- **C05 Enemies:** sword, patrol, charger, trap, ranged, and exit-interference roles.
- **C06 Collectibles/rewards:** secret Game Boy, Batteries, Cartridge, guardians, chests, ammo, and run boosts.
- **C07 Boss framework:** after C03.23A continue C07.16A -> C07.16 -> C07.17.
- **C08 Mini-bosses:** Square Jumper, Roller, Serpent, Quad Gunners; choose three per run.
- **C09 Narrative bosses:** preserve final-boss and complete Mother-boss contracts, including phase-specific Dodge budgets.
- **C10 Map/hazards:** multi-route generation, inaccessible natural macro-regions, legal doorways, hazards, and recovery.
- **C11 Camera/UI:** closed-wall visibility first, then minimap/HUD/settings/accessibility/mobile readability.
- **C12 Art/audio:** visual/audio production; C12.42 AudioMixer routing remains later.
- **C13 Story/endings:** incomplete-set endings, secret continuation, Mother loss/victory, state isolation.
- **C14 Balance/release:** profiling, pooling, persistence, cleanup, target build, clean-clone verification, release tag.

# Exact current sequence

1. Install V21 on top of the verified post-V20 local state.
2. Open Unity and wait for compilation.
3. Run `Boredom And Dungeons -> TEST EVERYTHING`; this raises/saves structural walls to 36 units.
4. Verify the entrance camera is higher while preserving the approved angle.
5. Cross multiple room/minimap nodes and confirm there is no backward visual snap.
6. Stand beside every closed wall, rotate the mouse, and confirm no adjacent room is visible.
7. Recheck V20 charged shot, AudioListener, cinematic input lock/right turn/stop, and BBH first frame.
8. Confirm the project Console is clean and record the real results here.
9. Resume C03.23A, then C07.16A -> C07.16 -> C07.17.
10. Keep C12.42 ordered later.

# Current risks

- Static QA can pass while visual Runtime behavior remains wrong.
- Camera ownership must never disable the only AudioListener.
- Buffered input must not leak after the cinematic.
- Old full-file package copies can erase current fixes.
- Stale documents can mislead future contributors.

# Current changelog

## 2026-06-07 — V20 local implementation package

- Made charged-projectile visual construction idempotent.
- Added exactly-one-listener Runtime ownership.
- Added farther/higher room-authored camera, complete input lock, right turn, full stop, and delayed release.
- Bound BBH timing to the first rendered frame.
- Hardened room-aware camera containment.
- Added V20 automated QA and focused Play Mode requirements.
- Consolidated the live status to current truth only.

## 2026-06-07 — V21 camera and room-handoff regression

- User confirmed closed-wall visibility leakage remains after V20.
- User requested a higher entrance establishing camera while preserving the improved angle.
- User reported a serious backward-jump visual snap whenever the active minimap node changes.
- Classified all three as earlier/blocking regressions; saved feature resume remains unchanged.
