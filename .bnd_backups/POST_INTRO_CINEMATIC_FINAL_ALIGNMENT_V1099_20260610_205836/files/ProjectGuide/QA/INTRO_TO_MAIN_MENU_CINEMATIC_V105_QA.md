<!-- BND_POST_INTRO_CINEMATIC_VISUAL_REPAIR_V1098:BEGIN -->
## V10.9.8 focused regression matrix

1. Run `TEST EVERYTHING`; require `0/0/0`.
2. During every visible frame inspect/log the device root and require exact position `(0,-7.27,0)`, rotation `(90,0,0)` and scale `(0.16,0.16,0.16)`.
3. Reject any `Vector3.one` scale write, entry slide/tilt, or ordinary-menu transform animation.
4. Verify the deepest rear-shell point remains above the tabletop with a tight contact shadow and no clipping or floating.
5. Confirm the final camera is `(0,1.50,-14.0)`, looks at `(0,-7.20,0)`, and resolves to `17f -> 15f` FOV by aspect.
6. Resize the Game view before, during and after the shot. No resize may restore the retired `49f -> 36.4f` FOV or change object scale.
7. Verify the whole screen and controls read naturally in the final frame without the low grazing distortion shown in the rejected screenshot.
8. Scrub BBH-to-shot frame 0/1 and the final 12 frames for no flash, snap, residual movement or correction.
9. Repeat at 24-equivalent, 30 FPS, 60 FPS, uneven frame pacing and internal-menu return paths.
10. Recheck the complete V10.8.1 mechanics matrix before acceptance.
<!-- BND_POST_INTRO_CINEMATIC_VISUAL_REPAIR_V1098:END -->

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
