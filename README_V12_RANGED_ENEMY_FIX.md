# Boredom & Dungeons — V12 Ranged Enemy Fix

תיקון חשוב: ב־V11 היה חסר האויב שתוקף מרחוק.

V12 מוסיף:

- Ranged Shooter Enemy
- קליע / Projectile
- האויב שומר מרחק מהשחקן
- יורה כל כמה זמן
- אם השחקן מתקרב מדי, האויב נסוג
- אם הקליע פוגע בשחקן, יורדים חיים
- הסצנה כוללת עכשיו 5 archetypes:
  - Sword Fighter
  - Charger
  - Patrol Guard
  - Trap/Bomb Layer
  - Ranged Shooter

## התקנה

1. העתק את `Assets/_Project` לתוך Unity.
2. חכה ל־Compile.
3. בתפריט לחץ:

```text
Boredom And Dungeons V12/Create Ranged Enemy Test Scene
```

4. לחץ Play.
5. לחץ בתוך Game/Simulator.

## שליטה

- WASD / Arrows — תנועה
- Space — קפיצה
- Left Shift — Dash
- C — Toggle Camera Follow
- Left Mouse / J — התקפה מהירה
- Right Mouse / K — התקפה חזקה

## מה לבדוק

1. האויב החדש `BD_V12_Enemy_RangedShooter` עומד במרחק.
2. הוא יורה קליעים לעבר השחקן.
3. אם הקליע פוגע בשחקן — יורדים חיים.
4. אם מתקרבים אליו, הוא מנסה לסגת.
5. אפשר להרוג אותו עם התקפות חרב.
6. שאר האויבים עדיין קיימים בסצנה.

## הערה

מניח פצצות הוא לא אויב ranged.  
הוא אויב area denial / hazard control.  
האויב החדש הוא האויב שתוקף מרחוק בפועל.
