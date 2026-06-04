# Boredom & Dungeons — Clean Core V36

זו גרסה נקייה מלאה להחלפה, לא פאטץ׳.

## חדש ב־V36 — מצלמה קבועה מאחורי השחקן

המצלמה עכשיו נשארת תמיד מאחורי השחקן/סוס, באותה זווית צפייה של הפרוטוטייפ.

## איך זה עובד

```text
Player turns with mouse
Camera rotates with player
Camera stays behind player
Camera keeps same height/angle/distance style
```

כלומר:

- אם השחקן מסתובב ימינה, המצלמה מסתובבת מאחוריו ימינה.
- אם השחקן מסתובב שמאלה, המצלמה מסתובבת מאחוריו שמאלה.
- המצלמה לא נשארת בכיוון עולם קבוע.
- המצלמה לא דורשת Toggle.
- `C` כבר לא משנה מצב מצלמה.

## בזמן רכיבה

גם על הסוס:

```text
Horse/player aim direction changes
Camera follows behind that direction
```

כך הרכיבה, ההתקפות והירי מרגישים באותו כיוון.

## נשמר מ־V35

- הסוס חוזר לשחקן כשהסכנה נגמרת.
- Dodge ב־Double Tap.
- Minimap בצד ימין למטה.
- חדר ההתחלה בלי אויבים.
- ניווט יחסית לכיוון העכבר.
- התקפות לפי כיוון העכבר.
- חדרי לחימה גדולים יותר.
- אויבים נדירים במסדרונות.
- Ranged Attack עם מחסנית 3 יריות ו־reload של 6 שניות.
- Healing drop של 16% מאויבים.
- ריפוי סוס עם `F`.
- Death reset.
- אויבים לפי תפקיד.

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

1. צור מבוך:
```text
Boredom And Dungeons/Create Clean Maze Prototype Scene
```

2. הזז את העכבר סביב השחקן.
3. השחקן אמור להסתובב לכיוון העכבר.
4. המצלמה אמורה להסתובב יחד איתו ולהישאר מאחוריו.
5. עלה על הסוס.
6. ודא שגם ברכיבה המצלמה נשארת מאחורי כיוון השחקן/הסוס.
