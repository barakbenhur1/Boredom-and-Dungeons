Boredom & Dungeons — Tap/Hold, Reload, Minimap, Stage 17 Fix
================================================================

מה תוקן
-------

1. לחיצה קצרה לעומת לחיצה ארוכה
--------------------------------
כאשר יש 2 כדורים או יותר:

- שחרור לפני 0.22 שניות:
  נורת ירייה רגילה אחת.

- המשך החזקה אחרי 0.22 שניות:
  מתחילה טעינת הירייה הגדולה.

- שחרור לאחר שהטעינה התחילה אבל לפני הסיום:
  הירייה מתבטלת ללא צריכת תחמושת.

- השלמת הטעינה:
  נורה הקליע הגדול ומשתמש בכל הכדורים שנותרו.

כאשר נשאר כדור אחד:
- הוא נורה מיד.
- אין השהיה ואין טעינה.

2. Reload אוטומטי
-----------------
אחרי הירייה הטעונה:
- המחסנית מתרוקנת.
- Reload מתחיל מיד.
- אין צורך ללחוץ שוב על Q.

3. מיני-מפה
------------
נוסף BDMinimapPerspectiveAlignment:

- מיישר את החלק העליון של המיני-מפה לחלק העליון של
  נקודת המבט במצלמת המשחק.
- עוקב אחרי השחקן ב-X/Z.
- מתקן את הסיבוב ב-LateUpdate.
- מזהה מצלמה בשם MinimapCamera, מצלמת RenderTexture,
  או מצלמת Orthographic עם viewport קטן.

מומלץ לקרוא למצלמת המפה:
MinimapCamera

4. Stage 17
-----------
תוקן:

Environment.TickCount

ל:

System.Environment.TickCount

התקנה
-----
מתוך תיקיית Boredom-and-Dungeons:

unzip -o ~/Downloads/Boredom-and-Dungeons_Tap_Charge_Reload_Minimap_Stage17_Fix.zip -d .
python3 tools/apply_tap_charge_reload_minimap_stage17_fix.py

לאחר מכן חזור ל-Unity והמתן לסיום הקומפילציה.

בדיקות
------
1. Tap מהיר עם 3 כדורים יורה ירייה רגילה אחת.
2. החזקה מעבר ל-0.22 שניות מתחילה טעינה.
3. שחרור באמצע הטעינה מבטל בלי לצרוך כדור.
4. השלמת הטעינה צורכת את כל הכדורים.
5. Reload מתחיל מיד בלי לחיצה נוספת.
6. כדור אחרון נורה מיד.
7. המיני-מפה מיושרת לנקודת המבט של המשחק.
8. שגיאת TickCount נעלמה.
9. אין Compiler Errors חדשים.

פקודות Git
----------
git status --short
git diff --check
git add -A
git commit -m "Fix tap shooting reload minimap alignment and Stage 17 seed"
git pull --rebase origin main
git push origin main
