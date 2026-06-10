---
name: unity-change-validation
description: Validate a Unity gameplay, scene, animation, UI, or asset change with the cheapest reliable test ladder; use after Unity project modifications.
---

Read repository-specific Unity commands before running anything.
Validate in order: serialization/import sanity, compile/static checks, targeted EditMode/PlayMode tests, affected scene/prefab checks, then the full project gate only if required.
Do not repeatedly launch Unity for checks that can share one run.
Capture concise logs and exact failing test names.
Check for unintended .meta changes, generated files, and scene/prefab churn.
