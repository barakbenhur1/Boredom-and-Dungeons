# Stage 03 / 36 — Scene Builder Decomposition Report

## Goal

Begin decomposing the large scene builder safely.

## Done

- Made `BDCreateCleanMazePrototypeScene` partial.
- Added `BDCreateCleanMazePrototypeScene.StageInfo.cs`.
- Added safe editor utility scaffolds:
  - `BDSceneBuilderObjectUtility`
  - `BDSceneBuilderMaterialUtility`
- Added runtime/editor dependency validator:
  - `BDRuntimeEditorDependencyValidator`
- Added decomposition plan:
  - `SCENE_BUILDER_DECOMPOSITION_PLAN_V115.md`

## Not changed

```text
Gameplay
Movement
Mouse attacks
Mounted shooting
Camera
Map layout
Collectible placement
Enemy placement
Ammo HUD
Cinematics
```

## Required QA

- Compile.
- Run `Create Clean Maze Prototype Scene`.
- Enter Play Mode.
- Verify V113 attack behavior:
  - on-foot attacks rotate player toward mouse
  - mounted shots aim at mouse but do not rotate horse
- Verify no Runtime UnityEditor references with validation menu.

## Progress

```text
Current stage: 3 / 36
Completed if QA passes: 3 / 36
Remaining: 33 / 36
Progress: 8.3%
```

## Next stage

Stage 4 / 36 — Inventory state full expansion for Game Cartridge and ending logic.
