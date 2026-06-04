# Mini-boss Random Reward Assignment Rules V127

## Purpose

Mini-bosses should be a reusable roster, not fixed one-to-one encounters.

The player should not know in advance which mini-boss protects which secret collectible.

## Core rule

At map generation time, assign mini-boss rewards randomly from the mini-boss roster.

Required secret rewards:

```text
Game Boy
Game Cartridge
```

Rules:

```text
One mini-boss guards / unlocks the Game Boy.
A different mini-boss guards / drops the Game Cartridge.
The order between those two mini-bosses is random.
The same mini-boss cannot guard both rewards in the same run.
```

## Pre-boss mini-boss rule

The game can still have a mini-boss before the final boss.

This mini-boss should be selected from the remaining legal mini-boss pool or from a dedicated pre-boss pool.

It does not have to guard a secret collectible.

Possible rewards later:

```text
opens a gate
removes a barrier
drops health/ammo
unlocks the route toward the final boss
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

## Distance rules

```text
Do not place mini-boss rooms too close to the start.
Do not place both secret mini-bosses too close to each other.
Do not place a secret mini-boss directly next to the final boss room.
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
RewardAssignment
MapPlacementCandidate
ProgressionDepth
BranchSide
```

Example assignment:

```text
Run A:
Square Jumper guards Game Boy
Mud Bell guards Game Cartridge
Blade TV appears as pre-boss
```

```text
Run B:
Blade TV guards Game Boy
Square Jumper guards Game Cartridge
Mud Bell appears as pre-boss
```

## QA checklist later

```text
Game Boy and Game Cartridge are assigned to different mini-bosses.
Order can change between generated maps.
Parallel branch layout can happen sometimes.
Bosses are not placed too close together.
Secret collectible UI rules are preserved.
Final boss remains in the final room.
Exit remains blocked until final boss is defeated.
```
