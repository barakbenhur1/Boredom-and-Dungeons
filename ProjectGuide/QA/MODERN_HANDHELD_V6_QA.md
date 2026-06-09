# Modern Handheld V6 QA

```text
Status: REQUIRED BEFORE MERGE
Branch: codex/handheld-v6-layout-polish
Unity result: NOT YET RUN
Play Mode result: NOT YET VERIFIED
User approval: NOT YET RECEIVED
```

Unity compilation, `TEST EVERYTHING`, focused Play Mode checks and user visual approval are mandatory before this review branch may be merged.

## Automated gate

- [ ] Unity compiles with no C# errors.
- [ ] `Boredom And Dungeons → TEST EVERYTHING` completes.
- [ ] Blockers: `0`.
- [ ] Warnings: `0`, or every warning is separately explained and approved.
- [ ] Info findings: `0`, or every item is separately explained.
- [ ] No missing script metadata or duplicate GUIDs.
- [ ] No scene, prefab, texture, shader or artwork was removed.

## Product-shot composition

- [ ] The device is visibly lower than V5.
- [ ] The device is not clipped by either vertical edge.
- [ ] The lower margin is intentionally smaller than the upper margin.
- [ ] Device and cast/contact shadows move together.
- [ ] The table texture, glass, shell and lighting remain otherwise unchanged.

## Main Menu image and contextual card

- [ ] The top of the hero image aligns with the top of Start Game.
- [ ] The card below the image remains visible for every Main Menu option.
- [ ] Start Game shows adventure-memory copy.
- [ ] Progression shows persistent-memory copy.
- [ ] Settings shows configuration copy.
- [ ] Credits shows people/production copy.
- [ ] Quit shows confirmed-exit copy.
- [ ] The small card stays text-only.
- [ ] Only Start Game/New Run uses Boy/Girl artwork.

## Text containment

- [ ] `THE MAZE AWAITS` is fully contained.
- [ ] Its body copy is fully contained.
- [ ] Main titles, subtitles, rows and badges do not overlap.
- [ ] Settings labels, values and badges remain inside their rows.
- [ ] Progression panel text remains inside its panel.
- [ ] Credits text remains inside its panel.
- [ ] Exit and abandon confirmation text remains contained.
- [ ] Footer text remains inside the screen bezel.
- [ ] Repeat at narrow, standard and wide landscape resolutions.

## Physical controls and Settings icon

- [ ] SELECT and EXIT are closer to the center and to each other.
- [ ] Neither overlaps D-pad, face buttons, labels or speakers.
- [ ] Their physical click areas match their new positions.
- [ ] Their labels remain centered below the correct buttons.
- [ ] Settings always shows a visible icon rather than an empty square.

## Tactile parity

- [ ] Each D-pad direction visibly presses and returns.
- [ ] SELECT visibly presses and returns.
- [ ] EXIT visibly presses and returns.
- [ ] X, Y, A and B retain their existing press behavior.
- [ ] All physical controls read as one depth/speed/compression family.
- [ ] Hover remains weaker than press.
- [ ] Repeated input never leaves a control stuck or scaled.
- [ ] Animation remains responsive while paused.

## Escape/Pause internal menu

- [ ] Escape opens and keeps the handheld visible.
- [ ] Pause no longer uses the Main Menu hero composition.
- [ ] Pause actions are presented inside one internal screen panel.
- [ ] Resume remains first and selected initially.
- [ ] Progression and Settings open their existing pages.
- [ ] Return to Main Menu still opens abandon confirmation.
- [ ] Mouse hit areas match the moved Pause rows.
- [ ] D-pad, arrows, WASD and gamepad remain equivalent.
- [ ] No gameplay HUD appears above Pause.

## Regression gate

- [ ] Main X starts New Game.
- [ ] Main A opens Progression.
- [ ] Main B opens Settings.
- [ ] B acts as Back on non-main pages.
- [ ] Main Y opens Credits.
- [ ] SELECT activates focus.
- [ ] EXIT opens confirmation and never exits immediately.
- [ ] Run, death, loading and abandon behavior is unchanged.
- [ ] Character-specific artwork rules remain unchanged.
- [ ] Legacy menu suppression remains single-owner.

## Evidence record

```text
Unity version:
UTC date/time:
Commit SHA:
TEST EVERYTHING result:
Blockers:
Warnings:
Info:
Resolutions tested:
Input devices tested:
Play Mode notes:
User approval:
Remaining defects:
```
