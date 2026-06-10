---
name: scoped-repo-inspection
description: Locate the smallest relevant code path without a broad repository scan; use before editing when files or symbols are unknown.
---

Follow the search ladder in `docs/agent-system/RESOURCE_POLICY.md`.
Start from status/architecture indexes, exact symbols, and scoped `rg`.
Exclude generated, vendor, cache, Library, build, and artifact trees.
Stop when you can name the execution path, affected files, and cheapest validation.
Return paths and symbols, not a long codebase summary.
