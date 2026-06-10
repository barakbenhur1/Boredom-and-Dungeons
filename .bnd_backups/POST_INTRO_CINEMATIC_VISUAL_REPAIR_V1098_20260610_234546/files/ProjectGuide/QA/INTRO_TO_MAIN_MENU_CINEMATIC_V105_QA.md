<!-- BND_POST_INTRO_CINEMATIC_QA_CONTRACT_REPAIR_V1097:BEGIN -->
## V10.9.7 automated and focused QA matrix

1. Run `Boredom And Dungeons -> TEST EVERYTHING`; require `0 blockers / 0 warnings / 0 info`.
2. Confirm `BDModernHandheld3DQA` no longer requires `new Vector3(0f, 0.28f, 0f)` and still requires the V10.9.6 flat pose, scale, first-frame priming, camera and resized-shadow contracts.
3. Record the last visible BBH frames and first cinematic frames. Reject any final/default/ordinary menu pose before the establishing frame.
4. Frame 0 must already use the exact start camera/look/FOV/clip state and live screen. Frame 1 must continue smoothly with no snap.
5. Confirm the device root remains at the documented flat rest position, 90-degree X rotation and scale `0.16` before, during and after the shot.
6. Confirm the screen faces upward and the device never stands upright, rotates or scales for readability.
7. Confirm a tight contact shadow plus restrained soft/core penumbra matches the `1.568 × 2.464` footprint; reject the retired giant shadow dimensions.
8. Confirm the device occupies `6.53%` of table width and `19.87%` of table depth.
9. Review 0.0s, 1.3s, 2.2s, 3.3s and final frame for one-camera continuity, static objects, natural leg exit, readable screen and preserved table edge/thickness.
10. Repeat at 24-equivalent capture, 30 FPS, 60 FPS and uneven frame pacing.
11. Return from Settings, Progression, Credits, Pause and gameplay; the shot must not replay and the flat pose/scale must remain identical.
12. Recheck the complete V10.8.1 mechanics matrix before acceptance.
<!-- BND_POST_INTRO_CINEMATIC_QA_CONTRACT_REPAIR_V1097:END -->

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
