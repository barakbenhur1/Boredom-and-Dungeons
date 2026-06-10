# Ordered Product Roadmap

This is a readable roadmap. Exact current truth and resume point remain in `../Status/CURRENT.md` and `../Status/WORK_QUEUE.md`.

## Current

1. Repair the missing `com.unity.ugui` dependency and restore clean Unity compilation.
2. Verify the real 3D upright handheld for Main Menu and Pause/Escape.
3. Verify mouse, D-pad, A/B/X/Y and the physical center SELECT/EXIT controls.
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

<!-- BND_FIRST_LAUNCH_TUTORIAL_PRODUCTION_COURSE_V10:BEGIN -->
## Approved immediate ordering update

1. Verify Modern Handheld and V10 first-launch tutorial.
2. Implement persistent Continue, non-destructive Save & Return, protected New Game overwrite and Abandon scoring/result flow.
3. Integrate those four run intents into the professional handheld↔gameplay cinematic transition.
4. Return to retained Runtime blockers, enemy attack animations and the paused architecture audit.
<!-- BND_FIRST_LAUNCH_TUTORIAL_PRODUCTION_COURSE_V10:END -->

<!-- BND_FIRST_LAUNCH_TUTORIAL_ENTRY_GATE_V103:BEGIN -->
## Animation production roadmap addition

### Active after V10.3 launch verification
- complete the tutorial player/horse/enemy/mini-boss animation production pass;
- add or rebuild animated limbs where current pixel models cannot communicate motion;
- synchronize attack, shot, hitbox, damage, knockback, landing, death and respawn events;
- validate movement-speed cadence and eliminate visible sliding.

### Queued main-game production pass
- production-ready rigs or equivalent model structure;
- distinct complete sets for player, horse and materially different enemy roles;
- mounted rider/horse synchronization;
- cleanup, performance and multi-enemy validation.

Neither item is complete until runtime integration, assets, QA and user acceptance are recorded.
<!-- BND_FIRST_LAUNCH_TUTORIAL_ENTRY_GATE_V103:END -->

<!-- BND_FIRST_LAUNCH_TUTORIAL_PROGRESSION_GATE_REPAIR_V104:BEGIN -->
## Tutorial V10.4 sequencing note

The animation production pass remains next, but it begins only after the repaired tutorial course is proven completable in one forward run. V10.4 does not mark the animation roadmap item complete.
<!-- BND_FIRST_LAUNCH_TUTORIAL_PROGRESSION_GATE_REPAIR_V104:END -->

<!-- BND_INTRO_TO_MAIN_MENU_CINEMATIC_AND_TUTORIAL_SPACING_V105:BEGIN -->
## V10.5 sequencing

Verify the intro-to-main-menu cinematic and choice spacing before the full tutorial animation production pass. Main-game animation work remains open.
<!-- BND_INTRO_TO_MAIN_MENU_CINEMATIC_AND_TUTORIAL_SPACING_V105:END -->
