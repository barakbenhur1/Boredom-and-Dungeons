# Natural Movement, Reliable Awareness, Horse Hazard Refusal, and Temporary Facing Markers V1

## Player movement

- Keep the player responsive, but remove abrupt starts, stops, and body snaps.
- Runtime values are applied in code so old serialized scene values cannot keep the previous movement feel.

## Horse movement

- The mounted horse is clearly faster than the player.
- Acceleration and braking are softer than the player.
- Travel direction turns gradually, creating slightly wider arcs instead of player-like instant direction changes.
- While moving, the horse faces its travel direction; while standing, it may still turn toward mounted aim.

## Horse hazard refusal

- The horse looks ahead for lava, holes, chasms, and missing ground.
- When a dangerous approach is detected, it swerves toward a safe direction before contact.
- After the swerve, it refuses movement for approximately one second.
- Continued unsafe input causes another refusal instead of entry into the hazard.
- Recovery without horse damage remains the final fallback for exceptional cases.

## Enemy movement and awareness

- Enemy movement direction changes are smoothed to avoid mechanical snapping.
- Committed high-speed attacks such as charges remain fast and readable.
- Enemies refresh their player target frequently and do not remain passive beside the player after mounting, dismounting, or a scene reset.
- Awareness and attack/start ranges are increased moderately, not globally exaggerated.

## Temporary front/back readability

- Until final models are installed, player, horse, and enemy actors receive temporary front and rear markers.
- Player front: cyan; horse front: warm gold; enemy front: red.
- Rear markers are smaller and darker.
- Runtime child names contain `TEMP` and `UNTIL_REAL_MODELS`.
- Remove `BDTemporaryFacingIndicator.cs` and its QA contract when production models make direction visually obvious.

## QA

The feature is not complete until Unity compiles with no C# errors, TEST EVERYTHING passes, and Play Mode confirms the player, horse, and enemies remain controllable and readable.
