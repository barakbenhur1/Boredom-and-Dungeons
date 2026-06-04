# Boredom & Dungeons — Clean Core V65

זו גרסה נקייה מלאה להחלפה, לא פאטץ׳.

## מה נוסף ב־V65

### 1. Hit Feedback

נוסף feedback ויזואלי לפגיעה:

```text
Enemy hit → flash קצר
Player hit → flash קצר על המודל + overlay קיים
Horse hit → flash קצר
```

המימוש משתמש ב־`MaterialPropertyBlock` כדי לא ליצור material instances מיותרים.

### 2. HUD ל־Ranged Attack

ה־HUD מציג עכשיו:

```text
Q Ammo: current / max
Reload bar
Reload remaining seconds
```

זה עוזר להבין מתי יש 3 יריות, מתי נגמרו, ומתי הטעינה חוזרת.

### 3. אופטימיזציה קטנה לקרב

ה־melee attack עבר מ:

```text
Physics.OverlapSphere
```

ל:

```text
Physics.OverlapSphereNonAlloc
```

כדי להפחית allocations בזמן מכות.

## נשמר מ־V64

- Continuous mouse aim smoothing.
- Movement smoothing.
- Performance optimizations.
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
