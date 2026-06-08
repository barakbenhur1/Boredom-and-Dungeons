# Wall Jump V1

- While airborne, a valid near-vertical wall contact is remembered for a short grace window.
- Pressing Jump again during that window turns the player away from the wall and launches a medium-distance arc.
- The launch contains both upward velocity and a temporary horizontal push away from the wall; it is never a straight horizontal dash.
- Air control is reduced during the launch so the arc remains readable, then returns naturally.
- A short cooldown prevents repeated activation from one contact frame.
- Wall jump increments the same committed jump sequence used by gameplay systems, while ordinary grounded jump remains unchanged.

## V23R19 solid-surface and reach refinement

- Any enabled non-trigger solid collider with a mostly vertical contact face can be a wall-jump surface.
- This includes structural walls, hard props, enemies, the horse, and other solid actors when a physical push-off is sensible.
- Floors, ceilings, ramps, trigger volumes, and the player's own colliders are rejected.
- Recent CharacterController contact remains primary; a short bounded sphere-cast probe catches nearby solid vertical surfaces when contact callbacks are insufficient.
- Horizontal launch reach is increased moderately while reduced air control and cooldown remain intact.


## V23R19D controlled-jump combat-contact safety

- A deliberate jump remains a valid airborne state while the player brushes, lands on, or is hit by an attacking enemy.
- Combat-grounding recovery must not reinterpret that temporary loss of floor support as an accidental fall.
- Taking damage during the controlled-jump window may apply normal damage feedback but cannot teleport the player to an older safe point.
- Real unsupported floor loss outside jump, dodge, quicksand, or explicit forced movement remains recoverable by the existing guard.
