# Mini-boss Archetype — Square Jumper V127

## Status

Design approved as a mini-boss archetype.

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
Large square enemy
Heavy body
Slow movement
Fast, powerful jump impact
Bullet-hell pressure
Occasional summon pressure
Dual-sword side attack
```

The enemy should feel simple to understand visually, but dangerous when it commits to a jump.

## Combat feel

The boss is mostly slow and readable.

The danger comes from sudden vertical commitment:

```text
slow tracking
clear jump telegraph
fast landing
heavy impact animation
bullet-hell burst after landing
```

The landing animation must communicate weight and power.

## Main attacks

### 1. Heavy jump slam

Behavior:

```text
Boss tracks the player slowly.
Boss crouches / compresses before jump.
Boss jumps toward the player.
Boss lands quickly and heavily.
Landing creates impact VFX and hit area.
```

Counterplay:

```text
Player sees the crouch telegraph.
Player dodges away before landing.
Dodge i-frames should protect from overlap damage if timed correctly.
```

### 2. Bullet-hell burst

Behavior:

```text
After landing, boss fires bullets around itself / around the player.
Pattern should be readable, not random noise.
```

Possible patterns:

```text
radial ring
spiral ring
cross pattern that rotates
short fan bursts from each side
```

Counterplay:

```text
Player moves between bullets.
Player can dodge through bullets while i-frames are active.
Bullets must still collide with walls.
```

### 3. Regular enemy summon

Behavior:

```text
Boss occasionally spawns 2–3 regular enemies.
```

Limits:

```text
Do not spam endlessly.
Do not spawn enemies directly on the player.
Do not spawn during every jump.
Summons should use spawn VFX / delay.
```

### 4. Dual sword side attack

Behavior:

```text
Boss sometimes pulls two swords.
Boss attacks with both swords from both sides at the same time.
```

Visual requirement:

```text
Both swords must be visible.
The attack must clearly show simultaneous left/right danger.
```

Counterplay:

```text
Player dodges backward, forward through i-frames, or out of the side arcs.
```

## Phase structure

### Phase 1 — Heavy box

Health range:

```text
100% → 65%
```

Behavior:

```text
slow movement
jump slam
light bullet-hell burst after landing
rare summon
rare dual sword attack
```

### Phase 2 — Broken rhythm

Health range:

```text
65% → 30%
```

Behavior:

```text
jump frequency increases
bullet-hell patterns become denser
summons become more likely
sword attack can happen after a landing
```

### Phase 3 — Panic box

Health range:

```text
30% → 0%
```

Behavior:

```text
jumps more often
more bullet-hell
summons up to the configured maximum
faster recovery between patterns
```

Hard limit:

```text
Do not create infinite enemy spam.
Do not make the arena unreadable.
Do not remove telegraphs.
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
Boss is slow outside jump attacks.
Jump has a visible crouch/charge telegraph.
Landing is fast and visually heavy.
Bullet-hell begins only after a readable tell.
Dodge i-frames work through boss bullets.
Boss bullets still hit walls.
Summons use VFX and do not appear on top of the player.
Summons are capped.
Two swords are visible during side attack.
Reward box opens only after boss death.
Secret collectible appears only after box opens.
No objective/checklist/missing text appears.
```
