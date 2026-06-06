# Boredom & Dungeons

**Boredom & Dungeons** is a Unity / C# top-down 2.5D action-adventure prototype about exploration, melee and ranged combat, horse traversal, hidden optional collectibles, protected encounters, mini-bosses, a final boss, and ending variations.

The project is an active vertical-slice work in progress. The target is a polished, complete dungeon-style level with readable combat, a designed multi-route map, meaningful secrets, professional code structure, strong visual/audio identity, and a complete start-to-ending flow.

## Current status

```text
Status date: 2026-06-06
Engine: Unity 6000.0.76f1
Authoritative plan: PROJECT_STATUS.md
Current category: C06 — player melee combat expansion
Current code baseline: natural movement + awareness + facing readability + spinning AOE light attack
Verification state: committed, requires clean Unity compilation, TEST EVERYTHING, and Play Mode verification
```

The old `Stage 11 / V126` summary is no longer the active status model. The categorized plan in `PROJECT_STATUS.md` is authoritative.

## Documentation

Read documents in this order:

1. [`PROJECT_STATUS.md`](PROJECT_STATUS.md) — complete authoritative requirements, progress, blockers, QA truth, and next action.
2. [`DOCUMENTATION_INDEX.md`](DOCUMENTATION_INDEX.md) — explains which documents are current, canonical, superseded, or historical.
3. This README — project overview and onboarding.
4. `Assets/_Project/Design/**` — detailed system and design specifications.
5. `Assets/_Project/Design/QA/**` — historical stage reports, not the current queue.

When documents conflict, `PROJECT_STATUS.md` wins.

## Current implemented gameplay baseline

### Player and combat

- Player movement with acceleration/deceleration smoothing.
- Mouse-directed facing and attacks.
- Dodge / dash with invulnerability frames.
- Light melee attack.
- Heavy melee attack.
- Long-press light-input spinning AOE attack:
  - quick left click / quick `J` remains the normal light attack;
  - hold for about `0.24s` to spin when the spin cooldown is ready;
  - damages every unique living enemy in range;
  - lower per-target damage than the normal attack;
  - outward knockback, stagger, flash, and impact feedback;
  - independent cooldown that does not block normal light attacks;
  - weapon-damage pickups scale the spin through the existing multiplier;
  - dedicated spin visual without the normal forward-slash animation.
- Ranged tap/charged shooting.
- Ammo and automatic reload HUD.
- Physical parry and related feedback.

### Player, horse, and enemy movement

- Player movement is responsive but less mechanically abrupt.
- Mounted horse speed is clearly higher than player speed.
- Horse acceleration/braking are softer and turns are wider than player turns.
- Enemy steering and awareness have been strengthened to reduce passive enemies near the player.
- Temporary front/back markers distinguish player, horse, and enemies until final models make facing self-evident.
- Temporary facing markers must be removed when production models are integrated.

### Horse

- Mount/dismount flow.
- Mounted shooting without forcing horse rotation from the shot.
- Horse health, healing, faint/recovery, buck, flee, and return behavior.
- Clean start beside the player on safe ground.
- Proactive lava/hole/chasm/missing-ground detection.
- Hazard recovery remains a final fallback and must not damage the horse.

Current pending horse follow-up:

```text
Replace the one-second hazard stop with a short retreat of roughly two horse steps
away from the threat, then return to normal behavior.
```

### Enemies and encounters

- Sword, charger, ranged, trap, jumper, patrol, and guardian foundations.
- Combat-room pressure and escape blocking.
- Protected collectible guardian encounters.
- Guardian anticipation/teleport effects.
- Spawn-position safety for collectible guardians.
- Shared boss/mini-boss framework foundations.
- Playable Square Jumper framework encounter foundation.

### Minimap and camera

- Fog-of-war room discovery.
- Player-position fallback discovery.
- Player-up orientation in 90-degree sectors.
- Horse-mounted movement direction support.
- Hard clipping/grouping to keep map drawing inside the minimap frame.
- Camera forward-visibility framing with the player lower on screen.

Current pending minimap follow-up:

```text
Keep the 90-degree orientation rule, but animate each orientation change more slowly,
smoothly, and professionally instead of snapping immediately.
```

### Main menu and settings

- BBH boot intro.
- Main menu before gameplay.
- START GAME, SETTINGS, and desktop QUIT.
- Persistent graphics, display, frame-rate, audio, sensitivity, and camera-shake settings.
- Pause menu.
- Death/result flow returns to the unchanged main menu without immediate scene reload.
- Gameplay reload occurs only after the player explicitly presses START GAME.
- Mother-victory completion relic behavior is documented for the final flow.

### Hidden collectibles and endings

Secret collectibles remain genuinely hidden and must not be advertised through objective UI.

Current collectible set:

```text
Game Boy
Battery x2
Game Cartridge
```

Forbidden advertising includes objective markers, empty slots, missing-item text, checklists, and `0/4` progress.

Ending foundations include variants for missing Game Boy, missing batteries, missing cartridge, and the fully powered Game Boy state. The deeper final/Mother-boss sequence remains governed by `PROJECT_STATUS.md` and the dedicated boss documents.

## Why enemy order can feel the same between runs

The current scene builder uses randomness when it **generates the Unity scene**, including room selection, enemy positions, and some enemy choices. A normal new run reloads the already-generated scene rather than generating a fresh runtime layout, so room/enemy order can remain identical between runs.

Enemy-type progression is also partly based on map depth and cell coordinates, which intentionally makes early/mid/late difficulty patterns more predictable.

This is already scheduled for the later production map/run architecture:

- deterministic run-seed ownership and storage;
- full multi-route map generation;
- random legal mini-boss selection, role assignment, and placement;
- multi-seed validation and reproducible failure seeds;
- run lifecycle reset/persistence rules.

A temporary second randomization system should not be added now because it would conflict with that planned architecture.

## QA workflow

There is one required Unity QA command:

```text
Boredom And Dungeons -> TEST EVERYTHING
```

`TEST EVERYTHING` owns the automated project checks. Do not add a second QA command that the developer must remember to run.

The final gate for a material gameplay change is:

1. Unity finishes compilation with no `CSxxxx` errors.
2. No parser failure or missing-script error.
3. Run `TEST EVERYTHING`.
4. Automated blockers: `0`.
5. Automated warnings: target `0`.
6. Enter Play Mode once and complete the displayed manual checklist.
7. Run repository hygiene / diff checks before commit.
8. Update `PROJECT_STATUS.md` with the verified result.

A feature is not DONE merely because code or a package exists.

## Technical stack

```text
Engine: Unity 6000.0.76f1
Language: C#
Style: top-down / angled 2.5D action-adventure
Current harness: desktop Unity Editor, keyboard + mouse
Final target: mobile landscape
Architecture: runtime scripts + editor scene-generation/validation tools
```

Important folders:

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

Runtime scripts must not depend on `UnityEditor`. Editor-only tools must remain under `Assets/_Project/Scripts/Editor`.

## Opening the project

```bash
git clone https://github.com/barakbenhur1/Boredom-and-Dungeons.git
```

Then:

1. Open the repository folder in Unity `6000.0.76f1`.
2. Let Unity import and compile fully.
3. Open `Assets/_Project/Scenes/02_CleanCore_MazePrototype.unity`.
4. Run `Boredom And Dungeons -> TEST EVERYTHING`.
5. Rebuild the prototype scene only when the current task explicitly requires scene regeneration.

Current scene-builder menu:

```text
Boredom And Dungeons -> Create Clean Maze Prototype Scene
```

Regenerating the scene can change generated maze/enemy placement. Merely restarting a run does not regenerate it.

## Repository hygiene

Commit:

```text
Assets/_Project/**
Packages/**
ProjectSettings/**
README.md
PROJECT_STATUS.md
DOCUMENTATION_INDEX.md
.gitignore
required Unity .meta files
```

Do not commit:

```text
Library/
Temp/
Obj/
Logs/
UserSettings/
Build/
Builds/
*.zip
one-shot patch/package tools
package payload folders
local QA output
chat exports
copied status snapshots
WORKING_NOW.md
PROJECT_STATUS_CURRENT*.md
IDE-generated files
```

Unity `.meta` files are required and must not be ignored globally.

## Current next action

1. Verify the committed natural movement and spinning AOE baseline in Unity.
2. Implement the approved horse two-step hazard retreat.
3. Replace the minimap's immediate 90-degree snap with a slower polished transition.
4. Keep enemy-run randomization in its planned seed/map architecture rather than adding a temporary duplicate system.
5. Update the authoritative status and run the one-button QA gate.
