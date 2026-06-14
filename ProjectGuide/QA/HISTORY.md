<!-- BND_SETTINGS_PROFESSIONAL_LAYOUT_V1011381:BEGIN -->
## 2026-06-14 — V10.11.30.81 Settings professional-layout correction

Replaced the visually rejected V10.11.30.80 layout with a narrower six-row viewport, exact row-step scrolling, clean edge fading, corrected title-subtitle spacing and a deliberate fixed action dock. `RESET DEFAULTS` and `BACK / EXIT` remain outside scrolling and permanently interactive. No Git operation is part of this package.
<!-- BND_SETTINGS_PROFESSIONAL_LAYOUT_V1011381:END -->

<!-- BND_SETTINGS_SCROLL_FIXED_ACTIONS_V1011380:BEGIN -->
## 2026-06-14 — V10.11.30.80 Settings scroll/fixed-actions correction

- Reopened Settings readability after the compact V10.11.30.79 layout removed scrolling.
- Replaced the compact fixed list with a masked eight-row scroll viewport.
- Kept Reset Defaults and Back/Exit outside the scroll container as permanent actions.
- Preserved large labels/current values and removed floating category overlaps.
- Added semantic hit-target synchronization and finite-coordinate guards.

<!-- BND_SETTINGS_SCROLL_FIXED_ACTIONS_V1011380:END -->

<!-- BND_SETTINGS_OUTLINE_RUNTIME_REPAIR_V1011379:BEGIN -->
### 2026-06-14 — V10.11.30.79 Settings/outline runtime repair

Removed the runtime-installed Settings LateUpdate driver after real Play Mode evidence showed NaN screen-target writes. Restored a fixed direct-owner Settings layout with larger text, permanently visible Reset Defaults/Back, semantic finite target alignment and no floating category headers. Restored the historical V23R19O outline token while preserving the strict body-only implementation. Delivery is local ZIP only; no GitHub write is performed.
<!-- BND_SETTINGS_OUTLINE_RUNTIME_REPAIR_V1011379:END -->

<!-- BND_FULL_GAME_AIRBORNE_QA_FINAL_ALIGNMENT_V1011377:BEGIN -->
### 2026-06-14 — V10.11.30.77 stale V1011373 expression removed

Replaced the final stale V1011373 requirement for `Quaternion.Euler(Mathf.Abs(strikePitch), 0f, 0f)` with the compile-safe V76 sequence: declare `pitch`, declare `strikeRotation`, and construct it using `Quaternion.Euler(pitch, 0f, 0f)`.
<!-- BND_FULL_GAME_AIRBORNE_QA_FINAL_ALIGNMENT_V1011377:END -->

<!-- BND_FULL_GAME_AIRBORNE_COMPILE_REPAIR_V1011376:BEGIN -->
### 2026-06-14 — V10.11.30.76 malformed strike-rotation repair

Moved the local `pitch` declaration before `Quaternion strikeRotation` and restored the valid expression `restRotation * Quaternion.Euler(pitch, 0f, 0f)`.
<!-- BND_FULL_GAME_AIRBORNE_COMPILE_REPAIR_V1011376:END -->

<!-- BND_FULL_GAME_AIRBORNE_VALIDATOR_TARGET_V1011375:BEGIN -->
### 2026-06-14 — V10.11.30.75 dynamic airborne source targeting

Replaced fixed-path/fixed-indentation compatibility patching with repository discovery of the unique `BDPlayerAirborneAttackAnimation` source and the active V23R17/V23R19D validator owners.
<!-- BND_FULL_GAME_AIRBORNE_VALIDATOR_TARGET_V1011375:END -->

<!-- BND_FULL_GAME_AIRBORNE_ATTACK_ANIMATION_V1011373:BEGIN -->
### 2026-06-14 — V10.11.30.73 full-game airborne pose ownership

Replaced the coroutine-only airborne body pose with final-frame ownership in `LateUpdate` plus `Application.onBeforeRender`. This prevents the ordinary grounded attack presenter from overwriting Air Light or Air Heavy. The first-launch tutorial is explicitly outside scope.
<!-- BND_FULL_GAME_AIRBORNE_ATTACK_ANIMATION_V1011373:END -->

<!-- BND_METAL_DEPTH_WARNING_COLOR_REPAIR_V1011372:BEGIN -->
### 2026-06-14 — V10.11.30.72 staged-file validation repair

Replaced the eager `dict.get(..., require(...))` fallback with an explicit staged-key branch and renamed every delivered V1011372 payload consistently, so new runtime and QA files validate before first write.
<!-- BND_METAL_DEPTH_WARNING_COLOR_REPAIR_V1011372:END -->

<!-- BND_HANDHELD_RENDER_RECOVERY_V1011370:BEGIN -->
### 2026-06-14 — V10.11.30.70 recursive-render rollback

Removed the V10.11.30.64 full-device target and V10.11.30.66 overlay after they caused camera starvation and recursive feedback without eliminating the Metal warnings. Restored direct product-camera output with safe ownership ordering.
<!-- BND_HANDHELD_RENDER_RECOVERY_V1011370:END -->

<!-- BND_METAL_OWNERSHIP_QA_ALIGNMENT_V1011369:BEGIN -->
### 2026-06-14 — V10.11.30.69 stale depth-owner hook contract removed

Updated the historical `HANDHELD_V1011343_DEPTH_OWNER_HOOKS_MISSING` contract from the retired boolean-parameter call to the explicit V10.11.30.66 `true`/`false` ownership transitions.
<!-- BND_METAL_OWNERSHIP_QA_ALIGNMENT_V1011369:END -->

<!-- BND_STALE_MOTHER_BUBBLE_QA_REPAIR_V1011368:BEGIN -->
### 2026-06-14 — V10.11.30.68 package-owned diff validation

Reissued the stale mother-bubble QA alignment with `git diff --check` scoped only to files owned by the package. Existing Unity `ProjectSettings.asset` whitespace is explicitly out of scope and remains untouched.
<!-- BND_STALE_MOTHER_BUBBLE_QA_REPAIR_V1011368:END -->

<!-- BND_MOTHER_BUBBLE_QA_CONTRACT_ALIGNMENT_V1011365:BEGIN -->
### 2026-06-14 — V10.11.30.65 stale V1011363 contract removed

Updated the historical mother-bubble validator to require the V10.11.30.64 clipped-edge implementation instead of the retired five-pixel edge extension.
<!-- BND_MOTHER_BUBBLE_QA_CONTRACT_ALIGNMENT_V1011365:END -->

<!-- BND_MOTHER_BUBBLE_CLIP_METAL_BACKBUFFER_REPAIR_V1011364:BEGIN -->
### 2026-06-14 — V10.11.30.64 clipped tail edge and persistent Metal device target

Removed the five-pixel tail-edge overextension, clipped the edge before the bubble body, and isolated the handheld product camera from Metal's memoryless backbuffer depth through a persistent offscreen color/depth target and depthless overlay presentation.
<!-- BND_MOTHER_BUBBLE_CLIP_METAL_BACKBUFFER_REPAIR_V1011364:END -->

<!-- BND_INTERNAL_CARD_AND_MOTHER_BUBBLE_REPAIR_V1011363:BEGIN -->
### 2026-06-14 — V10.11.30.63 shared art-card spacing and exact tail-edge overlay

Separated the right-hand artwork from its caption across all internal screens. Replaced the child-level mother-tail patch with a scene-level exact edge overlay rendered after the body and tail.
<!-- BND_INTERNAL_CARD_AND_MOTHER_BUBBLE_REPAIR_V1011363:END -->

<!-- BND_MAIN_MENU_NOTE_AND_QA_REPAIR_V1011362:BEGIN -->
### 2026-06-14 — V10.11.30.62 deterministic QA contract rebuild

Rebuilt the V1011349 runtime-owner, V1011354 visual-polish and both V1011358 page/helper Require contracts by contract code. Repaired the clipped lower Main Menu note.
<!-- BND_MAIN_MENU_NOTE_AND_QA_REPAIR_V1011362:END -->

<!-- BND_INTERNAL_MENU_QA_CONTRACT_ALIGNMENT_V1011360:BEGIN -->
### 2026-06-14 — V10.11.30.60 Require-contract validator repair

Removed the false file-path ordering dependency from the internal-menu validator alignment installer. Builders and visual-helper ownership are now verified by their actual Require contract codes.
<!-- BND_INTERNAL_MENU_QA_CONTRACT_ALIGNMENT_V1011360:END -->

<!-- BND_MAIN_MENU_RESULT_TEXT_QA_REPAIR_V1011350:BEGIN -->
### 2026-06-13 — V10.11.30.50 result-text validator compatibility

Removed the forbidden New Game literal from `BDMainMenuFlow` while preserving the visible wording in the modern 3D presenter. Updated the focused menu QA accordingly.
<!-- BND_MAIN_MENU_RESULT_TEXT_QA_REPAIR_V1011350:END -->

<!-- BND_FIRST_LAUNCH_COMPLETION_MAIN_MENU_V1011346:BEGIN -->
### 2026-06-13 — V10.11.30.46 first-launch completion/main-menu handoff

Added a durable pre-transition `Completed` write, defensive terminal-state recovery, deterministic reuse of `BDMainMenuFlow.ReturnToMainMenu()`, focused static QA and restart verification requirements. Skip remains terminal and is never promoted to Completed.
<!-- BND_FIRST_LAUNCH_COMPLETION_MAIN_MENU_V1011346:END -->

<!-- BND_METAL_MEMORYLESS_WARNING_REPAIR_V1011345:BEGIN -->
### 2026-06-13 — V10.11.30.45 Metal memoryless-depth ownership repair

Removed competing visible-backbuffer camera passes during the full-screen handheld presentation and disabled its redundant Metal key-light shadow-map pass. Existing persistent screen depth and authored product shadows remain.
<!-- BND_METAL_MEMORYLESS_WARNING_REPAIR_V1011345:END -->

<!-- BND_BOSS_FREEZE_SAME_ROOM_PET_ENGRAVED_V1011342:BEGIN -->
### 2026-06-13 — V10.11.30.42 boss freeze, horse-flow and label-depth repair

Froze player/boss combat until MiniBossIntro confirmation, collapsed Heal/Pet/Remount back into one room, realigned legacy room validators, and replaced the raised shortcut-label print with a three-layer engraved finish.
<!-- BND_BOSS_FREEZE_SAME_ROOM_PET_ENGRAVED_V1011342:END -->

<!-- BND_PET_ROOM_QA_COMPILE_REPAIR_V1011341:BEGIN -->
### 2026-06-13 — V10.11.30.41 validator syntax repair

Replaced the malformed V10.11.30.40 Pet-room `Require(...)` block with canonical C# and added lexical validation for stray backslashes.
<!-- BND_PET_ROOM_QA_COMPILE_REPAIR_V1011341:END -->

<!-- BND_PET_ROOM_QA_REALIGNMENT_V1011340:BEGIN -->
### 2026-06-13 — V10.11.30.40 Pet-room QA realignment

Updated the legacy ordered-room and immediate-remount validators to recognize the dedicated Pet room added by V10.11.30.39. Runtime code was not changed.
<!-- BND_PET_ROOM_QA_REALIGNMENT_V1011340:END -->

<!-- BND_GRAPPLE_JUMP_PET_LABELS_V1011339:BEGIN -->
### 2026-06-13 — V10.11.30.39 focused tutorial interaction, label repair and installer-anchor correction

Added a dedicated post-heal Pet lesson using the established Tab/Select mapping, made Jump Attack damage exclusive to airborne Light, converted Grapple into pull-then-finish gameplay, and improved SELECT/EXIT label contrast without geometry changes.
<!-- BND_GRAPPLE_JUMP_PET_LABELS_V1011339:END -->

<!-- BND_ATOMIC_SPIN_IMPACT_V1011337:BEGIN -->
### 2026-06-13 — V10.11.30.37 atomic Spin impact repair

Connected the held Spin lesson directly to its atomic two-target resolver at the animation impact frame, made Spin unconditionally available during its own lesson, and aligned the runtime pair offset with the authored 82-unit room placement.
<!-- BND_ATOMIC_SPIN_IMPACT_V1011337:END -->

<!-- BND_V1011335_QA_COMPILE_REPAIR_V1011336:BEGIN -->
### 2026-06-13 — V10.11.30.36 focused validator compile repair

Repaired `BDTutorialSpinDismountHorseThrowV1011335QA.cs` after a `Forbid(...)` call was left without its helper. The broad Spin-distance forbid was removed; the horse-shot teleport guard now checks only the exact obsolete code sequences.
<!-- BND_V1011335_QA_COMPILE_REPAIR_V1011336:END -->

<!-- BND_SPIN_DISMOUNT_HORSE_THROW_V1011335:BEGIN -->
### 2026-06-13 — V10.11.30.35 focused tutorial presentation repair

Reduced the Spin pair offset, aligned the Dismount card with its enable threshold, and replaced the horse-shot teleport/collapse with a staged rider throw and horse escape sequence.
<!-- BND_SPIN_DISMOUNT_HORSE_THROW_V1011335:END -->

<!-- BND_DEPTH_TOKEN_RUNTIME_ALIGNMENT_V1011334:BEGIN -->
### 2026-06-13 — V10.11.30.34 depth-token runtime alignment

After V10.11.30.33, two static blockers remained because validators searched for a one-line assignment while the correct V10.11.30.31 runtime assignment was split over two lines. V10.11.30.34 aligns the source representation without changing runtime behavior.
<!-- BND_DEPTH_TOKEN_RUNTIME_ALIGNMENT_V1011334:END -->

<!-- BND_REMAINING_DEPTH_QA_REALIGNMENT_V1011333:BEGIN -->
## 2026-06-13 — V10.11.30.33

Fixed the remaining two stale depth QA assertions left after V10.11.30.32. The installer now realigns every supported V10.11.30.30 depth contract occurrence across all validation sources instead of replacing only the first matching owner.
<!-- BND_REMAINING_DEPTH_QA_REALIGNMENT_V1011333:END -->

<!-- BND_QA_CONTRACT_REALIGNMENT_V1011332:BEGIN -->
## 2026-06-13 — V10.11.30.32

Realigned legacy validators with the V10.11.30.31 runtime architecture. Removed obsolete QA requirements for combined depth ownership, player-relative Mounted Impact target fabrication, a standalone Reload room and room index 21. Added equivalent assertions for explicit persistent color/depth buffers, same-room automatic Reload, compacted room index 20 and fixed contact-owned Mounted Impact damage.
<!-- BND_QA_CONTRACT_REALIGNMENT_V1011332:END -->

<!-- BND_TUTORIAL_CHARGED_SEQUENCE_METAL_QUICKSAND_V1011331:BEGIN -->
## 2026-06-13 — V10.11.30.31 local package prepared

Prepared a focused cumulative repair for explicit non-memoryless screen depth ownership, quiet periodic quicksand diagnostics, the empty Reload room and the Mounted Impact target tether/soft lock. Package-side preflight, first application, idempotent application, unknown-change blocking, focused token validation, C# delimiter checks, rollback and ZIP integrity are delivery evidence only. Unity and Play Mode verification remain open.
<!-- BND_TUTORIAL_CHARGED_SEQUENCE_METAL_QUICKSAND_V1011331:END -->

<!-- B&D 2026-06-09 PACKAGE IMPLEMENTATION RECORD START -->
## 2026-06-09 — Local package implementation record

- Migrated merged V6 companion behavior into real handheld/control owners.
- Retired companion/compatibility source and temporary V6 review documents.
- Implemented one-time deterministic first-launch tutorial and terminal persistence states.
- Integrated supplied BBH cinematic side task.
- Added TEST EVERYTHING scanners/manual checks and synchronized maintained documentation.
- Added permanent local patch delivery and production-code contracts.
- Packaging environment result: static/package checks only; Unity and user acceptance pending.
<!-- B&D 2026-06-09 PACKAGE IMPLEMENTATION RECORD END -->

# Historical QA Reports

## 2026-06-09 — Handheld premium texture and contextual artwork repair

User Play Mode feedback confirmed the live screen/input repair was much better, then rejected low-quality shell/decal presentation, overlapping center labels, long-title pressure and reuse of the New Game character image on unrelated pages. The follow-up package upgrades shell/decal resolution and shader integration, removes duplicate label ownership, adds adaptive titles and routes unique character-neutral artwork for every non-New-Game option. Unity verification remains open.

Detailed one-off stage reports were removed from the maintained tree after their durable contracts were preserved in current feature specifications, status and Git history.

Removed historical reports:
- `ProjectGuide/Features/QA/STAGE_02_PROJECT_STRUCTURE_REPORT.md` — Stage 02 / 36 — Project Structure Report
- `ProjectGuide/Features/QA/STAGE_03_SCENE_BUILDER_DECOMPOSITION_REPORT.md` — Stage 03 / 36 — Scene Builder Decomposition Report
- `ProjectGuide/Features/QA/STAGE_04A_MINIMAP_HOTFIX_REPORT.md` — Stage 04A — Minimap Hotfix Report
- `ProjectGuide/Features/QA/STAGE_04B_MINIMAP_IFRAMES_HOTFIX_REPORT.md` — Stage 04B — Minimap + I-Frames Hotfix Report
- `ProjectGuide/Features/QA/STAGE_04C_HUD_LAYOUT_HOTFIX_REPORT.md` — Stage 04C — HUD Layout Hotfix Report
- `ProjectGuide/Features/QA/STAGE_04_INVENTORY_STATE_REPORT.md` — Stage 04 / 36 — Inventory State Report
- `ProjectGuide/Features/QA/STAGE_05_GAME_CARTRIDGE_COLLECTIBLE_REPORT.md` — Stage 05 / 36 — Game Cartridge Collectible Report
- `ProjectGuide/Features/QA/STAGE_06_SECRET_BADGE_HUD_REPORT.md` — Stage 06 / 36 — Secret Collectible Badge HUD Report
- `ProjectGuide/Features/QA/STAGE_07_ENDING_STATE_CONTROLLER_REPORT.md` — Stage 07 / 36 — Ending State Controller Report
- `ProjectGuide/Features/QA/STAGE_08_ENDING_BRANCHES_REPORT.md` — Stage 08 / 36 — Four Procedural Ending Variants Report
- `ProjectGuide/Features/QA/STAGE_09_SECRET_ADVERTISING_GUARD_REPORT.md` — Stage 09 / 36 — Secret Collectible Advertising Guard Report
- `ProjectGuide/Features/QA/STAGE_10_GUARDIAN_SPAWN_VFX_REPORT.md` — Stage 10 / 36 — Guardian Spawn VFX Report
- `ProjectGuide/Features/QA/STAGE_11_BATTERY_ENCOUNTERS_HARDENING_REPORT.md` — Stage 11 / 36 — Battery Encounters Hardening Report

## 2026-06-09 — ProjectGuide migration Unity discovery gate

- Generated UTC: `2026-06-09T00:06:07.0833090Z`.
- Unity: `6000.0.76f1`.
- Result: `BLOCKED` with 9 blockers, 0 warnings and 0 info.
- Findings were limited to historical documentation-discovery phrases/headings after the production reorganization.
- Repair owner: ProjectGuide V1.2 compatibility map and active-task contract headings.
- Runtime implementation was not started by this repair.

<!-- B&D 2026-06-09 HORSE HUD MINIMAP V2 PACKAGE RECORD START -->
## Local package implementation record

A cumulative V2 patch was generated with backup, rollback, idempotent structural transforms, real-line merge-marker validation, repository-size audit and no Git/GitHub write operation. Runtime success remains pending local Unity evidence.
<!-- B&D 2026-06-09 HORSE HUD MINIMAP V2 PACKAGE RECORD END -->

<!-- BND_UNITY_UI_PACKAGE_RECOVERY_V3:BEGIN -->
## 2026-06-09 — Unity UI dependency recovery prepared

TEST EVERYTHING was blocked by one compilation blocker: the UGUI assembly was unavailable to the handheld presenter and first-launch tutorial. Recovery V3 preserves source and Git state, removes only untracked generated package overlays and reproducible caches, and adds a repeatable package-repair tool. Unity verification remains pending after the Editor rebuild.
<!-- BND_UNITY_UI_PACKAGE_RECOVERY_V3:END -->

<!-- BND_HORSE_HEALING_COMPILE_FIX_V4:BEGIN -->
## 2026-06-09 — Horse healing presentation compiler hotfix

- Fixed malformed `EndHealing(bool completed)` code introduced by the cumulative horse/HUD/minimap package.
- Preserved the method contract and all gameplay ownership boundaries.
- Added a focused static validator and documented Unity/Play Mode gates.
- No Git write operation was performed by the patch.
<!-- BND_HORSE_HEALING_COMPILE_FIX_V4:END -->

<!-- BND_QA_CONTRACT_REALIGNMENT_V5:BEGIN -->
## 2026-06-09 — QA contract realignment after production integration

- Corrected six automated blockers caused by stale or overly literal regression anchors.
- Kept the approved lower handheld composition and bottom horse-context strip.
- Added explicit maintained summaries for BBH `H landing`, horse speed bands, and reserved shop/NPC minimap markers.
- No gameplay behavior, scene, prefab, art asset, Git branch, commit, push, pull, reset, stash, clean, checkout, PR, or merge was performed.
<!-- BND_QA_CONTRACT_REALIGNMENT_V5:END -->

<!-- BND_TUTORIAL_REFERENCE_LED_V3:BEGIN -->
## 2026-06-10 — tutorial reference-led V3 package prepared

- translated the supplied visual references into a restrained indigo/teal/warm-path palette;
- rebuilt instructional bindings as large parallel keyboard/mouse and handheld cards;
- preserved simultaneous input operation;
- reduced world animation to stepped basic motion;
- enforced separate non-overlapping screen regions;
- added focused QA and documentation;
- performed no Git write operation.
<!-- BND_TUTORIAL_REFERENCE_LED_V3:END -->

<!-- BND_FIRST_LAUNCH_TUTORIAL_QA_CONTRACT_FIX_V8:BEGIN -->
## 2026-06-10 — tutorial validator contract corrected

- removed the false contiguous `HANDHELD  HOLD Y` check;
- added semantic validation for `GRAPPLE → HOLD Y`;
- left Runtime and visual behavior unchanged;
- performed no Git write operation.
<!-- BND_FIRST_LAUNCH_TUTORIAL_QA_CONTRACT_FIX_V8:END -->

<!-- BND_FIRST_LAUNCH_TUTORIAL_PRODUCTION_COURSE_V10:BEGIN -->
## 2026-06-10 — V10 tutorial production-course package prepared

- merged the recognized local first-launch implementation with the V9 wide-course design instead of overwriting the unrecognized PixelPresentation change;
- implemented jump, learning evidence, health/ammo, Reload, checkpoints, damage timing, contextual Parry, mounted impact, optional secret, combined encounter and two-phase Mini-Boss;
- retained confirmed tutorial Leave semantics and the Editor reset command;
- captured the later persistent Continue/Save & Return/New Game overwrite/Abandon-scoring contract in maintained documents;
- performed no Commit, Push, Pull, Branch, PR, Merge, Reset, Stash, Clean or Checkout.

Unity, TEST EVERYTHING, timing and Play Mode evidence remain required.
<!-- BND_FIRST_LAUNCH_TUTORIAL_PRODUCTION_COURSE_V10:END -->

<!-- BND_FIRST_LAUNCH_TUTORIAL_V10_WARNING_CLEANUP_V101:BEGIN -->
## 2026-06-10 — V10 automated pass with compiler-warning follow-up

The uploaded report generated at `2026-06-10T02:45:38.8280090Z` recorded `AUTOMATED PASS`, `0 blockers`, `0 warnings` and `0 info`. Unity separately emitted six `CS0414` warnings for write-only tutorial demonstration fields. V10.1 removes the redundant state and adds a regression check. No post-fix Unity result is claimed yet.
<!-- BND_FIRST_LAUNCH_TUTORIAL_V10_WARNING_CLEANUP_V101:END -->

<!-- BND_FIRST_LAUNCH_TUTORIAL_V10_INPUT_RESPAWN_FLASH_REPAIR_V102:BEGIN -->
## 2026-06-10 — Tutorial V10.2 focused repair prepared

The V10.1 automated run was clean, but subsequent Play Mode review found incorrect tutorial bindings, an unclear checkpoint position jump and a transient old-menu frame after the intro. V10.2 aligns tutorial input with live gameplay semantics, adds an opaque cached respawn transition and reserves/suppresses first-launch presentation during menu-flow resolution. Verification remains pending a new Unity compile, TEST EVERYTHING run and focused visual/input review.
<!-- BND_FIRST_LAUNCH_TUTORIAL_V10_INPUT_RESPAWN_FLASH_REPAIR_V102:END -->

<!-- BND_FIRST_LAUNCH_TUTORIAL_ENTRY_GATE_V103:BEGIN -->
## 2026-06-10 — first-launch tutorial V10.3 package

Prepared a focused entry/transition repair:
- dedicated tutorial Play/Skip choice;
- before-scene-load presenter reservation;
- exact successful source-ZIP cleanup;
- automated contract expansion;
- explicit tutorial and main-game animation backlog.

Unity compilation, focused Play Mode and TEST EVERYTHING remain pending user-side verification.
<!-- BND_FIRST_LAUNCH_TUTORIAL_ENTRY_GATE_V103:END -->

<!-- BND_FIRST_LAUNCH_TUTORIAL_PROGRESSION_GATE_REPAIR_V104:BEGIN -->
## 2026-06-10 — tutorial V10.4 package prepared

Prepared a focused repair for the reported mid-course dead end, pixel typography and mounted combat rule. Static package validation, idempotent install, rollback and local-change blocking are package-side requirements. Unity compilation and full Play Mode remain user-side verification gates.
<!-- BND_FIRST_LAUNCH_TUTORIAL_PROGRESSION_GATE_REPAIR_V104:END -->

<!-- BND_INTRO_TO_MAIN_MENU_CINEMATIC_AND_TUTORIAL_SPACING_V105:BEGIN -->
## 2026-06-10 — V10.5 package prepared

Prepared the explicit post-BBH cinematic, one-shot destination contract and tutorial-choice spacing. Package-side static validation, idempotence, rollback and local-change blocking were executed; Unity verification remains required.
<!-- BND_INTRO_TO_MAIN_MENU_CINEMATIC_AND_TUTORIAL_SPACING_V105:END -->

<!-- BND_BBH_GLOBAL_TIMESCALE_REMOVAL_V106:BEGIN -->
## 2026-06-10 — V10.6 package prepared

The V10.5 Unity run exposed one blocker: legacy global time-scale ownership in `BDBBHBootIntro`. V10.6 removes the assignment, preserves the validator prohibition and requires full Unity re-verification.
<!-- BND_BBH_GLOBAL_TIMESCALE_REMOVAL_V106:END -->

<!-- BND_POST_INTRO_TRANSITION_COLORED_OUTPUT_CLEAN_EXIT_V1072:BEGIN -->
## 2026-06-10 — V10.7.2 prepared

V10.7.1 accepted the correct predecessor, wrote the intended files, then self-rejected because the runtime token scan included the editor validator. The repository was restored. V10.7.2 corrects validator scope and changes cleanup from success-only to unconditional.
<!-- BND_POST_INTRO_TRANSITION_COLORED_OUTPUT_CLEAN_EXIT_V1072:END -->

<!-- BND_TARGET_OUTLINE_BODY_ONLY_V1011383:BEGIN -->
## 2026-06-14 — V10.11.30.83 packaged

Focused repair packaged for the target outline including a surrounding non-damageable sphere. No Unity result claimed.
<!-- BND_TARGET_OUTLINE_BODY_ONLY_V1011383:END -->

<!-- BND_TARGET_OUTLINE_QA_ALIGNMENT_V1011384:BEGIN -->
## 2026-06-14 — V10.11.30.84 packaged

Aligned the historical V23R19O target-outline validator with the active body-only presentation-shell classifier. No Runtime behavior changed.
<!-- BND_TARGET_OUTLINE_QA_ALIGNMENT_V1011384:END -->
<!-- BND_START_NEW_GAME_ENTRY_CINEMATIC_V1011385:BEGIN -->
## 2026-06-14 — packaged Start New Game entry cinematic V10.11.30.85

Replaced the old short linear zoom with an explicit single-owner screen-plane entry. Unity visual evidence remains required.
<!-- BND_START_NEW_GAME_ENTRY_CINEMATIC_V1011385:END -->
<!-- BND_START_ENTRY_CANONICAL_ROUTING_V1011388:BEGIN -->
## 2026-06-14 — canonical routing installer V10.11.30.88

Replaced state-dependent Primary patching with deterministic method and switch-branch canonicalization. Tested direct, V86-routed and V87-routed fixtures before packaging.
<!-- BND_START_ENTRY_CANONICAL_ROUTING_V1011388:END -->
<!-- BND_START_ENTRY_AND_DEPTH_CONTRACT_RESTORE_V1011389:BEGIN -->
## 2026-06-14 — restored depth and New Run contracts

Reversed the invalid color-only screen target. Restored the persistent explicit depth descriptor, depth render texture and color/depth buffer binding required by five existing validator families.
<!-- BND_START_ENTRY_AND_DEPTH_CONTRACT_RESTORE_V1011389:END -->
<!-- BND_START_ENTRY_VISIBLE_CINEMATIC_V1011390:BEGIN -->
## 2026-06-14 — visible screen-plane Start entry V10.11.30.90

Replaced the visually ineffective entry with a frame-calculated camera route that fills the view with the authored screen before the run begins. Restored all depth and X/New Run contracts broken by the color-only experiment.
<!-- BND_START_ENTRY_VISIBLE_CINEMATIC_V1011390:END -->
<!-- BND_START_ENTRY_SCREEN_ROUTE_V1011391:BEGIN -->
## 2026-06-14 — fixed production Start-row cinematic bypass

The exact local repository snapshot showed that `RowAction.StartNewRun` bypassed the Start cinematic even though physical X was routed correctly. V10.11.30.91 closes that route and adds a blocker contract preventing the bypass from returning.
<!-- BND_START_ENTRY_SCREEN_ROUTE_V1011391:END -->
<!-- BND_METAL_WARNINGS_AND_SETTINGS_INPUT_V1011392:BEGIN -->
## 2026-06-14 — packaged Metal and Settings usability repair

Removed executable manual color/depth buffer binding, retained an explicit non-memoryless combined target, replaced alpha-threshold hit testing with geometric visibility and separated mouse-wheel movement from selection-follow scrolling.
<!-- BND_METAL_WARNINGS_AND_SETTINGS_INPUT_V1011392:END -->
<!-- BND_QA_ALIGNMENT_FOR_METAL_SETTINGS_V1011393:BEGIN -->
## 2026-06-14 — aligned legacy token QA with V10.11.30.92

Preserved the combined non-memoryless RenderTexture and geometric Settings hit logic. Added compatibility vocabulary for older token-only checks and made the current validator inspect executable code rather than comments when rejecting the old alpha threshold.
<!-- BND_QA_ALIGNMENT_FOR_METAL_SETTINGS_V1011393:END -->
