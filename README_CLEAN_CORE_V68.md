# Boredom & Dungeons — Clean Core V68

זו גרסה נקייה מלאה להחלפה, לא פאטץ׳.

## מה תוקן ב־V68

### Heavy attack מרגיש עכשיו כמו מכה חזקה

ב־V67 קליק ימני עשה damage, אבל לא נתן תחושת פגיעה מספיק ברורה, וגם ה־knockback היה חלש מדי ונבלע בתוך תנועת ה־AI.

ב־V68:

```text
Right click / K heavy hit:
    enemy stronger flash
    large impact burst
    strong knockback
    short knock-control lock so AI does not immediately cancel the push

Left click / J light hit:
    smaller flash
    smaller impact burst
    smaller knockback
```

## ערכים חדשים

```text
Light knockback = 7.0
Heavy knockback = 19.0
Light knock lock = 0.08s
Heavy knock lock = 0.20s

Knockback drag = 5.8
Knockback max speed = 18.0
```

## חשוב

```text
אם ההתקפה לא פגעה באויב — אין אפקט מזויף.
אם היא פגעה — רואים flash + impact + knockback.
```

## נשמר מ־V67

- תיקון `BDRangedShooterEnemy`.
- אויבים פחות עולים אחד על השני.
- Screen-space center dead zone כשהעכבר קרוב לשחקן/סוס.
- Hit feedback.
- Q ammo/reload HUD.
- Continuous mouse aim smoothing.
- Movement smoothing.
- Performance optimizations.
- ירי על הסוס משתמש ב־Horse.LastMountedAimDirection.
- Healing Pickup נאסף גם על גב הסוס.
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
