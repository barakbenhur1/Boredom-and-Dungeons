Boredom & Dungeons — Horse Start/Idle + Landing Animation Fix
================================================================

התיקונים
--------
1. הסוס בתחילת המשחק
   - מופיע מיד לצד השחקן.
   - אינו מתחיל רחוק ואז רץ אליו.
   - פונה לאותו כיוון של השחקן.
   - מתחיל במצב Idle.

2. הסוס כשהשחקן קרוב
   - אינו מסתובב לעקוב אחרי השחקן.
   - אינו מפנה את הגוף בכל פעם שהשחקן מקיף אותו.
   - אינו נסוג רק משום שהשחקן נמצא קרוב.
   - חזרה לשחקן ממרחק עדיין נשארת פעילה.

3. Landing Attack
   - בזמן ירידה מקפיצה מופיע רק אפקט המכה מלמעלה.
   - אפקט ה-Light/Heavy הרגיל מדוכא עבור אותה מכה בלבד.
   - הנזק, ה-Knockback וה-Hit Feedback הרגילים נשארים.
   - מכה רגילה על הקרקע ממשיכה להציג את האנימציה הרגילה.

התקנה
-----
מתוך תיקיית Boredom-and-Dungeons:

unzip -o ~/Downloads/Boredom-and-Dungeons_Horse_And_Landing_Animation_Fix.zip -d .
python3 tools/apply_horse_and_landing_animation_fix.py

לאחר מכן חזור ל-Unity והמתן לסיום הקומפילציה.

קבצים שמתעדכנים
---------------
Assets/_Project/Scripts/Runtime/BDHorseController.cs
Assets/_Project/Scripts/Runtime/BDPlayerCombat.cs
Assets/_Project/Scripts/Runtime/Combat/BDPlayerMeleeEnhancer.cs

גיבוי
-----
הסקריפט שומר את שלושת הקבצים המקוריים בתוך:

/tmp/BoredomAndDungeons_horse_landing_fix_backup_YYYYMMDD_HHMMSS

בדיקות Unity
------------
1. הפעל משחק חדש:
   הסוס צריך להופיע ליד השחקן כבר בפריים הראשון.

2. הקף את הסוס ברגל:
   כשהשחקן קרוב, הסוס לא צריך להסתובב לעקוב אחריו.

3. התרחק מאוד מהסוס:
   לאחר שאין קרב, מנגנון החזרה לשחקן עדיין צריך לעבוד.

4. בצע Light על הקרקע:
   צריך להופיע אפקט Light רגיל בלבד.

5. בצע Heavy על הקרקע:
   צריך להופיע אפקט Heavy רגיל בלבד.

6. קפוץ ובצע Light בזמן הירידה:
   צריך להופיע רק Landing Light.

7. קפוץ ובצע Heavy בזמן הירידה:
   צריך להופיע רק Landing Heavy.

8. בדוק גם Landing Attack שנשמר דרך ה-Attack Buffer.

9. ודא שאין Compiler Errors או Console Errors חדשים.

כוונון Inspector
----------------
ב-BDHorseController ניתן לשנות:

Start Local Offset From Player
- ברירת מחדל: X=2.35, Y=0, Z=0.45

Nearby Idle No Tracking Radius
- ברירת מחדל: 4.25

פקודות Git לאחר שהבדיקות עוברות
-------------------------------
git status --short
git diff --check
git add -A
git commit -m "Fix horse starting position and landing attack visuals"
git pull --rebase origin main
git push origin main

בדיקה:
git status
git log --oneline -5
