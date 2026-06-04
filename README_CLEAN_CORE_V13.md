# Boredom & Dungeons — Clean Core V13

זו גרסה נקייה מלאה להחלפה, לא פאטץ׳.

## התקנה נקייה

מחק לגמרי:

```text
Assets/_Project
```

ואז העתק מה־ZIP את:

```text
Assets/_Project
```

חכה ל־Compile מלא.

## תפריטי Unity

```text
Boredom And Dungeons/Create Clean Prototype Scene
Boredom And Dungeons/Create Clean Maze Prototype Scene
```

## חדש / מתוקן ב־V13

### 1. Patrol Guards חוסמים יציאות בגוף שלהם

נשמר השינוי מ־V12:

- אם השחקן מתקרב ליציאה מחדר עם אויבים חיים:
  - אויבי פטרול רצים לפני היציאה.
  - הם עומדים בנתיב ומנסים לחסום בגוף שלהם.
  - שאר האויבים ממשיכים להפריע כמו ב־V11.
- אין קיר בלתי נראה ואין נעילת דלת מלאכותית.

### 2. הקירות גבוהים יותר

הקירות הוגבהו כך שלא אמור להיות אפשר לקפוץ מעליהם:

```text
Maze wall height: 5.5
Prototype wall height: 5.5
```

זה גבוה יותר מקפיצת השחקן, קפיצת הסוס, וקפיצת הירידה מהסוס, עם מרווח ביטחון.

## מה לבדוק

1. צור מבוך:

```text
Boredom And Dungeons/Create Clean Maze Prototype Scene
```

2. נסה לקפוץ ליד קיר עם השחקן.
3. נסה לקפוץ ליד קיר עם הסוס.
4. לא אמור להיות אפשר לעבור מעל הקיר.
5. היכנס לחדר עם `BD_Enemy_PatrolGuard`.
6. נסה לצאת בזמן שיש אויבים חיים.
7. הפטרול אמור לרוץ לפני הפתח ולחסום בגוף שלו.
8. שאר האויבים אמורים להמשיך להפריע.

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
