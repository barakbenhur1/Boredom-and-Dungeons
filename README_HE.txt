Boredom & Dungeons — Projectile Knockback Policy
================================================

מה השלב מתקן
-------------
כדורים של השחקן ממשיכים לעשות נזק לכל אויב חוקי, אבל Knockback נקבע
לפי BDCombatantProfile:

Regular Enemy
- Rank: Regular
- Receives Player Projectile Knockback: true
- מקבל Knockback.

Small Mini-Boss / Quad Gunner
- Rank: MiniBoss
- Receives Player Projectile Knockback: true
- מקבל Knockback.

Large Mini-Boss
- Rank: MiniBoss
- Receives Player Projectile Knockback: false
- אינו מקבל Knockback.

Final Boss
- Rank: Boss
- תמיד חסין ל-Knockback.
- גם אם מסומן true בטעות, הקוד מכריח false.

כללים חשובים
------------
- נזק הכדור אינו מתבטל בגלל חסינות ל-Knockback.
- Hit feedback וצליל פגיעה עדיין עובדים.
- אויב רגיל בלי BDCombatantProfile ממשיך לקבל Knockback כברירת מחדל.
- הכיוון של ה-Knockback הוא כיוון תנועת הכדור.
- CharacterController משתמש ב-BDKnockbackReceiver.
- Rigidbody דינמי מקבל VelocityChange.
- לא מוסיפים BDKnockbackReceiver לבוס חסין.

התקנה
-----
1. חלץ את כל תוכן ה-ZIP ישירות לתוך Boredom-and-Dungeons.
2. מתוך תיקיית הפרויקט הרץ:

python3 tools/apply_projectile_knockback_update.py

3. פתח Unity והמתן לסיום הקומפילציה.

הקבצים שמתעדכנים
----------------
Assets/_Project/Scripts/Runtime/BDPlayerRangedProjectile.cs
Assets/_Project/Scripts/Runtime/BDCombatantProfile.cs

הקבצים שנוספים
--------------
Assets/_Project/Scripts/Editor/CombatProfiles/BDCombatantProfileSetupTools.cs
Assets/_Project/Scripts/Editor/CombatProfiles/BDCombatantProfileSetupTools.cs.meta
Assets/_Project/Scripts/Editor/CombatProfiles.meta
tools/apply_projectile_knockback_update.py

כלי Unity
---------
לאחר הקומפילציה מופיע תפריט:

Boredom & Dungeons
→ Combat Profiles
→ Set Selected

אפשרויות:
- Regular Enemy
- Small Mini-Boss
- Large Mini-Boss
- Final Boss

הגדרת Quad Gunners
------------------
בחר כל אחד מארבעת החברים והפעל:
Small Mini-Boss

כך כל אחד:
- נחשב MiniBoss.
- מקבל Knockback.
- נשאר בעל צבע, ירי וזימון קבועים משלו.

הגדרת המיני-בוסים הגדולים
-------------------------
Square Jumper, Roller ו-Serpent:
Large Mini-Boss

הגדרת הבוס הסופי
----------------
Final Boss

בדיקות Unity
------------
1. ירה באויב רגיל: נזק + Knockback.
2. ירה ב-Quad Gunner: נזק + Knockback.
3. ירה במיני-בוס גדול: נזק בלי Knockback.
4. ירה בבוס הסופי: נזק בלי Knockback.
5. ודא שאין BDKnockbackReceiver חדש על בוס חסין.
6. ודא שהכדור עדיין נעצר בקירות.
7. ודא שהכדור לא פוגע בשחקן או בסוס.
8. ודא שאין Compiler Errors או Console Errors חדשים.

גיבוי
-----
הסקריפט שומר גיבוי אוטומטי בתוך:
/tmp/BoredomAndDungeons_projectile_knockback_backup_YYYYMMDD_HHMMSS

פקודות Git לאחר שהבדיקות עוברות
-------------------------------
git status --short
git diff --check
git add -A
git commit -m "Apply projectile knockback policy by combatant profile"
git pull --rebase origin main
git push origin main

בדיקת סיום:
git status
git log --oneline -5
