# QA Checklist — Required Verification Gates

Actual pass/fail truth belongs in `PROJECT_STATUS.md`.

## Single required Unity QA command

```text
Boredom And Dungeons -> TEST EVERYTHING
```

New automated checks integrate into this command; do not create another mandatory QA action.

## Gate order

1. **Repository/preflight:** required files, stable `.meta`, no real conflict markers, no staged package/cache/duplicate-status artifacts.
2. **Package validation:** syntax, first install, second install for idempotency, validator, `git diff --check`.
3. **Unity compilation:** documented Unity version, no compiler errors, no unexplained compiler warnings, no project-generated red Console error.
4. **Automated QA:** TEST EVERYTHING, zero blockers; warnings fixed or explicitly accepted.
5. **Console cleanliness:** no repeated project-generated warning spam, edit-mode material leak, charged-shot warning, or AudioListener warning.
6. **Focused Play Mode:** changed behavior, repeated use, death/restart/re-entry, nearby-system regression checks.
7. **Performance when relevant:** measure CPU/GPU/memory/GC/draw calls/loading on a representative target.
8. **Documentation truth:** record real results and exact resume point in `PROJECT_STATUS.md`.


## Active V23R6 node-transition camera diagnosis gate

1. **Room/node identity:** log every current-room identity change and legal handoff start/end while walking and riding.
2. **Camera distance:** record desired and final camera-to-target distance; neither may pulse during a legal transition.
3. **Camera orientation:** changing room/node metadata alone must not change yaw, pitch, or look direction.
4. **Target ownership:** record whether the camera target is the player or horse before, during, and after every reproduced jump.
5. **FOV truth:** verify whether FOV actually changes and distinguish FOV zoom from camera-distance compression.
6. **Root versus model:** measure player and horse gameplay roots separately from visual model and animation movement.
7. **Reproduction:** reproduce the issue on foot and mounted before changing maze geometry.
8. **No speculative resize:** keep rooms and corridors unchanged unless measurements prove insufficient space is the source.
9. **Focused acceptance:** repeated node transitions produce no direction snap, zoom pulse, or forward/back screen-space jump.
10. **Retained safety:** closed walls, containment, cinematic ownership, first visible New Game frame, holes, combat, and Console behavior remain passing.
## Active V23R5 first-frame mounted-camera gate

1. **No pre-cinematic leak:** after selecting New Game, no frame shows the camera beside the entrance, attached to the player/horse, or at normal gameplay follow framing.
2. **First visible pose:** the first visible gameplay frame is already the approved higher/farther inside-room shot looking at the entrance.
3. **Ownership order:** `sceneLoaded` disables `BDCameraFollow` and primes camera position/rotation/FOV before the presentation coroutine and before any frame yield.
4. **Fallback safety:** if the mounted intro cannot resolve, normal camera ownership is restored before the black cover is released.
5. **Retained flow:** input remains locked, the horse enters, turns right, fully stops, and only then the normal camera and controls return.
6. **Legacy QA alignment:** V20 checks validate the active V23R4 safety inset and handoff-only wall-cast contract rather than obsolete `3.40f` and `pushed inside closed room wall` strings.

## Active V23R4 camera micro-jitter gate

1. **Room-center distance:** while standing near the center of a room, rotate the aim through full cardinal and diagonal directions; camera distance and FOV remain visually constant.
2. **Walking wall pressure:** walk parallel to and toward each closed wall. The camera eases against containment without one-frame jumps, repeated micro-zooms, or pitch pulses.
3. **Mounted wall pressure:** repeat at horse speed, including corners and diagonal movement; higher movement speed does not reintroduce jitter.
4. **Look-point stability:** player/horse vertical settling and small floor changes do not make the framing bob or pulse.
5. **Room handoff:** cross legal openings repeatedly in both directions. The physical wall cast is active only during handoff, and the transition preserves distance/FOV without clipping through closed wall segments.
6. **Closed-wall visibility:** the reduced stable inset does not expose adjacent rooms from frontal, side, corner, diagonal, high, or low views.
7. **Performance:** normal follow does not execute repeated scene-wide room scans in one frame; no recurring camera-related GC allocation or spike is visible in the Profiler when checked.
8. **No map regeneration regression:** room, wall, doorway, enemy, hazard, portal, minimap, and authored-scene placements remain unchanged.

## Active V23R3 regression gate

1. **Single camera owner:** `BDCameraFollow` is the only normal-gameplay Main Camera transform owner; the old viewport-bias source is absent.
2. **Stable yaw/pitch:** wall proximity, enemies, damage, and room/tile handoff do not alter mouse sensitivity or gameplay pitch.
3. **Semantic camera QA:** automated checks validate the current rate-limited single-stage camera contract and do not require the obsolete `movementDirectionBlend` field.
4. **Planar shake:** combat shake has no vertical component.
5. **Closed walls:** on foot and mounted, side/corner/diagonal rotation never reveals an adjacent room; the maintained room-boundary design explicitly defines closed walls as a `visibility boundary`.
6. **Room/tile handoff:** cross several legal transitions in both directions. Distance/FOV/pitch remain stable with no snap or zoom.
7. **Combat grounding:** repeated enemy hits near walls and transitions do not move the player below the floor.
8. **Root-safe recovery:** any forced recovery places the CharacterController capsule fully above ground and movable.
9. **Walking-proof hole boundary:** approach every side and corner of the hole using ordinary walking at slow/full speed and diagonal angles; walking never enters.
10. **Swept footprint:** hazard filtering checks intermediate path samples and capsule footprint, not only final center position.
11. **Active intent only:** intentional gap entry is limited to active dodge, actively ascending jump, or explicit forced movement. Recent historical dodge/jump timestamps do not authorize later walking entry.
12. **Post-recovery suppression:** the named `0.85s` suppression contract blocks gap intent and forced-gap requests immediately after recovery; QA does not require obsolete `0.55s` text anchors.
13. **Near-hole respawn:** intentional fall recovery uses a valid local point just outside the same hole before older safe points or spawn.
14. **Respawn safety:** local recovery is outside all hazard volumes, above ground, free of overlap, and does not loop.
15. **Version-agnostic status QA:** active-work validation checks semantic headings and a Runtime version marker rather than a frozen category/version string.
16. **Existing regressions:** charged shot, AudioListener, mounted intro, BBH first frame, current-status QA, and V22R2 Console cleanup remain passing.
17. **Console:** no project-generated red errors or repeated warnings.

## Remote/local synchronization gate

- The remote reference used for this repair is `main` at `40104177ee396d19f375b62a20c410e4ac63bdc8`.
- Remote-only maintained-document improvements are present in the merged working tree.
- Local-only Runtime, scene, QA, `.meta`, package, and Codex work remains present.
- No reset, clean, broad checkout, or old full-file package replacement was used.
- Before push, create a safety branch/tag, commit the Unity-verified merged state, fetch `origin`, merge `origin/main`, rerun diff/hygiene checks, and inspect the merge result.

## Repository-hygiene gate

- Run `python3 tools/check_repository_hygiene.py` before handoff and commit.
- `AGENTS.md` is present as canonical maintained root Markdown and is scanned by TEST EVERYTHING.
- `.codex/config.toml` and `.codex/agents/` remain available as maintained project configuration.
- `AGENTS.rtf` is ignored as a local rich-text duplicate and must not be staged.
- Root Markdown matches `DOCUMENTATION_INDEX.md`.
- No obsolete roadmap/status copy, package README/manifest, repair narrative, chat export, or copied QA report remains tracked.
- Every removed document had valid requirements merged first.
- `PROJECT_STATUS.md` matches the real current state and resume point.

## V23R3B documentation compatibility gate

- `Assets/_Project/Design/Map/ROOM_BOUNDARY_CAMERA_AND_TEXTURE_READINESS.md` contains the exact durable term `visibility boundary`.
- This documentation repair does not replace Runtime or scene verification.
- Rerun TEST EVERYTHING and confirm `ROOM_BOUNDARY_DESIGN_MISSING` is absent.

## Truthful status language

- Static validator passed does not mean Unity compiled.
- Unity compiled does not mean Play Mode passed.
- Automated PASS does not mean the Console is clean.
- One Play Mode pass does not prove restart/re-entry behavior.
- No profiler data means performance remains unverified.
- A task is not DONE until all applicable gates pass.
