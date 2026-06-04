# Boredom & Dungeons — Clean Core V39

זו גרסת תיקון קומפילציה נקייה אחרי V38.

## מה תוקן

ב־V38 נוספה `ReturningToPlayer`, אבל `BDHorseController.cs` עדיין השתמש בעוד מצבים שלא היו בתוך `enum HorseState`:

```text
MovingToSafeSpot
WaitingSafe
```

ב־V39 ה־enum תוקן בצורה קשיחה וכולל עכשיו את כל המצבים שהקובץ משתמש בהם בפועל:

```csharp
private enum HorseState
{
    Idle,
    Mounted,
    MovingToSafeSpot,
    WaitingSafe,
    FleeingToSafeSpot,
    ReturningToPlayer,
    Fainted
}
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
