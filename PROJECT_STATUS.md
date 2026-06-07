# Boredom & Dungeons — Authoritative Project Status and Ordered Work

## Current development snapshot

```text
Status date: 2026-06-07
Classification: EARLIER / BLOCKING CAMERA TRANSITION REGRESSION
Active work: C01/C11.RUNTIME.V23R6-DIAGNOSIS
Current truth: V23R5 greatly improved first-frame mounted cinematic camera ownership. The brief camera view beside the entrance before the cinematic is substantially repaired. A smaller motion problem remains during ordinary walking and mounted movement: crossing procedural room or node boundaries can subtly change the camera direction, and intermittent framing changes appear as a small zoom pulse or forward/back jump of either the camera, player model, or horse model.
Unresolved cause: it is not yet proven whether the visible motion comes from room-handoff identity changes, containment correction, camera-target switching, camera boom distance, player or horse gameplay-root movement, animation/model movement, or another transform writer. Rooms and corridors must not be enlarged until instrumentation identifies the real source.
Verification truth: the user visually confirmed that V23R5 is much better. The remaining node-transition motion is still open. No newer TEST EVERYTHING output proving the complete post-V23R5 state has been recorded yet, so automated and focused verification remain required after remote synchronization.
Current action: commit the complete local V23R2-V23R5 Runtime, scene, QA, Codex, governance, and documentation state; merge current origin/main without destructive reset or rebase; push the synchronized work-in-progress checkpoint; then diagnose V23R6 from the synchronized state.
Saved feature resume point after the camera and retained regression gates pass: C03.23A -> C07.16A -> C07.16 -> C07.17.
Later work retained without interruption: C12.42 explicit AudioMixer routing for Master, Music, SFX, and Ambience.
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
9. When remote and local both contain valid unique progress, preserve both sides and merge; never reset one side over the other.


# Active blocking work — C01/C11.RUNTIME.V23R6-DIAGNOSIS

## V23R6.1 Node-transition camera and model-motion diagnosis — BLOCKING / RECORDED

- Preserve the V23R5 first-visible-frame cinematic repair and the V23R4 containment improvements.
- Do not enlarge rooms, corridors, openings, or regenerate the maze until measurements prove that physical dimensions remain the cause.
- During walking and mounted movement, record the current room/node identity and every legal handoff start and completion.
- Record desired camera position, contained position, final camera position, camera-target identity, camera-to-target distance, look point, pitch, yaw, and FOV.
- Record player and horse gameplay-root transforms separately from visual model and animation transforms.
- Determine whether the visible forward/back movement belongs to the camera, gameplay root, animated model, target switching, containment correction, or multiple stages.
- Changing room metadata alone must not rotate the camera.
- Legal transitions must preserve visual distance, FOV, pitch, yaw response, and stable player/horse screen placement.
- Implement the smallest evidence-based repair after the responsible stage is identified.

## V23R6 acceptance gate

1. Local and remote histories are synchronized without losing either side.
2. Unity compiles and TEST EVERYTHING passes on the synchronized state.
3. Diagnostics reproduce at least one problematic walking transition and one mounted transition.
4. The responsible transform owner or root/model-motion source is identified using measured deltas.
5. Repeated node transitions produce no direction snap, zoom pulse, or forward/back screen-space jump.
6. Closed-wall visibility and room containment remain intact.
7. Mounted intro, death restart, hole handling, combat grounding, charged shot, AudioListener, and BBH intro remain passing.
8. Record real verification results and resume the saved feature sequence.
# Active blocking work — C01/C11.RUNTIME.V23R5

## V23R5.1 First-render mounted cinematic camera ownership — BLOCKING / PREPARED

- On a fresh New Game or approved cinematic/victory restart, prime the approved inside-room cinematic camera synchronously in `sceneLoaded` while the black cover is fully opaque.
- Disable `BDCameraFollow` before its `Start` or `LateUpdate` can render the entrance-close gameplay pose.
- Do not yield a frame before scene presentation setup begins.
- Preserve the original camera FOV/orthographic size and whether the follow driver was enabled; restore them only after the horse completes the right turn, full stop, camera return, and input-release sequence.
- If required mounted-intro objects cannot be resolved, restore normal camera ownership before fading the cover.
- Update stale V20 camera QA to require the active V23R4 `1.20f` inset, stable wall-pressure marker, smoothed look point, and handoff-only wall cast instead of old text anchors.

## V23R5 acceptance gate

1. Installer and validator pass twice on the exact post-V23R4 state.
2. Repository hygiene and `git diff --check` pass after package cleanup.
3. Unity compiles without project errors or new warnings.
4. `Boredom And Dungeons -> TEST EVERYTHING` passes with zero blockers; the two stale V20 camera blockers are absent.
5. Launch fresh New Game repeatedly. No frame shows the camera beside the entrance or at normal follow framing before the cinematic shot.
6. The first visible gameplay frame is already the approved higher/farther inside-room view looking at the entrance.
7. The horse still enters, turns right, fully stops, and only then returns camera/control ownership.
8. Death -> New Game still skips the mounted intro and starts on foot with normal camera framing.
9. Re-run the retained V23R4 walking/riding wall-pressure and room-handoff tests and inspect the Console.
10. Record real results, then continue the retained gates and saved resume point.

# Active blocking work — C01/C11.RUNTIME.V23R4

## V23R4.1 Stable camera containment without enlarging the maze — BLOCKING / PREPARED

- Preserve the current room, corridor/opening, wall, enemy, hazard, portal, minimap, and scene placement geometry.
- Keep `BDCameraFollow` as the sole normal-gameplay camera transform owner.
- Make the 15.25-unit boom fit at room center by replacing the stale 2.25 scene inset with 1.20 and bounding legacy serialized values to a safe 0.90-1.35 range.
- Use room bounds for ordinary containment and run wall SphereCast only during an active legal room handoff.
- Smooth the independently constrained look point so player/horse height and wall pressure cannot pulse rotation or apparent zoom.
- Resolve room ownership at most once per frame and cache the room list between refreshes.
- Preserve closed-wall visibility, constant normal-gameplay FOV, cinematic ownership, planar shake, mouse yaw, and distance-preserving handoff contracts.

## V23R4 acceptance gate

1. Installer and validator pass twice on the post-V23R3A or post-V23R3B local state.
2. Repository hygiene and `git diff --check` pass after package cleanup.
3. Unity compiles without project errors or new warnings.
4. `Boredom And Dungeons -> TEST EVERYTHING` passes with zero blockers.
5. Rotate through cardinal/diagonal aim at room center; no distance pulse or apparent zoom occurs.
6. Walk and ride beside, toward, and away from walls and corners; no micro-jumps, repeated zooming, or pitch bob occurs.
7. Cross several authored openings in both directions; handoffs remain smooth and cannot reveal/cross closed wall segments.
8. Verify closed walls still hide adjacent rooms from all approved angles.
9. Confirm normal gameplay FOV remains constant and inspect the Console; optionally confirm no recurring camera room-scan spike in the Profiler.
10. Record real results, then continue the retained V23R3/V23R2 gate and saved resume point.

# Retained blocking work — C01/C11.DOCUMENTATION-QA.V23R3B

## V23R3B.1 Closed-wall visibility-boundary contract — BLOCKING / PREPARED

- Preserve the existing camera and room-boundary implementation unchanged.
- State explicitly that closed structural walls form a `visibility boundary` for camera body, look point, and adjacent-room geometry.
- Keep authored open doorways as the only legal visibility and actor-transition openings.
- Update current QA truth without weakening the existing wall, camera, or room-transition acceptance criteria.

## V23R3B acceptance gate

1. Installer and validator pass twice on the post-V23R3A state.
2. Repository hygiene and `git diff --check` pass after package cleanup.
3. Unity compiles without project errors or new warnings.
4. `Boredom And Dungeons -> TEST EVERYTHING` passes with zero blockers.
5. Focused Play Mode still confirms that closed walls hide adjacent rooms from frontal, side, corner, diagonal, high, and low camera angles.
6. Continue the retained V23R3/V23R2 gameplay acceptance gate and exact resume point.

# Retained blocking work — C00/C01/C03/C10/C11.RUNTIME.V23R3

## V23R3.1 Remote/local current-truth synchronization — BLOCKING / PREPARED

- Preserve local Runtime, scene, QA, `.meta`, package, and Codex work from the uploaded working tree.
- Preserve remote documentation commits through `40104177ee396d19f375b62a20c410e4ac63bdc8`.
- `ARCHITECTURE.md` already matches the remote blob and is not rewritten.
- Synchronize the remaining maintained documents with remote truth plus the V23R3 state.
- Do not use reset, clean, broad checkout, or an old full-project package.
- After Unity acceptance, create a safety reference, commit the merged local content, fetch the remote, merge its history, inspect, and push.

## V23R3.2 Version-agnostic current-status QA — BLOCKING / PREPARED

- Current-status QA requires the semantic snapshot, active-work line, active-blocking heading, Runtime version marker, saved resume point, and retained C12.42 work.
- It must not freeze `C03/C11/C12.RUNTIME.V*` when the approved current categories are `C00/C01/C03/C10/C11`.

## V23R3.3 Semantic camera QA compatibility — BLOCKING / PREPARED

- The V23 camera intentionally removed `movementDirectionBlend`.
- QA validates `BD SINGLE CAMERA TRANSFORM OWNER V23`, `BD STABLE SINGLE-STAGE CAMERA YAW V23`, `ResolveCameraIntentDirection`, `Vector3.RotateTowards`, `ResolvePlanarCameraShake`, and final `SetPositionAndRotation`.
- Do not reintroduce the obsolete field merely to satisfy a text scanner.

## V23R3.4 Named post-recovery walk suppression — BLOCKING / PREPARED

- Preserve the existing stronger `0.85s` suppression rather than downgrading to the stale `0.55s` text contract.
- Use a named constant/property for the suppression and validate that semantic contract.
- Recovery continues clearing dodge, jump, forced-gap, and motion state before control returns.

## V23R2 gameplay acceptance retained under V23R3

- `BDCameraFollow` remains the only normal-gameplay Main Camera transform owner.
- Enemy hits cannot push the player below the floor or recover the CharacterController inside ground.
- Ordinary walking cannot enter holes from any angle; only active dodge, ascending jump, or explicit forced movement can intentionally enter.
- Hole movement uses a swept capsule-footprint test across the requested path.
- Hole fall recovery prefers a nearby valid point beside the same hole before older safe points or spawn.
- Recovery clears stale gap-entry state and suppresses immediate re-entry.

## V23R3 acceptance gate

1. Installer and validator pass twice on a copy of the uploaded local state.
2. Repository hygiene and `git diff --check` pass.
3. The seven blockers from `ONE_CLICK_QA_latest.txt` are structurally removed without weakening active Runtime contracts.
4. Unity compiles without project errors or new warnings.
5. `Boredom And Dungeons -> TEST EVERYTHING` passes with zero blockers.
6. Walk toward every hole side/corner at several speeds; walking never falls in.
7. Dodge/jump into the hole; intentional fall still works.
8. Respawn occurs close to the same hole and remains outside it; immediate walking back remains blocked.
9. Repeated enemy hits never cause below-floor or embedded recovery.
10. Recheck camera/wall/room-transition behavior, charged shot, AudioListener, mounted intro, BBH first frame, and Console cleanliness.
11. Create a safety branch/tag, commit the verified merged state, fetch `origin`, merge current `origin/main`, rerun diff/hygiene checks, inspect, and push.
12. Record real results here, then resume C03.23A.

# Ordered project categories

- **C00 Governance:** one authoritative status, current-only documentation, request capture, repository hygiene, and lossless remote/local synchronization.
- **C01 Stability/QA:** one TEST EVERYTHING entry point and truthful semantic Runtime/Console regression coverage.
- **C02 Platform/architecture:** Unity 6000.0.76f1, runtime/editor separation, mobile-landscape target.
- **C03 Player/combat:** finish V23R3/V23R2 verification, then resume C03.23A.
- **C04 Horse:** mounted hit routing, buck logic, healing, flee, hazard safety, and restart grounding.
- **C05 Enemies:** sword, patrol, charger, trap, ranged, and exit-interference roles.
- **C06 Collectibles/rewards:** secret Game Boy, Batteries, Cartridge, guardians, chests, ammo, and run boosts.
- **C07 Boss framework:** after C03.23A continue C07.16A -> C07.16 -> C07.17.
- **C08 Mini-bosses:** Square Jumper, Roller, Serpent, Quad Gunners; choose three per run.
- **C09 Narrative bosses:** preserve final-boss and complete Mother-boss contracts, including phase-specific Dodge budgets.
- **C10 Map/hazards:** close walking-proof hole boundary and local recovery first, then continue map/hazard work.
- **C11 Camera/UI:** close camera/QA compatibility first, then minimap/HUD/settings/accessibility/mobile readability.
- **C12 Art/audio:** visual/audio production; C12.42 AudioMixer routing remains later.
- **C13 Story/endings:** incomplete-set endings, secret continuation, Mother loss/victory, state isolation.
- **C14 Balance/release:** profiling, pooling, persistence, cleanup, target build, clean-clone verification, release tag.

# Exact current sequence

1. Commit the exact current local V23R2-V23R5 Runtime, scene, QA, documentation, Codex, and repository-hygiene state as an explicit work-in-progress checkpoint.
2. Create a local safety branch pointing to that checkpoint.
3. Fetch current origin/main and merge its history without reset, rebase, clean, or broad checkout.
4. Resolve only known maintained-document conflicts using the newer locally reconciled current-truth documents; stop on any unexpected source, scene, package, or asset conflict.
5. Run repository hygiene and Git diff checks, then push the merged main branch.
6. Reopen Unity, wait for compilation, and run TEST EVERYTHING.
7. Instrument the remaining walking and mounted node-transition direction change and apparent zoom or forward/back jump.
8. Identify whether the source is room handoff, containment, camera-target switching, camera boom distance, gameplay-root movement, model animation, or another transform owner.
9. Implement the smallest evidence-based V23R6 repair without enlarging or regenerating the maze unless measurements prove geometry is the source.
10. Re-run walking, riding, wall, corner, room-transition, first-frame cinematic, hole, combat, and Console gates.
11. Record real results, then resume C03.23A -> C07.16A -> C07.16 -> C07.17.
12. Keep C12.42 ordered later.
# Current risks

- Camera boom distance plus safety inset can exceed available room half-size and create orientation-dependent compression.
- Repeated wall casts or room scans inside one LateUpdate can produce visible jitter and avoidable frame spikes.
- Reducing containment pressure must not regress closed-wall visibility or legal doorway handoff.
- A semantically correct design document can still fail a stable-token QA contract when the exact durable term is omitted.
- A valid project-level agent instruction file can be misclassified as stale root documentation when the index, external hygiene script, and Unity governance QA drift apart.
- Rich-text editing exports can become duplicate instruction sources unless ignored explicitly.
- Static QA can contradict the active implementation when it freezes old field names, durations, or category/version labels.
- A dirty local branch with a stale remote-tracking ref can make ordinary pull/rebase instructions unsafe.
- Replacing local files with remote copies would lose Runtime/scene progress; ignoring remote would lose governance and design progress.
- A failed sequential installer can leave an intentional partial state; repair packages must detect and continue from it.
- Static QA can pass while visual Runtime behavior remains wrong.

# Current changelog

## 2026-06-07 — V23R6 remaining node-transition camera motion

- V23R5 substantially improved the first visible mounted-cinematic frame and removed the main entrance-adjacent camera flash.
- A smaller issue remains while walking or riding: crossing procedural nodes can slightly change the camera direction.
- Intermittently the framing appears to zoom in or out, or jump forward and backward.
- It is not yet proven whether the moving element is the camera transform, player or horse gameplay root, animated model, or more than one system.
- The complete current local state is being synchronized to remote main as an explicit work-in-progress checkpoint.
- Synchronization does not mark the remaining camera regression as solved or the post-V23R5 Unity gates as passed.
- V23R6 begins with instrumentation and evidence-based isolation before another geometry or camera redesign.

## 2026-06-07 — V23R5 first-frame mounted camera leak

- User reported one visible entrance-close camera frame before the mounted intro camera reaches its approved pose.
- Latest TEST EVERYTHING remained blocked only by two obsolete V20 camera text contracts after V23R4.
- V23R5 primes cinematic ownership in `sceneLoaded`, removes the initial frame yield, and aligns V20 QA to the active V23R4 implementation.

## 2026-06-07 — V23R4 walking and mounted camera micro-jitter

- User reported continuing micro-jumps and apparent zoom pulses while walking and riding.
- Static inspection found the scene inset left 14.75 usable room units for a 15.25 camera boom, so normal framing was already compressed at room center.
- Normal follow also recast against structural walls and reconstrained the look point with the same large inset; room resolution could be requested repeatedly in one frame.
- V23R4 keeps authored maze geometry unchanged and repairs camera containment, look-point smoothing, room caching, and handoff-only wall casting instead.
- Unity and Play Mode results remain unverified until run on the user's machine.

## 2026-06-07 — V23R3B room-boundary documentation QA compatibility

- V23R3 and V23R3A installed and validated successfully; repository hygiene passed after cleanup.
- Unity TEST EVERYTHING then reported one blocker only: `ROOM_BOUNDARY_DESIGN_MISSING` for the missing token `visibility boundary`.
- The maintained design already described the behavior, so V23R3B adds the explicit durable term without changing Runtime or scene behavior.
- Unity TEST EVERYTHING and focused closed-wall Play Mode verification remain required.

## 2026-06-07 — V23R3A Codex instruction and repository-hygiene repair

- V23R3 installer and semantic validator passed on the user's current working tree.
- The subsequent repository-hygiene check blocked only because `AGENTS.md` was not listed as canonical root Markdown.
- `AGENTS.md` and `.codex/` are intentional current project inputs and must be preserved.
- `AGENTS.rtf` is classified as a local duplicate and is ignored rather than deleted.
- V23R3A updates the documentation map, workflow, technical decision, external hygiene script, and Unity governance QA without touching Gameplay or scene behavior.

## 2026-06-07 — V23R3 remote/local synchronization and QA compatibility

- User supplied the full local repository ZIP and required reconciliation with current GitHub without losing either side.
- Actual remote main was verified at `40104177ee396d19f375b62a20c410e4ac63bdc8`, 33 documentation-only commits ahead of the stale local `origin/main` reference.
- Local HEAD `99daaee` and the dirty working tree contain unique Runtime, scene, QA, package, and Codex progress.
- Latest TEST EVERYTHING report contains seven static blockers: one obsolete camera field, two missing permanent-document tokens, two obsolete post-recovery text anchors, and two frozen status-category strings.
- V23R3 supersedes the proposed text-only compatibility repair: it preserves the stronger active Runtime contracts, synchronizes documentation, and makes QA semantic/version-agnostic.
- Unity compilation and Play Mode remain unverified until run on the user's machine.
