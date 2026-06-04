# Boredom & Dungeons — Clean Core V52

זו גרסה נקייה מלאה להחלפה, לא פאטץ׳.

## חדש ב־V52 — Center Mouse Dead Zone

נוסף רדיוס קטן סביב השחקן שבו העכבר לא משפיע על הכיוון.

## למה

ב־V51 היה Front Dead Zone של 30°, אבל עדיין אם העכבר היה ממש על הגוף של השחקן או קרוב אליו, הכיוון היה יכול להשתנות בצורה לא טבעית.

ב־V52 נוסף גם:

```text
Center mouse dead zone radius
```

כלומר:

```text
אם העכבר בתוך רדיוס קטן סביב השחקן:
    לא משנים כיוון
    נשארים בכיוון הנוכחי
```

הרדיוס מכוון בערך ל:

```text
קוטר מודל השחקן + קצת
```

## ערכים

```text
Player center dead zone radius = 1.15 world units
Combat center dead zone radius = 1.15 world units
Mounted horse center dead zone radius = 1.35 world units
```

## שילוב עם Front Dead Zone

עכשיו יש שני מנגנונים יחד:

```text
1. Center dead zone:
   העכבר קרוב מדי לגוף → לא משפיע

2. Front dead zone:
   העכבר בתוך 30° קדימה → לא משנה כיוון
```

רק אם העכבר גם מחוץ לרדיוס הקרוב וגם מחוץ ל־30° הקדמי — הכיוון מתעדכן.

## חל על

```text
Player movement
Player idle rotation
Sword / Heavy attack
Q ranged attack
Mounted horse movement
Mounted horse idle rotation
Mounted Q ranged attack
Camera behind direction
```

## נשמר מ־V51

- Front mouse dead zone של 30°.
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
