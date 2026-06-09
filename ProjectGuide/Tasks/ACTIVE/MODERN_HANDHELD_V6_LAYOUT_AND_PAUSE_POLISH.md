# Modern Handheld V6 — Layout, Context Card, Tactile and Pause Polish

```text
Status: IMPLEMENTED ON REVIEW BRANCH / UNITY VERIFICATION REQUIRED
Branch: codex/handheld-v6-layout-polish
Base: c8bf6e39dbe4ebb46088e2fbdf418716fe62228e
Scope: additive presentation repair only
```

## User-approved correction set

This task records the focused Play Mode corrections requested on 2026-06-09:

1. Lower the physical handheld slightly in the table composition so it is not tight to either edge and reads a little closer to the bottom.
2. Keep the top of the large contextual image aligned with the top of the first Main Menu row.
3. Keep the second information card below the image for every Main Menu selection and change its copy to match the selected action.
4. Repair the clipped `THE MAZE AWAITS` card and apply safe text bounds to Main, Settings, Progression, Credits and confirmation pages.
5. Move the physical SELECT and EXIT controls inward and closer together without overlapping labels, the D-pad or the face buttons.
6. Guarantee a visible Settings icon even when the Runtime font does not contain the gear glyph.
7. Give D-pad, SELECT and EXIT the same depth-and-compression press language as X/Y/A/B.
8. Present Escape/Pause as an internal handheld menu rather than reusing the Main Menu product layout.

## Protected ownership and non-goals

- `BDMainMenuFlow` remains the only semantic owner of Main, Pause, Settings, Progression, abandon, resume and run state.
- `BDModernHandheld3DPresenter` remains the generated device and screen owner.
- V6 does not create a second menu state machine, alter input mappings, change gameplay, delete assets, replace approved artwork or modify run behavior.
- The work is additive. Existing files and approved assets remain intact.
- No success claim is allowed before Unity compilation, `TEST EVERYTHING`, focused Play Mode inspection and user visual acceptance.

## Implementation

### Product-shot composition

`BDModernHandheldV6Polish` installs after the existing presenter and creates one parent offset for the generated device and its shadow group. The offset is `-0.34` on local Y. The table remains fixed, so the handheld moves lower while its contact and cast shadows stay physically attached.

### Main Menu image and contextual card

- The hero frame is moved so its top aligns with the first Main Menu row.
- The existing text-only memory card is kept below the hero image for every Main Menu option.
- Context copy is deterministic:

| Selected option | Card heading | Card body |
|---|---|---|
| Start Game | `ADVENTURE MEMORY` | `A new path is ready` |
| Progression | `PERSISTENT MEMORY` | `Milestones and future upgrades` |
| Settings | `SYSTEM CONFIGURATION` | `Audio, controls and display` |
| Credits | `BEHIND THE ADVENTURE` | `People, ideas and production` |
| Quit | `LEAVE THE HANDHELD` | `Exit only after confirmation` |

The card remains text-only. Only the large Start Game hero image may use the active Boy/Girl pair.

### Safe text and card bounds

- The bottom information card receives corrected internal heading/body rectangles.
- Page titles, subtitles, row labels, badges and footer copy use wrapping and bounded best-fit behavior.
- Progression, Credits, Settings-art and confirmation panels clamp child text to their own bounds.
- The correction is shared across generated pages rather than special-casing only the screenshot.

### Physical SELECT and EXIT layout

- SELECT moves to local X `-0.66`, EXIT to `+0.66`.
- Both move to local Y `-3.82`.
- Their click colliders move with them.
- Their recessed labels move to the matching centered positions below the buttons.

### Settings icon

The Settings row first requests the gear glyph. If the active Runtime font does not contain it, the polish layer supplies a visible deterministic fallback instead of leaving an empty colored square.

### Unified tactile response

The existing control target still owns press timing. V6 adds `BDModernHandheldPressScaleFeedback` to each generated physical moving part through a compatibility bridge. D-pad directions, SELECT, EXIT and X/Y/A/B therefore share:

- `0.12` visible press depth;
- `8.5` response speed;
- `7.5%` pressed scale compression;
- unscaled-time animation so Pause remains responsive.

Screen rows are excluded from physical compression.

### Escape/Pause internal menu

The Pause page suppresses the Main-style hero and run cards, reuses the existing screen panel language as one internal menu frame, centers the four Pause actions, and moves the corresponding physical screen hit areas with the rows. This preserves mouse/controller parity and keeps every Pause element inside the device glass.

## Files added

```text
Assets/_Project/Scripts/Runtime/UI/BDModernHandheldV6Polish.cs
Assets/_Project/Scripts/Runtime/UI/BDModernHandheldV6Polish.Physical.cs
Assets/_Project/Scripts/Runtime/UI/BDModernHandheldV6Polish.MainPage.cs
Assets/_Project/Scripts/Runtime/UI/BDModernHandheldV6Polish.PausePage.cs
Assets/_Project/Scripts/Runtime/UI/BDModernHandheldV6Polish.PauseStyle.cs
Assets/_Project/Scripts/Runtime/UI/BDModernHandheldV6Polish.Layout.cs
Assets/_Project/Scripts/Runtime/UI/BDModernHandheldV6Polish.Helpers.cs
Assets/_Project/Scripts/Runtime/UI/BDModernHandheldV6Polish.ItemLayout.cs
Assets/_Project/Scripts/Runtime/UI/BDModernHandheldV6Polish.AreaAlignment.cs
Assets/_Project/Scripts/Runtime/UI/BDModernHandheldV6Polish.Tactile.cs
Assets/_Project/Scripts/Runtime/UI/BDModernHandheldPressScaleFeedback.cs
Assets/_Project/Scripts/Runtime/UI/BDModernHandheldTactileCompatibility.cs
```

## Files changed or deleted

- Existing gameplay and menu-authority files: unchanged.
- Existing artwork, textures, shaders and prefabs: unchanged.
- Deleted production files: none.

## Required verification

1. Open the project in Unity and wait for a clean compile.
2. Run `Boredom And Dungeons → TEST EVERYTHING` outside Play Mode.
3. Require `0 blockers`, `0 warnings`, `0 info` unless a newly documented unrelated infrastructure warning is explicitly accepted.
4. Inspect Main Menu at the real target aspect ratios:
   - device lower but not clipped;
   - image top aligned to Start Game row top;
   - contextual card visible and correct for all five selections;
   - `THE MAZE AWAITS` and all other card text fully contained;
   - Settings icon visible;
   - SELECT/EXIT centered, close and unobstructed.
5. Press every D-pad direction, SELECT, EXIT, X, Y, A and B by mouse and keyboard/gamepad paths. Confirm equal tactile depth, compression and return.
6. Enter gameplay and press Escape. Confirm Pause reads as an internal handheld menu and all four actions retain correct semantics and click areas.
7. Recheck Settings, Progression, Credits, exit confirmation and abandon confirmation for clipping at narrow and wide resolutions.
8. Confirm no gameplay HUD leaks above menu pages and no legacy menu competes with the 3D presenter.

## Exact resume point

Do not merge this branch into `main` yet. First complete Unity compilation, `TEST EVERYTHING`, focused Play Mode verification and user visual approval. Record the real results in `ProjectGuide/Status/CURRENT.md`, `ProjectGuide/Status/VERIFICATION.md`, `ProjectGuide/QA/HISTORY.md` and the acceptance checklist before merge.
