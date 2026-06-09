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
| `BUG-DOCS-QA-002` | ProjectGuide historical QA discovery compatibility | `IMPLEMENTED IN V1.2 / UNITY RERUN REQUIRED` | TEST EVERYTHING at `2026-06-09T00:06:07.0833090Z` reported 9 documentation-only blockers: missing V23R8/V23R9/V23R10 discovery phrases in the reorganized index/art owner and missing `Why this task exists`, `Protected content and behavior`, and `Performance contract` headings in the active V23R19Q task record. V1.2 restores those stable contracts in the new canonical files without recreating duplicate retired documents or weakening scanners. Require a clean Unity rerun before verification. |
| `BUG-DOCS-PKG-001` | ProjectGuide package validation handoff | `IMPLEMENTED / LOCAL REVALIDATION REQUIRED` | The first local application successfully reorganized the guide, but validation stopped on `.DS_Store` files at repository root, `Library/`, and `.codex/`. Package testing then exposed two more handoff defects: a brittle exact-phrase requirement rejected the valid `real upright 3D handheld` wording, and repository hygiene rejected the package manifest before the validator could finish. Package V1.1 adds explicit OS-metadata cleanup, concept-based validation, controlled allowance of transient package files during validation, a final strict hygiene pass after cleanup, and fail-fast ordering. Verify the corrected sequence passes before Unity testing or commit. |
| `BUG-V23R19U-001` | Auxiliary enemy ring transparency initializes a Unity native object from a `MonoBehaviour` field initializer | `IMPLEMENTED / UNITY VERIFICATION REQUIRED` | Play Mode repeatedly threw `CreateImpl is not allowed to be called from a MonoBehaviour constructor (or instance field initializer)` for every enemy receiving `BDAuxiliaryEnemyRingTransparency`. The component created `MaterialPropertyBlock` in its field declaration. Commit `8161ec9288a032b1dd5824be08c5c9be8f703d06` replaces that initializer with one cached, lazy `EnsurePropertyBlock()` allocation invoked from `Awake`/`Apply`. Unity compilation, TEST EVERYTHING and focused Play Mode must confirm zero recurrence before this row is verified. |
| `BUG-V23R19T-001` | Historical QA phase ID blocks current PROJECT_STATUS | `IMPLEMENTED / UNITY VERIFICATION REQUIRED` | TEST EVERYTHING at 2026-06-08T21:26:24.0618690Z reported the sole blocker `V23R19R_PROJECT_STATUS_MISSING` because V23R19R QA requires `C01.QA.V23R19R` to remain current. V23R19T makes V23R19R/V23R19S status and bug-ledger checks phase-agnostic and requires a clean rerun. |
| `BUG-V23R19S-001` | V23R19R continuity QA requires one brittle prose sentence | `IMPLEMENTED / UNITY VERIFICATION REQUIRED` | TEST EVERYTHING at 2026-06-08T21:21:28.2403370Z reported the sole blocker `V23R19R_CONTINUITY_CONTRACT_MISSING`. The continuity contract already contains the six-level verification ladder. V23R19S changes the scanner to validate each level independently and requires a clean rerun. |
| `BUG-V23R19R-001` | Legacy V23R10 menu QA requires removed prototype label | `IMPLEMENTED / UNITY VERIFICATION REQUIRED` | TEST EVERYTHING at 2026-06-08T21:09:44.6225160Z reported the sole blocker `V23R10_GAME_BOY_MENU_SHELL_MISSING` because a legacy scanner requires `B&D POCKET ADVENTURE`. V23R19R updates validation to the active `B&D // POCKET MEMORY` identity without changing Runtime UI. |
| `BUG-V23R19G-002` | Player death presentation | `AUTOMATED PASS / PLAY MODE OPEN` | Automated QA passed. Focused Play Mode must confirm the real player renderer visibly dies before any overlay/menu and the menu waits for the full pose plus readable hold. |
| `BUG-V23R19G-004` | Confirmed abandon flow | `AUTOMATED PASS / PLAY MODE OPEN` | Automated QA passed. Focused Play Mode must confirm abandon reloads a clean main-menu state and does not overlay the abandoned run. |
| `BUG-V23R19G-005` | Fresh mounted intro after abandon | `REOPENED / SERIOUS / IMPLEMENTED IN V23R19O / UNITY VERIFICATION REQUIRED` | The correct rider Transform is bound, but the Boy's body renderer is absent through the visible mounted entrance and appears only at cinematic completion. V23R19O captures the fresh-scene visible renderer baseline, clears renderer suppression, keeps skinned bounds updating while offscreen, and reasserts visibility from before cover reveal through control release. |
| `BUG-V23R19O-001` | Target outline includes non-damageable enemy ring | `REOPENED / USER REJECTED / REPAIR PENDING` | The required behavior is a red target treatment on the vulnerable enemy model only. The broad presentation-only ring/circle around the enemy must remain its normal color and must never inherit the red target outline. The user confirmed that the current result still colors the surrounding ring, so the prior V23R19O implementation is not accepted. Repair this after `BUG-V23R19U-001` is verified, without changing target range, wall blocking or one-target ownership. |
| `BUG-V23R19P-001` | Post-V23R19O QA semantic drift | `IMPLEMENTED / UNITY VERIFICATION REQUIRED` | Three automated blockers require an obsolete Wall Jump duration, a verified bug row, or whitespace-sensitive mounted-rider call text. V23R19P updates only QA semantics and requires a clean rerun. |
| `BUG-V23R19Q-001` | Two brittle post-V23R19P QA tokens | `IMPLEMENTED / UNITY VERIFICATION REQUIRED` | Traversal QA requires an exact Wall Jump assignment and Caterpillar QA requires non-canonical wording. V23R19Q changes only those expectations and requires a clean automated rerun. |
| `BUG-V23R19H-001` | Boy mounted hook incorrectly enabled | `AUTOMATED PASS / PLAY MODE OPEN` | Automated QA passed. Focused Play Mode must confirm the Boy launches neither sword melee nor hook while mounted and does not consume hook cooldown; future Girl permission remains documentation only. |

## Latest focused verification baseline

- Unity `TEST EVERYTHING` at `2026-06-09T00:06:07.0833090Z` reported 9 blockers, 0 warnings and 0 info; every blocker was a ProjectGuide documentation-discovery compatibility requirement, not a Runtime/compiler finding. V1.2 is implemented and awaits rerun.
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
