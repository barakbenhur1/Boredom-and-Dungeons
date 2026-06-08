# V23R12 Runtime Regression Repair

## Purpose

This contract closes the visual/runtime regressions reported after the automated V23R11 baseline passed. Static QA success does not replace Play Mode truth: the hook, mounted targeting, Parry presentation, horse prompts, enemy grounding, and menu shell must all be visibly correct during a real run.

## Grappling hook — reliable pull and safe release

- The hook applies its existing 2 damage exactly once.
- Pull eligibility is evaluated from the enemy's real movement/body envelope, never from attack, awareness, telegraph, or helper colliders.
- A `BDHealth` child may not be treated as the movement owner. The hook resolves and moves the real CharacterController, Rigidbody, combatant-profile, or actor root.
- Small regular enemies pull; oversized regular enemies, mini-bosses, and bosses remain damage-only.
- Pull completion stops before physical overlap while remaining inside the sword envelope.
- The pulled enemy receives a brief contact-attack suppression window, stagger, ground snap, and motion-stabilizer baseline reset.
- Pulling cannot create a free collision hit against the player.

## Target highlight — mounted and ranged truth

- The red corner frame remains presentation-only and selects at most one target.
- Mounted gameplay uses a horse-height targeting origin and a ranged-only envelope because melee and hook are unavailable while mounted.
- A distant target may be framed only when the loaded projectile line can actually reach it without a wall or nearer enemy intercepting it.
- The highlighter remains active while the combat component is temporarily disabled by mounted restrictions, provided gameplay targeting visibility is allowed.

## Parry — clear frozen attack presentation

- A successful Parry clears all active player slash meshes before anticipation/freeze begins.
- No light, heavy, spin, or airborne slash may remain suspended throughout slow time or wait for the next attack to disappear.
- Existing Parry anticipation, frozen moment, attached ring, and gradual recovery remain unchanged.
- Menu/death reset also clears temporary slash visuals.

## Horse contextual prompt placement

- `BDHorseContextActionPrompts` remains the single action-row presenter.
- The action row is raised above the horse status/name/health text with a clear screen-space gap.
- `BDHorseWorldStatusIndicator` may draw status and health, but suppresses its old action hint whenever the unified action presenter exists.
- Existing action-state matrix remains authoritative: on foot shows valid Mount/Pet/conditional Heal; mounted stationary shows Dismount only; mounted moving shows no row.

## Enemy grounding and spawn safety

- Ground raycast results represent the surface point. Actor root placement must compensate for CharacterController center, height, radius, and scale.
- Spawn validation runs before the first visible frame and once after initialization settles.
- Horizontal relocation is spawn/recovery-only; a LateUpdate safety pass may not teleport an active enemy to another side of the player.
- Ordinary late stabilization corrects impossible displacement and vertical error without fighting authored jumps, charges, knockback, hook pull, or stagger.
- When a valid ground point is accepted, ground stick and the motion stabilizer share the same root-aware position and baseline.
- Enemy bootstrap installs placement safety on CharacterController enemies.

## Menu/death Game Boy ownership

- The Game Boy shell is drawn inside the same `BDMainMenuFlow.OnGUI` pass as menu content.
- It cannot own a second independent `OnGUI`, cover the menu with an empty black screen, or race the main menu.
- The shell draws frame/device details around the content region while the existing menu flow owns buttons, death text, settings, pause, and loading state.
- Entering menu/death clears transient Parry/slash presentation before showing the device.
- Main, death, pause, and settings use the same shell contract; true Mother victory may switch the established awakened treatment.

## Acceptance

1. Hook pulls ordinary small enemies consistently and stops them before contact.
2. Hook never grants an immediate contact hit to the pulled enemy.
3. Mounted ranged targeting frames the one enemy the current shot would hit.
4. Parry leaves no slash mesh frozen on screen.
5. Horse prompts remain readable above status text.
6. Enemies spawn above ground, remain grounded, and do not teleport across the player.
7. Menu/death/pause/settings show one integrated Game Boy shell with visible content.
8. TEST EVERYTHING passes, followed by focused Play Mode verification of all seven items.
