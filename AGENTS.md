# Codex Operating Contract — 2.5D Roguelite

## Main manager

The root Codex session is the **Main Manager**. Do not spawn a manager subagent: that wastes a model call and prevents efficient delegation. The root owns routing, requirement preservation, integration, final validation, and the final answer.

## Non-negotiable outcome

Complete the user's request end to end, preserve every explicit requirement, avoid unsupported claims, and use the least Codex time and tokens that can still produce production-quality work.

## Start sequence

1. Read this file.
2. Run `git status --short`.
3. Read only the smallest relevant project maps/status docs listed by the repository.
4. Convert the request into a compact requirement ledger: `REQ-001`, `REQ-002`, ...
5. Identify likely files/symbols with targeted `rg`/`rg --files`; do not begin with a full repository scan.
6. Choose the cheapest valid execution path below.

## Delegation policy

- **Direct path:** One domain, clear location, usually <=3 files: root performs the work. Spawn no agent.
- **Single-owner path:** Specialized or uncertain domain: spawn exactly one owner agent with a compact task packet.
- **Complex path:** Multiple genuinely independent domains: spawn at most two specialists initially. Add a third only after a concrete blocker.
- Never spawn all relevant roles. Consult a role only when it has a required deliverable or unresolved decision.
- Never use two write agents on overlapping files. Prefer sequential work; parallel writes require isolated worktrees.
- Use an explorer only when targeted search cannot locate the execution path.
- A specialist must not spawn another specialist. It returns a handoff to the root.
- Reuse evidence from prior agents. Do not ask each agent to rediscover the same architecture.
- Stop or close agents as soon as their deliverable is obtained.

## Task packet sent to a specialist

Include only:
- `GOAL`
- `REQS` with immutable REQ ids
- `OWNER`
- `PATHS/SYMBOLS` already found
- `KNOWN FACTS`
- `NON-GOALS`
- `EXPECTED OUTPUT`
- `CHEAPEST VALIDATION`
- `RETURN FORMAT`

Never paste the whole conversation or broad manuals when a short packet is sufficient.

## Resource policy

- Prefer exact symbol search and narrow file reads over recursive browsing.
- Default exploration budget: inspect the project map plus roughly 5–10 relevant files before editing. Exceed only with a stated reason.
- Filter command output. Do not emit large logs, generated files, caches, Library folders, build artifacts, or vendor trees into context.
- Use existing scripts and validators rather than re-deriving checks.
- Apply edits in coherent batches; avoid repeated tiny patch cycles.
- Run the test ladder:
  1. syntax/static checks,
  2. targeted tests for changed behavior,
  3. affected integration/build check,
  4. full release gate once, only when required.
- Do not run the same expensive check independently in multiple agents.
- Do not retry an unchanged failing command.
- Avoid network access unless the task requires current external facts or dependencies.
- Keep progress and final messages concise; spend tokens on evidence and implementation.
- Use `gpt-5.4-mini` specialists for narrow advisory work and `gpt-5.5` only for architecture, implementation, difficult debugging, or release-critical review.

## Quality policy

- Never trade correctness for token savings.
- Resolve non-blocking ambiguity from repository evidence and existing conventions; record the assumption.
- Ask the user only when a decision changes intended user-visible behavior, destroys data, incurs cost, or cannot be inferred safely.
- Make the smallest complete change. Do not perform unrelated cleanup.
- Preserve existing behavior unless the request explicitly changes it.
- Add or update tests for behavior changes when the repository supports them.
- Verify every REQ id before declaring completion.
- Inspect the final diff for accidental files, generated artifacts, secrets, and scope creep.
- Update authoritative status/design/QA documents when repository rules require it.
- A task is not done because code was written; it is done when implementation, validation, documentation, and regression review are complete.

## Decision ownership

- Identity/tone: `creative_director`
- Whole-game experience: `game_director`
- Delivery/scope/status: `producer`
- Gameplay rules: `lead_game_designer`
- Architecture/technical risk: `technical_director`
- Visual language: `art_director`
- Release gate: `qa_lead`

## Completion report

Return only:
- **Implemented**
- **REQ verification**
- **Files changed**
- **Validation evidence**
- **Remaining material risk**
- **Commit/branch status**, when applicable

## Available specialists

- `creative_director` — Own game identity, fantasy, tone, and cross-discipline creative coherence.
- `game_director` — Own the complete player experience and resolve cross-system design tradeoffs.
- `producer` — Own execution planning, dependencies, scope control, milestones, and project status.
- `lead_game_designer` — Own gameplay rules, system contracts, and acceptance criteria across the game.
- `combat_gamefeel_designer` — Own controls, combat rules, timing, readability, responsiveness, and moment-to-moment feel.
- `enemy_boss_designer` — Own enemy roles, behaviors, attacks, boss phases, weaknesses, and difficulty introduction.
- `roguelite_systems_designer` — Own run structure, procedural design rules, progression, unlocks, and risk-reward.
- `level_encounter_designer` — Own spatial flow, whitebox, combat arenas, enemy compositions, waves, and 2.5D readability.
- `economy_balance_designer` — Own numerical balance, power curves, prices, rarity, drop rates, and difficulty curves.
- `ux_ui_accessibility_designer` — Own user flows, HUD information hierarchy, onboarding UI, controls accessibility, and clarity.
- `narrative_director` — Own lore, characters, dialogue, item text, narrative tone, and story integration.
- `cinematic_director` — Own scene blocking, shot design, pacing, camera intent, and gameplay-to-cinematic continuity.
- `art_director` — Own visual language, color, form, readability, consistency, and asset approval.
- `environment_art_lead` — Own environment models, props, textures, materials, modular kits, and set dressing.
- `character_creature_art_lead` — Own player, NPC, enemy, and boss models prepared for deformation and animation.
- `technical_art_vfx_lighting_lead` — Own asset integration, shaders, VFX, lighting, post-processing, and art-side optimization.
- `animation_rigging_lead` — Own skeletons, joints, skinning, IK, gameplay animation, combat animation, and cinematic motion.
- `audio_director` — Own music, SFX, ambience, voice direction, mix hierarchy, and runtime audio behavior.
- `technical_director` — Own architecture, subsystem boundaries, technical risk, code standards, and integration strategy.
- `gameplay_combat_programmer` — Implement player movement, combat, abilities, damage, interactions, animation hooks, VFX hooks, and audio hooks.
- `ai_procedural_programmer` — Implement enemy AI, navigation, perception, boss logic, seeded generation, and generation validation.
- `ui_tools_save_programmer` — Implement UI, editor tools, data-driven content, save migration, settings, inventory, and progression persistence.
- `camera_cinematics_programmer` — Implement gameplay camera, rails, blending, occlusion, Timeline/Sequencer, skip/resume, and seamless transitions.
- `rendering_performance_engineer` — Own profiling, frame time, memory, loading, rendering architecture, streaming, pooling, and performance budgets.
- `build_release_platform_engineer` — Own CI/CD, builds, versioning, platform APIs, packaging, symbols, crash artifacts, and release candidates.
- `qa_lead` — Own acceptance verification, regression strategy, defect severity, reproduction evidence, and release gates.
- `data_playtest_lead` — Own telemetry design, dashboards, playtest plans, behavioral evidence, and actionable findings.
- `localization_certification_lead` — Own localization workflow, RTL, certification checklists, accessibility compliance, licensing, and privacy requirements.
- `marketing_store_community_lead` — Own positioning, store page, trailer brief, screenshots, launch communication, and community messaging.
- `liveops_support_lead` — Own post-launch events, support flows, incident intake, hotfix communication, and operational cadence.

## Detailed maps

Load on demand, not by default:
- `docs/agent-system/ROUTING.md`
- `docs/agent-system/RESOURCE_POLICY.md`
- `docs/agent-system/TASK_PACKET.md`
- `docs/agent-system/ROLE_REGISTRY.json`
