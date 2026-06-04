# Boredom & Dungeons — Project Structure

This document defines the intended production structure under `Assets/_Project`.

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
  Horse/
  MiniBosses/
  Persistence/
  Player/
  UI/
  VFX/
```

Rules:
- Runtime scripts must not depend on `UnityEditor`.
- Runtime scripts must be safe in Play Mode and builds.
- Shared gameplay code belongs in `Common`, not in random scene-builder files.
- Future boss systems should go under `Bosses` and `MiniBosses`.
- Future encounter logic should go under `Encounters`.

## Editor scripts

```text
Scripts/Editor/
  Factories/
  SceneBuilders/
  Tools/
  Validation/
```

Rules:
- Editor scripts may use `UnityEditor`.
- Scene generation tools stay in Editor only.
- Factories used only by editor generation stay in Editor/Factories.
- Runtime factories must be separate and must not use UnityEditor.

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
  Models/
    Bosses/
    Characters/
    Enemies/
    Horse/
    Props/
```

Rules:
- Procedural prototype objects should eventually become prefabs.
- Runtime-created materials should eventually become asset materials.
- Final Game Boy, batteries, cartridge, horse, enemies and bosses belong here.

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

Rules:
- Audio clips should be grouped by purpose.
- Placeholder procedural audio should be replaced with clips later.

## Design docs

```text
Design/
  ArtDirection/
  Audio/
  Bosses/
  Cinematics/
  Collectibles/
  Combat/
  Enemies/
  Level/
  QA/
  UI/
```

Rules:
- Every major system should have a design note.
- Boss and mini-boss designs should be documented before implementation.
- QA reports should live in `Design/QA`.
