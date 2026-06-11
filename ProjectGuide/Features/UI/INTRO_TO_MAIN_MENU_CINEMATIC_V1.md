<!-- BND_CHILD_APPROACH_ROOM_ENTRANCE_FADE_V10931:BEGIN -->
## 2026-06-11 — Kitchen-entrance start, complete room shell and filmic fade V10.9.31

The post-intro child POV cinematic remains the active task. This pass implements the latest visual corrections in the game:

- the child now starts at a kitchen-entrance distance, farther behind and farther left of the chair;
- the entrance-to-chair walk follows a curved cubic route instead of a direct straight-line interpolation;
- the first frame stays fully black for `0.42s`, followed by a dedicated filmic fade that completes at `1.20s` before walking begins;
- the room now contains a real ceiling and a physical right wall, both extending across the camera route;
- the right wall continues the approved fruit wallpaper and includes a matching baseboard;
- the sequence remains shorter than the older `8.25s` version while allowing the longer entrance route and professional fade;
- the accepted chair climb, screen-off contract, power-on direction and animated tutorial-content reveal are preserved.

Acceptance requires Unity compilation, `TEST EVERYTHING 0/0/0`, and real-time visual review from the first black frame through tutorial readiness.
<!-- BND_CHILD_APPROACH_ROOM_ENTRANCE_FADE_V10931:END -->

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

<!-- BND_POST_INTRO_REAL_ROOM_AND_CLOSER_FRAMING_V10925:BEGIN -->
## V10.9.25 real-room staging addendum

The post-intro shot takes place in one normal room. The floor is a large physical wooden floor extending beyond the complete camera path. The only wall visible in the composition is the fruit-wallpaper back wall, positioned a believable distance behind the table. No cyclorama, curved ramp, black stage, or visible side-wall boundary is permitted.

The final camera is modestly closer than the prior accepted direction but must still contain the complete handheld and a small strip of tabletop below it.
<!-- BND_POST_INTRO_REAL_ROOM_AND_CLOSER_FRAMING_V10925:END -->

<!-- BND_POST_INTRO_CINEMATIC_BACKWALL_STABLE_REPAIR_V10921:BEGIN -->
## V10.9.21 back-wall delivery architecture

The exact wallpaper is delivered through `BDModernHandheld3DPresenter.CinematicWallpaperBackWall.cs`, a dedicated partial runtime file. The main cinematic environment calls one exact-wall builder. Legacy procedural wallpaper panels may remain as unused code for compatibility, but their build call is disabled, so no side wallpaper walls are spawned.
<!-- BND_POST_INTRO_CINEMATIC_BACKWALL_STABLE_REPAIR_V10921:END -->

<!-- BND_POST_INTRO_CINEMATIC_WALLPAPER_FOCUS_DELIVERY_REPAIR_V10916:BEGIN -->
## V10.9.16 framing, focus and wallpaper addendum

The post-intro shot now settles in a composed product frame that contains the entire handheld. The device remains the hero, but the final frame intentionally leaves a small amount of tabletop visible beneath it so the shot still reads as an object resting on furniture rather than a floating UI plate.

Depth of field is present only as a restrained photographic aid. The effect uses full-resolution blur passes and softer settings so the handheld and immediate tabletop stay crisp while the room falls gently out of focus.

The room set gains a kitchen-cartoon-inspired wallpaper treatment generated in code and applied to thin wall panels across the back and side walls. The pattern is warm, graphic and subtle enough to support the space without stealing attention from the device.
<!-- BND_POST_INTRO_CINEMATIC_WALLPAPER_FOCUS_DELIVERY_REPAIR_V10916:END -->

<!-- BND_POST_INTRO_CINEMATIC_FOCUSED_ROOM_POLISH_V10912:BEGIN -->
## V10.9.12 focused-room and final-framing addendum

The single continuous post-BBH camera shot now ends in a tighter, direct product frame. At common wide aspect ratios the full horizontal handheld should occupy approximately 70%–80% of frame height while preserving a controlled border of tabletop. The device remains horizontal, physically seated and slightly below frame center.

The room is no longer an empty studio placeholder. The complete table/floor/cyclorama set is dressed with a dark woven rug, walnut credenza, warm practical lamp, books, plant, wall slats and framed accents. These objects establish a believable space during the wide and middle portions; they must not compete with the device.

A built-in-pipeline depth-of-field image effect follows the handheld as the focus target. Focus distance is derived from the real camera/target geometry every frame. The device and immediate tabletop remain sharp; near floor and distant room dressing soften by physical distance. No screen-space fake device scale, second camera, crossfade or focus jump is permitted.
<!-- BND_POST_INTRO_CINEMATIC_FOCUSED_ROOM_POLISH_V10912:END -->

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
