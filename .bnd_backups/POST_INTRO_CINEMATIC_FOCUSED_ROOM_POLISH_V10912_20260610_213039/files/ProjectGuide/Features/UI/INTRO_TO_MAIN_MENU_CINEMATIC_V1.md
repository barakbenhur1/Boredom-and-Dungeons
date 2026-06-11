<!-- BND_POST_INTRO_CINEMATIC_FINAL_ALIGNMENT_V1099:BEGIN -->
## V10.9.9 authoritative front placement and direct final-menu pose

- Physical device root: `position = (0,-7.27,-3.60)`, `rotation = (90,0,0)`, `scale = 0.16`.
- Table front edge is `z = -7.20`; the device's nearest edge is approximately `z = -4.832`, preserving about `2.368` units of wood. This is intentionally closer to the front edge without contact.
- The final ordinary-menu camera is normal to the screen plane: position `(0,1.50,-3.19)`, look target `(0,-7.17,-3.19)`, rotation `Quaternion.Euler(90,0,0)`.
- The final lens is the original menu resolver `Mathf.Lerp(49f, 36.4f, fit)`. Because the camera is now perpendicular to the screen, this restores the original straight-on menu relationship without the previous grazing distortion.
- The 4.40-second path, phase boundaries and BBH hidden-frame priming remain. Only the spline's destination-side control points and target tracking are adjusted to reach the direct pose smoothly.
- The table, device and shadows remain static throughout the shot. Shadows and lighting are authored relative to the new device position.
- `entryProgress` is retired; no generic menu-entry state may own the physical device transform.
<!-- BND_POST_INTRO_CINEMATIC_FINAL_ALIGNMENT_V1099:END -->

<!-- BND_POST_INTRO_CINEMATIC_DIRECTOR_PASS_V109:BEGIN -->
## V10.9 authoritative director and staging contract

The post-BBH transition is a 4.40-second, one-camera, one-scene product reveal based on the supplied 100-frame/24 FPS motion reference and the later director instructions.

- Opening: high, farther away and left-offset; a complete table and floor establish physical scale.
- Environment: real thick tabletop, front lip, apron/frame, four legs/feet, charcoal floor and curved cyclorama. No plane-only table or black void.
- Device: the approved existing handheld remains stationary, centered on the table depth and active from the first frame.
- Path: five natural-cubic knots provide continuous position/acceleration; horizontal alignment finishes before forward/downward travel; look and FOV use separate jerk-limited clocks.
- Lens: perspective only, restrained start-to-final FOV change, no zoom substitution, no roll, no camera noise and no exposure transition.
- Final: exact ordinary Main Menu transform/FOV/clip state, device centered, no legs, but the tabletop front edge, thickness and shadowed apron remain visible.
- Handoff: input unlocks only after the exact final pose. No correction frame, screen swap, camera swap, cut, fade or internal-menu replay is permitted.
- Development: an editor/development-build context command may replay the shot without altering production one-shot persistence.
<!-- BND_POST_INTRO_CINEMATIC_DIRECTOR_PASS_V109:END -->

# Intro-to-Main-Menu Cinematic V1

Status: `IMPLEMENTED / UNITY VERIFICATION REQUIRED`.

The special camera move is a one-shot transition from the completed BBH intro to the first real main-menu presentation. It is not a generic main-menu entry animation.

## Explicit mode contract

- `IntroToMainMenuTransition`
- `RegularMainMenuEntry`

`BDBBHBootIntro` publishes a one-shot completion request. The modern handheld consumes it only when the effective destination is the real main menu. `PLAY TUTORIAL` explicitly cancels the request. `SKIP TUTORIAL` preserves it until the first main-menu destination.

The mode must not be inferred from time, camera position, scene load or menu visibility.

## Camera contract

- farther and differently angled opening pose;
- more table/environment visible;
- real active main-menu screen rendered from the opening frame;
- cubic spatial path plus smoother-step position, rotation and FOV interpolation;
- exact final ordinary-menu transform and FOV;
- input locked until completion;
- no overshoot, hard cut, legacy-menu frame, black replacement texture or correction frame.

The special shot never runs on return from Settings, Progression, Credits, gameplay, Pause or later ordinary menu entries.

<!-- BND_POST_INTRO_TRANSITION_COLORED_OUTPUT_CLEAN_EXIT_V1072:BEGIN -->
## Post-BBH landing contract

The one-shot cinematic is tied to completion of BBH, not to Main Menu specifically. Its legal landing page is either the tutorial choice screen or the regular Main Menu. The request is consumed there and can never survive until tutorial completion.
<!-- BND_POST_INTRO_TRANSITION_COLORED_OUTPUT_CLEAN_EXIT_V1072:END -->

<!-- BND_FIRST_LAUNCH_TUTORIAL_MECHANICS_REPAIR_V108:BEGIN -->
## V10.8 full-screen real-3D presentation correction

The special transition is rendered by the normal full-screen product camera looking at the actual 3D handheld/table scene. It may animate only the cached camera transform, FOV and clip values. The device, table, floor, cyclorama and every shadow root remain static. It may not approximate the shot with screen-space scale, a 2D card, a flat overlay, a slide transition or a cropped image.

The opening pose exposes the complete table and grounded set from a distinct high/left angle. A natural-cubic path and continuously recomputed look target move toward the ordinary menu pose. The final interval reaches the same centralized camera state directly, so no device/shadow restore or corrective frame is required. Screen content remains live and input remains locked through completion.
<!-- BND_FIRST_LAUNCH_TUTORIAL_MECHANICS_REPAIR_V108:END -->
