<!-- B&D HANDHELD V6 DIRECT REPAIR BUG LEDGER START -->
## Modern handheld merged-V6 regressions — implementation supplied, Unity verification pending

- **Image alignment regression:** merged V6 lowered the hero image to the Start Game row; repaired by removing that override and retaining title alignment.
- **Physical hover regression:** hardware received blue emission/frame-like feedback; repaired in the control owner with no hover visual and press-only movement.
- **Architecture debt:** V6 used a persistent LateUpdate companion plus compatibility/name lookup classes; accepted behavior is migrated into the presenter/control owners and the companion files are retired.
- **Broken card text:** `THE MAZE AWAITS` heading/body exceeded safe internal bounds; corrected in the authoritative card builder and bounded text creation.
- **Pause duplication:** Escape/Pause reused Main-style hero/run cards; replaced with an internal screen panel.
- **Settings glyph:** unsupported gear fonts could leave an empty square; font-safe deterministic fallback added.
- **Verification status:** static/package checks only until Unity compile, TEST EVERYTHING and Play Mode evidence are returned.
<!-- B&D HANDHELD V6 DIRECT REPAIR BUG LEDGER END -->

# Open Bug Tracker — Current Defect Ledger

> [!IMPORTANT]
> **This document must be updated every time project work discovers a new bug, changes a bug's status, implements a repair, verifies a repair, reopens a bug, or proves that a report is not a bug.**
>
> Update it in the same change set as the related code, QA and `ProjectGuide/Status/CURRENT.md`. It must always describe the most accurate current state. Do not wait until the end of a package or development stage.

This is the maintained focused ledger for current defects. `ProjectGuide/Status/CURRENT.md` remains the only authority for overall ordering, active stage, QA truth and resume point. This tracker must not become a second roadmap.

## Status vocabulary

- `OPEN` — reproduced or reported and not yet repaired.
- `IMPLEMENTED / UNITY VERIFICATION REQUIRED` — code/document repair exists, but Unity compilation and focused Play Mode have not both confirmed it.
- `AUTOMATED PASS / PLAY MODE OPEN` — Unity automated QA passed; focused behavior is still unverified.
- `VERIFIED` — the user or recorded focused test confirmed the repair. Verified bugs are removed from the open table and summarized in `ProjectGuide/Status/CURRENT.md`/Git history.
- `REOPENED` — a previously implemented or verified defect failed again.
- `NOT A BUG / SUPERSEDED` — evidence proved another owner or newer requirement replaced the report.

## Current open bugs

| ID | Area | Current status | Current truth / acceptance condition |
|---|---|---|---|
| `UI-HANDHELD-3D-010` | Final control mapping, page alignment, control-cap texture cleanup and product-shot visibility | `IMPLEMENTED / UNITY VERIFICATION REQUIRED` | User review found labels over controls, misaligned screen pages, clipped New Game cards, unreadable shadow/glass response, flat untextured controls and incorrect shortcut semantics. Final repair maps Main X=New Game, A=Progression, B=Settings, Y=Credits; non-main B=Back; center SELECT/EXIT; alpha-cleans approved control textures; raises device; strengthens short-left shadow/upper-right glint; aligns page grid/footer; and vertically separates New Game cards. Acceptance requires TEST EVERYTHING 0/0/0 and user visual/input confirmation. |
| `UI-HANDHELD-3D-009` | WASD parity and directional glass glint | `IMPLEMENTED / UNITY VERIFICATION REQUIRED` | W/A/S/D now participate in release gating and navigate exactly like the four arrows. Glass adds only a restrained upper-right glint aligned with the approved key light; it may not wash over central UI. Acceptance requires all eight directional keys to behave identically and the glint to remain subtle/readable at all menu pages. |
| `UI-HANDHELD-3D-008` | New Game-only detail card leaked route/Mother identity and duplicate art | `IMPLEMENTED / UNITY VERIFICATION REQUIRED` | The small right-side card is now text-only, contains no Boy/Girl/Mother wording, contains no duplicate image, and is visible only when a fresh Start Game/New Run row is selected. It hides for Progression, Settings, Credits, Quit, Pause and active-run Continue. The top bar is neutral `ADVENTURE SYSTEM`. |
| `UI-HANDHELD-3D-007` | Flat decal, weak volume and invisible table shadow | `REOPENED / REPAIR IMPLEMENTED / UNITY VERIFICATION REQUIRED` | Automated QA passed, but user Play Mode evidence showed the full-face decal crossing controls, insufficient molded depth and no readable short left shadow. V4 removes the runtime decal, uses a procedural/object-space molded shell material, increases body depth, adds outer bevel/rear core/side seams, and uses dedicated soft/core/contact shadow layers. Acceptance requires a believable 3D product with clean controls and a short soft shadow cast left. |
| `UI-HANDHELD-3D-006` | New Game art reused on Progression/other options | `IMPLEMENTED / UNITY VERIFICATION REQUIRED` | Only Start Game / New Run may use the Boy/Girl pair. Progression, Settings, Credits, Quit/Return, Resume/Pause and confirmation now resolve to dedicated character-neutral assets. Main/Pause selection changes the preview contextually. Acceptance requires no protagonist on any non-New-Game option and correct active-character art on New Game. |
| `UI-HANDHELD-3D-005` | Low-resolution shell/decal and overlapping center labels/title layout | `REOPENED / SUPERSEDED BY UI-HANDHELD-3D-007` | Label/title fixes remain, but the V3 full-face decal approach passed automation and failed visual acceptance. Runtime decal rendering is removed in V4; shell quality is now owned by molded geometry plus the surface shader. |
| `UI-HANDHELD-3D-004` | Blank device display, immediate Pause close, overlapping/reversed labels and incomplete XYAB click targets | `IMPLEMENTED / UNITY VERIFICATION REQUIRED` | User Play Mode evidence showed no live menu pixels inside the device screen, Escape Pause appeared only briefly, physical labels overlapped or duplicated, and only one face button responded reliably. Repair switches the internal uGUI Canvas to ScreenSpaceCamera, forces a render after page rebuild, arms input only after the initiating control is released, creates independent enlarged X/Y/A/B hit targets, and places compact front-facing hardware labels outside rotated button transforms. Acceptance requires visible live Main/Pause pages, Pause remaining open until a deliberate later Back/Resume action, correct X/Y/A/B orientation and independent mouse/keyboard/gamepad operation of all physical controls. |
| `UI-HANDHELD-3D-003` | Missing Unity UI (uGUI) package dependency | `COMPILATION VERIFIED / TEST EVERYTHING REQUIRED` | The project now reaches Play Mode, proving `UnityEngine.UI` resolves after adding `com.unity.ugui` `2.0.0`. A fresh TEST EVERYTHING result is still required before removing this row. |
| `UI-HANDHELD-3D-001` | New 3D Main/Pause handheld implementation | `IMPLEMENTED / UNITY VERIFICATION REQUIRED` | Runtime presenter, generated 3D shell, screen RenderTexture, glass, physical controls, Main/Pause pages, settings/progression routing, mouse/D-pad/A-B-X-Y input and deterministic Boy/Girl art selection are implemented. Acceptance requires Unity compilation, TEST EVERYTHING, repeated Main/Pause interaction, scene reload cleanup, performance/GC inspection and user visual approval. |
| `UI-HANDHELD-3D-002` | D-pad physical feedback initially moved only invisible hit targets | `IMPLEMENTED / UNITY VERIFICATION REQUIRED` | Static review found that directional hit targets received input but the visible D-pad cross did not travel. The cross is now split into a modeled center plus four separately animated directional caps, each with its own cached feedback renderer and bounded press distance. Unity Play Mode must confirm direction-specific press/release without gaps, overlap, stuck pose or duplicate action. |
| `BUG-V23R19U-001` | Auxiliary enemy ring transparency initializes a Unity native object from a `MonoBehaviour` field initializer | `IMPLEMENTED / UNITY VERIFICATION REQUIRED` | Play Mode repeatedly threw `CreateImpl is not allowed to be called from a MonoBehaviour constructor (or instance field initializer)` for every enemy receiving `BDAuxiliaryEnemyRingTransparency`. The component created `MaterialPropertyBlock` in its field declaration. Commit `8161ec9288a032b1dd5824be08c5c9be8f703d06` replaces that initializer with one cached, lazy `EnsurePropertyBlock()` allocation invoked from `Awake`/`Apply`. Unity compilation, TEST EVERYTHING and focused Play Mode must confirm zero recurrence before this row is verified. |
| `BUG-V23R19G-002` | Player death presentation | `AUTOMATED PASS / PLAY MODE OPEN` | Automated QA passed. Focused Play Mode must confirm the real player renderer visibly dies before any overlay/menu and the menu waits for the full pose plus readable hold. |
| `BUG-V23R19G-004` | Confirmed abandon flow | `AUTOMATED PASS / PLAY MODE OPEN` | Automated QA passed. Focused Play Mode must confirm abandon reloads a clean main-menu state and does not overlay the abandoned run. |
| `BUG-V23R19G-005` | Fresh mounted intro after abandon | `REOPENED / SERIOUS / IMPLEMENTED IN V23R19O / UNITY VERIFICATION REQUIRED` | The correct rider Transform is bound, but the Boy's body renderer is absent through the visible mounted entrance and appears only at cinematic completion. V23R19O captures the fresh-scene visible renderer baseline, clears renderer suppression, keeps skinned bounds updating while offscreen, and reasserts visibility from before cover reveal through control release. |
| `BUG-V23R19O-001` | Target outline includes non-damageable enemy ring | `REOPENED / USER REJECTED / REPAIR PENDING` | The required behavior is a red target treatment on the vulnerable enemy model only. The broad presentation-only ring/circle around the enemy must remain its normal color and must never inherit the red target outline. The user confirmed that the current result still colors the surrounding ring, so the prior V23R19O implementation is not accepted. Repair this after `BUG-V23R19U-001` is verified, without changing target range, wall blocking or one-target ownership. |
| `BUG-V23R19H-001` | Boy mounted hook incorrectly enabled | `AUTOMATED PASS / PLAY MODE OPEN` | Automated QA passed. Focused Play Mode must confirm the Boy launches neither sword melee nor hook while mounted and does not consume hook cooldown; future Girl permission remains documentation only. |

## Latest focused verification baseline

- 2026-06-09 latest user Play Mode review reopened final physical-control semantics and visual polish: labels/button overlap, page alignment, New Game card clipping, weak shadow/glass, flat control materials and incorrect shortcuts. `UI-HANDHELD-3D-010` is implemented and awaits Unity verification.
- 2026-06-09 second Play Mode review: live screen and interaction were much improved, but the user rejected texture quality, remaining layout breakage and shared New Game artwork. `UI-HANDHELD-3D-005` and `UI-HANDHELD-3D-006` are implemented and await Unity verification.

- 2026-06-09 user Play Mode rejected the first 3D handheld result: blank live screen, Pause closing immediately, overlapping/repeated hardware labels and unreliable XYAB hit targets. `UI-HANDHELD-3D-004` is implemented and awaits Unity verification.
- Unity `TEST EVERYTHING` at `2026-06-09T02:59:12.2289930Z` was blocked solely because C# compilation failed after `UnityEngine.UI` types were introduced without the `com.unity.ugui` package dependency. The dependency repair is implemented and awaits Unity verification.
- The automated-only V23R19R/S/T/P/Q QA-semantic defects are verified by the clean `2026-06-09T00:13:48.3411810Z` TEST EVERYTHING pass and were removed from the open table.
- Unity `TEST EVERYTHING` at `2026-06-09T00:13:48.3411810Z` passed with 0 blockers, 0 warnings and 0 info. This verifies the ProjectGuide V1.2 compatibility repair; it predates the new 3D handheld Runtime implementation, which requires a new rerun.
- On 2026-06-09, Play Mode reported repeated `UnityException` failures from `BDAuxiliaryEnemyRingTransparency` because `MaterialPropertyBlock` was constructed in a `MonoBehaviour` instance-field initializer. The repair is committed, but Unity has not yet rerun it.
- On 2026-06-09, the user rejected the current target-highlight result because the red treatment still affects the enemy's surrounding ring instead of only the damageable model. `BUG-V23R19O-001` is reopened.

- Unity `TEST EVERYTHING` ran at `2026-06-08T20:14:21.3030580Z` and reported 2 blockers, 0 warnings and 0 info.
- Both blockers are brittle QA tokens; compilation completed.

- Unity `TEST EVERYTHING` ran at `2026-06-08T20:02:41.9132920Z` and reported 3 blockers, 0 warnings and 0 info.
- All three findings are QA semantic drift; compilation completed.
- The Unity Account API accessibility warning is external package noise and is not a B&D automated blocker.

- Unity `TEST EVERYTHING` passed at `2026-06-08T19:25:10.9933680Z` with 0 blockers, 0 warnings and 0 info.
- The user verified the V23R19M airborne Light/Heavy orientation, absence of the duplicate horizontal slash, small regular-enemy intact death and retained large/Elite death path.
- Those verified rows were removed from the current open table; Git history and `ProjectGuide/Status/CURRENT.md` retain the completed record.

- Unity `TEST EVERYTHING` ran at `2026-06-08T18:53:41.7256860Z` and reported 2 blockers, 0 warnings and 0 info. Both blockers were stale historical bug-ledger requirements, not compiler errors.
- Focused Play Mode reopened airborne long-axis orientation and reported small regular-enemy death presentation as visually unacceptable.

- Unity `TEST EVERYTHING` passed on `2026-06-08T18:17:20.5169230Z` with 0 blockers, 0 warnings and 0 info items after V23R19K.
- V23R19K automated QA is verified; the listed V23R19G/V23R19H/V23R19K behaviors remain open only for focused Play Mode/user confirmation.
- The user reported that every previously requested V23R19E behavior not listed in the open table looked correct in Play Mode.
- V23R19I compile compatibility remains verified.

<!-- B&D 2026-06-09 HORSE HUD MINIMAP BUGS V2 START -->
## Horse/HUD/minimap repair set

- Fixed: horse injury used a continuous 55% slowdown instead of the approved 8% per missing 30% band.
- Fixed: horse interaction cards/icons occupied the horse silhouette.
- Fixed: horse and player health and ammo remained permanently visible.
- Fixed: horse heal lacked a clear authored presentation and completed too quickly.
- Fixed: minimap horse color/shape and enemy rank markers did not match the approved language.
- Fixed: local package validator treated decorative `=======` strings as merge-conflict markers; only real full-line conflict markers are now blocked.
<!-- B&D 2026-06-09 HORSE HUD MINIMAP BUGS V2 END -->

<!-- BND_UNITY_UI_PACKAGE_RECOVERY_V3:BEGIN -->
## 2026-06-09 — UnityEngine.UI package resolution after cache cleanup

Observed blocker: `UnityEngine.UI`, `Image`, `Text`, `RawImage`, and `Outline` could not resolve even though `com.unity.ugui` and `com.unity.modules.ui` are declared. Unity also reported generated package symlinks, altered immutable package contents, and a missing `Library/Search` database file. V3 moves untracked generated package overlays to an external backup and rebuilds only reproducible package/script/search caches. Status after applying V3: recovery applied; Unity compilation and TEST EVERYTHING still require real local proof.
<!-- BND_UNITY_UI_PACKAGE_RECOVERY_V3:END -->

<!-- BND_HORSE_HEALING_COMPILE_FIX_V4:BEGIN -->
## Horse healing presentation compile regression — local V4 hotfix

- Reported compiler errors: `CS1023` and `CS0103` in `BDHorseHealingPresentation.EndHealing`.
- Cause: an invalid declaration under an unbraced `if` and an out-of-scope `healthRatio` reference.
- Static correction: completed healing now requests a deterministic full completion pulse; interrupted healing keeps the normal fade-out path.
- Status: source repaired locally; Unity compilation, `TEST EVERYTHING`, and Play Mode verification remain required.
<!-- BND_HORSE_HEALING_COMPILE_FIX_V4:END -->

<!-- BND_QA_CONTRACT_REALIGNMENT_V5:BEGIN -->
## Stale automated contracts after the cumulative production patch — resolved locally

- Automated QA still required the superseded handheld rest position `0.62f`.
- V23R12 QA still required world-space horse prompt height/offset literals after horse actions moved to the bottom contextual strip.
- BBH and horse feature documents described the correct behavior but omitted explicit maintained summaries used by focused QA.
- V5 realigns the validators with the current authoritative owners and updates the maintained feature documentation.
- Unity compilation, `TEST EVERYTHING`, and Play Mode acceptance remain required before commit.
<!-- BND_QA_CONTRACT_REALIGNMENT_V5:END -->

<!-- BND_TUTORIAL_REFERENCE_LED_V3:BEGIN -->
## First-launch tutorial V3 defects addressed locally

- tutorial world lacked a coherent reference-led palette;
- instructions were still compressed into one multiline block;
- keyboard/mouse and handheld routes needed separate large cards;
- decorative layout required stronger safe-region ownership;
- smooth scripted movement weakened the intended pixel presentation.

The local V3 package addresses these items. Unity and Play Mode verification remain open.
<!-- BND_TUTORIAL_REFERENCE_LED_V3:END -->

<!-- BND_FIRST_LAUNCH_TUTORIAL_QA_CONTRACT_FIX_V8:BEGIN -->
## False first-launch tutorial QA blocker — corrected locally

V7 incorrectly required `HANDHELD  HOLD Y` as one contiguous string, although
the tutorial composes a card title and Grapple binding independently. V8
validates the authoritative fields separately. Runtime behavior is unchanged.
<!-- BND_FIRST_LAUNCH_TUTORIAL_QA_CONTRACT_FIX_V8:END -->
