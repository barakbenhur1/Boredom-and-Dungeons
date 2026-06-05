Boredom & Dungeons — Camera, Minimap and Enemy Safety Fix
===========================================================

תיקונים
-------

מצלמה:
- לחיצה על S/A/D אינה מסובבת את המצלמה.
- המצלמה מקבלת Yaw מכיוון המבט של השחקן.
- כשאין תנועת עכבר מכוונת, הכיוון נשמר.

מיני-מפה:
- משתמשת בכיוון התנועה האמיתי.
- המישור מחולק לארבעה כיוונים.
- האלכסונים ב-45 מעלות הם הגבולות בין האזורים.
- הכיוון הקרוב ביותר נבחר.
- בדיוק על אלכסון נשמר הכיוון הקודם.
- הסיבוב תמיד מיידי ובכפולות של 90 מעלות.
- החדרים נשארים מקבילים למסגרת.

אויבים:
- אויבים חדשים נבדקים מול קירות ומכשולים.
- Spawn לא חוקי מועבר לנקודה בטוחה.
- אויב שיורד או נוחת על השחקן מועבר הצידה.
- Square Jumper בוחר מראש נקודת נחיתה בטוחה.
- זימון של Square Jumper נבדק לפני יצירת האויב.
- אם אין מקום חוקי, הזימון מדולג.

התקנה
-----

מתוך Boredom-and-Dungeons:

unzip -o ~/Downloads/Boredom-and-Dungeons_Camera_Minimap_Enemy_Safety_Fix.zip -d .
python3 tools/apply_camera_minimap_enemy_safety_fix.py

לאחר מכן:

1. חזור ל-Unity.
2. המתן לקומפילציה.
3. נקה Console.
4. הרץ:

Boredom And Dungeons
→ TEST EVERYTHING

בדיקות
------

1. בלי להזיז עכבר, לחץ S כמה פעמים.
   המצלמה לא מסתובבת.

2. זוז באלכסון:
   כל עוד רכיב הקדימה גדול יותר, המפה מכוונת קדימה.
   ברגע שרכיב הצד גדול יותר, המפה קופצת לצד.

3. המפה לעולם אינה בזווית שאינה כפולה של 90 מעלות.

4. עמוד מתחת לאויב קופץ.
   הוא לא נוחת בתוך השחקן.

5. הפעל זימונים ליד קירות.
   אויבים לא מופיעים בתוך קירות.

6. אם אין נקודה חוקית לזימון, הזימון מדולג.

קבצים חדשים
------------

Assets/_Project/Scripts/Runtime/EnemyPlacementSafety/BDEnemyPlacementSafety.cs

קבצים שמתעדכנים
---------------

Assets/_Project/Scripts/Runtime/BDCameraFollow.cs
Assets/_Project/Scripts/Runtime/BDMazeMinimap.cs
Assets/_Project/Scripts/Runtime/Bosses/MiniBosses/SquareJumper/BDSquareJumperMiniBoss.cs

פקודות Git
----------

git status --short
git diff --check
git add -A
git commit -m "Fix camera minimap sectors and enemy placement safety"
git pull --rebase origin main
git push origin main
