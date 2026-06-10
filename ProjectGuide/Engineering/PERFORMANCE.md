
## V5 control and glass cost contract

Button-cap and glass-glint materials/textures are loaded once from Resources and cached. No texture cropping, material creation, mesh creation or label generation occurs per frame. Press feedback moves cached transforms only. The dedicated glint is one transparent quad and must remain confined to the upper-right screen edge.

# Performance Guidelines — Measurement and Optimization Contract

## Modern handheld V4 material and shadow budget

- The shell uses one cached generated mesh set and one object-gradient material; no full-face transparent decal draw is allowed.
- Shadow presentation uses three cached quads/materials (penumbra, core, contact) and performs no per-frame allocation.
- Glass glint is evaluated in the existing glass shader and does not spawn lights, particles or textures.
- WASD checks reuse the existing input poll and allocate nothing per frame.
- Page selection only toggles the cached New Game card and changes existing texture references.


This document defines how performance-sensitive work is evaluated. It does not invent unapproved numeric budgets. Approved platform budgets and current performance blockers belong in `ProjectGuide/Status/CURRENT.md`.

## Current platform context

- Engine: Unity 6000.0.76f1.
- Development harness: desktop/editor with keyboard and mouse.
- Final product target: mobile landscape.

Final device tiers, target FPS, memory ceilings, loading budgets, and thermal expectations must be explicitly approved and recorded before release optimization is declared complete.

## Measure before claiming improvement

For a performance-relevant change, capture applicable before/after evidence:

- CPU frame time;
- GPU frame time;
- main-thread and render-thread spikes;
- managed allocations and garbage collection;
- total/managed/native memory;
- texture, mesh, animation, and audio memory;
- draw calls, batches, SetPass calls, overdraw, and shadow cost;
- scene load and transition time;
- object-instantiation spikes;
- physics queries and collision cost;
- per-frame searches, reflection, and repeated hierarchy scans.

Do not claim a performance pass without real measurements.

## Runtime rules

- Avoid per-frame `Find*` calls unless cached/refreshed deliberately and proven acceptable.
- Avoid repeated allocations in `Update`, `LateUpdate`, physics loops, GUI loops, and combat hot paths.
- Use pooling for frequently spawned projectiles, impacts, damage numbers, enemies, or VFX when profiling justifies it.
- Disable or remove unnecessary `Update` execution when event-driven logic is sufficient.
- Keep editor-only validation and repair logic out of runtime assemblies.
- Prefer bounded physics queries and reusable buffers where scale requires them.
- Avoid duplicate controllers and duplicate state polling.
- Validate mobile-safe UI, effects, shadows, and post-processing under realistic resolution and device load.

## Asset and scene rules

- Preserve original assets; optimize through documented import/settings/pipeline changes.
- Texture asymmetry, UV orientation, tiling, and atlas decisions must not assume mirrored source art unless the art contract explicitly allows it.
- Track large textures, meshes, audio clips, animation data, and shader variants.
- Use additive/async loading only when it matches the approved scene architecture.
- Temporary test content must not ship enabled in production progression.

## Performance change record

Each measured optimization should record in `ProjectGuide/Status/CURRENT.md`:

- test device/environment;
- scene and reproduction path;
- before measurement;
- after measurement;
- visual/gameplay trade-offs;
- remaining bottleneck;
- whether the result is verified or still requires device testing.

## V23R19Q menu presentation budget

- Boot and menu procedural textures are generated once per owning component lifecycle.
- No texture/material generation is allowed in `OnGUI`.
- Scanlines must be tiled with one draw call rather than one call per line.
- The professional shell should add only a small fixed number of IMGUI draws independent of resolution.
- Mode transitions use `Time.unscaledTime` and do not allocate coroutines or per-transition textures.
- Verify no recurring GC allocation attributable to the new polish while the menu is idle.

<!-- B&D MODERN 3D HANDHELD PERFORMANCE START -->
## 3D handheld menu performance contract

- Keep one active device instance and one active screen RenderTexture per visible menu.
- Do not allocate meshes, materials, Texture2D, RenderTexture, event handlers or Unity native objects per frame.
- Button press feedback uses cached transforms/material properties and creates no temporary GameObjects.
- Reuse shared shell/control/glass/display materials; avoid one material per button unless a measured requirement justifies it.
- Keep transparent layers minimal: glass, optional reflection layer and the display.
- Profile glass/screen overdraw and disable hidden menu rendering.
- Select RenderTexture resolution by readability and target-device profiling; use tiered desktop/mobile resolution rather than the concept-art source size.
- Prewarm menu shaders/material variants so first Pause open does not hitch.
- Menu close/reload cleans listeners, RenderTextures and presenter references deterministically.
- Import visual reference images as documentation assets only; they are not loaded by Runtime.
<!-- B&D MODERN 3D HANDHELD PERFORMANCE END -->

## Implemented handheld performance boundaries

- One presenter instance survives scene loads; duplicate instances self-destruct.
- Meshes, materials, rounded UI texture, click clip and the screen RenderTexture are created once and released deterministically.
- The screen RenderTexture is fixed at `960×1080` for this verification slice; mobile tiering remains a profiling follow-up.
- Pointer raycasts occur only while the device is visible and use one dedicated layer mask.
- Menu screen hierarchy rebuilds only on page transitions, not each frame.
- Button motion and material-property feedback use cached transforms and cached `MaterialPropertyBlock` instances.
- Character textures load once from `Resources`; route changes swap cached references rather than reloading each frame.
- Loading percentage is the only intentionally live text update; idle Main/Pause must show no recurring allocation from navigation or button animation in the Profiler.
- Table depth-of-field styling is one material sampling a sharp and a prefiltered version of the same source texture; it does not allocate render targets or run a per-frame CPU blur.
- Shell microtexture, glass reflection, shadow masks and contextual artwork are loaded once from `Resources`. The obsolete full-face decal asset/shader are removed so they cannot add a draw call, build size or overlap risk.
- `BDPlayableCharacterIdentity` performs scene discovery only on explicit scene/visibility refresh, not every presenter frame.
- Four D-pad targets animate four separate cached cap transforms; no Rigidbody/physics simulation or temporary object is created per press.

<!-- B&D FIRST LAUNCH HANDHELD PERFORMANCE V1 START -->
## First-launch tutorial and handheld repair budgets

- No hierarchy-wide `FindObjectsByType`, LINQ, material creation, texture creation, mesh creation, filesystem access or PlayerPrefs read/write occurs in the tutorial frame loop.
- Tutorial page objects and hit targets are created once per page build and destroyed by the existing page cleanup.
- Persistent-control references use the existing cached list.
- Physical hover performs no material-property-block write. Press and tutorial guidance update only cached transforms.
- Tutorial state writes occur only on entry and terminal completion/skip.
- After `Completed` or `Skipped`, the tutorial has no normal-session idle allocation or update cost beyond one cheap state predicate during page resolution.
- Device and shadow placement share one authoritative constant; no LateUpdate hierarchy correction layer remains.
<!-- B&D FIRST LAUNCH HANDHELD PERFORMANCE V1 END -->

<!-- B&D CONTEXTUAL HUD + MINIMAP PERFORMANCE V2 START -->
## Contextual HUD and minimap performance

- HUD alpha decisions are scalar updates using cached references and unscaled time.
- Minimap combatants are refreshed on a throttled cadence rather than discovered in `OnGUI` or per raster pixel; marker rendering is limited to discovered rooms.
- Marker drawing reuses the existing raster buffers and creates no per-frame textures/materials.
- Horse healing owns one bounded particle system and one material, both reused for the horse lifetime and destroyed with the component.
- Repository maintenance hashes files by streaming chunks and never loads large binaries into memory as one buffer.
<!-- B&D CONTEXTUAL HUD + MINIMAP PERFORMANCE V2 END -->
