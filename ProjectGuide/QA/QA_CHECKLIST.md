<!-- BND_TUTORIAL_BUBBLE_DEPTH_HORSE_CONTINUE_V1011330:BEGIN -->
## V10.11.30.30 focused acceptance matrix

- [ ] `TEST EVERYTHING` returns `0 blockers / 0 warnings / 0 info`.
- [ ] The far-left mother-bubble diamond has one larger dark backing diamond, producing a complete border on every edge.
- [ ] The bubble body, all pointer diamonds, shadows and seam remain one visual group through enter, hold and exit.
- [ ] The handheld screen RT uses `SystemInfo.GetGraphicsFormat(DefaultFormat.DepthStencil)` and `RenderTextureMemoryless.None`.
- [ ] The old explicit `GraphicsFormat.None` depthless screen contract is absent.
- [ ] A clean Play Mode entry emits neither ignored memoryless depth load nor ignored memoryless depth store message.
- [ ] HorseShot and JumpAttack retain one living shooter until the player kills it.
- [ ] HorseReturn is absent throughout scrolling and begins after settlement.
- [ ] Mounted ranged/charged impact can kill its intended target.
- [ ] Every completed edge-travel room shows exactly one non-interactive CONTINUE cue and removes it before movement.
<!-- BND_TUTORIAL_BUBBLE_DEPTH_HORSE_CONTINUE_V1011330:END -->

<!-- BND_TUTORIAL_HORSE_COMBAT_CONTINUE_V1011329:BEGIN -->
## V10.11.30.29 focused acceptance matrix

- [ ] `TEST EVERYTHING` returns `0 blockers / 0 warnings / 0 info`.
- [ ] HorseShot and JumpAttack share the same room; the shooter never disappears between them.
- [ ] The shooter is one visible living target and is removed only at the player's real melee impact.
- [ ] The HorseReturn destination room scrolls fully into place with the horse inactive.
- [ ] HorseReturn reveals the injured horse only after settlement and a short authored delay, then begins the return motion from the left.
- [ ] Mounted ordinary projectile impact uses `Ranged`; charged projectile impact uses `Charged`; the focused target dies at visible impact.
- [ ] Every room that queues edge travel hides the lesson card and shows one non-interactive pixel CONTINUE badge.
- [ ] CONTINUE uses a stepped entrance and restrained arrow pulse, does not overlap the lesson card, and remains readable without obscuring the player.
- [ ] CONTINUE disappears before continuous camera movement; no fade, respawn, teleport or early lesson appears.
<!-- BND_TUTORIAL_HORSE_COMBAT_CONTINUE_V1011329:END -->

<!-- BND_TUTORIAL_FLOW_COHERENCE_V1011328:BEGIN -->
## V10.11.30.28 focused acceptance matrix

- [ ] `TEST EVERYTHING` returns `0 blockers / 0 warnings / 0 info`.
- [ ] After clearing the opening obstacle, the player no longer faces backward toward it and responds immediately to left/right movement.
- [ ] Jump remains available in every subsequent on-foot lesson and during right-edge travel after lesson completion.
- [ ] Heal Horse transitions directly to Mount Again in the same room; no extra room, edge walk or soft-lock occurs.
- [ ] The horse hit pose is applied in the impact frame; no idle injured-horse flash appears.
- [ ] The returning horse begins from its off-screen authored start and runs in without first appearing at room center.
- [ ] Wall-jump wall/platforms are visible only in Wall Jump.
- [ ] Finish gate is visible only in the boss/finish context and never blocks Heal/Remount or ordinary room travel.
- [ ] Before every room scroll starts, target-room static props, ordinary actors, hazards and geometry are configured; HorseReturn alone remains hidden until post-settlement reveal.
- [ ] No static room asset pops in after the camera has settled.

<!-- BND_TUTORIAL_FLOW_COHERENCE_V1011328:END -->

<!-- BND_TUTORIAL_HORSE_FREE_OPENING_PET_SUPPRESSION_V1011327:BEGIN -->
## V10.11.30.27 focused acceptance matrix

- [ ] `TEST EVERYTHING` returns `0 blockers / 0 warnings / 0 info`.
- [ ] WhiteBoot, Move and Jump never activate the tutorial horse or horse pixel visual.
- [ ] Jump landing queues Quick Attack, not MountHorse.
- [ ] Room order is Move/Jump -> Quick Attack -> Heavy Attack -> Dodge -> Parry -> Mount/Ride -> EnemyArrival/HorseShot -> Jump Attack.
- [ ] The deferred Mount room positions the player left of a reachable horse and Mount guidance remains until the real mount completes.
- [ ] The horse-shot story advances to Jump Attack and never reopens Quick Attack.
- [ ] The upper-right `PET` card and all other full-game horse prompts remain absent during the tutorial and every continuous handoff.
<!-- BND_TUTORIAL_HORSE_FREE_OPENING_PET_SUPPRESSION_V1011327:END -->

<!-- BND_TUTORIAL_CENTERED_PARRY_HORSE_METAL_V1011326:BEGIN -->
## V10.11.30.26 focused acceptance matrix

- [ ] `TEST EVERYTHING` returns `0 blockers / 0 warnings / 0 info`.
- [ ] Opening room order is: room 0 Move/Jump, room 1 Mount/Ride, room 2 EnemyArrival/HorseShot, room 3 Quick Attack.
- [ ] The horse tutorial is fully completed before entering the horse-shot story room.
- [ ] Quick Attack room starts on foot with the horse hidden and exactly one passive enemy at the exact visible center.
- [ ] The centered enemy reads as a full 64×92 pixel character rather than a tiny prop, projectile or background mark.
- [ ] One correct Light impact kills the target at visible contact; wrong mechanics do not complete the lesson.
- [ ] The Quick Attack card disappears after death and physical right-edge contact owns the next continuous handoff.
- [ ] Parry teacher remains active and alive when struck by Light, Heavy, ranged or other unlocked attacks.
- [ ] Parry room never has both production and focused tutorial projectiles active.
- [ ] Successful Parry atomically hides/cancels all projectiles, completes the animation, queues Jump Attack and never soft-locks.
- [ ] Device camera has MSAA disabled and requests no depth texture; screen RenderTexture remains depthless, single-sampled and non-memoryless.
- [ ] Fresh Unity/Metal scene reload emits neither `Ignoring depth surface load action as it is memoryless` nor the matching store-action message.
<!-- BND_TUTORIAL_CENTERED_PARRY_HORSE_METAL_V1011326:END -->

<!-- BND_TUTORIAL_CONTINUOUS_ROOM_SEQUENCE_V1011325:BEGIN -->
## V10.11.30.25 continuous remaining-room acceptance matrix

- [ ] `TEST EVERYTHING` returns `0 blockers / 0 warnings / 0 info`.
- [ ] Room centers are ordered monotonically across the complete tutorial course and all room-local objectives are reachable.
- [ ] Completing a lesson hides the entire instruction card but does not advance immediately.
- [ ] The completed room camera remains fixed while the player walks to its visible right edge.
- [ ] The next room begins only after physical edge contact.
- [ ] Every handoff uses continuous camera/player movement; no transition overlay, fade, black/white frame, respawn or teleport is visible.
- [ ] The next lesson instruction appears only after the camera finishes settling in the new room.
- [ ] The next room's step timer, enemy action delay, reload timer and input unlock begin after settlement.
- [ ] Heavy → Dodge → Parry → Jump Attack rooms each require their own real mechanic result and edge handoff.
- [ ] Horse return/heal → Remount → Ranged → Reload → Charged Shot → Mounted Impact → Dismount rooms retain correct mounted state and edge handoffs.
- [ ] Spin → Grapple → Hazard Knockback → Side Path → Combined Encounter → Wall Jump rooms retain visible, reachable objective geometry and edge handoffs.
- [ ] Wall Jump completion can reach the visible room edge after landing on upper ground.
- [ ] Boss phases stay in one boss room; after real boss death the collectible room is revealed continuously.
- [ ] Relic contact persists `Completed` and no post-completion room handoff is introduced.
<!-- BND_TUTORIAL_CONTINUOUS_ROOM_SEQUENCE_V1011325:END -->

<!-- BND_TUTORIAL_SCREEN_TWO_IMPACT_CONTINUOUS_HANDOFF_V1011324:BEGIN -->
## V10.11.30.24 screen-two focused acceptance

- [ ] `TEST EVERYTHING` returns `0 blockers / 0 warnings / 0 info`.
- [ ] The centered screen-two target has one health and remains passive.
- [ ] A valid ordinary Light attack reaches visible impact and kills the target in that same frame.
- [ ] The Light impact is accepted as damage source `Light`; source `None` cannot reject it.
- [ ] The instruction disappears after the kill.
- [ ] The player remains at the same world position when crossing from screen two into screen three.
- [ ] No white/black overlay, fade, respawn, teleport or hard camera cut occurs during that handoff.
- [ ] Screen three is revealed by continuous camera/world motion and its instruction becomes visible only after the camera settles.
<!-- BND_TUTORIAL_SCREEN_TWO_IMPACT_CONTINUOUS_HANDOFF_V1011324:END -->

<!-- BND_TUTORIAL_SECOND_SCREEN_LIGHT_ATTACK_V1011323:BEGIN -->
## V10.11.30.23 focused screen-two acceptance

- [ ] `TEST EVERYTHING` returns `0 blockers / 0 warnings / 0 info`.
- [ ] Ride completion queues `AttackEnemy` directly for screen two; EnemyArrival and HorseShot do not appear first.
- [ ] Screen two starts with the player on foot at the left and the horse hidden.
- [ ] Exactly one passive one-health Small enemy is centered on screen two.
- [ ] The ordinary-attack tutorial is fully populated and visible immediately on entry.
- [ ] Walking into range and performing one ordinary Light Attack kills the enemy at the visible melee impact.
- [ ] Enemy death hides the entire current lesson card immediately.
- [ ] The camera remains fixed on screen two while the player walks after the kill.
- [ ] Screen three transition begins only at the visible right edge.
- [ ] No screen-three or later mechanic is evaluated as part of this patch.
<!-- BND_TUTORIAL_SECOND_SCREEN_LIGHT_ATTACK_V1011323:END -->

<!-- BND_TUTORIAL_OPENING_SCREEN_SEQUENCE_V1011322:BEGIN -->
## V10.11.30.22 opening-screen acceptance matrix

- [ ] Move instruction is populated and visible when the first screen begins.
- [ ] Move completes only after 64 units of real forward displacement.
- [ ] Holding left at the world minimum or pushing against the obstacle does not accumulate Move completion.
- [ ] Move → Jump remains on the same screen; the Move card disappears and the populated Jump card enters without a respawn or black screen transition.
- [ ] Jump completes only after the player clears the obstacle and lands beyond `TutorialJumpObstacleX + 24f`.
- [ ] Jump → Mount remains on the same screen and positions the player within real mount range of the horse.
- [ ] Mount → Ride occurs only when the mount animation completes.
- [ ] Ride remains visible while mounted movement is being learned.
- [ ] At the Ride objective, Ride disappears and the player can still reach the next-screen transition point.
- [ ] Exactly one screen transition occurs after Ride; no later lesson behavior is changed by this patch.
<!-- BND_TUTORIAL_OPENING_SCREEN_SEQUENCE_V1011322:END -->

<!-- BND_SCREEN_RENDER_SCHEDULING_V1011321:BEGIN -->
## V10.11.30.21 screen-render scheduling gate

- [ ] Runtime contains no direct `screenCamera.Render()` call.
- [ ] `ForceScreenRender` still calls `Canvas.ForceUpdateCanvases()` and relies on the already-enabled screen camera's normal render schedule.
- [ ] The screen RT remains depthless with `GraphicsFormat.None`, `RenderTextureMemoryless.None`, MSAA 1 and `DepthTextureMode.None`.
- [ ] Project framebuffer memoryless mode remains `Unused` (`framebufferDepthMemorylessMode: 0`).
- [ ] Scene restore/open and first Play Mode entry emit no memoryless depth load/store pair.
- [ ] Child screen power-on, content feed-in, menu refresh and tutorial entry show no blank frame or pop.
- [ ] `TEST EVERYTHING` reports `0 blockers / 0 warnings / 0 info`.
<!-- BND_SCREEN_RENDER_SCHEDULING_V1011321:END -->

<!-- BND_TUTORIAL_QA_THRESHOLD_REALIGNMENT_V1011320:BEGIN -->
## V10.11.30.20 focused QA-contract gate

- [ ] `BDTutorialOpeningPolishV1011QA` requires `firstLaunchTutorialTravelDistance >= 64f`.
- [ ] The same scanner forbids retired `firstLaunchTutorialTravelDistance >= 12f`.
- [ ] Runtime `Gameplay.cs` remains byte-identical to installed V10.11.30.19.
- [ ] `TEST EVERYTHING` reports `0 blockers / 0 warnings / 0 info`.
- [ ] Neither `TUTORIAL_V101111_FORWARD_COURSE_MISSING` nor `TUTORIAL_V101113_START_OR_TRIGGER_REGRESSION` appears.
- [ ] Full V10.11.30.19 Play Mode verification remains pending and unchanged.
<!-- BND_TUTORIAL_QA_THRESHOLD_REALIGNMENT_V1011320:END -->

<!-- BND_TUTORIAL_RUNTIME_INTEGRITY_V1011319:BEGIN -->
## V10.11.30.19 tutorial runtime-integrity acceptance matrix

- [ ] Move remains active through a small tap and completes only after at least 64 world units of actual travel.
- [ ] Unity `6000.0.76f1` compiles with no error or warning.
- [ ] `TEST EVERYTHING` reports `0 blockers / 0 warnings / 0 info`, including `BDTutorialRuntimeIntegrityV1011319QA`.
- [ ] A fresh Editor/Play Mode entry emits neither `Ignoring depth surface load action as it is memoryless` nor `Ignoring depth surface store action as it is memoryless`.
- [ ] Mother bubble root remains at `(72,-108)` and its shadow/fill/far-tip point left as one joined shape during the whole animation.
- [ ] Completing Move removes every instruction-card component and leaves only the external `LESSON COMPLETE — MOVE RIGHT TO THE NEXT SCREEN` message.
- [ ] Reaching the exit fades to full dark, holds full dark while Jump is configured, then reveals a populated Jump card; no respawn overlay, label or visible teleport occurs.
- [ ] The same hide/travel/dark-hold/new-card sequence works for every non-exempt lesson transition.
- [ ] The retired station-travel gate never emits `NEW LESSON AREA` or hides a card after lesson-screen initialization.
- [ ] Move -> Jump -> Mount -> Ride remains the scoped first-screen chain; later mechanic lessons retain separate-screen ownership.
- [ ] Completed-screen actors, projectiles and hazards are inactive during exit travel and cannot damage, collide with or target the player.
- [ ] WASD and arrows both move; double-tap A/D and double-tap arrows both submit Dodge.
- [ ] E/Controller East/physical SELECT all perform Interact where taught.
- [ ] J or left mouse performs Light; hold J or left mouse performs Spin only after Spin unlock.
- [ ] K or right mouse performs Heavy; hold K or right mouse performs Grapple only after Grapple unlock.
- [ ] Q/RB/physical A fires ranged; holding the same route charges. Left mouse never charges ranged.
- [ ] Holding F/LB/physical A heals only in the healing context.
- [ ] Jump Attack presents and requires Jump + Light; Parry completes only after a timed Light/Heavy parry.
- [ ] Attack lessons advance only after the correct target dies at the authored impact; obstacle lessons advance only after the player actually clears/crosses them.
- [ ] The uninterrupted run reaches boss death, contact relic collection and persisted `Completed` without a progression lock.
<!-- BND_TUTORIAL_RUNTIME_INTEGRITY_V1011319:END -->

<!-- BND_DIALOGUE_SCOPE_COMPILE_REPAIR_QA_V1011315 -->
## V10.11.30.15 focused gate

- Unity compilation has no CS0103 errors in `ChildApproachDialogue.cs`.
- The mother bubble is slightly lower.
- The dialogue still enters, holds, exits and plays its voice once.
- TEST EVERYTHING reports `blockers=0, warnings=0, info=0` before Commit.

<!-- BND_TUTORIAL_FLOW_JUMP_CINEMATIC_QA_V1011314 -->
## V10.11.30.14 focused Play Mode gate

- Complete Move: the entire card, progress and shadow disappear; the external move-right instruction remains.
- Cross the nearby right boundary: the Jump screen appears with populated title, detail and binding card.
- Press Space, controller South and the physical Jump button: each starts a real jump after unlock.
- Clear the root before the Jump lesson completes.
- Replay the intro: mother bubble is lower; child POV stays higher through walking and chair climbing; no snap occurs at menu handoff.

<!-- BND_TUTORIAL_FINAL_QA_ZIP_CLEANUP_V1011310:BEGIN -->
## V10.11.30.10 QA

- [ ] Unity compiles without errors.
- [ ] TEST EVERYTHING reports 0/0/0.
- [ ] The impact-owned melee lesson completion contract is read from LessonScreens.cs.
- [ ] The installer source ZIP and stale Boredom-and-Dungeons_Tutorial_*.zip delivery packages are absent after both success and simulated failure.
- [ ] Full tutorial and child-menu-camera manual checks pass.
<!-- BND_TUTORIAL_FINAL_QA_ZIP_CLEANUP_V1011310:END -->

<!-- BND_TUTORIAL_QA_SEMANTIC_CAMERA_HEIGHT_V1011309:BEGIN -->
## V10.11.30.9 manual QA

- [ ] TEST EVERYTHING reports 0/0/0; none of the nine obsolete-token blockers remains.
- [ ] Child POV is visibly but subtly higher throughout approach and climb.
- [ ] No vertical snap occurs when the camera settles into the regular menu pose.
- [ ] Existing cinematic timing, chair clearance, device focus and power-on framing remain unchanged.
- [ ] Lesson-screen, input, world-proof objective, Parry and mounted-impact checks still pass.
<!-- BND_TUTORIAL_QA_SEMANTIC_CAMERA_HEIGHT_V1011309:END -->

<!-- BND_V1011307_QA -->
## V10.11.30.7 verification

- No invalid GUID or malformed `.meta` warning for the lesson-screen runtime
  partial or its QA scanner.
- Unity Console has no C# compiler errors.
- TEST EVERYTHING reports `0 blockers, 0 warnings, 0 info`.
- Each mechanic lesson starts on a separate screen except
  Jump → Mount Horse → Ride Horse.
- The current tutorial disappears at objective completion; the next tutorial
  appears only after entering the next screen.
- Left mouse is Light/Spin only; Q is Ranged only; E is Interact.
- The Parry enemy is passable, normally proportioned, and the lesson completes
  only after a valid projectile deflection.

<!-- BND_TUTORIAL_LESSON_SCREENS_INPUT_PARRY_V1011306:BEGIN -->
## V10.11.30.6 manual QA

- [ ] After the opening Move -> Jump -> Mount -> Ride chain, every later mechanic lesson starts after a dark transition on a separate screen.
- [ ] Jump -> mount -> ride is the only lesson sequence allowed to share one screen.
- [ ] Current lesson instruction disappears immediately when its real objective completes.
- [ ] The next lesson instruction appears only after entering the next screen.
- [ ] Enemies, obstacles and other lesson targets are centered by default.
- [ ] Left click/J Light; hold Spin only after Spin unlock.
- [ ] Right click/K Heavy; hold Grapple only after Grapple unlock.
- [ ] Q Ranged, E Interact, F Heal; WASD and arrows both move.
- [ ] Timed Light or Heavy parries; parry target is passable and visually unsquashed.
- [ ] Attack lessons require target death; Dodge requires crossing the hazard.
- [ ] TEST EVERYTHING reports 0/0/0 and a full tutorial run completes.
<!-- BND_TUTORIAL_LESSON_SCREENS_INPUT_PARRY_V1011306:END -->

<!-- BND_TUTORIAL_INPUT_MECHANICS_MOUNTED_IMPACT_V1011305:BEGIN -->
## V10.11.30.5 focused acceptance

- [ ] TEST EVERYTHING: 0 blockers / 0 warnings / 0 info.
- [ ] A/D and Left/Right move; W/S and Up/Down remain registered across supported input backends.
- [ ] E interact and Q ranged work before, during and after their highlighted lesson.
- [ ] Tap Primary performs Light; hold Primary performs Spin without first playing Light.
- [ ] Tap Heavy performs Heavy; hold Heavy performs Grapple without first playing Heavy.
- [ ] Mouse, controller and physical buttons follow the same action paths.
- [ ] Performing a mechanic outside its lesson affects the world but does not skip progression.
- [ ] Mounted Impact reaches the visible target by contact and cannot stop at an invisible wall.
- [ ] Complete one uninterrupted tutorial run.
<!-- BND_TUTORIAL_INPUT_MECHANICS_MOUNTED_IMPACT_V1011305:END -->

<!-- BND_TUTORIAL_PLAYER_CANONICAL_ASSET_NAME_V1011304:BEGIN -->
## V10.11.30.4 player asset-name verification

- [ ] `TEST EVERYTHING` returns `0 blockers / 0 warnings / 0 info`.
- [ ] Runtime contains exactly one `B&D Tutorial Player Simple Right Facing Sprite`.
- [ ] No validator or Runtime file retains `B&D Tutorial Player Simple Right Facing Idle`.
- [ ] The player remains visible in Idle, Walk A/B and Action A/B after alignment.
<!-- BND_TUTORIAL_PLAYER_CANONICAL_ASSET_NAME_V1011304:END -->

<!-- BND_TUTORIAL_PLAYER_VISIBILITY_RUNTIME_V1011303:BEGIN -->
## V10.11.30.3 player-visibility verification

- [ ] `TEST EVERYTHING` is `0/0/0`.
- [ ] Player is visible on the first gameplay frame.
- [ ] Pixel child remains active while the source Image remains disabled by design.
- [ ] Idle, two walk frames and two action frames are visible.
- [ ] Left/right movement flips the complete player through the parent transform.
- [ ] No articulated-player child model returns.
<!-- BND_TUTORIAL_PLAYER_VISIBILITY_RUNTIME_V1011303:END -->

<!-- BND_TUTORIAL_QA_CONTRACT_REALIGNMENT_V1011302:BEGIN -->
## V10.11.30.2 QA contract acceptance

- [ ] Editor assembly compiles after validator realignment.
- [ ] TEST EVERYTHING returns `0 blockers / 0 warnings / 0 info`.
- [ ] No validator requires `new Vector2(82f, 118f)` or superseded exact player palette literals.
- [ ] No validator requires the retired V10.11.17 player identity/sprite markers.
- [ ] Current player checks require composite-child disablement, the simple right-facing sprite marker and positive-X facing authorship.
- [ ] V10.11.30.1 Runtime files remain byte-identical during this QA-only repair.
<!-- BND_TUTORIAL_QA_CONTRACT_REALIGNMENT_V1011302:END -->

<!-- BND_TUTORIAL_FINAL_INPUT_COMBAT_PLAYER_V1011301:BEGIN -->
## V10.11.30.1 focused acceptance matrix

- [ ] No `RightArrowownArrow` compile error and no obsolete world-mouse `CS0414` warning.
- [ ] Arrow-only movement/entry branches work alongside controller, physical controls, mouse and touch.
- [ ] First Light miss does not progress; valid visible impact kills the registered actor and advances.
- [ ] Heavy follows the same impact-owned actor transaction.
- [ ] Player is one clean side-profile sprite and flips correctly in both directions.
- [ ] Spin requires both opposite-side enemies in one attack.
- [ ] Dodge requires landing on the opposite side of the obstacle.
- [ ] Full tutorial reaches persisted `Completed`; TEST EVERYTHING is `0/0/0`.
<!-- BND_TUTORIAL_FINAL_INPUT_COMBAT_PLAYER_V1011301:END -->

## V10.11.28 atomic lesson acceptance
- [ ] First quick-attack enemy dies and disappears at the correct hit frame.
- [ ] Wrong mechanics cannot damage focused lesson enemies.
- [ ] Spin touching one enemy kills neither; one spin touching both kills both together.
- [ ] Dodge input alone does not complete; the player exits beyond the obstacle.
- [ ] Prompt/detail/progress/feedback/binding text retain distinct colors.
- [ ] Articulated player model is visible during movement, combat, riding and respawn.

## V10.11.26 focused Play Mode matrix

- [ ] Quick-attack target dies only when the quick attack connects.
- [ ] Air-attack target dies only when the airborne quick attack connects.
- [ ] Heavy target dies only when the heavy impact connects.
- [ ] Spin group dies from the completed spin hit.
- [ ] Grapple target survives the pull, then dies from a previously unlocked attack.
- [ ] Hazard target dies only after entering the hazard.
- [ ] Mounted-shot target dies only on projectile impact.
- [ ] Charged-shot target dies only on charged projectile impact.
- [ ] Mounted-impact target dies only from the horse collision.
- [ ] A confirmed hit from one lesson cannot auto-complete the next lesson.
- [ ] Mini-boss and combined combat retain multi-hit health.

<!-- BD ALL LESSON TARGETS LETHAL V10.11.26 -->

# BD TUTORIAL LESSON ENTRY + DAMAGE OWNERSHIP V10.11.25

- Confirm the child starts with a clear runway before the first root.
- Confirm lesson text appears on step/room entry and does not disappear before success.
- Confirm the mechanic is available immediately.
- Confirm pressing/using it without hitting the target does not complete the lesson.
- Confirm other unlocked mechanics animate and consume resources normally but cannot damage the focused lesson target.
- Confirm MountedImpact target dies only from horse collision and the gate releases afterward.
- Confirm Grapple pulls the enemy, changes the prompt to finish it, and advances only after that enemy dies.
- Confirm no missing _MainTex warning and no handheld screen depth load/store warning.

## BD V10.11.24 MOUNTED RANGED SEQUENCE AND NO MOUNTED DODGE
- [ ] TEST EVERYTHING reports blockers=0 and warnings=0.
- [ ] RangedAttack -> Reload -> ChargedShot -> MountedImpact completes without a soft lock.
- [ ] Both impact-before-reload and reload-before-impact orders advance exactly once.
- [ ] Ordinary and charged misses remain retryable.
- [ ] No mounted input path triggers dodge animation or invulnerability.

## BD V10.11.23 LESSON PERSISTENCE AND PROGRESSION GATE
- [ ] TEST EVERYTHING reports blockers=0 and warnings=0.
- [ ] Every introduced instruction stays visible until its verified success path runs.
- [ ] Forward traversal cannot pass the current lesson boundary before success.
- [ ] Backtracking remains possible and no visible divider line is added.
- [ ] Full tutorial reaches completion without a hard lock.

## BD V10.11.22.2 QA CONTRACT RECONCILIATION
- [ ] TEST EVERYTHING reports blockers=0, warnings=0, info=0.
- [ ] Power-on constants are exactly 9.20 and 10.20 seconds.
- [ ] Keyboard/mouse and physical-handheld cards are both visible with OR divider.
- [ ] Heavy attack works with K, right mouse and physical Y.
- [ ] True reveal hides all content below the moving frontier.

<!-- BND_TUTORIAL_INPUT_PARITY_POWER_REVEAL_V101122:BEGIN -->
## V10.11.22 complete acceptance matrix

- [ ] `TEST EVERYTHING` returns `0 blockers / 0 warnings / 0 info`.
- [ ] Movement: WASD/arrows, gamepad, physical D-pad and display-side mouse hold work.
- [ ] Jump/wall jump: Space, gamepad south, physical B and contextual display click work.
- [ ] Interaction/mount/dismount: E, gamepad east, physical Select and contextual display click work.
- [ ] Light/jump attack: J, gamepad west, physical X and left/context display click work exactly once.
- [ ] Heavy attack: K, gamepad north, physical Y, right mouse and contextual display click work.
- [ ] Dodge: keyboard/gamepad/physical double-tap and double-clicking one display side work.
- [ ] Parry: light or heavy keyboard/gamepad/physical input and either display mouse attack work.
- [ ] Heal: hold F, gamepad LB, physical A and contextual display hold complete only at 100%.
- [ ] Spin: hold J, gamepad west, physical X and contextual left hold complete only at 100%.
- [ ] Grapple: hold K, gamepad north, physical Y and display right/context hold complete only at 100%.
- [ ] Ranged/charged: Q/RB/physical A and display middle/context hold preserve auto-fire behavior.
- [ ] Combined encounter and boss accept light, heavy and ranged input without duplicate same-frame actions.
- [ ] Binding cards are centered, professional, readable and never overlap screen content.
- [ ] Screen power begins immediately after settled framing but keeps its original reveal duration.
- [ ] The power line is a true mask; unrevealed content stays black until the line crosses it.
<!-- BND_TUTORIAL_INPUT_PARITY_POWER_REVEAL_V101122:END -->

<!-- BND_SMOOTH_DRIP_TUTORIAL_MOUSE_V101121:BEGIN -->
## V10.11.21 focused acceptance matrix

- [ ] `TEST EVERYTHING` returns `0 blockers / 0 warnings / 0 info`.
- [ ] The BBH screen before the melt is visually identical to the previous accepted screen.
- [ ] The room, camera, dialogue, device and tutorial are unchanged by the melt implementation.
- [ ] Melt motion is downward and continuous, with broad overlapping rounded lobes and no vertical strips, block steps or pixelated edge.
- [ ] The real scene is visible behind the melting captured BBH frame with no extra black fade or left-side glitch.
- [ ] During `AttackEnemy`, one left click on the physical display causes exactly one ordinary attack.
- [ ] A click outside the display or on hardware buttons does not create a world attack.
- [ ] Damage and progression occur only at the existing visible melee impact.
- [ ] Mount, riding, ranged, reload, charged shot, grapple, wall jump, boss and relic triggers still complete in order.
<!-- BND_SMOOTH_DRIP_TUTORIAL_MOUSE_V101121:END -->

<!-- BND_TUTORIAL_QA_CONTRACT_RECOVERY_V1011203:BEGIN -->
## V10.11.20.3 focused regression matrix

- [ ] `TEST EVERYTHING` returns `0 blockers / 0 warnings / 0 info`.
- [ ] Left click inside the tutorial world performs exactly one ordinary light attack at the AttackEnemy lesson.
- [ ] Clicking handheld navigation/face controls does not also emit a world light attack.
- [ ] Ability locking still prevents light attack before its lesson.
- [ ] MountHorse guidance stays visible until the mount animation completes successfully.
- [ ] Successful mount advances exactly once to RideHorse; failed/out-of-range interact does not advance.
- [ ] Keyboard/mouse keycaps and physical handheld illustration remain inside their cards.
- [ ] Full tutorial trigger sequence reaches completion with no stuck, skipped or prematurely hidden instruction.
<!-- BND_TUTORIAL_QA_CONTRACT_RECOVERY_V1011203:END -->

<!-- BND_TUTORIAL_INDIE_BINDING_VISUALS_HOTFIX_V101118:BEGIN -->
## V10.11.18 indie binding visual hotfix

- [ ] TEST EVERYTHING no longer reports `TUTORIAL_V101117_INDIE_BINDING_VISUALS_MISSING`.
- [ ] `PHYSICAL HANDHELD` is visible above the illustrated D-pad/button control.
- [ ] Desktop/controller and handheld visuals remain inside the instruction panel.
<!-- BND_TUTORIAL_INDIE_BINDING_VISUALS_HOTFIX_V101118:END -->

<!-- BND_OPENING_TUTORIAL_RECOVERY_V101117:BEGIN -->
## V10.11.17 opening and tutorial recovery

- [ ] BBH artwork drips in 32 clipped strips for about 0.62 seconds; the room never owns or repeats the effect.
- [ ] No left-edge line, black frame, fade re-arm, or camera-start glitch appears.
- [ ] Mother bubble text is exactly `Sweety, where are you?`; it begins before movement, remains into the first steps, and exits while walking.
- [ ] Move completes, Jump unlocks, a valid landing advances to MountHorse beside the horse, Select/E mounts, and RideHorse starts.
- [ ] No ability works before its own lesson; the expected keyboard/mouse and physical control work immediately after unlock.
- [ ] Tutorial text appears and disappears with the correct step/trigger throughout one complete run.
- [ ] Player art is visibly blond/red/blue after every renderer and does not erase hit/death feedback.
- [ ] Keyboard/mouse or controller input is presented as one colored keycap; the physical handheld side is a drawn D-pad/button/Select image, including B+X for airborne attack.
- [ ] `Pet [Tab]` and other full-game HUD prompts are absent from the handheld tutorial.
- [ ] Supplied application icon persists after an Editor restart and appears in a Standalone build.
<!-- BND_OPENING_TUTORIAL_RECOVERY_V101117:END -->

<!-- BND_TUTORIAL_DRIP_CONTRACT_BINDING_HOTFIX_V101116:BEGIN -->
## V10.11.16 Play Mode checks

- BBH drip occurs on the intro surface, not on the first child/menu frame.
- No left-side strip, black flash, or delayed drip remains after the BBH overlay releases.
- Every actionable tutorial instruction shows keyboard/controller and physical handheld cards together.
- Non-action informational steps hide both cards and the divider cleanly.
<!-- BND_TUTORIAL_DRIP_CONTRACT_BINDING_HOTFIX_V101116:END -->

<!-- BND_TUTORIAL_QA_COMPILATION_HOTFIX_V101115:BEGIN -->
## V10.11.15 compilation check

- No `CS1010`, `CS1003`, or `CS1026` errors remain in `BDTutorialOpeningPolishV1011QA.cs`.
- `TEST EVERYTHING` runs after compilation instead of stopping at `UNITY_SCRIPT_COMPILATION_FAILED`.
<!-- BND_TUTORIAL_QA_COMPILATION_HOTFIX_V101115:END -->

<!-- BND_TUTORIAL_DRIP_MOUNT_INPUT_BINDINGS_V101114:BEGIN -->
## V10.11.14 Play Mode checks

- BBH transition: the intro artwork itself visibly drips; no black flash, isolated strip, or delayed drip appears at child-camera start.
- Jump to Mount: land after the first obstacle and see the Mount Horse lesson immediately while within interaction range.
- Quick Attack: `J`, world-space left click, controller West and physical `X` each execute one equivalent light attack.
- Binding UI: both input cards remain visible together and fit inside the tutorial panel on every lesson.
<!-- BND_TUTORIAL_DRIP_MOUNT_INPUT_BINDINGS_V101114:END -->

<!-- BND_TUTORIAL_TRIGGER_UNLOCK_HUD_COLLISION_HOTFIX_V101113:BEGIN -->
## V10.11.13 Play Mode checks

- Opening: no isolated left-edge drip strip; no child reply bubble; mother line is exactly `Sweety, where are you?`.
- Tutorial HUD: no Mount/Heal/Pet cards outside the physical handheld presentation.
- Move -> Jump: Move prompt completes before the obstacle; Jump prompt appears at the obstacle; Jump input was blocked before this point.
- Ability sequence: interact, light, heavy, dodge, parry and ranged actions are blocked until their own lesson and immediately functional afterward.
- Prompt lifecycle: each prompt appears at the correct trigger and disappears when its lesson is complete or when travel to the next station begins.
- Collision: the player cannot cross a living visible enemy from either direction; defeated/hidden enemies no longer block.
<!-- BND_TUTORIAL_TRIGGER_UNLOCK_HUD_COLLISION_HOTFIX_V101113:END -->

<!-- BND_TUTORIAL_QA_DIALOGUE_HOTFIX_V101112:BEGIN -->
## V10.11.12 opening hotfix checks

- [ ] `TEST EVERYTHING` reports `0 blockers / 0 warnings / 0 info`.
- [ ] No `CHILD_APPROACH_CINEMATIC_CONTRACT_INVALID` finding remains.
- [ ] No `V23R19Q_BOOT_POLISH_MISSING` finding remains.
- [ ] Mother bubble text is exactly `Sweety, where are you?` including comma and question mark.
- [ ] No opening timing or drip behavior regressed.
<!-- BND_TUTORIAL_QA_DIALOGUE_HOTFIX_V101112:END -->

<!-- BND_TUTORIAL_FINAL_PRODUCTION_COURSE_V101111:BEGIN -->
## V10.11.11 acceptance matrix

- [ ] No compiler error/warning and automated QA reports `0 / 0 / 0`.
- [ ] BBH intro exits through a fast downward drip with the room already behind it.
- [ ] Mother bubble clearly points left/off-screen; child bubble appears above the child, says `רק שניה`, uses child voice, and exits before walking.
- [ ] Major lessons are spatially separated by forward travel.
- [ ] Future wall/platform/enemies are prepared off-screen; nothing pops in or vanishes while visible.
- [ ] Camera never scrolls backward and player cannot return to an earlier completed screen.
- [ ] Player is visibly blond/yellow-haired with red shirt and blue trousers.
- [ ] Every tutorial text owner is larger, colored, outlined, animated and remains inside its panel.
- [ ] Ground attack is straight horizontal; airborne attack swings from above to below.
- [ ] Melee/projectile damage occurs only when visual impact reaches a valid target.
- [ ] Player and every damaged enemy show a short hit flicker/jitter.
- [ ] Boss fires exactly three vertically fanned projectiles at range.
- [ ] Close boss slash and jump-slam are telegraphed on the model and show exact map-space ranges.
- [ ] Ordinary boss shot works; charged shot still auto-fires at full charge/recovery.
- [ ] Keyboard/mouse actions visibly press the equivalent physical handheld button.
- [ ] Player lifts relic overhead before magical light starts.
- [ ] Light originates from relic, fills screen, then fades to the real main menu.
<!-- BND_TUTORIAL_FINAL_PRODUCTION_COURSE_V101111:END -->

<!-- BND_TUTORIAL_PLAYER_TEXT_BOSS_ENVIRONMENT_V101110:BEGIN -->
## V10.11.10 focused acceptance matrix

- [ ] Unity compile emits no CS0114 warning from `BDTutorialLetterPulseEffect`.
- [ ] `TEST EVERYTHING` returns `0 blockers / 0 warnings / 0 info`.
- [ ] Player begins at `(-900, -108)` and is not visually attached to the first obstacle.
- [ ] Player sprite has blond hair, natural skin, red shirt, blue trousers and one correctly placed profile eye.
- [ ] Prompt, detail, progress, feedback, keyboard title/value, handheld title/value, divider, health, ammo and boss text all use modern color/outline/per-letter motion.
- [ ] All text uses best-fit with wrap and vertical truncation; no text leaves its assigned panel or card.
- [ ] Environment attack keeps enemy and hazard visible during travel, shows impact, then advances after `0.92s` rather than deleting both immediately.
- [ ] Ordinary boss shot targets the live boss and damages at the visible endpoint even if the boss is winding up.
- [ ] Charged boss shot still waits for its valid recovery behavior and is not regressed by ordinary-shot support.
- [ ] Final instruction says `COLLECT THE GREEN RELIC` and explains contact collection.
- [ ] Final green relic is visually outlined, highlighted and pulsing, not mistaken for background decoration.
- [ ] Contact with the relic completes and persists the tutorial; interact is not required.
<!-- BND_TUTORIAL_PLAYER_TEXT_BOSS_ENVIRONMENT_V101110:END -->

<!-- BND_TUTORIAL_WALLJUMP_BOSS_TYPOGRAPHY_DIALOGUE_V10118:BEGIN -->
## V10.11.8 focused acceptance matrix

- [ ] `TEST EVERYTHING` returns `0 blockers / 0 warnings / 0 info`.
- [ ] Dialogue body, tail shadow, tail fill and seam cover share one animated parent and one CanvasGroup alpha.
- [ ] Tail is behind the panel, visually joined, and no black border crosses the joint.
- [ ] Enter and exit never reveal body/tail timing separation.
- [ ] One airborne wall contact can trigger only one wall jump; landing resets eligibility.
- [ ] Platform standing baseline is `8`, upper-ground standing baseline is `64`, and the character does not sink.
- [ ] Rightward platform jump clears the wall and lands inside the upper-ground range.
- [ ] The right-side wall clamp never pulls a player back after the player has crossed the wall center.
- [ ] Instruction headline is 58px authored size, panel minimum is 840×258, and bindings/detail/feedback are enlarged.
- [ ] Per-character color/wave/entrance animation refreshes after every instruction text change without harming readability.
- [ ] Boss slam presents a ground warning zone and local impact flash; no full-length generic beam remains.
- [ ] Boss projectile windup presents a compact charge orb, then launches the real enemy projectile transaction.
- [ ] Phase one reaches projectile attack sequence; phase two begins with a projectile and alternates thereafter.
- [ ] Holding ranged in either boss phase charges; reaching 100% does not require release and waits for recovery before auto-fire.
- [ ] Charged boss damage resolves only at the player projectile endpoint and obeys boss recovery vulnerability.
- [ ] Mounted ordinary shot remains single-target and damage still resolves only at visible projectile impact.
- [ ] Full tutorial reaches upper ground, boss defeat, collectible and persisted `Completed`.
<!-- BND_TUTORIAL_WALLJUMP_BOSS_TYPOGRAPHY_DIALOGUE_V10118:END -->

<!-- BND_TUTORIAL_COMPLETION_INTEGRITY_V10117:BEGIN -->
## V10.11.7 focused regression and completion matrix

- [ ] `TEST EVERYTHING` returns `0 blockers / 0 warnings / 0 info`.
- [ ] Bubble diamond is a root sibling before the panel in draw order, visually behind the body, attached at all animation scales and never drawn over the text panel.
- [ ] Horse faces right while moving right and flips left while moving left/backward; mount, dismount, recoil and hit poses preserve facing.
- [ ] Mounted-shot lesson contains exactly one active target.
- [ ] Projectile position advances continuously rather than in six stepped jumps.
- [ ] No target health or active state changes before projectile endpoint contact.
- [ ] One ordinary mounted shot cannot kill or hide a second enemy.
- [ ] Hidden/inactive actors do not participate in targeting, enemy counts, rendering or collision.
- [ ] Crossing a lesson guidance boundary shows feedback but never clamps player movement to an invisible wall.
- [ ] Full tutorial reaches Mini-Boss defeat, opens the finish route, reaches the collectible and persists `Completed`.
<!-- BND_TUTORIAL_COMPLETION_INTEGRITY_V10117:END -->

<!-- BND_TUTORIAL_OPENING_POLISH_V10113:BEGIN -->
## V10.11.3 opening dialogue and tutorial polish gate

- [ ] Room fade finishes before any bubble pixel is visible.
- [ ] Bubble begins after a short visible-room pause, is top-left and reads exactly `honey come here a second`.
- [ ] Entry is professional fade+scale; exit is the exact reverse; the first walk step starts only after full disappearance.
- [ ] Voice reads feminine and nonverbal, plays once and is absent after skip/completion.
- [ ] ESC during entry, hold or exit leaves no bubble/audio and reaches the exact final screen/camera state.
- [ ] Walking off either wall-jump support causes a real fall; no hover or invisible support remains.
- [ ] Jump Attack simultaneously shows Jump + Attack and both input combinations.
- [ ] One mounted shot damages only its chosen living enemy and only when the projectile visually arrives.
- [ ] Existing Mini-Boss slam/telegraph presentation remains intact and every third phase-two attack is a distinct ranged shot.
- [ ] Colored course decorations remain behind actors and do not read as foreground floating blocks.
- [ ] Prompt, detail, bindings and HUD remain large, colorful and readable while the per-letter pulse runs.
<!-- BND_TUTORIAL_OPENING_POLISH_V10113:END -->

<!-- BND_POST_INTRO_CINEMATIC_QA_LATEST_COMMIT_ALIGNMENT_V1094:BEGIN -->
## V10.9.4 latest-commit alignment and partial-source regression check

- `BDModernHandheld3DQA` must validate base presenter behavior in `BDModernHandheld3DPresenter.cs` and generated set/shadow behavior in `BDModernHandheld3DPresenter.CinematicEnvironment.cs`.
- The focused check requires the complete tabletop, front edge/apron, four legs, floor, cyclorama, three device contact-shadow layers, four leg contact shadows and the three-light rig.
- The retired `Short Core Shadow To Left`, `Professional Blurred Wood Table` and `Table Cinematic Vignette` contracts may not be restored merely to satisfy old QA.
- Acceptance requires a fresh Unity compile and `TEST EVERYTHING` at `0/0/0`.
<!-- BND_POST_INTRO_CINEMATIC_QA_LATEST_COMMIT_ALIGNMENT_V1094:END -->

<!-- BND_POST_INTRO_CINEMATIC_DIRECTOR_PASS_V109:BEGIN -->
## Post-intro cinematic director gate V10.9

- [ ] `BDModernHandheld3DPresenter.CinematicEnvironment.cs` exists and builds the complete table, floor, cyclorama, grounded shadows and restrained lights.
- [ ] Runtime source contains no `Professional Blurred Wood Table`, `Table Cinematic Vignette`, `EvaluateCubicBezier`, old 3.05-second travel or object-motion cinematic ownership.
- [ ] Camera path duration/stage constants are 4.40 / 0.55 / 2.10 / 3.35 seconds.
- [ ] Natural cubic second derivatives are prepared once; no per-frame arrays, scene searches, LINQ or material creation occur.
- [ ] Horizontal camera alignment ends before the final forward/downward settle.
- [ ] Main Menu position, rotation, FOV, near clip and far clip use one centralized authoritative state for build, completion and ordinary restoration.
- [ ] Opening/final frame composition and 24/30/60 FPS focused checks pass.
- [ ] TEST EVERYTHING returns `0/0/0` and the user accepts the rendered shot.
<!-- BND_POST_INTRO_CINEMATIC_DIRECTOR_PASS_V109:END -->

<!-- BND_FIRST_LAUNCH_TUTORIAL_V1081_HOTFIX:BEGIN -->
## First-launch tutorial V10.8.1 regression gate

- Mounted lesson shot sets `advancesMountedShotLesson` only in `RangedAttack`.
- Lesson completion is called only from visible projectile impact after `hitLivingTarget` is confirmed.
- Impact transitions to Reload exactly once; Reload transitions to Charged Shot.
- Misses and animation completion do not advance the mounted shooting lesson.
- The post-BBH transition source contains no device/shadow start-transform or animated scale/position path; only camera pose/FOV changes.
- Table and vignette coverage remain larger than the visible framing envelope.
- Revised package tools pass interactive-color, `NO_COLOR`, redirected-output and `TERM=dumb` checks.
- Full prior V10.8 automated and focused Play Mode matrix remains mandatory.
<!-- BND_FIRST_LAUNCH_TUTORIAL_V1081_HOTFIX:END -->

<!-- BND_FIRST_LAUNCH_TUTORIAL_MECHANICS_REPAIR_V108:BEGIN -->
## First-launch tutorial V10.8 focused acceptance

- [ ] Unity compiles with no new error/warning and TEST EVERYTHING reports `0 / 0 / 0`.
- [ ] Injured horse refuses Mount until healing completes.
- [ ] Player, horse and enemies show real alternating leg frames while moving.
- [ ] Mounted and charged ranged damage/death occurs only at visible projectile impact.
- [ ] Charged Shot is hold-to-auto-fire; early pre-threshold release is ordinary, mid-charge release cancels, full ammo is consumed and Reload begins.
- [ ] Hook pull presentation completes before damage and progression.
- [ ] Major-lesson deaths restore locally behind the opaque checkpoint cover.
- [ ] Living enemy bodies block the player/horse; no pass-through remains.
- [ ] No decorative course-divider line or visible lesson gate remains.
- [ ] Final boss instructions persist; telegraph, impact, recovery and legal damage windows are readable and survivable.
- [ ] Post-BBH transition is a full-screen real-3D camera/device/table shot with no flat/slide-like frame and exact final restoration.
<!-- BND_FIRST_LAUNCH_TUTORIAL_MECHANICS_REPAIR_V108:END -->


## Modern Handheld V5 focused checks

- Main Menu X starts New Game, A opens Progression, B opens Settings and Y opens Credits.
- B returns on every non-main page.
- Center SELECT activates the highlighted row and center EXIT opens the correct in-screen confirmation.
- Arrow keys and WASD produce the same navigation.
- SELECT/EXIT recessed labels use equal font size, separate placement and no overlap.
- X/Y/A/B, D-pad and center buttons display approved source-sheet textures on real 3D bodies.
- New Game hero and text-only memory card are stacked vertically without clipping.
- Progression, Credits, Settings and confirmation screens stay within safe margins.
- Device is slightly higher in frame but remains grounded.
- Short left shadow and contact shadow are visible; upper-right glass glint is visible and non-obstructive.

# QA Checklist

<!-- B&D BBH CINEMATIC SIDE TASK V1 START -->
## BBH cinematic boot-intro side-task gate

- [ ] The active handheld priority remains unchanged; this remains a side-task verification slice.
- [ ] First rendered frame is fully black and input remains blocked.
- [ ] B, B, H still begin strictly one at a time.
- [ ] First B reads as careful; second B reads as confident and causes a subtle first-B reaction.
- [ ] H landing creates restrained shared weight without a harsh shake or exaggerated bounce.
- [ ] Circle starts only after all three primary entrances complete and gathers from a small point.
- [ ] The circle is clearly larger than before, remains filled with a visible rim, stays behind BBH, and never crops at tested aspect ratios.
- [ ] Completed logo performs one deterministic micro-settle/breath and does not idle-bounce continuously.
- [ ] Intro still plays once per application session and does not replay on same-session New Game.
- [ ] Desktop 16:9, 16:10, ultrawide and landscape-mobile framing remain centered and readable.
<!-- B&D BBH CINEMATIC SIDE TASK V1 END -->

## Modern handheld V4 focused Play Mode gate

- [ ] Full-face texture/decal does not render over D-pad, shortcuts, face buttons, speakers or lower shell.
- [ ] Shell reads as molded 3D plastic with visible rear depth, outer bevel and side seam from the approved slight angle.
- [ ] Upper-right light produces a short soft shadow to the left plus a tight contact shadow; shadow is visible but not long.
- [ ] Glass has a restrained upper-right glint and edge reflection without hiding menu text/art.
- [ ] W/A/S/D navigate exactly like Up/Left/Down/Right, including after opening Pause with Escape.
- [ ] New Game-only memory card is visible only for fresh Start Game/New Run selection.
- [ ] That card is text-only and contains no Boy, Girl, route or Mother wording.
- [ ] Non-New-Game options keep character-neutral artwork; only the large New Game art changes with active character.
- [ ] Repeated page changes do not create duplicate labels, shadows, meshes or materials.


## Modern handheld premium art/layout gate

- [ ] Shell gradient source is at least 2048×2048.
- [ ] Front decal is at least 2048×3000, has alpha transparency and leaves screen/control apertures open.
- [ ] Dedicated decal shader blends alpha and does not flatten the product lighting.
- [ ] SELECT/EXIT hardware labels appear once, use equal font size, and are recessed, separated and centered.
- [ ] `PROGRESSION` and other long titles remain within the left title zone.
- [ ] New Game / New Run shows Boy art for Boy and Girl art for Girl.
- [ ] Progression, Settings, Credits, Quit/Return and Resume/Pause each show their own character-neutral image.
- [ ] Hover, D-pad selection and page entry all update the right artwork consistently.

## Active modern-handheld first Play Mode repair gate

1. Unity compiles and TEST EVERYTHING reports 0 blockers, 0 warnings and 0 info.
2. Main Menu content is visibly rendered inside the physical device display; the screen is not only its clear/background color.
3. Pause opened by Escape remains visible after the initiating Escape press is released and closes only on a later deliberate Resume/Back action.
4. SELECT and EXIT center labels are compact, equal-size, recessed, separated and readable.
5. X, Y, A and B letters appear once, in the approved orientation: X top, Y left, A right, B bottom.
6. Mouse clicks independently activate Main X=New Game, A=Progression, B=Settings and Y=Credits; non-main B=Back.
7. Center SELECT activates the highlighted row and center EXIT opens the correct confirmation.
8. Keyboard/gamepad D-pad, confirm, back and shortcuts remain consistent with physical clicks.
9. Page transitions remain contained inside the device screen; no legacy menu appears above it.
10. Only Start Game / New Run is protagonist-aware: Boy route shows Boy art and Girl route shows Girl art. Every other option/page uses its dedicated character-neutral image.


## Modern handheld package and compilation gate

- `Packages/manifest.json` declares `"com.unity.ugui": "2.0.0"`.
- Unity Package Manager resolves the dependency without errors.
- `BDModernHandheld3DPresenter` compiles with `UnityEngine.UI`, `Image`, `Text`, `RawImage` and `Outline`.
- `BDModernHandheld3DQA` reports `HANDHELD_UGUI_PACKAGE_MISSING` if the dependency is removed or changed.
- No focused visual/input testing begins until Console compiler errors are zero.

## Modern 3D handheld Main/Pause gate

1. Unity compiles with no project-generated error or unexplained warning.
2. `TEST EVERYTHING` reports 0 blockers, 0 warnings and 0 info.
3. Main Menu is rendered inside one real upright 3D handheld, not a flat device screenshot.
4. Shell thickness, screen opening, bezel, backing, transparent glass thickness/reflection and recessed lit display are visible from the real camera.
5. Blue-to-orange texture follows the shell and remains readable under the menu lighting.
6. D-pad, A/B/X/Y, SELECT and EXIT are separate 3D controls with hover/press/release feedback.
7. Mouse hover and click select the same rows/actions as D-pad and A.
8. Arrow/gamepad D-pad/WASD navigates; Main X starts New Game, A opens Progression, B opens Settings, Y opens Credits; non-main B backs out; center SELECT/EXIT work.
9. The center SELECT button activates the highlighted option and center EXIT opens the correct in-screen confirmation.
10. User-facing label is `Progression` on one line; `Meta Progression` is absent from the new Runtime UI.
11. Pause uses the same device and interaction system, resumes safely and requests a confirmed clean reload before returning to Main Menu.
12. Main, Pause, Settings, Progression, Credits, Abandon and Loading do not create competing state owners.
13. Start Game / New Run shows Boy art while playing Boy and the matched Girl art while playing Girl. No other menu option/page depicts either protagonist.
14. New Game character-art selection is deterministic, never random, and re-resolves correctly after scene reload/page transitions; neutral art remains unchanged by character selection.
15. The New Game Boy/Girl pair has equal dimensions and matching import settings; all neutral option images share their required dimensions/import contract.
16. Repeated open/close/reload leaves one presenter, one screen camera, one device camera and no orphan RenderTexture/material/listener.
17. Idle menu and navigation show no recurring GC allocation attributable to the presenter; glass/display cost is profiled on desktop and landscape mobile.
18. Legacy menu/backdrop are hidden only while the 3D presenter owns the menu and remain a functional fallback if presenter creation fails.
19. User visually approves Main Menu and Pause before the task is marked verified.

## Modern handheld product-shot focused Play Mode gate

1. Confirm the game renders complete generated 3D shell geometry with rear depth, outer bevel, side seams, glass, display and controls.
2. Confirm there is no full-face Runtime decal/sticker object and no texture layer crosses the live screen, D-pad, A/B/X/Y, SELECT, EXIT or speaker openings.
3. Confirm the exact supplied wood source is visible under the device. Detail is relatively sharp around the focal/contact band and becomes smoothly more defocused toward both near and far table regions; no uniform blur seam is visible.
4. Confirm the upper device edge is slightly farther from camera, while the device remains readable and does not look severely foreshortened.
5. Confirm the principal highlight reads from upper-right and the short soft cast shadow falls left with a tighter contact shadow; no long floating shadow.
6. Confirm screen display, glass thickness, edge Fresnel and reflection are separate layers and text remains readable.
7. Confirm Main, Pause, Settings, Progression, Credits, Loading and the existing abandon confirmation replace content inside the same screen. No external overlay appears over the device.
8. Confirm every page change uses one short contained modern-handheld transition with no duplicate page, stuck shutter, black frame or lost focus.
9. Confirm mouse hover/click and D-pad/arrows share the same selection. Each visible D-pad cap depresses and recovers in the activated direction.
10. Confirm Main X/A/B/Y invoke New Game/Progression/Settings/Credits once, non-main B backs once, center SELECT activates once and center EXIT opens one confirmation.
11. Run as Boy and confirm only Start Game / New Run shows Boy; run as Girl and confirm only that same option shows Girl. Confirm Pause/Resume, Progression, Settings, Credits, Quit/Return and confirmation remain character-neutral after reload and scene transition.
12. Inspect Console and Profiler during repeated Main↔Settings↔Progression and gameplay↔Pause cycles: no duplicate presenter/camera, orphan RenderTexture, stuck input, recurring idle GC or per-press GameObject/material allocation.

## Active V23R19F semantic-QA compatibility gate

1. Guardian same-room QA validates the active V23R19E owner names and behavior: `TryResolvePlayerRoomFallback`, `spawnRoom.ContainsWorldPosition(`, and `playerTransform.position`.
2. The scanner must not require the removed local variable spelling `player.position`.
3. V23R9 status QA validates the maintained phrase `V23R8 automated QA passed` and the stable `Saved feature resume point`.
4. No Runtime or scene code is added solely to satisfy stale text.
5. TEST EVERYTHING must report neither `GUARDIAN_SAME_ROOM_SAFETY_MISSING` nor `V23R9_PROJECT_STATUS_MISSING`.
6. Passing this static gate does not close V23R19E focused Play Mode verification.

## Future cross-system expansion QA source

The future automated/manual matrix for enemy attack timing, bombs, flutes, summoned/ambient creatures, map teleport, movement animation, living-world effects, UI, performance, and delivery reporting is maintained in `ProjectGuide/Features/Runtime/GAMEPLAY_ABILITIES_MAP_AMBIENT_UI_EXPANSION_HE_V1.md`. Those checks become active only as each feature slice is implemented; their presence in the specification is not a pass claim.

## Active V23R19E mounted replay, death, airborne, and Battery-guardian gate

1. **Mounted abandon replay:** abandon a live run, start a new run, and verify the exact player is snapped to the horse before the first visible entrance movement and remains mounted until control release.
2. **Exact 90-degree airborne identity:** Air Light/Heavy reuse the exact grounded Light/Heavy arc geometry and differ only by the approved 90-degree orientation/vertical movement; no second horizontal slash appears.
3. **Player death before menu:** lethal damage disables player action, shows the full player death pose in gameplay, and only then opens the Game Boy menu.
4. **Enemy death before loot/despawn:** regular enemy archetypes stop movement/attacks, animate death, then release loot and despawn.
5. **Guardian reveal survives pickup:** collect Battery A/B immediately during reveal; the independent runner still activates every guardian.
6. **Guardian room safety:** exact pickup-room ownership is preferred; fallback uses only the player's containing room with bounded distance and clear line of sight.
7. **Guardian combat:** spawned guardians chase, attack, take player damage, die with animation, and retain Elite immunity to hook pull/knockback/small-enemy forced movement.
8. **Retained gates:** quicksand, jump-contact recovery, abandon confirmation, hook, mounted hazards, camera, Parry, bombs, and Console cleanliness remain passing.

## Active V23R18B mounted-hole ordering and animation-token gate

1. **Exact token:** root art direction, its Unity mirror, and production-animation requirements contain `temporary procedural animation is not final release animation`.
2. **Real API owner:** `ForceDismountAfterHazardRecovery` is provided by `BDHorseController`; QA does not search for it in `BDPlayerHazardRecovery`.
3. **Ordering:** mounted hole/lava force-dismounts before horse damage callbacks, before horse relocation, and before rider recovery.
4. **Damage callback isolation:** hazard damage reaching buck/faint thresholds cannot start the ordinary mounted buck/dismount presentation.
5. **Mounted hole:** horse and rider receive configured fall damage and finish unmounted at separate legal safe positions.
6. **Mounted lava:** only the horse receives lava damage; the rider follows the zero-damage recovery arc and both finish unmounted.
7. **Automated gate:** Unity compiles and TEST EVERYTHING passes with zero blockers and warnings.
8. **Focused gate:** repeat mounted hole/lava at normal HP and at damage-burst/faint thresholds before marking V23R18B complete.

## Active V23R18A production-animation and horse-hazard gate

1. **Animation authority:** `ProjectGuide/Product/ART_DIRECTION.md` links to the complete production-animation requirements.
2. **Coverage:** player, horse, enemies, bosses, interactions, hazards, destructibles, UI and cinematics are included.
3. **No false completion:** prototype procedural motion remains animation debt until approved at production quality.
4. **Unmounted hole:** horse receives configured fall damage and returns to its latest legal safe point.
5. **Mounted hole:** horse and rider both receive fall damage; mounted state ends safely.
6. **Unmounted lava:** horse receives lava damage and returns to its latest legal safe point.
7. **Mounted lava:** horse receives lava damage; rider receives zero lava damage and follows the recovery arc; mounted state ends before relocation.
8. **Paired recovery:** rider and horse finish at valid separated safe positions.
9. **Semantic quicksand QA:** current V23R17 player/horse/enemy failure methods and volume marker satisfy retained scanning.
10. **Retained safety:** quicksand, enemy hazard avoidance/forced entry, mounted collision damage, wall jump, intro direction and airborne attacks remain passing.
11. **Automated gate:** Unity compiles and TEST EVERYTHING passes with zero blockers and warnings.
12. **Focused gate:** record actual Play Mode results before marking V23R18A complete.

## Active V23R17 movement, hazard, and mounted-impact gate

1. Player movement multiplier decreases continuously with quicksand depth.
2. Grounded quicksand contact deals 2 damage per complete second; airborne time does not.
3. Jump extraction occurs once per committed jump; dodge pauses sinking and does not extract.
4. Half-body sink applies fall damage and safe recovery without a loop.
5. Enemy brain movement cannot enter hole, lava, quicksand, or future registered hazards.
6. Knockback and mounted impacts may force only small regular enemies into hazards.
7. Eligible enemies die immediately in hole/lava and sink before death in quicksand.
8. Mounted grazing/direct impacts deal 4-10 according to speed/directness and knock in the actual impact direction.
9. Repeated overlap cannot multi-hit inside the per-enemy cooldown.
10. Wall jump requires recent airborne wall contact, turns away, and produces a medium arc.
11. Intro horse ends facing a clear room direction and airborne Light/Heavy use distinct body animation.
12. Retained camera, combat, menu, damage-number, target-outline, and critical gates remain passing.

## Active V23R15D canonical-root token gate

1. Root `ProjectGuide/Product/ART_DIRECTION.md` is the sole canonical visual source.
2. The Unity-side mirror includes the exact phrase `Canonical root source`.
3. The same declaration names `ProjectGuide/Product/ART_DIRECTION.md` explicitly.
4. The mirror remains synchronized and does not become an independent policy.
5. No Runtime, combat, hazard, scene, targeting, or damage file is changed.
6. TEST EVERYTHING runs without `V23R9_ART_DIRECTION_MIRROR_MISSING`.

## Active V23R15C art-direction mirror gate

1. Root `ProjectGuide/Product/ART_DIRECTION.md` remains the canonical visual source of truth.
2. `ProjectGuide/Product/ART_DIRECTION.md` explicitly names `ProjectGuide/Product/ART_DIRECTION.md` as its canonical repository source.
3. The Unity-side document remains a synchronized mirror, not a competing policy.
4. No Runtime, combat, hazard, scene, or target-presentation file is changed by V23R15C.
5. Unity compiles and TEST EVERYTHING runs without `V23R9_ART_DIRECTION_MIRROR_MISSING`.
6. Existing V23R15/V23R15B damage-spectrum and independent AOE-critical gates remain unchanged.

## Active V23R15B per-target AOE critical and semantic QA gate

1. Spin resolves one shared ±10% pre-critical spectrum value before iterating targets.
2. Every unique enemy hit by the spin calls the exact 6% critical resolver independently inside the loop.
3. One spin may produce a normal amber number on one target and a critical fuchsia number on another.
4. Critical damage remains exactly 1.5 times the shared pre-critical value for the successful target.
5. Duplicate colliders belonging to one enemy do not produce duplicate rolls or duplicate damage.
6. Light, heavy, airborne light, and airborne heavy retain one critical state per committed attack.
7. Projectile and grappling-hook damage remain fixed and non-critical.
8. Quicksand, damage-number dispatch, and art-direction mirror QA validate active semantic contracts rather than stale exact text.
9. Unity compiles and TEST EVERYTHING runs with the three latest blockers absent.
10. Focused Play Mode verification is required before marking V23R15/V23R15B complete.

## Active V23R15A QA result API compatibility gate

1. `BDV23R14DamageNumbersLabelVisibilityQA` uses `result.findings` and `BDOneClickQAFinding`.
2. `BDV23R15MeleeDamageCriticalQA` uses `result.findings` and `BDOneClickQAFinding`.
3. Both scanners use `BDOneClickQASeverity.Blocker` and preserve their existing finding code/message content.
4. Neither scanner contains `result.Blockers` or `BDOneClickQAIssue`.
5. Unity compiles without the reported CS1061 and CS0246 errors.
6. TEST EVERYTHING runs before any Runtime PASS claim.
7. Existing V23R14/V23R15 damage-number, sword-spectrum, critical, projectile, and hook gates remain unchanged.

## Active V23R15 melee spectrum and critical gate

1. Unity compiles with no C# errors.
2. Light, heavy, airborne light/heavy, and spin vary around configured base damage.
3. Default sword variance is ±10%; spin shares one pre-critical spectrum roll across its targets.
4. Critical chance is exactly 6%; multiplier is exactly 1.5.
5. Spin rolls critical independently for every unique enemy hit.
6. Projectile and hook damage remain fixed for identical conditions.
7. Projectile and hook hits never use the critical damage-number color.
8. Critical sword hits use the dedicated fuchsia/magenta number color.
9. Misses, blocked damage, dodge, and Parry do not display false critical numbers.
10. Existing V23R14 damage-number and hazard-label visibility gates remain passing.

# QA Checklist

## Active V23R13 audio, quicksand, silhouette-outline, and warning gate

1. **Audio direction completeness:** root and Unity mirror include the non-exclusive sound-event matrix for combat, movement, breakables, hazards, horse, UI, menu, intro, ambience, enemies, bosses, and future actions.
2. **Honest audio status:** C12.42 remains open; documentation does not claim the authored library or mixer is already complete.
3. **Quicksand type:** `BDHazardType.Quicksand`, `BDQuicksandStatus`, player/horse movement filtering, full-sink recovery, prototype cues, and scene installation exist.
4. **Prototype volume set:** exactly one hole/chasm, one lava, and one quicksand volume are installed.
5. **Player quicksand:** progressive slow, visible following ring, escape decay, 12-damage full sink, and safe-point recovery.
6. **Horse quicksand:** progressive slow and safe full-sink recovery; mounted rider is not damaged twice.
7. **Silhouette only:** no `OnGUI`, `Rect`, corner frame, pulse amount, minimum box size, or distance-based frame growth remains in `BDCombatTargetHighlighter`.
8. **Outline renderer:** one fixed-pixel red inverted-hull shell follows eligible mesh/skinned renderers and is removed/hidden when target validity ends.
9. **Target truth:** one target, current weapon range, mounted ranged envelope, projectile path, and blocker rules remain unchanged.
10. **Warnings:** the four reported CS0414 fields are absent and Unity Console is checked after compilation.
11. **Retained safety:** V23R12 runtime gates, hole/lava behavior, camera, menu, horse, bombs, airborne attacks, and enemy stability remain passing.

## Active V23R12 focused runtime-regression gate

1. Hook resolves the actual enemy movement root when `BDHealth` is on a child.
2. Pull-size classification ignores attack, awareness, vision, range, prompt, and telegraph helper colliders.
3. Multiple small regular archetypes pull consistently; oversized regular, mini-boss, and boss targets remain damage-only.
4. Pulled enemies stop before overlap, remain in sword range, ground correctly, and cannot immediately contact-attack the player.
5. Mounted ranged aiming selects at most one unobstructed projectile-valid target using a horse-height origin.
6. A successful Parry clears all active player slash visuals before anticipation/freeze; menu/death reset does the same.
7. Horse action prompts sit above status/name/health and no legacy action sentence duplicates them.
8. Enemy spawn placement accounts for CharacterController center/height/radius/scale before first visibility.
9. Enemy late safety correction cannot relocate an active enemy horizontally; authored jump/charge/knockback/hook exceptions remain legal.
10. Ground stick, placement guard, jumper landing, hook completion, and motion stabilizer agree on root position and baseline.
11. Main menu, death, pause, and settings render content and the Game Boy shell from the same GUI owner/pass.
12. TEST EVERYTHING passes, then all visual/runtime items are verified repeatedly in Play Mode before PASS.

## Active V23R11 audio, bomb, and airborne-attack gate

1. `ProjectGuide/Product/AUDIO_DIRECTION.md` and its Unity-side mirror are synchronized and indexed as canonical/mirror rather than two competing sources.
2. Exploration, combat, mini-boss, boss, and Mother phase music contracts are explicit; Mother phase 4 includes a synchronized tick-tock stem.
3. AudioMixer groups, snapshots, headroom, true-peak, loudness, ducking, and transition rules are documented without falsely claiming runtime implementation.
4. A trap-layer bomb produces a visible core, expanding ring, sparks, explosion audio, and near-player camera accent.
5. Player and horse damage remain correct; nearby non-owner enemies each take one damage event despite multiple colliders.
6. Bomb owner immunity, stagger, flash, knockback, cleanup, and grounding remain stable.
7. The real committed light/heavy attack chooses the airborne presentation.
8. Airborne light uses the normal light mesh/color/timing language in a vertical high-to-low plane.
9. Airborne heavy uses the normal heavy mesh/color/weight language in a vertical high-to-low plane.
10. No early hold-press visual and no later horizontal duplicate appear.
11. Grounded light/heavy and spin/hook input behavior remain unchanged.
12. TEST EVERYTHING passes, then focused Play Mode verifies bomb VFX/friendly fire and both airborne attacks.

 — Required Verification Gates

Actual pass/fail truth belongs in `ProjectGuide/Status/CURRENT.md`.

## Single required Unity QA command

```text
Boredom And Dungeons -> TEST EVERYTHING
```

New automated checks integrate into this command; do not create another mandatory QA action.

## Gate order

1. **Repository/preflight:** required files, stable `.meta`, no real conflict markers, no staged package/cache/duplicate-status artifacts.
2. **Package validation:** syntax, first install, second install for idempotency, validator, `git diff --check`.
3. **Unity compilation:** documented Unity version, no compiler errors, no unexplained compiler warnings, no project-generated red Console error.
4. **Automated QA:** TEST EVERYTHING, zero blockers; warnings fixed or explicitly accepted.
5. **Console cleanliness:** no repeated project-generated warning spam, edit-mode material leak, charged-shot warning, or AudioListener warning.
6. **Focused Play Mode:** changed behavior, repeated use, death/restart/re-entry, nearby-system regression checks.
7. **Performance when relevant:** measure CPU/GPU/memory/GC/draw calls/loading on a representative target.
8. **Documentation truth:** record real results and exact resume point in `ProjectGuide/Status/CURRENT.md`.


## Active V23R8 camera, horse, combat, and UX gate

1. **40/60 composition:** on foot and mounted, the active target remains near viewport Y `0.40` through cardinal/diagonal aim, wall pressure, room handoffs, and snap-to-target.
2. **Legacy composition migration:** an existing scene value that serialized the old 50-degree minimum pitch cannot force the target back to screen center.
3. **Unified horse prompts:** near the horse, every currently valid Mount/Heal/Pet action displays a compact keycap plus clear label; no standalone legacy cue duplicates it.
4. **Mounted stationary prompts:** while mounted and fully stopped, Dismount and conditional Heal are visible; the row hides while moving.
5. **Mounted mouse coherence:** horse facing and forward travel use the same smoothed aim direction, with no raw-cursor slide, small-motion twitch, or sudden turn.
6. **Hook input:** short strong press performs normal heavy; long strong hold performs one hook and no duplicate heavy.
7. **Hook target policy:** the hook applies exactly 2 damage; small regular enemies pull into sword range; large regular enemies, mini-bosses, and bosses do not move.
8. **Hook collision/cooldown:** walls block acquisition; hook cooldown is independent; during hook cooldown normal heavy is attempted only when its own cooldown is ready.
9. **Airborne attack presentation:** airborne light/heavy attacks retain their identity as vertical slashes, suppress the duplicate horizontal slash, and keep existing damage ownership.
10. **Target outline:** exactly one unobstructed enemy inside the currently usable weapon range receives the thin constant-pixel red silhouette outline; invalid, occluded, dead, off-axis, and out-of-range targets do not.
11. **Single owners:** prompts, highlight, hook visuals, and airborne visuals do not become camera, gameplay-input, health, or persistent movement owners.
12. **Retained regressions:** V23R6 handoff stability, first visible New Game frame, closed walls, FOV, holes, combat grounding, charged shot, AudioListener, BBH intro, death restart, and Console cleanliness remain passing.
13. **Art-direction documents:** the completed canonical root `ProjectGuide/Product/ART_DIRECTION.md`, Unity-side mirror, and reference board remain synchronized and indexed.

## Active V23R9 horse-state and art-direction gate

1. **V23R8 automated baseline:** the run generated at `2026-06-07T17:56:35.5590780Z` passed with 0 blockers, 0 warnings, and 0 info.
2. **On foot near horse:** Mount and Pet show active-key prompts; Heal shows a prompt only when needed and legal.
3. **Mounted stationary:** Dismount shows its prompt; Pet works by key without a prompt; Heal is disabled.
4. **Mounted moving:** Dismount remains key-usable without a prompt; Pet and Heal are disabled; no row is visible.
5. **No heal carry-over:** mounting ends and clears an on-foot healing session.
6. **Mounted Pet stability:** movement, dismount, hazard recovery, combat pressure, damage, pause, or death cancels and restores captured states.
7. **No duplicate cues:** legacy Heal/Pet indicators remain suppressed.
8. **Art direction:** the authoritative document and compact reference board exist with stable `.meta` files and are indexed.
9. **Visual contract:** 65% colorful wonder / 35% mystery, polished stylized fantasy, hand-painted/restrained-PBR materials, fantasy headings, readable body text, Game Boy-inspired icons, and Game Boy menu-shell rules are documented.
10. **True-victory state:** Mother victory persistently restores/awakens the Game Boy presentation without replacing menu/accessibility ownership.
11. **Responsive language:** desktop and landscape mobile share the visual language while adapting layout and control size.
12. **Retained regressions:** V23R8 camera, steering, hook, airborne attacks, highlight, V23R6 camera stability, hazards, combat, intro, and Console remain passing.


## Active V23R19K explicit airborne branch gate

1. Unity compiles and TEST EVERYTHING returns 0 blockers, 0 warnings and 0 info.
2. `BDPlayerMeleeEnhancer` returns explicit airborne identity and owns the body animation only.
3. `BDPlayerCombat` consumes that identity at the committed hit and chooses one visual branch.
4. Grounded Light/Heavy produces one grounded slash.
5. Airborne Light/Heavy produces one `SpawnVertical` slash with no grounded duplicate.
6. No `combat.SuppressNextStandardMeleeVisual` call remains in the enhancer.
7. Boy mounted Light/Heavy still launches neither sword nor hook; future Girl mounted hook remains unimplemented design.
8. The resolved V23R19I compile bug is documented in status/history and is absent from the current open-bug table.
9. `OPENING_DIALOGUE_WORDLESS_CHARACTER_VOICE_HE_V1.md` contains the exact `I’m bored.` future requirement and is not reported as Runtime-complete.
10. Focused Play Mode verifies the actual orientation/placement before closing the reopened airborne bug.

## Active V23R19J semantic QA realignment gate

1. The mounted-intro release-order scanner recognizes `RestoreMountedIntroControls` and still proves release occurs after the full-stop hold.
2. Airborne melee QA validates committed damage/timing in `BDPlayerCombat` and actual presentation in `BDPlayerMeleeEnhancer`.
3. Mounted target QA validates the active horse-height `TargetHighlightOrigin` path.
4. Elite guardian QA validates explicit `ConfigureEliteGuardian()` plus forced-movement-disabled behavior without requiring a new serialized enum value.
5. Hook design retains hit-committed small-enemy pull and Elite guardian immunity while preserving boy-mounted-hook prohibition.
6. Death QA validates `BDCharacterDeathAnimation` through the current V23R19G renderer-branch owner.
7. Bug-ledger QA requires `BUG-V23R19H-001`, not the superseded incorrect `BUG-V23R19G-006` requirement.
8. The corrected Girl/Father/meta specification is marked required future work and contains Mother temporary-summon, phase-4 and temporary-meta-screen requirements.
9. TEST EVERYTHING reports zero blockers/warnings/info before focused Play Mode resumes.
10. No Runtime/scene/prefab change is permitted in this compatibility package.

## Active V23R19I forced-movement compile-compatibility gate

1. `BDCombatantProfile` exposes `ReceivesForcedMovement` and `CanReceiveForcedMovement(BDHealth)`.
2. The compatibility API delegates to the current serialized projectile/forced-movement policy rather than introducing a second mutable field.
3. Unity compiles without CS1061 in `BDKnockbackReceiver` and without CS0117 in `BDPlayerGrapplingHook`.
4. TEST EVERYTHING integrates `BDV23R19ICombatantForcedMovementCompatibilityQA` and reports no V23R19I blocker.
5. Regular enemies without a profile preserve current forced movement; explicit profiles remain authoritative.
6. Battery guardians/large enemies/bosses retain their intended no-pull/no-knockback behavior.
7. Boy mounted hook remains disabled; future Girl mounted hook remains design-only.
8. Rope, climbing and quicksand-swamp work is documented as `REQUIRED / LATER / NOT IMPLEMENTED`.

## Active V23R19H character-specific mounted-hook gate

1. **Boy on foot:** short Heavy and held grappling-hook behavior remain unchanged.
2. **Boy mounted:** Light/Heavy input emits neither sword melee nor grappling hook.
3. **Cooldown integrity:** blocked mounted input does not consume or reset the boy's hook cooldown.
4. **Mounted ranged:** normal mounted shooting remains functional.
5. **No global enable:** Runtime contains no mounted-hook input path for the current boy character.
6. **Girl future contract:** the future Girl character specification explicitly permits mounted hook use through character-specific capability data.
7. **Retained regressions:** airborne presentation, death presentation, abandon flow and mounted intro remain under the V23R19G focused gate.
8. **Automated gate:** TEST EVERYTHING includes `BDV23R19HCharacterMountedHookQA`.

## Active V23R19G focused regression and bug-ledger gate

1. **Airborne orientation:** Light/Heavy reuse the selected grounded arc and rotate it `-90°` around local X; the arc is in front and moves toward the floor.
2. **Player death visibility:** the real player renderer branch visibly falls/squashes; no death overlay or menu obscures the active pose.
3. **Menu timing:** the Game Boy menu opens only after the complete player death animation and readable hold.
4. **Large/Elite death:** large enemies and Battery guardians stop AI/collision, visibly animate, then allow loot/despawn.
5. **Clean abandon:** confirmation reloads the active scene and displays a clean main-menu state, never a popup over the abandoned run.
6. **Mounted replay:** abandon -> menu -> Start Game keeps the exact active-scene rider attached from first visible entrance frame through control release.
7. **Boy mounted combat:** mounted Light/Heavy input launches neither sword melee nor grappling hook and does not consume hook cooldown; on-foot hook behavior remains unchanged.
8. **Bug ledger:** `ProjectGuide/Status/BUGS.md` exists and matches every currently open/reopened/implemented-but-unverified defect.

## Active V23R6 actual-pose handoff and diagnostics gate

1. **Local-state preservation:** keep the current scene edits, four Codex-agent edits, `BDCameraTransitionDiagnostics.cs`, its `.meta`, and all existing diagnostics integration.
2. **No pre-smoothing release:** `ResolveRoomBoundaryConstrainedPosition` must not complete a handoff from an unsmoothed desired position.
3. **Actual camera body:** the two-room union remains active until the final smoothed camera position is inside the new room using the stable camera inset.
4. **Actual look point:** the union remains active until the smoothed constrained look point is inside the new room using the look-point inset.
5. **End-of-frame release:** completion occurs after the final pose write and before the diagnostic capture for that frame.
6. **Diagnostics remain gated:** F8 recording, F9 export, and F10 marker remain optional and inactive unless explicitly used.
7. **Diagnostic completeness:** CSV records room/handoff/target events, desired/contained/final poses and distances, FOV, external camera writers, player/horse roots, visual offsets, and Animator root-motion state.
8. **Walking acceptance:** repeated bidirectional node transitions on foot produce no direction snap, zoom pulse, or forward/back jump.
9. **Mounted acceptance:** repeated full-speed mounted transitions produce no direction snap, zoom pulse, or forward/back jump.
10. **Residual-motion procedure:** if anything remains, capture a short marked CSV before another camera, model, or geometry change.
11. **Retained safety:** closed walls, legal doorway visibility, normal-gameplay FOV, first visible New Game frame, death restart, holes, combat, charged shot, AudioListener, BBH intro, and Console behavior remain passing.
12. **Legacy semantic QA:** V20 regression coverage validates the active V23R6 actual-pose release markers and must not require the superseded `completed union room handoff` state string.
## Active V23R5 first-frame mounted-camera gate

1. **No pre-cinematic leak:** after selecting New Game, no frame shows the camera beside the entrance, attached to the player/horse, or at normal gameplay follow framing.
2. **First visible pose:** the first visible gameplay frame is already the approved higher/farther inside-room shot looking at the entrance.
3. **Ownership order:** `sceneLoaded` disables `BDCameraFollow` and primes camera position/rotation/FOV before the presentation coroutine and before any frame yield.
4. **Fallback safety:** if the mounted intro cannot resolve, normal camera ownership is restored before the black cover is released.
5. **Retained flow:** input remains locked, the horse enters, turns right, fully stops, and only then the normal camera and controls return.
6. **Legacy QA alignment:** V20 checks validate the active V23R4 safety inset, handoff-only wall cast, and V23R6 actual-pose release contract rather than obsolete numeric or state-string anchors.

## Active V23R4 camera micro-jitter gate

1. **Room-center distance:** while standing near the center of a room, rotate the aim through full cardinal and diagonal directions; camera distance and FOV remain visually constant.
2. **Walking wall pressure:** walk parallel to and toward each closed wall. The camera eases against containment without one-frame jumps, repeated micro-zooms, or pitch pulses.
3. **Mounted wall pressure:** repeat at horse speed, including corners and diagonal movement; higher movement speed does not reintroduce jitter.
4. **Look-point stability:** player/horse vertical settling and small floor changes do not make the framing bob or pulse.
5. **Room handoff:** cross legal openings repeatedly in both directions. The physical wall cast is active only during handoff, and the transition preserves distance/FOV without clipping through closed wall segments.
6. **Closed-wall visibility:** the reduced stable inset does not expose adjacent rooms from frontal, side, corner, diagonal, high, or low views.
7. **Performance:** normal follow does not execute repeated scene-wide room scans in one frame; no recurring camera-related GC allocation or spike is visible in the Profiler when checked.
8. **No map regeneration regression:** room, wall, doorway, enemy, hazard, portal, minimap, and authored-scene placements remain unchanged.

## Active V23R3 regression gate

1. **Single camera owner:** `BDCameraFollow` is the only normal-gameplay Main Camera transform owner; the old viewport-bias source is absent.
2. **Stable yaw/pitch:** wall proximity, enemies, damage, and room/tile handoff do not alter mouse sensitivity or gameplay pitch.
3. **Semantic camera QA:** automated checks validate the current rate-limited single-stage camera contract and do not require the obsolete `movementDirectionBlend` field.
4. **Planar shake:** combat shake has no vertical component.
5. **Closed walls:** on foot and mounted, side/corner/diagonal rotation never reveals an adjacent room; the maintained room-boundary design explicitly defines closed walls as a `visibility boundary`.
6. **Room/tile handoff:** cross several legal transitions in both directions. Distance/FOV/pitch remain stable with no snap or zoom.
7. **Combat grounding:** repeated enemy hits near walls and transitions do not move the player below the floor.
8. **Root-safe recovery:** any forced recovery places the CharacterController capsule fully above ground and movable.
9. **Walking-proof hole boundary:** approach every side and corner of the hole using ordinary walking at slow/full speed and diagonal angles; walking never enters.
10. **Swept footprint:** hazard filtering checks intermediate path samples and capsule footprint, not only final center position.
11. **Active intent only:** intentional gap entry is limited to active dodge, actively ascending jump, or explicit forced movement. Recent historical dodge/jump timestamps do not authorize later walking entry.
12. **Post-recovery suppression:** the named `0.85s` suppression contract blocks gap intent and forced-gap requests immediately after recovery; QA does not require obsolete `0.55s` text anchors.
13. **Near-hole respawn:** intentional fall recovery uses a valid local point just outside the same hole before older safe points or spawn.
14. **Respawn safety:** local recovery is outside all hazard volumes, above ground, free of overlap, and does not loop.
15. **Version-agnostic status QA:** active-work validation checks semantic headings and a Runtime version marker rather than a frozen category/version string.
16. **Existing regressions:** charged shot, AudioListener, mounted intro, BBH first frame, current-status QA, and V22R2 Console cleanup remain passing.
17. **Console:** no project-generated red errors or repeated warnings.

## Remote/local synchronization gate

- The remote reference used for this repair is `main` at `40104177ee396d19f375b62a20c410e4ac63bdc8`.
- Remote-only maintained-document improvements are present in the merged working tree.
- Local-only Runtime, scene, QA, `.meta`, package, and Codex work remains present.
- No reset, clean, broad checkout, or old full-file package replacement was used.
- Before push, create a safety branch/tag, commit the Unity-verified merged state, fetch `origin`, merge `origin/main`, rerun diff/hygiene checks, and inspect the merge result.

## Repository-hygiene gate

- Run `python3 tools/check_repository_hygiene.py` before handoff and commit.
- `AGENTS.md` is present as canonical maintained root Markdown and is scanned by TEST EVERYTHING.
- `.codex/config.toml` and `.codex/agents/` remain available as maintained project configuration.
- `AGENTS.rtf` is ignored as a local rich-text duplicate and must not be staged.
- Root Markdown matches `ProjectGuide/INDEX.md`.
- No obsolete roadmap/status copy, package README/manifest, repair narrative, chat export, or copied QA report remains tracked.
- Every removed document had valid requirements merged first.
- `ProjectGuide/Status/CURRENT.md` matches the real current state and resume point.

## V23R3B documentation compatibility gate

- `ProjectGuide/Features/Map/ROOM_BOUNDARY_CAMERA_AND_TEXTURE_READINESS.md` contains the exact durable term `visibility boundary`.
- This documentation repair does not replace Runtime or scene verification.
- Rerun TEST EVERYTHING and confirm `ROOM_BOUNDARY_DESIGN_MISSING` is absent.

## Truthful status language

- Static validator passed does not mean Unity compiled.
- Unity compiled does not mean Play Mode passed.
- Automated PASS does not mean the Console is clean.
- One Play Mode pass does not prove restart/re-entry behavior.
- No profiler data means performance remains unverified.
- A task is not DONE until all applicable gates pass.


## Active V23R19D quicksand, airborne, combat-contact, and abandon gate

1. **Quicksand slowdown:** entry movement is visibly slower and decreases progressively toward the half-body threshold; leaving returns movement to normal.
2. **Single application:** player slowdown is neither missing nor applied twice.
3. **Air strike placement:** Light and Heavy slash visuals are fully in front of the player, vertical, square to facing, and move from overhead toward the floor.
4. **No diagonal body roll:** the player body performs a forward/downward chop rather than a sideways rotation.
5. **Enemy contact while jumping:** jump onto an attacking Sword/Patrol/Charger; damage may occur, but the player never teleports to an old safe point.
6. **Abandon confirmation:** Pause -> Abandon opens confirmation; Cancel/Escape preserves the run; only explicit Yes returns to the main menu.
7. **Abandon replay:** after confirmed abandon, Start Game plays the mounted entrance with the exact player visibly attached to the horse throughout.
8. **Retained gates:** V23R19B hook/guardian behavior, V23R18B mounted hazards, camera, Parry, bombs, and Console cleanliness remain passing.

## Active V23R19C semantic QA compatibility gate

1. `BDV23R8CombatUxQA` validates the centralized `IsSmallRegularEnemy` and forced-movement policy rather than requiring a direct enum reference in the hook.
2. `BDV23R11BombAirborneAudioQA` validates committed airborne identity/body animation in `BDPlayerMeleeEnhancer`.
3. The same QA validates `BDMeleeSlashArcVisual.SpawnVertical` in `BDPlayerCombat`, the actual V23R19 presentation owner.
4. No runtime token, dead branch, duplicate visual call, or obsolete API is introduced merely to satisfy static text.
5. Rerun TEST EVERYTHING and confirm both reported blocker codes are absent.

## Future C06 merchant-shop and run-economy gate — NOT ACTIVE

1. Generate 2–4 shops per run with at least one initially-empty-room placement and one post-clear placement.
2. Present exactly three distinct weighted offers with prices inside the approved inclusive ranges.
3. Require both elapsed-time and progressed-room thresholds before automatic refresh; refill only purchased/empty slots and preserve occupied offers.
4. Validate one fixed-cost atomic full reroll that replaces all three offers and respects rarity/unique run history.
5. Give unique items lower selection weight and permit at most one appearance per run across all merchants/refreshes/rerolls.
6. Validate Friendly, HostileAlive, and Defeated run-global merchant states, including no stock/commerce for hostile encounters and no future appearance/refresh/reroll after defeat.
7. Validate aggression thresholds, transformation, forward shadow, cannon 45-degree rows, movement while firing, fast high-damage light sword, dodge, and no simultaneous cannon+sword action.
8. Validate free remaining rug stock on friendly defeat plus the exclusive one-of-two light-sword/cannon reward and matching model/damage-channel update.
9. Validate enemy 20% / 3–8 and breakable 30% / 2–10 currency rules.
10. Validate atomic purchases, sold stock, insufficient funds, wallet UI, new-run reset, and no duplicate rewards.
11. Validate player/horse protection bars and mounted protected-damage/no-buck routing.
12. Validate every percentage upgrade against the real owning runtime system.

## Future C06.META cross-run progression gate — NOT ACTIVE

1. Use an approved deterministic end-of-run formula with an understandable award breakdown.
2. Keep persistent meta points separate from run-shop currency.
3. Validate atomic spending/unlocking, save/load, migration, duplicate protection, and non-negative balances.
4. Verify every cosmetic/gameplay unlock against its real content owner.
5. Define and test defeat, victory, abandon, crash recovery, and partial-run award policy.
6. Finalize the feature name, menu UX/art, catalog, costs, pacing, save schema, and anti-farming rules before implementation is marked complete.

## Active V23R19B scene-safe hook and Battery-guardian gate

1. Scene installation updates only `jumpTravelMultiplier`, `jumpTravelBoostDuration`, Wall Jump reach/probe values, and `dashDistance` inside the single player-controller YAML block.
2. Quicksand-owned nonlethal jump recovery, longer normal jump/dodge, universal solid Wall Jump, and explicit airborne Light/Heavy presentation remain present.
3. Hook small-enemy eligibility is re-evaluated at impact through the canonical small-regular policy.
4. A living small regular hit cannot remain damage-only because of helper colliders, visual bounds, or child-health/root-locomotion separation.
5. Hook stagger and knockback cleanup resolve on the actual movement root.
6. Collectible guardians are built inactive and activated by root `SetActive(true)` only after final positioning.
7. Guardians include CharacterController, `BDHealth`, an AI archetype, grounding/stability, tactical components, and an Elite `BDCombatantProfile`.
8. Battery guardians are damageable and killable but reject grappling pull, knockback, mounted-impact displacement, and forced hazard entry.
9. Repeat Battery A and Battery B triggers after a fresh run and verify no inert or invulnerable guardian.
10. Unity compilation, TEST EVERYTHING, focused Play Mode, Console cleanliness, and restart/re-entry are required.

## Active V23R19 quicksand, traversal and airborne-identity gate

1. Jump repeatedly while holding each movement direction in quicksand; no safe-point teleport occurs before failure depth.
2. Verify progressive slowdown, 2 HP grounded ticks, jump extraction, dodge pause and true failure respawn.
3. Compare normal jump travel and dodge reach with prior behavior; both are modestly farther and remain controllable.
4. Wall jump from a room wall, hard prop, small enemy and horse; each launches away with the longer arc.
5. Wall jump does not trigger from floor, ceiling, slope or trigger-only hazard volume.
6. Jump then Light/Heavy: dedicated body motion plus vertical slash only; no regular horizontal attack animation/arc.
7. Ground Light/Heavy remain unchanged.
8. Retest hole boundaries, quicksand, mounted hazard recovery, camera and Console.

<!-- B&D TASK CONTINUITY QA GATE START -->
## Documentation continuity and handoff gate

Before handoff, package delivery, commit, category transition or claimed completion, verify:

- `ProjectGuide/Status/CURRENT.md` contains the current task's reason, stage, implementation truth, verification truth, blockers, next action and exact resume point;
- every large/multi-session task has a current detailed record under `ProjectGuide/Tasks/`;
- the task record includes scope, non-goals, decomposition, dependencies, affected systems/files, decisions, work completed, evidence, unverified areas and handoff point;
- all relevant canonical domain documents were updated in the same change;
- bug state is synchronized with `OPEN_BUG_TRACKER.md`;
- ownership/architecture, durable decisions, QA policy, performance policy and documentation ownership are synchronized when relevant;
- automated PASS is not presented as Play Mode, device, performance, visual, audio or user verification;
- no material task truth exists only in chat, a local note, package README or untracked file;
- `ProjectGuide/INDEX.md` lists every maintained task/contract document;
- a per-commit documentation relevance sweep was completed;
- valid information from completed/superseded documents was merged before removal;
- obsolete, duplicate, temporary, package-specific or irrelevant documents and broken references do not remain.

Failure of this gate blocks a handoff that depends on missing context.
<!-- B&D TASK CONTINUITY QA GATE END -->

<!-- B&D V23R19M AIRBORNE AXIS + SMALL ENEMY DEATH QA START -->
## V23R19M airborne-axis and small-enemy-death gate

Automated:

1. `BDMeleeSlashArcVisual` rotates the grounded mesh around local Z by exactly 90 degrees for airborne presentation.
2. The obsolete local-X `-90°` conversion is absent.
3. Grounded Light/Heavy still use their existing horizontal branch.
4. Airborne Light/Heavy still emit exactly one vertical branch.
5. Small regular enemies use the intact-body recoil/fall branch.
6. Historical V23R19J/V23R19K scanners do not require resolved bug rows or historical status sections to remain active documentation.
7. `TEST EVERYTHING` reports 0 blockers, 0 warnings and 0 info.

Focused Play Mode:

1. View a grounded Light and Heavy: the long attack line remains left-to-right and parallel to the floor.
2. Jump and use Light: the same long line reads top-to-bottom, perpendicular to the floor, in front of the player.
3. Jump and use Heavy: the same orientation rule applies with Heavy identity/width/color.
4. No horizontal duplicate appears in the airborne attack.
5. Kill at least two small regular enemy archetypes: they recoil and fall as intact bodies, do not flatten, and loot/despawn waits.
6. Kill one large/Elite enemy to confirm its existing death path was not changed.
<!-- B&D V23R19M AIRBORNE AXIS + SMALL ENEMY DEATH QA END -->

<!-- B&D V23R19N LEGACY AIRBORNE QA REALIGNMENT START -->
## V23R19N legacy airborne-QA semantic gate

1. V23R19D, V23R19E and V23R19G regression scanners require:
   - `BD LOCAL-Z AIRBORNE LINE ROTATION V23R19M`;
   - `Quaternion.AngleAxis(90f, Vector3.forward)`.
2. Those scanners must not require:
   - `BD CORRECT LOCAL-X AIRBORNE ROTATION V23R19G`;
   - `Quaternion.AngleAxis(-90f, Vector3.right)`.
3. Runtime files remain unchanged by V23R19N.
4. TEST EVERYTHING must report 0 blockers, 0 warnings and 0 info.
5. A clean automated pass does not close the V23R19M focused visual gates.
<!-- B&D V23R19N LEGACY AIRBORNE QA REALIGNMENT END -->

<!-- B&D V23R19O CRITICAL INTRO OUTLINE WALL JUMP QA START -->
## V23R19O critical mounted-intro, outline and Wall Jump gate

Automated:

1. mounted-intro renderer baseline capture and first-visible-frame reassertion exist;
2. skinned rider renderers update while offscreen during the cinematic;
3. every horse intro binding reasserts rider visibility;
4. target outline filters auxiliary rings and requires damageable-collider-envelope intersection;
5. auxiliary ring transparency owner is installed on enemies;
6. Wall Jump has explicit longer/higher tuning, retained speed and bounded steering;
7. camera yaw has a Wall Jump-only response multiplier;
8. TEST EVERYTHING reports 0 blockers, 0 warnings and 0 info.

Focused Play Mode:

1. abandon a live run, confirm, press Start Game and record the complete opening;
2. confirm the Boy is present on the horse on the first visible frame and never pops in late;
3. target at least one small and one large enemy and confirm only vulnerable model geometry receives the red outline;
4. confirm the ground ring/circle is not outlined and is more transparent;
5. verify target range, one-target limit and wall blocking;
6. wall-jump from a wall, enemy and horse where physically valid;
7. verify greater height/range and steer left/right/forward during the arc;
8. verify the model and camera yaw follow the steered trajectory;
9. verify ordinary grounded jump is unchanged.
<!-- B&D V23R19O CRITICAL INTRO OUTLINE WALL JUMP QA END -->

<!-- B&D V23R19P QA SEMANTIC + CATERPILLAR SPEC GATE START -->
## V23R19P QA semantic and Caterpillar specification gate

1. Traversal QA follows the active V23R19O Wall Jump contract:
   - 0.62-second minimum push window;
   - steering field;
   - retained-speed field.
2. QA does not require the superseded 0.48-second Runtime duration.
3. Current bug-ledger QA does not require verified bugs to remain open.
4. Mounted-rider visibility QA checks the qualified method symbol without depending on source whitespace.
5. The Caterpillar document remains `REQUIRED / FUTURE / NOT IMPLEMENTED`.
6. The document explicitly requires:
   - selected rooms only, not every room;
   - clean-room-only visibility;
   - animated appearance;
   - animated disappearance when enemies make the room unsafe;
   - enemy-safe active gambling sessions;
   - one game per Caterpillar;
   - finite bankroll;
   - passive threshold that is not an absolute maximum;
   - no invented rules or values.
7. TEST EVERYTHING must report 0 blockers, 0 warnings and 0 info.
<!-- B&D V23R19P QA SEMANTIC + CATERPILLAR SPEC GATE END -->

<!-- B&D V23R19Q PROFESSIONAL MEMORY-HANDHELD UI GATE START -->
## V23R19Q professional memory-handheld UI gate

Automated:

1. both stale V23R19P QA tokens are removed;
2. boot gradient/glow/scanline/vignette resources are cached;
3. shell rounded/glass/scanline resources are cached;
4. shell/content remain integrated in one IMGUI owner;
5. all current menu labels/actions remain;
6. the professional UI task record and canonical art/UI contracts are synchronized;
7. TEST EVERYTHING reports 0 blockers, 0 warnings and 0 info.

Visual/interaction:

1. inspect BBH from the first black frame through fade;
2. inspect Main, Settings, Pause, Abandon and Loading;
3. verify no lost button, setting, text, effect or true-victory state;
4. verify 16:9 desktop and landscape-mobile-like layouts;
5. verify hover, click and 180 ms page transitions;
6. verify text remains crisp and readable;
7. observe Profiler/Stats for repeated texture creation, GC spikes or a visible rendering hitch.
<!-- B&D V23R19Q PROFESSIONAL MEMORY-HANDHELD UI GATE END -->

<!-- B&D V23R19R MENU CONTRACT + MASTER LEDGER QA START -->
## V23R19R menu contract and work-ledger gate

1. No validation source requires the removed label `B&D POCKET ADVENTURE`.
2. The active shell retains `B&D // POCKET MEMORY`.
3. The V23R19Q professional shell marker remains.
4. The master work sequence contains:
   - current blocker;
   - enemy attack animations next;
   - professional opening cinematic after enemy animations;
   - all implemented-but-unconfirmed V23R19O/V23R19Q checks;
   - future systems;
   - exact resume point.
5. `ProjectGuide/Status/CURRENT.md` and `OPEN_BUG_TRACKER.md` reflect the one current blocker.
6. TEST EVERYTHING must report 0 blockers, 0 warnings and 0 info.
7. Automated PASS does not verify enemy animations, opening cinematic, UI appearance, mounted intro, target outline, ring transparency, Wall Jump, performance or user acceptance.
<!-- B&D V23R19R MENU CONTRACT + MASTER LEDGER QA END -->

<!-- B&D V23R19S CONTINUITY SEMANTIC QA START -->
## V23R19S continuity semantic gate

1. V23R19R QA must not require one prose sentence containing every verification type.
2. It must require each canonical verification level independently:
   - installer/static validation;
   - compilation;
   - TEST EVERYTHING;
   - focused Play Mode;
   - target-device/performance verification;
   - user acceptance.
3. The continuity contract must state that an earlier stage never implies a later stage.
4. The master execution order remains:
   - blocker closure;
   - Enemy Attack Animations;
   - Professional Opening Cinematic;
   - retained verification;
   - remaining product queue.
5. TEST EVERYTHING must report 0 blockers, 0 warnings and 0 info.
6. A clean result authorizes the Enemy Attack Animations stage; it does not verify any visual or gameplay feature.
<!-- B&D V23R19S CONTINUITY SEMANTIC QA END -->

<!-- B&D V23R19T PHASE-AGNOSTIC STATUS QA START -->
## V23R19T phase-agnostic status and bug-ledger gate

1. V23R19R QA must not require `C01.QA.V23R19R` to remain current.
2. V23R19S QA must not require `C01.QA.V23R19S` to remain current.
3. Neither scanner may require its historical bug ID to remain in the open table forever.
4. Both scanners validate:
   - a CURRENT project status;
   - the canonical master sequence;
   - Enemy Attack Animations next;
   - Professional Opening Cinematic after it;
   - permanent open-bug update discipline.
5. TEST EVERYTHING must report 0 blockers, 0 warnings and 0 info.
6. A clean result starts Enemy Attack Animations; it does not verify gameplay or visual work.
<!-- B&D V23R19T PHASE-AGNOSTIC STATUS QA END -->

<!-- B&D MODERN 3D HANDHELD QA START -->
## Approved upright 3D handheld Main/Pause gate

Documentation/static:

1. canonical 3D asset/UI specification exists and is indexed;
2. approved reference manifest and all referenced images exist with stable `.meta` files;
3. `Progression` replaces user-facing `Meta Progression` in the new design contract;
4. Boy/Girl image parity is explicit and paired assets use matched dimensions/import settings;
5. no document claims the 3D Runtime implementation is complete before it exists.

3D presentation:

6. device is an upright real 3D model with screen above controls;
7. shell has real depth, bevels and blue-to-orange material continuity;
8. glass/transparent cover is separate from bezel and display and visibly has thickness;
9. reflection/Fresnel never obscures text;
10. D-pad, A/B/X/Y, SELECT and EXIT are separate physical parts;
11. all physical buttons show tactile press and release;
12. screen content remains aligned behind glass during every approved device movement/transition.

Interaction:

13. mouse hover/click works on screen controls;
14. clickable physical controls work;
15. D-pad/arrows navigate deterministically;
16. Main X/A/B/Y invoke New Game/Progression/Settings/Credits once; non-main B returns once; center SELECT/EXIT each invoke one action;
17. B returns once;
18. Main B opens Settings; on non-main pages B returns;
19. Main A opens Progression; Main Y opens Credits;
20. changing between mouse and controller updates one shared focus without duplicate activation;
21. Pause opens quickly, Resume is safe/default and abandon still requires confirmation.

Content and parity:

22. Main and Pause/Escape use the same device system;
23. unsupported concept-art values/pages are omitted rather than fabricated;
24. Start Game / New Run uses Boy artwork for Boy and the exact matched Girl artwork for Girl;
25. every newly added protagonist image has its matched pair in the same change, while neutral option artwork contains neither protagonist and remains single-source;
26. `Progression` remains on one line at supported resolutions;
27. all existing real menu options/settings/true-victory behavior remain.

Performance and release:

28. no per-frame mesh/material/texture/RenderTexture/native-object allocation;
29. no first-open shader/material hitch after warm-up;
30. hidden device rendering is disabled or appropriately released;
31. desktop and representative mobile-like profiling is recorded;
32. Unity compiles without new errors;
33. TEST EVERYTHING reports 0 blockers, 0 warnings and 0 info;
34. repeated open/close, restart, abandon, death-menu and scene-transition tests pass;
35. final user visual/tactile acceptance is recorded.
<!-- B&D MODERN 3D HANDHELD QA END -->

## Documentation-reorganization compatibility gate

1. Canonical files remain under `ProjectGuide/`; retired duplicate root/Unity-side mirrors are not restored.
2. Historical QA discovery phrases that still represent valid contracts are preserved in `ProjectGuide/INDEX.md` or their canonical owner.
3. Active task records expose explicit `Why this task exists`, `Protected content and behavior`, `Performance contract`, and `Exact resume point` sections.
4. TEST EVERYTHING must report no documentation-map/task-record blocker before implementation continues.
5. A compatibility repair must update Current, Bugs, Verification, QA History and Work Queue in the same package.

## Modern handheld V5 focused QA — 2026-06-09

The presenter owns one semantic action per physical control: center SELECT activates focus; center EXIT opens the legal quit/abandon confirmation; Main Menu X starts New Game, A opens Progression, B opens Settings and Y opens Credits; B returns on every non-main page. WASD/arrows remain navigation-only, so keyboard A is never overloaded as the face-button A shortcut. Page UI, button pulse and state transition must derive from the same semantic action to prevent double execution or mismatched labels.

<!-- B&D FIRST LAUNCH + HANDHELD DIRECT REPAIR QA START -->
## First-launch tutorial and direct handheld repair gate

- [ ] Clean tutorial state: black pixel choice reveals tutorial gameplay directly; no white boot light or Main Menu flash appears first.
- [ ] Opening sequence: mount, scripted enemy arrival, guaranteed horse hit/dismount/retreat, one-hit enemy defeat, injured horse return and heal occur in order.
- [ ] Every tutorial lesson waits for its correct action; early/repeated inputs cannot skip future steps.
- [ ] Keyboard, gamepad, physical device, mouse and touch each complete the tutorial; prompt copy follows the last input source.
- [ ] EXIT opens the tutorial-native confirmation; activity pauses; Continue is default; opening input cannot confirm; EXIT/Back closes; Continue resumes exact state.
- [ ] Confirmed Leave stores Skipped before transition and never auto-replays. Completion stores Completed. Interrupted InProgress restarts safely.
- [ ] Tutorial creates no reward, run, progression, statistics or production-scene mutation and cleans every local object/highlight/target.
- [ ] Main context image aligns with title; all five context-card copies are correct and text-only.
- [ ] `THE MAZE AWAITS` and every affected screen/card remain contained at target aspect ratios.
- [ ] Physical hover adds no blue frame/emission/depth; actual presses animate D-Pad, SELECT, EXIT and X/Y/A/B consistently.
- [ ] SELECT/EXIT are centered/inward and do not overlap labels, D-Pad, face buttons or speakers.
- [ ] Settings icon is always visible.
- [ ] Pause is an internal handheld screen menu with correct Resume/Progression/Settings/Return semantics and hit areas.
- [ ] BBH cinematic side-task checklist passes without changing current task order.
<!-- B&D FIRST LAUNCH + HANDHELD DIRECT REPAIR QA END -->

<!-- B&D HORSE HUD MINIMAP V2 QA START -->
## Horse, contextual HUD, minimap and maintenance gate

- [ ] Horse speed is 100/92/84/76% at exact missing-health thresholds and is applied once to all horizontal movement.
- [ ] No icon/card/health bar floats above the horse.
- [ ] Horse bar appears only near or mounted and fades cleanly.
- [ ] Healing is slower and the ring/motes clearly begin, pulse and finish.
- [ ] Player health appears stationary, on damage and on heal, then fades; movement hides it.
- [ ] Ammo appears on ranged press including long hold and fades on release.
- [ ] Horse green dot, regular red dot, mini-boss larger red dot, boss red hexagon render and rotate/clamp correctly.
- [ ] Minimap dims to 38% while safely idle and wakes on movement/threat/discovery/toggle.
- [ ] Repository report contains before/after sizes, largest files/directories/types, duplicates, history/LFS proposals and removed generated paths.
- [ ] Unity compile, TEST EVERYTHING, PlayMode and clean-clone/build evidence recorded.
<!-- B&D HORSE HUD MINIMAP V2 QA END -->

<!-- BND_TUTORIAL_REFERENCE_LED_V3:BEGIN -->
## First-launch tutorial V3 focused checks

- [ ] action title is the largest text in the tutorial;
- [ ] short explanation remains readable without shrinking below its minimum;
- [ ] keyboard/mouse and handheld cards appear side by side;
- [ ] both routes remain visible after either route is used;
- [ ] active-route emphasis is subtle and never hides the inactive route;
- [ ] title, progress, feedback, world and instruction card do not overlap;
- [ ] edge decorations never cover lesson actors;
- [ ] world palette reads as indigo/purple, teal and warm path accents;
- [ ] world actors use point-filtered pixel sprites;
- [ ] scripted world movement is visibly stepped;
- [ ] animation remains basic and subordinate to instructions;
- [ ] keyboard/mouse-only completion passes;
- [ ] handheld-only completion passes;
- [ ] mixed-input completion passes;
- [ ] exit confirmation and first-launch persistence remain correct.
<!-- BND_TUTORIAL_REFERENCE_LED_V3:END -->

<!-- BND_FIRST_LAUNCH_TUTORIAL_PRODUCTION_COURSE_V10:BEGIN -->
## First-launch tutorial production course V10

- [ ] complete all routes in `QA/FIRST_LAUNCH_TUTORIAL_PRODUCTION_COURSE_V10.md`;
- [ ] prove one large instruction maximum and correct active Keycap switching;
- [ ] prove Tap/Hold never double-emits;
- [ ] prove checkpoints clean every transient action;
- [ ] prove the secret is optional, hidden and non-duplicating;
- [ ] prove the combined encounter has multiple legal solutions;
- [ ] prove the Mini-Boss phase/death/gate ordering;
- [ ] time representative runs at 5–8 minutes;
- [ ] verify the Editor reset command causes the tutorial to appear on the next Play Mode start.

## Queued persistent-run gate

When activated, verify normal Exit preserves Continue, Abandon removes it, New Game overwrite is confirmed, Abandon equals 84% of death-equivalent points, the shared result screen appears before the exit animation, and idempotency prevents duplicate awards.
<!-- BND_FIRST_LAUNCH_TUTORIAL_PRODUCTION_COURSE_V10:END -->

<!-- BND_FIRST_LAUNCH_TUTORIAL_V10_WARNING_CLEANUP_V101:BEGIN -->
## V10.1 compiler cleanliness gate

- [ ] Unity Console contains no `CS0414` warning for tutorial learning evidence.
- [ ] `TutorialLearningState` is the single source for Introduced/Attempted/Performed/Demonstrated/MasteredForTutorial evidence.
- [ ] The retired `firstLaunchTutorial*Demonstrated` boolean fields do not exist.
- [ ] Jump, Dodge, Parry, HazardKnockback, MountedShot and MountedImpact still record Demonstrated through `SetFirstLaunchTutorialLearningState`.
- [ ] TEST EVERYTHING remains `0 blockers / 0 warnings / 0 info` after recompilation.
<!-- BND_FIRST_LAUNCH_TUTORIAL_V10_WARNING_CLEANUP_V101:END -->

<!-- BND_FIRST_LAUNCH_TUTORIAL_V10_INPUT_RESPAWN_FLASH_REPAIR_V102:BEGIN -->
## First-launch tutorial V10.2 focused gate

- [ ] Unity compiles without new errors or warnings.
- [ ] TEST EVERYTHING: 0 blockers, 0 warnings, 0 info.
- [ ] Space/B Jump labels and behavior match.
- [ ] Directional double-tap Dodge labels and behavior match on keyboard and physical D-pad.
- [ ] J/left click, K/right click, X and Y all follow the Parry timing contract.
- [ ] Mount, ranged, heal, Spin and Hook labels remain correct after the remap.
- [ ] Checkpoint relocation is never visible outside the opaque respawn cover.
- [ ] Player color, scale, rotation, input and camera return cleanly after repeated recovery.
- [ ] No legacy or old menu frame appears after BBH intro.
- [ ] No recurring GC allocation is attributable to double-tap recognition or respawn presentation.
<!-- BND_FIRST_LAUNCH_TUTORIAL_V10_INPUT_RESPAWN_FLASH_REPAIR_V102:END -->

<!-- BND_FIRST_LAUNCH_TUTORIAL_ENTRY_GATE_V103:BEGIN -->
## Tutorial V10.3 focused checklist

- [ ] first post-BBH frame belongs to the modern handheld;
- [ ] `B&D` and `Boredom & Dungeons` are visible on black;
- [ ] `PLAY TUTORIAL` and `SKIP TUTORIAL` are vertically arranged and clearly selected;
- [ ] keyboard/controller/pointer/physical controls navigate and confirm;
- [ ] Play writes InProgress only after confirmation;
- [ ] Skip persists and does not auto-replay after restart;
- [ ] no legacy/stale page is exposed on either route;
- [ ] package source ZIP is removed after success and preserved after failure;
- [ ] TEST EVERYTHING reports zero blockers, warnings and info;
- [ ] animation production backlog remains open until separately verified.
<!-- BND_FIRST_LAUNCH_TUTORIAL_ENTRY_GATE_V103:END -->

<!-- BND_FIRST_LAUNCH_TUTORIAL_PROGRESSION_GATE_REPAIR_V104:BEGIN -->
## Tutorial V10.4 focused checklist

- [ ] entry choice typography is genuinely pixelated;
- [ ] no mounted melee/Spin/Hook transaction is created;
- [ ] one mounted shot transitions into Reload;
- [ ] Spin targets are ahead after dismount;
- [ ] Grapple target is ahead across the gap;
- [ ] lesson clamps remain invisible and provide contextual instruction feedback without divider/gate geometry;
- [ ] every mechanic appears in forward order;
- [ ] one complete run reaches the completion transition;
- [ ] completion persists across restart;
- [ ] compiler output and TEST EVERYTHING are clean.
<!-- BND_FIRST_LAUNCH_TUTORIAL_PROGRESSION_GATE_REPAIR_V104:END -->

<!-- BND_INTRO_TO_MAIN_MENU_CINEMATIC_AND_TUTORIAL_SPACING_V105:BEGIN -->
## V10.5 cinematic checklist

- [ ] title/subtitle separated;
- [ ] real screen active in opening shot;
- [ ] continuous position/rotation/FOV;
- [ ] exact final pose;
- [ ] input locked;
- [ ] no replay on ordinary entries;
- [ ] Play cancels and Skip preserves request;
- [ ] no legacy/incorrect/black frame;
- [ ] compiler and TEST EVERYTHING clean.
<!-- BND_INTRO_TO_MAIN_MENU_CINEMATIC_AND_TUTORIAL_SPACING_V105:END -->

<!-- BND_BBH_GLOBAL_TIMESCALE_REMOVAL_V106:BEGIN -->
## BBH time ownership regression check

- [ ] BBH code contains no global time-scale zero assignment.
- [ ] BBH still consumes/blocks input locally.
- [ ] BBH visual timing uses realtime/unscaled time.
- [ ] Intro-to-main-menu handoff remains exact and one-shot.
<!-- BND_BBH_GLOBAL_TIMESCALE_REMOVAL_V106:END -->

<!-- BND_POST_INTRO_TRANSITION_COLORED_OUTPUT_CLEAN_EXIT_V1072:BEGIN -->
## Clean-exit installer checks

- [ ] Success deletes exact source ZIP and extracted installer residue.
- [ ] Checksum failure deletes exact source ZIP and extracted installer residue.
- [ ] Preflight blocking deletes exact source ZIP and extracted installer residue.
- [ ] Post-write failure restores exact content and removes the verified failed-attempt backup.
- [ ] Unknown source remains blocked before writes.
- [ ] Runtime token checks scan Runtime source only.
- [ ] Editor QA rules cannot self-trigger from their own string literals.
- [ ] `NO_COLOR=1` remains readable.
<!-- BND_POST_INTRO_TRANSITION_COLORED_OUTPUT_CLEAN_EXIT_V1072:END -->
<!-- BND_TUTORIAL_CONTACT_DIRECTION_TRAVERSAL_SKIP_V1010:BEGIN -->
## Tutorial contact, facing, traversal and ESC skip

- [ ] A living enemy, jump root, active hazard, active gap and closed finish gate cannot be crossed by ordinary walking.
- [ ] Dead or inactive enemies no longer block traversal.
- [ ] Melee attacks behind the player or outside range do not damage.
- [ ] Projectiles do not damage before reaching their visible endpoint and misses do not show hit feedback.
- [ ] Left-facing and right-facing attacks travel and hit only in the facing direction.
- [ ] The airborne-attack lesson uses a grounded enemy and advances only after a visible airborne hit on the forward target.
- [ ] Wall-jump is the final lesson after the combined encounter and before boss intro.
- [ ] Wall-jump route is physically completed in order: approach reachable right wall, cross a clearly visible gap left to the platform, jump right to upper ground, continue to boss intro.
- [ ] Boss melee and projectile attacks show a readable body windup, pulsing attack lane, visible impact and clearly exposed recovery.
- [ ] No boss damage occurs without a corresponding visible impact or projectile contact.
- [ ] One normal `ESC` press during BBH intro or child approach skips to the exact final valid state; Enter, Space and controller confirm do not skip.
<!-- BND_TUTORIAL_CONTACT_DIRECTION_TRAVERSAL_SKIP_V1010:END -->
<!-- BND_CHILD_DIALOGUE_BUBBLE_POWER_TIMING_V10116:BEGIN -->
## V10.11.6 first-launch micro-polish checks

- [ ] `TEST EVERYTHING` reports `0 blockers / 0 warnings / 0 info`.
- [ ] Bubble appears at the upper-left slightly lower than V10.11.4 and remains fully inside the frame.
- [ ] Tail is visually connected to the bubble but does not overlap the bubble body.
- [ ] Child remains stationary until dialogue exit completes.
- [ ] Camera settles at the unchanged final pose before the screen powers on.
- [ ] Screen begins powering on at `9.20s`, completes at `10.20s`, and never flashes white.
- [ ] Skip still produces the valid final camera, powered screen, and active tutorial state.
<!-- BND_CHILD_DIALOGUE_BUBBLE_POWER_TIMING_V10116:END -->

<!-- BD_TUTORIAL_FINAL_INPUT_TARGET_PLAYER_V101130 -->
- First enemy Image and actor share state, and the quick-attack handler must start ResolveFirstLaunchTutorialProductionMelee before the impact frame. Keyboard letters are not tutorial action bindings.
- Player uses one readable silhouette with reduced limb swing and contextual facing.

<!-- BND_V1011308_QA -->
## V10.11.30.8 verification

- Unity Console contains no `AddBlocker` compiler error.
- `BDTutorialLessonScreensInputParryV1011306QA` adds blocker findings through
  `result.findings.Add(new BDOneClickQAFinding(...))`.
- TEST EVERYTHING reports `0 blockers, 0 warnings, 0 info`.
- Complete the existing lesson-screen, mouse mapping and Parry checks before
  Commit and Push.

## V10.11.30.16 checks
<!-- BND_TUTORIAL_CONTRACT_REPAIR_V1011316 -->
Require the four child-approach dialogue contracts and the Gameplay-owned LESSON COMPLETE — MOVE RIGHT TO THE NEXT SCREEN contract, plus compilation and one uninterrupted Play Mode tutorial run.
<!-- BND V10.11.30.17 LESSON COMPLETE CONTRACT -->
- The canonical lesson-complete travel message is owned by `Gameplay.cs`, consumed by `LessonScreens.cs`, and QA reports missing contracts against the actual source path.
