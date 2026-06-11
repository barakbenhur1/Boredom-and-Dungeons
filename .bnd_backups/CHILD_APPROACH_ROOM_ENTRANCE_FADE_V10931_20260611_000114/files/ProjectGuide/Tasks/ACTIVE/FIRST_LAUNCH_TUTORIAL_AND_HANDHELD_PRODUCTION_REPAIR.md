<!-- BND_CHILD_APPROACH_CINEMATIC_FADE_SPEED_SMOOTHING_V10930:BEGIN -->
## 2026-06-11 — Child approach fade, speed and transition smoothing V10.9.30

The post-intro child POV cinematic remains the active task. This pass:

- starts the child farther behind and farther left of the chair while preserving the accepted child eye height;
- begins from a fully black frame and fades the room in before visible movement starts;
- shortens the complete sequence from `8.25s` to `7.45s` without rushing the screen power-on;
- replaces the segmented chair climb with a tangent-continuous Catmull-Rom route;
- blends the chair-clearance release instead of switching at a hard threshold;
- replaces the two-step post-climb look movement with one cubic camera curve and one cubic look-target curve;
- preserves the accepted screen power direction and content fade-in.

Acceptance still requires Unity compilation, `TEST EVERYTHING 0/0/0`, and real-time visual review.
<!-- BND_CHILD_APPROACH_CINEMATIC_FADE_SPEED_SMOOTHING_V10930:END -->

<!-- BND_CHILD_APPROACH_CINEMATIC_PATH_CLEARANCE_V10929:BEGIN -->
## V10.9.29 active implementation
Visually verify the farther-left approach and collision-safe natural climb. Do not move to later tutorial scope until the camera clears every chair component at runtime.
<!-- BND_CHILD_APPROACH_CINEMATIC_PATH_CLEARANCE_V10929:END -->

<!-- BND_CHILD_APPROACH_CINEMATIC_POLISH_V10928:BEGIN -->
## V10.9.28 active implementation
Polish and visually verify the child approach/climb/power-on shot. Do not move to later tutorial scope before this shot is accepted.
<!-- BND_CHILD_APPROACH_CINEMATIC_POLISH_V10928:END -->

<!-- BND_CHILD_APPROACH_CINEMATIC_V10927:BEGIN -->
## V10.9.27 active implementation

Implement and verify the child-POV walk/climb/power-on sequence before moving to later tutorial work. The chair, table, handheld and tutorial design remain authoritative and must not be redesigned.
<!-- BND_CHILD_APPROACH_CINEMATIC_V10927:END -->

<!-- BND_POST_INTRO_CHAIR_AND_QA_REPAIR_V10926:BEGIN -->
## 2026-06-11 — Post-intro chair + QA access repair V10.9.26
- Added a centered wooden dining chair in front of the table, aligned with the handheld.
- The chair uses the new `BDCinematicDiningChair` texture resource and stays in the same physical room staging.
- Added chair geometry and chair contact shadows in `BDModernHandheld3DPresenter.CinematicEnvironment.cs`.
- Repaired the Unity editor compile blocker by changing `BDFirstLaunchTutorialQA.Scan()` from private to internal so `BDOneClickQAWindow` can call it.
- Runtime focus: no limbo/cyclorama reintroduction; room staging remains the active implementation.
<!-- BND_POST_INTRO_CHAIR_AND_QA_REPAIR_V10926:END -->

<!-- BND_POST_INTRO_REAL_ROOM_AND_CLOSER_FRAMING_V10925:BEGIN -->
## V10.9.25 active implementation

Continue the post-intro cinematic task. Replace the black cyclorama stage with a real room floor and distant wallpapered back wall, brighten the room, and move the final camera slightly closer without cropping the handheld. Do not modify tutorial mechanics or gameplay.
<!-- BND_POST_INTRO_REAL_ROOM_AND_CLOSER_FRAMING_V10925:END -->

<!-- BND_POST_INTRO_FINAL_FIRST_LAUNCH_QA_REPAIR_V10924:BEGIN -->
## V10.9.24 active QA repair

The post-intro cinematic remains the active task. Repair only the final inconsistent first-launch look-target contract. Do not modify the existing visual implementation. After `TEST EVERYTHING 0/0/0`, resume visual acceptance.
<!-- BND_POST_INTRO_FINAL_FIRST_LAUNCH_QA_REPAIR_V10924:END -->

Resolved authoritative final-look target: `new Vector3(0f, -7.18f, -4.18f)`.

<!-- BND_POST_INTRO_CINEMATIC_WALLPAPER_FOCUS_DELIVERY_REPAIR_V10916:BEGIN -->
## V10.9.16 active implementation

The active task remains the post-intro cinematic. This pass specifically refines blur, end framing and room character. Implement the gentler DOF, full-device final framing with a small bottom tabletop margin, and stylized wallpaper dressing. Preserve all approved tutorial and handheld-system behavior.
<!-- BND_POST_INTRO_CINEMATIC_WALLPAPER_FOCUS_DELIVERY_REPAIR_V10916:END -->

<!-- BND_POST_INTRO_CINEMATIC_QA_LATEST_COMMIT_ALIGNMENT_V1094:BEGIN -->
## V10.9.4 interruption and exact resume point

Commit `ebe0eb6c40eb2ba291fd5cc23edcd4eac2ecf572` is the required package baseline and its agent-system/cinematic changes are protected by preflight, before/after hashes and post-write validation. The first Unity gate after V10.9.1 compiled and ran, then stopped on one stale focused-QA token: `Short Core Shadow To Left`. V10.9.3 subsequently stopped before writing because it looked for skill identifiers in the wrong owner (`AGENTS.md`). V10.9.4 validates the actual root contract plus the two authoritative skill files, changes only `BDModernHandheld3DQA` and synchronized documentation, then resumes immediately at Unity recompilation and `TEST EVERYTHING`. After `0/0/0`, return to the preserved cinematic Play Mode review. No tutorial mechanics, camera tuning or queued task order changed.
<!-- BND_POST_INTRO_CINEMATIC_QA_LATEST_COMMIT_ALIGNMENT_V1094:END -->

<!-- BND_POST_INTRO_CINEMATIC_DIRECTOR_PASS_V109:BEGIN -->
## V10.9 focused interruption — post-intro camera and set completion

The user supplied a final director/staging contract and required this shot to be completed before later work. V10.9 replaces the plane-based table with a complete grounded 3D set and replaces the short single-ease move with the specified 4.40-second one-camera path. This is a focused improvement-only interruption: tutorial mechanics, screen/menu behavior, device design and the saved later queue remain unchanged.

**Resume after package application:** Unity compile -> TEST EVERYTHING `0/0/0` -> focused five-timepoint cinematic review -> mounted-shot regression check -> user acceptance. After acceptance return to the queued retro tutorial visual redesign.
<!-- BND_POST_INTRO_CINEMATIC_DIRECTOR_PASS_V109:END -->

<!-- BND_FIRST_LAUNCH_TUTORIAL_V1081_HOTFIX:BEGIN -->
## V10.8.1 interruption and exact resume point

The user reported that mounted shooting visually succeeds but the tutorial remains stuck. Root cause: the production shot was hard-coded as non-advancing. The hotfix makes only the `RangedAttack` lesson shot progression-capable and still requires confirmed impact on a living target.

The user also clarified the landing shot: there is always one full-screen 3D table scene with the handheld on it. The cinematic changes only the camera inside that scene; it must never scale, slide, rotate or reposition the device/table/shadow as screen-space elements.

The package revision also restores mandatory colored terminal output. After this hotfix passes Unity, resume the queued retro visual redesign; after that gate, return to the preserved broader queue. The new-enemy/articulated-model/difficulty contract is captured for its later dependency position.
<!-- BND_FIRST_LAUNCH_TUTORIAL_V1081_HOTFIX:END -->

# First-Launch Tutorial and Modern Handheld Production Repair

```text
Status: IMPLEMENTED IN LOCAL PATCH PACKAGE / UNITY VERIFICATION REQUIRED
Classification: CURRENT + EARLIER/BLOCKING REPAIR
Package base: origin/main at 17d5c39435b5fa075d3e606c4d2e43b8f824b03f
Git writes by assistant: PROHIBITED
Delivery: local backup-aware patch ZIP
```

## Accepted scope

This task combines the user-approved handheld correction pass, the first-launch tutorial contract, the supplied BBH cinematic side task, and the permanent repository delivery/production-code rules. Nothing in this task changes the ordered `WORK_QUEUE.md`.

### Modern handheld

- keep the device slightly lower in the table composition, with its cast/contact shadows moving from the same authoritative rest position;
- restore the large context image to the original title-aligned composition, not the Start Game row-aligned override;
- retain the text-only contextual card below the image for every Main Menu selection and update its heading/body from the selected action;
- fix the broken `THE MAZE AWAITS` card and enforce bounded text layout across Main, Pause, Settings, Progression, Credits and confirmation pages;
- move SELECT and EXIT inward through the authoritative model builder so labels and hit targets inherit the same positions;
- make Settings icon selection font-safe;
- remove blue physical-button hover emission/frame behavior;
- implement D-Pad, SELECT, EXIT and X/Y/A/B press depth and scale response directly in `BDModernHandheldControlTarget`;
- make Escape/Pause a dedicated internal handheld-screen menu instead of a Main Menu product-card clone;
- remove the merged V6 companion/compatibility classes after their accepted behavior is integrated into the real owners.

### First launch

- on `NotStarted` or interrupted `InProgress`, show white boot light then a deterministic 2D tutorial inside the handheld glass before Main Menu content;
- teach one mechanic at a time through an isolated scripted sequence;
- preserve the required horse/enemy/heal opening event;
- use the existing normalized handheld actions and actual project keyboard/controller bindings represented by the tutorial input map;
- support physical controls, keyboard, controller, mouse and touch without duplicate action execution;
- always show large modern keyboard/mouse and physical-handheld instructions in parallel;
- EXIT opens a tutorial-native confirmation panel with safe default Continue and an input guard;
- confirmed abandonment writes `Skipped` before transition and permanently suppresses automatic replay;
- successful completion writes `Completed`; interruption remains safely restartable;
- grant no run reward, progression, statistics or production-world mutation.

### BBH side task

Integrate the supplied cinematic BBH letter-motion/circle refinement in the existing `BDBBHBootIntro` owner and add its check to `TEST EVERYTHING`. The active task order is not changed.

## Production-code requirement

Every changed or encountered production area must follow `ProjectGuide/Rules/PRODUCTION_CODE_STANDARD.md`. The implementation must use existing authoritative owners, explicit state, deterministic cleanup and testable contracts. A parallel component may be added only when it represents a real independent responsibility; it may not be used to disguise an avoidable patch over the owning system.

## Files and ownership

- `BDModernHandheld3DPresenter.cs`: authoritative device composition, screen pages, input routing and page transition integration.
- `BDModernHandheld3DPresenter.FirstLaunchTutorial.cs`: cohesive partial of the same owner for the isolated tutorial screen mode.
- `BDModernHandheldControlTarget.cs`: authoritative physical hover/press/highlight presentation.
- `BDFirstLaunchTutorialStateStore.cs`: one versioned durable display-decision owner.
- `BDBBHBootIntro.cs`: existing BBH cinematic owner.
- `BDOneClickQAWindow.cs`: single project QA entry point.

## Removed implementation debt

The following merged V6 companion approach is retired after direct integration:

- `BDModernHandheldV6Polish*`;
- `BDModernHandheldTactileCompatibility`;
- `BDModernHandheldPressScaleFeedback`;
- `BDModernHandheldV6QA` and its temporary V6 review/task/QA documents.

No scene, prefab, art, texture, shader or gameplay file is removed by this retirement.

## Required verification

1. Apply with the supplied one-command installer.
2. Run its validator and repository scans.
3. Open Unity `6000.0.76f1` and wait for compilation.
4. Run only `Boredom And Dungeons -> TEST EVERYTHING`.
5. Require `0 blockers / 0 warnings / 0 info` unless the user explicitly accepts a separately explained infrastructure finding.
6. Complete the focused handheld, tutorial and BBH Play Mode checklist supplied in the package.
7. Send the actual Unity output and screenshots before any commit.
8. Only after acceptance, create the local commit using the supplied command. The assistant never commits or pushes.

## Exact resume point

After package installation and static validation, resume at Unity compilation followed by `TEST EVERYTHING`, then the focused Play Mode list in `VERIFY_AFTER_APPLY.txt`.

<!-- BND_TUTORIAL_REFERENCE_LED_V3:BEGIN -->
## Active V3 refinement — reference-led clean tutorial

- apply the approved remembered-console fantasy palette;
- keep the world sparse and readable;
- reserve edge decorations away from gameplay actors;
- make the modern instruction card the dominant screen element;
- split keyboard/mouse and handheld bindings into large parallel cards;
- keep both input families active simultaneously;
- use only basic stepped pixel animation where it supports comprehension;
- reject any clipping, overlap or tiny instructional copy;
- complete automated and three-route Play Mode verification before returning to the ordered work queue.
<!-- BND_TUTORIAL_REFERENCE_LED_V3:END -->

<!-- BND_FIRST_LAUNCH_TUTORIAL_PRODUCTION_COURSE_V10:BEGIN -->
## Active V10 production-course implementation

The local delivery now expands V9 into the complete planned tutorial vertical slice: real jump; explicit learning-state evidence; accelerated/decelerated horse travel; health, ammo and Reload; damage timing and checkpoints; context Parry; environmental knockback; mounted impact; optional hidden branch; combined sword/ranged/small-enemy encounter; and a two-phase Mini-Boss with bounded summons, recovery windows and death-before-gate ordering.

Current truth:

```text
Code package: IMPLEMENTED LOCALLY
Unity compilation: REQUIRED
TEST EVERYTHING: REQUIRED
5–8 minute timing evidence: REQUIRED
Keyboard/mouse full run: REQUIRED
Controller full run: REQUIRED
Physical handheld full run: REQUIRED
Mixed-input full run: REQUIRED
User gameplay/visual acceptance: REQUIRED
```

After tutorial verification, the next approved task is persistent run Resume/Safe Return/Abandon scoring. The earlier Runtime repair queue remains preserved for return after that ordered work.
<!-- BND_FIRST_LAUNCH_TUTORIAL_PRODUCTION_COURSE_V10:END -->

<!-- BND_FIRST_LAUNCH_TUTORIAL_V10_WARNING_CLEANUP_V101:BEGIN -->
## V10.1 compiler-warning repair

Unity compiled V10 and TEST EVERYTHING passed, but six tutorial demonstration booleans were assigned without being read. They are redundant with the explicit `TutorialLearningState` dictionary and violate the production requirement against duplicate state and warning-bearing code.

V10.1:
- removes the six redundant fields and all redundant writes;
- records MountedShot, Dodge and Parry completion directly through the learning-state owner;
- retains existing Jump, Hazard and MountedImpact learning-state writes;
- adds static QA rejection for reintroducing the retired fields;
- requires a clean Unity recompile and TEST EVERYTHING rerun before continuing to focused Play Mode.
<!-- BND_FIRST_LAUNCH_TUTORIAL_V10_WARNING_CLEANUP_V101:END -->

<!-- BND_FIRST_LAUNCH_TUTORIAL_V10_INPUT_RESPAWN_FLASH_REPAIR_V102:BEGIN -->
## V10.2 focused repair slice

Origin: user Play Mode review after the V10.1 automated pass.

Implemented in the delivery package:
- replace false tutorial Jump/Dodge/Parry bindings with the live semantic controls;
- route physical B to Jump and physical directional double-tap to Dodge;
- allow either light or heavy melee input to resolve the Parry lesson;
- replace uncovered checkpoint teleport with cached fade-out/opaque restore/fade-in presentation;
- reserve first-launch presentation while menu flow resolves and suppress the legacy menu during that reservation;
- extend focused static QA and documentation.

Protected:
- encounter order, timings, damage, checkpoints and Mini-Boss design are unchanged except for recovery presentation;
- no second input, menu, camera, persistence or damage owner is introduced;
- the persistent-run/Continue/Abandon-scoring specification remains queued after full tutorial verification;
- earlier work-queue resume points remain open.

Exact next action: apply V10.2 to the real working tree, recompile, run TEST EVERYTHING, then perform the focused binding/respawn/transition matrix before returning to the remaining tutorial acceptance run.
<!-- BND_FIRST_LAUNCH_TUTORIAL_V10_INPUT_RESPAWN_FLASH_REPAIR_V102:END -->

<!-- BND_FIRST_LAUNCH_TUTORIAL_ENTRY_GATE_V103:BEGIN -->
## V10.3 entry gate and next animation phase

Implemented for verification:
- black pixel-style `B&D / Boredom & Dungeons` choice screen;
- `PLAY TUTORIAL` and `SKIP TUTORIAL`;
- semantic navigation across supported inputs;
- skip persistence through the existing state store;
- presenter installation before scene load;
- modern-presentation reservation through BBH completion and flow resolution;
- exact source-ZIP cleanup after successful package application.

Remaining active work:
- Unity verification of the V10.3 routes and no-flash contract;
- the full tutorial animation production pass detailed in `FIRST_LAUNCH_TUTORIAL_ENTRY_AND_ANIMATION_V11.md`;
- final timing, performance and visual acceptance.

Do not mark the tutorial or main-game animation work complete from V10.3 alone.
<!-- BND_FIRST_LAUNCH_TUTORIAL_ENTRY_GATE_V103:END -->

<!-- BND_FIRST_LAUNCH_TUTORIAL_PROGRESSION_GATE_REPAIR_V104:BEGIN -->
## V10.4 progression repair slice

Implemented for verification:
- true pixel glyph rendering for the pre-tutorial choice;
- consistent mounted ranged-only enforcement;
- forward Spin and Grapple lesson placement;
- invisible progression clamps with contextual feedback; the earlier visible-gate treatment is superseded by V10.8 because section-divider lines were rejected;
- one-round mounted-shot setup leading directly to Reload;
- contact/fallback completion protection retained from the previous repair.

This slice is not accepted until one uninterrupted full tutorial run reaches completion. The full limb/state animation production pass remains active immediately afterward.
<!-- BND_FIRST_LAUNCH_TUTORIAL_PROGRESSION_GATE_REPAIR_V104:END -->

<!-- BND_INTRO_TO_MAIN_MENU_CINEMATIC_AND_TUTORIAL_SPACING_V105:BEGIN -->
## V10.5 intro handoff slice

Implemented for verification: explicit BBH request/consumption; dedicated cinematic versus regular entry modes; wide angled active-screen establishing shot; cubic position path; rotation/FOV interpolation; exact final restoration; input lock; Play cancellation; Skip preservation; corrected title/subtitle spacing.

This does not complete the broader animation task.
<!-- BND_INTRO_TO_MAIN_MENU_CINEMATIC_AND_TUTORIAL_SPACING_V105:END -->

<!-- BND_BBH_GLOBAL_TIMESCALE_REMOVAL_V106:BEGIN -->
## Immediate blocker — global time-scale removal

Before animation production continues, remove BBH presentation ownership of the global simulation clock and re-run the complete Unity verification gate. This correction does not change tutorial mechanics, balance or animation scope.
<!-- BND_BBH_GLOBAL_TIMESCALE_REMOVAL_V106:END -->

<!-- BND_POST_INTRO_TRANSITION_COLORED_OUTPUT_CLEAN_EXIT_V1072:BEGIN -->
## V10.7.2 delivery repair

The transition target remains unchanged from the intended V10.7.1 implementation. This revision fixes validator scope and enforces unconditional package cleanup without weakening local-change protection or repository rollback guarantees.
<!-- BND_POST_INTRO_TRANSITION_COLORED_OUTPUT_CLEAN_EXIT_V1072:END -->

<!-- BND_FIRST_LAUNCH_TUTORIAL_MECHANICS_REPAIR_V108:BEGIN -->
## V10.8 active repair — mechanics synchronization, locomotion, boss and 3D handoff

The user Play Mode review reopened injured-horse mounting, locomotion leg motion, projectile death timing, checkpoint distance, Hook completion, section-divider visuals, final-boss readability/fairness, enemy body collision, post-BBH cinematic quality and the missing Charged Shot lesson.

Implementation is contained in the existing tutorial presenter partials plus one cohesive V10.8 transaction/collision partial. The actual `BDPlayerCombat` Charged Shot constants and state order are mirrored deliberately; no release-to-fire behavior is invented. Damage remains owned by the production-course actor system and is called only from projectile/Hook completion. The intro handoff remains owned by the existing one-shot transition partial and manipulates only the real cached camera/device/shadow transforms.

This slice is `IMPLEMENTED / UNITY VERIFICATION REQUIRED`. After acceptance, continue the broader animation-production pass in `FIRST_LAUNCH_TUTORIAL_ENTRY_AND_ANIMATION_V11.md`; do not treat the new tutorial leg frames as completion of the separate main-game rig/animation roadmap.
<!-- BND_FIRST_LAUNCH_TUTORIAL_MECHANICS_REPAIR_V108:END -->
