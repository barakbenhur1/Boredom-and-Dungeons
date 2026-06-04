# Boredom & Dungeons — V14.2 FORCE Restore Movement

This patch fixes the situation where movement stops working after importing V13/V14 fixes.

It is self-contained:
- Does not depend on V12 movement classes.
- Does not depend on V13 room classes.
- Does not depend on V14 jumper classes.
- Adds one new direct movement controller to the current player.

## What it does

Menu:

```text
Boredom And Dungeons V14/FORCE Restore Movement In Current Scene
```

This menu action:

1. Finds the current player.
2. Removes old movement components from the player:
   - BD_DirectMovementV7
   - BD_DirectMovementV8
   - BD_DirectMovementV9
   - BD_DirectMovementV10
   - BD_DirectMovementV11
   - BD_DirectMovementV12
   - DirectPrototypePlayerController
   - PlayerController
   - PlayerMovement
   - PlayerDash
   - PlayerJump
3. Ensures the player has a CharacterController.
4. Adds:
   - `BD_ForceDirectMovementV14_2`
5. Adds/fixes a camera follow:
   - `BD_ForceCameraFollowV14_2`
6. Selects the player in the Hierarchy.

## Install

Copy the files into Unity and overwrite/merge:

```text
Assets/_Project/Scripts/EmergencyFix/BD_ForceDirectMovementV14_2.cs
Assets/_Project/Scripts/EmergencyFix/BD_ForceCameraFollowV14_2.cs
Assets/_Project/Scripts/Editor/BD_V14_2ForceRestoreMovementEditor.cs
```

Then wait for Unity to compile.

## Use

Open the scene you want to fix, then click:

```text
Boredom And Dungeons V14/FORCE Restore Movement In Current Scene
```

Press Play and click inside Game/Simulator.

## Controls

- W / Up Arrow = forward
- S / Down Arrow = backward
- A / Left Arrow = left
- D / Right Arrow = right
- Space = jump
- Left Shift = dash
- C = toggle camera follow

## Debug

The screen will show:

```text
B&D V14.2 FORCE MOVEMENT
Move X
Move Y
Position X
Position Z
Input Source
```

If Move X/Y changes, input works.

If Position X/Z changes, movement works.

If Move X/Y stays 0, the Game/Simulator window is not focused or Unity input handling is blocking input.
