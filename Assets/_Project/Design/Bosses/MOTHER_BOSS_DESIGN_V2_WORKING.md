# Boredom & Dungeons — Mother Boss Design V2 (Working)

Status: working design discussion, 2026-06-06.

This file records the confirmed patience-bar clarification and all remaining open design decisions. It does not replace `MOTHER_BOSS_DESIGN_V1.md` until the open decisions are approved.

## Confirmed resource model

Mother does not have a normal health bar.

The visible combat resource is **Patience**.

- Player damage reduces Mother’s Patience.
- Emptying the Patience bar ends the current combat phase.
- Emptying Patience does not mean Mother is physically injured or killed.
- At each transition, the bar refills for the next emotional state.
- The displayed state changes with the phase:
  1. Calm
  2. Irritated
  3. Angry
  4. Danger
- Phase 4 has no Patience bar because it is not a combat phase.

Internal Patience capacity:

- Calm: equivalent to `2.5x` the black/white boss health reference.
- Irritated: equivalent to `2.5x` the black/white boss health reference.
- Angry: equivalent to `3x` the black/white boss health reference.

The bar should visually read as patience/frustration depletion rather than blood, wounds, or ordinary HP.

## Proposed phase-transition presentation

Working proposal:

- Calm reaches zero Patience -> short transition -> Irritated Patience refills.
- Irritated reaches zero Patience -> stronger transition -> Angry Patience refills.
- Angry reaches zero Patience -> combat stops -> Danger task phase begins.
- Mother is temporarily protected during transitions.
- Existing projectiles and unsafe overlapping attacks are cleared or resolved before control returns.

## Proposed escalation structure

### Phase 1 — Calm

Purpose: teach the full core language of the boss one pattern at a time.

- Broom sweep or overhead, never both in an unreadable chain.
- Three-shot cleaner fan at close range.
- Introductory shoes/slippers patterns with large safe gaps.
- Stationary attraction field with eight radial lanes.
- Occasional dodge with long recovery.
- No Father, pinning beam, or scream unless later approved otherwise.

Suggested low-Patience escalation inside Phase 1:

- below 50% Patience: slightly shorter recovery, occasional two-attack sequence;
- below 25% Patience: denser footwear pattern or one additional broom follow-up, but no new mechanic.

### Phase 2 — Irritated

Purpose: combine already learned mechanics and introduce control attacks.

- Faster broom chains.
- Cleaner fan can be used after a dodge or broom recovery.
- Denser footwear patterns.
- Father charge introduced.
- Pinning beam introduced.
- Scream introduced.
- More frequent directional dodge.
- First half of Patience: stationary attraction with radial lanes.
- Below 50% Patience: rotating attraction and spiral projectiles begin.

Suggested low-Patience escalation inside Phase 2:

- below 50%: spiral begins, Father can enter from more directions;
- below 25%: faster spiral rotation and limited two-mechanic combinations;
- no stun attack may lead directly into unavoidable high damage.

### Phase 3 — Angry

Purpose: mastery test using the full kit.

- Spiral attraction is active from the first use.
- Faster pull accumulation and rotation.
- More complex footwear bullet hell.
- Faster broom combinations.
- Father, pinning beam, scream, cleaner spray, and dodge are more frequent.
- Controlled combinations are allowed.

Suggested low-Patience escalation inside Phase 3:

- below 66%: normal full-kit pressure;
- below 33%: shorter gaps between patterns, faster Father warnings, denser footwear patterns;
- below 15%: final desperation sequence with clearly telegraphed combinations and guaranteed safe solutions.

## Phase 4 — Danger working structure

Mother stops fighting and walks toward the Game Boy.

Her physical progress is the timer.

The player must:

1. complete a selected set of room tasks using existing movement, melee, shooting, pickup, and combat mechanics;
2. reach the required safe position beside the bed;
3. do both before Mother reaches the Game Boy.

Working task pool:

- hit or shoot dirt/dust targets;
- break light clutter piles;
- shoot high stains or cobweb targets;
- collect scattered toys/clothes by touch and deliver them through a basket/box zone;
- defeat 2–3 light enemies;
- clear a small ordered set of tidy spots by movement;
- navigate furniture and hazards to reach the bed endpoint.

Working recommendation:

- use 3 task groups per run;
- at least one movement task;
- at least one combat or destruction task;
- at least one pickup/cleanup task;
- then the final bed-position requirement.

## Open decisions

### Patience bar

1. Should each combat phase have a completely fresh full-width Patience bar, or should one long bar be divided into three labeled segments?
2. Should the bar visibly say `Patience`, or show only the emotional state name?
3. What colors should represent Calm, Irritated, Angry, and Danger?
4. Should ordinary attacks reduce Patience equally, or should specific attacks/actions annoy Mother more?
5. Can Patience regenerate when the player avoids attacking for too long?

### Phase transitions

6. What exactly happens when a Patience bar reaches zero?
7. Does Mother speak, animate, change posture, change lighting, or alter the room?
8. Can the player move during transitions?
9. Are player projectiles cleared at transition?
10. Does the player receive a short recovery/healing/ammo opportunity between phases?

### Attack rules

11. Is broom damage physical and parryable?
12. Can the cleaner projectiles be dodged through with i-frames?
13. Can shoes/slippers be destroyed by shooting or melee, or only avoided?
14. Does jumping fully reset attraction buildup immediately, or only reduce it?
15. Does dodge fully reset attraction buildup?
16. Can the player parry Father, or must Father always be avoided?
17. How long is the pinning-beam stun relative to the scream stun?
18. Does the scream interrupt attacks already in progress?
19. Does Mother’s dodge have i-frames?
20. Can Mother dodge through the player or hazards?

### Foreground clothing

21. Are clothing-obstruction events random, tied to low Patience, or caused by a specific attack?
22. Can the player interact with or destroy the clothing?
23. How long may one item cover part of the view?
24. Should clothing frequency increase by phase?

### Phase 4

25. Is Mother’s route to the Game Boy fixed and visible from the start?
26. Can the player slow Mother, or is her movement completely unstoppable?
27. How many tasks should appear in one run?
28. Are tasks randomized or always the same final challenge?
29. Must tasks be completed in order?
30. Does the player still have health and take damage in Phase 4?
31. Do environmental hazards remain active in the room?
32. Can the 2–3 light enemies damage or delay the player normally?
33. Does reaching the bed before completing all tasks count, or must tasks be completed first?
34. What exact event counts as Mother taking the Game Boy?
35. What happens visually on Phase-4 success before the colorful-light ending?

### Failure and restart

36. On defeat, does the bed cutscene happen immediately or after a short defeat animation?
37. Does `I'm bored` appear as speech bubble, voice, text, or all three?
38. Does restart mean a completely new procedural seed and zero run items/boosts?
39. Is there any checkpoint before the Mother Boss, or must the whole run always restart?

### Arena and presentation

40. Is the Mother fight in the same bedroom/ending room or a transformed version of it?
41. Does the room change between Calm, Irritated, Angry, and Danger?
42. Where is the Game Boy placed during the fight?
43. Where are the bed, door, basket/box, and task objects placed?
44. Is the horse present, absent, or removed before this encounter?
45. Can holes/lava appear in the Mother arena, or should the arena use only furniture/task hazards?

## Safety/fairness constraints already accepted

- No long-stun chain into unavoidable Father, scream, attraction, or lethal bullet pattern.
- Short stun resistance after recovery.
- Distinct telegraphs for all major attacks.
- At least one reachable safe solution during every pattern.
- Clothing cannot hide the only safe solution.
- Mother’s dodge cannot remove every punish window.
- Task generation cannot produce an impossible Phase 4.
