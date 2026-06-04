# Boredom & Dungeons — Clean Core V88

זו גרסת תיקון ניווט אחרי V87.

## מה תוקן ב־V88

### WASD / חצים כבר לא תלויים בעכבר

ב־V87 המצלמה כבר השתפרה, אבל התנועה עדיין הייתה מחושבת יחסית ל־mouse aim:

```text
WASD → relative to mouse direction
```

זה הרגיש לא צפוי.

ב־V88 התנועה הופרדה מהעכבר:

```text
WASD / arrows → relative to camera/player screen direction
Mouse          → aim / attack / ranged direction
```

## ההתנהגות עכשיו

```text
W / Up    → קדימה במסך
S / Down  → אחורה
A / Left  → שמאלה
D / Right → ימינה
```

והעכבר נשאר אחראי ל:

```text
סיבוב המודל
כיוון מכה
כיוון ירי
aim
```

## גם על הסוס

בזמן רכיבה:

```text
WASD / arrows → תנועה יחסית למצלמה/שחקן
Mouse          → כיוון רכיבה/ירי/aim
Q              → ירי
חרב            → עדיין חסומה על הסוס
```

## למה

זה מחזיר את תחושת הניווט לצורה צפויה:
- מקשי תנועה מזיזים אותך בכיוון המסך/המצלמה.
- העכבר מכוון קרב, לא מזיז את כיוון ההליכה.
- המצלמה עדיין לא מסתובבת בגלל aim בזמן שאתה עומד במקום.

## נשמר מ־V87

- אין התקפת חרב על הסוס, רק ירי.
- הסוס לא מסתובב כל הזמן לפנות לשחקן.
- המצלמה מסתובבת רק לפי תנועה אמיתית.
- מצלמה רגועה יותר.
- Editor scene builder fix.
- קליעים פוגעים בקירות.
- Camera occlusion polish.
- Performance Guard.
- Enemy attack telegraphs.
- Ranged muzzle flash/trail/impact.
- Melee slash arcs.
- Player low-health feedback.
- Enemy HP bars.
- Audio / hit-stop / camera shake.
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

חכה ל־Compile מלא ואז בדוק WASD/חצים ברגל ועל הסוס.
