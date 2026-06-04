# Boredom & Dungeons — Clean Core V14

זו גרסה נקייה מלאה להחלפה, לא פאטץ׳.

## מה תוקן ב־V14

תוקנה שגיאת הקומפילציה:

```text
CS0111:
Type 'BDCreateCleanPrototypeScene' already defines a member called 'CreateAnchor'
with the same parameter types
```

הגורם:
- ב־V13 היו שתי פונקציות זהות בשם `CreateAnchor` באותו קובץ:
```text
Assets/_Project/Scripts/Editor/BDCreateCleanPrototypeScene.cs
```

התיקון:
- הוסרה הכפילות.
- נשארת פונקציית `CreateAnchor` אחת בלבד.
- סצנת החדר הרגילה וסצנת המבוך עדיין יכולות להשתמש באותה פונקציה.

## נשמר מ־V13

- אויבי Patrol Guard רצים לפני היציאה וחוסמים בגוף שלהם.
- שאר האויבים ממשיכים להפריע ליציאה כמו ב־V11.
- אין חסימת פתח מלאכותית עם קיר/Collider.
- חדרי הלחימה גדולים יותר.
- הקירות גבוהים יותר כדי שלא יהיה אפשר לקפוץ מעליהם.
- הסוס נפגע.
- הסוס זורק את השחקן אחרי רצף פגיעות קצר.
- קפיצת סוס.
- ירידה מהסוס בקפיצה לפי כיוון הקלט.

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

## תפריטי Unity שאמורים להופיע

```text
Boredom And Dungeons/Create Clean Prototype Scene
Boredom And Dungeons/Create Clean Maze Prototype Scene
```

## בדיקה

1. ודא שאין Errors אדומים ב־Console.
2. צור מבוך:
```text
Boredom And Dungeons/Create Clean Maze Prototype Scene
```
3. נסה לקפוץ מעל קירות — לא אמור לעבוד.
4. התקרב ליציאה בחדר עם Patrol Guard — הוא אמור לרוץ לפני הפתח ולחסום בגוף שלו.
