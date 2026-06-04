# Boredom & Dungeons — Clean Core V9

זו גרסה נקייה מלאה להחלפה, לא פאטץ׳.

## מה לתקן ב־V9

תוקנה שגיאת הקומפילציה מ־V8:

```text
CS0103: The name 'EnsureScenesFolder' does not exist in the current context
```

הגורם:
- בקובץ `BDCreateCleanMazePrototypeScene.cs` הייתה קריאה ל־`EnsureScenesFolder()`.
- הפונקציה הייתה חסרה בקובץ.
- בגלל ש־Unity לא יכול לקמפל את הקובץ, גם התפריט השני של המבוך לא הופיע.

התיקון:
- נוספה הפונקציה החסרה ישירות ל־`BDCreateCleanMazePrototypeScene.cs`.
- אחרי קומפילציה נקייה אמורות להופיע שתי אפשרויות בתפריט.

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

## מה לבדוק

1. אין Errors אדומים ב־Console.
2. מופיעות שתי אפשרויות תחת `Boredom And Dungeons`.
3. לחץ:

```text
Boredom And Dungeons/Create Clean Maze Prototype Scene
```

4. נוצרת סצנת מבוך חדשה בשם:

```text
Assets/_Project/Scenes/02_CleanCore_MazePrototype.unity
```
