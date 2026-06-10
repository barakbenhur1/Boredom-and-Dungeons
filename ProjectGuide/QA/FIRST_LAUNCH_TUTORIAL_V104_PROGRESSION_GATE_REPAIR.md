# First-Launch Tutorial V10.4 — Progression Gate Repair

Status: `IMPLEMENTED / UNITY VERIFICATION REQUIRED`

## Defects corrected

- The Spin and Grapple lesson actors were still spawned at obsolete early-course coordinates behind the player after dismount. The player could therefore reach a hard forward clamp with no visible target and no way to complete the current lesson.
- V10.4 originally rendered a reusable pixel lesson gate at active boundaries. V10.8 supersedes that presentation: the coordinate clamp remains invisible and an explicit contextual completion message appears when the player presses into it, because visible section-divider lines were rejected in Play Mode.
- The mounted ranged lesson asked only for a shot but required emptying a full magazine before the Reload lesson could begin. V10.4 starts that lesson on the final round so one valid mounted shot transitions directly into the automatic reload demonstration.
- Sword and heavy/Hook actions were still accepted while mounted in tutorial-only input paths. V10.4 rejects them consistently and clears pending hold state.
- The pre-tutorial choice used a smooth UI font rather than actual pixel glyphs. V10.4 renders the title, subtitle, options and status through a point-filtered bitmap glyph renderer.

## Forward course order

1. mounted ranged shot at the mounted station;
2. automatic reload;
3. mounted impact;
4. visible dismount marker;
5. Spin targets at `TutorialSpinTargetX`;
6. Grapple target across `TutorialGapX`;
7. hazard knockback at `TutorialHazardStationX`;
8. side path;
9. combined encounter;
10. final test and completion.

No lesson target may be authored behind the player's expected entry point unless the lesson explicitly asks the player to backtrack.

## Acceptance

- The player can complete every lesson in forward order without hidden blockers.
- Every active hard progression boundary is enforced without visible gate/divider geometry and is explained through the instruction/feedback UI.
- A mounted Light, Heavy, Spin or Hook request does not attack.
- One mounted ranged shot begins the Reload lesson.
- The entry-choice typography is visibly pixelated.
- Completion persists and returns through the existing clean transition.
- Unity compilation and `TEST EVERYTHING` are clean.

<!-- BND_FIRST_LAUNCH_TUTORIAL_MECHANICS_REPAIR_V108:BEGIN -->
## V10.8 supersession note

The visible-gate visual from V10.4 is retired. The dependency order and coordinate clamp remain valid, but no line, bar or divider may be drawn between course sections. V10.8 QA owns this replacement requirement.
<!-- BND_FIRST_LAUNCH_TUTORIAL_MECHANICS_REPAIR_V108:END -->
