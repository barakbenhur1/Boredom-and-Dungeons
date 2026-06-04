# Boredom & Dungeons — Clean Core V87

זו גרסת תיקון שליטה/מצלמה/רכיבה אחרי V86.

## מה תוקן ב־V87

### 1. אין התקפת חרב על הסוס

בזמן רכיבה:

```text
Left click / J  → לא עושה melee
Right click / K → לא עושה heavy melee
Q               → יורה כרגיל
```

כלומר mounted combat הוא ranged-only.

### 2. הסוס לא מסתובב כל הזמן לפנות לשחקן

ב־V86 הסוס עדיין היה מסתובב אל השחקן כשהוא חזר או עמד לידו.

ב־V87:

```text
אם הסוס הגיע לרדיוס נוחות → הוא עומד ולא מסתובב אל השחקן
אם הוא קרוב מדי וזז אחורה → הוא פונה לכיוון התנועה, לא לשחקן
אם הוא באמת חוזר מרחוק → הוא פונה לכיוון התנועה
```

### 3. המצלמה לא מסתובבת לפי aim/עכבר בזמן קרב

ב־V86 המצלמה עדיין השתמשה ב־LastLookDirection / LastMountedAimDirection, ולכן בזמן קרב/aim היא הסתובבה.

ב־V87 המצלמה משתמשת רק בתנועה אמיתית:

```text
Player actual movement direction
Horse actual mounted movement direction
```

אם השחקן עומד במקום ורק מכוון/יורה/מרביץ:

```text
המצלמה נשארת יציבה
```

### 4. מצלמה פחות קופצנית

שונו ערכי מצלמה:

```text
followSmooth = 9.5
rotationSmooth = 8.5
cameraYawDegreesPerSecond = 86
cameraShakeMultiplier = 0.42
cameraShakeSmoothing = 18
```

## נשמר מ־V86

- Editor scene builder fix.
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

חכה ל־Compile מלא ואז בדוק רכיבה, קרב, מצלמה וסוס ליד השחקן.
