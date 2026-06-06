# Boredom & Dungeons

**Boredom & Dungeons** is a Unity / C# top-down 2.5D action-adventure prototype about exploration, melee and ranged combat, horse traversal, hidden collectibles, protected encounters, mini-bosses, a final boss, and multiple ending variations.

The target is a polished mobile-landscape vertical slice that feels like a designed game level rather than an endless square maze.

## Current status

```text
Status date: 2026-06-06
Engine: Unity 6000.0.76f1
Authoritative plan: PROJECT_STATUS.md
Current committed baseline: C03/C04/C05/C11 movement, horse, enemy-awareness, minimap, and spinning-AOE work
Current follow-up: C04/C11 horse hazard retreat and polished minimap cardinal transition
Verification state: clean Unity compilation, TEST EVERYTHING, and Play Mode verification still required
```

The old `Stage 11 / V126` README status is obsolete. The categorized C00–C14 plan in `PROJECT_STATUS.md` is authoritative.

## Documentation order

1. [`PROJECT_STATUS.md`](PROJECT_STATUS.md) — complete authoritative requirements, progress, blockers, QA truth, and resume point.
2. [`DOCUMENTATION_INDEX.md`](DOCUMENTATION_INDEX.md) — canonical, working, superseded, and historical document map.
3. This README — current overview and onboarding.
4. `Assets/_Project/Design/**` — focused specifications.
5. `Assets/_Project/Design/QA/**` — historical reports, not the current queue.

When documents conflict, `PROJECT_STATUS.md` wins.

## Current gameplay baseline

### Player and combat

- Natural player acceleration, deceleration, and turning.
- Dodge/dash and invulnerability frames.
- Light and heavy melee attacks.
- Physical parry and landing-attack foundations.
- Tap and charged ranged shooting.
- Ammo and automatic reload HUD.
- Long-press light-input spinning AOE attack:
  - quick left click / quick `J` remains the normal light attack;
  - hold about `0.24s` to spin when ready;
  - independent cooldown;
  - lower per-target damage;
  - outward knockback and hit feedback;
  - existing weapon-damage pickups scale it;
  - dedicated spin visual instead of the normal slash visual.

### Horse

- Mount/dismount and mounted shooting.
- Horse health, healing, faint/recovery, buck, flee, and return behavior.
- Faster movement than the player with softer acceleration and wider turns.
- Clean safe start beside the player.
- Proactive lava/hole/chasm/missing-ground detection.
- Emergency recovery without horse-health loss.

Approved next horse behavior:

```text
When a hazard is detected ahead, replace the one-second stationary refusal with
roughly two short backward horse steps, then resume the previous behavior.
```

Specification:

```text
Assets/_Project/Design/Horse/HORSE_HAZARD_TWO_STEP_RETREAT_V1.md
```

### Enemies and encounters

- Sword, shooter, patrol, jumper, trap, charger, and guardian foundations.
- Improved awareness and target refresh near the player.
- Combat-room pressure and escape blocking.
- Protected collectible guardian encounters and spawn presentation.
- Shared boss/mini-boss framework foundations.
- Temporary front/back markers until final models make facing clear.

### Camera and minimap

- Player-up minimap with four cardinal targets.
- Room discovery and player-position fallback.
- Mounted movement-direction support.
- Map clipping inside its square frame.
- Camera framing with more forward visibility.

Approved next minimap behavior:

```text
Keep the four cardinal targets, but animate each orientation change more slowly,
through the shortest angle, with a smoother professional transition.
```

Specification:

```text
Assets/_Project/Design/UI/MINIMAP_POLISHED_CARDINAL_ROTATION_V1.md
```

### Main menu and settings

- BBH boot intro.
- Main menu before gameplay.
- START GAME, SETTINGS, and desktop QUIT.
- Persistent graphics, display, frame-rate, audio, sensitivity, and camera-shake settings.
- Pause menu.
- Results return to the unchanged main menu without an immediate gameplay reload.
- The scene reloads only after the player explicitly presses START GAME.

`MAIN_MENU_SETTINGS_RESULT_FLOW_V2.md` is current. V1 is only a superseded redirect.

### Hidden collectibles and endings

Current hidden collectible set:

```text
Game Boy
Battery x2
Game Cartridge
```

Do not advertise secrets with objective markers, empty slots, missing-item text, checklists, or `0/4` progress.

## Why enemy order can feel identical between runs

The current prototype uses randomness when the **Unity scene is generated**. A normal run reloads that already-generated scene, so room and enemy order can remain the same between runs.

Enemy-type progression is also partly based on map depth and cell coordinates, so early/mid/late patterns are intentionally somewhat predictable.

This is already planned for the production architecture through:

- C02 deterministic run-seed ownership and storage;
- C10 final multi-route map generation and multi-seed validation;
- C14 random legal encounter roles/placement and run lifecycle verification.

No temporary competing runtime-randomization system is being added now.

## QA workflow

There is one required Unity QA command:

```text
Boredom And Dungeons -> TEST EVERYTHING
```

Do not add another required QA button.

A material gameplay change is not DONE until:

1. Unity compilation has no `CSxxxx` errors.
2. There is no parser failure or missing script.
3. `TEST EVERYTHING` reports zero blockers and targets zero warnings.
4. The displayed Play Mode checklist is completed.
5. Repository hygiene and diff checks pass.
6. `PROJECT_STATUS.md` records the real verified result.

## Technical structure

```text
Assets/_Project/Scripts/Runtime
Assets/_Project/Scripts/Editor
Assets/_Project/Design
Assets/_Project/Art
Assets/_Project/Audio
Assets/_Project/Scenes
Packages
ProjectSettings
```

Runtime code must not depend on `UnityEditor`. Editor-only tools stay under `Assets/_Project/Scripts/Editor`.

Current prototype scene:

```text
Assets/_Project/Scenes/02_CleanCore_MazePrototype.unity
```

Current scene-builder menu:

```text
Boredom And Dungeons -> Create Clean Maze Prototype Scene
```

Regenerating the scene may change generated maze/enemy placement. Restarting a run does not regenerate it.

## Repository hygiene

Commit real project files, required `.meta` files, and authoritative documentation.

Do not commit Unity caches, builds, ZIP files, package payloads, one-shot patch tools, chat exports, local QA output, copied status snapshots, `WORKING_NOW.md`, `PROJECT_STATUS_CURRENT*.md`, or accidental terminal output.

## Next action

1. Verify the committed natural-movement and spinning-AOE baseline.
2. Implement the approved horse two-step hazard retreat.
3. Implement the polished minimap cardinal transition.
4. Integrate both checks into the existing `TEST EVERYTHING` command.
5. Complete Unity and Play Mode verification before the gameplay changes are committed.
