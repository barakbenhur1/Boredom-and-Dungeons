# Boredom & Dungeons — Clean Core V46

זו גרסה נקייה מלאה להחלפה, לא פאטץ׳.

## מה תוקן ב־V46 — Mouse Look פחות sluggish

ב־V45 העכבר היה יציב, אבל כבד מדי.  
ה־yaw עצמו עבר smoothing, ולכן הכיוון הרגיש מאוחר.

ב־V46:

```text
Mouse delta changes yaw immediately
Movement uses the immediate yaw direction
Body rotation smooths visually
Camera follows faster
```

כלומר השליטה עצמה מגיבה מיד, אבל הסיבוב/מצלמה עדיין נשארים חלקים.

## ערכים חדשים

```text
Mouse yaw sensitivity: 0.68 degrees per pixel
Mouse max delta per frame: 160
Mouse yaw smoothing: immediate
Body rotation smoothing: 55
Camera follow smoothing: 24
Camera rotation smoothing: 32
```

גם על הסוס:

```text
Mounted mouse yaw sensitivity: 0.68
Mounted yaw: immediate
Horse rotation speed: faster
```

## נשמר

- קומפילציה נקייה אחרי תיקוני V45.
- הסוס חוזר רק עד רדיוס נוחות מהשחקן.
- AI אויבים עם discipline בסיסי נגד dogpile.
- מצלמה מאחורי השחקן.
- Dodge ב־Double Tap.
- Minimap בצד ימין למטה.
- חדר ההתחלה בלי אויבים.
- Ranged Attack עם מחסנית 3 יריות ו־reload של 6 שניות.
- Healing drop של 16% מאויבים.
- ריפוי סוס עם `F`.
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
