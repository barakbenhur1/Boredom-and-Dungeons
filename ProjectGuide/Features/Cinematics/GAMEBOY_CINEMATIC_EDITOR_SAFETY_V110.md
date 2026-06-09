# Game Boy Cinematic Editor Safety V110

## Fixed warning

```text
BDGameBoyCinematicRoom.consumeBatteries is assigned but never used
```

`consumeBatteries` is now implemented.

## Fixed editor-mode issue

Unity warning:

```text
Destroy may not be called from edit mode! Use DestroyImmediate instead.
```

Cause:
- The scene builder creates Game Boy and battery collectibles in Editor Mode.
- Their procedural visuals remove child primitive colliders.
- V108/V109 used `Destroy(...)`.

Fix:
- Added safe destroy helper:

```text
if Application.isPlaying:
    Destroy(obj)
else:
    DestroyImmediate(obj)
```

## Gameplay behavior

Default cinematic behavior is unchanged:

```text
consumeBatteries = false
```

So the batteries are still required, but not consumed unless the field is enabled.
