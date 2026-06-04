# Stage 04B — Minimap + I-Frames Hotfix Report

## Done

- Minimap moved to top-right.
- Minimap now resolves player directly with `BDPlayerMarker`.
- Minimap discovery now runs in Update and OnGUI.
- Minimap discovers nearest room if exact containment fails.
- Minimap shows explored room count.
- Dodge i-frame extra time increased from 0.04 to 0.14.
- Added `DodgeInvulnerableRemaining`.
- Added `DodgeInvulnerableProgress01`.
- Added `BDPlayerDodgeIFrameFeedback`.

## Required QA

Minimap:
- Run scene builder.
- Enter Play Mode.
- Walk across multiple rooms.
- Verify explored count increases.
- Verify visible rooms appear on the map.
- Verify minimap is not covered by ammo HUD.

I-Frames:
- Dodge through/near enemy hit.
- Verify player does not take damage during dodge.
- Verify player briefly pulses during i-frames.
- Verify player can take damage again after dodge.

## Progress

```text
Current planned stage: 4 / 36
Hotfix: 4B
Completed if QA passes: 4 / 36
Remaining: 32 / 36
Progress: 11.1%
```

## Next planned stage

Stage 5 / 36 — Game Cartridge collectible.
