# Boredom & Dungeons — Mother Boss Design V1

Status: design recovered and approved from the user’s reconstruction on 2026-06-06.

## Position in the game

The Mother Boss is a secret post-ending encounter. It does not replace the black/white final boss.

Unlock condition:

- Game Boy collected.
- Two Batteries collected.
- Game Cartridge collected.

Sequence:

1. The player reaches the ending with the complete hidden set.
2. The normal colorful light appears on the player’s face.
3. The bedroom door opens.
4. Mother appears only partially: a body fragment, silhouette, shadow, or equivalent limited reveal.
5. The Mother Boss encounter begins.

Loss from any phase:

1. Play a cutscene of the player in bed.
2. The player says `I'm bored`.
3. Reset the run and return to the beginning of the game.

Victory:

1. Play the colorful-light ending.
2. Mother does not enter at the end.
3. End on the colorful light.

## Visual identity

- Large model.
- Ponytail.
- Arms normally extended outward.
- During the scream, the arms extend even farther to the sides.
- Mother can dodge in every direction like the player.

## Resource model

Mother does not show a normal HP bar.

Visible progression:

1. Calm.
2. Irritated.
3. Angry.
4. Danger.

Internal durability:

- Phase 1: `2.5x` the black/white boss health reference.
- Phase 2: `2.5x` the black/white boss health reference.
- Phase 3: `3x` the black/white boss health reference.
- Phase 4: no combat durability; objective-race phase.

The UI should communicate anger-state progress without looking like a normal HP bar.

## Core attacks

### Broom

Mother can produce the broom in every combat phase.

Patterns:

- side-to-side sweep;
- top-to-bottom overhead strike.

Both require readable startup, separate hit volumes, and recovery windows.

### Window-cleaner spray

At relatively close range, Mother fires three normal-damage projectiles in a fan.

The attack is reusable in every combat phase and is controlled by the attack scheduler rather than a one-time limit.

### Shoes and slippers bullet hell

Mother fires complex patterns made from shoes and slippers.

Rules:

- each shoe/slipper has a slightly larger hit area than a normal bullet;
- patterns become more complex in later phases;
- safe gaps must remain readable.

### Full-screen attraction field

Early version:

- Mother stands in place;
- the field pulls the player toward her across the arena;
- continuous walking without jumping or dodging causes progressively stronger slowing;
- after enough time, the player begins to slide/pull backward toward Mother;
- jumping or dodging interrupts/resets the accumulated pull slowdown;
- Mother fires radial rows dividing the circle into eight equal sectors.

From the midpoint of Phase 2 onward:

- Mother rotates;
- the radial rows become a spiral.

Phase 3:

- spiral attraction is available from the beginning;
- pull accumulation and spiral rotation are stronger/faster;
- jump/dodge escape windows remain valid.

### Call Father

Mother calls Father, represented by a large figure running across the screen.

- collision deals high damage;
- entry side and lane require a clear warning;
- at least one reachable avoidance gap must remain.

### Pinning beam

A fast direct laser aimed at the player.

- low damage;
- long stun on hit;
- cannot be chained into unavoidable damage.

### Scream

Mother stands in place and screams in a radius.

- arms move farther out to the sides;
- medium-plus damage;
- short stun;
- readable radius telegraph.

### Dodge

Mother uses the player's core dodge movement contract rather than a separate teleport:

- distance: the player's current core dodge distance (`3.05m` unless the player contract changes globally);
- duration: the player's current core dodge duration (`0.12s`);
- i-frames: the same dodge duration plus the player's current extra invulnerability window (`0.14s`);
- the same wall, obstacle, hazard, doorway, and collision validation applies;
- no negative-scale flip, teleport, path crossing through closed geometry, or dodge into an illegal attack position;
- Mother may start a dodge only from neutral movement or an explicitly dodge-cancellable recovery state;
- she cannot cancel a committed broom active frame, spray release, Father call, pinning-beam fire, scream, attraction channel, anger transition, stun, or defeat state;
- after a dodge, the scheduler re-evaluates distance and line of sight instead of blindly firing the previously selected attack;
- every phase preserves real player punish windows and prevents permanent invulnerability.

#### Phase 1 — Calm dodge budget

- one stored dodge charge;
- charge recovery: `10.0s`;
- maximum: `1` dodge in any rolling `8.0s` window and no more than `3` total before the phase transition;
- used mainly to evade a clearly committed heavy/charged/landing punish or to leave a corner;
- preferred direction is lateral or backward; forward dodge is allowed only when collision-safe and never through the player;
- after the dodge recovery, Mother cannot begin another attack for `0.65s`.

#### Phase 2 — Irritated dodge budget

- two stored dodge charges;
- each charge recovers in `8.0s`;
- maximum: `2` dodges in any rolling `14.0s` window;
- minimum `1.10s` between dodge starts;
- the second dodge is legal only when the first still leaves Mother cornered or the player has committed a new high-value attack;
- after using the second dodge inside the rolling window, expose a minimum `1.00s` punish window before a new attack.

#### Phase 3 — Angry dodge budget

- two stored dodge charges;
- each charge recovers in `6.0s`;
- maximum: `3` dodges in any rolling `14.0s` window;
- no more than two consecutive dodges, with at least `0.75s` between starts;
- a dodge may reposition Mother into a legal follow-up angle, but the follow-up telegraph cannot start until at least `0.35s` after dodge recovery;
- after two consecutive dodges, enforce a `1.20s` dodge lockout and a readable punish window;
- dodge selection is disabled while the full-screen attraction/spiral channel is active.

#### Phase 4 — Danger

- zero combat dodges;
- Mother follows the fixed visible route toward the Game Boy and uses only the approved task-race movement rules.

## Foreground clothing obstruction

During active obstruction events, shirts, trousers, and underwear float between the camera and arena.

Rules:

- minimum 2 and maximum 4 clothing items visible simultaneously while the event is active;
- outside the event, they do not need to remain on screen;
- they may hide visibility briefly or sometimes somewhat longer;
- they obscure only part of the screen, never the entire screen;
- they must never fully hide the player, Mother, a critical telegraph, a hazard edge, or the only safe route at the same time.

## Phase structure

### Phase 1 — Calm

Durability: `2.5x` black/white boss reference.

Attack pool:

- broom sweep and overhead;
- close-range three-shot cleaning spray;
- introductory shoes/slippers patterns;
- stationary attraction field with eight radial sectors;
- Calm dodge budget: one charge, 10s recovery, at most 1 per 8s and 3 total in the phase.

Pacing:

- generous recovery windows;
- simple single-pattern pressure;
- teaches every core mechanic before combinations appear.

### Phase 2 — Irritated

Durability: `2.5x` black/white boss reference.

Attack pool:

- faster broom combinations;
- reusable cleaning spray;
- denser shoes/slippers patterns;
- Father charge;
- pinning beam;
- scream;
- Irritated dodge budget: two charges, 8s recovery each, at most 2 per 14s with 1.10s between starts.

Attraction rule:

- first half: stationary, eight radial sectors;
- from midpoint: Mother rotates and rows become a spiral.

### Phase 3 — Angry

Durability: `3x` black/white boss reference.

Attack pool:

- full kit;
- spiral attraction from phase start;
- stronger pull accumulation and faster spiral rotation;
- more complex shoes/slippers bullet hell;
- faster broom chains;
- more frequent Father, scream, and pinning beam use; Angry dodge budget is two charges, 6s recovery each, at most 3 per 14s and never more than two consecutively.

The scheduler may create limited combinations, but never simultaneous unavoidable overlaps.

### Phase 4 — Danger

This is not a combat phase.

Mother walks toward the Game Boy to take it. Her physical progress is the visible timer.

The player must complete a validated subset of room-cleaning/tasks using existing controls and mechanics, then reach the required spot beside the bed before Mother reaches the Game Boy.

Approved task pool without a new interaction button:

- hit or shoot dirt/dust targets;
- break light clutter piles/blockers with sword or shots;
- shoot hanging stains/cobweb-style targets outside melee reach;
- collect scattered toys/clothes by touching them and auto-deliver them by entering a basket/box zone;
- defeat 2–3 light enemies or mess-themed variants;
- touch/clear a small ordered set of marked tidy spots through movement;
- navigate around existing furniture/hazards and reach the bed endpoint.

Rules:

- use a readable subset of tasks per attempt rather than every type at once;
- task completion is shown through world changes and concise feedback;
- this feedback must not resemble the forbidden hidden-collectible checklist;
- success requires all selected tasks complete and the player reaching the bed safe spot before Mother enters the Game Boy pickup zone;
- failure occurs if Mother takes the Game Boy first;
- failure uses the same bed/`I'm bored` cutscene and full restart;
- task generation must never be impossible because of blocked paths, missing objects, enemies, hazards, or an invalid bed destination.

## Fairness and anti-lock rules

- Long pinning stun cannot be immediately chained into unavoidable Father collision, scream, attraction capture, or lethal bullet pattern.
- After any stun ends, apply short stun resistance before another stun can take effect.
- Father, scream, broom overhead, attraction, and spiral each require distinct telegraphs.
- At least one legal escape option must remain during every attack pattern.
- Foreground clothing cannot hide that only escape option.
- Phase transitions have clear animations/state changes and temporary transition protection.
- Mother’s dodge cannot cancel every player punish window.

## QA requirements

- Unlock only with the complete hidden collectible set.
- Verify all three durability multipliers.
- Verify every attack in every eligible phase.
- Verify Phase 2 midpoint spiral transition.
- Verify all stun-resistance and anti-lock rules.
- Verify 2–4 clothing items while obstruction is active and never full-screen coverage.
- Verify loss from Phase 1, 2, 3, and 4.
- Verify victory ending has no final Mother entrance.
- Verify every Phase-4 task combination is completable.
- Verify clean full-run restart after defeat.
- Complete mobile-landscape readability, balance, performance, and Play Mode QA.
