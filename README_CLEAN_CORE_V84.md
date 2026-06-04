# Boredom & Dungeons — Clean Core V84

זו גרסה נקייה מלאה להחלפה, לא פאטץ׳.

## מה תוקן ב־V84

### קליעים פוגעים בקירות

לפני V84, חלק מהקליעים יכלו לעבור דרך קירות/עמודים/מכשולים.

ב־V84:

```text
Player ranged projectile → hits enemies OR walls
Enemy ranged projectile → hits player/horse OR walls
Wall hit → projectile impact burst
Projectile destroyed on wall hit
```

## חשוב

```text
הקליע לא אמור להתפוצץ מהרצפה מיד אחרי שיוצא
הקליע עדיין פוגע באויבים/שחקן/סוס לפי הצד שלו
הקליע כבר לא עובר דרך קירות
```

## מה טכנית

```text
BDPlayerRangedProjectile uses SphereCastNonAlloc for wall collision
BDRangedProjectile uses SphereCastNonAlloc for wall collision
Enemy forgiving hit radius נשאר דרך OverlapCapsuleNonAlloc
Wall collision radius קטן יותר כדי לא לפגוע ברצפה
```

## נשמר מ־V83

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
- Camera shake עדין לקרב.
- Dodge i-frames.
- Dodge afterimage.
- סימון יציאה בלי טקסט.
- אין ROOM CLEAR.
- Enemy death burst.
- Heavy attack impact + knockback.
- Enemy separation.
- Screen-space center dead zone.
- Q ammo/reload HUD.
- Continuous mouse aim smoothing.
- Movement smoothing.
- ירי על הסוס משתמש ב־Horse.LastMountedAimDirection.
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
