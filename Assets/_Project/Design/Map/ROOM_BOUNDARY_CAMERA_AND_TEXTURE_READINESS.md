# Room Boundary, Camera Stop, and Asymmetric Texture Readiness

## Immediate room-boundary contract

- Room walls remain at least 64 world units tall, with their base fixed and height extending upward.
- Player, horse, enemies, projectiles, and camera stay within legal room boundaries.
- Authored open doorways remain legal actor transitions.

## Camera ownership and stability

- `BDCameraFollow` owns the Main Camera transform during normal gameplay.
- Other presentation components do not add a second position offset after the follow calculation.
- The run-presentation coordinator temporarily owns the camera only during its approved cinematic.
- Final room containment runs after smoothing and planar shake.
- Wall proximity does not alter yaw sensitivity or gameplay pitch.
- Combat shake remains horizontal and does not create vertical bobbing.

## Closed-wall visibility guarantee

- Resolve the current `BDMinimapRoom` near boundaries.
- Keep camera body and look point within the current room or legal two-room transition union.
- Use boundary collision checks to keep the camera on the playable side of closed walls.
- Structural walls remain opaque.
- Only authored openings expose the next room.

## Distance-preserving room handoff

- Use the union of previous and next room bounds during a legal transition.
- End the handoff when target and desired camera position fit naturally inside the next room.
- Minimap discovery and rotation do not reposition gameplay actors or camera.
- Room/tile changes preserve distance, FOV, pitch, and yaw response without snap or zoom.

## Future asymmetric texture readiness

- No negative scale or implicit mirroring.
- Store world-facing orientation, quarter-turn metadata, positive UV scale, and offset per wall segment.
- Future natural walls may use rocks, cliffs, cave formations, roots, vegetation, ruins, fences, mountains, or other non-uniform silhouettes.

## Verification

Run `TEST EVERYTHING`. Test each wall on foot and mounted from side, corner, and diagonal views. Cross multiple doorways while enemies are present. Confirm no adjacent-room view, snap, zoom, vertical bob, pitch change, or sensitivity change.
