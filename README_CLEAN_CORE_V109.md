# Boredom & Dungeons — Clean Core V109

זו גרסת תיקון מצלמה אחרי V108.

## מה תוקן

הבעיה לא הייתה העכבר ולא הייתה בעיקר זווית ההטיה.  
הבעיה הייתה שלא רואים מספיק מהמפה קדימה.

ב־V109 התיקון הוא **map visibility / zoom-out framing**:

```text
distanceBehind = 15.25
height = 17.75
lookAhead = 6.75
Camera fieldOfView = 58
```

ה־pitch לא ממשיך לעלות עוד:

```text
minPitch = 50
maxPitch = 68
```

## מה זה אמור לעשות

```text
רואים יותר מהמפה בפריים
האופק/המרחב קדימה מרגיש רחוק יותר
רואים יותר חדר לפני שנכנסים אליו
רואים יותר מסדרון קדימה
התחושה פחות צפופה
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
לא שונה Game Boy cinematic
```

## נשמר מ־V108

- Game Boy collectible.
- Battery collectibles.
- Dark room cinematic.
- Player sits on chair, inserts batteries, plays Game Boy.
- Natural Map Shape Pass.
- Enemy hit flash safe restore.
- Enemy stagger gates attacks.
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

אחר כך:

```text
Stop Play Mode
Run Create Clean Maze Prototype Scene
Enter Play Mode
```

בדיקה מומלצת:

```text
הליכה במסדרון
כניסה לחדר גדול
קרב בחדר
רכיבה על סוס
חדר ה־Game Boy
לוודא שרואים יותר מהמפה ולא רק שהזווית השתנתה
```
