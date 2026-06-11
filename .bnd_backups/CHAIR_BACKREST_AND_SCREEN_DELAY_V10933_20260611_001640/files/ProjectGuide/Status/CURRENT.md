<!-- BND_CHILD_APPROACH_ROOM_SHELL_QA_AGGREGATION_REPAIR_V10932:BEGIN -->
## 2026-06-11 — Child-approach room-shell QA aggregation repair V10.9.32

**Classification:** `CURRENT / FOCUSED AUTOMATED-QA REPAIR`

The V10.9.31 Runtime already contains the right wallpaper wall, right-wall baseboard, and kitchen ceiling. The remaining blocker is caused by `BDChildApproachCinematicQA` scanning the child, transition, and environment files but not the partial Runtime file `BDModernHandheld3DPresenter.CinematicWallpaperBackWall.cs` where those room-shell tokens are implemented.

V10.9.32 updates only the QA aggregation contract and maintained documentation. It does not change the camera path, room geometry, wallpaper, fade, chair, device, screen power sequence, tutorial, or gameplay.

**Exact resume point:** apply V10.9.32, let Unity compile, run `Boredom And Dungeons -> TEST EVERYTHING`, require `0 blockers / 0 warnings / 0 info`, then continue the visual review of the V10.9.31 scene.
<!-- BND_CHILD_APPROACH_ROOM_SHELL_QA_AGGREGATION_REPAIR_V10932:END -->

<!-- BND_CHILD_APPROACH_ROOM_ENTRANCE_FADE_V10931:BEGIN -->
## 2026-06-11 — Kitchen-entrance start, complete room shell and filmic fade V10.9.31

The post-intro child POV cinematic remains the active task. This pass implements the latest visual corrections in the game:

- the child now starts at a kitchen-entrance distance, farther behind and farther left of the chair;
- the entrance-to-chair walk follows a curved cubic route instead of a direct straight-line interpolation;
- the first frame stays fully black for `0.42s`, followed by a dedicated filmic fade that completes at `1.20s` before walking begins;
- the room now contains a real ceiling and a physical right wall, both extending across the camera route;
- the right wall continues the approved fruit wallpaper and includes a matching baseboard;
- the sequence remains shorter than the older `8.25s` version while allowing the longer entrance route and professional fade;
- the accepted chair climb, screen-off contract, power-on direction and animated tutorial-content reveal are preserved.

Acceptance requires Unity compilation, `TEST EVERYTHING 0/0/0`, and real-time visual review from the first black frame through tutorial readiness.
<!-- BND_CHILD_APPROACH_ROOM_ENTRANCE_FADE_V10931:END -->

<!-- BND_CHILD_APPROACH_CINEMATIC_FADE_SPEED_SMOOTHING_V10930:BEGIN -->
## 2026-06-11 — Child approach fade, speed and transition smoothing V10.9.30

The post-intro child POV cinematic remains the active task. This pass:

- starts the child farther behind and farther left of the chair while preserving the accepted child eye height;
- begins from a fully black frame and fades the room in before visible movement starts;
- shortens the complete sequence from `8.25s` to `7.45s` without rushing the screen power-on;
- replaces the segmented chair climb with a tangent-continuous Catmull-Rom route;
- blends the chair-clearance release instead of switching at a hard threshold;
- replaces the two-step post-climb look movement with one cubic camera curve and one cubic look-target curve;
- preserves the accepted screen power direction and content fade-in.

Acceptance still requires Unity compilation, `TEST EVERYTHING 0/0/0`, and real-time visual review.
<!-- BND_CHILD_APPROACH_CINEMATIC_FADE_SPEED_SMOOTHING_V10930:END -->

<!-- BND_CHILD_APPROACH_CINEMATIC_PATH_CLEARANCE_V10929:BEGIN -->
## 2026-06-11 — Child approach distance and chair-clearance polish V10.9.29

**Classification:** `CURRENT / ACTIVE CINEMATIC POLISH`

Automated QA was green after V10.9.28. The remaining visual defects were that the child began too close to the chair, was not far enough left, and the climb path intersected the chair.

V10.9.29 moves the opening camera substantially farther back and left while preserving the accepted child-eye POV height. The walk is extended to six restrained steps. The climb is now a three-stage physical route: approach the left-rear corner, rise while remaining outside the chair's left edge, reach a safe seat-side position in front of the backrest, and only then move inward over the seat. A runtime clearance guard prevents the camera from entering the chair before the backrest has been cleared.

The screen-off and professional power/content reveal from V10.9.28 remain unchanged.
<!-- BND_CHILD_APPROACH_CINEMATIC_PATH_CLEARANCE_V10929:END -->

<!-- BND_CHILD_APPROACH_CINEMATIC_POLISH_V10928:BEGIN -->
## 2026-06-11 — Child approach cinematic direction and power-on polish V10.9.28

**Classification:** `CURRENT / ACTIVE CINEMATIC POLISH`

Automated QA is already green. The active issue is visual quality: the child POV was too low, the approach did not read as beginning behind the chair, the movement felt mechanical, and tutorial content appeared during power-on without a professional reveal.

V10.9.28 raises the opening POV to the earlier cinematic height language, places it physically behind the chair, corrects the chair orientation so its backrest is away from the table, reduces walk bob/roll, replaces linear climb segments with authored curved arcs, and adds a delayed fade/vertical settle/micro-scale content feed after the screen glass and backlight wake.

The current task remains visual realtime acceptance of the complete first-launch shot.
<!-- BND_CHILD_APPROACH_CINEMATIC_POLISH_V10928:END -->

<!-- BND_CHILD_APPROACH_CINEMATIC_V10927:BEGIN -->
## 2026-06-11 — Child approach, chair climb and screen power-on V10.9.27

**Classification:** `CURRENT / ACTIVE CINEMATIC IMPLEMENTATION`

The current task is the first-launch post-intro shot leading into the tutorial. The shot is now defined as a single child point-of-view take:

1. begin behind the chair, slightly left, at a small child's eye height;
2. walk to the chair with restrained step bob and lateral weight transfer;
3. slow and look upward at the chair/table;
4. climb onto the chair in two weighted stages without a cut or teleport;
5. settle, lean toward the handheld and reach the exact tutorial camera pose;
6. keep the physical display fully off through all movement;
7. power the display from black only after camera settlement;
8. reveal tutorial content during the power-on and preserve the same final state when skipped.

The previous pre-tutorial white screen is explicitly retired. The display canvas, screen camera, display color/emission and interaction targets are all disabled while powered off; hiding UI alone is not accepted.

**Exact resume point:** install V10.9.27, let Unity compile, run `Boredom And Dungeons -> TEST EVERYTHING`, then review the complete realtime shot from BBH handoff through tutorial readiness at low and high frame rates.
<!-- BND_CHILD_APPROACH_CINEMATIC_V10927:END -->

<!-- BND_POST_INTRO_CHAIR_AND_QA_REPAIR_V10926:BEGIN -->
## 2026-06-11 — Post-intro chair + QA access repair V10.9.26
- Added a centered wooden dining chair in front of the table, aligned with the handheld.
- The chair uses the new `BDCinematicDiningChair` texture resource and stays in the same physical room staging.
- Added chair geometry and chair contact shadows in `BDModernHandheld3DPresenter.CinematicEnvironment.cs`.
- Repaired the Unity editor compile blocker by changing `BDFirstLaunchTutorialQA.Scan()` from private to internal so `BDOneClickQAWindow` can call it.
- Runtime focus: no limbo/cyclorama reintroduction; room staging remains the active implementation.
<!-- BND_POST_INTRO_CHAIR_AND_QA_REPAIR_V10926:END -->

<!-- BND_POST_INTRO_REAL_ROOM_AND_CLOSER_FRAMING_V10925:BEGIN -->
## 2026-06-11 — Real-room environment and closer final framing V10.9.25

**Classification:** `CURRENT / ACTIVE VISUAL IMPLEMENTATION`

The active post-intro cinematic remains the current task. The latest visual review identified three concrete problems: the final shot should be slightly closer to the handheld, the wide shot exposes a dark cyclorama/ramp, and the surrounding set reads as a black limbo rather than a real room.

V10.9.25 implements the correction in the game:

- removes the runtime cyclorama/ramp geometry completely;
- replaces the charcoal stage floor with a large warm wooden room floor that extends beyond every cinematic camera boundary;
- keeps no side walls in camera and uses only the distant fruit-wallpaper back wall;
- removes the visible credenza, plant, dark framed panels and other dressing from this shot so the composition reads as table + real floor + back wall;
- adds a physical baseboard where the floor meets the wallpapered wall;
- brightens the room with a warm room fill and a dedicated wallpaper wall wash;
- moves the final camera modestly closer and narrows the final lens while preserving the complete handheld and the small lower tabletop margin;
- preserves the device, table proportions, one-camera spline, live screen, subtle DOF, tutorial flow and gameplay.

**Exact resume point:** apply V10.9.25, wait for Unity compilation, run `Boredom And Dungeons -> TEST EVERYTHING`, require `0 blockers / 0 warnings / 0 info`, then review the opening/wide shot and final frame.
<!-- BND_POST_INTRO_REAL_ROOM_AND_CLOSER_FRAMING_V10925:END -->

<!-- BND_POST_INTRO_FINAL_FIRST_LAUNCH_QA_REPAIR_V10924:BEGIN -->
## 2026-06-11 — Final first-launch cinematic QA repair V10.9.24

**Classification:** `CURRENT / FOCUSED AUTOMATED-QA REPAIR`

The latest Unity evidence contains one blocker only:

```text
FIRST_LAUNCH_TUTORIAL_CONTRACT_INVALID
Missing first-launch tutorial camera-only full-set intro-to-main-menu cinematic token:
new Vector3(0f, -7.22f, -3.94f)
```

V10.9.22 successfully aligned the authoritative handheld QA contract with the installed Runtime, but the separate named first-launch camera contract remained inconsistent in the local repository variant.

V10.9.24 does not assume a Runtime field name or a historical numeric version. It:

1. reads the current `HANDHELD_3D_PHYSICAL_STAGING_MISSING` Require block from `BDModernHandheld3DQA.cs`;
2. identifies its single final-look target (`new Vector3` with the table/device look-axis Y);
3. verifies that exact target exists in `BDModernHandheld3DPresenter.IntroToMainMenuTransition.cs`;
4. replaces any stale final-look target in the named first-launch camera contract, or inserts the current target if that contract does not contain one;
5. leaves every Runtime, camera, room, wallpaper, depth-of-field, device, table, shader, and gameplay file byte-identical.

**Exact resume point:** apply V10.9.24, wait for Unity compilation, run `Boredom And Dungeons -> TEST EVERYTHING`, require `0 blockers / 0 warnings / 0 info`, then continue visual review.
<!-- BND_POST_INTRO_FINAL_FIRST_LAUNCH_QA_REPAIR_V10924:END -->

Resolved authoritative final-look target: `new Vector3(0f, -7.18f, -4.18f)`.

<!-- BND_POST_INTRO_CINEMATIC_WALLPAPER_FOCUS_DELIVERY_REPAIR_V10916:BEGIN -->
## 2026-06-11 — Post-intro wallpaper and focus polish V10.9.16

**Classification:** `CURRENT / ACTIVE VISUAL IMPLEMENTATION`

**Repository baseline:** commit `38ac49268a9afa0d711bd3cb8c1668c49cdc079f` with V10.9.13 installed locally.

The active post-intro cinematic remains the current task. The next polish pass addresses the latest visual direction:

- reduce or vastly improve the blur so it stays subtle and clean;
- end the shot with the entire handheld visible in frame;
- keep a small visible tabletop strip below the handheld in the final frame;
- add a kitchen-cartoon style wallpaper treatment so the room feels less generic.

V10.9.16 consolidates those requests in one patch over the V10.9.13 baseline:

- final direct camera becomes `(0, -1.58, -3.94)` with final look target `(0, -7.22, -3.94)`;
- responsive final lens becomes `Mathf.Lerp(40.5f, 34.6f, fit)` so the whole device stays inside frame with a tight professional border;
- depth-of-field becomes full-resolution and much softer: near `2.15`, far `8.20`, blur strength `0.28`, max blur `2.10`;
- the dressed room gains a patterned wallpaper pass built in code (`BD Cinematic Kitchen Wallpaper`) and dedicated wallpaper panels on the back and side walls;
- existing room dressing, table scale, one-camera spline, live screen, tutorial flow and physical device placement stay intact.

**Delivery correction:** the first V10.9.15 installer stopped before validation because it expected DOF numeric tokens that had never existed in either QA source. Automatic rollback restored V10.9.13 byte-for-byte. V10.9.16 inserts the new DOF contracts into the real existing QA blocks instead of replacing nonexistent anchors.

**Exact resume point:** apply V10.9.16, allow Unity to compile, run `Boredom And Dungeons -> TEST EVERYTHING`, require `0 blockers / 0 warnings / 0 info`, then visually confirm the blur quality, the full-device final framing, the small bottom wood margin, and the wallpaper feel.
<!-- BND_POST_INTRO_CINEMATIC_WALLPAPER_FOCUS_DELIVERY_REPAIR_V10916:END -->

<!-- BND_POST_INTRO_CINEMATIC_QA_LATEST_COMMIT_ALIGNMENT_V1094:BEGIN -->
## 2026-06-10 — V10.9.4 latest-commit-aligned QA ownership repair

**Latest repository baseline:** this package is rebuilt from commit `ebe0eb6c40eb2ba291fd5cc23edcd4eac2ecf572` (`prepare for codax`). It preserves the new 30-agent/5-skill Codex routing system, `.codex/config.toml`, `docs/agent-system/*`, `scripts/agent-system/validate_codex_agent_system.py`, the committed V10.9.1 cinematic Runtime files and all unrelated commit content. The installer owns only the focused QA file and synchronized maintained documentation; it refuses to run when the latest baseline is absent.

**Observed Unity truth:** `TEST EVERYTHING` ran at `2026-06-10T19:16:43.2950910Z` and reported `1 blocker / 0 warnings / 0 info`. The only blocker was `HANDHELD_3D_PRESENTER_MISSING`, claiming that `Short Core Shadow To Left` was absent.

**Root cause:** V10.9 intentionally retired the old vertical-product-shot shadow name and moved the grounded shadow geometry into `BDModernHandheld3DPresenter.CinematicEnvironment.cs` as `Device Soft Contact Penumbra`, `Device Core Contact Shadow`, `Device Base Contact Shadow` and `Table Leg Contact Shadow`. `BDModernHandheld3DQA` still scanned only the base presenter partial and required the retired token.

**Implemented:** the focused handheld validator now keeps base-presenter checks in the base partial, validates the complete table/floor/cyclorama/light/shadow contract in the cinematic-environment partial, and explicitly rejects the retired plane-table objects there. No Runtime behavior, camera path, model, material, input or tutorial mechanic is changed.

**Package-alignment correction:** V10.9.3 correctly stopped before writing, but its preflight incorrectly expected the literal skill identifiers `requirement-ledger` and `final-integration-gate` inside `AGENTS.md`. In the actual latest commit, `AGENTS.md` expresses those responsibilities as prose while the identifiers live in their own `SKILL.md` files. V10.9.4 validates each responsibility in its authoritative file, adds `docs/agent-system/REPOSITORY_RULES.md` to the baseline, and hashes all protected agent-system, scene and cinematic Runtime files before writing and after validation.

**Verification truth:** package/static validation is complete. Unity compilation and a fresh `TEST EVERYTHING` result are required. The exact resume point is to apply V10.9.4, wait for Unity compilation, rerun the single QA command, and require `0 blockers / 0 warnings / 0 info` before visual review or commit.
<!-- BND_POST_INTRO_CINEMATIC_QA_LATEST_COMMIT_ALIGNMENT_V1094:END -->

<!-- BND_POST_INTRO_CINEMATIC_DIRECTOR_PASS_V109:BEGIN -->
## 2026-06-10 — Post-intro cinematic director pass V10.9

**Classification:** `CURRENT / EARLIER-BLOCKING VISUAL-ARCHITECTURE REPAIR INSIDE THE FIRST-LAUNCH GATE`

**User correction:** the previous camera-only repair still built the visible "table" as one oversized vertical `Quad`, used a short UI-like interpolation and did not establish a complete grounded piece of furniture. The new supplied director/staging contract requires a real table, floor and cyclorama, one physical camera path, approximately 4.4 seconds at the 24 FPS reference cadence, early horizontal alignment, a long settle, and a final visible tabletop edge/thickness.

**Implemented in this package:** `BDModernHandheld3DPresenter` keeps the approved device unchanged and generates a full table with a thick top, front lip, four aprons, four connected legs and feet; a charcoal floor; a curved cyclorama; restrained product lighting; device contact shadows; and leg contact shadows. `IntroToMainMenuTransition` now uses a precomputed allocation-free five-knot natural cubic spline with independent horizontal, vertical, forward, look and FOV clocks. The camera begins high/far/left, moves from the first frame, aligns horizontally before the final settle and ends on the same centralized Main Menu position/rotation/FOV/clip state used by regular entry.

**Preservation:** the table, device, screen, shadows, floor and backdrop are static for the entire shot. No cut, camera swap, fade, scale animation, object movement, ghosting, model replacement, package upgrade or unrelated gameplay change is introduced. All V10.8.1 tutorial mechanics and terminal semantic colors remain protected.

**Delivery V10.9.1 correction:** the first real V10.9 application correctly wrote into a backup-protected working tree, but its post-write validator falsely treated the bug ledger's inline decorative `` `=======` `` prose as a Git merge conflict. Automatic rollback restored the repository byte-for-byte and removed the failed-attempt backup/package residue. V10.9.1 now recognizes only actual full-line Git conflict markers, includes a regression self-test for the existing bug-ledger wording, and removes double-clickable `.command` launchers. Installation is run as a child Python process inside the user's existing Terminal, so completion or failure returns to the normal prompt.

**QA truth:** V10.9.1 package tooling, full-line conflict-parser regression tests, structural source validation, C# delimiter/preprocessor checks, terminal-color compatibility tests, first install, idempotent install, unknown-transition-content blocking, rollback and `git diff --check` are package/static gates only. Unity compilation, TEST EVERYTHING, frame-by-frame rendering, 24/30/60 FPS behavior and user visual acceptance are not claimed.

**Exact resume point:** apply V10.9.1 to the current repository from the existing Terminal session, open Unity `6000.0.76f1`, require a clean compile and `Boredom And Dungeons -> TEST EVERYTHING` at `0 blockers / 0 warnings / 0 info`, then review the shot at 0.0s, 1.3s, 2.2s, 3.3s and the final frame. Confirm the full table/floor at the start, natural leg exit, visible front edge/thickness at the end, live readable screen, zero handoff jump and no replay on internal menu returns. Do not commit before user acceptance.
<!-- BND_POST_INTRO_CINEMATIC_DIRECTOR_PASS_V109:END -->

<!-- BND_FIRST_LAUNCH_TUTORIAL_V1081_HOTFIX:BEGIN -->
## 2026-06-10 — V10.8.1 shooting progression, camera-only landing and terminal-output compliance

**Classification:** `EARLIER/BLOCKING HOTFIX INSIDE CURRENT FIRST-LAUNCH TUTORIAL TASK`

**User regression:** after the mounted shooting lesson, the visible enemy could be hit and removed but the course remained stuck because the production shot transaction was always created with `advancesLesson: false`. The lesson therefore never reached its impact-owned transition to Reload.

**Implemented repair:** `FireFirstLaunchTutorialProductionShot` now marks only the real `RangedAttack` lesson shot as progression-capable. `ResolveFirstLaunchTutorialRangedProjectileImpact` completes the lesson only when that visible projectile reaches impact against a living target, then moves to Reload exactly once. Firing, animation completion or a miss cannot advance the lesson. The duplicate invalid charged-shot expression found during the audit is also removed.

**Camera clarification implemented:** the post-BBH scene remains one full-screen 3D table environment at all times. The table, device, screen and shadow remain at their authoritative rest transforms. Only the already-present 3D camera position, rotation and field of view animate along a safe dolly path. The table geometry receives additional off-screen coverage so the camera never exposes empty background during the establishing angle.

**Delivery repair:** the cumulative installer and validator now follow `ProjectGuide/Rules/TERMINAL_OUTPUT_STANDARD.md`: interactive PASS is bold green, BLOCKED/ERROR bold red, WARNING bold yellow, INFO cyan and CLEANED magenta; copied/non-interactive output keeps explicit prefixes without ANSI; `NO_COLOR` and `TERM=dumb` are respected.

**Preservation:** this is an additive repair. No tutorial lesson, mechanic, input path, visual asset, checkpoint, boss behavior, prior V10.8 repair or unrelated gameplay system is removed. The supplied retro tutorial redesign and new-enemy/model/difficulty requirements are preserved verbatim in queued maintained task documents and do not displace the current blocker gate.

**QA truth:** final package verification passes on both the supplied pre-V10.8 local state and the already-applied V10.8 state. Verified evidence: 34/34 owned target hashes, first install, cumulative install, idempotent no-rewrite run, unknown-local-change preflight with zero writes/no backup, repeated byte-for-byte rollback, pseudo-terminal green/red/cyan/magenta output, ANSI suppression under `NO_COLOR=1`, `TERM=dumb` and redirected output, cleanup of the exact source ZIP and extracted artifacts on success/failure, ZIP path safety/uniqueness (1004 members), full snapshot equality (962 files), exact Git changed set (34/34), `git diff --check`, source stability scan (0 blockers/0 warnings) and ProjectGuide hygiene PASS. Unity compilation, `TEST EVERYTHING`, rendered camera behavior and focused Play Mode remain unverified and are not claimed.

**Exact resume point:** install V10.8.1, reset the first-launch tutorial, reach mounted shooting, confirm that the enemy dies only at projectile impact and that the lesson immediately proceeds through Reload to Charged Shot. Then verify the post-BBH camera-only table scene and terminal colors. Do not commit before `0 blockers / 0 warnings / 0 info` and user acceptance.
<!-- BND_FIRST_LAUNCH_TUTORIAL_V1081_HOTFIX:END -->

<!-- BND_FIRST_LAUNCH_TUTORIAL_MECHANICS_REPAIR_V108:BEGIN -->
## 2026-06-10 — First-launch tutorial mechanics, readability and 3D cinematic repair V10.8

**Classification:** `CURRENT / EARLIER-BLOCKING USER PLAY-MODE REGRESSION REPAIR`

**Previous:** V10.7.2 corrected post-BBH landing/installer validation while the playable tutorial remained open for focused user review.

**Current:** the latest user run reopened ten tutorial defects. V10.8 repairs them in the existing presenter and production-course owners: injured-horse remount is rejected; player, horse and enemy locomotion uses real alternating leg frames; player ranged damage resolves only when the visible projectile reaches impact; Hook damage/progression resolves only after the pull presentation completes; death restores a nearby stable lesson checkpoint; tutorial enemies physically block the player; decorative lesson-divider lines/gates are removed while invisible progression clamps remain; the final boss keeps persistent instructions, readable telegraph/impact/recovery states and only accepts damage during a safe recovery opening; a mechanics-faithful Charged Shot lesson is added; and the BBH handoff uses a full-screen real-3D camera/device move rather than screen-space or slide-like interpolation.

**Charged Shot truth verified from `BDPlayerCombat`:** press/hold begins with the production `0.22s` threshold; full charge duration is `min(3.20s, 0.90s + 0.45s × rounds above two)`; full charge fires automatically without release; release before the threshold produces an ordinary shot; release after charge begins cancels; the charged shot consumes all remaining ammunition and starts Reload immediately.

**Implementation truth:** source and QA/documentation repairs exist in the local working tree. No parallel gameplay, damage, camera or input owner was introduced. Projectile and Hook effects are presentation transactions whose completion calls the existing tutorial damage/progression owner exactly once.

**QA truth:** C# tree-sitter structure, duplicate-member/signature scans, QA token simulation, repository stability/hygiene scans, ZIP integrity, first and idempotent installation, unknown-change blocking, byte-for-byte rollback, `git diff --check`, exact 27-file changed-set verification, 993-entry package-manifest verification and 960-file complete-snapshot comparison all pass. Unity compilation, TEST EVERYTHING, Play Mode, rendered timing, performance and user visual/gameplay acceptance are not yet claimed.

**Next:** apply the statically/package-verified V10.8 ZIP, compile in Unity `6000.0.76f1`, run `Boredom And Dungeons -> TEST EVERYTHING`, then perform the complete focused run in `ProjectGuide/QA/FIRST_LAUNCH_TUTORIAL_PRODUCTION_COURSE_V10.md`.

**Exact resume point:** begin at a reset first-launch state; verify the entry choice and full-screen 3D handoff, then complete one uninterrupted tutorial run while intentionally testing early/late Charged Shot release, injured-horse remount, enemy body blocking, death in several late lessons, mounted projectile impact, Hook pull completion and both boss phases. Do not commit until automated output is `0 blockers / 0 warnings / 0 info` and the user accepts the result.
<!-- BND_FIRST_LAUNCH_TUTORIAL_MECHANICS_REPAIR_V108:END -->

<!-- B&D 2026-06-09 FIRST LAUNCH + HANDHELD PRODUCTION PATCH START -->
## 2026-06-09 — First-launch tutorial, direct handheld repair and local-delivery correction

**Classification:** `CURRENT + EARLIER/BLOCKING PROCESS/ARCHITECTURE REPAIR`

**Previous:** merged V6 companion implementation and user visual review.

**Current:** local patch package migrates accepted V6 behavior into authoritative owners, restores title-aligned context art, removes physical blue hover, implements unified tactile controls, repairs all relevant text/card bounds, supplies internal Pause, implements the complete first-launch tutorial contract, and integrates the supplied BBH cinematic side task.

**Next:** user applies package, opens Unity, runs `TEST EVERYTHING`, executes the focused Play Mode list and returns real output/screenshots. No commit before acceptance.

**Process correction:** assistant Git/GitHub writes are permanently prohibited. Delivery is local patch ZIP only, with backup, rollback, validator and user-owned commit.

**QA truth:** package/static validation only. Unity compilation, TEST EVERYTHING, runtime performance and visual acceptance are not yet claimed.

**Exact resume point:** apply the package with `apply_patch.command`; after PASS, open Unity `6000.0.76f1`, run `Boredom And Dungeons -> TEST EVERYTHING`, then follow `VERIFY_AFTER_APPLY.txt`.
<!-- B&D 2026-06-09 FIRST LAUNCH + HANDHELD PRODUCTION PATCH END -->


## V5 CONTROL, LAYOUT AND PRODUCT-SHOT REPAIR — IMPLEMENTED / UNITY VERIFICATION REQUIRED

The latest user review accepted the overall direction but reopened control semantics, physical-control materials, page alignment, New Game card stacking, glass response, visible left shadow and composition height. V5 implements the final contract: Main Menu X=New Game, A=Progression, B=Settings, Y=Credits; B=Back on all non-main pages; center-left=SELECT; center-right=EXIT confirmation. It uses cleaned source-sheet textures on real 3D controls, separates page columns, stacks New Game cards, strengthens directional glass glint and short left shadow, and raises the device slightly higher on the table. Automated Unity verification has not yet been run on this revision.

# Current Development Snapshot

## 2026-06-09 — Final V5 control/layout/product-shot repair implemented

The latest user correction supersedes all earlier prototype mappings. Main Menu now uses X=New Game, A=Progression, B=Settings and Y=Credits. B is Back on every non-main page. The left black center button is SELECT and the right black center button is EXIT with an in-screen confirmation. The revision also raises the device, cleans the approved control textures for use on real 3D caps, strengthens but constrains the left shadow and upper-right glass glint, aligns page columns/footer, and stacks the New Game hero/text cards without clipping. Status: `IMPLEMENTED / UNITY VERIFICATION REQUIRED`; no commit/push until TEST EVERYTHING and user visual acceptance.


```text
Classification: FOCUSED PHYSICAL-MODEL REPAIR INSIDE CURRENT HANDHELD TASK
Active item: Replace the flat full-face decal treatment with a real molded 3D shell, restore a visible short left shadow, simplify the New Game-only memory card, add WASD navigation, and add a restrained upper-right glass glint
Source basis: latest local project, successful automated TEST EVERYTHING result, user Play Mode screenshot and direct visual/interaction corrections
Latest Unity result: AUTOMATED PASS; user visual acceptance failed because the device still looked flat/fake, the full-face texture overlapped modeled controls, no readable device shadow appeared, the small card exposed route/Mother data and duplicated art, and WASD/glass-light behavior were incomplete
Implementation state: V4 PHYSICAL MATERIAL / DEPTH / SHADOW / CARD / INPUT / GLASS REPAIR IMPLEMENTED / UNITY VERIFICATION REQUIRED
Exact resume point: install this full-project package, compile, run TEST EVERYTHING, then inspect the molded shell depth, left-cast shadow, absence of any front decal over controls, New Game-only text card, neutral top bar, WASD parity and upper-right glass glint before any commit
```

<!-- B&D BBH CINEMATIC SIDE TASK V1 START -->
## 2026-06-09 — Side task: cinematic BBH boot-intro motion

**Classification:** `SIDE TASK / DOES NOT CHANGE ACTIVE HANDHELD PRIORITY`

The user requested a more cinematic, premium-family-animation feeling for the BBH boot intro, with the letters behaving like expressive physical elements and with a somewhat larger circle when it appears. The existing active handheld item, blocker state, work queue and exact resume point remain unchanged.

Implementation supplied for integration:

- distinct motion profiles for the careful first B, confident second B and heavy stabilizing H;
- subtle second-B contact reaction, H shared landing reaction and circle-expansion lift;
- one deterministic completion breath instead of perpetual real-time pulsing;
- circle activation from a gathered point with controlled overshoot;
- desired circle diameter increased by 16%, with responsive width/height/edge clamps;
- focused TEST EVERYTHING coverage and maintained documentation updates.

Verification truth: package/static validation can be performed outside Unity, but Unity compilation, TEST EVERYTHING and focused Play Mode across aspect ratios are still required. No verified visual claim is made yet.
<!-- B&D BBH CINEMATIC SIDE TASK V1 END -->

## 2026-06-09 — Automated pass followed by user visual rejection; V4 repair

The post-V3 build reached `B&D TEST EVERYTHING: AUTOMATED PASS`, proving compilation and the automated contract were intact. The user nevertheless rejected the actual product shot in Play Mode. Automated success is not visual acceptance.

Observed defects:

- the full-face transparent sticker still read as a flat image and visibly crossed/covered modeled lower controls;
- the device lacked convincing side thickness, mold separation and product-volume cues;
- the requested short shadow to the left was not visibly readable on the wood;
- the small lower-right run card duplicated artwork and exposed `BOY/GIRL ROUTE` and `MOTHER` text that does not belong there;
- the small card should exist only for a fresh Start Game/New Run selection and be text-only;
- `WASD` must navigate exactly like the arrow keys;
- glass must receive a restrained upper-right directional glint matching the key light without obscuring screen content.

Implemented V4 response:

- removes the runtime full-face decal entirely; the supplied design now informs a molded shell material rather than being pasted across controls;
- upgrades the shell shader to an object-space blue→violet→orange molded gradient with micro-surface variation, side darkening, specular response and controlled rim light;
- increases physical body depth, adds a rear core, outer molded edge bevel and side mold seams, and moves screen/control layers to real front-surface depths;
- adds a dedicated shadow shader plus separate soft penumbra, core shadow and contact shadow, offset left under the approved upper-right key light;
- keeps the small memory card only on a fresh New Game/Start Game selection, removes its image, route identity and Mother status, and removes character labels from the top bar;
- adds W/A/S/D to input-release gating and directional navigation for both Input System and legacy input;
- adds a subtle animated upper-right glass glint with low opacity and no center-screen washout;
- removes the obsolete runtime decal shader/asset and updates QA/documentation ownership accordingly.

This is not verified until Unity compilation, TEST EVERYTHING and focused visual/interaction inspection pass.


## 2026-06-09 — Superseded V3 refinement: texture, layout and contextual artwork

The user confirmed that the previous repair is much closer, then identified three remaining production defects: the shell/front texture looked low quality, the center shortcut labels and some screen layout elements were still visually broken, and the New Game character artwork was reused on unrelated pages.

Implemented repair now present:

- replaces the 512px shell source with a 2048px premium blue→violet→orange micro-textured surface;
- replaces the front sticker with a 2048×3220 high-resolution masked decal and a dedicated lit transparent decal shader;
- replaces the broken merged shortcut text with two compact, separately positioned recessed 3D labels—`SELECT` and `EXIT`—while keeping both center buttons fully modeled and interactive;
- reduces long single-line title sizing so `PROGRESSION` cannot collide with the right artwork panel;
- adds dedicated character-neutral artwork for Progression, Settings, Credits, Quit/Return and Resume/Pause;
- changes the right artwork on Main and Pause as selection changes;
- keeps Boy/Girl switching exclusively on Start Game / New Run. Every other option uses one character-neutral asset and therefore does not require duplicate Boy/Girl production.

This V3 repair reached automated PASS but its full-face decal approach was rejected visually and is superseded by the V4 physical-material repair above.

## 2026-06-09 — First Play Mode handheld rejection and focused repair

The first runtime visual reached Play Mode, proving the uGUI compilation dependency was resolved, but it was not acceptable. The live menu RenderTexture remained at its clear color, the Pause page closed again immediately after Escape, 3D TextMesh labels were oversized/duplicated around the controls, and only one face-button hit area responded reliably.

Focused repair now present:

- the internal UI Canvas uses `ScreenSpaceCamera` at the exact `960×1080` RenderTexture size instead of a fragile world-space alignment;
- the screen camera is explicitly rendered after page construction and before the product camera consumes the texture;
- new menu input is armed only after the opening Escape/mouse/gamepad press has been released, preventing Pause from instantly resuming;
- X/Y/A/B use separate enlarged invisible hit targets that animate the real modeled buttons;
- hardware letters and SELECT/EXIT labels are small, front-facing siblings rather than oversized children inheriting rotated button transforms;
- TEST EVERYTHING now guards the screen-render, input-arming and physical-hit-target contracts.

This is not verified until Unity shows real live menu content in the screen and all focused interactions pass.


## 2026-06-09 — Handheld compilation blocker and repair

Unity compilation failed because `BDModernHandheld3DPresenter` uses the GameObject-based UI types `Image`, `Text`, `RawImage` and `Outline`, while `Packages/manifest.json` did not declare the `com.unity.ugui` package that supplies those Runtime components. The project only declared `com.unity.modules.ui`, which is not the uGUI package.

Repair now present:

- `Packages/manifest.json` declares `com.unity.ugui` version `2.0.0`;
- `BDModernHandheld3DQA` blocks future removal or version drift of that dependency;
- no menu behavior, visual requirement, input mapping or existing state authority was changed;
- Unity must resolve the package and compile before this repair can be verified.

## Newly captured follow-up — seamless handheld/gameplay camera transition

After the base 3D handheld compiles and passes focused Play Mode, the next approved presentation stage expands the existing professional opening-cinematic task:

- gameplay is already visible inside the handheld screen before entering;
- the menu camera zooms into the physical screen without a visible cut, black frame, perspective jump or post-processing mismatch;
- the gameplay camera is prepositioned high above the map, with gameplay HUD hidden, before handoff;
- the gameplay opening dive starts only after the screen fills the viewport and camera handoff is complete;
- exit/abandon performs the reverse sequence back into the handheld screen and then zooms out to the table product shot;
- state transitions are completion-driven and input-locked, not based on fragile fixed delays;
- the existing handheld, menu, gameplay, camera and UI designs remain protected.

The full requirement is merged into `ProjectGuide/Tasks/QUEUED/PROFESSIONAL_OPENING_CINEMATIC.md`; it is not implemented in this compile-repair package.

## Modern 3D handheld implementation

Status: **IMPLEMENTED / UNITY VERIFICATION REQUIRED**.

The Runtime now contains a real 3D menu device rather than a flat device image:

- procedural upright shell geometry with real thickness and a physical screen opening;
- approved blue→violet→orange shell texture;
- separate bezel, backing, emissive display, transparent glass and reflection layer;
- real modeled D-pad, A/B/X/Y, SELECT, EXIT, speaker inserts and status light;
- one isolated screen camera and one cached screen RenderTexture;
- Main Menu, Pause, Settings, Progression, Credits, Abandon confirmation and Loading pages inside the screen;
- mouse hover/click on screen rows and physical buttons;
- D-pad/arrow/WASD navigation; Main Menu X=New Game, A=Progression, B=Settings, Y=Credits; B=Back elsewhere; center SELECT activates and center EXIT confirms exit;
- tactile transform/material feedback and a cached click sound;
- contextual artwork: only Start Game / New Run uses active-character art—Boy shows Boy and Girl shows Girl; every other option/page uses one dedicated character-neutral image; selection is never random;
- legacy flat menu and backdrop are suppressed only while the 3D presenter owns the menu;
- new TEST EVERYTHING coverage validates the presenter, flow bridge, shaders, paired art and documentation truth.

No claim is made yet for Unity compilation, visual quality, input behavior, cleanup, performance or user acceptance.

## Latest approved physical-scene refinement

The user clarified that the in-game result must not be a rendered screenshot or a flat picture of a handheld. The implemented scene now follows this exact production contract:

- the uploaded orthographic handheld sheet is used only as a masked front-surface decal on the generated 3D shell; transparent cutouts leave the live screen and modeled controls unobstructed;
- the shell, back, bezel, display, glass, reflection, D-pad arms, face buttons, center shortcuts and speaker inserts remain separate 3D parts;
- the uploaded dark-wood image is the actual table source texture; a paired sharp/defocused texture set is blended in the table shader so focus falls off gradually toward the near and far table regions instead of applying one uniform blur;
- device, table and shadow share the same product-shot plane; the upper edge of the handheld is slightly farther from the camera;
- the key-light response comes from above/right and the short soft shadow falls left;
- all Main, Pause, Settings, Progression, Credits, abandon-confirmation and loading content stays inside the physical screen behind glass;
- page changes use a short modern-handheld shutter/flash/scanline transition inside the display;
- active Boy identity displays Boy art and active Girl identity displays Girl art only while Start Game / New Run is selected; Pause/Resume, Progression, Settings, Credits, Quit/Return and confirmation use dedicated character-neutral art;
- each D-pad direction now has its own modeled moving cap and tactile feedback rather than an invisible click target only.

Static source checks pass after removing obsolete intermediate texture exports. Fresh Unity compilation, TEST EVERYTHING, Play Mode, profiling and user approval remain mandatory.

## Documentation/tooling change in this package

- All maintained project knowledge moves to `ProjectGuide/` and out of Unity `Assets/`.
- Duplicate art/audio mirrors and one-off historical QA report files are removed after durable truth is retained.
- Documentation QA and every hard-coded feature-document path are migrated to the new hierarchy.
- The stability scanner incorrectly treated nested helper types such as `State`, `Runner` and `Bootstrap` as duplicate namespace-level types. The scanner now ignores declarations nested deeper than the project's namespace-level indentation. The source scan passes with 0 blockers and 0 warnings after this repair.
- No gameplay/runtime behavior is intentionally changed by the Project Guide migration.
- Package V1.1 repairs the validation chain discovered during local application/testing: macOS `.DS_Store` metadata blocked hygiene after installation, a brittle exact-phrase token rejected the valid `real upright 3D handheld` requirement, and the hygiene tool rejected the package manifest before validation could finish. The corrected sequence removes OS metadata first, validates stable concepts, permits only its own transient package files during validator execution, performs a strict final hygiene pass after cleanup, runs fail-fast, and deletes the ZIP only after all checks pass.
- Unity `TEST EVERYTHING` reran at `2026-06-09T00:13:48.3411810Z` after V1.2 and passed with 0 blockers, 0 warnings and 0 info. The ProjectGuide migration and historical QA discovery compatibility are verified at the automated level.

## Current user decisions

- All maintained project knowledge is organized under `ProjectGuide/`; root keeps only the public README and AI operating contract.
- Guide content must remain concise at entry level and detailed in linked specifications.
- New documents are created when they have a distinct owner; obsolete/duplicate/history-only documents are merged then removed.
- The in-game Main and Pause device is real 3D, not a flat image.
- Every Boy image requires a matched Girl image.
- User-facing label is `Progression`.
- Active character identity controls the paired Start Game / New Run art only: Boy gameplay shows Boy art and Girl gameplay shows Girl art; the selection is never random. Every other option remains character-neutral.

---

# Boredom & Dungeons — Authoritative Project Status and Ordered Work

## Current development snapshot

```text
Status date: 2026-06-09
Classification: CURRENT / USER-APPROVED 3D HANDHELD MENU SPECIFICATION
Active work: C11.UI.MODERN_HANDHELD_3D.V1
Current truth: The user explicitly reprioritized the main menu and Escape/Pause presentation before returning to the previously saved Runtime/QA repair sequence. The approved direction is an original upright portrait handheld with a real 3D shell, blue-to-orange molded-plastic gradient, separate tactile 3D controls, and a lit display recessed behind clear glass/transparent plastic with visible depth. Mouse, D-pad, arrows and WASD navigation are required. Main Menu X=New Game, A=Progression, B=Settings, Y=Credits; B=Back on every non-main page; center SELECT activates focus and center EXIT opens the legal confirmation. The user-facing label is `Progression`, not `Meta Progression`.
Implementation truth: The full Runtime vertical slice is implemented with generated 3D device geometry, cached screen RenderTexture, recessed display, glass/reflection layers, modeled controls, tactile feedback, Main/Pause/Settings/Progression/Credits/Abandon/Loading pages, mouse and hardware-style navigation, premium shell/decal sources and context-specific option artwork. Active-character Boy/Girl selection is restricted to Start Game / New Run.
Character-art truth: Any image that depicts the playable Boy requires a matched Girl version with identical dimensions, crop, composition, lighting, horse/background placement, safe areas and import settings. In the handheld, only Start Game / New Run uses this pair. Progression, Settings, Credits, Quit/Return, Resume/Pause and confirmation are character-neutral, single-source assets. Active identity is authoritative only for the New Game pair and selection is never random.
Verification truth: ProjectGuide V1.2 passed TEST EVERYTHING at 2026-06-09T00:13:48.3411810Z. That pass predates this Runtime implementation. Fresh Unity compilation, TEST EVERYTHING, focused Play Mode, target-device performance and user acceptance are required.
Current action: install the premium texture/layout/context-art full-project package, run Unity compilation and TEST EVERYTHING, then verify Main/Pause visuals, all mouse/D-pad/A-B-X-Y/shortcut interactions, cleanup/performance, unique option artwork, and Boy→Boy / Girl→Girl selection only on Start Game / New Run. Do not return to enemy animation or the prior repair queue until this UI stage is verified or explicitly deferred.
Saved interrupted resume point: ProjectGuide/Status/WORK_QUEUE.md retains the previous QA/target-outline/animation order for later return.
```


This file is the only live source for current status, ordering, blockers, verification truth, and the resume point. Durable behavior belongs in the maintained files under `ProjectGuide/Features/` and the relevant product/engineering owners. Git history stores previous states; stale package narratives and duplicate roadmaps are not live documentation.

## Permanent working rules

1. Record every material user request here before implementation or in the same change.
2. Classify it as earlier/blocking, current, later, or recovery-required.
3. Earlier regressions stop later feature work; preserve the resume point, repair, verify, then return.
4. Synchronize every affected design, architecture, QA, technical-decision, and performance document.
5. Maintained Git documentation reflects current truth only. Merge valid content before deleting a superseded document.
6. Do not claim Unity compilation, Play Mode, performance, or QA success unless it actually ran.
7. Repair from the actual local state; never replace current systems with older package copies.
8. Run repository hygiene on every handoff and before every commit.
9. When remote and local both contain valid unique progress, preserve both sides and merge; never reset one side over the other.

<!-- B&D DURABLE TASK CONTINUITY CONTRACT START -->
## Permanent task continuity and cross-session handoff rule

1. Every material task is summarized in this file with its reason, scope, current phase, implementation truth, verification truth, blockers, next action and exact resume point.
2. Every large, multi-step, cross-system or multi-session task also has a detailed active record under `ProjectGuide/Tasks/`.
3. The detailed task record contains the originating request, why the task exists, protected behavior, decomposition, dependencies, decisions, file/system ownership, work completed, evidence, failures, unverified areas and the exact Codex/chat handoff point.
4. The task record and this global status are updated immediately whenever requirements, status, implementation, QA, bugs, blockers, ordering, deferral or the resume point changes.
5. Every relevant canonical domain document is updated in the same change. Important truth may not live only in this file, only in a task record, only in chat, or only in a package README.
6. Bug changes also update `ProjectGuide/Status/BUGS.md`.
7. Ownership changes update `ProjectGuide/Engineering/ARCHITECTURE.md`; durable choices update `ProjectGuide/Engineering/TECHNICAL_DECISIONS.md`; QA changes update `ProjectGuide/QA/QA_CHECKLIST.md`; performance policy changes update `ProjectGuide/Engineering/PERFORMANCE.md`; maintained-document changes update `ProjectGuide/INDEX.md`.
8. No task may be described as complete or verified without the evidence required by its acceptance gate.
9. Before handoff to Codex, another chat or another contributor, the repository must explain why the task exists, what was approved, what was done, what was verified, what remains open and exactly where to continue.
10. Before every commit, perform a documentation relevance sweep: merge still-valid content into canonical owners, remove completed/superseded/duplicate/temporary documents, update references and `ProjectGuide/INDEX.md`, and block the commit if ownership is unclear or valid truth would be lost.

Canonical contract: `ProjectGuide/Rules/TASK_CONTINUITY.md`.
<!-- B&D DURABLE TASK CONTINUITY CONTRACT END -->

<!-- B&D MODERN 3D HANDHELD CURRENT REQUIREMENT START -->
# Current approved UI redesign — C11.UI.MODERN_HANDHELD_3D.V1

## Classification and priority

- Classification: `CURRENT / USER-PRIORITIZED BEFORE PREVIOUS REPAIR QUEUE`.
- This does not mark prior Runtime/QA defects resolved.
- Their exact resume point remains preserved in `MASTER_ACTIVE_WORK_SEQUENCE_V1.md`.

## Required result

The Main and Escape/Pause menus must be presented through one original upright 3D handheld device:

- portrait/classic standing handheld silhouette;
- screen in the upper half;
- controls in the lower half;
- continuous blue-left to orange-right shell gradient;
- real modeled thickness and bevels;
- real separate D-pad, A/B/X/Y, SELECT and EXIT controls;
- tactile press/release interaction;
- display recessed behind separate clear glass/transparent plastic with visible thickness and controlled reflections;
- professional dark-fantasy screen design;
- same physical device and interaction system across Main, Settings, Progression, Pause, Abandon and Loading where applicable.

## Exact interaction contract

- mouse hover/click is supported;
- D-pad/arrows navigate;
- Main Menu X starts New Game;
- Main Menu A opens Progression;
- Main Menu B opens Settings;
- Main Menu Y opens Credits;
- B goes back on every non-main page;
- physical center `SELECT` activates the highlighted option;
- physical center `EXIT` opens the correct quit/abandon confirmation;
- all paths call the same semantic menu actions and cannot double-activate.

## Text and character-art correction

- Use `Progression`, on one line.
- Do not show `Meta Progression` in the redesigned UI.
- Every image containing the Boy requires an equivalent Girl image in the same change and asset version.
- Runtime uses the variant matching the active playable character.

## Canonical specification and references

- `ProjectGuide/Production/ModernHandheld/MODERN_HANDHELD_3D_SPEC.md`
- `ProjectGuide/References/Visual/ModernHandheld3D/`

## Verification truth

This package captures design/spec/reference truth only. It does not prove Runtime implementation, Unity compilation, TEST EVERYTHING, tactile interaction, performance or final visual acceptance.
<!-- B&D MODERN 3D HANDHELD CURRENT REQUIREMENT END -->







# Previous implemented visual baseline — C11.UI.V23R19Q (superseded as final target)

## Reason

The intro and menu already contain the approved identity, but their composition and rendering still read as prototype IMGUI. The goal is not to replace the design. The goal is to preserve it and make it feel authored, cohesive and production-ready.

The requested memory is a Game Boy-like handheld as people remember it: chunky, tactile, friendly and screen-centered, but cleaner, richer and more modern than the literal old hardware.

## Preserved

- BBH sequence and timing;
- growing circle and light sweep;
- dreamy backdrop;
- title/subtitle;
- Start, Settings and Quit;
- Pause/Resume;
- Abandon confirmation;
- Loading;
- all settings;
- true-victory awakened state;
- current run/input behavior.

## V23R19Q implementation

- professional cached boot surface;
- rounded original handheld shell;
- inset glass screen and subtle scanlines;
- refined D-pad, A/B, Start/Select, speaker and status light;
- limited readable screen palette;
- modern list-action hover/focus treatment;
- 180 ms page slide/fade;
- restrained device focus halo;
- no per-frame Texture2D or Material creation;
- no scene/prefab replacement;
- no gameplay behavior change.

## V23R19Q acceptance gate

1. Unity compiles.
2. TEST EVERYTHING: 0 blockers, 0 warnings, 0 info.
3. BBH content/order/timing/circle/sweep remain.
4. The intro looks more professional without becoming busy.
5. Main, Settings, Pause, Abandon and Loading all appear inside one coherent handheld screen.
6. Every existing option still works.
7. Hover and page transitions are smooth.
8. The shell evokes remembered handheld language without copying a commercial model.
9. The device and text remain readable at desktop and landscape-mobile-like resolutions.
10. No recurring GC spike or visible frame hitch is introduced by the new presentation.
11. V23R19O gameplay-focused verification remains unchanged.

# Active blocking work — C01.QA.V23R19P

## Why this repair exists

The V23R19O automated run compiled and executed, but three old string-based QA expectations no longer match the active project truth:

1. V23R19 traversal QA still requires `wallJumpHorizontalDuration = 0.48f`, although V23R19O intentionally raised the active minimum to `0.62f` and added steering/retained-speed policy.
2. V23R19G QA requires `BUG-V23R19G-001` to remain in the current open-bug table, although that visual bug was verified and removed under the repository's documentation relevance policy.
3. V23R19O QA requires the exact single-line text `BDRunPresentationCoordinator.EnsureMountedIntroRiderVisible(rider)`, while the valid compiled call is formatted across several lines.

These are QA semantic defects. Restoring the old duration, reopening a verified bug, or reformatting Runtime only to satisfy brittle text matching would make the repository less accurate.

## V23R19P implementation

- V23R19 traversal QA now requires the active 0.62-second Wall Jump duration plus steering and retained-speed fields.
- Its scene contract keeps only stable scene-owned traversal values; active Wall Jump minimum migration remains Runtime-owned.
- V23R19G bug-ledger QA requires current open defects (`BUG-V23R19G-005`, `BUG-V23R19H-001`, `BUG-V23R19O-001`) rather than a verified/removed bug.
- V23R19O mounted-visibility QA requires the stable qualified method symbol instead of whitespace-sensitive argument formatting.
- A V23R19P scanner prevents those three semantic regressions.
- No Runtime behavior is changed.

## Caterpillar gambling NPC capture

The new canonical future specification records:

- only rooms selected to host the NPC are eligible;
- it does not appear in every room;
- in an eligible room it appears through animation only after the room is clear;
- enemy presence triggers an animated disappearance;
- an active gambling session has explicit temporary safety preventing enemy approach and attacks;
- each Caterpillar offers one configurable game;
- the current game candidates are not final;
- bankroll is finite;
- passive refill uses time and room progression and stops at a normal threshold;
- player-derived winnings can exceed that threshold and are never clipped;
- all missing formulas, odds, rules, frequencies, visuals and values remain open.

The Caterpillar remains `REQUIRED / FUTURE / NOT IMPLEMENTED`.

## V23R19P acceptance gate

1. Unity compiles without errors.
2. TEST EVERYTHING reports 0 blockers, 0 warnings and 0 info.
3. No scanner requires the 0.48-second Wall Jump duration.
4. No scanner requires verified `BUG-V23R19G-001` to return to the open ledger.
5. Mounted-rider QA accepts the active multi-line method call.
6. Caterpillar requirements are present in the canonical economy document, global status, documentation index, architecture/decision context and active task record.
7. V23R19O focused Play Mode remains open and unchanged.

# Active blocking work — C03/C04/C11.RUNTIME.V23R19O

## Why this task exists

The latest automated run is clean and the corrected airborne attack plus enemy-death presentation were accepted in focused Play Mode. Testing then exposed one serious retained regression and two requested presentation/traversal refinements.

1. After confirmed abandon and Start Game, the horse performs the mounted opening while the Boy is not rendered; the Boy becomes visible only when the cinematic finishes.
2. The red target silhouette includes the broad non-damageable circle/ring around an enemy instead of following only the enemy model that can actually receive damage.
3. Wall Jump must be higher and farther, allow directional steering during the push-off arc, rotate the player toward the changing direction, and make the gameplay camera yaw follow that direction.

The mounted-intro issue is a blocker because it breaks the first visible narrative/gameplay frame after a common run-flow action.

## V23R19O implementation

- Capture the active-scene rider's eligible visible Mesh/Skinned renderer baseline on scene load.
- Before the cinematic cover opens and on every mounted-intro rider rebind, force those renderer branches active/enabled and clear `forceRenderingOff`.
- Keep `SkinnedMeshRenderer.updateWhenOffscreen` enabled during the teleported cinematic so stale bounds cannot cull the Boy, then restore the original policy after control handoff.
- Preserve the existing authoritative horse/rider Transform binding.
- Resolve the enemy's actual enabled non-trigger colliders owned by the same `BDHealth`.
- Build outline shells only for non-auxiliary renderers intersecting that damageable envelope.
- Never outline thin ground rings/circles, range/selection indicators, telegraphs or target-outline shells.
- Apply a 0.62 alpha multiplier to auxiliary enemy rings and reduce temporary enemy attack-circle alpha modestly.
- Raise the Wall Jump minimum height/speed/duration, retain more launch speed through the arc, and rotate launch velocity toward held movement input at a bounded rate.
- Update player facing and `LastLookDirection` from the steered wall-jump trajectory.
- Increase camera yaw response only while `IsWallJumping`, preserving normal camera behavior outside the move.

## Protected behavior

- Grounded jump values and behavior remain unchanged.
- Wall Jump still requires a valid mostly vertical enabled non-trigger solid surface and keeps its cooldown.
- Player, horse, enemies, hazards, map generation and combat damage are not redesigned.
- Target selection range, blocker truth and one-target-only ownership remain unchanged.
- The auxiliary ring remains visible; it is only made subtler.
- No scene or prefab replacement is included.

## V23R19O acceptance gate

1. Unity compiles without errors.
2. TEST EVERYTHING reports 0 blockers, 0 warnings and 0 info.
3. Abandon -> clean main menu -> Start Game:
   - the exact active-scene Boy is already on the horse before the cover reveals the cinematic;
   - he remains visible from the first visible frame through the entire entrance and control release;
   - no late pop-in occurs.
4. Target highlighting:
   - the red outline follows only the vulnerable enemy model;
   - no outline appears on the non-damageable broad ring/circle;
   - the ring remains visible but is noticeably more transparent;
   - one target, range and wall blocking remain correct.
5. Wall Jump:
   - reaches visibly higher and farther than the previous accepted version;
   - begins away from the contacted valid vertical surface;
   - held directional input can curve/change the travel direction during the launch;
   - the player model turns with the trajectory;
   - the camera yaw follows the changed direction smoothly;
   - normal grounded jump remains unchanged.
6. Update `ProjectGuide/Status/CURRENT.md`, the open-bug ledger, task record and canonical domain documents with actual results.
7. Resume Phase 1 of `C01.ARCH.AUDIT.V1` only after the focused gates pass.

# Active blocking work — C01.QA.V23R19N

## Why this repair exists

The V23R19M Runtime correctly replaced the superseded local-X conversion with the approved local-Z conversion. The next Unity automated run compiled and executed, but three older regression scanners still required the removed marker and rotation expression. Each scanner emitted two findings, producing six blockers for one semantic drift.

The repair exists to make automated QA validate the active implementation rather than force restoration of the known-wrong rotation.

## Evidence

`TEST EVERYTHING` at `2026-06-08T19:10:30.7997940Z` reported:

- 2 findings from V23R19D;
- 2 findings from V23R19E;
- 2 findings from V23R19G;
- every finding requested either `BD CORRECT LOCAL-X AIRBORNE ROTATION V23R19G` or `Quaternion.AngleAxis(-90f, Vector3.right)`;
- 0 compiler errors, 0 warnings and 0 info.

## V23R19N implementation

- `BDV23R19DFocusedRegressionQA` now requires the V23R19M local-Z marker and `Quaternion.AngleAxis(90f, Vector3.forward)`.
- `BDV23R19EDeathGuardianIntroQA` now requires the same active contract.
- `BDV23R19GFocusedRegressionQA` now requires the same active contract.
- No Runtime, animation, scene, prefab, balance or content file changes.
- The V23R19M scanner remains the newest strict regression owner.

## V23R19N acceptance gate

1. Unity compiles without errors.
2. TEST EVERYTHING reports 0 blockers, 0 warnings and 0 info.
3. No legacy scanner requests the local-X marker or `-90°` local-X expression.
4. Automated PASS is recorded in `ProjectGuide/Status/CURRENT.md`, the bug ledger and the active audit interruption.
5. V23R19M focused Play Mode still verifies grounded/airborne Light and Heavy plus small/large enemy death.

# Active blocking work — C03/C11.RUNTIME.V23R19M

## Why this repair exists

Focused Play Mode showed that the airborne attack still did not match the user's simple geometric requirement: the normal attack line is horizontal/parallel to the floor, while the jumping attack must turn that same long line upright so it is top-to-bottom/perpendicular to the floor. Code inspection proved the previous repair rotated around local X, which stood up the plane but left the mesh's long local-X axis horizontal.

The user also confirmed that small regular-enemy death does not look good. The shared procedural path compresses enemy Y scale to 34%, producing a flat/rubber collapse instead of a readable death fall.

The V23R19L TEST EVERYTHING run at `2026-06-08T18:53:41.7256860Z` additionally reported two documentation-QA blockers because historical V23R19J/V23R19K scanners required resolved bug rows to remain in the current open ledger.

## V23R19M implementation

- Airborne Light/Heavy rotate the existing grounded mesh exactly 90 degrees around local Z.
- The long left-to-right local-X line becomes world up/down; local Z continues to face forward.
- Grounded attacks are unchanged.
- Small regular enemies use a short recoil, intact-body fall and subtle settle with only minimal scale compression.
- Player, large, Elite, guardian, mini-boss and boss death paths remain unchanged.
- Historical V23R19J/V23R19K QA no longer forces obsolete bug/status history to remain in current documents.
- A V23R19M regression scanner is integrated into TEST EVERYTHING.

## V23R19M acceptance gate

1. Unity compiles without errors.
2. TEST EVERYTHING reports 0 blockers, 0 warnings and 0 info.
3. Grounded Light/Heavy remain horizontal and parallel to the floor.
4. Airborne Light/Heavy long axis is top-to-bottom and perpendicular to the floor, directly in front of the player.
5. No horizontal duplicate appears while airborne.
6. At least two small regular enemy archetypes recoil and fall as intact bodies without flattening before loot/despawn.
7. One large/Elite enemy confirms its existing death path was not changed.
8. Update `ProjectGuide/Status/CURRENT.md`, this task interruption, canonical design docs, QA, technical decisions and open bugs with real results.
9. After verification, resume `C01.ARCH.AUDIT.V1` Phase 1 exactly where it was paused.

# Active blocking work — C01/C03.RUNTIME.V23R19K

## V23R19K explicit airborne visual branch and remaining QA realignment — IMPLEMENTED IN PACKAGE / UNITY RERUN REQUIRED

- Consume the `airbornePresentation` identity returned by `BDPlayerMeleeEnhancer` at the real committed melee hit.
- Spawn exactly one selected attack visual: `BDMeleeSlashArcVisual.SpawnVertical` while airborne, otherwise the normal grounded arc.
- Do not restore the superseded timing-based `combat.SuppressNextStandardMeleeVisual` call in the enhancer.
- Realign V23R11, V23R19 and V23R19J scanners to the explicit branch.
- Validate the future Girl hook wording semantically and keep the current Boy mounted hook disabled.
- Record V23R19I compile repair in project history rather than forcing a resolved bug into the open table.
- Capture `OPENING_DIALOGUE_WORDLESS_CHARACTER_VOICE_HE_V1.md` as required future work; no dialogue Runtime, UI or audio is implemented by this package.

## V23R19K acceptance gate

1. Installer and validator pass twice; the second installation writes zero files.
2. Changed-file fail-safe writes nothing.
3. Repository hygiene and `git diff --check` pass.
4. Unity compiles without C# errors.
5. TEST EVERYTHING reports 0 blockers, 0 warnings and 0 info.
6. A committed grounded Light/Heavy attack creates only its grounded arc.
7. A committed airborne Light/Heavy attack creates only the selected vertical arc and no horizontal duplicate.
8. The Boy remains unable to use sword melee or hook while mounted; the future Girl permission remains documentation only.
9. The opening `I’m bored.` dialogue remains `REQUIRED / LATER / NOT IMPLEMENTED`.
10. Continue the retained V23R19G/V23R19H focused Play Mode gates after automated PASS.

# Active blocking work — C01.DOCUMENTATION-QA.V23R19J

## V23R19J semantic QA realignment and corrected Girl/Father specification — PARTIAL / SUPERSEDED BY V23R19K

- Record V23R19I as a confirmed compile repair because Unity executed TEST EVERYTHING after installation.
- Update mounted-intro order QA to use `RestoreMountedIntroControls` after the full-stop hold and before `inputLocked = false`.
- Validate committed airborne damage/timing in `BDPlayerCombat` and actual airborne visual ownership in `BDPlayerMeleeEnhancer`.
- Validate mounted target origin through the active `TargetHighlightOrigin` implementation instead of an obsolete marker comment.
- Validate Battery guardians through `ConfigureEliteGuardian()` plus forced-movement-disabled policy without inserting a new serialized enum value.
- Restore the valid V23R19B hit-committed hook-pull design contract while preserving the corrected boy/girl mounted-hook rules.
- Validate the current V23R19G renderer-branch death owner and the corrected V23R19H bug ID.
- Replace the canonical Girl/Father/meta document with the latest approved correction: Girl is a meta unlock, Father temporarily summons invulnerable Mother for exactly 30 seconds in phases 1–3, phase 4 uses full Mother plus enemy-wave summons, and the temporary meta-points screen remains required future work.

## V23R19J acceptance gate

1. Installer and validator pass twice; the second install writes zero files.
2. Changed-file fail-safe performs zero writes.
3. Repository hygiene and `git diff --check` pass.
4. Unity compiles without restoring the V23R19I API errors.
5. TEST EVERYTHING reports 0 blockers, 0 warnings and 0 info.
6. The 15 reported blocker codes/tokens are absent without adding dead Runtime code or reverting active ownership.
7. Boy mounted hook remains disabled; future Girl mounted hook remains required and not implemented.
8. No Runtime, scene, prefab, balance or serialization behavior changes in this package.
9. Continue the retained V23R19G/V23R19H focused Play Mode checks after automated PASS.

# Active blocking work — C01/C03/C05.RUNTIME.V23R19I

## V23R19I forced-movement API compile compatibility — IMPLEMENTED IN PACKAGE / UNITY RERUN REQUIRED

- Restore `BDCombatantProfile.ReceivesForcedMovement` for `BDKnockbackReceiver`.
- Restore `BDCombatantProfile.CanReceiveForcedMovement(BDHealth)` for `BDPlayerGrapplingHook`.
- Preserve existing serialized data and `Regular` / `MiniBoss` / `Boss` semantics.
- Preserve Elite/Battery-guardian forced-movement immunity through their existing explicit profile configuration.
- Preserve the corrected character rule: the boy cannot use the hook while mounted; the future Girl may.
- Capture `ROPE_CLIMBING_AND_QUICKSAND_SWAMP_HE_V1.md` as required future work; no traversal or swamp Runtime is implemented here.

## V23R19I acceptance gate

1. Installer and validator pass twice; second install writes zero files.
2. Changed-file fail-safe performs zero writes.
3. Repository hygiene and `git diff --check` pass.
4. Unity compiles with neither CS1061 nor CS0117 for the forced-movement API.
5. TEST EVERYTHING reports 0 blockers and 0 warnings.
6. Regular small enemies remain pullable/knockback-capable; Battery guardians, large enemies and bosses preserve their intended immunity.
7. The boy-mounted-hook correction remains active.
8. Resume the retained V23R19G/V23R19H Play Mode verification.

# Active blocking work — C01/C03/C04.RUNTIME.V23R19H

## V23R19H character-specific mounted-hook correction — IMPLEMENTED IN PACKAGE / UNITY RERUN REQUIRED

- Correct the misinterpreted requirement: the **boy cannot use the hook while mounted**.
- While the boy is riding, Light/Heavy sword input and grappling-hook input are blocked without consuming the hook cooldown.
- Preserve the boy's normal on-foot hook behavior and the existing mounted ranged attack behavior.
- Record the future Girl character capability: the girl may use the hook while mounted when she is implemented, through character-specific data/capability rather than a global combat switch.
- Remove obsolete V23R19G QA/text anchors that required mounted hook use for the boy.
- Preserve all five focused V23R19G visual/run-flow repairs and the canonical open-bug tracker.

## V23R19H acceptance gate

1. Installer and validator pass from either the V23R19F base or an already-installed V23R19G state, and a second install writes zero files.
2. Repository hygiene and `git diff --check` pass.
3. Unity compiles and TEST EVERYTHING reports 0 blockers and 0 warnings.
4. On foot, the boy's short/long Heavy behavior remains unchanged and the hook still launches on a valid hold.
5. While mounted as the boy, Light/Heavy input launches neither sword melee nor hook and consumes no hook cooldown.
6. Mounted ranged shooting remains functional.
7. The future Girl specification explicitly requires mounted hook use through character-specific capability data.
8. The five retained V23R19G regressions remain open until focused Play Mode verifies them.

# Active blocking work — C01/C03/C04/C05/C11.RUNTIME.V23R19G

## V23R19G airborne, death, abandon, mounted replay, and bug-ledger repair — IMPLEMENTED IN PACKAGE / UNITY RERUN REQUIRED

- Correct the airborne slash from the wrong positive local-X rotation to the selected grounded attack rotated `-90°` around local X, with the same shape/scale language and downward motion in front of the player.
- Start lethal player/enemy death presentation synchronously at the health owner and animate actual top-level renderer branches, so the spherical player prototype, large enemies and Battery guardians all have visible death motion.
- Keep the player death view unobscured and wait for the complete pose plus readable hold before opening the menu.
- Confirmed abandon reloads the current scene into a clean main-menu state instead of drawing the menu over the abandoned run.
- Reassert the exact current rider, mounted state and mount-point pose throughout the entrance, and never restore the player's walking controller while the horse remains mounted.
- The original package temporarily enabled mounted hook holds for the boy; this requirement was corrected and superseded by V23R19H.
- Add and permanently maintain `ProjectGuide/Status/BUGS.md` on every bug discovery/status/repair/verification/reopen/reclassification.

## V23R19G acceptance gate

1. Package installer and validator pass twice and are idempotent; changed-file fail-safe writes nothing.
2. Repository hygiene and `git diff --check` pass.
3. Unity compiles and TEST EVERYTHING reports 0 blockers and 0 warnings.
4. Air Light and Heavy use the selected grounded arc rotated `-90°` around local X, directly in front and toward the floor, with no horizontal duplicate.
5. Player lethal damage shows an unmistakable death motion and hold before the menu.
6. Large enemies and Battery guardians stop gameplay, show death motion, then loot/despawn.
7. Confirmed abandon reloads to a clean main menu rather than a popup over gameplay.
8. Abandon -> Start Game keeps the exact player attached to the horse from the first visible intro frame through control release.
9. Superseded by V23R19H: the boy must not launch sword melee or hook while mounted; on-foot hook behavior remains correct.
10. Every bug status is synchronized in `OPEN_BUG_TRACKER.md` and `ProjectGuide/Status/CURRENT.md`.

# Active blocking work — C01.DOCUMENTATION-QA.V23R19F

## V23R19F semantic guardian/status QA compatibility — IMPLEMENTED IN PACKAGE / UNITY RERUN REQUIRED

- Replace the obsolete guardian scanner token `spawnRoom.ContainsWorldPosition(player.position, 0f)` with the active V23R19E semantic contract: `TryResolvePlayerRoomFallback`, `spawnRoom.ContainsWorldPosition(`, and `playerTransform.position`.
- Preserve strict same-room, room-interior, bounded-distance, and clear-path validation.
- Replace the obsolete V23R9 status phrase `V23R8 automated baseline` with the maintained current phrase `V23R8 automated QA passed`.
- Do not add dead Runtime code, duplicate local variables, or stale documentation text merely to satisfy scanners.
- Capture the uploaded gameplay/abilities/map/ambient-world/UI prompt in `ProjectGuide/Features/Runtime/GAMEPLAY_ABILITIES_MAP_AMBIENT_UI_EXPANSION_HE_V1.md` as required future work; no feature in that prompt is marked implemented by this package.

## V23R19F acceptance gate

1. Package installer and validator pass twice and remain idempotent.
2. Changed-file fail-safe performs zero writes.
3. Repository hygiene and `git diff --check` pass.
4. Unity compiles without new errors or warnings.
5. TEST EVERYTHING no longer reports `GUARDIAN_SAME_ROOM_SAFETY_MISSING`.
6. TEST EVERYTHING no longer reports `V23R9_PROJECT_STATUS_MISSING`.
7. V23R19E Runtime and scene behavior remain unchanged.
8. Continue V23R19E focused Play Mode verification after the automated gate passes.

# Active blocking work — C01/C03/C05/C06/C11/C12.RUNTIME.V23R19E

## V23R19E mounted replay, exact airborne rotation, death presentation, and Battery-guardian spawn — IMPLEMENTED IN PACKAGE / UNITY RERUN REQUIRED

- Clear stale player targeting cache on scene load and resolve the current player through the horse's serialized rider/canonical marker before generic component fallback.
- Start the entrance through `BeginMountedRunIntro`, snap the rider before the first visible movement frame, keep the unparented rider attached inside every `MoveByExternalControl` frame while the horse controller is disabled, and complete the authoritative mounted state before gameplay control returns.
- Reuse the exact grounded Light/Heavy slash geometry and rotate it exactly 90 degrees around local X for airborne presentation; body motion remains secondary and minimal.
- Keep gameplay visible during player death, play a dedicated player death animation, then open the Game Boy menu after the pose is readable.
- Play enemy death animation before regular-enemy loot and destruction; disable enemy gameplay/collision while the death pose runs.
- Build Battery guardians inactive, but finish delayed activation from a separate scene runner so collecting/destroying the Battery cannot cancel spawn.
- Preserve same-room safety and add a bounded line-of-sight player-room fallback for hideout pickup points just outside exact minimap-room bounds.
- Synchronize merchant hostile/alive/defeated behavior, partial empty-slot refresh, fixed-cost full reroll, exclusive weapon reward, and required open-design meta progression into maintained documents only; those future systems remain not implemented.

## V23R19E acceptance gate

1. Package installer and validator pass twice; changed-file fail-safe performs zero writes.
2. Repository hygiene and `git diff --check` pass.
3. Unity compiles and TEST EVERYTHING reports 0 blockers and 0 warnings.
4. Confirmed abandon -> Start Game shows the exact player attached to the horse from the first visible entrance frame through control release.
5. Air Light and Air Heavy look exactly like their grounded selected slash identity rotated 90 degrees, remain in front of the player, and do not spawn a second horizontal slash.
6. Player lethal damage shows the player death pose before the menu appears; input and combat cannot continue during the pose.
7. Sword, Patrol, Charger, Trap, Ranged, Jumper, and Exit-blocker enemies show death motion before loot/despawn and cannot attack/move during it.
8. Battery A and Battery B each trigger visible guardians even if the collectible is collected during reveal; guardians chase, attack, take damage, and retain Elite forced-movement immunity.
9. No guardian can trigger through an adjacent wall; fallback requires the player's containing room, bounded distance, and clear path.
10. Retained quicksand, hook, mounted hazards, camera, Parry, bomb, menu, and Console gates remain passing.

# Active blocking work — C01/C03/C10/C11.RUNTIME.V23R19D

## V23R19D focused Play Mode regression repair — IMPLEMENTED IN PACKAGE / UNITY RERUN REQUIRED

- Preserve the confirmed fix: jumping with movement input inside quicksand does not respawn a living player.
- Push the quicksand depth multiplier directly into `BDPlayerController` and apply it exactly once.
- Present airborne Light/Heavy directly in front of the player as a vertical overhead-to-floor chop with no diagonal body roll.
- Treat controlled-jump enemy contact/damage as valid airborne movement, not combat floor loss; do not teleport to an old safe point.
- Require confirmation before abandoning a live run.
- Resolve and mount the exact active loaded-scene player for abandon -> New Game mounted-intro replay.

## V23R19D acceptance gate

1. Installer/validator pass twice; fail-safe performs zero writes on a changed supported file.
2. Repository hygiene and `git diff --check` pass.
3. Unity compiles and TEST EVERYTHING reports 0 blockers and 0 warnings.
4. Quicksand entry is visibly slower, slowdown increases with sink depth, and leaving restores normal speed.
5. Air Light/Heavy appear in front, square to facing, and strike downward to the floor without a grounded slash or diagonal body roll.
6. Jump onto an attacking enemy repeatedly; the player may take damage but never teleports backward to a safe point.
7. Pause -> Abandon opens confirmation; Cancel/Escape preserves the run; Yes returns to menu.
8. After confirmed abandon, Start Game shows both horse and player entering together for the full intro.
9. Retained hook, Battery guardian, mounted hazard, camera, Parry, bomb, enemy-grounding, and Console gates remain passing.

# Active blocking work — C01.DOCUMENTATION-QA.V23R19C

## V23R19C semantic QA compatibility — IMPLEMENTED IN PACKAGE / UNITY RERUN REQUIRED

- Replace the obsolete direct `BDCombatantRank.Regular` hook token with the active centralized contract: `BDEnemyHazardNavigation.IsSmallRegularEnemy` plus `BDCombatantProfile.CanReceiveForcedMovement`.
- Keep strict validation that a real hook impact applies damage and uses the safe pull-stop contract.
- Validate committed airborne identity/body animation in `BDPlayerMeleeEnhancer`.
- Validate the actual vertical slash spawn in its V23R19 owner, `BDPlayerCombat`, rather than requiring that presentation call in the enhancer.
- Runtime hook, airborne combat, scene, guardians, quicksand, traversal, assets, and balance remain unchanged.

## V23R19C acceptance gate

1. Package installer and validator pass twice and remain idempotent.
2. Repository hygiene and `git diff --check` pass.
3. Unity compiles without new errors or warnings.
4. TEST EVERYTHING no longer reports `C03_23A_GRAPPLING_RUNTIME_MISSING`.
5. TEST EVERYTHING no longer reports `V23R11_COMMITTED_AIRBORNE_MISSING`.
6. No runtime code is added merely to satisfy obsolete scanner text.
7. Continue all V23R19B focused Play Mode verification before marking gameplay complete.

# Active blocking work — C01/C03/C05/C06/C10.RUNTIME.V23R19B

## V23R19B scene-safe traversal, committed hook pull, and Battery-guardian repair — IMPLEMENTED / UNITY UNVERIFIED

- Preserve arbitrary unrelated local scene edits and patch only the approved movement serialization fields.
- A hook impact on a living canonical small regular enemy commits the pull; helper colliders or a child-health layout cannot downgrade it to damage-only.
- Oversized, elite, mini-boss, and boss targets remain fixed-damage-only for the hook.
- Construct collectible guardians inactive with the complete runtime stack and activate the root atomically at the final spawn position.
- Battery guardians acquire the player, move, attack, receive player damage, show normal feedback, and die normally.
- Battery guardians use the Elite category and never receive hook pull, knockback, mounted-impact displacement, or small-enemy forced hazard entry.

## V23R19B acceptance gate

1. Installer accepts the supported post-V23R18A and post-V23R18B code/document states without requiring an exact whole-scene hash.
2. Unrelated scene text and objects remain byte-identical except for the approved movement fields.
3. Quicksand nonlethal jumping never invokes generic respawn; jump, dodge, and Wall Jump reach match V23R19.
4. Airborne Light and Heavy use only their explicit airborne presentation.
5. Hook several Sword, Charger, Patrol, Trap, and Jumper small regular enemies; every real hit pulls the living target into safe sword range.
6. Hook a Battery guardian; it receives exactly 2 damage and does not move.
7. Battery guardians visibly chase/attack, expose a valid hit collider, lose HP to Light/Heavy/Spin/Airborne/Ranged attacks, and die normally.
8. Player melee, explosions, horse impacts, and other knockback paths cannot displace Battery guardians.
9. Unity compiles and TEST EVERYTHING passes with zero blockers and warnings.
10. Focused Play Mode results are recorded before marking V23R19B complete.

# Active blocking work — C01/C03/C10.RUNTIME.V23R19

## V23R19.1 Quicksand nonlethal-jump recovery — IMPLEMENTED / UNITY VERIFICATION REQUIRED

- Generic ground-exit and combat-grounding recovery are disabled while quicksand owns active/residual sink state.
- Jumping with movement input cannot teleport a living player to a safe point.
- Only half-body quicksand failure may apply fall damage and respawn.

## V23R19.2 Traversal reach and universal solid wall jump — IMPLEMENTED / UNITY VERIFICATION REQUIRED

- Normal jump held-movement travel is increased by 10% for the controlled jump window.
- Dodge distance increases from 3.05 to 3.35.
- Wall-jump horizontal speed/duration increase from 7.1/0.42 to 8.2/0.48.
- Wall jump accepts any sensible enabled non-trigger solid vertical surface, including enemies and the horse, through contact plus a bounded probe.

## V23R19.3 Dedicated airborne attack identity — IMPLEMENTED / UNITY VISUAL VERIFICATION REQUIRED

- The committed attack explicitly selects grounded or airborne presentation.
- Airborne Light/Heavy use their dedicated body animation and vertical slash only.
- The regular horizontal slash is never invoked for that same airborne attack.

## V23R19 acceptance gate

1. Unity compiles without new errors or warnings and TEST EVERYTHING passes.
2. Jumping and steering inside quicksand never teleports before failure depth.
3. Quicksand damage, extraction, slowdown and true failure respawn remain correct.
4. Normal jump and dodge are slightly farther without becoming dash-like or bypassing hazard guards.
5. Wall jump works from structural walls, hard props, a small enemy and the horse, but not floors/triggers.
6. Air Light and Air Heavy show only dedicated vertical/body presentation; grounded attacks remain horizontal.
7. Retained V23R18B mounted-hole/lava ordering and all prior focused gates remain intact.

# Active blocking work — C01/C04/C10/C12.RUNTIME.V23R18B

## V23R18B.1 Mounted hazard dismount-before-damage ordering — IMPLEMENTED / UNITY VERIFICATION REQUIRED

- `BDHorseHazardSafety` captures the rider recovery owner while mounted, then calls `ForceDismountAfterHazardRecovery` before applying horse hazard damage.
- Horse `DamageBurstTriggered` and `Fainted` callbacks therefore cannot launch the ordinary buck/dismount path while hazard recovery still considers the pair mounted.
- Horse relocation occurs only after dismount and damage resolution.
- Mounted hole rider damage/recovery and mounted lava zero-damage rider recovery begin only after the pair is already unmounted.

## V23R18B.2 Exact production-animation token — IMPLEMENTED / UNITY VERIFICATION REQUIRED

- The exact lower-case token `temporary procedural animation is not final release animation` exists in root art direction, the Unity-side visual mirror, and the production-animation requirements.
- The visible human-facing sentence remains unchanged; the exact scanner token is retained as a non-rendered documentation contract.

## V23R18B.3 Focused regression QA — IMPLEMENTED / UNITY VERIFICATION REQUIRED

- `BDV23R18BMountedHoleAnimationTokenQA` validates the real API owner in `BDHorseController`, exact token coverage, and source ordering from dismount through damage, horse relocation and rider recovery.
- The scanner uses the active `result.findings` / `BDOneClickQAFinding` API and is integrated into the single TEST EVERYTHING entry point.

## V23R18B acceptance gate

1. Installer and validator pass twice; fail-safe writes nothing on a mismatched base.
2. Repository hygiene and `git diff --check` pass after package cleanup.
3. Unity compiles without new project errors or warnings.
4. TEST EVERYTHING passes without the V23R18A exact-token blocker and without a V23R18B ordering blocker.
5. Mounted hole force-dismounts before horse damage callbacks and before horse/rider relocation.
6. Mounted hole still applies configured damage to horse and rider; both finish unmounted at separate legal safe positions.
7. Mounted lava still damages only the horse; the rider follows the zero-damage recovery arc and both finish unmounted.
8. A horse faint or damage-burst threshold reached by hazard damage does not start the ordinary buck/dismount presentation.
9. V23R18A quicksand semantic QA and all retained V23R17 movement/hazard/wall-jump behavior remain intact.
10. Record real Unity and focused Play Mode results before marking V23R18B PASS.

# Active blocking work — C01/C04/C10/C12.RUNTIME.V23R18A

## V23R18A.1 Production-animation completeness contract — DOCUMENTED / IMPLEMENTATION PROGRAM OPEN

- Every action requiring visible motion must receive a final production-quality animation or an explicitly approved production procedural solution.
- Coverage includes player, horse, enemies, mini-bosses, bosses, interactions, hazards, destructibles, UI motion and cinematics.
- Temporary prototype transforms are recorded as animation debt and are not release-complete.
- The detailed source is `ProjectGuide/Features/Animation/PRODUCTION_ANIMATION_REQUIREMENTS_V1.md`.

## V23R18A.2 Horse hole/lava damage ownership — IMPLEMENTED / UNITY VERIFICATION REQUIRED

- Unmounted hole/chasm damages the horse and returns it to its latest safe point.
- Mounted hole/chasm damages horse and rider.
- Unmounted lava damages the horse and returns it to its latest safe point.
- Mounted lava damages the horse instead of the rider.
- Mounted recovery force-dismounts before actor relocation.
- The rider receives zero lava damage and follows a safe recovery arc to a separated point beside the recovered horse.

## V23R18A.3 V23R13 quicksand semantic-QA compatibility — IMPLEMENTED / UNITY RERUN REQUIRED

- Replace obsolete `TriggerFullSink` with the active player/horse/enemy failure methods.
- Replace the V23R13 volume marker requirement with the active V23R17 marker.
- Do not restore obsolete methods or comments merely to satisfy static scanning.

## V23R18A acceptance gate

1. Installer and validator pass twice; repository hygiene and `git diff --check` pass.
2. Unity compiles without new project errors or warnings.
3. TEST EVERYTHING passes without the two stale V23R13 quicksand blockers.
4. Unmounted horse hole contact applies fall damage and safe recovery.
5. Mounted horse hole contact applies damage to horse and rider and separates them safely.
6. Unmounted horse lava contact applies lava damage and safe recovery.
7. Mounted horse lava contact damages only the horse; the rider loses no HP and both recover unmounted.
8. Existing quicksand, enemy hazard, mounted impact, wall jump, intro and airborne-attack behavior remains intact.
9. Production animation requirements are discoverable from root art direction and the documentation index.
10. Record real results before marking V23R18A PASS.

# Active blocking work — C01/C03/C04/C10.RUNTIME.V23R17

## V23R17.1 Exact quicksand behavior — IMPLEMENTED / UNITY VERIFICATION REQUIRED

- Grounded standing and walking sink continuously and slow progressively with depth.
- Every grounded contact second deals exactly 2 damage.
- Committed jumps extract a fixed amount; deeper sink requires more jumps.
- Dodge pauses additional sink and grants no extraction.
- Half-body depth triggers fall damage and safe respawn.

## V23R17.2 Enemy hazard intent and forced entry — IMPLEMENTED / UNITY VERIFICATION REQUIRED

- Enemy brain motion cannot intentionally enter registered hazards.
- Jumping enemies reject hazard landing candidates.
- External knockback and mounted impacts may push small regular enemies into hazards.
- Hole/lava kill eligible small enemies immediately; quicksand sinks and then kills them.

## V23R17.3 Mounted small-enemy impact — IMPLEMENTED / UNITY VERIFICATION REQUIRED

- A moving mounted horse deals 4-10 damage to small regular enemies according to speed and contact directness.
- Glancing contact is valid at the lower end; a fast square collision reaches 10.
- Knockback direction follows the actual strike and may deliver the enemy into a hazard.
- Large enemies, mini-bosses, and bosses are excluded.

## V23R17.4 Wall jump — IMPLEMENTED / UNITY VERIFICATION REQUIRED

- Jump pressed during recent airborne wall contact turns the player away and launches a medium upward arc.
- The launch is not a straight dash and has reduced temporary air control.

## V23R17.5 Carried-forward presentation repairs — IMPLEMENTED / UNITY VERIFICATION REQUIRED

- Mounted intro chooses a clear direction rather than fixed right.
- Air Light and Air Heavy use distinct body-animation timelines in addition to vertical slash presentation.

## V23R17 acceptance gate

1. Installer and validator pass twice; repository hygiene and `git diff --check` pass.
2. Unity compiles without new errors or warnings and TEST EVERYTHING passes.
3. Player quicksand matches the documented damage, jump, dodge, depth, slowdown, and recovery contract.
4. Enemies avoid hazards under brain motion; only valid external force pushes small regular enemies inside.
5. Hole/lava immediate death and quicksand sink-death work for eligible enemies.
6. Mounted grazing and direct impacts deal values inside 4-10 and apply correctly directed knockback once per cooldown.
7. Wall jump turns away from the wall and follows a medium arc.
8. Intro turn avoids walls/hazards and Air Light/Heavy are visibly distinct from ground attacks and each other.
9. Record real results before resuming the saved feature sequence.

# Active blocking work — C01/C12.QA.V23R15D-CANONICAL-ROOT-TOKEN

## V23R15D.1 Exact canonical-root phrase compatibility — IMPLEMENTED / UNITY RERUN REQUIRED

- Keep root `ProjectGuide/Product/ART_DIRECTION.md` as the sole canonical visual source.
- Keep the asset-side document as the synchronized Unity mirror.
- Use the exact, semantically correct declaration `Canonical root source: ProjectGuide/Product/ART_DIRECTION.md`.
- Do not modify Runtime, combat, hazards, scenes, target presentation, damage systems, or art policy.

## V23R15D acceptance gate

1. Installer and validator pass twice; package text contains no whitespace or merge-marker defects.
2. The Unity-side mirror contains both `Canonical root source` and `ProjectGuide/Product/ART_DIRECTION.md`.
3. Unity compiles and TEST EVERYTHING no longer reports `V23R9_ART_DIRECTION_MIRROR_MISSING`.
4. Record the real automated result, then continue V23R15 focused combat verification.

# Active blocking work — C01/C12.QA.V23R15C-ART-DIRECTION-MIRROR

## V23R15C.1 Canonical root filename in Unity mirror — IMPLEMENTED / UNITY RERUN REQUIRED

- Preserve the full approved art-direction content and reference board.
- Keep `ProjectGuide/Product/ART_DIRECTION.md` as the canonical root source.
- State the exact root filename inside the Unity-side mirror so humans and automated governance resolve the same authority.
- Do not copy or fork a second independent design policy.
- Do not modify Runtime, combat, hazards, target presentation, scenes, or assets.

## V23R15C acceptance gate

1. Installer and validator pass twice; repository hygiene and `git diff --check` pass.
2. The Unity-side mirror contains the literal canonical path `ProjectGuide/Product/ART_DIRECTION.md`.
3. `ProjectGuide/Product/ART_DIRECTION.md` remains the root authority and the asset-side document remains a synchronized mirror.
4. Unity compiles and TEST EVERYTHING no longer reports `V23R9_ART_DIRECTION_MIRROR_MISSING`.
5. Record the real automated result, then continue V23R15 focused combat verification.

# Active blocking work — C01/C03.QA.RUNTIME.V23R15B-AOE-CRITICAL-SEMANTIC-COMPATIBILITY

## V23R15B.1 Independent critical per AOE target — IMPLEMENTED / UNITY VERIFICATION REQUIRED

- Preserve one committed spin spectrum roll so the AOE remains one coherent sword attack.
- Roll the exact 6% critical chance independently for each unique `BDHealth` hit by the spin.
- Apply the exact 1.5 multiplier only to the target whose roll succeeds.
- Duplicate colliders of one enemy do not create extra rolls or damage.
- Light, heavy, airborne light, and airborne heavy retain one critical roll per committed attack.
- Projectiles and grappling hook remain fixed damage and cannot enter the sword-critical path.

## V23R15B.2 Semantic QA contract compatibility — IMPLEMENTED / UNITY RERUN REQUIRED

- Validate quicksand through its active class and behavior methods rather than a comment token.
- Validate the critical-aware damage-number dispatch through the active multiline call and parameters.
- Validate the art-direction mirror through its canonical-root declaration and path rather than a frozen sentence.
- Preserve strict QA coverage without adding fake compatibility tokens to Runtime.

## V23R15B acceptance gate

1. Installer and validator pass twice; repository hygiene and `git diff --check` pass.
2. Unity compiles with zero errors and TEST EVERYTHING no longer reports the three stale blockers.
3. Multi-target spin shares one pre-critical spectrum value.
4. Every unique enemy hit rolls critical independently; one spin can display a normal number on one enemy and a critical fuchsia number on another.
5. Statistical sampling per AOE target approaches 6% critical frequency.
6. Duplicate colliders do not create duplicate critical rolls or duplicate damage.
7. Non-AOE sword attacks retain one critical state per committed attack.
8. Projectiles and hook remain fixed and never use the critical color.
9. Record real Unity and Play Mode results before resuming the saved feature sequence.

# Active blocking work — C01.QA.V23R15A-RESULT-API-COMPATIBILITY

## V23R15A.1 Repair scanner integration with maintained QA API — IMPLEMENTED / UNITY RERUN REQUIRED

- Replace nonexistent `result.Blockers` access with `result.findings`.
- Replace nonexistent `BDOneClickQAIssue` with `BDOneClickQAFinding`.
- Add findings with `BDOneClickQASeverity.Blocker`, empty asset/object paths, and the existing code/message values.
- Apply the same maintained pattern to both V23R14 and V23R15 scanners.
- Do not modify Runtime combat, damage resolution, critical chance, damage-number presentation, scene data, or gameplay assets.

## V23R15A acceptance gate

1. Installer and validator pass twice; repository hygiene and `git diff --check` pass.
2. Neither scanner contains `result.Blockers` or `BDOneClickQAIssue`.
3. Both scanners add `BDOneClickQAFinding` instances to `result.findings` with blocker severity.
4. Unity recompiles without the four reported CS1061/CS0246 errors.
5. TEST EVERYTHING runs and reports the real automated result.
6. If automated QA passes, resume V23R15 sword-spectrum, critical-frequency, fixed-projectile, and fixed-hook focused verification.

# Active work — C01/C03/C11.RUNTIME.V23R15

## V23R15.1 Sword damage spectrum — IMPLEMENTED / UNITY VERIFICATION REQUIRED

- Light, heavy, airborne light/heavy, and spin roll once per committed attack inside a ±10% spectrum around configured sword damage.
- Every target hit by one spin shares the same pre-critical spectrum roll; each unique enemy rolls critical independently.
- Projectiles and grappling hook remain fixed-damage paths and do not use the spectrum resolver.

## V23R15.2 Sword critical attacks — IMPLEMENTED / UNITY VERIFICATION REQUIRED

- Eligible player sword attacks have exactly 6% critical chance.
- Critical damage is exactly 1.5 times the rolled pre-critical sword damage.
- Critical damage numbers use a dedicated fuchsia/magenta identity.
- Ranged, hook, bombs, hazards, horse, and enemy attacks cannot use this critical path.

## V23R15.3 Compilation truth — OPEN UNTIL UNITY RERUN

- The previous V23R14 report contained only the aggregate `UNITY_SCRIPT_COMPILATION_FAILED` blocker.
- V23R15 uses conservative Runtime APIs for the V23R14 presentation additions, but compilation is not marked passed without a new Unity result.
- If compilation remains blocked, record the exact red Console compiler lines before another code change.

## V23R15 acceptance gate

1. Installer and validator pass twice; repository hygiene and `git diff --check` pass.
2. Unity compiles with zero errors.
3. TEST EVERYTHING passes with zero blockers.
4. Repeated light/heavy/airborne/spin hits show a spectrum rather than one fixed number.
5. Sampling many eligible attacks approaches 6% critical frequency.
6. Critical damage is exactly 1.5x the same attack's pre-critical resolved value.
7. A multi-target spin shares one spectrum roll, while each unique enemy receives an independent critical roll.
8. Projectiles and hook remain fixed and never use the critical color.
9. Critical numbers use the dedicated fuchsia color and clean up correctly.
10. Record real results before resuming the saved feature sequence.

# Active work — C01/C03/C11.RUNTIME.V23R14

## V23R14.1 Animated damage numbers — IMPLEMENTED / UNITY VERIFICATION REQUIRED

- Successful player damage displays a coral-red animated world-space value.
- Successful enemy damage displays an amber-gold animated world-space value.
- Values pop, rise, settle, fade, stack under rapid hits, and use unscaled time.
- Zero/blocked/dodged/parried damage does not display a false value.
- `BDHealth` remains the damage authority; the number system is presentation only.

## V23R14.2 Prototype hazard-label wall visibility — IMPLEMENTED / UNITY VERIFICATION REQUIRED

- Hole, lava, quicksand, and future generated test labels hide behind walls and solid blockers.
- Labels are distance-gated and hidden outside active gameplay HUD state.
- Existing scene labels are upgraded at runtime; future generated labels receive the visibility component from the installer.
- Hazard Runtime behavior and obstacle visibility are unchanged.

## V23R14.3 Semantic QA compatibility — IMPLEMENTED / AUTOMATED RERUN REQUIRED

- V23R11 audio QA validates the active complete-coverage mirror instead of a removed phrase.
- V23R13 quicksand QA validates the current state-owner marker.
- V23R8 target QA validates the V23R13 silhouette outline rather than removed GUI corner-frame fields.

## V23R14 acceptance gate

1. Installer and validator pass twice; repository hygiene and `git diff --check` pass.
2. Unity compiles without new warnings or errors.
3. TEST EVERYTHING passes with zero blockers.
4. Player damage numbers are red/coral; enemy damage numbers are amber/gold.
5. Rapid hits stack legibly and every number cleans itself up.
6. Parry slow motion does not strand damage text.
7. Hole/lava/quicksand test labels hide behind walls and when distant.
8. Menus, pause, death, and intro show no combat numbers or prototype labels.
9. Existing V23R13 and retained regression gates remain passing.
10. Record real results before resuming the saved feature sequence.

# Active work — C01/C03/C10/C11/C12.RUNTIME.V23R13

## V23R13.1 Complete audio-event direction — COMPLETED AS DOCUMENTATION

- The audio source of truth now covers all gameplay, weapon, impact, movement, horse, hazard, UI, menu, intro, cinematic, ambience, enemy, boss, and future sound events as a minimum non-exclusive matrix.
- Menu music, Game Boy UI sounds, intro sound design, button feedback, breakables, shots, sword actions, jump/dodge, hole/lava/quicksand, riding, and horse vocalizations are explicitly included.
- Full authored assets, mixer routing, adaptive states, and mastering remain honestly ordered under C12.42.

## V23R13.2 Playable quicksand — IMPLEMENTED / UNITY VERIFICATION REQUIRED

- `BDHazardType.Quicksand` is implemented and installed as the third prototype hazard.
- Player and horse movement slow progressively while sinking.
- A sand surface ring follows the actor; prototype entry/sink/escape audio is provided.
- Escape before full sink clears gradually.
- Full player sink deals 12 damage and safe-recovers; horse full sink uses horse-safe recovery without horse damage.
- The mounted pair is recovered without double rider damage.

## V23R13.3 Constant-size enemy silhouette outline — IMPLEMENTED / UNITY VERIFICATION REQUIRED

- The rectangular GUI corner frame and pulse are removed.
- One red inverted-hull outline follows the actual enemy mesh shape.
- Thickness remains constant in pixels and does not grow with distance.
- Sword, hook, ranged, mounted, line-of-fire, wall, death, and menu rules remain authoritative.

## V23R13.4 Compiler-warning cleanup — IMPLEMENTED / UNITY VERIFICATION REQUIRED

- Remove the four reported assigned-but-unused fields from target highlight, Parry, and enemy placement safety.

## V23R13 acceptance gate

1. Installer and validator pass twice; repository hygiene and `git diff --check` pass.
2. Unity compiles without the four reported CS0414 warnings.
3. TEST EVERYTHING passes with zero blockers.
4. Player quicksand entry slows progressively, displays a following ring, clears after escape, and full sink deals 12 damage plus safe recovery.
5. Horse and mounted quicksand behavior recover safely without repeated or double damage.
6. Prototype scene contains one hole/chasm, one lava, and one quicksand volume with distinct readable surfaces.
7. One red silhouette outline follows the real enemy shape at legal melee/hook/projectile ranges on foot and mounted.
8. Outline thickness remains stable, never becomes a box, never pulses/grows, and never appears through blockers.
9. Existing V23R12 and retained regression gates remain passing.
10. Record real results before resuming the saved feature sequence.

# Active work — C01/C03/C04/C05/C11.RUNTIME.V23R12

## V23R12.1 Reliable hook pull and safe contact release — IMPLEMENTED / UNITY VERIFICATION REQUIRED

- Resolve and move the actual CharacterController/Rigidbody/combatant root even when `BDHealth` lives on a child.
- Classify pull size from the body controller/collider, not attack or awareness volumes.
- Keep large regular enemies, mini-bosses, and bosses damage-only.
- Stop before player overlap, ground the enemy, accept the new motion baseline, and suppress contact attacks briefly.

## V23R12.2 Mounted highlight and Parry transient cleanup — IMPLEMENTED / UNITY VERIFICATION REQUIRED

- Mounted targeting uses a horse-height ranged envelope and may frame the one enemy the current shot would hit.
- Successful Parry and menu/death reset clear active player slash visuals before time freeze or UI ownership changes.

## V23R12.3 Horse prompt readability — IMPLEMENTED / UNITY VERIFICATION REQUIRED

- Raise the unified horse action row.
- Suppress the legacy action sentence in the horse status presenter whenever the unified row exists.
- Preserve the approved on-foot/mounted-stationary/mounted-moving action matrix.

## V23R12.4 Enemy spawn, grounding, and teleport repair — IMPLEMENTED / UNITY VERIFICATION REQUIRED

- Ground placement converts the sampled surface to the correct CharacterController root.
- Validate before first visibility and after initialization settles.
- Continuous safety logic may correct vertical grounding but cannot relocate an active enemy horizontally across the player.
- Bootstrap, ground stick, jumper landing, hook completion, and the motion stabilizer share the root-aware contract.

## V23R12.5 Integrated Game Boy menu/death shell — IMPLEMENTED / UNITY VERIFICATION REQUIRED

- Draw the shell and content in the same `BDMainMenuFlow.OnGUI` pass.
- Remove the shell's independent GUI owner that could cover content or race the plain menu.
- Clear transient Parry/slash presentation before menu/death display.

## V23R12 acceptance gate

1. Installer and validator pass twice; repository hygiene and `git diff --check` pass.
2. Unity compiles and TEST EVERYTHING passes with zero blockers.
3. Hook pulls multiple small regular enemy archetypes consistently, stops before contact, and causes no immediate contact damage.
4. Mounted ranged aim displays one truthful target frame at legal projectile distance.
5. Parry slow time contains no frozen slash visual.
6. Horse action row is visibly separated from status/name/health text.
7. Repeated room combat produces no floating, floor-spawned, stuck-air, impossible-speed, or opposite-side teleporting enemy.
8. Main menu, death, pause, and settings show readable content inside one Game Boy shell.
9. Record the real result before resuming the saved feature sequence.

# Active work — C01/C03/C05/C12.RUNTIME.V23R11

## V23R11.1 Music and audio direction — COMPLETED AS DOCUMENTATION

- Root `ProjectGuide/Product/AUDIO_DIRECTION.md` is canonical and the asset-side music/audio document is its synchronized mirror.
- Exploration is cheerful nature/fantasy; standard combat, mini-boss, and boss increase rhythmic intensity in distinct tiers.
- Boss music uses fantasy-heavy-rock energy with sparse one-word vocal stabs.
- Mother uses an escalating child-song motif; phase 4 adds a synchronized tick-tock stem.
- Mixer groups, snapshots, transition timing, headroom, true peak, loudness, ducking, asset metadata, and QA are defined.
- Full runtime implementation remains later under C12.42.

## V23R11.2 Trap-layer bomb explosion and friendly fire — IMPLEMENTED / UNITY VERIFICATION REQUIRED

- Bombs now create visible core/ring/spark explosion feedback and generated impact audio.
- Player and horse damage routing is preserved.
- Other enemies take one damage event per `BDHealth`; multiple colliders cannot multiply damage.
- The bomb owner is excluded; enemy hits receive flash, stagger, radial knockback, and existing motion stabilization.

## V23R11.3 Airborne light/heavy animation repair — IMPLEMENTED / UNITY VISUAL VERIFICATION REQUIRED

- The initial press of hold-capable light/heavy input no longer decides the airborne visual.
- The actual committed attack resolves landing damage and presentation.
- Light and heavy reuse the grounded mesh/color/width identity in a vertical high-to-low plane.
- The horizontal slash is suppressed for the committed airborne attack.
- **Implementation checklist status: DONE in code and documentation; Play Mode PASS still required.**

## V23R11 acceptance gate

1. Installer and validator pass twice; repository hygiene and `git diff --check` pass.
2. Unity compiles and TEST EVERYTHING passes with zero blockers.
3. Bomb explosion is clearly visible and audible.
4. Player and horse receive expected bomb damage; at least two nearby enemies each receive one friendly-fire hit; owner is immune.
5. Jump + light shows one vertical light slash matching normal light identity.
6. Jump + heavy short press shows one vertical heavy slash matching normal heavy identity.
7. No horizontal duplicate or early hold-press slash appears.
8. Ground attacks, spin hold, hook hold, cooldowns, damage, camera, menu, horse, targeting, and enemy stability remain passing.
9. Record real results and resume the saved feature sequence.

# Retained work — C01/C03/C05/C11/C12.RUNTIME.V23R10

## V23R10.1 Menu/death UI ownership and Game Boy shell — PREPARED

- Root `ProjectGuide/Product/ART_DIRECTION.md` becomes the canonical visual source.
- `BDGameplayUiVisibility` prevents gameplay HUD and feedback from leaking into menu/death/pause/settings states.
- `BDGameBoyMenuShell` frames the existing readable menu and changes after true Mother victory.

## V23R10.2 Hook, Parry, target frame, and enemy stability — PREPARED

- Hook range becomes 13.5, hit radius 0.52, and configured release distance 2.35 with CharacterController-safe separation and post-release stagger.
- Parry uses anticipation, frozen moment, and gradual recovery; its ring and bursts remain parented to the player.
- A loaded ranged projectile may identify the one distant on-screen enemy its actual path would hit.
- `BDEnemyMotionStabilizer`, capped brain motion, and safer jumper landings prevent invalid teleport/overshoot/floating behavior.

## V23R10 acceptance gate

1. Installer and validator pass twice; repository hygiene and `git diff --check` pass.
2. Unity compiles and TEST EVERYTHING passes with zero blockers.
3. Menu/death/pause/settings contain no gameplay UI leakage and visibly use the Game Boy shell.
4. Hook stops small enemies before contact but inside sword range, with no immediate overlap damage.
5. Parry has a clear pre-freeze cue, player-following ring, and gradual release.
6. One truthful ranged target frame is visible at distance when a shot would hit.
7. Repeated combat shows no enemy teleport through the player, impossible speed burst, or persistent floating state.
8. Record focused results and resume the saved feature sequence.

# Active current work — C04/C11/C12.RUNTIME.V23R9

## V23R9.1 Horse interaction state matrix — PREPARED

- On foot near the horse: show Mount, Pet, and conditional Heal prompts with active bindings.
- Mounted and stationary: show Dismount only; Pet remains available by key without a prompt; Heal is disabled.
- Mounted and moving: show no horse action row; Dismount remains available by key; Pet and Heal are disabled.
- Mounting terminates and clears any active on-foot healing transaction.
- Mounted Pet uses a restrained saddle interaction and cancels when stationary conditions or safety conditions fail.

## V23R9.2 Art direction and interface conventions — PREPARED

- Establish 65% colorful wonder / 35% mystery and danger.
- Use polished stylized-fantasy geometry with hand-painted color design and restrained PBR materials.
- Preserve clear silhouettes, magical ambient atmosphere, child-centered coolness, and gameplay readability.
- Use clear fantasy headings, readable body typography, and Game Boy-inspired modern-resolution icons.
- Frame menus inside an original Game Boy-like in-world device.
- After true victory over Mother, persistently restore/awaken the device shell, magical inlays, boot treatment, and palette for the post-victory layer.
- Use one responsive visual language with platform-specific desktop and landscape-mobile layouts.
- Preserve the compact user-approved reference board without copying its brands or exact layouts.

## V23R9 acceptance gate

1. Installer and validator pass twice; repository hygiene and `git diff --check` pass.
2. Unity compiles and TEST EVERYTHING passes with the V23R9 domain scan.
3. On foot: Mount/Pet prompts appear when legal; Heal appears only when needed.
4. Mounted stationary: Dismount prompt appears, Pet works without a prompt, Heal never works.
5. Mounted moving: no row appears, Dismount works, Pet and Heal do not.
6. Mounted Pet cancels safely and restores control/visual state.
7. V23R8 and V23R6 retained regressions remain passing.
8. The art-direction document, reference board, index, architecture, decisions, QA, and status remain synchronized.
9. Record real results, then resume `C07.16A -> C07.16 -> C07.17`.

# Retained implemented work — C01/C03/C04/C11.RUNTIME.V23R8

- Explicit viewport-Y 0.40 camera composition is implemented.
- Unified horse prompt presentation and coherent mounted mouse steering are implemented; V23R9 refines the exact mounted visibility/availability matrix.
- Heavy-hold grappling hook C03.23A is implemented.
- Airborne light/heavy vertical presentation is implemented.
- One-target in-range red corner highlight is implemented.
- TEST EVERYTHING generated at `2026-06-07T17:56:35.5590780Z` passed with 0 blockers, 0 warnings, and 0 info.
- Focused Play Mode truth remains recorded separately from automated QA.
# Active blocking work — C01.QA.V23R6B-PROJECT-STATUS-TOKEN

## V23R6B.1 Restore canonical saved-feature token — BLOCKING / PREPARED

- Preserve V23R6 Runtime, diagnostics, scene work, Codex-agent work, and V23R6A semantic camera QA unchanged.
- Replace the non-canonical snapshot wording `Saved resume point` with the required stable contract `Saved feature resume point`.
- Record the real latest TEST EVERYTHING result: two documentation blockers, zero warnings, and zero info items.
- Treat the two reported blocker codes as one documentation cause, not as a Runtime or camera failure.

## V23R6B acceptance gate

1. Installer and validator pass twice on the exact post-V23R6A status file.
2. Only `ProjectGuide/Status/CURRENT.md` changes.
3. Repository hygiene and `git diff --check` pass after package cleanup.
4. TEST EVERYTHING no longer reports `DOCUMENTATION_CONTRACT_TOKEN_MISSING` or `PROJECT_STATUS_CURRENT_ACTIVE_WORK_MISSING` for `Saved feature resume point`.
5. Record the new automated result, then resume V23R6 focused walking and mounted transition verification.

# Active blocking work — C01.QA.V23R6A-SEMANTIC-COMPATIBILITY

## V23R6A.1 Replace stale V20 handoff-state anchor — BLOCKING / PREPARED

- Preserve the installed V23R6 Runtime, diagnostics, scene, Codex-agent changes, and authored map geometry unchanged.
- Remove the obsolete V20 requirement for the state string `completed union room handoff`.
- Require the active semantic Runtime anchors: `BD ACTUAL-POSE ROOM HANDOFF RELEASE V23R6`, `TryCompleteRoomHandoffAfterFinalPose`, and `completed actual-pose room handoff`.
- Keep V23 regression coverage strict; this is not a weakening or a compatibility comment inserted into Runtime.
- Record the real blocked TEST EVERYTHING result without committing the generated QA report itself.

## V23R6A acceptance gate

1. Installer and validator pass twice on the exact post-V23R6 local state.
2. Only `BDV20ActiveRegressionQA.cs`, `ProjectGuide/Status/CURRENT.md`, and `ProjectGuide/QA/QA_CHECKLIST.md` are changed.
3. `BDCameraFollow`, the scene, diagnostics, Codex agents, packages, and gameplay assets remain byte-for-byte untouched.
4. Repository hygiene and `git diff --check` pass after package cleanup.
5. Unity compiles and TEST EVERYTHING no longer reports `V20_CLOSED_WALL_CAMERA_CONTRACT_MISSING`.
6. Record the new automated result, then resume V23R6 focused walking/mounted transition verification.

# Active blocking work — C01/C11.RUNTIME.V23R6-ACTUAL-POSE-HANDOFF

## V23R6.1 Existing gated transition diagnostics — IMPLEMENTED LOCALLY / UNVERIFIED

- Preserve `BDCameraTransitionDiagnostics.cs`, its `.meta`, the camera integration, the QA integration, and the local Codex-agent guidance.
- Recording is user-gated: F8 starts/stops and exports, F9 exports, and F10 marks the observed pulse.
- Diagnostics record room switches, handoff start/end, target switches, wall-cast application, desired/contained/final camera poses, FOV, external camera writers, player/horse roots, visual offsets, and Animator root-motion state.
- Diagnostics do not become a second camera transform owner and do not record unless explicitly enabled.

## V23R6.2 Actual-pose room-handoff release — BLOCKING / PREPARED

- Preserve V23R5 first-frame cinematic ownership and V23R4 stable room containment.
- Preserve the current scene, diagnostics, Codex-agent changes, map geometry, corridors, openings, walls, portals, hazards, enemies, and minimap layout.
- Remove handoff completion from the pre-smoothing position-constraint stage.
- Keep the previous/current two-room union until the target is safely inside the new room.
- Require the actual final smoothed camera position to be inside the new room with the stable camera safety inset.
- Require the actual smoothed constrained look point to be inside the new room with the look-point inset.
- Complete the handoff after the final camera pose is written and before the diagnostic sample is captured, so diagnostics record the real completion frame.
- If motion remains after this repair, use the existing CSV diagnostics to distinguish camera movement from target/root/model movement rather than changing geometry speculatively.

## V23R6 acceptance gate

1. **PASS baseline:** local and remote were synchronized at `d6a73960b08889cc4fd4e3c14c8dd7dfc5deeecb`, and the pre-V23R6 TEST EVERYTHING baseline passed with 0 blockers and 0 warnings.
2. Installer preflight matches the captured local working tree and does not overwrite scene, Codex-agent, or diagnostics files.
3. Installer and validator pass twice; repository hygiene and `git diff --check` pass after cleanup.
4. Unity compiles without project errors or new warnings.
5. TEST EVERYTHING passes with the actual-pose release requirement, existing diagnostics requirement, and obsolete pre-smoothing release guard.
6. Repeated walking transitions in both directions produce no direction snap, apparent zoom, or forward/back jump.
7. Repeated mounted transitions at full speed produce no direction snap, apparent zoom, or forward/back jump.
8. The handoff flag clears only after the final camera body and smoothed look point are legal in the new room.
9. Closed-wall visibility, doorway passage, FOV, first-frame intro, death restart, holes, combat grounding, charged shot, AudioListener, BBH intro, and Console cleanliness remain passing.
10. If residual motion remains, record a short F8 session, press F10 at the visible pulse, export, and inspect the CSV before another repair.
11. Record real results, then resume the saved feature sequence.
# Active blocking work — C01/C11.RUNTIME.V23R5

## V23R5.1 First-render mounted cinematic camera ownership — IMPLEMENTED / AUTOMATED PASS / FOCUSED PARTIAL

- On a fresh New Game or approved cinematic/victory restart, prime the approved inside-room cinematic camera synchronously in `sceneLoaded` while the black cover is fully opaque.
- Disable `BDCameraFollow` before its `Start` or `LateUpdate` can render the entrance-close gameplay pose.
- Do not yield a frame before scene presentation setup begins.
- Preserve the original camera FOV/orthographic size and whether the follow driver was enabled; restore them only after the horse completes the right turn, full stop, camera return, and input-release sequence.
- If required mounted-intro objects cannot be resolved, restore normal camera ownership before fading the cover.
- Update stale V20 camera QA to require the active V23R4 `1.20f` inset, stable wall-pressure marker, smoothed look point, and handoff-only wall cast instead of old text anchors.

## V23R5 acceptance gate

1. Installer and validator passed twice on the exact post-V23R4 state.
2. Repository hygiene and `git diff --check` passed after package cleanup.
3. Unity compiled without project errors or new warnings in the reported synchronized TEST EVERYTHING run.
4. `Boredom And Dungeons -> TEST EVERYTHING` passed with zero blockers; the two stale V20 camera blockers are absent.
5. The user confirmed that the entrance-adjacent first-frame camera problem is much better; repeated focused first-frame verification remains part of the retained camera gate.
6. The first visible gameplay frame is intended to be the approved higher/farther inside-room view looking at the entrance.
7. The horse still enters, turns right, fully stops, and only then returns camera/control ownership.
8. Death -> New Game still skips the mounted intro and starts on foot with normal camera framing.
9. The remaining walking/riding node-transition motion is tracked under V23R6, not treated as a V23R5 first-frame failure.
10. Continue V23R6 focused diagnosis and the retained regression gates.

# Active blocking work — C01/C11.RUNTIME.V23R4

## V23R4.1 Stable camera containment without enlarging the maze — IMPLEMENTED / AUTOMATED PASS / FOCUSED OPEN

- Preserve the current room, corridor/opening, wall, enemy, hazard, portal, minimap, and scene placement geometry.
- Keep `BDCameraFollow` as the sole normal-gameplay camera transform owner.
- Make the 15.25-unit boom fit at room center by replacing the stale 2.25 scene inset with 1.20 and bounding legacy serialized values to a safe 0.90-1.35 range.
- Use room bounds for ordinary containment and run wall SphereCast only during an active legal room handoff.
- Smooth the independently constrained look point so player/horse height and wall pressure cannot pulse rotation or apparent zoom.
- Resolve room ownership at most once per frame and cache the room list between refreshes.
- Preserve closed-wall visibility, constant normal-gameplay FOV, cinematic ownership, planar shake, mouse yaw, and distance-preserving handoff contracts.

## V23R4 acceptance gate

1. Installer and validator passed twice on the post-V23R3A or post-V23R3B local state.
2. Repository hygiene and `git diff --check` passed after package cleanup.
3. Unity compiled in the synchronized TEST EVERYTHING run.
4. `Boredom And Dungeons -> TEST EVERYTHING` passed with zero blockers.
5. Rotate through cardinal/diagonal aim at room center; no distance pulse or apparent zoom should occur.
6. Walking and riding are substantially improved, but residual node-transition direction/zoom/forward-back motion remains open under V23R6.
7. Cross several authored openings in both directions; handoffs must remain smooth and cannot reveal/cross closed wall segments.
8. Verify closed walls still hide adjacent rooms from all approved angles.
9. Confirm normal gameplay FOV remains constant and inspect the Console; optionally confirm no recurring camera room-scan spike in the Profiler.
10. Complete V23R6 diagnosis before closing the focused V23R4 camera gate.

# Retained blocking work — C01/C11.DOCUMENTATION-QA.V23R3B

## V23R3B.1 Closed-wall visibility-boundary contract — IMPLEMENTED / AUTOMATED PASS

- Preserve the existing camera and room-boundary implementation unchanged.
- State explicitly that closed structural walls form a `visibility boundary` for camera body, look point, and adjacent-room geometry.
- Keep authored open doorways as the only legal visibility and actor-transition openings.
- Update current QA truth without weakening the existing wall, camera, or room-transition acceptance criteria.

## V23R3B acceptance gate

1. Installer and validator passed twice on the post-V23R3A state.
2. Repository hygiene and `git diff --check` passed after package cleanup.
3. Unity compiled in the synchronized TEST EVERYTHING run.
4. `Boredom And Dungeons -> TEST EVERYTHING` passed with zero blockers.
5. Focused Play Mode must continue confirming that closed walls hide adjacent rooms from frontal, side, corner, diagonal, high, and low camera angles.
6. Continue the retained gameplay and focused camera acceptance gates.

# Retained blocking work — C00/C01/C03/C10/C11.RUNTIME.V23R3

## V23R3.1 Remote/local current-truth synchronization — COMPLETE

- Preserve local Runtime, scene, QA, `.meta`, package, and Codex work from the uploaded working tree.
- Preserve remote documentation commits through `40104177ee396d19f375b62a20c410e4ac63bdc8`.
- `ProjectGuide/Engineering/ARCHITECTURE.md` already matched the remote blob and was not unnecessarily rewritten.
- Synchronize the maintained documents with remote truth plus the V23R3-V23R5 local state.
- Do not use reset, clean, broad checkout, or an old full-project package.
- Completed through checkpoint commit `4477834da3bd8bb028104e9dc8f17d6905ce61d2` and merge commit `3af374d839cd8eb917501ce8cc6f8c703cc45474`.

## V23R3.2 Version-agnostic current-status QA — IMPLEMENTED / PASS

- Current-status QA requires the semantic snapshot, active-work line, active-blocking heading, Runtime version marker, saved resume point, and retained C12.42 work.
- It must not freeze `C03/C11/C12.RUNTIME.V*` when the approved current categories are `C00/C01/C03/C10/C11`.

## V23R3.3 Semantic camera QA compatibility — IMPLEMENTED / PASS

- The V23 camera intentionally removed `movementDirectionBlend`.
- QA validates `BD SINGLE CAMERA TRANSFORM OWNER V23`, `BD STABLE SINGLE-STAGE CAMERA YAW V23`, `ResolveCameraIntentDirection`, `Vector3.RotateTowards`, `ResolvePlanarCameraShake`, and final `SetPositionAndRotation`.
- Do not reintroduce the obsolete field merely to satisfy a text scanner.

## V23R3.4 Named post-recovery walk suppression — IMPLEMENTED / PASS

- Preserve the existing stronger `0.85s` suppression rather than downgrading to the stale `0.55s` text contract.
- Use a named constant/property for the suppression and validate that semantic contract.
- Recovery continues clearing dodge, jump, forced-gap, and motion state before control returns.

## V23R2 gameplay acceptance retained under V23R3

- `BDCameraFollow` remains the only normal-gameplay Main Camera transform owner.
- Enemy hits cannot push the player below the floor or recover the CharacterController inside ground.
- Ordinary walking cannot enter holes from any angle; only active dodge, ascending jump, or explicit forced movement can intentionally enter.
- Hole movement uses a swept capsule-footprint test across the requested path.
- Hole fall recovery prefers a nearby valid point beside the same hole before older safe points or spawn.
- Recovery clears stale gap-entry state and suppresses immediate re-entry.

## V23R3 acceptance gate

1. Installer and validator passed twice on a copy of the uploaded local state.
2. Repository hygiene and `git diff --check` passed.
3. The seven stale static blockers were structurally removed without weakening active Runtime contracts.
4. Unity compiled in the synchronized TEST EVERYTHING run.
5. `Boredom And Dungeons -> TEST EVERYTHING` passed with zero blockers.
6. Focused hole-side and corner walking verification remains part of the retained Play Mode gate.
7. Intentional dodge/jump hole entry remains part of the retained Play Mode gate.
8. Near-hole recovery and immediate post-recovery walking remain part of the retained Play Mode gate.
9. Repeated enemy-hit grounding remains part of the retained Play Mode gate.
10. Camera/wall/room-transition behavior, charged shot, AudioListener, mounted intro, BBH first frame, and Console cleanliness remain in focused verification.
11. Safety checkpoint, remote fetch, merge, inspection, and push are complete.
12. Complete V23R6 focused diagnosis, then resume C03.23A.

# Approved future work — C06 SHOP/ECONOMY

## Merchant shop and run currency — APPROVED REQUIREMENTS / NOT IMPLEMENTED

- Durable specification: `ProjectGuide/Features/Economy/SHOP_AND_CURRENCY_SYSTEM_V1.md`.
- Place 2–4 shops per run: 1–2 in rooms that began empty, with the remainder spawning after combat-room clear. Every run has at least one of each placement class.
- Merchant presentation: seated hooded monk-like figure with hidden face, original game-compatible cloak, rug, and exactly three offers.
- Weighted stock uses the complete approved item/price pool. Once-per-run items are rarer and may appear at most once across all shops and refreshes.
- Automatic stock refresh requires both elapsed-time and progressed-room gates and refills only purchased/empty slots; occupied offers remain unchanged.
- A separate fixed-cost `REROLL` replaces all three offers atomically. The exact fixed cost remains a balance value to approve.
- Friendly, HostileAlive, and Defeated are run-global merchant states. A hostile living merchant appears without stock and attacks; a defeated merchant never appears again and cannot refresh/reroll.
- Killing a friendly merchant makes the ordinary offers still on that rug free, and also creates an exclusive one-of-two light-sword/cannon reward choice. Taking one removes the other and doubles the matching base damage channel for the run with updated models/presentation.
- Enemy currency: 20% chance for 3–8. Appropriate breakables: 30% chance for 2–10 in the relevant reward slot.
- Runtime implementation waits until the current V23R19E stability and focused verification gates close.

# Approved future work — C06.META

## Cross-run meta progression — REQUIRED / DESIGN OPEN / NOT IMPLEMENTED

- Durable specification: `ProjectGuide/Features/Economy/META_PROGRESSION_SYSTEM_V1.md`.
- End every run with persistent meta points based on approved progress/performance factors.
- Meta points are separate from in-run shop money and are spent in a new main-menu area whose name and visual design remain open.
- Candidate unlocks include player/horse skins, a new playable character, a new boss, and additional content defined later.
- Every unlock has a point cost based on desirability/rarity/impact; the exact catalog, costs, formula, pacing, save schema, and abandon/crash policy require a dedicated future design phase.


# Approved future work — C03/C05/C06/C10/C11/C12 CROSS-SYSTEM EXPANSION

## Gameplay abilities, map teleport, living world, and UI polish — REQUIRED / NOT IMPLEMENTED

- Durable Hebrew source: `ProjectGuide/Features/Runtime/GAMEPLAY_ABILITIES_MAP_AMBIENT_UI_EXPANSION_HE_V1.md`.
- Required future scope includes: readable enemy attack animations and hit timing; player bomb inventory/use on `R`; generic collectible flutes; knockback and animal-summoning flutes; shared flute cooldown and short/long `Shift` input; summoned-creature AI/health/threat/despawn; two additional design-open flutes; jump/dodge animations; map teleport to safe visited-cleared rooms; ambient animals, insects, birds, fish, reactive vegetation, particles, footprints, spatial ambience, rare events, pooling/LOD/mobile performance; and professional menu/HUD polish.
- Ambient and summoned creatures must share approved base assets where practical while keeping combat state, AI, damage, targeting, room-clear, teleport, and reward behavior strictly separated.
- The source also defines extensive automated/manual QA and mandatory completion-report fields.
- This is a multi-stage product expansion. No item is considered implemented until its own Runtime, assets, UI/input, save/load, QA, performance, and focused Play Mode gates pass.
- Current V23R19F/V23R19E blockers remain earlier work and must close before this expansion begins.

# Ordered project categories

- **C00 Governance:** one authoritative status, current-only documentation, request capture, repository hygiene, and lossless remote/local synchronization.
- **C01 Stability/QA:** V23R19D automated baseline passed; V23R19E Unity compilation, TEST EVERYTHING, focused Runtime, documentation, and Console verification are active.
- **C02 Platform/architecture:** Unity 6000.0.76f1, runtime/editor separation, mobile-landscape target.
- **C03 Player/combat:** V23R19E owns the exact grounded-slash-rotated-90-degrees airborne presentation and player death lock; resume C03.23A only after its focused gate passes.
- **C04 Horse:** preserve mounted damage, prompts, healing, flee, hazard, and restart contracts; V23R19E repairs external-control rider attachment during the mounted intro.
- **C05 Enemies:** V23R19E adds visible death presentation before regular-enemy loot/despawn while preserving sword, patrol, charger, trap, ranged, jumper, and exit-interference behavior.
- **C06 Collectibles/rewards/economy:** V23R19E repairs Battery guardian activation; run currency, merchant shop/combat, partial refresh/full reroll, and required open-design meta progression remain approved future work.
- **C07 Boss framework:** after V23R19E and C03.23A pass, continue C07.16A -> C07.16 -> C07.17.
- **C08 Mini-bosses:** Square Jumper, Roller, Serpent, Quad Gunners; choose three per run.
- **C09 Narrative bosses:** preserve final-boss and complete Mother-boss contracts, including phase-specific Dodge budgets.
- **C10 Map/hazards:** preserve V23R19D quicksand slowdown/controlled-jump fixes and retained walking-proof hole recovery.
- **C11 Camera/UI:** V23R19E delays the death menu until the player death pose is visible while preserving camera, target outline, Game Boy menu, and abandonment confirmation.
- **C12 Art/audio:** final authored death/attack/merchant animation and audio remain mandatory production work; C12.42 AudioMixer routing remains later.
- **C13 Story/endings:** incomplete-set endings, secret continuation, Mother loss/victory, state isolation.
- **C14 Balance/release:** profiling, pooling, persistence, cleanup, target build, clean-clone verification, release tag.

# Exact current sequence

1. Install V23R19E on the exact uploaded post-V23R19D local state without resetting, cleaning, or replacing unrelated files.
2. Run package validation, repository hygiene, and `git diff --check`; wait for Unity compilation to finish.
3. Run `Boredom And Dungeons -> TEST EVERYTHING` and require 0 blockers and 0 warnings.
4. Confirm Pause -> Abandon -> confirm -> Start Game binds the active-scene player to the horse before movement and keeps the rider attached through the full entrance animation.
5. Verify Air Light and Air Heavy reuse their selected grounded slash identity rotated exactly 90 degrees, in front of the player, with no second horizontal slash.
6. Kill the player and confirm a readable player death animation completes before the Game Boy death menu appears.
7. Kill each regular enemy archetype and confirm death animation occurs before loot/despawn while attacks, movement, collision, and damage stop immediately.
8. Trigger Battery A and Battery B guardians, including collecting during the reveal; confirm guardians still activate, chase, attack, take damage, die, and retain Elite immunity to small-enemy pull/knockback.
9. Re-run retained quicksand slowdown, hook, wall jump, mounted hazards, camera, Parry, bomb, menu, and Console checks.
10. Record real automated and focused results in this file; only then close V23R19E and resume C03.23A -> C07.16A -> C07.16 -> C07.17.
11. Keep merchant shop/combat and C06.META as required future systems, and keep final authored animation/audio replacement as mandatory release work.
# Current risks

- Existing Unity scene serialization may retain old camera pitch values; Runtime migration must preserve the explicit 40/60 contract without rewriting the scene.
- Multiple standalone horse prompts can overlap the horse HUD; unified prompts must suppress legacy presenters without taking gameplay ownership.
- Range highlighting must use current attack availability and blockers, not merely enemy proximity.
- Hook pull must not override boss/large-enemy movement policies or bypass walls.
- Airborne presentation must not duplicate the committed melee attack or grant unapproved damage.
- The Art-direction conventions document must remain pending until user answers exist; inventing the style guide would create false project truth.

- Camera boom distance plus safety inset can exceed available room half-size and create orientation-dependent compression.
- Repeated wall casts or room scans inside one LateUpdate can produce visible jitter and avoidable frame spikes.
- Reducing containment pressure must not regress closed-wall visibility or legal doorway handoff.
- A semantically correct design document can still fail a stable-token QA contract when the exact durable term is omitted.
- A valid project-level agent instruction file can be misclassified as stale root documentation when the index, external hygiene script, and Unity governance QA drift apart.
- Rich-text editing exports can become duplicate instruction sources unless ignored explicitly.
- Static QA can contradict the active implementation when it freezes old field names, durations, or category/version labels.
- A dirty local branch with a stale remote-tracking ref can make ordinary pull/rebase instructions unsafe.
- Replacing local files with remote copies would lose Runtime/scene progress; ignoring remote would lose governance and design progress.
- A failed sequential installer can leave an intentional partial state; repair packages must detect and continue from it.
- Static QA can pass while visual Runtime behavior remains wrong.
- Mounted hazard damage callbacks can trigger normal buck/faint dismount logic unless the hazard-specific dismount happens before damage is applied.

# Current changelog

## 2026-06-08 — V23R19K explicit airborne branch and dialogue-spec capture

- The post-V23R19J Unity run reported 10 blockers with no warnings or info items.
- Inspection proved the remaining airborne blockers were not only wording drift: the new explicit airborne identity was returned but ignored by `BDPlayerCombat`, which still spawned the horizontal arc.
- V23R19K consumes that identity and chooses exactly one visual branch, then updates the remaining scanners to the active ownership.
- Resolved V23R19I compilation history remains in `ProjectGuide/Status/CURRENT.md`; it is not forced back into the open-bug table.
- The approved opening `I’m bored.` / wordless-character-voice prompt is captured as required future design and is not reported as implemented.


## 2026-06-08 — V23R19J semantic QA realignment and corrected Girl/Father specification

- Unity compiled after V23R19I and TEST EVERYTHING reported 15 semantic/text QA blockers with zero warnings.
- V23R19J updates active-owner scanners rather than reintroducing obsolete code or incorrect mounted-hook behavior.
- The V23R19B hook-reliability contract is restored in the design document, and the latest corrected Girl/Father/meta specification becomes canonical future work.


## 2026-06-08 — V23R19I compile compatibility and traversal-requirement capture

- Unity reported CS1061 and CS0117 because forced-movement call sites outlived the compatibility APIs in `BDCombatantProfile`.
- V23R19I restores the property/static API as semantic aliases without changing serialization or gameplay classification.
- The rope, climbing and quicksand-swamp specification is captured as required future work and remains not implemented.


## 2026-06-08 — V23R19H character-specific mounted-hook correction

- The previous request was interpreted incorrectly as enabling the hook while mounted for the current boy character.
- Correct contract: the boy cannot use the hook while mounted; the future Girl character may use it while riding when her character-specific mounted-combat capability is implemented.
- V23R19H removes the boy's mounted hook input/highlight path, updates QA and the open-bug ledger, and captures the approved Girl/Father/meta-progression prompt as future required design.


## 2026-06-08 — V23R19G reopened focused regressions, mounted hook, and canonical bug tracker

- V23R19F automated QA passed with 0 blockers, 0 warnings, and 0 info items.
- Focused Play Mode confirmed all unmentioned behavior looked correct, while reopening airborne rotation, player/large/guardian death presentation, clean abandon-to-menu navigation, and mounted replay binding.
- User added the requirement that the hook work while riding and required a permanently maintained open-bug tracker.
- V23R19G implements the focused repairs and creates the current defect ledger; Unity verification remains open.

## 2026-06-08 — V23R19F semantic QA repair and cross-system requirement capture

- The post-V23R19E TEST EVERYTHING run reported two static blockers and no warnings.
- Guardian Runtime already uses the current `playerTransform.position` same-room contract with a player-room fallback; the older scanner was frozen to `player.position`.
- V23R9 status QA was frozen to `V23R8 automated baseline`; maintained status uses `V23R8 automated QA passed`.
- V23R19F aligns both scanners with active truth without changing Runtime or scene behavior.
- The uploaded Hebrew gameplay/abilities/map/ambient-world/UI prompt is preserved as a required future specification and is not falsely marked implemented.


## 2026-06-08 — V23R19E run-flow, death, airborne, guardian, shop-state, and meta requirements

- The post-V23R19D TEST EVERYTHING run passed with 0 blockers and 0 warnings, but focused Play Mode exposed four Runtime/presentation defects: riderless abandon replay, incorrect airborne visual, abrupt death menu, and missing Battery guardians.
- User clarified that death animation is required for both player and enemies.
- V23R19E repairs the four active defects on the exact uploaded local state and adds focused regression QA.
- Merchant requirements now include partial empty-slot refresh, fixed-cost three-slot reroll, HostileAlive/Defeated global states, no commerce after aggression, free remaining rug stock on friendly defeat, and exclusive light-sword/cannon reward choice.
- Cross-run meta progression is recorded as required but design-open and remains separate from run currency.


## 2026-06-08 — V23R19D focused Play Mode findings

- Unity TEST EVERYTHING passed after V23R19C with 0 blockers, 0 warnings, and 0 info items.
- The user confirmed that jumping/moving inside quicksand no longer causes premature respawn.
- The same focused test showed no effective player slowdown, so multiplier delivery is repaired at the movement owner.
- The airborne attack is now distinct but needs a front-facing vertical overhead-to-floor presentation.
- Jumping onto an attacking enemy can arm the combat grounding guard and teleport the player backward; controlled jumps are now excluded.
- Abandon requires confirmation, and abandon -> New Game must bind the exact newly loaded player to the mounted intro horse.


## 2026-06-08 — V23R19C QA compatibility and future merchant-shop requirements

- TEST EVERYTHING reported exactly two static blockers and no warnings: one obsolete hook-rank token and one obsolete airborne visual-owner token.
- The active runtime already centralizes small-regular pull eligibility and places vertical slash spawning in `BDPlayerCombat`; V23R19C aligns QA with those owners without changing runtime behavior.
- The user approved a future 2–4-shop per-run merchant system, weighted three-item stock, complete item/price pool, enemy/breakable money drops, player/horse protection, and upgrades.
- Shop stock may refresh only after both elapsed-time and progressed-room thresholds pass. Unique once-per-run offers are rarer and removed from the run pool after their first appearance.
- Shop implementation is scheduled under C06 and does not interrupt the active regression gate.

## 2026-06-08 — V23R19B scene-safe install, hook commitment, and Battery guardians

- The first V23R19 package correctly stopped before writing because the local scene no longer matched its whole-file hash.
- V23R19B replaces whole-scene replacement with a structural patch of only the approved player movement fields.
- Hook eligibility is rechecked on impact and a valid living small-enemy hit commits the pull on the real movement root.
- Collectible guardians are now created fully inactive and activated atomically, avoiding partially initialized underground AI/collision state.
- Battery guardians are damageable Elite combatants: normal AI and health apply, while small-enemy pull/knockback/forced movement does not.

## 2026-06-08 — V23R19 quicksand, traversal reach, universal wall jump and airborne identity

- User reported a nonlethal quicksand jump/movement teleport, requested slightly farther normal jump and dodge, farther wall jump from every sensible solid vertical surface, and reiterated that airborne attack still used the regular presentation.
- V23R19 repairs the ownership/order issue rather than adding another visual suppression workaround.
- Unity and focused Play Mode verification remain required.

## 2026-06-08 — V23R18B rebuilt from the authoritative latest local ZIP

- The user identified `Boredom-and-Dungeons.zip` as the most up-to-date local state.
- The previous rebuilt installer correctly wrote nothing, but its preflight looked for `ForceDismountAfterHazardRecovery` in the wrong owner file.
- Inspection confirmed that `BDHorseController` owns the API and `BDHorseHazardSafety` owns mounted hazard ordering.
- Hazard-specific dismount now occurs before horse damage callbacks, horse relocation and rider recovery.
- The exact lower-case animation token is synchronized across all three maintained animation/art documents.
- Unity verification remains required after installation.

## 2026-06-08 — V23R18A production animation and horse hazard damage ownership

- User required final production-grade animation coverage for every action that needs animation, including combat, locomotion, firing, jump, dodge, horse interactions, healing, knockback, stagger and stun.
- A dedicated production-animation contract was added and linked from canonical art direction.
- Horse hole/lava behavior now has explicit mounted and unmounted damage ownership.
- Mounted hole damages horse and rider; mounted lava damages the horse instead of the rider and uses a zero-damage rider recovery arc.
- The latest automated run was blocked only by two stale V23R13 quicksand scanner tokens, updated semantically by V23R18A.

## 2026-06-08 — V23R17 quicksand, enemy hazards, mounted impact, and wall jump

- V23R16 was safely rejected by preflight because newer local quicksand/document work existed.
- V23R17 was rebuilt on the exact uploaded local state.
- The package adds exact player quicksand, enemy hazard avoidance and forced-entry outcomes, variable 4-10 mounted impact damage for small enemies, correctly directed knockback, wall jump, clear-direction intro turn, and distinct airborne body animations.
- Unity verification remains open.

## 2026-06-07 — V23R15D exact canonical-root token compatibility

- TEST EVERYTHING after V23R15C reported one blocker, zero warnings, and zero info items.
- The mirror already named `ProjectGuide/Product/ART_DIRECTION.md`, but used `Canonical repository source`.
- The retained V23R9 scanner requires the exact phrase `Canonical root source`.
- V23R15D adopts that accurate phrase without changing the design policy or Runtime.

## 2026-06-07 — V23R15C art-direction mirror root-token repair

- TEST EVERYTHING ran after V23R15B with Unity compilation successful.
- The run reported exactly one blocker, zero warnings, and zero info items.
- The Unity-side art-direction mirror already contained the complete approved policy, but omitted the literal canonical filename `ProjectGuide/Product/ART_DIRECTION.md`.
- V23R15C adds the explicit root-source declaration without changing art policy or Runtime behavior.

## 2026-06-07 — V23R15B independent AOE criticals and semantic QA compatibility

- User clarified that every enemy hit by an AOE attack must calculate critical independently.
- Spinning sword AOE now shares one pre-critical spectrum roll but performs one exact 6% critical roll per unique enemy target.
- Light, heavy, and airborne sword attacks keep one critical state per committed attack.
- The same repair replaces the three latest stale exact-text QA contracts with active semantic checks.
- Runtime and automated/focused Unity verification remain open until the new run is supplied.

## 2026-06-07 — V23R15A QA result API compatibility

- Unity reported two CS1061 errors because `BDOneClickQAResult` has no `Blockers` collection.
- Unity reported two CS0246 errors because `BDOneClickQAIssue` does not exist.
- The maintained API is `result.findings.Add(new BDOneClickQAFinding(...))` with `BDOneClickQASeverity.Blocker`.
- V23R15A applies that exact pattern to the V23R14 and V23R15 scanners only.
- Runtime sword variance, critical behavior, projectiles, hook, scenes, and assets are unchanged.

## 2026-06-07 — V23R15 sword spectrum and critical attacks

- User specified an exact 6% critical chance and exact 1.5 damage multiplier.
- Eligibility is limited to light, heavy, airborne light/heavy, and spinning sword attacks.
- Sword damage now uses a configurable spectrum rather than one fixed value.
- Ranged projectiles and the grappling hook remain fixed damage and cannot crit through this system.
- Critical enemy damage numbers use a dedicated fuchsia/magenta color.
- Previous automated QA was blocked by a compilation aggregate; exact Unity compiler lines remain required if the new rerun still fails.

## 2026-06-07 — V23R14 damage numbers and occlusion-safe test labels

- User requested animated damage values for player and enemies with different colors.
- User reported first-room hole/lava/obstacle labels visible through walls and confusing adjacent-room readability.
- The latest automated run was blocked only by five stale semantic QA tokens and reported zero warnings/info items.
- V23R14 adds applied-damage number presentation, wall/distance/UI-state label gating, and aligns legacy QA with active V23R13 contracts.
- Visual and Play Mode verification remains open until tested in Unity.

## 2026-06-07 — V23R13 audio coverage, quicksand, and silhouette outline

- V23R12 automated QA passed with zero blockers, warnings, and info items.
- User requested a comprehensive, non-exclusive sound-event contract covering every action/state that requires audio.
- User requested immediate quicksand implementation and explicit completion tracking.
- User rejected distance-growing rectangular target frames and required a constant-size red outline following the enemy shape.
- Four reported CS0414 warning sources are removed.
- V23R13 implements the code/doc/QA changes; Unity and focused Play Mode verification remain open.

## 2026-06-07 — V23R12 focused Play Mode regression repair

- V23R11 automated QA passed with zero findings, but focused Play Mode exposed hook, mounted-targeting, Parry-visual, horse-prompt, enemy-grounding/motion, and menu-shell regressions.
- Hook pulling now resolves the actual movement root and body envelope and suppresses immediate contact attacks after safe release.
- Mounted targeting uses the ranged envelope; Parry/menu entry clear active slash visuals.
- Enemy ground placement is CharacterController-center-aware and continuous horizontal safety teleporting is removed.
- The Game Boy shell and menu content now share one GUI owner/pass.
- All items remain Unity-verification-required until the user confirms the focused gate.

## 2026-06-07 — V23R11 music direction, bomb explosion, and committed airborne animation

- V23R10 automated QA passed with 0 blockers, 0 warnings, and 0 info.
- User defined exploration/combat/mini-boss/boss/Mother music progression and requested correct channel/mix/master design.
- Canonical root and Unity-side audio-direction documents were added; runtime adaptive music remains under C12.42.
- Trap-layer bombs now have visible/audio explosion feedback and damage non-owner enemies once each.
- Airborne animation is now selected at actual attack commit and reuses the regular light/heavy mesh in a vertical plane.
- Code/document implementation is marked done; Unity visual/gameplay verification remains pending.

## 2026-06-07 — V23R10 menu, hook, Parry, targeting, and enemy stability

- Focused Play Mode found gameplay UI leaking over the menu/death screen, no final Game Boy shell, unsafe hook release distance, incomplete Parry payoff, insufficient distant ranged target framing, and unstable enemy displacement/grounding.
- The latest automated blockers are stale V23R8 documentation tokens only; art direction is no longer pending.
- V23R10 makes root `ProjectGuide/Product/ART_DIRECTION.md` canonical, adds one gameplay-UI visibility owner and a Game Boy shell, safely tunes the hook, phases Parry feedback, extends truthful ranged highlighting, and adds enemy motion stabilization.
- V23R10 automated verification pending.

## 2026-06-07 — V23R9 horse state matrix and art-direction conventions

- V23R8 automated QA passed with zero blockers, warnings, and info items.
- User refined horse interactions: on-foot actions show prompts; mounted stationary shows only Dismount while Pet remains invisible-key; mounted moving shows no row; Heal is on-foot only.
- User approved 65% colorful / 35% mysterious stylized fantasy, clear fantasy typography, Game Boy-inspired icons, a full Game Boy menu shell, smooth/elegant plus weighty animation, and responsive desktop/mobile language.
- The supplied visual references are preserved as one compact board and translated into an original non-copying production contract.
- True victory over Mother is recorded as a persistent restored/awakened Game Boy device state for the future post-victory layer.

## 2026-06-07 — V23R8 camera, horse, combat, and UX package

- The user confirmed that the V23R6 node-transition camera issue is fixed.
- Latest pre-V23R8 TEST EVERYTHING passed with 0 blockers, 0 warnings, and 0 info items.
- New reported regressions: target returned to screen center, horse Heal/Pet cues were badly placed, and mounted mouse control still felt inconsistent.
- New combat requirements: heavy-hold rope/hook with 2 damage, small-enemy pull, large-enemy damage-only, independent cooldown, and normal-heavy fallback.
- New presentation requirements: airborne light/heavy attacks keep their identity as vertical slashes; one subtle red frame marks only an aimed enemy in usable range.
- New horse UX requirements: Mount/Dismount/Heal/Pet prompts show the real key and appear only in their valid on-foot or mounted-stationary contexts.
- V23R7 failed safely before writes because `ProjectGuide/Engineering/ARCHITECTURE.md` had changed. V23R8 is rebuilt from the exact captured local state and preserves all newer local work.
- The Art-direction conventions document remains pending because the required user questionnaire has not yet been answered.

## 2026-06-07 — V23R6B canonical saved-feature status token

- V23R6A removed the stale V20 camera-state blocker.
- The next TEST EVERYTHING run reported two blockers, zero warnings, and zero info items.
- Both blockers came from one documentation wording mismatch: `Saved resume point` instead of `Saved feature resume point`.
- V23R6B changes only `ProjectGuide/Status/CURRENT.md` and restores the canonical token without touching Runtime or QA implementation.
- Focused walking and mounted node-transition verification remains next after automated QA is green.

## 2026-06-07 — V23R6A stale V20 semantic QA blocker

- The local-state-aware V23R6 actual-pose repair was installed and Unity TEST EVERYTHING ran.
- The run at `2026-06-07T16:23:06.1766460Z` reported exactly one blocker, zero warnings, and zero info items.
- The blocker was static only: `BDV20ActiveRegressionQA` still required `completed union room handoff`, which V23R6 intentionally replaced.
- V23R6A updates the scanner to require the actual-pose release marker, method, and state string without touching Runtime or scene behavior.
- Focused walking and mounted verification remains pending after automated QA is green.

## 2026-06-07 — V23R6 actual local diagnostics and actual-pose handoff release

- The first V23R6 installer correctly stopped because the local camera source had advanced beyond the remote-based structure.
- The captured local state showed new gated transition diagnostics, four updated Codex agent profiles, a modified Unity scene, and camera/QA integration that must be preserved.
- Local and origin/main both point to `d6a73960b08889cc4fd4e3c14c8dd7dfc5deeecb`; the listed changes are later uncommitted local progress.
- Inspection of that exact camera source found that handoff completion still used the unsmoothed desired camera position inside containment.
- The local-state-aware repair moves completion after the final pose write and requires both actual final camera position and smoothed look point to fit in the new room.
- Existing CSV diagnostics remain intact and will be used only if residual movement remains.
- No scene, Codex-agent, diagnostics, or map-geometry file is replaced by this repair.

## 2026-06-07 — Synchronized checkpoint and automated QA baseline recorded

- Local V23R2-V23R5 Runtime, scene, QA, Codex, governance, and documentation work was committed as `4477834da3bd8bb028104e9dc8f17d6905ce61d2`.
- Current origin/main history was merged without losing either side in `3af374d839cd8eb917501ce8cc6f8c703cc45474` and pushed to remote main.
- Unity 6000.0.76f1 TEST EVERYTHING passed at `2026-06-07T12:56:12.9858310Z` with 0 blockers, 0 warnings, and 0 info items.
- This automated PASS does not close the remaining visual node-transition camera/model-motion regression.
- V23R6 diagnosis is now the active next implementation step.

## 2026-06-07 — V23R6 remaining node-transition camera motion

- V23R5 substantially improved the first visible mounted-cinematic frame and removed the main entrance-adjacent camera flash.
- A smaller issue remains while walking or riding: crossing procedural nodes can slightly change the camera direction.
- Intermittently the framing appears to zoom in or out, or jump forward and backward.
- It is not yet proven whether the moving element is the camera transform, player or horse gameplay root, animated model, or more than one system.
- Remote/local synchronization is complete; the remaining camera regression is explicitly preserved as open work.
- TEST EVERYTHING now passes on the synchronized state, but focused visual diagnosis remains required.
- V23R6 begins with instrumentation and evidence-based isolation before another geometry or camera redesign.

## 2026-06-07 — V23R5 first-frame mounted camera leak

- User reported one visible entrance-close camera frame before the mounted intro camera reaches its approved pose.
- TEST EVERYTHING had remained blocked only by two obsolete V20 camera text contracts after V23R4.
- V23R5 primes cinematic ownership in `sceneLoaded`, removes the initial frame yield, and aligns V20 QA to the active V23R4 implementation.
- The user subsequently confirmed that the result is much better; the separate node-transition motion is tracked under V23R6.

## 2026-06-07 — V23R4 walking and mounted camera micro-jitter

- User reported continuing micro-jumps and apparent zoom pulses while walking and riding.
- Static inspection found the scene inset left 14.75 usable room units for a 15.25 camera boom, so normal framing was already compressed at room center.
- Normal follow also recast against structural walls and reconstrained the look point with the same large inset; room resolution could be requested repeatedly in one frame.
- V23R4 keeps authored maze geometry unchanged and repairs camera containment, look-point smoothing, room caching, and handoff-only wall casting instead.
- Automated QA now passes; residual node-transition visual motion remains open under V23R6.

## 2026-06-07 — V23R3B room-boundary documentation QA compatibility

- V23R3 and V23R3A installed and validated successfully; repository hygiene passed after cleanup.
- Unity TEST EVERYTHING then reported one blocker only: `ROOM_BOUNDARY_DESIGN_MISSING` for the missing token `visibility boundary`.
- The maintained design already described the behavior, so V23R3B added the explicit durable term without changing Runtime or scene behavior.
- The later synchronized TEST EVERYTHING run passed with zero blockers.

## 2026-06-07 — V23R3A Codex instruction and repository-hygiene repair

- V23R3 installer and semantic validator passed on the user's current working tree.
- The subsequent repository-hygiene check blocked only because `AGENTS.md` was not listed as canonical root Markdown.
- `AGENTS.md` and `.codex/` are intentional current project inputs and must be preserved.
- `AGENTS.rtf` is classified as a local duplicate and is ignored rather than deleted.
- V23R3A updates the documentation map, workflow, technical decision, external hygiene script, and Unity governance QA without touching Gameplay or scene behavior.

## 2026-06-07 — V23R3 remote/local synchronization and QA compatibility

- User supplied the full local repository ZIP and required reconciliation with current GitHub without losing either side.
- Actual remote main was verified at `40104177ee396d19f375b62a20c410e4ac63bdc8`, 33 documentation-only commits ahead of the stale local `origin/main` reference.
- Local HEAD `99daaee` and the dirty working tree contained unique Runtime, scene, QA, package, and Codex progress.
- The reported static blockers were one obsolete camera field, two missing permanent-document tokens, two obsolete post-recovery text anchors, and two frozen status-category strings.
- V23R3 preserved the stronger active Runtime contracts, synchronized documentation, and made QA semantic/version-agnostic.
- Synchronization and automated Unity QA are now complete; focused V23R6 diagnosis remains active.

<!-- B&D V23R19R CURRENT EXECUTION ORDER START -->
# Current mandatory execution order — V23R19R

1. **Blocker repair:** remove the stale `B&D POCKET ADVENTURE` QA expectation and obtain TEST EVERYTHING 0/0/0.
2. **Enemy Attack Animations:** implement and verify visible windup, commit, impact/release, follow-through, recovery and interruption for every active attack-capable enemy archetype.
3. **Professional Opening Cinematic:** high hidden-map start, controlled dive, slightly farther final framing, mounted entrance, visible Boy from first frame, stop, `I'm bored.` bubble and reusable nonverbal speech sound.
4. **Retained verification:** complete every implemented-but-unconfirmed V23R19O/V23R19Q visual, gameplay, responsive and performance gate.
5. **Resume remaining queue:** architecture audit and future systems in the canonical master sequence.

Canonical detailed queue:

`ProjectGuide/Status/WORK_QUEUE.md`

Nothing is considered verified merely because it was installed, compiled, or passed automated QA. Bug, Play Mode, performance, device, visual, audio and user-confirmation states remain separate.
<!-- B&D V23R19R CURRENT EXECUTION ORDER END -->

<!-- B&D V23R19S CONTINUITY QA REPAIR START -->
# Current automated repair — V23R19S

The V23R19R master work sequence and continuity contract are present.

The remaining blocker is caused by a brittle sentence-level string check, not by missing continuity information.

V23R19S requires the six verification levels independently:

1. installer/static validation;
2. compilation;
3. TEST EVERYTHING;
4. focused Play Mode;
5. target-device/performance verification;
6. user acceptance.

No earlier level implies a later level.

After a clean automated rerun, the next implementation remains **Enemy Attack Animations**. The opening cinematic remains immediately after enemy attack animations.
<!-- B&D V23R19S CONTINUITY QA REPAIR END -->

<!-- B&D V23R19T PHASE-AGNOSTIC QA REPAIR START -->
# Current automated repair — V23R19T

Historical validation stages must not require their own phase identifier to remain the current phase forever.

V23R19T converts V23R19R and V23R19S status checks to stable project truth:

- `ProjectGuide/Status/CURRENT.md` has a `CURRENT` classification;
- it references `MASTER_ACTIVE_WORK_SEQUENCE_V1.md`;
- Enemy Attack Animations are next;
- Professional Opening Cinematic follows;
- the open-bug tracker retains its permanent update discipline.

Historical bug IDs remain available in Git and status history, but old QA scanners no longer require them to remain open indefinitely.

After a clean 0/0/0 automated result, begin Enemy Attack Animations.
<!-- B&D V23R19T PHASE-AGNOSTIC QA REPAIR END -->

<!-- B&D 2026-06-09 HORSE HUD MINIMAP + REPO MAINTENANCE V2 START -->
## Local V2 package — Unity verification required

Implemented locally through a no-Git-write patch package: exact horse injury speed bands, slower healing with explicit presentation, removal of above-horse icons, proximity horse health, contextual player-health/ammo visibility, minimap rank markers and professional idle dimming. A safe repository-size audit/cleanup tool records before/after measurements without history rewriting or lossy asset changes.

### Exact resume point

After package application: open Unity, clear compiler errors, run `Boredom And Dungeons → TEST EVERYTHING`, then complete `VERIFY_AFTER_APPLY.txt`. Only after all checks pass should the user make the local commit and resume the existing work queue without reordering it.
<!-- B&D 2026-06-09 HORSE HUD MINIMAP + REPO MAINTENANCE V2 END -->

<!-- BND_TUTORIAL_REFERENCE_LED_V3:BEGIN -->
## Current local refinement — first-launch tutorial V3

```text
Reference-led pixel world: IMPLEMENTED IN LOCAL PACKAGE
Modern dominant instruction card: IMPLEMENTED IN LOCAL PACKAGE
Parallel input cards: IMPLEMENTED IN LOCAL PACKAGE
Unity compilation: REQUIRED
TEST EVERYTHING: REQUIRED
Keyboard/mouse-only run: REQUIRED
Physical-handheld-only run: REQUIRED
Mixed-input run: REQUIRED
Visual approval: REQUIRED
```

The ordered work queue remains unchanged and must resume only after this focused tutorial gate passes.
<!-- BND_TUTORIAL_REFERENCE_LED_V3:END -->

<!-- BND_FIRST_LAUNCH_TUTORIAL_PRODUCTION_COURSE_V10:BEGIN -->
## Current user-prioritized sequence — tutorial production course and saved-run follow-up

```text
1. Modern Handheld base visual/input verification remains an acceptance dependency.
2. First-launch tutorial V10 production course: IMPLEMENTED IN LOCAL PACKAGE / UNITY VERIFICATION REQUIRED.
3. Persistent run Continue, Save & Return, New Game overwrite confirmation and Abandon scoring: APPROVED / QUEUED AFTER TUTORIAL VERIFICATION.
4. Professional handheld↔gameplay transition: integrate four distinct intents after the saved-run task.
5. Return to the previously preserved Runtime/QA/enemy-animation/audit queue afterward.
```

Abandon scoring is now approved as 84% of the meta points the shared death evaluator would award at the same moment. Its shared result screen must close before the agreed exit animation begins. This is documentation truth only; the saved-run/meta implementation has not yet been written or verified.

The V10 tutorial package does not change the normal run, meta balance or result flow. Next action after installation is Unity compilation and the focused V10 QA contract.
<!-- BND_FIRST_LAUNCH_TUTORIAL_PRODUCTION_COURSE_V10:END -->

<!-- BND_FIRST_LAUNCH_TUTORIAL_V10_WARNING_CLEANUP_V101:BEGIN -->
## Current interruption — V10.1 compiler-warning cleanup

The uploaded Unity run on `2026-06-10T02:45:38.8280090Z` reported `TEST EVERYTHING: PASS` with `0 blockers / 0 warnings / 0 info`, but Unity compilation also emitted six `CS0414` warnings for write-only tutorial demonstration fields. Automated QA did not classify those compiler warnings, so the tutorial is not yet at the required clean-compilation gate.

V10.1 removes the redundant booleans, keeps `TutorialLearningState` as the single source of tutorial learning evidence, and adds a regression check forbidding the retired fields. Unity compilation and TEST EVERYTHING must be rerun after applying V10.1. The saved-run/Continue/Abandon-scoring task remains next after tutorial verification.
<!-- BND_FIRST_LAUNCH_TUTORIAL_V10_WARNING_CLEANUP_V101:END -->

<!-- BND_FIRST_LAUNCH_TUTORIAL_V10_INPUT_RESPAWN_FLASH_REPAIR_V102:BEGIN -->
## First-launch tutorial V10.2 input, respawn and transition repair

Status: `IMPLEMENTED BY DELIVERY PACKAGE / UNITY VERIFICATION REQUIRED`.

Focused user Play Mode review of V10.1 found three earlier blockers before tutorial acceptance:

1. displayed and consumed tutorial bindings did not match the live gameplay contract: Jump used Up/W instead of Space, Dodge used Space instead of directional double-tap, and Parry was described as one dedicated heavy key rather than a correctly timed light or heavy melee attack;
2. checkpoint recovery teleported the player after a short fall pose without a readable transition;
3. the legacy/main-menu surface could receive a brief visible frame while `BDMainMenuFlow` was still resolving after the BBH intro.

V10.2 repairs those items without redesigning tutorial encounters:

- desktop Jump is `Space`; physical-handheld Jump is `B`;
- Dodge is committed by a second left/right direction tap within `0.30s` on keyboard, controller D-pad or physical D-pad;
- Parry accepts the same light/heavy melee inputs used by gameplay when timed before impact;
- respawn now uses a cached full-screen fade, `RETURNING TO CHECKPOINT...`, hidden checkpoint restoration and a controlled reveal;
- the tutorial presentation is reserved before flow resolution and legacy-menu suppression covers that reservation, eliminating the one-frame old-menu exposure;
- focused QA now rejects the retired labels and implementation paths.

The uploaded V10.1 automated baseline was clean (`0 blockers / 0 warnings / 0 info`), but that result predates V10.2. Unity compilation, TEST EVERYTHING and focused Play Mode must run again. The tutorial remains the active priority; the saved post-tutorial run-resume/Abandon-scoring task and prior repair queue remain unchanged.
<!-- BND_FIRST_LAUNCH_TUTORIAL_V10_INPUT_RESPAWN_FLASH_REPAIR_V102:END -->

<!-- BND_FIRST_LAUNCH_TUTORIAL_ENTRY_GATE_V103:BEGIN -->
## First-launch tutorial V10.3 — entry choice and launch-frame gate

Status: `IMPLEMENTED / UNITY VERIFICATION REQUIRED`.

V10.3 adds the required pre-tutorial handheld choice (`PLAY TUTORIAL` / `SKIP TUTORIAL`) and a launch-presentation reservation that prevents the legacy menu from owning the frame exposed after the BBH intro. The presenter is installed before scene load and releases the reservation only after the BBH intro has completed and `BDMainMenuFlow` has resolved.

The source ZIP cleanup defect is corrected in the V10.3 installer: successful application removes the exact V10/V10.1/V10.2/V10.3 tutorial package ZIPs from `~/Downloads`; blocked or failed application preserves the package and rollback data.

Not yet verified in Unity:
- BBH intro → choice screen with no legacy-frame flash;
- Play and Skip routes on all supported input schemes;
- skip persistence across restart;
- clean compiler output and `TEST EVERYTHING`.

Current next action: install V10.3, reset the tutorial, run the focused launch/choice matrix, then continue the tutorial animation production pass defined by `Features/UI/FIRST_LAUNCH_TUTORIAL_ENTRY_AND_ANIMATION_V11.md`.
<!-- BND_FIRST_LAUNCH_TUTORIAL_ENTRY_GATE_V103:END -->

<!-- BND_FIRST_LAUNCH_TUTORIAL_PROGRESSION_GATE_REPAIR_V104:BEGIN -->
## First-launch tutorial V10.4 — forward progression repair

Status: `IMPLEMENTED / UNITY VERIFICATION REQUIRED`.

V10.4 corrects the mid-course dead end reported after dismount. Spin and Grapple lesson actors now spawn at their forward authored stations instead of obsolete coordinates behind the player. Hard step boundaries are represented by a visible reusable pixel gate, mounted melee/Hook input is rejected, the ranged lesson starts on its final round so Reload follows one valid shot, and the tutorial entry page uses real point-filtered pixel glyphs.

Current next action: install V10.4, reset the tutorial, complete one uninterrupted full run through every mechanic, rerun `TEST EVERYTHING`, then continue the dedicated tutorial animation production pass. The main-game animation backlog remains open.
<!-- BND_FIRST_LAUNCH_TUTORIAL_PROGRESSION_GATE_REPAIR_V104:END -->

<!-- BND_INTRO_TO_MAIN_MENU_CINEMATIC_AND_TUTORIAL_SPACING_V105:BEGIN -->
## V10.5 — intro-to-main-menu cinematic and tutorial-choice spacing

Status: `IMPLEMENTED / UNITY VERIFICATION REQUIRED`.

V10.5 adds an explicit one-shot `IntroToMainMenuTransition` emitted by BBH completion and consumed only by the first real main-menu destination. The handheld starts in a wider, differently angled table shot with the real screen active, follows an eased cubic camera path and settles exactly on the ordinary menu pose while input remains locked. Regular entries never replay it.

The first-launch choice also separates `B&D` from `Boredom & Dungeons` with a non-overlapping pixel hierarchy.

Current next action: install V10.5, verify the cinematic and spacing matrix, rerun TEST EVERYTHING, then continue the full tutorial animation production pass.
<!-- BND_INTRO_TO_MAIN_MENU_CINEMATIC_AND_TUTORIAL_SPACING_V105:END -->

<!-- BND_BBH_GLOBAL_TIMESCALE_REMOVAL_V106:BEGIN -->
## V10.6 — BBH global time-scale ownership correction

The latest Unity run was blocked by `FIRST_LAUNCH_TUTORIAL_CONTRACT_INVALID` because `BDBBHBootIntro` still assigned the global simulation clock to zero. V10.6 removes that presentation-layer ownership while preserving the realtime BBH timeline and local input/presentation gate. Unity recompilation and a clean `TEST EVERYTHING` run are required before continuing to the animation-production task.
<!-- BND_BBH_GLOBAL_TIMESCALE_REMOVAL_V106:END -->

<!-- BND_POST_INTRO_TRANSITION_COLORED_OUTPUT_CLEAN_EXIT_V1072:BEGIN -->
## V10.7.2 — post-intro transition delivery repair and clean installer exit

V10.7.1 accepted the authoritative tutorial source but its post-write validator scanned the editor QA source as Runtime code and rejected the validator's own forbidden-token strings. V10.7.2 scopes runtime validation correctly and makes package cleanup unconditional on success or failure. Unity verification remains required before continuing the tutorial gameplay and animation tasks.
<!-- BND_POST_INTRO_TRANSITION_COLORED_OUTPUT_CLEAN_EXIT_V1072:END -->
