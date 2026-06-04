# Boredom & Dungeons — Clean Core V41

זו גרסה נקייה מלאה להחלפה, לא פאטץ׳.

## מה תוקן ב־V41

### 1. רגישות העכבר ירדה

נוספו smoothing ו־dead zone סביב הדמות כדי שהכיוון לא יקפוץ מכל תזוזה קטנה של העכבר.

```text
Player mouse aim smoothing = 7.0
Player mouse aim dead zone = 1.15 world units
Horse mouse aim smoothing = 6.5
Horse mouse aim dead zone = 1.35 world units
```

### 2. Mouse aim עובד גם על הסוס

חישוב ה־mouse aim מתעלם עכשיו מ־Player, Horse, Enemies, Triggers, Pickups ו־Minimap helper rooms, ומנסה למצוא רצפה/קרקע אמיתית.

בזמן רכיבה:

```text
Mouse = כיוון הסתכלות/כוונה
W = קדימה ביחס לעכבר
S = אחורה
A/D = סטרייפ ביחס לעכבר
Q = ירי לכיוון העכבר
```

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

1. צור `Boredom And Dungeons/Create Clean Maze Prototype Scene`.
2. הזז את העכבר סביב השחקן — הסיבוב אמור להיות פחות עצבני.
3. עלה על הסוס ובדוק שהסוס מסתובב וזז יחסית לכיוון העכבר.
4. לחץ `Q` על הסוס — הירי צריך לצאת לכיוון העכבר.
