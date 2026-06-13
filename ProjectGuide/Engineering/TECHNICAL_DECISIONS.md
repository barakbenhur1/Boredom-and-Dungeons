<!-- BND_TUTORIAL_CHARGED_SEQUENCE_METAL_QUICKSAND_V1011331:BEGIN -->
## Decision — explicit persistent screen color and depth buffers

The live handheld screen keeps its existing camera/uGUI architecture, but color and depth/stencil are separate persistent RenderTextures bound through `Camera.SetTargetBuffers`. This prevents a platform render path from silently substituting a transient memoryless depth surface while preserving stencil-based UI masks. Both targets have one lifecycle owner and are released together.
<!-- BND_TUTORIAL_CHARGED_SEQUENCE_METAL_QUICKSAND_V1011331:END -->

<!-- BND_POST_INTRO_CINEMATIC_QA_LATEST_COMMIT_ALIGNMENT_V1094:BEGIN -->
## V10.9.4 decision — preserve latest commit and validate authoritative partials

When a production owner is split across C# partial files, QA follows responsibility boundaries. A migrated geometry or presentation token is validated in its authoritative partial. Dead object names are not reintroduced as comments, duplicate objects or fake strings to satisfy stale scans. This decision repaired the `Short Core Shadow To Left` false blocker without changing Runtime behavior.
<!-- BND_POST_INTRO_CINEMATIC_QA_LATEST_COMMIT_ALIGNMENT_V1094:END -->

<!-- BND_POST_INTRO_CINEMATIC_DIRECTOR_PASS_V109:BEGIN -->
## Decision — generated full 3D product set and one natural-cubic camera

The existing presenter already owns generated product geometry and the product camera, so V10.9 extends that owner rather than adding Timeline, Cinemachine, a second camera or a pre-rendered clip. A five-knot non-uniform natural cubic spline is used because it provides continuous velocity/acceleration through the establish, descent, alignment and settle regions. Independent jerk-limited clocks tune X, Y, Z, look and FOV; X reaches the final center first. Regular and cinematic completion poses share centralized position/rotation/FOV/clip values. The full table and cyclorama are runtime-generated from existing approved wood assets to preserve repository architecture and avoid an unrelated asset/package dependency.
<!-- BND_POST_INTRO_CINEMATIC_DIRECTOR_PASS_V109:END -->

<!-- BND_FIRST_LAUNCH_TUTORIAL_V1081_HOTFIX:BEGIN -->
## Decision — impact-owned tutorial progression and camera-only product-scene landing

**Status:** `ACTIVE / UNITY VERIFICATION REQUIRED`

A ranged tutorial lesson is complete only when its visible projectile transaction confirms impact on the intended living target. Input, ammunition changes and animation completion are insufficient evidence. This prevents both premature completion and the V10.8 stuck state caused by a non-advancing transaction.

The post-BBH product scene is never animated as a card, scaled frame or moving device. The table, handheld and shadow are persistent world geometry; the cinematic result comes exclusively from the existing perspective camera's dolly, aim and restrained lens change. Static table coverage may be enlarged off-screen to protect composition across aspect ratios.
<!-- BND_FIRST_LAUNCH_TUTORIAL_V1081_HOTFIX:END -->


## Decision — textured caps over modeled control bodies

Controls must not be a flat front-sheet overlay. Each D-pad direction, face button and center shortcut is a real moving 3D body. A small transparent textured cap, cropped from the approved orthographic source sheet, is parented to that moving body. This preserves tactile depth, independent hit targets and authentic surface detail while preventing any texture from covering neighboring controls.

# Technical Decisions — Durable Project Choices

<!-- B&D BBH CINEMATIC SIDE TASK V1 START -->
### TD-037 — Cinematic BBH motion stays inside the existing intro owner

**Decision:** Extend the existing BDBBHBootIntro owner with distinct authored motion for the first B, second B and H, subtle inter-letter reactions, one deterministic completion breath, and a responsive circle with a 16% larger desired diameter. Do not create a parallel intro, menu, camera, Timeline, input or state owner for this focused side task.

**Reason:** The requested liveliness is achievable inside the current short boot presenter while preserving first-frame black, strict sequence, session gating, unscaled timing and existing menu integration. Keeping one owner reduces integration risk while allowing later replacement by production-authored assets if approved.

**Constraint:** This side task does not change the current handheld priority, work queue, resume point, gameplay, menu design or camera handoff. Unity compilation, TEST EVERYTHING and focused Play Mode remain required before verification.
<!-- B&D BBH CINEMATIC SIDE TASK V1 END -->

### TD-034 — Molded handheld surface replaces full-face decal

**Decision:** Do not render a full-device transparent sticker over the 3D model. Use generated molded geometry plus an object-space blue→violet→orange surface shader. The original texture sheet remains reference input, not a flat Runtime shell replacement.

**Reason:** The full-face decal passed automation but visibly crossed controls, flattened material depth and failed the product-realism gate.

### TD-035 — Product shadow and glass obey one upper-right key light

- A cached soft penumbra, denser core and tight contact shadow are cast to the left.
- Glass receives a low-opacity glint only in the upper-right region; central UI readability wins over spectacle.

### TD-036 — New Game-only identity and keyboard navigation parity

- Active-character art appears only in the large Start Game/New Run hero panel.
- The small card is fresh-New-Game-only, text-only and contains no route/Mother identity.
- W/A/S/D and arrow keys are equivalent navigation inputs under the same release gate.


## TD-HANDHELD-ART-ROUTING — protagonist pair only for New Game

**Decision:** Keep one Boy/Girl pair only for Start Game / New Run. Resolve all other option/page images from shared character-neutral assets.

**Reason:** The protagonist identity matters when starting a route, but duplicating Progression, Settings, Credits, Quit and Pause artwork creates unnecessary asset cost and risks visual drift without adding product meaning.

## TD-HANDHELD-DECAL-QUALITY — high-resolution masked lit decal

**Superseded V3 decision:** A 2048×3220 transparent front decal was attempted, passed automated QA and failed visual acceptance. TD-034 replaces it with molded geometry and an object-space shell material.

**Reason:** A low-resolution unlit sticker looked soft and disconnected from the physical shell. The dedicated shader preserves crisp detail while responding subtly to the same product-light direction.

## TD-HANDHELD-UI-002 — Declare uGUI explicitly for the live device screen

**Decision:** The 3D handheld's internal screen uses Unity's GameObject-based uGUI package (`com.unity.ugui` `2.0.0`) for `Canvas`, `Image`, `Text`, `RawImage` and `Outline`, rendered by the isolated screen camera into the cached RenderTexture.

**Reason:** The presenter already uses those components, and `com.unity.modules.ui` does not supply the `UnityEngine.UI` namespace. Explicit package ownership prevents environment-dependent compilation and keeps the existing live-screen architecture intact.

**Constraints:**

- Do not remove or silently downgrade the dependency while the presenter uses uGUI.
- Do not add a second UI framework for the same screen without a documented migration plan.
- Package presence is enforced by TEST EVERYTHING.
- Package resolution is not considered verified until Unity compiles and automated QA reruns.

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
- Final physical mapping: Main Menu X=New Game, A=Progression, B=Settings, Y=Credits; B=Back on non-main pages; center SELECT activates focus; center EXIT opens the legal confirmation.
- The user-facing label is `Progression`.
- Character-bearing UI art is allowed only for Start Game / New Run and uses one deterministic Boy/Girl pair. All other menu art is character-neutral.
- Flat one-piece screenshot projection is rejected as the final implementation because it cannot provide tactile depth, true glass layering or physical button interaction.
<!-- B&D TD-053 MODERN 3D HANDHELD END -->

### TD-034 — One real 3D handheld presenter with active-character art authority

- `BDMainMenuFlow` remains the authoritative menu/run-state owner.
- `BDModernHandheld3DPresenter` is a presentation and input-translation layer only.
- One cached screen camera and RenderTexture feed a real recessed display behind separate glass.
- Device controls are modeled objects with bounded transform/material feedback; they do not use gameplay physics.
- Mouse, D-pad, A/B/X/Y and physical shortcut clicks resolve to the same flow actions.
- `BDPlayableCharacterIdentity` is authoritative only for the Start Game / New Run pair: Boy identity selects Boy art and Girl identity selects Girl art. Selection is never random, and persisted fallback cannot override an active identity. Progression, Settings, Credits, Quit/Return, Resume/Pause and confirmation never consult protagonist identity.
- Paired assets must match dimensions/import settings and are validated by TEST EVERYTHING.
- Legacy IMGUI is retained only as a safe fallback during rollout, not as a simultaneous visual owner.


### TD-054 — Masked shell decal and product-shot focus without flattening the device

- The supplied handheld texture sheet may be used as a front-surface decal only after masking the screen and control regions transparent. It is visual surface detail, not a substitute for shell volume, glass, display or controls.
- The supplied wood image is the table source. Progressive focus is implemented by blending matched sharp/defocused versions in one table shader around the device focal band, avoiding dependency on a global post-processing package and avoiding a uniform fake blur.
- Device, table and shadow share one product-shot rest transform; the device upper edge reads slightly farther from the camera.
- Surface lighting is authored to read from upper-right and shadow assets are short/leftward.
- Physical-device raycasts use a dedicated high-number layer, not Unity's built-in UI layer.
- Each D-pad direction animates a separate visible cap so pointer and keyboard/controller activation produce the same tactile result.
- New Game character-art resolution is event-driven/cached. Active route always wins; missing Girl art never falls back visually to Boy. Neutral option art remains single-source and route-independent.

## Final handheld control decision — 2026-06-09

The presenter owns one semantic action per physical control: center SELECT activates focus; center EXIT opens the legal quit/abandon confirmation; Main Menu X starts New Game, A opens Progression, B opens Settings and Y opens Credits; B returns on every non-main page. WASD/arrows remain navigation-only, so keyboard A is never overloaded as the face-button A shortcut. Page UI, button pulse and state transition must derive from the same semantic action to prevent double execution or mismatched labels.

<!-- B&D TD-038 FIRST LAUNCH OWNER AND DIRECT HANDHELD REPAIR START -->
### TD-038 — First launch is a handheld screen mode; tactile repair belongs to the control owner

**Decision:** Implement the one-time tutorial as an explicit `EffectivePage.FirstLaunchTutorial` mode of the existing handheld presenter, with a separate durable state store. Integrate device position, SELECT/EXIT placement, context-card behavior, Pause composition, text bounds and hardware tactile behavior directly into their current owners. Retire the merged V6 companion/compatibility classes after migration.

**Reason:** The requested behavior is part of existing handheld screen and control responsibilities. Direct ownership provides deterministic lifecycle, prevents per-frame hierarchy scanning and duplicate state, and makes `TEST EVERYTHING` able to validate the real implementation.

**Persistence:** `Completed` and `Skipped` are terminal for automatic display; `InProgress` restarts safely. `Skipped` is saved before visual transition.

**Delivery:** Assistant output is a local backup-aware patch only. Git/GitHub writes are prohibited.
<!-- B&D TD-038 FIRST LAUNCH OWNER AND DIRECT HANDHELD REPAIR END -->

<!-- B&D TD-039 CONTEXTUAL HUD + DISCRETE HORSE INJURY V2 START -->
## TD-039 — Contextual HUD and discrete horse injury bands

- Horse speed uses exact discrete 30%-missing-health bands, not a continuous interpolation.
- Health/ammo widgets reveal on player intent or state events and fade professionally; they are not permanently opaque.
- The minimap remains available while stationary because that is a common map-reading moment. It dims to 38% after an idle delay instead of disappearing, and wakes on movement, threat, discovery or explicit map input.
- Horse interaction prompts move to a bottom HUD strip so no icon competes with the horse silhouette.
<!-- B&D TD-039 CONTEXTUAL HUD + DISCRETE HORSE INJURY V2 END -->

<!-- BND_QA_CONTRACT_REALIGNMENT_V5:BEGIN -->
## TD-038 — Regression QA follows current authoritative behavior

Automated regression checks must validate the current approved owner and semantic contract. They must not force superseded numeric literals or retired presentation mechanisms back into production code merely to satisfy an old token check.

Applied examples:

- handheld composition validates the approved `0.28f` rest offset rather than the retired `0.62f` value;
- horse interaction QA validates the fixed bottom contextual strip and forbids the retired world-space prompt projection contract;
- maintained prose may include explicit canonical summaries when those summaries improve human readability and automated stability.
<!-- BND_QA_CONTRACT_REALIGNMENT_V5:END -->

<!-- BND_TUTORIAL_REFERENCE_LED_V3:BEGIN -->
## TD-040 — Tutorial input presentation uses parallel cards

The tutorial has one normalized action gate and two simultaneous presentation routes. Keyboard/mouse and physical-handheld bindings are resolved separately and rendered in dedicated cards. No last-input-source switch removes an active route.

The same tutorial owner remains responsible for flow and input. The pixel-presentation partial owns generated point-filtered sprites, reference-led backdrop decoration, binding-card emphasis and stepped visual animation. No adapter, compatibility layer or second state machine is introduced.
<!-- BND_TUTORIAL_REFERENCE_LED_V3:END -->

<!-- BND_FIRST_LAUNCH_TUTORIAL_QA_CONTRACT_FIX_V8:BEGIN -->
## TD-041 — composed UI fields are validated independently

QA must not require values from separate UI fields to appear as one
contiguous source token. Card titles and dynamically resolved bindings are
validated through their separate authoritative contracts.
<!-- BND_FIRST_LAUNCH_TUTORIAL_QA_CONTRACT_FIX_V8:END -->

<!-- BND_FIRST_LAUNCH_TUTORIAL_PRODUCTION_COURSE_V10:BEGIN -->
## TD-053 — Tutorial teaches through one bounded playable course

The first-launch tutorial uses a fixed side-view course with contextual prompts, explicit learning evidence and soft progression gates. It does not reuse the production procedural run or introduce a second global input/menu owner. Optional mechanics such as Parry and the secret are encouraged but do not block completion.

## TD-054 — Normal exit and Abandon are separate run transactions

A future normal Save & Return preserves a valid Continue snapshot and grants no end-of-run reward. Abandon is destructive, uses the shared death-equivalent meta evaluator, applies the approved 0.84 multiplier before existing rounding/clamp, awards once, presents the shared result screen, and begins the exit animation only after that screen closes.
<!-- BND_FIRST_LAUNCH_TUTORIAL_PRODUCTION_COURSE_V10:END -->

<!-- BND_FIRST_LAUNCH_TUTORIAL_V10_WARNING_CLEANUP_V101:BEGIN -->
## TD-055 — One tutorial learning-evidence source

The tutorial uses `TutorialLearningState` as its sole mechanic-evidence model. Per-mechanic write-only boolean mirrors are rejected. This eliminates duplicate truth, prevents CS0414 debt and keeps completion evidence queryable through one state contract.
<!-- BND_FIRST_LAUNCH_TUTORIAL_V10_WARNING_CLEANUP_V101:END -->

<!-- BND_FIRST_LAUNCH_TUTORIAL_V10_INPUT_RESPAWN_FLASH_REPAIR_V102:BEGIN -->
## TD-TUTORIAL-V102 — Tutorial instruction truth and hidden checkpoint restore

Status: ACTIVE
Date: 2026-06-10

Decision:
- Tutorial prompts and consumers mirror the actual gameplay gesture rather than mapping a convenient substitute.
- Dodge is a directional double-tap transaction; Parry is a timed light/heavy melee transaction.
- Checkpoint relocation occurs only behind an opaque cached presentation cover.
- Pending first-launch state reserves the modern presenter before menu-flow lookup completes, and legacy presentation is suppressed during that reservation.

Consequences:
- physical B is available for Jump while D-pad double-tap owns Dodge;
- X and Y both remain valid Parry inputs in the Parry context;
- no uncovered checkpoint teleport or one-frame legacy-menu exposure is acceptable;
- static QA rejects the retired bindings and flow order.
<!-- BND_FIRST_LAUNCH_TUTORIAL_V10_INPUT_RESPAWN_FLASH_REPAIR_V102:END -->

<!-- BND_INTRO_TO_MAIN_MENU_CINEMATIC_AND_TUTORIAL_SPACING_V105:BEGIN -->
## Decision — explicit one-shot intro destination signal

The post-intro cinematic is driven by an explicit one-shot request owned by `BDBBHBootIntro`, never inferred from time, scene load, camera transform or generic menu visibility.
<!-- BND_INTRO_TO_MAIN_MENU_CINEMATIC_AND_TUTORIAL_SPACING_V105:END -->

<!-- BND_BBH_GLOBAL_TIMESCALE_REMOVAL_V106:BEGIN -->
## Decision — presentation code does not own global time scale

Boot, tutorial and handheld presentation components may use unscaled timing and explicit input/state gates, but may not assign the global simulation clock. Gameplay-wide pause ownership must remain with the authoritative pause/run flow.
<!-- BND_BBH_GLOBAL_TIMESCALE_REMOVAL_V106:END -->

<!-- BND_POST_INTRO_TRANSITION_COLORED_OUTPUT_CLEAN_EXIT_V1072:BEGIN -->
## Decision — validator scope and installer cleanup

Runtime implementation invariants are evaluated only against Runtime source. Editor validators are validated separately and must not self-trigger through their rule literals. ZIP installers use a shell EXIT/finally cleanup registered before verification and remove owned ZIPs and extracted package residue on every outcome.
<!-- BND_POST_INTRO_TRANSITION_COLORED_OUTPUT_CLEAN_EXIT_V1072:END -->

<!-- BND_FIRST_LAUNCH_TUTORIAL_MECHANICS_REPAIR_V108:BEGIN -->
## TD-TUTORIAL-V108 — Semantic completion owns tutorial effects

Status: `ACTIVE / UNITY VERIFICATION REQUIRED`
Date: 2026-06-10

Decision:

- a visible tutorial projectile applies damage only at its impact event;
- a tutorial Hook applies damage/progression only after the pull presentation completes;
- tutorial Charged Shot mirrors `BDPlayerCombat` and auto-fires at full charge without release;
- living actor bodies block movement through a bounded tutorial-local collision resolver;
- lesson boundaries use invisible clamps plus instruction feedback, not decorative divider geometry;
- boss attacks expose explicit telegraph/commit/recovery state and damage windows;
- the post-BBH special shot manipulates the real 3D scene, never a screen-space proxy.

Reason: gameplay truth, visual timing and learning evidence must describe the same event. Immediate damage, timer-only attacks and proxy camera effects made the tutorial misleading even when automated source checks passed.

Consequences: presentation completion forwards to one authoritative damage/progression owner; final assets may replace procedural frames without changing semantic event order; Unity Play Mode remains mandatory because static checks cannot prove visual contact, readability or fairness.
<!-- BND_FIRST_LAUNCH_TUTORIAL_MECHANICS_REPAIR_V108:END -->
