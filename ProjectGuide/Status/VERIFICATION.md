<!-- B&D FIRST LAUNCH + HANDHELD PRODUCTION VERIFICATION START -->
## Pending verification — first launch, handheld direct repair and BBH cinematic side task

Current evidence is limited to package structure, installer idempotency, source-contract scans, repository hygiene and diff whitespace checks. Unity has not been run in the packaging environment.

Required evidence:

- clean Unity compile;
- TEST EVERYTHING: 0 blockers / 0 warnings / 0 info;
- title-aligned context image and correct context card for all Main selections;
- no blue hardware hover, equal press family across all physical controls;
- SELECT/EXIT positions, labels and click areas align without overlap;
- all screen text remains contained at narrow, standard and wide landscape ratios;
- Escape/Pause is an internal screen menu and every action preserves semantics;
- first clean-install tutorial appears before Main, supports all input routes and completes the full scripted lesson list;
- EXIT popup pauses tutorial, guards opening input, Continue resumes exactly, confirmed Leave persists Skipped before transition;
- Completed/Skipped do not automatically replay; interrupted InProgress restarts safely;
- BBH first frame/order/session gate remains valid and new authored motion/circle is visually accepted;
- no run reward, progression, scene actor or normal-game state changes are caused by tutorial.
<!-- B&D FIRST LAUNCH + HANDHELD PRODUCTION VERIFICATION END -->

<!-- B&D BBH CINEMATIC SIDE TASK V1 START -->
## Pending side-task verification — cinematic BBH boot intro

This side task does not change the active handheld priority or resume point. Required evidence before it is verified:

- Unity compilation has zero project compiler errors;
- TEST EVERYTHING reports 0 blockers / 0 warnings / 0 info;
- first frame is fully black and strict B -> B -> H order remains intact;
- first B, second B and H have visibly different but restrained personalities;
- the second-B nudge and H landing reaction are readable without slapstick;
- the circle gathers only after the letters settle, is clearly larger, remains filled/behind the text and does not crop;
- the completion breath occurs once and no perpetual idle bounce remains;
- same-session New Game does not replay the intro;
- 16:9, 16:10, ultrawide and landscape-mobile framing are visually accepted.

Current truth: implementation/package validation only; Unity and user visual acceptance are pending.
<!-- B&D BBH CINEMATIC SIDE TASK V1 END -->


## V5 pending focused verification

- compile with no warnings from the handheld scripts;
- TEST EVERYTHING returns 0 blockers / 0 warnings / 0 info;
- device sits slightly higher while remaining grounded on the wood;
- short soft shadow is visible to the left and contact shadow remains tight;
- upper-right glass glint is visible but never obscures title, rows or artwork;
- Main Menu X starts New Game, A opens Progression, B opens Settings and Y opens Credits;
- B returns on every non-main page;
- center SELECT activates the highlighted option and center EXIT opens the correct confirmation;
- SELECT/EXIT labels are equal-size, recessed and do not touch buttons;
- New Game hero and memory cards are stacked with no clipping;
- Progression, Credits, Settings and confirmation layouts stay inside safe margins;
- all physical controls read as textured 3D parts and press independently.

## 2026-06-09 — Modern handheld first Play Mode rejection

## Latest evidence — V3 automated PASS, visual gate failed

- Unity reached `B&D TEST EVERYTHING: AUTOMATED PASS`.
- Compiler emitted one non-blocking warning for the now-removed unused `displayedCharacterInitialized` field.
- User Play Mode evidence rejected the product result: no readable left shadow, insufficient depth, full-face texture overlap on controls, and incorrect small-card content/visibility.
- V4 is implemented statically but has not yet been compiled or visually verified in Unity.

Required V4 evidence: compiler clean, TEST EVERYTHING 0/0/0, screenshots of Main and Pause, visible short left shadow, no decal overlap, tactile depth, New Game-only card behavior, WASD parity and non-obscuring upper-right glass glint.


## Pending focused gate — premium handheld art routing repair

- [ ] Premium shell texture is sharp and stable at the actual camera angle.
- [ ] No full-face Runtime decal exists; molded material and geometry remain clean around the live screen and controls.
- [ ] SELECT and EXIT labels are separated, single-owned, recessed and readable.
- [ ] Long page titles do not overlap the artwork panel.
- [ ] Main/Pause selection swaps to the correct unique option image.
- [ ] Start Game / New Run alone uses Boy/Girl active-character art.
- [ ] Progression, Settings, Credits, Quit/Return, Resume/Pause and confirmation contain neither protagonist.
- [ ] New TEST EVERYTHING result is 0 blockers / 0 warnings / 0 info.

- Compilation reached Play Mode after the uGUI dependency repair.
- User-observed failures: no live menu in the physical display, Escape Pause visible only momentarily, overlapping/repeated hardware labels, reversed-looking face-button lettering, and only one reliable XYAB click target.
- Repair evidence required next: live Main and Pause pixels inside the glass, Pause remains open after the initiating Escape is released, compact correctly oriented labels, and separate successful mouse clicks on X, Y, A, B, SELECT and EXIT.
- Automated QA must pass before any visual acceptance claim.

## 2026-06-09 — Modern handheld compilation blocker

- Unity: `6000.0.76f1`.
- Generated UTC: `2026-06-09T02:59:12.2289930Z`.
- `TEST EVERYTHING`: BLOCKED.
- Blockers: 1.
- Warnings: 0.
- Info: 0.
- Blocker: `UNITY_SCRIPT_COMPILATION_FAILED`.
- Console evidence: `UnityEngine.UI` namespace and `Image`, `Text`, `RawImage`, `Outline` types were unresolved in `BDModernHandheld3DPresenter`.
- Root cause: `com.unity.ugui` was absent from `Packages/manifest.json`.
- Repair: declare `com.unity.ugui` `2.0.0` and add a TEST EVERYTHING dependency guard.
- Required next evidence: package resolves, Console compiler errors are zero, TEST EVERYTHING is `0 / 0 / 0`.

## 2026-06-09 — ProjectGuide V1.2 automated verification

- Unity: `6000.0.76f1`.
- Generated UTC: `2026-06-09T00:13:48.3411810Z`.
- `TEST EVERYTHING`: PASS.
- Blockers: 0.
- Warnings: 0.
- Info: 0.
- Scope truth: this pass verifies ProjectGuide migration/compatibility before the new 3D handheld Runtime code. The 3D handheld implementation requires a fresh compilation and TEST EVERYTHING run.

## Pending — Modern 3D handheld implementation

Required evidence:

- compiler/Console clean;
- automated QA clean;
- real 3D device volume and glass layering visible;
- Main Menu and Pause pages correct;
- mouse and hardware-style controls correct;
- SELECT and EXIT center controls correct;
- Boy active character → Boy art on every relevant menu surface;
- Girl active character → Girl art on every relevant menu surface;
- no random, stale or mismatched paired art after scene reload;
- repeated open/close/reload leaves no duplicate cameras, RenderTextures, materials, listeners or input owners;
- idle menu navigation produces no recurring GC allocation attributable to the presenter;
- user visual acceptance.

### Static/package evidence recorded before Unity

- all 247 C# files parse without syntax errors using the C# grammar;
- repository source stability scan passes after obsolete intermediate texture exports were removed;
- repository hygiene and `git diff --check` are required again on the final ZIP fixture;
- V3 front decal alpha was valid but the approach was visually rejected and removed in V4; Boy/Girl assets remain `490×700` with matched imports; sharp/defocused wood textures remain `1254×1254`;
- Runtime uses one physical layer and one isolated screen layer, one presenter, one screen camera and one cached RenderTexture;
- this evidence is static only and does not imply Unity compilation or visual success.

# Pending Verification Ledger

No item is considered verified merely because code exists or automated checks passed.

## Unity rerun required

- `MaterialPropertyBlock` initialization repair: compile, TEST EVERYTHING and repeated Play Mode spawn.
- Historical QA semantic repairs: clean TEST EVERYTHING rerun.
- Fresh mounted intro after abandon: Boy visible from the first presented frame.

## Focused Play Mode still open

- Player death completes before menu overlay.
- Confirmed abandon returns to a clean main menu.
- Boy mounted melee/hook remain blocked without consuming cooldown.
- Target outline affects only the damageable enemy model; helper ring remains normal/subtle.
- Existing professional menu shell behavior remains functional until superseded by the approved 3D device.

## New handheld implementation acceptance

- Main and Pause display inside the same real 3D upright device.
- Mouse and D-pad navigation agree on focus.
- X/A/B/Y and the SELECT/EXIT center buttons trigger exactly one context-correct action.
- Glass depth and reflections do not reduce readability.
- Buttons visibly press and recover without stuck state.
- The Start Game / New Run Boy/Girl pair follows the active character; all other option images remain character-neutral.
- Desktop and landscape-mobile layouts remain readable and performant.

## ProjectGuide package revalidation

- Remove `.DS_Store` and AppleDouble `._*` files.
- Run the ProjectGuide validator again and require a clean PASS.
- Require repository hygiene, source stability scan and `git diff --check` to pass before Unity verification.
- Do not commit the documentation migration while this gate is open.

## ProjectGuide V1.2 Unity documentation gate

- Previous run: `2026-06-09T00:06:07.0833090Z`.
- Previous result: `BLOCKED`, 9 blockers, 0 warnings, 0 info.
- Scope of all findings: documentation discovery/continuity tokens only.
- Install V1.2 and rerun `Boredom And Dungeons -> TEST EVERYTHING`.
- Acceptance: none of `V23R10_DOCUMENTATION_MAP_MISSING`, `V23R19Q_TASK_RECORD_MISSING`, `V23R8_DOCUMENTATION_MAP_MISSING`, `V23R9_ART_DIRECTION_MIRROR_MISSING`, or `V23R9_DOCUMENTATION_INDEX_MISSING` returns.
- Do not start the 3D vertical slice or commit until this automated documentation gate is clean.

## V5 focused acceptance matrix

1. Main Menu: X starts New Game, A opens Progression, B opens Settings, Y opens Credits.
2. Every non-main page: B returns exactly once.
3. Center SELECT activates the highlighted option; center EXIT opens the in-screen confirmation.
4. WASD and arrows remain navigation-only; keyboard A is never reused as the face-button A shortcut.
5. No control-cap texture contains neighboring buttons or square crop backgrounds.
6. New Game hero card and text card are vertically separated.
7. Left shadow is visible on wood; upper-right glass glint is visible without hiding UI.
8. Footer and page columns do not wrap, clip or collide.

<!-- B&D HORSE HUD MINIMAP V2 PENDING VERIFICATION START -->
## Pending verification — horse, contextual HUD, minimap and repository maintenance

Static package checks are provided. Unity compilation, Editor/PlayMode tests, `TEST EVERYTHING`, supported builds, clean-clone build, visual timing and user acceptance remain required locally. No automated-pass claim is recorded before that evidence exists.
<!-- B&D HORSE HUD MINIMAP V2 PENDING VERIFICATION END -->

<!-- BND_UNITY_UI_PACKAGE_RECOVERY_V3:BEGIN -->
## Unity UI package recovery V3 verification gate

Static recovery validates the package manifest, package lock, protected package ownership, source references, documentation, and rollback state. It does not claim Unity compilation. Reopen Unity 6000.0.76f1, wait for package resolution/import, clear the Console, and rerun `Boredom And Dungeons → TEST EVERYTHING`. Record the resulting blocker/warning/info counts before committing.
<!-- BND_UNITY_UI_PACKAGE_RECOVERY_V3:END -->

<!-- BND_HORSE_HEALING_COMPILE_FIX_V4:BEGIN -->
## Horse healing presentation compile-fix verification

```text
Static patch validation: PASS
Unity compilation after fix: NOT YET RECORDED
TEST EVERYTHING after fix: NOT YET RECORDED
Horse-healing Play Mode check: NOT YET RECORDED
```

Required evidence is defined in `ProjectGuide/QA/HORSE_HEALING_PRESENTATION_COMPILE_FIX_V1.md`.
<!-- BND_HORSE_HEALING_COMPILE_FIX_V4:END -->

<!-- BND_QA_CONTRACT_REALIGNMENT_V5:BEGIN -->
## QA contract realignment V5

```text
Patch integrity: PASS
Static source/document validation: PASS
Unity compilation after V5: NOT YET RECORDED
TEST EVERYTHING after V5: NOT YET RECORDED
Play Mode visual acceptance: NOT YET RECORDED
```

The verification contract is defined in `ProjectGuide/QA/QA_CONTRACT_REALIGNMENT_V5.md`.
<!-- BND_QA_CONTRACT_REALIGNMENT_V5:END -->

<!-- BND_TUTORIAL_REFERENCE_LED_V3:BEGIN -->
## First-launch tutorial reference-led V3 verification

```text
Package integrity: PASS
Static C# structure: PASS
First install mock: REQUIRED BY PACKAGE BUILD
Repeated install mock: REQUIRED BY PACKAGE BUILD
Rollback mock: REQUIRED BY PACKAGE BUILD
Unity compilation: NOT YET RECORDED
TEST EVERYTHING: NOT YET RECORDED
Three input-route runs: NOT YET RECORDED
User visual approval: NOT YET RECORDED
```

Focused acceptance: `ProjectGuide/QA/FIRST_LAUNCH_TUTORIAL_REFERENCE_LED_PRESENTATION_V3.md`.
<!-- BND_TUTORIAL_REFERENCE_LED_V3:END -->

<!-- BND_FIRST_LAUNCH_TUTORIAL_QA_CONTRACT_FIX_V8:BEGIN -->
## First-launch tutorial QA contract correction V8

```text
Package integrity: PASS
Targeted source validation: PASS
git diff --check: PASS
Unity compilation after V8: NOT YET RECORDED
TEST EVERYTHING after V8: NOT YET RECORDED
Play Mode acceptance: STILL REQUIRED
```
<!-- BND_FIRST_LAUNCH_TUTORIAL_QA_CONTRACT_FIX_V8:END -->
