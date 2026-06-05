Boredom & Dungeons — One-Click QA Self-Scan Fix V2
===================================================

למה V1 נכשל
-----------
הסקריפט הראשון חיפש בלוק קוד שלם עם רווחים ופורמט מדויקים.
הקובץ המקומי כבר היה שונה מעט, ולכן התקבלה השגיאה:

Expected editor-source scan block was not found exactly once.

מה V2 עושה
----------
V2 אינו מחפש בלוק שלם.

הוא מאתר לפי הסדר:

1. את אזור סריקת קובצי ה-Editor:
   if (Directory.Exists(editorRoot))

2. את השורה:
   string relative = MakeRelative(file);

3. מוסיף מיד אחריה החרגה רק עבור:
   Assets/_Project/Scripts/Editor/Validation/BDOneClickQAWindow.cs

הבדיקה ממשיכה לסרוק את כל שאר קובצי ה-Editor.

התקנה
-----
מתוך תיקיית Boredom-and-Dungeons:

unzip -o ~/Downloads/Boredom-and-Dungeons_One_Click_QA_Self_Scan_Fix_V2.zip -d .
python3 tools/apply_one_click_qa_self_scan_fix_v2.py

לאחר מכן:

1. חזור ל-Unity.
2. המתן לסיום הקומפילציה.
3. נקה את Console.
4. הפעל:

Boredom And Dungeons
→ TEST EVERYTHING

התוצאה הצפויה
-------------
ה-Blocker הבא צריך להיעלם:

EDITOR_RENDERER_MATERIAL_ACCESS
Assets/_Project/Scripts/Editor/Validation/BDOneClickQAWindow.cs

אם אין בעיות נוספות, יופיע:

AUTOMATED PASS
