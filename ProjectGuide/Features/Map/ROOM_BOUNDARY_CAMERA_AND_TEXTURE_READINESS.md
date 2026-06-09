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
- Closed structural walls form a complete `visibility boundary`: the camera body and look point remain on the playable side, and adjacent-room geometry is visible only through authored openings.
- Only authored openings expose the next room.

## Micro-jitter and wall-pressure stability

- Room and doorway geometry remain unchanged; the 34-unit rooms and authored 10.5-unit openings are not regenerated for this repair.
- The normal 15.25-unit camera boom must fit at room center without being shortened by an oversized safety inset.
- Closed-room bounds are the primary normal-gameplay containment mechanism.
- Physical wall `SphereCast` correction runs only during an active two-room handoff, where the temporary union could otherwise cross a closed wall segment.
- The constrained look point uses its own smaller inset and smoothing so target-height movement, horse movement, and wall proximity do not create pitch or zoom pulses.
- Room discovery is resolved at most once per rendered frame and uses a cached room list between refreshes; containment checks must not trigger repeated `FindObjectsByType` scans in the same frame.
- Normal gameplay does not animate or modify camera FOV. Apparent zoom must come only from deliberate cinematics, never from wall-pressure jitter.

## Distance-preserving room handoff

- Use the union of previous and next room bounds during a legal transition.
- Do not end the handoff from an unsmoothed desired camera position.
- End the handoff only after the target is safely inside the next room and the actual final camera body plus the smoothed look point are both inside that room.
- Release happens after the final camera pose is written for the frame, so removing the previous-room bounds cannot create a next-frame clamp.
- Minimap discovery and rotation do not reposition gameplay actors or camera.
- Room/tile changes preserve distance, FOV, pitch, and yaw response without snap or zoom.
- The existing gated V23R6 diagnostics distinguish camera motion from player/horse root and visual-model motion without adding a second camera owner.

## Future asymmetric texture readiness

- No negative scale or implicit mirroring.
- Store world-facing orientation, quarter-turn metadata, positive UV scale, and offset per wall segment.
- Future natural walls may use rocks, cliffs, cave formations, roots, vegetation, ruins, fences, mountains, or other non-uniform silhouettes.

## Verification

Run `TEST EVERYTHING`. Test each wall on foot and mounted from side, corner, and diagonal views. Cross multiple doorways while enemies are present. Confirm no adjacent-room view, snap, zoom, vertical bob, pitch change, or sensitivity change.
