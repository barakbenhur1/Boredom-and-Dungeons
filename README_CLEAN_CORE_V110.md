# Boredom & Dungeons — Clean Core V110

זו גרסת תיקון ל־Game Boy collectibles / cinematic אחרי V109.

## מה תוקן

### 1. Destroy בזמן Editor Mode

הלוג הראה:

```text
Destroy may not be called from edit mode! Use DestroyImmediate instead.
BDGameBoyCollectible.MakeTriggerless
```

ב־V110 תוקן:

```text
אם המשחק רץ:
    Destroy(obj)

אם זה Editor Mode / scene builder:
    DestroyImmediate(obj)
```

זה מנקה את ה־warnings בזמן `Create Clean Maze Prototype Scene`.

### 2. consumeBatteries warning

הלוג הראה:

```text
BDGameBoyCinematicRoom.consumeBatteries is assigned but never used
```

ב־V110 הוספתי שימוש אמיתי:

```text
BDGameBoyInventory.ConsumeBatteries(int amount)
```

וה־cinematic משתמש בזה אם:

```text
consumeBatteries = true
```

ברירת מחדל נשארת:

```text
consumeBatteries = false
```

כלומר הסוללות עדיין נדרשות, אבל לא נצרכות אלא אם תפעיל את זה.

## מה לא השתנה

```text
לא שונה מצלמה
לא שונה עכבר
לא שונה W/S/A/D
לא שונה combat
לא שונה AI
לא שונה HUD
לא שונה מיקום Game Boy / סוללות / חדר cinematic
```

## נשמר מ־V109

- יותר רואים מהמפה בפריים.
- Game Boy collectible.
- Battery collectibles.
- Dark room cinematic.
- Player sits, inserts batteries, plays Game Boy.
- Natural Map Shape Pass.
- Enemy hit flash safe restore.
- Enemy stagger gates attacks.
- Dodge camera yaw lock.
- Mounted melee disabled.
- Projectile wall collision.

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
```

ה־Destroy warnings לא אמורים להופיע יותר.
