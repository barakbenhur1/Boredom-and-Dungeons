# Performance Guidelines — Measurement and Optimization Contract

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
