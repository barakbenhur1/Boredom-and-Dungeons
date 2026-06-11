<!-- BND_CHILD_APPROACH_CINEMATIC_FADE_SPEED_SMOOTHING_V10930:BEGIN -->
## 2026-06-11 — Child approach fade, speed and transition smoothing V10.9.30

The post-intro child POV cinematic remains the active task. This pass:

- starts the child farther behind and farther left of the chair while preserving the accepted child eye height;
- begins from a fully black frame and fades the room in before visible movement starts;
- shortens the complete sequence from `8.25s` to `7.45s` without rushing the screen power-on;
- replaces the segmented chair climb with a tangent-continuous Catmull-Rom route;
- blends the chair-clearance release instead of switching at a hard threshold;
- replaces the two-step post-climb look movement with one cubic camera curve and one cubic look-target curve;
- preserves the accepted screen power direction and content fade-in.

Acceptance still requires Unity compilation, `TEST EVERYTHING 0/0/0`, and real-time visual review.
<!-- BND_CHILD_APPROACH_CINEMATIC_FADE_SPEED_SMOOTHING_V10930:END -->

<!-- BND_CHILD_APPROACH_CINEMATIC_PATH_CLEARANCE_V10929:BEGIN -->
## V10.9.29 visual checkpoints
- first camera position is visibly farther from the chair and farther left;
- POV height matches the accepted V10.9.28 height language;
- six small grounded steps lead to the left-rear side of the chair;
- no camera point crosses the chair backrest while below/inside its height range;
- the camera remains outside the left edge until the seat-side waypoint;
- inward movement occurs only after the camera is safely in front of the backrest and above the seat;
- power-on/content reveal remains unchanged.
<!-- BND_CHILD_APPROACH_CINEMATIC_PATH_CLEARANCE_V10929:END -->

<!-- BND_CHILD_APPROACH_CINEMATIC_POLISH_V10928:BEGIN -->
## V10.9.28 visual checkpoints
- opening camera behind the chair, slightly left, not near the floor;
- chair backrest between camera and seat at frame zero;
- five restrained steps with no strong shake;
- curved two-stage climb with no straight drone lift;
- screen dark through settlement;
- content begins only after glass/backlight wake and feeds in visibly;
- final tutorial state and skip state are identical.
<!-- BND_CHILD_APPROACH_CINEMATIC_POLISH_V10928:END -->

<!-- BND_CHILD_APPROACH_CINEMATIC_V10927:BEGIN -->
## V10.9.27 child-approach QA

Required visual checkpoints: behind-chair child-height opening; six restrained walking steps; natural slowdown; two-stage climb; balance correction; final exact camera; screen fully dark through camera settlement; power-on only after settlement; no white frame; tutorial content revealed during power-on; skip lands in the same valid final state.
<!-- BND_CHILD_APPROACH_CINEMATIC_V10927:END -->

<!-- BND_POST_INTRO_CHAIR_AND_QA_REPAIR_V10926:BEGIN -->
## 2026-06-11 — Post-intro chair + QA access repair V10.9.26
- Added a centered wooden dining chair in front of the table, aligned with the handheld.
- The chair uses the new `BDCinematicDiningChair` texture resource and stays in the same physical room staging.
- Added chair geometry and chair contact shadows in `BDModernHandheld3DPresenter.CinematicEnvironment.cs`.
- Repaired the Unity editor compile blocker by changing `BDFirstLaunchTutorialQA.Scan()` from private to internal so `BDOneClickQAWindow` can call it.
- Runtime focus: no limbo/cyclorama reintroduction; room staging remains the active implementation.
<!-- BND_POST_INTRO_CHAIR_AND_QA_REPAIR_V10926:END -->

<!-- BND_POST_INTRO_REAL_ROOM_AND_CLOSER_FRAMING_V10925:BEGIN -->
## V10.9.25 real-room QA matrix

Require:

- `BuildCinematicRoomFloor`;
- `Cinematic Real Room Floor`;
- `BD Cinematic Warm Room Floor`;
- `Cinematic Exact Fruit Wallpaper Back Wall`;
- `Cinematic Wallpaper Baseboard`;
- `Cinematic Warm Room Fill`;
- `Cinematic Wallpaper Wall Wash`;
- final camera `new Vector3(0f, -1.94f, -4.18f)`;
- final approach `new Vector3(-0.14f, -0.22f, -6.48f)`;
- final lens `Mathf.Lerp(37.8f, 30.6f, fit)`.

Forbid:

- `BuildCinematicFloorAndCyclorama`;
- `Cinematic Cyclorama`;
- `BD Cinematic Charcoal Floor`;
- `BD Cinematic Charcoal Cyclorama`.

Visual acceptance: no ramp/limbo, only a distant wallpapered back wall, warm readable room lighting, and a slightly closer complete-device final frame.
<!-- BND_POST_INTRO_REAL_ROOM_AND_CLOSER_FRAMING_V10925:END -->

<!-- BND_POST_INTRO_FINAL_FIRST_LAUNCH_QA_REPAIR_V10924:BEGIN -->
## V10.9.24 authoritative first-launch look-target contract

The named `camera-only full-set intro-to-main-menu cinematic` contract in `BDFirstLaunchTutorialQA.cs` must contain exactly one final-look target.

That target must be discovered from the current `HANDHELD_3D_PHYSICAL_STAGING_MISSING` Require block in `BDModernHandheld3DQA.cs` and must also exist in the transition Runtime.

No historical intermediate final-look target may remain in the named first-launch contract.
<!-- BND_POST_INTRO_FINAL_FIRST_LAUNCH_QA_REPAIR_V10924:END -->

Resolved authoritative final-look target: `new Vector3(0f, -7.18f, -4.18f)`.

<!-- BND_POST_INTRO_CINEMATIC_WALLPAPER_FOCUS_DELIVERY_REPAIR_V10916:BEGIN -->
## V10.9.16 framing/focus/wallpaper QA matrix

### Automated contracts

Require:

- final camera `new Vector3(0f, -1.58f, -3.94f)`;
- final target `new Vector3(0f, -7.22f, -3.94f)`;
- responsive final lens `Mathf.Lerp(40.5f, 34.6f, fit)`;
- DOF values `2.15f`, `8.20f`, `0.28f`, `2.10f` and full-resolution descriptors;
- wallpaper tokens: `CreateCinematicWallpaperMaterial`, `BuildCinematicWallpaperPanels`, `Cinematic Kitchen Wallpaper Back Wall`, `BD Cinematic Kitchen Wallpaper`;
- all previously accepted room-dressing and one-camera contracts.

Forbid the previous stronger blur settings and older final camera tokens.

### Visual checks

- the whole handheld is visible in the final frame;
- a small strip of wood remains visible under the device;
- the blur is subtle and clean;
- wallpaper reads as part of the room mostly during the wider phase of the shot;
- screen readability is not harmed.
<!-- BND_POST_INTRO_CINEMATIC_WALLPAPER_FOCUS_DELIVERY_REPAIR_V10916:END -->

<!-- BND_POST_INTRO_CINEMATIC_DIRECTOR_PASS_V109:BEGIN -->
## V10.9 director-pass focused Play Mode matrix

1. Confirm total movement is approximately 4.40 seconds and begins subtly on the first rendered frame.
2. Opening frame: complete tabletop, apron/frame, at least three readable legs, floor before/behind, dark non-black cyclorama and the existing active handheld.
3. At 1.3 seconds: forward/downward motion is clear, legs remain visible and there is no cut, ghosting, duplicate furniture or object-scale change.
4. At 2.2 seconds: camera is materially closer/less left-offset; perspective and table scale remain physically coherent.
5. At 3.3 seconds: horizontal alignment is almost complete; legs leave only through framing; tabletop thickness/front lip remain readable.
6. Final frame: exact Main Menu composition, centered device, zero roll, readable screen, no legs, visible top plus front edge/apron occupying a meaningful lower-frame band.
7. Inspect frame by frame for continuous position, rotation, FOV, focus/exposure appearance and screen content. Reject any late correction or snap.
8. Verify device/table/floor/cyclorama/shadows remain static by logging or inspecting transforms during playback.
9. Verify input is locked through the final stable frame and enabled exactly once without auto-activating a menu item.
10. Verify Settings, Progression, Credits, Pause/gameplay return and submenu close never replay the shot.
11. Verify the development-only replay command works in Editor/Development Build and is absent from production behavior.
12. Repeat at 30 FPS and 60 FPS and capture a 24 FPS review recording. Motion and final state must be identical.
13. Run `Boredom And Dungeons -> TEST EVERYTHING`; require `0 blockers / 0 warnings / 0 info`.
<!-- BND_POST_INTRO_CINEMATIC_DIRECTOR_PASS_V109:END -->

# Intro-to-Main-Menu Cinematic V10.5 QA

Status: `UNITY PLAY MODE REQUIRED`.

1. With tutorial completed, verify BBH ends on a wide angled active-handheld shot.
2. Verify the real menu content exists from the first cinematic frame.
3. Verify position, rotation and FOV move continuously as one shot.
4. Verify the final pose matches ordinary main-menu entry exactly.
5. Verify input is unavailable until completion.
6. Verify Settings, Progression, Credits and gameplay returns do not replay it.
7. Reset first launch and choose Play; the pending cinematic is cancelled.
8. Reset first launch and choose Skip; the first resulting main menu consumes it.
9. Verify no legacy menu, wrong page, empty/black screen or texture pop.
10. Verify `B&D` and `Boredom & Dungeons` have clear vertical separation.
11. Run TEST EVERYTHING with zero blockers, warnings and info.

<!-- BND_FIRST_LAUNCH_TUTORIAL_MECHANICS_REPAIR_V108:BEGIN -->
## V10.8 visual rejection repair

12. Confirm the shot occupies the complete application viewport and contains the real 3D table, handheld, live screen, glass and shadow.
13. Confirm camera perspective changes through actual 3D position/look/FOV, not a screen-space slide or scale.
14. Confirm the real device, table, floor, cyclorama and every shadow remain completely static; only the existing product camera moves.
15. Confirm there is no flat image/card, crop, black correction frame, wrong-page frame or PowerPoint-like easing.
<!-- BND_FIRST_LAUNCH_TUTORIAL_MECHANICS_REPAIR_V108:END -->
