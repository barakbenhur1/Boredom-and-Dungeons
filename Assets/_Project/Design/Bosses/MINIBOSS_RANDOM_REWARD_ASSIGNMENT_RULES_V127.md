# Mini-boss Random Reward Assignment Rules V127

## Purpose

Mini-bosses are a reusable roster, not fixed one-to-one encounters.

The player should not know in advance which mini-boss protects which secret collectible.

## Core roster rule

The game has a pool of four mini-boss archetypes.

At map generation time:

```text
Select 3 mini-bosses randomly from the 4-mini-boss pool.
Leave 1 mini-boss out of that generated map.
Assign the 3 selected mini-bosses to the 3 mini-boss roles randomly.
```

The three roles are:

```text
Game Boy guardian
Game Cartridge guardian
Pre-boss mini-boss
```

Rules:

```text
One selected mini-boss guards / unlocks the Game Boy.
A different selected mini-boss guards / drops the Game Cartridge.
A third selected mini-boss appears before the final boss.
The same mini-boss cannot fill more than one role in the same run.
The same role should not always be assigned to the same archetype across runs.
```

## Required secret rewards

```text
Game Boy
Game Cartridge
```

The pre-boss mini-boss does not have to guard a secret collectible.

Possible pre-boss rewards later:

```text
opens a gate
removes a barrier
drops health/ammo
unlocks the route toward the final boss
```

## Complete mini-boss pool

```text
1. Square Jumper
2. Roller
3. Serpent
4. Quad Gunners
```

All four archetypes are now designed.

Exactly three are selected for each generated map.

Exactly one archetype is absent from that run.

## Permanent archetype identities

### Square Jumper

```text
Large square body
Jump slam
Bullet-hell patterns
Dual swords
Summons sword, shooter, and patrol enemies
```

### Roller

```text
Large round body
Roll charge
Rotating spiral bullet-hell
Summons jumping, bomb-placer, and sword enemies
```

### Serpent

```text
Large segmented snake
Lunge bite
Tail grab
Tail whip
Three-shot fan bursts
Head-only weak point
No regular summons
```

### Quad Gunners

```text
Four coordinated characters
Four hard-coded colors
Four hard-coded projectile profiles
Four hard-coded summon mappings
Coordinated sword flanking
Survivors become progressively faster
```

## Selection examples

### Run A

```text
Selected: Square Jumper, Roller, Serpent
Not selected: Quad Gunners
Square Jumper guards Game Boy
Roller guards Game Cartridge
Serpent appears before final boss
```

### Run B

```text
Selected: Roller, Serpent, Quad Gunners
Not selected: Square Jumper
Quad Gunners guard Game Boy
Roller guards Game Cartridge
Serpent appears before final boss
```

### Run C

```text
Selected: Square Jumper, Serpent, Quad Gunners
Not selected: Roller
Serpent guards Game Boy
Square Jumper guards Game Cartridge
Quad Gunners appear before final boss
```

### Run D

```text
Selected: Square Jumper, Roller, Quad Gunners
Not selected: Serpent
Roller guards Game Boy
Quad Gunners guard Game Cartridge
Square Jumper appears before final boss
```

## Placement patterns

The map generator / placement pass may choose between several layouts.

### Pattern A — sequential secret path

```text
Mini-boss reward A appears earlier.
Mini-boss reward B appears later.
The player may encounter one before the other.
```

### Pattern B — parallel branches

Two reward mini-bosses can appear around a similar progression depth.

Spatial rule:

```text
They can be placed at roughly similar Y progression,
but far apart on the X axis.
```

This creates a left/right optional route choice.

### Pattern C — one main-route pressure, one side secret

```text
One reward mini-boss sits near a stronger optional side area.
The other is later or deeper in the map.
```

### Pattern D — mixed route with pre-boss later

```text
The Game Boy and Game Cartridge mini-bosses are optional/secret route encounters.
The pre-boss mini-boss appears later, before the final boss,
but there should still be some map to play after it.
```

## Distance rules

```text
Do not place mini-boss rooms too close to the start.
Do not place both secret mini-bosses too close to each other.
Do not place a secret mini-boss directly next to the final boss room.
Do not place the pre-boss directly adjacent to the final boss room.
After the pre-boss, there should still be a bit more map before the final boss.
Do not place a mini-boss where the player is forced into instant damage.
```

## Reward chest rule

All secret collectible mini-boss rewards use a chest.

```text
The chest stays closed while the mini-boss is alive.
After the mini-boss dies, the chest opens with an animation.
The assigned Game Boy or Game Cartridge appears inside.
The contents are not advertised before victory.
```

## Secret rules

Forbidden before pickup:

```text
Find the Game Boy
Find the Cartridge
Objective marker
Checklist
Missing item text
0/4 progress text
```

Allowed:

```text
Distinct room silhouette
Environmental hints
Boss arena identity
Reward reveal after victory
Badge only after pickup
```

## Implementation notes for later

Use deterministic seeded randomization so the same generated map can be reproduced for debugging.

Possible data model:

```text
MiniBossArchetypeId
MiniBossPool
SelectedMiniBosses
SkippedMiniBoss
RewardAssignment
MapPlacementCandidate
ProgressionDepth
BranchSide
```

Pseudo-flow:

```text
1. Create fixed pool: Square Jumper, Roller, Serpent, Quad Gunners.
2. Shuffle the four-archetype pool using the map seed.
3. Take the first three as selected mini-bosses.
4. Mark the fourth as skipped for that run.
5. Shuffle roles: Game Boy, Game Cartridge, Pre-boss.
6. Assign one selected mini-boss to each role.
7. Choose legal placement candidates for each assigned role.
8. Validate distance, progression, parallel-layout, and secret rules.
9. If validation fails, reroll placement or assignment within a bounded retry count.
```

## QA checklist later

```text
The source pool always contains exactly four archetypes.
Exactly three mini-bosses appear in a generated map.
Exactly one mini-boss from the pool is absent.
Game Boy and Game Cartridge are assigned to different mini-bosses.
Pre-boss mini-boss is different from both secret reward mini-bosses.
Role assignment changes between generated maps.
Order can change between generated maps.
Parallel branch layout can happen sometimes.
Mini-boss rooms are not too close together.
Pre-boss is before final boss but not directly adjacent to it.
All collectible rewards use a post-victory chest.
Secret collectible UI rules are preserved.
Final boss remains in the final room.
Exit remains blocked until final boss is defeated.
```
