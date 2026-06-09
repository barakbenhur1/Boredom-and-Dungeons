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
- No success claim is allowed before Unity compilation, both handheld QA commands, focused Play Mode inspection and user visual acceptance.

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
- Their click colliders and recessed labels move with them.
- V6 pins only their planar X/Y positions after the original control update, preserving the original Z-axis press/release animation and preventing drift back to the old layout.

### Settings icon

The Settings row requests the gear glyph first, then a menu/settings mark, then a guaranteed visible text fallback. An unsupported Runtime font can no longer leave an empty colored square.

### Unified tactile response

The existing control target remains the owner of pulse timing, hover state, emission and release travel. V6 adds a compatible visual layer to the generated moving parts. It reads the existing pressed emission state, distinguishes it from hover, and normalizes:

- `0.12` visible pressed depth;
- `8.5` response speed;
- `7.5%` pressed scale compression;
- unscaled-time response while paused.

The visual layer never owns release position, so controls cannot remain stuck after a pulse. Screen rows are excluded from physical compression.

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
Assets/_Project/Scripts/Editor/Validation/BDModernHandheldV6QA.cs
ProjectGuide/Tasks/ACTIVE/MODERN_HANDHELD_V6_LAYOUT_AND_PAUSE_POLISH.md
ProjectGuide/QA/MODERN_HANDHELD_V6_QA.md
ProjectGuide/Status/MODERN_HANDHELD_V6_REVIEW.md
```

Every new Unity C# source has a tracked `.meta` file.

## Existing files changed

- `ProjectGuide/INDEX.md`: adds the V6 task and QA records to the maintained index.
- Existing gameplay, `BDMainMenuFlow`, `BDModernHandheld3DPresenter`, scenes, prefabs, artwork, textures and shaders remain unchanged.

## Deleted files

- Production files deleted: none.
- No existing asset or document was replaced.

## Verification tooling

`Boredom And Dungeons → Validate Modern Handheld V6 Polish` checks the actual Runtime sources and maintained V6 documents rather than validating its own token list. It is also a build preprocessor gate. This focused check complements, but does not replace, the project-wide `TEST EVERYTHING` command.

## Required verification

1. Open the project in Unity and wait for a clean compile.
2. Run `Boredom And Dungeons → Validate Modern Handheld V6 Polish`.
3. Run `Boredom And Dungeons → TEST EVERYTHING` outside Play Mode.
4. Require `0 blockers`, `0 warnings`, `0 info` unless a newly documented unrelated infrastructure warning is explicitly accepted.
5. Inspect Main Menu at the real target aspect ratios:
   - device lower but not clipped;
   - image top aligned to Start Game row top;
   - contextual card visible and correct for all five selections;
   - `THE MAZE AWAITS` and all other card text fully contained;
   - Settings icon visible;
   - SELECT/EXIT centered, close and unobstructed.
6. Press every D-pad direction, SELECT, EXIT, X, Y, A and B by mouse and keyboard/gamepad paths. Confirm equal tactile depth, compression, release and rapid-repeat recovery.
7. Enter gameplay and press Escape. Confirm Pause reads as an internal handheld menu and all four actions retain correct semantics and click areas.
8. Recheck Settings, Progression, Credits, exit confirmation and abandon confirmation for clipping at narrow and wide resolutions.
9. Confirm no gameplay HUD leaks above menu pages and no legacy menu competes with the 3D presenter.

## Exact resume point

Do not merge this branch into `main` yet. First complete Unity compilation, both QA commands, focused Play Mode verification and user visual approval. Record real results in `ProjectGuide/Status/CURRENT.md`, `ProjectGuide/Status/VERIFICATION.md`, `ProjectGuide/QA/HISTORY.md` and the V6 checklist before merge.
