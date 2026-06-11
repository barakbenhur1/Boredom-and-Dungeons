<!-- BND_POST_INTRO_CINEMATIC_FOCUSED_ROOM_POLISH_V10912:BEGIN -->
## V10.9.12 tabletop product-shot specification

- physical device scale remains `0.16`;
- authoritative rest position: `(0, -7.27, -4.28)`;
- direct final camera: `(0, -1.92, -3.82)`, rotation `(90, 0, 0)`;
- final responsive perspective FOV: `39°` at narrow aspect to `33.5°` at wide aspect;
- device remains fully inside frame and approximately 70%–80% of wide-frame height;
- front table margin remains positive and visibly intentional;
- depth-of-field focus target is the actual device transform, not a fixed arbitrary distance;
- near/far blur is restrained and the screen must remain readable;
- room dressing is secondary, dark, warm-accented and physically grounded.
<!-- BND_POST_INTRO_CINEMATIC_FOCUSED_ROOM_POLISH_V10912:END -->

<!-- BND_POST_INTRO_CINEMATIC_FINAL_ALIGNMENT_V1099:BEGIN -->
## V10.9.9 physical table placement and direct viewing geometry

Authoritative persistent device transform:

```text
position = (0, -7.27, -3.60)
rotation = (90, 0, 0)
scale    = (0.16, 0.16, 0.16)
```

The table spans `z = -7.20 ... +5.20`. The device's long-axis half-extent is approximately `7.70 × 0.16 = 1.232`, so the nearest edge is `-3.60 - 1.232 = -4.832`. The remaining front margin is `-4.832 - (-7.20) = 2.368` units. This is the approved near-front placement: clearly forward of center, but not attached to or overhanging the edge.

The screen center is approximately `DeviceRestPosition.z + ScreenCenterY × scale = -3.60 + 2.56 × 0.16 = -3.1904`. The final camera and target use `z ≈ -3.19`, producing a view normal to the horizontal screen. Contact shadows and light aim points must be offset from `DeviceRestPosition.z`, not hard-coded to the old table center.
<!-- BND_POST_INTRO_CINEMATIC_FINAL_ALIGNMENT_V1099:END -->

<!-- BND_POST_INTRO_CINEMATIC_DIRECTOR_PASS_V109:BEGIN -->
## Post-BBH product-set requirement V10.9

The approved handheld is staged on a complete premium dark-wood table, not a vertical texture plane. The set includes a thick top, front edge/lip, apron/frame, four connected legs/feet, matte charcoal floor, curved charcoal cyclorama, device contact shadow, leg contact shadows and restrained key/fill/separation lighting. The device sits straight on the table axis and remains stationary. In the final Main Menu frame the legs may be outside the viewport, but the tabletop top plane plus front thickness/apron must remain visible so the table still reads as physical furniture.
<!-- BND_POST_INTRO_CINEMATIC_DIRECTOR_PASS_V109:END -->


## V5 approved physical/input details

- The device sits slightly higher in the table composition than V4.
- Main Menu X starts New Game.
- Main Menu A opens Progression.
- Main Menu B opens Settings.
- Main Menu Y opens Credits.
- B returns on every non-main page.
- Center SELECT activates the highlighted option.
- Center EXIT opens the correct in-screen quit/abandon confirmation.
- Their labels are same-size recessed marks below separate buttons.
- D-pad, face and center-button top surfaces use cropped textures from the approved source sheet over real modeled depth.
- The screen glass shows a restrained upper-right key-light glint.
- New Game hero art and its text-only memory card are stacked vertically.

# Modern Upright Handheld — 3D Asset and Interactive UI Specification V1

## Context artwork matrix

| Context | Artwork rule | Character variants |
|---|---|---|
| Start Game / New Run | Boy or Girl with horse, selected from active route | Required pair |
| Resume / Pause | Neutral current-adventure/castle atmosphere | One shared asset |
| Progression | Character-neutral arcane memory hall | One shared asset |
| Settings | Character-neutral magical mechanism/workshop | One shared asset |
| Credits | Character-neutral creator library/worktable | One shared asset |
| Quit / Return / Confirm | Character-neutral moonlit exit gate | One shared asset |

Main and Pause selection must update the preview artwork before activation. Entered pages retain the same matching context image.

## Texture quality floor

- Shell source: minimum 2048×2048, high-quality/uncrunched import, anisotropic filtering.
- Runtime shell: real rounded geometry with rear depth, outer bevel and side mold seams.
- Surface: minimum 2048×2048 microtexture combined with an object-space blue→violet→orange molded-material shader.
- Full-face Runtime decals/stickers are forbidden because they flatten volume and can overlap modeled controls. The orthographic texture sheet remains a source/reference asset only.

```text
Status: V4 PHYSICAL-MATERIAL REPAIR IMPLEMENTED / UNITY VERIFICATION REQUIRED
Task ID: C11.UI.MODERN_HANDHELD_3D.V1
Captured: 2026-06-09
Primary owners: BDMainMenuFlow (state/input), 3D handheld presenter (presentation), menu-screen view (screen content)
```

## 1. Purpose

This document is the complete asset breakdown and implementation contract for the redesigned Boredom & Dungeons main menu and Escape/Pause menu.

The approved presentation is an original upright, portrait-oriented, modern handheld device inspired by the emotional memory of a classic Game Boy silhouette without copying a commercial device. The screen sits in the upper half. The physical controls sit in the lower half. The device is a real three-dimensional object with material depth, a tactile body, separate pressable controls and a real layered display assembly behind clear glass or transparent plastic.

The approved device replaces the idea of a flat decorative frame. It must read as a physical fantasy-tech object that could exist inside the game world.


## Implemented Runtime architecture

- `BDModernHandheld3DPresenter` owns presentation only and is installed once after scene load.
- `BDMainMenuFlow` remains the state, pause, reload and action authority.
- One isolated screen camera renders a World-Space Canvas to one cached `960×1080` RenderTexture.
- The RenderTexture is shown on a recessed display plane behind independent glass/reflection geometry.
- The shell, bezel, backing, controls and speaker inserts are generated once as real 3D meshes and cached until teardown.
- Mouse interaction uses bounded raycasts only while the menu is visible.
- Keyboard/gamepad D-pad and A/B/X/Y actions share the same action bridge as physical device clicks.
- `BDPlayableCharacterIdentity` selects the Boy/Girl texture pair from the active player identity, with deterministic saved fallback only when no active identity exists.
- Legacy IMGUI remains as a fallback but is suppressed while the presenter is ready and visible.

This implementation is not accepted until Unity compile, automated QA, focused Play Mode, performance and visual approval pass.

## 2. Approved visual identity

### 2.1 Form

- Upright portrait handheld.
- Screen in the upper half.
- D-pad in the lower-left area.
- X/Y/A/B cluster in the lower-right area.
- Two small center shortcut buttons between the D-pad and face buttons.
- Speaker perforations in the lower shell.
- Rounded, friendly, tactile silhouette with modern refinement.
- Original industrial design; no copied logo, exact dimensions, branded text or exact commercial hardware arrangement.

### 2.2 Shell color and finish

- The shell uses a continuous blue-to-orange gradient.
- Left side: rich electric/deep blue.
- Center transition: violet/indigo blend.
- Right side: warm orange/amber.
- The gradient belongs to the actual shell material/texture and follows the device consistently around bevels and edges.
- The finish is premium molded plastic with subtle fantasy patterning and controlled roughness variation.
- The device must not look like plain gray plastic, a flat illustration, a toy sticker or a simple UI border.

### 2.3 Screen depth

The display must visibly sit behind a separate clear cover layer.

Required physical layers, from front to back:

1. clear glass or transparent-plastic cover;
2. thin air/recess gap;
3. dark screen bezel/mask;
4. emissive display surface;
5. optional internal shadow/backing plate.

The clear cover requires:

- visible thickness at its edge;
- a small bevel;
- controlled reflection;
- subtle Fresnel response;
- restrained smudging/micro-surface variation only if it does not reduce readability;
- no opaque milky look;
- no reflection strong enough to obscure text.

### 2.4 Fantasy detailing

- Restrained etched/rune-like patterning may exist in the shell.
- A small magical status light/crystal accent may glow.
- Fantasy ornament remains secondary to readability and industrial clarity.
- The true-victory awakened variant may shift toward cyan/gold while preserving the same mesh hierarchy and interaction contract.

## 3. Mandatory character-art parity

Every menu, loading, progression, promotional, tutorial, preview, current-run, next-up or other UI image that depicts the Boy must have a parallel Girl version.

The parallel Girl version must preserve:

- exact canvas dimensions;
- crop and safe areas;
- camera angle;
- composition;
- background;
- lighting;
- horse pose when present;
- color grading;
- visual hierarchy;
- UI text-free regions;
- file format and import settings;
- naming/version relationship.

Only the player character representation changes. The Girl version must not be treated as an optional later marketing variant.

### 3.1 Active-route display rule

Active-character selection applies only to Start Game / New Run, because that is the only handheld option allowed to depict the playable protagonist. The New Game hero image and its small preview must change together. A Girl route may never show the Boy for one frame while identity resolves; unresolved identity uses the persisted deterministic preference, then refreshes immediately when an active identity appears. Missing route art produces a neutral/black-safe surface rather than displaying the other character.

Required naming pattern for protagonist art:

```text
<AssetName>_Boy_V###
<AssetName>_Girl_V###
```

Progression, Settings, Credits, Quit/Return, Resume/Pause and confirmation use dedicated character-neutral assets. They do not branch on active character and must not be duplicated by route.

When a new Boy image is added, its matching Girl image is part of the same Definition of Done. Neutral option artwork is complete as one shared asset only when it contains neither playable protagonist.

## 3.2 Table, camera, light and shadow composition

- The handheld rests on the supplied dark-wood table surface.
- The camera sees the device primarily front-on with a restrained perspective angle; the upper device edge is slightly farther from camera than the lower edge.
- The table and device must share a believable contact plane. The device may not float above a background quad.
- Key light reads from above/right. The principal cast shadow is short, soft and leftward, with a tighter contact shadow at the base.
- Focus is on the handheld and its screen. Table blur is depth-like and progressive: a relatively sharp focal/contact band transitions smoothly to stronger defocus toward near and far areas. A uniform pre-blurred background is not acceptable.
- The supplied wood image remains the source texture; generated sharp/blur variants must share crop, dimensions and color.

## 4. Terminology and labels

- The user-facing label is **`Progression`**.
- Do not use `Meta Progression` in the redesigned device UI.
- `Progression` must remain on one line at supported resolutions.
- The left center physical button is labeled **`SELECT`** and activates the highlighted option.
- The right center physical button is labeled **`EXIT`** and opens the correct in-screen quit/abandon confirmation.
- On Main Menu: X starts a new game, A opens Progression, B opens Settings, Y opens Credits.
- On every non-main page: B goes back.
- The center-button contract is independent of the face-button shortcuts.

## 5. Full 3D asset breakdown

### 5.1 Recommended prefab hierarchy

Names may follow the project's existing conventions, but responsibility must remain equivalent.

```text
BDModernHandheldDevice
├── DeviceRoot
│   ├── Shell
│   │   ├── ShellFront
│   │   ├── ShellBack
│   │   ├── ShellSideTrim
│   │   ├── ScreenRecess
│   │   ├── SpeakerPerforationsLeft
│   │   ├── SpeakerPerforationsRight
│   │   ├── EngravingOrDecalSurface
│   │   └── StatusLightHousing
│   ├── ScreenAssembly
│   │   ├── ScreenGlass
│   │   ├── ScreenGlassEdge
│   │   ├── ScreenBezel
│   │   ├── ScreenDisplay
│   │   ├── ScreenBacking
│   │   └── ScreenReflectionMask
│   ├── Controls
│   │   ├── DPadRoot
│   │   │   ├── DPadCap
│   │   │   └── DPadCenterAccent
│   │   ├── FaceButtons
│   │   │   ├── ButtonA
│   │   │   ├── ButtonB
│   │   │   ├── ButtonX
│   │   │   └── ButtonY
│   │   ├── ShortcutButtons
│   │   │   ├── ButtonSelect
│   │   │   └── ButtonExit
│   │   └── OptionalHardwareAccents
│   ├── Labels
│   │   ├── LabelSelect
│   │   ├── LabelExit
│   │   ├── LabelA
│   │   ├── LabelB
│   │   ├── LabelX
│   │   └── LabelY
│   ├── InteractionTargets
│   │   ├── DPadUpHitTarget
│   │   ├── DPadDownHitTarget
│   │   ├── DPadLeftHitTarget
│   │   ├── DPadRightHitTarget
│   │   ├── ButtonAHitTarget
│   │   ├── ButtonBHitTarget
│   │   ├── ButtonXHitTarget
│   │   ├── ButtonYHitTarget
│   │   ├── ButtonSelectHitTarget
│   │   └── ButtonExitHitTarget
│   └── Effects
│       ├── ScreenGlow
│       ├── StatusLight
│       └── OptionalFocusGlow
└── MenuPresentationAnchors
    ├── FrontCameraAnchor
    ├── EntryCameraAnchor
    ├── ScreenContentAnchor
    └── InteractionPlane
```

### 5.2 Shell meshes

#### ShellFront

- Main visible body.
- Rounded perimeter and tactile depth.
- Screen opening modeled into the geometry.
- Control openings/recesses modeled rather than painted.
- Bevels sized to remain visible at the real menu camera distance.
- UVs support the left-blue/right-orange gradient without seam discontinuity.

#### ShellBack

- Provides believable thickness in entry/transition angles.
- May be simpler than the front but cannot be absent if the device rotates or slides into view.
- No open backfaces visible at supported camera angles.

#### ShellSideTrim

- Separates the body from the screen assembly.
- May use darker plastic or restrained metallic trim.
- Must not create excessive material slots.

#### Speaker perforations

Preferred implementation:

- actual modeled or instanced holes where silhouette/parallax is visible;
- optimized perforated insert mesh is acceptable;
- normal-map-only holes are allowed only when they read convincingly at the final camera distance.

### 5.3 Display assembly

#### ScreenGlass

- Separate mesh.
- Clear material.
- Slightly raised above or seated in front of the bezel.
- Rounded corners matching the aperture.
- Receives controlled reflection but does not cast distracting opaque shadows.

#### ScreenGlassEdge

- Optional separate edge strip when required to sell thickness.
- May use darker refraction/tint than the front face.

#### ScreenBezel

- Dark, non-reflective frame that masks the display edges.
- Prevents the screen content from appearing pasted onto the shell.

#### ScreenDisplay

- Separate plane or shallow display mesh.
- Uses the menu-screen RenderTexture/material.
- Slightly recessed behind the cover.
- Emissive enough to read as a lit display, not a printed decal.

#### ScreenReflectionMask

- Optional subtle reflection/gradient layer.
- Must be static or cheaply animated.
- Must not interfere with mouse hit testing or text readability.

### 5.4 D-pad

- A real modeled part with visible height above the shell.
- May be one rocking cross or a visually unified control with four directional interaction zones.
- The center accent can use a restrained purple/blue glow.
- Physical response must indicate direction without producing unrealistic deep travel.
- Each direction needs a distinct hit target for mouse/pointer interaction when physical controls are clickable.

### 5.5 Face buttons

- Separate A, B, X and Y meshes.
- Convex or softly domed tops.
- A/B use warm orange/amber identity.
- X/Y use violet/purple identity.
- Letter marks are geometry, decals or high-quality texture details that remain sharp.
- Buttons require separate rest and pressed transforms.

### 5.6 Center shortcut buttons

Two separate pill-like physical buttons:

- left: `SELECT`;
- right: `EXIT`.

They are not named Select and Start in the final design.

Both must be clickable with the mouse and usable through the input system. Their physical labels must remain readable from the final menu camera.

### 5.7 Labels and decals

- Device labels must be authored as crisp decals, meshes or signed-distance-field text appropriate to the final camera.
- Do not bake incorrect or obsolete labels into a shared shell texture.
- `SELECT` and `EXIT` use identical font size, tracking, depth and recessed two-tone treatment.
- Labels should not glow strongly; the controls provide the feedback.

## 6. Material and texture breakdown

### 6.1 Shell material

Required maps:

- Base Color;
- Normal;
- Mask/packed map for roughness, ambient occlusion and optional metallic value;
- optional detail normal/pattern mask;
- optional emission mask for fantasy accents.

Recommended authoring:

- 4K source texture set for master production;
- 2K runtime default unless profiling supports higher;
- mobile override may use 1K/2K depending on device tier;
- consistent texel density across front and visible sides;
- no visible center seam in the gradient.

### 6.2 Black control plastic

Used for:

- D-pad;
- button skirts;
- bezel details;
- center shortcut caps where appropriate.

Properties:

- low metallic;
- medium roughness;
- subtle micro-surface breakup;
- readable edge highlights.

### 6.3 Purple face-button material

Used for X/Y.

- dark violet body;
- controlled glossy top;
- brighter glyph;
- hover/press emission only when needed.

### 6.4 Orange face-button material

Used for A/B.

- dark warm body;
- amber/orange glyph;
- sufficient contrast against the orange side of the shell through dark skirts/edge separation.

### 6.5 Glass/transparent-plastic material

- transparent or physically plausible alpha blend;
- low tint;
- subtle Fresnel/specular response;
- no expensive distortion unless profiling proves it safe;
- no full-screen blur pass required;
- single shared material instance where possible;
- reflections should be authored to read from the fixed menu camera.

### 6.6 Display material

- unlit/emissive or appropriately lit screen shader;
- accepts menu RenderTexture;
- correct color-space handling;
- no mip shimmer at the fixed camera;
- optional extremely subtle scanline/response texture;
- scanlines must never reduce accessibility or produce moiré.

## 7. Main-menu screen content

The screen retains the approved modern dark-fantasy layout while adapting to the real display aperture.

Required primary entries:

1. Continue, when a valid run exists;
2. New Run / Start Game according to current game-state wording;
3. Progression;
4. Settings;
5. Credits;
6. Quit on supported desktop builds.

Required supporting content:

- Boredom & Dungeons identity;
- current run summary when valid;
- active character-correct Boy/Girl image;
- progression/resources only when backed by real data;
- no fabricated statistics;
- no placeholder online state presented as real functionality;
- control legend based on active input device.

The concept-art values such as level, currency, depth, location names and online status are visual references only unless those values already exist in the game. Implementation must bind to real project data or omit the unsupported field.

## 8. Escape/Pause screen content

The Escape/Pause screen uses the same physical device and screen assembly.

Required behavior:

- Esc opens Pause during a live controllable run.
- Resume is the initial safe selection.
- Existing pause, settings and abandon-confirmation behavior remains authoritative.
- Abandon remains confirmation-gated.
- No gameplay HUD leaks above the menu device.
- The right-side information area may show only real current-run data.

Recommended entries, subject to current implemented availability:

- Resume;
- Inventory only if an inventory page exists;
- Map only if the map page exists;
- Progression only if permitted during a run;
- Settings;
- Controls;
- Return to Main Menu / Abandon Run with confirmation.

Unsupported pages must not be added as dead buttons merely because they exist in concept art.

## 9. Input and focus contract

### 9.1 Required input paths

- Mouse hover and click.
- D-pad/arrow navigation.
- center `SELECT` = activate the focused item.
- center `EXIT` = open the correct quit/abandon confirmation.
- Main Menu X = Start/New Game.
- Main Menu A = Progression.
- Main Menu B = Settings.
- Main Menu Y = Credits.
- On non-main pages B = Back.

### 9.2 One focus model

Mouse and controller/keyboard navigation share one selected item.

- Mouse movement over a valid menu item updates hover/focus.
- Mouse click activates the hovered item once.
- D-pad/arrow input moves focus through the deterministic navigation graph.
- After D-pad/arrow input, focus remains visible even when the pointer is elsewhere.
- center SELECT activates the focused item once.
- center EXIT opens the legal quit/abandon confirmation once.
- B opens Settings only on Main Menu and returns on every non-main page.
- X/A/Y direct shortcuts are legal only on Main Menu.
- Input from two devices in the same frame must not double-activate an action.

### 9.3 Physical-device hit testing

The visible physical controls may be clicked directly with the mouse.

- Clicking D-pad directions performs navigation.
- Clicking X starts the Main Menu game action.
- Clicking A opens Progression from Main Menu.
- Clicking B opens Settings on Main Menu and goes Back elsewhere.
- Clicking Y opens Credits from Main Menu.
- Clicking center SELECT activates the highlighted option.
- Clicking center EXIT opens the appropriate exit or abandon confirmation.

Physical-device clicks and on-screen UI clicks call the same semantic actions. They must not create parallel menu state logic.

## 10. Tactile animation and interaction

All values below are starting ranges and must be tuned in Unity at the final camera distance.

### 10.1 Face-button press

- physical travel: visually equivalent to approximately 0.8–1.5 mm;
- press-down duration: 0.04–0.08 seconds;
- release duration: 0.08–0.14 seconds;
- easing: fast tactile compression, slightly softer return;
- no overshoot that looks rubbery unless deliberately subtle;
- press state synchronizes with the semantic action.

### 10.2 D-pad response

- subtle directional rock/tilt or directional compression;
- only the active direction moves;
- center remains visually connected;
- no diagonal ambiguity in menu navigation unless diagonal navigation is explicitly supported;
- release returns cleanly to rest.

### 10.3 Center-button response

- short pill-button travel;
- SELECT and EXIT use the same tactile language;
- button can highlight on pointer hover before press;
- visible pressed state persists for at least one rendered frame.

### 10.4 On-screen focus response

- selected row uses a controlled neon-blue border/glow and slight lift/depth;
- hover/focus response is short and non-flashy;
- focus movement must remain readable without relying only on color;
- selected state includes shape/border/icon movement or another non-color cue.

### 10.5 Screen/device transition

- device may enter with a restrained position/scale/rotation settle;
- no prolonged showcase before the player can interact;
- Pause should appear quickly enough to feel responsive;
- main menu may use a slightly richer reveal after the approved boot sequence;
- screen content and physical device motion stay synchronized.

## 11. Audio and haptics

- Hover/navigation uses a short restrained device click.
- Confirm uses a slightly firmer click.
- Back uses a distinct softer return click.
- SELECT and EXIT center buttons may have subtle differentiated hover feedback but retain one shared physical material family.
- Physical button animation and sound start together.
- Repeated held navigation follows the input repeat policy and does not create audio spam.
- Haptics may be used on supported controllers/mobile devices, but must respect settings and never be required for meaning.
- All sound routes through the existing audio ownership/mixer policy.

## 12. Target runtime architecture

This is the required target architecture. It is not proof that the classes already exist.

### 12.1 State and behavior owner

`BDMainMenuFlow` remains the single owner of:

- active menu mode/page;
- selected/focused item;
- legal actions;
- settings/progression navigation;
- pause/resume/abandon behavior;
- input semantics;
- current run/menu data binding.

### 12.2 3D presentation owner

A dedicated view/presenter, preferably by extending or replacing only the presentation responsibility of `BDGameBoyMenuShell`, owns:

- 3D handheld prefab instance;
- device camera pose;
- screen/glass/body materials;
- button transform animation;
- physical hit targets;
- original and awakened shell palettes;
- visibility and cleanup.

It must not own run state or create a competing menu controller.

### 12.3 Screen-content view

The menu-screen view owns visual widgets and sends semantic actions back to `BDMainMenuFlow`.

Preferred rendering target:

- a dedicated menu-screen Canvas/UI view rendered to one RenderTexture;
- the RenderTexture is displayed by the `ScreenDisplay` material behind the glass;
- the menu content is not baked into the shell texture.

A front-only transitional implementation may align a screen-space view with the 3D aperture, but the final architecture must support physical depth and any approved device angle without visual drift.

### 12.4 Input routing

One input adapter maps mouse, keyboard, controller and physical-device hit targets to semantic menu commands:

```text
NavigateUp
NavigateDown
NavigateLeft
NavigateRight
Confirm
Back
OpenSettings
OpenProgression
PointerMove
PointerClick
```

No individual view creates a second navigation state.

## 13. Performance and memory contract

- One active 3D device instance per menu owner.
- One active screen RenderTexture per visible device.
- Disable or release expensive screen rendering when the menu is fully hidden.
- No mesh, material, RenderTexture, Texture2D or event-system allocation per frame.
- Use MaterialPropertyBlock only when needed and initialize Unity native objects outside constructors/field initializers.
- Button animation is transform/material-property based; no instantiation per press.
- Reuse shared materials and texture sets.
- Keep transparent layering minimal: glass, optional reflection layer and display only.
- Profile overdraw caused by the glass and screen.
- Desktop reference screen target may start at 1536–2048 pixels on the long axis.
- Mobile/tiered target may start at 768–1280 pixels on the long axis.
- Final RenderTexture resolution must be selected by readability and profiling, not hard-coded from concept-art dimensions.
- Pause opening must not cause a visible shader/material compilation hitch after warm-up.
- Prewarm required shaders/material variants where appropriate.

## 14. Accessibility and responsive behavior

- Text remains readable behind the glass layer.
- Reflections never cover selected text.
- Focus is visible without color alone.
- Mouse targets and screen buttons have forgiving hit areas.
- Controller navigation order is deterministic.
- UI scale supports desktop and landscape mobile gameplay even though the physical device itself is portrait-oriented.
- The device may scale/letterbox within the gameplay viewport; no control or screen content may be clipped.
- Safe-area rules apply on mobile.
- Input legends adapt to the active input family and do not falsely show unavailable hardware bindings.

## 15. Asset deliverables

### 15.1 3D

- device shell high/production mesh;
- optimized runtime mesh;
- screen glass mesh;
- bezel and display meshes;
- D-pad;
- A/B/X/Y buttons;
- Select/Exit center buttons;
- speaker/perforation geometry or approved optimized substitute;
- collision/hit-target meshes;
- menu camera anchors;
- final prefab with stable `.meta` files.

### 15.2 Textures/materials

- shell texture set;
- control-plastic texture/material;
- purple button material;
- orange button material;
- glass material;
- display material;
- optional engraving/emission masks;
- original and awakened palette configuration.

### 15.3 UI

- main-menu screen layout;
- pause/Escape screen layout;
- Settings page integration;
- Progression page integration;
- abandon confirmation integration;
- input legend;
- real data bindings;
- Boy/Girl paired image assets.

### 15.4 Reference package

The approved reference images are stored under:

`ProjectGuide/References/Visual/ModernHandheld3D/`

The product renders remain direction references. The separately supplied orthographic/front texture sheet is approved as a masked front-surface decal on the real shell, with transparent cutouts for the live screen and all modeled controls. It may not replace the mesh, glass, display or physical controls, and it may not bake live menu text into the shell.

## 16. Implementation phases

### Phase A — Data/ownership audit

- inspect current `BDMainMenuFlow`, `BDGameBoyMenuShell`, visibility owner and input paths;
- list every current menu page/action;
- verify what real progression/current-run data exists;
- preserve all current behavior;
- confirm Boy/Girl identity source.

### Phase B — 3D shell vertical slice

- import/create device mesh hierarchy;
- establish menu camera and framing;
- implement shell, controls, screen glass and display depth;
- show existing menu content on/in the display without changing behavior;
- create near-immediate test access.

### Phase C — Interactive controls

- implement physical hit targets;
- implement button press animation;
- route D-pad/A/B/X/Y and center buttons to semantic actions;
- synchronize mouse and focus navigation;
- prevent duplicate activation.

### Phase D — Main menu content

- adapt main screen hierarchy;
- replace user-facing `Meta Progression` with `Progression`;
- bind real data only;
- integrate Boy/Girl paired art selection;
- verify Continue/New Run legality.

### Phase E — Pause/Escape content

- use the same device/presenter;
- preserve Resume and abandon confirmation;
- bind only supported pages;
- confirm no gameplay UI leakage.

### Phase F — materials, audio and polish

- tune blue/orange shell;
- tune glass/reflections;
- tune tactile travel and easing;
- add audio/haptics through existing systems;
- implement awakened palette.

### Phase G — QA and optimization

- automated validation;
- compilation;
- TEST EVERYTHING;
- focused main/pause interaction testing;
- boy/girl parity audit;
- performance profiling;
- desktop/mobile-like layout verification;
- user visual acceptance.

## 17. Acceptance gate

The task is complete only when all of the following are true:

1. The handheld is a real 3D model with visible thickness.
2. Shell, glass, display, D-pad, A/B/X/Y and center buttons are separate modeled parts.
3. The shell uses the approved blue-to-orange gradient.
4. The screen visibly sits behind a clear cover with believable depth.
5. Reflections do not reduce readability.
6. Main and Pause/Escape use the same device system.
7. Mouse hover/click works.
8. D-pad/arrow navigation works.
9. Main Menu X starts New Game, A opens Progression, B opens Settings and Y opens Credits.
10. B goes back on every non-main page.
11. Center SELECT activates the highlighted row.
12. Center EXIT opens the correct quit/abandon confirmation.
13. Physical controls visibly press and release.
14. On-screen and physical controls call the same semantic actions.
15. No duplicate activation occurs when devices overlap.
16. `Progression` is used instead of `Meta Progression` and remains on one line.
17. Every Boy image has a matching Girl version with identical layout metadata.
18. Runtime selects the image matching the active character.
19. No unsupported concept-art statistic or online feature is presented as real.
20. Existing menu, settings, pause, abandon and true-victory behavior is preserved.
21. No gameplay UI leaks above the device.
22. No per-frame native-object/material/texture allocation is introduced.
23. Unity compiles without new errors.
24. TEST EVERYTHING reports 0 blockers, 0 warnings and 0 info.
25. Focused Play Mode tests pass on repeated open/close, restart and scene transitions.
26. Performance is profiled on desktop and representative mobile settings.
27. The user approves the visual and tactile result.

## 18. Non-goals

- Do not copy Nintendo/Game Boy branding or exact industrial design.
- Do not make concept-art statistics functional by inventing backend/game systems.
- Do not create a second menu state owner.
- Do not keep the final device as a flat screenshot or one-piece textured quad.
- Do not bake menu text into the shell texture.
- Do not add dead Inventory/Map/Online buttons if their systems do not exist.
- Do not remove existing menu behavior or effects.
- Do not treat Girl art as optional after Boy art is created.

## 19. Exact continuation point

The procedural vertical slice, screen RenderTexture, glass, physical controls, masked shell decal, uploaded wood table, gradual focus shader, contained screen pages/transitions and deterministic Boy/Girl artwork are implemented in the current working tree. The next step is not additional speculative design: install the full-project ZIP, let Unity import/compile, run `TEST EVERYTHING`, and complete the focused visual/input/performance/user acceptance gate. Any failure reopens this task and is repaired before returning to the saved Runtime queue.

## V4 selection-card and input addendum

- The small memory card exists only when a fresh Start Game/New Run row is selected.
- It is text-only: no duplicate artwork and no Boy/Girl route or Mother state.
- Only the large New Game hero panel resolves the active Boy/Girl pair.
- W/A/S/D navigate identically to arrow keys.
- Glass glint is limited to the upper-right light-facing region and remains subordinate to UI readability.
- Shadow stack: soft left penumbra, denser short core and tight contact shadow.

## V5 final control/layout/product-shot addendum

- The device sits slightly higher in the table composition.
- The left shadow stack and upper-right glass glint are strengthened but remain short/readable and non-obscuring.
- Main Menu hero art and its small text card are stacked vertically with a deliberate gap and no clipping.
- Page layouts use a common left-content/right-art grid and a concise footer that does not wrap.
- Control textures are alpha-cleaned from the approved source sheet so each 3D cap shows only its own button surface rather than neighboring controls or square crop backgrounds.
- This final addendum supersedes V4/prototype control labels and mappings.

<!-- BND_INTRO_TO_MAIN_MENU_CINEMATIC_AND_TUTORIAL_SPACING_V105:BEGIN -->
## Post-intro cinematic camera mode

The special post-BBH shot starts farther away and at a different angle, exposes more table, keeps the real screen active and moves to the exact ordinary camera transform/FOV. It is a camera path, not device scaling or a camera cut.
<!-- BND_INTRO_TO_MAIN_MENU_CINEMATIC_AND_TUTORIAL_SPACING_V105:END -->
