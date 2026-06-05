Boredom & Dungeons — Stage 16 Shared Summon Budget
=================================================

מה נוסף
-------
1. BDBossSharedSummonBudget
   - מגבלה משותפת לכל הזימונים במפגש.
   - סופר רק אויבים מזומנים שעדיין חיים.
   - משחרר מקום אוטומטית כשאויב מת או נהרס.
   - מנקה זימונים ב-Reset, Victory או Failure.
   - לא מאפשר לעבור את המקסימום.

2. BDSummonedEnemyLease
   - מחבר כל אויב מזומן לתקציב.
   - מאזין ל-BDHealth.Died.
   - מונע שחרור כפול של אותו slot.

3. BDBossSummonEmitter
   - זימון ידני או אוטומטי.
   - 1–3 אויבים או כל טווח אחר.
   - prefab קבוע או pool רנדומלי.
   - spawn points קבועים או fallback מסביב לבוס.
   - פועל רק כשמצב ה-Encounter הוא Active.
   - יכול לעצור כשערוץ החיים של הבעלים מגיע ל-0.

4. BDBossSummonBudgetDebugHud
   - חיווי Debug בלבד:
     Summons: current/max

5. כלי Setup ב-Unity
   Boredom & Dungeons
   → Bosses
   → Stage 16

התקנה
-----
מתוך תיקיית Boredom-and-Dungeons:

unzip -o ~/Downloads/Boredom-and-Dungeons_Stage16_Summon_Budget.zip -d .

לאחר מכן פתח Unity והמתן לסיום הקומפילציה.

הגדרת Encounter
---------------
1. בחר את ה-root שמכיל BDBossEncounterController.
2. הפעל:

Boredom & Dungeons
→ Bosses
→ Stage 16
→ Add Shared Summon Budget To Selected Encounter

ברירת המחדל היא 8 אויבים מזומנים חיים במקביל.
אפשר לשנות Maximum Alive Summons ב-Inspector.

הוספת Emitter
-------------
1. בחר את הבוס או את החלק שמזמן.
2. הפעל:

Boredom & Dungeons
→ Bosses
→ Stage 16
→ Add Summon Emitter To Selected Boss Part

3. ב-Inspector הגדר:
- Summon Prefabs
- Spawn Points
- Minimum Per Wave
- Maximum Per Wave
- Wave Cooldown
- Automatic Summoning

הגדרת המיני-בוסים
-----------------
Square Jumper:
- Summon Prefabs: sword / shooter / patrol
- wave לפי התכנון שלו
- אותו Shared Budget של כל ה-Encounter

Roller:
- Summon Prefabs: jumper / bomb / sword
- אותו Shared Budget

Quad Gunners:
- Emitter נפרד לכל דמות
- לכל Emitter prefab אחד קבוע בלבד
- Minimum Per Wave = 2
- Maximum Per Wave = 2
- Wave Cooldown = 10
- כל ארבעת ה-Emitters חולקים Budget אחד
- כך סוג הזימון של כל דמות נשאר קבוע

הבוס הסופי Stage 3:
- Emitter אחד לכל חצי
- pool של אויבים רגילים
- Wave Cooldown = 2
- שני החלקים חולקים Budget אחד
- Require Active Encounter = true
- זימונים פעילים רק כאשר סקריפט השלב מפעיל את ה-Emitter
- אם חצי על 0 עדיין אמור לזמן, בטל:
  Stop When Owner Health Is Zero

כללי התקציב
-----------
- אם נשאר slot אחד וביקשו 3, יזומן רק אויב אחד.
- אם התקציב מלא, לא נוצר אויב.
- אויב שמת משחרר slot מיד.
- אויב שנהרס משחרר slot.
- Encounter שהסתיים מנקה את כל הזימונים.
- כל Emitter באותו Encounter חייב להפנות לאותו Budget.

בדיקות Unity
------------
1. הגדר Maximum Alive Summons = 4.
2. הפעל שני Emitters שמנסים לזמן 3 כל אחד.
3. ודא שלא קיימים יותר מ-4 אויבים מזומנים.
4. הרוג אויב מזומן וודא שהמונה יורד ל-3.
5. ודא שה-wave הבא יכול למלא שוב את המקום שהתפנה.
6. ודא שאין זימון ב-Dormant, Intro או Transition.
7. ודא ש-Victory מנקה את הזימונים.
8. ודא ש-ResetEncounter מחזיר את התקציב ל-0.
9. בדוק Quad Gunners עם ארבעה Emitters ו-Budget משותף.
10. בדוק שאין Compiler Errors או Console Errors.

קבצים חדשים
------------
Assets/_Project/Scripts/Runtime/Bosses/Stage16/Summons/
Assets/_Project/Scripts/Editor/BossesStage16/Summons/

לא הוחלפו קבצים קיימים.

פקודות Git לאחר שהבדיקות עוברות
-------------------------------
git status --short
git diff --check
git add -A
git commit -m "Add shared summon budget for boss encounters"
git pull --rebase origin main
git push origin main

בדיקה:
git status
git log --oneline -5
