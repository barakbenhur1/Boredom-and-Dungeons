# Boredom & Dungeons — Clean Core V1

זו גרסה נקייה, לא פאטץ׳.

## מה לעשות לפני שמייבאים

מחק לגמרי:

```text
Assets/_Project
```

אחר כך העתק מה־ZIP את:

```text
Assets/_Project
```

אם לא תמחק את `Assets/_Project` הישן, קבצי V13/V14 שבורים יכולים להמשיך לשבור קומפילציה.

## מה יש בגרסה הזאת

קוד Runtime נקי:

```text
Assets/_Project/Scripts/Runtime
```

קוד Editor נקי:

```text
Assets/_Project/Scripts/Editor
```

אין תלות ב־V12/V13/V14.
אין reflection.
אין target binders.
אין patch scripts.
אין שמות מחלקות ישנים.

## תפריט Unity

אחרי Compile:

```text
Boredom And Dungeons/Create Clean Prototype Scene
```

זה יוצר סצנה עם:

- שחקן
- תנועה
- קפיצה
- Dash
- מצלמה
- Health
- חרב / Fast + Heavy Attack
- חדר עם פתחים
- Room Encounter
- Exit Zones
- Exit Blocker
- Sword Enemy
- Charger Enemy
- Patrol Guard Enemy
- Trap/Bomb Layer Enemy
- Ranged Shooter Enemy
- Jumper/Ambusher Enemy

## שליטה

```text
WASD / Arrows    Move
Space            Jump
Left Shift       Dash
C                Toggle camera follow
Left Mouse / J   Fast attack
Right Mouse / K  Heavy attack
```

## בדיקה

1. לחץ Play.
2. לחץ בתוך Game/Simulator.
3. ודא שהתנועה עובדת.
4. ודא שהאויבים מזהים את השחקן.
5. התקרב לפתח לפני שהרגת אויבים — ה־Exit Blocker אמור לחסום.
6. הרוג את האויבים — ה־Encounter מסתיים.
