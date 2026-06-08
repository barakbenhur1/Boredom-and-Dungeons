# Boredom & Dungeons — Canonical Art Direction and Interface Conventions

> **Canonical root source: `ART_DIRECTION.md`.** This file under `Assets/_Project/Design/Visual/` is the synchronized Unity-side mirror for discovery from the Project window. Every art, UI, VFX, model, material, texture, animation, menu, HUD, icon, typography, and presentation change must follow the canonical root document.

## Authority and reference use

This is the durable visual source of truth for environments, characters, models, materials, textures, lighting, menus, HUD, prompts, icons, effects, animation feel, and desktop/mobile presentation.

Reference board: `Assets/_Project/Design/Visual/References/BOREDOM_AND_DUNGEONS_ART_DIRECTION_REFERENCE_BOARD_V1.jpg`

The board guides finish quality, palette, atmosphere, fantasy ornament, child-centered adventure energy, and readable interaction language. Do not copy its logos, characters, branded layouts, camera perspective, or individual assets.

## Core visual promise

- 65% colorful wonder.
- 35% mystery and danger.
- Impressive, clean, stylized fantasy from a child's point of view.
- Cool and adventurous rather than infantile.
- Magical without visual noise.
- Readable from the approved angled top-down / 2.5D gameplay camera.

## 3D form language

Use polished medium-detail stylized geometry with clean silhouettes, slightly exaggerated proportions, readable masses, and selective detail. Friendly and magical forms lean rounder and more open; danger, corruption, traps, and hostile machinery lean sharper, heavier, broken, or asymmetrical. The horse may carry more elegance and magical detail than ordinary props and minor enemies. Bosses earn denser detail but must remain readable from gameplay height.

Avoid blockout-looking low-poly, photorealism, tiny unreadable geometry, realistic gore, and equal detail density everywhere.

## Materials and textures

Use a hand-painted / restrained-PBR hybrid. Hand-authored color grouping and gradients create identity; roughness, normals, and specular response support form without photographic noise. Wood, stone, cloth, metal, leather, foliage, crystal, slime, and magic remain distinct. Use broad variation instead of micro-noise, selective wear instead of universal grime, and controlled emissive accents only where magic or guidance justifies them.

## Color language

Structural neutrals: deep navy, charcoal, dark blue-green, muted warm brown, and desaturated stone.

Primary accents:

- cyan/turquoise — discovery, navigation, friendly magic, clarity;
- royal purple — mystery, void energy, dangerous magic, elite enemies;
- amber/gold — courage, reward, important interaction, lantern light, Game Boy ornament;
- natural green — life, recovery, safe nature, healing;
- controlled red — danger, damage, hostile targeting, urgent states.

Dark scenes must remain colorful enough to guide the eye.

## Lighting and atmosphere

Use thin ambient mist, soft volumetric-looking layers, light shafts, motes, fireflies, dust, spores, and restrained magical particles. Separate foreground, playable plane, and background through value, saturation, and atmosphere. Contrast warm practical light with cool magic. Effects and fog must never obscure hazards, targets, or prompts.

## Environment families

The shared language supports bright grasslands and ruins, enchanted forests, lantern paths, luminous mushroom marshes, rune caves, reflective water, crystal portals, sunlit canyons, ancient machinery, haunted villages, stitched creatures, clockwork corruption, and dark magical battlefields. Reused assets must be re-contextualized through material, color, vegetation, lighting, and silhouette treatment.

## Character and enemy readability

Player, horse, small enemy, large enemy, mini-boss, and boss silhouettes must be distinguishable before texture detail. Large enemies visually communicate mass and pull resistance. Boss spectacle never removes attack readability. Friendly creatures use softer shapes and warmer treatment. Targeting uses a thin constant-pixel red silhouette outline that follows the real enemy shape only when the active attack can truly hit; never fill the model red or draw a rectangular frame.

## HUD and interface

Use modern readability inside restrained fantasy ornament:

- dark graphite/deep navy translucent panels;
- thin intentional borders;
- gold, cyan, turquoise, purple, green, and red functional accents;
- stable edge placement for persistent data;
- an open center and forward gameplay area;
- context prompts near their subject and only while useful;
- one visual owner per action;
- clear active, cooldown, disabled, and unavailable states without relying only on color.

## Game Boy menu shell

The full main menu, settings, pause, results, collection, progression, and control explanations live inside an original stylized Game Boy-like in-world device. It may use modern color while retaining a chunky refined shell, fantasy engravings, magical status lights, pixel-influenced page transitions, cursor clicks, scanline/boot motifs, and short device sounds. Do not imitate a specific commercial model or lower resolution until text becomes hard to read.

### True victory transformation

After the real victory over Mother, the persistent device changes visibly: the worn/ordinary shell becomes restored or awakened, gold and cyan inlays activate, the boot sequence and palette change, and a unique cartridge/emblem state becomes available for the secret continuation or post-victory layer. This is a persistent narrative reward, not a temporary flash, and it must preserve settings and accessibility.

## Typography

Use a clear fantasy display font for titles, locations, bosses, and major headings. Use a highly readable companion font for body text, settings, descriptions, values, and prompts. Decorative fantasy lettering is not used for long or small text.

## Iconography

Use Game Boy-inspired game icons: simple silhouettes, strong light/dark separation, limited internal complexity, slightly pixel-influenced corners, and polished modern-resolution execution. Prompted actions show the active binding instead of a hard-coded key.

## Horse interaction visibility matrix

| State | Mount | Dismount | Heal | Pet |
|---|---|---|---|---|
| On foot near horse | Available + icon when legal | No | Available + icon only when needed | Available + icon when legal |
| Mounted and stationary | No | Available + icon | Disabled | Available by key, no icon |
| Mounted and moving | No | Available by key, no icon | Disabled | Disabled |

Transitions must not flicker around the stationary threshold.

## Effects

Effects are stylized, cool, and understandable from a child's point of view. Light attacks are quick and clean; heavy attacks are broader and weightier; airborne attacks preserve light/heavy identity in a vertical plane; the hook has a clear rope path and pull direction; healing uses friendly green/cyan light; Pet is warm and character-focused; dodge is crisp; bosses are dramatic but telegraphed; target highlight remains a thin red silhouette outline. Damage numbers use a short pop/rise/fade: coral-red for player damage, warm amber-gold for normal enemy damage, and vivid fuchsia/magenta for critical sword damage, with no opaque box and no exaggerated distance-based growth.

## Animation feel

Combine smooth elegance with physical weight. Traversal and horse movement are fluid. Turns connect visible facing with travel. Light attacks are fast; heavy attacks have anticipation and follow-through; landings, pulls, and large hits carry weight without excessive camera violence; friendly interactions use soft arcs and readable pauses; UI uses short Game Boy-like clicks, slides, page swaps, and boot/fade motifs.

## Production animation completeness

Every gameplay action that benefits from visible motion requires a final, production-quality animation or an explicitly approved procedural solution. Working hitboxes and state transitions are not sufficient visual completion.

Required coverage includes player, horse, enemy, mini-boss and boss locomotion; attacks; firing; jumping; wall jumping; dodging; Parry; mounting and dismounting; horse petting and healing; damage reactions; knockback; stagger; stun; hazard struggle; death; recovery; interactions; destructibles; and every other action whose readability or emotional weight depends on motion.

Temporary procedural animation is not final release animation. Detailed coverage, timing ownership, root-motion policy, interruption rules and the production acceptance gate are defined in `Assets/_Project/Design/Animation/PRODUCTION_ANIMATION_REQUIREMENTS_V1.md`.

<!-- temporary procedural animation is not final release animation -->

## Responsive desktop and mobile language

Desktop and landscape mobile share palette, typography hierarchy, symbols, icon rules, and Game Boy identity, but layouts adapt. Desktop uses keyboard/mouse keycaps and edge placement. Mobile uses larger touch targets, safe areas, fewer simultaneous prompts, and thumb-zone controls. The forward gameplay view stays clear on both.

## Quality gate

A visual element is approved only when it reads from the real camera, has one purpose, belongs to the shared palette/material language, is impressive without clutter, feels cool and understandable to a child, works in bright and dark biomes, does not duplicate another owner, adapts to desktop/mobile, avoids literal copying, and supports gameplay truth.

## V23R19Q professional memory-handheld finish

The menu device should evoke the emotional memory of a classic handheld rather than reproduce exact old hardware.

- Preserve the chunky, tactile, screen-centered silhouette.
- Modernize proportions, spacing, glass depth, material separation, status lighting and interaction response.
- Keep the device original: no copied logo, exact commercial shell, button layout measurement or branded industrial detail.
- All menu pages remain inside one screen.
- The BBH boot sequence keeps its content and timing but may gain restrained screen-surface depth, scanlines, glow and vignette.
- Nostalgia must not reduce readability, resolution, touch-target size or accessibility.
- Professional finish means consistent hierarchy, aligned spacing, controlled color, subtle motion, cached rendering resources and no prototype-looking flat rectangles.

<!-- B&D MODERN 3D HANDHELD ART DIRECTION START -->
## Approved upright 3D handheld menu device

The approved Main and Escape/Pause presentation is no longer satisfied by a flat illustrated frame. The device must be a real three-dimensional upright handheld with a screen in the upper half and physical controls below.

- Use an original portrait handheld silhouette inspired by remembered retro hardware, not a commercial replica.
- The molded-plastic shell transitions from rich blue on the left through violet/indigo to warm orange on the right.
- Model real thickness, rounded edges, screen recess, bezel, speaker perforations and material separation.
- The display sits behind a separate glossy glass/transparent-plastic cover with visible edge thickness, controlled reflection and a shallow physical gap.
- D-pad, A/B/X/Y, Settings and Progression are separate tactile 3D parts with visible press/release response.
- Left center shortcut: `SETTINGS`. Right center shortcut: `PROGRESSION`.
- A = Select/Confirm; B = Back; X = Settings; Y = Progression.
- Mouse and D-pad navigation share one visual focus language.
- Use the single-line label `Progression`; do not use `Meta Progression` in this redesigned interface.
- Main, Settings, Progression, Pause, Abandon and Loading share the same device, screen-material and interaction language.
- Concept-art data that is not supported by real game systems must be omitted rather than fabricated.

### Boy/Girl image parity

Every interface/reference/promotional image that depicts the Boy must ship with a matched Girl version in the same asset change. The pair keeps identical dimensions, crop, camera, background, lighting, horse pose, safe areas, grading and import settings. Runtime shows the variant matching the active playable character. A Boy image without its Girl pair fails the visual acceptance gate.

Full specification: `Assets/_Project/Design/UI/MODERN_HANDHELD_3D_ASSET_AND_INTERACTIVE_UI_SPEC_V1.md`.
<!-- B&D MODERN 3D HANDHELD ART DIRECTION END -->
