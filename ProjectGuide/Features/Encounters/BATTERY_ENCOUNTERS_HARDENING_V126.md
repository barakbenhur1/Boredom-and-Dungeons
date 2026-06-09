# Battery Encounters Hardening V126

## Goal

Make protected battery encounters harder but fair.

## Changed

Guardian encounters now resolve fair spawn positions before activating enemies.

## Fairness rules

```text
Guardians should not spawn on top of the player.
Guardians should not spawn too close to the collectible.
Guardians should not overlap each other.
If the first spawn point is bad, search alternate angles/distances.
If all candidates are imperfect, use the best scored fallback.
```

## New tuning values

```text
triggerRadius = 8.25
swordGuardians = 3
chargerGuardians = 1
spawnDistance = 6.6
spawnArcDegrees = 150
swordHealth = 190
chargerHealth = 260
minimumDistanceFromPlayer = 6.25
minimumDistanceFromCollectible = 3.75
minimumGuardianSpacing = 1.85
maxSpawnDistance = 9.25
spawnDistanceStep = 0.75
spawnAngleStep = 18
spawnResolveAttempts = 14
spawnVfxDelay = 1.05
```

## Preserved from V125

```text
Teleport/smoke VFX
Activation delay
Hidden inactive enemies during anticipation
Final flash on activation
```

## Not changed

```text
No collectible placement changes.
No map generation changes.
No objective text.
No minimap changes.
No combat aiming changes.
```
