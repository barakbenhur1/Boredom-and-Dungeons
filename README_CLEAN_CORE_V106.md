# Boredom & Dungeons — Clean Core V106

זו גרסת תיקון מצלמה אחרי V105.

## מה תוקן

זווית ההטיה של המצלמה עכשיו קצת יותר קהה / קצת יותר מלמעלה.

ב־V105:

```text
minPitch = 46
maxPitch = 64
```

ב־V106:

```text
minPitch = 50
maxPitch = 68
```

## מה זה עושה

```text
המצלמה מרגישה קצת יותר מוטה מלמעלה
רואים את המרחב קדימה בצורה נקייה יותר
הקרב אמור להיות קצת יותר קריא
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

## נשמר מ־V105

- יותר ראייה קדימה במסך.
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
לוודא שהזווית מרגישה קצת יותר טובה ולא גבוהה מדי
```
