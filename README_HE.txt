Boredom & Dungeons — Reliable Auto Reload + Dynamic Player-Up Minimap
============================================================================

מה תוקן
-------

1. טעינה אוטומטית אמינה
------------------------
נוסף watchdog שפועל בכל Update:

- אם המחסנית ריקה.
- אם Reload עדיין לא התחיל.
- ואם השחקן אינו באמצע Charged Shot.

אז Reload מתחיל אוטומטית.

סיום Charged Shot מפעיל במפורש Reload חדש:
- התחמושת נקבעת ל-0.
- מצב Reload קודם מאופס אם נתקע.
- טיימר Reload חדש מתחיל מיד.
- אין צורך ללחוץ שוב על Q.

2. מיני-מפה דינמית לפי כיוון השחקן
-----------------------------------
המיני-מפה האמיתית היא BDMazeMinimap.
היא מצוירת באמצעות OnGUI ולא משתמשת במצלמת MinimapCamera.

התיקון החדש:
- מציג תמיד את כיוון השחקן כלפי מעלה.
- מסובב את המפה סביב נקודת השחקן.
- משאיר את סמן השחקן יציב.
- מעדכן את הסיבוב בכל פריים.
- משתמש ב-LastLookDirection ברגל.
- משתמש בכיוון הרכיבה/הכוונה בזמן רכיבה.
- מסובב גם את הסוס והחדרים.

3. ניקוי התיקון הקודם
---------------------
הסקריפט מסיר את:
BDMinimapPerspectiveAlignment.cs
BDMinimapPerspectiveAlignment.cs.meta

התקנה
-----
מתוך תיקיית Boredom-and-Dungeons:

unzip -o ~/Downloads/Boredom-and-Dungeons_Reliable_Reload_Player_Up_Minimap_Fix.zip -d .
python3 tools/apply_reliable_reload_player_up_minimap_fix.py

לאחר מכן חזור ל-Unity והמתן לסיום הקומפילציה.

קבצים שמתעדכנים
---------------
Assets/_Project/Scripts/Runtime/BDPlayerCombat.cs
Assets/_Project/Scripts/Runtime/BDMazeMinimap.cs

גיבוי
-----
/tmp/BoredomAndDungeons_reload_player_up_minimap_backup_YYYYMMDD_HHMMSS

בדיקות
------
1. בצע Charged Shot מלא.
2. מיד אחריו ה-HUD צריך להציג RELOAD.
3. אל תלחץ שוב על Q.
4. המחסנית צריכה להתמלא בסיום הטיימר.
5. רוקן גם את הכדור האחרון בירייה רגילה ובדוק Reload אוטומטי.
6. הסתובב במקום: המפה צריכה להסתובב בכל פריים.
7. הכיוון שאליו השחקן פונה צריך להיות תמיד למעלה.
8. סמן השחקן צריך להישאר יציב.
9. בדוק גם בזמן רכיבה.
10. ודא שאין Compiler Errors חדשים.

כוונון
------
BDMazeMinimap:
Rotate With Player Direction = true
Rotation Speed Degrees Per Second = 900
Rotation Offset Degrees = 0

אם הכיוון הפוך ב-180 מעלות:
Rotation Offset Degrees = 180

פקודות Git
----------
git status --short
git diff --check
git add -A
git commit -m "Fix automatic reload and rotate minimap with player direction"
git pull --rebase origin main
git push origin main
