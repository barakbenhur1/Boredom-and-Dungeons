# Parry Success Presentation V1

A successful Parry uses three readable phases: a short anticipation flash and sound before time locks, a frozen-moment phase, and a gradual recovery phase. The ground ring is parented to the player so it follows movement. The release uses a second burst, audio punctuation, fading frame treatment, and gradual animator recovery instead of an immediate visual cut. The player remains responsive while the non-player world is held.

## V23R12 frozen-slash cleanup

- Clear every active player slash presentation before Parry anticipation/freeze begins.
- Menu/death reset also clears temporary slash meshes.
- Slow time must never leave an attack visual suspended until the next attack.
