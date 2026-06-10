# First-Launch Tutorial — Reference-Led Pixel World and Modern Instruction Layer V3

## Status

```text
Implementation package: PREPARED
Unity compilation: REQUIRED
TEST EVERYTHING: REQUIRED
Keyboard/mouse-only completion: REQUIRED
Physical-handheld-only completion: REQUIRED
Mixed-input completion: REQUIRED
Visual acceptance: REQUIRED
```

## Approved visual direction

The provided visual references define a remembered-console fantasy style rather than literal Atari, Macintosh, DOS, CRT, or hardware emulation.

The playable world uses:

- a deep indigo/purple background;
- dark teal vegetation and edge framing;
- a warm brown/gold traversable path;
- restrained amber and cyan accents;
- clean, readable silhouettes;
- only a few environmental props placed away from the active lesson;
- point-filtered pixel actors;
- basic stepped animation only where motion communicates the lesson.

The references are inspiration for hierarchy, palette and atmosphere. They are not copied literally and do not introduce unrelated gameplay systems.

## Instruction-first hierarchy

The instruction layer is the most important element in the tutorial.

Each actionable lesson displays:

1. one very large action title;
2. one short explanatory sentence;
3. one large `KEYBOARD / MOUSE` card;
4. one large `HANDHELD` card;
5. both routes simultaneously, without hiding one when the other is used.

The last active input route may receive a subtle color emphasis, but both routes remain visible and functional.

## No-overlap contract

The screen is divided into fixed safe regions:

- title and lesson count;
- transient success feedback;
- playable pixel world;
- modern instruction card.

No normal tutorial element may overlap another. The exit confirmation is the only intentional modal overlay.

## Input truth

- Move: `WASD / ARROWS` or `D-PAD`
- Interact: `E` or `SELECT`
- Light attack: `J / LEFT CLICK` or `X`
- Heal: `HOLD F` or `HOLD A`
- Dodge: `SPACE` or `B`
- Heavy attack: `K / RIGHT CLICK` or `Y`
- Spin attack: `HOLD J / LEFT CLICK` or `HOLD X`
- Parry: `Q` or `Y`
- Grapple: `HOLD K / RIGHT CLICK` or `HOLD Y`
- Exit: `ESC / BACK` or `EXIT`

Keyboard/mouse and physical-handheld controls remain active in parallel throughout the entire tutorial.

## Animation contract

World animation is deliberately basic:

- four-frame-per-second stepped timing;
- tiny two-pixel idle bobs;
- stepped projectile movement;
- restrained collectible pulse;
- no elaborate squash, rotation, camera motion or decorative loops.

The instruction card may use a short modern fade/slide/scale transition because it belongs to the modern teaching layer.

## Acceptance

- all text is comfortably readable at the real device resolution;
- both input cards fit without clipping or wrapping into each other;
- all world actors read as pixel art;
- no decorative prop obscures an actor, hazard, projectile, gap or collectible;
- all lessons can be completed using keyboard/mouse only;
- all lessons can be completed using physical controls only;
- alternating between input routes never duplicates or skips lesson advancement.
