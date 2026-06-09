# Range-Aware Target Highlight V1

## Purpose

Make weapon aim readable without adding lock-on behavior or visual clutter.

## Target selection

- At most **one target** is highlighted.
- The target is the first valid enemy along the active weapon aim direction.
- Structural walls and blockers prevent highlighting an enemy behind them.
- The highlight appears only when that enemy is within the real range of the attack the player can currently use:
  - melee reach for a sword attack;
  - hook range while a ready heavy-hold hook is being prepared;
  - projectile range only when ranged fire is currently available.
- Player, horse, dead enemies, and non-combat objects are never highlighted.

## Visual language

- Use one subtle professional red **silhouette outline** that follows the visible shape of the enemy model.
- Do not use a rectangular screen-space box, corner frame, filled tint, pulse, or distance-based growth.
- Outline thickness remains visually constant in pixels at near and far legal targeting distances.
- The outline uses an inverted render shell and never changes the source materials, model scale, camera, movement, or target lock state.
- The outline disappears immediately when aim, line of fire, range, availability, death, menu state, or target ownership changes.

## Verification

1. One in-range aimed enemy receives the red silhouette outline.
2. Off-axis and out-of-range enemies receive no frame.
3. A wall removes the frame.
4. Switching from melee to ranged/hook updates the valid range.
5. The outline causes no damage, movement, target lock, material instance leak, or camera ownership change.

## V23R10 ranged visibility

When a normal projectile is loaded and not reloading, the one primary enemy that the current projectile path would hit may receive the red corner frame even before the fire button is pressed. The frame remains line-of-fire truthful, uses the projectile radius and lifetime, disappears behind blockers, and never highlights more than one enemy.

## V23R12 mounted ranged targeting

- While mounted, resolve a ranged-only envelope from a horse-height origin.
- The highlighter may remain operational even when grounded melee input is disabled, but only while gameplay targeting visibility is allowed.
- The framed enemy must still be the first valid projectile hit along the current aim path.

## V23R13 silhouette contract

The old GUI corner box is removed. A fixed-pixel inverted-hull shader traces the enemy mesh silhouette directly, including mounted ranged targeting, without becoming larger as distance changes.

## V23R19O damageable-model-only silhouette

- The red silhouette represents the enemy surface that can actually receive damage, not every renderer parented under the enemy root.
- Resolve enabled, non-trigger colliders whose nearest `BDHealth` owner is the selected enemy.
- Create outline shells only for eligible Mesh/Skinned renderers that intersect that damageable collider envelope.
- Exclude broad flat ground circles/rings, range or selection indicators, telegraphs, health/prompt UI, particles, trails and existing outline shells.
- The auxiliary enemy ring remains visible but receives a 0.62 alpha multiplier so it reads as supporting information rather than the target body.
- Target selection, range, line-of-fire blockers, attack ownership and damage are unchanged.
