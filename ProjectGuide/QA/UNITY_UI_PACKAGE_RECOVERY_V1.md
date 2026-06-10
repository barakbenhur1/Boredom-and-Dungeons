# Unity UI Package Recovery V1

```text
Status: RECOVERY IMPLEMENTED / UNITY VERIFICATION REQUIRED
Package: BND_UNITY_UI_PACKAGE_RECOVERY_V3
Observed Unity: 6000.0.76f1
```

## Trigger

`TEST EVERYTHING` reported one blocker because the handheld presenter and
first-launch tutorial could not resolve `UnityEngine.UI`, `Image`, `Text`,
`RawImage`, or `Outline`.

The project manifest already declares `com.unity.ugui` and
`com.unity.modules.ui`. The same Editor session also reported:

- symbolic package overlays under `Packages/com.unity.*`;
- unexpectedly altered immutable package contents;
- a missing `Library/Search/propertyDatabase.db.st` path.

This is a package-resolution/cache-state failure. It is not repaired by
rewriting valid gameplay or UI source to avoid UGUI.

## Recovery contract

1. Unity must be fully closed.
2. Preserve `Packages/manifest.json` and a valid package lock.
3. Never remove tracked or explicit `file:`, Git, SSH, or HTTP packages.
4. Move only untracked generated `Packages/com.unity.*` overlays to an external
   rollback backup.
5. Delete only reproducible package, script-compilation, Bee, Search and artifact
   caches.
6. Preserve all Assets, ProjectSettings, source, documentation and `.meta` files.
7. Reopen Unity and allow Package Manager/import/compilation to finish.
8. Rerun `Boredom And Dungeons → TEST EVERYTHING`.
9. Do not commit until the real Unity result and Play Mode verification pass.

## Verification

- `Packages/manifest.json` contains `com.unity.ugui` and
  `com.unity.modules.ui`.
- A present `packages-lock.json` contains both required entries.
- No untracked generated Unity package overlay remains at the top of
  `Packages/`.
- Both handheld source files still import `UnityEngine.UI`.
- Package recovery creates an external rollback backup and a JSON evidence
  report.
- Unity compilation is never inferred from static checks.
