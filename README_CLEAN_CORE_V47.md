# Boredom & Dungeons — Clean Core V47

זו גרסה נקייה מלאה להחלפה, לא פאטץ׳.

## מה תוקן ב־V47 — Professional cumulative mouse look

ב־V46 הכיוון כבר היה מבוסס `mouse delta`, אבל העכבר עדיין לא היה נעול למרכז המסך.  
בגלל זה השליטה לא הרגישה כמו משחק מקצועי: העכבר היה יכול להגיע לקצה המסך, ואז הסיבוב לא הרגיש מצטבר באמת.

ב־V47:

```text
Cursor locks to screen center during gameplay
Mouse delta accumulates yaw
Yaw is persistent
Movement/attacks/camera use the accumulated yaw
```

כלומר:

```text
Move mouse right → yaw increases
Move mouse right again → yaw keeps increasing
Move mouse left → yaw decreases
```

זה לא תלוי במיקום העכבר על המסך.

## שליטה בעכבר

```text
Mouse move = turn / aim
Esc = release cursor
Left click / right click / middle click = lock cursor again
```

## התוצאה הרצויה

- אפשר להסתובב 360° שוב ושוב.
- אין “קצה מסך”.
- הכיוון מצטבר כמו במשחקים רציניים.
- התקפות ו־Q יוצאות לכיוון הכיוון המצטבר.
- המצלמה נשארת מאחורי הכיוון המצטבר.
- עובד גם ברגל וגם על הסוס.

## נשמר מ־V46

- Mouse yaw מגיב מיידית.
- מצלמה מאחורי השחקן.
- הסוס חוזר רק עד רדיוס נוחות.
- Dodge ב־Double Tap.
- Minimap בצד ימין למטה.
- Ranged Attack עם מחסנית 3 יריות ו־reload של 6 שניות.
- Healing drop של 16% מאויבים.
- ריפוי סוס עם `F`.
- Death reset.

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

1. צור:
```text
Boredom And Dungeons/Create Clean Maze Prototype Scene
```

2. לחץ בתוך חלון המשחק כדי לנעול Cursor.
3. הזז את העכבר ימינה כמה פעמים ברצף.
4. השחקן אמור להמשיך להסתובב, בלי להיעצר בקצה מסך.
5. לחץ `Esc` — העכבר משתחרר.
6. לחץ שוב בתוך המשחק — העכבר ננעל שוב.
7. עלה על הסוס ובדוק אותו דבר.
