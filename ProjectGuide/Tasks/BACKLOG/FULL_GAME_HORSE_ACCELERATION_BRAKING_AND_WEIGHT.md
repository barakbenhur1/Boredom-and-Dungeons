# Full Game Horse Acceleration, Braking, Direction Change and Weight

**Status:** `QUEUED — AFTER CURRENT FIRST-LAUNCH TUTORIAL RELEASE GATE`

**Scope:** Full game only. Do not apply to the standalone first-launch tutorial unless it explicitly shares the same production riding system or a later task requests unification.

## Objective

Improve the full-game horse movement system so starting movement, acceleration, speed changes, direction changes and stopping feel natural, gradual and convincing rather than snapping between idle, maximum riding speed and a complete stop.

## Starting movement and acceleration

- The horse must not jump immediately from idle to maximum riding speed.
- It must respond immediately at a low but clearly perceptible speed so control remains responsive.
- Speed then rises gradually to the existing maximum speed.
- Default acceleration time from idle to maximum speed: **1.5 seconds**.
- Allowed tuning range: **1.3–1.8 seconds** after play testing.
- Keep the current maximum speed unchanged; change only how it is reached.
- Use a natural non-linear acceleration curve: restrained start, stronger middle acceleration, smooth settling near maximum speed.
- Holding movement input continues acceleration to maximum speed.
- A short tap must not automatically produce maximum speed.
- Acceleration values must be centralized and tunable, not duplicated as hard-coded literals.

## Deceleration and stopping

- Releasing movement input must not stop the horse instantly.
- Deceleration begins from the horse's current speed.
- Higher speed produces a slightly longer stopping time and distance.
- Default stop time from maximum speed: **0.9 seconds**.
- Allowed tuning range: **0.7–1.2 seconds**.
- Braking must remain responsive and must not feel like excessive sliding.
- Use a natural non-linear braking curve: clear initial slowing, gradual speed reduction, soft final stop without a last-frame snap.
- Do not allow long uncontrolled coasting after input is released.
- If input resumes while the horse is slowing, continue smoothly from current velocity instead of resetting speed.

## Direction change

- Do not reverse direction instantly at full speed.
- Small direction changes may preserve part of the current speed.
- Sharp turns and direct reversal require meaningful deceleration before accelerating in the new direction.
- After the direction changes, acceleration resumes through the same acceleration system.
- The horse must never look like a vehicle rotating on its axis or a weightless character flipping direction.

## Control responsiveness and obstacle safety

- Input response must remain immediate and readable despite gradual speed changes.
- Acceleration must not make the horse feel heavy, delayed or frustrating.
- Braking must not make the player feel that control was lost.
- Near walls, obstacles, ledges and narrow areas, momentum must not create unavoidable collisions.
- A subtle obstacle-aware braking assist may be used when required, provided it does not feel artificial.
- Existing collision, mount, dismount, attack, jump, fall, damage and camera behavior must remain intact.

## Animation synchronization

All horse animation must be driven by actual world speed and acceleration state:

- Full stop: idle animation.
- Low speed: walk / slow movement.
- Medium speed: progressively faster leg cycle.
- High speed: run / gallop animation.
- Blend idle, walk, run and gallop gradually without popping.
- Leg speed must never visibly disagree with world movement speed.
- Head, body, tail and joints should reflect speed, acceleration and deceleration.
- A slight forward body lean may accompany acceleration.
- Braking may use a restrained backward body response and natural leg stabilization.
- Do not exaggerate movement in a way that harms control or readability.

## Rider synchronization

- The rider reacts to acceleration and braking.
- Acceleration may move the rider subtly backward before settling.
- Braking may move the rider subtly forward before returning to a stable riding pose.
- Rider hands and legs remain visually connected to the horse.
- The rider must not slide, jump or detach from the mount.

## Audio and effects

- Hoof cadence follows current speed continuously.
- Full gallop cadence must not begin immediately from idle.
- Cadence rises during acceleration and falls during braking.
- Surface-specific stop, friction and impact sounds remain restrained.
- Dust, ground particles and motion effects scale with actual speed.
- Do not use strong gallop effects at low speed.
- A short subtle dust response may occur after stopping from high speed, without exaggerated skidding.

## Special states

- Releasing input before maximum speed starts braking from the speed already reached.
- Resuming input during braking continues smoothly from current speed.
- A new movement cycle after a full stop restarts acceleration.
- Collisions must not reset velocity instantly without matching visual feedback.
- Damage, jumps, falls, attacks and other special actions must integrate with the velocity state instead of fighting it.
- Different surfaces may tune acceleration and braking slightly, but control must remain consistent.

## Default tunable values

- Acceleration time, idle to maximum: **1.5 seconds**.
- Acceleration tuning range: **1.3–1.8 seconds**.
- Deceleration time, maximum to stop: **0.9 seconds**.
- Deceleration tuning range: **0.7–1.2 seconds**.

## Acceptance criteria

- The horse no longer reaches maximum speed instantly.
- Acceleration is clear, smooth and natural.
- Releasing input no longer causes an immediate stop.
- Braking is natural without harming responsiveness.
- Reversal no longer flips direction instantly at full speed.
- Animation speed matches actual movement speed.
- Hoof audio and effects match speed, acceleration and braking.
- Rider motion responds naturally to speed changes and remains attached.
- There is no excessive sliding, speed snapping, mechanical motion or unavoidable obstacle collision.
- Behavior is consistent in every full-game area where horse riding is available.
- No regression is introduced in existing riding mechanics, collisions, controls, animation, camera or mounted actions.
