# Collectible Guardians V111

## Goal

Game Boy and battery collectibles should not be available in the starting room.
They should feel hidden and protected.

## Added

- Collectibles moved deeper / side-offset from the starting area.
- Each collectible has a small visual hideout.
- Each collectible has a guardian spawner.
- Guardians spawn when player enters the collectible radius.
- Guardians are stronger than normal corridor enemies.

## Guardian intent

They are not just decoration. Their job is to:
- interrupt the pickup attempt
- make leaving with the collectible harder
- create a small local combat event around important collectibles

## Current guardian sets

- Game Boy: 2 sword guardians + 1 charger guardian
- Battery A: 1 sword guardian + 1 charger guardian
- Battery B: 2 sword guardians

## Future pass

Replace procedural guardian spawn with authored encounter layouts per final map zone.
