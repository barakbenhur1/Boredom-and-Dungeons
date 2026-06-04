# Final Boss — Black / White Colossus V127

## Status

Final boss design approved at concept level.

This document defines the boss identity, attacks, split behavior, and three-stage structure.

Implementation details such as exact health values, laser damage, projectile counts, death animation, and exit-opening animation are still to be tuned later.

## Placement rule

```text
The boss is always located in the final room.
The exit remains blocked while the boss is alive.
The exit can open only after the full boss encounter is defeated.
```

This boss is fixed and is not part of the random mini-boss pool.

## Core visual identity

```text
Height: at least 2.5x the player height
Right side: black
Left side: white
Normal state: both halves joined into one body
Split state: black half and white half become separate active enemies
```

The black/white split must remain visually clear in every phase.

## Core combat identity

Joined form:

```text
Large, slower, heavy-impact boss
Wide crushing attacks
Two-eye laser attacks
Large spinning body attack
Very difficult bullet-hell patterns
```

Split form:

```text
Two faster half-bosses
One eye laser per half
Coordinated flanking
Separate jump/stomp pressure
Separate one-arm spinning attacks
Very difficult bullet-hell patterns
Frequent enemy summoning
```

## Three-stage fight structure

### Stage 1 — Joined form

Health range:

```text
100% down toward 60%
```

The boss remains fully joined.

Available attacks:

```text
Clap crush
Dual-eye sweeping lasers
Fast direct eye laser
Stomp
Arms-out spinning movement
Very difficult bullet hell
```

### Stage 2 — Break / separation transition

Trigger:

```text
Boss reaches 60% health.
```

Behavior:

```text
The boss is still joined at the start of the transition.
A clear break/separation animation begins.
The black and white halves pull apart.
The arena changes from one-boss pressure to two-half coordination.
```

Fairness rules:

```text
The transition must be clearly telegraphed.
The player must not take unavoidable damage during the separation animation.
The separated halves must not become fully active before the transition visual is readable.
```

### Stage 3 — Split form

Health range:

```text
After the 60% split transition until defeat
```

The black and white halves fight independently but coordinate against the player.

Split-form behavior:

```text
Each half uses the one eye it still has.
Both halves are faster and more agile than the joined form.
The halves flank the player instead of stacking on the same position.
Each half can jump / stomp toward the player with its leg.
Each half can use a reduced-range one-arm spinning attack.
Each half participates in very difficult bullet-hell patterns.
Each half can summon one random regular enemy every 2 seconds.
```

## Joined-form attacks

### 1. Clap crush

Behavior:

```text
The boss opens both arms.
The player receives a clear side-crush telegraph.
The boss claps both hands together, trying to crush the player between them.
The attack deals very high damage.
```

Counterplay:

```text
Move out of the center line before the clap closes.
Use a correctly timed dodge through the danger zone.
Punish during the recovery after the clap.
```

Fairness rules:

```text
No instant clap without telegraph.
The clap must have a readable closing animation.
The hit volume must match the visible hands.
```

### 2. Dual-eye sweeping lasers

Behavior:

```text
One laser comes from each eye.
The lasers sweep from side to side across the visible arena / screen.
The sweep repeats 3 times.
```

Required sequence:

```text
Sweep 1
Short readable pause / direction change
Sweep 2
Short readable pause / direction change
Sweep 3
Recovery
```

Counterplay:

```text
Move with the safe gaps.
Use arena positioning.
Use dodge i-frames when crossing the laser line, if the implementation allows laser i-frame interaction.
```

### 3. Fast direct eye laser

Behavior:

```text
The boss occasionally fires a fast laser directly toward the player.
```

Required telegraph:

```text
Eye charge flash
Short aim line or directional cue
Fast release
```

The laser should be fast, but not invisible or unreactable.

### 4. Stomp

Behavior:

```text
The boss raises one foot.
The target area is telegraphed.
The boss tries to step on the player.
The impact deals heavy damage.
```

Optional impact effects later:

```text
Ground shockwave
Dust/debris
Camera impulse
Heavy impact sound
```

### 5. Arms-out spinning movement

Behavior:

```text
The boss stretches both arms outward.
The boss spins while moving around the arena.
The arms become a large moving damage zone.
The boss attempts to collide with the player during the movement path.
```

Counterplay:

```text
Read the startup pose.
Move away from the path.
Use arena edges and open gaps.
Punish after the spin recovery.
```

### 6. Joined-form bullet hell

Behavior:

```text
The boss creates a very difficult projectile pattern.
The pattern should challenge movement and dodge timing.
```

Required rules:

```text
Patterns must remain visually readable.
Projectiles must collide with walls where appropriate.
Dodge i-frames must prevent projectile damage while active.
No unavoidable overlap with clap, stomp, or spin without a safe route.
```

## Split-form coordination

### Flanking rule

The two halves should not stack on the same position.

Required behavior:

```text
One half approaches from one side.
The other half approaches from the opposite side or rear-side angle.
They reserve separate flank positions.
They reposition if they become too close together.
```

The goal is coordinated pressure, not visual overlap.

### Split jump / stomp

Behavior:

```text
Each half can jump toward the player using its remaining leg.
The jump lands quickly after a readable startup.
The impact attempts to stomp the player.
```

Coordination rule:

```text
The halves should not land on the exact same point at the exact same time unless there is a large readable safe area.
```

### Split one-eye laser

Behavior:

```text
Each half can fire one laser from its remaining eye.
The halves may aim from different sides.
```

Fairness rules:

```text
Do not create unavoidable crossing lasers without a safe gap.
Both charge cues must remain readable.
```

### Split one-arm spinning attack

Behavior:

```text
Each half stretches its remaining arm.
Each half spins independently.
Each half has approximately half the attack range of the joined two-arm spin.
```

Required spatial behavior:

```text
The halves do not occupy the same spin path.
Each half uses a separate movement route.
```

### Split-form bullet hell

Behavior:

```text
Both halves create a very difficult coordinated projectile pattern.
```

Possible coordination later:

```text
Black half controls one angle / rhythm.
White half controls a complementary angle / rhythm.
Patterns overlap in a planned way rather than random noise.
```

Hard rules:

```text
There must always be a possible survival route.
Projectile colors must remain readable against both black and white boss halves.
Dodge i-frames must work against projectile damage while active.
```

## Split-form summoning

Each separated half can summon one random regular enemy every 2 seconds.

Required rule:

```text
Black half: one random regular enemy every 2 seconds while allowed.
White half: one random regular enemy every 2 seconds while allowed.
```

This means the encounter may attempt to create up to two regular enemies every 2 seconds.

Required implementation safety:

```text
Use a shared active-summon cap for the entire final boss encounter.
Do not allow infinite enemy accumulation.
Do not summon enemies directly on top of the player.
Use spawn VFX and activation delay.
Do not let summoned enemies attack before their spawn VFX completes.
```

The exact shared cap is still to be tuned during implementation and balance testing.

## Speed and agility change

Joined form:

```text
Slower
Heavier
Wider attacks
Longer recovery
```

Split form:

```text
Faster movement
Faster repositioning
Faster attack transitions
More flanking pressure
More frequent ranged pressure
```

The split halves must still retain readable telegraphs even when faster.

## Damage and health structure

Current required transition:

```text
The boss splits at 60% health.
```

Implementation decision still to confirm later:

```text
Option A: one shared health pool continues through the split.
Option B: the remaining health is divided between the black and white halves.
```

The final encounter is defeated only when the required split-form defeat condition is complete.

No exit-opening event may happen early.

## Final room and exit lock

Before victory:

```text
The exit is physically blocked and cannot be used.
The block must be visible and readable.
```

After full boss defeat:

```text
The exit block is removed or opened.
The player receives a clear visual and audio confirmation.
The ending route becomes available.
```

The exact exit-opening animation is still to be designed.

## Death animation — open design item

The exact death animation was not defined yet.

It must later answer:

```text
Do the halves rejoin before dying?
Do black and white collapse separately?
Do they dissolve, crack, or fall apart?
How does the death connect visually to the exit opening?
```

No final death animation should be implemented until this is decided.

## Required implementation systems later

```text
Joined boss controller
Black/white visual split rig
60% phase-transition controller
Clap crush hit system
Dual-eye three-sweep laser system
Fast direct laser attack
Stomp telegraph and impact system
Moving spin attack
Joined bullet-hell controller
Separated-half AI coordinator
Flank-slot reservation
One-eye laser attack per half
One-arm half-range spin attack
Split jump/stomp attack
Split bullet-hell coordinator
Random regular-enemy summon system
Shared summon cap
Final-room exit lock controller
Boss defeat / exit-open event
```

## QA checklist for implementation later

```text
Boss is at least 2.5x player height.
Right side is always black.
Left side is always white.
Joined form remains visually unified before split.
Clap has a visible startup and matching hit volume.
Clap deals heavy damage but is avoidable.
Dual-eye laser performs exactly 3 side-to-side sweeps.
Fast direct laser has a readable charge cue.
Stomp has a visible target telegraph.
Joined spin uses both arms and moves around the arena.
Joined bullet hell remains difficult but readable.
Boss starts separation at 60% health.
Transition does not deal unavoidable damage.
Black and white halves become separate active enemies.
Each half uses only one eye laser.
The halves flank instead of stacking.
Each half can jump/stomp with its leg.
Each half uses a half-range one-arm spin.
Split-form bullet hell has a possible survival route.
Each half may attempt one summon every 2 seconds.
Shared summon cap prevents infinite accumulation.
Summons use VFX and do not spawn on the player.
Split halves are faster but retain telegraphs.
Exit remains blocked until the full encounter is defeated.
Exit opening cannot trigger early.
No console errors occur during split or defeat.
```
