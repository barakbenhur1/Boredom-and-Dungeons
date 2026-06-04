# Mini-boss Archetype — Roller V127

## Status

Design approved as a second mini-boss archetype.

This is not locked to a specific reward.

The mini-boss roster system should be able to randomly assign this archetype to one of the secret rewards, according to the random reward assignment rules.

Possible reward assignments:

```text
Game Boy guardian
Game Cartridge guardian
Pre-boss mini-boss, if selected for that role later
```

## Core identity

```text
Large round enemy
Heavy rolling body
Slow tracking outside attacks
Fast roll attack toward the player
Rotating spiral bullet-hell
Summons jumping enemies, bomb placers, and sword enemies
```

This boss is similar in role to Square Jumper, but its movement identity is different:

```text
Square Jumper commits vertically.
Roller commits horizontally by rolling across the arena.
```

## Combat feel

The boss should feel like a heavy rolling hazard.

Outside attack windows, it is slow and readable.

When it commits, it rolls quickly toward the player and forces repositioning.

## Main attacks

### 1. Heavy roll charge

Behavior:

```text
Boss slowly aims toward the player.
Boss compresses / spins up before rolling.
Boss rolls quickly toward the player.
Boss continues past the player or stops with a heavy skid.
```

Counterplay:

```text
Player reads spin-up.
Player dodges sideways.
Player can punish after the roll recovery.
```

### 2. Rotating spiral bullet-hell

Behavior:

```text
Boss emits bullets in a spiral.
The spiral continues rotating over time.
The pattern should feel like a moving rotating hazard, not a single static ring.
```

Counterplay:

```text
Player moves with/against spiral gaps.
Player can dodge through bullets while i-frames are active.
Bullets must still collide with walls.
```

### 3. Regular enemy summon

Behavior:

```text
Boss occasionally spawns 2–3 regular enemies.
```

Allowed summon pool for this archetype:

```text
Jumping enemies
Bomb-placer enemies
Sword enemies
```

Limits:

```text
Do not spam endlessly.
Do not spawn enemies directly on the player.
Do not spawn during every roll.
Summons should use spawn VFX / delay.
Summons must be capped per phase/fight.
```

### 4. Body crush / wall bounce

Optional later attack:

```text
Boss rolls into wall.
Boss bounces or redirects with a readable delay.
```

This should be added only if it remains readable and fair.

## Phase structure

### Phase 1 — Slow wheel

Health range:

```text
100% → 65%
```

Behavior:

```text
slow tracking
single roll charge
light rotating spiral
rare summon from jumper/bomb/sword pool
```

### Phase 2 — Wobble spiral

Health range:

```text
65% → 30%
```

Behavior:

```text
roll frequency increases
spiral rotates longer
summons become more likely
shorter recovery after roll
```

### Phase 3 — Runaway wheel

Health range:

```text
30% → 0%
```

Behavior:

```text
rolls more often
spiral bullet-hell becomes denser
summons up to the configured maximum
faster recovery between patterns
```

Hard limit:

```text
Do not create infinite enemy spam.
Do not make the arena unreadable.
Do not remove telegraphs.
Do not create unavoidable roll damage.
```

## Reward behavior

If this mini-boss is assigned to guard the Game Boy:

```text
After the boss dies, a box opens with an animation.
The Game Boy appears inside the box.
The player can pick it up only after the box opens.
```

If this mini-boss is assigned to guard the Game Cartridge:

```text
After the boss dies, a box opens with an animation.
The Game Cartridge appears inside the box.
```

The reward box should not advertise what is inside before victory.

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
Boss arena identity
Box opening animation after victory
Badge only after pickup
Cinematic consequence later
```

## Required related rule

The dodge system must support projectile i-frame behavior:

```text
If dodge i-frames are active, the player should pass through enemy bullets without damage.
If i-frames are not active, bullets should damage normally.
Bullets should still hit walls.
```

See:

```text
Assets/_Project/Design/Combat/DODGE_IFRAME_PROJECTILE_RULE.md
```

## QA checklist for implementation later

```text
Boss is slow outside roll attacks.
Roll has a visible spin-up telegraph.
Roll is fast but avoidable.
Spiral bullet-hell rotates continuously.
Dodge i-frames work through boss bullets.
Boss bullets still hit walls.
Summons use VFX and do not appear on top of the player.
Summons are capped.
Summons use only jumping/bomb-placer/sword enemies for this archetype.
Reward box opens only after boss death.
Secret collectible appears only after box opens.
No objective/checklist/missing text appears.
```
