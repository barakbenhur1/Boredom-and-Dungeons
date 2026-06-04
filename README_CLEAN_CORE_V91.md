# Boredom & Dungeons — Clean Core V91

זו גרסת תיקון Editor אחרי V90.

## מה תוקן

### 1. Warning קטן

ב־V90 הופיעה אזהרה:

```text
BDPlayerDodgeAfterimage.cs:
warning CS0414: groundOffset is assigned but never used
```

ב־V91 השדה הלא־משומש הוסר.

### 2. Error בזמן Play Mode

ב־V90 הופיעה שגיאה:

```text
InvalidOperationException:
This cannot be used during play mode, please use SceneManager.CreateScene() instead.

EditorSceneManager.NewScene
BDCreateCleanMazePrototypeScene.CreateCleanMazePrototypeScene()
```

הסיבה:
- כלי יצירת הסצנה הוא Editor tool.
- הוא ניסה להריץ `EditorSceneManager.NewScene(...)` בזמן Play Mode.
- Unity לא מאפשר את זה.

## התיקון ב־V91

נוסף guard בתחילת scene builders:

```text
if EditorApplication.isPlayingOrWillChangePlaymode:
    log warning
    return safely
```

וגם הקריאה ל־`EditorSceneManager.NewScene(...)` עוברת דרך helper בטוח:

```text
SafeNewEditorScene(...)
```

## התוצאה

```text
אם מפעילים את יוצר הסצנה בזמן Play Mode:
    אין exception
    מקבלים warning ברור
    צריך לעצור Play Mode ולהריץ שוב

אם מריצים מחוץ ל־Play Mode:
    יצירת הסצנה עובדת רגיל
```

## נשמר מ־V90

- Dodge effect הוא 2D ground trail.
- הסוס לא מסתובב כל הזמן לשחקן.
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

חכה ל־Compile מלא.

חשוב:
```text
אל תריץ את Create Clean Maze Prototype Scene בזמן Play Mode.
עצור Play Mode ואז הרץ את יוצר הסצנה.
```
