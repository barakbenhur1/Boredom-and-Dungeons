# Boredom & Dungeons — Multi-Route Map and Hazard Design V1

Status: recovered and approved requirements, 2026-06-06.

## Major route topology

- Every accepted map must have at least 3 meaningful solution routes from the early map toward completion.
- Target 4 major routes when the seed and available space permit.
- Routes do not need to be completely separate.
- Routes should intentionally split, merge, and sometimes split again.
- Each route must contain a substantial unique segment and lead through different areas/rooms for part of the run.
- Major routes should ideally be spatially far from one another instead of running as nearby parallel corridors.
- Routes may converge into one route only after enough distance/progression has passed to make the earlier route choice meaningful.
- Tiny detours around the same obstacle do not count as separate solution routes.
- Add graph validation that rejects seeds without the required number of topologically meaningful route families.

## Inaccessible macro-regions

Reserve inaccessible regions with an approximate footprint of 1–4 room units.

Possible formations:

- mountains;
- lakes;
- chasms;
- holes;
- columns;
- large obstacles;
- mixed landmark formations.

Purposes:

- keep major routes separated;
- create recognizable landmarks;
- hide connections;
- force meaningful detours;
- reduce the feeling of a square grid maze.

Validation:

- no formation may disconnect every valid route;
- required content cannot become trapped;
- player and horse must retain legal progression;
- clearance around columns/obstacles must work for player, horse, enemies, and camera.

## Room holes and chasms

Player:

- falling into a hole or chasm removes exactly 15 health;
- respawn at the latest valid safe point;
- safe point must be stable walkable ground outside hazards and away from blocked geometry;
- short recovery protection prevents immediate repeated hazard damage.

Horse:

- navigation and flee behavior avoid holes/chasms;
- if the horse still falls, it loses no health;
- return it to the latest legal horse-safe point.

Mounted pair:

- if player and horse fall together, recover them together when possible;
- preserve mounted state when safe;
- otherwise place them safely beside one another without overlap.

## Lava

Player:

- lava contact removes exactly 10 health;
- immediately return/knock the player to the latest valid non-lava safe point;
- prevent repeated contact loops after recovery.

Horse:

- navigation and flee behavior avoid lava;
- if the horse still enters lava, it loses no health;
- return it to a legal non-lava horse-safe point.

## Quicksand

Player:

- quicksand is a progressive hazard, not instant damage on first contact;
- horizontal movement, dodge travel, and upward escape motion become progressively slower while the sink state deepens;
- a visible sand ring follows the actor at the quicksand surface and becomes stronger as sinking progresses;
- leaving the volume before full submerge clears the effect gradually;
- full submerge removes exactly 12 health and recovers the player to the latest valid safe point;
- safe points never update inside quicksand.

Horse:

- horse navigation and safety logic treat quicksand as unsafe terrain;
- entering quicksand progressively slows the horse rather than teleporting it immediately;
- full submerge recovers the horse without horse-health loss, matching the existing horse hazard-safety policy;
- when mounted, the pair is separated/recovered safely and the rider receives the player quicksand consequence once.

Readability:

- prototype quicksand uses a distinct warm sand/ochre surface, an explicit label, sticky movement, a surface ring, and entry/sink/escape cues;
- production art must remain visible under combat VFX and must not resemble ordinary walkable ground;
- the edge must be readable early enough for player and horse decisions.

## Safe-point validation

A safe point may update only when:

- the actor is on stable walkable terrain;
- it is outside holes, chasms, lava, and quicksand;
- there is enough clearance from edges, walls, props, enemies, and barriers;
- it cannot place the actor inside an active boss barrier or inaccessible region.

Fallback rules must handle missing or invalid recent safe points without soft-locks.

## Readability and placement

- Hazard edges must remain readable under lighting, VFX, enemies, bullet hell, camera framing, and Mother Boss foreground clothing.
- Required routes, boss arenas, reward chests, collectibles, and ending interactions must remain reachable.
- Escape-blocking enemies cannot force unavoidable movement into a hazard.

## QA

- Validate at least 3 meaningful routes for every accepted seed; target 4 when possible.
- Reject tiny nearby detours as fake routes.
- Validate route split/merge/re-split behavior.
- Validate inaccessible regions from 1–4 room units.
- Test repeated falls, low-health falls, mounted falls, lava-edge contact, invalid checkpoints, death during hazard damage, and recovery near enemies/barriers.
- Test horse follow, flee, wander, and mounted movement near holes/chasms, lava, and quicksand.
