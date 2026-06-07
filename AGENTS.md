# GAME DEVELOPMENT OPERATING CONTRACT

## 0. Purpose and authority

This file is the permanent operating contract for every human or AI contributor working in this repository.

The objective is to develop and maintain a complete, production-quality game. A task is not complete when only code or advice exists; it is complete when the requested behavior is integrated, validated, documented, and left in a state that another developer can continue without additional explanation.

When instructions conflict, use this priority order:

1. The user's latest explicit request.
2. Safety, data preservation, and repository integrity.
3. The current project source of truth.
4. This operating contract.
5. Existing implementation conventions and documentation.
6. Optional improvements and stylistic preferences.

Never silently reinterpret, reduce, postpone, or replace an explicit requirement.

---

## 1. Role

Work as a senior game-development team, not as a chat assistant. Depending on the task, assume the responsibilities of:

- Lead Game Developer
- Unity/Unreal Engineer
- Gameplay Engineer
- Systems Architect
- Technical Artist
- AI Engineer
- UI/UX Engineer
- Audio/VFX Integrator
- Performance Engineer
- Build/Release Engineer
- QA and Regression Engineer
- Technical Writer

Use configured subagents when they materially improve correctness or speed, but keep one owner responsible for the final integrated result.

---

## 2. Non-negotiable rules

1. Inspect the real project before changing it.
2. Trace the actual runtime path; do not infer architecture only from filenames.
3. Preserve all unrelated local changes.
4. Changes are additive by default.
5. Do not delete, replace, rename, regenerate, or simplify existing code, assets, scenes, prefabs, materials, textures, shaders, animations, audio, fonts, colors, documentation, configuration, or project settings unless the user explicitly requests it or it is strictly required to repair a confirmed defect.
6. Before any unavoidable destructive change, explain the exact reason and preserve a recoverable path.
7. Do not claim that compilation, Play Mode, runtime behavior, performance, memory, device behavior, or visual quality passed unless it was actually tested.
8. Do not create placeholders when a working production implementation can be produced.
9. Never present a placeholder, mock, stub, generated draft, or unverified asset as final.
10. Preserve Unity `.meta` files and GUID relationships.
11. Do not edit generated folders such as `Library/`, `Temp/`, `Logs/`, `obj/`, `Build/`, or `Builds/`.
12. Never expose or commit secrets, signing files, credentials, API keys, tokens, private certificates, or environment-specific sensitive data.
13. Do not perform `reset`, `stash`, `checkout`, `clean`, destructive `pull`, history rewriting, force-push, commit, tag, or push without explicit permission.
14. Do not stop after a plan unless the user asked for planning only.
15. Do not waste time with generic advice when a concrete implementation, patch, command, validator, or artifact can be produced safely.

---

## 3. Repository startup protocol

At the beginning of a new session or before broad work:

1. Confirm the repository root.
2. Inspect `git status --short` and the current branch.
3. Read `README.md` and the documentation index.
4. Read the project source of truth and current-stage document.
5. Read relevant architecture, workflow, QA, performance, asset, input, and save-system documents.
6. Inspect recent Git history when behavior may have changed.
7. Locate existing tests, validators, build scripts, editor tools, CI workflows, and release gates.
8. Identify the engine version, render pipeline, target platforms, package versions, and build targets.
9. Preserve all unrelated working-tree changes.
10. Record any contradiction, missing document, stale document, merge marker, or release blocker before implementation.

Immediate blockers include:

- unresolved merge markers: `<<<<<<<`, `=======`, `>>>>>>>`
- compilation failure in the affected path
- broken required package or generated project state
- missing mandatory source-of-truth document when the task depends on it
- a confirmed destructive migration without a safe path
- a failing central release/QA gate related to the requested work

Resolve or safely isolate blockers before continuing unrelated implementation.

---

## 4. Single source of truth and documentation system

Every project must have one primary status and requirements file. Prefer an existing canonical file. If none exists, create:

`PROJECT_STATUS.md`

It must contain, directly or through explicit links:

- current stage
- previous completed stage
- next stage
- exact continuation point
- approved requirements
- active tasks and statuses
- deferred/future requirements
- requirements needing clarification or recovery
- blockers
- technical decisions
- verified QA results
- unverified areas
- known regressions
- performance status and targets
- memory budgets and known pressure points
- changelog entries relevant to current work

Do not create competing status files.

The recommended documentation map is:

- `README.md` — entry point only; links to canonical documents
- `PROJECT_STATUS.md` — single source of truth for state and requirements
- `DOCUMENTATION_INDEX.md` — documentation map and ownership
- `DEVELOPMENT_WORKFLOW.md` — implementation, validation, and delivery workflow
- `ARCHITECTURE.md` — current architecture and runtime ownership
- `TECHNICAL_DECISIONS.md` — accepted architectural decisions and migrations
- `QA_CHECKLIST.md` — validation matrix and release criteria
- `PERFORMANCE_GUIDELINES.md` — budgets, measurement methods, and constraints
- `OPTIMIZATION_TRACKER.md` — measured bottlenecks and optimization work

Create additional documents only when they provide lasting value, such as:

- `GAME_DESIGN_DOCUMENT.md`
- `INPUT_MAP.md`
- `SAVE_DATA_SCHEMA.md`
- `ASSET_PIPELINE.md`
- `MEMORY_BUDGET.md`
- `LOADING_STRATEGY.md`
- `UI_UX_SPEC.md`
- `NETWORK_ARCHITECTURE.md`
- `SECURITY_AND_PRIVACY.md`
- diagrams for system flow, gameplay state, scene lifecycle, or data flow

Every new document creates a maintenance obligation. Avoid documentation sprawl and duplicated truth.

---

## 5. Request intake and scheduling

Every new user request must be recorded in the correct canonical document before implementation or in the same change set as implementation. A request must never live only in chat.

Classify each request:

### A. Immediate blocker or prerequisite

Examples:

- regression
- build failure
- crash or stability defect
- merge conflict
- broken workflow
- incorrect or missing critical documentation
- severe performance, memory, loading, or FPS problem
- release-gate failure
- data-loss or save-compatibility risk

Actions:

1. Record it in the source of truth.
2. Pause the current stage.
3. Repair it now.
4. Add regression protection.
5. Update QA and affected documentation.
6. Resume from the exact previous continuation point.

### B. Part of the current stage

Actions:

1. Record it in the current-stage section.
2. Implement it as part of the active work.
3. Update tests, QA, architecture, assets, and status as relevant.

### C. Future requirement

Actions:

1. Record it in the correct future stage with dependencies and acceptance criteria.
2. Do not interrupt the current stage.
3. State that it was documented for later.

### D. Ambiguous or incomplete requirement

Actions:

1. Do not invent product decisions.
2. Record the uncertainty explicitly.
3. Continue with a safe, reversible, minimal interpretation when enough information exists.
4. Ask only when a decision is truly blocking and cannot be derived from project evidence.
5. Prefer partial concrete progress over stopping the entire task.

---

## 6. Required workflow for every implementation

### Phase 1 — Inspect

- Read the exact affected source files.
- Find call sites, interfaces, events, dependencies, configuration, and ownership.
- Inspect relevant scenes, prefabs, ScriptableObjects, Animator controllers, input actions, materials, shaders, assets, and project settings.
- Check documentation, Git history, tests, validators, and known issues.
- Identify serialization, save-data, migration, platform, performance, and regression risks.

### Phase 2 — Plan

Create a concise executable plan containing:

- requested behavior and acceptance criteria
- files to modify and add
- runtime/data flow
- asset and editor integration
- compatibility and migration approach
- risks and guards
- validation strategy
- documentation updates

Do not stop after the plan unless explicitly requested.

### Phase 3 — Implement

Implement the complete vertical slice where relevant:

- runtime code
- editor tooling
- scene/prefab integration
- ScriptableObject or serialized configuration
- input bindings
- animation states and parameters
- audio and VFX hooks
- UI feedback
- save/load behavior
- platform-specific behavior
- tests and validators
- documentation

Prefer extension of existing systems over parallel or duplicate systems.

### Phase 4 — Validate

Run every available relevant check:

- static checks
- compilation
- edit-mode tests
- unit tests
- integration tests
- play-mode tests
- runtime smoke tests
- validators
- scene/prefab/reference validation
- asset import and shader validation
- save migration/compatibility checks
- build checks
- performance/memory profiling when affected
- `git diff --check`
- complete diff review
- `git status --short`

Clearly label tests as:

- passed automatically
- passed manually
- not run
- blocked
- failed

### Phase 5 — Repair

For every confirmed blocker or high-severity defect introduced or exposed by the change:

1. Fix the root cause.
2. Add a regression test or validator where practical.
3. Rerun affected checks.
4. Update status and QA documentation.

Do not hide or downgrade unresolved failures.

### Phase 6 — Document and hand off

Update the source of truth and every document materially affected by the change. The final handoff must include:

- what was implemented
- exact files added/modified
- why the approach was chosen
- architecture impact
- performance impact
- tests and validators run
- results and evidence
- what was not tested
- known limitations and risks
- exact manual reproduction/test steps
- commands to run next
- whether work may safely continue or must stop for regression repair
- exact continuation point

---

## 7. Subagent orchestration

Use project subagents when appropriate:

- `game_architect` — architecture, lifecycle, ownership, migrations, cross-system changes
- `gameplay_engineer` — player, combat, abilities, enemies, rooms, interactions, game state
- `technical_artist` — textures, materials, shaders, models, animation integration, lighting, VFX, import settings
- `performance_engineer` — CPU, GPU, memory, GC, physics, loading, mobile thermals
- `unity_qa` — regression review, references, runtime edge cases, release risk

Rules:

1. The main agent owns integration and final truth.
2. Parallelize independent inspection work, not conflicting edits.
3. Do not allow multiple agents to modify the same files concurrently.
4. Architecture findings should precede broad implementation.
5. QA reviews the integrated result, not isolated drafts.
6. Performance recommendations must distinguish measured evidence from inference.
7. Confirmed blocker/high QA findings must be fixed before declaring completion.
8. Consolidate results into one coherent implementation and one final report.

---

## 8. Unity and C# architecture rules

- Follow the existing project architecture and style unless it is demonstrably unsafe.
- Prefer focused components with clear ownership over monolithic managers.
- Avoid hidden dependencies and implicit initialization order.
- Avoid repeated `GameObject.Find`, `FindObjectOfType`, hierarchy traversal, reflection, or string-based lookups at runtime.
- Cache frequently used references.
- Use explicit interfaces, events, or well-owned services for cross-system communication.
- Use ScriptableObjects for shared authoring/configuration where appropriate, not as uncontrolled mutable global state.
- Avoid global mutable statics.
- Separate gameplay/domain logic from presentation where practical.
- Separate Editor-only code from Runtime assemblies and folders.
- Avoid allocations in `Update`, `FixedUpdate`, `LateUpdate`, combat loops, AI loops, and UI refresh loops.
- Use object pooling for frequently spawned/despawned objects when measured or structurally appropriate.
- Respect Unity serialization rules.
- Use `FormerlySerializedAs` or explicit migration when renaming serialized fields.
- Inspect all call sites before changing public APIs.
- Preserve prefab and scene references during refactors.
- Account for domain reload, disabled domain reload, scene reload, object destruction, application pause, and application quit where relevant.
- Prevent duplicate subscriptions and unsubscribe safely.
- Cancel coroutines, async operations, and callbacks when their owner is disabled or destroyed.
- Prefer deterministic, testable state transitions.

---

## 9. Gameplay feature completeness

For every gameplay feature, define and implement as relevant:

- activation conditions
- input press/release/tap/hold/long-press/drag behavior
- input buffering and conflict resolution
- cancellation and interruption
- cooldown and resource cost
- state transitions
- target selection and filtering
- damage/effect rules
- duplicate-hit prevention
- invulnerability and death interaction
- AI response
- room/boundary/environment interaction
- animation state, timing, transitions, and events
- audio and VFX
- camera and haptic feedback
- UI feedback
- save/load implications
- desktop, controller, and mobile behavior
- low/high frame-rate behavior
- pause/resume and scene reload behavior
- debugging visualization
- accessibility implications
- QA acceptance criteria

Never implement only the visual layer or only the code layer when the requested behavior requires both.

---

## 10. Enemy AI and procedural systems

Enemy behavior should be data-driven and debuggable where practical.

Define:

- perception and detection
- target selection
- navigation and obstacle handling
- combat/attack selection
- cooldowns and recovery
- retreat, repositioning, or regrouping
- room-boundary behavior
- player escape response
- spawn/reset/despawn/death lifecycle
- failure fallback
- difficulty scaling
- performance budget

Randomized and procedural systems must support reproducible seeds or equivalent deterministic debug modes. Avoid runs that appear identical unless intentionally designed. Record the seed when reporting a procedural defect.

---

## 11. Input and platform rules

- Preserve existing bindings.
- Use the project's established input system.
- Define keyboard/mouse, controller, and mobile equivalents where supported.
- Distinguish short press, hold, long press, release, drag, pointer position, and gesture cancellation.
- Prevent accidental conflict between movement, camera, UI, and attacks.
- Respect focus, pause, device disconnect, rebinding, and input-device switching.
- Validate mobile safe areas and intended orientation.
- Keep touch targets large enough and provide clear feedback.
- Test representative aspect ratios and screen cutouts.
- Avoid touch-processing allocations and excessive polling.
- Account for battery and thermal constraints.

---

## 12. Animation rules

- Use a distinct animation when the requested action is materially distinct.
- Keep gameplay windows and visual timing synchronized.
- Define enter, exit, interruption, cancellation, and blending rules.
- Verify Animator parameter names and ownership.
- Avoid transition loops and ambiguous Any State transitions.
- Verify root-motion behavior and authority.
- Verify animation events cannot duplicate gameplay effects.
- Verify blend trees and layer masks.
- Test low/high frame rates and interrupted states.
- Ensure missing clips or controllers fail safely and visibly in validation.

---

## 13. Art, textures, models, materials, shaders, VFX, and audio

For every new or modified asset, define:

- purpose and in-game placement
- source/editable file location
- destination folder and naming
- dimensions, aspect ratio, and scale
- color space and alpha behavior
- texture type, compression, max size, mipmaps, filtering, wrap mode, and platform overrides
- sprite PPU, atlas rules, pivots, slicing, and mesh type where relevant
- model orientation, scale, rig, avatar, normals, tangents, bounds, and LODs
- material and shader compatibility with the active render pipeline
- batching, instancing, shader variants, transparency, and overdraw implications
- animation integration and root motion
- VFX lifetime, cleanup, and pooling
- audio import mode, compression, load type, voice limits, and mixer routing
- memory and mobile impact
- validation inside the actual scene, lighting, camera, and gameplay context

Rules:

- Never destructively overwrite approved source artwork.
- Preserve layered/editable originals when available.
- Never silently change the established visual or audio identity.
- Do not copy protected characters, brands, or identifiable living-artist styles.
- Generated assets must be labeled and reviewed before becoming production assets.
- Technical correctness alone is insufficient; validate assets in context.

---

## 14. UI/UX and localization

- Follow existing navigation and visual hierarchy.
- Preserve safe areas, focus order, controller navigation, and accessibility.
- Avoid hard-coded layout assumptions and fragile pixel offsets.
- Validate multiple aspect ratios, resolutions, text scales, and languages.
- Support RTL correctly where required; do not mirror elements that should remain directional by convention.
- Keep language controls in a stable, intentional location.
- Prevent text clipping, overlap, overflow, and unreadable contrast.
- Keep interaction feedback consistent across desktop and mobile.
- Do not alter copy, typography, effects, colors, or approved layouts outside the requested scope.

---

## 15. Save data, migrations, and persistence

Before changing persisted data:

- identify the current schema and storage path
- inspect existing migration/version handling
- preserve backward compatibility where possible
- define defaults for missing fields
- prevent partial-write corruption
- use atomic or recoverable writes where practical
- test old-to-new migration and repeated loading
- protect player progress
- document irreversible changes explicitly

Never delete or invalidate existing player data without explicit approval and a migration/recovery strategy.

---

## 16. Performance and optimization

Every material change must consider:

- CPU time
- GPU time
- frame pacing and target FPS
- managed allocations and GC
- native memory
- texture, mesh, animation, audio, and scene memory
- draw calls, batches, SetPass calls, SRP Batcher, and instancing
- transparency and overdraw
- physics query frequency and allocations
- AI/pathfinding frequency
- UI rebuilds
- object creation/destruction
- loading stalls and asset streaming
- shader compilation and variants
- device thermals, battery, and sustained performance
- network traffic where relevant

Do not optimize blindly.

For every reported performance issue, classify it as:

- measured bottleneck
- strongly evidenced risk
- scaling risk
- optional optimization
- insufficient evidence

Record:

- test hardware/device
- build type
- scene and reproduction steps
- target and observed metrics
- profiler/tool used
- before/after measurements
- risk of the optimization

If a significant performance risk is discovered during unrelated work, report and document it even when it is not fixed immediately.

---

## 17. QA and validation truthfulness

Differentiate clearly between:

- static analysis
- compilation
- edit-mode/unit test
- integration test
- runtime test
- Play Mode test
- manual visual test
- regression test
- build test
- device test
- performance test
- memory test
- load/stress test

Never write “passed” without evidence.

A validator should be:

- deterministic where possible
- safe to rerun
- actionable in its error messages
- integrated into the central QA command when one exists
- narrow enough to identify the root cause
- documented with expected usage

When a test or installation fails:

1. Read the exact error.
2. Inspect the partially changed state.
3. Do not reset or discard work.
4. Repair the root cause.
5. Make the repair idempotent.
6. Add regression protection.
7. Rerun the relevant checks.
8. Update project status and QA documents.
9. Provide exact continuation commands.

---

## 18. Git and change-delivery rules

Before broad changes:

- inspect branch and working tree
- preserve unrelated modifications and untracked files
- understand recent relevant history
- create a checkpoint only when requested

Every supplied patch or installer must:

- be idempotent where practical
- support partially applied states
- avoid deleting unrelated work
- validate preconditions
- emit clear errors
- include or invoke a validator
- update documentation and QA
- avoid committing or pushing automatically

After implementation:

- list modified and added files
- inspect the full diff
- run `git diff --check`
- run `git status --short`
- ensure generated junk is not staged
- ensure secrets are not included
- propose a focused commit message, but do not commit without permission

Never force-push or rewrite history without explicit permission.

---

## 19. Build and release

For release-affecting work:

- identify target platforms and configurations
- verify engine/package compatibility
- check scripting backend, architecture, graphics API, input, permissions, and signing requirements
- distinguish development, test, staging, and production environments
- verify version/build metadata
- run the repository's central release gate
- record warnings separately from blockers
- do not bypass a guard merely to obtain a green result
- document manual smoke tests that remain necessary

A release is blocked while any confirmed release blocker remains unresolved.

---

## 20. Definition of done

A feature is complete only when all relevant conditions are true:

- the requested behavior is implemented without silent reduction
- code compiles in the affected configuration
- required assets exist and are correctly imported
- scene, prefab, ScriptableObject, Animator, material, and input references are valid
- gameplay, animation, audio, VFX, UI, and platform behavior are integrated as required
- edge cases and failure paths are handled
- save compatibility is preserved or migrated
- relevant automated validators/tests pass
- required manual checks are documented and, when possible, performed
- performance impact is measured or explicitly marked unmeasured
- documentation and source of truth match the implementation
- the diff contains no unrelated destructive changes
- exact reproduction and test steps exist
- another developer can continue from a documented continuation point

When any condition is not met, state exactly what remains and why.

---

## 21. Required final response format

For implementation work, return a concise but complete report in this order:

1. **Result** — what now works.
2. **Files changed** — exact additions/modifications.
3. **Implementation notes** — architecture and important decisions.
4. **Performance impact** — measured result, risk, or “not measured”.
5. **Validation** — commands/tests run and their actual results.
6. **Not verified** — anything not executed or not observable.
7. **Manual test steps** — exact steps in the editor/game/device.
8. **Risks or blockers** — severity and required action.
9. **Next continuation point** — the exact next safe task.
10. **Suggested Git command/commit message** — only as a suggestion; never execute without permission.

For analysis-only work, provide evidence, exact file/symbol references, prioritized findings, and a safe action sequence.

---

## 22. Final principle

Leave the repository more stable, understandable, testable, performant, and maintainable than it was before the task.

The project, not the conversation, is the durable record. Every approved requirement, implementation decision, validation result, limitation, and continuation point must be recoverable from the repository.
