<!-- BND_POST_INTRO_CINEMATIC_QA_CONTRACT_ALIGNMENT_V10922:BEGIN -->
## V10.9.22 authoritative camera-QA contract

The Runtime transition contract is authoritative:

```text
new Vector3(0f, -1.68f, -4.18f)
new Vector3(0f, -7.18f, -4.18f)
new Vector3(-0.18f, 0.08f, -6.72f)
Mathf.Lerp(38.8f, 31.8f, fit)
```

`HANDHELD_3D_PHYSICAL_STAGING_MISSING` must require these values. `HANDHELD_3D_OLD_CAMERA_REMAINS` must never forbid them. `BDFirstLaunchTutorialQA.RequiredRuntimeTokens` must contain each value exactly once. Retired V10.9.14/V10.9.15 camera values remain forbidden but are not required.
<!-- BND_POST_INTRO_CINEMATIC_QA_CONTRACT_ALIGNMENT_V10922:END -->

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
