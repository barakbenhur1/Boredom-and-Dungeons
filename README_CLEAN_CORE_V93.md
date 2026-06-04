# Boredom & Dungeons — Clean Core V93

זו גרסת תיקון כיוון מודל אחרי V92.

## מה תוקן ב־V93

### העכבר כן מסובב את המודל — אבל רק בקדימה

ב־V88/V92 הפרדנו בין תנועה לבין עכבר:

```text
WASD / arrows → תנועה
Mouse          → aim
```

אבל עכשיו נוסף גבול נכון לכיוון המודל:

```text
Mouse can rotate model only inside a 60° front cone
```

## ההתנהגות עכשיו

```text
WASD / arrows → תנועה יחסית למצלמה/מסך
Mouse          → משנה את כיוון המודל/מכה/ירי
אבל רק בתוך קונוס קדמי של 60 מעלות
```

כלומר:
- העכבר לא מסובב את הדמות לאחור.
- העכבר לא שובר את כיוון ההליכה לצדדים חדים.
- המודל עדיין מגיב לעכבר בתוך אזור קדמי.
- מכה/ירי משתמשים בכיוון המודל המוגבל.

## גם על הסוס

בזמן רכיבה:

```text
Mouse aim על הסוס מוגבל לאותו front cone של 60°
Q עדיין יורה לפי הכיוון המוגבל
תנועה עדיין לא תלויה בעכבר
חרב עדיין חסומה על הסוס
```

## ערכים

```text
mouseModelFrontConeDegrees = 60
mountedMouseModelFrontConeDegrees = 60
mouseFrontDeadZoneDegrees = 3
mountedMouseFrontDeadZoneDegrees = 3
```

ה־60 הוא קונוס קדמי כולל, כלומר בערך 30° לכל צד מהקדימה.

## נשמר מ־V92

- סוס פונה לשחקן רק כשהוא בא אליו.
- שחקן שמתקרב לסוס לא משנה לסוס תנוחה.
- חרב לא מפספסת אויבים צמודים.
- התחמקות לפי הכפתור שנלחץ ולא לפי העכבר.
- Dodge effect הוא 2D ground trail.
- חיווי שאפשר לרפא את הסוס.
- קליעים פוגעים בקירות.
- Performance Guard.
- Enemy attack telegraphs.
- Ranged muzzle flash/trail/impact.
- Melee slash arcs.
- Enemy HP bars.
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

חכה ל־Compile מלא ואז בדוק:
- תנועה קדימה עם W והזזת עכבר לצדדים.
- שהמודל מסתובב רק מעט ימינה/שמאלה ולא לאחור.
- ירי/מכה באותו כיוון מוגבל.
- אותו הדבר על הסוס עם Q.
