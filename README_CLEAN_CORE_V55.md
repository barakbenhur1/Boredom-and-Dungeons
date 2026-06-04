# Boredom & Dungeons — Clean Core V55

זו גרסת תיקון קומפילציה נקייה אחרי V54.

## מה תוקן

ב־V54 היו שגיאות:

```text
BDHorseController does not contain a definition for Rider
```

הסיבה:
- `BDPlayerHealingPickup` עודכן כדי לאפשר איסוף רפואה בזמן רכיבה.
- הוא ניסה לקרוא ל־`horse.Rider`.
- אבל `BDHorseController` עדיין לא חשף את ה־rider כ־public property.

ב־V55 נוסף:

```csharp
public Transform Rider => rider;
```

## בנוסף

ה־warnings הלא קריטיים של שדות mounted mouse yaw ישנים ב־`BDHorseController` הושתקו, כי הם נשארו כתשתית/legacy אבל לא משמשים במודל השליטה הנוכחי.

## נשמר מ־V54

- Center mouse dead zone מוגדל:
  - Player: 1.85
  - Combat: 1.85
  - Horse: 2.20
- Front dead zone = 8°.
- אפשר לאסוף Healing Pickup גם על גב הסוס.
- הריפוי עדיין מרפא את השחקן בלבד.
- Mounted ranged projectile hit fixes.
- W/S/A/D relative movement.
- Free cursor mouse point aim.
- הסוס חוזר רק עד רדיוס נוחות מהשחקן.
- Dodge ב־Double Tap.
- Minimap בצד ימין למטה.
- Ranged Attack עם מחסנית 3 יריות ו־reload של 6 שניות.
- Healing drop של 16% מאויבים.
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
