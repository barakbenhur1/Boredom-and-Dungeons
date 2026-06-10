# First-Launch Tutorial QA Contract Correction V4

## Root cause

The V7 validator required the impossible contiguous source token
`HANDHELD  HOLD Y`.

The Runtime implementation intentionally composes these values separately:

- `HANDHELD` is the title of the physical-control card.
- `HOLD Y` is the Grapple binding returned by
  `ResolveFirstLaunchTutorialHandheldBinding`.

The Runtime tutorial was correct. The automated check was wrong.

## Correct contract

The focused validator now verifies independently:

1. the `HANDHELD` card title;
2. the handheld binding resolver;
3. the `GRAPPLE` action case;
4. `return "HOLD Y";`.

No dead string or fake display content is added to Runtime code.

## Scope

- focused Editor QA source;
- maintained bug, verification, QA history, technical-decision,
  production-standard and index records;
- no gameplay, tutorial presentation, scene, prefab, package or project
  setting changes;
- no Git write operation.

## Required verification

1. Wait for Unity to recompile.
2. Clear Console.
3. Run `Boredom And Dungeons → TEST EVERYTHING`.
4. Confirm the tutorial contract blocker is gone.
5. Continue the full Play Mode tutorial acceptance pass.
