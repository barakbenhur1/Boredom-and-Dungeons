<!-- BND_TUTORIAL_BUBBLE_DEPTH_HORSE_CONTINUE_V1011330:BEGIN -->
## V10.11.30.30 focused defects

| ID | Area | Status | Acceptance condition |
|---|---|---|---|
| `FL-CIN-MOTHER-FAR-DIAMOND-NO-FRAME` | Opening dialogue | `FIXED IN CODE / VISUAL VERIFY` | The far-left pointer diamond has a complete centered dark frame on all four edges. |
| `FL-RENDER-METAL-MEMORYLESS-LOAD-STORE` | Handheld screen rendering | `FIXED IN CODE / CLEAN-CONSOLE VERIFY` | The screen camera uses an owned persistent non-memoryless depth/stencil attachment; neither ignored load nor ignored store message appears after a fresh Play Mode entry. |
| `FL-TUT-HORSE-SHOOTER-DESPAWNS` | Horse-shot room | `FIXED IN CODE / PLAY MODE VERIFY` | The shooter remains after wounding the horse and disappears only when killed by the player. |
| `FL-TUT-HORSE-RETURN-DURING-SCROLL` | Horse return | `FIXED IN CODE / VISUAL VERIFY` | The horse is absent during scrolling and starts returning only after destination settlement. |
| `FL-TUT-MOUNTED-SHOT-TARGET-INVULNERABLE` | Mounted shot lesson | `FIXED IN CODE / PLAY MODE VERIFY` | The target dies from the required visible ordinary/charged projectile impact. |
| `FL-TUT-NO-CONTINUE-CUE` | Room completion | `FIXED IN CODE / FULL-RUN VERIFY` | A single professional pixel CONTINUE cue appears after each completed lesson and disappears before scrolling. |
<!-- BND_TUTORIAL_BUBBLE_DEPTH_HORSE_CONTINUE_V1011330:END -->

<!-- BND_TUTORIAL_HORSE_COMBAT_CONTINUE_V1011329:BEGIN -->
## V10.11.30.29 focused defects

| ID | Area | Status | Acceptance condition |
|---|---|---|---|
| `FL-TUT-HORSE-SHOOTER-DESPAWNS` | Horse-shot room | `FIXED IN CODE / PLAY MODE VERIFY` | The shooter remains visible after wounding the horse and disappears only when the player kills it during Jump Attack. |
| `FL-TUT-HORSE-RETURN-DURING-SCROLL` | Horse return staging | `FIXED IN CODE / VISUAL VERIFY` | The injured horse remains inactive during the room scroll and begins returning only after camera settlement plus the authored short beat. |
| `FL-TUT-MOUNTED-SHOT-TARGET-INVULNERABLE` | Mounted ranged lesson | `FIXED IN CODE / PLAY MODE VERIFY` | Visible ordinary/charged projectile impact supplies the required semantic damage source and can kill its lesson target. |
| `FL-TUT-NO-CONTINUE-CUE` | Room completion presentation | `FIXED IN CODE / FULL-RUN VERIFY` | Every edge-travel room displays one pixel-style CONTINUE badge after objective completion and removes it before scrolling. |
<!-- BND_TUTORIAL_HORSE_COMBAT_CONTINUE_V1011329:END -->

<!-- BND_TUTORIAL_FLOW_COHERENCE_V1011328:BEGIN -->
## V10.11.30.28 focused defects

| ID | Area | Status | Acceptance condition |
|---|---|---|---|
| `FL-TUT-OPENING-FACING-STUCK` | Opening room | `FIXED IN CODE / PLAY MODE VERIFY` | After landing beyond the first obstacle, the player follows actual movement direction and does not keep facing back toward the obstacle. |
| `FL-TUT-JUMP-LOST-AFTER-LESSON` | Persistent mechanics | `FIXED IN CODE / PLAY MODE VERIFY` | Jump remains available in every later on-foot room, including travel to the right edge after completing a lesson. |
| `FL-TUT-HEAL-REMOUNT-DELAY` | Horse flow | `FIXED IN CODE / PLAY MODE VERIFY` | Completing Heal Horse immediately presents Mount Again in the same room and Interact starts mounting without an edge transition. |
| `FL-TUT-HORSE-FIRST-FRAME-POP` | Horse presentation | `FIXED IN CODE / VISUAL VERIFY` | Horse hit and return sequences begin from their authored first pose; no idle horse flashes before the animation. |
| `FL-TUT-STRAY-GATE-WALL` | Room geometry | `FIXED IN CODE / FULL-RUN VERIFY` | Wall-jump geometry and the finish gate are active only in their owning rooms and never block Heal/Remount or unrelated rooms. |
| `FL-TUT-ROOM-ASSET-POP-IN` | Room transitions | `FIXED IN CODE / FULL-RUN VERIFY` | Static props, ordinary actors, hazards and geometry enter naturally with the scroll; HorseReturn is intentionally revealed only after settlement. |

<!-- BND_TUTORIAL_FLOW_COHERENCE_V1011328:END -->

<!-- BND_TUTORIAL_HORSE_FREE_OPENING_PET_SUPPRESSION_V1011327:BEGIN -->
## V10.11.30.27 focused defects

| ID | Area | Status | Acceptance condition |
|---|---|---|---|
| `FL-TUT-HORSE-IN-OPENING` | Opening room | `FIXED IN CODE / PLAY MODE VERIFY` | Horse GameObject and pixel visual remain inactive throughout WhiteBoot, Move and Jump. |
| `FL-TUT-HORSE-LESSON-TOO-EARLY` | Room order | `FIXED IN CODE / FULL-RUN VERIFY` | Room 1 is Quick Attack; Mount/Ride occurs only after Parry and immediately before EnemyArrival/HorseShot. |
| `FL-TUT-MOUNT-ROOM-SOFTLOCK` | Mount/Ride room | `FIXED IN CODE / PLAY MODE VERIFY` | Horse is positioned inside the deferred Mount room and the player can reach, mount and ride it. |
| `FL-TUT-PET-HUD-UPPER-RIGHT` | Legacy gameplay HUD | `FIXED IN CODE / VISUAL VERIFY` | The full-game upper-right PET card never appears during any first-launch tutorial phase or handoff. |
<!-- BND_TUTORIAL_HORSE_FREE_OPENING_PET_SUPPRESSION_V1011327:END -->

<!-- BND_TUTORIAL_CENTERED_PARRY_HORSE_METAL_V1011326:BEGIN -->
## V10.11.30.26 reopened blocking defects

| ID | Area | Status | Acceptance condition |
|---|---|---|---|
| `FL-TUT-QUICK-TARGET-NOT-CENTERED` | Quick Attack room | `FIXED IN CODE / VISUAL + PLAY MODE VERIFY` | Exactly one readable 64×92 passive one-health enemy remains at the exact visible room center until a valid Light impact kills it. |
| `FL-TUT-PARRY-TEACHER-DIES` | Parry lesson | `FIXED IN CODE / PLAY MODE VERIFY` | Player attacks cannot damage or kill the Parry teacher; the lesson completes only from a valid projectile parry. |
| `FL-TUT-PARRY-PROJECTILES-PERSIST` | Parry projectile lifecycle | `FIXED IN CODE / PLAY MODE VERIFY` | One tutorial projectile owner is active; production projectiles are cancelled, and success/exit cancels all projectile state before progression. |
| `FL-TUT-PARRY-SOFTLOCK` | Parry progression | `FIXED IN CODE / FULL-RUN VERIFY` | A successful parry reaches JumpAttack and right-edge travel; no dead teacher or residual projectile can block the transition. |
| `FL-TUT-HORSE-LESSON-WRONG-ROOM` | Opening room order | `FIXED IN CODE / FULL-RUN VERIFY` | MountHorse + RideHorse occupy the room immediately before EnemyArrival + HorseShot; Quick Attack follows in its own room. |
| `FL-RENDER-MEMORYLESS-DEVICE-MSAA` | Metal device-camera depth | `MITIGATION IMPLEMENTED / UNITY-METAL VERIFY` | Product/device camera uses no MSAA and no depth texture; screen RT remains depthless/non-memoryless; the load/store warning pair is absent after a fresh scene reload. |
<!-- BND_TUTORIAL_CENTERED_PARRY_HORSE_METAL_V1011326:END -->

<!-- BND_TUTORIAL_CONTINUOUS_ROOM_SEQUENCE_V1011325:BEGIN -->
## V10.11.30.25 remaining-room defects

| ID | Area | Status | Acceptance condition |
|---|---|---|---|
| `FL-TUT-ROOM-FADE-RECURRING` | Inter-room presentation | `FIXED IN CODE / FULL RUN VERIFY` | Every remaining room is revealed through one continuous camera/player move; no fade, black/white cover or opaque transition becomes active. |
| `FL-TUT-ROOM-EDGE-NOT-VISIBLE` | Completion travel | `FIXED IN CODE / FULL RUN VERIFY` | After objective completion the camera stays locked to the completed room, the card is hidden, and only physical contact with the visible right edge starts the next room. |
| `FL-TUT-WALLJUMP-EXIT-UNREACHABLE` | Wall Jump handoff | `FIXED IN CODE / PLAY MODE VERIFY` | Clearing the upper ground releases the mechanic clamp and permits travel to the same visible edge used by every other room. |
| `FL-TUT-NEXT-LESSON-EARLY-CLOCK` | Room entry timing | `FIXED IN CODE / FULL RUN VERIFY` | The next lesson card, timed sequence, enemy action clock, reload and input unlock begin after camera settlement, not while the room is still sliding into view. |
| `FL-TUT-ROOM-LAYOUT-LEGACY-COORDS` | Remaining room layout | `FIXED IN CODE / FULL RUN VERIFY` | Every remaining objective, actor, obstacle, hazard, secret, wall-jump route, boss and relic is positioned from its room center and remains reachable. |
<!-- BND_TUTORIAL_CONTINUOUS_ROOM_SEQUENCE_V1011325:END -->

<!-- BND_TUTORIAL_SCREEN_TWO_IMPACT_CONTINUOUS_HANDOFF_V1011324:BEGIN -->
## V10.11.30.24 focused defects

| ID | Area | Status | Acceptance condition |
|---|---|---|---|
| `FL-TUT-S2-LIGHT-SOURCE-NONE` | Screen-two combat | FIXED IN CODE / PLAY MODE VERIFY | The visible ordinary Light impact is routed as `Light`, kills the one-health centered target, and hides its image in the same impact frame. |
| `FL-TUT-S2-S3-CUT` | Room handoff | FIXED IN CODE / PLAY MODE VERIFY | After the kill, walking through the right edge reveals screen three continuously; no fade, black overlay, respawn, teleport or player-position rewrite occurs. |
<!-- BND_TUTORIAL_SCREEN_TWO_IMPACT_CONTINUOUS_HANDOFF_V1011324:END -->

<!-- BND_TUTORIAL_SECOND_SCREEN_LIGHT_ATTACK_V1011323:BEGIN -->
## V10.11.30.23 second-screen defects

| ID | Area | Status | Acceptance condition |
|---|---|---|---|
| `FL-TUT-SCREEN2-OLD-STORY-BEATS` | Screen-two entry | `FIXED IN CODE / PLAY MODE VERIFY` | Superseded by V10.11.30.26: completing Mount/Ride enters the separate EnemyArrival/HorseShot story room, then Quick Attack opens in the following room. |
| `FL-TUT-SCREEN2-NOT-ON-FOOT` | Screen-two player state | `FIXED IN CODE / PLAY MODE VERIFY` | Player begins screen two on foot at the left; the horse is not visible on this screen. |
| `FL-TUT-SCREEN2-TARGET-PLACEMENT` | Ordinary-attack target | `FIXED IN CODE / PLAY MODE VERIFY` | Exactly one passive one-health enemy is visible at screen center when the screen opens. |
| `FL-TUT-SCREEN2-EARLY-TRANSITION` | Screen-two completion | `FIXED IN CODE / PLAY MODE VERIFY` | Enemy death hides the lesson immediately, but screen three opens only after the player reaches the visible right edge. |
<!-- BND_TUTORIAL_SECOND_SCREEN_LIGHT_ATTACK_V1011323:END -->

<!-- BND_TUTORIAL_OPENING_SCREEN_SEQUENCE_V1011322:BEGIN -->
## V10.11.30.22 first-screen defects

| ID | Area | Status | Acceptance condition |
|---|---|---|---|
| `FL-TUT-OPENING-MOVE-JUMP-WRONG-SCREEN` | First tutorial screen | `FIXED IN CODE / PLAY MODE VERIFY` | Move completion hides/replaces the Move card with Jump on the same screen; no screen transition or respawn occurs between them. |
| `FL-TUT-MOVE-ATTEMPTED-DISTANCE` | Move proof | `FIXED IN CODE / PLAY MODE VERIFY` | Only real forward displacement counts toward the 64-unit objective; pushing left at the world edge or into the obstacle cannot finish Move. |
| `FL-TUT-RIDE-EXIT-UNREACHABLE` | First-screen exit | `FIXED IN CODE / PLAY MODE VERIFY` | Ride hides at its mounted-travel objective and the subsequent screen-exit point remains reachable inside the RideHorse movement boundary. |
<!-- BND_TUTORIAL_OPENING_SCREEN_SEQUENCE_V1011322:END -->

<!-- BND_SCREEN_RENDER_SCHEDULING_V1011321:BEGIN -->
## V10.11.30.21 focused defect

| ID | Area | Status | Acceptance condition |
|---|---|---|---|
| `RENDER-V1011321-DUPLICATE-SCREEN-CAMERA` | Metal/editor scene restore | `FIX IMPLEMENTED / UNITY VERIFY` | Scene restore and tutorial entry emit no memoryless depth load/store pair; the handheld screen remains correct because the enabled screen camera renders once through Unity's normal schedule. |
<!-- BND_SCREEN_RENDER_SCHEDULING_V1011321:END -->

<!-- BND_TUTORIAL_QA_THRESHOLD_REALIGNMENT_V1011320:BEGIN -->
## V10.11.30.20 QA blocker

| ID | Area | Status | Acceptance condition |
|---|---|---|---|
| `FL-TUT-QA-MOVE-THRESHOLD-CONFLICT` | Automated QA | `FIXED IN QA / UNITY RERUN REQUIRED` | The legacy opening-polish scanner requires the active `64f` Move threshold, forbids retired `12f`, and produces neither `TUTORIAL_V101111_FORWARD_COURSE_MISSING` nor `TUTORIAL_V101113_START_OR_TRIGGER_REGRESSION`. |
<!-- BND_TUTORIAL_QA_THRESHOLD_REALIGNMENT_V1011320:END -->

<!-- BND_TUTORIAL_RUNTIME_INTEGRITY_V1011319:BEGIN -->
## V10.11.30.19 blocking tutorial regressions

| ID | Area | Status | Acceptance condition |
|---|---|---|---|
| `FL-TUT-MOVE-INSTANT-COMPLETE` | Move lesson | `FIXED IN CODE / PLAY MODE VERIFY` | Move requires 64 world units of actual horizontal travel; one small tap cannot complete the lesson or start the next-screen transition. |
| `FL-TUT-EMPTY-INSTRUCTION-CARD` | Lesson UI | `FIXED IN CODE / PLAY MODE VERIFY` | Completion releases the instruction latch, disables the whole composition, and no empty panel/shadow/card returns while travelling. |
| `FL-TUT-SCREEN-CHANGE-READS-AS-RESPAWN` | Lesson transition | `FIXED IN CODE / PLAY MODE VERIFY` | The next scene/layout changes only under a fully opaque dark hold; no respawn overlay/label or visible player teleport appears between Move and Jump or any later lessons. |
| `FL-TUT-STALE-LESSON-ACTORS` | Progression safety | `FIXED IN CODE / FULL-RUN VERIFY` | Completed-screen enemies, projectiles, hazards and transactions cannot attack, collide, kill or block the player while travelling to the next screen. |
| `FL-TUT-DUPLICATE-TRAVEL-OWNER` | Lesson ownership | `FIXED IN CODE / FULL-RUN VERIFY` | The legacy station-travel gate remains inactive once lesson screens initialize and never emits duplicate arrival feedback or hides the active card. |
| `FL-TUT-BINDING-COPY-MISMATCH` | Input/UI | `FIXED IN CODE / INPUT-MATRIX VERIFY` | Every card states the same keyboard, mouse, gamepad and physical action accepted by the active reader. |
| `FL-TUT-Q-HOLD-MISROUTED` | Charged/ranged input | `FIXED IN CODE / PLAY MODE VERIFY` | Q/RB/physical A own ranged hold; left mouse remains Light/Spin and never starts a charged ranged transaction. |
| `FL-CIN-MOTHER-TAIL-DOWN` | Opening dialogue | `FIXED IN CODE / VISUAL VERIFY` | Bubble stays at `(72,-108)` and the complete pointer visibly exits the left edge of the body, never downward. |
| `FL-RENDER-MEMORYLESS-DEPTH` | Handheld screen rendering | `MITIGATION IMPLEMENTED / UNITY-METAL VERIFY` | Project-owned screen RT has no depth/stencil and no memoryless surface; both `Ignoring depth surface ... as it is memoryless` messages are absent in a fresh Editor run. |
<!-- BND_TUTORIAL_RUNTIME_INTEGRITY_V1011319:END -->

<!-- BND_DIALOGUE_SCOPE_COMPILE_REPAIR_BUG_V1011315 -->
## Resolved locally in V10.11.30.15

- `SetChildApproachDialogueImmediate` compiled outside the field-owning presenter scope, producing CS0103 errors for the dialogue canvas, visual rect and rest position.
- The file is now rebuilt from the tracked canonical structure before applying the single approved layout delta.

<!-- BND_TUTORIAL_FLOW_JUMP_CINEMATIC_BUG_V1011314 -->
## Resolved locally in V10.11.30.14

- Empty instruction frame remained after lesson completion.
- The between-screen state looked frozen because its travel instruction expired.
- Jump could be rejected by an unrelated generic action lock after its lesson unlocked.
- The mother bubble sat too high.
- The child-camera height correction needed an explicit full-strength walk/climb guarantee.

<!-- BND_TUTORIAL_FINAL_QA_ZIP_CLEANUP_V1011310:BEGIN -->
## Resolved locally; Unity verification pending

- One validator still looked for HandleFirstLaunchTutorialMeleeLessonDeathAtImpact in Gameplay.cs instead of LessonScreens.cs.
- Delivery ZIPs remained in Downloads after installers completed or failed.
- The validator path and unconditional ZIP cleanup are now corrected.
<!-- BND_TUTORIAL_FINAL_QA_ZIP_CLEANUP_V1011310:END -->

<!-- BND_TUTORIAL_QA_SEMANTIC_CAMERA_HEIGHT_V1011309:BEGIN -->
## Resolved locally; Unity verification pending

- TEST EVERYTHING reported nine false blockers from superseded tutorial implementation tokens.
- Child approach/menu POV sat slightly lower than requested.
- Validators now inspect the active semantic owners and the child camera receives a small blended height correction.
<!-- BND_TUTORIAL_QA_SEMANTIC_CAMERA_HEIGHT_V1011309:END -->

<!-- BND_V1011307_BUGS -->
## Closed by V10.11.30.7

- `LessonScreens.cs` ignored because its `.meta` contained literal `\n`
  characters instead of YAML line breaks.
- `BDTutorialLessonScreensInputParryV1011306QA.cs` ignored for the same reason.
- `ReadFirstLaunchTutorialConfirmPressed` could not call the E / interact
  reader after that reader was converted to an instance method.
- Retained tutorial unlock and impact-proof fields produced assigned-but-unused
  compiler warnings.

<!-- BND_TUTORIAL_LESSON_SCREENS_INPUT_PARRY_V1011306:BEGIN -->
## Resolved locally; Unity verification pending

- Immediate next-tutorial display on the same screen.
- Mechanics unlocking only near the target instead of at screen entry.
- Left mouse incorrectly routed to Ranged.
- Parry target acting as a transparent wall.
- Parry not resolving through the canonical Light/Heavy inputs.
- Parry enemy retaining a squashed action scale.
- Attack lessons advancing without target death.
- Dodge lesson advancing without crossing the hazard.
<!-- BND_TUTORIAL_LESSON_SCREENS_INPUT_PARRY_V1011306:END -->

<!-- BND_TUTORIAL_INPUT_MECHANICS_MOUNTED_IMPACT_V1011305:BEGIN -->
## V10.11.30.5 focused defects

| ID | Area | Status | Acceptance condition |
|---|---|---|---|
| FL-TUT-WASD-REMOVED | Keyboard movement | FIXED IN CODE / PLAY MODE VERIFY | WASD and Arrow keys work in parallel under both Unity input backends. |
| FL-TUT-E-Q-STEP-LOCKED | Interact/ranged input | FIXED IN CODE / PLAY MODE VERIFY | E and Q are routed regardless of the highlighted lesson; healing remains contextual near the injured horse. |
| FL-TUT-MECHANICS-LESSON-LOCKED | Combat mechanics | FIXED IN CODE / PLAY MODE VERIFY | Light, Heavy, Spin, Grapple, Dodge, Parry and ordinary ranged execution are not disabled merely because another lesson is highlighted. |
| FL-TUT-MOUNTED-IMPACT-INVISIBLE-WALL | Horse ram | FIXED IN CODE / PLAY MODE VERIFY | Horse contact reaches and defeats the target without a transparent collision wall or hard lock. |
<!-- BND_TUTORIAL_INPUT_MECHANICS_MOUNTED_IMPACT_V1011305:END -->

<!-- BND_TUTORIAL_PLAYER_CANONICAL_ASSET_NAME_V1011304:BEGIN -->
## V10.11.30.4 — duplicated canonical player-name QA blockers

| ID | Area | Status | Acceptance condition |
|---|---|---|---|
| FL-TUT-PLAYER-ASSET-NAME-SPLIT | QA/runtime contract | FIXED IN PACKAGE / UNITY VERIFY | The real visible idle sprite and all maintained validators use `B&D Tutorial Player Simple Right Facing Sprite`; no obsolete `... Idle` contract remains and `TEST EVERYTHING` returns `0/0/0`. |
<!-- BND_TUTORIAL_PLAYER_CANONICAL_ASSET_NAME_V1011304:END -->

<!-- BND_TUTORIAL_PLAYER_VISIBILITY_RUNTIME_V1011303:BEGIN -->
## V10.11.30.3 focused defect

| ID | Area | Status | Acceptance condition |
|---|---|---|---|
| FL-TUT-PLAYER-INVISIBLE-CHILD-DISABLED | Tutorial player rendering | FIXED IN CODE / PLAY MODE VERIFY | The authoritative `Tutorial Player Pixel Visual` child stays active, displays the simple colored player, retains walk/action frames and inherits left/right facing from the player parent. |
<!-- BND_TUTORIAL_PLAYER_VISIBILITY_RUNTIME_V1011303:END -->

<!-- BND_TUTORIAL_QA_CONTRACT_REALIGNMENT_V1011302:BEGIN -->
## V10.11.30.2 focused QA defect

| ID | Area | Status | Acceptance condition |
|---|---|---|---|
| FL-TUT-STALE-PLAYER-QA-V101111-V101117 | Automated QA | FIXED IN PATCH / UNITY VERIFY | Legacy validators no longer demand retired player dimensions, exact palette literals or obsolete sprite markers; they validate the current simple right-facing sprite contract while preserving unrelated typography checks. |
<!-- BND_TUTORIAL_QA_CONTRACT_REALIGNMENT_V1011302:END -->

<!-- BND_TUTORIAL_FINAL_INPUT_COMBAT_PLAYER_V1011301:BEGIN -->
## V10.11.30.1 focused defects

| ID | Area | Status | Acceptance condition |
|---|---|---|---|
| FL-TUT-RIGHTARROWOWNARROW | Compilation/input | FIXED IN PATCH / UNITY VERIFY | No malformed `KeyCode` member remains; Arrow movement/entry compile and work. |
| FL-TUT-FIRST-ENEMY-IMMORTAL | First melee lesson | FIXED IN PATCH / PLAY MODE VERIFY | The registered actor receives lethal damage at visible Light impact; misses do not hide or advance it. |
| FL-TUT-HEAVY-BYPASSES-DAMAGE | Heavy lesson | FIXED IN PATCH / PLAY MODE VERIFY | Heavy uses the same authoritative melee transaction and advances only after confirmed lethal impact. |
| FL-TUT-PLAYER-REVERSED-CLUTTERED | Player visual | FIXED IN PATCH / VISUAL VERIFY | One compact side-profile sprite is visible; positive X faces right and runtime flipping follows movement. |
| FL-TUT-SPIN-NONATOMIC | Spin lesson | FIXED IN PATCH / PLAY MODE VERIFY | Both opposite-side targets must be in the same spin; partial coverage kills neither and does not advance. |
| FL-TUT-DODGE-BUTTON-ONLY | Dodge lesson | FIXED IN PATCH / PLAY MODE VERIFY | Dodge completes only after crossing to the other side of the obstacle. |
<!-- BND_TUTORIAL_FINAL_INPUT_COMBAT_PLAYER_V1011301:END -->

## TUTORIAL_ATOMIC_LETHALITY_PALETTE_PLAYER_MODEL_V101128 — FIXED
- Removed delayed/unsynchronized first-enemy death, flat monochrome tutorial text, partial spin kills and input-only dodge completion.
- Installer uses method boundaries and verifies installed method bodies before PASS.

## Resolved — tutorial targets surviving the correct lesson hit

The ownership gate previously allowed the correct source but retained generic combat damage, so a two-health target could survive a one-damage quick attack. Focused lesson hits now consume the target's remaining health; grapple is the intentional exception until its follow-up kill.

<!-- BD ALL LESSON TARGETS LETHAL V10.11.26 -->

# BD TUTORIAL LESSON ENTRY + DAMAGE OWNERSHIP V10.11.25

Resolved: crowded opening obstacle, late/disappearing mounted-shot guidance, completion on wrong damage sources, stale MountedImpact wall, reveal _MainTex warning and handheld UI depth warnings.

## BD V10.11.24 MOUNTED RANGED SEQUENCE AND NO MOUNTED DODGE
- Fixed: reload could finish before projectile impact, causing Reload or ChargedShot to wait forever.
- Fixed: charged auto-fire could remain latched after a miss.
- Fixed: directional double-tap could grant dodge invulnerability while riding.
- Pending local proof: Unity compilation, TEST EVERYTHING and the complete mounted tutorial sequence.

## BD V10.11.23 LESSON PERSISTENCE AND PROGRESSION GATE
- Fixed: later tutorial stations could be passed before completing the current lesson, leaving required state behind and hard-locking progression.
- Fixed: tutorial instruction text could disappear because proximity/time presentation logic released it before success.
- Pending local proof: Unity compilation, TEST EVERYTHING and a complete Play Mode tutorial run.

## BD V10.11.22.2 QA CONTRACT RECONCILIATION
- Resolved: five false blockers caused by conflicting power timing and hidden dual-binding contracts.
- Pending local proof: Unity compilation, TEST EVERYTHING 0/0/0 and focused Play Mode verification.

<!-- BND_TUTORIAL_INPUT_PARITY_POWER_REVEAL_V101122:BEGIN -->
## V10.11.22 — tutorial parity, binding presentation and screen power reveal

| ID | Status | Acceptance condition |
|---|---|---|
| `TUTORIAL-ALL-INPUT-PARITY-V101122` | `FIXED IN CODE / PLAY MODE VERIFY` | Every lesson accepts its documented keyboard/gamepad input, the matching physical handheld control and contextual mouse input on the real display. |
| `TUTORIAL-HEAVY-MOUSE-V101122` | `FIXED IN CODE / PLAY MODE VERIFY` | Heavy attack responds to `K`, gamepad north, physical `Y`, right mouse on the display and the contextual lesson click. |
| `TUTORIAL-BINDING-CARD-V101122` | `FIXED IN CODE / VISUAL VERIFY` | One centered professional card remains inside the screen with no overlap, clipping or loose labels. |
| `HANDHELD-POWER-REVEAL-V101122` | `FIXED IN CODE / VISUAL VERIFY` | Power-on begins immediately after camera settlement, preserves its duration, and the moving line reveals content rather than decorating an already-visible screen. |
<!-- BND_TUTORIAL_INPUT_PARITY_POWER_REVEAL_V101122:END -->

<!-- BND_SMOOTH_DRIP_TUTORIAL_MOUSE_V101121:BEGIN -->
## V10.11.21 — visible drip and ordinary mouse attack regressions

| ID | Status | Acceptance condition |
|---|---|---|
| `OPENING-DRIP-QUALITY-V101121` | `FIXED IN CODE / VISUAL VERIFY` | The unchanged BBH frame melts downward through a continuous antialiased liquid edge; no 32-strip, shutter, stair-step or pixelated silhouette is visible. |
| `TUTORIAL-MOUSE-LIGHT-ATTACK-V101121` | `FIXED IN CODE / PLAY MODE VERIFY` | During `AttackEnemy`, one desktop left click on the real physical display produces exactly one ordinary attack and uses the existing visible impact transaction. |
| `OPENING-DRIP-SCOPE-V101121` | `GUARDED` | The drip pass does not change BBH text, colors, logo movement, camera, room, dialogue, device, tutorial layout or gameplay state. |
<!-- BND_SMOOTH_DRIP_TUTORIAL_MOUSE_V101121:END -->

<!-- BND_TUTORIAL_QA_CONTRACT_RECOVERY_V1011203:BEGIN -->
## V10.11.20.3 — tutorial QA contract blockers

| ID | Status | Acceptance condition |
|---|---|---|
| `TUTORIAL_V101114_WORLD_MOUSE_ATTACK_MISSING` | `FIXED IN CODE / PLAY MODE VERIFY` | A left click inside the tutorial world reaches the light-attack action before handheld screen controls can consume the frame. |
| `TUTORIAL_V101117_INDIE_BINDING_VISUALS_MISSING` | `CONTRACT RESTORED / VISUAL VERIFY` | Keyboard/mouse keycaps retain the indie-card implementation and the exact `BD INDIE INPUT KEYCAPS V10.11.17` contract. |
| `TUTORIAL_V101117_PHYSICAL_HANDHELD_MISSING` | `CONTRACT RESTORED / VISUAL VERIFY` | The physical side retains its illustrated control and exact `PHYSICAL HANDHELD` contract. |
| `TUTORIAL_V101117_MOUNT_HANDOFF_MISSING` | `FIXED IN CODE / PLAY MODE VERIFY` | Mount guidance persists through the real mount animation and advances only from `MountHorse` to `RideHorse` on successful completion. |
<!-- BND_TUTORIAL_QA_CONTRACT_RECOVERY_V1011203:END -->

<!-- BND_TUTORIAL_INDIE_BINDING_VISUALS_HOTFIX_V101118:BEGIN -->
## V10.11.18 resolved automated blocker

- Fixed `TUTORIAL_V101117_INDIE_BINDING_VISUALS_MISSING`: the illustrated binding source now contains and applies the exact `PHYSICAL HANDHELD` title required by the QA contract.
<!-- BND_TUTORIAL_INDIE_BINDING_VISUALS_HOTFIX_V101118:END -->

<!-- BND_OPENING_TUTORIAL_RECOVERY_V101117:BEGIN -->
## V10.11.17 fixes awaiting local acceptance

- BBH exit effect previously leaked into the child-camera shot instead of moving the intro layer itself.
- The room fade could be re-armed after the drip and create a black/left-edge glitch.
- Jump landing could miss the exact threshold and never expose the horse-mount lesson.
- The player could fall back to a generic visual instead of the requested blond/red/blue art.
- Mother dialogue disappeared before the child began walking.
- Physical handheld bindings were shown as plain text rather than illustrated controls.
- The supplied application icon was not assigned through current Unity 6 APIs.
<!-- BND_OPENING_TUTORIAL_RECOVERY_V101117:END -->

<!-- BND_TUTORIAL_DRIP_CONTRACT_BINDING_HOTFIX_V101116:BEGIN -->
## V10.11.16 focused defects

| ID | Status | Acceptance |
|---|---|---|
| FL-INTRO-DRIP-HANDOFF-CONTRACT | FIXED IN CODE / VISUAL VERIFY | BBH artwork visibly drips while the kitchen is behind it; no drip stripe appears after the intro disappears. |
| FL-TUT-PHYSICAL-CARD-QA-CONTRACT | FIXED IN CODE / VISUAL VERIFY | Keyboard/controller and physical handheld cards are both visible for every actionable lesson. |
| FL-QA-V101114-STALE-DRIP-ASSERTION | FIXED IN CODE / AUTOMATED VERIFY | QA no longer requires the obsolete always-transparent child-scene fade. |
<!-- BND_TUTORIAL_DRIP_CONTRACT_BINDING_HOTFIX_V101116:END -->

<!-- BND_TUTORIAL_QA_COMPILATION_HOTFIX_V101115:BEGIN -->
## V10.11.15 focused compiler defect

| ID | Status | Acceptance |
|---|---|---|
| FL-QA-MULTILINE-STRING-COMPILE | FIXED IN CODE / UNITY VERIFY | `BDTutorialOpeningPolishV1011QA.cs` compiles with no CS1010, CS1003 or CS1026 errors, then `TEST EVERYTHING` reaches runtime QA. |
<!-- BND_TUTORIAL_QA_COMPILATION_HOTFIX_V101115:END -->

<!-- BND_TUTORIAL_DRIP_MOUNT_INPUT_BINDINGS_V101114:BEGIN -->
## V10.11.14 focused defects

| ID | Status | Acceptance |
|---|---|---|
| FL-INTRO-DRIP-WRONG-LAYER | FIXED IN CODE / VISUAL VERIFY | BBH artwork itself drips away and no strip/fade glitch appears when the room animation starts. |
| FL-TUT-MOUNT-PROMPT-LATE | FIXED IN CODE / PLAY MODE VERIFY | Mount instruction appears immediately beside the horse after the first jump. |
| FL-TUT-WORLD-LMB-NO-ATTACK | FIXED IN CODE / PLAY MODE VERIFY | Left mouse click performs light attack in the game screen and physical X remains a single equivalent action. |
| FL-TUT-PHYSICAL-BINDING-HIDDEN | FIXED IN CODE / VISUAL VERIFY | Every binding lesson shows keyboard/controller and physical handheld controls together. |
<!-- BND_TUTORIAL_DRIP_MOUNT_INPUT_BINDINGS_V101114:END -->

<!-- BND_TUTORIAL_TRIGGER_UNLOCK_HUD_COLLISION_HOTFIX_V101113:BEGIN -->
## V10.11.13 focused defects

| ID | Status | Acceptance |
|---|---|---|
| FL-TUT-PROMPT-LIFECYCLE | FIXED IN CODE / PLAY MODE VERIFY | Every lesson prompt appears when its lesson starts and disappears when it completes or travel begins. |
| FL-TUT-MOVE-JUMP-ORDER | FIXED IN CODE / PLAY MODE VERIFY | Player starts before the obstacle; Move completes before contact; Jump remains locked until its lesson. |
| FL-TUT-PREMATURE-ABILITIES | FIXED IN CODE / PLAY MODE VERIFY | Jump, interact, light, heavy, dodge, parry and ranged input are unavailable before their lessons. |
| FL-TUT-ENEMY-PHASE-THROUGH | FIXED IN CODE / PLAY MODE VERIFY | Living visible enemies block horizontal traversal; dead/hidden enemies do not. |
| FL-TUT-HORSE-PET-HUD-LEAK | FIXED IN CODE / PLAY MODE VERIFY | No full-game horse prompt is drawn outside the Game Boy during the tutorial. |
| FL-TUT-CHILD-BUBBLE-REMOVAL | FIXED IN CODE / VISUAL VERIFY | No child reply bubble or child voice is created. |
| FL-TUT-LEFT-DRIP-GLITCH | FIXED IN CODE / VISUAL VERIFY | BBH drip begins as a coherent center-out wipe with no isolated left-edge strip. |
<!-- BND_TUTORIAL_TRIGGER_UNLOCK_HUD_COLLISION_HOTFIX_V101113:END -->

<!-- BND_TUTORIAL_QA_DIALOGUE_HOTFIX_V101112:BEGIN -->
## V10.11.12 focused QA defects

| ID | Status | Acceptance |
|---|---|---|
| CHILD_APPROACH_CINEMATIC_CONTRACT_INVALID | FIXED IN QA CONTRACT / RERUN REQUIRED | Validator expects V10.11.11 two-speaker timings and drip handoff tokens. |
| V23R19Q_BOOT_POLISH_MISSING | FIXED IN QA CONTRACT / RERUN REQUIRED | Validator accepts the current `DrawBootLayer`/`DrawDrippingBootLayer` structure and `DrawComposition(alpha)`. |
| OPENING-MOTHER-LINE-TYPO | FIXED IN CODE / VISUAL VERIFY | Bubble reads exactly `Sweety, where are you?`. |
<!-- BND_TUTORIAL_QA_DIALOGUE_HOTFIX_V101112:END -->

<!-- BND_TUTORIAL_FINAL_PRODUCTION_COURSE_V101111:BEGIN -->
## V10.11.11 focused defects

| ID | Status | Acceptance |
|---|---|---|
| FL-TUT-COURSE-SAME-LOCATION | FIXED IN CODE / PLAY MODE VERIFY | Major lessons occupy separate forward stations with travel between them. |
| FL-TUT-VISIBLE-SPAWN-POP | FIXED IN CODE / PLAY MODE VERIFY | New actors spawn beyond the visible right edge; wall/platform geometry exists before approach. |
| FL-TUT-VISIBLE-OBSTACLE-DESPAWN | FIXED IN CODE / PLAY MODE VERIFY | Persistent wall, platforms and finish gate never disappear in view; the gate opens by moving. |
| FL-TUT-BACKTRACK | FIXED IN CODE / PLAY MODE VERIFY | Camera and progression floor advance only forward. |
| FL-TUT-BOSS-PATTERN | FIXED IN CODE / PLAY MODE VERIFY | Boss uses three-shot vertical fan at range and telegraphed slash/jump-slam at close range. |
| FL-TUT-HIT-FEEDBACK | FIXED IN CODE / VISUAL VERIFY | Player/enemies flicker and jitter briefly on real damage. |
| FL-TUT-PLAYER-PALETTE | FIXED IN CODE / VISUAL VERIFY | Blond/yellow hair, red shirt and blue trousers are unmistakable. |
| FL-TUT-ATTACK-VISUAL-CONTACT | FIXED IN CODE / PLAY MODE VERIFY | Ground slash is horizontal; airborne slash travels top-to-bottom; damage remains impact-owned. |
| FL-TUT-VIRTUAL-PHYSICAL-INPUT | FIXED IN CODE / PLAY MODE VERIFY | Keyboard/mouse virtual actions pulse the corresponding physical handheld control. |
| FL-TUT-OPENING-DIALOGUE-SEQUENCE | FIXED IN CODE / AUDIO-VISUAL VERIFY | Off-screen mother bubble, child `רק שניה` reply, then walking. |
| FL-TUT-INTRO-DRIP | FIXED IN CODE / VISUAL VERIFY | BBH screen drips rapidly to reveal the already-prepared room. |
| FL-TUT-RELIC-HANDOFF | FIXED IN CODE / PLAY MODE VERIFY | Player lifts relic overhead; light grows from it and fades to main menu. |
<!-- BND_TUTORIAL_FINAL_PRODUCTION_COURSE_V101111:END -->

<!-- BND_TUTORIAL_PLAYER_TEXT_BOSS_ENVIRONMENT_V101110:BEGIN -->
## V10.11.10 focused defects

| ID | Area | Status | Acceptance condition |
|---|---|---|---|
| FL-TUT-CS0114-LETTER-EFFECT | Compilation | FIXED IN CODE / UNITY VERIFY | `BDTutorialLetterPulseEffect.OnEnable()` overrides `BaseMeshEffect.OnEnable()` and calls `base.OnEnable()`; no CS0114 warning remains. |
| FL-TUT-PLAYER-GENERIC | Player visual | FIXED IN CODE / VISUAL VERIFY | Player reads as a side-profile child with natural skin, blond hair, red shirt, blue trousers and correctly placed eye. |
| FL-TUT-START-TOO-CLOSE | Tutorial start | FIXED IN CODE / PLAY MODE VERIFY | Initial player X is `-900`, leaving readable approach distance before the first obstacle. |
| FL-TUT-TEXT-PARTIAL-POLISH | Tutorial UI | FIXED IN CODE / VISUAL VERIFY | Prompt, detail, progress, feedback, both binding titles and values, divider, health, ammo and boss text are larger, outlined, colored and animated without clipping. |
| FL-TUT-BOSS-ORDINARY-SHOT | Mini-boss | FIXED IN CODE / PLAY MODE VERIFY | Ordinary ranged input selects the living boss explicitly and resolves one point of damage at visible projectile impact, including during boss windup. |
| FL-TUT-COLLECTIBLE-WRONG-INSTRUCTION | Completion | FIXED IN CODE / PLAY MODE VERIFY | Final prompt says to walk into the relic; interact is not presented as required and contact completes collection. |
| FL-TUT-COLLECTIBLE-BACKGROUND-LIKE | Completion visual | FIXED IN CODE / VISUAL VERIFY | Green relic uses a bright outlined gem sprite with highlight and pulse, clearly separated from background decoration. |
| FL-TUT-ENVIRONMENT-NO-FEEDBACK | Environment lesson | FIXED IN CODE / PLAY MODE VERIFY | Attack visibly knocks the enemy into the hazard, shows impact, then advances only after the complete sequence. |
<!-- BND_TUTORIAL_PLAYER_TEXT_BOSS_ENVIRONMENT_V101110:END -->

<!-- BND_TUTORIAL_WALLJUMP_BOSS_TYPOGRAPHY_DIALOGUE_V10118:BEGIN -->
## V10.11.8 focused defects

| ID | Area | Status | Acceptance condition |
|---|---|---|---|
| FL-TUT-WALL-JUMP-INFINITE | Wall jump | FIXED IN CODE / PLAY MODE VERIFY | A wall contact grants at most one wall jump until the player lands on ground, platform or upper ground. |
| FL-TUT-WALL-JUMP-UPPER-UNREACHABLE | Wall jump route | FIXED IN CODE / PLAY MODE VERIFY | The player can jump right from the platform, clear the wall and land on the upper ground without a returning invisible clamp. |
| FL-TUT-WALL-JUMP-SUNK | Character grounding | FIXED IN CODE / VISUAL VERIFY | Player baseline sits above the platform and upper-ground surfaces instead of sinking into them. |
| FL-CIN-DIALOGUE-TAIL-SEPARATE | Opening dialogue | FIXED IN CODE / VISUAL VERIFY | Body, tail, shadow and seam animate under one parent; no intersecting black outlines or split disappearance. |
| FL-TUT-TYPOGRAPHY-UNDERSIZED | Tutorial UI | FIXED IN CODE / VISUAL VERIFY | Headline, detail, bindings, feedback and HUD are materially larger, high-contrast, step-colored and animated. |
| FL-TUT-BOSS-OLD-VISUAL | Mini-boss | FIXED IN CODE / PLAY MODE VERIFY | Slam uses a ground warning zone; projectile uses a compact charge orb and visible projectile release, not the old generic beam. |
| FL-TUT-BOSS-NO-SHOTS | Mini-boss | FIXED IN CODE / PLAY MODE VERIFY | Phase one shoots on a predictable sequence and phase two opens with and repeatedly alternates projectile attacks. |
| FL-TUT-BOSS-CHARGE-BROKEN | Mini-boss/player combat | FIXED IN CODE / PLAY MODE VERIFY | Holding ranged charges during either boss phase and auto-fires at full charge on a valid recovery frame. |
<!-- BND_TUTORIAL_WALLJUMP_BOSS_TYPOGRAPHY_DIALOGUE_V10118:END -->

<!-- BND_TUTORIAL_COMPLETION_INTEGRITY_V10117:BEGIN -->
## V10.11.7 tutorial completion integrity defects

| ID | Area | Status | Acceptance condition |
|---|---|---|---|
| FL-CIN-BUBBLE-TAIL-LAYER | Opening dialogue | FIXED IN CODE / VISUAL VERIFY | The diamond is drawn behind the bubble body, stays attached and follows the same entrance/exit transform. |
| FL-TUT-HORSE-REVERSE-FACING | Horse presentation | FIXED IN CODE / PLAY MODE VERIFY | Left/backward travel flips the horse and rider presentation; right/forward travel restores forward facing. |
| FL-TUT-MOUNTED-SHOT-DOUBLE-KILL | Mounted shooting | FIXED IN CODE / PLAY MODE VERIFY | The lesson spawns one target, the projectile moves continuously, and only the locked living target is damaged at visible impact. |
| FL-TUT-INVISIBLE-PROGRESSION-WALL | Course traversal | FIXED IN CODE / FULL-RUN VERIFY | Lesson limits provide guidance only and hidden actors never block movement; the complete tutorial can reach the collectible and completion state. |
<!-- BND_TUTORIAL_COMPLETION_INTEGRITY_V10117:END -->

<!-- BND_TUTORIAL_OPENING_POLISH_V10113:BEGIN -->
## V10.11.3 — opening dialogue and focused tutorial defects

| ID | Status | Acceptance condition |
|---|---|---|
| `OPENING-V10113-001` | `IMPLEMENTED / UNITY VERIFICATION REQUIRED` | The mother bubble appears only after the room is visible, uses fade+scale in, holds, reverses out and is fully gone before walking. |
| `OPENING-V10113-002` | `IMPLEMENTED / UNITY VERIFICATION REQUIRED` | The exact text is `honey come here a second`; the cue is feminine, nonverbal, deterministic and non-spatial. |
| `TUTORIAL-V10113-003` | `IMPLEMENTED / PLAY MODE REQUIRED` | Leaving either real wall-jump support releases grounded state and produces a fall; no hovering remains. |
| `TUTORIAL-V10113-004` | `IMPLEMENTED / PLAY MODE REQUIRED` | Jump Attack visibly states Jump + Attack with keyboard/mouse and handheld combinations. |
| `TUTORIAL-V10113-005` | `PRESERVED / PLAY MODE REQUIRED` | Mounted fire damages exactly its selected living target only at visible projectile impact. |
| `TUTORIAL-V10113-006` | `IMPLEMENTED / PLAY MODE REQUIRED` | The existing local Mini-Boss slam presentation is preserved and every third phase-two attack becomes a readable ranged shot. |
| `TUTORIAL-V10113-007` | `IMPLEMENTED / VISUAL VERIFICATION REQUIRED` | Colored course decorations remain behind actors and the larger per-letter animation remains readable. |
<!-- BND_TUTORIAL_OPENING_POLISH_V10113:END -->

<!-- BND_CHAIR_BACKREST_AND_SCREEN_DELAY_V10933:BEGIN -->
## V10.9.33 — chair backrest gaps and premature screen ignition

| ID | Area | Status | Acceptance condition |
|---|---|---|---|
| `CHAIR-V10933-001` | Backrest slats | `IMPLEMENTED / VISUAL VERIFICATION REQUIRED` | Every vertical slat reaches the top of the lower horizontal rail without a floating gap. |
| `CHAIR-V10933-002` | Rail-to-seat spacing | `IMPLEMENTED / VISUAL VERIFICATION REQUIRED` | The lower back rail is visibly closer to the seat while remaining separate. |
| `SCREEN-V10933-003` | Power-on timing | `IMPLEMENTED / VISUAL VERIFICATION REQUIRED` | Camera settlement completes, a short dark-screen beat remains, then the existing power-on effect starts. |
<!-- BND_CHAIR_BACKREST_AND_SCREEN_DELAY_V10933:END -->

<!-- BND_CHILD_APPROACH_ROOM_SHELL_QA_AGGREGATION_REPAIR_V10932:BEGIN -->
## V10.9.32 — room-shell tokens omitted from child cinematic QA aggregation

| ID | Status | Acceptance condition |
|---|---|---|
| `CHILD-CINEMATIC-V10932-001` | `FIX IMPLEMENTED / UNITY VERIFICATION REQUIRED` | `BDChildApproachCinematicQA` requires and reads `BDModernHandheld3DPresenter.CinematicWallpaperBackWall.cs`. |
| `CHILD-CINEMATIC-V10932-002` | `REGRESSION GUARDED` | The three V10.9.31 room-shell tokens pass without changing Runtime implementation files. |
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
## V10.9.29 visual path issues

| ID | Status | Acceptance |
|---|---|---|
| `CINEMATIC-V10929-001` | `IMPLEMENTED / VISUAL VERIFICATION REQUIRED` | Opening frame is clearly farther behind and farther left of the chair, at the accepted child POV height. |
| `CINEMATIC-V10929-002` | `IMPLEMENTED / VISUAL VERIFICATION REQUIRED` | The child approaches the chair through a longer six-step walk rather than beginning next to it. |
| `CINEMATIC-V10929-003` | `IMPLEMENTED / VISUAL VERIFICATION REQUIRED` | The climb routes around the left side and over the seat without crossing the backrest, rails, legs, or seat volume. |
| `CINEMATIC-V10929-004` | `PRESERVED` | V10.9.28 screen power-on and delayed tutorial-content reveal remain unchanged. |
<!-- BND_CHILD_APPROACH_CINEMATIC_PATH_CLEARANCE_V10929:END -->

<!-- BND_CHILD_APPROACH_CINEMATIC_POLISH_V10928:BEGIN -->
## V10.9.28 visual polish issues

| ID | Status | Acceptance |
|---|---|---|
| `CINEMATIC-V10928-001` | `IMPLEMENTED / VISUAL VERIFICATION REQUIRED` | Shot begins behind the correctly oriented chair at the raised prior-POV height and advances toward it. |
| `CINEMATIC-V10928-002` | `IMPLEMENTED / VISUAL VERIFICATION REQUIRED` | Walk and climb read as grounded, restrained and cinematic rather than low, floating or mechanical. |
| `CINEMATIC-V10928-003` | `IMPLEMENTED / VISUAL VERIFICATION REQUIRED` | Screen glass/backlight wake first; tutorial content then feeds in with fade, small rise and scale settle. |
| `CINEMATIC-V10928-004` | `REPAIRED` | Unused `introToMainMenuStartPosePrimed` field and CS0414 warning are removed. |
<!-- BND_CHILD_APPROACH_CINEMATIC_POLISH_V10928:END -->

<!-- BND_CHILD_APPROACH_CINEMATIC_V10927:BEGIN -->
## V10.9.27 cinematic and compile issues

| ID | Status | Acceptance |
|---|---|---|
| `CINEMATIC-V10927-001` | `IMPLEMENTED / UNITY VISUAL VERIFICATION REQUIRED` | Camera starts behind the chair, walks, climbs and settles in one continuous child-POV shot. |
| `CINEMATIC-V10927-002` | `IMPLEMENTED / UNITY VISUAL VERIFICATION REQUIRED` | The handheld display stays physically off with no white frame until the final power-on. |
| `CINEMATIC-V10927-003` | `IMPLEMENTED / UNITY VISUAL VERIFICATION REQUIRED` | Skip produces the exact final camera, powered screen and ready tutorial state. |
| `QA-V10927-001` | `REPAIRED / UNITY COMPILE REQUIRED` | `BDFirstLaunchTutorialQA.Scan(BDOneClickQAResult result)` matches the one-argument caller. |
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
## V10.9.25 — environment and final-framing visual findings

| ID | Status | Acceptance condition |
|---|---|---|
| `CINEMATIC-V10925-001` | `IMPLEMENTED / VISUAL VERIFICATION REQUIRED` | No curved ramp/cyclorama or black-limbo stage is visible. |
| `CINEMATIC-V10925-002` | `IMPLEMENTED / VISUAL VERIFICATION REQUIRED` | The shot reads as one real room with a warm floor and a distant wallpapered back wall; side walls remain outside camera bounds. |
| `CINEMATIC-V10925-003` | `IMPLEMENTED / VISUAL VERIFICATION REQUIRED` | Final camera is slightly closer while the whole handheld and small lower wood margin remain visible. |
| `CINEMATIC-V10925-004` | `IMPLEMENTED / VISUAL VERIFICATION REQUIRED` | Room and wallpaper are properly illuminated instead of appearing nearly black. |
<!-- BND_POST_INTRO_REAL_ROOM_AND_CLOSER_FRAMING_V10925:END -->

<!-- BND_POST_INTRO_FINAL_FIRST_LAUNCH_QA_REPAIR_V10924:BEGIN -->
## V10.9.24 — final stale first-launch camera contract

| ID | Status | Acceptance condition |
|---|---|---|
| `CINEMATIC-V10924-001` | `FIX IMPLEMENTED / UNITY VERIFICATION REQUIRED` | The named first-launch camera contract contains exactly one final-look target, identical to the authoritative handheld QA Require block and present in Runtime. |
| `CINEMATIC-V10924-002` | `REGRESSION GUARDED` | No other final-look vector remains in that named contract. |
| `CINEMATIC-V10924-003` | `IMPLEMENTATION PROTECTED` | Runtime, wallpaper, room, DOF, camera, device, table, shader, and `BDModernHandheld3DQA` remain byte-identical. |
<!-- BND_POST_INTRO_FINAL_FIRST_LAUNCH_QA_REPAIR_V10924:END -->

Resolved authoritative final-look target: `new Vector3(0f, -7.18f, -4.18f)`.

<!-- BND_POST_INTRO_CINEMATIC_WALLPAPER_FOCUS_DELIVERY_REPAIR_V10916:BEGIN -->
## Post-intro cinematic V10.9.16 — framing, blur and wallpaper pass

| ID | Area | Status | Acceptance condition |
|---|---|---|---|
| `CINEMATIC-V10916-001` | Blur quality | `IMPLEMENTED / VISUAL VERIFICATION REQUIRED` | DOF is substantially cleaner and subtler; the screen and shell remain crisp while the background softens gently. |
| `CINEMATIC-V10916-002` | Final framing | `IMPLEMENTED / VISUAL VERIFICATION REQUIRED` | The entire handheld is visible in the final frame with a small visible tabletop strip below it. |
| `CINEMATIC-V10916-003` | Wallpaper art direction | `IMPLEMENTED / VISUAL VERIFICATION REQUIRED` | The room includes patterned wallpaper reminiscent of stylized kitchen sets in animated films/series, without competing with the device. |
| `CINEMATIC-V10916-004` | Regression safety | `PACKAGE VERIFIED / UNITY REQUIRED` | V10.9.16 preserves one camera, no scale-cheat, stable device placement, and no tutorial regression. |
<!-- BND_POST_INTRO_CINEMATIC_WALLPAPER_FOCUS_DELIVERY_REPAIR_V10916:END -->

| `CINEMATIC-V10916-005` | Installer delivery | `RESOLVED` | QA numeric contracts are inserted into actual existing anchors; failed V10.9.15 state was rolled back before this package. |

<!-- BND_POST_INTRO_CINEMATIC_QA_LATEST_COMMIT_ALIGNMENT_V1094:BEGIN -->
## V10.9.4 latest-commit-aligned QA defect

| ID | Area | Status | Current truth / acceptance condition |
|---|---|---|---|
| `QA-CINEMATIC-V1094-001` | Modern handheld partial-source ownership | `IMPLEMENTED / UNITY VERIFICATION REQUIRED` | `BDModernHandheld3DQA` required the retired `Short Core Shadow To Left` token only from the base presenter after V10.9 moved the grounded shadow system into `BDModernHandheld3DPresenter.CinematicEnvironment.cs`. The validator now checks the authoritative partial and its new contact-shadow names. Close only after a fresh Unity compile and `TEST EVERYTHING` return `0/0/0`. |
| `DELIVERY-CINEMATIC-V1094-002` | Latest-commit preflight | `FIXED IN PACKAGE / INSTALL REQUIRED` | V10.9.3 required skill identifiers inside `AGENTS.md`, although the committed contract keeps them in `.agents/skills/*/SKILL.md` and describes the responsibilities in prose in `AGENTS.md`. The package correctly wrote nothing. V10.9.4 validates each source at its real owner and protects unrelated latest-commit files by before/after SHA-256 comparison. |
<!-- BND_POST_INTRO_CINEMATIC_QA_LATEST_COMMIT_ALIGNMENT_V1094:END -->

<!-- BND_POST_INTRO_CINEMATIC_DIRECTOR_PASS_V109:BEGIN -->
## Post-intro cinematic director defects — V10.9 implementation supplied, Unity verification pending

| ID | Area | Status | Current truth / acceptance condition |
|---|---|---|---|
| `CINEMATIC-V109-001` | Table/environment geometry | `IMPLEMENTED / UNITY VERIFICATION REQUIRED` | The prior visible table was a vertical `Quad`. The replacement must render a complete thick tabletop, apron/frame, four connected legs/feet, grounded floor and curved dark cyclorama. At the opening at least three legs and floor must read clearly; the final frame must retain the front edge and thickness. |
| `CINEMATIC-V109-002` | Camera path and pacing | `IMPLEMENTED / UNITY VERIFICATION REQUIRED` | One existing camera now runs for 4.40 seconds through a five-knot natural cubic path. It starts high/far/left, descends and advances, completes horizontal alignment before the long settle, uses no roll/noise/cut and ends at the exact ordinary Main Menu camera state. |
| `CINEMATIC-V109-003` | Static scene ownership and handoff | `IMPLEMENTED / UNITY VERIFICATION REQUIRED` | Device, table, floor, cyclorama and shadows remain at authoritative rest transforms. Input stays locked until the exact final frame. Internal Main Menu returns must not replay the shot. Acceptance requires no visible position/FOV/exposure/screen jump at handoff. |
| `DELIVERY-V1091-001` | False merge-conflict detection and dead Terminal launcher | `PACKAGE VERIFIED` | The first V10.9 real application rolled back safely because inline bug-ledger prose containing `` `=======` `` was misclassified as a conflict marker. V10.9.1 accepts decorative inline separators, blocks only full-line Git markers, self-tests both cases, and uses a child Python installer from the existing Terminal instead of a double-clickable `.command` process. |
<!-- BND_POST_INTRO_CINEMATIC_DIRECTOR_PASS_V109:END -->

<!-- BND_FIRST_LAUNCH_TUTORIAL_V1081_HOTFIX:BEGIN -->
## First-launch tutorial V10.8.1 hotfixes — implementation supplied, Unity verification pending

| ID | Area | Status | Current truth / acceptance condition |
|---|---|---|---|
| `TUTORIAL-V1081-001` | Mounted shooting progression | `IMPLEMENTED / UNITY VERIFICATION REQUIRED` | The prior shot transaction always used `advancesLesson: false`, so a confirmed projectile impact could kill the enemy without advancing. Only the real `RangedAttack` lesson shot now opts into progression, and only a visible impact against a living target completes `MountedShot` and enters Reload. Misses, firing and animation completion do not advance. |
| `TUTORIAL-V1081-002` | Post-BBH scene ownership | `IMPLEMENTED / UNITY VERIFICATION REQUIRED` | The table, handheld, screen and shadow are one persistent full-screen 3D scene. They remain at rest transforms throughout the transition; only the existing scene camera moves/rotates/changes lens. Expanded table coverage must prevent empty-background exposure or apparent clipping at the opening angle. |
| `DELIVERY-V1081-001` | Terminal semantic colors | `PACKAGE VERIFIED` | Pseudo-terminal tests confirmed bold green PASS, bold red BLOCKED, cyan INFO and magenta CLEANED. `NO_COLOR=1`, `TERM=dumb` and redirected output contained no ANSI sequences while retaining textual prefixes. Success and blocked paths removed the exact source ZIP and extracted package artifacts. |

The two Runtime rows remain open for Unity/Play Mode. The terminal-delivery row passed its package gate and remains here only as the current recorded verification truth.
<!-- BND_FIRST_LAUNCH_TUTORIAL_V1081_HOTFIX:END -->

<!-- BND_FIRST_LAUNCH_TUTORIAL_MECHANICS_REPAIR_V108:BEGIN -->
## First-launch tutorial V10.8 regressions — implementation supplied, Unity verification pending

| ID | Area | Status | Current truth / acceptance condition |
|---|---|---|---|
| `TUTORIAL-V108-001` | Injured horse | `IMPLEMENTED / UNITY VERIFICATION REQUIRED` | A horse in the injured/red state rejects Mount and explains that healing is required. Mount may succeed only after the real tutorial healing transaction restores it. |
| `TUTORIAL-V108-002` | Locomotion | `IMPLEMENTED / UNITY VERIFICATION REQUIRED` | Player, horse and active enemies use alternating point-filtered leg frames derived from actual movement/action state. No stationary bob may substitute for moving legs. |
| `TUTORIAL-V108-003` | Mounted/charged projectile timing | `IMPLEMENTED / UNITY VERIFICATION REQUIRED` | Target health/death and lesson completion occur only when the visible projectile reaches its impact phase; firing may not kill the target immediately. One projectile resolves at most once. |
| `TUTORIAL-V108-004` | Death recovery | `IMPLEMENTED / UNITY VERIFICATION REQUIRED` | Each major lesson records a nearby stable checkpoint. Death still uses the covered restore sequence but may not send the player several completed lessons backward. |
| `TUTORIAL-V108-005` | Grappling Hook | `IMPLEMENTED / UNITY VERIFICATION REQUIRED` | The selected target is pulled by the visible rope/action presentation first. Damage and lesson progression resolve only when that presentation completes. |
| `TUTORIAL-V108-006` | Course dividers | `IMPLEMENTED / UNITY VERIFICATION REQUIRED` | Decorative transition lines/visible lesson gates are absent. Progression remains enforced by invisible coordinate clamps plus contextual feedback, not unexplained world-space stripes. |
| `TUTORIAL-V108-007` | Final boss clarity/fairness | `IMPLEMENTED / UNITY VERIFICATION REQUIRED` | Boss instructions and state remain visible; attacks expose telegraph, committed impact and recovery; close contact alone is not instant death; ranged attacks travel visibly and can be avoided; the boss accepts damage only during a stated recovery opening. |
| `TUTORIAL-V108-008` | Enemy collision | `IMPLEMENTED / UNITY VERIFICATION REQUIRED` | Living enemies and the boss block the player/horse body on the course axis. The player cannot walk or ride through them, while defeated/inactive actors stop blocking. |
| `TUTORIAL-V108-009` | Post-BBH camera | `IMPLEMENTED / UNITY VERIFICATION REQUIRED` | The special one-shot is a full-screen real-3D product-scene camera/device move with continuous perspective and exact final-pose restoration. No screen-space scale, slide, flat card or PowerPoint-like interpolation is allowed. |
| `TUTORIAL-V108-010` | Charged Shot lesson | `IMPLEMENTED / UNITY VERIFICATION REQUIRED` | Holding Ranged beyond the production threshold starts charge; full charge fires automatically; early release fires ordinary before threshold or cancels after charge begins; all remaining ammo is consumed; Reload starts immediately; progression waits for the charged projectile impact and reload completion. |

Source/static/package checks do not verify these rows. They remain open until a fresh Unity compile, TEST EVERYTHING and the focused Play Mode matrix pass.
<!-- BND_FIRST_LAUNCH_TUTORIAL_MECHANICS_REPAIR_V108:END -->

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

<!-- BND_FIRST_LAUNCH_TUTORIAL_PRODUCTION_COURSE_V10:BEGIN -->
## Tutorial production-course acceptance status

The earlier tutorial linearity, sustained-riding and missing-action-motion findings are implemented locally through the V10 course, but remain open for Unity/Play Mode confirmation. Additional acceptance risks tracked by V10 are jump collision, Tap/Hold double emission, checkpoint orphan cleanup, contextual Parry timing, mounted-only permissions, optional-secret duplication and Mini-Boss phase/death ordering. No item is marked verified by static package checks.
<!-- BND_FIRST_LAUNCH_TUTORIAL_PRODUCTION_COURSE_V10:END -->

<!-- BND_FIRST_LAUNCH_TUTORIAL_V10_WARNING_CLEANUP_V101:BEGIN -->
## BUG-FIRST-LAUNCH-V10-001 — write-only tutorial learning flags

```text
Status: FIXED IN V10.1 CODE / UNITY RERUN REQUIRED
Severity: warning-cleanliness and duplicate-state debt
Evidence: six CS0414 warnings in ProductionCourse.cs after V10 installation
```

The write-only Jump, Dodge, Parry, Hazard, MountedShot and MountedImpact booleans duplicated the existing `TutorialLearningState` dictionary. V10.1 removes those fields and routes the remaining lesson completions through `SetFirstLaunchTutorialLearningState`. Close only after Unity recompiles without these warnings and TEST EVERYTHING remains clean.
<!-- BND_FIRST_LAUNCH_TUTORIAL_V10_WARNING_CLEANUP_V101:END -->

<!-- BND_FIRST_LAUNCH_TUTORIAL_V10_INPUT_RESPAWN_FLASH_REPAIR_V102:BEGIN -->
## Active tutorial V10.2 repair findings

| ID | Severity | Status | Finding | Acceptance |
|---|---:|---|---|---|
| `BUG-TUTORIAL-V102-001` | Blocker | IMPLEMENTED / VERIFY IN UNITY | Jump, Dodge and Parry tutorial labels/consumers diverged from live controls. | Space jumps; directional double-tap dodges; timed light or heavy attack parries; physical labels and actions match. |
| `BUG-TUTORIAL-V102-002` | High | IMPLEMENTED / VERIFY IN UNITY | Player death/checkpoint restore read as an unexplained position jump. | Character fades out, checkpoint restores only under opaque cover, then the character fades back in. |
| `BUG-TUTORIAL-V102-003` | High | IMPLEMENTED / VERIFY IN UNITY | Legacy/old menu could flash while the modern first-launch page waited for flow resolution. | No old/legacy menu frame is visible between BBH intro and the intended handheld page. |

Do not close these findings from static inspection or the earlier V10.1 automated pass. Close only after the new build compiles, TEST EVERYTHING is clean and focused Play Mode confirms each visible behavior.
<!-- BND_FIRST_LAUNCH_TUTORIAL_V10_INPUT_RESPAWN_FLASH_REPAIR_V102:END -->

<!-- BND_FIRST_LAUNCH_TUTORIAL_ENTRY_GATE_V103:BEGIN -->
## V10.3 focused defects

- `FIX IMPLEMENTED / VERIFY`: the package installer previously deleted only the extracted installer payload because the source ZIP remained in `~/Downloads`, outside `PACKAGE_ROOT`. V10.3 deletes the exact known tutorial ZIP filenames only after successful validation.
- `FIX IMPLEMENTED / VERIFY`: a legacy or stale menu frame could appear between the BBH boot intro and the correct modern handheld page because the presenter was installed after scene load and visibility depended on a resolved flow. V10.3 installs before scene load and reserves the modern presentation until boot completion plus flow resolution.
- `NEW FEATURE / VERIFY`: first-launch state now presents a dedicated pixel choice screen before tutorial gameplay.
- `OPEN`: the full tutorial limb/animation production pass remains required and must not be marked complete from this patch.
<!-- BND_FIRST_LAUNCH_TUTORIAL_ENTRY_GATE_V103:END -->

<!-- BND_FIRST_LAUNCH_TUTORIAL_PROGRESSION_GATE_REPAIR_V104:BEGIN -->
## V10.4 tutorial progression defects

- `FIX IMPLEMENTED / VERIFY`: Spin and Grapple targets spawned behind the player's post-dismount position, producing an apparent invisible wall and preventing later mechanics from appearing.
- `FIX IMPLEMENTED / VERIFY`: progression clamps had no visible world representation.
- `FIX IMPLEMENTED / VERIFY`: mounted tutorial paths could still execute sword/Heavy/Hook actions.
- `FIX IMPLEMENTED / VERIFY`: the mounted-shot lesson wording did not match the full-magazine requirement needed to reach Reload.
- `FIX IMPLEMENTED / VERIFY`: the Play/Skip choice used non-pixel `Text` rendering.
- `OPEN`: full production animation pass for player, horse, enemies and mini-boss remains the next implementation task after V10.4 verification.
<!-- BND_FIRST_LAUNCH_TUTORIAL_PROGRESSION_GATE_REPAIR_V104:END -->

<!-- BND_INTRO_TO_MAIN_MENU_CINEMATIC_AND_TUTORIAL_SPACING_V105:BEGIN -->
## V10.5 presentation defects

- `FIX IMPLEMENTED / VERIFY`: subtitle crowded against the `B&D` title.
- `FIX IMPLEMENTED / VERIFY`: intro completion lacked an explicit one-shot cinematic destination contract.
- `FIX IMPLEMENTED / VERIFY`: ordinary main-menu entries were not formally separated from the special post-intro shot.
- `OPEN`: full tutorial player/horse/enemy/mini-boss animation production remains next.
<!-- BND_INTRO_TO_MAIN_MENU_CINEMATIC_AND_TUTORIAL_SPACING_V105:END -->

<!-- BND_BBH_GLOBAL_TIMESCALE_REMOVAL_V106:BEGIN -->
## BBH intro mutates global time scale

- Severity: blocker.
- Detected by: `FIRST_LAUNCH_TUTORIAL_CONTRACT_INVALID`.
- Cause: legacy `BDBBHBootIntro` startup code assigned the global time scale to zero.
- Package correction: remove the assignment; keep unscaled timing and explicit local ownership.
- Verification: pending Unity compile, BBH playback, cinematic handoff and `TEST EVERYTHING`.
<!-- BND_BBH_GLOBAL_TIMESCALE_REMOVAL_V106:END -->

<!-- BND_POST_INTRO_TRANSITION_COLORED_OUTPUT_CLEAN_EXIT_V1072:BEGIN -->
## V10.7.1 self-rejected after writing

- Severity: delivery blocker.
- Cause: runtime token validation concatenated all changed C# files, including the editor validator containing the forbidden-token literals.
- Safety result: the backup restored the repository.
- Correction: scan only `/Runtime/` files for runtime tokens; validate editor QA independently.
- Cleanup correction: source ZIP and extracted installer residue must be removed on every exit path.
<!-- BND_POST_INTRO_TRANSITION_COLORED_OUTPUT_CLEAN_EXIT_V1072:END -->
<!-- BND_CHILD_DIALOGUE_BUBBLE_POWER_TIMING_V10116:BEGIN -->
## V10.11.6 focused visual fixes

| ID | Area | Status | Acceptance condition |
|---|---|---|---|
| FL-CIN-BUBBLE-TAIL-OVERLAP | Opening dialogue bubble | FIXED IN CODE / PLAY MODE VERIFY | Tail touches the lower bubble edge without drawing over the bubble body. |
| FL-CIN-BUBBLE-VERTICAL-POSITION | Opening dialogue bubble | FIXED IN CODE / PLAY MODE VERIFY | Entire bubble sits slightly lower while remaining fully readable and inside frame. |
| FL-CIN-POWER-ON-DELAY | Opening power-on | FIXED IN CODE / PLAY MODE VERIFY | After camera settlement at `9.02s`, power-on starts at `9.20s` and ends at `10.20s`, with no white frame or camera discontinuity. |
<!-- BND_CHILD_DIALOGUE_BUBBLE_POWER_TIMING_V10116:END -->

<!-- BD_TUTORIAL_FINAL_INPUT_TARGET_PLAYER_V101130 -->
- Resolved: AttackEnemy only played its animation and never called ResolveFirstLaunchTutorialProductionMelee, so no impact transaction existed. The handler now starts the real transaction.
- Resolved: player model was visually dense and could face away from the lesson target.

<!-- BND_V1011308_BUGS -->
## Closed by V10.11.30.8

- `BDTutorialLessonScreensInputParryV1011306QA` called a nonexistent
  `BDOneClickQAResult.AddBlocker` API and blocked Unity compilation.

## Resolved in V10.11.30.16
<!-- BND_TUTORIAL_CONTRACT_REPAIR_V1011316 -->
Resolved five QA blockers caused by dialogue-contract loss and the lesson-complete travel message living outside Gameplay.cs.
<!-- BND V10.11.30.17 LESSON COMPLETE CONTRACT -->
- The canonical lesson-complete travel message is owned by `Gameplay.cs`, consumed by `LessonScreens.cs`, and QA reports missing contracts against the actual source path.
