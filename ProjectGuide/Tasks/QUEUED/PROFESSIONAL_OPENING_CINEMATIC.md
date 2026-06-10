# Queued Task — Seamless Handheld-to-Gameplay Transition and Professional Opening Cinematic

```text
Status: REQUIRED / CAPTURED / BLOCKED BY 3D HANDHELD COMPILATION AND BASE VISUAL VERIFICATION
Classification: CURRENT FOLLOW-UP
```

## Why this task exists

The approved upright 3D handheld must become the actual visual bridge between menu and gameplay. Starting or leaving a run must feel like one continuous cinematic shot, not a scene cut, camera swap, black frame or overlay transition. This task expands and supersedes the earlier opening-only wording while preserving every valid opening-dialogue and mounted-entry requirement.

## Protected content and behavior

- Do not redesign the handheld, menu pages, gameplay, HUD, opening route or existing UI authorities.
- Gameplay is already visible inside the handheld screen before entry begins.
- Every transition remains inside the physical screen until the camera reaches or leaves the screen plane.
- No screen-space dialog or foreign overlay appears above the device.
- Boy route uses Boy imagery and presentation; Girl route uses the paired Girl version.
- The existing abandon confirmation wording/semantics stay authoritative and render inside the device screen.

## State contract

Use one explicit transition authority with states equivalent to:

- `MenuIdle`
- `EnteringGame`
- `GameIntro`
- `Gameplay`
- `ExitingGame`
- `ReturningToMenu`

Each state owns camera enablement, visible UI, legal input, gameplay simulation, loading readiness and interruption policy. Double Start, repeated exit, enter-during-exit and exit-during-enter may not launch duplicate sequences. Progression is completion-driven; do not use unrelated hard-coded delays as state truth.

## Entry sequence

1. Lock menu/gameplay input and reject duplicate transition requests.
2. Load/warm the gameplay view and show it inside the handheld screen using the existing cached rendering path.
3. Preposition the gameplay camera at the approved very-high sky pose before it can render; the map, player and doorway must not flash from a temporary pose.
4. Hide every gameplay HUD element before the first gameplay frame. The opening speech bubble remains allowed when its authored moment arrives.
5. Move the menu/product-shot camera smoothly toward the physical screen, keeping the screen aperture centered despite the device's table angle and perspective.
6. Continue until the live gameplay image fills the viewport.
7. Perform a same-frame or blended handoff whose last menu-camera frame matches the first gameplay-camera frame in position, rotation, FOV/orthographic size, aspect, clipping, exposure, color grading, AA, bloom, motion blur, depth of field and render scale.
8. Only after handoff completes, start the opening dive from the high sky pose to the approved slightly farther gameplay composition.
9. Continue the child-and-horse entrance, settle, hold, exact `I’m bored.` bubble and wordless voice sequence.
10. Reveal HUD with a short controlled fade/motion only after the full intro completes, then return player control exactly once.

## Exit and abandon sequence

1. Lock input and stop unsafe gameplay actions.
2. Hide HUD cleanly.
3. Move the gameplay camera through a natural reverse departure toward the matched high connection pose.
4. Handoff back to the menu camera without a visible change in framing or processing.
5. Keep gameplay alive inside the handheld screen while the menu camera zooms back to the table product shot.
6. Restore the correct menu page and device input only after the camera reaches its exact idle pose.
7. Perform cleanup only after the gameplay image is no longer exposed by the screen transition.

## Camera and rendering match

Synchronize:

- position and rotation;
- FOV or orthographic size;
- aspect ratio and screen mask;
- near/far clipping;
- RenderTexture scale and filtering;
- exposure, color grading, anti-aliasing, bloom, motion blur and depth of field;
- lighting and any camera-stack/post-processing ownership.

Prefer the same live render source throughout the visible handoff. Never disable one camera and enable another across separate frames if that creates an invalid intermediate frame.

## Performance contract

- Pre-create/cache transition cameras, render targets, materials and references.
- Warm shaders/assets before visible zoom.
- No heavy instantiate, first-use shader compilation, scene search, LINQ or recurring allocations during transition updates.
- Use stable frame-rate-independent interpolation; use unscaled time where the transition must continue during pause.
- Avoid rendering two full-cost cameras longer than necessary.
- Profile CPU, GPU, frame time, memory and GC across repeated enter/exit cycles and lower-end targets.

## Edge cases

Verify normal entry/exit, rapid double input, exit during intro, input during zoom, slow load, low frame rate, resolution/aspect changes, desktop/mobile, pause then exit, death return, victory return, immediate new run, repeated cycles, focus loss/app quit, audio continuity, no early HUD, no wrong camera frame and no black/frozen/double frame.

## Retained opening sequence

1. camera begins high enough that the map is not visible;
2. controlled professional dive to a slightly farther final gameplay composition;
3. child and horse enter through the existing entrance;
4. child renderer is visible from the first presented mounted frame;
5. horse reaches the established stop and fully settles;
6. short natural hold;
7. speech bubble displays exactly `I’m bored.`;
8. reusable wordless character-voice system uses the Child/Bored profile;
9. bubble exits completely;
10. control returns exactly once.

Full dialogue rules remain in `../../Features/Cinematics/OPENING_DIALOGUE_WORDLESS_CHARACTER_VOICE_HE_V1.md`.

## Acceptance gate

- No visible cut, snap, black/transparent/frozen frame, pixel twitch, FOV jump or processing mismatch.
- Gameplay is visibly running inside the handheld before entry and remains there long enough during return.
- Gameplay camera begins only at the correct high pose and its dive starts only after handoff.
- HUD never appears early and returns only after intro completion.
- Input/state cannot double-trigger or strand cameras/UI/render targets.
- Repeated cycles remain stable with no leaks or meaningful frame spikes.
- Existing TEST EVERYTHING checks remain clean and new transition/state checks are added.

## Exact resume point

Do not implement this while the base handheld cannot compile. After `com.unity.ugui` resolves, TEST EVERYTHING passes and the 3D device receives focused visual/input approval, audit current menu camera, gameplay camera, run loading, HUD visibility and opening coordinator owners; then implement the smallest additive transition state machine without creating a second camera or run-flow authority.

<!-- BND_FIRST_LAUNCH_TUTORIAL_PRODUCTION_COURSE_V10:BEGIN -->
## Saved-run transition integration dependency

Before final transition implementation, the run-flow task must define four distinct intents: New Game, Continue, Save & Return and Abandon. New Game runs the approved opening. Continue restores without replaying New Game. Save & Return preserves Continue. Abandon waits for its meta-result screen to close, then performs the agreed reverse exit animation. No single generic return path may erase these semantic differences.
<!-- BND_FIRST_LAUNCH_TUTORIAL_PRODUCTION_COURSE_V10:END -->

<!-- BND_INTRO_TO_MAIN_MENU_CINEMATIC_AND_TUTORIAL_SPACING_V105:BEGIN -->
## V10.5 implemented handheld handoff subset

The BBH-to-main-menu handheld camera handoff is implemented for Unity verification. This does not complete the broader gameplay opening cinematic task.
<!-- BND_INTRO_TO_MAIN_MENU_CINEMATIC_AND_TUTORIAL_SPACING_V105:END -->
