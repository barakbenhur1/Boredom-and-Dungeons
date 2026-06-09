# Boy/Girl Character Artwork Parity

The parity rule applies whenever an image actually depicts the playable protagonist. Any Boy image must have a composition-matched Girl version in the same change, and vice versa.

## Current handheld scope

Only Start Game / New Run is allowed to depict the playable Boy or Girl. That option requires the matched pair and deterministic active-character selection.

The following contexts must be character-neutral and use one shared asset each:

- Resume / Pause;
- Progression;
- Settings;
- Credits;
- Quit / Return to Main Menu;
- abandon confirmation;
- any other menu option that does not need to depict the selected playable character.

Do not create redundant Boy/Girl copies of neutral artwork. If a neutral image accidentally includes either protagonist, it becomes paired artwork and blocks completion until the counterpart exists or the protagonist is removed.

## Pair production contract

A real pair must match in dimensions, crop, camera, composition, background, lighting, horse pose when present, grading, safe areas, file format, naming relationship and import settings. Only the playable character changes.

Recommended names:

```text
<AssetName>_Boy_V###
<AssetName>_Girl_V###
```

## Runtime selection contract

The active playable character is authoritative only where paired protagonist art is required:

- Boy route / active Boy identity → Boy New Game asset;
- Girl route / active Girl identity → Girl New Game asset.

Selection is never random. A saved preference may be used only when no active player identity can be resolved, and it may not override a detected active character.

`BDPlayableCharacterIdentity` is the Runtime bridge. Future character selection must set or serialize that identity rather than manually swapping individual images. Scene reload and page changes must not show one stale frame of the other route.

For the modern handheld, `BDModernHandheld3DPresenter.ResolveContextArtwork` owns the final image decision. It consults `BDPlayableCharacterIdentity` only for Start Game / New Run. Progression, Settings, Credits, Quit/Return, Resume/Pause and confirmation must resolve to character-neutral assets.

## New Game card clarification

The small New Game memory card does not depict a character and therefore has no Boy/Girl pair. It is text-only, appears only for a fresh Start Game/New Run selection, and must not mention Boy/Girl route or Mother state. The large New Game hero image remains the sole paired character artwork on that screen.
