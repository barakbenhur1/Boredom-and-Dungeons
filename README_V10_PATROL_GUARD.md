# Boredom & Dungeons — V10 Patrol Guard Enemy

הגרסה הזאת ממשיכה את V9 ומוסיפה את האויב השלישי:

- אויב פטרול / שמירה
- נע בין נקודות פטרול
- שומר על אזור
- מזהה שחקן שנכנס לאזור
- רודף ותוקף
- אם השחקן מתרחק מדי, חוזר לאזור השמירה
- כולל ספירה שחורה
- כולל Health ו־Knockback

## התקנה

1. העתק את `Assets/_Project` לתוך Unity.
2. חכה ל־Compile.
3. בתפריט לחץ:

```text
Boredom And Dungeons V10/Create Patrol Guard Test Scene
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
2. אויב הניגוח עושה windup ואז charge.
3. אויב הפטרול נע בין נקודות.
4. כשאתה נכנס לטווח השמירה שלו, הוא רודף.
5. אם אתה מתרחק, הוא חוזר לפטרול.
6. אפשר לפגוע בו והוא נדחף אחורה.
7. כשהוא מת הוא נעלם.

## הערה

זה עדיין Prototype. המטרה היא שיהיו כבר שלושה archetypes ברורים:
- Sword Fighter
- Charger
- Patrol Guard
