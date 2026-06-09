# Dodge I-Frames V118

## Goal

Make dodge invulnerability frames clearer and more reliable.

## Changed

```text
dodgeInvulnerabilityExtraTime: 0.04 → 0.14
```

PlayerController now exposes:

```text
DodgeInvulnerableRemaining
DodgeInvulnerableProgress01
```

Added:

```text
BDPlayerDodgeIFrameFeedback
```

It pulses the player color during i-frames.

## Existing behavior preserved

`BDHealth` already ignores player damage during dodge through:

```text
BDPlayerController.IsDodgeInvulnerable
```

V118 extends and visualizes that window.
