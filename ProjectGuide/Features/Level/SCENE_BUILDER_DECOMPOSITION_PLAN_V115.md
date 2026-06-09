# Scene Builder Decomposition Plan V115

## Goal

Start decomposing `BDCreateCleanMazePrototypeScene` without changing generated gameplay behavior.

## What V115 does

- Converts `BDCreateCleanMazePrototypeScene` to a `partial` class.
- Adds a new partial stage-info file under `Scripts/Editor/SceneBuilders`.
- Adds editor utility scaffolds under `Scripts/Editor/Factories`.
- Adds a runtime UnityEditor dependency validator under `Scripts/Editor/Validation`.
- Updates project structure documentation.

## What V115 does not do

```text
No gameplay changes
No map layout changes
No collectible placement changes
No enemy placement changes
No camera changes
No movement changes
No combat balance changes
No HUD changes
```

## Next decomposition targets

1. Material creation helpers.
2. Prop/object creation helpers.
3. Collectible placement helpers.
4. Enemy spawn helpers.
5. Cinematic room builder.
6. Map layout builder.
7. Boss arena builder.

## Safety rule

Every future extraction must pass this test:

```text
Generated scene must be functionally identical unless the stage explicitly says otherwise.
```
