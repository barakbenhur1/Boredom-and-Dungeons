# Ending Speech Bubble Rules

## Purpose

The ending day scene should communicate the result of the optional secret collectible path with one simple speech bubble.

This is a planning rule only. It is not implemented in code yet.

## Required collectibles for the happy line

The player is considered to have all secret collectibles only when they have:

```text
Game Boy
2 Batteries
Game Cartridge
```

## Speech bubble rule

### Missing one or more collectibles

If the player reaches the ending day scene without all required secret collectibles, show this speech bubble at the end:

```text
I'm bored
```

### All collectibles collected

If the player reaches the ending day scene with all required secret collectibles, show this speech bubble at the end:

```text
I'm having fun :)
```

## Design intent

The line should be small, clear, and funny.

It should not become an objective, checklist, or tutorial hint.

## UI rules

Allowed:

```text
Speech bubble at the end of the day ending scene
Small readable text
Short timing before fade/end screen
```

Forbidden:

```text
Pre-ending hint telling the player what is missing
Checklist
0/4 progress
Objective marker
Text telling the player to find the Game Boy, batteries, or cartridge
```

## Implementation note for later

This should be connected to the existing ending state / inventory checks:

```text
HasGameBoy == true
BatteryCount >= 2
HasGameCartridge == true
```

If all are true, show:

```text
I'm having fun :)
```

Otherwise show:

```text
I'm bored
```
