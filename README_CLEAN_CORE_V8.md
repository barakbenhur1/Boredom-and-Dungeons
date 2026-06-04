# Boredom & Dungeons — Clean Core V8

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

## תיקון עיקרי ב־V8

תוקנה שגיאת הקומפילציה מ־V7:

```text
CS0104: 'Object' is an ambiguous reference between 'UnityEngine.Object' and 'object'
```

הגורם:
- בקובץ `BDCreateCleanMazePrototypeScene.cs` היה `using System`.
- באותו קובץ השתמשנו ב־`Object.DestroyImmediate`.
- Unity/C# פירש את `Object` בצורה דו־משמעית.

התיקון:
- כל הקריאות שונו ל־`UnityEngine.Object.DestroyImmediate`.
- גם קובץ יצירת סצנת הבדיקה הרגילה נוקה לאותו סגנון כדי למנוע חזרה של הבעיה.

## תפריטי Unity

אחרי Compile יש שני תפריטים:

```text
Boredom And Dungeons/Create Clean Prototype Scene
Boredom And Dungeons/Create Clean Maze Prototype Scene
```

## מה יש בגרסה

- שחקן
- תנועה
- קפיצה
- Dash
- קרב
- כל האויבים
- סוס
- קפיצת סוס
- ירידה מהסוס בקפיצה לפי כיוון הקלט
- מבוך פתיר
- כניסה רנדומלית בצלע התחתונה
- יציאה רנדומלית בצלע העליונה
- חדרים רחבים
- אויבים מפוזרים במבוך

## בדיקה

1. מחק `Assets/_Project`.
2. העתק את `Assets/_Project` מה־ZIP.
3. חכה ל־Compile.
4. ודא שאין Errors אדומים ב־Console.
5. צור סצנת מבוך:

```text
Boredom And Dungeons/Create Clean Maze Prototype Scene
```
