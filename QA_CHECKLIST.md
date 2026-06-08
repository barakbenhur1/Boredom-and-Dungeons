## Active V23R19F semantic-QA compatibility gate

1. Guardian same-room QA validates the active V23R19E owner names and behavior: `TryResolvePlayerRoomFallback`, `spawnRoom.ContainsWorldPosition(`, and `playerTransform.position`.
2. The scanner must not require the removed local variable spelling `player.position`.
3. V23R9 status QA validates the maintained phrase `V23R8 automated QA passed` and the stable `Saved feature resume point`.
4. No Runtime or scene code is added solely to satisfy stale text.
5. TEST EVERYTHING must report neither `GUARDIAN_SAME_ROOM_SAFETY_MISSING` nor `V23R9_PROJECT_STATUS_MISSING`.
6. Passing this static gate does not close V23R19E focused Play Mode verification.

## Future cross-system expansion QA source

The future automated/manual matrix for enemy attack timing, bombs, flutes, summoned/ambient creatures, map teleport, movement animation, living-world effects, UI, performance, and delivery reporting is maintained in `Assets/_Project/Design/Runtime/GAMEPLAY_ABILITIES_MAP_AMBIENT_UI_EXPANSION_HE_V1.md`. Those checks become active only as each feature slice is implemented; their presence in the specification is not a pass claim.

## Active V23R19E mounted replay, death, airborne, and Battery-guardian gate

1. **Mounted abandon replay:** abandon a live run, start a new run, and verify the exact player is snapped to the horse before the first visible entrance movement and remains mounted until control release.
2. **Exact 90-degree airborne identity:** Air Light/Heavy reuse the exact grounded Light/Heavy arc geometry and differ only by the approved 90-degree orientation/vertical movement; no second horizontal slash appears.
3. **Player death before menu:** lethal damage disables player action, shows the full player death pose in gameplay, and only then opens the Game Boy menu.
4. **Enemy death before loot/despawn:** regular enemy archetypes stop movement/attacks, animate death, then release loot and despawn.
5. **Guardian reveal survives pickup:** collect Battery A/B immediately during reveal; the independent runner still activates every guardian.
6. **Guardian room safety:** exact pickup-room ownership is preferred; fallback uses only the player's containing room with bounded distance and clear line of sight.
7. **Guardian combat:** spawned guardians chase, attack, take player damage, die with animation, and retain Elite immunity to hook pull/knockback/small-enemy forced movement.
8. **Retained gates:** quicksand, jump-contact recovery, abandon confirmation, hook, mounted hazards, camera, Parry, bombs, and Console cleanliness remain passing.

## Active V23R18B mounted-hole ordering and animation-token gate

1. **Exact token:** root art direction, its Unity mirror, and production-animation requirements contain `temporary procedural animation is not final release animation`.
2. **Real API owner:** `ForceDismountAfterHazardRecovery` is provided by `BDHorseController`; QA does not search for it in `BDPlayerHazardRecovery`.
3. **Ordering:** mounted hole/lava force-dismounts before horse damage callbacks, before horse relocation, and before rider recovery.
4. **Damage callback isolation:** hazard damage reaching buck/faint thresholds cannot start the ordinary mounted buck/dismount presentation.
5. **Mounted hole:** horse and rider receive configured fall damage and finish unmounted at separate legal safe positions.
6. **Mounted lava:** only the horse receives lava damage; the rider follows the zero-damage recovery arc and both finish unmounted.
7. **Automated gate:** Unity compiles and TEST EVERYTHING passes with zero blockers and warnings.
8. **Focused gate:** repeat mounted hole/lava at normal HP and at damage-burst/faint thresholds before marking V23R18B complete.

## Active V23R18A production-animation and horse-hazard gate

1. **Animation authority:** `ART_DIRECTION.md` links to the complete production-animation requirements.
2. **Coverage:** player, horse, enemies, bosses, interactions, hazards, destructibles, UI and cinematics are included.
3. **No false completion:** prototype procedural motion remains animation debt until approved at production quality.
4. **Unmounted hole:** horse receives configured fall damage and returns to its latest legal safe point.
5. **Mounted hole:** horse and rider both receive fall damage; mounted state ends safely.
6. **Unmounted lava:** horse receives lava damage and returns to its latest legal safe point.
7. **Mounted lava:** horse receives lava damage; rider receives zero lava damage and follows the recovery arc; mounted state ends before relocation.
8. **Paired recovery:** rider and horse finish at valid separated safe positions.
9. **Semantic quicksand QA:** current V23R17 player/horse/enemy failure methods and volume marker satisfy retained scanning.
10. **Retained safety:** quicksand, enemy hazard avoidance/forced entry, mounted collision damage, wall jump, intro direction and airborne attacks remain passing.
11. **Automated gate:** Unity compiles and TEST EVERYTHING passes with zero blockers and warnings.
12. **Focused gate:** record actual Play Mode results before marking V23R18A complete.

## Active V23R17 movement, hazard, and mounted-impact gate

1. Player movement multiplier decreases continuously with quicksand depth.
2. Grounded quicksand contact deals 2 damage per complete second; airborne time does not.
3. Jump extraction occurs once per committed jump; dodge pauses sinking and does not extract.
4. Half-body sink applies fall damage and safe recovery without a loop.
5. Enemy brain movement cannot enter hole, lava, quicksand, or future registered hazards.
6. Knockback and mounted impacts may force only small regular enemies into hazards.
7. Eligible enemies die immediately in hole/lava and sink before death in quicksand.
8. Mounted grazing/direct impacts deal 4-10 according to speed/directness and knock in the actual impact direction.
9. Repeated overlap cannot multi-hit inside the per-enemy cooldown.
10. Wall jump requires recent airborne wall contact, turns away, and produces a medium arc.
11. Intro horse ends facing a clear room direction and airborne Light/Heavy use distinct body animation.
12. Retained camera, combat, menu, damage-number, target-outline, and critical gates remain passing.

## Active V23R15D canonical-root token gate

1. Root `ART_DIRECTION.md` is the sole canonical visual source.
2. The Unity-side mirror includes the exact phrase `Canonical root source`.
3. The same declaration names `ART_DIRECTION.md` explicitly.
4. The mirror remains synchronized and does not become an independent policy.
5. No Runtime, combat, hazard, scene, targeting, or damage file is changed.
6. TEST EVERYTHING runs without `V23R9_ART_DIRECTION_MIRROR_MISSING`.

## Active V23R15C art-direction mirror gate

1. Root `ART_DIRECTION.md` remains the canonical visual source of truth.
2. `Assets/_Project/Design/Visual/ART_DIRECTION_AND_INTERFACE_CONVENTIONS_V1.md` explicitly names `ART_DIRECTION.md` as its canonical repository source.
3. The Unity-side document remains a synchronized mirror, not a competing policy.
4. No Runtime, combat, hazard, scene, or target-presentation file is changed by V23R15C.
5. Unity compiles and TEST EVERYTHING runs without `V23R9_ART_DIRECTION_MIRROR_MISSING`.
6. Existing V23R15/V23R15B damage-spectrum and independent AOE-critical gates remain unchanged.

## Active V23R15B per-target AOE critical and semantic QA gate

1. Spin resolves one shared ±10% pre-critical spectrum value before iterating targets.
2. Every unique enemy hit by the spin calls the exact 6% critical resolver independently inside the loop.
3. One spin may produce a normal amber number on one target and a critical fuchsia number on another.
4. Critical damage remains exactly 1.5 times the shared pre-critical value for the successful target.
5. Duplicate colliders belonging to one enemy do not produce duplicate rolls or duplicate damage.
6. Light, heavy, airborne light, and airborne heavy retain one critical state per committed attack.
7. Projectile and grappling-hook damage remain fixed and non-critical.
8. Quicksand, damage-number dispatch, and art-direction mirror QA validate active semantic contracts rather than stale exact text.
9. Unity compiles and TEST EVERYTHING runs with the three latest blockers absent.
10. Focused Play Mode verification is required before marking V23R15/V23R15B complete.

## Active V23R15A QA result API compatibility gate

1. `BDV23R14DamageNumbersLabelVisibilityQA` uses `result.findings` and `BDOneClickQAFinding`.
2. `BDV23R15MeleeDamageCriticalQA` uses `result.findings` and `BDOneClickQAFinding`.
3. Both scanners use `BDOneClickQASeverity.Blocker` and preserve their existing finding code/message content.
4. Neither scanner contains `result.Blockers` or `BDOneClickQAIssue`.
5. Unity compiles without the reported CS1061 and CS0246 errors.
6. TEST EVERYTHING runs before any Runtime PASS claim.
7. Existing V23R14/V23R15 damage-number, sword-spectrum, critical, projectile, and hook gates remain unchanged.

## Active V23R15 melee spectrum and critical gate

1. Unity compiles with no C# errors.
2. Light, heavy, airborne light/heavy, and spin vary around configured base damage.
3. Default sword variance is ±10%; spin shares one pre-critical spectrum roll across its targets.
4. Critical chance is exactly 6%; multiplier is exactly 1.5.
5. Spin rolls critical independently for every unique enemy hit.
6. Projectile and hook damage remain fixed for identical conditions.
7. Projectile and hook hits never use the critical damage-number color.
8. Critical sword hits use the dedicated fuchsia/magenta number color.
9. Misses, blocked damage, dodge, and Parry do not display false critical numbers.
10. Existing V23R14 damage-number and hazard-label visibility gates remain passing.

# QA Checklist

## Active V23R13 audio, quicksand, silhouette-outline, and warning gate

1. **Audio direction completeness:** root and Unity mirror include the non-exclusive sound-event matrix for combat, movement, breakables, hazards, horse, UI, menu, intro, ambience, enemies, bosses, and future actions.
2. **Honest audio status:** C12.42 remains open; documentation does not claim the authored library or mixer is already complete.
3. **Quicksand type:** `BDHazardType.Quicksand`, `BDQuicksandStatus`, player/horse movement filtering, full-sink recovery, prototype cues, and scene installation exist.
4. **Prototype volume set:** exactly one hole/chasm, one lava, and one quicksand volume are installed.
5. **Player quicksand:** progressive slow, visible following ring, escape decay, 12-damage full sink, and safe-point recovery.
6. **Horse quicksand:** progressive slow and safe full-sink recovery; mounted rider is not damaged twice.
7. **Silhouette only:** no `OnGUI`, `Rect`, corner frame, pulse amount, minimum box size, or distance-based frame growth remains in `BDCombatTargetHighlighter`.
8. **Outline renderer:** one fixed-pixel red inverted-hull shell follows eligible mesh/skinned renderers and is removed/hidden when target validity ends.
9. **Target truth:** one target, current weapon range, mounted ranged envelope, projectile path, and blocker rules remain unchanged.
10. **Warnings:** the four reported CS0414 fields are absent and Unity Console is checked after compilation.
11. **Retained safety:** V23R12 runtime gates, hole/lava behavior, camera, menu, horse, bombs, airborne attacks, and enemy stability remain passing.

## Active V23R12 focused runtime-regression gate

1. Hook resolves the actual enemy movement root when `BDHealth` is on a child.
2. Pull-size classification ignores attack, awareness, vision, range, prompt, and telegraph helper colliders.
3. Multiple small regular archetypes pull consistently; oversized regular, mini-boss, and boss targets remain damage-only.
4. Pulled enemies stop before overlap, remain in sword range, ground correctly, and cannot immediately contact-attack the player.
5. Mounted ranged aiming selects at most one unobstructed projectile-valid target using a horse-height origin.
6. A successful Parry clears all active player slash visuals before anticipation/freeze; menu/death reset does the same.
7. Horse action prompts sit above status/name/health and no legacy action sentence duplicates them.
8. Enemy spawn placement accounts for CharacterController center/height/radius/scale before first visibility.
9. Enemy late safety correction cannot relocate an active enemy horizontally; authored jump/charge/knockback/hook exceptions remain legal.
10. Ground stick, placement guard, jumper landing, hook completion, and motion stabilizer agree on root position and baseline.
11. Main menu, death, pause, and settings render content and the Game Boy shell from the same GUI owner/pass.
12. TEST EVERYTHING passes, then all visual/runtime items are verified repeatedly in Play Mode before PASS.

## Active V23R11 audio, bomb, and airborne-attack gate

1. `AUDIO_DIRECTION.md` and its Unity-side mirror are synchronized and indexed as canonical/mirror rather than two competing sources.
2. Exploration, combat, mini-boss, boss, and Mother phase music contracts are explicit; Mother phase 4 includes a synchronized tick-tock stem.
3. AudioMixer groups, snapshots, headroom, true-peak, loudness, ducking, and transition rules are documented without falsely claiming runtime implementation.
4. A trap-layer bomb produces a visible core, expanding ring, sparks, explosion audio, and near-player camera accent.
5. Player and horse damage remain correct; nearby non-owner enemies each take one damage event despite multiple colliders.
6. Bomb owner immunity, stagger, flash, knockback, cleanup, and grounding remain stable.
7. The real committed light/heavy attack chooses the airborne presentation.
8. Airborne light uses the normal light mesh/color/timing language in a vertical high-to-low plane.
9. Airborne heavy uses the normal heavy mesh/color/weight language in a vertical high-to-low plane.
10. No early hold-press visual and no later horizontal duplicate appear.
11. Grounded light/heavy and spin/hook input behavior remain unchanged.
12. TEST EVERYTHING passes, then focused Play Mode verifies bomb VFX/friendly fire and both airborne attacks.

 — Required Verification Gates

Actual pass/fail truth belongs in `PROJECT_STATUS.md`.

## Single required Unity QA command

```text
Boredom And Dungeons -> TEST EVERYTHING
```

New automated checks integrate into this command; do not create another mandatory QA action.

## Gate order

1. **Repository/preflight:** required files, stable `.meta`, no real conflict markers, no staged package/cache/duplicate-status artifacts.
2. **Package validation:** syntax, first install, second install for idempotency, validator, `git diff --check`.
3. **Unity compilation:** documented Unity version, no compiler errors, no unexplained compiler warnings, no project-generated red Console error.
4. **Automated QA:** TEST EVERYTHING, zero blockers; warnings fixed or explicitly accepted.
5. **Console cleanliness:** no repeated project-generated warning spam, edit-mode material leak, charged-shot warning, or AudioListener warning.
6. **Focused Play Mode:** changed behavior, repeated use, death/restart/re-entry, nearby-system regression checks.
7. **Performance when relevant:** measure CPU/GPU/memory/GC/draw calls/loading on a representative target.
8. **Documentation truth:** record real results and exact resume point in `PROJECT_STATUS.md`.


## Active V23R8 camera, horse, combat, and UX gate

1. **40/60 composition:** on foot and mounted, the active target remains near viewport Y `0.40` through cardinal/diagonal aim, wall pressure, room handoffs, and snap-to-target.
2. **Legacy composition migration:** an existing scene value that serialized the old 50-degree minimum pitch cannot force the target back to screen center.
3. **Unified horse prompts:** near the horse, every currently valid Mount/Heal/Pet action displays a compact keycap plus clear label; no standalone legacy cue duplicates it.
4. **Mounted stationary prompts:** while mounted and fully stopped, Dismount and conditional Heal are visible; the row hides while moving.
5. **Mounted mouse coherence:** horse facing and forward travel use the same smoothed aim direction, with no raw-cursor slide, small-motion twitch, or sudden turn.
6. **Hook input:** short strong press performs normal heavy; long strong hold performs one hook and no duplicate heavy.
7. **Hook target policy:** the hook applies exactly 2 damage; small regular enemies pull into sword range; large regular enemies, mini-bosses, and bosses do not move.
8. **Hook collision/cooldown:** walls block acquisition; hook cooldown is independent; during hook cooldown normal heavy is attempted only when its own cooldown is ready.
9. **Airborne attack presentation:** airborne light/heavy attacks retain their identity as vertical slashes, suppress the duplicate horizontal slash, and keep existing damage ownership.
10. **Target outline:** exactly one unobstructed enemy inside the currently usable weapon range receives the thin constant-pixel red silhouette outline; invalid, occluded, dead, off-axis, and out-of-range targets do not.
11. **Single owners:** prompts, highlight, hook visuals, and airborne visuals do not become camera, gameplay-input, health, or persistent movement owners.
12. **Retained regressions:** V23R6 handoff stability, first visible New Game frame, closed walls, FOV, holes, combat grounding, charged shot, AudioListener, BBH intro, death restart, and Console cleanliness remain passing.
13. **Art-direction documents:** the completed canonical root `ART_DIRECTION.md`, Unity-side mirror, and reference board remain synchronized and indexed.

## Active V23R9 horse-state and art-direction gate

1. **V23R8 automated baseline:** the run generated at `2026-06-07T17:56:35.5590780Z` passed with 0 blockers, 0 warnings, and 0 info.
2. **On foot near horse:** Mount and Pet show active-key prompts; Heal shows a prompt only when needed and legal.
3. **Mounted stationary:** Dismount shows its prompt; Pet works by key without a prompt; Heal is disabled.
4. **Mounted moving:** Dismount remains key-usable without a prompt; Pet and Heal are disabled; no row is visible.
5. **No heal carry-over:** mounting ends and clears an on-foot healing session.
6. **Mounted Pet stability:** movement, dismount, hazard recovery, combat pressure, damage, pause, or death cancels and restores captured states.
7. **No duplicate cues:** legacy Heal/Pet indicators remain suppressed.
8. **Art direction:** the authoritative document and compact reference board exist with stable `.meta` files and are indexed.
9. **Visual contract:** 65% colorful wonder / 35% mystery, polished stylized fantasy, hand-painted/restrained-PBR materials, fantasy headings, readable body text, Game Boy-inspired icons, and Game Boy menu-shell rules are documented.
10. **True-victory state:** Mother victory persistently restores/awakens the Game Boy presentation without replacing menu/accessibility ownership.
11. **Responsive language:** desktop and landscape mobile share the visual language while adapting layout and control size.
12. **Retained regressions:** V23R8 camera, steering, hook, airborne attacks, highlight, V23R6 camera stability, hazards, combat, intro, and Console remain passing.


## Active V23R19K explicit airborne branch gate

1. Unity compiles and TEST EVERYTHING returns 0 blockers, 0 warnings and 0 info.
2. `BDPlayerMeleeEnhancer` returns explicit airborne identity and owns the body animation only.
3. `BDPlayerCombat` consumes that identity at the committed hit and chooses one visual branch.
4. Grounded Light/Heavy produces one grounded slash.
5. Airborne Light/Heavy produces one `SpawnVertical` slash with no grounded duplicate.
6. No `combat.SuppressNextStandardMeleeVisual` call remains in the enhancer.
7. Boy mounted Light/Heavy still launches neither sword nor hook; future Girl mounted hook remains unimplemented design.
8. The resolved V23R19I compile bug is documented in status/history and is absent from the current open-bug table.
9. `OPENING_DIALOGUE_WORDLESS_CHARACTER_VOICE_HE_V1.md` contains the exact `I’m bored.` future requirement and is not reported as Runtime-complete.
10. Focused Play Mode verifies the actual orientation/placement before closing the reopened airborne bug.

## Active V23R19J semantic QA realignment gate

1. The mounted-intro release-order scanner recognizes `RestoreMountedIntroControls` and still proves release occurs after the full-stop hold.
2. Airborne melee QA validates committed damage/timing in `BDPlayerCombat` and actual presentation in `BDPlayerMeleeEnhancer`.
3. Mounted target QA validates the active horse-height `TargetHighlightOrigin` path.
4. Elite guardian QA validates explicit `ConfigureEliteGuardian()` plus forced-movement-disabled behavior without requiring a new serialized enum value.
5. Hook design retains hit-committed small-enemy pull and Elite guardian immunity while preserving boy-mounted-hook prohibition.
6. Death QA validates `BDCharacterDeathAnimation` through the current V23R19G renderer-branch owner.
7. Bug-ledger QA requires `BUG-V23R19H-001`, not the superseded incorrect `BUG-V23R19G-006` requirement.
8. The corrected Girl/Father/meta specification is marked required future work and contains Mother temporary-summon, phase-4 and temporary-meta-screen requirements.
9. TEST EVERYTHING reports zero blockers/warnings/info before focused Play Mode resumes.
10. No Runtime/scene/prefab change is permitted in this compatibility package.

## Active V23R19I forced-movement compile-compatibility gate

1. `BDCombatantProfile` exposes `ReceivesForcedMovement` and `CanReceiveForcedMovement(BDHealth)`.
2. The compatibility API delegates to the current serialized projectile/forced-movement policy rather than introducing a second mutable field.
3. Unity compiles without CS1061 in `BDKnockbackReceiver` and without CS0117 in `BDPlayerGrapplingHook`.
4. TEST EVERYTHING integrates `BDV23R19ICombatantForcedMovementCompatibilityQA` and reports no V23R19I blocker.
5. Regular enemies without a profile preserve current forced movement; explicit profiles remain authoritative.
6. Battery guardians/large enemies/bosses retain their intended no-pull/no-knockback behavior.
7. Boy mounted hook remains disabled; future Girl mounted hook remains design-only.
8. Rope, climbing and quicksand-swamp work is documented as `REQUIRED / LATER / NOT IMPLEMENTED`.

## Active V23R19H character-specific mounted-hook gate

1. **Boy on foot:** short Heavy and held grappling-hook behavior remain unchanged.
2. **Boy mounted:** Light/Heavy input emits neither sword melee nor grappling hook.
3. **Cooldown integrity:** blocked mounted input does not consume or reset the boy's hook cooldown.
4. **Mounted ranged:** normal mounted shooting remains functional.
5. **No global enable:** Runtime contains no mounted-hook input path for the current boy character.
6. **Girl future contract:** the future Girl character specification explicitly permits mounted hook use through character-specific capability data.
7. **Retained regressions:** airborne presentation, death presentation, abandon flow and mounted intro remain under the V23R19G focused gate.
8. **Automated gate:** TEST EVERYTHING includes `BDV23R19HCharacterMountedHookQA`.

## Active V23R19G focused regression and bug-ledger gate

1. **Airborne orientation:** Light/Heavy reuse the selected grounded arc and rotate it `-90°` around local X; the arc is in front and moves toward the floor.
2. **Player death visibility:** the real player renderer branch visibly falls/squashes; no death overlay or menu obscures the active pose.
3. **Menu timing:** the Game Boy menu opens only after the complete player death animation and readable hold.
4. **Large/Elite death:** large enemies and Battery guardians stop AI/collision, visibly animate, then allow loot/despawn.
5. **Clean abandon:** confirmation reloads the active scene and displays a clean main-menu state, never a popup over the abandoned run.
6. **Mounted replay:** abandon -> menu -> Start Game keeps the exact active-scene rider attached from first visible entrance frame through control release.
7. **Boy mounted combat:** mounted Light/Heavy input launches neither sword melee nor grappling hook and does not consume hook cooldown; on-foot hook behavior remains unchanged.
8. **Bug ledger:** `Assets/_Project/Design/Runtime/OPEN_BUG_TRACKER.md` exists and matches every currently open/reopened/implemented-but-unverified defect.

## Active V23R6 actual-pose handoff and diagnostics gate

1. **Local-state preservation:** keep the current scene edits, four Codex-agent edits, `BDCameraTransitionDiagnostics.cs`, its `.meta`, and all existing diagnostics integration.
2. **No pre-smoothing release:** `ResolveRoomBoundaryConstrainedPosition` must not complete a handoff from an unsmoothed desired position.
3. **Actual camera body:** the two-room union remains active until the final smoothed camera position is inside the new room using the stable camera inset.
4. **Actual look point:** the union remains active until the smoothed constrained look point is inside the new room using the look-point inset.
5. **End-of-frame release:** completion occurs after the final pose write and before the diagnostic capture for that frame.
6. **Diagnostics remain gated:** F8 recording, F9 export, and F10 marker remain optional and inactive unless explicitly used.
7. **Diagnostic completeness:** CSV records room/handoff/target events, desired/contained/final poses and distances, FOV, external camera writers, player/horse roots, visual offsets, and Animator root-motion state.
8. **Walking acceptance:** repeated bidirectional node transitions on foot produce no direction snap, zoom pulse, or forward/back jump.
9. **Mounted acceptance:** repeated full-speed mounted transitions produce no direction snap, zoom pulse, or forward/back jump.
10. **Residual-motion procedure:** if anything remains, capture a short marked CSV before another camera, model, or geometry change.
11. **Retained safety:** closed walls, legal doorway visibility, normal-gameplay FOV, first visible New Game frame, death restart, holes, combat, charged shot, AudioListener, BBH intro, and Console behavior remain passing.
12. **Legacy semantic QA:** V20 regression coverage validates the active V23R6 actual-pose release markers and must not require the superseded `completed union room handoff` state string.
## Active V23R5 first-frame mounted-camera gate

1. **No pre-cinematic leak:** after selecting New Game, no frame shows the camera beside the entrance, attached to the player/horse, or at normal gameplay follow framing.
2. **First visible pose:** the first visible gameplay frame is already the approved higher/farther inside-room shot looking at the entrance.
3. **Ownership order:** `sceneLoaded` disables `BDCameraFollow` and primes camera position/rotation/FOV before the presentation coroutine and before any frame yield.
4. **Fallback safety:** if the mounted intro cannot resolve, normal camera ownership is restored before the black cover is released.
5. **Retained flow:** input remains locked, the horse enters, turns right, fully stops, and only then the normal camera and controls return.
6. **Legacy QA alignment:** V20 checks validate the active V23R4 safety inset, handoff-only wall cast, and V23R6 actual-pose release contract rather than obsolete numeric or state-string anchors.

## Active V23R4 camera micro-jitter gate

1. **Room-center distance:** while standing near the center of a room, rotate the aim through full cardinal and diagonal directions; camera distance and FOV remain visually constant.
2. **Walking wall pressure:** walk parallel to and toward each closed wall. The camera eases against containment without one-frame jumps, repeated micro-zooms, or pitch pulses.
3. **Mounted wall pressure:** repeat at horse speed, including corners and diagonal movement; higher movement speed does not reintroduce jitter.
4. **Look-point stability:** player/horse vertical settling and small floor changes do not make the framing bob or pulse.
5. **Room handoff:** cross legal openings repeatedly in both directions. The physical wall cast is active only during handoff, and the transition preserves distance/FOV without clipping through closed wall segments.
6. **Closed-wall visibility:** the reduced stable inset does not expose adjacent rooms from frontal, side, corner, diagonal, high, or low views.
7. **Performance:** normal follow does not execute repeated scene-wide room scans in one frame; no recurring camera-related GC allocation or spike is visible in the Profiler when checked.
8. **No map regeneration regression:** room, wall, doorway, enemy, hazard, portal, minimap, and authored-scene placements remain unchanged.

## Active V23R3 regression gate

1. **Single camera owner:** `BDCameraFollow` is the only normal-gameplay Main Camera transform owner; the old viewport-bias source is absent.
2. **Stable yaw/pitch:** wall proximity, enemies, damage, and room/tile handoff do not alter mouse sensitivity or gameplay pitch.
3. **Semantic camera QA:** automated checks validate the current rate-limited single-stage camera contract and do not require the obsolete `movementDirectionBlend` field.
4. **Planar shake:** combat shake has no vertical component.
5. **Closed walls:** on foot and mounted, side/corner/diagonal rotation never reveals an adjacent room; the maintained room-boundary design explicitly defines closed walls as a `visibility boundary`.
6. **Room/tile handoff:** cross several legal transitions in both directions. Distance/FOV/pitch remain stable with no snap or zoom.
7. **Combat grounding:** repeated enemy hits near walls and transitions do not move the player below the floor.
8. **Root-safe recovery:** any forced recovery places the CharacterController capsule fully above ground and movable.
9. **Walking-proof hole boundary:** approach every side and corner of the hole using ordinary walking at slow/full speed and diagonal angles; walking never enters.
10. **Swept footprint:** hazard filtering checks intermediate path samples and capsule footprint, not only final center position.
11. **Active intent only:** intentional gap entry is limited to active dodge, actively ascending jump, or explicit forced movement. Recent historical dodge/jump timestamps do not authorize later walking entry.
12. **Post-recovery suppression:** the named `0.85s` suppression contract blocks gap intent and forced-gap requests immediately after recovery; QA does not require obsolete `0.55s` text anchors.
13. **Near-hole respawn:** intentional fall recovery uses a valid local point just outside the same hole before older safe points or spawn.
14. **Respawn safety:** local recovery is outside all hazard volumes, above ground, free of overlap, and does not loop.
15. **Version-agnostic status QA:** active-work validation checks semantic headings and a Runtime version marker rather than a frozen category/version string.
16. **Existing regressions:** charged shot, AudioListener, mounted intro, BBH first frame, current-status QA, and V22R2 Console cleanup remain passing.
17. **Console:** no project-generated red errors or repeated warnings.

## Remote/local synchronization gate

- The remote reference used for this repair is `main` at `40104177ee396d19f375b62a20c410e4ac63bdc8`.
- Remote-only maintained-document improvements are present in the merged working tree.
- Local-only Runtime, scene, QA, `.meta`, package, and Codex work remains present.
- No reset, clean, broad checkout, or old full-file package replacement was used.
- Before push, create a safety branch/tag, commit the Unity-verified merged state, fetch `origin`, merge `origin/main`, rerun diff/hygiene checks, and inspect the merge result.

## Repository-hygiene gate

- Run `python3 tools/check_repository_hygiene.py` before handoff and commit.
- `AGENTS.md` is present as canonical maintained root Markdown and is scanned by TEST EVERYTHING.
- `.codex/config.toml` and `.codex/agents/` remain available as maintained project configuration.
- `AGENTS.rtf` is ignored as a local rich-text duplicate and must not be staged.
- Root Markdown matches `DOCUMENTATION_INDEX.md`.
- No obsolete roadmap/status copy, package README/manifest, repair narrative, chat export, or copied QA report remains tracked.
- Every removed document had valid requirements merged first.
- `PROJECT_STATUS.md` matches the real current state and resume point.

## V23R3B documentation compatibility gate

- `Assets/_Project/Design/Map/ROOM_BOUNDARY_CAMERA_AND_TEXTURE_READINESS.md` contains the exact durable term `visibility boundary`.
- This documentation repair does not replace Runtime or scene verification.
- Rerun TEST EVERYTHING and confirm `ROOM_BOUNDARY_DESIGN_MISSING` is absent.

## Truthful status language

- Static validator passed does not mean Unity compiled.
- Unity compiled does not mean Play Mode passed.
- Automated PASS does not mean the Console is clean.
- One Play Mode pass does not prove restart/re-entry behavior.
- No profiler data means performance remains unverified.
- A task is not DONE until all applicable gates pass.


## Active V23R19D quicksand, airborne, combat-contact, and abandon gate

1. **Quicksand slowdown:** entry movement is visibly slower and decreases progressively toward the half-body threshold; leaving returns movement to normal.
2. **Single application:** player slowdown is neither missing nor applied twice.
3. **Air strike placement:** Light and Heavy slash visuals are fully in front of the player, vertical, square to facing, and move from overhead toward the floor.
4. **No diagonal body roll:** the player body performs a forward/downward chop rather than a sideways rotation.
5. **Enemy contact while jumping:** jump onto an attacking Sword/Patrol/Charger; damage may occur, but the player never teleports to an old safe point.
6. **Abandon confirmation:** Pause -> Abandon opens confirmation; Cancel/Escape preserves the run; only explicit Yes returns to the main menu.
7. **Abandon replay:** after confirmed abandon, Start Game plays the mounted entrance with the exact player visibly attached to the horse throughout.
8. **Retained gates:** V23R19B hook/guardian behavior, V23R18B mounted hazards, camera, Parry, bombs, and Console cleanliness remain passing.

## Active V23R19C semantic QA compatibility gate

1. `BDV23R8CombatUxQA` validates the centralized `IsSmallRegularEnemy` and forced-movement policy rather than requiring a direct enum reference in the hook.
2. `BDV23R11BombAirborneAudioQA` validates committed airborne identity/body animation in `BDPlayerMeleeEnhancer`.
3. The same QA validates `BDMeleeSlashArcVisual.SpawnVertical` in `BDPlayerCombat`, the actual V23R19 presentation owner.
4. No runtime token, dead branch, duplicate visual call, or obsolete API is introduced merely to satisfy static text.
5. Rerun TEST EVERYTHING and confirm both reported blocker codes are absent.

## Future C06 merchant-shop and run-economy gate — NOT ACTIVE

1. Generate 2–4 shops per run with at least one initially-empty-room placement and one post-clear placement.
2. Present exactly three distinct weighted offers with prices inside the approved inclusive ranges.
3. Require both elapsed-time and progressed-room thresholds before automatic refresh; refill only purchased/empty slots and preserve occupied offers.
4. Validate one fixed-cost atomic full reroll that replaces all three offers and respects rarity/unique run history.
5. Give unique items lower selection weight and permit at most one appearance per run across all merchants/refreshes/rerolls.
6. Validate Friendly, HostileAlive, and Defeated run-global merchant states, including no stock/commerce for hostile encounters and no future appearance/refresh/reroll after defeat.
7. Validate aggression thresholds, transformation, forward shadow, cannon 45-degree rows, movement while firing, fast high-damage light sword, dodge, and no simultaneous cannon+sword action.
8. Validate free remaining rug stock on friendly defeat plus the exclusive one-of-two light-sword/cannon reward and matching model/damage-channel update.
9. Validate enemy 20% / 3–8 and breakable 30% / 2–10 currency rules.
10. Validate atomic purchases, sold stock, insufficient funds, wallet UI, new-run reset, and no duplicate rewards.
11. Validate player/horse protection bars and mounted protected-damage/no-buck routing.
12. Validate every percentage upgrade against the real owning runtime system.

## Future C06.META cross-run progression gate — NOT ACTIVE

1. Use an approved deterministic end-of-run formula with an understandable award breakdown.
2. Keep persistent meta points separate from run-shop currency.
3. Validate atomic spending/unlocking, save/load, migration, duplicate protection, and non-negative balances.
4. Verify every cosmetic/gameplay unlock against its real content owner.
5. Define and test defeat, victory, abandon, crash recovery, and partial-run award policy.
6. Finalize the feature name, menu UX/art, catalog, costs, pacing, save schema, and anti-farming rules before implementation is marked complete.

## Active V23R19B scene-safe hook and Battery-guardian gate

1. Scene installation updates only `jumpTravelMultiplier`, `jumpTravelBoostDuration`, Wall Jump reach/probe values, and `dashDistance` inside the single player-controller YAML block.
2. Quicksand-owned nonlethal jump recovery, longer normal jump/dodge, universal solid Wall Jump, and explicit airborne Light/Heavy presentation remain present.
3. Hook small-enemy eligibility is re-evaluated at impact through the canonical small-regular policy.
4. A living small regular hit cannot remain damage-only because of helper colliders, visual bounds, or child-health/root-locomotion separation.
5. Hook stagger and knockback cleanup resolve on the actual movement root.
6. Collectible guardians are built inactive and activated by root `SetActive(true)` only after final positioning.
7. Guardians include CharacterController, `BDHealth`, an AI archetype, grounding/stability, tactical components, and an Elite `BDCombatantProfile`.
8. Battery guardians are damageable and killable but reject grappling pull, knockback, mounted-impact displacement, and forced hazard entry.
9. Repeat Battery A and Battery B triggers after a fresh run and verify no inert or invulnerable guardian.
10. Unity compilation, TEST EVERYTHING, focused Play Mode, Console cleanliness, and restart/re-entry are required.

## Active V23R19 quicksand, traversal and airborne-identity gate

1. Jump repeatedly while holding each movement direction in quicksand; no safe-point teleport occurs before failure depth.
2. Verify progressive slowdown, 2 HP grounded ticks, jump extraction, dodge pause and true failure respawn.
3. Compare normal jump travel and dodge reach with prior behavior; both are modestly farther and remain controllable.
4. Wall jump from a room wall, hard prop, small enemy and horse; each launches away with the longer arc.
5. Wall jump does not trigger from floor, ceiling, slope or trigger-only hazard volume.
6. Jump then Light/Heavy: dedicated body motion plus vertical slash only; no regular horizontal attack animation/arc.
7. Ground Light/Heavy remain unchanged.
8. Retest hole boundaries, quicksand, mounted hazard recovery, camera and Console.
