# Boredom & Dungeons — Clean Core V3

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

## תיקון עיקרי ב־V3

תוקן הסוס שלא זז בזמן רכיבה.

הבעיה ב־V2:
- בזמן Mount, הסוס כיבה את `BDPlayerController`.
- אבל הסוס קרא תנועה מתוך `BDPlayerController.LastMoveInput`.
- בגלל שהקומפוננט כבוי, הקלט לא התעדכן.
- לכן הסוס עמד במקום.

התיקון ב־V3:
- `BDHorseController` קורא input בעצמו בזמן רכיבה.
- השחקן עדיין מנוטרל בזמן רכיבה כדי שלא יהיו שני CharacterControllers מתנגשים.
- הסוס זז לפי WASD / Arrows בזמן רכיבה.
- ה־Debug של הסוס מציג `Ride Move X/Y`.

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

## בדיקת הסוס

1. צור סצנה חדשה דרך:
```text
Boredom And Dungeons/Create Clean Prototype Scene
```

2. לחץ Play.
3. לחץ בתוך Game/Simulator.
4. לך לסוס.
5. לחץ `E`.
6. אחרי שעלית עליו, לחץ `WASD`.

אם עובד:
- הסוס זז.
- השחקן נשאר עליו.
- ה־Debug של הסוס מראה `Ride Move X/Y`.
