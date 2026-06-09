# Active Task — Real 3D Handheld Main Menu and Pause UI

```text
Status: CURRENT / DESIGN APPROVED / IMPLEMENTATION NOT STARTED
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
- Boy/Girl artwork parity is mandatory and runtime selection follows the active character.

## Performance contract

- The device mesh, glass, buttons and screen materials are created/cached once; no per-frame mesh, material, texture or RenderTexture creation is allowed.
- Pointer and D-pad focus updates are event/state driven; do not scan the scene or allocate collections every frame.
- Button travel uses bounded transform/material animation and must not add physics simulation or a second gameplay input owner.
- Screen rendering uses one deliberately sized render target per active device presentation, released on teardown and not silently duplicated between Main and Pause.
- Glass reflections, transparency and emissive treatment must remain readable and scalable on desktop and landscape mobile.
- Performance acceptance requires profiling in the real menu and Pause states, stable frame pacing, no orphaned render targets/material instances and no recurring GC allocations from navigation or button animation.

## Required implementation order

1. Audit existing menu, pause, input, UI rendering and scene ownership.
2. Decide the minimum additive 3D presentation architecture without duplicating state owners.
3. Build the device hierarchy, materials, screen render target and interaction colliders.
4. Build Main Menu screen content using real current data only.
5. Build Pause screen content using real current data only.
6. Add mouse and hardware-style navigation/activation.
7. Add tactile animation, sound, glass depth and accessibility states.
8. Add deterministic Boy/Girl paired art selection.
9. Add/adjust TEST EVERYTHING checks.
10. Run compile, automated QA, focused Play Mode, performance and user acceptance.

## Canonical asset specification

[`../../Production/ModernHandheld/MODERN_HANDHELD_3D_SPEC.md`](../../Production/ModernHandheld/MODERN_HANDHELD_3D_SPEC.md)

## Exact resume point

After this guide-reorganization package is installed and verified, inspect the current menu/pause/input code and create the smallest testable 3D vertical slice. Do not start from a flat image and do not remove protected existing menu behavior.
