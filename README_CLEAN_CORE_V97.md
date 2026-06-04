# Boredom & Dungeons — Clean Core V97

זו גרסת פוליש melee אחרי V96.

## מה נוסף ב־V97

### Soft melee aim assist

נוסף תיקון עדין לכיוון מכות חרב.

הבעיה:
```text
כשאויב קרוב מאוד לשחקן
לפעמים המכה מרגישה כאילו היא עוברת לידו
גם אחרי תיקון ה־capsule
```

ב־V97:

```text
אם יש אויב קרוב בתוך קונוס קדמי קטן
המכה יכולה להיטה אליו מעט
```

## חשוב

זה לא auto-target מלא.

```text
לא מסתובב לאחור
לא מחפש אויבים רחוקים
לא משנה תנועה
לא מושך מכות לכל כיוון
רק תיקון קטן בתוך הקונוס הקדמי
```

## ערכים

```text
meleeSoftAimAssistRange = 2.65
meleeSoftAimAssistConeDegrees = 56
meleeSoftAimAssistMaxAngleDegrees = 24
```

כלומר המכה יכולה לתקן עד 24 מעלות בלבד, ורק אם האויב כבר יחסית קדימה וקרוב.

## נשמר מ־V96

- בזמן dodge המצלמה לא משנה yaw.
- רק W/קדימה מושפע מהעכבר.
- S/A/D לא נגררים אחרי העכבר.
- Mouse/model aim מוגבל ל־60° קדימה.
- סוס פונה לשחקן רק כשהוא בא אליו.
- חרב משתמשת ב־capsule נגד אויבים צמודים.
- Dodge effect הוא 2D ground trail.
- חיווי שאפשר לרפא את הסוס.
- אין חרב על הסוס, רק ירי.
- קליעים פוגעים בקירות.
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

חכה ל־Compile מלא ואז בדוק מכות חרב מול אויבים קרובים מאוד ובזווית קלה.
