# Horse Clean Run Start and Local-Threat Flee

## Problem

A newly loaded run could show the horse taking damage and fleeing even when no
enemy was present in the player’s room.

The root behavior was not one single health value. Two flee components treated
any unfinished `BDRoomEncounter` anywhere in the loaded scene as active danger.
That allowed remote enemies and test encounters to command the horse to a safe
spot. Startup damage/hazard callbacks also needed one coordinated reset before
AI updates.

## Required behavior

On every fresh gameplay scene load, including death -> `New Game`:

- the horse starts at full health;
- recent-hit, buck, healing-lock, fainted, movement, external-control, combat,
  flee, and hazard-retreat state are reset;
- the horse remains calm for `2.50s`;
- startup maintenance runs for `0.55s` so `Awake`, `OnEnable`, and `Start`
  callbacks cannot restore stale flee state;
- startup damage is rejected by the existing health protection;
- hazard polling and retreat are delayed through the calm window;
- a safe-spot command during startup is ignored;
- remote unfinished encounters do not count as danger;
- flee begins only when a living combatant is actually within the horse/player
  combat-awareness radius after the calm window.

## Preservation

This change does not remove combat flee. Once startup calm ends, a real nearby
enemy still causes normal safe-spot selection and flee behavior.

It does not change:

- maximum horse health;
- healing amount or curve;
- buck hit count;
- safe-spot scoring;
- mounted controls;
- exhausted follow;
- Pet interaction;
- hazard damage rules;
- enemy AI or encounter activation.

## Play Mode verification

1. Start a new game and do not move.
2. Confirm the horse health bar is full.
3. Confirm no horse-damage log, panic movement, safe-spot run, or flee occurs.
4. Confirm remote enemies in other rooms do not move the horse.
5. Wait beyond `2.50s`, approach a real enemy, and confirm the horse can flee.
6. Damage and heal the horse normally.
7. Die, choose `New Game`, and repeat the clean-start checks twice.
8. Run `Boredom And Dungeons -> TEST EVERYTHING`.
