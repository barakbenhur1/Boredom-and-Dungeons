# Boredom & Dungeons — Clean Core V85

זו גרסת תיקון קומפילציה נקייה אחרי V84.

## מה תוקן

ב־V84 הופיעה שגיאת קומפילציה:

```text
Assets/_Project/Scripts/Runtime/BDEnemyLootDropper.cs(44,35):
error CS0117: 'BDPlayerHealingPickup' does not contain a definition for 'Spawn'
```

הסיבה:
- `BDEnemyLootDropper` עדיין קרא ל־`BDPlayerHealingPickup.Spawn(...)`.
- בגרסאות האחרונות `BDPlayerHealingPickup` שודרג ל־pulse/float/magnet/collect burst.
- ה־static `Spawn` הוסר בטעות.

## התיקון ב־V85

הוחזרו overloads תואמים:

```text
BDPlayerHealingPickup.Spawn(Vector3 position)
BDPlayerHealingPickup.Spawn(Vector3 position, float healFraction)
BDPlayerHealingPickup.Spawn(Vector3 position, float healFraction, float scale)
```

ה־Spawn החדש יוצר pickup עם:
- sphere pickup ירוק
- trigger collider
- BDPlayerHealingPickup
- pulse / float / magnet
- collect burst
- 40% heal כברירת מחדל

## נשמר מ־V84

- קליעים פוגעים בקירות ולא עוברים דרכם.
- Camera occlusion polish.
- Performance Guard.
- Enemy attack telegraphs.
- Ranged muzzle flash/trail/impact.
- Melee slash arcs.
- Player low-health / damage screen feedback.
- Enemy world HP bars.
- Procedural game-feel audio.
- Hit-stop לפגיעות.
- Healing pickup pulse/float/magnet/collect burst.
- Dodge i-frames.
- Enemy death burst.
- Heavy attack impact + knockback.
- Enemy separation.
- Q ammo/reload HUD.
- Death reset.

## התקנה נקייה

מחק לגמרי:

```text
Assets/_Project
```

ואז העתק מה־ZIP את:

```text
Assets/_Project
```

חכה ל־Compile מלא.
