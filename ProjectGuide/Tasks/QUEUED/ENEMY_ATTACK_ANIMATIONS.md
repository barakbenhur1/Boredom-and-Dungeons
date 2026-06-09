# Queued Task — Enemy Attack Animations

```text
Status: REQUIRED / NEXT PRODUCTION FEATURE AFTER RETURNED BLOCKERS
```

Every active attack-capable enemy requires a readable sequence:

`Windup -> Commit -> Impact/Release -> Follow-through -> Recovery -> Cancelled`

Requirements:

- no damage before the visible contact/release moment;
- no accidental duplicate hit;
- distinct visual identity for materially different attacks;
- Battery guardians receive separate normal and strong attacks;
- gameplay remains the owner of damage, range, cooldown and target truth;
- animation, hitbox, VFX and audio synchronize to the same transaction;
- interruption, death, stagger, pause and scene/reset cleanup are explicit;
- add a near-spawn test harness and TEST EVERYTHING coverage;
- temporary procedural motion is not final production animation unless explicitly approved.

The full animation quality contract is `../../Features/Animation/PRODUCTION_ANIMATION_REQUIREMENTS_V1.md`.
