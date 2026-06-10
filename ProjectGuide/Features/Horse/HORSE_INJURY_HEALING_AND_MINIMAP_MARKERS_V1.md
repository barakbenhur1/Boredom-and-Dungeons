# Horse Injury, Healing and Minimap Markers V1

## Status

Implemented by local patch; Unity verification required.

## Injury movement contract

Horse horizontal movement is reduced in exact, discrete bands based on missing maximum health:

| Missing health | Horizontal speed |
|---|---:|
| less than 30% | 100% |
| 30% to less than 60% | 92% |
| 60% to less than 90% | 84% |
| 90% or more | 76% |

Canonical speed-band summary: **100% / 92% / 84% / 76%**.

The multiplier is applied exactly once by `BDHorseController` to every horse horizontal movement route, including mounted travel, autonomous return, safe-spot movement and explicit external-control motion. Gravity, jump height and authored vertical recovery arcs are unchanged.

## Healing contract

- Healing remains on foot and in range.
- The rate is deliberately slower than the previous curve.
- `BDHorseController` owns healing state and health application.
- `BDHorseHealingPresentation` owns only the visible grounded ring and rising motes.
- The effect begins with the healing session, pulses when health is actually added and fades after release/completion.
- No healing icon is rendered above the horse.

## Horse UI contract

- No icon, action card or health bar floats above the horse model.
- Mount, heal, pet and dismount actions use one bottom-center contextual strip.
- Horse health appears in the main HUD only while the player is on foot and physically near the horse. It stays hidden while mounted.

## Minimap language

- Horse: green dot.
- Regular enemies and guards: small red dots, only inside discovered rooms.
- Mini-bosses: larger red dots, only inside discovered rooms.
- Bosses: large red hexagons, only inside discovered rooms.
- The player marker remains the existing player marker. Combatant markers never reveal undiscovered fog-of-war rooms.

## Future shop and NPC markers

The `future shop and NPC markers` contract remains reserved until authoritative shop and NPC entity types exist.

Shop and NPC marker categories are reserved for the future implementation stage when those entities exist. The marker registry should add stable semantic categories rather than identify objects from names. Suggested future visual language:

- shop: small gold diamond;
- friendly/general NPC: blue circle with a white center;
- quest/critical NPC: cyan diamond with a restrained pulse.

No shop/NPC runtime marker is created in this version because those authoritative entity types are not implemented yet.
