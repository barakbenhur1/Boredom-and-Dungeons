# Mounted Enemy Impact V1

- Only a mounted horse moving at meaningful speed can damage a collision target.
- Only small regular enemies are eligible; large enemies, mini-bosses, and bosses are not rammed or damaged by this rule.
- A glancing rub and a full frontal collision are both valid impacts.
- Damage ranges continuously from 4 to 10 according to travel speed and contact directness.
- Knockback direction follows the physical strike: a blend of horse travel and the outward contact direction.
- Knockback strength scales with the same impact measure.
- Per-enemy contact cooldown prevents continuous-overlap multi-hits.
- A rammed small enemy may be pushed into a hole, lava, or quicksand and receives the corresponding hazard result.
