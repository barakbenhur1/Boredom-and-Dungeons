# Professional Memory-Handheld UI Modernization V1

## Task identity

- **Task ID:** `C11.UI.V23R19Q`
- **Status:** `IMPLEMENTED / UNITY VISUAL VERIFICATION REQUIRED`
- **Origin:** user request to professionalize the intro screen, main screen and all menu pages without losing any existing content or effects
- **Visual direction:** an original modern handheld interface that feels like the Game Boy remembered through nostalgia, not a literal commercial-device copy

## Why this task exists

The project already has the correct conceptual ingredients:

- BBH boot sequence;
- sequential letters;
- growing metal circle;
- dreamy fantasy backdrop;
- a Game Boy-like shell;
- main menu, settings, pause, abandon confirmation and loading pages;
- true-victory device transformation.

The problem is finish quality rather than missing identity. The existing implementation reads as functional prototype rectangles and independent visual ideas rather than one production-level device and screen system.

This task therefore improves composition, hierarchy, depth, interaction feedback, page transitions and visual consistency while preserving every existing behavior and content path.

## Protected content and behavior

The following must remain:

1. BBH letter order and identity.
2. Initial black hold.
3. Sequential letter appearance.
4. Existing compact depth.
5. Growing filled circle behind the letters.
6. Completion light sweep.
7. Existing boot timing and fade ownership.
8. Dreamy menu backdrop, moon, stars, cloud haze, storybook horizon and golden path.
9. Main-menu title and subtitle.
10. Start Game.
11. Settings.
12. desktop Quit.
13. Pause/Resume.
14. Abandon confirmation.
15. Loading progress.
16. all current settings rows and values.
17. true-victory awakened device state.
18. one integrated IMGUI owner for shell and content.
19. current input, run-flow and gameplay behavior.

## Approved visual direction

### Memory rather than replica

The device should feel like the handheld console people remember:

- chunky;
- friendly;
- tactile;
- instantly readable;
- screen-centered;
- familiar D-pad and two-button language.

It must not reproduce a specific commercial model, logo, exact dimensions, colorway or industrial design.

### Modernization

Modern production finish includes:

- restrained rounded device silhouette;
- layered drop shadow;
- inset screen bezel;
- glass depth and controlled reflections;
- subtle scanline memory;
- crisp high-resolution typography;
- consistent screen palette;
- modern hover/focus response;
- short page slide/fade transitions;
- clear selected, danger, settings and progress accents;
- professional spacing and alignment;
- readable desktop and landscape-mobile scaling.

### Boot intro

Keep the existing BBH/circle animation and add:

- deep graphite-to-navy surface;
- restrained central glow;
- subtle scanline texture;
- soft edge vignette;
- thin internal screen frame;
- no additional narrative text;
- no timing loss;
- no visual clutter.

### Menu device

The device shell receives:

- cached rounded-body texture;
- depth shadow;
- top cartridge ridge;
- inset dark screen housing;
- screen glass;
- small power/status light;
- modern fantasy/handheld label;
- refined D-pad;
- refined A/B buttons;
- Start/Select pills;
- speaker slots;
- awakened cyan/gold variation after true victory.

### Screen content

All menu content remains inside the screen.

- The screen uses a limited deep blue-green/graphite palette with warm readable text.
- Menu actions read as modern high-resolution handheld list entries.
- Hover adds a controlled side bar, border and lift rather than a large color flash.
- Page changes use a short 180 ms slide/fade.
- Pause, Settings, Abandon and Loading use the same screen system.
- Danger remains controlled red; progress uses cool cyan/blue; settings uses violet/cyan.
- UI meaning never depends only on color.

## Performance contract

1. Generated UI textures are created once and cached.
2. No Texture2D or Material is created per frame.
3. Scanlines use one tiled texture draw, not hundreds of line draw calls.
4. Rounded panels reuse one cached alpha texture.
5. Vignette and glow reuse cached textures.
6. Existing one-pass menu ownership remains.
7. The redesign does not add scene searches inside OnGUI.
8. The redesign does not change gameplay clocks, run state or input ownership.

## Implementation summary

### Runtime files

- `BDBBHBootIntro.cs`
  - cached gradient, glow, scanline and vignette;
  - professional boot surface around the preserved BBH sequence.

- `BDGameBoyMenuShell.cs`
  - modern memory-handheld body;
  - screen housing/glass;
  - refined controls;
  - tiled scanline overlay;
  - original and awakened palettes.

- `BDMainMenuFlow.cs`
  - mode label passed to the device screen;
  - 180 ms page transition;
  - modern list-button presentation;
  - professional typography and screen colors;
  - no behavior or option removal.

- `BDDreamyMainMenuBackdrop.cs`
  - restrained focus halo behind the device;
  - all existing backdrop layers preserved.

## Verification required

### Automated

- compilation;
- TEST EVERYTHING 0 blockers, 0 warnings, 0 info;
- cached texture markers;
- integrated shell ownership;
- preserved BBH markers;
- preserved menu actions;
- no obsolete V23R19P QA tokens.

### Visual Play Mode

1. Boot intro:
   - first frame remains black;
   - BBH appears in the same sequence;
   - circle remains behind;
   - completion sweep remains;
   - new surface reads polished but restrained;
   - no frame hitch.

2. Main menu:
   - device reads as one professional handheld;
   - content is visibly inside the screen;
   - backdrop remains visible and supports the device;
   - title and actions are clear;
   - no overlap at 16:9 and landscape mobile-like aspect ratios.

3. Menu pages:
   - Main, Settings, Pause, Abandon and Loading share one visual system;
   - all controls still work;
   - hover and page transition are smooth;
   - text remains readable.

4. True victory:
   - awakened cyan/gold state remains distinct.

## Exact resume point

After automated and visual acceptance:

1. synchronize final verification in `PROJECT_STATUS.md`;
2. remove this active task record after durable content is confirmed in canonical UI/art documents;
3. resume `C01.ARCH.AUDIT.V1` Phase 1.

<!-- B&D V23R19R UI QA INTERRUPTION START -->
## V23R19R automated interruption

The V23R19Q UI implementation is still awaiting automated and visual verification.

Current automated blocker:

- `V23R10_GAME_BOY_MENU_SHELL_MISSING`;
- stale token: `B&D POCKET ADVENTURE`;
- active token: `B&D // POCKET MEMORY`.

V23R19R changes QA only. It does not alter the professional shell presentation.

After TEST EVERYTHING passes, this UI task remains open for full visual, responsive, interaction and performance confirmation listed in this document and in `MASTER_ACTIVE_WORK_SEQUENCE_V1.md`.
<!-- B&D V23R19R UI QA INTERRUPTION END -->

<!-- B&D MODERN 3D HANDHELD REDESIGN START -->
## User-approved V2 redesign — real upright 3D device

**Status:** `DESIGN APPROVED / ASSET SPEC COMPLETE / RUNTIME IMPLEMENTATION REQUIRED`

The earlier V23R19Q flat/procedural professional-memory shell is not the final acceptance target. The user approved a stronger physical-device direction that must be implemented before returning to the previously saved repair/feature queue unless explicitly deferred.

### Approved changes

- upright portrait handheld, screen above controls;
- blue-left/orange-right molded-plastic gradient;
- real 3D shell with volume, bevels and speaker perforations;
- separate glass/transparent-plastic cover over a recessed lit display;
- separate tactile D-pad, A/B/X/Y and center shortcut buttons;
- left center button is Settings;
- right center button is Progression;
- A selects, B returns, X opens Settings, Y opens Progression;
- mouse and D-pad navigation both work;
- physical device controls may be clicked directly;
- `Progression` replaces user-facing `Meta Progression` and remains on one line;
- Main and Escape/Pause use the same device system;
- every Boy image has a matched Girl version with identical layout metadata.

### Canonical implementation contract

`Assets/_Project/Design/UI/MODERN_HANDHELD_3D_ASSET_AND_INTERACTIVE_UI_SPEC_V1.md`

### Approved references

`Assets/_Project/Design/Visual/References/ModernHandheld3D/`

The references define form/material/screen-depth direction. They are not flat production textures and do not authorize a one-piece image-based shell.

### Preserved owners

- `BDMainMenuFlow` remains state/input/action owner.
- The 3D device presenter remains presentation-only.
- Existing pause, abandon confirmation, settings, run-flow and UI visibility rules remain.

### Exact next step

Before Runtime edits, audit the current local `BDMainMenuFlow`, `BDGameBoyMenuShell`, input and visibility implementation. Record conflicts between the current one-pass IMGUI rendering and the approved 3D screen target. Then build a testable 3D vertical slice without inventing unsupported menu data.
<!-- B&D MODERN 3D HANDHELD REDESIGN END -->
