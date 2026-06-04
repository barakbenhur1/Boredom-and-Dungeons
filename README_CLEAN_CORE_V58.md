# Boredom & Dungeons — Clean Core V58

זו גרסת תיקון קומפילציה נקייה אחרי V57.

## מה תוקן

ב־V57 נוצרה שגיאת קומפילציה ב־`BDPlayerCombat.cs`:

```text
CS1519: Invalid token 'return'
CS1022: Type or namespace definition, or end-of-file expected
```

ב־V58 הוחלף `BDPlayerCombat.cs` בצורה קשיחה ונקייה.

התקפות וירי משתמשים עדיין במערכת הניווט המרכזית של V57:

```text
Player LastLookDirection
Mounted Horse LastMountedAimDirection
Fallback transform.forward
```

## התקנה נקייה

מחק לגמרי:

```text
Assets/_Project
```

ואז העתק מה־ZIP את:

```text
Assets/_Project
```

חכה ל־Compile מלא.
