# Boredom & Dungeons — Clean Core V59

זו גרסה נקייה מלאה להחלפה, לא פאטץ׳.

## מה תוקן ב־V59

### 1. העכבר קצת פחות רגיש

ב־V58 מערכת הניווט כבר אחידה, אבל הכיוון עדיין הרגיש מעט רגיש מדי.

ב־V59 הורדתי את מהירות סיבוב ה־aim:

```text
Player idle aim turn: 220°/sec → 185°/sec
Player moving aim turn: 320°/sec → 275°/sec

Horse idle aim turn: 180°/sec → 150°/sec
Horse moving aim turn: 260°/sec → 220°/sec
```

המשמעות:
- עדיין מגיב.
- פחות עצבני.
- יותר הדרגתי וטבעי.

### 2. תיקון ArgumentNullException בירי

ב־V58 הייתה שגיאה בזמן ירי:

```text
ArgumentNullException: Value cannot be null.
Parameter name: shader
BDPlayerCombat.CreateProjectileMaterial()
```

הסיבה:
- Unity לא מצא את shader בשם `Universal Render Pipeline/Lit`.
- גם `Standard` יכול להיות לא זמין בפרויקטים מסוימים.
- הקוד ניסה ליצור `new Material(null)`.

ב־V59 יש fallback בטוח:

```text
Universal Render Pipeline/Lit
→ Standard
→ Unlit/Color
→ Sprites/Default
→ Hidden/InternalErrorShader
→ Built-in Default-Material
→ no custom material, without crashing
```

אם שום חומר לא נמצא, הקליע עדיין נוצר בלי להפיל exception.

## נשמר מ־V58

- תיקון `BDPlayerCombat.cs`.
- מערכת ניווט אחידה:
  - Mouse target
  - Dead zones
  - Facing aim הדרגתי
  - Movement relative to facing aim
  - Combat/Ranged same facing aim
  - Camera same facing aim
- Front dead zone = 3°.
- Center dead zone:
  - Player = 1.85
  - Horse = 2.20
- אפשר לאסוף Healing Pickup על גב הסוס.
- Mounted ranged projectile hit fixes.
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
