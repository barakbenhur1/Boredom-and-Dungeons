# Boredom & Dungeons — Clean Core V44

זו גרסה נקייה מלאה להחלפה, לא פאטץ׳.

## מה תוקן ב־V44

### הסוס חוזר עד רדיוס מהשחקן, לא עד השחקן עצמו

בגרסאות קודמות, כשהסכנה נגמרה הסוס היה חוזר לשחקן, אבל היה יכול להגיע צמוד מדי.

ב־V44:

```text
Combat ends → horse returns toward player
Horse reaches comfort radius → horse stops
Horse too close → horse backs away slightly
```

## ערכי ברירת מחדל

```text
Return approach distance = 9.0
Return comfort radius = 4.2
Return too close radius = 2.8
Return stop hysteresis = 0.45
```

כלומר:

- אם הסוס רחוק, הוא חוזר.
- כשהוא מגיע בערך ל־4.2 יחידות מהשחקן, הוא עוצר.
- אם הוא איכשהו נכנס קרוב מדי, מתחת ל־2.8, הוא מתרחק קצת.
- יש hysteresis קטן כדי שלא יהיה רעד קדימה/אחורה על הגבול.

## נשמר מ־V43

- רגישות עכבר משופרת.
- AI אויבים עם discipline בסיסי נגד dogpile.
- מצלמה מאחורי השחקן.
- Dodge ב־Double Tap.
- Minimap בצד ימין למטה.
- חדר ההתחלה בלי אויבים.
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

1. צור:
```text
Boredom And Dungeons/Create Clean Maze Prototype Scene
```

2. היכנס לקרב ורד מהסוס.
3. הסוס אמור לברוח ל־Safe Spot.
4. כשהסכנה נגמרת, הוא אמור לחזור.
5. הוא צריך לעצור ליד השחקן ברדיוס נוח, לא להידבק אליו.
6. אם הוא קרוב מדי, הוא אמור לזוז מעט החוצה.
