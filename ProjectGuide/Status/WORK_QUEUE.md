<!-- BND_TUTORIAL_BUBBLE_DEPTH_HORSE_CONTINUE_V1011330:BEGIN -->
## Immediate gate — V10.11.30.30

1. Compile in Unity `6000.0.76f1` and run `Boredom And Dungeons -> TEST EVERYTHING`; require `0 blockers / 0 warnings / 0 info`.
2. Clear Console and enter Play Mode from a stopped state; confirm neither memoryless depth load nor store message appears.
3. Inspect the mother's bubble and confirm the far-left diamond has a complete frame with no missing left/top/bottom edge.
4. Verify the horse shooter remains after the scripted hit and is killed by the following player attack.
5. Verify the pixel CONTINUE cue appears after completed lessons, remains while walking to the right edge and disappears before the room scroll.
6. Verify HorseReturn stays invisible during scrolling and begins only after the destination room settles.
7. Verify the mounted-shooting target dies exactly at the required projectile impact.
<!-- BND_TUTORIAL_BUBBLE_DEPTH_HORSE_CONTINUE_V1011330:END -->

<!-- BND_TUTORIAL_HORSE_COMBAT_CONTINUE_V1011329:BEGIN -->
## Immediate gate — V10.11.30.29

1. Compile in Unity `6000.0.76f1` and run `Boredom And Dungeons -> TEST EVERYTHING`; require `0 blockers / 0 warnings / 0 info`.
2. In the horse-shot room, confirm the shooter remains after the scripted hit and is killed by the following Jump Attack rather than disappearing during a reset.
3. After that room completes, confirm the pixel CONTINUE badge appears while the player walks to the physical right edge and disappears before scrolling.
4. During the scroll into Horse Return, confirm the horse is absent. After the camera settles, confirm the short beat, then the injured horse's return animation begins.
5. In Fire While Riding, hit the centered target with the required shot and confirm it dies at visible projectile impact.
6. Repeat several other completed rooms and confirm each shows exactly one restrained CONTINUE effect with no input lock, fade, teleport or early next-room lesson.
<!-- BND_TUTORIAL_HORSE_COMBAT_CONTINUE_V1011329:END -->

<!-- BND_TUTORIAL_FLOW_COHERENCE_V1011328:BEGIN -->
## Immediate gate — V10.11.30.28

1. Compile in Unity `6000.0.76f1` with no new errors or warnings.
2. Run `Boredom And Dungeons -> TEST EVERYTHING`; require `0 blockers / 0 warnings / 0 info`.
3. In the first room, cross and land beyond the obstacle, then move both directions and confirm facing follows movement.
4. In at least three later on-foot rooms, use Jump before and after completing the room objective.
5. Watch the horse-shot impact and later HorseReturn: the shooter remains until killed, and the horse stays absent during scrolling before its post-settle return begins.
6. Complete Heal Horse and press Interact immediately; Mount Again must occur in the same room.
7. Confirm no blue finish gate or wall-jump wall appears in the Heal/Remount room or any unrelated room.
8. Watch every continuous room scroll: static assets and ordinary actors enter with the viewport; HorseReturn alone appears only after settlement by design.

<!-- BND_TUTORIAL_FLOW_COHERENCE_V1011328:END -->

<!-- BND_TUTORIAL_HORSE_FREE_OPENING_PET_SUPPRESSION_V1011327:BEGIN -->
## Immediate gate — V10.11.30.27

1. Compile in Unity `6000.0.76f1`.
2. Run `Boredom And Dungeons -> TEST EVERYTHING`; require `0 blockers / 0 warnings / 0 info`.
3. Reset first launch and verify room 0 contains only the player, Move and Jump; no horse is visible at any time.
4. Verify room 1 is centered one-hit Quick Attack and the upper-right `PET` card is absent.
5. Complete Heavy Attack, Dodge and Parry.
6. Verify the next room contains Mount then Ride with a reachable horse.
7. Reach the following room and verify EnemyArrival/HorseShot, then continue to Jump Attack without replaying Quick Attack or soft-locking.
<!-- BND_TUTORIAL_HORSE_FREE_OPENING_PET_SUPPRESSION_V1011327:END -->

<!-- BND_TUTORIAL_CENTERED_PARRY_HORSE_METAL_V1011326:BEGIN -->
## Immediate gate — V10.11.30.26

1. Compile in Unity `6000.0.76f1` with no errors or new warnings.
2. Run `Boredom And Dungeons -> TEST EVERYTHING`; require `0 blockers / 0 warnings / 0 info`.
3. Verify room 0 remains Move → Jump and room 1 is MountHorse → RideHorse.
4. Verify room 2 is the authored EnemyArrival → HorseShot story and room 3 begins Quick Attack on foot.
5. In room 3, confirm one clearly readable enemy is exactly at screen center, one valid Light impact kills it, the card disappears, and right-edge contact advances.
6. In Parry, try ordinary attacks and confirm the teacher cannot die. Confirm exactly one projectile loop is visible, a valid parry removes it immediately, and the lesson progresses without residual shots or soft-lock.
7. Clear Console, reload the scene or re-enter Play Mode, and confirm neither memoryless depth load/store message appears.
8. Continue through the next right-edge handoff before accepting; do not commit or push until all checks pass.
<!-- BND_TUTORIAL_CENTERED_PARRY_HORSE_METAL_V1011326:END -->

<!-- BND_TUTORIAL_CONTINUOUS_ROOM_SEQUENCE_V1011325:BEGIN -->
## Immediate gate — V10.11.30.25

1. Compile in Unity `6000.0.76f1` with no errors or new warnings.
2. Run `Boredom And Dungeons -> TEST EVERYTHING`; require `0 blockers / 0 warnings / 0 info`.
3. Continue from screen two through every remaining room.
4. In every room, verify the lesson appears only after camera settlement, remains until its real objective succeeds, and then disappears completely.
5. After each objective, verify the camera stays fixed and the player must visibly touch the right edge before progression.
6. Verify every handoff scrolls smoothly into the next room with walking motion and no fade-out, fade-in, black/white cover, respawn, checkpoint restore or teleport.
7. Verify the next room's enemies, scripted events, reload timing and input do not start before it fully enters the frame.
8. Complete the boss, collect the relic and persist `Completed` before any commit or push.
<!-- BND_TUTORIAL_CONTINUOUS_ROOM_SEQUENCE_V1011325:END -->

<!-- BND_TUTORIAL_SCREEN_TWO_IMPACT_CONTINUOUS_HANDOFF_V1011324:BEGIN -->
## Immediate gate — V10.11.30.24

1. Compile in Unity `6000.0.76f1`.
2. Run `Boredom And Dungeons -> TEST EVERYTHING`; require `0 blockers / 0 warnings / 0 info`.
3. Enter screen two and use one ordinary Light attack on the centered enemy.
4. Confirm the enemy disappears exactly at visible impact and the instruction disappears.
5. Walk to and through the right edge; confirm screen three is revealed through continuous movement/camera motion with no cut, fade, cover, respawn or teleport.
6. Stop immediately after screen three appears; do not evaluate later lessons in this gate.
<!-- BND_TUTORIAL_SCREEN_TWO_IMPACT_CONTINUOUS_HANDOFF_V1011324:END -->

<!-- BND_TUTORIAL_SECOND_SCREEN_LIGHT_ATTACK_V1011323:BEGIN -->
## Immediate gate — V10.11.30.23 screen two only

1. Compile in Unity `6000.0.76f1`.
2. Run `Boredom And Dungeons -> TEST EVERYTHING`; require `0 blockers / 0 warnings / 0 info`.
3. Complete screen one and enter screen two.
4. Confirm the ordinary-attack tutorial appears immediately, the player is on foot at the left and one enemy is centered.
5. Walk into ordinary melee range and use one Light Attack; the enemy must die at visible impact.
6. Confirm the complete tutorial card disappears after death.
7. Walk to the visible right edge; only there may the transition to screen three begin.
8. Stop immediately after entering screen three; do not evaluate or change later lessons in this pass.
<!-- BND_TUTORIAL_SECOND_SCREEN_LIGHT_ATTACK_V1011323:END -->

<!-- BND_TUTORIAL_OPENING_SCREEN_SEQUENCE_V1011322:BEGIN -->
## Immediate gate — V10.11.30.22 opening screen only

1. Compile in Unity `6000.0.76f1` with no new error or warning.
2. Run `Boredom And Dungeons -> TEST EVERYTHING`; require `0 blockers / 0 warnings / 0 info`.
3. Reset first launch and verify the Move card appears on the first screen.
4. Confirm left/blocked input cannot complete Move; walk forward until the Move card is replaced by Jump on the same screen.
5. Jump over the obstacle, land beyond it and confirm Mount appears immediately on the same screen.
6. Mount the horse and confirm Ride appears only after the mount animation completes.
7. Ride to the objective, confirm Ride disappears, continue forward and confirm one clean transition to the next screen.
8. Stop verification there; do not evaluate or modify later lessons in this pass.
<!-- BND_TUTORIAL_OPENING_SCREEN_SEQUENCE_V1011322:END -->

<!-- BND_SCREEN_RENDER_SCHEDULING_V1011321:BEGIN -->
## Immediate gate — V10.11.30.21

1. Install the focused duplicate-render repair and let Unity recompile/reload.
2. Clear Console, restore/open the authoritative scene and enter Play Mode once.
3. Confirm neither memoryless depth load/store message appears.
4. Run `Boredom And Dungeons -> TEST EVERYTHING`; require `0 blockers / 0 warnings / 0 info`.
5. Verify the device screen, child power-on reveal and first tutorial card still render without a blank or one-frame pop.
6. Resume the complete V10.11.30.19 tutorial acceptance run.
<!-- BND_SCREEN_RENDER_SCHEDULING_V1011321:END -->

<!-- BND_TUTORIAL_QA_THRESHOLD_REALIGNMENT_V1011320:BEGIN -->
## Immediate gate — V10.11.30.20

1. Apply the QA-only threshold realignment over installed V10.11.30.19.
2. Let Unity recompile; no Runtime file should change.
3. Run `Boredom And Dungeons -> TEST EVERYTHING` and require `0 blockers / 0 warnings / 0 info`.
4. Confirm both stale threshold blocker codes are absent.
5. Resume the V10.11.30.19 full tutorial Play Mode and input matrix only after automated PASS.
<!-- BND_TUTORIAL_QA_THRESHOLD_REALIGNMENT_V1011320:END -->

<!-- BND_TUTORIAL_RUNTIME_INTEGRITY_V1011319:BEGIN -->
## Immediate blocking gate — V10.11.30.19

1. Install the cumulative patch over the captured V10.11.30 local state without discarding unrelated work.
2. Compile with Unity `6000.0.76f1`; require no compiler error or warning.
3. Run `Boredom And Dungeons -> TEST EVERYTHING`; require `0 blockers / 0 warnings / 0 info`.
4. In Move, verify one brief tap does not complete the lesson; travel at least 64 world units before completion.
5. Re-enter Play Mode from a fresh first-launch reset and confirm the Console does not emit either memoryless depth load/store message.
6. Verify the lower mother bubble has one attached left-facing pointer throughout enter, hold and exit.
7. Complete Move, walk right, and verify: no empty tutorial shell, no respawn text/cover, one fully dark screen change, then the populated Jump instruction.
8. Continue every lesson. Each mechanic unlocks only on its screen, completes only from its required world result, and no previous-screen actor/projectile/hazard can interfere during travel.
9. Verify keyboard/mouse mapping exactly: Move WASD/arrows; Jump Space; Interact E; Light J/left mouse; Heavy K/right mouse; Ranged Q/hold Q; Heal hold F; Dodge double-tap A/D or arrows; Spin hold J/left mouse; Grapple hold K/right mouse.
10. Repeat the actionable matrix with controller and physical handheld controls, including physical held X/Y/A routes.
11. Defeat the boss, collect the relic by contact, persist `Completed`, then commit/push only after the complete run is accepted.
<!-- BND_TUTORIAL_RUNTIME_INTEGRITY_V1011319:END -->

<!-- BND_TUTORIAL_FLOW_JUMP_CINEMATIC_QUEUE_V1011314 -->
## Active verification — V10.11.30.14

- [x] Remove empty tutorial-card chrome between lessons.
- [x] Restore Space / controller / physical Jump after the Jump screen unlocks.
- [x] Keep the next tutorial absent until the next-screen transition.
- [x] Lower the mother speech bubble slightly.
- [x] Preserve the higher child camera through walking and climbing.
- [ ] Require Unity compilation, TEST EVERYTHING 0/0/0 and one uninterrupted tutorial run before Commit.

<!-- BND_TUTORIAL_FINAL_QA_ZIP_CLEANUP_V1011310:BEGIN -->
## Active verification after V10.11.30.10

1. Compile in Unity 6000.0.76f1.
2. Run TEST EVERYTHING and require 0 blockers, 0 warnings, 0 info.
3. Confirm TUTORIAL_V1011301_INPUT_DAMAGE_WORLD_PROOF_MISSING does not return.
4. Complete one uninterrupted tutorial run and recheck the child menu camera.
5. Only after all gates pass, run the separate Commit and Push commands.
<!-- BND_TUTORIAL_FINAL_QA_ZIP_CLEANUP_V1011310:END -->

<!-- BND_TUTORIAL_QA_SEMANTIC_CAMERA_HEIGHT_V1011309:BEGIN -->
## Active verification after V10.11.30.9

1. Compile in Unity 6000.0.76f1.
2. Run TEST EVERYTHING and require 0 blockers, 0 warnings, 0 info.
3. Verify the nine obsolete-token blockers do not return.
4. Replay the child approach and confirm the POV is slightly higher without a snap at the regular menu handoff.
5. Recheck lesson-screen flow, mouse/Q/E input, Parry, mounted impact and one uninterrupted tutorial run.
<!-- BND_TUTORIAL_QA_SEMANTIC_CAMERA_HEIGHT_V1011309:END -->

<!-- BND_V1011307_QUEUE -->
## Current release gate — V10.11.30.7

1. Let Unity import and compile the corrected lesson-screen partial.
2. Run `Boredom And Dungeons -> TEST EVERYTHING`.
3. Require `blockers=0, warnings=0, info=0`.
4. Complete the manual lesson-screen, mouse mapping and Parry checks.
5. Only after all gates pass, run the separate Commit command and then the
   separate Push command supplied with this package.

<!-- BND_TUTORIAL_LESSON_SCREENS_INPUT_PARRY_V1011306:BEGIN -->
## Active verification after V10.11.30.6

1. Compile in Unity 6000.0.76f1.
2. Run TEST EVERYTHING and require 0 blockers, 0 warnings, 0 info.
3. Verify each completed lesson hides its instruction and never displays the next one before the next screen.
4. Verify all physical targets are centered by default and each objective requires world-state proof.
5. Verify canonical keyboard/mouse, gamepad and physical handheld mappings.
6. Verify the parry target is passable, parry works with timed Light or Heavy, and the enemy is not squashed.
7. Complete one uninterrupted tutorial run.
<!-- BND_TUTORIAL_LESSON_SCREENS_INPUT_PARRY_V1011306:END -->

<!-- BND_TUTORIAL_INPUT_MECHANICS_MOUNTED_IMPACT_V1011305:BEGIN -->
## Immediate gate — V10.11.30.5

Verify every keyboard, mouse, controller and physical route, then verify free mechanic execution outside the highlighted lesson and complete Mounted Impact by direct horse contact with no invisible collision wall. Do not return to the ordered queue until TEST EVERYTHING is 0/0/0 and one uninterrupted tutorial run completes.
<!-- BND_TUTORIAL_INPUT_MECHANICS_MOUNTED_IMPACT_V1011305:END -->

<!-- BND_TUTORIAL_PLAYER_CANONICAL_ASSET_NAME_V1011304:BEGIN -->
## Immediate gate — V10.11.30.4

1. Apply the canonical player asset-name alignment over V10.11.30.3.
2. Wait for Unity compilation.
3. Run `Boredom And Dungeons -> TEST EVERYTHING`; require `0/0/0`.
4. Confirm the visible player from V10.11.30.3 still appears and animates.
5. Continue the existing full tutorial Play Mode acceptance run.
<!-- BND_TUTORIAL_PLAYER_CANONICAL_ASSET_NAME_V1011304:END -->

<!-- BND_TUTORIAL_PLAYER_VISIBILITY_RUNTIME_V1011303:BEGIN -->
## Immediate gate — V10.11.30.3

1. Compile in Unity `6000.0.76f1`.
2. Run `Boredom And Dungeons -> TEST EVERYTHING`; require `0 blockers / 0 warnings / 0 info`.
3. Start the tutorial and confirm the blond/red/blue player is immediately visible at the left side of the course.
4. Move left and right; confirm the full player flips with direction and remains visible.
5. Jump, light attack, heavy attack and spin; confirm walk/action frames remain active and no player layer disappears.
6. Continue the existing enemy-lethality and full tutorial completion verification.
<!-- BND_TUTORIAL_PLAYER_VISIBILITY_RUNTIME_V1011303:END -->

<!-- BND_TUTORIAL_QA_CONTRACT_REALIGNMENT_V1011302:BEGIN -->
## Immediate gate — V10.11.30.2

1. Install the QA-only contract realignment package.
2. Let Unity recompile the changed Editor validation assembly.
3. Run `Boredom And Dungeons -> TEST EVERYTHING`; require `0 blockers / 0 warnings / 0 info`.
4. Confirm no blocker still requests `82x118`, the retired exact hair/shirt/pants literals, or V10.11.17 sprite markers.
5. Continue the V10.11.30.1 focused Play Mode checks: first Light/Heavy kill at visible impact, Arrow/mouse/controller/physical input, atomic Spin, outcome-owned Dodge and correctly facing compact player.
6. Complete the full tutorial before acceptance or commit.
<!-- BND_TUTORIAL_QA_CONTRACT_REALIGNMENT_V1011302:END -->

<!-- BND_TUTORIAL_FINAL_INPUT_COMBAT_PLAYER_V1011301:BEGIN -->
## Immediate gate — V10.11.30.1

1. Install the backup-aware package and confirm its PASS line.
2. Let Unity `6000.0.76f1` compile with no `RightArrowownArrow` error and no `CS0414` mouse-capture warnings.
3. Run `Boredom And Dungeons -> TEST EVERYTHING`; require `0 blockers / 0 warnings / 0 info`.
4. Verify Arrow keys, mouse buttons, controller and physical controls each execute their tutorial actions once.
5. In the first melee lesson, a miss does not progress; the valid visible Light impact kills the first enemy and advances. Repeat for Heavy.
6. Verify the compact player sprite faces travel/attack direction and no articulated child pieces remain visible.
7. Verify Spin kills neither target unless one spin contains both; verify Dodge requires reaching the other side.
8. Complete the full tutorial before acceptance or commit.
<!-- BND_TUTORIAL_FINAL_INPUT_COMBAT_PLAYER_V1011301:END -->

## V10.11.28 focused verification
- Run TEST EVERYTHING and a complete tutorial pass.
- Verify first quick-attack target disappears on the correct impact, colored text persists, spin kills both together only, dodge crosses the obstacle, and the articulated model remains visible.

## Completed — all tutorial enemy lesson lethality

- [x] Correct authored hit is lethal for every focused non-boss enemy lesson.
- [x] Wrong mechanics remain available but cannot damage protected lesson targets.
- [x] Grapple pull stays non-lethal and requires the follow-up kill.
- [x] Per-lesson hit confirmation resets on every step transition.

<!-- BD ALL LESSON TARGETS LETHAL V10.11.26 -->

# BD TUTORIAL LESSON ENTRY + DAMAGE OWNERSHIP V10.11.25

Current verification target: complete every focused lesson only by hitting its target with the requested mechanic while all previously unlocked mechanics remain usable.

## BD V10.11.24 MOUNTED RANGED SEQUENCE AND NO MOUNTED DODGE
1. Verify ordinary projectile impact and reload can finish in either order.
2. Verify Reload remains visible briefly and always reaches ChargedShot.
3. Verify charged auto-fire reaches MountedImpact after impact plus reload, and a miss can be retried.
4. Verify all mounted dodge inputs remain movement only and never grant invulnerability.

## BD V10.11.23 LESSON PERSISTENCE AND PROGRESSION GATE
1. Run Unity compilation and TEST EVERYTHING.
2. Complete every lesson using keyboard/mouse and repeat critical actions with physical controls.
3. At each lesson, hold forward before completing it and verify the player stops at the boundary without a visual divider.
4. Verify the instruction remains visible until success, then changes only to the next lesson.

## BD V10.11.22.2 QA CONTRACT RECONCILIATION
1. Run Unity compilation and TEST EVERYTHING.
2. Verify heavy attack with K, right mouse and physical Y.
3. Verify both binding cards remain visible and unclipped.
4. Verify screen power begins after camera settle and reveals behind the moving line.

<!-- BND_TUTORIAL_INPUT_PARITY_POWER_REVEAL_V101122:BEGIN -->
## Immediate release gate — V10.11.22

1. Compile with Unity `6000.0.76f1`.
2. Run `Boredom And Dungeons -> TEST EVERYTHING`; require `0 blockers / 0 warnings / 0 info`.
3. Replay the opening: after the child sits and the camera settles, power-on must begin almost immediately; duration must feel unchanged.
4. Confirm the moving power line exposes only the portion it has crossed. The full screen must never appear before the line.
5. Complete every tutorial lesson once with keyboard/gamepad bindings.
6. Complete every tutorial lesson with physical handheld controls.
7. Verify direct mouse display input: left/context click, right heavy click, middle ranged click, hold actions and directional dodge double-click.
8. Confirm clicks outside the physical display never trigger world actions.
9. Confirm the binding panel remains centered, readable and unclipped at the supported aspect ratios.
10. Commit and Push only after the complete matrix passes.
<!-- BND_TUTORIAL_INPUT_PARITY_POWER_REVEAL_V101122:END -->

<!-- BND_SMOOTH_DRIP_TUTORIAL_MOUSE_V101121:BEGIN -->
## Immediate gate — V10.11.21

1. Compile with Unity `6000.0.76f1`.
2. Run `Boredom And Dungeons -> TEST EVERYTHING`; require `0 blockers / 0 warnings / 0 info`.
3. Replay the complete BBH opening at 16:9 and confirm only the melt changed: same logo, color, timing before melt and scene behind.
4. Confirm the melt moves downward, uses broad connected rounded liquid forms, has a smooth antialiased edge and contains no visible strips or pixel stairs.
5. At the ordinary-attack lesson, click once on the physical display; require exactly one light attack, existing visible impact and correct lesson advance.
6. Click outside the physical display and on hardware controls; no accidental world attack may occur.
7. Complete all remaining tutorial lessons and verify no existing trigger was skipped, hidden early or stuck.
8. Commit and Push only after automated and focused Play Mode acceptance.
<!-- BND_SMOOTH_DRIP_TUTORIAL_MOUSE_V101121:END -->

<!-- BND_TUTORIAL_QA_CONTRACT_RECOVERY_V1011203:BEGIN -->
## Immediate gate — V10.11.20.3

1. Compile with Unity `6000.0.76f1`.
2. Run `Boredom And Dungeons -> TEST EVERYTHING`; require `0 blockers / 0 warnings / 0 info`.
3. Start from Move and confirm every instruction appears and disappears only at its authored trigger.
4. At the first normal enemy, click inside the tutorial world and confirm one light attack occurs; clicks on D-pad/handheld controls must not create an extra attack.
5. Complete Jump → MountHorse → RideHorse; the mount instruction must not disappear before mounting succeeds.
6. Verify keyboard/mouse keycaps and the physical handheld illustration remain bounded and readable.
7. Complete the tutorial through the boss and relic, then run Commit and Push as separate commands.
<!-- BND_TUTORIAL_QA_CONTRACT_RECOVERY_V1011203:END -->

<!-- BND_TUTORIAL_INDIE_BINDING_VISUALS_HOTFIX_V101118:BEGIN -->
## Immediate verification — V10.11.18

1. Compile in Unity with no Console errors.
2. Run TEST EVERYTHING and require 0 blockers / 0 warnings / 0 info.
3. Confirm the tutorial instruction panel shows the desktop/controller keycap and the illustrated physical handheld control together, with the title `PHYSICAL HANDHELD`, inside bounds.
4. Continue the full unskipped opening/tutorial acceptance run before committing.
<!-- BND_TUTORIAL_INDIE_BINDING_VISUALS_HOTFIX_V101118:END -->

<!-- BND_OPENING_TUTORIAL_RECOVERY_V101117:BEGIN -->
## Immediate release gate — V10.11.17

1. Compile with zero Console errors and zero compiler warnings.
2. Run `Boredom And Dungeons -> TEST EVERYTHING`; require `0 blockers / 0 warnings / 0 info`.
3. Watch the BBH ending frame-by-frame: the BBH surface itself must drip downward while the room is already behind it; no isolated strip, black flash, or duplicate fade may enter the child shot.
4. Confirm `Sweety, where are you?` remains visible during the first child steps and fades while walking.
5. Complete `Move -> Jump -> MountHorse -> RideHorse` without pausing; landing must normalize beside the horse, the mount prompt must remain visible, and the correct interaction must advance to riding.
6. Confirm every lesson remains timed correctly, abilities remain locked until their lesson, left-click attack works when unlocked, enemies block the player, and full-game horse prompts remain hidden during the handheld tutorial.
7. Confirm the player is visibly blond with a red shirt and blue trousers throughout normal, hit, airborne, mounted, and death/retry presentation.
8. Confirm desktop/controller keycaps and illustrated physical handheld controls are both visible and stay inside the instruction panel.
9. Confirm the supplied artwork is visible in Player Settings and a Standalone development build.
<!-- BND_OPENING_TUTORIAL_RECOVERY_V101117:END -->

<!-- BND_TUTORIAL_DRIP_CONTRACT_BINDING_HOTFIX_V101116:BEGIN -->
## Immediate verification gate — V10.11.16

1. Let Unity compile with no C# errors.
2. Run `Boredom And Dungeons -> TEST EVERYTHING`; require `0 blockers / 0 warnings / 0 info`.
3. Watch the BBH handoff frame-by-frame: the drip belongs to the intro artwork, the kitchen is visible behind it only during the drip, and no stripe/glitch starts the child animation.
4. Verify each actionable tutorial lesson displays both input cards and the correct physical handheld button.
<!-- BND_TUTORIAL_DRIP_CONTRACT_BINDING_HOTFIX_V101116:END -->

<!-- BND_TUTORIAL_QA_COMPILATION_HOTFIX_V101115:BEGIN -->
## Immediate verification gate — V10.11.15

1. Let Unity finish recompiling.
2. Confirm the Console has no compiler errors from `BDTutorialOpeningPolishV1011QA.cs`.
3. Run `Boredom And Dungeons -> TEST EVERYTHING`.
4. Continue the pending V10.11.14 Play Mode checks only after automated QA runs.
<!-- BND_TUTORIAL_QA_COMPILATION_HOTFIX_V101115:END -->

<!-- BND_TUTORIAL_DRIP_MOUNT_INPUT_BINDINGS_V101114:BEGIN -->
## Immediate verification gate — V10.11.14

1. Compile in Unity `6000.0.76f1` with zero errors and zero warnings.
2. Run `Boredom And Dungeons -> TEST EVERYTHING`; require `0 blockers / 0 warnings / 0 info`.
3. Watch the BBH ending frame-by-frame: the BBH layer drips off the screen, the kitchen is already behind it, and the child camera starts only after the drip finishes.
4. Complete Move and Jump: the Mount Horse prompt must appear while the player is still beside the horse.
5. At Quick Attack, use world-space left click and verify one real attack transaction and damage only on contact.
6. Verify every lesson displays both the keyboard/controller binding and the physical handheld button.
<!-- BND_TUTORIAL_DRIP_MOUNT_INPUT_BINDINGS_V101114:END -->

<!-- BND_TUTORIAL_TRIGGER_UNLOCK_HUD_COLLISION_HOTFIX_V101113:BEGIN -->
## Immediate verification gate — V10.11.13

1. Compile in Unity `6000.0.76f1` with zero warnings/errors.
2. Run `Boredom And Dungeons -> TEST EVERYTHING`; require `0 blockers / 0 warnings / 0 info`.
3. Start the opening without skipping: verify no left-edge descending line, only the mother bubble appears, and the exact line is `Sweety, where are you?`.
4. Start the tutorial: verify the full-game `TAB PET` prompt never appears outside the Game Boy.
5. Verify Move registers within the short run-up before the first obstacle, its text exits, and Jump text appears at the obstacle.
6. Verify Jump and every later ability do nothing before its lesson, then work immediately when the lesson begins.
7. Verify every lesson text appears and clears exactly once through the complete course.
8. Verify the player cannot pass through any living visible enemy and can continue after it is defeated.
<!-- BND_TUTORIAL_TRIGGER_UNLOCK_HUD_COLLISION_HOTFIX_V101113:END -->

<!-- BND_TUTORIAL_QA_DIALOGUE_HOTFIX_V101112:BEGIN -->
## Immediate verification gate — V10.11.12

1. Compile in Unity `6000.0.76f1` with zero errors and zero warnings.
2. Run `Boredom And Dungeons -> TEST EVERYTHING`; require `0 blockers / 0 warnings / 0 info`.
3. Play the opening and confirm the mother bubble reads exactly `Sweety, where are you?`.
4. Continue the existing V10.11.11 full opening/tutorial Play Mode acceptance pass without changing scope.
<!-- BND_TUTORIAL_QA_DIALOGUE_HOTFIX_V101112:END -->

<!-- BND_TUTORIAL_FINAL_PRODUCTION_COURSE_V101111:BEGIN -->
## Immediate verification gate — V10.11.11

1. Compile in Unity `6000.0.76f1`; require zero compiler warnings/errors.
2. Run `Boredom And Dungeons -> TEST EVERYTHING`; require `0 blockers / 0 warnings / 0 info`.
3. Play the complete opening and tutorial without skipping.
4. Verify BBH drips quickly, mother/child bubbles sequence correctly, and walking starts only after the child reply exits.
5. Verify each major lesson is reached by forward travel; no enemy/obstacle appears or disappears while visible; backtracking to a completed screen is impossible.
6. Verify the player's blond hair/red shirt/blue trousers and all enlarged bounded animated text.
7. Verify straight ground slash, overhead-down airborne slash, and damage only at visible contact.
8. Verify player/enemy hit reactions.
9. Verify boss three-shot vertical fan, slash and jump-slam telegraphs/ranges, ordinary shot and charged shot.
10. Collect the relic by contact, watch the player lift it, and verify magical light fades directly to the main menu.

### Queued immediately after the current tutorial release gate

- Implement the full-game horse acceleration, braking, direction-change, animation, rider, hoof-audio and speed-scaled effects task defined in `ProjectGuide/Tasks/BACKLOG/FULL_GAME_HORSE_ACCELERATION_BRAKING_AND_WEIGHT.md`.
- This queued task is **not part of V10.11.11** and must not be applied to the standalone tutorial unless a later task explicitly unifies the systems.
<!-- BND_TUTORIAL_FINAL_PRODUCTION_COURSE_V101111:END -->

<!-- BND_TUTORIAL_PLAYER_TEXT_BOSS_ENVIRONMENT_V101110:BEGIN -->
## Immediate gate — V10.11.10

1. Compile in Unity `6000.0.76f1` and confirm the CS0114 warning is absent.
2. Run `Boredom And Dungeons -> TEST EVERYTHING`; require `0 blockers / 0 warnings / 0 info`.
3. Start the tutorial and verify the player begins at X `-900`, visibly separated from the first obstacle.
4. Verify player colors/profile: natural skin, blond hair, red shirt, blue trousers, eye on the visible side.
5. Inspect every tutorial text surface for larger bounded text, outline, step color and restrained letter animation without leaving its panel.
6. In the environment lesson, attack and watch the enemy travel into the hazard, impact, linger briefly, then advance.
7. Fight the boss with ordinary shots and charged shots; both must resolve only at projectile impact.
8. At the final relic, verify the instruction says to walk into it, the object reads as collectible, and contact completes the tutorial.
<!-- BND_TUTORIAL_PLAYER_TEXT_BOSS_ENVIRONMENT_V101110:END -->

<!-- BND_TUTORIAL_WALLJUMP_BOSS_TYPOGRAPHY_DIALOGUE_V10118:BEGIN -->
## Immediate gate — V10.11.8

1. Compile in Unity `6000.0.76f1`.
2. Run `Boredom And Dungeons -> TEST EVERYTHING`; require `0 blockers / 0 warnings / 0 info`.
3. Replay the opening and verify the dialogue bubble body and diamond read as one shape during enter, hold and reverse exit.
4. In wall-jump training, verify repeated airborne jump presses do not create infinite jumps.
5. Land on the left platform, confirm the character is not sunk, jump right, clear the wall and land on upper ground.
6. Verify headline/detail/binding text is visibly larger, colored and animated without clipping.
7. Fight the boss through both phases; confirm the redesigned slam, visible projectile attacks and hold-to-charge auto-fire.
8. Complete the entire tutorial and persist `Completed` before resuming the broader retro redesign queue.
<!-- BND_TUTORIAL_WALLJUMP_BOSS_TYPOGRAPHY_DIALOGUE_V10118:END -->

<!-- BND_TUTORIAL_COMPLETION_INTEGRITY_V10117:BEGIN -->
## Immediate gate — V10.11.7

1. Compile in Unity `6000.0.76f1`.
2. Run `Boredom And Dungeons -> TEST EVERYTHING`; require `0 blockers / 0 warnings / 0 info`.
3. Replay the opening and confirm the diamond is behind the bubble body.
4. Ride both directions and confirm the horse turns with movement, including during mounted action presentation.
5. Complete mounted shooting and confirm one enemy dies only when the continuously visible projectile arrives.
6. Complete the entire tutorial without an invisible mid-course wall, collect the final item and reach `Completed`.
7. Resume the broader retro tutorial production pass only after this full-run gate passes.
<!-- BND_TUTORIAL_COMPLETION_INTEGRITY_V10117:END -->

<!-- BND_TUTORIAL_OPENING_POLISH_V10113:BEGIN -->
## Immediate gate — exact-local-state V10.11.3

1. Install the V10.11.3 package built from the captured local snapshot; do not stash or discard the existing scene/gameplay work.
2. Let Unity 6000.0.76f1 compile with no project warning or error.
3. Run `Boredom And Dungeons -> TEST EVERYTHING`; require `0 blockers / 0 warnings / 0 info`.
4. Reset first launch and verify: room reveal -> short pause -> bubble fade/scale in -> feminine nonverbal cue -> readable hold -> exact reverse exit -> clean beat -> first child step.
5. Verify ESC during bubble entry, hold and exit reaches the exact final camera/screen/tutorial state with no lingering bubble or audio.
6. Complete the tutorial and verify natural wall-jump falls, explicit Jump + Attack, one-target impact-timed mounted shots, background-only decorations, larger animated type and distinct Mini-Boss slam/occasional-shot patterns.
7. Keep the broader retro art/animation pass active after this gate.
<!-- BND_TUTORIAL_OPENING_POLISH_V10113:END -->

<!-- BND_TUTORIAL_CONTACT_DIRECTION_TRAVERSAL_SKIP_V1010:BEGIN -->
## Immediate gate — tutorial contact, facing, traversal and ESC skip

1. Compile the current Runtime changes in Unity.
2. Run `Boredom And Dungeons -> TEST EVERYTHING`; require `0 blockers / 0 warnings / 0 info`.
3. Verify one normal `ESC` press during either opening phase lands at the exact valid final state.
4. Verify attacks always follow player facing and only damage at visible contact.
5. Verify living enemies and active obstacles cannot be walked through.
6. Confirm the airborne-attack lesson uses a normal grounded enemy.
7. After the combined encounter, complete the final pre-boss wall-jump route: reachable right wall -> clearly separated left platform -> right upper ground -> boss intro.
8. Confirm every boss attack visibly shows windup pose and lane, impact flash, then recovery pose before another attack.
<!-- BND_TUTORIAL_CONTACT_DIRECTION_TRAVERSAL_SKIP_V1010:END -->

<!-- BND_FIRST_LAUNCH_TUTORIAL_RETRO_REDIRECTION_V1:BEGIN -->
## Immediate gate — tutorial retro visual redesign pass 1

1. Unity `6000.0.76f1` compilation: PASS.
2. Fresh `Boredom And Dungeons -> TEST EVERYTHING`: PASS, `0 blockers / 0 warnings / 0 info`.
3. Reset first launch and confirm there is no white boot light, then complete the full tutorial while reviewing the visible retro night scene, action silhouettes, no-walk-during-action behavior, stepped instruction presentation and block-built environment art.
4. Record remaining visual defects and continue the broader retro art, animation, UI, effects and game-feel redesign.
5. Do not mark the full tutorial redesign complete or commit before user acceptance.
<!-- BND_FIRST_LAUNCH_TUTORIAL_RETRO_REDIRECTION_V1:END -->

<!-- BND_CHAIR_BACKREST_AND_SCREEN_DELAY_V10933:BEGIN -->
## Immediate gate — V10.9.33

1. Apply the focused chair-backrest and screen-delay polish.
2. Wait for Unity compilation.
3. Run `Boredom And Dungeons -> TEST EVERYTHING` and require automated PASS.
4. Confirm the slats touch the lower rail and the rail-to-seat gap is reduced.
5. Confirm the screen remains off slightly longer after settlement before ignition.
6. Continue the active cinematic task only after visual acceptance.
<!-- BND_CHAIR_BACKREST_AND_SCREEN_DELAY_V10933:END -->

<!-- BND_CHILD_APPROACH_ROOM_SHELL_QA_AGGREGATION_REPAIR_V10932:BEGIN -->
## Immediate gate — V10.9.32

1. Apply the focused QA aggregation repair.
2. Wait for Unity compilation.
3. Run `Boredom And Dungeons -> TEST EVERYTHING`.
4. Require `PASS | blockers=0, warnings=0, info=0`.
5. Resume visual validation of the entrance start, fade, ceiling, right wall, walk, climb, device focus, and screen power-on.
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
## Current gate — V10.9.29
1. Apply V10.9.29 and wait for Unity compilation.
2. Run `Boredom And Dungeons -> TEST EVERYTHING`; retain `0/0/0`.
3. Watch from the first visible frame through the seated settle.
4. Confirm the child starts farther back and left, walks toward the chair, passes around its left side, rises outside the backrest, and only moves inward after reaching seat height.
5. Confirm there is no geometry penetration at low and high frame rates.
6. Reconfirm the screen stays off until settlement and the V10.9.28 content feed-in is unchanged.
<!-- BND_CHILD_APPROACH_CINEMATIC_PATH_CLEARANCE_V10929:END -->

<!-- BND_CHILD_APPROACH_CINEMATIC_POLISH_V10928:BEGIN -->
## Current gate — V10.9.28
1. Apply V10.9.28 and wait for Unity compilation.
2. Run `Boredom And Dungeons -> TEST EVERYTHING`; retain `0/0/0`.
3. Review the shot from the first visible frame through tutorial readiness.
4. Confirm raised behind-chair POV, correct travel direction, restrained walk, curved climb, stable final framing, dark screen until settle, and delayed professional content feed-in.
5. Verify skip still lands in the exact valid final state.
<!-- BND_CHILD_APPROACH_CINEMATIC_POLISH_V10928:END -->

<!-- BND_CHILD_APPROACH_CINEMATIC_V10927:BEGIN -->
## Current gate — V10.9.27

1. Apply V10.9.27.
2. Wait for all Unity C# compilation/import work to finish.
3. Run `Boredom And Dungeons -> TEST EVERYTHING`; require `0 blockers / 0 warnings / 0 info`.
4. Run the first-launch flow from BBH intro through tutorial readiness.
5. Verify child-height start, grounded walk, collision-free chair climb, stable final framing, fully dark screen before power-on, black-to-content power-on, seamless tutorial handoff and correct skip state.
6. Repeat at low and high frame rates before acceptance.
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
<!-- BND_CHILD_DIALOGUE_BUBBLE_POWER_TIMING_V10116:BEGIN -->
## Immediate verification — V10.11.6

Run `Boredom And Dungeons -> TEST EVERYTHING`, then replay the first-launch cinematic and verify only: the bubble is slightly lower, its tail is tangent without overlap, and the handheld starts powering on slightly sooner after the child settles. Resume the existing active tutorial repair task afterward.
<!-- BND_CHILD_DIALOGUE_BUBBLE_POWER_TIMING_V10116:END -->

<!-- BD_TUTORIAL_FINAL_INPUT_TARGET_PLAYER_V101130 -->
- Validate left click starts the real quick-attack transaction, kills the visible first enemy on impact and advances exactly once. Confirm no J/K/F/Q/E/WASD labels or readers remain.
- Validate the heavy target is recreated and killable.
- Validate the simplified player faces obstacles and focused enemies.

<!-- BND_V1011308_QUEUE -->
## Current release gate — V10.11.30.8

1. Let Unity compile the corrected lesson-screen QA scanner.
2. Run `Boredom And Dungeons -> TEST EVERYTHING`.
3. Require `blockers=0, warnings=0, info=0`.
4. Complete the manual lesson-screen/input/Parry run.
5. Run the supplied Commit command, then the separate Push command.

## V10.11.30.16 verification
<!-- BND_TUTORIAL_CONTRACT_REPAIR_V1011316 -->
Run Unity compilation, TEST EVERYTHING 0/0/0, then verify the mother dialogue and the post-lesson travel message in Play Mode.
<!-- BND V10.11.30.17 LESSON COMPLETE CONTRACT -->
- The canonical lesson-complete travel message is owned by `Gameplay.cs`, consumed by `LessonScreens.cs`, and QA reports missing contracts against the actual source path.
