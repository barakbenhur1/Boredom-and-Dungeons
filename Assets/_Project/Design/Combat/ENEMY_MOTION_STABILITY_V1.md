# Enemy Motion Stability V1

- Normal enemy brain movement is speed-capped and smoothed.
- Charger, knockback, authored jumper motion, and hook pulling receive explicit exceptional envelopes rather than unlimited displacement.
- A late stability owner detects impossible one-frame displacement and corrects it without becoming an AI brain.
- Enemies that remain above or below valid ground after the allowed authored motion are recovered to ground.
- Jumpers prefer near-side and side landings, keep clearance from the player, avoid repeated ground snapping, and finish at a validated landing point.
- Enemies must never teleport across the player, outrun the horse through a glitch, or remain suspended and unhittable.

## V23R12 root-aware grounding and spawn repair

- Convert the sampled ground surface to the correct CharacterController root using center/height/radius/scale.
- Validate before first visibility and once after initialization.
- Horizontal relocation is spawn/recovery-only, never a continuous LateUpdate teleport.
- Ground correction and hook completion reset the stabilizer baseline.
