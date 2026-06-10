# Contextual Gameplay HUD Visibility V1

## Goal

Keep the world readable while showing information immediately when it becomes relevant. HUD elements fade rather than pop, and every widget remains owned by its existing renderer.

## Player health

Player health is visible when:

- the player is standing still; it remains visible until movement begins;
- the player takes damage; it remains visible briefly, then fades;
- the player receives healing; it remains visible briefly, then fades;
- the player is dead.

Damage and healing use the existing `BDHealth.HealthChanged` event. The visibility director does not infer pickups or alter health.

## Horse health

Horse health is hidden unless the player is on foot and standing beside the horse. It remains hidden while mounted and uses the same professional fade language as player health.

## Ranged ammunition

The ammunition/reload widget:

- appears on the ranged-button press frame;
- remains visible for the complete hold, including charged-shot holds;
- begins fading on release;
- does not wait for the shot to commit before appearing.

`BDPlayerCombat` exposes input intent; `BDGameHud` remains the renderer.

## Minimap

The minimap is not fully hidden merely because the player stops. Standing still is a common map-reading moment. Instead:

- it remains full strength during movement, nearby threat, room discovery or explicit map input;
- after a safe idle delay it fades to 38% opacity;
- it returns to full opacity quickly when relevant again.

This preserves orientation and accessibility without leaving a permanently dominant panel.

## Other on-screen UI

- Interaction prompts are contextual and disappear when unavailable.
- Combat-event and death messages remain event-driven.
- Targeting presentation remains governed by combat intent and the existing gameplay visibility gate.
- Menu, tutorial, cinematic and pause owners can still suppress the complete gameplay HUD globally.

## Animation quality

- Fade-in is fast enough to feel immediate.
- Fade-out is slightly slower to avoid flicker.
- Context prompts combine opacity with a small vertical ease.
- All timing uses unscaled time so pause/cinematic transitions cannot strand partially visible UI.
