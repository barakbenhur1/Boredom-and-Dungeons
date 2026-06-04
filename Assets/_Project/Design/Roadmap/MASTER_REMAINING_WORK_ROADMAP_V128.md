# Boredom & Dungeons — Master Remaining Work Roadmap V128

## Status

```text
Total roadmap stages: 36
Completed / designed through: 15
Next implementation stage: 16
Remaining stages: 21
Planning progress: 41.7%
```

Important distinction:

```text
Stages 12–15 are design-complete.
The mini-bosses and final boss are not implemented yet.
```

## Completed / design-complete stages

```text
1. QA baseline workflow
2. Project structure pass
3. Scene-builder decomposition preparation
4. Inventory state expansion
5. Game Cartridge collectible type
6. Secret collectible badge HUD
7. Ending-state controller
8. Four procedural ending variants
9. Secret collectible advertising guard
10. Guardian Spawn VFX
11. Battery encounters hardening
12. Four-mini-boss roster and random-role rules
13. Square Jumper and Roller mini-boss designs
14. Serpent and Quad Gunners mini-boss designs
15. Final black/white split-boss design
```

## Approved mini-boss system

Pool:

```text
1. Square Jumper
2. Roller
3. Serpent
4. Quad Gunners
```

Per generated map:

```text
Select 3 of 4 randomly.
Leave 1 out.
Assign the selected 3 randomly to:
- Game Boy guardian
- Game Cartridge guardian
- Pre-boss encounter
```

All secret collectible rewards are revealed from a chest after the assigned mini-boss is defeated.

## Approved final-boss system

```text
Black/white colossus
At least 2.5x player height
Final room only
Magical exit barrier remains until full victory
```

Stages:

```text
Stage 1: joined form, one health bar, no summons
Stage 2: split form, one health bar per half, no summons
         a half reaching zero is knocked out for the rest of Stage 2
Stage 3: both halves return, one final health bar per half
         summoning enabled only here
         final collapse waits until both final bars reach zero
```

Final result:

```text
Black and white halves collapse separately.
The magical exit barrier disappears with an animation.
The exit becomes traversable only after that animation completes.
```

# Remaining implementation stages

## Stage 16 — Boss and mini-boss production framework

Build shared production-ready systems before implementing individual encounters.

Required systems:

```text
Boss encounter state machine
Phase controller
Boss health UI
Multiple health bars / segmented health support
Attack telegraph interface
Boss arena activation / deactivation
Spawn / intro VFX lockout
Death / knockout / critical-state handling
Reward chest controller
Final-room magical barrier controller
Shared summon-cap controller
Deterministic random seed support
```

Combat prerequisites included in this stage:

```text
Dodge i-frames must ignore enemy projectile damage while active.
Enemy projectiles must still collide with walls.
Laser i-frame behavior must be explicitly defined and tested.
Sword enemy weapon animation must be visually verified in Play Mode.
```

Completion report must include:

```text
Implemented systems
Files changed
Play Mode QA results
Completed stages / total stages
Remaining stages
Known blockers
Recommendations
```

## Stage 17 — Mini-boss random selection, role assignment, and placement

Implement seeded selection and assignment:

```text
Choose 3 of 4 archetypes
Assign Game Boy / Cartridge / Pre-boss roles randomly
Keep roles unique
Place encounters only in legal rooms
Support sequential and parallel layouts
Allow similar Y progression with large X separation
Keep encounters away from start and final-boss room
Keep some playable map after the pre-boss
```

Add bounded reroll logic when placement validation fails.

## Stage 18 — Square Jumper implementation

Implement:

```text
Large square body
Slow tracking
Fast heavy jump landing
Readable landing telegraph and impact
Bullet-hell patterns
Two simultaneous side swords
Summons: sword, shooter, patrol enemies
Late-phase increased jump and bullet-hell pressure
Post-victory reward chest
```

## Stage 19 — Roller implementation

Implement:

```text
Large round body
Fast roll charge
Continuous rotating spiral bullet hell
Summons: jumper, bomb-placer, sword enemies
Readable roll startup and recovery
Late-phase faster rolls and denser spiral
Post-victory reward chest
```

## Stage 20 — Serpent implementation

Implement:

```text
Segmented snake movement
Head-only damage weak point
Body contact damage with cooldown
Lunge bite
Tail grab with automatic release
Tail whip
Three-shot fan, repeatable up to 3 times
Enraged faster final phase
Post-victory reward chest
```

## Stage 21 — Quad Gunners implementation

Implement four fixed identities:

```text
RapidBlue       -> blue   -> fast low-damage bullets -> jumpers
HeavyRed        -> red    -> slow high-damage bullets -> bomb placers
SlowPurple      -> purple -> normal damage + 1s slow  -> shooters
KnockbackYellow -> yellow -> normal damage + knockback -> sword enemies
```

Also implement:

```text
Hard-coded identity mapping
Per-member 10-second summon timer
2 enemies per summon
Shared active-summon cap
Coordinated sword flanking without clustering
Speed escalation:
4 alive = 1.00x
3 alive = 1.08x
2 alive = 1.16x
1 alive = 1.25x
Four-color segmented health UI
Reward chest after all four die
```

## Stage 22 — Final black/white boss implementation

Implement joined and split systems:

```text
Joined clap crush
Dual-eye 3-pass sweeping lasers
Fast direct laser
Stomp
Two-arm moving spin
Hard but readable bullet hell
60% split transition
Independent Stage-2 health bars and knockout states
Stage-3 health reset / final bars
Both laser types per half
Leg jump/stomp per half
Hand strike per half
Half-range one-arm spin
Split flanking
Stage-3-only summoning every 2 seconds per half
Shared summon cap
Linked final-death condition
Separate collapses
Animated magical barrier disappearance
```

## Stage 23 — Full map redesign

Replace the prototype maze feeling with a final level map.

Requirements:

```text
Complex overall route
Large rooms that do not feel like maze cells
Readable primary route
Optional side paths
Secret encounter spaces
Final room for boss and exit
Room scale appropriate for horse, bullet hell, and boss movement
```

The final map should feel like a designed game level, not a procedural grid maze.

## Stage 24 — Natural geometry and curved-turn pass

Add:

```text
Rounded turns
Curved corridors
Occasional curled / winding paths
Less constant 90-degree geometry
Natural transitions between spaces
```

Do not make every turn curved. Use a controlled mixture for readability.

## Stage 25 — Gameplay placement and encounter pacing

Place and validate:

```text
Player start
Horse
Normal enemies
Protected batteries
Game Boy mini-boss role
Game Cartridge mini-boss role
Pre-boss role
Final boss
Ammo / health support
Secret routes
Cinematic ending room
```

Ensure optional collectibles remain difficult and hidden without objective text.

## Stage 26 — Biomes, terrain, and ground textures

Create final environment surfaces:

```text
Grass
Sand
Mud
Water
Dry ground
Mixed transition zones
```

The amount of grass / sand / mud should vary naturally by area.

## Stage 27 — Walls, environment materials, and props

Create region-appropriate walls and environmental identity.

Requirements:

```text
Walls reflect nearby biome
Some areas have heavy vegetation
Some areas have little or no grass
Sand / mud / water affect surrounding materials
Large rooms have distinctive silhouettes
Environmental storytelling props
```

## Stage 28 — Lighting, atmosphere, camera readability, and VFX

Polish:

```text
Forward visibility and readable horizon distance
Camera tilt and darkness balance
Room lighting
Outdoor atmosphere
Wind
Spawn smoke / teleport effects
Impact VFX
Laser VFX
Bullet-hell readability
Magical exit barrier
```

Do not change mouse aiming rules while adjusting camera visibility.

## Stage 29 — Character, enemy, horse, weapon, and projectile art

Replace prototypes with coherent final visual direction:

```text
Player model / textures
Horse model / textures
Normal enemy models / textures
Mini-boss models / textures
Final boss model / textures
Weapons
Sword visuals
Bullets
Lasers
Bombs
Explosions
Collectibles
Reward chests
```

## Stage 30 — HUD, minimap, ammo, reload, and boss UI polish

Polish:

```text
Ammo count
Time until next ammo / reload completion
Animated reload state
Minimap discovery and placement
Collectible badge after pickup only
Boss health bars
Multi-bar / segmented boss UI
Status effects: slow / knockback feedback
```

No hidden collectible checklist or missing-item UI.

## Stage 31 — Audio foundation and full sound pass

Add background music / ambience and required effects:

```text
General gameplay background audio
Wind
Walking
Running
Horse riding
Horse states
Weak hit
Heavy hit
Sword attacks
Gunfire
Projectile impacts
Bomb placement and explosion
Enemy spawn
Mini-boss intro
Boss phase changes
Laser charge and firing
Clap crush
Stomp
Snake attacks
Chest opening
Collectible pickup
Magical barrier disappearance
```

## Stage 32 — Ending cinematics and speech bubbles

Polish all ending branches.

Collectible states:

```text
No Game Boy: player sits only
Game Boy without enough batteries: takes it out, cannot power it, places it aside
Powered Game Boy without cartridge: gray / white no-game lighting
Powered Game Boy with cartridge: colorful loaded-game lighting
```

Final speech bubble:

```text
Missing any required collectible: I'm bored
All required collectibles: I'm having fun :)
```

Required complete set:

```text
Game Boy
2 Batteries
Game Cartridge
```

## Stage 33 — Combat and difficulty balance

Balance:

```text
Normal enemies
Battery guardians
Mini-boss health and damage
Final boss health and damage
Projectile density
Summon caps
Summon timing
Dodge i-frame duration
Ammo economy
Horse survivability
Recovery windows
```

Every difficult attack must remain readable and avoidable.

## Stage 34 — Readability, accessibility, and performance

Validate:

```text
Projectile color contrast
Black/white boss readability
Telegraphs
Screen clutter
Camera motion
No slow camera spin during backward dodge
No enemy clustering
Object pooling for projectiles and VFX
Profiler pass
Memory / GC allocations
Stable frame time
```

## Stage 35 — Production code, repository, assets, and progression cleanup

Required production work:

```text
Modular code architecture
Remove obsolete generated versions
Keep one authoritative README and roadmap
Unity .meta integrity
No Library / Temp / Obj / Logs in Git
Prefab and ScriptableObject data where appropriate
No runtime UnityEditor dependencies
Automated validation tools
Save / progression planning
Deterministic map seed storage
Build/version metadata
```

## Stage 36 — Full vertical-slice QA and playable build

Final acceptance:

```text
Zero compiler errors
Zero release-blocking Console errors
Complete start-to-finish run
All 3 selected mini-bosses function
Random roles and placement function
One of 4 mini-bosses is absent per run
Final boss fully functions
Exit opens only after full victory
All ending variants work
Speech bubble rule works
Minimap works
Ammo / reload UI works
Audio works
No hidden collectible advertising
Performance target passes
Playable desktop build generated
```

# Current next action

```text
Stage 16 — Boss and mini-boss production framework
```

Before individual boss implementation, create the shared architecture and combat prerequisites so the four mini-bosses and final boss do not become five isolated one-off systems.
