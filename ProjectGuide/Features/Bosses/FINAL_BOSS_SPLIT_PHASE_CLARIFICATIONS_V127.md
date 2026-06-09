# Final Boss Split-Phase Clarifications V127

## Status

This document is the authoritative clarification for:

```text
ProjectGuide/Features/Bosses/FINAL_BOSS_BLACK_WHITE_COLOSSUS_V127.md
```

These rules override any ambiguous wording in the earlier final-boss design document.

## Final phase structure

```text
Stage 1: joined black/white form
Stage 2: separation phase beginning at 60% joined health
Stage 3: final fully separated phase
```

## Stage 1 — Joined form

```text
The boss is one joined body.
It uses the joined-form attack set.
There is one joined boss health bar.
No regular enemies are summoned.
```

When the joined health reaches 60%, Stage 2 begins.

## Stage 2 — Separation phase with independent phase health

At the start of Stage 2:

```text
The boss performs the full split animation.
The black half and white half become separate active enemies.
Each half receives its own visible Stage-2 health bar.
```

Stage-2 knockout rule:

```text
If one half loses all of its Stage-2 health,
that half becomes knocked out / unconscious for the remainder of Stage 2.
It collapses temporarily and stops moving, attacking, firing, summoning, and dealing contact damage.
It is not permanently dead.
The surviving half continues fighting alone.
```

Stage 2 ends only after both halves have lost all of their Stage-2 health and both are knocked out.

Then Stage 3 begins and both halves return alive for the final phase.

Important:

```text
No regular enemies are summoned during Stage 2.
```

## Stage 3 — Final split phase with linked final defeat

At the beginning of Stage 3:

```text
Both the black half and white half are alive again.
Each half has its own separate final health bar.
Both halves use the full Stage-3 attack set.
Summoning becomes enabled.
```

Final linked-defeat rule:

```text
Both halves continue fighting until both final health bars have reached zero.
The final death event is delayed until both separate final health bars are zero.
```

If one half reaches zero first:

```text
Its health remains clamped at zero.
It stays fully active and continues moving, attacking, firing, flanking, using bullet-hell patterns, and summoning.
It does not become knocked out.
It does not collapse.
It cannot trigger victory or remove the exit barrier by itself.
```

Damage-immunity rule for a zero-health half:

```text
Once a Stage-3 half has reached zero health,
additional player attacks against that half do nothing.
They cannot reduce health below zero.
They cannot create additional damage, stagger, knockback, or repeated defeat reactions.
The player must damage the other half until its final health also reaches zero.
```

The complete boss encounter ends only when:

```text
Black final health == 0
AND
White final health == 0
```

Only at that moment do both halves stop attacking and begin their separate final collapses.

## Summoning rule — Stage 3 only

Enemy summoning is enabled only after Stage 3 has fully started.

```text
Stage 1: no regular-enemy summoning
Stage 2: no regular-enemy summoning
Stage 3: summoning enabled
```

In Stage 3:

```text
Black half may summon 1 random regular enemy every 2 seconds.
White half may summon 1 random regular enemy every 2 seconds.
```

A half that has reached zero health first remains combat-active and may continue its Stage-3 summoning behavior until both final health bars are zero.

Required safeguards:

```text
Use one shared active-summon cap for the entire final-boss encounter.
Do not accumulate unlimited enemies.
Do not spawn enemies directly on the player.
Use spawn VFX and activation delay.
Summoned enemies cannot attack before the spawn VFX completes.
Summon timers stop immediately when both final health bars reach zero and the encounter ends.
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

A zero-health half keeps this complete attack set active until the other half also reaches zero.

## Final death animation

When both Stage-3 final health bars reach zero:

```text
Both halves stop attacking.
The black half collapses separately.
The white half collapses separately.
They do not rejoin before dying.
```

The two collapses should be visually coordinated as one final defeat moment, while remaining separate bodies.

Neither half performs its final collapse before both final health bars have reached zero.

## Exit barrier rule

The final-room exit is blocked by a visible magical barrier while the boss encounter is active.

Before full defeat:

```text
The magical barrier remains solid and cannot be crossed.
It does not disappear when only one half reaches zero.
It does not disappear during Stage 2.
It does not disappear while either Stage-3 final health bar is above zero.
```

After both halves have completed their separate final collapse:

```text
The magical barrier disappears with a dedicated animation.
The effect visibly weakens, cracks, breaks apart, fades, dissolves, or disperses.
The player receives clear visual and audio confirmation.
The exit becomes traversable only after the barrier animation completes.
```

## QA checklist

```text
Stage 1 uses one joined health bar.
No enemies are summoned during Stage 1.
The boss separates at 60% joined health.
Stage 2 gives each half its own health bar.
A half reaching zero in Stage 2 becomes knocked out and stops fighting for the remainder of that stage.
A knocked-out Stage-2 half is not treated as permanently dead.
Stage 2 ends only after both Stage-2 half health bars are depleted.
No enemies are summoned during Stage 2.
Both halves return alive for Stage 3.
Stage 3 gives each half its own final health bar.
A Stage-3 half reaching zero first remains fully combat-active.
A zero-health Stage-3 half continues attacking and summoning until the other half reaches zero.
Additional attacks against a zero-health Stage-3 half do no damage and cause no stagger or knockback.
The final defeat waits until both Stage-3 health bars reach zero.
Summon timers begin only after Stage 3 becomes active.
Summon timers stop only when both final health bars reach zero.
Each half can perform the sweeping laser attack.
Each half can perform the fast direct laser attack.
Each half can attack with its remaining leg.
Each half can attack with its remaining hand.
Each hand strike has a visible telegraph and matching hit volume.
Crossing laser patterns always leave a readable survival route.
The shared summon cap prevents unlimited enemy buildup.
Black and white halves collapse separately after both final bars reach zero.
The magical exit barrier remains until both collapses complete.
The magical barrier disappears with an animation.
The exit cannot be crossed before the barrier animation finishes.
```
