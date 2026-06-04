# Boredom & Dungeons — V5 FORCE MOVEMENT FIX

המטרה של הגרסה הזאת היא לא “לשפר” את המערכת הישנה, אלא להכריח את הסצנה הנוכחית להשתמש בבקר תנועה חדש אחד.

## מה זה עושה

נוסף תפריט חדש:

```text
Boredom And Dungeons V5/FORCE Fix Movement In Current Scene
```

הפעולה הזאת:

1. מחפשת Player קיים בסצנה.
2. אם אין Player — יוצרת אחד.
3. מוחקת ממנו קומפוננטות תנועה ישנות לפי שמות:
   - PlayerController
   - PlayerMovement
   - PlayerDash
   - PlayerJump
   - DirectPrototypePlayerController
4. מוסיפה קומפוננט חדש:
   - `BD_DirectMovementV5`
5. מוודאת שיש CharacterController.
6. מוודאת שיש מצלמה.
7. יוצרת רצפה אם אין.
8. יוצרת ספירת אור לבנה אם אין.

## למה זה אמור לפתור

אם “שום דבר לא השתנה”, אז כנראה Unity עדיין משתמש בסקריפט ישן או בסצנה ישנה.  
הגרסה הזאת עובדת על הסצנה הנוכחית ומחליפה את הבקר בפועל.

## התקנה

1. אל תמחק כלום כרגע אם אתה לא רוצה.
2. תעתיק את התיקייה:
```text
Assets/_Project/Scripts/EmergencyFix
Assets/_Project/Scripts/Editor
```
מה־ZIP לתוך הפרויקט.

או פשוט תעתיק את כל `Assets/_Project`.

3. חכה ל־compile.

4. פתח את הסצנה שבה אתה בודק.

5. לחץ:
```text
Boredom And Dungeons V5/FORCE Fix Movement In Current Scene
```

6. לחץ Play.

## מה לבדוק ב־Inspector

בחר את השחקן.  
חייב להיות עליו:

```text
BD_DirectMovementV5
```

אם זה לא שם — התיקון לא הופעל על השחקן הנכון.

## שליטה

Keyboard:
- W / Up Arrow = קדימה במסך
- S / Down Arrow = אחורה במסך
- A / Left Arrow = שמאלה
- D / Right Arrow = ימינה
- Space = Jump
- Left Shift = Dash

Mouse / Mobile-style:
- החזק עכבר שמאלי וגרור = תנועה כמו finger trace
- Touch במובייל = גרירה

## Debug

בזמן Play יש overlay שמראה:

- Move X
- Move Y
- Backend
- Controller name

אם `Move Y` משתנה אבל הדמות לא זזה למעלה/למטה — זו בעיית CharacterController/Collision.
אם `Move Y` לא משתנה — זו בעיית Input/Focus.
