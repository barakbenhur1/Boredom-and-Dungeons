# Boredom & Dungeons

**Boredom & Dungeons** is a Unity / C# top-down 2.5D action-adventure prototype about exploration, combat, riding, hidden optional collectibles, protected encounters, mini-bosses, a final boss, and multiple ending variations.

The project is still a prototype / vertical-slice work in progress. The goal is to grow it into a polished playable dungeon-style level with professional code structure, clear combat feel, readable UI, hidden secrets, meaningful encounters, strong environmental identity, sound, visual effects, and a complete playable ending flow.

---

## Current status

```text
Current development stage: 11 / 36
Current latest clean core: V126
Current latest feature: Battery encounters hardening
Current next planned stage: Stage 12 — Mini-boss 1 design: Game Boy guardian
```

---

## Technical stack

```text
Engine: Unity
Language: C#
Game style: top-down / angled 2.5D action-adventure
Input target: keyboard + mouse first
Current platform target: desktop / Unity Editor prototype
Architecture: Unity Runtime scripts + Unity Editor scene-builder tools
```

Important folders:

```text
Assets/_Project/Scripts/Runtime
Assets/_Project/Scripts/Editor
Assets/_Project/Design
Assets/_Project/Art
Assets/_Project/Audio
Packages
ProjectSettings
```

Runtime scripts must not depend on `UnityEditor`. Editor-only tools must stay under `Assets/_Project/Scripts/Editor`.

---

## Project vision

The game should feel like a real level/map, not just a maze generator.

Core direction:

- A readable top-down / angled 2.5D action game.
- Exploration through a complex but understandable map.
- Large rooms and natural areas rather than endless square corridors.
- Natural rounded turns and less grid-like movement flow.
- Horse riding as part of traversal and combat.
- Melee and ranged combat with clear aiming and feedback.
- Optional hidden collectibles that change the ending.
- Stronger enemies, mini-bosses, and a final boss.
- A cinematic ending room with different outcomes based on secrets found.

The long-term goal is a playable vertical slice that feels like a small complete game level.

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

Planned horse improvements:

- Better horse texture/model.
- Clearer healing indicator.
- Better riding sound and feedback.
- Better visual state for healthy / damaged / fainted / healing horse.

---

### Minimap and HUD

- Minimap exists.
- Minimap discovers explored rooms.
- Minimap uses player world-position fallback, not only trigger events.
- Minimap displays explored room count.

Current intended HUD layout:

```text
Ammo / reload HUD: top-right
Minimap: bottom-right
Secret collectible badges: top-left, only after pickup
```

---

### Secret collectibles

Secret collectibles are optional and should not be advertised to the player.

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

### Guardian encounters

Protected collectibles can spawn guardian enemies.

Current V126 behavior:

```text
1. Player approaches a protected collectible.
2. A teleport/smoke/ring VFX appears on the floor.
3. Guardians are created hidden and inactive.
4. A short anticipation delay runs.
5. Guardians move into fair resolved positions and become active.
6. A final flash appears.
```

Battery guardian encounters are now harder and fairer:

```text
- Default encounter pressure is higher.
- Guardians avoid spawning too close to the player.
- Guardians avoid spawning too close to the collectible.
- Guardians avoid overlapping each other.
- If the first spawn point is bad, alternate angles/distances are tried.
- If all candidates are imperfect, the best scored fallback is used.
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

## Current roadmap

The full roadmap is 36 stages. The project is currently around Stage 11.

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
10. Guardian Spawn VFX
11. Battery encounters hardening
```

Next major stages:

```text
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

## Boss placement rules

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
README_V*.md
README_CLEAN_CORE*.md
package_manifest.json
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

## Current priority

```text
Stage 12 — Mini-boss 1 design: Game Boy guardian
```

Goal:

- Define the first mini-boss that protects the Game Boy.
- Keep its location decided later by explicit placement, not by assumption.
- Create a readable design before implementation.
- Preserve the secret collectible rule: no objective marker, no checklist, no advertising.
