# Mounted Run Introduction and Doorway Portals

## Purpose

A fresh New Game, or a restart that follows a non-death cinematic/victory flow,
begins with the child already mounted on the horse. The horse rides through the
map entrance before gameplay control opens.

For the current production rule, any return to a new run after a cinematic shown
following the doorway beyond the first boss counts as a victory/cinematic restart
and receives the mounted entrance.

Death -> New Game is explicitly different: the mounted entrance does not play.
The freshly loaded gameplay scene keeps the player unmounted at the authored
start-room spawn point with the normal gameplay camera.

This replaces an arbitrary invisible startup delay with an in-world transition
only for the approved fresh/cinematic start reasons.

## Start-reason selection

The persistent run-start coordinator observes whether the player died before the
gameplay scene was left.

- **Fresh New Game:** play the mounted entrance.
- **Victory/cinematic restart:** play the mounted entrance.
- **Any restart after a cinematic beyond the first-boss doorway:** currently
  classify as victory/cinematic and play the mounted entrance.
- **Death -> New Game:** death takes precedence over any death presentation or
  cutscene; skip the mounted entrance.

The death reason is consumed only when a real gameplay scene containing player,
horse, and minimap-room metadata loads. Menu or cinematic scenes do not consume
it.

## Mounted intro sequence — fresh or cinematic/victory start only

1. Lock gameplay input as soon as the gameplay scene loads.
2. Resolve the player, horse, start room, and legal entrance opening.
3. Remove transient red enemy/landing telegraph objects left during scene startup.
4. Build the entrance and exit portal surfaces at legal external openings.
5. Place the child mounted on the horse outside the entrance threshold.
6. Ride through the entrance over `2.25s`.
7. Zoom the camera in modestly over `0.48s`.
8. Return smoothly to normal gameplay framing over `0.72s`.
9. Keep clearing buffered combat actions.
10. After the animation, keep control locked only while a gameplay input remains
    physically held.
11. When all gameplay inputs are released, unlock control immediately while the
    child remains mounted.

## Death restart sequence

1. Load the gameplay scene normally.
2. Do not move the player to the doorway and do not call the mount/ride intro.
3. Keep the player unmounted at the authored start-room spawn point.
4. Keep the camera at its normal gameplay framing with no cinematic zoom.
5. Clear transient combat actions and red floor telegraphs.
6. If an input is still physically held from loading, discard it until release.
7. Unlock gameplay immediately after release; when nothing is held, unlock
   without an added timer.

## Input contract

There is no arbitrary post-animation delay.

Movement, attack, ranged, landing attack, horse commands, Pet, and buffered
actions pressed during loading or the cinematic are discarded. A held button
must be released before gameplay opens, so it cannot replay as the first attack.

## Red floor artifact

Enemy proximity/attack telegraphs and their transient floor visuals are blocked
during the intro. Existing transient red/orange floor objects are destroyed when
the scene loads and throughout the cinematic.

The player is protected from incoming damage during the locked transition.

## Portals

Open external entrance and exit doorways receive an opaque animated light portal
that hides the area behind the doorway. It has:

- two-sided opaque light surfaces;
- a warm metallic luminous frame;
- animated vertical light bands;
- distinct but related entrance and exit colors;
- no collider, so horse and player passage remains legal.

The portal is derived from `BDMinimapRoom` opening metadata and does not create a
closed-wall passage.

## Camera

The active camera's existing follow/orbit driver is temporarily disabled.
The camera tracks the mounted pair with its existing world offset, uses a modest
projection zoom, and restores the previous camera driver and projection at the
end.

## Verification

1. Start a fresh New Game.
2. Confirm the child begins mounted outside the entrance.
3. Confirm the horse rides through the visible light portal.
4. Confirm the camera is slightly closer and returns smoothly to normal.
5. Hold left/right mouse or a movement key during scene loading.
6. Confirm the held input does not trigger an attack or movement when control
   opens.
7. Release all inputs and confirm mounted control opens immediately.
8. Confirm no red floor marker, slash, impact, shake, flash, hit-stop, or damage
   appears during the intro.
9. Confirm entrance and exit portals hide what is behind them and remain
   traversable.
10. Trigger a non-death cinematic/victory restart and confirm the mounted
    entrance plays again.
11. Die and select New Game.
12. Confirm there is no entrance ride and no cinematic zoom.
13. Confirm the player is unmounted at the authored start-room spawn point.
14. Hold left/right mouse during the death restart load and confirm no attack
    replays after release.
15. Repeat the death restart at least three times.
16. Run `Boredom And Dungeons -> TEST EVERYTHING`.
