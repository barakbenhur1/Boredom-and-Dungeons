# Minimap Polished Cardinal Rotation V1

**Status: APPROVED — PENDING IMPLEMENTATION**

Status date: **2026-06-06**

## Requirement

The minimap remains player-up and uses the existing four cardinal orientations:

```text
0 degrees
90 degrees
180 degrees
270 degrees
```

The map must no longer jump immediately between orientations.

Each 90-degree change must animate slightly more slowly and feel controlled and professional.

## Behavior

- Keep movement-based cardinal selection.
- Keep diagonal-boundary stability so the minimap does not flicker between sectors.
- Smooth the displayed angle toward the selected cardinal target.
- Use shortest-angle interpolation, including transitions around 0/360 degrees.
- Do not allow free continuous rotation; the target remains one of the four cardinal angles.
- The transition must be responsive enough for navigation but visibly softer than the current snap.
- The map, rooms, connectors, player marker, and horse marker rotate together.
- All map drawing remains clipped inside the square frame during the entire animation.

## QA

Integrate source and behavior contracts into the existing `Boredom And Dungeons -> TEST EVERYTHING` command.

Play Mode acceptance:

- move forward, backward, left, and right;
- cross every cardinal sector boundary;
- test repeated direction changes;
- test mounted movement;
- confirm the animation follows the shortest path;
- confirm nothing is drawn outside the minimap frame;
- confirm no additional required QA command is added.
