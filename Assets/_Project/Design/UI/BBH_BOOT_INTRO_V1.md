# BBH Boot Intro V1

## Placement

The boot intro appears once when the application starts from the operating
system, before the main menu becomes visible.

It does not replay when a new game reloads the gameplay scene during the same
application session.

The screen is fully black.

`BBH` is centered horizontally. The visible BBH glyph block should sit in the upper part of the screen, around 40% of the screen height measured from the top, which is equivalently about 60% from the bottom. The runtime therefore defaults to `0.40f` with neutral visual compensation so the letters do not drift down into the lower half.

## Letter sequence

The letters appear strictly one after another:

1. the first `B` completes its full animation;
2. the second `B` begins only afterward;
3. `H` begins only after the second `B` is complete.

There is no overlapping letter entrance.

Each letter begins as if it is far behind the screen and travels toward the
viewer:

- extremely small initial scale;
- increasing opacity and brightness;
- layered depth trail;
- subtle perspective rotation;
- controlled overshoot;
- precise settle into the final plane;
- cool edge highlight and restrained glow.

After all letters settle, one soft light sweep crosses the completed mark.
The complete image holds briefly and fades cleanly into the unchanged main
menu.

The sequence is deliberately unskippable because it is short and functions as
the application identity mark.

## Duration

Default total duration is approximately 3.9 seconds:

- short initial black hold;
- three sequential 0.72-second letter animations;
- small non-overlapping gaps;
- completed-logo hold;
- final fade.

All animation uses unscaled real time.

## Integration and safety

The intro is installed on the existing `B&D Main Menu And Settings` root.

The installer also repairs missing MonoBehaviour references on that root before
adding:

- `BDMainMenuFlow`;
- `BDSettingsAudioRouter`;
- `BDBBHBootIntro`.

Nested installers must never call `EditorSceneManager.SaveScene`. They mark the
scene dirty and allow the top-level TEST EVERYTHING flow to own scene saving.

## Visual Style

- The BBH letters should feel like solid pseudo-3D blocks: a bright front face,
  darker cube-like side depth, and subtle bevel-style highlight accents.
- Each letter has a shadow behind it that grows with the same entrance motion,
  so the shadow visibly scales and offsets as the letter comes forward.
- The goal is a polished faux-3D logo feel rather than flat text.

## Exact vertical placement

- The visual center of `BBH` is exactly `45%` of screen height measured from the top and therefore `55%` from the bottom.
- The runtime uses a non-serialized constant. Scene or Inspector values cannot override this position.
- This places the logo more in the upper half than in the lower half.


<!-- B&D BBH FILLED CIRCLE V7 START -->
## Completed filled-circle badge

After all three letters have fully settled, a circular badge begins behind the completed `BBH` text:

- it starts at zero size;
- it grows smoothly to its final diameter behind the letters;
- it contains a full opaque/tinted interior fill and a visible outer rim, not an empty outline;
- the already-settled letters remain drawn above it;
- after the circle reaches full size, the completed mark remains for exactly `0.50s`;
- the final fade is contained inside that half-second hold, and the intro ends when the hold ends;
- all timing remains based on unscaled real time.
<!-- B&D BBH FILLED CIRCLE V7 END -->
