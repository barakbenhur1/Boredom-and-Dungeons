# Boredom & Dungeons — Clean Core V54

זו גרסה נקייה מלאה להחלפה, לא פאטץ׳.

## מה תוקן ב־V54

### Center Mouse Dead Zone גדול יותר

ב־V53 הרדיוס סביב השחקן שבו העכבר לא משנה כיוון היה עדיין קטן מדי.

ב־V54 הרדיוס הורחב לפחות פי 1.6:

```text
Player center dead zone radius: 1.15 → 1.85
Combat center dead zone radius: 1.15 → 1.85
Mounted horse center dead zone radius: 1.35 → 2.20
```

## התנהגות

```text
אם העכבר בתוך הרדיוס סביב הגוף:
    לא משנים כיוון
    נשארים בכיוון הנוכחי
    התקפה/ירי נשארים בכיוון הנוכחי

אם העכבר מחוץ לרדיוס:
    הכיוון יכול להתעדכן לפי העכבר
```

## נשמר מ־V53

- Front dead zone ירד ל־8°.
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

אם Unity עדיין מציג “All compiler errors have to be fixed”, צריך לפתוח Console ולשלוח את השורות האדומות עצמן, כי ההודעה הזאת היא רק הודעת חסימה כללית ולא מפרטת את השגיאה.
