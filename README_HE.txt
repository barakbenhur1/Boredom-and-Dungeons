Boredom & Dungeons — Stage 16 Duplicate HUD Fix
================================================

הבעיה
-----
בפרויקט כבר קיימת מחלקה בשם BDBossHealthHud.
חבילת Stage 16 הוסיפה קובץ נוסף עם אותה מחלקה ולכן Unity דיווח על:

CS0101
CS0579
CS0111

התיקון
------
התיקון מסיר רק את הקובץ הכפול שנוסף בתוך Stage16:

Assets/_Project/Scripts/Runtime/Bosses/Stage16/BDBossHealthHud.cs
Assets/_Project/Scripts/Runtime/Bosses/Stage16/BDBossHealthHud.cs.meta

ה-HUD המקורי שכבר קיים בפרויקט נשאר ללא שינוי.

התקנה
-----
מתוך תיקיית Boredom-and-Dungeons:

unzip -o ~/Downloads/Boredom-and-Dungeons_Stage16_Duplicate_Hud_Fix.zip -d .
python3 tools/remove_duplicate_stage16_boss_hud.py

לאחר מכן חזור ל-Unity והמתן לסיום הקומפילציה.

פקודות Git לאחר שהקומפילציה עוברת
---------------------------------
git status --short
git diff --check
git add -A
git commit -m "Remove duplicate Stage 16 boss health HUD"
git pull --rebase origin main
git push origin main
