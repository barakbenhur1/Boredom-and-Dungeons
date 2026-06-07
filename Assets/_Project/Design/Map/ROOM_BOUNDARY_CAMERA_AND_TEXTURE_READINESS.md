# Room Boundary, Camera Stop, and Asymmetric Texture Readiness

## Immediate room-boundary contract

- Room walls remain at least 64 world units tall, with their base fixed and height extending upward.
- Player, horse, enemies, projectiles, and camera cannot pass through a closed wall.
- Authored open doorways remain legal actor transitions.

## Closed-wall visibility guarantee

A closed wall is a visibility boundary, not only a movement collider.

- Resolve the target's current `BDMinimapRoom` every frame near boundaries.
- Keep camera body and look point inside the current room or the active legal two-room handoff union.
- Apply room clamping after shake and smoothing, not only to the desired position.
- Sphere-cast from target to camera and push inward before a boundary collision.
- Structural room walls remain permanently opaque; legacy `BDOccludingWall` components are removed from structural walls.
- Near a closed wall, outward camera intent is attenuated while tangential and inward rotation remain available.
- Near-wall pitch becomes more top-down to prevent side, corner, diagonal, mounted, and screen-edge leakage.
- Only legal authored openings expose the next room through the actual opening.

## Distance-preserving room handoff

- Crossing a legal doorway never changes the camera clamp by a full room in one frame.
- During handoff, use the union of previous and next room bounds instead of interpolating boundary half-size.
- End the handoff only when the target and desired camera position naturally fit inside the next room.
- Minimap discovery and rotation never move player or horse transforms.
- Room/node changes preserve camera distance and create neither backward snap nor zoom-in.

## Future asymmetric texture readiness

- No negative scale or implicit mirroring.
- Store world-facing orientation, quarter-turn metadata, positive UV scale, and offset per wall segment.
- Future natural walls may use rocks, cliffs, cave formations, roots, vegetation, ruins, fences, mountains, or other non-uniform silhouettes.

## Verification

Run `TEST EVERYTHING` so the authoritative scene is upgraded and saved. Test every wall on foot and mounted, close to the wall, rotating rapidly through cardinal and diagonal views. No adjacent room may appear in any frame. Cross multiple legal doorways in both directions and verify there is no backward snap or zoom-in. Verify open doorways still transition normally and texture metadata remains valid.
