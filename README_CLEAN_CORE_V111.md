# Boredom & Dungeons — Clean Core V111

זו גרסת תיקון Collectibles + Ammo HUD אחרי V110.

## מה תוקן ב־collectibles

ה־Game Boy והסוללות כבר לא נמצאים בחדר/אזור ההתחלה.

עכשיו הם:

```text
מוחבאים רחוק יותר במפה
נמצאים בנקודות צדדיות יותר
יושבים בתוך hideout קטן
מוגנים על ידי guardian enemies
```

## Guardian spawner

נוסף:

```text
BDCollectibleGuardianSpawner
```

כאשר השחקן נכנס לרדיוס של collectible:

```text
מופיעים אויבים חזקים
הם מופיעים מסביב/מאחור
המטרה שלהם להפריע לקחת את ה־collectible ולברוח
```

ה־encounters:

```text
Game Boy  → 2 sword guardians + 1 charger
Battery A → 1 sword guardian + 1 charger
Battery B → 2 sword guardians
```

## חיווי כדורים / טעינה

שודרג ה־HUD של הירי.

עכשיו יש widget מקצועי יותר בצד ימין למטה:

```text
כמה כדורים נשארו
כמה כדורים במקסימום
pips לכדורים
bar טעינה
ring טעינה אנימטיבי
READY / RELOAD seconds
```

ה־reload מקבל אנימציה:

```text
פיפסים מהבהבים
בר טעינה מתקדם
טבעת טעינה מסתובבת/מתמלאת ויזואלית
```

## מה לא השתנה

```text
לא שונה מצלמה
לא שונה עכבר
לא שונה W/S/A/D
לא שונה aim
לא שונה damage
לא שונה reload duration
לא שונה AI כללי
לא שונה Game Boy cinematic flow
```

## קבצים חדשים

```text
Assets/_Project/Scripts/Runtime/Collectibles/BDCollectibleGuardianSpawner.cs
Assets/_Project/Design/Cinematics/COLLECTIBLE_GUARDIANS_V111.md
Assets/_Project/Design/Cinematics/AMMO_RELOAD_HUD_V111.md
```

## קבצים שעודכנו

```text
Assets/_Project/Scripts/Editor/BDCreateCleanMazePrototypeScene.cs
Assets/_Project/Scripts/Runtime/BDGameHud.cs
```

## התקנה נקייה

מחק לגמרי:

```text
Assets/_Project
```

ואז העתק מה־ZIP את:

```text
Assets/_Project
```

אחר כך:

```text
Stop Play Mode
Run Create Clean Maze Prototype Scene
Enter Play Mode
```

בדיקה:

```text
Game Boy לא בחדר התחלה
סוללות לא בחדר התחלה
כשמתקרבים ל־collectible מופיעים guardians
ה־HUD מציג ammo/reload מקצועי יותר
Q ירי + reload מציג אנימציה
```
