<!-- BND_FIRST_LAUNCH_TUTORIAL_V1081_HOTFIX:BEGIN -->
## V10.8.1 focused additions

- In mounted shooting, capture three moments: immediately after trigger, immediately before impact and immediately after impact. The enemy/step may change only in the third moment.
- Confirm the impact enters Reload and then Charged Shot without an extra shot, movement trick or page reload.
- Confirm a miss cannot complete the lesson.
- During the post-BBH landing, compare the device/table/shadow transforms at opening, midpoint and final frame; they must remain identical. Only camera transform/FOV may differ.
- Confirm the table covers the full viewport throughout the move on the tested aspect ratio.
- Re-run every V10.8 focused check; this hotfix does not waive prior acceptance conditions.
<!-- BND_FIRST_LAUNCH_TUTORIAL_V1081_HOTFIX:END -->

# First-Launch Tutorial Production Course V10 — QA Contract

```text
Status: IMPLEMENTED IN LOCAL PACKAGE / UNITY VERIFICATION REQUIRED
Scope: first-launch 2D playable tutorial only
Target duration: 5–8 minutes
```

## Automated gate

- Unity compiles with no project C# errors.
- `Boredom And Dungeons -> TEST EVERYTHING` reports 0 blockers, 0 warnings and 0 info unless a separately documented infrastructure finding is explicitly accepted.
- The production-course partial, its `.meta`, the tutorial state store, reset tool and maintained documents exist.
- One tutorial lifecycle owner, one instruction owner, one player movement owner, one camera owner and one damage-resolution path are retained.
- No Runtime dependency on `UnityEditor` is introduced.
- No per-prompt texture, material, mesh, collection or GameObject allocation is introduced.

## Full-run matrix

1. Keyboard and mouse only.
2. Controller only.
3. Clickable physical handheld controls only.
4. Mixed input in one run, including switching while an instruction is visible.
5. Fast completion.
6. New-player pace.
7. Intentional mistakes and checkpoint recovery.
8. Optional secret skipped.
9. Optional secret collected.
10. Confirmed tutorial leave at every major section.
11. Application interruption mid-tutorial and safe restart.
12. Completion followed by application restart; automatic replay must remain suppressed.
13. Confirmed leave followed by application restart; automatic replay must remain suppressed.

## Course and instruction checks

- The course is traversable from start to finish and is not a sequence of static button panels.
- Movement, jump, horse, basic combat, Tap/Hold, environmental knockback, mounted combat, optional exploration, combined encounter and Mini-Boss are introduced in that order.
- Only one large instruction is visible at a time.
- The instruction appears only when the action is available and relevant.
- The active input source changes the Keycap without replaying the entire entrance animation.
- Tap and Hold are visually distinct.
- Failed attempts do not count as demonstrated actions.
- Hint escalation is staged and never completes the action automatically.
- The secret receives no objective, marker, empty HUD slot or advance prompt.

## Gameplay checks

- Jump requires leaving the ground, clearing the obstacle and landing legally.
- Mount and Dismount complete through readable action presentation and safe placement.
- Light and Heavy are distinct.
- Spin does not also emit a normal Light attack and can affect multiple valid targets.
- Hook is blocked by eligibility rules; the Mini-Boss is never pulled.
- Dodge i-frames prevent a legal attack when timed correctly.
- Contextual Parry cancels a committed valid attack and creates recovery.
- Mounted Boy combat remains ranged-only.
- Reload blocks firing until completion.
- Mounted impact requires movement and affects only a small valid enemy.
- Environmental defeat is possible but not mandatory for progression.
- Enemy damage occurs only after visible windup/commit timing.
- No attack damages twice from one transaction.

## Failure, reset and cleanup

- Player defeat shows a short readable death pose before checkpoint restoration.
- Combined encounter and Mini-Boss each have a nearby checkpoint.
- Reset clears pending Tap/Hold state, Reload, projectiles, Hook state, active instructions and attack transactions.
- The optional secret is not duplicated by Retry.
- The Mini-Boss phase transition occurs once per attempt.
- Boss death disables damage first, completes visible death, then opens the gate once.
- No projectile, rope, event, coroutine, hitbox or input lock survives completion, leave, death, reset, page change or presenter destruction.

## Timing and presentation acceptance

- Three representative runs are timed.
- Fast target: about 5 minutes.
- Average new-player target: 6–7 minutes.
- Explorer/slightly struggling target: no more than about 8 minutes.
- No area relies on excessive enemy health, long empty walking, repeated forced practice or long unskippable text.
- All required action animations remain readable at the real handheld screen size.
- Final acceptance requires the user's visual and gameplay approval.

<!-- BND_FIRST_LAUNCH_TUTORIAL_V10_WARNING_CLEANUP_V101:BEGIN -->
## V10.1 warning cleanup addendum

The first Unity compile exposed six write-only demonstration booleans. They did not control progression and duplicated the explicit learning-state dictionary. V10.1 removes them and requires direct learning-state writes for all affected lessons. Acceptance now explicitly requires zero compiler warnings in addition to TEST EVERYTHING `0 / 0 / 0`.
<!-- BND_FIRST_LAUNCH_TUTORIAL_V10_WARNING_CLEANUP_V101:END -->

<!-- BND_FIRST_LAUNCH_TUTORIAL_V10_INPUT_RESPAWN_FLASH_REPAIR_V102:BEGIN -->
## V10.2 focused regression matrix

### Input truth
- Space performs Jump and the displayed keyboard keycap says `SPACE`.
- One A/D or Left/Right tap moves; the second same-direction tap inside `0.30s` performs one Dodge.
- Physical B performs Jump.
- One physical D-pad Left/Right press moves; the second matching press performs one Dodge.
- During Parry, J/left click and K/right click are both accepted; physical X and Y are both accepted.
- No Space Dodge, W/Up Jump or dedicated K-only Parry remains.

### Checkpoint recovery
- lethal damage shows a readable death pose and character fade;
- the screen reaches full cover before the checkpoint position changes;
- `RETURNING TO CHECKPOINT...` is readable during cover;
- restored player fades in with clean rotation/scale and no stale action;
- repeated deaths do not stack covers, duplicate reset or leave the player transparent.

### BBH handoff
- reset tutorial, exit Play Mode, start from a fresh application/session;
- inspect the final BBH frames and first handheld frames at normal and slowed capture;
- no old menu, legacy IMGUI, blank plain menu or wrong page appears between them.
<!-- BND_FIRST_LAUNCH_TUTORIAL_V10_INPUT_RESPAWN_FLASH_REPAIR_V102:END -->

<!-- BND_FIRST_LAUNCH_TUTORIAL_MECHANICS_REPAIR_V108:BEGIN -->
## V10.8 focused regression matrix

### Mechanics and timing

- [ ] Attempt Mount while the horse is injured/red: Mount is rejected, no mount animation starts and the instruction says to heal first.
- [ ] Walk on foot, ride the horse and observe at least one moving enemy: legs alternate visibly; idle bodies do not continue a fake locomotion bob.
- [ ] Fire during the mounted-shot lesson: the target remains alive until projectile impact; impact then applies one damage transaction and advances to Reload.
- [ ] Tap Ranged and release before `0.22s`: one ordinary shot appears; Charged Shot does not complete and the lesson remains retryable.
- [ ] Hold past `0.22s`, then release before full charge: charge cancels; no charged projectile or completion occurs.
- [ ] Hold continuously: full charge auto-fires without release, consumes the full magazine, begins Reload and advances only after impact plus Reload completion.
- [ ] Start Hook: target travels with the rope/pull animation; target damage and next-step transition occur only after animation completion.
- [ ] Walk/ride into each living enemy and the boss: body contact blocks traversal; dead/inactive actors no longer block.
- [ ] Die during at least three late lessons: covered restore returns to a nearby safe point, not the beginning or several lessons back.

### Course and boss readability

- [ ] No horizontal/vertical divider line or visible lesson-gate stripe marks section transitions.
- [ ] Attempt to skip a required lesson: invisible clamp and contextual feedback prevent bypass without drawing a world-space barrier.
- [ ] Boss instruction remains long enough to read and stays available while the encounter is active.
- [ ] Phase 1 melee shows telegraph, committed impact and recovery; approaching does not create unexplained instant death.
- [ ] Phase 2 shot is a visible projectile captured at release and can miss when the player moves away from its target point.
- [ ] Player attacks during committed boss attack are rejected with readable feedback; attacks during recovery damage exactly once.
- [ ] Boss defeat/completion occurs only after the legal second-phase damage window.

### Cinematic handoff

- [ ] The post-BBH shot begins on the real active 3D product scene at full viewport size.
- [ ] Camera position, look direction, FOV and real device/shadow pose move continuously in 3D.
- [ ] No screen-space card, flat overlay, scaling slide, black correction frame or perspective snap appears.
- [ ] Final camera/device/shadow pose equals the ordinary menu pose exactly and input unlocks only after restoration.
<!-- BND_FIRST_LAUNCH_TUTORIAL_MECHANICS_REPAIR_V108:END -->
