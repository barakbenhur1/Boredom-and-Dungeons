# Gameplay Shadow Policy V1

## Required basic shadows

The following gameplay models always cast and receive basic shadows:

- player;
- horse;
- normal enemies;
- mini-bosses;
- bosses;
- batteries;
- Game Boy;
- game cartridge / game tape;
- collectibles, pickups, quest items, and central interactables.

Required gameplay shadows are never removed by the optional performance budget.

## Optional performance-aware shadows

Secondary non-decoration gameplay models may cast shadows only while they are
inside the configured camera distance and renderer budget.

Default budget:

- maximum optional distance: 22 world units;
- maximum optional shadow renderers: 28;
- budget refresh: every 0.35 seconds;
- dynamic-object discovery: every 2.5 seconds.

## Decoration rule

The automatic policy does not add shadows to floors, walls, ceilings, terrain,
background decoration, VFX, particles, minimap/UI objects, lava, holes,
chasms, hazards, labels, or attack telegraphs.

These may receive separately authored shadows only after profiling confirms
that target-platform frame time remains inside budget.

## Preservation rule

The policy changes only Renderer shadow-casting and shadow-receiving settings.
It does not remove or replace models, materials, textures, effects, lights,
animation, colliders, gameplay components, or scene content.

## Dynamic models

The runtime policy periodically discovers newly spawned enemies, bosses,
collectibles, interactables, and other eligible gameplay models.
