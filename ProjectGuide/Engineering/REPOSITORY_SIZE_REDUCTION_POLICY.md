# Repository Size Reduction Policy

## Objective

Reduce local and tracked repository size only through measured, lossless, reversible and behavior-neutral work. The game, visual output, code, scenes, assets, documentation, GUIDs and history must remain intact.

## Allowed automatic cleanup

The maintenance tool may remove only untracked generated local artifacts whose regeneration path is established, including Unity `Library`, `Temp`, `Logs`, `obj`, editor caches, Python caches and OS metadata. Before removal, it verifies that Git tracks no file under the candidate path.

## Report-only categories

The following are analyzed but never modified automatically:

- duplicate tracked files;
- PNG or other asset optimization candidates;
- large historical Git objects;
- Git LFS candidates or migrations;
- tracked build outputs;
- source-art, audio, model or archive files;
- any file with uncertain ownership or dynamic references.

## Absolute prohibitions

- no lossy compression or resolution/quality reduction;
- no Unity GUID or `.meta` changes for size reduction;
- no documentation deletion, shortening or archival that harms search/access;
- no history rewrite, force push, rebase, filter-repo or BFG;
- no LFS history migration;
- no automatic Git commit, push, branch or pull request;
- no assumption that an asset is unused from text search alone.

## Evidence

`repository_size_audit_and_cleanup.py` writes stable latest reports containing before/after sizes, largest files/directories, extension totals, duplicate hashes, removed generated paths, historical object candidates, LFS status/candidates and untouched-risk notes. The tool records every decision and leaves uncertain items untouched.

<!-- BND_UNITY_UI_PACKAGE_RECOVERY_V3:BEGIN -->
## Package and Unity-cache exclusion rule

Repository size reduction is allowed to remove reproducible Unity caches only while the Editor is closed. It must treat `Packages/` as a protected dependency boundary, must not follow package symlinks, and must not mutate immutable package contents. Package overlays and package-resolution state are repaired separately through `tools/repair_unity_package_state.py`.
<!-- BND_UNITY_UI_PACKAGE_RECOVERY_V3:END -->
