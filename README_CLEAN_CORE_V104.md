# Boredom & Dungeons — Clean Core V104

זו גרסת תיקון קומפילציה אחרי V103.

## השגיאה שתוקנה

```text
Assets/_Project/Scripts/Editor/BDCreateCleanMazePrototypeScene.cs(992,39):
error CS0104: 'Object' is an ambiguous reference between 'UnityEngine.Object' and 'object'
```

## הסיבה

בתוך Editor scripts יש `using System`, ולכן `Object` יכול להיות דו־משמעי:

```text
UnityEngine.Object
object / System.Object
```

## התיקון ב־V104

תיקון קשיח:

```text
כל Object.* עצמאי בתוך Editor scripts
→ UnityEngine.Object.*
```

הבדיקה לא נוגעת ב־`GameObject`.

## מה לא השתנה

```text
אין שינוי gameplay
אין שינוי camera
אין שינוי player movement
אין שינוי combat
אין שינוי AI
אין שינוי HUD
```

זה תיקון קומפילציה בלבד.

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

אם עדיין יש error:
```text
שלח את השורה האדומה הראשונה המלאה מה־Console.
```
