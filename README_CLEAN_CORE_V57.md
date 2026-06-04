# Boredom & Dungeons — Clean Core V57

זו גרסה נקייה מלאה להחלפה, לא פאטץ׳.

## מה תוקן ב־V57 — מערכת ניווט מקצועית ואחידה

בגרסאות הקודמות כמה מערכות חישבו כיוון בנפרד:

```text
Player movement
Player rotation
Combat aim
Ranged aim
Horse movement
Camera
```

זה יצר תחושה לא טבעית, כי לפעמים התנועה, המכה, הירי והמצלמה לא השתמשו בדיוק באותו כיוון.

ב־V57 המודל אחיד:

```text
Mouse → Ground Aim Target
Dead Zones → Stable Aim Target
Facing Aim → Gradual turn toward target
Movement → Relative to Facing Aim
Combat → Uses same Facing Aim
Camera → Uses same Facing Aim
```

## מודל השליטה

### כשלא זזים

```text
העכבר קובע יעד כיוון
המודל מסתובב בהדרגה לכיוון היעד
השחקן לא זז
מכה/ירי משתמשים בכיוון הנוכחי של המודל
```

### כשזזים

```text
W / Up      = קדימה ביחס לכיוון הנוכחי
S / Down    = אחורה
A / Left    = שמאלה
D / Right   = ימינה
```

הכיוון הנוכחי מסתובב בהדרגה לכיוון העכבר, כך שאין קפיצה חדה ואין תחושת עכבר עצבנית.

## Dead zones

נשמרו שני ה־dead zones:

```text
Center dead zone:
    Player = 1.85
    Horse = 2.20

Front dead zone:
    3°
```

## Turn rates

```text
Player idle aim turn = 220°/sec
Player moving aim turn = 320°/sec

Horse idle aim turn = 180°/sec
Horse moving aim turn = 260°/sec
```

כלומר בזמן תנועה הכיוון מגיב מהר יותר, ובזמן idle הוא רגוע יותר.

## Combat/Ranged

ב־V57 התקפות כבר לא מחשבות כיוון משלהן מהעכבר.  
הן משתמשות בכיוון ה־Facing Aim המרכזי:

```text
Sword / Heavy / Q = current facing aim
```

זה גורם למכה ולירי להרגיש מחוברים לגוף ולא “לקפוץ” לכיוון אחר.

## נשמר

- Healing Pickup נאסף גם על גב הסוס.
- Mounted ranged projectile hit fixes.
- Projectile capsule sweep.
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
