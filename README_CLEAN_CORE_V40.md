# Boredom & Dungeons — Clean Core V40

זו גרסת תיקון קומפילציה נקייה אחרי V39.

## מה תוקן

ב־V39 נשארה שגיאה:

```text
BDCameraFollow does not contain a definition for SetTarget
```

ב־V40 הוחזר ל־`BDCameraFollow` ה־API שה־Editor scene builders צריכים:

```csharp
public void SetTarget(Transform newTarget)
```

בנוסף נוקה ה־warning:

```text
BDMazeMinimap.marginTop is assigned but its value is never used
```

## נשמר

- מצלמה קבועה מאחורי השחקן.
- המצלמה מסתובבת עם כיוון השחקן/הסוס.
- הסוס חוזר לשחקן כשהסכנה נגמרת.
- Dodge ב־Double Tap.
- Minimap בצד ימין למטה.
- חדר ההתחלה בלי אויבים.
- ניווט יחסית לכיוון העכבר.
- התקפות לפי כיוון העכבר.
- Ranged Attack עם מחסנית 3 יריות ו־reload של 6 שניות.

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

## בדיקה

1. ודא שאין errors בקונסול.
2. צור:
```text
Boredom And Dungeons/Create Clean Maze Prototype Scene
```
3. ודא שהמצלמה נשארת מאחורי השחקן.
