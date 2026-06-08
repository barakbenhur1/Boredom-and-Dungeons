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
