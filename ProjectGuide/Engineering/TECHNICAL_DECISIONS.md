# Technical Decisions — Durable Project Choices

This file records long-lived choices and rationale. Current implementation status and open work remain in `ProjectGuide/Status/CURRENT.md`.

## Active decisions

### TD-001 — One authoritative product-status source

- `ProjectGuide/Status/CURRENT.md` is the only live source for requirements, priorities, current/next work, blockers, QA truth, and resume point.
- Git history stores previous states.
- No competing `WORKING_NOW`, `LATEST_STATUS`, or versioned status documents.

### TD-002 — One mandatory QA entry point

- All automated project checks integrate into `Boredom And Dungeons -> TEST EVERYTHING`.
- Focused QA classes remain modular but are orchestrated by the existing window.

### TD-003 — Additive, non-destructive changes

- Preserve existing functionality and assets by default.
- Partial-package failures are repaired on top of the real local state.
- Broad reset, clean, or checkout is not an accepted repair strategy without explicit user approval.

### TD-004 — Existing system owner before new system

- Extend the established owner when possible.
- Do not introduce parallel menu, pause, run-flow, camera, minimap, mount, health, or QA controllers for the same responsibility.

### TD-005 — Runtime/editor separation

- Runtime assemblies do not depend on `UnityEditor`.
- Scene generation, validation, and repair tools remain editor-only.

### TD-006 — Structural patching over fragile text matching

- Installers prefer method, block, or token-aware edits.
- Whitespace-sensitive replacement is acceptable only when preflight uniquely proves the expected state.
- Installers must be idempotent and support partial previous application.

### TD-007 — Documentation changes are part of implementation

- Every user request is captured in the correct `ProjectGuide/Status/CURRENT.md` location before or with implementation.
- Architecture, design, QA, technical decisions, and performance documents are updated when their truth changes.
- Chat-only decisions are not accepted project state.

### TD-008 — Conflict-marker scans are line-aware

- Only standalone Git conflict-marker lines are blockers.
- Inline examples in maintained documentation and quoted strings in validator source are allowed.
- External package validation and Unity QA use the same interpretation.

### TD-009 — QA contracts follow the active implementation

- Regression checks validate current behavior and stable ownership contracts.
- QA must not require obsolete local-variable names or superseded package labels after a structural implementation changes.

### TD-010 — Current-only maintained documentation

- Maintained Git documents describe current truth, not a chronological accumulation of package repairs.
- Git history stores historical versions.
- When a document is superseded, merge valid requirements into its authoritative owner, update `ProjectGuide/INDEX.md`, and remove the obsolete file in the same change.
- Root Markdown is restricted to `README.md` and `AGENTS.md`. Maintained project knowledge lives under `ProjectGuide/`.

### TD-011 — Cinematic camera and input ownership

- `BDRunPresentationCoordinator` may temporarily own camera transform and input lock during the mounted entrance.
- `BDCameraFollow` is restored only after the horse completes the approved right turn and full stop.
- The Main Camera GameObject and its sole `AudioListener` remain active.
- All gameplay input readers, including mouse-facing state, respect the central presentation lock.

### TD-012 — Semantic documentation QA

- Stable IDs and implementation anchors remain strict.
- Human documentation wording is validated semantically when equivalent phrases describe the same approved behavior.
- Semantic wording tolerance must not weaken Runtime, scene, ownership, or Play Mode verification.

### TD-013 — One normal-gameplay camera transform owner

- `BDCameraFollow` owns every normal-gameplay Main Camera position and rotation write.
- Composition, room containment, smoothing, collision, and shake are resolved inside that owner before one final transform assignment.
- A second post-follow offset component is not allowed because it can bypass wall containment and produce transition or combat camera drift.
- Mouse yaw uses one rate-limited stage. Wall proximity does not change sensitivity, and combat shake does not change vertical position or pitch.

### TD-014 — CharacterController-root-safe recovery

- Ground recovery computes root height from `CharacterController.center`, `height`, and `skinWidth`; a fixed world-space offset is insufficient.
- Safe-ground probes reject dynamic actors, hazards, structural walls, and moving bodies.
- Successful player damage starts a short grounding guard that freezes safe-point updates and recovers only unexpected floor loss, while intentional jump/dodge/gap motion remains valid.

### TD-015 — Swept hole boundary and active-intent entry

- Ordinary movement is evaluated as a swept CharacterController footprint, not a single endpoint test.
- Walking cannot enter holes through diagonal tunnelling or corner overlap.
- Hole entry permission is state-based and frame-current: active dodge, actively ascending jump, or explicit forced-gap movement only.
- Historical dodge/jump timestamps never authorize later walking movement.
- Recovery clears gap-entry state before control returns.

### TD-016 — Local hole recovery before historical safe points

- Hole falls capture a nearby valid recovery anchor beside the same hazard.
- Local recovery uses a small dedicated edge clearance and CharacterController-root-safe placement.
- The local anchor is preferred before older safe points or spawn, while loop-breaking and global fallbacks remain available only when the local point is invalid.

### TD-017 — Preserve both sides of remote/local divergence

- When remote and local contain unique valid progress, synchronization is a merge task, not a replacement task.
- Inspect the real remote head and local dirty tree before changing history.
- Build and validate the combined working state first, then create a safety reference, commit the verified local result, fetch the remote, and merge its history.
- Never use reset, clean, broad checkout, or an automatic conflict preference unless the merged file content was already reviewed and validated.

### TD-018 — Codex operating contract and project configuration are first-class repository content

- `AGENTS.md` is the canonical project-wide Codex/AI game-development operating contract and belongs in the root Markdown allowlist.
- `.codex/config.toml` and `.codex/agents/*.toml` are maintained project configuration, not caches or temporary assistant output.
- `AGENTS.rtf` is a local editing/export duplicate of `AGENTS.md`; it is ignored and must not be committed as a competing instruction source.
- Repository-hygiene and Unity documentation-governance QA must recognize this ownership without weakening the prohibition on duplicate status or roadmap documents.

### TD-019 — Stable room-bound camera pressure without map regeneration

- Do not enlarge or regenerate the whole maze merely to hide camera jitter when the root cause is camera containment.
- The normal camera boom must fit at room center with a bounded safety inset; room geometry remains authoritative and unchanged.
- Closed-room bounds own ordinary containment. Physical wall casts run only during a legal two-room handoff.
- Camera-body containment and look-point containment are separate: the look point uses a smaller inset and smoothing so wall proximity and actor-height movement do not create apparent zoom or pitch pulses.
- Room lookup is cached and resolved at most once per frame to avoid repeated scene-wide scans during camera follow.

### TD-020 — First-render cinematic camera priming

- Fresh/victory mounted introductions acquire the existing Main Camera synchronously from `sceneLoaded`, before `BDCameraFollow.Start` or `LateUpdate` can render normal follow framing.
- The black transition cover remains opaque while the cinematic pose and FOV are primed.
- The follow driver's prior enabled state and the camera's original projection values are preserved and restored only after the approved stop/return sequence.
- QA validates ordering and active semantic contracts rather than obsolete numeric/text anchors.

### TD-021 — Gated transition diagnostics and actual-pose handoff release

- Transition diagnostics remain a passive observer attached to the existing camera owner; they never write the camera transform and record only after explicit F8 activation.
- A legal two-room union is released from the actual final smoothed camera body and actual smoothed constrained look point, never from an unsmoothed desired position.
- The target, final camera body, and final look point must all be legal in the new room before previous-room bounds are removed.
- Release occurs after the current frame's final pose write and before diagnostic capture, preventing a next-frame containment clamp while preserving an accurate handoff-complete event.
- Existing scene, Codex-agent, diagnostics, and map-geometry changes are preserved.
- If residual motion remains, the marked CSV determines whether it belongs to camera ownership, external camera writes, player/horse root movement, visual-model motion, or Animator root motion before another repair.

### TD-022 — Explicit viewport composition with legacy scene migration

- Normal gameplay composes the player or mounted horse at viewport Y `0.40`, leaving approximately 60% of visible space ahead.
- `BDCameraFollow` derives pitch from camera/target geometry and FOV and remains the single transform owner.
- Runtime bounds legacy serialized minimum-pitch values so authoritative scenes do not need regeneration merely to adopt the composition contract.
- V23R6 actual-pose handoff, room containment, constant normal FOV, and cinematic ownership remain unchanged.

### TD-023 — Heavy-hold hook is one input transaction with independent fallback

- `BDPlayerCombat` owns strong-button short/hold resolution and both cooldown decisions.
- A committed hook applies exactly 2 damage once. Pull eligibility is restricted to small regular enemies; oversized regular enemies, mini-bosses, and bosses remain stationary.
- During hook cooldown, the same strong input may commit the existing heavy sword attack only if that attack is ready.
- The transient hook visual/pull component cannot become a persistent enemy movement owner or bypass closed geometry.

### TD-024 — Unified contextual horse prompts and range-aware target frame are presentation-only

- `BDHorseContextActionPrompts` reads mount/heal/pet state and displays current valid actions with their keys; it never performs the action.
- Mounted prompts appear only while fully stationary, and legacy standalone Heal/Pet presenters suppress themselves when the unified owner exists.
- `BDCombatTargetHighlighter` selects at most one unobstructed enemy using the active weapon envelope and draws only a subtle red corner frame.
- Neither presenter changes input bindings, damage, movement, target lock, actor materials, or camera transforms.

### TD-025 — Airborne melee keeps attack identity and changes only visual plane

- Light and heavy attacks retain their existing committed attack, cooldown, damage owner, color, width, and timing while airborne.
- The standard horizontal slash is suppressed and replaced by one vertical slash.
- The existing descending damage multiplier remains conditional; ordinary airborne presentation does not grant new damage.
- Buffered and immediate attacks follow the same rule.

### TD-026 — Horse action availability and prompt visibility are separate contracts

- On foot, valid Mount, conditional Heal, and Pet actions show their active key.
- Mounted and stationary shows only Dismount; Pet remains key-usable without a prompt; Heal is disabled.
- Mounted and moving shows no prompt row; Dismount remains key-usable while Pet and Heal are disabled.
- Horse healing is on-foot only and mounting clears any active healing transaction.

### TD-027 — One stylized-fantasy visual language and Game Boy menu shell

- `ART_DIRECTION_AND_INTERFACE_CONVENTIONS_V1.md` is the visual source of truth.
- The target balance is 65% colorful wonder and 35% mystery/danger.
- Production uses polished stylized geometry with hand-painted color design and restrained PBR response.
- The full menu language is framed by an original Game Boy-like in-world device while `BDMainMenuFlow` retains navigation/state ownership.
- True victory over Mother persistently restores/awakens the shell and boot/palette treatment.
- User-provided references guide quality, palette, atmosphere, and readability but are never copied literally.

### TD-028 — Root art direction and one gameplay-UI visibility owner

- `ProjectGuide/Product/ART_DIRECTION.md` is the canonical root visual source; the asset-side document is a synchronized mirror.
- `BDGameplayUiVisibility` is the single visibility decision for gameplay HUD and overlays.
- Menu, death, pause, settings, boot, and result states cannot leak gameplay HUD, prompts, minimap, target frames, or damage overlays.

### TD-029 — Safe hook release and truthful long-range target frame

- Hook pulling releases before CharacterController contact while remaining inside the sword envelope.
- The one red target frame represents the enemy the loaded projectile path would actually hit, including distant on-screen targets.

### TD-030 — Professional Parry phases and enemy motion stabilization

- Parry presentation has anticipation, frozen moment, and gradual recovery; player-parented visuals remain attached while the player moves.
- Enemy movement has one late stability owner for impossible displacement and floating recovery, while authored jumps, charges, knockback, and hook pulls use explicit exceptional envelopes.

### TD-031 — Canonical adaptive-music and mix direction

- Root `ProjectGuide/Product/AUDIO_DIRECTION.md` is canonical; the asset-side file is a synchronized mirror.
- Adaptive music uses one future music director, synchronized stems, bar-aware transitions, explicit mixer groups/snapshots, and one master loudness ceiling.
- Mother phase 4 tick-tock is a synchronized Music stem.
- The document completes direction only; C12.42 remains responsible for implementation.

### TD-032 — Bomb explosion owns visible feedback and unique enemy friendly fire

- `BDBombHazard` executes one unique damage transaction per `BDHealth`.
- The bomb owner is immune to its own bomb; other enemies receive damage, stagger, flash, and radial knockback.
- `BDBombExplosionVisual` and `BDGameFeelAudio.PlayBombExplosion` provide synchronized temporary presentation without becoming gameplay owners.

### TD-033 — The committed attack owns airborne presentation

- Hold-capable input may not emit an airborne slash on initial press.
- `BDPlayerCombat.TryMeleeAttack` asks `BDPlayerMeleeEnhancer` to resolve the final committed damage/presentation at the actual attack transaction.
- `BDMeleeSlashArcVisual.SpawnVertical` uses the same mesh and light/heavy identity as grounded attacks, in a vertical high-to-low plane.

### TD-034 — Runtime correction uses real actor roots and one presentation owner

- Systems that move an enemy resolve the actual CharacterController/Rigidbody/combatant root; a child `BDHealth` is not assumed to be locomotion ownership.
- Spawn grounding converts a surface point to the correct root using controller geometry and runs before first visibility.
- Continuous combat-time safety code may correct vertical error but cannot teleport an enemy horizontally.
- Hook release supplies a short contact-attack suppression interval and resets grounding/motion baselines.
- Mounted highlight uses the ranged target origin/envelope without enabling mounted melee.
- Parry/menu transitions clear temporary slash visuals.
- The Game Boy shell and menu content share the single `BDMainMenuFlow` GUI pass.


### TD-035 — Progressive quicksand reuses hazard recovery owners

- Quicksand contact creates/refreshes one `BDQuicksandStatus` on the actual player or horse root.
- Controllers consume its movement multiplier; the status owns sink timing and visual ring only.
- Existing `BDPlayerHazardRecovery` and `BDHorseHazardSafety` remain the only full-submerge relocation owners.
- Quicksand is unsafe for checkpoints but does not instantly recover on entry.

### TD-036 — Target readability uses a constant-pixel silhouette shell

- Target selection remains in `BDCombatTargetHighlighter`.
- Presentation uses an inverted-hull mesh/skinned shell with fixed screen-pixel thickness.
- Rectangular GUI boxes, pulse growth, source-material mutation, and distance-scaled frames are prohibited.

### TD-037 — Audio coverage is event-complete by policy

- The audio direction contains a minimum non-exclusive matrix rather than a closed list of examples.
- Every new gameplay/UI/narrative event requires an intentional authored sound, music, ambience, or deliberate-silence decision.
- Runtime ownership, variation, spatialization, loops, time-scale behavior, voice limits, and mixer routing must be defined before release.

### TD-038 — Damage feedback is applied-damage presentation; test labels are visibility-gated

- `BDHealth` remains the only authority that decides whether damage was actually applied.
- `BDDamageNumberFeedback` is spawned only from the resolved positive damage value and uses different player/enemy color identities.
- Damage-number animation uses unscaled time so cinematic/Parry time scaling cannot strand transient text.
- Prototype hazard labels are never global HUD. `BDPrototypeHazardLabelVisibility` gates them by gameplay UI state, player distance, and camera line of sight.
- Hazard labels may be removed from production content without changing hazard Runtime ownership.

### TD-039 — Animation presentation is mandatory production scope

- Every action requiring motion readability receives a final authored animation or an explicitly approved production-quality procedural solution.
- Gameplay remains the authoritative owner of state, movement, damage and hit timing.
- Placeholder transform motion is tracked as animation debt and cannot silently become release quality.

### TD-040 — Horse and rider have separate hazard damage ownership

- The horse always receives its own hole/chasm or lava damage.
- Mounted hole/chasm damages both horse and rider.
- Mounted lava damages the horse instead of the rider. For every mounted hole/lava recovery, the hazard-specific forced dismount occurs before horse damage callbacks, before horse relocation, and before rider recovery begins.
- This ordering prevents normal buck/faint dismount callbacks from competing with hazard recovery.
- Horse positioning belongs to `BDHorseHazardSafety`; rider recovery belongs to `BDPlayerHazardRecovery`.

### TD-041 — Elite guardians are damageable but not small-enemy forced-movement targets

- `BDCombatantProfile` separates combat damage from forced movement.
- Battery guardians use the explicit `ConfigureEliteGuardian()` policy, currently serialized through the established `MiniBoss` rank with forced movement disabled; they remain fully targetable, damageable and killable.
- Their gameplay category is still **Elite guardian**, not small regular enemy. No new enum value may be inserted casually because existing serialized enum values must remain compatible.
- They opt out of hook pull, knockback, mounted impact, and forced hazard entry.
- Collectible guardians initialize as one inactive complete actor and activate atomically at their legal spawn position. Partial component toggling is not an accepted spawn lifecycle.
- Grappling pull eligibility is evaluated again at impact; a confirmed living small regular enemy hit commits the pull on the real movement root.

### TD-042 — One quicksand multiplier and controlled-jump recovery exclusion

- `BDQuicksandStatus` owns the depth-derived multiplier and pushes it directly to `BDPlayerController`.
- Player movement applies it exactly once through `EffectiveMoveSpeed`; post-velocity filtering does not apply a duplicate player slowdown.
- A controlled jump, including contact with an attacking enemy, is not accidental floor loss and cannot trigger combat safe-point teleport recovery.

### TD-043 — Confirmed abandon and deterministic mounted-intro actor binding

- A live run cannot be abandoned from a single Pause-menu press; confirmation is mandatory.
- `BDMainMenuFlow` remains the only popup/menu state owner.
- Fresh/victory mounted intros bind the active loaded-scene player and horse using typed deterministic selection before moving either actor.
- The exact selected player must be the horse rider for the entire intro. Because the horse controller is disabled during presentation and the rider is intentionally not parented, every external-control movement step must explicitly snap the rider to the mount point.


### TD-044 — Death presentation precedes player menu and regular-enemy cleanup

- `BDCharacterDeathAnimation` owns the visible temporary death pose for player and regular enemies.
- `BDMainMenuFlow` keeps gameplay visible and unpaused during the player pose, disables player action, and opens the menu only after the pose completes.
- Regular enemy AI/collision stops immediately at death; loot and destruction wait until the death pose completes.
- Bosses/mini-bosses with authored death sequences retain their specialized owners.

### TD-045 — Guardian reveal lifetime is independent from collectible lifetime

- Collectible proximity and same-room safety remain owned by `BDCollectibleGuardianSpawner`.
- Guardians are fully constructed inactive before reveal.
- Delayed activation runs on `BDGuardianSpawnSequence`, a separate scene object, so pickup destruction cannot cancel the encounter.
- A player-room fallback is legal only under bounded distance and clear line of sight when the authored hideout point falls just outside exact room bounds.

### TD-046 — Airborne melee reuses grounded identity through transform only

- Air Light/Heavy use the exact grounded selected arc geometry and parameters.
- Airborne presentation rotates that arc exactly 90 degrees and moves it downward in front of the player.
- A separate substitute arc shape or simultaneous grounded slash is prohibited.

### TD-047 — Maintained open-bug ledger

- `ProjectGuide/Status/BUGS.md` is the focused current ledger for defects only.
- Update it in the same change whenever a bug is discovered, changes status, is repaired, is verified, is reopened, or is reclassified.
- `ProjectGuide/Status/CURRENT.md` remains the sole owner of project ordering, active stage, overall QA truth, and resume point.

### TD-048 — Clean abandon reload and synchronous death presentation

- Confirmed abandon reloads the active scene before showing the main menu, preventing stale run state and rider references from leaking into the next run.
- `BDHealth` starts visible death presentation synchronously on lethal damage. Menus, loot, and destruction wait for that owner instead of racing it.
- Death motion targets actual renderer branches rather than relying on one assumed model-child name.


### TD-049 — Character-specific mounted combat permissions

- The current Boy character is ranged-only while mounted and cannot use sword melee or the grappling hook.
- The future Girl character may use the grappling hook and approved attacks while mounted.
- Mounted-combat permissions must come from character-specific profile/capability data when multiple characters exist; never enable a Girl-only capability globally in shared combat code.
- Until the character-profile system is implemented, current Runtime must preserve the Boy restriction explicitly.


### TD-050 — QA follows active semantic ownership rather than historical text anchors

- Historical package markers may remain useful evidence, but they cannot override the active Runtime owner after a documented responsibility move.
- Cross-file contracts are validated at each active owner: the melee enhancer resolves airborne identity/body animation, combat consumes that identity and selects exactly one committed visual branch, and the current mounted-intro restore method owns control release.
- A corrected requirement ID supersedes an incorrect bug ID; QA must validate the current requirement rather than force stale text back into maintained documents.
- Semantic realignment must not weaken behavioral gates such as no duplicate slash, no boy-mounted hook, Elite forced-movement immunity, or death-before-cleanup ordering.


### TD-051 — Character speech uses reusable wordless voice, not UI-wide sounds

- The approved opening line is exactly `I’m bored.` and is future required work.
- Character speech must use a reusable speech-bubble/typewriter system plus character-specific wordless voice profiles and explicit emotion data.
- Wordless voice plays only for actual character dialogue, never automatically for menus, HUD, tips or system text.
- Opening control release waits until text reveal, voice, reading hold and bubble exit are fully complete.
- No temporary sound asset may be represented as final production content.

## Decision lifecycle

A decision may be `ACTIVE`, `SUPERSEDED`, `REJECTED`, or `RECOVERY REQUIRED`. Do not silently delete superseded decisions.

## Template for future decisions

```text
### TD-XXX — Title
Status: ACTIVE | SUPERSEDED | REJECTED | RECOVERY REQUIRED
Date: YYYY-MM-DD
Context:
Decision:
Consequences:
Alternatives considered:
Affected code/docs/QA:
```

### TD-035 — Shared sword spectrum with per-target AOE criticals

- Player sword light, heavy, and airborne attacks resolve one spectrum and one critical roll per committed attack.
- Default variance is ±10%, critical chance is exactly 6%, and the critical multiplier is exactly 1.5.
- Spinning AOE resolves one shared spectrum value before target iteration, then rolls the exact 6% critical chance independently for every unique enemy hit.
- Duplicate colliders of one enemy never create duplicate rolls or duplicate damage.
- Projectiles and grappling hook bypass the sword resolver and remain fixed damage.
- Critical metadata is presentation-only after damage resolution; `BDHealth` remains authoritative.

### TD-035 — Intent-filtered hazards and force-authorized enemy entry

- Enemy AI movement is filtered against registered hazards before execution.
- Knockback and mounted impact explicitly authorize temporary forced entry for small regular enemies.
- Environmental outcome is then owned by the hazard: hole/lava immediate death, quicksand sink-death.

### TD-036 — Mounted collision damage derives from speed and directness

- A mounted horse collision against a small regular enemy resolves continuous 4-10 damage from speed and contact directness.
- Knockback direction blends travel direction with the outward contact direction and uses a per-enemy cooldown.

### TD-037 — Wall jump is a second committed jump from recent wall contact

- Wall jump is owned by `BDPlayerController`, uses a grace window, rotates away from the wall, and applies upward plus temporary horizontal velocity.

### TD-036 — Explicit hazard and airborne-presentation ownership

- Quicksand suppresses generic nonlethal ground recovery while its state is active; only its failure path may respawn.
- Wall-jump eligibility is based on physical solid vertical surfaces rather than wall-only object categories.
- Airborne melee presentation is selected explicitly by the committed attack and does not rely on delayed suppression of the grounded visual.

### TD-052 — Durable task continuity is repository state

- Every material task updates `ProjectGuide/Status/CURRENT.md` with the global current truth and exact resume point.
- Large, multi-step, cross-system or multi-session work also receives a detailed record under `ProjectGuide/Tasks/`.
- The record owns task rationale, decomposition, implementation evidence, unverified areas, blockers and handoff context; durable system behavior remains in its canonical domain documents.
- Task records, global status, bug ledger and relevant architecture/QA/performance/decision documents are synchronized in the same change.
- A chat, package README or local note is never the only source for material task context.
- Verification vocabulary remains evidence-based; automated PASS does not imply Play Mode, device, camera-feel, performance, visual, audio or user acceptance.

#### V23R19M exact axis clarification

For the current grounded arc mesh, local X is the visible left-to-right long axis and local Z is the forward/depth axis. Airborne presentation rotates 90 degrees around local Z so the long axis becomes world up/down while attack depth continues to face forward. Rotating around local X only stands the plane up and is insufficient because it leaves the visible long axis horizontal.

#### V23R19O mounted renderer, damageable silhouette and steerable wall-jump decisions

- Mounted intro owns temporary rider-renderer visibility in addition to Transform binding. A teleported skinned rider must update offscreen during the cinematic to prevent bounds culling and late pop-in.
- Target silhouette eligibility is collider-envelope based. A renderer parented to an enemy is not automatically damageable presentation.
- Auxiliary ground rings are presentation-only, receive reduced alpha and are never target-outline geometry.
- Wall-jump steering rotates the launch vector at a bounded rate while retaining an outward impulse. Player facing and camera intent consume the same current trajectory so movement, model and view do not disagree.

#### V23R19P Caterpillar gambling invariants

- Room eligibility and room safety are different states: the run selects only some rooms for a Caterpillar; enemy presence temporarily hides an eligible Caterpillar but never creates one in an unselected room.
- Appearance and disappearance are explicit animated state transitions.
- A gambling session is a single owned transaction with a temporary safety state that prevents enemy approach and attacks until deterministic cleanup.
- Each Caterpillar instance owns exactly one assigned game.
- Passive refill is capped at the normal threshold; money won from the player may exceed it and is retained.
- Game rules, odds, values, frequency, art, exact safety mechanism and balance remain open.

#### V23R19Q remembered-handheld rendering

- Use an original memory-based handheld design rather than a literal commercial-device copy.
- Preserve one-pass IMGUI ownership because replacing the UI framework inside a focused visual polish task would create unnecessary behavior risk.
- Improve finish through cached procedural textures, consistent screen palette, layout hierarchy and a short mode transition.
- Tiled scanlines use one cached texture draw.
- Rounded body/screen elements reuse one cached alpha texture.
- No Texture2D or Material is created in OnGUI.

<!-- B&D TD-053 MODERN 3D HANDHELD START -->
### TD-053 — One semantic menu owner behind a real 3D handheld view

- The approved final menu device is a true upright 3D object with separate shell, glass, display and tactile controls.
- The visual upgrade does not authorize a parallel menu controller: `BDMainMenuFlow` remains the semantic state/action owner.
- Menu content should render to the physical screen through a dedicated view/RenderTexture architecture that can remain aligned under depth and approved device motion.
- Physical controls, mouse, keyboard and controller map to the same semantic commands.
- Settings and Progression center buttons are direct shortcuts; X opens Settings and Y opens Progression; A confirms and B returns.
- The user-facing label is `Progression`.
- Character-bearing UI art is paired Boy/Girl content with deterministic active-character selection.
- Flat one-piece screenshot projection is rejected as the final implementation because it cannot provide tactile depth, true glass layering or physical button interaction.
<!-- B&D TD-053 MODERN 3D HANDHELD END -->
