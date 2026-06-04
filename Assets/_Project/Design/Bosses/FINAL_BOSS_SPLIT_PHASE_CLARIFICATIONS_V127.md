# Final Boss Split-Phase Clarifications V127

## Status

This document is an authoritative clarification for:

```text
Assets/_Project/Design/Bosses/FINAL_BOSS_BLACK_WHITE_COLOSSUS_V127.md
```

These rules override any ambiguous wording in the earlier final-boss design document.

## Phase structure

```text
Stage 1: joined form
Stage 2: joined-to-split transition at 60% health
Stage 3: fully separated black and white halves
```

## Summoning rule — Stage 3 only

Enemy summoning is enabled only after Stage 3 has fully started.

```text
Stage 1: no regular-enemy summoning
Stage 2: no regular-enemy summoning
Stage 3: summoning enabled
```

The separation transition in Stage 2 must finish before either half can begin its summon timer.

In Stage 3:

```text
Black half may summon 1 random regular enemy every 2 seconds.
White half may summon 1 random regular enemy every 2 seconds.
```

Required safeguards:

```text
Use one shared active-summon cap for the entire final-boss encounter.
Do not accumulate unlimited enemies.
Do not spawn enemies directly on the player.
Use spawn VFX and activation delay.
Summoned enemies cannot attack before the spawn VFX completes.
Summon timers stop immediately when the relevant half dies or the encounter ends.
```

## Split-form laser rule

Each separated half has one remaining eye, but each half can perform both laser attack types.

### Sweeping laser

```text
Each half can fire a sweeping laser from its one eye.
The laser moves from side to side.
The sweeping sequence can repeat 3 times.
```

### Fast direct laser

```text
Each half can sometimes fire a fast laser directly at the player.
The attack must have an eye-charge flash and a short directional cue before release.
```

Coordination requirements:

```text
The halves should not create unavoidable crossing lasers.
A readable survival gap must remain.
Their laser timing may overlap only when the resulting pattern is fair and readable.
```

## Split-form physical attacks

Each half can attack with both its remaining leg and its remaining arm.

### Leg attack

```text
The half jumps or lunges toward the player with its remaining leg.
It attempts to stomp or collide with the player.
The landing location must be telegraphed.
```

### Hand attack

```text
The half reaches, swings, slams, or strikes toward the player with its remaining hand.
The hand hit volume must match the visible arm movement.
The attack must have a readable startup.
```

### One-arm spinning attack

```text
Each half can still perform its independent spinning attack with one arm.
Its attack radius is approximately half the joined-form two-arm spin radius.
```

## Stage 3 required attack set

Each separated half can use:

```text
Sweeping eye laser
Fast direct eye laser
Leg jump / stomp attack
Hand strike attack
One-arm reduced-range spinning attack
Coordinated bullet-hell patterns
Random regular-enemy summoning every 2 seconds while below the shared cap
```

## QA checklist

```text
No enemies are summoned during Stage 1.
No enemies are summoned during the Stage 2 separation transition.
Summon timers begin only after Stage 3 becomes active.
Each half can perform the sweeping laser attack.
Each half can perform the fast direct laser attack.
Each half can attack with its remaining leg.
Each half can attack with its remaining hand.
Each hand strike has a visible telegraph and matching hit volume.
Crossing laser patterns always leave a readable survival route.
The shared summon cap prevents unlimited enemy buildup.
Summons stop when the encounter ends.
```
