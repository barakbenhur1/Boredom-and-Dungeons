# Boredom & Dungeons — Mandatory Development Workflow

This document defines **how work must be performed** on the project.

It is a permanent process contract. It does not duplicate the current feature
status or the complete requirement list.

- `PROJECT_STATUS.md` is the only authoritative source for product requirements,
  current progress, QA truth, blockers, ordering, and the exact resume point.
- `DEVELOPMENT_WORKFLOW.md` is the authoritative source for the working method.
- `README.md` points contributors to both documents.

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
5. Every requirement change must be recorded in `PROJECT_STATUS.md` before or in the
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

1. add it to the correct place in `PROJECT_STATUS.md`;
2. save the current resume point;
3. implement the earlier item immediately;
4. validate it;
5. update QA truth and documentation;
6. return to the saved resume point.

### Inside the current item/category

Update the current requirement and implement it in dependency order.

### Later than the current work

Add it to the correct future location in `PROJECT_STATUS.md`, then continue the
current item.

### Cross-cutting regression

A regression that affects startup, death/restart, core movement, combat, scene
safety, data integrity, or QA reliability blocks later feature work and is handled
as an earlier task.

### Category transition

Before entering a new category:

1. finish the previous category acceptance gate;
2. synchronize code, QA, and `PROJECT_STATUS.md`;
3. state which category was completed;
4. state the proposed next category;
5. ask the user for additions or changes;
6. record the answer;
7. continue only after approval.

---

## 3. Repository inspection before editing

Before creating a package:

1. inspect the latest repository state;
2. read `README.md`, `PROJECT_STATUS.md`, relevant design files, relevant runtime
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
4. run the package validator;
5. run `git diff --check` in the test fixture;
6. report the ZIP SHA-256.

---

## 5. Exact local handoff sequence

The user receives exact commands in this order:

1. enter the repository root;
2. define the ZIP path;
3. remove only old extracted package-tool/payload folders;
4. unzip the package;
5. remove the downloaded ZIP after successful extraction;
6. run the installer;
7. run the validator;
8. remove temporary package folders and package README/manifest;
9. run `git diff --check`;
10. run `git status --short`.

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
6. update `PROJECT_STATUS.md` with the cleanup result;
7. do not postpone all cleanup until the end of the project.

A feature is not DONE while unapproved temporary test clutter remains in the
authoritative scene.

---

## 9. Documentation synchronization

Every material package updates all affected truth sources.

At minimum:

- `PROJECT_STATUS.md`;
- relevant design/specification files;
- relevant QA contracts;
- `README.md` only when stable repository usage or discovery information changes;
- this workflow document when the process changes.

`PROJECT_STATUS.md` must always contain:

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
git add -A
git diff --cached --check
git diff --cached --stat
git status --short

git commit -m "<accurate change summary>"

git pull --rebase origin main
git push origin main
```

Before commit, verify package ZIPs, extracted `.package_tools`,
`.package_payload`, temporary manifests, local QA reports, caches, and unrelated
debug artifacts are not staged.

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
