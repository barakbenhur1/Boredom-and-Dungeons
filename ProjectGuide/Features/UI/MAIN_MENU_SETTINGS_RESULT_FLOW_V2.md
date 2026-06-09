# Main Menu, Settings, and Result Routing V2

## Corrected result rule

The main menu never displays result words, status text, defeat text, victory
text, or a changed Start-button label after a run.

The base main menu stays visually and textually identical after:

- an ordinary player death;
- reaching the ending door without the items required for the Mother fight;
- losing the Mother fight;
- winning the Mother fight.

The only exception is the permanent non-verbal completion relic described
below.

## Ordinary death

An ordinary player death returns directly to the unchanged main menu.

No automatic scene reload or loading screen happens at death.

The gameplay scene reloads only after the player presses the unchanged
**START GAME** button.

## Narrative sequences before the menu

When an ending has a cinematic, every relevant cinematic must finish before
the main menu appears.

Sequence contract:

1. At the beginning of the narrative ending, call
   `BDGameFlowSignals.BeginResultSequence()`.
2. During the sequence, player death does not open the menu and legacy death
   listeners cannot reload the scene.
3. After the ending-door-without-items sequence or Mother-loss sequence
   finishes, call
   `BDGameFlowSignals.ReturnToMainMenuAfterSequence()`.
4. After all Mother-victory cinematics and the final colored-light ending have
   finished, call
   `BDGameFlowSignals.CompleteMotherVictorySequence()`.

`BDGameCompletionMarker` exposes matching public methods for Timeline Signals,
Unity Events, or Animation Events.

## Permanent Mother-victory change

Defeating Mother stores a permanent `PlayerPrefs` progression flag.

From that point onward, including future app launches, the unchanged main menu
contains a small lit Game Boy relic:

- the Game Boy screen emits the same changing colored light associated with
  the final victory ending;
- a cartridge is visibly seated in it;
- a few quiet colored pixels drift around it;
- there is no text, label, badge wording, completion percentage, trophy word,
  or explicit statement that the game was completed.

Before Mother is defeated, this relic is not shown.

The symbol changes the world of the menu rather than announcing an
achievement in words.

## Main menu and settings

The main menu contains:

- **START GAME**
- **SETTINGS**
- desktop **QUIT**

The Start-button text never changes.

Settings remain persistent:

- graphics quality;
- fullscreen/windowed mode;
- VSync;
- target frame rate;
- master volume;
- music volume;
- SFX volume;
- mouse sensitivity;
- camera-shake intensity;
- reset defaults.

## Pause

Escape opens:

- Resume
- Settings
- Main Menu

Returning to Main Menu abandons the current run without result wording.

## V23R19E player death presentation before menu

- Lethal player damage does not open the main/death menu immediately.
- Gameplay input and combat stop, the gameplay camera remains visible, and the player death animation completes first.
- Only after the readable final death pose may `BDMainMenuFlow` open the integrated Game Boy menu and freeze the world.
- Enemy death presentation is independent from the menu: regular enemies animate, then release loot/despawn.
