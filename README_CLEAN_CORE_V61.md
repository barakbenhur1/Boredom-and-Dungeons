# Boredom & Dungeons — Clean Core V61

זו גרסה נקייה מלאה להחלפה, לא פאטץ׳.

## מה תוקן ב־V61

### דיליי קטן בין העכבר לבין תגובת הדמות

נוסף דיליי מאוד קטן ומבוקר לכיוון העכבר:

```text
Player mouse aim response delay = 0.045s
Mounted horse mouse aim response delay = 0.055s
```

## למה

ב־V60 הכיוון כבר היה אחיד ונכון, אבל התגובה עדיין יכלה להרגיש קצת מיידית/עצבנית.

ב־V61:

```text
Mouse target direction
→ tiny response delay
→ gradual facing aim
→ movement / attack / ranged / camera
```

הדיליי לא אמור להרגיש כמו input lag כבד.  
הוא רק נותן לדמות מעט משקל לפני שהיא מתחילה להגיב לכיוון חדש.

## נשמר מ־V60

- ירי על הסוס משתמש קודם ב־Horse.LastMountedAimDirection.
- רגישות עכבר מעט נמוכה יותר.
- Safe projectile material fallback.
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
