# Boredom & Dungeons — Clean Core V94

זו גרסת תיקון כיוון תנועה אחרי V93.

## מה תוקן ב־V94

ב־V93 העכבר סובב את המודל רק בתוך קונוס קדמי של 60°.  
אבל התנועה עצמה עדיין נשארה יחסית למצלמה בלבד.

ב־V94:

```text
Mouse changes model direction inside 60° front cone
AND movement direction follows that same clamped direction
```

## ההתנהגות עכשיו

```text
W / Up    → קדימה לפי הכיוון הקדמי החדש של המודל
S / Down  → אחורה ביחס לכיוון הזה
A / Left  → שמאלה ביחס לכיוון הזה
D / Right → ימינה ביחס לכיוון הזה
```

אבל עדיין:

```text
העכבר מוגבל ל־60° קדימה
העכבר לא יכול לסובב אותך לאחור
העכבר לא יכול לשבור את התנועה לצדדים חדים
```

## גם על הסוס

בזמן רכיבה:

```text
Mouse changes mounted model/aim inside 60° front cone
WASD / arrows follow that same clamped mounted direction
Q shoots by the clamped direction
Sword remains disabled while mounted
```

## התחמקות

התחמקות עדיין לפי הכפתור שנלחץ, אבל הכיוון הקדמי שלה עכשיו תואם לכיוון התנועה החדש:

```text
Double W → dodge קדימה לפי front cone direction
Double S → dodge אחורה
Double A → dodge שמאלה
Double D → dodge ימינה
```

## נשמר מ־V93

- העכבר מוגבל ל־60° קדימה.
- סוס פונה לשחקן רק כשהוא בא אליו.
- חרב לא מפספסת אויבים צמודים.
- Dodge effect הוא 2D ground trail.
- חיווי שאפשר לרפא את הסוס.
- אין חרב על הסוס, רק ירי.
- קליעים פוגעים בקירות.
- Performance Guard.
- Enemy attack telegraphs.
- Ranged muzzle flash/trail/impact.
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

חכה ל־Compile מלא ואז בדוק:
- W קדימה עם עכבר קצת ימינה/שמאלה.
- שהתנועה משתנה לפי הכיוון המוגבל.
- שהעכבר לא מסובב לאחור.
- אותו הדבר על הסוס.
