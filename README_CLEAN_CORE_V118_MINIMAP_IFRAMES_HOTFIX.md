# Boredom & Dungeons — Clean Core V118 Minimap + I-Frames Hotfix

Hotfix after V117.

## Fixed: minimap still not updating

V118 makes minimap discovery much more aggressive and reliable.

```text
The minimap now:
- resolves player directly by BDPlayerMarker
- discovers the current room by player world position
- if containment fails, discovers the nearest room within range
- runs discovery in Update and OnGUI
- moved to top-right so ammo HUD cannot cover it
- shows Explored X/Y
```

## Added: dodge i-frames

Dodge invulnerability window is now clearer:

```text
dodgeInvulnerabilityExtraTime = 0.14
```

Added:

```text
DodgeInvulnerableRemaining
DodgeInvulnerableProgress01
BDPlayerDodgeIFrameFeedback
```

The player briefly pulses during i-frames.

## What did not change

```text
No map generation changes
No combat aim changes
No inventory behavior changes
No collectible placement changes
No enemy placement changes
No camera changes
No ammo/reload behavior changes
No cinematic changes
```

## Updated files

```text
Assets/_Project/Scripts/Runtime/BDMazeMinimap.cs
Assets/_Project/Scripts/Runtime/BDMinimapRoom.cs
Assets/_Project/Scripts/Runtime/BDPlayerController.cs
Assets/_Project/Scripts/Runtime/BDPlayerDodgeIFrameFeedback.cs
Assets/_Project/Scripts/Editor/BDCreateCleanMazePrototypeScene.cs
```

## Test

```text
Delete Assets/_Project
Copy Assets/_Project from V118 ZIP
Compile
Run Create Clean Maze Prototype Scene
Enter Play Mode
Walk through several rooms
Watch minimap top-right
Dodge near enemies
```

Expected:

```text
Minimap explored count increases.
Rooms appear as you explore.
Minimap is not covered by ammo HUD.
Dodge gives a short damage immunity pulse.
```
