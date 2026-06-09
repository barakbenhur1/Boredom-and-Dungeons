# Heavy-Hold Grappling Hook V1

## Input contract

- The existing strong/melee input remains Right Mouse Button on desktop with the existing keyboard fallback.
- A short press or release before `0.30s` performs the existing heavy sword attack.
- Holding for at least `0.30s` launches the grappling hook when its independent cooldown is ready.
- The hook cooldown is independent from the normal heavy-attack cooldown.
- If the hook is cooling down, the same input falls back to the normal heavy attack when that attack is ready.
- If both cooldowns are active, no attack is emitted and neither cooldown is reset.
- **Boy mounted rule:** The boy cannot use the grappling hook while mounted. His mounted combat remains ranged-only; mounted Light/Heavy sword input and mounted hook input are blocked without consuming the hook cooldown.
- **Girl future rule:** When the Girl character is implemented, the girl may use the grappling hook while mounted as part of her approved mounted-combat capability. This must be enabled through character-specific data/capability, never by globally enabling the hook for every player character.

## Hook behavior

The pull policy distinguishes small regular enemies and large enemies explicitly.

- The player turns toward the active combat aim direction before launch.
- A visible rope and original metal hook travel toward the first unobstructed hit up to the configured range.
- Structural geometry blocks the hook; it cannot acquire an enemy through a closed wall.
- Every enemy hit receives exactly **2 damage** from the hook.
- **Small regular enemies** are pulled toward the player.
- **Large enemies**, oversized regular enemies, mini-bosses, and bosses receive the 2 damage but are not pulled.
- Pulling ends inside normal sword range so the next light or heavy sword attack can connect.
- CharacterController or Rigidbody motion is reused where available, so the pull does not become a permanent second movement owner.

## Presentation

- The rope remains connected during travel, impact, pull, and retraction.
- Missing, blocked, large, or immune targets still produce readable impact/retraction feedback.
- No protected character model, animation, voice line, sound, or visual asset is copied.

## Verification

1. On foot, a short strong press performs the normal heavy sword attack. While the boy is mounted, short and long strong input perform neither sword melee nor grappling hook and consume no hook cooldown.
2. Long strong hold launches one hook and no duplicate heavy slash.
3. Small regular enemies take 2 damage and are pulled into sword range.
4. Large enemies take 2 damage and do not move.
5. Walls stop the hook.
6. The independent cooldown and heavy fallback work exactly as specified.
7. Verify the boy can use the hook on foot but cannot use it while mounted. When the Girl character is implemented, verify her mounted hook uses the same damage/pull eligibility, cooldown, wall blocking and cleanup contracts as her on-foot hook.
8. Repeated use leaves no stuck input, permanent enemy lock, duplicate damage, or orphan rope.

## V23R10 safe-release tuning

The hook reaches 13.5 units with a 0.52-unit forgiving cast radius. A pulled enemy is released before physical contact at a safe separation derived from both CharacterControllers, with the configured target distance of 2.35 units still inside the player's sword envelope. Post-pull stagger prevents immediate contact retaliation, and the target is grounded at release.

## V23R12 movement-root reliability

- Pull the real actor movement root rather than assuming the `BDHealth` transform is the root.
- Size classification ignores attack/awareness/helper colliders and uses CharacterController or body bounds.
- After safe release, suppress contact attacks briefly, snap to ground, and reset the motion-stability baseline.

## V23R19B hit-committed pull reliability

- Pull eligibility is re-evaluated at the actual impact frame against the living `BDHealth`, real movement root and authoritative forced-movement profile.
- Once the hook has physically hit a living small regular enemy that is eligible for forced movement, the **pull is committed** for that launch; helper/awareness colliders cannot downgrade it to damage-only after impact.
- Large enemies, mini-bosses, bosses and **Elite collectible guardians** still receive the configured hook damage but are never pulled.
- This policy is independent from character input permissions: the boy uses the hook only on foot; the future Girl may opt into mounted hook use through character-specific capability data.
