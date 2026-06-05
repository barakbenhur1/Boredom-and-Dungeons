Boredom & Dungeons — Charged Shot + Forward Dodge Visual Fix
================================================================

1. Charged Shot
---------------
כפתור הירי נשאר Q.

כאשר נשאר כדור אחד:
- לחיצה על Q יורה מיד.
- אין טעינה.

כאשר נשארים 2 כדורים או יותר:
- לחיצה והחזקה מתחילה טעינה.
- אם משחררים לפני סיום הטעינה, הירייה מתבטלת.
- לא נצרכת תחמושת במקרה של ביטול.
- בסיום הטעינה הירייה נורת אוטומטית.
- היא צורכת את כל הכדורים שנשמרו בתחילת הטעינה.
- הנזק הוא:
  נזק כדור רגיל × מספר הכדורים שנצרכו.
- Weapon Damage Boost מוחל לפני הכפלת מספר הכדורים.

זמני טעינה ברירת מחדל
--------------------
2 כדורים: 0.90 שניות
3 כדורים: 1.35 שניות
4 כדורים: 1.80 שניות
5 כדורים: 2.25 שניות
6 כדורים: 2.70 שניות

הזמן מוגבל ל-3.20 שניות כברירת מחדל.

חיווי ויזואלי
-------------
בזמן הטעינה:
- כדור אנרגיה גדל מול השחקן.
- שתי טבעות חלקיקים מסתובבות.
- מהירות הסיבוב והאור מתחזקים עם ההתקדמות.
- צבע האפקט משתנה לפי מספר הכדורים.

בזמן הירי:
- הקליע גדול יותר ככל שהוא צורך יותר כדורים.
- הילה כפולה מסתובבת סביב הקליע.
- נוסף Trail חזק יותר.
- Muzzle burst גדול יותר.
- Impact burst גדול יותר.
- Knockback, Hit Radius ו-Camera Shake מתחזקים בהדרגה.

ביטול:
- אם Q משתחרר לפני סיום הטעינה, מופיע אפקט התכווצות קצר.
- לא נורה קליע.
- לא נצרכת תחמושת.

2. Forward Dodge Ground Effect
------------------------------
אפקט ההתחמקות קדימה נוצר כעת 0.82 יחידות מאחורי הדמות.
מרחק ההתחמקות, המהירות וה-i-frames אינם משתנים.
ההזזה חלה רק על התחמקות קדימה.

התקנה
-----
מתוך תיקיית Boredom-and-Dungeons:

unzip -o ~/Downloads/Boredom-and-Dungeons_Charged_Shot_And_Dodge_Visual.zip -d .
python3 tools/apply_charged_shot_and_dodge_visual.py

לאחר מכן חזור ל-Unity והמתן לסיום הקומפילציה.

קבצים חדשים
------------
Assets/_Project/Scripts/Runtime/Combat/BDChargedShotVisuals.cs
Assets/_Project/Scripts/Runtime/Combat/BDChargedShotVisuals.cs.meta

קבצים שמתעדכנים
---------------
Assets/_Project/Scripts/Runtime/BDPlayerCombat.cs
Assets/_Project/Scripts/Runtime/BDPlayerController.cs

גיבוי
-----
הסקריפט שומר גיבוי של שני הקבצים המקוריים בתוך:

/tmp/BoredomAndDungeons_charged_shot_backup_YYYYMMDD_HHMMSS

בדיקות Unity
------------
1. מחסנית עם כדור אחד:
   Q יורה מיד.

2. מחסנית עם 2+:
   החזק Q ושחרר מוקדם.
   לא אמורה להיירות ירייה ולא אמורה להיצרך תחמושת.

3. החזק Q עד סוף הטעינה:
   נורה קליע אחד ונצרכת כל התחמושת שנשמרה.

4. בדוק נזק:
   3 כדורים צריכים לעשות נזק כולל של 3 כדורים רגילים.

5. אסוף Extra Ammo Boost:
   זמן הטעינה והחיווי צריכים לגדול בהתאם לכמות שנותרה.

6. אסוף Weapon Damage Boost:
   הנזק של כל כדור בחישוב המצטבר צריך לעלות.

7. בדוק ירי בזמן רכיבה:
   הירי עדיין מכוון לעכבר ולא מסובב את הסוס.

8. בדוק פגיעה בקיר:
   הקליע עדיין נעצר ומציג Impact.

9. בדוק אויב קטן ומיני-בוס גדול:
   מדיניות ה-Knockback הקיימת נשמרת.

10. בצע התחמקות קדימה:
    אפקט הקרקע צריך להופיע מאחורי הדמות ולהיות קריא יותר.

11. בצע התחמקות לאחור ולצדדים:
    המיקום הקיים שלהם לא אמור להשתנות.

12. ודא שאין Compiler Errors או Console Errors חדשים.

כוונון Inspector
----------------
BDPlayerCombat:
- Charged Shot Base Duration
- Charged Shot Seconds Per Additional Ammo
- Charged Shot Maximum Duration
- Charged Projectile Scale Per Extra Ammo
- Charged Hit Radius Per Extra Ammo
- Charged Knockback Per Extra Ammo
- Charged Speed Per Extra Ammo

BDPlayerController:
- Forward Dodge Visual Rear Offset

פקודות Git לאחר שהבדיקות עוברות
-------------------------------
git status --short
git diff --check
git add -A
git commit -m "Add charged magazine shot and improve forward dodge visual"
git pull --rebase origin main
git push origin main

בדיקה:
git status
git log --oneline -5
