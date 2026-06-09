# Active Task — Real 3D Handheld Main Menu and Pause UI

## V4 active repair — physical product acceptance after automated PASS

**Status:** `IMPLEMENTED / UNITY VERIFICATION REQUIRED`

The V3 build passed automated QA but failed the required user visual gate. Current work replaces the rejected flat decal model with actual molded material/depth, makes the table shadow visible, simplifies the New Game-only card, adds WASD navigation and adds a controlled upper-right glass glint.

Protected requirements:

- all screens stay inside the handheld display;
- only Start Game/New Run uses active-character Boy/Girl art; all other art remains neutral;
- the small card is fresh-New-Game-only, text-only and never mentions Boy/Girl route or Mother;
- no full-face image may cover modeled controls;
- the product remains on the supplied wood with progressive defocus, upper-right key light and short left shadow;
- arrows, WASD, mouse, D-pad and A/B/X/Y semantic ownership remain unified.

Implementation changes:

- removed runtime full-face decal object and obsolete decal shader/resource;
- object-space molded gradient surface shader with microdetail/specular response;
- deeper shell, rear core, outer bevel and side seams;
- dedicated penumbra/core/contact shadow layers;
- New Game-only text card and neutral top bar;
- WASD parity in both input backends;
- subtle upper-right glass glint.

Exact resume point: install, compile, run TEST EVERYTHING, then capture Main/Pause screenshots and verify every V4 focused gate before committing.

```text
Status: CURRENT / PREMIUM TEXTURE, LAYOUT AND CONTEXT-ART REPAIR IMPLEMENTED / UNITY VERIFICATION REQUIRED
Priority: user override before the previous runtime repair sequence
```

## Why this task exists

The current Main Menu and Escape/Pause presentation is still a flat/prototype shell and does not meet the approved product direction. The user has explicitly prioritized a production-ready upright 3D handheld before returning to the saved Runtime repair and enemy-animation queue. The task exists to create one coherent, tactile, reusable presentation system without replacing the existing menu-state, settings, progression, pause or run-flow authorities.

## Goal

Replace the flat/prototype menu presentation with a real upright 3D handheld device used by both the Main Menu and Escape/Pause flow.

## Protected content and behavior

- Upright classic handheld silhouette: screen above, controls below.
- Blue-to-orange molded-plastic gradient shell.
- Recessed emissive screen behind separate clear glass/plastic with visible thickness and restrained reflections.
- Separate modeled D-pad, A/B/X/Y, Settings and Progression buttons.
- Tactile hover/press/release motion, material response, sound and optional haptic feedback.
- Mouse and D-pad navigation.
- `A` confirm, `B` back, `X` Settings, `Y` Progression.
- Physical Settings and Progression shortcut buttons open those pages.
- User-facing `Progression` label on one line.
- Main and Pause share one visual and interaction system while retaining their different actions/state.
- No flat screenshot is used as the in-game device.
- Only Start Game / New Run uses paired Boy/Girl artwork and runtime selection follows the active character. Progression, Settings, Credits, Quit, Resume/Pause and confirmation artwork is character-neutral and must not be duplicated by character.

## Performance contract

- The device mesh, glass, buttons and screen materials are created/cached once; no per-frame mesh, material, texture or RenderTexture creation is allowed.
- Pointer and D-pad focus updates are event/state driven; do not scan the scene or allocate collections every frame.
- Button travel uses bounded transform/material animation and must not add physics simulation or a second gameplay input owner.
- Screen rendering uses one deliberately sized render target per active device presentation, released on teardown and not silently duplicated between Main and Pause.
- Glass reflections, transparency and emissive treatment must remain readable and scalable on desktop and landscape mobile.
- Performance acceptance requires profiling in the real menu and Pause states, stable frame pacing, no orphaned render targets/material instances and no recurring GC allocations from navigation or button animation.


## Compilation dependency repair

The first Unity import exposed one earlier/blocking integration defect: the presenter uses GameObject-based uGUI components (`Image`, `Text`, `RawImage`, `Outline`) but the project did not declare the Unity UI package.

The repair is intentionally narrow:

- add `com.unity.ugui` `2.0.0` to `Packages/manifest.json`;
- keep the existing Runtime presenter and its state/input ownership unchanged;
- add an automated manifest check to `BDModernHandheld3DQA`;
- require Unity compilation and TEST EVERYTHING before any visual or cinematic follow-up.

This repair does not verify the device and does not implement the queued camera transition.


## First Play Mode rejection and repair

### Second Play Mode refinement

The next Play Mode review confirmed major improvement but rejected the remaining low-resolution shell treatment, overlapping center labels, long-title pressure and shared New Game artwork. The focused production repair now uses a high-resolution shell/decal pipeline, a dedicated decal shader, separated hardware-label ownership, adaptive title sizing and context artwork resolved from the selected action/page.

The first compiled runtime result was rejected and remains part of this active task. Observed failures were: blank live display, Escape Pause closing again immediately, oversized/repeated hardware labels, reversed-looking XYAB lettering and incomplete physical-button click coverage.

The focused repair changes presentation only and preserves `BDMainMenuFlow` as semantic owner:

- internal page UI renders through a `ScreenSpaceCamera` Canvas into the existing cached `960×1080` RenderTexture;
- page construction forces one immediate screen-camera render so the product camera never samples an uninitialized clear frame;
- presenter activation waits for the initiating Escape/mouse/gamepad press to be released before accepting menu actions;
- every X/Y/A/B and center shortcut has its own enlarged invisible collider/hit target and animates the visible modeled control;
- face letters and shortcut labels are compact front-facing siblings and do not inherit the cylinder rotation.

Focused acceptance requires Main and Pause content visible inside the glass, stable Pause persistence, no overlapping labels, and successful independent clicks on X, Y, A, B, Settings and Progression.

## Implemented vertical slice

The first complete runtime vertical slice is now present in code and assets:

- `BDModernHandheld3DPresenter` creates one persistent upright 3D device, one isolated screen camera and one cached RenderTexture.
- The shell is real generated 3D geometry with a screen opening, blue-to-orange texture, thickness, bezel, backing, modeled controls and speaker inserts.
- The display is recessed behind separate transparent glass and reflection layers.
- Main Menu, Pause, Settings, Progression, Credits, Abandon confirmation and Loading render inside the device screen.
- Mouse raycasts can hover/click screen rows and physical controls.
- Arrow/gamepad D-pad navigation, A confirm, B back, X Settings and Y Progression are wired through the existing `BDMainMenuFlow` owner.
- Physical `SETTINGS` and `PROGRESSION` controls use the same actions.
- Button meshes have bounded tactile press/hover motion and cached material feedback.
- `BDPlayableCharacterIdentity` deterministically selects the paired hero image only for Start Game / New Run.
- Boy play uses Boy New Game art; Girl play uses Girl New Game art. The choice is never random and is refreshed when active character identity changes.
- Main and Pause option selection routes to dedicated character-neutral Progression, Settings, Credits, Quit/Return and Resume artwork; those assets are single-source and never duplicated by character.
- Legacy flat menu/backdrop drawing is suppressed only while the new 3D presenter owns the visible menu.
- Automated QA coverage validates architecture tokens, shaders, interaction mapping and paired-art dimensions/import settings.

Implementation is not verified until Unity compilation, automated QA, focused Play Mode and user visual acceptance are complete.

## Approved physical product-shot scene

- Build a rounded 3D rectangular handheld with the approved portrait proportions; do not place a flat device image in the game.
- Apply the uploaded multi-view/front artwork as a masked surface decal on the shell only. The screen aperture, D-pad, A/B/X/Y, Settings, Progression and speaker areas remain live 3D geometry.
- Put the live menu RenderTexture in the physical screen dimensions, recessed behind a separate glass/plastic cover with thickness.
- Place the device on the uploaded dark-wood table texture. The device faces the camera with a small backward tilt so the upper edge is farther away.
- Light from above/right and keep the device shadow soft, short and leftward.
- The table is not uniformly blurred. The device is the focus subject and table detail transitions gradually from relatively sharp around the contact/focal band to defocused in the near/far regions.
- All page changes and the existing abandon confirmation happen inside the handheld screen. No separate screen-space dialog may appear above the product scene.

## Current implementation inventory

Runtime owners and assets in this change:

- `BDModernHandheld3DPresenter` — generated device/table/shadow geometry, cameras, RenderTexture, pages, transitions and input translation;
- `BDModernHandheldControlTarget` — cached physical hover/press/release response;
- `BDPlayableCharacterIdentity` — deterministic active-character art authority;
- `BDMainMenuFlow` — unchanged semantic owner expanded with safe presenter actions and Progression page state;
- `BDModernHandheldSurface`, `BDModernHandheldGlass`, `BDModernHandheldShadow`, `BDModernHandheldTable` — molded shell/control, directional glass, product shadow and gradual-focus table shaders;
- shell microtexture, exact wood sharp/blur pair, shadow masks, scanlines, glass reflection and paired Boy/Girl hero assets under `Assets/_Project/Resources/ModernHandheld/`;
- `BDModernHandheld3DQA` integrated into `TEST EVERYTHING`.

## Latest user clarification — artwork scope

Only the New Game / New Run option is protagonist-aware:

- active Boy route → Boy New Game image;
- active Girl route → Girl New Game image.

All other options and pages—Progression, Settings, Credits, Quit/Return, Resume/Pause and abandon confirmation—must use their own character-neutral image. They must not display either protagonist and do not require duplicate Boy/Girl variants. A saved preference is only a deterministic fallback for the New Game pair when no active player identity is available; it may never override a detected active character.

## Required implementation order

1. Audit existing menu, pause, input, UI rendering and scene ownership.
2. Decide the minimum additive 3D presentation architecture without duplicating state owners.
3. Build the device hierarchy, materials, screen render target and interaction colliders.
4. Build Main Menu screen content using real current data only.
5. Build Pause screen content using real current data only.
6. Add mouse and hardware-style navigation/activation.
7. Add tactile animation, sound, glass depth and accessibility states.
8. Add deterministic Boy/Girl paired art selection only to Start Game / New Run and route all other options/pages to character-neutral assets.
9. Add/adjust TEST EVERYTHING checks.
10. Run compile, automated QA, focused Play Mode, performance and user acceptance.

## Canonical asset specification

[`../../Production/ModernHandheld/MODERN_HANDHELD_3D_SPEC.md`](../../Production/ModernHandheld/MODERN_HANDHELD_3D_SPEC.md)

## Latest artwork-routing clarification

- Only Start Game / New Run may depict the playable protagonist.
- Boy gameplay uses the Boy New Game image; Girl gameplay uses the matched Girl New Game image.
- Progression, Settings, Credits, Quit/Return, Resume/Pause and abandon confirmation each use their own character-neutral image.
- Changing the active character must not alter any neutral image.
- This avoids unnecessary duplicate Boy/Girl production while preserving exact route identity where it matters.

## Exact resume point

Install the V4 physical-material repair ZIP, compile and rerun `TEST EVERYTHING`. Verify molded shell depth/bevels/seams, no full-face decal or texture overlap, visible short left shadow, restrained upper-right glass glint, separate Settings/Progression labels, correct contextual artwork, fresh-New-Game-only text card, WASD/arrow parity, live screen, Pause persistence, all controls, table focus, transitions and cleanup. Only after user visual acceptance may the queued seamless handheld-to-gameplay transition begin.
