<div dir="rtl">

# C# Programming in the .NET Framework — יום 1

חומרי הלימוד ליום הראשון של הקורס: **תכנות מונחה-עצמים, אוספים וטיפול בחריגות**, ב-C# 13 / .NET 10.

## לפני היום הראשון

1. התקינו את הכלים לפי [`00-Setup/INSTALL.md`](00-Setup/INSTALL.md): ‏.NET 10 SDK, ‏Visual Studio 2022 או VS Code עם התוסף C# Dev Kit, ו-Git.
2. הורידו את המאגר:

<div dir="ltr">

```bash
git clone https://github.com/Lior1234yaa/csharp-course-day1.git
```

</div>

3. בדקו שהסביבה מוכנה:

<div dir="ltr">

```bash
cd csharp-course-day1/00-Setup/VerifySetup
dotnet run
```

</div>

אם `dotnet` לא מזוהה בטרמינל של VS Code אחרי ההתקנה: סגרו את VS Code לגמרי (`File` → `Exit`) ופתחו מחדש.

## מה יש במאגר

<div dir="ltr">

```text
00-Setup/
  INSTALL.md                     מדריך התקנה
  CHEATSHEET.md                  דף עזר מהיר ל-C#
  VerifySetup/                   בדיקת סביבה (dotnet run)
Day1-OOP-Collections-Exceptions/
  README.md                      סדר היום, מטרות, קישורים
  Notes/                         חומרי הלימוד, קובץ לכל מודול
  Slides/Day1.pptx               המצגת
  Demos/                         הדמואים מהשיעור (dotnet run)
  Exercises/README.md            14 תרגילים קצרים
  Labs/LabN-<Name>/              מעבדה: README.md (הוראות) + Starter/ (פרויקט התחלה)
```

</div>

מתחילים מ-[`Day1-OOP-Collections-Exceptions/README.md`](Day1-OOP-Collections-Exceptions/README.md).

## איך עובדים

- **Notes** הם "הספר" של הקורס. קראו אותם אחרי כל מודול, ובמיוחד את סעיף "טעויות נפוצות" בסוף כל קובץ.
- **Demos** הם הקוד שמוצג בשיעור. אפשר להריץ ולשנות: `cd Demos/<Project>` ואז `dotnet run`.
- **Labs**: העתיקו את תיקיית `Starter` לתיקייה משלכם ועבדו על העותק. הוראות מפורטות ב-README של כל מעבדה.

</div>
