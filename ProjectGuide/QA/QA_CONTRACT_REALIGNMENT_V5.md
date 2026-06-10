# QA Contract Realignment V5

## Purpose

This corrective patch realigns automated QA with the current approved production behavior. It does not weaken verification and does not restore superseded presentation values.

## Correct contracts

- The modern handheld product-shot owner uses `new Vector3(0f, 0.28f, 0f)` after the approved lower-on-table composition pass.
- The horse interaction owner is the fixed bottom contextual strip.
- World-space horse prompt height and screen-offset literals are obsolete and must not be required.
- The BBH feature document explicitly records the `H landing` motion.
- Horse injury documentation includes the canonical speed-band summary `100% / 92% / 84% / 76%`.
- The reserved `future shop and NPC markers` contract remains documented without prematurely creating runtime markers.

## Verification

1. Apply this patch from the repository root.
2. Unity may remain open; this patch changes only source and maintained documentation.
3. Wait for Unity compilation to finish.
4. Run `Boredom And Dungeons → TEST EVERYTHING`.
5. The six stale-contract blockers that caused this patch must be absent.
6. Do not commit until automated QA and the required Play Mode review pass.
