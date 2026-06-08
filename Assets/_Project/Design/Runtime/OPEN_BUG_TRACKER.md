# Open Bug Tracker — Current Defect Ledger

> [!IMPORTANT]
> **This document must be updated every time project work discovers a new bug, changes a bug's status, implements a repair, verifies a repair, reopens a bug, or proves that a report is not a bug.**
>
> Update it in the same change set as the related code, QA and `PROJECT_STATUS.md`. It must always describe the most accurate current state. Do not wait until the end of a package or development stage.

This is the maintained focused ledger for current defects. `PROJECT_STATUS.md` remains the only authority for overall ordering, active stage, QA truth and resume point. This tracker must not become a second roadmap.

## Status vocabulary

- `OPEN` — reproduced or reported and not yet repaired.
- `IMPLEMENTED / UNITY VERIFICATION REQUIRED` — code/document repair exists, but Unity compilation and focused Play Mode have not both confirmed it.
- `AUTOMATED PASS / PLAY MODE OPEN` — Unity automated QA passed; focused behavior is still unverified.
- `VERIFIED` — the user or recorded focused test confirmed the repair. Verified bugs are removed from the open table and summarized in `PROJECT_STATUS.md`/Git history.
- `REOPENED` — a previously implemented or verified defect failed again.
- `NOT A BUG / SUPERSEDED` — evidence proved another owner or newer requirement replaced the report.

## Current open bugs

| ID | Area | Current status | Current truth / acceptance condition |
|---|---|---|---|
| `BUG-V23R19J-001` | QA semantic/version drift after V23R19H/V23R19I | `REOPENED / SUPERSEDED BY V23R19K` | V23R19J reduced the blocker count from 15 to 10, but several scanners still required the obsolete suppression owner and a resolved compile-bug row. V23R19K owns the remaining semantic correction. |
| `BUG-V23R19K-001` | Committed airborne visual branch and remaining QA drift | `IMPLEMENTED / UNITY VERIFICATION REQUIRED` | The explicit airborne identity was ignored by `BDPlayerCombat`, so the committed attack still selected the horizontal visual. Consume the identity, spawn exactly one vertical or grounded arc, realign the remaining scanners, and require TEST EVERYTHING 0/0/0. |
| `BUG-V23R19G-001` | Airborne melee presentation | `REOPENED / IMPLEMENTED / UNITY VERIFICATION REQUIRED` | The selected Light/Heavy grounded slash must be rotated **-90° around its local X axis**, remain directly in front of the player, retain the same shape/scale language and travel toward the floor. No horizontal duplicate. |
| `BUG-V23R19G-002` | Player death presentation | `REOPENED / IMPLEMENTED / UNITY VERIFICATION REQUIRED` | Lethal damage must visibly animate the real player renderer branch, including the current spherical prototype model, before any dark overlay or menu. The menu must wait for the full pose plus a readable hold. |
| `BUG-V23R19G-003` | Large/Elite/guardian death presentation | `REOPENED / IMPLEMENTED / UNITY VERIFICATION REQUIRED` | All non-final-boss enemies, including large enemies and Battery guardians, must stop gameplay, animate their actual renderer branches, then allow loot/despawn. |
| `BUG-V23R19G-004` | Confirmed abandon flow | `REOPENED / IMPLEMENTED / UNITY VERIFICATION REQUIRED` | Confirmed abandon must reload the active scene into a clean main-menu state. It must not render the main menu as a popup over the abandoned run. |
| `BUG-V23R19G-005` | Fresh mounted intro after abandon | `REOPENED / IMPLEMENTED / UNITY VERIFICATION REQUIRED` | After abandon -> main menu -> Start Game, the exact active-scene player is mounted and snapped to the horse from the first visible entrance frame through control release. |
| `BUG-V23R19H-001` | Boy mounted hook incorrectly enabled | `IMPLEMENTED / UNITY VERIFICATION REQUIRED` | The boy cannot use the grappling hook while mounted. Mounted Light/Heavy input must not launch sword melee or hook and must not consume hook cooldown. The future Girl character may use the hook while mounted through character-specific capability data. |

## Latest focused verification baseline

- Unity `TEST EVERYTHING` passed on `2026-06-08T14:28:47.7152820Z` with 0 blockers, 0 warnings and 0 info items before the current V23R19G repairs.
- The user reported that every previously requested V23R19E behavior not listed in the open table looked correct in Play Mode.
- V23R19I compile compatibility remains verified. The 2026-06-08T17:07:43.5483780Z run reported 10 blockers; V23R19K owns the remaining QA drift and the ignored airborne visual identity.
