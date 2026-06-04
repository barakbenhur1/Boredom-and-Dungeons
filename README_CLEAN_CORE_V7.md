# Boredom & Dungeons — Clean Core V7

זו גרסה נקייה מלאה להחלפה, לא פאטץ׳.

## מה לעשות לפני שמייבאים

מחק לגמרי:

```text
Assets/_Project
```

אחר כך העתק מה־ZIP את:

```text
Assets/_Project
```

## תפריטי Unity

אחרי Compile יש שני תפריטים:

```text
Boredom And Dungeons/Create Clean Prototype Scene
Boredom And Dungeons/Create Clean Maze Prototype Scene
```

הראשון יוצר חדר בדיקה כמו קודם.  
השני יוצר מבוך בסיסי של חדרים.

## חדש ב־V7

נוסף מחולל מבוך בסיסי:

- מבוך פתיר בוודאות.
- כניסה רנדומלית על הצלע התחתונה של המבוך.
- יציאה רנדומלית על הצלע העליונה של המבוך.
- מבוך חדרים רחבים שמתאימים לקרבות.
- פתחים בין חדרים לפי אלגוריתם DFS.
- קירות חיצוניים ופנימיים.
- שחקן וסוס מתחילים בכניסה.
- סוס מקבל Safe Spot בחדר ההתחלה.
- אויבים מפוזרים בחדרים שונים.
- יש Exit Marker בחלק העליון.
- יש יעד סיום זמני: להגיע ל־Top Exit.

## למה זה חשוב

עד עכשיו היה חדר אחד.  
עכשיו מתחיל המבנה האמיתי של המשחק: מבוך שמורכב מחדרים וצמתים, עם ניווט, אויבים, וסיום.

## שליטה

```text
WASD / Arrows    Move player / move horse while mounted
Space            Jump on foot / horse jump while mounted
Left Shift       Dash when on foot
C                Toggle camera follow
Left Mouse / J   Fast attack
Right Mouse / K  Heavy attack
E                Mount / Dismount / Heal horse
```

## בדיקה

1. לחץ:
```text
Boredom And Dungeons/Create Clean Maze Prototype Scene
```

2. לחץ Play.
3. בדוק שהשחקן מתחיל באזור התחתון של המבוך.
4. בדוק שיש יציאה מסומנת באזור העליון.
5. נווט בין חדרים דרך פתחים.
6. ודא שיש אויבים בכמה חדרים.
7. ודא שהסוס עדיין עובד:
   - רכיבה
   - קפיצה
   - ירידה בקפיצה לפי כיוון
   - ריצה לפינה בטוחה
