# Boredom & Dungeons — Exhausted Horse Follow and Pet Interaction

Status: IMPLEMENTED IN LOCAL PACKAGE / VERIFY, 2026-06-06.

## 1. Zero-health horse behavior

The horse reaching zero health does not kill or permanently remove it.

It keeps the current zero-health/fainted/exhausted behavior already used by the game, with one new exception:

- while the player remains nearby, the horse behaves exactly as it currently does at zero health;
- if the player moves genuinely far away, the horse gets up or enters an exhausted-follow state and moves toward the player very slowly;
- this movement exists only to prevent permanent separation and soft-locks;
- it does not restore health;
- it does not return the horse to normal follow, combat, flee, mounted, or healthy behavior.

### Working distance thresholds

Initial implementation values, subject to Play Mode tuning:

- begin exhausted follow when distance from player is greater than `14m` for at least `1.25s`;
- stop exhausted follow when distance is `8m` or less;
- use hysteresis so the horse does not repeatedly start/stop near one threshold;
- if the player teleports, changes room, or moves through an inaccessible connection, use the existing safe reposition fallback rather than letting the horse walk through invalid geometry.

### Working movement values

- exhausted-follow speed: approximately `20%` of normal horse follow speed;
- no sprinting, charging, jumping, fleeing, or mounted acceleration;
- slow turn rate and visibly exhausted movement animation;
- obstacle avoidance remains active;
- hole, chasm, and lava avoidance remains active;
- the horse cannot enter boss barriers, post-boss doors, the ending room, or the Mother Boss arena.

### Zero-health restrictions

While health is zero:

- mounting is disabled;
- mounted shooting is disabled;
- combat flee is disabled;
- the horse cannot attack or provide combat utility;
- normal follow AI cannot override exhausted follow;
- exhausted follow cannot restore or modify health;
- healing restores the horse to the appropriate existing recovered state and ends exhausted follow.

## 2. Pet button visibility

A contextual `Pet` button appears when all required conditions are true:

- player is on foot;
- player is close to the horse;
- distance is at most `2.25m`;
- there is a clear interaction line or valid nearby interaction point;
- neither actor is in a non-interruptible transition;
- the player is not attacking, dodging, stunned, mounted, entering a door, or in a cutscene;
- the horse is not inside an active flee, buck, damage, teleport, or doorway-transition animation.

The button hides immediately when the player leaves the interaction range or a blocking state begins.

The interaction is available whether the horse is healthy, damaged, or at zero health, provided the animation can play safely. Petting does not heal or change gameplay stats unless a later requirement explicitly adds that effect.

## 3. Short press — player pets horse

A short press on `Pet` makes the player pet the horse.

Input rule:

- press and release before the long-press threshold;
- working long-press threshold: `0.65s`.

Sequence:

1. lock the interaction pair to validated facing/spacing points;
2. stop normal movement for the short animation;
3. player turns toward the horse;
4. horse turns its head/body only as needed;
5. player performs the pet animation;
6. horse performs a small positive response animation;
7. restore normal control and AI state.

The animation must not move either actor into hazards, walls, props, enemies, or door triggers.

## 4. Long press — horse pets player

Holding `Pet` for at least `0.65s` triggers the reverse interaction: the horse pets the player.

The horse does this through an animal-appropriate gesture such as:

- head nuzzle;
- muzzle rub;
- gentle head press against the player;
- neck/face affection animation.

It must not look like the horse has human hands or performs the player’s pet animation.

Sequence:

1. show visible long-press progress on the button;
2. once the threshold is reached, commit the long-press interaction;
3. align player and horse at safe interaction points;
4. horse nuzzles/pets the player;
5. player reacts positively;
6. restore normal control and AI state.

Releasing after the threshold must not also trigger the short-press animation.

## 5. Input and cancellation rules

- short press and long press are mutually exclusive;
- long-press progress resets if the finger/button is released before `0.65s`;
- cancel before animation commit if player moves out of range, attacks, dodges, mounts, takes a hard interrupt, or a cutscene/door transition starts;
- once the animation has safely committed, ordinary movement input is ignored until its brief non-interruptible section ends;
- serious damage, death, forced teleport, or invalid geometry may emergency-cancel the animation;
- cancellation restores player input and the horse’s previous legal AI state;
- no duplicate interaction may start while one is active.

## 6. UI behavior

- label: `Pet`;
- mobile: normal tap for player-pets-horse, press-and-hold for horse-pets-player;
- desktop debug input must expose equivalent short/long input behavior;
- long press shows a small progress ring/fill so the second interaction is discoverable;
- optional one-time tooltip may explain `Hold: horse pets you`, but it must be dismissible and must not repeatedly interrupt play;
- the button must not overlap mount, attack, dodge, or other contextual controls;
- when both Mount and Pet are available, both actions must remain clearly distinguishable.

## 7. Relationship to combat and progression

- Pet interactions are primarily expressive and do not restore horse or player health;
- no score, collectible, hidden objective, mandatory progression, or combat advantage is attached by default;
- the button may remain hidden during active combat to avoid accidental interaction and control clutter;
- zero-health exhausted follow is a navigation safety behavior, not a healing mechanic.

## 8. QA requirements

### Exhausted follow

- verify current zero-health behavior remains unchanged while player is nearby;
- verify exhausted follow begins only after the player is genuinely far away;
- verify very slow movement toward the player;
- verify stop distance and threshold hysteresis;
- verify no health restoration;
- verify no mounting, sprint, flee, combat, or boss-door entry at zero health;
- verify obstacle and hazard avoidance;
- verify safe fallback across rooms and invalid paths;
- verify healing exits exhausted follow correctly.

### Pet interaction

- verify button appears only within valid range and on foot;
- verify short press triggers only player-pets-horse;
- verify long press triggers only horse-nuzzles-player;
- verify release after threshold does not fire both actions;
- verify interaction with healthy, damaged, and zero-health horse;
- verify no healing/stat changes;
- verify cancellation for distance, combat, dodge, mount, damage, cutscene, and doorway transition;
- verify no overlap with hazards, walls, props, enemies, or controls;
- verify mobile and desktop debug input parity;
- verify no duplicate interaction or stuck player/horse state.


## 9. Current implementation state

- The current desktop default for Pet is `Tab`. Existing serialized legacy `P` values migrate to `Tab` at runtime, and the world cue plus on-screen Pet button display the active binding rather than a hard-coded key.

- One runtime component owns both zero-health exhausted follow and contextual Pet interaction.
- Exhausted follow begins only after the player remains beyond `14m` for `1.25s`, stops at `8m`, and moves at `20%` of the reference normal follow speed.
- Movement uses the existing horse hazard filter. A safe fallback near the player is attempted only after the horse remains stuck while still far away.
- Normal horse, flee, mount, jump, and combat control are explicitly locked while exhausted follow or Pet owns movement.
- Short press and long press are mutually exclusive. The long-press threshold remains `0.65s`.
- Horse interaction cues use a fixed non-overlapping stack above the horse HUD: healing occupies the lower-left screen-relative slot and Pet occupies the higher-right screen-relative slot. Both use a small toward-camera depth offset and separate sorting orders, and may never cover the horse name/health bar or one another. The amber Pet cue shows long-press progress and changes to `PETTING` during the interaction.
- When key rebinding is implemented, every keyboard label in this interaction must read the live binding display string instead of a hard-coded `P` or `F`.
- Pet interaction temporarily disables player movement/combat input, restores the exact previous enabled states, and emergency-cancels on serious damage, invalid separation, death, or hazard recovery.
- Neither Pet nor exhausted follow calls healing, damage, maximum-health, score, collectible, or progression APIs.
- The prototype scene installer serializes the component; runtime repair remains an idempotent fallback for older scenes.
- Automated checks are integrated into the existing `TEST EVERYTHING` command.
- Unity compilation, automated QA, and the full Play Mode matrix remain required before DONE.
