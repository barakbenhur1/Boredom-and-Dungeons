# Boredom & Dungeons — Clean Core V22

זו גרסה נקייה מלאה להחלפה, לא פאטץ׳.

## חדש ב־V22 — Ranged Attack לשחקן

נוסף לשחקן Special Ranged Attack:

```text
Q = Ranged Attack
```

## איך זה עובד

- הירייה יוצאת מהשחקן קדימה.
- היא פוגעת באויב הראשון שהיא פוגשת.
- היא עובדת גם כשהשחקן על הסוס.
- כשהשחקן רוכב, הכיוון הוא הכיוון של הסוס/השחקן בזמן הרכיבה.
- היא חזקה פי 3 מה־Fast Attack הרגיל.

ברירת מחדל:

```text
Fast Attack Damage = 25
Ranged Attack Damage = 75
Ranged Cooldown = 4 seconds
```

## למה cooldown בינוני

זו לא התקפה רגילה שאפשר להספים.  
היא מיועדת להיות כלי חזק לפתיחת קרב, עצירת אויב מסוכן, או יצירת מרווח בריחה.

## אינדיקציה

ב־Combat Debug/HUD מופיע:

```text
Q ranged x3
Ranged: READY
```

או:

```text
Ranged CD: 4.2s
```

## שליטה מלאה

```text
WASD / Arrows    Move player / move horse while mounted
Space            Jump on foot / horse jump while mounted
Left Shift       Dash when on foot
C                Toggle camera follow
Left Mouse / J   Fast attack
Right Mouse / K  Heavy attack
Q                Ranged attack, x3 fast damage, 4 second cooldown
E                Mount / Dismount
F held           Heal / Revive horse
```

## נשמר מ־V21

- Tactical Director.
- Patrol Guard שומר יציאות מראש.
- איגוף משמאל/ימין.
- הסוס לא בורח בכל ירידה, רק בזמן קרב.
- ריפוי סוס עם `F`.
- HP bars לשחקן ולסוס.
- נזק ומוות אמיתי לשחקן.
- קירות שקופים כשהם מסתירים.
- השחקן לא נמתח על הסוס.

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

## בדיקה

1. צור מבוך.
2. לחץ `Q` מול אויב — הירייה אמורה לצאת קדימה.
3. בדוק שהאויב מקבל נזק משמעותי.
4. לחץ `Q` שוב מיד — לא אמורה לצאת עוד ירייה עד סוף ה־4 שניות cooldown.
5. עלה על הסוס.
6. לחץ `Q` בזמן רכיבה — הירייה עדיין אמורה לעבוד.
