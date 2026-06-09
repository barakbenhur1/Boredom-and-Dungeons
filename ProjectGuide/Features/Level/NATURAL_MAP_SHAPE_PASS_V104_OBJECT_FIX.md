# Natural Map Shape Pass V104 — Object Ambiguity Fix

Unity compile error fixed:

```text
Assets/_Project/Scripts/Editor/BDCreateCleanMazePrototypeScene.cs(992,39):
error CS0104: 'Object' is an ambiguous reference between 'UnityEngine.Object' and 'object'
```

Cause:
- Editor script imports `System`.
- `Object` can resolve ambiguously between C# `object/System.Object` and `UnityEngine.Object`.

Fix:
- Standalone `Object.*` calls are now fully-qualified as:

```text
UnityEngine.Object.*
```

This is a compile-only fix. No gameplay behavior changed.
