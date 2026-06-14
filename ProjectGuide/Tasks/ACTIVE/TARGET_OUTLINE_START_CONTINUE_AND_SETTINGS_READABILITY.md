# Target Outline, Start/Continue Transition, and Settings Readability

Status: `IMPLEMENTED / UNITY VERIFICATION REQUIRED`
Captured: 2026-06-14

## Ordered user priority

1. Repair target highlighting so only the vulnerable enemy model receives the red silhouette outline.
2. Keep the surrounding enemy ring in its normal authored color and opacity.
3. Make the Settings page readable with larger setting names and current values.
4. Make Settings scroll when the larger rows do not fit, while keeping the selected row visible for mouse, keyboard, and controller navigation.
5. Add a professional Start Game press transition.
6. Preserve Continue as future work until the save/continue system exists.

## Implemented target-outline repair

- Damageable non-trigger colliders define the vulnerable enemy envelope.
- Auxiliary rings and broad flat geometry near the feet are excluded.
- Mixed body/ring renderers are filtered by submesh bounds so support-ring triangles do not enter the generated outline shell.
- The auxiliary ring classifier no longer changes ring color or alpha.
- Target selection, attack range, line-of-fire blocking, damage, and camera ownership are unchanged.

## Implemented Settings readability

- Setting labels use a larger bold font.
- Current values use a larger high-contrast bold font.
- Rows are taller and more widely spaced.
- The page scrolls smoothly and automatically follows the selected row.
- Mouse-wheel scrolling is supported when the active input backend provides it.
- Offscreen rows fade and their physical screen hit targets are disabled until visible.

## Implemented Start Game transition

- Primary activation from the physical X button, Select/Confirm, or the on-screen Start Game row enters one shared transition.
- The pressed control receives tactile response and sound.
- Menu input locks for the transition.
- The device camera moves into the physical screen with a restrained luminous screen commit.
- At the end, the existing `BDMainMenuFlow` remains the authoritative owner that starts or reloads the run.
- No second run-flow or save-flow owner is introduced.

## Future Continue contract

Continue is deliberately not implemented before the save/continue system exists.

When implemented:

- Continue uses the same physical-button and screen-entry visual language as Start Game.
- The transition ends directly at the player's saved position on the map.
- It restores the saved run state and camera context.
- It does not replay the New Game opening, mounted entrance, first-launch tutorial, or fresh-run cinematic.
- No placeholder location or fabricated save state may be used.

## Files changed

- `Assets/_Project/Scripts/Runtime/Combat/BDTargetOutlineVisual.cs`
- `Assets/_Project/Scripts/Runtime/Combat/BDAuxiliaryEnemyRingTransparency.cs`
- `Assets/_Project/Scripts/Runtime/UI/BDModernHandheld3DPresenter.SettingsReadability.cs`
- `Assets/_Project/Scripts/Runtime/UI/BDModernHandheld3DPresenter.StartGameTransition.cs`

## Verification required

1. Unity compilation completes with no errors.
2. `TEST EVERYTHING` is rerun.
3. In Play Mode, only the enemy body is outlined; the ring stays normal.
4. Test a separate-ring enemy and any enemy whose body and ring share one mesh.
5. Open Settings and confirm names and current values are readable at the real game resolution.
6. Navigate every Settings row with mouse, keyboard, and controller; the selected row must remain visible and clickable.
7. Trigger Start Game through X, Select/Confirm, and mouse click; each path must play one transition and start exactly one run.
8. Confirm Pause Resume does not accidentally use the New Game transition.

## Exact resume point

Run Unity compilation and `TEST EVERYTHING`. Repair only evidence-backed compile or runtime failures. After user acceptance, continue to the next queued production task. Continue remains documented-only until the save/continue system is implemented.
