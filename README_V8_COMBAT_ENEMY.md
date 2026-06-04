# Boredom & Dungeons — V8 Combat + First Sword Enemy

הגרסה הזאת מוסיפה את השלב הבא אחרי שהתנועה ב־V7 עובדת:

- שחקן עם תנועה
- מצלמה
- ספירת אור לבנה
- מערכת חיים
- התקפה מהירה
- התקפה חזקה
- אויב חרב ראשון
- ספירה שחורה לאויב
- אויב רודף אחרי השחקן
- אויב תוקף מקרוב
- Debug overlay לקרב

## התקנה

1. העתק את `Assets/_Project` לתוך Unity.
2. חכה ל־Compile.
3. בתפריט לחץ:

```text
Boredom And Dungeons V8/Create Combat Enemy Test Scene
```

4. לחץ Play.
5. לחץ בתוך Game/Simulator.

## שליטה

תנועה:
- W / Up Arrow = קדימה
- S / Down Arrow = אחורה
- A / Left Arrow = שמאלה
- D / Right Arrow = ימינה

תנועה מיוחדת:
- Space = Jump
- Left Shift = Dash
- C = Toggle Camera Follow

קרב:
- Left Mouse / J = Fast Attack
- Right Mouse / K = Heavy Attack

Debug:
- P = פגיעה עצמית לשחקן לצורך בדיקה

## מה לבדוק

1. השחקן זז.
2. האויב רודף אחרי השחקן.
3. אם האויב קרוב, הוא תוקף ומוריד חיים.
4. Left Mouse / J פוגע באויב בטווח קצר.
5. Right Mouse / K פוגע חזק יותר אבל איטי יותר.
6. כשהאויב מת הוא נעלם.
7. הספירה השחורה סביב האויב נראית.
8. הספירה הלבנה סביב השחקן נראית.

## הערה

זו עדיין מערכת combat ראשונית.  
אין עדיין אנימציות אמיתיות, אין סוס, אין מבוך.  
המטרה של V8 היא לוודא שהקרב הבסיסי עובד לפני שממשיכים.
