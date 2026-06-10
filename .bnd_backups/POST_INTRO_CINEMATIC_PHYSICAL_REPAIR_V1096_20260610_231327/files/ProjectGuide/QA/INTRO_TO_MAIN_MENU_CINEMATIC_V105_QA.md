<!-- BND_POST_INTRO_CINEMATIC_PHYSICAL_CORRECTION_V1095:BEGIN -->
## V10.9.5 focused regression matrix

1. Scrub from the last BBH frame into the cinematic one frame at a time. Reject any glimpse of the ordinary/final menu pose, default camera, wrong FOV, wrong page or restored object pose before the establishing frame.
2. Confirm frame 0 already equals the authoritative high/far/left start state and frame 1 continues smoothly from it; position, rotation, FOV, look target, screen content and exposure must be continuous.
3. Confirm the handheld lies flat on the tabletop before, during and after the shot. It may not stand upright, tilt into a display stand, slide, rotate or change scale.
4. Confirm a tight device contact shadow and coherent tabletop contact with no floating gap or geometry penetration.
5. Confirm physical proportions are believable: handheld approximately 14–16 × 8–10 × 2–3 cm versus table approximately 140–180 × 75–90 × 72–76 cm, or equivalent documented ratios.
6. Measure project-unit ratios: handheld long axis approximately 8–12% of table width; handheld short axis approximately 9–14% of table depth. Record actual values in the verification result.
7. Confirm the wide opening reads as a complete table with a small portable device, not a giant console on a miniature table.
8. Confirm the close final frame is achieved solely by camera travel/lens tuning; device/table transforms and scales remain byte-for-byte/approximately constant throughout playback.
9. Verify the final ordinary Main Menu pose remains exact, screen readable, front tabletop edge/thickness visible and input enabled only after full settle.
10. Repeat at 24-equivalent capture, 30 FPS, 60 FPS and unstable frame pacing; no pre-roll flicker or frame-dependent difference is permitted.
11. Return from Settings, Progression and Credits and confirm the special shot does not replay and the flat rest pose/proportions remain unchanged.
12. Run `Boredom And Dungeons -> TEST EVERYTHING`; require `0 blockers / 0 warnings / 0 info` after the Runtime/QA implementation exists.
<!-- BND_POST_INTRO_CINEMATIC_PHYSICAL_CORRECTION_V1095:END -->

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
