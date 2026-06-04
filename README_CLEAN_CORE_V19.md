# Boredom & Dungeons — Clean Core V19

זו גרסה נקייה מלאה להחלפה, לא פאטץ׳.

## חדש ב־V19 — ריפוי סוס ברור

ב־V18 היה HUD לסוס, אבל הכפתור של הריפוי לא היה מספיק ברור.

ב־V19 הפרדתי בין רכיבה לריפוי:

```text
E = Mount / Dismount
F held = Heal / Revive horse
```

## איך מרפאים את הסוס

### אם הסוס פצוע אבל לא מעולף

1. עמוד ליד הסוס.
2. החזק `F`.
3. ה־Horse HP עולה בהדרגה.
4. `E` עדיין מעלה אותך על הסוס, גם אם הוא פצוע.

### אם הסוס מעולף

1. עמוד ליד הסוס.
2. החזק `F`.
3. הסוס מקבל חיים בהדרגה.
4. כשהוא עובר את סף העילפון הוא חוזר למצב רגיל.
5. אחרי זה אפשר לעלות עליו עם `E`.

## אינדיקציות חדשות

### HUD

ה־HUD מציג עכשיו:

- Player HP
- Horse HP
- Horse state:
  - Ready
  - Injured — Hold F to heal
  - Fainted — Hold F to revive

### מעל הסוס

מעל הסוס מופיע:

- HP bar קטן.
- אם הוא פצוע:
```text
HOLD F TO HEAL
```

- אם הוא מעולף:
```text
HORSE FAINTED
HOLD F TO REVIVE
```

## נשמר מ־V18

- Player HP bar.
- Horse HP bar.
- Damage flash לשחקן.
- מוות אמיתי לשחקן: תנועה וקרב מושבתים.
- AI Director.
- Patrol Guard חוסם יציאות בגוף.
- השחקן לא נמתח על הסוס.
- קירות שקופים כשהם מסתירים.
- קירות גבוהים.
- הסוס נפגע ויכול לזרוק את השחקן.

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

## שליטה

```text
WASD / Arrows    Move player / move horse while mounted
Space            Jump on foot / horse jump while mounted
Left Shift       Dash when on foot
C                Toggle camera follow
Left Mouse / J   Fast attack
Right Mouse / K  Heavy attack
E                Mount / Dismount
F held           Heal / Revive horse
```

## בדיקה

1. צור מבוך.
2. תן לאויבים לפגוע בסוס.
3. ודא ש־Horse HP יורד.
4. עמוד ליד הסוס והחזק `F`.
5. ודא ש־Horse HP עולה.
6. תן לסוס להתעלף.
7. ודא שמופיע:
```text
HORSE FAINTED
HOLD F TO REVIVE
```
8. החזק `F` לידו עד שהוא חוזר.
