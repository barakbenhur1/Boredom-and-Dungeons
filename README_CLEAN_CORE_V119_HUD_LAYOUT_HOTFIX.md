# Boredom & Dungeons — Clean Core V119 HUD Layout Hotfix

Hotfix after V118.

## What changed

```text
Ammo / Reload HUD moved to top-right.
Minimap returned to bottom-right.
```

## What stayed from V118

```text
Minimap still updates by player world position.
Minimap still has nearest-room fallback.
Minimap still shows Explored X/Y.
Dodge i-frames still exist.
Dodge i-frame visual pulse still exists.
```

## What did not change

```text
No gameplay changes
No movement changes
No combat aim changes
No inventory changes
No collectible placement changes
No enemy placement changes
No camera changes
No map generation changes
No cinematic changes
```

## Updated files

```text
Assets/_Project/Scripts/Runtime/BDMazeMinimap.cs
Assets/_Project/Scripts/Runtime/BDGameHud.cs
Assets/_Project/Design/UI/HUD_LAYOUT_HOTFIX_V119.md
Assets/_Project/Design/QA/STAGE_04C_HUD_LAYOUT_HOTFIX_REPORT.md
```

## Test

```text
Delete Assets/_Project
Copy Assets/_Project from V119 ZIP
Compile
Run Create Clean Maze Prototype Scene
Enter Play Mode
```

Expected:

```text
Ammo HUD is top-right.
Minimap is bottom-right.
Minimap still updates while exploring.
Dodge i-frames still work.
```
