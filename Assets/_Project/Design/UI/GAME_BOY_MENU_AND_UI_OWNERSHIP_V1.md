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
