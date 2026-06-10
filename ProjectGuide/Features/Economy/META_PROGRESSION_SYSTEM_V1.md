# Cross-Run Meta Progression System V1

Status: REQUIRED FUTURE SYSTEM — DESIGN AND VISUAL DIRECTION OPEN
Owner categories: C06 Collectibles/Rewards/Economy, C11 UI, C13 Content/Endings, C14 Balance/Save/Release.

## 1. Required product role

The game requires a persistent progression layer between runs. At the end of a run, the player receives meta-progression points based on run progress and performance. These points are spent in a new main-menu area whose final name, visual structure, navigation, and presentation will be decided later.

This is a required system, not an optional idea, but its exact reward formula, catalog, prices, layout, art direction, and pacing remain deliberately open for a dedicated design phase.

## 2. Currency separation

- Meta-progression points are separate from the in-run merchant currency.
- In-run money resets according to the future run-economy contract and cannot silently become meta currency.
- Meta points persist between runs and application launches.
- Awarding, spending, unlocking, saving, loading, migration, and rollback safety must be atomic and testable.

## 3. End-of-run award direction

The final formula remains open, but candidates that must be evaluated include:

- map/room progress;
- enemies, guardians, mini-bosses, and bosses defeated;
- batteries, secrets, objectives, and collectibles found;
- ending or victory state reached;
- damage taken, survival, combat efficiency, or special achievements;
- whether the run was completed, lost, abandoned, interrupted, or recovered after a crash.

The summary screen must eventually explain how the awarded total was calculated. The system must not encourage one dominant repetitive farming strategy.

## 4. Unlock catalog direction

Potential persistent unlocks include, but are not limited to:

- player skins;
- horse skins;
- a new playable character;
- a new boss;
- additional content, options, encounters, or cosmetic rewards defined later.

Each unlock costs a configured number of points based on desirability, rarity, production value, and gameplay impact. Exact items and costs remain unapproved until the dedicated planning phase.

## 5. Mandatory design safeguards

- Permanent unlocks are granted exactly once and remain owned after restart/update.
- Locked content is communicated clearly without misleading the player.
- Gameplay-changing unlocks require explicit balance and run-generation integration; they cannot be treated as cosmetic data.
- The system must not erase the importance of player skill or in-run decisions.
- It must not become pay-to-win.
- New characters, bosses, and mechanically significant unlocks require their own content, save, QA, animation, audio, balance, and compatibility contracts.

## 6. Open decisions requiring future approval

- final feature/menu name;
- menu location, UX, visual design, Game Boy integration, and mobile-landscape layout;
- exact end-of-run point formula and caps;
- catalog contents and point prices;
- linear list, tiers, tracks, tree, collections, or another progression structure;
- cosmetic versus gameplay-changing split;
- duplicate handling and completion-state presentation;
- abandon/crash/partial-run award policy;
- save schema, versioning, migration, backup, corruption recovery, and platform synchronization;
- anti-exploit and farming protections;
- analytics and balancing methodology.

## 7. Future acceptance gate

1. End-of-run points are calculated deterministically from an approved formula and displayed with an understandable breakdown.
2. Meta points persist safely and remain completely separate from run-shop currency.
3. Purchases/unlocks are atomic, cannot duplicate or create negative balances, and survive reload/version migration.
4. Every unlocked item integrates with its actual owning systems and does not exist as UI-only state.
5. Abandon, defeat, victory, crash recovery, new run, and clean-install/migration cases match the approved policy.
6. TEST EVERYTHING, save-data validation, focused Play Mode, and platform testing cover earning, spending, migration, and content activation.

## 8. Latest approved temporary implementation backlog

Status: `REQUIRED / FUTURE / NOT IMPLEMENTED`.

The latest Girl/Father correction specification additionally requires a temporary vertical slice before the final economy formula is designed:

- one centrally configured positive integer reward used after defeat and final victory;
- award exactly once per run after the relevant death/ending cinematic;
- persistent `TotalMetaPoints` as the single source of truth;
- a professional reward summary screen showing previous balance, awarded points and new balance;
- a main-menu balance display and a `Meta Progression / התקדמות מטא` button;
- an expandable but currently non-spending Meta Progression screen with correct Back navigation;
- explicit backlog work for the final reward formula, Girl unlock price, Father unlock price, anti-farming simulation and final visual design.

No temporary reward value, save schema or UI is implemented by V23R19J. The value must be selected and reported when this backlog item becomes active.

<!-- BND_FIRST_LAUNCH_TUTORIAL_PRODUCTION_COURSE_V10:BEGIN -->
## Approved Abandon award policy

The broader defeat/victory formula remains open, but Abandon now has an exact relationship to the eventual/current death evaluator: evaluate the points that death would award at that exact run state, multiply by `0.84`, then apply the evaluator's existing rounding/clamp policy. Use the same evaluator and one idempotent award transaction; do not duplicate the death formula.

The shared meta-result screen appears before the gameplay-to-handheld exit animation. Abandon cleanup and the animation begin only after the player legally closes that result screen. Ordinary Save & Return grants no end-of-run points.
<!-- BND_FIRST_LAUNCH_TUTORIAL_PRODUCTION_COURSE_V10:END -->
