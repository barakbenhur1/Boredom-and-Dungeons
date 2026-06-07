# Room Boundary, Camera Stop, and Asymmetric Texture Readiness

## Immediate prototype contract

- Every room-boundary wall reaches at least **22 world units** in visible and collision height.
- The original wall base remains fixed; height grows upward.
- The player, horse, enemies, projectiles, and camera may not pass through a closed room wall.
- Authored open doorway sides remain usable and transition normally when the player actually crosses them.

## Closed-wall visibility guarantee

A closed wall is a camera-visibility boundary, not only a movement collider.

- Clamp camera position and look point to the current room interior every frame.
- Rotating mouse or touch aim while standing beside a wall may not move either point around, through, or above the wall.
- Sphere and collision tests must handle diagonal angles, near-corner positions, screen-edge/frustum leakage, and mounted target height.
- Push the camera inward when necessary instead of allowing a view into an adjacent room.
- Re-resolve the current room immediately near boundaries; a stale-room interval may not expose another room.
- Only legal authored openings permit visibility and traversal between rooms.

## Future natural wall contract

Future biome walls may use rocks, cliffs, cave formations, roots, vegetation, ruins, fences, mountains, and other non-uniform silhouettes.

Every wall segment is prepared for asymmetric textures:

- no negative scale;
- texture mirroring disabled by default;
- world-facing orientation stored per segment;
- quarter-turn rotation metadata stored explicitly;
- positive UV scale and offset stored independently;
- production materials never assume left and right texture sides are interchangeable.

The profile is metadata, not a forced material clone. Future shaders or authored meshes can consume it without changing shared materials at runtime.

## QA

Verify all four closed sides, every open doorway, mounted and on-foot targets, quick camera rotations, diagonal and corner viewpoints, different aspect ratios, tall-wall bases, non-negative scale, and future texture-orientation metadata. No closed-wall test may reveal an adjacent room in any frame.
