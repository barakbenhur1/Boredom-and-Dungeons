# Boredom & Dungeons

<!-- B&D MANDATORY START HERE V8 START -->
> [!IMPORTANT]
> **Codex and AI assistants:** read [`AGENTS.md`](AGENTS.md) first, then
> [`START_HERE.md`](START_HERE.md).
>
> **Human contributors:** begin with [`START_HERE.md`](START_HERE.md).
>
> Then read `DEVELOPMENT_WORKFLOW.md`, `PROJECT_STATUS.md`, and
> `DOCUMENTATION_INDEX.md` before proposing or applying a material change.
<!-- B&D MANDATORY START HERE V8 END -->

**Boredom & Dungeons** is a Unity / C# top-down 2.5D action-adventure project about exploration, melee and ranged combat, horse traversal, hidden collectibles, protected encounters, mini-bosses, bosses, and multiple ending paths.

## Authoritative project state

The complete and only authoritative source for the full ordered development list, current requirements, implementation status, verification status, exact current development step, and category transitions is:

```text
PROJECT_STATUS.md
```

This README intentionally does not duplicate current progress or the active task, so it cannot become a second conflicting status source.

<!-- B&D DEVELOPMENT WORKFLOW LINK START -->
## Mandatory development workflow

The exact required working method for changes, ZIP delivery, verification,
near-spawn test harnesses, temporary-test cleanup, documentation synchronization,
and Git handoff is:

```text
DEVELOPMENT_WORKFLOW.md
```

`PROJECT_STATUS.md` remains the only authoritative product progress/requirement
list. `DEVELOPMENT_WORKFLOW.md` defines the process and does not duplicate status.
<!-- B&D DEVELOPMENT WORKFLOW LINK END -->

## Engine and target

```text
Engine: Unity 6000.0.76f1
Language: C#
Current development controls: keyboard and mouse
Final product target: mobile landscape
```

## Codex project configuration

```text
AGENTS.md
.codex/config.toml
.codex/agents/*.toml
```

`AGENTS.md` is the canonical project-wide Codex/AI operating contract. The
`.codex/` directory contains the maintained project configuration and specialist
agent profiles. Local rich-text copies such as `AGENTS.rtf` are not repository
documents.

## Canonical art and interface direction

```text
ART_DIRECTION.md
```

`ART_DIRECTION.md` is the mandatory root source for art, UI, VFX, models,
materials, textures, animation feel, typography, icons, Game Boy menu treatment,
and desktop/mobile presentation. The document under `Assets/_Project/Design/Visual/`
is a synchronized Unity-side mirror, not a competing source.

## Canonical music and audio direction

```text
AUDIO_DIRECTION.md
```

`AUDIO_DIRECTION.md` is the mandatory root source for exploration/combat/boss/Mother music, stems, transitions, AudioMixer routing, snapshots, SFX relationship, mastering, loudness, and audio QA. The asset-side file under `Assets/_Project/Design/Audio/` is a synchronized mirror.

## Main project locations

```text
Assets/_Project/Scripts/Runtime
Assets/_Project/Scripts/Editor
Assets/_Project/Design
Assets/_Project/Art
Assets/_Project/Audio
Assets/_Project/Scenes
Packages
ProjectSettings
```

Runtime code must not depend on `UnityEditor`. Editor-only tools belong under `Assets/_Project/Scripts/Editor`.

## Current prototype scene

```text
Assets/_Project/Scenes/02_CleanCore_MazePrototype.unity
```

The editor scene builder is:

```text
Boredom And Dungeons -> Create Clean Maze Prototype Scene
```

Regenerating the scene can change generated maze and enemy placement. Restarting a normal run reloads the existing generated scene and does not regenerate it.

## QA

There is one required Unity QA entry point:

```text
Boredom And Dungeons -> TEST EVERYTHING
```

New automated checks must be integrated into this command rather than creating another required QA action.

A gameplay change is not marked DONE until Unity compiles cleanly, TEST EVERYTHING passes, Play Mode behavior is verified, and the real result is recorded in `PROJECT_STATUS.md`.

## Repository hygiene

Commit source, assets, required Unity `.meta` files, and the synchronized authoritative documentation.

Do not commit Unity caches, builds, ZIP packages, extracted package tools, local QA output, chat exports, copied status files, accidental terminal output, or duplicate progress documents.

<!-- B&D MODERN HANDHELD SPEC ENTRY START -->
## Current approved menu-device specification

The approved upright 3D handheld asset and interaction contract is documented at:

`Assets/_Project/Design/UI/MODERN_HANDHELD_3D_ASSET_AND_INTERACTIVE_UI_SPEC_V1.md`

Visual references are under `Assets/_Project/Design/Visual/References/ModernHandheld3D/`. Current implementation/status truth remains in `PROJECT_STATUS.md`.
<!-- B&D MODERN HANDHELD SPEC ENTRY END -->
