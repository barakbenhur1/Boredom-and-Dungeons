<!-- BND_TUTORIAL_BUBBLE_DEPTH_HORSE_CONTINUE_V1011330:BEGIN -->
## Dialogue pointer, persistent screen depth and completed-room cue — V10.11.30.30

The opening mother bubble's left-facing pointer is a layered diamond composition. Every exposed diamond owns a complete dark frame; the far-left diamond uses a centered larger backing diamond rather than a one-direction drop shadow. All layers remain under the same animated visual parent.

The handheld screen camera renders into an owned RenderTexture with one platform-supported persistent depth/stencil attachment. Memoryless storage, MSAA and depth-texture requests remain disabled. A depthless target is prohibited because Unity may internally substitute a transient memoryless depth surface for ScreenSpaceCamera UI clipping/masking.

After a room objective is proven complete, the instruction composition disappears and one fixed-screen pixel `CONTINUE` cue appears while the player walks to the physical right edge. It disappears before continuous scrolling. HorseReturn remains the single authored preloading exception: it is revealed only after the destination room settles.
<!-- BND_TUTORIAL_BUBBLE_DEPTH_HORSE_CONTINUE_V1011330:END -->

<!-- BND_TUTORIAL_HORSE_COMBAT_CONTINUE_V1011329:BEGIN -->
## Horse-shot, HorseReturn and post-lesson CONTINUE contract — V10.11.30.29

`EnemyArrival -> HorseShot -> JumpAttack` is one room-owned sequence. The shooter that visibly wounds the horse remains the same living actor for JumpAttack and is removed only by the player's verified visible kill. The room is not complete merely because the scripted horse hit finished.

Target-room static assets, ordinary actors, hazards and geometry still preload before scrolling. HorseReturn is the explicit authored exception: the injured horse remains inactive throughout the scroll and is revealed only after the destination camera has settled; its return animation then begins from the left after a short beat.

When a room's final objective is complete and edge travel becomes available, the instruction composition disappears and one fixed-screen pixel-style `CONTINUE` badge appears. It is informational, never a button. It uses a short stepped entrance and restrained directional pulse, remains visible while the player walks to the physical right edge, and is removed before the continuous handoff starts.
<!-- BND_TUTORIAL_HORSE_COMBAT_CONTINUE_V1011329:END -->

<!-- BND_TUTORIAL_FLOW_COHERENCE_V1011328:BEGIN -->
## Room-flow coherence addendum — V10.11.30.28

- Learned mechanics are persistent. In particular, Jump remains usable in every later on-foot room and while walking to the right edge after objective completion.
- A room may contain several tightly coupled beats when no new spatial context is needed. Horse Return, Heal Horse and Mount Again share one room; healing completion immediately exposes Mount Again.
- Continuous room transitions preload target-room static props, ordinary actors, hazards and obstacle geometry before camera motion. The authored HorseReturn actor is the explicit exception: it stays hidden during the scroll and begins returning only after settlement.
- Contextual geometry is owned by its lesson room. Wall-jump structures and the finish gate must be inactive in all unrelated rooms and cannot become invisible or visible blockers.
- Scripted horse animation starts from the authored first pose in the same frame as activation. An idle or target-position horse may not flash before hit/return motion begins.

<!-- BND_TUTORIAL_FLOW_COHERENCE_V1011328:END -->

<!-- BND_TUTORIAL_HORSE_FREE_OPENING_PET_SUPPRESSION_V1011327:BEGIN -->
## Current opening and horse-order contract — V10.11.30.27

Room 0 contains only `Move -> Jump`; the horse is neither active nor visible there. Landing beyond the root completes Jump and queues room 1 `AttackEnemy`. The early mechanic order is `AttackEnemy -> HeavyAttack -> Dodge -> Parry`. A successful Parry then queues the dedicated `MountHorse -> RideHorse` room. The next separate room owns `EnemyArrival -> HorseShot`, and the horse-shot story advances to `JumpAttack`.

The full-game horse HUD is prohibited during the first-launch presentation. In particular, no upper-right `PET` card may render while the tutorial is reserved, active or moving between rooms.
<!-- BND_TUTORIAL_HORSE_FREE_OPENING_PET_SUPPRESSION_V1011327:END -->

<!-- BND_TUTORIAL_CENTERED_PARRY_HORSE_METAL_V1011326:BEGIN -->
## Current opening-room and focused-combat contract — V10.11.30.26

The opening sequence uses four separate rooms with continuous physical right-edge handoffs: room 0 teaches `Move → Jump`; room 1 teaches `MountHorse → RideHorse`; room 2 contains `EnemyArrival → HorseShot`; room 3 teaches `AttackEnemy`. Only steps listed together share a room. A later room's instruction, actors and timers do not begin before its camera handoff has settled.

The Quick Attack room contains one passive one-health Small enemy at the exact room center. Its full pixel character remains readable and active until the visible Light impact kills it. Completion hides the entire instruction card; the player then reaches the visible right edge to advance.

The Parry room uses a persistent passive ranged teacher and one focused tutorial projectile transaction. The teacher is not a kill target and cannot take player damage during Parry. Production enemy projectiles are cancelled while the focused transaction owns the room. A successful parry cancels all projectile state before the animation advances the lesson, preventing residual shots and progression soft-locks.
<!-- BND_TUTORIAL_CENTERED_PARRY_HORSE_METAL_V1011326:END -->

<!-- BND_TUTORIAL_CONTINUOUS_ROOM_SEQUENCE_V1011325:BEGIN -->
## Continuous room progression contract — V10.11.30.25

After the authored opening chain, every new mechanic lesson owns a separate spatial room. A room becomes active only when its camera settlement is complete. The instruction remains visible until the mechanic's real objective succeeds. Success hides the complete instruction UI and leaves normal horizontal movement available. The room camera remains fixed, making the right boundary visually meaningful. Progression occurs only when the player physically reaches that boundary.

Room changes are diegetic course movement, not scene cuts. The next room is prepared beyond the current edge and revealed by one smooth camera/player translation while the player retains a locomotion pose. Transition overlays, fade-out/fade-in, black or white frames, respawn, checkpoint restoration and teleporting are prohibited. New-room instructions, timers, enemy actions and input unlocks begin only after the new room owns the frame.

The only approved opening exception remains Move → Jump → Mount → Ride on the first screen. Boss phase changes remain inside the same boss room because they are phases of one encounter rather than new traversal rooms.
<!-- BND_TUTORIAL_CONTINUOUS_ROOM_SEQUENCE_V1011325:END -->

<!-- BND_TUTORIAL_SCREEN_TWO_IMPACT_CONTINUOUS_HANDOFF_V1011324:BEGIN -->
## Screen-two ordinary attack and room exit contract

Screen two contains one passive enemy centered in the room. Its tutorial appears on entry. One valid ordinary Light attack kills it at visible impact; the tutorial then disappears. The player walks to the room's right edge and enters screen three through continuous world and camera movement. Room changes do not use fades, black covers, respawns, teleports or hidden player-position rewrites.
<!-- BND_TUTORIAL_SCREEN_TWO_IMPACT_CONTINUOUS_HANDOFF_V1011324:END -->

<!-- BND_TUTORIAL_SECOND_SCREEN_LIGHT_ATTACK_V1011323:BEGIN -->
## Screen two contract — ordinary attack

Screen two begins directly after the first screen's mounted-travel exit. Its entry state is `AttackEnemy`: player on foot at the left, horse hidden, one passive one-health enemy at the exact center, and the ordinary Light Attack instruction visible immediately. The enemy dies only from the visible ordinary melee impact. That death removes the instruction but does not open the next screen. The player must walk to the fixed screen's right edge before screen three transitions in.
<!-- BND_TUTORIAL_SECOND_SCREEN_LIGHT_ATTACK_V1011323:END -->

<!-- BND_TUTORIAL_OPENING_SCREEN_SEQUENCE_V1011322:BEGIN -->
## V10.11.30.22 authoritative opening-screen sequence

The opening tutorial is split into authored rooms: room 0 contains `Move → Jump`; room 1 contains `MountHorse → RideHorse`; room 2 contains `EnemyArrival → HorseShot`; room 3 contains `AttackEnemy`. Prompts replace one another only within their listed room. Every completed room then requires physical contact with its visible right edge before the continuous handoff begins.

This opening-screen exception is intentionally scoped. No later lesson ordering or completion contract is changed by V10.11.30.22.
<!-- BND_TUTORIAL_OPENING_SCREEN_SEQUENCE_V1011322:END -->

<!-- BND_TUTORIAL_RUNTIME_INTEGRITY_V1011319:BEGIN -->
## V10.11.30.19 authoritative lesson-screen and input contract

The opening first screen contains the authored sequence Move -> Jump -> Mount -> Ride. After that opening chain, each later mechanic lesson begins on a new screen and displays its populated instruction only after that screen becomes active. Objective completion hides the complete current instruction composition, freezes the completed lesson's transient hazards/actors/projectiles, and leaves only the external move-right message. The next step and layout are applied during a fully opaque screen-transition hold; this handoff is not a death, checkpoint restore or respawn.

Canonical desktop controls are: WASD/arrows Move; Space Jump; E Interact; J/left mouse Light; K/right mouse Heavy; Q/hold Q Ranged/Charged; hold F Heal; double-tap A/D or left/right Dodge; hold J/left mouse Spin; hold K/right mouse Grapple. Controller and physical handheld cards present their matching semantic controls at the same time. A lesson completes only from its authored world result, never merely from button input.

The mother bubble stays at the approved lower position and points left. The project-owned handheld screen render target carries color only and explicitly requests no depth/stencil or memoryless attachment.
<!-- BND_TUTORIAL_RUNTIME_INTEGRITY_V1011319:END -->

<!-- BND_TUTORIAL_FINAL_INPUT_COMBAT_PLAYER_V1011301:BEGIN -->
## V10.11.30.1 final blocking invariants

- Keyboard navigation and movement use explicit Arrow keys; mouse, controller and physical-handheld actions remain simultaneous and non-duplicating.
- Light and Heavy lesson completion is owned by a registered living actor and confirmed visible melee impact, never by hiding an Image at animation completion.
- Spin is atomic across the required front/back pair; Dodge is proven by crossing the obstacle.
- The tutorial child is a compact readable side-profile sprite authored facing positive X and flipped only by the existing facing owner.
<!-- BND_TUTORIAL_FINAL_INPUT_COMBAT_PLAYER_V1011301:END -->

<!-- BND_OPENING_TUTORIAL_RECOVERY_V101117:BEGIN -->
## V10.11.17 early-course and binding contract

The opening course begins with the player in front of the obstacle. Abilities remain locked until their own lesson. Clearing and landing beyond the obstacle completes room 0; physical right-edge contact carries the player continuously into room 1, where MountHorse remains visible until mounting succeeds and then changes immediately to RideHorse.

Every actionable instruction simultaneously presents the active keyboard/mouse or controller route and an illustrated physical handheld control. `JumpAttack` shows B + X, movement/dodge show the D-pad, and hold actions include a HOLD label. The final visible player art is blond hair, red shirt, and blue trousers rather than the generic entity tint.
<!-- BND_OPENING_TUTORIAL_RECOVERY_V101117:END -->

<!-- BND_TUTORIAL_DRIP_CONTRACT_BINDING_HOTFIX_V101116:BEGIN -->
## V10.11.16 binding presentation contract

Every actionable tutorial lesson presents two simultaneous routes: the current keyboard/controller binding and the matching physical handheld button. Input-source changes may update the left card but may not hide the physical-handheld card.
<!-- BND_TUTORIAL_DRIP_CONTRACT_BINDING_HOTFIX_V101116:END -->

<!-- BND_TUTORIAL_DRIP_MOUNT_INPUT_BINDINGS_V101114:BEGIN -->
## V10.11.14 input and lesson-order contract

Mount Horse is an immediate post-jump lesson, not a travel-gated lesson. The player lands beside the already-present horse and receives the interaction instruction there.

Tutorial attack input distinguishes world-space mouse clicks from clicks on physical handheld controls so the pointer system cannot consume a legitimate light attack. Binding information always teaches both the active desktop/controller route and the physical handheld equivalent.
<!-- BND_TUTORIAL_DRIP_MOUNT_INPUT_BINDINGS_V101114:END -->

<!-- BND_TUTORIAL_TRIGGER_UNLOCK_HUD_COLLISION_HOTFIX_V101113:BEGIN -->
## V10.11.13 trigger and unlock contract

The player begins at `x = -820`, directly before the authored first obstacle. The Move lesson completes after a short 12-unit run-up, enabling Jump before obstacle contact. Every active ability is locked until its lesson begins and all input routes use the same unlock state.

Travel-station gating is resolved before instruction visibility. Each step transition resets and restarts the instruction lifecycle; Jump and Attack Enemy are immediate lessons and are never given a second travel gate. Visible living enemies are solid horizontal blockers.

The modern handheld suppresses all full-game HUD while it owns first-launch presentation. Opening dialogue contains the mother only, with exact text `Sweety, where are you?`; no child bubble or voice remains.
<!-- BND_TUTORIAL_TRIGGER_UNLOCK_HUD_COLLISION_HOTFIX_V101113:END -->

<!-- BND_TUTORIAL_FINAL_PRODUCTION_COURSE_V101111:BEGIN -->
## V10.11.11 production-course contract

The tutorial is a continuous forward course: teach one major mechanic, travel to the next station, then teach the next mechanic. Course geometry is persistent and authored ahead of the camera. Actors are spawned outside the visible viewport. The camera and progression floor are monotonic, so a completed screen cannot be revisited.

Boss range behavior is a three-projectile vertical fan. Close behavior alternates a model-telegraphed straight slash and a jump-slam whose floor radius is visibly marked. All damage is impact-owned. Player/enemy damage feedback is a short model flicker/jitter.

The player palette is fixed to blond/yellow hair, red shirt and blue trousers. Ground sword presentation is horizontal; airborne sword presentation is overhead-to-downward.

Completion is contact collection followed by an authored sequence: lift relic overhead, originate magical light at the relic, expand to full screen, reveal main menu as the light fades.
<!-- BND_TUTORIAL_FINAL_PRODUCTION_COURSE_V101111:END -->

<!-- BND_TUTORIAL_PLAYER_TEXT_BOSS_ENVIRONMENT_V101110:BEGIN -->
## V10.11.10 player, text, combat and completion invariants

- Initial player position is `(-900, -108)` so the opening movement lesson has readable approach space.
- Tutorial player art is generated as clean side-profile pixel art: natural skin, blond hair, red shirt, blue trousers, dark shoes and a visible-side eye.
- Modern typography applies to every tutorial Text owner, not only the headline. Each owner uses bounded best-fit sizing, wrap, vertical truncation, outline, contextual colors and restrained per-character entrance/motion.
- Ordinary boss shots explicitly select the active boss and use the existing projectile transaction. Damage is single-target and resolves only at the endpoint. Ordinary projectiles may damage during windup; charged and melee recovery rules remain unchanged.
- Environment use is a timed visual transaction: attack launch, enemy arc toward the hazard, visible impact, short hold, then lesson advance.
- The final relic is contact-collected. Its prompt never requests `E`/interact, and its bright outlined gem presentation distinguishes it from background decoration.
<!-- BND_TUTORIAL_PLAYER_TEXT_BOSS_ENVIRONMENT_V101110:END -->

<!-- BND_TUTORIAL_WALLJUMP_BOSS_TYPOGRAPHY_DIALOGUE_V10118:BEGIN -->
## V10.11.8 wall-jump, typography and boss-combat invariants

- Wall-jump eligibility is a per-airborne-cycle transaction. A successful wall jump consumes it; landing on the ground, platform or upper ground resets it.
- Platform and upper-ground collision use explicit character standing baselines above the rendered surface. The platform extends to `3370`, wall-jump foot speed is `250`, and platform launch uses `1.28 × TutorialJumpVelocity`.
- The left-side wall clamp applies only while the player remains left of the wall. It cannot pull the player back after crossing.
- Tutorial headline authored size is 58 with a minimum 840×258 instruction panel. Detail, bindings, feedback and HUD sizes are raised and outlined.
- `BDTutorialLetterPulseEffect` owns per-character entrance, color cycling and restrained motion; text changes restart the entrance phase.
- Mini-boss phase one includes a projectile on sequence 2; phase two begins with a projectile and alternates projectile/slam attacks.
- Boss slam telegraph is a floor warning zone. Boss projectile telegraph is a compact charge orb followed by the existing physical enemy projectile transaction.
- During either boss phase, holding ranged starts a dedicated charge transaction. At 100% it stays armed and auto-fires on the first recovery frame; release is not required.
<!-- BND_TUTORIAL_WALLJUMP_BOSS_TYPOGRAPHY_DIALOGUE_V10118:END -->

<!-- BND_TUTORIAL_COMPLETION_INTEGRITY_V10117:BEGIN -->
## V10.11.7 completion and ownership invariants

- The mother-bubble diamond is a sibling rendered immediately before the panel, not a panel child, so it remains behind the bubble body while sharing its animated scale and offset.
- Horse facing is derived from `firstLaunchTutorialLastMoveDirection.x` in ordinary rendering and authored horse action poses.
- The ordinary mounted-shot lesson creates exactly one living target. A shot transaction locks that target and damage resolves only when the continuously rendered projectile reaches the endpoint.
- Step maximum-X values are instructional guidance, never physical collision. Physical collision remains limited to visible active obstacles and visible living actors.
- Actors whose images are inactive in the hierarchy cannot be targeted, counted, rendered or used as invisible collision blockers.
- Tutorial completion remains Mini-Boss defeat -> open finish route -> collectible/contact -> persisted `Completed`.
<!-- BND_TUTORIAL_COMPLETION_INTEGRITY_V10117:END -->

<!-- BND_TUTORIAL_OPENING_POLISH_V10113:BEGIN -->
## V10.11.3 focused tutorial invariants

- The pre-walk mother bubble uses exactly `honey come here a second`, appears top-left after the room reveal and blocks walk timing until its reverse exit completes.
- The feminine nonverbal cue is presentation-only and does not falsely complete the future global dialogue system.
- Leaving supported wall-jump geometry releases grounded state and produces a real fall.
- Jump Attack explicitly requires Jump + Attack and shows both input routes.
- A mounted projectile transaction owns exactly one selected target and resolves damage only at its visible endpoint.
- The existing local Mini-Boss telegraph/impact visuals remain authoritative; every third phase-two attack uses the ranged path.
- Decorative colored blocks remain behind gameplay actors. Major instruction text is larger and uses a restrained stepped per-letter pulse.
<!-- BND_TUTORIAL_OPENING_POLISH_V10113:END -->

<!-- BND_FIRST_LAUNCH_TUTORIAL_V1081_HOTFIX:BEGIN -->
## Mounted shooting progression invariant

The mounted shooting lesson owns one progression-capable shot transaction. The shot does not complete when input is pressed, ammunition is consumed or the recoil animation ends. It completes only when the visible projectile reaches a living lesson target, applies damage through the existing actor owner and records `MountedShot`. The step then moves to Reload exactly once.

The post-BBH presentation remains the same physical full-screen table/handheld scene. The device and its shadow stay fixed on the table; the scene camera alone changes position, rotation and field of view.
<!-- BND_FIRST_LAUNCH_TUTORIAL_V1081_HOTFIX:END -->

# First-Launch Tutorial V1

```text
Status: IMPLEMENTED / UNITY VERIFICATION REQUIRED
Classification: CURRENT FEATURE + STARTUP-SAFETY GATE
Runtime owner: BDModernHandheld3DPresenter first-launch partial
Persistent owner: BDFirstLaunchTutorialStateStore
Normal-run side effects: NONE
```

## Purpose

A clean installation opens the existing physical handheld product shot, but the normal Main Menu is not shown inside the glass yet. The black pixel choice screen reveals a short, deterministic two-dimensional tutorial directly without a white boot light. Completion or confirmed abandonment permanently suppresses automatic replay and returns smoothly to the normal Main Menu inside the same handheld screen.

The tutorial is intentionally not a real run. It does not create a procedural map, increment run counters, alter seeds, grant rewards, consume items, change meta progression, write gameplay statistics, or mutate the production horse/enemy state.

## Durable state

`BDFirstLaunchTutorialStateStore` persists a versioned state:

- `NotStarted`: show automatically.
- `InProgress`: an earlier session ended unexpectedly; restart safely from the beginning.
- `Completed`: never show automatically again.
- `Skipped`: confirmed Leave Tutorial; never show automatically again.

`Skipped` is written and flushed before the exit transition starts. Closing the game during the transition therefore cannot cause an automatic replay. Merely opening or cancelling the exit confirmation does not change the terminal state.

## Presentation flow

1. Existing handheld, table, glass and physical controls appear normally.
2. The screen content begins on the black pixel choice screen and reveals tutorial gameplay directly without a white boot light.
3. The light fades into a dedicated 2D tutorial scene contained entirely within the glass.
4. Only one scripted lesson is active at a time.
5. The regular Main Menu never appears underneath or between tutorial frames.
6. Completion or confirmed abandonment fades back to white, destroys the tutorial page and allows the existing Main Menu owner to rebuild its normal page.
7. Menu input remains guarded until the transition and held-input release complete.

## Scripted lessons

The fixed sequence is:

1. mount the nearby horse;
2. observe the scripted enemy arrival;
3. observe the guaranteed horse hit, forced dismount and short horse retreat;
4. defeat the scripted enemy with one attack;
5. wait for the injured horse to return;
6. heal the horse;
7. move through the room;
8. dodge a hazard;
9. use a heavy attack against an armored target;
10. hold the attack control to charge the spinning area attack;
11. parry a projectile;
12. grapple across a gap;
13. interact with the final memory collectible;
14. receive completion feedback and transition to Main Menu.

Scripted entities are local UI objects. They cannot miss, wander away, kill the player, become stuck in navigation or affect production gameplay objects.

## Input ownership

The tutorial does not create a second global input system. It is an explicit screen mode of `BDModernHandheld3DPresenter` and consumes the same normalized handheld actions already used by that owner:

| Tutorial action | Physical handheld | Keyboard | Gamepad |
|---|---|---|---|
| Move | D-Pad | WASD / arrows | bound movement control |
| Interact / confirm | SELECT | E | East / B |
| Light attack / spin hold | X | J / Left Mouse; hold for Spin after unlock | West / X |
| Heal horse | A | hold F | hold LB |
| Jump | B | Space | South / A |
| Dodge | D-Pad | double-tap A/D or Left/Right | double-tap D-Pad Left/Right |
| Heavy attack / grapple hold | Y | K / Right Mouse; hold for Grapple after unlock | North / Y |
| Ranged / charged hold | A | Q / hold Q | RB / hold RB |
| Parry | X or Y | timed J/K or Left/Right Mouse | timed West/North |
| Exit request | EXIT | Escape / Backspace | bound Menu / Select control |

Mouse and touch interact with the actual physical model through the existing device raycast path. The dominant instruction card always shows the keyboard/mouse route and the physical-handheld route at the same time; using one route never hides the other. Gamepad input remains accepted through the normalized action gate. Multiple sources still feed one tutorial action gate, and every stage is idempotent against repeated input.

## Physical highlighting

The required physical control receives a restrained scale-only guidance pulse. It never receives the rejected blue hover frame/glow. Pointer hover itself has no blue emission and no visual frame. Actual presses use the same depth, speed and scale-compression profile across D-Pad, SELECT, EXIT and X/Y/A/B.

## Exit confirmation

EXIT never abandons immediately. It opens an in-tutorial confirmation panel:

- title: `LEAVE THE TUTORIAL?`
- explanation: `The tutorial will not appear automatically again.`
- safe default: `CONTINUE TUTORIAL`
- destructive option: `LEAVE TUTORIAL`

While open, scripted time, entities and stage progression are paused. D-Pad changes selection, SELECT confirms, EXIT closes and resumes, and pointer/touch can select either row. A short input guard prevents the opening press from confirming a choice in the same frame.

Confirmed leave:

1. writes `Skipped` and flushes it;
2. disables tutorial interaction;
3. fades out inside the handheld;
4. destroys tutorial-local UI and hit targets;
5. restores the existing Main Menu owner;
6. grants no reward and updates no run or learning-completion statistic.

## Cleanup

All tutorial UI belongs to the generated tutorial page and is destroyed by the existing page cleanup path. No coroutine, scene object, texture, material, AI actor or audio loop survives the page transition. Physical-control highlight flags are cleared on completion, skip, scene change and presenter destruction.

## Performance constraints

- UI and screen hit targets are allocated once when the tutorial page is built.
- No textures, materials or meshes are generated per frame.
- Stage update uses cached component references.
- No scene searches or LINQ occur in the tutorial update path.
- Timing uses unscaled time so confirmation and screen transitions remain deterministic.
- The tutorial page is absent after `Completed` or `Skipped`, so it has no normal-session idle cost.

## Required verification

The feature is not verified until Unity compiles, `Boredom And Dungeons -> TEST EVERYTHING` reports `0 / 0 / 0`, and focused Play Mode covers clean install, completion, interruption, confirmed skip, input-source switching, physical controls, pointer/touch, popup guards, repeated input, transitions and future-launch suppression.

## Complete QA scenario matrix

The following scenarios are mandatory and are also reproduced in the package verification handoff.

### Activation and persistence

1. Clean installation / absent state presents the tutorial.
2. Full completion writes `Completed`.
3. A later launch after `Completed` presents the regular Main Menu.
4. Confirmed abandonment writes `Skipped`.
5. A later launch after `Skipped` presents the regular Main Menu.
6. Closing mid-tutorial without confirmed Leave writes neither terminal state.
7. Relaunch after interrupted `InProgress` restarts the tutorial safely.
8. New Run, run reset, death, win and return-to-menu do not reset tutorial state.
9. Full data deletion / clean install makes the tutorial eligible again.
10. `Skipped` is persisted before the visual return transition starts.
11. Closing or crashing after confirmed Leave but during transition still suppresses replay.

### Scripted progression

12. Every lesson advances in the documented order.
13. Repeated/parallel input cannot skip a lesson.
14. The scripted enemy dies from exactly one accepted tutorial attack.
15. The horse is hit, forces dismount, retreats and returns deterministically.
16. Horse healing is accepted only during the healing lesson.
17. An instruction is removed only after its correct action succeeds.
18. No two lessons or instruction cards are active simultaneously.

### Input routes

19. Keyboard can complete every lesson.
20. Gamepad can complete every lesson.
21. Physical handheld controls can complete every lesson.
22. A physical control feeds the same normalized tutorial action as its equivalent input route.
23. Prompt copy simultaneously shows keyboard/mouse and physical-handheld routes while gamepad input remains accepted.
24. Multiple input sources in one frame produce at most one accepted tutorial action.

### Exit confirmation

25. Physical EXIT opens the confirmation.
26. Scripted time, entities and lesson progression are fully paused while it is open.
27. `CONTINUE TUTORIAL` is the safe default.
28. EXIT/Back while open closes the confirmation and resumes.
29. Continue returns to the same lesson and exact scripted time.
30. Leave writes `Skipped` rather than `Completed`.
31. Leave cleans tutorial-local UI/targets/highlights and transitions to Main Menu.
32. Leave prevents every future automatic presentation.
33. The opening input guard prevents immediate accidental confirmation.
34. A second confirmation cannot be opened while one exists.
35. Abandonment grants no reward and changes no gameplay statistic.
36. Confirmation copy never promises that the tutorial will return.

### Presentation and regression

37. The black pixel choice screen reveals tutorial content directly without a white flash or legacy-frame regression.
38. Main Menu content never appears underneath or before the tutorial.
39. Completion-to-menu transition is continuous inside the same glass.
40. Leave-to-menu transition is continuous inside the same glass.
41. Every instruction and confirmation text remains inside screen bounds.
42. Confirmation styling belongs to the 2D tutorial language, not OS/Unity debug UI.
43. Main Menu control remains locked until the transition and held-input release complete.
44. The regular Main/Pause/Settings/Progression/Credits/exit flows remain behaviorally unchanged after either terminal tutorial state.

<!-- BND_TUTORIAL_REFERENCE_LED_V3:BEGIN -->
## V3 reference-led presentation

The user-approved visual references establish a clean remembered-console fantasy direction: deep indigo/purple space, dark teal edge vegetation, a warm traversable path and restrained amber/cyan accents. The references guide palette, hierarchy and atmosphere without being copied literally.

The pixel world remains deliberately simple and subordinate to teaching. Decorative elements stay outside the active play lane. World movement is stepped and basic.

The instruction card is the visual priority. Every actionable lesson presents a large action title, one short explanation and two large side-by-side cards:

- `KEYBOARD / MOUSE`
- `HANDHELD`

Both control routes are visible and active at the same time. The most recently used route may receive a subtle visual emphasis, but the other route never disappears.

The title/progress, feedback, world and instruction card own separate safe regions. Normal tutorial UI may not overlap.
<!-- BND_TUTORIAL_REFERENCE_LED_V3:END -->

<!-- BND_FIRST_LAUNCH_TUTORIAL_PRODUCTION_COURSE_V10:BEGIN -->
## V10 production playable-course contract

The tutorial is a 5–8 minute side-view pixel-art game contained inside the physical handheld screen. It teaches through play rather than a slideshow. The maintained sequence is: free movement and jump; horse Mount/Ride/Dismount; a readable sword enemy; Light/Heavy and Dodge/Parry; Tap/Hold Spin and Grappling Hook; environmental knockback; mounted shooting, Reload and mounted impact; an unadvertised optional secret; a multi-solution combined encounter; and a two-phase Mini-Boss using only previously taught mechanics.

The map is fixed but not corridor-like. It contains a small opening choice, local backtracking, one optional branch that reconnects, varied play spaces and a final arena. Progress gates prevent bypassing entire lessons without requiring arbitrary input counts or full mastery of optional mechanics.

`BDModernHandheld3DPresenter.FirstLaunchTutorial.ProductionCourse.cs` owns the production-course actors, explicit learning states, jump physics, checkpoints, health/ammo, enemy attack transactions, optional secret, combined encounter and Mini-Boss. It remains a cohesive partial of the existing presenter rather than a new global game/menu owner.

The instruction system displays one large localized action card at a time. It distinguishes Tap and Hold, changes the active Keycap without rebuilding the card, records Introduced/Attempted/Performed/Demonstrated/MasteredForTutorial separately, escalates hints gradually and never auto-solves the action. The optional secret receives no advance instruction or marker.

Confirmed tutorial Leave still writes `Skipped`, grants no run/meta reward and permanently suppresses automatic replay. Full completion writes `Completed`. The Editor reset command remains `Boredom And Dungeons -> Development -> Reset First Launch Tutorial`; stop and restart Play Mode afterward.

Implementation is not verified until Unity compilation, TEST EVERYTHING, all input-route runs, timing, reset/failure coverage and user acceptance pass.
<!-- BND_FIRST_LAUNCH_TUTORIAL_PRODUCTION_COURSE_V10:END -->

<!-- BND_FIRST_LAUNCH_TUTORIAL_V10_WARNING_CLEANUP_V101:BEGIN -->
## Tutorial learning-state ownership clarification

`TutorialLearningState` is the only tutorial-local evidence store for Introduced, Attempted, Performed, Demonstrated and MasteredForTutorial. Individual write-only booleans for specific mechanics are prohibited because they create duplicate truth, generate compiler warnings and can drift from prompt/progression decisions.
<!-- BND_FIRST_LAUNCH_TUTORIAL_V10_WARNING_CLEANUP_V101:END -->

<!-- BND_FIRST_LAUNCH_TUTORIAL_V10_INPUT_RESPAWN_FLASH_REPAIR_V102:BEGIN -->
## V10.2 authoritative input and recovery clarification

The tutorial must teach and consume the current gameplay gestures, not convenient substitute keys.

### Desktop and physical-handheld contract

| Action | Keyboard / mouse | Physical handheld |
|---|---|---|
| Move | WASD or arrows | D-pad |
| Jump | Space | B |
| Interact / Mount / Dismount | E | SELECT |
| Light / Spin | J or left click / hold | X / hold X |
| Heavy / Hook | K or right click / hold | Y / hold Y |
| Ranged | Q | A |
| Heal | Hold F | Hold A |
| Dodge | Double-tap a movement direction; the side-view tutorial uses A/D or Left/Right | Double-tap D-pad Left/Right |
| Parry | Time either Light or Heavy immediately before a valid impact | Time X or Y immediately before a valid impact |

Parry is a result of melee timing and is not presented as a separate dedicated button. Dodge completion requires the second directional tap; a single tap remains movement. Input labels and the action consumer must derive from the same semantic contract.

### Checkpoint presentation

Death recovery is an explicit sequence:

`death pose -> character fade -> opaque checkpoint cover -> restore stable snapshot -> controlled reveal`.

The checkpoint transform may change only while the cover is opaque. The cached cover and label are reused and produce no per-recovery material or texture allocation. Combat input, holds, reload and transient effects remain cancelled before restoration.

### Intro/menu presentation boundary

When first-launch tutorial state is pending, the modern presenter reserves the presentation before `BDMainMenuFlow` resolution. Legacy-menu suppression includes that reservation. `ResolveEffectivePage` chooses the tutorial before falling back to Main Menu for a temporarily unresolved flow. This prevents any one-frame legacy or stale-main-menu exposure after the BBH intro.
<!-- BND_FIRST_LAUNCH_TUTORIAL_V10_INPUT_RESPAWN_FLASH_REPAIR_V102:END -->

<!-- BND_FIRST_LAUNCH_TUTORIAL_ENTRY_GATE_V103:BEGIN -->
## V10.3 entry decision

Before tutorial gameplay, an unresolved first-launch state presents a black pixel-style handheld page with `B&D`, `Boredom & Dungeons`, `PLAY TUTORIAL` and `SKIP TUTORIAL`.

`PLAY TUTORIAL` writes `InProgress` after confirmation and then begins the existing tutorial course. `SKIP TUTORIAL` persists a terminal no-auto-replay state and returns to the normal modern main menu. Both routes must remain covered until the correct next page is ready; no legacy or stale page may be exposed.

The authoritative expanded contract and animation backlog are in `FIRST_LAUNCH_TUTORIAL_ENTRY_AND_ANIMATION_V11.md`.
<!-- BND_FIRST_LAUNCH_TUTORIAL_ENTRY_GATE_V103:END -->

<!-- BND_FIRST_LAUNCH_TUTORIAL_PROGRESSION_GATE_REPAIR_V104:BEGIN -->
## V10.4 forward-progress and gate rule

A tutorial progression boundary may use a coordinate clamp only when the instruction director clearly states that the current lesson must be completed. V10.8 removes the rejected visible pixel-world gate/divider treatment.

Lesson actors must spawn at or ahead of the expected player entry position. The post-dismount order is mounted shot → Reload → mounted impact → dismount → Spin → Grapple → hazard → side path → combined encounter → final test.

While mounted, the boy tutorial supports ranged combat only. Light, Heavy, Spin and Grappling Hook requests are rejected without creating an attack transaction.
<!-- BND_FIRST_LAUNCH_TUTORIAL_PROGRESSION_GATE_REPAIR_V104:END -->

<!-- BND_FIRST_LAUNCH_TUTORIAL_MECHANICS_REPAIR_V108:BEGIN -->
## V10.8 mechanics/readability correction

V10.8 supersedes the V10.4 requirement for visible lesson gates. The course may retain invisible progression clamps to prevent skipping an untaught dependency, but it may not draw decorative boundary lines or gate bars between lessons. A blocked player receives contextual instruction feedback instead.

The following are authoritative:

- injured/red horse state is not mountable; healing must complete first;
- living tutorial actors have blocking body volume and cannot be traversed by ordinary walking/riding;
- walking/riding/chasing uses explicit alternating leg frames for player, horse and enemies;
- tutorial ranged damage is an impact transaction: projectile presentation reaches impact before target damage, death or lesson advancement;
- tutorial Hook is a completion transaction: target pull presentation finishes before damage and advancement;
- checkpoints are updated at safe entry points for major lessons so recovery remains local;
- final-boss HUD/instruction remains visible, attacks state `TELEGRAPH`, `IMPACT` and `RECOVER`, and the safe attack opening is the recovery window;
- Phase 2 ranged damage comes from a visible projectile transaction rather than a distant timer hit.

### Charged Shot parity

The tutorial mirrors `BDPlayerCombat` instead of inventing a release-to-fire action:

1. the ranged press remains pending for `0.22s`;
2. release before that threshold produces an ordinary shot and does not complete the lesson;
3. after threshold, charge duration is `min(3.20s, 0.90s + 0.45s × ammunition above two)`;
4. release during active charge cancels it;
5. reaching full charge fires automatically while the input is still held;
6. the shot consumes all ammunition remaining in the magazine;
7. Reload starts immediately;
8. lesson completion waits for the charged projectile impact and resulting Reload completion.

No separate combat owner is introduced. The tutorial transaction layer presents and forwards completion to the existing tutorial actor/damage/learning-state owners.
<!-- BND_FIRST_LAUNCH_TUTORIAL_MECHANICS_REPAIR_V108:END -->
<!-- BND_TUTORIAL_CONTACT_DIRECTION_TRAVERSAL_SKIP_V1010:BEGIN -->
## Contact, facing and traversal teaching contract

Tutorial attacks are owned by player facing and apply damage only at visible contact. Airborne attack is taught against a normal grounded enemy. The final lesson before the boss is a physical wall-jump sequence with a reachable wall on the right, a clearly separated platform on the left, and upper ground to the right above the wall. Boss attacks must visibly communicate windup, attack lane, impact and recovery. Ordinary walking cannot pass through active obstacles or living enemies.
<!-- BND_TUTORIAL_CONTACT_DIRECTION_TRAVERSAL_SKIP_V1010:END -->
