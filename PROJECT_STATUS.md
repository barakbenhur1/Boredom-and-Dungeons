# Boredom & Dungeons — Authoritative Project Status and Ordered Work

## Current development snapshot

```text
Status date: 2026-06-07
Classification: EARLIER / BLOCKING REGRESSION
Active work: C03/C10/C11.RUNTIME.V23R2
Current truth: V23R1 did not install because its preflight pinned an outdated design-document hash. No V23R1 code or QA file was applied. The current local code remains post-V22R2.
New gameplay blockers: the hole/chasm can still be entered by ordinary walking at certain angles, and after one intentional dodge fall ordinary walking can keep re-entering it. Hole recovery also returns the player too far from the hole.
Prepared repair: V23R2 supersedes V23R1. It includes the single-owner camera and combat-grounding repairs, replaces endpoint-only hole blocking with a swept capsule-footprint check, limits intentional hole entry to an active dodge/jump/forced movement window, clears stale gap intent after recovery, and stores a nearest valid local recovery anchor just outside the hole.
Verification state: package construction/static validation required; Unity compilation, TEST EVERYTHING, focused Play Mode, and Console verification remain mandatory.
Saved feature resume point after V23R2 passes: C03.23A -> C07.16A -> C07.16 -> C07.17.
Later work retained without interrupting V23R2: C12.42 explicit AudioMixer routing for Master, Music, SFX, and Ambience.
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

# Active blocking work — C03/C10/C11.RUNTIME.V23R2

## V23R2.1 Single-owner stable gameplay camera — BLOCKING / PREPARED

- `BDCameraFollow` becomes the only normal-gameplay writer of the Main Camera transform.
- Remove the secondary viewport-bias writer and obsolete QA requirement.
- Preserve temporary cinematic ownership only through the existing run-presentation coordinator.
- Apply room containment after smoothing and planar shake, then write one final camera pose.
- Keep mouse/player-intent yaw in one rate-limited stage; wall/enemy proximity must not change sensitivity or pitch.
- Preserve stable camera distance/FOV through room/tile handoff and prevent adjacent-room visibility through closed walls.

## V23R2.2 Combat floor-loss and root-safe recovery — BLOCKING / PREPARED

- Successful player damage starts a short grounding guard and freezes safe-point updates.
- Unexpected floor-support loss during the guard recovers immediately without damage unless the player is deliberately jumping, dodging, or entering a gap.
- Recovery probes reject players, horses, enemies, CharacterControllers, hazards, structural walls, and moving bodies.
- Recovery placement computes root height from `CharacterController.center`, `height`, and `skinWidth`.

## V23R2.3 Walking-proof hole boundary — BLOCKING / PREPARED

- Ordinary walking may never enter a hole/chasm, including diagonal/corner approaches.
- Motion filtering checks the swept capsule footprint along the whole requested horizontal path, not only the endpoint.
- Sliding along a safe axis remains possible; penetration into the hole does not.
- Intentional hole entry is allowed only while a dodge is actively moving, a jump is actively ascending across the edge, or an explicit forced-gap movement window is active.
- Old `lastDodgeStartedAt`/`lastJumpStartedAt` grace tails do not permit later walking entry.
- Recovery clears all gap-entry state and applies a short re-entry suppression window.

## V23R2.4 Near-hole respawn — BLOCKING / PREPARED

- At the start of an intentional hole fall, capture the closest valid grounded point just outside the same hole.
- Prefer that local anchor for the respawn before older safe points or the initial spawn.
- Use a small safe edge clearance sufficient to prevent immediate re-entry, rather than the global distant hazard clearance.
- The respawn must be visibly near the fall location, fully above ground, outside the hole volume, movable immediately, and free of recovery loops.

## V23R2 acceptance gate

1. Install V23R2 on the exact current post-V22R2 local state; the failed V23R1 attempt must not be required.
2. Unity compiles without project errors or new warnings.
3. `Boredom And Dungeons -> TEST EVERYTHING` passes, including camera, grounding, swept-hole, active-intent, and local-respawn contracts.
4. Walk toward every side and corner of the hole at slow and full speed; walking never falls in.
5. Dodge or jump into the hole; the fall still works intentionally.
6. After recovery, immediately walk toward the same hole from several angles; walking remains blocked.
7. Respawn occurs near the same hole, not at a distant historical safe point or spawn.
8. Repeated enemy attacks never place the player below the floor or return the player embedded/stuck.
9. Recheck closed walls, camera transitions, charged shot, AudioListener, mounted intro, BBH first frame, and Console cleanliness.
10. Record real results here, then resume at C03.23A.

# Ordered project categories

- **C00 Governance:** one authoritative status, current-only documentation, request capture, repository hygiene.
- **C01 Stability/QA:** one TEST EVERYTHING entry point and truthful Runtime/Console regression coverage.
- **C02 Platform/architecture:** Unity 6000.0.76f1, runtime/editor separation, mobile-landscape target.
- **C03 Player/combat:** finish V23R2 verification, then resume C03.23A.
- **C04 Horse:** mounted hit routing, buck logic, healing, flee, hazard safety, and restart grounding.
- **C05 Enemies:** sword, patrol, charger, trap, ranged, and exit-interference roles.
- **C06 Collectibles/rewards:** secret Game Boy, Batteries, Cartridge, guardians, chests, ammo, and run boosts.
- **C07 Boss framework:** after C03.23A continue C07.16A -> C07.16 -> C07.17.
- **C08 Mini-bosses:** Square Jumper, Roller, Serpent, Quad Gunners; choose three per run.
- **C09 Narrative bosses:** preserve final-boss and complete Mother-boss contracts, including phase-specific Dodge budgets.
- **C10 Map/hazards:** close V23R2 hole boundary and local recovery first, then continue map/hazard work.
- **C11 Camera/UI:** close V23R2 camera work first, then minimap/HUD/settings/accessibility/mobile readability.
- **C12 Art/audio:** visual/audio production; C12.42 AudioMixer routing remains later.
- **C13 Story/endings:** incomplete-set endings, secret continuation, Mother loss/victory, state isolation.
- **C14 Balance/release:** profiling, pooling, persistence, cleanup, target build, clean-clone verification, release tag.

# Exact current sequence

1. Install V23R2 on the current post-V22R2 local state.
2. Wait for Unity compilation.
3. Run TEST EVERYTHING.
4. Verify camera/wall/room-transition behavior.
5. Verify walking-proof hole boundaries from every angle.
6. Verify intentional dodge/jump falls and near-hole recovery, including immediate post-recovery walking.
7. Stress enemy attacks and confirm no floor fall-through or stuck recovery.
8. Inspect the Console and record real results here.
9. Resume C03.23A, then C07.16A -> C07.16 -> C07.17.
10. Keep C12.42 ordered later.

# Current risks

- Exact-hash preflight on maintained documentation can block a valid code repair after documentation was updated independently.
- Endpoint-only hazard checks allow diagonal tunnelling.
- Long recent-action grace windows can turn later ordinary walking into an unintended gap entry.
- A global conservative safe point can make hole recovery feel disconnected from the fall location.
- Static QA can pass while visual Runtime behavior remains wrong.

# Current changelog

## 2026-06-07 — V23R2 hole boundary and local respawn regression

- V23R1 installer stopped before modifying code because one maintained design document had a newer hash than its package manifest.
- User reported ordinary walking can enter the hole from specific angles.
- User reported walking can repeatedly re-enter after an earlier dodge fall.
- User requested a respawn close to the same hole rather than a distant safe point.
- V23R2 supersedes V23R1 and preserves the saved feature resume point.
