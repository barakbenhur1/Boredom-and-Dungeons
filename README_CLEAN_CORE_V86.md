# Boredom & Dungeons — Clean Core V86

זו גרסת תיקון קומפילציה/Editor נקייה אחרי V85.

## מה תוקן

ב־V85 הופיעה שגיאת Editor בזמן יצירת הסצנה:

```text
InvalidOperationException:
DontDestroyOnLoad can only be used in play mode

BD_GameFeelAudio
BDGameFeelAudio.EnsureRunner()
BDGameFeelAudio.PlayDamage()
BDHealth.RequestDamageCameraShake()
BDHealth.SetMaxHealth()
BDCreateCleanMazePrototypeScene.CreatePlayer()
```

## הסיבה

ה־Editor scene builder קורא ל־`BDHealth.SetMaxHealth(...)` בזמן שאיננו ב־Play Mode.  
זה הפעיל בטעות מערכות runtime כמו:

```text
BDGameFeelAudio
BDGameFeelEvents
BDHitStop
BDPerformanceGuard
```

וחלק מהן יצרו GameObject עם `DontDestroyOnLoad`, פעולה שמותרת רק בזמן Play Mode.

## התיקון ב־V86

נוספו guards:

```text
if (!Application.isPlaying) return;
```

במקומות הרלוונטיים:

```text
BDHealth.RequestDamageCameraShake
BDGameFeelAudio.Play / EnsureRunner / EnsureClips
BDGameFeelEvents
BDHitStop
BDPerformanceGuard
DontDestroyOnLoad bootstrap helpers
```

## התוצאה

```text
Editor scene builder עובד בלי exception
Play Mode עדיין מקבל audio / camera shake / hit-stop / performance guard
אין שינוי gameplay
אין HUD חדש
אין טקסט חדש
```

## נשמר מ־V85

- BDPlayerHealingPickup.Spawn compile fix.
- קליעים פוגעים בקירות ולא עוברים דרכם.
- Camera occlusion polish.
- Performance Guard.
- Enemy attack telegraphs.
- Ranged muzzle flash/trail/impact.
- Melee slash arcs.
- Player low-health / damage screen feedback.
- Enemy world HP bars.
- Procedural game-feel audio.
- Hit-stop לפגיעות.
- Healing pickup pulse/float/magnet/collect burst.
- Dodge i-frames.
- Enemy death burst.
- Heavy attack impact + knockback.
- Enemy separation.
- Q ammo/reload HUD.
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

חכה ל־Compile מלא ואז הרץ שוב את יצירת הסצנה.
