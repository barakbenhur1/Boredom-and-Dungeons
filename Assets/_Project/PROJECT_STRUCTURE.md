# Boredom & Dungeons — Project Structure

Status date: **2026-06-06**

This document defines folder ownership under `Assets/_Project`. Current progress and requirements remain authoritative only in `/ProjectGuide/Status/CURRENT.md`.

## Runtime scripts

```text
Scripts/Runtime/
  Audio/
  Bosses/
  Cinematics/
  Collectibles/
  Combat/
  Common/
  Encounters/
  Enemies/
  Environment/
    NaturalMap/
  Hazards/
  Horse/
  MiniBosses/
  Persistence/
  Player/
  Rendering/
  UI/
  VFX/
```

Rules:

- Runtime scripts must not depend on `UnityEditor`.
- Shared gameplay code belongs in the appropriate runtime system folder, not in editor scene builders.
- Boss logic belongs under `Bosses` or `MiniBosses`.
- Encounter logic belongs under `Encounters`.
- Hazard detection and recovery belong under `Hazards` when separated from actor controllers.
- Rendering policy belongs under `Rendering` and does not own gameplay state.
- UI flow, settings, boot intro, menu, and HUD behavior belong under `UI`.
- Older root-level runtime files may remain during migration; move them only through tested refactors that preserve Unity references.

## Editor scripts

```text
Scripts/Editor/
  BossesMiniBosses/
  BossesStage16/
  BossesStage17/
  CombatProfiles/
  Factories/
  SceneBuilders/
  Tools/
  Validation/
```

Rules:

- Editor scripts may use `UnityEditor`.
- Scene generation and repair remain editor-only.
- Editor factories remain under `Scripts/Editor`.
- `Boredom And Dungeons -> TEST EVERYTHING` is the one required QA entry point; new validators must be integrated into it.
- Scene installers must be safe to run repeatedly and must not create duplicate systems.

## Art

```text
Art/
  Materials/
    Bosses/
    Characters/
    Collectibles/
    Environment/
    Horse/
    VFX/
    Weapons/
  Textures/
    Bosses/
    Characters/
    Collectibles/
    Environment/
    Horse/
    Weapons/
  Prefabs/
    Bosses/
    Characters/
    Cinematics/
    Collectibles/
    Enemies/
    Environment/
    Horse/
    VFX/
    Weapons/
  Models/
    Bosses/
    Characters/
    Enemies/
    Horse/
    Props/
```

Rules:

- Procedural prototype objects should become reusable prefabs where appropriate.
- Runtime-created materials should become reusable material assets before release.
- Final player, horse, enemy, boss, collectible, weapon, and prop assets belong here.
- Temporary facing markers are removed only when final models make front/back direction clear.

## Audio

```text
Audio/
  Ambience/
  Music/
  SFX/
    Bosses/
    Cinematics/
    Collectibles/
    Combat/
    Horse/
    Movement/
    UI/
```

Audio assets are grouped by purpose and must respect the persistent master/music/SFX settings flow.

## Scenes and prefabs

```text
Scenes/
Prefabs/
  Enemies/
    Regular/
```

Current prototype scene:

```text
Assets/_Project/Scenes/02_CleanCore_MazePrototype.unity
```

The editor scene builder may regenerate this scene. A normal run reloads the existing generated scene and does not create a new runtime map seed.

## Design documentation

```text
Design/
  ArtDirection/
  Audio/
  Bosses/
  Cinematics/
  Collectibles/
  Combat/
  Encounters/
  Enemies/
  Horse/
  Level/
  Map/
  Movement/
  QA/
  Rendering/
  Roadmap/
  UI/
  VFX/
```

Document ownership:

- `/ProjectGuide/Status/CURRENT.md` — only complete authoritative project plan and status.
- `/README.md` — current overview and onboarding.
- `/ProjectGuide/INDEX.md` — current/canonical/historical document map.
- `Design/**` — detailed system specifications.
- `Design/QA/**` — historical stage reports.
- `Design/Roadmap/MASTER_REMAINING_WORK_ROADMAP_V128.md` — historical only.

Rules:

- A versioned design document is not the live implementation queue.
- Old QA reports do not prove the current branch still passes.
- Remove contradictory superseded documents only after their valid decisions are preserved in the canonical source.
- Do not add duplicate current-status files.

## Repository hygiene

Do not commit Unity caches, ZIP packages, extracted package payloads, one-shot patch tools, local QA output, chat exports, copied status snapshots, or accidental terminal output.

Unity `.meta` files for committed assets are required and must remain stable.
