# BBH Boot Intro V1

## Placement

The boot intro appears once when the application starts from the operating system, before the main menu becomes visible. It does not replay when a new game reloads the gameplay scene during the same application session.

The screen is fully black. `BBH` is centered horizontally and its visual center is exactly `45%` of screen height measured from the top. Runtime constants, not stale Inspector values, own the final placement and timing contract.

## Letter sequence

The letters appear strictly one after another:

1. the first `B` completes its full animation;
2. the second `B` begins only afterward;
3. `H` begins only after the second `B` is complete.

There is no overlapping letter entrance.

Each letter begins as if it is far behind the screen and travels toward the viewer:

- zero or extremely small initial scale;
- increasing opacity and brightness;
- layered depth trail;
- subtle perspective rotation;
- controlled overshoot;
- precise settle into the final plane;
- cool edge highlight and restrained glow.

## First-frame and first-letter timing contract

- The first rendered intro frame is fully black.
- No first `B`, shadow, depth copy, highlight, or rim is visible during the initial pre-roll.
- `FirstLetterStartTime` is strictly greater than zero; default target is `0.20–0.35s`.
- The first `B` begins from zero scale and opacity, and all shadow/depth layers use the same animation progress.
- Inspector or stale serialized timing values may not make the first `B` visible before its start time.

## Completed filled-circle badge

After all three letters have fully settled, a circular badge begins behind the completed `BBH` text:

- it starts at zero size;
- it grows smoothly to its final diameter behind the letters;
- it contains a full graphite/steel interior fill and visible rim, not an empty outline;
- the settled letters remain above it;
- after the circle reaches full size, the completed mark remains for exactly `0.50s`;
- the intro ends only after that hold;
- all timing uses unscaled real time.

## Duration and integration

The intro remains short and unskippable. It is installed on the existing main-menu root and must not create a parallel menu owner. Nested installers mark the scene dirty; the top-level QA/install flow owns final scene saving.


<!-- B&D BBH CINEMATIC SIDE TASK V1 START -->
## Cinematic side-task refinement

This is a focused side task and does not change the active handheld task, its priority, its saved resume point, or the ordered work queue.

The existing `BDBBHBootIntro` remains the sole owner. The intro keeps the first black frame, strict `B -> B -> H` entrance order, unscaled timing, one-play-per-application-session behavior, input blocking, and the existing menu handoff.

Animation intent:

- the first B is careful and curious, using a restrained searching offset and corrective settle;
- the second B is more confident and produces a small readable nudge/rebound interaction with the first B;
- the H is heavier and stabilizing, producing a restrained shared landing reaction;
- the `H landing` is the authored heavy settling beat that stabilizes the completed BBH mark before the circle finishes expanding;
- the completed letters respond subtly to the circle expansion;
- the previous perpetual idle pulse is replaced by one deterministic completion breath.

The filled graphite/steel circle is activated only after the H has completed its primary entrance. It gathers from a small point, expands with controlled overshoot, and settles to a responsive final badge that is **16% larger** than the previous desired diameter. The responsive clamp uses at most 78% of screen width and 72% of screen height plus a safe-edge bound, so the larger circle remains fully visible and never covers the letters.

No eyes, faces, limbs, parallel camera owner, menu redesign, gameplay change, or new runtime dependency is introduced.
<!-- B&D BBH CINEMATIC SIDE TASK V1 END -->

## Verification

1. Start the application from the operating system.
2. Confirm the first rendered frame is black with no visible first `B` or shadow copy.
3. Confirm the first `B` visibly animates from zero, then the second `B`, then `H`.
4. Confirm the filled graphite/steel circle grows from zero behind the letters.
5. Confirm the completed mark holds exactly `0.50s` before the intro finishes.
6. Confirm the intro does not replay during same-session gameplay scene reloads.

<!-- BND_INTRO_TO_MAIN_MENU_CINEMATIC_AND_TUTORIAL_SPACING_V105:BEGIN -->
## V10.5 explicit handoff signal

BBH completion publishes one pending `IntroToMainMenuTransition` request. The modern handheld consumes it once only at a real main-menu destination. Play cancels it; Skip preserves it.
<!-- BND_INTRO_TO_MAIN_MENU_CINEMATIC_AND_TUTORIAL_SPACING_V105:END -->

<!-- BND_BBH_GLOBAL_TIMESCALE_REMOVAL_V106:BEGIN -->
## Time ownership rule

The BBH intro uses realtime/unscaled timing and explicit presentation/input ownership. It must never pause or resume the global simulation clock.
<!-- BND_BBH_GLOBAL_TIMESCALE_REMOVAL_V106:END -->

<!-- BND_FIRST_LAUNCH_TUTORIAL_MECHANICS_REPAIR_V108:BEGIN -->
## V10.8 post-intro presentation boundary

The BBH owner still emits only the one-shot destination request. The receiving transition must present the real full-screen 3D handheld/table scene; BBH does not create or animate a 2D representation of the device and does not own the destination camera path.
<!-- BND_FIRST_LAUNCH_TUTORIAL_MECHANICS_REPAIR_V108:END -->
