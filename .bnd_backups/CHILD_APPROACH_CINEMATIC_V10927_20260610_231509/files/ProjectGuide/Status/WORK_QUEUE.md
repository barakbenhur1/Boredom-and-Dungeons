<!-- BND_POST_INTRO_CHAIR_AND_QA_REPAIR_V10926:BEGIN -->
## 2026-06-11 — Post-intro chair + QA access repair V10.9.26
- Added a centered wooden dining chair in front of the table, aligned with the handheld.
- The chair uses the new `BDCinematicDiningChair` texture resource and stays in the same physical room staging.
- Added chair geometry and chair contact shadows in `BDModernHandheld3DPresenter.CinematicEnvironment.cs`.
- Repaired the Unity editor compile blocker by changing `BDFirstLaunchTutorialQA.Scan()` from private to internal so `BDOneClickQAWindow` can call it.
- Runtime focus: no limbo/cyclorama reintroduction; room staging remains the active implementation.
<!-- BND_POST_INTRO_CHAIR_AND_QA_REPAIR_V10926:END -->

<!-- BND_POST_INTRO_REAL_ROOM_AND_CLOSER_FRAMING_V10925:BEGIN -->
## Current gate — V10.9.25

1. Apply the real-room and closer-framing implementation.
2. Wait for Unity compilation.
3. Run `Boredom And Dungeons -> TEST EVERYTHING`; require `0/0/0`.
4. Check the wide shot: no ramp, no limbo, no visible side-wall boundary, normal warm floor, wallpaper wall at believable distance.
5. Check the final shot: slightly closer, complete handheld visible, small wood margin below.
6. Keep the post-intro cinematic active until explicit visual acceptance.
<!-- BND_POST_INTRO_REAL_ROOM_AND_CLOSER_FRAMING_V10925:END -->

<!-- BND_POST_INTRO_FINAL_FIRST_LAUNCH_QA_REPAIR_V10924:BEGIN -->
## Immediate gate — V10.9.24

1. Apply the focused first-launch QA repair.
2. Wait for Unity compilation.
3. Run `Boredom And Dungeons -> TEST EVERYTHING`.
4. Require `PASS | blockers=0, warnings=0, info=0`.
5. Continue visual validation of the already-installed cinematic.
6. Commit and push only after automated PASS and visual acceptance.
<!-- BND_POST_INTRO_FINAL_FIRST_LAUNCH_QA_REPAIR_V10924:END -->

Resolved authoritative final-look target: `new Vector3(0f, -7.18f, -4.18f)`.

<!-- BND_POST_INTRO_CINEMATIC_WALLPAPER_FOCUS_DELIVERY_REPAIR_V10916:BEGIN -->
## Current task gate — V10.9.16 wallpaper and focus polish

1. Apply V10.9.16 over installed V10.9.13.
2. Let Unity compile all Runtime/QA changes.
3. Run `Boredom And Dungeons -> TEST EVERYTHING`; require `0/0/0`.
4. Review the shot, especially the final frame.
5. Verify:
   - blur is subtle and clean, not smeary;
   - the full handheld is inside frame;
   - a small wood margin remains visible under the device;
   - wallpaper contributes character to the room;
   - screen readability remains perfect.
6. Do not move to the retro tutorial redesign before explicit cinematic acceptance.
<!-- BND_POST_INTRO_CINEMATIC_WALLPAPER_FOCUS_DELIVERY_REPAIR_V10916:END -->

<!-- BND_POST_INTRO_CINEMATIC_QA_LATEST_COMMIT_ALIGNMENT_V1094:BEGIN -->
## Current immediate gate — V10.9.4 latest-commit-aligned QA repair

1. Apply the V10.9.4 package without resetting the installed V10.9.1 cinematic work.
2. Let Unity recompile `BDModernHandheld3DQA.cs`.
3. Run only `Boredom And Dungeons -> TEST EVERYTHING` and require `0 blockers / 0 warnings / 0 info`.
4. If clean, continue the existing V10.9 shot review at 0.0s, 1.3s, 2.2s, 3.3s and the final frame.
5. Do not commit before the automated gate and visual acceptance pass.
<!-- BND_POST_INTRO_CINEMATIC_QA_LATEST_COMMIT_ALIGNMENT_V1094:END -->

<!-- BND_POST_INTRO_CINEMATIC_DIRECTOR_PASS_V109:BEGIN -->
## Current immediate gate — V10.9 post-intro cinematic director pass

1. Apply the V10.9.1 backup-aware package from the existing Terminal session without resetting or discarding V10.8.1 work; the parent shell must remain open on success or failure.
2. Compile in Unity `6000.0.76f1` and run `Boredom And Dungeons -> TEST EVERYTHING`; require `0/0/0`.
3. Reset first launch and let BBH land on the tutorial-choice or Main Menu destination.
4. At 0.0s verify a complete grounded table, visible floor and high/far/left framing.
5. At 1.3s and 2.2s verify continuous descent/advance, no duplicates and legs still readable.
6. At 3.3s verify near-frontal alignment, natural leg exit and preserved tabletop thickness.
7. At the final frame verify exact Main Menu framing, readable live screen, visible front edge/apron, zero movement or correction and seamless input enable.
8. Repeat at 24-equivalent capture, 30 FPS and 60 FPS; return from Settings, Progression and Credits and verify no replay.
9. Recheck mounted-shot progression and all prior V10.8.1 acceptance items to prove no regression.
10. Commit only after user acceptance, then continue with the already queued retro tutorial visual redesign.
<!-- BND_POST_INTRO_CINEMATIC_DIRECTOR_PASS_V109:END -->

<!-- BND_FIRST_LAUNCH_TUTORIAL_V1081_HOTFIX:BEGIN -->
## Current immediate gate — V10.8.1 cumulative hotfix

1. Apply the cumulative V10.8.1 package without resetting or discarding prior V10.8 work.
2. Terminal delivery gate is package-verified: interactive semantic colors and ANSI-free `NO_COLOR=1` / `TERM=dumb` / redirected logs passed.
3. Compile and run `Boredom And Dungeons -> TEST EVERYTHING`; require `0/0/0`.
4. Reset the tutorial and verify mounted shooting progresses only after actual projectile impact, then reaches Reload and Charged Shot without becoming stuck.
5. Verify the post-BBH composition remains a full-screen table scene and only the camera animates; the handheld/table/shadow never scale or slide.
6. Recheck all prior V10.8 acceptance items to prove no regression.
7. After the mechanics gate is accepted, continue with `Tasks/QUEUED/FIRST_LAUNCH_TUTORIAL_RETRO_VISUAL_REDESIGN.md`.
8. The new enemy/model/difficulty contract remains queued at `Tasks/QUEUED/NEW_ENEMY_ARCHETYPES_ARTICULATED_MODELS_AND_DIFFICULTY_GRAPH.md` and must not be partially improvised during this hotfix.
<!-- BND_FIRST_LAUNCH_TUTORIAL_V1081_HOTFIX:END -->

<!-- BND_FIRST_LAUNCH_TUTORIAL_MECHANICS_REPAIR_V108:BEGIN -->
## Current immediate gate — V10.8 tutorial mechanics and cinematic repair

**Status:** `IMPLEMENTED / PACKAGE + STATIC PASS / UNITY VERIFICATION REQUIRED`

1. Apply the package/static-verified V10.8 ZIP to the exact current local repository; its installer must continue to block unknown edits before overwrite.
2. Wait for Unity compilation and resolve every project-generated error/warning.
3. Run `Boredom And Dungeons -> TEST EVERYTHING`; require `0 blockers / 0 warnings / 0 info`.
4. Reset first-launch state and verify BBH lands through one continuous full-screen 3D camera/device shot with no flat/sliding frame.
5. Complete the tutorial with keyboard/mouse, then repeat with physical controls/mixed input.
6. Intentionally test injured-horse Mount rejection, moving leg frames, mounted impact timing, Hook completion, enemy body collision and local checkpoint recovery.
7. Test Charged Shot three ways: release before threshold, release during charge, hold through automatic fire; then verify impact and Reload.
8. Fight both boss phases slowly enough to confirm persistent instructions, readable attack states, avoidable projectile/contact behavior and safe recovery damage windows.
9. Record real evidence in Current/Bugs/Verification. Commit only after user acceptance; then return to the preserved broader tutorial/main-game animation order.
<!-- BND_FIRST_LAUNCH_TUTORIAL_MECHANICS_REPAIR_V108:END -->


## Current immediate gate

1. Install V5 full-project package.
2. Run Unity compilation and TEST EVERYTHING.
3. Capture Main, Progression, Credits, Pause, quit confirmation and abandon confirmation.
4. Verify device height, visible left shadow, upper-right glass glint, aligned UI, stacked New Game cards, recessed labels and textured 3D controls.
5. Do not resume queued Runtime repairs until the user accepts this visual/input gate.

# Master Active Work Sequence V1

## Current blocking slice — V4 handheld physical-product repair

1. Install and compile the V4 package.
2. Run TEST EVERYTHING; automated status must remain 0/0/0.
3. In Main Menu verify no full-face decal crosses controls, body depth/bevels read clearly, and the short soft shadow falls left.
4. Verify the New Game-only text card appears only for fresh Start Game/New Run and contains no image, Boy/Girl route or Mother text.
5. Verify arrows and WASD are equivalent, including input release gating.
6. Verify the upper-right glass glint is visible but never obscures labels or artwork.
7. Only after user visual approval return to the previously preserved Runtime repair queue.


## Priority -6 — Premium handheld texture, layout and contextual option artwork

**Status:** `IMPLEMENTED / UNITY VERIFICATION REQUIRED`

1. Superseded: V3 upgraded the front decal, but V4 removes that rejected approach and uses molded material/geometry instead.
2. Remove duplicate center-button labels and keep one clean label owner.
3. Prevent long titles from colliding with artwork.
4. Give Main/Pause options dedicated context artwork.
5. Restrict Boy/Girl switching to Start Game / New Run; keep every other image character-neutral.
6. Compile, run TEST EVERYTHING and complete focused Play Mode/user visual acceptance before advancing.

## Priority -5 — Repair first Play Mode handheld regressions

**Status:** `IMPLEMENTED / UNITY VERIFICATION REQUIRED`

1. Render the live uGUI menu reliably into the physical screen RenderTexture.
2. Prevent the Escape press that opens Pause from closing it again during presenter activation.
3. Remove oversized/duplicated hardware labels and preserve correct X/Y/A/B orientation.
4. Give every physical face/shortcut button an independent, forgiving hit target while animating its real 3D mesh.
5. Run TEST EVERYTHING, then focused Main/Pause and six-button mouse checks before any cinematic transition work.

## Priority -4 — Restore handheld compilation

**Status:** `IMPLEMENTED / UNITY VERIFICATION REQUIRED`

1. Install the corrected project package.
2. Let Unity Package Manager resolve `com.unity.ugui` `2.0.0`.
3. Require zero compiler errors for `BDModernHandheld3DPresenter`.
4. Rerun TEST EVERYTHING and require `0 blockers / 0 warnings / 0 info`.
5. Do not begin visual tuning or transition implementation while compilation is blocked.

## Priority -3.5 — Seamless handheld-to-gameplay transition

**Status:** `CAPTURED / BLOCKED BY BASE HANDHELD VERIFICATION`

After the 3D handheld is compiled and visually verified, implement the expanded professional opening/exit contract in `ProjectGuide/Tasks/QUEUED/PROFESSIONAL_OPENING_CINEMATIC.md`:

- live gameplay inside the device screen before zoom-in;
- frame-matched menu-camera to gameplay-camera handoff;
- high-sky gameplay-camera starting pose and hidden HUD;
- opening dive only after handoff;
- reverse exit/abandon transition back into the handheld;
- no cut, black frame, snap, double transition, early HUD or fixed-delay state drift.

## Priority -3 — Modern 3D handheld Main/Pause verification

**Status:** `IMPLEMENTED / STATIC PASS / UNITY VERIFICATION REQUIRED`

1. Install the full-project implementation package.
2. Wait for Unity compilation and resolve every compiler/Console issue before testing behavior.
3. Run `Boredom And Dungeons -> TEST EVERYTHING`; require 0 blockers, 0 warnings and 0 info unless the user explicitly accepts a finding.
4. Verify Main Menu visual hierarchy, real 3D shell volume, screen recess, glass thickness/reflection and blue-orange material response.
5. Verify mouse selection, D-pad/arrow/WASD navigation, Main Menu X New Game / A Progression / B Settings / Y Credits, non-main B Back, center SELECT and center EXIT.
6. Start as Boy and verify Start Game / New Run shows Boy art; start as Girl and verify only that option switches to the matched Girl art. No random or stale image is allowed.
7. Verify Progression, Settings, Credits, Quit/Return, Resume/Pause and confirmation each use the correct character-neutral image; Pause resumes safely and returns to a clean main menu only after confirmation.
8. Repeat open/close/reload and inspect Console, RenderTexture cleanup, materials, listeners, idle GC and frame pacing.
9. Record real evidence in Current/Bugs/Verification. Only then move this task out of ACTIVE and return to the preserved Runtime repair order.

Implementation-specific focused checks also include the uploaded masked front texture, exact uploaded wood source, progressive near/far table defocus, slight top-away camera angle, upper-right light response, short left shadow, page transitions contained inside the screen and direction-specific D-pad travel.

## Priority -2 — Project Guide reorganization

**Status:** `AUTOMATED VERIFIED / MIGRATION COMPLETE`

- Move maintained project knowledge out of Unity `Assets/` into `ProjectGuide/`.
- Provide a concise mandatory entry page and organized topic hierarchy.
- Preserve detailed feature truth, remove duplicate mirrors and one-off historical QA reports, and update all references/QA paths.
- After package validation, continue directly to the real 3D handheld Main/Pause task.



## Priority -1 — Clear ProjectGuide Unity discovery blockers

**Status:** `VERIFIED — TEST EVERYTHING 0/0/0 AT 2026-06-09T00:13:48.3411810Z`

- TEST EVERYTHING at `2026-06-09T00:06:07.0833090Z` reported 9 documentation-only blockers after the reorganization.
- Restore stable V23R8/V23R9/V23R10 discovery language in the new index/art owner.
- Restore the exact V23R19Q task headings required for durable handoff.
- Do not recreate retired duplicate documentation and do not weaken QA.
- Rerun TEST EVERYTHING and require 0 blockers, 0 warnings and 0 info before starting the 3D handheld vertical slice.

<!-- B&D USER-PRIORITIZED MODERN 3D HANDHELD START -->
## User priority override — upright 3D Main/Pause device before Priority 0

**Status:** `CURRENT / IMPLEMENTED / UNITY VERIFICATION REQUIRED`

The user explicitly requested this stage before returning to the prior repair and enemy-animation sequence.

Required order:

1. install/review the complete asset and interaction specification;
2. inspect current local menu/input/presentation ownership;
3. build a testable real-3D vertical slice of the upright device;
4. implement screen depth/glass and tactile physical controls;
5. implement mouse + D-pad + final X/A/B/Y contextual shortcuts + SELECT/EXIT center buttons;
6. adapt Main and Escape/Pause without inventing unsupported pages/data;
7. implement deterministic Boy/Girl paired art selection only for Start Game / New Run and character-neutral art routing elsewhere;
8. run static, compilation, TEST EVERYTHING, focused Play Mode, performance and user acceptance gates;
9. only then return to the saved prior blocker/repair sequence below unless the user reprioritizes again.

Canonical specification:

`ProjectGuide/Production/ModernHandheld/MODERN_HANDHELD_3D_SPEC.md`

Important:

- The existing target-outline/ring and QA/runtime issues remain open; this priority change does not verify or close them.
- `Progression` is the final user-facing label.
- Every Boy image requires a matched Girl image.
<!-- B&D USER-PRIORITIZED MODERN 3D HANDHELD END -->


## Purpose

This is the canonical cross-session execution queue for the current Boredom & Dungeons work.

It exists so a new ChatGPT conversation, Codex session, developer, or future package can continue without relying on chat memory.

This document must be updated whenever:

- a blocker appears or is cleared;
- a bug is found, repaired, reopened, accepted, or rejected;
- implementation begins or stops;
- automated QA passes or fails;
- Play Mode verification passes or fails;
- the user confirms or rejects behavior;
- task order changes;
- a future requirement is added or clarified;
- the exact resume point changes.

`ProjectGuide/Status/CURRENT.md` remains the global summary.
`OPEN_BUG_TRACKER.md` remains the current open-defect ledger.
This file owns the complete ordered execution sequence and the distinction between implemented, verified, unverified, and future work.

---

## Status vocabulary

- `BLOCKING`
- `IMPLEMENTED / AUTOMATED VERIFICATION REQUIRED`
- `AUTOMATED PASS / PLAY MODE REQUIRED`
- `PLAY MODE PASS / USER CONFIRMATION REQUIRED`
- `VERIFIED`
- `REOPENED`
- `PLANNED`
- `FUTURE / NOT IMPLEMENTED`
- `DEFERRED`
- `SUPERSEDED`

Never collapse these states into a generic “done”.

---

# Mandatory execution order

## Priority 0 — Close current automated blocker

### V23R19R — remembered-handheld QA contract

**Status:** `IMPLEMENTED / AUTOMATED VERIFICATION REQUIRED`

Current TEST EVERYTHING result:

- generated UTC: `2026-06-08T21:09:44.6225160Z`;
- blockers: 1;
- warnings: 0;
- info: 0;
- blocker: `V23R10_GAME_BOY_MENU_SHELL_MISSING`;
- stale expected text: `B&D POCKET ADVENTURE`.

Reason:

V23R19Q intentionally replaced the old prototype label with the approved professional remembered-handheld identity. The old QA contract still requires the removed label.

Required repair:

- update the old V23R10 scanner to require the active `B&D // POCKET MEMORY` identity;
- never restore the old visible label merely to satisfy QA;
- add a regression scanner preventing the stale token from returning;
- rerun TEST EVERYTHING.

Acceptance:

- 0 blockers;
- 0 warnings;
- 0 info;
- no Runtime/UI regression;
- the active professional shell remains unchanged.

No work below Priority 0 is considered verified until this automated blocker is clean.

---

## Priority 1 — Enemy attack animations

**Status:** `PLANNED / NEXT IMPLEMENTATION`

This is the next implementation immediately after Priority 0 passes.

### Why this task exists

Enemies currently have attack logic, damage timing, movement states, and telegraphs, but visible authored attack motion is incomplete, inconsistent, or missing across archetypes.

An enemy must visibly communicate:

1. anticipation/windup;
2. committed attack;
3. exact contact or projectile-release moment;
4. follow-through;
5. readable recovery;
6. interruption/death cleanup.

The task exists to make combat readable, fair, satisfying, and professionally synchronized without silently changing balance.

### Required coverage

Audit and implement every current attack-capable enemy, including at minimum the active known categories:

- Sword / close-melee enemy;
- Charger / rammer;
- Jumper / leap attacker;
- Patrol / guard melee behavior;
- bomb-placing or bomb-throwing enemy;
- Exit Blocker attack behavior;
- Battery Guardians / Elite guardians;
- any current mini-boss or boss attack still using placeholder/no visible motion;
- any additional active enemy attack path discovered during the implementation audit.

Do not assume a type is covered merely because it shows a telegraph.

### Animation contract

Every attack path must expose the same semantic phases:

- `Windup`
- `Commit`
- `ImpactOrRelease`
- `FollowThrough`
- `Recovery`
- `Cancelled`

The gameplay owner remains authoritative for:

- whether the attack is legal;
- cooldown;
- target;
- damage;
- range;
- projectile creation;
- collision;
- AI state.

Animation may synchronize to those events but must not become a second damage or AI owner.

### Archetype-specific intent

- Sword: body anticipation plus visible weapon windup, slash, follow-through, recovery.
- Charger: body compression, backward brace, explosive launch, impact reaction, deceleration/recovery.
- Jumper: crouch/compression, takeoff, airborne posture, landing impact, recovery.
- Patrol/Guard: readable guard-to-attack transition, strike, recoil/recovery.
- Bomb enemy: deliberate bomb preparation/placement/throw, exact release frame, recoil and return.
- Guardian/Elite: heavier anticipation, weight, impact and recovery; must not reuse “small enemy” motion blindly.
- Boss/mini-boss: keep existing design and timing, but replace missing/placeholder attack motion where required.

### Preservation rules

Do not change without explicit need and documentation:

- damage values;
- hit radius;
- attack range;
- cooldown;
- AI selection;
- navigation;
- attack frequency;
- status effects;
- enemy rank;
- loot;
- boss phases.

If synchronization requires moving a damage or release timestamp, document the exact old/new timing and prove no double hit or lost hit.

### Fallback rule

Use an existing Animator/clip when present.

When final art clips do not exist, a procedural fallback may be used only when:

- it is archetype-specific;
- it is readable;
- it restores the exact base pose;
- it handles interruption/death;
- it is clearly documented as temporary procedural animation, not final release animation.

### Required QA

Automated:

- every attack-capable enemy has an animation presenter or explicit specialized animation owner;
- no damage is duplicated by animation;
- telegraph, commit, damage/release and recovery order is explicit;
- interruption resets the visual state;
- death cannot leave an attack routine active;
- no per-attack Material/Texture allocation;
- no reflection-based gameplay dispatch.

Play Mode:

- test each archetype for at least several attacks;
- inspect windup readability;
- inspect exact impact/release synchronization;
- inspect recovery;
- interrupt by knockback/stagger where legal;
- kill during windup/recovery;
- test player and horse targets;
- verify no changed damage/cooldown/range;
- verify Guardians remain damageable but forced-movement immune as approved.

Exact deliverables for this priority:

- implementation;
- canonical enemy-animation contract;
- architecture ownership update;
- QA scanner;
- open-bug/status synchronization;
- focused verification checklist;
- exact list of archetypes actually tested.

---

## Priority 2 — Professional opening cinematic

**Status:** `PLANNED / AFTER ENEMY ATTACK ANIMATIONS`

### Preserved approved content

- the opening remains a mounted Boy-and-horse entrance;
- the Boy must be visible on the horse from the first visible cinematic frame;
- the horse enters through the doorway;
- the horse reaches the approved stop;
- the Boy says `I'm bored.` in a speech bubble;
- dialogue uses the reusable nonverbal speech-buzz language, with tone matching meaning;
- the sequence hands control back only after its authored finish.

### Required professional camera sequence

1. Start with the camera extremely high so the map itself is not visible.
2. Perform a controlled cinematic dive downward.
3. Settle into a final framing slightly farther back than the current framing.
4. Only after the camera is correctly established, show the Boy and horse crossing the entrance.
5. Use professional easing, no sudden snap, no doorway flash, no exposed incorrect camera frame.
6. Preserve clear room/map readability once the final shot is established.
7. Keep the rider and horse synchronized throughout.
8. The dialogue beat occurs only after the horse reaches its stop.

### Required polish

- authored anticipation and easing;
- camera acceleration/deceleration;
- stable horizon;
- no clipping;
- no map reveal before intended;
- no one-frame wrong camera;
- synchronized hoof/entrance/audio hooks;
- clean transition to gameplay camera;
- performance-safe implementation.

### Verification

- fresh start;
- abandon → main menu → Start Game;
- death → Start Game if that path uses the cinematic;
- different aspect ratios;
- Boy visible from first frame;
- no camera snap;
- exact final distance;
- speech bubble and buzz;
- no control before release.

---

## Priority 3 — Retained implemented but not yet user-verified work

These items must remain visible until the user explicitly confirms them.

### V23R19O mounted intro visibility

**Status:** `IMPLEMENTED / USER CONFIRMATION REQUIRED`

- After abandon and Start Game, Boy must be visible from the first visible cinematic frame.
- No late pop-in at cinematic completion.
- Must be retested before Priority 2 and again after Priority 2.

### V23R19O target silhouette

**Status:** `IMPLEMENTED / USER CONFIRMATION REQUIRED`

- Red outline only on vulnerable/damageable enemy model.
- No red outline on non-damageable ground ring.
- One target only.
- Wall blocking and range remain correct.

### V23R19O auxiliary enemy ring transparency

**Status:** `IMPLEMENTED / USER CONFIRMATION REQUIRED`

- Ring remains visible.
- Ring is subtler/more transparent.
- It must not become unreadable.
- It must not inherit the target outline.

### V23R19O Wall Jump refinement

**Status:** `IMPLEMENTED / USER CONFIRMATION REQUIRED`

- higher;
- farther;
- directional steering during arc;
- player model turns with changed direction;
- camera yaw follows;
- normal grounded jump unchanged;
- works on valid vertical solid surfaces, enemies, horse, and other logical push-off geometry.

### V23R19Q professional handheld UI

**Status:** `IMPLEMENTED / AUTOMATED PASS AND USER VISUAL CONFIRMATION REQUIRED`

Must verify:

- original black boot hold;
- BBH order;
- circle and light sweep;
- professional surface does not obscure content;
- Main, Settings, Pause, Abandon, Loading;
- all options still work;
- remembered-handheld look rather than literal commercial replica;
- desktop and landscape-mobile-like aspect ratios;
- no overlap/cutoff;
- true-victory awakened state;
- no recurring texture creation, GC spikes, or visible frame hitch.

---

## Priority 4 — Resume existing product queue

After Priorities 0–3 are completed or explicitly deferred:

1. Resume `C01.ARCH.AUDIT.V1` Phase 1 repository-wide audit mapping.
2. Continue the previously approved feature order recorded in `ProjectGuide/Status/CURRENT.md`.
3. Keep all unresolved feature specifications visible.

Known future work includes:

- Caterpillar gambling NPC;
- merchant shop and run economy;
- meta progression;
- Girl character and Father route;
- reusable dialogue system;
- rope swinging;
- climbable vegetation;
- quicksand swamp expansion;
- final production animations;
- audio implementation;
- broader gameplay/map/ambient/UI expansion.

---

# Future systems that must not be treated as implemented

## Caterpillar gambling NPC

**Status:** `FUTURE / NOT IMPLEMENTED`

Canonical document:

`ProjectGuide/Features/Economy/CATERPILLAR_GAMBLING_NPC_V1.md`

Keep all current approved constraints, including:

- selected rooms only;
- not every room;
- animated appearance only when room is clear;
- animated disappearance when hostiles make room unsafe;
- active gambling session prevents enemy approach/attack;
- one game per Caterpillar;
- finite bankroll;
- passive refill threshold is not an absolute maximum;
- no invented rules/values.

## Merchant shop

**Status:** `FUTURE / NOT IMPLEMENTED`

Keep all approved inventory, room placement, refresh, reroll, hostility, death, free-loot, unique-item, money-drop, horse-upgrade, player-upgrade, and merchant-boss requirements in the canonical shop document.

## Meta progression

**Status:** `FUTURE / OPEN DESIGN`

End-of-run points based on progress/performance.
Future unlock area/name, exact rewards, costs, skins, characters, bosses, and progression balance remain open.

---

# Open-bug and verification discipline

For every current and future item:

1. A successful installer is not verification.
2. Compilation is not TEST EVERYTHING.
3. TEST EVERYTHING is not Play Mode.
4. Play Mode is not user confirmation.
5. A visual pass on one archetype is not coverage of all archetypes.
6. A bug remains open until the required acceptance level is reached.
7. A reopened bug retains its prior history but current documents show only current truth.
8. Resolved bug rows may be removed from the live open table only after durable truth is merged into canonical documents.
9. Every failure records:
   - timestamp;
   - exact message;
   - affected file/system;
   - classification;
   - next action.
10. Every user rejection immediately reopens the item even if automated QA passed.

---

# Exact current resume point

Install V23R19R and run TEST EVERYTHING.

If and only if the result is:

- blockers: 0;
- warnings: 0;
- info: 0;

begin Priority 1: Enemy Attack Animations.

Do not jump directly to the opening cinematic or architecture audit before the enemy-animation implementation and focused verification step.

<!-- B&D V23R19S PRIORITY 0 REPAIR START -->
## Priority 0 follow-up — V23R19S

Latest automated result:

- UTC: `2026-06-08T21:21:28.2403370Z`;
- blockers: 1;
- warnings: 0;
- info: 0;
- blocker: `V23R19R_CONTINUITY_CONTRACT_MISSING`.

Root cause:

The continuity document contains the correct verification ladder, but V23R19R QA requests one non-canonical prose sentence.

Repair:

- validate each verification level independently;
- keep the existing continuity contract and work order;
- add regression coverage;
- rerun TEST EVERYTHING.

Exact resume point:

- if TEST EVERYTHING is 0/0/0, begin Priority 1 — Enemy Attack Animations;
- otherwise remain in Priority 0 and repair only the reported blocker.
<!-- B&D V23R19S PRIORITY 0 REPAIR END -->

<!-- B&D V23R19T PRIORITY 0 REPAIR START -->
## Priority 0 follow-up — V23R19T

Latest automated result:

- UTC: `2026-06-08T21:26:24.0618690Z`;
- blockers: 1;
- warnings: 0;
- info: 0;
- blocker: `V23R19R_PROJECT_STATUS_MISSING`.

Root cause:

The V23R19R scanner requires its own historical phase ID to remain in the current PROJECT_STATUS snapshot. This creates a permanent contradiction whenever the project advances.

Repair:

- make V23R19R and V23R19S scanners phase-agnostic;
- validate stable work-order truth instead of exact historical IDs;
- stop requiring historical repair bug IDs to remain open forever;
- add V23R19T regression coverage;
- rerun TEST EVERYTHING.

Exact resume point:

- 0/0/0 → begin Priority 1 — Enemy Attack Animations;
- any blocker → remain in Priority 0 and repair only the reported blocker.
<!-- B&D V23R19T PRIORITY 0 REPAIR END -->

## Immediate verification order — final control repair

1. Compile and run TEST EVERYTHING.
2. Verify Main Menu face-button mapping and center SELECT/EXIT.
3. Verify B is Settings only on Main Menu and Back everywhere else.
4. Verify all pages align to the shared grid, the footer remains one line, and the New Game cards do not overlap.
5. Verify textured 3D control caps, visible short-left shadow, upper-right glass glint and raised device composition.
6. Do not commit or resume earlier Runtime work until the user accepts this visual/input gate.

<!-- BND_FIRST_LAUNCH_TUTORIAL_PRODUCTION_COURSE_V10:BEGIN -->
## Current temporary priority override

1. Verify the installed Modern Handheld base and V10 first-launch tutorial.
2. Complete all V10 tutorial Play Mode, input, timing, cleanup and user-acceptance gates.
3. Implement and verify Persistent Run Resume, non-destructive Save & Return, protected New Game overwrite and Abandon scoring/result routing.
4. Integrate New Game, Continue, Save & Return and Abandon into the professional handheld↔gameplay transition.
5. Resume the previously preserved Runtime blocker, enemy-animation and architecture-audit sequence without closing or skipping it.

This override changes ordering only. It does not mark any retained task complete.
<!-- BND_FIRST_LAUNCH_TUTORIAL_PRODUCTION_COURSE_V10:END -->

<!-- BND_FIRST_LAUNCH_TUTORIAL_V10_INPUT_RESPAWN_FLASH_REPAIR_V102:BEGIN -->
## Current interruption — tutorial V10.2 focused repair

Complete and verify this order before continuing tutorial feature expansion:

1. apply the V10.2 binding/respawn/legacy-flash repair;
2. recompile Unity and rerun TEST EVERYTHING;
3. verify keyboard/mouse, controller and physical-handheld mappings;
4. verify checkpoint fade/reveal in multiple encounters;
5. verify a clean BBH-intro-to-modern-handheld transition with no old-menu frame;
6. continue remaining tutorial Play Mode acceptance only after the three focused defects pass.

The post-tutorial persistent-run/Continue/regular-exit/Abandon-scoring task remains next after the tutorial is fully verified. The earlier runtime/QA repair resume point remains preserved after that.
<!-- BND_FIRST_LAUNCH_TUTORIAL_V10_INPUT_RESPAWN_FLASH_REPAIR_V102:END -->

<!-- BND_FIRST_LAUNCH_TUTORIAL_ENTRY_GATE_V103:BEGIN -->
## Temporary priority override — first-launch tutorial entry and animation

1. Verify V10.3 tutorial entry choice, launch-frame ownership and package cleanup.
2. Complete the tutorial production animation pass: player, horse, enemies, mini-boss, attack/impact synchronization, limbs and no-sliding acceptance.
3. Complete remaining tutorial gameplay/visual acceptance.
4. Implement persistent run Continue / safe return / Abandon scoring according to the queued contract.
5. Resume the previously saved repository work-queue position without closing or skipping older open work.
<!-- BND_FIRST_LAUNCH_TUTORIAL_ENTRY_GATE_V103:END -->

<!-- BND_FIRST_LAUNCH_TUTORIAL_PROGRESSION_GATE_REPAIR_V104:BEGIN -->
## Temporary priority override — tutorial V10.4

1. Verify the V10.4 pixel entry typography, mounted-action prohibition and full forward course completion.
2. Complete the tutorial production animation pass for player, horse, enemies and mini-boss.
3. Complete remaining tutorial visual/gameplay acceptance.
4. Implement persistent run Continue / safe return / Abandon scoring.
5. Resume the previously saved repository work-queue pointer.
<!-- BND_FIRST_LAUNCH_TUTORIAL_PROGRESSION_GATE_REPAIR_V104:END -->

<!-- BND_INTRO_TO_MAIN_MENU_CINEMATIC_AND_TUTORIAL_SPACING_V105:BEGIN -->
## Temporary priority override — V10.5

1. Verify cinematic and tutorial-choice spacing.
2. Complete tutorial production animations.
3. Complete remaining tutorial acceptance.
4. Implement Continue / safe return / Abandon scoring.
5. Resume saved repository queue.
<!-- BND_INTRO_TO_MAIN_MENU_CINEMATIC_AND_TUTORIAL_SPACING_V105:END -->

<!-- BND_POST_INTRO_TRANSITION_COLORED_OUTPUT_CLEAN_EXIT_V1072:BEGIN -->
## Immediate next action

Install V10.7.2, verify both post-BBH landing destinations and run TEST EVERYTHING. After acceptance continue in order: final-boss clarity, solid enemy blocking, locomotion animation, then the complete BBH intro color-direction pass.
<!-- BND_POST_INTRO_TRANSITION_COLORED_OUTPUT_CLEAN_EXIT_V1072:END -->
