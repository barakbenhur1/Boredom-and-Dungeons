<!-- BND_POST_INTRO_CINEMATIC_PHYSICAL_CORRECTION_V1095:BEGIN -->
## V10.9.5 authoritative physical-staging and first-frame contract

### First visible frame

The ordinary/final Main Menu pose must never be exposed before the special shot begins. Before presentation becomes visible, the existing product camera, look target, FOV/clip state and live screen must already be at the exact establishing values. The first rendered frame is therefore the opening pose itself, not a default pose followed by a snap. No one-frame restore, final-pose flash, camera enable-order mismatch or late initialization is permitted.

### Handheld rest orientation

The handheld retains its portrait industrial design, but in this scene it lies flat on the tabletop. It is not staged upright as a display stand. Its transform, scale and contact relationship remain constant throughout the shot. The camera is positioned high enough and pitched toward the screen so the display becomes readable naturally while the device stays physically plausible.

### Physical scale

Use one coherent real-world scale family. Baseline ranges are:

- handheld: about 14–16 cm long, 8–10 cm wide and 2–3 cm thick;
- table: about 140–180 cm wide, 75–90 cm deep and 72–76 cm high.

Equivalent project-unit ratios are valid when the handheld long axis reads at about 8–12% of table width and its short axis at about 9–14% of table depth. Do not scale either object for composition or animation. The camera path is the only source of apparent enlargement.

The final framing may fill the viewport with the device because the camera is close, but continuity from the wide shot, visible tabletop thickness and stable perspective must preserve the knowledge that it is a small device resting on full-size furniture.
<!-- BND_POST_INTRO_CINEMATIC_PHYSICAL_CORRECTION_V1095:END -->

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
