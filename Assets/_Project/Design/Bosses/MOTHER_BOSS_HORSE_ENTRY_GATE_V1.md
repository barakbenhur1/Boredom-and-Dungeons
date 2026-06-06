# Boredom & Dungeons — Mother Boss Horse Entry Gate V1

Status: approved, 2026-06-06.

This document resolves the final open Mother Boss design question in `MOTHER_BOSS_DESIGN_V3_DECISIONS.md`.

## Narrative position

The black/white boss is the first boss in the final ending sequence.

After defeating it, the player reaches the door that leads toward the ending room and the secret Mother Boss continuation.

## Mandatory dismount rule

- The horse does not enter the Mother Boss encounter.
- The player must dismount before entering the door located after the black/white boss.
- The door cannot be entered while mounted.
- The game must not silently teleport the mounted player through the doorway.
- The game must not automatically carry the horse into the next room.

## Door interaction while mounted

When the mounted player attempts to enter:

- entry is blocked;
- a short readable prompt tells the player to dismount;
- no loading, transition, fade, or encounter activation begins;
- the door remains available immediately after the player dismounts.

Recommended English prompt:

`Dismount before entering.`

The prompt is functional guidance, not a secret-collectible hint.

## Horse waiting state

After dismounting near the door:

- the horse moves or snaps only if necessary to a validated safe waiting point outside the doorway;
- the horse remains outside the ending/Mother encounter area;
- follow, flee, wander, and mounted-recall systems cannot move it through the door;
- the horse does not appear in the Mother Boss arena, Phase 4, defeat cutscene, or victory cutscene;
- the horse cannot block the doorway or trap the player between the horse and arena geometry;
- the waiting point must avoid enemies, hazards, walls, barriers, and the black/white boss arena exit path.

The horse may use an idle/wait animation so its absence is intentional rather than unexplained.

## State handling

- Entering the post-boss door records that the horse has been left outside.
- Mother Boss loading/scene generation does not instantiate or migrate the horse.
- On Mother Boss defeat, the full-run restart resets the horse with the rest of the run.
- On Mother Boss victory, the game ends through the established colorful-light ending; no horse reunion is required.
- If the player reaches the door already on foot, entry works normally.
- If the horse is unavailable/fainted elsewhere, the door does not require the horse to be nearby; it only requires that the player is not mounted.

## QA requirements

- Verify mounted entry is blocked.
- Verify the dismount prompt appears and is readable.
- Verify manual dismount immediately enables entry.
- Verify no automatic dismount bypasses the intended player action.
- Verify no horse object, rider state, mounted weapon state, camera state, or horse HUD leaks into the ending/Mother encounter.
- Verify the horse waits outside without blocking the door.
- Verify follow/flee/recall systems cannot pull the horse through the door.
- Verify full restart restores normal horse state.
- Verify entering on foot works when the horse is distant, fainted, or absent.
