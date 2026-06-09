# Long-Press Light Attack — Spinning AOE V1

## Input

- A quick left mouse click, or quick J key press, remains the normal light attack.
- Holding the same input for `0.24` seconds triggers the spinning AOE attack when its own cooldown is ready.
- If the spinning attack is still cooling down, pressing the light-attack input immediately performs the normal light attack. It does not wait for the hold threshold.
- Sword attacks remain disabled while mounted, matching the existing combat rule.

## Combat

- The attack damages every unique living enemy in a radius around the player.
- Per-target damage is `82%` of normal light-attack damage.
- Damage is multiplied by the existing `WeaponDamageMultiplier`, so weapon-damage pickups improve it automatically.
- Every target receives outward knockback, hit stagger, hit flash, and impact feedback.
- Player and horse colliders are excluded.
- The spinning attack has its own `0.85` second cooldown and does not place the normal light attack on cooldown.

## Visual and feedback

- The spinning attack never calls the standard forward melee slash visual.
- `BDSpinAttackVisual` draws three rotating AOE arcs around the player for a short `0.30` second animation.
- Audio, camera shake, and hit stop are requested once per spinning attack rather than once per target.

## QA

- TEST EVERYTHING validates input fallback, independent cooldown, AOE damage, weapon boost integration, knockback, target exclusions, and the dedicated animation.
