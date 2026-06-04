# Boredom & Dungeons — Clean Core V66

זו גרסה נקייה מלאה להחלפה, לא פאטץ׳.

## מה תוקן ב־V66

### 1. אויבים פחות עולים אחד על השני

ב־V65 עדיין היו מצבים שבהם אויבים נצמדים או עולים אחד על השני.

ב־V66 נוסף/חוזק מנגנון הפרדה:

```text
Enemy personal space radius
Enemy separation force
NonAlloc overlap check
Role-based spacing
Ground-only push
```

המטרה:
- אויבים יכולים להתקרב ולהילחם.
- אבל לא אמורים להיכנס אחד לתוך השני או לעמוד בערימה.
- תוקפים יכולים להיות קרובים יותר.
- יורים/קופצים/מפטרלים שומרים קצת יותר מרחק.

### 2. העכבר קרוב לשחקן כבר לא מסובב את המסך בקרב

ב־V65 היה Center Dead Zone בעולם, אבל בזמן קרב כשהעכבר ממש קרוב לגוף/על הגוף, ה־ray של המצלמה עדיין יכל ליצור כיוון לא יציב ולגרום למסך להסתובב.

ב־V66 נוסף גם:

```text
Screen-space center dead zone
```

כלומר:

```text
אם העכבר קרוב מדי למרכז הדמות במסך:
    לא מעדכנים aim target
    לא מסובבים את הדמות
    לא מסובבים מצלמה בעקבות זה
```

## ערכים

```text
Player screen center dead zone = 70px
Horse screen center dead zone = 88px

Enemy base personal radius = 1.35
Enemy separation force = 7.5
```

## נשמר מ־V65

- Hit feedback.
- Q ammo/reload HUD.
- Continuous mouse aim smoothing.
- Movement smoothing.
- Performance optimizations.
- ירי על הסוס משתמש ב־Horse.LastMountedAimDirection.
- Safe projectile material fallback.
- Front dead zone = 3°.
- Center dead zone:
  - Player = 1.85
  - Horse = 2.20
- אפשר לאסוף Healing Pickup על גב הסוס.
- Projectile capsule sweep.
- הסוס חוזר רק עד רדיוס נוחות מהשחקן.
- Dodge ב־Double Tap.
- Minimap בצד ימין למטה.
- Ranged Attack עם מחסנית 3 יריות ו־reload של 6 שניות.
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
