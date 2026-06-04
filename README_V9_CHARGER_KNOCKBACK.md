# Boredom & Dungeons — V9 Charger Enemy + Knockback

הגרסה הזאת ממשיכה את V8 ומוסיפה:

- אויב ניגוח / Charger
- Windup לפני הסתערות
- Charge מהיר קדימה
- פגיעה בשחקן בזמן ניגוח
- Knockback בסיסי כשפוגעים באויב
- אויב חרב נשאר קיים
- סצנת בדיקה חדשה עם שני אויבים

## התקנה

1. העתק את `Assets/_Project` לתוך Unity.
2. חכה ל־Compile.
3. בתפריט לחץ:

```text
Boredom And Dungeons V9/Create Charger Combat Test Scene
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

## מה לבדוק

1. אויב החרב רודף ותוקף.
2. אויב הניגוח מזהה אותך.
3. הוא עוצר רגע ב־Windup.
4. הוא מסתער קדימה.
5. אם הוא פוגע בשחקן, הוא מוריד חיים.
6. כשאתה פוגע באויב, יש Knockback.
7. אויב מת ונעלם כשהחיים שלו נגמרים.

## הערה

זה עדיין Prototype combat. אין עדיין אנימציות, אבל יש כבר הבדל משחקי בין:
- אויב חרב
- אויב ניגוח
