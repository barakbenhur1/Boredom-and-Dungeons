# Architecture — Stable System Map

This document explains durable ownership and integration boundaries. It does not track current task status; `PROJECT_STATUS.md` owns that information.

## High-level layers

```mermaid
flowchart TB
    Input[Desktop and future mobile input] --> Player[Player runtime systems]
    Input --> Horse[Horse runtime systems]
    Player --> Combat[Combat, health, hazards, feedback]
    Horse --> Combat
    Player --> Camera[Camera and minimap presentation]
    Horse --> Camera
    Flow[Run/menu/result flow] --> Player
    Flow --> Horse
    Flow --> Camera
    Scene[Authoritative Unity scene and generated content] --> Player
    Scene --> Horse
    Scene --> Combat
    Scene --> Camera
    Editor[Editor installers and scene builders] --> Scene
    QA[TEST EVERYTHING] --> Editor
    QA --> RuntimeContracts[Runtime/source/scene/documentation contracts]
    Design[Design specifications] --> Status[PROJECT_STATUS]
    Status --> Editor
    Status --> RuntimeContracts
```

## Runtime ownership

### Player and combat

- `BDPlayerController` owns player movement/input-facing runtime behavior.
- Combat behavior is split into dedicated combat components rather than accumulated into menu or scene-builder classes.
- Health, hazard recovery, attack feedback, and state reset must remain explicit and independently testable.

### Horse

- `BDHorseController` owns the real mounted/unmounted state and rider placement contract.
- Horse health, hazard safety, flee behavior, recovery, and interactions remain separate components where practical.
- Cinematic or run-presentation systems must call explicit horse APIs rather than duplicating mount state.

### Camera and minimap

- `BDCameraFollow` owns runtime camera target selection, follow composition, and camera collision/boundary behavior.
- `BDMazeMinimap` owns map presentation, discovery, cardinal rotation, clipping, and map markers.
- Camera and minimap may consume player/horse state but must not own gameplay state.

### Run, menu, pause, and result flow

- `BDMainMenuFlow` remains the single UI owner for main-menu, settings, pause, and loading overlays.
- `BDGameFlowSignals` and completion markers route death/result/cinematic transitions without creating parallel menu controllers.
- Run-presentation components may coordinate temporary locks and authored entrance/exit presentation, but must release control back to existing gameplay owners.

## Editor ownership

- Runtime code under `Assets/_Project/Scripts/Runtime` must not depend on `UnityEditor`.
- Editor installers/builders under `Assets/_Project/Scripts/Editor` may configure authoritative scenes and components.
- Nested installers mark scenes dirty; the top-level QA/install flow owns final saving when documented.
- Unity `.meta` GUIDs are stable project data and must remain synchronized.

## QA ownership

There is one required entry point:

```text
Boredom And Dungeons -> TEST EVERYTHING
```

`BDOneClickQAWindow` orchestrates the checks. Domain-specific QA belongs in focused `BD*QA.cs` classes exposing `Scan(BDOneClickQAResult result)` and is integrated into the single entry point.

## Run-flow diagram

```mermaid
stateDiagram-v2
    [*] --> BootIntro
    BootIntro --> MainMenu
    MainMenu --> FreshRun: START GAME
    FreshRun --> Gameplay: entrance presentation releases control
    Gameplay --> Pause: Escape
    Pause --> Gameplay: Resume
    Pause --> MainMenu: Abandon/return
    Gameplay --> MainMenu: ordinary death
    MainMenu --> DeathRestart: START GAME after death
    DeathRestart --> Gameplay: on foot at spawn
    Gameplay --> EndingSequence: authored exit/result trigger
    EndingSequence --> MainMenu: sequence completes
```

## Change rules

Update this document when any of the following changes:

- ownership moves between systems;
- a new major runtime/editor layer is introduced;
- scene generation or installer ownership changes;
- a new persistent data boundary is introduced;
- a parallel controller is consolidated or removed;
- run/menu/result flow changes structurally;
- performance strategy changes system boundaries.

Minor tuning values and current progress remain in design files and `PROJECT_STATUS.md`, not here.
