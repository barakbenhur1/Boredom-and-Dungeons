# Boredom & Dungeons — Clean Core V64

זו גרסה נקייה מלאה להחלפה, לא פאטץ׳.

## מה תוקן ב־V64

### 1. שינוי הכיוון לפי העכבר רציף יותר

ב־V63 היה movement smoothing, אבל שינוי הכיוון לפי העכבר עדיין יכול היה להרגיש קטוע בגלל מנגנון delay קטן:

```text
Mouse changes
→ wait tiny delay
→ update direction
```

זה נתן תחושה של micro-steps.

ב־V64 זה הוחלף ל־continuous aim smoothing:

```text
Raw mouse direction
→ continuous smoothed target direction
→ gradual facing aim
→ movement / attack / ranged / camera
```

כלומר אין “מדרגות” בדיליי. הכיוון מתעדכן רציף בכל frame.

## ערכי Aim חדשים

```text
Player aim target smoothing = 26
Horse aim target smoothing = 22
Player idle turn = 185°/sec
Player moving turn = 275°/sec
Horse idle turn = 150°/sec
Horse moving turn = 220°/sec
```

### 2. אופטימיזציה

תוקנו מוקדי האטה עיקריים:

```text
Debug OnGUI overlays off by default
BDTargetFinder caches player lookup
BDMouseAimUtility uses RaycastNonAlloc instead of RaycastAll allocations
BDEnemyGroundStick uses NonAlloc physics queries
BDPlayerRangedProjectile uses NonAlloc capsule hit buffer
BDRangedProjectile uses NonAlloc overlap buffer
```

זה אמור להפחית GC spikes ולהוריד עומס בזמן שיש הרבה אויבים/יריות.

## נשמר מ־V63

- Movement smoothing:
  - Player acceleration/deceleration
  - Horse acceleration/deceleration
- מערכת ניווט אחידה.
- Movement / attack / ranged / camera משתמשים באותו כיוון.
- ירי על הסוס משתמש ב־Horse.LastMountedAimDirection.
- Safe projectile material fallback.
- Front dead zone = 3°.
- Center dead zone:
  - Player = 1.85
  - Horse = 2.20
- אפשר לאסוף Healing Pickup על גב הסוס.
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
