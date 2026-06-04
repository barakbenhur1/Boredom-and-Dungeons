# Boredom & Dungeons — Clean Core V124 Secret Collectible Guard

Stage 09 / 36.

## What changed

Added a validation guard to prevent accidental visible advertising of secret collectibles.

New validator:

```text
Assets/_Project/Scripts/Editor/Validation/BDSecretCollectibleAdvertisingValidator.cs
```

Unity menu:

```text
Boredom & Dungeons/Validation/Check Secret Collectible Advertising
```

## What it checks

It searches gameplay Runtime/Editor scripts for forbidden phrases like:

```text
Collect the Game Boy
Find batteries
Find the cartridge
Missing Game Boy
0/4
Objective: Game Boy
Secret checklist
```

## What is still allowed

```text
GB / BAT / CART badges after pickup
Pickup effects
Cinematic consequences
Environmental hints
```

## What did not change

```text
No gameplay changes
No movement changes
No combat changes
No ending changes
No cinematic changes
No minimap changes
No dodge i-frame changes
No ammo HUD changes
No map generation changes
No enemy changes
```

## Test

```text
Delete Assets/_Project
Copy Assets/_Project from V124 ZIP
Compile
Run:
Boredom & Dungeons/Validation/Check Secret Collectible Advertising
Run Create Clean Maze Prototype Scene
Enter Play Mode
```

Expected:

```text
Validation passes.
No objective/checklist/missing text appears.
Secret badges still appear only after pickup.
```

## Progress

```text
Stage: 9 / 36
Completed if QA passes: 9 / 36
Remaining: 27 / 36
Progress: 25.0%
```
