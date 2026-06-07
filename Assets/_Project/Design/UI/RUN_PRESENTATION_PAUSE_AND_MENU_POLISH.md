# Run Presentation, Authored Doorways, Pause, and Menu Polish

## Scope

This repair is a cross-cutting C03/C11 regression fix. It preserves the authored maze, the existing `BDMainMenuFlow`, its settings model, the BBH boot intro, the existing ending/result router, and all unrelated local work.

## Authoritative behavior

### Startup and main menu

- A presentation cover exists before the gameplay scene can draw, so the map never flashes before the main menu.
- The BBH boot intro remains above that cover and is not replaced.
- The project keeps one menu/pause owner: the existing `BDMainMenuFlow`. No second Pause controller is created.

### Entrance

- No replacement entrance door, frame, or collider is authored.
- Obsolete generated `BD_Entrance_Portal` and `BD_Exit_Portal` objects are removed.
- A visual-only surface is attached to the authored entrance and authored exit.
- A fresh run, an abandoned run started again, and a post-cinematic/victory run use the mounted authored-entrance sequence.
- The camera begins modestly closer, the horse enters automatically, stops at the authored spawn, the camera returns to normal, and control is released only after the sequence.
- An ordinary death restart begins on foot at spawn and does not replay the mounted intro.

### Exit

- An invisible approach trigger is attached to the authored exit only.
- Once the player or mounted horse starts entering, input is locked and the final movement continues automatically.
- The screen fades/cuts and hands off to the existing ending/video route.

### Pause and buttons

- Escape uses the existing `BDMainMenuFlow` pause popup and stops game time and audio.
- Resume returns to the run, Settings uses the existing persistent `BDGameSettings`, and Main Menu / Abandon Run returns to the unchanged main menu.
- Main-menu and Pause buttons share one visual language, with distinct action colors and symbols for progression, configuration, and leaving/abandoning.

### Minimap

- Rooms, walls, openings, player, and horse rotate through one `GUI.matrix` root as a rigid unit.
- The original matrix is restored in `finally`.
- Post-transform overflow masks repaint every region outside the authored inner square, so the minimap cannot escape its frame.

### Room walls and camera

- Prototype room walls are raised to a minimum visual/collision height of 64 world units so the angled camera cannot see over them into another room.
- The camera is constrained by real profiled wall colliders and the current room or legal two-room handoff union. Structural walls stay permanently opaque. Approaching a closed wall constrains outward camera intent and uses a more top-down pitch instead of revealing the next room.
- Open doorway sides remain eligible for normal room transition. Room/node handoff preserves camera distance and produces neither backward snap nor zoom-in.
- Wall segments receive explicit non-mirrored texture-facing metadata for future asymmetric natural textures.

## Verification

Run the single required QA command and then test fresh start, ordinary death restart, abandon-and-restart, authored exit, Escape pause, repeated 90/180-degree minimap changes, and camera movement against every closed wall while mounted and on foot.
