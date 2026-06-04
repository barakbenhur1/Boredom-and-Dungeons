# Boredom & Dungeons — Clean Core V102

זו גרסת Natural Map Shape Pass אחרי V101, עם התחלה של סדר ריפו מקצועי יותר.

## מה נוסף ב־V102

### 1. Professional project structure

נוספו folders מסודרים לעבודת המשך מקצועית:

```text
Assets/_Project/Art/Materials/Environment
Assets/_Project/Art/Materials/Characters
Assets/_Project/Art/Materials/Horse
Assets/_Project/Art/Materials/Weapons
Assets/_Project/Art/Materials/VFX
Assets/_Project/Art/Prefabs/Environment
Assets/_Project/Art/Textures/Environment
Assets/_Project/Art/Textures/Horse
Assets/_Project/Audio/Ambience
Assets/_Project/Audio/SFX/Combat
Assets/_Project/Audio/SFX/Movement
Assets/_Project/Audio/SFX/Horse
Assets/_Project/Audio/Music
Assets/_Project/Design/Level
Assets/_Project/Design/ArtDirection
Assets/_Project/Scripts/Runtime/Environment/NaturalMap
Assets/_Project/Scripts/Editor/Environment
```

נוספו גם:

```text
PROJECT_STRUCTURE.md
Design/Level/NATURAL_MAP_SHAPE_PASS_V102.md
Design/ArtDirection/ASSET_PIPELINE_NOTES.md
```

### 2. Natural Map Shape Visual Pass

התחלה של מעבר ממראה maze/grid אל תחושה של level טבעי יותר.

ה־maze scene builder מוסיף עכשיו בצורה deterministic:

```text
rounded corner rocks
soft edge boulders
small ground patches
mossy variants
earth/grass/dry variants
```

## חשוב

זה עדיין לא המפה הסופית.

```text
זה visual pass ראשון
לא משנה את graph של המבוך
לא משנה solvability
לא משנה pathfinding
לא משנה gameplay core
```

המטרה היא להתחיל למחוק את תחושת ה־grid בלי לשבור את הבסיס.

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

אחר כך:

```text
עצור Play Mode
הרץ Create Clean Maze Prototype Scene מחדש
```

בדוק:

```text
פינות פחות חדות
קירות פחות grid-like
חדרים/מסדרונות עם יותר שבירה טבעית
שאין חסימות לא רצויות
שה־FPS עדיין תקין
שהמבנה החדש של הריפו נשמר
```
