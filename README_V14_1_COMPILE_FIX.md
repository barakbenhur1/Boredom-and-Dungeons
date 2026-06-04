# Boredom & Dungeons — V14.1 Compile Fix

This fixes the compile error:

```text
BD_ExitBlockingEnemyV13.cs: error CS0246:
The type or namespace name 'BD_KnockbackReceiverV12' could not be found
```

## What happened

`BD_ExitBlockingEnemyV13.cs` referenced `BD_KnockbackReceiverV12` directly.
If Unity cannot find that class for any reason, the whole project stops compiling.

## What this patch does

It replaces `BD_ExitBlockingEnemyV13.cs` with a safer version that:

- Does not directly reference `BD_KnockbackReceiverV12`
- Checks knockback through reflection/helper methods
- Still works with V12 if the component exists
- Does not block V14 from compiling

It also replaces `BD_RoomExitZoneV13.cs` with a safer version that detects the player by component name instead of directly requiring `BD_DirectMovementV12`.

## Install

Copy these files into the exact same paths in Unity and overwrite existing files:

```text
Assets/_Project/Scripts/EmergencyFix/BD_ExitBlockingEnemyV13.cs
Assets/_Project/Scripts/EmergencyFix/BD_RoomExitZoneV13.cs
```

Do not create duplicate files like:

```text
BD_ExitBlockingEnemyV13 (1).cs
BD_RoomExitZoneV13 (1).cs
```

After Unity compiles, the Console error should disappear.
Then you can create the V14 scene again:

```text
Boredom And Dungeons V14/Create Jumper Enemy Test Scene
```
