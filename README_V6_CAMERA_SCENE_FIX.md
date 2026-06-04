# Boredom & Dungeons — V6 Camera + Scene Save Fix

הגרסה הזאת מתקנת שני דברים שנראו בצילומים:

1. `Missing Script` על ה־Main Camera.
2. שגיאת שמירה:
```text
Parent directory must exist before creating asset at: Assets/_Project/Scenes/...
```

## למה זה קרה

ב־V5 ה־Camera Follow הוגדר בטעות בתוך קובץ שבתיקיית `Editor`.  
Unity לא יכול להריץ קומפוננט runtime מתוך תיקיית Editor, ולכן על ה־Camera הופיע Missing Script.

בנוסף, ה־Editor script ניסה לשמור סצנה לתיקייה שלא בהכרח קיימת.

## התקנה

1. תעתיק את `Assets/_Project` מה־ZIP לתוך הפרויקט.
2. חכה ל־Compile.
3. פתח את הסצנה שלך.
4. לחץ בתפריט:

```text
Boredom And Dungeons V6/FIX Current Scene
```

זה יעשה:

- ימחוק Missing Scripts מה־Main Camera ומהשחקן אם יש.
- יוודא שיש `BD_V6CameraFollow` תקין על המצלמה.
- יוודא שיש `BD_DirectMovementV6` על השחקן.
- יוודא שיש תיקיית `Assets/_Project/Scenes`.
- ישמור סצנה תקינה.

## בדיקה

ב־Hierarchy בחר:

```text
Main Camera
```

אסור שיהיה עליה Missing Script.

בחר:

```text
BD_V6_Player
```

או השחקן הקיים.

חייב להיות עליו:

```text
BD_DirectMovementV6
```

ב־Play:

- W / Up Arrow = Move Y 1
- S / Down Arrow = Move Y -1
- A / Left Arrow = Move X -1
- D / Right Arrow = Move X 1

יש Debug Overlay על המסך.
