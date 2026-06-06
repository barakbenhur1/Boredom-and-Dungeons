# Main Menu, Settings, and Result Routing V1

## Main menu

The game opens on a main screen before gameplay starts.

Required controls:

- **Start Game**
- **Settings**
- **Quit** on supported desktop builds

After a completed run, the Start button becomes **Start New Game**.

## Settings

The settings overlay is reachable from the main menu and the pause screen.

Implemented settings:

- graphics quality;
- fullscreen/windowed mode on desktop;
- VSync;
- target frame rate: 30, 60, 120, or unlimited;
- master volume;
- music volume;
- sound-effects volume;
- mouse sensitivity;
- camera-shake intensity;
- reset to defaults.

Settings are stored in `PlayerPrefs` and applied before scene gameplay starts.

## Result routing

Player death is routed to the main menu before legacy `Died` listeners can
automatically reload the scene.

A completed final encounter can report victory through:

- `BDGameFlowSignals.ShowVictory()`;
- `BDGameCompletionMarker` on a completed encounter;
- a final encounter name containing `mother`, `final`, `ending`, or
  `game_complete`.

Defeat or victory pauses the current run and shows the main menu immediately.
The scene is not reloaded at that moment.

When the player chooses **Start New Game**, the current gameplay scene reloads
behind the menu and starts automatically. Loading therefore happens only after
the player's explicit Start command, not immediately after death or victory.

## Pause

Escape opens the pause menu during gameplay:

- Resume
- Settings
- Main Menu

Returning to the main menu abandons the current run. Starting again performs a
clean scene reload.

## Audio routing

`AudioListener.volume` controls master volume.

Generated `BDGameFeelAudio` effects use the SFX setting directly. Other
`AudioSource` objects are classified as music when they loop or use music/BGM/
theme/soundtrack naming; all other sources use the SFX level.

## Future final-boss integration

The final Mother-boss victory cutscene should call
`BDGameFlowSignals.ShowVictory()` only after its colored-light ending finishes.
Its defeat cutscene should call `BDGameFlowSignals.ShowDefeat()` only after the
player says `I'm bored`.

## Installer migration scan

No safely identifiable legacy death/victory scene-load method was found. Player death is still intercepted centrally through `BDHealth`.
