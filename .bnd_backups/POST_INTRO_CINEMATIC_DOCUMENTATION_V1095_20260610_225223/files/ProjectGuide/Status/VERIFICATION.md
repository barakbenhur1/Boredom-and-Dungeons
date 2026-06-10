<!-- BND_POST_INTRO_CINEMATIC_QA_LATEST_COMMIT_ALIGNMENT_V1094:BEGIN -->
## V10.9.4 verification record — 2026-06-10

Unity `TEST EVERYTHING` at `2026-06-10T19:16:43.2950910Z` produced exactly one blocker: `HANDHELD_3D_PRESENTER_MISSING — Missing required contract: Short Core Shadow To Left`. Compilation reached the QA run; there were no warning or info findings.

Static inspection confirmed a QA ownership drift rather than a missing Runtime shadow: V10.9's authoritative `CinematicEnvironment` partial contains the grounded device penumbra/core/base shadows and four table-leg contact shadows, while the older focused validator scanned only `BDModernHandheld3DPresenter.cs` and required the retired object name.

V10.9.3 then stopped safely before writing because its package preflight searched `AGENTS.md` for literal skill identifiers that the real commit stores in `.agents/skills`. V10.9.4 validates the actual `AGENTS.md` prose contract, validates each named skill in its own `SKILL.md`, includes `REPOSITORY_RULES.md`, and verifies protected latest-commit files remain byte-for-byte unchanged. A new Unity compile and `TEST EVERYTHING` run remain mandatory; no automated or visual acceptance is claimed yet.
<!-- BND_POST_INTRO_CINEMATIC_QA_LATEST_COMMIT_ALIGNMENT_V1094:END -->

<!-- BND_POST_INTRO_CINEMATIC_DIRECTOR_PASS_V109:BEGIN -->
## V10.9 post-intro cinematic verification truth — 2026-06-10

The first V10.9 real application exposed one delivery defect after writes: its broad substring scan interpreted the maintained bug-ledger phrase `` `=======` `` as a conflict. The installer then performed the intended automatic byte-for-byte rollback, verified restoration, removed the failed-attempt backup and cleaned the downloaded/extracted package. V10.9.1 replaces that scan with full-line Git-marker recognition and explicitly tests that the real decorative phrase passes while `<<<<<<<`, `|||||||`, `=======` and `>>>>>>>` full-line markers fail.

Package/static verification covers source-anchor preflight, backup creation, atomic writes, exact target validation, first and idempotent application, unknown transition-content blocking, byte-for-byte rollback, semantic terminal colors on a pseudo-terminal, ANSI suppression for `NO_COLOR=1`, `TERM=dumb` and redirected output, existing-shell installer cleanup, C# brace/preprocessor balance, forbidden old plane/spline tokens, documentation synchronization and `git diff --check` where Git is available.

Not yet verified: Unity compilation, `TEST EVERYTHING`, actual 3D lighting/shadows, screen readability, table proportions, 24/30/60 FPS frame pacing, frame-by-frame continuity, final handoff or user visual acceptance. These remain mandatory and must not be inferred from static checks.
<!-- BND_POST_INTRO_CINEMATIC_DIRECTOR_PASS_V109:END -->

<!-- BND_FIRST_LAUNCH_TUTORIAL_V1081_HOTFIX:BEGIN -->
## V10.8.1 verification contract

### Static/package — PASS

```text
Owned target hashes: PASS (34/34)
Install from supplied pre-V10.8 local state: PASS (34 paths)
Cumulative install over V10.8: PASS (24 paths changed; prior V10.8 preserved)
Idempotent second run: PASS (0 rewrites / no extra backup)
Unknown-local-change preflight: PASS (blocked before write; no backup)
Rollback: PASS byte-for-byte; repeated rollback also PASS
Interactive semantic colors: PASS (green PASS, red BLOCKED, cyan INFO, magenta CLEANED)
NO_COLOR=1: PASS (no ANSI)
TERM=dumb: PASS (no ANSI)
Redirected output: PASS (no ANSI)
Success/failure cleanup: PASS (exact ZIP and extracted artifacts removed)
ZIP integrity/path safety/unique members: PASS (1004 members)
File-manifest target hashes: PASS (34/34)
Complete project snapshot equality: PASS (962 files)
Git diff --check: PASS
Exact Git changed set: PASS (34/34)
Repository stability source scan: PASS (0 blockers / 0 warnings)
ProjectGuide repository hygiene: PASS
Unity compilation: NOT YET RECORDED
TEST EVERYTHING: NOT YET RECORDED
Focused Play Mode/camera/gameplay acceptance: NOT YET RECORDED
```

QA requires `advancesMountedShotLesson`, `hitLivingTarget` and `CompleteFirstLaunchTutorialMountedShotLessonAtImpact`, rejects reintroduction of device/shadow transform animation in `IntroToMainMenuTransition`, and retains the entire V10.8 regression matrix.

### Unity / focused Play Mode

1. Reach the mounted shooting lesson with its spawned enemy alive.
2. Fire once. Before projectile impact, the enemy remains alive and the step remains `RangedAttack`.
3. At visible impact, the enemy dies, `MountedShot` is demonstrated and the step changes to Reload exactly once.
4. Reload completes and the course enters Charged Shot; no second input is required to unstick the previous lesson.
5. A miss or shot without a living lesson target cannot advance.
6. Run the post-BBH landing: the wood table fills every frame, the device stays physically fixed on it, and only camera/lens motion changes composition.
7. Repeat the full V10.8 regression matrix before acceptance.

Unity results are not claimed until recorded from the user's run.
<!-- BND_FIRST_LAUNCH_TUTORIAL_V1081_HOTFIX:END -->

<!-- BND_FIRST_LAUNCH_TUTORIAL_MECHANICS_REPAIR_V108:BEGIN -->
## V10.8 package/static verification passed — Unity verification pending

```text
Source basis: user-supplied current local-state archive
C# tree-sitter structural/syntax validation: PASS for all changed tutorial/transition/QA sources
Duplicate presenter-field and exact-method-signature scan: PASS
QA required-token and runtime-contract simulation: PASS (0 missing)
Repository stability source scan: PASS (0 blockers / 0 warnings)
ProjectGuide repository hygiene scan: PASS
ZIP integrity, duplicate-member and traversal-path checks: PASS (994 members)
Installer first run from pristine supplied baseline: PASS
Installed target byte comparison: PASS (27/27 files)
Installer second/idempotent run: PASS (0 rewrites / no extra backup)
Unknown-local-change protection: PASS (blocked before write; sentinel preserved)
Rollback byte-for-byte test: PASS (27/27 files; repeated rollback also PASS)
git diff --check in committed fixture: PASS
Exact Git changed-set comparison: PASS (27/27 expected paths)
Package SHA manifest verification: PASS (993/993 entries)
Complete project snapshot byte comparison: PASS (960/960 files)
Unity compilation after V10.8: NOT YET RECORDED
TEST EVERYTHING after V10.8: NOT YET RECORDED
Focused Play Mode: NOT YET RECORDED
Performance/profile evidence: NOT YET RECORDED
User acceptance: NOT YET RECORDED
```

These package/static results prove deterministic delivery and source-contract integrity only. They do not prove Unity compilation, rendered timing, collision feel, camera quality, performance or gameplay acceptance.

Focused Unity evidence must include:

1. injured/red horse cannot be mounted and becomes mountable only after healing;
2. player, horse and enemies visibly alternate leg frames while moving and stop in an idle frame when stationary;
3. mounted and charged targets keep health until the projectile visibly arrives;
4. death at multiple late lessons restores near the current lesson behind the opaque checkpoint cover;
5. Hook target visibly travels before damage/progression;
6. no decorative section-divider line or visible lesson gate remains;
7. living enemies physically block walking/riding through them;
8. Charged Shot follows production timing/auto-fire/cancel/ammo/reload semantics;
9. final boss instructions persist and its telegraph, impact, recovery and damage window are understandable and survivable;
10. post-BBH handoff is one continuous full-screen 3D camera/device motion with exact final pose and no wrong/flat frame.
<!-- BND_FIRST_LAUNCH_TUTORIAL_MECHANICS_REPAIR_V108:END -->

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

<!-- BND_FIRST_LAUNCH_TUTORIAL_PRODUCTION_COURSE_V10:BEGIN -->
## V10 tutorial and queued saved-run verification truth

```text
V10 package integrity/static validation: performed by installer
Unity compilation: NOT YET RECORDED
TEST EVERYTHING: NOT YET RECORDED
Keyboard/mouse full tutorial: NOT YET RECORDED
Controller full tutorial: NOT YET RECORDED
Physical handheld full tutorial: NOT YET RECORDED
Mixed-input full tutorial: NOT YET RECORDED
5–8 minute timing: NOT YET RECORDED
User approval: NOT YET RECORDED
Persistent run/Continue/Abandon implementation: NOT STARTED
```

Focused tutorial contract: `ProjectGuide/QA/FIRST_LAUNCH_TUTORIAL_PRODUCTION_COURSE_V10.md`.
<!-- BND_FIRST_LAUNCH_TUTORIAL_PRODUCTION_COURSE_V10:END -->

<!-- BND_FIRST_LAUNCH_TUTORIAL_V10_WARNING_CLEANUP_V101:BEGIN -->
## V10.1 warning-cleanup verification truth

```text
Reported Unity automated run: 2026-06-10T02:45:38.8280090Z
TEST EVERYTHING before V10.1: PASS / 0 blockers / 0 warnings / 0 info
Unity compiler before V10.1: six CS0414 warnings
V10.1 static package validation: PASS
Unity compilation after V10.1: REQUIRED
TEST EVERYTHING after V10.1: REQUIRED
Focused tutorial Play Mode: STILL REQUIRED
```

The automated PASS is valid evidence for its own scanner output, but it does not override the separately reported compiler warnings.
<!-- BND_FIRST_LAUNCH_TUTORIAL_V10_WARNING_CLEANUP_V101:END -->

<!-- BND_FIRST_LAUNCH_TUTORIAL_V10_INPUT_RESPAWN_FLASH_REPAIR_V102:BEGIN -->
## V10.2 verification gate — input truth, readable respawn and no legacy flash

Pre-package evidence:
- V10.1 TEST EVERYTHING report: automated PASS, blockers `0`, warnings `0`, info `0`.
- focused Play Mode subsequently exposed binding, respawn-readability and transition-flash defects; therefore V10.1 is not the acceptance result for V10.2.

Required rerun:
1. Unity compiles with no Console error or warning introduced by V10.2.
2. TEST EVERYTHING reports `0 blockers / 0 warnings / 0 info`.
3. Keyboard/mouse: Space jumps; W/A/S/D and arrows move; a second A/D or Left/Right tap within the allowed window dodges; J/left click and K/right click can each produce a correctly timed Parry.
4. Physical handheld: B jumps; D-pad moves; double-tapping D-pad Left/Right dodges; X or Y can Parry; SELECT still interacts; A still owns the contextual ranged/heal lesson; X/Y tap/hold combat remains intact.
5. Death during at least the combined encounter and Mini-Boss shows the fall/fade, an opaque checkpoint cover and a controlled fade-in. No uncovered teleport is visible.
6. Fresh process launch after tutorial reset: BBH intro transitions directly to the intended modern handheld/tutorial surface with no legacy menu, plain menu or stale page visible for one frame.
7. Repeat at low frame rate and after app focus loss to ensure transition ownership remains deterministic.

No Play Mode, visual or user-acceptance claim is made by the package itself.
<!-- BND_FIRST_LAUNCH_TUTORIAL_V10_INPUT_RESPAWN_FLASH_REPAIR_V102:END -->

<!-- BND_FIRST_LAUNCH_TUTORIAL_ENTRY_GATE_V103:BEGIN -->
## First-launch tutorial V10.3 verification gate

Required before approval:

1. reset first-launch tutorial state outside Play Mode;
2. start Play Mode and confirm BBH intro transitions directly to the black `B&D` choice screen;
3. confirm no legacy menu, stale page or unwanted frame appears;
4. verify up/down and confirm using keyboard, controller, pointer and physical handheld controls;
5. choose `PLAY TUTORIAL`, confirm `InProgress` flow and controlled reveal;
6. reset, choose `SKIP TUTORIAL`, confirm the modern main menu appears with no flash;
7. restart and confirm choice/tutorial do not auto-return after skip;
8. rerun `Boredom And Dungeons → TEST EVERYTHING`;
9. confirm compiler errors/warnings introduced by V10.3 are zero;
10. confirm the source package ZIP is deleted only after a successful installer result.

This section does not verify the queued tutorial or main-game animation production passes.
<!-- BND_FIRST_LAUNCH_TUTORIAL_ENTRY_GATE_V103:END -->

<!-- BND_FIRST_LAUNCH_TUTORIAL_PROGRESSION_GATE_REPAIR_V104:BEGIN -->
## First-launch tutorial V10.4 verification gate

1. reset first-launch state outside Play Mode;
2. confirm the `B&D` choice title, subtitle, both options and status line use visibly pixelated point-filtered glyphs;
3. choose Play and complete the course in one forward run;
4. verify mounted Light/Heavy/Spin/Hook attempts do not attack and show `ON HORSE: RANGED ATTACKS ONLY`;
5. fire the final mounted round once and confirm automatic Reload begins;
6. ram the mounted-impact target and dismount at the marker;
7. verify Spin enemies appear ahead at the Spin station;
8. verify the Grapple target appears ahead across the gap;
9. verify any active hard lesson boundary is an invisible clamp with contextual instruction feedback and no visible divider/gate geometry;
10. continue through hazard, side path, combined encounter, final test and completion;
11. restart and confirm completion persists;
12. run `Boredom And Dungeons → TEST EVERYTHING` and require zero blockers, warnings and info.
<!-- BND_FIRST_LAUNCH_TUTORIAL_PROGRESSION_GATE_REPAIR_V104:END -->

<!-- BND_INTRO_TO_MAIN_MENU_CINEMATIC_AND_TUTORIAL_SPACING_V105:BEGIN -->
## V10.5 verification gate

- verify title/subtitle separation;
- verify BBH → wide angled active-handheld shot → exact regular pose;
- verify Skip preserves and Play cancels the one-shot request;
- verify internal-menu/gameplay returns never replay it;
- verify no legacy, wrong, black or correction frame;
- verify input lock;
- run TEST EVERYTHING with zero blockers, warnings and info.
<!-- BND_INTRO_TO_MAIN_MENU_CINEMATIC_AND_TUTORIAL_SPACING_V105:END -->

<!-- BND_BBH_GLOBAL_TIMESCALE_REMOVAL_V106:BEGIN -->
## V10.6 verification gate

Pending:
- compile with no errors/warnings;
- BBH sequence duration and visuals unchanged;
- no menu/gameplay input during BBH;
- IntroToMainMenu transition remains one-shot;
- `TEST EVERYTHING`: blockers=0, warnings=0, info=0.
<!-- BND_BBH_GLOBAL_TIMESCALE_REMOVAL_V106:END -->

<!-- BND_POST_INTRO_TRANSITION_COLORED_OUTPUT_CLEAN_EXIT_V1072:BEGIN -->
## V10.7.2 verification gate

- authoritative predecessor hash is accepted;
- unknown source is blocked before writes;
- runtime token validation excludes Editor QA;
- checksum, preflight and post-write failures all clean package residue;
- verified post-write rollback removes the failed-attempt backup;
- both post-BBH landing destinations work exactly once;
- TEST EVERYTHING returns blockers=0, warnings=0, info=0.
<!-- BND_POST_INTRO_TRANSITION_COLORED_OUTPUT_CLEAN_EXIT_V1072:END -->
