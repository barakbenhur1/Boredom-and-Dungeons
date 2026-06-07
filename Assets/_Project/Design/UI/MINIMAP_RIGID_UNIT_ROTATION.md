# Minimap Rigid-Unit Cardinal Rotation

## Problem

The previous minimap rotated room centers, wall endpoints, and markers
separately. Room rectangles remained axis-aligned while their centers moved,
which could make the map appear to deform, flicker, stretch, or separate.

## Correct architecture

The frame, title, clipping region, and footer remain fixed.

All map-space content is drawn in ordinary unrotated coordinates:

- discovered rooms;
- current-room outline;
- walls and openings;
- player marker;
- horse marker;
- future route, enemy, hazard, and decoration markers.

A single GUI matrix rotation then rotates the complete content around one
authoritative pivot. No room, wall, line, icon, or marker receives an
independent animated transform.

## Rotation behavior

- Preserve the four cardinal targets.
- Preserve movement threshold and diagonal-boundary hold.
- Preserve shortest-angle `Mathf.SmoothDampAngle`.
- Retarget from the currently displayed angle during an active turn.
- Snap exactly to the final angle inside the completion epsilon.
- Restore the original `GUI.matrix` in a `finally` block.
- Keep the fixed group as the clipping boundary.

## Acceptance

1. Turn repeatedly through all four cardinal directions.
2. Reverse by `180` degrees.
3. Retarget before a previous turn finishes.
4. Repeat on foot and while mounted.
5. Discover a room during or immediately after rotation.
6. Test desktop and mobile landscape dimensions.
7. Confirm zero node jitter, flicker, stretching, drifting, pulsing, corridor
   separation, clipping escape, or per-element wobble.
8. Run `Boredom And Dungeons -> TEST EVERYTHING`.

`PROJECT_STATUS.md` remains the only complete authoritative plan and progress
record. This file is the focused contract for `C11.13B`.
