# Boredom & Dungeons — Clean Core V42

זו גרסה נקייה מלאה להחלפה, לא פאטץ׳.

## חדש ב־V42 — Professional Mouse Look

החלפתי את כיוון העכבר ממנגנון Raycast קופצני למנגנון מקצועי של mouse-delta yaw.

```text
Mouse movement → stable yaw rotation
No aim raycast for player/horse turning
No wall/trigger/horse/enemy hit jitter
```

## התנהגות

```text
Mouse move        Turn / aim
W                 Move forward relative to current look direction
S                 Move backward
A/D               Strafe relative to current look direction
Double tap WASD   Dodge
Q                 Ranged attack toward current look direction
Left/J            Sword attack toward current look direction
Right/K           Heavy attack toward current look direction
E                 Mount / Dismount
F held            Heal / Revive horse
M                 Toggle minimap
```

## למה זה יותר מקצועי

המצלמה עכשיו מאחורי השחקן, ולכן שימוש בנקודת עכבר בעולם גורם feedback: המצלמה מסתובבת, נקודת ה־ray משתנה, ואז הכיוון קופץ.  
ב־V42 הכיוון נשלט לפי תנועת העכבר עצמה, כמו במשחקי גוף שלישי/action RPG.

## נשמר

- מצלמה קבועה מאחורי השחקן.
- הסוס חוזר לשחקן כשהסכנה נגמרת.
- Dodge ב־Double Tap.
- Minimap בצד ימין למטה.
- חדר ההתחלה בלי אויבים.
- Ranged Attack עם מחסנית 3 יריות ו־reload של 6 שניות.
- Healing drop של 16% מאויבים.
- ריפוי סוס עם `F`.
- Death reset.
- אויבים לפי תפקיד.

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
