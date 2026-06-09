# Inventory State V116

## Goal

Prepare the inventory state for the full secret collectible route.

## Secret collectibles

```text
Game Boy
Battery / battery count
Game Cartridge
```

## Added to BDGameBoyInventory

```text
hasGameCartridge
HasGameCartridge
HasEnoughBatteries(requiredBatteries = 2)
HasPoweredGameBoy(requiredBatteries = 2)
HasPlayableGameBoy(requiredBatteries = 2)
CollectGameCartridge()
Collect(BDSecretCollectibleKind kind)
HasCollected(BDSecretCollectibleKind kind)
SecretCollected event
```

## Meaning

```text
HasPoweredGameBoy = Game Boy + enough batteries
HasPlayableGameBoy = Game Boy + enough batteries + Game Cartridge
```

## Not changed yet

```text
No Game Cartridge collectible is spawned yet.
No ending branch behavior changed yet.
No badge HUD added yet.
No objective text added.
No collectible advertising added.
```

## Important design rule

The Game Cartridge is optional and secret. It should affect the final ending variant later, but it should not be advertised to the player.
