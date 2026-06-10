# First-Launch Tutorial and Modern Handheld Production Repair

```text
Status: IMPLEMENTED IN LOCAL PATCH PACKAGE / UNITY VERIFICATION REQUIRED
Classification: CURRENT + EARLIER/BLOCKING REPAIR
Package base: origin/main at 17d5c39435b5fa075d3e606c4d2e43b8f824b03f
Git writes by assistant: PROHIBITED
Delivery: local backup-aware patch ZIP
```

## Accepted scope

This task combines the user-approved handheld correction pass, the first-launch tutorial contract, the supplied BBH cinematic side task, and the permanent repository delivery/production-code rules. Nothing in this task changes the ordered `WORK_QUEUE.md`.

### Modern handheld

- keep the device slightly lower in the table composition, with its cast/contact shadows moving from the same authoritative rest position;
- restore the large context image to the original title-aligned composition, not the Start Game row-aligned override;
- retain the text-only contextual card below the image for every Main Menu selection and update its heading/body from the selected action;
- fix the broken `THE MAZE AWAITS` card and enforce bounded text layout across Main, Pause, Settings, Progression, Credits and confirmation pages;
- move SELECT and EXIT inward through the authoritative model builder so labels and hit targets inherit the same positions;
- make Settings icon selection font-safe;
- remove blue physical-button hover emission/frame behavior;
- implement D-Pad, SELECT, EXIT and X/Y/A/B press depth and scale response directly in `BDModernHandheldControlTarget`;
- make Escape/Pause a dedicated internal handheld-screen menu instead of a Main Menu product-card clone;
- remove the merged V6 companion/compatibility classes after their accepted behavior is integrated into the real owners.

### First launch

- on `NotStarted` or interrupted `InProgress`, show white boot light then a deterministic 2D tutorial inside the handheld glass before Main Menu content;
- teach one mechanic at a time through an isolated scripted sequence;
- preserve the required horse/enemy/heal opening event;
- use the existing normalized handheld actions and actual project keyboard/controller bindings represented by the tutorial input map;
- support physical controls, keyboard, controller, mouse and touch without duplicate action execution;
- always show large modern keyboard/mouse and physical-handheld instructions in parallel;
- EXIT opens a tutorial-native confirmation panel with safe default Continue and an input guard;
- confirmed abandonment writes `Skipped` before transition and permanently suppresses automatic replay;
- successful completion writes `Completed`; interruption remains safely restartable;
- grant no run reward, progression, statistics or production-world mutation.

### BBH side task

Integrate the supplied cinematic BBH letter-motion/circle refinement in the existing `BDBBHBootIntro` owner and add its check to `TEST EVERYTHING`. The active task order is not changed.

## Production-code requirement

Every changed or encountered production area must follow `ProjectGuide/Rules/PRODUCTION_CODE_STANDARD.md`. The implementation must use existing authoritative owners, explicit state, deterministic cleanup and testable contracts. A parallel component may be added only when it represents a real independent responsibility; it may not be used to disguise an avoidable patch over the owning system.

## Files and ownership

- `BDModernHandheld3DPresenter.cs`: authoritative device composition, screen pages, input routing and page transition integration.
- `BDModernHandheld3DPresenter.FirstLaunchTutorial.cs`: cohesive partial of the same owner for the isolated tutorial screen mode.
- `BDModernHandheldControlTarget.cs`: authoritative physical hover/press/highlight presentation.
- `BDFirstLaunchTutorialStateStore.cs`: one versioned durable display-decision owner.
- `BDBBHBootIntro.cs`: existing BBH cinematic owner.
- `BDOneClickQAWindow.cs`: single project QA entry point.

## Removed implementation debt

The following merged V6 companion approach is retired after direct integration:

- `BDModernHandheldV6Polish*`;
- `BDModernHandheldTactileCompatibility`;
- `BDModernHandheldPressScaleFeedback`;
- `BDModernHandheldV6QA` and its temporary V6 review/task/QA documents.

No scene, prefab, art, texture, shader or gameplay file is removed by this retirement.

## Required verification

1. Apply with the supplied one-command installer.
2. Run its validator and repository scans.
3. Open Unity `6000.0.76f1` and wait for compilation.
4. Run only `Boredom And Dungeons -> TEST EVERYTHING`.
5. Require `0 blockers / 0 warnings / 0 info` unless the user explicitly accepts a separately explained infrastructure finding.
6. Complete the focused handheld, tutorial and BBH Play Mode checklist supplied in the package.
7. Send the actual Unity output and screenshots before any commit.
8. Only after acceptance, create the local commit using the supplied command. The assistant never commits or pushes.

## Exact resume point

After package installation and static validation, resume at Unity compilation followed by `TEST EVERYTHING`, then the focused Play Mode list in `VERIFY_AFTER_APPLY.txt`.

<!-- BND_TUTORIAL_REFERENCE_LED_V3:BEGIN -->
## Active V3 refinement — reference-led clean tutorial

- apply the approved remembered-console fantasy palette;
- keep the world sparse and readable;
- reserve edge decorations away from gameplay actors;
- make the modern instruction card the dominant screen element;
- split keyboard/mouse and handheld bindings into large parallel cards;
- keep both input families active simultaneously;
- use only basic stepped pixel animation where it supports comprehension;
- reject any clipping, overlap or tiny instructional copy;
- complete automated and three-route Play Mode verification before returning to the ordered work queue.
<!-- BND_TUTORIAL_REFERENCE_LED_V3:END -->
