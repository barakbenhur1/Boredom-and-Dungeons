# Boredom & Dungeons — Mother Boss Design V3 Decisions

Status: approved decisions recorded from the 2026-06-06 design discussion.

This document supersedes the unresolved decision list in `MOTHER_BOSS_DESIGN_V2_WORKING.md` except for the single remaining horse-presence question.

## 1. Patience resource

Mother does not have visible HP.

The visible resource is **Patience**.

- Each combat phase has a fresh full-width Patience bar.
- The label shows both the resource and the emotional state:
  - `Patience — Calm`
  - `Patience — Irritated`
  - `Patience — Angry`
- Danger has no draining Patience bar.
- Player damage reduces Patience using the normal damage values of melee, ranged, charged attacks, and applicable damage boosts.
- Patience never regenerates.
- Emptying Patience ends the current emotional phase; it does not mean Mother is physically injured or killed.

Patience capacities:

- Calm: `2.5x` black/white boss health reference.
- Irritated: `2.5x` black/white boss health reference.
- Angry: `3x` black/white boss health reference.

Color direction:

- Calm: pale cyan / blue-white.
- Irritated: amber / yellow-orange.
- Angry: red.
- Danger: pulsing dark red state indicator without a draining bar.

## 2. Phase transitions

When Patience reaches zero:

- a dedicated Mother transition animation plays;
- Mother becomes temporarily invulnerable;
- player movement remains enabled;
- all active player and boss projectiles are cleared safely;
- the room geometry and lighting do not change;
- the transition itself provides a short combat pause;
- the player restores `10%` of maximum health, capped at maximum health;
- no automatic ammunition grant is given;
- after the animation, the next Patience bar appears full.

Transition sequence:

- Calm zero -> animation -> Irritated.
- Irritated zero -> stronger animation -> Angry.
- Angry zero -> animation -> combat ends -> Danger begins.

## 3. Attack interaction rules

### Broom

- Physical attack.
- Parryable.
- Side sweep and overhead strike use distinct telegraphs and hit volumes.

### Window-cleaner spray

- Three projectiles in a close-range fan.
- Dodge i-frames can avoid the projectiles.

### Shoes and slippers

- Cannot be destroyed, shot, or cut.
- Must be avoided.
- Their hit area remains slightly larger than a normal bullet.

### Attraction buildup

Jump and dodge reduce buildup but do not reset it completely.

Implementation values:

- jump reduces current attraction buildup by `35%`;
- dodge reduces current attraction buildup by `60%`;
- values clamp at zero;
- repeated movement without another jump/dodge resumes buildup from the reduced value.

### Father charge

- Cannot be parried.
- Must be avoided.
- Deals high damage on collision.

### Stuns

- Pinning beam stun: `1.75s`.
- Scream stun: `0.75s`.
- Scream interrupts the player’s current attack.
- After any stun ends, the player receives `1.25s` of stun resistance.
- Stun resistance prevents another stun but does not prevent normal damage or knockback.
- The scheduler cannot chain a stun into unavoidable Father, broom, attraction, or lethal bullet-hell damage.

### Mother dodge

Mother’s dodge uses the same core rules as the player’s dodge:

- same dodge distance;
- same dodge duration;
- same i-frame window;
- same world-collision behavior;
- cannot pass through walls, solid furniture, or invalid terrain;
- can evade eligible attack hitboxes during i-frames;
- encounter AI controls how often it may be selected so it cannot remove every punish window.

## 4. Foreground clothing

Clothing cannot be attacked or destroyed.

It is controlled by a phase-aware background event scheduler, independent of the main attack scheduler.

### Calm

- event interval: `14–20s`;
- `2` items visible;
- each item obstructs for approximately `0.8–1.2s`.

### Irritated

- event interval: `10–16s`;
- `2–3` items visible;
- each item obstructs for approximately `1.0–1.5s`.

### Angry

- event interval: `7–12s`;
- `3–4` items visible;
- each item obstructs for approximately `1.2–1.8s`.

### Danger

- clothing obstruction events are disabled so task readability remains clear.

Global rules:

- minimum `2`, maximum `4` visible while an event is active;
- never cover the whole screen;
- never hide the player, Mother, a critical telegraph, and the only safe route at the same time;
- never hide task targets in Danger because clothing is disabled there.

## 5. Combat escalation

### Calm

Core attacks:

- broom sweep or overhead;
- cleaner fan;
- simple footwear patterns;
- stationary attraction with eight radial lanes;
- occasional player-style dodge.

Patience thresholds:

- below `50%`: slightly shorter recovery and occasional two-action sequence;
- below `25%`: denser footwear or one additional broom follow-up;
- no Father, pinning beam, or scream.

### Irritated

Core attacks:

- faster broom chains;
- cleaner fan after dodge or recovery;
- denser footwear patterns;
- Father;
- pinning beam;
- scream;
- more frequent dodge.

Patience thresholds:

- above `50%`: stationary attraction and eight radial lanes;
- below `50%`: rotating attraction and spiral begin;
- below `25%`: faster spiral rotation and limited two-mechanic combinations.

### Angry

Core attacks:

- full attack kit;
- spiral attraction from the first use;
- stronger pull accumulation;
- more complex footwear patterns;
- faster broom chains;
- more frequent Father, pinning beam, scream, cleaner, and dodge.

Patience thresholds:

- `100–66%`: full-kit pressure with readable recovery;
- `66–33%`: shorter gaps, faster Father warning, denser footwear;
- `33–15%`: up to three sequential actions, but not three simultaneously damaging attacks;
- below `15%`: semi-scripted desperation sequence with guaranteed safe solutions.

Recommended desperation sequence:

1. telegraphed scream;
2. side dodge;
3. short fast attraction spiral;
4. Father crosses the room;
5. overhead broom strike;
6. clear extended punish window.

## 6. Danger phase

Danger is not a combat phase.

### Mother movement

- Mother follows one fixed, visible path from the bedroom door to the Game Boy.
- Her physical progress is the timer.
- She cannot be damaged, parried, stunned, or slowed by attacks.
- Player attacks pass through/do nothing to her during Danger.
- Completing one task group pauses Mother for `0.8s`, then she resumes walking.

### Task structure

Each run selects exactly `4` task groups from the approved pool.

Selection rules:

- tasks change between runs;
- tasks do not need to be completed in a fixed order;
- include at least one cleanup/pickup group;
- include at least one destruction/combat group;
- include at least one movement/navigation group;
- the fourth group may come from any compatible category;
- each group may contain several linked actions, such as cleaning three dirt spots or collecting four clothing items.

Approved task groups:

- hit or shoot dirt/dust targets;
- break light clutter piles/blockers;
- shoot high stains/cobweb targets;
- collect toys/clothes by touch and auto-deliver them through a basket/box zone;
- defeat `2–3` light enemies;
- clear a small set of tidy spots by movement;
- navigate furniture and open/reach the bed route.

### Damage and hazards

- Player does not lose health during Danger.
- Environmental lava, holes, and chasms are absent.
- Light enemies do not deal health damage.
- Every successful enemy hit applies knockback to delay the player.
- The player cannot die during Danger; failure is only the Game Boy race condition.

### Bed requirement

- Reaching the bed before all four task groups are complete does not count.
- After all four groups are complete, the final bed-side destination becomes active.
- Success requires entering that destination before Mother finishes taking the Game Boy.

### Failure condition

- Mother reaching the Game Boy begins a short pickup animation.
- Failure occurs only when the pickup animation completes and the Game Boy is in Mother’s hand.
- This animation is the final visible grace window.

### Success sequence

1. all four task groups complete;
2. player reaches the active destination beside the bed;
3. Mother stops;
4. Mother turns and leaves through the door;
5. player takes the Game Boy;
6. the agreed colorful-light ending plays;
7. Mother does not re-enter at the end.

## 7. Defeat and restart

- A short defeat animation plays first.
- Then the normal bed cutscene presentation is used.
- `I'm bored` appears in the same format as the other ending cutscenes.
- The full run restarts from the beginning.
- A new procedural seed is generated.
- All run collectibles and run-only boosts reset.
- There is no Mother Boss checkpoint.

## 8. Arena layout and presentation

The arena is a large transformed bedroom that already contains the combined visual language of all Mother phases from the start.

It does not structurally or visually transform between Calm, Irritated, and Angry.

Allowed escalation comes from:

- Mother’s animations and posture;
- attack density;
- foreground clothing frequency;
- audio intensity;
- Patience UI state.

The room geometry and lighting remain stable across transitions.

### Object placement

- bedroom door: upper/back wall;
- Mother begins near the door;
- bed: opposite side of the room;
- Game Boy: hidden during Calm, Irritated, and Angry;
- at Danger start, the Game Boy is revealed on a low bedside table beside the bed;
- Mother’s path runs visibly from the door toward that table;
- player’s final destination is on the safe side of the bed, outside Mother’s direct path;
- basket/box: central-left area, reachable from all generated pickup positions;
- task objects are distributed across the room without blocking Mother’s path or making a task impossible.

There are no lava pools, holes, or chasms in the Mother arena.

Furniture and task objects provide the only navigation obstacles.

## 9. Remaining unresolved decision

- Is the horse present in the Mother Boss arena, or removed before the encounter?

No other V2 design question remains unresolved.
