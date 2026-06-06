Boredom & Dungeons — Obsolete Fields Fix V2
================================================

גרסה זו מיועדת גם למצב החלקי שנוצר לאחר שהסקריפט הקודם:

- הסיר rotationSpeedDegreesPerSecond
- הסיר snapToMovementCardinals
- נעצר ב-mapRotationInitialized

V2 היא idempotent:
- שדה שכבר הוסר מסומן SKIP.
- שדה שעדיין קיים מוסר.
- mapRotationInitialized מזוהה גם כשהוא private bool רגיל
  ללא SerializeField וללא ערך התחלתי.
- השמות נבדקים שוב בסיום כדי לוודא שלא נשארה הפניה מתה.

התקנה
-----

מתוך Boredom-and-Dungeons:

unzip -o ~/Downloads/Boredom-and-Dungeons_Remove_Obsolete_Camera_Minimap_Fields_V2.zip -d .
python3 tools/apply_remove_obsolete_camera_minimap_fields_v2.py

פלט צפוי במצב שלך:

SKIP: rotationSpeedDegreesPerSecond declaration is already absent.
SKIP: snapToMovementCardinals declaration is already absent.
REMOVED: mapRotationInitialized declaration (1)
REMOVED: mapRotationInitialized assignment(s) (1)
PATCHED: BDMazeMinimap.cs
REMOVED: minimumMovementDirectionMagnitude declaration (1)
REMOVED: rotateOnlyWhenActuallyMoving declaration (1)
PATCHED: BDCameraFollow.cs
SUCCESS: all five obsolete fields are gone.

לאחר מכן:

1. חזור ל-Unity.
2. המתן לסיום הקומפילציה.
3. נקה Console.
4. ודא שאין יותר את חמש אזהרות CS0414.
5. הרץ TEST EVERYTHING.
