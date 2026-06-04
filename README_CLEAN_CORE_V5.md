# Boredom & Dungeons — Clean Core V5

זו גרסה נקייה מלאה להחלפה, לא פאטץ׳.

## מה לעשות לפני שמייבאים

מחק לגמרי:

```text
Assets/_Project
```

אחר כך העתק מה־ZIP את:

```text
Assets/_Project
```

## תפריט Unity

אחרי Compile:

```text
Boredom And Dungeons/Create Clean Prototype Scene
```

## חדש ב־V5

ירידה מהסוס עכשיו היא קפיצה לפי הכיוון שהשחקן לוחץ:

- `E + W` — יורד בקפיצה קדימה.
- `E + S` — יורד בקפיצה אחורה.
- `E + A` — יורד בקפיצה שמאלה.
- `E + D` — יורד בקפיצה ימינה.
- אלכסונים עובדים גם כן.
- אם אין כיוון לחוץ בזמן הירידה, יש fallback קטן לצד של הסוס.

## ליד אויבים

הכיוון עדיין נקבע לפי המקלדת, לא לפי האויב.

אבל אם יש אויב קרוב:
- אותה קפיצה לאותו כיוון שהשחקן לוחץ.
- המרחק גדול יותר.
- הקשת גבוהה יותר.
- משך הקפיצה קצת ארוך יותר.

## אחרי הירידה

- השליטה בשחקן חוזרת בסוף הקפיצה.
- הסוס ממשיך ללכת ל־Safe Spot כמו ב־V4.

## שליטה

```text
WASD / Arrows    Move player / move horse while mounted
Space            Jump when on foot
Left Shift       Dash when on foot
C                Toggle camera follow
Left Mouse / J   Fast attack
Right Mouse / K  Heavy attack
E                Mount / Dismount / Heal horse
```

## בדיקה

1. צור סצנה חדשה דרך:
```text
Boredom And Dungeons/Create Clean Prototype Scene
```

2. לחץ Play.
3. עלה על הסוס עם `E`.
4. החזק `W` ולחץ `E` — השחקן יורד בקפיצה קדימה.
5. עלה שוב, החזק `A` ולחץ `E` — השחקן יורד שמאלה.
6. התקרב לאויב, החזק כיוון ולחץ `E` — הקפיצה גדולה יותר באותו כיוון.
7. הסוס הולך לפינה הבטוחה אחרי הירידה.
