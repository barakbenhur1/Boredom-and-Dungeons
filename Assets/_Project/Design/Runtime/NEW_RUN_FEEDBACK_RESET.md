# New Run Feedback Reset

## Problem

After player death, selecting `New Game` reloads the gameplay scene. Transient
feedback from the previous run must not appear as a fresh hit when the new map
becomes visible.

## Required behavior

For the first `0.45s` of a newly loaded gameplay scene:

- restore normal `Time.timeScale`;
- ensure the audio listener is not left paused;
- reset known static parry/game-feel state when an explicit reset method exists;
- clear hit/damage/impact particles;
- stop only non-looping transient hit/damage/impact/death/parry audio;
- hide strictly named damage/hit flash canvas groups;
- reset strictly named damage/hit flash animators;
- clear transient camera-shake state without changing authored shake tuning;
- repeat cleanup for several frames so `Awake`, `OnEnable`, and `Start` cannot
  replay stale feedback.

The guard exposes `IsFeedbackSuppressed` and
`RunStartFeedbackResetRequested` for explicit future integrations.

## Preservation

The reset does not:

- block legitimate damage after the short startup window;
- alter player health;
- alter enemy state;
- modify the scene;
- disable looping music;
- remove gameplay objects;
- change camera-shake configuration;
- reload the scene itself.

## Verification

1. Start a game normally.
2. Receive damage and die.
3. Return to the main menu and select `New Game`.
4. Confirm the map appears without hit-stop, camera shake, damage flash,
   impact sound, or old impact particles.
5. Confirm a real hit after the startup window still shows normal feedback.
6. Repeat death -> New Game at least twice.
7. Run `Boredom And Dungeons -> TEST EVERYTHING`.
