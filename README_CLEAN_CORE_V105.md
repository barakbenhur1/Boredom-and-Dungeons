# Boredom & Dungeons — Clean Core V105

זו גרסת תיקון מיידית אחרי V104.

## מה תוקן

לא שיניתי את העכבר.

התיקון הוא למצלמה:  
השחקן לא ראה מספיק רחוק קדימה במסך.

ב־V105 המצלמה מראה יותר קדימה:

```text
distanceBehind = 11.25
height = 13.25
lookAhead = 4.75
minPitch = 46
maxPitch = 64
```

## מה זה עושה

```text
רואים יותר שטח קדימה
קל יותר להבין לאן הולכים
קל יותר לראות אויבים/מסדרון/חדר קדימה
השחקן עדיין נשאר קריא
```

## מה לא השתנה

```text
לא שונה העכבר
לא שונה mouseModelFrontConeDegrees
לא שונה mountedMouseModelFrontConeDegrees
לא שונה W/S/A/D
לא שונה aim
לא שונה melee
לא שונה ranged
לא שונה AI
לא שונה HUD
לא שונה המפה
```

## נשמר מ־V104

- Object ambiguity compile fix.
- Natural Map Shape Pass.
- Professional project structure.
- Enemy hit flash safe restore.
- Enemy stagger gates attacks.
- Enemy micro-stagger.
- Soft melee aim assist.
- Dodge camera yaw lock.
- Mounted melee disabled.
- Projectile wall collision.

## התקנה נקייה

מחק לגמרי:

```text
Assets/_Project
```

ואז העתק מה־ZIP את:

```text
Assets/_Project
```

בדיקה מומלצת:

```text
הליכה קדימה במסדרון
כניסה לחדר גדול
קרב בחדר
רכיבה על סוס
Dodge אחורה/צדדים
לוודא שרואים רחוק יותר קדימה בלי שהמצלמה מרגישה רחוקה מדי
```
