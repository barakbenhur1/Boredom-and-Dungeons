# Repository Map

## Modern handheld visual assets

- `Assets/_Project/Resources/ModernHandheld/Textures/HANDHELD_SHELL_GRADIENT_V1.png` — premium 2048px molded-shell source.
- `Assets/_Project/Resources/ModernHandheld/UI/HANDHELD_HERO_BOY_V1.png` / `HANDHELD_HERO_GIRL_V1.png` — paired New Game protagonist art only.
- `Assets/_Project/Resources/ModernHandheld/UI/HANDHELD_ART_*_V1.png` — shared character-neutral option/page art.
- `Assets/_Project/Shaders/BDModernHandheldSurface.shader` — molded object-gradient shell material with microdetail/specular response.
- `Assets/_Project/Shaders/BDModernHandheldShadow.shader` — cached product-shot penumbra/core/contact shadow compositing.
- `Assets/_Project/Shaders/BDModernHandheldGlass.shader` — transparent glass with restrained upper-right directional glint.

```text
Assets/                 Unity runtime/editor content only
Packages/               Unity package manifest and lock
ProjectSettings/        Unity project configuration
ProjectGuide/           all maintained rules, status, plans, QA and specifications
.codex/                 Codex project configuration and specialist profiles
tools/                  repository validation and maintenance tools
AGENTS.md               concise AI/Codex operating contract
README.md               public entry point
```

Project knowledge is intentionally outside `Assets/` to avoid Unity importing planning and reference files as runtime assets.
