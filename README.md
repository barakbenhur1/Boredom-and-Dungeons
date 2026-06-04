# Boredom & Dungeons

**Boredom & Dungeons** is a Unity / C# top-down 2.5D action-adventure prototype about exploration, combat, riding, hidden optional collectibles, and multiple ending variations.

The current project is still a prototype/vertical-slice work in progress, but the goal is to grow it into a polished playable dungeon-style level with a professional code structure, clear combat feel, readable UI, hidden secrets, bosses, environmental identity, sound, visual effects, and a complete playable ending flow.

---

## Project vision

The game should feel like a real level/map, not just a maze generator.

Core direction:

- A top-down / angled 2.5D Unity action game.
- Exploration through a complex but readable map.
- Large rooms and natural areas rather than endless square corridors.
- Horse riding as part of traversal and combat.
- Melee and ranged combat with clear aiming and feedback.
- Optional hidden collectibles that change the ending.
- Stronger enemies, mini-bosses, and a final boss.
- A cinematic ending room with different outcomes based on secrets found.

The long-term goal is a playable vertical slice that feels like a small complete game level.

---

## Current status

```text
Current development stage: 9 / 36
Current latest clean core: V124
Current next planned stage: Stage 10 — Guardian Spawn VFX
```

The current state includes code up to:

```text
Clean Core V124 — Secret Collectible Guard
```

---

## Technical stack

```text
Engine: Unity
Language: C#
Game style: top-down / angled 2.5D action-adventure
Input target: keyboard + mouse first
Current platform target: desktop / Unity Editor prototype
Architecture: Unity Runtime scripts + Unity Editor scene builder tools
```

Important project folders:

```text
Assets/_Project/Scripts/Runtime
Assets/_Project/Scripts/Editor
Assets/_Project/Design
Assets/_Project/Art
Assets/_Project/Audio
Packages
ProjectSettings
```

Unity-generated folders such as `Library`, `Temp`, `Obj`, `Logs`, and local build folders should not be committed.

---

## Main systems currently implemented

### Player and combat

- Player movement.
- Dodge / dash movement.
- Dodge i-frames.
- Visual pulse during dodge invulnerability.
- Light melee attack.
- Heavy melee attack.
- Ranged shooting.
- Ammo / reload HUD.
- Bullets should collide with walls instead of passing through them.

Current combat aiming rule:

```text
On foot:
attacks aim toward the mouse world point
and the player rotates toward the attack direction.
```

```text
Mounted:
shots aim toward the mouse world point
but the horse does not rotate because of shooting.
```

---

### Horse system

- Horse exists as part of gameplay.
- Mounted shooting is supported.
- Mounted shooting does not force the horse to rotate.
- Horse healing support exists.
- Horse behavior and visuals still need further polish.

Planned horse improvements:

- Better horse texture/model.
- Clearer healing indicator.
- Better riding sound and feedback.
- Better visual state for healthy / damaged / fainted / healing horse.

---

### Minimap

- Minimap exists.
- Minimap discovers explored rooms.
- Minimap uses player world-position fallback, not only trigger events.
- Minimap displays explored room count.
- Minimap is positioned bottom-right.

Current intended HUD layout:

```text
Ammo / reload HUD: top-right
Minimap: bottom-right
Secret collectible badges: top-left, only after pickup
```

---

### Secret collectibles

The secret collectibles are optional and should not be advertised to the player.

Current secret collectible types:

```text
Game Boy
Battery
Game Cartridge
```

Important design rule:

```text
Secrets are discovered through play, not through objective text.
```

Forbidden:

```text
Objective markers
Checklist
Empty slots
0/4 progress
Missing item text
Text telling the player to find or collect secrets
```

Allowed:

```text
Small badge only after pickup
Pickup visual effects
Environmental hints
Cinematic consequences
```

Current badge labels:

```text
GB
BAT / BAT x2
CART
```

---

### Game Boy ending logic

The ending system has four procedural ending variants based on collected secrets.

Current ending variants:

```text
NoGameBoy
GameBoyNoBatteries
PoweredNoCartridge
PoweredWithCartridge
```

Meaning:

```text
NoGameBoy:
The player reaches the ending room, sits, and does not pull out anything.
```

```text
GameBoyNoBatteries:
The player has the Game Boy, pulls it out, but there is not enough power.
```

```text
PoweredNoCartridge:
The player has Game Boy + enough batteries.
The Game Boy powers on with gray/white light, but there is no game loaded.
```

```text
PoweredWithCartridge:
The player has Game Boy + enough batteries + Game Cartridge.
The Game Boy powers on with colorful lights, showing that a game loaded.
```

The current ending cinematic is procedural and temporary. It still needs final animation, camera cuts, audio, lighting, and polish.

---

## Current architecture

Runtime gameplay scripts live under:

```text
Assets/_Project/Scripts/Runtime
```

Editor-only tools live under:

```text
Assets/_Project/Scripts/Editor
```

Important rule:

```text
Runtime scripts must not depend on UnityEditor.
Editor tools must stay under Scripts/Editor.
```

The scene builder is currently being prepared for gradual decomposition. The project should keep moving toward a professional structure with separated systems for:

- Player
- Combat
- Horse
- Enemies
- Encounters
- Collectibles
- Cinematics
- UI
- Audio
- Bosses
- Mini-bosses
- Environment / map generation
- Editor-only scene building tools

---

## Current roadmap

The full roadmap is 36 stages. The project is currently around Stage 9.

Completed or partially completed:

```text
1. QA baseline workflow
2. Project structure pass
3. Scene builder decomposition preparation
4. Inventory state expansion
5. Game Cartridge collectible type
6. Secret collectible badge HUD
7. Ending state controller
8. Four procedural ending variants
9. Secret collectible advertising guard
```

Next major stages:

```text
10. Guardian Spawn VFX
11. Harder battery guardian encounters
12. Mini-boss 1 design — guards the Game Boy
13. Mini-boss 2 design — pre-boss encounter
14. Mini-boss 3 design — random late mini-boss that drops Game Cartridge
15. Final boss design
16. Boss / mini-boss framework
17. Full map redesign
18. Natural rounded / curved map pass
19. Large room and level-feel pass
20. Gameplay placement pass
21. Biome / ground texture pass
22. Wall / environment material pass
23. Lighting / atmosphere pass
24. Environmental storytelling props
25. Player art / texture pass
26. Horse art / texture pass
27. Enemy / guardian / boss art pass
28. Weapons / projectiles / bombs VFX pass
29. Ammo / reload HUD polish
30. General UI / HUD polish
31. Audio foundation
32. Cinematic polish
33. Combat / difficulty balance
34. Readability + performance pass
35. Code cleanup + production asset pipeline + save/progression planning
36. Full vertical slice QA + playable build
```

---

## Planned gameplay direction

### Map / level

The map should eventually stop feeling like a simple maze.

Planned map direction:

- Large rooms.
- Natural curved turns.
- Less grid-like structure.
- More readable level flow.
- Side areas and hidden optional paths.
- Main route toward final boss and exit.
- Optional hard routes for secrets.

### Boss placement rules

Planned boss structure:

```text
Mini-boss 1:
Guards the Game Boy.
```

```text
Mini-boss 2:
Appears before the final boss, but not directly next to it.
After beating it, there should still be a bit more map to play.
```

```text
Mini-boss 3:
Appears later in the map in a dynamic/random legal location.
Drops the Game Cartridge.
```

```text
Final boss:
Located in the last room.
The exit is blocked until the boss is defeated.
```

---

## How to open the project

1. Clone the repository:

```bash
git clone https://github.com/barakbenhur1/Boredom-and-Dungeons.git
```

2. Open the folder in Unity.

3. Let Unity import and compile.

4. Use the editor scene builder menu to create the current prototype scene.

The exact editor menu may change as the scene builder evolves. Current work has been using the clean maze prototype scene builder flow.

---

## Repository hygiene

This repository should contain the real extracted Unity project files, not old ZIP/version packages.

Should be committed:

```text
Assets/_Project/**
Packages/**
ProjectSettings/**
README.md
.gitignore
Unity .meta files for committed assets
```

Should not be committed:

```text
Library/
Temp/
Obj/
Build/
Builds/
Logs/
UserSettings/
*.zip
README_CLEAN_CORE_V*.md
old generated version folders
local exported packages
IDE generated files
```

Important:

```text
Do not ignore Unity .meta files globally.
Unity .meta files are required for references to stay stable.
```

---

## Current cleanup note

If old version files such as these appear in the repository root:

```text
README_CLEAN_CORE_V1.md
README_CLEAN_CORE_V102.md
README_CLEAN_CORE_V124_SECRET_COLLECTIBLE_GUARD.md
boredom_and_dungeons_clean_core_v*.zip
old clean_core folders
```

They should be removed from Git tracking and kept out of the repository. The root README should be this file only.

---

## Current priority

The next development priority is:

```text
Stage 10 — Guardian Spawn VFX
```

Goal:

- Enemies should not simply pop into existence near collectibles.
- They should spawn with smoke / teleport / anticipation delay.
- Enemies should become active only after the spawn effect.
- This should make battery guardian encounters feel intentional and polished.
