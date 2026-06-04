# Boredom & Dungeons — Clean Core V34

זו גרסה נקייה מלאה להחלפה, לא פאטץ׳.

## חדש ב־V34

### 1. Dodge בלחיצה כפולה על כיוון

התחמקות כבר לא מופעלת עם `Shift`.

עכשיו:

```text
Double tap W / Up Arrow    Dodge forward
Double tap S / Down Arrow  Dodge backward
Double tap A / Left Arrow  Dodge left
Double tap D / Right Arrow Dodge right
```

ה־dodge עובד לפי שיטת הניווט של V32:

```text
W / Up = קדימה לכיוון העכבר
S / Down = אחורה מהעכבר
A / Left = שמאלה ביחס לעכבר
D / Right = ימינה ביחס לעכבר
```

### 2. Minimap בצד ימין למטה

המינימאפ עבר ל:

```text
Bottom Right
```

## מה בוטל

```text
Shift no longer dodges
```

## נשמר מ־V33

- Minimap נוצר בפועל.
- חדר ההתחלה בלי אויבים.
- ניווט יחסית לכיוון העכבר.
- התקפות חרב ו־Ranged לפי כיוון העכבר.
- חדרי לחימה גדולים יותר.
- אויבים נדירים במסדרונות.
- Ranged Attack עם מחסנית 3 יריות ו־reload של 6 שניות.
- Healing drop של 16% מאויבים.
- ריפוי סוס עם `F`, כולל Healing Floor Lock.
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

2. ודא שה־Minimap מופיע בצד ימין למטה.
3. לחץ `Shift` — לא אמורה להיות התחמקות.
4. לחץ פעמיים מהר `W` או `Up Arrow` — התחמקות קדימה לכיוון העכבר.
5. לחץ פעמיים מהר `S` או `Down Arrow` — התחמקות אחורה.
6. לחץ פעמיים מהר `A/D` או `Left/Right Arrow` — התחמקות לצדדים.
