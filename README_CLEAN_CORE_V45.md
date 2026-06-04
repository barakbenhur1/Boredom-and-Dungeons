# Boredom & Dungeons — Clean Core V45

זו גרסת תיקון קומפילציה נקייה אחרי V44.

## מה תוקן

ב־V44 היו שתי שגיאות C#:

```text
Embedded statement cannot be a declaration or labeled statement
The name 'clampedMouseX' does not exist in the current context
```

הסיבה הייתה קוד כזה:

```csharp
if (condition)
    float clampedMouseX = ...
    targetYaw += clampedMouseX ...
```

ב־C# חייבים סוגריים כשמכריזים משתנה בתוך `if`.

ב־V45 זה תוקן גם ב:

```text
BDPlayerController.cs
BDHorseController.cs
```

## בנוסף

נוקו warnings מסוג:

```text
CS0414 field is assigned but its value is never used
```

עבור debug/internal state fields שלא חשובים כרגע לקומפילציה.

## נשמר מ־V44

- הסוס חוזר רק עד רדיוס נוחות מהשחקן, לא צמוד אליו.
- רגישות עכבר משופרת מ־V43.
- AI אויבים עם discipline בסיסי נגד dogpile.
- מצלמה מאחורי השחקן.
- Dodge ב־Double Tap.
- Minimap בצד ימין למטה.
- חדר ההתחלה בלי אויבים.
- Ranged Attack עם מחסנית 3 יריות ו־reload של 6 שניות.
- Healing drop של 16% מאויבים.
- ריפוי סוס עם `F`.
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
