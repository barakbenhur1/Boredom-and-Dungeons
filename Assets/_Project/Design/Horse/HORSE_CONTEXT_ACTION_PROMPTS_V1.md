# Horse Context Action Prompts V1

## Goal

Replace scattered floating heal/pet cues with one compact contextual action row that always shows the real key and a clear action label.

## On foot near the horse

Show only actions that are currently valid:

- `[E] MOUNT` when mounting is available.
- `[F] HOLD HEAL` when the horse needs healing or is fainted and healing is allowed.
- `[active pet key] PET`, `HOLD PET`, or `PETTING` according to the current pet interaction state.

## Mounted and stationary

- Show `[E] DISMOUNT` using the real active interaction binding.
- Pet remains available through its bound key, but intentionally has no icon/card.
- Healing is disabled and has no icon/card.
- Pet may run only while the horse remains fully stationary.

## Mounted and moving

- Dismount remains available through its bound key, but intentionally has no icon/card.
- Pet is unavailable.
- Healing is unavailable.
- No horse context row is drawn.

## State matrix

| Player/horse state | Mount | Dismount | Heal | Pet |
|---|---:|---:|---:|---:|
| On foot, near horse | Available + prompt when legal | No | Available + prompt only when needed | Available + prompt when legal |
| Mounted, stationary | No | Available + prompt | Disabled | Available, no prompt |
| Mounted, moving | No | Available, no prompt | Disabled | Disabled |

## Presentation

- Anchor the row near the horse in screen space.
- Use compact dark translucent cards with a distinct keycap, small action symbol, and readable label.
- Keep mount, dismount, heal, and pet accents distinct but restrained.
- Clamp the row to the screen and avoid overlap with the horse health bar where possible.
- Legacy standalone heal and pet indicators suppress themselves while the unified prompt component exists.

## Verification

1. Near an unmounted horse, every available action shows its real key and clear label.
2. Moving out of range hides the row.
3. Mounted and stationary shows only Dismount; Pet remains key-usable without a prompt and Heal is disabled.
4. Mounted and moving shows no row; Dismount remains key-usable while Pet and Heal are disabled.
5. Healing is available only on foot.
6. Gameplay actions remain owned by existing components.
7. No duplicated legacy icon remains.

## V23R12 readability placement

- Raise the unified action row above horse name/status/health text.
- The legacy status presenter suppresses its old action sentence whenever the unified row exists.
- Status text and action prompts must never occupy the same screen-space band.
