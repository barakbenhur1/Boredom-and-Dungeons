# Boredom & Dungeons — Clean Core V60

זו גרסה נקייה מלאה להחלפה, לא פאטץ׳.

## מה תוקן ב־V60

### ירי על הסוס יצא לכיוון לא נכון

ב־V59 הירי השתמש במערכת ה־Facing Aim המרכזית, אבל ב־`BDPlayerCombat` סדר העדיפויות היה בעייתי:

```text
Player LastLookDirection
ורק אחר כך
Horse LastMountedAimDirection
```

בזמן רכיבה, `BDPlayerController.LastLookDirection` יכול להיות ישן/לא פעיל, ולכן הירי על הסוס יצא לפעמים לכיוון לא נכון.

ב־V60 הסדר תוקן:

```text
אם השחקן רוכב:
    השתמש קודם ב־Horse.LastMountedAimDirection

אם לא רוכב:
    השתמש ב־Player.LastLookDirection

Fallback:
    transform.forward
```

## התוצאה

```text
Q על הסוס = ירי לפי כיוון הסוס/רוכב הנוכחי
הקליע יוצא מאותו Facing Aim שהמצלמה והתנועה משתמשות בו
אין שימוש בכיוון ישן של השחקן בזמן רכיבה
```

## נשמר מ־V59

- רגישות עכבר מעט נמוכה יותר.
- תיקון safe projectile material fallback.
- מערכת ניווט אחידה:
  - Mouse target
  - Dead zones
  - Facing aim הדרגתי
  - Movement relative to facing aim
  - Combat/Ranged same facing aim
  - Camera same facing aim
- Front dead zone = 3°.
- Center dead zone:
  - Player = 1.85
  - Horse = 2.20
- אפשר לאסוף Healing Pickup על גב הסוס.
- Mounted ranged projectile hit fixes.
- Projectile capsule sweep.
- הסוס חוזר רק עד רדיוס נוחות מהשחקן.
- Dodge ב־Double Tap.
- Minimap בצד ימין למטה.
- Ranged Attack עם מחסנית 3 יריות ו־reload של 6 שניות.
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
