# Mounted Run Introduction and Doorway Portals

## Purpose

A fresh New Game, or a restart that follows a non-death cinematic/victory flow, begins with the child already mounted on the horse. The horse rides through the map entrance before gameplay control opens.

Any return to a new run after a cinematic shown following the doorway beyond the first boss counts as a victory/cinematic restart and receives the mounted entrance.

Death -> New Game is explicitly different: the mounted entrance does not play. The gameplay scene keeps the player unmounted at the authored start-room spawn point with the normal gameplay camera.

## Start-reason selection

- **Fresh New Game:** play the mounted entrance.
- **Victory/cinematic restart:** play the mounted entrance.
- **Restart after a cinematic beyond the first-boss doorway:** classify as victory/cinematic and play the mounted entrance.
- **Death -> New Game:** death takes precedence; skip the mounted entrance.

The death reason is consumed only when a real gameplay scene containing player, horse, and minimap-room metadata loads.

## Mounted intro sequence

1. During `sceneLoaded`, while the black transition cover is fully opaque, resolve and place the cinematic camera before `Start`, `LateUpdate`, or any rendered gameplay frame can expose the normal follow position.
2. Lock every gameplay input before the first visible gameplay frame, including mouse aim/facing and horse steering.
3. Resolve the player, horse, entrance room, exact doorway center/plane, opposite rear wall, and normal gameplay camera owner.
4. Keep the existing Main Camera and its only `AudioListener` active; disable only the follow driver while the cinematic owns the transform.
5. Place the child mounted on the horse outside the authored entrance.
6. Place the cinematic camera inside the room, noticeably higher and farther from the entrance, approximately `30%` of room depth measured from the opposite rear wall toward the entrance, with safe wall inset.
7. Aim the cinematic camera at the center of the entrance opening; it is not the rider viewpoint.
8. Ride straight through the entrance.
9. At the approved turn point, rotate the horse approximately `90°` to the right with eased movement.
10. Decelerate to a full stop and hold the stopped pose for `0.15–0.30s`.
11. Only after the stop, restore the normal `BDCameraFollow` framing and release input after all held and buffered actions are discarded.

## First-visible-frame camera contract

- No frame may show the Main Camera at the player, horse, doorway mouth, or normal follow position before the mounted cinematic shot.
- `BDRunPresentationCoordinator` primes the cinematic position, rotation, and cinematic FOV synchronously from `sceneLoaded` while the cover is opaque.
- `BDCameraFollow` is disabled before its `Start`/`LateUpdate` can write a pose and is restored only after the cinematic stop and camera-return sequence.
- Failure to resolve the cinematic safely restores the normal follow owner before the cover is released.

## Camera-height refinement

The approved depth remains `30%` from the opposite rear wall toward the entrance. The V21 elevation is approximately `72%` of room depth, clamped to `17–21.5` world units, while the look point remains the doorway center.

## Death restart sequence

1. Load the gameplay scene normally.
2. Do not move the player to the doorway and do not call the mounted intro.
3. Keep the player unmounted at the authored start-room spawn point.
4. Keep the camera at normal gameplay framing.
5. Clear transient combat actions and red floor telegraphs.
6. Discard held loading input until release.

## Input contract

During the entire mounted cinematic, the central presentation lock blocks movement, horse steering, mouse/touch aim, facing, every attack, jump, dodge, mount/dismount, Pet, interactions, buffered actions, and look/aim-state updates.

Nothing pressed or held during loading or the cinematic may replay when control returns. The lock ends only after the horse finishes the right turn, reaches a full stop, completes the short stop hold, and normal camera ownership is restored.

## Portals

Open external entrance and exit doorways receive an opaque animated light portal that hides the entire area behind the doorway. The cover aligns to the real doorway plane and fills the opening from left to right and floor to top, with no visibility leak from frontal, side, diagonal, high, or low viewing angles. It has no gameplay collider, so legal passage remains possible.

## Verification

1. Start a fresh New Game and confirm there is no entrance-close or rider-follow frame before the cinematic shot appears.
2. Confirm the very first visible gameplay frame already uses the inside-room cinematic camera, farther and visibly higher, looking at the entrance rather than from the rider viewpoint.
3. Move the mouse and press every gameplay input throughout the cinematic; no gameplay state changes.
4. Confirm the horse enters, turns about `90°` right, fully stops, and holds briefly.
5. Confirm control and normal camera return only after the stop.
6. Confirm exactly one active `AudioListener` before, during, and after the cinematic.
7. Confirm the authored entrance and exit covers fill their exact openings and hide everything behind them from every angle.
8. Die and select New Game; confirm the player starts unmounted at the authored spawn with no mounted intro.
9. Repeat fresh/cinematic and death-restart paths.
10. Run `Boredom And Dungeons -> TEST EVERYTHING` and inspect the Console.

## V23R17 clear-direction final turn

- The horse no longer performs a fixed right turn.
- At the authored stop point it probes forward, right, left, then back with a body-sized capsule.
- The chosen direction must remain inside the entrance room, avoid registered hazards, and contain no solid blocker.
- The same chosen direction owns the final horse facing, gameplay camera return, and mounted-control handoff.


## V23R19D abandon-to-new-run rider binding

- Abandoning a live run requires explicit confirmation before the run is discarded.
- A confirmed abandon classifies the next run as a fresh mounted-intro start.
- The intro resolves the active loaded-scene `BDPlayerController` and `BDHorseController` deterministically.
- `ForceMountForCinematic` must bind that exact current-scene player before the horse is moved to the entrance.
- During every entrance movement phase the rider remains snapped to the horse mount point; the horse may never leave the entrance alone while the player remains at spawn.

## V23R19E abandon-to-new-run rider authority

- Scene load clears the cached player target before resolving intro actors.
- The horse's valid active-scene serialized rider is preferred, then the canonical active `BDPlayerMarker`, then typed fallback.
- `BeginMountedRunIntro` must establish the mounted state and snap the rider before the first visible horse movement frame.
- Every cinematic movement frame snaps the authoritative `horseController.Rider`; `CompleteMountedRunIntro` finishes mounted ownership before gameplay control returns.
- A riderless horse entrance after confirmed abandon is a blocking run-flow regression.

## V23R19O first-visible-frame rider-renderer contract

- Transform binding alone is insufficient. The active-scene Boy's eligible body Mesh/Skinned renderer branches must be visible before the cinematic cover begins revealing the scene.
- The coordinator captures the fresh scene's visible rider-renderer baseline and reasserts it after cinematic control components are disabled.
- Every mounted-intro rebind must:
  - keep the rider root and baseline body-renderer branch active;
  - set each baseline renderer enabled;
  - clear `forceRenderingOff`;
  - keep skinned renderers updating while offscreen during the teleport/entrance sequence.
- The Boy must be visible from the first revealed frame after abandon -> main menu -> Start Game, not appear only during the final camera handoff.
- Restore the original skinned offscreen-update policy after mounted gameplay control is released.
