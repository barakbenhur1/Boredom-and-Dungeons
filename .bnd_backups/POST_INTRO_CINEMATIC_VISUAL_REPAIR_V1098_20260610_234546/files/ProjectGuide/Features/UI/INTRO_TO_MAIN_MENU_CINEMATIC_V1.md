<!-- BND_POST_INTRO_CINEMATIC_PHYSICAL_REPAIR_V1096:BEGIN -->
## V10.9.6 authoritative first-frame, flat-pose and scale contract

- While BBH is still rendering its opaque/fading layer, the product camera is already placed at the exact establishing pose. The transition timer begins only after this pose and the live screen have been applied.
- The first exposed frame can therefore only be the high/far/left opening frame. An ordinary-menu, final, default, restore or wrong-FOV frame before it is forbidden.
- The portrait-form handheld lies flat: fixed rest rotation `Quaternion.Euler(90f, 0f, 0f)`, fixed tabletop contact position and fixed scale `0.16`.
- Its short axis remains parallel to the table front edge and measures `1.568 / 24 = 6.53%` of table width. Its long axis runs front-to-back and measures `2.464 / 12.4 = 19.87%` of table depth. These ratios correspond to a roughly `9.8 × 15.4 cm` handheld on a roughly `150 × 78 cm` table.
- Camera travel and lens tuning alone create the close final composition. Device, table, floor, cyclorama and shadow roots remain static.
- The final camera is the same ordinary Main Menu camera used after the cinematic; no restore/correction frame follows the settle. The screen remains readable and the front tabletop edge/thickness stays visible.
<!-- BND_POST_INTRO_CINEMATIC_PHYSICAL_REPAIR_V1096:END -->

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
