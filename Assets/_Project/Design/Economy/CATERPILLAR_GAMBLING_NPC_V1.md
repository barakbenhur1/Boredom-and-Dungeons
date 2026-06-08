# Caterpillar Gambling NPC V1

## Status

`REQUIRED / FUTURE / NOT IMPLEMENTED`

This document is the canonical approved requirement for the Caterpillar gambling NPC. It does not mean that the NPC, its games, economy, art, animation, room spawning, or safety behavior currently exist in Runtime.

## Purpose

Add an original caterpillar NPC with a strange, colorful, surreal fantasy atmosphere compatible with the visual language of Boredom & Dungeons. The feeling may evoke whimsical dreamlike fantasy, but the design must remain original and must not copy a branded character, costume, silhouette, scene, or layout.

The Caterpillar provides optional gambling interactions using the run's money system while remaining finite, configurable, extensible, and resistant to repeated farming.

## Character contract

1. The NPC is a caterpillar.
2. It may have several configurable color variants.
3. Color variants do not currently imply different powers, odds, games, personalities, rewards, or rules.
4. Do not assign gameplay meaning to a color unless a later approved requirement explicitly does so.
5. The Caterpillar is not tied to one fixed position or one fixed color.

## Room eligibility and map placement

1. A Caterpillar does **not** appear in every room.
2. Only rooms selected by the run-generation/content-placement system as Caterpillar rooms are eligible.
3. A room that was not selected for this NPC never creates or reveals one merely because it becomes empty.
4. The number, distribution, region rules, rarity, minimum spacing, maximum count, and exact selection algorithm are still open.
5. No final frequency or spawn probability may be invented before approval.
6. The selected-room identity must remain stable for that run; temporary enemy presence changes visibility, not whether the room was selected.

## Clean-room visibility contract

For an eligible Caterpillar room:

1. The Caterpillar is visible and interactable only when the room is clear of active hostile enemies.
2. When the room becomes clear, the Caterpillar appears through an authored appearance animation.
3. The Caterpillar must not pop into existence without presentation.
4. If active hostile enemies enter, spawn, reactivate, or otherwise make the room unsafe, the Caterpillar disappears through an authored disappearance animation.
5. It must not simply be disabled in one frame unless a technical emergency fallback is required.
6. When the room becomes clear again, the Caterpillar may return through its appearance animation.
7. Appearance/disappearance timing, particles, sound, poses, and exact visual treatment remain open, but the transition must be visibly animated.
8. The clean-room rule applies only to rooms already selected to host the Caterpillar.

## Gambling-session safety contract

1. A gambling session may begin only while the Caterpillar is visible, interactable, and its room is clear.
2. During an active gambling session, hostile enemies must not be able to approach and attack the player.
3. The player must not receive enemy contact, melee, ranged, bomb, charge, jump, trap-placement, or other hostile attack pressure while the session UI/game is active.
4. The safety mechanism must prevent new enemy approach/engagement and must not depend only on hiding health damage after a hit.
5. The exact technical owner is open, but the architecture must use an explicit temporary gambling-session safety state rather than scattered enemy-specific exceptions.
6. The safety state begins only after the session is committed and ends on session completion, cancellation, NPC removal, room unload, run end, player death, or other forced closure.
7. Normal enemy behavior resumes after the safety state ends.
8. Because a session can begin only in a clear room, the expected normal case has no enemies already inside the room. If an invalid enemy state appears during the session, the safety contract still prevents approach and attacks until the session closes safely.
9. The gambling interaction must not become a general combat pause exploitable outside the active session.
10. Whether the rest of the world freezes, enemies are held outside a boundary, or room encounter intent is suspended is not yet decided. Only the player-safety outcome is approved.

## Interaction and game assignment

1. The player can approach the Caterpillar and gamble money.
2. Each Caterpillar instance offers exactly one game.
3. One Caterpillar does not present a menu containing every available gambling game.
4. The assigned game comes from a configurable game pool.
5. Current discussed candidates:
   - dice;
   - pick-up sticks;
   - closest to the wall.
6. These candidates are not a final or closed list.
7. Games can be added, removed, replaced, enabled, or disabled later without changing the one-game-per-Caterpillar rule.
8. Do not invent final rules, controls, stake sizes, payout ratios, win probabilities, AI behavior, tie behavior, animation timing, or presentation for these games before approval.

## Caterpillar bankroll

1. Every Caterpillar instance has a limited available bankroll.
2. The Caterpillar is not an infinite source of money.
3. A player cannot remain beside one Caterpillar and generate unlimited money.
4. A player win can pay only what the transaction and the Caterpillar's available bankroll allow.
5. Money won from the player is added to that Caterpillar's bankroll.
6. Each Caterpillar has a normal passive-refill threshold.
7. Passive refill uses both:
   - elapsed time;
   - player room progression.
8. Final formulas, intervals, amounts, ratios, and event cadence are open.
9. Passive refill stops at the normal refill threshold.
10. The refill threshold is **not** an absolute wallet maximum.
11. If the Caterpillar wins money from the player at or above the threshold, the full amount is retained.
12. Player-derived winnings may raise the Caterpillar bankroll above the threshold.
13. Money above the threshold is never clipped, deleted, reset, refunded, or normalized back to the threshold.
14. While bankroll is at or above the threshold, no passive refill is applied.
15. If bankroll later falls below the threshold, passive refill may resume only up to the threshold.

## Configurable data

The system must keep these configurable:

- assigned game;
- game-pool membership;
- color variation;
- starting bankroll;
- passive-refill threshold;
- time-refill policy;
- room-progression refill policy;
- stakes;
- payout rules;
- room eligibility;
- map distribution;
- appearance/disappearance presentation;
- gambling-session safety ownership and behavior.

Temporary test values must be clearly labeled as temporary and must not be presented as approved balance.

## Transaction and exploit prevention

1. Never pay more than the permitted resolved payout and available Caterpillar bankroll.
2. Never pay one win twice.
3. Never add one player loss to the Caterpillar bankroll twice.
4. Never process one time-refill or room-progress event more than once.
5. Passive refill cannot exceed the threshold.
6. Money earned from the player may exceed the threshold.
7. Session start, transaction resolution, payout, bankroll mutation, cancellation, and cleanup require explicit one-time transaction ownership.
8. Closing, room unload, run end, player death, or interruption cannot duplicate or lose an already committed transaction.
9. The gambling-session safety state cannot remain stuck after the interaction ends.

## Animation and presentation requirements

Required animation states include at minimum:

- hidden/dormant state;
- appearance;
- idle/present;
- interaction/session start;
- gambling-game presentation hooks;
- session finish/cancel;
- disappearance.

Exact authored motion is open. Placeholder motion may be used only when clearly documented as temporary.

## Acceptance requirements

The future implementation is accepted only when:

1. Caterpillars can be assigned to selected rooms in different map locations.
2. They do not appear in every room.
3. A non-selected room never creates one only because it is empty.
4. Eligible selected rooms reveal the Caterpillar only when clear.
5. Appearance is animated.
6. Enemy presence causes an animated disappearance.
7. Clearing the room again allows an animated return.
8. Several configurable color variants exist without invented gameplay meaning.
9. Every Caterpillar offers exactly one assigned game.
10. The game pool remains extensible and the current three candidates are not treated as final.
11. Each Caterpillar has a finite bankroll.
12. Passive refill uses time and room progression without double counting.
13. Passive refill never exceeds the threshold.
14. Money won from the player can exceed the threshold and is retained.
15. The player cannot farm unlimited money from one Caterpillar.
16. During an active gambling session, enemies cannot approach or attack the player.
17. Session safety always cleans up correctly.
18. No unapproved final rules, numbers, odds, frequencies, color meaning, game logic, or economy balance are introduced.

## Open decisions

The following require later design approval:

- exact art/model and color palette;
- eligible-room count and rarity;
- region and route distribution;
- selection algorithm and spacing;
- exact appearance/disappearance animation and audio;
- exact game list;
- full rules for every game;
- stakes and payout curves;
- initial bankroll;
- refill threshold;
- elapsed-time refill rate;
- room-progress refill rate;
- save/resume behavior for Caterpillar state;
- whether different Caterpillar instances share or own independent bankroll;
- exact enemy-safety mechanism and world-pause scope;
- UX, camera, input, accessibility, mobile controls, and cancellation behavior.

## Protected meaning

Do not:

- place a Caterpillar in every room;
- reveal one in an unselected room merely because it is clear;
- keep it visible while active hostiles occupy the room;
- remove it without an animation in normal operation;
- let enemies attack the player during an active gambling session;
- turn the passive-refill threshold into an absolute maximum;
- passively refill above the threshold;
- prevent player-derived winnings from exceeding the threshold;
- let one Caterpillar offer several games at once;
- freeze the current candidate game list as final;
- invent missing rules, probabilities, values, frequencies, or color meanings.
