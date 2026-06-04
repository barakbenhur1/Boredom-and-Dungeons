# Boredom & Dungeons — Clean Core V2

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

## מה חדש ב־V2

נוסף Horse Core:

- סוס בסיסי.
- השחקן יכול לעלות על הסוס.
- השחקן יכול לרדת מהסוס.
- רכיבה מהירה יותר מתנועה רגילה.
- בתחילת קרב/Encounter הסוס נשלח לפינה בטוחה.
- הסוס לא עוזב את החדר.
- לסוס יש Health.
- כשהסוס נפצע הוא נע לאט יותר.
- הסוס לא מת; הוא מתעלף.
- אפשר לעמוד לידו ולרפא אותו.
- אויבים יכולים לפגוע בסוס אם הוא קרוב או אם הוא נמצא בינם לבין יציאה/קרב.

## שליטה

```text
WASD / Arrows    Move
Space            Jump
Left Shift       Dash
C                Toggle camera follow
Left Mouse / J   Fast attack
Right Mouse / K  Heavy attack
E                Mount / Dismount / Heal horse when near
```

## בדיקה

1. צור סצנה דרך התפריט.
2. לחץ Play.
3. גש לסוס ולחץ E — השחקן עולה עליו.
4. התנועה אמורה להיות מהירה יותר.
5. לחץ E שוב — השחקן יורד.
6. התקרב לאויבים / Encounter — הסוס אמור ללכת לפינה הבטוחה.
7. אם הסוס נפגע, הוא נע לאט יותר.
8. ליד סוס פצוע לחץ E והחזק קרוב אליו — הוא מתרפא.
