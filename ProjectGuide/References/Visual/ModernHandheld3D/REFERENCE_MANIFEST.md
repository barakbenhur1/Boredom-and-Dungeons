# Modern Handheld 3D Reference Manifest

Status: approved visual references for `C11.UI.MODERN_HANDHELD_3D.V1`.

These files define form, material, screen depth, physical controls, menu composition and approved source surfaces. Product renders remain references. The orthographic texture sheet and wood source are approved production inputs only through the masking/material rules below; neither may flatten the live 3D device or bake menu behavior into a screenshot.

## Files

- `HANDHELD_3D_FRONT_GIRL_V1.png` — front-facing product/form reference.
- `HANDHELD_3D_THREE_QUARTER_GIRL_V1.png` — thickness, bevel, side profile and layered-screen reference.
- `HANDHELD_3D_MATERIAL_CLOSEUP_GIRL_V1.png` — tactile button, plastic, perforation, glass and surface-material reference.
- `HANDHELD_MENU_BOY_V1.png` — Boy-and-horse main-menu art/layout variant.
- `HANDHELD_MENU_GIRL_V1.png` — Girl-and-horse main-menu art/layout variant.
- `HANDHELD_ORTHOGRAPHIC_TEXTURE_SHEET_V1.png` — user-supplied multi-view/front device source. It guides color, ornament and proportions; Runtime does not paste it as a full-face decal and keeps screen/controls as real independent geometry.
- `HANDHELD_TABLE_DARK_WOOD_SOURCE_V1.png` — exact user-supplied table source. Runtime sharp/defocused variants preserve this crop and color.

## Character parity rule

Every reference or production image that depicts the Boy requires a matched Girl version with the same dimensions, crop, composition, lighting, horse pose, background, safe areas and import settings. The pair in this folder establishes the naming and parity expectation. Future pairs use `<AssetName>_Boy_V###` and `<AssetName>_Girl_V###`.

## Approved non-literal direction

- original upright handheld, not a commercial-device replica;
- real 3D shell and controls;
- blue-to-orange molded-plastic gradient;
- screen recessed behind clear glass/transparent plastic with depth;
- SELECT and EXIT center buttons with equal-size recessed labels;
- Main Menu X = New Game, A = Progression, B = Settings, Y = Credits; non-main B = Back; center SELECT activates; center EXIT confirms exit;
- mouse and D-pad navigation;
- user-facing label `Progression`, never `Meta Progression` in this redesigned UI.


## Production-use limits

- The orthographic sheet may contribute shell stickers/decals and material detail only. It never replaces shell volume, glass, display or modeled controls.
- The screen region is cut out and displays live UI through the RenderTexture.
- D-pad, A/B/X/Y, SELECT and EXIT areas remain separate so the 3D interactive parts stay visible and clickable.
- The wood source is used for both sharp and defocused matching textures; focus falloff is performed by the table material, not by replacing the entire scene with one blurred image.
