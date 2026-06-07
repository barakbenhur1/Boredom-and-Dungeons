# Room Boundary, Camera Stop, and Asymmetric Texture Readiness

## Immediate prototype contract

- Every room-boundary wall must reach at least **22 world units** in visible and collision height.
- The original wall base remains fixed; height grows upward.
- The player, horse, enemies, projectiles, and camera may not pass through a closed room wall.
- When the player approaches a closed wall, the camera position and look point stop at that room boundary and may not reveal the adjacent room.
- Authored open doorway sides remain usable and transition normally when the player actually crosses them.

## Future natural wall contract

Future biome walls may use rocks, cliffs, cave formations, roots, vegetation, ruins, fences, mountains, and other non-uniform silhouettes.

Every wall segment is prepared for asymmetric textures:

- no negative scale is allowed;
- texture mirroring is disabled by default;
- world-facing orientation is stored per segment;
- quarter-turn rotation metadata is stored explicitly;
- positive UV scale and offset are stored independently;
- replacing prototype materials must not assume that the left and right sides of a texture are interchangeable.

The profile is metadata, not a forced material clone. Future production shaders or authored meshes can consume it without changing shared materials during runtime.

## QA

Verify all four closed sides, every open doorway, mounted camera targeting, quick camera rotations, different aspect ratios, tall-wall bases, non-negative scale, and future texture-orientation metadata.
