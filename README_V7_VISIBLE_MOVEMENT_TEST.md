# Boredom & Dungeons — V7 Visible Movement Test

הגרסה הזאת נועדה לפתור מצב שבו W/S נראים כאילו הם מזיזים מצלמה ולא את השחקן.

## מה השתנה

1. יש סצנת בדיקה עם סמני כיוון ברורים:
   - NORTH / W / UP
   - SOUTH / S / DOWN
   - EAST / D / RIGHT
   - WEST / A / LEFT

2. יש רצפה עם קוביות צבעוניות מסביב, כדי שתראה תנועה גם אם המצלמה עוקבת.

3. ה־Debug overlay מציג:
   - Move X
   - Move Y
   - Player Position X/Z
   - האם Camera Follow פעיל

4. נוסף כפתור בדיקה:
```text
C = Toggle Camera Follow
```

אם אתה לוחץ C ומכבה Camera Follow, אז W/S חייבים להזיז את השחקן על המסך בצורה ברורה.

## התקנה

1. תעתיק את `Assets/_Project` מה־ZIP לתוך Unity.
2. חכה ל־Compile.
3. לחץ בתפריט:
```text
Boredom And Dungeons V7/Create Visible Movement Test Scene
```

4. לחץ Play.
5. לחץ פעם אחת בתוך Game/Simulator.
6. בדוק:

```text
W / Up Arrow    = השחקן הולך לכיוון NORTH
S / Down Arrow  = השחקן הולך לכיוון SOUTH
A / Left Arrow  = WEST
D / Right Arrow = EAST
C               = מכבה/מדליק Camera Follow
```

## אם W/S עדיין מזיזים את מצלמת העריכה

זה אומר שה־Scene View בפוקוס, לא Game/Simulator.

במצב כזה:
1. לחץ Play.
2. לחץ פעם אחת בתוך חלון Game/Simulator.
3. אל תיגע ב־Scene View.
4. נסה שוב W/S.

## סימן ברור שזה עובד

ב־Debug overlay:
- W צריך לעשות `Move Y: 1.00`
- S צריך לעשות `Move Y: -1.00`
- Position Z צריך להשתנות כשאתה לוחץ W/S
