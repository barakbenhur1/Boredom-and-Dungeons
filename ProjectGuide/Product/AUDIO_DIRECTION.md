<!-- BND_TUTORIAL_OPENING_POLISH_V10113:BEGIN -->
## V10.11.3 authored mother-call prototype cue

The pre-walk `honey come here a second` bubble uses one short feminine nonverbal vocal texture synchronized to bubble entry. It is non-spatial, moderate in level, deterministic, stops on skip and never continues into walking. This is a scoped cinematic cue and does not falsely complete the future reusable wordless-character-voice system.
<!-- BND_TUTORIAL_OPENING_POLISH_V10113:END -->

# Boredom & Dungeons — Canonical Music and Audio Direction

> **Canonical root source, organized under ProjectGuide.** Every music, ambience, SFX, voice, mixer, mastering, transition and audio-accessibility change must read this file.

## Core audio promise

The audio must feel like a colorful fantasy adventure seen through a child's imagination: inviting and playful when exploring, exciting and readable in combat, and emotionally surprising in major encounters. It must support the established **65% colorful wonder / 35% mystery and danger** visual balance without becoming childish, noisy, or exhausting.

Music intensity must come from arrangement, rhythm, orchestration, harmony, and controlled density — not from making every later state simply louder.

## Music-state hierarchy

### 1. Exploration / no active combat

- Cheerful nature-and-fantasy music.
- Warm, curious, lightly adventurous, and loopable for long sessions.
- Preferred palette: acoustic strings, light flutes/woodwinds, pizzicato, hand percussion, bells/celesta, marimba-like colors, soft pads, gentle harp/guitar, and subtle natural textures.
- Avoid constant heroic climax, comedy music, nursery-song clichés, and dense percussion.
- Ambience remains audible: wind, leaves, birds, water, insects, distant magical tones, room/cave tone, and location-specific details.

### 2. Standard combat

- More rhythmic and urgent while preserving the exploration theme or tonal identity.
- Add a clear pulse, stronger percussion, bass movement, ostinato, and sharper accents.
- Keep attacks, telegraphs, parry, damage, horse cues, and hazards readable above the music.
- Enter and exit combat through bar-aligned transitions or compatible stems; do not hard-cut between unrelated tracks.

### 3. Mini-boss combat

- Faster and more forceful than standard combat.
- Add stronger drums, low strings/brass or stylized synth weight, more active bass, and short encounter stingers.
- The mini-boss state must feel meaningfully elevated without spending the full final-boss intensity budget.

### 4. Boss combat

- Very high rhythmic energy with a fantasy-rock / heavy-rock hybrid.
- Heavy guitars and powerful drums may combine with orchestral/fantasy colors, magical textures, and the boss's motif.
- Use occasional **single-word vocal stabs** or very short shouts to raise adrenaline. They are punctuation, not full lyrics, and must not mask instructions, telegraphs, or dialogue.
- Vocal words remain original, licensable, sparse, localization-aware, and appropriate to the game's age rating.
- Boss intensity is created by arrangement and impact while remaining inside the same master loudness ceiling as the rest of the game.

### 5. Mother boss — child-song transformation

Mother uses a deliberately pleasant children's-song / lullaby identity rather than ordinary boss rock. The emotional tension comes from the arrangement becoming progressively more urgent while the underlying melody remains recognizable.

- **Phase 1:** gentle music-box, celesta, soft humming-like timbre, simple acoustic accompaniment, comfortable tempo.
- **Phase 2:** add pulse, pizzicato movement, more active harmony, and restrained percussion.
- **Phase 3:** increase tempo and density, add stronger drums and harmonic pressure while retaining the child-song motif.
- **Phase 4:** highest tempo and intensity. Add a synchronized **tick-tock stem** behind the music. The ticking is a musical layer, not a loose one-shot SFX, so it stays phase-locked and mixes predictably.
- Phase transitions happen on musical boundaries with authored risers/stingers; the melody must not restart abruptly at every phase.
- The contrast should feel emotionally unsettling and memorable, not like generic horror noise.

## Music implementation architecture

Future implementation under retained work item `C12.42` uses a dedicated music director and an explicit AudioMixer rather than volume changes spread across unrelated scripts.

Recommended runtime owners:

- `BDMusicDirector` — one music-state and transition owner.
- `BDMusicState` — Exploration, Combat, MiniBoss, Boss, MotherPhase1, MotherPhase2, MotherPhase3, MotherPhase4, Menu, Death, Victory, and Cinematic.
- stem sets with shared BPM/key/loop metadata;
- bar/beat-aware transition scheduling;
- short hysteresis before leaving combat so rapid aggro changes do not flutter the soundtrack;
- scene/run reset restores a known state and stops orphaned stems.

## Required AudioMixer routing

```text
Master
├── Music
│   ├── Exploration
│   ├── Combat
│   ├── Boss
│   ├── MotherMusic
│   ├── MotherTick
│   └── VocalStabs
├── SFX
│   ├── Combat
│   ├── Movement
│   ├── Horse
│   ├── Hazards
│   ├── UI
│   ├── Collectibles
│   └── Cinematics
├── Ambience
│   ├── Nature
│   ├── Interior
│   └── Magic
└── Voice
    ├── Dialogue
    └── Efforts
```

- `MotherTick` is routed inside Music for synchronization and snapshot control, but remains a separate stem for phase-4 balancing.
- UI is never routed through world-distance attenuation.
- World SFX use intentional spatial blend, distance curves, voice limits, and priority.
- The existing Master, Music, and SFX settings must eventually control mixer parameters rather than ad-hoc per-source multiplication.
- Ambience receives its own user-facing control when the settings design is expanded; until then it follows the documented fallback without bypassing Master.

## Mix and mastering targets

These are project targets, not permission to normalize every clip independently:

- Master true peak must remain at or below **-1 dBTP**.
- Preserve at least **6 dB of practical headroom** during normal implementation and content review.
- Exploration, combat, mini-boss, and boss states should feel progressively denser but remain within roughly the same perceived master loudness ceiling.
- Typical full-gameplay loudness target: approximately **-18 to -14 LUFS short-term**, evaluated in real scenes rather than on isolated music files.
- Boss music may feel more intense but should not exceed normal gameplay by more than about **1.5 LU** without an explicit cinematic reason.
- Dialogue, critical one-word vocal stabs, parry confirmation, danger telegraphs, and accessibility cues may duck Music briefly by roughly **1.5–3 dB** using smooth attack/release.
- Combat may duck Ambience by roughly **2–4 dB**, never mute it abruptly.
- Avoid brick-wall limiting, constant low-frequency saturation, clipping, and frequency masking between guitars, drums, explosions, horse movement, and warning cues.

## Snapshots and transitions

Required mixer snapshots:

- Exploration
- Combat
- MiniBoss
- Boss
- MotherPhase1
- MotherPhase2
- MotherPhase3
- MotherPhase4
- Menu/Pause
- Death
- Victory/Cinematic

Rules:

- Transition on the next compatible beat/bar when possible.
- Crossfade compatible stems; use authored stingers when harmony or tempo changes.
- Pause/menu snapshots lower density and world SFX without destroying playback state unless the flow explicitly restarts music.
- Death and victory own deliberate tails; no accidental immediate stop.
- Snapshot restoration is gradual and deterministic.

## SFX and music relationship

- Sword wind-ups/impacts, ranged fire/impacts, breakables, bombs, hook travel/hits, parry phases, jump/dodge/landing, hole/lava/quicksand states, horse movement/vocalizations, UI buttons, intro cues, damage, warnings, and boss telegraphs remain intelligible at all music intensities.
- Important gameplay cues receive priority over decorative particles or ambience.
- Bomb explosions require a visible explosion and a synchronized low/mid impact sound; they must not rely on screen shake alone.
- Repeated rapid sounds use small controlled pitch/variation pools rather than identical machine-gun repetition.
- Mobile speakers, headphones, and common laptop speakers are all part of acceptance testing.

## Complete sound-event coverage

The following matrix is a **minimum coverage model, not an exclusive list**. Every player action, enemy action, world reaction, hazard, state transition, UI action, and narrative beat that benefits from audible feedback must receive an intentional sound decision: authored sound, deliberate silence, ambience, music, or a combination. New gameplay is not complete merely because it functions visually.

### Player combat and weapons

- sword light/heavy wind-up, whoosh, impact, blocked impact, miss, airborne vertical slash, spin attack, charged action, and cooldown denial;
- ranged weapon fire, projectile travel when useful, impact by surface/material, enemy impact, empty/reload/ready cues, and projectile expiry where readability needs it;
- grappling hook launch, rope/chain travel, wall miss, enemy catch, 2-damage impact, pull tension, release, resisted-large-enemy hit, and cooldown fallback;
- parry anticipation, success punctuation, frozen moment, gradual release, failed/late attempt when feedback is useful, and reward-state variation;
- player damage, low-health warning, healing, death, respawn/recovery, temporary invulnerability, and status changes.

### Enemy combat

- attack anticipation and telegraph families by archetype;
- sword swings, charger acceleration/impact, ranged shots, trap placement, bomb fuse/explosion, jumper takeoff/landing, boss and mini-boss signature attacks;
- damage reaction, stagger, knockback, armor/block, death, spawn/arrival, alert/aggro, lost-target, and recovery;
- enemy sound density uses priority and voice limits so groups remain readable instead of becoming noise.

### Movement and physical actions

- footsteps by material and movement state, including ordinary ground, stone, wood, mud, shallow water, lava-edge danger, and quicksand;
- jump takeoff, airborne movement only when useful, landing by weight/surface, dodge/dash, skid, hard stop, collision, fall, and hole/chasm descent/recovery;
- breakable objects: crack, final break, fragments, debris landing, crate/pot/wood/stone material variations, and reward reveal;
- doors, gates, portals, switches, mechanisms, pickups, chests, collectibles, and environmental reactions.

### Hazards

- hole/chasm edge cue when appropriate, fall wind, impact/recovery return;
- lava contact, burn layer, bounce/recovery, and persistent environmental lava bed;
- quicksand entry, sticky step/struggle, progressive sink, full-submerge failure, safe recovery, and escape/release;
- bombs, traps, spikes, toxic pools, collapsing surfaces, and future hazards each require telegraph, active loop only when justified, impact, and end-state cues.

### Horse and mounted play

- mount, dismount, saddle/gear movement, rider landing, walk/trot/gallop hoof families by surface, turn/skid, stop, jump, landing, buck, flee, return, injury, healing, pet interaction, and faint/recovery;
- horse vocalizations include calm breaths, snort, effort, warning, pain, fear, affection, fainting, and recovery, with cooldowns and variation so they never spam;
- mounted combat must preserve horse readability without masking weapon, enemy, or hazard cues.

### UI, Game Boy shell, menus, intro, and narrative

- dedicated menu music or a compatible menu arrangement;
- Game Boy boot, cartridge/memory-ready, selection move, button hover/focus where relevant, confirm, back, disabled/error, settings adjustment, tab/page movement, pause, unpause, results, and unlock;
- intro/cinematic ambience, stingers, title reveal, transition in/out, skip, and handoff into gameplay;
- Mother true-victory Game Boy transformation receives a unique persistent boot/awakening identity;
- subtitles, dialogue, voice efforts, one-word boss vocal stabs, and accessibility cues receive their own priority and ducking rules.

### World ambience

- location beds for forest, ruins, caves, interiors, magical areas, water, wind, wildlife, distant mechanisms, fire, crowds, and weather;
- ambience must support orientation and mood without pretending to be a gameplay telegraph;
- one-shots are spatially distributed with controlled randomness, minimum repeat times, and biome ownership.

## Sound-event implementation rules

- Every sound has one clear runtime owner; duplicate scripts must not play the same event.
- Event names should describe the action and result, not the implementation function that happened to trigger them.
- Repeated sounds use variation pools, small pitch/volume ranges, and cooldowns; critical cues keep stable pitch identity.
- World sounds use spatial blend, distance curves, occlusion policy, priority, and voice limits appropriate to gameplay importance.
- UI and menu sounds are non-spatial and remain consistent across scenes.
- Loops have explicit start, sustain, release, interruption, pause, death, scene-change, and object-destruction behavior. Orphaned loops are blockers.
- Slow motion, pause, Parry freeze, and time-scale changes define whether each sound follows scaled time, unscaled time, pitch shifting, or snapshot treatment.
- Important actions are reviewed in context with music, ambience, multiple enemies, horse movement, and mobile speakers — not only in isolated preview.
- Generated prototype tones are temporary readability placeholders and must be replaced or explicitly approved before release.

## Menu and intro music

- Main menu uses a memorable, compact fantasy theme presented through the Game Boy device identity: warm, curious, slightly mysterious, and less rhythmically dense than combat.
- Pause may retain the current music through a filtered/ducked snapshot rather than restarting the menu theme.
- Intro music and sound design own boot/device cues, title reveal, cinematic accents, environmental bed, and a musically compatible transition into exploration.
- Death, restart, victory, true Mother victory, and post-victory device awakening each receive authored music/state behavior rather than accidental continuation or abrupt silence.

## Asset and metadata contract

Every music asset records:

- state and encounter owner;
- BPM, time signature, musical key/mode;
- exact loop start/end and tail behavior;
- stem names and phase compatibility;
- intended mixer group and snapshot;
- import compression/load type by platform;
- licensing/source/editable project location;
- loudness/peak measurement notes;
- whether a vocal sample needs localization or content review.

Naming examples:

```text
MUS_Exploration_Forest_A_Main
MUS_Combat_Forest_A_Percussion
MUS_MiniBoss_A_Full
MUS_Boss_A_Guitars
MUS_Mother_Motif_Core
MUS_Mother_Phase4_Tick
STG_Boss_Enter_A
VOC_Boss_Stab_Run_01
```

## QA and acceptance

1. No hard cut when ordinary combat starts or ends.
2. Rapid enemy aggro changes do not flutter between exploration and combat.
3. Standard combat, mini-boss, and boss each have a clearly different intensity tier.
4. Boss vocal stabs are sparse, intelligible, original/licensed, and never mask gameplay information.
5. Mother phases retain one recognizable child-song motif while tempo/arrangement escalate.
6. Mother phase 4 tick-tock is synchronized and independently mixable.
7. Master, Music, SFX, Ambience, and Voice routing never bypasses Master.
8. No clipping, pumping, excessive limiting, or frequency masking in representative scenes.
9. Settings changes remain stable across scene reload, death, new run, and device restart.
10. Desktop, mobile landscape, headphones, laptop speakers, and phone speakers remain readable.

## Current implementation status

This document is **complete and active as direction**. - Full SFX asset coverage and event routing are part of the same retained C12.42 implementation scope; this document now defines the required coverage rather than a small example subset.

Full dynamic music, stem scheduling, snapshots, mixer routing, and mastering implementation remains ordered under `C12.42`; it is not falsely marked implemented by this document alone.

<!-- B&D MODERN 3D HANDHELD AUDIO START -->
## Upright 3D handheld tactile UI audio

The approved physical menu device requires restrained tactile audio synchronized to the visible 3D control response.

- D-pad navigation uses one short, clean device click with controlled repeat behavior.
- A/Confirm uses a slightly firmer press sound.
- B/Back uses a softer return/cancel sound.
- Main Menu B opens Settings and uses the settings-page cue.
- Main Menu A opens Progression and uses the progression-page cue; Y opens Credits; center SELECT/EXIT use distinct tactile click/confirmation cues.
- Mouse clicks on screen items or physical-device controls use the same semantic action sound as the equivalent hardware input.
- Button press animation and sound begin together; no delayed double-click layer.
- Held navigation must not create uncontrolled sound spam.
- Haptics, when supported, respect user settings and are supplementary rather than required for meaning.
- Route all sounds through the existing UI/SFX mixer ownership; do not create a second audio system for the device.
- The visual reference package is not evidence that final sound assets exist.
<!-- B&D MODERN 3D HANDHELD AUDIO END -->
