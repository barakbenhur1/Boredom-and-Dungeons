# Mini-boss Random Reward Assignment Rules V127

## Purpose

Mini-bosses should be a reusable roster, not fixed one-to-one encounters.

The player should not know in advance which mini-boss protects which secret collectible.

## Core roster rule

The game should have a pool of four mini-boss archetypes.

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

## Current mini-boss pool

Planned pool size:

```text
4 mini-boss archetypes total
```

Currently designed:

```text
1. Square Jumper
2. Roller
```

Still to design:

```text
3. Mini-boss archetype 3
4. Mini-boss archetype 4
```

## Selection examples

### Run A

```text
Selected: Square Jumper, Roller, Mini-boss 3
Not selected: Mini-boss 4
Square Jumper guards Game Boy
Roller guards Game Cartridge
Mini-boss 3 appears before final boss
```

### Run B

```text
Selected: Roller, Mini-boss 3, Mini-boss 4
Not selected: Square Jumper
Mini-boss 4 guards Game Boy
Roller guards Game Cartridge
Mini-boss 3 appears before final boss
```

### Run C

```text
Selected: Square Jumper, Mini-boss 3, Mini-boss 4
Not selected: Roller
Mini-boss 3 guards Game Boy
Square Jumper guards Game Cartridge
Mini-boss 4 appears before final boss
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

The future implementation should use deterministic seeded randomization so the same generated map can be reproduced for debugging.

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
1. Shuffle 4-mini-boss pool.
2. Take first 3 as selected mini-bosses.
3. Shuffle roles: Game Boy, Game Cartridge, Pre-boss.
4. Assign one selected mini-boss to each role.
5. Choose legal placement candidates for each assigned role.
6. Validate distance/progression/secret rules.
7. If validation fails, reroll placement or assignment within a bounded retry count.
```

## QA checklist later

```text
Exactly 3 mini-bosses appear in a generated map.
Exactly 1 mini-boss from the pool is absent.
Game Boy and Game Cartridge are assigned to different mini-bosses.
Pre-boss mini-boss is different from both secret reward mini-bosses.
Order can change between generated maps.
Parallel branch layout can happen sometimes.
Bosses are not placed too close together.
Pre-boss is before final boss but not directly adjacent to it.
Secret collectible UI rules are preserved.
Final boss remains in the final room.
Exit remains blocked until final boss is defeated.
```
