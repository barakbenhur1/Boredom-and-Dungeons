# Boredom & Dungeons — Clean Core V56

זו גרסה נקייה מלאה להחלפה, לא פאטץ׳.

## מה תוקן ב־V56

### 1. Front Dead Zone קטן יותר

הזווית הקדמית ירדה מ־8 מעלות ל־3 מעלות:

```text
Front dead zone: 8° → 3°
```

כלומר רק כשהעכבר ממש כמעט ישר מקדימה, הכיוון לא משתנה.

חל על:

```text
Player
Horse
Sword / Heavy
Q ranged attack
Camera direction
```

### 2. העכבר נהיה הדרגתי

ב־V55 הכיוון עדיין היה חד מדי: כאשר העכבר יצא מהרדיוס/זווית, הכיוון הלוגי היה יכול לקפוץ מיד.

ב־V56 הכיוון משתנה בהדרגה:

```text
Mouse target direction = raw mouse point
Gameplay aim direction = gradual turn toward target
Body rotation = visual smoothing toward gameplay aim
Camera = follows gameplay aim
```

כלומר:

```text
העכבר לא מקפיץ את השחקן מיד
הכיוון מסתובב בהדרגה לכיוון העכבר
התנועה/מכה/ירי משתמשים בכיוון ההדרגתי
```

## ערכים חדשים

```text
Player aim turn speed = 260° per second
Horse aim turn speed = 220° per second
Front dead zone = 3°
Player center dead zone = 1.85
Horse center dead zone = 2.20
```

## נשמר מ־V55

- `BDHorseController.Rider` compile fix.
- אפשר לאסוף Healing Pickup על גב הסוס.
- Center mouse dead zone מוגדל.
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
