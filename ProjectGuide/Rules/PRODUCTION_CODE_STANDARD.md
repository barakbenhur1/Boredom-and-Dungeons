# Production Code Standard

This is a permanent engineering contract for Boredom & Dungeons.

## Scope

The standard applies to every production file that is created, modified, reviewed, repaired, moved, or materially encountered while implementing a task. Existing unrelated code is not rewritten gratuitously, but any touched area must not be left in a knowingly weaker state.

## Ownership first

1. Change the system that already owns the behavior.
2. Do not create a parallel state machine, input owner, camera owner, menu owner, damage owner, save owner or presentation owner to avoid understanding the current design.
3. A new component is valid only when it has a distinct lifecycle and responsibility that cannot cleanly belong to the existing owner.
4. Never describe an avoidable workaround as architecture. If a temporary bridge is unavoidable, record its removal condition, owner and deadline; otherwise implement the durable design now.

## Implementation quality

Production code must be:

- deterministic where behavior is scripted;
- explicit about state transitions and terminal states;
- idempotent for initialization, cleanup and package application;
- defensive at external boundaries without hiding internal contract violations;
- free of duplicate input, duplicated authority and accidental per-frame allocation;
- safe under pause, scene reload, repeated use, interruption and cancellation;
- clear in naming, method size, responsibility and comments;
- compatible with the project Unity/C# version;
- testable through the existing `TEST EVERYTHING` entry point and focused Play Mode evidence.

## Performance and lifecycle

- Cache stable references; do not perform broad scene searches, LINQ, material/texture creation or unbounded allocations in frame loops.
- Use the correct clock (`unscaledTime` for menu/pause presentation; gameplay clocks for gameplay truth).
- Unsubscribe events and stop temporary work on destroy, page replacement, scene change and cancellation.
- Generated Unity objects must have one owner and one cleanup path.
- Preserve `.meta` GUIDs and do not add Runtime dependencies on `UnityEditor`.

## Change discipline

- Preserve approved behavior and assets by default.
- Make the smallest cohesive durable change, not the smallest textual edit.
- Remove obsolete code only after migrating every valid responsibility and documenting the deletion.
- Update status, bug, architecture, technical decision, performance and QA truth in the same package.
- Never claim compilation, runtime behavior, performance or acceptance without real evidence.

## Delivery discipline

The assistant does not commit, push, create branches or open pull requests. Material work is delivered as a local backup-aware patch ZIP with preflight, idempotent install, validator and rollback. The user performs Unity verification and creates the local Git commit only after acceptance.

<!-- BND_QA_CONTRACT_REALIGNMENT_V5:BEGIN -->
## Automated-contract maintenance

When production behavior changes intentionally, the focused QA contract must be updated in the same local patch. Do not reintroduce retired code, obsolete coordinates, or duplicate owners solely to satisfy an outdated validator. Focused token checks must represent the current authoritative architecture and must remain readable in maintained documentation.
<!-- BND_QA_CONTRACT_REALIGNMENT_V5:END -->

<!-- BND_TUTORIAL_REFERENCE_LED_V3:BEGIN -->
## Reference-led tutorial implementation

When visual references are supplied, convert them into explicit palette, hierarchy, spacing and motion contracts. Do not copy unrelated content literally and do not leave the direction as subjective prose.

Instructional UI must use separate bounded regions and must be testable for clipping and overlap. Multiple simultaneously active input routes must be represented independently rather than compressed into tiny multiline text.
<!-- BND_TUTORIAL_REFERENCE_LED_V3:END -->

<!-- BND_FIRST_LAUNCH_TUTORIAL_QA_CONTRACT_FIX_V8:BEGIN -->
## QA for composed UI

For UI built from independent fields, automated checks validate each field
and resolver separately. Do not add dead strings or fake Runtime values only
to satisfy token scanning.
<!-- BND_FIRST_LAUNCH_TUTORIAL_QA_CONTRACT_FIX_V8:END -->
