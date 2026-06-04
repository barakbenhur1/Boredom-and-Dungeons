# Boredom & Dungeons — Clean Core V63

זו גרסה נקייה מלאה להחלפה, לא פאטץ׳.

## מה תוקן ב־V63

### Movement Smoothing

ב־V62 הכיוון כבר היה הדרגתי, אבל התנועה עצמה עדיין יכלה להרגיש קטועה:

```text
Input pressed   → מיד מהירות מלאה
Input released  → מיד עצירה
Direction change → שינוי חד מדי
```

ב־V63 נוספה החלקת תנועה:

```text
Desired movement direction
→ acceleration / deceleration
→ smoothed horizontal velocity
→ CharacterController.Move
```

## ערכי ברירת מחדל

### Player

```text
Move acceleration = 34
Move deceleration = 42
```

### Horse

```text
Mounted acceleration = 24
Mounted deceleration = 30
```

## התוצאה הרצויה

```text
התנועה מתחילה חלק
עצירה לא נחתכת בבת אחת
שינויי כיוון מרגישים רציפים
התחושה פחות קטועה ופחות רובוטית
```

## נשמר מ־V62

- דיליי קצר:
  - Player = 0.020s
  - Horse = 0.025s
- מערכת ניווט אחידה.
- Facing aim הדרגתי.
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
