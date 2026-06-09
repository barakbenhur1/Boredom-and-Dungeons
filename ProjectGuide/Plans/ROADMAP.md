# Ordered Product Roadmap

This is a readable roadmap. Exact current truth and resume point remain in `../Status/CURRENT.md` and `../Status/WORK_QUEUE.md`.

## Current

1. Repair the missing `com.unity.ugui` dependency and restore clean Unity compilation.
2. Verify the real 3D upright handheld for Main Menu and Pause/Escape.
3. Verify mouse, D-pad, A/B/X/Y and physical Settings/Progression controls.
4. Verify the Boy/Girl pair only on Start Game / New Run, dedicated neutral artwork on every other option/page, and responsive screen readability.
5. Implement the seamless handheld-screen → gameplay opening handoff and reverse gameplay → handheld return.

## Return immediately afterward

1. Unity rerun for the `MaterialPropertyBlock` initialization repair.
2. Repair target outline so only the damageable enemy model is outlined; the surrounding ring remains normal and subtle.
3. Complete retained Play Mode checks for death, abandon, mounted intro and mounted-hook restrictions.

## Next production stages

1. Enemy attack animations for every active attack-capable archetype.
2. Complete any retained opening-cinematic/dialogue polish not already delivered by the handheld-to-gameplay transition stage.
3. Retained UI, performance and device verification.
4. Resume architecture/gameplay/camera audit.

## Future systems

Detailed approved future work is indexed in `FUTURE_SYSTEMS.md` and the relevant files under `../Features/`.
