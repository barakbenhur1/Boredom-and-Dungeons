<!-- BND_CHILD_APPROACH_CINEMATIC_PATH_CLEARANCE_V10929:BEGIN -->
## V10.9.29 path-clearance addendum
The child begins well behind the chair and clearly left of its centerline at the previously accepted POV height. The child does not climb through the backrest. The route approaches the left-rear corner, climbs outside the chair footprint, advances to a seat-side point after clearing the backrest, and then settles inward over the seat.
<!-- BND_CHILD_APPROACH_CINEMATIC_PATH_CLEARANCE_V10929:END -->

<!-- BND_CHILD_APPROACH_CINEMATIC_POLISH_V10928:BEGIN -->
## V10.9.28 direction addendum
The first visible camera is behind the chair, slightly left, at the raised POV used by the earlier cinematic rather than near-floor height. The chair faces the table, with its backrest toward the approaching child. The power-on is two-stage: physical glass/backlight wake, then delayed tutorial-content feed-in with fade, upward settle and micro-scale correction.
<!-- BND_CHILD_APPROACH_CINEMATIC_POLISH_V10928:END -->

<!-- BND_CHILD_APPROACH_CINEMATIC_V10927:BEGIN -->
## Child point-of-view room entry addendum — V10.9.27

The tutorial entry is one uninterrupted physical-camera shot. The child approaches from behind the chair, climbs onto it, leans toward the handheld, settles at the final tutorial composition, then powers on the display. The tutorial UI may exist in memory before that moment but must not be visible or emissive until the authored power-on begins.

Screen-off means: screen canvas inactive, screen camera disabled, page alpha zero, display color near-black, emission/brightness zero, scanlines and hit targets disabled. The final reveal uses dark shutters and a restrained blue electronic line; no white full-screen flash is allowed.
<!-- BND_CHILD_APPROACH_CINEMATIC_V10927:END -->

# First-Launch Tutorial Entry Choice and Animation Direction V11

Status: `ENTRY CHOICE IMPLEMENTED IN V10.3 / TUTORIAL ANIMATION PRODUCTION PASS ACTIVE / MAIN-GAME ANIMATION PASS QUEUED`

## 1. Product contract

When the durable first-launch tutorial state is `NotStarted` or `InProgress`, the modern handheld owns the first visible post-boot frame. Before tutorial gameplay begins, the handheld screen presents a dedicated pixel-style choice:

- `B&D`
- `Boredom & Dungeons`
- `PLAY TUTORIAL`
- `SKIP TUTORIAL`

The choice screen is black with white pixel-style text. It must not expose the legacy menu, a temporary page, a stale render target, or tutorial gameplay behind it.

`PLAY TUTORIAL` writes `InProgress`, performs a short controlled transition, then reveals the tutorial course and unlocks input.

`SKIP TUTORIAL` writes the same terminal no-auto-replay decision as completing or abandoning the tutorial. It transitions to the normal main menu without exposing an intermediate legacy screen.

The screen is shown only while the durable tutorial state still requires presentation. Completion or skip survives restart and normal scene reloads.

## 2. Input contract

The entry choice uses semantic navigation rather than hard-coded displayed labels:

- up/down changes the selected option;
- the existing confirm action accepts it;
- keyboard, gamepad, pointer/touch and physical handheld controls use the active project mapping;
- hover/click targets match the visible options;
- navigation and confirmation provide restrained visual and audio feedback.

The implementation may not invent a gameplay binding or reuse a gameplay action label incorrectly.

## 3. Launch-frame ownership

The modern handheld presenter is installed before scene load and reserves presentation while the BBH boot intro is active or while the first-launch tutorial decision is pending.

The reservation is released only after:

1. the boot intro has completed;
2. the authoritative `BDMainMenuFlow` has resolved; and
3. the correct modern handheld page is ready.

The legacy menu suppression decision must not require a visible legacy frame before the presenter becomes ready.

## 4. Tutorial respawn clarity

A tutorial death, fall or checkpoint restore must not look like an unexplained teleport. The existing V10.2 sequence remains mandatory:

1. loss of control;
2. readable hit/death response;
3. character fade;
4. opaque checkpoint cover;
5. restore while covered;
6. fade back in;
7. input restoration after cleanup.

## 5. Tutorial animation production pass — active remaining work

The tutorial still requires a dedicated production animation pass. It is not considered complete merely because the requirement is documented.

### Player

Required states include:

- Idle, Start Move, Walk/Run, Stop, Turn;
- Jump, Fall, Land;
- Dodge;
- Light, Heavy, Spin, Grapple, Ranged, Reload and Parry;
- Hit, Stagger, Knockback;
- Mount, Mounted, Dismount;
- Death, disappear/respawn and recovery.

Arms and legs must visibly participate in the actions. Attack presentation must show preparation, committed motion, impact and recovery. Locomotion must stop during incompatible actions and may not create visible foot sliding.

### Horse

Required states include:

- Idle/breathing;
- Start Move, Walk, faster travel, Stop, Turn;
- Back Step and backward travel;
- mounted travel synchronized with the rider;
- threat reaction and retreat;
- Hit, Fall/Death/Despawn where the tutorial uses those outcomes.

All four legs must animate at a cadence derived from actual movement speed. Forward movement may not be reused backward without a distinct presentation.

### Enemies and mini-boss

Each enemy role requires behavior-specific movement and combat presentation:

- Idle, Patrol/Guard, Walk, Run/Chase;
- Charge/Ram where relevant;
- Attack Preparation, committed attack or shot, follow-through and recovery;
- Hit, Stagger, Knockback, Fall, Death and Despawn;
- mini-boss phase transition, summon and defeat.

A charging enemy may not look like a faster patrol loop. Damage/release timing must be synchronized to the authored attack event, not to an unrelated timer.

## 6. Animation architecture

Animation state must be driven from authoritative gameplay state and explicit action transactions. It may not be implemented as scattered one-off timers.

Required safeguards:

- one presentation owner per character;
- no two incompatible animation states simultaneously;
- no locomotion cycle while stationary or attacking;
- no attack pose retained after cancel, disable, death, reset or scene transition;
- no frame-rate-dependent animation;
- no duplicate Animator controller without a documented ownership reason;
- no placeholder animation represented as final production art;
- no changes to AI, damage, hitboxes or cooldowns solely to fit presentation.

## 7. Main-game animation roadmap — queued, not complete

The main game requires a separate full character and animation production pass. It must explicitly cover:

- adding animated arms/legs to models that lack them;
- rig/bones or an equivalent production-ready deformation system;
- full player locomotion, combat, ranged, ability, hit, mount and death sets;
- full horse locomotion, backward motion, reactions, hit and mounted synchronization;
- distinct animation sets for each materially different enemy role;
- synchronization of damage, projectiles, hitboxes, charged actions, AOE, knockback, landing and death;
- prevention of foot sliding and rider/horse desynchronization;
- cleanup on disable, despawn, death and scene transitions;
- performance verification with several enemies active.

This roadmap item remains open until assets, runtime state integration, QA, performance and user acceptance are complete.

## 8. V10.3 verification

Automated checks must verify:

- `PLAY TUTORIAL` and `SKIP TUTORIAL` exist exactly once;
- skip persists and prevents automatic replay;
- play writes `InProgress` only after confirmation;
- entry targets are disabled after selection;
- the presenter install hook uses `BeforeSceneLoad`;
- launch reservation remains active until boot completion and flow resolution;
- successful package application deletes the exact source ZIP from Downloads;
- failure preserves the source ZIP and rollback backup.

Manual Play Mode checks:

1. clean/reset state → BBH intro → tutorial choice, with no legacy-frame flash;
2. Play Tutorial → controlled reveal and correct input timing;
3. Skip Tutorial → correct modern main menu, no tutorial and no legacy-frame flash;
4. restart after skip → direct normal flow;
5. mouse, keyboard, controller and physical handheld navigation;
6. repeat at narrow and wide landscape resolutions;
7. verify source ZIP deletion only after a successful installer result.

<!-- BND_FIRST_LAUNCH_TUTORIAL_PROGRESSION_GATE_REPAIR_V104:BEGIN -->
## V10.4 entry typography correction

The tutorial choice page does not use a smooth system/UI font. `B&D`, `Boredom & Dungeons`, `PLAY TUTORIAL`, `SKIP TUTORIAL` and the status line are rendered from a maintained bitmap glyph table with point filtering and no smoothing.

This typography correction is implemented. The broader player/horse/enemy animation requirements in this document remain active and unverified.
<!-- BND_FIRST_LAUNCH_TUTORIAL_PROGRESSION_GATE_REPAIR_V104:END -->

<!-- BND_INTRO_TO_MAIN_MENU_CINEMATIC_AND_TUTORIAL_SPACING_V105:BEGIN -->
## V10.5 choice composition correction

`B&D` and `Boredom & Dungeons` require an intentional vertical gap. Their bounds may not touch or overlap, and the option stack retains its own readable region. Pixel glyph rendering remains mandatory.
<!-- BND_INTRO_TO_MAIN_MENU_CINEMATIC_AND_TUTORIAL_SPACING_V105:END -->

<!-- BND_FIRST_LAUNCH_TUTORIAL_MECHANICS_REPAIR_V108:BEGIN -->
## V10.8 tutorial locomotion subset

The playable tutorial now includes deterministic point-filtered step-frame pairs for the player, horse and tutorial enemies/mini-boss. Frame selection is driven by actual movement/action translation and stops at idle. This closes the specific “legs do not move” tutorial regression but does not close the full production-animation backlog above or the separate main-game model/rig work.

Ranged and Hook actions also obey impact/completion events: visual projectile impact precedes damage, and visible Hook pull completion precedes target damage/progression. These timing rules are permanent even when final authored animation assets replace the procedural pixel frames.
<!-- BND_FIRST_LAUNCH_TUTORIAL_MECHANICS_REPAIR_V108:END -->
