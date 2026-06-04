# Boredom & Dungeons — Clean Core V117 Minimap Visibility Hotfix

Hotfix after V116.

## What was fixed

The minimap did not show explored rooms reliably.

The old version relied mainly on trigger events. If those triggers did not fire correctly, rooms were never marked as discovered.

## New behavior

```text
The minimap now discovers the room by player world position every frame.
```

It still supports trigger discovery, but now has a fallback:

```text
If player is inside a minimap room:
    force discover that room

If no room is discovered yet:
    force discover nearest room to player
```

## Visual improvement

```text
Discovered rooms are more visible.
Current room gets a clear outline.
```

## What did not change

```text
No gameplay changes
No movement changes
No combat changes
No inventory changes
No collectible changes
No enemy changes
No camera changes
No map generation changes
No ammo HUD changes
No cinematic changes
```

## Updated files

```text
Assets/_Project/Scripts/Runtime/BDMazeMinimap.cs
Assets/_Project/Scripts/Runtime/BDMinimapRoom.cs
Assets/_Project/Design/UI/MINIMAP_VISIBILITY_HOTFIX_V117.md
Assets/_Project/Design/QA/STAGE_04A_MINIMAP_HOTFIX_REPORT.md
```

## Install

Delete:

```text
Assets/_Project
```

Copy from this ZIP:

```text
Assets/_Project
```

Then:

```text
Compile
Run Create Clean Maze Prototype Scene
Enter Play Mode
Walk into several rooms
```

Expected:

```text
The minimap reveals rooms as the player explores them.
It should not stay empty.
It should not reveal the full map immediately.
```
