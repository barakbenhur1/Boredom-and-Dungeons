---
name: final-integration-gate
description: Run the final integration, requirement, regression, documentation, and diff gate before Codex declares a task complete.
---

Verify every REQ id.
Inspect `git diff --check`, `git status --short`, and the final scoped diff.
Run the one required integration/release check.
Confirm no secrets, generated artifacts, unrelated changes, or documentation drift.
Update the authoritative project status when required.
Return only implemented result, REQ verification, changed files, validation evidence, and material risk.
