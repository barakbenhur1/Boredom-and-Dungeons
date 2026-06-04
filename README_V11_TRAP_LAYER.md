# Boredom & Dungeons — V11 Trap/Bomb Layer Enemy

הגרסה הזאת מוסיפה את האויב הרביעי:

- Enemy Trap/Bomb Layer
- מניח פצצות/מלכודות
- הפצצות מתחמשות אחרי זמן קצר
- אחרי countdown הן מתפוצצות
- אם השחקן בטווח, הוא מקבל נזק
- האויב מנסה לשמור מרחק מהשחקן
- כל ארבעת סוגי האויבים נמצאים עכשיו בסצנה אחת

## התקנה

1. העתק את `Assets/_Project` לתוך Unity.
2. חכה ל־Compile.
3. בתפריט לחץ:

```text
Boredom And Dungeons V11/Create Trap Layer Test Scene
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

1. Sword Fighter רודף ותוקף.
2. Charger עושה windup ואז מסתער.
3. Patrol Guard מפטרל, מזהה, רודף וחוזר לשמור.
4. Trap Layer מניח פצצות.
5. פצצה מופיעה, מחכה, ואז מתפוצצת.
6. אם השחקן בטווח הפיצוץ, יורדים חיים.
7. אפשר להרוג את ה־Trap Layer.
8. כל האויבים נדחפים אחורה כשפוגעים בהם.

## מה זה סוגר

ב־V11 יש Prototype ראשוני לכל ארבעת האויבים שהגדרת:

- אויב חרב
- אויב פטרול/שמירה
- אויב ניגוח
- אויב מניח פצצות/מלכודות
