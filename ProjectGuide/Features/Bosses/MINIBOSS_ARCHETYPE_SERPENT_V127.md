# Mini-boss Archetype — Serpent V127

## Status

Design approved as the third mini-boss archetype in the four-mini-boss pool.

This archetype is not permanently tied to one reward.

Possible randomly assigned roles:

```text
Game Boy guardian
Game Cartridge guardian
Pre-boss mini-boss
```

## Core identity

```text
Large crawling snake
Long segmented body
Fast lunge bite
Tail grab with heavy damage
Tail whip
Three-shot fan burst from mouth
Contact damage on body touch
Head-only weak point
Enraged final phase
Reward always appears inside a chest after victory
```

## Combat role

The Serpent is a precision and positioning mini-boss.

The player cannot damage the whole body normally. The head is the required damage target.

The body controls space, while the head periodically exposes itself during attacks.

## Movement

The Serpent should crawl and curve like a snake rather than rotate as one rigid object.

Required movement feel:

```text
head leads movement
body segments follow with delay
turns create a visible snake curve
movement remains readable from top-down camera
```

The boss should not instantly rotate its entire body toward the player.

## Damage rules

### Boss damaging the player

```text
Touching the snake body causes contact damage.
Touch damage should use a short per-target cooldown.
The player must not lose health every frame while touching the body.
```

### Player damaging the boss

```text
Only attacks that hit the head deal normal boss damage.
Body hits should deal no damage or clearly reduced damage.
Body hits need visible feedback showing that the attack was ineffective.
```

Recommended feedback for body hits:

```text
small armor/scale spark
muted hit sound
brief gray damage indicator
no normal damage number
```

## Main attacks

### 1. Lunge bite

Behavior:

```text
The snake raises or pulls back its head.
The bite direction is telegraphed.
The head lunges quickly toward the player.
The head remains exposed briefly after the bite.
```

Counterplay:

```text
Dodge sideways or diagonally.
Attack the head during recovery.
Correct dodge timing can use i-frames against the bite damage.
```

Fairness rules:

```text
No instant bite without head-pullback telegraph.
Do not perfectly track the player after the lunge begins.
Recovery must be long enough for at least one meaningful punish window.
```

### 2. Tail grab and release

Behavior:

```text
The tail attempts to wrap around the player.
If the grab connects, the player is briefly restrained.
The snake squeezes or slams once for heavy damage.
The snake then releases the player automatically.
```

Counterplay:

```text
Dodge before the tail closes.
Move out of the telegraphed tail zone.
```

Hard rules:

```text
The grab cannot hold the player indefinitely.
The grab cannot repeat immediately after release.
The player must regain control after the release animation.
The grab needs a clear visual telegraph.
```

### 3. Tail whip

Behavior:

```text
The snake swings its tail across a wide arc.
The attack controls the area beside and behind the snake.
```

Counterplay:

```text
Move outside the arc.
Dodge through with active i-frames.
Approach the head from the opposite side after the swing.
```

### 4. Three-shot mouth fan

Behavior:

```text
The snake fires 3 projectiles from its mouth in a fan.
The attack can repeat up to 3 times in sequence.
Each repeated fan has a short readable pause before the next one.
```

Maximum sequence:

```text
Fan 1: 3 projectiles
Fan 2: 3 projectiles
Fan 3: 3 projectiles
Maximum total in one sequence: 9 projectiles
```

The boss may stop after the first or second fan. It does not always need to fire all three repetitions.

Counterplay:

```text
Move through fan gaps.
Dodge through projectiles while i-frames are active.
Use walls as cover where legal.
Punish the head during firing recovery.
```

Projectile rules:

```text
Projectiles must collide with walls.
Projectiles must not pass through level geometry.
Dodge i-frames prevent projectile damage while active.
```

## Phase structure

### Phase 1 — Hunting coil

Health range:

```text
100% → 65%
```

Behavior:

```text
slow-to-medium crawling
single lunge bite
single three-shot fan
rare tail whip
rare tail grab
longer punish windows on head
```

### Phase 2 — Tightening coil

Health range:

```text
65% → 30%
```

Behavior:

```text
faster crawling
more frequent lunge bite
two fan repetitions become common
tail attacks become more frequent
shorter head recovery windows, but still readable
```

### Phase 3 — Enraged serpent

Health range:

```text
30% → 0%
```

Behavior:

```text
movement becomes faster
attack transitions become faster
up to three fan repetitions
more frequent lunge attacks
more aggressive tail whip and grab attempts
```

Required enraged presentation:

```text
clear phase-change animation
faster body movement
stronger head/eye glow or equivalent visual cue
stronger sound cue
```

Hard limits:

```text
Do not remove attack telegraphs.
Do not make the head permanently unreachable.
Do not chain grab directly into another grab.
Do not combine unavoidable body contact with unavoidable projectile patterns.
```

## Summons

Current design:

```text
The Serpent does not summon regular enemies.
```

Its challenge comes from body positioning, head-only damage, contact damage, tail control, and projectile fans.

This keeps it distinct from Square Jumper and Roller.

## Reward behavior

All mini-boss rewards use a chest.

If assigned to the Game Boy role:

```text
The Serpent dies.
A reward chest unlocks and opens with an animation.
The Game Boy is revealed inside.
```

If assigned to the Game Cartridge role:

```text
The Serpent dies.
A reward chest unlocks and opens with an animation.
The Game Cartridge is revealed inside.
```

If assigned to the pre-boss role:

```text
The Serpent dies.
A chest may contain health/ammo or the encounter may unlock the forward route.
The exact pre-boss reward is decided later.
```

The chest must not reveal its contents before the mini-boss dies.

## Secret UI rules

Forbidden:

```text
Find the Game Boy
Find the Cartridge
Objective marker
Checklist
Missing item text
0/4 progress
```

Allowed:

```text
Boss health bar
Head weak-point feedback
Chest opening animation after victory
Badge only after collectible pickup
```

## Required implementation systems later

```text
Segmented snake-follow movement
Head weak-point hitbox
Body contact damage cooldown
Tail grab state and forced release
Tail whip arc telegraph
Mouth projectile fan sequence
Enraged phase controller
Reward chest controller
```

## QA checklist for implementation later

```text
Snake crawls in a curved segmented motion.
Snake does not rotate as one rigid block.
Touching the body causes damage with a cooldown, not every frame.
Body attacks do not deal normal boss damage.
Head attacks deal boss damage.
Body-hit feedback clearly shows ineffective damage.
Bite has a visible telegraph and fixed lunge commitment.
Head is punishable after bite.
Tail grab releases the player automatically.
Player regains control after release.
Tail grab cannot chain immediately.
Tail whip has a visible arc.
Mouth attack fires exactly 3 projectiles per fan.
Mouth attack repeats no more than 3 times per sequence.
Projectiles collide with walls.
Dodge i-frames prevent projectile damage while active.
Enraged phase is faster but still readable.
Reward chest opens only after death.
Secret reward appears only after chest opens.
No objective/checklist/missing text appears.
```
