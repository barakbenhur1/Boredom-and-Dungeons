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
- [ ] Settings and Progression labels are separated, single-owned and readable.
- [ ] Long page titles do not overlap the artwork panel.
- [ ] Main/Pause selection swaps to the correct unique option image.
- [ ] Start Game / New Run alone uses Boy/Girl active-character art.
- [ ] Progression, Settings, Credits, Quit/Return, Resume/Pause and confirmation contain neither protagonist.
- [ ] New TEST EVERYTHING result is 0 blockers / 0 warnings / 0 info.

- Compilation reached Play Mode after the uGUI dependency repair.
- User-observed failures: no live menu in the physical display, Escape Pause visible only momentarily, overlapping/repeated hardware labels, reversed-looking face-button lettering, and only one reliable XYAB click target.
- Repair evidence required next: live Main and Pause pixels inside the glass, Pause remains open after the initiating Escape is released, compact correctly oriented labels, and separate successful mouse clicks on X, Y, A, B, Settings and Progression.
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
- Settings and Progression shortcuts correct;
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
- A/B/X/Y and physical shortcut buttons trigger exactly one action.
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
