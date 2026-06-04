# Boredom & Dungeons — Clean Core V90

זו גרסת תיקון אפקט התחמקות אחרי V89.

## מה תוקן ב־V90

### Dodge effect הוא עכשיו 2D על הרצפה

בגרסאות הקודמות אפקט ההתחמקות היה נראה כמו afterimage תלת־ממדי/קפסולה, וזה לא נראה טוב.

ב־V90 הוא הוחלף ל:

```text
2D ground dodge trail
שטוח על הרצפה
מיושר לכיוון ה־dash
נמוג מהר
ללא collider
ללא HUD
ללא טקסט
```

## התנהגות

```text
Double-tap dodge
→ trail שטוח על הרצפה
→ הכיוון לפי תנועת ההתחמקות בפועל
→ לא לפי סיבוב גוף בלבד
```

## למה

זה נראה יותר כמו משחק אקשן מלמעלה:
- לא מסתיר את הדמות.
- לא נראה כמו רוח/כפיל.
- יושב על הרצפה.
- יותר קריא בזמן קרב.

## נשמר מ־V89

- הסוס לא מסתובב כל הזמן לפנות לשחקן.
- חיווי שאפשר לרפא את הסוס.
- WASD/חצים לא תלויים בעכבר.
- אין חרב על הסוס, רק ירי.
- קליעים פוגעים בקירות.
- Performance Guard.
- Enemy attack telegraphs.
- Ranged muzzle flash/trail/impact.
- Melee slash arcs.
- Enemy HP bars.
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

חכה ל־Compile מלא ואז בדוק התחמקות ליד קירות ובחדרי קרב.
