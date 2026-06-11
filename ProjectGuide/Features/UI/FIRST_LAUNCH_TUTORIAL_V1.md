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
| Interact / confirm | SELECT | E (Enter/Space also accepted for menu confirmation) | bound Interact / Confirm control |
| Light attack / spin hold | X | Left Mouse or J | bound Light Attack control |
| Heal horse | A | hold F | bound Horse Heal control |
| Dodge | B | Space | bound Dodge control |
| Heavy attack | Y | Right Mouse or K | bound Heavy Attack control |
| Parry | Y | Q at impact | bound Parry control |
| Grapple | Y | hold Right Mouse or K | bound Heavy / Grapple control |
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

Tutorial attacks are owned by player facing and apply damage only at visible contact. The early traversal course teaches a grounded jump, an airborne forward attack, then a physical wall-jump sequence with the wall on the right, a raised platform on the left, and upper ground to the right above the wall. Ordinary walking cannot pass through active obstacles or living enemies.
<!-- BND_TUTORIAL_CONTACT_DIRECTION_TRAVERSAL_SKIP_V1010:END -->
