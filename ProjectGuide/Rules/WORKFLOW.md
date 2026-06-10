# Boredom & Dungeons — Mandatory Development Workflow

This document defines **how work must be performed** on the project.

It is a permanent process contract. It does not duplicate the current feature
status or the complete requirement list.

- `AGENTS.md` is the canonical project-wide Codex/AI operating contract.
- `.codex/config.toml` and `.codex/agents/*.toml` are maintained project
  configuration and specialist-agent profiles.
- `ProjectGuide/Status/CURRENT.md` is the only authoritative source for product requirements,
  current progress, QA truth, blockers, ordering, and the exact resume point.
- `ProjectGuide/Rules/WORKFLOW.md` is the authoritative source for the working method.
- `README.md` points contributors to these entry points.

No parallel status document, copied roadmap, temporary progress file, or chat-only
decision may compete with these sources.

---

## 1. Core operating rules

1. Preserve the current project.
2. Work additively by default.
3. Never remove code, assets, design, colors, files, scene content, or requirements
   unless the user explicitly approves the removal or the item is a temporary test
   artifact whose cleanup is already part of the accepted task.
4. Never silently replace a requirement.
5. Every requirement change must be recorded in `ProjectGuide/Status/CURRENT.md` before or in the
   same package as the implementation.
6. Every material code change must update the authoritative documentation and the
   relevant QA contract in the same working tree.
7. The assistant does not commit or push directly.
8. The assistant delivers a ZIP package and then provides exact local Git commands.
9. The user performs the final Unity verification and Git commit/push.
10. A task is not DONE merely because code exists.

---

## 2. Request intake and ordering

Every new request is classified before implementation.

### Earlier than the current work

When the request must logically happen before the current item:

1. add it to the correct place in `ProjectGuide/Status/CURRENT.md`;
2. save the current resume point;
3. implement the earlier item immediately;
4. validate it;
5. update QA truth and documentation;
6. return to the saved resume point.

### Inside the current item/category

Update the current requirement and implement it in dependency order.

### Later than the current work

Add it to the correct future location in `ProjectGuide/Status/CURRENT.md`, then continue the
current item.

### Cross-cutting regression

A regression that affects startup, death/restart, core movement, combat, scene
safety, data integrity, or QA reliability blocks later feature work and is handled
as an earlier task.

### Category transition

Before entering a new category:

1. finish the previous category acceptance gate;
2. synchronize code, QA, and `ProjectGuide/Status/CURRENT.md`;
3. state which category was completed;
4. state the proposed next category;
5. ask the user for additions or changes;
6. record the answer;
7. continue only after approval.

---

<!-- B&D PERMANENT REQUEST CAPTURE V8 START -->
## 2A. PERMANENT USER-REQUEST CAPTURE CONTRACT

This is a standing project instruction. **The user never needs to repeat this instruction in a later chat or task.**

Every user request, correction, idea, constraint, or changed priority must be recorded in the correct logical location in `ProjectGuide/Status/CURRENT.md` before implementation or in the same package as implementation. No material request may remain chat-only.

For every new request:

1. inspect the repository and relevant maintained documents;
2. record the request in `ProjectGuide/Status/CURRENT.md` at its correct dependency/category position;
3. classify it as `EARLIER/BLOCKING`, `CURRENT`, `LATER`, or `UNKNOWN/RECOVERY REQUIRED`;
4. preserve the exact current resume point before interrupting active work;
5. implement immediately only when the request is earlier/blocking or belongs to the current work;
6. when the request is later, document it and continue the current item without abandoning the saved order;
7. update every affected design, architecture, QA, technical-decision, and performance document;
8. record real verification truth and the exact resume point.

Classification rules:

- `EARLIER/BLOCKING`: regression, compile/build failure, conflict marker, broken QA, corrupted scene/data, documentation/workflow failure, or missing prerequisite. Stop later feature work, repair now, validate, then return to the saved resume point.
- `CURRENT`: part of the active category/item. Implement now in dependency order.
- `LATER`: future work that does not block the active item. Record it in the correct future section and continue current work.
- `UNKNOWN/RECOVERY REQUIRED`: evidence is incomplete. Preserve the request without inventing details.

When a maintained document is added, renamed, superseded, or changes responsibility, update `ProjectGuide/INDEX.md`. When system boundaries change, update `ProjectGuide/Engineering/ARCHITECTURE.md`. When a durable technical choice changes, update `ProjectGuide/Engineering/TECHNICAL_DECISIONS.md`. When measurement policy or budgets change, update `ProjectGuide/Engineering/PERFORMANCE.md`.
<!-- B&D PERMANENT REQUEST CAPTURE V8 END -->

## 3. Repository inspection before editing

Before creating a package:

1. inspect the latest repository state;
2. read `README.md`, `ProjectGuide/Status/CURRENT.md`, relevant design files, relevant runtime
   code, scene installers/builders, and QA checks;
3. account for known local uncommitted changes;
4. do not assume Git `main` already contains the latest local work;
5. avoid patches that depend on fragile whitespace when a structural method or
   token-based edit is possible;
6. preserve Unity GUIDs and `.meta` files;
7. do not add runtime dependencies on `UnityEditor`.

When an earlier installer partially succeeded, the repair package must continue
from the actual partial state. Do not instruct the user to reset valid local work.

---

## 4. Mandatory ZIP delivery

Every material change is delivered as a downloadable ZIP.

### Preferred package size

- Prefer a ZIP containing the complete updated project when practical.
- When the full project is too large, include every changed directory and every
  file required to apply and validate the change.
- Never omit a changed `.meta`, scene, prefab, design, QA, or status file.

### Required package contents

A normal package contains:

```text
.package_payload/
.package_tools/
README_<PACKAGE>.txt
PACKAGE_MANIFEST_<PACKAGE>.txt
```

The package must provide:

- an idempotent installer;
- an idempotent validator;
- preflight checks against the expected current state;
- explicit failure messages;
- no automatic commit or push;
- no destructive reset, checkout, clean, or pull;
- documentation updates;
- QA-contract updates when behavior changes.

### Package testing

Before delivery:

1. syntax-check package tools;
2. test the first installer run;
3. test a second installer run for idempotency;
4. remove macOS metadata files (`.DS_Store` and AppleDouble `._*`) from the test fixture;
5. run the package validator;
6. run `git diff --check` in the test fixture;
7. report the ZIP SHA-256.

Validation commands must run fail-fast. A failed installer or validator must stop the
handoff sequence before temporary package tools are removed or Git commands are shown.
The package validator may explicitly allow its own transient README/manifest while it runs; the final standalone hygiene check must run after those artifacts are removed and may not allow them.

---

## 5. Exact local handoff sequence

The user receives exact commands in this order:

1. enter the repository root;
2. define the ZIP path;
3. enable fail-fast shell behavior (`set -euo pipefail`);
4. remove only old extracted package-tool/payload folders;
5. unzip the package;
6. run the installer;
7. remove `.DS_Store` and AppleDouble `._*` metadata;
8. run the validator;
9. remove temporary package folders and package README/manifest;
10. run `git diff --check`;
11. run `git status --short`;
12. remove the downloaded ZIP only after every preceding step succeeds.

Do not ask the user to commit before Unity verification.

---

## 6. Unity verification gate

After installation:

1. open the project in the documented Unity version;
2. wait for compilation;
3. confirm there are no compiler errors;
4. run the single required QA command:

```text
Boredom And Dungeons -> TEST EVERYTHING
```

5. perform the focused Play Mode test list supplied with the package;
6. test repeated use, reset/restart behavior, and nearby-system interactions;
7. send back the real QA output and observed behavior.

No second mandatory QA menu action may be created. New automated checks are added
to `TEST EVERYTHING`.

---

## 7. Near-spawn test harness rule

Any new gameplay element that cannot be verified immediately in normal play must
receive a deliberate test harness.

Examples include:

- bosses and mini-bosses;
- boss phases and health-channel behavior;
- hazards and obstacles;
- enemy archetypes;
- rewards and pickups;
- doors, barriers, rooms, and transitions;
- combat attacks and status effects;
- mount/horse interactions;
- cutscenes and restart flows.

### Required accessibility

The test content must be:

1. in a clearly identified test room, test lane, test arena, or direct test portal;
2. close enough to the player spawn that the user can reach it immediately;
3. isolated from unrelated production gameplay;
4. safe to enter repeatedly;
5. deterministic enough to reproduce failures;
6. included in the focused Play Mode instructions.

The user must never need to search a large generated map to locate newly added
test content.

### Mini-boss rule

A mini-boss test encounter must remain escapable. It must not hard-lock the room
or trap the player behind a boss barrier.

Full/final bosses may lock an arena only when their approved encounter contract
requires it.

### Spawn safety

Temporary lava, holes, chasms, damage volumes, enemies, barriers, or test props
must not overlap or threaten the player/horse spawn. They must be placed in the
dedicated test area or disabled until the user deliberately enters it.

---

## 8. Test-artifact cleanup rule

Temporary test content must never accumulate indefinitely.

After the user accepts the feature:

1. classify every test object as `remove`, `keep as development harness`, or
   `convert to production`;
2. remove obsolete labels, placeholder meshes, duplicate HUDs, temporary portals,
   debug-only barriers, unsafe hazard samples, and abandoned setup objects;
3. when a reusable development harness is retained, keep it clearly named,
   isolated, disabled from normal production progression, and documented;
4. update scene builders/installers so regeneration does not recreate obsolete
   test objects;
5. update `TEST EVERYTHING` to detect prohibited stale test artifacts where useful;
6. update `ProjectGuide/Status/CURRENT.md` with the cleanup result;
7. do not postpone all cleanup until the end of the project.

A feature is not DONE while unapproved temporary test clutter remains in the
authoritative scene.

---

## 9. Documentation synchronization

<!-- B&D DOCUMENT DISCOVERY V8 START -->
### Maintained-document discovery

The mandatory read order and maintained-document ownership map are defined by:

- `AGENTS.md` for Codex and AI assistants;
- `ProjectGuide/README.md`;
- `ProjectGuide/INDEX.md`;
- `ProjectGuide/Engineering/ARCHITECTURE.md`;
- `ProjectGuide/QA/QA_CHECKLIST.md`;
- `ProjectGuide/Engineering/TECHNICAL_DECISIONS.md`;
- `ProjectGuide/Engineering/PERFORMANCE.md`.

These files describe stable process and architecture. They must not become competing progress/status sources.
<!-- B&D DOCUMENT DISCOVERY V8 END -->

Every material package updates all affected truth sources.

`ProjectGuide/Status/BUGS.md` is the mandatory current defect ledger. Update it immediately whenever a bug is discovered, changes status, receives an implementation, passes/fails verification, is reopened, or is reclassified. Do not leave bug status only in chat or wait until a later package.

At minimum:

- `ProjectGuide/Status/CURRENT.md`;
- relevant design/specification files;
- relevant QA contracts;
- `README.md` only when stable repository usage or discovery information changes;
- this workflow document when the process changes.

`ProjectGuide/Status/CURRENT.md` must always contain:

1. current date;
2. previous/current/next pointer;
3. affected checklist items;
4. implementation status;
5. actual QA truth;
6. blockers and risks;
7. exact resume point;
8. changelog.

Do not create versioned live status copies such as:

```text
PROJECT_STATUS_CURRENT_V2.md
WORKING_NOW.md
LATEST_STATUS.md
```

Git history stores old states.

---

## 9A. PERMANENT REPOSITORY HYGIENE CONTRACT — current-document rule

This is a standing rule and the user does not need to repeat it.

1. Every material request, correction, implementation, QA result, blocker, ordering change, resume-point change, and synchronization state is recorded in Git-maintained documentation.
2. `ProjectGuide/Status/CURRENT.md` remains the only live source for current product status and ordering.
3. Maintained documents describe current truth; package narratives and temporary repair notes are not permanent documentation.
4. When a document is superseded, first merge every still-valid contract into its authoritative owner, then update `ProjectGuide/INDEX.md`, then remove the obsolete document in the same change.
5. Git history stores old versions. Do not preserve stale files merely as an archive.
6. Before every handoff and commit, remove or ignore package ZIPs, `.package_tools`, `.package_payload`, local QA exports, caches, chat exports, copied status files, stale patch scripts, and obsolete root documents.
7. Root Markdown is limited to `README.md` and `AGENTS.md`; maintained project knowledge belongs under `ProjectGuide/`.
8. `.codex/` is maintained repository configuration and must remain available to Codex; local rich-text copies such as `AGENTS.rtf` are ignored rather than committed.
9. Repository-hygiene QA must fail when a prohibited duplicate status/roadmap or known obsolete document returns.
10. Run `python3 tools/check_repository_hygiene.py` on every handoff and before every commit.

## 9B. Remote/local divergence contract

When the remote branch and the local working tree both contain unique valid progress:

1. inspect the actual remote head, local `HEAD`, merge base, dirty files, and untracked files;
2. classify which files are remote-only, local-only, or changed on both sides;
3. build the reviewed merged content before changing Git history;
4. preserve a safety branch/tag and commit the verified merged local state;
5. fetch the remote and merge its history; do not use reset, clean, broad checkout, or a blind conflict preference;
6. rerun repository hygiene, diff checks, Unity QA, and any focused checks affected by conflict resolution;
7. push only after the merged tree still contains both sides' valid progress.

---

## 10. Failure and repair handling

When installation or validation fails:

1. do not commit;
2. do not discard unrelated local changes;
3. capture the exact terminal output;
4. determine which files were already modified;
5. build a repair package for that actual partial state;
6. make the repair idempotent;
7. validate structure and formatting;
8. rerun Unity QA after the repair.

Never claim a gameplay result passed when only static validation was performed.

---

## 11. Git handoff after PASS

Only after clean compilation, `TEST EVERYTHING`, and focused Play Mode acceptance,
provide commands equivalent to:

```bash
# Preserve the pre-sync history pointer before recording the verified local tree.
git branch safety/pre-remote-sync

git add -A
git diff --cached --check
git diff --cached --stat
git status --short

git commit -m "<accurate change summary>"

# Preserve the complete verified local implementation before merging remote history.
git branch safety/verified-local-before-remote-merge

git fetch origin
git merge --no-ff origin/main

python3 tools/check_repository_hygiene.py
git diff --check
git status --short

git push origin main
```

Before commit, verify package ZIPs, extracted `.package_tools`,
`.package_payload`, temporary manifests, local QA reports, caches, and unrelated
debug artifacts are not staged. When remote and local both contain valid unique
progress, resolve the merged content deliberately; never use a blind `ours`/`theirs`
choice, destructive reset, or `pull --rebase` over an unrecorded dirty tree.

The commit includes all real changed source, assets, `.meta` files, scenes,
documentation, and QA contracts.

---

## 12. Definition of complete work

A change is complete only when:

- the requirement is recorded;
- code/assets are implemented;
- Unity compiles;
- automated QA passes;
- focused Play Mode behavior passes;
- edge cases pass;
- temporary test artifacts are removed or deliberately retained;
- authoritative documentation is synchronized;
- Git diff checks pass;
- the user commits and pushes the verified state.

<!-- B&D DURABLE TASK RECORD WORKFLOW START -->
## Durable task records and cross-session handoff

Every material task updates `ProjectGuide/Status/CURRENT.md`. In addition, create and continuously maintain a task record under `ProjectGuide/Tasks/` when the work is large, multi-step, cross-system, likely to span sessions, or likely to move to Codex/another contributor.

The task record must include:

- task ID and title;
- originating request;
- why the task exists and the risk/problem it solves;
- approved outcome;
- scope, non-goals and protected behavior;
- dependencies and ordered sub-tasks;
- relevant files, systems, scenes, assets, data and documents;
- decisions and rationale;
- implementation performed;
- verification evidence and exact results;
- unverified areas;
- bug IDs, blockers, contradictions and remaining risks;
- deferred work;
- exact resume point and exact next action;
- commit, package and QA references when available.

Update the task record, `ProjectGuide/Status/CURRENT.md` and every relevant canonical document in the same change whenever requirements, implementation, status, QA, bugs, blockers, ordering, deferral or the resume point changes. Do not wait for final delivery.

Before handing work to Codex, another chat or another developer, verify that the repository alone explains the complete context and continuation path.

Before every commit, classify maintained documents as keep-current, merge-then-remove, obsolete-remove or commit-blocking. Merge valid information first, then remove completed, superseded, duplicate, temporary or irrelevant documents and update `ProjectGuide/INDEX.md` plus all references.

Canonical policy: `ProjectGuide/Rules/TASK_CONTINUITY.md`.
<!-- B&D DURABLE TASK RECORD WORKFLOW END -->

## Historical QA compatibility after document moves

When maintained documents are reorganized, do not assume path replacement alone is sufficient. Before delivery:

1. inspect all `TEST EVERYTHING` scanners for stable discovery phrases and required task headings;
2. move valid contracts into the new canonical owner or a clearly labeled compatibility map;
3. never recreate a superseded duplicate merely to satisfy a token;
4. never weaken a scanner when the underlying contract is still valid;
5. run a static token-equivalence validator in the package fixture;
6. record the real Unity result and keep implementation blocked until the Unity rerun passes.

<!-- B&D LOCAL PATCH DELIVERY V1 START -->
## Permanent local patch delivery and production-code gate

This section supersedes any wording that implies the assistant may write to Git or GitHub.

1. The assistant must not commit, push, create or move branches, open or merge pull requests, reset, clean, stash, checkout or pull.
2. The normal handoff is one downloadable, backup-aware local patch ZIP. It contains an idempotent installer, validator, rollback tool, manifest and focused verification list.
3. Before any file is changed, the installer creates a timestamped backup of every path it can modify or remove. A failed preflight changes nothing; a failed later step keeps the backup and prints the rollback command.
4. The installer must preserve unrelated local work. Unexpected owner structure blocks application instead of overwriting it.
5. Package extraction/application is one command. Temporary package folders and the downloaded ZIP are removed only after all static checks pass; backups remain until the user deletes them.
6. Unity verification and the local commit belong to the user. No push command is automatic.
7. Every touched or materially encountered production area follows `ProjectGuide/Rules/PRODUCTION_CODE_STANDARD.md`. Durable behavior is implemented in the authoritative owner; avoidable workaround/compatibility layers are prohibited.
<!-- B&D LOCAL PATCH DELIVERY V1 END -->

<!-- B&D SAFE REPOSITORY MAINTENANCE V2 START -->
## Safe repository-size maintenance

Repository reduction is evidence-driven and lossless. Generated local data may be removed only when untracked and reproducible. No documentation, source asset, Unity `.meta`, GUID, history, commit hash, LFS history, visual quality or game behavior may change. When uncertain, retain the file and report it as an untouched candidate. The assistant still performs no push, commit, branch or PR operation.
<!-- B&D SAFE REPOSITORY MAINTENANCE V2 END -->

<!-- BND_UNITY_UI_PACKAGE_RECOVERY_V3:BEGIN -->
## Unity package/cache repair safety

- Unity must be fully closed before deleting or rebuilding `Library`, `Temp`, `obj`, package caches, or package overlays.
- Repository-maintenance tools must never recurse into or delete `Packages/**` as if it were a generic cache.
- Generated, untracked `Packages/com.unity.*` overlays may be moved only after the manifest is validated, tracked/local packages are excluded, and an external rollback backup is created.
- Package recovery must preserve `Packages/manifest.json` and a valid `packages-lock.json`; source files are not rewritten to avoid a missing package assembly.
<!-- BND_UNITY_UI_PACKAGE_RECOVERY_V3:END -->
