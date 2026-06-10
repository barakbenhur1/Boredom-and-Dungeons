# First-Launch Tutorial V1

```text
Status: IMPLEMENTED / UNITY VERIFICATION REQUIRED
Classification: CURRENT FEATURE + STARTUP-SAFETY GATE
Runtime owner: BDModernHandheld3DPresenter first-launch partial
Persistent owner: BDFirstLaunchTutorialStateStore
Normal-run side effects: NONE
```

## Purpose

A clean installation opens the existing physical handheld product shot, but the normal Main Menu is not shown inside the glass yet. A white boot light reveals a short, deterministic two-dimensional tutorial. Completion or confirmed abandonment permanently suppresses automatic replay and returns smoothly to the normal Main Menu inside the same handheld screen.

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
2. The screen content begins as a white boot light.
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

37. White boot light reveals tutorial content without a flash or black-frame regression.
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
