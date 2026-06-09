# Secret Collectible Badge HUD V121

## Goal

Show a small badge only after the player has collected a secret collectible.

## Rules

Do not show:

```text
empty slots
0/4
missing
objective text
arrows
markers
instructions to collect anything
```

Show only after pickup:

```text
GB
BAT / BAT x2
CART
```

## Added

```text
BDSecretCollectibleHud
```

It listens to `BDGameBoyInventory.SecretCollected` and displays only collected secrets.

## Placement

The badge row is small and placed top-left.

It should not interfere with:

```text
Ammo HUD top-right
Minimap bottom-right
Status panel bottom-left
```

## Important

This is a secret-discovery UI, not a quest UI.
