# Game Boy Menu Shell and UI Ownership V1

- Main menu, death return, pause, and settings are presented inside the original B&D Game Boy-like shell.
- Gameplay HUD, minimap, horse prompts, world horse status, target frame, boss HUD, collectible HUD, and damage overlays have one shared visibility owner.
- No gameplay element remains visible above a non-gameplay menu state.
- The true Mother victory permanently awakens the shell with cyan/gold inlays and a changed device state.
- The shell is readable and modern; it is inspired by handheld-game language without copying a commercial device.

## V23R12 integrated draw ownership

- The shell is drawn from the existing main-menu `OnGUI` pass around visible content.
- The shell has no independent competing `OnGUI` and cannot produce an empty device screen or plain-menu race.
- Menu/death entry clears transient combat presentation before drawing.


## V23R19D abandon confirmation

- Selecting `MAIN MENU / ABANDON RUN` from Pause opens a confirmation popup inside the same Game Boy menu owner.
- The first press never destroys the run.
- `YES, ABANDON RUN` performs the existing return-to-menu flow.
- `CANCEL` or Escape returns to Pause with the run still intact.

## V23R19Q memory rather than replica

- The shell is an original fantasy handheld remembered through nostalgia, not a literal Game Boy replica.
- Main, Settings, Pause, Abandon, Loading and future pages share one professional screen system.
- Existing content and behavior are preserved.
- The screen uses glass depth, restrained scanlines, a limited readable palette, consistent spacing and modern high-resolution type.
- The body uses a rounded chunky silhouette, tactile D-pad/buttons, Start/Select pills, speaker slots and a small magical status light.
- Hover/focus uses a short side accent, border and controlled lift.
- Page changes use a short 180 ms slide/fade.
- The true-victory awakened state keeps its cyan/gold transformation.
- Boot and menu textures are generated once and cached; no per-frame texture or material allocation is allowed.
- Professional finish is required on all menu pages, not only the main page.

<!-- B&D MODERN 3D HANDHELD OWNERSHIP START -->
## Approved 3D device ownership target

- The final shell is a real upright 3D device, not a flat frame.
- `BDMainMenuFlow` remains the only semantic owner of page, selection, legal action, pause/resume, settings, progression and abandon state.
- The 3D device presenter owns only device model visibility, screen/glass/body materials, camera framing, physical hit targets and tactile button animation.
- The screen-content view renders real menu data and reports semantic actions to `BDMainMenuFlow`; it does not create a second menu state machine.
- Mouse, D-pad/arrows, A/B/X/Y and clickable physical controls resolve through the same action map.
- The physical Settings and Progression center buttons invoke the same actions as X and Y.
- The user-facing page/shortcut label is `Progression`.
- Every Boy image has a matched Girl variant selected from active-character identity.
- The exact asset/render/input contract is owned by `MODERN_HANDHELD_3D_ASSET_AND_INTERACTIVE_UI_SPEC_V1.md`.
<!-- B&D MODERN 3D HANDHELD OWNERSHIP END -->
