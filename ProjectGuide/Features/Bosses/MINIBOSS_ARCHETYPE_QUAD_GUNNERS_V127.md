# Mini-boss Archetype — Quad Gunners V127

## Status

Design approved as the fourth mini-boss archetype in the four-mini-boss pool.

This encounter is made of four separate characters that act as one coordinated mini-boss squad.

It is not permanently tied to one reward.

Possible randomly assigned roles:

```text
Game Boy guardian
Game Cartridge guardian
Pre-boss mini-boss
```

## Core identity

```text
Four separate characters
Each character has a minigun
Each character has one unique bullet behavior
Each character has one fixed hard-coded color
Each character has one fixed hard-coded summon type
High-volume ranged pressure
Coordinated sword flanking when one member is attacked in melee
Survivors become gradually faster as members die
Reward always appears inside a chest after victory
```

## Hard-coded identity rule

Color, bullet type, summon type, and character identity are permanent.

They must never be randomized between runs.

They must not be assigned dynamically by list order.

They should be represented by explicit IDs / enum values in code.

Recommended fixed IDs:

```text
RapidBlue
HeavyRed
SlowPurple
KnockbackYellow
```

Required rule:

```text
RapidBlue is always blue.
HeavyRed is always red.
SlowPurple is always purple.
KnockbackYellow is always yellow.
```

If the squad is instantiated in a different hierarchy order, the identity still must remain the same.

## Permanent character mapping

### 1. RapidBlue

Hard-coded color:

```text
Blue / cyan
```

Bullet behavior:

```text
Bullets move faster than a normal bullet.
Bullets deal less damage than a normal bullet.
```

Fixed summon type:

```text
Jumping enemies
```

Permanent mapping:

```text
RapidBlue -> Blue color -> Fast low-damage bullets -> Jumping enemies
```

### 2. HeavyRed

Hard-coded color:

```text
Red / crimson
```

Bullet behavior:

```text
Bullets move slower than a normal bullet.
Bullets deal more damage than a normal bullet.
```

Fixed summon type:

```text
Bomb-placer enemies
```

Permanent mapping:

```text
HeavyRed -> Red color -> Slow high-damage bullets -> Bomb-placer enemies
```

### 3. SlowPurple

Hard-coded color:

```text
Purple / violet
```

Bullet behavior:

```text
Bullets deal normal damage.
A hit slows the player for exactly 1 second.
```

Fixed summon type:

```text
Shooter enemies
```

Permanent mapping:

```text
SlowPurple -> Purple color -> Normal-damage slowing bullets -> Shooter enemies
```

### 4. KnockbackYellow

Hard-coded color:

```text
Yellow / amber
```

Bullet behavior:

```text
Bullets deal normal damage.
A hit applies knockback to the player.
```

Fixed summon type:

```text
Sword enemies
```

Permanent mapping:

```text
KnockbackYellow -> Yellow color -> Normal-damage knockback bullets -> Sword enemies
```

## Fixed mapping table

This table must never change between runs:

```text
RapidBlue       -> Blue   -> Fast / low damage   -> Jumpers
HeavyRed        -> Red    -> Slow / high damage  -> Bomb placers
SlowPurple      -> Purple -> Normal + 1s slow    -> Shooters
KnockbackYellow -> Yellow -> Normal + knockback  -> Sword enemies
```

## Summon behavior

Each living gunner has its own summon timer.

Required rule:

```text
Every 10 seconds, each living gunner may summon 2 enemies of its permanently assigned type.
```

Examples:

```text
RapidBlue summons 2 jumpers.
HeavyRed summons 2 bomb placers.
SlowPurple summons 2 shooters.
KnockbackYellow summons 2 sword enemies.
```

Spawn requirements:

```text
Summons use spawn VFX and anticipation delay.
Summons do not appear directly on the player.
Summons do not overlap each other.
Summons do not attack before their spawn VFX completes.
```

Safety cap:

```text
Use one shared active-summon cap for the whole encounter.
Recommended starting cap: 8 active summoned enemies.
```

If the cap is reached, summon timers may continue, but no new enemies spawn until active count drops below the cap.

Infinite accumulation is forbidden.

## Minigun firing behavior

The squad fires a lot.

Required feel:

```text
High projectile density
Distinct fixed projectile colors
Distinct permanent projectile behavior
Short micro-pauses so patterns remain readable
Staggered firing offsets so all four do not create one unreadable wall every frame
```

Each gunner keeps its identity for the entire fight.

Killing one gunner permanently removes that gunner's bullet type and summon type from the encounter.

## Sword-flank response

Every gunner also carries a backup sword.

Trigger:

```text
If the player attacks one gunner with the sword,
the other living gunners switch temporarily to coordinated melee pressure.
```

Behavior with four alive:

```text
The attacked gunner disengages or keeps limited suppressive fire.
The other three run toward three separate flank positions around the player.
They attack with swords from different directions.
```

Flanking rule:

```text
They must not run to the same point.
They must not cluster together.
They must reserve separate approach slots.
```

Recommended flank slots:

```text
left-front
right-front
rear-side
```

Behavior with fewer survivors:

```text
3 alive: the other 2 use separate left/right flank slots.
2 alive: the partner uses one side flank while the attacked gunner disengages.
1 alive: no squad flank is possible; the last gunner uses solo behavior.
```

After the melee response window ends, survivors return to ranged formation.

## Formation behavior

The squad should spread around the arena rather than form one cluster.

Rules:

```text
Each gunner reserves a different formation slot.
Maintain minimum spacing between squad members.
Reposition when two members become too close.
Do not block every escape direction at once without a telegraph.
```

## Survivor speed escalation

When one gunner dies, all surviving gunners become slightly faster.

Required gradual multipliers:

```text
4 alive: 1.00x speed
3 alive: 1.08x speed
2 alive: 1.16x speed
1 alive: 1.25x speed
```

Apply to:

```text
movement speed
rotation / reposition speed
attack transition speed
minigun burst recovery
melee flank approach speed
```

Do not alter permanent identity values:

```text
RapidBlue remains blue and keeps fast low-damage bullets.
HeavyRed remains red and keeps slow high-damage bullets.
SlowPurple remains purple and keeps a 1-second slow.
KnockbackYellow remains yellow and keeps knockback bullets.
```

## Fight progression by survivors

### Four alive

```text
All four colors active
All four bullet types active
All four summon types possible
Three-person sword flank response
1.00x speed
```

### Three alive

```text
One permanent identity removed
Remaining three become slightly faster
Two-person flank response
1.08x speed
```

### Two alive

```text
Two permanent identities remain
Higher individual pressure
One partner can flank
1.16x speed
```

### Last survivor

```text
No squad formation support
No coordinated flank response
Keeps its own hard-coded color, bullet type, and summon type
1.25x speed
```

The last survivor becomes faster, but does not gain an unavoidable new attack.

## Damage and death rules

```text
Each gunner has individual health.
The encounter ends only after all four die.
Killing one permanently removes its bullet type and summon type.
```

Preferred boss UI:

```text
One shared segmented health bar with four fixed color segments:
Blue, Red, Purple, Yellow.
```

Segment colors must match the characters permanently.

## Reward behavior

All mini-boss rewards use a chest.

If assigned to the Game Boy role:

```text
All four gunners must die.
The chest unlocks and opens with an animation.
The Game Boy appears inside.
```

If assigned to the Game Cartridge role:

```text
All four gunners must die.
The chest unlocks and opens with an animation.
The Game Cartridge appears inside.
```

If assigned to the pre-boss role:

```text
All four gunners must die.
The forward route unlocks and/or a resource chest opens.
```

The chest must not reveal contents before the full squad is defeated.

## Fairness rules

```text
Every bullet type has a clear fixed color.
Fast bullets do less damage.
High-damage bullets remain slower and visible.
Slow lasts 1 second.
Knockback cannot create permanent stun-lock.
Summons use VFX and are globally capped.
Flank attackers reserve separate positions.
Dodge i-frames prevent projectile damage while active.
Projectiles still collide with walls.
```

## Required implementation systems later

```text
Hard-coded GunnerIdentity enum
Four-member squad coordinator
Fixed color constants per identity
Fixed projectile profile per identity
Fixed summon mapping per identity
Slow status effect
Knockback projectile effect
Per-member summon timers
Shared summon cap
Formation-slot reservation
Melee flank-slot reservation
Alive-count speed multiplier
Four-segment colored boss health UI
Reward chest controller
```

## Suggested implementation constants

```text
RapidBlueColor       = hard-coded blue/cyan
HeavyRedColor        = hard-coded red/crimson
SlowPurpleColor      = hard-coded purple/violet
KnockbackYellowColor = hard-coded yellow/amber
```

The exact RGB values can be tuned visually later, but the identity-to-color mapping cannot change.

## QA checklist for implementation later

```text
Exactly four gunner characters spawn.
RapidBlue is always blue.
HeavyRed is always red.
SlowPurple is always purple.
KnockbackYellow is always yellow.
Hierarchy order does not change identity.
Prefab order does not change identity.
RapidBlue bullets are faster and lower damage.
HeavyRed bullets are slower and higher damage.
SlowPurple bullets deal normal damage and slow for 1 second.
KnockbackYellow bullets deal normal damage and apply knockback.
RapidBlue always summons jumpers.
HeavyRed always summons bomb placers.
SlowPurple always summons shooters.
KnockbackYellow always summons sword enemies.
Each living member may summon 2 enemies every 10 seconds.
Shared summon cap prevents infinite accumulation.
Sword flankers use separate positions and do not cluster.
Speed multipliers progress 1.00 / 1.08 / 1.16 / 1.25.
The final survivor keeps its original fixed identity.
All four must die before the reward chest opens.
No objective/checklist/missing collectible text appears.
```
