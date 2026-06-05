Boredom & Dungeons — Stability Gate
====================================

מטרת החבילה
------------
זהו שער הייצוב שחייב לעבור לפני שמסמנים את Stage 16 או Stage 17
כמושלמים ולפני שמתחילים לממש את ארבעת המיני-בוסים.

החבילה אינה משנה gameplay קיים.
היא מוסיפה כלי בדיקה בלבד.

שכבה 1 — Terminal Source Scan
------------------------------
פועלת גם כאשר Unity לא מצליח לקמפל.

בודקת:
- UnityEditor imports בתוך Runtime.
- GUID כפול בקבצי meta.
- קבצים חשובים בלי meta.
- הגדרות type כפולות שאינן partial.
- RuntimeInitializeOnLoadMethod שדורשים בדיקת כפילות.
- רכיב יישור המיני-מפה הישן לצד BDMazeMinimap האמיתי.

הרצה:

python3 tools/bd_stability_source_scan.py

הפקודה מחזירה exit code 1 אם נמצאו Blockers.

הדוחות נשמרים ב:

Library/BoredomAndDungeons/StabilityReports/

שכבה 2 — Unity Editor Stability Gate
-------------------------------------
לאחר שהקומפילציה עוברת, ב-Unity:

Boredom And Dungeons
→ Validation
→ Run Full Stability Gate

בודקת את כל הסצנות וה-Prefabs תחת Assets/_Project:

- Missing Scripts.
- שני עותקים של אותו MonoBehaviour על אותו GameObject.
- יותר ממערכת קריטית אחת:
  Player, PlayerController, PlayerCombat, Horse, HUD, Minimap.
- מערכות מתחרות לאותו תפקיד.
- Runtime installers כפולים.
- שחקן בלי PlayerController / PlayerCombat / Health.
- סוס בלי HorseHealth.
- אויב עם יותר מ-BDHealth אחד.
- UnityEditor בקוד Runtime.
- GUID כפולים.

בנייה מחדש + בדיקה
------------------
אפשר להפעיל:

Boredom And Dungeons
→ Validation
→ Rebuild Prototype And Run Gate

הכלי מפעיל:

Boredom And Dungeons
→ Create Clean Maze Prototype Scene

ולאחר מכן מריץ את שער הייצוב המלא.

Play Mode Smoke Test
--------------------
פתח:

Boredom And Dungeons
→ Validation
→ Open Play Mode Smoke Checklist

הבדיקות:

- Movement.
- Jump / Landing.
- Dodge + i-frames.
- Light / Heavy / Attack Buffer.
- Landing Attack.
- Physical Parry.
- Tap / Charged Shot.
- Automatic Reload.
- Horse.
- Damage / Death / Reset.
- Dynamic Minimap.
- Console.

PASS report ניתן לשמור רק לאחר שכל הבדיקות סומנו כעוברות.

דוחות
-----
כל הדוחות נשמרים מחוץ ל-Assets כדי שלא ייכנסו ל-build או לגיט:

Library/BoredomAndDungeons/StabilityReports/

התקנה
-----
מתוך תיקיית Boredom-and-Dungeons:

unzip -o ~/Downloads/Boredom-and-Dungeons_Stability_Gate.zip -d .

סריקה ראשונה:

python3 tools/bd_stability_source_scan.py

לאחר שהסריקה אינה מציגה Blockers:

1. פתח Unity.
2. המתן לסיום הקומפילציה.
3. הרץ Rebuild Prototype And Run Gate.
4. הרץ Play Mode Smoke Checklist.
5. אל תסמן שלב כמושלם לפני שלושת ה-PASS reports.

קבצים חדשים
------------
Assets/_Project/Scripts/Editor/Validation/
tools/bd_stability_source_scan.py

לא הוחלפו או נמחקו קבצי gameplay.

פקודות Git לאחר שכל הבדיקות עוברות
----------------------------------
git status --short
git diff --check
git add -A
git commit -m "Add project stability gate and smoke test tools"
git pull --rebase origin main
git push origin main

בדיקת סיום:

git status
git log --oneline -5
