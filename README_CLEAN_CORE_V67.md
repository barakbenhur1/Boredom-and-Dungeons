# Boredom & Dungeons — Clean Core V67

זו גרסת תיקון קומפילציה נקייה אחרי V66.

## מה תוקן

ב־V66 הייתה שגיאת קומפילציה:

```text
BDEnemyCollisionDiscipline.cs
CS0246: The type or namespace name 'BDRangedEnemy' could not be found
```

הסיבה:
- בקוד השתמשתי בשם מחלקה שלא קיים בפרויקט:
  `BDRangedEnemy`
- בפועל מחלקת היורה שנמצאה היא:
  `BDRangedShooterEnemy`

ב־V67 זה תוקן.

## נשמר מ־V66

- אויבים פחות עולים אחד על השני.
- Screen-space center dead zone כשהעכבר קרוב לשחקן/סוס.
- Hit feedback.
- Q ammo/reload HUD.
- Continuous mouse aim smoothing.
- Movement smoothing.
- Performance optimizations.
- ירי על הסוס משתמש ב־Horse.LastMountedAimDirection.
- Healing Pickup נאסף גם על גב הסוס.
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
