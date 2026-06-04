# Boredom & Dungeons — Clean Core V37

זו גרסת תיקון קומפילציה נקייה ל־V36.

## מה תוקן

ב־V36 היו שתי שגיאות קומפילציה ב־`BDHorseController.cs`:

```text
HorseState does not contain ReturningToPlayer
characterController does not exist in the current context
```

ב־V37 תוקן:

```text
HorseState כולל ReturningToPlayer
הקוד משתמש בשם הנכון של ה־CharacterController: controller
```

## נשמר מ־V36

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
3. בדוק שהמצלמה נשארת מאחורי השחקן.
4. בדוק שהסוס חוזר אחרי סיום סכנה.
