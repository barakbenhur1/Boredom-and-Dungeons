# Boredom & Dungeons — Clean Core V53

זו גרסה נקייה מלאה להחלפה, לא פאטץ׳.

## מה תוקן ב־V53

### 1. Front Dead Zone קטן יותר

ב־V52 ה־Front Dead Zone היה גדול מדי:

```text
30°
```

ב־V53 הוא ירד ל:

```text
8°
```

כלומר רק כשהעכבר ממש כמעט מול הדמות הכיוון לא משתנה.  
אם העכבר זז קצת יותר הצידה, הכיוון כן יתעדכן.

חל על:

```text
Player
Horse
Sword / Heavy
Q ranged attack
Camera direction
```

### 2. אפשר לאסוף רפואה גם על גב הסוס

ב־V52 ה־Healing Pickup נאסף רק דרך גוף השחקן.

ב־V53:

```text
אם השחקן רוכב על הסוס
והסוס עובר על Healing Pickup
ה־Pickup נאסף
והריפוי הולך לשחקן
```

חשוב:

```text
הריפוי עדיין לא מרפא את הסוס
הריפוי עדיין לא מרפא אויבים
הריפוי הוא עדיין 40% מה־Max HP של השחקן
```

## נשמר מ־V52

- Center mouse dead zone סביב השחקן.
- Movement/attacks use raw mouse aim direction outside dead zones.
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
